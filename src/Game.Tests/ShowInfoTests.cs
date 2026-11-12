using System.Buffers.Binary;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Xunit.Abstractions;
using LibraryImage = Aow.Game.Draw.Images.LibraryImage;

namespace Aow.Game.Tests;

// The blend slot of TShowInfo (unions-and-kinds.md 3.3): a percentage or a user blend table, the
// 8-byte file image, and the arms of Show, ShowStretched and ShowEx that read it.
public sealed class ShowInfoTests(ITestOutputHelper output)
{
    static ShowInfoTests() => LibraryImageClass.Register<BlendRecorder>();

    private static readonly Rect s_square = new() { Right = 4, Bottom = 4 };

    [Fact]
    public void BlendValueHoldsOnePercentageOrOneTable()
    {
        BlendValue empty = default;
        Assert.False(empty.HasValue);
        Assert.Null(empty.Value);
        Assert.Equal(0, empty.Percentage);
        Assert.False(empty is BlendTable);
        Assert.False(empty is int);

        BlendValue percentage = 37;
        Assert.True(percentage.HasValue);
        Assert.Equal(37, percentage.Percentage);
        Assert.True(percentage is int p && p == 37);
        Assert.False(percentage is BlendTable);
        Assert.Equal(37, percentage.Value);

        byte small = 9;
        BlendValue widened = small;
        Assert.Equal(9, widened.Percentage);

        BlendValue zero = 0;
        Assert.True(zero.HasValue);
        Assert.Equal(0, zero.Percentage);

        var table = new BlendTable5Bit();
        {
            BlendValue user = table;
            Assert.True(user.HasValue);
            Assert.True(user is BlendTable t && ReferenceEquals(t, table));
            Assert.False(user is int);
            Assert.Same(table, user.Value);
            Assert.Throws<InvalidCastException>(() => user.Percentage);

            BlendValue nil = (BlendTable)null!;
            Assert.False(nil.HasValue);
            Assert.Equal(0, nil.Percentage);
        }
    }

    private static byte[] Written(ShowInfo info)
    {
        using var stream = new MemoryStream();
        Assert.Equal(8, info.WriteImage(stream, true));
        return stream.ToArray();
    }

    private static ShowInfo Read(byte[] image)
    {
        using var stream = new MemoryStream();
        stream.Write(image);
        stream.Position = 0;
        ShowInfo info = default;
        Assert.Equal(8, info.ReadImage(stream, true));
        return info;
    }

    [Fact]
    public void TheFileImageKeepsModesPaddingAndPercentage()
    {
        byte[] image = [(byte)ShowMode.Blended, (byte)BlendMode.Shadow, 0x5A, 0xA5, 0x78, 0x56, 0x34, 0x12];
        var info = Read(image);
        Assert.Equal(ShowMode.Blended, info.ShowMode);
        Assert.Equal(BlendMode.Shadow, info.BlendMode);
        Assert.True(info.Blend is int percentage && percentage == 0x12345678);
        Assert.Equal(image, Written(info));

        Assert.Equal(new byte[8], Written(default));
        Assert.True(Read(new byte[8]).Blend is int read && read == 0);

        var table = new BlendTable5Bit();
        {
            // A table writes 0 where the original wrote its address, and reads back as the percentage 0.
            var user = new ShowInfo { ShowMode = ShowMode.Blended, BlendMode = BlendMode.User, Blend = table };
            byte[] written = Written(user);
            Assert.Equal(new byte[] { 2, 0, 0, 0, 0, 0, 0, 0 }, written);
            var back = Read(written);
            Assert.Equal(BlendMode.User, back.BlendMode);
            Assert.False(back.Blend is BlendTable);
            Assert.Equal(0, back.Blend.Percentage);
        }
    }

    private sealed class BlendRecorder : LibraryImage, ILibraryImageClass
    {
        static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new BlendRecorder(owner);

        public readonly List<BlendTable> Drawn = [];

        internal BlendRecorder(CustomImageLibrary owner) : base(owner) { }

        public override bool SupportShowMode(ShowMode showMode, int pixelFormatId) => true;
        public override void ShowBlended(int X, int Y, in LockedSurface lockedSurface, BlendTable table) => Drawn.Add(table);
        public override void ShowBlendedStretched(in Rect R, in LockedSurface lockedSurface, BlendTable table) => Drawn.Add(table);
    }

    // Writes the image header with the given show info and reads it into a fresh image, the path
    // of an image in a library file (TLibraryImage.ReadImageData).
    private static BlendRecorder FromFile(ShowInfo info)
    {
        using var writer = new BlendRecorder(null!);
        using var stream = new MemoryStream();
        writer.ShowInfo = info;
        writer.WriteImageData(stream);
        stream.Position = 0;
        var reader = new BlendRecorder(null!);
        reader.ReadImageData(stream);
        Assert.Equal(stream.Length, stream.Position);
        Assert.Equal(3, reader._version);
        return reader;
    }

