using System.Buffers.Binary;
using Aow.Game.Draw;
using Aow.Graphics;

namespace Aow.Game.Tests;

// GP-2: pins the blitters of Aow.Graphics and the GFXE converters on small buffers: positive and
// negative pitches, odd widths, unaligned starts, rows that run past the end of the memory, and a
// source and destination in one block of memory (the overlap cases). Each routine prints a hash of
// the destination after every case and the test compares the combined hash per routine. The
// expected hashes were recorded from the pointer implementation before the conversion to spans and
// stay unchanged through it. Only the region "Calls" follows the signatures.
public sealed class BlitterTests
{
    // A block of memory, the byte index of the first pixel of row 0 and the signed pitch.
    private readonly record struct Area(byte[] Memory, int Start, int Pitch);

    private delegate void Rectangle(Area source, Area destination, int width, int height);

    private delegate void Sprite(int x, int y, Area destination, Rect clip, byte[] data, int width, int height);

    #region Calls

    private static SurfaceView ViewOf(Area area, Rect clip = default) => new(area.Memory, area.Start, area.Pitch, clip);

    private static readonly byte[] s_table = Bytes(262144, 7);
    private static readonly byte[] s_blend = Bytes(1024, 8);

    // The 1,024 blend bytes read as 256 little-endian palette pixel values, as the pointer
    // implementation read its palette argument.
    private static readonly uint[] s_palettePixels = PixelValues(s_blend);

