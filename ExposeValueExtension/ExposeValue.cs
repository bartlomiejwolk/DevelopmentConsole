namespace DevelopmentConsoleTool.ExposeValueExtension {
    public class ExposeValue {

        private static readonly ExposeValue _instance
            = new ExposeValue();

        public static ExposeValue Instance {
            get { return _instance; }
        }

        private ExposeValue() { }

        public void RegisterValue(string customName, string category, object value) {
            
        }

        public void ShowValue(string valueName) {
            
        }

        public void HideValue(string valueName) {
            
        }
    }
}