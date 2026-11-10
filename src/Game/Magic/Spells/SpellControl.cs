// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// (registered-spell list)
public partial class SpellControl : EObject
{

    // Spells.PAS:577
    public SpellControl(ECustomNode owner) : base(owner)
    {
        FRegisteredSpells = new DelphiList();
    }

    public DelphiList FRegisteredSpells;

    // Spells.PAS:582
    public override void Dispose()
    {
        UnregisterAll();
        FRegisteredSpells?.Dispose();
        base.Dispose();
    }

    // Spells.PAS:588
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        for (int I = 0, last = FRegisteredSpells.Count - 1; I <= last; I++)
        {
            if (FRegisteredSpells[I] != null)
            {
                Stream.rwEObject((EObject)FRegisteredSpells[I], I + 10);
            }
        }
    }

    // Spells.PAS:597
    public int GetCount() => FRegisteredSpells.Count;

    // Spells.PAS:601
    public Spell GetSpell(int Index)
    {
        Spell Result;
        object P;
        if (Index < 0)
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, "Invalid SpellID", "D:\\AoWDev\\AoWE\\inc\\Spells.PAS", 54989);
        }

        if (Index < FRegisteredSpells.Count)
        {
            P = FRegisteredSpells[Index];
            Result = (Spell)P;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Spells.PAS:610
    public void RegisterSpell(Spell Spell)
    {
        int ID;
        ID = Spell.FSpellID;
        if (ID != 0)
        {
            if (ID < FRegisteredSpells.Count && FRegisteredSpells[ID] != null)
            {
                throw new InvalidOperationException("Spell already registered (" + IntToStr(ID) + ")");
            }

            while (FRegisteredSpells.Count <= ID)
            {
                FRegisteredSpells.Add(null);
            }

            FRegisteredSpells.List[ID] = Spell;
            Spell.FCastImages.LinkToIL();
        }
        else
        {
            throw new ArgumentException("Invalid SpellID (" + Spell.GetType().Name + ")");
        }
    }

    // Spells.PAS:627
    public void UnregisterSpell(Spell Spell)
    {
        int ID;
        ID = Spell.FSpellID;
        if (ID != 0)
        {
            if (ID < 0 || ID >= FRegisteredSpells.Count || FRegisteredSpells[ID] != Spell)
            {
                throw new InvalidOperationException("Cannot Unregister Spell (" + IntToStr(ID) + ")");
            }

            if (FRegisteredSpells.List[ID] != null)
            {
                Spell.FCastImages.ReleaseIL();
                ((Spell)FRegisteredSpells.List[ID])?.Dispose();
                FRegisteredSpells.List[ID] = null;
            }
        }
        else
        {
            throw new ArgumentException("Invalid SpellID (" + Spell.GetType().Name + ")");
        }
    }

    // Spells.PAS:646
    public void UnregisterSpellID(int SpellID)
    {
        if (FRegisteredSpells.List[SpellID] != null)
        {
            UnregisterSpell((Spell)FRegisteredSpells.List[SpellID]);
        }
    }

    // Spells.PAS:651
    public void UnregisterAll()
    {
        for (int I = FRegisteredSpells.Count - 1; I >= 0; I--)
        {
            if (FRegisteredSpells[I] != null)
            {
                UnregisterSpell((Spell)FRegisteredSpells[I]);
            }
        }
    }

    // Spells.PAS:659
    public void ListSpells(StringCollection SpellStrings, Set<MagicSphere> Spheres)
    {
        SpellStrings.Clear();
        for (int I = 0, last = this.FRegisteredSpells.Count - 1; I <= last; I++)
        {
            if (this.FRegisteredSpells.List[I] != null && Spheres.Contains(((Spell)this.FRegisteredSpells.List[I]).FMagicSphere))
            {
                SpellStrings.AddObject(((Spell)this.FRegisteredSpells.List[I]).FSpellName, IntToObject(I));
            }
        }
    }

    // Spells.PAS:674
    public int GetRandomSpell(int MinLevel, int MaxLevel, Set<MagicSphere> Spheres)
    {
        int Result;
        IntegerList Candidates;
        Spell Spell;
        Candidates = new IntegerList();
        for (int I = this.FRegisteredSpells.Count - 1; I >= 0; I--)
        {
            Spell = (Spell)this.FRegisteredSpells.List[I];
            if (Spell != null && Spheres.Contains(Spell.FMagicSphere) && Spell.FSpellLevel >= MinLevel && Spell.FSpellLevel <= MaxLevel)
            {
                Candidates.Add(I);
            }
        }

        if (Candidates.Count != 0)
        {
            Result = Candidates[AoWHSMap.Current.Random(Candidates.Count)];
        }
        else
        {
            Result = 0;
        }

        Candidates?.Dispose();
        return Result;
    }

    // intf_P4_02.inc:947

}
