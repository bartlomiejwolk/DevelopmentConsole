using System;
using UnityEngine;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    public class GraphInfo : IGraphInfo {
        public string ValueName { get; set; }
        public Func<object> ValueDelegate { get; set; }
        public GameObject Go { get; set; }
        public bool Enabled { get; private set; }
        public Vector3 Position { get; private set; }

        public GraphPlotter GraphPlotter {
            get {
                if (_graphPlotter == null) {
                    _graphPlotter = Go.GetComponent<GraphPlotter>();
                }
                return _graphPlotter;
            }
        }

        private GraphPlotter _graphPlotter;

        // todo assign value
        private RectTransform _rectTransform;

        public RectTransform RectTransform {
            get {
                if (_rectTransform != null) {
                    return _rectTransform;
                }
                var rectTransform = Go.GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        public Vector2 Size { get; private set; }

        public IGraphInfo SetPosition(Vector3 position) {
            Position = position;
            return this;
        }

        public IGraphInfo SetSize(Vector2 size) {
            Size = size;
            return this;
        }

        public IGraphInfo Enable(bool state) {
            Enabled = state;
            return this;
        }
    }

    public interface IGraphInfo {
        IGraphInfo SetPosition(Vector3 position);
        IGraphInfo SetSize(Vector2 size);
        IGraphInfo Enable(bool state);
    }
}