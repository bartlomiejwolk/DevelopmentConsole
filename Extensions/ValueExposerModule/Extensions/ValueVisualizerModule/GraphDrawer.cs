using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    public class GraphDrawer : MonoBehaviour {

        [SerializeField]
        private Image _dotSprite;

        [SerializeField]
        private RectTransform _dotInstantiatePoint;

        private readonly List<RectTransform> _dots = new List<RectTransform>();
        private event EventHandler<DotInstantiatedEventArgs> DotInstantiated;

        private void Awake() {
            Assert.IsNotNull(_dotSprite);

            DotInstantiated += OnDotInstantiated;
        }

        private void OnDotInstantiated(object sender, DotInstantiatedEventArgs eventArgs) {
            _dots.Add(eventArgs.RectTransform);
        }

        public void DrawValuePoint(object value) {
            var number = (float) value;
            InstantiateDot();
            OffsetDotsLeft();
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
            var dotGo = Instantiate(_dotSprite);
            dotGo.transform.SetParent(_dotInstantiatePoint);
            dotGo.rectTransform.anchoredPosition = _dotInstantiatePoint.anchoredPosition;

            var args = new DotInstantiatedEventArgs() {
                RectTransform = dotGo.rectTransform
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
