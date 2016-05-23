using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevelopmentConsoleTool.Utilities;

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
            var splitData = SplitCommandString(commandString);
            var commandName = splitData[0];
            var commandHandler = GetHandlerByCommandName(commandName);
            if (commandHandler != null) {
                var parameters = splitData.GetRange(1, splitData.Count - 1);
                commandHandler.Invoke(parameters.ToArray());
            }
        }

        private static List<string> SplitCommandString(string commandString) {
            var arguments = commandString.Split(null).ToList();
            if (arguments.Count == 0) {
                return null;
            }
            return arguments;
        }

        private CommandHandler GetHandlerByCommandName(string commandName) {
            CommandHandler handler;
            commandHandlers.TryGetValue(commandName, out handler);
            return handler;
        }

        public void RegisterCommandHandlers(Type type) {
            RegisterCommandHandlers(type, null);
        }

        public void RegisterCommandHandlers(object obj) {
            RegisterCommandHandlers(obj.GetType(), obj);
        }

        public void RegisterCommandHandlers(Type type, object obj) {
            if (handlerTypes.Contains(type)) {
                return;
            }

            handlerTypes.Add(type);
            RegisterCommandHandlerMethods(type, obj);
        }

        public void RegisterCommandHandlerMethods(Type type, object obj) {
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

            var handler = new MethodCommandHandler(type, obj, method, commandName, description);
            commandHandlers.Add(commandName.ToLower(), handler);
        }
    }
}