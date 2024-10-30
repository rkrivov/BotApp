using BotApp.Logger.Api.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Logger.Api
{
    public static class LoggerService
    {
        private static ILogger? _logger = null;
        public static ILogger? Logger => _logger;

        private static ILoggingBuilder Configure(ILoggingBuilder loggerBuilder)
        {
            loggerBuilder.ClearProviders();
            loggerBuilder.AddColorConsoleLogger(configuration =>
                {
                    configuration.LogLevelToColorMap[LogLevel.Information] = ConsoleColor.Cyan;
                    configuration.LogLevelToColorMap[LogLevel.Debug] = ConsoleColor.Gray;
                    configuration.LogLevelToColorMap[LogLevel.Warning] = ConsoleColor.DarkYellow;
                    configuration.LogLevelToColorMap[LogLevel.Error] = ConsoleColor.DarkRed;
                });

            return loggerBuilder;
        }

        public static ILogger BuildLogger<T>()
        {
            using ILoggerFactory factory = LoggerFactory.Create(builder => Configure(builder));

            return factory.CreateLogger<T>();
        }

        public static void ClearLogger()
        {
            _logger = null;
        }
        public static void SetLogger(ILogger? logger)
        {
            _logger = logger;
        }

        public static void InitLogger<T>()
        {
            _logger = BuildLogger<T>();
        }
    }
}
