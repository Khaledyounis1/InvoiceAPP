using Serilog;
using Serilog.Events;
using System.Runtime.CompilerServices;


namespace InvoiceApp.Service.Logs
{
    public static class LoggerExtensions
    {
        public static void LogErorr(this Serilog.ILogger logger,  string message, [CallerMemberName] string methodName = "")
        {
            logger
                .ForContext("MethodName", methodName)
                .Write(LogEventLevel.Error, message);
        }
        public static void LogInfo(this Serilog.ILogger logger, string message, [CallerMemberName] string methodName = "")
        {
            logger
                .ForContext("MethodName", methodName)
                .Write(LogEventLevel.Information, message);
        }
    }
}
