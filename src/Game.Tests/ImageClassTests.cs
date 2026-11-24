#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using System.Text.Json;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

namespace Aow.Game.Tests;

public sealed class ImageClassTests
{
    private sealed class UnregisteredImage(CustomImageLibrary owner) : LibraryImage(owner);

    private sealed class UnregisteredImageFile : ImageFile;

    [Fact]
    public void ImageDescriptorsRequireExplicitRegistration()
    {
        _ = Assert.Throws<InvalidOperationException>(LibraryImageClass.Of<UnregisteredImage>);
        _ = Assert.Throws<InvalidOperationException>(() => LibraryImageClass.Of(typeof(UnregisteredImage)));
        _ = Assert.Throws<InvalidOperationException>(ImageFileClass.Of<UnregisteredImageFile>);
        _ = Assert.Throws<InvalidOperationException>(() => ImageFileClass.Of(typeof(UnregisteredImageFile)));
        _ = Assert.Throws<InvalidOperationException>(LibraryImageClass.Register<Picture08>);
        _ = Assert.Throws<InvalidOperationException>(ImageFileClass.Register<BmpImageFile>);
        Assert.Equal("Picture08", LibraryImageClass.Of<Picture08>().StorageKey);
        Type[] types = typeof(LibraryImage).Assembly.GetTypes();
        Assert.Equal(types.Where(t => t.IsClass && typeof(ILibraryImageClass).IsAssignableFrom(t)).Select(t => t.FullName).Order(StringComparer.Ordinal),
            LibraryImageClass.All.Where(c => c.Type.Assembly == typeof(LibraryImage).Assembly).Select(c => c.Type.FullName).Order(StringComparer.Ordinal));
        Assert.Equal(types.Where(t => t.IsClass && typeof(IImageFileClass).IsAssignableFrom(t)).Select(t => t.FullName).Order(StringComparer.Ordinal),
            ImageFileClass.All.Select(c => c.Type.FullName).Order(StringComparer.Ordinal));
    }

    private sealed record Baseline(string Type, int ClassId, byte ClassVersion, string Name);

    [Fact]
    public void LibraryLoadKeepsEveryRegisteredImageClassAgainstBaseSnapshot()
    {
        // Captured from the public image-class registry at b635b9f before the descriptor edits.
        using Stream baselineStream = typeof(ImageClassTests).Assembly.GetManifestResourceStream("Aow.Game.Tests.ImageClassSnapshot.json")!;
        Baseline[] baseline = JsonSerializer.Deserialize<Baseline[]>(baselineStream)!;
        List<LibraryImageClass> previousClasses = ImageLibraries.LibraryImageClasses;
        ImageLibraries.LibraryImageClasses = [];
        ImageLibrary source = new(null!);
        ImageLibrary loaded = new(null!);
        MemoryStream stream = new();
        try
        {
            ImageLibraries.RegisterLibraryImageClasses([
                LibraryImageClass.Of<Picture08>(), LibraryImageClass.Of<RleSprite08>(),
                LibraryImageClass.Of<Picture16>(), LibraryImageClass.Of<TransparentPicture16>(),
                LibraryImageClass.Of<RleSprite16>(), LibraryImageClass.Of<TransparentRleSprite16>(),
                LibraryImageClass.Of<Shadow>(), LibraryImageClass.Of<ImageNode>(), LibraryImageClass.Of<Sprite16>()]);
            Assert.Equal(baseline.Length, ImageLibraries.LibraryImageClasses.Count);
            for (int i = 0; i < baseline.Length; i++)
            {
                Baseline expected = baseline[i];
                LibraryImageClass descriptor = ImageLibraries.LibraryImageClasses[i];
                Assert.Equal(expected.Type, descriptor.Type.FullName);
                Assert.Equal(expected.ClassId, descriptor.ClassId);
                Assert.Equal(expected.ClassVersion, descriptor.ClassVersion);
                Assert.Equal(expected.Name, descriptor.Name);
                Assert.Same(descriptor, LibraryCatalog.FindLibraryImageClass(expected.ClassId));
                Assert.Same(descriptor, LibraryImageClass.Of(descriptor.Type));
                LibraryImage image = descriptor.CreateInstance(source);
                image.ReadDiscardableImageData(stream);
                if (image is Image08 indexed)
                {
                    Palette palette = new();
                    {
                        indexed.CreateLibraryImagePalette(palette);
                    }
                }
                source.SetImage(i, image);
            }
            source.SaveToStream(stream);
            stream.Position = 0;
            loaded.LoadFromStream(stream);
            Assert.Equal(baseline.Length, loaded.Count);
            for (int i = 0; i < baseline.Length; i++)
            {
                Baseline expected = baseline[i];
                LibraryImage image = loaded[i];
                Assert.Equal(expected.Type, image.GetType().FullName);
                Assert.Equal(expected.ClassVersion, image.ClassVersion());
                Assert.Equal(expected.Name, image.Name());
                Assert.Same(loaded, image.Owner);
                Assert.Same(LibraryCatalog.FindLibraryImageClass(expected.ClassId), image.ImageClass);
                Assert.Same(image.ImageClass, image.ImageClass);
            }
            Assert.Null(LibraryCatalog.FindLibraryImageClass(-1));
            _ = Assert.Throws<InvalidOperationException>(() => ImageLibraries.RegisterLibraryImageClasses([LibraryImageClass.Of<Picture08>()]));
        }
        finally
        {
            stream.Dispose();
            loaded.Dispose();
            source.Dispose();
            ImageLibraries.LibraryImageClasses.Clear();
            ImageLibraries.LibraryImageClasses = previousClasses;
        }
    }

