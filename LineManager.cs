using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
#pragma warning disable 169
#pragma warning disable 649

namespace DevelopmentConsoleTool {

    /// <summary>
    /// Class responsible for visual handling of the command lines.
    /// </summary>
    public class LineManager : MonoBehaviour {

        public event EventHandler<LineInstantiatedEventArgs> LineInstantiated;

        public CommandLine LastLine {
            get { return lines.LastOrDefault(); }
        }

        public CommandLine PenultimateLine
        {
            get {
                if (lines != null && lines.Count > 1) {
                    var penultimate = lines[lines.Count - 2];
                    return penultimate;
                }
                return null;
            }
        }

	    public string CommandString {
		    get { return LastLine.Text; }
	    }

	    [SerializeField]
        [CanBeNull]
        private GameObject commandLineTemplate;

        [SerializeField]
        private Transform container;

        private const string Prompt = "> ";

        // new lines will be added here right after being instantiated
        private List<CommandLine> lines;

        private void Awake() {
            // todo extract
            Assert.IsNotNull(commandLineTemplate);
            LineInstantiated += OnLineInstantiated;
            

            lines = new List<CommandLine>();
        }

        private void Start() {
        }

        private void Update() {
        }

        /// <summary>
        /// Handles all the stuff related to creation of a new command line
        /// </summary>
        public void AddNewLine() {
            InstantiateLine();
            RepositionLines();
        }

        private void RepositionLines() {
            // calculate offset
            var correctPos = LastLine.Height/2;
            var pos = LastLine.transform.position.y;
            var offset = pos - correctPos;
            if (offset > 0) {
                return;
            }

            // update lines container position
            var currentVerticalPos = container.position.y;
            var newVerticalPos = currentVerticalPos + Mathf.Abs(offset);
            container.position = new Vector3(container.position.x, newVerticalPos, container.position.z);
        }

        private void InstantiateLine() {
            var cmdLineGo = Instantiate(commandLineTemplate);
            cmdLineGo.gameObject.SetActive(true);
            cmdLineGo.transform.SetParent(container, false);

            OnLineInstantiated(cmdLineGo);
        }

        // moves line to correct place on the screen
        private void PositionLine() {
            var newLineRectTransform = LastLine.GetComponent<RectTransform>();
            var targetPos = CalculateLinePosition();

            newLineRectTransform.anchoredPosition = targetPos;
        }

        // calculates position to place line on the screen
        private Vector2 CalculateLinePosition() {
            // first line case
            if (lines.Count == 1) {
                var pos = CalculatePositionForFirstLine();
                return pos;
            }

            var newPos = CalculatePositionForNonFirstLine();

            return newPos;
        }

        private Vector2 CalculatePositionForFirstLine() {
            var rectTransform = LastLine.GetComponent<RectTransform>();
            var lineHeight = rectTransform.sizeDelta.y;
            var verticalPos = -(lineHeight/2);

            var pos = new Vector2(
                rectTransform.anchoredPosition.x,
                verticalPos);

            return pos;
        }

        private Vector2 CalculatePositionForNonFirstLine() {
            // previous line height
            var prevLineRect = PenultimateLine.RectTransform;
            var prevLinePos = prevLineRect.anchoredPosition;
            var prevLineHeight = prevLineRect.sizeDelta.y;

            // new line height
            var currentLineRect = LastLine.GetComponent<RectTransform>();
            var currentLineHeight = currentLineRect.sizeDelta.y;

            // calculate position
            var verticalOffset = (prevLineHeight/2) + (currentLineHeight/2);
            var verticalPos = prevLinePos.y - verticalOffset;
            var newPos = new Vector2(
                prevLineRect.anchoredPosition.x,
                verticalPos);

            return newPos;
        }

        #region EVENT INVOCATORS

        protected virtual void OnLineInstantiated(GameObject instantiatedGo) {
            var handler = LineInstantiated;
            var args = new LineInstantiatedEventArgs(instantiatedGo);
            if (handler != null) handler(this, args);
        }

        #endregion

        #region EVENT HANDLERS

        private void OnLineInstantiated(object sender, LineInstantiatedEventArgs eventArgs) {
            var go = eventArgs.InstantiatedGo;
            var cmdLine = go.GetComponent<CommandLine>();

            lines.Add(cmdLine);
            PositionLine();
        }

        #endregion
    }

    public class LineInstantiatedEventArgs : EventArgs {
        public GameObject InstantiatedGo { get; private set; }

        public LineInstantiatedEventArgs(GameObject instantiatedGo) {
            this.InstantiatedGo = instantiatedGo;
        }
    }

}