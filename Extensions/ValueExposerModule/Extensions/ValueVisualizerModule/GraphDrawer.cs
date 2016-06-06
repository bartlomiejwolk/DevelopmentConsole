using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    public class GraphDrawer : MonoBehaviour {

        [SerializeField]
        private Image _dotSprite;

        private void Awake() {
            Assert.IsNotNull(_dotSprite);
        }

        public void DrawValuePoint(object value) {
            var number = (float) value;
        }
    }
}
