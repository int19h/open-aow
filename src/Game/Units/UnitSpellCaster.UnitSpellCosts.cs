#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.Units;

public partial class UnitSpellCaster
{
    // Spells.PAS:909. The recovered casting view aliases the hero.
    public int GetRequiredMana()
    {
        Hero? hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(CasterSourceId);
        return hero is null ? -1 : hero.CastingMana(GamePack.Current.SpellControl[CasterSpellId]) * GetSelectionCount();
    }

    // Spells.PAS:925. The remainder uses current points, as in the original.
    public int GetRequiredDays()
    {
        Troop? hero = StrategicMap.Current.UnitControl.FindUnit(CasterSourceId);
        if (hero is null || hero.GetCastingPointsMax() == 0)
        {
            return 0;
        }

        int deficit = GetRequiredMana() - hero.GetCastingPoints();
        return deficit <= 0 ? 0 : (deficit / hero.GetCastingPointsMax()) + (deficit % hero.GetCastingPoints() != 0 ? 1 : 0);
    }
}
