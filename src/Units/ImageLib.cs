// Translated from ImageLib.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.HashTabU;
using static Aow.Units.HashTabU.HashTabU;

namespace Aow.Units.ImageLib
{
    public enum TImageLibraryStatusElement : byte
    {
        isLoadingFromFile = 0,
        isLoading = 1,
        isModified = 2,
        isSaving = 3,
        isSavingToFile = 4
    }

    public enum TSetTextModeElement : byte
    {
        stmCenter = 0
    }

    public enum TILGridEditModeElement : byte
    {
        igCut = 0,
        igCopy = 1,
        igPaste = 2,
        igDelete = 3,
        igIns = 4,
        igDel = 5
    }

    public enum TILManagerCategoryElement : byte
    {
        icDisk = 0,
        icOwned = 1
    }

    public enum TILManagerOptionsElement : byte
    {
        ioAutoSearchPath = 0
    }

    public enum TBltMode : byte
    {
        bmNone = 0,
        bmStretch = 1,
        bmStretchToFit = 2,
        bmCenter = 3
    }

    // { ImageLib.TLibraryImage (instance size 64): an image of an image library. +4 FOwner, +8
//     FDataLoaded (the discardable data, the pixels, are in memory), +$C FDataPosition (of the
//     discardable data in the library file, -1 for none), +$10 FDataSize, +$14 FReloadingEnabled,
//     +$15 FVersion, +$16 FLoadMode, +$18 FDataWidth, +$1C FDataHeight (of the pixels), +$20
//     FWidth, +$24 FHeight (the visual size), +$28 FXshift, +$2C FYshift (of the pixels in the
//     visual rectangle), +$30 FFileName, +$34 FFileIndex (where the image was grabbed), +$38
//     FShowInfo. }
    public partial class TLibraryImage : Aow.Rtl.TPersistent
    {
        internal Aow.Units.ImageLib.TCustomImageLibrary FOwner;
        internal bool FDataLoaded;
        internal int FDataPosition;
        internal int FDataSize;
        internal bool FReloadingEnabled;
        internal byte FVersion;
        internal Aow.Units.ImageLib.TLoadMode FLoadMode;
        internal int FDataWidth;
        internal int FDataHeight;
        internal int FWidth;
        internal int FHeight;
        internal int FXshift;
        internal int FYshift;
        internal Aow.Rtl.AnsiString FFileName;
        internal int FFileIndex;
        internal Aow.Units.ImageLib.TShowInfo FShowInfo;
        // ImageLib.pas:1613
        public virtual void SetOwner(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            if (((AOwner != FOwner) && (FOwner != null)) && !FDataLoaded)
            {
                SetReloadingEnabled(false);
            }

            FOwner = AOwner;
        }

        // ImageLib.pas:1877
        public virtual void SetLoadMode(Aow.Units.ImageLib.TLoadMode Value)
        {
            FLoadMode = Value;
            if ((FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadAlways) && !FDataLoaded)
            {
                LoadDiscardableImageData();
            }
        }

        // ImageLib.pas:1883
        public virtual Aow.Units.GFXE.TGFXEPalette GetPalette()
        {
            Aow.Units.GFXE.TGFXEPalette Result = default;
            Result = null;
            return Result;
        }

        // ImageLib.pas:1888
        public virtual Aow.Units.GFXE.TGFXEPalette GetRemapPalette()
        {
            Aow.Units.GFXE.TGFXEPalette Result = default;
            Result = null;
            return Result;
        }

