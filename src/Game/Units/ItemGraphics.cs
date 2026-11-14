// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Draw.Images;

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

    public string ImageLibraryName { get; set; } = "";
    public int ImageIndex { get; set; }
    public ItemType ItemType { get; set; }

    private CustomImageLibrary _imageLibrary;
    private int _refCount;

    public bool Hidden { get; set; }

    // Army.PAS:5167
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        ImageLibraryName = persister.String("ImageLibraryName", value: ImageLibraryName);
        ImageIndex = persister.Int32("ImageIndex", value: ImageIndex);
        ItemType = persister.Enum("ItemType", value: ItemType);
        Hidden = persister.Boolean("Hidden", value: Hidden);
    }

    // Army.PAS:5138
    public override int AddRef()
    {
        if (_refCount == 0)
        {
            LinkToImageLibrary();
        }

        _refCount += 1;
        return _refCount;
    }

    // Army.PAS:5145
    public override int Release()
    {
        _refCount -= 1;
        if (_refCount == 0)
        {
            ReleaseImageLibrary();
        }

        return _refCount;
    }

    // Army.PAS:5152
    public override LibraryImage GetCellImage()
    {
        LibraryImage image;
        AddRef();
        image = _imageLibrary[ImageIndex];
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
            _imageLibrary = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), ImageLibraryName);
        }
        else
        {
            _imageLibrary = null;
        }
    }

    // Army.PAS:5116
    public void ReleaseImageLibrary()
    {
        if (_imageLibrary != null)
        {
            if (!IsEmpty(ImageLibraryName))
            {
                GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), ImageLibraryName);
            }

            _imageLibrary = null;
        }
    }

    // Army.PAS:5126
    public void SetImageLibraryName(string value)
    {
        if (!IsEmpty(ImageLibraryName))
        {
            GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), ImageLibraryName);
            _imageLibrary = null;
        }

        ImageLibraryName = value;
        if (!IsEmpty(ImageLibraryName) && _refCount != 0)
        {
            _imageLibrary = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), ImageLibraryName);
        }
    }

    // Army.PAS:5161
    public LibraryImage GetItemImage()
    {
        LibraryImage result;
        if (_imageLibrary != null)
        {
            result = _imageLibrary[ImageIndex];
        }
        else
        {
            result = null;
        }

        return result;
    }

}
