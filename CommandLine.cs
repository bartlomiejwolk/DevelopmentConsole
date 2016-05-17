using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 649

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
	    private CustomInputField inputField;

		private CustomInputField InputField {
		    get {
			    if (inputField != null) {
				    return inputField;
			    }
			    var component = GetComponent<CustomInputField>();
			    return component;
		    }
	    }

	    public float Height {
            get { return RectTransform.rect.height; }
        }

	    public string Text {
		    get { return InputField.text; }
	    }

        private void Awake() {
            Init();
        }

	    public void GetFocus() {
		    InputField.ActivateInputField();
	    }

	    public void MoveCarretToEnd() {
		    InputField.MoveCarretToEnd();
	    }

        private void Init() {
            InputField.text = prompt;
            RectTransform = GetComponent<RectTransform>();
        }

	    public void SetIgnoredChars(string chars) {
		    InputField.IgnoredChars = chars;
	    }
    }

}