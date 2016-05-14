using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
#pragma warning disable 169
#pragma warning disable 649

namespace DevelopmentConsole {

    /// <summary>
    /// Class responsible for visual handling of the command lines.
    /// </summary>
    public class LineManager : MonoBehaviour {

        public event EventHandler<LineInstantiatedEventArgs> LineInstantiated;

        public CommandLine LastLine {
            get { return lines.LastOrDefault(); }
        }

        [SerializeField]
        [CanBeNull]
        private GameObject commandLineTemplate;

        [SerializeField]
        private Transform container;

        private const string Prompt = "> ";

        private List<CommandLine> lines;

        private void Awake() {
            // todo extract
            Assert.IsNotNull(commandLineTemplate);
            LineInstantiated += OnLineInstantiated;
            // ReSharper disable once PossibleNullReferenceException
            var inputFieldComponent = commandLineTemplate.GetComponent<CustomInputField>(); 
            Assert.IsNotNull(inputFieldComponent);

            lines = new List<CommandLine>();
        
            inputFieldComponent.text = Prompt;
        }

        private void OnLineInstantiated(object sender, LineInstantiatedEventArgs eventArgs) {
            // update active line
            var go = eventArgs.InstantiatedGo;
            var cmdLine = go.GetComponent<CommandLine>();
            lines.Add(cmdLine);
        }

        private void Start() {
        }

        private void Update() {
        }

        /// <summary>
        /// Handles all the stuff related to creation of a new command line
        /// </summary>
        public void AddNewLine() {
            // Instantiate new line from prefab
            InstantiateInputField();

            // Reposition all lines
        }

        private void InstantiateInputField() {
            var cmdLineGo = Instantiate(commandLineTemplate);
            cmdLineGo.gameObject.SetActive(true);
            cmdLineGo.transform.SetParent(container, false);
            var commandLineCo = cmdLineGo.GetComponent<CommandLine>();
            PositionLine(commandLineCo);
            OnLineInstantiated(cmdLineGo);
        }

        private void PositionLine(CommandLine commandLineCo) {
            var newLineRectTransform = commandLineCo.GetComponent<RectTransform>();
            var targetPos = CalculateLinePosition(commandLineCo);

            newLineRectTransform.anchoredPosition = targetPos;
        }

        private Vector2 CalculateLinePosition(CommandLine newLine) {
            // first line case
            if (LastLine == null) {
                var pos = GetPositionForFirstLine(newLine);
                return pos;
            }

            var newPos = CalculatePositionForNonFirstLine(newLine);

            return newPos;
        }

        private Vector2 GetPositionForFirstLine(CommandLine newLine) {
            var rectTransform = newLine.GetComponent<RectTransform>();
            var newLineHeight = rectTransform.sizeDelta.y;
            var verticalPos = newLineHeight/2;

            var pos = new Vector2(
                rectTransform.anchoredPosition.x,
                verticalPos);

            return pos;
        }

        private Vector2 CalculatePositionForNonFirstLine(CommandLine newLine) {
            // active line height
            var activeLineRectTransform = LastLine.RectTransform;
            var activeLinePos = activeLineRectTransform.anchoredPosition;
            var activeLineHeight = activeLineRectTransform.sizeDelta.y;

            // new line height
            var newLineRectTransform = newLine.GetComponent<RectTransform>();
            var newLineHeight = newLineRectTransform.sizeDelta.y;

            // calculate position
            var verticalOffset = (activeLineHeight/2) + (newLineHeight/2);
            var verticalPos = activeLinePos.y + verticalOffset;
            var newPos = new Vector2(
                activeLineRectTransform.anchoredPosition.x,
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
    }

    public class LineInstantiatedEventArgs : EventArgs {
        public GameObject InstantiatedGo { get; private set; }

        public LineInstantiatedEventArgs(GameObject instantiatedGo) {
            this.InstantiatedGo = instantiatedGo;
        }
    }

}