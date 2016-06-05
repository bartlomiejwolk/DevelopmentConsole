using DevelopmentConsole.Core;
using DevelopmentConsole.Core.CommandHandlerSystem;

namespace DevelopmentConsole.Extensions {
    public class ExtendedPredefinedCommands : PredefinedCommands {
        protected override void Start() {
            base.Start();
            CommandHandlerManager.Instance.RegisterCommandHandlers(
                typeof(ExtendedPredefinedCommands), this);
        }

        [CommandHandler]
        private void ExposeValue(string valueName) {
            var instance = Extensions.ValueExposerModule.ValueExposer.Instance;
            if (instance != null) {
                instance.ShowValue(valueName);
            }
        }

        [CommandHandler]
        private void HideExposedValue(string valueName) {
            var instance = Extensions.ValueExposerModule.ValueExposer.Instance;
            if (instance != null) {
                instance.HideValue(valueName);
            }
        }
    }
}