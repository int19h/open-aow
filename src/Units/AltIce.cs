// Translated from AltIce.pas.
using Aow.Rtl;
using Aow.Units.Altar;
using Aow.Units.Engine;
using Aow.Units.Storms;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.AltIce;

public partial class TAltarOfIce : TAltar
{
    public TAltarOfIce(TECustomNode owner) : base(owner)
    { }

    // AltIce.pas:36
    public new const int ClassID = 131659;

    // AltIce.pas:39
    public override TStorm CreateStorm() => new TIceStorm(null);

}

public partial class TAltarOfIceResource : TAltarResource
{
    public TAltarOfIceResource(TECustomNode owner) : base(owner)
    {
    }

    // AltIce.pas:52
    public new const int ClassID = 131660;

    // AltIce.pas:44

    // AltIce.pas:55
    public override int ResourceUserClassID() => 131659;

    // AltIce.pas:26

    // AltIce.pas:49
    public override string ResourceEditName() => TranslateRStr(AltarOfIceRStr);

}

public static partial class AltIceUnit
{
    // AltIce.pas:58
    public static void RegisterAltarOfIce(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TAltarOfIce>(), EClass.Of<TAltarOfIceResource>()]);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
