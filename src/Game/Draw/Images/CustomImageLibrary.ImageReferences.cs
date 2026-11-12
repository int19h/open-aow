namespace Aow.Game.Draw.Images;

public partial class CustomImageLibrary
{
    // ImageLib.pas:9890 uses Result as a temporary integer reference index.
    public LibraryImage Get(int index)
    {
        var image = _images[index].Image;
        if (image != null) return image;
        int reference = _images[index].Reference;
        return reference == -1 ? null : _images[reference].Image;
    }

    public LibraryImage GetSafe(int index)
    {
        if (index < 0 || index >= _count) return null;
        var image = _images[index].Image;
        if (image != null) return image;
        int reference = _images[index].Reference;
        return reference >= 0 && reference < _count ? _images[reference].Image : null;
    }
}
