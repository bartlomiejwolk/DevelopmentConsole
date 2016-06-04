using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
#pragma warning disable 649

namespace DevelopmentConsoleTool.ExposeValueExtension {
    public class ExposeValue : MonoBehaviour {

        [SerializeField]
        private GameObject _valuePrefab;

        [SerializeField]
        private Transform _container;

        private static ExposeValue _instance;
        private ExposedValuesManager _exposedValuesManager;

        public static ExposeValue Instance {
            get {
                if (_instance != null) {
                    return _instance;
                }
				Debug.LogWarning("Add ExposeValue prefab to the scene!");
	            return null;
            }
        }

        private void Awake() {
            _instance = this;
            _exposedValuesManager = ExposedValuesManager.Instance;

            Assert.IsNotNull(_valuePrefab);
            Assert.IsNotNull(_container);
        }

	    private void Update() {
		    UpdateValues();
	    }

	    private void UpdateValues() {
		    var values = _exposedValuesManager.ValuesSources;
		    foreach (var exposedValue in values) {
			    if (exposedValue.Value.Enabled) {
				    UpdateValue(exposedValue);
			    }
		    }
	    }

	    private void UpdateValue(KeyValuePair<string, ExposedValue> exposedValue) {
		    var textCo = exposedValue.Value.TextComponent;
		    if (textCo == null) {
			    return;
		    }
		    var valueString = exposedValue.Value.ValueString;
		    textCo.text = valueString;
	    }

	    public void ShowValue(string valueName) {
            var go = InstantiateValuePrefab();
			
			// todo move to OnValueInstantiated event handler
		    var exposedValue = _exposedValuesManager.GetExposedValue(valueName);
		    exposedValue.Enabled = true;
		    var textCo = go.GetComponentInChildren<Text>();
		    exposedValue.TextComponent = textCo;
	    }

        private GameObject InstantiateValuePrefab() {
            var valueGo = Instantiate(_valuePrefab);
            valueGo.transform.SetParent(_container, false);
            return valueGo;
        }

        public void HideValue(string valueName) {

        }
    }
}