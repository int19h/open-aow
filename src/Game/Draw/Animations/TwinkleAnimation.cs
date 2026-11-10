// Translated from Animation.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Animations;

// sequence, particle count, radius, lifetime, random start delay, mode
public partial class TwinkleAnimation : Animation
{
    public TwinkleAnimation(ECustomNode owner) : base(owner)
    {
        FAnimationLevel = 28;
    }

    public ImageSequence FImageSequence;
    public int FCount;
    public int FRadius;
    public int FDuration;
    public int FStartDelay;
    public Set<TwinkleModeElement> FMode;

    // Animation.pas:532

    // Animation.pas:538
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        ImageSequence Sequence;
        int OldSeed;
        int Delay;
        int X = default;
        int Y = default;
        int DX = default;
        int DY = default;
        int Radius;
        int Angle;
        int Index;
        OldSeed = RandSeed;
        HexGeometry.HXtoHP(FXhx, FYhx, ref X, ref Y);
        XhpSurf = X - XhpSurf + 16;
        YhpSurf = Y - YhpSurf + 16;
        var xhx = GetXhx();
        var yhx = GetYhx();
        RandSeed = xhx + AoWHSMap.Current.FInitialRandomSeed + yhx * 3;
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            Radius = Random(FRadius);
            Angle = Random(360);
            Delay = Random(FStartDelay);
            if (FFrameCounter - Delay >= 0)
            {
                if (FMode.Contains(TwinkleModeElement.Expand))
                {
                    Radius = Round((double)(Radius * FFrameCounter) / FDuration) + Radius / 2;
                }

                if (FMode.Contains(TwinkleModeElement.Explode))
                {
                    Radius += Round((FFrameCounter - Delay) * (FFrameCounter - Delay));
                }

                if (FMode.Contains(TwinkleModeElement.Implode))
                {
                    Radius += Round(FDuration - (FFrameCounter - Delay));
                }

                if (FMode.Contains(TwinkleModeElement.Rotate))
                {
                    Angle += FFrameCounter * 4;
                }

                AbilityRules.DegreesRadToCo(Angle, Radius, ref DX, ref DY);
                Sequence = FImageSequence;
                while (Sequence != null)
                {
                    Index = Sequence.GetUnloopedFrameIndex(FFrameCounter - Delay);
                    if (Sequence.ImageLibrary.ImageExist(Index))
                    {
                        Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift + DX, YhpSurf + Sequence.dYshift + DY, SurfaceDesc);
                    }

                    Sequence = Sequence.Next;
                }
            }
        }

        RandSeed = OldSeed;
        if (FFrameCounter >= FDuration)
        {
            Dispose();
        }
    }

    // Animation.pas:575
    public void Setup(ImageSequence Sequence, Set<TwinkleModeElement> Mode, int Count, int Radius, int Duration, int StartDelay)
    {
        FImageSequence = Sequence;
        FCount = Count;
        FRadius = Radius;
        FMode = Mode;
        FStartDelay = StartDelay;
        FDuration = Duration;
    }

    // Animation.pas:117

}
