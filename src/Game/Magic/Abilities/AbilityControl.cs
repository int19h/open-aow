#nullable disable warnings
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Rtl;

using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.Magic.Abilities;

// (Abilities, AbilitiesByType,)
public sealed class AbilityControl : EngineObject
{
    private const AbilitySelectionTypes AllAbilitySelectionTypes = AbilitySelectionTypes.Unit | AbilitySelectionTypes.HeadItem | AbilitySelectionTypes.TorsoItem | AbilitySelectionTypes.AttackItem | AbilitySelectionTypes.DefenseItem | AbilitySelectionTypes.RingItem | AbilitySelectionTypes.UseItem | AbilitySelectionTypes.CustomizeLeader | AbilitySelectionTypes.HeroUpgrade | AbilitySelectionTypes.Editor;

    // Ability.PAS:2035
    public AbilityControl(CustomNode? owner) : base(owner)
    {
        Abilities = [];
        for (int kindOrdinal = (int)AbilityType.Enhancement; kindOrdinal <= (int)AbilityType.Enchantment; kindOrdinal++)
        {
            AbilityType kind = (AbilityType)kindOrdinal;
            AbilityTypeList abilityTypeList = [];
            AbilitiesByType[(int)kind] = abilityTypeList;
        }
    }

    public List<Ability?> Abilities { get; }

    public AbilityTypeList[] AbilitiesByType { get; } = new AbilityTypeList[5];

    // Ability.PAS:2044
    public override void Dispose()
    {
        UnregisterAll();
        for (int kindOrdinal = (int)AbilityType.Enhancement; kindOrdinal <= (int)AbilityType.Enchantment; kindOrdinal++)
        {
            AbilityType kind = (AbilityType)kindOrdinal;
            AbilitiesByType[(int)kind]?.Dispose();
        }

        Abilities?.Clear();
        base.Dispose();
    }

    // Ability.PAS:2053
    // A slot no registered ability asks for stays unread, as the original ReadWrite leaves its id.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        for (int i = 0, last = Abilities.Count - 1; i <= last; i++)
        {
            if (Abilities[i] is not null)
            {
                persister.ObjectSlot("Abilities", slot: i, objectValue: Abilities[i]);
            }
        }

