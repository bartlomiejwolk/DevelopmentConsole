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
    }
}