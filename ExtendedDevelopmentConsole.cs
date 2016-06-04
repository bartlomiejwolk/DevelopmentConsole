using System.Linq;
using DevelopmentConsoleTool.ValueExposerExtension;

namespace DevelopmentConsoleTool {

	public class ExtendedDevelopmentConsole : DevelopmentConsole {

		protected override void LineManager_OnLineValueChanged(
			object sender, LineValueChangedEventArgs eventArgs) {

			base.LineManager_OnLineValueChanged(sender, eventArgs);
			DisplayValueExposerAutoCompletionPanel(eventArgs.Value);
		}

		private void DisplayValueExposerAutoCompletionPanel(string input) {
			// todo in PredefinedCommands create enum with command names.
			if (TypedCommand != "exposevalue") {
				return;
			}
			CodeCompletion.ClearResults();

			var names = ExposedValueManager.Instance.GetValueNames();
			var arg = Arguments[0];
			var matches = FuzzySearch.MatchResultSet(names, arg);
			if (matches == null)
			{
				return;
			}
			LineManager.CurrentLine.ForceLabelUpdate();
			var options = matches.Select(match => match.TextValue).ToList();
			var textCo = LineManager.CurrentLine.textComponent;
			CodeCompletion.DisplayOptions(options, textCo);
		}
	}
}