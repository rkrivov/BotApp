using BotApp.Logger.Api;
using BotApp.Service;
using Microsoft.Extensions.Logging;

internal class Program
{
    private static async Task Main(string[] args)
    {
        LoggerService.InitLogger<Program>();

        BotService.Start();

        await Task.Delay(-1);
    }
}