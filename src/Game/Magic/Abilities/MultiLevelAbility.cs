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
        FLevelCosts = new IntegerList();
        FMaxLevel = 4;
    }

    public int FMaxLevel;
    public IntegerList FLevelCosts;

    // AoWAb.PAS:255
    public override void Dispose()
    {
        FLevelCosts?.Dispose();
        base.Dispose();
    }

    // AoWAb.PAS:322
    public virtual string GetLevelName(int Level) => "Unknown " + IntToStr(Level);

    // AoWAb.PAS:317
    // { branch past the missing-data exit and nil-owner zero result }
    public override EClass AbilityDataClass() => AbilityRules.P4MultiLevelDataClass;

    public override int GetLevel(AbilityOwner owner) => owner == null ? 0 : ((MultiLevelAbilityData)owner.GetAbilityData(FAbilityID))?.FLevel ?? 0;
    public override int GetInherentLevel(AbilityOwner owner) => owner == null ? 0 : ((MultiLevelAbilityData)owner.GetAbilityData(FAbilityID))?.FLevel ?? 0;
    public override string GetName(AbilityOwner owner) => GetLevelName(GetLevel(owner));
    public override string ExpandName(AbilityOwner owner) => GetLevelName(GetLevel(owner) + 1);

    public override int ExpandCost(AbilityOwner owner)
    {
        int level = GetInherentLevel(owner);
        return level >= FMaxLevel ? 0 : FLevelCosts.Get(level + 1);
    }

    public override bool CanExpand(AbilityOwner owner) => (owner.GetAbilitySelectionTypes() & FSelectionTypes) != default(Set<AbilitySelectionType>) && GetInherentLevel(owner) < FMaxLevel;

    public override int GetSkillPoints(AbilityOwner owner)
    {
        int level = GetInherentLevel(owner), sum = 0;
        // The original reads the current level cost on every iteration.
        for (int i = level; i >= 1; --i) sum = sum + FLevelCosts.Get(level);
        return sum;
    }

    public override bool Expand(AbilityOwner owner)
    {
        var data = (MultiLevelAbilityData)owner.GetAbilityData(FAbilityID);
        if (data == null)
        {
            data = (MultiLevelAbilityData)AbilityDataClass().Create(null);
            data.FLevelAbilityID = (ushort)FAbilityID;
            data.FLevel = 1;
            SetAb(owner, FAbilityID, 1);
            owner.AddAbilityData(data);
            return true;
        }
        if (data.FLevel == FMaxLevel) return false;
        data.FLevel = (byte)(data.FLevel + 1);
        return true;
    }

    public override bool Remove(AbilityOwner owner)
    {
        var data = (MultiLevelAbilityData)owner.GetAbilityData(FAbilityID);
        if (data == null) { SetAb(owner, FAbilityID, 0); return false; }
        if (data.FLevel == 1) { SetAb(owner, FAbilityID, 0); owner.RemoveAbilityData(data); }
        else data.FLevel = (byte)(data.FLevel - 1);
        return true;
    }
}
