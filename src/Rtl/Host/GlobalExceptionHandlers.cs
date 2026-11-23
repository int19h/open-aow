using Microsoft.Extensions.Logging;

namespace Aow.Rtl.Host;

public static class GlobalExceptionHandlers
{
    public static void Install(ILoggerFactory factory)
    {
        ILogger logger = factory.CreateLogger("OpenAoW.Startup");
        AppDomain.CurrentDomain.UnhandledException += (_, arguments) =>
        {
            try
            {
                Exception error = arguments.ExceptionObject as Exception ?? new InvalidOperationException("Unhandled non-exception object");
                HostLog.UnhandledException(logger, error: error);
            }
            catch
            {
            }
            finally
            {
                try
                {
                    factory.Dispose();
                }
                catch
                {
                }
            }
        };
        TaskScheduler.UnobservedTaskException += (_, arguments) =>
        {
            try
            {
                HostLog.UnobservedTaskException(logger, error: arguments.Exception);
                arguments.SetObserved();
            }
            catch
            {
            }
        };
    }
}
