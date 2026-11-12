// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// the image sequences of the indices, each the first layer of a chain (nil for none). FLinked
// (the sequences are linked to their libraries), FILManager, FImageLibrary (the library of the
// sequences without a library name)
public partial class ImageSequenceList : DelphiList<ImageSequence>
{
    private bool _linked;
    // { HSEngine's TMapObjectResource.MsgProc writes FILManager directly }
    private ImageLibraryManager _imageLibraryManager;
    private CustomImageLibrary _imageLibrary;

    // ImageLib.pas:8903
    public ImageSequenceList(ImageLibraryManager aILManager, CustomImageLibrary imageLibrary)
    {
        _imageLibraryManager = aILManager;
        _imageLibrary = imageLibrary;
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
        string result = "";
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            if (List[I] != null)
            {
                result = result + IntToStr(I) + ",";
            }
        }

        if (!IsEmpty(result))
        {
            result = Copy(result, 1, result.Length - 1);
        }

        return result;
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
    public ImageSequence GetLayerImageSequence(int index, int layer)
    {
        ImageSequence result;
        if (index < Count)
        {
            result = List[index];
        }
        else
        {
            result = null;
        }

        while (result != null && layer != result._layer)
        {
            result = result.Next;
        }

        return result;
    }

    // ImageLib.pas:8949
    // { Deletes the empty indices at the end; returns the last index with a sequence }
    public int ReduceCount()
    {
        int result = Count - 1;
        while (result >= 0 && List[result] == null)
        {
            base.Delete(result);
            result -= 1;
        }

        return result;
    }

    // ImageLib.pas:8959
    public void LoadFromStream(Stream stream)
    {
        int N = default;
        bool isNil = default;
        ImageSequence sequence;
        DestroyItems();
        stream.ReadExactly(Bytes.Of(ref N, 4));
        for (int I = 0, last = N - 1; I <= last; I++)
        {
            stream.ReadExactly(Bytes.Of(ref isNil, 1));
            if (!isNil)
            {
                sequence = new ImageSequence(this);
                sequence.LoadFromStream(stream);
                base.Add(sequence);
            }
            else
            {
                base.Add(null);
            }
        }

        if (_linked)
        {
            _linked = false;
            LinkToImageLibrary();
        }
    }

    // ImageLib.pas:8986
    public void SaveToStream(Stream stream)
    {
        bool isNil = default;
        ReduceCount();
        stream.Write(ValueBytes(Count, 4));
        for (int I = 0, last = Count - 1; I <= last; I++)
        {
            isNil = List[I] == null;
            stream.Write(Bytes.Of(ref isNil, 1));
            if (!isNil)
            {
                List[I].SaveToStream(stream);
            }
        }
    }

