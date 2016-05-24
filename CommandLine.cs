using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 649

namespace DevelopmentConsoleTool {

    public class CommandLine : InputField {
	    
        [SerializeField]
		private string _prompt = "> ";

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
		    text = _prompt;
		    ActivateInputField();
		    MoveCaretToEnd();
	    }

	    #endregion

	    public void MoveCaretToEnd() {
		    StartCoroutine(MoveTextEnd_NextFrame());
	    }

	    public string GetCommandString()
		{
			var cmdString = text.Substring(_prompt.Length);
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

	    private char ValidateInputHandler(string fieldText, int charIndex, char addedChar)
	    {
		    if (IgnoredChars.Contains(addedChar.ToString())) {
			    return '\0';
		    }
		    return addedChar;
	    }

	    private void ValueChangedHandler(string value) {
		    // prevent prompt to be deleted
		    if (value.Length < _prompt.Length) {
			    text = _prompt;
		    }

		    // prevent caret from going onto the prompt
		    if (caretPosition < _prompt.Length) {
			    MoveTextEnd(false);
		    }
	    }

	    #endregion
    }

}