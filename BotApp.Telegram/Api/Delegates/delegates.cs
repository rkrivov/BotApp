using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace BotApp.Telegram.Api.Delegates
{
    public delegate Task CommandHandle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    public delegate Task SchedulerHandle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    public delegate Task LoggerHandle(string messahe);
}
