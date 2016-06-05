using System;

namespace DevelopmentConsole.Core.LineManagerModule {

	public class LineValueChangedEventArgs : EventArgs {
        
        public string Value { get; set; }
    }
}