// Translated from ImageLib.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw.Images;

// an animation of the images of a library, one layer of a chain sorted by layer. FOwner,
// FLayer, FNext (the next layer), FImageLibrary (linked by LinkToIL: FImageLibraryName from the
// manager of FOwner, or the library of FOwner), FImageLibraryName, FSequenceSpeed (frames per
// image - 1), FSequenceMode, FFrameTable (the image indices of smFrameTable and
// smRndFrameTable), FImageIndex (the image of smNone, the random start of smRandom and
// smRndFrameTable), _offsetX, _offsetY, _lastFrame, _speedCount, _frameCounter
public partial class ImageSequence : IDisposable
{
    internal ImageSequenceList _owner;
    internal byte _layer;
    internal ImageSequence _next;
    internal CustomImageLibrary _imageLibrary;
    internal string _imageLibraryName = "";
    internal byte _sequenceSpeed;
    internal SequenceMode _sequenceMode;
    internal FrameTableList _frameTable;
    internal int _imageIndex;
    internal int _offsetX;
    internal int _offsetY;
    internal int _lastFrame;
    internal byte _speedCount;
    internal int _frameCounter;

    // ImageLib.pas:8606
    internal void SetImageLibraryName(string value)
    {
        ReleaseImageLibrary();
        _imageLibraryName = value;
        if (_owner._linked)
        {
            LinkToImageLibrary();
        }
    }

    // ImageLib.pas:8613
    internal void SetSequenceMode(SequenceMode value)
    {
        if (Set<SequenceMode>.Of(SequenceMode.FrameTable, SequenceMode.RandomFrameTable).Contains(value) && _frameTable == null)
        {
            _frameTable = new FrameTableList();
        }

        _sequenceMode = value;
    }

    // ImageLib.pas:8553
    public ImageSequence(ImageSequenceList owner)
    {
        _owner = owner;
    }

    // ImageLib.pas:8559
    public virtual void Dispose()
    {
        _frameTable?.Dispose();
        ReleaseImageLibrary();
        if (_next != null)
        {
            _next.Dispose();
        }
    }

    // ImageLib.pas:8567
    public ImageSequence Copy(ImageSequenceList owner)
    {
        ImageSequence result = default;
        MemoryStream stream;
        stream = new MemoryStream();
        try
        {
            result = new ImageSequence(owner);
            SaveToStream(stream);
            stream.Position = 0;
            result.LoadFromStream(stream);
        }
        finally
        {
            stream?.Dispose();
        }

        return result;
    }

    // ImageLib.pas:8582
    public LibraryImage GetPreviewImage()
    {
        LibraryImage result;
        int I;
        if (this != null)
        {
            result = null;
            if (_frameTable != null && _frameTable._count != 0)
            {
                result = _imageLibrary.GetSafe(_frameTable._list[0]);
            }

            if (_sequenceMode == SequenceMode.None)
            {
                result = _imageLibrary.GetSafe(_imageIndex);
            }

            if (result == null)
            {
                I = 0;
                while (I != _imageLibrary.Count && result == null)
                {
                    result = _imageLibrary[I];
                    I += 1;
                }
            }
        }
        else
        {
            result = null;
        }

        return result;
    }

    // ImageLib.pas:8620
    public void GetDimensions(ref Rect R)
    {
        LibraryImage image;
        Rect ir = default;
        R.Left = 0;
        R.Right = 0;
        R.Top = 0;
        R.Bottom = 0;
        if (_imageLibrary != null)
        {
            switch (_sequenceMode)
            {
                case SequenceMode.FrameTable:
                case SequenceMode.RandomFrameTable:
                    {
                        for (int I = 0, last = _frameTable.Count - 1; I <= last; I++)
                        {
                            var item = _frameTable[I];
                            image = _imageLibrary.GetSafe(item);
                            if (image != null)
                            {
                                image.GetVisualDimensions(ref ir);
                                UnionRect(ref R, R, ir);
                            }
                        }
                    }

                    break;
                case SequenceMode.Up:
                case SequenceMode.UpDown:
                case SequenceMode.Random:
                    {
                        for (int I = 0, last = _imageLibrary.Count - 1; I <= last; I++)
                        {
                            image = _imageLibrary.GetSafe(I);
                            if (image != null)
                            {
                                image.GetVisualDimensions(ref ir);
                                UnionRect(ref R, R, ir);
                            }
                        }
                    }

                    break;
                case SequenceMode.None:
                    image = _imageLibrary.GetSafe(_imageIndex);
                    if (image != null)
                    {
                        image.GetVisualDimensions(ref R);
                    }

                    break;
            }

            R.Left += _offsetX;
            R.Right += _offsetX;
            R.Top += _offsetY;
            R.Bottom += _offsetY;
        }
    }

