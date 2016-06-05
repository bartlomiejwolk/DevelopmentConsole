using DevelopmentConsole.Extensions.ValueExposerModule.Core;
using UnityEngine;
using UnityEngine.Assertions;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions {
    public class ExtendedValueExposer : ValueExposer {
        #region INSPCTOR FIELDS

        [SerializeField]
        private ValueVisualizer _valueVisualizer;

        #endregion

        private void Awake() {
            Assert.IsNotNull(_valueVisualizer);
        }

        public override void ShowValue(string valueName) {
            base.ShowValue(valueName);
            var valueDelegate = ExposedValueManager.Instance.GetSourceCallback(
                valueName);
            _valueVisualizer.VisualizeValue(valueDelegate, Container);
        }
    }
}