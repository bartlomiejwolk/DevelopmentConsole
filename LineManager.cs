using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Extensions.DevelopmentConsole {

    public class LineManager : MonoBehaviour {

        [SerializeField]
        private CustomInputField commandLineTemplate;

        private const string prompt = "> ";

        private List<InputField> lines;

        private void Awake()
        {
            Assert.IsNotNull(commandLineTemplate);

            commandLineTemplate.text = prompt;
        }

        private void Start()
        {
            CreateNewInputLine();
        }

        private void OnEndEdit(string text)
        {
            CreateNewInputLine();
        }

        private void CreateNewInputLine()
        {
            var cmdLine = Instantiate(commandLineTemplate);
            cmdLine.gameObject.SetActive(true);
            cmdLine.transform.SetParent(commandLineTemplate.transform.parent, false);
            cmdLine.onEndEdit.AddListener(OnEndEdit);

            lines.Add(cmdLine);

            var cmdLineRect = cmdLine.GetComponent<RectTransform>();
            if (cmdLineRect != null) {
                var verticalPos = CalculatePositionForNewLine();
                cmdLineRect.anchoredPosition = new Vector2(cmdLineRect.anchoredPosition.x, verticalPos);
            }
        }

        private int CalculatePositionForNewLine() {
            throw new System.NotImplementedException();
        }

    }

}