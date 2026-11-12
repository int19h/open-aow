using Aow.Game.Draw;

namespace Aow.Game.Tests;

// The optional rectangles of the surface methods (graphics.md 3.3). The original passes a PRect
// that is nil for the whole surface, the port a TRect? that is null. Each case follows the Pascal
// of GFXE.pas 12007 to 12225: a 4 by 3 source of distinct 555 pixels is copied into a 6 by 5
// destination of zeros, and the expected pixels come from the clipping rules of those methods.
public sealed class SurfaceRectangleTests
{
    private const int SourceWidth = 4, SourceHeight = 3, DestinationWidth = 6, DestinationHeight = 5;

    private static int Source(int x, int y) => 0x100 * y + x + 1;

    private static DataSurface MakeSource()
    {
        var surface = new DataSurface(SourceWidth, SourceHeight, PixelFormats.pfRGB555, null!);
        LockedSurface lockedSurface = default;
        Assert.Equal(PixelFormats.GFXE_OK, surface.Lock(ref lockedSurface));
        for (int y = 0; y < SourceHeight; y++)
            for (int x = 0; x < SourceWidth; x++) lockedSurface.View().UInt16(y * lockedSurface.Pitch + x * 2, Source(x, y));
        surface.Unlock();
        return surface;
    }

    // The destination after a copy of the source rectangle (left, top, right, bottom) to (x, y),
    // with the pixels equal to key left out.
    private static int[,] Expected(int left, int top, int right, int bottom, int x, int y, int key = -1)
    {
        var result = new int[DestinationWidth, DestinationHeight];
        for (int sy = top; sy < bottom; sy++)
            for (int sx = left; sx < right; sx++)
                if (Source(sx, sy) != key) result[x + sx - left, y + sy - top] = Source(sx, sy);
        return result;
    }

    private static void Check(int[,] expected, Surface destination)
    {
        for (int y = 0; y < DestinationHeight; y++)
            for (int x = 0; x < DestinationWidth; x++)
                Assert.True(expected[x, y] == destination.GetPixelInfo(x, y), $"pixel {x}, {y}: expected {expected[x, y]:X}, actual {destination.GetPixelInfo(x, y):X}");
    }

    private static void Run(Func<Surface, Surface, int> copy, int[,] expected, int result = PixelFormats.GFXE_OK)
    {
        var source = MakeSource();
        var destination = new DataSurface(DestinationWidth, DestinationHeight, PixelFormats.pfRGB555, null!);
        try
        {
            Assert.Equal(result, copy(source, destination));
            Check(expected, destination);
        }
        finally
        {
            source.Dispose();
            destination.Dispose();
        }
    }

    [Fact]
    public void BlitCopiesTheWholeSourceForANullRectangle()
        => Run((s, d) => s.Blit(d, null, 1, 1), Expected(0, 0, 4, 3, 1, 1));

    [Fact]
    public void BlitCopiesTheGivenRectangle()
        => Run((s, d) => s.Blit(d, new Rect(1, 1, 3, 3), 0, 0), Expected(1, 1, 3, 3, 0, 0));

    [Fact]
    public void BlitClippedMovesTheWholeSourceInsideTheDestination()
        // R = (0, 0, 4, 3) at (-1, -1): X < 0 cuts the left column, Y < 0 the top row.
        => Run((s, d) => s.BlitClipped(d, null, -1, -1), Expected(1, 1, 4, 3, 0, 0));

    [Fact]
    public void BlitClippedCutsARectangleToTheSource()
        // (-2, -1, 8, 5) at (0, 0): the right and bottom are cut to the source, the negative left
        // and top move X and Y by 2 and 1.
        => Run((s, d) => s.BlitClipped(d, new Rect(-2, -1, 8, 5), 0, 0), Expected(0, 0, 4, 3, 2, 1));

    [Fact]
    public void BlitClippedCutsAtTheDestinationEdge()
        // (0, 0, 4, 3) at (4, 3): W = 4 - (4 + 4 - 6) = 2, H = 3 - (3 + 3 - 5) = 2.
        => Run((s, d) => s.BlitClipped(d, null, 4, 3), Expected(0, 0, 2, 2, 4, 3));

    [Fact]
    public void BlitClippedDrawsNothingOutside()
        => Run((s, d) => s.BlitClipped(d, null, 6, 0), new int[DestinationWidth, DestinationHeight]);

    [Fact]
    public void BlitColorKeySkipsTheKeyForANullRectangle()
        => Run((s, d) => s.BlitColorKey(d, null, 0, 0, Source(1, 1)), Expected(0, 0, 4, 3, 0, 0, Source(1, 1)));

    [Fact]
    public void BlitColorKeySkipsTheKeyInTheGivenRectangle()
        => Run((s, d) => s.BlitColorKey(d, new Rect(0, 1, 3, 3), 2, 2, Source(2, 2)), Expected(0, 1, 3, 3, 2, 2, Source(2, 2)));

    [Fact]
    public void BlitColorKeyClippedClipsLikeBlitClipped()
        => Run((s, d) => s.BlitColorKeyClipped(d, null, -1, -1, Source(2, 1)), Expected(1, 1, 4, 3, 0, 0, Source(2, 1)));

