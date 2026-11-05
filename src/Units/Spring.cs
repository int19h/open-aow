// Translated from Spring.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.Sound.Sound;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.Animation;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Spring;

public partial class TSpringOfLifeAnimation : TAnimation
{
    public TSpringOfLifeAnimation(TECustomNode owner) : base(owner)
    {}

    // Spring.pas:64
    public override void NewFrame()
    {
        base.NewFrame();
        if (FFrameCounter > 20)
        {
            Destroy();
        }
    }

    // Animation.pas:43


    // Spring.pas:59
    public override TSet<TAnimationControlModeElement> AnimationControlMode() => Spring.SpringMode;

}

public partial class TSpringOfLife : TStructure
{
    public TSpringOfLife(TECustomNode owner) : base(owner)
    {
    }

    // Spring.pas:73
    public new const int ClassID = 131328;

    // Spring.pas:70


    // Spring.pas:111
    public override void ArmyPlaced(TArmyHS PlacedArmy)
    {
        base.ArmyPlaced(PlacedArmy);
        if (!TSet<byte>.WithBounds(0, 255).With(3).With(5).Contains((byte)((TMapField)Owner).TerrainType))
        {
            ExecuteNaturesBlessing(PlacedArmy);
        }
    }

    // Spring.pas:76
    public void ShowBlessingAnimation(TAnimation Anim, int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
        Xhp -= XhpSurf;
        Yhp -= YhpSurf;
        FResource.ImageSequenceList.Show(20, Xhp, Yhp, Anim.FFrameCounter, SurfaceDesc);
    }

    // Spring.pas:86
    public void ExecuteNaturesBlessing(TArmyHS ArmyHS)
    {
        TSpringOfLifeAnimation Anim = default;
        TRect R = default;
        // { Keep the parameter in its original stack slot. }
        for (int I = 0, last = ArmyHS.FArmy.Count - 1; I <= last; I++)
        {
            ArmyHS.FArmy.GetUnit(I).ExpandAbility(99);
        }

        bool condition = ArmyHS.FArmy.FPlayer == AoWHSMap.FSeatedPlayer;
        if (!condition)
        {
            var lhx = GetLhx();
            var yhx = GetYhx();
            var xhx = GetXhx();
            condition = AoWHSMap.Watching(xhx, yhx, lhx, 0);
        }

        if (condition)
        {
            PlaySample("SFX\\ATTACK\\ENCHP02.WAV");
        }

        var lhx2 = GetLhx();
        var yhx2 = GetYhx();
        var xhx2 = GetXhx();
        if (AoWHSMap.Visible(xhx2, yhx2, lhx2, 0))
        {
            Anim = new TSpringOfLifeAnimation(null);
            R = Rect(-16, -8, 32, 40);
            Anim.SetDimensions(R);
            Anim.FAnimationLevel = 27;
            Anim.FOnShow = ShowBlessingAnimation;
            var lhx = GetLhx();
            var yhx = GetYhx();
            var xhx = GetXhx();
            AoWHSMap.PlaceHX(Anim, xhx, yhx, lhx);
        }
    }

    // Spring.pas:27


}

public partial class TSpringOfLifeResource : TStructureResource
{
    public TSpringOfLifeResource(TECustomNode owner) : base(owner)
    {
        FTerrainBound = false;
    }

    // Spring.pas:123
    public new const int ClassID = 131329;

    // Spring.pas:117


    // Spring.pas:129
    public override int ResourceUserClassID() => 131328;

    // Spring.pas:37


    // Spring.pas:126
    public override string ResourceEditName() => TranslateRStr(SpringOfLifeRStr);

}

public struct TSpringAnimationCallback
{
    public FixedArray_byte_36 Reserved;
    public TShowAnimationEvent OnShow;
}

public static partial class Spring
{
    // Spring.pas:132
    public static void RegisterSpringOfLife(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TSpringOfLife>(), EClass.Of<TSpringOfLifeResource>()]);
    }

    public static TSet<TAnimationControlModeElement> SpringMode = TSet<TAnimationControlModeElement>.FromMask(17UL);
    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
