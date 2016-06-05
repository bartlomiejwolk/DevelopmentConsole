using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 649

namespace DevelopmentConsoleTool.ValueExposerExtension {
    public class ValueExposer : MonoBehaviour {
	    
		#region INSPECTOR FIELDS

	    [SerializeField]
        private GameObject _valuePrefab;

	    [SerializeField]
	    private Transform _container;

	    #endregion

	    private static ValueExposer _instance;

	    private ExposedValueManager _exposedValueManager;

		public event EventHandler<ValueInstantiatedEventArgs> ValueInstantiated;

		public static ValueExposer Instance {
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
            _exposedValueManager = ExposedValueManager.Instance;

            Assert.IsNotNull(_valuePrefab);
            Assert.IsNotNull(_container);

			ValueInstantiated += OnValueInstantiated;
        }

	    private void Update() {
		    UpdateValues();
	    }

	    #endregion

	    public void ShowValue(string valueName) {
		    var value = _exposedValueManager.GetExposedValue(valueName);
		    if (value == null) {
			    return;
		    }
		    if (value.Go == null) {
			    InstantiateValuePrefab(valueName);
		    }
		    value.UpdateEnabled = true;
	    }

	    public void HideValue(string valueName) {
		    var value = _exposedValueManager.GetExposedValue(valueName);
		    if (value == null) {
			    return;
		    }
		    value.UpdateEnabled = false;
	    }

	    private void InstantiateValuePrefab(string valueName) {
		    var valueGo = Instantiate(_valuePrefab);
		    valueGo.transform.SetParent(_container, false);
		    InvokeValueInstantiatedEvent(valueName, valueGo);
	    }

	    private void UpdateValues() {
		    var values = _exposedValueManager.ExposedValues;
		    foreach (var value in values) {
			    if (value.Value.UpdateEnabled) {
				    UpdateValue(value);
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

		    var args = new ValueInstantiatedEventArgs() {
			    ValueName = valueName,
				Go = go
		    };
		    var handler = ValueInstantiated;
		    if (handler != null) handler(this, args);
	    }

	    #endregion

	    #region EVENT HANDLERS

	    private void OnValueInstantiated(
		    object sender,
		    ValueInstantiatedEventArgs eventArgs) {

		    var exposedValue = _exposedValueManager.GetExposedValue(
			    eventArgs.ValueName);
		    exposedValue.Go = eventArgs.Go;
	    }

	    #endregion
    }
}