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
        // todo merge with _dots in one object
        private readonly List<float> _values = new List<float>();
        private int _maxDots;

        private RectTransform DotTransform {
            get {
                var rectTransform = _dotTemplate.GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        private float DotWidth {
            get { return DotTransform.rect.width; }
        }

        private RectTransform _rectTransform;

        private RectTransform RectTransform {
            get {
                if (_rectTransform == null) {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }

        public int MaxDots {
            get {
                if (_maxDots > 0) {
                    return _maxDots;
                }
                var containerWidth = RectTransform.rect.width;
                var maxDots = Mathf.FloorToInt(containerWidth/DotWidth);
                return maxDots;
            }
        }

        private void Awake() {
            Assert.IsNotNull(_dotTemplate);
            DotInstantiated += OnDotInstantiated;
        }

        private void OnDotInstantiated(object sender,
            DotInstantiatedEventArgs eventArgs) {
            _dots.Add(eventArgs.RectTransform);
            ApplyDotOffsets(eventArgs.RectTransform);
            HandleRemovingDots();
        }

        // applies horizontal and vertical offset
        private void ApplyDotOffsets(RectTransform rectTransform) {
            var vertOffset = CalculateVerticalOffset();
            // position dot inside the parent
            var x = rectTransform.anchoredPosition.x - DotWidth/2;
            // offset corresponding to the value being represented on the graph
            var y = rectTransform.anchoredPosition.y + vertOffset;
            rectTransform.anchoredPosition = new Vector2(x, y);
            //Debug.Log(rectTransform.anchoredPosition);
        }

        private void HandleRemovingDots() {
            if (_values.Count > MaxDots) {
                var dot = _dots[0];
                Destroy(dot.gameObject);
                _dots.RemoveAt(0);
                _values.RemoveAt(0);
            }
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
            var valuePercentage = normalizedValue/normalizedMax;

            // calculate offset
            var halfDotHeight = DotWidth/2;
            float resultOffset;
            // value is min. value
            if (Math.Abs(value - minValue) < float.Epsilon) {
                resultOffset = 0;
                // offset to stay within the parent rect
                resultOffset += halfDotHeight;
                return resultOffset;
            }
            // value is max. value
            if (Math.Abs(value - maxValue) < float.Epsilon) {
                resultOffset = containerHeight;
                // offset to stay within the parent rect
                resultOffset -= halfDotHeight;
                return resultOffset;
            }
            // value is between max. and min.
            resultOffset = containerHeight*valuePercentage;

            // offset to stay within the parent rect
            var maxVertOffset = containerHeight - halfDotHeight;
            if (resultOffset > maxVertOffset) {
                resultOffset = maxVertOffset;
            }
            else if (resultOffset < halfDotHeight) {
                resultOffset = halfDotHeight;
            }

            return resultOffset;
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