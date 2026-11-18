#nullable disable
// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

public partial class ImageNode : LibraryImage, ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 256;
    static string ILibraryImageClass.Name => "";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new ImageNode(owner);

    internal LibraryImage[] Images { get; private set; }
    internal int Capacity { get; private set; }

    // ImageLib.pas:1313
    internal void SetCapacity(int newCapacity)
    {
        if (newCapacity != Capacity)
        {
            Images = ReallocArray(Images, newCapacity);
            Capacity = newCapacity;
        }
    }

    // ImageLib.pas:1322
    internal void SetCount(int newCount)
    {
        if (newCount > Capacity)
        {
            SetCapacity(newCount);
        }

        if (newCount > Count)
        {
            ClearElements(Images, Count, newCount - Count);
        }

        Count = newCount;
    }

    // ImageLib.pas:1286
    public ImageNode(CustomImageLibrary owner) : base(owner) => FileName = "(multi images)";

    // ImageLib.pas:1292
    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    // ImageLib.pas:1308
    public override string Info() => "Image Node";

    // ImageLib.pas:1330
    public LibraryImage GetImage(int index) => Images[index];

    // ImageLib.pas:1335
    public LibraryImage GetSafeImage(int index)
    {
        LibraryImage result = index >= 0 && index < Count ? Images[index] : null;
        return result;
    }

    // ImageLib.pas:1343
    public void UpdateDimensions()
    {
        SetWidthWithoutOverrides(0);
        SetHeightWithoutOverrides(0);
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            if (Images[i].Width > Width)
            {
                SetWidthWithoutOverrides(Images[i].Width);
            }

            if (Images[i].Height > Height)
            {
                SetHeightWithoutOverrides(Images[i].Height);
            }
        }
    }

    // ImageLib.pas:1356
    public int Add(LibraryImage image)
    {
        SetCount(Count + 1);
        Images[Count - 1] = image;
        if (image.Width > Width)
        {
            SetWidthWithoutOverrides(image.Width);
        }

        if (image.Height > Height)
        {
            SetHeightWithoutOverrides(image.Height);
        }

        return Count;
    }

    // ImageLib.pas:1365
    public void Insert(int index, LibraryImage image)
    {
        if (Capacity == Count)
        {
            SetCapacity(Capacity + 1);
        }

        if (index < Count)
        {
            MoveElements(Images, index, Images, index + 1, Count - index);
        }

        Images[index] = image;
        if (image.Width > Width)
        {
            SetWidthWithoutOverrides(image.Width);
        }

        if (image.Height > Height)
        {
            SetHeightWithoutOverrides(image.Height);
        }

        Count += 1;
    }

    // ImageLib.pas:1376
    public void Remove(LibraryImage image)
    {
        int i;
        i = Count;
        while (i >= 0 && image != Images[i])
        {
            i -= 1;
        }

        if (i >= 0)
        {
            Count -= 1;
            if (i < Count)
            {
                MoveElements(Images, i + 1, Images, i, Count - i);
            }

            UpdateDimensions();
        }
    }

    // ImageLib.pas:1391
    public int IndexOf(LibraryImage image)
    {
        int result = Count - 1;
        while (result >= 0 && image != Images[result])
        {
            result -= 1;
        }

        return result;
    }

    // ImageLib.pas:1397
    public void Delete(int index)
    {
        if (index >= 0 && index < Count)
        {
            Count -= 1;
            if (index < Count)
            {
                MoveElements(Images, index + 1, Images, index, Count - index);
            }

            UpdateDimensions();
        }
    }

    // ImageLib.pas:1408
    public override void SaveToStream(Stream stream, Stream dataStream)
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            WriteImageToStream(Images[i], stream, dataStream);
        }

        WriteImageToStream(null, stream);
    }

    // ImageLib.pas:1416
    public override void SaveToStream(Stream stream)
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            WriteImageToStream(Images[i], stream);
        }

        WriteImageToStream(null, stream);
    }

    // ImageLib.pas:1424
    public override void LoadFromStream(Stream stream)
    {
        LibraryImage image;
        Clear();
        image = ImageStreams.ReadImageFromStream(stream, Owner);
        while (image != null)
        {
            _ = Add(image);
            image = ImageStreams.ReadImageFromStream(stream, Owner);
        }
    }

    // ImageLib.pas:1437
    public override void LoadFromStream(Stream stream, Stream dataStream)
    {
        LibraryImage image;
        image = ImageStreams.ReadImageFromStream(stream, dataStream, Owner);
        while (image != null)
        {
            _ = Add(image);
            image = ImageStreams.ReadImageFromStream(stream, dataStream, Owner);
        }
    }

    // ImageLib.pas:1449
    public override void Clear()
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            Images[i].Dispose();
        }

        Count = 0;
        SetCapacity(0);
    }

    // ImageLib.pas:1458
    public override void SetReloadingEnabled(bool value)
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            Images[i].SetReloadingEnabled(false);
        }
    }

    // ImageLib.pas:1465
    public override void SetOwner(CustomImageLibrary owner)
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            Images[i].SetOwner(owner);
        }

        SetOwnerWithoutReloadCheck(owner);
    }

    // ImageLib.pas:1473
    public override GraphicsResult RemapToPixelFormat(PixelFormat pixelFormatId)
    {
        int i;
        GraphicsResult result = GraphicsResult.Ok;
        i = Count;
        while (i != 0 && result == GraphicsResult.Ok)
        {
            i -= 1;
            result = Images[i].RemapToPixelFormat(pixelFormatId);
        }

        return result;
    }

    // ImageLib.pas:1486
    public override void Show(int X, int Y, in LockedSurface lockedSurface)
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            Images[i].Show(X, Y, lockedSurface);
        }
    }

    // ImageLib.pas:1493
    public override void ShowOpaque(int X, int Y, in LockedSurface lockedSurface)
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            Images[i].ShowOpaque(X, Y, lockedSurface);
        }
    }

    // ImageLib.pas:1500
    public override void ShowTransparent(int X, int Y, in LockedSurface lockedSurface)
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            Images[i].ShowTransparent(X, Y, lockedSurface);
        }
    }

    // ImageLib.pas:1507
    public override void ShowBlended(int X, int Y, in LockedSurface lockedSurface, BlendTable table)
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            Images[i].ShowBlended(X, Y, lockedSurface, table);
        }
    }

    // ImageLib.pas:1514
    public override void ShowDisplayController(Rect? sourceRectangle, Rect? destinationRectangle, int displayController, int bkColor) =>
        /* TODO: Implement Windows.WindowFromDC in the platform or editor service. */
        throw new NotImplementedException();

    // ImageLib.pas:1550
    public override void Show(int X, int Y, in LockedSurface lockedSurface, in ShowInfo showInfo)
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            Images[i].Show(X, Y, lockedSurface, showInfo);
        }
    }

    // ImageLib.pas:1557
    public override bool SupportShowMode(ShowMode showMode, PixelFormat pixelFormatId)
    {
        int i;
        i = Count;
        bool result = true;
        while (result && i != 0)
        {
            i -= 1;
            result = Images[i].SupportShowMode(showMode, pixelFormatId);
        }

        return result;
    }

    // ImageLib.pas:1570
    public override void SetLoadMode(LoadMode value)
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            Images[i].SetLoadMode(value);
        }

        SetLoadModeWithoutOverrides(value);
    }

    // ImageLib.pas:1578
    public override void ReleaseDiscardableImageData()
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            Images[i].ReleaseDiscardableImageData();
        }

        DataLoaded = false;
    }

    // ImageLib.pas:1586
    public override void LoadDiscardableImageData()
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            Images[i].LoadDiscardableImageData();
        }

        DataLoaded = true;
    }

    public int Count { get; internal set; }

    public LibraryImage this[int index] => GetImage(index);

}
