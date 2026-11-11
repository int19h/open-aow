// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

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
    public override void Show(int X, int Y, in LockedSurface SD)
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            FImages[I].Show(X, Y, SD);
        }
    }

    // ImageLib.pas:1493
    public override void ShowOpaque(int X, int Y, in LockedSurface SD)
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            FImages[I].ShowOpaque(X, Y, SD);
        }
    }

    // ImageLib.pas:1500
    public override void ShowTransparent(int X, int Y, in LockedSurface SD)
    {
        for (int I = 0, last = FCount - 1; I <= last; I++)
        {
            FImages[I].ShowTransparent(X, Y, SD);
        }
    }

    // ImageLib.pas:1507
    public override void ShowBlended(int X, int Y, in LockedSurface SD, BlendTable Table)
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
    public override void ShowEx(int X, int Y, in LockedSurface SD, in ShowInfo ShowInfo)
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
