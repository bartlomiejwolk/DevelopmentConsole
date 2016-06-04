using System;
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

		public event EventHandler<ValueInstantiatedEventArgs> ValueInstantiated;

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

			ValueInstantiated += OnValueInstantiated;
        }

		private void OnValueInstantiated(
			object sender,
			ValueInstantiatedEventArgs eventArgs) {

			var exposedValue =
				_exposedValuesManager.GetExposedValue(eventArgs.ValueName);
			exposedValue.Enabled = true;
			var textCo = eventArgs.GameObject.GetComponentInChildren<Text>();
			exposedValue.TextComponent = textCo;
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
            InstantiateValuePrefab(valueName);
	    }

        private void InstantiateValuePrefab(string valueName) {
            var valueGo = Instantiate(_valuePrefab);
            valueGo.transform.SetParent(_container, false);
			InvokeValueInstantiatedEvent(valueName, valueGo);
        }

        public void HideValue(string valueName) {

        }

	    protected virtual void InvokeValueInstantiatedEvent(
			string valueName,
			GameObject gameObject) {

		    var args = new ValueInstantiatedEventArgs(valueName, gameObject);
		    var handler = ValueInstantiated;
		    if (handler != null) handler(this, args);
	    }
    }

	public class ValueInstantiatedEventArgs : EventArgs {
		public string ValueName { get; private set; }
		public GameObject GameObject { get; private set; }

		public ValueInstantiatedEventArgs(
			string valueName,
			GameObject gameObject) {

			ValueName = valueName;
			GameObject = gameObject;
		}
	}
}