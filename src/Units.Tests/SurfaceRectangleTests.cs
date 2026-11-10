using Aow.Game.Draw;

namespace Aow.Units.Tests;

// The optional rectangles of the surface methods (graphics.md 3.3). The original passes a PRect
// that is nil for the whole surface, the port a TRect? that is null. Each case follows the Pascal
// of GFXE.pas 12007 to 12225: a 4 by 3 source of distinct 555 pixels is copied into a 6 by 5
// destination of zeros, and the expected pixels come from the clipping rules of those methods.
public sealed class SurfaceRectangleTests
{
    private const int SourceWidth = 4, SourceHeight = 3, DestWidth = 6, DestHeight = 5;

    private static int Source(int x, int y) => 0x100 * y + x + 1;

    private static DataSurface MakeSource()
    {
        var surface = new DataSurface(SourceWidth, SourceHeight, Aow.Game.Draw.PixelFormats.pfRGB555, null!);
        SurfaceDesc sd = default;
        Assert.Equal(Aow.Game.Draw.PixelFormats.GFXE_OK, surface.Lock(ref sd));
        for (int y = 0; y < SourceHeight; y++)
            for (int x = 0; x < SourceWidth; x++) sd.View().Word(y * sd.Pitch + x * 2, Source(x, y));
        surface.Unlock();
        return surface;
    }

    // The destination after a copy of the source rectangle (left, top, right, bottom) to (x, y),
    // with the pixels equal to key left out.
    private static int[,] Expected(int left, int top, int right, int bottom, int x, int y, int key = -1)
    {
        var result = new int[DestWidth, DestHeight];
        for (int sy = top; sy < bottom; sy++)
            for (int sx = left; sx < right; sx++)
                if (Source(sx, sy) != key) result[x + sx - left, y + sy - top] = Source(sx, sy);
        return result;
    }

    private static void Check(int[,] expected, Surface dest)
    {
        for (int y = 0; y < DestHeight; y++)
            for (int x = 0; x < DestWidth; x++)
                Assert.True(expected[x, y] == dest.GetPixelInfo(x, y), $"pixel {x}, {y}: expected {expected[x, y]:X}, actual {dest.GetPixelInfo(x, y):X}");
    }

    private static void Run(Func<Surface, Surface, int> copy, int[,] expected, int result = Aow.Game.Draw.PixelFormats.GFXE_OK)
    {
        var source = MakeSource();
        var dest = new DataSurface(DestWidth, DestHeight, Aow.Game.Draw.PixelFormats.pfRGB555, null!);
        try
        {
            Assert.Equal(result, copy(source, dest));
            Check(expected, dest);
        }
        finally
        {
            source.Dispose();
            dest.Dispose();
        }
    }

    [Fact]
    public void BltCopiesTheWholeSourceForANullRectangle()
        => Run((s, d) => s.Blt(d, null, 1, 1), Expected(0, 0, 4, 3, 1, 1));

    [Fact]
    public void BltCopiesTheGivenRectangle()
        => Run((s, d) => s.Blt(d, RtlUnit.Rect(1, 1, 3, 3), 0, 0), Expected(1, 1, 3, 3, 0, 0));

    [Fact]
    public void BltClippedMovesTheWholeSourceInsideTheDestination()
        // R = (0, 0, 4, 3) at (-1, -1): X < 0 cuts the left column, Y < 0 the top row.
        => Run((s, d) => s.BltClipped(d, null, -1, -1), Expected(1, 1, 4, 3, 0, 0));

    [Fact]
    public void BltClippedCutsARectangleToTheSource()
        // (-2, -1, 8, 5) at (0, 0): the right and bottom are cut to the source, the negative left
        // and top move X and Y by 2 and 1.
        => Run((s, d) => s.BltClipped(d, RtlUnit.Rect(-2, -1, 8, 5), 0, 0), Expected(0, 0, 4, 3, 2, 1));

    [Fact]
    public void BltClippedCutsAtTheDestinationEdge()
        // (0, 0, 4, 3) at (4, 3): W = 4 - (4 + 4 - 6) = 2, H = 3 - (3 + 3 - 5) = 2.
        => Run((s, d) => s.BltClipped(d, null, 4, 3), Expected(0, 0, 2, 2, 4, 3));

    [Fact]
    public void BltClippedDrawsNothingOutside()
        => Run((s, d) => s.BltClipped(d, null, 6, 0), new int[DestWidth, DestHeight]);

    [Fact]
    public void BltColorKeySkipsTheKeyForANullRectangle()
        => Run((s, d) => s.BltColorKey(d, null, 0, 0, Source(1, 1)), Expected(0, 0, 4, 3, 0, 0, Source(1, 1)));

    [Fact]
    public void BltColorKeySkipsTheKeyInTheGivenRectangle()
        => Run((s, d) => s.BltColorKey(d, RtlUnit.Rect(0, 1, 3, 3), 2, 2, Source(2, 2)), Expected(0, 1, 3, 3, 2, 2, Source(2, 2)));

    [Fact]
    public void BltColorKeyClippedClipsLikeBltClipped()
        => Run((s, d) => s.BltColorKeyClipped(d, null, -1, -1, Source(2, 1)), Expected(1, 1, 4, 3, 0, 0, Source(2, 1)));

    [Fact]
    public void BltColorKeyClippedCutsARectangleToTheSource()
        => Run((s, d) => s.BltColorKeyClipped(d, RtlUnit.Rect(-2, -1, 8, 5), 0, 0, Source(0, 0)), Expected(0, 0, 4, 3, 2, 1, Source(0, 0)));

