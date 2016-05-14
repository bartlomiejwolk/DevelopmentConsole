using System;
using UnityEngine;
using UnityEngine.Assertions;
#pragma warning disable 649

namespace DevelopmentConsole {

    [RequireComponent(typeof(LineManager))]
    public class DevelopmentConsole : MonoBehaviour {

        [SerializeField]
        private LineManager lineManager;
        private Action returnKeyPressed;


        private void Awake() {
            Assert.IsNotNull(lineManager);
            returnKeyPressed += lineManager.CreateNewCommandLine;
        }

        private void Start() {
            lineManager.CreateNewCommandLine();
        }

        private void Update() {
            CheckForReturnKey();
        }

        private void CheckForReturnKey()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                returnKeyPressed();
            }
        }

    }
}