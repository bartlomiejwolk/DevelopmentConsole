using System;

#pragma warning disable 414

namespace DevelopmentConsoleTool.CommandHandlerSystem {

    public abstract class CommandHandler {

        protected readonly string CommandName;
        protected readonly string Description;
        protected readonly WeakReference ObjectReference;
        protected readonly Type Type;
        private readonly bool isStatic;

        protected CommandHandler(
            Type type,
            object obj,
            string commandName,
            string description) {
            
            Type = type;
            if (obj != null) {
                ObjectReference = new WeakReference(obj);
            }
            else {
                isStatic = true;
            }
            CommandName = commandName;
            Description = description;
        }

        public abstract void Invoke(params string[] arguments);
    }
}