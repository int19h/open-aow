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
public partial class ItemGraphics : EngineResource
{
    public new const int ClassId = 131689;

    public ItemGraphics(ECustomNode owner) : base(owner)
    { }

    public string ImageLibraryName = "";
    public int ImageIndex;
    public ItemType ItemType;
    public CustomImageLibrary ImageLibrary;
    public int RefCount;
    public bool Hidden;

    // Army.PAS:5093
    public override GameEngine GetEngine() => GameTables.GameEngine;

    // Army.PAS:5167
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.rwString(ref ImageLibraryName, 6);
        }

        stream.rwInteger(ref ImageIndex, 7);
        stream.rwData(Bytes.Of(ref ItemType, 1), 1, 8);
        stream.rwBool(ref Hidden, 9);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.String("ImageLibraryName", ref ImageLibraryName);
        persister.Int32("ImageIndex", ref ImageIndex);
        persister.Enum("ItemType", ref ItemType);
        persister.Boolean("Hidden", ref Hidden);
    }

    // Army.PAS:5138
    public override int AddRef()
    {
        if (RefCount == 0)
        {
            LinkToImageLibrary();
        }

        RefCount += 1;
        return RefCount;
    }

    // Army.PAS:5145
    public override int Release()
    {
        RefCount -= 1;
        if (RefCount == 0)
        {
            ReleaseImageLibrary();
        }

        return RefCount;
    }

    // Army.PAS:5152
    public override LibraryImage GetCellImage()
    {
        LibraryImage image;
        AddRef();
        image = ImageLibrary[ImageIndex];
        Release();
        return image;
    }

    // Army.PAS:5103
    public int ItemGraphicsIndex() => Owner.FindChildIndex(this);

    // Army.PAS:5108
    public void LinkToImageLibrary()
    {
        ReleaseImageLibrary();
        if (!IsEmpty(ImageLibraryName))
        {
            ImageLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), ImageLibraryName);
        }
        else
        {
            ImageLibrary = null;
        }
    }

    // Army.PAS:5116
    public void ReleaseImageLibrary()
    {
        if (ImageLibrary != null)
        {
            if (!IsEmpty(ImageLibraryName))
            {
                AoWHSSet.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), ImageLibraryName);
            }

            ImageLibrary = null;
        }
    }

    // Army.PAS:5126
    public void SetImageLibraryName(string value)
    {
        if (!IsEmpty(ImageLibraryName))
        {
            AoWHSSet.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), ImageLibraryName);
            ImageLibrary = null;
        }

        ImageLibraryName = value;
        if (!IsEmpty(ImageLibraryName) && RefCount != 0)
        {
            ImageLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), ImageLibraryName);
        }
    }

    // Army.PAS:5161
    public LibraryImage GetItemImage()
    {
        LibraryImage result;
        if (ImageLibrary != null)
        {
            result = ImageLibrary[ImageIndex];
        }
        else
        {
            result = null;
        }

        return result;
    }

}
