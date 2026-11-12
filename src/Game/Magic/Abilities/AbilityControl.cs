// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

// (Abilities, AbilitiesByType,)
public partial class AbilityControl : EngineObject
{
    // Ability.PAS:2035
    public AbilityControl(ECustomNode owner) : base(owner)
    {
        AbilityType kind;

        Abilities = new List<Ability>();
        for (int kindOrdinal = (int)(AbilityType)0; kindOrdinal <= (int)(AbilityType)4; kindOrdinal++)
        {
            kind = (AbilityType)kindOrdinal;
            var abilityTypeList = new AbilityTypeList();
            AbilitiesByType[(int)kind] = abilityTypeList;
        }
    }

    public List<Ability> Abilities;
    public AbilityTypeListArray5 AbilitiesByType;

    // Ability.PAS:2044
    public override void Dispose()
    {
        AbilityType kind;
        UnregisterAll();
        for (int kindOrdinal = (int)(AbilityType)0; kindOrdinal <= (int)(AbilityType)4; kindOrdinal++)
        {
            kind = (AbilityType)kindOrdinal;
            AbilitiesByType[(int)kind]?.Dispose();
        }

        Abilities?.Clear();
        base.Dispose();
    }

    // Ability.PAS:2053
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        for (int I = 0, last = Abilities.Count - 1; I <= last; I++)
        {
            if (Abilities[I] != null)
            {
                stream.rwEObject(Abilities[I], I + 10);
            }
        }
    }

    // Ability.PAS:2061
    public int GetCount() => Abilities.Count;

    // Ability.PAS:2078
    public void RegisterAbility(Ability ability)
    {
        int id;
        id = ability.AbilityId;
        if (id != -1)
        {
            if (id < this.Abilities.Count && this.Abilities[id] != null)
            {
                throw new InvalidOperationException("Ability already registered (" + IntToStr(id) + ")");
            }

            while (id >= Abilities.Count)
            {
                Abilities.Add(null);
            }

            Abilities[id] = ability;
            var abilityType = ability.GetAbilityType();
            AbilitiesByType[(int)abilityType].Add(ability);
            ability.ImageSequences.LinkToImageLibrary();
        }
        else
        {
            throw new ArgumentException("Invalid AbilityID (" + ability.GetType().Name + ")");
        }
    }

    // Ability.PAS:2096
    public void UnregisterAbility(Ability ability)
    {
        int id;
        id = ability.AbilityId;
        if (id != -1)
        {
            if (id < 0 || id >= Abilities.Count || Abilities[id] != ability)
            {
                throw new InvalidOperationException("Cannot Unregister Ability (" + IntToStr(id) + ")");
            }

            if (Abilities[id] != null)
            {
                var abilityType = ability.GetAbilityType();
                AbilitiesByType[(int)abilityType].Remove(ability);
                (Abilities[id]).ImageSequences.ReleaseImageLibrary();
                (Abilities[id] as IDisposable)?.Dispose();
                Abilities[id] = null;
            }
        }
        else
        {
            throw new ArgumentException("Invalid AbilityID (" + ability.GetType().Name + ")");
        }
    }

    // Ability.PAS:2116
    public void UnregisterAll()
    {
        for (int I = Abilities.Count - 1; I >= 0; I--)
        {
            if (Abilities[I] != null)
            {
                UnregisterAbility(Abilities[I]);
            }
        }
    }

    // Ability.PAS:2129
    public void ListAbilities(StringCollection text, Set<AbilitySelectionType> types)
    {
        text.Clear();
        for (int I = 0, last = this.Abilities.Count - 1; I <= last; I++)
        {
            if (this.Abilities[I] != null)
            {
                if (((this.Abilities[I]).SelectionTypes & types) != Set<AbilitySelectionType>.Of() || (types == Set<AbilitySelectionType>.Of().WithRange(0, (AbilitySelectionType)9) && (this.Abilities[I]).SelectionTypes == Set<AbilitySelectionType>.Of()))
                {
                    text.AddObject((this.Abilities[I]).GetName(null), IntToObject(I));
                }
            }
        }
    }

    // Ability.PAS:2148
    public void ListAbilityInfo(StringCollection text, bool sortByName, Set<AbilityInfoModeElement> mode)
    {
        DelphiList list = default;
        list = new DelphiList();
        for (int I = 0, last = Abilities.Count - 1; I <= last; I++)
        {
            if (Abilities[I] != null)
            {
                list.Add(Abilities[I]);
            }
        }

        if (sortByName)
        {
            list.Sort(CompareAbilityNames);
        }

        // { Original frees the temporary list only after all ability callbacks succeed. }
        for (int I = 0, last = list.Count - 1; I <= last; I++)
        {
            ((Ability)list[I]).ListInfo(text, mode);
            text.Add("-------------------------------------------------");
            text.Add("");
        }

        list?.Dispose();
    }

    // intf_P1_06.inc:178
    public void UnregisterAbilityId(int id)
    {
        if (Abilities[id] is Ability ability) UnregisterAbility(ability);
    }

    public Ability GetAbility(int index)
    {
        Assert(index >= 0, "Invalid AbilityID", "Ability.PAS", 2069);
        return index < Abilities.Count ? Abilities[index] : null;
    }

    public static int CompareAbilityNames(object item1, object item2)
    {
        var name = ((Ability)item2).GetName(null);
        return CompareStr(((Ability)item1).GetName(null), name);
    }

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
