using System;
using System.Collections.Generic;
using System.Reflection;

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
            throw new NotImplementedException();
        }
    }
}