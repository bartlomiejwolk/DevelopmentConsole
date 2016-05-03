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
            //commandLine.MoveTextEnd(false);
        }

        private void Start() {
            commandLine.onEndEdit.AddListener(OnEndEdit);
            CreateNewInputLine();
        }

        private void OnEndEdit(string text) {
            Debug.Log("End edit.");
        }

        private void CreateNewInputLine() {
            var cmdLine = Instantiate(commandLine);
            cmdLine.gameObject.SetActive(true);
            cmdLine.transform.SetParent(commandLine.transform.parent, false);
        }
    }
}