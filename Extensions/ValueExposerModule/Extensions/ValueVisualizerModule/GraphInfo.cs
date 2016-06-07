using System;
using UnityEngine;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    public class GraphInfo {
        public string ValueName { get; set; }
        public Func<object> ValueDelegate { get; set; } 
        public GameObject Go { get; set; }
        public bool Enabled { get; set; }

        public GraphDrawer GraphDrawer {
            get {
                if (_graphDrawer == null && Go != null) {
                    _graphDrawer = Go.GetComponent<GraphDrawer>();
                }
                return _graphDrawer;
            }
        }

        private GraphDrawer _graphDrawer;

        public void DrawValuePoint() {
            var value = ValueDelegate();
            GraphDrawer.DrawValuePoint(value);
        }
    }
}