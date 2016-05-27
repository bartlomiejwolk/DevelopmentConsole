using System;

namespace DevelopmentConsoleTool {
    public class LineValueChangedEventArgs : EventArgs {
        
        public string Value { get; private set; }

        public LineValueChangedEventArgs(string value) {
            Value = value;
        }
    }
}