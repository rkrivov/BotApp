using BotApp.Logger.Api;
using BotApp.Telegram.Api.Commands;
using BotApp.Telegram.Api.Extensions;
using BotApp.Telegram.Api.Routes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotApp.Telegram.Api.Handlers
{
    internal class MessageHandler : Handler
    {
        public override Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            botClient.ThrowIfNull();
            update.ThrowIfNull(); 

            var messageType = update?.Message?.Type ?? MessageType.Unknown;

            await TelegramClient.ExecuteHandler(
                messageType == MessageType.Venue ? MessageType.Location : messageType,
                botClient.ThrowIfNull(nameof(botClient)), 
                update.ThrowIfNull(nameof(update)), 
                cancellationToken);

            LoggerService.Logger?.LogInformation("Message text: {updateType}", update?.Message?.Text ?? string.Empty);
            LoggerService.Logger?.LogInformation("From: {FirstName} {LastName}", update?.CallbackQuery?.From?.FirstName ?? string.Empty, update?.CallbackQuery?.From?.LastName ?? string.Empty);
            LoggerService.Logger?.LogInformation("ID: {Id}", update?.CallbackQuery?.From?.Id ?? 0);

            switch (messageType)
            {
                case MessageType.Text:
                    var command = Command.Parse(update?.Message?.Text ?? string.Empty);

                    if (command == null) return;

                    await TelegramClient.ExecuteHandler(
                        command.Name,
                        command.Arguments,
                        botClient,
                        update!,
                        cancellationToken);
                    break;

                default:
                    return;
            }
        }

        public override Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
