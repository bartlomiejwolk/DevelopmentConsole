using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 649

namespace DevelopmentConsoleTool {

    public class CommandLine : InputField {
	    [SerializeField]
		private string prompt = "> ";

	    private RectTransform rectTransform;

	    public RectTransform RectTransform {
		    get {
			    if (rectTransform != null) {
				    return rectTransform;
			    }
				var com = GetComponent<RectTransform>();
			    return com;
		    }
	    }

	    public string IgnoredChars { get; set; }

		public float Height
		{
			get { return RectTransform.rect.height; }
		}

	    #region UNITY MESSAGES

	    private void OnGUI() {
		    RedefineUpDownArrowBehavior();
		}

	    protected override void Awake() {
		    base.Awake();

		    onValidateInput += ValidateInputHandler;
		    onValueChanged.AddListener(ValueChangedHandler);
	    }

	    protected override void Start() {
		    text = prompt;
		    ActivateInputField();
		    StartCoroutine(MoveTextEnd_NextFrame());
	    }

	    #endregion

	    private void RedefineUpDownArrowBehavior() {
		    var currentEvent = Event.current;
		    if (currentEvent.keyCode == KeyCode.UpArrow ||
				currentEvent.keyCode == KeyCode.DownArrow) {

			    currentEvent.Use();
			    MoveCaretToEnd();
		    }
	    }

	    private char ValidateInputHandler(string fieldText, int charIndex, char addedChar)
        {
            if (IgnoredChars.Contains(addedChar.ToString())) {
                return '\0';
            }
            return addedChar;
        }

        private void ValueChangedHandler(string value) {
            // prevent prompt to be deleted
            if (value.Length < prompt.Length) {
                text = prompt;
            }

            // prevent caret from going onto the prompt
            if (caretPosition < prompt.Length) {
                MoveTextEnd(false);
            }
        }

	    public void MoveCaretToEnd() {
            StartCoroutine(MoveTextEnd_NextFrame());
        }

        IEnumerator MoveTextEnd_NextFrame() {
            yield return 0; 
            MoveTextEnd(false);
        }

		public string GetCommandString()
		{
			var cmdString = text.Substring(prompt.Length);
			return cmdString;
		}

		public void GetFocus()
		{
			ActivateInputField();
		}

		public void SetReadOnly()
		{
			readOnly = true;
		}

		public void SetCommandString(string cmd)
		{
			var result = prompt + cmd;
			text = result;
		}

	}

}