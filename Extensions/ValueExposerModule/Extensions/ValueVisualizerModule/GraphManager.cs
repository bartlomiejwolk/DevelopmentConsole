using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    
    public class GraphManager {
        
        // todo use hash instead of string
        private readonly Dictionary<string, GraphInfo> _valueGraphs = new Dictionary<string, GraphInfo>();

        // todo rename to GraphInfos
        public List<GraphInfo> ValueGraphs {
            get {
                var graphs = _valueGraphs.Values.ToList();
                return graphs;
            }
        }

        public void AddGraph(GraphInfo info) {
            var name = info.ValueName;
            GraphInfo val;
            _valueGraphs.TryGetValue(name, out val);
            if (val != null) {
                _valueGraphs[name] = info;
            }
            else {
                _valueGraphs.Add(name, info);
            }
        }

        public GraphInfo GetGraphByName(string valueName) {
            GraphInfo val;
            _valueGraphs.TryGetValue(valueName, out val);
            return val;
        }

        public void EnableGraph(string valueName) {
            GraphInfo val;
            _valueGraphs.TryGetValue(valueName, out val);
            if (val != null) {
                val.Enabled = true;
            }
        }

        public static void AddOrUpdate(Dictionary<int, int> dic, int key, int newValue) {
            int val;
            if (dic.TryGetValue(key, out val)) {
                dic[key] = val + newValue;
            }
            else {
                dic.Add(key, newValue);
            }
        }
    }
}