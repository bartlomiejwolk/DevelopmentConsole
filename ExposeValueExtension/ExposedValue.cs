using System;
using UnityEngine;
using UnityEngine.UI;

namespace DevelopmentConsoleTool.ExposeValueExtension {
	public class ExposedValue {
		// todo change to properties
		private bool _updateEnabled;
		public bool UpdateEnabled {
			get { return _updateEnabled; }
			set {
				_updateEnabled = true;
				if (Go != null) {
					Go.SetActive(value);
				}
			}
		}

		public Func<object> Callback; 
		public string Category;
		public GameObject Go;

		public Text TextComponent {
			get {
				if (Go == null) {
					return null;
				}
				var textCo = Go.GetComponentInChildren<Text>();
				return textCo;
			}
		}

		public string ValueString {
			get {
				var value = Callback();
				var result = value.ToString();
				return result;
			}
		}
	}
}