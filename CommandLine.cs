using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 649

namespace DevelopmentConsoleTool {

    /// <summary>
    /// Class to be attached to a line prefab.
    /// </summary>
    [RequireComponent(typeof(InputField))]
    public class CommandLine : MonoBehaviour {

        [SerializeField]
        private string prompt = "> ";

        // cache
        public RectTransform RectTransform { get; private set; }
        private CustomInputField inputField;

        private CustomInputField InputField {
            get {
                if (inputField != null) {
                    return inputField;
                }
                var component = GetComponent<CustomInputField>();
                return component;
            }
        }

        public float Height {
            get { return RectTransform.rect.height; }
        }

        public string Text {
            get { return InputField.text; }
            set { InputField.text = value; }
        }

        private void Awake() {
            Init();
        }

        public void GetFocus() {
            InputField.ActivateInputField();
        }

        public void MoveCaretToEnd() {
            InputField.MoveCaretToEnd();
        }

        private void Init() {
            InputField.Prompt = prompt;
            RectTransform = GetComponent<RectTransform>();
        }

        public void SetIgnoredChars(string chars) {
            InputField.IgnoredChars = chars;
        }

        public void SetReadOnly() {
            InputField.readOnly = true;
        }

        public string GetCommandString() {
            var cmdString = Text.Substring(prompt.Length);
            return cmdString;
        }

        public void SetCommandString(string cmd) {
            var result = prompt + cmd;
            Text = result;
        }
    }

}