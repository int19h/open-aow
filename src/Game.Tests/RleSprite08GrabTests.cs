using Aow.Game.Draw;
using Aow.Game.Draw.Images;

namespace Aow.Game.Tests;

// TRLESprite08.GrabImage (ImageLib.pas 7659) on 8-bit surfaces: the smallest rectangle of opaque
// pixels, per row a length dword whose low word counts the row's bytes with the dword itself, the
// opaque indexes, and each transparent run as the transparent index and a length of at most 254.
// The expected bytes are worked by hand from the Pascal. ShowOpaque draws the grab back with
// ShowRLE08, which writes the opaque pixels and skips the runs.
public sealed class RleSprite08GrabTests
{
    private static (RleSprite08 Image, bool Grabbed) Grab(byte[,] pixels, Rect rectangle)
    {
        int w = pixels.GetLength(1), h = pixels.GetLength(0);
        var palette = new Palette { PixelFormatId = PixelFormats.pfBGR24 };
        var surface = new DataSurface(w, h, PixelFormats.pfPAL8, palette);
        var image = new RleSprite08(null!);
        var grab = image.CreateGrabInfo();
        try
        {
            LockedSurface lockedSurface = default;
            Assert.Equal(PixelFormats.GFXE_OK, surface.Lock(ref lockedSurface));
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++) lockedSurface.View()[y * lockedSurface.Pitch + x] = pixels[y, x];
            grab.TransparentPixel = 0;
            grab.FileName = "grab.bmp";
            bool grabbed = image.GrabImage(lockedSurface, rectangle, grab);
            surface.Unlock();
            return (image, grabbed);
        }
        finally
        {
            surface.Dispose();
        }
    }

    private static byte[] Encoded(RleSprite08 image) => image.Data.AsSpan(0, image._dataSize).ToArray();

    [Fact]
    public void GrabEncodesTheOpaqueRectangle()
    {
        byte[,] pixels =
        {
            { 0, 0, 0, 0, 0 },
            { 0, 7, 0, 0, 9 },
            { 0, 0, 0, 0, 0 },
        };
        var (image, grabbed) = Grab(pixels, new Rect(0, 0, 5, 3));
        try
        {
            Assert.True(grabbed);
            Assert.Equal((5, 3), (image._width, image._height));
            Assert.Equal((1, 1, 4, 1), (image._rleLeft, image._rleTop, image._rleWidth, image._rleHeight));
            // Row 1 from x = 1: 7, a run of two, 9. The length word counts 8 bytes.
            Assert.Equal(new byte[] { 8, 0, 0, 0, 7, 0, 2, 9 }, Encoded(image));
            Assert.Equal(new byte[,] { { 0x55, 0x55, 0x55, 0x55, 0x55 }, { 0x55, 7, 0x55, 0x55, 9 }, { 0x55, 0x55, 0x55, 0x55, 0x55 } }, Draw(image, 5, 3));
        }
        finally { image.Dispose(); }
    }

    [Fact]
    public void GrabSplitsLongRunsAt254()
    {
        var pixels = new byte[2, 300];
        pixels[0, 0] = 3;
        pixels[0, 299] = 4;
        pixels[1, 10] = 5;
        var (image, grabbed) = Grab(pixels, new Rect(0, 0, 300, 2));
        try
        {
            Assert.True(grabbed);
            Assert.Equal((0, 0, 300, 2), (image._rleLeft, image._rleTop, image._rleWidth, image._rleHeight));
            // Row 0: 3, a run of 298 as 254 and 44, 4. Row 1: a run of 10, 5, a run of 289 as 254
            // and 35, flushed by EndLine.
            Assert.Equal(new byte[] { 10, 0, 0, 0, 3, 0, 254, 0, 44, 4, 11, 0, 0, 0, 0, 10, 5, 0, 254, 0, 35 }, Encoded(image));
            var drawn = Draw(image, 300, 2);
            for (int y = 0; y < 2; y++)
                for (int x = 0; x < 300; x++) Assert.Equal(pixels[y, x] == 0 ? (byte)0x55 : pixels[y, x], drawn[y, x]);
        }
        finally { image.Dispose(); }
    }

    [Fact]
    public void GrabOfAnInsetRectangleKeepsItsOffset()
    {
        byte[,] pixels =
        {
            { 1, 1, 1, 1 },
            { 1, 0, 6, 1 },
            { 1, 6, 6, 1 },
            { 1, 1, 1, 1 },
        };
        var (image, grabbed) = Grab(pixels, new Rect(1, 1, 3, 3));
        try
        {
            Assert.True(grabbed);
            Assert.Equal((2, 2), (image._width, image._height));
            Assert.Equal((0, 0, 2, 2), (image._rleLeft, image._rleTop, image._rleWidth, image._rleHeight));
            Assert.Equal(new byte[] { 7, 0, 0, 0, 0, 1, 6, 6, 0, 0, 0, 6, 6 }, Encoded(image));
        }
        finally { image.Dispose(); }
    }

    [Fact]
    public void GrabOfAnotherFormatFails()
    {
        var surface = new DataSurface(2, 2, PixelFormats.pfRGB555, null!);
        var image = new RleSprite08(null!);
        var grab = image.CreateGrabInfo();
        try
        {
            LockedSurface lockedSurface = default;
            surface.Lock(ref lockedSurface);
            Assert.False(image.GrabImage(lockedSurface, new Rect(0, 0, 2, 2), grab));
            surface.Unlock();
        }
        finally
        {
            image.Dispose();
            surface.Dispose();
        }
    }

    // Draws the image at (0, 0) on an 8-bit surface filled with $55.
    private static byte[,] Draw(RleSprite08 image, int w, int h)
    {
        var palette = new Palette { PixelFormatId = PixelFormats.pfBGR24 };
        using var surface = new DataSurface(w, h, PixelFormats.pfPAL8, palette);
        LockedSurface lockedSurface = default;
        Assert.Equal(PixelFormats.GFXE_OK, surface.Lock(ref lockedSurface));
        for (int y = 0; y < h; y++) lockedSurface.View().Bytes(y * lockedSurface.Pitch, w).Fill(0x55);
        image.ShowOpaque(0, 0, lockedSurface);
        var result = new byte[h, w];
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++) result[y, x] = lockedSurface.View()[y * lockedSurface.Pitch + x];
        surface.Unlock();
        return result;
    }
}
