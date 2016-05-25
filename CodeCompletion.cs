using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 649

namespace DevelopmentConsoleTool {
    
    public class CodeCompletion : MonoBehaviour {

        [SerializeField]
        private GameObject _optionTemplate;

        public void DisplayResults(List<Match> options) {
            foreach (var option in options) {
                CreateOption(option);
            }
        }

        private void CreateOption(Match option) {
            var optionGo = Instantiate(_optionTemplate);
            optionGo.transform.SetParent(transform);
            optionGo.SetActive(true);

            var textCo = optionGo.GetComponent<Text>();
            textCo.text = option.TextValue;
        }
    }
}