// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

// (max level, level costs)
public partial class MultiLevelAbility : AdjustableAbility
{

    // AoWAb.PAS:248
    public MultiLevelAbility(ECustomNode owner) : base(owner)
    {
        LevelCosts = new Int32List();
        MaxLevel = 4;
    }

    public int MaxLevel { get; protected set; }
    public Int32List LevelCosts { get; }

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

    public override int GetLevel(AbilityOwner owner) => owner == null ? 0 : ((MultiLevelAbilityData)owner.GetAbilityData(AbilityId))?.Level ?? 0;
    public override int GetInherentLevel(AbilityOwner owner) => owner == null ? 0 : ((MultiLevelAbilityData)owner.GetAbilityData(AbilityId))?.Level ?? 0;
    public override string GetName(AbilityOwner owner) => GetLevelName(GetLevel(owner));
    public override string ExpandName(AbilityOwner owner) => GetLevelName(GetLevel(owner) + 1);

    public override int ExpandCost(AbilityOwner owner)
    {
        int level = GetInherentLevel(owner);
        return level >= MaxLevel ? 0 : LevelCosts.Get(level + 1);
    }

    public override bool CanExpand(AbilityOwner owner) => (owner.GetAbilitySelectionTypes() & SelectionTypes) != default(Set<AbilitySelectionType>) && GetInherentLevel(owner) < MaxLevel;

    public override int GetSkillPoints(AbilityOwner owner)
    {
        int level = GetInherentLevel(owner), sum = 0;
        // The original reads the current level cost on every iteration.
        for (int i = level; i >= 1; --i) sum = sum + LevelCosts.Get(level);
        return sum;
    }

    public override bool Expand(AbilityOwner owner)
    {
        var data = (MultiLevelAbilityData)owner.GetAbilityData(AbilityId);
        if (data == null)
        {
            data = (MultiLevelAbilityData)AbilityDataClass().CreateInstance(null, holder: CreationHolder());
            data.LevelAbilityId = (ushort)AbilityId;
            data.Level = 1;
            SetAbility(owner, AbilityId, 1);
            owner.AddAbilityData(data);
            return true;
        }
        if (data.Level == MaxLevel) return false;
        data.Level = (byte)(data.Level + 1);
        return true;
    }

    public override bool Remove(AbilityOwner owner)
    {
        var data = (MultiLevelAbilityData)owner.GetAbilityData(AbilityId);
        if (data == null) { SetAbility(owner, AbilityId, 0); return false; }
        if (data.Level == 1) { SetAbility(owner, AbilityId, 0); owner.RemoveAbilityData(data); }
        else data.Level = (byte)(data.Level - 1);
        return true;
    }
}
