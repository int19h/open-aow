#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiMemory;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.Heap;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.Magic.Abilities;

// ability-bit buffer, bit count
public partial class CustomAbilityList(CustomNode owner) : EngineObject(owner)
{
    public byte[] AbilityBits { get; internal set; } = [];

    public int AbilityCount { get; set; }

    // Ability.PAS:942
    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    // Ability.PAS:1161
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int count = AbilityCount;
        count = persister.Int32("AbilityCount", count);
        if (persister.Loading)
        {
            SetAbilityCount(count);
        }

        int length = (int)(((uint)(AbilityCount + 7)) >> 3);
        persister.BitArray("AbilityBits", AbilityCount, AbilityBits.AsSpan(0, length));
        if (persister.Preview)
        {
            return;
        }

        for (int index = 0; index < AbilityCount; index++)
        {
            if (GetAbilitySetAt(index) && GamePack.Current.AbilityControl.GetAbility(index) is null)
            {
                SetAbilitySet(index, 0);
            }
        }
    }

    // Ability.PAS:948
    public override void Clear()
    {
        base.Clear();
        SetAbilityCount(0);
    }

    // Ability.PAS:1130
    public virtual int GetOwnAbilityCount() => AbilityCount;

    // Ability.PAS:1216
    public virtual void ListAbilities(DelphiStringCollection text, Set<AbilitySelectionType> types)
    {
        text.Clear();
        for (int i = 0, last = AbilityCount - 1; i <= last; i++)
        {
            if (GetAbilitySetAt(i))
            {
                Ability ability = GamePack.Current.AbilityControl.GetAbility(i);
                if ((Set<byte>.FromMask(ability.SelectionTypes.Mask) & Set<byte>.FromMask(types.Mask)) != Set<byte>.WithBounds(0, 15))
                {
                    _ = text.AddObject(ability.GetName(null), IntToObject(i));
                }
            }
        }
    }

    // Ability.PAS:994
    public void SetAbilityCount(int value)
    {
        // Ability.PAS:1119 asserts a bounds predicate that admits every Integer, so no check remains.
        int oldSize = (int)(((uint)(AbilityCount + 7)) >> 3);
        int newSize = (int)(((uint)(value + 7)) >> 3);
        if (newSize != oldSize)
        {
            byte[] abilityBits = AbilityBits;
            Array.Resize(ref abilityBits, newSize);
            AbilityBits = abilityBits;
            if (newSize > oldSize)
            {
                FillBytes(AbilityBits.AsSpan(oldSize, newSize - oldSize), 0);
            }
        }

        AbilityCount = value;
    }

    // Ability.PAS:1134
    public void AssignAbility(CustomAbilityList source)
    {
        int count = source.GetOwnAbilityCount() > GetOwnAbilityCount() ? source.GetOwnAbilityCount() : GetOwnAbilityCount();
        for (int i = 0; i <= count - 1; i++)
        {
            if (source.GetAbilitySetAt(i) != GetAbilitySetAt(i))
            {
                SetAbilitySet(i, (byte)(source.GetAbilitySetAt(i) ? 1 : 0));
            }
        }
    }

    // Ability.PAS:1144
    public bool CompareAbility(CustomAbilityList source)
    {
        bool result = source.GetOwnAbilityCount() == AbilityCount;
        if (result)
        {
            int abilityCount = GetOwnAbilityCount();
            return CompareMem(AbilityBits.AsSpan(0, (int)(((uint)(abilityCount + 7)) >> 3)), source.AbilityBits.AsSpan(0, (int)(((uint)(abilityCount + 7)) >> 3)), (int)(((uint)(abilityCount + 7)) >> 3));
        }

        return result;
    }

    // Ability.PAS:1149
    public bool ComposeAbility(CustomAbilityList source)
    {
        bool result = false;
        for (int i = 0, last = source.GetOwnAbilityCount() - 1; i <= last; i++)
        {
            if (source.GetAbilitySetAt(i) && !GetAbilitySetAt(i))
            {
                SetAbilitySet(i, 1);
                result = true;
            }
        }

        return result;
    }

    // Ability.PAS:1184
    public string GetItemString()
    {
        string result = "";
        for (int i = 0, last = AbilityCount - 1; i <= last; i++)
        {
            if (GetAbilitySetAt(i))
            {
                result = $"{result}{IntToStr(i)},";
            }
        }

        return !IsEmpty(result) ? DelphiStrings.Copy(result, 1, result.Length - 1) : result;
    }

    // Ability.PAS:1193
    public void SetItemString(string S)
    {
        Clear();
        int p = Pos(",", S);
        while (!IsEmpty(S))
        {
            string part;
            if (p != 0)
            {
                part = DelphiStrings.Copy(S, 1, p - 1);
                S = Delete(S, 1, p);
            }
            else
            {
                part = S;
                S = "";
            }

            if (!IsEmpty(part))
            {
                SetAbilitySet(StrToInt(part), 1);
            }

            p = Pos(",", S);
        }
    }

    public virtual void SetAbilitySet(int index, byte value)
    {
        if ((uint)index >= (uint)AbilityCount)
        {
            SetAbilityCount(index + 1);
        }

        byte[] bits = AbilityBits;
        byte mask = (byte)(1 << (index & 7));
        if (value != 0)
        {
            bits[index >> 3] |= mask;
        }
        else
        {
            bits[index >> 3] &= (byte)~mask;
        }
    }

    public virtual bool GetAbilitySetAt(int index) => (uint)index < (uint)AbilityCount && (AbilityBits[index >> 3] & (1 << (index & 7))) != 0;
}
