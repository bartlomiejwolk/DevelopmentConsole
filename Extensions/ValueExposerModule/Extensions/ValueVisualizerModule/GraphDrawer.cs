using UnityEngine;
using System.Collections;

namespace DevelopmentConsole.Extensions.ValueExposerModule.Extensions.ValueVisualizerModule {
    public class GraphDrawer : MonoBehaviour {
        public void DrawValuePoint(object value) {
            var number = (float) value;
        }
    }
}
