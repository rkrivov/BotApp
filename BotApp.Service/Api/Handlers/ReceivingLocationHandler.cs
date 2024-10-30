using BotApp.Service.Api.Exceptions;
using BotApp.Service.Api.Extensions;
using BotApp.Service.Api.Utils;
using BotApp.Service.Api.SunrizeSunet;
using BotApp.Telegram.Api.Handlers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotApp.Service.Api.Handlers
{
    internal class ReceivingLocationHandler : ServiceHandler
    {
        public override Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                long id = GetId(update);
                Location? location = update?.Message?.Location ?? null;

                if (location == null)
                    location = update?.ChannelPost?.Location ?? null;

                if (location == null || (location?.Latitude ?? 0) == 0 || (location?.Longitude ?? 0) == 0)
                    throw new WrongLocationException();

                GlobalServiceData.SetLocation(id, location);

                await ProcessHandle(botClient, update!, cancellationToken);
            }
            catch (Exception ex) 
            {
                await SendMessage(botClient, update!, ex);
            }
            finally
            {
                await SendFooter(botClient, update!, cancellationToken);
            }
        }

        public override Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
