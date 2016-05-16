﻿using System;
using DevelopmentConsoleTool.CommandHandlerSystem;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 649

namespace DevelopmentConsoleTool {

	/// <summary>
	/// The main class of the DevelopmentConsole tool.
	/// </summary>
    [RequireComponent(typeof(LineManager))]
    public class DevelopmentConsole : MonoBehaviour {

	    public static DevelopmentConsole Instance;

	    [SerializeField]
	    private bool dontDestroyOnLoad = true;

		[SerializeField]
		private Canvas canvas;

        private LineManager lineManager;
	    private readonly CommandHandlerManager commandHandlerManager =
			new CommandHandlerManager();

	    private Action returnKeyPressed;

	    #region UNITY MESSAGES

	    private void Awake() {
	        InitializeSingleton();
			returnKeyPressed += OnReturnKeyPressed;

		    lineManager = GetComponent<LineManager>();
		    Assert.IsNotNull(lineManager);
			Assert.IsNotNull(canvas);
	    }

	    private void Update() {
		    CheckForReturnKey();
	    }

	    #endregion

	    public static void RegisterCommandHandlers(Type type, object obj) {
		    var manager = Instance.commandHandlerManager;
			manager.RegisterCommandHandlers(type, obj);
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

	    private void CheckForReturnKey()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                returnKeyPressed();
            }
        }

		private void OpenConsoleWindow() {
			canvas.enabled = true;
		}

		private void CloseConsoleWindow() {
			canvas.enabled = false;
		}
    }
}