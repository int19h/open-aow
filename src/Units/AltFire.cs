// Translated from AltFire.pas.
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class TAltarOfFire : TAltar
{
    public TAltarOfFire(TECustomNode owner) : base(owner)
    {
    }

    // AltFire.pas:42
    public new const int ClassID = 131642;

    // AltFire.pas:37

    // AltFire.pas:45
    public override TStorm CreateStorm() => new TFireStorm(null);

    // AltFire.pas:19

}

public partial class TAltarOfFireResource : TAltarResource
{
    public TAltarOfFireResource(TECustomNode owner) : base(owner)
    {
    }

    // AltFire.pas:58
    public new const int ClassID = 131643;

    // AltFire.pas:50

    // AltFire.pas:61
    public override int ResourceUserClassID() => 131642;

    // AltFire.pas:27

    // AltFire.pas:55
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TAltarOfFireResource.AltarOfFireRStr);

    private static readonly string AltarOfFireRStr = "Altar of Fire";

}
