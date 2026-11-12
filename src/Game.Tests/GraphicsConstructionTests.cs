using System.Reflection;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using LibraryImage = Aow.Game.Draw.Images.LibraryImage;

namespace Aow.Game.Tests;

public sealed class GraphicsConstructionTests
{
    private static object? Field(object value, string name) =>
        (MemberNames.Field(value.GetType(), name) ?? throw new InvalidOperationException(name)).GetValue(value);

    public static IEnumerable<object[]> Images()
    {
        yield return new object[] { typeof(LibraryImage), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<LibraryImage>().CreateInstance(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new LibraryImage(owner)) };
        yield return new object[] { typeof(ImageNode), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<ImageNode>().CreateInstance(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new ImageNode(owner)) };
        yield return new object[] { typeof(Image08), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<Image08>().CreateInstance(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new Image08(owner)) };
        yield return new object[] { typeof(Picture08), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<Picture08>().CreateInstance(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new Picture08(owner)) };
        yield return new object[] { typeof(RleSprite08), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<RleSprite08>().CreateInstance(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new RleSprite08(owner)) };
        yield return new object[] { typeof(Image16), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<Image16>().CreateInstance(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new Image16(owner)) };
        yield return new object[] { typeof(Picture16), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<Picture16>().CreateInstance(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new Picture16(owner)) };
        yield return new object[] { typeof(TransparentPicture16), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TransparentPicture16>().CreateInstance(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new TransparentPicture16(owner)) };
        yield return new object[] { typeof(Sprite16), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<Sprite16>().CreateInstance(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new Sprite16(owner)) };
        yield return new object[] { typeof(RleSprite16), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<RleSprite16>().CreateInstance(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new RleSprite16(owner)) };
        yield return new object[] { typeof(TransparentRleSprite16), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TransparentRleSprite16>().CreateInstance(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new TransparentRleSprite16(owner)) };
        yield return new object[] { typeof(BitMask), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<BitMask>().CreateInstance(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new BitMask(owner)) };
        yield return new object[] { typeof(Shadow), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<Shadow>().CreateInstance(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new Shadow(owner)) };

    }

    [Theory]
    [MemberData(nameof(Images))]
    public void ImagesKeepDefaultsAndOwner(Type type, Func<CustomImageLibrary, LibraryImage> create, Func<CustomImageLibrary, LibraryImage> direct)
    {
        using var owner = new ImageLibrary(null!);
        var descriptor = LibraryImageClass.Of(type);
        Assert.Equal(type, descriptor.Type);
        foreach (var construct in new[] { direct, create })
            foreach (var suppliedOwner in new CustomImageLibrary[] { null!, owner })
            {
                var image = construct(suppliedOwner);
                Assert.Equal(type, image.GetType());
                Assert.Same(descriptor, image.ImageClass);
                Assert.Same(suppliedOwner, image.Owner);
                Assert.Equal(-1, image.DataPosition);
                Assert.False(image.ReloadingEnabled);
                Assert.Equal(LoadMode.LoadOnDemand, image.LoadMode);
                Assert.Equal(BlendMode.Alpha, image.ShowInfo.BlendMode);
                Assert.False(image.ShowInfo.Blend.HasValue);
                Assert.Equal(0, image.ShowInfo.Blend.Percentage);
                Assert.Equal(type.Name.StartsWith("Transparent", StringComparison.Ordinal) ? ShowMode.Transparent : ShowMode.Opaque, image.ShowInfo.ShowMode);
                Assert.Equal(0, image.Width);
                Assert.Equal(0, image.Height);
                if (image is ImageNode) Assert.Equal("(multi images)", image.FileName);
                Assert.Equal(0, owner.Count);
                Assert.Empty((System.Collections.IEnumerable)Field(Field(owner, "_paletteManager")!, "_palettes")!);
                image.Dispose();
            }
    }

