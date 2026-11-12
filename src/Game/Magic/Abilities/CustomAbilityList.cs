// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

// ability-bit buffer, bit count
public partial class CustomAbilityList : EngineObject
{

    public CustomAbilityList(ECustomNode owner) : base(owner)
    { }

    public byte[] AbilityBits = [];
    public int AbilityCount;

    // Ability.PAS:942
    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    // Ability.PAS:1161
    public override void ReadWrite(EStorageStream stream)
    {
        int I = default;
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of())
        {
            if (stream.StorageMode().Contains(StorageModeElement.Read))
            {
                stream.rwInteger(ref I, 2);
                SetAbilityCount(I);
                stream.rwData(AbilityBits.AsSpan(0, (int)(((uint)(AbilityCount + 7)) >> 3)), (int)(((uint)(AbilityCount + 7)) >> 3), 3);
            }
            else
            {
                stream.rwInteger(ref AbilityCount, 2);
                stream.rwData(AbilityBits.AsSpan(0, (int)(((uint)(AbilityCount + 7)) >> 3)), (int)(((uint)(AbilityCount + 7)) >> 3), 3);
            }
        }

        if (!stream.StorageMode().Contains(StorageModeElement.Preview))
        {
            I = 0;
            int last = AbilityCount - 1;
            for (; I <= last; I += 1)
            {
                if (GetAbilitySetAt(I) && AoWHSSet.Current.AbilityControl.GetAbility(I) == null)
                {
                    SetAbilitySet(I, 0);
                }
            }
        }
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int count = AbilityCount;
        persister.Int32("AbCount", ref count);
        if (persister.Loading) SetAbilityCount(count);
        int length = (int)(((uint)(AbilityCount + 7)) >> 3);
        persister.BitArray("AbilityCount", AbilityCount, AbilityBits.AsSpan(0, length));
        for (int index = 0; index < AbilityCount; index++)
        {
            if (GetAbilitySetAt(index) && AoWHSSet.Current.AbilityControl.GetAbility(index) == null)
                SetAbilitySet(index, 0);
        }
    }

    // Ability.PAS:948
    public override void Clear()
    {
        base.Clear();
        SetAbilityCount(0);
    }

    // Ability.PAS:1130
    public virtual int GetAbCount() => AbilityCount;

    // Ability.PAS:1216
    public virtual void ListAbilities(StringCollection text, Set<AbilitySelectionType> types)
    {
        Ability ability;
        text.Clear();
        for (int I = 0, last = AbilityCount - 1; I <= last; I++)
        {
            if (GetAbilitySetAt(I))
            {
                ability = AoWHSSet.Current.AbilityControl.GetAbility(I);
                if ((Set<byte>.FromMask(ability.SelectionTypes.Mask) & Set<byte>.FromMask(types.Mask)) != Set<byte>.WithBounds(0, 15))
                {
                    text.AddObject(ability.GetName(null), IntToObject(I));
                }
            }
        }
    }

    // Ability.PAS:994
    public void SetAbilityCount(int value)
    {
        int oldSize;
        int newSize;
        // { Original assertion accepts every Integer: its bounds predicate uses OR. }
        Assert(!(value < 0 && value >= 134217727), "Invalid AbCount Value (TArrayList)", "Ability.PAS", 1119);
        oldSize = (int)(((uint)(AbilityCount + 7)) >> 3);
        newSize = (int)(((uint)(value + 7)) >> 3);
        if (newSize != oldSize)
        {
            Array.Resize(ref AbilityBits, newSize);
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
        int count;
        if (source.GetAbCount() > GetAbCount())
        {
            count = source.GetAbCount();
        }
        else
        {
            count = GetAbCount();
        }

        for (int I = 0; I <= count - 1; I++)
        {
            if (source.GetAbilitySetAt(I) != GetAbilitySetAt(I))
            {
                SetAbilitySet(I, (byte)(source.GetAbilitySetAt(I) ? 1 : 0));
            }
        }
    }

    // Ability.PAS:1144
    public bool CompareAbility(CustomAbilityList source)
    {
        bool result = source.GetAbCount() == AbilityCount;
        if (result)
        {
            var abilityCount = GetAbCount();
            result = CompareMem(AbilityBits.AsSpan(0, (int)(((uint)(abilityCount + 7)) >> 3)), source.AbilityBits.AsSpan(0, (int)(((uint)(abilityCount + 7)) >> 3)), (int)(((uint)(abilityCount + 7)) >> 3));
        }

        return result;
    }

    // Ability.PAS:1149
    public bool ComposeAbility(CustomAbilityList source)
    {
        bool result = false;
        for (int I = 0, last = source.GetAbCount() - 1; I <= last; I++)
        {
            if (source.GetAbilitySetAt(I) && !GetAbilitySetAt(I))
            {
                SetAbilitySet(I, 1);
                result = true;
            }
        }

        return result;
    }

    // Ability.PAS:1184
    public string GetItemString()
    {
        string result = "";
        for (int I = 0, last = AbilityCount - 1; I <= last; I++)
        {
            if (GetAbilitySetAt(I))
            {
                result = result + IntToStr(I) + ",";
            }
        }

        if (!IsEmpty(result))
        {
            result = RtlUnit.Copy(result, 1, result.Length - 1);
        }

        return result;
    }

    // Ability.PAS:1193
    public void SetItemString(string S)
    {
        string part;
        int P;
        Clear();
        P = Pos(",", S);
        while (!IsEmpty(S))
        {
            if (P != 0)
            {
                part = RtlUnit.Copy(S, 1, P - 1);
                S = Delete(S, 1, P);
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

            P = Pos(",", S);
        }
    }

    public virtual void SetAbilitySet(int index, byte value)
    {
        if ((uint)index >= (uint)AbilityCount) SetAbilityCount(index + 1);
        var bits = AbilityBits;
        byte mask = (byte)(1 << (index & 7));
        if (value != 0) bits[index >> 3] |= mask;
        else bits[index >> 3] &= (byte)~mask;
    }

    public virtual bool GetAbilitySetAt(int index) => (uint)index < (uint)AbilityCount && (AbilityBits[index >> 3] & (1 << (index & 7))) != 0;
}
