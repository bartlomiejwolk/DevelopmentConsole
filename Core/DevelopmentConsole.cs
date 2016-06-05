using System;
using System.Collections.Generic;
using System.Linq;
using DevelopmentConsoleTool.CodeCompletionModule;
using DevelopmentConsoleTool.CommandHandlerSystem;
using DevelopmentConsoleTool.FuzzySearchTool;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 169
#pragma warning disable 649
#pragma warning disable 0414

namespace DevelopmentConsoleTool {

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

	    #region DELEGATES

	    private Action _toggleConsoleWindowKeyPressed;
	    private Action _returnKeyPressed;
	    private Action _arrowUpKeyPressed;
	    private Action _arrowDownKeyPressed;

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

		    _toggleConsoleWindowKeyPressed = OnToggleConsoleWindowKeyPressed;
		    _returnKeyPressed = OnReturnKeyPressed;
		    _arrowUpKeyPressed = OnArrowUpPressed;
		    _arrowDownKeyPressed = OnArrowDownPressed;
	    }

	    private void OpenConsoleWindow() {
		    _canvas.gameObject.SetActive(true);
			// todo fire ConsoleWindowOpened event and SetFocus() in the handler
		    LineManager.SetFocus();
	    }

	    private void CloseConsoleWindow() {
		    _canvas.gameObject.SetActive(false);
	    }

	    private void DisplayCodeAutoCompletionPanel(string typedChars) {
			var names = CommandHandlerManager.Instance.GetCommandNames();
			var matches = FuzzySearch.MatchResultSet(names, typedChars);
			if (matches == null) {
				CodeCompletion.ClearResults();
				return;
		    }
		    LineManager.CurrentLine.ForceLabelUpdate();
		    var options = matches.Select(match => match.TextValue).ToList();
		    var textCo = LineManager.CurrentLine.textComponent;
		    CodeCompletion.DisplayOptions(options, textCo);
	    }

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

	    #endregion

	    #region EVENT HANDLERS

        protected virtual void LineManager_OnLineValueChanged(
            object sender,
            LineValueChangedEventArgs eventArgs) {

	        var input = eventArgs.Value;
	        UpdateTypedCommand(input);
	        _argumentParser.ParseArguments(input);
			// todo rename to DisplayAutoCompletionPanel
	        DisplayCodeAutoCompletionPanel(input);
        }

	    protected virtual void CodeCompletion_OnOptionSelected(
            object sender,
            SelectedOptionEventArgs selectedOptionEventArgs) {

            var option = selectedOptionEventArgs.Option;
            LineManager.SetCommandString(option);
            LineManager.SetFocus();
        }

        #endregion
    }
}