using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public static class MsgArgIds
{
    extension(MsgArg)
    {
        public static MsgArg FromSpell(SpellId? spell) => new(spell.ToOriginal());
    }

    extension(MsgArg argument)
    {
        public SpellId? Spell => SpellId.FromOriginal(argument.Int);
    }
}
