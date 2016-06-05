using UnityEngine;
using DevelopmentConsoleTool.CommandHandlerSystem;

namespace DevelopmentConsoleTool {

    public class PredefinedCommands : MonoBehaviour {

        protected virtual void Start() {
            CommandHandlerManager.Instance.RegisterCommandHandlers(
				typeof(PredefinedCommands), this);
        }

	    [CommandHandler]
	    private void TimeScale(float scale) {
		    Time.timeScale = scale;
	    }
    }
}
