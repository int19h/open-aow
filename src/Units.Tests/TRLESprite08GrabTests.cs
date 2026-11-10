using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using G = Aow.Game.Draw.GFXEUnit;

namespace Aow.Units.Tests;

// TRLESprite08.GrabImage (ImageLib.pas 7659) on 8-bit surfaces: the smallest rectangle of opaque
// pixels, per row a length dword whose low word counts the row's bytes with the dword itself, the
// opaque indexes, and each transparent run as the transparent index and a length of at most 254.
// The expected bytes are worked by hand from the Pascal. ShowOpaque draws the grab back with
// ShowRLE08, which writes the opaque pixels and skips the runs.
public sealed class TRLESprite08GrabTests
{
    private static (TRLESprite08 Image, bool Grabbed) Grab(byte[,] pixels, TRect rect)
    {
        int w = pixels.GetLength(1), h = pixels.GetLength(0);
        var palette = new TGFXEPalette { PixelFormatID = G.pfBGR24 };
        var surface = new TDataSurface(w, h, G.pfPAL8, palette);
        var image = new TRLESprite08(null!);
        var grab = image.CreateGrabInfo();
        try
        {
            TSurfaceDesc sd = default;
            Assert.Equal(G.GFXE_OK, surface.Lock(ref sd));
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++) sd.View()[y * sd.Pitch + x] = pixels[y, x];
            grab.TransparentPixel = 0;
            grab.FileName = "grab.bmp";
            bool grabbed = image.GrabImage(sd, rect, grab);
            surface.Unlock();
            return (image, grabbed);
        }
        finally
        {
            surface.Dispose();
        }
    }

    private static byte[] Encoded(TRLESprite08 image) => image.FData.AsSpan(0, image.FDataSize).ToArray();

    [Fact]
    public void GrabEncodesTheOpaqueRectangle()
    {
        byte[,] pixels =
        {
            { 0, 0, 0, 0, 0 },
            { 0, 7, 0, 0, 9 },
            { 0, 0, 0, 0, 0 },
        };
        var (image, grabbed) = Grab(pixels, RtlUnit.Rect(0, 0, 5, 3));
        try
        {
            Assert.True(grabbed);
            Assert.Equal((5, 3), (image.FWidth, image.FHeight));
            Assert.Equal((1, 1, 4, 1), (image.FRLELeft, image.FRLETop, image.FRLEWidth, image.FRLEHeight));
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
        var (image, grabbed) = Grab(pixels, RtlUnit.Rect(0, 0, 300, 2));
        try
        {
            Assert.True(grabbed);
            Assert.Equal((0, 0, 300, 2), (image.FRLELeft, image.FRLETop, image.FRLEWidth, image.FRLEHeight));
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
        var (image, grabbed) = Grab(pixels, RtlUnit.Rect(1, 1, 3, 3));
        try
        {
            Assert.True(grabbed);
            Assert.Equal((2, 2), (image.FWidth, image.FHeight));
            Assert.Equal((0, 0, 2, 2), (image.FRLELeft, image.FRLETop, image.FRLEWidth, image.FRLEHeight));
            Assert.Equal(new byte[] { 7, 0, 0, 0, 0, 1, 6, 6, 0, 0, 0, 6, 6 }, Encoded(image));
        }
        finally { image.Dispose(); }
    }

    [Fact]
    public void GrabOfAnotherFormatFails()
    {
        var surface = new TDataSurface(2, 2, G.pfRGB555, null!);
        var image = new TRLESprite08(null!);
        var grab = image.CreateGrabInfo();
        try
        {
            TSurfaceDesc sd = default;
            surface.Lock(ref sd);
            Assert.False(image.GrabImage(sd, RtlUnit.Rect(0, 0, 2, 2), grab));
            surface.Unlock();
        }
        finally
        {
            image.Dispose();
            surface.Dispose();
        }
    }

    // Draws the image at (0, 0) on an 8-bit surface filled with $55.
    private static byte[,] Draw(TRLESprite08 image, int w, int h)
    {
        var palette = new TGFXEPalette { PixelFormatID = G.pfBGR24 };
        using var surface = new TDataSurface(w, h, G.pfPAL8, palette);
        TSurfaceDesc sd = default;
        Assert.Equal(G.GFXE_OK, surface.Lock(ref sd));
        for (int y = 0; y < h; y++) sd.View().Bytes(y * sd.Pitch, w).Fill(0x55);
        image.ShowOpaque(0, 0, sd);
        var result = new byte[h, w];
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++) result[y, x] = sd.View()[y * sd.Pitch + x];
        surface.Unlock();
        return result;
    }
}
