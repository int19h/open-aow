// Translated from Animation.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Animations;

// sequence, pixel offsets, last frame, loop flag
public partial class ImageSequenceAnimation : Animation
{
    public ImageSequenceAnimation(ECustomNode owner) : base(owner)
    {
        FFrameCount = -1;
        FXhpOffset = 0;
        FYhpOffset = 0;
    }

    public ImageSequence FImageSequence;
    public int FXhpOffset;
    public int FYhpOffset;
    public int FFrameCount;
    public bool FLooped;

    // Animation.pas:424

    // Animation.pas:493
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        ImageSequence Sequence;
        int X = default;
        int Y = default;
        int Index;
        base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        if (GetLhx() == FAnimationMap.ViewL)
        {
            HexGeometry.HXtoHP(FXhx, FYhx, ref X, ref Y);
            XhpSurf = X - XhpSurf + 16 + FXhpOffset;
            YhpSurf = Y - YhpSurf + 16 + FYhpOffset;
            if (!FLooped)
            {
                Sequence = FImageSequence;
                while (Sequence != null)
                {
                    Index = Sequence.GetUnloopedFrameIndex(FFrameCounter);
                    if (Index != -1)
                    {
                        if (Sequence.ImageLibrary.ImageExist(Index))
                        {
                            Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                        }
                    }

                    Sequence = Sequence.Next;
                }
            }
            else
            {
                Sequence = FImageSequence;
                while (Sequence != null)
                {
                    Index = Sequence.GetFrameIndex(0, FFrameCounter);
                    if (Index != -1)
                    {
                        Sequence.ImageLibrary[Index].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                    }

                    Sequence = Sequence.Next;
                }
            }
        }
    }

    // Animation.pas:487
    public override void NewFrame()
    {
        base.NewFrame();
        if (FFrameCounter == FFrameCount)
        {
            Dispose();
        }
    }

    // Animation.pas:432
    public void UpdateDimensions()
    {
        ImageSequence Sequence;
        Rect R = default;
        FDimensions.Left = 0;
        FDimensions.Right = 0;
        FDimensions.Top = 0;
        FDimensions.Bottom = 0;
        Sequence = FImageSequence;
        while (Sequence != null)
        {
            Sequence.GetDimensions(ref R);
            UnionRect(ref FDimensions, FDimensions, R);
            Sequence = Sequence.Next;
        }

        FDimensions.Left += 16;
        FDimensions.Right += 16;
        FDimensions.Top += 16;
        FDimensions.Bottom += 16;
    }

    // Animation.pas:454
    public void SetImageSequence(ImageSequence Sequence)
    {
        ImageSequence Current;
        int Frames;
        FImageSequence = Sequence;
        UpdateDimensions();
        if (!FLooped)
        {
            Current = FImageSequence;
            FFrameCount = -1;
            while (Current != null)
            {
                Frames = Current.GetTotalSequenceFrames();
                if (Frames > FFrameCount)
                {
                    FFrameCount = Frames;
                }

                Current = Current.Next;
            }

            if (FFrameCount != -1)
            {
                FFrameCount += 1;
            }
        }
    }

    // Animation.pas:475
    public void SetXhpOffset(int Value)
    {
        FXhpOffset = Value;
        UpdateDimensions();
    }

    // Animation.pas:481
    public void SetYhpOffset(int Value)
    {
        FYhpOffset = Value;
        UpdateDimensions();
    }

    // Animation.pas:98

}
