// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Net;

// _tokenEvents (stored with id 5)
public partial class TokenPackage : DataPackage
{
    // NetworkE.pas:1100
    public new const int ClassId = ClassIdTokenPackage;

    private TokenEventList _tokenEvents;

    // NetworkE.pas:1105
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwEObject(_tokenEvents, 5);
    }

    // NetworkE.pas:1088
    public TokenPackage(ECustomNode owner) : base(owner)
    {
        _tokenEvents = new TokenEventList(null);
    }

    // NetworkE.pas:1094
    public override void Dispose()
    {
        _tokenEvents?.Dispose();
        base.Dispose();
    }

    public TokenEventList TokenEvents { get => _tokenEvents; }

    private const short ClassIdTokenPackage = 1360;

}
