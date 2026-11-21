#nullable disable
// Translated from ImageLib.pas.
using System.Globalization;
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

// an image of an image library. FOwner, FDataLoaded (the discardable data, the pixels, are in
// memory), FDataPosition (of the discardable data in the library file, -1 for none), FDataSize,
// FReloadingEnabled, FVersion, FLoadMode, FDataWidth, FDataHeight (of the pixels), FWidth,
// FHeight (the visual size), FXshift, FYshift (of the pixels in the visual rectangle),
// FFileName, FFileIndex (where the image was grabbed), FShowInfo
public class LibraryImage : Persistent, ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 0;
    static byte ILibraryImageClass.ClassVersion => 3;
    static string ILibraryImageClass.Name => "Abstract Library Image";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new(owner);

    public LibraryImageClass ImageClass => field ??= LibraryImageClass.Of(GetType());

    public virtual byte ClassVersion() => ImageClass.ClassVersion;
    public virtual string Name() => ImageClass.Name;

    private CustomImageLibrary _owner;
    private int _dataPosition;
    private int _dataSize;
    private bool _reloadingEnabled;
    private byte _version;
    private LoadMode _loadMode;
    private int _dataWidth;
    private int _dataHeight;
    private int _width;
    private int _height;
    private int _xshift;
    private int _yshift;
    private string _fileName = "";
    private int _fileIndex;

    internal ShowInfo ShowInfoValue { get; set; }

    // ImageLib.pas:1613
    public virtual void SetOwner(CustomImageLibrary owner)
    {
        if (owner != _owner && _owner is not null && !DataLoaded)
        {
            SetReloadingEnabled(false);
        }

        _owner = owner;
    }

    // ImageLib.pas:1877
    public virtual void SetLoadMode(LoadMode value)
    {
        _loadMode = value;
        if (_loadMode == LoadMode.LoadAlways && !DataLoaded)
        {
            LoadDiscardableImageData();
        }
    }

    // ImageLib.pas:1883
    public virtual Palette GetPalette() => null;

    // ImageLib.pas:1888
    public virtual Palette RemapPalette => null;

    // ImageLib.pas:1893
    public virtual bool Transparent => false;

    // ImageLib.pas:1656
    public virtual void SetReloadingEnabled(bool value)
    {
        if (!value && !DataLoaded)
        {
            LoadDiscardableImageData();
        }

        _reloadingEnabled = value && !_reloadingEnabled ? throw new InvalidOperationException("Cannot turn on Reloading when disabled") : value;
    }

    // ImageLib.pas:1991
    public virtual void SetDefaultShowInfo(ShowInfo value)
    {
        if (SupportShowMode(value.ShowMode, pixelFormatId: 0))
        {
            ShowInfoValue = value;
        }
    }

    // ImageLib.pas:1620
    public virtual void SetXshift(int value) => _xshift = value;

    // ImageLib.pas:1625
    public virtual void SetYshift(int value) => _yshift = value;

    // ImageLib.pas:1630
    public virtual void SetWidth(int value) => _width = value;

    // ImageLib.pas:1635
    public virtual void SetHeight(int value) => _height = value;

    // ImageLib.pas:1640
    public virtual Rect GetVisualDimensions() => new() { Left = 0, Right = _width, Top = 0, Bottom = _height };

    // ImageLib.pas:1702
    public virtual void ReadImageData(Stream stream)
    {
        stream.ReadExactly(Bytes.Of(ref _version));
        LibraryDrawing.ReadStringFromStream(ref _fileName, stream);
        stream.ReadExactly(Bytes.Of(ref _dataWidth));
        stream.ReadExactly(Bytes.Of(ref _dataHeight));
        stream.ReadExactly(Bytes.Of(ref _xshift));
        stream.ReadExactly(Bytes.Of(ref _yshift));
        stream.ReadExactly(Bytes.Of(ref _fileIndex));
        stream.ReadExactly(Bytes.Of(ref _loadMode));
        stream.ReadExactly(Bytes.Of(ref _dataSize));
        if (_version > 0)
        {
            if (_version >= 2)
            {
                stream.ReadExactly(Bytes.Of(ref _dataPosition));
            }

            stream.ReadExactly(Bytes.Of(ref _width));
            stream.ReadExactly(Bytes.Of(ref _height));
        }
        else
        {
            _width = _dataWidth + _xshift;
            _height = _dataHeight + _yshift;
        }

        if (_version >= 3)
        {
            ShowInfo showInfoValue = ShowInfoValue;
            _ = showInfoValue.ReadImage(stream, exact: false);
            ShowInfoValue = showInfoValue;
        }
    }

    // ImageLib.pas:1727
    public virtual void WriteImageData(Stream stream)
    {
        byte v = ClassVersion();
        stream.Write(Bytes.Of(ref v));
        LibraryDrawing.WriteStringToStream(_fileName, stream);
        stream.Write(Bytes.Of(ref _dataWidth));
        stream.Write(Bytes.Of(ref _dataHeight));
        stream.Write(Bytes.Of(ref _xshift));
        stream.Write(Bytes.Of(ref _yshift));
        stream.Write(Bytes.Of(ref _fileIndex));
        stream.Write(Bytes.Of(ref _loadMode));
        stream.Write(Bytes.Of(ref _dataSize));
        stream.Write(Bytes.Of(ref _dataPosition));
        stream.Write(Bytes.Of(ref _width));
        stream.Write(Bytes.Of(ref _height));
        ShowInfo showInfoValue = ShowInfoValue;
        _ = showInfoValue.WriteImage(stream, exact: false);
        ShowInfoValue = showInfoValue;
    }

    // ImageLib.pas:1747
    public virtual void ReadDiscardableImageData(Stream stream) => DataLoaded = true;

    // ImageLib.pas:1752
    public virtual void WriteDiscardableImageData(Stream stream)
    {
    }

    // ImageLib.pas:1756
    public virtual void ReleaseDiscardableImageData() => DataLoaded = false;

    // ImageLib.pas:1761
    public virtual void LoadDiscardableImageData()
    {
        if (!DataLoaded)
        {
            Stream stream = new FastLoadFileStream(_owner.LoadedFileName);
            try
            {
                _ = stream.Seek(_dataPosition + _owner.DataOffset, origin: SeekOrigin.Begin);
                ReadDiscardableImageData(stream);
                DataLoaded = true;
            }
            finally
            {
                stream?.Dispose();
            }
        }
    }

    // ImageLib.pas:1859
    public virtual void SaveToStream(Stream stream, Stream dataStream)
    {
        int p = _dataPosition;
        _dataPosition = (int)dataStream.Position;
        WriteImageData(stream);
        _dataPosition = p;
        if (!DataLoaded)
        {
            LoadDiscardableImageData();
            WriteDiscardableImageData(dataStream);
            if (_loadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            WriteDiscardableImageData(dataStream);
        }
    }

    // ImageLib.pas:1835
    public virtual void LoadFromStream(Stream stream, Stream dataStream)
    {
        ReadImageData(stream);
        if (_version >= 2)
        {
            if (_loadMode == LoadMode.LoadAlways || _owner is null || !_owner.Reloading || !_owner.Status.Contains(ImageLibraryStatuses.LoadingFromFile) || _dataPosition == -1)
            {
                if (_dataPosition != -1)
                {
                    dataStream.Position = _owner.DataOffset + _dataPosition;
                    ReadDiscardableImageData(dataStream);
                }
                else
                {
                    ReadDiscardableImageData(stream);
                }

                _reloadingEnabled = false;
            }
            else
            {
                _reloadingEnabled = true;
            }
        }
        else
        {
            throw new ArgumentException("Invalid image format");
        }
    }

    // ImageLib.pas:1985
    public virtual GraphicsResult RemapToPixelFormat(PixelFormat pixelFormatId) => throw new NotSupportedException("Surface format not supported");

    // ImageLib.pas:1685
    public virtual void Clear()
    {
        _version = ClassVersion();
        ReleaseDiscardableImageData();
        _dataSize = 0;
        DataLoaded = false;
        _dataPosition = -1;
        _dataSize = 0;
        _reloadingEnabled = false;
        _xshift = 0;
        _dataWidth = 0;
        _width = 0;
        _yshift = 0;
        _dataHeight = 0;
        _height = 0;
    }

    // ImageLib.pas:1596
    public LibraryImage(CustomImageLibrary owner)
    {
        _dataPosition = -1;
        _reloadingEnabled = false;
        _loadMode = LoadMode.LoadOnDemand;
        ShowInfo showInfoValue = ShowInfoValue;
        showInfoValue.ShowMode = ShowMode.Opaque;
        ShowInfoValue = showInfoValue;
        ShowInfo showInfoValue2 = ShowInfoValue;
        showInfoValue2.BlendMode = BlendMode.Alpha;
        ShowInfoValue = showInfoValue2;
        SetOwner(owner);
    }

    // ImageLib.pas:1979
    public virtual string Info()
    {
        string result = $"{Name()} {_width.ToString(CultureInfo.InvariantCulture)}x{_height.ToString(CultureInfo.InvariantCulture)}";
        if (Transparent)
        {
            result += " Transparent";
        }

        return result;
    }

    // ImageLib.pas:1670
    public virtual LibraryImage Copy()
    {
        MemoryStream stream = new();
        LibraryImage result;
        try
        {
            WriteImageToStream(this, stream);
            stream.Position = 0;
            result = ImageStreams.ReadImageFromStream(stream, _owner);
            result.SetOwner(null);
        }
        finally
        {
            stream?.Dispose();
        }

        return result;
    }

    // ImageLib.pas:1778
    public virtual void SaveToStream(Stream stream)
    {
        int p = _dataPosition;
        _dataPosition = -1;
        WriteImageData(stream);
        _dataPosition = p;
        if (!DataLoaded)
        {
            LoadDiscardableImageData();
            WriteDiscardableImageData(stream);
            if (_loadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            WriteDiscardableImageData(stream);
        }
    }

    // ImageLib.pas:1796
    public virtual void LoadFromStream(Stream stream)
    {
        Clear();
        ReadImageData(stream);
        if (_version < 2)
        {
            _dataPosition = (int)stream.Position;
            if (_loadMode == LoadMode.LoadAlways || _owner is null || !_owner.Reloading || !_owner.Status.Contains(ImageLibraryStatuses.LoadingFromFile))
            {
                ReadDiscardableImageData(stream);
                _reloadingEnabled = false;
            }
            else
            {
                _ = stream.Seek(_dataSize, origin: SeekOrigin.Current);
                _reloadingEnabled = true;
            }
        }
        else
        {
            if (_loadMode == LoadMode.LoadAlways || _owner is null || !_owner.Reloading || !_owner.Status.Contains(ImageLibraryStatuses.LoadingFromFile) || _dataPosition == -1)
            {
                if (_dataPosition != -1)
                {
                    int p = (int)stream.Position;
                    stream.Position = _owner.DataOffset + _dataPosition;
                    ReadDiscardableImageData(stream);
                    stream.Position = p;
                }
                else
                {
                    ReadDiscardableImageData(stream);
                }

                _reloadingEnabled = false;
            }
            else
            {
                _reloadingEnabled = true;
            }
        }
    }

    // ImageLib.pas:1996
    public virtual void Show(int X, int Y, in LockedSurface lockedSurface)
    {
        switch (ShowInfoValue.ShowMode)
        {
            case ShowMode.Opaque:
                ShowOpaque(X, Y, lockedSurface);
                break;
            case ShowMode.Transparent:
                ShowTransparent(X, Y, lockedSurface);
                break;
            case ShowMode.Blended:
                switch (ShowInfoValue.BlendMode)
                {
                    case BlendMode.User:
                        if (ShowInfoValue.Blend is BlendTable table)
                        {
                            ShowBlended(X, Y, lockedSurface, table);
                        }

                        break;
                    case BlendMode.Alpha:
                        ShowBlended(X, Y, lockedSurface, table: GraphicsState.AlphaBlendTable.Lookup[ShowInfoValue.Blend.Percentage]);
                        break;
                    case BlendMode.Brighten:
                        ShowBlended(X, Y, lockedSurface, table: GraphicsState.BrightenBlendTable.Lookup[ShowInfoValue.Blend.Percentage]);
                        break;
                    case BlendMode.Intensity:
                        ShowBlended(X, Y, lockedSurface, table: GraphicsState.IntensityBlendTable.Lookup[ShowInfoValue.Blend.Percentage]);
                        break;
                    case BlendMode.Shadow:
                        ShowBlended(X, Y, lockedSurface, table: GraphicsState.ShadowBlendTable.Lookup[ShowInfoValue.Blend.Percentage]);
                        break;
                    case BlendMode.LinearAlpha:
                        ShowBlended(X, Y, lockedSurface, table: GraphicsState.LinearAlphaBlendTable.Lookup[ShowInfoValue.Blend.Percentage]);
                        break;
                    default:
                        break;
                }

                break;
            default:
                break;
        }
    }

    // ImageLib.pas:2040
    public virtual void ShowStretched(in Rect r, in LockedSurface lockedSurface)
    {
        switch (ShowInfoValue.ShowMode)
        {
            case ShowMode.Opaque:
                ShowOpaqueStretched(r, lockedSurface);
                break;
            case ShowMode.Blended:
                switch (ShowInfoValue.BlendMode)
                {
                    case BlendMode.User:
                        if (ShowInfoValue.Blend is BlendTable table)
                        {
                            ShowBlendedStretched(r, lockedSurface, table);
                        }

                        break;
                    case BlendMode.Alpha:
                        ShowBlendedStretched(r, lockedSurface, table: GraphicsState.AlphaBlendTable.Lookup[ShowInfoValue.Blend.Percentage]);
                        break;
                    case BlendMode.Brighten:
                        ShowBlendedStretched(r, lockedSurface, table: GraphicsState.BrightenBlendTable.Lookup[ShowInfoValue.Blend.Percentage]);
                        break;
                    case BlendMode.Intensity:
                        ShowBlendedStretched(r, lockedSurface, table: GraphicsState.IntensityBlendTable.Lookup[ShowInfoValue.Blend.Percentage]);
                        break;
                    case BlendMode.Shadow:
                        ShowBlendedStretched(r, lockedSurface, table: GraphicsState.ShadowBlendTable.Lookup[ShowInfoValue.Blend.Percentage]);
                        break;
                    case BlendMode.LinearAlpha:
                        ShowBlendedStretched(r, lockedSurface, table: GraphicsState.LinearAlphaBlendTable.Lookup[ShowInfoValue.Blend.Percentage]);
                        break;
                    default:
                        break;
                }

                break;
            case ShowMode.Transparent:
            default:
                break;
        }
    }

    // ImageLib.pas:2066
    public virtual void ShowOpaque(int X, int Y, in LockedSurface lockedSurface)
    {
    }

    // ImageLib.pas:2070
    public virtual void ShowOpaqueStretched(in Rect R, in LockedSurface lockedSurface)
    {
    }

    // ImageLib.pas:2078
    public virtual void ShowBlended(int X, int Y, in LockedSurface lockedSurface, BlendTable table)
    {
    }

    // ImageLib.pas:2082
    public virtual void ShowBlendedStretched(in Rect R, in LockedSurface lockedSurface, BlendTable table)
    {
    }

    // ImageLib.pas:2074
    public virtual void ShowTransparent(int X, int Y, in LockedSurface lockedSurface)
    {
    }

    // ImageLib.pas:2020
    // { ShowEx: the blend mode comes from the image's own show info (an original bug: ShowInfo only
    //   gives the show mode and the blend value), and bmLinearAlpha is not handled. }
    public virtual void Show(int X, int Y, in LockedSurface lockedSurface, in ShowInfo showInfo)
    {
        switch (showInfo.ShowMode)
        {
            case ShowMode.Opaque:
                ShowOpaque(X, Y, lockedSurface);
                break;
            case ShowMode.Transparent:
                ShowTransparent(X, Y, lockedSurface);
                break;
            case ShowMode.Blended:
                switch (ShowInfoValue.BlendMode)
                {
                    case BlendMode.User:
                        if (showInfo.Blend is BlendTable table)
                        {
                            ShowBlended(X, Y, lockedSurface, table);
                        }

                        break;
                    case BlendMode.Alpha:
                        ShowBlended(X, Y, lockedSurface, table: GraphicsState.AlphaBlendTable.Lookup[showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Brighten:
                        ShowBlended(X, Y, lockedSurface, table: GraphicsState.BrightenBlendTable.Lookup[showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Intensity:
                        ShowBlended(X, Y, lockedSurface, table: GraphicsState.IntensityBlendTable.Lookup[showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Shadow:
                        ShowBlended(X, Y, lockedSurface, table: GraphicsState.ShadowBlendTable.Lookup[showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.LinearAlpha:
                    default:
                        break;
                }

                break;
            default:
                break;
        }
    }

    // ImageLib.pas:2087
    public virtual void ShowDisplayController(Rect? sourceRectangle, Rect? destinationRectangle, int displayController, int bkColor)
    {
    }

    // ImageLib.pas:2091
    public virtual bool SupportShowMode(ShowMode showMode, PixelFormat pixelFormatId) => false;

    // ImageLib.pas:1903
    public virtual bool SupportGrabPixelFormat(PixelFormat pixelFormatId) => false;

    // ImageLib.pas:1914
    public virtual bool GrabImage(in LockedSurface lockedSurface, in Rect R, GrabInfo grabInfo) => false;

    // ImageLib.pas:1920
    public virtual GrabInfo CreateGrabInfo() => new();

    // ImageLib.pas:1607
    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    // ImageLib.pas:1648
    public Rect GetDimensions() => new() { Left = 0, Right = _width, Top = 0, Bottom = _height };

    // ImageLib.pas:2061
    public void ShowClipped(int X, int Y, in LockedSurface lockedSurface) => Show(X, Y, lockedSurface);

    public CustomImageLibrary Owner { get => _owner; set => SetOwner(value); }

    protected void SetOwnerWithoutReloadCheck(CustomImageLibrary value) => _owner = value;

    public bool DataLoaded { get; protected set; }
    public int DataPosition => _dataPosition;
    public int DataSize { get => _dataSize; protected set => _dataSize = value; }
    public bool ReloadingEnabled { get => _reloadingEnabled; set => SetReloadingEnabled(value); }
    public byte Version => _version;
    public LoadMode LoadMode { get => _loadMode; set => SetLoadMode(value); }

    protected void SetLoadModeWithoutOverrides(LoadMode value) => _loadMode = value;

    public int DataWidth { get => _dataWidth; protected set => _dataWidth = value; }
    public int DataHeight { get => _dataHeight; protected set => _dataHeight = value; }
    public int Width { get => _width; set => SetWidth(value); }

    protected void SetWidthWithoutOverrides(int value) => _width = value;

    public int Height { get => _height; set => SetHeight(value); }

    protected void SetHeightWithoutOverrides(int value) => _height = value;

    public int Xshift { get => _xshift; set => SetXshift(value); }
    public int Yshift { get => _yshift; set => SetYshift(value); }
    public string FileName { get => _fileName; protected set => _fileName = value; }
    public int FileIndex { get => _fileIndex; protected set => _fileIndex = value; }
    public ShowInfo ShowInfo { get => ShowInfoValue; internal set => SetDefaultShowInfo(value); }
    public Palette Palette => GetPalette();

    public static void WriteImageToStream(LibraryImage image, Stream stream)
    {
        int id;
        if (image is not null)
        {
            id = image.ImageClass.ClassId;
            stream.Write(Bytes.Of(ref id));
            image.SaveToStream(stream);
        }
        else
        {
            id = 0;
            stream.Write(Bytes.Of(ref id));
        }
    }

    internal static void WriteImageToStream(LibraryImage image, Stream stream, Stream dataStream)
    {
        int id;
        if (image is not null)
        {
            id = image.ImageClass.ClassId;
            stream.Write(Bytes.Of(ref id));
            image.SaveToStream(stream, dataStream);
        }
        else
        {
            id = 0;
            stream.Write(Bytes.Of(ref id));
        }
    }
}
