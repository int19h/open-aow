#nullable disable
using Aow.Engine.Nodes;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public class AltarOfFire(CustomNode owner) : Altar(owner)
{

    // AltFire.pas:42
    public new const int ClassId = 131_642;

    // AltFire.pas:45
    public override Storm CreateStorm() => new FireStorm(null) { Holder = CreationHolder() };

    public new class Resource(CustomNode owner) : AltarResource(owner)
    {

        // AltFire.pas:58
        public new const int ClassId = 131_643;

        // AltFire.pas:61
        public override int ResourceUserClassId() => AltarOfFire.ClassId;

        // AltFire.pas:55
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_altarOfFireText);

        private static readonly string s_altarOfFireText = "Altar of Fire";

    }
}
