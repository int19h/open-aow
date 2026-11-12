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
        AnimationLevel = 28;
    }

    public ImageSequence ImageSequence;
    public int Count;
    public int Radius;
    public int Duration;
    public int StartDelay;
    public Set<TwinkleModeElement> Mode;

    // Animation.pas:532

    // Animation.pas:538
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int oldSeed;
        int delay;
        int X = default;
        int Y = default;
        int deltaX = default;
        int deltaY = default;
        int radius;
        int angle;
        int index;
        oldSeed = RandSeed;
        HexGeometry.HexToPixel(_hexX, _hexY, ref X, ref Y);
        pixelXSurface = X - pixelXSurface + 16;
        pixelYSurface = Y - pixelYSurface + 16;
        var hexX = GetHexX();
        var hexY = GetHexY();
        RandSeed = hexX + AoWHSMap.Current.InitialRandomSeed + hexY * 3;
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            radius = Random(Radius);
            angle = Random(360);
            delay = Random(StartDelay);
            if (FrameCounter - delay >= 0)
            {
                if (Mode.Contains(TwinkleModeElement.Expand))
                {
                    radius = Round((double)(radius * FrameCounter) / Duration) + radius / 2;
                }

                if (Mode.Contains(TwinkleModeElement.Explode))
                {
                    radius += Round((FrameCounter - delay) * (FrameCounter - delay));
                }

                if (Mode.Contains(TwinkleModeElement.Implode))
                {
                    radius += Round(Duration - (FrameCounter - delay));
                }

                if (Mode.Contains(TwinkleModeElement.Rotate))
                {
                    angle += FrameCounter * 4;
                }

                AbilityRules.PolarToCartesian(angle, radius, ref deltaX, ref deltaY);
                sequence = ImageSequence;
                while (sequence != null)
                {
                    index = sequence.GetUnloopedFrameIndex(FrameCounter - delay);
                    if (sequence.ImageLibrary.ImageExist(index))
                    {
                        sequence.ImageLibrary[index].Show(pixelXSurface + sequence.OffsetX + deltaX, pixelYSurface + sequence.OffsetY + deltaY, lockedSurface);
                    }

                    sequence = sequence.Next;
                }
            }
        }

        RandSeed = oldSeed;
        if (FrameCounter >= Duration)
        {
            Dispose();
        }
    }

    // Animation.pas:575
    public void Setup(ImageSequence sequence, Set<TwinkleModeElement> mode, int count, int radius, int duration, int startDelay)
    {
        ImageSequence = sequence;
        Count = count;
        Radius = radius;
        Mode = mode;
        StartDelay = startDelay;
        Duration = duration;
    }

    // Animation.pas:117

}
