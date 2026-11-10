// Translated from GlobalTargetSpells.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells.GlobalTarget;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

public partial class WatcherTE : GlobalTargetSpellTE
{
    public WatcherTE(ECustomNode owner) : base(owner)
    { }

    // GlobalTargetSpells.pas:1476
    public new const int ClassID = 135963;

    // GlobalTargetSpells.pas:1479
    public override bool Process(AbstractUnit source, int value)
    {
        WatcherHS Watcher;
        base.Process(source, value);
        PlayDefaultEffects(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
        Watcher = new WatcherHS(null);
        Watcher.FEnchantment.FPlayerIndex = source.FPlayer;
        var spell = AoWHSSet.Current.FSpellControl.GetSpell(57);
        Watcher.FEnchantment.FEnchantmentManaUpkeep = spell.FManaUpkeep;
        AoWHSMap.Current.PlaceHX(Watcher, Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
        if ((byte)AoWHSMap.Current.FSeatedPlayer == FEventPlayerIndex)
        {
            Watcher.StartAnimation(Unsafe.As<XYL, SpellLocation>(ref FLocation).X, Unsafe.As<XYL, SpellLocation>(ref FLocation).Y, Unsafe.As<XYL, SpellLocation>(ref FLocation).Layer);
        }

        var fSpellControlSpell = AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID);
        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, fSpellControlSpell.FBaseManaCost);
        return false;
    }

    // NetworkE.pas:230

}
