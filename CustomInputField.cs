using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DevelopmentConsoleTool {

    public class CustomInputField : InputField {

		public string IgnoredChars { get; set; }
		public string Prompt { get; set; }

	    protected override void Awake() {
		    base.Awake();

			onValidateInput += ValidateInputHandler;
		    onValueChanged.AddListener(ValueChangedHandler);
	    }

		private char ValidateInputHandler(string text, int charIndex, char addedChar)
		{
			if (IgnoredChars.Contains(addedChar.ToString())) {
				return '\0';
			}
			return addedChar;
		}

	    private void ValueChangedHandler(string value) {
			// prevent prompt to be deleted
		    if (value.Length < Prompt.Length) {
			    text = Prompt;
		    }

			// prevent caret from going onto the prompt
		    if (caretPosition < Prompt.Length) {
			    MoveTextEnd(false);
		    }
	    }

		protected override void OnEnable() {
		    base.OnEnable();
		}

	    protected override void Start() {
			text = Prompt;
			ActivateInputField();
			StartCoroutine(MoveTextEnd_NextFrame());
		}

	    public void MoveCarretToEnd() {
			StartCoroutine(MoveTextEnd_NextFrame());
		}

		IEnumerator MoveTextEnd_NextFrame() {
            yield return 0; 
            MoveTextEnd(false);
        }

    }

}