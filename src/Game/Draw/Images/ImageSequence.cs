#nullable disable
// Translated from ImageLib.pas.
using Aow.Formats.Tree;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.RectRoutines;

namespace Aow.Game.Draw.Images;

// an animation of the images of a library, one layer of a chain sorted by layer. FOwner,
// FLayer, FNext (the next layer), FImageLibrary (linked by LinkToIL: FImageLibraryName from the
// manager of FOwner, or the library of FOwner), FImageLibraryName, FSequenceSpeed (frames per
// image - 1), FSequenceMode, FFrameTable (the image indices of smFrameTable and
// smRndFrameTable), FImageIndex (the image of smNone, the random start of smRandom and
// smRndFrameTable), _offsetX, _offsetY, _lastFrame, _speedCount, _frameCounter
public partial class ImageSequence(ImageSequenceList owner) : IDisposable
{
    private byte _layer;
    private string _imageLibraryName = "";
    private byte _sequenceSpeed;
    private SequenceMode _sequenceMode;
    private int _imageIndex;
    private int _offsetX;
    private int _offsetY;
    private int _lastFrame;
    private int _frameCounter;

    // ImageLib.pas:8606
    internal void SetImageLibraryName(string value)
    {
        ReleaseImageLibrary();
        _imageLibraryName = value;
        if (Owner.Linked)
        {
            LinkToImageLibrary();
        }
    }

    // ImageLib.pas:8613
    internal void SetSequenceMode(SequenceMode value)
    {
        if (Set<SequenceMode>.Of(SequenceMode.FrameTable, SequenceMode.RandomFrameTable).Contains(value) && FrameTable is null)
        {
            FrameTable = new FrameTableList();
        }

        _sequenceMode = value;
    }

    // ImageLib.pas:8559
    public virtual void Dispose()
    {
        FrameTable?.Dispose();
        ReleaseImageLibrary();
        Next?.Dispose();
    }

