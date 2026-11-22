#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using LibraryImage = Aow.Game.Draw.Images.LibraryImage;

namespace Aow.Game.Tests;

public sealed class GraphicsConstructionTests
{
    private static object? Field(object value, string name)
        => (MemberNames.Field(value.GetType(), name) ?? throw new InvalidOperationException(name)).GetValue(value);

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
        using ImageLibrary owner = new(null!);
        LibraryImageClass descriptor = LibraryImageClass.Of(type);
        Assert.Equal(type, descriptor.Type);
        foreach (Func<CustomImageLibrary, LibraryImage>? construct in new[] { direct, create })
        {
            foreach (CustomImageLibrary suppliedOwner in new CustomImageLibrary[] { null!, owner })
            {
                LibraryImage image = construct(suppliedOwner);
                Assert.Equal(type, image.GetType());
                Assert.Same(descriptor, image.ImageClass);
                Assert.Same(suppliedOwner, image.Owner);
                Assert.Equal(-1, image.DataPosition);
                Assert.False(image.ReloadingEnabled);
                Assert.Equal(LoadMode.LoadOnDemand, image.LoadMode);
                Assert.Equal(BlendMode.Alpha, image.ShowInfo.BlendMode);
                Assert.False(image.ShowInfo.Blend.HasValue);
                Assert.Equal(0, image.ShowInfo.Blend.Percentage);
                Assert.Equal(type.Name.StartsWith("Transparent", comparisonType: StringComparison.Ordinal) ? ShowMode.Transparent : ShowMode.Opaque, image.ShowInfo.ShowMode);
                Assert.Equal(0, image.Width);
                Assert.Equal(0, image.Height);
                if (image is ImageNode)
                {
                    Assert.Equal("(multi images)", image.FileName);
                }

                Assert.Equal(0, owner.Count);
                Assert.Empty((System.Collections.IEnumerable)Field(Field(owner, name: "PaletteManager")!, name: "Palettes")!);
                image.Dispose();
            }
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
        Component owner = new(null);
        Component value = create(owner);
        Assert.Equal(type, value.GetType());
        Assert.Same(owner, value.Owner);
        Assert.Equal(1, owner.ComponentCount);
        if (value is CustomDisplay display)
        {
            Assert.Equal(0, display.Color);
            Assert.Equal(640, Field(display, name: "_backSurfaceWidth"));
            Assert.Equal(480, Field(display, name: "_backSurfaceHeight"));
            Assert.Equal(30, Field(display, name: "_frameRate"));
            Assert.Equal(BufferingScheme.Double, Field(display, name: "_bufferingScheme"));
        }
        if (value is DisplayController displayController)
        {
            Assert.NotNull(displayController.InverseMatrix);
            Assert.Equal(4, Field(displayController.InverseMatrix, name: "Width"));
            Assert.Equal(4, Field(displayController.InverseMatrix, name: "Height"));
        }
        if (value is CustomImageLibrary library)
        {
            Assert.Equal(0, library.Count);
            Assert.Equal(true, Field(library, name: "_loadOnDemand"));
            Assert.NotNull(Field(library, name: "PaletteManager"));
        }
        if (value is ImageLibraryFont)
        {
            Assert.Equal(2, Field(value, name: "CharacterDistance"));
            Assert.Equal(-1, Field(value, name: "_fontColorPaletteIndex"));
        }
        if (value is ImageLibraryManager)
        {
            Assert.NotNull(Field(value, name: "DiskList"));
            Assert.NotSame(Field(value, name: "DiskList"), Field(value, name: "OwnedList"));
        }
        value.Dispose();
        Assert.Equal(0, owner.ComponentCount);
        owner.Dispose();
    }

    [Theory]
    [MemberData(nameof(Components))]
    public void PlainComponentsStayPublishedUntilTeardown(Type type, Func<Component, Component> create)
    {
        using Component owner = new(null);
        Component value = create(owner);
        Assert.IsType(type, @object: value);
        Assert.Same(owner, value.Owner);
        Assert.Equal(1, owner.ComponentCount);
        Assert.Same(value, owner.Components[0]);
        value.Dispose();
        Assert.Equal(0, owner.ComponentCount);
    }

