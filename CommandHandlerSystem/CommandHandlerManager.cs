using System;
using System.Collections.Generic;
using System.Reflection;

namespace DevelopmentConsoleTool.CommandHandlerSystem {

    public sealed class CommandHandlerManager {

        private static readonly CommandHandlerManager instance 
            = new CommandHandlerManager();

        private readonly HashSet<Type> handlerTypes = new HashSet<Type>();

        private readonly Dictionary<string, CommandHandler> commandHandlers =
            new Dictionary<string, CommandHandler>();

        public static CommandHandlerManager Instance {
            get { return instance; }
        }

        private CommandHandlerManager() {}

        public void HandleCommand(string commandString) {
            CommandHandler commandHandler;
            commandHandlers.TryGetValue(commandString, out commandHandler);
            if (commandHandler != null) {
                commandHandler.Invoke();
            }
        }

        public void RegisterCommandHandlers(Type type, object obj) {
            if (handlerTypes.Contains(type)) {
                return;
            }

            handlerTypes.Add(type);
            RegisterMethodHandlers(type, obj);
        }

        public void RegisterMethodHandlers(Type type, object obj) {
            var methods = GetMethodsFromType(type, obj);

            foreach (var method in methods) {
                var customAttributes = method.GetCustomAttributes(
                    typeof (CommandHandlerAttribute), true);

                if (customAttributes.Length <= 0) {
                    continue;
                }

                var attribute = (CommandHandlerAttribute) customAttributes[0];
                RegisterMethodCommandHandler(
                    type,
                    obj,
                    method,
                    attribute.Name,
                    attribute.Description);
            }
        }

        private IEnumerable<MethodInfo> GetMethodsFromType(
            Type type,
            object obj) {

            MethodInfo[] methods;
            if (obj != null) {
                methods = type.GetMethods(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic);
            }
            // class
            else {
                methods = type.GetMethods(
                    BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.NonPublic);
            }

            return methods;
        }

        private void RegisterMethodCommandHandler(
            Type type,
            object obj,
            MethodInfo method,
            string commandName,
            string description) {

            if (string.IsNullOrEmpty(commandName)) {
                commandName = method.Name;
            }

            var handler = new MethodCommandHandler(commandName, description, obj, type, method);
            commandHandlers.Add(commandName.ToLower(), handler);
        }
    }
}