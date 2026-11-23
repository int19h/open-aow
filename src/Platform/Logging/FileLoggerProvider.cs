using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Aow.Platform.Logging;

public sealed record FileLoggerSettings(string Directory, int RetainedFileCount, string? StartupError = null)
{
    public const string DirectoryVariable = "OPENAOW_LOG_DIRECTORY";
    public const string RetainedFileCountVariable = "OPENAOW_LOG_RETAINED_COUNT";
    public const int MaximumRetainedFileCount = 100;

    public static FileLoggerSettings FromEnvironment(string defaultDirectory, bool honorOverrides = true)
    {
        try
        {
            string? directory = honorOverrides ? Environment.GetEnvironmentVariable(DirectoryVariable) : null;
            string? retainedText = honorOverrides ? Environment.GetEnvironmentVariable(RetainedFileCountVariable) : null;
            int retained = 10;
            string? error = null;
            if (!string.IsNullOrWhiteSpace(retainedText) && (!int.TryParse(retainedText, style: NumberStyles.Integer, provider: CultureInfo.InvariantCulture, result: out retained) || retained < 0 || retained > MaximumRetainedFileCount))
            {
                retained = 10;
                error = $"{RetainedFileCountVariable} must be an integer from 0 to {MaximumRetainedFileCount}. Using {retained}.";
            }

            string selected = string.IsNullOrWhiteSpace(directory) ? defaultDirectory : directory;
            try
            {
                return new(Path.GetFullPath(selected), RetainedFileCount: retained, StartupError: error);
            }
            catch (Exception pathError)
            {
                string message = $"Log directory {selected} is invalid: {pathError.Message}";
                return new(Path.GetFullPath(defaultDirectory), RetainedFileCount: retained, StartupError: error is null ? message : $"{error} {message}");
            }
        }
        catch (Exception error)
        {
            return new(defaultDirectory, RetainedFileCount: 10, StartupError: error.Message);
        }
    }
}

public sealed class FileLoggerProvider : ILoggerProvider
{
    private readonly Lock _lock = new();
    private readonly StreamWriter _writer;
    private bool _disposed;

    public FileLoggerProvider(FileLoggerSettings settings)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(settings.RetainedFileCount);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(settings.RetainedFileCount, other: FileLoggerSettings.MaximumRetainedFileCount);
        _ = Directory.CreateDirectory(settings.Directory);
        string current = Path.Combine(settings.Directory, "openaow.log");
        try
        {
            if (File.Exists(current))
            {
                using FileStream probe = new(current, mode: FileMode.Open, access: FileAccess.ReadWrite, share: FileShare.ReadWrite);
            }

            RollFiles(settings, current);
            _writer = Open(current);
            FilePath = current;
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException)
        {
            string fallback = Path.Combine(settings.Directory, $"openaow.{Environment.ProcessId}.log");
            _writer = Open(fallback);
            FilePath = fallback;
            StartupError = $"Log file {current} is unavailable: {error.Message} Using {fallback}.";
        }
    }

    public string FilePath { get; }

    public string? StartupError { get; }

    public ILogger CreateLogger(string categoryName) => new FileLogger(this, categoryName);

    public void Dispose()
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _writer.Dispose();
        }
    }

    private static StreamWriter Open(string path) => new(new FileStream(path, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.Read), encoding: new UTF8Encoding(false))
    {
        AutoFlush = true,
        NewLine = "\n"
    };

    private static void RollFiles(FileLoggerSettings settings, string current)
    {
        foreach (string path in Directory.EnumerateFiles(settings.Directory, searchPattern: "openaow.*.log"))
        {
            if (int.TryParse(Path.GetFileNameWithoutExtension(path).AsSpan("openaow.".Length), style: NumberStyles.Integer, provider: CultureInfo.InvariantCulture, result: out int index) && index > settings.RetainedFileCount)
            {
                File.Delete(path);
            }
        }

        if (!File.Exists(current))
        {
            return;
        }

        if (settings.RetainedFileCount == 0)
        {
            File.Delete(current);
            return;
        }

        File.Delete(Path.Combine(settings.Directory, $"openaow.{settings.RetainedFileCount}.log"));
        for (int index = settings.RetainedFileCount - 1; index >= 1; index--)
        {
            string source = Path.Combine(settings.Directory, $"openaow.{index}.log");
            if (File.Exists(source))
            {
                File.Move(source, destFileName: Path.Combine(settings.Directory, $"openaow.{index + 1}.log"), overwrite: true);
            }
        }

        if (File.Exists(current))
        {
            File.Move(current, destFileName: Path.Combine(settings.Directory, "openaow.1.log"), overwrite: true);
        }
    }

    private sealed class FileLogger(FileLoggerProvider provider, string categoryName) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            try
            {
                lock (provider._lock)
                {
                    if (provider._disposed)
                    {
                        return;
                    }

                    provider._writer.Write(DateTimeOffset.UtcNow.ToString("O", formatProvider: CultureInfo.InvariantCulture));
                    provider._writer.Write(' ');
                    provider._writer.Write(logLevel);
                    provider._writer.Write(' ');
                    provider._writer.Write(categoryName);
                    provider._writer.Write(": ");
                    provider._writer.WriteLine(formatter(state, exception));
                    if (exception is not null)
                    {
                        provider._writer.WriteLine(exception);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
