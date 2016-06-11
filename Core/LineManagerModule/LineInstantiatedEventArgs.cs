using System;
using UnityEngine;

namespace DevelopmentConsole.Core.LineManagerModule {
    public class LineInstantiatedEventArgs : EventArgs {
        public GameObject InstantiatedGo { get; set; }
    }
}