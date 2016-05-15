using System;

namespace DevelopmentConsoleTool {

    public abstract class CommandHandler {

	    private readonly string commandName;
	    private readonly string description;
	    private readonly WeakReference objectReference;
	    private readonly Type type;
	    private readonly bool isStatic;

	    protected CommandHandler(string commandName, string description, object obj, Type type) {
		    this.commandName = commandName;
		    this.description = description;
		    this.type = type;
			if (obj != null)
				objectReference = new WeakReference(obj);
			else
				isStatic = true;
		}
    }
}