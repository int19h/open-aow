#nullable disable
// Translated from Animation.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw.Images;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Draw.Animations;

// sequence, pixel offsets, last frame, loop flag
public sealed class ImageSequenceAnimation(CustomNode owner) : Animation(owner)
{

    public ImageSequence ImageSequence { get; private set; }
    public int PixelXOffset { get; private set; }
    public int PixelYOffset { get; private set; }
    public int FrameCount { get; set; } = -1;
    public bool Looped { get; set; }

    // Animation.pas:493
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        base.Show(pixelXSurface, pixelYSurface, lockedSurface);
        if (HexLevel == AnimationMap.ViewMapLevel)
        {
            HexGeometry.HexToPixel(Location.X, Location.Y, out int x, out int y);
            pixelXSurface = x - pixelXSurface + 16 + PixelXOffset;
            pixelYSurface = y - pixelYSurface + 16 + PixelYOffset;
            ImageSequence sequence;
            int index;
            if (!Looped)
            {
                sequence = ImageSequence;
                while (sequence is not null)
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
                while (sequence is not null)
                {
                    index = sequence.GetFrameIndex(0, offset: FrameCounter);
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
        Rect dimensions = Dimensions;
        dimensions.Left = 0;
        Dimensions = dimensions;
        Rect dimensions2 = Dimensions;
        dimensions2.Right = 0;
        Dimensions = dimensions2;
        Rect dimensions3 = Dimensions;
        dimensions3.Top = 0;
        Dimensions = dimensions3;
        Rect dimensions4 = Dimensions;
        dimensions4.Bottom = 0;
        Dimensions = dimensions4;
        ImageSequence sequence = ImageSequence;
        while (sequence is not null)
        {
            Rect r = sequence.GetDimensions();
            Rect input = Dimensions;
            _ = input.Union(right: r, result: out Rect dimensions5);
            Dimensions = dimensions5;
            sequence = sequence.Next;
        }

        Rect dimensions6 = Dimensions;
        dimensions6.Left += 16;
        Dimensions = dimensions6;
        Rect dimensions7 = Dimensions;
        dimensions7.Right += 16;
        Dimensions = dimensions7;
        Rect dimensions8 = Dimensions;
        dimensions8.Top += 16;
        Dimensions = dimensions8;
        Rect dimensions9 = Dimensions;
        dimensions9.Bottom += 16;
        Dimensions = dimensions9;
    }

    // Animation.pas:454
    public void SetImageSequence(ImageSequence sequence)
    {
        ImageSequence = sequence;
        UpdateDimensions();
        if (!Looped)
        {
            ImageSequence current = ImageSequence;
            FrameCount = -1;
            while (current is not null)
            {
                int frames = current.TotalSequenceFrames;
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
}
