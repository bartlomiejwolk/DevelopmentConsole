using UnityEngine.UI;

namespace Assets.Extensions.DevelopmentConsole {

    public class CustomInputField : InputField {

        protected override void LateUpdate() {
            base.LateUpdate();

            MoveTextEnd(false);
        }

    }

}