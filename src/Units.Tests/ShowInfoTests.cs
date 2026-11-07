using System.Buffers.Binary;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Xunit.Abstractions;
using G = Aow.Units.GFXE.GFXEUnit;
using Images = Aow.Units.ImageLib.ImageLibUnit;
using LibraryImage = Aow.Units.ImageLib.TLibraryImage;

namespace Aow.Units.Tests;

/// <summary>Skips with the resolved path when the installed image libraries are absent.</summary>
public sealed class InstalledLibrariesFactAttribute : FactAttribute
{
    internal static string[] Folders => [Path.Combine(InstalledFontsFactAttribute.Root, "Images"), Path.Combine(InstalledFontsFactAttribute.Root, "Int")];

    public InstalledLibrariesFactAttribute()
    {
        if (Folders.Any(folder => !Directory.Exists(folder) || !Directory.EnumerateFiles(folder, "*.ILB", SearchOption.AllDirectories).Any()))
            Skip = $"Installed image libraries are absent: {string.Join(", ", Folders)}. Set AOW_GAME_ROOT to the game installation.";
    }
}

// The blend slot of TShowInfo (unions-and-kinds.md 3.3): a percentage or a user blend table, the
// 8-byte file image, and the arms of Show, ShowStretched and ShowEx that read it.
public sealed class ShowInfoTests(ITestOutputHelper output)
{
    static ShowInfoTests() => LibraryImageClass.Register<BlendRecorder>();

    private static readonly TRect Square = new() { Right = 4, Bottom = 4 };

    [Fact]
    public void BlendValueHoldsOnePercentageOrOneTable()
    {
        BlendValue empty = default;
        Assert.False(empty.HasValue);
        Assert.Null(empty.Value);
        Assert.Equal(0, empty.Percentage);
        Assert.False(empty is TBlendTable);
        Assert.False(empty is int);

        BlendValue percentage = 37;
        Assert.True(percentage.HasValue);
        Assert.Equal(37, percentage.Percentage);
        Assert.True(percentage is int p && p == 37);
        Assert.False(percentage is TBlendTable);
        Assert.Equal(37, percentage.Value);

        byte small = 9;
        BlendValue widened = small;
        Assert.Equal(9, widened.Percentage);

        BlendValue zero = 0;
        Assert.True(zero.HasValue);
        Assert.Equal(0, zero.Percentage);

        var table = new TBlendTable5Bit();
        try
        {
            BlendValue user = table;
            Assert.True(user.HasValue);
            Assert.True(user is TBlendTable t && ReferenceEquals(t, table));
            Assert.False(user is int);
            Assert.Same(table, user.Value);
            Assert.Throws<EInvalidCast>(() => user.Percentage);

            BlendValue nil = (TBlendTable)null!;
            Assert.False(nil.HasValue);
            Assert.Equal(0, nil.Percentage);
        }
        finally { table.Destroy(); }
    }

    private static byte[] Written(TShowInfo info)
    {
        var stream = new TMemoryStream();
        try
        {
            Assert.Equal(8, info.WriteImage(stream, true));
            return stream.Memory.AsSpan(0, stream.Size).ToArray();
        }
        finally { stream.Destroy(); }
    }

    private static TShowInfo Read(byte[] image)
    {
        var stream = new TMemoryStream();
        try
        {
            stream.Write(image);
            stream.Position = 0;
            TShowInfo info = default;
            Assert.Equal(8, info.ReadImage(stream, true));
            return info;
        }
        finally { stream.Destroy(); }
    }

    [Fact]
    public void TheFileImageKeepsModesPaddingAndPercentage()
    {
        byte[] image = [(byte)TShowMode.smBlended, (byte)TBlendMode.bmShadow, 0x5A, 0xA5, 0x78, 0x56, 0x34, 0x12];
        var info = Read(image);
        Assert.Equal(TShowMode.smBlended, info.ShowMode);
        Assert.Equal(TBlendMode.bmShadow, info.BlendMode);
        Assert.True(info.Blend is int percentage && percentage == 0x12345678);
        Assert.Equal(image, Written(info));

        Assert.Equal(new byte[8], Written(default));
        Assert.True(Read(new byte[8]).Blend is int read && read == 0);

        var table = new TBlendTable5Bit();
        try
        {
            // A table writes 0 where the original wrote its address, and reads back as the percentage 0.
            var user = new TShowInfo { ShowMode = TShowMode.smBlended, BlendMode = TBlendMode.bmUser, Blend = table };
            byte[] written = Written(user);
            Assert.Equal(new byte[] { 2, 0, 0, 0, 0, 0, 0, 0 }, written);
            var back = Read(written);
            Assert.Equal(TBlendMode.bmUser, back.BlendMode);
            Assert.False(back.Blend is TBlendTable);
            Assert.Equal(0, back.Blend.Percentage);
        }
        finally { table.Destroy(); }
    }

