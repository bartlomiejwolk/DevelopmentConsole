using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Debug = UnityEngine.Debug;

namespace DevelopmentConsoleTool.CommandHandlerSystem {

    public class MethodCommandHandler : CommandHandler {

        private readonly MethodInfo _methodInfo;
        private readonly List<ParamInfo> paramInfos;

        public MethodCommandHandler(
            Type type,
            object obj,
            MethodInfo methodInfo,
            string commandName,
            string description) : base(type, obj, commandName, description) {

            _methodInfo = methodInfo;
            // todo update _methodInfo directly from the method
            paramInfos = GetMethodParameters(methodInfo);
        }

        private List<ParamInfo> GetMethodParameters(MethodInfo methodInfo) {
            var parameters = methodInfo.GetParameters();
            var result = new List<ParamInfo>();
            foreach (var parameter in parameters) {
                var paramInfo = new ParamInfo(parameter);
                result.Add(paramInfo);
            }
            return result;
        }

        public override void Invoke(params string[] arguments) {
            var convertedArgs = ConvertArgumentsToObjects(arguments);
            if (ObjectReference != null) {
                _methodInfo.Invoke(ObjectReference.Target, convertedArgs);
            }
            else {
                _methodInfo.Invoke(null, convertedArgs);
            }
        }

        private object[] ConvertArgumentsToObjects(string[] arguments) {
            var argObjects = new List<object>();
            for (var i = 0; i < paramInfos.Count; i++) {
                var paramInfo = paramInfos[i];
                var argument = arguments[i];
                var argObject = GetArgumentValueFromString(paramInfo.Type, argument);
                argObjects.Add(argObject);
            }
            return argObjects.ToArray();
        }

        private object GetArgumentValueFromString(Type type, string argument) {
            try
            {
                if (type == typeof(string))
                {
                    var argObject = (object)argument;
                    return argObject;
                }
                if (type == typeof(int))
                {
                    var argObject = (object)int.Parse(argument);
                    return argObject;
                }
                if (type == typeof(float))
                {
                    var argObject = (object)float.Parse(argument);
                    return argObject;
                }
                if (type == typeof(bool))
                {
                    var argObject = (object)bool.Parse(argument);
                    return argObject;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Could not parse command line argument!");
            }
            return null;
        }
    }
}