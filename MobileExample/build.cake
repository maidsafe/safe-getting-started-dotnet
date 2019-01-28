#addin nuget:?package=Cake.AppleSimulator&Version=0.1.0
#addin nuget:?package=Cake.Android.Adb&version=2.0.6
#addin nuget:?package=Cake.Android.AvdManager&version=1.0.3
#addin nuget:?package=Cake.FileHelpers
using System.Net;
using System.Net.Sockets;

var TARGET = Argument("target", "Default");

var IOS_SIM_NAME = EnvironmentVariable("IOS_SIM_NAME") ?? "iPhone X";
var IOS_SIM_RUNTIME = EnvironmentVariable("IOS_SIM_RUNTIME") ?? "iOS 12.0";
var IOS_PROJ = "./Tests/SafeTodoExample.Tests.iOS/SafeTodoExample.Tests.iOS.csproj";
var IOS_BUNDLE_ID = "net.maidsafe.SafetodoExampleTests";
var IOS_IPA_PATH = "./Tests/SafeTodoExample.Tests.iOS/bin/iPhoneSimulator/Release/NunitTests.app";
var IOS_TEST_RESULTS_PATH = "./ios-test.xml";
var IOS_TCP_LISTEN_HOST = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

var ANDROID_HOME = EnvironmentVariable("ANDROID_HOME");
var ANDROID_PROJ = "./Tests/SafetodoExample.Tests.Android/SafetodoExample.Tests.Android.csproj";
var ANDROID_TEST_RESULTS_PATH = "./android-test.xml";
var ANDROID_AVD = "SafeAppEmulator";
var ANDROID_PKG_NAME = "net.maidsafe.SafetodoExampleTests";
var ANDROID_EMU_TARGET = EnvironmentVariable("ANDROID_EMU_TARGET") ?? "system-images;android-28;google_apis;x86_64";
var ANDROID_EMU_DEVICE = EnvironmentVariable("ANDROID_EMU_DEVICE") ?? "Nexus 5X";
var ANDROID_TCP_LISTEN_HOST = System.Net.IPAddress.Any;

var TCP_LISTEN_PORT = 10500;

Func<IPAddress, int, string, Task> DownloadTcpTextAsync = (IPAddress TCP_LISTEN_HOST, int TCP_LISTEN_PORT, string RESULTS_PATH) => System.Threading.Tasks.Task.Run(() =>
{
    TcpListener server = null;
    try
    {
        server = new TcpListener(TCP_LISTEN_HOST, TCP_LISTEN_PORT);
        server.Start();
        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            StreamReader data_in = new StreamReader(client.GetStream());
            var result = data_in.ReadToEnd();
            System.IO.File.AppendAllText(RESULTS_PATH, result);
            client.Close();
            break;
        }
    }
    catch (SocketException e)
    {
        Information("SocketException: {0}", e);
    }
    finally
    {
        server.Stop();
    }
});

Task ("Build-iOS")
    .Does (() =>
{
    // Nuget restore
    MSBuild (IOS_PROJ, c => {
        c.Configuration = "Release";
        c.Targets.Clear();
        c.Targets.Add("Restore");
    });

    // Build the project (with ipa)
    MSBuild (IOS_PROJ, c => {
        c.Configuration = "Release";
        c.Properties["Platform"] = new List<string> { "iPhoneSimulator" };
        c.Properties["BuildIpa"] = new List<string> { "true" };
        c.Properties["ContinuousIntegrationBuild"] = new List<string> { "false" };
        c.Targets.Clear();
        c.Targets.Add("Rebuild");
    });
});

Task ("Run-iOS-Tests")
    .IsDependentOn ("Build-iOS")
    .Does (() =>
{
    // Look for a matching simulator on the system
    var sim = ListAppleSimulators ()
        .FirstOrDefault (s => (s.Availability.Contains("available") || s.Availability.Contains("booted"))
                && !s.Availability.Contains("unavailable")
                && s.Name == IOS_SIM_NAME && s.Runtime == IOS_SIM_RUNTIME);

    // Boot the simulator
    Information("Booting: {0} ({1} - {2})", sim.Name, sim.Runtime, sim.UDID);
    if (!sim.State.ToLower().Contains ("booted"))
        BootAppleSimulator (sim.UDID);

    // Wait for it to be booted
    var booted = false;
    for (int i = 0; i < 100; i++) {
        if (ListAppleSimulators().Any (s => s.UDID == sim.UDID && s.State.ToLower().Contains("booted"))) {
            booted = true;
            break;
        }
        System.Threading.Thread.Sleep(1000);
    }

    // Install the IPA that was previously built
    var ipaPath = new FilePath(IOS_IPA_PATH);
    Information ("Installing: {0}", ipaPath);
    InstalliOSApplication(sim.UDID, MakeAbsolute(ipaPath).FullPath);

    // Start our Test Results TCP listener
    Information("Started TCP Test Results Listener on port: {0}", TCP_LISTEN_PORT);
    var tcpListenerTask = DownloadTcpTextAsync(IOS_TCP_LISTEN_HOST, TCP_LISTEN_PORT, IOS_TEST_RESULTS_PATH);

    // Launch the IPA
    Information("Launching: {0}", IOS_BUNDLE_ID);
    LaunchiOSApplication(sim.UDID, IOS_BUNDLE_ID);

    // Wait for the TCP listener to get results
    Information("Waiting for tests...");
    tcpListenerTask.Wait ();

    // Close up simulators
    Information("Closing Simulator");
    ShutdownAllAppleSimulators ();
})
.ReportError(exception =>
{  
   Information(exception.Message); 
});


