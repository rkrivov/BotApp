using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using BotApp.Telegram.Api.Telegram;
using Telegram.Bot.Polling;
using BotApp.Telegram.Api.Contracts;

namespace BotApp.Telegram.Api.Handlers
{
    public interface IHandler : IUpdateHandler, IDisposable
    {
        ITelegramClient TelegramClient { get; }
        IEnumerable<object> Arguments { get; }
        int ArgumentsCount { get; }

        void SetClient(ITelegramClient client);
        void SetArguments(IEnumerable<object>? arguments);

        Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    }
}
