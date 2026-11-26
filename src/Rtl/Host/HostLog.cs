using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace Aow.Rtl;

public static partial class HostLog
{
    [LoggerMessage(EventId = 35500, Level = LogLevel.Critical, Message = "Unhandled exception")]
    private static partial void UnhandledExceptionCore(ILogger logger, Exception error);

    public static void UnhandledException(ILogger logger, Exception error) => Safe(() => UnhandledExceptionCore(logger, error: error));

    [LoggerMessage(EventId = 35501, Level = LogLevel.Error, Message = "Unobserved task exception")]
    private static partial void UnobservedTaskExceptionCore(ILogger logger, Exception error);

    public static void UnobservedTaskException(ILogger logger, Exception error) => Safe(() => UnobservedTaskExceptionCore(logger, error: error));

    [LoggerMessage(EventId = 35502, Level = LogLevel.Debug, Message = "First-chance exception: {Detail}")]
    private static partial void FirstChanceExceptionCore(ILogger logger, string detail);

    public static void FirstChanceException(ILogger logger, string detail) => Safe(() => FirstChanceExceptionCore(logger, detail: detail));

    [LoggerMessage(EventId = 35503, Level = LogLevel.Warning, Message = "Fallback in {Location}")]
    private static partial void FallbackCore(ILogger logger, Exception error, string location);

    public static void Fallback(ILogger logger, Exception error, string location) => Safe(() => FallbackCore(logger, error: error, location: location));

    [LoggerMessage(EventId = 35504, Level = LogLevel.Debug, Message = "Fallback in {Location}")]
    private static partial void DebugFallbackCore(ILogger logger, Exception error, string location);

    public static void DebugFallback(ILogger logger, Exception error, string location) => Safe(() => DebugFallbackCore(logger, error: error, location: location));

    [LoggerMessage(EventId = 35505, Level = LogLevel.Warning, Message = "Catch dialog in {Location}: {Text}")]
    private static partial void CatchDialogCore(ILogger logger, string location, string text, Exception? error = null);

    public static void CatchDialog(ILogger logger, string location, string text, Exception? error = null) => Safe(() => CatchDialogCore(logger, location: location, text: text, error: error));

    [LoggerMessage(EventId = 35506, Level = LogLevel.Debug, Message = "Engine {Channel}: {Text}")]
    private static partial void DebugChannelCore(ILogger logger, string channel, string text);

    public static void DebugChannel(ILogger logger, string channel, string text) => Safe(() => DebugChannelCore(logger, channel: channel, text: text));

    [LoggerMessage(EventId = 35507, Level = LogLevel.Error, Message = "Host error in {Location}")]
    private static partial void HostErrorCore(ILogger logger, Exception error, string location);

    public static void HostError(ILogger logger, Exception error, string location) => Safe(() => HostErrorCore(logger, error: error, location: location));

    [LoggerMessage(EventId = 35508, Level = LogLevel.Error, Message = "{Text}")]
    private static partial void ErrorCore(ILogger logger, string text);

    public static void Error(ILogger logger, string text) => Safe(() => ErrorCore(logger, text: text));

    [LoggerMessage(EventId = 35509, Level = LogLevel.Warning, Message = "{Text}")]
    private static partial void WarningCore(ILogger logger, string text);

    public static void Warning(ILogger logger, string text) => Safe(() => WarningCore(logger, text: text));

    [LoggerMessage(EventId = 35510, Level = LogLevel.Information, Message = "{Text}")]
    private static partial void InformationCore(ILogger logger, string text);

    public static void Information(ILogger logger, string text) => Safe(() => InformationCore(logger, text: text));

    [LoggerMessage(EventId = 35511, Level = LogLevel.Warning, Message = "Fallback in {Location}")]
    private static partial void FallbackAtCallSiteCore(ILogger logger, Exception error, [CallerMemberName] string location = "");

    public static void FallbackAtCallSite(ILogger logger, Exception error, [CallerMemberName] string location = "") => Safe(() => FallbackAtCallSiteCore(logger, error: error, location: location));

    [LoggerMessage(EventId = 35512, Level = LogLevel.Warning, Message = "Catch dialog in {Location}: {Text}")]
    private static partial void CatchDialogAtCallSiteCore(ILogger logger, string text, [CallerMemberName] string location = "");

    public static void CatchDialogAtCallSite(ILogger logger, string text, [CallerMemberName] string location = "") => Safe(() => CatchDialogAtCallSiteCore(logger, text: text, location: location));

    [LoggerMessage(EventId = 35513, Level = LogLevel.Warning, Message = "{Text} in {Location}")]
    private static partial void FallbackWithTextCore(ILogger logger, Exception error, string text, string location);

    public static void FallbackWithText(ILogger logger, Exception error, string text, string location) => Safe(() => FallbackWithTextCore(logger, error: error, text: text, location: location));

    [LoggerMessage(EventId = 35514, Level = LogLevel.Warning, Message = "Load of {FileName} went on without {Count} objects that could not be read")]
    private static partial void DroppedObjectsCore(ILogger logger, string fileName, int count);

    /// <summary>A player-facing load went on without objects it could not read (issue 384).</summary>
    public static void DroppedObjects(ILogger logger, string fileName, int count) => Safe(() => DroppedObjectsCore(logger, fileName: fileName, count: count));

    [LoggerMessage(EventId = 35515, Level = LogLevel.Warning, Message = "Load of {FileName} could not read {ClassName} at {Path}: {Text}")]
    private static partial void DroppedObjectCore(ILogger logger, Exception error, string fileName, string className, string path, string text);

    /// <summary>One object a player-facing load went on without, with the exception that ended its read.</summary>
    public static void DroppedObject(ILogger logger, Exception error, string fileName, string className, string path, string text)
        => Safe(() => DroppedObjectCore(logger, error: error, fileName: fileName, className: className, path: path, text: text));

    [LoggerMessage(EventId = 35516, Level = LogLevel.Warning, Message = "{EventName} of {Owner} raises with {Count} handlers, at most one was expected (issue 326, decision 4)")]
    private static partial void SecondEventHandlerCore(ILogger logger, string eventName, object owner, int count);

    /// <summary>An event that had at most one subscriber in the original raises with several (issue 326, decision 4).</summary>
    public static void SecondEventHandler(ILogger logger, string eventName, object owner, int count)
        => Safe(() => SecondEventHandlerCore(logger, eventName: eventName, owner: owner, count: count));

    private static void Safe(Action log)
    {
        try
        {
            log();
        }
        catch
        {
        }
    }
}
