using Aow.Units.AoWE;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Images = Aow.Units.ImageLib.ImageLib;
using LibraryImage = Aow.Units.ImageLib.TLibraryImage;
using G = Aow.Units.GFXE.GFXE;

namespace Aow.Units.Tests;

// Issue #90: Structur.PAS ShowBorderImage tiles quotient + 1 middle images between the bottom and top images.
public sealed class StructureBorderTests
{
    private sealed class BorderStructure(LibraryImage top, LibraryImage middle, LibraryImage bottom) : TStructure(null!)
    {
        public override LibraryImage GetTopBorderImage() => top;
        public override LibraryImage GetMiddleBorderImage() => middle;
        public override LibraryImage GetBottomBorderImage() => bottom;
    }

    private const int Width = 2;
    private const int TopColor = 0x0000FF;
    private const int MiddleColor = 0x00FF00;
    private const int BottomColor = 0xFF0000;

    private static LibraryImage Grab(TImageLibrary library, int height, int color)
    {
        var surface = new TDIB16Surface(Width, height, G.pfRGB565);
        var grab = new TGrabInfo { FileName = "border.bmp", FindRectangles = false };
        TSurfaceDesc sd = default;
        try
        {
            Assert.Equal(G.GFXE_OK, surface.Cls(color));
            Assert.Equal(G.GFXE_OK, surface.Lock(ref sd));
            Assert.Equal(0, LibraryImageClass.Of<TPicture16>().GrabImages(sd, grab, library, null!));
            return library[library.Count - 1];
        }
        finally
        {
            surface.Unlock();
            surface.Destroy();
            grab.Destroy();
        }
    }

    private static int Pixel(int color)
    {
        var surface = new TDIB16Surface(1, 1, G.pfRGB565);
        TSurfaceDesc sd = default;
        surface.Cls(color);
        surface.Lock(ref sd);
        int pixel = Runtime.BytePointer(sd.Data).Reinterpret<ushort>().Value;
        surface.Unlock();
        surface.Destroy();
        return pixel;
    }

    [Fact]
    public void BorderTilesQuotientPlusOneMiddleImages()
    {
        // Top 1 row, middle tiles of 4 rows, bottom 5 rows, window 20 rows high. The bound is
        // (20 - 5) div 4 = 3, so the loop draws four middle tiles at rows 11, 7, 3 and -1. Only the
        // fourth tile paints rows 1 and 2, so three iterations leave them blank.
        const int height = 20;
        var library = new TImageLibrary(null!);
        var target = new TDIB16Surface(Width, height, G.pfRGB565);
        var previousClasses = Images.LibraryImageClasses;
        Images.LibraryImageClasses = new TList();
        Images.RegisterLibraryImageClasses([LibraryImageClass.Of<TPicture16>()]);
        try
        {
            var top = Grab(library, 1, TopColor);
            var middle = Grab(library, 4, MiddleColor);
            var bottom = Grab(library, 5, BottomColor);
            var structure = new BorderStructure(top, middle, bottom);
            TSurfaceDesc sd = default;
            Assert.Equal(G.GFXE_OK, target.Cls(0));
            Assert.Equal(G.GFXE_OK, target.Lock(ref sd));
            Assert.True(structure.ShowBorderImage(sd, Runtime.Rect(0, 0, Width, height)));
            var data = Runtime.BytePointer(sd.Data);
            var rows = new int[height];
            for (int y = 0; y < height; y++)
            {
                int row = (data + y * sd.Pitch).Reinterpret<ushort>().Value;
                Assert.Equal(row, (data + y * sd.Pitch + 2).Reinterpret<ushort>().Value);
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
            target.Destroy();
            library.Destroy();
            Images.LibraryImageClasses.Destroy();
            Images.LibraryImageClasses = previousClasses;
        }
    }
}
