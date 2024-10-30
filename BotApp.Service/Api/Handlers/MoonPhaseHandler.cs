using BotApp.Service.Api.Astronomy.Moon;
using BotApp.Service.Api.Exceptions;
using BotApp.Service.Api.Extensions;
using BotApp.Service.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotApp.Service.Api.Handlers
{
    internal class MoonPhaseHandler : ServiceHandler
    {
        public override Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var id = GetId(update);
            var location = GlobalServiceData.GetLocation(id);

            if (location == null)
                throw new LocationNotReceiptException();

            var latitude = Convert.ToDecimal(location?.Latitude ?? 0.00);
            var longitude = Convert.ToDecimal(location?.Longitude ?? 0.00);

            var datetime = DateTime.Now;

            await MoonHelper.GetMoonPhase(latitude, longitude, datetime);
            await MoonHelper.GetMoonPhase(latitude, longitude, datetime.GetMonthStart(), datetime.GetMonthEnd());
        }

        public override Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
