using System.Reflection;
using System.Text.Json;
using Aow.Rtl;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Xunit.Abstractions;
using G = Aow.Units.GFXE.GFXEUnit;
using Images = Aow.Units.ImageLib.ImageLibUnit;
using LibraryImage = Aow.Units.ImageLib.TLibraryImage;
using TPaletteEntry = Aow.Units.GFXE.TPaletteEntry;

namespace Aow.Units.Tests;

// The image classes on the shipped libraries (graphics.md 6): every image of every installed
// library is decoded, its buffer hashed with its pitch and size, and drawn opaque, transparent,
// blended and stretched into test surfaces of both 16 bit formats (and the 8 bit images into an
// 8 bit surface). A second pass loads each library on demand and shows every image once, the
// path that reads the data at the first draw. The hashes per library were recorded at main
// cff6832, before GP-3 moved the image buffers off Delphi-style pointers, and stay unchanged.
// Set AOW_RECORD_IMAGE_DECODE to a folder to write the hashes there in place of comparing.
public sealed class ImageDecodeTests(ITestOutputHelper output)
{
    private sealed record Entry(string Library, int Images, string Hash);

    [InstalledLibrariesFact]
    public void EveryInstalledImageDecodesAndDrawsAsRecorded()
    {
        string root = InstalledFontsFactAttribute.Root;
        string[] files = InstalledLibrariesFactAttribute.Folders
            .SelectMany(folder => Directory.EnumerateFiles(folder, "*.ILB", SearchOption.AllDirectories))
            .Order(StringComparer.OrdinalIgnoreCase).ToArray();
        var entries = new List<Entry>();
        var classes = new SortedDictionary<string, int>(StringComparer.Ordinal);
        WithImageClasses(() =>
        {
            foreach (string file in files)
            {
                var hash = new Fnv();
                int count = 0;
                Decode(file, onDemand: false, image =>
                {
                    classes[image.GetType().Name] = classes.GetValueOrDefault(image.GetType().Name) + 1;
                    count++;
                    Pin(image, hash);
                });
                Decode(file, onDemand: true, image => hash.Add(Draw(image, G.pfRGB555, 3, 2, d => image.Show(3, 2, d))));
                entries.Add(new Entry(Path.GetRelativePath(root, file).Replace('\\', '/'), count, hash.Value.ToString("X16")));
            }
        });

        foreach (var (name, count) in classes) output.WriteLine($"{name}: {count}");
        output.WriteLine($"{entries.Count} libraries, {entries.Sum(e => e.Images)} images");
        // The image classes the shipped libraries hold. They hold no 8 bit picture, no transparent
        // 16 bit picture, no mask and no shadow: GrabbedImagesDecodeAndDrawAsRecorded covers those.
        Assert.Equal(["TPicture16", "TRLESprite08", "TRLESprite16", "TSprite16", "TTransparentRLESprite16"], classes.Keys);
        Check("Installed", entries);
    }

