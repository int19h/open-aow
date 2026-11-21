#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Combat;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.Magic.Abilities;

// (AbilityData,)
public class AbilityOwner(CustomNode? owner) : CustomAbilityList(owner)
{
    public AbilityData? AbilityData { get; set; }

    // Ability.PAS:1762
    // The load follows AbilityDataIds, as the original ReadWrite does: one AbilityData per stored id, from the slot of
    // that id, and a slot AbilityDataIds does not name stays unread. The original ReadWrite reads neither member when the
    // ability bits (property 3) are absent.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        if (persister.Loading)
        {
            if (!persister.Has("AbilityBits"))
            {
                persister.Skip("AbilityDataIds");
                persister.Skip("AbilityData");
                return;
            }

            Int32List ids = new();
            persister.Int32List(name: "AbilityDataIds", list: ids);
            AbilityData? previous = null;
            for (int i = 0; i < ids.Count; i++)
            {
                AbilityData? data = persister.CreatedSlot<AbilityData>("AbilityData", slot: ids[i], objectValue: null);
                previous?.Next = data;

                previous = data;
                AbilityData ??= data;
            }

            persister.Skip("AbilityData");
            ids.Dispose();
            return;
        }

        // Under the translate stream the original takes its read branch, and the stream's empty
        // property table holds no ability bits, so no ability data is visited.
        if (AbilityData is null || persister.Translating())
        {
            return;
        }

        Int32List stored = new();
        for (AbilityData? data = AbilityData; data is not null; data = data.Next)
        {
            _ = stored.Add(data.GetAbilityId());
        }

        persister.Int32List(name: "AbilityDataIds", list: stored);
        for (AbilityData? data = AbilityData; data is not null; data = data.Next)
        {
            _ = persister.CreatedSlot("AbilityData", slot: data.GetAbilityId(), objectValue: data);
        }

