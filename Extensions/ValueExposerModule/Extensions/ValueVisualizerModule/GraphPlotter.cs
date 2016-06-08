using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

#pragma warning disable 649

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    public class GraphPlotter : MonoBehaviour {

        [SerializeField]
        private GameObject _dotTemplate;

        private readonly List<RectTransform> _dots = new List<RectTransform>();
        private event EventHandler<DotInstantiatedEventArgs> DotInstantiated;
        private readonly List<float> _values = new List<float>(); 

        private RectTransform DotTransform {
            get {
                var rectTransform = _dotTemplate.GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        private float DotWidth {
            get { return DotTransform.rect.width; }
        }

        private void Awake() {
            Assert.IsNotNull(_dotTemplate);
            DotInstantiated += OnDotInstantiated;
        }

        private void OnDotInstantiated(object sender,
            DotInstantiatedEventArgs eventArgs) {
            
            _dots.Add(eventArgs.RectTransform);
            ApplyOffsets(eventArgs.RectTransform);
            HandleRemovingDots();
        }

        // applies horizontal and vertical offset
        private void ApplyOffsets(RectTransform rectTransform) {
            var vertOffset = CalculateVerticalOffset();
            // position dot inside the parent
            var x = rectTransform.anchoredPosition.x - DotWidth/2;
            // offset corresponding to the value being represented on the graph
            var y = rectTransform.anchoredPosition.y + vertOffset;
            rectTransform.anchoredPosition = new Vector2(x, y);
        }

        private void HandleRemovingDots() {

        }

        public void DrawFloatValuePoint(float value) {
            _values.Add(value);
            OffsetDotsLeft();
            InstantiateDot();
        }

        public void DrawBoolPoint(bool value) {
            throw new NotImplementedException();
        }

        private float CalculateVerticalOffset() {
            // container height
            var rectTransform = GetComponent<RectTransform>();
            var containerHeight = rectTransform.rect.height;

            // value position offset as percentage of the container height
            var value = _values.Last();
            var maxValue = _values.Max();
            var minValue = _values.Min();
            var normalizedMax = maxValue - minValue;
            var normalizedValue = value - minValue;
            var valuePercentage = normalizedValue / normalizedMax;

            // todo create Epsilon const
            // value is min. value
            if (Math.Abs(value - minValue) < 0.001) {
                return 0;
            }
            // value is max. value
            if (Math.Abs(value - maxValue) < 0.001) {
                return containerHeight;
            }
            // value is between max. and min.
            var offset = containerHeight*valuePercentage;
            return offset;
        }

        private void OffsetDotsLeft() {
            foreach (var dot in _dots) {
                var x = dot.anchoredPosition.x - DotWidth;
                var y = dot.anchoredPosition.y;
                dot.anchoredPosition = new Vector2(x, y);
            }
        }

        private void InstantiateDot() {
            var dotGo = Instantiate(_dotTemplate);
            dotGo.transform.SetParent(transform, false);
            
            // fire event
            var rectTrans = dotGo.GetComponent<RectTransform>();
            var args = new DotInstantiatedEventArgs() {
                RectTransform = rectTrans
            };
            InvokeDotInstantiatedEvent(args);
        }

        protected virtual void InvokeDotInstantiatedEvent(
            DotInstantiatedEventArgs args) {

            var handler = DotInstantiated;
            if (handler != null) handler(this, args);
        }
    }

    public class DotInstantiatedEventArgs : EventArgs {
        public RectTransform RectTransform { get; set; }
    }
}
