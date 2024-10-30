using BotApp.Telegram.Api.Telegram;

namespace BotApp.Telegram.Api
{
    public static class TelegramService
    {
        public static ITelegramClient GetTelegramClient(string tiken) => new TelegramClient(tiken);
    }
}
