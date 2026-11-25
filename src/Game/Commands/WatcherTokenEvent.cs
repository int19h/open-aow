#nullable disable warnings
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

public class WatcherTokenEvent(CustomNode? owner) : GlobalTargetSpellTokenEvent(owner)
{

    // GlobalTargetSpells.pas:1476
    public new const int ClassId = 135_963;

    // GlobalTargetSpells.pas:1479
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        PlayDefaultEffects(Location.X, Location.Y, layer: Location.MapLevel);
        WatcherHexSprite watcher = new(null) { Holder = CreationHolder() };
        watcher.Enchantment.PlayerIndex = source.Player;
        Spell? spell = GetEngine().GamePack.SpellControl[57];
        watcher.Enchantment.EnchantmentManaUpkeep = spell.ManaUpkeep;
        _ = GetEngine().StrategicMap.PlaceHex(watcher, Location.X, Location.Y, L: Location.MapLevel);
        if ((byte)PlayerIdExtensions.ToOriginal(GetEngine().StrategicMap.SeatedPlayer) == (byte)PlayerIdExtensions.ToOriginal(EventPlayerIndex))
        {
            watcher.StartAnimation(Location.X, Location.Y, layer: Location.MapLevel);
        }

        GamePack gamePack = GetEngine().GamePack;
        Spell? fSpellControlSpell = gamePack.SpellControl[EventSpellId];
        gamePack.SpellControl[EventSpellId].CastingDone(source, mana: fSpellControlSpell.BaseManaCost);
        return false;
    }
}
