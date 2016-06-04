using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace DevelopmentConsoleTool.ExposeValueExtension {

	// todo extract to file
	// todo move Unity API to ExposeValue class
    public class ExposedValue {
		// todo change to properties
	    private bool _updateEnabled;
	    public bool UpdateEnabled {
		    get { return _updateEnabled; }
		    set {
			    _updateEnabled = true;
			    if (Go != null) {
				    Go.SetActive(value);
			    }
		    }
	    }

        public Func<object> Callback; 
        public string Category;
	    public GameObject Go;

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
			_valuesSources.TryGetValue(valueName, out exposedValue);
			return exposedValue;
		}
    }
}