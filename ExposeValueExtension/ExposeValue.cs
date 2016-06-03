using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace DevelopmentConsoleTool.ExposeValueExtension {

    public struct ValueSource {
        public Func<string> StringDelegate;
        public Func<int> IntDelegate;
        public Func<float> FloatDelegate;
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

        // todo create overloads for other return types
        public void RegisterValue(
            string customName,
            string category,
            Func<float> value) {
            
            var valueSource = new ValueSource() {
                FloatDelegate = value,
                Category = category
            };
            _valuesSources.Add(customName, valueSource);
        }

        // todo make it work for other return types
        public void ShowValue(string valueName) {
            ValueSource valueSource;
            _valuesSources.TryGetValue(valueName, out valueSource);

            var value = valueSource.FloatDelegate;
            if (value != null) {
                Debug.Log(value());
            }
        }

        public void HideValue(string valueName) {
            
        }
    }
}