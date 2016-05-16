using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace DevelopmentConsoleTool {

    /// <summary>
    /// Class to be attached to a line prefab.
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

	    public string Text {
		    get { return InputField.text; }
	    }

        private void Awake() {
            Init();
        }

        private void Init() {
            InputField = GetComponent<InputField>();
            InputField.text = prompt;
            RectTransform = GetComponent<RectTransform>();
        }
    }

}