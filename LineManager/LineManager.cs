using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 169
#pragma warning disable 649

namespace DevelopmentConsoleTool {

    /// <summary>
    /// Class responsible for handling command lines displayed on the screen.
    /// </summary>
    public class LineManager : MonoBehaviour {

        public event EventHandler<LineInstantiatedEventArgs> LineInstantiated;
        public event EventHandler<LineValueChangedEventArgs> LineValueChanged;
        public string IgnoredChars { get; set; }

        [SerializeField]
        private CommandLine _firstLine;

        [SerializeField]
        private CommandLine _commandLineTemplate;

        private readonly List<CommandLine> _lines = new List<CommandLine>();

        public CommandLine LastLine {
            get { return _lines.LastOrDefault(); }
        }

        public CommandLine PenultimateLine
        {
            get {
                if (_lines.Count > 1) {
                    var penultimate = _lines[_lines.Count - 2];
                    return penultimate;
                }
                return null;
            }
        }

        public string CommandString {
            get
            {
                var cmd = LastLine.GetCommandString();
                return cmd;
            }
        }

        #region UNITY MESSAGES

        private void Awake() {
            Assert.IsNotNull(_commandLineTemplate);
            Assert.IsNotNull(_firstLine);

            LineInstantiated += OnInstantiated;
            _lines.Add(_firstLine);
            SubscribeToValueChangedEvent();
        }

        private void OnEnable() {
            LastLine.GetFocus();
            LastLine.MoveCaretToEnd();
        }

        private void Start() {
            _firstLine.IgnoredChars = IgnoredChars;
        }

        #endregion

        private void RepositionLines() {
            // calculate offset (relative to global (0; 0))
            var correctPos = LastLine.Height/2;
            var pos = LastLine.transform.position.y;
            var offset = pos - correctPos;
            
            // line is fully within the canvas
            if (offset > 0) {
                return;
            }

            // update line container position
            var verticalPos = transform.position.y + Mathf.Abs(offset);
            transform.position = new Vector3(transform.position.x, verticalPos, transform.position.z);
        }

        public void InstantiateLine() {
            var cmdLineGo = Instantiate(_commandLineTemplate);
            cmdLineGo.gameObject.SetActive(true);
            cmdLineGo.transform.SetParent(transform, false);

            var args = new LineInstantiatedEventArgs(cmdLineGo.gameObject);
            InvokeLineInstantiatedEvent(args);
        }

        private void PositionLine() {
            var newLineRectTransform = LastLine.GetComponent<RectTransform>();
            var targetPos = CalculateLinePosition();
            newLineRectTransform.anchoredPosition = targetPos;
        }

        // calculates position to place line on the screen
        private Vector2 CalculateLinePosition() {
            var verticalOffset = (PenultimateLine.Height / 2) +
                (LastLine.Height / 2);
            var yPos = PenultimateLine.RectTransform.anchoredPosition.y;
            var verticalPos = yPos - verticalOffset;
            var newPos = new Vector2(
                PenultimateLine.RectTransform.anchoredPosition.x,
                verticalPos);

            return newPos;
        }

        #region EVENT INVOCATORS

        protected virtual void InvokeLineInstantiatedEvent(LineInstantiatedEventArgs args) {
            var handler = LineInstantiated;
            if (handler != null) handler(this, args);
        }

        protected virtual void InvokeLineValueChangedEvent(LineValueChangedEventArgs args) {
            var handler = LineValueChanged;
            if (handler != null) handler(this, args);
        }

        #endregion

        #region EVENT HANDLERS

        private void OnInstantiated(object sender, LineInstantiatedEventArgs eventArgs) {
            var go = eventArgs.InstantiatedGo;
            var cmdLine = go.GetComponent<CommandLine>();

            _lines.Add(cmdLine);
            cmdLine.IgnoredChars = IgnoredChars;
            PositionLine();
            PenultimateLine.SetReadOnly();
            RepositionLines();
            UnsubscribeFromValueChangedEvent();
            SubscribeToValueChangedEvent();
        }

        private void UnsubscribeFromValueChangedEvent() {
            PenultimateLine.onValueChanged.RemoveAllListeners();
        }

        private void SubscribeToValueChangedEvent() {
            LastLine.onValueChanged.AddListener(InputField_OnValueChanged);
        }

        private void InputField_OnValueChanged(string text) {
            var commandString = StripPrompt(text);
            var args = new LineValueChangedEventArgs(commandString);
            InvokeLineValueChangedEvent(args);
        }

        private string StripPrompt(string text) {
            if (text.Length <= LastLine.Prompt.Length) {
                return string.Empty;
            }
            var commandString = text.Substring(LastLine.Prompt.Length);
            return commandString;
        }

        #endregion

        public void SetFocus() {
            LastLine.GetFocus();
            LastLine.MoveCaretToEnd();
        }

        public void SetCommandString(string text) {
            LastLine.SetCommandString(text);
        }
    }
}