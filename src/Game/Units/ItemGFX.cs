// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Draw.Images;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// library name, image index, item type, library, references, hidden
public partial class ItemGFX : EResource
{
    public new const int ClassID = 131689;

    public ItemGFX(ECustomNode owner) : base(owner)
    { }

    public string FImageLibraryName = "";
    public int FImageIndex;
    public ItemType FItemType;
    public CustomImageLibrary FImageLibrary;
    public int FRefCount;
    public bool FHidden;

    // Army.PAS:5093
    public override GameEngine GetEngine() => GameTables.AoWEngine;

    // Army.PAS:5167
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            Stream.rwString(ref FImageLibraryName, 6);
        }

        Stream.rwInteger(ref FImageIndex, 7);
        Stream.rwData(Bytes.Of(ref FItemType, 1), 1, 8);
        Stream.rwBool(ref FHidden, 9);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.String("ImageLibraryName", ref FImageLibraryName);
        persister.Int("ImageIndex", ref FImageIndex);
        persister.Enum("ItemType", ref FItemType);
        persister.Bool("Hidden", ref FHidden);
    }

    // Army.PAS:5138
    public override int AddRef()
    {
        if (FRefCount == 0)
        {
            LinkToIL();
        }

        FRefCount += 1;
        return FRefCount;
    }

    // Army.PAS:5145
    public override int Release()
    {
        FRefCount -= 1;
        if (FRefCount == 0)
        {
            ReleaseIL();
        }

        return FRefCount;
    }

    // Army.PAS:5152
    public override LibraryImage GetCellImage()
    {
        LibraryImage Image;
        AddRef();
        Image = FImageLibrary[FImageIndex];
        Release();
        return Image;
    }

    // Army.PAS:5103
    public int ItemGFXIndex() => Owner.FindChildIndex(this);

    // Army.PAS:5108
    public void LinkToIL()
    {
        ReleaseIL();
        if (!IsEmpty(FImageLibraryName))
        {
            FImageLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), FImageLibraryName);
        }
        else
        {
            FImageLibrary = null;
        }
    }

    // Army.PAS:5116
    public void ReleaseIL()
    {
        if (FImageLibrary != null)
        {
            if (!IsEmpty(FImageLibraryName))
            {
                AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), FImageLibraryName);
            }

            FImageLibrary = null;
        }
    }

    // Army.PAS:5126
    public void SetImageLibraryName(string Value)
    {
        if (!IsEmpty(FImageLibraryName))
        {
            AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), FImageLibraryName);
            FImageLibrary = null;
        }

        FImageLibraryName = Value;
        if (!IsEmpty(FImageLibraryName) && FRefCount != 0)
        {
            FImageLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), FImageLibraryName);
        }
    }

    // Army.PAS:5161
    public LibraryImage GetItemImage()
    {
        LibraryImage Result;
        if (FImageLibrary != null)
        {
            Result = FImageLibrary[FImageIndex];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
