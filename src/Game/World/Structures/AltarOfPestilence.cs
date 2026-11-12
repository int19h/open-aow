using Aow.Engine.Nodes;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class AltarOfPestilence : Altar
{
    public AltarOfPestilence(ECustomNode owner) : base(owner)
    { }

    // AltPest.pas:36
    public new const int ClassId = 131852;

    // AltPest.pas:39
    public override Storm CreateStorm() => new PestilenceStorm(null) { Holder = CreationHolder() };

    public new partial class Resource : AltarResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
        }

        // AltPest.pas:52
        public new const int ClassId = 131853;

        // AltPest.pas:44

        // AltPest.pas:55
        public override int ResourceUserClassId() => 131852;

        // AltPest.pas:26

        // AltPest.pas:49
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_altarOfPestilenceText);

        private static readonly string s_altarOfPestilenceText = "Altar of Pestilence";

    }

}
