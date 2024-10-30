using BotApp.Service.Api.Exceptions;
using BotApp.Service.Api.Utils;
using Telegram.Bot;
using Telegram.Bot.Types;

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
