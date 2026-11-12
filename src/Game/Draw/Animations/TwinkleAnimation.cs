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

    private ImageSequence _imageSequence;
    private int _count;
    private int _radius;
    private int _duration;
    private int _startDelay;
    private Set<TwinkleModeElement> _mode;

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
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            radius = Random(_radius);
            angle = Random(360);
            delay = Random(_startDelay);
            if (FrameCounter - delay >= 0)
            {
                if (_mode.Contains(TwinkleModeElement.Expand))
                {
                    radius = Round((double)(radius * FrameCounter) / _duration) + radius / 2;
                }

                if (_mode.Contains(TwinkleModeElement.Explode))
                {
                    radius += Round((FrameCounter - delay) * (FrameCounter - delay));
                }

                if (_mode.Contains(TwinkleModeElement.Implode))
                {
                    radius += Round(_duration - (FrameCounter - delay));
                }

                if (_mode.Contains(TwinkleModeElement.Rotate))
                {
                    angle += FrameCounter * 4;
                }

                AbilityRules.PolarToCartesian(angle, radius, ref deltaX, ref deltaY);
                sequence = _imageSequence;
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
        if (FrameCounter >= _duration)
        {
            Dispose();
        }
    }

    // Animation.pas:575
    public void Setup(ImageSequence sequence, Set<TwinkleModeElement> mode, int count, int radius, int duration, int startDelay)
    {
        _imageSequence = sequence;
        _count = count;
        _radius = radius;
        _mode = mode;
        _startDelay = startDelay;
        _duration = duration;
    }

    // Animation.pas:117

}
