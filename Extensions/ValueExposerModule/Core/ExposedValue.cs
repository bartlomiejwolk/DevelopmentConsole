using System;
using UnityEngine;
using UnityEngine.UI;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Core {
    public class ExposedValue {
        private bool _updateEnabled;

        public bool UpdateEnabled {
            get { return _updateEnabled; }
            set {
                _updateEnabled = value;
                if (Go != null) {
                    Go.SetActive(value);
                }
            }
        }

        public Func<object> Callback { get; set; }
        public string Category { get; set; }
        public GameObject Go { get; set; }
        private RectTransform _rectTransform;

        public RectTransform RectTransform {
            get {
                if (_rectTransform == null) {
                    _rectTransform = Go.GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }

        public Text TextComponent {
            get {
                if (Go == null) {
                    return null;
                }
                var textCo = Go.GetComponentInChildren<Text>();
                return textCo;
            }
        }

        public string ValueString {
            get {
                var value = Callback();
                var result = value.ToString();
                return result;
            }
        }

        public Vector3 PivotPosition {
            get {
                var pos = Go.transform.position;
                return pos;
            }
        }

        public Vector3 BottomRightCornerPosition {
            get {
                Canvas.ForceUpdateCanvases();
                var corners = new Vector3[4];
                RectTransform.GetWorldCorners(corners);
                return corners[3];
            }
        }

        public Vector2 Size {
            get {
                var rect = RectTransform.rect;
                var size = new Vector2(rect.width, rect.height);
                return size;
            }
        }
    }
}