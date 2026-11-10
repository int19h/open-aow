// Translated from AltPest.pas.
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class AltarOfPestilence : Altar
{
    public AltarOfPestilence(ECustomNode owner) : base(owner)
    { }

    // AltPest.pas:36
    public new const int ClassID = 131852;

    // AltPest.pas:39
    public override Storm CreateStorm() => new PestilenceStorm(null);

    public new partial class Resource : AltarResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
        }

        // AltPest.pas:52
        public new const int ClassID = 131853;

        // AltPest.pas:44

        // AltPest.pas:55
        public override int ResourceUserClassID() => 131852;

        // AltPest.pas:26

        // AltPest.pas:49
        public override string ResourceEditName() => Magic.Abilities.AbilityRules.TranslateRStr(AltarOfPestilenceRStr);

        private static readonly string AltarOfPestilenceRStr = "Altar of Pestilence";

    }

}
