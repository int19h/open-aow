// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;

using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

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
        RefCount = 0;
        var sfxLibrary = new SfxLibrary(null);
        SfxLibraries[0] = sfxLibrary;
        var sfxLibrary2 = new SfxLibrary(null);
        SfxLibraries[1] = sfxLibrary2;
        var sfxLibrary3 = new SfxLibrary(null);
        SfxLibraries[2] = sfxLibrary3;
        var sfxLibrary4 = new SfxLibrary(null);
        SfxLibraries[3] = sfxLibrary4;
        var sfxLibrary5 = new SfxLibrary(null);
        SfxLibraries[4] = sfxLibrary5;
    }

    public string Name = "";
    public int FaceIndex;
    public CustomImageLibrary FaceImageLibrary;
    public string FaceImageLibraryName = "";
    public ImageSequenceList Sequences;
    public int RefCount;
    public byte MoveAnimationSpeed;
    public SfxLibraryArray5 SfxLibraries;

    // UnitHS.PAS:12
    public override void Dispose()
    {
        if (RefCount != 0)
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

    // UnitHS.PAS:27
    public override GameEngine GetEngine() => GameTables.GameEngine;

    // UnitHS.PAS:116
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStreamProc(5, Sequences.LoadFromStream, Sequences.SaveToStream);
        if (!stream.StorageMode().Contains(EngineServices.Text))
        {
            stream.rwString(ref FaceImageLibraryName, 6);
            stream.rwString(ref Name, 7);
        }

        stream.rwInteger(ref FaceIndex, 8);
        stream.rwData(Bytes.Of(ref MoveAnimationSpeed, 1), 1, 10);
        stream.rwEObject(SfxLibraries[0], 15);
        stream.rwEObject(SfxLibraries[1], 16);
        stream.rwEObject(SfxLibraries[2], 17);
        stream.rwEObject(SfxLibraries[3], 18);
        stream.rwEObject(SfxLibraries[4], 19);
    }

    // UnitHS.PAS:66
    public override int AddRef()
    {
        if (RefCount == 0)
        {
            LinkToImageLibrary();
        }

        RefCount += 1;
        return RefCount;
    }

    // UnitHS.PAS:72
    public override int Release()
    {
        RefCount -= 1;
        if (RefCount == 0 && !AoWHSSet.Current.Status.Contains(HexSpriteSetStatusElement.Editting) && (AoWHSMap.Current == null || !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting)))
        {
            ReleaseImageLibrary();
        }

        return RefCount;
    }

    // UnitHS.PAS:31
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            Sequences._imageLibraryManager = message.Payload.As<GameEngine>().HexSpriteSet.ImageLibraryManager;
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
        if (!IsEmpty(FaceImageLibraryName))
        {
            FaceImageLibrary = Sequences.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FaceImageLibraryName);
            return;
        }

        FaceImageLibrary = null;
    }

    // UnitHS.PAS:47
    public void ReleaseImageLibrary()
    {
        Sequences.ReleaseImageLibrary();
        if (FaceImageLibrary != null)
        {
            if (!IsEmpty(FaceImageLibraryName))
            {
                Sequences.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FaceImageLibraryName);
            }

            FaceImageLibrary = null;
        }
    }

    // UnitHS.PAS:57
    public void SetFaceImageLibraryName(string value)
    {
        if (!IsEmpty(FaceImageLibraryName))
        {
            Sequences.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FaceImageLibraryName);
        }

        FaceImageLibraryName = value;
        // { Original releases the old library even when no reference is held. }
        if (!IsEmpty(FaceImageLibraryName) && RefCount != 0)
        {
            FaceImageLibrary = Sequences.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FaceImageLibraryName);
        }
    }

    // UnitHS.PAS:80
    public LibraryImage GetFaceImage()
    {
        LibraryImage result;
        AddRef();
        if (FaceImageLibrary != null)
        {
            result = FaceImageLibrary.GetSafe(FaceIndex);
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
        sequence = (ImageSequence)Sequences.List[direction - 1];
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
