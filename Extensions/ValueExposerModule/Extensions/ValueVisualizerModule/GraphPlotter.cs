using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    public class GraphPlotter : MonoBehaviour {

        [SerializeField]
        private GameObject _dotTemplate;

        [SerializeField]
        private RectTransform _dotInstantiatePoint;

        private readonly List<RectTransform> _dots = new List<RectTransform>();
        private event EventHandler<DotInstantiatedEventArgs> DotInstantiated;

        private void Awake() {
            Assert.IsNotNull(_dotTemplate);
            Assert.IsNotNull(_dotInstantiatePoint);

            DotInstantiated += OnDotInstantiated;
        }

        private void OnDotInstantiated(object sender, DotInstantiatedEventArgs eventArgs) {
            _dots.Add(eventArgs.RectTransform);

            // apply vertical offset
            var vertOffset = CalculateVerticalOffset();
            var rectTrans = eventArgs.RectTransform;
            var x = rectTrans.anchoredPosition.x;
            var y = rectTrans.anchoredPosition.y + vertOffset;
            rectTrans.anchoredPosition = new Vector2(x, y);

            RemoveOldDots();
        }

        private void RemoveOldDots() {
        }

        public void DrawValuePoint(object value) {
            var number = (float) value;
            OffsetDotsLeft();
            InstantiateDot();
        }

        private float CalculateVerticalOffset() {
            return 5;
        }

        private void OffsetDotsLeft() {
            foreach (var dot in _dots) {
                // todo get dot size from dot rect
                var x = dot.anchoredPosition.x - 4;
                var y = dot.anchoredPosition.y;
                dot.anchoredPosition = new Vector2(x, y);
            }
        }

        private void InstantiateDot() {
            var dotGo = Instantiate(_dotTemplate);
            dotGo.transform.SetParent(_dotInstantiatePoint);
            var rectTrans = dotGo.GetComponent<RectTransform>();
            rectTrans.anchoredPosition = _dotInstantiatePoint.anchoredPosition;

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
