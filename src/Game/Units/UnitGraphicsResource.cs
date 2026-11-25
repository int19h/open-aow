#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Audio;

using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Units;

// { Interface declaration run owned by P2. }
// Name. FaceIndex. FaceImageLibrary. FaceImageLibraryName. Sequences. RefCount. flags.
// SfxLibraries
public sealed class UnitGraphicsResource : EngineResource
{
    public new const int ClassId = 131_600;

    // UnitHS.PAS:1
    public UnitGraphicsResource(CustomNode? owner) : base(owner)
    {
        Sequences = new ImageSequenceList(null, imageLibrary: null);
        _refCount = 0;
        SfxLibrary sfxLibrary = new(null) { Holder = CreationHolder() };
        SfxLibraries[0] = sfxLibrary;
        SfxLibrary sfxLibrary2 = new(null) { Holder = CreationHolder() };
        SfxLibraries[1] = sfxLibrary2;
        SfxLibrary sfxLibrary3 = new(null) { Holder = CreationHolder() };
        SfxLibraries[2] = sfxLibrary3;
        SfxLibrary sfxLibrary4 = new(null) { Holder = CreationHolder() };
        SfxLibraries[3] = sfxLibrary4;
        SfxLibrary sfxLibrary5 = new(null) { Holder = CreationHolder() };
        SfxLibraries[4] = sfxLibrary5;
    }

    public string Name { get; set; } = "";

    private int _faceIndex;
    private CustomImageLibrary? _faceImageLibrary;
    private string _faceImageLibraryName = "";

    public ImageSequenceList Sequences { get; }

    private int _refCount;

    public int MoveAnimationSpeed { get; set; }

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
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.ImageSequences(name: "Sequences", list: Sequences);
        // The original leaves both names out under the text-only mode of the translate stream.
        if (!persister.Translating())
        {
            _faceImageLibraryName = persister.String("FaceImageLibraryName", value: _faceImageLibraryName);
            Name = persister.String("Name", value: Name);
        }

        _faceIndex = persister.Int32("FaceIndex", value: _faceIndex);
        MoveAnimationSpeed = persister.RawByte("MoveAnimationSpeed", value: MoveAnimationSpeed.TruncateToByte());
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
        if (_refCount == 0 && !GamePack.Current.Status.Contains(HexSpriteSetStatuses.Editting) && (StrategicMap.Current is null || !StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting)))
        {
            ReleaseImageLibrary();
        }

        return _refCount;
    }

    // UnitHS.PAS:31
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            Sequences.ImageLibraryManager = message.Payload.As<GameEngine>().GamePack.ImageLibraryManager;
        }
    }

    // UnitHS.PAS:88
    public LibraryImage? GetCellImage()
    {
        _ = AddRef();
        LibraryImage? result = Sequences.GetImageSequence(2)?.GetPreviewImage() ?? Sequences.GetPreviewImage();
        _ = Release();
        return result;
    }

    // UnitHS.PAS:37
    public void LinkToImageLibrary()
    {
        Sequences.LinkToImageLibrary();
        if (!IsEmpty(_faceImageLibraryName))
        {
            _faceImageLibrary = Sequences.ImageLibraryManager.LinkToImageLibrary(ImageLibraryManagerCategories.Disk, fileName: _faceImageLibraryName);
            return;
        }

        _faceImageLibrary = null;
    }

    // UnitHS.PAS:47
    public void ReleaseImageLibrary()
    {
        Sequences.ReleaseImageLibrary();
        if (_faceImageLibrary is not null)
        {
            if (!IsEmpty(_faceImageLibraryName))
            {
                Sequences.ImageLibraryManager.ReleaseImageLibrary(ImageLibraryManagerCategories.Disk, fileName: _faceImageLibraryName);
            }

            _faceImageLibrary = null;
        }
    }

    // UnitHS.PAS:57
    public void SetFaceImageLibraryName(string value)
    {
        if (!IsEmpty(_faceImageLibraryName))
        {
            Sequences.ImageLibraryManager.ReleaseImageLibrary(ImageLibraryManagerCategories.Disk, fileName: _faceImageLibraryName);
        }

        _faceImageLibraryName = value;
        // { Original releases the old library even when no reference is held. }
        if (!IsEmpty(_faceImageLibraryName) && _refCount != 0)
        {
            _faceImageLibrary = Sequences.ImageLibraryManager.LinkToImageLibrary(ImageLibraryManagerCategories.Disk, fileName: _faceImageLibraryName);
        }
    }

    // UnitHS.PAS:80
    public LibraryImage? GetFaceImage()
    {
        _ = AddRef();
        LibraryImage? result = _faceImageLibrary?.GetSafe(_faceIndex);

        _ = Release();
        return result;
    }

    // UnitHS.PAS:97
    public void Show(int direction, int X, int Y, in LockedSurface lockedSurface)
    {
        _ = AddRef();
        ImageSequence? sequence = Sequences.List[direction - 1];
        while (sequence is not null)
        {
            int index = sequence.GetFrameIndex(1, offset: 0);
            if (index != -1)
            {
                sequence.ImageLibrary[index].Show(X, Y, lockedSurface);
            }

            sequence = sequence.Next;
        }

        _ = Release();
    }

    // UnitHS.PAS:112
    public int Index => EditIndex;
}
