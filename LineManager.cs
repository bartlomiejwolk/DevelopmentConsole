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
        public string IgnoredChars { get; set; }

        [SerializeField]
        private CommandLine firstLine;

        [SerializeField]
        private GameObject commandLineTemplate;

        private readonly List<CommandLine> lines = new List<CommandLine>();

        public CommandLine LastLine {
            get { return lines.LastOrDefault(); }
        }

        public CommandLine PenultimateLine
        {
            get {
                if (lines.Count > 1) {
                    var penultimate = lines[lines.Count - 2];
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
            Assert.IsNotNull(commandLineTemplate);
            Assert.IsNotNull(firstLine);

            LineInstantiated += LineInstantiatedHandler;
            lines.Add(firstLine);
        }

        private void OnEnable() {
            LastLine.GetFocus();
            LastLine.MoveCaretToEnd();
        }

        private void Start() {
            firstLine.SetIgnoredChars(IgnoredChars);
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
            var cmdLineGo = Instantiate(commandLineTemplate);
            cmdLineGo.gameObject.SetActive(true);
            cmdLineGo.transform.SetParent(transform, false);

            RaiseLineInstantiatedEvent(cmdLineGo);
        }

        private void PositionLine() {
            var newLineRectTransform = LastLine.GetComponent<RectTransform>();
            var targetPos = CalculateLinePosition();
            newLineRectTransform.anchoredPosition = targetPos;
        }

        // calculates position to place line on the screen
        private Vector2 CalculateLinePosition() {
            var verticalOffset = (PenultimateLine.Height / 2) + (LastLine.Height / 2);
            var verticalPos = PenultimateLine.RectTransform.anchoredPosition.y - verticalOffset;
            var newPos = new Vector2(
                PenultimateLine.RectTransform.anchoredPosition.x,
                verticalPos);

            return newPos;
        }

        #region EVENT INVOCATORS

        protected virtual void RaiseLineInstantiatedEvent(GameObject instantiatedGo) {
            var handler = LineInstantiated;
            var args = new LineInstantiatedEventArgs(instantiatedGo);
            if (handler != null) handler(this, args);
        }

        #endregion

        #region EVENT HANDLERS

        private void LineInstantiatedHandler(object sender, LineInstantiatedEventArgs eventArgs) {
            var go = eventArgs.InstantiatedGo;
            var cmdLine = go.GetComponent<CommandLine>();

            lines.Add(cmdLine);
            cmdLine.SetIgnoredChars(IgnoredChars);
            PositionLine();
            PenultimateLine.SetReadOnly();
            RepositionLines();
        }

        #endregion

        public void GetFocus() {
            LastLine.GetFocus();
        }

        public void SetText(string text) {
            throw new NotImplementedException();
        }
    }

    public class LineInstantiatedEventArgs : EventArgs {

        public GameObject InstantiatedGo { get; private set; }

        public LineInstantiatedEventArgs(GameObject instantiatedGo) {
            InstantiatedGo = instantiatedGo;
        }
    }

}