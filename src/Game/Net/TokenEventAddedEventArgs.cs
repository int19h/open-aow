using Aow.Game.Commands;
using Aow.Game.Session;

namespace Aow.Game.Net;

/// <summary>A token event entered a queue of the executor's token manager (issue 343).</summary>
public sealed class TokenEventAddedEventArgs : EventArgs
{
    public required TokenEvent TokenEvent { get; init; }

    public required TokenEventQueue Queue { get; init; }

    /// <summary>The player of the token control for <see cref="TokenEventQueue.Player"/>, null otherwise.</summary>
    public required PlayerId? Player { get; init; }
}
