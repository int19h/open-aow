// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// { Interface declaration run owned by P1. }
public partial class FaceResource : EngineResource
{
    public new const int ClassID = 131696;

    public FaceResource(ECustomNode owner) : base(owner)
    { }

    public string FImageLibraryName = "";
    public int FImageIndex;
    public CustomImageLibrary FImageLibrary;
    public int FReferences;
    public bool FDeveloperOnly;

    // Ability.PAS:445
    public override GameEngine GetEngine() => GameTables.AoWEngine;

    // Ability.PAS:512
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FImageLibraryName, 5);
        }

        Stream.rwInteger(ref FImageIndex, 6);
        Stream.rwBool(ref FDeveloperOnly, 7);
    }

    // Ability.PAS:482
    public override int AddRef()
    {
        if (FReferences == 0)
        {
            LinkToIL();
        }

        FReferences += 1;
        return FReferences;
    }

    // Ability.PAS:489
    public override int Release()
    {
        FReferences -= 1;
        if (FReferences == 0)
        {
            ReleaseIL();
        }

        return FReferences;
    }

    // Ability.PAS:496
    public override LibraryImage GetCellImage()
    {
        AddRef();
        LibraryImage Result = FImageLibrary[FImageIndex];
        Release();
        return Result;
    }

    // Ability.PAS:448
    public int FaceResourceIndex() => Owner.FindChildIndex(this);

    // Ability.PAS:451
    public void LinkToIL()
    {
        ReleaseIL();
        if (!IsEmpty(FImageLibraryName))
        {
            FImageLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FImageLibraryName);
        }
        else
        {
            FImageLibrary = null;
        }
    }

    // Ability.PAS:459
    public void ReleaseIL()
    {
        if (FImageLibrary != null)
        {
            if (!IsEmpty(FImageLibraryName))
            {
                AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FImageLibraryName);
            }

            FImageLibrary = null;
        }
    }

    // Ability.PAS:469
    public void SetImageLibraryName(string Value)
    {
        if (!IsEmpty(FImageLibraryName))
        {
            // { Original releases the old name even when this resource has no references. }
            AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FImageLibraryName);
            FImageLibrary = null;
        }

        FImageLibraryName = Value;
        if (!IsEmpty(FImageLibraryName) && FReferences != 0)
        {
            FImageLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), FImageLibraryName);
        }
    }

    // Ability.PAS:503
    public LibraryImage GetFaceImage()
    {
        LibraryImage Result;
        CustomImageLibrary Images;
        Images = FImageLibrary;
        if (Images != null)
        {
            Result = Images[FImageIndex];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
