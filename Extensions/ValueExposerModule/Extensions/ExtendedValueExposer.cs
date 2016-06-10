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

        protected override void Awake() {
            base.Awake();
            Assert.IsNotNull(_valueVisualizerPrefab);
            Assert.IsNotNull(_canvas);
            
            _valueVisualizer = Instantiate(_valueVisualizerPrefab);
        }


        public override void ShowValue(string valueName) {
            base.ShowValue(valueName);
            // draw graph
            var valueDelegate = ExposedValueManager.Instance.GetSourceCallback(
                valueName);
            var graphPos = ExposedValueManager.Instance.GetBottomRightCornerPosition(valueName);
            var graphSize = ExposedValueManager.Instance.GetValueSize(valueName);
            _valueVisualizer.RegisterValue(valueName, valueDelegate, graphPos, graphSize, true);
        }
    }
}