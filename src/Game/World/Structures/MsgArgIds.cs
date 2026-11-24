using Aow.Rtl;

namespace Aow.Game.World.Structures;

public static class MsgArgIds
{
    extension(MsgArg)
    {
        public static MsgArg FromStructure(StructureId? structure) => new(structure.ToOriginal());
    }

    extension(MsgArg argument)
    {
        public StructureId? Structure => StructureId.FromOriginal(argument.Int);
    }
}
