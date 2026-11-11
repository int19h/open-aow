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
    public new const int ClassID = 135963;

    // GlobalTargetSpells.pas:1479
    public override bool Process(Troop source, int value)
    {
        WatcherHexSprite Watcher;
        base.Process(source, value);
        PlayDefaultEffects(FLocation.X, FLocation.Y, FLocation.L);
        Watcher = new WatcherHexSprite(null);
        Watcher.FEnchantment.FPlayerIndex = source.FPlayer;
        var spell = AoWHSSet.Current.FSpellControl.GetSpell(57);
        Watcher.FEnchantment.FEnchantmentManaUpkeep = spell.FManaUpkeep;
        AoWHSMap.Current.PlaceHX(Watcher, FLocation.X, FLocation.Y, FLocation.L);
        if ((byte)AoWHSMap.Current.FSeatedPlayer == FEventPlayerIndex)
        {
            Watcher.StartAnimation(FLocation.X, FLocation.Y, FLocation.L);
        }

        var fSpellControlSpell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, fSpellControlSpell.FBaseManaCost);
        return false;
    }

    // NetworkE.pas:230

}
