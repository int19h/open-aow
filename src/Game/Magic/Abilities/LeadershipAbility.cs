#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public class LeadershipAbility : MultiLevelAbility
{
    // AoWAb.PAS:965
    public LeadershipAbility(CustomNode? owner) : base(owner)
    {
        MaxLevel = 1;
        AbilityId = 46;
        LevelCosts[1] = 10;
    }

    // AoWAb.PAS:1002
    public override int GetAttack(AbilityOwner? owner)
    {
        int level = GetLevel(owner);
        return level != 0 ? LeadershipAttack[level] : 0;
    }

    // AoWAb.PAS:1013
    public override int GetDefense(AbilityOwner? owner)
    {
        int level = GetLevel(owner);
        return level != 0 ? LeadershipDefense[level] : 0;
    }

    // AoWAb.PAS:978
    public override int GetLevel(AbilityOwner? owner)
    {
        if (owner is not null)
        {
            LeadershipAbilityData data = GetData(owner);
            return data is not null ? data.ExternalLevel > data.Level ? data.ExternalLevel : data.Level : 0;
        }
        else
        {
            return 0;
        }
    }

    // AoWAb.PAS:1067
    public override string GetSourceName(AbilityOwner? owner)
    {
        LeadershipAbilityData data = GetData(owner);
        return data is not null && data.ExternalLevel > data.Level ? data.ExternalSource : owner.GetOwnerName();
    }

    // AoWAb.PAS:1078
    public override bool GetInherent(AbilityOwner owner)
    {
        LeadershipAbilityData data = GetData(owner);
        return data is not null && data.Level > 0;
    }

    // AoWAb.PAS:1086
    public override bool CanExpand(AbilityOwner? owner)
    {
        LeadershipAbilityData data = GetData(owner);
        return data is null || data.Level < MaxLevel;
    }

    // AoWAb.PAS:1097
    public override string GetLevelName(int level) => AbilityRules.TranslateResourceString(GetEngine(), S: s_leadershipText);

    // AoWAb.PAS:1024
    public void SetExternalSource(AbilityOwner owner, int level, string sourceName)
    {
        LeadershipAbilityData data = GetData(owner);
        if (data is null)
        {
            SetAbility(owner, index: AbilityId, value: 1);
            data = new LeadershipAbilityData(null)
            {
                Holder = CreationHolder(),
                LevelAbilityId = (ushort)AbilityId,
                Level = 0
            };
            owner.AddAbilityData(data);
        }

        // { Original compares with the inherent level, even if an external level is higher. }
        if (data.Level < level)
        {
            data.ExternalSource = sourceName;
            data.ExternalLevel = (byte)level;
        }

        if (owner is Troop)
        {
            owner.Changed();
        }
    }

    // AoWAb.PAS:1047
    public void ResetExternalSource(AbilityOwner owner)
    {
        LeadershipAbilityData data = GetData(owner);
        if (data is not null)
        {
            if (data.Level != 0)
            {
                data.ExternalSource = "";
                data.ExternalLevel = 0;
            }
            else
            {
                SetAbility(owner, index: AbilityId, value: 0);
                owner.RemoveAbilityData(data);
            }
        }

        if (owner is Troop)
        {
            owner.Changed();
        }
    }

    // AoWAb.PAS:973
    public override EngineClass AbilityDataClass() => EngineClass.Of<LeadershipAbilityData>();

    public override LeadershipAbilityData GetData(AbilityOwner? owner) => owner.GetAbilityData<LeadershipAbilityData>(AbilityId);

    private static ReadOnlySpan<sbyte> LeadershipAttack => [0, 1, 1, 2, 2];
    private static ReadOnlySpan<sbyte> LeadershipDefense => [2, 0, 1, 1, 2];

    private static readonly string s_leadershipText = "Leadership";

}