    // ImageLib.pas:8666
    public int GetTotalSequenceFrames()
    {
        int result;
        switch (_sequenceMode)
        {
            case SequenceMode.FrameTable:
                result = _frameTable.Count * (_sequenceSpeed + 1);
                break;
            case SequenceMode.Up:
                result = _imageLibrary.Count * (_sequenceSpeed + 1);
                break;
            case SequenceMode.UpDown:
                result = _imageLibrary.Count * (_sequenceSpeed + 1) * 2;
                break;
            default:
                result = _sequenceSpeed + 1;
                break;
        }

        return result;
    }

    // ImageLib.pas:8677
    public int GetFrameIndex(int frame, int offset)
    {
        int result;
        if (frame >= 0)
        {
            if (_sequenceMode != SequenceMode.None)
            {
                switch (_sequenceMode)
                {
                    case SequenceMode.FrameTable:
                        if (frame != _lastFrame)
                        {
                            if (_speedCount == 0)
                            {
                                _frameCounter = (_frameCounter + 1) & 16777215;
                                _speedCount = _sequenceSpeed;
                            }
                            else
                            {
                                _speedCount = (byte)(_speedCount - 1);
                            }

                            _lastFrame = frame;
                        }

                        result = _frameCounter + offset;
                        if (_frameTable.Count != 0)
                        {
                            result = result % _frameTable.Count;
                            result = _frameTable._list[result];
                        }
                        else
                        {
                            result = -1;
                        }

                        break;
                    case SequenceMode.Up:
                        if (frame != _lastFrame)
                        {
                            if (_speedCount == 0)
                            {
                                _frameCounter = (_frameCounter + 1) & 16777215;
                                _speedCount = _sequenceSpeed;
                            }
                            else
                            {
                                _speedCount = (byte)(_speedCount - 1);
                            }

                            _lastFrame = frame;
                        }

                        result = _frameCounter + offset;
                        result = result % _imageLibrary.Count;
                        break;
                    case SequenceMode.UpDown:
                        if (frame != _lastFrame)
                        {
                            if (_speedCount == 0)
                            {
                                _frameCounter = (_frameCounter + 1) & 16777215;
                                _speedCount = _sequenceSpeed;
                            }
                            else
                            {
                                _speedCount = (byte)(_speedCount - 1);
                            }

                            _lastFrame = frame;
                        }

                        result = (_frameCounter + offset) % ((_imageLibrary.Count - 1) * 2);
                        result = Math.Abs(result - (_imageLibrary.Count - 2));
                        break;
                    case SequenceMode.RandomFrameTable:
                        if (frame != _lastFrame && frame % (_sequenceSpeed + 1) == 0)
                        {
                            _imageIndex = Random(_frameTable.Count);
                            _lastFrame = frame;
                        }

                        if (_frameTable.Count != 0)
                        {
                            result = _frameTable._list[(_imageIndex + offset) % _frameTable.Count];
                        }
                        else
                        {
                            result = -1;
                        }

                        break;
                    case SequenceMode.Random:
                        if (frame != _lastFrame && frame % (_sequenceSpeed + 1) == 0)
                        {
                            _imageIndex = Random(_imageLibrary.Count);
                            _lastFrame = frame;
                        }

                        result = (_imageIndex + offset) % _imageLibrary.Count;
                        break;
                    default:
                        result = -1;
                        break;
                }
            }
            else
            {
                result = _imageIndex;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // ImageLib.pas:8768
    public int GetUnloopedFrameIndex(int frame)
    {
        int result;
        if (frame >= 0)
        {
            if (_sequenceMode != SequenceMode.None)
            {
                switch (_sequenceMode)
                {
                    case SequenceMode.FrameTable:
                        result = frame / (_sequenceSpeed + 1);
                        if (result < _frameTable.Count)
                        {
                            result = _frameTable._list[result];
                        }
                        else
                        {
                            result = -1;
                        }

                        break;
                    case SequenceMode.Up:
                        result = frame / (_sequenceSpeed + 1);
                        if (result >= _imageLibrary.Count)
                        {
                            result = -1;
                        }

                        break;
                    case SequenceMode.UpDown:
                        result = -1;
                        break;
                    case SequenceMode.RandomFrameTable:
                        result = -1;
                        break;
                    case SequenceMode.Random:
                        result = -1;
                        break;
                    default:
                        result = -1;
                        break;
                }
            }
            else
            {
                return _imageIndex;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // ImageLib.pas:8803
    public void LinkToImageLibrary()
    {
        if (_imageLibrary != null)
        {
            ReleaseImageLibrary();
        }

        if (!IsEmpty(_imageLibraryName))
        {
            _imageLibrary = _owner._imageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _imageLibraryName);
        }
        else
        {
            _imageLibrary = _owner._imageLibrary;
        }
    }

    // ImageLib.pas:8812
    public void ReleaseImageLibrary()
    {
        if (_imageLibrary != null)
        {
            if (!IsEmpty(_imageLibraryName))
            {
                _owner._imageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _imageLibraryName);
            }

            _imageLibrary = null;
        }
    }

    // ImageLib.pas:8822
    public void SaveToStream(Stream stream)
    {
        bool hasNext;
        LibraryDrawing.WriteStringToStream(_imageLibraryName, stream);
        stream.Write(Bytes.Of(ref _layer, 1));
        stream.Write(Bytes.Of(ref _sequenceSpeed, 1));
        stream.Write(Bytes.Of(ref _sequenceMode, 1));
        stream.Write(Bytes.Of(ref _imageIndex, 4));
        stream.Write(Bytes.Of(ref _offsetX, 4));
        stream.Write(Bytes.Of(ref _offsetY, 4));
        if (Set<SequenceMode>.Of(SequenceMode.FrameTable, SequenceMode.RandomFrameTable).Contains(_sequenceMode))
        {
            _frameTable.SaveToStream(stream);
        }

        hasNext = _next != null;
        stream.Write(Bytes.Of(ref hasNext, 1));
        if (hasNext)
        {
            _next.SaveToStream(stream);
        }
    }

    // ImageLib.pas:8839
    public void LoadFromStream(Stream stream)
    {
        bool hasNext = default;
        ReleaseImageLibrary();
        LibraryDrawing.ReadStringFromStream(ref _imageLibraryName, stream);
        stream.ReadExactly(Bytes.Of(ref _layer, 1));
        stream.ReadExactly(Bytes.Of(ref _sequenceSpeed, 1));
        stream.ReadExactly(Bytes.Of(ref _sequenceMode, 1));
        stream.ReadExactly(Bytes.Of(ref _imageIndex, 4));
        stream.ReadExactly(Bytes.Of(ref _offsetX, 4));
        stream.ReadExactly(Bytes.Of(ref _offsetY, 4));
        if (Set<SequenceMode>.Of(SequenceMode.FrameTable, SequenceMode.RandomFrameTable).Contains(_sequenceMode))
        {
            if (_frameTable == null)
            {
                _frameTable = new FrameTableList();
            }

            _frameTable.LoadFromStream(stream);
            _imageIndex = Random(_frameTable.Count);
        }

        stream.ReadExactly(Bytes.Of(ref hasNext, 1));
        if (hasNext)
        {
            _next = new ImageSequence(_owner);
            _next.LoadFromStream(stream);
        }
    }

    // ImageLib.pas:8865
    public void SaveToIniFile(string section, IniFile iniFile)
    {
        /* TODO: Implement IniFiles.WriteString in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:8880
    public void LoadFromIniFile(string section, IniFile iniFile)
    {
        /* TODO: Implement IniFiles.ReadString in the platform or editor service. */
        throw new NotImplementedException();
    }

    public ImageSequenceList Owner { get => _owner; }
    public byte Layer { get => _layer; set => _layer = value; }
    public ImageSequence Next { get => _next; set => _next = value; }
    public CustomImageLibrary ImageLibrary { get => _imageLibrary; }
    public string ImageLibraryName { get => _imageLibraryName; set => SetImageLibraryName(value); }
    public byte SequenceSpeed { get => _sequenceSpeed; set => _sequenceSpeed = value; }
    public byte SpeedCount { get => _speedCount; }
    public SequenceMode SequenceMode { get => _sequenceMode; set => SetSequenceMode(value); }
    public FrameTableList FrameTable { get => _frameTable; }
    public int ImageIndex { get => _imageIndex; set => _imageIndex = value; }
    public int OffsetX { get => _offsetX; set => _offsetX = value; }
    public int OffsetY { get => _offsetY; set => _offsetY = value; }

    internal static void BlitImageSequence(ImageSequence sequence, int displayController, int X, int Y, in Rect R, int frame, int bkColor)
    {
        LockedSurface lockedSurface = default;
        Dib16Surface surface;
        int index;
        surface = new Dib16Surface(R.Right - R.Left, R.Bottom - R.Top, PixelFormats.pfRGB565);
        surface.Clear(bkColor);
        surface.Lock(ref lockedSurface);
        while (sequence != null)
        {
            index = sequence.GetUnloopedFrameIndex(frame);
            if (index != -1 && sequence._imageLibrary != null && sequence._imageLibrary.GetSafe(index) != null)
            {
                sequence._imageLibrary[index].Show(X + sequence._offsetX, Y + sequence._offsetY, lockedSurface);
            }

            sequence = sequence._next;
        }

        surface.Unlock();
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        surface?.Dispose();
    }

}
