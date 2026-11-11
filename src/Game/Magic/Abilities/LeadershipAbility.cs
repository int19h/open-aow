// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities;

public partial class LeadershipAbility : MultiLevelAbility
{
    // AoWAb.PAS:965
    public LeadershipAbility(ECustomNode owner) : base(owner)
    {
        FMaxLevel = 1;
        FAbilityID = 46;
        FLevelCosts[1] = 10;
    }

    // AoWAb.PAS:1002
    public override sbyte GetAttack(AbilityOwner Owner)
    {
        sbyte Result;
        int Level;
        Level = GetLevel(Owner);
        if (Level != 0)
        {
            Result = P4LeadershipAttack[Level];
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:1013
    public override sbyte GetDefense(AbilityOwner Owner)
    {
        sbyte Result;
        int Level;
        Level = GetLevel(Owner);
        if (Level != 0)
        {
            Result = P4LeadershipDefense[Level];
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:978
    public override int GetLevel(AbilityOwner owner)
    {
        int Result;
        LeadershipAbilityData Data;
        if (owner != null)
        {
            Data = (LeadershipAbilityData)owner.GetAbilityData(FAbilityID);
            if (Data != null)
            {
                if (Data.FExternalLevel > Data.FLevel)
                {
                    Result = Data.FExternalLevel;
                }
                else
                {
                    Result = Data.FLevel;
                }
            }
            else
            {
                return 0;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:1067
    public override string GetSourceName(AbilityOwner Owner)
    {
        string Result;
        LeadershipAbilityData Data;
        Data = (LeadershipAbilityData)Owner.GetAbilityData(FAbilityID);
        if (Data != null && Data.FExternalLevel > Data.FLevel)
        {
            Result = Data.FExternalSource;
        }
        else
        {
            Result = Owner.GetOwnerName();
        }

        return Result;
    }

    // AoWAb.PAS:1078
    public override bool GetInherent(AbilityOwner owner)
    {
        LeadershipAbilityData Data;
        Data = (LeadershipAbilityData)owner.GetAbilityData(FAbilityID);
        return Data != null && Data.FLevel > 0;
    }

    // AoWAb.PAS:1086
    public override bool CanExpand(AbilityOwner owner)
    {
        bool Result;
        LeadershipAbilityData Data;
        Data = (LeadershipAbilityData)owner.GetAbilityData(FAbilityID);
        if (Data != null)
        {
            Result = Data.FLevel < FMaxLevel;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // AoWAb.PAS:1097
    public override string GetLevelName(int Level) => AbilityRules.TranslateRStr(LeadershipRStr);

    // AoWAb.PAS:1024
    public void SetExternalSource(AbilityOwner Owner, byte Level, string SourceName)
    {
        LeadershipAbilityData Data;
        Data = (LeadershipAbilityData)Owner.GetAbilityData(FAbilityID);
        if (Data == null)
        {
            SetAb(Owner, FAbilityID, 1);
            Data = new LeadershipAbilityData(null);
            Data.FLevelAbilityID = (ushort)FAbilityID;
            Data.FLevel = 0;
            Owner.AddAbilityData(Data);
        }

        // { Original compares with the inherent level, even if an external level is higher. }
        if (Data.FLevel < Level)
        {
            Data.FExternalSource = SourceName;
            Data.FExternalLevel = Level;
        }

        if (Owner is Troop)
        {
            Owner.Changed();
        }
    }

    // AoWAb.PAS:1047
    public void ResetExternalSource(AbilityOwner Owner)
    {
        LeadershipAbilityData Data;
        Data = (LeadershipAbilityData)Owner.GetAbilityData(FAbilityID);
        if (Data != null)
        {
            if (Data.FLevel != 0)
            {
                Data.FExternalSource = "";
                Data.FExternalLevel = 0;
            }
            else
            {
                SetAb(Owner, FAbilityID, 0);
                Owner.RemoveAbilityData(Data);
            }
        }

        if (Owner is Troop)
        {
            Owner.Changed();
        }
    }

    // AoWAb.PAS:973
    public override EngineClass AbilityDataClass() => EngineClass.Of<LeadershipAbilityData>();

    private static SByteArray5 P4LeadershipAttack = SByteArray5.From((ReadOnlySpan<sbyte>)new sbyte[5] { 0, 1, 1, 2, 2 });
    private static SByteArray5 P4LeadershipDefense = SByteArray5.From((ReadOnlySpan<sbyte>)new sbyte[5] { 2, 0, 1, 1, 2 });
    private static readonly string LeadershipRStr = "Leadership";

}
