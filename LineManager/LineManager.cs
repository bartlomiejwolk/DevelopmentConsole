﻿using System;
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
		private string _prompt = "> ";

        [SerializeField]
        private CommandLine _commandLineTemplate;

        private readonly List<CommandLine> _lines = new List<CommandLine>();

		// todo rename to CurrentLine
        public CommandLine CurrentLine {
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
                var cmd = CurrentLine.GetCommandString();
                return cmd;
            }
        }

        #region UNITY MESSAGES

        private void Awake() {
            Assert.IsNotNull(_commandLineTemplate);

            LineInstantiated = OnLineInstantiated;
            SubscribeToValueChangedEvent();
        }

        private void OnEnable() {
            CurrentLine.GetFocus();
            CurrentLine.MoveCaretToEnd();
        }

        private void Start() {
			InstantiateLine();
        }

	    private void OnDestroy() {
		    UnsubscribeFromValueChangedEvent();
	    }

        #endregion

        private void RepositionLines() {
            // calculate offset (relative to global (0; 0))
            var correctPos = CurrentLine.Height/2;
            var pos = CurrentLine.transform.position.y;
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

        private void HandleLinePositioning() {
			if (CurrentLine == null)
			{
				return;
			}
			// first line is instantiated at correct position
	        if (_lines.Count == 1) {
		        return;
	        }
			var currentLine = CurrentLine.GetComponent<RectTransform>();
			var targetPos = CalculateLinePosition();
            currentLine.anchoredPosition = targetPos;
        }

		// calculates target position for instantiated line
        private Vector2 CalculateLinePosition() {
			var vertOffset = (PenultimateLine.Height / 2)
				+ (CurrentLine.Height / 2);
			var penultimateVertPos = PenultimateLine.RectTransform.anchoredPosition.y;
			var endVertPos = penultimateVertPos - vertOffset;
	        var penultimateHorPos = PenultimateLine.RectTransform.anchoredPosition.x;
			var newPos = new Vector2(
				penultimateHorPos,
				endVertPos);
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

        private void OnLineInstantiated(object sender, LineInstantiatedEventArgs eventArgs) {
            var go = eventArgs.InstantiatedGo;
            var cmdLine = go.GetComponent<CommandLine>();

	        _lines.Add(cmdLine);
	        cmdLine.Init(_prompt, IgnoredChars);
            HandleLinePositioning();
            SetPenultimateLineToReadOnly();
            RepositionLines();
            UnsubscribeFromValueChangedEvent();
            SubscribeToValueChangedEvent();
        }

	    private void SetPenultimateLineToReadOnly() {
		    if (PenultimateLine == null) {
			    return;
		    }
		    PenultimateLine.SetReadOnly();
	    }

	    private void UnsubscribeFromValueChangedEvent() {
	        if (PenultimateLine == null) {
		        return;
	        }
            PenultimateLine.onValueChanged.RemoveAllListeners();
        }

        private void SubscribeToValueChangedEvent() {
	        if (CurrentLine == null) {
		        return;
	        }
            CurrentLine.onValueChanged.AddListener(InputField_OnValueChanged);
        }

        private void InputField_OnValueChanged(string text) {
            var commandString = StripPrompt(text);
            var args = new LineValueChangedEventArgs(commandString);
            InvokeLineValueChangedEvent(args);
        }

        private string StripPrompt(string text) {
            if (text.Length <= _prompt.Length) {
                return string.Empty;
            }
            var commandString = text.Substring(_prompt.Length);
            return commandString;
        }

        #endregion

        public void SetFocus() {
            CurrentLine.GetFocus();
            CurrentLine.MoveCaretToEnd();
        }

        public void SetCommandString(string text) {
            CurrentLine.SetCommandString(text);
        }
    }
}