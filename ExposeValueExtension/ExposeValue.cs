using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace DevelopmentConsoleTool.ExposeValueExtension {

    public struct ValueSource {
        public object Value;
        public Func<string> StringDelegate;
        public Func<int> IntDelegate;
        public Func<float> FloatDelegate;
        public string Category;
    }

    public class ExposeValue {

        private static readonly ExposeValue _instance
            = new ExposeValue();

        private readonly Dictionary<string, ValueSource> _valuesSources
            = new Dictionary<string, ValueSource>();

        public static ExposeValue Instance {
            get { return _instance; }
        }

        private ExposeValue() { }

        public void RegisterValue(
            string customName,
            string category,
            object value) {
            
            var valueSource = new ValueSource() {
                Value = value,
                Category = category
            };
            _valuesSources.Add(customName, valueSource);
        }

        public void ShowValue(string valueName) {
            Debug.Log("ShowValue()");
        }

        public void HideValue(string valueName) {
            
        }
    }
}