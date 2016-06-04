using System.Collections.Generic;
using System.Linq;
using DevelopmentConsoleTool.CodeCompletionModule;
using DevelopmentConsoleTool.ValueExposerExtension;

namespace DevelopmentConsoleTool {

	public class ExtendedDevelopmentConsole : DevelopmentConsole {

		protected override void LineManager_OnLineValueChanged(
			object sender, LineValueChangedEventArgs eventArgs) {

			base.LineManager_OnLineValueChanged(sender, eventArgs);
			DisplayValueExposerAutoCompletionPanel();
		}

		private void DisplayValueExposerAutoCompletionPanel() {
			// todo in PredefinedCommands create enum with command names.
			if (TypedCommand != "exposevalue") {
				return;
			}
			CodeCompletion.ClearResults();

			var names = ExposedValueManager.Instance.GetValueNames();
			var arg = Arguments[0];
			var matches = FuzzySearch.MatchResultSet(names, arg);
			LineManager.CurrentLine.ForceLabelUpdate();
			List<string> options;
			if (matches == null) {
				options = names;
			}
			else {
				options = matches.Select(match => match.TextValue).ToList();
			}
			var textCo = LineManager.CurrentLine.textComponent;
			CodeCompletion.DisplayOptions(options, textCo);
		}

		protected override void CodeCompletion_OnOptionSelected(
			object sender, SelectedOptionEventArgs selectedOptionEventArgs) {

			var option = selectedOptionEventArgs.Option;
			string fullCmd;
			if (TypedCommand != null) {
				fullCmd = TypedCommand + " " + option;
			}
			else {
				fullCmd = option;
			}
			LineManager.SetCommandString(fullCmd);
			LineManager.SetFocus();
		}
	}
}