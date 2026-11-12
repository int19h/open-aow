using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.World.Structures;
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
        var surface = new Dib16Surface(Width, height, PixelFormats.pfRGB565);
        var grab = new GrabInfo { FileName = "border.bmp", FindRectangles = false };
        LockedSurface lockedSurface = default;
        try
        {
            Assert.Equal(PixelFormats.GFXE_OK, surface.Clear(color));
            Assert.Equal(PixelFormats.GFXE_OK, surface.Lock(ref lockedSurface));
            Assert.Equal(0, LibraryImageClass.Of<Picture16>().GrabImages(lockedSurface, grab, library, null!));
            return library[library.Count - 1];
        }
        finally
        {
            surface.Unlock();
            surface.Dispose();
        }
    }

    private static int Pixel(int color)
    {
        var surface = new Dib16Surface(1, 1, PixelFormats.pfRGB565);
        LockedSurface lockedSurface = default;
        surface.Clear(color);
        surface.Lock(ref lockedSurface);
        int pixel = lockedSurface.View().UInt16(0);
        surface.Unlock();
        surface.Dispose();
        return pixel;
    }

    [Fact]
    public void BorderTilesQuotientPlusOneMiddleImages()
    {
        // Top 1 row, middle tiles of 4 rows, bottom 5 rows, window 20 rows high. The bound is
        // (20 - 5) div 4 = 3, so the loop draws four middle tiles at rows 11, 7, 3 and -1. Only the
        // fourth tile paints rows 1 and 2, so three iterations leave them blank.
        const int Height = 20;
        var library = new ImageLibrary(null!);
        var target = new Dib16Surface(Width, Height, PixelFormats.pfRGB565);
        var previousClasses = ImageLibraries.LibraryImageClasses;
        ImageLibraries.LibraryImageClasses = new List<LibraryImageClass>();
        ImageLibraries.RegisterLibraryImageClasses([LibraryImageClass.Of<Picture16>()]);
        try
        {
            var top = Grab(library, 1, TopColor);
            var middle = Grab(library, 4, MiddleColor);
            var bottom = Grab(library, 5, BottomColor);
            var structure = new BorderStructure(top, middle, bottom);
            LockedSurface lockedSurface = default;
            Assert.Equal(PixelFormats.GFXE_OK, target.Clear(0));
            Assert.Equal(PixelFormats.GFXE_OK, target.Lock(ref lockedSurface));
            Assert.True(structure.ShowBorderImage(lockedSurface, new Rect(0, 0, Width, Height)));
            var rows = new int[Height];
            for (int y = 0; y < Height; y++)
            {
                int row = lockedSurface.View().UInt16(y * lockedSurface.Pitch);
                Assert.Equal(row, lockedSurface.View().UInt16(y * lockedSurface.Pitch + 2));
                rows[y] = row;
            }
            Assert.Equal(1, rows.Count(row => row == Pixel(TopColor)));
            Assert.Equal(14, rows.Count(row => row == Pixel(MiddleColor)));
            Assert.Equal(5, rows.Count(row => row == Pixel(BottomColor)));
            int[] expected = [
                .. Enumerable.Repeat(Pixel(TopColor), 1),
                .. Enumerable.Repeat(Pixel(MiddleColor), 14),
                .. Enumerable.Repeat(Pixel(BottomColor), 5)];
            Assert.Equal(expected, rows);
        }
        finally
        {
            target.Unlock();
            target.Dispose();
            library.Dispose();
            ImageLibraries.LibraryImageClasses.Clear();
            ImageLibraries.LibraryImageClasses = previousClasses;
        }
    }
}
