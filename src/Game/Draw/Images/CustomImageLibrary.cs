#nullable disable
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
            Images = ReallocArray(Images, newCapacity);
            if (newCapacity > Capacity)
            {
                ClearElements(Images, Capacity, newCapacity - Capacity);
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
        string result;
        string drive;
        string path;
        if (!IsEmpty(_fileName))
        {
            path = ComponentState.Contains(Rtl.ComponentState.Designing)
                ? ExtractFilePath(ToolServices.GetProjectName())
                : ImageLibraries.BaseImageLibraryPath;

            drive = ExtractFileDrive(_fileName);
            result = IsEmpty(drive) || drive == ExtractFileDrive(path) ? ExpandFileName(path + _fileName) : _fileName;
        }
        else
        {
            result = "";
        }

        return result;
    }

    // ImageLib.pas:9743
    internal void SetFileName(string value)
    {
        string path = ComponentState.Contains(Rtl.ComponentState.Designing)
            ? ExtractFilePath(ToolServices.GetProjectName())
            : ImageLibraries.BaseImageLibraryPath;
        value = ExtractRelativePath(path, value);
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
                    ShowMessage("Error loading: " + GetFullFileName());
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
        MoveElements(Images, index + 1, Images, index, _count - index - 1);
        ClearElements(Images, _count - 1, 1);
        _count -= 1;
        SetModified(true);
    }

    // ImageLib.pas:9783
    internal void InsertItem(int index)
    {
        SetCount(_count + 1);
        MoveElements(Images, index, Images, index + 1, _count - index - 1);
        ClearElements(Images, index, 1);
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
    public override void DefineProperties(Filer filer) =>
        /* TODO: Implement Classes.DefineBinaryProperty in the platform or editor service. */
        throw new NotImplementedException();

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
    public virtual bool GetModified() => Status.Contains(ImageLibraryStatusElement.Modified);

    // ImageLib.pas:9691
    public virtual void SetModified(bool value)
    {
        Status = value
            ? Status + Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Modified)
            : Status - Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Modified);
    }

    // ImageLib.pas:9826
    public virtual void Clear()
    {
        for (int i = _count - 1; i >= 0; i--)
        {
            if (Images[i].Image != null)
            {
                Images[i].Image?.Dispose();
            }
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
        int startPosition;
        MemoryStream milliseconds;
        ImageLibraryHeader header = default;
        int subIndex;
        LibraryImage image;
        Status += Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Loading);
        _changeLock = (byte)(_changeLock + 1);
        Clear();
        Reloading = Status.Contains(ImageLibraryStatusElement.LoadingFromFile) && _loadOnDemand;
        startPosition = (int)stream.Position;
        stream.ReadExactly(Bytes.Of(ref header, 24));
        _ = stream.Seek(header.HeaderSize + startPosition, SeekOrigin.Begin);
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
                stream.ReadBuffer(Bytes.Of(ref index, 4));
                while (index != -1)
                {
                    subIndex = 0;
                    image = ImageStreams.ReadImageFromStream(stream, this);
                    while (image != null)
                    {
                        SetSubImage(index, subIndex, image);
                        subIndex += 1;
                        image = ImageStreams.ReadImageFromStream(stream, this);
                    }

                    stream.ReadExactly(Bytes.Of(ref reference, 4));
                    SetImageReference(index, reference);
                    stream.ReadBuffer(Bytes.Of(ref index, 4));
                }
            }
            else
            {
                if ((double)header.Version == 3)
                {
                    stream.ReadBuffer(Bytes.Of(ref index, 4));
                    while (index != -1)
                    {
                        SetImage(image: ImageStreams.ReadImageFromStream(stream, this), index: index);
                        stream.ReadExactly(Bytes.Of(ref reference, 4));
                        SetImageReference(index, reference);
                        stream.ReadBuffer(Bytes.Of(ref index, 4));
                    }
                }
            }
        }
        else
        {
            DataOffset = header.IndexEnd + startPosition;
            milliseconds = new MemoryStream();
            try
            {
                milliseconds.SetLength(header.IndexEnd - header.HeaderSize);
                stream.ReadBuffer(milliseconds.GetBuffer().AsSpan(length: (int)milliseconds.Length, start: 0));
                PaletteManager.LoadFromStream(milliseconds);
                milliseconds.ReadBuffer(Bytes.Of(ref index, 4));
                while (index != -1)
                {
                    SetImage(image: ImageStreams.ReadImageFromStream(milliseconds, stream, this), index: index);
                    _ = milliseconds.Read(Bytes.Of(ref reference, 4));
                    SetImageReference(index, reference);
                    milliseconds.ReadBuffer(Bytes.Of(ref index, 4));
                }
            }
            finally
            {
                milliseconds?.Dispose();
            }

            if (!Status.Contains(ImageLibraryStatusElement.LoadingFromFile))
            {
                _ = stream.Seek(header.Size + startPosition, SeekOrigin.Begin);
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
        Stream swap;
        int startPosition;
        string swapName;
        int i;
        ImageLibraryHeader header = default;
        startPosition = (int)stream.Position;
        swapName = ExtractFilePath(RtlUnit.Application.ExeName) + "\\SWAP.$$$";
        swap = OpenFile(swapName, 65535);
        try
        {
            Status += Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.Saving);
            header.HeaderSize = 24;
            header.Id = "ILB\0";
            header.Version = 4;
            stream.Write(Bytes.Of(ref header, 24));
            PaletteManager.SaveToStream(stream);
            i = 0;
            int last = _count - 1;
            for (; i <= last; i += 1)
            {
                if (Images[i].Image != null || Images[i].Reference != -1)
                {
                    stream.Write(Bytes.Of(ref i, 4));
                    LibraryImage.WriteImageToStream(Images[i].Image, stream, swap);
                    int reference = Images[i].Reference;
                    stream.Write(Bytes.Of(ref reference, 4));
                    Images[i].Reference = reference;
                }
            }

            i = -1;
            stream.Write(Bytes.Of(ref i, 4));
            long position = stream.Position;
            header.IndexEnd = (int)position - startPosition;
            swap.Position = 0;
            _ = stream.CopyFrom(swap, (int)swap.Length);
            long streamPosition = stream.Position;
            header.Size = (int)streamPosition - startPosition;
            stream.Position = startPosition;
            stream.Write(Bytes.Of(ref header, 24));
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
        Stream stream;
        stream = new FastLoadFileStream(fileName);
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
        Stream stream;
        string name;
        name = ExpandFileName(fileName);
        if (name == LoadedFileName)
        {
            DisableReloading();
        }

        Status += Set<ImageLibraryStatusElement>.Of(ImageLibraryStatusElement.SavingToFile);
        stream = OpenFile(name, 65535);
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
        if (_changeLock == 0 && OnChange != null && !ComponentState.Contains(Rtl.ComponentState.Destroying) && !ComponentState.Contains(Rtl.ComponentState.Reading))
        {
            _changeLock = (byte)(_changeLock + 1);
            OnChange(this);
            _changeLock = (byte)(_changeLock - 1);
        }
    }

    // ImageLib.pas:10152
    public virtual CustomImageLibrary Copy()
    {
        CustomImageLibrary result = default;
        MemoryStream stream;
        stream = new MemoryStream();
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
        while (_count > 0 && Get(_count - 1) == null)
        {
            _count -= 1;
        }

        return _count;
    }

    // ImageLib.pas:9790
    public void UpdateImageItemSettings(int index)
    {
        LibraryImage image;
        image = Images[index].Image;
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
        LibraryImage image;
        if (Reloading)
        {
            for (int i = 0, last = _count - 1; i <= last; i++)
            {
                image = Images[i].Image;
                image?.ReleaseDiscardableImageData();
            }
        }
    }

    // ImageLib.pas:9865
    public void LoadDiscardableImageData()
    {
        LibraryImage image;
        for (int i = 0, last = _count - 1; i <= last; i++)
        {
            image = Images[i].Image;
            image?.LoadDiscardableImageData();
        }
    }

    // ImageLib.pas:9884
    public bool IsReference(int index) => index >= 0 && index < _count && Images[index].Image == null && Images[index].Reference != -1;

    // ImageLib.pas:9922
    public bool ImageExist(int index)
    {
        bool result;
        int r;
        if (index < _count && index >= 0)
        {
            result = Images[index].Image != null;
            if (!result)
            {
                r = Images[index].Reference;
                result = r >= 0 && r < _count && Images[r].Image != null;
            }
        }
        else
        {
            result = false;
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
        SetModified(true);
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
    public void CopyToClipboard() =>
        /* TODO: Implement Clipbrd.SetComponent in the platform or editor service. */
        throw new NotImplementedException();

    // ImageLib.pas:10132
    public void PasteFromClipboard() =>
        /* TODO: Implement Clipbrd.GetComponent in the platform or editor service. */
        throw new NotImplementedException();

    // ImageLib.pas:10168
    public override void Assign(Persistent source)
    {
        MemoryStream stream;
        if (source is CustomImageLibrary library)
        {
            stream = new MemoryStream();
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
            if (Images[index].Image != null)
            {
                Images[index].Image?.Dispose();
            }

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
            if (Images[result].Image != null)
            {
                Images[result].Image?.Dispose();
            }

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

        if (result == _count)
        {
            result = -1;
        }

        return result;
    }

    // ImageLib.pas:10218
    public LibraryImage GetImage(int index)
    {
        LibraryImage result;
        if (Images[index].Image != null)
        {
            result = Images[index].Image;
            result.SetOwner(null);
            Images[index].Image = null;
            SetModified(true);
            if (_changeLock == 0)
            {
                CallOnChangeEvent();
            }
        }
        else
        {
            result = null;
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

        SetModified(true);
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
        int i;
        i = 0;
        while (i != _count && Images[i].Image == null)
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
            if (Get(i) == null)
            {
                Delete(i);
            }
        }

        EnableOnChangeEvent();
    }

    // ImageLib.pas:10275
    public int GetSubCount(int index)
    {
        int result;
        LibraryImage image;
        image = GetSafe(index);
        result = image is ImageNode node ? node.Count : image != null ? 1 : -1;

        return result;
    }

    // ImageLib.pas:10291
    public LibraryImage GetSub(int index, int subIndex)
    {
        LibraryImage result;
        LibraryImage image;
        image = Images[index].Image;
        if (image == null)
        {
            index = Images[index].Reference;
            if (index != -1)
            {
                image = Images[index].Image;
            }
        }

        result = image is ImageNode node ? node.Images[subIndex] : image != null && subIndex == 0 ? image : null;

        return result;
    }

    // ImageLib.pas:10314
    public LibraryImage GetSafeSub(int index, int subIndex)
    {
        LibraryImage result;
        LibraryImage image;
        image = GetSafe(index);
        result = image is ImageNode node
            ? node.GetSafeImage(subIndex)
            : image != null && subIndex == 0 ? image : null;

        return result;
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

                SetModified(true);
            }
            else
            {
                result = null;
            }
        }
        else if (result != null)
        {
            if (subIndex == 0)
            {
                result.SetOwner(null);
                Images[index].Image = null;
                SetModified(true);
            }
            else
            {
                result = null;
            }
        }

        return result;
    }

    // ImageLib.pas:10367
    public void SetSubImage(int index, int subIndex, LibraryImage image)
    {
        LibraryImage old;
        old = index < _count ? Images[index].Image : null;

        if (old is ImageNode node)
        {
            if (subIndex >= 0 && subIndex < node.Count)
            {
                old = node.Images[subIndex];
                old?.Dispose();

                node.Images[subIndex] = image;
                image.SetOwner(this);
                SetModified(true);
            }
            else
            {
                _ = node.Add(image);
                image.SetOwner(this);
                SetModified(true);
            }
        }
        else if (old != null)
        {
            if (subIndex != 0)
            {
                ImageNode imageNode = new(this);
                _ = imageNode.Add(old);
                _ = imageNode.Add(image);
                Images[index].Image = imageNode;
                image.SetOwner(this);
                SetModified(true);
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
        ImageNode node;
        // The cast stays: a single image throws here, as the port records the original inverted
        // node test, and a pattern test would silently skip it instead.
        node = (ImageNode)Images[index].Image;
        if (node != null)
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

                SetModified(true);
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
        int result;
        LibraryImage old;
        old = Images[index].Image;
        if (old is ImageNode node)
        {
            result = node.IndexOf(image);
            if (result != -1)
            {
                DeleteSub(index, result);
            }
        }
        else if (old != null)
        {
            if (old == image)
            {
                old?.Dispose();
                Images[index].Image = null;
                SetModified(true);
                result = 0;
            }
            else
            {
                result = -1;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // ImageLib.pas:10465
    public int IndexOfSub(int index, LibraryImage image)
    {
        int result;
        LibraryImage old;
        old = Images[index].Image;
        result = old is ImageNode node ? node.IndexOf(image) : old != null && old == image ? 0 : -1;

        return result;
    }

    // ImageLib.pas:10483
    public void InsertSub(int index, int subIndex, LibraryImage image)
    {
        LibraryImage old;
        old = Images[index].Image;
        if (old is ImageNode node)
        {
            node.Insert(subIndex, image);
            image.SetOwner(this);
            SetModified(true);
        }
        else if (old != null)
        {
            ImageNode imageNode = new(this);
            _ = imageNode.Add(old);
            Images[index].Image = imageNode;
            imageNode.Insert(subIndex, image);
            image.SetOwner(this);
            SetModified(true);
        }
        else
        {
            AddSub(index, image);
        }
    }

    // ImageLib.pas:10511
    public void AddSub(int index, LibraryImage image)
    {
        LibraryImage old;
        old = Images[index].Image;
        if (old is ImageNode node)
        {
            _ = node.Add(image);
            image.SetOwner(this);
            SetModified(true);
        }
        else if (old != null)
        {
            ImageNode imageNode = new(this);
            _ = imageNode.Add(old);
            Images[index].Image = imageNode;
            image.SetOwner(this);
            SetModified(true);
        }
        else
        {
            SetImage(index, image);
        }
    }

    // ImageLib.pas:10538
    public bool SubImageExist(int index, int subIndex)
    {
        bool result;
        LibraryImage image;
        image = Images[index].Image;
        result = image != null && subIndex >= 0 && (image is ImageNode node ? subIndex < node.Count : subIndex == 0);

        return result;
    }

    public int Count { get => _count; set => SetCount(value); }

    public LibraryImage this[int index] => Get(index);

    public bool Modified { get => GetModified(); set => SetModified(value); }
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
        LibraryImage sub;
        LibraryImage newImage;
        int result = 0;
        if (image != null)
        {
            for (int i = 0, last = imageLibrary.Count - 1; i <= last; i++)
            {
                if (!imageLibrary.IsReference(i))
                {
                    for (int j = 0, lastJ = imageLibrary.GetSubCount(i) - 1; j <= lastJ; j++)
                    {
                        sub = imageLibrary.GetSub(i, j);
                        if (sub != null && CompareText(sub.FileName, image.FileName) == 0 && sub.FileIndex == image.FileIndex)
                        {
                            newImage = image.Copy();
                            newImage.Xshift = sub.Xshift;
                            newImage.Yshift = sub.Yshift;
                            imageLibrary.SetSubImage(i, j, newImage);
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
