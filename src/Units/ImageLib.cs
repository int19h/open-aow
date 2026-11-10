// Translated from ImageLib.pas.
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Aow.Game.Draw;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Hash;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

public enum ImageLibraryStatusElement : byte
{
    LoadingFromFile = 0,
    Loading = 1,
    Modified = 2,
    Saving = 3,
    SavingToFile = 4
}

public enum SetTextModeElement : byte
{
    Center = 0
}

public enum ILGridEditModeElement : byte
{
    Cut = 0,
    Copy = 1,
    Paste = 2,
    Delete = 3,
    Ins = 4,
    Del = 5
}

public enum ILManagerCategoryElement : byte
{
    Disk = 0,
    Owned = 1
}

public enum ILManagerOptionsElement : byte
{
    AutoSearchPath = 0
}

public enum BltMode : byte
{
    None = 0,
    Stretch = 1,
    StretchToFit = 2,
    Center = 3
}

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
    public virtual GFXEPalette GetPalette() => null;

    // ImageLib.pas:1888
    public virtual GFXEPalette GetRemapPalette() => null;

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
        Stream.ReadBytes(Bytes.Of(ref FVersion, 1));
        LibraryDrawing.ReadStringFromStream(ref FFileName, Stream);
        Stream.ReadBytes(Bytes.Of(ref FDataWidth, 4));
        Stream.ReadBytes(Bytes.Of(ref FDataHeight, 4));
        Stream.ReadBytes(Bytes.Of(ref FXshift, 4));
        Stream.ReadBytes(Bytes.Of(ref FYshift, 4));
        Stream.ReadBytes(Bytes.Of(ref FFileIndex, 4));
        Stream.ReadBytes(Bytes.Of(ref FLoadMode, 1));
        Stream.ReadBytes(Bytes.Of(ref FDataSize, 4));
        if (FVersion > 0)
        {
            if (FVersion >= 2)
            {
                Stream.ReadBytes(Bytes.Of(ref FDataPosition, 4));
            }

            Stream.ReadBytes(Bytes.Of(ref FWidth, 4));
            Stream.ReadBytes(Bytes.Of(ref FHeight, 4));
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
    public virtual void Show(int X, int Y, in SurfaceDesc SD)
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
                        ShowBlended(X, Y, SD, GfxState.AlphaBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Brighten:
                        ShowBlended(X, Y, SD, GfxState.BrightenBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Intensity:
                        ShowBlended(X, Y, SD, GfxState.IntensityBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Shadow:
                        ShowBlended(X, Y, SD, GfxState.ShadowBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.LinearAlpha:
                        ShowBlended(X, Y, SD, GfxState.LinearAlphaBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                }

                break;
        }
    }

    // ImageLib.pas:2040
    public virtual void ShowStretched(in Rect R, in SurfaceDesc SD)
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
                        ShowBlendedStretched(R, SD, GfxState.AlphaBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Brighten:
                        ShowBlendedStretched(R, SD, GfxState.BrightenBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Intensity:
                        ShowBlendedStretched(R, SD, GfxState.IntensityBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Shadow:
                        ShowBlendedStretched(R, SD, GfxState.ShadowBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.LinearAlpha:
                        ShowBlendedStretched(R, SD, GfxState.LinearAlphaBlendTable.Lookup[FShowInfo.Blend.Percentage]);
                        break;
                }

                break;
        }
    }

    // ImageLib.pas:2066
    public virtual void ShowOpaque(int X, int Y, in SurfaceDesc SD)
    {
    }

    // ImageLib.pas:2070
    public virtual void ShowOpaqueStretched(in Rect R, in SurfaceDesc SD)
    {
    }

    // ImageLib.pas:2078
    public virtual void ShowBlended(int X, int Y, in SurfaceDesc SD, BlendTable Table)
    {
    }

    // ImageLib.pas:2082
    public virtual void ShowBlendedStretched(in Rect R, in SurfaceDesc SD, BlendTable Table)
    {
    }

    // ImageLib.pas:2074
    public virtual void ShowTransparent(int X, int Y, in SurfaceDesc SD)
    {
    }

    // ImageLib.pas:2020
    // { ShowEx: the blend mode comes from the image's own show info (an original bug: ShowInfo only
    //   gives the show mode and the blend value), and bmLinearAlpha is not handled. }
    public virtual void ShowEx(int X, int Y, in SurfaceDesc SD, in ShowInfo ShowInfo)
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
                        ShowBlended(X, Y, SD, GfxState.AlphaBlendTable.Lookup[ShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Brighten:
                        ShowBlended(X, Y, SD, GfxState.BrightenBlendTable.Lookup[ShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Intensity:
                        ShowBlended(X, Y, SD, GfxState.IntensityBlendTable.Lookup[ShowInfo.Blend.Percentage]);
                        break;
                    case BlendMode.Shadow:
                        ShowBlended(X, Y, SD, GfxState.ShadowBlendTable.Lookup[ShowInfo.Blend.Percentage]);
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
    public virtual Form CreateGrabForm(Component AOwner, in SurfaceDesc SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:1914
    public virtual bool GrabImage(in SurfaceDesc SD, in Rect R, GrabInfo GrabInfo) => false;

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
    public void ShowClipped(int X, int Y, in SurfaceDesc SD)
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
    public GFXEPalette Palette { get => GetPalette(); }
    public GFXEPalette RemapPalette { get => GetRemapPalette(); }
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

// FIndex, FPalette, FRemapPalette (the palette remapped to a pixel format, made on demand),
// FRefCount
public partial class LibraryImagePalette : Persistent
{
    internal int FIndex;
    internal GFXEPalette FPalette;
    internal GFXEPalette FRemapPalette;
    internal int FRefCount;

    // ImageLib.pas:7084
    public LibraryImagePalette()
    {
        FPalette = new GFXEPalette();
    }

    // ImageLib.pas:7090
    public override void Dispose()
    {
        base.Dispose();
    }

    // ImageLib.pas:7097
    public void SaveToStream(Stream Stream)
    {
        FPalette.SaveToStream(Stream);
    }

    // ImageLib.pas:7102
    public void LoadFromStream(Stream Stream)
    {
        FPalette.LoadFromStream(Stream);
    }

    // ImageLib.pas:7107
    public GFXEPalette RemapPalette(int PixelFormatID)
    {
        GFXEPalette Result;
        if (PixelFormatID == FPalette.PixelFormatID)
        {
            Result = FPalette;
        }
        else
        {
            PixelFormats.Color2PixelInfo(0, PixelFormatID, FPalette);
            if (FRemapPalette == null || PixelFormatID != FRemapPalette.PixelFormatID)
            {
                if (FRemapPalette == null)
                {
                    FRemapPalette = new GFXEPalette();
                }

                FRemapPalette.Assign(FPalette);
                FRemapPalette.Remap(PixelFormatID);
            }

            Result = FRemapPalette;
        }

        return Result;
    }

    // ImageLib.pas:7124
    public override void Assign(Persistent Source)
    {
        if (Source is LibraryImagePalette)
        {
            FPalette.Assign(((LibraryImagePalette)Source).FPalette);
            if (((LibraryImagePalette)Source).FRemapPalette != null)
            {
                if (FRemapPalette == null)
                {
                    FRemapPalette = new GFXEPalette();
                }

                FRemapPalette.Assign(((LibraryImagePalette)Source).FRemapPalette);
            }
            else
            {
            }
        }
        else
        {
            throw new FormatException("Can only assign to a TLibraryImagePalette descendant");
        }
    }

    public int Index { get => FIndex; }
    public GFXEPalette Palette { get => FPalette; }
    public int RefCount { get => FRefCount; }

}

[LegacyObject]
public partial class PaletteManager : IDisposable
{
    internal DelphiList FPalettes;

    // ImageLib.pas:6984
    internal int GetCount() => FPalettes.Count;

    // ImageLib.pas:6989
    internal LibraryImagePalette GetPalette(int Index) => (LibraryImagePalette)FPalettes[Index];

    // ImageLib.pas:6994
    internal void UpdateIndices()
    {
        for (int I = 0, last = FPalettes.Count - 1; I <= last; I++)
        {
            ((LibraryImagePalette)FPalettes[I]).FIndex = I;
        }
    }

    // ImageLib.pas:6971
    public PaletteManager()
    {
        CaptureAddress.Assign(this);
        FPalettes = new DelphiList();
    }

    // ImageLib.pas:6977
    public virtual void Dispose()
    {
        Clear();
        FPalettes?.Dispose();
    }

    // ImageLib.pas:7001
    public void LoadFromStream(Stream Stream)
    {
        int N = default;
        LibraryImagePalette P;
        Clear();
        Stream.ReadBytes(Bytes.Of(ref N, 4));
        for (int I = 0, last = N - 1; I <= last; I++)
        {
            P = new LibraryImagePalette();
            P.LoadFromStream(Stream);
            FPalettes.Add(P);
            P.FIndex = I;
        }
    }

    // ImageLib.pas:7017
    public void SaveToStream(Stream Stream)
    {
        Stream.Write(ValueBytes(FPalettes.Count, 4));
        for (int I = 0, last = FPalettes.Count - 1; I <= last; I++)
        {
            ((LibraryImagePalette)FPalettes.List[I]).SaveToStream(Stream);
        }
    }

    // ImageLib.pas:7025
    public void Clear()
    {
        for (int I = FPalettes.Count - 1; I >= 0; I--)
        {
            ((LibraryImagePalette)FPalettes.List[I])?.Dispose();
        }

        FPalettes.Clear();
    }

    // ImageLib.pas:7033
    public LibraryImagePalette LinkToPalette(int Index)
    {
        LibraryImagePalette Result = (LibraryImagePalette)FPalettes[Index];
        Result.FRefCount += 1;
        return Result;
    }

    // ImageLib.pas:7039
    public LibraryImagePalette RegisterPalette(GFXEPalette Palette)
    {
        LibraryImagePalette Result;
        int I;
        bool Found;
        LibraryImagePalette P;
        I = FPalettes.Count;
        Found = false;
        while (I != 0 && !Found)
        {
            I -= 1;
            Found = Palette.Compare(((LibraryImagePalette)FPalettes.List[I]).FPalette);
        }

        if (!Found)
        {
            P = new LibraryImagePalette();
            P.FPalette.Assign(Palette);
            P.FIndex = FPalettes.Count;
            P.FRefCount = 1;
            FPalettes.Add(P);
            Result = P;
        }
        else
        {
            var libraryImagePalette = FPalettes[I];
            ((LibraryImagePalette)libraryImagePalette).FRefCount += 1;
            Result = (LibraryImagePalette)FPalettes[I];
        }

        return Result;
    }

    // ImageLib.pas:7068
    public void UnregisterPalette(int Index)
    {
        LibraryImagePalette P;
        P = (LibraryImagePalette)FPalettes[Index];
        P.FRefCount -= 1;
        if (P.FRefCount <= 0)
        {
            FPalettes.Delete(Index);
            UpdateIndices();
            P?.Dispose();
        }
    }

    public int Count { get => GetCount(); }

    public sealed class PalettesAccessor(PaletteManager target)
    {
        public LibraryImagePalette this[int Index] { get => target.GetPalette(Index); }
    }

    public PalettesAccessor Palettes => new PalettesAccessor(this);

}

// FStatus, FOnChange, FChangeLock (OnChange is called when it is 0), FReloading (the images can
// reload their data from FLoadedFileName), FLoadOnDemand, FLoadedFileName, FFileName (relative
// to the project or to BaseImageLibraryPath), FDataOffset (where the discardable image data
// starts in the file), FCapacity, FCount, FImages, FPaletteManager
public partial class CustomImageLibrary : Component
{

    internal Set<ImageLibraryStatusElement> FStatus;
    internal NotifyEvent<object> FOnChange;
    internal byte FChangeLock;
    internal bool FReloading;
    internal bool FLoadOnDemand;
    internal string FLoadedFileName = "";
    internal string FFileName = "";
    internal int FDataOffset;
    internal int FCapacity;
    internal int FCount;
    internal ImageItem[] FImages;
    internal PaletteManager FPaletteManager;

    // ImageLib.pas:9699
    internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity != 0)
        {
            NewCapacity = NewCapacity | 7;
        }

        if (NewCapacity != FCapacity)
        {
            FImages = ReallocArray(FImages, NewCapacity);
            if (NewCapacity > FCapacity)
            {
                ClearElements(FImages, FCapacity, NewCapacity - FCapacity);
                for (int I = FCapacity; I <= NewCapacity - 1; I++)
                {
                    FImages[I].Reference = -1;
                }
            }

            FCapacity = NewCapacity;
            if (FCapacity < FCount)
            {
                FCount = FCapacity;
            }
        }
    }

    // ImageLib.pas:9717
    internal void SetCount(int Value)
    {
        FCount = Value;
        if (FCount > FCapacity)
        {
            SetCapacity(FCount);
        }
    }

    // ImageLib.pas:9723
    internal string GetFullFileName()
    {
        string Result;
        string Drive;
        string Path;
        if (!IsEmpty(FFileName))
        {
            if (ComponentState.Contains(Rtl.ComponentState.Designing))
            {
                Path = ExtractFilePath(ToolServices.GetProjectName());
            }
            else
            {
                Path = ImageLibraries.BaseImageLibraryPath;
            }

            Drive = ExtractFileDrive(FFileName);
            if (IsEmpty(Drive) || Drive == ExtractFileDrive(Path))
            {
                Result = ExpandFileName(Path + FFileName);
            }
            else
            {
                Result = FFileName;
            }
        }
        else
        {
            Result = "";
        }

        return Result;
    }

    // ImageLib.pas:9743
    internal void SetFileName(string Value)
    {
        string Path;
        if (ComponentState.Contains(Rtl.ComponentState.Designing))
        {
            Path = ExtractFilePath(ToolServices.GetProjectName());
        }
        else
        {
            Path = ImageLibraries.BaseImageLibraryPath;
        }

        Value = ExtractRelativePath(Path, Value);
        if ((ComponentState.Contains(Rtl.ComponentState.Designing) || ComponentState.Contains(Rtl.ComponentState.Loading)) && Value != FFileName)
        {
            FFileName = Value;
            if (!IsEmpty(FFileName))
            {
                try
                {
                    LoadFromFile(GetFullFileName());
                }
                catch
                {
                    ShowMessage("Error loading: " + GetFullFileName());
                    FFileName = "";
                }
            }
            else
            {
                DisableReloading();
            }
        }
        else
        {
            FFileName = Value;
        }
    }

    // ImageLib.pas:9775
    internal void DeleteItem(int Index)
    {
        MoveElements(FImages, Index + 1, FImages, Index, FCount - Index - 1);
        ClearElements(FImages, FCount - 1, 1);
        FCount -= 1;
        SetModified(true);
    }

    // ImageLib.pas:9783
    internal void InsertItem(int Index)
    {
        SetCount(FCount + 1);
        MoveElements(FImages, Index, FImages, Index + 1, FCount - Index - 1);
        ClearElements(FImages, Index, 1);
    }

    // ImageLib.pas:9877
    internal void SetLoadOnDemand(bool Value)
    {
        if (!Value && FLoadOnDemand)
        {
            DisableReloading();
        }

        FLoadOnDemand = Value;
        if (!Value)
        {
            FReloading = false;
        }
    }

    // ImageLib.pas:9820
    public override void DefineProperties(Filer Filer)
    {
        /* TODO: Implement Classes.DefineBinaryProperty in the platform or editor service. */
        throw new NotImplementedException();
    }

    public CustomImageLibrary(Component owner) : base(owner)
    {
        FPaletteManager = new PaletteManager();
        FLoadOnDemand = true;
        FFileName = "";
        FStatus = Set<ImageLibraryStatusElement>.Of();
        Clear();
    }

    // ImageLib.pas:9679
    public override void Dispose()
    {
        Clear();
        FPaletteManager?.Dispose();
        base.Dispose();
    }

    // ImageLib.pas:9686
    public virtual bool GetModified() => FStatus.Contains(ImageLibraryStatusElement.Modified);

    // ImageLib.pas:9691
    public virtual void SetModified(bool Value)
    {
        if (Value)
        {
            FStatus = FStatus + Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Modified);
        }
        else
        {
            FStatus = FStatus - Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Modified);
        }
    }

    // ImageLib.pas:9826
    public virtual void Clear()
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (FImages[I].Image != null)
            {
                FImages[I].Image?.Dispose();
            }
        }

        if (!FStatus.Contains(ImageLibraryStatusElement.LoadingFromFile))
        {
            FFileName = "";
            FLoadedFileName = "";
        }

        FReloading = false;
        SetCapacity(0);
        FPaletteManager.Clear();
        if (FChangeLock == 0)
        {
            CallOnChangeEvent();
        }
    }

    // ImageLib.pas:9960
    public virtual void LoadFromStream(Stream Stream)
    {
        int Index = default;
        int Reference = default;
        int StartPos;
        MemoryStream MS;
        ILBHeader Header = default;
        int SubIndex;
        LibraryImage Image;
        FStatus = FStatus + Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Loading);
        FChangeLock = (byte)(FChangeLock + 1);
        Clear();
        FReloading = FStatus.Contains(ImageLibraryStatusElement.LoadingFromFile) && FLoadOnDemand;
        StartPos = (int)(Stream.Position);
        Stream.ReadBytes(Bytes.Of(ref Header, 24));
        Stream.Seek(Header.HeaderSize + StartPos, SeekOrigin.Begin);
        if ((double)Header.Version < 4)
        {
            FDataOffset = 0;
            if ((double)Header.Version > 1.5)
            {
                FPaletteManager.LoadFromStream(Stream);
            }
            else
            {
                FPaletteManager.Clear();
            }

            // The original reads each index with Stream.Read. A short read leaves the index without its
            // terminator, so a truncated file repeats the loop without end. The port reads it with
            // ReadBuffer, which raises EndOfStreamException on a short read. Valid files always end with the -1
            // terminator. The version 4 path below reads its index region the same way, so a region
            // without the terminator raises there too.
            if ((double)Header.Version <= 2)
            {
                Stream.ReadBuffer(Bytes.Of(ref Index, 4));
                while (Index != -1)
                {
                    SubIndex = 0;
                    Image = ImageStreams.ReadImageFromStream(Stream, this);
                    while (Image != null)
                    {
                        SetSubImage(Index, SubIndex, Image);
                        SubIndex += 1;
                        Image = ImageStreams.ReadImageFromStream(Stream, this);
                    }

                    Stream.ReadBytes(Bytes.Of(ref Reference, 4));
                    SetImageReference(Index, Reference);
                    Stream.ReadBuffer(Bytes.Of(ref Index, 4));
                }
            }
            else
            {
                if ((double)Header.Version == 3)
                {
                    Stream.ReadBuffer(Bytes.Of(ref Index, 4));
                    while (Index != -1)
                    {
                        var readImageFromStream = ImageStreams.ReadImageFromStream(Stream, this);
                        SetImage(Index, readImageFromStream);
                        Stream.ReadBytes(Bytes.Of(ref Reference, 4));
                        SetImageReference(Index, Reference);
                        Stream.ReadBuffer(Bytes.Of(ref Index, 4));
                    }
                }
            }
        }
        else
        {
            FDataOffset = Header.IndexEnd + StartPos;
            MS = new MemoryStream();
            try
            {
                MS.SetLength(Header.IndexEnd - Header.HeaderSize);
                var size = (int)MS.Length;
                Stream.ReadBuffer(MS.GetBuffer().AsSpan(0, size));
                FPaletteManager.LoadFromStream(MS);
                MS.ReadBuffer(Bytes.Of(ref Index, 4));
                while (Index != -1)
                {
                    var readImageFromStreamEx = ImageStreams.ReadImageFromStreamEx(MS, Stream, this);
                    SetImage(Index, readImageFromStreamEx);
                    MS.Read(Bytes.Of(ref Reference, 4));
                    SetImageReference(Index, Reference);
                    MS.ReadBuffer(Bytes.Of(ref Index, 4));
                }
            }
            finally
            {
                MS?.Dispose();
            }

            if (!FStatus.Contains(ImageLibraryStatusElement.LoadingFromFile))
            {
                Stream.Seek(Header.Size + StartPos, SeekOrigin.Begin);
            }
        }

        FChangeLock = (byte)(FChangeLock - 1);
        if (FChangeLock == 0)
        {
            CallOnChangeEvent();
        }

        FStatus = FStatus - Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Loading);
    }

    // ImageLib.pas:10038
    public virtual void SaveToStream(Stream Stream)
    {
        Stream Swap;
        int StartPos;
        string SwapName;
        int I;
        ILBHeader Header = default;
        StartPos = (int)(Stream.Position);
        SwapName = ExtractFilePath(RtlUnit.Application.ExeName) + "\\SWAP.$$$";
        Swap = OpenFile(SwapName, 65535);
        try
        {
            FStatus = FStatus + Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Saving);
            Header.HeaderSize = 24;
            Header.ID = "ILB\0";
            Header.Version = 4;
            Stream.Write(Bytes.Of(ref Header, 24));
            FPaletteManager.SaveToStream(Stream);
            I = 0;
            int last = FCount - 1;
            for (; I <= last; I += 1)
            {
                if (FImages[I].Image != null || FImages[I].Reference != -1)
                {
                    Stream.Write(Bytes.Of(ref I, 4));
                    LibraryImage.WriteImageToStreamEx(FImages[I].Image, Stream, Swap);
                    Stream.Write(Bytes.Of(ref FImages[I].Reference, 4));
                }
            }

            I = -1;
            Stream.Write(Bytes.Of(ref I, 4));
            var position = Stream.Position;
            Header.IndexEnd = (int)(position) - StartPos;
            Swap.Position = 0;
            Stream.CopyFrom(Swap, (int)Swap.Length);
            var streamPosition = Stream.Position;
            Header.Size = (int)(streamPosition) - StartPos;
            Stream.Position = StartPos;
            Stream.Write(Bytes.Of(ref Header, 24));
            Stream.Position = StartPos + Header.Size;
        }
        finally
        {
            FStatus = FStatus - Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Saving);
            Swap?.Dispose();
            DeleteFile(SwapName);
        }
    }

    // ImageLib.pas:10079
    public virtual void LoadFromFile(string AFileName)
    {
        Stream Stream;
        Stream = new FastLoadFileStream(AFileName);
        try
        {
            FChangeLock = (byte)(FChangeLock + 1);
            FStatus = FStatus + Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.LoadingFromFile);
            FileName = ExpandFileName(AFileName);
            FLoadedFileName = GetFullFileName();
            LoadFromStream(Stream);
        }
        finally
        {
            FStatus = FStatus - Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.LoadingFromFile);
            Stream?.Dispose();
            FChangeLock = (byte)(FChangeLock - 1);
            if (FChangeLock == 0)
            {
                CallOnChangeEvent();
            }
        }
    }

    // ImageLib.pas:10098
    public virtual void SaveToFile(string AFileName)
    {
        Stream Stream;
        string Name;
        Name = ExpandFileName(AFileName);
        if (Name == FLoadedFileName)
        {
            DisableReloading();
        }

        FStatus = FStatus + Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.SavingToFile);
        Stream = OpenFile(Name, 65535);
        try
        {
            SaveToStream(Stream);
        }
        finally
        {
            FStatus = FStatus - Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.SavingToFile);
            Stream?.Dispose();
        }
    }

    // ImageLib.pas:9798
    public virtual void CallOnChangeEvent()
    {
        if (FChangeLock == 0 && FOnChange != null && !ComponentState.Contains(Rtl.ComponentState.Destroying) && !ComponentState.Contains(Rtl.ComponentState.Reading))
        {
            FChangeLock = (byte)(FChangeLock + 1);
            FOnChange(this);
            FChangeLock = (byte)(FChangeLock - 1);
        }
    }

    // ImageLib.pas:10152
    public virtual CustomImageLibrary Copy()
    {
        CustomImageLibrary Result = default;
        MemoryStream Stream;
        Stream = new MemoryStream();
        try
        {
            Result = (CustomImageLibrary)new Component(null);
            SaveToStream(Stream);
            Stream.Position = 0;
            Result.LoadFromStream(Stream);
            Result.FReloading = false;
        }
        finally
        {
            Stream.Dispose();
        }

        return Result;
    }

    // ImageLib.pas:9769
    public int ReduceCount()
    {
        while (FCount > 0 && Get(FCount - 1) == null)
        {
            FCount -= 1;
        }

        return FCount;
    }

    // ImageLib.pas:9790
    public void UpdateImageItemSettings(int Index)
    {
        LibraryImage Image;
        Image = FImages[Index].Image;
        if (Image != null && Image.ImageClass.ClassID == 256)
        {
            ((ImageNode)Image).UpdateDimensions();
        }
    }

    // ImageLib.pas:9809
    public void DisableOnChangeEvent()
    {
        FChangeLock = (byte)(FChangeLock + 1);
    }

    // ImageLib.pas:9814
    public void EnableOnChangeEvent()
    {
        FChangeLock = (byte)(FChangeLock - 1);
        if (FChangeLock == 0)
        {
            CallOnChangeEvent();
        }
    }

    // ImageLib.pas:9843
    public void DisableReloading()
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            if (FImages[I].Image != null)
            {
                FImages[I].Image.SetReloadingEnabled(false);
            }
        }

        FReloading = false;
    }

    // ImageLib.pas:9852
    public void ReleaseDiscardableImageData()
    {
        LibraryImage Image;
        if (FReloading)
        {
            for (int I = 0, last = FCount - 1; I <= last; I++)
            {
                Image = FImages[I].Image;
                if (Image != null)
                {
                    Image.ReleaseDiscardableImageData();
                }
            }
        }
    }

    // ImageLib.pas:9865
    public void LoadDiscardableImageData()
    {
        LibraryImage Image;
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            Image = FImages[I].Image;
            if (Image != null)
            {
                Image.LoadDiscardableImageData();
            }
        }
    }

    // ImageLib.pas:9884
    public bool IsReference(int Index) => Index >= 0 && Index < FCount && FImages[Index].Image == null && FImages[Index].Reference != -1;

    // ImageLib.pas:9922
    public bool ImageExist(int Index)
    {
        bool Result;
        int R;
        if (Index < FCount && Index >= 0)
        {
            Result = FImages[Index].Image != null;
            if (!Result)
            {
                R = FImages[Index].Reference;
                if (R >= 0 && R < FCount)
                {
                    Result = FImages[R].Image != null;
                }
                else
                {
                    Result = false;
                }
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // ImageLib.pas:9942
    public int GetImageReference(int Index) => FImages[Index].Reference;

    // ImageLib.pas:9947
    public void SetImageReference(int Index, int Value)
    {
        if (Index >= FCount)
        {
            SetCount(Index + 1);
        }

        if (Value != -1)
        {
            if (FImages[Index].Image != null)
            {
                FImages[Index].Image.Dispose();
            }

            FImages[Index].Image = null;
        }

        FImages[Index].Reference = Value;
        UpdateImageItemSettings(Index);
        SetModified(true);
    }

    // ImageLib.pas:10115
    public void LoadFromFileStream(string AFileName, Stream Stream)
    {
        FStatus = FStatus + Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.LoadingFromFile);
        try
        {
            FileName = ExpandFileName(AFileName);
            FLoadedFileName = GetFullFileName();
            LoadFromStream(Stream);
        }
        finally
        {
            FStatus = FStatus - Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.LoadingFromFile);
        }
    }

    // ImageLib.pas:10127
    public void CopyToClipboard()
    {
        /* TODO: Implement Clipbrd.SetComponent in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:10132
    public void PasteFromClipboard()
    {
        /* TODO: Implement Clipbrd.GetComponent in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:10168
    public override void Assign(Persistent Source)
    {
        MemoryStream Stream;
        if (Source is CustomImageLibrary)
        {
            Stream = new MemoryStream();
            try
            {
                ((CustomImageLibrary)Source).SaveToStream(Stream);
                Stream.Position = 0;
                LoadFromStream(Stream);
                FFileName = ((CustomImageLibrary)Source).FFileName;
                FLoadedFileName = ((CustomImageLibrary)Source).FLoadedFileName;
                if (FChangeLock == 0)
                {
                    CallOnChangeEvent();
                }
            }
            finally
            {
                Stream?.Dispose();
            }
        }
        else
        {
            throw new FormatException("Can only assign to a TCustomImageLibrary descendant");
        }
    }

    // ImageLib.pas:10190
    public void Delete(int Index)
    {
        if (Index < FCount)
        {
            if (FImages[Index].Image != null)
            {
                FImages[Index].Image?.Dispose();
            }

            DeleteItem(Index);
            if (FChangeLock == 0)
            {
                CallOnChangeEvent();
            }
        }
    }

    // ImageLib.pas:10200
    public int Remove(LibraryImage Image)
    {
        int Result = IndexOf(Image);
        if (Result != -1)
        {
            if (FImages[Result].Image != null)
            {
                FImages[Result].Image?.Dispose();
            }

            DeleteItem(Result);
            if (FChangeLock == 0)
            {
                CallOnChangeEvent();
            }
        }

        return Result;
    }

    // ImageLib.pas:10211
    public int IndexOf(LibraryImage Image)
    {
        int Result = 0;
        while (Result < FCount && Image != FImages[Result].Image)
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // ImageLib.pas:10218
    public LibraryImage GetImage(int Index)
    {
        LibraryImage Result;
        if (FImages[Index].Image != null)
        {
            Result = FImages[Index].Image;
            Result.SetOwner(null);
            FImages[Index].Image = null;
            SetModified(true);
            if (FChangeLock == 0)
            {
                CallOnChangeEvent();
            }
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // ImageLib.pas:10232
    public void SetImage(int Index, LibraryImage Image)
    {
        if (Index >= FCount)
        {
            SetCount(Index + 1);
        }

        if (FImages[Index].Image != null)
        {
            FImages[Index].Image.Dispose();
        }

        FImages[Index].Image = Image;
        FImages[Index].Reference = -1;
        UpdateImageItemSettings(Index);
        if (Image != null)
        {
            Image.SetOwner(this);
        }

        SetModified(true);
        if (FChangeLock == 0)
        {
            CallOnChangeEvent();
        }
    }

    // ImageLib.pas:10244
    public void Insert(int Index, LibraryImage Image)
    {
        if (Index < 0)
        {
            Index = 0;
        }

        if (Index < FCount)
        {
            InsertItem(Index);
        }

        SetImage(Index, Image);
    }

    // ImageLib.pas:10251
    public void Add(LibraryImage Image)
    {
        SetImage(FCount, Image);
    }

    // ImageLib.pas:10256
    public bool Empty()
    {
        int I;
        I = 0;
        while (I != FCount && FImages[I].Image == null)
        {
            I += 1;
        }

        return I == FCount;
    }

    // ImageLib.pas:10265
    public void Pack()
    {
        DisableOnChangeEvent();
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (Get(I) == null)
            {
                Delete(I);
            }
        }

        EnableOnChangeEvent();
    }

    // ImageLib.pas:10275
    public int GetSubCount(int Index)
    {
        int Result;
        LibraryImage Image;
        Image = GetSafe(Index);
        if (Image != null)
        {
            if (Image.ImageClass.ClassID != 256)
            {
                Result = 1;
            }
            else
            {
                Result = ((ImageNode)Image).FCount;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // ImageLib.pas:10291
    public LibraryImage GetSub(int Index, int SubIndex)
    {
        LibraryImage Result;
        LibraryImage Image;
        Image = FImages[Index].Image;
        if (Image == null)
        {
            Index = FImages[Index].Reference;
            if (Index != -1)
            {
                Image = FImages[Index].Image;
            }
        }

        if (Image != null)
        {
            if (Image.ImageClass.ClassID == 256)
            {
                Result = ((ImageNode)Image).FImages[SubIndex];
            }
            else
            {
                if (SubIndex == 0)
                {
                    Result = Image;
                }
                else
                {
                    Result = null;
                }
            }
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // ImageLib.pas:10314
    public LibraryImage GetSafeSub(int Index, int SubIndex)
    {
        LibraryImage Result;
        LibraryImage Image;
        Image = GetSafe(Index);
        if (Image != null)
        {
            if (Image.ImageClass.ClassID == 256)
            {
                Result = ((ImageNode)Image).GetSafeImage(SubIndex);
            }
            else
            {
                if (SubIndex == 0)
                {
                    Result = Image;
                }
                else
                {
                    Result = null;
                }
            }
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // ImageLib.pas:10332
    public LibraryImage GetSubImage(int Index, int SubIndex)
    {
        ImageNode Node;
        LibraryImage Result = FImages[Index].Image;
        if (Result != null)
        {
            if (Result.ImageClass.ClassID == 256)
            {
                Node = (ImageNode)Result;
                if (SubIndex >= 0 && SubIndex < Node.FCount)
                {
                    Result = Node.FImages[SubIndex];
                    Result.SetOwner(null);
                    Node.Delete(SubIndex);
                    if (Node.FCount == 1)
                    {
                        var image = Node.GetImage(0);
                        FImages[Index].Image = image;
                        Node.Delete(0);
                        Node?.Dispose();
                    }

                    SetModified(true);
                }
                else
                {
                    Result = null;
                }
            }
            else
            {
                if (SubIndex == 0)
                {
                    Result.SetOwner(null);
                    FImages[Index].Image = null;
                    SetModified(true);
                }
                else
                {
                    Result = null;
                }
            }
        }

        return Result;
    }

    // ImageLib.pas:10367
    public void SetSubImage(int Index, int SubIndex, LibraryImage Image)
    {
        LibraryImage Old;
        ImageNode Node;
        if (Index < FCount)
        {
            Old = FImages[Index].Image;
        }
        else
        {
            Old = null;
        }

        if (Old != null)
        {
            if (Old.ImageClass.ClassID == 256)
            {
                Node = (ImageNode)Old;
                if (SubIndex >= 0 && SubIndex < Node.FCount)
                {
                    Old = Node.FImages[SubIndex];
                    if (Old != null)
                    {
                        Old.Dispose();
                    }

                    Node.FImages[SubIndex] = Image;
                    Image.SetOwner(this);
                    SetModified(true);
                }
                else
                {
                    Node.Add(Image);
                    Image.SetOwner(this);
                    SetModified(true);
                }
            }
            else
            {
                if (SubIndex != 0)
                {
                    Node = new ImageNode(this);
                    Node.Add(Old);
                    Node.Add(Image);
                    FImages[Index].Image = Node;
                    Image.SetOwner(this);
                    SetModified(true);
                }
                else
                {
                    SetImage(Index, Image);
                }
            }
        }
        else
        {
            SetImage(Index, Image);
        }
    }

    // ImageLib.pas:10414
    // { DeleteSub: the test of the node class is inverted (an original bug): a single image is treated
    //   as a node, and a node is freed whole when SubIndex is 0. }
    public void DeleteSub(int Index, int SubIndex)
    {
        ImageNode Node;
        Node = (ImageNode)FImages[Index].Image;
        if (Node != null)
        {
            if (Node.ImageClass.ClassID != 256)
            {
                Node.GetImage(SubIndex).Dispose();
                Node.Delete(SubIndex);
                if (Node.FCount == 1)
                {
                    FImages[Index].Image = Node.FImages[0];
                    Node.Delete(0);
                    Node.Dispose();
                }

                SetModified(true);
            }
            else
            {
                if (SubIndex == 0)
                {
                    Node?.Dispose();
                    FImages[Index].Image = null;
                }
            }
        }
    }

    // ImageLib.pas:10439
    public int RemoveSub(int Index, LibraryImage Image)
    {
        int Result;
        LibraryImage Old;
        ImageNode Node;
        Old = FImages[Index].Image;
        if (Old != null)
        {
            if (Old.ImageClass.ClassID == 256)
            {
                Node = (ImageNode)Old;
                Result = Node.IndexOf(Image);
                if (Result != -1)
                {
                    DeleteSub(Index, Result);
                }
            }
            else
            {
                if (Old == Image)
                {
                    Old?.Dispose();
                    FImages[Index].Image = null;
                    SetModified(true);
                    Result = 0;
                }
                else
                {
                    Result = -1;
                }
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // ImageLib.pas:10465
    public int IndexOfSub(int Index, LibraryImage Image)
    {
        int Result;
        LibraryImage Old;
        Old = FImages[Index].Image;
        if (Old != null)
        {
            if (Old.ImageClass.ClassID == 256)
            {
                Result = ((ImageNode)Old).IndexOf(Image);
            }
            else
            {
                if (Old == Image)
                {
                    Result = 0;
                }
                else
                {
                    Result = -1;
                }
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // ImageLib.pas:10483
    public void InsertSub(int Index, int SubIndex, LibraryImage Image)
    {
        LibraryImage Old;
        ImageNode Node;
        Old = FImages[Index].Image;
        if (Old != null)
        {
            if (Old.ImageClass.ClassID == 256)
            {
                ((ImageNode)Old).Insert(SubIndex, Image);
                Image.SetOwner(this);
                SetModified(true);
            }
            else
            {
                Node = new ImageNode(this);
                Node.Add(Old);
                FImages[Index].Image = Node;
                Node.Insert(SubIndex, Image);
                Image.SetOwner(this);
                SetModified(true);
            }
        }
        else
        {
            AddSub(Index, Image);
        }
    }

    // ImageLib.pas:10511
    public void AddSub(int Index, LibraryImage Image)
    {
        LibraryImage Old;
        ImageNode Node;
        Old = FImages[Index].Image;
        if (Old != null)
        {
            if (Old.ImageClass.ClassID == 256)
            {
                ((ImageNode)Old).Add(Image);
                Image.SetOwner(this);
                SetModified(true);
            }
            else
            {
                Node = new ImageNode(this);
                Node.Add(Old);
                FImages[Index].Image = Node;
                Image.SetOwner(this);
                SetModified(true);
            }
        }
        else
        {
            SetImage(Index, Image);
        }
    }

    // ImageLib.pas:10538
    public bool SubImageExist(int Index, int SubIndex)
    {
        bool Result;
        LibraryImage Image;
        Image = FImages[Index].Image;
        if (Image != null && SubIndex >= 0)
        {
            if (Image.ImageClass.ClassID == 256)
            {
                Result = SubIndex < ((ImageNode)Image).FCount;
            }
            else
            {
                Result = SubIndex == 0;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    public int Count { get => FCount; set => SetCount(value); }

    public LibraryImage this[int Index] { get => Get(Index); }

    public bool Modified { get => GetModified(); set => SetModified(value); }
    public Set<ImageLibraryStatusElement> Status { get => FStatus; }
    public PaletteManager PaletteManager { get => FPaletteManager; }
    public string FullFileName { get => GetFullFileName(); }
    public string LoadedFileName { get => FLoadedFileName; }
    public int DataOffset { get => FDataOffset; }
    public bool Reloading { get => FReloading; }
    public bool LoadOnDemand { get => FLoadOnDemand; set => SetLoadOnDemand(value); }
    public NotifyEvent<object> OnChange { get => FOnChange; set => FOnChange = value; }
    public string FileName { get => FFileName; set => SetFileName(value); }

    internal static int UpdateImage(CustomImageLibrary ImageLibrary, LibraryImage Image)
    {
        LibraryImage Sub;
        LibraryImage NewImage;
        int Result = 0;
        if (Image != null)
        {
            for (int I = 0, last = ImageLibrary.Count - 1; I <= last; I++)
            {
                if (!ImageLibrary.IsReference(I))
                {
                    for (int J = 0, lastJ = ImageLibrary.GetSubCount(I) - 1; J <= lastJ; J++)
                    {
                        Sub = ImageLibrary.GetSub(I, J);
                        if (Sub != null && CompareText(Sub.FFileName, Image.FFileName) == 0 && Sub.FFileIndex == Image.FFileIndex)
                        {
                            NewImage = Image.Copy();
                            NewImage.Xshift = Sub.Xshift;
                            NewImage.Yshift = Sub.Yshift;
                            ImageLibrary.SetSubImage(I, J, NewImage);
                            Result += 1;
                        }
                    }
                }
            }
        }

        return Result;
    }

    public struct ImageItem
    {
        public LibraryImage Image;
        public int Reference;
    }

}

// the image library component
public partial class ImageLibrary : CustomImageLibrary
{

    public ImageLibrary(Component owner) : base(owner)
    { }

}

public enum LoadMode : byte
{
    LoadAlways = 0,
    LoadOnDemand = 1,
    LoadFreeOnDemand = 2
}

[LegacyObject]
public partial class GrabInfo
{
    public static GrabInfoClass Descriptor { get; } = new("GrabInfo", static () => new GrabInfo());

    public int TransparentColor;
    public bool FindRectangles;
    public int TransparentPixel;
    public string FileName = "";
    public int FileIndex;

    // ImageLib.pas:9634
    public virtual void LoadFromStream(Stream Stream)
    {
        Stream.ReadBytes(Bytes.Of(ref TransparentColor, 4));
        Stream.ReadBytes(Bytes.Of(ref FindRectangles, 1));
        Stream.ReadBytes(Bytes.Of(ref TransparentPixel, 4));
        LibraryDrawing.ReadStringFromStream(ref FileName, Stream);
        Stream.ReadBytes(Bytes.Of(ref FileIndex, 4));
    }

    // ImageLib.pas:9643
    public virtual void SaveToStream(Stream Stream)
    {
        Stream.Write(Bytes.Of(ref TransparentColor, 4));
        Stream.Write(Bytes.Of(ref FindRectangles, 1));
        Stream.Write(Bytes.Of(ref TransparentPixel, 4));
        LibraryDrawing.WriteStringToStream(FileName, Stream);
        Stream.Write(Bytes.Of(ref FileIndex, 4));
    }

    // ImageLib.pas:9629
    public virtual GrabInfoClass GrabInfoClass() => Descriptor;

    // ImageLib.pas:9652
    public virtual GrabInfo Copy()
    {
        GrabInfo Result = default;
        MemoryStream Stream;
        Stream = new MemoryStream();
        try
        {
            SaveToStream(Stream);
            Stream.Position = 0;
            Result = GrabInfoClass().Create();
            Result.LoadFromStream(Stream);
        }
        finally
        {
            Stream?.Dispose();
        }

        return Result;
    }

    public GrabInfo()
    {
        CaptureAddress.Assign(this);
    }

}

public delegate void ImagesGrabbedEvent(CustomImageLibrary GrabbedImages, GrabInfo UsedGrabInfo);

public enum ShowMode : byte
{
    Opaque = 0,
    Transparent = 1,
    Blended = 2
}

public enum BlendMode : byte
{
    User = 0,
    Alpha = 1,
    Brighten = 2,
    Intensity = 3,
    Shadow = 4,
    LinearAlpha = 5
}

// { How an image is shown (8 bytes): the blend value is a percentage into the blend table
//     collection of the blend mode, or the blend table itself for bmUser. }
public struct ShowInfo
{
    public ShowMode ShowMode;
    public BlendMode BlendMode;
    public BlendValue Blend;

    // The file image: the mode bytes at 0 and 1, the padding at 2 and the percentage at 4. A
    // table writes 0 where the original wrote its address, which no reader of a file uses, and
    // reading always yields the percentage case.
    private ushort imagePadding;

    private byte[] Image()
    {
        byte[] image = new byte[8];
        image[0] = (byte)ShowMode;
        image[1] = (byte)BlendMode;
        BinaryPrimitives.WriteUInt16LittleEndian(image.AsSpan(2), imagePadding);
        BinaryPrimitives.WriteInt32LittleEndian(image.AsSpan(4), Blend.TryGetValue(out int percentage) ? percentage : 0);
        return image;
    }

    public int ReadImage(Stream stream, bool exact)
    {
        byte[] image = Image();
        int count = stream.Read(image);
        ShowMode = (ShowMode)image[0];
        BlendMode = (BlendMode)image[1];
        imagePadding = BinaryPrimitives.ReadUInt16LittleEndian(image.AsSpan(2));
        Blend = BinaryPrimitives.ReadInt32LittleEndian(image.AsSpan(4));
        if (exact && count != 8)
            throw new EndOfStreamException("Stream read error");
        return count;
    }

    public int WriteImage(Stream stream, bool exact)
    {
        byte[] image = Image();
        stream.Write(image);
        int count = image.Length;
        if (exact && count != 8)
            throw new IOException("Stream write error");
        return count;
    }
}

public partial class ImageNode : LibraryImage, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 256;
    static string ILibraryImageClass.Name => "";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new ImageNode(owner);

    internal LibraryImage[] FImages;
    internal int FCapacity;
    internal int FCount;

    // ImageLib.pas:1313
    internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity != FCapacity)
        {
            FImages = ReallocArray(FImages, NewCapacity);
            FCapacity = NewCapacity;
        }
    }

    // ImageLib.pas:1322
    internal void SetCount(int NewCount)
    {
        if (NewCount > FCapacity)
        {
            SetCapacity(NewCount);
        }

        if (NewCount > FCount)
        {
            ClearElements(FImages, FCount, NewCount - FCount);
        }

        FCount = NewCount;
    }

    // ImageLib.pas:1286
    public ImageNode(CustomImageLibrary owner) : base(owner)
    {
        FFileName = "(multi images)";
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
    public LibraryImage GetImage(int Index) => FImages[Index];

    // ImageLib.pas:1335
    public LibraryImage GetSafeImage(int Index)
    {
        LibraryImage Result;
        if (Index >= 0 && Index < FCount)
        {
            Result = FImages[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // ImageLib.pas:1343
    public void UpdateDimensions()
    {
        FWidth = 0;
        FHeight = 0;
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            if (FImages[I].FWidth > FWidth)
            {
                FWidth = FImages[I].FWidth;
            }

            if (FImages[I].FHeight > FHeight)
            {
                FHeight = FImages[I].FHeight;
            }
        }
    }

    // ImageLib.pas:1356
    public int Add(LibraryImage Image)
    {
        SetCount(FCount + 1);
        FImages[FCount - 1] = Image;
        if (Image.FWidth > FWidth)
        {
            FWidth = Image.FWidth;
        }

        if (Image.FHeight > FHeight)
        {
            FHeight = Image.FHeight;
        }

        return FCount;
    }

    // ImageLib.pas:1365
    public void Insert(int Index, LibraryImage Image)
    {
        if (FCapacity == FCount)
        {
            SetCapacity(FCapacity + 1);
        }

        if (Index < FCount)
        {
            MoveElements(FImages, Index, FImages, Index + 1, FCount - Index);
        }

        FImages[Index] = Image;
        if (Image.FWidth > FWidth)
        {
            FWidth = Image.FWidth;
        }

        if (Image.FHeight > FHeight)
        {
            FHeight = Image.FHeight;
        }

        FCount += 1;
    }

    // ImageLib.pas:1376
    public void Remove(LibraryImage Image)
    {
        int I;
        I = FCount;
        while (I >= 0 && Image != FImages[I])
        {
            I -= 1;
        }

        if (I >= 0)
        {
            FCount -= 1;
            if (I < FCount)
            {
                MoveElements(FImages, I + 1, FImages, I, FCount - I);
            }

            UpdateDimensions();
        }
    }

    // ImageLib.pas:1391
    public int IndexOf(LibraryImage Image)
    {
        int Result = FCount - 1;
        while (Result >= 0 && Image != FImages[Result])
        {
            Result -= 1;
        }

        return Result;
    }

    // ImageLib.pas:1397
    public void Delete(int Index)
    {
        if (Index >= 0 && Index < FCount)
        {
            FCount -= 1;
            if (Index < FCount)
            {
                MoveElements(FImages, Index + 1, FImages, Index, FCount - Index);
            }

            UpdateDimensions();
        }
    }

    // ImageLib.pas:1408
    public override void SaveToStreamEx(Stream Stream, Stream DataStream)
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            WriteImageToStreamEx(FImages[I], Stream, DataStream);
        }

        WriteImageToStream(null, Stream);
    }

    // ImageLib.pas:1416
    public override void SaveToStream(Stream Stream)
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            WriteImageToStream(FImages[I], Stream);
        }

        WriteImageToStream(null, Stream);
    }

    // ImageLib.pas:1424
    public override void LoadFromStream(Stream Stream)
    {
        LibraryImage Image;
        Clear();
        Image = ImageStreams.ReadImageFromStream(Stream, FOwner);
        while (Image != null)
        {
            Add(Image);
            Image = ImageStreams.ReadImageFromStream(Stream, FOwner);
        }
    }

    // ImageLib.pas:1437
    public override void LoadFromStreamEx(Stream Stream, Stream DataStream)
    {
        LibraryImage Image;
        Image = ImageStreams.ReadImageFromStreamEx(Stream, DataStream, FOwner);
        while (Image != null)
        {
            Add(Image);
            Image = ImageStreams.ReadImageFromStreamEx(Stream, DataStream, FOwner);
        }
    }

    // ImageLib.pas:1449
    public override void Clear()
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            FImages[I].Dispose();
        }

        FCount = 0;
        SetCapacity(0);
    }

    // ImageLib.pas:1458
    public override void SetReloadingEnabled(bool Value)
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            FImages[I].SetReloadingEnabled(false);
        }
    }

    // ImageLib.pas:1465
    public override void SetOwner(CustomImageLibrary AOwner)
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            FImages[I].SetOwner(AOwner);
        }

        FOwner = AOwner;
    }

    // ImageLib.pas:1473
    public override int RemapToPixelFormat(int PixelFormatID)
    {
        int I;
        int Result = 0;
        I = FCount;
        while (I != 0 && Result == 0)
        {
            I -= 1;
            Result = FImages[I].RemapToPixelFormat(PixelFormatID);
        }

        return Result;
    }

    // ImageLib.pas:1486
    public override void Show(int X, int Y, in SurfaceDesc SD)
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            FImages[I].Show(X, Y, SD);
        }
    }

    // ImageLib.pas:1493
    public override void ShowOpaque(int X, int Y, in SurfaceDesc SD)
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            FImages[I].ShowOpaque(X, Y, SD);
        }
    }

    // ImageLib.pas:1500
    public override void ShowTransparent(int X, int Y, in SurfaceDesc SD)
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            FImages[I].ShowTransparent(X, Y, SD);
        }
    }

    // ImageLib.pas:1507
    public override void ShowBlended(int X, int Y, in SurfaceDesc SD, BlendTable Table)
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            FImages[I].ShowBlended(X, Y, SD, Table);
        }
    }

    // ImageLib.pas:1514
    public override void ShowDC(Rect? SrcRect, Rect? DestRect, int DC, int BkColor)
    {
        /* TODO: Implement Windows.WindowFromDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:1550
    public override void ShowEx(int X, int Y, in SurfaceDesc SD, in ShowInfo ShowInfo)
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            FImages[I].ShowEx(X, Y, SD, ShowInfo);
        }
    }

    // ImageLib.pas:1557
    public override bool SupportShowMode(ShowMode ShowMode, int PixelFormatID)
    {
        int I;
        I = FCount;
        bool Result = true;
        while (Result && I != 0)
        {
            I -= 1;
            Result = FImages[I].SupportShowMode(ShowMode, PixelFormatID);
        }

        return Result;
    }

    // ImageLib.pas:1570
    public override void SetLoadMode(LoadMode Value)
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            FImages[I].SetLoadMode(Value);
        }

        FLoadMode = Value;
    }

    // ImageLib.pas:1578
    public override void ReleaseDiscardableImageData()
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            FImages[I].ReleaseDiscardableImageData();
        }

        FDataLoaded = false;
    }

    // ImageLib.pas:1586
    public override void LoadDiscardableImageData()
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            FImages[I].LoadDiscardableImageData();
        }

        FDataLoaded = true;
    }

    public int Count { get => FCount; }

    public LibraryImage this[int Index] { get => GetImage(Index); }

}

// the base of the 8 bit images. FOwnPalette (the image owns FImagePalette. otherwise it is
// shared through the palette manager of the library), FImagePalette, FRemapPalette (the palette
// the pixels are shown with: the image palette, or the copy remapped to the pixel format of the
// last surface). RegisterPalette (share the palette through the owner's palette manager) and
// UnregisterPalette (make a private copy again)
public partial class Image08 : LibraryImage, ILibraryImageClass
{
    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new Image08(owner);

    public Image08(CustomImageLibrary owner) : base(owner)
    { }

    internal bool FOwnPalette;
    internal LibraryImagePalette FImagePalette;
    internal GFXEPalette FRemapPalette;

    // ImageLib.pas:7152
    public override void Clear()
    {
        UnregisterPalette();
        FImagePalette?.Dispose();
        FImagePalette = null;
        base.Clear();
    }

    // ImageLib.pas:7160
    public override void ReadImageData(Stream Stream)
    {
        int Index = default;
        base.ReadImageData(Stream);
        Stream.ReadBytes(Bytes.Of(ref FOwnPalette, 1));
        if (FOwnPalette)
        {
            FImagePalette = new LibraryImagePalette();
            FImagePalette.LoadFromStream(Stream);
        }
        else
        {
            Stream.ReadBytes(Bytes.Of(ref Index, 4));
            FImagePalette = FOwner.FPaletteManager.LinkToPalette(Index);
        }

        FRemapPalette = FImagePalette.FPalette;
    }

    // ImageLib.pas:7179
    public override void WriteImageData(Stream Stream)
    {
        base.WriteImageData(Stream);
        Stream.Write(Bytes.Of(ref FOwnPalette, 1));
        if (FOwnPalette)
        {
            FImagePalette.SaveToStream(Stream);
        }
        else
        {
            Stream.Write(Bytes.Of(ref FImagePalette.FIndex, 4));
        }
    }

    // ImageLib.pas:7189
    public virtual void RegisterPalette()
    {
        LibraryImagePalette Old;
        if (FOwnPalette && FImagePalette != null && FOwner != null)
        {
            Old = FImagePalette;
            FImagePalette = FOwner.FPaletteManager.RegisterPalette(FImagePalette.FPalette);
            Old?.Dispose();
            FRemapPalette = FImagePalette.FPalette;
            FOwnPalette = false;
        }
    }

    // ImageLib.pas:7203
    public virtual void UnregisterPalette()
    {
        LibraryImagePalette P;
        if (!FOwnPalette && FImagePalette != null)
        {
            P = new LibraryImagePalette();
            P.Assign(FImagePalette);
            FOwner.FPaletteManager.UnregisterPalette(FImagePalette.FIndex);
            FImagePalette = P;
            FRemapPalette = FImagePalette.FPalette;
            FOwnPalette = true;
        }
    }

    // ImageLib.pas:7218
    public void CreateLibraryImagePalette(GFXEPalette Palette)
    {
        UnregisterPalette();
        if (FImagePalette == null)
        {
            FImagePalette = new LibraryImagePalette();
        }

        FImagePalette.FPalette.Assign(Palette);
        FRemapPalette = FImagePalette.FPalette;
        FOwnPalette = true;
    }

    // ImageLib.pas:7227
    public override void SetOwner(CustomImageLibrary AOwner)
    {
        if (AOwner != FOwner)
        {
            UnregisterPalette();
            base.SetOwner(AOwner);
            RegisterPalette();
        }
    }

    // ImageLib.pas:7237
    public override int RemapToPixelFormat(int PixelFormatID)
    {
        int Result;
        GFXEPalette P;
        P = FImagePalette.RemapPalette(PixelFormatID);
        if (P != null)
        {
            FRemapPalette = P;
            Result = PixelFormats.GFXE_OK;
        }
        else
        {
            Result = PixelFormats.GFXE_NOTSUPPORTED;
        }

        return Result;
    }

    // ImageLib.pas:7251
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:7258
    // { ShowDC remaps the palette to pfPAL8 (an original slip: the surface is pfRGB565; ShowOpaque
    //   remaps the palette again when it differs). }
    public override void ShowDC(Rect? SrcRect, Rect? DestRect, int DC, int BkColor)
    {
        /* TODO: Implement Windows.WindowFromDC in the platform or editor service. */
        throw new NotImplementedException();
    }
}

