using BotApp.Logger.Api;
using BotApp.Service;
using BotApp.Telegram.Api;

namespace BotApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            LoggerService.InitLogger<Application>();
            BotService.Start();

            Application.Run(new MainForm());
        }
    }
}