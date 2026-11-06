namespace Aow.Units.AoWE;

public partial class TUnitSpellCaster
{
    // Spells.PAS:909. The recovered casting view aliases the hero.
    public int GetRequiredMana()
    {
        var hero = (THero)AoWEUnit.AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
        return hero is null ? -1 : hero.CastingMana(AoWEUnit.AoWHSSet.FSpellControl.GetSpell(FCasterSpellID)) * GetSelectionCount();
    }

    // Spells.PAS:925. The remainder uses current points, as in the original.
    public int GetRequiredDays()
    {
        var hero = AoWEUnit.AoWHSMap.FUnitControl.FindUnit(FCasterSourceID);
        if (hero is null || hero.GetCastingPointsMax() == 0) return 0;
        int deficit = GetRequiredMana() - hero.GetCastingPoints();
        if (deficit <= 0) return 0;
        return deficit / hero.GetCastingPointsMax() + (deficit % hero.GetCastingPoints() != 0 ? 1 : 0);
    }
}
