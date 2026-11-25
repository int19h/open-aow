namespace Aow.Headless;

/// <summary>
/// The load rule for every loader. A dropped child is a partial load. A clean stop between
/// maps keeps that outcome. A transfer fails when the load dropped a child. A finished turn
/// loop after a partial load still counts as a completed run.
/// </summary>
internal static class RunOutcome
{
    public const string Played = "played";

    public const string PartialLoad = "partial-load";

    public const string BetweenMaps = "between-maps";

    public static string Name(bool droppedChildren, bool betweenMaps) => droppedChildren
        ? PartialLoad
        : betweenMaps ? BetweenMaps : Played;

    public static bool Succeeds(bool complete, bool failed, bool transfer, bool droppedChildren)
        => complete && !failed && !(transfer && droppedChildren);
}
