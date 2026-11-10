namespace Aow.Game.Draw.Images;

public partial class CustomImageLibrary
{
    // ImageLib.pas:9890 uses Result as a temporary integer reference index.
    public LibraryImage Get(int index)
    {
        var image = FImages[index].Image;
        if (image != null) return image;
        int reference = FImages[index].Reference;
        return reference == -1 ? null : FImages[reference].Image;
    }

    public LibraryImage GetSafe(int index)
    {
        if (index < 0 || index >= FCount) return null;
        var image = FImages[index].Image;
        if (image != null) return image;
        int reference = FImages[index].Reference;
        return reference >= 0 && reference < FCount ? FImages[reference].Image : null;
    }
}
