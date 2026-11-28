using Aow.Game.Commands;

namespace Aow.Game.Net;

/// <summary>An executing token event validated itself (issue 343).</summary>
public sealed class TokenEventValidatedEventArgs : EventArgs
{
    public required TokenEvent TokenEvent { get; init; }

    public required bool IsValid { get; init; }
}
