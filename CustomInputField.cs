using System;
using System.Collections;
using UnityEngine.UI;

namespace DevelopmentConsoleTool {

    public class CustomInputField : InputField {

		public string IgnoredChars { get; set; }

	    protected override void Awake() {
		    base.Awake();

			onValidateInput += ValidateInputHandler;
	    }

		private char ValidateInputHandler(string text, int charIndex, char addedChar)
		{
			if (IgnoredChars.Contains(addedChar.ToString())) {
				return '\0';
			}
			return addedChar;
		}

		protected override void OnEnable() {
		    base.OnEnable();
		}

	    protected override void Start() {
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