        stored.Dispose();
    }

    // Ability.PAS:1667
    public override void Clear()
    {
        base.Clear();
        while (AbilityData is not null)
        {
            RemoveAbilityData(AbilityData);
        }
    }

    // Ability.PAS:1998
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
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
    public override void SetAbilitySet(int index, byte value) => base.SetAbilitySet(index, value);

    // Ability.PAS:2004
    public override void ListAbilities(DelphiStringCollection text, AbilitySelectionTypes types)
    {
        text.Clear();
        for (int i = 0, last = AbilityCount - 1; i <= last; i++)
        {
            if (GetAbilitySetAt(i))
            {
                Ability? ability = GamePack.Current.AbilityControl[i];
                if ((ability.SelectionTypes & types) != AbilitySelectionTypes.None)
                {
                    _ = text.AddObject(ability.GetName(this), item: IntToObject(i));
                }
            }
        }
    }

    // Ability.PAS:1758
    public virtual int Execute() => 1;

    // Ability.PAS:1676
    public virtual string GetOwnerName() => "";

    // Ability.PAS:1927
    public virtual string GetOwnAbilityName(int abilityId) => GamePack.Current.AbilityControl[abilityId].GetName(this);

    // Ability.PAS:1931
    public virtual sbyte GetAbilityAttack(int abilityId) => GamePack.Current.AbilityControl[abilityId].GetAttack(this);

    // Ability.PAS:1935
    public virtual sbyte GetAbilityDefense(int abilityId) => GamePack.Current.AbilityControl[abilityId].GetDefense(this);

    // Ability.PAS:1939
    public virtual sbyte GetAbilityResistance(int abilityId) => GamePack.Current.AbilityControl[abilityId].GetResistance(this);

    // Ability.PAS:1943
    public virtual sbyte GetAbilityDamage(int abilityId) => GamePack.Current.AbilityControl[abilityId].GetDamage(this);

    // Ability.PAS:1947
    public virtual MoveTypes GetAbilityMoveTypes(int abilityId) => GamePack.Current.AbilityControl[abilityId].GetMoveTypes(this);

    // Ability.PAS:1951
    public virtual DamageTypes GetAbilityProtectionTypes(int abilityId) => GamePack.Current.AbilityControl[abilityId].GetProtectionTypes(this);

    // Ability.PAS:1955
    public virtual DamageTypes GetAbilityImmunityTypes(int abilityId) => GamePack.Current.AbilityControl[abilityId].GetImmunityTypes(this);

    // Ability.PAS:1959
    public virtual int GetOwnAbilityLevel(int abilityId) => GamePack.Current.AbilityControl[abilityId].GetLevel(this);

    // Ability.PAS:1963
    public virtual bool GetOwnAbilityEnabled(int abilityId) => GetAbilitySetAt(abilityId) && GamePack.Current.AbilityControl[abilityId].GetEnabled(this);

    // Ability.PAS:1672
    public virtual AbilitySelectionTypes GetAbilitySelectionTypes() => AbilitySelectionTypes.None;

    // Ability.PAS:1837
    public virtual void Changed()
    {
    }

    // Ability.PAS:1840
    public virtual bool ExpandAbility(int abilityId)
    {
        bool result = GamePack.Current.AbilityControl[abilityId].Expand(this);
        if (result)
        {
            Changed();
        }

        return result;
    }

    // Ability.PAS:1845
    public virtual bool RemoveAbility(int abilityId)
    {
        bool result = GamePack.Current.AbilityControl[abilityId].Remove(this);
        if (result)
        {
            Changed();
        }

        return result;
    }

    // Ability.PAS:1678
    public void ValidateOwnerTypeAbilities()
    {
        AbilitySelectionTypes types = GetAbilitySelectionTypes();
        for (int i = OwnAbilityCount - 1; i >= 0; i--)
        {
            if (GetAbilitySetAt(i) && (GamePack.Current.AbilityControl[i].SelectionTypes & types) == AbilitySelectionTypes.None)
            {
                SetAbilitySet(i, value: 0);
            }
        }
    }

    // Ability.PAS:1688
    public AbilityData? GetAbilityData(int abilityId)
    {
        AbilityData? data = AbilityData;
        while (data is not null && data.GetAbilityId() != abilityId)
        {
            data = data.Next;
        }

        return data;
    }

    // The data for one ability, typed by the caller. Each call site names the
    // data class its ability id stores, so the lookup keeps the single cast.
    public T GetAbilityData<T>(int abilityId) where T : AbilityData => (T)GetAbilityData(abilityId);

    // Ability.PAS:1696
    public void AssignAbility(AbilityOwner source)
    {
        while (AbilityData is not null)
        {
            RemoveAbilityData(AbilityData);
        }

        SetAbilityCount(source.OwnAbilityCount);
        for (int i = source.OwnAbilityCount - 1; i >= 0; i--)
        {
            if (source.GetAbilitySetAt(i) != GetAbilitySetAt(i))
            {
                SetAbilitySet(i, value: (byte)(source.GetAbilitySetAt(i) ? 1 : 0));
            }
        }

        AbilityData? data = source.AbilityData;
        while (data is not null)
        {
            AddAbilityData(data.Copy<AbilityData>(GetEngine()));
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
        AbilityData? current = AbilityData;
        AbilityData? previous = null;
        while (current is not null && current != data)
        {
            previous = current;
            current = current.Next;
        }

        if (current == data)
        {
            // { Original dereferences nil when Data is nil. }
            if (previous is not null)
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
        AbilityData? current = AbilityData;
        AbilityData? previous = null;
        while (current is not null && current.GetAbilityId() != abilityId)
        {
            previous = current;
            current = current.Next;
        }

        if (current is not null && current.GetAbilityId() == abilityId)
        {
            if (previous is not null)
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
        for (int i = source.OwnAbilityCount - 1; i >= 0; i--)
        {
            if (source.GetAbilitySetAt(i) && (!GetAbilitySetAt(i) || source.GetOwnAbilityLevel(i) > GetOwnAbilityLevel(i)))
            {
                AbilityData? data = source.GetAbilityData(i);
                if (data is not null)
                {
                    RemoveAbilityDataId(i);
                    data = data.Copy<AbilityData>(GetEngine());
                    AddAbilityData(data);
                }

                SetAbilitySet(i, value: 1);
            }
        }
    }

    // Ability.PAS:1850
    public sbyte GetAbilityAttackAll()
    {
        sbyte result = 0;
        for (int i = 0, last = OwnAbilityCount - 1; i <= last; i++)
        {
            if (GetOwnAbilityEnabled(i))
            {
                result = (sbyte)(result + GamePack.Current.AbilityControl[i].GetAttack(this));
            }
        }

        return result;
    }

    // Ability.PAS:1858
    public sbyte GetAbilityDefenseAll()
    {
        sbyte result = 0;
        for (int i = 0, last = OwnAbilityCount - 1; i <= last; i++)
        {
            if (GetOwnAbilityEnabled(i))
            {
                result = (sbyte)(result + GamePack.Current.AbilityControl[i].GetDefense(this));
            }
        }

        return result;
    }

    // Ability.PAS:1866
    public sbyte GetAbilityResistanceAll()
    {
        sbyte result = 0;
        for (int i = 0, last = OwnAbilityCount - 1; i <= last; i++)
        {
            if (GetOwnAbilityEnabled(i))
            {
                result = (sbyte)(result + GamePack.Current.AbilityControl[i].GetResistance(this));
            }
        }

        return result;
    }

    // Ability.PAS:1874
    public sbyte GetAbilityDamageAll()
    {
        sbyte result = 0;
        for (int i = 0, last = OwnAbilityCount - 1; i <= last; i++)
        {
            if (GetOwnAbilityEnabled(i))
            {
                result = (sbyte)(result + GamePack.Current.AbilityControl[i].GetDamage(this));
            }
        }

        return result;
    }

    // Ability.PAS:1882
    public MoveTypes GetAbilityMoveTypesAll()
    {
        MoveTypes result = MoveTypes.None;
        if (GetOwnAbilityEnabled(0))
        {
            result |= MoveTypes.Walking;
        }

        if (GetOwnAbilityEnabled(1) || GetOwnAbilityEnabled(165) || GetOwnAbilityEnabled(59))
        {
            result |= MoveTypes.Flying;
        }

        if (GetOwnAbilityEnabled(2) || GetOwnAbilityEnabled(23) || GetOwnAbilityEnabled(164) || GetOwnAbilityEnabled(166))
        {
            result |= MoveTypes.Swimming;
        }

        if (GetOwnAbilityEnabled(3) || GetOwnAbilityEnabled(157))
        {
            result |= MoveTypes.Forestry;
        }

        if (GetOwnAbilityEnabled(4) || GetOwnAbilityEnabled(157))
        {
            result |= MoveTypes.CaveCrawling;
        }

        if (GetOwnAbilityEnabled(5) || GetOwnAbilityEnabled(157))
        {
            result |= MoveTypes.Mountaineering;
        }

        if (GetOwnAbilityEnabled(7) || GetOwnAbilityEnabled(163))
        {
            // The original set constant is $40.
            result |= MoveTypes.LavaCrossing;
        }

        if (GetOwnAbilityEnabled(42))
        {
            // The original set constant is $80.
            result |= MoveTypes.Tunneling;
        }

        return result;
    }

    // Ability.PAS:1894
    public WallCombatFeatures GetAbilityWallCombatFeatures()
    {
        WallCombatFeatures result = WallCombatFeatures.None;
        // { Original intersects into an empty set, so the result is always empty. }
        for (int i = OwnAbilityCount - 1; i >= 0; i--)
        {
            if (GetOwnAbilityEnabled(i))
            {
                result &= GamePack.Current.AbilityControl[i].GetWallCombatFeatures(this);
            }
        }

        return result;
    }

    // Ability.PAS:1903
    public DamageTypes GetAbilityProtectionTypesAll()
    {
        DamageTypes result = DamageTypes.None;
        if (GetOwnAbilityEnabled(77) || GetOwnAbilityEnabled(166))
        {
            result |= DamageTypes.Physical;
        }

        if (GetOwnAbilityEnabled(71) || GetOwnAbilityEnabled(167))
        {
            result |= DamageTypes.Fire;
        }

        if (GetOwnAbilityEnabled(75))
        {
            result |= DamageTypes.Magic;
        }

        if (GetOwnAbilityEnabled(76))
        {
            result |= DamageTypes.Cold;
        }

        if (GetOwnAbilityEnabled(74))
        {
            result |= DamageTypes.Lightning;
        }

        if (GetOwnAbilityEnabled(73))
        {
            result |= DamageTypes.Poison;
        }

        if (GetOwnAbilityEnabled(70) || GetOwnAbilityEnabled(158))
        {
            result |= DamageTypes.Death;
        }

        if (GetOwnAbilityEnabled(72))
        {
            result |= DamageTypes.Holy;
        }

        return result;
    }

    // Ability.PAS:1915
    public DamageTypes GetAbilityImmunityTypesAll()
    {
        DamageTypes result = DamageTypes.None;
        if (GetOwnAbilityEnabled(13))
        {
            result |= DamageTypes.Physical;
        }

        if (GetOwnAbilityEnabled(7) || GetOwnAbilityEnabled(163))
        {
            result |= DamageTypes.Fire;
        }

        if (GetOwnAbilityEnabled(6))
        {
            result |= DamageTypes.Magic;
        }

        if (GetOwnAbilityEnabled(8))
        {
            result |= DamageTypes.Cold;
        }

        if (GetOwnAbilityEnabled(9))
        {
            result |= DamageTypes.Lightning;
        }

        if (GetOwnAbilityEnabled(10))
        {
            result |= DamageTypes.Poison;
        }

        if (GetOwnAbilityEnabled(11))
        {
            result |= DamageTypes.Death;
        }

        if (GetOwnAbilityEnabled(12))
        {
            result |= DamageTypes.Holy;
        }

        return result;
    }

    // Ability.PAS:1967
    public void TriggerNewDay()
    {
        try
        {
            for (int i = 0, last = OwnAbilityCount - 1; i <= last; i++)
            {
                if (GetAbilitySetAt(i))
                {
                    GamePack.Current.AbilityControl[i].NewDay(this);
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
        for (int i = 0, last = OwnAbilityCount - 1; i <= last; i++)
        {
            if (GetAbilitySetAt(i))
            {
                GamePack.Current.AbilityControl[i].NewTurn(this, player);
            }
        }
    }

    // Ability.PAS:1984
    public void TriggerNewCombatTurn(CombatObject combat, sbyte player)
    {
        for (int i = 0, last = OwnAbilityCount - 1; i <= last; i++)
        {
            if (GetAbilitySetAt(i))
            {
                GamePack.Current.AbilityControl[i].NewCombatTurn(combat, player);
            }
        }
    }

    // Ability.PAS:1991
    public void TriggerCombatDone(CombatObject combat)
    {
        for (int i = 0, last = OwnAbilityCount - 1; i <= last; i++)
        {
            if (GetAbilitySetAt(i))
            {
                GamePack.Current.AbilityControl[i].CombatDone(combat);
            }
        }
    }

    // Ability.PAS:2018
    public void ListAbilities(DelphiStringCollection text)
    {
        text.Clear();
        for (int i = 0, last = AbilityCount - 1; i <= last; i++)
        {
            if (GetAbilitySetAt(i))
            {
                Ability? ability = GamePack.Current.AbilityControl[i];
                string name = ability.GetName(this);
                if (!IsEmpty(name))
                {
                    _ = text.AddObject(name, item: IntToObject(i));
                }
            }
        }
    }
}
