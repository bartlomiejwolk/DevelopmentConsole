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

        public void AddNewLine() {
            var inputFieldGo = InstantiateNewInputField();

            var inputFieldCo = inputFieldGo.GetComponent<InputField>();
            var cmdLine = new CommandLine();

            lines.Add(cmdLine);
            activeLine = inputFieldCo;

            // todo should accept line as arg.
            SetActiveLineVerticalPosition();
        }

        private GameObject InstantiateNewInputField() {
            var cmdLine = Instantiate(commandLineTemplate);
            cmdLine.gameObject.SetActive(true);
            cmdLine.transform.SetParent(container, false);
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

    }

}