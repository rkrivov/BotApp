using BotApp.Telegram.Api.Delegates;

using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using BotApp.Telegram.Api.Routes;
using BotApp.Telegram.Api.Handlers;
using System.Diagnostics;
using BotApp.Telegram.Api.Contracts;
using BotApp.Telegram.Api.Extensions;
using System.Threading;
using System.Collections;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata;
using BotApp.Logger.Api;
using System.Runtime.InteropServices;
using BotApp.Telegram.Api.Commands;
using BotApp.Telegram.Api.Utils;
using System.Data;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotApp.Telegram.Api.Telegram
{
    public class TelegramClient : ITelegramClient
    {
        private bool disposed = false;
        private readonly HandlersRouter<string, IHandler> _handlersRouter = new();
        private readonly Dictionary<string, Message?> lastMessages = new();
        private Timer? _timer = null;

        public string BotToken { get; private set; }
        public ITelegramBotClient BotClient { get; private set; }
        public ReceiverOptions ReceiverOptions { get; private set; }

        public TelegramClient(string token)
        {
            using var cts = new CancellationTokenSource();
            BotToken = token;

            ReceiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage,
                    UpdateType.ChannelPost,
                    UpdateType.EditedChannelPost,
                    UpdateType.InlineQuery,
                    UpdateType.CallbackQuery,
                },
                ThrowPendingUpdates = true,
            };

            BotClient = new TelegramBotClient(token);
        }

        ~TelegramClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                BotClient.DeleteMyCommandsAsync().Wait();
                BotClient.CloseAsync().Wait();
            }

            disposed = true;
        }

        public Task InitBotCLient()
        {
            return Task.CompletedTask;
            /*
            var me = await BotClient.GetMeAsync();

            var chatId = me?.Id ?? 0;

            if (chatId == 0) return;

            try
            {
                var administrators = await BotClient.GetChatAdministratorsAsync(chatId);
                LoggerService.Logger?.LogInformation($"{nameof(administrators)} = {Global.ToString(administrators)}");
            }
            catch(Exception exception)
            {
                LoggerService.Logger?.LogError(exception, "Exception: {0}\n{1}", exception.Message, exception.StackTrace);
            }

            try
            {
                var member = await BotClient.GetChatMemberAsync(chatId, 0);
                LoggerService.Logger?.LogInformation($"{nameof(member)} = {Global.ToString(member)}");
            }
            catch (Exception exception)
            {
                LoggerService.Logger?.LogError(exception, "Exception: {0}\n{1}", exception.Message, exception.StackTrace);
            }

            LoggerService.Logger?.LogInformation($"{nameof(me)} = {Global.ToString(me)}");

            LoggerService.Logger?.LogInformation($"{nameof(chatId)} = {Global.ToString(chatId)}");

            var count = await BotClient.GetChatMemberCountAsync(chatId);

            LoggerService.Logger?.LogInformation($"{nameof(count)} = {Global.ToString(count)}");

            var chat = await BotClient.GetChatAsync(chatId);
            LoggerService.Logger?.LogInformation($"{nameof(chat)} = {Global.ToString(chat)}");

            await SetCommands(new List<BotCommand> 
            {
                new BotCommand { Command = "/start", Description = "Start new session" },
                new BotCommand { Command = "/mainmenu", Description = "Show main menu" },
            });
            */
        }

        public void ClearLastMessages() => lastMessages.Clear();

        public Message? GetLastMessge(Type type)
        {
            if (type == null) return null;
            var key = type?.FullName ?? string.Empty;

            if (string.IsNullOrEmpty(key)) return null;

            key = key.ToHexString();

            return lastMessages.ContainsKey(key) ? lastMessages[key] : null;
        }
        public Message? GetLastMessge(object? obj) => obj == null ? null : GetLastMessge(obj.GetType());
        public Message? GetLastMessge<T>() where T : class => GetLastMessge(typeof(T));

        public void SetLastMessge(object? obj, Message? message)
        {
            if (obj == null) return;
            var type = obj.GetType();
            var key = type?.FullName ?? string.Empty;

            if (string.IsNullOrEmpty(key)) return;

            key = key.ToHexString();

            lastMessages[key] = message;
        }

        public async Task ClearCommands()
        {
            await BotClient.DeleteMyCommandsAsync();
        }

        public async Task SetCommands(IEnumerable<BotCommand> botCommands)
        {
            LoggerService.Logger?.LogInformation("Set command");
            await BotClient.SetMyCommandsAsync(botCommands);
        }

        public void StartReceiving()
        {
            LoggerService.Logger?.LogInformation("Start receiving");

            using var cts = new CancellationTokenSource();

            var updateHander = new UpdateHandler();
            updateHander.SetClient(this);

            BotClient.StartReceiving(updateHander, ReceiverOptions, cts.Token);

            AddHandler<MessageHandler>(UpdateType.Message);
            AddHandler<CallbackQueryHandler>(UpdateType.CallbackQuery);
            AddHandler<ChannelPostHandler>(UpdateType.ChannelPost);
        }
        public void StopReceiving()
        {
            LoggerService.Logger?.LogInformation("Stop receiving");

            RemoveHandler(UpdateType.Message);
            RemoveHandler(UpdateType.CallbackQuery);
            RemoveHandler(UpdateType.ChannelPost);
        }

        public void AddHandler<THandler>(object key) where THandler : class, IHandler, new()
        {
            var keyValue = Global.Key(key);

            THandler handler = new();
            handler.SetClient(this);

            LoggerService.Logger?.LogInformation($"Add handler {Convert.ToString(handler)} for the key {key} ({keyValue})");

            _handlersRouter.Add(keyValue, handler);
        }
        public void RemoveHandler(object key)
        {
            var keyValue = Global.Key(key);
            LoggerService.Logger?.LogInformation($"Remove handler for the key {key} ({keyValue})");

            _handlersRouter.Remove(keyValue);
        }
        public bool HandleAvail(object key)
        {
            var keyValue = Global.Key(key);
            return _handlersRouter.Exist(keyValue);
        }

        private IEnumerable<object>? ConvertArguments(object? state)
        {
            if (state == null) return null;

            if (state.GetType().IsArray)
            {
                object[] array = (object[])state;

                return array.ToList();
            }

            if (state is IEnumerable enumerable)
            {
                return (IEnumerable<object>?)enumerable;
            }

            return new object[] { state };
        }
        private IEnumerable<object>? ConvertArguments(params object[] args) => ConvertArguments(args);

        private IEnumerable? MergeArguments(object? args1, object? args2)
        {
            if (args1 != null || args2 != null)
            {
                var enumerable_1 = ConvertArguments(args1);
                var enumerable_2 = ConvertArguments(args2);

                var result = new List<object>();

                if (enumerable_1 != null)
                {
                    foreach (var item in enumerable_1)
                    {
                        if (!result.Contains(item))
                            result.Add(item);
                    }
                }

                if (enumerable_2 != null)
                {
                    foreach (var item in enumerable_2)
                    {
                        if (!result.Contains(item))
                            result.Add(item);
                    }
                }

                return result;
            }

            return null;
        }
        private async Task ExecuteHandlePrim(object key, object? state, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var keyValue = Global.Key(key);

            if (_handlersRouter.Exist(keyValue))
            {
                var handler = _handlersRouter.Get(keyValue);

#if DEBUG
                LoggerService.Logger?.LogDebug($"Execute handler <{handler.ToString()}> for {key.ToString()}");
#endif
                LoggerService.Logger?.LogInformation($"Execute handler {Convert.ToString(handler)} for the key {key} ({keyValue})");

                var arguments = ConvertArguments(state);

                handler.SetArguments(arguments);
                await handler.HandleProcessAsync(botClient, update, cancellationToken);
            }
        }
        public async Task ExecuteHandler(object key, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (key is Command command)
            {
                await ExecuteHandlePrim(command.Name, command.Arguments, botClient, update, cancellationToken);
            }
            else
            {
                await ExecuteHandlePrim(key, null, botClient, update, cancellationToken);
            }
        }
        public async Task ExecuteHandler(object key, object args, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (key is Command command)
            {
                await ExecuteHandlePrim(command.Name, MergeArguments(args, command.Arguments), botClient, update, cancellationToken);
            }
            else
            {
                await ExecuteHandlePrim(key, args, botClient, update, cancellationToken);
            }
        }

        public void StartScheduler(TimeSpan dueTime, TimeSpan period, object state, SchedulerHandle handle)
        {
            StopScheduler();

            _timer = new Timer((state) =>
                {
                    LoggerService.Logger?.LogInformation($"Start schedule timer by every {period} with handle {Convert.ToString(handle)}");
                    BotContract? contract = state as BotContract;

                    if (contract != null)
                    {
                        handle.Invoke(contract.BotClient.ThrowIfNull(), contract.Update.ThrowIfNull(), contract.CancellationToken);
                    }
                },
                state,
                dueTime, // period,
                period);
        }
        public void StartScheduler(TimeSpan dueTime, TimeSpan period, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, SchedulerHandle handle)
        {
            var contract = new BotContract
            {
                BotClient = botClient,
                Update = update,
                CancellationToken = cancellationToken
            };

            StartScheduler(dueTime, period, contract, handle);
        }
        public void StopScheduler()
        {
            LoggerService.Logger?.LogInformation($"Stop schedule timer");
            _timer?.Dispose();
            _timer = null;
        }
        public bool SchedulerActive => _timer != null;

        public async Task SendTextMessage(ChatId chatId, string message)
        {
            try
            {
                LoggerService.Logger?.LogInformation($"Chat Id: {chatId}, message: {message}");
                await BotClient.SendTextMessageAsync(chatId, message);
            }
            catch (Exception ex)
            {
                LoggerService.Logger?.LogCritical(ex, "Send message exception");
            }
        }
        public async Task SendTextMessage(ChatId chatId, string message, IReplyMarkup replyMarkup)
        {
            try
            {
                LoggerService.Logger?.LogInformation($"Chat Id: {chatId}, message: {message}");
                await BotClient.SendTextMessageAsync(chatId, message, replyMarkup: replyMarkup);
            }
            catch (Exception ex)
            {
                LoggerService.Logger?.LogCritical(ex, "Send message exception");
            }
        }

        public async Task SendTextMessage(ITelegramBotClient botClient, ChatId chatId, string message)
        {
            try
            {
                LoggerService.Logger?.LogInformation($"Chat Id: {chatId}, message: {message}");
                await botClient.SendTextMessageAsync(chatId, message);
            }
            catch (Exception ex)
            {
                LoggerService.Logger?.LogCritical(ex, "Send message exception");
            }
        }

        public async Task SendCommand(ChatId chatId, string command)
        {
            await SendTextMessage(chatId, command);
        }
        public async Task SendCommand(long chatId, string command)
        {
            await SendTextMessage(chatId, command);
        }
        public async Task SendMessage(long chatId, string message)
        {
            await SendTextMessage(chatId, message);
        }
    }
}
