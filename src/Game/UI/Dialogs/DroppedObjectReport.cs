using System.Collections.Immutable;
using Aow.Formats.Tree;
using Aow.Rtl;
using Microsoft.Extensions.Logging;

namespace Aow.Game.UI.Dialogs;

/// <summary>
/// The objects one player-facing load could not read (issue 384, a port addition). The load
/// passes <see cref="Dropped"/> to the engine's load routines, which add each object the tree
/// loader goes on without and continue as the original does. <see cref="Report"/> then writes the
/// list to openaow.log and shows it once in the message dialog. A load that dropped nothing shows
/// and logs nothing. <see cref="Load"/> runs a load and its report as one step.
/// </summary>
/// <remarks>
/// The strings are the port's own. They go through the resource dictionary like the dialog's
/// other text, and the shipped dictionaries hold none of them, so every language shows the English.
/// </remarks>
public sealed class DroppedObjectReport(string fileName)
{
    // The dialog names this many objects and counts the rest.
    private const int ListedCount = 8;

    // A longer line of one object is cut to this many characters in the dialog. The log keeps all of it.
    private const int LineLength = 120;

    private bool _isLogged;

    public string FileName { get; } = fileName;

    /// <summary>The list the load routines fill.</summary>
    public List<DroppedObject> Dropped { get; } = [];

    /// <summary>Whether the player has been shown the list.</summary>
    public bool IsReported { get; private set; }

    internal static string DroppedObjectText { get; } = "The game loaded with 1 object that could not be read:";

    internal static string DroppedObjectsFormat { get; } = "The game loaded with %d objects that could not be read:";

    internal static string DroppedObjectFormat { get; } = "%s at %s: %s";

    internal static string MoreDroppedObjectsFormat { get; } = "... and %d more.";

    internal static string FullListText { get; } = "The full list is in openaow.log.";

    /// <summary>
    /// Runs one player-facing load with a new report and reports once after it. Every engine
    /// load inside <paramref name="load"/> fills the same list, so a campaign and its map give
    /// one message.
    /// </summary>
    public static DroppedObjectReport Load(UIContext ui, string fileName, Action<List<DroppedObject>> load)
    {
        DroppedObjectReport report = new(fileName);
        load(report.Dropped);
        _ = report.Report(ui);
        return report;
    }

    /// <summary>
    /// Logs every dropped object once and shows the list in the message dialog once. A load that
    /// dropped nothing does nothing. A context without the message dialog logs that the player
    /// was not shown the list, and a later call can still show it. Returns whether this call
    /// showed the list.
    /// </summary>
    public bool Report(UIContext ui)
    {
        if (IsReported || Dropped.Count == 0)
        {
            return false;
        }

        ILogger logger = RtlHost.LoggerFor("OpenAoW.Load");
        if (!_isLogged)
        {
            _isLogged = true;
            HostLog.DroppedObjects(logger, fileName: FileName, count: Dropped.Count);
            foreach (DroppedObject dropped in Dropped)
            {
                HostLog.DroppedObject(logger, error: dropped.Error, fileName: FileName, className: dropped.ClassName, path: dropped.Path, text: dropped.Message);
            }
        }

        if (ui.MessageDialog is not MessageDialog dialog)
        {
            HostLog.Warning(logger, text: $"Load of {FileName}: the message dialog is not loaded, so the player was not shown the {Dropped.Count} objects that could not be read");
            return false;
        }

        Show(dialog, lines: Lines(ui.Engine));
        IsReported = true;
        return true;
    }

    /// <summary>The lines of the message: the count, up to eight objects, the rest counted, and where the full list is.</summary>
    internal ImmutableArray<string> Lines(GameEngine engine)
    {
        ImmutableArray<string>.Builder lines = ImmutableArray.CreateBuilder<string>();
        lines.Add(Dropped.Count == 1
            ? engine.TranslateResourceString(DroppedObjectText)
            : DelphiFormat.Format(engine.TranslateResourceString(DroppedObjectsFormat), args: [Dropped.Count]));
        string objectFormat = engine.TranslateResourceString(DroppedObjectFormat);
        foreach (DroppedObject dropped in Dropped.Take(ListedCount))
        {
            lines.Add(Cp1252.Normalize(Short(DelphiFormat.Format(objectFormat, args: [dropped.ClassName, dropped.Path, FirstLine(dropped.Message)]))));
        }

        if (Dropped.Count > ListedCount)
        {
            lines.Add(DelphiFormat.Format(engine.TranslateResourceString(MoreDroppedObjectsFormat), args: [Dropped.Count - ListedCount]));
        }

        lines.Add(engine.TranslateResourceString(FullListText));
        return lines.ToImmutable();
    }

    // The OK message the way ShowDemoMessage shows one: no event, so OK only hides the dialog.
    private static void Show(MessageDialog dialog, ImmutableArray<string> lines)
    {
        MessageDialog.ResetMessageDialog(dialog);
        foreach (string line in lines)
        {
            _ = dialog.MessageMemo.Items.Add(line);
        }

        dialog.OKPanel.Visible = true;
        dialog.Window.Visible = true;
    }

    private static string FirstLine(string message)
    {
        int end = message.IndexOfAny(['\r', '\n']);
        return end < 0 ? message : message[..end];
    }

    private static string Short(string line) => line.Length <= LineLength ? line : $"{line[..LineLength]}...";
}
