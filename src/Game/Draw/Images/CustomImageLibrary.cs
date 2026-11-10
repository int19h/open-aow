// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

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
