// Translated from AltLight.pas.
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Structures;
using static Aow.Game.Magic.Abilities.AoWEUnit;

namespace Aow.Game.World.Structures;

public partial class TAltarOfLightning : TAltar
{
    public TAltarOfLightning(TECustomNode owner) : base(owner)
    { }

    // AltLight.pas:36
    public new const int ClassID = 131645;

    // AltLight.pas:39
    public override TStorm CreateStorm() => new TLightningStorm(null);

}

public partial class TAltarOfLightningResource : TAltarResource
{
    public TAltarOfLightningResource(TECustomNode owner) : base(owner)
    {
    }

    // AltLight.pas:52
    public new const int ClassID = 131646;

    // AltLight.pas:44

    // AltLight.pas:55
    public override int ResourceUserClassID() => 131645;

    // AltLight.pas:26

    // AltLight.pas:49
    public override string ResourceEditName() => TranslateRStr(AltarOfLightningRStr);

}

public static partial class AltLightUnit
{
    // AltLight.pas:58
    public static void RegisterAltarOfLightning(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TAltarOfLightning>(), EClass.Of<TAltarOfLightningResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
