using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
namespace Aow.Game.Units;

public partial class UnitSpellCaster
{
    // Spells.PAS:909. The recovered casting view aliases the hero.
    public int GetRequiredMana()
    {
        var hero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(FCasterSourceID);
        return hero is null ? -1 : hero.CastingMana(AoWHSSet.Current.FSpellControl.GetSpell(FCasterSpellID)) * GetSelectionCount();
    }

    // Spells.PAS:925. The remainder uses current points, as in the original.
    public int GetRequiredDays()
    {
        var hero = AoWHSMap.Current.FUnitControl.FindUnit(FCasterSourceID);
        if (hero is null || hero.GetCastingPointsMax() == 0) return 0;
        int deficit = GetRequiredMana() - hero.GetCastingPoints();
        if (deficit <= 0) return 0;
        return deficit / hero.GetCastingPointsMax() + (deficit % hero.GetCastingPoints() != 0 ? 1 : 0);
    }
}
