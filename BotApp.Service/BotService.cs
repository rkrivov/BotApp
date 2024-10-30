using BotApp.Service.Api.Controls;
using BotApp.Telegram.Api.Telegram;
using Telegram.Bot.Types.Enums;
using BotApp.Service.Api.Utils;
using BotApp.Service.Api.Handlers;
using Microsoft.Extensions.Logging;
using BotApp.Service.Api.Extensions;
using System.Runtime.InteropServices;
using Telegram.Bot;
using BotApp.Telegram.Api;
using BotApp.Logger.Api;

namespace BotApp.Service
{
    public static class BotService
    {
        private readonly static string BotToken = "<BOT-TOKEN>";
        private readonly static ITelegramClient telegramClient = TelegramService.GetTelegramClient(BotToken);
        public static ITelegramClient TelegramClient => telegramClient.ThrowIfNull();

        public static void Start()
        {
            using var cts = new CancellationTokenSource();

            TelegramClient.InitBotCLient()
                .ContinueWith(task =>
                {
                    if (task.IsFaulted) return;

                    TelegramClient.AddHandler<StartHandler>(Keyboard.COMMAND_START);
                    TelegramClient.AddHandler<KeyboardHandler>(Keyboard.COMMAND_KEYBOARD);

                    TelegramClient.AddHandler<YesterdayHandler>(Keyboard.COMMAND_YESTERDAY);
                    TelegramClient.AddHandler<TodayHandler>(Keyboard.COMMAND_TODAY);
                    TelegramClient.AddHandler<TomorrowHandler>(Keyboard.COMMAND_TOMORROW);
                    TelegramClient.AddHandler<StartSchedulerHandler>(Keyboard.COMMAND_START_SCHEDULER);
                    TelegramClient.AddHandler<StopSchedulerHandler>(Keyboard.COMMAND_STOP_SCHEDULER);
                    TelegramClient.AddHandler<SetTimezoneOffsetHandler>(Keyboard.COMMAND_SET_TIMEZONE);

                    TelegramClient.AddHandler<ReceivingLocationHandler>(MessageType.Location);
                    TelegramClient.AddHandler<ReceivingLocationHandler>(MessageType.Venue);

                    TelegramClient.StartReceiving();

                })
                .Wait(cts.Token);

            UIFunctions.SetCommands(TelegramClient)
                .Wait(cts.Token);
        }
        public static void Stop()
        {

        }
        
        public static async Task<long> GetBotChatId()
        {
            var me = await telegramClient.BotClient.GetMeAsync();

            return me.Id;
        }
        public static async Task<string> GetBotUsername()
        {
            var me = await telegramClient.BotClient.GetMeAsync();

            return me.Username ?? string.Empty;
        }
    }
}
