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

        [SerializeField]
        private Canvas _canvas;

        #endregion

        protected override void Awake() {
            base.Awake();
            Assert.IsNotNull(_valueVisualizer);
            Assert.IsNotNull(_canvas);
        }

        private void Start() {
            var visualizer = Instantiate(_valueVisualizer);
            visualizer.transform.SetParent(_canvas.transform, false);
        }

        public override void ShowValue(string valueName) {
            base.ShowValue(valueName);
            var valueDelegate = ExposedValueManager.Instance.GetSourceCallback(
                valueName);
            _valueVisualizer.RegisterValue(valueName, valueDelegate, transform.position);
        }
    }
}