using System.Collections.Generic;

namespace DevelopmentConsoleTool {

    public class CommandHistory {

        private readonly List<string> history = new List<string>();
        private int commandNumber;

        private bool HasPreviousCommand {
            get { return commandNumber > 0; }
        }

        private bool HasNextCommand {
            get { return history.Count > 0 &&
                commandNumber < history.Count - 1; }
        }

        public void AddCommand(string cmd) {
            if (cmd.Trim().Length == 0) {
                return;
            }
            history.Add(cmd);
            commandNumber = history.Count;
        }

        public string GetPreviousCommand() {
            if (!HasPreviousCommand) {
                return string.Empty;
            }
            commandNumber -= 1;
            var cmd = history[commandNumber];
            return cmd;
        }

        public string GetNextCommand() {
            if (!HasNextCommand) {
                return string.Empty;
            }
            // todo handle case when the first command in history should be returned
            commandNumber += 1;
            var cmd = history[commandNumber];
            return cmd;
        }
    }
}