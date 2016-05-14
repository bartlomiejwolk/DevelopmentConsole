using UnityEngine;
using UnityEngine.UI;

namespace DevelopmentConsole {

    public class CommandLine {

        private readonly InputField inputField;
        private RectTransform rectTransform;

        public float Height {
            get { return rectTransform.rect.height; }
        }

        public CommandLine(InputField inputField) {
            this.inputField = inputField;

            Init();
        }

        private void Init() {
            if (inputField != null) {
                rectTransform = inputField.GetComponent<RectTransform>();
            }
        }

    }

}