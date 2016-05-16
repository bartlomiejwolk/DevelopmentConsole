using System.Collections;
using UnityEngine.UI;

namespace DevelopmentConsoleTool {

    public class CustomInputField : InputField {

	    protected override void OnEnable() {
		    base.OnEnable();

			ActivateInputField();
			StartCoroutine(MoveTextEnd_NextFrame());
		}

		protected override void Start() {
		    base.Start();
        }

	    IEnumerator MoveTextEnd_NextFrame() {
            yield return 0; 
            MoveTextEnd(false);
        }

    }

}