using BotApp.Service.Api.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Service.Api.Consts
{
    internal static class AuthorizeTokensConsts
    {
        public readonly static string BotToken = Environment.GetEnvironmentVariable("BOTTOKEN") ?? throw new EnviromentNotFoundException("BOTTOKEN");
        
        public readonly static string TDAccessKey = Environment.GetEnvironmentVariable("TDACCESSKEY") ?? throw new EnviromentNotFoundException("TDACCESSKEY");
        public readonly static string TDSecretKey = Environment.GetEnvironmentVariable("TDSECRETKEY") ?? throw new EnviromentNotFoundException("TDSECRETKEY");
    }
}
