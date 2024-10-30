using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Service.Api.Exceptions
{
    internal class LocationNotReceiptException : ServiceException
    {
        public LocationNotReceiptException() : base(Properties.Resources.PleaseSendLocation) { }
    }
}
