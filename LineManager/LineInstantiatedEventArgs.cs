using System;
using UnityEngine;

namespace DevelopmentConsoleTool {
    public class LineInstantiatedEventArgs : EventArgs {

        public GameObject InstantiatedGo { get; private set; }

        public LineInstantiatedEventArgs(GameObject instantiatedGo) {
            InstantiatedGo = instantiatedGo;
        }
    }
}