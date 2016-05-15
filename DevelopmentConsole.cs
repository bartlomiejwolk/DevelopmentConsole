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
            returnKeyPressed += lineManager.AddNewLine;
        }

        private void Start() {
            lineManager.AddNewLine();
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

        public static void RegisterCommandHandlers(Type type, object obj) {
            if (CommandHandlers.HandlerTypes.Contains(type)) {
                return;
            }

            CommandHandlers.HandlerTypes.Add(type);
            CommandHandlers.RegisterMethodHandlers(type, obj);
        }

    }
}