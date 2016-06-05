using UnityEngine;
using DevelopmentConsoleTool.CommandHandlerSystem;

namespace DevelopmentConsoleTool {

    public class PredefinedCommands : MonoBehaviour {

        private void Start() {
            CommandHandlerManager.Instance.RegisterCommandHandlers(typeof(PredefinedCommands), this);
        }

		// todo create ICommandHandlerInfo with AutoCompleteNames property. Pass it as arg. to CommandHandler attr. ValueExposer could implement this interface.
        [CommandHandler]
        private void ExposeValue(string valueName) {
	        var instance = ValueExposerExtension.ValueExposer.Instance;
			if (instance != null) {
				instance.ShowValue(valueName);
			}
		}

	    [CommandHandler]
	    private void HideExposedValue(string valueName) {
			var instance = ValueExposerExtension.ValueExposer.Instance;
			if (instance != null)
			{
				instance.HideValue(valueName);
			}
		}
    }
}
