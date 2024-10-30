using BotApp.Logger.Api;
using BotApp.Telegram.Api.Extensions;
using BotApp.Telegram.Api.Utils;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotApp.Telegram.Api.Handlers
{
    internal class UpdateHandler : Handler
    {
        public override Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public override Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                //LoggerService.Logger?.LogInformation(Global.ToString(update));
                var updateType = update?.Type ?? UpdateType.Unknown;

                LoggerService.Logger?.LogInformation("Update type: {updateType}", updateType);

                if (updateType == UpdateType.Unknown)
                    return;

                await TelegramClient.ExecuteHandler(
                    updateType, 
                    botClient.ThrowIfNull(nameof(botClient)), 
                    update.ThrowIfNull(nameof(update)), 
                    cancellationToken);
            }
            catch (Exception exception)
            {
                await SendMessage(botClient, update, exception);
            }
        }
    }
}
