using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace DevelopmentConsoleTool.ExposeValueExtension {

    public class ExposedValue {
	    public bool Enabled;
        public Func<object> Callback; 
        public string Category;
	    public Text TextComponent;

	    public string ValueString {
		    get {
			    var value = Callback();
			    var result = value.ToString();
			    return result;
		    }
	    }
    }

    public class ExposedValuesManager {

        private static readonly ExposedValuesManager _instance
            = new ExposedValuesManager();

        private readonly Dictionary<string, ExposedValue> _valuesSources
            = new Dictionary<string, ExposedValue>();

        public static ExposedValuesManager Instance {
            get { return _instance; }
        }

	    public Dictionary<string, ExposedValue> ValuesSources {
		    get { return _valuesSources; }
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

	    public ExposedValue GetExposedValue(string valueName) {
			ExposedValue exposedValue;
			_valuesSources.TryGetValue(valueName, out exposedValue);
			return exposedValue;
		}
    }
}