    [Fact]
    public void BltStretchOfUnequalSizesIsNotSupported()
        // Null rectangles are the whole source (4 by 3) and the whole destination (6 by 5).
        => Run((s, d) => s.BltStretch(d, null, null), new int[DestWidth, DestHeight], Aow.Game.Draw.PixelFormats.GFXE_NOTSUPPORTED);

    [Fact]
    public void BltStretchOfEqualSizesCopiesToTheDestinationRectangle()
        => Run((s, d) => s.BltStretch(d, RtlUnit.Rect(1, 0, 3, 2), RtlUnit.Rect(3, 2, 5, 4)), Expected(1, 0, 3, 2, 3, 2));

    [Fact]
    public void BltStretchOfANullSourceRectangleTakesTheWholeSource()
        => Run((s, d) => s.BltStretch(d, null, RtlUnit.Rect(2, 1, 6, 4)), Expected(0, 0, 4, 3, 2, 1));

    [Fact]
    public void BltAreaClippedPassesItsArea()
        => Run((s, d) => s.BltAreaClipped(d, RtlUnit.Rect(1, 0, 4, 2), 3, 3), Expected(1, 0, 4, 2, 3, 3));

    [Fact]
    public void BltAreaColorKeyClippedPassesItsArea()
        => Run((s, d) => s.BltAreaColorKeyClipped(d, RtlUnit.Rect(1, 0, 4, 2), 3, 3, Source(2, 1)), Expected(1, 0, 4, 2, 3, 3, Source(2, 1)));

    [Fact]
    public void CapturedSurfaceTakesTheWholeSourceOrTheRectangle()
    {
        var source = MakeSource();
        var whole = new DataSurface(source, null);
        var part = new DataSurface(source, RtlUnit.Rect(1, 1, 3, 3));
        try
        {
            Assert.Equal((SourceWidth, SourceHeight), (whole.Width, whole.Height));
            for (int y = 0; y < SourceHeight; y++)
                for (int x = 0; x < SourceWidth; x++) Assert.Equal(Source(x, y), whole.GetPixelInfo(x, y));
            Assert.Equal((2, 2, 1, 1), (part.Width, part.Height, part.FX, part.FY));
            for (int y = 0; y < 2; y++)
                for (int x = 0; x < 2; x++) Assert.Equal(Source(x + 1, y + 1), part.GetPixelInfo(x, y));
        }
        finally
        {
            whole.Dispose();
            part.Dispose();
            source.Dispose();
        }
    }

    // LockEx and CreateSDfromSD move the first pixel by index arithmetic on DataIndex, as the
    // original moves the Data pointer.
    [Fact]
    public void SubDescriptorsMoveTheFirstPixel()
    {
        var source = MakeSource();
        try
        {
            SurfaceDesc full = default, part = default, sub = default;
            Assert.Equal(Aow.Game.Draw.PixelFormats.GFXE_OK, source.Lock(ref full));
            source.Unlock();
            Assert.Equal(0, full.DataIndex);
            Assert.Equal(Aow.Game.Draw.PixelFormats.GFXE_OK, source.LockEx(ref part, RtlUnit.Rect(1, 1, 4, 3)));
            source.UnlockEx(part);
            Assert.Same(full.Data, part.Data);
            Assert.Equal(full.Pitch + 2, part.DataIndex);
            Assert.Equal((3, 2), (part.Width, part.Height));
            Assert.Equal(Source(1, 1), part.View().Word(0));
            Assert.Equal(Source(3, 2), part.View().Word(part.Pitch + 4));
            Aow.Game.Draw.SurfaceDesc.CreateSDfromSD(full, RtlUnit.Rect(2, 1, 4, 3), ref sub);
            Assert.Same(full.Data, sub.Data);
            Assert.Equal(full.Pitch + 4, sub.DataIndex);
            Assert.Equal(RtlUnit.Rect(0, 0, 2, 2), sub.ClipRect);
            Assert.Equal(Source(2, 1), sub.View().Word(0));
            Assert.Equal(Aow.Game.Draw.PixelFormats.GFXE_ERROR, source.LockEx(ref part, RtlUnit.Rect(1, 1, 5, 3)));
        }
        finally { source.Dispose(); }
    }

    // GetPrimarySurfaceRgn returns @FPrimarySurfaceRgn in the original: never nil, the rectangle
    // SetPrimarySurfaceRgn stored.
    [Fact]
    public void ControllersReturnThePrimaryRegion()
    {
        using var owner = new global::Aow.Rtl.Component(null!);
        var dc = new DisplayCtrl(owner);
        var sdl = new SdlDC(new NullDisplayHost(), owner);
        var region = RtlUnit.Rect(3, 4, 643, 484);
        dc.SetPrimarySurfaceRgn(region);
        sdl.SetPrimarySurfaceRgn(region);
        Assert.Equal(region, dc.GetPrimarySurfaceRgn());
        Assert.Equal(region, sdl.GetPrimarySurfaceRgn());
        Assert.Equal(region, dc.FPrimarySurfaceRgn);
    }

    private sealed class NullDisplayHost : Aow.Game.Host.IDisplayHost
    {
        public (int Width, int Height) ClientSize => (640, 480);
        public bool Fullscreen { get; set; }
        public bool HasFocus => true;

        public void Present(in SurfaceDesc surface) { }

        public (int X, int Y, bool Inside) PointerPosition() => (0, 0, true);

        public void PumpEvents(Aow.Game.Host.IDisplayEvents sink) { }

        public void ShowSystemCursor(bool show) { }
    }
}
