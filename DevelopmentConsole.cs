using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Assets.Extensions.DevelopmentConsole {

    public class DevelopmentConsole : MonoBehaviour {

        [SerializeField]
        private InputField commandLine;

        private const string prompt = "> ";

        private void Awake() {
            Assert.IsNotNull(commandLine);

            commandLine.text = prompt;
        }
    }
}