#nullable disable
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
    public ImageSequenceAnimation(CustomNode owner) : base(owner)
    {
        FrameCount = -1;
        PixelXOffset = 0;
        PixelYOffset = 0;
    }

    public ImageSequence ImageSequence { get; private set; }
    public int PixelXOffset { get; private set; }
    public int PixelYOffset { get; private set; }
    public int FrameCount { get; set; }
    public bool Looped { get; set; }

    // Animation.pas:424

    // Animation.pas:493
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int x = default;
        int y = default;
        int index;
        base.Show(pixelXSurface, pixelYSurface, lockedSurface);
        if (GetHexLevel() == AnimationMap.ViewMapLevel)
        {
            HexGeometry.HexToPixel(StoredHexX, StoredHexY, out x, out y);
            pixelXSurface = x - pixelXSurface + 16 + PixelXOffset;
            pixelYSurface = y - pixelYSurface + 16 + PixelYOffset;
            if (!Looped)
            {
                sequence = ImageSequence;
                while (sequence != null)
                {
                    index = sequence.GetUnloopedFrameIndex(FrameCounter);
                    if (index != -1)
                    {
                        if (sequence.ImageLibrary.ImageExist(index))
                        {
                            sequence.ImageLibrary[index].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface);
                        }
                    }

                    sequence = sequence.Next;
                }
            }
            else
            {
                sequence = ImageSequence;
                while (sequence != null)
                {
                    index = sequence.GetFrameIndex(0, FrameCounter);
                    if (index != -1)
                    {
                        sequence.ImageLibrary[index].Show(pixelXSurface + sequence.OffsetX, pixelYSurface + sequence.OffsetY, lockedSurface);
                    }

                    sequence = sequence.Next;
                }
            }
        }
    }

    // Animation.pas:487
    public override void NewFrame()
    {
        base.NewFrame();
        if (FrameCounter == FrameCount)
        {
            Dispose();
        }
    }

    // Animation.pas:432
    public void UpdateDimensions()
    {
        ImageSequence sequence;
        Rect r = default;
        var dimensions = Dimensions;
        dimensions.Left = 0;
        Dimensions = dimensions;
        var dimensions2 = Dimensions;
        dimensions2.Right = 0;
        Dimensions = dimensions2;
        var dimensions3 = Dimensions;
        dimensions3.Top = 0;
        Dimensions = dimensions3;
        var dimensions4 = Dimensions;
        dimensions4.Bottom = 0;
        Dimensions = dimensions4;
        sequence = ImageSequence;
        while (sequence != null)
        {
            sequence.GetDimensions(out r);
            var dimensions5 = Dimensions;
            UnionRect(out dimensions5, dimensions5, r);
            Dimensions = dimensions5;
            sequence = sequence.Next;
        }

        var dimensions6 = Dimensions;
        dimensions6.Left += 16;
        Dimensions = dimensions6;
        var dimensions7 = Dimensions;
        dimensions7.Right += 16;
        Dimensions = dimensions7;
        var dimensions8 = Dimensions;
        dimensions8.Top += 16;
        Dimensions = dimensions8;
        var dimensions9 = Dimensions;
        dimensions9.Bottom += 16;
        Dimensions = dimensions9;
    }

    // Animation.pas:454
    public void SetImageSequence(ImageSequence sequence)
    {
        ImageSequence current;
        int frames;
        ImageSequence = sequence;
        UpdateDimensions();
        if (!Looped)
        {
            current = ImageSequence;
            FrameCount = -1;
            while (current != null)
            {
                frames = current.GetTotalSequenceFrames();
                if (frames > FrameCount)
                {
                    FrameCount = frames;
                }

                current = current.Next;
            }

            if (FrameCount != -1)
            {
                FrameCount += 1;
            }
        }
    }

    // Animation.pas:475
    public void SetPixelXOffset(int value)
    {
        PixelXOffset = value;
        UpdateDimensions();
    }

    // Animation.pas:481
    public void SetPixelYOffset(int value)
    {
        PixelYOffset = value;
        UpdateDimensions();
    }

    // Animation.pas:98

}
