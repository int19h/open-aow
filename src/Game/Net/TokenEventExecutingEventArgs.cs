using Aow.Game.Commands;

namespace Aow.Game.Net;

/// <summary>
/// The executor is about to execute a token event (issue 343). An event that returns
/// <see cref="ExecuteResult.Retry"/> executes again on a later frame.
/// </summary>
public sealed class TokenEventExecutingEventArgs : EventArgs
{
    public required TokenEvent TokenEvent { get; init; }
}
