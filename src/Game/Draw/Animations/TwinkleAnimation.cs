#nullable disable
// Translated from Animation.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Draw.Animations;

// sequence, particle count, radius, lifetime, random start delay, mode
public sealed class TwinkleAnimation : Animation
{
    public TwinkleAnimation(CustomNode owner) : base(owner) => AnimationLevel = 28;

    private ImageSequence _imageSequence;
    private int _count;
    private int _radius;
    private int _duration;
    private int _startDelay;
    private Set<TwinkleModeElement> _mode;

    // Animation.pas:538
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        int oldSeed = RandSeed;
        HexGeometry.HexToPixel(Location.X, Location.Y, out int x, out int y);
        pixelXSurface = x - pixelXSurface + 16;
        pixelYSurface = y - pixelYSurface + 16;
        sbyte hexX = HexX;
        sbyte hexY = HexY;
        RandSeed = hexX + StrategicMap.Current.InitialRandomSeed + (hexY * 3);
        for (int i = 0, last = _count - 1; i <= last; i++)
        {
            int radius = Random(_radius);
            int angle = Random(360);
            int delay = Random(_startDelay);
            if (FrameCounter - delay >= 0)
            {
                if (_mode.Contains(TwinkleModeElement.Expand))
                {
                    radius = Round((double)(radius * FrameCounter) / _duration) + (radius / 2);
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

                AbilityRules.PolarToCartesian(angle, radius, out int deltaX, out int deltaY);
                ImageSequence sequence = _imageSequence;
                while (sequence is not null)
                {
                    int index = sequence.GetUnloopedFrameIndex(FrameCounter - delay);
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
}
