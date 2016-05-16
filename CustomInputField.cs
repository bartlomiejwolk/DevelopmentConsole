using System.Collections;
using UnityEngine.UI;

namespace DevelopmentConsoleTool {

    public class CustomInputField : InputField {

	    protected override void OnEnable() {
		    base.OnEnable();
		}

	    protected override void Start() {
			ActivateInputField();
			StartCoroutine(MoveTextEnd_NextFrame());
		}

	    public void MoveCarretToEnd() {
			StartCoroutine(MoveTextEnd_NextFrame());
		}

		IEnumerator MoveTextEnd_NextFrame() {
            yield return 0; 
            MoveTextEnd(false);
        }

    }

}