// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

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
        Stream.ReadExactly(Bytes.Of(ref FLayer, 1));
        Stream.ReadExactly(Bytes.Of(ref FSequenceSpeed, 1));
        Stream.ReadExactly(Bytes.Of(ref FSequenceMode, 1));
        Stream.ReadExactly(Bytes.Of(ref FImageIndex, 4));
        Stream.ReadExactly(Bytes.Of(ref FdXshift, 4));
        Stream.ReadExactly(Bytes.Of(ref FdYshift, 4));
        if (Set<SequenceMode>.Of(SequenceMode.FrameTable, SequenceMode.RndFrameTable).Contains(FSequenceMode))
        {
            if (FFrameTable == null)
            {
                FFrameTable = new FrameTableList();
            }

            FFrameTable.LoadFromStream(Stream);
            FImageIndex = Random(FFrameTable.Count);
        }

        Stream.ReadExactly(Bytes.Of(ref HasNext, 1));
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
