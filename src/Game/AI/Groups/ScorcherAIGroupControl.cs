#nullable disable
// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class ScorcherAIGroupControl(CustomNode owner) : BehaviorAIGroupControl(owner)
{

    // BehaviorAG.pas:625
    public override void ProcessStates()
    {
        AIGroupDefenseControl defense;
        AIGroupTargetFinder finder;
        AIRecruitmentGroupFinder recruitment;
        AIProductionLocationFinder production;
        AIGroupRazeControl raze;
        AIGroupWanderControl wander;
        switch (State)
        {
            case 0:
                defense = new AIGroupDefenseControl(null)
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
                finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                _ = finder.Setup(this);
                if (Group.Transporter == null)
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
                    recruitment = new AIRecruitmentGroupFinder(null) { Holder = CreationHolder() };
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
                    production = new AIProductionLocationFinder(null) { Holder = CreationHolder() };
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
                    TargetSelector.Superiority = (float)1.75;
                    TargetSelector.NearSuperiority = (float)1.3;
                    TargetSelector.MultiTargetFactor = 200;
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
                    raze = new AIGroupRazeControl(null) { Holder = CreationHolder() };
                    _ = raze.Setup(this);
                    raze.Mode += Set<AIGroupRazeControlModeElement>.Of(AIGroupRazeControlModeElement.OnMigrate);
                    raze.CityStrengthFactor = (float)1.5;
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
                wander = new AIGroupWanderControl(null) { Holder = CreationHolder() };
                _ = wander.Setup(this);
                wander.MovePointsMax = 100;
                ActivatePlugin(wander);
                _ = wander.Release();
                State += 1;
                break;
            case 11:
                if (Group.Transporter != null)
                {
                    _ = Group.DeleteUnit(Group.Transporter);
                }

                CombineIdleGroups();
                Done();
                break;
        }
    }
}
