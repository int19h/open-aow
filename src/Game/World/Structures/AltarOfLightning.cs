using Aow.Engine.Nodes;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.World.Structures;

public partial class AltarOfLightning : Altar
{
    public AltarOfLightning(ECustomNode owner) : base(owner)
    { }

    // AltLight.pas:36
    public new const int ClassId = 131645;

    // AltLight.pas:39
    public override Storm CreateStorm() => new LightningStorm(null) { Holder = CreationHolder() };

    public new partial class Resource : AltarResource
    {
        public Resource(ECustomNode owner) : base(owner)
        {
        }

        // AltLight.pas:52
        public new const int ClassId = 131646;

        // AltLight.pas:44

        // AltLight.pas:55
        public override int ResourceUserClassId() => 131645;

        // AltLight.pas:26

        // AltLight.pas:49
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_altarOfLightningText);

        private static readonly string s_altarOfLightningText = "Altar of Lightning";

    }

}
