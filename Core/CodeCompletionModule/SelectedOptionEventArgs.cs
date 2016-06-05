using System;

namespace DevelopmentConsole.Core.CodeCompletionModule {
    public class SelectedOptionEventArgs : EventArgs {
        public string Option { get; set; }
    }
}