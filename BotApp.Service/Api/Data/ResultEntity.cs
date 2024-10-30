using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Service.Api.Data
{
    [Serializable]  
    internal class ResultEntity
    {
        public SunriseSunsetEntity? results { get; set; }
    
        public string status { get; set; } = string.Empty;
        public string tzid { get; set; } = string.Empty;
    }
}
