using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 649

namespace DevelopmentConsole.Extensions.ValueExposerModule.Core {
    public class ValueExposer : MonoBehaviour {
        #region INSPECTOR FIELDS

        [SerializeField]
        private GameObject _exposedValueTemplate;

        [SerializeField]
        private Transform _container;

        #endregion

        private static ValueExposer _instance;

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

        protected Transform Container {
            get { return _container; }
        }

        protected ExposedValueManager ExposedValueManager { get; private set; }

        #region UNITY MESSAGES

        protected virtual void Awake() {
            _instance = this;
            ExposedValueManager = ExposedValueManager.Instance;

            Assert.IsNotNull(_exposedValueTemplate);
            Assert.IsNotNull(_container);

            ValueInstantiated += OnValueInstantiated;
        }

        protected virtual void Update() {
            UpdateValues();
        }

        #endregion

        // todo create RegisterValue(), register value delegate here instead of the ExposedValueManager class
        public virtual void ShowValue(string valueName) {
            // get value by name
            var value = ExposedValueManager.GetExposedValue(valueName);
            if (value == null) {
                return;
            }
            // enable
            value.UpdateEnabled = true;
            // instantiate prefab
            if (value.Go == null) {
                InstantiateValuePrefab(valueName);
            }
        }

        public void HideValue(string valueName) {
            var value = ExposedValueManager.GetExposedValue(valueName);
            if (value == null) {
                return;
            }
            value.UpdateEnabled = false;
        }

        private void InstantiateValuePrefab(string valueName) {
            var valueGo = Instantiate(_exposedValueTemplate);
            valueGo.transform.SetParent(_container, false);
            InvokeValueInstantiatedEvent(valueName, valueGo);
        }

        private void UpdateValues() {
            var exposedValues = ExposedValueManager.ExposedValues;
            foreach (var exposedValue in exposedValues.Values) {
                if (exposedValue.UpdateEnabled) {
                    UpdateValue(exposedValue);
                }
            }
        }

        private void UpdateValue(ExposedValue exposedValue) {
            var textCo = exposedValue.TextComponent;
            if (textCo == null) {
                return;
            }
            var text = exposedValue.ValueString;
            textCo.text = text;
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

            var exposedValue = ExposedValueManager.GetExposedValue(
                eventArgs.ValueName);
            exposedValue.Go = eventArgs.Go;
        }

        #endregion
    }
}