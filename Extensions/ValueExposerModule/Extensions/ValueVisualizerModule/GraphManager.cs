using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    
    public class GraphManager {
        
        private readonly List<GraphInfo> _valueGraphs = new List<GraphInfo>();

        public List<GraphInfo> ValueGraphs {
            get { return _valueGraphs; }
        }

        public void AddGraph(GraphInfo info) {
            _valueGraphs.Add(info);
        }
    }
}