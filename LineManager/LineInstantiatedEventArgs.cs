using System;
using UnityEngine;

namespace DevelopmentConsoleTool {

	public class LineInstantiatedEventArgs : EventArgs {

        public GameObject InstantiatedGo { get; set; }
    }
}