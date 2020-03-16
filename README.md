# Safe-getting-started-dotnet

This repository contains .Net Framework and Xamarin.Forms (Android, iOS) examples showcasing the usage of the [SafeApp](https://www.nuget.org/packages/MaidSafe.SafeApp) package.

|SafeTodo Mobile Example| Console Desktop Example |
|:-:|:-:|
|[![Build Status](https://dev.azure.com/maidsafe/DevHub%20Example/_apis/build/status/DevHub%20Example-Mobile-CI)](https://dev.azure.com/maidsafe/DevHub%20Example/_build/latest?definitionId=6)|[![Build Status](https://dev.azure.com/maidsafe/DevHub%20Example/_apis/build/status/DevHub%20Example-.NET%20Desktop-CI)](https://dev.azure.com/maidsafe/DevHub%20Example/_build/latest?definitionId=5)|

## Desktop example (.Net Framework)

### Features

Demonstrates:
 - Usage of the [MaidSafe.SafeApp](https://www.nuget.org/packages/MaidSafe.SafeApp) NuGet package to build a desktop app for the SAFE Network
 - Modes of Authentication
     - Mock authentication using the mock Authenticator API
     - Mock authentication using the [mock-safe-browser](https://github.com/maidsafe/safe_browser/releases/latest)
     - Test net authentication using the [safe-browser](https://github.com/maidsafe/safe_browser/releases/latest)
 - Mutable Data
     - CRUD operations on mutable data

### Prerequisites

- Visual Studio Windows desktop development workload

### Supported Platforms

- Windows (x64)

## Mobile example (Xamarin.Forms)

### Features

Demonstrates:
 - Usage of the [MaidSafe.SafeApp](https://www.nuget.org/packages/MaidSafe.SafeApp) NuGet package to build a mobile (Android, iOS) application for the SAFE network
 - Modes of Authentication
     - Mock authentication using the mock Authenticator API
     - Authentication using the [SAFE Authenticator](https://github.com/maidsafe/safe-authenticator-mobile)
 - Mock & non-mock versions
    - Developer can switch between mock and non mock library for testing and deployment
 - Mutable data
    - CRUD operations on mutable data
 - Usage of app's container
     - Store mutable data information in an app's default container. This can be used to retain and retrieve data used in the application

### Prerequisites
The [SAFE Authenticator mobile](https://github.com/maidsafe/safe-authenticator-mobile) application is required for authentication to the SAFE Network.

On Visual Studio 2017, you will need the following SDKs and workloads installed:
- Xamarin

### Required SDK/Tools
- Android 9.0 SDK
- Xcode 10+

### Supported Platforms
- Android 4.4+ (armeabi-v7, x86_64)
- iOS 8+ (ARM64, x64)

## Further Help

Get your developer related questions clarified on the [SAFE Dev Forum](https://forum.safedev.org/). If you're looking to share any other ideas or thoughts on the SAFE Network you can reach out on the [SAFE Network Forum](https://safenetforum.org/).

## Contributing

Want to contribute? Great :tada:

There are many ways to give back to the project, whether it be writing new code, fixing bugs, or just reporting errors. All forms of contributions are encouraged!

For instructions on how to contribute, see our [Guide to contributing](https://github.com/maidsafe/QA/blob/master/CONTRIBUTING.md).

## License

This SAFE Network library is dual-licensed under the Modified BSD ([LICENSE-BSD](LICENSE-BSD) https://opensource.org/licenses/BSD-3-Clause) or the MIT license ([LICENSE-MIT](LICENSE-MIT) https://opensource.org/licenses/MIT) at your option.
