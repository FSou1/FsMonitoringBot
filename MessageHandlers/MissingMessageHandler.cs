using System;
using System.Threading.Tasks;

namespace MessageHandlers
{
    public class MissingMessageHandler : IMessageHandler
    {
        public Task<string> HandleAsync(string command)
        {
            var result = $"Unknown command: {command}";
            return Task.FromResult(result);
        }
    }
}