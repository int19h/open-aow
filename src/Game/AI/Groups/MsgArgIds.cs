using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public static class MsgArgIds
{
    extension(MsgArg)
    {
        public static MsgArg FromGroup(GroupId? group) => new(group.ToOriginal());
    }

    extension(MsgArg argument)
    {
        public GroupId? Group => GroupId.FromOriginal(argument.Int);
    }
}
