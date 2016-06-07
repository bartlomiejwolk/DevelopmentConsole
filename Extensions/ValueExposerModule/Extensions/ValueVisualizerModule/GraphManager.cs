using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    
    public class GraphManager {
        
        private readonly List<GraphInfo> _valueGraphs = new List<GraphInfo>();

        public List<GraphInfo> ValueGraphs {
            get { return _valueGraphs; }
        }

        public GraphInfo AddGraph(string valueName, Func<object> valueDelegate,
            GameObject go) {

            var valueGraph = new GraphInfo() {
                ValueName = valueName,
                ValueDelegate = valueDelegate,
                Go = go
            };
            _valueGraphs.Add(valueGraph);
            return valueGraph;
        }
    }
}