    public static IEnumerable<object[]> Components()
    {
        yield return new object[] { typeof(SdlDisplayController), (Func<Component, Component>)(owner => new SdlDisplayController(new NullDisplayHost(), owner)) };
        yield return new object[] { typeof(Display), (Func<Component, Component>)(owner => new Display(owner)) };
        yield return new object[] { typeof(CustomDisplay), (Func<Component, Component>)(owner => new CustomDisplay(owner)) };
        yield return new object[] { typeof(DisplayController), (Func<Component, Component>)(owner => new DisplayController(owner)) };
        yield return new object[] { typeof(CustomImageLibrary), (Func<Component, Component>)(owner => new CustomImageLibrary(owner)) };
        yield return new object[] { typeof(ImageLibrary), (Func<Component, Component>)(owner => new ImageLibrary(owner)) };
        yield return new object[] { typeof(ImageLibraryFont), (Func<Component, Component>)(owner => new ImageLibraryFont(owner)) };
        yield return new object[] { typeof(ImageLibraryManager), (Func<Component, Component>)(owner => new ImageLibraryManager(owner)) };

    }

    [Theory]
    [MemberData(nameof(Components))]
    public void ComponentsKeepDefaultsAndOwnership(Type type, Func<Component, Component> create)
    {
        var owner = new Component(null!);
        var value = create(owner);
        Assert.Equal(type, value.GetType());
        Assert.Same(owner, value.Owner);
        Assert.Equal(1, owner.ComponentCount);
        if (value is CustomDisplay display)
        {
            Assert.Equal(100, display.Width);
            Assert.Equal(100, display.Height);
            Assert.Equal(640, Field(display, "_backSurfaceWidth"));
            Assert.Equal(480, Field(display, "_backSurfaceHeight"));
            Assert.Equal(30, Field(display, "_frameRate"));
            Assert.Equal(BufferingScheme.Double, Field(display, "_bufferingScheme"));
        }
        if (value is DisplayController displayController)
        {
            Assert.NotNull(displayController.GetInverseMatrix());
            Assert.Equal((byte)4, Field(displayController.GetInverseMatrix(), "_width"));
            Assert.Equal((byte)4, Field(displayController.GetInverseMatrix(), "_height"));
        }
        if (value is CustomImageLibrary library)
        {
            Assert.Equal(0, library.Count);
            Assert.Equal(true, Field(library, "_loadOnDemand"));
            Assert.NotNull(Field(library, "_paletteManager"));
        }
        if (value is ImageLibraryFont)
        {
            Assert.Equal(2, Field(value, "_characterDistance"));
            Assert.Equal(-1, Field(value, "_fontColorPaletteIndex"));
        }
        if (value is ImageLibraryManager)
        {
            Assert.NotNull(Field(value, "_diskList"));
            Assert.NotSame(Field(value, "_diskList"), Field(value, "_ownedList"));
        }
        value.Dispose();
        Assert.Equal(0, owner.ComponentCount);
        owner.Dispose();
    }

    [Theory]
    [MemberData(nameof(Components))]
    public void PlainComponentsStayPublishedUntilTeardown(Type type, Func<Component, Component> create)
    {
        using var owner = new Component(null!);
        var value = create(owner);
        Assert.IsType(type, value);
        Assert.Same(owner, value.Owner);
        Assert.Equal(1, owner.ComponentCount);
        Assert.Same(value, owner.Components[0]);
        value.Dispose();
        Assert.Equal(0, owner.ComponentCount);
    }

