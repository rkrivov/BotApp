using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Service.Api.Exceptions
{
    internal class WrongLocationException : ServiceException
    {
        public WrongLocationException() : base(Properties.Resources.WrongLocation) { }
    }
}
