// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// (spell list)
public partial class SpellList : IDisposable
{
    // Spells.PAS:695
    public SpellList()
    {
        Spells = new DelphiList<Spell>();
    }

    public DelphiList<Spell> Spells;

    // Spells.PAS:700
    public virtual void Dispose()
    {
        Spells?.Dispose();
    }

    // Spells.PAS:705
    public Spell GetSpell(int index)
    {
        Spell result;
        if (index >= 0 && index < GetCount())
        {
            result = Spells[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Spells.PAS:712
    public int GetCount() => Spells.Count;

    // Spells.PAS:716
    public void Add(Spell spell)
    {
        Spells.Add(spell);
    }

    // Spells.PAS:720
    public void Delete(int index)
    {
        Spells.Delete(index);
    }

    // Spells.PAS:724
    public void Clear()
    {
        Spells.Clear();
    }

    // Spells.PAS:734
    public void SortOnName()
    {
        Spells.Sort(CompareSpellNames);
    }

    // Spells.PAS:744
    public void SortOnLevel()
    {
        Spells.Sort(CompareSpellLevels);
    }

    public static int CompareSpellNames(Spell item1, Spell item2)
    {
        int number = RtlUnit.CompareText(item1.SpellName, item2.SpellName);
        if (number == 0)
        {
            number = item1.SpellLevel - item2.SpellLevel;
        }
        return number;
    }

    public static int CompareSpellLevels(Spell item1, Spell item2)
    {
        int number = item1.SpellLevel - item2.SpellLevel;
        if (number == 0)
        {
            number = RtlUnit.CompareText(item1.SpellName, item2.SpellName);
        }
        return number;
    }

    // intf_P4_02.inc:964

    // intf_P4_02.inc:964

}
