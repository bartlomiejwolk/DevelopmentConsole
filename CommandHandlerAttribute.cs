using System;

namespace DevelopmentConsole {
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class CommandHandlerAttribute : Attribute
    {
    }
}