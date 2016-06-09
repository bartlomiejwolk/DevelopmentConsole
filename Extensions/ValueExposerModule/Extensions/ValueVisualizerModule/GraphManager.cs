using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    
    public class GraphManager {
        
        // todo use dictionary instead <valueName, GraphInfo>. Remove value name from GraphInfo
        private readonly List<GraphInfo> _valueGraphs = new List<GraphInfo>();

        // todo rename to GraphInfos
        public List<GraphInfo> ValueGraphs {
            get { return _valueGraphs; }
        }

        public void AddGraph(GraphInfo info) {
            _valueGraphs.Add(info);
        }

        public GraphInfo GetGraphByName(string valueName) {
            foreach (var valueGraph in _valueGraphs) {
                if (valueGraph.ValueName == valueName) {
                    return valueGraph;
                }
            }
            return null;
        }

        public void EnableGraph(string valueName) {
            foreach (var valueGraph in _valueGraphs)
            {
                if (valueGraph.ValueName == valueName)
                {
                    valueGraph.Enabled = true;
                }
            }
        }
    }
}