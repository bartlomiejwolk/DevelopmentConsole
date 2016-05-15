using System;
using UnityEngine;
using UnityEngine.Assertions;
#pragma warning disable 649

namespace DevelopmentConsoleTool {

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
            if (CommandHandlerManager.HandlerTypes.Contains(type)) {
                return;
            }

            CommandHandlerManager.HandlerTypes.Add(type);
            CommandHandlerManager.RegisterMethodHandlers(type, obj);
        }

    }
}