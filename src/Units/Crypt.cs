// Translated from Crypt.pas.
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class TCrypt : TItemExplorationSite
{
    public TCrypt(TECustomNode owner) : base(owner)
    { }

    // Crypt.pas:34
    public new const int ClassID = 131850;

}

public partial class TCryptResource : TItemExplorationSiteResource
{
    public TCryptResource(TECustomNode owner) : base(owner)
    { }

    // Crypt.pas:40
    public new const int ClassID = 131851;

    // Crypt.pas:43
    public override int ResourceUserClassID() => 131850;

    // Crypt.pas:37
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TCryptResource.CryptRStr);

    private static readonly string CryptRStr = "Crypt";

}
