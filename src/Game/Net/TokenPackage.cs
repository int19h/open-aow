// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Net;

// FTokenEvents (stored with id 5)
public partial class TokenPackage : DataPackage
{
    // NetworkE.pas:1100
    public new const int ClassID = CID_TokenPackage;

    internal TokenEventList FTokenEvents;

    // NetworkE.pas:1105
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FTokenEvents, 5);
    }

    // NetworkE.pas:1088
    public TokenPackage(ECustomNode owner) : base(owner)
    {
        FTokenEvents = new TokenEventList(null);
    }

    // NetworkE.pas:1094
    public override void Dispose()
    {
        FTokenEvents?.Dispose();
        base.Dispose();
    }

    public TokenEventList TokenEvents { get => FTokenEvents; }

    private const short CID_TokenPackage = 1360;

}