    [Fact]
    public void BlitColorKeyClippedCutsARectangleToTheSource()
        => Run((s, d) => s.BlitColorKeyClipped(d, new Rect(-2, -1, 8, 5), 0, 0, Source(0, 0)), Expected(0, 0, 4, 3, 2, 1, Source(0, 0)));

    [Fact]
    public void BlitStretchOfUnequalSizesIsNotSupported()
        // Null rectangles are the whole source (4 by 3) and the whole destination (6 by 5).
        => Run((s, d) => s.BlitStretch(d, null, null), new int[DestinationWidth, DestinationHeight], PixelFormats.GFXE_NOTSUPPORTED);

    [Fact]
    public void BlitStretchOfEqualSizesCopiesToTheDestinationRectangle()
        => Run((s, d) => s.BlitStretch(d, new Rect(1, 0, 3, 2), new Rect(3, 2, 5, 4)), Expected(1, 0, 3, 2, 3, 2));

    [Fact]
    public void BlitStretchOfANullSourceRectangleTakesTheWholeSource()
        => Run((s, d) => s.BlitStretch(d, null, new Rect(2, 1, 6, 4)), Expected(0, 0, 4, 3, 2, 1));

    [Fact]
    public void BlitAreaClippedPassesItsArea()
        => Run((s, d) => s.BlitAreaClipped(d, new Rect(1, 0, 4, 2), 3, 3), Expected(1, 0, 4, 2, 3, 3));

    [Fact]
    public void BlitAreaColorKeyClippedPassesItsArea()
        => Run((s, d) => s.BlitAreaColorKeyClipped(d, new Rect(1, 0, 4, 2), 3, 3, Source(2, 1)), Expected(1, 0, 4, 2, 3, 3, Source(2, 1)));

    [Fact]
    public void CapturedSurfaceTakesTheWholeSourceOrTheRectangle()
    {
        var source = MakeSource();
        var whole = new DataSurface(source, null);
        var part = new DataSurface(source, new Rect(1, 1, 3, 3));
        try
        {
            Assert.Equal((SourceWidth, SourceHeight), (whole.Width, whole.Height));
            for (int y = 0; y < SourceHeight; y++)
                for (int x = 0; x < SourceWidth; x++) Assert.Equal(Source(x, y), whole.GetPixelInfo(x, y));
            Assert.Equal((2, 2, 1, 1), (part.Width, part.Height, part._x, part._y));
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
            LockedSurface full = default, part = default, sub = default;
            Assert.Equal(PixelFormats.GFXE_OK, source.Lock(ref full));
            source.Unlock();
            Assert.Equal(0, full.DataIndex);
            Assert.Equal(PixelFormats.GFXE_OK, source.Lock(ref part, new Rect(1, 1, 4, 3)));
            source.Unlock(part);
            Assert.Same(full.Data, part.Data);
            Assert.Equal(full.Pitch + 2, part.DataIndex);
            Assert.Equal((3, 2), (part.Width, part.Height));
            Assert.Equal(Source(1, 1), part.View().UInt16(0));
            Assert.Equal(Source(3, 2), part.View().UInt16(part.Pitch + 4));
            LockedSurface.CreateFrom(full, new Rect(2, 1, 4, 3), ref sub);
            Assert.Same(full.Data, sub.Data);
            Assert.Equal(full.Pitch + 4, sub.DataIndex);
            Assert.Equal(new Rect(0, 0, 2, 2), sub.ClipRectangle);
            Assert.Equal(Source(2, 1), sub.View().UInt16(0));
            Assert.Equal(PixelFormats.GFXE_ERROR, source.Lock(ref part, new Rect(1, 1, 5, 3)));
        }
        finally { source.Dispose(); }
    }

    // GetPrimarySurfaceRgn returns @FPrimarySurfaceRgn in the original: never nil, the rectangle
    // SetPrimarySurfaceRgn stored.
    [Fact]
    public void ControllersReturnThePrimaryRegion()
    {
        using var owner = new Component(null!);
        var displayController = new DisplayController(owner);
        var sdl = new SdlDisplayController(new NullDisplayHost(), owner);
        var region = new Rect(3, 4, 643, 484);
        displayController.SetPrimarySurfaceRegion(region);
        sdl.SetPrimarySurfaceRegion(region);
        Assert.Equal(region, displayController.GetPrimarySurfaceRegion());
        Assert.Equal(region, sdl.GetPrimarySurfaceRegion());
        Assert.Equal(region, displayController._primarySurfaceRegion);
    }

    private sealed class NullDisplayHost : Game.Host.IDisplayHost
    {
        public (int Width, int Height) ClientSize => (640, 480);
        public bool Fullscreen { get; set; }
        public bool HasFocus => true;

        public void Present(in LockedSurface surface) { }

        public (int X, int Y, bool Inside) PointerPosition() => (0, 0, true);

        public void PumpEvents(Game.Host.IDisplayEvents sink) { }

        public void ShowSystemCursor(bool show) { }
    }
}
