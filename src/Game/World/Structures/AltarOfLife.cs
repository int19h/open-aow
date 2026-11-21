#nullable disable warnings
using Aow.Engine.Nodes;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public class AltarOfLife(CustomNode owner) : Altar(owner)
{

    // AltLife.pas:36
    public new const int ClassId = 131_635;

    // AltLife.pas:39
    public override Storm CreateStorm() => new DivineStorm(null) { Holder = CreationHolder() };

    public new class Resource(CustomNode owner) : AltarResource(owner)
    {

        // AltLife.pas:52
        public new const int ClassId = 131_636;

        // AltLife.pas:55
        public override int ResourceUserClassId() => AltarOfLife.ClassId;

        // AltLife.pas:49
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_altarOfLifeText);

        private static readonly string s_altarOfLifeText = "Altar of Life";

    }
}