    private sealed class BlendRecorder : LibraryImage, ILibraryImageClass
    {
        static LibraryImage ILibraryImageClass.Create(TCustomImageLibrary owner) => new BlendRecorder(owner);

        public readonly List<TBlendTable> Drawn = [];

        internal BlendRecorder(TCustomImageLibrary owner) : base(owner) { }

        public override bool SupportShowMode(TShowMode ShowMode, int PixelFormatID) => true;
        public override void ShowBlended(int X, int Y, in TSurfaceDesc SD, TBlendTable Table) => Drawn.Add(Table);
        public override void ShowBlendedStretched(in TRect R, in TSurfaceDesc SD, TBlendTable Table) => Drawn.Add(Table);
    }

    // Writes the image header with the given show info and reads it into a fresh image, the path
    // of an image in a library file (TLibraryImage.ReadImageData).
    private static BlendRecorder FromFile(TShowInfo info)
    {
        var writer = new BlendRecorder(null!);
        var stream = new TMemoryStream();
        try
        {
            writer.ShowInfo = info;
            writer.WriteImageData(stream);
            stream.Position = 0;
            var reader = new BlendRecorder(null!);
            reader.ReadImageData(stream);
            Assert.Equal(stream.Size, stream.Position);
            Assert.Equal(3, reader.FVersion);
            return reader;
        }
        finally
        {
            stream.Destroy();
            writer.Destroy();
        }
    }

    [Fact]
    public void AUserImageReadFromAFileCarriesAnIntAndDrawsNothing()
    {
        var image = FromFile(new TShowInfo { ShowMode = TShowMode.smBlended, BlendMode = TBlendMode.bmUser, Blend = 1234 });
        var table = new TBlendTable5Bit();
        try
        {
            Assert.Equal(TBlendMode.bmUser, image.ShowInfo.BlendMode);
            Assert.True(image.ShowInfo.Blend is int value && value == 1234);
            TSurfaceDesc sd = default;
            image.Show(0, 0, sd);
            image.ShowStretched(Square, sd);
            image.ShowEx(0, 0, sd, new TShowInfo { ShowMode = TShowMode.smBlended, BlendMode = TBlendMode.bmUser, Blend = 50 });
            image.ShowEx(0, 0, sd, new TShowInfo { ShowMode = TShowMode.smBlended, BlendMode = TBlendMode.bmAlpha, Blend = 50 });
            Assert.Empty(image.Drawn);

            // ShowEx reads the image's own blend mode with the caller's blend value
            // (transpiler.md 4.3): a caller's table draws through the image's bmUser arm.
            image.ShowEx(0, 0, sd, new TShowInfo { ShowMode = TShowMode.smBlended, BlendMode = TBlendMode.bmAlpha, Blend = table });
            Assert.Equal(new TBlendTable[] { table }, image.Drawn);

            // The image's own table, set in memory, draws through Show and ShowStretched.
            image.Drawn.Clear();
            image.ShowInfo = new TShowInfo { ShowMode = TShowMode.smBlended, BlendMode = TBlendMode.bmUser, Blend = table };
            image.Show(0, 0, sd);
            image.ShowStretched(Square, sd);
            Assert.Equal(new TBlendTable[] { table, table }, image.Drawn);
        }
        finally
        {
            table.Destroy();
            image.Destroy();
        }
    }

