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
// FName. FFaceIndex. FFaceImageLibrary. FFaceImageLibraryName. FSequences. FRefCount. flags.
// FSFXLibraries
public partial class UnitGraphicsResource : EngineResource
{
    public new const int ClassID = 131600;

    // UnitHS.PAS:1
    public UnitGraphicsResource(ECustomNode owner) : base(owner)
    {
        FSequences = new ImageSequenceList(null, null);
        FRefCount = 0;
        var sfxLibrary = new SfxLibrary(null);
        FSFXLibraries[0] = sfxLibrary;
        var sfxLibrary2 = new SfxLibrary(null);
        FSFXLibraries[1] = sfxLibrary2;
        var sfxLibrary3 = new SfxLibrary(null);
        FSFXLibraries[2] = sfxLibrary3;
        var sfxLibrary4 = new SfxLibrary(null);
        FSFXLibraries[3] = sfxLibrary4;
        var sfxLibrary5 = new SfxLibrary(null);
        FSFXLibraries[4] = sfxLibrary5;
    }

    public string FName = "";
    public int FFaceIndex;
    public CustomImageLibrary FFaceImageLibrary;
    public string FFaceImageLibraryName = "";
    public ImageSequenceList FSequences;
    public int FRefCount;
    public byte F_UnitGFXResource_30;
    public SfxLibraryArray5 FSFXLibraries;

    // UnitHS.PAS:12
    public override void Dispose()
    {
        if (FRefCount != 0)
        {
            ReleaseIL();
        }

        FSequences?.Dispose();
        FSFXLibraries[0]?.Dispose();
        FSFXLibraries[1]?.Dispose();
        FSFXLibraries[2]?.Dispose();
        FSFXLibraries[3]?.Dispose();
        FSFXLibraries[4]?.Dispose();
        base.Dispose();
    }

    // UnitHS.PAS:27
    public override GameEngine GetEngine() => GameTables.AoWEngine;

    // UnitHS.PAS:116
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(5, FSequences.LoadFromStream, FSequences.SaveToStream);
        if (!Stream.StorageMode().Contains(EngineServices.smText))
        {
            Stream.rwString(ref FFaceImageLibraryName, 6);
            Stream.rwString(ref FName, 7);
        }

        Stream.rwInteger(ref FFaceIndex, 8);
        Stream.rwData(Bytes.Of(ref F_UnitGFXResource_30, 1), 1, 10);
        Stream.rwEObject(FSFXLibraries[0], 15);
        Stream.rwEObject(FSFXLibraries[1], 16);
        Stream.rwEObject(FSFXLibraries[2], 17);
        Stream.rwEObject(FSFXLibraries[3], 18);
        Stream.rwEObject(FSFXLibraries[4], 19);
    }

    // UnitHS.PAS:66
    public override int AddRef()
    {
        if (FRefCount == 0)
        {
            LinkToIL();
        }

        FRefCount += 1;
        return FRefCount;
    }

    // UnitHS.PAS:72
    public override int Release()
    {
        FRefCount -= 1;
        if (FRefCount == 0 && !AoWHSSet.Current.Status.Contains(HexSpriteSetStatusElement.Editting) && (AoWHSMap.Current == null || !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting)))
        {
            ReleaseIL();
        }

        return FRefCount;
    }

    // UnitHS.PAS:31
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            FSequences.FILManager = Msg.Payload.As<GameEngine>().HSSet.ImageLibraryManager;
        }
    }

    // UnitHS.PAS:88
    public override LibraryImage GetCellImage()
    {
        LibraryImage Result;
        AddRef();
        if (FSequences.GetImageSequence(2) != null)
        {
            Result = FSequences.GetImageSequence(2).GetPreviewImage();
        }
        else
        {
            Result = null;
        }

        if (Result == null)
        {
            Result = FSequences.GetPreviewImage();
        }

        Release();
        return Result;
    }

    // UnitHS.PAS:37
    public void LinkToIL()
    {
        FSequences.LinkToIL();
        if (!IsEmpty(FFaceImageLibraryName))
        {
            FFaceImageLibrary = FSequences.ILManager.LinkToIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FFaceImageLibraryName);
            return;
        }

        FFaceImageLibrary = null;
    }

    // UnitHS.PAS:47
    public void ReleaseIL()
    {
        FSequences.ReleaseIL();
        if (FFaceImageLibrary != null)
        {
            if (!IsEmpty(FFaceImageLibraryName))
            {
                FSequences.ILManager.ReleaseIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FFaceImageLibraryName);
            }

            FFaceImageLibrary = null;
        }
    }

    // UnitHS.PAS:57
    public void SetFaceImageLibraryName(string Value)
    {
        if (!IsEmpty(FFaceImageLibraryName))
        {
            FSequences.ILManager.ReleaseIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FFaceImageLibraryName);
        }

        FFaceImageLibraryName = Value;
        // { Original releases the old library even when no reference is held. }
        if (!IsEmpty(FFaceImageLibraryName) && FRefCount != 0)
        {
            FFaceImageLibrary = FSequences.ILManager.LinkToIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FFaceImageLibraryName);
        }
    }

    // UnitHS.PAS:80
    public LibraryImage GetFaceImage()
    {
        LibraryImage Result;
        AddRef();
        if (FFaceImageLibrary != null)
        {
            Result = FFaceImageLibrary.GetSafe(FFaceIndex);
        }
        else
        {
            Result = null;
        }

        Release();
        return Result;
    }

    // UnitHS.PAS:97
    public void Show(int Direction, int X, int Y, in LockedSurface SD)
    {
        ImageSequence Sequence;
        int Index;
        AddRef();
        Sequence = (ImageSequence)FSequences.List[Direction - 1];
        while (Sequence != null)
        {
            Index = Sequence.GetFrameIndex(1, 0);
            if (Index != -1)
            {
                Sequence.ImageLibrary[Index].Show(X, Y, SD);
            }

            Sequence = Sequence.Next;
        }

        Release();
    }

    // UnitHS.PAS:112
    public int GetIndex() => EditIndex;

    // intf_P2_03.inc:22

}
