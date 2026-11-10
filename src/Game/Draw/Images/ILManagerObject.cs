// Translated from ImageLib.pas.

namespace Aow.Game.Draw.Images;

// an entry of a TILManagerList. FRefCount (the LinkToIL calls not yet released), FImageLibrary
// (loaded by the first LinkToIL)
public partial class ILManagerObject
{
    internal int FRefCount;
    internal CustomImageLibrary FImageLibrary;

    public int RefCount { get => FRefCount; }
    public CustomImageLibrary ImageLibrary { get => FImageLibrary; }
}