        persister.Skip("Abilities");
    }

    // Ability.PAS:2061
    public int GetCount() => Abilities.Count;

    // Ability.PAS:2078
    public void RegisterAbility(Ability ability)
    {
        int id = ability.AbilityId;
        if (id != -1)
        {
            if (id < Abilities.Count && Abilities[id] is not null)
            {
                throw new InvalidOperationException($"Ability already registered ({id.ToString(CultureInfo.InvariantCulture)})");
            }

            while (id >= Abilities.Count)
            {
                Abilities.Add(null);
            }

            Abilities[id] = ability;
            AbilityType abilityType = ability.GetAbilityType();
            _ = AbilitiesByType[(int)abilityType].Add(ability);
            ability.ImageSequences.LinkToImageLibrary();
        }
        else
        {
            throw new ArgumentException($"Invalid AbilityID ({ability.GetType().Name})");
        }
    }

    // Ability.PAS:2096
    public void UnregisterAbility(Ability? ability)
    {
        int id = ability.AbilityId;
        if (id != -1)
        {
            if (id < 0 || id >= Abilities.Count || Abilities[id] != ability)
            {
                throw new InvalidOperationException($"Cannot Unregister Ability ({id.ToString(CultureInfo.InvariantCulture)})");
            }

            if (Abilities[id] is not null)
            {
                AbilityType abilityType = ability.GetAbilityType();
                _ = AbilitiesByType[(int)abilityType].Remove(ability);
                Abilities[id].ImageSequences.ReleaseImageLibrary();
                (Abilities[id] as IDisposable)?.Dispose();
                Abilities[id] = null;
            }
        }
        else
        {
            throw new ArgumentException($"Invalid AbilityID ({ability.GetType().Name})");
        }
    }

    // Ability.PAS:2116
    public void UnregisterAll()
    {
        for (int i = Abilities.Count - 1; i >= 0; i--)
        {
            if (Abilities[i] is not null)
            {
                UnregisterAbility(Abilities[i]);
            }
        }
    }

    // Ability.PAS:2129
    public void ListAbilities(DelphiStringCollection text, AbilitySelectionTypes types)
    {
        text.Clear();
        for (int i = 0, last = Abilities.Count - 1; i <= last; i++)
        {
            if (Abilities[i] is not null)
            {
                if ((Abilities[i].SelectionTypes & types) != AbilitySelectionTypes.None || (types == AllAbilitySelectionTypes && Abilities[i].SelectionTypes == AbilitySelectionTypes.None))
                {
                    _ = text.AddObject(Abilities[i].GetName(null), item: IntToObject(i));
                }
            }
        }
    }

    // Ability.PAS:2148
    public void ListAbilityInfo(DelphiStringCollection text, bool sortByName, AbilityInfoModes mode)
    {
        DelphiList<Ability> list = [.. Abilities.Where(ability => ability is not null)];

        if (sortByName)
        {
            list.Sort(CompareAbilityNames);
        }

        // { Original frees the temporary list only after all ability callbacks succeed. }
        for (int i = 0, last = list.Count - 1; i <= last; i++)
        {
            list[i].ListInfo(text, mode);
            _ = text.Add("-------------------------------------------------");
            _ = text.Add("");
        }
    }

    // intf_P1_06.inc:178
    public void UnregisterAbilityId(int id)
    {
        if (Abilities[id] is Ability ability)
        {
            UnregisterAbility(ability);
        }
    }

    public Ability? this[int index]
    {
        get
        {
            Assert(index >= 0, message: "Invalid AbilityID", file: "Ability.PAS", line: 2069);
            return index < Abilities.Count ? Abilities[index] : null;
        }
    }

    // RegisterAbility stores exactly one instance per id and refuses a second registration. The caller names
    // the kind registered at that id. Another class throws InvalidCastException. An id past the list returns
    // null, as GetAbility does.
    public T GetAbility<T>(int id) where T : Ability => (T)this[id];

    public static int CompareAbilityNames(Ability? item1, Ability? item2) => CompareStr(b: item2.GetName(null), a: item1.GetName(null));

    internal static void UnregisterPassiveAbilities(AbilityControl control)
    {
        control.UnregisterAbilityId(0);
        control.UnregisterAbilityId(1);
        control.UnregisterAbilityId(2);
        control.UnregisterAbilityId(3);
        control.UnregisterAbilityId(4);
        control.UnregisterAbilityId(5);
        control.UnregisterAbilityId(23);
        control.UnregisterAbilityId(42);
        control.UnregisterAbilityId(6);
        control.UnregisterAbilityId(7);
        control.UnregisterAbilityId(8);
        control.UnregisterAbilityId(9);
        control.UnregisterAbilityId(10);
        control.UnregisterAbilityId(11);
        control.UnregisterAbilityId(12);
        control.UnregisterAbilityId(13);
        control.UnregisterAbilityId(70);
        control.UnregisterAbilityId(71);
        control.UnregisterAbilityId(76);
        control.UnregisterAbilityId(72);
        control.UnregisterAbilityId(73);
        control.UnregisterAbilityId(74);
        control.UnregisterAbilityId(75);
        control.UnregisterAbilityId(77);
        control.UnregisterAbilityId(64);
        control.UnregisterAbilityId(146);
        control.UnregisterAbilityId(147);
        control.UnregisterAbilityId(26);
        control.UnregisterAbilityId(36);
        control.UnregisterAbilityId(66);
        control.UnregisterAbilityId(39);
        control.UnregisterAbilityId(43);
        control.UnregisterAbilityId(49);
        control.UnregisterAbilityId(51);
        control.UnregisterAbilityId(54);
        control.UnregisterAbilityId(63);
        control.UnregisterAbilityId(64);
        control.UnregisterAbilityId(37);
        control.UnregisterAbilityId(41);
        control.UnregisterAbilityId(67);
        control.UnregisterAbilityId(68);
        control.UnregisterAbilityId(69);
        control.UnregisterAbilityId(109);
        control.UnregisterAbilityId(32);
        control.UnregisterAbilityId(129);
        control.UnregisterAbilityId(128);
        control.UnregisterAbilityId(130);
        control.UnregisterAbilityId(93);
        control.UnregisterAbilityId(94);
        control.UnregisterAbilityId(95);
        control.UnregisterAbilityId(96);
        control.UnregisterAbilityId(97);
        control.UnregisterAbilityId(98);
        control.UnregisterAbilityId(92);
        control.UnregisterAbilityId(99);
        control.UnregisterAbilityId(168);
        control.UnregisterAbilityId(34);
        control.UnregisterAbilityId(53);
        control.UnregisterAbilityId(152);
        control.UnregisterAbilityId(153);
        control.UnregisterAbilityId(154);
        control.UnregisterAbilityId(155);
        control.UnregisterAbilityId(156);
        control.UnregisterAbilityId(157);
        control.UnregisterAbilityId(160);
        control.UnregisterAbilityId(161);
        control.UnregisterAbilityId(162);
        control.UnregisterAbilityId(163);
        control.UnregisterAbilityId(164);
        control.UnregisterAbilityId(165);
        control.UnregisterAbilityId(166);
        control.UnregisterAbilityId(158);
        control.UnregisterAbilityId(126);
        control.UnregisterAbilityId(127);
        control.UnregisterAbilityId(167);
        control.UnregisterAbilityId(108);
        control.UnregisterAbilityId(131);
        control.UnregisterAbilityId(100);
        control.UnregisterAbilityId(169);
        control.UnregisterAbilityId(132);
        control.UnregisterAbilityId(101);
        control.UnregisterAbilityId(138);
        control.UnregisterAbilityId(139);
        control.UnregisterAbilityId(140);
        control.UnregisterAbilityId(141);
        control.UnregisterAbilityId(142);
        control.UnregisterAbilityId(143);
        control.UnregisterAbilityId(144);
        control.UnregisterAbilityId(145);
    }
}
