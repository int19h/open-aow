// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Audio;

using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// { Interface declaration run owned by P2. }
// Name. FaceIndex. FaceImageLibrary. FaceImageLibraryName. Sequences. RefCount. flags.
// SfxLibraries
public partial class UnitGraphicsResource : EngineResource
{
    public new const int ClassId = 131600;

    // UnitHS.PAS:1
    public UnitGraphicsResource(ECustomNode owner) : base(owner)
    {
        Sequences = new ImageSequenceList(null, null);
        _refCount = 0;
        var sfxLibrary = new SfxLibrary(null) { Holder = CreationHolder() };
        SfxLibraries[0] = sfxLibrary;
        var sfxLibrary2 = new SfxLibrary(null) { Holder = CreationHolder() };
        SfxLibraries[1] = sfxLibrary2;
        var sfxLibrary3 = new SfxLibrary(null) { Holder = CreationHolder() };
        SfxLibraries[2] = sfxLibrary3;
        var sfxLibrary4 = new SfxLibrary(null) { Holder = CreationHolder() };
        SfxLibraries[3] = sfxLibrary4;
        var sfxLibrary5 = new SfxLibrary(null) { Holder = CreationHolder() };
        SfxLibraries[4] = sfxLibrary5;
    }

    public string Name = "";
    private int _faceIndex;
    private CustomImageLibrary _faceImageLibrary;
    private string _faceImageLibraryName = "";

    public ImageSequenceList Sequences { get; }

    private int _refCount;
    public byte MoveAnimationSpeed;

    public SfxLibrary[] SfxLibraries { get; } = new SfxLibrary[5];

    // UnitHS.PAS:12
    public override void Dispose()
    {
        if (_refCount != 0)
        {
            ReleaseImageLibrary();
        }

        Sequences?.Dispose();
        SfxLibraries[0]?.Dispose();
        SfxLibraries[1]?.Dispose();
        SfxLibraries[2]?.Dispose();
        SfxLibraries[3]?.Dispose();
        SfxLibraries[4]?.Dispose();
        base.Dispose();
    }

    // UnitHS.PAS:116
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Payload(5, Sequences.LoadFromStream, Sequences.SaveToStream);
        if (!stream.StorageMode().Contains(EngineServices.Text))
        {
            stream.String(ref _faceImageLibraryName, 6);
            stream.String(ref Name, 7);
        }

        stream.Int32(ref _faceIndex, 8);
        stream.Bytes(Bytes.Of(ref MoveAnimationSpeed, 1), 1, 10);
        stream.Object(SfxLibraries[0], 15);
        stream.Object(SfxLibraries[1], 16);
        stream.Object(SfxLibraries[2], 17);
        stream.Object(SfxLibraries[3], 18);
        stream.Object(SfxLibraries[4], 19);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.ImageSequences(persister, name: "Sequences", list: Sequences);
        _faceImageLibraryName = persister.String("FaceImageLibraryName", value: _faceImageLibraryName);
        Name = persister.String("Name", value: Name);
        _faceIndex = persister.Int32("FaceIndex", value: _faceIndex);
        MoveAnimationSpeed = persister.RawByte("MoveAnimationSpeed", value: MoveAnimationSpeed);
        persister.Object("SelectSfxLibrary", objectValue: SfxLibraries[0]);
        persister.Object("MoveSfxLibrary", objectValue: SfxLibraries[1]);
        persister.Object("MeleeSfxLibrary", objectValue: SfxLibraries[2]);
        persister.Object("HitSfxLibrary", objectValue: SfxLibraries[3]);
        persister.Object("DeathSfxLibrary", objectValue: SfxLibraries[4]);
    }

    // UnitHS.PAS:66
    public override int AddRef()
    {
        if (_refCount == 0)
        {
            LinkToImageLibrary();
        }

        _refCount += 1;
        return _refCount;
    }

    // UnitHS.PAS:72
    public override int Release()
    {
        _refCount -= 1;
        if (_refCount == 0 && !GamePack.Current.Status.Contains(HexSpriteSetStatusElement.Editting) && (StrategicMap.Current == null || !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting)))
        {
            ReleaseImageLibrary();
        }

        return _refCount;
    }

    // UnitHS.PAS:31
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            Sequences.ImageLibraryManager = message.Payload.As<GameEngine>().HexSpriteSet.ImageLibraryManager;
        }
    }

    // UnitHS.PAS:88
    public override LibraryImage GetCellImage()
    {
        LibraryImage result;
        AddRef();
        if (Sequences.GetImageSequence(2) != null)
        {
            result = Sequences.GetImageSequence(2).GetPreviewImage();
        }
        else
        {
            result = null;
        }

        if (result == null)
        {
            result = Sequences.GetPreviewImage();
        }

        Release();
        return result;
    }

    // UnitHS.PAS:37
    public void LinkToImageLibrary()
    {
        Sequences.LinkToImageLibrary();
        if (!IsEmpty(_faceImageLibraryName))
        {
            _faceImageLibrary = Sequences.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _faceImageLibraryName);
            return;
        }

        _faceImageLibrary = null;
    }

    // UnitHS.PAS:47
    public void ReleaseImageLibrary()
    {
        Sequences.ReleaseImageLibrary();
        if (_faceImageLibrary != null)
        {
            if (!IsEmpty(_faceImageLibraryName))
            {
                Sequences.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _faceImageLibraryName);
            }

            _faceImageLibrary = null;
        }
    }

    // UnitHS.PAS:57
    public void SetFaceImageLibraryName(string value)
    {
        if (!IsEmpty(_faceImageLibraryName))
        {
            Sequences.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _faceImageLibraryName);
        }

        _faceImageLibraryName = value;
        // { Original releases the old library even when no reference is held. }
        if (!IsEmpty(_faceImageLibraryName) && _refCount != 0)
        {
            _faceImageLibrary = Sequences.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _faceImageLibraryName);
        }
    }

    // UnitHS.PAS:80
    public LibraryImage GetFaceImage()
    {
        LibraryImage result;
        AddRef();
        if (_faceImageLibrary != null)
        {
            result = _faceImageLibrary.GetSafe(_faceIndex);
        }
        else
        {
            result = null;
        }

        Release();
        return result;
    }

    // UnitHS.PAS:97
    public void Show(int direction, int X, int Y, in LockedSurface lockedSurface)
    {
        ImageSequence sequence;
        int index;
        AddRef();
        sequence = Sequences.List[direction - 1];
        while (sequence != null)
        {
            index = sequence.GetFrameIndex(1, 0);
            if (index != -1)
            {
                sequence.ImageLibrary[index].Show(X, Y, lockedSurface);
            }

            sequence = sequence.Next;
        }

        Release();
    }

    // UnitHS.PAS:112
    public int GetIndex() => EditIndex;

    // intf_P2_03.inc:22

}
