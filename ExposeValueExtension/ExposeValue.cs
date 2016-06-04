using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
#pragma warning disable 649

namespace DevelopmentConsoleTool.ExposeValueExtension {
    public class ExposeValue : MonoBehaviour {
	    #region INSPECTOR FIELDS

	    [SerializeField]
        private GameObject _valuePrefab;

	    [SerializeField]
	    private Transform _container;

	    #endregion

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

	    #region UNITY MESSAGES

	    private void Awake() {
            _instance = this;
            _exposedValuesManager = ExposedValuesManager.Instance;

            Assert.IsNotNull(_valuePrefab);
            Assert.IsNotNull(_container);

			ValueInstantiated += OnValueInstantiated;
        }

	    private void Update() {
		    UpdateValues();
	    }

	    #endregion

	    public void ShowValue(string valueName) {
		    var value = _exposedValuesManager.GetExposedValue(valueName);
		    if (value.Go == null) {
			    InstantiateValuePrefab(valueName);
		    }
		    value.UpdateEnabled = true;
	    }

	    public void HideValue(string valueName) {
		    var value = _exposedValuesManager.GetExposedValue(valueName);
		    value.UpdateEnabled = false;
		    value.Go.SetActive(false);
	    }

	    private void InstantiateValuePrefab(string valueName) {
		    var valueGo = Instantiate(_valuePrefab);
		    valueGo.transform.SetParent(_container, false);
		    InvokeValueInstantiatedEvent(valueName, valueGo);
	    }

	    private void UpdateValues() {
		    var values = _exposedValuesManager.ValuesSources;
		    foreach (var exposedValue in values) {
			    if (exposedValue.Value.UpdateEnabled) {
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

	    #region EVENT INVOCATORS

	    protected virtual void InvokeValueInstantiatedEvent(
			string valueName,
			GameObject go) {

		    var args = new ValueInstantiatedEventArgs(valueName, go);
		    var handler = ValueInstantiated;
		    if (handler != null) handler(this, args);
	    }

	    #endregion

	    #region EVENT HANDLERS

	    private void OnValueInstantiated(
		    object sender,
		    ValueInstantiatedEventArgs eventArgs) {

		    var exposedValue = _exposedValuesManager.GetExposedValue(
			    eventArgs.ValueName);
		    exposedValue.Go = eventArgs.Go;
	    }

	    #endregion
    }
}