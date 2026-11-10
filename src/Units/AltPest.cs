// Translated from AltPest.pas.
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class TAltarOfPestilence : TAltar
{
    public TAltarOfPestilence(TECustomNode owner) : base(owner)
    { }

    // AltPest.pas:36
    public new const int ClassID = 131852;

    // AltPest.pas:39
    public override TStorm CreateStorm() => new TPestilenceStorm(null);

}

public partial class TAltarOfPestilenceResource : TAltarResource
{
    public TAltarOfPestilenceResource(TECustomNode owner) : base(owner)
    {
    }

    // AltPest.pas:52
    public new const int ClassID = 131853;

    // AltPest.pas:44

    // AltPest.pas:55
    public override int ResourceUserClassID() => 131852;

    // AltPest.pas:26

    // AltPest.pas:49
    public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TAltarOfPestilenceResource.AltarOfPestilenceRStr);

    private static readonly string AltarOfPestilenceRStr = "Altar of Pestilence";

}
