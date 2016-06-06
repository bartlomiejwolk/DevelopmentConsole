using UnityEngine;
using System.Collections;
using DevelopmentConsole.Core.CommandHandlerSystem;

public class CommandHandlerTest : MonoBehaviour {

	void Start () {
	    CommandHandlerManager.Instance.RegisterCommandHandlers(typeof(CommandHandlerTest), this);
	}

    [CommandHandler]
    private void Echo(string text) {
        Debug.Log(text);
    }

    [CommandHandler]
    private void Add(int a, int b) {
        Debug.Log(a + b);
    }

}
