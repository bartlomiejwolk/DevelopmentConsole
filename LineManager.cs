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

        public event EventHandler LineInstantiated;

        [SerializeField]
        [CanBeNull]
        private GameObject commandLineTemplate;

        [SerializeField]
        private Transform container;

        private const string Prompt = "> ";

        private List<CommandLine> lines;
        private InputField activeLine;

        private void Awake() {
            // todo extract
            Assert.IsNotNull(commandLineTemplate);
            // ReSharper disable once PossibleNullReferenceException
            var inputFieldComponent = commandLineTemplate.GetComponent<CustomInputField>(); 
            Assert.IsNotNull(inputFieldComponent);

            lines = new List<CommandLine>();
        
            inputFieldComponent.text = Prompt;
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

            // Position line after the last line
            PositionCurrentLineAtEnd();

            // Reposition all lines

            // todo delete
            var inputFieldGo = InstantiateInputField();

            var inputFieldCo = inputFieldGo.GetComponent<InputField>();
            var cmdLine = new CommandLine();

            lines.Add(cmdLine);
            activeLine = inputFieldCo;

            // todo should accept line as arg.
            SetActiveLineVerticalPosition();
        }

        private void PositionCurrentLineAtEnd() {
            throw new System.NotImplementedException();
        }
        
        private GameObject InstantiateInputField() {
            var cmdLine = Instantiate(commandLineTemplate);
            cmdLine.gameObject.SetActive(true);
            cmdLine.transform.SetParent(container, false);
            OnLineInstantiated();
            return cmdLine;
        }

        private void SetActiveLineVerticalPosition() {
            var cmdLineRect = activeLine.GetComponent<RectTransform>();
            if (cmdLineRect != null) {
                var verticalPos = CalculatePositionForNewLine();
                cmdLineRect.anchoredPosition = new Vector2(cmdLineRect.anchoredPosition.x, verticalPos);
            }
        }

        private int CalculatePositionForNewLine() {
            // calculate new position below the current line

            // check if new line fits inside the screen

            // if not, move all other lines up

            // todo get input field height instead of using hardcoded values
            // todo create lineHeight variable
            // todo create existingLinesTotalHeight
            var lastLine = lines.Last();
            var verticalOffset = (lines.Count * 30) - 15;
            return -verticalOffset;
        }

        #region EVENT INVOCATORS

        protected virtual void OnLineInstantiated() {
            var handler = LineInstantiated;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        #endregion
    }

}