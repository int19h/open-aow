using Aow.Engine.Nodes;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class AltarOfIce : Altar
{
    public AltarOfIce(ECustomNode owner) : base(owner)
    { }

    // AltIce.pas:36
    public new const int ClassId = 131659;

    // AltIce.pas:39
    public override Storm CreateStorm() => new IceStorm(null) { Holder = CreationHolder() };

    public new partial class Resource : AltarResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
        }

        // AltIce.pas:52
        public new const int ClassId = 131660;

        // AltIce.pas:44

        // AltIce.pas:55
        public override int ResourceUserClassId() => 131659;

        // AltIce.pas:26

        // AltIce.pas:49
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_altarOfIceText);

        private static readonly string s_altarOfIceText = "Altar of Ice";

    }

}
