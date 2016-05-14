using UnityEngine;
using UnityEngine.UI;

namespace DevelopmentConsole {

    /// <summary>
    /// Class attached to a line prefab.
    /// </summary>
    [RequireComponent(typeof(InputField))]
    public class CommandLine : MonoBehaviour {

        private RectTransform rectTransform;

        public float Height {
            get { return rectTransform.rect.height; }
        }

        private void Awake() {
            Init();
        }

        private void Init() {
            var inputField = GetComponent<InputField>();
            rectTransform = inputField.GetComponent<RectTransform>();
        }

    }

}