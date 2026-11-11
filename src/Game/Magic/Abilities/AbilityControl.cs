// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

// (FAbilities, FAbilitiesByType,)
public partial class AbilityControl : EngineObject
{
    // Ability.PAS:2035
    public AbilityControl(ECustomNode owner) : base(owner)
    {
        AbilityType Kind;

        FAbilities = new List<Ability>();
        for (int kindOrdinal = (int)(AbilityType)0; kindOrdinal <= (int)(AbilityType)4; kindOrdinal++)
        {
            Kind = (AbilityType)kindOrdinal;
            var abilityTypeList = new AbilityTypeList();
            FAbilitiesByType[(int)Kind] = abilityTypeList;
        }
    }

    public List<Ability> FAbilities;
    public AbilityTypeListArray5 FAbilitiesByType;

    // Ability.PAS:2044
    public override void Dispose()
    {
        AbilityType Kind;
        UnregisterAll();
        for (int kindOrdinal = (int)(AbilityType)0; kindOrdinal <= (int)(AbilityType)4; kindOrdinal++)
        {
            Kind = (AbilityType)kindOrdinal;
            FAbilitiesByType[(int)Kind]?.Dispose();
        }

        FAbilities?.Clear();
        base.Dispose();
    }

    // Ability.PAS:2053
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
        {
            if (FAbilities[I] != null)
            {
                Stream.rwEObject(FAbilities[I], I + 10);
            }
        }
    }

    // Ability.PAS:2061
    public int GetCount() => FAbilities.Count;

    // Ability.PAS:2078
    public void RegisterAbility(Ability Ability)
    {
        int ID;
        ID = Ability.FAbilityID;
        if (ID != -1)
        {
            if (ID < this.FAbilities.Count && this.FAbilities[ID] != null)
            {
                throw new InvalidOperationException("Ability already registered (" + IntToStr(ID) + ")");
            }

            while (ID >= FAbilities.Count)
            {
                FAbilities.Add(null);
            }

            FAbilities[ID] = Ability;
            var abilityType = Ability.GetAbilityType();
            FAbilitiesByType[(int)abilityType].Add(Ability);
            Ability.FImageSequences.LinkToIL();
        }
        else
        {
            throw new ArgumentException("Invalid AbilityID (" + Ability.GetType().Name + ")");
        }
    }

    // Ability.PAS:2096
    public void UnregisterAbility(Ability Ability)
    {
        int ID;
        ID = Ability.FAbilityID;
        if (ID != -1)
        {
            if (ID < 0 || ID >= FAbilities.Count || FAbilities[ID] != Ability)
            {
                throw new InvalidOperationException("Cannot Unregister Ability (" + IntToStr(ID) + ")");
            }

            if (FAbilities[ID] != null)
            {
                var abilityType = Ability.GetAbilityType();
                FAbilitiesByType[(int)abilityType].Remove(Ability);
                (FAbilities[ID]).FImageSequences.ReleaseIL();
                (FAbilities[ID] as IDisposable)?.Dispose();
                FAbilities[ID] = null;
            }
        }
        else
        {
            throw new ArgumentException("Invalid AbilityID (" + Ability.GetType().Name + ")");
        }
    }

    // Ability.PAS:2116
    public void UnregisterAll()
    {
        for (int I = FAbilities.Count - 1; I >= 0; I--)
        {
            if (FAbilities[I] != null)
            {
                UnregisterAbility(FAbilities[I]);
            }
        }
    }

    // Ability.PAS:2129
    public void ListAbilities(StringCollection Text, Set<AbilitySelectionType> Types)
    {
        Text.Clear();
        for (int I = 0, last = this.FAbilities.Count - 1; I <= last; I++)
        {
            if (this.FAbilities[I] != null)
            {
                if (((this.FAbilities[I]).FSelectionTypes & Types) != Set<AbilitySelectionType>.Of() || (Types == Set<AbilitySelectionType>.Of().WithRange(0, (AbilitySelectionType)9) && (this.FAbilities[I]).FSelectionTypes == Set<AbilitySelectionType>.Of()))
                {
                    Text.AddObject((this.FAbilities[I]).GetName(null), IntToObject(I));
                }
            }
        }
    }

    // Ability.PAS:2148
    public void ListAbilityInfo(StringCollection Text, bool SortByName, Set<AbilityInfoModeElement> Mode)
    {
        DelphiList List = default;
        List = new DelphiList();
        for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
        {
            if (FAbilities[I] != null)
            {
                List.Add(FAbilities[I]);
            }
        }

        if (SortByName)
        {
            List.Sort(CompareAbilityNames);
        }

        // { Original frees the temporary list only after all ability callbacks succeed. }
        for (int I = 0, last = List.Count - 1; I <= last; I++)
        {
            ((Ability)List[I]).ListInfo(Text, Mode);
            Text.Add("-------------------------------------------------");
            Text.Add("");
        }

        List?.Dispose();
    }

    // intf_P1_06.inc:178
    public void UnregisterAbilityID(int id)
    {
        if (FAbilities[id] is Ability ability) UnregisterAbility(ability);
    }

    public Ability GetAbility(int index)
    {
        Assert(index >= 0, "Invalid AbilityID", "Ability.PAS", 2069);
        return index < FAbilities.Count ? FAbilities[index] : null;
    }

    public static int CompareAbilityNames(object Item1, object Item2)
    {
        var name = ((Ability)Item2).GetName(null);
        return CompareStr(((Ability)Item1).GetName(null), name);
    }

    internal static void UnregisterPassiveAbilities(AbilityControl Control)
    {
        Control.UnregisterAbilityID(0);
        Control.UnregisterAbilityID(1);
        Control.UnregisterAbilityID(2);
        Control.UnregisterAbilityID(3);
        Control.UnregisterAbilityID(4);
        Control.UnregisterAbilityID(5);
        Control.UnregisterAbilityID(23);
        Control.UnregisterAbilityID(42);
        Control.UnregisterAbilityID(6);
        Control.UnregisterAbilityID(7);
        Control.UnregisterAbilityID(8);
        Control.UnregisterAbilityID(9);
        Control.UnregisterAbilityID(10);
        Control.UnregisterAbilityID(11);
        Control.UnregisterAbilityID(12);
        Control.UnregisterAbilityID(13);
        Control.UnregisterAbilityID(70);
        Control.UnregisterAbilityID(71);
        Control.UnregisterAbilityID(76);
        Control.UnregisterAbilityID(72);
        Control.UnregisterAbilityID(73);
        Control.UnregisterAbilityID(74);
        Control.UnregisterAbilityID(75);
        Control.UnregisterAbilityID(77);
        Control.UnregisterAbilityID(64);
        Control.UnregisterAbilityID(146);
        Control.UnregisterAbilityID(147);
        Control.UnregisterAbilityID(26);
        Control.UnregisterAbilityID(36);
        Control.UnregisterAbilityID(66);
        Control.UnregisterAbilityID(39);
        Control.UnregisterAbilityID(43);
        Control.UnregisterAbilityID(49);
        Control.UnregisterAbilityID(51);
        Control.UnregisterAbilityID(54);
        Control.UnregisterAbilityID(63);
        Control.UnregisterAbilityID(64);
        Control.UnregisterAbilityID(37);
        Control.UnregisterAbilityID(41);
        Control.UnregisterAbilityID(67);
        Control.UnregisterAbilityID(68);
        Control.UnregisterAbilityID(69);
        Control.UnregisterAbilityID(109);
        Control.UnregisterAbilityID(32);
        Control.UnregisterAbilityID(129);
        Control.UnregisterAbilityID(128);
        Control.UnregisterAbilityID(130);
        Control.UnregisterAbilityID(93);
        Control.UnregisterAbilityID(94);
        Control.UnregisterAbilityID(95);
        Control.UnregisterAbilityID(96);
        Control.UnregisterAbilityID(97);
        Control.UnregisterAbilityID(98);
        Control.UnregisterAbilityID(92);
        Control.UnregisterAbilityID(99);
        Control.UnregisterAbilityID(168);
        Control.UnregisterAbilityID(34);
        Control.UnregisterAbilityID(53);
        Control.UnregisterAbilityID(152);
        Control.UnregisterAbilityID(153);
        Control.UnregisterAbilityID(154);
        Control.UnregisterAbilityID(155);
        Control.UnregisterAbilityID(156);
        Control.UnregisterAbilityID(157);
        Control.UnregisterAbilityID(160);
        Control.UnregisterAbilityID(161);
        Control.UnregisterAbilityID(162);
        Control.UnregisterAbilityID(163);
        Control.UnregisterAbilityID(164);
        Control.UnregisterAbilityID(165);
        Control.UnregisterAbilityID(166);
        Control.UnregisterAbilityID(158);
        Control.UnregisterAbilityID(126);
        Control.UnregisterAbilityID(127);
        Control.UnregisterAbilityID(167);
        Control.UnregisterAbilityID(108);
        Control.UnregisterAbilityID(131);
        Control.UnregisterAbilityID(100);
        Control.UnregisterAbilityID(169);
        Control.UnregisterAbilityID(132);
        Control.UnregisterAbilityID(101);
        Control.UnregisterAbilityID(138);
        Control.UnregisterAbilityID(139);
        Control.UnregisterAbilityID(140);
        Control.UnregisterAbilityID(141);
        Control.UnregisterAbilityID(142);
        Control.UnregisterAbilityID(143);
        Control.UnregisterAbilityID(144);
        Control.UnregisterAbilityID(145);
    }

}
