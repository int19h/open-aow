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
    public new const int ClassId = 131696;

    public FaceResource(ECustomNode owner) : base(owner)
    { }

    public string ImageLibraryName = "";
    public int ImageIndex;
    public CustomImageLibrary ImageLibrary;
    public int References;
    public bool DeveloperOnly;

    // Ability.PAS:445
    public override GameEngine GetEngine() => GameTables.GameEngine;

    // Ability.PAS:512
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        if (!stream.StorageMode().Contains(StorageModeElement.TextOnly))
        {
            stream.rwString(ref ImageLibraryName, 5);
        }

        stream.rwInteger(ref ImageIndex, 6);
        stream.rwBool(ref DeveloperOnly, 7);
    }

    // Ability.PAS:482
    public override int AddRef()
    {
        if (References == 0)
        {
            LinkToImageLibrary();
        }

        References += 1;
        return References;
    }

    // Ability.PAS:489
    public override int Release()
    {
        References -= 1;
        if (References == 0)
        {
            ReleaseImageLibrary();
        }

        return References;
    }

    // Ability.PAS:496
    public override LibraryImage GetCellImage()
    {
        AddRef();
        LibraryImage result = ImageLibrary[ImageIndex];
        Release();
        return result;
    }

    // Ability.PAS:448
    public int FaceResourceIndex() => Owner.FindChildIndex(this);

    // Ability.PAS:451
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

    // Ability.PAS:459
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

    // Ability.PAS:469
    public void SetImageLibraryName(string value)
    {
        if (!IsEmpty(ImageLibraryName))
        {
            // { Original releases the old name even when this resource has no references. }
            AoWHSSet.Current.ImageLibraryManager.ReleaseImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), ImageLibraryName);
            ImageLibrary = null;
        }

        ImageLibraryName = value;
        if (!IsEmpty(ImageLibraryName) && References != 0)
        {
            ImageLibrary = AoWHSSet.Current.ImageLibraryManager.LinkToImageLibrary(Set<ImageLibraryManagerCategoryElement>.Of(ImageLibraryManagerCategoryElement.Disk), ImageLibraryName);
        }
    }

    // Ability.PAS:503
    public LibraryImage GetFaceImage()
    {
        LibraryImage result;
        CustomImageLibrary images;
        images = ImageLibrary;
        if (images != null)
        {
            result = images[ImageIndex];
        }
        else
        {
            result = null;
        }

        return result;
    }

}
