using System;

#pragma warning disable 414

namespace DevelopmentConsole.Core.CommandHandlerSystem {
    public abstract class CommandHandler {
        protected readonly string CommandName;
        protected readonly string Description;
        protected readonly WeakReference ObjectReference;
        protected readonly Type Type;
        private readonly bool _isStatic;

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
                _isStatic = true;
            }
            CommandName = commandName;
            Description = description;
        }

        public abstract void Invoke(params string[] arguments);
    }
}