using System;
using DevelopmentConsoleTool.CommandHandlerSystem;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 649

namespace DevelopmentConsoleTool {

	/// <summary>
	/// The main class of the DevelopmentConsole tool.
	/// </summary>
    public class DevelopmentConsole : MonoBehaviour {

	    public static DevelopmentConsole Instance;

	    [SerializeField]
	    private bool dontDestroyOnLoad = true;

		[SerializeField]
		private LineManager lineManager;

		[SerializeField]
		private Canvas canvas;

		[SerializeField]
		private KeyCode toggleConsoleWindowKey = KeyCode.BackQuote;

	    private Action returnKeyPressed;
		private Action toggleConsoleWindowKeyPressed;

		private bool IsConsoleWindowOpen {
			get { return canvas.gameObject.activeSelf; }
		}

	    #region UNITY MESSAGES

	    private void Awake() {
	        InitializeSingleton();

			returnKeyPressed += OnReturnKeyPressed;
		    toggleConsoleWindowKeyPressed += OnToggleConsoleWindowKeyPressed;

			var keyChar = (char)toggleConsoleWindowKey;
			lineManager.IgnoredChars = keyChar.ToString();

			Assert.IsNotNull(lineManager);
			Assert.IsNotNull(canvas);
	    }

		private void Start() {
		}

		private void Update() {
		    CheckForReturnKey();
			CheckForToggleConsoleWindowKey();
	    }

	    #endregion

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

		#region INPUT HANDLERS

		private void OnReturnKeyPressed() {
			CommandHandlerManager.HandleCommand(lineManager.CommandString);
			lineManager.AddNewLine();
	    }

		private void OnToggleConsoleWindowKeyPressed() {
			if (IsConsoleWindowOpen) {
				CloseConsoleWindow();
				return;
			}
			OpenConsoleWindow();
		}

		#endregion

		private void CheckForReturnKey()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                returnKeyPressed();
            }
        }

		private void CheckForToggleConsoleWindowKey() {
			if (Input.GetKeyDown(toggleConsoleWindowKey)) {
				toggleConsoleWindowKeyPressed();
			}
		}

		private void OpenConsoleWindow() {
			canvas.gameObject.SetActive(true);
			lineManager.GetFocus();
		}

		private void CloseConsoleWindow() {
			canvas.gameObject.SetActive(false);
		}
    }
}