// Translated from AltDeath.pas.
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class AltarOfDeath : Altar
{
    public AltarOfDeath(ECustomNode owner) : base(owner)
    { }

    // AltDeath.pas:36
    public new const int ClassID = 131637;

    // AltDeath.pas:39
    public override Storm CreateStorm() => new DeathStorm(null);

    public new partial class Resource : AltarResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
        }

        // AltDeath.pas:52
        public new const int ClassID = 131638;

        // AltDeath.pas:44

        // AltDeath.pas:55
        public override int ResourceUserClassID() => 131637;

        // AltDeath.pas:26

        // AltDeath.pas:49
        public override string ResourceEditName() => Magic.Abilities.AbilityRules.TranslateRStr(AltarOfDeathRStr);

        private static readonly string AltarOfDeathRStr = "Altar of Death";

    }

}
