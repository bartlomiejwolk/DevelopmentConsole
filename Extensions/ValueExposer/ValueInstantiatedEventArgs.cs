using System;
using UnityEngine;

namespace DevelopmentConsoleTool.ValueExposerExtension {

	public class ValueInstantiatedEventArgs : EventArgs {

		public string ValueName { get; set; }
		public GameObject Go { get; set; }
	}
}