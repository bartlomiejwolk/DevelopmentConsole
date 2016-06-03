using UnityEngine;

namespace DevelopmentConsoleTool.ExposeValueExtension {
    public class ExposeValue : MonoBehaviour {

        private static ExposeValue _instance;

        private ExposedValuesManager _exposedValuesManager;

        public static ExposeValue Instance {
            get {
                if (_instance != null) {
                    return _instance;
                }

                var go = new GameObject();
                var component = go.AddComponent<ExposeValue>();
                return component;
            }
        }

        private void Awake() {
            _instance = this;
            _exposedValuesManager = ExposedValuesManager.Instance;
        }

        public void ShowValue(string valueName) {
            var value = _exposedValuesManager.GetSourceValue(valueName);
            Debug.Log(value);
        }

        public void HideValue(string valueName) {

        }
    }
}