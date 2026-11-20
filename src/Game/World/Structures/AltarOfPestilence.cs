#nullable disable
using Aow.Engine.Nodes;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public class AltarOfPestilence(CustomNode owner) : Altar(owner)
{

    // AltPest.pas:36
    public new const int ClassId = 131_852;

    // AltPest.pas:39
    public override Storm CreateStorm() => new PestilenceStorm(null) { Holder = CreationHolder() };

    public new class Resource(CustomNode owner) : AltarResource(owner)
    {

        // AltPest.pas:52
        public new const int ClassId = 131_853;

        // AltPest.pas:55
        public override int ResourceUserClassId() => AltarOfPestilence.ClassId;

        // AltPest.pas:49
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_altarOfPestilenceText);

        private static readonly string s_altarOfPestilenceText = "Altar of Pestilence";

    }
}
