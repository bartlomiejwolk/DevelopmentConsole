using System;
using System.Reflection;

namespace DevelopmentConsoleTool {

	public class MethodCommandHandler : CommandHandler {

		private readonly MethodInfo methodInfo;

		public MethodCommandHandler(
			string commandName,
			string description,
			object obj,
			Type type,
			MethodInfo methodInfo) : base(commandName, description, obj, type) {

			this.methodInfo = methodInfo;
		}
	}
}