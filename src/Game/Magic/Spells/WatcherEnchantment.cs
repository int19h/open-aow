// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// Watcher
public partial class WatcherEnchantment : GlobalEnchantment
{
    public WatcherHexSprite Watcher;

    // GlobalTargetSpells.pas:1121
    public WatcherEnchantment(ECustomNode owner) : base(owner)
    {
        EnchantmentMode = Set<GlobalEnchantmentModeElement>.FromMask(EnchantmentMode.Mask) - Set<GlobalEnchantmentModeElement>.Of(GlobalEnchantmentModeElement.TrueGlobal, GlobalEnchantmentModeElement.Owned);
        DispelMana = 50;
    }

    // GlobalTargetSpells.pas:1129
    public override string GetName() => AoWHSSet.Current.SpellControl.GetSpell(57).SpellName;

    // The unused fourth byte in the original register result has no defined value.
    public override MapCoordinate GetLocation() => new() { X = Watcher.GetHexX(), Y = Watcher.GetHexY(), MapLevel = Watcher.GetHexLevel() };

    public override bool Deactivate(byte player)
    {
        if ((EnchantmentStatus.Image & 1) == 0) return false;
        bool result = base.Deactivate(player);
        if ((Watcher.WatcherState & 1) != 0) Watcher?.Dispose();
        return result;
    }
}
