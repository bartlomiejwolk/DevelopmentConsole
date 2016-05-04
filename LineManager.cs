using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Extensions.DevelopmentConsole {

    public class LineManager : MonoBehaviour {

        [SerializeField]
        private CustomInputField commandLineTemplate;

        private const string prompt = "> ";

        private Action returnKeyPressed;
        private List<InputField> lines;
        private InputField activeLine;

        private void Awake() {
            Assert.IsNotNull(commandLineTemplate);

            lines = new List<InputField>();
            commandLineTemplate.text = prompt;
            returnKeyPressed += CreateNewInputLine;
        }

        private void Start() {
            CreateNewInputLine();
        }

        private void Update() {
            CheckForReturnKey();
        }

        private void CheckForReturnKey() {
            if (Input.GetKeyDown(KeyCode.Return)) {
                returnKeyPressed();
            }
        }

        // todo rename to CreateNewCmdLine
        private void CreateNewInputLine() {
            var cmdLine = InstantiateNewLine();
            lines.Add(cmdLine);
            activeLine = cmdLine;
            SetActiveLineVerticalPosition();
        }

        private CustomInputField InstantiateNewLine() {
            var cmdLine = Instantiate(commandLineTemplate);
            cmdLine.gameObject.SetActive(true);
            cmdLine.transform.SetParent(commandLineTemplate.transform.parent, false);
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
            // todo get input field height instead of using hardcoded values
            var verticalOffset = (lines.Count * 30) - 15;
            return -verticalOffset;
        }

    }

}