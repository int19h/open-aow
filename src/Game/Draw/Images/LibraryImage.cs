// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// an image of an image library. FOwner, FDataLoaded (the discardable data, the pixels, are in
// memory), FDataPosition (of the discardable data in the library file, -1 for none), FDataSize,
// FReloadingEnabled, FVersion, FLoadMode, FDataWidth, FDataHeight (of the pixels), FWidth,
// FHeight (the visual size), FXshift, FYshift (of the pixels in the visual rectangle),
// FFileName, FFileIndex (where the image was grabbed), FShowInfo
public partial class LibraryImage : Persistent, ILibraryImageClass
{
    static int ILibraryImageClass.ClassId => 0;
    static byte ILibraryImageClass.ClassVersion => 3;
    static string ILibraryImageClass.Name => "Abstract Library Image";

    static LibraryImage ILibraryImageClass.CreateInstance(CustomImageLibrary owner) => new LibraryImage(owner);

    private LibraryImageClass _imageClass;

    public LibraryImageClass ImageClass => _imageClass ??= LibraryImageClass.Of(GetType());

    public virtual byte ClassVersion() => ImageClass.ClassVersion;
    public virtual string Name() => ImageClass.Name;

    internal CustomImageLibrary _owner;
    internal bool _dataLoaded;
    internal int _dataPosition;
    internal int _dataSize;
    internal bool _reloadingEnabled;
    internal byte _version;
    internal LoadMode _loadMode;
    internal int _dataWidth;
    internal int _dataHeight;
    internal int _width;
    internal int _height;
    internal int _xshift;
    internal int _yshift;
    internal string _fileName = "";
    internal int _fileIndex;
    internal ShowInfo _showInfo;

    // ImageLib.pas:1613
    public virtual void SetOwner(CustomImageLibrary owner)
    {
        if (owner != _owner && _owner != null && !_dataLoaded)
        {
            SetReloadingEnabled(false);
        }

        _owner = owner;
    }

    // ImageLib.pas:1877
    public virtual void SetLoadMode(LoadMode value)
    {
        _loadMode = value;
        if (_loadMode == LoadMode.LoadAlways && !_dataLoaded)
        {
            LoadDiscardableImageData();
        }
    }

    // ImageLib.pas:1883
    public virtual Palette GetPalette() => null;

    // ImageLib.pas:1888
    public virtual Palette GetRemapPalette() => null;

    // ImageLib.pas:1893
    public virtual bool GetTransparent() => false;

    // ImageLib.pas:1656
    public virtual void SetReloadingEnabled(bool value)
    {
        if (!value && !_dataLoaded)
        {
            LoadDiscardableImageData();
        }

        if (value && !_reloadingEnabled)
        {
            throw new InvalidOperationException("Cannot turn on Reloading when disabled");
        }
        else
        {
            _reloadingEnabled = value;
        }
    }

    // ImageLib.pas:1991
    public virtual void SetDefaultShowInfo(ShowInfo value)
    {
        if (SupportShowMode(value.ShowMode, 0))
        {
            _showInfo = value;
        }
    }

    // ImageLib.pas:1620
    public virtual void SetXshift(int value)
    {
        _xshift = value;
    }

    // ImageLib.pas:1625
    public virtual void SetYshift(int value)
    {
        _yshift = value;
    }

    // ImageLib.pas:1630
    public virtual void SetWidth(int value)
    {
        _width = value;
    }

    // ImageLib.pas:1635
    public virtual void SetHeight(int value)
    {
        _height = value;
    }

    // ImageLib.pas:1640
    public virtual void GetVisualDimensions(ref Rect R)
    {
        R.Left = 0;
        R.Right = _width;
        R.Top = 0;
        R.Bottom = _height;
    }

