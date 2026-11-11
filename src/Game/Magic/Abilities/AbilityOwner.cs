// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

// (FAbilityData,)
public partial class AbilityOwner : CustomAbilityList
{

    // Ability.PAS:1663
    public AbilityOwner(ECustomNode owner) : base(owner)
    {

    }

    public AbilityData FAbilityData;

    // Ability.PAS:1762
    public override void ReadWrite(EStorageStream Stream)
    {
        AbilityData Previous = default;
        AbilityData Data = default;
        Int32List IDs = default;
        base.ReadWrite(Stream);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            // The original tests property 3 before reading the data-ID list.
            if (Stream.PropertyTable.PropertyExist(3))
            {
                Previous = null;
                IDs = new Int32List();
                Stream.rwStreamProc(49, IDs.LoadFromStream, IDs.SaveToStream);
                for (int I = 0, last = IDs.Count - 1; I <= last; I++)
                {
                    var item = IDs[I];
                    Stream.rwCreateEObject(ref Data, null, item + 50);
                    if (Previous != null)
                    {
                        Previous.FNext = Data;
                    }

                    Previous = Data;
                    if (FAbilityData == null)
                    {
                        FAbilityData = Data;
                    }
                }

                IDs?.Dispose();
            }
        }
        else
        {
            if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Write)) == Set<StorageModeElement>.Of(StorageModeElement.Write))
            {
                if (FAbilityData != null)
                {
                    IDs = new Int32List();
                    Data = FAbilityData;
                    while (Data != null)
                    {
                        IDs.Add(Data.GetAbilityID());
                        Data = Data.FNext;
                    }

                    Stream.rwStreamProc(49, IDs.LoadFromStream, IDs.SaveToStream);
                    Data = FAbilityData;
                    while (Data != null)
                    {
                        var abilityID = Data.GetAbilityID();
                        Stream.rwCreateEObject(ref Data, null, abilityID + 50);
                        Data = Data.FNext;
                    }

                    IDs?.Dispose();
                }
            }
            else
            {
                if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Compare))
                {
                    Data = FAbilityData;
                    while (Data != null)
                    {
                        var abilityID = Data.GetAbilityID();
                        Stream.rwCreateEObject(ref Data, null, abilityID + 50);
                        Data = Data.FNext;
                    }
                }
            }
        }
    }

    // Ability.PAS:1667
    public override void Clear()
    {
        base.Clear();
        while (FAbilityData != null)
        {
            RemoveAbilityData(FAbilityData);
        }
    }

    // Ability.PAS:1998
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001985)
        {
            TriggerNewDay();
        }
        else
        {
            if (Msg.MsgID == 537001986)
            {
                TriggerNewTurn((sbyte)Msg.Payload.Int);
            }
        }
    }

    // Ability.PAS:1754
    public override void SetAbSet(int index, byte value)
    {
        base.SetAbSet(index, value);
    }

    // Ability.PAS:2004
    public override void ListAbilitiesEx(StringCollection Text, Set<AbilitySelectionType> Types)
    {
        Ability Ability;
        Text.Clear();
        for (int I = 0, last = FAbilityCount - 1; I <= last; I++)
        {
            if (GetAbSet(I))
            {
                Ability = AoWHSSet.Current.FAbilityControl.GetAbility(I);
                if ((Ability.FSelectionTypes & Types) != Set<AbilitySelectionType>.Of())
                {
                    Text.AddObject(Ability.GetName(this), IntToObject(I));
                }
            }
        }
    }

    // Ability.PAS:1758
    public virtual int Execute() => 1;

    // Ability.PAS:1676
    public virtual string GetOwnerName() => "";

    // Ability.PAS:1927
    public virtual string GetAbName(int AbilityID) => AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetName(this);

    // Ability.PAS:1931
    public virtual sbyte GetAbAttack(int AbilityID) => AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetAttack(this);

    // Ability.PAS:1935
    public virtual sbyte GetAbDefense(int AbilityID) => AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetDefense(this);

    // Ability.PAS:1939
    public virtual sbyte GetAbResistance(int AbilityID) => AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetResistance(this);

    // Ability.PAS:1943
    public virtual sbyte GetAbDamage(int AbilityID) => AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetDamage(this);

    // Ability.PAS:1947
    public virtual Set<MoveType> GetAbMoveTypes(int AbilityID) => AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetMoveTypes(this);

    // Ability.PAS:1951
    public virtual Set<DamageType> GetAbProtectionTypes(int AbilityID) => AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetProtectionTypes(this);

    // Ability.PAS:1955
    public virtual Set<DamageType> GetAbImmunityTypes(int AbilityID) => AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetImmunityTypes(this);

    // Ability.PAS:1959
    public virtual int GetAbLevel(int AbilityID) => AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetLevel(this);

    // Ability.PAS:1963
    public virtual bool GetAbEnabled(int AbilityID) => GetAbSet(AbilityID) && AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetEnabled(this);

    // Ability.PAS:1672
    public virtual Set<AbilitySelectionType> GetAbilitySelectionTypes() => Set<AbilitySelectionType>.Of();

    // Ability.PAS:1837
    public virtual void Changed()
    {
    }

    // Ability.PAS:1840
    public virtual bool ExpandAbility(int AbilityID)
    {
        bool Result = AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).Expand(this);
        if (Result)
        {
            Changed();
        }

        return Result;
    }

    // Ability.PAS:1845
    public virtual bool RemoveAbility(int AbilityID)
    {
        bool Result = AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).Remove(this);
        if (Result)
        {
            Changed();
        }

        return Result;
    }

    // Ability.PAS:1678
    public void ValidateOwnerTypeAbilities()
    {
        Set<AbilitySelectionType> Types;
        Types = GetAbilitySelectionTypes();
        for (int I = GetAbCount() - 1; I >= 0; I--)
        {
            if (GetAbSet(I) && (AoWHSSet.Current.FAbilityControl.GetAbility(I).FSelectionTypes & Types) == Set<AbilitySelectionType>.Of())
            {
                SetAbSet(I, 0);
            }
        }
    }

    // Ability.PAS:1688
    public AbilityData GetAbilityData(int AbilityID)
    {
        AbilityData Data;
        Data = FAbilityData;
        while (Data != null && Data.GetAbilityID() != AbilityID)
        {
            Data = Data.FNext;
        }

        return Data;
    }

    // Ability.PAS:1696
    public void AssignAb(AbilityOwner Source)
    {
        AbilityData Data;
        while (FAbilityData != null)
        {
            RemoveAbilityData(FAbilityData);
        }

        SetAbCount(Source.GetAbCount());
        for (int I = Source.GetAbCount() - 1; I >= 0; I--)
        {
            if (Source.GetAbSet(I) != GetAbSet(I))
            {
                SetAbSet(I, (byte)(Source.GetAbSet(I) ? 1 : 0));
            }
        }

        Data = Source.FAbilityData;
        while (Data != null)
        {
            AddAbilityData((AbilityData)Data.Copy(GameTables.AoWEngine));
            Data = Data.FNext;
        }
    }

    // Ability.PAS:1712
    public void AddAbilityData(AbilityData Data)
    {
        Data.FNext = FAbilityData;
        FAbilityData = Data;
    }

    // Ability.PAS:1717
    public void RemoveAbilityData(AbilityData Data)
    {
        AbilityData Current;
        AbilityData Previous;
        Current = FAbilityData;
        Previous = null;
        while (Current != null && Current != Data)
        {
            Previous = Current;
            Current = Current.FNext;
        }

        if (Current == Data)
        {
            // { Original dereferences nil when Data is nil. }
            if (Previous != null)
            {
                Previous.FNext = Current.FNext;
            }
            else
            {
                FAbilityData = Current.FNext;
            }

            Current?.Dispose();
        }
    }

    // Ability.PAS:1736
    public void RemoveAbilityDataID(int AbilityID)
    {
        AbilityData Current;
        AbilityData Previous;
        Current = FAbilityData;
        Previous = null;
        while (Current != null && Current.GetAbilityID() != AbilityID)
        {
            Previous = Current;
            Current = Current.FNext;
        }

        if (Current != null && Current.GetAbilityID() == AbilityID)
        {
            if (Previous != null)
            {
                Previous.FNext = Current.FNext;
            }
            else
            {
                FAbilityData = Current.FNext;
            }

            Current?.Dispose();
        }
    }

    // Ability.PAS:1818
    public void UpdateDefaultAbilities(AbilityOwner Source)
    {
        AbilityData Data;
        for (int I = Source.GetAbCount() - 1; I >= 0; I--)
        {
            if (Source.GetAbSet(I) && (!GetAbSet(I) || Source.GetAbLevel(I) > GetAbLevel(I)))
            {
                Data = Source.GetAbilityData(I);
                if (Data != null)
                {
                    RemoveAbilityDataID(I);
                    Data = (AbilityData)Data.Copy(GameTables.AoWEngine);
                    AddAbilityData(Data);
                }

                SetAbSet(I, 1);
            }
        }
    }

    // Ability.PAS:1850
    public sbyte GetAbAttackAll()
    {
        sbyte Result = 0;
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbEnabled(I))
            {
                Result = (sbyte)(Result + AoWHSSet.Current.FAbilityControl.GetAbility(I).GetAttack(this));
            }
        }

        return Result;
    }

    // Ability.PAS:1858
    public sbyte GetAbDefenseAll()
    {
        sbyte Result = 0;
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbEnabled(I))
            {
                Result = (sbyte)(Result + AoWHSSet.Current.FAbilityControl.GetAbility(I).GetDefense(this));
            }
        }

        return Result;
    }

    // Ability.PAS:1866
    public sbyte GetAbResistanceAll()
    {
        sbyte Result = 0;
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbEnabled(I))
            {
                Result = (sbyte)(Result + AoWHSSet.Current.FAbilityControl.GetAbility(I).GetResistance(this));
            }
        }

        return Result;
    }

    // Ability.PAS:1874
    public sbyte GetAbDamageAll()
    {
        sbyte Result = 0;
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbEnabled(I))
            {
                Result = (sbyte)(Result + AoWHSSet.Current.FAbilityControl.GetAbility(I).GetDamage(this));
            }
        }

        return Result;
    }

    // Ability.PAS:1882
    public Set<MoveType> GetAbMoveTypesAll()
    {
        Set<MoveType> Result = Set<MoveType>.Of();
        if (GetAbEnabled(0))
        {
            Result = Result + Set<MoveType>.Of(MoveType.Walking);
        }

        if (GetAbEnabled(1) || GetAbEnabled(165) || GetAbEnabled(59))
        {
            Result = Result + Set<MoveType>.Of(MoveType.Flying);
        }

        if (GetAbEnabled(2) || GetAbEnabled(23) || GetAbEnabled(164) || GetAbEnabled(166))
        {
            Result = Result + Set<MoveType>.Of(MoveType.Swimming);
        }

        if (GetAbEnabled(3) || GetAbEnabled(157))
        {
            Result = Result + Set<MoveType>.Of(MoveType.Forestry);
        }

        if (GetAbEnabled(4) || GetAbEnabled(157))
        {
            Result = Result + Set<MoveType>.Of(MoveType.CaveCrawling);
        }

        if (GetAbEnabled(5) || GetAbEnabled(157))
        {
            Result = Result + Set<MoveType>.Of(MoveType.Mountaineering);
        }

        if (GetAbEnabled(7) || GetAbEnabled(163))
        {
            // The original set constant is $40.
            Result = Result + Set<MoveType>.Of(MoveType.LavaCrossing);
        }

        if (GetAbEnabled(42))
        {
            // The original set constant is $80.
            Result = Result + Set<MoveType>.Of(MoveType.Tunneling);
        }

        return Result;
    }

    // Ability.PAS:1894
    public Set<WallCombatFeaturesElement> GetAbWallCombatFeatures()
    {
        Set<WallCombatFeaturesElement> Result = Set<WallCombatFeaturesElement>.Of();
        // { Original intersects into an empty set, so the result is always empty. }
        for (int I = GetAbCount() - 1; I >= 0; I--)
        {
            if (GetAbEnabled(I))
            {
                Result = Result & AoWHSSet.Current.FAbilityControl.GetAbility(I).GetWallCombatFeatures(this);
            }
        }

        return Result;
    }

    // Ability.PAS:1903
    public Set<DamageType> GetAbProtectionTypesAll()
    {
        Set<DamageType> Result = Set<DamageType>.Of();
        if (GetAbEnabled(77) || GetAbEnabled(166))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Physical);
        }

        if (GetAbEnabled(71) || GetAbEnabled(167))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Fire);
        }

        if (GetAbEnabled(75))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Magic);
        }

        if (GetAbEnabled(76))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Cold);
        }

        if (GetAbEnabled(74))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Lightning);
        }

        if (GetAbEnabled(73))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Poison);
        }

        if (GetAbEnabled(70) || GetAbEnabled(158))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Death);
        }

        if (GetAbEnabled(72))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Holy);
        }

        return Result;
    }

    // Ability.PAS:1915
    public Set<DamageType> GetAbImmunityTypesAll()
    {
        Set<DamageType> Result = Set<DamageType>.Of();
        if (GetAbEnabled(13))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Physical);
        }

        if (GetAbEnabled(7) || GetAbEnabled(163))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Fire);
        }

        if (GetAbEnabled(6))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Magic);
        }

        if (GetAbEnabled(8))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Cold);
        }

        if (GetAbEnabled(9))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Lightning);
        }

        if (GetAbEnabled(10))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Poison);
        }

        if (GetAbEnabled(11))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Death);
        }

        if (GetAbEnabled(12))
        {
            Result = Result + Set<DamageType>.Of(DamageType.Holy);
        }

        return Result;
    }

    // Ability.PAS:1967
    public void TriggerNewDay()
    {
        try
        {
            for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
            {
                if (GetAbSet(I))
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(I).NewDay(this);
                }
            }
        }
        catch
        {
        }
    }

    // Ability.PAS:1977
    public void TriggerNewTurn(sbyte Player)
    {
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbSet(I))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(I).NewTurn(this, Player);
            }
        }
    }

    // Ability.PAS:1984
    public void TriggerNewCombatTurn(CombatObject Combat, sbyte Player)
    {
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbSet(I))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(I).NewCombatTurn(Combat, Player);
            }
        }
    }

    // Ability.PAS:1991
    public void TriggerCombatDone(CombatObject Combat)
    {
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbSet(I))
            {
                AoWHSSet.Current.FAbilityControl.GetAbility(I).CombatDone(Combat);
            }
        }
    }

    // Ability.PAS:2018
    public void ListAbilities(StringCollection Text)
    {
        Ability Ability;
        string Name;
        Text.Clear();
        for (int I = 0, last = FAbilityCount - 1; I <= last; I++)
        {
            if (GetAbSet(I))
            {
                Ability = AoWHSSet.Current.FAbilityControl.GetAbility(I);
                Name = Ability.GetName(this);
                if (!IsEmpty(Name))
                {
                    Text.AddObject(Name, IntToObject(I));
                }
            }
        }
    }

    // intf_P1_06.inc:120

}
