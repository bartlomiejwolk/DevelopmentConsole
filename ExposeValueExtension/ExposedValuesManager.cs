using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace DevelopmentConsoleTool.ExposeValueExtension {

    public struct ExposedValue {
	    public bool Enabled;
        public Func<object> Callback; 
        public string Category;
    }

    public class ExposedValuesManager {

        private static readonly ExposedValuesManager _instance
            = new ExposedValuesManager();

        private readonly Dictionary<string, ExposedValue> _valuesSources
            = new Dictionary<string, ExposedValue>();

        public static ExposedValuesManager Instance {
            get { return _instance; }
        }

        private ExposedValuesManager() { }

        public void RegisterValue(
            string customName,
            string category,
            Func<object> value) {
            
            var valueSource = new ExposedValue() {
                Callback = value,
                Category = category
            };
            _valuesSources.Add(customName, valueSource);
        }

        public Func<object> GetSourceCallback(string sourceName) {
            ExposedValue exposedValue;
            _valuesSources.TryGetValue(sourceName, out exposedValue);
            var callback = exposedValue.Callback;
            return callback;
        }

        public object GetSourceValue(string sourceName) {
            var callback = GetSourceCallback(sourceName);
            object value = null;
            if (callback != null) {
                value = callback();
            }
            return value;
        }
    }
}