using Aow.Rtl;

namespace Aow.Units.AoWE;

public static partial class AoWEUnit
{
    public static int MoveTypesToInt(TSet<TMoveType> types) => (byte)types.Image;
    public static TSet<TMoveType> IntToMoveTypes(int value) => TSet<TMoveType>.FromMask((byte)value);
}
