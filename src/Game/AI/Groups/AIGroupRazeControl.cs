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

    public AIGroupControl Control;
    public Set<AIGroupRazeControlModeElement> Mode;
    public Set<DamageType> DamageTypes;
    public float StructureStrengthFactor;
    public float CityStrengthFactor;

    // AGCPlugins.pas:101
    public override bool Activate(AIGroupControl control)
    {
        State = 0;
        Control = control;
        return true;
    }

    // AGCPlugins.pas:133
    public override bool Process()
    {
        bool result;
        Structure structure;
        BattleFieldInfo info = default;
        if (State == 0)
        {
            structure = (Structure)AoWHSMap.Current.MapContainer.GetFieldMapCoordinate(Unsafe.As<MapCoordinate, int>(ref Control.Location)).FindNoneTransparentHexSprite(typeof(Structure));
            if (structure != null && structure.GetRazeable())
            {
                BattleFieldInfo.SetupBattleFieldInfo(ref info, Control.Group.Player, Control.Group.IndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnoreIndependents), DamageTypes);
                AoWHSMap.Current.RetrieveBattleFieldInfo(Control.Location.X, Control.Location.Y, Control.Location.MapLevel, 15, ref info);
                if (structure is City)
                {
                    result = ValidateRazeCity((City)structure, info);
                }
                else
                {
                    result = ValidateRazeStructure(structure, info);
                }

                if (result)
                {
                    ExecuteRaze(structure);
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
        DamageTypes = AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        Mode = Set<AIGroupRazeControlModeElement>.Of(AIGroupRazeControlModeElement.CannotDefend);
        CityStrengthFactor = 2;
        StructureStrengthFactor = 2;
    }

    // AGCPlugins.pas:98
    public bool Setup(AIGroupControl control)
    {
        Reset();
        return true;
    }

    // AGCPlugins.pas:108
    public void ExecuteRaze(Structure structure)
    {
        structure.Raze(Control.Group.Player);
    }

    // AGCPlugins.pas:111
    public bool ValidateRazeCity(City city, in BattleFieldInfo info)
    {
        bool result;
        switch (AoWHSMap.Current.Races.GetRace(city.Race).GetPlayerRelation(Control.Group.Player))
        {
            case Session.Status.Hostile:
            case Session.Status.Wary:
                result = Mode.Contains(AIGroupRazeControlModeElement.Always);
                if (Mode.Contains(AIGroupRazeControlModeElement.OnMigrate))
                {
                    result = true;
                }

                if (Mode.Contains(AIGroupRazeControlModeElement.CannotDefend))
                {
                    result = result || Round((double)info.FriendlyStrength * CityStrengthFactor) < info.Strength;
                }

                break;
            default:
                result = Mode.Contains(AIGroupRazeControlModeElement.Always);
                break;
        }

        return result;
    }

    // AGCPlugins.pas:125
    public bool ValidateRazeStructure(Structure structure, in BattleFieldInfo info)
    {
        bool result = false;
        if (Mode.Contains(AIGroupRazeControlModeElement.Always))
        {
            result = true;
        }

        if (Mode.Contains(AIGroupRazeControlModeElement.CannotDefend) && Round((double)info.FriendlyStrength * StructureStrengthFactor) < info.Strength)
        {
            result = true;
        }

        return result;
    }

    // intf_P5_07.inc:218

}
