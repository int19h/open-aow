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
public class CustomAbilityList(CustomNode owner) : EngineObject(owner)
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

        int length = (AbilityCount + 7) >>> 3;
        persister.BitArray("AbilityBits", count: AbilityCount, bits: AbilityBits.AsSpan(0, length));
        if (persister.Preview)
        {
            return;
        }

        for (int index = 0; index < AbilityCount; index++)
        {
            if (GetAbilitySetAt(index) && GamePack.Current.AbilityControl[index] is null)
            {
                SetAbilitySet(index, value: 0);
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
    public virtual int OwnAbilityCount => AbilityCount;

    // Ability.PAS:1216
    public virtual void ListAbilities(DelphiStringCollection text, Set<AbilitySelectionType> types)
    {
        text.Clear();
        for (int i = 0, last = AbilityCount - 1; i <= last; i++)
        {
            if (GetAbilitySetAt(i))
            {
                Ability ability = GamePack.Current.AbilityControl[i];
                if ((Set<byte>.FromMask(ability.SelectionTypes.Mask) & Set<byte>.FromMask(types.Mask)) != Set<byte>.WithBounds(0, 15))
                {
                    _ = text.AddObject(ability.GetName(null), item: IntToObject(i));
                }
            }
        }
    }

    // Ability.PAS:994
    public void SetAbilityCount(int value)
    {
        // Ability.PAS:1119 asserts a bounds predicate that admits every Integer, so no check remains.
        int oldSize = (AbilityCount + 7) >>> 3;
        int newSize = (value + 7) >>> 3;
        if (newSize != oldSize)
        {
            byte[] abilityBits = AbilityBits;
            Array.Resize(ref abilityBits, newSize);
            AbilityBits = abilityBits;
            if (newSize > oldSize)
            {
                FillBytes(AbilityBits.AsSpan(oldSize, length: newSize - oldSize), value: 0);
            }
        }

        AbilityCount = value;
    }

    // Ability.PAS:1134
    public void AssignAbility(CustomAbilityList source)
    {
        int count = source.OwnAbilityCount > OwnAbilityCount ? source.OwnAbilityCount : OwnAbilityCount;
        for (int i = 0; i <= count - 1; i++)
        {
            if (source.GetAbilitySetAt(i) != GetAbilitySetAt(i))
            {
                SetAbilitySet(i, value: (byte)(source.GetAbilitySetAt(i) ? 1 : 0));
            }
        }
    }

    // Ability.PAS:1144
    public bool CompareAbility(CustomAbilityList source)
    {
        bool result = source.OwnAbilityCount == AbilityCount;
        if (result)
        {
            int abilityCount = OwnAbilityCount;
            return CompareMem(AbilityBits.AsSpan(0, length: (abilityCount + 7) >>> 3), source.AbilityBits.AsSpan(0, length: (abilityCount + 7) >>> 3), count: (abilityCount + 7) >>> 3);
        }

        return result;
    }

    // Ability.PAS:1149
    public bool ComposeAbility(CustomAbilityList source)
    {
        bool result = false;
        for (int i = 0, last = source.OwnAbilityCount - 1; i <= last; i++)
        {
            if (source.GetAbilitySetAt(i) && !GetAbilitySetAt(i))
            {
                SetAbilitySet(i, value: 1);
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

        return !IsEmpty(result) ? DelphiStrings.Copy(result, index: 1, count: result.Length - 1) : result;
    }

    // Ability.PAS:1193
    public void SetItemString(string S)
    {
        Clear();
        int p = Pos(",", haystack: S);
        while (!IsEmpty(S))
        {
            string part;
            if (p != 0)
            {
                part = DelphiStrings.Copy(S, index: 1, count: p - 1);
                S = Delete(S, index: 1, count: p);
            }
            else
            {
                part = S;
                S = "";
            }

            if (!IsEmpty(part))
            {
                SetAbilitySet(StrToInt(part), value: 1);
            }

            p = Pos(",", haystack: S);
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
