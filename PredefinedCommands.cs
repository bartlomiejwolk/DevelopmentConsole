using UnityEngine;
using System.Collections;
using DevelopmentConsoleTool.CommandHandlerSystem;

namespace DevelopmentConsoleTool {
    public class PredefinedCommands : MonoBehaviour {

        private void Start() {
            CommandHandlerManager.Instance.RegisterCommandHandlers(typeof(CommandHanderTest), this);
        }

        [CommandHandler]
        private void ExposeValue(string valueName) {
            ExposeValueExtension.ExposeValue.Instance.ShowValue(valueName);
        }
    }
}
