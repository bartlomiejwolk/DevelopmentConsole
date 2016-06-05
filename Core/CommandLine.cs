using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#pragma warning disable 649

namespace DevelopmentConsoleTool {

    public class CommandLine : InputField {
	    
		private string _prompt;
	    private string _ignoredChars;
	    private RectTransform _rectTransform;

	    public RectTransform RectTransform {
		    get {
			    if (_rectTransform != null) {
				    return _rectTransform;
			    }
				var com = GetComponent<RectTransform>();
			    return com;
		    }
	    }

		public float Height
		{
			get { return RectTransform.rect.height; }
		}

        public string Prompt {
            //get { return _prompt; }
			set { _prompt = value; }
        }

        #region UNITY MESSAGES

	    private void OnGUI() {
		    RedefineUpDownArrowBehavior();
		}

	    protected override void Awake() {
		    base.Awake();

		    onValidateInput += ValidateInputHandler;
		    onValueChanged.AddListener(OnValueChanged);
	    }

	    protected override void Start() {
		    text = _prompt;
		    ActivateInputField();
		    MoveCaretToEnd();
	    }

	    #endregion

	    public void Init(string prompt, string ignoredChars) {
		    _prompt = prompt;
		    _ignoredChars = ignoredChars;
	    }

	    public void MoveCaretToEnd() {
		    StartCoroutine(MoveTextEnd_NextFrame());
	    }

	    public string GetCommandString() {
			var cmdString = text.Substring(_prompt.Length);
			return cmdString;
		}

		public void GetFocus() {
			ActivateInputField();
		}

		public void SetReadOnly() {
			readOnly = true;
		}

		public void SetCommandString(string cmd) {
			var result = _prompt + cmd;
			text = result;
		}

	    private void RedefineUpDownArrowBehavior() {
		    var currentEvent = Event.current;
		    if (currentEvent.keyCode == KeyCode.UpArrow ||
		        currentEvent.keyCode == KeyCode.DownArrow) {

			    currentEvent.Use();
			    MoveCaretToEnd();
		    }
	    }

	    IEnumerator MoveTextEnd_NextFrame() {
		    yield return 0; 
		    MoveTextEnd(false);
	    }

	    #region EVENT HANDLERS

	    private char ValidateInputHandler(string fieldText, int charIndex, char addedChar) {
		    if (_ignoredChars.Contains(addedChar.ToString())) {
			    return '\0';
		    }
			// ignore input when modifier key is pressed
		    if (Input.GetKey(KeyCode.LeftControl)) {
			    return '\0';
		    }
		    return addedChar;
	    }

	    private void OnValueChanged(string value) {
		    // prevent prompt to be deleted
		    if (value.Length < _prompt.Length) {
			    text = _prompt;
		    }

		    // prevent caret from going onto the prompt
		    if (caretPosition < _prompt.Length) {
			    MoveTextEnd(false);
		    }
	    }

	    public override void OnPointerClick(PointerEventData eventData) {
			base.OnPointerClick(eventData);
			MoveCaretToEnd();
	    }

	    #endregion
    }

}