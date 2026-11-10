// Translated from AltLife.pas.
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Structures;

public partial class AltarOfLife : Altar
{
    public AltarOfLife(ECustomNode owner) : base(owner)
    { }

    // AltLife.pas:36
    public new const int ClassID = 131635;

    // AltLife.pas:39
    public override Storm CreateStorm() => new DivineStorm(null);

    public new partial class Resource : AltarResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
        }

        // AltLife.pas:52
        public new const int ClassID = 131636;

        // AltLife.pas:44

        // AltLife.pas:55
        public override int ResourceUserClassID() => 131635;

        // AltLife.pas:26

        // AltLife.pas:49
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(AltarOfLifeRStr);

        private static readonly string AltarOfLifeRStr = "Altar of Life";

    }

}
