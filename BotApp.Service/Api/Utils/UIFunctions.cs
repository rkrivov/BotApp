using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using BotApp.Service.Api.Controls;
using BotApp.Telegram.Api.Telegram;
using System.Diagnostics;
using BotApp.Telegram.Api.Utils;

namespace BotApp.Service.Api.Utils
{
    internal static class UIFunctions
    {
        internal static async Task SendMainMenu(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var inlineKeyboard = Keyboard.MainMenu();

            var chatId = Global.GetId(update);

            if (chatId == 0) return;

            await botClient.SendTextMessageAsync(
                chatId,
                Properties.Resources.MainMenu,
                replyMarkup: inlineKeyboard);
        }

        internal static async Task SetCommands(ITelegramClient client)
        {
            await client.ClearCommands()
                .ContinueWith(async task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        var commands = new List<BotCommand>();

                        commands.Add(new BotCommand { Command = Keyboard.COMMAND_START, Description = "Start new session" });
                        commands.Add(new BotCommand { Command = Keyboard.COMMAND_KEYBOARD, Description = "Main menu" });
                        commands.Add(new BotCommand { Command = Keyboard.COMMAND_SET_TIMEZONE, Description = "Set timezone" });

                        /*
                        if (GlobalData.Location != null)
                        {
                            commands.Add(new BotCommand { Command = Keyboard.COMMAND_YESTERDAY, Description = Properties.Resources.Yesterday });
                            commands.Add(new BotCommand { Command = Keyboard.COMMAND_TODAY, Description = Properties.Resources.Today });
                            commands.Add(new BotCommand { Command = Keyboard.COMMAND_TOMORROW, Description = Properties.Resources.Tomorrow });

                            if (client.SchedulerActive)
                            {
                                commands.Add(new BotCommand { Command = Keyboard.COMMAND_STOP_SCHEDULER, Description = "Stop scheduler" });
                            }
                            else
                            {
                                commands.Add(new BotCommand { Command = Keyboard.COMMAND_START_SCHEDULER, Description = "Start scheduler" });
                            }
                        }
                        */

                        await client.SetCommands(commands);
                    }
    #if DEBUG
                    else
                    {
                        Debug.Print("Exception: {0}", task.Exception?.Message ?? "Unknown exception");
                    }
    #endif
                });
        }
    }
}
