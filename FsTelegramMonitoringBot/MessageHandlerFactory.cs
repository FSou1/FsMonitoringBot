using System;
using System.Collections.Generic;
using System.Linq;
using MessageHandlers;

namespace FsTelegramMonitoringBot
{
    public class MessageHandlerFactory
    {
        public MessageHandlerFactory(
            IMessageHandler serviceInfoMessageHandler,
            IMessageHandler missingMessageHandler
        )
        {
            _serviceInfoMessageHandler = serviceInfoMessageHandler;
            _missingMessageHandler = missingMessageHandler;
        }

        public IMessageHandler CreateHandler(string message)
        {
            var command = message.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)[0];

            if (command.Contains("/svc"))
            {
                return _serviceInfoMessageHandler;
            }

            return _missingMessageHandler;
        }

        private readonly IMessageHandler _serviceInfoMessageHandler;
        private readonly IMessageHandler _missingMessageHandler;
    }
}