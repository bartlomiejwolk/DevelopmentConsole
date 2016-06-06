using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    
    public class GraphManager {
        
        private readonly List<ValueGraph> _valueGraphs = new List<ValueGraph>();

        public List<ValueGraph> ValueGraphs {
            get { return _valueGraphs; }
        }

        public ValueGraph AddGraph(string valueName, Func<object> valueDelegate,
            GameObject go) {

            var valueGraph = new ValueGraph() {
                ValueName = valueName,
                ValueDelegate = valueDelegate,
                Go = go
            };
            _valueGraphs.Add(valueGraph);
            return valueGraph;
        }
    }

    public class ValueGraph {
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