    [Fact]
    public void ThePercentageArmsDrawTheTableOfTheirCollection() => WithBlendTables(() =>
    {
        var table = new TBlendTable5Bit();
        try
        {
            var collections = new (TBlendMode Mode, TLookupBlendTableCollection100 Tables)[]
            {
                (TBlendMode.bmAlpha, G.AlphaBlendTable), (TBlendMode.bmBrighten, G.BrightenBlendTable),
                (TBlendMode.bmIntensity, G.IntensityBlendTable), (TBlendMode.bmShadow, G.ShadowBlendTable),
                (TBlendMode.bmLinearAlpha, G.LinearAlphaBlendTable),
            };
            TSurfaceDesc sd = default;
            foreach (var (mode, tables) in collections)
            {
                var image = FromFile(new TShowInfo { ShowMode = TShowMode.smBlended, BlendMode = mode, Blend = 30 });
                try
                {
                    image.Show(0, 0, sd);
                    image.ShowStretched(Square, sd);
                    var expected = new List<TBlendTable> { tables.Lookup[30], tables.Lookup[30] };
                    // ShowEx has no bmLinearAlpha arm in the original.
                    image.ShowEx(0, 0, sd, new TShowInfo { ShowMode = TShowMode.smBlended, Blend = 70 });
                    if (mode != TBlendMode.bmLinearAlpha) expected.Add(tables.Lookup[70]);
                    Assert.Equal(expected, image.Drawn);
                    Assert.DoesNotContain(null, image.Drawn);

                    // A table in a percentage arm has no percentage to read.
                    if (mode != TBlendMode.bmLinearAlpha)
                        Assert.Throws<EInvalidCast>(() => image.ShowEx(0, 0, sd, new TShowInfo { ShowMode = TShowMode.smBlended, Blend = table }));
                    image.ShowInfo = new TShowInfo { ShowMode = TShowMode.smBlended, BlendMode = mode, Blend = table };
                    Assert.Throws<EInvalidCast>(() => image.Show(0, 0, sd));
                }
                finally { image.Destroy(); }
            }
        }
        finally { table.Destroy(); }
    });

    // The union is a readonly struct: drawing through a percentage arm allocates nothing.
    [Fact]
    public void DrawingThroughTheBlendSlotAllocatesNothing() => WithBlendTables(() =>
    {
        var image = new LibraryImage(null!);
        try
        {
            image.FShowInfo = new TShowInfo { ShowMode = TShowMode.smBlended, BlendMode = TBlendMode.bmShadow, Blend = 40 };
            var caller = new TShowInfo { ShowMode = TShowMode.smBlended, Blend = 60 };
            TSurfaceDesc sd = default;
            image.Show(0, 0, sd);
            image.ShowEx(0, 0, sd, caller);
            long before = GC.GetAllocatedBytesForCurrentThread();
            for (int i = 0; i < 1000; i++)
            {
                image.Show(0, 0, sd);
                image.ShowStretched(Square, sd);
                image.ShowEx(0, 0, sd, caller);
            }
            Assert.Equal(0, GC.GetAllocatedBytesForCurrentThread() - before);
        }
        finally { image.Destroy(); }
    });

    // Shown images draw through the blend tables that GFXE.Initialize builds at game start.
    private static void WithBlendTables(Action body)
    {
        var previous = (G.AlphaBlendTable, G.IntensityBlendTable, G.BrightenBlendTable, G.ShadowBlendTable, G.LinearAlphaBlendTable);
        (G.AlphaBlendTable, G.IntensityBlendTable, G.BrightenBlendTable, G.ShadowBlendTable, G.LinearAlphaBlendTable) =
            (new(), new(), new(), new(), new());
        try
        {
            G.InitBlendTables(5, 31);
            body();
        }
        finally
        {
            G.AlphaBlendTable.Free();
            G.IntensityBlendTable.Free();
            G.BrightenBlendTable.Free();
            G.ShadowBlendTable.Free();
            G.LinearAlphaBlendTable.Free();
            (G.AlphaBlendTable, G.IntensityBlendTable, G.BrightenBlendTable, G.ShadowBlendTable, G.LinearAlphaBlendTable) = previous;
        }
    }

    private sealed class Tally
    {
        public int Libraries, Images, WithoutShowInfo, User, Blended;
        public readonly SortedDictionary<string, int> Modes = new(StringComparer.Ordinal);
    }

