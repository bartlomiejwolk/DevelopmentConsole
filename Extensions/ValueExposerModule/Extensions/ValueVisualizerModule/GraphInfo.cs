using System;
using UnityEngine;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    public class GraphInfo {
        public string ValueName { get; set; }
        public Func<object> ValueDelegate { get; set; } 
        public GameObject Go { get; set; }
        public bool Enabled { get; set; }
        public Vector3 Position { get; set; }

        public GraphPlotter GraphPlotter {
            get {
                if (_graphPlotter == null) {
                    _graphPlotter = Go.GetComponent<GraphPlotter>();
                }
                return _graphPlotter;
            }
        }

        private GraphPlotter _graphPlotter;
    }
}