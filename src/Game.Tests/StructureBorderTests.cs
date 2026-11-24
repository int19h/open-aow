#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.World.Structures;
using Aow.Graphics;
using LibraryImage = Aow.Game.Draw.Images.LibraryImage;

namespace Aow.Game.Tests;

// Issue #90: Structur.PAS ShowBorderImage tiles quotient + 1 middle images between the bottom and top images.
public sealed class StructureBorderTests
{
    private sealed class BorderStructure(LibraryImage top, LibraryImage middle, LibraryImage bottom) : Structure(null!)
    {
        public override LibraryImage GetTopBorderImage() => top;
        public override LibraryImage GetMiddleBorderImage() => middle;
        public override LibraryImage GetBottomBorderImage() => bottom;
    }

    private const int Width = 2;
    private const int TopColor = 0x0000FF;
    private const int MiddleColor = 0x00FF00;
    private const int BottomColor = 0xFF0000;

    private static LibraryImage Grab(ImageLibrary library, int height, int color)
    {
        Dib16Surface surface = new(Width, height, pixelFormatId: PixelFormat.Rgb565);
        GrabInfo grab = new() { FileName = "border.bmp", FindRectangles = false };
        try
        {
            Assert.Equal(GraphicsResult.Ok, surface.Clear(Color.FromPacked(color)));
            LockedSurface lockedSurface = default;
            Assert.Equal(GraphicsResult.Ok, surface.Lock(ref lockedSurface));
            Assert.Equal(GraphicsResult.Ok, LibraryImageClass.Of<Picture16>().GrabImages(lockedSurface, grabInfo: grab, imageLibrary: library, onProgress: null));
            return library[^1];
        }
        finally
        {
            _ = surface.Unlock();
        }
    }

    private static int Pixel(int color)
    {
        Dib16Surface surface = new(1, 1, pixelFormatId: PixelFormat.Rgb565);
        LockedSurface lockedSurface = default;
        _ = surface.Clear(Color.FromPacked(color));
        _ = surface.Lock(ref lockedSurface);
        int pixel = lockedSurface.View().UInt16(0);
        _ = surface.Unlock();
        return pixel;
    }

    [Fact]
    public void BorderTilesQuotientPlusOneMiddleImages()
    {
        // Top 1 row, middle tiles of 4 rows, bottom 5 rows, window 20 rows high. The bound is
        // (20 - 5) div 4 = 3, so the loop draws four middle tiles at rows 11, 7, 3 and -1. Only the
        // fourth tile paints rows 1 and 2, so three iterations leave them blank.
        const int height = 20;
        ImageLibrary library = new(null!);
        Dib16Surface target = new(Width, height, pixelFormatId: PixelFormat.Rgb565);
        List<LibraryImageClass> previousClasses = ImageLibraries.LibraryImageClasses;
        ImageLibraries.LibraryImageClasses = [];
        ImageLibraries.RegisterLibraryImageClasses([LibraryImageClass.Of<Picture16>()]);
        try
        {
            BorderStructure structure = new(Grab(library, height: 1, color: TopColor), middle: Grab(library, height: 4, color: MiddleColor), bottom: Grab(library, height: 5, color: BottomColor));
            LockedSurface lockedSurface = default;
            Assert.Equal(GraphicsResult.Ok, target.Clear(Color.Black));
            Assert.Equal(GraphicsResult.Ok, target.Lock(ref lockedSurface));
            Assert.True(structure.ShowBorderImage(lockedSurface, R: new Rect(0, 0, Width, height)));
            int[] rows = new int[height];
            for (int y = 0; y < height; y++)
            {
                int row = lockedSurface.View().UInt16(y * lockedSurface.Pitch);
                Assert.Equal(row, lockedSurface.View().UInt16((y * lockedSurface.Pitch) + 2));
                rows[y] = row;
            }
            Assert.Equal(1, rows.Count(row => row == Pixel(TopColor)));
            Assert.Equal(14, rows.Count(row => row == Pixel(MiddleColor)));
            Assert.Equal(5, rows.Count(row => row == Pixel(BottomColor)));
            int[] expected = [
                .. Enumerable.Repeat(Pixel(TopColor), count: 1),
                .. Enumerable.Repeat(Pixel(MiddleColor), count: 14),
                .. Enumerable.Repeat(Pixel(BottomColor), count: 5)];
            Assert.Equal(expected, rows);
        }
        finally
        {
            _ = target.Unlock();
            library.Dispose();
            ImageLibraries.LibraryImageClasses.Clear();
            ImageLibraries.LibraryImageClasses = previousClasses;
        }
    }
}