    [Fact]
    public void GrabInfoDescriptorCopiesTheRuntimeTypeAndAllFields()
    {
        GrabInfoClass descriptor = BitMask.GrabInfo.Descriptor;
        Assert.Equal("BitMask.GrabInfo", descriptor.Name);
        BitMask.GrabInfo value = Assert.IsType<BitMask.GrabInfo>(descriptor.CreateInstance());
        value.TransparentColor = 19;
        value.FindRectangles = true;
        value.TransparentPixel = 23;
        value.FileName = "mask.bmp";
        value.FileIndex = 7;
        value.Inverse = true;
        BitMask.GrabInfo copy = Assert.IsType<BitMask.GrabInfo>(value.Copy());
        Assert.Same(descriptor, copy.GrabInfoClass());
        Assert.Equal(value.TransparentColor, copy.TransparentColor);
        Assert.Equal(value.FindRectangles, actual: copy.FindRectangles);
        Assert.Equal(value.TransparentPixel, copy.TransparentPixel);
        Assert.Equal(value.FileName, copy.FileName);
        Assert.Equal(value.FileIndex, copy.FileIndex);
        Assert.Equal(value.Inverse, actual: copy.Inverse);
    }

    [Fact]
    public void ImageFileDescriptorKeepsIdentityAndBitmapDispatch()
    {
        ImageFileClass descriptor = ImageFileClass.Of<BmpImageFile>();
        Assert.Same(descriptor, ImageFileClass.Of<BmpImageFile>());
        Assert.Equal(typeof(BmpImageFile), descriptor.Type);
        Assert.Same(descriptor, ImageFileClass.Of(typeof(BmpImageFile)));
        Assert.Equal("BmpImageFile", descriptor.StorageKey);
        Assert.Equal("BMP", descriptor.Extension);
        Assert.Equal("Bitmap files (*.bmp)", descriptor.Description);
        _ = Assert.Throws<AbstractErrorException>(() => descriptor.Name);
        ImageFileClassList classes = [];
        _ = classes.Add(descriptor);
        Assert.Same(descriptor, classes.FindClass(".bmp"));
        Assert.Null(classes.FindClass("png"));
    }

    [Fact]
    public void GrabImagesUsesTheDescriptorCreationDelegate()
    {
        ImageLibrary library = new(null!);
        DataSurface surface = new(2, 3, pixelFormatId: PixelFormat.Rgb555, palette: null!);
        List<LibraryImageClass> previousClasses = ImageLibraries.LibraryImageClasses;
        ImageLibraries.LibraryImageClasses = [];
        ImageLibraries.RegisterLibraryImageClasses([LibraryImageClass.Of<Picture16>()]);
        LockedSurface lockedSurface = default;
        Assert.Equal(GraphicsResult.Ok, surface.Lock(ref lockedSurface));
        GrabInfo grab = new() { FileName = "grab.bmp", FindRectangles = false };
        try
        {
            Assert.Equal(GraphicsResult.Ok, LibraryImageClass.Of<Picture16>().GrabImages(lockedSurface, grabInfo: grab, imageLibrary: library, onProgress: null!));
            Picture16 image = Assert.IsType<Picture16>(library[0]);
            Assert.Equal(2, image.Width);
            Assert.Equal(3, image.Height);
            Assert.Same(library, image.Owner);
            Assert.Equal("grab.bmp", image.FileName);
        }
        finally
        {
            _ = surface.Unlock();
            library.Dispose();
            ImageLibraries.LibraryImageClasses.Clear();
            ImageLibraries.LibraryImageClasses = previousClasses;
        }
    }
}
