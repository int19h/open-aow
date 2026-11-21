#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.Units;

// library name, image index, item type, library, references, hidden
public sealed class ItemGraphics(CustomNode? owner) : EngineResource(owner)
{
    public new const int ClassId = 131_689;

    public string ImageLibraryName { get; set; } = "";
    public int ImageIndex { get; set; }
    public ItemType ItemType { get; set; }

    private CustomImageLibrary? _imageLibrary;
    private int _refCount;

    public bool Hidden { get; set; }

    // Army.PAS:5167
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        // The original leaves the library name out under the text-only mode of the translate stream.
        if (!persister.Translating())
        {
            ImageLibraryName = persister.String("ImageLibraryName", value: ImageLibraryName);
        }
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
    public override LibraryImage? GetCellImage()
    {
        _ = AddRef();
        LibraryImage? image = _imageLibrary[ImageIndex];
        _ = Release();
        return image;
    }

    // Army.PAS:5103
    public int ItemGraphicsIndex() => Owner.FindChildIndex(this);

    // Army.PAS:5108
    public void LinkToImageLibrary()
    {
        ReleaseImageLibrary();
        _imageLibrary = !IsEmpty(ImageLibraryName)
            ? GamePack.Current.ImageLibraryManager.LinkToImageLibrary(ImageLibraryManagerCategories.Disk, fileName: ImageLibraryName)
            : null;
    }

    // Army.PAS:5116
    public void ReleaseImageLibrary()
    {
        if (_imageLibrary is not null)
        {
            if (!IsEmpty(ImageLibraryName))
            {
                GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(ImageLibraryManagerCategories.Disk, fileName: ImageLibraryName);
            }

            _imageLibrary = null;
        }
    }

    // Army.PAS:5126
    public void SetImageLibraryName(string value)
    {
        if (!IsEmpty(ImageLibraryName))
        {
            GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(ImageLibraryManagerCategories.Disk, fileName: ImageLibraryName);
            _imageLibrary = null;
        }

        ImageLibraryName = value;
        if (!IsEmpty(ImageLibraryName) && _refCount != 0)
        {
            _imageLibrary = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(ImageLibraryManagerCategories.Disk, fileName: ImageLibraryName);
        }
    }

    // Army.PAS:5161
    public LibraryImage? ItemImage => _imageLibrary?[ImageIndex];

}
