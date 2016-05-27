﻿using System;
using DevelopmentConsoleTool.CommandHandlerSystem;
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
        private CodeCompletion _codeCompletion;

        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private KeyCode _toggleConsoleWindowKey = KeyCode.BackQuote;

        private Action _returnKeyPressed;
        private Action _toggleConsoleWindowKeyPressed;
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
            _returnKeyPressed += OnReturnKeyPressed;
            _toggleConsoleWindowKeyPressed += OnToggleConsoleWindowKeyPressed;
            _arrowUpKeyPressed += OnArrowUpPressed;
            _arrowDownKeyPressed += OnArrowDownPressed;
            _codeCompletion.OptionSelected += CodeCompletion_OnOptionSelected;

            var keyChar = (char)_toggleConsoleWindowKey;
            _lineManager.IgnoredChars = keyChar.ToString();
        }

        private void Start() {
        }

        private void Update() {
            CheckForToggleConsoleWindowKey();
            HandleInConsoleKeyboardInput();
        }

        private void OnDestroy() {
            _lineManager.LineValueChanged -= LineManager_OnLineValueChanged;
        }

        #endregion

        #region INPUT

        private void CheckForReturnKey() {
            if (Input.GetKeyDown(KeyCode.Return)) {
                _returnKeyPressed();
            }
        }

        private void CheckForToggleConsoleWindowKey() {
            if (Input.GetKeyDown(_toggleConsoleWindowKey)) {
                _toggleConsoleWindowKeyPressed();
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

        private void HandleInConsoleKeyboardInput() {
            if (!IsConsoleWindowOpen) {
                return;
            }

            CheckForReturnKey();
            CheckForArrowUpKey();
            CheckForArrowDownKey();
        }

        private void OpenConsoleWindow() {
            _canvas.gameObject.SetActive(true);
            _lineManager.GetFocus();
        }

        private void CloseConsoleWindow() {
            _canvas.gameObject.SetActive(false);
        }

        #region EVENT HANDLERS

        private void LineManager_OnLineValueChanged(object sender, LineValueChangedEventArgs eventArgs) {
            var typedChars = eventArgs.Value;
            var names = CommandHandlerManager.Instance.GetCommandNames();
            var matches = _fuzzySearch.MatchResultSet(names, typedChars);
            _codeCompletion.DisplayResults(matches);
        }

        private void CodeCompletion_OnOptionSelected(
            object sender,
            SelectedOptionEventArgs selectedOptionEventArgs) {

            var option = selectedOptionEventArgs.Option;
            _lineManager.SetCommandString(option);
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