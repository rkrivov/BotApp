using BotApp.Logger.Api;
using BotApp.Telegram.Api.Commands;
using BotApp.Telegram.Api.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotApp.Telegram.Api.Handlers
{
    internal class CallbackQueryHandler : Handler
    {
        public override Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            LoggerService.Logger?.LogInformation("Callback data: {data}", update?.CallbackQuery?.Data ?? string.Empty);
            LoggerService.Logger?.LogInformation("From: {FirstName} {LastName}", update?.CallbackQuery?.From?.FirstName ?? string.Empty, update?.CallbackQuery?.From?.LastName ?? string.Empty);
            LoggerService.Logger?.LogInformation("ID: {Id}", update?.CallbackQuery?.From?.Id ?? 0);

            var command = Command.Parse(update?.CallbackQuery?.Data ?? string.Empty);

            if (command == null) return;

            await TelegramClient.ExecuteHandler(command.Name, command.Arguments, botClient!, update!, cancellationToken);
        }

        public override Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
