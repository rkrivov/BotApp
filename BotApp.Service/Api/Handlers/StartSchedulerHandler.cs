using BotApp.Service.Api.Extensions;
using BotApp.Service.Api.Utils;
using BotApp.Service.Api.SunrizeSunet;
using BotApp.Telegram.Api.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using BotApp.Telegram.Api.Utils;

namespace BotApp.Service.Api.Handlers
{
    internal class StartSchedulerHandler : ServiceHandler
    {
        public override Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task DoProcess(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                await ProcessHandle(botClient, update!, cancellationToken);
            }
            catch (Exception ex)
            {
                await SendMessage(botClient, update!, ex);
            }
            finally
            {
                await SendFooter(botClient, update!, cancellationToken);
            }
        }

        public override Task HandleProcessAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var id = GetId(update);
            var locaion = GlobalServiceData.GetLocation(id);

            if (locaion == null)
            {
                SendMessage(botClient, update, "First, send your coordinates.").Wait();

                UIFunctions.SetCommands(TelegramClient).Wait();
                return Task.CompletedTask;
            }

            var dueTime = TimeSpan.Zero;
            TimeSpan period = TimeSpan.Zero;

            if (period == TimeSpan.Zero)
            {
                int intValue = FindArgument<int>();

                if (intValue != 0)
                    period = new TimeSpan(0, intValue, 0);
            }

            if (period == TimeSpan.Zero)
            {
                var longValue = FindArgument<long>();

                if (longValue != 0)
                    period = new TimeSpan(longValue);
            }


            if (period == TimeSpan.Zero)
            {
                var stringValue = FindArgument<string>();

                if (!string.IsNullOrEmpty(stringValue))
                {
                    int minutes = 0;

                    if (int.TryParse(stringValue, out minutes))
                        period = new TimeSpan(0, minutes, 0);
                    else
                    {
                        if (!TimeSpan.TryParse(stringValue, out period))
                            return Task.CompletedTask;
                    }
                }
            }

            if (period == TimeSpan.Zero)
            {
                period = new TimeSpan(0, 5, 0);
            }

            var dateTime = DateTime.Now;
            var dateTimeTicks = dateTime.Ticks;
            var periodTicks = period.Ticks;

            if ((dateTime.Ticks % periodTicks) != 0)
            {
                var ticks = dateTimeTicks / periodTicks;
                ticks = ticks + 1;
                ticks = ticks * periodTicks;
                ticks = ticks - dateTimeTicks;

                dueTime = new TimeSpan(ticks);
            }

            TelegramClient.StartScheduler(
                dueTime,
                period,
                botClient, update, cancellationToken,
                async (botClient, update, cancellationToken) => await DoProcess(botClient, update, cancellationToken));


            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Starting the timer for every {period.ToString(@"hh\:mm\:ss")}");

            if (dueTime != TimeSpan.Zero)
            {
                stringBuilder.AppendLine($"The nearest launch is in {dueTime.ToString(@"hh\:mm\:ss")}");
            }

            TelegramClient.SendTextMessage(botClient, Global.GetId(update), stringBuilder.ToString()).Wait();

            UIFunctions.SetCommands(TelegramClient).Wait();

            return Task.CompletedTask;
        }

        public override Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
