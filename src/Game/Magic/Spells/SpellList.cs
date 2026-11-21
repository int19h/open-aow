#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// (spell list)
public sealed class SpellList : IDisposable
{
    // Spells.PAS:695
    public SpellList() => _spells = [];

    private readonly DelphiList<Spell> _spells;

    // Spells.PAS:700
    public void Dispose() => _spells?.Dispose();

    // Spells.PAS:705
    public Spell? this[int index] => index >= 0 && index < GetCount() ? _spells[index] : null;

    // Spells.PAS:712
    public int GetCount() => _spells.Count;

    // Spells.PAS:716
    public void Add(Spell spell) => _spells.Add(spell);

    // Spells.PAS:720
    public void Delete(int index) => _spells.Delete(index);

    // Spells.PAS:724
    public void Clear() => _spells.Clear();

    // Spells.PAS:734
    public void SortOnName() => _spells.Sort(CompareSpellNames);

    // Spells.PAS:744
    public void SortOnLevel() => _spells.Sort(CompareSpellLevels);

    public static int CompareSpellNames(Spell? item1, Spell? item2)
    {
        int number = DelphiTextCompare.CompareText(item1.SpellName, item2.SpellName);
        if (number == 0)
        {
            number = item1.SpellLevel - item2.SpellLevel;
        }
        return number;
    }

    public static int CompareSpellLevels(Spell? item1, Spell? item2)
    {
        int number = item1.SpellLevel - item2.SpellLevel;
        if (number == 0)
        {
            number = DelphiTextCompare.CompareText(item1.SpellName, item2.SpellName);
        }
        return number;
    }
}
