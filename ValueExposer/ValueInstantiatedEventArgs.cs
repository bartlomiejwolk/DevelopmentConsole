using System;
using UnityEngine;

namespace DevelopmentConsoleTool.ValueExposerExtension {
	public class ValueInstantiatedEventArgs : EventArgs {
		public string ValueName { get; private set; }
		public GameObject Go { get; private set; }

		public ValueInstantiatedEventArgs(
			string valueName,
			GameObject go) {

			ValueName = valueName;
			Go = go;
		}
	}
}