// an opaque 8 bit picture (ClassID 1). FPitch (of the rows: a multiple of 4 below 32 pixels, of
// 8 from 32 on), FBuffer (the rows from its first byte. The original's FBits, FBuffer aligned to
// 8 bytes, is FBuffer itself in the port)
public partial class Picture08 : Image08, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 1;
    static string ILibraryImageClass.Name => "Picture (08bpp)";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new Picture08(owner);

    public Picture08(CustomImageLibrary owner) : base(owner)
    { }

    internal int FPitch;
    internal byte[] FBuffer;

    // The rows of the picture: FBuffer from its first byte, FPitch bytes apart.
    internal SurfaceView View => new(FBuffer, 0, FPitch);

    // ImageLib.pas:7305
    public override void SetXshift(int Value)
    {
        if (Value < 0)
        {
            Value = 0;
        }

        base.SetXshift(Value);
        SetWidth(FWidth);
    }

    // ImageLib.pas:7312
    public override void SetYshift(int Value)
    {
        if (Value < 0)
        {
            Value = 0;
        }

        base.SetYshift(Value);
        SetHeight(FHeight);
    }

    // ImageLib.pas:7319
    public override void SetWidth(int Value)
    {
        if (Value < FDataWidth + FXshift)
        {
            Value = FDataWidth + FXshift;
        }

        base.SetWidth(Value);
    }

    // ImageLib.pas:7325
    public override void SetHeight(int Value)
    {
        if (Value < FDataHeight + FYshift)
        {
            Value = FDataHeight + FYshift;
        }

        base.SetHeight(Value);
    }

    // ImageLib.pas:7331
    public override bool SupportShowMode(ShowMode ShowMode, int PixelFormatID) => (PixelFormatID == 0 || PixelFormatID == PixelFormats.pfPAL8 || PixelFormatID == PixelFormats.pfRGB555 || PixelFormatID == PixelFormats.pfRGB565) && ShowMode == ShowMode.Opaque;

    // ImageLib.pas:7337
    public void AllocateDiscardableImageData(int AWidth, int AHeight)
    {
        int Size;
        if (AWidth < 32)
        {
            FPitch = (AWidth + 3) & ~3;
        }
        else
        {
            FPitch = (AWidth + 7) & ~7;
        }

        // The original allocates 7 bytes more than the rows and aligns its pixel pointer to 8 for
        // MMX. The alignment has no effect on pixels, so the rows start at the first byte and the
        // 7 bytes stay after the last row (graphics.md 6.8).
        Size = FPitch * AHeight + 7;
        FBuffer = MemorySurfaces.Reallocate(FBuffer, Size);
    }

    // ImageLib.pas:7350
    public override void ReleaseDiscardableImageData()
    {
        if (FDataLoaded)
        {
            FBuffer = null;
            FDataLoaded = false;
        }
    }

    // ImageLib.pas:7359
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        AllocateDiscardableImageData(FDataWidth, FDataHeight);
        for (int Y = 0, last = FDataHeight - 1; Y <= last; Y++)
        {
            Stream.ReadBytes(FBuffer.AsSpan(Y * FPitch, FDataWidth));
        }
    }

    // ImageLib.pas:7369
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
        for (int Y = 0, last = FDataHeight - 1; Y <= last; Y++)
        {
            Stream.Write(FBuffer.AsSpan(Y * FPitch, FDataWidth));
        }
    }

    // ImageLib.pas:7378
    public override Form CreateGrabForm(Component AOwner, in SurfaceDesc SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:7384
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:7389
    public override bool GrabImage(in SurfaceDesc SD, in Rect R, GrabInfo GrabInfo)
    {
        bool Result;
        try
        {
            Clear();
            Result = true;
            FDataLoaded = true;
            FDataWidth = R.Right - R.Left;
            FDataHeight = R.Bottom - R.Top;
            FWidth = FDataWidth;
            FHeight = FDataHeight;
            FFileName = ExtractFileName(GrabInfo.FileName);
            FFileIndex = GrabInfo.FileIndex;
            FDataSize = FDataWidth * FDataHeight;
            AllocateDiscardableImageData(FWidth, FHeight);
            if (SD.PixelFormatID == PixelFormats.pfPAL8)
            {
                for (int Y = 0, last = FHeight - 1; Y <= last; Y++)
                {
                    SD.View().Bytes((R.Top + Y) * SD.Pitch + R.Left, FDataWidth).CopyTo(FBuffer.AsSpan(FPitch * Y, FDataWidth));
                }

                CreateLibraryImagePalette(SD.Palette);
                if (FOwner != null)
                {
                    RegisterPalette();
                }
            }
            else
            {
                Result = false;
            }
        }
        catch
        {
            ReleaseDiscardableImageData();
            Result = false;
        }

        return Result;
    }

    public override void ShowOpaque(int x, int y, in SurfaceDesc sd)
    {
        int destX = x + FXshift, destY = y + FYshift;
        int right = destX + FDataWidth, bottom = destY + FDataHeight;
        int width = FDataWidth, height = FDataHeight, srcX = 0, srcY = 0;
        // Preserve the source offsets and byte destination offset of ImageLib.pas:7424.
        if (destX < sd.ClipRect.Left)
        {
            srcX = sd.ClipRect.Left - x;
            destX = sd.ClipRect.Left;
            width -= srcX;
        }
        if (right > sd.ClipRect.Right) width -= right - sd.ClipRect.Right;
        if (width <= 0) return;
        if (destY < sd.ClipRect.Top)
        {
            srcY = sd.ClipRect.Top - y;
            destY = sd.ClipRect.Top;
            height -= srcY;
        }
        if (bottom > sd.ClipRect.Bottom) height -= bottom - sd.ClipRect.Bottom;
        if (height <= 0) return;

        bool loadedHere = !FDataLoaded;
        if (loadedHere) LoadDiscardableImageData();
        var source = View.At(srcX + srcY * FPitch);
        var destination = sd.View(destX + destY * sd.Pitch);
        if (sd.PixelFormatID == PixelFormats.pfPAL8)
            BlitRects.BltRect(source, destination, width, height);
        else
        {
            if (sd.PixelFormatID != FRemapPalette.PixelFormatID &&
                RemapToPixelFormat(sd.PixelFormatID) != PixelFormats.GFXE_OK)
                throw new NotSupportedException("Surface pixel format not supported!");
            if (sd.PixelFormatID == PixelFormats.pfRGB555)
                PixelRemap.RemapSurface08to555(source, destination, width, height, FRemapPalette);
            else if (sd.PixelFormatID == PixelFormats.pfRGB565)
                PixelRemap.RemapSurface08to565(source, destination, width, height, FRemapPalette);
        }
        if (loadedHere && FLoadMode == LoadMode.LoadFreeOnDemand) ReleaseDiscardableImageData();
    }
}

