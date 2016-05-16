using System;

#pragma warning disable 414

namespace DevelopmentConsoleTool.CommandHandlerSystem {

    public abstract class CommandHandler {

	    protected readonly string CommandName;
	    protected readonly string Description;
	    protected readonly WeakReference ObjectReference;
	    protected readonly Type Type;
	    private readonly bool isStatic;

	    protected CommandHandler(string commandName, string description, object obj, Type type) {
		    CommandName = commandName;
		    Description = description;
		    Type = type;
			if (obj != null)
				ObjectReference = new WeakReference(obj);
			else
				isStatic = true;
		}

		public abstract void Invoke(params string[] arguments);
	}
}