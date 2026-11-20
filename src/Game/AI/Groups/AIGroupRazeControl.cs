#nullable disable
// Translated from AGCPlugins.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.AI.Groups;

// { ot=1 }
// fields control, mode, damage types, structure factor, city factor
public partial class AIGroupRazeControl(CustomNode owner) : AIGroupTask(owner)
{
    private AIGroupControl _control;

    public Set<AIGroupRazeControlModeElement> Mode { get; set; }

    private Set<DamageType> _damageTypes;
    private float _structureStrengthFactor;

    public float CityStrengthFactor { get; set; }

    // AGCPlugins.pas:101
    public override bool Activate(AIGroupControl control)
    {
        State = 0;
        _control = control;
        return true;
    }

    // AGCPlugins.pas:133
    public override bool Process()
    {
        if (State == 0)
        {
            Structure structure = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(_control.Location).FindNoneTransparentHexSprite<Structure>();
            if (structure is not null && structure.GetRazeable())
            {
                BattleFieldInfo.SetupBattleFieldInfo(out BattleFieldInfo info, _control.Group.Player, _control.Group.IndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnoreIndependents), _damageTypes);
                StrategicMap.Current.RetrieveBattleFieldInfo(_control.Location.X, _control.Location.Y, _control.Location.MapLevel, 15, ref info);
                bool result = structure is City city ? ValidateRazeCity(city, info) : ValidateRazeStructure(structure, info);

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
        _damageTypes = AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        Mode = Set<AIGroupRazeControlModeElement>.Of(AIGroupRazeControlModeElement.CannotDefend);
        CityStrengthFactor = 2;
        _structureStrengthFactor = 2;
    }

    // AGCPlugins.pas:98
    public bool Setup(AIGroupControl control)
    {
        Reset();
        return true;
    }

    // AGCPlugins.pas:108
    public void ExecuteRaze(Structure structure) => structure.Raze(_control.Group.Player);

    // AGCPlugins.pas:111
    public bool ValidateRazeCity(City city, in BattleFieldInfo info)
    {
        bool result;
        switch (StrategicMap.Current.Races.GetRace(city.Race).GetPlayerRelation(_control.Group.Player))
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
                    return result || Round((double)info.FriendlyStrength * CityStrengthFactor) < info.Strength;
                }

                break;
            case Session.Status.Neutral:
            case Session.Status.Polite:
            case Session.Status.Friendly:
            default:
                return Mode.Contains(AIGroupRazeControlModeElement.Always);
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

        return (Mode.Contains(AIGroupRazeControlModeElement.CannotDefend) && Round((double)info.FriendlyStrength * _structureStrengthFactor) < info.Strength) || result;
    }

    // intf_P5_07.inc:218

}
