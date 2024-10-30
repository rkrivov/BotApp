using BotApp.Logger.Api;
using BotApp.Telegram.Api.Contracts;
using BotApp.Telegram.Api.Extensions;
using BotApp.Telegram.Api.Telegram;
using BotApp.Telegram.Api.Utils;
using Microsoft.Extensions.Logging;
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
    public abstract class Handler : IHandler
    {
        private ITelegramClient? client = null;
        private IEnumerable<object>? arguments = null;
        private bool disposed = false;

        public ITelegramClient TelegramClient => client.ThrowIfNull();
        public IEnumerable<object> Arguments => arguments.ThrowIfNull();
        public int ArgumentsCount => arguments?.Count() ?? 0;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed || !disposing) return;
            disposed = true;
        }

        ~Handler()
        {
            Dispose(false);
        }

        protected object? GetArgument(int index)
        {
            if (arguments == null) return null;
            if (index < 0) return null;
            if (index >= arguments.Count()) return null;

            var array = arguments!.ToArray();

            return array[index];
        }
        protected object? FindArgument(Type type)
        {
            object? result = null;

            if (arguments == null) return result;
            if (arguments.Count() == 0) return result;

            var array = arguments!.ToArray();

            for (int ix = 0; ix < array.Length; ix++)
            {
                if (array[ix] != null && array[ix].GetType() == type)
                    return array[ix];
            }

            return result;
        }
        protected object? FindArgument(Type type, int startIndex, out int index)
        {
            index = -1;
            object? result = null;

            if (arguments == null) return result;
            if (arguments.Count() == 0) return result;
            if (startIndex >= arguments.Count()) return result;

            var array = arguments!.ToArray();

            for (int ix = startIndex; ix < array.Length; ix++)
            {
                if (array[ix] != null && array[ix].GetType() == type)
                {
                    index = 0;
                    result =  array[ix];

                    break;
                }
            }

            return result;
        }
        protected object? FindArgument(Type type, out int index)
        {
            index = -1;
            object? result = null;

            if (arguments == null) return result; 
            if (arguments.Count() == 0) return result;

            var array = arguments!.ToArray();

            for (int ix = 0; ix < array.Length; ix++)
            {
                if (array[ix] != null && array[ix].GetType() == type)
                {
                    index = ix;
                    result = array[ix];
                    
                    break;
                }
            }

            if (result == null) return null;

            var list = arguments!.ToList();
            list.RemoveAt(index);
            arguments = list;

            return result;
        }
        protected T? FindArgument<T>()
        {
            object? argument = FindArgument(typeof(T));

            return argument == null ? default : (T?)argument;
        }
        protected T? FindArgument<T>(int startIndex, out int index)
        {
            object? argument = FindArgument(typeof(T), startIndex, out index);

            return argument == null ? default : (T?)argument;
        }
        protected T? FindArgument<T>(out int index)
        {
            object? argument = FindArgument(typeof(T), out index);

            return argument == null ? default : (T?)argument;
        }

        protected async Task SendMessage(ITelegramBotClient botClient, Update update, string message)
        {
            var id = GetId(update);

            if (id == 0) return;
            if (string.IsNullOrEmpty(message)) return;

            await botClient.SendTextMessageAsync(id, message, parseMode: ParseMode.Html);
        }
        protected async Task SendMessage(ITelegramBotClient botClient, Update update, string fmt, params object[] args)
        {
            var id = GetId(update);

            if (id == 0) return;
            if (string.IsNullOrEmpty(fmt)) return;

            await botClient.SendTextMessageAsync(id, string.Format(fmt, args), parseMode: ParseMode.Html);
        }
        protected async Task SendMessage(ITelegramBotClient botClient, Update update, Exception exception)
        {
            LoggerService.Logger?.LogError(exception, "Exception in {1}: {0}\n{2}\n", exception.Message, exception.Source, exception.StackTrace);

            var id = GetId(update);

            if (id == 0) return;
            if (exception == null) return;

            await botClient.SendTextMessageAsync(id, string.Format("<b>Exception</b>: <em>{0}</em> (<code>{1}</code>).", exception.Message, exception.Source), parseMode: ParseMode.Html);
        }

        protected long GetId(Update? update) => Global.GetId(update); 
        
        public void SetClient(ITelegramClient client)
        {
            this.client = client;
        }
        public void SetArguments(IEnumerable<object>? arguments)
        {
            this.arguments = arguments; 
        }

        public abstract Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        public abstract Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
        public abstract Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken);
    }
}
