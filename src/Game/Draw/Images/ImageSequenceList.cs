// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

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
