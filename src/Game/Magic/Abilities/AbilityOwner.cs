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

// (AbilityData,)
public partial class AbilityOwner : CustomAbilityList
{

    // Ability.PAS:1663
    public AbilityOwner(ECustomNode owner) : base(owner)
    {

    }

    public AbilityData AbilityData { get; set; }

    // Ability.PAS:1762
    public override void ReadWrite(StorageStream stream)
    {
        AbilityData previous = default;
        AbilityData data = default;
        Int32List ids = default;
        base.ReadWrite(stream);
        if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            // The original tests property 3 before reading the data-ID list.
            if (stream.PropertyTable.PropertyExist(3))
            {
                previous = null;
                ids = new Int32List();
                stream.Payload(49, ids.LoadFromStream, ids.SaveToStream);
                for (int I = 0, last = ids.Count - 1; I <= last; I++)
                {
                    var item = ids[I];
                    stream.Created(ref data, null, item + 50);
                    if (previous != null)
                    {
                        previous.Next = data;
                    }

                    previous = data;
                    if (AbilityData == null)
                    {
                        AbilityData = data;
                    }
                }

                ids?.Dispose();
            }
        }
        else
        {
            if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Write)) == Set<StorageModeElement>.Of(StorageModeElement.Write))
            {
                if (AbilityData != null)
                {
                    ids = new Int32List();
                    data = AbilityData;
                    while (data != null)
                    {
                        ids.Add(data.GetAbilityId());
                        data = data.Next;
                    }

                    stream.Payload(49, ids.LoadFromStream, ids.SaveToStream);
                    data = AbilityData;
                    while (data != null)
                    {
                        var abilityId = data.GetAbilityId();
                        stream.Created(ref data, null, abilityId + 50);
                        data = data.Next;
                    }

                    ids?.Dispose();
                }
            }
            else
            {
                if ((stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Compare))
                {
                    data = AbilityData;
                    while (data != null)
                    {
                        var abilityId = data.GetAbilityId();
                        stream.Created(ref data, null, abilityId + 50);
                        data = data.Next;
                    }
                }
            }
        }
    }

    // Ability.PAS:1667
    public override void Clear()
    {
        base.Clear();
        while (AbilityData != null)
        {
            RemoveAbilityData(AbilityData);
        }
    }

    // Ability.PAS:1998
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001985)
        {
            TriggerNewDay();
        }
        else
        {
            if (message.MessageId == 537001986)
            {
                TriggerNewTurn((sbyte)message.Payload.Int);
            }
        }
    }

    // Ability.PAS:1754
    public override void SetAbilitySet(int index, byte value)
    {
        base.SetAbilitySet(index, value);
    }

    // Ability.PAS:2004
    public override void ListAbilities(StringCollection text, Set<AbilitySelectionType> types)
    {
        Ability ability;
        text.Clear();
        for (int I = 0, last = AbilityCount - 1; I <= last; I++)
        {
            if (GetAbilitySetAt(I))
            {
                ability = GamePack.Current.AbilityControl.GetAbility(I);
                if ((ability.SelectionTypes & types) != Set<AbilitySelectionType>.Of())
                {
                    text.AddObject(ability.GetName(this), IntToObject(I));
                }
            }
        }
    }

    // Ability.PAS:1758
    public virtual int Execute() => 1;

    // Ability.PAS:1676
    public virtual string GetOwnerName() => "";

    // Ability.PAS:1927
    public virtual string GetAbName(int abilityId) => GamePack.Current.AbilityControl.GetAbility(abilityId).GetName(this);

    // Ability.PAS:1931
    public virtual sbyte GetAbilityAttack(int abilityId) => GamePack.Current.AbilityControl.GetAbility(abilityId).GetAttack(this);

    // Ability.PAS:1935
    public virtual sbyte GetAbilityDefense(int abilityId) => GamePack.Current.AbilityControl.GetAbility(abilityId).GetDefense(this);

    // Ability.PAS:1939
    public virtual sbyte GetAbilityResistance(int abilityId) => GamePack.Current.AbilityControl.GetAbility(abilityId).GetResistance(this);

    // Ability.PAS:1943
    public virtual sbyte GetAbilityDamage(int abilityId) => GamePack.Current.AbilityControl.GetAbility(abilityId).GetDamage(this);

    // Ability.PAS:1947
    public virtual Set<MoveType> GetAbilityMoveTypes(int abilityId) => GamePack.Current.AbilityControl.GetAbility(abilityId).GetMoveTypes(this);

    // Ability.PAS:1951
    public virtual Set<DamageType> GetAbilityProtectionTypes(int abilityId) => GamePack.Current.AbilityControl.GetAbility(abilityId).GetProtectionTypes(this);

    // Ability.PAS:1955
    public virtual Set<DamageType> GetAbilityImmunityTypes(int abilityId) => GamePack.Current.AbilityControl.GetAbility(abilityId).GetImmunityTypes(this);

    // Ability.PAS:1959
    public virtual int GetAbLevel(int abilityId) => GamePack.Current.AbilityControl.GetAbility(abilityId).GetLevel(this);

    // Ability.PAS:1963
    public virtual bool GetAbEnabled(int abilityId) => GetAbilitySetAt(abilityId) && GamePack.Current.AbilityControl.GetAbility(abilityId).GetEnabled(this);

    // Ability.PAS:1672
    public virtual Set<AbilitySelectionType> GetAbilitySelectionTypes() => Set<AbilitySelectionType>.Of();

    // Ability.PAS:1837
    public virtual void Changed()
    {
    }

    // Ability.PAS:1840
    public virtual bool ExpandAbility(int abilityId)
    {
        bool result = GamePack.Current.AbilityControl.GetAbility(abilityId).Expand(this);
        if (result)
        {
            Changed();
        }

        return result;
    }

    // Ability.PAS:1845
    public virtual bool RemoveAbility(int abilityId)
    {
        bool result = GamePack.Current.AbilityControl.GetAbility(abilityId).Remove(this);
        if (result)
        {
            Changed();
        }

        return result;
    }

    // Ability.PAS:1678
    public void ValidateOwnerTypeAbilities()
    {
        Set<AbilitySelectionType> types;
        types = GetAbilitySelectionTypes();
        for (int I = GetAbCount() - 1; I >= 0; I--)
        {
            if (GetAbilitySetAt(I) && (GamePack.Current.AbilityControl.GetAbility(I).SelectionTypes & types) == Set<AbilitySelectionType>.Of())
            {
                SetAbilitySet(I, 0);
            }
        }
    }

    // Ability.PAS:1688
    public AbilityData GetAbilityData(int abilityId)
    {
        AbilityData data;
        data = AbilityData;
        while (data != null && data.GetAbilityId() != abilityId)
        {
            data = data.Next;
        }

        return data;
    }

    // Ability.PAS:1696
    public void AssignAbility(AbilityOwner source)
    {
        AbilityData data;
        while (AbilityData != null)
        {
            RemoveAbilityData(AbilityData);
        }

        SetAbilityCount(source.GetAbCount());
        for (int I = source.GetAbCount() - 1; I >= 0; I--)
        {
            if (source.GetAbilitySetAt(I) != GetAbilitySetAt(I))
            {
                SetAbilitySet(I, (byte)(source.GetAbilitySetAt(I) ? 1 : 0));
            }
        }

        data = source.AbilityData;
        while (data != null)
        {
            AddAbilityData((AbilityData)data.Copy(GameTables.GameEngine));
            data = data.Next;
        }
    }

    // Ability.PAS:1712
    public void AddAbilityData(AbilityData data)
    {
        data.Next = AbilityData;
        AbilityData = data;
    }

    // Ability.PAS:1717
    public void RemoveAbilityData(AbilityData data)
    {
        AbilityData current;
        AbilityData previous;
        current = AbilityData;
        previous = null;
        while (current != null && current != data)
        {
            previous = current;
            current = current.Next;
        }

        if (current == data)
        {
            // { Original dereferences nil when Data is nil. }
            if (previous != null)
            {
                previous.Next = current.Next;
            }
            else
            {
                AbilityData = current.Next;
            }

            current?.Dispose();
        }
    }

    // Ability.PAS:1736
    public void RemoveAbilityDataId(int abilityId)
    {
        AbilityData current;
        AbilityData previous;
        current = AbilityData;
        previous = null;
        while (current != null && current.GetAbilityId() != abilityId)
        {
            previous = current;
            current = current.Next;
        }

        if (current != null && current.GetAbilityId() == abilityId)
        {
            if (previous != null)
            {
                previous.Next = current.Next;
            }
            else
            {
                AbilityData = current.Next;
            }

            current?.Dispose();
        }
    }

    // Ability.PAS:1818
    public void UpdateDefaultAbilities(AbilityOwner source)
    {
        AbilityData data;
        for (int I = source.GetAbCount() - 1; I >= 0; I--)
        {
            if (source.GetAbilitySetAt(I) && (!GetAbilitySetAt(I) || source.GetAbLevel(I) > GetAbLevel(I)))
            {
                data = source.GetAbilityData(I);
                if (data != null)
                {
                    RemoveAbilityDataId(I);
                    data = (AbilityData)data.Copy(GameTables.GameEngine);
                    AddAbilityData(data);
                }

                SetAbilitySet(I, 1);
            }
        }
    }

    // Ability.PAS:1850
    public sbyte GetAbilityAttackAll()
    {
        sbyte result = 0;
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbEnabled(I))
            {
                result = (sbyte)(result + GamePack.Current.AbilityControl.GetAbility(I).GetAttack(this));
            }
        }

        return result;
    }

    // Ability.PAS:1858
    public sbyte GetAbilityDefenseAll()
    {
        sbyte result = 0;
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbEnabled(I))
            {
                result = (sbyte)(result + GamePack.Current.AbilityControl.GetAbility(I).GetDefense(this));
            }
        }

        return result;
    }

    // Ability.PAS:1866
    public sbyte GetAbilityResistanceAll()
    {
        sbyte result = 0;
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbEnabled(I))
            {
                result = (sbyte)(result + GamePack.Current.AbilityControl.GetAbility(I).GetResistance(this));
            }
        }

        return result;
    }

    // Ability.PAS:1874
    public sbyte GetAbilityDamageAll()
    {
        sbyte result = 0;
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbEnabled(I))
            {
                result = (sbyte)(result + GamePack.Current.AbilityControl.GetAbility(I).GetDamage(this));
            }
        }

        return result;
    }

    // Ability.PAS:1882
    public Set<MoveType> GetAbilityMoveTypesAll()
    {
        Set<MoveType> result = Set<MoveType>.Of();
        if (GetAbEnabled(0))
        {
            result = result + Set<MoveType>.Of(MoveType.Walking);
        }

        if (GetAbEnabled(1) || GetAbEnabled(165) || GetAbEnabled(59))
        {
            result = result + Set<MoveType>.Of(MoveType.Flying);
        }

        if (GetAbEnabled(2) || GetAbEnabled(23) || GetAbEnabled(164) || GetAbEnabled(166))
        {
            result = result + Set<MoveType>.Of(MoveType.Swimming);
        }

        if (GetAbEnabled(3) || GetAbEnabled(157))
        {
            result = result + Set<MoveType>.Of(MoveType.Forestry);
        }

        if (GetAbEnabled(4) || GetAbEnabled(157))
        {
            result = result + Set<MoveType>.Of(MoveType.CaveCrawling);
        }

        if (GetAbEnabled(5) || GetAbEnabled(157))
        {
            result = result + Set<MoveType>.Of(MoveType.Mountaineering);
        }

        if (GetAbEnabled(7) || GetAbEnabled(163))
        {
            // The original set constant is $40.
            result = result + Set<MoveType>.Of(MoveType.LavaCrossing);
        }

        if (GetAbEnabled(42))
        {
            // The original set constant is $80.
            result = result + Set<MoveType>.Of(MoveType.Tunneling);
        }

        return result;
    }

    // Ability.PAS:1894
    public Set<WallCombatFeaturesElement> GetAbilityWallCombatFeatures()
    {
        Set<WallCombatFeaturesElement> result = Set<WallCombatFeaturesElement>.Of();
        // { Original intersects into an empty set, so the result is always empty. }
        for (int I = GetAbCount() - 1; I >= 0; I--)
        {
            if (GetAbEnabled(I))
            {
                result = result & GamePack.Current.AbilityControl.GetAbility(I).GetWallCombatFeatures(this);
            }
        }

        return result;
    }

    // Ability.PAS:1903
    public Set<DamageType> GetAbilityProtectionTypesAll()
    {
        Set<DamageType> result = Set<DamageType>.Of();
        if (GetAbEnabled(77) || GetAbEnabled(166))
        {
            result = result + Set<DamageType>.Of(DamageType.Physical);
        }

        if (GetAbEnabled(71) || GetAbEnabled(167))
        {
            result = result + Set<DamageType>.Of(DamageType.Fire);
        }

        if (GetAbEnabled(75))
        {
            result = result + Set<DamageType>.Of(DamageType.Magic);
        }

        if (GetAbEnabled(76))
        {
            result = result + Set<DamageType>.Of(DamageType.Cold);
        }

        if (GetAbEnabled(74))
        {
            result = result + Set<DamageType>.Of(DamageType.Lightning);
        }

        if (GetAbEnabled(73))
        {
            result = result + Set<DamageType>.Of(DamageType.Poison);
        }

        if (GetAbEnabled(70) || GetAbEnabled(158))
        {
            result = result + Set<DamageType>.Of(DamageType.Death);
        }

        if (GetAbEnabled(72))
        {
            result = result + Set<DamageType>.Of(DamageType.Holy);
        }

        return result;
    }

    // Ability.PAS:1915
    public Set<DamageType> GetAbilityImmunityTypesAll()
    {
        Set<DamageType> result = Set<DamageType>.Of();
        if (GetAbEnabled(13))
        {
            result = result + Set<DamageType>.Of(DamageType.Physical);
        }

        if (GetAbEnabled(7) || GetAbEnabled(163))
        {
            result = result + Set<DamageType>.Of(DamageType.Fire);
        }

        if (GetAbEnabled(6))
        {
            result = result + Set<DamageType>.Of(DamageType.Magic);
        }

        if (GetAbEnabled(8))
        {
            result = result + Set<DamageType>.Of(DamageType.Cold);
        }

        if (GetAbEnabled(9))
        {
            result = result + Set<DamageType>.Of(DamageType.Lightning);
        }

        if (GetAbEnabled(10))
        {
            result = result + Set<DamageType>.Of(DamageType.Poison);
        }

        if (GetAbEnabled(11))
        {
            result = result + Set<DamageType>.Of(DamageType.Death);
        }

        if (GetAbEnabled(12))
        {
            result = result + Set<DamageType>.Of(DamageType.Holy);
        }

        return result;
    }

    // Ability.PAS:1967
    public void TriggerNewDay()
    {
        try
        {
            for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
            {
                if (GetAbilitySetAt(I))
                {
                    GamePack.Current.AbilityControl.GetAbility(I).NewDay(this);
                }
            }
        }
        catch
        {
        }
    }

    // Ability.PAS:1977
    public void TriggerNewTurn(sbyte player)
    {
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbilitySetAt(I))
            {
                GamePack.Current.AbilityControl.GetAbility(I).NewTurn(this, player);
            }
        }
    }

    // Ability.PAS:1984
    public void TriggerNewCombatTurn(CombatObject combat, sbyte player)
    {
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbilitySetAt(I))
            {
                GamePack.Current.AbilityControl.GetAbility(I).NewCombatTurn(combat, player);
            }
        }
    }

    // Ability.PAS:1991
    public void TriggerCombatDone(CombatObject combat)
    {
        for (int I = 0, last = GetAbCount() - 1; I <= last; I++)
        {
            if (GetAbilitySetAt(I))
            {
                GamePack.Current.AbilityControl.GetAbility(I).CombatDone(combat);
            }
        }
    }

    // Ability.PAS:2018
    public void ListAbilities(StringCollection text)
    {
        Ability ability;
        string name;
        text.Clear();
        for (int I = 0, last = AbilityCount - 1; I <= last; I++)
        {
            if (GetAbilitySetAt(I))
            {
                ability = GamePack.Current.AbilityControl.GetAbility(I);
                name = ability.GetName(this);
                if (!IsEmpty(name))
                {
                    text.AddObject(name, IntToObject(I));
                }
            }
        }
    }

    // intf_P1_06.inc:120

}
