using BotApp.Logger.Api.Configures;
using Microsoft.Extensions.Logging;

namespace BotApp.Logger.Api.Logger
{
    public sealed class ColorConsoleLogger : ILogger
    {
        private readonly string name;
        private readonly Func<ColorConsoleLoggerConfiguration> getCurrentConfig;

        public ColorConsoleLogger(string name, Func<ColorConsoleLoggerConfiguration> getCurrentConfig)
        {
            this.name = name;
            this.getCurrentConfig = getCurrentConfig;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel) => 
            getCurrentConfig().LogLevelToColorMap.ContainsKey(logLevel);

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            ColorConsoleLoggerConfiguration config = getCurrentConfig();
            if (config.EventId == 0 || config.EventId == eventId.Id)
            {
                ConsoleColor originalColor = Console.ForegroundColor;

                Console.ForegroundColor = config.LogLevelToColorMap[logLevel];
                Console.Write($"{logLevel,-12}");
                Console.ForegroundColor = originalColor;
                Console.Write(" - ");
                Console.Write($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), -20}");
                Console.Write(" - ");
                Console.Write($"{formatter(state, exception)}");
                Console.WriteLine();
            }
        }
    }
}
