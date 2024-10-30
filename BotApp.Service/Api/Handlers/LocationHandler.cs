using BotApp.Service.Api.Utils;
using BotApp.Telegram.Api.Handlers;
using BotApp.Telegram.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotApp.Service.Api.Handlers
{
    internal class LocationHandler : ServiceHandler
    {
        public override Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await botClient.SendTextMessageAsync(Global.GetId(update), Properties.Resources.PleaseSendLocation);

            TelegramClient.AddHandler<ReceivingLocationHandler>(MessageType.Location);
            TelegramClient.AddHandler<ReceivingLocationHandler>(MessageType.Venue);
        }

        public override Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
