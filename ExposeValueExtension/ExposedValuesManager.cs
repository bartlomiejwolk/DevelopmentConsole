using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace DevelopmentConsoleTool.ExposeValueExtension {

    public struct ValueSource {
        public Func<object> Callback; 
        public string Category;
    }

    public class ExposedValuesManager {

        private static readonly ExposedValuesManager _instance
            = new ExposedValuesManager();

        private readonly Dictionary<string, ValueSource> _valuesSources
            = new Dictionary<string, ValueSource>();

        public static ExposedValuesManager Instance {
            get { return _instance; }
        }

        private ExposedValuesManager() { }

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

        public Func<object> GetSourceCallback(string sourceName) {
            ValueSource valueSource;
            _valuesSources.TryGetValue(sourceName, out valueSource);
            var callback = valueSource.Callback;
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