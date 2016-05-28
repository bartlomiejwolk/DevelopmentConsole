using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
#pragma warning disable 649

namespace DevelopmentConsoleTool.CodeCompletion {
    
    public class CodeCompletion : MonoBehaviour {

        public event EventHandler<SelectedOptionEventArgs> OptionSelected;

        [SerializeField]
        private GameObject _optionTemplate;

        [SerializeField]
        private Transform _container;

        [SerializeField]
        private readonly Color _highlightedColor = Color.red;

        private readonly List<GameObject> _options = new List<GameObject>();
        private int _activeOption;
        private Color _inactiveOptionColor = Color.white;
	    private readonly PositionCalculator _positionCalculator
			= new PositionCalculator();

		// helper
	    private Text _target;

        #region EVENTS

        private Action<GameObject, string> _optionCreated;
        private Action _tabKeyPressed;
        private Action _returnKeyPressed;

        #endregion

	    public float ContainerHeight {
		    get {
				Canvas.ForceUpdateCanvases();
			    var rt = _container.GetComponent<RectTransform>();
			    var height = rt.rect.height;
			    return height;
		    }    
	    }

        public bool IsOpen {
            get { return _options.Count > 0; }
        }

        private GameObject ActiveOption {
            get {
                if (_options.Count == 0)
                {
                    return null;
                }
                return _options[_activeOption];
            }
        }

        private Text CurrentOptionLabel {
            get {
                if (ActiveOption == null) {
                    return null;
                }
                var textCo = ActiveOption.GetComponentInChildren<Text>();
                return textCo;
            }
        }

        private int PreviousOptionIndex {
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
            Assert.IsNotNull(_container);

            _optionCreated = OnOptionCreated;
            _tabKeyPressed = OnTabKeyPressed;
            _returnKeyPressed = OnReturnKeyPressed;
        }

        private void Start() {
            CacheOptionBgColor();
        }

        private void Update() {
            HandleInput();
        }

        #endregion

        private void CacheOptionBgColor() {
            var imageCo = _optionTemplate.GetComponent<Image>();
            _inactiveOptionColor = imageCo.color;
        }

        private void HandleInput() {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                _tabKeyPressed();
            }
            if (Input.GetKeyDown(KeyCode.Return)) {
                _returnKeyPressed();
            }
        }

        public void DisplayOptions(List<string> options, Text target) {
	        _target = target;
            CleanResults();
            if (options == null) {
                return;
            }
			foreach (var option in options) {
                CreateOption(option);
            }
			PositionOnScreen();
        }

        private void CleanResults() {
            foreach (var child in _container) {
                var childTransform = (Transform) child;
                Destroy(childTransform.gameObject);
            }
            _activeOption = 0;
            _options.Clear();
        }

        private void CreateOption(string option) {
            var optionGo = Instantiate(_optionTemplate);
            optionGo.transform.SetParent(_container);
            optionGo.SetActive(true);

            _optionCreated(optionGo, option);
        }

        #region INPUT HANDLERS

        private void OnTabKeyPressed() {
            if (_activeOption < _options.Count - 1) {
                _activeOption++;
            }
            else if (_activeOption == _options.Count - 1) {
                _activeOption = 0;
            }

            HighlightOption(_activeOption);
            UnhighlightOption(PreviousOptionIndex);
        }

        private void OnReturnKeyPressed() {
            if (_options.Count == 0) {
                return;
            }
            var args = new SelectedOptionEventArgs(CurrentOptionLabel.text);
            InvokeOptionSelected(args);

            CleanResults();
        }

        #endregion

        protected virtual void InvokeOptionSelected(SelectedOptionEventArgs e) {
            var handler = OptionSelected;
            if (handler != null) handler(this, e);
        }

        #region EVENT HANDLERS

        private void OnOptionCreated(GameObject option, string text) {
            _options.Add(option);
            HighlightOption(_activeOption);

            // update label
            var textCo = option.GetComponentInChildren<Text>();
            textCo.text = text;
        }

        #endregion

        private void HighlightOption(int index) {
            if (_options.Count == 0) {
                return;
            }
            var option = _options[index];
            var imageCo = option.GetComponent<Image>();
            imageCo.color = _highlightedColor;
        }

        private void UnhighlightOption(int index) {
            if (_options.Count <= 1) {
                return;
            }
            var option = _options[index];
            var imageCo = option.GetComponent<Image>();
            imageCo.color = _inactiveOptionColor;
        }

	    private void PositionOnScreen() {
		    var position = _positionCalculator.CalculatePosition(
				_target,
				ContainerHeight);
		    _container.transform.position = position;
	    }
    }
}