Task ("Build-Android")
    .Does (() =>
{
    // Nuget restore
    MSBuild (ANDROID_PROJ, c => {
        c.Configuration = "Debug";
        c.Targets.Clear();
        c.Targets.Add("Restore");
    });

    // Build the app in debug mode
    // needs to be debug so unit tests get discovered
    MSBuild (ANDROID_PROJ, c => {
        c.Configuration = "Debug";
        c.Properties["ContinuousIntegrationBuild"]  = new List<string> { "false" };
        c.Targets.Clear();
        c.Targets.Add("Rebuild");
    });
});

Task ("Run-Android-Tests")
    .IsDependentOn ("Build-Android")
    .Does (() =>
{
    if (EnvironmentVariable("ANDROID_SKIP_AVD_CREATE") == null) {
        var avdSettings = new AndroidAvdManagerToolSettings  { SdkRoot = ANDROID_HOME };

        // Create the AVD if necessary
        Information ("Creating AVD if necessary: {0}...", ANDROID_AVD);
        if (!AndroidAvdListAvds (avdSettings).Any (a => a.Name == ANDROID_AVD))
            AndroidAvdCreate (ANDROID_AVD, ANDROID_EMU_TARGET, ANDROID_EMU_DEVICE, force: true, settings: avdSettings);
    }

    // We need to find `emulator` and the best way is to try within a specified ANDROID_HOME
    var emulatorExt = IsRunningOnWindows() ? ".exe" : "";
    string emulatorPath = "emulator" + emulatorExt;

    if (ANDROID_HOME != null) {
        var andHome = new DirectoryPath(ANDROID_HOME);
        if (DirectoryExists(andHome)) {
            emulatorPath = MakeAbsolute(andHome.Combine("tools").CombineWithFilePath("emulator" + emulatorExt)).FullPath;
            if (!FileExists(emulatorPath))
                emulatorPath = MakeAbsolute(andHome.Combine("emulator").CombineWithFilePath("emulator" + emulatorExt)).FullPath;
            if (!FileExists(emulatorPath))
                emulatorPath = "emulator" + emulatorExt;
        }
    }

    // Start up the emulator by name
    Information ("Starting Emulator: {0}...", ANDROID_AVD);
    var emu = StartAndReturnProcess (emulatorPath, new ProcessSettings { 
        Arguments = $"-avd {ANDROID_AVD}  -gpu auto -noaudio" });

    var adbSettings = new AdbToolSettings { SdkRoot = ANDROID_HOME };

    // Keep checking adb for an emulator with an AVD name matching the one we just started
    var emuSerial = string.Empty;
    for (int i = 0; i < 100; i++) {
        foreach (var device in AdbDevices(adbSettings).Where(d => d.Serial.StartsWith("emulator-"))) {
            if (AdbGetAvdName(device.Serial).Equals(ANDROID_AVD, StringComparison.OrdinalIgnoreCase)) {
                emuSerial = device.Serial;
                break;
            }
        }

        if (!string.IsNullOrEmpty(emuSerial))
            break;
        else
            System.Threading.Thread.Sleep(1000);
    }

    Information ("Matched ADB Serial: {0}", emuSerial);
    adbSettings = new AdbToolSettings { SdkRoot = ANDROID_HOME, Serial = emuSerial };

    // Wait for the emulator to enter a 'booted' state
    AdbWaitForEmulatorToBoot(TimeSpan.FromSeconds(100), adbSettings);
    Information ("Emulator finished booting.");

    // Try uninstalling the existing package (if installed)
    try { 
        AdbUninstall (ANDROID_PKG_NAME, false, adbSettings);
        Information ("Uninstalled old: {0}", ANDROID_PKG_NAME);
    } catch { }

    // Use the Install target to push the app onto emulator
    MSBuild (ANDROID_PROJ, c => {
        c.Configuration = "Debug";
        c.Properties["ContinuousIntegrationBuild"] = new List<string> { "false" };
        c.Properties["AdbTarget"] = new List<string> { "-s " + emuSerial };
        c.Targets.Clear();
        c.Targets.Add("Install");
    });

    // Start the TCP Test results listener
    Information("Started TCP Test Results Listener on port: {0}", TCP_LISTEN_PORT);
    var tcpListenerTask = DownloadTcpTextAsync (ANDROID_TCP_LISTEN_HOST, TCP_LISTEN_PORT, ANDROID_TEST_RESULTS_PATH);

    // Launch the app on the emulator
    AdbShell ($"am start -n {ANDROID_PKG_NAME}/{ANDROID_PKG_NAME}.MainActivity", adbSettings);

    // Wait for the test results to come back
    Information("Waiting for tests...");
    tcpListenerTask.Wait ();

    // Close emulator
    emu.Kill();
})
.ReportError(exception =>
{  
   Information(exception.Message); 
});


Task("Default")
		.IsDependentOn ("Run-Android-Tests")
    .IsDependentOn ("Run-iOS-Tests")
    .Does(() => { });

RunTarget(TARGET);
