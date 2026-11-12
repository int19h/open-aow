using Aow.Engine.Nodes;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class AltarOfDeath : Altar
{
    public AltarOfDeath(ECustomNode owner) : base(owner)
    { }

    // AltDeath.pas:36
    public new const int ClassId = 131637;

    // AltDeath.pas:39
    public override Storm CreateStorm() => new DeathStorm(null);

    public new partial class Resource : AltarResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
        }

        // AltDeath.pas:52
        public new const int ClassId = 131638;

        // AltDeath.pas:44

        // AltDeath.pas:55
        public override int ResourceUserClassId() => 131637;

        // AltDeath.pas:26

        // AltDeath.pas:49
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(s_altarOfDeathText);

        private static readonly string s_altarOfDeathText = "Altar of Death";

    }

}
