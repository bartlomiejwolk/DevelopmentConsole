using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace DevelopmentConsoleTool.ExposeValueExtension {

    public struct ValueSource {
        public Func<object> Callback; 
        public string Category;
    }

    // todo rename to noun
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
            Func<object> value) {
            
            var valueSource = new ValueSource() {
                Callback = value,
                Category = category
            };
            _valuesSources.Add(customName, valueSource);
        }

        public void ShowValue(string valueName) {
            ValueSource valueSource;
            _valuesSources.TryGetValue(valueName, out valueSource);

            var callback = valueSource.Callback;
            if (callback != null) {
                Debug.Log(callback().ToString());
            }
        }

        public void HideValue(string valueName) {
            
        }
    }
}