using System;
using JsonViewerApp.Interfaces;
using Serilog;

namespace JsonViewerApp.Services
{
    /// <summary>
    ///     Статический класс для централизованного управления логированием в приложении.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        static LoggerService()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        /// <inheritdoc />
        public void Error(Exception ex, string message)
        {
            Log.Error(ex, message);
        }

        /// <inheritdoc />
        public void Information(string message)
        {
            Log.Information(message);
        }

        /// <inheritdoc />
        public void CloseAndFlush()
        {
            Log.CloseAndFlush();
        }
    }
}