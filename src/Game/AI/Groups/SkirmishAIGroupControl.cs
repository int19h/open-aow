// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.AI.Groups;

public partial class SkirmishAIGroupControl : BehaviorAIGroupControl
{
    public SkirmishAIGroupControl(ECustomNode owner) : base(owner)
    { }

    // BehaviorAG.pas:738
    public override void ProcessStates()
    {
        AIGroupTargetFinder finder;
        AIGroup group;
        switch (State)
        {
            case 0:
                Targets.Clear();
                finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
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
            case 1:
                if (Targets.Count != 0)
                {
                    TargetSelector.Setup(this);
                    TargetSelector.Superiority = 3;
                    TargetSelector.NearSuperiority = 2;
                    TargetSelector.Mode = TargetSelector.Mode - Set<AIGroupTargetingModes>.Of(AIGroupTargetingModes.Cooperate, AIGroupTargetingModes.Recruit, AIGroupTargetingModes.Produce);
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
                if (Group.Transporter != null)
                {
                    Group.DeleteUnit(Group.Transporter);
                }

                var randomGroupBehavior = StrategicMap.Current.Players.GetPlayers(Group.Player).AIControl.GetRandomGroupBehavior();
                group = StrategicMap.Current.AIGroupManager.CreateGroup(Group.Player, 0, randomGroupBehavior);
                group.AddUnits(Group.Units);
                group.Release();
                Done();
                break;
        }
    }
}
