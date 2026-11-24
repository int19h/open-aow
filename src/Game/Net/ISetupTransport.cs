using Aow.Game.Session;

namespace Aow.Game.Net;

// The network transport of a SetupControl: the platform sends map images and setup data
// through it.
public interface ISetupTransport
{
    // Sends the map image to the passed recipient.
    void SendMap(ReadOnlyMemory<byte> image, PlayerId recipient);

    // Sends the setup data to the passed recipient and tells whether the send worked.
    bool SendData(ReadOnlyMemory<byte> data, PlayerId recipient);
}
