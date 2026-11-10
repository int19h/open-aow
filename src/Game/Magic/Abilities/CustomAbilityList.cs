// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

// ability-bit buffer, bit count
public partial class CustomAbilityList : EObject
{

    public CustomAbilityList(ECustomNode owner) : base(owner)
    { }

    public byte[] FAbilityBits = [];
    public int FAbilityCount;

    // Ability.PAS:942
    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    // Ability.PAS:1161
    public override void ReadWrite(EStorageStream Stream)
    {
        int I = default;
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of())
        {
            if (Stream.StorageMode().Contains(StorageModeElement.Read))
            {
                Stream.rwInteger(ref I, 2);
                SetAbCount(I);
                Stream.rwData(FAbilityBits.AsSpan(0, (int)(((uint)(FAbilityCount + 7)) >> 3)), (int)(((uint)(FAbilityCount + 7)) >> 3), 3);
            }
            else
            {
                Stream.rwInteger(ref FAbilityCount, 2);
                Stream.rwData(FAbilityBits.AsSpan(0, (int)(((uint)(FAbilityCount + 7)) >> 3)), (int)(((uint)(FAbilityCount + 7)) >> 3), 3);
            }
        }

        if (!Stream.StorageMode().Contains(StorageModeElement.Preview))
        {
            I = 0;
            int last = FAbilityCount - 1;
            for (; I <= last; I += 1)
            {
                if (GetAbSet(I) && AoWHSSet.Current.FAbilityControl.GetAbility(I) == null)
                {
                    SetAbSet(I, 0);
                }
            }
        }
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        int count = FAbilityCount;
        persister.Int("AbCount", ref count);
        if (persister.Loading) SetAbCount(count);
        int length = (int)(((uint)(FAbilityCount + 7)) >> 3);
        persister.BitArray("AbilityCount", FAbilityCount, FAbilityBits.AsSpan(0, length));
        for (int index = 0; index < FAbilityCount; index++)
        {
            if (GetAbSet(index) && AoWHSSet.Current.FAbilityControl.GetAbility(index) == null)
                SetAbSet(index, 0);
        }
    }

    // Ability.PAS:948
    public override void Clear()
    {
        base.Clear();
        SetAbCount(0);
    }

    // Ability.PAS:1130
    public virtual int GetAbCount() => FAbilityCount;

    // Ability.PAS:1216
    public virtual void ListAbilitiesEx(StringCollection Text, Set<AbilitySelectionType> Types)
    {
        Ability Ability;
        Text.Clear();
        for (int I = 0, last = FAbilityCount - 1; I <= last; I++)
        {
            if (GetAbSet(I))
            {
                Ability = AoWHSSet.Current.FAbilityControl.GetAbility(I);
                if ((Set<byte>.FromMask(Ability.FSelectionTypes.Mask) & Set<byte>.FromMask(Types.Mask)) != Set<byte>.WithBounds(0, 15))
                {
                    Text.AddObject(Ability.GetName(null), IntToObject(I));
                }
            }
        }
    }

    // Ability.PAS:994
    public void SetAbCount(int Value)
    {
        int OldSize;
        int NewSize;
        // { Original assertion accepts every Integer: its bounds predicate uses OR. }
        Assert(!(Value < 0 && Value >= 134217727), "Invalid AbCount Value (TArrayList)", "Ability.PAS", 1119);
        OldSize = (int)(((uint)(FAbilityCount + 7)) >> 3);
        NewSize = (int)(((uint)(Value + 7)) >> 3);
        if (NewSize != OldSize)
        {
            Array.Resize(ref FAbilityBits, NewSize);
            if (NewSize > OldSize)
            {
                FillBytes(FAbilityBits.AsSpan(OldSize, NewSize - OldSize), 0);
            }
        }

        FAbilityCount = Value;
    }

    // Ability.PAS:1134
    public void AssignAb(CustomAbilityList Source)
    {
        int Count;
        if (Source.GetAbCount() > GetAbCount())
        {
            Count = Source.GetAbCount();
        }
        else
        {
            Count = GetAbCount();
        }

        for (int I = 0; I <= Count - 1; I++)
        {
            if (Source.GetAbSet(I) != GetAbSet(I))
            {
                SetAbSet(I, (byte)(Source.GetAbSet(I) ? 1 : 0));
            }
        }
    }

    // Ability.PAS:1144
    public bool CompareAb(CustomAbilityList Source)
    {
        bool Result = Source.GetAbCount() == FAbilityCount;
        if (Result)
        {
            var abCount = GetAbCount();
            Result = CompareMem(FAbilityBits.AsSpan(0, (int)(((uint)(abCount + 7)) >> 3)), Source.FAbilityBits.AsSpan(0, (int)(((uint)(abCount + 7)) >> 3)), (int)(((uint)(abCount + 7)) >> 3));
        }

        return Result;
    }

    // Ability.PAS:1149
    public bool ComposeAb(CustomAbilityList Source)
    {
        bool Result = false;
        for (int I = 0, last = Source.GetAbCount() - 1; I <= last; I++)
        {
            if (Source.GetAbSet(I) && !GetAbSet(I))
            {
                SetAbSet(I, 1);
                Result = true;
            }
        }

        return Result;
    }

    // Ability.PAS:1184
    public string GetItemString()
    {
        string Result = "";
        for (int I = 0, last = FAbilityCount - 1; I <= last; I++)
        {
            if (GetAbSet(I))
            {
                Result = Result + IntToStr(I) + ",";
            }
        }

        if (!IsEmpty(Result))
        {
            Result = RtlUnit.Copy(Result, 1, Result.Length - 1);
        }

        return Result;
    }

    // Ability.PAS:1193
    public void SetItemString(string S)
    {
        string Part;
        int P;
        Clear();
        P = Pos(",", S);
        while (!IsEmpty(S))
        {
            if (P != 0)
            {
                Part = RtlUnit.Copy(S, 1, P - 1);
                S = Delete(S, 1, P);
            }
            else
            {
                Part = S;
                S = "";
            }

            if (!IsEmpty(Part))
            {
                SetAbSet(StrToInt(Part), 1);
            }

            P = Pos(",", S);
        }
    }

    public virtual void SetAbSet(int index, byte value)
    {
        if ((uint)index >= (uint)FAbilityCount) SetAbCount(index + 1);
        var bits = FAbilityBits;
        byte mask = (byte)(1 << (index & 7));
        if (value != 0) bits[index >> 3] |= mask;
        else bits[index >> 3] &= (byte)~mask;
    }

    public virtual bool GetAbSet(int index) => (uint)index < (uint)FAbilityCount && (FAbilityBits[index >> 3] & (1 << (index & 7))) != 0;
}