    [Fact]
    public void AUserImageReadFromAFileCarriesAnIntegerAndDrawsNothing()
    {
        var image = FromFile(new ShowInfo { ShowMode = ShowMode.Blended, BlendMode = BlendMode.User, Blend = 1234 });
        var table = new BlendTable5Bit();
        try
        {
            Assert.Equal(BlendMode.User, image.ShowInfo.BlendMode);
            Assert.True(image.ShowInfo.Blend is int value && value == 1234);
            LockedSurface lockedSurface = default;
            image.Show(0, 0, lockedSurface);
            image.ShowStretched(s_square, lockedSurface);
            image.Show(0, 0, lockedSurface, new ShowInfo { ShowMode = ShowMode.Blended, BlendMode = BlendMode.User, Blend = 50 });
            image.Show(0, 0, lockedSurface, new ShowInfo { ShowMode = ShowMode.Blended, BlendMode = BlendMode.Alpha, Blend = 50 });
            Assert.Empty(image.Drawn);

            // ShowEx reads the image's own blend mode with the caller's blend value
            // (transpiler.md 4.3): a caller's table draws through the image's bmUser arm.
            image.Show(0, 0, lockedSurface, new ShowInfo { ShowMode = ShowMode.Blended, BlendMode = BlendMode.Alpha, Blend = table });
            Assert.Equal(new BlendTable[] { table }, image.Drawn);

            // The image's own table, set in memory, draws through Show and ShowStretched.
            image.Drawn.Clear();
            image.ShowInfo = new ShowInfo { ShowMode = ShowMode.Blended, BlendMode = BlendMode.User, Blend = table };
            image.Show(0, 0, lockedSurface);
            image.ShowStretched(s_square, lockedSurface);
            Assert.Equal(new BlendTable[] { table, table }, image.Drawn);
        }
        finally
        {
            image.Dispose();
        }
    }

    [Fact]
    public void ThePercentageArmsDrawTheTableOfTheirCollection() => WithBlendTables(() =>
    {
        var table = new BlendTable5Bit();
        {
            var collections = new (BlendMode Mode, LookupBlendTableCollection100 Tables)[]
            {
                (BlendMode.Alpha, GraphicsState.AlphaBlendTable), (BlendMode.Brighten, GraphicsState.BrightenBlendTable),
                (BlendMode.Intensity, GraphicsState.IntensityBlendTable), (BlendMode.Shadow, GraphicsState.ShadowBlendTable),
                (BlendMode.LinearAlpha, GraphicsState.LinearAlphaBlendTable),
            };
            LockedSurface lockedSurface = default;
            foreach (var (mode, tables) in collections)
            {
                var image = FromFile(new ShowInfo { ShowMode = ShowMode.Blended, BlendMode = mode, Blend = 30 });
                try
                {
                    image.Show(0, 0, lockedSurface);
                    image.ShowStretched(s_square, lockedSurface);
                    var expected = new List<BlendTable> { tables.Lookup[30], tables.Lookup[30] };
                    // ShowEx has no bmLinearAlpha arm in the original.
                    image.Show(0, 0, lockedSurface, new ShowInfo { ShowMode = ShowMode.Blended, Blend = 70 });
                    if (mode != BlendMode.LinearAlpha) expected.Add(tables.Lookup[70]);
                    Assert.Equal(expected, image.Drawn);
                    Assert.DoesNotContain(null, image.Drawn);

                    // A table in a percentage arm has no percentage to read.
                    if (mode != BlendMode.LinearAlpha)
                        Assert.Throws<InvalidCastException>(() => image.Show(0, 0, lockedSurface, new ShowInfo { ShowMode = ShowMode.Blended, Blend = table }));
                    image.ShowInfo = new ShowInfo { ShowMode = ShowMode.Blended, BlendMode = mode, Blend = table };
                    Assert.Throws<InvalidCastException>(() => image.Show(0, 0, lockedSurface));
                }
                finally { image.Dispose(); }
            }
        }
    });

    // The union is a readonly struct: drawing through a percentage arm allocates nothing.
    [Fact]
    public void DrawingThroughTheBlendSlotAllocatesNothing() => WithBlendTables(() =>
    {
        using var image = new LibraryImage(null!);
        image._showInfo = new ShowInfo { ShowMode = ShowMode.Blended, BlendMode = BlendMode.Shadow, Blend = 40 };
        var caller = new ShowInfo { ShowMode = ShowMode.Blended, Blend = 60 };
        LockedSurface lockedSurface = default;
        image.Show(0, 0, lockedSurface);
        image.Show(0, 0, lockedSurface, caller);
        long before = GC.GetAllocatedBytesForCurrentThread();
        for (int i = 0; i < 1000; i++)
        {
            image.Show(0, 0, lockedSurface);
            image.ShowStretched(s_square, lockedSurface);
            image.Show(0, 0, lockedSurface, caller);
        }
        Assert.Equal(0, GC.GetAllocatedBytesForCurrentThread() - before);
    });

