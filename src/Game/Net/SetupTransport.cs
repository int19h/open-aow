namespace Aow.Game.Net;

// The network transport of a SetupControl: the platform sends map images and setup data
// through it. DirectPlay is not ported, so both calls fail until the platform implements them.
public sealed class SetupTransport : ISetupTransport
{
    private SetupTransport()
    {
    }

    // No platform implements the transport. A call fails at once, so a path that sends by
    // mistake stops instead of waiting for a send that never happens.
    public static SetupTransport Unported { get; } = new();

    public void SendMap(ReadOnlyMemory<byte> image, int recipient)
        /* TODO: Implement aowDPlayComp file transfer in the platform or editor service. */
        => throw new NotImplementedException();

    public bool SendData(ReadOnlyMemory<byte> data, int recipient)
        /* TODO: Implement aowDPlayComp.SendMessage in the platform or editor service. */
        => throw new NotImplementedException();
}
