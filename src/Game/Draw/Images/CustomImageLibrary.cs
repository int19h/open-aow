#nullable disable
// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.DelphiFiles;
using static Aow.Rtl.DelphiMemory;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiStreams;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.LegacyApplication;

namespace Aow.Game.Draw.Images;

// FStatus, FOnChange, FChangeLock (OnChange is called when it is 0), FReloading (the images can
// reload their data from FLoadedFileName), FLoadOnDemand, FLoadedFileName, FFileName (relative
// to the project or to BaseImageLibraryPath), FDataOffset (where the discardable image data
// starts in the file), FCapacity, FCount, FImages, FPaletteManager
public partial class CustomImageLibrary : Component
{
    public event NotifyEvent<object> OnChange;
    private byte _changeLock;
    private bool _loadOnDemand;
    private string _fileName = "";

    internal int Capacity { get; private set; }

    private int _count;

    internal ImageItem[] Images { get; private set; }

    // ImageLib.pas:9699
    internal void SetCapacity(int newCapacity)
    {
        if (newCapacity != 0)
        {
            newCapacity |= 7;
        }

        if (newCapacity != Capacity)
        {
            Images = ReallocArray(Images, count: newCapacity);
            if (newCapacity > Capacity)
            {
                ClearElements(Images, index: Capacity, count: newCapacity - Capacity);
                for (int i = Capacity; i <= newCapacity - 1; i++)
                {
                    Images[i].Reference = -1;
                }
            }

            Capacity = newCapacity;
            if (Capacity < _count)
            {
                _count = Capacity;
            }
        }
    }

    // ImageLib.pas:9717
    internal void SetCount(int value)
    {
        _count = value;
        if (_count > Capacity)
        {
            SetCapacity(_count);
        }
    }

    // ImageLib.pas:9723
    internal string GetFullFileName()
    {
        if (!IsEmpty(_fileName))
        {
            string path = ImageLibraries.BaseImageLibraryPath;

            string drive = ExtractFileDrive(_fileName);
            return IsEmpty(drive) || drive == ExtractFileDrive(path) ? ExpandFileName(path + _fileName) : _fileName;
        }
        else
        {
            return "";
        }
    }

    // ImageLib.pas:9743
    internal void SetFileName(string value)
    {
        string path = ImageLibraries.BaseImageLibraryPath;
        value = ExtractRelativePath(path, destination: value);
        if ((ComponentState.Contains(Rtl.ComponentState.Designing) || ComponentState.Contains(Rtl.ComponentState.Loading)) && value != _fileName)
        {
            _fileName = value;
            if (!IsEmpty(_fileName))
            {
                try
                {
                    LoadFromFile(GetFullFileName());
                }
                catch
                {
                    ShowMessage($"Error loading: {GetFullFileName()}");
                    _fileName = "";
                }
            }
            else
            {
                DisableReloading();
            }
        }
        else
        {
            _fileName = value;
        }
    }

    // ImageLib.pas:9775
    internal void DeleteItem(int index)
    {
        MoveElements(Images, sourceIndex: index + 1, destination: Images, destinationIndex: index, count: _count - index - 1);
        ClearElements(Images, index: _count - 1, count: 1);
        _count -= 1;
        Modified = true;
    }

    // ImageLib.pas:9783
    internal void InsertItem(int index)
    {
        SetCount(_count + 1);
        MoveElements(Images, sourceIndex: index, destination: Images, destinationIndex: index + 1, count: _count - index - 1);
        ClearElements(Images, index, count: 1);
    }

    // ImageLib.pas:9877
    internal void SetLoadOnDemand(bool value)
    {
        if (!value && _loadOnDemand)
        {
            DisableReloading();
        }

        _loadOnDemand = value;
        if (!value)
        {
            Reloading = false;
        }
    }

    // ImageLib.pas:9820
    public override void DefineProperties(Filer filer)
        /* TODO: Implement Classes.DefineBinaryProperty in the platform or editor service. */
        => throw new NotImplementedException();

    public CustomImageLibrary(Component owner) : base(owner)
    {
        PaletteManager = new PaletteManager();
        _loadOnDemand = true;
        _fileName = "";
        Status = Set<ImageLibraryStatusElement>.Of();
        Clear();
    }

    // ImageLib.pas:9679
    public override void Dispose()
    {
        Clear();
        PaletteManager?.Dispose();
        base.Dispose();
    }

