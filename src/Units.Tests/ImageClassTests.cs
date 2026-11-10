using System.Text.Json;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

namespace Aow.Units.Tests;

public sealed class ImageClassTests
{
    private sealed class UnregisteredImage(CustomImageLibrary owner) : Aow.Game.Draw.Images.LibraryImage(owner);

    private sealed class UnregisteredImageFile : ImageFile;

    [Fact]
    public void ImageDescriptorsRequireExplicitRegistration()
    {
        Assert.Throws<InvalidOperationException>(() => LibraryImageClass.Of<UnregisteredImage>());
        Assert.Throws<InvalidOperationException>(() => LibraryImageClass.Of(typeof(UnregisteredImage)));
        Assert.Throws<InvalidOperationException>(() => ImageFileClass.Of<UnregisteredImageFile>());
        Assert.Throws<InvalidOperationException>(() => ImageFileClass.Of(typeof(UnregisteredImageFile)));
        Assert.Throws<InvalidOperationException>(() => LibraryImageClass.Register<Picture08>());
        Assert.Throws<InvalidOperationException>(() => ImageFileClass.Register<BMPImageFile>());
        Assert.Equal("Picture08", LibraryImageClass.Of<Picture08>().ClassName);
        var types = typeof(Aow.Game.Draw.Images.LibraryImage).Assembly.GetTypes();
        Assert.Equal(types.Where(t => t.IsClass && typeof(ILibraryImageClass).IsAssignableFrom(t)).Select(t => t.FullName).Order(StringComparer.Ordinal),
            LibraryImageClass.All.Where(c => c.Type.Assembly == typeof(Aow.Game.Draw.Images.LibraryImage).Assembly).Select(c => c.Type.FullName).Order(StringComparer.Ordinal));
        Assert.Equal(types.Where(t => t.IsClass && typeof(IImageFileClass).IsAssignableFrom(t)).Select(t => t.FullName).Order(StringComparer.Ordinal),
            ImageFileClass.All.Select(c => c.Type.FullName).Order(StringComparer.Ordinal));
    }

    private sealed record Baseline(string Type, int ClassID, byte ClassVersion, string Name);

    [Fact]
    public void LibraryLoadKeepsEveryRegisteredImageClassAgainstBaseSnapshot()
    {
        // Captured from the public image-class registry at b635b9f before the descriptor edits.
        using var baselineStream = typeof(ImageClassTests).Assembly.GetManifestResourceStream("Aow.Units.Tests.ImageClassSnapshot.json")!;
        var baseline = JsonSerializer.Deserialize<Baseline[]>(baselineStream)!;
        var previousClasses = Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses;
        Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses = new DelphiList();
        var source = new ImageLibrary(null!);
        var loaded = new ImageLibrary(null!);
        var stream = new MemoryStream();
        try
        {
            Aow.Game.Draw.Images.ImageLibraries.RegisterLibraryImageClasses([
                LibraryImageClass.Of<Picture08>(), LibraryImageClass.Of<RLESprite08>(),
                LibraryImageClass.Of<Picture16>(), LibraryImageClass.Of<TransparentPicture16>(),
                LibraryImageClass.Of<RLESprite16>(), LibraryImageClass.Of<TransparentRLESprite16>(),
                LibraryImageClass.Of<Shadow>(), LibraryImageClass.Of<ImageNode>(), LibraryImageClass.Of<Sprite16>()]);
            Assert.Equal(baseline.Length, Aow.Game.Draw.Images.LibraryCatalog.LibraryImageClassCount());
            for (int i = 0; i < baseline.Length; i++)
            {
                var expected = baseline[i];
                var descriptor = Aow.Game.Draw.Images.LibraryCatalog.GetLibraryImageClass(i);
                Assert.Equal(expected.Type, descriptor.Type.FullName);
                Assert.Equal(expected.ClassID, descriptor.ClassID);
                Assert.Equal(expected.ClassVersion, descriptor.ClassVersion);
                Assert.Equal(expected.Name, descriptor.Name);
                Assert.Same(descriptor, Aow.Game.Draw.Images.LibraryCatalog.FindLibraryImageClass(expected.ClassID));
                Assert.Same(descriptor, LibraryImageClass.Of(descriptor.Type));
                var image = descriptor.Create(source);
                image.ReadDiscardableImageData(stream);
                if (image is Image08 indexed)
                {
                    var palette = new GFXEPalette();
                    { indexed.CreateLibraryImagePalette(palette); }
                }
                source.SetImage(i, image);
            }
            source.SaveToStream(stream);
            stream.Position = 0;
            loaded.LoadFromStream(stream);
            Assert.Equal(baseline.Length, loaded.Count);
            for (int i = 0; i < baseline.Length; i++)
            {
                var expected = baseline[i];
                var image = loaded[i];
                Assert.Equal(expected.Type, image.GetType().FullName);
                Assert.Equal(expected.ClassVersion, image.ClassVersion());
                Assert.Equal(expected.Name, image.Name());
                Assert.Same(loaded, image.Owner);
                Assert.Same(Aow.Game.Draw.Images.LibraryCatalog.FindLibraryImageClass(expected.ClassID), image.ImageClass);
                Assert.Same(image.ImageClass, image.ImageClass);
            }
            Assert.Null(Aow.Game.Draw.Images.LibraryCatalog.FindLibraryImageClass(-1));
            Assert.Throws<InvalidOperationException>(() => Aow.Game.Draw.Images.ImageLibraries.RegisterLibraryImageClasses([LibraryImageClass.Of<Picture08>()]));
        }
        finally
        {
            stream.Dispose();
            loaded.Dispose();
            source.Dispose();
            Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses.Dispose();
            Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses = previousClasses;
        }
    }

