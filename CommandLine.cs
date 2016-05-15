using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace DevelopmentConsoleTool {

    /// <summary>
    /// Class attached to a line prefab.
    /// </summary>
    [RequireComponent(typeof(InputField))]
    public class CommandLine : MonoBehaviour {

        [SerializeField]
        private string prompt = "> ";

        // cache
        public RectTransform RectTransform { get; private set; }
        private InputField InputField { get; set; }
        
        public float Height {
            get { return RectTransform.rect.height; }
        }

        private void Awake() {
            Init();
        }

        private void Init() {
            // ReSharper disable once PossibleNullReferenceException
            InputField = GetComponent<InputField>();
            Assert.IsNotNull(InputField);
            InputField.text = prompt;
            RectTransform = GetComponent<RectTransform>();
        }

        private void Update() {
        }

    }

}