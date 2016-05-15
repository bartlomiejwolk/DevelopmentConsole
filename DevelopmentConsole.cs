using System;
using UnityEngine;
using UnityEngine.Assertions;
#pragma warning disable 649

namespace DevelopmentConsoleTool {

    [RequireComponent(typeof(LineManager))]
    public class DevelopmentConsole : MonoBehaviour {

	    public static DevelopmentConsole Instance { get; private set; }

        [SerializeField]
        private LineManager lineManager;
        private Action returnKeyPressed;
	    private CommandHandlerManager commandHandlerManager =
			new CommandHandlerManager();

        private void Awake() {
			// todo implement proper singleton
			Instance = this;

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
	        var manager = Instance.commandHandlerManager;

			if (manager.HandlerTypes.Contains(type)) {
                return;
            }

			manager.HandlerTypes.Add(type);
			manager.RegisterMethodHandlers(type, obj);
        }

    }
}