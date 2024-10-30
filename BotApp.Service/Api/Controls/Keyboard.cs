using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using System.Security.Cryptography;
using BotApp.Service.Api.Extensions;
using System.Runtime.Serialization;
using BotApp.Service.Api.Consts;

namespace BotApp.Service.Api.Controls
{
    internal static class Keyboard
    {
        internal readonly static string COMMAND_START = "/start";
        internal readonly static string COMMAND_KEYBOARD = "/mainmenu";

        internal readonly static string COMMAND_LOCATION = "/location";
        internal readonly static string COMMAND_YESTERDAY = "/yesterday";
        internal readonly static string COMMAND_TODAY = "/today";
        internal readonly static string COMMAND_TOMORROW = "/tomorrow";

        internal readonly static string COMMAND_START_SCHEDULER = "/start_scheduler";
        internal readonly static string COMMAND_START_SCHEDULER_10 = "/start_scheduler 00:10:00";
        internal readonly static string COMMAND_START_SCHEDULER_30 = "/start_scheduler 00:30:00";
        internal readonly static string COMMAND_START_SCHEDULER_60 = "/start_scheduler 01:00:00";
        internal readonly static string COMMAND_START_SCHEDULER_90 = "/start_scheduler 01:30:00";
        internal readonly static string COMMAND_STOP_SCHEDULER = "/stop_scheduler";

        internal readonly static string COMMAND_SET_TIMEZONE = "/set_timezone";
        internal readonly static string COMMON_MOONPHASE = "/moon";

        internal static InlineKeyboardMarkup MainMenu()
        {
            var datetime = DateTime.Now;

            return new InlineKeyboardMarkup(
                new List<InlineKeyboardButton[]>()
                {
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData(Properties.Resources.SelectTimezone, COMMAND_SET_TIMEZONE),
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData(Properties.Resources.Yesterday, COMMAND_YESTERDAY),
                        InlineKeyboardButton.WithCallbackData(Properties.Resources.Today, COMMAND_TODAY),
                        InlineKeyboardButton.WithCallbackData(Properties.Resources.Tomorrow, COMMAND_TOMORROW),
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData(string.Format(Properties.Resources.MinutesShort, 10), COMMAND_START_SCHEDULER_10),
                        InlineKeyboardButton.WithCallbackData(string.Format(Properties.Resources.MinutesShort, 30), COMMAND_START_SCHEDULER_30),
                        InlineKeyboardButton.WithCallbackData(string.Format(Properties.Resources.MinutesShort, 60), COMMAND_START_SCHEDULER_60),
                        InlineKeyboardButton.WithCallbackData(string.Format(Properties.Resources.MinutesShort, 90), COMMAND_START_SCHEDULER_90),
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData(Properties.Resources.StopScheduler, COMMAND_STOP_SCHEDULER),
                    },
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Moon phase", COMMON_MOONPHASE),
                    },
                });
        }
    }
}
