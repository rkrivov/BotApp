using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotApp.Service.Api.Handlers
{
    internal class TodayHandler : ServiceHandler
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
