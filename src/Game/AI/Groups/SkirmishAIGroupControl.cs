#nullable disable warnings
// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

namespace Aow.Game.AI.Groups;

public class SkirmishAIGroupControl(CustomNode owner) : BehaviorAIGroupControl(owner)
{

    // BehaviorAG.pas:738
    public override void ProcessStates()
    {
        switch (State)
        {
            case 0:
                Targets.Clear();
                AIGroupTargetFinder finder = new(null) { Holder = CreationHolder() };
                _ = finder.Setup(this);
                if (Group.Transporter is null)
                {
                    Troop? availableTransporter = FindAvailableTransporter();
                    finder.Selector.SetTransporter(availableTransporter);
                }
                else
                {
                    finder.Selector.SetTransporter(Group.Transporter);
                }

                ActivatePlugin(finder);
                _ = finder.Release();
                State += 1;
                break;
            case 1:
                if (Targets.Count != 0)
                {
                    _ = TargetSelector.Setup(this);
                    TargetSelector.Superiority = 3;
                    TargetSelector.NearSuperiority = 2;
                    TargetSelector.Mode &= ~(AIGroupTargetingModes.Cooperate | AIGroupTargetingModes.Recruit | AIGroupTargetingModes.Produce);
                    ActivatePlugin(TargetSelector);
                    State += 1;
                }
                else
                {
                    State = 10;
                }

                break;
            case 2:
                if (TargetSelector.TargetStatus == AIGroupTargetingStatus.TargetConquered)
                {
                    State = 5;
                }
                else
                {
                    if (TargetSelector.TargetStatus == AIGroupTargetingStatus.NoTargetsFound)
                    {
                        State = 10;
                    }
                    else
                    {
                        Done();
                    }
                }

                break;
            case 5:
                Player? player = GetEngine().StrategicMap.Players.GetPlayers(Group.Player);
                if (player.PlayerType is PlayerType.Cpu3 or PlayerType.Cpu4 or PlayerType.Cpu5)
                {
                    State = 0;
                }
                else
                {
                    Done();
                }

                break;
            case 10:
                if (Group.Transporter is not null)
                {
                    _ = Group.DeleteUnit(Group.Transporter);
                }

                GroupBehavior randomGroupBehavior = GetEngine().StrategicMap.Players.GetPlayers(Group.Player).AIControl.GetRandomGroupBehavior();
                AIGroup? group = GetEngine().StrategicMap.AIGroupManager.CreateGroup(Group.Player, relation: 0, behavior: randomGroupBehavior);
                _ = group.AddUnits(Group.Units);
                _ = group.Release();
                Done();
                break;
            default:
                break;
        }
    }
}
