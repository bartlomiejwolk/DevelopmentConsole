using System.Collections.Generic;
using System.Linq;

namespace DevelopmentConsole.Core {
    public class CommandLineArgumentParser {
        public List<string> Arguments { get; set; }

        public void ParseArguments(string input) {
            var elements = input.Split(' ').ToList();
            // remove command
            Arguments = elements.GetRange(1, elements.Count - 1);
        }
    }
}