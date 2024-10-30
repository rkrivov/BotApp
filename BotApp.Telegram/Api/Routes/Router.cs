using BotApp.Telegram.Api.Delegates;
using BotApp.Telegram.Api.Handlers;
using Telegram.Bot.Types.Enums;

namespace BotApp.Telegram.Api.Routes
{
    internal static class Router
    {
        internal readonly static Route<string, CommandHandle> CommandRoute = new();
        internal readonly static Route<MessageType, CommandHandle> MessageTypeRoute = new();
        internal readonly static HandlersRouter<object, IHandler> Handlers = new();
    }
}
