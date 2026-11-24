using Aow.Rtl;

namespace Aow.Game.Magic;

public static class MsgArgIds
{
    extension(MsgArg)
    {
        public static MsgArg FromEnchantment(EnchantmentId? enchantment) => new(enchantment.ToOriginal());
    }

    extension(MsgArg argument)
    {
        public EnchantmentId? Enchantment => EnchantmentId.FromOriginal(argument.Int);
    }
}
