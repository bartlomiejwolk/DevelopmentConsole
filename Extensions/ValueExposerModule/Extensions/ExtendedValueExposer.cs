using DevelopmentConsole.Extensions.ValueExposerModule.Core;
using DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule;
using UnityEngine;
using UnityEngine.Assertions;
#pragma warning disable 649

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions {
    public class ExtendedValueExposer : ValueExposer {
        #region INSPCTOR FIELDS

        [SerializeField]
        private ValueVisualizer _valueVisualizer;

        #endregion

        protected override void Awake() {
            base.Awake();
            Assert.IsNotNull(_valueVisualizer);
        }

        public override void ShowValue(string valueName) {
            base.ShowValue(valueName);
            var valueDelegate = ExposedValueManager.Instance.GetSourceCallback(
                valueName);

            // todo debug
            var canvas = Container.GetComponentInParent<Canvas>();

            // todo don't pass container. Get reference to the GO's transform instead and position it here.
            _valueVisualizer.VisualizeValue(valueName, valueDelegate, canvas.transform);
        }
    }
}