    // ImageLib.pas:1702
    public virtual void ReadImageData(Stream stream)
    {
        stream.ReadExactly(Bytes.Of(ref _version, 1));
        LibraryDrawing.ReadStringFromStream(ref _fileName, stream);
        stream.ReadExactly(Bytes.Of(ref _dataWidth, 4));
        stream.ReadExactly(Bytes.Of(ref _dataHeight, 4));
        stream.ReadExactly(Bytes.Of(ref _xshift, 4));
        stream.ReadExactly(Bytes.Of(ref _yshift, 4));
        stream.ReadExactly(Bytes.Of(ref _fileIndex, 4));
        stream.ReadExactly(Bytes.Of(ref _loadMode, 1));
        stream.ReadExactly(Bytes.Of(ref _dataSize, 4));
        if (_version > 0)
        {
            if (_version >= 2)
            {
                stream.ReadExactly(Bytes.Of(ref _dataPosition, 4));
            }

            stream.ReadExactly(Bytes.Of(ref _width, 4));
            stream.ReadExactly(Bytes.Of(ref _height, 4));
        }
        else
        {
            _width = _dataWidth + _xshift;
            _height = _dataHeight + _yshift;
        }

        if (_version >= 3)
        {
            _showInfo.ReadImage(stream, false);
        }
    }

    // ImageLib.pas:1727
    public virtual void WriteImageData(Stream stream)
    {
        byte V;
        V = ClassVersion();
        stream.Write(Bytes.Of(ref V, 1));
        LibraryDrawing.WriteStringToStream(_fileName, stream);
        stream.Write(Bytes.Of(ref _dataWidth, 4));
        stream.Write(Bytes.Of(ref _dataHeight, 4));
        stream.Write(Bytes.Of(ref _xshift, 4));
        stream.Write(Bytes.Of(ref _yshift, 4));
        stream.Write(Bytes.Of(ref _fileIndex, 4));
        stream.Write(Bytes.Of(ref _loadMode, 1));
        stream.Write(Bytes.Of(ref _dataSize, 4));
        stream.Write(Bytes.Of(ref _dataPosition, 4));
        stream.Write(Bytes.Of(ref _width, 4));
        stream.Write(Bytes.Of(ref _height, 4));
        _showInfo.WriteImage(stream, false);
    }

    // ImageLib.pas:1747
    public virtual void ReadDiscardableImageData(Stream stream)
    {
        _dataLoaded = true;
    }

    // ImageLib.pas:1752
    public virtual void WriteDiscardableImageData(Stream stream)
    {
    }

    // ImageLib.pas:1756
    public virtual void ReleaseDiscardableImageData()
    {
        _dataLoaded = false;
    }

