namespace Aow.Game.Draw.Images;

public partial class CustomImageLibrary
{
    // ImageLib.pas:9890 uses Result as a temporary integer reference index.
    public LibraryImage Get(int index)
    {
        var image = Images[index].Image;
        if (image != null) return image;
        int reference = Images[index].Reference;
        return reference == -1 ? null : Images[reference].Image;
    }

    public LibraryImage GetSafe(int index)
    {
        if (index < 0 || index >= _count) return null;
        var image = Images[index].Image;
        if (image != null) return image;
        int reference = Images[index].Reference;
        return reference >= 0 && reference < _count ? Images[reference].Image : null;
    }
}
