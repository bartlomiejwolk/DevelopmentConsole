using System;
using System.Collections.Generic;
using System.Linq;
using DevelopmentConsole.Core.CodeCompletionModule;
using DevelopmentConsole.Core.CommandHandlerSystem;
using DevelopmentConsole.Core.FuzzySearchModule;
using DevelopmentConsole.Core.LineManagerModule;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 169
#pragma warning disable 649
#pragma warning disable 0414

namespace DevelopmentConsole.Core {

    /// <summary>
    /// The main class of the DevelopmentConsole tool.
    /// </summary>
    public class DevelopmentConsole : MonoBehaviour {
	    #region INSPECTOR

	    [SerializeField]
	    protected LineManager LineManager;

	    [SerializeField]
	    protected CodeCompletion CodeCompletion;

	    [SerializeField]
        private bool _dontDestroyOnLoad = true;

	    [SerializeField]
	    private Canvas _canvas;

	    [SerializeField]
	    private KeyCode _toggleConsoleWindowKey = KeyCode.BackQuote;

		#endregion

		public event EventHandler ConsoleWindowOpened;

		#region DELEGATES

		private Action _toggleConsoleWindowKeyPressed;
	    private Action _returnKeyPressed;
	    private Action _arrowUpKeyPressed;
	    private Action _arrowDownKeyPressed;
	    private Action _leftCtrlSpaceKeysPressed;

	    #endregion

	    protected string TypedCommand;
	    protected readonly FuzzySearch FuzzySearch = new FuzzySearch();
	    private readonly CommandHistory _commandHistory = new CommandHistory();
	    private readonly CommandLineArgumentParser _argumentParser
			= new CommandLineArgumentParser();

	    protected List<string> Arguments {
		    get { return _argumentParser.Arguments; }
	    }

	    private bool IsConsoleWindowOpen {
            get { return _canvas.gameObject.activeSelf; }
        }

	    #region UNITY MESSAGES

        private void Awake() {
            Assert.IsNotNull(LineManager);
            Assert.IsNotNull(CodeCompletion);
            Assert.IsNotNull(_canvas);
            
            SubscribeEventHandlers();

			// set ignored chars
	        var keyChar = (char)_toggleConsoleWindowKey;
            LineManager.IgnoredChars = keyChar.ToString();
        }

	    private void Update() {
            CheckForToggleConsoleWindowKey();
            HandleInConsoleKeyboardInput();
        }

        private void OnDestroy() {
            LineManager.LineValueChanged -= LineManager_OnLineValueChanged;
            CodeCompletion.OptionSelected -= CodeCompletion_OnOptionSelected;
        }

        #endregion

	    private void SubscribeEventHandlers() {
		    LineManager.LineValueChanged += LineManager_OnLineValueChanged;
		    CodeCompletion.OptionSelected += CodeCompletion_OnOptionSelected;
			ConsoleWindowOpened += OnConsoleWindowOpened;

		    _toggleConsoleWindowKeyPressed = OnToggleConsoleWindowKeyPressed;
		    _returnKeyPressed = OnReturnKeyPressed;
		    _arrowUpKeyPressed = OnArrowUpPressed;
		    _arrowDownKeyPressed = OnArrowDownPressed;
		    _leftCtrlSpaceKeysPressed = OnLeftCtrlSpacePressed;
	    }

	    private void OpenConsoleWindow() {
		    _canvas.gameObject.SetActive(true);
			InvokeConsoleWindowOpenedEvent();
	    }

	    private void CloseConsoleWindow() {
		    _canvas.gameObject.SetActive(false);
	    }

		// todo refactor
	    private void HandleDisplayCommandAutoCompletePanel(string typedChars) {
		    if (TypedCommand != null) {
			    return;
		    }
			var names = CommandHandlerManager.Instance.GetCommandNames();
			var matches = FuzzySearch.MatchResultSet(names, typedChars);
			List<string> options;
			if (matches == null) {
				options = names;
			}
			else {
				options = matches.Select(match => match.TextValue).ToList();
			}
			LineManager.CurrentLine.ForceLabelUpdate();
		    var textCo = LineManager.CurrentLine.textComponent;
		    CodeCompletion.DisplayOptions(options, textCo);
	    }

