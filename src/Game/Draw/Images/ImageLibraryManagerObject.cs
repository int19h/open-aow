// Translated from ImageLib.pas.

namespace Aow.Game.Draw.Images;

// an entry of a TILManagerList. FRefCount (the LinkToIL calls not yet released), FImageLibrary
// (loaded by the first LinkToIL)
public partial class ImageLibraryManagerObject
{
    internal int _refCount;
    internal CustomImageLibrary _imageLibrary;

    public int RefCount { get => _refCount; }
    public CustomImageLibrary ImageLibrary { get => _imageLibrary; }
}
