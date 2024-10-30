using BotApp.Service.Api.Utils;
using BotApp.Telegram.Api.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotApp.Service.Api.Handlers
{
    internal class StopSchedulerHandler : Handler
    {
        public override Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            TelegramClient.StopScheduler();
            UIFunctions.SetCommands(TelegramClient).Wait(cancellationToken);

            return Task.CompletedTask;
        }

        public override Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
