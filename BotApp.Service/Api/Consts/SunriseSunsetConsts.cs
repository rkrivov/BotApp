using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Service.Api.Consts
{
    internal static class SunriseSunsetConsts
    {
        public const string BASE_URL = "https://api.sunrise-sunset.org/";
        public const string STATUS_OK = "OK";

        public const string PATH_JSON = "./json";

        public const string PARAM_LAT = "lat";
        public const string PARAM_LNG = "lng";
        public const string PARAM_DATE = "date";
        public const string PARAM_FORMATTED = "formatted";
        public const string PARAM_TZ = "tzid";
    }
}
