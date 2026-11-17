#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public partial class LeadershipAbility : MultiLevelAbility
{
    // AoWAb.PAS:965
    public LeadershipAbility(CustomNode owner) : base(owner)
    {
        MaxLevel = 1;
        AbilityId = 46;
        LevelCosts[1] = 10;
    }

    // AoWAb.PAS:1002
    public override sbyte GetAttack(AbilityOwner owner)
    {
        sbyte result;
        int level;
        level = GetLevel(owner);
        result = level != 0 ? LeadershipAttack[level] : (sbyte)0;

        return result;
    }

    // AoWAb.PAS:1013
    public override sbyte GetDefense(AbilityOwner owner)
    {
        sbyte result;
        int level;
        level = GetLevel(owner);
        result = level != 0 ? LeadershipDefense[level] : (sbyte)0;

        return result;
    }

    // AoWAb.PAS:978
    public override int GetLevel(AbilityOwner owner)
    {
        int result;
        LeadershipAbilityData data;
        if (owner != null)
        {
            data = GetData(owner);
            if (data != null)
            {
                result = data.ExternalLevel > data.Level ? data.ExternalLevel : data.Level;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWAb.PAS:1067
    public override string GetSourceName(AbilityOwner owner)
    {
        string result;
        LeadershipAbilityData data;
        data = GetData(owner);
        result = data != null && data.ExternalLevel > data.Level ? data.ExternalSource : owner.GetOwnerName();

        return result;
    }

    // AoWAb.PAS:1078
    public override bool GetInherent(AbilityOwner owner)
    {
        LeadershipAbilityData data;
        data = GetData(owner);
        return data != null && data.Level > 0;
    }

    // AoWAb.PAS:1086
    public override bool CanExpand(AbilityOwner owner)
    {
        bool result;
        LeadershipAbilityData data;
        data = GetData(owner);
        result = data == null || data.Level < MaxLevel;

        return result;
    }

    // AoWAb.PAS:1097
    public override string GetLevelName(int level) => AbilityRules.TranslateResourceString(GetEngine(), s_leadershipText);

    // AoWAb.PAS:1024
    public void SetExternalSource(AbilityOwner owner, byte level, string sourceName)
    {
        LeadershipAbilityData data;
        data = GetData(owner);
        if (data == null)
        {
            SetAbility(owner, AbilityId, 1);
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
            data.ExternalLevel = level;
        }

        if (owner is Troop)
        {
            owner.Changed();
        }
    }

    // AoWAb.PAS:1047
    public void ResetExternalSource(AbilityOwner owner)
    {
        LeadershipAbilityData data;
        data = GetData(owner);
        if (data != null)
        {
            if (data.Level != 0)
            {
                data.ExternalSource = "";
                data.ExternalLevel = 0;
            }
            else
            {
                SetAbility(owner, AbilityId, 0);
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

    public override LeadershipAbilityData GetData(AbilityOwner owner) => owner.GetAbilityData<LeadershipAbilityData>(AbilityId);

    private static ReadOnlySpan<sbyte> LeadershipAttack => [0, 1, 1, 2, 2];
    private static ReadOnlySpan<sbyte> LeadershipDefense => [2, 0, 1, 1, 2];

    private static readonly string s_leadershipText = "Leadership";

}
