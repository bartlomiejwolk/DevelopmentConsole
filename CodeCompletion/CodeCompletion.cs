using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
#pragma warning disable 649

namespace DevelopmentConsoleTool.CodeCompletion {
    
    public class CodeCompletion : MonoBehaviour {
	    #region INSPECTOR

	    [SerializeField]
        private GameObject _optionTemplate;

	    [SerializeField]
	    private Transform _container;

	    [SerializeField]
	    private readonly Color _highlightedColor = Color.red;

	    #endregion

	    #region DELEGATES

	    public event EventHandler<SelectedOptionEventArgs> OptionSelected;
	    private Action<GameObject, string> _optionCreated;
	    private Action _tabKeyPressed;
	    private Action _returnKeyPressed;

	    #endregion

	    private readonly List<GameObject> _options = new List<GameObject>();
        private int _activeOptionIndex;
        private Color _inactiveOptionColor = Color.white;
	    private readonly PositionCalculator _positionCalculator
			= new PositionCalculator();
	    private Text _target;

	    #region PROPERTIES

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
			    return _options[_activeOptionIndex];
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
			    if (_activeOptionIndex > 0) {
				    result = _activeOptionIndex - 1;
				    return result;
			    }
			    result = _options.Count - 1;
			    return result;
		    }
	    }

	    #endregion

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

	    public void DisplayOptions(List<string> options, Text target) {
		    _target = target;
		    ClearResults();
		    if (options == null) {
			    return;
		    }
		    foreach (var option in options) {
			    CreateOption(option);
		    }
		    PositionOnScreen();
	    }

	    public void ClearResults() {
		    foreach (var child in _container) {
			    var childTransform = (Transform) child;
			    Destroy(childTransform.gameObject);
		    }
		    _activeOptionIndex = 0;
		    _options.Clear();
	    }

	    private void CreateOption(string option) {
		    var optionGo = Instantiate(_optionTemplate);
		    optionGo.transform.SetParent(_container);
		    optionGo.SetActive(true);

		    _optionCreated(optionGo, option);
	    }

	    private void PositionOnScreen() {
		    var position = _positionCalculator.CalculatePosition(
			    _target,
			    ContainerHeight);
		    _container.transform.position = position;
	    }

	    private void HandleInput() {
		    if (Input.GetKeyDown(KeyCode.Tab)) {
			    _tabKeyPressed();
		    }
		    if (Input.GetKeyDown(KeyCode.Return)) {
			    _returnKeyPressed();
		    }
	    }

	    private void CacheOptionBgColor() {
            var imageCo = _optionTemplate.GetComponent<Image>();
            _inactiveOptionColor = imageCo.color;
        }

	    protected virtual void InvokeOptionSelected(SelectedOptionEventArgs e) {
		    var handler = OptionSelected;
		    if (handler != null) handler(this, e);
	    }

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

	    #region INPUT

        private void OnTabKeyPressed() {
            if (_activeOptionIndex < _options.Count - 1) {
                _activeOptionIndex++;
            }
            else if (_activeOptionIndex == _options.Count - 1) {
                _activeOptionIndex = 0;
            }

            HighlightOption(_activeOptionIndex);
            UnhighlightOption(PreviousOptionIndex);
        }

        private void OnReturnKeyPressed() {
            if (_options.Count == 0) {
                return;
            }
            var args = new SelectedOptionEventArgs(CurrentOptionLabel.text);
            InvokeOptionSelected(args);

            ClearResults();
        }

        #endregion

	    #region EVENT HANDLERS

        private void OnOptionCreated(GameObject option, string text) {
            _options.Add(option);
            HighlightOption(_activeOptionIndex);

            // update label
            var textCo = option.GetComponentInChildren<Text>();
            textCo.text = text;
        }

        #endregion
    }
}