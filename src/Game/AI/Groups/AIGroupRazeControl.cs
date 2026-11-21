#nullable disable warnings
// Translated from AGCPlugins.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.AI.Groups;

// { ot=1 }
// fields control, mode, damage types, structure factor, city factor
public class AIGroupRazeControl(CustomNode? owner) : AIGroupTask(owner)
{
    private AIGroupControl _control;

    public AIGroupRazeControlModes Mode { get; set; }

    private DamageTypes _damageTypes;
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
            if (structure is not null && structure.Razeable)
            {
                BattleFieldInfo.SetupBattleFieldInfo(out BattleFieldInfo info, _control.Group.Player, relation: _control.Group.IndependentRelation, requests: BattleFieldInfoRequests.IgnoreIndependents, _damageTypes);
                StrategicMap.Current.RetrieveBattleFieldInfo(_control.Location.X, _control.Location.Y, _control.Location.MapLevel, radius: 15, ref info);
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
        _damageTypes = AbilityRules.AllDamageTypes & ~DamageTypes.Wall;
        Mode = AIGroupRazeControlModes.CannotDefend;
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
                result = Mode.Contains(AIGroupRazeControlModes.Always);
                if (Mode.Contains(AIGroupRazeControlModes.OnMigrate))
                {
                    result = true;
                }

                if (Mode.Contains(AIGroupRazeControlModes.CannotDefend))
                {
                    return result || Round((double)info.FriendlyStrength * CityStrengthFactor) < info.Strength;
                }

                break;
            case Session.Status.Neutral:
            case Session.Status.Polite:
            case Session.Status.Friendly:
            default:
                return Mode.Contains(AIGroupRazeControlModes.Always);
        }

        return result;
    }

    // AGCPlugins.pas:125
    public bool ValidateRazeStructure(Structure structure, in BattleFieldInfo info)
    {
        bool result = false;
        if (Mode.Contains(AIGroupRazeControlModes.Always))
        {
            result = true;
        }

        return (Mode.Contains(AIGroupRazeControlModes.CannotDefend) && Round((double)info.FriendlyStrength * _structureStrengthFactor) < info.Strength) || result;
    }
}
