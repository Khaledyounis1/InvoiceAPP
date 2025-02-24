using Serilog;
using Serilog.Events;
using System.Runtime.CompilerServices;


namespace InvoiceAPI.Logs
{
    public static class LoggerExtensions
    {
        public static void LogWithMethodName(this Serilog.ILogger logger, LogEventLevel level, string message, [CallerMemberName] string methodName = "")
        {
            logger
                .ForContext("MethodName", methodName)
                .Write(level, message);
        }
    }
}
