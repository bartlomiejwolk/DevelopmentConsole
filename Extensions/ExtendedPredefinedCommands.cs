using DevelopmentConsoleTool.CommandHandlerSystem;

namespace DevelopmentConsoleTool {

	public class ExtendedPredefinedCommands : PredefinedCommands {

		protected override void Start() {
			base.Start();
			CommandHandlerManager.Instance.RegisterCommandHandlers(
				typeof(ExtendedPredefinedCommands), this);
		}

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
			if (instance != null) {
				instance.HideValue(valueName);
			}
		}
	}
}