using Aow.Engine.Nodes;
using Aow.Game.Draw.Animations;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class AltarOfIce : Altar
{
    public AltarOfIce(ECustomNode owner) : base(owner)
    { }

    // AltIce.pas:36
    public new const int ClassID = 131659;

    // AltIce.pas:39
    public override Storm CreateStorm() => new IceStorm(null);

    public new partial class Resource : AltarResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
        }

        // AltIce.pas:52
        public new const int ClassID = 131660;

        // AltIce.pas:44

        // AltIce.pas:55
        public override int ResourceUserClassID() => 131659;

        // AltIce.pas:26

        // AltIce.pas:49
        public override string ResourceEditName() => AbilityRules.TranslateRStr(AltarOfIceRStr);

        private static readonly string AltarOfIceRStr = "Altar of Ice";

    }

}
