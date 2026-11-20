#nullable disable
// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public class DefenderAIGroupControl(CustomNode owner) : BehaviorAIGroupControl(owner)
{

    // BehaviorAG.pas:286
    public override void ProcessStates()
    {
        switch (State)
        {
            case 0:
                AIGroupDefenseControl defense = new(null)
                {
                    Holder = CreationHolder(),
                    DefendMode = AIGroupDefenseControl.GroupDefendMode.Split
                };
                ActivatePlugin(defense);
                _ = defense.Release();
                State += 1;
                break;
            case 1:
                Targets.Clear();
                AIGroupTargetFinder finder = new(null) { Holder = CreationHolder() };
                _ = finder.Setup(this);
                if (Group.Transporter is null)
                {
                    Troop availableTransporter = FindAvailableTransporter();
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
            case 2:
                if (Targets.Count != 0)
                {
                    AIRecruitmentGroupFinder recruitment = new(null) { Holder = CreationHolder() };
                    _ = recruitment.Setup(this);
                    ActivatePlugin(recruitment);
                    _ = recruitment.Release();
                    State += 1;
                }
                else
                {
                    State = 10;
                }

                break;
            case 3:
                if (Targets.Count != 0)
                {
                    ProductionLocations.Clear();
                    AIProductionLocationFinder production = new(null) { Holder = CreationHolder() };
                    _ = production.Setup(this);
                    ActivatePlugin(production);
                    _ = production.Release();
                    State += 1;
                }
                else
                {
                    State = 10;
                }

                break;
            case 4:
                if (Targets.Count != 0)
                {
                    _ = TargetSelector.Setup(this);
                    TargetSelector.BattleFieldFactor = (float)1.7;
                    TargetSelector.CurrentBattleFieldFactor = (float)1.2;
                    TargetSelector.Superiority = 2;
                    TargetSelector.NearSuperiority = (float)1.5;
                    TargetSelector.MinimumStrength = 10_000;
                    TargetSelector.MultiTargetFactor = 2;
                    ActivatePlugin(TargetSelector);
                    State += 1;
                }
                else
                {
                    State += 1;
                }

                break;
            case 5:
                if (TargetSelector.TargetStatus == AIGroupTargetingStatus.TargetConquered)
                {
                    AIGroupRazeControl raze = new(null) { Holder = CreationHolder() };
                    _ = raze.Setup(this);
                    ActivatePlugin(raze);
                    _ = raze.Release();
                    State = 6;
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
            case 6:
                Player player = StrategicMap.Current.Players.GetPlayers(Group.Player);
                if (Set<PlayerType>.Of(PlayerType.Cpu3, PlayerType.Cpu4, PlayerType.Cpu5).Contains(player.PlayerType))
                {
                    State = 0;
                }
                else
                {
                    Done();
                }

                break;
            case 10:
                AIGroupDefendTargetSelector defend = new(null) { Holder = CreationHolder() };
                _ = defend.Setup(this);
                ActivatePlugin(defend);
                _ = defend.Release();
                State = 11;
                break;
            case 11:
                if (Group.Transporter is not null)
                {
                    _ = Group.DeleteUnit(Group.Transporter);
                }

                CombineIdleGroups();
                Done();
                break;
            default:
                break;
        }
    }
}
