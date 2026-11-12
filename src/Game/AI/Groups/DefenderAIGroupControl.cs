// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class DefenderAIGroupControl : BehaviorAIGroupControl
{
    public DefenderAIGroupControl(ECustomNode owner) : base(owner)
    { }

    // BehaviorAG.pas:286
    public override void ProcessStates()
    {
        AIGroupDefenseControl defense;
        AIGroupTargetFinder finder;
        AIRecruitmentGroupFinder recruitment;
        AIProductionLocationFinder production;
        AIGroupRazeControl raze;
        AIGroupDefendTargetSelector defend;
        switch (State)
        {
            case 0:
                defense = new AIGroupDefenseControl(null);
                defense.DefendMode = AIGroupDefenseControl.GroupDefendMode.Split;
                ActivatePlugin(defense);
                defense.Release();
                State += 1;
                break;
            case 1:
                Targets.Clear();
                finder = new AIGroupTargetFinder(null);
                finder.Setup(this);
                if (Group.Transporter == null)
                {
                    var availableTransporter = FindAvailableTransporter();
                    finder.Selector.SetTransporter(availableTransporter);
                }
                else
                {
                    finder.Selector.SetTransporter(Group.Transporter);
                }

                ActivatePlugin(finder);
                finder.Release();
                State += 1;
                break;
            case 2:
                if (Targets.Count != 0)
                {
                    recruitment = new AIRecruitmentGroupFinder(null);
                    recruitment.Setup(this);
                    ActivatePlugin(recruitment);
                    recruitment.Release();
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
                    production = new AIProductionLocationFinder(null);
                    production.Setup(this);
                    ActivatePlugin(production);
                    production.Release();
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
                    TargetSelector.Setup(this);
                    TargetSelector.BattleFieldFactor = (float)1.7;
                    TargetSelector.CurrentBattleFieldFactor = (float)1.2;
                    TargetSelector.Superiority = 2;
                    TargetSelector.NearSuperiority = (float)1.5;
                    TargetSelector.MinimumStrength = 10000;
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
                    raze = new AIGroupRazeControl(null);
                    raze.Setup(this);
                    ActivatePlugin(raze);
                    raze.Release();
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
                var player = StrategicMap.Current.Players.GetPlayers(Group.Player);
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
                defend = new AIGroupDefendTargetSelector(null);
                defend.Setup(this);
                ActivatePlugin(defend);
                defend.Release();
                State = 11;
                break;
            case 11:
                if (Group.Transporter != null)
                {
                    Group.DeleteUnit(Group.Transporter);
                }

                CombineIdleGroups();
                Done();
                break;
        }
    }
}
