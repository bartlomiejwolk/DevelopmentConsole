using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {

    public class ValueVisualizer : MonoBehaviour {

        [SerializeField]
        private GraphDrawer _graphDrawerTemplate;

        private void Awake() {
            Assert.IsNotNull(_graphDrawerTemplate);
        }

        public void VisualizeValue(Func<object> valueDelegate, Transform container) {
            throw new NotImplementedException();
        }
    }
}
