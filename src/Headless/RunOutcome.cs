namespace Aow.Headless;

/// <summary>
/// The outcome a run record stores. A dropped child is a partial load, ahead of a stop
/// between maps and ahead of a finished turn loop. A stop between maps with a clean load
/// keeps that name, including a run that did not finish a turn loop. A finished turn loop
/// with a clean load is played. A run that stops before any of those records no outcome.
/// A transfer fails when the load dropped a child. A finished turn loop after a partial
/// load still counts as a completed run.
/// </summary>
internal static class RunOutcome
{
    public const string Played = "played";

    public const string PartialLoad = "partial-load";

    public const string BetweenMaps = "between-maps";

    public static string? Name(bool complete, bool droppedChildren, bool betweenMaps) => droppedChildren
        ? PartialLoad
        : betweenMaps ? BetweenMaps
        : complete ? Played : null;

    public static bool Succeeds(bool complete, bool failed, bool transfer, bool droppedChildren)
        => complete && !failed && !(transfer && droppedChildren);
}
