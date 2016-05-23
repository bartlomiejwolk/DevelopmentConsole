using System;
using System.Reflection;

namespace DevelopmentConsoleTool.CommandHandlerSystem {

    public class MethodCommandHandler : CommandHandler {

        private readonly MethodInfo methodInfo;
        private readonly List<ParamInfo> paramInfos = new List<ParamInfo>();

        public MethodCommandHandler(
            string commandName,
            string description,
            object obj,
            Type type,
            MethodInfo methodInfo) : base(commandName, description, obj, type) {

            this.methodInfo = methodInfo;
        }

        public override void Invoke(params string[] arguments) {
            methodInfo.Invoke(ObjectReference.Target, null);
        }
    }
}