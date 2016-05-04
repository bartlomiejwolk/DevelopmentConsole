using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Extensions.DevelopmentConsole {

    public class DevelopmentConsole : MonoBehaviour {

        [SerializeField]
        private CustomInputField commandLine;

        private const string prompt = "> ";

        private void Awake() {
            Assert.IsNotNull(commandLine);

            commandLine.text = prompt;
        }

        private void Start() {
            CreateNewInputLine();
        }

        private void OnEndEdit(string text) {
            CreateNewInputLine();
        }

        private void CreateNewInputLine() {
            var cmdLine = Instantiate(commandLine);
            cmdLine.gameObject.SetActive(true);
            cmdLine.transform.SetParent(commandLine.transform.parent, false);
            cmdLine.onEndEdit.AddListener(OnEndEdit);
        }
    }
}