    // Compares the entries with their section of the snapshot, or writes them as the file
    // <section>.json in the folder AOW_RECORD_IMAGE_DECODE names.
    private static void Check(string section, List<Entry> entries)
    {
        if (Environment.GetEnvironmentVariable("AOW_RECORD_IMAGE_DECODE") is { Length: > 0 } record)
        {
            Directory.CreateDirectory(record);
            File.WriteAllText(Path.Combine(record, section + ".json"),
                JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true }).Replace("\r\n", "\n") + "\n");
            return;
        }

        using var stream = typeof(ImageDecodeTests).Assembly.GetManifestResourceStream("Aow.Units.Tests.ImageDecodeSnapshot.json")!;
        var expected = JsonSerializer.Deserialize<Dictionary<string, Entry[]>>(stream)![section];
        Assert.Equal(expected.Select(e => e.Library), entries.Select(e => e.Library));
        foreach (var (want, got) in expected.Zip(entries)) Assert.Equal(want, got);
    }

    // The classes the shipped libraries do not hold, and the grabbers of every class: every 400th
    // installed image is drawn on a 555 surface keyed with magenta, copied into 565, 8 bit and
    // 24 bit surfaces, and grabbed from each into every class that grabs that format, whole and
    // inset by a pixel. The grabbed image is pinned as an installed one, written to a stream, read
    // back and pinned again. One hash per source image covers all its grabs. GP-4 ported the 8 bit
    // grab of TRLESprite08, which raised NotImplementedException inside GrabImage and so returned
    // false before. With that grab reverted the recording of cff6832 matched in full, so the 59
    // sources whose hash changed changed only through it. The old grab raised at its first pixel,
    // so the one unchanged source (PoisnCld.ILB 1200) is one whose 8 bit grabs read no pixel.
    // TRLESprite08GrabTests checks the bytes of the encoding.
    [InstalledLibrariesFact]
    public void GrabbedImagesDecodeAndDrawAsRecorded()
    {
        string root = InstalledFontsFactAttribute.Root;
        string[] files = InstalledLibrariesFactAttribute.Folders
            .SelectMany(folder => Directory.EnumerateFiles(folder, "*.ILB", SearchOption.AllDirectories))
            .Order(StringComparer.OrdinalIgnoreCase).ToArray();
        var entries = new List<Entry>();
        WithImageClasses(() =>
        {
            int seen = 0;
            foreach (string file in files)
            {
                Decode(file, onDemand: false, image =>
                {
                    if (seen++ % 400 != 0 || image.Width < 1 || image.Height < 1) return;
                    string label = $"{Path.GetRelativePath(root, file).Replace('\\', '/')} {seen - 1}";
                    GrabAll(image, label, entries);
                });
            }
        });
        output.WriteLine($"{entries.Count} sources, {entries.Sum(e => e.Images)} grabs");
        Check("Grabbed", entries);
    }

    private const int Magenta555 = 0x7C1F;

    private static void GrabAll(LibraryImage source, string label, List<Entry> entries)
    {
        int w = Math.Min(source.Width, 120), h = Math.Min(source.Height, 90);
        var palette = new TGFXEPalette();
        for (int i = 0; i < 256; i++) palette.Entries[i] = new TPaletteEntry { R = (byte)(i * 5), G = (byte)(i * 7 + 3), B = (byte)(i * 13 + 1) };
        palette.PixelFormatID = G.pfBGR24;
        var s555 = new TDataSurface(w, h, G.pfRGB555, null!);
        var s565 = new TDataSurface(w, h, G.pfRGB565, null!);
        var s8 = new TDataSurface(w, h, G.pfPAL8, palette);
        var s24 = new TDataSurface(w, h, G.pfRGB24, null!);
        var hash = new Fnv();
        int grabs = 0;
        try
        {
            TSurfaceDesc d555 = default, d565 = default, d8 = default, d24 = default;
            s555.Lock(ref d555);
            s565.Lock(ref d565);
            s8.Lock(ref d8);
            s24.Lock(ref d24);
            var words = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, ushort>(d555.Data.AsSpan(d555.DataIndex, d555.Pitch * h));
            words.Fill(Magenta555);
            try { source.Show(0, 0, d555); }
            catch (System.Exception) { }
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    int c = words[y * d555.Pitch / 2 + x];
                    int r = (c >> 10) & 31, g = (c >> 5) & 31, b = c & 31;
                    int c565 = (r << 11) | (g << 6) | ((g >> 4) << 5) | b;
                    d565.View().Word(y * d565.Pitch + x * 2, c565);
                    d8.View()[y * d8.Pitch + x] = c == Magenta555 ? (byte)0 : (byte)(1 + (r ^ (g << 3) ^ (b << 5)) % 255);
                    var p24 = d24.View().Bytes(y * d24.Pitch + x * 3, 3);
                    p24[0] = (byte)(b << 3);
                    p24[1] = (byte)(g << 3);
                    p24[2] = (byte)(r << 3);
                }

            int key565 = 0xF81F, key24 = 0xF800F8;
            var sources = new (string Name, TSurfaceDesc SD, int Key)[] { ("555", d555, Magenta555), ("565", d565, key565), ("8", d8, 0), ("24", d24, key24) };
            var rects = new[] { RtlUnit.Rect(0, 0, w, h), RtlUnit.Rect(Math.Min(1, w - 1), Math.Min(1, h - 1), Math.Max(1, w - 1), Math.Max(1, h - 1)) };
            var grabbers = new Func<LibraryImage>[]
            {
                () => new TPicture08(null!), () => new TRLESprite08(null!), () => new TPicture16(null!),
                () => new TSprite16(null!), () => new TRLESprite16(null!), () => new TBitMask(null!), () => new TShadow(null!),
            };
            foreach (var (name, sd, key) in sources)
                foreach (var rect in rects)
                    foreach (var create in grabbers)
                        foreach (bool inverse in new[] { false, true })
                        {
                            var image = create();
                            if (!image.SupportGrabPixelFormat(sd.PixelFormatID) || (inverse && image is not TBitMask))
                            {
                                image.Destroy();
                                continue;
                            }
                            hash.Add($"{image.GetType().Name} {name} {rect.Left} {inverse}");
                            grabs++;
                            var grab = image.CreateGrabInfo();
                            grab.TransparentPixel = key;
                            grab.FileName = "grab.bmp";
                            if (grab is TBitMaskGrabInfo mask) mask.Inverse = inverse;
                            try
                            {
                                bool grabbed;
                                try { grabbed = image.GrabImage(sd, rect, grab); }
                                catch (System.Exception error) { hash.Add(error.GetType().Name); grabbed = false; }
                                hash.Add(grabbed ? 1 : 0);
                                if (grabbed)
                                {
                                    Pin(image, hash);
                                    RoundTrip(image, hash);
                                }
                            }
                            finally
                            {
                                grab.Destroy();
                                image.Destroy();
                            }
                        }
            entries.Add(new Entry(label, grabs, hash.Value.ToString("X16")));
            s24.Unlock();
            s8.Unlock();
            s565.Unlock();
            s555.Unlock();
        }
        finally
        {
            s24.Destroy();
            s8.Destroy();
            s565.Destroy();
            s555.Destroy();
            palette.Destroy();
        }
    }

    // Writes the image to a stream, reads it back and pins the copy.
    private static void RoundTrip(LibraryImage image, Fnv hash)
    {
        var stream = new TMemoryStream();
        try
        {
            try
            {
                Images.WriteImageToStream(image, stream);
                hash.Add(stream.Memory.AsSpan(0, stream.Size));
                stream.Position = 0;
                var copy = Images.ReadImageFromStream(stream, null!);
                try { Pin(copy, hash); }
                finally { copy.Destroy(); }
            }
            catch (System.Exception error) { hash.Add(error.GetType().Name); }
        }
        finally { stream.Destroy(); }
    }

    // The pixel readers of GFXE on every pixel of small surfaces of each format and width: the
    // virtual readers of the surface, the static GetPixelInfo, and the rectangle finders that read
    // pixels with their own nested readers. The 24 bit readers of GetPixelInfo and the two
    // Minimize functions read four bytes at every pixel (GFXE.pas 2326, 2547, 2614), so at the
    // last pixel of the last row they read into the 7 bytes a TDataSurface keeps after its rows,
    // as the original does. Recorded on cff6832 and re-recorded for those reads alone when the 7
    // bytes came back.
    [Fact]
    public void PixelReadersReadEveryFormatAsRecorded()
    {
        var entries = new List<Entry>();
        var palette = new TGFXEPalette();
        for (int i = 0; i < 256; i++) palette.Entries[i] = new TPaletteEntry { R = (byte)i, G = (byte)(i * 3), B = (byte)(i * 11) };
        palette.PixelFormatID = G.pfBGR24;
        try
        {
            foreach (int format in new[] { G.pfPAL8, G.pfRGB555, G.pfRGB565, G.pfRGB24, G.pfRGB32 })
                for (int w = 1; w <= 17; w++)
                {
                    int h = 3 + w % 3;
                    var surface = new TDataSurface(w, h, format, format == G.pfPAL8 ? palette : null!);
                    var hash = new Fnv();
                    try
                    {
                        TSurfaceDesc sd = default;
                        surface.Lock(ref sd);
                        var bytes = sd.Data.AsSpan(sd.DataIndex, sd.Pitch * h);
                        for (int i = 0; i < bytes.Length; i++) bytes[i] = (byte)((i * 7 + w) % 5 == 0 ? 0xC3 : i * 29 + 1);
                        hash.Add(sd.Pitch);
                        for (int y = 0; y < h; y++)
                            for (int x = 0; x < w; x++)
                            {
                                hash.Add(Read(() => surface.GetPixelInfo(x, y)));
                                hash.Add(Read(() => surface.GetColor(x, y)));
                                hash.Add(Read(() => G.GetPixelInfo(sd, x, y)));
                            }
                        foreach (int key in new[] { Read(() => G.GetPixelInfo(sd, 0, 0)), Read(() => surface.GetPixelInfo(w - 1, h - 1)), 0xC3, 0xC3C3 })
                        {
                            var whole = RtlUnit.Rect(0, 0, w, h);
                            var inset = RtlUnit.Rect(0, 1, Math.Max(0, w - 1), h);
                            foreach (var rect in new[] { whole, inset })
                            {
                                TRect found = default;
                                hash.Add(Read(() => { G.MinimizeTransparentRect(sd, rect, key, ref found); return 0; }));
                                foreach (int value in new[] { found.Left, found.Top, found.Right, found.Bottom }) hash.Add(value);
                                found = default;
                                hash.Add(Read(() => { G.MinimizeInvTransparentRect(sd, rect, key, ref found); return 0; }));
                                foreach (int value in new[] { found.Left, found.Top, found.Right, found.Bottom }) hash.Add(value);
                            }
                            TRectangleList? list = null;
                            hash.Add(Read(() => { list = G.GetSurfaceRectangles(sd, key, null!); return 0; }));
                            hash.Add(list?.Count ?? -1);
                            if (list is not null)
                            {
                                for (int i = 0; i < list.Count; i++)
                                    foreach (int value in new[] { list.GetRectangle(i).Rect.Left, list.GetRectangle(i).Rect.Top, list.GetRectangle(i).Rect.Right, list.GetRectangle(i).Rect.Bottom }) hash.Add(value);
                                list.Free();
                            }
                        }
                        surface.Unlock();
                    }
                    finally { surface.Destroy(); }
                    entries.Add(new Entry($"{format:X8} {w}", h, hash.Value.ToString("X16")));
                }
        }
        finally { palette.Destroy(); }
        Check("Pixels", entries);
    }

    // The blend tables GFXE.Initialize builds, as the blenders read them (Bytes) and as their
    // generators write them (Table[I][J]), and a copy of each (Copy and Assign).
    [Fact]
    public void BlendTablesHoldTheirBytesAsRecorded()
    {
        var entries = new List<Entry>();
        WithImageClasses(() =>
        {
            foreach (var (name, collection) in new[] { ("alpha", G.AlphaBlendTable), ("intensity", G.IntensityBlendTable),
                         ("brighten", G.BrightenBlendTable), ("shadow", G.ShadowBlendTable), ("linear", G.LinearAlphaBlendTable) })
            {
                var hash = new Fnv();
                for (int k = 0; k < collection.Tables.Count; k++)
                {
                    var table = (TBlendTable5Bit)collection.Tables.Tables[k];
                    Assert.Equal(5, table.Bits);
                    hash.Add(table.Bytes);
                    var copy = (TBlendTable5Bit)table.Copy();
                    try
                    {
                        for (int i = 0; i < 32; i++)
                            for (int j = 0; j < 32; j++)
                            {
                                Assert.Equal(table.Bytes[i * 32 + j], table.Table[i][j]);
                                Assert.Equal(table.Table[i][j], copy.Table[i][j]);
                            }
                        copy.Table[3][5] = (byte)(table.Table[3][5] ^ 0xFF);
                        Assert.Equal(copy.Table[3][5], copy.Bytes[3 * 32 + 5]);
                        Assert.NotEqual(copy.Table[3][5], table.Table[3][5]);
                    }
                    finally { copy.Destroy(); }
                }
                for (int i = 0; i <= 100; i++) hash.Add(collection.Tables.Count == 0 ? -1 : IndexOf(collection, collection.Lookup[i]));
                entries.Add(new Entry(name, collection.Tables.Count, hash.Value.ToString("X16")));
            }
        });
        Check("Blend", entries);
    }

    private static int IndexOf(TLookupBlendTableCollection100 collection, TBlendTable table)
    {
        for (int k = 0; k < collection.Tables.Count; k++)
            if (ReferenceEquals(collection.Tables.Tables[k], table)) return k;
        return -1;
    }

    // The value a reader returns, or a code for the exception it raises.
    private static int Read(Func<int> reader)
    {
        try { return reader(); }
        catch (System.Exception error)
        {
            var name = new Fnv();
            name.Add(error.GetType().Name);
            return (int)(name.Value & 0x3FFFFFFF) | 1 << 30;
        }
    }

    // Loads a library and hands every image to the action, the images of the nodes in order.
    private static void Decode(string file, bool onDemand, Action<LibraryImage> action)
    {
        var library = new TImageLibrary(null!);
        try
        {
            library.LoadOnDemand = onDemand;
            library.LoadFromFile(file);
            for (int i = 0; i < library.Count; i++)
                if (!library.IsReference(i)) Visit(library[i], action);
        }
        finally { library.Destroy(); }
    }

    private static void Visit(LibraryImage? image, Action<LibraryImage> action)
    {
        if (image is null) return;
        if (image is TImageNode node)
        {
            for (int i = 0; i < node.Count; i++) Visit(node[i], action);
            return;
        }
        action(image);
    }

    // The decoded buffer, its pitch and size, then the draws in a fixed order. A draw into the
    // other 16 bit format remaps the image in place, so the buffer is hashed again at the end.
    private static void Pin(LibraryImage image, Fnv hash)
    {
        hash.Add(image.GetType().Name);
        hash.Add(image.DataLoaded ? 1 : 0);
        foreach (int value in new[] { image.DataWidth, image.DataHeight, image.Width, image.Height, image.Xshift, image.Yshift, image.DataSize })
            hash.Add(value);
        AddBuffer(image, hash);
        int w = image.Width, h = image.Height;
        var stretched = RtlUnit.Rect(1, 1, 1 + Math.Max(1, w * 3 / 2), 1 + Math.Max(1, h * 2 / 3));
        var shrunk = RtlUnit.Rect(-w / 4, 2, -w / 4 + Math.Max(1, w / 2), 2 + Math.Max(1, h * 5 / 4));
        if (image is TImage08)
        {
            hash.Add(Draw(image, G.pfPAL8, 3, 2, d => image.ShowOpaque(3, 2, d)));
            hash.Add(Draw(image, G.pfPAL8, -w / 3, -h / 3, d => image.ShowOpaque(-w / 3, -h / 3, d)));
        }

        foreach (int format in new[] { G.pfRGB555, G.pfRGB565 })
        {
            hash.Add(Draw(image, format, 3, 2, d => image.ShowOpaque(3, 2, d)));
            hash.Add(Draw(image, format, -w / 3, -h / 3, d => image.ShowOpaque(-w / 3, -h / 3, d)));
            hash.Add(Draw(image, format, 3, 2, d => image.ShowTransparent(3, 2, d)));
            hash.Add(Draw(image, format, 3, 2, d => image.ShowBlended(3, 2, d, G.AlphaBlendTable.Lookup[50])));
            hash.Add(Draw(image, format, -w / 3, -h / 3, d => image.ShowBlended(-w / 3, -h / 3, d, G.IntensityBlendTable.Lookup[30])));
            hash.Add(Draw(image, format, 0, 0, d => image.ShowOpaqueStretched(stretched, d)));
            hash.Add(Draw(image, format, 0, 0, d => image.ShowOpaqueStretched(shrunk, d)));
            hash.Add(Draw(image, format, 0, 0, d => image.ShowBlendedStretched(stretched, d, G.AlphaBlendTable.Lookup[50])));
            hash.Add(Draw(image, format, 0, 0, d => image.ShowBlendedStretched(shrunk, d, G.ShadowBlendTable.Lookup[70])));
        }
        AddBuffer(image, hash);
    }

    // The image's decoded buffer as the class holds it, a byte array.
    private static void AddBuffer(LibraryImage image, Fnv hash)
    {
        var type = image.GetType();
        const BindingFlags Fields = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        var field = new[] { "FBuffer", "FMaskData", "FData" }.Select(name => type.GetField(name, Fields)).FirstOrDefault(f => f is not null);
        var pitch = type.GetField("FPitch", Fields);
        hash.Add(pitch is null ? -1 : (int)pitch.GetValue(image)!);
        byte[]? bytes = field?.GetValue(image) switch
        {
            byte[] array => array,
            null => null,
            var other => throw new InvalidOperationException($"{type.Name}.{field!.Name} is a {other.GetType().Name}"),
        };
        hash.Add(bytes is null ? -1 : bytes.Length);
        if (bytes is not null) hash.Add(bytes);
    }

    // A surface two pixels wider and taller on each side than the image, filled with a pattern,
    // clipped one pixel in from its edges, after the draw: the hash of its pixels, or the name of
    // the exception the draw raised.
    private static ulong Draw(LibraryImage image, int format, int x, int y, Action<TSurfaceDesc> draw)
    {
        int w = Math.Max(1, image.Width + 6), h = Math.Max(1, image.Height + 4);
        var palette = new TGFXEPalette();
        var surface = new TDataSurface(w, h, format, palette);
        var hash = new Fnv();
        try
        {
            TSurfaceDesc sd = default;
            Assert.Equal(0, surface.Lock(ref sd));
            var pixels = sd.Data.AsSpan(sd.DataIndex, sd.Pitch * h);
            for (int i = 0; i < pixels.Length; i++) pixels[i] = (byte)(i * 37 + x * 5 + y);
            sd.ClipRect = RtlUnit.Rect(1, 1, w - 1, h - 1);
            try { draw(sd); }
            catch (System.Exception error) { hash.Add(error.GetType().Name); }
            hash.Add(pixels);
            surface.Unlock();
        }
        finally
        {
            surface.Destroy();
            palette.Destroy();
        }
        return hash.Value;
    }

    // The classes registered and the blend tables built, as at game start.
    private static void WithImageClasses(Action body)
    {
        var previousClasses = Images.LibraryImageClasses;
        // Engine startup sets the base to the startup directory, so a library stores its name
        // relative to it and loads its on-demand images from there.
        var previousBase = Images.BaseImageLibraryPath;
        Images.BaseImageLibraryPath = InstalledFontsFactAttribute.Root + Path.DirectorySeparatorChar;
        var previousTables = (G.AlphaBlendTable, G.IntensityBlendTable, G.BrightenBlendTable, G.ShadowBlendTable, G.LinearAlphaBlendTable);
        (G.AlphaBlendTable, G.IntensityBlendTable, G.BrightenBlendTable, G.ShadowBlendTable, G.LinearAlphaBlendTable) =
            (new(), new(), new(), new(), new());
        Images.LibraryImageClasses = new TList();
        try
        {
            G.InitBlendTables(5, 31);
            Images.RegisterLibraryImageClasses([
                LibraryImageClass.Of<TPicture08>(), LibraryImageClass.Of<TRLESprite08>(),
                LibraryImageClass.Of<TPicture16>(), LibraryImageClass.Of<TTransparentPicture16>(),
                LibraryImageClass.Of<TRLESprite16>(), LibraryImageClass.Of<TTransparentRLESprite16>(),
                LibraryImageClass.Of<TShadow>(), LibraryImageClass.Of<TImageNode>(), LibraryImageClass.Of<TSprite16>(),
                LibraryImageClass.Of<TBitMask>()]);
            body();
        }
        finally
        {
            Images.LibraryImageClasses.Destroy();
            Images.LibraryImageClasses = previousClasses;
            Images.BaseImageLibraryPath = previousBase;
            G.AlphaBlendTable.Free();
            G.IntensityBlendTable.Free();
            G.BrightenBlendTable.Free();
            G.ShadowBlendTable.Free();
            G.LinearAlphaBlendTable.Free();
            (G.AlphaBlendTable, G.IntensityBlendTable, G.BrightenBlendTable, G.ShadowBlendTable, G.LinearAlphaBlendTable) = previousTables;
        }
    }

    // FNV-1a over 64 bits.
    private sealed class Fnv
    {
        public ulong Value { get; private set; } = 14695981039346656037ul;

        public void Add(ReadOnlySpan<byte> bytes)
        {
            ulong h = Value;
            foreach (byte b in bytes) h = (h ^ b) * 1099511628211ul;
            Value = h;
        }

        public void Add(int value) => Add(BitConverter.GetBytes(value));

        public void Add(ulong value) => Add(BitConverter.GetBytes(value));

        public void Add(string value) => Add(Cp1252.Encode(value));
    }
}
