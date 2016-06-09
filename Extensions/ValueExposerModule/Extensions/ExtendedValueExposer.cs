using DevelopmentConsole.Extensions.ValueExposerModule.Core;
using DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule;
using UnityEngine;
using UnityEngine.Assertions;
#pragma warning disable 649

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions {
    public class ExtendedValueExposer : ValueExposer {
        #region INSPCTOR FIELDS

        [SerializeField]
        private ValueVisualizer _valueVisualizerPrefab;

        [SerializeField]
        private Canvas _canvas;

        private ValueVisualizer _valueVisualizer;

        #endregion

        protected override void Awake() {
            base.Awake();
            Assert.IsNotNull(_valueVisualizerPrefab);
            Assert.IsNotNull(_canvas);
            
            _valueVisualizer = Instantiate(_valueVisualizerPrefab);
            _valueVisualizer.transform.SetParent(_canvas.transform, false);
        }

        private void Start() {
            
        }

        public override void ShowValue(string valueName) {
            base.ShowValue(valueName);
            var valueDelegate = ExposedValueManager.Instance.GetSourceCallback(
                valueName);
            _valueVisualizer.RegisterValue(valueName, valueDelegate, transform.position, true);
        }
    }
}