using System;
using UnityEngine;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    public class GraphDrawerInstantiatedEventArgs : EventArgs {
        public string ValueName;
        public GameObject Go;
    }
}