// an 8 bit sprite, run length encoded (ClassID 2). FTransparentColor (the palette index of the
// transparent pixels), FReserved (only stored), FData (the encoded rows: per row a length
// dword, then the opaque pixels and, for a transparent run, the transparent index and the run
// length), FRLELeft, FRLETop, FRLEWidth, FRLEHeight (the rectangle of the encoded pixels in the
// data rectangle)
public partial class RLESprite08 : Image08, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 2;
    static string ILibraryImageClass.Name => "RLE Sprite (08bpp)";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new RLESprite08(owner);

    public RLESprite08(CustomImageLibrary owner) : base(owner)
    { }

    internal int FTransparentColor;
    internal int FReserved;
    internal byte[] FData;
    internal int FRLELeft;
    internal int FRLETop;
    internal int FRLEWidth;
    internal int FRLEHeight;

    // ImageLib.pas:7512
    public override void ReleaseDiscardableImageData()
    {
        if (FDataLoaded)
        {
            FData = null;
            FDataLoaded = false;
        }
    }

    // ImageLib.pas:7521
    public override void SetXshift(int Value)
    {
        if (Value < -FRLELeft)
        {
            Value = -FRLELeft;
        }

        base.SetXshift(Value);
        SetWidth(FWidth);
    }

    // ImageLib.pas:7528
    public override void SetYshift(int Value)
    {
        if (Value < -FRLETop)
        {
            Value = -FRLETop;
        }

        base.SetYshift(Value);
        SetHeight(FHeight);
    }

    // ImageLib.pas:7535
    public override void SetWidth(int Value)
    {
        int W;
        W = FXshift + FRLELeft + FRLEWidth;
        if (Value < W)
        {
            Value = W;
        }

        base.SetWidth(Value);
    }

    // ImageLib.pas:7544
    public override void SetHeight(int Value)
    {
        int H;
        H = FYshift + FRLETop + FRLEHeight;
        if (Value < H)
        {
            Value = H;
        }

        base.SetHeight(Value);
    }

    // ImageLib.pas:7553
    public override void GetVisualDimensions(ref Rect R)
    {
        R.Left = FXshift + FRLELeft;
        R.Right = R.Left + FRLEWidth;
        R.Top = FYshift + FRLETop;
        R.Bottom = R.Top + FRLEHeight;
    }

    // ImageLib.pas:7561
    public override void ReadImageData(Stream Stream)
    {
        base.ReadImageData(Stream);
        Stream.ReadBytes(Bytes.Of(ref FRLEWidth, 4));
        Stream.ReadBytes(Bytes.Of(ref FRLEHeight, 4));
        Stream.ReadBytes(Bytes.Of(ref FRLELeft, 4));
        Stream.ReadBytes(Bytes.Of(ref FRLETop, 4));
        Stream.ReadBytes(Bytes.Of(ref FTransparentColor, 4));
        Stream.ReadBytes(Bytes.Of(ref FReserved, 4));
    }

    // ImageLib.pas:7572
    public override void WriteImageData(Stream Stream)
    {
        base.WriteImageData(Stream);
        Stream.Write(Bytes.Of(ref FRLEWidth, 4));
        Stream.Write(Bytes.Of(ref FRLEHeight, 4));
        Stream.Write(Bytes.Of(ref FRLELeft, 4));
        Stream.Write(Bytes.Of(ref FRLETop, 4));
        Stream.Write(Bytes.Of(ref FTransparentColor, 4));
        Stream.Write(Bytes.Of(ref FReserved, 4));
    }

    // ImageLib.pas:7583
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        FData = MemorySurfaces.Reallocate(FData, FDataSize);
        Stream.ReadBytes(FData.AsSpan(0, FDataSize));
    }

    // ImageLib.pas:7590
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
        Stream.Write(FData.AsSpan(0, FDataSize));
    }

    // ImageLib.pas:7596
    public override bool SupportShowMode(ShowMode ShowMode, int PixelFormatID) => (PixelFormatID == 0 || PixelFormatID == PixelFormats.pfRGB555 || PixelFormatID == PixelFormats.pfRGB565) && ShowMode == ShowMode.Opaque;

    // ImageLib.pas:7602
    public override void ShowOpaque(int X, int Y, in SurfaceDesc SD)
    {
        if (!FDataLoaded)
        {
            LoadDiscardableImageData();
            if (SD.PixelFormatID == PixelFormats.pfPAL8)
            {
                ShowRLE08(X + FXshift + FRLELeft, Y + FYshift + FRLETop, SD, FData, FRLEWidth, FRLEHeight, (byte)FTransparentColor);
            }
            else
            {
                if (SD.PixelFormatID != FRemapPalette.PixelFormatID && RemapToPixelFormat(SD.PixelFormatID) != PixelFormats.GFXE_OK)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                switch (SD.PixelFormatID)
                {
                    case PixelFormats.pfRGB555:
                    case PixelFormats.pfRGB565:
                        ShowRLE08to16(X + FXshift + FRLELeft, Y + FYshift + FRLETop, SD, FData, FRLEWidth, FRLEHeight, (byte)FTransparentColor, MemoryMarshal.AsBytes(FRemapPalette.Entries.AsSpan()));
                        break;
                    default:
                        throw new NotSupportedException("PixelFormat not supported");
                }
            }

            if (FLoadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            if (SD.PixelFormatID == PixelFormats.pfPAL8)
            {
                ShowRLE08(X + FXshift + FRLELeft, Y + FYshift + FRLETop, SD, FData, FRLEWidth, FRLEHeight, (byte)FTransparentColor);
            }
            else
            {
                if (SD.PixelFormatID != FRemapPalette.PixelFormatID && RemapToPixelFormat(SD.PixelFormatID) != PixelFormats.GFXE_OK)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                switch (SD.PixelFormatID)
                {
                    case PixelFormats.pfRGB555:
                    case PixelFormats.pfRGB565:
                        ShowRLE08to16(X + FXshift + FRLELeft, Y + FYshift + FRLETop, SD, FData, FRLEWidth, FRLEHeight, (byte)FTransparentColor, MemoryMarshal.AsBytes(FRemapPalette.Entries.AsSpan()));
                        break;
                    default:
                        throw new NotSupportedException("PixelFormat not supported");
                }
            }
        }
    }

    // ImageLib.pas:7643
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:7648
    public override Form CreateGrabForm(Component AOwner, in SurfaceDesc SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:7659
    // { GrabImage encodes the rows of the smallest rectangle holding the opaque pixels: per row a
    //   length dword (only its low word is written), the opaque pixels, and the runs of transparent
    //   pixels as the transparent index and the run length (up to 254). The buffer grows by 4 KB
    //   whenever the position reaches FDataSize, which stays 0 until the end (an original slip: every
    //   byte reallocates). Temp is never created. }
    public override bool GrabImage(in SurfaceDesc SD, in Rect R, GrabInfo GrabInfo)
    {
        bool Result;
        int Pos;
        int LineStart;
        int Count;
        object Temp;
        int Line;
        Rect MinRect = default;
        int B;
        int Left;
        int Top;
        int Bottom;
        void AddByte(byte B)
        {
            if (FDataSize <= Pos)
            {
                FData = MemorySurfaces.Reallocate(FData, Pos + 4096);
            }

            FData[Pos] = B;
            Pos += 1;
        }

        void StartLine()
        {
            LineStart = Pos;
            AddByte(0);
            AddByte(0);
            AddByte(0);
            AddByte(0);
        }

        void FlushTransparent()
        {
            while (Count != 0)
            {
                AddByte((byte)FTransparentColor);
                if (Count > 254)
                {
                    Count -= 254;
                    AddByte(254);
                }
                else
                {
                    AddByte((byte)Count);
                    Count = 0;
                }
            }
        }

        void EndLine()
        {
            FlushTransparent();
            if (FDataSize <= Pos)
            {
                FData = MemorySurfaces.Reallocate(FData, Pos + 4096);
            }

            BinaryPrimitives.WriteUInt16LittleEndian(FData.AsSpan(LineStart, 2), (ushort)(Pos - LineStart));
        }

        if (SD.BitCount == 8)
        {
            Result = true;
            try
            {
                Temp = null;
                try
                {
                    FTransparentColor = GrabInfo.TransparentPixel;
                    Clear();
                    FDataLoaded = true;
                    FDataWidth = R.Right - R.Left;
                    FDataHeight = R.Bottom - R.Top;
                    FWidth = FDataWidth;
                    FHeight = FDataHeight;
                    FFileName = ExtractFileName(GrabInfo.FileName);
                    FFileIndex = GrabInfo.FileIndex;
                    Pos = 0;
                    Count = 0;
                    SurfaceDesc.MinimizeTransparentRect(SD, R, FTransparentColor, ref MinRect);
                    Left = MinRect.Left;
                    FRLELeft = Left - R.Left;
                    Top = MinRect.Top;
                    FRLETop = Top - R.Top;
                    FRLEWidth = MinRect.Right - Left;
                    Bottom = MinRect.Bottom;
                    FRLEHeight = Bottom - Top;
                    for (int Y = Top; Y <= Bottom - 1; Y++)
                    {
                        // Line is the byte index of row Y in the surface's memory.
                        Line = SD.DataIndex + SD.Pitch * Y;
                        StartLine();
                        for (int X = MinRect.Left, last = MinRect.Right - 1; X <= last; X++)
                        {
                            B = SD.Data[Line + X];
                            if (B != FTransparentColor)
                            {
                                FlushTransparent();
                                AddByte((byte)B);
                            }
                            else
                            {
                                Count += 1;
                            }
                        }

                        EndLine();
                    }

                    FData = MemorySurfaces.Reallocate(FData, Pos);
                    FDataSize = Pos;
                    CreateLibraryImagePalette(SD.Palette);
                }
                finally
                {
                    if (Temp != null)
                    {
                        ((Temp ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
                    }
                }
            }
            catch
            {
                ReleaseDiscardableImageData();
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static void ShowRLE08(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height, byte ColorKey)
    {
        Blit.ShowRLE08(X, Y, SD.View(), Data, Width, Height, ColorKey);
    }

    private static void ShowRLE08to16(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height, byte ColorKey, ReadOnlySpan<byte> Palette)
    {
        Blit.ShowRLE08to16(X, Y, SD.View(), Data, Width, Height, ColorKey, Palette);
    }

}

// the base of the 16 bit images (pfRGB565 or pfRGB555). FPixelFormatID (of the pixels in
// memory), FStoredPixelFormatID (of the pixels in the library file: WriteImageData remaps the
// pixels back to it before they are written)
public partial class Image16 : LibraryImage, ILibraryImageClass
{
    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new Image16(owner);

    internal int FPixelFormatID;
    internal int FStoredPixelFormatID;

    // ImageLib.pas:2098
    public Image16(CustomImageLibrary owner) : base(owner)
    {
    }

    // ImageLib.pas:2103
    public override void ReadImageData(Stream Stream)
    {
        base.ReadImageData(Stream);
        Stream.ReadBytes(Bytes.Of(ref FStoredPixelFormatID, 4));
        FPixelFormatID = FStoredPixelFormatID;
    }

    // ImageLib.pas:2110
    public override void WriteImageData(Stream Stream)
    {
        base.WriteImageData(Stream);
        RemapToPixelFormat(FStoredPixelFormatID);
        Stream.Write(Bytes.Of(ref FStoredPixelFormatID, 4));
    }

    // ImageLib.pas:2117
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        FPixelFormatID = FStoredPixelFormatID;
    }

    // ImageLib.pas:2123
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
    }

    // ImageLib.pas:2128
    public override void ShowDC(Rect? SrcRect, Rect? DestRect, int DC, int BkColor)
    {
        /* TODO: Implement Windows.WindowFromDC in the platform or editor service. */
        throw new NotImplementedException();
    }

}

// (ClassID $10): an opaque 16 bit picture. FPitch (bytes per row, a multiple of 4, or of 8 from
// 32 pixels on), FBuffer (the rows from its first byte. The original's FData, FBuffer aligned to
// 8 bytes, is FBuffer itself in the port)
public partial class Picture16 : Image16, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 16;
    static string ILibraryImageClass.Name => "Picture (16bpp)";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new Picture16(owner);

    public Picture16(CustomImageLibrary owner) : base(owner)
    { }

    internal int FPitch;
    internal byte[] FBuffer;

    // The rows of the picture: FBuffer from its first byte, FPitch bytes apart.
    internal SurfaceView View => new(FBuffer, 0, FPitch);

    // ImageLib.pas:2206
    internal void AllocateDiscardableImageData(int AWidth, int AHeight)
    {
        int Size;
        if (AWidth < 32)
        {
            FPitch = ((AWidth << 1) + 3) & ~3;
        }
        else
        {
            FPitch = ((AWidth << 1) + 7) & ~7;
        }

        // The rows start at the first byte of FBuffer, as TPicture08's (graphics.md 6.8).
        Size = FPitch * AHeight + 7;
        FBuffer = MemorySurfaces.Reallocate(FBuffer, Size);
    }

    // ImageLib.pas:2174
    public override void SetXshift(int Value)
    {
        if (Value < 0)
        {
            Value = 0;
        }

        base.SetXshift(Value);
        SetWidth(FWidth);
    }

    // ImageLib.pas:2181
    public override void SetYshift(int Value)
    {
        if (Value < 0)
        {
            Value = 0;
        }

        base.SetYshift(Value);
        SetHeight(FHeight);
    }

    // ImageLib.pas:2188
    public override void SetWidth(int Value)
    {
        if (Value < FXshift + FDataWidth)
        {
            Value = FXshift + FDataWidth;
        }

        base.SetWidth(Value);
    }

    // ImageLib.pas:2194
    public override void SetHeight(int Value)
    {
        if (Value < FYshift + FDataHeight)
        {
            Value = FYshift + FDataHeight;
        }

        base.SetHeight(Value);
    }

    // ImageLib.pas:2200
    public override bool SupportShowMode(ShowMode ShowMode, int PixelFormatID) => (PixelFormatID == 0 || PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555) && Set<ShowMode>.Of(ShowMode.Opaque, ShowMode.Transparent, ShowMode.Blended).Contains(ShowMode);

    // ImageLib.pas:2219
    public override void ReleaseDiscardableImageData()
    {
        if (FDataLoaded)
        {
            FBuffer = null;
            FDataLoaded = false;
        }
    }

    // ImageLib.pas:2228
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        AllocateDiscardableImageData(FDataWidth, FDataHeight);
        for (int Y = 0, last = FDataHeight - 1; Y <= last; Y++)
        {
            Stream.ReadBytes(FBuffer.AsSpan(Y * FPitch, FDataWidth * 2));
        }
    }

    // ImageLib.pas:2238
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
        for (int Y = 0, last = FDataHeight - 1; Y <= last; Y++)
        {
            Stream.Write(FBuffer.AsSpan(Y * FPitch, FDataWidth * 2));
        }
    }

    // ImageLib.pas:2247
    public override Form CreateGrabForm(Component AOwner, in SurfaceDesc SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:2253
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555 || PixelFormatID == PixelFormats.pfRGB24 || PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:2259
    public override bool GrabImage(in SurfaceDesc SD, in Rect R, GrabInfo GrabInfo)
    {
        bool Result;
        try
        {
            Clear();
            Result = true;
            FDataLoaded = true;
            FDataWidth = R.Right - R.Left;
            FDataHeight = R.Bottom - R.Top;
            FWidth = FDataWidth;
            FHeight = FDataHeight;
            FFileName = ExtractFileName(GrabInfo.FileName);
            FFileIndex = GrabInfo.FileIndex;
            FDataSize = FDataWidth * FDataHeight * 2;
            AllocateDiscardableImageData(FWidth, FHeight);
            switch (SD.PixelFormatID)
            {
                case PixelFormats.pfPAL8:
                    {
                        for (int Y = 0, last = FHeight - 1; Y <= last; Y++)
                        {
                            var S = SD.View().Bytes((R.Top + Y) * SD.Pitch + R.Left, FWidth);
                            var D = MemoryMarshal.Cast<byte, ushort>(FBuffer.AsSpan(FPitch * Y, FWidth * 2));
                            for (int X = 0, lastX = FWidth - 1; X <= lastX; X++)
                            {
                                ref readonly GFXEPalette.PaletteEntry P = ref SD.Palette.Entries[S[X]];
                                D[X] = (ushort)(((P.R & 248) << 8) + ((P.G & 252) << 3) + (int)(((uint)P.B) >> 3));
                            }
                        }
                    }

                    FStoredPixelFormatID = PixelFormats.pfRGB565;
                    break;
                case PixelFormats.pfRGB565:
                case PixelFormats.pfRGB555:
                    {
                        for (int I = 0, last = FHeight - 1; I <= last; I++)
                        {
                            SD.View().Bytes((R.Top + I) * SD.Pitch + R.Left * 2, FWidth * 2).CopyTo(FBuffer.AsSpan(FPitch * I, FWidth * 2));
                        }
                    }

                    FStoredPixelFormatID = SD.PixelFormatID;
                    break;
                case PixelFormats.pfRGB24:
                    {
                        for (int Y = 0, last = FHeight - 1; Y <= last; Y++)
                        {
                            var D = MemoryMarshal.Cast<byte, ushort>(FBuffer.AsSpan(FPitch * Y, FWidth * 2));
                            var S24 = SD.View().Bytes((R.Top + Y) * SD.Pitch + R.Left * 3, FWidth * 3);
                            for (int X = 0, lastX = FWidth - 1; X <= lastX; X++)
                            {
                                D[X] = (ushort)(((S24[X * 3 + 2] & 248) << 8) + ((S24[X * 3 + 1] & 252) << 3) + (int)(((uint)S24[X * 3]) >> 3));
                            }
                        }
                    }

                    FStoredPixelFormatID = PixelFormats.pfRGB565;
                    break;
                default:
                    Result = false;
                    break;
            }

            FPixelFormatID = FStoredPixelFormatID;
        }
        catch
        {
            ReleaseDiscardableImageData();
            Result = false;
        }

        return Result;
    }

    // ImageLib.pas:2329
    public override int RemapToPixelFormat(int PixelFormatID)
    {
        int Result = 0;
        if (PixelFormatID != FPixelFormatID)
        {
            switch (PixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    PixelRemap.RemapSurface555to565(View, View, FDataWidth, FDataHeight);
                    FPixelFormatID = PixelFormats.pfRGB565;
                    break;
                case PixelFormats.pfRGB555:
                    PixelRemap.RemapSurface565to555(View, View, FDataWidth, FDataHeight);
                    FPixelFormatID = PixelFormats.pfRGB555;
                    break;
                default:
                    Result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }
        }

        return Result;
    }

    // ImageLib.pas:2349
    public override void ShowOpaque(int X, int Y, in SurfaceDesc SD)
    {
        int DX;
        int Right;
        int W;
        int H;
        int SX;
        int SY;
        int DY;
        int Bottom;
        SurfaceView Src;
        SurfaceView Dst;
        DX = X + FXshift;
        W = FDataWidth;
        Right = FDataWidth + DX;
        if (DX < SD.ClipRect.Left)
        {
            SX = SD.ClipRect.Left - X;
            DX = SD.ClipRect.Left;
            W -= SX;
        }
        else
        {
            SX = 0;
        }

        if (Right > SD.ClipRect.Right)
        {
            W -= Right - SD.ClipRect.Right;
        }

        if (W > 0)
        {
            DY = Y + FYshift;
            H = FDataHeight;
            Bottom = DY + FDataHeight;
            if (DY < SD.ClipRect.Top)
            {
                SY = SD.ClipRect.Top - Y;
                DY = SD.ClipRect.Top;
                H -= SY;
            }
            else
            {
                SY = 0;
            }

            if (Bottom > SD.ClipRect.Bottom)
            {
                H -= Bottom - SD.ClipRect.Bottom;
            }

            if (H > 0)
            {
                if (!FDataLoaded)
                {
                    LoadDiscardableImageData();
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        BlitRects.BltRect(Src, Dst, W * 2, H);
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BltRect(Src, Dst, W * 2, H);
                    }

                    if (FLoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        BlitRects.BltRect(Src, Dst, W * 2, H);
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BltRect(Src, Dst, W * 2, H);
                    }
                }
            }
        }
    }

    // ImageLib.pas:2412
    public override void ShowBlended(int X, int Y, in SurfaceDesc SD, BlendTable Table)
    {
        int DX;
        int Right;
        int W;
        int H;
        int SX;
        int SY;
        int DY;
        int Bottom;
        SurfaceView Src;
        SurfaceView Dst;
        DX = X + FXshift;
        W = FDataWidth;
        Right = FDataWidth + DX;
        if (DX < SD.ClipRect.Left)
        {
            SX = SD.ClipRect.Left - X;
            DX = SD.ClipRect.Left;
            W -= SX;
        }
        else
        {
            SX = 0;
        }

        if (Right > SD.ClipRect.Right)
        {
            W -= Right - SD.ClipRect.Right;
        }

        if (W > 0)
        {
            DY = Y + FYshift;
            H = FDataHeight;
            Bottom = DY + FDataHeight;
            if (DY < SD.ClipRect.Top)
            {
                SY = SD.ClipRect.Top - Y;
                DY = SD.ClipRect.Top;
                H -= SY;
            }
            else
            {
                SY = 0;
            }

            if (Bottom > SD.ClipRect.Bottom)
            {
                H -= Bottom - SD.ClipRect.Bottom;
            }

            if (H > 0)
            {
                if (!FDataLoaded)
                {
                    LoadDiscardableImageData();
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        if (Table.Bits == 5)
                        {
                            switch (FPixelFormatID)
                            {
                                case PixelFormats.pfRGB565:
                                    BlendSurface565(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                                case PixelFormats.pfRGB555:
                                    BlendSurface555(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid BlendTable format");
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (Table.Bits == 5)
                        {
                            switch (FPixelFormatID)
                            {
                                case PixelFormats.pfRGB565:
                                    BlendSurface565(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                                case PixelFormats.pfRGB555:
                                    BlendSurface555(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid BlendTable format");
                        }
                    }

                    if (FLoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        if (Table.Bits == 5)
                        {
                            switch (FPixelFormatID)
                            {
                                case PixelFormats.pfRGB565:
                                    BlendSurface565(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                                case PixelFormats.pfRGB555:
                                    BlendSurface555(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid BlendTable format");
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (Table.Bits == 5)
                        {
                            switch (FPixelFormatID)
                            {
                                case PixelFormats.pfRGB565:
                                    BlendSurface565(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                                case PixelFormats.pfRGB555:
                                    BlendSurface555(Src, Dst, W, H, ((BlendTable5Bit)Table).Bytes);
                                    break;
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Invalid BlendTable format");
                        }
                    }
                }
            }
        }
    }

    // ImageLib.pas:2511
    public override void ShowTransparent(int X, int Y, in SurfaceDesc SD)
    {
        int DX;
        int Right;
        int W;
        int H;
        int SX;
        int SY;
        int DY;
        int Bottom;
        SurfaceView Src;
        SurfaceView Dst;
        DX = X + FXshift;
        W = FDataWidth;
        Right = FDataWidth + DX;
        if (DX < SD.ClipRect.Left)
        {
            SX = SD.ClipRect.Left - X;
            DX = SD.ClipRect.Left;
            W -= SX;
        }
        else
        {
            SX = 0;
        }

        if (Right > SD.ClipRect.Right)
        {
            W -= Right - SD.ClipRect.Right;
        }

        if (W > 0)
        {
            DY = Y + FYshift;
            H = FDataHeight;
            Bottom = DY + FDataHeight;
            if (DY < SD.ClipRect.Top)
            {
                SY = SD.ClipRect.Top - Y;
                DY = SD.ClipRect.Top;
                H -= SY;
            }
            else
            {
                SY = 0;
            }

            if (Bottom > SD.ClipRect.Bottom)
            {
                H -= Bottom - SD.ClipRect.Bottom;
            }

            if (H > 0)
            {
                if (!FDataLoaded)
                {
                    LoadDiscardableImageData();
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        if (FPixelFormatID == PixelFormats.pfRGB565)
                        {
                            BltTransparent565(Src, Dst, W, H);
                        }
                        else
                        {
                            BltTransparent555(Src, Dst, W, H);
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (FPixelFormatID == PixelFormats.pfRGB565)
                        {
                            BltTransparent565(Src, Dst, W, H);
                        }
                        else
                        {
                            BltTransparent555(Src, Dst, W, H);
                        }
                    }

                    if (FLoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        if (FPixelFormatID == PixelFormats.pfRGB565)
                        {
                            BltTransparent565(Src, Dst, W, H);
                        }
                        else
                        {
                            BltTransparent555(Src, Dst, W, H);
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        if (FPixelFormatID == PixelFormats.pfRGB565)
                        {
                            BltTransparent565(Src, Dst, W, H);
                        }
                        else
                        {
                            BltTransparent555(Src, Dst, W, H);
                        }
                    }
                }
            }
        }
    }

    private static void BltTransparent565(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        Blit.BltTransparent565(Src, Dst, Width, Height);
    }

    private static void BltTransparent555(SurfaceView Src, SurfaceView Dst, int Width, int Height)
    {
        Blit.BltTransparent555(Src, Dst, Width, Height);
    }

    internal static void BltStretched16(SurfaceView Source, ReadOnlySpan<int> RowTable, ReadOnlySpan<int> ColTable, SurfaceView Dst, int Width, int Height)
    {
        Blit.BltStretched16(Source, RowTable, ColTable, Dst, Width, Height);
    }

    private static void BlendSurface555(SurfaceView Src, SurfaceView Dst, int Width, int Height, ReadOnlySpan<byte> Table)
    {
        Blit.BlendSurface555(Src, Dst, Width, Height, Table);
    }

    private static void BlendSurface565(SurfaceView Src, SurfaceView Dst, int Width, int Height, ReadOnlySpan<byte> Table)
    {
        Blit.BlendSurface565(Src, Dst, Width, Height, Table);
    }

    internal static void BlendStretched555(SurfaceView Source, ReadOnlySpan<int> RowTable, ReadOnlySpan<int> ColTable, SurfaceView Dst, int Width, int Height, ReadOnlySpan<byte> Table)
    {
        Blit.BlendStretched555(Source, RowTable, ColTable, Dst, Width, Height, Table);
    }

    private static void BlendStretched565(SurfaceView Source, ReadOnlySpan<int> RowTable, ReadOnlySpan<int> ColTable, SurfaceView Dst, int Width, int Height, ReadOnlySpan<byte> Table)
    {
        Blit.BlendStretched565(Source, RowTable, ColTable, Dst, Width, Height, Table);
    }

}

// (ClassID $15): a TPicture16 shown transparent by default. it cannot be grabbed
public partial class TransparentPicture16 : Picture16, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 21;
    static string ILibraryImageClass.Name => "Transparent Picture (16bpp)";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new TransparentPicture16(owner);

    // ImageLib.pas:2845
    public TransparentPicture16(CustomImageLibrary owner) : base(owner)
    {
        FShowInfo.ShowMode = ShowMode.Transparent;
    }

    // ImageLib.pas:2861
    public override bool SupportGrabPixelFormat(int PixelFormatID) => false;

}

// (ClassID $16): a 16 bit picture with a transparent color, cut to the smallest rectangle
// holding its opaque pixels. FPitch, FBuffer (as TPicture16), FTransparentColor (in
// FStoredPixelFormatID), FColorKey (the transparent color in FPixelFormatID), FSpriteX,
// FSpriteY (of the pixels in the grabbed rectangle), FSpriteWidth, FSpriteHeight (of the
// pixels)
public partial class Sprite16 : Image16, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 22;
    static string ILibraryImageClass.Name => "Sprite (16bpp)";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new Sprite16(owner);

    internal int FPitch;
    internal byte[] FBuffer;
    internal int FTransparentColor;
    internal int FColorKey;
    internal int FSpriteX;
    internal int FSpriteY;
    internal int FSpriteWidth;
    internal int FSpriteHeight;

    // The rows of the sprite: FBuffer from its first byte, FPitch bytes apart.
    internal SurfaceView View => new(FBuffer, 0, FPitch);

    // ImageLib.pas:4302
    internal void AllocateDiscardableImageData(int AWidth, int AHeight)
    {
        int Size;
        if (AWidth < 32)
        {
            FPitch = ((AWidth << 1) + 3) & ~3;
        }
        else
        {
            FPitch = ((AWidth << 1) + 7) & ~7;
        }

        // The rows start at the first byte of FBuffer, as TPicture08's (graphics.md 6.8).
        Size = FPitch * AHeight + 7;
        FBuffer = MemorySurfaces.Reallocate(FBuffer, Size);
    }

    // ImageLib.pas:4247
    public Sprite16(CustomImageLibrary owner) : base(owner)
    {
    }

    // ImageLib.pas:4262
    public override void SetXshift(int Value)
    {
        if (Value < -FSpriteX)
        {
            Value = -FSpriteX;
        }

        base.SetXshift(Value);
        SetWidth(FWidth);
    }

    // ImageLib.pas:4269
    public override void SetYshift(int Value)
    {
        if (Value < -FSpriteY)
        {
            Value = -FSpriteY;
        }

        base.SetYshift(Value);
        SetHeight(FHeight);
    }

    // ImageLib.pas:4276
    public override void SetWidth(int Value)
    {
        int W;
        W = FXshift + FSpriteX + FSpriteWidth;
        if (Value < W)
        {
            Value = W;
        }

        base.SetWidth(Value);
    }

    // ImageLib.pas:4285
    public override void SetHeight(int Value)
    {
        int W;
        W = FYshift + FSpriteY + FSpriteHeight;
        if (Value < W)
        {
            Value = W;
        }

        base.SetHeight(Value);
    }

    // ImageLib.pas:4294
    public override void GetVisualDimensions(ref Rect R)
    {
        R.Left = FXshift + FSpriteX;
        R.Right = R.Left + FSpriteWidth;
        R.Top = FYshift + FSpriteY;
        R.Bottom = R.Top + FSpriteHeight;
    }

    // ImageLib.pas:4315
    public override void ReleaseDiscardableImageData()
    {
        if (FDataLoaded)
        {
            FBuffer = null;
            FDataLoaded = false;
        }
    }

    // ImageLib.pas:4324
    public override void ReadImageData(Stream Stream)
    {
        base.ReadImageData(Stream);
        Stream.ReadBytes(Bytes.Of(ref FSpriteWidth, 4));
        Stream.ReadBytes(Bytes.Of(ref FSpriteHeight, 4));
        Stream.ReadBytes(Bytes.Of(ref FSpriteX, 4));
        Stream.ReadBytes(Bytes.Of(ref FSpriteY, 4));
        Stream.ReadBytes(Bytes.Of(ref FTransparentColor, 4));
    }

    // ImageLib.pas:4334
    public override void WriteImageData(Stream Stream)
    {
        base.WriteImageData(Stream);
        Stream.Write(Bytes.Of(ref FSpriteWidth, 4));
        Stream.Write(Bytes.Of(ref FSpriteHeight, 4));
        Stream.Write(Bytes.Of(ref FSpriteX, 4));
        Stream.Write(Bytes.Of(ref FSpriteY, 4));
        Stream.Write(Bytes.Of(ref FTransparentColor, 4));
    }

    // ImageLib.pas:4344
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        AllocateDiscardableImageData(FSpriteWidth, FSpriteHeight);
        for (int Y = 0, last = FSpriteHeight - 1; Y <= last; Y++)
        {
            Stream.ReadBytes(FBuffer.AsSpan(Y * FPitch, FSpriteWidth * 2));
        }

        FColorKey = FTransparentColor;
    }

    // ImageLib.pas:4355
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
        for (int Y = 0, last = FSpriteHeight - 1; Y <= last; Y++)
        {
            Stream.Write(FBuffer.AsSpan(Y * FPitch, FSpriteWidth * 2));
        }
    }

    // ImageLib.pas:4364
    public override int RemapToPixelFormat(int PixelFormatID)
    {
        int Result = 0;
        if (PixelFormatID != FPixelFormatID)
        {
            switch (PixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    PixelRemap.RemapSurface555to565(View, View, FSpriteWidth, FSpriteHeight);
                    FColorKey = PixelRemap.RemapPixelInfo(FColorKey, PixelFormats.pfRGB555, null, PixelFormats.pfRGB565, null);
                    FPixelFormatID = PixelFormats.pfRGB565;
                    break;
                case PixelFormats.pfRGB555:
                    PixelRemap.RemapSurface565to555(View, View, FSpriteWidth, FSpriteHeight);
                    FColorKey = PixelRemap.RemapPixelInfo(FColorKey, PixelFormats.pfRGB565, null, PixelFormats.pfRGB555, null);
                    FPixelFormatID = PixelFormats.pfRGB555;
                    break;
                default:
                    Result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }
        }

        return Result;
    }

    // ImageLib.pas:4386
    public override bool SupportShowMode(ShowMode ShowMode, int PixelFormatID) => (PixelFormatID == 0 || PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555) && Set<ShowMode>.Of(ShowMode.Opaque, ShowMode.Transparent, ShowMode.Blended).Contains(ShowMode);

    // ImageLib.pas:4392
    public override void ShowOpaque(int X, int Y, in SurfaceDesc SD)
    {
        int DX;
        int Right;
        int W;
        int H;
        int SX;
        int SY;
        int DY;
        int Bottom;
        SurfaceView Src;
        SurfaceView Dst;
        X += FSpriteX;
        DX = FXshift + X;
        W = FSpriteWidth;
        Right = FSpriteWidth + DX;
        if (DX < SD.ClipRect.Left)
        {
            SX = SD.ClipRect.Left - X;
            DX = SD.ClipRect.Left;
            W -= SX;
        }
        else
        {
            SX = 0;
        }

        if (Right > SD.ClipRect.Right)
        {
            W -= Right - SD.ClipRect.Right;
        }

        if (W > 0)
        {
            Y += FSpriteY;
            DY = Y + FYshift;
            H = FSpriteHeight;
            Bottom = DY + FSpriteHeight;
            if (DY < SD.ClipRect.Top)
            {
                SY = SD.ClipRect.Top - Y;
                DY = SD.ClipRect.Top;
                H -= SY;
            }
            else
            {
                SY = 0;
            }

            if (Bottom > SD.ClipRect.Bottom)
            {
                H -= Bottom - SD.ClipRect.Bottom;
            }

            if (H > 0)
            {
                if (!FDataLoaded)
                {
                    LoadDiscardableImageData();
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        BlitRects.BltColorKey16(Src, Dst, W, H, FColorKey);
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BltColorKey16(Src, Dst, W, H, FColorKey);
                    }

                    if (FLoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        BlitRects.BltColorKey16(Src, Dst, W, H, FColorKey);
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitRects.BltColorKey16(Src, Dst, W, H, FColorKey);
                    }
                }
            }
        }
    }

    // ImageLib.pas:4457
    public override void ShowBlended(int X, int Y, in SurfaceDesc SD, BlendTable Table)
    {
        int DX;
        int Right;
        int W;
        int H;
        int SX;
        int SY;
        int DY;
        int Bottom;
        SurfaceView Src;
        SurfaceView Dst;
        X += FSpriteX;
        DX = FXshift + X;
        W = FSpriteWidth;
        Right = FSpriteWidth + DX;
        if (DX < SD.ClipRect.Left)
        {
            SX = SD.ClipRect.Left - X;
            DX = SD.ClipRect.Left;
            W -= SX;
        }
        else
        {
            SX = 0;
        }

        if (Right > SD.ClipRect.Right)
        {
            W -= Right - SD.ClipRect.Right;
        }

        if (W > 0)
        {
            Y += FSpriteY;
            DY = Y + FYshift;
            H = FSpriteHeight;
            Bottom = DY + FSpriteHeight;
            if (DY < SD.ClipRect.Top)
            {
                SY = SD.ClipRect.Top - Y;
                DY = SD.ClipRect.Top;
                H -= SY;
            }
            else
            {
                SY = 0;
            }

            if (Bottom > SD.ClipRect.Bottom)
            {
                H -= Bottom - SD.ClipRect.Bottom;
            }

            if (H > 0)
            {
                if (!FDataLoaded)
                {
                    LoadDiscardableImageData();
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        switch (FPixelFormatID)
                        {
                            case PixelFormats.pfRGB555:
                                BlendColorKey555(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendColorKey565(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        switch (FPixelFormatID)
                        {
                            case PixelFormats.pfRGB555:
                                BlendColorKey555(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendColorKey565(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                        }
                    }

                    if (FLoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    Src = View.At(SX * 2 + SY * FPitch);
                    Dst = SD.View((DX << 1) + SD.Pitch * DY);
                    if (SD.PixelFormatID == FPixelFormatID)
                    {
                        switch (FPixelFormatID)
                        {
                            case PixelFormats.pfRGB555:
                                BlendColorKey555(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendColorKey565(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        switch (FPixelFormatID)
                        {
                            case PixelFormats.pfRGB555:
                                BlendColorKey555(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendColorKey565(Src, Dst, W, H, FColorKey, ((BlendTable5Bit)Table).Bytes);
                                break;
                        }
                    }
                }
            }
        }
    }

    // ImageLib.pas:4543
    // { ShowTransparent shows the sprite blended 50% }
    public override void ShowTransparent(int X, int Y, in SurfaceDesc SD)
    {
        ShowBlended(X, Y, SD, GfxState.AlphaBlendTable.LookupTables[50]);
    }

    // ImageLib.pas:4548
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555 || PixelFormatID == PixelFormats.pfRGB24 || PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:4554
    public override Form CreateGrabForm(Component AOwner, in SurfaceDesc SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:4561
    // { GrabImage locks the conversion surface and never unlocks it (an original bug) }
    public override bool GrabImage(in SurfaceDesc SD, in Rect R, GrabInfo GrabInfo)
    {
        DataSurface Surface;
        SurfaceDesc LSD = default;
        Rect GR = default;
        Rect MR = default;
        int Left;
        int Top;
        int Bottom;
        // { Absolute views preserve frame slots and the separate GR coordinate reads. }
        bool Result = true;
        try
        {
            Surface = null;
            try
            {
                if (SD.BitCount != 16)
                {
                    Surface = new DataSurface(R.Right - R.Left, R.Bottom - R.Top, PixelFormats.pfRGB565, null);
                    GR.Left = 0;
                    GR.Top = 0;
                    var width = Surface.GetWidth();
                    GR.Right = width;
                    var height = Surface.GetHeight();
                    GR.Bottom = height;
                    Surface.Lock(ref LSD);
                    var surfaceWidth = Surface.GetWidth();
                    var surfaceHeight = Surface.GetHeight();
                    if (SurfaceDesc.RemapSurfacePart(SD, R.Left, R.Top, surfaceWidth, surfaceHeight, LSD, 0, 0) != 0)
                    {
                        throw new ArgumentException("Invalid PixelFormat");
                    }

                    FTransparentColor = PixelRemap.RemapPixelInfo(GrabInfo.TransparentPixel, SD.PixelFormatID, SD.Palette, LSD.PixelFormatID, null);
                }
                else
                {
                    LSD = SD;
                    GR = R;
                    FTransparentColor = GrabInfo.TransparentPixel;
                }

                Clear();
                FDataLoaded = true;
                FDataWidth = GR.Right - GR.Left;
                FDataHeight = GR.Bottom - GR.Top;
                FWidth = FDataWidth;
                FHeight = FDataHeight;
                FFileName = ExtractFileName(GrabInfo.FileName);
                FFileIndex = GrabInfo.FileIndex;
                FStoredPixelFormatID = LSD.PixelFormatID;
                FPixelFormatID = FStoredPixelFormatID;
                FColorKey = FTransparentColor;
                SurfaceDesc.MinimizeTransparentRect(LSD, GR, FTransparentColor, ref MR);
                Left = MR.Left;
                FSpriteX = Left - GR.Left;
                Top = MR.Top;
                FSpriteY = Top - GR.Top;
                FSpriteWidth = MR.Right - Left;
                Bottom = MR.Bottom;
                FSpriteHeight = Bottom - Top;
                FDataSize = FSpriteWidth * FSpriteHeight * 2;
                AllocateDiscardableImageData(FSpriteWidth, FSpriteHeight);
                for (int Y = Top; Y <= Bottom - 1; Y++)
                {
                    LSD.View().Bytes(LSD.Pitch * Y + MR.Left * 2, FSpriteWidth * 2).CopyTo(FBuffer.AsSpan((Y - MR.Top) * FPitch, FSpriteWidth * 2));
                }
            }
            finally
            {
                if (Surface != null)
                {
                    Surface.Dispose();
                }
            }
        }
        catch
        {
            ReleaseDiscardableImageData();
            Result = false;
        }

        return Result;
    }

    private static void BlendColorKey555(SurfaceView Src, SurfaceView Dst, int Width, int Height, int ColorKey, ReadOnlySpan<byte> Table)
    {
        Blit.BlendColorKey555(Src, Dst, Width, Height, ColorKey, Table);
    }

    private static void BlendColorKey565(SurfaceView Src, SurfaceView Dst, int Width, int Height, int ColorKey, ReadOnlySpan<byte> Table)
    {
        Blit.BlendColorKey565(Src, Dst, Width, Height, ColorKey, Table);
    }

}

// (ClassID $11): a run length encoded 16 bit sprite. FColorKey (the transparent color, in
// FPixelFormatID), FData (FDataSize bytes), FSpriteX, FSpriteY, FSpriteWidth, FSpriteHeight (as
// TSprite16). The data is FSpriteHeight rows. a row is a word with its size in bytes (the
// header included), a zero word, then pixels and skips (the color key and the size of the skip
// in bytes), padded with the color key to a dword
public partial class RLESprite16 : Image16, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 17;
    static string ILibraryImageClass.Name => "RLE Sprite (16bpp)";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new RLESprite16(owner);

    internal int FColorKey;
    internal byte[] FData;
    internal int FSpriteX;
    internal int FSpriteY;
    internal int FSpriteWidth;
    internal int FSpriteHeight;

    // ImageLib.pas:2881
    public RLESprite16(CustomImageLibrary owner) : base(owner)
    {
    }

    // ImageLib.pas:2896
    public override void SetXshift(int Value)
    {
        if (Value < -FSpriteX)
        {
            Value = -FSpriteX;
        }

        base.SetXshift(Value);
        SetWidth(FWidth);
    }

    // ImageLib.pas:2903
    public override void SetYshift(int Value)
    {
        if (Value < -FSpriteY)
        {
            Value = -FSpriteY;
        }

        base.SetYshift(Value);
        SetHeight(FHeight);
    }

    // ImageLib.pas:2910
    public override void SetWidth(int Value)
    {
        int W;
        W = FXshift + FSpriteX + FSpriteWidth;
        if (Value < W)
        {
            Value = W;
        }

        base.SetWidth(Value);
    }

    // ImageLib.pas:2919
    public override void SetHeight(int Value)
    {
        int W;
        W = FYshift + FSpriteY + FSpriteHeight;
        if (Value < W)
        {
            Value = W;
        }

        base.SetHeight(Value);
    }

    // ImageLib.pas:2928
    public override void GetVisualDimensions(ref Rect R)
    {
        R.Left = FXshift + FSpriteX;
        R.Right = R.Left + FSpriteWidth;
        R.Top = FYshift + FSpriteY;
        R.Bottom = R.Top + FSpriteHeight;
    }

    // ImageLib.pas:2936
    public override void ReleaseDiscardableImageData()
    {
        if (FDataLoaded)
        {
            FData = null;
            FDataLoaded = false;
        }
    }

    // ImageLib.pas:2947
    // { The last dword of the image data is not used: ReadImageData skips it and WriteImageData
    //   writes an uninitialized local variable (an original bug). }
    public override void ReadImageData(Stream Stream)
    {
        int Reserved = default;
        base.ReadImageData(Stream);
        Stream.ReadBytes(Bytes.Of(ref FSpriteWidth, 4));
        Stream.ReadBytes(Bytes.Of(ref FSpriteHeight, 4));
        Stream.ReadBytes(Bytes.Of(ref FSpriteX, 4));
        Stream.ReadBytes(Bytes.Of(ref FSpriteY, 4));
        Stream.ReadBytes(Bytes.Of(ref FColorKey, 4));
        Stream.ReadBytes(Bytes.Of(ref Reserved, 4));
    }

    // ImageLib.pas:2960
    public override void WriteImageData(Stream Stream)
    {
        int Reserved = default;
        base.WriteImageData(Stream);
        Stream.Write(Bytes.Of(ref FSpriteWidth, 4));
        Stream.Write(Bytes.Of(ref FSpriteHeight, 4));
        Stream.Write(Bytes.Of(ref FSpriteX, 4));
        Stream.Write(Bytes.Of(ref FSpriteY, 4));
        Stream.Write(Bytes.Of(ref FColorKey, 4));
        Stream.Write(Bytes.Of(ref Reserved, 4));
    }

    // ImageLib.pas:2973
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        FData = MemorySurfaces.Reallocate(FData, FDataSize);
        Stream.ReadBytes(FData.AsSpan(0, FDataSize));
    }

    // ImageLib.pas:2980
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
        Stream.Write(FData.AsSpan(0, FDataSize));
    }

    // ImageLib.pas:2986
    public override int RemapToPixelFormat(int PixelFormatID)
    {
        int Result = 0;
        if (PixelFormatID != FPixelFormatID)
        {
            switch (PixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    RemapRLE555to565(FData, FSpriteHeight, FColorKey);
                    FPixelFormatID = PixelFormats.pfRGB565;
                    break;
                case PixelFormats.pfRGB555:
                    RemapRLE565to555(FData, FSpriteHeight, FColorKey);
                    FPixelFormatID = PixelFormats.pfRGB555;
                    break;
                default:
                    Result = PixelFormats.GFXE_NOTSUPPORTED;
                    break;
            }
        }

        return Result;
    }

    // ImageLib.pas:3006
    public override bool SupportShowMode(ShowMode ShowMode, int PixelFormatID) => (PixelFormatID == 0 || PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555) && Set<ShowMode>.Of(ShowMode.Opaque, ShowMode.Transparent).Contains(ShowMode);

    // ImageLib.pas:3012
    public override void ShowOpaque(int X, int Y, in SurfaceDesc SD)
    {
        if (!FDataLoaded)
        {
            LoadDiscardableImageData();
            if (SD.PixelFormatID == FPixelFormatID)
            {
                BltRLE16(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
            }
            else
            {
                if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                BltRLE16(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
            }

            if (FLoadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            if (SD.PixelFormatID == FPixelFormatID)
            {
                BltRLE16(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
            }
            else
            {
                if (RemapToPixelFormat(SD.PixelFormatID) != 0)
                {
                    throw new NotSupportedException("Surface pixel format not supported!");
                }

                BltRLE16(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
            }
        }
    }

    // ImageLib.pas:3045
    // { ShowTransparent shows the sprite blended 50% }
    public override void ShowTransparent(int X, int Y, in SurfaceDesc SD)
    {
        if (!FDataLoaded)
        {
            LoadDiscardableImageData();
            if (SD.PixelFormatID != FPixelFormatID && RemapToPixelFormat(SD.PixelFormatID) != 0)
            {
                throw new NotSupportedException("Surface pixel format not supported!");
            }

            switch (FPixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    BlendRLE565(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                    break;
                case PixelFormats.pfRGB555:
                    BlendRLE555(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                    break;
            }

            if (FLoadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            if (SD.PixelFormatID != FPixelFormatID && RemapToPixelFormat(SD.PixelFormatID) != 0)
            {
                throw new NotSupportedException("Surface pixel format not supported!");
            }

            switch (FPixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    BlendRLE565(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                    break;
                case PixelFormats.pfRGB555:
                    BlendRLE555(X + FXshift + FSpriteX, Y + FYshift + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                    break;
            }
        }
    }

    // ImageLib.pas:3073
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555 || PixelFormatID == PixelFormats.pfRGB24 || PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:3079
    public override Form CreateGrabForm(Component AOwner, in SurfaceDesc SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:3090
    // The sprites take their transparent color from TGrabInfo (declared Reserved in the core),
    // not from TransparentColor (+4, the color GetSurfaceRectangles separates the images with).
    // GrabImage encodes the rows into FData, which grows in 4 KB steps. FDataSize stays 0 until
    // the end, so every word added past the first step reallocates (an original inefficiency)
    public override bool GrabImage(in SurfaceDesc SD, in Rect R, GrabInfo GrabInfo)
    {
        int Pos;
        int RowStart;
        int Skip;
        DataSurface Surface;
        SurfaceDesc LSD = default;
        Rect GR = default;
        Rect MR = default;
        int C;
        void AddWord(ushort W)
        {
            if (FDataSize <= Pos)
            {
                FData = MemorySurfaces.Reallocate(FData, Pos + 4096);
            }

            BinaryPrimitives.WriteUInt16LittleEndian(FData.AsSpan(Pos, 2), W);
            Pos += 2;
        }

        void StartRow()
        {
            RowStart = Pos;
            AddWord(0);
            AddWord(0);
        }

        void FlushSkip()
        {
            if (Skip != 0)
            {
                AddWord((ushort)FColorKey);
                AddWord((ushort)(Skip * 2));
                Skip = 0;
            }
        }

        void EndRow()
        {
            FlushSkip();
            if (FDataSize <= Pos)
            {
                FData = MemorySurfaces.Reallocate(FData, Pos + 4096);
            }

            BinaryPrimitives.WriteUInt16LittleEndian(FData.AsSpan(RowStart, 2), (ushort)(Pos - RowStart));

            if ((Pos & 3) != 0)
            {
                AddWord((ushort)FColorKey);
            }
        }

        bool Result = true;
        try
        {
            Surface = null;
            try
            {
                if (SD.BitCount != 16)
                {
                    Surface = new DataSurface(R.Right - R.Left, R.Bottom - R.Top, PixelFormats.pfRGB565, null);
                    GR.Left = 0;
                    GR.Top = 0;
                    var width = Surface.GetWidth();
                    GR.Right = width;
                    var height = Surface.GetHeight();
                    GR.Bottom = height;
                    Surface.Lock(ref LSD);
                    var surfaceWidth = Surface.GetWidth();
                    var surfaceHeight = Surface.GetHeight();
                    if (SurfaceDesc.RemapSurfacePart(SD, R.Left, R.Top, surfaceWidth, surfaceHeight, LSD, 0, 0) != 0)
                    {
                        throw new ArgumentException("Invalid PixelFormat");
                    }

                    FColorKey = PixelRemap.RemapPixelInfo(GrabInfo.TransparentPixel, SD.PixelFormatID, SD.Palette, LSD.PixelFormatID, null);
                }
                else
                {
                    LSD = SD;
                    GR = R;
                    FColorKey = GrabInfo.TransparentPixel;
                }

                Clear();
                FDataLoaded = true;
                FDataWidth = GR.Right - GR.Left;
                FDataHeight = GR.Bottom - GR.Top;
                FWidth = FDataWidth;
                FHeight = FDataHeight;
                FFileName = ExtractFileName(GrabInfo.FileName);
                FFileIndex = GrabInfo.FileIndex;
                FStoredPixelFormatID = LSD.PixelFormatID;
                FPixelFormatID = FStoredPixelFormatID;
                Pos = 0;
                Skip = 0;
                SurfaceDesc.MinimizeTransparentRect(LSD, GR, FColorKey, ref MR);
                FSpriteX = MR.Left - GR.Left;
                FSpriteY = MR.Top - GR.Top;
                FSpriteWidth = MR.Right - MR.Left;
                FSpriteHeight = MR.Bottom - MR.Top;
                for (int Y = MR.Top, last = MR.Bottom - 1; Y <= last; Y++)
                {
                    int Row = LSD.Pitch * Y;
                    StartRow();
                    for (int X = MR.Left, lastX = MR.Right - 1; X <= lastX; X++)
                    {
                        C = LSD.View().Word(Row + X + X);
                        if (C != FColorKey)
                        {
                            FlushSkip();
                            AddWord((ushort)C);
                        }
                        else
                        {
                            Skip += 1;
                        }
                    }

                    EndRow();
                }

                FData = MemorySurfaces.Reallocate(FData, Pos);
                FDataSize = Pos;
            }
            finally
            {
                if (Surface != null)
                {
                    Surface.Dispose();
                }
            }
        }
        catch
        {
            ReleaseDiscardableImageData();
            Result = false;
        }

        return Result;
    }

    private static void RemapRLE565to555(Span<byte> Data, int Height, int ColorKey)
    {
        Blit.RemapRLE565to555(Data, Height, ColorKey);
    }

    private static void RemapRLE555to565(Span<byte> Data, int Height, int ColorKey)
    {
        Blit.RemapRLE555to565(Data, Height, ColorKey);
    }

    private static void BltRLE16(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height, int ColorKey)
    {
        Blit.BltRLE16(X, Y, SD.View(), Data, Width, Height, ColorKey);
    }

    private static void BlendRLE565(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height, int ColorKey)
    {
        Blit.BlendRLE565(X, Y, SD.View(), Data, Width, Height, ColorKey);
    }

    private static void BlendRLE555(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height, int ColorKey)
    {
        Blit.BlendRLE555(X, Y, SD.View(), Data, Width, Height, ColorKey);
    }

}

// (ClassID $12): a TRLESprite16 shown transparent by default. it cannot be grabbed
public partial class TransparentRLESprite16 : RLESprite16, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 18;
    static string ILibraryImageClass.Name => "Transparent RLE Sprite (16bpp)";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new TransparentRLESprite16(owner);

    // ImageLib.pas:3592
    public TransparentRLESprite16(CustomImageLibrary owner) : base(owner)
    {
        FShowInfo.ShowMode = ShowMode.Transparent;
    }

    // ImageLib.pas:3608
    public override bool SupportGrabPixelFormat(int PixelFormatID) => false;

}

// a mask of one bit per pixel (ClassID $13), shown as white pixels. FPitch (bytes per row,
// whole dwords), FMaskData (rows of dwords, the first pixel in the high bit), FMaskLeft,
// FMaskTop, FMaskWidth, FMaskHeight (the rectangle of the mask in the data rectangle)
public partial class BitMask : LibraryImage, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 19;
    static string ILibraryImageClass.Name => "Bit Mask";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new BitMask(owner);

    public BitMask(CustomImageLibrary owner) : base(owner)
    { }

    internal int FPitch;
    internal byte[] FMaskData;
    internal int FMaskLeft;
    internal int FMaskTop;
    internal int FMaskWidth;
    internal int FMaskHeight;

    // ImageLib.pas:4662
    public override void SetXshift(int Value)
    {
        if (Value < -FMaskLeft)
        {
            Value = -FMaskLeft;
        }

        base.SetXshift(Value);
        SetWidth(FWidth);
    }

    // ImageLib.pas:4669
    public override void SetYshift(int Value)
    {
        if (Value < -FMaskTop)
        {
            Value = -FMaskTop;
        }

        base.SetYshift(Value);
        SetHeight(FHeight);
    }

    // ImageLib.pas:4676
    public override void SetWidth(int Value)
    {
        int W;
        W = FXshift + FMaskLeft + FMaskWidth;
        if (Value < W)
        {
            Value = W;
        }

        base.SetWidth(Value);
    }

    // ImageLib.pas:4685
    public override void SetHeight(int Value)
    {
        int H;
        H = FYshift + FMaskTop + FMaskHeight;
        if (Value < H)
        {
            Value = H;
        }

        base.SetHeight(Value);
    }

    // ImageLib.pas:4694
    public override void ReadImageData(Stream Stream)
    {
        base.ReadImageData(Stream);
        Stream.ReadBytes(Bytes.Of(ref FMaskWidth, 4));
        Stream.ReadBytes(Bytes.Of(ref FMaskHeight, 4));
        Stream.ReadBytes(Bytes.Of(ref FMaskLeft, 4));
        Stream.ReadBytes(Bytes.Of(ref FMaskTop, 4));
    }

    // ImageLib.pas:4703
    public override void WriteImageData(Stream Stream)
    {
        base.WriteImageData(Stream);
        Stream.Write(Bytes.Of(ref FMaskWidth, 4));
        Stream.Write(Bytes.Of(ref FMaskHeight, 4));
        Stream.Write(Bytes.Of(ref FMaskLeft, 4));
        Stream.Write(Bytes.Of(ref FMaskTop, 4));
    }

    // ImageLib.pas:4721
    public override void ReleaseDiscardableImageData()
    {
        FMaskData = null;
        FDataLoaded = false;
    }

    // ImageLib.pas:4727
    public override void ReadDiscardableImageData(Stream Stream)
    {
        base.ReadDiscardableImageData(Stream);
        AllocateMaskData(FMaskWidth, FMaskHeight);
        Stream.ReadBytes(FMaskData.AsSpan(0, FDataSize));
    }

    // ImageLib.pas:4734
    public override void WriteDiscardableImageData(Stream Stream)
    {
        base.WriteDiscardableImageData(Stream);
        Stream.Write(FMaskData.AsSpan(0, FDataSize));
    }

    // ImageLib.pas:4740
    public override bool SupportGrabPixelFormat(int PixelFormatID) => PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555 || PixelFormatID == PixelFormats.pfRGB24 || PixelFormatID == PixelFormats.pfPAL8;

    // ImageLib.pas:4746
    public override Images.GrabInfo CreateGrabInfo() => new GrabInfo();

    // ImageLib.pas:4751
    public override Form CreateGrabForm(Component AOwner, in SurfaceDesc SD, ImagesGrabbedEvent OnImagesGrabbed) => null;

    // ImageLib.pas:4759
    // { GrabImage sets a bit for every pixel that differs from the transparent pixel value. With an
    //   inverse grab info no bit is added at all (an original bug: the mask stays empty). }
    public override bool GrabImage(in SurfaceDesc SD, in Rect R, Images.GrabInfo GrabInfo)
    {
        int Bits;
        int BitCount;
        int Offset;
        int TransparentColor;
        int Left;
        int Top;
        int Bottom;
        Rect MinRect = default;
        void AddBit(bool Bit)
        {
            Bits = Bits << 1;
            if (Bit)
            {
                Bits += 1;
            }

            BitCount += 1;
            if (BitCount == 32)
            {
                BinaryPrimitives.WriteInt32LittleEndian(FMaskData.AsSpan(Offset, 4), Bits);

                Offset += 4;
                BitCount = 0;
                Bits = 0;
            }
        }

        void FlushBits()
        {
            Bits = Bits << (32 - BitCount);
            BinaryPrimitives.WriteInt32LittleEndian(FMaskData.AsSpan(Offset, 4), Bits);

            Offset += 4;
            BitCount = 0;
            Bits = 0;
        }

        Clear();
        bool Result = true;
        FDataLoaded = true;
        Bits = 0;
        Offset = 0;
        BitCount = 0;
        FDataWidth = R.Right - R.Left;
        FDataHeight = R.Bottom - R.Top;
        FWidth = FDataWidth;
        FHeight = FDataHeight;
        FFileName = ExtractFileName(GrabInfo.FileName);
        FFileIndex = GrabInfo.FileIndex;
        TransparentColor = GrabInfo.TransparentPixel;
        if (!((GrabInfo)GrabInfo).Inverse)
        {
            SurfaceDesc.MinimizeTransparentRect(SD, R, TransparentColor, ref MinRect);
        }
        else
        {
            MinimizeInvTransparentRect(SD, R, TransparentColor, ref MinRect);
        }

        Left = MinRect.Left;
        FMaskLeft = Left - R.Left;
        Top = MinRect.Top;
        FMaskTop = Top - R.Top;
        FMaskWidth = MinRect.Right - Left;
        Bottom = MinRect.Bottom;
        FMaskHeight = Bottom - Top;
        AllocateMaskData(FMaskWidth, FMaskHeight);
        for (int Y = Top; Y <= Bottom - 1; Y++)
        {
            Offset = (Y - MinRect.Top) * FPitch;
            for (int X = MinRect.Left, last = MinRect.Right - 1; X <= last; X++)
            {
                if (!((GrabInfo)GrabInfo).Inverse)
                {
                    AddBit(GetPixelInfo(SD, X, Y) != TransparentColor);
                }
            }

            FlushBits();
        }

        return Result;
    }

    // ImageLib.pas:4827
    // { ShowDC clears the surface to gray, not to BkColor. }
    public override void ShowDC(Rect? SrcRect, Rect? DestRect, int DC, int BkColor)
    {
        /* TODO: Implement Windows.WindowFromDC in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:4861
    public override void ShowOpaque(int X, int Y, in SurfaceDesc SD)
    {
        if (!FDataLoaded)
        {
            LoadDiscardableImageData();
        }

        ShowBitMask16(X + FXshift + FMaskLeft, Y + FYshift + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
    }

    public void AllocateMaskData(int AWidth, int AHeight)
    {
        FPitch = checked(((AWidth + 31) >> 5) * 4);
        FDataSize = checked(FPitch * FMaskHeight);
        // GrabImage flushes one extra zero dword after a full final word.
        FMaskData = MemorySurfaces.Reallocate(FMaskData, checked(FDataSize + 4));
    }

    internal static int GetPixelInfo(in SurfaceDesc SD, int X, int Y)
    {
        SurfaceView val;
        switch (SD.BitCount)
        {
            case 8:
                val = SD.View();
                return val.Byte(X + SD.Pitch * Y);
            case 16:
                val = SD.View();
                return val.Word(X + X + SD.Pitch * Y);
            case 24:
                val = SD.View();
                return (int)(val.Dword(X * 3 + SD.Pitch * Y) & 0xFFFFFF);
            case 32:
                val = SD.View();
                return (int)val.Dword(X * 4 + SD.Pitch * Y);
            default:
                throw new NotSupportedException("PixelFormat is not supported");
        }
    }

    internal static void MinimizeInvTransparentRect(in SurfaceDesc SD, in Rect R, int TransparentColor, ref Rect Result)
    {
        SurfaceDesc SD__value = SD;
        Rect val = R;
        bool flag = false;
        while (!flag && val.Top < val.Bottom)
        {
            int num = val.Left;
            while (!flag && num != val.Right)
            {
                flag = GetPixel(num, val.Top) == TransparentColor;
                num++;
            }
            if (!flag)
            {
                val.Top++;
            }
        }
        flag = false;
        while (!flag && val.Bottom > val.Top)
        {
            int num = val.Left;
            while (!flag && num != val.Right)
            {
                flag = GetPixel(num, val.Bottom - 1) == TransparentColor;
                num++;
            }
            if (!flag)
            {
                val.Bottom--;
            }
        }
        flag = false;
        while (!flag && val.Left < val.Right)
        {
            int num2 = val.Top;
            while (!flag && num2 != val.Bottom)
            {
                flag = GetPixel(val.Left, num2) == TransparentColor;
                num2++;
            }
            if (!flag)
            {
                val.Left++;
            }
        }
        flag = false;
        while (!flag && val.Right > val.Left)
        {
            int num2 = val.Top;
            while (!flag && num2 != val.Bottom)
            {
                flag = GetPixel(val.Right - 1, num2) == TransparentColor;
                num2++;
            }
            if (!flag)
            {
                val.Right--;
            }
        }
        Result = val;
        int GetPixel(int X, int Y)
        {
            SurfaceView val2;
            switch (SD__value.BitCount)
            {
                case 8:
                    val2 = SD__value.View();
                    return val2.Byte(X + SD__value.Pitch * Y);
                case 16:
                    val2 = SD__value.View();
                    return val2.Word(X + X + SD__value.Pitch * Y);
                case 24:
                    val2 = SD__value.View();
                    return (int)(val2.Dword(X * 3 + SD__value.Pitch * Y) & 0xFFFFFF);
                case 32:
                    val2 = SD__value.View();
                    return (int)val2.Dword(X * 4 + SD__value.Pitch * Y);
                default:
                    throw new NotSupportedException("PixelFormat is not supported");
            }
        }
    }

    private static void ShowBitMask16(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height)
    {
        Blit.ShowBitMask16(X, Y, SD.View(), Data, Width, Height);
    }

    // Inverse (grab the non-transparent rectangle with MinimizeInvTransparentRect)
    public partial class GrabInfo : Images.GrabInfo
    {
        public static new GrabInfoClass Descriptor { get; } = new("BitMask.GrabInfo", static () => new GrabInfo());

        public bool Inverse;

        // ImageLib.pas:4638
        public override void LoadFromStream(Stream Stream)
        {
            base.LoadFromStream(Stream);
            Stream.ReadBytes(Bytes.Of(ref Inverse, 1));
        }

        // ImageLib.pas:4644
        public override void SaveToStream(Stream Stream)
        {
            base.SaveToStream(Stream);
            Stream.Write(Bytes.Of(ref Inverse, 1));
        }

        // ImageLib.pas:4633
        public override GrabInfoClass GrabInfoClass() => Descriptor;
    }
}

// a bit mask shown as a shadow, darkening the 16 bit surface under the set bits (ClassID $14)
public partial class Shadow : BitMask, ILibraryImageClass
{
    static int ILibraryImageClass.ClassID => 20;
    static string ILibraryImageClass.Name => "Shadow";

    static LibraryImage ILibraryImageClass.Create(CustomImageLibrary owner) => new Shadow(owner);

    // ImageLib.pas:5100
    public Shadow(CustomImageLibrary owner) : base(owner)
    {
    }

    // ImageLib.pas:5106
    // { SupportShowMode asks TLibraryImage, which supports nothing. }
    public override bool SupportShowMode(ShowMode ShowMode, int PixelFormatID) => (PixelFormatID == PixelFormats.pfRGB565 || PixelFormatID == PixelFormats.pfRGB555) && base.SupportShowMode(ShowMode, PixelFormatID);

    // ImageLib.pas:5122
    public override void ShowOpaque(int X, int Y, in SurfaceDesc SD)
    {
        if (!FDataLoaded)
        {
            LoadDiscardableImageData();
            switch (SD.PixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    ShowShadow565(X + FXshift + FMaskLeft, Y + FYshift + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
                    break;
                case PixelFormats.pfRGB555:
                    ShowShadow555(X + FXshift + FMaskLeft, Y + FYshift + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
                    break;
                default:
                    throw new NotSupportedException("Surface format not supported");
            }

            if (FLoadMode == LoadMode.LoadFreeOnDemand)
            {
                ReleaseDiscardableImageData();
            }
        }
        else
        {
            switch (SD.PixelFormatID)
            {
                case PixelFormats.pfRGB565:
                    ShowShadow565(X + FXshift + FMaskLeft, Y + FYshift + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
                    break;
                case PixelFormats.pfRGB555:
                    ShowShadow555(X + FXshift + FMaskLeft, Y + FYshift + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
                    break;
                default:
                    throw new NotSupportedException("Surface format not supported");
            }
        }
    }

    private static void ShowShadow565(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height)
    {
        Blit.ShowShadow565(X, Y, SD.View(), Data, Width, Height);
    }

    private static void ShowShadow555(int X, int Y, in SurfaceDesc SD, ReadOnlySpan<byte> Data, int Width, int Height)
    {
        Blit.ShowShadow555(X, Y, SD.View(), Data, Width, Height);
    }

}

// an image library whose image N is the character with code N. FCharacterDistance (pixels
// between two characters), FHeight (the largest image height, set by LoadFromStream),
// FCharCase, FColor, FFontColorPaletteIndex (the entry of the first palette that SetColor sets
// to Color, -1 for none). A character without an image is 5 pixels wide
public partial class ImageLibraryFont : CustomImageLibrary
{

    internal int FCharacterDistance;
    internal int FHeight;
    internal EditCharCase FCharCase;
    internal int FColor;
    internal int FFontColorPaletteIndex;

    // ImageLib.pas:6266
    internal void SetFontColorPaletteIndex(int Value)
    {
        if (Value > 255)
        {
            Value = 255;
        }

        if (Value < 0)
        {
            Value = -1;
        }

        FFontColorPaletteIndex = Value;
    }

    // ImageLib.pas:6258
    public ImageLibraryFont(Component owner) : base(owner)
    {
        FCharacterDistance = 2;
        FFontColorPaletteIndex = -1;
        FCharCase = EditCharCase.Normal;
    }

    // ImageLib.pas:6291
    public override void LoadFromStream(Stream Stream)
    {
        int I;
        base.LoadFromStream(Stream);
        FHeight = 0;
        I = 0;
        while (I < FCount)
        {
            if (this[I] != null)
            {
                if (this[I].Height > FHeight)
                {
                    FHeight = this[I].Height;
                }
            }

            I += 1;
        }
    }

    // ImageLib.pas:6367
    public int GetTextWidth(string Text)
    {
        int Result;
        switch (FCharCase)
        {
            case EditCharCase.Normal:
                Result = TextWidth(Text);
                break;
            case EditCharCase.LowerCase:
                Result = TextWidth(LowerCase(Text));
                break;
            case EditCharCase.UpperCase:
                Result = TextWidth(UpperCase(Text));
                break;
            default:
                Result = 0;
                break;
        }

        return Result;
    }

    // ImageLib.pas:6378
    public int GetTextHeight(string Text)
    {
        int H;
        int C;
        string S;
        switch (FCharCase)
        {
            case EditCharCase.LowerCase:
                S = LowerCase(Text);
                break;
            case EditCharCase.UpperCase:
                S = UpperCase(Text);
                break;
            default:
                S = Text;
                break;
        }

        H = 0;
        for (int I = 1; I <= S.Length; I++)
        {
            C = Cp1252.Byte(S[I - 1]);
            if (C < FCount && this[C] != null && this[C].Height > H)
            {
                H = this[C].Height;
            }
        }

        return H;
    }

    // ImageLib.pas:6399
    public void SetText(int X, int Y, string Text, in SurfaceDesc SD)
    {
        switch (FCharCase)
        {
            case EditCharCase.Normal:
                ShowText(X, Y, Text, SD);
                break;
            case EditCharCase.LowerCase:
                ShowText(X, Y, LowerCase(Text), SD);
                break;
            case EditCharCase.UpperCase:
                ShowText(X, Y, UpperCase(Text), SD);
                break;
        }
    }

    // ImageLib.pas:6408
    public void SetTextCentered(int X, int Y, string Text, in SurfaceDesc SD)
    {
        string S;
        switch (FCharCase)
        {
            case EditCharCase.Normal:
                X = X - (int)(((uint)TextWidth(Text)) >> 1);
                ShowText(X, Y, Text, SD);
                break;
            case EditCharCase.LowerCase:
                S = LowerCase(Text);
                X = X - (int)(((uint)TextWidth(S)) >> 1);
                ShowText(X, Y, S, SD);
                break;
            case EditCharCase.UpperCase:
                S = UpperCase(Text);
                X = X - (int)(((uint)TextWidth(S)) >> 1);
                ShowText(X, Y, S, SD);
                break;
        }
    }

    // ImageLib.pas:6433
    public void SetTextEx(int X, int Y, string Text, in SurfaceDesc SD, Set<SetTextModeElement> Mode, in ShowInfo ShowInfo)
    {
        int C;
        string S;
        switch (FCharCase)
        {
            case EditCharCase.LowerCase:
                S = LowerCase(Text);
                break;
            case EditCharCase.UpperCase:
                S = UpperCase(Text);
                break;
            default:
                S = Text;
                break;
        }

        if (Mode.Contains(SetTextModeElement.Center))
        {
            X = X - (int)(((uint)TextWidth(S)) >> 1);
        }

        for (int I = 1; I <= S.Length; I++)
        {
            C = Cp1252.Byte(S[I - 1]);
            if (C < FCount && this[C] != null)
            {
                this[C].ShowEx(X, Y, SD, ShowInfo);
                X += this[C].Width + FCharacterDistance;
            }
            else
            {
                X += FCharacterDistance + 5;
            }
        }
    }

    public int Height { get => FHeight; }

    [Published]
    public int CharacterDistance { get => FCharacterDistance; set => FCharacterDistance = value; }

    [Published]
    public EditCharCase CharCase { get => FCharCase; set => FCharCase = value; }

    [Published]
    public int Color { get => FColor; set => SetColor(value); }

    [Published]
    public int FontColorPaletteIndex { get => FFontColorPaletteIndex; set => SetFontColorPaletteIndex(value); }

}

// { Splits Text into lines no wider than MaxWidth pixels in Font, at spaces and line breaks, and
//   adds them to Strings. }
// a panel that shows one image of an image library with BltLibraryImage. FScaleX, FScaleY
// (percent), FBltMode, FOnUpdated (called after every Paint), FImageLibrary, FImageIndex
public partial class CustomImageLibraryPanel : CustomPanel
{
    internal int FScaleX;
    internal int FScaleY;
    internal BltMode FBltMode;
    internal NotifyEvent<object> FOnUpdated;
    internal CustomImageLibrary FImageLibrary;
    internal int FImageIndex;

    // ImageLib.pas:6513
    internal void SetBltMode(BltMode Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6519
    internal void SetScaleX(int Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6525
    internal void SetScaleY(int Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6531
    internal void SetImageIndex(int Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6537
    internal void SetImageLibrary(CustomImageLibrary Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6543
    protected internal override void Paint()
    {
        /* TODO: Implement Controls.Canvas in the platform or editor service. */
        throw new NotImplementedException();
    }

    protected internal CustomImageLibrary ImageLibrary { get => FImageLibrary; set => SetImageLibrary(value); }
    protected internal int ImageIndex { get => FImageIndex; set => SetImageIndex(value); }
    protected internal BltMode BltMode { get => FBltMode; set => SetBltMode(value); }
    protected internal NotifyEvent<object> OnUpdated { get => FOnUpdated; set => FOnUpdated = value; }
    protected internal int ScaleX { get => FScaleX; set => SetScaleX(value); }
    protected internal int ScaleY { get => FScaleY; set => SetScaleY(value); }

    // ImageLib.pas:6504

    public CustomImageLibraryPanel(Component owner) : base(owner)
    {
        /* TODO: Implement ExtCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }
}

public partial class ImageLibraryPanel : CustomImageLibraryPanel
{
    public ImageLibraryPanel(Component owner) : base(owner)
    { }

}

// a grid of Count image cells. FScaleX, FScaleY, FBltMode, FEditMode, FOnCellUpdated (draws a
// cell at run time), FOnSelectCell, FFocusColor, FGrowDirection, FSelectionEnabled,
// FMinRowCount, FMinColCount, FMaxRowCount, FMaxColCount (0 for no limit), FShowGridIndex,
// FCount. Ctrl+X/C/V/Del call the edit methods, Ins and Del without a modifier InsCell and
// DelCell. they do nothing here
public partial class CustomImageDrawGrid : CustomGrid
{
    internal int FScaleX;
    internal int FScaleY;
    internal BltMode FBltMode;
    internal Set<ILGridEditModeElement> FEditMode;
    internal DrawCellEvent<object> FOnCellUpdated;
    internal SelectCellEvent<object> FOnSelectCell;
    internal int FFocusColor;
    internal ILGrowDirection FGrowDirection;
    internal bool FSelectionEnabled;
    internal int FMinRowCount;
    internal int FMinColCount;
    internal int FMaxRowCount;
    internal int FMaxColCount;
    internal bool FShowGridIndex;
    internal int FCount;

    // ImageLib.pas:6705
    internal void SetBltMode(BltMode Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6711
    internal void SetSelectionEnabled(bool Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6717
    internal void SetFocusColor(int Value)
    {
        FFocusColor = Value;
    }

    // ImageLib.pas:6722
    internal void SetScaleX(int Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6728
    internal void SetScaleY(int Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6734
    internal void SetShowGridIndex(bool Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6740
    internal int GetColCount()
    {
        /* TODO: Implement Grids.ColCount in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6745
    internal new void SetColCount(int Value)
    {
        /* TODO: Implement Grids.ColCount in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6752
    internal void SetMinRowCount(int Value)
    {
        FMinRowCount = Value;
        _ = RowCount;
    }

    // ImageLib.pas:6758
    internal void SetMinColCount(int Value)
    {
        FMinColCount = Value;
        _ = ColCount;
    }

    // ImageLib.pas:6764
    internal void SetMaxRowCount(int Value)
    {
        FMaxRowCount = Value;
        _ = RowCount;
    }

    // ImageLib.pas:6770
    internal void SetMaxColCount(int Value)
    {
        FMaxColCount = Value;
        _ = ColCount;
    }

    // ImageLib.pas:6776
    internal int GetRowCount()
    {
        /* TODO: Implement Grids.RowCount in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6781
    internal new void SetRowCount(int Value)
    {
        /* TODO: Implement Grids.RowCount in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6788
    protected internal override void DrawCell(int ACol, int ARow, Rect ARect, Set<GridDrawState> AState)
    {
        /* TODO: Implement Graphics.Color in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6613
    protected internal override bool SelectCell(int ACol, int ARow)
    {
        bool Result;
        Result = true;
        if (FOnSelectCell != null)
        {
            FOnSelectCell(this, ACol, ARow, ref Result);
        }

        FSelectionEnabled = true;
        return Result;
    }

    // ImageLib.pas:6620
    protected internal override void KeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        /* TODO: Implement Grids.KeyDown in the platform or editor service. */
        throw new NotImplementedException();
    }

    protected internal bool SelectionEnabled { get => FSelectionEnabled; set => SetSelectionEnabled(value); }
    protected internal int FocusColor { get => FFocusColor; set => SetFocusColor(value); }
    protected internal BltMode BltMode { get => FBltMode; set => SetBltMode(value); }
    protected internal DrawCellEvent<object> OnCellUpdated { get => FOnCellUpdated; set => FOnCellUpdated = value; }
    protected internal SelectCellEvent<object> OnSelectCell { get => FOnSelectCell; set => FOnSelectCell = value; }
    protected internal int ScaleX { get => FScaleX; set => SetScaleX(value); }
    protected internal int ScaleY { get => FScaleY; set => SetScaleY(value); }
    protected internal new int ColCount { get => GetColCount(); set => SetColCount(value); }
    protected internal new int RowCount { get => GetRowCount(); set => SetRowCount(value); }
    protected internal Set<ILGridEditModeElement> EditMode { get => FEditMode; set => FEditMode = value; }
    protected internal ILGrowDirection GrowDirection { get => FGrowDirection; set => FGrowDirection = value; }
    protected internal int MinRowCount { get => FMinRowCount; set => SetMinRowCount(value); }
    protected internal int MinColCount { get => FMinColCount; set => SetMinColCount(value); }
    protected internal int MaxRowCount { get => FMaxRowCount; set => SetMaxRowCount(value); }
    protected internal int MaxColCount { get => FMaxColCount; set => SetMaxColCount(value); }
    protected internal bool ShowGridIndex { get => FShowGridIndex; set => SetShowGridIndex(value); }

    // ImageLib.pas:6594

    public CustomImageDrawGrid(Component owner) : base(owner)
    {
        /* TODO: Implement Grids.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6661
    public virtual void SetCount(int Value)
    {
        FCount = Value;
        UpdateDimensions();
    }

    // ImageLib.pas:6668
    public virtual void UpdateDimensions()
    {
        switch (FGrowDirection)
        {
            case ILGrowDirection.Horizontal:
                var rowCount = RowCount;
                var rowCount2 = RowCount;
                ColCount = (rowCount + FCount) / rowCount2;
                break;
            case ILGrowDirection.Vertical:
                var colCount = ColCount;
                var colCount2 = ColCount;
                RowCount = (colCount + FCount) / colCount2;
                break;
        }
    }

    // ImageLib.pas:6637
    public virtual void CutCell()
    {
    }

    // ImageLib.pas:6641
    public virtual void CopyCell()
    {
    }

    // ImageLib.pas:6645
    public virtual void DeleteCell()
    {
    }

    // ImageLib.pas:6649
    public virtual void PasteCell()
    {
    }

    // ImageLib.pas:6653
    public virtual void InsCell()
    {
    }

    // ImageLib.pas:6657
    public virtual void DelCell()
    {
    }

    // ImageLib.pas:6676
    public virtual int ImageIndex(int ACol, int ARow) => ColCount * ARow + ACol;

    // ImageLib.pas:6681
    public virtual void IndexToCell(int Index, ref int ACol, ref int ARow)
    {
        ACol = Index % ColCount;
        ARow = Index / ColCount;
    }

    // ImageLib.pas:6687
    public void SelectIndex(int Index)
    {
        /* TODO: Implement Grids.Left in the platform or editor service. */
        throw new NotImplementedException();
    }

    public int Count { get => FCount; set => SetCount(value); }

    public enum ILGrowDirection : byte
    {
        None = 0, Horizontal = 1, Vertical = 2
    }
}

public partial class ImageDrawGrid : CustomImageDrawGrid
{
    public ImageDrawGrid(Component owner) : base(owner)
    { }

}

// a grid of the images of an image library, FImageLibrary. The edit methods work on the
// selected cells, through the clipboard format of TCustomImageLibrary, when EditMode allows
// them
public partial class CustomImageLibraryGrid : CustomImageDrawGrid
{
    public CustomImageLibraryGrid(Component owner) : base(owner)
    { }

    internal CustomImageLibrary FImageLibrary;

    // ImageLib.pas:6934
    internal void SetImageLibrary(CustomImageLibrary Value)
    {
        /* TODO: Implement Controls.Invalidate in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6942
    protected internal override void DrawCell(int ACol, int ARow, Rect ARect, Set<GridDrawState> AState)
    {
        /* TODO: Implement Controls.Canvas in the platform or editor service. */
        throw new NotImplementedException();
    }

    protected internal CustomImageLibrary ImageLibrary { get => FImageLibrary; set => SetImageLibrary(value); }

    // ImageLib.pas:6818
    public CustomImageLibrary CreateSelectedIL()
    {
        /* TODO: Implement Grids.Top in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6834
    public override void InsCell()
    {
        /* TODO: Implement Grids.Col in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6843
    public override void DelCell()
    {
        /* TODO: Implement Grids.Col in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6852
    public override void CutCell()
    {
        /* TODO: Implement Grids.Top in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6877
    public override void CopyCell()
    {
        CustomImageLibrary L;
        if (FEditMode.Contains(ILGridEditModeElement.Copy) && FImageLibrary != null)
        {
            L = CreateSelectedIL();
            try
            {
                L.CopyToClipboard();
            }
            finally
            {
                L?.Dispose();
            }
        }
    }

    // ImageLib.pas:6892
    public override void DeleteCell()
    {
        /* TODO: Implement Grids.Top in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:6907
    public override void PasteCell()
    {
        /* TODO: Implement Grids.Top in the platform or editor service. */
        throw new NotImplementedException();
    }
}

public partial class ImageLibraryGrid : CustomImageLibraryGrid
{
    public ImageLibraryGrid(Component owner) : base(owner)
    { }

}

// the libraries a program shares, linked by file name. FSearchPath (upper case, ends with a
// backslash), FDiskList, FOwnedList
public partial class ImageLibraryManager : Component
{

    internal string FSearchPath = "";
    internal ILManagerList FDiskList;
    internal ILManagerList FOwnedList;

    // ImageLib.pas:8260
    public ImageLibraryManager(Component owner) : base(owner)
    {
        FDiskList = new ILManagerList(this);
        FOwnedList = new ILManagerList(this);
    }

    // ImageLib.pas:8267
    public override void Dispose()
    {
        Clear();
        FDiskList?.Dispose();
        FOwnedList?.Dispose();
        base.Dispose();
    }

    // ImageLib.pas:8275
    public void Clear()
    {
        FDiskList.Clear();
        FOwnedList.Clear();
    }

    // ImageLib.pas:8281
    public void UnloadIL(Set<ILManagerCategoryElement> Category)
    {
        if (Category.Contains(ILManagerCategoryElement.Disk))
        {
            FDiskList.UnloadIL();
        }

        if (Category.Contains(ILManagerCategoryElement.Owned))
        {
            FOwnedList.UnloadIL();
        }
    }

    // ImageLib.pas:8288
    // { Lists the .ILB files of the search path and of its subdirectories in FDiskList }
    public void ExecutePathSearch(string Path)
    {
        string Dir;
        void SearchDirectory(string SubDirectory)
        {
            StringList List;
            SearchRec SR = default;
            bool Found;
            Found = FindFirst(FSearchPath + SubDirectory + "*.ilb", 63 - 16, ref SR) == 0;
            while (Found)
            {
                FDiskList.AddILFile(SubDirectory + UpperCase(SR.Name));
                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            List = new StringList();
            Found = FindFirst(SubDirectory + "*.*", 16, ref SR) == 0;
            while (Found)
            {
                if ((SR.Attr & 16) != 0 && SR.Name != "." && SR.Name != "..")
                {
                    List.Add(SubDirectory + UpperCase(SR.Name) + "\\");
                }

                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            for (int I = 0, last = List.Count - 1; I <= last; I++)
            {
                SearchDirectory(List[I]);
            }

            List?.Dispose();
        }

        FDiskList.Clear();
        FSearchPath = UpperCase(Path);
        if (FSearchPath[FSearchPath.Length - 1] != 92)
        {
            FSearchPath = FSearchPath + "\\";
        }

        Dir = GetCurrentDir();
        SetCurrentDir(Path);
        SearchDirectory("");
        SetCurrentDir(Dir);
    }

    // ImageLib.pas:8331
    // { A missing owned library is created empty and added. The exception for a missing library is
    //   raised and swallowed at once (it only shows in the debugger), as in the original. }
    public CustomImageLibrary LinkToIL(Set<ILManagerCategoryElement> Category, string FileName)
    {
        CustomImageLibrary Result = null;
        if (Category.Contains(ILManagerCategoryElement.Disk))
        {
            Result = FDiskList.LinkToIL(FileName);
        }

        if (Category.Contains(ILManagerCategoryElement.Owned) && Result == null)
        {
            Result = FOwnedList.LinkToIL(FileName);
            if (Result == null)
            {
                Result = new ImageLibrary(null);
                FOwnedList.AddILManagerObject(FileName, Result);
            }
        }

        if (Result == null)
        {
            try
            {
                throw new FileNotFoundException("Missing IL Resource: " + FileName);
            }
            catch
            {
            }
        }

        return Result;
    }

    // ImageLib.pas:8352
    public void ReleaseIL(Set<ILManagerCategoryElement> Category, string FileName)
    {
        bool Released;
        Released = false;
        if (Category.Contains(ILManagerCategoryElement.Disk))
        {
            Released = FDiskList.ReleaseIL(FileName);
        }

        if (Category.Contains(ILManagerCategoryElement.Owned) && !Released)
        {
            FOwnedList.ReleaseIL(FileName);
        }
    }

    // ImageLib.pas:8361
    public void SaveToStream(Stream Stream)
    {
        FOwnedList.SaveToStream(Stream);
    }

    // ImageLib.pas:8366
    public void LoadFromStream(Stream Stream)
    {
        FOwnedList.LoadFromStream(Stream);
    }

    public ILManagerList DiskList { get => FDiskList; }
    public ILManagerList OwnedList { get => FOwnedList; }

    [Published]
    public string SearchPath { get => FSearchPath; set => FSearchPath = value; }

}

[LegacyObject]
// an entry of a TILManagerList. FRefCount (the LinkToIL calls not yet released), FImageLibrary
// (loaded by the first LinkToIL)
public partial class ILManagerObject
{
    internal int FRefCount;
    internal CustomImageLibrary FImageLibrary;

    public int RefCount { get => FRefCount; }
    public CustomImageLibrary ImageLibrary { get => FImageLibrary; }

    public ILManagerObject()
    {
        CaptureAddress.Assign(this);
    }
}

// library file names (upper case, relative to the search path) with their TILManagerObject.
// FHashTable (the objects by name), FOwner
public partial class ILManagerList : StringList
{
    internal StringHashTable256 FHashTable;
    internal ImageLibraryManager FOwner;

    // ImageLib.pas:8093
    public ILManagerList(ImageLibraryManager owner)
    {
        FOwner = owner;
        FHashTable = new StringHashTable256();
    }

    // ImageLib.pas:8102
    // { Original bug: the inherited destructor does not call Clear, so the TILManagerObjects and
    //   their libraries are never freed. }
    public override void Dispose()
    {
        base.Dispose();
        FHashTable?.Dispose();
    }

    // ImageLib.pas:8108
    public CustomImageLibrary GetIL(int Index) => ((ILManagerObject)Objects[Index]).FImageLibrary;

    // ImageLib.pas:8113
    public void UnloadIL()
    {
        ILManagerObject Obj;
        for (int I = Count - 1; I >= 0; I--)
        {
            Obj = (ILManagerObject)Objects[I];
            if (Obj != null && Obj.FImageLibrary != null)
            {
                Obj.FImageLibrary.ReleaseDiscardableImageData();
            }
        }
    }

    // ImageLib.pas:8126
    public ILManagerObject AddILFile(string FileName)
    {
        ILManagerObject Obj;
        Obj = new ILManagerObject();
        FileName = UpperCase(FileName);
        AddObject(FileName, Obj);
        FHashTable.Add(Obj, FileName);
        return Obj;
    }

    // ImageLib.pas:8139
    // { Original bug: the object is deleted from the hash table instead of added, so LinkToIL never
    //   finds the owned libraries. }
    public ILManagerObject AddILManagerObject(string Name, CustomImageLibrary ImageLibrary)
    {
        string S;
        ILManagerObject Obj;
        S = UpperCase(Name);
        Obj = new ILManagerObject();
        Obj.FImageLibrary = ImageLibrary;
        AddObject(S, Obj);
        FHashTable.Delete(S);
        return Obj;
    }

    // ImageLib.pas:8153
    public override void Delete(int Index)
    {
        ILManagerObject Obj;
        Obj = (ILManagerObject)Objects[Index];
        if (Obj != null)
        {
            Obj.FImageLibrary?.Dispose();
        }

        var item = Strings[Index];
        FHashTable.Delete(item);
        base.Delete(Index);
    }

    // ImageLib.pas:8167
    public override void Clear()
    {
        ILManagerObject Obj;
        FHashTable.Clear();
        for (int I = Count - 1; I >= 0; I--)
        {
            Obj = (ILManagerObject)Objects[I];
            if (Obj != null)
            {
                Obj.FImageLibrary?.Dispose();
            }
        }

        base.Clear();
    }

    // ImageLib.pas:8187
    // { Links to the library of FileName (relative to the search path), loading it on the first link.
    //   The original clears FileName at the end (a second @LStrClr of the parameter). }
    public CustomImageLibrary LinkToIL(string FileName)
    {
        CustomImageLibrary Result;
        ILManagerObject Obj;
        FileName = UpperCase(FileName);
        Obj = (ILManagerObject)FHashTable.Find(FileName);
        if (Obj != null)
        {
            Obj.FRefCount += 1;
            if (Obj.FImageLibrary == null)
            {
                Obj.FImageLibrary = new ImageLibrary(null);
                Obj.FImageLibrary.LoadOnDemand = true;
                try
                {
                    Obj.FImageLibrary.LoadFromFile(FOwner.FSearchPath + FileName);
                }
                catch
                {
                    throw new IOException("Error loading: " + FOwner.FSearchPath + FileName);
                }
            }

            Result = Obj.FImageLibrary;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // ImageLib.pas:8213
    public bool ReleaseIL(string FileName)
    {
        bool Result;
        ILManagerObject Obj;
        FileName = UpperCase(FileName);
        Obj = (ILManagerObject)FHashTable.Find(FileName);
        if (Obj != null)
        {
            Obj.FRefCount -= 1;
            if (Obj.FRefCount == 0)
            {
                Obj.FImageLibrary.ReleaseDiscardableImageData();
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // ImageLib.pas:8229
    public override void LoadFromStream(Stream Stream)
    {
        int N = default;
        string Name = "";
        CustomImageLibrary IL;
        Stream.ReadBytes(Bytes.Of(ref N, 4));
        for (int I = 0, last = N - 1; I <= last; I++)
        {
            LibraryDrawing.ReadStringFromStream(ref Name, Stream);
            IL = new ImageLibrary(null);
            IL.LoadFromStream(Stream);
            AddILManagerObject(Name, IL);
        }
    }

    // ImageLib.pas:8245
    public override void SaveToStream(Stream Stream)
    {
        int I;
        I = Count;
        Stream.Write(Bytes.Of(ref I, 4));
        I = 0;
        int last = Count - 1;
        for (; I <= last; I += 1)
        {
            LibraryDrawing.WriteStringToStream(Strings[I], Stream);
            ((ILManagerObject)Objects[I]).FImageLibrary.SaveToStream(Stream);
        }
    }

}

// a combo box with the library names of a manager. FILManager, FCategory, FSubDirectory (only
// the names in it are listed, without it), FSkipDirectories (separated by semicolons),
// FEmptyILName (the first item, for no library)
public partial class ILManagerComboBox : ComboBox
{
    internal ImageLibraryManager FILManager;
    internal Set<ILManagerCategoryElement> FCategory;
    internal string FSubDirectory = "";
    internal string FSkipDirectories = "";
    internal string FEmptyILName = "";

    // ImageLib.pas:8373
    internal void SetILManager(ImageLibraryManager Value)
    {
        FILManager = Value;
        GenerateItems();
    }

    // ImageLib.pas:8379
    internal void SetSubDirectory(string Value)
    {
        FSubDirectory = UpperCase(Value);
        if (!IsEmpty(Value) && FSubDirectory[FSubDirectory.Length - 1] != 92)
        {
            FSubDirectory = FSubDirectory + "\\";
        }
    }

    // ImageLib.pas:8393
    internal string GetImageLibraryName()
    {
        /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:8401
    internal void SetImageLibraryName(string Value)
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:8386

    public ILManagerComboBox(Component owner) : base(owner)
    {
        /* TODO: Implement StdCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:8414
    public void GenerateItems()
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    public string ImageLibraryName { get => GetImageLibraryName(); set => SetImageLibraryName(value); }

    [Published]
    public ImageLibraryManager ILManager { get => FILManager; set => SetILManager(value); }

    [Published]
    public Set<ILManagerCategoryElement> Category { get => FCategory; set => FCategory = value; }

    [Published]
    public string SubDirectory { get => FSubDirectory; set => SetSubDirectory(value); }

    [Published]
    public string SkipDirectories { get => FSkipDirectories; set => FSkipDirectories = value; }

    [Published]
    public string EmptyILName { get => FEmptyILName; set => FEmptyILName = value; }
}

// the list box version of TILManagerComboBox. FILManager, FCategory, FSubDirectory,
// FSkipDirectories, FEmptyILName
public partial class ILManagerListBox : ListBox
{
    internal ImageLibraryManager FILManager;
    internal Set<ILManagerCategoryElement> FCategory;
    internal string FSubDirectory = "";
    internal string FSkipDirectories = "";
    internal string FEmptyILName = "";

    // ImageLib.pas:8463
    internal void SetILManager(ImageLibraryManager Value)
    {
        FILManager = Value;
        GenerateItems();
    }

    // ImageLib.pas:8469
    internal void SetSubDirectory(string Value)
    {
        FSubDirectory = UpperCase(Value);
        if (!IsEmpty(Value) && FSubDirectory[FSubDirectory.Length - 1] != 92)
        {
            FSubDirectory = FSubDirectory + "\\";
        }
    }

    // ImageLib.pas:8483
    internal string GetImageLibraryName()
    {
        /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:8491
    internal void SetImageLibraryName(string Value)
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:8476

    public ILManagerListBox(Component owner) : base(owner)
    {
        /* TODO: Implement StdCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:8504
    public void GenerateItems()
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    public string ImageLibraryName { get => GetImageLibraryName(); set => SetImageLibraryName(value); }

    [Published]
    public ImageLibraryManager ILManager { get => FILManager; set => SetILManager(value); }

    [Published]
    public Set<ILManagerCategoryElement> Category { get => FCategory; set => FCategory = value; }

    [Published]
    public string SubDirectory { get => FSubDirectory; set => SetSubDirectory(value); }

    [Published]
    public string SkipDirectories { get => FSkipDirectories; set => FSkipDirectories = value; }

    [Published]
    public string EmptyILName { get => FEmptyILName; set => FEmptyILName = value; }
}

public enum SequenceMode : byte
{
    None = 0,
    Up = 1,
    UpDown = 2,
    Random = 3,
    FrameTable = 4,
    RndFrameTable = 5
}

// the image sequences of the indices, each the first layer of a chain (nil for none). FLinked
// (the sequences are linked to their libraries), FILManager, FImageLibrary (the library of the
// sequences without a library name)
public partial class ImageSequenceList : DelphiList
{
    internal bool FLinked;
    // { HSEngine's TMapObjectResource.MsgProc writes FILManager directly }
    public ImageLibraryManager FILManager;
    internal CustomImageLibrary FImageLibrary;

    // ImageLib.pas:8903
    public ImageSequenceList(ImageLibraryManager AILManager, CustomImageLibrary AImageLibrary)
    {
        FILManager = AILManager;
        FImageLibrary = AImageLibrary;
    }

    // ImageLib.pas:8911
    public override void Dispose()
    {
        DestroyItems();
        base.Dispose();
    }

    // ImageLib.pas:8918
    // { The indices with a sequence, separated by commas }
    public string UsedIndexString()
    {
        string Result = "";
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if (List[I] != null)
            {
                Result = Result + IntToStr(I) + ",";
            }
        }

        if (!IsEmpty(Result))
        {
            Result = Copy(Result, 1, Result.Length - 1);
        }

        return Result;
    }

    // ImageLib.pas:8928
    public void DestroyItems()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            if (List[I] != null)
            {
                ((List[I] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
            }

            base.Delete(I);
        }
    }

    // ImageLib.pas:8939
    public ImageSequence GetLayerImageSequence(int Index, int Layer)
    {
        ImageSequence Result;
        if (Index < Count)
        {
            Result = (ImageSequence)List[Index];
        }
        else
        {
            Result = null;
        }

        while (Result != null && Layer != Result.FLayer)
        {
            Result = Result.FNext;
        }

        return Result;
    }

    // ImageLib.pas:8949
    // { Deletes the empty indices at the end; returns the last index with a sequence }
    public int ReduceCount()
    {
        int Result = Count - 1;
        while (Result >= 0 && List[Result] == null)
        {
            base.Delete(Result);
            Result -= 1;
        }

        return Result;
    }

    // ImageLib.pas:8959
    public void LoadFromStream(Stream Stream)
    {
        int N = default;
        bool IsNil = default;
        ImageSequence Sequence;
        DestroyItems();
        Stream.ReadBytes(Bytes.Of(ref N, 4));
        for (int I = 0, last = N - 1; I <= last; I++)
        {
            Stream.ReadBytes(Bytes.Of(ref IsNil, 1));
            if (!IsNil)
            {
                Sequence = new ImageSequence(this);
                Sequence.LoadFromStream(Stream);
                base.Add(Sequence);
            }
            else
            {
                base.Add(null);
            }
        }

        if (FLinked)
        {
            FLinked = false;
            LinkToIL();
        }
    }

    // ImageLib.pas:8986
    public void SaveToStream(Stream Stream)
    {
        bool IsNil = default;
        ReduceCount();
        Stream.Write(ValueBytes(Count, 4));
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            IsNil = List[I] == null;
            Stream.Write(Bytes.Of(ref IsNil, 1));
            if (!IsNil)
            {
                ((ImageSequence)List[I]).SaveToStream(Stream);
            }
        }
    }

    // ImageLib.pas:9001
    public void SaveToIniFile(string Section, IniFile IniFile)
    {
        /* TODO: Implement IniFiles.WriteString in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:9016
    public void LoadFromIniFile(string Section, IniFile IniFile)
    {
        /* TODO: Implement IniFiles.ReadString in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:9044
    public ImageSequence GetImageSequence(int Index)
    {
        ImageSequence Result;
        if (Index >= 0 && Index < Count)
        {
            Result = (ImageSequence)List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // ImageLib.pas:9052
    public void SetImageSequence(int Index, ImageSequence Value)
    {
        while (Index >= Count)
        {
            base.Add(null);
        }

        if (List[Index] != null)
        {
            ((List[Index] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
        }

        List[Index] = Value;
    }

    // ImageLib.pas:9059
    public int GetSubCount(int Index)
    {
        ImageSequence Sequence;
        int Result = 0;
        if (Index < Count)
        {
            Sequence = (ImageSequence)List[Index];
        }
        else
        {
            Sequence = null;
        }

        while (Sequence != null)
        {
            Sequence = Sequence.FNext;
            Result += 1;
        }

        return Result;
    }

    // ImageLib.pas:9076
    // { Original bug: a Sequence that is not in the chain runs past its end (nil dereference). }
    public int IndexOfSub(int Index, ImageSequence Sequence)
    {
        ImageSequence S;
        int Result = 0;
        if (Index < Count)
        {
            S = (ImageSequence)List[Index];
        }
        else
        {
            S = null;
        }

        while (S != Sequence)
        {
            S = S.FNext;
            Result += 1;
        }

        if (S == null)
        {
            Result = -1;
        }

        return Result;
    }

    // ImageLib.pas:9093
    public ImageSequence GetSubImageSequence(int Index, int SubIndex)
    {
        ImageSequence Result;
        int I;
        I = 0;
        if (Index < Count)
        {
            Result = (ImageSequence)List[Index];
        }
        else
        {
            Result = null;
        }

        while (Result != null && I != SubIndex)
        {
            Result = Result.FNext;
            I += 1;
        }

        return Result;
    }

    // ImageLib.pas:9111
    // { Inserts Sequence into the chain of Index by its layer. Original bug: an existing layer is
    //   only detected after the first link, not at the head of the chain. }
    public void Add(int Index, ImageSequence Sequence)
    {
        ImageSequence S;
        ImageSequence Prev;
        if (Index < Count)
        {
            S = (ImageSequence)List[Index];
        }
        else
        {
            S = null;
        }

        if (S != null)
        {
            Prev = null;
            while (S != null && S.FLayer < Sequence.FLayer)
            {
                Prev = S;
                S = S.FNext;
            }

            if (Prev != null)
            {
                if (S != null && Sequence.FLayer == S.FLayer)
                {
                    throw new InvalidOperationException("Layer does already exist");
                }

                Prev.FNext = Sequence;
                Sequence.FNext = S;
            }
            else
            {
                List[Index] = Sequence;
                Sequence.FNext = S;
            }
        }
        else
        {
            SetImageSequence(Index, Sequence);
        }
    }

    // ImageLib.pas:9145
    // { Takes Sequence out of the chain of Index (it is not freed) }
    public void Delete(int Index, ImageSequence Sequence)
    {
        ImageSequence S;
        ImageSequence Prev;
        if (Index < Count)
        {
            Prev = null;
            S = (ImageSequence)List[Index];
            while (S != null && S != Sequence)
            {
                Prev = S;
                S = S.FNext;
            }

            if (S != null && S == Sequence)
            {
                if (Prev != null)
                {
                    Prev.FNext = S.FNext;
                }
                else
                {
                    List[Index] = S.FNext;
                }

                S.FNext = null;
            }
        }
    }

    // ImageLib.pas:9169
    public void LinkToIL()
    {
        ImageSequence Sequence;
        if (!FLinked)
        {
            FLinked = true;
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                Sequence = (ImageSequence)List[I];
                while (Sequence != null)
                {
                    Sequence.LinkToIL();
                    Sequence = Sequence.FNext;
                }
            }
        }
    }

    // ImageLib.pas:9189
    public void ReleaseIL()
    {
        ImageSequence Sequence;
        if (FLinked)
        {
            FLinked = false;
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                Sequence = (ImageSequence)List[I];
                while (Sequence != null)
                {
                    Sequence.ReleaseIL();
                    Sequence = Sequence.FNext;
                }
            }
        }
    }

    // ImageLib.pas:9209
    public bool Animating()
    {
        int I;
        ImageSequence Sequence;
        bool Result = false;
        I = 0;
        while (I != Count && !Result)
        {
            Sequence = (ImageSequence)List[I];
            while (Sequence != null && !Result)
            {
                Result = Sequence.FSequenceMode != SequenceMode.None;
                Sequence = Sequence.FNext;
            }

            I += 1;
        }

        return Result;
    }

    // ImageLib.pas:9228
    public LibraryImage GetPreviewImage()
    {
        int I;
        ImageSequence Sequence;
        LibraryImage Result = null;
        I = 0;
        while (I != Count && Result == null)
        {
            Sequence = (ImageSequence)List[I];
            while (Sequence != null && Result == null)
            {
                Result = Sequence.GetPreviewImage();
                Sequence = Sequence.FNext;
            }

            I += 1;
        }

        return Result;
    }

    // ImageLib.pas:9247
    public void GetDimensions(int Index, ref Rect R)
    {
        ImageSequence Sequence;
        Rect SR = default;
        R.Left = 0;
        R.Right = 0;
        R.Top = 0;
        R.Bottom = 0;
        if (Index < Count && Index >= 0)
        {
            Sequence = (ImageSequence)List[Index];
            while (Sequence != null)
            {
                Sequence.GetDimensions(ref SR);
                UnionRect(ref R, R, SR);
                Sequence = Sequence.FNext;
            }
        }
    }

    // ImageLib.pas:9270
    // { Shows the layers of Index for Frame without looping; returns False when no layer has an
    //   image for Frame }
    public bool Show(int Index, int X, int Y, int Frame, in SurfaceDesc SD)
    {
        ImageSequence Sequence;
        int I;
        bool Result = false;
        if (Index < Count)
        {
            Sequence = (ImageSequence)List[Index];
            while (Sequence != null)
            {
                I = Sequence.GetUnloopedFrameIndex(Frame);
                if (I != -1)
                {
                    Result = true;
                    if (Sequence.FImageLibrary.ImageExist(I))
                    {
                        Sequence.FImageLibrary[I].Show(X + Sequence.FdXshift, Y + Sequence.FdYshift, SD);
                    }
                }

                Sequence = Sequence.FNext;
            }
        }

        return Result;
    }

    // ImageLib.pas:9293
    public int GetFramesCount(int Index)
    {
        ImageSequence Sequence;
        int N;
        int Result = 0;
        if (Index < Count)
        {
            Sequence = (ImageSequence)List[Index];
            while (Sequence != null)
            {
                N = Sequence.GetTotalSequenceFrames();
                if (N > Result)
                {
                    Result = N;
                }

                Sequence = Sequence.FNext;
            }
        }

        return Result;
    }

    // ImageLib.pas:9311
    public void ShowLooped(int Index, int X, int Y, int Frame, in SurfaceDesc SD)
    {
        ImageSequence Sequence;
        int I;
        if (Index < Count)
        {
            Sequence = (ImageSequence)List[Index];
            while (Sequence != null)
            {
                I = Sequence.GetFrameIndex(Frame, 0);
                if (I != -1)
                {
                    if (Sequence.FImageLibrary.ImageExist(I))
                    {
                        Sequence.FImageLibrary[I].Show(X + Sequence.FdXshift, Y + Sequence.FdYshift, SD);
                    }
                }

                Sequence = Sequence.FNext;
            }
        }
    }

    // ImageLib.pas:9330
    public void ShowLoopedEx(int Index, int X, int Y, int Frame, int Offset, in SurfaceDesc SD)
    {
        ImageSequence Sequence;
        int I;
        if (Index < Count)
        {
            Sequence = (ImageSequence)List[Index];
            while (Sequence != null)
            {
                I = Sequence.GetFrameIndex(Frame, Offset);
                if (I != -1)
                {
                    if (Sequence.FImageLibrary.ImageExist(I))
                    {
                        Sequence.FImageLibrary[I].Show(X + Sequence.FdXshift, Y + Sequence.FdYshift, SD);
                    }
                }

                Sequence = Sequence.FNext;
            }
        }
    }

    // ImageLib.pas:9350
    public void CopyToClipboard()
    {
    }

    // ImageLib.pas:9354
    public void PasteFromClipboard()
    {
    }

    public sealed class ImageSequencesAccessor(ImageSequenceList target)
    {
        public ImageSequence this[int Index] { get => target.GetImageSequence(Index); set => target.SetImageSequence(Index, value); }
    }

    public ImageSequencesAccessor ImageSequences => new ImageSequencesAccessor(this);
    public bool Linked { get => FLinked; }
    public ImageLibraryManager ILManager { get => FILManager; }
    public CustomImageLibrary ImageLibrary { get => FImageLibrary; }

}

public partial class FrameTableList : Persistent
{
    internal int[] FList;
    internal int FCount;
    internal int FCapacity;
    internal int FDefaultValue;

    // ImageLib.pas:9434
    protected internal int Get(int Index)
    {
        if (Index < 0 || Index >= FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        return FList[Index];
    }

    // ImageLib.pas:9440
    protected internal virtual void Grow()
    {
        int Delta;
        if (FCapacity > 64)
        {
            Delta = FCapacity / 4;
        }
        else
        {
            if (FCapacity > 8)
            {
                Delta = 16;
            }
            else
            {
                Delta = 4;
            }
        }

        SetCapacity(FCapacity + Delta);
    }

    // ImageLib.pas:9486
    // { Unlike TList.Put, an index past the end grows the list }
    protected internal void Put(int Index, int Item)
    {
        if (Index < 0)
        {
            Error("List index out of bounds (%d)", Index);
        }

        if (Index >= FCount)
        {
            SetCount(Index + 1);
        }

        FList[Index] = Item;
    }

    // ImageLib.pas:9506
    protected internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity < FCount || NewCapacity > 134217727)
        {
            Error("List capacity out of bounds (%d)", NewCapacity);
        }

        if (NewCapacity != FCapacity)
        {
            FList = ReallocArray(FList, NewCapacity);
            FCapacity = NewCapacity;
        }
    }

    // ImageLib.pas:9517
    protected internal void SetCount(int NewCount)
    {
        if (NewCount < 0 || NewCount > 134217727)
        {
            Error("List count out of bounds (%d)", NewCount);
        }

        if (NewCount > FCapacity)
        {
            SetCapacity(NewCount);
        }

        if (NewCount > FCount)
        {
            FillBytes(MemoryMarshal.AsBytes(FList.AsSpan(FCount, NewCount - FCount)), FDefaultValue);
        }

        FCount = NewCount;
    }

    // ImageLib.pas:9360
    public override void Dispose()
    {
        Clear();
    }

    // ImageLib.pas:9365
    public override void Assign(Persistent Source)
    {
        if (Source is FrameTableList)
        {
            var frameTableList = (FrameTableList)Source;
            this.SetCapacity(frameTableList.FCount);
            this.FCount = frameTableList.FCount;
            MoveElements(frameTableList.FList, 0, this.FList, 0, this.FCount);
            this.FDefaultValue = frameTableList.FDefaultValue;
        }
        else
        {
            base.Assign(Source);
        }
    }

    // ImageLib.pas:9379
    public int Add(int Item)
    {
        int Result = FCount;
        if (Result == FCapacity)
        {
            Grow();
        }

        FList[Result] = Item;
        FCount += 1;
        return Result;
    }

    // ImageLib.pas:9387
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // ImageLib.pas:9393
    public void Delete(int Index)
    {
        if (Index < 0 || Index >= FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        FCount -= 1;
        if (Index < FCount)
        {
            MoveElements(FList, Index + 1, FList, Index, FCount - Index);
        }
    }

    // ImageLib.pas:9412
    public void Exchange(int Index1, int Index2)
    {
        int Item;
        if (Index1 < 0 || Index1 >= FCount)
        {
            Error("List index out of bounds (%d)", Index1);
        }

        if (Index2 < 0 || Index2 >= FCount)
        {
            Error("List index out of bounds (%d)", Index2);
        }

        Item = FList[Index1];
        FList[Index1] = FList[Index2];
        FList[Index2] = Item;
    }

    // ImageLib.pas:9423
    public FrameTableList Expand()
    {
        if (FCount == FCapacity)
        {
            Grow();
        }

        return this;
    }

    // ImageLib.pas:9429
    public int First() => Get(0);

    // ImageLib.pas:9450
    public int IndexOf(int Item)
    {
        int Result = 0;
        while (Result < FCount && FList[Result] != Item)
        {
            Result += 1;
        }

        if (Result == FCount)
        {
            Result = -1;
        }

        return Result;
    }

    // ImageLib.pas:9457
    public void Insert(int Index, int Item)
    {
        if (Index < 0 || Index > FCount)
        {
            Error("List index out of bounds (%d)", Index);
        }

        if (FCount == FCapacity)
        {
            Grow();
        }

        if (Index < FCount)
        {
            MoveElements(FList, Index, FList, Index + 1, FCount - Index);
        }

        FList[Index] = Item;
        FCount += 1;
    }

    // ImageLib.pas:9467
    public int Last() => Get(FCount - 1);

    // ImageLib.pas:9472
    public void Move(int CurIndex, int NewIndex)
    {
        int Item;
        if (CurIndex != NewIndex)
        {
            if (NewIndex < 0 || NewIndex >= FCount)
            {
                Error("List index out of bounds (%d)", NewIndex);
            }

            Item = Get(CurIndex);
            Delete(CurIndex);
            Insert(NewIndex, Item);
        }
    }

    // ImageLib.pas:9493
    public int Remove(int Item)
    {
        int Result = IndexOf(Item);
        if (Result != -1)
        {
            Delete(Result);
        }

        return Result;
    }

    // ImageLib.pas:9499
    public void Pack()
    {
        for (int I = FCount - 1; I >= 0; I--)
        {
            if (this[I] == 0)
            {
                Delete(I);
            }
        }
    }

    // ImageLib.pas:9554
    public void Sort(FrameTableSortCompare Compare)
    {
        if (FList != null && Count > 0)
        {
            FrameTableQuickSort(FList, 0, Count - 1, Compare);
        }
    }

    // ImageLib.pas:9560
    public bool Compare(FrameTableList List) => List.FCount == FCount && CompareMem(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)), MemoryMarshal.AsBytes(List.FList.AsSpan(0, FCount)), FCount * 4);

    // ImageLib.pas:9566
    public void LoadFromStream(Stream Stream)
    {
        int N = default;
        Clear();
        Stream.ReadBytes(Bytes.Of(ref FDefaultValue, 4));
        Stream.ReadBytes(Bytes.Of(ref N, 4));
        SetCapacity(N);
        Stream.ReadBytes(MemoryMarshal.AsBytes(FList.AsSpan(0, N)));
        FCount = N;
    }

    // ImageLib.pas:9578
    public void SaveToStream(Stream Stream)
    {
        Stream.Write(Bytes.Of(ref FDefaultValue, 4));
        Stream.Write(Bytes.Of(ref FCount, 4));
        Stream.Write(MemoryMarshal.AsBytes(FList.AsSpan(0, FCount)));
    }

    // ImageLib.pas:9586
    // { The items separated by commas }
    public string GetItemString()
    {
        string Result = "";
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            Result = Result + IntToStr(FList[I]) + ",";
        }

        if (!IsEmpty(Result))
        {
            Result = Copy(Result, 1, Result.Length - 1);
        }

        return Result;
    }

    // ImageLib.pas:9597
    // { Reads items separated by commas; other characters than digits are dropped, an empty item
    //   is 0 }
    public void SetItemString(string Value)
    {
        int I;
        int N;
        string S;
        I = Value.Length;
        for (; I >= 1; I -= 1)
        {
            if (!Set<byte>.WithBounds(0, 255).With(44).WithRange(48, 57).Contains(Cp1252.Byte(Value[I - 1])))
            {
                Value = RtlUnit.Delete(Value, I, 1);
            }
        }

        Clear();
        I = Pos(",", Value);
        while (!IsEmpty(Value))
        {
            if (I != 0)
            {
                S = Copy(Value, 1, I - 1);
                Value = RtlUnit.Delete(Value, 1, I);
            }
            else
            {
                S = Value;
                Value = "";
            }

            if (!IsEmpty(S))
            {
                N = StrToInt(S);
            }
            else
            {
                N = 0;
            }

            Add(N);
            I = Pos(",", Value);
        }
    }

    public int Capacity { get => FCapacity; set => SetCapacity(value); }
    public int Count { get => FCount; set => SetCount(value); }

    public int this[int Index] { get => Get(Index); set => Put(Index, value); }

    public int[] List { get => FList; }
    public int DefaultValue { get => FDefaultValue; set => FDefaultValue = value; }
    public string ItemString { get => GetItemString(); set => SetItemString(value); }

    // ImageLib.pas:9401
    public static void Error(string Msg, int Data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(Msg, [Data]));
    }

    private static void FrameTableQuickSort(int[] SortList, int L, int R, FrameTableSortCompare SCompare)
    {
        int num = 0;
        int num2;
        do
        {
            num2 = L;
            int num3 = R;
            int item = SortList[L + R >>> 1];
            while (true)
            {
                if (SCompare(SortList[num2], item) < 0)
                {
                    num2++;
                    continue;
                }
                while (SCompare(SortList[num3], item) > 0)
                {
                    num3--;
                }
                if (num2 <= num3)
                {
                    num = SortList[num2];
                    SortList[num2] = SortList[num3];
                    SortList[num3] = num;
                    num2++;
                    num3--;
                }
                if (num2 > num3)
                {
                    break;
                }
            }
            if (L < num3)
            {
                FrameTableQuickSort(SortList, L, num3, SCompare);
            }
            L = num2;
        }
        while (num2 < R);
    }

    public delegate int FrameTableSortCompare(int Item1, int Item2);

}

[LegacyObject]
// an animation of the images of a library, one layer of a chain sorted by layer. FOwner,
// FLayer, FNext (the next layer), FImageLibrary (linked by LinkToIL: FImageLibraryName from the
// manager of FOwner, or the library of FOwner), FImageLibraryName, FSequenceSpeed (frames per
// image - 1), FSequenceMode, FFrameTable (the image indices of smFrameTable and
// smRndFrameTable), FImageIndex (the image of smNone, the random start of smRandom and
// smRndFrameTable), FdXshift, FdYshift, FLastFrame, FSpeedCount, FFrameCounter
public partial class ImageSequence : IDisposable
{
    internal ImageSequenceList FOwner;
    internal byte FLayer;
    internal ImageSequence FNext;
    internal CustomImageLibrary FImageLibrary;
    internal string FImageLibraryName = "";
    internal byte FSequenceSpeed;
    internal SequenceMode FSequenceMode;
    internal FrameTableList FFrameTable;
    internal int FImageIndex;
    internal int FdXshift;
    internal int FdYshift;
    internal int FLastFrame;
    internal byte FSpeedCount;
    internal int FFrameCounter;

    // ImageLib.pas:8606
    internal void SetImageLibraryName(string Value)
    {
        ReleaseIL();
        FImageLibraryName = Value;
        if (FOwner.FLinked)
        {
            LinkToIL();
        }
    }

    // ImageLib.pas:8613
    internal void SetSequenceMode(SequenceMode Value)
    {
        if (Set<SequenceMode>.Of(SequenceMode.FrameTable, SequenceMode.RndFrameTable).Contains(Value) && FFrameTable == null)
        {
            FFrameTable = new FrameTableList();
        }

        FSequenceMode = Value;
    }

    // ImageLib.pas:8553
    public ImageSequence(ImageSequenceList owner)
    {
        CaptureAddress.Assign(this);
        FOwner = owner;
    }

    // ImageLib.pas:8559
    public virtual void Dispose()
    {
        FFrameTable?.Dispose();
        ReleaseIL();
        if (FNext != null)
        {
            FNext.Dispose();
        }
    }

    // ImageLib.pas:8567
    public ImageSequence Copy(ImageSequenceList AOwner)
    {
        ImageSequence Result = default;
        MemoryStream Stream;
        Stream = new MemoryStream();
        try
        {
            Result = new ImageSequence(AOwner);
            SaveToStream(Stream);
            Stream.Position = 0;
            Result.LoadFromStream(Stream);
        }
        finally
        {
            Stream?.Dispose();
        }

        return Result;
    }

    // ImageLib.pas:8582
    public LibraryImage GetPreviewImage()
    {
        LibraryImage Result;
        int I;
        if (this != null)
        {
            Result = null;
            if (FFrameTable != null && FFrameTable.FCount != 0)
            {
                Result = FImageLibrary.GetSafe(FFrameTable.FList[0]);
            }

            if (FSequenceMode == SequenceMode.None)
            {
                Result = FImageLibrary.GetSafe(FImageIndex);
            }

            if (Result == null)
            {
                I = 0;
                while (I != FImageLibrary.Count && Result == null)
                {
                    Result = FImageLibrary[I];
                    I += 1;
                }
            }
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // ImageLib.pas:8620
    public void GetDimensions(ref Rect R)
    {
        LibraryImage Image;
        Rect IR = default;
        R.Left = 0;
        R.Right = 0;
        R.Top = 0;
        R.Bottom = 0;
        if (FImageLibrary != null)
        {
            switch (FSequenceMode)
            {
                case SequenceMode.FrameTable:
                case SequenceMode.RndFrameTable:
                    {
                        for (int I = 0, last = FFrameTable.Count - 1; I <= last; I++)
                        {
                            var item = FFrameTable[I];
                            Image = FImageLibrary.GetSafe(item);
                            if (Image != null)
                            {
                                Image.GetVisualDimensions(ref IR);
                                UnionRect(ref R, R, IR);
                            }
                        }
                    }

                    break;
                case SequenceMode.Up:
                case SequenceMode.UpDown:
                case SequenceMode.Random:
                    {
                        for (int I = 0, last = FImageLibrary.Count - 1; I <= last; I++)
                        {
                            Image = FImageLibrary.GetSafe(I);
                            if (Image != null)
                            {
                                Image.GetVisualDimensions(ref IR);
                                UnionRect(ref R, R, IR);
                            }
                        }
                    }

                    break;
                case SequenceMode.None:
                    Image = FImageLibrary.GetSafe(FImageIndex);
                    if (Image != null)
                    {
                        Image.GetVisualDimensions(ref R);
                    }

                    break;
            }

            R.Left += FdXshift;
            R.Right += FdXshift;
            R.Top += FdYshift;
            R.Bottom += FdYshift;
        }
    }

    // ImageLib.pas:8666
    public int GetTotalSequenceFrames()
    {
        int Result;
        switch (FSequenceMode)
        {
            case SequenceMode.FrameTable:
                Result = FFrameTable.Count * (FSequenceSpeed + 1);
                break;
            case SequenceMode.Up:
                Result = FImageLibrary.Count * (FSequenceSpeed + 1);
                break;
            case SequenceMode.UpDown:
                Result = FImageLibrary.Count * (FSequenceSpeed + 1) * 2;
                break;
            default:
                Result = FSequenceSpeed + 1;
                break;
        }

        return Result;
    }

    // ImageLib.pas:8677
    public int GetFrameIndex(int Frame, int Offset)
    {
        int Result;
        if (Frame >= 0)
        {
            if (FSequenceMode != SequenceMode.None)
            {
                switch (FSequenceMode)
                {
                    case SequenceMode.FrameTable:
                        if (Frame != FLastFrame)
                        {
                            if (FSpeedCount == 0)
                            {
                                FFrameCounter = (FFrameCounter + 1) & 16777215;
                                FSpeedCount = FSequenceSpeed;
                            }
                            else
                            {
                                FSpeedCount = (byte)(FSpeedCount - 1);
                            }

                            FLastFrame = Frame;
                        }

                        Result = FFrameCounter + Offset;
                        if (FFrameTable.Count != 0)
                        {
                            Result = Result % FFrameTable.Count;
                            Result = FFrameTable.FList[Result];
                        }
                        else
                        {
                            Result = -1;
                        }

                        break;
                    case SequenceMode.Up:
                        if (Frame != FLastFrame)
                        {
                            if (FSpeedCount == 0)
                            {
                                FFrameCounter = (FFrameCounter + 1) & 16777215;
                                FSpeedCount = FSequenceSpeed;
                            }
                            else
                            {
                                FSpeedCount = (byte)(FSpeedCount - 1);
                            }

                            FLastFrame = Frame;
                        }

                        Result = FFrameCounter + Offset;
                        Result = Result % FImageLibrary.Count;
                        break;
                    case SequenceMode.UpDown:
                        if (Frame != FLastFrame)
                        {
                            if (FSpeedCount == 0)
                            {
                                FFrameCounter = (FFrameCounter + 1) & 16777215;
                                FSpeedCount = FSequenceSpeed;
                            }
                            else
                            {
                                FSpeedCount = (byte)(FSpeedCount - 1);
                            }

                            FLastFrame = Frame;
                        }

                        Result = (FFrameCounter + Offset) % ((FImageLibrary.Count - 1) * 2);
                        Result = Math.Abs(Result - (FImageLibrary.Count - 2));
                        break;
                    case SequenceMode.RndFrameTable:
                        if (Frame != FLastFrame && Frame % (FSequenceSpeed + 1) == 0)
                        {
                            FImageIndex = Random(FFrameTable.Count);
                            FLastFrame = Frame;
                        }

                        if (FFrameTable.Count != 0)
                        {
                            Result = FFrameTable.FList[(FImageIndex + Offset) % FFrameTable.Count];
                        }
                        else
                        {
                            Result = -1;
                        }

                        break;
                    case SequenceMode.Random:
                        if (Frame != FLastFrame && Frame % (FSequenceSpeed + 1) == 0)
                        {
                            FImageIndex = Random(FImageLibrary.Count);
                            FLastFrame = Frame;
                        }

                        Result = (FImageIndex + Offset) % FImageLibrary.Count;
                        break;
                    default:
                        Result = -1;
                        break;
                }
            }
            else
            {
                Result = FImageIndex;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // ImageLib.pas:8768
    public int GetUnloopedFrameIndex(int Frame)
    {
        int Result;
        if (Frame >= 0)
        {
            if (FSequenceMode != SequenceMode.None)
            {
                switch (FSequenceMode)
                {
                    case SequenceMode.FrameTable:
                        Result = Frame / (FSequenceSpeed + 1);
                        if (Result < FFrameTable.Count)
                        {
                            Result = FFrameTable.FList[Result];
                        }
                        else
                        {
                            Result = -1;
                        }

                        break;
                    case SequenceMode.Up:
                        Result = Frame / (FSequenceSpeed + 1);
                        if (Result >= FImageLibrary.Count)
                        {
                            Result = -1;
                        }

                        break;
                    case SequenceMode.UpDown:
                        Result = -1;
                        break;
                    case SequenceMode.RndFrameTable:
                        Result = -1;
                        break;
                    case SequenceMode.Random:
                        Result = -1;
                        break;
                    default:
                        Result = -1;
                        break;
                }
            }
            else
            {
                return FImageIndex;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // ImageLib.pas:8803
    public void LinkToIL()
    {
        if (FImageLibrary != null)
        {
            ReleaseIL();
        }

        if (!IsEmpty(FImageLibraryName))
        {
            FImageLibrary = FOwner.FILManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), FImageLibraryName);
        }
        else
        {
            FImageLibrary = FOwner.FImageLibrary;
        }
    }

    // ImageLib.pas:8812
    public void ReleaseIL()
    {
        if (FImageLibrary != null)
        {
            if (!IsEmpty(FImageLibraryName))
            {
                FOwner.FILManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), FImageLibraryName);
            }

            FImageLibrary = null;
        }
    }

    // ImageLib.pas:8822
    public void SaveToStream(Stream Stream)
    {
        bool HasNext;
        LibraryDrawing.WriteStringToStream(FImageLibraryName, Stream);
        Stream.Write(Bytes.Of(ref FLayer, 1));
        Stream.Write(Bytes.Of(ref FSequenceSpeed, 1));
        Stream.Write(Bytes.Of(ref FSequenceMode, 1));
        Stream.Write(Bytes.Of(ref FImageIndex, 4));
        Stream.Write(Bytes.Of(ref FdXshift, 4));
        Stream.Write(Bytes.Of(ref FdYshift, 4));
        if (Set<SequenceMode>.Of(SequenceMode.FrameTable, SequenceMode.RndFrameTable).Contains(FSequenceMode))
        {
            FFrameTable.SaveToStream(Stream);
        }

        HasNext = FNext != null;
        Stream.Write(Bytes.Of(ref HasNext, 1));
        if (HasNext)
        {
            FNext.SaveToStream(Stream);
        }
    }

    // ImageLib.pas:8839
    public void LoadFromStream(Stream Stream)
    {
        bool HasNext = default;
        ReleaseIL();
        LibraryDrawing.ReadStringFromStream(ref FImageLibraryName, Stream);
        Stream.ReadBytes(Bytes.Of(ref FLayer, 1));
        Stream.ReadBytes(Bytes.Of(ref FSequenceSpeed, 1));
        Stream.ReadBytes(Bytes.Of(ref FSequenceMode, 1));
        Stream.ReadBytes(Bytes.Of(ref FImageIndex, 4));
        Stream.ReadBytes(Bytes.Of(ref FdXshift, 4));
        Stream.ReadBytes(Bytes.Of(ref FdYshift, 4));
        if (Set<SequenceMode>.Of(SequenceMode.FrameTable, SequenceMode.RndFrameTable).Contains(FSequenceMode))
        {
            if (FFrameTable == null)
            {
                FFrameTable = new FrameTableList();
            }

            FFrameTable.LoadFromStream(Stream);
            FImageIndex = Random(FFrameTable.Count);
        }

        Stream.ReadBytes(Bytes.Of(ref HasNext, 1));
        if (HasNext)
        {
            FNext = new ImageSequence(FOwner);
            FNext.LoadFromStream(Stream);
        }
    }

    // ImageLib.pas:8865
    public void SaveToIniFile(string Section, IniFile IniFile)
    {
        /* TODO: Implement IniFiles.WriteString in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:8880
    public void LoadFromIniFile(string Section, IniFile IniFile)
    {
        /* TODO: Implement IniFiles.ReadString in the platform or editor service. */
        throw new NotImplementedException();
    }

    public ImageSequenceList Owner { get => FOwner; }
    public byte Layer { get => FLayer; set => FLayer = value; }
    public ImageSequence Next { get => FNext; set => FNext = value; }
    public CustomImageLibrary ImageLibrary { get => FImageLibrary; }
    public string ImageLibraryName { get => FImageLibraryName; set => SetImageLibraryName(value); }
    public byte SequenceSpeed { get => FSequenceSpeed; set => FSequenceSpeed = value; }
    public byte SpeedCount { get => FSpeedCount; }
    public SequenceMode SequenceMode { get => FSequenceMode; set => SetSequenceMode(value); }
    public FrameTableList FrameTable { get => FFrameTable; }
    public int ImageIndex { get => FImageIndex; set => FImageIndex = value; }
    public int dXshift { get => FdXshift; set => FdXshift = value; }
    public int dYshift { get => FdYshift; set => FdYshift = value; }

    internal static void BltImageSequence(ImageSequence Sequence, int DC, int X, int Y, in Rect R, int Frame, int BkColor)
    {
        SurfaceDesc SD = default;
        DIB16Surface Surface;
        int Index;
        Surface = new DIB16Surface(R.Right - R.Left, R.Bottom - R.Top, PixelFormats.pfRGB565);
        Surface.Cls(BkColor);
        Surface.Lock(ref SD);
        while (Sequence != null)
        {
            Index = Sequence.GetUnloopedFrameIndex(Frame);
            if (Index != -1 && Sequence.FImageLibrary != null && Sequence.FImageLibrary.GetSafe(Index) != null)
            {
                Sequence.FImageLibrary[Index].Show(X + Sequence.FdXshift, Y + Sequence.FdYshift, SD);
            }

            Sequence = Sequence.FNext;
        }

        Surface.Unlock();
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        Surface?.Dispose();
    }

}

// A read-only file opened for a sequential load. Dispose releases that file.
[LegacyObject]
public partial class FastLoadFileStream : Stream
{
}

// { The header of an .ILB file (24 bytes). Up to version 3 the images follow it; from version 4
//     on the image index (HeaderSize to IndexEnd) is followed by the discardable image data. }
[StructLayout(LayoutKind.Explicit, Size = 24)]
public struct ILBHeader
{
    [FieldOffset(0)]
    public FixedArray_byte_5 ID__image;

    public string ID { get => ReadShortString(ID__image); set => WriteShortString(ID__image, value); }

    [FieldOffset(8)]
    public float Version;
    [FieldOffset(12)]
    public int HeaderSize;
    [FieldOffset(16)]
    public int IndexEnd;
    [FieldOffset(20)]
    public int Size;
}

// (declared in the implementation): the property editor of TCustomImageLibrary.FileName
public partial class ILFilenameProperty : StringProperty
{
    // ImageLib.pas:10669
    public override void Edit()
    {
        /* TODO: Implement DsgnIntf.GetComponent in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:10720
    public override Set<PropertyAttribute> GetAttributes() => Set<PropertyAttribute>.Of(PropertyAttribute.Dialog, PropertyAttribute.Revertable);

    // ImageLib.pas:10663
    public override string GetValue()
    {
        /* TODO: Implement DsgnIntf.GetValue in the platform or editor service. */
        throw new NotImplementedException();
    }
}
