using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Core {
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

        private ExposedValueManager() {}

        // todo create overload for int, float and bool
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

        // todo rename to GetValueDelegate
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

        public List<string> GetValueNames() {
            var names = _exposedValues.Keys.ToList();
            return names;
        }

        public Vector3 GetValuePosition(string valueName) {
            ExposedValue exposedValue;
            _exposedValues.TryGetValue(valueName, out exposedValue);
            var pos = Vector3.zero;
            if (exposedValue != null) {
                pos = exposedValue.Position;
            }
            return pos;
        }
    }
}