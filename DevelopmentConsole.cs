using System;
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
        private Canvas _canvas;

        [SerializeField]
        private KeyCode _toggleConsoleWindowKey = KeyCode.BackQuote;

        private Action _returnKeyPressed;
        private Action _toggleConsoleWindowKeyPressed;
        private Action _arrowUpKeyPressed;
        private Action _arrowDownKeyPressed;

        private readonly CommandHistory _commandHistory = new CommandHistory();

        private bool IsConsoleWindowOpen {
            get { return _canvas.gameObject.activeSelf; }
        }

        #region UNITY MESSAGES

        private void Awake() {
            _lineManager.LineValueChanged += LineManagerOnLineValueChanged;
            _returnKeyPressed += OnReturnKeyPressed;
            _toggleConsoleWindowKeyPressed += OnToggleConsoleWindowKeyPressed;
            _arrowUpKeyPressed += OnArrowUpPressed;
            _arrowDownKeyPressed += OnArrowDownPressed;

            var keyChar = (char)_toggleConsoleWindowKey;
            _lineManager.IgnoredChars = keyChar.ToString();

            Assert.IsNotNull(_lineManager);
            Assert.IsNotNull(_canvas);
        }

        private void LineManagerOnLineValueChanged(object sender, LineValueChangedEventArgs eventArgs) {
            Debug.Log(eventArgs.Value);
        }

        private void Start() {
        }

        private void Update() {
            CheckForToggleConsoleWindowKey();
            HandleInConsoleKeyboardInput();
        }

        private void OnDestroy() {
            _lineManager.LineValueChanged -= LineManagerOnLineValueChanged;
        }

        #endregion

        #region CHECK METHODS

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

        #region INPUT HANDLERS

        private void OnReturnKeyPressed() {
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