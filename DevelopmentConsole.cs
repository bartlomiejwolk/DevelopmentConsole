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
        private bool dontDestroyOnLoad = true;

        [SerializeField]
        private LineManager lineManager;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private KeyCode toggleConsoleWindowKey = KeyCode.BackQuote;

        private Action returnKeyPressed;
        private Action toggleConsoleWindowKeyPressed;
        private Action arrowUpKeyPressed;
        private Action arrowDownKeyPressed;

        private readonly CommandHistory commandHistory = new CommandHistory();

        private bool IsConsoleWindowOpen {
            get { return canvas.gameObject.activeSelf; }
        }

        #region UNITY MESSAGES

        private void Awake() {
            returnKeyPressed += OnReturnKeyPressed;
            toggleConsoleWindowKeyPressed += OnToggleConsoleWindowKeyPressed;
            arrowUpKeyPressed += OnArrowUpPressed;
            arrowDownKeyPressed += OnArrowDownPressed;

            var keyChar = (char)toggleConsoleWindowKey;
            lineManager.IgnoredChars = keyChar.ToString();

            Assert.IsNotNull(lineManager);
            Assert.IsNotNull(canvas);
        }

        private void Start() {
        }

        private void Update() {
            CheckForToggleConsoleWindowKey();
            HandleInConsoleKeyboardInput();
        }

        private void HandleInConsoleKeyboardInput() {
            if (!IsConsoleWindowOpen) {
                return;
            }

            CheckForReturnKey();
            CheckForArrowUpKey();
            CheckForArrowDownKey();
        }

        #endregion

        #region CHECK METHODS

        private void CheckForReturnKey() {
            if (Input.GetKeyDown(KeyCode.Return)) {
                returnKeyPressed();
            }
        }

        private void CheckForToggleConsoleWindowKey() {
            if (Input.GetKeyDown(toggleConsoleWindowKey)) {
                toggleConsoleWindowKeyPressed();
            }
        }

        private void CheckForArrowUpKey() {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                arrowUpKeyPressed();
            }
        }

        private void CheckForArrowDownKey() {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                arrowDownKeyPressed();
            }
        }

        #endregion

        private void OpenConsoleWindow() {
            canvas.gameObject.SetActive(true);
            lineManager.GetFocus();
        }

        private void CloseConsoleWindow() {
            canvas.gameObject.SetActive(false);
        }

        #region INPUT HANDLERS

        private void OnReturnKeyPressed() {
            commandHistory.AddCommand(lineManager.CommandString);
            CommandHandlerManager.Instance.HandleCommand(lineManager.CommandString);
            lineManager.InstantiateLine();
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
            var nextInput = commandHistory.GetPreviousCommand();
            if (nextInput == null) {
                return;
            }
            lineManager.SetCommandString(nextInput);
        }

        private void OnArrowDownPressed() {
            var previousInput = commandHistory.GetNextCommand();
            if (previousInput == null) {
                return;
            }
            lineManager.SetCommandString(previousInput);
        }

        #endregion
    }
}