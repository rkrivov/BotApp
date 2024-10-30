using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Telegram.Api.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }
        List<string> Arguments { get; }
    }
}
