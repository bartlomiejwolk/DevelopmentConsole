using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

#pragma warning disable 649

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    public class GraphPlotter : MonoBehaviour {

        [SerializeField]
        private GameObject _dotTemplate;

        private readonly List<RectTransform> _dots = new List<RectTransform>();
        private event EventHandler<DotInstantiatedEventArgs> DotInstantiated;

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
            RemoveOldDots();
        }

        private void ApplyOffsets(RectTransform rectTransform) {
            // apply horizontal offset

            // apply vertical offset
            var vertOffset = CalculateVerticalOffset();
            var rectTrans = rectTransform;
            var x = rectTrans.anchoredPosition.x;
            var y = rectTrans.anchoredPosition.y + vertOffset;
            rectTrans.anchoredPosition = new Vector2(x, y);
        }

        private void RemoveOldDots() {
        }

        public void DrawFloatValuePoint(float value) {
            OffsetDotsLeft();
            InstantiateDot();
        }

        public void DrawBoolPoint(bool value) {
            throw new NotImplementedException();
        }

        private float CalculateVerticalOffset() {
            return 0;
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
