namespace Aow.Game.Session;

// The one I/O host of a SetupControl: the screens and the session control it asks to act.
// The network transport lives on ISetupTransport.
public interface ISetupHost
{
    // Loads the named map file and reports it loaded when done.
    void LoadMap(string fileName);

    // Opens the leader customization for the passed control.
    void CustomizeLeader(CustomizeLeaderControl control);

    // Closes the session with a host whose version does not fit.
    void CloseIncompatibleSession(int majorVersion, int minorVersion);

    // Starts the game once the setup map is ready.
    void StartGame();

    // Shows the synchronization screen.
    void Synchronize();

    // Ejects the player with the passed remote id from the network session.
    void EjectPlayer(int remoteId);
}