    // Shown images draw through the blend tables that GFXE.Initialize builds at game start.
    private static void WithBlendTables(Action body)
    {
        var previous = (GraphicsState.AlphaBlendTable, GraphicsState.IntensityBlendTable, GraphicsState.BrightenBlendTable, GraphicsState.ShadowBlendTable, GraphicsState.LinearAlphaBlendTable);
        (GraphicsState.AlphaBlendTable, GraphicsState.IntensityBlendTable, GraphicsState.BrightenBlendTable, GraphicsState.ShadowBlendTable, GraphicsState.LinearAlphaBlendTable) =
            (new(), new(), new(), new(), new());
        try
        {
            GraphicsState.InitializeBlendTables(5, 31);
            body();
        }
        finally
        {
            GraphicsState.AlphaBlendTable?.Dispose();
            GraphicsState.IntensityBlendTable?.Dispose();
            GraphicsState.BrightenBlendTable?.Dispose();
            GraphicsState.ShadowBlendTable?.Dispose();
            GraphicsState.LinearAlphaBlendTable?.Dispose();
            (GraphicsState.AlphaBlendTable, GraphicsState.IntensityBlendTable, GraphicsState.BrightenBlendTable, GraphicsState.ShadowBlendTable, GraphicsState.LinearAlphaBlendTable) = previous;
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
        var previousClasses = ImageLibraries.LibraryImageClasses;
        ImageLibraries.LibraryImageClasses = new List<LibraryImageClass>();
        var tally = new Tally();
        try
        {
            ImageLibraries.RegisterLibraryImageClasses([LibraryImageClass.Of<Picture08>(), LibraryImageClass.Of<RleSprite08>()]);
            ImageLibraries.RegisterLibraryImageClasses([
                LibraryImageClass.Of<Picture16>(), LibraryImageClass.Of<TransparentPicture16>(),
                LibraryImageClass.Of<RleSprite16>(), LibraryImageClass.Of<TransparentRleSprite16>(),
                LibraryImageClass.Of<Shadow>(), LibraryImageClass.Of<ImageNode>(), LibraryImageClass.Of<Sprite16>()]);
            foreach (string file in files) CheckLibrary(file, tally);
        }
        finally
        {
            ImageLibraries.LibraryImageClasses.Clear();
            ImageLibraries.LibraryImageClasses = previousClasses;
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
        using var library = new ImageLibrary(null!);
        library.LoadFromFile(file);
        tally.Libraries++;
        int from = 0;
        for (int i = 0; i < library._count; i++)
            if (library._images[i].Image is { } image) CheckImage(file, bytes, image, ref from, tally);
    }

    private static void CheckImage(string file, byte[] bytes, LibraryImage image, ref int from, Tally tally)
    {
        if (image is ImageNode node)
        {
            for (int i = 0; i < node._count; i++) CheckImage(file, bytes, node._images[i], ref from, tally);
            return;
        }

        tally.Images++;
        if (image._version < 3)
        {
            tally.WithoutShowInfo++;
            return;
        }

        var info = image._showInfo;
        Assert.True(info.Blend is int, $"{file}: a show info read from a file holds a percentage");
        tally.Modes.TryGetValue($"{info.ShowMode} {info.BlendMode}", out int seen);
        tally.Modes[$"{info.ShowMode} {info.BlendMode}"] = seen + 1;
        if (info.BlendMode == BlendMode.User) tally.User++;
        if (info.ShowMode == ShowMode.Blended) tally.Blended++;

        byte[] header = Header(image);
        int at = bytes.AsSpan(from).IndexOf(header);
        if (at >= 0) at += from;
        else at = bytes.AsSpan().IndexOf(header);
        Assert.True(at >= 0, $"{file}: the header of {image._fileName} at data position {image._dataPosition} is not in the file");
        int record = at + header.Length;
        Assert.Equal(bytes.AsSpan(record, 8).ToArray(), Written(info));
        from = record + 8;
    }

    // The fields of a version 3 header between the version byte and the show info record
    // (TLibraryImage.ReadImageData).
    private static byte[] Header(LibraryImage image)
    {
        byte[] name = Cp1252.Encode(image._fileName);
        byte[] header = new byte[4 + name.Length + 37];
        var span = header.AsSpan();
        BinaryPrimitives.WriteInt32LittleEndian(span, name.Length);
        name.CopyTo(span[4..]);
        span = span[(4 + name.Length)..];
        foreach (int value in new[] { image._dataWidth, image._dataHeight, image._xshift, image._yshift, image._fileIndex })
        {
            BinaryPrimitives.WriteInt32LittleEndian(span, value);
            span = span[4..];
        }
        span[0] = (byte)image._loadMode;
        span = span[1..];
        foreach (int value in new[] { image._dataSize, image._dataPosition, image._width, image._height })
        {
            BinaryPrimitives.WriteInt32LittleEndian(span, value);
            span = span[4..];
        }
        return header;
    }
}
