using UnityEngine;
using UnityEngine.UI;

namespace DevelopmentConsole {

    /// <summary>
    /// Class attached to a line prefab.
    /// </summary>
    [RequireComponent(typeof(InputField))]
    public class CommandLine : MonoBehaviour {

        public RectTransform RectTransform { get; private set; }

        public float Height {
            get { return RectTransform.rect.height; }
        }

        private void Awake() {
            Init();
        }

        private void Init() {
            var inputField = GetComponent<InputField>();
            RectTransform = inputField.GetComponent<RectTransform>();
        }

    }

}