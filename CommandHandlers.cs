﻿using System.Collections.Generic;

namespace DevelopmentConsole {

    public static class CommandHandlers {

        public static readonly HashSet<System.Type> HandlerTypes = new HashSet<System.Type>();
        private static readonly Dictionary<string, CommandHandler> Handlers = new Dictionary<string, CommandHandler>();

    }
}