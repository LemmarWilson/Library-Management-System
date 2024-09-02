using Microsoft.Extensions.Logging;
using System;

namespace Library_Management_System.Logging
{
    public static class LoggingConfig
    {
        private static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
        });

        public static ILogger<T> CreateLogger<T>() => loggerFactory.CreateLogger<T>();
    }
}
