using BotApp.Logger.Api;
using BotApp.Service.Api.Consts;
using BotApp.Service.Api.Controls;
using BotApp.Service.Api.Extensions;
using BotApp.Service.Api.Utils;
using BotApp.Telegram.Api.Handlers;
using BotApp.Telegram.Api.Telegram;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

namespace BotApp.Service.Api.Handlers
{
    internal class SetTimezoneOffsetHandler : Handler
    {
        const string PAGE = "page";
        const string SET = "set";
        const string RETURN = "return";

        const int ItemsPerPage = 8;
        const int ButtonsPerRow = 1;


        private readonly ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();

        private int timeZonesCount => timeZones?.Count ?? 0;
        private int pagesCount => (timeZonesCount / ItemsPerPage) + (((timeZonesCount % ItemsPerPage) == 0) ? 0 : 1);

        private async Task SendEditabledMessage(ITelegramBotClient botClient, Update update, string text)
        {
            long chatId = GetId(update);

            if (chatId == 0) return;

            var lastMessage = TelegramClient.GetLastMessge(this);
            var lastMessageId = lastMessage?.MessageId ?? 0;

            if (lastMessageId == 0)
            {
                var message = await botClient.SendTextMessageAsync(chatId, text);
                TelegramClient.SetLastMessge(this, message);
            }
            else
            {
                var message = await botClient.EditMessageTextAsync(chatId, lastMessageId, text);
                TelegramClient.SetLastMessge(this, message);
            }
        }
        private async Task SendEditabledMessage(ITelegramBotClient botClient, Update update, string text, IEnumerable<InlineKeyboardButton> buttons)
        {
            long chatId = GetId(update);

            if (chatId == 0) return;

            var lastMessage = TelegramClient.GetLastMessge(this);
            var lastMessageId = lastMessage?.MessageId ?? 0;

            if (lastMessageId == 0)
            {
                var replyMarkup = new InlineKeyboardMarkup(buttons);
                var message = await botClient.SendTextMessageAsync(chatId, text, replyMarkup: replyMarkup);
                TelegramClient.SetLastMessge(this, message);
            }
            else
            {
                var inlineKeyboardMarkup = new InlineKeyboardMarkup(buttons);
                var message = await botClient.EditMessageTextAsync(chatId, lastMessageId, text, replyMarkup: inlineKeyboardMarkup);
                TelegramClient.SetLastMessge(this, message);
            }
        }
        private async Task SendEditabledMessage(ITelegramBotClient botClient, Update update, string text, IEnumerable<IEnumerable<InlineKeyboardButton>> buttons)
        {
            long chatId = GetId(update);

            if (chatId == 0) return;

            var lastMessage = TelegramClient.GetLastMessge(this);
            var lastMessageId = lastMessage?.MessageId ?? 0;

            if (lastMessageId == 0)
            {
                var replyMarkup = new InlineKeyboardMarkup(buttons);
                var message = await botClient.SendTextMessageAsync(chatId, text, replyMarkup: replyMarkup);
                TelegramClient.SetLastMessge(this, message);
            }
            else
            {
                var inlineKeyboardMarkup = new InlineKeyboardMarkup(buttons);
                var message = await botClient.EditMessageTextAsync(chatId, lastMessageId, text, replyMarkup: inlineKeyboardMarkup);
                TelegramClient.SetLastMessge(this, message);
            }
        }
        private IEnumerable<IEnumerable<InlineKeyboardButton>> MakeButtonsMenu(int pageIndex, int pageSize)
        {
            var buttonsTable = new List<InlineKeyboardButton[]>();

            var startIndex = pageIndex * pageSize;

            if (pageIndex > 0)
            {
                buttonsTable.Add(
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("<< Prev page", string.Format("{0} {1} {2}", Keyboard.COMMAND_SET_TIMEZONE, PAGE, pageIndex - 1)),
                        InlineKeyboardButton.WithCallbackData("Return", string.Format("{0} {1}", Keyboard.COMMAND_SET_TIMEZONE, RETURN)),
                    });
            }
            else
            {
                buttonsTable.Add(
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Return", string.Format("{0} {1}", Keyboard.COMMAND_SET_TIMEZONE, RETURN)),
                    });
            }

