using System;
using System.Configuration;
using System.Linq;
using MessageHandlers;

namespace FsTelegramMonitoringBot
{
    class Program
    {
        static void Main()
        {
            var botApiKey = Setup.GetBotApiKey().Trim();
            var chatIds = Setup.GetChatIds();
            var serviceNames = Setup.GetServiceNames();

            TelegramMonitoringBot bot = null;

            try
            {
                var factory = new MessageHandlerFactory(
                    new ServiceInfoMessageHandler(serviceNames), 
                    new MissingMessageHandler()
                );

                bot = new TelegramMonitoringBot(
                    botApiKey, chatIds, factory
                );

                bot.StartReceiving();

                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Bot is running...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine);
                Console.WriteLine($"Exception occurred: {ex}");
            }
            finally
            {
                bot?.StopReceiving();
            }
        }

        static class Setup
        {
            internal static string GetBotApiKey()
            {
                var botApiKey = Settings.BotApiKey;
                if (string.IsNullOrEmpty(Settings.BotApiKey))
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("Enter your telegram bot api key:");
                    botApiKey = Console.ReadLine();
                }

                return botApiKey;
            }

            internal static string[] GetChatIds()
            {
                var chatIds = Settings.ChatIds;
                if (chatIds.Length == 0)
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("Enter your telegram chat ids (separator ','):");
                    chatIds = Console.ReadLine().Split(',');
                }

                return chatIds;
            }

            internal static string[] GetServiceNames()
            {
                var serviceNames = Settings.TrackingWindowsServices;
                if (serviceNames.Length == 0)
                {
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine("Enter your service names (separator ','):");
                    serviceNames = Console.ReadLine().Split(',');
                }

                return serviceNames;
            }
        }

        static class Settings
        {
            internal static readonly string BotApiKey = ConfigUtils.AppSettings("botApiKey");

            internal static readonly string[] ChatIds = ConfigUtils.AppSettings("chatIds", ',');

            internal static readonly string[] TrackingWindowsServices
                = ConfigUtils.AppSettings("trackingWindowsServices", ',');
        }

        static class ConfigUtils
        {
            internal static string AppSettings(string key)
                => ConfigurationManager.AppSettings[key];

            internal static string[] AppSettings(string key, char separator)
                => AppSettings(key).Split(new [] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}