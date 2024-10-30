using BotApp.Service.Api.Exceptions;
using BotApp.Service.Api.Extensions;
using BotApp.Service.Api.Utils;
using BotApp.Telegram.Api.Handlers;
using Telegram.Bot;
using Telegram.Bot.Types;
using BotApp.Service.Api.Astronomy.Sun;

namespace BotApp.Service.Api.Handlers
{
    internal abstract class ServiceHandler : Handler
    {
        protected virtual async Task ProcessHandle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var id = GetId(update);
            var location = GlobalServiceData.GetLocation(id);

            if (location == null)
                throw new LocationNotReceiptException();

            var datetime = DateTime.UtcNow;
            var showFull = false;

            if (this is YesterdayHandler)
            {
                datetime = datetime.AddDays(-1);
                datetime = new DateTime(datetime.Year, datetime.Month, datetime.Day, 12, 0, 0, 0);
                
                showFull = true;
            }

            if (this is TomorrowHandler)
            {
                datetime = datetime.AddDays(1);
                datetime = new DateTime(datetime.Year, datetime.Month, datetime.Day, 12, 0, 0, 0);
                
                showFull = true;
            }

            await SunriseSunsetHelper.SendSunriseSunsetInfo(
                botClient.ThrowIfNull(),
                update.ThrowIfNull(),
                datetime,
                showFull,
                cancellationToken);
        }

        protected virtual async Task SendFooter(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await UIFunctions.SetCommands(TelegramClient);
            // await UIFunctions.SendMainMenu(botClient, update, cancellationToken);
        }
    }
}