            var buttonsRow = new List<InlineKeyboardButton>();
            var selectedTimeZone = GlobalServiceData.TimeZone ?? TimeZoneInfo.Utc;

            for (int index = 0; index < ItemsPerPage; index++)
            {
                if ((index + startIndex) >= timeZones.Count) break;

                if (ButtonsPerRow == 1 || (index % ButtonsPerRow) == 0)
                {
                    if (index > 0) buttonsTable.Add(buttonsRow.ToArray());

                    buttonsRow.Clear();
                }

                var timeZone = timeZones[startIndex + index];
                var buttonCaption = timeZone == selectedTimeZone
                    ? string.Format("** {0} **", timeZone.DisplayName)
                    : timeZone.DisplayName;

                LoggerService.Logger?.LogInformation("{0} ({1}) => {2}", timeZone.DisplayName, timeZone.BaseUtcOffset, buttonCaption);

                InlineKeyboardButton inlineKeyboardButton = InlineKeyboardButton
                    .WithCallbackData(
                buttonCaption,
                        string.Format("{0} {1} {2}", Keyboard.COMMAND_SET_TIMEZONE, SET, startIndex + index));

                buttonsRow.Add(inlineKeyboardButton);
            }

            if (buttonsRow.Count > 0)
                buttonsTable.Add(buttonsRow.ToArray());

            if (pageIndex < (pagesCount - 1))
            {
                buttonsTable.Add(
                    new InlineKeyboardButton[]
                    {
                        InlineKeyboardButton.WithCallbackData("Next page >>", string.Format("{0} {1} {2}", Keyboard.COMMAND_SET_TIMEZONE, PAGE, pageIndex + 1))
                    });
            }

            return buttonsTable;
        }


        public override Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override async Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            int pageIndex = 0;
            int startIndex = 0;
            int index = 0;
            
            index = timeZones.IndexOf(GlobalServiceData.TimeZone ?? TimeZoneInfo.Utc);

            if (index >= 0 && index < timeZones.Count)
            {
                pageIndex = index / ItemsPerPage;
                startIndex = (pageIndex * ItemsPerPage);
            }

            if (Arguments == null || Arguments.Count() == 0)
            {
                TelegramClient.SetLastMessge(this, null);
            }

            string argument = FindArgument<string>(0, out index) ?? string.Empty;

            if (argument.Equals(SET, StringComparison.OrdinalIgnoreCase))
            {
                string value = FindArgument<string>(index + 1, out index) ?? string.Empty;
                
                if (!value.IsInteger()) value = "0";

                index = int.Parse(value);

                var timeZone = timeZones[index];

                GlobalServiceData.TimeZone = timeZone;

                await SendEditabledMessage(botClient, update, string.Format("Select timezone: {0}", timeZone.DisplayName));

                TelegramClient.SetLastMessge(this, null);
                return;
            }
            
            if (argument.Equals(RETURN, StringComparison.OrdinalIgnoreCase))
            {
                await SendEditabledMessage(botClient, update, "The operation was canceled by the user.");

                TelegramClient.SetLastMessge(this, null);
                return;
            }

            if (argument.Equals(PAGE, StringComparison.OrdinalIgnoreCase))
            {
                string value = FindArgument<string>(index + 1, out index) ?? string.Empty;
                if (string.IsNullOrEmpty(value) || !value.IsInteger()) value = "0";

                pageIndex = int.Parse(value);
                pageIndex = Math.Max(Math.Min(pageIndex, pagesCount - 1), 0);
            }

            var buttonsTable = MakeButtonsMenu(pageIndex, ItemsPerPage);

            await SendEditabledMessage(botClient, update, string.Format("Time zones (page {0})", pageIndex + 1), buttonsTable);
        }

        public override Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
