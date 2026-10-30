using System.Reflection;
using Aow.Rtl;
using Aow.Units.DisplayC;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using LibraryImage = Aow.Units.ImageLib.TLibraryImage;
using G = Aow.Units.GFXE.GFXE;

namespace Aow.Rtl.Tests;

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
        yield return new object[] { typeof(LibraryImage), (Func<TCustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<LibraryImage>().Create(owner)), (Func<TCustomImageLibrary, LibraryImage>)(owner => new LibraryImage(owner)) };
        yield return new object[] { typeof(TImageNode), (Func<TCustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TImageNode>().Create(owner)), (Func<TCustomImageLibrary, LibraryImage>)(owner => new TImageNode(owner)) };
        yield return new object[] { typeof(TImage08), (Func<TCustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TImage08>().Create(owner)), (Func<TCustomImageLibrary, LibraryImage>)(owner => new TImage08(owner)) };
        yield return new object[] { typeof(TPicture08), (Func<TCustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TPicture08>().Create(owner)), (Func<TCustomImageLibrary, LibraryImage>)(owner => new TPicture08(owner)) };
        yield return new object[] { typeof(TRLESprite08), (Func<TCustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TRLESprite08>().Create(owner)), (Func<TCustomImageLibrary, LibraryImage>)(owner => new TRLESprite08(owner)) };
        yield return new object[] { typeof(TImage16), (Func<TCustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TImage16>().Create(owner)), (Func<TCustomImageLibrary, LibraryImage>)(owner => new TImage16(owner)) };
        yield return new object[] { typeof(TPicture16), (Func<TCustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TPicture16>().Create(owner)), (Func<TCustomImageLibrary, LibraryImage>)(owner => new TPicture16(owner)) };
        yield return new object[] { typeof(TTransparentPicture16), (Func<TCustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TTransparentPicture16>().Create(owner)), (Func<TCustomImageLibrary, LibraryImage>)(owner => new TTransparentPicture16(owner)) };
        yield return new object[] { typeof(TSprite16), (Func<TCustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TSprite16>().Create(owner)), (Func<TCustomImageLibrary, LibraryImage>)(owner => new TSprite16(owner)) };
        yield return new object[] { typeof(TRLESprite16), (Func<TCustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TRLESprite16>().Create(owner)), (Func<TCustomImageLibrary, LibraryImage>)(owner => new TRLESprite16(owner)) };
        yield return new object[] { typeof(TTransparentRLESprite16), (Func<TCustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TTransparentRLESprite16>().Create(owner)), (Func<TCustomImageLibrary, LibraryImage>)(owner => new TTransparentRLESprite16(owner)) };
        yield return new object[] { typeof(TBitMask), (Func<TCustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TBitMask>().Create(owner)), (Func<TCustomImageLibrary, LibraryImage>)(owner => new TBitMask(owner)) };
        yield return new object[] { typeof(TShadow), (Func<TCustomImageLibrary, LibraryImage>)(owner => LibraryImageClass.Of<TShadow>().Create(owner)), (Func<TCustomImageLibrary, LibraryImage>)(owner => new TShadow(owner)) };

    }

    [Theory]
    [MemberData(nameof(Images))]
    public void ImagesKeepDefaultsAndOwner(Type type, Func<TCustomImageLibrary, LibraryImage> create, Func<TCustomImageLibrary, LibraryImage> direct)
    {
        var owner = TImageLibrary.Create(null!);
        try
        {
            var descriptor = LibraryImageClass.Of(type);
            Assert.Equal(type, descriptor.Type);
            foreach (var construct in new[] { direct, create })
            foreach (var suppliedOwner in new TCustomImageLibrary[] { null!, owner })
            {
                var image = construct(suppliedOwner);
                Assert.Equal(type, image.GetType());
                Assert.Same(descriptor, image.ImageClass);
                Assert.Same(suppliedOwner, image.Owner);
                Assert.Equal(-1, image.DataPosition);
                Assert.False(image.ReloadingEnabled);
                Assert.Equal(TLoadMode.lmLoadOnDemand, image.LoadMode);
                Assert.Equal(TBlendMode.bmAlpha, image.ShowInfo.BlendMode);
                Assert.Equal(type.Name.StartsWith("TTransparent", StringComparison.Ordinal) ? TShowMode.smTransparent : TShowMode.smOpaque, image.ShowInfo.ShowMode);
                Assert.Equal(0, image.Width);
                Assert.Equal(0, image.Height);
                if (image is TImageNode) Assert.Equal("(multi images)", image.FileName.ToString());
                Assert.Equal(0, owner.Count);
                Assert.Equal(0, ((TList)Field(Field(owner, "FPaletteManager")!, "FPalettes")!).Count);
                image.Destroy();
            }
        }
        finally { owner.Destroy(); }
    }

    public static IEnumerable<object[]> Components()
    {
        yield return new object[] { typeof(TSdlDC), (Func<TComponent, TComponent>)(owner => TSdlDC.Create(new NullDisplayHost(), owner)) };
        yield return new object[] { typeof(TDisplay), (Func<TComponent, TComponent>)(owner => TDisplay.Create(owner)) };
        yield return new object[] { typeof(TWindows24DC), (Func<TComponent, TComponent>)(owner => TWindows24DC.Create(owner)) };
        yield return new object[] { typeof(TWindows32DC), (Func<TComponent, TComponent>)(owner => TWindows32DC.Create(owner)) };
        yield return new object[] { typeof(TDDrawDC), (Func<TComponent, TComponent>)(owner => TDDrawDC.Create(owner)) };
        yield return new object[] { typeof(TCustomDisplay), (Func<TComponent, TComponent>)(owner => TCustomDisplay.Create(owner)) };
        yield return new object[] { typeof(TDisplayCtrl), (Func<TComponent, TComponent>)(owner => TDisplayCtrl.Create(owner)) };
        yield return new object[] { typeof(TWindowsDC), (Func<TComponent, TComponent>)(owner => TWindowsDC.Create(owner)) };
        yield return new object[] { typeof(TWindows08DC), (Func<TComponent, TComponent>)(owner => TWindows08DC.Create(owner)) };
        yield return new object[] { typeof(TWindows16DC), (Func<TComponent, TComponent>)(owner => TWindows16DC.Create(owner)) };
        yield return new object[] { typeof(TDDrawFullScrDC), (Func<TComponent, TComponent>)(owner => TDDrawFullScrDC.Create(owner)) };
        yield return new object[] { typeof(TDDrawFullScr16DC), (Func<TComponent, TComponent>)(owner => TDDrawFullScr16DC.Create(owner)) };
        yield return new object[] { typeof(TDDrawFullScr24DC), (Func<TComponent, TComponent>)(owner => TDDrawFullScr24DC.Create(owner)) };
        yield return new object[] { typeof(TDDrawFullScr32DC), (Func<TComponent, TComponent>)(owner => TDDrawFullScr32DC.Create(owner)) };
        yield return new object[] { typeof(TCustomImageLibrary), (Func<TComponent, TComponent>)(owner => TCustomImageLibrary.Create(owner)) };
        yield return new object[] { typeof(TImageLibrary), (Func<TComponent, TComponent>)(owner => TImageLibrary.Create(owner)) };
        yield return new object[] { typeof(TImageLibraryFont), (Func<TComponent, TComponent>)(owner => TImageLibraryFont.Create(owner)) };
        yield return new object[] { typeof(TImageLibraryManager), (Func<TComponent, TComponent>)(owner => TImageLibraryManager.Create(owner)) };

    }

    [Theory]
    [MemberData(nameof(Components))]
    public void ComponentsKeepDefaultsAndOwnership(Type type, Func<TComponent, TComponent> create)
    {
        var owner = new TComponent();
        var value = create(owner);
        Assert.Equal(type, value.GetType());
        Assert.Same(owner, value.Owner);
        Assert.Equal(1, owner.ComponentCount);
        if (value is TCustomDisplay display)
        {
            Assert.Equal(100, display.Width);
            Assert.Equal(100, display.Height);
            Assert.Equal(640, Field(display, "FBackSurfaceWidth"));
            Assert.Equal(480, Field(display, "FBackSurfaceHeight"));
            Assert.Equal(30, Field(display, "FFrameRate"));
            Assert.Equal(TBufferingScheme.bsDouble, Field(display, "FBufferingScheme"));
        }
        if (value is TDisplayCtrl dc)
        {
            Assert.NotNull(dc.GetInvMatrix());
            Assert.Equal((byte)4, Field(dc.GetInvMatrix(), "FWidth"));
            Assert.Equal((byte)4, Field(dc.GetInvMatrix(), "FHeight"));
        }
        if (value is TCustomImageLibrary library)
        {
            Assert.Equal(0, library.Count);
            Assert.Equal(true, Field(library, "FLoadOnDemand"));
            Assert.NotNull(Field(library, "FPaletteManager"));
        }
        if (value is TImageLibraryFont)
        {
            Assert.Equal(2, Field(value, "FCharacterDistance"));
            Assert.Equal(-1, Field(value, "FFontColorPaletteIndex"));
        }
        if (value is TImageLibraryManager)
        {
            Assert.NotNull(Field(value, "FDiskList"));
            Assert.NotSame(Field(value, "FDiskList"), Field(value, "FOwnedList"));
        }
        if (value is TDDrawFullScrDC)
        {
            Assert.Throws<NotImplementedException>(() => value.Destroy());
            Assert.Equal(1, owner.ComponentCount);
            owner.RemoveComponent(value);
        }
        else value.Destroy();
        Assert.Equal(0, owner.ComponentCount);
        owner.Destroy();
    }

    [Theory]
    [MemberData(nameof(Components))]
    public void FailureDuringOwnerNotificationKeepsExistingCleanup(Type type, Func<TComponent, TComponent> create)
    {
        var owner = new ThrowingOwner();
        var error = Record.Exception(() => create(owner));
        Assert.NotNull(error);
        bool incompleteCleanup = typeof(TDDrawFullScrDC).IsAssignableFrom(type) || typeof(TCustomImageLibrary).IsAssignableFrom(type) || type == typeof(TImageLibraryManager);
        Assert.Equal(incompleteCleanup ? 1 : 0, owner.ComponentCount);
        Assert.NotNull(owner.Last);
        Assert.Equal(incompleteCleanup ? owner : null, owner.Last.Owner);
        Assert.IsType(incompleteCleanup
            ? typeof(TDDrawFullScrDC).IsAssignableFrom(type) ? typeof(NotImplementedException) : typeof(NullReferenceException)
            : typeof(InvalidOperationException), error);
        if (incompleteCleanup) owner.RemoveComponent(owner.Last);
        owner.Destroy();
    }

    private sealed class ThrowingOwner : TComponent
    {
        public TComponent? Last;
        public override void Notification(TComponent component, TOperation operation)
        {
            base.Notification(component, operation);
            if (operation == TOperation.opInsert)
            {
                Last = component;
                throw new InvalidOperationException("owner callback after insertion");
            }
        }
    }

    [Fact]
    public void PrivateHelpersKeepInitialState()
    {
        var palette = new TLibraryImagePalette();
        Assert.NotNull(Field(palette, "FPalette"));
        var manager = new TPaletteManager();
        Assert.NotNull(Field(manager, "FPalettes"));
        var ilManager = TImageLibraryManager.Create(null!);
        var list = new TILManagerList(ilManager);
        Assert.Same(ilManager, Field(list, "FOwner"));
        Assert.NotNull(Field(list, "FHashTable"));
        var sequences = new TImageSequenceList(ilManager, null!);
        Assert.Same(ilManager, Field(sequences, "FILManager"));
        Assert.Equal(0, sequences.Count);
        var sequence = new TImageSequence(sequences);
        Assert.Same(sequences, Field(sequence, "FOwner"));
        var matrix = new TMatrix(3, 2);
        Assert.Equal((byte)0, matrix.GetVal(2, 1));
        var modes = new TDisplayModeList();
        Assert.NotNull(Field(modes, "FList"));
        var blend = new TBlendTable5Bit();
        Assert.Equal(5, blend.Bits);
        var blends = new TBlendTableList();
        Assert.NotNull(Field(blends, "FList"));
        var lookup = new TLookupBlendTableCollection100();
        Assert.NotNull(Field(lookup, "FTables"));
        var gdi = new TGDISurface(123);
        Assert.Equal(123, Field(gdi, "FHandle"));
        foreach (var value in new TObject[] { palette, manager, list, sequence, sequences, ilManager, matrix, modes, blend, blends, lookup, gdi }) value.Destroy();
    }

    public static IEnumerable<object[]> Surfaces()
    {
        yield return new object[] { (Func<TGFXEPalette, TSurface>)(palette => TDataSurface.Create(7, 4, G.pfPAL8, palette)), G.pfPAL8, 8 };
        yield return new object[] { (Func<TGFXEPalette, TSurface>)(palette => new TDIB08Surface(7, 4, palette)), G.pfPAL8, 8 };
        yield return new object[] { (Func<TGFXEPalette, TSurface>)(_ => new TDIB16Surface(7, 4, G.pfRGB555)), G.pfRGB555, 16 };
        yield return new object[] { (Func<TGFXEPalette, TSurface>)(_ => new TDIB24Surface(7, 4)), G.pfRGB24, 24 };
        yield return new object[] { (Func<TGFXEPalette, TSurface>)(_ => new TDIB32Surface(7, 4)), G.pfRGB32, 28 };
    }

    [Theory]
    [MemberData(nameof(Surfaces))]
    public void SurfacesKeepDimensionsFormatAndPaletteIsolation(Func<TGFXEPalette, TSurface> create, int format, int pitch)
    {
        var palette = TGFXEPalette.Create();
        palette.Entries[0].R = 29;
        var surface = create(palette);
        TSurfaceDesc sd = default;
        Assert.Equal(G.GFXE_OK, surface.Lock(ref sd));
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
        surface.Destroy();
        Assert.Equal((byte)29, palette.Entries[0].R);
        palette.Destroy();
    }

    [Fact]
    public void DataSurfaceFailureCleansPrivateStateAndKeepsSuppliedPalette()
    {
        var palette = TGFXEPalette.Create();
        palette.Entries[0].R = 29;
        FailingDataSurface.Last = null;
        Assert.Throws<InvalidOperationException>(() =>
        {
            ConstructionContext.Create(context => new FailingDataSurface(context, palette));
        });
        var failed = FailingDataSurface.Last;
        Assert.NotNull(failed);
        Assert.Equal(1, failed.Destructions);
        Assert.Null(failed.Palette);
        Assert.Equal((byte)29, palette.Entries[0].R);
        palette.Destroy();
    }

    private sealed class FailingDataSurface : TDataSurface
    {
        public static FailingDataSurface? Last;
        internal FailingDataSurface(ConstructionContext context, TGFXEPalette palette) : base(context, 7, 4, G.pfPAL8, palette) { }
        public int Destructions;
        public override int Resize(int width, int height)
        {
            base.Resize(width, height);
            Last = this;
            throw new InvalidOperationException("after buffer allocation");
        }
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    [Fact]
    public void DisplayFailureAfterPublicationRemovesOwnerOnce()
    {
        var owner = new TComponent();
        FailingDisplay.Last = null;
        Assert.Throws<InvalidOperationException>(() =>
        {
            ConstructionContext.Create(context => new FailingDisplay(context, owner));
        });
        var failed = FailingDisplay.Last;
        Assert.NotNull(failed);
        Assert.Equal(1, failed.Destructions);
        Assert.Equal(0, owner.ComponentCount);
        Assert.Null(failed.Owner);
        owner.Destroy();
    }

    private sealed class FailingDisplay : TCustomDisplay
    {
        public static FailingDisplay? Last;
        internal FailingDisplay(ConstructionContext context, TComponent owner) : base(context, owner) { }
        public int Destructions;
        public override void SetFrameRate(int value) { base.SetFrameRate(value); Last = this; throw new InvalidOperationException("after insertion"); }
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    [Fact]
    public void InvalidDibDimensionsAndFormatDoNotTouchExternalPalette()
    {
        var palette = TGFXEPalette.Create();
        palette.Entries[0].R = 29;
        Assert.Throws<ArgumentOutOfRangeException>(() => new TDIB08Surface(-1, 4, palette));
        Assert.Throws<ArgumentOutOfRangeException>(() => new TDIB16Surface(-1, 4, G.pfRGB555));
        Assert.Throws<ENotSupported>(() => new TDIB16Surface(7, 4, G.pfPAL8));
        Assert.Equal((byte)29, palette.Entries[0].R);
        palette.Destroy();
    }
    [Fact]
    public void LibraryLegacyBridgeAndLayoutFlagsRunOnce()
    {
        var owner = new TComponent();
        var session = new Aow.Layout.LayoutSession();
        var library = TImageLibrary.Create(owner);
        session.BeginRead(library);
        var font = TImageLibraryFont.Create(owner);
        session.BeginRead(font);
        Assert.Equal(2, owner.ComponentCount);
        Assert.True(library.ComponentState.Contains(TComponentState.csLoading));
        Assert.True(library.ComponentState.Contains(TComponentState.csReading));
        session.EndRead(library);
        Assert.False(library.ComponentState.Contains(TComponentState.csReading));
        Assert.True(library.ComponentState.Contains(TComponentState.csLoading));
        Assert.Equal(2, Field(font, "FCharacterDistance"));
        var legacy = Aow.Units.HSLib.THSLibrary.Create(owner);
        Assert.NotNull(Field(legacy, "FList"));
        Assert.NotNull(Field(legacy, "FPaletteManager"));
        Assert.Equal(3, owner.ComponentCount);
        owner.Destroy();
    }

    [Fact]
    public void WindowsPaletteSurfaceKeepsOwnerBitmapAndDimensions()
    {
        var owner = TWindows08DC.Create(null!);
        var surface = new TWindows08DIBSurface(7, 4, owner);
        TSurfaceDesc sd = default;
        Assert.Equal(0, surface.Lock(ref sd));
        Assert.Equal(7, sd.Width);
        Assert.Equal(4, sd.Height);
        Assert.Equal(-8, sd.Pitch);
        Assert.Equal(G.pfPAL8, sd.PixelFormatID);
        Assert.Same(Field(owner, "FPalette"), sd.Palette);
        Assert.Same(owner, Field(surface, "FOwner"));
        surface.Unlock();
        surface.Destroy();
        owner.Destroy();
    }

    [Fact]
    public void FastLoadStreamUsesOneReadOnlyManagedFile()
    {
        string directory = AppContext.BaseDirectory;
        while (!File.Exists(Path.Combine(directory, "OpenAow.slnx"))) directory = Path.GetDirectoryName(directory)!;
        string path = Path.Combine(directory, "AGENTS.md");
        var stream = new TFastLoadFileStream(path);
        Span<byte> bytes = stackalloc byte[6];
        Assert.Equal(6, stream.Read(bytes));
        Assert.Equal("# Rule", System.Text.Encoding.UTF8.GetString(bytes));
        Assert.Equal(0, stream.Seek(0, 0));
        stream.Destroy();
        Assert.Throws<EFOpenError>(() => new TFastLoadFileStream(path + ".absent"));
    }

    [Fact]
    public void SurfaceCaptureKeepsCopiedPixelsAndPalette()
    {
        var source = new TDIB16Surface(7, 4, G.pfRGB555);
        source.Cls(255);
        var copy = TDataSurface.CreateFromSurface(source, default);
        TSurfaceDesc sd = default;
        copy.Lock(ref sd);
        Assert.Equal(7, sd.Width);
        Assert.Equal(4, sd.Height);
        Assert.Equal(G.pfRGB555, sd.PixelFormatID);
        var data = Aow.Rtl.Rtl.BytePointer(sd.Data);
        Assert.Equal((ushort)0x7C00, data.Reinterpret<ushort>().Value);
        copy.Unlock();
        copy.Destroy();
        source.Destroy();
    }

    [Fact]
    public void VirtualOwnerCallSeesBaseDefaultsBeforeDerivedBody()
    {
        LibraryImageClass.Register<ObservedImage>();
        var image = new ObservedImage(null!);
        Assert.Equal(0, image.SeenDerivedValue);
        Assert.Equal(-1, image.SeenPosition);
        Assert.Equal(77, image.DerivedValue);
        image.Destroy();
    }

    private sealed class ObservedImage : LibraryImage, ILibraryImageClass
    {
        static LibraryImage ILibraryImageClass.Create(TCustomImageLibrary owner) => new ObservedImage(owner);
        public int DerivedValue;
        public int SeenDerivedValue;
        public int SeenPosition;
        internal ObservedImage(TCustomImageLibrary owner) : base(owner) { DerivedValue = 77; }
        public override void SetOwner(TCustomImageLibrary owner)
        {
            SeenDerivedValue = DerivedValue;
            SeenPosition = DataPosition;
            base.SetOwner(owner);
        }
    }


    [Fact]
    public void SdlHostCapturePrecedesOwnerCallbacks()
    {
        var owner = new DisplayObservingOwner();
        // This named raw consumer pins the retained host-before-owner route beside its factory.
        var legacy = new TSdlDC(new NullDisplayHost());
        legacy.Create__init(owner);
        Assert.Equal(7, owner.X);
        Assert.Equal(9, owner.Y);
        Assert.Equal(1, owner.ComponentCount);
        legacy.Destroy();
        var controller = TSdlDC.Create(new NullDisplayHost(), owner);
        Assert.Equal(7, owner.X);
        Assert.Equal(9, owner.Y);
        Assert.Equal(1, owner.ComponentCount);
        controller.Destroy();
        Assert.Equal(0, owner.ComponentCount);
        owner.Destroy();
    }

    private sealed class DisplayObservingOwner : TComponent
    {
        public int X, Y;
        public override void Notification(TComponent component, TOperation operation)
        {
            if (operation == TOperation.opInsert && component is TSdlDC controller)
                controller.GetMousePos(ref X, ref Y);
            base.Notification(component, operation);
        }
    }

    private sealed class NullDisplayHost : Aow.Platform.IDisplayHost
    {
        public (int Width, int Height) ClientSize => (640, 480);
        public bool Fullscreen { get; set; }
        public bool HasFocus => true;
        public void Present(in TSurfaceDesc surface) { }
        public (int X, int Y, bool Inside) PointerPosition() => (7, 9, true);
        public void PumpEvents(Aow.Platform.IDisplayEvents sink) { }
        public void ShowSystemCursor(bool show) { }
    }

}
