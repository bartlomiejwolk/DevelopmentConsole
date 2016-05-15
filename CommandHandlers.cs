using System;
using System.Collections.Generic;
using System.Reflection;

namespace DevelopmentConsoleTool {

    public static class CommandHandlers {

        public static readonly HashSet<Type> HandlerTypes = new HashSet<Type>();
        private static readonly Dictionary<string, CommandHandler> Handlers = new Dictionary<string, CommandHandler>();

        public static void RegisterMethodHandlers(Type type, object obj) {
            var methods = GetMethodsFromType(type, obj);

            foreach (var methodInfo in methods) {
                var customAttributes = methodInfo.GetCustomAttributes(typeof (CommandHandlerAttribute), true);
                if (customAttributes.Length > 0) {
                    var attribute = (CommandHandlerAttribute) customAttributes[0];
                    RegisterMethodCommandHandler(type, obj, methodInfo, attribute.Name, attribute.Description);
                }
            }
        }

        private static void RegisterMethodCommandHandler(Type type, object obj, MethodInfo method, string name, string description) {
            throw new NotImplementedException();
        }

        private static MethodInfo[] GetMethodsFromType(Type type, object obj) {
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
    }
}