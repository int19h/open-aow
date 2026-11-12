using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.Units;

public partial class UnitSpellCaster
{
    // Spells.PAS:909. The recovered casting view aliases the hero.
    public int GetRequiredMana()
    {
        var hero = AoWHSMap.Current.UnitControl.FindUnit<Hero>(CasterSourceId);
        return hero is null ? -1 : hero.CastingMana(AoWHSSet.Current.SpellControl.GetSpell(CasterSpellId)) * GetSelectionCount();
    }

    // Spells.PAS:925. The remainder uses current points, as in the original.
    public int GetRequiredDays()
    {
        var hero = AoWHSMap.Current.UnitControl.FindUnit(CasterSourceId);
        if (hero is null || hero.GetCastingPointsMax() == 0) return 0;
        int deficit = GetRequiredMana() - hero.GetCastingPoints();
        if (deficit <= 0) return 0;
        return deficit / hero.GetCastingPointsMax() + (deficit % hero.GetCastingPoints() != 0 ? 1 : 0);
    }
}
