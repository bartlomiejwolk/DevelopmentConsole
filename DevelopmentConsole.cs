using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.UI;
#pragma warning disable 649

namespace Assets.Extensions.DevelopmentConsole {

    [RequireComponent(typeof(LineManager))]
    public class DevelopmentConsole : MonoBehaviour {

        [SerializeField]
        private LineManager lineManager;

        private void Awake() {
        }

    }
}