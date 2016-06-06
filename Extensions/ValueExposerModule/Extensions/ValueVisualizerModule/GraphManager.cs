using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    
    public class GraphManager {
        
        private readonly List<ValueGraph> _valueGraphs = new List<ValueGraph>(); 

        public void AddGraph(string valueName, Func<object> valueDelegate,
            GameObject go) {

            var valueGraph = new ValueGraph() {
                ValueName = valueName,
                ValueDelegate = valueDelegate,
                Go = go
            };
            _valueGraphs.Add(valueGraph);
        }
    }

    public class ValueGraph {
        public string ValueName { get; set; }
        public Func<object> ValueDelegate { get; set; } 
        public GameObject Go { get; set; }
    }
}