using Aow.Engine.Nodes;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class AltarOfFire : Altar
{
    public AltarOfFire(ECustomNode owner) : base(owner)
    {
    }

    // AltFire.pas:42
    public new const int ClassId = 131642;

    // AltFire.pas:37

    // AltFire.pas:45
    public override Storm CreateStorm() => new FireStorm(null) { Holder = CreationHolder() };

    public new partial class Resource : AltarResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
        }

        // AltFire.pas:58
        public new const int ClassId = 131643;

        // AltFire.pas:50

        // AltFire.pas:61
        public override int ResourceUserClassId() => 131642;

        // AltFire.pas:27

        // AltFire.pas:55
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_altarOfFireText);

        private static readonly string s_altarOfFireText = "Altar of Fire";

    }

    // AltFire.pas:19

}
