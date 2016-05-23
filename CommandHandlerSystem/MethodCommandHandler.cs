using System;
using System.Collections.Generic;
using System.Reflection;

namespace DevelopmentConsoleTool.CommandHandlerSystem {

    public class MethodCommandHandler : CommandHandler {

        private readonly MethodInfo methodInfo;
        private readonly List<ParamInfo> paramInfos = new List<ParamInfo>();

        public MethodCommandHandler(
            Type type,
            object obj,
            MethodInfo methodInfo,
            string commandName,
            string description) : base(type, obj, commandName, description) {

            this.methodInfo = methodInfo;
        }

        public override void Invoke(params string[] arguments) {
            methodInfo.Invoke(ObjectReference.Target, null);
        }
    }
}