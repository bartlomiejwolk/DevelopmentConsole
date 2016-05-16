using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

#pragma warning disable 649

namespace DevelopmentConsoleTool {

    [RequireComponent(typeof(LineManager))]
    public class DevelopmentConsole : MonoBehaviour {

	    public static DevelopmentConsole Instance;

	    [SerializeField]
	    private bool dontDestroyOnLoad = true;

        [SerializeField]
        private LineManager lineManager;
        private Action returnKeyPressed;
	    private readonly CommandHandlerManager commandHandlerManager =
			new CommandHandlerManager();

        private void Awake() {
	        InitializeSingleton();

            Assert.IsNotNull(lineManager);
			returnKeyPressed += OnReturnKeyPressed;
		}

	    private void InitializeSingleton() {
		    if (Instance != null) {
			    if (Instance == this) {
				    return;
			    }
			    Debug.Log("Multiple DevelopmentConsole instances detected in the scene. Only one DevelopmentConsole can exist at a time. The duplicate DevelopmentConsole will not be used.");
			    Destroy(gameObject);
			    return;
		    }
		    Instance = this;

		    if (dontDestroyOnLoad) {
			    DontDestroyOnLoad(this);
		    }
	    }

	    private void OnReturnKeyPressed() {
			commandHandlerManager.HandleCommand(lineManager.CommandString);
			lineManager.AddNewLine();
	    }

	    private void Start() {
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