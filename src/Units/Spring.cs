// Translated from Spring.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Animation
{

    public partial class TSpringOfLifeAnimation : TAnimation
    {
        public TSpringOfLifeAnimation(TECustomNode owner) : base(owner)
        { }

        // Spring.pas:64
        public override void NewFrame()
        {
            base.NewFrame();
            if (FFrameCounter > 20)
            {
                Dispose();
            }
        }

        // Animation.pas:43

        // Spring.pas:59
        public override TSet<TAnimationControlModeElement> AnimationControlMode() => Aow.Game.Draw.Animation.TSpringOfLifeAnimation.SpringMode;

        private static TSet<global::Aow.Game.Draw.Animation.TAnimationControlModeElement> SpringMode = TSet<global::Aow.Game.Draw.Animation.TAnimationControlModeElement>.FromMask(17uL);

    }
}

namespace Aow.Game.World.Structures
{

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
            Aow.Game.World.Map.HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
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

            bool condition = ArmyHS.FArmy.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer;
            if (!condition)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                condition = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Watching(xhx, yhx, lhx, 0);
            }

            if (condition)
            {
                Aow.Game.Audio.AudioPlayback.PlaySample("SFX\\ATTACK\\ENCHP02.WAV");
            }

            var lhx2 = GetLhx();
            var yhx2 = GetYhx();
            var xhx2 = GetXhx();
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Visible(xhx2, yhx2, lhx2, 0))
            {
                Anim = new TSpringOfLifeAnimation(null);
                R = Rect(-16, -8, 32, 40);
                Anim.SetDimensions(R);
                Anim.FAnimationLevel = 27;
                Anim.FOnShow = ShowBlessingAnimation;
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.PlaceHX(Anim, xhx, yhx, lhx);
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
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Structures.TSpringOfLifeResource.SpringOfLifeRStr);

        private static readonly string SpringOfLifeRStr = "Spring of Life";

    }
}

namespace Aow.Game.Draw.Animation
{

    public struct TSpringAnimationCallback
    {
        public FixedArray_byte_36 Reserved;
        public TShowAnimationEvent OnShow;
    }
}
