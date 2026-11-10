using System.Reflection;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using LibraryImage = Aow.Game.Draw.Images.LibraryImage;

namespace Aow.Game.Tests;

public sealed class GraphicsConstructionTests
{
    private static object? Field(object value, string name)
    {
        for (Type? type = value.GetType(); type is not null; type = type.BaseType)
            if (type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly) is { } field)
                return field.GetValue(value);
        throw new InvalidOperationException(name);
    }

    public static IEnumerable<object[]> Images()
    {
        yield return new object[] { typeof(LibraryImage), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<LibraryImage>().Create(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new LibraryImage(owner)) };
        yield return new object[] { typeof(ImageNode), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<ImageNode>().Create(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new ImageNode(owner)) };
        yield return new object[] { typeof(Image08), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<Image08>().Create(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new Image08(owner)) };
        yield return new object[] { typeof(Picture08), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<Picture08>().Create(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new Picture08(owner)) };
        yield return new object[] { typeof(RLESprite08), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<RLESprite08>().Create(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new RLESprite08(owner)) };
        yield return new object[] { typeof(Image16), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<Image16>().Create(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new Image16(owner)) };
        yield return new object[] { typeof(Picture16), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<Picture16>().Create(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new Picture16(owner)) };
        yield return new object[] { typeof(TransparentPicture16), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TransparentPicture16>().Create(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new TransparentPicture16(owner)) };
        yield return new object[] { typeof(Sprite16), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<Sprite16>().Create(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new Sprite16(owner)) };
        yield return new object[] { typeof(RLESprite16), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<RLESprite16>().Create(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new RLESprite16(owner)) };
        yield return new object[] { typeof(TransparentRLESprite16), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TransparentRLESprite16>().Create(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new TransparentRLESprite16(owner)) };
        yield return new object[] { typeof(BitMask), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<BitMask>().Create(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new BitMask(owner)) };
        yield return new object[] { typeof(Shadow), (Func<CustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<Shadow>().Create(owner)), (Func<CustomImageLibrary, LibraryImage>)(owner => new Shadow(owner)) };

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
                Assert.Equal(0, ((DelphiList)Field(Field(owner, "FPaletteManager")!, "FPalettes")!).Count);
                image.Dispose();
            }
    }

    public static IEnumerable<object[]> Components()
    {
        yield return new object[] { typeof(SdlDC), (Func<Component, Component>)(owner => new SdlDC(new NullDisplayHost(), owner)) };
        yield return new object[] { typeof(Display), (Func<Component, Component>)(owner => new Display(owner)) };
        yield return new object[] { typeof(Windows24DC), (Func<Component, Component>)(owner => new Windows24DC(owner)) };
        yield return new object[] { typeof(Windows32DC), (Func<Component, Component>)(owner => new Windows32DC(owner)) };
        yield return new object[] { typeof(CustomDisplay), (Func<Component, Component>)(owner => new CustomDisplay(owner)) };
        yield return new object[] { typeof(DisplayCtrl), (Func<Component, Component>)(owner => new DisplayCtrl(owner)) };
        yield return new object[] { typeof(WindowsDC), (Func<Component, Component>)(owner => new WindowsDC(owner)) };
        yield return new object[] { typeof(Windows08DC), (Func<Component, Component>)(owner => new Windows08DC(owner)) };
        yield return new object[] { typeof(Windows16DC), (Func<Component, Component>)(owner => new Windows16DC(owner)) };
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
            Assert.Equal(640, Field(display, "FBackSurfaceWidth"));
            Assert.Equal(480, Field(display, "FBackSurfaceHeight"));
            Assert.Equal(30, Field(display, "FFrameRate"));
            Assert.Equal(BufferingScheme.Double, Field(display, "FBufferingScheme"));
        }
        if (value is DisplayCtrl dc)
        {
            Assert.NotNull(dc.GetInvMatrix());
            Assert.Equal((byte)4, Field(dc.GetInvMatrix(), "FWidth"));
            Assert.Equal((byte)4, Field(dc.GetInvMatrix(), "FHeight"));
        }
        if (value is CustomImageLibrary library)
        {
            Assert.Equal(0, library.Count);
            Assert.Equal(true, Field(library, "FLoadOnDemand"));
            Assert.NotNull(Field(library, "FPaletteManager"));
        }
        if (value is ImageLibraryFont)
        {
            Assert.Equal(2, Field(value, "FCharacterDistance"));
            Assert.Equal(-1, Field(value, "FFontColorPaletteIndex"));
        }
        if (value is ImageLibraryManager)
        {
            Assert.NotNull(Field(value, "FDiskList"));
            Assert.NotSame(Field(value, "FDiskList"), Field(value, "FOwnedList"));
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
        Assert.NotNull(Field(palette, "FPalette"));
        var manager = new PaletteManager();
        Assert.NotNull(Field(manager, "FPalettes"));
        var ilManager = new ImageLibraryManager(null!);
        var list = new ILManagerList(ilManager);
        Assert.Same(ilManager, Field(list, "FOwner"));
        Assert.NotNull(Field(list, "FHashTable"));
        var sequences = new ImageSequenceList(ilManager, null!);
        Assert.Same(ilManager, Field(sequences, "FILManager"));
        Assert.Equal(0, sequences.Count);
        var sequence = new ImageSequence(sequences);
        Assert.Same(sequences, Field(sequence, "FOwner"));
        var matrix = new Matrix(3, 2);
        Assert.Equal((byte)0, matrix.GetVal(2, 1));
        var modes = new DisplayModeList();
        Assert.NotNull(Field(modes, "FList"));
        var blend = new BlendTable5Bit();
        Assert.Equal(5, blend.Bits);
        var blends = new BlendTableList();
        Assert.NotNull(Field(blends, "FList"));
        var lookup = new LookupBlendTableCollection100();
        Assert.NotNull(Field(lookup, "FTables"));
        var gdi = new GDISurface(123);
        Assert.Equal(123, Field(gdi, "FHandle"));
        foreach (var value in new object[] { palette, manager, list, sequence, sequences, ilManager, matrix, modes, blend, blends, lookup, gdi }) ((value ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
    }

    public static IEnumerable<object[]> Surfaces()
    {
        yield return new object[] { (Func<GFXEPalette, Surface>)(palette => new DataSurface(7, 4, PixelFormats.pfPAL8, palette)), PixelFormats.pfPAL8, 8 };
        yield return new object[] { (Func<GFXEPalette, Surface>)(palette => new DIB08Surface(7, 4, palette)), PixelFormats.pfPAL8, 8 };
        yield return new object[] { (Func<GFXEPalette, Surface>)(_ => new DIB16Surface(7, 4, PixelFormats.pfRGB555)), PixelFormats.pfRGB555, 16 };
        yield return new object[] { (Func<GFXEPalette, Surface>)(_ => new DIB24Surface(7, 4)), PixelFormats.pfRGB24, 24 };
        yield return new object[] { (Func<GFXEPalette, Surface>)(_ => new DIB32Surface(7, 4)), PixelFormats.pfRGB32, 28 };
    }

    [Theory]
    [MemberData(nameof(Surfaces))]
    public void SurfacesKeepDimensionsFormatAndPaletteIsolation(Func<GFXEPalette, Surface> create, int format, int pitch)
    {
        var palette = new GFXEPalette();
        palette.Entries[0].R = 29;
        var surface = create(palette);
        SurfaceDesc sd = default;
        Assert.Equal(PixelFormats.GFXE_OK, surface.Lock(ref sd));
        Assert.Equal(7, sd.Width);
        Assert.Equal(4, sd.Height);
        Assert.Equal(format, sd.PixelFormatID);
        Assert.Equal(pitch, sd.Pitch);
        Assert.NotNull(sd.Data);
        if (sd.Palette is { } copy)
        {
            Assert.NotSame(palette, copy);
            Assert.Equal((byte)29, copy.Entries[0].R);
            copy.Entries[0].R = 63;
        }
        surface.Unlock();
        surface.Dispose();
        Assert.Equal((byte)29, palette.Entries[0].R);
    }

    [Fact]
    public void PlainDataSurfaceCopiesPaletteAndAllocatesItsBuffer()
    {
        var palette = new GFXEPalette();
        palette.Entries[0].R = 29;
        var surface = new DataSurface(7, 4, PixelFormats.pfPAL8, palette);
        Assert.NotSame(palette, surface.Palette);
        Assert.Equal((byte)29, surface.Palette.Entries[0].R);
        Assert.Equal(7, surface.Width);
        Assert.Equal(4, surface.Height);
        SurfaceDesc descriptor = default;
        Assert.Equal(0, surface.Lock(ref descriptor));
        Assert.NotNull(descriptor.Data);
        surface.Unlock();
        surface.Dispose();
        Assert.Equal((byte)29, palette.Entries[0].R);
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
        var palette = new GFXEPalette();
        palette.Entries[0].R = 29;
        Assert.Throws<ArgumentOutOfRangeException>(() => new DIB08Surface(-1, 4, palette));
        Assert.Throws<ArgumentOutOfRangeException>(() => new DIB16Surface(-1, 4, PixelFormats.pfRGB555));
        Assert.Throws<NotSupportedException>(() => new DIB16Surface(7, 4, PixelFormats.pfPAL8));
        Assert.Equal((byte)29, palette.Entries[0].R);
    }

    [Fact]
    public void LibraryLegacyBridgeAndLayoutFlagsRunOnce()
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
        Assert.Equal(2, Field(font, "FCharacterDistance"));
        var legacy = new HSLibrary(owner);
        Assert.NotNull(Field(legacy, "FList"));
        Assert.NotNull(Field(legacy, "FPaletteManager"));
        Assert.Equal(3, owner.ComponentCount);
    }

    [Fact]
    public void WindowsPaletteSurfaceKeepsOwnerBitmapAndDimensions()
    {
        using var owner = new Windows08DC(null!);
        using var surface = new Windows08DIBSurface(7, 4, owner);
        SurfaceDesc sd = default;
        Assert.Equal(0, surface.Lock(ref sd));
        Assert.Equal(7, sd.Width);
        Assert.Equal(4, sd.Height);
        Assert.Equal(-8, sd.Pitch);
        Assert.Equal(PixelFormats.pfPAL8, sd.PixelFormatID);
        Assert.Same(Field(owner, "FPalette"), sd.Palette);
        Assert.Same(owner, Field(surface, "FOwner"));
        surface.Unlock();
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
        using var source = new DIB16Surface(7, 4, PixelFormats.pfRGB555);
        source.Cls(255);
        using var copy = new DataSurface(source, default);
        SurfaceDesc sd = default;
        copy.Lock(ref sd);
        Assert.Equal(7, sd.Width);
        Assert.Equal(4, sd.Height);
        Assert.Equal(PixelFormats.pfRGB555, sd.PixelFormatID);
        Assert.Equal((ushort)0x7C00, sd.View().Word(0));
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
        static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new ObservedImage(owner);

        public int DerivedValue;
        public int SeenDerivedValue;
        public int SeenPosition;

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
        var controller = new SdlDC(new NullDisplayHost(), owner);
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
            if (operation == Aow.Rtl.Component.Operation.Insert && component is SdlDC controller)
                controller.GetMousePos(ref X, ref Y);
            base.Notification(component, operation);
        }
    }

    private sealed class NullDisplayHost : Game.Host.IDisplayHost
    {
        public (int Width, int Height) ClientSize => (640, 480);
        public bool Fullscreen { get; set; }
        public bool HasFocus => true;

        public void Present(in SurfaceDesc surface) { }

        public (int X, int Y, bool Inside) PointerPosition() => (7, 9, true);

        public void PumpEvents(Game.Host.IDisplayEvents sink) { }

        public void ShowSystemCursor(bool show) { }
    }

}
