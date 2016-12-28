using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FsTelegramMonitoringBot.Extension;
using MessageHandlers;
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
            IMessageHandler svcMessageHandler
        ) : base(token)
        {
            _chatIds = chatIds;
            _svcMessageHandler = svcMessageHandler;

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
            if (message.Text.Contains("/svc"))
            {
                await HandleServiceMessageAsync();
            }
            else
            {
                await this.SendMultipleMultiTextMessageExAsync(
                    _chatIds, $"Unknown command: {message.Text}");
            }
        }

        private async Task HandleServiceMessageAsync()
        {
            var output = await _svcMessageHandler.HandleAsync();
            await this.SendMultipleMultiTextMessageExAsync(_chatIds, output);
        }

        private readonly string[] _chatIds;
        private readonly IMessageHandler _svcMessageHandler;
    }
}
