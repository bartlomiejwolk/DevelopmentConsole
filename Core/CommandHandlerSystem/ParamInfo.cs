using System;
using System.Reflection;

namespace DevelopmentConsole.Core.CommandHandlerSystem {
    public struct ParamInfo {
        public Type Type;
        public object DefaultValue;
        public string Name;
        public bool IsOptional;
        public bool IsParamArray;

        public ParamInfo(ParameterInfo paramInfo) : this() {
            Type = paramInfo.ParameterType;
            DefaultValue = paramInfo.DefaultValue;
            Name = paramInfo.Name;
            IsOptional = paramInfo.IsOptional;
            var paramAttributes = paramInfo.GetCustomAttributes(typeof(ParamArrayAttribute), false);
            IsParamArray = paramAttributes.Length > 0;
        }
    }
}