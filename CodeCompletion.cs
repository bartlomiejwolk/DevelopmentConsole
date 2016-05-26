using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
#pragma warning disable 649

namespace DevelopmentConsoleTool {
    
    // todo rename to CodeCompletionManager
    public class CodeCompletion : MonoBehaviour {

        [SerializeField]
        private GameObject _optionTemplate;

        private readonly List<GameObject> _options = new List<GameObject>();
        private int _activeOption;

        #region EVENTS

        private Action<GameObject, Match> _optionCreated;

        private Action _tabKeyPressed;

        #endregion

        private int PreviousOption {
            get {
                int result;
                if (_activeOption > 0) {
                    result = _activeOption - 1;
                    return result;
                }
                result = _options.Count - 1;
                return result;
            }
        }

        #region UNITY MESSAGES

        private void Awake() {
            Assert.IsNotNull(_optionTemplate);

            _optionCreated += OnOptionCreated;
            _tabKeyPressed += OnTabKeyPressed;
        }

        private void Start() {
            // todo read image color and cache. Use for unhighlighting option
        }

        private void Update() {
            HandleInput();
        }

        #endregion

        public void DisplayResults(List<Match> options) {
            if (options == null) {
                return;
            }

            CleanResults();
            foreach (var option in options) {
                CreateOption(option);
            }
        }

        private void CleanResults() {
            foreach (var child in transform) {
                var childTransform = (Transform) child;
                Destroy(childTransform.gameObject);
            }
            _activeOption = 0;
            _options.Clear();
        }

        private void CreateOption(Match matchInfo) {
            var optionGo = Instantiate(_optionTemplate);
            optionGo.transform.SetParent(transform);
            optionGo.SetActive(true);

            _optionCreated(optionGo, matchInfo);
        }

        #region New region

        private void OnTabKeyPressed() {
            // update active option index
            if (_activeOption < _options.Count - 1) {
                _activeOption++;
            }
            else if (_activeOption == _options.Count - 1) {
                _activeOption = 0;
            }

            HighlightOption(_activeOption);
            UnhighlightOption(PreviousOption);
        }

        #endregion

        private void HandleInput() {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                _tabKeyPressed();
            }
        }

        #region EVENT HANDLERS

        private void OnOptionCreated(GameObject option, Match info) {
            _options.Add(option);
            HighlightOption(_activeOption);

            // update label
            var textCo = option.GetComponentInChildren<Text>();
            textCo.text = info.TextValue;
        }

        #endregion

        private void HighlightOption(int index) {
            if (_options.Count == 0) {
                return;
            }

            var option = _options[index];
            var imageCo = option.GetComponent<Image>();
            // todo set color through inspector
            imageCo.color = Color.red;
        }

        private void UnhighlightOption(int index) {
            // if there's only one option, it should always be highlighted
            if (_options.Count <= 1) {
                return;
            }

            var option = _options[index];
            var imageCo = option.GetComponent<Image>();
            // todo set color through inspector
            imageCo.color = Color.white;
        }
    }
}