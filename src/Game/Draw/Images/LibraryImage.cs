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
    static int ILibraryImageClass.ClassID => 0;
    static byte ILibraryImageClass.ClassVersion => 3;
    static string ILibraryImageClass.Name => "Abstract Library Image";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new LibraryImage(owner);

    private LibraryImageClass imageClass;

    public LibraryImageClass ImageClass => imageClass ??= LibraryImageClass.Of(GetType());

    public virtual byte ClassVersion() => ImageClass.ClassVersion;
    public virtual string Name() => ImageClass.Name;

    internal CustomImageLibrary FOwner;
    internal bool FDataLoaded;
    internal int FDataPosition;
    internal int FDataSize;
    internal bool FReloadingEnabled;
    internal byte FVersion;
    internal LoadMode FLoadMode;
    internal int FDataWidth;
    internal int FDataHeight;
    internal int FWidth;
    internal int FHeight;
    internal int FXshift;
    internal int FYshift;
    internal string FFileName = "";
    internal int FFileIndex;
    internal ShowInfo FShowInfo;

    // ImageLib.pas:1613
    public virtual void SetOwner(CustomImageLibrary AOwner)
    {
        if (AOwner != FOwner && FOwner != null && !FDataLoaded)
        {
            SetReloadingEnabled(false);
        }

        FOwner = AOwner;
    }

    // ImageLib.pas:1877
    public virtual void SetLoadMode(LoadMode Value)
    {
        FLoadMode = Value;
        if (FLoadMode == LoadMode.LoadAlways && !FDataLoaded)
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
    public virtual void SetReloadingEnabled(bool Value)
    {
        if (!Value && !FDataLoaded)
        {
            LoadDiscardableImageData();
        }

        if (Value && !FReloadingEnabled)
        {
            throw new InvalidOperationException("Cannot turn on Reloading when disabled");
        }
        else
        {
            FReloadingEnabled = Value;
        }
    }

    // ImageLib.pas:1991
    public virtual void SetDefaultShowInfo(ShowInfo Value)
    {
        if (SupportShowMode(Value.ShowMode, 0))
        {
            FShowInfo = Value;
        }
    }

    // ImageLib.pas:1620
    public virtual void SetXshift(int Value)
    {
        FXshift = Value;
    }

    // ImageLib.pas:1625
    public virtual void SetYshift(int Value)
    {
        FYshift = Value;
    }

    // ImageLib.pas:1630
    public virtual void SetWidth(int Value)
    {
        FWidth = Value;
    }

    // ImageLib.pas:1635
    public virtual void SetHeight(int Value)
    {
        FHeight = Value;
    }

    // ImageLib.pas:1640
    public virtual void GetVisualDimensions(ref Rect R)
    {
        R.Left = 0;
        R.Right = FWidth;
        R.Top = 0;
        R.Bottom = FHeight;
    }

    // ImageLib.pas:1702
    public virtual void ReadImageData(Stream Stream)
    {
        Stream.ReadExactly(Bytes.Of(ref FVersion, 1));
        LibraryDrawing.ReadStringFromStream(ref FFileName, Stream);
        Stream.ReadExactly(Bytes.Of(ref FDataWidth, 4));
        Stream.ReadExactly(Bytes.Of(ref FDataHeight, 4));
        Stream.ReadExactly(Bytes.Of(ref FXshift, 4));
        Stream.ReadExactly(Bytes.Of(ref FYshift, 4));
        Stream.ReadExactly(Bytes.Of(ref FFileIndex, 4));
        Stream.ReadExactly(Bytes.Of(ref FLoadMode, 1));
        Stream.ReadExactly(Bytes.Of(ref FDataSize, 4));
        if (FVersion > 0)
        {
            if (FVersion >= 2)
            {
                Stream.ReadExactly(Bytes.Of(ref FDataPosition, 4));
            }

            Stream.ReadExactly(Bytes.Of(ref FWidth, 4));
            Stream.ReadExactly(Bytes.Of(ref FHeight, 4));
        }
        else
        {
            FWidth = FDataWidth + FXshift;
            FHeight = FDataHeight + FYshift;
        }

        if (FVersion >= 3)
        {
            FShowInfo.ReadImage(Stream, false);
        }
    }

    // ImageLib.pas:1727
    public virtual void WriteImageData(Stream Stream)
    {
        byte V;
        V = ClassVersion();
        Stream.Write(Bytes.Of(ref V, 1));
        LibraryDrawing.WriteStringToStream(FFileName, Stream);
        Stream.Write(Bytes.Of(ref FDataWidth, 4));
        Stream.Write(Bytes.Of(ref FDataHeight, 4));
        Stream.Write(Bytes.Of(ref FXshift, 4));
        Stream.Write(Bytes.Of(ref FYshift, 4));
        Stream.Write(Bytes.Of(ref FFileIndex, 4));
        Stream.Write(Bytes.Of(ref FLoadMode, 1));
        Stream.Write(Bytes.Of(ref FDataSize, 4));
        Stream.Write(Bytes.Of(ref FDataPosition, 4));
        Stream.Write(Bytes.Of(ref FWidth, 4));
        Stream.Write(Bytes.Of(ref FHeight, 4));
        FShowInfo.WriteImage(Stream, false);
    }

    // ImageLib.pas:1747
    public virtual void ReadDiscardableImageData(Stream Stream)
    {
        FDataLoaded = true;
    }

    // ImageLib.pas:1752
    public virtual void WriteDiscardableImageData(Stream Stream)
    {
    }

    // ImageLib.pas:1756
    public virtual void ReleaseDiscardableImageData()
    {
        FDataLoaded = false;
    }

    // ImageLib.pas:1761
    public virtual void LoadDiscardableImageData()
    {
        Stream Stream;
        if (!FDataLoaded)
        {
            Stream = new FastLoadFileStream(FOwner.FLoadedFileName);
            try
            {
                Stream.Seek(FDataPosition + FOwner.FDataOffset, SeekOrigin.Begin);
                ReadDiscardableImageData(Stream);
                FDataLoaded = true;
            }
            finally
            {
                Stream?.Dispose();
            }
        }
    }

    // ImageLib.pas:1859
    public virtual void SaveToStreamEx(Stream Stream, Stream DataStream)
    {
        int P;
        P = FDataPosition;
        FDataPosition = (int)(DataStream.Position);
        WriteImageData(Stream);
        FDataPosition = P;
        if (!FDataLoaded)
        {
            LoadDiscardableImageData();
            WriteDiscardableImageData(DataStream);
            if (FLoadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            WriteDiscardableImageData(DataStream);
        }
    }

    // ImageLib.pas:1835
    public virtual void LoadFromStreamEx(Stream Stream, Stream DataStream)
    {
        ReadImageData(Stream);
        if (FVersion >= 2)
        {
            if (FLoadMode == LoadMode.LoadAlways || FOwner == null || !FOwner.FReloading || !FOwner.FStatus.Contains(ImageLibraryStatusElement.LoadingFromFile) || FDataPosition == -1)
            {
                if (FDataPosition != -1)
                {
                    DataStream.Position = FOwner.FDataOffset + FDataPosition;
                    ReadDiscardableImageData(DataStream);
                }
                else
                {
                    ReadDiscardableImageData(Stream);
                }

                FReloadingEnabled = false;
            }
            else
            {
                FReloadingEnabled = true;
            }
        }
        else
        {
            throw new ArgumentException("Invalid image format");
        }
    }

    // ImageLib.pas:1985
    public virtual int RemapToPixelFormat(int PixelFormatID)
    {
        throw new NotSupportedException("Surface format not supported");
    }

    // ImageLib.pas:1685
    public virtual void Clear()
    {
        FVersion = ClassVersion();
        ReleaseDiscardableImageData();
        FDataSize = 0;
        FDataLoaded = false;
        FDataPosition = -1;
        FDataSize = 0;
        FReloadingEnabled = false;
        FXshift = 0;
        FDataWidth = 0;
        FWidth = 0;
        FYshift = 0;
        FDataHeight = 0;
        FHeight = 0;
    }

    // ImageLib.pas:1596
    public LibraryImage(CustomImageLibrary owner)
    {
        FDataPosition = -1;
        FReloadingEnabled = false;
        FLoadMode = LoadMode.LoadOnDemand;
        FShowInfo.ShowMode = ShowMode.Opaque;
        FShowInfo.BlendMode = BlendMode.Alpha;
        SetOwner(owner);
    }

    // ImageLib.pas:1979
    public virtual string Info()
    {
        string Result = Name() + " " + IntToStr(FWidth) + "x" + IntToStr(FHeight);
        if (GetTransparent())
        {
            Result = Result + " Transparent";
        }

        return Result;
    }

    // ImageLib.pas:1670
    public virtual LibraryImage Copy()
    {
        LibraryImage Result = default;
        MemoryStream Stream;
        Stream = new MemoryStream();
        try
        {
            WriteImageToStream(this, Stream);
            Stream.Position = 0;
            Result = ImageStreams.ReadImageFromStream(Stream, FOwner);
            Result.SetOwner(null);
        }
        finally
        {
            Stream?.Dispose();
        }

        return Result;
    }

    // ImageLib.pas:1778
    public virtual void SaveToStream(Stream Stream)
    {
        int P;
        P = FDataPosition;
        FDataPosition = -1;
        WriteImageData(Stream);
        FDataPosition = P;
        if (!FDataLoaded)
        {
            LoadDiscardableImageData();
            WriteDiscardableImageData(Stream);
            if (FLoadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            WriteDiscardableImageData(Stream);
        }
    }

    // ImageLib.pas:1796
    public virtual void LoadFromStream(Stream Stream)
    {
        int P;
        Clear();
        ReadImageData(Stream);
        if (FVersion < 2)
        {
            FDataPosition = (int)(Stream.Position);
            if (FLoadMode == LoadMode.LoadAlways || FOwner == null || !FOwner.FReloading || !FOwner.FStatus.Contains(ImageLibraryStatusElement.LoadingFromFile))
            {
                ReadDiscardableImageData(Stream);
                FReloadingEnabled = false;
            }
            else
            {
                Stream.Seek(FDataSize, SeekOrigin.Current);
                FReloadingEnabled = true;
            }
        }
        else
        {
            if (FLoadMode == LoadMode.LoadAlways || FOwner == null || !FOwner.FReloading || !FOwner.FStatus.Contains(ImageLibraryStatusElement.LoadingFromFile) || FDataPosition == -1)
            {
                if (FDataPosition != -1)
                {
                    P = (int)(Stream.Position);
                    Stream.Position = FOwner.FDataOffset + FDataPosition;
                    ReadDiscardableImageData(Stream);
                    Stream.Position = P;
                }
                else
                {
                    ReadDiscardableImageData(Stream);
                }

                FReloadingEnabled = false;
            }
            else
            {
                FReloadingEnabled = true;
            }
        }
    }

    // ImageLib.pas:1996
    public virtual void Show(int X, int Y, in LockedSurface SD)
    {
        switch (FShowInfo.ShowMode)
        {
            case ShowMode.Opaque:
                ShowOpaque(X, Y, SD);
                break;
            case ShowMode.Transparent:
                ShowTransparent(X, Y, SD);
                break;
            case ShowMode.Blended:
                switch (FShowInfo.BlendMode)
                {
                    case BlendMode.User:
                        if (FShowInfo.Blend is BlendTable table)
                        {
                            ShowBlended(X, Y, SD, table);
                        }

                        break;
                    case BlendMode.Alpha:
                        ShowBlended(X, Y, SD, GraphicsState.AlphaBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Brighten:
                        ShowBlended(X, Y, SD, GraphicsState.BrightenBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Intensity:
                        ShowBlended(X, Y, SD, GraphicsState.IntensityBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Shadow:
                        ShowBlended(X, Y, SD, GraphicsState.ShadowBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.LinearAlpha:
                        ShowBlended(X, Y, SD, GraphicsState.LinearAlphaBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                }

                break;
        }
    }

    // ImageLib.pas:2040
    public virtual void ShowStretched(in Rect R, in LockedSurface SD)
    {
        switch (FShowInfo.ShowMode)
        {
            case ShowMode.Opaque:
                ShowOpaqueStretched(R, SD);
                break;
            case ShowMode.Blended:
                switch (FShowInfo.BlendMode)
                {
                    case BlendMode.User:
                        if (FShowInfo.Blend is BlendTable table)
                        {
                            ShowBlendedStretched(R, SD, table);
                        }

                        break;
                    case BlendMode.Alpha:
                        ShowBlendedStretched(R, SD, GraphicsState.AlphaBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Brighten:
                        ShowBlendedStretched(R, SD, GraphicsState.BrightenBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Intensity:
                        ShowBlendedStretched(R, SD, GraphicsState.IntensityBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Shadow:
                        ShowBlendedStretched(R, SD, GraphicsState.ShadowBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.LinearAlpha:
                        ShowBlendedStretched(R, SD, GraphicsState.LinearAlphaBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                }

                break;
        }
    }

    // ImageLib.pas:2066
    public virtual void ShowOpaque(int X, int Y, in LockedSurface SD)
    {
    }

    // ImageLib.pas:2070
    public virtual void ShowOpaqueStretched(in Rect R, in LockedSurface SD)
    {
    }

    // ImageLib.pas:2078
    public virtual void ShowBlended(int X, int Y, in LockedSurface SD, BlendTable Table)
    {
    }

    // ImageLib.pas:2082
    public virtual void ShowBlendedStretched(in Rect R, in LockedSurface SD, BlendTable Table)
    {
    }

    // ImageLib.pas:2074
    public virtual void ShowTransparent(int X, int Y, in LockedSurface SD)
    {
    }

    // ImageLib.pas:2020
    // { ShowEx: the blend mode comes from the image's own show info (an original bug: ShowInfo only
    //   gives the show mode and the blend value), and bmLinearAlpha is not handled. }
    public virtual void ShowEx(int X, int Y, in LockedSurface SD, in ShowInfo ShowInfo)
    {
        switch (ShowInfo.ShowMode)
        {
            case ShowMode.Opaque:
                ShowOpaque(X, Y, SD);
                break;
            case ShowMode.Transparent:
                ShowTransparent(X, Y, SD);
                break;
            case ShowMode.Blended:
                switch (FShowInfo.BlendMode)
                {
                    case BlendMode.User:
                        if (ShowInfo.Blend is BlendTable table)
                        {
                            ShowBlended(X, Y, SD, table);
                        }

                        break;
                    case BlendMode.Alpha:
                        ShowBlended(X, Y, SD, GraphicsState.AlphaBlendTable.Lookup[ShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Brighten:
                        ShowBlended(X, Y, SD, GraphicsState.BrightenBlendTable.Lookup[ShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Intensity:
                        ShowBlended(X, Y, SD, GraphicsState.IntensityBlendTable.Lookup[ShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Shadow:
                        ShowBlended(X, Y, SD, GraphicsState.ShadowBlendTable.Lookup[ShowInfo.Blend.Percentage]);
                        break;
                }

                break;
        }
    }

    // ImageLib.pas:2087
    public virtual void ShowDC(Rect? SrcRect, Rect? DestRect, int DC, int BkColor)
    {
    }

    // ImageLib.pas:2091
    public virtual bool SupportShowMode(ShowMode ShowMode, int PixelFormatID) => false;

    // ImageLib.pas:1903
    public virtual bool SupportGrabPixelFormat(int PixelFormatID) => false;

    // ImageLib.pas:1908
    public virtual Form CreateGrabForm(Component AOwner, in LockedSurface SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:1914
    public virtual bool GrabImage(in LockedSurface SD, in Rect R, GrabInfo GrabInfo) => false;

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
        R.Right = FWidth;
        R.Top = 0;
        R.Bottom = FHeight;
    }

    // ImageLib.pas:2061
    public void ShowClipped(int X, int Y, in LockedSurface SD)
    {
        Show(X, Y, SD);
    }

    public CustomImageLibrary Owner { get => FOwner; set => SetOwner(value); }
    public bool DataLoaded { get => FDataLoaded; }
    public int DataPosition { get => FDataPosition; }
    public int DataSize { get => FDataSize; }
    public bool ReloadingEnabled { get => FReloadingEnabled; set => SetReloadingEnabled(value); }
    public byte Version { get => FVersion; }
    public LoadMode LoadMode { get => FLoadMode; set => SetLoadMode(value); }
    public int DataWidth { get => FDataWidth; }
    public int DataHeight { get => FDataHeight; }
    public int Width { get => FWidth; set => SetWidth(value); }
    public int Height { get => FHeight; set => SetHeight(value); }
    public int Xshift { get => FXshift; set => SetXshift(value); }
    public int Yshift { get => FYshift; set => SetYshift(value); }
    public string FileName { get => FFileName; set => FFileName = value; }
    public int FileIndex { get => FFileIndex; set => FFileIndex = value; }
    public ShowInfo ShowInfo { get => FShowInfo; set => SetDefaultShowInfo(value); }
    public Palette Palette { get => GetPalette(); }
    public Palette RemapPalette { get => GetRemapPalette(); }
    public bool Transparent { get => GetTransparent(); }

    public static void WriteImageToStream(LibraryImage Image, Stream Stream)
    {
        int ID = default;
        if (Image != null)
        {
            ID = Image.ImageClass.ClassID;
            Stream.Write(Bytes.Of(ref ID, 4));
            Image.SaveToStream(Stream);
        }
        else
        {
            ID = 0;
            Stream.Write(Bytes.Of(ref ID, 4));
        }
    }

    internal static void WriteImageToStreamEx(LibraryImage Image, Stream Stream, Stream DataStream)
    {
        int ID = default;
        if (Image != null)
        {
            ID = Image.ImageClass.ClassID;
            Stream.Write(Bytes.Of(ref ID, 4));
            Image.SaveToStreamEx(Stream, DataStream);
        }
        else
        {
            ID = 0;
            Stream.Write(Bytes.Of(ref ID, 4));
        }
    }

}