    // ImageLib.pas:9686
    public virtual bool Modified
    {
        get => Status.Contains(ImageLibraryStatusElement.Modified);
        // ImageLib.pas:9691
        set => Status = value
            ? Status + Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Modified)
            : Status - Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Modified);
    }

    // ImageLib.pas:9826
    public virtual void Clear()
    {
        for (int i = _count - 1; i >= 0; i--)
        {
            Images[i].Image?.Dispose();
        }

        if (!Status.Contains(ImageLibraryStatusElement.LoadingFromFile))
        {
            _fileName = "";
            LoadedFileName = "";
        }

        Reloading = false;
        SetCapacity(0);
        PaletteManager.Clear();
        if (_changeLock == 0)
        {
            CallOnChangeEvent();
        }
    }

    // ImageLib.pas:9960
    public virtual void LoadFromStream(Stream stream)
    {
        int index = default;
        int reference = default;
        ImageLibraryHeader header = default;
        Status += Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Loading);
        _changeLock = (byte)(_changeLock + 1);
        Clear();
        Reloading = Status.Contains(ImageLibraryStatusElement.LoadingFromFile) && _loadOnDemand;
        int startPosition = (int)stream.Position;
        stream.ReadExactly(Bytes.Of(ref header, count: 24));
        _ = stream.Seek(header.HeaderSize + startPosition, origin: SeekOrigin.Begin);
        if ((double)header.Version < 4)
        {
            DataOffset = 0;
            if (header.Version > 1.5)
            {
                PaletteManager.LoadFromStream(stream);
            }
            else
            {
                PaletteManager.Clear();
            }

            // The original reads each index with Stream.Read. A short read leaves the index without its
            // terminator, so a truncated file repeats the loop without end. The port reads it with
            // ReadBuffer, which raises EndOfStreamException on a short read. Valid files always end with the -1
            // terminator. The version 4 path below reads its index region the same way, so a region
            // without the terminator raises there too.
            if ((double)header.Version <= 2)
            {
                stream.ReadBuffer(Bytes.Of(ref index, count: 4));
                while (index != -1)
                {
                    int subIndex = 0;
                    LibraryImage image = ImageStreams.ReadImageFromStream(stream, owner: this);
                    while (image is not null)
                    {
                        SetSubImage(index, subIndex, image);
                        subIndex += 1;
                        image = ImageStreams.ReadImageFromStream(stream, owner: this);
                    }

                    stream.ReadExactly(Bytes.Of(ref reference, count: 4));
                    SetImageReference(index, value: reference);
                    stream.ReadBuffer(Bytes.Of(ref index, count: 4));
                }
            }
            else
            {
                if ((double)header.Version == 3)
                {
                    stream.ReadBuffer(Bytes.Of(ref index, count: 4));
                    while (index != -1)
                    {
                        SetImage(image: ImageStreams.ReadImageFromStream(stream, owner: this), index: index);
                        stream.ReadExactly(Bytes.Of(ref reference, count: 4));
                        SetImageReference(index, value: reference);
                        stream.ReadBuffer(Bytes.Of(ref index, count: 4));
                    }
                }
            }
        }
        else
        {
            DataOffset = header.IndexEnd + startPosition;
            MemoryStream milliseconds = new();
            try
            {
                milliseconds.SetLength(header.IndexEnd - header.HeaderSize);
                stream.ReadBuffer(milliseconds.GetBuffer().AsSpan(length: (int)milliseconds.Length, start: 0));
                PaletteManager.LoadFromStream(milliseconds);
                milliseconds.ReadBuffer(Bytes.Of(ref index, count: 4));
                while (index != -1)
                {
                    SetImage(image: ImageStreams.ReadImageFromStream(milliseconds, dataStream: stream, owner: this), index: index);
                    _ = milliseconds.Read(Bytes.Of(ref reference, count: 4));
                    SetImageReference(index, value: reference);
                    milliseconds.ReadBuffer(Bytes.Of(ref index, count: 4));
                }
            }
            finally
            {
                milliseconds?.Dispose();
            }

            if (!Status.Contains(ImageLibraryStatusElement.LoadingFromFile))
            {
                _ = stream.Seek(header.Size + startPosition, origin: SeekOrigin.Begin);
            }
        }

        _changeLock = (byte)(_changeLock - 1);
        if (_changeLock == 0)
        {
            CallOnChangeEvent();
        }

        Status -= Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Loading);
    }

    // ImageLib.pas:10038
    public virtual void SaveToStream(Stream stream)
    {
        ImageLibraryHeader header = default;
        int startPosition = (int)stream.Position;
        string swapName = $"{ExtractFilePath(Application.ExeName)}\\SWAP.$$$";
        Stream swap = OpenFile(swapName, mode: 65535);
        try
        {
            Status += Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Saving);
            header.HeaderSize = 24;
            header.Id = "ILB\0";
            header.Version = 4;
            stream.Write(Bytes.Of(ref header, count: 24));
            PaletteManager.SaveToStream(stream);
            int i = 0;
            int last = _count - 1;
            for (; i <= last; i += 1)
            {
                if (Images[i].Image is not null || Images[i].Reference != -1)
                {
                    stream.Write(Bytes.Of(ref i, count: 4));
                    LibraryImage.WriteImageToStream(Images[i].Image, stream, dataStream: swap);
                    int reference = Images[i].Reference;
                    stream.Write(Bytes.Of(ref reference, count: 4));
                    Images[i].Reference = reference;
                }
            }

            i = -1;
            stream.Write(Bytes.Of(ref i, count: 4));
            long position = stream.Position;
            header.IndexEnd = (int)position - startPosition;
            swap.Position = 0;
            _ = stream.CopyFrom(swap, count: (int)swap.Length);
            long streamPosition = stream.Position;
            header.Size = (int)streamPosition - startPosition;
            stream.Position = startPosition;
            stream.Write(Bytes.Of(ref header, count: 24));
            stream.Position = startPosition + header.Size;
        }
        finally
        {
            Status -= Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Saving);
            swap?.Dispose();
            _ = DeleteFile(swapName);
        }
    }

    // ImageLib.pas:10079
    public virtual void LoadFromFile(string fileName)
    {
        Stream stream = new FastLoadFileStream(fileName);
        try
        {
            _changeLock = (byte)(_changeLock + 1);
            Status += Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.LoadingFromFile);
            FileName = ExpandFileName(fileName);
            LoadedFileName = GetFullFileName();
            LoadFromStream(stream);
        }
        finally
        {
            Status -= Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.LoadingFromFile);
            stream?.Dispose();
            _changeLock = (byte)(_changeLock - 1);
            if (_changeLock == 0)
            {
                CallOnChangeEvent();
            }
        }
    }

    // ImageLib.pas:10098
    public virtual void SaveToFile(string fileName)
    {
        string name = ExpandFileName(fileName);
        if (name == LoadedFileName)
        {
            DisableReloading();
        }

        Status += Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.SavingToFile);
        Stream stream = OpenFile(name, mode: 65535);
        try
        {
            SaveToStream(stream);
        }
        finally
        {
            Status -= Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.SavingToFile);
            stream?.Dispose();
        }
    }

    // ImageLib.pas:9798
    public virtual void CallOnChangeEvent()
    {
        if (_changeLock == 0 && OnChange is not null && !ComponentState.Contains(Rtl.ComponentState.Destroying) && !ComponentState.Contains(Rtl.ComponentState.Reading))
        {
            _changeLock = (byte)(_changeLock + 1);
            OnChange(this);
            _changeLock = (byte)(_changeLock - 1);
        }
    }

    // ImageLib.pas:10152
    public virtual CustomImageLibrary Copy()
    {
        MemoryStream stream = new();
        CustomImageLibrary result;
        try
        {
            // The Delphi constructor makes the runtime class, so a library of another class
            // copies as itself. The port builds the base Component instead, so the cast back
            // always throws. No caller stores that throw today.
            result = (CustomImageLibrary)new Component(null);
            SaveToStream(stream);
            stream.Position = 0;
            result.LoadFromStream(stream);
            result.Reloading = false;
        }
        finally
        {
            stream.Dispose();
        }

        return result;
    }

    // ImageLib.pas:9769
    public int ReduceCount()
    {
        while (_count > 0 && Get(_count - 1) is null)
        {
            _count -= 1;
        }

        return _count;
    }

    // ImageLib.pas:9790
    public void UpdateImageItemSettings(int index)
    {
        LibraryImage image = Images[index].Image;
        if (image is ImageNode node)
        {
            node.UpdateDimensions();
        }
    }

    // ImageLib.pas:9809
    public void DisableOnChangeEvent() => _changeLock = (byte)(_changeLock + 1);

    // ImageLib.pas:9814
    public void EnableOnChangeEvent()
    {
        _changeLock = (byte)(_changeLock - 1);
        if (_changeLock == 0)
        {
            CallOnChangeEvent();
        }
    }

    // ImageLib.pas:9843
    public void DisableReloading()
    {
        for (int i = 0, last = _count - 1; i <= last; i++)
        {
            Images[i].Image?.SetReloadingEnabled(false);
        }

        Reloading = false;
    }

    // ImageLib.pas:9852
    public void ReleaseDiscardableImageData()
    {
        if (Reloading)
        {
            for (int i = 0, last = _count - 1; i <= last; i++)
            {
                LibraryImage image = Images[i].Image;
                image?.ReleaseDiscardableImageData();
            }
        }
    }

    // ImageLib.pas:9865
    public void LoadDiscardableImageData()
    {
        for (int i = 0, last = _count - 1; i <= last; i++)
        {
            LibraryImage image = Images[i].Image;
            image?.LoadDiscardableImageData();
        }
    }

    // ImageLib.pas:9884
    public bool IsReference(int index) => index >= 0 && index < _count && Images[index].Image is null && Images[index].Reference != -1;

    // ImageLib.pas:9922
    public bool ImageExist(int index)
    {
        bool result;
        if (index < _count && index >= 0)
        {
            result = Images[index].Image is not null;
            if (!result)
            {
                int r = Images[index].Reference;
                return r >= 0 && r < _count && Images[r].Image is not null;
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // ImageLib.pas:9942
    public int GetImageReference(int index) => Images[index].Reference;

    // ImageLib.pas:9947
    public void SetImageReference(int index, int value)
    {
        if (index >= _count)
        {
            SetCount(index + 1);
        }

        if (value != -1)
        {
            Images[index].Image?.Dispose();

            Images[index].Image = null;
        }

        Images[index].Reference = value;
        UpdateImageItemSettings(index);
        Modified = true;
    }

    // ImageLib.pas:10115
    public void LoadFromFileStream(string fileName, Stream stream)
    {
        Status += Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.LoadingFromFile);
        try
        {
            FileName = ExpandFileName(fileName);
            LoadedFileName = GetFullFileName();
            LoadFromStream(stream);
        }
        finally
        {
            Status -= Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.LoadingFromFile);
        }
    }

    // ImageLib.pas:10127
    public void CopyToClipboard()
        /* TODO: Implement Clipbrd.SetComponent in the platform or editor service. */
        => throw new NotImplementedException();

    // ImageLib.pas:10132
    public void PasteFromClipboard()
        /* TODO: Implement Clipbrd.GetComponent in the platform or editor service. */
        => throw new NotImplementedException();

    // ImageLib.pas:10168
    public override void Assign(Persistent source)
    {
        if (source is CustomImageLibrary library)
        {
            MemoryStream stream = new();
            try
            {
                library.SaveToStream(stream);
                stream.Position = 0;
                LoadFromStream(stream);
                _fileName = library._fileName;
                LoadedFileName = library.LoadedFileName;
                if (_changeLock == 0)
                {
                    CallOnChangeEvent();
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }
        else
        {
            throw new FormatException("Can only assign to a TCustomImageLibrary descendant");
        }
    }

    // ImageLib.pas:10190
    public void Delete(int index)
    {
        if (index < _count)
        {
            Images[index].Image?.Dispose();

            DeleteItem(index);
            if (_changeLock == 0)
            {
                CallOnChangeEvent();
            }
        }
    }

    // ImageLib.pas:10200
    public int Remove(LibraryImage image)
    {
        int result = IndexOf(image);
        if (result != -1)
        {
            Images[result].Image?.Dispose();

            DeleteItem(result);
            if (_changeLock == 0)
            {
                CallOnChangeEvent();
            }
        }

        return result;
    }

    // ImageLib.pas:10211
    public int IndexOf(LibraryImage image)
    {
        int result = 0;
        while (result < _count && image != Images[result].Image)
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    // ImageLib.pas:10218
    public LibraryImage GetImage(int index)
    {
        LibraryImage result;
        if (Images[index].Image is not null)
        {
            result = Images[index].Image;
            result.SetOwner(null);
            Images[index].Image = null;
            Modified = true;
            if (_changeLock == 0)
            {
                CallOnChangeEvent();
            }
        }
        else
        {
            return null;
        }

        return result;
    }

    // ImageLib.pas:10232
    public void SetImage(int index, LibraryImage image)
    {
        if (index >= _count)
        {
            SetCount(index + 1);
        }

        Images[index].Image?.Dispose();

        Images[index].Image = image;
        Images[index].Reference = -1;
        UpdateImageItemSettings(index);
        image?.SetOwner(this);

        Modified = true;
        if (_changeLock == 0)
        {
            CallOnChangeEvent();
        }
    }

    // ImageLib.pas:10244
    public void Insert(int index, LibraryImage image)
    {
        if (index < 0)
        {
            index = 0;
        }

        if (index < _count)
        {
            InsertItem(index);
        }

        SetImage(index, image);
    }

    // ImageLib.pas:10251
    public void Add(LibraryImage image) => SetImage(_count, image);

    // ImageLib.pas:10256
    public bool Empty()
    {
        int i = 0;
        while (i != _count && Images[i].Image is null)
        {
            i += 1;
        }

        return i == _count;
    }

    // ImageLib.pas:10265
    public void Pack()
    {
        DisableOnChangeEvent();
        for (int i = _count - 1; i >= 0; i--)
        {
            if (Get(i) is null)
            {
                Delete(i);
            }
        }

        EnableOnChangeEvent();
    }

    // ImageLib.pas:10275
    public int GetSubCount(int index)
    {
        LibraryImage image = GetSafe(index);
        return image is ImageNode node ? node.Count : image is not null ? 1 : -1;
    }

    // ImageLib.pas:10291
    public LibraryImage GetSub(int index, int subIndex)
    {
        LibraryImage image = Images[index].Image;
        if (image is null)
        {
            index = Images[index].Reference;
            if (index != -1)
            {
                image = Images[index].Image;
            }
        }

        return image is ImageNode node ? node.Images[subIndex] : image is not null && subIndex == 0 ? image : null;
    }

    // ImageLib.pas:10314
    public LibraryImage GetSafeSub(int index, int subIndex)
    {
        LibraryImage image = GetSafe(index);
        return image is ImageNode node
            ? node.GetSafeImage(subIndex)
            : image is not null && subIndex == 0 ? image : null;
    }

    // ImageLib.pas:10332
    public LibraryImage GetSubImage(int index, int subIndex)
    {
        LibraryImage result = Images[index].Image;
        if (result is ImageNode node)
        {
            if (subIndex >= 0 && subIndex < node.Count)
            {
                result = node.Images[subIndex];
                result.SetOwner(null);
                node.Delete(subIndex);
                if (node.Count == 1)
                {
                    LibraryImage image = node.GetImage(0);
                    Images[index].Image = image;
                    node.Delete(0);
                    node?.Dispose();
                }

                Modified = true;
            }
            else
            {
                return null;
            }
        }
        else if (result is not null)
        {
            if (subIndex == 0)
            {
                result.SetOwner(null);
                Images[index].Image = null;
                Modified = true;
            }
            else
            {
                return null;
            }
        }

        return result;
    }

    // ImageLib.pas:10367
    public void SetSubImage(int index, int subIndex, LibraryImage image)
    {
        LibraryImage old = index < _count ? Images[index].Image : null;

        if (old is ImageNode node)
        {
            if (subIndex >= 0 && subIndex < node.Count)
            {
                old = node.Images[subIndex];
                old?.Dispose();

                node.Images[subIndex] = image;
                image.SetOwner(this);
                Modified = true;
            }
            else
            {
                _ = node.Add(image);
                image.SetOwner(this);
                Modified = true;
            }
        }
        else if (old is not null)
        {
            if (subIndex != 0)
            {
                ImageNode imageNode = new(this);
                _ = imageNode.Add(old);
                _ = imageNode.Add(image);
                Images[index].Image = imageNode;
                image.SetOwner(this);
                Modified = true;
            }
            else
            {
                SetImage(index, image);
            }
        }
        else
        {
            SetImage(index, image);
        }
    }

    // ImageLib.pas:10414
    // { DeleteSub: the test of the node class is inverted (an original bug): a single image is treated
    //   as a node, and a node is freed whole when SubIndex is 0. }
    public void DeleteSub(int index, int subIndex)
    {
        // The cast stays: a single image throws here, as the port records the original inverted
        // node test, and a pattern test would silently skip it instead.
        ImageNode node = (ImageNode)Images[index].Image;
        if (node is not null)
        {
            if (node.ImageClass.ClassId != 256)
            {
                node.GetImage(subIndex).Dispose();
                node.Delete(subIndex);
                if (node.Count == 1)
                {
                    Images[index].Image = node.Images[0];
                    node.Delete(0);
                    node.Dispose();
                }

                Modified = true;
            }
            else
            {
                if (subIndex == 0)
                {
                    node?.Dispose();
                    Images[index].Image = null;
                }
            }
        }
    }

    // ImageLib.pas:10439
    public int RemoveSub(int index, LibraryImage image)
    {
        LibraryImage old = Images[index].Image;
        int result;
        if (old is ImageNode node)
        {
            result = node.IndexOf(image);
            if (result != -1)
            {
                DeleteSub(index, subIndex: result);
            }
        }
        else if (old is not null)
        {
            if (old == image)
            {
                old?.Dispose();
                Images[index].Image = null;
                Modified = true;
                return 0;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }

        return result;
    }

    // ImageLib.pas:10465
    public int IndexOfSub(int index, LibraryImage image)
    {
        LibraryImage old = Images[index].Image;
        return old is ImageNode node ? node.IndexOf(image) : old is not null && old == image ? 0 : -1;
    }

    // ImageLib.pas:10483
    public void InsertSub(int index, int subIndex, LibraryImage image)
    {
        LibraryImage old = Images[index].Image;
        if (old is ImageNode node)
        {
            node.Insert(subIndex, image);
            image.SetOwner(this);
            Modified = true;
        }
        else if (old is not null)
        {
            ImageNode imageNode = new(this);
            _ = imageNode.Add(old);
            Images[index].Image = imageNode;
            imageNode.Insert(subIndex, image);
            image.SetOwner(this);
            Modified = true;
        }
        else
        {
            AddSub(index, image);
        }
    }

    // ImageLib.pas:10511
    public void AddSub(int index, LibraryImage image)
    {
        LibraryImage old = Images[index].Image;
        if (old is ImageNode node)
        {
            _ = node.Add(image);
            image.SetOwner(this);
            Modified = true;
        }
        else if (old is not null)
        {
            ImageNode imageNode = new(this);
            _ = imageNode.Add(old);
            Images[index].Image = imageNode;
            image.SetOwner(this);
            Modified = true;
        }
        else
        {
            SetImage(index, image);
        }
    }

    // ImageLib.pas:10538
    public bool SubImageExist(int index, int subIndex)
    {
        LibraryImage image = Images[index].Image;
        return image is not null && subIndex >= 0 && (image is ImageNode node ? subIndex < node.Count : subIndex == 0);
    }

    public int Count { get => _count; set => SetCount(value); }

    public LibraryImage this[int index] => Get(index);
    public Set<ImageLibraryStatusElement> Status { get; private set; }
    public PaletteManager PaletteManager { get; }
    public string FullFileName => GetFullFileName();
    public string LoadedFileName { get; private set; } = "";
    public int DataOffset { get; private set; }
    public bool Reloading { get; private set; }
    public bool LoadOnDemand { get => _loadOnDemand; set => SetLoadOnDemand(value); }
    public string FileName { get => _fileName; set => SetFileName(value); }

    internal static int UpdateImage(CustomImageLibrary imageLibrary, LibraryImage image)
    {
        int result = 0;
        if (image is not null)
        {
            for (int i = 0, last = imageLibrary.Count - 1; i <= last; i++)
            {
                if (!imageLibrary.IsReference(i))
                {
                    for (int j = 0, lastJ = imageLibrary.GetSubCount(i) - 1; j <= lastJ; j++)
                    {
                        LibraryImage sub = imageLibrary.GetSub(i, subIndex: j);
                        if (sub is not null && CompareText(sub.FileName, image.FileName) == 0 && sub.FileIndex == image.FileIndex)
                        {
                            LibraryImage newImage = image.Copy();
                            newImage.Xshift = sub.Xshift;
                            newImage.Yshift = sub.Yshift;
                            imageLibrary.SetSubImage(i, subIndex: j, image: newImage);
                            result += 1;
                        }
                    }
                }
            }
        }

        return result;
    }

    public struct ImageItem
    {
        public LibraryImage Image { get; set; }

        public int Reference { get; internal set; }
    }
}
