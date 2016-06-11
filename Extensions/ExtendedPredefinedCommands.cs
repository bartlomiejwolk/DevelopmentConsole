using DevelopmentConsole.Core;
using DevelopmentConsole.Core.CommandHandlerSystem;
using DevelopmentConsole.Extensions.ValueExposerModule.Core;

namespace DevelopmentConsole.Extensions {
    public class ExtendedPredefinedCommands : PredefinedCommands {
        protected override void Start() {
            base.Start();
            CommandHandlerManager.Instance.RegisterCommandHandlers(
                typeof(ExtendedPredefinedCommands), this);
        }

        [CommandHandler]
        private void ExposeValue(string valueName) {
            var instance = ValueExposer.Instance;
            if (instance != null) {
                instance.ShowValue(valueName);
            }
        }

        [CommandHandler]
        private void HideExposedValue(string valueName) {
            var instance = ValueExposer.Instance;
            if (instance != null) {
                instance.HideValue(valueName);
            }
        }
    }
}