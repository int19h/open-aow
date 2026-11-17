#nullable disable
// Translated from ImageLib.pas.

namespace Aow.Game.Draw.Images;

// an entry of a TILManagerList. FRefCount (the LinkToIL calls not yet released), FImageLibrary
// (loaded by the first LinkToIL)
public partial class ImageLibraryManagerObject
{
    public int RefCount { get; internal set; }
    public CustomImageLibrary ImageLibrary { get; internal set; }
}
