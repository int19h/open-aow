// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Draw.Images;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Units;

// { Interface declaration run owned by P1. }
public partial class FaceResource : EngineResource
{
    public new const int ClassId = 131696;

    public FaceResource(ECustomNode owner) : base(owner)
    { }

    private string _imageLibraryName = "";
    private int _imageIndex;
    private CustomImageLibrary _imageLibrary;
    private int _references;
    public bool DeveloperOnly;

    // Ability.PAS:512
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.String(ref _imageLibraryName, 5);
        }

        stream.Int32(ref _imageIndex, 6);
        stream.Boolean(ref DeveloperOnly, 7);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _imageLibraryName = persister.String("ImageLibraryName", value: _imageLibraryName);
        _imageIndex = persister.Int32("ImageIndex", value: _imageIndex);
        DeveloperOnly = persister.Boolean("DeveloperOnly", value: DeveloperOnly);
    }

    // Ability.PAS:482
    public override int AddRef()
    {
        if (_references == 0)
        {
            LinkToImageLibrary();
        }

        _references += 1;
        return _references;
    }

    // Ability.PAS:489
    public override int Release()
    {
        _references -= 1;
        if (_references == 0)
        {
            ReleaseImageLibrary();
        }

        return _references;
    }

    // Ability.PAS:496
    public override LibraryImage GetCellImage()
    {
        AddRef();
        LibraryImage result = _imageLibrary[_imageIndex];
        Release();
        return result;
    }

    // Ability.PAS:448
    public int FaceResourceIndex() => Owner.FindChildIndex(this);

    // Ability.PAS:451
    public void LinkToImageLibrary()
    {
        ReleaseImageLibrary();
        if (!IsEmpty(_imageLibraryName))
        {
            _imageLibrary = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _imageLibraryName);
        }
        else
        {
            _imageLibrary = null;
        }
    }

    // Ability.PAS:459
    public void ReleaseImageLibrary()
    {
        if (_imageLibrary != null)
        {
            if (!IsEmpty(_imageLibraryName))
            {
                GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _imageLibraryName);
            }

            _imageLibrary = null;
        }
    }

    // Ability.PAS:469
    public void SetImageLibraryName(string value)
    {
        if (!IsEmpty(_imageLibraryName))
        {
            // { Original releases the old name even when this resource has no references. }
            GamePack.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _imageLibraryName);
            _imageLibrary = null;
        }

        _imageLibraryName = value;
        if (!IsEmpty(_imageLibraryName) && _references != 0)
        {
            _imageLibrary = GamePack.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), _imageLibraryName);
        }
    }

    // Ability.PAS:503
    public LibraryImage GetFaceImage()
    {
        LibraryImage result;
        CustomImageLibrary images;
        images = _imageLibrary;
        if (images != null)
        {
            result = images[_imageIndex];
        }
        else
        {
            result = null;
        }

        return result;
    }

}
