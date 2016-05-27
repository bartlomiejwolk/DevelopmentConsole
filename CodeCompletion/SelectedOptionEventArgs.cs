using System;

namespace DevelopmentConsoleTool {

    public class SelectedOptionEventArgs : EventArgs {
        
        public string Option { get; private set; }
            
        public SelectedOptionEventArgs(string option) {
            Option = option;
        }
    }
}