    [Fact]
    public void PrivateHelpersKeepInitialState()
    {
        var palette = new LibraryImagePalette();
        Assert.NotNull(Field(palette, "_palette"));
        var manager = new PaletteManager();
        Assert.NotNull(Field(manager, "_palettes"));
        var imageLibraryManager = new ImageLibraryManager(null!);
        var list = new ImageLibraryManagerList(imageLibraryManager);
        Assert.Same(imageLibraryManager, Field(list, "_owner"));
        Assert.NotNull(Field(list, "HashTable"));
        var sequences = new ImageSequenceList(imageLibraryManager, null!);
        Assert.Same(imageLibraryManager, Field(sequences, "_imageLibraryManager"));
        Assert.Equal(0, sequences.Count);
        var sequence = new ImageSequence(sequences);
        Assert.Same(sequences, Field(sequence, "_owner"));
        var matrix = new Matrix(3, 2);
        Assert.Equal((byte)0, matrix.GetValue(2, 1));
        var modes = new DisplayModeList();
        Assert.NotNull(Field(modes, "_list"));
        var blend = new BlendTable5Bit();
        Assert.Equal(5, blend.Bits);
        var blends = new BlendTableList();
        Assert.NotNull(Field(blends, "_list"));
        var lookup = new LookupBlendTableCollection100();
        Assert.NotNull(Field(lookup, "_tables"));
        foreach (var value in new object[] { palette, manager, list, sequence, sequences, imageLibraryManager, matrix, modes, blend, blends, lookup }) ((value ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
    }

    public static IEnumerable<object[]> Surfaces()
    {
        yield return new object[] { (Func<Palette, Surface>)(palette => new DataSurface(7, 4, PixelFormats.pfPAL8, palette)), PixelFormats.pfPAL8, 8 };
        yield return new object[] { (Func<Palette, Surface>)(_ => new Dib16Surface(7, 4, PixelFormats.pfRGB555)), PixelFormats.pfRGB555, 16 };
    }

    [Theory]
    [MemberData(nameof(Surfaces))]
    public void SurfacesKeepDimensionsFormatAndPaletteIsolation(Func<Palette, Surface> create, int format, int pitch)
    {
        var palette = new Palette();
        palette.Entries[0].Red = 29;
        var surface = create(palette);
        LockedSurface lockedSurface = default;
        Assert.Equal(PixelFormats.GFXE_OK, surface.Lock(ref lockedSurface));
        Assert.Equal(7, lockedSurface.Width);
        Assert.Equal(4, lockedSurface.Height);
        Assert.Equal(format, lockedSurface.PixelFormatId);
        Assert.Equal(pitch, lockedSurface.Pitch);
        Assert.NotNull(lockedSurface.Data);
        if (lockedSurface.Palette is { } copy)
        {
            Assert.NotSame(palette, copy);
            Assert.Equal((byte)29, copy.Entries[0].Red);
            copy.Entries[0].Red = 63;
        }
        surface.Unlock();
        surface.Dispose();
        Assert.Equal((byte)29, palette.Entries[0].Red);
    }

    [Fact]
    public void PlainDataSurfaceCopiesPaletteAndAllocatesItsBuffer()
    {
        var palette = new Palette();
        palette.Entries[0].Red = 29;
        var surface = new DataSurface(7, 4, PixelFormats.pfPAL8, palette);
        Assert.NotSame(palette, surface.Palette);
        Assert.Equal((byte)29, surface.Palette.Entries[0].Red);
        Assert.Equal(7, surface.Width);
        Assert.Equal(4, surface.Height);
        LockedSurface descriptor = default;
        Assert.Equal(0, surface.Lock(ref descriptor));
        Assert.NotNull(descriptor.Data);
        surface.Unlock();
        surface.Dispose();
        Assert.Equal((byte)29, palette.Entries[0].Red);
    }

    [Fact]
    public void PlainDisplayInitializesFrameRateAndOwner()
    {
        using var owner = new Component(null!);
        var display = new CustomDisplay(owner);
        Assert.Same(owner, display.Owner);
        Assert.Equal(1, owner.ComponentCount);
        Assert.Same(display, owner.Components[0]);
        Assert.True(display.FrameRate > 0);
        display.Dispose();
        Assert.Equal(0, owner.ComponentCount);
    }

    [Fact]
    public void InvalidDibDimensionsAndFormatDoNotTouchExternalPalette()
    {
        var palette = new Palette();
        palette.Entries[0].Red = 29;
        Assert.Throws<ArgumentOutOfRangeException>(() => new Dib16Surface(-1, 4, PixelFormats.pfRGB555));
        Assert.Throws<NotSupportedException>(() => new Dib16Surface(7, 4, PixelFormats.pfPAL8));
        Assert.Equal((byte)29, palette.Entries[0].Red);
    }

    [Fact]
    public void ImageLibraryLayoutFlagsRunOnce()
    {
        using var owner = new Component(null!);
        var session = new LayoutSession();
        var library = new ImageLibrary(owner);
        session.BeginRead(library);
        var font = new ImageLibraryFont(owner);
        session.BeginRead(font);
        Assert.Equal(2, owner.ComponentCount);
        Assert.True(library.ComponentState.Contains(ComponentState.Loading));
        Assert.True(library.ComponentState.Contains(ComponentState.Reading));
        session.EndRead(library);
        Assert.False(library.ComponentState.Contains(ComponentState.Reading));
        Assert.True(library.ComponentState.Contains(ComponentState.Loading));
        Assert.Equal(2, Field(font, "_characterDistance"));
    }

    [Fact]
    public void FastLoadStreamUsesOneReadOnlyManagedFile()
    {
        string directory = AppContext.BaseDirectory;
        while (!File.Exists(Path.Combine(directory, "OpenAow.slnx"))) directory = Path.GetDirectoryName(directory)!;
        string path = Path.Combine(directory, "AGENTS.md");
        var stream = new FastLoadFileStream(path);
        Span<byte> bytes = stackalloc byte[6];
        Assert.Equal(6, stream.Read(bytes));
        Assert.Equal("# Rule", System.Text.Encoding.UTF8.GetString(bytes));
        Assert.Equal(0L, stream.Seek(0, SeekOrigin.Begin));
        stream.Dispose();
        Assert.Throws<FileNotFoundException>(() => new FastLoadFileStream(path + ".absent"));
    }

    [Fact]
    public void SurfaceCaptureKeepsCopiedPixelsAndPalette()
    {
        using var source = new Dib16Surface(7, 4, PixelFormats.pfRGB555);
        source.Clear(255);
        using var copy = new DataSurface(source, default);
        LockedSurface lockedSurface = default;
        copy.Lock(ref lockedSurface);
        Assert.Equal(7, lockedSurface.Width);
        Assert.Equal(4, lockedSurface.Height);
        Assert.Equal(PixelFormats.pfRGB555, lockedSurface.PixelFormatId);
        Assert.Equal((ushort)0x7C00, lockedSurface.View().UInt16(0));
        copy.Unlock();
    }

    [Fact]
    public void VirtualOwnerCallSeesBaseDefaultsBeforeDerivedBody()
    {
        LibraryImageClass.Register<ObservedImage>();
        using var image = new ObservedImage(null!);
        Assert.Equal(0, image.SeenDerivedValue);
        Assert.Equal(-1, image.SeenPosition);
        Assert.Equal(77, image.DerivedValue);
    }

    private sealed class ObservedImage : LibraryImage, ILibraryImageClass
    {
        static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new ObservedImage(owner);

        public int DerivedValue { get; }
        public int SeenDerivedValue { get; private set; }
        public int SeenPosition { get; private set; }

        internal ObservedImage(CustomImageLibrary owner) : base(owner) { DerivedValue = 77; }

        public override void SetOwner(CustomImageLibrary owner)
        {
            SeenDerivedValue = DerivedValue;
            SeenPosition = DataPosition;
            base.SetOwner(owner);
        }
    }

    [Fact]
    public void SdlHostCapturePrecedesOwnerCallbacks()
    {
        using var owner = new DisplayObservingOwner();
        var controller = new SdlDisplayController(new NullDisplayHost(), owner);
        Assert.Equal(7, owner.X);
        Assert.Equal(9, owner.Y);
        Assert.Equal(1, owner.ComponentCount);
        controller.Dispose();
        Assert.Equal(0, owner.ComponentCount);
    }

    private sealed class DisplayObservingOwner : Component
    {
        public int X, Y;

        public override void Notification(Component component, Aow.Rtl.Component.Operation operation)
        {
            if (operation == Aow.Rtl.Component.Operation.Insert && component is SdlDisplayController controller)
                controller.GetMousePosition(ref X, ref Y);
            base.Notification(component, operation);
        }
    }

    private sealed class NullDisplayHost : Game.Host.IDisplayHost
    {
        public (int Width, int Height) ClientSize => (640, 480);
        public bool Fullscreen { get; set; }
        public bool HasFocus => true;

        public void Present(in LockedSurface surface) { }

        public (int X, int Y, bool Inside) PointerPosition() => (7, 9, true);

        public void PumpEvents(Game.Host.IDisplayEvents sink) { }

        public void ShowSystemCursor(bool show) { }
    }

}
