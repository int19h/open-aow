// Translated from AoWE.pas.
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// (spell list)
public partial class SpellList : IDisposable
{
    // Spells.PAS:695
    public SpellList()
    {
        FSpells = new DelphiList<Spell>();
    }

    public DelphiList<Spell> FSpells;

    // Spells.PAS:700
    public virtual void Dispose()
    {
        FSpells?.Dispose();
    }

    // Spells.PAS:705
    public Spell GetSpell(int Index)
    {
        Spell Result;
        if (Index >= 0 && Index < GetCount())
        {
            Result = FSpells[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Spells.PAS:712
    public int GetCount() => FSpells.Count;

    // Spells.PAS:716
    public void Add(Spell Spell)
    {
        FSpells.Add(Spell);
    }

    // Spells.PAS:720
    public void Delete(int Index)
    {
        FSpells.Delete(Index);
    }

    // Spells.PAS:724
    public void Clear()
    {
        FSpells.Clear();
    }

    // Spells.PAS:734
    public void SortOnName()
    {
        FSpells.Sort(P4CompareSpellNames);
    }

    // Spells.PAS:744
    public void SortOnLevel()
    {
        FSpells.Sort(P4CompareSpellLevels);
    }

    public static int P4CompareSpellNames(Spell Item1, Spell Item2)
    {
        int num = RtlUnit.CompareText(Item1.FSpellName, Item2.FSpellName);
        if (num == 0)
        {
            num = Item1.FSpellLevel - Item2.FSpellLevel;
        }
        return num;
    }

    public static int P4CompareSpellLevels(Spell Item1, Spell Item2)
    {
        int num = Item1.FSpellLevel - Item2.FSpellLevel;
        if (num == 0)
        {
            num = RtlUnit.CompareText(Item1.FSpellName, Item2.FSpellName);
        }
        return num;
    }

    // intf_P4_02.inc:964

    // intf_P4_02.inc:964

}
