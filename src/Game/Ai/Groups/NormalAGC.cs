// Translated from BehaviorAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Ai.Groups;

public partial class NormalAGC : AbstractBehaviorAGC
{
    public NormalAGC(ECustomNode owner) : base(owner)
    { }

    // BehaviorAG.pas:172
    public override void ProcessStates()
    {
        AIGroupDefenseControl Defense;
        AIGroupTargetFinder Finder;
        AIRecruitmentGroupFinder Recruitment;
        AIProductionLocationFinder Production;
        AIGroupRazeControl Raze;
        AIGroupDefendTargetSelector Defend;
        switch (FState)
        {
            case 0:
                Defense = new AIGroupDefenseControl(null);
                Defense.FDefendMode = AIGroupDefenseControl.GroupDefendMode.Split;
                ActivatePlugin(Defense);
                Defense.Release();
                FState += 1;
                break;
            case 1:
                if (FGroup.Count > 0)
                {
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
                }
                else
                {
                    Done();
                }

                break;
            case 2:
                if (FTargets.Count != 0)
                {
                    Recruitment = new AIRecruitmentGroupFinder(null);
                    Recruitment.Setup(this);
                    ActivatePlugin(Recruitment);
                    Recruitment.Release();
                    FState += 1;
                }
                else
                {
                    FState = 10;
                }

                break;
            case 3:
                if (FTargets.Count != 0)
                {
                    FProductionLocations.Clear();
                    Production = new AIProductionLocationFinder(null);
                    Production.Setup(this);
                    ActivatePlugin(Production);
                    Production.Release();
                    FState += 1;
                }
                else
                {
                    FState = 10;
                }

                break;
            case 4:
                if (FTargets.Count != 0)
                {
                    FTargetSelector.Setup(this);
                    FTargetSelector.FSuperiority = (float)1.5;
                    FTargetSelector.FPathStrengthFactor = (float)0.9;
                    FTargetSelector.FCurrentPathStrengthFactor = (float)0.7;
                    FTargetSelector.FNearSuperiority = (float)1.3;
                    FTargetSelector.FMultiTargetFactor = 2;
                    ActivatePlugin(FTargetSelector);
                    FState += 1;
                }
                else
                {
                    FState += 1;
                }

                break;
            case 5:
                if (FTargetSelector.FTargetStatus == AIGroupTargetSelectorStatus.TargetConquered)
                {
                    Raze = new AIGroupRazeControl(null);
                    Raze.Setup(this);
                    ActivatePlugin(Raze);
                    Raze.Release();
                    FState = 6;
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
            case 6:
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
                Defend = new AIGroupDefendTargetSelector(null);
                Defend.Setup(this);
                ActivatePlugin(Defend);
                Defend.Release();
                FState = 11;
                break;
            case 11:
                if (FGroup.FTransporter != null)
                {
                    FGroup.DeleteUnit(FGroup.FTransporter);
                }

                CombineIdleGroups();
                Done();
                break;
        }
    }
}
