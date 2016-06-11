using System.Linq;
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

        private ExposedValueManager _exposedValueManager;

        protected override void Awake() {
            base.Awake();
            Assert.IsNotNull(_valueVisualizerPrefab);
            Assert.IsNotNull(_canvas);

            _exposedValueManager = ExposedValueManager.Instance;
            _valueVisualizer = Instantiate(_valueVisualizerPrefab);
        }


        public override void ShowValue(string valueName) {
            base.ShowValue(valueName);
            DrawGraph(valueName);
        }

        private void DrawGraph(string valueName) {
            var valueDelegate = _exposedValueManager.GetSourceCallback(
                valueName);
            var graphPos = _exposedValueManager.GetBottomRightCornerPosition(
                valueName);
            var graphSize = _exposedValueManager.GetValueSize(valueName);

            _valueVisualizer.RegisterValue(valueName, valueDelegate)
                .SetPosition(graphPos)
                .SetSize(graphSize)
                .Enable(true);
        }
    }
}