// Translated from ImageLib.pas.

namespace Aow.Game.Draw.Images;

// an entry of a TILManagerList. FRefCount (the LinkToIL calls not yet released), FImageLibrary
// (loaded by the first LinkToIL)
public partial class ImageLibraryManagerObject
{
    private int _refCount;
    private CustomImageLibrary _imageLibrary;

    public int RefCount { get => _refCount; internal set => _refCount = value; }
    public CustomImageLibrary ImageLibrary { get => _imageLibrary; internal set => _imageLibrary = value; }
}
