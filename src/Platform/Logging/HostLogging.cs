using Aow.Rtl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace Aow.Platform.Logging;

public static class HostLogging
{
    public static ILoggerFactory Create(string defaultDirectory, bool honorEnvironmentOverrides = true)
    {
        FileLoggerSettings settings = FileLoggerSettings.FromEnvironment(defaultDirectory, honorOverrides: honorEnvironmentOverrides);
        List<string> errors = [];
        if (settings.StartupError is not null)
        {
            errors.Add(settings.StartupError);
        }

        FileLoggerProvider? fileProvider = null;
        try
        {
            fileProvider = new FileLoggerProvider(settings);
            if (fileProvider.StartupError is not null)
            {
                errors.Add(fileProvider.StartupError);
            }
        }
        catch (Exception error)
        {
            errors.Add($"File logging is unavailable: {error.Message}");
        }

        ILoggerFactory factory;
        try
        {
            factory = LoggerFactory.Create(builder => Configure(builder, fileProvider));
        }
        catch (Exception error)
        {
            fileProvider?.Dispose();
            errors.Add($"The logging providers failed: {error.Message}");
            try
            {
                factory = LoggerFactory.Create(builder => Configure(builder, fileProvider: null));
            }
            catch
            {
                factory = NullLoggerFactory.Instance;
            }
        }

        foreach (string error in errors)
        {
            HostLog.Error(factory.CreateLogger("OpenAoW.Startup"), text: error);
        }

        return new OwnedLoggerFactory(factory, fileProvider);
    }

    private sealed class OwnedLoggerFactory(ILoggerFactory inner, FileLoggerProvider? fileProvider) : ILoggerFactory
    {
        public ILogger CreateLogger(string categoryName) => inner.CreateLogger(categoryName);

        public void AddProvider(ILoggerProvider provider) => inner.AddProvider(provider);

        public void Dispose()
        {
            try
            {
                inner.Dispose();
            }
            finally
            {
                fileProvider?.Dispose();
            }
        }
    }

    private static void Configure(ILoggingBuilder builder, FileLoggerProvider? fileProvider)
    {
        _ = builder.SetMinimumLevel(LogLevel.Debug);
        _ = builder.AddFilter<ConsoleLoggerProvider>(category: null, level: LogLevel.Information);
        _ = builder.AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Warning);
        _ = builder.AddSimpleConsole(options =>
        {
            options.ColorBehavior = LoggerColorBehavior.Disabled;
            options.IncludeScopes = false;
            options.SingleLine = true;
            options.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fff'Z' ";
            options.UseUtcTimestamp = true;
        });
        if (fileProvider is not null)
        {
            _ = builder.AddProvider(fileProvider);
        }
    }
}