    // ImageLib.pas:8567
    public ImageSequence Copy(ImageSequenceList owner)
    {
        MemoryStream stream = new();
        ImageSequence result;
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
        if (this is not null)
        {
            result = null;
            if (FrameTable is not null && FrameTable.Count != 0)
            {
                result = ImageLibrary.GetSafe(FrameTable.List[0]);
            }

            if (_sequenceMode == SequenceMode.None)
            {
                result = ImageLibrary.GetSafe(_imageIndex);
            }

            if (result is null)
            {
                int i = 0;
                while (i != ImageLibrary.Count && result is null)
                {
                    result = ImageLibrary[i];
                    i += 1;
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
    public void GetDimensions(out Rect R)
    {
        // The out value is assigned before its members are written.
        R = default;
        if (ImageLibrary is not null)
        {
            Rect ir;
            LibraryImage image;
            switch (_sequenceMode)
            {
                case SequenceMode.FrameTable:
                case SequenceMode.RandomFrameTable:
                    {
                        for (int i = 0, last = FrameTable.Count - 1; i <= last; i++)
                        {
                            int item = FrameTable[i];
                            image = ImageLibrary.GetSafe(item);
                            if (image is not null)
                            {
                                image.GetVisualDimensions(out ir);
                                _ = UnionRect(out R, R, ir);
                            }
                        }
                    }

                    break;
                case SequenceMode.Up:
                case SequenceMode.UpDown:
                case SequenceMode.Random:
                    {
                        for (int i = 0, last = ImageLibrary.Count - 1; i <= last; i++)
                        {
                            image = ImageLibrary.GetSafe(i);
                            if (image is not null)
                            {
                                image.GetVisualDimensions(out ir);
                                _ = UnionRect(out R, R, ir);
                            }
                        }
                    }

                    break;
                case SequenceMode.None:
                    image = ImageLibrary.GetSafe(_imageIndex);
                    image?.GetVisualDimensions(out R);

                    break;
                default:
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
        int result = _sequenceMode switch
        {
            SequenceMode.FrameTable => FrameTable.Count * (_sequenceSpeed + 1),
            SequenceMode.Up => ImageLibrary.Count * (_sequenceSpeed + 1),
            SequenceMode.UpDown => ImageLibrary.Count * (_sequenceSpeed + 1) * 2,
            SequenceMode.None or SequenceMode.Random or SequenceMode.RandomFrameTable or _ => _sequenceSpeed + 1,
        };
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
                            if (SpeedCount == 0)
                            {
                                _frameCounter = (_frameCounter + 1) & 16777215;
                                SpeedCount = _sequenceSpeed;
                            }
                            else
                            {
                                SpeedCount = (byte)(SpeedCount - 1);
                            }

                            _lastFrame = frame;
                        }

                        result = _frameCounter + offset;
                        if (FrameTable.Count != 0)
                        {
                            result %= FrameTable.Count;
                            result = FrameTable.List[result];
                        }
                        else
                        {
                            result = -1;
                        }

                        break;
                    case SequenceMode.Up:
                        if (frame != _lastFrame)
                        {
                            if (SpeedCount == 0)
                            {
                                _frameCounter = (_frameCounter + 1) & 16777215;
                                SpeedCount = _sequenceSpeed;
                            }
                            else
                            {
                                SpeedCount = (byte)(SpeedCount - 1);
                            }

                            _lastFrame = frame;
                        }

                        result = _frameCounter + offset;
                        result %= ImageLibrary.Count;
                        break;
                    case SequenceMode.UpDown:
                        if (frame != _lastFrame)
                        {
                            if (SpeedCount == 0)
                            {
                                _frameCounter = (_frameCounter + 1) & 16777215;
                                SpeedCount = _sequenceSpeed;
                            }
                            else
                            {
                                SpeedCount = (byte)(SpeedCount - 1);
                            }

                            _lastFrame = frame;
                        }

                        result = (_frameCounter + offset) % ((ImageLibrary.Count - 1) * 2);
                        result = Math.Abs(result - (ImageLibrary.Count - 2));
                        break;
                    case SequenceMode.RandomFrameTable:
                        if (frame != _lastFrame && frame % (_sequenceSpeed + 1) == 0)
                        {
                            _imageIndex = Random(FrameTable.Count);
                            _lastFrame = frame;
                        }

                        result = FrameTable.Count != 0 ? FrameTable.List[(_imageIndex + offset) % FrameTable.Count] : -1;

                        break;
                    case SequenceMode.Random:
                        if (frame != _lastFrame && frame % (_sequenceSpeed + 1) == 0)
                        {
                            _imageIndex = Random(ImageLibrary.Count);
                            _lastFrame = frame;
                        }

                        result = (_imageIndex + offset) % ImageLibrary.Count;
                        break;
                    case SequenceMode.None:
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
                        result = result < FrameTable.Count ? FrameTable.List[result] : -1;

                        break;
                    case SequenceMode.Up:
                        result = frame / (_sequenceSpeed + 1);
                        if (result >= ImageLibrary.Count)
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
                    case SequenceMode.None:
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
        if (ImageLibrary is not null)
        {
            ReleaseImageLibrary();
        }

        ImageLibrary = !IsEmpty(_imageLibraryName)
            ? Owner.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _imageLibraryName)
            : Owner.ImageLibrary;
    }

    // ImageLib.pas:8812
    public void ReleaseImageLibrary()
    {
        if (ImageLibrary is not null)
        {
            if (!IsEmpty(_imageLibraryName))
            {
                Owner.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _imageLibraryName);
            }

            ImageLibrary = null;
        }
    }

    // ImageLib.pas:8822
    public void SaveToStream(Stream stream)
    {
        LibraryDrawing.WriteStringToStream(_imageLibraryName, stream);
        stream.Write(Bytes.Of(ref _layer, 1));
        stream.Write(Bytes.Of(ref _sequenceSpeed, 1));
        stream.Write(Bytes.Of(ref _sequenceMode, 1));
        stream.Write(Bytes.Of(ref _imageIndex, 4));
        stream.Write(Bytes.Of(ref _offsetX, 4));
        stream.Write(Bytes.Of(ref _offsetY, 4));
        if (Set<SequenceMode>.Of(SequenceMode.FrameTable, SequenceMode.RandomFrameTable).Contains(_sequenceMode))
        {
            FrameTable.SaveToStream(stream);
        }

        bool hasNext = Next is not null;
        stream.Write(Bytes.Of(ref hasNext, 1));
        if (hasNext)
        {
            Next.SaveToStream(stream);
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
            FrameTable ??= new FrameTableList();

            FrameTable.LoadFromStream(stream);
            _imageIndex = Random(FrameTable.Count);
        }

        stream.ReadExactly(Bytes.Of(ref hasNext, 1));
        if (hasNext)
        {
            Next = new ImageSequence(Owner);
            Next.LoadFromStream(stream);
        }
    }

    /// <summary>The layers of this chain as <see cref="SaveToStream"/> writes them.</summary>
    internal List<SequenceLayer> SaveLayers()
    {
        List<SequenceLayer> layers = [];
        for (ImageSequence sequence = this; sequence is not null; sequence = sequence.Next)
        {
            FrameTable frames = null;
            if (Set<SequenceMode>.Of(SequenceMode.FrameTable, SequenceMode.RandomFrameTable).Contains(sequence._sequenceMode))
            {
                frames = new FrameTable(Default: sequence.FrameTable.DefaultValue, Items: sequence.FrameTable.List.AsSpan(0, sequence.FrameTable.Count).ToArray());
            }

            layers.Add(new SequenceLayer
            {
                Library = sequence._imageLibraryName,
                Layer = sequence._layer,
                Speed = sequence._sequenceSpeed,
                Mode = (byte)sequence._sequenceMode,
                ImageIndex = sequence._imageIndex,
                OffsetX = sequence._offsetX,
                OffsetY = sequence._offsetY,
                Frames = frames,
            });
        }

        return layers;
    }

    /// <summary>
    /// <see cref="LoadFromStream"/> from the stored layer at <paramref name="index"/> and the
    /// layers after it. A frame-table layer draws its image index from <c>Random</c> after its
    /// frame table, before the next layer, as the stream load does.
    /// </summary>
    internal void LoadLayers(IReadOnlyList<SequenceLayer> layers, int index)
    {
        SequenceLayer stored = layers[index];
        ReleaseImageLibrary();
        _imageLibraryName = stored.Library;
        _layer = stored.Layer;
        _sequenceSpeed = stored.Speed;
        _sequenceMode = (SequenceMode)stored.Mode;
        _imageIndex = stored.ImageIndex;
        _offsetX = stored.OffsetX;
        _offsetY = stored.OffsetY;
        if (Set<SequenceMode>.Of(SequenceMode.FrameTable, SequenceMode.RandomFrameTable).Contains(_sequenceMode))
        {
            FrameTable ??= new FrameTableList();

            FrameTable.LoadItems(stored.Frames.Default, stored.Frames.Items);
            _imageIndex = Random(FrameTable.Count);
        }

        if (index + 1 < layers.Count)
        {
            Next = new ImageSequence(Owner);
            Next.LoadLayers(layers, index + 1);
        }
    }

    // ImageLib.pas:8865
    public void SaveToIniFile(string section, IniFile iniFile) =>
        /* TODO: Implement IniFiles.WriteString in the platform or editor service. */
        throw new NotImplementedException();

    // ImageLib.pas:8880
    public void LoadFromIniFile(string section, IniFile iniFile) =>
        /* TODO: Implement IniFiles.ReadString in the platform or editor service. */
        throw new NotImplementedException();

    public ImageSequenceList Owner { get; } = owner;
    public byte Layer { get => _layer; set => _layer = value; }
    public ImageSequence Next { get; set; }
    public CustomImageLibrary ImageLibrary { get; private set; }
    public string ImageLibraryName { get => _imageLibraryName; set => SetImageLibraryName(value); }
    public byte SequenceSpeed { get => _sequenceSpeed; set => _sequenceSpeed = value; }
    public byte SpeedCount { get; private set; }
    public SequenceMode SequenceMode { get => _sequenceMode; set => SetSequenceMode(value); }
    public FrameTableList FrameTable { get; private set; }
    public int ImageIndex { get => _imageIndex; set => _imageIndex = value; }
    public int OffsetX { get => _offsetX; set => _offsetX = value; }
    public int OffsetY { get => _offsetY; set => _offsetY = value; }

    internal static void BlitImageSequence(ImageSequence sequence, int X, int Y, in Rect R, int frame, int bkColor)
    {
        LockedSurface lockedSurface = default;
        Dib16Surface surface = new(R.Right - R.Left, R.Bottom - R.Top, PixelFormat.Rgb565);
        _ = surface.Clear(bkColor);
        _ = surface.Lock(ref lockedSurface);
        while (sequence is not null)
        {
            int index = sequence.GetUnloopedFrameIndex(frame);
            if (index != -1 && sequence.ImageLibrary is not null && sequence.ImageLibrary.GetSafe(index) is not null)
            {
                sequence.ImageLibrary[index].Show(X + sequence._offsetX, Y + sequence._offsetY, lockedSurface);
            }

            sequence = sequence.Next;
        }

        _ = surface.Unlock();
        /* TODO: Implement cell or array backing for this address. */
        throw new NotImplementedException();
        surface?.Dispose();
    }

}
