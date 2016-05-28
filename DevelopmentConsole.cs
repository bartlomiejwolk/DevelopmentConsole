using System;
using System.Collections.Generic;
using System.Linq;
using DevelopmentConsoleTool.CodeCompletion;
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

        [SerializeField]
        private bool _dontDestroyOnLoad = true;

        [SerializeField]
        private LineManager _lineManager;

        [SerializeField]
        private CodeCompletion.CodeCompletion _codeCompletion;

        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private KeyCode _toggleConsoleWindowKey = KeyCode.BackQuote;

        private Action _toggleConsoleWindowKeyPressed;
        private Action _returnKeyPressed;
        private Action _arrowUpKeyPressed;
        private Action _arrowDownKeyPressed;

        private readonly CommandHistory _commandHistory = new CommandHistory();
        private readonly FuzzySearch _fuzzySearch = new FuzzySearch();

        private bool IsConsoleWindowOpen {
            get { return _canvas.gameObject.activeSelf; }
        }

        #region UNITY MESSAGES

        private void Awake() {
            Assert.IsNotNull(_lineManager);
            Assert.IsNotNull(_codeCompletion);
            Assert.IsNotNull(_canvas);
            
            _lineManager.LineValueChanged += LineManager_OnLineValueChanged;
            _codeCompletion.OptionSelected += CodeCompletion_OnOptionSelected;

            _toggleConsoleWindowKeyPressed = OnToggleConsoleWindowKeyPressed;
            _returnKeyPressed = OnReturnKeyPressed;
            _arrowUpKeyPressed = OnArrowUpPressed;
            _arrowDownKeyPressed = OnArrowDownPressed;

            var keyChar = (char)_toggleConsoleWindowKey;
            _lineManager.IgnoredChars = keyChar.ToString();
        }

        private void Update() {
            CheckForToggleConsoleWindowKey();
            HandleInConsoleKeyboardInput();
        }

        private void OnDestroy() {
            _lineManager.LineValueChanged -= LineManager_OnLineValueChanged;
            _codeCompletion.OptionSelected -= CodeCompletion_OnOptionSelected;
        }

        #endregion

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

        private void OpenConsoleWindow() {
            _canvas.gameObject.SetActive(true);
            _lineManager.SetFocus();
        }

        private void CloseConsoleWindow() {
            _canvas.gameObject.SetActive(false);
        }

        #region EVENT HANDLERS

        private void LineManager_OnLineValueChanged(
            object sender,
            LineValueChangedEventArgs eventArgs) {
            
            var typedChars = eventArgs.Value;
            var names = CommandHandlerManager.Instance.GetCommandNames();
            var matches = _fuzzySearch.MatchResultSet(names, typedChars);
			var options = matches.Select(match => match.TextValue).ToList();
	        _codeCompletion.DisplayOptions(
				options,
				_lineManager.LastLine.textComponent);
        }

        private void CodeCompletion_OnOptionSelected(
            object sender,
            SelectedOptionEventArgs selectedOptionEventArgs) {

            var option = selectedOptionEventArgs.Option;
            _lineManager.SetCommandString(option);
            _lineManager.SetFocus();
        }

        #endregion

        #region INPUT HANDLERS

        private void OnReturnKeyPressed() {
            // let the code completion handle this
            if (_codeCompletion.IsOpen) {
                return;
            }
            _commandHistory.AddCommand(_lineManager.CommandString);
            CommandHandlerManager.Instance.HandleCommand(_lineManager.CommandString);
            _lineManager.InstantiateLine();
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
            _lineManager.SetCommandString(nextInput);
        }

        private void OnArrowDownPressed() {
            var previousInput = _commandHistory.GetNextCommand();
            if (previousInput == null) {
                return;
            }
            _lineManager.SetCommandString(previousInput);
        }

        #endregion
    }
}