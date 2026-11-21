// Translated from ImageLib.pas.

namespace Aow.Game.Draw.Images;

[Flags]
public enum ImageLibraryStatuses : byte
{
    None = 0,
    LoadingFromFile = 1 << 0,
    Loading = 1 << 1,
    Modified = 1 << 2,
    Saving = 1 << 3,
    SavingToFile = 1 << 4,
}
