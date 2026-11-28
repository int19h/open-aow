using Aow.Game.Commands;

namespace Aow.Game.Net;

/// <summary>The executor executed a token event once (issue 343).</summary>
public sealed class TokenEventExecutedEventArgs : EventArgs
{
    public required TokenEvent TokenEvent { get; init; }

    public required ExecuteResult Result { get; init; }
}
