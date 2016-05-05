﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
#pragma warning disable 169
#pragma warning disable 649

namespace Assets.Extensions.DevelopmentConsole {

    public class LineManager : MonoBehaviour {

        [SerializeField]
        [CanBeNull]
        private GameObject commandLineTemplate;

        [SerializeField]
        private Transform container;

        private const string prompt = "> ";

        private Action returnKeyPressed;
        private List<CommandLine> lines;
        private InputField activeLine;

        private void Awake() {
            // todo extract
            Assert.IsNotNull(commandLineTemplate);
            // ReSharper disable once PossibleNullReferenceException
            var inputFieldComponent = commandLineTemplate.GetComponent<CustomInputField>(); 
            Assert.IsNotNull(inputFieldComponent);

            lines = new List<CommandLine>();
        
            inputFieldComponent.text = prompt;
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
            var inputFieldGo = InstantiateNewInputField();
            var inputField = inputFieldGo.GetComponent<InputField>();
            var cmdLine = new CommandLine(inputField);
            lines.Add(cmdLine);
            activeLine = inputField;
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
            // todo get input field height instead of using hardcoded values
            // todo create lineHeight variable
            // todo create existingLinesTotalHeight
            var verticalOffset = (lines.Count * 30) - 15;
            return -verticalOffset;
        }

    }

}