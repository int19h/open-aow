namespace Aow.Game.Recording;

/// <summary>
/// Who decided an input token event (recordings.md 4). A replay with the AI live injects the
/// <see cref="Human"/> events, replays the host actions behind the <see cref="Host"/> events and
/// lets the AI decide the others again.
/// </summary>
public enum TokenEventOrigin
{
    /// <summary>
    /// Neither the AI nor a host action: in the desktop build, the user interface. The headless
    /// host has no such add. Until a later step scopes the user interface, a reaction of the game
    /// outside every execution reads as human too.
    /// </summary>
    Human,

    /// <summary>The strategic AI executor, or the tactical battle AI on the seat whose turn it holds.</summary>
    AI,

    /// <summary>
    /// Added inside a host action (<see cref="TokenRecorder.HostInput"/>), the action's own command
    /// or the game's reaction to it. The action's marker line in <c>events.jsonl</c> names it.
    /// </summary>
    Host,
}
