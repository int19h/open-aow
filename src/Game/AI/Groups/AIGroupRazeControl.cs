// Translated from AGCPlugins.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// { ot=1 }
// fields control, mode, damage types, structure factor, city factor
public partial class AIGroupRazeControl : AIGroupTask
{
    public AIGroupRazeControl(ECustomNode owner) : base(owner)
    { }

    public AIGroupControl FControl;
    public Set<AIGroupRazeControlModeElement> FMode;
    public Set<DamageType> FDamageTypes;
    public float FStructureStrengthFactor;
    public float FCityStrengthFactor;

    // AGCPlugins.pas:101
    public override bool Activate(AIGroupControl Control)
    {
        FState = 0;
        FControl = Control;
        return true;
    }

    // AGCPlugins.pas:133
    public override bool Process()
    {
        bool Result;
        Structure Structure;
        BattleFieldInfo Info = default;
        if (FState == 0)
        {
            Structure = (Structure)AoWHSMap.Current.MapContainer.GetFieldXYL(Unsafe.As<MapCoordinate, int>(ref FControl.FLocation)).FindNoneTransparentHS(typeof(Structure));
            if (Structure != null && Structure.GetRazeable())
            {
                BattleFieldInfo.SetupBattleFieldInfo(ref Info, FControl.FGroup.FPlayer, FControl.FGroup.FIndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnoreIndependents), FDamageTypes);
                AoWHSMap.Current.RetrieveBattleFieldInfo(FControl.FLocation.X, FControl.FLocation.Y, FControl.FLocation.L, 15, ref Info);
                if (Structure is City)
                {
                    Result = ValidateRazeCity((City)Structure, Info);
                }
                else
                {
                    Result = ValidateRazeStructure(Structure, Info);
                }

                if (Result)
                {
                    ExecuteRaze(Structure);
                }
            }

            Done();
        }
        else
        {
            Done();
        }

        return false;
    }

    // AGCPlugins.pas:90
    public void Reset()
    {
        FDamageTypes = AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        FMode = Set<AIGroupRazeControlModeElement>.Of(AIGroupRazeControlModeElement.CannotDefend);
        FCityStrengthFactor = 2;
        FStructureStrengthFactor = 2;
    }

    // AGCPlugins.pas:98
    public bool Setup(AIGroupControl Control)
    {
        Reset();
        return true;
    }

    // AGCPlugins.pas:108
    public void ExecuteRaze(Structure Structure)
    {
        Structure.Raze(FControl.FGroup.FPlayer);
    }

    // AGCPlugins.pas:111
    public bool ValidateRazeCity(City City, in BattleFieldInfo Info)
    {
        bool Result;
        switch (AoWHSMap.Current.FRaces.GetRace(City.FRace).GetPlayerRelation(FControl.FGroup.FPlayer))
        {
            case Status.Hostile:
            case Status.Wary:
                Result = FMode.Contains(AIGroupRazeControlModeElement.Always);
                if (FMode.Contains(AIGroupRazeControlModeElement.OnMigrate))
                {
                    Result = true;
                }

                if (FMode.Contains(AIGroupRazeControlModeElement.CannotDefend))
                {
                    Result = Result || Round((double)Info.FriendlyStrength * FCityStrengthFactor) < Info.Strength;
                }

                break;
            default:
                Result = FMode.Contains(AIGroupRazeControlModeElement.Always);
                break;
        }

        return Result;
    }

    // AGCPlugins.pas:125
    public bool ValidateRazeStructure(Structure Structure, in BattleFieldInfo Info)
    {
        bool Result = false;
        if (FMode.Contains(AIGroupRazeControlModeElement.Always))
        {
            Result = true;
        }

        if (FMode.Contains(AIGroupRazeControlModeElement.CannotDefend) && Round((double)Info.FriendlyStrength * FStructureStrengthFactor) < Info.Strength)
        {
            Result = true;
        }

        return Result;
    }

    // intf_P5_07.inc:218

}
