using BotApp.Logger.Api;
using BotApp.Telegram.Api.Commands;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotApp.Telegram.Api.Handlers
{
    internal class ChannelPostHandler : Handler
    {
        public override Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            LoggerService.Logger?.LogInformation("Message text: {updateType}", update?.ChannelPost?.Text ?? string.Empty);
            LoggerService.Logger?.LogInformation("Chat title: {ChatTitle}", update?.ChannelPost?.Chat?.Title ?? string.Empty);
            LoggerService.Logger?.LogInformation("Chat Id: {ChatId}", update?.ChannelPost?.Chat?.Id ?? 0);

            var messageType = update?.ChannelPost?.Type ?? MessageType.Unknown;

            if (messageType == MessageType.Unknown) return;

            if (TelegramClient.HandleAvail(messageType))
            {
                await TelegramClient.ExecuteHandler(messageType, botClient, update!, cancellationToken);
            }
            else
            {
                var command = Command.Parse(update?.ChannelPost?.Text ?? string.Empty);

                if (command == null) return;

                await TelegramClient.ExecuteHandler(command.Name, command.Arguments, botClient, update!, cancellationToken);
            }
        }

        public override Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
