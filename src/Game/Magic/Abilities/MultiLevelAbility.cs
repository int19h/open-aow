#nullable disable warnings
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

// (max level, level costs)
public class MultiLevelAbility(CustomNode? owner) : AdjustableAbility(owner)
{

    public int MaxLevel { get; protected init; } = 4;
    public Int32List LevelCosts { get; } = [];

    // AoWAb.PAS:255
    public override void Dispose()
    {
        LevelCosts?.Dispose();
        base.Dispose();
    }

    // AoWAb.PAS:322
    public virtual string GetLevelName(int level) => $"Unknown {level.ToString(CultureInfo.InvariantCulture)}";

    // AoWAb.PAS:317
    // { branch past the missing-data exit and nil-owner zero result }
    public override EngineClass AbilityDataClass() => AbilityRules.MultiLevelDataClass;

    public virtual MultiLevelAbilityData GetData(AbilityOwner? owner) => owner.GetAbilityData<MultiLevelAbilityData>(AbilityId);

    protected virtual MultiLevelAbilityData CreateData() => AbilityDataClass().Create<MultiLevelAbilityData>(null, holder: CreationHolder());

    public override int GetLevel(AbilityOwner? owner) => owner is null ? 0 : GetData(owner)?.Level ?? 0;
    public override int GetInherentLevel(AbilityOwner? owner) => owner is null ? 0 : GetData(owner)?.Level ?? 0;
    public override string GetName(AbilityOwner? owner) => GetLevelName(GetLevel(owner));
    public override string ExpandName(AbilityOwner? owner) => GetLevelName(GetLevel(owner) + 1);

    public override int ExpandCost(AbilityOwner? owner)
    {
        int level = GetInherentLevel(owner);
        return level >= MaxLevel ? 0 : LevelCosts.Get(level + 1);
    }

    public override bool CanExpand(AbilityOwner? owner) => (owner.GetAbilitySelectionTypes() & SelectionTypes) != default && GetInherentLevel(owner) < MaxLevel;

    public override int GetSkillPoints(AbilityOwner owner)
    {
        int level = GetInherentLevel(owner), sum = 0;
        // The original reads the current level cost on every iteration.
        for (int i = level; i >= 1; --i)
        {
            sum += LevelCosts.Get(level);
        }

        return sum;
    }

    public override bool Expand(AbilityOwner? owner)
    {
        MultiLevelAbilityData data = GetData(owner);
        if (data is null)
        {
            data = CreateData();
            data.LevelAbilityId = (ushort)AbilityId;
            data.Level = 1;
            SetAbility(owner, index: AbilityId, value: 1);
            owner.AddAbilityData(data);
            return true;
        }
        if (data.Level == MaxLevel)
        {
            return false;
        }

        data.Level = (byte)(data.Level + 1);
        return true;
    }

    public override bool Remove(AbilityOwner? owner)
    {
        MultiLevelAbilityData data = GetData(owner);
        if (data is null)
        {
            SetAbility(owner, index: AbilityId, value: 0);
            return false;
        }
        if (data.Level == 1)
        {
            SetAbility(owner, index: AbilityId, value: 0);
            owner.RemoveAbilityData(data);
        }
        else
        {
            data.Level = (byte)(data.Level - 1);
        }

        return true;
    }
}
