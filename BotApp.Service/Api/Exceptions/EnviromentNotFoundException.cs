using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Service.Api.Exceptions
{
    internal class EnviromentNotFoundException : Exception
    {
        public EnviromentNotFoundException(string variableName) : base(string.Format("The system environment variable {0} was not found.", variableName)) { }
    }
}
