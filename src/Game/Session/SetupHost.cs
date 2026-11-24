namespace Aow.Game.Session;

// The hosts a SetupControl runs against. SetupEvents implements the live one.
public sealed class SetupHost : ISetupHost
{
    private SetupHost()
    {
    }

    // A control with no host. A call fails at once, so a path that asks by mistake stops
    // instead of waiting for an answer that never arrives.
    public static SetupHost Unattended { get; } = new();

    public void LoadMap(string fileName) => throw new InvalidOperationException("No setup host is installed to load a map.");

    public void CustomizeLeader(CustomizeLeaderControl control) => throw new InvalidOperationException("No setup host is installed to customize a leader.");

    public void CloseIncompatibleSession(int majorVersion, int minorVersion) => throw new InvalidOperationException("No setup host is installed to close an incompatible session.");

    public void StartGame() => throw new InvalidOperationException("No setup host is installed to start the game.");

    public void Synchronize() => throw new InvalidOperationException("No setup host is installed to synchronize.");

    public void EjectPlayer(int remoteId) => throw new InvalidOperationException("No setup host is installed to eject a player.");
}
