using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace FsTelegramMonitoringBot.Extension
{
    public static class BotClientExtension
    {
        public static async Task SendMultipleMultiTextMessageExAsync(
            this ITelegramBotClient bot, string[] chatIds, string message, int chunkSize = 4096
        )
        {
            foreach (var chatId in chatIds)
            {
                await bot.SendMultiTextMessageExAsync(chatId, message);
            }
        }

        public static async Task SendMultiTextMessageExAsync(
            this ITelegramBotClient bot, string chatId, string message, int chunkSize = 4096
        )
        {
            for (var i = 0; i < message.Length; i += chunkSize)
            {
                var chunk = message.Substring(i, Math.Min(chunkSize, message.Length - i));
                await bot.SendTextMessageAsync(chatId, chunk);
            }
        }
    }
}