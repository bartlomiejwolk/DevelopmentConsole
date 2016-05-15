using System;
using System.Collections.Generic;
using System.Reflection;

namespace DevelopmentConsoleTool {

    public class CommandHandlerManager {

        public readonly HashSet<Type> HandlerTypes = new HashSet<Type>();
        private readonly Dictionary<string, CommandHandler> CommandHandlers =
			new Dictionary<string, CommandHandler>();

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
			CommandHandlers.Add(commandName.ToLower(), handler);
		}

	    public void HandleCommand(string commandString) {
		    throw new NotImplementedException();
	    }
    }
}