using System;

namespace App
{
    [Serializable]
    public class NamedPipePayload
    {
        public bool SignalQuit { get; set; }

        public string Arguments { get; set; }
    }
}
