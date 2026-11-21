#nullable disable warnings
using Aow.Engine.Nodes;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public class AltarOfDeath(CustomNode owner) : Altar(owner)
{

    // AltDeath.pas:36
    public new const int ClassId = 131_637;

    // AltDeath.pas:39
    public override Storm CreateStorm() => new DeathStorm(null) { Holder = CreationHolder() };

    public new class Resource(CustomNode owner) : AltarResource(owner)
    {

        // AltDeath.pas:52
        public new const int ClassId = 131_638;

        // AltDeath.pas:55
        public override int ResourceUserClassId() => AltarOfDeath.ClassId;

        // AltDeath.pas:49
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_altarOfDeathText);

        private static readonly string s_altarOfDeathText = "Altar of Death";

    }
}
