// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// FWatcher
public partial class WatcherEnchantment : GlobalEnchantment
{
    public WatcherHS FWatcher;

    // GlobalTargetSpells.pas:1121
    public WatcherEnchantment(ECustomNode owner) : base(owner)
    {
        FEnchantmentMode = Set<GlobalEnchantmentModeElement>.FromMask(FEnchantmentMode.Mask) - Set<GlobalEnchantmentModeElement>.Of(GlobalEnchantmentModeElement.TrueGlobal, GlobalEnchantmentModeElement.Owned);
        FDispelMana = 50;
    }

    // GlobalTargetSpells.pas:1129
    public override string GetName() => AoWHSSet.Current.FSpellControl.GetSpell(57).FSpellName;

    // The unused fourth byte in the original register result has no defined value.
    public override XYL GetLocation() => new() { X = FWatcher.GetXhx(), Y = FWatcher.GetYhx(), L = FWatcher.GetLhx() };

    public override bool Deactivate(byte player)
    {
        if ((FEnchantmentStatus.Image & 1) == 0) return false;
        bool result = base.Deactivate(player);
        if ((FWatcher.FWatcherState & 1) != 0) FWatcher?.Dispose();
        return result;
    }
}
