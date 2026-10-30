using System.Text.Json;
using Aow.Rtl;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Images = Aow.Units.ImageLib.ImageLib;
using G = Aow.Units.GFXE.GFXE;

namespace Aow.Rtl.Tests;

public sealed class ImageClassTests
{
    private sealed record Baseline(string Type, int ClassID, byte ClassVersion, string Name);

    [Fact]
    public void LibraryLoadKeepsEveryRegisteredImageClassAgainstBaseSnapshot()
    {
        // Captured from the public image-class registry at b635b9f before the descriptor edits.
        using var baselineStream = typeof(ImageClassTests).Assembly.GetManifestResourceStream("Aow.Rtl.Tests.ImageClassBaseline.json")!;
        var baseline = JsonSerializer.Deserialize<Baseline[]>(baselineStream)!;
        var previousClasses = Images.LibraryImageClasses;
        Images.LibraryImageClasses = new TList();
        var source = TImageLibrary.Create(null!);
        var loaded = TImageLibrary.Create(null!);
        var stream = new TMemoryStream();
        try
        {
            Images.RegisterLibraryImageClasses([
                LibraryImageClass.Of<TPicture08>(), LibraryImageClass.Of<TRLESprite08>(),
                LibraryImageClass.Of<TPicture16>(), LibraryImageClass.Of<TTransparentPicture16>(),
                LibraryImageClass.Of<TRLESprite16>(), LibraryImageClass.Of<TTransparentRLESprite16>(),
                LibraryImageClass.Of<TShadow>(), LibraryImageClass.Of<TImageNode>(), LibraryImageClass.Of<TSprite16>()]);
            Assert.Equal(baseline.Length, Images.LibraryImageClassCount());
            for (int i = 0; i < baseline.Length; i++)
            {
                var expected = baseline[i];
                var descriptor = Images.GetLibraryImageClass(i);
                Assert.Equal(expected.Type, descriptor.Type.FullName);
                Assert.Equal(expected.ClassID, descriptor.ClassID);
                Assert.Equal(expected.ClassVersion, descriptor.ClassVersion);
                Assert.Equal(expected.Name, descriptor.Name.ToString());
                Assert.Same(descriptor, Images.FindLibraryImageClass(expected.ClassID));
                Assert.Same(descriptor, LibraryImageClass.Of(descriptor.Type));
                var image = descriptor.Create(source);
                image.ReadDiscardableImageData(stream);
                if (image is TImage08 indexed)
                {
                    var palette = TGFXEPalette.Create();
                    try { indexed.CreateLibraryImagePalette(palette); }
                    finally { palette.Destroy(); }
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
                Assert.Equal(expected.Name, image.Name().ToString());
                Assert.Same(loaded, image.Owner);
                Assert.Same(Images.FindLibraryImageClass(expected.ClassID), image.ImageClass);
                Assert.Same(image.ImageClass, image.ImageClass);
            }
            Assert.Null(Images.FindLibraryImageClass(-1));
            Assert.Throws<Aow.Rtl.Exception>(() => Images.RegisterLibraryImageClasses([LibraryImageClass.Of<TPicture08>()]));
        }
        finally
        {
            stream.Destroy();
            loaded.Destroy();
            source.Destroy();
            Images.LibraryImageClasses.Destroy();
            Images.LibraryImageClasses = previousClasses;
        }
    }

    [Fact]
    public void GrabInfoDescriptorCopiesTheRuntimeTypeAndAllFields()
    {
        var descriptor = GrabInfoClass.Of<TBitMaskGrabInfo>();
        Assert.Same(descriptor, GrabInfoClass.Of<TBitMaskGrabInfo>());
        var value = Assert.IsType<TBitMaskGrabInfo>(descriptor.Create());
        value.TransparentColor = 19;
        value.FindRectangles = true;
        value.TransparentPixel = 23;
        value.FileName = "mask.bmp";
        value.FileIndex = 7;
        value.Inverse = true;
        var copy = Assert.IsType<TBitMaskGrabInfo>(value.Copy());
        Assert.Same(descriptor, copy.GrabInfoClass());
        Assert.Equal(value.TransparentColor, copy.TransparentColor);
        Assert.Equal(value.FindRectangles, copy.FindRectangles);
        Assert.Equal(value.TransparentPixel, copy.TransparentPixel);
        Assert.Equal(value.FileName, copy.FileName);
        Assert.Equal(value.FileIndex, copy.FileIndex);
        Assert.Equal(value.Inverse, copy.Inverse);
        copy.Destroy();
        value.Destroy();
    }

    [Fact]
    public void ImageFileDescriptorKeepsIdentityAndBitmapDispatch()
    {
        var descriptor = ImageFileClass.Of<TBMPImageFile>();
        Assert.Same(descriptor, ImageFileClass.Of<TBMPImageFile>());
        Assert.Equal(typeof(TBMPImageFile), descriptor.Type);
        Assert.Equal("BMP", descriptor.GetExtension().ToString());
        Assert.Equal("Bitmap files (*.bmp)", descriptor.GetDescription().ToString());
        Assert.Throws<EAbstractError>(() => descriptor.GetName());
        var classes = new TImageFileClassList();
        classes.Add(descriptor);
        Assert.Same(descriptor, classes.FindClass(".bmp"));
        Assert.Same(descriptor, classes.Classes[0]);
        Assert.Null(classes.FindClass("png"));
        classes.Destroy();
    }

    [Fact]
    public void GrabImagesUsesTheDescriptorCreationDelegate()
    {
        var library = TImageLibrary.Create(null!);
        var surface = TDataSurface.Create(2, 3, G.pfRGB555, null!);
        var previousClasses = Images.LibraryImageClasses;
        Images.LibraryImageClasses = new TList();
        Images.RegisterLibraryImageClasses([LibraryImageClass.Of<TPicture16>()]);
        TSurfaceDesc sd = default;
        Assert.Equal(0, surface.Lock(ref sd));
        var grab = new TGrabInfo { FileName = "grab.bmp", FindRectangles = false };
        try
        {
            Assert.Equal(0, LibraryImageClass.Of<TPicture16>().GrabImages(sd, grab, library, null!));
            var image = Assert.IsType<TPicture16>(library[0]);
            Assert.Equal(2, image.Width);
            Assert.Equal(3, image.Height);
            Assert.Same(library, image.Owner);
            Assert.Equal("grab.bmp", image.FileName.ToString());
        }
        finally
        {
            grab.Destroy();
            surface.Unlock();
            surface.Destroy();
            library.Destroy();
            Images.LibraryImageClasses.Destroy();
            Images.LibraryImageClasses = previousClasses;
        }
    }
}
