using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 649

namespace DevelopmentConsoleTool {
    
    // todo rename to CodeCompletionManager
    public class CodeCompletion : MonoBehaviour {

        [SerializeField]
        private GameObject _optionTemplate;

        private List<GameObject> _options = new List<GameObject>();
        private Action<GameObject, Match> _optionCreated;

        private void Awake() {
            _optionCreated += OnOptionCreated;
        }

        private void OnOptionCreated(GameObject option, Match info) {
            _options.Add(option);

            // update label
            var textCo = option.GetComponentInChildren<Text>();
            textCo.text = info.TextValue;
        }

        public void DisplayResults(List<Match> options) {
            CleanResults();

            if (options == null) {
                return;
            }

            foreach (var option in options) {
                CreateOption(option);
            }
        }

        private void CleanResults() {
            foreach (var child in transform) {
                var childTransform = (Transform) child;

                // option template
                if (!childTransform.gameObject.activeSelf) {
                    continue;
                }

                Destroy(childTransform.gameObject);
            }
        }

        private void CreateOption(Match matchInfo) {
            var optionGo = Instantiate(_optionTemplate);
            optionGo.transform.SetParent(transform);
            optionGo.SetActive(true);

            _optionCreated(optionGo, matchInfo);
        }
    }
}