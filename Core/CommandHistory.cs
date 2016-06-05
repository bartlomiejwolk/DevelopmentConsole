using System.Collections.Generic;

namespace DevelopmentConsole.Core {

    public class CommandHistory {

        private readonly List<string> _history = new List<string>();
        private int _commandNumber;

        private bool HasPreviousCommand {
            get { return _commandNumber > 0; }
        }

        private bool HasNextCommand {
            get { return _history.Count > 0 &&
                _commandNumber < _history.Count - 1; }
        }

        public void AddCommand(string cmd) {
            if (cmd.Trim().Length == 0) {
                return;
            }
            _history.Add(cmd);
            _commandNumber = _history.Count;
        }

        public string GetPreviousCommand() {
            if (!HasPreviousCommand) {
                return string.Empty;
            }
            _commandNumber -= 1;
            var cmd = _history[_commandNumber];
            return cmd;
        }

        public string GetNextCommand() {
            if (!HasNextCommand) {
                return string.Empty;
            }
            _commandNumber += 1;
            var cmd = _history[_commandNumber];
            return cmd;
        }
    }
}