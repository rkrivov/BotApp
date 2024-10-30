using BotApp.Service.Api.Extensions;
using BotApp.Service.Api.Utils;
using BotApp.Service.Api.SunrizeSunet;
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
    internal class YesterdayHandler : ServiceHandler
    {
        public override Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
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
