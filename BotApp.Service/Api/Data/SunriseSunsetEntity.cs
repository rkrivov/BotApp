using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Service.Api.Data
{
    [Serializable]
    internal class SunriseSunsetEntity
    {
        public string sunrise { get; set; } = string.Empty;
        public string sunset { get; set; } = string.Empty;

        public string solar_noon { get; set; } = string.Empty;

        public int day_length { get; set; } = 0;

        public string civil_twilight_begin { get; set; } = string.Empty;

        public string civil_twilight_end { get; set; } = string.Empty;

        public string nautical_twilight_begin { get; set; } = string.Empty;

        public string nautical_twilight_end { get; set; } = string.Empty;

        public string astronomical_twilight_begin { get; set; } = string.Empty;

        public string astronomical_twilight_end { get; set; } = string.Empty;

    }
}
