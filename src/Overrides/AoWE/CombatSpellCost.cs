namespace Aow.Units.AoWE;

public partial class TSpell
{
    // Spells.PAS:215. The casting view aliases the combat unit's hero.
    public void CombatSpellCast(TCombatUnit source) =>
        CombatCastingDone(source, ((THero)source.FUnit).CastingMana(this));
}