    // unions-and-kinds.md section 7 risk 5: every image of every installed library writes its
    // 8-byte show info record back as the file holds it. An image header of version 3 and later
    // ends with the record, right after the fields the test locates it by.
    [InstalledLibrariesFact]
    public void EveryInstalledImageWritesItsShowInfoBackAsTheFileHoldsIt()
    {
        string[] files = InstalledLibrariesFactAttribute.Folders
            .SelectMany(folder => Directory.EnumerateFiles(folder, "*.ILB", SearchOption.AllDirectories))
            .Order(StringComparer.OrdinalIgnoreCase).ToArray();
        var previousClasses = Images.LibraryImageClasses;
        Images.LibraryImageClasses = new TList();
        var tally = new Tally();
        try
        {
            Images.RegisterLibraryImageClasses([LibraryImageClass.Of<TPicture08>(), LibraryImageClass.Of<TRLESprite08>()]);
            Images.RegisterLibraryImageClasses([
                LibraryImageClass.Of<TPicture16>(), LibraryImageClass.Of<TTransparentPicture16>(),
                LibraryImageClass.Of<TRLESprite16>(), LibraryImageClass.Of<TTransparentRLESprite16>(),
                LibraryImageClass.Of<TShadow>(), LibraryImageClass.Of<TImageNode>(), LibraryImageClass.Of<TSprite16>()]);
            foreach (string file in files) CheckLibrary(file, tally);
        }
        finally
        {
            Images.LibraryImageClasses.Destroy();
            Images.LibraryImageClasses = previousClasses;
        }

        output.WriteLine($"{tally.Libraries} libraries, {tally.Images} images, {tally.WithoutShowInfo} without a show info record, " +
                         $"{tally.User} bmUser images, {tally.Blended} blended images");
        foreach (var (key, count) in tally.Modes) output.WriteLine($"  {key}: {count}");
        Assert.Equal(files.Length, tally.Libraries);
        Assert.True(tally.Images - tally.WithoutShowInfo > 1000, $"only {tally.Images - tally.WithoutShowInfo} records compared");
        // A bmUser image in a file carries an int in place of a table (spec 3.3), and draws nothing.
        Assert.Equal(0, tally.User);
    }

    private static void CheckLibrary(string file, Tally tally)
    {
        byte[] bytes = File.ReadAllBytes(file);
        var library = new TImageLibrary(null!);
        try
        {
            library.LoadFromFile(file);
            tally.Libraries++;
            int from = 0;
            for (int i = 0; i < library.FCount; i++)
                if (library.FImages[i].Image is { } image) CheckImage(file, bytes, image, ref from, tally);
        }
        finally { library.Destroy(); }
    }

    private static void CheckImage(string file, byte[] bytes, LibraryImage image, ref int from, Tally tally)
    {
        if (image is TImageNode node)
        {
            for (int i = 0; i < node.FCount; i++) CheckImage(file, bytes, node.FImages[i], ref from, tally);
            return;
        }

        tally.Images++;
        if (image.FVersion < 3)
        {
            tally.WithoutShowInfo++;
            return;
        }

        var info = image.FShowInfo;
        Assert.True(info.Blend is int, $"{file}: a show info read from a file holds a percentage");
        tally.Modes.TryGetValue($"{info.ShowMode} {info.BlendMode}", out int seen);
        tally.Modes[$"{info.ShowMode} {info.BlendMode}"] = seen + 1;
        if (info.BlendMode == TBlendMode.bmUser) tally.User++;
        if (info.ShowMode == TShowMode.smBlended) tally.Blended++;

        byte[] header = Header(image);
        int at = bytes.AsSpan(from).IndexOf(header);
        if (at >= 0) at += from;
        else at = bytes.AsSpan().IndexOf(header);
        Assert.True(at >= 0, $"{file}: the header of {image.FFileName} at data position {image.FDataPosition} is not in the file");
        int record = at + header.Length;
        Assert.Equal(bytes.AsSpan(record, 8).ToArray(), Written(info));
        from = record + 8;
    }

    // The fields of a version 3 header between the version byte and the show info record
    // (TLibraryImage.ReadImageData).
    private static byte[] Header(LibraryImage image)
    {
        byte[] name = Cp1252.Encode(image.FFileName);
        byte[] header = new byte[4 + name.Length + 37];
        var span = header.AsSpan();
        BinaryPrimitives.WriteInt32LittleEndian(span, name.Length);
        name.CopyTo(span[4..]);
        span = span[(4 + name.Length)..];
        foreach (int value in new[] { image.FDataWidth, image.FDataHeight, image.FXshift, image.FYshift, image.FFileIndex })
        {
            BinaryPrimitives.WriteInt32LittleEndian(span, value);
            span = span[4..];
        }
        span[0] = (byte)image.FLoadMode;
        span = span[1..];
        foreach (int value in new[] { image.FDataSize, image.FDataPosition, image.FWidth, image.FHeight })
        {
            BinaryPrimitives.WriteInt32LittleEndian(span, value);
            span = span[4..];
        }
        return header;
    }
}
