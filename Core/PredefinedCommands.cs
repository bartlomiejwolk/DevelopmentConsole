using DevelopmentConsole.Core.CommandHandlerSystem;
using UnityEngine;

namespace DevelopmentConsole.Core {

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