    [Fact]
    public void PrivateHelpersKeepInitialState()
    {
        LibraryImagePalette palette = new();
        Assert.NotNull(Field(palette, name: "Palette"));
        PaletteManager manager = new();
        Assert.NotNull(Field(manager, name: "Palettes"));
        ImageLibraryManager imageLibraryManager = new(null);
        ImageLibraryManagerList list = new(imageLibraryManager);
        Assert.Same(imageLibraryManager, Field(list, name: "_owner"));
        Assert.NotNull(Field(list, name: "HashTable"));
        ImageSequenceList sequences = new(imageLibraryManager, imageLibrary: null);
        Assert.Same(imageLibraryManager, Field(sequences, name: "ImageLibraryManager"));
        Assert.Empty(sequences);
        ImageSequence sequence = new(sequences);
        Assert.Same(sequences, Field(sequence, name: "Owner"));
        Matrix matrix = new(3, 2);
        Assert.Equal(0, matrix.GetValue(2, 1));
        DisplayModeList modes = new();
        Assert.NotNull(Field(modes, name: "_list"));
        BlendTable5Bit blend = new();
        Assert.Equal(5, blend.Bits);
        BlendTableList blends = new();
        Assert.NotNull(Field(blends, name: "_list"));
        LookupBlendTableCollection100 lookup = new();
        Assert.NotNull(Field(lookup, name: "Tables"));
        foreach (object value in new object[] { palette, manager, list, sequence, sequences, imageLibraryManager, matrix, modes, blend, blends, lookup })
        {
            ((value ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
        }
    }

    public static IEnumerable<object[]> Surfaces()
    {
        yield return new object[] { (Func<Palette, Surface>)(palette => new DataSurface(7, 4, pixelFormatId: PixelFormat.Pal8, palette)), PixelFormat.Pal8, 8 };
        yield return new object[] { (Func<Palette, Surface>)(_ => new Dib16Surface(7, 4, pixelFormatId: PixelFormat.Rgb555)), PixelFormat.Rgb555, 16 };
    }

    [Theory]
    [MemberData(nameof(Surfaces))]
    public void SurfacesKeepDimensionsFormatAndPaletteIsolation(Func<Palette, Surface> create, PixelFormat format, int pitch)
    {
        Palette palette = new();
        palette[0] = palette[0] with { Red = 29 };
        Surface surface = create(palette);
        LockedSurface lockedSurface = default;
        Assert.Equal(GraphicsResult.Ok, surface.Lock(ref lockedSurface));
        Assert.Equal(7, lockedSurface.Width);
        Assert.Equal(4, lockedSurface.Height);
        Assert.Equal(format, lockedSurface.PixelFormatId);
        Assert.Equal(pitch, lockedSurface.Pitch);
        Assert.NotNull(lockedSurface.Data);
        if (lockedSurface.Palette is { } copy)
        {
            Assert.NotSame(palette, copy);
            Assert.Equal((byte)29, copy[0].Red);
            copy[0] = copy[0] with { Red = 63 };
        }
        _ = surface.Unlock();
        surface.Dispose();
        Assert.Equal((byte)29, palette[0].Red);
    }

    [Fact]
    public void PlainDataSurfaceCopiesPaletteAndAllocatesItsBuffer()
    {
        Palette palette = new();
        palette[0] = palette[0] with { Red = 29 };
        DataSurface surface = new(7, 4, pixelFormatId: PixelFormat.Pal8, palette);
        Assert.NotSame(palette, surface.Palette);
        Assert.Equal((byte)29, surface.Palette[0].Red);
        Assert.Equal(7, surface.Width);
        Assert.Equal(4, surface.Height);
        LockedSurface descriptor = default;
        Assert.Equal(GraphicsResult.Ok, surface.Lock(ref descriptor));
        Assert.NotNull(descriptor.Data);
        _ = surface.Unlock();
        surface.Dispose();
        Assert.Equal((byte)29, palette[0].Red);
    }

    [Fact]
    public void PlainDisplayInitializesFrameRateAndOwner()
    {
        using Component owner = new(null);
        CustomDisplay display = new(owner);
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
        Palette palette = new();
        palette[0] = palette[0] with { Red = 29 };
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => new Dib16Surface(-1, 4, pixelFormatId: PixelFormat.Rgb555));
        _ = Assert.Throws<NotSupportedException>(() => new Dib16Surface(7, 4, pixelFormatId: PixelFormat.Pal8));
        Assert.Equal((byte)29, palette[0].Red);
    }

    [Fact]
    public void ImageLibraryLayoutFlagsRunOnce()
    {
        using Component owner = new(null);
        LayoutSession session = new();
        ImageLibrary library = new(owner);
        session.BeginRead(library);
        ImageLibraryFont font = new(owner);
        session.BeginRead(font);
        Assert.Equal(2, owner.ComponentCount);
        Assert.True(library.ComponentState.Contains(ComponentStates.Loading));
        Assert.True(library.ComponentState.Contains(ComponentStates.Reading));
        session.EndRead(library);
        Assert.False(library.ComponentState.Contains(ComponentStates.Reading));
        Assert.True(library.ComponentState.Contains(ComponentStates.Loading));
        Assert.Equal(2, Field(font, name: "CharacterDistance"));
    }

    [Fact]
    public void FastLoadStreamUsesOneReadOnlyManagedFile()
    {
        string directory = AppContext.BaseDirectory;
        while (!File.Exists(Path.Combine(directory, "OpenAow.slnx")))
        {
            directory = Path.GetDirectoryName(directory)!;
        }

        string path = Path.Combine(directory, "AGENTS.md");
        FastLoadFileStream stream = new(path);
        Span<byte> bytes = stackalloc byte[6];
        Assert.Equal(6, stream.Read(bytes));
        Assert.Equal("# Rule", System.Text.Encoding.UTF8.GetString(bytes));
        Assert.Equal(0L, stream.Seek(0, origin: SeekOrigin.Begin));
        stream.Dispose();
        _ = Assert.Throws<FileNotFoundException>(() => new FastLoadFileStream($"{path}.absent"));
    }

    [Fact]
    public void SurfaceCaptureKeepsCopiedPixelsAndPalette()
    {
        using Dib16Surface source = new(7, 4, pixelFormatId: PixelFormat.Rgb555);
        _ = source.Clear(255);
        using DataSurface copy = new(source, R: default);
        LockedSurface lockedSurface = default;
        _ = copy.Lock(ref lockedSurface);
        Assert.Equal(7, lockedSurface.Width);
        Assert.Equal(4, lockedSurface.Height);
        Assert.Equal(PixelFormat.Rgb555, lockedSurface.PixelFormatId);
        Assert.Equal((ushort)0x7C00, lockedSurface.View().UInt16(0));
        _ = copy.Unlock();
    }

    [Fact]
    public void VirtualOwnerCallSeesBaseDefaultsBeforeDerivedBody()
    {
        _ = LibraryImageClass.Register<ObservedImage>();
        using ObservedImage image = new(null!);
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

        internal ObservedImage(CustomImageLibrary owner) : base(owner) => DerivedValue = 77;

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
        using DisplayObservingOwner owner = new();
        SdlDisplayController controller = new(new NullDisplayHost(), owner);
        Assert.Equal(7, owner.X);
        Assert.Equal(9, owner.Y);
        Assert.Equal(1, owner.ComponentCount);
        controller.Dispose();
        Assert.Equal(0, owner.ComponentCount);
    }

    private sealed class DisplayObservingOwner : Component
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public override void Notification(Component component, Operation operation)
        {
            if (operation == Operation.Insert && component is SdlDisplayController controller)
            {
                int x = X;
                int y = Y;
                controller.GetMousePosition(ref x, ref y);
                X = x;
                Y = y;
            }
            base.Notification(component, operation);
        }
    }

    private sealed class NullDisplayHost : Host.IDisplayHost
    {
        public (int Width, int Height) ClientSize => (640, 480);
        public bool Fullscreen { get; set; }
        public bool HasFocus => true;

        public void Present(in LockedSurface surface) { }

        public (int X, int Y, bool Inside) PointerPosition() => (7, 9, true);

        public void PumpEvents(Host.IDisplayEvents sink) { }

        public void ShowSystemCursor(bool show) { }
    }
}
