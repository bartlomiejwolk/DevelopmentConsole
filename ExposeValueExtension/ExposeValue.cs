using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
#pragma warning disable 649

namespace DevelopmentConsoleTool.ExposeValueExtension {
    public class ExposeValue : MonoBehaviour {

        [SerializeField]
        private GameObject _valuePrefab;

        [SerializeField]
        private Transform _container;

        private static ExposeValue _instance;
        private ExposedValuesManager _exposedValuesManager;

        public static ExposeValue Instance {
            get {
                if (_instance != null) {
                    return _instance;
                }
				Debug.LogWarning("Add ExposeValue prefab to the scene!");
	            return null;
            }
        }

        private void Awake() {
            _instance = this;
            _exposedValuesManager = ExposedValuesManager.Instance;

            Assert.IsNotNull(_valuePrefab);
            Assert.IsNotNull(_container);
        }

        public void ShowValue(string valueName) {
            var go = InstantiateValuePrefab();
            var value = _exposedValuesManager.GetSourceValue(valueName);
            var textCo = go.GetComponent<Text>();
            if (textCo != null) {
                textCo.text = value.ToString();
            }
        }

        private GameObject InstantiateValuePrefab() {
            var valueGo = Instantiate(_valuePrefab);
            valueGo.transform.SetParent(_container, false);
            return valueGo;
        }

        public void HideValue(string valueName) {

        }
    }
}