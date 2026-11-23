using System.Collections.Concurrent;
using Aow.Platform.Logging;
using Microsoft.Extensions.Logging;

namespace Aow.Rtl.Tests;

public sealed class LoggingTests
{
    [Fact]
    public void TestLogDirectoryRollsIndependentlyOfPlaytestLogs()
    {
        string root = TemporaryDirectory();
        string playtest = Path.Combine(root, "playtest");
        string tests = Path.Combine(root, "tests");
        try
        {
            for (int run = 0; run < 13; run++)
            {
                using FileLoggerProvider provider = new(new(tests, RetainedFileCount: 10));
                HostLog.Information(provider.CreateLogger("Test"), text: $"run {run}");
            }

            string[] files = Directory.GetFiles(tests, searchPattern: "openaow*.log");
            Assert.Equal(11, files.Length);
            Assert.Contains("run 12", File.ReadAllText(Path.Combine(tests, "openaow.log")));
            Assert.Contains("run 11", File.ReadAllText(Path.Combine(tests, "openaow.1.log")));
            Assert.Contains("run 2", File.ReadAllText(Path.Combine(tests, "openaow.10.log")));
            Assert.False(File.Exists(Path.Combine(tests, "openaow.11.log")));
            Assert.False(Directory.Exists(playtest));
            Assert.NotEqual(Path.GetFullPath(playtest), Path.GetFullPath(tests));
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Fact]
    public void EnvironmentVariablesOverrideLogDirectoryAndRetainedCount()
    {
        string root = TemporaryDirectory();
        string playtest = Path.Combine(root, "playtest");
        string tests = Path.Combine(root, "tests");
        string? previousDirectory = Environment.GetEnvironmentVariable(FileLoggerSettings.DirectoryVariable);
        string? previousCount = Environment.GetEnvironmentVariable(FileLoggerSettings.RetainedFileCountVariable);
        try
        {
            Environment.SetEnvironmentVariable(FileLoggerSettings.DirectoryVariable, value: tests);
            Environment.SetEnvironmentVariable(FileLoggerSettings.RetainedFileCountVariable, value: "3");
            FileLoggerSettings settings = FileLoggerSettings.FromEnvironment(playtest);
            Assert.Equal(Path.GetFullPath(tests), settings.Directory);
            Assert.Equal(3, settings.RetainedFileCount);
            _ = Directory.CreateDirectory(tests);
            File.WriteAllText(Path.Combine(tests, "openaow.4.log"), contents: "stale");
            for (int run = 0; run < 5; run++)
            {
                using FileLoggerProvider provider = new(settings);
                HostLog.Information(provider.CreateLogger("Test"), text: $"run {run}");
            }

            Assert.Equal(4, Directory.GetFiles(tests, searchPattern: "openaow*.log").Length);
            Assert.False(File.Exists(Path.Combine(tests, "openaow.4.log")));
            Assert.False(Directory.Exists(playtest));
        }
        finally
        {
            Environment.SetEnvironmentVariable(FileLoggerSettings.DirectoryVariable, value: previousDirectory);
            Environment.SetEnvironmentVariable(FileLoggerSettings.RetainedFileCountVariable, value: previousCount);
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Fact]
    public async Task IsolatedFactoriesFlowWithTheTestAndRestoreTheirPredecessor()
    {
        using CollectingLoggerFactory first = new();
        using CollectingLoggerFactory second = new();
        ILoggerFactory previous = RtlHost.LoggerFactory;
        using (RtlHost.IsolateLoggerFactory(first))
        {
            HostLog.Information(RtlHost.Logger, text: "first");
            await Task.Run(() => HostLog.Information(RtlHost.Logger, text: "first-async"));
            using (RtlHost.IsolateLoggerFactory(second))
            {
                HostLog.Warning(RtlHost.Logger, text: "second");
            }

            HostLog.Information(RtlHost.Logger, text: "first-after");
        }

        Assert.Same(previous, RtlHost.LoggerFactory);
        Assert.Equal(["first", "first-async", "first-after"], first.Messages);
        Assert.Equal(["second"], second.Messages);
    }

    [Fact]
    public void ToolRunIgnoresSetEnvironmentOverride()
    {
        string root = TemporaryDirectory();
        string playtest = Path.Combine(root, "playtest");
        string tests = Path.Combine(root, "tests");
        string? previousDirectory = Environment.GetEnvironmentVariable(FileLoggerSettings.DirectoryVariable);
        string? previousCount = Environment.GetEnvironmentVariable(FileLoggerSettings.RetainedFileCountVariable);
        try
        {
            Environment.SetEnvironmentVariable(FileLoggerSettings.DirectoryVariable, value: playtest);
            Environment.SetEnvironmentVariable(FileLoggerSettings.RetainedFileCountVariable, value: "1");
            using ILoggerFactory factory = HostLogging.Create(tests, honorEnvironmentOverrides: false);
            HostLog.Information(factory.CreateLogger("Test"), text: "tool");
            Assert.False(Directory.Exists(playtest));
            Assert.True(File.Exists(Path.Combine(tests, "openaow.log")));
        }
        finally
        {
            Environment.SetEnvironmentVariable(FileLoggerSettings.DirectoryVariable, value: previousDirectory);
            Environment.SetEnvironmentVariable(FileLoggerSettings.RetainedFileCountVariable, value: previousCount);
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Fact]
    public void InvalidRetainedCountKeepsTheDefaultAndLogsAnError()
    {
        string root = TemporaryDirectory();
        string? previousCount = Environment.GetEnvironmentVariable(FileLoggerSettings.RetainedFileCountVariable);
        try
        {
            Environment.SetEnvironmentVariable(FileLoggerSettings.RetainedFileCountVariable, value: "101");
            using ILoggerFactory factory = HostLogging.Create(root);
            HostLog.Information(factory.CreateLogger("Test"), text: "default");
            factory.Dispose();
            string text = File.ReadAllText(Path.Combine(root, "openaow.log"));
            Assert.Contains("OPENAOW_LOG_RETAINED_COUNT must be an integer from 0 to 100", text);
            Assert.Contains("default", text);
        }
        finally
        {
            Environment.SetEnvironmentVariable(FileLoggerSettings.RetainedFileCountVariable, value: previousCount);
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Fact]
    public void LockedCurrentLogUsesProcessFileAndDisposalDropsLateEntries()
    {
        string root = TemporaryDirectory();
        try
        {
            string current = Path.Combine(root, "openaow.log");
            _ = Directory.CreateDirectory(root);
            using FileStream held = new(current, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.Read);
            FileLoggerProvider provider = new(new(root, RetainedFileCount: 10));
            Assert.NotEqual(current, provider.FilePath);
            Assert.Equal(Path.Combine(root, $"openaow.{Environment.ProcessId}.log"), provider.FilePath);
            Assert.False(File.Exists(Path.Combine(root, "openaow.1.log")));
            ILogger logger = provider.CreateLogger("Test");
            HostLog.Information(logger, text: "process");
            provider.Dispose();
            Assert.Null(Record.Exception(() => logger.Log(LogLevel.Information, eventId: 0, state: "late", exception: null, formatter: (value, _) => value)));
            Assert.Contains("process", File.ReadAllText(provider.FilePath));
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Fact]
    public void InvalidDirectoryFallsBackToConsoleOnly()
    {
        string root = TemporaryDirectory();
        try
        {
            using ILoggerFactory factory = HostLogging.Create(Path.Combine(root, "invalid\0directory"));
            HostLog.Error(factory.CreateLogger("Test"), text: "console");
            Assert.False(Directory.Exists(root));
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Fact]
    public void LoggerForCachesOneLoggerPerFactoryAndCategory()
    {
        using CollectingLoggerFactory factory = new();
        using (RtlHost.IsolateLoggerFactory(factory))
        {
            Assert.Same(RtlHost.Logger, RtlHost.Logger);
            Assert.Same(RtlHost.LoggerFor("OpenAoW.Engine"), RtlHost.LoggerFor("OpenAoW.Engine"));
            Assert.NotSame(RtlHost.Logger, RtlHost.LoggerFor("OpenAoW.Engine"));
        }
    }

    private static string TemporaryDirectory() => Path.Combine(AppContext.BaseDirectory, "log-tests", Guid.NewGuid().ToString("N"));

    private sealed class CollectingLoggerFactory : ILoggerFactory
    {
        private readonly ConcurrentQueue<string> _messages = new();

        public string[] Messages => [.. _messages];

        public ILogger CreateLogger(string categoryName) => new Collector(_messages);

        public void AddProvider(ILoggerProvider provider) { }

        public void Dispose() { }

        private sealed class Collector(ConcurrentQueue<string> messages) : ILogger
        {
            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) => messages.Enqueue(formatter(state, exception));
        }
    }
}
