using BotApp.Telegram.Api.Extensions;
using BotApp.Telegram.Api.Handlers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotApp.Telegram.Api.Routes
{
    internal class HandlersRouter<TKey, THandler> : Route<TKey, THandler>
        where TKey : notnull
        where THandler : class, IHandler
    {
        internal Task? Execute(TKey key, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            THandler? handler = Exist(key.ThrowIfNull(nameof(key))) ? Get(key) : null;

            return handler?.HandleProcessAsync(
                botClient.ThrowIfNull(nameof(botClient)), 
                update.ThrowIfNull(nameof(update)), 
                cancellationToken);
        }

    }
}
