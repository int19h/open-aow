// Translated from AltIce.pas.
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class TAltarOfIce : TAltar
{
    public TAltarOfIce(TECustomNode owner) : base(owner)
    { }

    // AltIce.pas:36
    public new const int ClassID = 131659;

    // AltIce.pas:39
    public override TStorm CreateStorm() => new TIceStorm(null);

}

public partial class TAltarOfIceResource : TAltarResource
{
    public TAltarOfIceResource(TECustomNode owner) : base(owner)
    {
    }

    // AltIce.pas:52
    public new const int ClassID = 131660;

    // AltIce.pas:44

    // AltIce.pas:55
    public override int ResourceUserClassID() => 131659;

    // AltIce.pas:26

    // AltIce.pas:49
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TAltarOfIceResource.AltarOfIceRStr);

    private static readonly string AltarOfIceRStr = "Altar of Ice";

}