    // ImageLib.pas:1761
    public virtual void LoadDiscardableImageData()
    {
        Stream stream;
        if (!_dataLoaded)
        {
            stream = new FastLoadFileStream(_owner._loadedFileName);
            try
            {
                stream.Seek(_dataPosition + _owner._dataOffset, SeekOrigin.Begin);
                ReadDiscardableImageData(stream);
                _dataLoaded = true;
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
        int P;
        P = _dataPosition;
        _dataPosition = (int)(dataStream.Position);
        WriteImageData(stream);
        _dataPosition = P;
        if (!_dataLoaded)
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
            if (_loadMode == LoadMode.LoadAlways || _owner == null || !_owner._reloading || !_owner._status.Contains(ImageLibraryStatusElement.LoadingFromFile) || _dataPosition == -1)
            {
                if (_dataPosition != -1)
                {
                    dataStream.Position = _owner._dataOffset + _dataPosition;
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
    public virtual int RemapToPixelFormat(int pixelFormatId)
    {
        throw new NotSupportedException("Surface format not supported");
    }

    // ImageLib.pas:1685
    public virtual void Clear()
    {
        _version = ClassVersion();
        ReleaseDiscardableImageData();
        _dataSize = 0;
        _dataLoaded = false;
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
        _showInfo.ShowMode = ShowMode.Opaque;
        _showInfo.BlendMode = BlendMode.Alpha;
        SetOwner(owner);
    }

    // ImageLib.pas:1979
    public virtual string Info()
    {
        string result = Name() + " " + IntToStr(_width) + "x" + IntToStr(_height);
        if (GetTransparent())
        {
            result = result + " Transparent";
        }

        return result;
    }

    // ImageLib.pas:1670
    public virtual LibraryImage Copy()
    {
        LibraryImage result = default;
        MemoryStream stream;
        stream = new MemoryStream();
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
        int P;
        P = _dataPosition;
        _dataPosition = -1;
        WriteImageData(stream);
        _dataPosition = P;
        if (!_dataLoaded)
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
        int P;
        Clear();
        ReadImageData(stream);
        if (_version < 2)
        {
            _dataPosition = (int)(stream.Position);
            if (_loadMode == LoadMode.LoadAlways || _owner == null || !_owner._reloading || !_owner._status.Contains(ImageLibraryStatusElement.LoadingFromFile))
            {
                ReadDiscardableImageData(stream);
                _reloadingEnabled = false;
            }
            else
            {
                stream.Seek(_dataSize, SeekOrigin.Current);
                _reloadingEnabled = true;
            }
        }
        else
        {
            if (_loadMode == LoadMode.LoadAlways || _owner == null || !_owner._reloading || !_owner._status.Contains(ImageLibraryStatusElement.LoadingFromFile) || _dataPosition == -1)
            {
                if (_dataPosition != -1)
                {
                    P = (int)(stream.Position);
                    stream.Position = _owner._dataOffset + _dataPosition;
                    ReadDiscardableImageData(stream);
                    stream.Position = P;
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
        switch (_showInfo.ShowMode)
        {
            case ShowMode.Opaque:
                ShowOpaque(X, Y, lockedSurface);
                break;
            case ShowMode.Transparent:
                ShowTransparent(X, Y, lockedSurface);
                break;
            case ShowMode.Blended:
                switch (_showInfo.BlendMode)
                {
                    case BlendMode.User:
                        if (_showInfo.Blend is BlendTable table)
                        {
                            ShowBlended(X, Y, lockedSurface, table);
                        }

                        break;
                    case BlendMode.Alpha:
                        ShowBlended(X, Y, lockedSurface, GraphicsState.AlphaBlendTable.Lookup[_showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Brighten:
                        ShowBlended(X, Y, lockedSurface, GraphicsState.BrightenBlendTable.Lookup[_showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Intensity:
                        ShowBlended(X, Y, lockedSurface, GraphicsState.IntensityBlendTable.Lookup[_showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Shadow:
                        ShowBlended(X, Y, lockedSurface, GraphicsState.ShadowBlendTable.Lookup[_showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.LinearAlpha:
                        ShowBlended(X, Y, lockedSurface, GraphicsState.LinearAlphaBlendTable.Lookup[_showInfo.Blend.Percentage]);
                        break;
                }

                break;
        }
    }

    // ImageLib.pas:2040
    public virtual void ShowStretched(in Rect R, in LockedSurface lockedSurface)
    {
        switch (_showInfo.ShowMode)
        {
            case ShowMode.Opaque:
                ShowOpaqueStretched(R, lockedSurface);
                break;
            case ShowMode.Blended:
                switch (_showInfo.BlendMode)
                {
                    case BlendMode.User:
                        if (_showInfo.Blend is BlendTable table)
                        {
                            ShowBlendedStretched(R, lockedSurface, table);
                        }

                        break;
                    case BlendMode.Alpha:
                        ShowBlendedStretched(R, lockedSurface, GraphicsState.AlphaBlendTable.Lookup[_showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Brighten:
                        ShowBlendedStretched(R, lockedSurface, GraphicsState.BrightenBlendTable.Lookup[_showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Intensity:
                        ShowBlendedStretched(R, lockedSurface, GraphicsState.IntensityBlendTable.Lookup[_showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Shadow:
                        ShowBlendedStretched(R, lockedSurface, GraphicsState.ShadowBlendTable.Lookup[_showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.LinearAlpha:
                        ShowBlendedStretched(R, lockedSurface, GraphicsState.LinearAlphaBlendTable.Lookup[_showInfo.Blend.Percentage]);
                        break;
                }

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
                switch (_showInfo.BlendMode)
                {
                    case BlendMode.User:
                        if (showInfo.Blend is BlendTable table)
                        {
                            ShowBlended(X, Y, lockedSurface, table);
                        }

                        break;
                    case BlendMode.Alpha:
                        ShowBlended(X, Y, lockedSurface, GraphicsState.AlphaBlendTable.Lookup[showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Brighten:
                        ShowBlended(X, Y, lockedSurface, GraphicsState.BrightenBlendTable.Lookup[showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Intensity:
                        ShowBlended(X, Y, lockedSurface, GraphicsState.IntensityBlendTable.Lookup[showInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Shadow:
                        ShowBlended(X, Y, lockedSurface, GraphicsState.ShadowBlendTable.Lookup[showInfo.Blend.Percentage]);
                        break;
                }

                break;
        }
    }

    // ImageLib.pas:2087
    public virtual void ShowDisplayController(Rect? sourceRectangle, Rect? destinationRectangle, int displayController, int bkColor)
    {
    }

    // ImageLib.pas:2091
    public virtual bool SupportShowMode(ShowMode showMode, int pixelFormatId) => false;

    // ImageLib.pas:1903
    public virtual bool SupportGrabPixelFormat(int pixelFormatId) => false;

    // ImageLib.pas:1908
    public virtual Form CreateGrabForm(Component owner, in LockedSurface lockedSurface, ImagesGrabbedEvent onImagesGrabbed) => null;

    // ImageLib.pas:1914
    public virtual bool GrabImage(in LockedSurface lockedSurface, in Rect R, GrabInfo grabInfo) => false;

    // ImageLib.pas:1920
    public virtual GrabInfo CreateGrabInfo() => new GrabInfo();

    // ImageLib.pas:1607
    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    // ImageLib.pas:1648
    public void GetDimensions(ref Rect R)
    {
        R.Left = 0;
        R.Right = _width;
        R.Top = 0;
        R.Bottom = _height;
    }

    // ImageLib.pas:2061
    public void ShowClipped(int X, int Y, in LockedSurface lockedSurface)
    {
        Show(X, Y, lockedSurface);
    }

    public CustomImageLibrary Owner { get => _owner; set => SetOwner(value); }
    public bool DataLoaded { get => _dataLoaded; }
    public int DataPosition { get => _dataPosition; }
    public int DataSize { get => _dataSize; }
    public bool ReloadingEnabled { get => _reloadingEnabled; set => SetReloadingEnabled(value); }
    public byte Version { get => _version; }
    public LoadMode LoadMode { get => _loadMode; set => SetLoadMode(value); }
    public int DataWidth { get => _dataWidth; }
    public int DataHeight { get => _dataHeight; }
    public int Width { get => _width; set => SetWidth(value); }
    public int Height { get => _height; set => SetHeight(value); }
    public int Xshift { get => _xshift; set => SetXshift(value); }
    public int Yshift { get => _yshift; set => SetYshift(value); }
    public string FileName { get => _fileName; set => _fileName = value; }
    public int FileIndex { get => _fileIndex; set => _fileIndex = value; }
    public ShowInfo ShowInfo { get => _showInfo; set => SetDefaultShowInfo(value); }
    public Palette Palette { get => GetPalette(); }
    public Palette RemapPalette { get => GetRemapPalette(); }
    public bool Transparent { get => GetTransparent(); }

    public static void WriteImageToStream(LibraryImage image, Stream stream)
    {
        int id = default;
        if (image != null)
        {
            id = image.ImageClass.ClassId;
            stream.Write(Bytes.Of(ref id, 4));
            image.SaveToStream(stream);
        }
        else
        {
            id = 0;
            stream.Write(Bytes.Of(ref id, 4));
        }
    }

    internal static void WriteImageToStream(LibraryImage image, Stream stream, Stream dataStream)
    {
        int id = default;
        if (image != null)
        {
            id = image.ImageClass.ClassId;
            stream.Write(Bytes.Of(ref id, 4));
            image.SaveToStream(stream, dataStream);
        }
        else
        {
            id = 0;
            stream.Write(Bytes.Of(ref id, 4));
        }
    }

}
