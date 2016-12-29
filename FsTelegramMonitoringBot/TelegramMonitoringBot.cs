using System.Threading.Tasks;
using FsTelegramMonitoringBot.Extension;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace FsTelegramMonitoringBot
{
    public class TelegramMonitoringBot : TelegramBotClient
    {
        public TelegramMonitoringBot(
            string token, 
            string[] chatIds,
            MessageHandlerFactory handlerFactory
        ) : base(token)
        {
            _chatIds = chatIds;
            _handlerFactory = handlerFactory;

            OnMessage += HandleMessageAsync;
        }

        private async void HandleMessageAsync(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            if (message == null || message.Type != MessageType.TextMessage)
                return;

            await Dispatch(message);
        }

        private async Task Dispatch(Message message)
        {
            var command = message.Text;
            var handler = _handlerFactory.CreateHandler(command);

            await this.SendMultipleMultiTextMessageExAsync(_chatIds, await handler.HandleAsync(command));
        }

        private readonly string[] _chatIds;
        private readonly MessageHandlerFactory _handlerFactory;
    }
}
