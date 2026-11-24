using Aow.Rtl;

namespace Aow.Game.Units;

public static class MsgArgIds
{
    extension(MsgArg)
    {
        public static MsgArg FromUnitType(UnitTypeId? unitType) => new(unitType.ToOriginal());
        public static MsgArg FromUnit(UnitId? unit) => new(unit.ToOriginal());
        public static MsgArg FromItem(ItemId item) => new(item.OriginalValue);
        public static MsgArg FromLibrary(LibraryId? library) => new(library.ToOriginal());
    }

    extension(MsgArg argument)
    {
        public UnitTypeId? UnitType => UnitTypeId.FromOriginal(argument.Int);
        public UnitId? Unit => UnitId.FromOriginal(argument.Int);
        public ItemId Item => ItemId.FromInt32(argument.Int);
        public LibraryId? Library => LibraryId.FromOriginal(argument.Int);
    }
}
