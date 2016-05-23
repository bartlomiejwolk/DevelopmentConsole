using System;

namespace DevelopmentConsoleTool.CommandHandlerSystem {
    
    public struct ParamInfo {

        public Type Type;
        public object DefaultValue;
        public string Name;
        public bool IsOptional;
        public bool IsParamArray;

    }
}