        // ImageLib.pas:1893
        public virtual bool GetTransparent()
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // ImageLib.pas:1656
        public virtual void SetReloadingEnabled(bool Value)
        {
            if (!Value && !FDataLoaded)
            {
                LoadDiscardableImageData();
            }

            if (Value && !FReloadingEnabled)
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"Cannot turn on Reloading when disabled");
            }
            else
            {
                FReloadingEnabled = Value;
            }
        }

        // ImageLib.pas:1991
        public virtual void SetDefaultShowInfo(Aow.Units.ImageLib.TShowInfo Value)
        {
            var call__1 = SupportShowMode(Value.ShowMode, 0);
            if (call__1)
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
        public virtual void GetVisualDimensions(ref Aow.Rtl.TRect R)
        {
            R.Left = 0;
            R.Right = FWidth;
            R.Top = 0;
            R.Bottom = FHeight;
        }

        // ImageLib.pas:1702
        public virtual void ReadImageData(Aow.Rtl.TStream Stream)
        {
            Stream.Read(Aow.Rtl.Bytes.Of(ref FVersion, 1));
            Aow.Units.ImageLib.ImageLib.ReadStringFromStream(ref FFileName, Stream);
            Stream.Read(Aow.Rtl.Bytes.Of(ref FDataWidth, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FDataHeight, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FXshift, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FYshift, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FFileIndex, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FLoadMode, 1));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FDataSize, 4));
            if (FVersion > 0)
            {
                if (FVersion >= 2)
                {
                    Stream.Read(Aow.Rtl.Bytes.Of(ref FDataPosition, 4));
                }

                Stream.Read(Aow.Rtl.Bytes.Of(ref FWidth, 4));
                Stream.Read(Aow.Rtl.Bytes.Of(ref FHeight, 4));
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
        public virtual void WriteImageData(Aow.Rtl.TStream Stream)
        {
            byte V = default;
            V = unchecked((byte)ClassType.ClassVersion());
            Stream.Write(Aow.Rtl.Bytes.Of(ref V, 1));
            Aow.Units.ImageLib.ImageLib.WriteStringToStream(FFileName, Stream);
            Stream.Write(Aow.Rtl.Bytes.Of(ref FDataWidth, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FDataHeight, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FXshift, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FYshift, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FFileIndex, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FLoadMode, 1));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FDataSize, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FDataPosition, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FWidth, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FHeight, 4));
            FShowInfo.WriteImage(Stream, false);
        }

        // ImageLib.pas:1747
        public virtual void ReadDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            FDataLoaded = true;
        }

        // ImageLib.pas:1752
        public virtual void WriteDiscardableImageData(Aow.Rtl.TStream Stream)
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
            Aow.Rtl.TStream Stream = default;
            if (!FDataLoaded)
            {
                var call__2 = Aow.Units.ImageLib.TFastLoadFileStream.Create(FOwner.FLoadedFileName);
                Stream = call__2;
                try
                {
                    Stream.Seek(FDataPosition + FOwner.FDataOffset, 0);
                    ReadDiscardableImageData(Stream);
                    FDataLoaded = true;
                }
                finally
                {
                    Stream.Free();
                }
            }
        }

        // ImageLib.pas:1859
        public virtual void SaveToStreamEx(Aow.Rtl.TStream Stream, Aow.Rtl.TStream DataStream)
        {
            int P = default;
            P = FDataPosition;
            var call__3 = DataStream.Position;
            FDataPosition = call__3;
            WriteImageData(Stream);
            FDataPosition = P;
            if (!FDataLoaded)
            {
                LoadDiscardableImageData();
                WriteDiscardableImageData(DataStream);
                if (FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadFreeOnDemand)
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
        public virtual void LoadFromStreamEx(Aow.Rtl.TStream Stream, Aow.Rtl.TStream DataStream)
        {
            ReadImageData(Stream);
            if (FVersion >= 2)
            {
                if (((((FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadAlways) || (FOwner == null)) || !FOwner.FReloading) || !FOwner.FStatus.Contains(Aow.Units.ImageLib.TImageLibraryStatusElement.isLoadingFromFile)) || (FDataPosition == -1))
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
                throw Aow.Rtl.Exception.Create((AnsiString)"Invalid image format");
            }
        }

        // ImageLib.pas:1985
        public virtual int RemapToPixelFormat(int PixelFormatID)
        {
            int Result = default;
            throw Aow.Rtl.Exception.Create((AnsiString)"Surface format not supported");
            Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
            return Result;
        }

        // ImageLib.pas:1685
        public virtual void Clear()
        {
            var call__4 = ClassType.ClassVersion();
            FVersion = unchecked((byte)call__4);
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
        // { TLibraryImage }
        public virtual void Create__init(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            ;
            FDataPosition = -1;
            FReloadingEnabled = false;
            FLoadMode = Aow.Units.ImageLib.TLoadMode.lmLoadOnDemand;
            FShowInfo.ShowMode = Aow.Units.ImageLib.TShowMode.smOpaque;
            FShowInfo.BlendMode = Aow.Units.ImageLib.TBlendMode.bmAlpha;
            SetOwner(AOwner);
        }

        // ImageLib.pas:1979
        public virtual Aow.Rtl.AnsiString Info()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__5 = ClassType.Name();
            Result = (((call__5 + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(FWidth)) + AnsiString.FromBytes(120)) + Aow.Rtl.Rtl.IntToStr(FHeight);
            if (GetTransparent())
            {
                Result = Result + ((AnsiString)" Transparent");
            }

            return Result;
        }

        // ImageLib.pas:1670
        public virtual Aow.Units.ImageLib.TLibraryImage Copy()
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            Aow.Rtl.TMemoryStream Stream = default;
            Stream = new Aow.Rtl.TMemoryStream();
            try
            {
                Aow.Units.ImageLib.ImageLib.WriteImageToStream(this, Stream);
                Stream.Position = 0;
                var call__6 = Aow.Units.ImageLib.ImageLib.ReadImageFromStream(Stream, FOwner);
                Result = call__6;
                Result.SetOwner(null);
            }
            finally
            {
                Stream.Free();
            }

            return Result;
        }

        // ImageLib.pas:1778
        public virtual void SaveToStream(Aow.Rtl.TStream Stream)
        {
            int P = default;
            P = FDataPosition;
            FDataPosition = -1;
            WriteImageData(Stream);
            FDataPosition = P;
            if (!FDataLoaded)
            {
                LoadDiscardableImageData();
                WriteDiscardableImageData(Stream);
                if (FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadFreeOnDemand)
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
        public virtual void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            int P = default;
            Clear();
            ReadImageData(Stream);
            if (FVersion < 2)
            {
                var call__7 = Stream.Position;
                FDataPosition = call__7;
                if ((((FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadAlways) || (FOwner == null)) || !FOwner.FReloading) || !FOwner.FStatus.Contains(Aow.Units.ImageLib.TImageLibraryStatusElement.isLoadingFromFile))
                {
                    ReadDiscardableImageData(Stream);
                    FReloadingEnabled = false;
                }
                else
                {
                    Stream.Seek(FDataSize, 1);
                    FReloadingEnabled = true;
                }
            }
            else
            {
                if (((((FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadAlways) || (FOwner == null)) || !FOwner.FReloading) || !FOwner.FStatus.Contains(Aow.Units.ImageLib.TImageLibraryStatusElement.isLoadingFromFile)) || (FDataPosition == -1))
                {
                    if (FDataPosition != -1)
                    {
                        var call__8 = Stream.Position;
                        P = call__8;
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
        public virtual void Show(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            // { Tried addressed/absolute Y, named X and post-call identity; entry X save still differs. }
            switch (FShowInfo.ShowMode)
            {
                case Aow.Units.ImageLib.TShowMode.smOpaque:
                    ShowOpaque(X, Y, SD);
                    break;
                case Aow.Units.ImageLib.TShowMode.smTransparent:
                    ShowTransparent(X, Y, SD);
                    break;
                case Aow.Units.ImageLib.TShowMode.smBlended:
                    switch (FShowInfo.BlendMode)
                    {
                        case Aow.Units.ImageLib.TBlendMode.bmUser:
                            if (FShowInfo.BlendTable != null)
                            {
                                ShowBlended(X, Y, SD, FShowInfo.BlendTable);
                            }

                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmAlpha:
                            ShowBlended(X, Y, SD, Aow.Units.GFXE.GFXE.AlphaBlendTable.Lookup[FShowInfo.BlendPercentage]);
                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmBrighten:
                            ShowBlended(X, Y, SD, Aow.Units.GFXE.GFXE.BrightenBlendTable.Lookup[FShowInfo.BlendPercentage]);
                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmIntensity:
                            ShowBlended(X, Y, SD, Aow.Units.GFXE.GFXE.IntensityBlendTable.Lookup[FShowInfo.BlendPercentage]);
                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmShadow:
                            ShowBlended(X, Y, SD, Aow.Units.GFXE.GFXE.ShadowBlendTable.Lookup[FShowInfo.BlendPercentage]);
                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmLinearAlpha:
                            ShowBlended(X, Y, SD, Aow.Units.GFXE.GFXE.LinearAlphaBlendTable.Lookup[FShowInfo.BlendPercentage]);
                            break;
                    }

                    break;
            }
        }

        // ImageLib.pas:2040
        public virtual void ShowStretched(in Aow.Rtl.TRect R, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            // { Tried named descriptor pointer, pointer identity and receiver scope; descriptor save still differs. }
            switch (FShowInfo.ShowMode)
            {
                case Aow.Units.ImageLib.TShowMode.smOpaque:
                    ShowOpaqueStretched(R, SD);
                    break;
                case Aow.Units.ImageLib.TShowMode.smBlended:
                    switch (FShowInfo.BlendMode)
                    {
                        case Aow.Units.ImageLib.TBlendMode.bmUser:
                            if (FShowInfo.BlendTable != null)
                            {
                                ShowBlendedStretched(R, SD, FShowInfo.BlendTable);
                            }

                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmAlpha:
                            ShowBlendedStretched(R, SD, Aow.Units.GFXE.GFXE.AlphaBlendTable.Lookup[FShowInfo.BlendPercentage]);
                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmBrighten:
                            ShowBlendedStretched(R, SD, Aow.Units.GFXE.GFXE.BrightenBlendTable.Lookup[FShowInfo.BlendPercentage]);
                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmIntensity:
                            ShowBlendedStretched(R, SD, Aow.Units.GFXE.GFXE.IntensityBlendTable.Lookup[FShowInfo.BlendPercentage]);
                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmShadow:
                            ShowBlendedStretched(R, SD, Aow.Units.GFXE.GFXE.ShadowBlendTable.Lookup[FShowInfo.BlendPercentage]);
                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmLinearAlpha:
                            ShowBlendedStretched(R, SD, Aow.Units.GFXE.GFXE.LinearAlphaBlendTable.Lookup[FShowInfo.BlendPercentage]);
                            break;
                    }

                    break;
            }
        }

        // ImageLib.pas:2066
        public virtual void ShowOpaque(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
        }

        // ImageLib.pas:2070
        public virtual void ShowOpaqueStretched(in Aow.Rtl.TRect R, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
        }

        // ImageLib.pas:2078
        public virtual void ShowBlended(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.GFXE.TBlendTable Table)
        {
        }

        // ImageLib.pas:2082
        public virtual void ShowBlendedStretched(in Aow.Rtl.TRect R, in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.GFXE.TBlendTable Table)
        {
        }

        // ImageLib.pas:2074
        public virtual void ShowTransparent(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
        }

        // ImageLib.pas:2020
        // { ShowEx: the blend mode comes from the image's own show info (an original bug: ShowInfo only
//   gives the show mode and the blend value), and bmLinearAlpha is not handled. }
        public virtual void ShowEx(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD, in Aow.Units.ImageLib.TShowInfo ShowInfo)
        {
            // { Tried addressed/absolute coordinates, Self identity and named receiver; entry Self save still differs. }
            switch (ShowInfo.ShowMode)
            {
                case Aow.Units.ImageLib.TShowMode.smOpaque:
                    ShowOpaque(X, Y, SD);
                    break;
                case Aow.Units.ImageLib.TShowMode.smTransparent:
                    ShowTransparent(X, Y, SD);
                    break;
                case Aow.Units.ImageLib.TShowMode.smBlended:
                    switch (FShowInfo.BlendMode)
                    {
                        case Aow.Units.ImageLib.TBlendMode.bmUser:
                            if (ShowInfo.BlendTable != null)
                            {
                                ShowBlended(X, Y, SD, ShowInfo.BlendTable);
                            }

                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmAlpha:
                            ShowBlended(X, Y, SD, Aow.Units.GFXE.GFXE.AlphaBlendTable.Lookup[ShowInfo.BlendPercentage]);
                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmBrighten:
                            ShowBlended(X, Y, SD, Aow.Units.GFXE.GFXE.BrightenBlendTable.Lookup[ShowInfo.BlendPercentage]);
                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmIntensity:
                            ShowBlended(X, Y, SD, Aow.Units.GFXE.GFXE.IntensityBlendTable.Lookup[ShowInfo.BlendPercentage]);
                            break;
                        case Aow.Units.ImageLib.TBlendMode.bmShadow:
                            ShowBlended(X, Y, SD, Aow.Units.GFXE.GFXE.ShadowBlendTable.Lookup[ShowInfo.BlendPercentage]);
                            break;
                    }

                    break;
            }
        }

        // ImageLib.pas:2087
        public virtual void ShowDC(Ptr<Aow.Rtl.TRect> SrcRect, Ptr<Aow.Rtl.TRect> DestRect, int DC, int BkColor)
        {
        }

        // ImageLib.pas:2091
        public virtual bool SupportShowMode(Aow.Units.ImageLib.TShowMode ShowMode, int PixelFormatID)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // ImageLib.pas:1903
        public virtual bool SupportGrabPixelFormat(int PixelFormatID)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // ImageLib.pas:1908
        public virtual Aow.Rtl.TForm CreateGrabForm(Aow.Rtl.TComponent AOwner, in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.ImageLib.TImagesGrabbedEvent OnImagesGrabbed)
        {
            Aow.Rtl.TForm Result = default;
            Result = null;
            return Result;
        }

        // ImageLib.pas:1914
        public virtual bool GrabImage(in Aow.Units.GFXE.TSurfaceDesc SD, in Aow.Rtl.TRect R, Aow.Units.ImageLib.TGrabInfo GrabInfo)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // ImageLib.pas:1920
        public virtual Aow.Units.ImageLib.TGrabInfo CreateGrabInfo()
        {
            Aow.Units.ImageLib.TGrabInfo Result = default;
            Result = Aow.Units.ImageLib.TGrabInfo.Create();
            return Result;
        }

        // ImageLib.pas:1607
        public override void Destroy()
        {
            Clear();
            base.Destroy();
        }

        // ImageLib.pas:1648
        public void GetDimensions(ref Aow.Rtl.TRect R)
        {
            R.Left = 0;
            R.Right = FWidth;
            R.Top = 0;
            R.Bottom = FHeight;
        }

        // ImageLib.pas:2061
        public void ShowClipped(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Show(X, Y, SD);
        }

        public Aow.Units.ImageLib.TCustomImageLibrary Owner { get => FOwner; set => SetOwner(value); }
        public bool DataLoaded { get => FDataLoaded; }
        public int DataPosition { get => FDataPosition; }
        public int DataSize { get => FDataSize; }
        public bool ReloadingEnabled { get => FReloadingEnabled; set => SetReloadingEnabled(value); }
        public byte Version { get => FVersion; }
        public Aow.Units.ImageLib.TLoadMode LoadMode { get => FLoadMode; set => SetLoadMode(value); }
        public int DataWidth { get => FDataWidth; }
        public int DataHeight { get => FDataHeight; }
        public int Width { get => FWidth; set => SetWidth(value); }
        public int Height { get => FHeight; set => SetHeight(value); }
        public int Xshift { get => FXshift; set => SetXshift(value); }
        public int Yshift { get => FYshift; set => SetYshift(value); }
        public Aow.Rtl.AnsiString FileName { get => FFileName; set => FFileName = value; }
        public int FileIndex { get => FFileIndex; set => FFileIndex = value; }
        public Aow.Units.ImageLib.TShowInfo ShowInfo { get => FShowInfo; set => SetDefaultShowInfo(value); }
        public Aow.Units.GFXE.TGFXEPalette Palette { get => GetPalette(); }
        public Aow.Units.GFXE.TGFXEPalette RemapPalette { get => GetRemapPalette(); }
        public bool Transparent { get => GetTransparent(); }

        // ImageLib.pas:271
        public static Aow.Units.ImageLib.TLibraryImage Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            Aow.Units.ImageLib.TLibraryImage o = new Aow.Units.ImageLib.TLibraryImage();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // ImageLib.pas:1898
            public virtual int ClassID()
            {
                int Result = default;
                Result = 0;
                return Result;
            }

            // ImageLib.pas:1974
            public virtual Aow.Rtl.AnsiString Name()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Abstract Library Image";
                return Result;
            }

            // ImageLib.pas:1665
            public virtual byte ClassVersion()
            {
                byte Result = default;
                Result = 3;
                return Result;
            }

            // ImageLib.pas:1927
            // { GrabImages: grabs the images of a surface into ImageLibrary, one per rectangle that
//   GetSurfaceRectangles finds (or the whole surface). }
            public virtual int GrabImages(in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.ImageLib.TGrabInfo GrabInfo, Aow.Units.ImageLib.TCustomImageLibrary ImageLibrary, Aow.Units.ImageLib.TGrabProgressEvent OnProgress)
            {
                int Result = default;
                Aow.Units.ImageLib.TLibraryImage.Meta ImageClass = default;
                Aow.Units.ImageLib.TLibraryImage Image = default;
                Aow.Rtl.AnsiString Msg = default;
                Aow.Units.GFXE.TRectangleList Rects = default;
                int I = default;
                void Progress(int Percent, Aow.Rtl.AnsiString Msg)
                {
                    if (OnProgress != null)
                    {
                        OnProgress(null, Percent, Msg);
                    }
                }

                Progress(10, (AnsiString)"Analyzing Image..");
                var call__9 = ClassID();
                var call__10 = Aow.Units.ImageLib.ImageLib.FindLibraryImageClass(call__9);
                ImageClass = call__10;
                if (GrabInfo.FindRectangles)
                {
                    var call__11 = Aow.Units.GFXE.GFXE.GetSurfaceRectangles(SD, GrabInfo.TransparentColor, null);
                    Rects = call__11;
                }
                else
                {
                    Rects = Aow.Units.GFXE.TRectangleList.Create();
                    var call__12 = Aow.Units.GFXE.TRectangle.Create(0, 0, SD.Width, SD.Height);
                    Rects.Add(call__12);
                }

                if ((Rects != null) && (ImageClass != null))
                {
                    {
                        I = 0;
                        int end__13 = Rects.Count - 1;
                        for (; I <= end__13; I += 1)
                        {
                            Msg = (AnsiString)"Grabbing Image";
                            if (Rects.Count > 1)
                            {
                                Progress(Aow.Rtl.Rtl.Round(((double)(I * 90)) / (Rects.Count - 1)) + 10, ((AnsiString)"Grabbing Image ") + Aow.Rtl.Rtl.IntToStr(I + 1));
                            }
                            else
                            {
                                Progress(100, (AnsiString)"Grabbing Image");
                            }

                            Image = ImageClass.Create(null);
                            GrabInfo.FileIndex = I;
                            var call__14 = Rects.Rectangles[I];
                            var call__15 = Image.GrabImage(SD, call__14.Rect, GrabInfo);
                            if (call__15)
                            {
                                ImageLibrary.Add(Image);
                            }
                            else
                            {
                                Image.Free();
                            }
                        }
                    }

                    Result = 0;
                }
                else
                {
                    Result = Aow.Units.GFXE.GFXE.GFXE_ERROR;
                }

                Rects.Free();
                return Result;
            }

            // ImageLib.pas:271
            public Aow.Units.ImageLib.TLibraryImage Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
            {
                Aow.Units.ImageLib.TLibraryImage o = (Aow.Units.ImageLib.TLibraryImage)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ImageLib.TLibraryImage => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    // { ImageLib.TLibraryImagePalette (instance size 20): +4 FIndex, +8 FPalette, +$C FRemapPalette
//     (the palette remapped to a pixel format, made on demand), +$10 FRefCount. }
    public partial class TLibraryImagePalette : Aow.Rtl.TPersistent
    {
        internal int FIndex;
        internal Aow.Units.GFXE.TGFXEPalette FPalette;
        internal Aow.Units.GFXE.TGFXEPalette FRemapPalette;
        internal int FRefCount;
        // ImageLib.pas:7084
        // { TLibraryImagePalette }
        public void Create__init_TLibraryImagePalette()
        {
            ;
            var call__16 = Aow.Units.GFXE.TGFXEPalette.Create();
            FPalette = call__16;
        }

        // ImageLib.pas:7090
        public override void Destroy()
        {
            FPalette.Free();
            FRemapPalette.Free();
            base.Destroy();
        }

        // ImageLib.pas:7097
        public void SaveToStream(Aow.Rtl.TStream Stream)
        {
            FPalette.SaveToStream(Stream);
        }

        // ImageLib.pas:7102
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            FPalette.LoadFromStream(Stream);
        }

        // ImageLib.pas:7107
        public Aow.Units.GFXE.TGFXEPalette RemapPalette(int PixelFormatID)
        {
            Aow.Units.GFXE.TGFXEPalette Result = default;
            if (PixelFormatID == FPalette.PixelFormatID)
            {
                Result = FPalette;
            }
            else
            {
                Aow.Units.GFXE.GFXE.Color2PixelInfo(0, PixelFormatID, FPalette);
                if ((FRemapPalette == null) || (PixelFormatID != FRemapPalette.PixelFormatID))
                {
                    if (FRemapPalette == null)
                    {
                        var call__17 = Aow.Units.GFXE.TGFXEPalette.Create();
                        FRemapPalette = call__17;
                    }

                    FRemapPalette.Assign(FPalette);
                    FRemapPalette.Remap(PixelFormatID);
                }

                Result = FRemapPalette;
            }

            return Result;
        }

        // ImageLib.pas:7124
        public override void Assign(Aow.Rtl.TPersistent Source)
        {
            if (Source is Aow.Units.ImageLib.TLibraryImagePalette)
            {
                FPalette.Assign(((Aow.Units.ImageLib.TLibraryImagePalette)Source).FPalette);
                if (((Aow.Units.ImageLib.TLibraryImagePalette)Source).FRemapPalette != null)
                {
                    if (FRemapPalette == null)
                    {
                        var call__18 = Aow.Units.GFXE.TGFXEPalette.Create();
                        FRemapPalette = call__18;
                    }

                    FRemapPalette.Assign(((Aow.Units.ImageLib.TLibraryImagePalette)Source).FRemapPalette);
                }
                else
                {
                    FRemapPalette.Free();
                }
            }
            else
            {
                throw Aow.Rtl.EConvertError.Create((AnsiString)"Can only assign to a TLibraryImagePalette descendant");
            }
        }

        public int Index { get => FIndex; }
        public Aow.Units.GFXE.TGFXEPalette Palette { get => FPalette; }
        public int RefCount { get => FRefCount; }

        // ImageLib.pas:57
        public static Aow.Units.ImageLib.TLibraryImagePalette Create()
        {
            Aow.Units.ImageLib.TLibraryImagePalette o = new Aow.Units.ImageLib.TLibraryImagePalette();
            try
            {
                o.Create__init_TLibraryImagePalette();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public partial class TPaletteManager : Aow.Rtl.TObject
    {
        internal Aow.Rtl.TList FPalettes;
        // ImageLib.pas:6984
        internal int GetCount()
        {
            int Result = default;
            Result = FPalettes.Count;
            return Result;
        }

        // ImageLib.pas:6989
        internal Aow.Units.ImageLib.TLibraryImagePalette GetPalette(int Index)
        {
            Aow.Units.ImageLib.TLibraryImagePalette Result = default;
            var call__19 = FPalettes[Index];
            Result = (Aow.Units.ImageLib.TLibraryImagePalette)call__19;
            return Result;
        }

        // ImageLib.pas:6994
        internal void UpdateIndices()
        {
            int I = default;
            {
                I = 0;
                int end__20 = FPalettes.Count - 1;
                for (; I <= end__20; I += 1)
                {
                    var call__21 = FPalettes[I];
                    ((Aow.Units.ImageLib.TLibraryImagePalette)call__21).FIndex = I;
                }
            }
        }

        // ImageLib.pas:6971
        // { TPaletteManager }
        public void Create__init_TPaletteManager()
        {
            ;
            var call__22 = new Aow.Rtl.TList();
            FPalettes = call__22;
        }

        // ImageLib.pas:6977
        public override void Destroy()
        {
            Clear();
            FPalettes.Free();
            base.Destroy();
        }

        // ImageLib.pas:7001
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            int I = default;
            int N = default;
            Aow.Units.ImageLib.TLibraryImagePalette P = default;
            Clear();
            Stream.Read(Aow.Rtl.Bytes.Of(ref N, 4));
            {
                I = 0;
                int end__23 = N - 1;
                for (; I <= end__23; I += 1)
                {
                    P = Aow.Units.ImageLib.TLibraryImagePalette.Create();
                    P.LoadFromStream(Stream);
                    FPalettes.Add(P);
                    P.FIndex = I;
                }
            }
        }

        // ImageLib.pas:7017
        public void SaveToStream(Aow.Rtl.TStream Stream)
        {
            int I = default;
            Stream.Write(Aow.Rtl.Rtl.ValueBytes<int>(FPalettes.Count, 4));
            {
                I = 0;
                int end__24 = FPalettes.Count - 1;
                for (; I <= end__24; I += 1)
                {
                    ((Aow.Units.ImageLib.TLibraryImagePalette)FPalettes.List[I]).SaveToStream(Stream);
                }
            }
        }

        // ImageLib.pas:7025
        public void Clear()
        {
            int I = default;
            {
                I = FPalettes.Count - 1;
                byte end__25 = 0;
                for (; I >= end__25; I -= 1)
                {
                    ((Aow.Units.ImageLib.TLibraryImagePalette)FPalettes.List[I]).Free();
                }
            }

            FPalettes.Clear();
        }

        // ImageLib.pas:7033
        public Aow.Units.ImageLib.TLibraryImagePalette LinkToPalette(int Index)
        {
            Aow.Units.ImageLib.TLibraryImagePalette Result = default;
            var call__26 = FPalettes[Index];
            Result = (Aow.Units.ImageLib.TLibraryImagePalette)call__26;
            Result.FRefCount += 1;
            return Result;
        }

        // ImageLib.pas:7039
        public Aow.Units.ImageLib.TLibraryImagePalette RegisterPalette(Aow.Units.GFXE.TGFXEPalette Palette)
        {
            Aow.Units.ImageLib.TLibraryImagePalette Result = default;
            int I = default;
            bool Found = default;
            Aow.Units.ImageLib.TLibraryImagePalette P = default;
            I = FPalettes.Count;
            Found = false;
            while ((I != 0) && !Found)
            {
                I -= 1;
                var call__27 = Palette.Compare(((Aow.Units.ImageLib.TLibraryImagePalette)FPalettes.List[I]).FPalette);
                Found = call__27;
            }

            if (!Found)
            {
                P = Aow.Units.ImageLib.TLibraryImagePalette.Create();
                P.FPalette.Assign(Palette);
                P.FIndex = FPalettes.Count;
                P.FRefCount = 1;
                FPalettes.Add(P);
                Result = P;
            }
            else
            {
                var call__28 = FPalettes[I];
                ((Aow.Units.ImageLib.TLibraryImagePalette)call__28).FRefCount += 1;
                var call__29 = FPalettes[I];
                Result = (Aow.Units.ImageLib.TLibraryImagePalette)call__29;
            }

            return Result;
        }

        // ImageLib.pas:7068
        public void UnregisterPalette(int Index)
        {
            Aow.Units.ImageLib.TLibraryImagePalette P = default;
            var call__30 = FPalettes[Index];
            P = (Aow.Units.ImageLib.TLibraryImagePalette)call__30;
            P.FRefCount -= 1;
            if (P.FRefCount <= 0)
            {
                FPalettes.Delete(Index);
                UpdateIndices();
                P.Free();
            }
        }

        public int Count { get => GetCount(); }

        public sealed class PalettesAccessor(Aow.Units.ImageLib.TPaletteManager target)
        {
            public Aow.Units.ImageLib.TLibraryImagePalette this[int Index] { get => target.GetPalette(Index); }
        }

        public PalettesAccessor Palettes => new PalettesAccessor(this);

        // ImageLib.pas:36
        public static Aow.Units.ImageLib.TPaletteManager Create()
        {
            Aow.Units.ImageLib.TPaletteManager o = new Aow.Units.ImageLib.TPaletteManager();
            try
            {
                o.Create__init_TPaletteManager();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public struct TImageItem
    {
        public Aow.Units.ImageLib.TLibraryImage Image;
        public int Reference;
    }

    // { ImageLib.TCustomImageLibrary (instance size 80): +$24 FStatus, +$28 FOnChange, +$30
//     FChangeLock (OnChange is called when it is 0), +$31 FReloading (the images can reload their
//     data from FLoadedFileName), +$32 FLoadOnDemand, +$34 FLoadedFileName, +$38 FFileName (relative
//     to the project or to BaseImageLibraryPath), +$3C FDataOffset (where the discardable image
//     data starts in the file), +$40 FCapacity, +$44 FCount, +$48 FImages, +$4C FPaletteManager. }
    public partial class TCustomImageLibrary : Aow.Rtl.TComponent
    {
        internal TSet<Aow.Units.ImageLib.TImageLibraryStatusElement> FStatus;
        internal Aow.Rtl.TNotifyEvent FOnChange;
        internal byte FChangeLock;
        internal bool FReloading;
        internal bool FLoadOnDemand;
        internal Aow.Rtl.AnsiString FLoadedFileName;
        internal Aow.Rtl.AnsiString FFileName;
        internal int FDataOffset;
        internal int FCapacity;
        internal int FCount;
        internal Ptr<Aow.Units.ImageLib.TImageItem> FImages;
        internal Aow.Units.ImageLib.TPaletteManager FPaletteManager;
        // ImageLib.pas:9699
        internal void SetCapacity(int NewCapacity)
        {
            int I = default;
            if (NewCapacity != 0)
            {
                NewCapacity = NewCapacity | 7;
            }

            if (NewCapacity != FCapacity)
            {
                FImages = FImages.Realloc(NewCapacity);
                if (NewCapacity > FCapacity)
                {
                    Ptr<Aow.Units.ImageLib.TImageItem>.Fill(FImages + FCapacity, (NewCapacity - FCapacity), default);
                    {
                        I = FCapacity;
                        int end__31 = NewCapacity - 1;
                        for (; I <= end__31; I += 1)
                        {
                            FImages[I].Reference = -1;
                        }
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
        internal Aow.Rtl.AnsiString GetFullFileName()
        {
            Aow.Rtl.AnsiString Result = default;
            Aow.Rtl.AnsiString Drive = default;
            Aow.Rtl.AnsiString Path = default;
            if (FFileName != ((AnsiString)""))
            {
                if (ComponentState.Contains(Aow.Rtl.TComponentState.csDesigning))
                {
                    var call__32 = Aow.Rtl.Rtl.ToolServices.GetProjectName();
                    var call__33 = Aow.Rtl.Rtl.ExtractFilePath(call__32);
                    Path = call__33;
                }
                else
                {
                    Path = Aow.Units.ImageLib.ImageLib.BaseImageLibraryPath;
                }

                var call__34 = Aow.Rtl.Rtl.ExtractFileDrive(FFileName);
                Drive = call__34;
                if ((Drive == ((AnsiString)"")) || (Drive == Aow.Rtl.Rtl.ExtractFileDrive(Path)))
                {
                    var call__35 = Aow.Rtl.Rtl.ExpandFileName(Path + FFileName);
                    Result = call__35;
                }
                else
                {
                    Result = FFileName;
                }
            }
            else
            {
                Result = (AnsiString)"";
            }

            return Result;
        }

        // ImageLib.pas:9743
        internal void SetFileName(Aow.Rtl.AnsiString Value)
        {
            Aow.Rtl.AnsiString Path = default;
            if (ComponentState.Contains(Aow.Rtl.TComponentState.csDesigning))
            {
                var call__36 = Aow.Rtl.Rtl.ToolServices.GetProjectName();
                var call__37 = Aow.Rtl.Rtl.ExtractFilePath(call__36);
                Path = call__37;
            }
            else
            {
                Path = Aow.Units.ImageLib.ImageLib.BaseImageLibraryPath;
            }

            Value = Aow.Rtl.Rtl.ExtractRelativePath(Path, Value);
            if ((ComponentState.Contains(Aow.Rtl.TComponentState.csDesigning) || ComponentState.Contains(Aow.Rtl.TComponentState.csLoading)) && (Value != FFileName))
            {
                FFileName = Value;
                if (FFileName != ((AnsiString)""))
                {
                    try
                    {
                        var call__39 = GetFullFileName();
                        LoadFromFile(call__39);
                    }
                    catch (System.Exception)
                    {
                        var call__38 = GetFullFileName();
                        Aow.Rtl.Rtl.ShowMessage(((AnsiString)"Error loading: ") + call__38);
                        FFileName = (AnsiString)"";
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
            // { Tried Self identity, receiver scope and shifted index; Self/array registers still differ. }
            Ptr<Aow.Units.ImageLib.TImageItem>.Copy(FImages + (Index + 1), FImages + Index, ((FCount - Index) - 1));
            Ptr<Aow.Units.ImageLib.TImageItem>.Fill(FImages + (FCount - 1), 1, default);
            FCount -= 1;
            SetModified(true);
        }

        // ImageLib.pas:9783
        internal void InsertItem(int Index)
        {
            SetCount(FCount + 1);
            Ptr<Aow.Units.ImageLib.TImageItem>.Copy(FImages + Index, FImages + (Index + 1), ((FCount - Index) - 1));
            Ptr<Aow.Units.ImageLib.TImageItem>.Fill(FImages + Index, 1, default);
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
        public override void DefineProperties(Aow.Rtl.TFiler Filer)
        {
            /* TODO(T-shim) Platform or editor boundary Classes.DefineBinaryProperty */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:9669
        // { TCustomImageLibrary }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            var call__40 = Aow.Units.ImageLib.TPaletteManager.Create();
            FPaletteManager = call__40;
            FLoadOnDemand = true;
            FFileName = (AnsiString)"";
            FStatus = TSet<Aow.Units.ImageLib.TImageLibraryStatusElement>.Of();
            Clear();
        }

        // ImageLib.pas:9679
        public override void Destroy()
        {
            Clear();
            FPaletteManager.Free();
            base.Destroy();
        }

        // ImageLib.pas:9686
        public virtual bool GetModified()
        {
            bool Result = default;
            Result = FStatus.Contains(Aow.Units.ImageLib.TImageLibraryStatusElement.isModified);
            return Result;
        }

        // ImageLib.pas:9691
        public virtual void SetModified(bool Value)
        {
            if (Value)
            {
                FStatus = FStatus + TSet<Aow.Units.ImageLib.TImageLibraryStatusElement>.Of(Aow.Units.ImageLib.TImageLibraryStatusElement.isModified);
            }
            else
            {
                FStatus = FStatus - TSet<Aow.Units.ImageLib.TImageLibraryStatusElement>.Of(Aow.Units.ImageLib.TImageLibraryStatusElement.isModified);
            }
        }

        // ImageLib.pas:9826
        public virtual void Clear()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__41 = 0;
                for (; I >= end__41; I -= 1)
                {
                    if (FImages[I].Image != null)
                    {
                        FImages[I].Image.Free();
                    }
                }
            }

            if (!FStatus.Contains(Aow.Units.ImageLib.TImageLibraryStatusElement.isLoadingFromFile))
            {
                FFileName = (AnsiString)"";
                FLoadedFileName = (AnsiString)"";
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
        public virtual void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            int Index = default;
            int Reference = default;
            int StartPos = default;
            Aow.Rtl.TMemoryStream MS = default;
            Aow.Units.ImageLib.TILBHeader Header = default;
            int SubIndex = default;
            Aow.Units.ImageLib.TLibraryImage Image = default;
            FStatus = FStatus + TSet<Aow.Units.ImageLib.TImageLibraryStatusElement>.Of(Aow.Units.ImageLib.TImageLibraryStatusElement.isLoading);
            FChangeLock = (byte)(((int)FChangeLock) + 1);
            Clear();
            FReloading = FStatus.Contains(Aow.Units.ImageLib.TImageLibraryStatusElement.isLoadingFromFile) && FLoadOnDemand;
            var call__42 = Stream.Position;
            StartPos = call__42;
            Stream.Read(Aow.Rtl.Bytes.Of(ref Header, 24));
            Stream.Seek(Header.HeaderSize + StartPos, 0);
            if (((double)Header.Version) < 4)
            {
                FDataOffset = 0;
                if (((double)Header.Version) > 1.5)
                {
                    FPaletteManager.LoadFromStream(Stream);
                }
                else
                {
                    FPaletteManager.Clear();
                }

                if (((double)Header.Version) <= 2)
                {
                    Stream.Read(Aow.Rtl.Bytes.Of(ref Index, 4));
                    while (Index != -1)
                    {
                        SubIndex = 0;
                        Image = Aow.Units.ImageLib.ImageLib.ReadImageFromStream(Stream, this);
                        while (Image != null)
                        {
                            SetSubImage(Index, SubIndex, Image);
                            SubIndex += 1;
                            Image = Aow.Units.ImageLib.ImageLib.ReadImageFromStream(Stream, this);
                        }

                        Stream.Read(Aow.Rtl.Bytes.Of(ref Reference, 4));
                        SetImageReference(Index, Reference);
                        Stream.Read(Aow.Rtl.Bytes.Of(ref Index, 4));
                    }
                }
                else
                {
                    if (((double)Header.Version) == 3)
                    {
                        Stream.Read(Aow.Rtl.Bytes.Of(ref Index, 4));
                        while (Index != -1)
                        {
                            var call__43 = Aow.Units.ImageLib.ImageLib.ReadImageFromStream(Stream, this);
                            SetImage(Index, call__43);
                            Stream.Read(Aow.Rtl.Bytes.Of(ref Reference, 4));
                            SetImageReference(Index, Reference);
                            Stream.Read(Aow.Rtl.Bytes.Of(ref Index, 4));
                        }
                    }
                }
            }
            else
            {
                FDataOffset = Header.IndexEnd + StartPos;
                MS = new Aow.Rtl.TMemoryStream();
                try
                {
                    MS.SetSize(Header.IndexEnd - Header.HeaderSize);
                    var call__44 = MS.Size;
                    Stream.ReadBuffer(MS.Memory.AsSpan(0, call__44));
                    FPaletteManager.LoadFromStream(MS);
                    MS.Read(Aow.Rtl.Bytes.Of(ref Index, 4));
                    while (Index != -1)
                    {
                        var call__45 = Aow.Units.ImageLib.ImageLib.ReadImageFromStreamEx(MS, Stream, this);
                        SetImage(Index, call__45);
                        MS.Read(Aow.Rtl.Bytes.Of(ref Reference, 4));
                        SetImageReference(Index, Reference);
                        MS.Read(Aow.Rtl.Bytes.Of(ref Index, 4));
                    }
                }
                finally
                {
                    MS.Free();
                }

                if (!FStatus.Contains(Aow.Units.ImageLib.TImageLibraryStatusElement.isLoadingFromFile))
                {
                    Stream.Seek(Header.Size + StartPos, 0);
                }
            }

            FChangeLock = (byte)(((int)FChangeLock) - 1);
            if (FChangeLock == 0)
            {
                CallOnChangeEvent();
            }

            FStatus = FStatus - TSet<Aow.Units.ImageLib.TImageLibraryStatusElement>.Of(Aow.Units.ImageLib.TImageLibraryStatusElement.isLoading);
        }

        // ImageLib.pas:10038
        public virtual void SaveToStream(Aow.Rtl.TStream Stream)
        {
            Aow.Rtl.TFileStream Swap = default;
            int StartPos = default;
            Aow.Rtl.AnsiString SwapName = default;
            int I = default;
            Aow.Units.ImageLib.TILBHeader Header = default;
            var call__46 = Stream.Position;
            StartPos = call__46;
            var call__47 = Aow.Rtl.Rtl.Application.ExeName;
            var call__48 = Aow.Rtl.Rtl.ExtractFilePath(call__47);
            SwapName = call__48 + ((AnsiString)"\\SWAP.$$$");
            Swap = new Aow.Rtl.TFileStream(SwapName, 65535);
            try
            {
                FStatus = FStatus + TSet<Aow.Units.ImageLib.TImageLibraryStatusElement>.Of(Aow.Units.ImageLib.TImageLibraryStatusElement.isSaving);
                Header.HeaderSize = 24;
                Header.ID = AnsiString.FromBytes(73, 76, 66, 0);
                Header.Version = 4;
                Stream.Write(Aow.Rtl.Bytes.Of(ref Header, 24));
                FPaletteManager.SaveToStream(Stream);
                {
                    I = 0;
                    int end__49 = FCount - 1;
                    for (; I <= end__49; I += 1)
                    {
                        if ((FImages[I].Image != null) || (FImages[I].Reference != -1))
                        {
                            Stream.Write(Aow.Rtl.Bytes.Of(ref I, 4));
                            Aow.Units.ImageLib.ImageLib.WriteImageToStreamEx(FImages[I].Image, Stream, Swap);
                            Stream.Write(Aow.Rtl.Bytes.Of(ref FImages[I].Reference, 4));
                        }
                    }
                }

                I = -1;
                Stream.Write(Aow.Rtl.Bytes.Of(ref I, 4));
                var call__50 = Stream.Position;
                Header.IndexEnd = call__50 - StartPos;
                Swap.Position = 0;
                var call__51 = Swap.Size;
                Stream.CopyFrom(Swap, call__51);
                var call__52 = Stream.Position;
                Header.Size = call__52 - StartPos;
                Stream.Position = StartPos;
                Stream.Write(Aow.Rtl.Bytes.Of(ref Header, 24));
                Stream.Position = StartPos + Header.Size;
            }
            finally
            {
                FStatus = FStatus - TSet<Aow.Units.ImageLib.TImageLibraryStatusElement>.Of(Aow.Units.ImageLib.TImageLibraryStatusElement.isSaving);
                Swap.Free();
                Aow.Rtl.Rtl.DeleteFile(SwapName);
            }
        }

        // ImageLib.pas:10079
        public virtual void LoadFromFile(Aow.Rtl.AnsiString AFileName)
        {
            Aow.Rtl.TStream Stream = default;
            Stream = Aow.Units.ImageLib.TFastLoadFileStream.Create(AFileName);
            try
            {
                FChangeLock = (byte)(((int)FChangeLock) + 1);
                FStatus = FStatus + TSet<Aow.Units.ImageLib.TImageLibraryStatusElement>.Of(Aow.Units.ImageLib.TImageLibraryStatusElement.isLoadingFromFile);
                var call__53 = Aow.Rtl.Rtl.ExpandFileName(AFileName);
                FileName = call__53;
                var call__54 = GetFullFileName();
                FLoadedFileName = call__54;
                LoadFromStream(Stream);
            }
            finally
            {
                FStatus = FStatus - TSet<Aow.Units.ImageLib.TImageLibraryStatusElement>.Of(Aow.Units.ImageLib.TImageLibraryStatusElement.isLoadingFromFile);
                Stream.Free();
                FChangeLock = (byte)(((int)FChangeLock) - 1);
                if (FChangeLock == 0)
                {
                    CallOnChangeEvent();
                }
            }
        }

        // ImageLib.pas:10098
        public virtual void SaveToFile(Aow.Rtl.AnsiString AFileName)
        {
            Aow.Rtl.TFileStream Stream = default;
            Aow.Rtl.AnsiString Name = default;
            Name = Aow.Rtl.Rtl.ExpandFileName(AFileName);
            if (Name == FLoadedFileName)
            {
                DisableReloading();
            }

            FStatus = FStatus + TSet<Aow.Units.ImageLib.TImageLibraryStatusElement>.Of(Aow.Units.ImageLib.TImageLibraryStatusElement.isSavingToFile);
            Stream = new Aow.Rtl.TFileStream(Name, 65535);
            try
            {
                SaveToStream(Stream);
            }
            finally
            {
                FStatus = FStatus - TSet<Aow.Units.ImageLib.TImageLibraryStatusElement>.Of(Aow.Units.ImageLib.TImageLibraryStatusElement.isSavingToFile);
                Stream.Free();
            }
        }

        // ImageLib.pas:9798
        public virtual void CallOnChangeEvent()
        {
            if ((((FChangeLock == 0) && (FOnChange != null)) && !ComponentState.Contains(Aow.Rtl.TComponentState.csDestroying)) && !ComponentState.Contains(Aow.Rtl.TComponentState.csReading))
            {
                FChangeLock = (byte)(((int)FChangeLock) + 1);
                FOnChange(this);
                FChangeLock = (byte)(((int)FChangeLock) - 1);
            }
        }

        // ImageLib.pas:10152
        public virtual Aow.Units.ImageLib.TCustomImageLibrary Copy()
        {
            Aow.Units.ImageLib.TCustomImageLibrary Result = default;
            Aow.Rtl.TMemoryStream Stream = default;
            Stream = new Aow.Rtl.TMemoryStream();
            try
            {
                Result = (Aow.Units.ImageLib.TCustomImageLibrary)new Aow.Rtl.TComponent(null);
                SaveToStream(Stream);
                Stream.Position = 0;
                Result.LoadFromStream(Stream);
                Result.FReloading = false;
            }
            finally
            {
                Stream.Destroy();
            }

            return Result;
        }

        // ImageLib.pas:9769
        public int ReduceCount()
        {
            int Result = default;
            while (true)
            {
                bool bool__55 = (FCount > 0);
                if (bool__55)
                {
                    var call__56 = Get(FCount - 1);
                    bool__55 = (call__56 == null);
                }

                if (!bool__55)
                    break;
                {
                    FCount -= 1;
                }
            }

            Result = FCount;
            return Result;
        }

        // ImageLib.pas:9790
        public void UpdateImageItemSettings(int Index)
        {
            Aow.Units.ImageLib.TLibraryImage Image = default;
            Image = FImages[Index].Image;
            if ((Image != null) && (Image.ClassType.ClassID() == 256))
            {
                ((Aow.Units.ImageLib.TImageNode)Image).UpdateDimensions();
            }
        }

        // ImageLib.pas:9809
        public void DisableOnChangeEvent()
        {
            FChangeLock = (byte)(((int)FChangeLock) + 1);
        }

        // ImageLib.pas:9814
        public void EnableOnChangeEvent()
        {
            FChangeLock = (byte)(((int)FChangeLock) - 1);
            if (FChangeLock == 0)
            {
                CallOnChangeEvent();
            }
        }

        // ImageLib.pas:9843
        public void DisableReloading()
        {
            int I = default;
            {
                I = 0;
                int end__57 = FCount - 1;
                for (; I <= end__57; I += 1)
                {
                    if (FImages[I].Image != null)
                    {
                        FImages[I].Image.SetReloadingEnabled(false);
                    }
                }
            }

            FReloading = false;
        }

        // ImageLib.pas:9852
        public void ReleaseDiscardableImageData()
        {
            int I = default;
            Aow.Units.ImageLib.TLibraryImage Image = default;
            if (FReloading)
            {
                {
                    I = 0;
                    int end__58 = FCount - 1;
                    for (; I <= end__58; I += 1)
                    {
                        Image = FImages[I].Image;
                        if (Image != null)
                        {
                            Image.ReleaseDiscardableImageData();
                        }
                    }
                }
            }
        }

        // ImageLib.pas:9865
        public void LoadDiscardableImageData()
        {
            int I = default;
            Aow.Units.ImageLib.TLibraryImage Image = default;
            {
                I = 0;
                int end__59 = FCount - 1;
                for (; I <= end__59; I += 1)
                {
                    Image = FImages[I].Image;
                    if (Image != null)
                    {
                        Image.LoadDiscardableImageData();
                    }
                }
            }
        }

        // ImageLib.pas:9884
        public bool IsReference(int Index)
        {
            bool Result = default;
            Result = (((Index >= 0) && (Index < FCount)) && (FImages[Index].Image == null)) && (FImages[Index].Reference != -1);
            return Result;
        }

        // ImageLib.pas:9922
        public bool ImageExist(int Index)
        {
            bool Result = default;
            int R = default;
            if ((Index < FCount) && (Index >= 0))
            {
                Result = FImages[Index].Image != null;
                if (!Result)
                {
                    R = FImages[Index].Reference;
                    if ((R >= 0) && (R < FCount))
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
        public int GetImageReference(int Index)
        {
            int Result = default;
            Result = FImages[Index].Reference;
            return Result;
        }

        // ImageLib.pas:9947
        public void SetImageReference(int Index, int Value)
        {
            // { Tried old-image local and class casts; cached image/index registers still differ. }
            if (Index >= FCount)
            {
                SetCount(Index + 1);
            }

            if (Value != -1)
            {
                if (FImages[Index].Image != null)
                {
                    FImages[Index].Image.Destroy();
                }

                FImages[Index].Image = null;
            }

            FImages[Index].Reference = Value;
            UpdateImageItemSettings(Index);
            SetModified(true);
        }

        // ImageLib.pas:10115
        public void LoadFromFileStream(Aow.Rtl.AnsiString AFileName, Aow.Rtl.TStream Stream)
        {
            FStatus = FStatus + TSet<Aow.Units.ImageLib.TImageLibraryStatusElement>.Of(Aow.Units.ImageLib.TImageLibraryStatusElement.isLoadingFromFile);
            try
            {
                var call__60 = Aow.Rtl.Rtl.ExpandFileName(AFileName);
                FileName = call__60;
                var call__61 = GetFullFileName();
                FLoadedFileName = call__61;
                LoadFromStream(Stream);
            }
            finally
            {
                FStatus = FStatus - TSet<Aow.Units.ImageLib.TImageLibraryStatusElement>.Of(Aow.Units.ImageLib.TImageLibraryStatusElement.isLoadingFromFile);
            }
        }

        // ImageLib.pas:10127
        public void CopyToClipboard()
        {
            /* TODO(T-shim) Platform or editor boundary Clipbrd.SetComponent */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:10132
        public void PasteFromClipboard()
        {
            /* TODO(T-shim) Platform or editor boundary Clipbrd.GetComponent */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:10168
        public override void Assign(Aow.Rtl.TPersistent Source)
        {
            Aow.Rtl.TMemoryStream Stream = default;
            if (Source is Aow.Units.ImageLib.TCustomImageLibrary)
            {
                Stream = new Aow.Rtl.TMemoryStream();
                try
                {
                    ((Aow.Units.ImageLib.TCustomImageLibrary)Source).SaveToStream(Stream);
                    Stream.Position = 0;
                    LoadFromStream(Stream);
                    FFileName = ((Aow.Units.ImageLib.TCustomImageLibrary)Source).FFileName;
                    FLoadedFileName = ((Aow.Units.ImageLib.TCustomImageLibrary)Source).FLoadedFileName;
                    if (FChangeLock == 0)
                    {
                        CallOnChangeEvent();
                    }
                }
                finally
                {
                    Stream.Free();
                }
            }
            else
            {
                throw Aow.Rtl.EConvertError.Create((AnsiString)"Can only assign to a TCustomImageLibrary descendant");
            }
        }

        // ImageLib.pas:10190
        public void Delete(int Index)
        {
            if (Index < FCount)
            {
                if (FImages[Index].Image != null)
                {
                    FImages[Index].Image.Free();
                }

                DeleteItem(Index);
                if (FChangeLock == 0)
                {
                    CallOnChangeEvent();
                }
            }
        }

        // ImageLib.pas:10200
        public int Remove(Aow.Units.ImageLib.TLibraryImage Image)
        {
            int Result = default;
            Result = IndexOf(Image);
            if (Result != -1)
            {
                if (FImages[Result].Image != null)
                {
                    FImages[Result].Image.Free();
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
        public int IndexOf(Aow.Units.ImageLib.TLibraryImage Image)
        {
            int Result = default;
            Result = 0;
            while ((Result < FCount) && (Image != FImages[Result].Image))
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
        public Aow.Units.ImageLib.TLibraryImage GetImage(int Index)
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
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
        public void SetImage(int Index, Aow.Units.ImageLib.TLibraryImage Image)
        {
            // { Tried old-image local and class casts; cached image/index registers still differ. }
            if (Index >= FCount)
            {
                SetCount(Index + 1);
            }

            if (FImages[Index].Image != null)
            {
                FImages[Index].Image.Destroy();
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
        public void Insert(int Index, Aow.Units.ImageLib.TLibraryImage Image)
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
        public void Add(Aow.Units.ImageLib.TLibraryImage Image)
        {
            SetImage(FCount, Image);
        }

        // ImageLib.pas:10256
        public bool Empty()
        {
            bool Result = default;
            int I = default;
            I = 0;
            while ((I != FCount) && (FImages[I].Image == null))
            {
                I += 1;
            }

            Result = I == FCount;
            return Result;
        }

        // ImageLib.pas:10265
        public void Pack()
        {
            int I = default;
            DisableOnChangeEvent();
            {
                I = FCount - 1;
                byte end__62 = 0;
                for (; I >= end__62; I -= 1)
                {
                    if (Get(I) == null)
                    {
                        Delete(I);
                    }
                }
            }

            EnableOnChangeEvent();
        }

        // ImageLib.pas:10275
        public int GetSubCount(int Index)
        {
            int Result = default;
            Aow.Units.ImageLib.TLibraryImage Image = default;
            Image = GetSafe(Index);
            if (Image != null)
            {
                if (Image.ClassType.ClassID() != 256)
                {
                    Result = 1;
                }
                else
                {
                    Result = ((Aow.Units.ImageLib.TImageNode)Image).FCount;
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // ImageLib.pas:10291
        public Aow.Units.ImageLib.TLibraryImage GetSub(int Index, int SubIndex)
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            Aow.Units.ImageLib.TLibraryImage Image = default;
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
                if (Image.ClassType.ClassID() == 256)
                {
                    Result = ((Aow.Units.ImageLib.TImageNode)Image).FImages[SubIndex];
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
        public Aow.Units.ImageLib.TLibraryImage GetSafeSub(int Index, int SubIndex)
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            Aow.Units.ImageLib.TLibraryImage Image = default;
            Image = GetSafe(Index);
            if (Image != null)
            {
                if (Image.ClassType.ClassID() == 256)
                {
                    Result = ((Aow.Units.ImageLib.TImageNode)Image).GetSafeImage(SubIndex);
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
        public Aow.Units.ImageLib.TLibraryImage GetSubImage(int Index, int SubIndex)
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            Aow.Units.ImageLib.TImageNode Node = default;
            Result = FImages[Index].Image;
            if (Result != null)
            {
                if (Result.ClassType.ClassID() == 256)
                {
                    Node = (Aow.Units.ImageLib.TImageNode)Result;
                    if ((SubIndex >= 0) && (SubIndex < Node.FCount))
                    {
                        Result = Node.FImages[SubIndex];
                        Result.SetOwner(null);
                        Node.Delete(SubIndex);
                        if (Node.FCount == 1)
                        {
                            var call__63 = Node.GetImage(0);
                            FImages[Index].Image = call__63;
                            Node.Delete(0);
                            Node.Free();
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
        public void SetSubImage(int Index, int SubIndex, Aow.Units.ImageLib.TLibraryImage Image)
        {
            Aow.Units.ImageLib.TLibraryImage Old = default;
            Aow.Units.ImageLib.TImageNode Node = default;
            // { Tried local order, receiver scope and index identity; old-image/index registers still differ. }
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
                if (Old.ClassType.ClassID() == 256)
                {
                    Node = (Aow.Units.ImageLib.TImageNode)Old;
                    if ((SubIndex >= 0) && (SubIndex < Node.FCount))
                    {
                        Old = Node.FImages[SubIndex];
                        if (Old != null)
                        {
                            Old.Destroy();
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
                        Node = Aow.Units.ImageLib.TImageNode.Create(this);
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
            Aow.Units.ImageLib.TImageNode Node = default;
            Node = (Aow.Units.ImageLib.TImageNode)FImages[Index].Image;
            if (Node != null)
            {
                if (Node.ClassType.ClassID() != 256)
                {
                    var call__64 = Node.GetImage(SubIndex);
                    call__64.Destroy();
                    Node.Delete(SubIndex);
                    if (Node.FCount == 1)
                    {
                        FImages[Index].Image = Node.FImages[0];
                        Node.Delete(0);
                        Node.Destroy();
                    }

                    SetModified(true);
                }
                else
                {
                    if (SubIndex == 0)
                    {
                        Node.Free();
                        FImages[Index].Image = null;
                    }
                }
            }
        }

        // ImageLib.pas:10439
        public int RemoveSub(int Index, Aow.Units.ImageLib.TLibraryImage Image)
        {
            int Result = default;
            Aow.Units.ImageLib.TLibraryImage Old = default;
            Aow.Units.ImageLib.TImageNode Node = default;
            Old = FImages[Index].Image;
            if (Old != null)
            {
                if (Old.ClassType.ClassID() == 256)
                {
                    Node = (Aow.Units.ImageLib.TImageNode)Old;
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
                        Old.Free();
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
        public int IndexOfSub(int Index, Aow.Units.ImageLib.TLibraryImage Image)
        {
            int Result = default;
            Aow.Units.ImageLib.TLibraryImage Old = default;
            Old = FImages[Index].Image;
            if (Old != null)
            {
                if (Old.ClassType.ClassID() == 256)
                {
                    Result = ((Aow.Units.ImageLib.TImageNode)Old).IndexOf(Image);
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
        public void InsertSub(int Index, int SubIndex, Aow.Units.ImageLib.TLibraryImage Image)
        {
            Aow.Units.ImageLib.TLibraryImage Old = default;
            Aow.Units.ImageLib.TImageNode Node = default;
            Old = FImages[Index].Image;
            if (Old != null)
            {
                if (Old.ClassType.ClassID() == 256)
                {
                    ((Aow.Units.ImageLib.TImageNode)Old).Insert(SubIndex, Image);
                    Image.SetOwner(this);
                    SetModified(true);
                }
                else
                {
                    Node = Aow.Units.ImageLib.TImageNode.Create(this);
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
        public void AddSub(int Index, Aow.Units.ImageLib.TLibraryImage Image)
        {
            Aow.Units.ImageLib.TLibraryImage Old = default;
            Aow.Units.ImageLib.TImageNode Node = default;
            Old = FImages[Index].Image;
            if (Old != null)
            {
                if (Old.ClassType.ClassID() == 256)
                {
                    ((Aow.Units.ImageLib.TImageNode)Old).Add(Image);
                    Image.SetOwner(this);
                    SetModified(true);
                }
                else
                {
                    Node = Aow.Units.ImageLib.TImageNode.Create(this);
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
            bool Result = default;
            Aow.Units.ImageLib.TLibraryImage Image = default;
            Image = FImages[Index].Image;
            if ((Image != null) && (SubIndex >= 0))
            {
                if (Image.ClassType.ClassID() == 256)
                {
                    Result = SubIndex < ((Aow.Units.ImageLib.TImageNode)Image).FCount;
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

        public Aow.Units.ImageLib.TLibraryImage this[int Index] { get => Get(Index); }

        public bool Modified { get => GetModified(); set => SetModified(value); }
        public TSet<Aow.Units.ImageLib.TImageLibraryStatusElement> Status { get => FStatus; }
        public Aow.Units.ImageLib.TPaletteManager PaletteManager { get => FPaletteManager; }
        public Aow.Rtl.AnsiString FullFileName { get => GetFullFileName(); }
        public Aow.Rtl.AnsiString LoadedFileName { get => FLoadedFileName; }
        public int DataOffset { get => FDataOffset; }
        public bool Reloading { get => FReloading; }
        public bool LoadOnDemand { get => FLoadOnDemand; set => SetLoadOnDemand(value); }
        public Aow.Rtl.TNotifyEvent OnChange { get => FOnChange; set => FOnChange = value; }
        public Aow.Rtl.AnsiString FileName { get => FFileName; set => SetFileName(value); }

        // ImageLib.pas:111
        public static Aow.Units.ImageLib.TCustomImageLibrary Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.ImageLib.TCustomImageLibrary o = new Aow.Units.ImageLib.TCustomImageLibrary();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { ImageLib.TImageLibrary (instance size 80): the image library component }
    public partial class TImageLibrary : Aow.Units.ImageLib.TCustomImageLibrary
    {
        // ImageLib.pas:111
        public static new Aow.Units.ImageLib.TImageLibrary Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.ImageLib.TImageLibrary o = new Aow.Units.ImageLib.TImageLibrary();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public enum TLoadMode : byte
    {
        lmLoadAlways = 0,
        lmLoadOnDemand = 1,
        lmLoadFreeOnDemand = 2
    }

    public partial class TGrabInfo : Aow.Rtl.TObject
    {
        public int TransparentColor;
        public bool FindRectangles;
        public int TransparentPixel;
        public Aow.Rtl.AnsiString FileName;
        public int FileIndex;
        // ImageLib.pas:9634
        public virtual void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            Stream.Read(Aow.Rtl.Bytes.Of(ref TransparentColor, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FindRectangles, 1));
            Stream.Read(Aow.Rtl.Bytes.Of(ref TransparentPixel, 4));
            Aow.Units.ImageLib.ImageLib.ReadStringFromStream(ref FileName, Stream);
            Stream.Read(Aow.Rtl.Bytes.Of(ref FileIndex, 4));
        }

        // ImageLib.pas:9643
        public virtual void SaveToStream(Aow.Rtl.TStream Stream)
        {
            Stream.Write(Aow.Rtl.Bytes.Of(ref TransparentColor, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FindRectangles, 1));
            Stream.Write(Aow.Rtl.Bytes.Of(ref TransparentPixel, 4));
            Aow.Units.ImageLib.ImageLib.WriteStringToStream(FileName, Stream);
            Stream.Write(Aow.Rtl.Bytes.Of(ref FileIndex, 4));
        }

        // ImageLib.pas:9629
        // { TGrabInfo }
        public virtual Aow.Units.ImageLib.TGrabInfo.Meta GrabInfoClass()
        {
            Aow.Units.ImageLib.TGrabInfo.Meta Result = default;
            Result = Aow.Units.ImageLib.TGrabInfo.Meta.Of<Aow.Units.ImageLib.TGrabInfo>();
            return Result;
        }

        // ImageLib.pas:9652
        public virtual Aow.Units.ImageLib.TGrabInfo Copy()
        {
            Aow.Units.ImageLib.TGrabInfo Result = default;
            Aow.Rtl.TMemoryStream Stream = default;
            Stream = new Aow.Rtl.TMemoryStream();
            try
            {
                SaveToStream(Stream);
                Stream.Position = 0;
                var call__65 = GrabInfoClass();
                var call__66 = new Aow.Units.ImageLib.TGrabInfo();
                Result = call__66;
                Result.LoadFromStream(Stream);
            }
            finally
            {
                Stream.Free();
            }

            return Result;
        }

        // System.pas:111
        public static Aow.Units.ImageLib.TGrabInfo Create()
        {
            return new Aow.Units.ImageLib.TGrabInfo();
        }

        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            public static new Meta Of<T>()
                where T : Aow.Units.ImageLib.TGrabInfo => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public delegate void TImagesGrabbedEvent(Aow.Units.ImageLib.TCustomImageLibrary GrabbedImages, Aow.Units.ImageLib.TGrabInfo UsedGrabInfo);
    public delegate void TGrabProgressEvent(Aow.Rtl.TObject Sender, int Percent, Aow.Rtl.AnsiString Msg);
    public enum TShowMode : byte
    {
        smOpaque = 0,
        smTransparent = 1,
        smBlended = 2
    }

    public enum TBlendMode : byte
    {
        bmUser = 0,
        bmAlpha = 1,
        bmBrighten = 2,
        bmIntensity = 3,
        bmShadow = 4,
        bmLinearAlpha = 5
    }

    // { How an image is shown (8 bytes): the blend value is a percentage into the blend table
//     collection of the blend mode, or the blend table itself for bmUser. }
    public struct TShowInfo
    {
        public Aow.Units.ImageLib.TShowMode ShowMode;
        public Aow.Units.ImageLib.TBlendMode BlendMode;
        public IntOrRef Payload;
        public int BlendPercentage { get => Payload.Value; set => Payload = value; }
        public Aow.Units.GFXE.TBlendTable BlendTable { get => Payload.As<Aow.Units.GFXE.TBlendTable>(); set => Payload = value; }

        private ushort imagePadding;
        private byte[] Image()
        {
            byte[] image = new byte[8];
            image[0] = (byte)ShowMode;
            image[1] = (byte)BlendMode;
            System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(image.AsSpan(2), imagePadding);
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(image.AsSpan(4), Payload.Value);
            return image;
        }

        public int ReadImage(TStream stream, bool exact)
        {
            byte[] image = Image();
            int count = stream.Read(image);
            ShowMode = (Aow.Units.ImageLib.TShowMode)image[0];
            BlendMode = (Aow.Units.ImageLib.TBlendMode)image[1];
            imagePadding = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(image.AsSpan(2));
            Payload = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(image.AsSpan(4));
            if (exact && (count != 8))
                throw EReadError.Create("Stream read error");
            return count;
        }

        public int WriteImage(TStream stream, bool exact)
        {
            byte[] image = Image();
            int count = stream.Write(image);
            if (exact && (count != 8))
                throw EWriteError.Create("Stream write error");
            return count;
        }
    }

    public partial class TImageNode : Aow.Units.ImageLib.TLibraryImage
    {
        internal Ptr<Aow.Units.ImageLib.TLibraryImage> FImages;
        internal int FCapacity;
        internal int FCount;
        // ImageLib.pas:1313
        internal void SetCapacity(int NewCapacity)
        {
            if (NewCapacity != FCapacity)
            {
                FImages = FImages.Realloc(NewCapacity);
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
                Ptr<Aow.Units.ImageLib.TLibraryImage>.Fill(FImages + FCount, (NewCount - FCount), default);
            }

            FCount = NewCount;
        }

        // ImageLib.pas:1286
        // { TImageNode }
        public override void Create__init(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            base.Create__init(AOwner);
            FFileName = (AnsiString)"(multi images)";
        }

        // ImageLib.pas:1292
        public override void Destroy()
        {
            Clear();
            base.Destroy();
        }

        // ImageLib.pas:1308
        public override Aow.Rtl.AnsiString Info()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Image Node";
            return Result;
        }

        // ImageLib.pas:1330
        public Aow.Units.ImageLib.TLibraryImage GetImage(int Index)
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            Result = FImages[Index];
            return Result;
        }

        // ImageLib.pas:1335
        public Aow.Units.ImageLib.TLibraryImage GetSafeImage(int Index)
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            if ((Index >= 0) && (Index < FCount))
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
            int I = default;
            FWidth = 0;
            FHeight = 0;
            {
                I = 0;
                int end__67 = FCount - 1;
                for (; I <= end__67; I += 1)
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
        }

        // ImageLib.pas:1356
        public int Add(Aow.Units.ImageLib.TLibraryImage Image)
        {
            int Result = default;
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

            Result = FCount;
            return Result;
        }

        // ImageLib.pas:1365
        public void Insert(int Index, Aow.Units.ImageLib.TLibraryImage Image)
        {
            if (FCapacity == FCount)
            {
                SetCapacity(FCapacity + 1);
            }

            if (Index < FCount)
            {
                Ptr<Aow.Units.ImageLib.TLibraryImage>.Copy(FImages + Index, FImages + (Index + 1), (FCount - Index));
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
        public void Remove(Aow.Units.ImageLib.TLibraryImage Image)
        {
            int I = default;
            I = FCount;
            while ((I >= 0) && (Image != FImages[I]))
            {
                I -= 1;
            }

            if (I >= 0)
            {
                FCount -= 1;
                if (I < FCount)
                {
                    Ptr<Aow.Units.ImageLib.TLibraryImage>.Copy(FImages + (I + 1), FImages + I, (FCount - I));
                }

                UpdateDimensions();
            }
        }

        // ImageLib.pas:1391
        public int IndexOf(Aow.Units.ImageLib.TLibraryImage Image)
        {
            int Result = default;
            Result = FCount - 1;
            while ((Result >= 0) && (Image != FImages[Result]))
            {
                Result -= 1;
            }

            return Result;
        }

        // ImageLib.pas:1397
        public void Delete(int Index)
        {
            if ((Index >= 0) && (Index < FCount))
            {
                FCount -= 1;
                if (Index < FCount)
                {
                    Ptr<Aow.Units.ImageLib.TLibraryImage>.Copy(FImages + (Index + 1), FImages + Index, (FCount - Index));
                }

                UpdateDimensions();
            }
        }

        // ImageLib.pas:1408
        public override void SaveToStreamEx(Aow.Rtl.TStream Stream, Aow.Rtl.TStream DataStream)
        {
            int I = default;
            {
                I = 0;
                int end__68 = FCount - 1;
                for (; I <= end__68; I += 1)
                {
                    Aow.Units.ImageLib.ImageLib.WriteImageToStreamEx(FImages[I], Stream, DataStream);
                }
            }

            Aow.Units.ImageLib.ImageLib.WriteImageToStream(null, Stream);
        }

        // ImageLib.pas:1416
        public override void SaveToStream(Aow.Rtl.TStream Stream)
        {
            int I = default;
            {
                I = 0;
                int end__69 = FCount - 1;
                for (; I <= end__69; I += 1)
                {
                    Aow.Units.ImageLib.ImageLib.WriteImageToStream(FImages[I], Stream);
                }
            }

            Aow.Units.ImageLib.ImageLib.WriteImageToStream(null, Stream);
        }

        // ImageLib.pas:1424
        public override void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            Aow.Units.ImageLib.TLibraryImage Image = default;
            Clear();
            var call__70 = Aow.Units.ImageLib.ImageLib.ReadImageFromStream(Stream, FOwner);
            Image = call__70;
            while (Image != null)
            {
                Add(Image);
                var call__71 = Aow.Units.ImageLib.ImageLib.ReadImageFromStream(Stream, FOwner);
                Image = call__71;
            }
        }

        // ImageLib.pas:1437
        public override void LoadFromStreamEx(Aow.Rtl.TStream Stream, Aow.Rtl.TStream DataStream)
        {
            Aow.Units.ImageLib.TLibraryImage Image = default;
            var call__72 = Aow.Units.ImageLib.ImageLib.ReadImageFromStreamEx(Stream, DataStream, FOwner);
            Image = call__72;
            while (Image != null)
            {
                Add(Image);
                var call__73 = Aow.Units.ImageLib.ImageLib.ReadImageFromStreamEx(Stream, DataStream, FOwner);
                Image = call__73;
            }
        }

        // ImageLib.pas:1449
        public override void Clear()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__74 = 0;
                for (; I >= end__74; I -= 1)
                {
                    FImages[I].Destroy();
                }
            }

            FCount = 0;
            SetCapacity(0);
        }

        // ImageLib.pas:1458
        public override void SetReloadingEnabled(bool Value)
        {
            int I = default;
            {
                I = 0;
                int end__75 = FCount - 1;
                for (; I <= end__75; I += 1)
                {
                    FImages[I].SetReloadingEnabled(false);
                }
            }
        }

        // ImageLib.pas:1465
        public override void SetOwner(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            int I = default;
            {
                I = 0;
                int end__76 = FCount - 1;
                for (; I <= end__76; I += 1)
                {
                    FImages[I].SetOwner(AOwner);
                }
            }

            FOwner = AOwner;
        }

        // ImageLib.pas:1473
        public override int RemapToPixelFormat(int PixelFormatID)
        {
            int Result = default;
            int I = default;
            Result = 0;
            I = FCount;
            while ((I != 0) && (Result == 0))
            {
                I -= 1;
                var call__77 = FImages[I].RemapToPixelFormat(PixelFormatID);
                Result = call__77;
            }

            return Result;
        }

        // ImageLib.pas:1486
        public override void Show(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int I = default;
            {
                I = 0;
                int end__78 = FCount - 1;
                for (; I <= end__78; I += 1)
                {
                    FImages[I].Show(X, Y, SD);
                }
            }
        }

        // ImageLib.pas:1493
        public override void ShowOpaque(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int I = default;
            {
                I = 0;
                int end__79 = FCount - 1;
                for (; I <= end__79; I += 1)
                {
                    FImages[I].ShowOpaque(X, Y, SD);
                }
            }
        }

        // ImageLib.pas:1500
        public override void ShowTransparent(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int I = default;
            {
                I = 0;
                int end__80 = FCount - 1;
                for (; I <= end__80; I += 1)
                {
                    FImages[I].ShowTransparent(X, Y, SD);
                }
            }
        }

        // ImageLib.pas:1507
        public override void ShowBlended(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.GFXE.TBlendTable Table)
        {
            int I = default;
            {
                I = 0;
                int end__81 = FCount - 1;
                for (; I <= end__81; I += 1)
                {
                    FImages[I].ShowBlended(X, Y, SD, Table);
                }
            }
        }

        // ImageLib.pas:1514
        public override void ShowDC(Ptr<Aow.Rtl.TRect> SrcRect, Ptr<Aow.Rtl.TRect> DestRect, int DC, int BkColor)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.WindowFromDC */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:1550
        public override void ShowEx(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD, in Aow.Units.ImageLib.TShowInfo ShowInfo)
        {
            int I = default;
            {
                I = 0;
                int end__82 = FCount - 1;
                for (; I <= end__82; I += 1)
                {
                    FImages[I].ShowEx(X, Y, SD, ShowInfo);
                }
            }
        }

        // ImageLib.pas:1557
        public override bool SupportShowMode(Aow.Units.ImageLib.TShowMode ShowMode, int PixelFormatID)
        {
            bool Result = default;
            int I = default;
            I = FCount;
            Result = true;
            while (Result && (I != 0))
            {
                I -= 1;
                var call__83 = FImages[I].SupportShowMode(ShowMode, PixelFormatID);
                Result = call__83;
            }

            return Result;
        }

        // ImageLib.pas:1570
        public override void SetLoadMode(Aow.Units.ImageLib.TLoadMode Value)
        {
            int I = default;
            {
                I = 0;
                int end__84 = FCount - 1;
                for (; I <= end__84; I += 1)
                {
                    FImages[I].SetLoadMode(Value);
                }
            }

            FLoadMode = Value;
        }

        // ImageLib.pas:1578
        public override void ReleaseDiscardableImageData()
        {
            int I = default;
            {
                I = 0;
                int end__85 = FCount - 1;
                for (; I <= end__85; I += 1)
                {
                    FImages[I].ReleaseDiscardableImageData();
                }
            }

            FDataLoaded = false;
        }

        // ImageLib.pas:1586
        public override void LoadDiscardableImageData()
        {
            int I = default;
            {
                I = 0;
                int end__86 = FCount - 1;
                for (; I <= end__86; I += 1)
                {
                    FImages[I].LoadDiscardableImageData();
                }
            }

            FDataLoaded = true;
        }

        public int Count { get => FCount; }

        public Aow.Units.ImageLib.TLibraryImage this[int Index] { get => GetImage(Index); }

        // ImageLib.pas:334
        public static new Aow.Units.ImageLib.TImageNode Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            Aow.Units.ImageLib.TImageNode o = new Aow.Units.ImageLib.TImageNode();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.ImageLib.TLibraryImage.Meta
        {
            // ImageLib.pas:1298
            public override int ClassID()
            {
                int Result = default;
                Result = 256;
                return Result;
            }

            // ImageLib.pas:1303
            public override Aow.Rtl.AnsiString Name()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"";
                return Result;
            }

            // ImageLib.pas:334
            public new Aow.Units.ImageLib.TImageNode Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
            {
                Aow.Units.ImageLib.TImageNode o = (Aow.Units.ImageLib.TImageNode)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ImageLib.TImageNode => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { ImageLib.TImage08 (instance size 76): the base of the 8 bit images. +$40 FOwnPalette (the
//     image owns FImagePalette; otherwise it is shared through the palette manager of the
//     library), +$44 FImagePalette, +$48 FRemapPalette (the palette the pixels are shown with:
//     the image palette, or the copy remapped to the pixel format of the last surface). VMT: adds
//     RegisterPalette (share the palette through the owner's palette manager) and
//     UnregisterPalette (make a private copy again). }
    public partial class TImage08 : Aow.Units.ImageLib.TLibraryImage
    {
        internal bool FOwnPalette;
        internal Aow.Units.ImageLib.TLibraryImagePalette FImagePalette;
        internal Aow.Units.GFXE.TGFXEPalette FRemapPalette;
        // ImageLib.pas:7152
        // { TImage08 }
        public override void Clear()
        {
            UnregisterPalette();
            FImagePalette.Free();
            FImagePalette = null;
            base.Clear();
        }

        // ImageLib.pas:7160
        public override void ReadImageData(Aow.Rtl.TStream Stream)
        {
            int Index = default;
            base.ReadImageData(Stream);
            Stream.Read(Aow.Rtl.Bytes.Of(ref FOwnPalette, 1));
            if (FOwnPalette)
            {
                var call__87 = Aow.Units.ImageLib.TLibraryImagePalette.Create();
                FImagePalette = call__87;
                FImagePalette.LoadFromStream(Stream);
            }
            else
            {
                Stream.Read(Aow.Rtl.Bytes.Of(ref Index, 4));
                var call__88 = FOwner.FPaletteManager.LinkToPalette(Index);
                FImagePalette = call__88;
            }

            FRemapPalette = FImagePalette.FPalette;
        }

        // ImageLib.pas:7179
        public override void WriteImageData(Aow.Rtl.TStream Stream)
        {
            base.WriteImageData(Stream);
            Stream.Write(Aow.Rtl.Bytes.Of(ref FOwnPalette, 1));
            if (FOwnPalette)
            {
                FImagePalette.SaveToStream(Stream);
            }
            else
            {
                Stream.Write(Aow.Rtl.Bytes.Of(ref FImagePalette.FIndex, 4));
            }
        }

        // ImageLib.pas:7189
        public virtual void RegisterPalette()
        {
            Aow.Units.ImageLib.TLibraryImagePalette Old = default;
            if ((FOwnPalette && (FImagePalette != null)) && (FOwner != null))
            {
                Old = FImagePalette;
                var call__89 = FOwner.FPaletteManager.RegisterPalette(FImagePalette.FPalette);
                FImagePalette = call__89;
                Old.Free();
                FRemapPalette = FImagePalette.FPalette;
                FOwnPalette = false;
            }
        }

        // ImageLib.pas:7203
        public virtual void UnregisterPalette()
        {
            Aow.Units.ImageLib.TLibraryImagePalette P = default;
            if (!FOwnPalette && (FImagePalette != null))
            {
                P = Aow.Units.ImageLib.TLibraryImagePalette.Create();
                P.Assign(FImagePalette);
                FOwner.FPaletteManager.UnregisterPalette(FImagePalette.FIndex);
                FImagePalette = P;
                FRemapPalette = FImagePalette.FPalette;
                FOwnPalette = true;
            }
        }

        // ImageLib.pas:7218
        public void CreateLibraryImagePalette(Aow.Units.GFXE.TGFXEPalette Palette)
        {
            UnregisterPalette();
            if (FImagePalette == null)
            {
                var call__90 = Aow.Units.ImageLib.TLibraryImagePalette.Create();
                FImagePalette = call__90;
            }

            FImagePalette.FPalette.Assign(Palette);
            FRemapPalette = FImagePalette.FPalette;
            FOwnPalette = true;
        }

        // ImageLib.pas:7227
        public override void SetOwner(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
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
            int Result = default;
            Aow.Units.GFXE.TGFXEPalette P = default;
            var call__91 = FImagePalette.RemapPalette(PixelFormatID);
            P = call__91;
            if (P != null)
            {
                FRemapPalette = P;
                Result = Aow.Units.GFXE.GFXE.GFXE_OK;
            }
            else
            {
                Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
            }

            return Result;
        }

        // ImageLib.pas:7251
        public override bool SupportGrabPixelFormat(int PixelFormatID)
        {
            bool Result = default;
            Result = PixelFormatID == Aow.Units.GFXE.GFXE.pfPAL8;
            return Result;
        }

        // ImageLib.pas:7258
        // { ShowDC remaps the palette to pfPAL8 (an original slip: the surface is pfRGB565; ShowOpaque
//   remaps the palette again when it differs). }
        public override void ShowDC(Ptr<Aow.Rtl.TRect> SrcRect, Ptr<Aow.Rtl.TRect> DestRect, int DC, int BkColor)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.WindowFromDC */
            throw new System.NotImplementedException();
        }
    }

    // { ImageLib.TPicture08 (instance size 88): an opaque 8 bit picture (ClassID 1). +$4C FPitch (of
//     the rows: a multiple of 4 below 32 pixels, of 8 from 32 on), +$50 FBuffer (the allocated
//     block), +$54 FBits (FBuffer aligned to 8 bytes). }
    public partial class TPicture08 : Aow.Units.ImageLib.TImage08
    {
        internal int FPitch;
        internal Ptr<byte> FBuffer;
        internal object FBits;
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
            if (Value < (FDataWidth + FXshift))
            {
                Value = FDataWidth + FXshift;
            }

            base.SetWidth(Value);
        }

        // ImageLib.pas:7325
        public override void SetHeight(int Value)
        {
            if (Value < (FDataHeight + FYshift))
            {
                Value = FDataHeight + FYshift;
            }

            base.SetHeight(Value);
        }

        // ImageLib.pas:7331
        public override bool SupportShowMode(Aow.Units.ImageLib.TShowMode ShowMode, int PixelFormatID)
        {
            bool Result = default;
            Result = ((((PixelFormatID == 0) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfPAL8)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB555)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565)) && (ShowMode == Aow.Units.ImageLib.TShowMode.smOpaque);
            return Result;
        }

        // ImageLib.pas:7337
        public void AllocateDiscardableImageData(int AWidth, int AHeight)
        {
            int Size = default;
            if (AWidth < 32)
            {
                FPitch = (AWidth + 3) & ~3;
            }
            else
            {
                FPitch = (AWidth + 7) & ~7;
            }

            Size = (FPitch * AHeight) + 7;
            FBuffer = FBuffer.Realloc(Size);
            FBits = Aow.Rtl.Rtl.AlignPointer(FBuffer + 7, ~7).Box();
        }

        // ImageLib.pas:7350
        public override void ReleaseDiscardableImageData()
        {
            if (FDataLoaded)
            {
                FBuffer = FBuffer.Realloc(0);
                FDataLoaded = false;
            }
        }

        // ImageLib.pas:7359
        public override void ReadDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            int Y = default;
            base.ReadDiscardableImageData(Stream);
            AllocateDiscardableImageData(FDataWidth, FDataHeight);
            {
                Y = 0;
                int end__92 = FDataHeight - 1;
                for (; Y <= end__92; Y += 1)
                {
                    Stream.Read((Aow.Rtl.Rtl.BytePointer(FBits) + (Y * FPitch)).AsSpan(FDataWidth));
                }
            }
        }

        // ImageLib.pas:7369
        public override void WriteDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            int Y = default;
            base.WriteDiscardableImageData(Stream);
            {
                Y = 0;
                int end__93 = FDataHeight - 1;
                for (; Y <= end__93; Y += 1)
                {
                    Stream.Write((Aow.Rtl.Rtl.BytePointer(FBits) + (Y * FPitch)).AsSpan(FDataWidth));
                }
            }
        }

        // ImageLib.pas:7378
        public override Aow.Rtl.TForm CreateGrabForm(Aow.Rtl.TComponent AOwner, in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.ImageLib.TImagesGrabbedEvent OnImagesGrabbed)
        {
            Aow.Rtl.TForm Result = default;
            Result = null;
            return Result;
        }

        // ImageLib.pas:7384
        public override bool SupportGrabPixelFormat(int PixelFormatID)
        {
            bool Result = default;
            Result = PixelFormatID == Aow.Units.GFXE.GFXE.pfPAL8;
            return Result;
        }

        // ImageLib.pas:7389
        public override bool GrabImage(in Aow.Units.GFXE.TSurfaceDesc SD, in Aow.Rtl.TRect R, Aow.Units.ImageLib.TGrabInfo GrabInfo)
        {
            bool Result = default;
            int Y = default;
            try
            {
                Clear();
                Result = true;
                FDataLoaded = true;
                FDataWidth = R.Right - R.Left;
                FDataHeight = R.Bottom - R.Top;
                FWidth = FDataWidth;
                FHeight = FDataHeight;
                var call__94 = Aow.Rtl.Rtl.ExtractFileName(GrabInfo.FileName);
                FFileName = call__94;
                FFileIndex = GrabInfo.FileIndex;
                FDataSize = FDataWidth * FDataHeight;
                AllocateDiscardableImageData(FWidth, FHeight);
                if (SD.PixelFormatID == Aow.Units.GFXE.GFXE.pfPAL8)
                {
                    {
                        Y = 0;
                        int end__95 = FHeight - 1;
                        for (; Y <= end__95; Y += 1)
                        {
                            Aow.Rtl.Rtl.Move(((Aow.Rtl.Rtl.BytePointer(SD.Data) + ((R.Top + Y) * SD.Pitch)) + R.Left).AsSpan(FDataWidth), (Aow.Rtl.Rtl.BytePointer(FBits) + (FPitch * Y)).AsSpan(FDataWidth), FDataWidth);
                        }
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
            catch (System.Exception)
            {
                ReleaseDiscardableImageData();
                Result = false;
            }

            return Result;
        }

        public new partial class Meta : Aow.Units.ImageLib.TLibraryImage.Meta
        {
            // ImageLib.pas:7295
            // { TPicture08 }
            public override int ClassID()
            {
                int Result = default;
                Result = 1;
                return Result;
            }

            // ImageLib.pas:7300
            public override Aow.Rtl.AnsiString Name()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Picture (08bpp)";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ImageLib.TPicture08 => (Meta)MetaRegistry.Of(typeof(T));
            // ImageLib.pas:271
            public new Aow.Units.ImageLib.TPicture08 Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
            {
                Aow.Units.ImageLib.TPicture08 o = (Aow.Units.ImageLib.TPicture08)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { ImageLib.TRLESprite08 (instance size 104): an 8 bit sprite, run length encoded (ClassID 2).
//     +$4C FTransparentColor (the palette index of the transparent pixels), +$50 FReserved (only
//     stored), +$54 FData (the encoded rows: per row a length dword, then the opaque pixels and,
//     for a transparent run, the transparent index and the run length), +$58 FRLELeft, +$5C
//     FRLETop, +$60 FRLEWidth, +$64 FRLEHeight (the rectangle of the encoded pixels in the data
//     rectangle). }
    public partial class TRLESprite08 : Aow.Units.ImageLib.TImage08
    {
        internal int FTransparentColor;
        internal int FReserved;
        internal Ptr<byte> FData;
        internal int FRLELeft;
        internal int FRLETop;
        internal int FRLEWidth;
        internal int FRLEHeight;
        // ImageLib.pas:7512
        public override void ReleaseDiscardableImageData()
        {
            if (FDataLoaded)
            {
                FData = FData.Realloc(0);
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
            int W = default;
            W = (FXshift + FRLELeft) + FRLEWidth;
            if (Value < W)
            {
                Value = W;
            }

            base.SetWidth(Value);
        }

        // ImageLib.pas:7544
        public override void SetHeight(int Value)
        {
            int H = default;
            H = (FYshift + FRLETop) + FRLEHeight;
            if (Value < H)
            {
                Value = H;
            }

            base.SetHeight(Value);
        }

        // ImageLib.pas:7553
        public override void GetVisualDimensions(ref Aow.Rtl.TRect R)
        {
            R.Left = FXshift + FRLELeft;
            R.Right = R.Left + FRLEWidth;
            R.Top = FYshift + FRLETop;
            R.Bottom = R.Top + FRLEHeight;
        }

        // ImageLib.pas:7561
        public override void ReadImageData(Aow.Rtl.TStream Stream)
        {
            base.ReadImageData(Stream);
            Stream.Read(Aow.Rtl.Bytes.Of(ref FRLEWidth, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FRLEHeight, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FRLELeft, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FRLETop, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FTransparentColor, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FReserved, 4));
        }

        // ImageLib.pas:7572
        public override void WriteImageData(Aow.Rtl.TStream Stream)
        {
            base.WriteImageData(Stream);
            Stream.Write(Aow.Rtl.Bytes.Of(ref FRLEWidth, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FRLEHeight, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FRLELeft, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FRLETop, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FTransparentColor, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FReserved, 4));
        }

        // ImageLib.pas:7583
        public override void ReadDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            base.ReadDiscardableImageData(Stream);
            FData = FData.Realloc(FDataSize);
            Stream.Read(FData.AsSpan(FDataSize));
        }

        // ImageLib.pas:7590
        public override void WriteDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            base.WriteDiscardableImageData(Stream);
            Stream.Write(FData.AsSpan(FDataSize));
        }

        // ImageLib.pas:7596
        public override bool SupportShowMode(Aow.Units.ImageLib.TShowMode ShowMode, int PixelFormatID)
        {
            bool Result = default;
            Result = (((PixelFormatID == 0) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB555)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565)) && (ShowMode == Aow.Units.ImageLib.TShowMode.smOpaque);
            return Result;
        }

        // ImageLib.pas:7602
        public override void ShowOpaque(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            if (!FDataLoaded)
            {
                LoadDiscardableImageData();
                if (SD.PixelFormatID == Aow.Units.GFXE.GFXE.pfPAL8)
                {
                    Aow.Units.ImageLib.ImageLib.ShowRLE08((X + FXshift) + FRLELeft, (Y + FYshift) + FRLETop, SD, FData, FRLEWidth, FRLEHeight, unchecked((byte)FTransparentColor));
                }
                else
                {
                    bool bool__96 = (SD.PixelFormatID != FRemapPalette.PixelFormatID);
                    if (bool__96)
                    {
                        var call__97 = RemapToPixelFormat(SD.PixelFormatID);
                        bool__96 = (call__97 != Aow.Units.GFXE.GFXE.GFXE_OK);
                    }

                    if (bool__96)
                    {
                        throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                    }

                    switch (SD.PixelFormatID)
                    {
                        case Aow.Units.GFXE.GFXE.pfRGB555:
                        case Aow.Units.GFXE.GFXE.pfRGB565:
                            Aow.Units.ImageLib.ImageLib.ShowRLE08to16((X + FXshift) + FRLELeft, (Y + FYshift) + FRLETop, SD, FData, FRLEWidth, FRLEHeight, unchecked((byte)FTransparentColor), Ptr<Aow.Units.GFXE.TPaletteEntry>.To(FRemapPalette.Entries, 0).Reinterpret<Aow.Units.GFXE.TPaletteEntry>().Box());
                            break;
                        default:
                            throw Aow.Rtl.Exception.Create((AnsiString)"PixelFormat not supported");
                            break;
                    }
                }

                if (FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadFreeOnDemand)
                {
                    ReleaseDiscardableImageData();
                }
            }
            else
            {
                if (SD.PixelFormatID == Aow.Units.GFXE.GFXE.pfPAL8)
                {
                    Aow.Units.ImageLib.ImageLib.ShowRLE08((X + FXshift) + FRLELeft, (Y + FYshift) + FRLETop, SD, FData, FRLEWidth, FRLEHeight, unchecked((byte)FTransparentColor));
                }
                else
                {
                    bool bool__98 = (SD.PixelFormatID != FRemapPalette.PixelFormatID);
                    if (bool__98)
                    {
                        var call__99 = RemapToPixelFormat(SD.PixelFormatID);
                        bool__98 = (call__99 != Aow.Units.GFXE.GFXE.GFXE_OK);
                    }

                    if (bool__98)
                    {
                        throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                    }

                    switch (SD.PixelFormatID)
                    {
                        case Aow.Units.GFXE.GFXE.pfRGB555:
                        case Aow.Units.GFXE.GFXE.pfRGB565:
                            Aow.Units.ImageLib.ImageLib.ShowRLE08to16((X + FXshift) + FRLELeft, (Y + FYshift) + FRLETop, SD, FData, FRLEWidth, FRLEHeight, unchecked((byte)FTransparentColor), Ptr<Aow.Units.GFXE.TPaletteEntry>.To(FRemapPalette.Entries, 0).Reinterpret<Aow.Units.GFXE.TPaletteEntry>().Box());
                            break;
                        default:
                            throw Aow.Rtl.Exception.Create((AnsiString)"PixelFormat not supported");
                            break;
                    }
                }
            }
        }

        // ImageLib.pas:7643
        public override bool SupportGrabPixelFormat(int PixelFormatID)
        {
            bool Result = default;
            Result = PixelFormatID == Aow.Units.GFXE.GFXE.pfPAL8;
            return Result;
        }

        // ImageLib.pas:7648
        public override Aow.Rtl.TForm CreateGrabForm(Aow.Rtl.TComponent AOwner, in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.ImageLib.TImagesGrabbedEvent OnImagesGrabbed)
        {
            Aow.Rtl.TForm Result = default;
            Result = null;
            return Result;
        }

        // ImageLib.pas:7659
        // { GrabImage encodes the rows of the smallest rectangle holding the opaque pixels: per row a
//   length dword (only its low word is written), the opaque pixels, and the runs of transparent
//   pixels as the transparent index and the run length (up to 254). The buffer grows by 4 KB
//   whenever the position reaches FDataSize, which stays 0 until the end (an original slip: every
//   byte reallocates). Temp is never created. }
        public override bool GrabImage(in Aow.Units.GFXE.TSurfaceDesc SD, in Aow.Rtl.TRect R, Aow.Units.ImageLib.TGrabInfo GrabInfo)
        {
            bool Result = default;
            int Pos = default;
            int LineStart = default;
            int Count = default;
            Aow.Rtl.TObject Temp = default;
            int Line = default;
            Aow.Rtl.TRect MinRect = default;
            int X = default;
            int Y = default;
            int B = default;
            int Left = default;
            int Top = default;
            int Bottom = default;
            void AddByte(byte B)
            {
                Ptr<byte> P = default;
                if (FDataSize <= Pos)
                {
                    FData = FData.Realloc(Pos + 4096);
                }

                P = (FData + Pos).Reinterpret<byte>();
                P.Value = unchecked((byte)B);
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
                    AddByte(unchecked((byte)FTransparentColor));
                    if (Count > 254)
                    {
                        Count -= 254;
                        AddByte(254);
                    }
                    else
                    {
                        AddByte(unchecked((byte)Count));
                        Count = 0;
                    }
                }
            }

            void EndLine()
            {
                FlushTransparent();
                if (FDataSize <= Pos)
                {
                    FData = FData.Realloc(Pos + 4096);
                }

                {
                    ushort value__100 = unchecked((ushort)(Pos - LineStart));
                    MemoryMarshal.Write<ushort>((FData + LineStart).AsSpan(2), in value__100);
                }
            }

            // { Tried record rebasing/casts and receiver scope; minimizer argument order still differs. }
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
                        var call__101 = Aow.Rtl.Rtl.ExtractFileName(GrabInfo.FileName);
                        FFileName = call__101;
                        FFileIndex = GrabInfo.FileIndex;
                        Pos = 0;
                        Count = 0;
                        Aow.Units.GFXE.GFXE.MinimizeTransparentRect(SD, R, FTransparentColor, ref MinRect);
                        Left = MinRect.Left;
                        FRLELeft = Left - R.Left;
                        Top = MinRect.Top;
                        FRLETop = Top - R.Top;
                        FRLEWidth = MinRect.Right - Left;
                        Bottom = MinRect.Bottom;
                        FRLEHeight = Bottom - Top;
                        {
                            Y = Top;
                            int end__102 = Bottom - 1;
                            for (; Y <= end__102; Y += 1)
                            {
                                Line = Aow.Rtl.Rtl.BytePointer(SD.Data).ByteIndex + (SD.Pitch * Y);
                                StartLine();
                                {
                                    X = MinRect.Left;
                                    int end__103 = MinRect.Right - 1;
                                    for (; X <= end__103; X += 1)
                                    {
                                        /* TODO(T-A3) Integer address requires managed pointer provenance */
                                        throw new System.NotImplementedException();
                                        if (B != FTransparentColor)
                                        {
                                            FlushTransparent();
                                            AddByte(unchecked((byte)B));
                                        }
                                        else
                                        {
                                            Count += 1;
                                        }
                                    }
                                }

                                EndLine();
                            }
                        }

                        FData = FData.Realloc(Pos);
                        FDataSize = Pos;
                        CreateLibraryImagePalette(SD.Palette);
                    }
                    finally
                    {
                        if (Temp != null)
                        {
                            Temp.Destroy();
                        }
                    }
                }
                catch (System.Exception)
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

        public new partial class Meta : Aow.Units.ImageLib.TLibraryImage.Meta
        {
            // ImageLib.pas:7502
            // { TRLESprite08 }
            public override int ClassID()
            {
                int Result = default;
                Result = 2;
                return Result;
            }

            // ImageLib.pas:7507
            public override Aow.Rtl.AnsiString Name()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"RLE Sprite (08bpp)";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ImageLib.TRLESprite08 => (Meta)MetaRegistry.Of(typeof(T));
            // ImageLib.pas:271
            public new Aow.Units.ImageLib.TRLESprite08 Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
            {
                Aow.Units.ImageLib.TRLESprite08 o = (Aow.Units.ImageLib.TRLESprite08)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { ImageLib.TImage16 (instance size 72): the base of the 16 bit images (pfRGB565 or pfRGB555).
//     +$40 FPixelFormatID (of the pixels in memory), +$44 FStoredPixelFormatID (of the pixels in
//     the library file: WriteImageData remaps the pixels back to it before they are written). }
    public partial class TImage16 : Aow.Units.ImageLib.TLibraryImage
    {
        internal int FPixelFormatID;
        internal int FStoredPixelFormatID;
        // ImageLib.pas:2098
        // { TImage16 }
        public override void Create__init(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            base.Create__init(AOwner);
        }

        // ImageLib.pas:2103
        public override void ReadImageData(Aow.Rtl.TStream Stream)
        {
            base.ReadImageData(Stream);
            Stream.Read(Aow.Rtl.Bytes.Of(ref FStoredPixelFormatID, 4));
            FPixelFormatID = FStoredPixelFormatID;
        }

        // ImageLib.pas:2110
        public override void WriteImageData(Aow.Rtl.TStream Stream)
        {
            base.WriteImageData(Stream);
            RemapToPixelFormat(FStoredPixelFormatID);
            Stream.Write(Aow.Rtl.Bytes.Of(ref FStoredPixelFormatID, 4));
        }

        // ImageLib.pas:2117
        public override void ReadDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            base.ReadDiscardableImageData(Stream);
            FPixelFormatID = FStoredPixelFormatID;
        }

        // ImageLib.pas:2123
        public override void WriteDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            base.WriteDiscardableImageData(Stream);
        }

        // ImageLib.pas:2128
        public override void ShowDC(Ptr<Aow.Rtl.TRect> SrcRect, Ptr<Aow.Rtl.TRect> DestRect, int DC, int BkColor)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.WindowFromDC */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:464
        public static new Aow.Units.ImageLib.TImage16 Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            Aow.Units.ImageLib.TImage16 o = new Aow.Units.ImageLib.TImage16();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { ImageLib.TPicture16 (instance size 84, ClassID $10): an opaque 16 bit picture. +$48 FPitch
//     (bytes per row, a multiple of 4, or of 8 from 32 pixels on), +$4C FBuffer (the allocated
//     memory), +$50 FData (FBuffer aligned to 8 bytes). }
    public partial class TPicture16 : Aow.Units.ImageLib.TImage16
    {
        internal int FPitch;
        internal Ptr<byte> FBuffer;
        internal object FData;
        // ImageLib.pas:2206
        internal void AllocateDiscardableImageData(int AWidth, int AHeight)
        {
            int Size = default;
            if (AWidth < 32)
            {
                FPitch = ((AWidth << 1) + 3) & ~3;
            }
            else
            {
                FPitch = ((AWidth << 1) + 7) & ~7;
            }

            Size = (FPitch * AHeight) + 7;
            FBuffer = FBuffer.Realloc(Size);
            FData = Aow.Rtl.Rtl.AlignPointer(FBuffer + 7, ~7).Box();
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
            if (Value < (FXshift + FDataWidth))
            {
                Value = FXshift + FDataWidth;
            }

            base.SetWidth(Value);
        }

        // ImageLib.pas:2194
        public override void SetHeight(int Value)
        {
            if (Value < (FYshift + FDataHeight))
            {
                Value = FYshift + FDataHeight;
            }

            base.SetHeight(Value);
        }

        // ImageLib.pas:2200
        public override bool SupportShowMode(Aow.Units.ImageLib.TShowMode ShowMode, int PixelFormatID)
        {
            bool Result = default;
            Result = (((PixelFormatID == 0) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB555)) && TSet<Aow.Units.ImageLib.TShowMode>.Of(Aow.Units.ImageLib.TShowMode.smOpaque, Aow.Units.ImageLib.TShowMode.smTransparent, Aow.Units.ImageLib.TShowMode.smBlended).Contains(ShowMode);
            return Result;
        }

        // ImageLib.pas:2219
        public override void ReleaseDiscardableImageData()
        {
            if (FDataLoaded)
            {
                FBuffer = FBuffer.Realloc(0);
                FDataLoaded = false;
            }
        }

        // ImageLib.pas:2228
        public override void ReadDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            int Y = default;
            base.ReadDiscardableImageData(Stream);
            AllocateDiscardableImageData(FDataWidth, FDataHeight);
            {
                Y = 0;
                int end__104 = FDataHeight - 1;
                for (; Y <= end__104; Y += 1)
                {
                    Stream.Read((Aow.Rtl.Rtl.BytePointer(FData) + (Y * FPitch)).AsSpan(FDataWidth * 2));
                }
            }
        }

        // ImageLib.pas:2238
        public override void WriteDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            int Y = default;
            base.WriteDiscardableImageData(Stream);
            {
                Y = 0;
                int end__105 = FDataHeight - 1;
                for (; Y <= end__105; Y += 1)
                {
                    Stream.Write((Aow.Rtl.Rtl.BytePointer(FData) + (Y * FPitch)).AsSpan(FDataWidth * 2));
                }
            }
        }

        // ImageLib.pas:2247
        public override Aow.Rtl.TForm CreateGrabForm(Aow.Rtl.TComponent AOwner, in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.ImageLib.TImagesGrabbedEvent OnImagesGrabbed)
        {
            Aow.Rtl.TForm Result = default;
            Result = null;
            return Result;
        }

        // ImageLib.pas:2253
        public override bool SupportGrabPixelFormat(int PixelFormatID)
        {
            bool Result = default;
            Result = (((PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB555)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB24)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfPAL8);
            return Result;
        }

        // ImageLib.pas:2259
        public override bool GrabImage(in Aow.Units.GFXE.TSurfaceDesc SD, in Aow.Rtl.TRect R, Aow.Units.ImageLib.TGrabInfo GrabInfo)
        {
            bool Result = default;
            int Y = default;
            int I = default;
            int X = default;
            Ptr<byte> S = default;
            Ptr<ushort> D = default;
            Ptr<Aow.Units.GFXE.TPaletteEntry> P = default;
            Ptr<byte> S24 = default;
            // { Tried absolute/addressed descriptor and receiver scope; descriptor/pixel locations still differ. }
            try
            {
                Clear();
                Result = true;
                FDataLoaded = true;
                FDataWidth = R.Right - R.Left;
                FDataHeight = R.Bottom - R.Top;
                FWidth = FDataWidth;
                FHeight = FDataHeight;
                var call__106 = Aow.Rtl.Rtl.ExtractFileName(GrabInfo.FileName);
                FFileName = call__106;
                FFileIndex = GrabInfo.FileIndex;
                FDataSize = (FDataWidth * FDataHeight) * 2;
                AllocateDiscardableImageData(FWidth, FHeight);
                switch (SD.PixelFormatID)
                {
                    case Aow.Units.GFXE.GFXE.pfPAL8:
                    {
                        Y = 0;
                        int end__107 = FHeight - 1;
                        for (; Y <= end__107; Y += 1)
                        {
                            S = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + ((R.Top + Y) * SD.Pitch)) + R.Left).Reinterpret<byte>();
                            D = (Aow.Rtl.Rtl.BytePointer(FData) + (FPitch * Y)).Reinterpret<ushort>();
                            {
                                X = 0;
                                int end__108 = FWidth - 1;
                                for (; X <= end__108; X += 1)
                                {
                                    P = Ptr<Aow.Units.GFXE.TPaletteEntry>.To(SD.Palette.Entries, S.Value);
                                    D.Value = unchecked((ushort)((((P.Value.R & 248) << 8) + ((P.Value.G & 252) << 3)) + ((int)(((uint)P.Value.B) >> 3))));
                                    S += 1;
                                    D += 1;
                                }
                            }
                        }
                    }

                        FStoredPixelFormatID = Aow.Units.GFXE.GFXE.pfRGB565;
                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB565:
                    case Aow.Units.GFXE.GFXE.pfRGB555:
                    {
                        I = 0;
                        int end__109 = FHeight - 1;
                        for (; I <= end__109; I += 1)
                        {
                            Aow.Rtl.Rtl.Move(((Aow.Rtl.Rtl.BytePointer(SD.Data) + ((R.Top + I) * SD.Pitch)) + (R.Left * 2)).AsSpan(FWidth * 2), (Aow.Rtl.Rtl.BytePointer(FData) + (FPitch * I)).AsSpan(FWidth * 2), FWidth * 2);
                        }
                    }

                        FStoredPixelFormatID = SD.PixelFormatID;
                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB24:
                    {
                        Y = 0;
                        int end__110 = FHeight - 1;
                        for (; Y <= end__110; Y += 1)
                        {
                            D = (Aow.Rtl.Rtl.BytePointer(FData) + (FPitch * Y)).Reinterpret<ushort>();
                            S24 = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + ((R.Top + Y) * SD.Pitch)) + (R.Left * 3)).Reinterpret<byte>();
                            {
                                X = 0;
                                int end__111 = FWidth - 1;
                                for (; X <= end__111; X += 1)
                                {
                                    D.Value = unchecked((ushort)((((S24[2] & 248) << 8) + ((S24[1] & 252) << 3)) + ((int)(((uint)S24[0]) >> 3))));
                                    S24 = S24.Reinterpret<byte>() + 3;
                                    D += 1;
                                }
                            }
                        }
                    }

                        FStoredPixelFormatID = Aow.Units.GFXE.GFXE.pfRGB565;
                        break;
                    default:
                        Result = false;
                        break;
                }

                FPixelFormatID = FStoredPixelFormatID;
            }
            catch (System.Exception)
            {
                ReleaseDiscardableImageData();
                Result = false;
            }

            return Result;
        }

        // ImageLib.pas:2329
        public override int RemapToPixelFormat(int PixelFormatID)
        {
            int Result = default;
            Result = 0;
            if (PixelFormatID != FPixelFormatID)
            {
                switch (PixelFormatID)
                {
                    case Aow.Units.GFXE.GFXE.pfRGB565:
                        Aow.Units.GFXE.GFXE.RemapSurface555to565(FData, FData, FPitch, FPitch, FDataWidth, FDataHeight);
                        FPixelFormatID = Aow.Units.GFXE.GFXE.pfRGB565;
                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB555:
                        Aow.Units.GFXE.GFXE.RemapSurface565to555(FData, FData, FPitch, FPitch, FDataWidth, FDataHeight);
                        FPixelFormatID = Aow.Units.GFXE.GFXE.pfRGB555;
                        break;
                    default:
                        Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                        break;
                }
            }

            return Result;
        }

        // ImageLib.pas:2349
        public override void ShowOpaque(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int DX = default;
            int Right = default;
            int W = default;
            int H = default;
            int SX = default;
            int SY = default;
            int DY = default;
            int Bottom = default;
            object Src = default;
            object Dst = default;
            // { Tried Self identity, receiver/descriptor with and descriptor pointer; image/descriptor registers still differ. }
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
                        Src = ((Aow.Rtl.Rtl.BytePointer(FData) + (SX * 2)) + (SY * FPitch)).Box();
                        Dst = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + (DX << 1)) + (SD.Pitch * DY)).Box();
                        if (SD.PixelFormatID == FPixelFormatID)
                        {
                            Aow.Units.GFXE.GFXE.BltRect(Src, Dst, FPitch, SD.Pitch, W * 2, H);
                        }
                        else
                        {
                            var call__112 = RemapToPixelFormat(SD.PixelFormatID);
                            if (call__112 != 0)
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                            }

                            Aow.Units.GFXE.GFXE.BltRect(Src, Dst, FPitch, SD.Pitch, W * 2, H);
                        }

                        if (FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadFreeOnDemand)
                        {
                            ReleaseDiscardableImageData();
                        }
                    }
                    else
                    {
                        Src = ((Aow.Rtl.Rtl.BytePointer(FData) + (SX * 2)) + (SY * FPitch)).Box();
                        Dst = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + (DX << 1)) + (SD.Pitch * DY)).Box();
                        if (SD.PixelFormatID == FPixelFormatID)
                        {
                            Aow.Units.GFXE.GFXE.BltRect(Src, Dst, FPitch, SD.Pitch, W * 2, H);
                        }
                        else
                        {
                            var call__113 = RemapToPixelFormat(SD.PixelFormatID);
                            if (call__113 != 0)
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                            }

                            Aow.Units.GFXE.GFXE.BltRect(Src, Dst, FPitch, SD.Pitch, W * 2, H);
                        }
                    }
                }
            }
        }

        // ImageLib.pas:2412
        public override void ShowBlended(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.GFXE.TBlendTable Table)
        {
            int DX = default;
            int Right = default;
            int W = default;
            int H = default;
            int SX = default;
            int SY = default;
            int DY = default;
            int Bottom = default;
            object Src = default;
            object Dst = default;
            // { Tried Self identity, receiver/descriptor with and descriptor pointer; image/descriptor registers still differ. }
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
                        Src = ((Aow.Rtl.Rtl.BytePointer(FData) + (SX * 2)) + (SY * FPitch)).Box();
                        Dst = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + (DX << 1)) + (SD.Pitch * DY)).Box();
                        if (SD.PixelFormatID == FPixelFormatID)
                        {
                            if (Table.Bits == 5)
                            {
                                switch (FPixelFormatID)
                                {
                                    case Aow.Units.GFXE.GFXE.pfRGB565:
                                        Aow.Units.GFXE.GFXE.BlendSurface565(Src, Dst, FPitch, SD.Pitch, W, H, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                        break;
                                    case Aow.Units.GFXE.GFXE.pfRGB555:
                                        Aow.Units.GFXE.GFXE.BlendSurface555(Src, Dst, FPitch, SD.Pitch, W, H, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                        break;
                                }
                            }
                            else
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Invalid BlendTable format");
                            }
                        }
                        else
                        {
                            var call__114 = RemapToPixelFormat(SD.PixelFormatID);
                            if (call__114 != 0)
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                            }

                            if (Table.Bits == 5)
                            {
                                switch (FPixelFormatID)
                                {
                                    case Aow.Units.GFXE.GFXE.pfRGB565:
                                        Aow.Units.GFXE.GFXE.BlendSurface565(Src, Dst, FPitch, SD.Pitch, W, H, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                        break;
                                    case Aow.Units.GFXE.GFXE.pfRGB555:
                                        Aow.Units.GFXE.GFXE.BlendSurface555(Src, Dst, FPitch, SD.Pitch, W, H, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                        break;
                                }
                            }
                            else
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Invalid BlendTable format");
                            }
                        }

                        if (FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadFreeOnDemand)
                        {
                            ReleaseDiscardableImageData();
                        }
                    }
                    else
                    {
                        Src = ((Aow.Rtl.Rtl.BytePointer(FData) + (SX * 2)) + (SY * FPitch)).Box();
                        Dst = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + (DX << 1)) + (SD.Pitch * DY)).Box();
                        if (SD.PixelFormatID == FPixelFormatID)
                        {
                            if (Table.Bits == 5)
                            {
                                switch (FPixelFormatID)
                                {
                                    case Aow.Units.GFXE.GFXE.pfRGB565:
                                        Aow.Units.GFXE.GFXE.BlendSurface565(Src, Dst, FPitch, SD.Pitch, W, H, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                        break;
                                    case Aow.Units.GFXE.GFXE.pfRGB555:
                                        Aow.Units.GFXE.GFXE.BlendSurface555(Src, Dst, FPitch, SD.Pitch, W, H, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                        break;
                                }
                            }
                            else
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Invalid BlendTable format");
                            }
                        }
                        else
                        {
                            var call__115 = RemapToPixelFormat(SD.PixelFormatID);
                            if (call__115 != 0)
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                            }

                            if (Table.Bits == 5)
                            {
                                switch (FPixelFormatID)
                                {
                                    case Aow.Units.GFXE.GFXE.pfRGB565:
                                        Aow.Units.GFXE.GFXE.BlendSurface565(Src, Dst, FPitch, SD.Pitch, W, H, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                        break;
                                    case Aow.Units.GFXE.GFXE.pfRGB555:
                                        Aow.Units.GFXE.GFXE.BlendSurface555(Src, Dst, FPitch, SD.Pitch, W, H, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                        break;
                                }
                            }
                            else
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Invalid BlendTable format");
                            }
                        }
                    }
                }
            }
        }

        // ImageLib.pas:2511
        public override void ShowTransparent(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int DX = default;
            int Right = default;
            int W = default;
            int H = default;
            int SX = default;
            int SY = default;
            int DY = default;
            int Bottom = default;
            object Src = default;
            object Dst = default;
            // { Tried Self identity, receiver/descriptor with and descriptor pointer; image/descriptor registers still differ. }
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
                        Src = ((Aow.Rtl.Rtl.BytePointer(FData) + (SX * 2)) + (SY * FPitch)).Box();
                        Dst = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + (DX << 1)) + (SD.Pitch * DY)).Box();
                        if (SD.PixelFormatID == FPixelFormatID)
                        {
                            if (FPixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565)
                            {
                                Aow.Units.GFXE.GFXE.BltTransparent565(Src, Dst, FPitch, SD.Pitch, W, H);
                            }
                            else
                            {
                                Aow.Units.GFXE.GFXE.BltTransparent555(Src, Dst, FPitch, SD.Pitch, W, H);
                            }
                        }
                        else
                        {
                            var call__116 = RemapToPixelFormat(SD.PixelFormatID);
                            if (call__116 != 0)
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                            }

                            if (FPixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565)
                            {
                                Aow.Units.GFXE.GFXE.BltTransparent565(Src, Dst, FPitch, SD.Pitch, W, H);
                            }
                            else
                            {
                                Aow.Units.GFXE.GFXE.BltTransparent555(Src, Dst, FPitch, SD.Pitch, W, H);
                            }
                        }

                        if (FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadFreeOnDemand)
                        {
                            ReleaseDiscardableImageData();
                        }
                    }
                    else
                    {
                        Src = ((Aow.Rtl.Rtl.BytePointer(FData) + (SX * 2)) + (SY * FPitch)).Box();
                        Dst = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + (DX << 1)) + (SD.Pitch * DY)).Box();
                        if (SD.PixelFormatID == FPixelFormatID)
                        {
                            if (FPixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565)
                            {
                                Aow.Units.GFXE.GFXE.BltTransparent565(Src, Dst, FPitch, SD.Pitch, W, H);
                            }
                            else
                            {
                                Aow.Units.GFXE.GFXE.BltTransparent555(Src, Dst, FPitch, SD.Pitch, W, H);
                            }
                        }
                        else
                        {
                            var call__117 = RemapToPixelFormat(SD.PixelFormatID);
                            if (call__117 != 0)
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                            }

                            if (FPixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565)
                            {
                                Aow.Units.GFXE.GFXE.BltTransparent565(Src, Dst, FPitch, SD.Pitch, W, H);
                            }
                            else
                            {
                                Aow.Units.GFXE.GFXE.BltTransparent555(Src, Dst, FPitch, SD.Pitch, W, H);
                            }
                        }
                    }
                }
            }
        }

        public new partial class Meta : Aow.Units.ImageLib.TLibraryImage.Meta
        {
            // ImageLib.pas:2164
            // { TPicture16 }
            public override int ClassID()
            {
                int Result = default;
                Result = 16;
                return Result;
            }

            // ImageLib.pas:2169
            public override Aow.Rtl.AnsiString Name()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Picture (16bpp)";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ImageLib.TPicture16 => (Meta)MetaRegistry.Of(typeof(T));
            // ImageLib.pas:464
            public new Aow.Units.ImageLib.TPicture16 Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
            {
                Aow.Units.ImageLib.TPicture16 o = (Aow.Units.ImageLib.TPicture16)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { ImageLib.TTransparentPicture16 (instance size 84, ClassID $15): a TPicture16 shown
//     transparent by default; it cannot be grabbed. }
    public partial class TTransparentPicture16 : Aow.Units.ImageLib.TPicture16
    {
        // ImageLib.pas:2845
        // { TTransparentPicture16 }
        public override void Create__init(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            base.Create__init(AOwner);
            FShowInfo.ShowMode = Aow.Units.ImageLib.TShowMode.smTransparent;
        }

        // ImageLib.pas:2861
        public override bool SupportGrabPixelFormat(int PixelFormatID)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // ImageLib.pas:509
        public static new Aow.Units.ImageLib.TTransparentPicture16 Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            Aow.Units.ImageLib.TTransparentPicture16 o = new Aow.Units.ImageLib.TTransparentPicture16();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.ImageLib.TPicture16.Meta
        {
            // ImageLib.pas:2851
            public override int ClassID()
            {
                int Result = default;
                Result = 21;
                return Result;
            }

            // ImageLib.pas:2856
            public override Aow.Rtl.AnsiString Name()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Transparent Picture (16bpp)";
                return Result;
            }

            // ImageLib.pas:509
            public new Aow.Units.ImageLib.TTransparentPicture16 Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
            {
                Aow.Units.ImageLib.TTransparentPicture16 o = (Aow.Units.ImageLib.TTransparentPicture16)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ImageLib.TTransparentPicture16 => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { ImageLib.TSprite16 (instance size 108, ClassID $16): a 16 bit picture with a transparent
//     color, cut to the smallest rectangle holding its opaque pixels. +$48 FPitch, +$4C FBuffer,
//     +$50 FData (as TPicture16), +$54 FTransparentColor (in FStoredPixelFormatID), +$58 FColorKey
//     (the transparent color in FPixelFormatID), +$5C FSpriteX, +$60 FSpriteY (of the pixels in the
//     grabbed rectangle), +$64 FSpriteWidth, +$68 FSpriteHeight (of the pixels). }
    public partial class TSprite16 : Aow.Units.ImageLib.TImage16
    {
        internal int FPitch;
        internal Ptr<byte> FBuffer;
        internal object FData;
        internal int FTransparentColor;
        internal int FColorKey;
        internal int FSpriteX;
        internal int FSpriteY;
        internal int FSpriteWidth;
        internal int FSpriteHeight;
        // ImageLib.pas:4302
        internal void AllocateDiscardableImageData(int AWidth, int AHeight)
        {
            int Size = default;
            if (AWidth < 32)
            {
                FPitch = ((AWidth << 1) + 3) & ~3;
            }
            else
            {
                FPitch = ((AWidth << 1) + 7) & ~7;
            }

            Size = (FPitch * AHeight) + 7;
            FBuffer = FBuffer.Realloc(Size);
            FData = Aow.Rtl.Rtl.AlignPointer(FBuffer + 7, ~7).Box();
        }

        // ImageLib.pas:4247
        // { TSprite16 }
        public override void Create__init(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            base.Create__init(AOwner);
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
            int W = default;
            W = (FXshift + FSpriteX) + FSpriteWidth;
            if (Value < W)
            {
                Value = W;
            }

            base.SetWidth(Value);
        }

        // ImageLib.pas:4285
        public override void SetHeight(int Value)
        {
            int W = default;
            W = (FYshift + FSpriteY) + FSpriteHeight;
            if (Value < W)
            {
                Value = W;
            }

            base.SetHeight(Value);
        }

        // ImageLib.pas:4294
        public override void GetVisualDimensions(ref Aow.Rtl.TRect R)
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
                FBuffer = FBuffer.Realloc(0);
                FDataLoaded = false;
            }
        }

        // ImageLib.pas:4324
        public override void ReadImageData(Aow.Rtl.TStream Stream)
        {
            base.ReadImageData(Stream);
            Stream.Read(Aow.Rtl.Bytes.Of(ref FSpriteWidth, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FSpriteHeight, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FSpriteX, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FSpriteY, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FTransparentColor, 4));
        }

        // ImageLib.pas:4334
        public override void WriteImageData(Aow.Rtl.TStream Stream)
        {
            base.WriteImageData(Stream);
            Stream.Write(Aow.Rtl.Bytes.Of(ref FSpriteWidth, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FSpriteHeight, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FSpriteX, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FSpriteY, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FTransparentColor, 4));
        }

        // ImageLib.pas:4344
        public override void ReadDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            int Y = default;
            base.ReadDiscardableImageData(Stream);
            AllocateDiscardableImageData(FSpriteWidth, FSpriteHeight);
            {
                Y = 0;
                int end__118 = FSpriteHeight - 1;
                for (; Y <= end__118; Y += 1)
                {
                    Stream.Read((Aow.Rtl.Rtl.BytePointer(FData) + (Y * FPitch)).AsSpan(FSpriteWidth * 2));
                }
            }

            FColorKey = FTransparentColor;
        }

        // ImageLib.pas:4355
        public override void WriteDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            int Y = default;
            base.WriteDiscardableImageData(Stream);
            {
                Y = 0;
                int end__119 = FSpriteHeight - 1;
                for (; Y <= end__119; Y += 1)
                {
                    Stream.Write((Aow.Rtl.Rtl.BytePointer(FData) + (Y * FPitch)).AsSpan(FSpriteWidth * 2));
                }
            }
        }

        // ImageLib.pas:4364
        public override int RemapToPixelFormat(int PixelFormatID)
        {
            int Result = default;
            Result = 0;
            if (PixelFormatID != FPixelFormatID)
            {
                switch (PixelFormatID)
                {
                    case Aow.Units.GFXE.GFXE.pfRGB565:
                        Aow.Units.GFXE.GFXE.RemapSurface555to565(FData, FData, FPitch, FPitch, FSpriteWidth, FSpriteHeight);
                        var call__120 = Aow.Units.GFXE.GFXE.RemapPixelInfo(FColorKey, Aow.Units.GFXE.GFXE.pfRGB555, null, Aow.Units.GFXE.GFXE.pfRGB565, null);
                        FColorKey = call__120;
                        FPixelFormatID = Aow.Units.GFXE.GFXE.pfRGB565;
                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB555:
                        Aow.Units.GFXE.GFXE.RemapSurface565to555(FData, FData, FPitch, FPitch, FSpriteWidth, FSpriteHeight);
                        var call__121 = Aow.Units.GFXE.GFXE.RemapPixelInfo(FColorKey, Aow.Units.GFXE.GFXE.pfRGB565, null, Aow.Units.GFXE.GFXE.pfRGB555, null);
                        FColorKey = call__121;
                        FPixelFormatID = Aow.Units.GFXE.GFXE.pfRGB555;
                        break;
                    default:
                        Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                        break;
                }
            }

            return Result;
        }

        // ImageLib.pas:4386
        public override bool SupportShowMode(Aow.Units.ImageLib.TShowMode ShowMode, int PixelFormatID)
        {
            bool Result = default;
            Result = (((PixelFormatID == 0) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB555)) && TSet<Aow.Units.ImageLib.TShowMode>.Of(Aow.Units.ImageLib.TShowMode.smOpaque, Aow.Units.ImageLib.TShowMode.smTransparent, Aow.Units.ImageLib.TShowMode.smBlended).Contains(ShowMode);
            return Result;
        }

        // ImageLib.pas:4392
        public override void ShowOpaque(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int DX = default;
            int Right = default;
            int W = default;
            int H = default;
            int SX = default;
            int SY = default;
            int DY = default;
            int Bottom = default;
            object Src = default;
            object Dst = default;
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
                        Src = ((Aow.Rtl.Rtl.BytePointer(FData) + (SX * 2)) + (SY * FPitch)).Box();
                        Dst = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + (DX << 1)) + (SD.Pitch * DY)).Box();
                        if (SD.PixelFormatID == FPixelFormatID)
                        {
                            Aow.Units.GFXE.GFXE.BltColorKey16(Src, Dst, FPitch, SD.Pitch, W, H, FColorKey);
                        }
                        else
                        {
                            var call__122 = RemapToPixelFormat(SD.PixelFormatID);
                            if (call__122 != 0)
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                            }

                            Aow.Units.GFXE.GFXE.BltColorKey16(Src, Dst, FPitch, SD.Pitch, W, H, FColorKey);
                        }

                        if (FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadFreeOnDemand)
                        {
                            ReleaseDiscardableImageData();
                        }
                    }
                    else
                    {
                        Src = ((Aow.Rtl.Rtl.BytePointer(FData) + (SX * 2)) + (SY * FPitch)).Box();
                        Dst = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + (DX << 1)) + (SD.Pitch * DY)).Box();
                        if (SD.PixelFormatID == FPixelFormatID)
                        {
                            Aow.Units.GFXE.GFXE.BltColorKey16(Src, Dst, FPitch, SD.Pitch, W, H, FColorKey);
                        }
                        else
                        {
                            var call__123 = RemapToPixelFormat(SD.PixelFormatID);
                            if (call__123 != 0)
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                            }

                            Aow.Units.GFXE.GFXE.BltColorKey16(Src, Dst, FPitch, SD.Pitch, W, H, FColorKey);
                        }
                    }
                }
            }
        }

        // ImageLib.pas:4457
        public override void ShowBlended(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.GFXE.TBlendTable Table)
        {
            int DX = default;
            int Right = default;
            int W = default;
            int H = default;
            int SX = default;
            int SY = default;
            int DY = default;
            int Bottom = default;
            object Src = default;
            object Dst = default;
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
                        Src = ((Aow.Rtl.Rtl.BytePointer(FData) + (SX * 2)) + (SY * FPitch)).Box();
                        Dst = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + (DX << 1)) + (SD.Pitch * DY)).Box();
                        if (SD.PixelFormatID == FPixelFormatID)
                        {
                            switch (FPixelFormatID)
                            {
                                case Aow.Units.GFXE.GFXE.pfRGB555:
                                    Aow.Units.GFXE.GFXE.BlendColorKey555(Src, Dst, FPitch, SD.Pitch, W, H, FColorKey, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                    break;
                                case Aow.Units.GFXE.GFXE.pfRGB565:
                                    Aow.Units.GFXE.GFXE.BlendColorKey565(Src, Dst, FPitch, SD.Pitch, W, H, FColorKey, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                    break;
                            }
                        }
                        else
                        {
                            var call__124 = RemapToPixelFormat(SD.PixelFormatID);
                            if (call__124 != 0)
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                            }

                            switch (FPixelFormatID)
                            {
                                case Aow.Units.GFXE.GFXE.pfRGB555:
                                    Aow.Units.GFXE.GFXE.BlendColorKey555(Src, Dst, FPitch, SD.Pitch, W, H, FColorKey, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                    break;
                                case Aow.Units.GFXE.GFXE.pfRGB565:
                                    Aow.Units.GFXE.GFXE.BlendColorKey565(Src, Dst, FPitch, SD.Pitch, W, H, FColorKey, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                    break;
                            }
                        }

                        if (FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadFreeOnDemand)
                        {
                            ReleaseDiscardableImageData();
                        }
                    }
                    else
                    {
                        Src = ((Aow.Rtl.Rtl.BytePointer(FData) + (SX * 2)) + (SY * FPitch)).Box();
                        Dst = ((Aow.Rtl.Rtl.BytePointer(SD.Data) + (DX << 1)) + (SD.Pitch * DY)).Box();
                        if (SD.PixelFormatID == FPixelFormatID)
                        {
                            switch (FPixelFormatID)
                            {
                                case Aow.Units.GFXE.GFXE.pfRGB555:
                                    Aow.Units.GFXE.GFXE.BlendColorKey555(Src, Dst, FPitch, SD.Pitch, W, H, FColorKey, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                    break;
                                case Aow.Units.GFXE.GFXE.pfRGB565:
                                    Aow.Units.GFXE.GFXE.BlendColorKey565(Src, Dst, FPitch, SD.Pitch, W, H, FColorKey, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                    break;
                            }
                        }
                        else
                        {
                            var call__125 = RemapToPixelFormat(SD.PixelFormatID);
                            if (call__125 != 0)
                            {
                                throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                            }

                            switch (FPixelFormatID)
                            {
                                case Aow.Units.GFXE.GFXE.pfRGB555:
                                    Aow.Units.GFXE.GFXE.BlendColorKey555(Src, Dst, FPitch, SD.Pitch, W, H, FColorKey, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                    break;
                                case Aow.Units.GFXE.GFXE.pfRGB565:
                                    Aow.Units.GFXE.GFXE.BlendColorKey565(Src, Dst, FPitch, SD.Pitch, W, H, FColorKey, Ptr<Aow.Units.FixedArray_byte_32>.To(((Aow.Units.GFXE.TBlendTable5Bit)Table).Table, 0).Reinterpret<byte>().Box());
                                    break;
                            }
                        }
                    }
                }
            }
        }

        // ImageLib.pas:4543
        // { ShowTransparent shows the sprite blended 50% }
        public override void ShowTransparent(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            ShowBlended(X, Y, SD, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[50]);
        }

        // ImageLib.pas:4548
        public override bool SupportGrabPixelFormat(int PixelFormatID)
        {
            bool Result = default;
            Result = (((PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB555)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB24)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfPAL8);
            return Result;
        }

        // ImageLib.pas:4554
        public override Aow.Rtl.TForm CreateGrabForm(Aow.Rtl.TComponent AOwner, in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.ImageLib.TImagesGrabbedEvent OnImagesGrabbed)
        {
            Aow.Rtl.TForm Result = default;
            Result = null;
            return Result;
        }

        // ImageLib.pas:4561
        // { GrabImage locks the conversion surface and never unlocks it (an original bug) }
        public override bool GrabImage(in Aow.Units.GFXE.TSurfaceDesc SD, in Aow.Rtl.TRect R, Aow.Units.ImageLib.TGrabInfo GrabInfo)
        {
            bool Result = default;
            Aow.Units.GFXE.TDataSurface Surface = default;
            Aow.Units.GFXE.TSurfaceDesc LSD = default;
            Aow.Rtl.TRect GR = default;
            Aow.Rtl.TRect MR = default;
            int Y = default;
            int Left = default;
            int Top = default;
            int Bottom = default;
            // { Absolute views preserve frame slots and the separate GR coordinate reads. }
            Result = true;
            try
            {
                Surface = null;
                try
                {
                    if (SD.BitCount != 16)
                    {
                        var call__126 = Aow.Units.GFXE.TDataSurface.Create(R.Right - R.Left, R.Bottom - R.Top, Aow.Units.GFXE.GFXE.pfRGB565, null);
                        Surface = call__126;
                        GR.Left = 0;
                        GR.Top = 0;
                        var call__127 = Surface.GetWidth();
                        GR.Right = call__127;
                        var call__128 = Surface.GetHeight();
                        GR.Bottom = call__128;
                        Surface.Lock(ref LSD);
                        var call__129 = Surface.GetWidth();
                        var call__130 = Surface.GetHeight();
                        var call__131 = Aow.Units.GFXE.GFXE.RemapSurfacePart(SD, R.Left, R.Top, call__129, call__130, LSD, 0, 0);
                        if (call__131 != 0)
                        {
                            throw Aow.Rtl.Exception.Create((AnsiString)"Invalid PixelFormat");
                        }

                        var call__132 = Aow.Units.GFXE.GFXE.RemapPixelInfo(GrabInfo.TransparentPixel, SD.PixelFormatID, SD.Palette, LSD.PixelFormatID, null);
                        FTransparentColor = call__132;
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
                    var call__133 = Aow.Rtl.Rtl.ExtractFileName(GrabInfo.FileName);
                    FFileName = call__133;
                    FFileIndex = GrabInfo.FileIndex;
                    FStoredPixelFormatID = LSD.PixelFormatID;
                    FPixelFormatID = FStoredPixelFormatID;
                    FColorKey = FTransparentColor;
                    Aow.Units.GFXE.GFXE.MinimizeTransparentRect(LSD, GR, FTransparentColor, ref MR);
                    Left = MR.Left;
                    FSpriteX = Left - GR.Left;
                    Top = MR.Top;
                    FSpriteY = Top - GR.Top;
                    FSpriteWidth = MR.Right - Left;
                    Bottom = MR.Bottom;
                    FSpriteHeight = Bottom - Top;
                    FDataSize = (FSpriteWidth * FSpriteHeight) * 2;
                    AllocateDiscardableImageData(FSpriteWidth, FSpriteHeight);
                    {
                        Y = Top;
                        int end__134 = Bottom - 1;
                        for (; Y <= end__134; Y += 1)
                        {
                            Aow.Rtl.Rtl.Move(((Aow.Rtl.Rtl.BytePointer(LSD.Data) + (LSD.Pitch * Y)) + (MR.Left * 2)).AsSpan(FSpriteWidth * 2), (Aow.Rtl.Rtl.BytePointer(FData) + ((Y - MR.Top) * FPitch)).AsSpan(FSpriteWidth * 2), FSpriteWidth * 2);
                        }
                    }
                }
                finally
                {
                    if (Surface != null)
                    {
                        Surface.Destroy();
                    }
                }
            }
            catch (System.Exception)
            {
                ReleaseDiscardableImageData();
                Result = false;
            }

            return Result;
        }

        // ImageLib.pas:533
        public static new Aow.Units.ImageLib.TSprite16 Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            Aow.Units.ImageLib.TSprite16 o = new Aow.Units.ImageLib.TSprite16();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.ImageLib.TLibraryImage.Meta
        {
            // ImageLib.pas:4252
            public override int ClassID()
            {
                int Result = default;
                Result = 22;
                return Result;
            }

            // ImageLib.pas:4257
            public override Aow.Rtl.AnsiString Name()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Sprite (16bpp)";
                return Result;
            }

            // ImageLib.pas:533
            public new Aow.Units.ImageLib.TSprite16 Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
            {
                Aow.Units.ImageLib.TSprite16 o = (Aow.Units.ImageLib.TSprite16)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ImageLib.TSprite16 => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { ImageLib.TRLESprite16 (instance size 96, ClassID $11): a run length encoded 16 bit sprite.
//     +$48 FColorKey (the transparent color, in FPixelFormatID), +$4C FData (FDataSize bytes),
//     +$50 FSpriteX, +$54 FSpriteY, +$58 FSpriteWidth, +$5C FSpriteHeight (as TSprite16). The data
//     is FSpriteHeight rows; a row is a word with its size in bytes (the header included), a zero
//     word, then pixels and skips (the color key and the size of the skip in bytes), padded with the
//     color key to a dword. }
    public partial class TRLESprite16 : Aow.Units.ImageLib.TImage16
    {
        internal int FColorKey;
        internal Ptr<byte> FData;
        internal int FSpriteX;
        internal int FSpriteY;
        internal int FSpriteWidth;
        internal int FSpriteHeight;
        // ImageLib.pas:2881
        // { TRLESprite16 }
        public override void Create__init(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            base.Create__init(AOwner);
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
            int W = default;
            W = (FXshift + FSpriteX) + FSpriteWidth;
            if (Value < W)
            {
                Value = W;
            }

            base.SetWidth(Value);
        }

        // ImageLib.pas:2919
        public override void SetHeight(int Value)
        {
            int W = default;
            W = (FYshift + FSpriteY) + FSpriteHeight;
            if (Value < W)
            {
                Value = W;
            }

            base.SetHeight(Value);
        }

        // ImageLib.pas:2928
        public override void GetVisualDimensions(ref Aow.Rtl.TRect R)
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
                FData = FData.Realloc(0);
                FDataLoaded = false;
            }
        }

        // ImageLib.pas:2947
        // { The last dword of the image data is not used: ReadImageData skips it and WriteImageData
//   writes an uninitialized local variable (an original bug). }
        public override void ReadImageData(Aow.Rtl.TStream Stream)
        {
            int Reserved = default;
            base.ReadImageData(Stream);
            Stream.Read(Aow.Rtl.Bytes.Of(ref FSpriteWidth, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FSpriteHeight, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FSpriteX, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FSpriteY, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FColorKey, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref Reserved, 4));
        }

        // ImageLib.pas:2960
        public override void WriteImageData(Aow.Rtl.TStream Stream)
        {
            int Reserved = default;
            base.WriteImageData(Stream);
            Stream.Write(Aow.Rtl.Bytes.Of(ref FSpriteWidth, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FSpriteHeight, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FSpriteX, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FSpriteY, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FColorKey, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref Reserved, 4));
        }

        // ImageLib.pas:2973
        public override void ReadDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            base.ReadDiscardableImageData(Stream);
            FData = FData.Realloc(FDataSize);
            Stream.Read(FData.AsSpan(FDataSize));
        }

        // ImageLib.pas:2980
        public override void WriteDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            base.WriteDiscardableImageData(Stream);
            Stream.Write(FData.AsSpan(FDataSize));
        }

        // ImageLib.pas:2986
        public override int RemapToPixelFormat(int PixelFormatID)
        {
            int Result = default;
            Result = 0;
            if (PixelFormatID != FPixelFormatID)
            {
                switch (PixelFormatID)
                {
                    case Aow.Units.GFXE.GFXE.pfRGB565:
                        Aow.Units.ImageLib.ImageLib.RemapRLE555to565(FData, FSpriteHeight, FColorKey);
                        FPixelFormatID = Aow.Units.GFXE.GFXE.pfRGB565;
                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB555:
                        Aow.Units.ImageLib.ImageLib.RemapRLE565to555(FData, FSpriteHeight, FColorKey);
                        FPixelFormatID = Aow.Units.GFXE.GFXE.pfRGB555;
                        break;
                    default:
                        Result = Aow.Units.GFXE.GFXE.GFXE_NOTSUPPORTED;
                        break;
                }
            }

            return Result;
        }

        // ImageLib.pas:3006
        public override bool SupportShowMode(Aow.Units.ImageLib.TShowMode ShowMode, int PixelFormatID)
        {
            bool Result = default;
            Result = (((PixelFormatID == 0) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB555)) && TSet<Aow.Units.ImageLib.TShowMode>.Of(Aow.Units.ImageLib.TShowMode.smOpaque, Aow.Units.ImageLib.TShowMode.smTransparent).Contains(ShowMode);
            return Result;
        }

        // ImageLib.pas:3012
        public override void ShowOpaque(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            if (!FDataLoaded)
            {
                LoadDiscardableImageData();
                if (SD.PixelFormatID == FPixelFormatID)
                {
                    Aow.Units.ImageLib.ImageLib.BltRLE16((X + FXshift) + FSpriteX, (Y + FYshift) + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                }
                else
                {
                    var call__135 = RemapToPixelFormat(SD.PixelFormatID);
                    if (call__135 != 0)
                    {
                        throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                    }

                    Aow.Units.ImageLib.ImageLib.BltRLE16((X + FXshift) + FSpriteX, (Y + FYshift) + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                }

                if (FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadFreeOnDemand)
                {
                    ReleaseDiscardableImageData();
                }
            }
            else
            {
                if (SD.PixelFormatID == FPixelFormatID)
                {
                    Aow.Units.ImageLib.ImageLib.BltRLE16((X + FXshift) + FSpriteX, (Y + FYshift) + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                }
                else
                {
                    var call__136 = RemapToPixelFormat(SD.PixelFormatID);
                    if (call__136 != 0)
                    {
                        throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                    }

                    Aow.Units.ImageLib.ImageLib.BltRLE16((X + FXshift) + FSpriteX, (Y + FYshift) + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                }
            }
        }

        // ImageLib.pas:3045
        // { ShowTransparent shows the sprite blended 50% }
        public override void ShowTransparent(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            if (!FDataLoaded)
            {
                LoadDiscardableImageData();
                bool bool__137 = (SD.PixelFormatID != FPixelFormatID);
                if (bool__137)
                {
                    var call__138 = RemapToPixelFormat(SD.PixelFormatID);
                    bool__137 = (call__138 != 0);
                }

                if (bool__137)
                {
                    throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                }

                switch (FPixelFormatID)
                {
                    case Aow.Units.GFXE.GFXE.pfRGB565:
                        Aow.Units.ImageLib.ImageLib.BlendRLE565((X + FXshift) + FSpriteX, (Y + FYshift) + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB555:
                        Aow.Units.ImageLib.ImageLib.BlendRLE555((X + FXshift) + FSpriteX, (Y + FYshift) + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                        break;
                }

                if (FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadFreeOnDemand)
                {
                    ReleaseDiscardableImageData();
                }
            }
            else
            {
                bool bool__139 = (SD.PixelFormatID != FPixelFormatID);
                if (bool__139)
                {
                    var call__140 = RemapToPixelFormat(SD.PixelFormatID);
                    bool__139 = (call__140 != 0);
                }

                if (bool__139)
                {
                    throw Aow.Rtl.Exception.Create((AnsiString)"Surface pixel format not supported!");
                }

                switch (FPixelFormatID)
                {
                    case Aow.Units.GFXE.GFXE.pfRGB565:
                        Aow.Units.ImageLib.ImageLib.BlendRLE565((X + FXshift) + FSpriteX, (Y + FYshift) + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB555:
                        Aow.Units.ImageLib.ImageLib.BlendRLE555((X + FXshift) + FSpriteX, (Y + FYshift) + FSpriteY, SD, FData, FSpriteWidth, FSpriteHeight, FColorKey);
                        break;
                }
            }
        }

        // ImageLib.pas:3073
        public override bool SupportGrabPixelFormat(int PixelFormatID)
        {
            bool Result = default;
            Result = (((PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB555)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB24)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfPAL8);
            return Result;
        }

        // ImageLib.pas:3079
        public override Aow.Rtl.TForm CreateGrabForm(Aow.Rtl.TComponent AOwner, in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.ImageLib.TImagesGrabbedEvent OnImagesGrabbed)
        {
            Aow.Rtl.TForm Result = default;
            Result = null;
            return Result;
        }

        // ImageLib.pas:3090
        // { The sprites take their transparent color from TGrabInfo +$C (declared Reserved in the core),
//   not from TransparentColor (+4, the color GetSurfaceRectangles separates the images with).
// 
//   GrabImage encodes the rows into FData, which grows in 4 KB steps; FDataSize stays 0 until the
//   end, so every word added past the first step reallocates (an original inefficiency). }
        public override bool GrabImage(in Aow.Units.GFXE.TSurfaceDesc SD, in Aow.Rtl.TRect R, Aow.Units.ImageLib.TGrabInfo GrabInfo)
        {
            bool Result = default;
            int Pos = default;
            int RowStart = default;
            int Skip = default;
            Aow.Units.GFXE.TDataSurface Surface = default;
            object Row = default;
            int Y = default;
            Aow.Units.GFXE.TSurfaceDesc LSD = default;
            Aow.Rtl.TRect GR = default;
            Aow.Rtl.TRect MR = default;
            int X = default;
            int C = default;
            void AddWord(ushort W)
            {
                if (FDataSize <= Pos)
                {
                    FData = FData.Realloc(Pos + 4096);
                }

                {
                    ushort value__141 = unchecked((ushort)W);
                    MemoryMarshal.Write<ushort>((FData + Pos).AsSpan(2), in value__141);
                }

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
                    AddWord(unchecked((ushort)FColorKey));
                    AddWord(unchecked((ushort)(Skip * 2)));
                    Skip = 0;
                }
            }

            void EndRow()
            {
                FlushSkip();
                if (FDataSize <= Pos)
                {
                    FData = FData.Realloc(Pos + 4096);
                }

                {
                    ushort value__142 = unchecked((ushort)(Pos - RowStart));
                    MemoryMarshal.Write<ushort>((FData + RowStart).AsSpan(2), in value__142);
                }

                if ((Pos & 3) != 0)
                {
                    AddWord(unchecked((ushort)FColorKey));
                }
            }

            // { Tried Self identity, receiver scope and record rebasing; private rectangle/FS code still differs. }
            Result = true;
            try
            {
                Surface = null;
                try
                {
                    if (SD.BitCount != 16)
                    {
                        var call__143 = Aow.Units.GFXE.TDataSurface.Create(R.Right - R.Left, R.Bottom - R.Top, Aow.Units.GFXE.GFXE.pfRGB565, null);
                        Surface = call__143;
                        GR.Left = 0;
                        GR.Top = 0;
                        var call__144 = Surface.GetWidth();
                        GR.Right = call__144;
                        var call__145 = Surface.GetHeight();
                        GR.Bottom = call__145;
                        Surface.Lock(ref LSD);
                        var call__146 = Surface.GetWidth();
                        var call__147 = Surface.GetHeight();
                        var call__148 = Aow.Units.GFXE.GFXE.RemapSurfacePart(SD, R.Left, R.Top, call__146, call__147, LSD, 0, 0);
                        if (call__148 != 0)
                        {
                            throw Aow.Rtl.Exception.Create((AnsiString)"Invalid PixelFormat");
                        }

                        var call__149 = Aow.Units.GFXE.GFXE.RemapPixelInfo(GrabInfo.TransparentPixel, SD.PixelFormatID, SD.Palette, LSD.PixelFormatID, null);
                        FColorKey = call__149;
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
                    var call__150 = Aow.Rtl.Rtl.ExtractFileName(GrabInfo.FileName);
                    FFileName = call__150;
                    FFileIndex = GrabInfo.FileIndex;
                    FStoredPixelFormatID = LSD.PixelFormatID;
                    FPixelFormatID = FStoredPixelFormatID;
                    Pos = 0;
                    Skip = 0;
                    Aow.Units.GFXE.GFXE.MinimizeTransparentRect(LSD, GR, FColorKey, ref MR);
                    FSpriteX = MR.Left - GR.Left;
                    FSpriteY = MR.Top - GR.Top;
                    FSpriteWidth = MR.Right - MR.Left;
                    FSpriteHeight = MR.Bottom - MR.Top;
                    {
                        Y = MR.Top;
                        int end__151 = MR.Bottom - 1;
                        for (; Y <= end__151; Y += 1)
                        {
                            Row = (Aow.Rtl.Rtl.BytePointer(LSD.Data) + (LSD.Pitch * Y)).Box();
                            StartRow();
                            {
                                X = MR.Left;
                                int end__152 = MR.Right - 1;
                                for (; X <= end__152; X += 1)
                                {
                                    C = MemoryMarshal.Read<ushort>(((Aow.Rtl.Rtl.BytePointer(Row) + X) + X).AsSpan(2));
                                    if (C != FColorKey)
                                    {
                                        FlushSkip();
                                        AddWord(unchecked((ushort)C));
                                    }
                                    else
                                    {
                                        Skip += 1;
                                    }
                                }
                            }

                            EndRow();
                        }
                    }

                    FData = FData.Realloc(Pos);
                    FDataSize = Pos;
                }
                finally
                {
                    if (Surface != null)
                    {
                        Surface.Destroy();
                    }
                }
            }
            catch (System.Exception)
            {
                ReleaseDiscardableImageData();
                Result = false;
            }

            return Result;
        }

        // ImageLib.pas:572
        public static new Aow.Units.ImageLib.TRLESprite16 Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            Aow.Units.ImageLib.TRLESprite16 o = new Aow.Units.ImageLib.TRLESprite16();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.ImageLib.TLibraryImage.Meta
        {
            // ImageLib.pas:2886
            public override int ClassID()
            {
                int Result = default;
                Result = 17;
                return Result;
            }

            // ImageLib.pas:2891
            public override Aow.Rtl.AnsiString Name()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"RLE Sprite (16bpp)";
                return Result;
            }

            // ImageLib.pas:572
            public new Aow.Units.ImageLib.TRLESprite16 Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
            {
                Aow.Units.ImageLib.TRLESprite16 o = (Aow.Units.ImageLib.TRLESprite16)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ImageLib.TRLESprite16 => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { ImageLib.TTransparentRLESprite16 (instance size 96, ClassID $12): a TRLESprite16 shown
//     transparent by default; it cannot be grabbed. }
    public partial class TTransparentRLESprite16 : Aow.Units.ImageLib.TRLESprite16
    {
        // ImageLib.pas:3592
        // { TTransparentRLESprite16 }
        public override void Create__init(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            base.Create__init(AOwner);
            FShowInfo.ShowMode = Aow.Units.ImageLib.TShowMode.smTransparent;
        }

        // ImageLib.pas:3608
        public override bool SupportGrabPixelFormat(int PixelFormatID)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // ImageLib.pas:599
        public static new Aow.Units.ImageLib.TTransparentRLESprite16 Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            Aow.Units.ImageLib.TTransparentRLESprite16 o = new Aow.Units.ImageLib.TTransparentRLESprite16();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.ImageLib.TRLESprite16.Meta
        {
            // ImageLib.pas:3598
            public override int ClassID()
            {
                int Result = default;
                Result = 18;
                return Result;
            }

            // ImageLib.pas:3603
            public override Aow.Rtl.AnsiString Name()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Transparent RLE Sprite (16bpp)";
                return Result;
            }

            // ImageLib.pas:599
            public new Aow.Units.ImageLib.TTransparentRLESprite16 Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
            {
                Aow.Units.ImageLib.TTransparentRLESprite16 o = (Aow.Units.ImageLib.TTransparentRLESprite16)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ImageLib.TTransparentRLESprite16 => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { ImageLib.TBitMaskGrabInfo (instance size 28): +$18 Inverse (grab the non-transparent
//     rectangle with MinimizeInvTransparentRect). }
    public partial class TBitMaskGrabInfo : Aow.Units.ImageLib.TGrabInfo
    {
        public bool Inverse;
        // ImageLib.pas:4638
        public override void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            base.LoadFromStream(Stream);
            Stream.Read(Aow.Rtl.Bytes.Of(ref Inverse, 1));
        }

        // ImageLib.pas:4644
        public override void SaveToStream(Aow.Rtl.TStream Stream)
        {
            base.SaveToStream(Stream);
            Stream.Write(Aow.Rtl.Bytes.Of(ref Inverse, 1));
        }

        // ImageLib.pas:4633
        // { TBitMaskGrabInfo }
        public override Aow.Units.ImageLib.TGrabInfo.Meta GrabInfoClass()
        {
            Aow.Units.ImageLib.TGrabInfo.Meta Result = default;
            Result = Aow.Units.ImageLib.TGrabInfo.Meta.Of<Aow.Units.ImageLib.TBitMaskGrabInfo>();
            return Result;
        }

        // System.pas:111
        public static new Aow.Units.ImageLib.TBitMaskGrabInfo Create()
        {
            return new Aow.Units.ImageLib.TBitMaskGrabInfo();
        }
    }

    // { ImageLib.TBitMask (instance size 88): a mask of one bit per pixel (ClassID $13), shown as
//     white pixels. +$40 FPitch (bytes per row, whole dwords), +$44 FMaskData (rows of dwords, the
//     first pixel in the high bit), +$48 FMaskLeft, +$4C FMaskTop, +$50 FMaskWidth, +$54
//     FMaskHeight (the rectangle of the mask in the data rectangle). }
    public partial class TBitMask : Aow.Units.ImageLib.TLibraryImage
    {
        internal int FPitch;
        internal Ptr<byte> FMaskData;
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
            int W = default;
            W = (FXshift + FMaskLeft) + FMaskWidth;
            if (Value < W)
            {
                Value = W;
            }

            base.SetWidth(Value);
        }

        // ImageLib.pas:4685
        public override void SetHeight(int Value)
        {
            int H = default;
            H = (FYshift + FMaskTop) + FMaskHeight;
            if (Value < H)
            {
                Value = H;
            }

            base.SetHeight(Value);
        }

        // ImageLib.pas:4694
        public override void ReadImageData(Aow.Rtl.TStream Stream)
        {
            base.ReadImageData(Stream);
            Stream.Read(Aow.Rtl.Bytes.Of(ref FMaskWidth, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FMaskHeight, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FMaskLeft, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FMaskTop, 4));
        }

        // ImageLib.pas:4703
        public override void WriteImageData(Aow.Rtl.TStream Stream)
        {
            base.WriteImageData(Stream);
            Stream.Write(Aow.Rtl.Bytes.Of(ref FMaskWidth, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FMaskHeight, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FMaskLeft, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FMaskTop, 4));
        }

        // ImageLib.pas:4721
        public override void ReleaseDiscardableImageData()
        {
            FMaskData = FMaskData.Realloc(0);
            FDataLoaded = false;
        }

        // ImageLib.pas:4727
        public override void ReadDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            base.ReadDiscardableImageData(Stream);
            AllocateMaskData(FMaskWidth, FMaskHeight);
            Stream.Read(FMaskData.AsSpan(FDataSize));
        }

        // ImageLib.pas:4734
        public override void WriteDiscardableImageData(Aow.Rtl.TStream Stream)
        {
            base.WriteDiscardableImageData(Stream);
            Stream.Write(FMaskData.AsSpan(FDataSize));
        }

        // ImageLib.pas:4740
        public override bool SupportGrabPixelFormat(int PixelFormatID)
        {
            bool Result = default;
            Result = (((PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB555)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB24)) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfPAL8);
            return Result;
        }

        // ImageLib.pas:4746
        public override Aow.Units.ImageLib.TGrabInfo CreateGrabInfo()
        {
            Aow.Units.ImageLib.TGrabInfo Result = default;
            Result = Aow.Units.ImageLib.TBitMaskGrabInfo.Create();
            return Result;
        }

        // ImageLib.pas:4751
        public override Aow.Rtl.TForm CreateGrabForm(Aow.Rtl.TComponent AOwner, in Aow.Units.GFXE.TSurfaceDesc SD, Aow.Units.ImageLib.TImagesGrabbedEvent OnImagesGrabbed)
        {
            Aow.Rtl.TForm Result = default;
            Result = null;
            return Result;
        }

        // ImageLib.pas:4759
        // { GrabImage sets a bit for every pixel that differs from the transparent pixel value. With an
//   inverse grab info no bit is added at all (an original bug: the mask stays empty). }
        public override bool GrabImage(in Aow.Units.GFXE.TSurfaceDesc SD, in Aow.Rtl.TRect R, Aow.Units.ImageLib.TGrabInfo GrabInfo)
        {
            bool Result = default;
            int Bits = default;
            int BitCount = default;
            int Offset = default;
            int X = default;
            int Y = default;
            int TransparentColor = default;
            int Left = default;
            int Top = default;
            int Bottom = default;
            Aow.Rtl.TRect MinRect = default;
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
                    {
                        int value__153 = Bits;
                        MemoryMarshal.Write<int>((FMaskData + Offset).AsSpan(4), in value__153);
                    }

                    Offset += 4;
                    BitCount = 0;
                    Bits = 0;
                }
            }

            void FlushBits()
            {
                Bits = Bits << (32 - BitCount);
                {
                    int value__154 = Bits;
                    MemoryMarshal.Write<int>((FMaskData + Offset).AsSpan(4), in value__154);
                }

                Offset += 4;
                BitCount = 0;
                Bits = 0;
            }

            // { Tried record rebasing/casts and receiver scope; minimizer argument order still differs. }
            Clear();
            Result = true;
            FDataLoaded = true;
            Bits = 0;
            Offset = 0;
            BitCount = 0;
            FDataWidth = R.Right - R.Left;
            FDataHeight = R.Bottom - R.Top;
            FWidth = FDataWidth;
            FHeight = FDataHeight;
            var call__155 = Aow.Rtl.Rtl.ExtractFileName(GrabInfo.FileName);
            FFileName = call__155;
            FFileIndex = GrabInfo.FileIndex;
            TransparentColor = GrabInfo.TransparentPixel;
            if (!((Aow.Units.ImageLib.TBitMaskGrabInfo)GrabInfo).Inverse)
            {
                Aow.Units.GFXE.GFXE.MinimizeTransparentRect(SD, R, TransparentColor, ref MinRect);
            }
            else
            {
                Aow.Units.GFXE.GFXE.MinimizeInvTransparentRect(SD, R, TransparentColor, ref MinRect);
            }

            Left = MinRect.Left;
            FMaskLeft = Left - R.Left;
            Top = MinRect.Top;
            FMaskTop = Top - R.Top;
            FMaskWidth = MinRect.Right - Left;
            Bottom = MinRect.Bottom;
            FMaskHeight = Bottom - Top;
            AllocateMaskData(FMaskWidth, FMaskHeight);
            {
                Y = Top;
                int end__156 = Bottom - 1;
                for (; Y <= end__156; Y += 1)
                {
                    Offset = (Y - MinRect.Top) * FPitch;
                    {
                        X = MinRect.Left;
                        int end__157 = MinRect.Right - 1;
                        for (; X <= end__157; X += 1)
                        {
                            if (!((Aow.Units.ImageLib.TBitMaskGrabInfo)GrabInfo).Inverse)
                            {
                                var call__158 = Aow.Units.GFXE.GFXE.GetPixelInfo(SD, X, Y);
                                AddBit(call__158 != TransparentColor);
                            }
                        }
                    }

                    FlushBits();
                }
            }

            return Result;
        }

        // ImageLib.pas:4827
        // { ShowDC clears the surface to gray, not to BkColor. }
        public override void ShowDC(Ptr<Aow.Rtl.TRect> SrcRect, Ptr<Aow.Rtl.TRect> DestRect, int DC, int BkColor)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.WindowFromDC */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:4861
        public override void ShowOpaque(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            if (!FDataLoaded)
            {
                LoadDiscardableImageData();
            }

            Aow.Units.ImageLib.ImageLib.ShowBitMask16((X + FXshift) + FMaskLeft, (Y + FYshift) + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
        }

        public new partial class Meta : Aow.Units.ImageLib.TLibraryImage.Meta
        {
            // ImageLib.pas:4652
            // { TBitMask }
            public override int ClassID()
            {
                int Result = default;
                Result = 19;
                return Result;
            }

            // ImageLib.pas:4657
            public override Aow.Rtl.AnsiString Name()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Bit Mask";
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ImageLib.TBitMask => (Meta)MetaRegistry.Of(typeof(T));
            // ImageLib.pas:271
            public new Aow.Units.ImageLib.TBitMask Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
            {
                Aow.Units.ImageLib.TBitMask o = (Aow.Units.ImageLib.TBitMask)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { ImageLib.TShadow (instance size 88): a bit mask shown as a shadow, darkening the 16 bit
//     surface under the set bits (ClassID $14). }
    public partial class TShadow : Aow.Units.ImageLib.TBitMask
    {
        // ImageLib.pas:5100
        // { TShadow }
        public override void Create__init(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            base.Create__init(AOwner);
        }

        // ImageLib.pas:5106
        // { SupportShowMode asks TLibraryImage, which supports nothing. }
        public override bool SupportShowMode(Aow.Units.ImageLib.TShowMode ShowMode, int PixelFormatID)
        {
            bool Result = default;
            Result = ((PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB565) || (PixelFormatID == Aow.Units.GFXE.GFXE.pfRGB555)) && base.SupportShowMode(ShowMode, PixelFormatID);
            return Result;
        }

        // ImageLib.pas:5122
        public override void ShowOpaque(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            if (!FDataLoaded)
            {
                LoadDiscardableImageData();
                switch (SD.PixelFormatID)
                {
                    case Aow.Units.GFXE.GFXE.pfRGB565:
                        Aow.Units.ImageLib.ImageLib.ShowShadow565((X + FXshift) + FMaskLeft, (Y + FYshift) + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB555:
                        Aow.Units.ImageLib.ImageLib.ShowShadow555((X + FXshift) + FMaskLeft, (Y + FYshift) + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
                        break;
                    default:
                        throw Aow.Rtl.Exception.Create((AnsiString)"Surface format not supported");
                        break;
                }

                if (FLoadMode == Aow.Units.ImageLib.TLoadMode.lmLoadFreeOnDemand)
                {
                    ReleaseDiscardableImageData();
                }
            }
            else
            {
                switch (SD.PixelFormatID)
                {
                    case Aow.Units.GFXE.GFXE.pfRGB565:
                        Aow.Units.ImageLib.ImageLib.ShowShadow565((X + FXshift) + FMaskLeft, (Y + FYshift) + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
                        break;
                    case Aow.Units.GFXE.GFXE.pfRGB555:
                        Aow.Units.ImageLib.ImageLib.ShowShadow555((X + FXshift) + FMaskLeft, (Y + FYshift) + FMaskTop, SD, FMaskData, FMaskWidth, FMaskHeight);
                        break;
                    default:
                        throw Aow.Rtl.Exception.Create((AnsiString)"Surface format not supported");
                        break;
                }
            }
        }

        // ImageLib.pas:653
        public static new Aow.Units.ImageLib.TShadow Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            Aow.Units.ImageLib.TShadow o = new Aow.Units.ImageLib.TShadow();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.ImageLib.TBitMask.Meta
        {
            // ImageLib.pas:5112
            public override int ClassID()
            {
                int Result = default;
                Result = 20;
                return Result;
            }

            // ImageLib.pas:5117
            public override Aow.Rtl.AnsiString Name()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = (AnsiString)"Shadow";
                return Result;
            }

            // ImageLib.pas:653
            public new Aow.Units.ImageLib.TShadow Create(Aow.Units.ImageLib.TCustomImageLibrary AOwner)
            {
                Aow.Units.ImageLib.TShadow o = (Aow.Units.ImageLib.TShadow)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ImageLib.TShadow => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { ImageLib.TImageLibraryFont (instance size 100): an image library whose image N is the
//     character with code N. +$50 FCharacterDistance (pixels between two characters), +$54
//     FHeight (the largest image height, set by LoadFromStream), +$58 FCharCase, +$5C FColor,
//     +$60 FFontColorPaletteIndex (the entry of the first palette that SetColor sets to Color, -1
//     for none). A character without an image is 5 pixels wide. }
    public partial class TImageLibraryFont : Aow.Units.ImageLib.TCustomImageLibrary
    {
        internal int FCharacterDistance;
        internal int FHeight;
        internal Aow.Rtl.TEditCharCase FCharCase;
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
        // { TImageLibraryFont }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            FCharacterDistance = 2;
            FFontColorPaletteIndex = -1;
            FCharCase = Aow.Rtl.TEditCharCase.ecNormal;
        }

        // ImageLib.pas:6291
        public override void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            int I = default;
            base.LoadFromStream(Stream);
            FHeight = 0;
            I = 0;
            while (I < FCount)
            {
                var call__159 = this[I];
                if (call__159 != null)
                {
                    var call__160 = this[I];
                    if (call__160.Height > FHeight)
                    {
                        var call__161 = this[I];
                        FHeight = call__161.Height;
                    }
                }

                I += 1;
            }
        }

        // ImageLib.pas:6367
        public int GetTextWidth(Aow.Rtl.AnsiString Text)
        {
            int Result = default;
            switch (FCharCase)
            {
                case Aow.Rtl.TEditCharCase.ecNormal:
                    Result = TextWidth(Text);
                    break;
                case Aow.Rtl.TEditCharCase.ecLowerCase:
                    Result = TextWidth(Aow.Rtl.Rtl.LowerCase(Text));
                    break;
                case Aow.Rtl.TEditCharCase.ecUpperCase:
                    Result = TextWidth(Aow.Rtl.Rtl.UpperCase(Text));
                    break;
                default:
                    Result = 0;
                    break;
            }

            return Result;
        }

        // ImageLib.pas:6378
        public int GetTextHeight(Aow.Rtl.AnsiString Text)
        {
            int Result = default;
            int I = default;
            int H = default;
            int C = default;
            Aow.Rtl.AnsiString S = default;
            switch (FCharCase)
            {
                case Aow.Rtl.TEditCharCase.ecLowerCase:
                    S = Aow.Rtl.Rtl.LowerCase(Text);
                    break;
                case Aow.Rtl.TEditCharCase.ecUpperCase:
                    S = Aow.Rtl.Rtl.UpperCase(Text);
                    break;
                default:
                    S = Text;
                    break;
            }

            H = 0;
            {
                I = 1;
                int end__162 = S.Length;
                for (; I <= end__162; I += 1)
                {
                    C = (int)S[I];
                    bool bool__163 = (C < FCount);
                    if (bool__163)
                    {
                        var call__164 = this[C];
                        bool__163 = (call__164 != null);
                    }

                    bool bool__165 = bool__163;
                    if (bool__165)
                    {
                        var call__166 = this[C];
                        bool__165 = (call__166.Height > H);
                    }

                    if (bool__165)
                    {
                        var call__167 = this[C];
                        H = call__167.Height;
                    }
                }
            }

            Result = H;
            return Result;
        }

        // ImageLib.pas:6399
        public void SetText(int X, int Y, Aow.Rtl.AnsiString Text, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            switch (FCharCase)
            {
                case Aow.Rtl.TEditCharCase.ecNormal:
                    ShowText(X, Y, Text, SD);
                    break;
                case Aow.Rtl.TEditCharCase.ecLowerCase:
                    ShowText(X, Y, Aow.Rtl.Rtl.LowerCase(Text), SD);
                    break;
                case Aow.Rtl.TEditCharCase.ecUpperCase:
                    ShowText(X, Y, Aow.Rtl.Rtl.UpperCase(Text), SD);
                    break;
            }
        }

        // ImageLib.pas:6408
        public void SetTextCentered(int X, int Y, Aow.Rtl.AnsiString Text, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Rtl.AnsiString S = default;
            switch (FCharCase)
            {
                case Aow.Rtl.TEditCharCase.ecNormal:
                    X = X - ((int)(((uint)TextWidth(Text)) >> 1));
                    ShowText(X, Y, Text, SD);
                    break;
                case Aow.Rtl.TEditCharCase.ecLowerCase:
                    S = Aow.Rtl.Rtl.LowerCase(Text);
                    X = X - ((int)(((uint)TextWidth(S)) >> 1));
                    ShowText(X, Y, S, SD);
                    break;
                case Aow.Rtl.TEditCharCase.ecUpperCase:
                    S = Aow.Rtl.Rtl.UpperCase(Text);
                    X = X - ((int)(((uint)TextWidth(S)) >> 1));
                    ShowText(X, Y, S, SD);
                    break;
            }
        }

        // ImageLib.pas:6433
        public void SetTextEx(int X, int Y, Aow.Rtl.AnsiString Text, in Aow.Units.GFXE.TSurfaceDesc SD, TSet<Aow.Units.ImageLib.TSetTextModeElement> Mode, in Aow.Units.ImageLib.TShowInfo ShowInfo)
        {
            int I = default;
            int C = default;
            Aow.Rtl.AnsiString S = default;
            switch (FCharCase)
            {
                case Aow.Rtl.TEditCharCase.ecLowerCase:
                    S = Aow.Rtl.Rtl.LowerCase(Text);
                    break;
                case Aow.Rtl.TEditCharCase.ecUpperCase:
                    S = Aow.Rtl.Rtl.UpperCase(Text);
                    break;
                default:
                    S = Text;
                    break;
            }

            if (Mode.Contains(Aow.Units.ImageLib.TSetTextModeElement.stmCenter))
            {
                X = X - ((int)(((uint)TextWidth(S)) >> 1));
            }

            {
                I = 1;
                int end__168 = S.Length;
                for (; I <= end__168; I += 1)
                {
                    C = (int)S[I];
                    bool bool__169 = (C < FCount);
                    if (bool__169)
                    {
                        var call__170 = this[C];
                        bool__169 = (call__170 != null);
                    }

                    if (bool__169)
                    {
                        var call__171 = this[C];
                        call__171.ShowEx(X, Y, SD, ShowInfo);
                        var call__172 = this[C];
                        X += call__172.Width + FCharacterDistance;
                    }
                    else
                    {
                        X += FCharacterDistance + 5;
                    }
                }
            }
        }

        public int Height { get => FHeight; }

        [Published]
        public int CharacterDistance { get => FCharacterDistance; set => FCharacterDistance = value; }

        [Published]
        public Aow.Rtl.TEditCharCase CharCase { get => FCharCase; set => FCharCase = value; }

        [Published]
        public int Color { get => FColor; set => SetColor(value); }

        [Published]
        public int FontColorPaletteIndex { get => FFontColorPaletteIndex; set => SetFontColorPaletteIndex(value); }

        // ImageLib.pas:681
        public static new Aow.Units.ImageLib.TImageLibraryFont Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.ImageLib.TImageLibraryFont o = new Aow.Units.ImageLib.TImageLibraryFont();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Splits Text into lines no wider than MaxWidth pixels in Font, at spaces and line breaks, and
//   adds them to Strings. }
    // { ImageLib.TCustomImageLibraryPanel (instance size 340): a panel that shows one image of an
//     image library with BltLibraryImage. +$138 FScaleX, +$13C FScaleY (percent), +$140 FBltMode,
//     +$144 FOnUpdated (called after every Paint), +$14C FImageLibrary, +$150 FImageIndex. }
    public partial class TCustomImageLibraryPanel : Aow.Rtl.TCustomPanel
    {
        internal int FScaleX;
        internal int FScaleY;
        internal Aow.Units.ImageLib.TBltMode FBltMode;
        internal Aow.Rtl.TNotifyEvent FOnUpdated;
        internal Aow.Units.ImageLib.TCustomImageLibrary FImageLibrary;
        internal int FImageIndex;
        // ImageLib.pas:6513
        internal void SetBltMode(Aow.Units.ImageLib.TBltMode Value)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6519
        internal void SetScaleX(int Value)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6525
        internal void SetScaleY(int Value)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6531
        internal void SetImageIndex(int Value)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6537
        internal void SetImageLibrary(Aow.Units.ImageLib.TCustomImageLibrary Value)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6543
        protected internal override void Paint()
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Canvas */
            throw new System.NotImplementedException();
        }

        protected internal Aow.Units.ImageLib.TCustomImageLibrary ImageLibrary { get => FImageLibrary; set => SetImageLibrary(value); }
        protected internal int ImageIndex { get => FImageIndex; set => SetImageIndex(value); }
        protected internal Aow.Units.ImageLib.TBltMode BltMode { get => FBltMode; set => SetBltMode(value); }
        protected internal Aow.Rtl.TNotifyEvent OnUpdated { get => FOnUpdated; set => FOnUpdated = value; }
        protected internal int ScaleX { get => FScaleX; set => SetScaleX(value); }
        protected internal int ScaleY { get => FScaleY; set => SetScaleY(value); }

        // ImageLib.pas:6504
        // { TCustomImageLibraryPanel }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            /* TODO(T-shim) Platform or editor boundary ExtCtrls.Create */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:727
        public static Aow.Units.ImageLib.TCustomImageLibraryPanel Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.ImageLib.TCustomImageLibraryPanel o = new Aow.Units.ImageLib.TCustomImageLibraryPanel();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { ImageLib.TImageLibraryPanel (instance size 340) }
    public partial class TImageLibraryPanel : Aow.Units.ImageLib.TCustomImageLibraryPanel
    {
    }

    public enum TILGrowDirection : byte
    {
        ilNone = 0,
        ilHorizontal = 1,
        ilVertical = 2
    }

    // { ImageLib.TCustomImageDrawGrid (instance size 476): a grid of Count image cells. +$1A0
//     FScaleX, +$1A4 FScaleY, +$1A8 FBltMode, +$1A9 FEditMode, +$1AC FOnCellUpdated (draws a cell
//     at run time), +$1B4 FOnSelectCell, +$1BC FFocusColor, +$1C0 FGrowDirection, +$1C1
//     FSelectionEnabled, +$1C4 FMinRowCount, +$1C8 FMinColCount, +$1CC FMaxRowCount, +$1D0
//     FMaxColCount (0 for no limit), +$1D4 FShowGridIndex, +$1D8 FCount. Ctrl+X/C/V/Del call the
//     edit methods, Ins and Del without a modifier InsCell and DelCell; they do nothing here. }
    public partial class TCustomImageDrawGrid : Aow.Rtl.TCustomGrid
    {
        internal int FScaleX;
        internal int FScaleY;
        internal Aow.Units.ImageLib.TBltMode FBltMode;
        internal TSet<Aow.Units.ImageLib.TILGridEditModeElement> FEditMode;
        internal Aow.Rtl.TDrawCellEvent FOnCellUpdated;
        internal Aow.Rtl.TSelectCellEvent FOnSelectCell;
        internal int FFocusColor;
        internal Aow.Units.ImageLib.TILGrowDirection FGrowDirection;
        internal bool FSelectionEnabled;
        internal int FMinRowCount;
        internal int FMinColCount;
        internal int FMaxRowCount;
        internal int FMaxColCount;
        internal bool FShowGridIndex;
        internal int FCount;
        // ImageLib.pas:6705
        internal void SetBltMode(Aow.Units.ImageLib.TBltMode Value)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6711
        internal void SetSelectionEnabled(bool Value)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6717
        internal void SetFocusColor(int Value)
        {
            FFocusColor = Value;
        }

        // ImageLib.pas:6722
        internal void SetScaleX(int Value)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6728
        internal void SetScaleY(int Value)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6734
        internal void SetShowGridIndex(bool Value)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6740
        internal int GetColCount()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.ColCount */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6745
        internal new void SetColCount(int Value)
        {
            /* TODO(T-shim) Platform or editor boundary Grids.ColCount */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6752
        internal void SetMinRowCount(int Value)
        {
            FMinRowCount = Value;
            var call__173 = RowCount;
            ;
        }

        // ImageLib.pas:6758
        internal void SetMinColCount(int Value)
        {
            FMinColCount = Value;
            var call__174 = ColCount;
            ;
        }

        // ImageLib.pas:6764
        internal void SetMaxRowCount(int Value)
        {
            FMaxRowCount = Value;
            var call__175 = RowCount;
            ;
        }

        // ImageLib.pas:6770
        internal void SetMaxColCount(int Value)
        {
            FMaxColCount = Value;
            var call__176 = ColCount;
            ;
        }

        // ImageLib.pas:6776
        internal int GetRowCount()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.RowCount */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6781
        internal new void SetRowCount(int Value)
        {
            /* TODO(T-shim) Platform or editor boundary Grids.RowCount */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6788
        protected internal override void DrawCell(int ACol, int ARow, Aow.Rtl.TRect ARect, TSet<Aow.Rtl.TGridDrawState> AState)
        {
            /* TODO(T-shim) Platform or editor boundary Graphics.Color */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6613
        protected internal override bool SelectCell(int ACol, int ARow)
        {
            bool Result = default;
            Result = true;
            if (FOnSelectCell != null)
            {
                FOnSelectCell(this, ACol, ARow, ref Result);
            }

            FSelectionEnabled = true;
            return Result;
        }

        // ImageLib.pas:6620
        protected internal override void KeyDown(ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            /* TODO(T-shim) Platform or editor boundary Grids.KeyDown */
            throw new System.NotImplementedException();
        }

        protected internal bool SelectionEnabled { get => FSelectionEnabled; set => SetSelectionEnabled(value); }
        protected internal int FocusColor { get => FFocusColor; set => SetFocusColor(value); }
        protected internal Aow.Units.ImageLib.TBltMode BltMode { get => FBltMode; set => SetBltMode(value); }
        protected internal Aow.Rtl.TDrawCellEvent OnCellUpdated { get => FOnCellUpdated; set => FOnCellUpdated = value; }
        protected internal Aow.Rtl.TSelectCellEvent OnSelectCell { get => FOnSelectCell; set => FOnSelectCell = value; }
        protected internal int ScaleX { get => FScaleX; set => SetScaleX(value); }
        protected internal int ScaleY { get => FScaleY; set => SetScaleY(value); }
        protected internal new int ColCount { get => GetColCount(); set => SetColCount(value); }
        protected internal new int RowCount { get => GetRowCount(); set => SetRowCount(value); }
        protected internal TSet<Aow.Units.ImageLib.TILGridEditModeElement> EditMode { get => FEditMode; set => FEditMode = value; }
        protected internal Aow.Units.ImageLib.TILGrowDirection GrowDirection { get => FGrowDirection; set => FGrowDirection = value; }
        protected internal int MinRowCount { get => FMinRowCount; set => SetMinRowCount(value); }
        protected internal int MinColCount { get => FMinColCount; set => SetMinColCount(value); }
        protected internal int MaxRowCount { get => FMaxRowCount; set => SetMaxRowCount(value); }
        protected internal int MaxColCount { get => FMaxColCount; set => SetMaxColCount(value); }
        protected internal bool ShowGridIndex { get => FShowGridIndex; set => SetShowGridIndex(value); }

        // ImageLib.pas:6594
        // { TCustomImageDrawGrid }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Create */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6661
        public virtual void SetCount(int Value)
        {
            // { Tried receiver cast, identity and with scope; extra Self save remains. }
            FCount = Value;
            UpdateDimensions();
        }

        // ImageLib.pas:6668
        public virtual void UpdateDimensions()
        {
            switch (FGrowDirection)
            {
                case Aow.Units.ImageLib.TILGrowDirection.ilHorizontal:
                    var call__177 = RowCount;
                    var call__178 = RowCount;
                    ColCount = (call__177 + FCount) / call__178;
                    break;
                case Aow.Units.ImageLib.TILGrowDirection.ilVertical:
                    var call__179 = ColCount;
                    var call__180 = ColCount;
                    RowCount = (call__179 + FCount) / call__180;
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
        public virtual int ImageIndex(int ACol, int ARow)
        {
            int Result = default;
            var call__181 = ColCount;
            Result = (call__181 * ARow) + ACol;
            return Result;
        }

        // ImageLib.pas:6681
        public virtual void IndexToCell(int Index, ref int ACol, ref int ARow)
        {
            var call__182 = ColCount;
            ACol = Index % call__182;
            var call__183 = ColCount;
            ARow = Index / call__183;
        }

        // ImageLib.pas:6687
        public void SelectIndex(int Index)
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Left */
            throw new System.NotImplementedException();
        }

        public int Count { get => FCount; set => SetCount(value); }

        // ImageLib.pas:808
        public static Aow.Units.ImageLib.TCustomImageDrawGrid Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.ImageLib.TCustomImageDrawGrid o = new Aow.Units.ImageLib.TCustomImageDrawGrid();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { AoWEd 0042D13C }
    // { ImageLib.TImageDrawGrid (instance size 476) }
    public partial class TImageDrawGrid : Aow.Units.ImageLib.TCustomImageDrawGrid
    {
    }

    // { ImageLib.TCustomImageLibraryGrid (instance size 480): a grid of the images of an image
//     library, +$1DC FImageLibrary. The edit methods work on the selected cells, through the
//     clipboard format of TCustomImageLibrary, when EditMode allows them. }
    public partial class TCustomImageLibraryGrid : Aow.Units.ImageLib.TCustomImageDrawGrid
    {
        internal Aow.Units.ImageLib.TCustomImageLibrary FImageLibrary;
        // ImageLib.pas:6934
        internal void SetImageLibrary(Aow.Units.ImageLib.TCustomImageLibrary Value)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Invalidate */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6942
        protected internal override void DrawCell(int ACol, int ARow, Aow.Rtl.TRect ARect, TSet<Aow.Rtl.TGridDrawState> AState)
        {
            /* TODO(T-shim) Platform or editor boundary Controls.Canvas */
            throw new System.NotImplementedException();
        }

        protected internal Aow.Units.ImageLib.TCustomImageLibrary ImageLibrary { get => FImageLibrary; set => SetImageLibrary(value); }

        // ImageLib.pas:6818
        // { TCustomImageLibraryGrid }
        public Aow.Units.ImageLib.TCustomImageLibrary CreateSelectedIL()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Top */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6834
        public override void InsCell()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Col */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6843
        public override void DelCell()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Col */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6852
        public override void CutCell()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Top */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6877
        public override void CopyCell()
        {
            Aow.Units.ImageLib.TCustomImageLibrary L = default;
            if (FEditMode.Contains(Aow.Units.ImageLib.TILGridEditModeElement.igCopy) && (FImageLibrary != null))
            {
                L = CreateSelectedIL();
                try
                {
                    L.CopyToClipboard();
                }
                finally
                {
                    L.Free();
                }
            }
        }

        // ImageLib.pas:6892
        public override void DeleteCell()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Top */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6907
        public override void PasteCell()
        {
            /* TODO(T-shim) Platform or editor boundary Grids.Top */
            throw new System.NotImplementedException();
        }
    }

    // { ImageLib.TImageLibraryGrid (instance size 480) }
    public partial class TImageLibraryGrid : Aow.Units.ImageLib.TCustomImageLibraryGrid
    {
    }

    // { ImageLib.TImageLibraryManager (instance size 48): the libraries a program shares, linked by
//     file name. +$24 FSearchPath (upper case, ends with a backslash), +$28 FDiskList, +$2C
//     FOwnedList. }
    public partial class TImageLibraryManager : Aow.Rtl.TComponent
    {
        internal Aow.Rtl.AnsiString FSearchPath;
        internal Aow.Units.ImageLib.TILManagerList FDiskList;
        internal Aow.Units.ImageLib.TILManagerList FOwnedList;
        // ImageLib.pas:8260
        // { TImageLibraryManager }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            var call__184 = Aow.Units.ImageLib.TILManagerList.Create(this);
            FDiskList = call__184;
            var call__185 = Aow.Units.ImageLib.TILManagerList.Create(this);
            FOwnedList = call__185;
        }

        // ImageLib.pas:8267
        public override void Destroy()
        {
            Clear();
            FDiskList.Free();
            FOwnedList.Free();
            base.Destroy();
        }

        // ImageLib.pas:8275
        public void Clear()
        {
            FDiskList.Clear();
            FOwnedList.Clear();
        }

        // ImageLib.pas:8281
        public void UnloadIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement> Category)
        {
            if (Category.Contains(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk))
            {
                FDiskList.UnloadIL();
            }

            if (Category.Contains(Aow.Units.ImageLib.TILManagerCategoryElement.icOwned))
            {
                FOwnedList.UnloadIL();
            }
        }

        // ImageLib.pas:8288
        // { Lists the .ILB files of the search path and of its subdirectories in FDiskList }
        public void ExecutePathSearch(Aow.Rtl.AnsiString Path)
        {
            Aow.Rtl.AnsiString Dir = default;
            void SearchDirectory(Aow.Rtl.AnsiString SubDirectory)
            {
                Aow.Rtl.TStringList List = default;
                Aow.Rtl.TSearchRec SR = default;
                bool Found = default;
                int I = default;
                var call__186 = Aow.Rtl.Rtl.FindFirst((FSearchPath + SubDirectory) + ((AnsiString)"*.ilb"), 63 - 16, ref SR);
                Found = call__186 == 0;
                while (Found)
                {
                    FDiskList.AddILFile(SubDirectory + Aow.Rtl.Rtl.UpperCase(SR.Name));
                    Found = Aow.Rtl.Rtl.FindNext(ref SR) == 0;
                }

                Aow.Rtl.Rtl.FindClose(ref SR);
                List = new Aow.Rtl.TStringList();
                Found = Aow.Rtl.Rtl.FindFirst(SubDirectory + ((AnsiString)"*.*"), 16, ref SR) == 0;
                while (Found)
                {
                    if ((((SR.Attr & 16) != 0) && (SR.Name != AnsiString.FromBytes(46))) && (SR.Name != ((AnsiString)"..")))
                    {
                        List.Add((SubDirectory + Aow.Rtl.Rtl.UpperCase(SR.Name)) + AnsiString.FromBytes(92));
                    }

                    Found = Aow.Rtl.Rtl.FindNext(ref SR) == 0;
                }

                Aow.Rtl.Rtl.FindClose(ref SR);
                {
                    I = 0;
                    int end__187 = List.Count - 1;
                    for (; I <= end__187; I += 1)
                    {
                        var call__188 = List[I];
                        SearchDirectory(call__188);
                    }
                }

                List.Free();
            }

            FDiskList.Clear();
            FSearchPath = Aow.Rtl.Rtl.UpperCase(Path);
            if (FSearchPath[FSearchPath.Length] != 92)
            {
                FSearchPath = FSearchPath + AnsiString.FromBytes(92);
            }

            Dir = Aow.Rtl.Rtl.GetCurrentDir();
            Aow.Rtl.Rtl.SetCurrentDir(Path);
            SearchDirectory((AnsiString)"");
            Aow.Rtl.Rtl.SetCurrentDir(Dir);
        }

        // ImageLib.pas:8331
        // { A missing owned library is created empty and added. The exception for a missing library is
//   raised and swallowed at once (it only shows in the debugger), as in the original. }
        public Aow.Units.ImageLib.TCustomImageLibrary LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement> Category, Aow.Rtl.AnsiString FileName)
        {
            Aow.Units.ImageLib.TCustomImageLibrary Result = default;
            Result = null;
            if (Category.Contains(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk))
            {
                var call__189 = FDiskList.LinkToIL(FileName);
                Result = call__189;
            }

            if (Category.Contains(Aow.Units.ImageLib.TILManagerCategoryElement.icOwned) && (Result == null))
            {
                var call__190 = FOwnedList.LinkToIL(FileName);
                Result = call__190;
                if (Result == null)
                {
                    Result = Aow.Units.ImageLib.TImageLibrary.Create(null);
                    FOwnedList.AddILManagerObject(FileName, Result);
                }
            }

            if (Result == null)
            {
                try
                {
                    throw Aow.Rtl.Exception.Create(((AnsiString)"Missing IL Resource: ") + FileName);
                }
                catch (System.Exception)
                {
                }
            }

            return Result;
        }

        // ImageLib.pas:8352
        public void ReleaseIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement> Category, Aow.Rtl.AnsiString FileName)
        {
            bool Released = default;
            Released = false;
            if (Category.Contains(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk))
            {
                var call__191 = FDiskList.ReleaseIL(FileName);
                Released = call__191;
            }

            if (Category.Contains(Aow.Units.ImageLib.TILManagerCategoryElement.icOwned) && !Released)
            {
                FOwnedList.ReleaseIL(FileName);
            }
        }

        // ImageLib.pas:8361
        public void SaveToStream(Aow.Rtl.TStream Stream)
        {
            FOwnedList.SaveToStream(Stream);
        }

        // ImageLib.pas:8366
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            FOwnedList.LoadFromStream(Stream);
        }

        public Aow.Units.ImageLib.TILManagerList DiskList { get => FDiskList; }
        public Aow.Units.ImageLib.TILManagerList OwnedList { get => FOwnedList; }

        [Published]
        public Aow.Rtl.AnsiString SearchPath { get => FSearchPath; set => FSearchPath = value; }

        // ImageLib.pas:953
        public static Aow.Units.ImageLib.TImageLibraryManager Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.ImageLib.TImageLibraryManager o = new Aow.Units.ImageLib.TImageLibraryManager();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { ImageLib.TILManagerObject (instance size 12): an entry of a TILManagerList. +4 FRefCount
//     (the LinkToIL calls not yet released), +8 FImageLibrary (loaded by the first LinkToIL). }
    public partial class TILManagerObject : Aow.Rtl.TObject
    {
        internal int FRefCount;
        internal Aow.Units.ImageLib.TCustomImageLibrary FImageLibrary;
        public int RefCount { get => FRefCount; }
        public Aow.Units.ImageLib.TCustomImageLibrary ImageLibrary { get => FImageLibrary; }

        // System.pas:111
        public static Aow.Units.ImageLib.TILManagerObject Create()
        {
            return new Aow.Units.ImageLib.TILManagerObject();
        }
    }

    // { ImageLib.TILManagerList (instance size 52): library file names (upper case, relative to the
//     search path) with their TILManagerObject. +$2C FHashTable (the objects by name), +$30
//     FOwner. }
    public partial class TILManagerList : Aow.Rtl.TStringList
    {
        internal Aow.Units.HashTabU.TStringHashTable256 FHashTable;
        internal Aow.Units.ImageLib.TImageLibraryManager FOwner;
        // ImageLib.pas:8093
        // { TILManagerList }
        public void Create__init_TILManagerList(Aow.Units.ImageLib.TImageLibraryManager AOwner)
        {
            ;
            FOwner = AOwner;
            var call__192 = Aow.Units.HashTabU.TStringHashTable256.Create();
            FHashTable = call__192;
        }

        // ImageLib.pas:8102
        // { Original bug: the inherited destructor does not call Clear, so the TILManagerObjects and
//   their libraries are never freed. }
        public override void Destroy()
        {
            base.Destroy();
            FHashTable.Free();
        }

        // ImageLib.pas:8108
        public Aow.Units.ImageLib.TCustomImageLibrary GetIL(int Index)
        {
            Aow.Units.ImageLib.TCustomImageLibrary Result = default;
            var call__193 = Objects[Index];
            Result = ((Aow.Units.ImageLib.TILManagerObject)call__193).FImageLibrary;
            return Result;
        }

        // ImageLib.pas:8113
        public void UnloadIL()
        {
            int I = default;
            Aow.Units.ImageLib.TILManagerObject Obj = default;
            {
                I = Count - 1;
                byte end__194 = 0;
                for (; I >= end__194; I -= 1)
                {
                    var call__195 = Objects[I];
                    Obj = (Aow.Units.ImageLib.TILManagerObject)call__195;
                    if ((Obj != null) && (Obj.FImageLibrary != null))
                    {
                        Obj.FImageLibrary.ReleaseDiscardableImageData();
                    }
                }
            }
        }

        // ImageLib.pas:8126
        public Aow.Units.ImageLib.TILManagerObject AddILFile(Aow.Rtl.AnsiString FileName)
        {
            Aow.Units.ImageLib.TILManagerObject Result = default;
            Aow.Units.ImageLib.TILManagerObject Obj = default;
            Obj = Aow.Units.ImageLib.TILManagerObject.Create();
            FileName = Aow.Rtl.Rtl.UpperCase(FileName);
            AddObject(FileName, Obj);
            FHashTable.Add(Obj, FileName);
            Result = Obj;
            return Result;
        }

        // ImageLib.pas:8139
        // { Original bug: the object is deleted from the hash table instead of added, so LinkToIL never
//   finds the owned libraries. }
        public Aow.Units.ImageLib.TILManagerObject AddILManagerObject(Aow.Rtl.AnsiString Name, Aow.Units.ImageLib.TCustomImageLibrary ImageLibrary)
        {
            Aow.Units.ImageLib.TILManagerObject Result = default;
            Aow.Rtl.AnsiString S = default;
            Aow.Units.ImageLib.TILManagerObject Obj = default;
            S = Aow.Rtl.Rtl.UpperCase(Name);
            Obj = Aow.Units.ImageLib.TILManagerObject.Create();
            Obj.FImageLibrary = ImageLibrary;
            AddObject(S, Obj);
            FHashTable.Delete(S);
            Result = Obj;
            return Result;
        }

        // ImageLib.pas:8153
        public override void Delete(int Index)
        {
            Aow.Units.ImageLib.TILManagerObject Obj = default;
            var call__196 = Objects[Index];
            Obj = (Aow.Units.ImageLib.TILManagerObject)call__196;
            if (Obj != null)
            {
                Obj.FImageLibrary.Free();
                Obj.Free();
            }

            var call__197 = Strings[Index];
            FHashTable.Delete(call__197);
            base.Delete(Index);
        }

        // ImageLib.pas:8167
        public override void Clear()
        {
            int I = default;
            Aow.Units.ImageLib.TILManagerObject Obj = default;
            FHashTable.Clear();
            {
                I = Count - 1;
                byte end__198 = 0;
                for (; I >= end__198; I -= 1)
                {
                    var call__199 = Objects[I];
                    Obj = (Aow.Units.ImageLib.TILManagerObject)call__199;
                    if (Obj != null)
                    {
                        Obj.FImageLibrary.Free();
                        Obj.Free();
                    }
                }
            }

            base.Clear();
        }

        // ImageLib.pas:8187
        // { Links to the library of FileName (relative to the search path), loading it on the first link.
//   The original clears FileName at the end (a second @LStrClr of the parameter). }
        public Aow.Units.ImageLib.TCustomImageLibrary LinkToIL(Aow.Rtl.AnsiString FileName)
        {
            Aow.Units.ImageLib.TCustomImageLibrary Result = default;
            Aow.Units.ImageLib.TILManagerObject Obj = default;
            FileName = Aow.Rtl.Rtl.UpperCase(FileName);
            var call__200 = FHashTable.Find(FileName);
            Obj = (Aow.Units.ImageLib.TILManagerObject)call__200;
            if (Obj != null)
            {
                Obj.FRefCount += 1;
                if (Obj.FImageLibrary == null)
                {
                    var call__201 = Aow.Units.ImageLib.TImageLibrary.Create(null);
                    Obj.FImageLibrary = call__201;
                    Obj.FImageLibrary.LoadOnDemand = true;
                    try
                    {
                        Obj.FImageLibrary.LoadFromFile(FOwner.FSearchPath + FileName);
                    }
                    catch (System.Exception)
                    {
                        var call__202 = Aow.Rtl.Exception.Create((((AnsiString)"Error loading: ") + FOwner.FSearchPath) + FileName);
                        throw call__202;
                    }
                }

                Result = Obj.FImageLibrary;
            }
            else
            {
                Result = null;
            }

            FileName = (AnsiString)"";
            return Result;
        }

        // ImageLib.pas:8213
        public bool ReleaseIL(Aow.Rtl.AnsiString FileName)
        {
            bool Result = default;
            Aow.Units.ImageLib.TILManagerObject Obj = default;
            FileName = Aow.Rtl.Rtl.UpperCase(FileName);
            var call__203 = FHashTable.Find(FileName);
            Obj = (Aow.Units.ImageLib.TILManagerObject)call__203;
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
        public override void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            int I = default;
            int N = default;
            Aow.Rtl.AnsiString Name = default;
            Aow.Units.ImageLib.TCustomImageLibrary IL = default;
            Stream.Read(Aow.Rtl.Bytes.Of(ref N, 4));
            {
                I = 0;
                int end__204 = N - 1;
                for (; I <= end__204; I += 1)
                {
                    Aow.Units.ImageLib.ImageLib.ReadStringFromStream(ref Name, Stream);
                    IL = Aow.Units.ImageLib.TImageLibrary.Create(null);
                    IL.LoadFromStream(Stream);
                    AddILManagerObject(Name, IL);
                }
            }
        }

        // ImageLib.pas:8245
        public override void SaveToStream(Aow.Rtl.TStream Stream)
        {
            int I = default;
            var call__205 = Count;
            I = call__205;
            Stream.Write(Aow.Rtl.Bytes.Of(ref I, 4));
            {
                I = 0;
                int end__206 = Count - 1;
                for (; I <= end__206; I += 1)
                {
                    var call__207 = Strings[I];
                    Aow.Units.ImageLib.ImageLib.WriteStringToStream(call__207, Stream);
                    var call__208 = Objects[I];
                    ((Aow.Units.ImageLib.TILManagerObject)call__208).FImageLibrary.SaveToStream(Stream);
                }
            }
        }

        // ImageLib.pas:930
        public static Aow.Units.ImageLib.TILManagerList Create(Aow.Units.ImageLib.TImageLibraryManager AOwner)
        {
            Aow.Units.ImageLib.TILManagerList o = new Aow.Units.ImageLib.TILManagerList();
            try
            {
                o.Create__init_TILManagerList(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { ImageLib.TILManagerComboBox (instance size 388): a combo box with the library names of a
//     manager. +$170 FILManager, +$174 FCategory, +$178 FSubDirectory (only the names in it are
//     listed, without it), +$17C FSkipDirectories (separated by semicolons), +$180 FEmptyILName
//     (the first item, for no library). }
    public partial class TILManagerComboBox : Aow.Rtl.TComboBox
    {
        internal Aow.Units.ImageLib.TImageLibraryManager FILManager;
        internal TSet<Aow.Units.ImageLib.TILManagerCategoryElement> FCategory;
        internal Aow.Rtl.AnsiString FSubDirectory;
        internal Aow.Rtl.AnsiString FSkipDirectories;
        internal Aow.Rtl.AnsiString FEmptyILName;
        // ImageLib.pas:8373
        // { TILManagerComboBox }
        internal void SetILManager(Aow.Units.ImageLib.TImageLibraryManager Value)
        {
            FILManager = Value;
            GenerateItems();
        }

        // ImageLib.pas:8379
        internal void SetSubDirectory(Aow.Rtl.AnsiString Value)
        {
            FSubDirectory = Aow.Rtl.Rtl.UpperCase(Value);
            if ((Value != ((AnsiString)"")) && (FSubDirectory[FSubDirectory.Length] != 92))
            {
                FSubDirectory = FSubDirectory + AnsiString.FromBytes(92);
            }
        }

        // ImageLib.pas:8393
        internal Aow.Rtl.AnsiString GetImageLibraryName()
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.ItemIndex */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:8401
        internal void SetImageLibraryName(Aow.Rtl.AnsiString Value)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Items */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:8386
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Create */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:8414
        public void GenerateItems()
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Items */
            throw new System.NotImplementedException();
        }

        public Aow.Rtl.AnsiString ImageLibraryName { get => GetImageLibraryName(); set => SetImageLibraryName(value); }

        [Published]
        public Aow.Units.ImageLib.TImageLibraryManager ILManager { get => FILManager; set => SetILManager(value); }

        [Published]
        public TSet<Aow.Units.ImageLib.TILManagerCategoryElement> Category { get => FCategory; set => FCategory = value; }

        [Published]
        public Aow.Rtl.AnsiString SubDirectory { get => FSubDirectory; set => SetSubDirectory(value); }

        [Published]
        public Aow.Rtl.AnsiString SkipDirectories { get => FSkipDirectories; set => FSkipDirectories = value; }

        [Published]
        public Aow.Rtl.AnsiString EmptyILName { get => FEmptyILName; set => FEmptyILName = value; }

        // ImageLib.pas:984
        public static Aow.Units.ImageLib.TILManagerComboBox Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.ImageLib.TILManagerComboBox o = new Aow.Units.ImageLib.TILManagerComboBox();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { ImageLib.TILManagerListBox (instance size 360): the list box version of TILManagerComboBox.
//     +$154 FILManager, +$158 FCategory, +$15C FSubDirectory, +$160 FSkipDirectories, +$164
//     FEmptyILName. }
    public partial class TILManagerListBox : Aow.Rtl.TListBox
    {
        internal Aow.Units.ImageLib.TImageLibraryManager FILManager;
        internal TSet<Aow.Units.ImageLib.TILManagerCategoryElement> FCategory;
        internal Aow.Rtl.AnsiString FSubDirectory;
        internal Aow.Rtl.AnsiString FSkipDirectories;
        internal Aow.Rtl.AnsiString FEmptyILName;
        // ImageLib.pas:8463
        // { TILManagerListBox }
        internal void SetILManager(Aow.Units.ImageLib.TImageLibraryManager Value)
        {
            FILManager = Value;
            GenerateItems();
        }

        // ImageLib.pas:8469
        internal void SetSubDirectory(Aow.Rtl.AnsiString Value)
        {
            FSubDirectory = Aow.Rtl.Rtl.UpperCase(Value);
            if ((Value != ((AnsiString)"")) && (FSubDirectory[FSubDirectory.Length] != 92))
            {
                FSubDirectory = FSubDirectory + AnsiString.FromBytes(92);
            }
        }

        // ImageLib.pas:8483
        internal Aow.Rtl.AnsiString GetImageLibraryName()
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.ItemIndex */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:8491
        internal void SetImageLibraryName(Aow.Rtl.AnsiString Value)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Items */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:8476
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Create */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:8504
        public void GenerateItems()
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Items */
            throw new System.NotImplementedException();
        }

        public Aow.Rtl.AnsiString ImageLibraryName { get => GetImageLibraryName(); set => SetImageLibraryName(value); }

        [Published]
        public Aow.Units.ImageLib.TImageLibraryManager ILManager { get => FILManager; set => SetILManager(value); }

        [Published]
        public TSet<Aow.Units.ImageLib.TILManagerCategoryElement> Category { get => FCategory; set => FCategory = value; }

        [Published]
        public Aow.Rtl.AnsiString SubDirectory { get => FSubDirectory; set => SetSubDirectory(value); }

        [Published]
        public Aow.Rtl.AnsiString SkipDirectories { get => FSkipDirectories; set => FSkipDirectories = value; }

        [Published]
        public Aow.Rtl.AnsiString EmptyILName { get => FEmptyILName; set => FEmptyILName = value; }

        // ImageLib.pas:1010
        public static Aow.Units.ImageLib.TILManagerListBox Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.ImageLib.TILManagerListBox o = new Aow.Units.ImageLib.TILManagerListBox();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public enum TSequenceMode : byte
    {
        smNone = 0,
        smUp = 1,
        smUpDown = 2,
        smRandom = 3,
        smFrameTable = 4,
        smRndFrameTable = 5
    }

    // { ImageLib.TImageSequenceList (instance size 28): the image sequences of the indices, each the
//     first layer of a chain (nil for none). +$10 FLinked (the sequences are linked to their
//     libraries), +$14 FILManager, +$18 FImageLibrary (the library of the sequences without a
//     library name). }
    public partial class TImageSequenceList : Aow.Rtl.TList
    {
        internal bool FLinked;
        // { HSEngine's TMapObjectResource.MsgProc writes FILManager directly }
        public Aow.Units.ImageLib.TImageLibraryManager FILManager;
        internal Aow.Units.ImageLib.TCustomImageLibrary FImageLibrary;
        // ImageLib.pas:8903
        // { TImageSequenceList }
        public void Create__init_TImageSequenceList(Aow.Units.ImageLib.TImageLibraryManager AILManager, Aow.Units.ImageLib.TCustomImageLibrary AImageLibrary)
        {
            FILManager = AILManager;
            FImageLibrary = AImageLibrary;
            ;
        }

        // ImageLib.pas:8911
        public override void Destroy()
        {
            DestroyItems();
            base.Destroy();
        }

        // ImageLib.pas:8918
        // { The indices with a sequence, separated by commas }
        public Aow.Rtl.AnsiString UsedIndexString()
        {
            Aow.Rtl.AnsiString Result = default;
            int I = default;
            Result = (AnsiString)"";
            {
                I = 0;
                int end__209 = Count - 1;
                for (; I <= end__209; I += 1)
                {
                    if (List[I] != null)
                    {
                        Result = (Result + Aow.Rtl.Rtl.IntToStr(I)) + AnsiString.FromBytes(44);
                    }
                }
            }

            if (Result != ((AnsiString)""))
            {
                Result = Aow.Rtl.Rtl.Copy(Result, 1, Result.Length - 1);
            }

            return Result;
        }

        // ImageLib.pas:8928
        public void DestroyItems()
        {
            int I = default;
            {
                I = Count - 1;
                byte end__210 = 0;
                for (; I >= end__210; I -= 1)
                {
                    if (List[I] != null)
                    {
                        ((Aow.Units.ImageLib.TImageSequence)List[I]).Destroy();
                    }

                    base.Delete(I);
                }
            }
        }

        // ImageLib.pas:8939
        public Aow.Units.ImageLib.TImageSequence GetLayerImageSequence(int Index, int Layer)
        {
            Aow.Units.ImageLib.TImageSequence Result = default;
            if (Index < Count)
            {
                Result = (Aow.Units.ImageLib.TImageSequence)List[Index];
            }
            else
            {
                Result = null;
            }

            while ((Result != null) && (Layer != Result.FLayer))
            {
                Result = Result.FNext;
            }

            return Result;
        }

        // ImageLib.pas:8949
        // { Deletes the empty indices at the end; returns the last index with a sequence }
        public int ReduceCount()
        {
            int Result = default;
            Result = Count - 1;
            while ((Result >= 0) && (List[Result] == null))
            {
                base.Delete(Result);
                Result -= 1;
            }

            return Result;
        }

        // ImageLib.pas:8959
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            int I = default;
            int N = default;
            bool IsNil = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            DestroyItems();
            Stream.Read(Aow.Rtl.Bytes.Of(ref N, 4));
            {
                I = 0;
                int end__211 = N - 1;
                for (; I <= end__211; I += 1)
                {
                    Stream.Read(Aow.Rtl.Bytes.Of(ref IsNil, 1));
                    if (!IsNil)
                    {
                        Sequence = Aow.Units.ImageLib.TImageSequence.Create(this);
                        Sequence.LoadFromStream(Stream);
                        base.Add(Sequence);
                    }
                    else
                    {
                        base.Add(null);
                    }
                }
            }

            if (FLinked)
            {
                FLinked = false;
                LinkToIL();
            }
        }

        // ImageLib.pas:8986
        public void SaveToStream(Aow.Rtl.TStream Stream)
        {
            int I = default;
            bool IsNil = default;
            ReduceCount();
            Stream.Write(Aow.Rtl.Rtl.ValueBytes<int>(Count, 4));
            {
                I = 0;
                int end__212 = Count - 1;
                for (; I <= end__212; I += 1)
                {
                    IsNil = List[I] == null;
                    Stream.Write(Aow.Rtl.Bytes.Of(ref IsNil, 1));
                    if (!IsNil)
                    {
                        ((Aow.Units.ImageLib.TImageSequence)List[I]).SaveToStream(Stream);
                    }
                }
            }
        }

        // ImageLib.pas:9001
        public void SaveToIniFile(Aow.Rtl.AnsiString Section, Aow.Rtl.TIniFile IniFile)
        {
            /* TODO(T-shim) Platform or editor boundary IniFiles.WriteString */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:9016
        public void LoadFromIniFile(Aow.Rtl.AnsiString Section, Aow.Rtl.TIniFile IniFile)
        {
            /* TODO(T-shim) Platform or editor boundary IniFiles.ReadString */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:9044
        public Aow.Units.ImageLib.TImageSequence GetImageSequence(int Index)
        {
            Aow.Units.ImageLib.TImageSequence Result = default;
            if ((Index >= 0) && (Index < Count))
            {
                Result = (Aow.Units.ImageLib.TImageSequence)List[Index];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // ImageLib.pas:9052
        public void SetImageSequence(int Index, Aow.Units.ImageLib.TImageSequence Value)
        {
            while (Index >= Count)
            {
                base.Add(null);
            }

            if (List[Index] != null)
            {
                ((Aow.Units.ImageLib.TImageSequence)List[Index]).Destroy();
            }

            List[Index] = Value;
        }

        // ImageLib.pas:9059
        public int GetSubCount(int Index)
        {
            int Result = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            Result = 0;
            if (Index < Count)
            {
                Sequence = (Aow.Units.ImageLib.TImageSequence)List[Index];
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
        public int IndexOfSub(int Index, Aow.Units.ImageLib.TImageSequence Sequence)
        {
            int Result = default;
            Aow.Units.ImageLib.TImageSequence S = default;
            Result = 0;
            if (Index < Count)
            {
                S = (Aow.Units.ImageLib.TImageSequence)List[Index];
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
        public Aow.Units.ImageLib.TImageSequence GetSubImageSequence(int Index, int SubIndex)
        {
            Aow.Units.ImageLib.TImageSequence Result = default;
            int I = default;
            I = 0;
            if (Index < Count)
            {
                Result = (Aow.Units.ImageLib.TImageSequence)List[Index];
            }
            else
            {
                Result = null;
            }

            while ((Result != null) && (I != SubIndex))
            {
                Result = Result.FNext;
                I += 1;
            }

            return Result;
        }

        // ImageLib.pas:9111
        // { Inserts Sequence into the chain of Index by its layer. Original bug: an existing layer is
//   only detected after the first link, not at the head of the chain. }
        public void Add(int Index, Aow.Units.ImageLib.TImageSequence Sequence)
        {
            Aow.Units.ImageLib.TImageSequence S = default;
            Aow.Units.ImageLib.TImageSequence Prev = default;
            if (Index < Count)
            {
                S = (Aow.Units.ImageLib.TImageSequence)List[Index];
            }
            else
            {
                S = null;
            }

            if (S != null)
            {
                Prev = null;
                while ((S != null) && (S.FLayer < Sequence.FLayer))
                {
                    Prev = S;
                    S = S.FNext;
                }

                if (Prev != null)
                {
                    if ((S != null) && (Sequence.FLayer == S.FLayer))
                    {
                        throw Aow.Rtl.Exception.Create((AnsiString)"Layer does already exist");
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
        public void Delete(int Index, Aow.Units.ImageLib.TImageSequence Sequence)
        {
            Aow.Units.ImageLib.TImageSequence S = default;
            Aow.Units.ImageLib.TImageSequence Prev = default;
            if (Index < Count)
            {
                Prev = null;
                S = (Aow.Units.ImageLib.TImageSequence)List[Index];
                while ((S != null) && (S != Sequence))
                {
                    Prev = S;
                    S = S.FNext;
                }

                if ((S != null) && (S == Sequence))
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
            int I = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            if (!FLinked)
            {
                FLinked = true;
                {
                    I = 0;
                    int end__213 = Count - 1;
                    for (; I <= end__213; I += 1)
                    {
                        Sequence = (Aow.Units.ImageLib.TImageSequence)List[I];
                        while (Sequence != null)
                        {
                            Sequence.LinkToIL();
                            Sequence = Sequence.FNext;
                        }
                    }
                }
            }
        }

        // ImageLib.pas:9189
        public void ReleaseIL()
        {
            int I = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            if (FLinked)
            {
                FLinked = false;
                {
                    I = 0;
                    int end__214 = Count - 1;
                    for (; I <= end__214; I += 1)
                    {
                        Sequence = (Aow.Units.ImageLib.TImageSequence)List[I];
                        while (Sequence != null)
                        {
                            Sequence.ReleaseIL();
                            Sequence = Sequence.FNext;
                        }
                    }
                }
            }
        }

        // ImageLib.pas:9209
        public bool Animating()
        {
            bool Result = default;
            int I = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            Result = false;
            I = 0;
            while ((I != Count) && !Result)
            {
                Sequence = (Aow.Units.ImageLib.TImageSequence)List[I];
                while ((Sequence != null) && !Result)
                {
                    Result = Sequence.FSequenceMode != Aow.Units.ImageLib.TSequenceMode.smNone;
                    Sequence = Sequence.FNext;
                }

                I += 1;
            }

            return Result;
        }

        // ImageLib.pas:9228
        public Aow.Units.ImageLib.TLibraryImage GetPreviewImage()
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            int I = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            Result = null;
            I = 0;
            while ((I != Count) && (Result == null))
            {
                Sequence = (Aow.Units.ImageLib.TImageSequence)List[I];
                while ((Sequence != null) && (Result == null))
                {
                    Result = Sequence.GetPreviewImage();
                    Sequence = Sequence.FNext;
                }

                I += 1;
            }

            return Result;
        }

        // ImageLib.pas:9247
        public void GetDimensions(int Index, ref Aow.Rtl.TRect R)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            Aow.Rtl.TRect SR = default;
            R.Left = 0;
            R.Right = 0;
            R.Top = 0;
            R.Bottom = 0;
            if ((Index < Count) && (Index >= 0))
            {
                Sequence = (Aow.Units.ImageLib.TImageSequence)List[Index];
                while (Sequence != null)
                {
                    Sequence.GetDimensions(ref SR);
                    Aow.Rtl.Rtl.UnionRect(ref R, R, SR);
                    Sequence = Sequence.FNext;
                }
            }
        }

        // ImageLib.pas:9270
        // { Shows the layers of Index for Frame without looping; returns False when no layer has an
//   image for Frame }
        public bool Show(int Index, int X, int Y, int Frame, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            bool Result = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int I = default;
            Result = false;
            if (Index < Count)
            {
                Sequence = (Aow.Units.ImageLib.TImageSequence)List[Index];
                while (Sequence != null)
                {
                    I = Sequence.GetUnloopedFrameIndex(Frame);
                    if (I != -1)
                    {
                        Result = true;
                        var call__215 = Sequence.FImageLibrary.ImageExist(I);
                        if (call__215)
                        {
                            var call__216 = Sequence.FImageLibrary[I];
                            call__216.Show(X + Sequence.FdXshift, Y + Sequence.FdYshift, SD);
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
            int Result = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int N = default;
            Result = 0;
            if (Index < Count)
            {
                Sequence = (Aow.Units.ImageLib.TImageSequence)List[Index];
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
        public void ShowLooped(int Index, int X, int Y, int Frame, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int I = default;
            if (Index < Count)
            {
                Sequence = (Aow.Units.ImageLib.TImageSequence)List[Index];
                while (Sequence != null)
                {
                    I = Sequence.GetFrameIndex(Frame, 0);
                    if (I != -1)
                    {
                        var call__217 = Sequence.FImageLibrary.ImageExist(I);
                        if (call__217)
                        {
                            var call__218 = Sequence.FImageLibrary[I];
                            call__218.Show(X + Sequence.FdXshift, Y + Sequence.FdYshift, SD);
                        }
                    }

                    Sequence = Sequence.FNext;
                }
            }
        }

        // ImageLib.pas:9330
        public void ShowLoopedEx(int Index, int X, int Y, int Frame, int Offset, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int I = default;
            if (Index < Count)
            {
                Sequence = (Aow.Units.ImageLib.TImageSequence)List[Index];
                while (Sequence != null)
                {
                    I = Sequence.GetFrameIndex(Frame, Offset);
                    if (I != -1)
                    {
                        var call__219 = Sequence.FImageLibrary.ImageExist(I);
                        if (call__219)
                        {
                            var call__220 = Sequence.FImageLibrary[I];
                            call__220.Show(X + Sequence.FdXshift, Y + Sequence.FdYshift, SD);
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

        public sealed class ImageSequencesAccessor(Aow.Units.ImageLib.TImageSequenceList target)
        {
            public Aow.Units.ImageLib.TImageSequence this[int Index] { get => target.GetImageSequence(Index); set => target.SetImageSequence(Index, value); }
        }

        public ImageSequencesAccessor ImageSequences => new ImageSequencesAccessor(this);
        public bool Linked { get => FLinked; }
        public Aow.Units.ImageLib.TImageLibraryManager ILManager { get => FILManager; }
        public Aow.Units.ImageLib.TCustomImageLibrary ImageLibrary { get => FImageLibrary; }

        // ImageLib.pas:1093
        public static Aow.Units.ImageLib.TImageSequenceList Create(Aow.Units.ImageLib.TImageLibraryManager AILManager, Aow.Units.ImageLib.TCustomImageLibrary AImageLibrary)
        {
            Aow.Units.ImageLib.TImageSequenceList o = new Aow.Units.ImageLib.TImageSequenceList();
            try
            {
                o.Create__init_TImageSequenceList(AILManager, AImageLibrary);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public partial class TFrameTableList : Aow.Rtl.TPersistent
    {
        internal Ptr<int> FList;
        internal int FCount;
        internal int FCapacity;
        internal int FDefaultValue;
        // ImageLib.pas:9434
        protected internal int Get(int Index)
        {
            int Result = default;
            if ((Index < 0) || (Index >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            Result = FList[Index];
            return Result;
        }

        // ImageLib.pas:9440
        protected internal virtual void Grow()
        {
            int Delta = default;
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
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
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
            if ((NewCapacity < FCount) || (NewCapacity > 134217727))
            {
                ClassType.Error((AnsiString)"List capacity out of bounds (%d)", NewCapacity);
            }

            if (NewCapacity != FCapacity)
            {
                FList = FList.Realloc(NewCapacity);
                FCapacity = NewCapacity;
            }
        }

        // ImageLib.pas:9517
        protected internal void SetCount(int NewCount)
        {
            if ((NewCount < 0) || (NewCount > 134217727))
            {
                ClassType.Error((AnsiString)"List count out of bounds (%d)", NewCount);
            }

            if (NewCount > FCapacity)
            {
                SetCapacity(NewCount);
            }

            if (NewCount > FCount)
            {
                Aow.Rtl.Rtl.FillBytes(MemoryMarshal.AsBytes((FList + FCount).Array.AsSpan((FList + FCount).Index, (NewCount - FCount))), FDefaultValue);
            }

            FCount = NewCount;
        }

        // ImageLib.pas:9360
        // { TFrameTableList }
        public override void Destroy()
        {
            Clear();
        }

        // ImageLib.pas:9365
        public override void Assign(Aow.Rtl.TPersistent Source)
        {
            if (Source is Aow.Units.ImageLib.TFrameTableList)
            {
                var frameTableList = (Aow.Units.ImageLib.TFrameTableList)Source;
                this.SetCapacity(frameTableList.FCount);
                this.FCount = frameTableList.FCount;
                Ptr<int>.Copy(frameTableList.FList, this.FList, this.FCount);
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
            int Result = default;
            Result = FCount;
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
            if ((Index < 0) || (Index >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            FCount -= 1;
            if (Index < FCount)
            {
                Ptr<int>.Copy(FList + (Index + 1), FList + Index, (FCount - Index));
            }
        }

        // ImageLib.pas:9412
        public void Exchange(int Index1, int Index2)
        {
            int Item = default;
            if ((Index1 < 0) || (Index1 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index1);
            }

            if ((Index2 < 0) || (Index2 >= FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index2);
            }

            Item = FList[Index1];
            FList[Index1] = FList[Index2];
            FList[Index2] = Item;
        }

        // ImageLib.pas:9423
        public Aow.Units.ImageLib.TFrameTableList Expand()
        {
            Aow.Units.ImageLib.TFrameTableList Result = default;
            if (FCount == FCapacity)
            {
                Grow();
            }

            Result = this;
            return Result;
        }

        // ImageLib.pas:9429
        public int First()
        {
            int Result = default;
            Result = Get(0);
            return Result;
        }

        // ImageLib.pas:9450
        public int IndexOf(int Item)
        {
            int Result = default;
            Result = 0;
            while ((Result < FCount) && (FList[Result] != Item))
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
            if ((Index < 0) || (Index > FCount))
            {
                ClassType.Error((AnsiString)"List index out of bounds (%d)", Index);
            }

            if (FCount == FCapacity)
            {
                Grow();
            }

            if (Index < FCount)
            {
                Ptr<int>.Copy(FList + Index, FList + (Index + 1), (FCount - Index));
            }

            FList[Index] = Item;
            FCount += 1;
        }

        // ImageLib.pas:9467
        public int Last()
        {
            int Result = default;
            var call__222 = Get(FCount - 1);
            Result = call__222;
            return Result;
        }

        // ImageLib.pas:9472
        public void Move(int CurIndex, int NewIndex)
        {
            int Item = default;
            if (CurIndex != NewIndex)
            {
                if ((NewIndex < 0) || (NewIndex >= FCount))
                {
                    ClassType.Error((AnsiString)"List index out of bounds (%d)", NewIndex);
                }

                Item = Get(CurIndex);
                Delete(CurIndex);
                Insert(NewIndex, Item);
            }
        }

        // ImageLib.pas:9493
        public int Remove(int Item)
        {
            int Result = default;
            Result = IndexOf(Item);
            if (Result != -1)
            {
                Delete(Result);
            }

            return Result;
        }

        // ImageLib.pas:9499
        public void Pack()
        {
            int I = default;
            {
                I = FCount - 1;
                byte end__223 = 0;
                for (; I >= end__223; I -= 1)
                {
                    var call__224 = this[I];
                    if (call__224 == 0)
                    {
                        Delete(I);
                    }
                }
            }
        }

        // ImageLib.pas:9554
        public void Sort(Aow.Units.ImageLib.TFrameTableSortCompare Compare)
        {
            if (!FList.IsNull && (Count > 0))
            {
                Aow.Units.ImageLib.ImageLib.FrameTableQuickSort(FList, 0, Count - 1, Compare);
            }
        }

        // ImageLib.pas:9560
        public bool Compare(Aow.Units.ImageLib.TFrameTableList List)
        {
            bool Result = default;
            Result = List.FCount == FCount;
            if (Result)
            {
                var call__225 = Aow.Rtl.Rtl.CompareMem(MemoryMarshal.AsBytes(FList.Array.AsSpan(FList.Index, FCount)), MemoryMarshal.AsBytes(List.FList.Array.AsSpan(List.FList.Index, FCount)), FCount * 4);
                Result = call__225;
            }

            return Result;
        }

        // ImageLib.pas:9566
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            int N = default;
            Clear();
            Stream.Read(Aow.Rtl.Bytes.Of(ref FDefaultValue, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref N, 4));
            SetCapacity(N);
            Stream.Read(MemoryMarshal.AsBytes(FList.Array.AsSpan(FList.Index, N)));
            FCount = N;
        }

        // ImageLib.pas:9578
        public void SaveToStream(Aow.Rtl.TStream Stream)
        {
            Stream.Write(Aow.Rtl.Bytes.Of(ref FDefaultValue, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FCount, 4));
            Stream.Write(MemoryMarshal.AsBytes(FList.Array.AsSpan(FList.Index, FCount)));
        }

        // ImageLib.pas:9586
        // { The items separated by commas }
        public Aow.Rtl.AnsiString GetItemString()
        {
            Aow.Rtl.AnsiString Result = default;
            int I = default;
            Result = (AnsiString)"";
            {
                I = 0;
                int end__226 = FCount - 1;
                for (; I <= end__226; I += 1)
                {
                    Result = (Result + Aow.Rtl.Rtl.IntToStr(FList[I])) + AnsiString.FromBytes(44);
                }
            }

            if (Result != ((AnsiString)""))
            {
                Result = Aow.Rtl.Rtl.Copy(Result, 1, Result.Length - 1);
            }

            return Result;
        }

        // ImageLib.pas:9597
        // { Reads items separated by commas; other characters than digits are dropped, an empty item
//   is 0 }
        public void SetItemString(Aow.Rtl.AnsiString Value)
        {
            int I = default;
            int N = default;
            Aow.Rtl.AnsiString S = default;
            {
                I = Value.Length;
                byte end__227 = 1;
                for (; I >= end__227; I -= 1)
                {
                    if (!TSet<byte>.WithBounds(0, 255).With(44).WithRange(48, 57).Contains(Value[I]))
                    {
                        Value = Aow.Rtl.Rtl.Delete(Value, I, 1);
                    }
                }
            }

            Clear();
            I = Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(44), Value);
            while (Value != ((AnsiString)""))
            {
                if (I != 0)
                {
                    S = Aow.Rtl.Rtl.Copy(Value, 1, I - 1);
                    Value = Aow.Rtl.Rtl.Delete(Value, 1, I);
                }
                else
                {
                    S = Value;
                    Value = (AnsiString)"";
                }

                if (S != ((AnsiString)""))
                {
                    N = Aow.Rtl.Rtl.StrToInt(S);
                }
                else
                {
                    N = 0;
                }

                Add(N);
                I = Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(44), Value);
            }
        }

        public int Capacity { get => FCapacity; set => SetCapacity(value); }
        public int Count { get => FCount; set => SetCount(value); }

        public int this[int Index] { get => Get(Index); set => Put(Index, value); }

        public Ptr<int> List { get => FList; }
        public int DefaultValue { get => FDefaultValue; set => FDefaultValue = value; }
        public Aow.Rtl.AnsiString ItemString { get => GetItemString(); set => SetItemString(value); }

        // System.pas:111
        public static Aow.Units.ImageLib.TFrameTableList Create()
        {
            return new Aow.Units.ImageLib.TFrameTableList();
        }

        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // ImageLib.pas:9401
            public virtual void Error(Aow.Rtl.AnsiString Msg, int Data)
            {
                throw Aow.Rtl.EListError.CreateFmt(Msg, [(int)Data]);
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ImageLib.TFrameTableList => (Meta)MetaRegistry.Of(typeof(T));
        }

        public new Meta ClassType => (Meta)base.ClassType;
    }

    // { ImageLib.TImageSequence (instance size 56): an animation of the images of a library, one
//     layer of a chain sorted by layer. +4 FOwner, +8 FLayer, +$C FNext (the next layer), +$10
//     FImageLibrary (linked by LinkToIL: FImageLibraryName from the manager of FOwner, or the
//     library of FOwner), +$14 FImageLibraryName, +$18 FSequenceSpeed (frames per image - 1), +$19
//     FSequenceMode, +$1C FFrameTable (the image indices of smFrameTable and smRndFrameTable),
//     +$20 FImageIndex (the image of smNone, the random start of smRandom and smRndFrameTable),
//     +$24 FdXshift, +$28 FdYshift, +$2C FLastFrame, +$30 FSpeedCount, +$34 FFrameCounter. }
    public partial class TImageSequence : Aow.Rtl.TObject
    {
        internal Aow.Units.ImageLib.TImageSequenceList FOwner;
        internal byte FLayer;
        internal Aow.Units.ImageLib.TImageSequence FNext;
        internal Aow.Units.ImageLib.TCustomImageLibrary FImageLibrary;
        internal Aow.Rtl.AnsiString FImageLibraryName;
        internal byte FSequenceSpeed;
        internal Aow.Units.ImageLib.TSequenceMode FSequenceMode;
        internal Aow.Units.ImageLib.TFrameTableList FFrameTable;
        internal int FImageIndex;
        internal int FdXshift;
        internal int FdYshift;
        internal int FLastFrame;
        internal byte FSpeedCount;
        internal int FFrameCounter;
        // ImageLib.pas:8606
        internal void SetImageLibraryName(Aow.Rtl.AnsiString Value)
        {
            ReleaseIL();
            FImageLibraryName = Value;
            if (FOwner.FLinked)
            {
                LinkToIL();
            }
        }

        // ImageLib.pas:8613
        internal void SetSequenceMode(Aow.Units.ImageLib.TSequenceMode Value)
        {
            if (TSet<Aow.Units.ImageLib.TSequenceMode>.Of(Aow.Units.ImageLib.TSequenceMode.smFrameTable, Aow.Units.ImageLib.TSequenceMode.smRndFrameTable).Contains(Value) && (FFrameTable == null))
            {
                var call__228 = Aow.Units.ImageLib.TFrameTableList.Create();
                FFrameTable = call__228;
            }

            FSequenceMode = Value;
        }

        // ImageLib.pas:8553
        // { TImageSequence }
        public void Create__init_TImageSequence(Aow.Units.ImageLib.TImageSequenceList AOwner)
        {
            FOwner = AOwner;
            ;
        }

        // ImageLib.pas:8559
        public override void Destroy()
        {
            FFrameTable.Free();
            ReleaseIL();
            if (FNext != null)
            {
                FNext.Destroy();
            }

            base.Destroy();
        }

        // ImageLib.pas:8567
        public Aow.Units.ImageLib.TImageSequence Copy(Aow.Units.ImageLib.TImageSequenceList AOwner)
        {
            Aow.Units.ImageLib.TImageSequence Result = default;
            Aow.Rtl.TMemoryStream Stream = default;
            Stream = new Aow.Rtl.TMemoryStream();
            try
            {
                Result = Aow.Units.ImageLib.TImageSequence.Create(AOwner);
                SaveToStream(Stream);
                Stream.Position = 0;
                Result.LoadFromStream(Stream);
            }
            finally
            {
                Stream.Free();
            }

            return Result;
        }

        // ImageLib.pas:8582
        public Aow.Units.ImageLib.TLibraryImage GetPreviewImage()
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            int I = default;
            if (this != null)
            {
                Result = null;
                if ((FFrameTable != null) && (FFrameTable.FCount != 0))
                {
                    var call__229 = FImageLibrary.GetSafe(FFrameTable.FList[0]);
                    Result = call__229;
                }

                if (FSequenceMode == Aow.Units.ImageLib.TSequenceMode.smNone)
                {
                    var call__230 = FImageLibrary.GetSafe(FImageIndex);
                    Result = call__230;
                }

                if (Result == null)
                {
                    I = 0;
                    while ((I != FImageLibrary.Count) && (Result == null))
                    {
                        var call__231 = FImageLibrary[I];
                        Result = call__231;
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
        public void GetDimensions(ref Aow.Rtl.TRect R)
        {
            int I = default;
            Aow.Units.ImageLib.TLibraryImage Image = default;
            Aow.Rtl.TRect IR = default;
            R.Left = 0;
            R.Right = 0;
            R.Top = 0;
            R.Bottom = 0;
            if (FImageLibrary != null)
            {
                switch (FSequenceMode)
                {
                    case Aow.Units.ImageLib.TSequenceMode.smFrameTable:
                    case Aow.Units.ImageLib.TSequenceMode.smRndFrameTable:
                    {
                        I = 0;
                        int end__232 = FFrameTable.Count - 1;
                        for (; I <= end__232; I += 1)
                        {
                            var call__233 = FFrameTable[I];
                            var call__234 = FImageLibrary.GetSafe(call__233);
                            Image = call__234;
                            if (Image != null)
                            {
                                Image.GetVisualDimensions(ref IR);
                                Aow.Rtl.Rtl.UnionRect(ref R, R, IR);
                            }
                        }
                    }

                        break;
                    case Aow.Units.ImageLib.TSequenceMode.smUp:
                    case Aow.Units.ImageLib.TSequenceMode.smUpDown:
                    case Aow.Units.ImageLib.TSequenceMode.smRandom:
                    {
                        I = 0;
                        int end__235 = FImageLibrary.Count - 1;
                        for (; I <= end__235; I += 1)
                        {
                            var call__236 = FImageLibrary.GetSafe(I);
                            Image = call__236;
                            if (Image != null)
                            {
                                Image.GetVisualDimensions(ref IR);
                                Aow.Rtl.Rtl.UnionRect(ref R, R, IR);
                            }
                        }
                    }

                        break;
                    case Aow.Units.ImageLib.TSequenceMode.smNone:
                        var call__237 = FImageLibrary.GetSafe(FImageIndex);
                        Image = call__237;
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
            int Result = default;
            switch (FSequenceMode)
            {
                case Aow.Units.ImageLib.TSequenceMode.smFrameTable:
                    Result = FFrameTable.Count * (FSequenceSpeed + 1);
                    break;
                case Aow.Units.ImageLib.TSequenceMode.smUp:
                    Result = FImageLibrary.Count * (FSequenceSpeed + 1);
                    break;
                case Aow.Units.ImageLib.TSequenceMode.smUpDown:
                    Result = (FImageLibrary.Count * (FSequenceSpeed + 1)) * 2;
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
            int Result = default;
            if (Frame >= 0)
            {
                if (FSequenceMode != Aow.Units.ImageLib.TSequenceMode.smNone)
                {
                    switch (FSequenceMode)
                    {
                        case Aow.Units.ImageLib.TSequenceMode.smFrameTable:
                            if (Frame != FLastFrame)
                            {
                                if (FSpeedCount == 0)
                                {
                                    FFrameCounter = (FFrameCounter + 1) & 16777215;
                                    FSpeedCount = unchecked((byte)FSequenceSpeed);
                                }
                                else
                                {
                                    FSpeedCount = (byte)(((int)FSpeedCount) - 1);
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
                        case Aow.Units.ImageLib.TSequenceMode.smUp:
                            if (Frame != FLastFrame)
                            {
                                if (FSpeedCount == 0)
                                {
                                    FFrameCounter = (FFrameCounter + 1) & 16777215;
                                    FSpeedCount = unchecked((byte)FSequenceSpeed);
                                }
                                else
                                {
                                    FSpeedCount = (byte)(((int)FSpeedCount) - 1);
                                }

                                FLastFrame = Frame;
                            }

                            Result = FFrameCounter + Offset;
                            Result = Result % FImageLibrary.Count;
                            break;
                        case Aow.Units.ImageLib.TSequenceMode.smUpDown:
                            if (Frame != FLastFrame)
                            {
                                if (FSpeedCount == 0)
                                {
                                    FFrameCounter = (FFrameCounter + 1) & 16777215;
                                    FSpeedCount = unchecked((byte)FSequenceSpeed);
                                }
                                else
                                {
                                    FSpeedCount = (byte)(((int)FSpeedCount) - 1);
                                }

                                FLastFrame = Frame;
                            }

                            Result = (FFrameCounter + Offset) % ((FImageLibrary.Count - 1) * 2);
                            Result = System.Math.Abs(Result - (FImageLibrary.Count - 2));
                            break;
                        case Aow.Units.ImageLib.TSequenceMode.smRndFrameTable:
                            if ((Frame != FLastFrame) && ((Frame % (FSequenceSpeed + 1)) == 0))
                            {
                                var call__238 = Aow.Rtl.Rtl.Random(FFrameTable.Count);
                                FImageIndex = call__238;
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
                        case Aow.Units.ImageLib.TSequenceMode.smRandom:
                            if ((Frame != FLastFrame) && ((Frame % (FSequenceSpeed + 1)) == 0))
                            {
                                var call__239 = Aow.Rtl.Rtl.Random(FImageLibrary.Count);
                                FImageIndex = call__239;
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
            int Result = default;
            if (Frame >= 0)
            {
                if (FSequenceMode != Aow.Units.ImageLib.TSequenceMode.smNone)
                {
                    switch (FSequenceMode)
                    {
                        case Aow.Units.ImageLib.TSequenceMode.smFrameTable:
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
                        case Aow.Units.ImageLib.TSequenceMode.smUp:
                            Result = Frame / (FSequenceSpeed + 1);
                            if (Result >= FImageLibrary.Count)
                            {
                                Result = -1;
                            }

                            break;
                        case Aow.Units.ImageLib.TSequenceMode.smUpDown:
                            Result = -1;
                            break;
                        case Aow.Units.ImageLib.TSequenceMode.smRndFrameTable:
                            Result = -1;
                            break;
                        case Aow.Units.ImageLib.TSequenceMode.smRandom:
                            Result = -1;
                            break;
                        default:
                            Result = -1;
                            break;
                    }
                }
                else
                {
                    Result = FImageIndex;
                    return Result;
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

            if (FImageLibraryName != ((AnsiString)""))
            {
                var call__240 = FOwner.FILManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), FImageLibraryName);
                FImageLibrary = call__240;
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
                if (FImageLibraryName != ((AnsiString)""))
                {
                    FOwner.FILManager.ReleaseIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), FImageLibraryName);
                }

                FImageLibrary = null;
            }
        }

        // ImageLib.pas:8822
        public void SaveToStream(Aow.Rtl.TStream Stream)
        {
            bool HasNext = default;
            Aow.Units.ImageLib.ImageLib.WriteStringToStream(FImageLibraryName, Stream);
            Stream.Write(Aow.Rtl.Bytes.Of(ref FLayer, 1));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FSequenceSpeed, 1));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FSequenceMode, 1));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FImageIndex, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FdXshift, 4));
            Stream.Write(Aow.Rtl.Bytes.Of(ref FdYshift, 4));
            if (TSet<Aow.Units.ImageLib.TSequenceMode>.Of(Aow.Units.ImageLib.TSequenceMode.smFrameTable, Aow.Units.ImageLib.TSequenceMode.smRndFrameTable).Contains(FSequenceMode))
            {
                FFrameTable.SaveToStream(Stream);
            }

            HasNext = FNext != null;
            Stream.Write(Aow.Rtl.Bytes.Of(ref HasNext, 1));
            if (HasNext)
            {
                FNext.SaveToStream(Stream);
            }
        }

        // ImageLib.pas:8839
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            bool HasNext = default;
            ReleaseIL();
            Aow.Units.ImageLib.ImageLib.ReadStringFromStream(ref FImageLibraryName, Stream);
            Stream.Read(Aow.Rtl.Bytes.Of(ref FLayer, 1));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FSequenceSpeed, 1));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FSequenceMode, 1));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FImageIndex, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FdXshift, 4));
            Stream.Read(Aow.Rtl.Bytes.Of(ref FdYshift, 4));
            if (TSet<Aow.Units.ImageLib.TSequenceMode>.Of(Aow.Units.ImageLib.TSequenceMode.smFrameTable, Aow.Units.ImageLib.TSequenceMode.smRndFrameTable).Contains(FSequenceMode))
            {
                if (FFrameTable == null)
                {
                    var call__241 = Aow.Units.ImageLib.TFrameTableList.Create();
                    FFrameTable = call__241;
                }

                FFrameTable.LoadFromStream(Stream);
                var call__242 = Aow.Rtl.Rtl.Random(FFrameTable.Count);
                FImageIndex = call__242;
            }

            Stream.Read(Aow.Rtl.Bytes.Of(ref HasNext, 1));
            if (HasNext)
            {
                var call__243 = Aow.Units.ImageLib.TImageSequence.Create(FOwner);
                FNext = call__243;
                FNext.LoadFromStream(Stream);
            }
        }

        // ImageLib.pas:8865
        public void SaveToIniFile(Aow.Rtl.AnsiString Section, Aow.Rtl.TIniFile IniFile)
        {
            /* TODO(T-shim) Platform or editor boundary IniFiles.WriteString */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:8880
        public void LoadFromIniFile(Aow.Rtl.AnsiString Section, Aow.Rtl.TIniFile IniFile)
        {
            /* TODO(T-shim) Platform or editor boundary IniFiles.ReadString */
            throw new System.NotImplementedException();
        }

        public Aow.Units.ImageLib.TImageSequenceList Owner { get => FOwner; }
        public byte Layer { get => FLayer; set => FLayer = value; }
        public Aow.Units.ImageLib.TImageSequence Next { get => FNext; set => FNext = value; }
        public Aow.Units.ImageLib.TCustomImageLibrary ImageLibrary { get => FImageLibrary; }
        public Aow.Rtl.AnsiString ImageLibraryName { get => FImageLibraryName; set => SetImageLibraryName(value); }
        public byte SequenceSpeed { get => FSequenceSpeed; set => FSequenceSpeed = value; }
        public byte SpeedCount { get => FSpeedCount; }
        // { $30; 5579143B }
        public Aow.Units.ImageLib.TSequenceMode SequenceMode { get => FSequenceMode; set => SetSequenceMode(value); }
        public Aow.Units.ImageLib.TFrameTableList FrameTable { get => FFrameTable; }
        public int ImageIndex { get => FImageIndex; set => FImageIndex = value; }
        public int dXshift { get => FdXshift; set => FdXshift = value; }
        public int dYshift { get => FdYshift; set => FdYshift = value; }

        // ImageLib.pas:1053
        public static Aow.Units.ImageLib.TImageSequence Create(Aow.Units.ImageLib.TImageSequenceList AOwner)
        {
            Aow.Units.ImageLib.TImageSequence o = new Aow.Units.ImageLib.TImageSequence();
            try
            {
                o.Create__init_TImageSequence(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public delegate int TFrameTableSortCompare(int Item1, int Item2);
    // { TFastLoadFileStream (instance size 8, declared in the implementation): a read-only file
//     stream opened with FILE_FLAG_SEQUENTIAL_SCAN. Destroy does not call the inherited destructor. }
    public partial class TFastLoadFileStream : Aow.Rtl.THandleStream
    {
        // ImageLib.pas:1214
        public static Aow.Units.ImageLib.TFastLoadFileStream Create(Aow.Rtl.AnsiString FileName)
        {
            Aow.Units.ImageLib.TFastLoadFileStream o = new Aow.Units.ImageLib.TFastLoadFileStream();
            try
            {
                o.Create__init_TFastLoadFileStream(FileName);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { The header of an .ILB file (24 bytes). Up to version 3 the images follow it; from version 4
//     on the image index (HeaderSize to IndexEnd) is followed by the discardable image data. }
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct TILBHeader
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_5 ID__image;
        public Aow.Rtl.AnsiString ID { get => Aow.Rtl.Rtl.ReadShortString((ReadOnlySpan<byte>)ID__image); set => Aow.Rtl.Rtl.WriteShortString((Span<byte>)ID__image, value); }

        [FieldOffset(8)]
        public float Version;
        [FieldOffset(12)]
        public int HeaderSize;
        [FieldOffset(16)]
        public int IndexEnd;
        [FieldOffset(20)]
        public int Size;
    }

    // { TILFilenameProperty (instance size 16, declared in the implementation): the property editor
//     of TCustomImageLibrary.FileName. }
    public partial class TILFilenameProperty : Aow.Rtl.TStringProperty
    {
        // ImageLib.pas:10669
        public override void Edit()
        {
            /* TODO(T-shim) Platform or editor boundary DsgnIntf.GetComponent */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:10720
        public override TSet<Aow.Rtl.TPropertyAttribute> GetAttributes()
        {
            TSet<Aow.Rtl.TPropertyAttribute> Result = default;
            Result = TSet<Aow.Rtl.TPropertyAttribute>.Of(Aow.Rtl.TPropertyAttribute.paDialog, Aow.Rtl.TPropertyAttribute.paRevertable);
            return Result;
        }

        // ImageLib.pas:10663
        // { TILFilenameProperty }
        public override Aow.Rtl.AnsiString GetValue()
        {
            /* TODO(T-shim) Platform or editor boundary DsgnIntf.GetValue */
            throw new System.NotImplementedException();
        }
    }

    public static partial class ImageLib
    {
        // ImageLib.pas:5779
        // { Draws Image into Rect of Canvas: bmStretch stretches it over Rect, bmStretchToFit stretches it
//   keeping the aspect ratio, bmCenter scales it by ScaleX and ScaleY percent and centers it,
//   bmNone scales it and puts it at the top left. The rest of Rect is filled with BkColor, or
//   with Color when Selected (bmStretchToFit and bmCenter); with Color when there is no image.
//   DrawRect receives Rect, cut to the image for a selected centered image. }
        public static void BltLibraryImage(Aow.Rtl.TCanvas Canvas, Aow.Units.ImageLib.TLibraryImage Image, Aow.Rtl.TRect Rect, Aow.Units.ImageLib.TBltMode Mode, int ScaleX, int ScaleY, int BkColor, int Color, bool Selected, ref Aow.Rtl.TRect DrawRect)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.SetStretchBltMode */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:5911
        // { Draws the layers of a sequence for Frame on a 16 bit surface of the size of R, at X, Y, and
//   blits it to R of DC }
        public static void BltImageSequence(Aow.Units.ImageLib.TImageSequence Sequence, int DC, int X, int Y, in Aow.Rtl.TRect R, int Frame, int BkColor)
        {
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            Aow.Units.GFXE.TDIB16Surface Surface = default;
            int Index = default;
            var call__244 = Aow.Units.GFXE.TDIB16Surface.Create(R.Right - R.Left, R.Bottom - R.Top, Aow.Units.GFXE.GFXE.pfRGB565);
            Surface = call__244;
            Surface.Cls(BkColor);
            Surface.Lock(ref SD);
            while (Sequence != null)
            {
                Index = Sequence.GetUnloopedFrameIndex(Frame);
                bool bool__245 = (Index != -1) && (Sequence.FImageLibrary != null);
                if (bool__245)
                {
                    var call__246 = Sequence.FImageLibrary.GetSafe(Index);
                    bool__245 = (call__246 != null);
                }

                if (bool__245)
                {
                    var call__247 = Sequence.FImageLibrary[Index];
                    call__247.Show(X + Sequence.FdXshift, Y + Sequence.FdYshift, SD);
                }

                Sequence = Sequence.FNext;
            }

            Surface.Unlock();
            /* TODO(T-4.3) Address needs a cell or array backing */
            throw new System.NotImplementedException();
            Surface.Free();
        }

        // ImageLib.pas:5936
        // { BltLibraryImage for the layers of a sequence: they are drawn for Frame on a 16 bit surface
//   of the size of their union first }
        public static void BltImageSequenceEx(Aow.Rtl.TCanvas Canvas, Aow.Units.ImageLib.TImageSequence Sequence, int Frame, Aow.Rtl.TRect Rect, Aow.Units.ImageLib.TBltMode Mode, int ScaleX, int ScaleY, int BkColor, int Color, bool Selected, ref Aow.Rtl.TRect DrawRect)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.SetStretchBltMode */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:6099
        // { Replaces every sub image of the library that was grabbed from the same file and index as
//   Image by a copy of Image with the shifts of the old one; returns the number replaced }
        public static int UpdateImage(Aow.Units.ImageLib.TCustomImageLibrary ImageLibrary, Aow.Units.ImageLib.TLibraryImage Image)
        {
            int Result = default;
            int I = default;
            int J = default;
            Aow.Units.ImageLib.TLibraryImage Sub = default;
            Aow.Units.ImageLib.TLibraryImage NewImage = default;
            Result = 0;
            if (Image != null)
            {
                {
                    I = 0;
                    int end__248 = ImageLibrary.Count - 1;
                    for (; I <= end__248; I += 1)
                    {
                        if (!ImageLibrary.IsReference(I))
                        {
                            {
                                J = 0;
                                int end__249 = ImageLibrary.GetSubCount(I) - 1;
                                for (; J <= end__249; J += 1)
                                {
                                    Sub = ImageLibrary.GetSub(I, J);
                                    if (((Sub != null) && (Aow.Rtl.Rtl.CompareText(Sub.FFileName, Image.FFileName) == 0)) && (Sub.FFileIndex == Image.FFileIndex))
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
                }
            }

            return Result;
        }

        // ImageLib.pas:6124
        // { Shows Image at every point of a line (Bresenham), skipping Spacing points after each one }
        public static void DrawImageLine(Aow.Units.ImageLib.TLibraryImage Image, int X1, int Y1, int X2, int Y2, int Spacing, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int X = default;
            int Y = default;
            int Inc2 = default;
            int DX2 = default;
            int DY2 = default;
            int DX = default;
            int DY = default;
            int D = default;
            int Skip = default;
            Skip = 0;
            if (X1 > X2)
            {
                X = X2;
                X2 = X1;
                X1 = X;
                Y = Y2;
                Y2 = Y1;
                Y1 = Y;
            }
            else
            {
                X = X1;
                Y = Y1;
            }

            if (Y1 <= Y2)
            {
                DX = X2 - X1;
                DY = Y2 - Y1;
                if (DY >= DX)
                {
                    Inc2 = (DX - DY) * 2;
                    DX2 = DX * 2;
                    D = DX2 - DY;
                    while (Y != Y2)
                    {
                        if (Skip == 0)
                        {
                            Image.Show(X, Y, SD);
                            Skip = Spacing;
                        }
                        else
                        {
                            Skip -= 1;
                        }

                        if (D >= 0)
                        {
                            D += Inc2;
                            X += 1;
                        }
                        else
                        {
                            D += DX2;
                        }

                        Y += 1;
                    }

                    if (Skip == 0)
                    {
                        Image.Show(X, Y, SD);
                    }
                }
                else
                {
                    Inc2 = (DY - DX) * 2;
                    DY2 = DY * 2;
                    D = DY2 - DX;
                    while (X != X2)
                    {
                        if (Skip == 0)
                        {
                            Image.Show(X, Y, SD);
                            Skip = Spacing;
                        }
                        else
                        {
                            Skip -= 1;
                        }

                        if (D >= 0)
                        {
                            D += Inc2;
                            Y += 1;
                        }
                        else
                        {
                            D += DY2;
                        }

                        X += 1;
                    }

                    if (Skip == 0)
                    {
                        Image.Show(X, Y, SD);
                    }
                }
            }
            else
            {
                DX = X2 - X1;
                DY = Y1 - Y2;
                if (DY >= DX)
                {
                    Inc2 = (DX - DY) * 2;
                    DX2 = DX * 2;
                    D = DX2 - DY;
                    while (Y != Y2)
                    {
                        if (Skip == 0)
                        {
                            Image.Show(X, Y, SD);
                            Skip = Spacing;
                        }
                        else
                        {
                            Skip -= 1;
                        }

                        if (D >= 0)
                        {
                            D += Inc2;
                            X += 1;
                        }
                        else
                        {
                            D += DX2;
                        }

                        Y -= 1;
                    }

                    if (Skip == 0)
                    {
                        Image.Show(X, Y, SD);
                    }
                }
                else
                {
                    Inc2 = (DY - DX) * 2;
                    DY2 = DY * 2;
                    D = DY2 - DX;
                    while (X != X2)
                    {
                        if (Skip == 0)
                        {
                            Image.Show(X, Y, SD);
                            Skip = Spacing;
                        }
                        else
                        {
                            Skip -= 1;
                        }

                        if (D >= 0)
                        {
                            D += Inc2;
                            Y -= 1;
                        }
                        else
                        {
                            D += DY2;
                        }

                        X += 1;
                    }

                    if (Skip == 0)
                    {
                        Image.Show(X, Y, SD);
                    }
                }
            }
        }

        // ImageLib.pas:6458
        public static void ParseFontText(Aow.Rtl.AnsiString Text, Aow.Rtl.TStrings Strings, Aow.Units.ImageLib.TImageLibraryFont Font, int MaxWidth)
        {
            int I = default;
            int J = default;
            int W = default;
            int Len = default;
            int LineWidth = default;
            Aow.Rtl.AnsiString Word = default;
            Aow.Rtl.AnsiString Line = default;
            I = 1;
            LineWidth = 0;
            Line = (AnsiString)"";
            Len = Text.Length;
            while (I <= Len)
            {
                if (TSet<byte>.WithBounds(0, 255).With(13).With(32).Contains(Text[I]))
                {
                    I += 1;
                }

                J = 0;
                while (((I + J) <= Len) && !TSet<byte>.WithBounds(0, 255).With(10).With(13).With(32).Contains(Text[I + J]))
                {
                    J += 1;
                }

                Word = Aow.Rtl.Rtl.Copy(Text, I, J);
                W = Font.GetTextWidth(Word);
                if (((LineWidth + W) > MaxWidth) && (Line != ((AnsiString)"")))
                {
                    Strings.Add(Line);
                    Line = (AnsiString)"";
                    LineWidth = 0;
                }

                if (Line != ((AnsiString)""))
                {
                    Line = (Line + AnsiString.FromBytes(32)) + Word;
                    LineWidth = (LineWidth + W) + Font.GetTextWidth(AnsiString.FromBytes(32));
                }
                else
                {
                    Line = Line + Word;
                    LineWidth += W;
                }

                if (((I + J) <= Len) && (Text[I + J] == 10))
                {
                    Strings.Add(Line);
                    Line = (AnsiString)"";
                    LineWidth = 0;
                    I += 1;
                }

                I += J;
            }

            if (Line != ((AnsiString)""))
            {
                Strings.Add(Line);
            }
        }

        // ImageLib.pas:10556
        // { The library image classes }
        public static Aow.Units.ImageLib.TLibraryImage.Meta GetLibraryImageClass(int Index)
        {
            Aow.Units.ImageLib.TLibraryImage.Meta Result = default;
            var call__250 = Aow.Units.ImageLib.ImageLib.LibraryImageClasses[Index];
            Result = (Aow.Units.ImageLib.TLibraryImage.Meta)call__250;
            return Result;
        }

        // ImageLib.pas:10561
        public static int LibraryImageClassCount()
        {
            int Result = default;
            Result = Aow.Units.ImageLib.ImageLib.LibraryImageClasses.Count;
            return Result;
        }

        // ImageLib.pas:10566
        public static Aow.Units.ImageLib.TLibraryImage.Meta FindLibraryImageClass(int ClassID)
        {
            Aow.Units.ImageLib.TLibraryImage.Meta Result = default;
            int I = default;
            bool Found = default;
            Found = false;
            I = 0;
            while ((I < Aow.Units.ImageLib.ImageLib.LibraryImageClasses.Count) && !Found)
            {
                var call__251 = Aow.Units.ImageLib.ImageLib.LibraryImageClasses[I];
                var call__252 = ((Aow.Units.ImageLib.TLibraryImage.Meta)call__251).ClassID();
                Found = ClassID == call__252;
                I += 1;
            }

            if (Found)
            {
                var call__253 = Aow.Units.ImageLib.ImageLib.LibraryImageClasses[I - 1];
                Result = (Aow.Units.ImageLib.TLibraryImage.Meta)call__253;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // ImageLib.pas:10584
        public static void RegisterLibraryImageClasses(ReadOnlySpan<Aow.Units.ImageLib.TLibraryImage.Meta> AClasses)
        {
            int I = default;
            {
                I = 0;
                int end__254 = AClasses.Length - 1;
                for (; I <= end__254; I += 1)
                {
                    var call__255 = AClasses[I].ClassID();
                    bool bool__256 = (call__255 != 0);
                    if (bool__256)
                    {
                        var call__257 = AClasses[I].ClassID();
                        var call__258 = Aow.Units.ImageLib.ImageLib.FindLibraryImageClass(call__257);
                        bool__256 = (call__258 == null);
                    }

                    if (bool__256)
                    {
                        Aow.Units.ImageLib.ImageLib.LibraryImageClasses.Add(AClasses[I]);
                    }
                    else
                    {
                        var call__259 = Aow.Rtl.Exception.Create((((AnsiString)"Image ClassID is already used (") + ((AnsiString)AClasses[I].ClassName)) + AnsiString.FromBytes(41));
                        throw call__259;
                    }
                }
            }
        }

        // ImageLib.pas:10595
        public static Aow.Units.ImageLib.TLibraryImage ReadImageFromStream(Aow.Rtl.TStream Stream, Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            int ID = default;
            Aow.Units.ImageLib.TLibraryImage.Meta ImageClass = default;
            Stream.Read(Aow.Rtl.Bytes.Of(ref ID, 4));
            ImageClass = Aow.Units.ImageLib.ImageLib.FindLibraryImageClass(ID);
            if (ImageClass != null)
            {
                Result = ImageClass.Create(AOwner);
                Result.LoadFromStream(Stream);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // ImageLib.pas:10611
        public static void WriteImageToStream(Aow.Units.ImageLib.TLibraryImage Image, Aow.Rtl.TStream Stream)
        {
            int ID = default;
            if (Image != null)
            {
                ID = Image.ClassType.ClassID();
                Stream.Write(Aow.Rtl.Bytes.Of(ref ID, 4));
                Image.SaveToStream(Stream);
            }
            else
            {
                ID = 0;
                Stream.Write(Aow.Rtl.Bytes.Of(ref ID, 4));
            }
        }

        // ImageLib.pas:10628
        public static Aow.Units.ImageLib.TLibraryImage ReadImageFromStreamEx(Aow.Rtl.TStream Stream, Aow.Rtl.TStream DataStream, Aow.Units.ImageLib.TCustomImageLibrary AOwner)
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            int ID = default;
            Aow.Units.ImageLib.TLibraryImage.Meta ImageClass = default;
            Stream.Read(Aow.Rtl.Bytes.Of(ref ID, 4));
            ImageClass = Aow.Units.ImageLib.ImageLib.FindLibraryImageClass(ID);
            if (ImageClass != null)
            {
                Result = ImageClass.Create(AOwner);
                Result.LoadFromStreamEx(Stream, DataStream);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // ImageLib.pas:10644
        public static void WriteImageToStreamEx(Aow.Units.ImageLib.TLibraryImage Image, Aow.Rtl.TStream Stream, Aow.Rtl.TStream DataStream)
        {
            int ID = default;
            if (Image != null)
            {
                ID = Image.ClassType.ClassID();
                Stream.Write(Aow.Rtl.Bytes.Of(ref ID, 4));
                Image.SaveToStreamEx(Stream, DataStream);
            }
            else
            {
                ID = 0;
                Stream.Write(Aow.Rtl.Bytes.Of(ref ID, 4));
            }
        }

        // ImageLib.pas:10725
        public static void Register()
        {
            /* TODO(T-shim) Platform or editor boundary DsgnIntf.RegisterComponentEditor */
            throw new System.NotImplementedException();
        }

        // { The directory library file names are relative to at run time (the program's directory) }
        public static Aow.Rtl.AnsiString BaseImageLibraryPath;
        // { Access to the lookup tables of a GFXE blend table collection: the original reads the
//     array directly (GFXE.TLookupBlendTableCollection100 +8), which the GFXE source declares
//     private. }
        public static Aow.Rtl.TList LibraryImageClasses;
        // ImageLib.pas:1266
        // { The strings of the image sequences: a length and the characters }
        public static void WriteStringToStream(Aow.Rtl.AnsiString S, Aow.Rtl.TStream Stream)
        {
            int L = default;
            L = S.Length;
            Stream.Write(Aow.Rtl.Bytes.Of(ref L, 4));
            Stream.Write(Aow.Rtl.Rtl.StringBytes(S).Slice(0, L));
        }

        // ImageLib.pas:1275
        public static void ReadStringFromStream(ref Aow.Rtl.AnsiString S, Aow.Rtl.TStream Stream)
        {
            int L = default;
            Stream.Read(Aow.Rtl.Bytes.Of(ref L, 4));
            S = S.Resized(L);
            Aow.Rtl.Rtl.ReadString(Stream, ref S, 1 - 1, L, false);
        }

        // ImageLib.pas:6568
        // { Draws the two-pixel frame in Color around R and, when Focused, the focus rectangles inside
//   and outside it (TCustomImageDrawGrid.DrawCell). }
        public static void DrawFocusFrame(int DC, Aow.Rtl.TRect R, bool Focused, int Color)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.CreateSolidBrush */
            throw new System.NotImplementedException();
        }

        // ImageLib.pas:9527
        public static void FrameTableQuickSort(Ptr<int> SortList, int L, int R, Aow.Units.ImageLib.TFrameTableSortCompare SCompare)
        {
            int I = default;
            int J = default;
            int P = default;
            int T = default;
            do
            {
                I = L;
                J = R;
                P = SortList[(int)(((uint)(L + R)) >> 1)];
                do
                {
                    while (true)
                    {
                        var call__260 = SCompare(SortList[I], P);
                        if (!(call__260 < 0))
                            break;
                        {
                            I += 1;
                        }
                    }

                    while (true)
                    {
                        var call__261 = SCompare(SortList[J], P);
                        if (!(call__261 > 0))
                            break;
                        {
                            J -= 1;
                        }
                    }

                    if (I <= J)
                    {
                        T = SortList[I];
                        SortList[I] = SortList[J];
                        SortList[J] = T;
                        I += 1;
                        J -= 1;
                    }
                }
                while (!(I > J));
                if (L < J)
                {
                    Aow.Units.ImageLib.ImageLib.FrameTableQuickSort(SortList, L, J, SCompare);
                }

                L = I;
            }
            while (!(I >= R));
        }

        public static void Initialize()
        {
            var call__262 = Aow.Rtl.Rtl.Application.ExeName;
            var call__263 = Aow.Rtl.Rtl.ExtractFilePath(call__262);
            Aow.Units.ImageLib.ImageLib.BaseImageLibraryPath = call__263;
            Aow.Rtl.Rtl.RegisterClasses([typeof(Aow.Units.ImageLib.TImageLibrary)]);
            var call__264 = new Aow.Rtl.TList();
            Aow.Units.ImageLib.ImageLib.LibraryImageClasses = call__264;
            Aow.Units.ImageLib.ImageLib.RegisterLibraryImageClasses([Aow.Units.ImageLib.TLibraryImage.Meta.Of<Aow.Units.ImageLib.TPicture08>(), Aow.Units.ImageLib.TLibraryImage.Meta.Of<Aow.Units.ImageLib.TRLESprite08>()]);
            Aow.Units.ImageLib.ImageLib.RegisterLibraryImageClasses([Aow.Units.ImageLib.TLibraryImage.Meta.Of<Aow.Units.ImageLib.TPicture16>(), Aow.Units.ImageLib.TLibraryImage.Meta.Of<Aow.Units.ImageLib.TTransparentPicture16>(), Aow.Units.ImageLib.TLibraryImage.Meta.Of<Aow.Units.ImageLib.TRLESprite16>(), Aow.Units.ImageLib.TLibraryImage.Meta.Of<Aow.Units.ImageLib.TTransparentRLESprite16>(), Aow.Units.ImageLib.TLibraryImage.Meta.Of<Aow.Units.ImageLib.TShadow>(), Aow.Units.ImageLib.TLibraryImage.Meta.Of<Aow.Units.ImageLib.TImageNode>(), Aow.Units.ImageLib.TLibraryImage.Meta.Of<Aow.Units.ImageLib.TSprite16>()]);
        }

        public static void Finalize()
        {
            Aow.Units.ImageLib.ImageLib.LibraryImageClasses.Free();
        }
    }
}
