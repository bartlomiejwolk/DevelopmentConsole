using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace DevelopmentConsoleTool.ValueExposerExtension {

	public class ExposedValueManager {

        private static readonly ExposedValueManager _instance
            = new ExposedValueManager();

        private readonly Dictionary<string, ExposedValue> _exposedValues
            = new Dictionary<string, ExposedValue>();

        public static ExposedValueManager Instance {
            get { return _instance; }
        }

	    public Dictionary<string, ExposedValue> ExposedValues {
		    get { return _exposedValues; }
	    }

	    private ExposedValueManager() { }

        public void RegisterValue(
            string customName,
            string category,
            Func<object> value) {
            
            var valueSource = new ExposedValue() {
                Callback = value,
                Category = category
            };
            _exposedValues.Add(customName, valueSource);
        }

        public Func<object> GetSourceCallback(string sourceName) {
            ExposedValue exposedValue;
            _exposedValues.TryGetValue(sourceName, out exposedValue);
	        if (exposedValue == null || exposedValue.Callback == null) {
		        return null;
	        }
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

	    public ExposedValue GetExposedValue(string valueName) {
			ExposedValue exposedValue;
			_exposedValues.TryGetValue(valueName, out exposedValue);
			return exposedValue;
		}
    }
}