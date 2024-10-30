using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotApp.Telegram.Api.Contracts
{
    public class BotContract
    {
        public ITelegramBotClient? BotClient { get; set; }
        public Update? Update { get; set; }  
        public CancellationToken CancellationToken { get; set; }
    }
}
