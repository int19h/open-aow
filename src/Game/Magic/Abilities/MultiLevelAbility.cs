#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.Magic.Abilities;

// (max level, level costs)
public partial class MultiLevelAbility(CustomNode owner) : AdjustableAbility(owner)
{

    public int MaxLevel { get; protected set; } = 4;
    public Int32List LevelCosts { get; } = new Int32List();

    // AoWAb.PAS:255
    public override void Dispose()
    {
        LevelCosts?.Dispose();
        base.Dispose();
    }

    // AoWAb.PAS:322
    public virtual string GetLevelName(int level) => "Unknown " + IntToStr(level);

    // AoWAb.PAS:317
    // { branch past the missing-data exit and nil-owner zero result }
    public override EngineClass AbilityDataClass() => AbilityRules.MultiLevelDataClass;

    public virtual MultiLevelAbilityData GetData(AbilityOwner owner) => owner.GetAbilityData<MultiLevelAbilityData>(AbilityId);

    protected virtual MultiLevelAbilityData CreateData() => AbilityDataClass().Create<MultiLevelAbilityData>(null, holder: CreationHolder());

    public override int GetLevel(AbilityOwner owner) => owner == null ? 0 : GetData(owner)?.Level ?? 0;
    public override int GetInherentLevel(AbilityOwner owner) => owner == null ? 0 : GetData(owner)?.Level ?? 0;
    public override string GetName(AbilityOwner owner) => GetLevelName(GetLevel(owner));
    public override string ExpandName(AbilityOwner owner) => GetLevelName(GetLevel(owner) + 1);

    public override int ExpandCost(AbilityOwner owner)
    {
        int level = GetInherentLevel(owner);
        return level >= MaxLevel ? 0 : LevelCosts.Get(level + 1);
    }

    public override bool CanExpand(AbilityOwner owner) => (owner.GetAbilitySelectionTypes() & SelectionTypes) != default && GetInherentLevel(owner) < MaxLevel;

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

    public override bool Expand(AbilityOwner owner)
    {
        MultiLevelAbilityData data = GetData(owner);
        if (data == null)
        {
            data = CreateData();
            data.LevelAbilityId = (ushort)AbilityId;
            data.Level = 1;
            SetAbility(owner, AbilityId, 1);
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

    public override bool Remove(AbilityOwner owner)
    {
        MultiLevelAbilityData data = GetData(owner);
        if (data == null) { SetAbility(owner, AbilityId, 0); return false; }
        if (data.Level == 1) { SetAbility(owner, AbilityId, 0); owner.RemoveAbilityData(data); }
        else
        {
            data.Level = (byte)(data.Level - 1);
        }

        return true;
    }
}
