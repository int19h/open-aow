#nullable disable
// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Net;

// _tokenEvents (stored with id 5)
public partial class TokenPackage : DataPackage
{
    // NetworkE.pas:1100
    public new const int ClassId = ClassIdTokenPackage;

    // NetworkE.pas:1105
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Object("TokenEvents", objectValue: TokenEvents);
    }

    // NetworkE.pas:1088
    public TokenPackage(CustomNode owner) : base(owner) => TokenEvents = new TokenEventList(null) { Holder = CreationHolder() };

    // NetworkE.pas:1094
    public override void Dispose()
    {
        TokenEvents?.Dispose();
        base.Dispose();
    }

    public TokenEventList TokenEvents { get; }

    private const short ClassIdTokenPackage = 1360;

}
