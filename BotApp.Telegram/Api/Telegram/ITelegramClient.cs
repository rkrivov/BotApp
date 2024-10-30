using BotApp.Telegram.Api.Delegates;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using BotApp.Telegram.Api.Handlers;
using BotApp.Telegram.Api.Contracts;
using Microsoft.Extensions.Logging;

namespace BotApp.Telegram.Api.Telegram
{
    public interface ITelegramClient : IDisposable
    {
        string BotToken { get; }
        ITelegramBotClient BotClient { get; }
        ReceiverOptions ReceiverOptions { get; }

        Task InitBotCLient();

        void ClearLastMessages();
        Message? GetLastMessge(Type type);
        Message? GetLastMessge<T>() where T : class;
        Message? GetLastMessge(object? obj);
        void SetLastMessge(object obj, Message?  message);

        Task ClearCommands();
        Task SetCommands(IEnumerable<BotCommand> botCommands);

        void AddHandler<THandler>(object key) where THandler : class, IHandler, new();
        void RemoveHandler(object key);
        bool HandleAvail(object key);
        Task ExecuteHandler(object key, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        Task ExecuteHandler(object key, object args, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);

        void StartScheduler(TimeSpan dueTime, TimeSpan period, object state, SchedulerHandle handle);
        void StartScheduler(TimeSpan dueTime, TimeSpan period, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, SchedulerHandle handle);
        void StopScheduler();
        bool SchedulerActive { get; }

        void StartReceiving();

        void StopReceiving();

        Task SendTextMessage(ChatId chatId, string message);
        Task SendTextMessage(ITelegramBotClient botClient, ChatId chatId, string message);
        Task SendCommand(long chatId, string command);
        Task SendMessage(long chatId, string message);
    }
}
