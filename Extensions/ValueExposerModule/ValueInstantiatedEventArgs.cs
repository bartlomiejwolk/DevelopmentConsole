using System;
using UnityEngine;

namespace DevelopmentConsole.Extensions.ValueExposerModule {
    public class ValueInstantiatedEventArgs : EventArgs {
        public string ValueName { get; set; }
        public GameObject Go { get; set; }
    }
}