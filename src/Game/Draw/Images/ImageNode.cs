// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

public partial class ImageNode : LibraryImage, ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 256;
    static string ILibraryImageClass.Name => "";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new ImageNode(owner);

    internal LibraryImage[] _images;
    internal int _capacity;
    internal int _count;

    // ImageLib.pas:1313
    internal void SetCapacity(int newCapacity)
    {
        if (newCapacity != _capacity)
        {
            _images = ReallocArray(_images, newCapacity);
            _capacity = newCapacity;
        }
    }

    // ImageLib.pas:1322
    internal void SetCount(int newCount)
    {
        if (newCount > _capacity)
        {
            SetCapacity(newCount);
        }

        if (newCount > _count)
        {
            ClearElements(_images, _count, newCount - _count);
        }

        _count = newCount;
    }

    // ImageLib.pas:1286
    public ImageNode(CustomImageLibrary owner) : base(owner)
    {
        _fileName = "(multi images)";
    }

    // ImageLib.pas:1292
    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    // ImageLib.pas:1308
    public override string Info() => "Image Node";

    // ImageLib.pas:1330
    public LibraryImage GetImage(int index) => _images[index];

    // ImageLib.pas:1335
    public LibraryImage GetSafeImage(int index)
    {
        LibraryImage result;
        if (index >= 0 && index < _count)
        {
            result = _images[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // ImageLib.pas:1343
    public void UpdateDimensions()
    {
        _width = 0;
        _height = 0;
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            if (_images[I]._width > _width)
            {
                _width = _images[I]._width;
            }

            if (_images[I]._height > _height)
            {
                _height = _images[I]._height;
            }
        }
    }

    // ImageLib.pas:1356
    public int Add(LibraryImage image)
    {
        SetCount(_count + 1);
        _images[_count - 1] = image;
        if (image._width > _width)
        {
            _width = image._width;
        }

        if (image._height > _height)
        {
            _height = image._height;
        }

        return _count;
    }

    // ImageLib.pas:1365
    public void Insert(int index, LibraryImage image)
    {
        if (_capacity == _count)
        {
            SetCapacity(_capacity + 1);
        }

        if (index < _count)
        {
            MoveElements(_images, index, _images, index + 1, _count - index);
        }

        _images[index] = image;
        if (image._width > _width)
        {
            _width = image._width;
        }

        if (image._height > _height)
        {
            _height = image._height;
        }

        _count += 1;
    }

    // ImageLib.pas:1376
    public void Remove(LibraryImage image)
    {
        int I;
        I = _count;
        while (I >= 0 && image != _images[I])
        {
            I -= 1;
        }

        if (I >= 0)
        {
            _count -= 1;
            if (I < _count)
            {
                MoveElements(_images, I + 1, _images, I, _count - I);
            }

            UpdateDimensions();
        }
    }

    // ImageLib.pas:1391
    public int IndexOf(LibraryImage image)
    {
        int result = _count - 1;
        while (result >= 0 && image != _images[result])
        {
            result -= 1;
        }

        return result;
    }

    // ImageLib.pas:1397
    public void Delete(int index)
    {
        if (index >= 0 && index < _count)
        {
            _count -= 1;
            if (index < _count)
            {
                MoveElements(_images, index + 1, _images, index, _count - index);
            }

            UpdateDimensions();
        }
    }

    // ImageLib.pas:1408
    public override void SaveToStream(Stream stream, Stream dataStream)
    {
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            WriteImageToStream(_images[I], stream, dataStream);
        }

        WriteImageToStream(null, stream);
    }

    // ImageLib.pas:1416
    public override void SaveToStream(Stream stream)
    {
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            WriteImageToStream(_images[I], stream);
        }

        WriteImageToStream(null, stream);
    }

    // ImageLib.pas:1424
    public override void LoadFromStream(Stream stream)
    {
        LibraryImage image;
        Clear();
        image = ImageStreams.ReadImageFromStream(stream, _owner);
        while (image != null)
        {
            Add(image);
            image = ImageStreams.ReadImageFromStream(stream, _owner);
        }
    }

    // ImageLib.pas:1437
    public override void LoadFromStream(Stream stream, Stream dataStream)
    {
        LibraryImage image;
        image = ImageStreams.ReadImageFromStream(stream, dataStream, _owner);
        while (image != null)
        {
            Add(image);
            image = ImageStreams.ReadImageFromStream(stream, dataStream, _owner);
        }
    }

    // ImageLib.pas:1449
    public override void Clear()
    {
        for (int I = _count - 1; I >= 0; I--)
        {
            _images[I].Dispose();
        }

        _count = 0;
        SetCapacity(0);
    }

    // ImageLib.pas:1458
    public override void SetReloadingEnabled(bool value)
    {
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            _images[I].SetReloadingEnabled(false);
        }
    }

    // ImageLib.pas:1465
    public override void SetOwner(CustomImageLibrary owner)
    {
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            _images[I].SetOwner(owner);
        }

        _owner = owner;
    }

    // ImageLib.pas:1473
    public override int RemapToPixelFormat(int pixelFormatId)
    {
        int I;
        int result = 0;
        I = _count;
        while (I != 0 && result == 0)
        {
            I -= 1;
            result = _images[I].RemapToPixelFormat(pixelFormatId);
        }

        return result;
    }

    // ImageLib.pas:1486
    public override void Show(int X, int Y, in LockedSurface lockedSurface)
    {
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            _images[I].Show(X, Y, lockedSurface);
        }
    }

    // ImageLib.pas:1493
    public override void ShowOpaque(int X, int Y, in LockedSurface lockedSurface)
    {
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            _images[I].ShowOpaque(X, Y, lockedSurface);
        }
    }

    // ImageLib.pas:1500
    public override void ShowTransparent(int X, int Y, in LockedSurface lockedSurface)
    {
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            _images[I].ShowTransparent(X, Y, lockedSurface);
        }
    }

    // ImageLib.pas:1507
    public override void ShowBlended(int X, int Y, in LockedSurface lockedSurface, BlendTable table)
    {
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            _images[I].ShowBlended(X, Y, lockedSurface, table);
        }
    }

    // ImageLib.pas:1514
    public override void ShowDisplayController(Rect? sourceRectangle, Rect? destinationRectangle, int displayController, int bkColor)
    {
        /* TODO: Implement Windows.WindowFromDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:1550
    public override void Show(int X, int Y, in LockedSurface lockedSurface, in ShowInfo showInfo)
    {
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            _images[I].Show(X, Y, lockedSurface, showInfo);
        }
    }

    // ImageLib.pas:1557
    public override bool SupportShowMode(ShowMode showMode, int pixelFormatId)
    {
        int I;
        I = _count;
        bool result = true;
        while (result && I != 0)
        {
            I -= 1;
            result = _images[I].SupportShowMode(showMode, pixelFormatId);
        }

        return result;
    }

    // ImageLib.pas:1570
    public override void SetLoadMode(LoadMode value)
    {
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            _images[I].SetLoadMode(value);
        }

        _loadMode = value;
    }

    // ImageLib.pas:1578
    public override void ReleaseDiscardableImageData()
    {
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            _images[I].ReleaseDiscardableImageData();
        }

        _dataLoaded = false;
    }

    // ImageLib.pas:1586
    public override void LoadDiscardableImageData()
    {
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            _images[I].LoadDiscardableImageData();
        }

        _dataLoaded = true;
    }

    public int Count { get => _count; }

    public LibraryImage this[int index] { get => GetImage(index); }

}
