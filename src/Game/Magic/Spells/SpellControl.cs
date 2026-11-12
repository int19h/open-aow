// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// (registered-spell list)
public partial class SpellControl : EngineObject
{

    // Spells.PAS:577
    public SpellControl(ECustomNode owner) : base(owner)
    {
        RegisteredSpells = new List<Spell>();
    }

    public List<Spell> RegisteredSpells { get; init; }

    // Spells.PAS:582
    public override void Dispose()
    {
        UnregisterAll();
        RegisteredSpells?.Clear();
        base.Dispose();
    }

    // Spells.PAS:588
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        for (int I = 0, last = RegisteredSpells.Count - 1; I <= last; I++)
        {
            if (RegisteredSpells[I] != null)
            {
                stream.Object(RegisteredSpells[I], I + 10);
            }
        }
    }

    // A slot no registered spell asks for stays unread, as ReadWrite leaves its id.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        for (int I = 0, last = RegisteredSpells.Count - 1; I <= last; I++)
        {
            if (RegisteredSpells[I] != null)
            {
                persister.ObjectSlot("Spells", slot: I, objectValue: RegisteredSpells[I]);
            }
        }

        persister.Skip("Spells");
    }

    // Spells.PAS:597
    public int GetCount() => RegisteredSpells.Count;

    // Spells.PAS:601
    public Spell GetSpell(int index)
    {
        Spell result;
        Spell P;
        if (index < 0)
        {
            // { Preserve the original assertion diagnostic. }
            Assert(false, "Invalid SpellID", "D:\\AoWDev\\AoWE\\inc\\Spells.PAS", 54989);
        }

        if (index < RegisteredSpells.Count)
        {
            P = RegisteredSpells[index];
            result = P;
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Spells.PAS:610
    public void RegisterSpell(Spell spell)
    {
        int id;
        id = spell.SpellId;
        if (id != 0)
        {
            if (id < RegisteredSpells.Count && RegisteredSpells[id] != null)
            {
                throw new InvalidOperationException("Spell already registered (" + IntToStr(id) + ")");
            }

            while (RegisteredSpells.Count <= id)
            {
                RegisteredSpells.Add(null);
            }

            RegisteredSpells[id] = spell;
            spell.CastImages.LinkToImageLibrary();
        }
        else
        {
            throw new ArgumentException("Invalid SpellID (" + spell.GetType().Name + ")");
        }
    }

    // Spells.PAS:627
    public void UnregisterSpell(Spell spell)
    {
        int id;
        id = spell.SpellId;
        if (id != 0)
        {
            if (id < 0 || id >= RegisteredSpells.Count || RegisteredSpells[id] != spell)
            {
                throw new InvalidOperationException("Cannot Unregister Spell (" + IntToStr(id) + ")");
            }

            if (RegisteredSpells[id] != null)
            {
                spell.CastImages.ReleaseImageLibrary();
                (RegisteredSpells[id])?.Dispose();
                RegisteredSpells[id] = null;
            }
        }
        else
        {
            throw new ArgumentException("Invalid SpellID (" + spell.GetType().Name + ")");
        }
    }

    // Spells.PAS:646
    public void UnregisterSpellId(int spellId)
    {
        if (RegisteredSpells[spellId] != null)
        {
            UnregisterSpell(RegisteredSpells[spellId]);
        }
    }

    // Spells.PAS:651
    public void UnregisterAll()
    {
        for (int I = RegisteredSpells.Count - 1; I >= 0; I--)
        {
            if (RegisteredSpells[I] != null)
            {
                UnregisterSpell(RegisteredSpells[I]);
            }
        }
    }

    // Spells.PAS:659
    public void ListSpells(StringCollection spellStrings, Set<MagicSphere> spheres)
    {
        spellStrings.Clear();
        for (int I = 0, last = this.RegisteredSpells.Count - 1; I <= last; I++)
        {
            if (this.RegisteredSpells[I] != null && spheres.Contains((this.RegisteredSpells[I]).MagicSphere))
            {
                spellStrings.AddObject((this.RegisteredSpells[I]).SpellName, IntToObject(I));
            }
        }
    }

    // Spells.PAS:674
    public int GetRandomSpell(int minLevel, int maxLevel, Set<MagicSphere> spheres)
    {
        int result;
        Int32List candidates;
        Spell spell;
        candidates = new Int32List();
        for (int I = this.RegisteredSpells.Count - 1; I >= 0; I--)
        {
            spell = this.RegisteredSpells[I];
            if (spell != null && spheres.Contains(spell.MagicSphere) && spell.SpellLevel >= minLevel && spell.SpellLevel <= maxLevel)
            {
                candidates.Add(I);
            }
        }

        if (candidates.Count != 0)
        {
            result = candidates[StrategicMap.Current.Random(candidates.Count)];
        }
        else
        {
            result = 0;
        }

        candidates?.Dispose();
        return result;
    }

    // intf_P4_02.inc:947

}
