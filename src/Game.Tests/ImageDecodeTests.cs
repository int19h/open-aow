using System.Reflection;
using System.Text.Json;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Xunit.Abstractions;
using LibraryImage = Aow.Game.Draw.Images.LibraryImage;
using TPaletteEntry = Aow.Game.Draw.Palette.Entry;

namespace Aow.Game.Tests;

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
        string[] files = [.. InstalledLibrariesFactAttribute.Folders
            .SelectMany(folder => Directory.EnumerateFiles(folder, "*.ILB", SearchOption.AllDirectories))
            .Order(StringComparer.OrdinalIgnoreCase)];
        List<Entry> entries = [];
        SortedDictionary<string, int> classes = [with(StringComparer.Ordinal)];
        WithImageClasses(() =>
        {
            foreach (string file in files)
            {
                Fnv hash = new();
                int count = 0;
                Decode(file, onDemand: false, image =>
                {
                    classes[image.GetType().Name] = classes.GetValueOrDefault(image.GetType().Name) + 1;
                    count++;
                    Pin(image, hash);
                });
                Decode(file, onDemand: true, image => hash.Add(Draw(image, PixelFormat.Rgb555, 3, 2, d => image.Show(3, 2, d))));
                entries.Add(new Entry(Path.GetRelativePath(root, file).Replace('\\', '/'), count, hash.Value.ToString("X16")));
            }
        });

        foreach ((string? name, int count) in classes)
        {
            output.WriteLine($"{name}: {count}");
        }

        output.WriteLine($"{entries.Count} libraries, {entries.Sum(e => e.Images)} images");
        // The image classes the shipped libraries hold. They hold no 8 bit picture, no transparent
        // 16 bit picture, no mask and no shadow: GrabbedImagesDecodeAndDrawAsRecorded covers those.
        Assert.Equal(["Picture16", "RleSprite08", "RleSprite16", "Sprite16", "TransparentRleSprite16"], classes.Keys);
        Check("Installed", entries);
    }

    // Compares the entries with their section of the snapshot, or writes them as the file
    // <section>.json in the folder AOW_RECORD_IMAGE_DECODE names.
    private static void Check(string section, List<Entry> entries)
    {
        if (Environment.GetEnvironmentVariable("AOW_RECORD_IMAGE_DECODE") is { Length: > 0 } record)
        {
            _ = Directory.CreateDirectory(record);
            File.WriteAllText(Path.Combine(record, $"{section}.json"),
                $"{JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true }).Replace("\r\n", "\n")}\n");
            return;
        }

        using Stream stream = typeof(ImageDecodeTests).Assembly.GetManifestResourceStream("Aow.Game.Tests.ImageDecodeSnapshot.json")!;
        Entry[] expected = JsonSerializer.Deserialize<Dictionary<string, Entry[]>>(stream)![section];
        Assert.Equal(expected.Select(e => e.Library), entries.Select(e => e.Library));
        foreach ((Entry? want, Entry? got) in expected.Zip(entries))
        {
            Assert.Equal(want, got);
        }
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
    // RleSprite08GrabTests checks the bytes of the encoding.
    [InstalledLibrariesFact]
    public void GrabbedImagesDecodeAndDrawAsRecorded()
    {
        string root = InstalledFontsFactAttribute.Root;
        string[] files = [.. InstalledLibrariesFactAttribute.Folders
            .SelectMany(folder => Directory.EnumerateFiles(folder, "*.ILB", SearchOption.AllDirectories))
            .Order(StringComparer.OrdinalIgnoreCase)];
        List<Entry> entries = [];
        WithImageClasses(() =>
        {
            int seen = 0;
            foreach (string file in files)
            {
                Decode(file, onDemand: false, image =>
                {
                    if (seen++ % 400 != 0 || image.Width < 1 || image.Height < 1)
                    {
                        return;
                    }

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
        Palette palette = new();
        for (int i = 0; i < 256; i++)
        {
            palette[i] = new TPaletteEntry { Red = (byte)(i * 5), Green = (byte)((i * 7) + 3), Blue = (byte)((i * 13) + 1) };
        }

        palette.PixelFormatId = PixelFormat.Bgr24;
        DataSurface s555 = new(w, h, PixelFormat.Rgb555, null!);
        DataSurface s565 = new(w, h, PixelFormat.Rgb565, null!);
        DataSurface s8 = new(w, h, PixelFormat.Pal8, palette);
        DataSurface s24 = new(w, h, PixelFormat.Rgb24, null!);
        Fnv hash = new();
        try
        {
            LockedSurface d555 = default, d565 = default, d8 = default, d24 = default;
            _ = s555.Lock(ref d555);
            _ = s565.Lock(ref d565);
            _ = s8.Lock(ref d8);
            _ = s24.Lock(ref d24);
            Span<ushort> words = System.Runtime.InteropServices.MemoryMarshal.Cast<byte, ushort>(d555.Data.AsSpan(d555.DataIndex, d555.Pitch * h));
            words.Fill(Magenta555);
            try { source.Show(0, 0, d555); }
            catch (Exception) { }
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int c = words[(y * d555.Pitch / 2) + x];
                    int r = (c >> 10) & 31, g = (c >> 5) & 31, b = c & 31;
                    d565.View().UInt16(value: (r << 11) | (g << 6) | ((g >> 4) << 5) | b, offset: (y * d565.Pitch) + (x * 2));
                    d8.View()[(y * d8.Pitch) + x] = c == Magenta555 ? (byte)0 : (byte)(1 + ((r ^ (g << 3) ^ (b << 5)) % 255));
                    Span<byte> p24 = d24.View().Bytes((y * d24.Pitch) + (x * 3), 3);
                    p24[0] = (byte)(b << 3);
                    p24[1] = (byte)(g << 3);
                    p24[2] = (byte)(r << 3);
                }
            }

            int key565 = 0xF81F, key24 = 0xF800F8;
            (string Name, LockedSurface SD, int Key)[] sources = [("555", d555, Magenta555), ("565", d565, key565), ("8", d8, 0), ("24", d24, key24)];
            Rect[] rects = [new Rect(0, 0, w, h), new Rect(Math.Min(1, w - 1), Math.Min(1, h - 1), Math.Max(1, w - 1), Math.Max(1, h - 1))];
            Func<LibraryImage>[] grabbers =
            [
                () => new Picture08(null!), () => new RleSprite08(null!), () => new Picture16(null!),
                () => new Sprite16(null!), () => new RleSprite16(null!), () => new BitMask(null!), () => new Shadow(null!),
            ];
            int grabs = 0;
            foreach ((string? name, LockedSurface lockedSurface, int key) in sources)
            {
                foreach (Rect rectangle in rects)
                {
                    foreach (Func<LibraryImage> create in grabbers)
                    {
                        foreach (bool inverse in new[] { false, true })
                        {
                            LibraryImage image = create();
                            if (!image.SupportGrabPixelFormat(lockedSurface.PixelFormatId) || (inverse && image is not BitMask))
                            {
                                image.Dispose();
                                continue;
                            }
                            hash.Add($"{RecordedTypeName(image.GetType())} {name} {rectangle.Left} {inverse}");
                            grabs++;
                            GrabInfo grab = image.CreateGrabInfo();
                            grab.TransparentPixel = key;
                            grab.FileName = "grab.bmp";
                            if (grab is BitMask.GrabInfo mask)
                            {
                                mask.Inverse = inverse;
                            }

                            try
                            {
                                bool grabbed;
                                try { grabbed = image.GrabImage(lockedSurface, rectangle, grab); }
                                catch (Exception error) { hash.Add(error.GetType().Name); grabbed = false; }
                                hash.Add(grabbed ? 1 : 0);
                                if (grabbed)
                                {
                                    Pin(image, hash);
                                    RoundTrip(image, hash);
                                }
                            }
                            finally
                            {
                                image.Dispose();
                            }
                        }
                    }
                }
            }

            entries.Add(new Entry(label, grabs, hash.Value.ToString("X16")));
            _ = s24.Unlock();
            _ = s8.Unlock();
            _ = s565.Unlock();
            _ = s555.Unlock();
        }
        finally
        {
            s24.Dispose();
            s8.Dispose();
            s565.Dispose();
            s555.Dispose();
        }
    }

    // Writes the image to a stream, reads it back and pins the copy.
    private static void RoundTrip(LibraryImage image, Fnv hash)
    {
        using MemoryStream stream = new();
        try
        {
            LibraryImage.WriteImageToStream(image, stream);
            hash.Add(stream.GetBuffer().AsSpan(0, (int)stream.Length));
            stream.Position = 0;
            LibraryImage copy = ImageStreams.ReadImageFromStream(stream, null);
            try { Pin(copy, hash); }
            finally { copy.Dispose(); }
        }
        catch (Exception error) { hash.Add(error.GetType().Name); }
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
        List<Entry> entries = [];
        Palette palette = new();
        for (int i = 0; i < 256; i++)
        {
            palette[i] = new TPaletteEntry { Red = (byte)i, Green = (byte)(i * 3), Blue = (byte)(i * 11) };
        }

        palette.PixelFormatId = PixelFormat.Bgr24;
        {
            foreach (PixelFormat format in new[] { PixelFormat.Pal8, PixelFormat.Rgb555, PixelFormat.Rgb565, PixelFormat.Rgb24, PixelFormat.Rgb32 })
            {
                for (int w = 1; w <= 17; w++)
                {
                    int h = 3 + (w % 3);
                    DataSurface surface = new(w, h, format, format == PixelFormat.Pal8 ? palette : null);
                    Fnv hash = new();
                    try
                    {
                        LockedSurface lockedSurface = default;
                        _ = surface.Lock(ref lockedSurface);
                        Span<byte> bytes = lockedSurface.Data.AsSpan(lockedSurface.DataIndex, lockedSurface.Pitch * h);
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            bytes[i] = (byte)(((i * 7) + w) % 5 == 0 ? 0xC3 : (i * 29) + 1);
                        }

                        hash.Add(lockedSurface.Pitch);
                        for (int y = 0; y < h; y++)
                        {
                            for (int x = 0; x < w; x++)
                            {
                                hash.Add(Read(() => surface.GetPixelInfo(x, y)));
                                hash.Add(Read(() => surface.GetColor(x, y)));
                                hash.Add(Read(() => BitMask.GetPixelInfo(lockedSurface, x, y)));
                            }
                        }

                        foreach (int key in new[] { Read(() => BitMask.GetPixelInfo(lockedSurface, 0, 0)), Read(() => surface.GetPixelInfo(w - 1, h - 1)), 0xC3, 0xC3C3 })
                        {
                            Rect whole = new(0, 0, w, h);
                            Rect inset = new(0, 1, Math.Max(0, w - 1), h);
                            foreach (Rect rectangle in new[] { whole, inset })
                            {
                                Rect found = default;
                                hash.Add(Read(() => { LockedSurface.MinimizeTransparentRectangle(lockedSurface, rectangle, key, out found); return 0; }));
                                foreach (int value in new[] { found.Left, found.Top, found.Right, found.Bottom })
                                {
                                    hash.Add(value);
                                }

                                found = default;
                                hash.Add(Read(() => { BitMask.MinimizeInverseTransparentRectangle(lockedSurface, rectangle, key, out found); return 0; }));
                                foreach (int value in new[] { found.Left, found.Top, found.Right, found.Bottom })
                                {
                                    hash.Add(value);
                                }
                            }
                            RectangleList? list = null;
                            hash.Add(Read(() => { list = LibraryImageClass.GetSurfaceRectangles(lockedSurface, key, null); return 0; }));
                            hash.Add(list?.Count ?? -1);
                            if (list is not null)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    foreach (int value in new[] { list.GetRectangle(i).Bounds.Left, list.GetRectangle(i).Bounds.Top, list.GetRectangle(i).Bounds.Right, list.GetRectangle(i).Bounds.Bottom })
                                    {
                                        hash.Add(value);
                                    }
                                }

                                list?.Dispose();
                            }
                        }
                        _ = surface.Unlock();
                    }
                    finally { surface.Dispose(); }
                    entries.Add(new Entry($"{(int)format:X8} {w}", h, hash.Value.ToString("X16")));
                }
            }
        }
        Check("Pixels", entries);
    }

    // The blend tables GFXE.Initialize builds, as the blenders read them (Bytes) and as their
    // generators write them (Table[I][J]), and a copy of each (Copy and Assign).
    [Fact]
    public void BlendTablesHoldTheirBytesAsRecorded()
    {
        List<Entry> entries = [];
        WithImageClasses(() =>
        {
            foreach ((string? name, LookupBlendTableCollection100? collection) in new[] { ("alpha", GraphicsState.AlphaBlendTable), ("intensity", GraphicsState.IntensityBlendTable),
                         ("brighten", GraphicsState.BrightenBlendTable), ("shadow", GraphicsState.ShadowBlendTable), ("linear", GraphicsState.LinearAlphaBlendTable) })
            {
                Fnv hash = new();
                for (int k = 0; k < collection.Tables.Count; k++)
                {
                    BlendTable5Bit table = (BlendTable5Bit)collection.Tables.Tables[k];
                    Assert.Equal(5, table.Bits);
                    hash.Add(table.Bytes);
                    BlendTable5Bit copy = (BlendTable5Bit)table.Copy();
                    {
                        for (int i = 0; i < 32; i++)
                        {
                            for (int j = 0; j < 32; j++)
                            {
                                Assert.Equal(table.Bytes[(i * 32) + j], table.RowView[i][j]);
                                Assert.Equal(table.RowView[i][j], copy.RowView[i][j]);
                            }
                        }

                        copy.RowView[3][5] = (byte)(table.RowView[3][5] ^ 0xFF);
                        Assert.Equal(copy.RowView[3][5], copy.Bytes[(3 * 32) + 5]);
                        Assert.NotEqual(copy.RowView[3][5], table.RowView[3][5]);
                    }
                }
                for (int i = 0; i <= 100; i++)
                {
                    hash.Add(collection.Tables.Count == 0 ? -1 : IndexOf(collection, collection.Lookup[i]));
                }

                entries.Add(new Entry(name, collection.Tables.Count, hash.Value.ToString("X16")));
            }
        });
        Check("Blend", entries);
    }

    private static int IndexOf(LookupBlendTableCollection100 collection, BlendTable table)
    {
        for (int k = 0; k < collection.Tables.Count; k++)
        {
            if (ReferenceEquals(collection.Tables.Tables[k], table))
            {
                return k;
            }
        }

        return -1;
    }

    // The value a reader returns, or a code for the exception it raises.
    private static int Read(Func<int> reader)
    {
        try { return reader(); }
        catch (Exception error)
        {
            Fnv name = new();
            name.Add(error.GetType().Name);
            return (int)(name.Value & 0x3FFFFFFF) | (1 << 30);
        }
    }

    // Loads a library and hands every image to the action, the images of the nodes in order.
    private static void Decode(string file, bool onDemand, Action<LibraryImage> action)
    {
        using ImageLibrary library = new(null!);
        library.LoadOnDemand = onDemand;
        library.LoadFromFile(file);
        for (int i = 0; i < library.Count; i++)
        {
            if (!library.IsReference(i))
            {
                Visit(library[i], action);
            }
        }
    }

    private static void Visit(LibraryImage? image, Action<LibraryImage> action)
    {
        if (image is null)
        {
            return;
        }

        if (image is ImageNode node)
        {
            for (int i = 0; i < node.Count; i++)
            {
                Visit(node[i], action);
            }

            return;
        }
        action(image);
    }

    // The recorded FNV mixes pixels with the CLR simple name from before this rename.
    // These three classes changed only in case. The hash keeps that spelling.
    private static string RecordedTypeName(Type type) => type.Name switch
    {
        "RleSprite08" => "RLESprite08",
        "RleSprite16" => "RLESprite16",
        "TransparentRleSprite16" => "TransparentRLESprite16",
        _ => type.Name,
    };

    // The decoded buffer, its pitch and size, then the draws in a fixed order. A draw into the
    // other 16 bit format remaps the image in place, so the buffer is hashed again at the end.
    private static void Pin(LibraryImage image, Fnv hash)
    {
        hash.Add(RecordedTypeName(image.GetType()));
        hash.Add(image.DataLoaded ? 1 : 0);
        foreach (int value in new[] { image.DataWidth, image.DataHeight, image.Width, image.Height, image.Xshift, image.Yshift, image.DataSize })
        {
            hash.Add(value);
        }

        AddBuffer(image, hash);
        int w = image.Width, h = image.Height;
        Rect stretched = new(1, 1, 1 + Math.Max(1, w * 3 / 2), 1 + Math.Max(1, h * 2 / 3));
        Rect shrunk = new(-w / 4, 2, (-w / 4) + Math.Max(1, w / 2), 2 + Math.Max(1, h * 5 / 4));
        if (image is Image08)
        {
            hash.Add(Draw(image, PixelFormat.Pal8, 3, 2, d => image.ShowOpaque(3, 2, d)));
            hash.Add(Draw(image, PixelFormat.Pal8, -w / 3, -h / 3, d => image.ShowOpaque(-w / 3, -h / 3, d)));
        }

        foreach (PixelFormat format in new[] { PixelFormat.Rgb555, PixelFormat.Rgb565 })
        {
            hash.Add(Draw(image, format, 3, 2, d => image.ShowOpaque(3, 2, d)));
            hash.Add(Draw(image, format, -w / 3, -h / 3, d => image.ShowOpaque(-w / 3, -h / 3, d)));
            hash.Add(Draw(image, format, 3, 2, d => image.ShowTransparent(3, 2, d)));
            hash.Add(Draw(image, format, 3, 2, d => image.ShowBlended(3, 2, d, GraphicsState.AlphaBlendTable.Lookup[50])));
            hash.Add(Draw(image, format, -w / 3, -h / 3, d => image.ShowBlended(-w / 3, -h / 3, d, GraphicsState.IntensityBlendTable.Lookup[30])));
            hash.Add(Draw(image, format, 0, 0, d => image.ShowOpaqueStretched(stretched, d)));
            hash.Add(Draw(image, format, 0, 0, d => image.ShowOpaqueStretched(shrunk, d)));
            hash.Add(Draw(image, format, 0, 0, d => image.ShowBlendedStretched(stretched, d, GraphicsState.AlphaBlendTable.Lookup[50])));
            hash.Add(Draw(image, format, 0, 0, d => image.ShowBlendedStretched(shrunk, d, GraphicsState.ShadowBlendTable.Lookup[70])));
        }
        AddBuffer(image, hash);
    }

    // The image's decoded buffer as the class holds it, a byte array.
    private static void AddBuffer(LibraryImage image, Fnv hash)
    {
        Type type = image.GetType();
        const BindingFlags fields = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        FieldInfo? field = new[] { "_buffer", "MaskData", "_data", "Data" }.Select(name => MemberNames.Field(type, name, fields)).FirstOrDefault(f => f is not null);
        FieldInfo? pitch = MemberNames.Field(type, "Pitch", fields) ?? MemberNames.Field(type, "_pitch", fields);
        hash.Add(pitch is null ? -1 : (int)pitch.GetValue(image)!);
        byte[]? bytes = field?.GetValue(image) switch
        {
            byte[] array => array,
            null => null,
            var other => throw new InvalidOperationException($"{type.Name}.{field!.Name} is a {other.GetType().Name}"),
        };
        hash.Add(bytes is null ? -1 : bytes.Length);
        if (bytes is not null)
        {
            hash.Add(bytes);
        }
    }

    // A surface two pixels wider and taller on each side than the image, filled with a pattern,
    // clipped one pixel in from its edges, after the draw: the hash of its pixels, or the name of
    // the exception the draw raised.
    private static ulong Draw(LibraryImage image, PixelFormat format, int x, int y, Action<LockedSurface> draw)
    {
        int w = Math.Max(1, image.Width + 6), h = Math.Max(1, image.Height + 4);
        DataSurface surface = new(palette: new Palette(), width: w, height: h, pixelFormatId: format);
        Fnv hash = new();
        try
        {
            LockedSurface lockedSurface = default;
            Assert.Equal(GraphicsResult.Ok, surface.Lock(ref lockedSurface));
            Span<byte> pixels = lockedSurface.Data.AsSpan(lockedSurface.DataIndex, lockedSurface.Pitch * h);
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = (byte)((i * 37) + (x * 5) + y);
            }

            lockedSurface.ClipRectangle = new Rect(1, 1, w - 1, h - 1);
            try { draw(lockedSurface); }
            catch (Exception error) { hash.Add(error.GetType().Name); }
            hash.Add(pixels);
            _ = surface.Unlock();
        }
        finally
        {
            surface.Dispose();
        }
        return hash.Value;
    }

    // The classes registered and the blend tables built, as at game start.
    private static void WithImageClasses(Action body)
    {
        List<LibraryImageClass> previousClasses = ImageLibraries.LibraryImageClasses;
        // Engine startup sets the base to the startup directory, so a library stores its name
        // relative to it and loads its on-demand images from there.
        string previousBase = ImageLibraries.BaseImageLibraryPath;
        ImageLibraries.BaseImageLibraryPath = InstalledFontsFactAttribute.Root + Path.DirectorySeparatorChar;
        (LookupBlendTableCollection100 AlphaBlendTable, LookupBlendTableCollection100 IntensityBlendTable, LookupBlendTableCollection100 BrightenBlendTable, LookupBlendTableCollection100 ShadowBlendTable, LookupBlendTableCollection100 LinearAlphaBlendTable) previousTables = (GraphicsState.AlphaBlendTable, GraphicsState.IntensityBlendTable, GraphicsState.BrightenBlendTable, GraphicsState.ShadowBlendTable, GraphicsState.LinearAlphaBlendTable);
        (GraphicsState.AlphaBlendTable, GraphicsState.IntensityBlendTable, GraphicsState.BrightenBlendTable, GraphicsState.ShadowBlendTable, GraphicsState.LinearAlphaBlendTable) =
            (new(), new(), new(), new(), new());
        ImageLibraries.LibraryImageClasses = [];
        try
        {
            GraphicsState.InitializeBlendTables(5, 31);
            ImageLibraries.RegisterLibraryImageClasses([
                LibraryImageClass.Of<Picture08>(), LibraryImageClass.Of<RleSprite08>(),
                LibraryImageClass.Of<Picture16>(), LibraryImageClass.Of<TransparentPicture16>(),
                LibraryImageClass.Of<RleSprite16>(), LibraryImageClass.Of<TransparentRleSprite16>(),
                LibraryImageClass.Of<Shadow>(), LibraryImageClass.Of<ImageNode>(), LibraryImageClass.Of<Sprite16>(),
                LibraryImageClass.Of<BitMask>()]);
            body();
        }
        finally
        {
            ImageLibraries.LibraryImageClasses.Clear();
            ImageLibraries.LibraryImageClasses = previousClasses;
            ImageLibraries.BaseImageLibraryPath = previousBase;
            GraphicsState.AlphaBlendTable?.Dispose();
            GraphicsState.IntensityBlendTable?.Dispose();
            GraphicsState.BrightenBlendTable?.Dispose();
            GraphicsState.ShadowBlendTable?.Dispose();
            GraphicsState.LinearAlphaBlendTable?.Dispose();
            (GraphicsState.AlphaBlendTable, GraphicsState.IntensityBlendTable, GraphicsState.BrightenBlendTable, GraphicsState.ShadowBlendTable, GraphicsState.LinearAlphaBlendTable) = previousTables;
        }
    }

    // FNV-1a over 64 bits.
    private sealed class Fnv
    {
        public ulong Value { get; private set; } = 14695981039346656037ul;

        public void Add(ReadOnlySpan<byte> bytes)
        {
            ulong h = Value;
            foreach (byte b in bytes)
            {
                h = (h ^ b) * 1099511628211ul;
            }

            Value = h;
        }

        public void Add(int value) => Add(BitConverter.GetBytes(value));

        public void Add(ulong value) => Add(BitConverter.GetBytes(value));

        public void Add(string value) => Add(Cp1252.Encode(value));
    }
}
