using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Aow.Rtl.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using static Aow.Rtl.LegacyApplication;

namespace Aow.Rtl;

public static class RtlHost
{
    private static readonly AsyncLocal<ILoggerFactory?> s_loggerFactory = new();
    private static readonly ConditionalWeakTable<ILoggerFactory, ConcurrentDictionary<string, ILogger>> s_loggers = [];

    public static IFileSystem Files { get; set; } = new PhysicalFileSystem();
    public static IClock Clock { get; set; } = new SystemClock();

    public static ISettingsStore? Settings { get; set; }
    public static IDialogService? Dialogs { get; set; }
    public static IShellService? Shell { get; set; }

    public static ILoggerFactory GlobalLoggerFactory { get; set; } = NullLoggerFactory.Instance;

    public static ILoggerFactory LoggerFactory
    {
        get => s_loggerFactory.Value ?? GlobalLoggerFactory;
        set => GlobalLoggerFactory = value ?? NullLoggerFactory.Instance;
    }

    public static ILogger Logger => LoggerFor("OpenAoW");

    public static ILogger LoggerFor(string category)
    {
        ILoggerFactory factory = LoggerFactory;
        return s_loggers.GetOrCreateValue(factory).GetOrAdd(category, static (name, current) => current.CreateLogger(name), factoryArgument: factory);
    }

    public static IDisposable IsolateLoggerFactory(ILoggerFactory factory)
    {
        ILoggerFactory? previous = s_loggerFactory.Value;
        s_loggerFactory.Value = factory;
        return new LoggerFactoryScope(previous);
    }

    /// <summary>The installed host's choice of written extensions (IRtlHost.WritesOwnExtensions).</summary>
    public static bool WritesOwnExtensions { get; set; } = true;

    public static void InstallHost(IRtlHost host)
    {
        Files = host.Files;
        Clock = host.Clock;
        Settings = host.Settings;
        Dialogs = host.Dialogs;
        WritesOwnExtensions = host.WritesOwnExtensions;
        Shell = host.Shell;
        LoggerFactory = host.LoggerFactory;
        Application.ExeName = host.ExecutableName;
        MessageHandler = message => host.Dialogs.Show(new(message, Caption: "", Flags: 0, DefaultResult: 1));
    }

    public static int MessageBox(int handle, string text, string caption, int flags)
    {
        int[] buttons = (flags & 15) switch
        {
            1 => [1, 2],
            2 => [3, 4, 5],
            3 => [6, 7, 2],
            4 => [6, 7],
            5 => [4, 2],
            _ => [1]
        };
        int selected = Math.Min((flags >> 8) & 3, buttons.Length - 1);
        return (Dialogs ?? throw new InvalidOperationException(text)).Show(new(text, caption, flags, DefaultResult: buttons[selected]));
    }

    private sealed class LoggerFactoryScope(ILoggerFactory? previous) : IDisposable
    {
        public void Dispose() => s_loggerFactory.Value = previous;
    }
}
