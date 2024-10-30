using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotApp.Telegram.Api.Contracts
{
    internal class HandlerContract
    {
        public ITelegramBotClient? BotClient { get; set; } = null;
        public Update? Update { get; set; } = null;
        public CancellationToken CancellationToken { get; set; } = default;
    }
}
