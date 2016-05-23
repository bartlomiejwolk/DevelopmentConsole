using System;
using System.Collections.Generic;
using System.Linq;
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
            _methodInfo.Invoke(ObjectReference.Target, null);
        }
    }
}