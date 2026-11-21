// Translated from ImageLib.pas.

namespace Aow.Game.Draw.Images;

[Flags]
public enum ImageLibraryManagerCategories : byte
{
    None = 0,
    Disk = 1 << 0,
    Owned = 1 << 1,
}
