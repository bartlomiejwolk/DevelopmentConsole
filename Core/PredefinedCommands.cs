using DevelopmentConsole.Core.CommandHandlerSystem;
using UnityEngine;

namespace DevelopmentConsole.Core {
    public class PredefinedCommands : MonoBehaviour {
        protected virtual void Start() {
            CommandHandlerManager.Instance.RegisterCommandHandlers(
                typeof(PredefinedCommands), this);
        }

        [CommandHandler]
        private void SetTimeScale(float scale) {
            Time.timeScale = scale;
            Debug.Log("Timescale set to " + Time.timeScale);
        }
    }
}