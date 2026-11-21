#nullable disable warnings
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.Magic.Spells;

// (registered-spell list)
public sealed class SpellControl(CustomNode? owner) : EngineObject(owner)
{

    public List<Spell?> RegisteredSpells { get; init; } = [];

    // Spells.PAS:582
    public override void Dispose()
    {
        UnregisterAll();
        RegisteredSpells?.Clear();
        base.Dispose();
    }

    // Spells.PAS:588
    // A slot no registered spell asks for stays unread, as the original ReadWrite leaves its id.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        for (int i = 0, last = RegisteredSpells.Count - 1; i <= last; i++)
        {
            if (RegisteredSpells[i] is not null)
            {
                persister.ObjectSlot("RegisteredSpells", slot: i, objectValue: RegisteredSpells[i]);
            }
        }

        persister.Skip("RegisteredSpells");
    }

    // Spells.PAS:597
    public int GetCount() => RegisteredSpells.Count;

    // Spells.PAS:601
    public Spell? this[int index]
    {
        get
        {
            if (index < 0)
            {
                // { Preserve the original assertion diagnostic. }
                Assert(false, message: "Invalid SpellID", file: "D:\\AoWDev\\AoWE\\inc\\Spells.PAS", line: 54989);
            }
            if (index < RegisteredSpells.Count)
            {
                Spell? p = RegisteredSpells[index];
                return p;
            }
            else
            {
                return null;
            }
        }
    }

    // RegisterSpell stores exactly one instance per id and refuses a second registration. The caller names
    // the kind registered at that id. Another class throws InvalidCastException. An id past the list returns
    // null, as GetSpell does.
    public T GetSpell<T>(int index) where T : Spell => (T)this[index];

    // Spells.PAS:610
    public void RegisterSpell(Spell spell)
    {
        int id = spell.SpellId;
        if (id != 0)
        {
            if (id < RegisteredSpells.Count && RegisteredSpells[id] is not null)
            {
                throw new InvalidOperationException($"Spell already registered ({id.ToString(CultureInfo.InvariantCulture)})");
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
            throw new ArgumentException($"Invalid SpellID ({spell.GetType().Name})");
        }
    }

    // Spells.PAS:627
    public void UnregisterSpell(Spell? spell)
    {
        int id = spell.SpellId;
        if (id != 0)
        {
            if (id < 0 || id >= RegisteredSpells.Count || RegisteredSpells[id] != spell)
            {
                throw new InvalidOperationException($"Cannot Unregister Spell ({id.ToString(CultureInfo.InvariantCulture)})");
            }

            if (RegisteredSpells[id] is not null)
            {
                spell.CastImages.ReleaseImageLibrary();
                RegisteredSpells[id]?.Dispose();
                RegisteredSpells[id] = null;
            }
        }
        else
        {
            throw new ArgumentException($"Invalid SpellID ({spell.GetType().Name})");
        }
    }

    // Spells.PAS:646
    public void UnregisterSpellId(int spellId)
    {
        if (RegisteredSpells[spellId] is not null)
        {
            UnregisterSpell(RegisteredSpells[spellId]);
        }
    }

    // Spells.PAS:651
    public void UnregisterAll()
    {
        for (int i = RegisteredSpells.Count - 1; i >= 0; i--)
        {
            if (RegisteredSpells[i] is not null)
            {
                UnregisterSpell(RegisteredSpells[i]);
            }
        }
    }

    // Spells.PAS:659
    public void ListSpells(DelphiStringCollection spellStrings, MagicSpheres spheres)
    {
        spellStrings.Clear();
        for (int i = 0, last = RegisteredSpells.Count - 1; i <= last; i++)
        {
            if (RegisteredSpells[i] is not null && spheres.Contains(RegisteredSpells[i].MagicSphere.ToFlag()))
            {
                _ = spellStrings.AddObject(RegisteredSpells[i].SpellName, item: IntToObject(i));
            }
        }
    }

    // Spells.PAS:674
    public int GetRandomSpell(int minLevel, int maxLevel, MagicSpheres spheres)
    {
        Int32List candidates = [];
        for (int i = RegisteredSpells.Count - 1; i >= 0; i--)
        {
            Spell? spell = RegisteredSpells[i];
            if (spell is not null && spheres.Contains(spell.MagicSphere.ToFlag()) && spell.SpellLevel >= minLevel && spell.SpellLevel <= maxLevel)
            {
                _ = candidates.Add(i);
            }
        }

        int result = candidates.Count != 0 ? candidates[StrategicMap.Current.Random(candidates.Count)] : 0;

        candidates?.Dispose();
        return result;
    }
}
