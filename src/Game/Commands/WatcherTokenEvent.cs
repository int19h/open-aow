// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

public partial class WatcherTokenEvent : GlobalTargetSpellTokenEvent
{
    public WatcherTokenEvent(ECustomNode owner) : base(owner)
    { }

    // GlobalTargetSpells.pas:1476
    public new const int ClassId = 135963;

    // GlobalTargetSpells.pas:1479
    public override bool Process(Troop source, int value)
    {
        WatcherHexSprite watcher;
        base.Process(source, value);
        PlayDefaultEffects(Location.X, Location.Y, Location.MapLevel);
        watcher = new WatcherHexSprite(null);
        watcher.Enchantment.PlayerIndex = source.Player;
        var spell = AoWHSSet.Current.SpellControl.GetSpell(57);
        watcher.Enchantment.EnchantmentManaUpkeep = spell.ManaUpkeep;
        AoWHSMap.Current.PlaceHex(watcher, Location.X, Location.Y, Location.MapLevel);
        if ((byte)AoWHSMap.Current.SeatedPlayer == EventPlayerIndex)
        {
            watcher.StartAnimation(Location.X, Location.Y, Location.MapLevel);
        }

        var fSpellControlSpell = AoWHSSet.Current.SpellControl.GetSpell(EventSpellId);
        AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, fSpellControlSpell.BaseManaCost);
        return false;
    }

    // NetworkE.pas:230

}
