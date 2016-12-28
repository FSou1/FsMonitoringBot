using System.Threading.Tasks;

namespace MessageHandlers
{
    public interface IMessageHandler
    {
        Task<string> HandleAsync();
    }
}
