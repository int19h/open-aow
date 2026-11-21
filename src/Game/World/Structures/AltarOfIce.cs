#nullable disable warnings
using Aow.Engine.Nodes;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public class AltarOfIce(CustomNode owner) : Altar(owner)
{

    // AltIce.pas:36
    public new const int ClassId = 131_659;

    // AltIce.pas:39
    public override Storm CreateStorm() => new IceStorm(null) { Holder = CreationHolder() };

    public new class Resource(CustomNode owner) : AltarResource(owner)
    {

        // AltIce.pas:52
        public new const int ClassId = 131_660;

        // AltIce.pas:55
        public override int ResourceUserClassId() => AltarOfIce.ClassId;

        // AltIce.pas:49
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_altarOfIceText);

        private static readonly string s_altarOfIceText = "Altar of Ice";

    }
}
