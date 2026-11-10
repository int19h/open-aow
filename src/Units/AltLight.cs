// Translated from AltLight.pas.
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class TAltarOfLightning : TAltar
{
    public TAltarOfLightning(TECustomNode owner) : base(owner)
    { }

    // AltLight.pas:36
    public new const int ClassID = 131645;

    // AltLight.pas:39
    public override TStorm CreateStorm() => new TLightningStorm(null);

}

public partial class TAltarOfLightningResource : TAltarResource
{
    public TAltarOfLightningResource(TECustomNode owner) : base(owner)
    {
    }

    // AltLight.pas:52
    public new const int ClassID = 131646;

    // AltLight.pas:44

    // AltLight.pas:55
    public override int ResourceUserClassID() => 131645;

    // AltLight.pas:26

    // AltLight.pas:49
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TAltarOfLightningResource.AltarOfLightningRStr);

    private static readonly string AltarOfLightningRStr = "Altar of Lightning";

}