    private static uint[] PixelValues(byte[] bytes)
    {
        var values = new uint[bytes.Length / 4];
        for (int i = 0; i < values.Length; i++) values[i] = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan(4 * i, 4));
        return values;
    }

    private static readonly (string Name, Rectangle Call)[] s_copies =
    [
        ("BltRect", (s, d, w, h) => Blit.BlitRectangle(ViewOf(s), ViewOf(d), w, h)),
        ("RegisterBltRect", (s, d, w, h) => Blit.RegisterBlitRectangle(ViewOf(s), ViewOf(d), w, h)),
        ("RepMovsdBltRect", (s, d, w, h) => Blit.RepMovsdBlitRectangle(ViewOf(s), ViewOf(d), w, h)),
        ("GFXEUnit.BltRect", (s, d, w, h) => BlitRects.BlitRectangle(ViewOf(s), ViewOf(d), w, h)),
    ];

    private static readonly (string Name, int SourceBytes, Rectangle Call)[] s_pixels =
    [
        ("BltColorKey16", 2, (s, d, w, h) => Blit.BlitColorKey16(ViewOf(s), ViewOf(d), w, h, 0x1234)),
        ("BltTransparent555", 2, (s, d, w, h) => Blit.BlitTransparent555(ViewOf(s), ViewOf(d), w, h)),
        ("BltTransparent565", 2, (s, d, w, h) => Blit.BlitTransparent565(ViewOf(s), ViewOf(d), w, h)),
        ("RemapSurface16to16", 2, (s, d, w, h) => Blit.RemapSurface16To16(ViewOf(s), ViewOf(d), s_table, w, h)),
        ("RemapSurface16to08", 2, (s, d, w, h) => Blit.RemapSurface16To08(ViewOf(s), ViewOf(d), s_table, w, h)),
        ("RemapSurface16to32", 2, (s, d, w, h) => Blit.RemapSurface16To32(ViewOf(s), ViewOf(d), s_table, w, h)),
        ("RemapSurface555to32", 2, (s, d, w, h) => Blit.RemapSurface555To32(ViewOf(s), ViewOf(d), w, h)),
        ("RemapSurface565to32", 2, (s, d, w, h) => Blit.RemapSurface565To32(ViewOf(s), ViewOf(d), w, h)),
        ("RemapSurface555to565", 2, (s, d, w, h) => Blit.RemapSurface555To565(ViewOf(s), ViewOf(d), w, h)),
        ("RemapSurface565to555", 2, (s, d, w, h) => Blit.RemapSurface565To555(ViewOf(s), ViewOf(d), w, h)),
        ("BlendSurface555", 2, (s, d, w, h) => Blit.BlendSurface555(ViewOf(s), ViewOf(d), w, h, s_blend)),
        ("BlendSurface565", 2, (s, d, w, h) => Blit.BlendSurface565(ViewOf(s), ViewOf(d), w, h, s_blend)),
        ("BlendColorKey555", 2, (s, d, w, h) => Blit.BlendColorKey555(ViewOf(s), ViewOf(d), w, h, 0x1234, s_blend)),
        ("BlendColorKey565", 2, (s, d, w, h) => Blit.BlendColorKey565(ViewOf(s), ViewOf(d), w, h, 0x1234, s_blend)),
        ("RemapSurface08to24", 1, (s, d, w, h) => PixelRemap.RemapSurface08To24(ViewOf(s), ViewOf(d), w, h, s_palette)),
        ("RemapSurface08to555", 1, (s, d, w, h) => PixelRemap.RemapSurface08To555(ViewOf(s), ViewOf(d), w, h, s_palette)),
        ("RemapSurface08to565", 1, (s, d, w, h) => PixelRemap.RemapSurface08To565(ViewOf(s), ViewOf(d), w, h, s_palette)),
    ];

    // The six converters between 16-bit and 24 or 32-bit pixels. Before GP-2 each one raised
    // InvalidCastException on its first pixel (a boxed byte pointer unboxed as a word pointer), so their
    // expectation is the Delphi source (GFXE.pas:8325 to 8448), not a recording.
    private static readonly (string Name, int SourceBytes, int DestinationBytes, Rectangle Call)[] s_converters =
    [
        ("RemapSurface24to565", 3, 2, (s, d, w, h) => PixelRemap.RemapSurface24To565(ViewOf(s), ViewOf(d), w, h)),
        ("RemapSurface24to555", 3, 2, (s, d, w, h) => PixelRemap.RemapSurface24To555(ViewOf(s), ViewOf(d), w, h)),
        ("RemapSurface555to24", 2, 3, (s, d, w, h) => PixelRemap.RemapSurface555To24(ViewOf(s), ViewOf(d), w, h)),
        ("RemapSurface565to24", 2, 3, (s, d, w, h) => PixelRemap.RemapSurface565To24(ViewOf(s), ViewOf(d), w, h)),
        ("RemapSurface32to565", 4, 2, (s, d, w, h) => PixelRemap.RemapSurface32To565(ViewOf(s), ViewOf(d), w, h)),
        ("RemapSurface32to555", 4, 2, (s, d, w, h) => PixelRemap.RemapSurface32To555(ViewOf(s), ViewOf(d), w, h)),
    ];

    private static readonly (string Name, Action<Area, int[], int[], Area, int, int> Call)[] s_stretches =
    [
        ("BltStretched16", (s, rows, cols, d, w, h) => Blit.BlitStretched16(ViewOf(s), rows, cols, ViewOf(d), w, h)),
        ("BlendStretched555", (s, rows, cols, d, w, h) => Blit.BlendStretched555(ViewOf(s), rows, cols, ViewOf(d), w, h, s_blend)),
        ("BlendStretched565", (s, rows, cols, d, w, h) => Blit.BlendStretched565(ViewOf(s), rows, cols, ViewOf(d), w, h, s_blend)),
        ("GFXEUnit.BltStretched16", (s, rows, cols, d, w, h) => Game.Draw.Images.Picture16.BlitStretched16(ViewOf(s), rows, cols, ViewOf(d), w, h)),
        ("GFXEUnit.BlendStretched555", (s, rows, cols, d, w, h) => Game.Draw.Images.Picture16.BlendStretched555(ViewOf(s), rows, cols, ViewOf(d), w, h, s_blend)),
    ];

    private static readonly (string Name, Sprite Call)[] s_sprites16 =
    [
        ("BltRLE16", (x, y, d, clip, data, w, h) => Blit.BlitRle16(x, y, ViewOf(d, clip), data, w, h, 0)),
        ("BlendRLE555", (x, y, d, clip, data, w, h) => Blit.BlendRle555(x, y, ViewOf(d, clip), data, w, h, 0)),
        ("BlendRLE565", (x, y, d, clip, data, w, h) => Blit.BlendRle565(x, y, ViewOf(d, clip), data, w, h, 0)),
    ];

    private static readonly (string Name, Sprite Call)[] s_masks =
    [
        ("ShowBitMask16", (x, y, d, clip, data, w, h) => Blit.ShowBitMask16(x, y, ViewOf(d, clip), data, w, h)),
        ("ShowShadow555", (x, y, d, clip, data, w, h) => Blit.ShowShadow555(x, y, ViewOf(d, clip), data, w, h)),
        ("ShowShadow565", (x, y, d, clip, data, w, h) => Blit.ShowShadow565(x, y, ViewOf(d, clip), data, w, h)),
    ];

    private static readonly (string Name, Sprite Call)[] s_sprites08 =
    [
        ("ShowRLE08", (x, y, d, clip, data, w, h) => Blit.ShowRle08(x, y, ViewOf(d, clip), data, w, h, 255)),
        ("ShowRLE08to16", (x, y, d, clip, data, w, h) => Blit.ShowRle08To16(x, y, ViewOf(d, clip), data, w, h, 255, s_palettePixels)),
    ];

    private static void RemapRle(bool to565, byte[] data, int height)
    {
        if (to565) Blit.RemapRle555To565(data, height, 0);
        else Blit.RemapRle565To555(data, height, 0);
    }

    private static void DrawLine(int x1, int y1, int x2, int y2, int skip, Area d, Rect clip)
        => Blit.DrawLine16(x1, y1, x2, y2, skip, 0x6B5A, ViewOf(d, clip));

    #endregion

    private static readonly Palette s_palette = MakePalette();

    private static Palette MakePalette()
    {
        var palette = new Palette();
        var random = new Random(9);
        for (int i = 0; i < 256; i++)
        {
            byte red = (byte)random.Next(256), green = (byte)random.Next(256), blue = (byte)random.Next(256);
            palette[i] = new(red, green, blue);
        }
        return palette;
    }

    private static byte[] Bytes(int count, int seed)
    {
        var bytes = new byte[count];
        new Random(seed).NextBytes(bytes);
        return bytes;
    }

    // Random 16-bit pixels with the color key 0x1234 at about every fourth pixel.
    private static byte[] Pixels16(int count, int seed)
    {
        var bytes = Bytes(count, seed);
        var random = new Random(seed + 1000);
        for (int i = 0; i + 1 < count; i += 2)
            if (random.Next(4) == 0) BinaryPrimitives.WriteUInt16LittleEndian(bytes.AsSpan(i), 0x1234);
        return bytes;
    }

    private sealed class Recorder(string routine)
    {
        private uint _combined = 2166136261;
        private int _cases;

        public void Add(byte[] buffer, bool fault)
        {
            uint hash = 2166136261;
            foreach (byte b in buffer) hash = unchecked((hash ^ b) * 16777619);
            hash = unchecked((hash ^ (fault ? 1u : 0u)) * 16777619);
            _combined = unchecked((_combined ^ hash) * 16777619);
            _cases++;
        }

        public void Run(byte[] buffer, Action call)
        {
            bool fault = false;
            try { call(); }
            catch (IndexOutOfRangeException) { fault = true; }
            catch (ArgumentOutOfRangeException) { fault = true; }
            catch (NullReferenceException) { fault = true; }
            catch (AccessViolationException) { fault = true; }
            Add(buffer, fault);
        }

        public string Result => $"{routine} {_cases} {_combined:X8}";
    }

    // The area of a rectangle of `height` rows of `pitch` bytes at alignment `align`, in a block
    // with 16 spare bytes on both sides. A negative sign makes the rows bottom-up: row 0 is the
    // last row of the block and the pitch is negative.
    private static Area Make(byte[] memory, int align, int pitch, int height, int sign)
        => sign > 0 ? new Area(memory, 16 + align, pitch) : new Area(memory, 16 + (height - 1) * pitch + align, -pitch);

    private static int Size(int pitch, int height) => 32 + pitch * height;

    private static readonly int[] s_copyWidths = [1, 2, 3, 4, 5, 7, 8, 9, 30, 31, 32, 33, 63, 64, 65, 70];
    private static readonly int[] s_pixelWidths = [1, 2, 3, 4, 5, 15, 16, 17, 31, 33];

    private static readonly Dictionary<string, string> s_expected = new()
    {
        ["DrawLine16"] = "1800 FA6E601A",
        ["BltRLE16"] = "440 4C6CD59C",
        ["BlendRLE555"] = "440 4F17EC66",
        ["BlendRLE565"] = "440 129EBD76",
        ["ShowBitMask16"] = "440 E2583079",
        ["ShowShadow555"] = "440 42D54B7D",
        ["ShowShadow565"] = "440 235901A7",
        ["ShowRLE08"] = "440 065070FC",
        ["ShowRLE08to16"] = "440 4C7EE71A",
        ["RemapRLE555to565"] = "11 E8C55D85",
        ["RemapRLE565to555"] = "11 A0ECD1FD",
        ["BltRect-literal"] = "1 " +
            "00010203040405060708090A0B0B0D0E0F101112131315161718191A1B1B1D1E1F202122232325262728292A2B2D2E2F",
        ["RegisterBltRect-literal"] = "1 " +
            "00010203040405060708090A0B0B0D0E0F101112131315161718191A1B1B1D1E1F202122232325262728292A2B2D2E2F",
        ["RepMovsdBltRect-literal"] = "1 " +
            "00010203040405060707090A0B0B0D0E0F0F1112131315161717191A1B1B1D1E1F1F2122232325262727292A2B2D2E2F",
        ["GFXEUnit.BltRect-literal"] = "1 " +
            "00010203040405060708090A0B0B0D0E0F101112131315161718191A1B1B1D1E1F202122232325262728292A2B2D2E2F",
        ["BltRect"] = "1536 077B085C",
        ["RegisterBltRect"] = "1536 077B085C",
        ["RepMovsdBltRect"] = "1536 FF6590A2",
        ["GFXEUnit.BltRect"] = "1536 077B085C",
        ["BltColorKey16"] = "680 D8274095",
        ["BltTransparent555"] = "680 CFB4CA52",
        ["BltTransparent565"] = "680 47455F2A",
        ["RemapSurface16to16"] = "680 D166F55F",
        ["RemapSurface16to08"] = "680 D3C3A35D",
        ["RemapSurface16to32"] = "680 8616FEB7",
        ["RemapSurface555to32"] = "680 0CEA249F",
        ["RemapSurface565to32"] = "680 90C36AA3",
        ["RemapSurface555to565"] = "680 16B92BE0",
        ["RemapSurface565to555"] = "680 61481D85",
        ["BlendSurface555"] = "680 CAFFAE49",
        ["BlendSurface565"] = "680 BCFAFD3C",
        ["BlendColorKey555"] = "680 5EACDD91",
        ["BlendColorKey565"] = "680 1F530C5A",
        ["RemapSurface08to24"] = "640 65C15C72",
        ["RemapSurface08to555"] = "640 B2FD4A25",
        ["RemapSurface08to565"] = "640 75284938",
        ["BltRect-overlap"] = "1152 41D6236C",
        ["RegisterBltRect-overlap"] = "1152 41D6236C",
        ["RepMovsdBltRect-overlap"] = "1152 0B592AF4",
        ["GFXEUnit.BltRect-overlap"] = "1152 41D6236C",
        ["BltStretched16"] = "80 3F5A36E8",
        ["BlendStretched555"] = "80 1F848311",
        ["BlendStretched565"] = "80 C9201A19",
        ["GFXEUnit.BltStretched16"] = "80 3F5A36E8",
        ["GFXEUnit.BlendStretched555"] = "80 1F848311",
    };

    private static void Check(List<string> results)
    {
        string? record = Environment.GetEnvironmentVariable("AOW_BLITTER_RECORD");
        if (record is not null)
        {
            File.AppendAllLines(record, results);
            return;
        }
        foreach (string result in results)
        {
            string name = result[..result.IndexOf(' ')];
            Assert.True(s_expected.TryGetValue(name, out string? expected), $"no expectation for {name}");
            Assert.Equal(expected, result[(name.Length + 1)..]);
        }
    }

    [Fact]
    public void CopiesPinTheirBytes()
    {
        var results = new List<string>();
        foreach (var (name, call) in s_copies)
        {
            var recorder = new Recorder(name);
            int seed = 1;
            foreach (int height in (int[])[3, 0])
                foreach (int w in s_copyWidths)
                    for (int sa = 0; sa < 4; sa++)
                        for (int da = 0; da < 4; da++)
                            foreach (int sign in (int[])[1, -1])
                                foreach (bool cut in (bool[])[false, true])
                                {
                                    if (cut && height == 0) continue;
                                    var source = Bytes(Size(80, 3), seed++);
                                    var destination = Bytes(Size(88, 3) - (cut ? 104 - da - w / 2 : 0), seed++);
                                    var s = Make(source, sa, 80, 3, sign);
                                    var d = Make(destination, da, 88, 3, cut ? 1 : sign);
                                    recorder.Run(destination, () => call(s, d, w, height));
                                }
            results.Add(recorder.Result);
        }
        Check(results);
    }

    // The source and destination in one block: shifted within a row in both directions by less
    // and more than the width and the 8 and 32 byte steps of the original loops, and by a row
    // plus or minus a few bytes (a scroll).
    [Fact]
    public void CopiesWithinOneBlockPinTheOrderOfWrites()
    {
        int[] shifts = [-160 + 2, -33, -9, -8, -4, -3, -1, 0, 1, 2, 3, 4, 5, 7, 8, 9, 16, 24, 31, 32, 33, 159, 160, 163];
        var results = new List<string>();
        foreach (var (name, call) in s_copies)
        {
            var recorder = new Recorder(name + "-overlap");
            int seed = 5000;
            foreach (int w in (int[])[4, 9, 31, 33, 64, 70])
                foreach (int shift in shifts)
                    for (int a = 0; a < 4; a++)
                        foreach (int sign in (int[])[1, -1])
                        {
                            var memory = Bytes(1024, seed++);
                            int start = sign > 0 ? 200 + a : 200 + 3 * 160 + a;
                            var s = new Area(memory, start, 160 * sign);
                            var d = new Area(memory, start + shift, 160 * sign);
                            recorder.Run(memory, () => call(s, d, w, 3));
                        }
            results.Add(recorder.Result);
        }
        Check(results);
    }

    // The literal bytes of a forward shift by one byte within a row: the original copies in steps
    // of 4 or 8 bytes forward, so a shift shorter than the step repeats bytes.
    [Fact]
    public void ForwardShiftRepeatsBytesAsTheOriginal()
    {
        var results = new List<string>();
        foreach (var (name, call) in s_copies)
        {
            var memory = new byte[48];
            for (int i = 0; i < memory.Length; i++) memory[i] = (byte)i;
            call(new Area(memory, 4, 48), new Area(memory, 5, 48), 40, 1);
            results.Add($"{name}-literal 1 {System.Convert.ToHexString(memory)}");
        }
        Check(results);
    }

    [Fact]
    public void PixelRoutinesPinTheirBytes()
    {
        var results = new List<string>();
        foreach (var (name, size, call) in s_pixels)
        {
            var recorder = new Recorder(name);
            int seed = 10000;
            foreach (int w in s_pixelWidths)
                for (int sa = 0; sa < 4; sa++)
                    for (int da = 0; da < 4; da++)
                        foreach (int sign in (int[])[1, -1])
                            foreach (bool cut in (bool[])[false, true])
                            {
                                var source = Pixels16(Size(136, 3), seed++);
                                var destination = Pixels16(Size(144, 3) - (cut ? 160 - da - w : 0), seed++);
                                var s = Make(source, sa, 136, 3, sign);
                                var d = Make(destination, da, 144, 3, cut ? 1 : sign);
                                recorder.Run(destination, () => call(s, d, w, 3));
                            }
            // In place, as TImage16.RemapToPixelFormat converts its own buffer.
            if (size == 2)
                foreach (int w in s_pixelWidths)
                    for (int a = 0; a < 4; a++)
                    {
                        var memory = Pixels16(Size(144, 3), seed++);
                        var area = Make(memory, a, 144, 3, 1);
                        recorder.Run(memory, () => call(area, area, w, 3));
                    }
            results.Add(recorder.Result);
        }
        Check(results);
    }

    // One pixel as GFXE.pas writes it: the destination bytes from the source bytes.
    private static void Convert(string name, ReadOnlySpan<byte> s, Span<byte> d)
    {
        int word = s.Length >= 2 ? s[0] | s[1] << 8 : 0;
        switch (name)
        {
            case "RemapSurface24to565": BinaryPrimitives.WriteUInt16LittleEndian(d, (ushort)(((s[2] & 0xF8) << 8) + ((s[1] & 0xFC) << 3) + (s[0] >> 3))); break;
            case "RemapSurface24to555": BinaryPrimitives.WriteUInt16LittleEndian(d, (ushort)(((s[2] & 0xF8) << 7) + ((s[1] & 0xF8) << 2) + (s[0] >> 3))); break;
            case "RemapSurface555to24": d[2] = (byte)((word & 0x7C00) >> 7); d[1] = (byte)((word & 0x3E0) >> 2); d[0] = (byte)((word & 31) << 3); break;
            case "RemapSurface565to24": d[2] = (byte)((word & 0xF800) >> 8); d[1] = (byte)((word & 0x7E0) >> 3); d[0] = (byte)((word & 31) << 3); break;
            default: BinaryPrimitives.WriteUInt16LittleEndian(d, (ushort)(((s[1] & 0xF8) << 7) + ((s[2] & 0xF8) << 2) + (s[3] >> 3))); break;
        }
    }

    [Fact]
    public void ConvertersFollowTheDelphiSource()
    {
        foreach (var (name, sourceBytes, destinationBytes, call) in s_converters)
        {
            int seed = 15000;
            foreach (int w in s_pixelWidths)
                for (int sa = 0; sa < 4; sa++)
                    for (int da = 0; da < 4; da++)
                        foreach (int sign in (int[])[1, -1])
                        {
                            var source = Bytes(Size(136, 3), seed++);
                            var destination = Bytes(Size(144, 3), seed++);
                            var expected = (byte[])destination.Clone();
                            var s = Make(source, sa, 136, 3, sign);
                            var d = Make(destination, da, 144, 3, sign);
                            for (int y = 0; y < 3; y++)
                                for (int x = 0; x < w; x++)
                                    Convert(name, source.AsSpan(s.Start + y * s.Pitch + x * sourceBytes, sourceBytes), expected.AsSpan(d.Start + y * d.Pitch + x * destinationBytes, destinationBytes));
                            call(s, d, w, 3);
                            Assert.True(expected.AsSpan().SequenceEqual(destination), $"{name} {w} {sa} {da} {sign}");
                        }
        }
    }

    [Fact]
    public void StretchesPinTheirBytes()
    {
        var results = new List<string>();
        foreach (var (name, call) in s_stretches)
        {
            var recorder = new Recorder(name);
            int seed = 20000;
            foreach (int w in s_pixelWidths)
                for (int a = 0; a < 4; a++)
                    foreach (int sign in (int[])[1, -1])
                    {
                        var source = Pixels16(Size(96, 6), seed++);
                        var s = Make(source, a, 96, 6, sign);
                        var random = new Random(seed);
                        int[] rows = [.. Enumerable.Range(0, 4).Select(_ => random.Next(6) * s.Pitch)];
                        int[] cols = [.. Enumerable.Range(0, w).Select(_ => random.Next(40))];
                        var destination = Pixels16(Size(80, 4), seed++);
                        var d = Make(destination, (a + 1) & 3, 80, 4, sign);
                        recorder.Run(destination, () => call(s, rows, cols, d, w, 4));
                    }
            results.Add(recorder.Result);
        }
        Check(results);
    }

    // An RLE sprite of 16-bit pixels: per row a dword with the row's size in bytes including the
    // dword, then opaque pixels and transparent runs (the key 0 and the run length in bytes), the
    // row padded to 4 bytes.
    private static byte[] Rle16(int width, int height, Random random)
    {
        var data = new List<byte>();
        for (int y = 0; y < height; y++)
        {
            var words = new List<ushort>();
            for (int x = 0; x < width;)
            {
                if (random.Next(3) == 0)
                {
                    int run = 1 + random.Next(Math.Min(width - x, 5));
                    words.Add(0); words.Add((ushort)(run * 2)); x += run;
                }
                else { words.Add((ushort)(1 + random.Next(0xFFFF))); x++; }
            }
            int length = 4 + words.Count * 2;
            data.AddRange(BitConverter.GetBytes(length));
            foreach (ushort word in words) data.AddRange(BitConverter.GetBytes(word));
            while ((data.Count & 3) != 0) data.Add(0xCC);
        }
        return [.. data];
    }

    // An RLE sprite of 8-bit pixels: per row an unread dword, then opaque indices and transparent
    // runs (the key 255 and the run length in pixels).
    private static byte[] Rle08(int width, int height, Random random)
    {
        var data = new List<byte>();
        for (int y = 0; y < height; y++)
        {
            data.AddRange([0xAA, 0xBB, 0xCC, 0xDD]);
            for (int x = 0; x < width;)
            {
                if (random.Next(3) == 0)
                {
                    int run = 1 + random.Next(Math.Min(width - x, 5));
                    data.Add(255); data.Add((byte)run); x += run;
                }
                else { data.Add((byte)random.Next(255)); x++; }
            }
        }
        return [.. data];
    }

    // A bit mask: per row (width + 31) / 32 dwords, the leftmost pixel in the top bit.
    private static byte[] Mask(int width, int height, Random random)
    {
        int words = (width + 31) >> 5;
        var data = new byte[words * 4 * height + 64];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                if (random.Next(3) != 0)
                {
                    int at = y * words * 4 + (x >> 5) * 4;
                    uint bits = BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan(at)) | (0x80000000u >> (x & 31));
                    BinaryPrimitives.WriteUInt32LittleEndian(data.AsSpan(at), bits);
                }
        return data;
    }

    private static readonly int[] s_spriteWidths = [1, 2, 3, 5, 8, 17, 31, 32, 33, 40, 70];

    // The clip rectangles of a 96 by 12 surface: whole, each edge cut, a miss, an inner
    // rectangle, the sprite above and left of the origin, and cuts beyond the first mask dword.
    private static (Rect Clip, int X, int Y) Placement(int clip, int width)
    {
        int x = 4, y = 3;
        var rectangle = new Rect(0, 0, 96, 12);
        switch (clip)
        {
            case 1: rectangle.Left = 6; break;
            case 2: rectangle.Right = x + width - 2; break;
            case 3: rectangle.Top = 5; break;
            case 4: rectangle.Bottom = 5; break;
            case 5: rectangle = new Rect(90, 10, 96, 12); break;
            case 6: rectangle = new Rect(6, 4, x + width - 1, 6); break;
            case 7: x = -2; y = -1; break;
            case 8: rectangle.Left = 37; break;
            case 9: rectangle.Right = x + width - 32; break;
        }
        return (rectangle, x, y);
    }

    private static void RunSprites((string Name, Sprite Call)[] routines, Func<int, int, Random, byte[]> encode, int pixelBytes, List<string> results)
    {
        foreach (var (name, call) in routines)
        {
            var recorder = new Recorder(name);
            int seed = 30000;
            foreach (int w in s_spriteWidths)
                for (int clip = 0; clip < 10; clip++)
                    foreach (int sign in (int[])[1, -1])
                        for (int a = 0; a < 2; a++)
                        {
                            var data = encode(w, 4, new Random(seed));
                            var destination = Pixels16(Size(96 * pixelBytes + 16, 12), seed++);
                            var d = Make(destination, a * 2, 96 * pixelBytes + 16, 12, sign);
                            var (rectangle, x, y) = Placement(clip, w);
                            recorder.Run(destination, () => call(x, y, d, rectangle, data, w, 4));
                        }
            results.Add(recorder.Result);
        }
    }

    [Fact]
    public void SpritesPinTheirBytes()
    {
        var results = new List<string>();
        RunSprites(s_sprites16, Rle16, 2, results);
        RunSprites(s_masks, Mask, 2, results);
        RunSprites(s_sprites08, Rle08, 2, results);
        foreach (bool to565 in (bool[])[true, false])
        {
            var recorder = new Recorder(to565 ? "RemapRLE555to565" : "RemapRLE565to555");
            foreach (int w in s_spriteWidths)
            {
                var data = Rle16(w, 4, new Random(w));
                recorder.Run(data, () => RemapRle(to565, data, 4));
            }
            results.Add(recorder.Result);
        }
        Check(results);
    }

    [Fact]
    public void LinesPinTheirBytes()
    {
        int[] px = [-3, 0, 4, 11, 15, 7, 2, 5, 11, -2], py = [-2, 0, 4, 11, 14, 2, 7, 5, -2, 11];
        var recorder = new Recorder("DrawLine16");
        int seed = 40000;
        for (int i = 0; i < px.Length; i++)
            for (int j = 0; j < px.Length; j++)
                for (int skip = 0; skip < 3; skip++)
                    for (int clip = 0; clip < 3; clip++)
                        foreach (int sign in (int[])[1, -1])
                        {
                            var destination = Pixels16(Size(40, 16), seed++);
                            var d = Make(destination, 2, 40, 16, sign);
                            var rectangle = clip == 0 ? new Rect(0, 0, 16, 16) : clip == 1 ? new Rect(3, 3, 12, 12) : new Rect(5, 5, 5, 5);
                            recorder.Run(destination, () => DrawLine(px[i], py[i], px[j], py[j], skip, d, rectangle));
                        }
        Check([recorder.Result]);
    }
}
