#nullable disable
// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

public partial class WatcherTokenEvent(CustomNode owner) : GlobalTargetSpellTokenEvent(owner)
{

    // GlobalTargetSpells.pas:1476
    public new const int ClassId = 135963;

    // GlobalTargetSpells.pas:1479
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        PlayDefaultEffects(Location.X, Location.Y, Location.MapLevel);
        WatcherHexSprite watcher = new(null) { Holder = CreationHolder() };
        watcher.Enchantment.PlayerIndex = source.Player;
        Spell spell = GamePack.Current.SpellControl.GetSpell(57);
        watcher.Enchantment.EnchantmentManaUpkeep = spell.ManaUpkeep;
        _ = StrategicMap.Current.PlaceHex(watcher, Location.X, Location.Y, Location.MapLevel);
        if ((byte)StrategicMap.Current.SeatedPlayer == EventPlayerIndex)
        {
            watcher.StartAnimation(Location.X, Location.Y, Location.MapLevel);
        }

        Spell fSpellControlSpell = GamePack.Current.SpellControl.GetSpell(EventSpellId);
        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, fSpellControlSpell.BaseManaCost);
        return false;
    }

    // NetworkE.pas:230

}
