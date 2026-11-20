#nullable disable
// Translated from MainAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public class PassiveAIGroupControl(CustomNode owner) : AIGroupControl(owner)
{

    // MainAG.pas:199
    public override void ProcessStates()
    {
        switch (State)
        {
            case 0:
                CombinePassiveGroups();
                MapCoordinate location = Group.GetLocation();
                GroupDefenseSelector selector = new(this);
                _ = selector.Execute(Group, location.X, location.Y, L: location.MapLevel, units: null);
                if (selector.Defenders.Count != Group.Units.Count && Group.Player != 0)
                {
                    GroupBehavior randomGroupBehavior = StrategicMap.Current.Players.GetPlayers(Group.Player).AIControl.GetRandomGroupBehavior();
                    AIGroup group = StrategicMap.Current.AIGroupManager.CreateGroup(Group.Player, relation: Group.IndependentRelation, behavior: randomGroupBehavior);
                    for (int i = Group.Units.Count - 1; i >= 0; i--)
                    {
                        Troop troop = Group.Units.GetUnit(i);
                        if (selector.Defenders.IndexOf(troop) == -1)
                        {
                            _ = group.AddUnit(troop);
                        }
                    }

                    _ = group.Release();
                }

                if (selector.DefenseLocation != MapCoordinate.None)
                {
                    Group.TargetLocation = selector.DefenseLocation;
                    Group.TargetMode = (byte)AIGroupDefenseControl.GroupDefendMode.Custom;
                    SetupME();
                    MoveExecutor.Mode = MoveExecutor.Mode - Set<AIMoveExecutorModeElement>.Of(AIMoveExecutorModeElement.Adjacent);
                    ExecuteME(selector.DefenseLocation.X, selector.DefenseLocation.Y, L: selector.DefenseLocation.MapLevel);
                    Done();
                }
                else
                {
                    Done();
                }

                selector?.Dispose();
                break;
            default:
                break;
        }
    }

    // MainAG.pas:176
    public void CombinePassiveGroups()
    {
        Player player = StrategicMap.Current.Players.GetPlayers(Group.Player);
        MapCoordinate location = Group.GetLocation();
        for (int i = player.AIGroups.Count - 1; i >= 0; i--)
        {
            AIGroup group = player.AIGroups.GetGroups(i);
            if (Group.GroupId != group.GroupId && group.EngineClass.ClassId == 136280 && Group.IndependentRelation == group.IndependentRelation)
            {
                MapCoordinate otherLocation = group.GetLocation();
                if (location.MapLevel == otherLocation.MapLevel && HexGeometry.DeltaHexToRadius(location.X, location.Y, otherLocation.X, otherLocation.Y) <= 1)
                {
                    _ = Group.AddUnits(group.Units);
                }
            }
        }
    }
}
