using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

public static class MsgArgIds
{
    extension(MsgArg)
    {
        public static MsgArg FromAbility(AbilityId? ability) => new(ability.ToOriginal());
    }

    extension(MsgArg argument)
    {
        public AbilityId? Ability => AbilityId.FromOriginal(argument.Int);
    }
}
