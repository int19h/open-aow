#nullable disable warnings
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// Watcher
public class WatcherEnchantment : GlobalEnchantment
{
    public WatcherHexSprite Watcher { get; init; }

    // GlobalTargetSpells.pas:1121
    public WatcherEnchantment(CustomNode? owner) : base(owner)
    {
        EnchantmentMode = (GlobalEnchantmentModes)(ulong)EnchantmentMode & ~(GlobalEnchantmentModes.TrueGlobal | GlobalEnchantmentModes.Owned);
        DispelMana = 50;
    }

    // GlobalTargetSpells.pas:1129
    public override string GetName() => GamePack.Current.SpellControl[57].SpellName;

    // The unused fourth byte in the original register result has no defined value.
    public override MapCoordinate GetLocation() => new() { X = Watcher.HexX, Y = Watcher.HexY, MapLevel = Watcher.HexLevel };

    public override bool Deactivate(PlayerId? player)
    {
        if (((ulong)EnchantmentStatus & 1) == 0)
        {
            return false;
        }

        bool result = base.Deactivate(player);
        if ((Watcher.WatcherState & 1) != 0)
        {
            Watcher?.Dispose();
        }

        return result;
    }
}
