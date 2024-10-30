using BotApp.Service.Api.Utils;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotApp.Service.Api.Handlers
{
    internal class StartHandler : ServiceHandler
    {
        public override Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            long id = GetId(update);
            GlobalServiceData.SetLocation(id, null);
            TelegramClient.StopScheduler();
            TelegramClient.ClearLastMessages();

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<b>Hi</b>,");
            stringBuilder.AppendLine("This bot will show you the planets of the day of week and calculate the planetary hour.");
            stringBuilder.AppendLine("Before starting work, you need to send your coordinates.");

            await SendMessage(botClient, update, stringBuilder.ToString());
            await SendFooter(botClient, update, cancellationToken);
        }

        public override Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