    [Fact]
    public void GrabInfoDescriptorCopiesTheRuntimeTypeAndAllFields()
    {
        var descriptor = Aow.Game.Draw.Images.BitMask.GrabInfo.Descriptor;
        Assert.Equal("BitMask.GrabInfo", descriptor.Name);
        var value = Assert.IsType<Aow.Game.Draw.Images.BitMask.GrabInfo>(descriptor.Create());
        value.TransparentColor = 19;
        value.FindRectangles = true;
        value.TransparentPixel = 23;
        value.FileName = "mask.bmp";
        value.FileIndex = 7;
        value.Inverse = true;
        var copy = Assert.IsType<Aow.Game.Draw.Images.BitMask.GrabInfo>(value.Copy());
        Assert.Same(descriptor, copy.GrabInfoClass());
        Assert.Equal(value.TransparentColor, copy.TransparentColor);
        Assert.Equal(value.FindRectangles, copy.FindRectangles);
        Assert.Equal(value.TransparentPixel, copy.TransparentPixel);
        Assert.Equal(value.FileName, copy.FileName);
        Assert.Equal(value.FileIndex, copy.FileIndex);
        Assert.Equal(value.Inverse, copy.Inverse);
    }

    [Fact]
    public void ImageFileDescriptorKeepsIdentityAndBitmapDispatch()
    {
        var descriptor = ImageFileClass.Of<BMPImageFile>();
        Assert.Same(descriptor, ImageFileClass.Of<BMPImageFile>());
        Assert.Equal(typeof(BMPImageFile), descriptor.Type);
        Assert.Same(descriptor, ImageFileClass.Of(typeof(BMPImageFile)));
        Assert.Equal("BMPImageFile", descriptor.ClassName);
        Assert.Equal("BMP", descriptor.Extension);
        Assert.Equal("Bitmap files (*.bmp)", descriptor.Description);
        Assert.Throws<AbstractErrorException>(() => descriptor.Name);
        using var classes = new ImageFileClassList();
        classes.Add(descriptor);
        Assert.Same(descriptor, classes.FindClass(".bmp"));
        Assert.Same(descriptor, classes.Classes[0]);
        Assert.Null(classes.FindClass("png"));
    }

    [Fact]
    public void GrabImagesUsesTheDescriptorCreationDelegate()
    {
        var library = new ImageLibrary(null!);
        var surface = new DataSurface(2, 3, Aow.Game.Draw.PixelFormats.pfRGB555, null!);
        var previousClasses = Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses;
        Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses = new DelphiList();
        Aow.Game.Draw.Images.ImageLibraries.RegisterLibraryImageClasses([LibraryImageClass.Of<Picture16>()]);
        SurfaceDesc sd = default;
        Assert.Equal(0, surface.Lock(ref sd));
        var grab = new GrabInfo { FileName = "grab.bmp", FindRectangles = false };
        try
        {
            Assert.Equal(0, LibraryImageClass.Of<Picture16>().GrabImages(sd, grab, library, null!));
            var image = Assert.IsType<Picture16>(library[0]);
            Assert.Equal(2, image.Width);
            Assert.Equal(3, image.Height);
            Assert.Same(library, image.Owner);
            Assert.Equal("grab.bmp", image.FileName);
        }
        finally
        {
            surface.Unlock();
            surface.Dispose();
            library.Dispose();
            Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses.Dispose();
            Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses = previousClasses;
        }
    }
}
