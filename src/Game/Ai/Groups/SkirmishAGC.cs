// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

public partial class SkirmishAGC : AbstractBehaviorAGC
{
    public SkirmishAGC(ECustomNode owner) : base(owner)
    { }

    // BehaviorAG.pas:738
    public override void ProcessStates()
    {
        AIGroupTargetFinder Finder;
        AIGroup Group;
        switch (FState)
        {
            case 0:
                FTargets.Clear();
                Finder = new AIGroupTargetFinder(null);
                Finder.Setup(this);
                if (FGroup.FTransporter == null)
                {
                    var availableTransporter = FindAvailableTransporter();
                    Finder.FSelector.SetTransporter(availableTransporter);
                }
                else
                {
                    Finder.FSelector.SetTransporter(FGroup.FTransporter);
                }

                ActivatePlugin(Finder);
                Finder.Release();
                FState += 1;
                break;
            case 1:
                if (FTargets.Count != 0)
                {
                    FTargetSelector.Setup(this);
                    FTargetSelector.FSuperiority = 3;
                    FTargetSelector.FNearSuperiority = 2;
                    FTargetSelector.FMode = FTargetSelector.FMode - Set<AIGroupTargetSelectorModeElement>.Of(AIGroupTargetSelectorModeElement.Cooperate, AIGroupTargetSelectorModeElement.Recruit, AIGroupTargetSelectorModeElement.Produce);
                    ActivatePlugin(FTargetSelector);
                    FState += 1;
                }
                else
                {
                    FState = 10;
                }

                break;
            case 2:
                if (FTargetSelector.FTargetStatus == AIGroupTargetSelectorStatus.TargetConquered)
                {
                    FState = 5;
                }
                else
                {
                    if (FTargetSelector.FTargetStatus == AIGroupTargetSelectorStatus.NoTargetsFound)
                    {
                        FState = 10;
                    }
                    else
                    {
                        Done();
                    }
                }

                break;
            case 5:
                var player = AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer);
                if (Set<PlayerType>.Of(PlayerType.CPU3, PlayerType.CPU4, PlayerType.CPU5).Contains(player.FPlayerType))
                {
                    FState = 0;
                }
                else
                {
                    Done();
                }

                break;
            case 10:
                if (FGroup.FTransporter != null)
                {
                    FGroup.DeleteUnit(FGroup.FTransporter);
                }

                var randomGroupBehavior = AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer).FAIControl.GetRandomGroupBehavior();
                Group = AoWHSMap.Current.FAIGroupManager.CreateGroup(FGroup.FPlayer, 0, randomGroupBehavior);
                Group.AddUnits(FGroup);
                Group.Release();
                Done();
                break;
        }
    }
}