    // ImageLib.pas:9001
    public void SaveToIniFile(string section, IniFile iniFile)
    {
        /* TODO: Implement IniFiles.WriteString in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:9016
    public void LoadFromIniFile(string section, IniFile iniFile)
    {
        /* TODO: Implement IniFiles.ReadString in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:9044
    public ImageSequence GetImageSequence(int index)
    {
        ImageSequence result;
        if (index >= 0 && index < Count)
        {
            result = List[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // ImageLib.pas:9052
    public void SetImageSequence(int index, ImageSequence value)
    {
        while (index >= Count)
        {
            base.Add(null);
        }

        if (List[index] != null)
        {
            ((List[index] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
        }

        List[index] = value;
    }

    // ImageLib.pas:9059
    public int GetSubCount(int index)
    {
        ImageSequence sequence;
        int result = 0;
        if (index < Count)
        {
            sequence = List[index];
        }
        else
        {
            sequence = null;
        }

        while (sequence != null)
        {
            sequence = sequence.Next;
            result += 1;
        }

        return result;
    }

    // ImageLib.pas:9076
    // { Original bug: a Sequence that is not in the chain runs past its end (nil dereference). }
    public int IndexOfSub(int index, ImageSequence sequence)
    {
        ImageSequence S;
        int result = 0;
        if (index < Count)
        {
            S = List[index];
        }
        else
        {
            S = null;
        }

        while (S != sequence)
        {
            S = S.Next;
            result += 1;
        }

        if (S == null)
        {
            result = -1;
        }

        return result;
    }

    // ImageLib.pas:9093
    public ImageSequence GetSubImageSequence(int index, int subIndex)
    {
        ImageSequence result;
        int I;
        I = 0;
        if (index < Count)
        {
            result = List[index];
        }
        else
        {
            result = null;
        }

        while (result != null && I != subIndex)
        {
            result = result.Next;
            I += 1;
        }

        return result;
    }

    // ImageLib.pas:9111
    // { Inserts Sequence into the chain of Index by its layer. Original bug: an existing layer is
    //   only detected after the first link, not at the head of the chain. }
    public void Add(int index, ImageSequence sequence)
    {
        ImageSequence S;
        ImageSequence previous;
        if (index < Count)
        {
            S = List[index];
        }
        else
        {
            S = null;
        }

        if (S != null)
        {
            previous = null;
            while (S != null && S._layer < sequence._layer)
            {
                previous = S;
                S = S.Next;
            }

            if (previous != null)
            {
                if (S != null && sequence._layer == S._layer)
                {
                    throw new InvalidOperationException("Layer does already exist");
                }

                previous.Next = sequence;
                sequence.Next = S;
            }
            else
            {
                List[index] = sequence;
                sequence.Next = S;
            }
        }
        else
        {
            SetImageSequence(index, sequence);
        }
    }

    // ImageLib.pas:9145
    // { Takes Sequence out of the chain of Index (it is not freed) }
    public void Delete(int index, ImageSequence sequence)
    {
        ImageSequence S;
        ImageSequence previous;
        if (index < Count)
        {
            previous = null;
            S = List[index];
            while (S != null && S != sequence)
            {
                previous = S;
                S = S.Next;
            }

            if (S != null && S == sequence)
            {
                if (previous != null)
                {
                    previous.Next = S.Next;
                }
                else
                {
                    List[index] = S.Next;
                }

                S.Next = null;
            }
        }
    }

    // ImageLib.pas:9169
    public void LinkToImageLibrary()
    {
        ImageSequence sequence;
        if (!_linked)
        {
            _linked = true;
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                sequence = List[I];
                while (sequence != null)
                {
                    sequence.LinkToImageLibrary();
                    sequence = sequence.Next;
                }
            }
        }
    }

    // ImageLib.pas:9189
    public void ReleaseImageLibrary()
    {
        ImageSequence sequence;
        if (_linked)
        {
            _linked = false;
            for (int I = 0, last = Count - 1; I <= last; I++)
            {
                sequence = List[I];
                while (sequence != null)
                {
                    sequence.ReleaseImageLibrary();
                    sequence = sequence.Next;
                }
            }
        }
    }

    // ImageLib.pas:9209
    public bool Animating()
    {
        int I;
        ImageSequence sequence;
        bool result = false;
        I = 0;
        while (I != Count && !result)
        {
            sequence = List[I];
            while (sequence != null && !result)
            {
                result = sequence._sequenceMode != SequenceMode.None;
                sequence = sequence.Next;
            }

            I += 1;
        }

        return result;
    }

    // ImageLib.pas:9228
    public LibraryImage GetPreviewImage()
    {
        int I;
        ImageSequence sequence;
        LibraryImage result = null;
        I = 0;
        while (I != Count && result == null)
        {
            sequence = List[I];
            while (sequence != null && result == null)
            {
                result = sequence.GetPreviewImage();
                sequence = sequence.Next;
            }

            I += 1;
        }

        return result;
    }

    // ImageLib.pas:9247
    public void GetDimensions(int index, ref Rect R)
    {
        ImageSequence sequence;
        Rect sr = default;
        R.Left = 0;
        R.Right = 0;
        R.Top = 0;
        R.Bottom = 0;
        if (index < Count && index >= 0)
        {
            sequence = List[index];
            while (sequence != null)
            {
                sequence.GetDimensions(ref sr);
                UnionRect(ref R, R, sr);
                sequence = sequence.Next;
            }
        }
    }

    // ImageLib.pas:9270
    // { Shows the layers of Index for Frame without looping; returns False when no layer has an
    //   image for Frame }
    public bool Show(int index, int X, int Y, int frame, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int I;
        bool result = false;
        if (index < Count)
        {
            sequence = List[index];
            while (sequence != null)
            {
                I = sequence.GetUnloopedFrameIndex(frame);
                if (I != -1)
                {
                    result = true;
                    if (sequence.ImageLibrary.ImageExist(I))
                    {
                        sequence.ImageLibrary[I].Show(X + sequence._offsetX, Y + sequence._offsetY, lockedSurface);
                    }
                }

                sequence = sequence.Next;
            }
        }

        return result;
    }

    // ImageLib.pas:9293
    public int GetFramesCount(int index)
    {
        ImageSequence sequence;
        int N;
        int result = 0;
        if (index < Count)
        {
            sequence = List[index];
            while (sequence != null)
            {
                N = sequence.GetTotalSequenceFrames();
                if (N > result)
                {
                    result = N;
                }

                sequence = sequence.Next;
            }
        }

        return result;
    }

    // ImageLib.pas:9311
    public void ShowLooped(int index, int X, int Y, int frame, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int I;
        if (index < Count)
        {
            sequence = List[index];
            while (sequence != null)
            {
                I = sequence.GetFrameIndex(frame, 0);
                if (I != -1)
                {
                    if (sequence.ImageLibrary.ImageExist(I))
                    {
                        sequence.ImageLibrary[I].Show(X + sequence._offsetX, Y + sequence._offsetY, lockedSurface);
                    }
                }

                sequence = sequence.Next;
            }
        }
    }

    // ImageLib.pas:9330
    public void ShowLooped(int index, int X, int Y, int frame, int offset, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int I;
        if (index < Count)
        {
            sequence = List[index];
            while (sequence != null)
            {
                I = sequence.GetFrameIndex(frame, offset);
                if (I != -1)
                {
                    if (sequence.ImageLibrary.ImageExist(I))
                    {
                        sequence.ImageLibrary[I].Show(X + sequence._offsetX, Y + sequence._offsetY, lockedSurface);
                    }
                }

                sequence = sequence.Next;
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
        public ImageSequence this[int index] { get => target.GetImageSequence(index); set => target.SetImageSequence(index, value); }
    }

    public ImageSequencesAccessor ImageSequences => new ImageSequencesAccessor(this);
    public bool Linked { get => _linked; }
    public ImageLibraryManager ImageLibraryManager { get => _imageLibraryManager; internal set => _imageLibraryManager = value; }
    public CustomImageLibrary ImageLibrary { get => _imageLibrary; }

}
