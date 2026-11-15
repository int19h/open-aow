#nullable disable
// Translated from ImageLib.pas.
using Aow.Formats.Tree;
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
    private readonly CustomImageLibrary _imageLibrary;

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
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            if (List[i] != null)
            {
                result = result + IntToStr(i) + ",";
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
        for (int i = Count - 1; i >= 0; i--)
        {
            if (List[i] != null)
            {
                ((List[i] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
            }

            Delete(i);
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

        while (result != null && layer != result.Layer)
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
            Delete(result);
            result -= 1;
        }

        return result;
    }

    /// <summary>
    /// The entries <c>TImageSequenceList.SaveToStream</c> (ImageLib.pas:8986) writes, after the same
    /// <see cref="ReduceCount"/>.
    /// </summary>
    internal List<IReadOnlyList<SequenceLayer>> SaveLayers()
    {
        ReduceCount();
        List<IReadOnlyList<SequenceLayer>> entries = new(Count);
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            entries.Add(List[i]?.SaveLayers());
        }

        return entries;
    }

    /// <summary>
    /// <c>TImageSequenceList.LoadFromStream</c> (ImageLib.pas:8959) from stored entries: the chains
    /// are built in entry and layer order, and each frame-table layer draws its image index from
    /// <c>Random</c> where the stream load draws it.
    /// </summary>
    internal void LoadLayers(IReadOnlyList<IReadOnlyList<SequenceLayer>> entries)
    {
        DestroyItems();
        foreach (IReadOnlyList<SequenceLayer> layers in entries)
        {
            if (layers != null)
            {
                ImageSequence sequence = new(this);
                sequence.LoadLayers(layers, index: 0);
                Add(sequence);
            }
            else
            {
                Add(null);
            }
        }

        if (_linked)
        {
            _linked = false;
            LinkToImageLibrary();
        }
    }

    // ImageLib.pas:9001
    public void SaveToIniFile(string section, IniFile iniFile) =>
        /* TODO: Implement IniFiles.WriteString in the platform or editor service. */
        throw new NotImplementedException();

    // ImageLib.pas:9016
    public void LoadFromIniFile(string section, IniFile iniFile) =>
        /* TODO: Implement IniFiles.ReadString in the platform or editor service. */
        throw new NotImplementedException();

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
            Add(null);
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
        ImageSequence s;
        int result = 0;
        if (index < Count)
        {
            s = List[index];
        }
        else
        {
            s = null;
        }

        while (s != sequence)
        {
            s = s.Next;
            result += 1;
        }

        if (s == null)
        {
            result = -1;
        }

        return result;
    }

    // ImageLib.pas:9093
    public ImageSequence GetSubImageSequence(int index, int subIndex)
    {
        ImageSequence result;
        int i;
        i = 0;
        if (index < Count)
        {
            result = List[index];
        }
        else
        {
            result = null;
        }

        while (result != null && i != subIndex)
        {
            result = result.Next;
            i += 1;
        }

        return result;
    }

    // ImageLib.pas:9111
    // { Inserts Sequence into the chain of Index by its layer. Original bug: an existing layer is
    //   only detected after the first link, not at the head of the chain. }
    public void Add(int index, ImageSequence sequence)
    {
        ImageSequence s;
        ImageSequence previous;
        if (index < Count)
        {
            s = List[index];
        }
        else
        {
            s = null;
        }

        if (s != null)
        {
            previous = null;
            while (s != null && s.Layer < sequence.Layer)
            {
                previous = s;
                s = s.Next;
            }

            if (previous != null)
            {
                if (s != null && sequence.Layer == s.Layer)
                {
                    throw new InvalidOperationException("Layer does already exist");
                }

                previous.Next = sequence;
                sequence.Next = s;
            }
            else
            {
                List[index] = sequence;
                sequence.Next = s;
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
        ImageSequence s;
        ImageSequence previous;
        if (index < Count)
        {
            previous = null;
            s = List[index];
            while (s != null && s != sequence)
            {
                previous = s;
                s = s.Next;
            }

            if (s != null && s == sequence)
            {
                if (previous != null)
                {
                    previous.Next = s.Next;
                }
                else
                {
                    List[index] = s.Next;
                }

                s.Next = null;
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
            for (int i = 0, last = Count - 1; i <= last; i++)
            {
                sequence = List[i];
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
            for (int i = 0, last = Count - 1; i <= last; i++)
            {
                sequence = List[i];
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
        int i;
        ImageSequence sequence;
        bool result = false;
        i = 0;
        while (i != Count && !result)
        {
            sequence = List[i];
            while (sequence != null && !result)
            {
                result = sequence.SequenceMode != SequenceMode.None;
                sequence = sequence.Next;
            }

            i += 1;
        }

        return result;
    }

    // ImageLib.pas:9228
    public LibraryImage GetPreviewImage()
    {
        int i;
        ImageSequence sequence;
        LibraryImage result = null;
        i = 0;
        while (i != Count && result == null)
        {
            sequence = List[i];
            while (sequence != null && result == null)
            {
                result = sequence.GetPreviewImage();
                sequence = sequence.Next;
            }

            i += 1;
        }

        return result;
    }

    // ImageLib.pas:9247
    public void GetDimensions(int index, out Rect R)
    {
        ImageSequence sequence;
        Rect sr = default;
        // The out value is assigned before its members are written.
        R = default;
        if (index < Count && index >= 0)
        {
            sequence = List[index];
            while (sequence != null)
            {
                sequence.GetDimensions(out sr);
                UnionRect(out R, R, sr);
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
        int i;
        bool result = false;
        if (index < Count)
        {
            sequence = List[index];
            while (sequence != null)
            {
                i = sequence.GetUnloopedFrameIndex(frame);
                if (i != -1)
                {
                    result = true;
                    if (sequence.ImageLibrary.ImageExist(i))
                    {
                        sequence.ImageLibrary[i].Show(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
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
        int n;
        int result = 0;
        if (index < Count)
        {
            sequence = List[index];
            while (sequence != null)
            {
                n = sequence.GetTotalSequenceFrames();
                if (n > result)
                {
                    result = n;
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
        int i;
        if (index < Count)
        {
            sequence = List[index];
            while (sequence != null)
            {
                i = sequence.GetFrameIndex(frame, 0);
                if (i != -1)
                {
                    if (sequence.ImageLibrary.ImageExist(i))
                    {
                        sequence.ImageLibrary[i].Show(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
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
        int i;
        if (index < Count)
        {
            sequence = List[index];
            while (sequence != null)
            {
                i = sequence.GetFrameIndex(frame, offset);
                if (i != -1)
                {
                    if (sequence.ImageLibrary.ImageExist(i))
                    {
                        sequence.ImageLibrary[i].Show(X + sequence.OffsetX, Y + sequence.OffsetY, lockedSurface);
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

    public ImageSequencesAccessor ImageSequences => new(this);
    public bool Linked => _linked;
    public ImageLibraryManager ImageLibraryManager { get => _imageLibraryManager; internal set => _imageLibraryManager = value; }
    public CustomImageLibrary ImageLibrary => _imageLibrary;

}