		// todo rename to HandleUpdateTypedCommand
	    private void UpdateTypedCommand(string input) {
		    var cmdNames = CommandHandlerManager.Instance.GetCommandNames();
		    foreach (var cmdName in cmdNames) {
			    if (input.StartsWith(cmdName + " ")) {
				    TypedCommand = cmdName;
				    return;
			    }
		    }
		    TypedCommand = null;
	    }

	    #region INPUT

        private void HandleInConsoleKeyboardInput() {
            if (!IsConsoleWindowOpen) {
                return;
            }

            CheckForReturnKey();
            CheckForArrowUpKey();
            CheckForArrowDownKey();
			CheckForLeftCtrlSpaceKeys();
        }

        private void CheckForToggleConsoleWindowKey() {
            if (Input.GetKeyDown(_toggleConsoleWindowKey)) {
                _toggleConsoleWindowKeyPressed();
            }
        }

        private void CheckForReturnKey() {
            if (Input.GetKeyDown(KeyCode.Return)) {
                _returnKeyPressed();
            }
        }

        private void CheckForArrowUpKey() {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                _arrowUpKeyPressed();
            }
        }

        private void CheckForArrowDownKey() {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                _arrowDownKeyPressed();
            }
        }

	    private void CheckForLeftCtrlSpaceKeys() {
		    if (Input.GetKey(KeyCode.LeftControl)
		        && Input.GetKeyDown(KeyCode.Space)) {

			    _leftCtrlSpaceKeysPressed();
		    }
	    }

        #endregion

	    #region INPUT HANDLERS

	    private void OnReturnKeyPressed() {
		    // let the code completion handle this
		    if (CodeCompletion.IsOpen) {
			    return;
		    }
		    _commandHistory.AddCommand(LineManager.CommandString);
		    CommandHandlerManager.Instance.HandleCommand(LineManager.CommandString);
		    LineManager.InstantiateLine();
	    }

	    private void OnToggleConsoleWindowKeyPressed() {
		    if (IsConsoleWindowOpen) {
			    CloseConsoleWindow();
		    }
		    else {
			    OpenConsoleWindow();
		    }
	    }

	    private void OnArrowUpPressed() {
		    var nextInput = _commandHistory.GetPreviousCommand();
		    if (nextInput == null) {
			    return;
		    }
		    LineManager.SetCommandString(nextInput);
	    }

	    private void OnArrowDownPressed() {
		    var previousInput = _commandHistory.GetNextCommand();
		    if (previousInput == null) {
			    return;
		    }
		    LineManager.SetCommandString(previousInput);
	    }

	    protected virtual void OnLeftCtrlSpacePressed() {
		    HandleDisplayCommandAutoCompletePanel(string.Empty);
	    }

	    #endregion

	    #region EVENT INVOCATORS

	    protected virtual void InvokeConsoleWindowOpenedEvent() {
		    var handler = ConsoleWindowOpened;
		    if (handler != null) handler(this, EventArgs.Empty);
	    }

	    #endregion

	    #region EVENT HANDLERS

	    private void OnConsoleWindowOpened(object sender, EventArgs eventArgs) {
		    LineManager.SetFocus();
	    }

	    protected virtual void LineManager_OnLineValueChanged(
            object sender,
            LineValueChangedEventArgs eventArgs) {

	        var input = eventArgs.Value;
	        UpdateTypedCommand(input);
	        _argumentParser.ParseArguments(input);
	        HandleDisplayCommandAutoCompletePanel(input);
        }

	    protected virtual void CodeCompletion_OnOptionSelected(
            object sender,
            SelectedOptionEventArgs selectedOptionEventArgs) {

            var option = selectedOptionEventArgs.Option;
            LineManager.SetCommandString(option);
		    LineManager.AddSpace();
            LineManager.SetFocus();
        }

        #endregion
    }
}