// Translated from BehaviorAG.pas.
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Ai.Groups
{

    public partial class AbstractBehaviorAG : AIGroup
    {
        public AbstractBehaviorAG(ECustomNode owner) : base(owner)
        { }

        // BehaviorAG.pas:137
        public override bool CooperateWithGroup(AIGroup Group, AIGroupControlTarget Target)
        {
            bool Result = base.CooperateWithGroup(Group, Target);
            return Result || Group.GroupClass.Behavior == GroupBehavior.Fortify;
        }
    }

    // FTargetSelector
    public partial class AbstractBehaviorAGC : AIGroupControl
    {
        public AIGroupTargetSelector FTargetSelector;

        // BehaviorAG.pas:150
        public override void Dispose()
        {
            FTargetSelector.Release();
            base.Dispose();
        }

        // BehaviorAG.pas:144
        public AbstractBehaviorAGC(ECustomNode owner) : base(owner)
        {
            FTargetSelector = new AIGroupTargetSelector(null);
        }

    }

    public partial class SkirmishAG : AbstractBehaviorAG, IAIGroupClass
    {
        // BehaviorAG.pas:729
        public new const int ClassID = 136294;

        // BehaviorAG.pas:722
        public SkirmishAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 12;
            FRecruitmentPriority = 12;
        }

        // BehaviorAG.pas:735
        public override EClass GroupControlClass() => EClass.Of<SkirmishAGC>();

        // BehaviorAG.pas:732
        public static new GroupBehavior Behavior => GroupBehavior.Skirmish;

    }

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

    public partial class NormalAG : AbstractBehaviorAG, IAIGroupClass
    {
        // BehaviorAG.pas:163
        public new const int ClassID = 136292;

        // BehaviorAG.pas:156
        public NormalAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:169
        public override EClass GroupControlClass() => EClass.Of<NormalAGC>();

        // BehaviorAG.pas:166
        public static new GroupBehavior Behavior => GroupBehavior.Normal;

    }

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

    public partial class DefenderAG : AbstractBehaviorAG, IAIGroupClass
    {
        // BehaviorAG.pas:277
        public new const int ClassID = 136278;

        // BehaviorAG.pas:270
        public DefenderAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:283
        public override EClass GroupControlClass() => EClass.Of<DefenderAGC>();

        // BehaviorAG.pas:280
        public static new GroupBehavior Behavior => GroupBehavior.Defensive;

    }

    public partial class DefenderAGC : AbstractBehaviorAGC
    {
        public DefenderAGC(ECustomNode owner) : base(owner)
        { }

        // BehaviorAG.pas:286
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
                        FTargetSelector.FBattleFieldFactor = (float)1.7;
                        FTargetSelector.FCurrentBattleFieldFactor = (float)1.2;
                        FTargetSelector.FSuperiority = 2;
                        FTargetSelector.FNearSuperiority = (float)1.5;
                        FTargetSelector.FMinimumStrength = 10000;
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

    public partial class AggressorAG : AbstractBehaviorAG, IAIGroupClass
    {
        // BehaviorAG.pas:390
        public new const int ClassID = 136279;

        // BehaviorAG.pas:383
        public AggressorAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:396
        public override EClass GroupControlClass() => EClass.Of<AggressorAGC>();

        // BehaviorAG.pas:393
        public static new GroupBehavior Behavior => GroupBehavior.Aggressive;

    }

    public partial class AggressorAGC : AbstractBehaviorAGC
    {
        public AggressorAGC(ECustomNode owner) : base(owner)
        { }

        // BehaviorAG.pas:399
        public override void ProcessStates()
        {
            AIGroupDefenseControl Defense;
            AIGroupTargetFinder Finder;
            AIRecruitmentGroupFinder Recruitment;
            AIProductionLocationFinder Production;
            AIGroupRazeControl Raze;
            AIGroupWanderControl Wander;
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
                        FTargetSelector.FPathStrengthFactor = (float)0.75;
                        FTargetSelector.FCurrentPathStrengthFactor = (float)0.55;
                        FTargetSelector.FSuperiority = (float)1.4;
                        FTargetSelector.FNearSuperiority = (float)1.2;
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
                    Wander = new AIGroupWanderControl(null);
                    Wander.Setup(this);
                    Wander.FMovePointsMax = 100;
                    ActivatePlugin(Wander);
                    Wander.Release();
                    FState += 1;
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

    public partial class ExpanderAG : AbstractBehaviorAG, IAIGroupClass
    {
        // BehaviorAG.pas:502
        public new const int ClassID = 136290;

        // BehaviorAG.pas:495
        public ExpanderAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:508
        public override EClass GroupControlClass() => EClass.Of<ExpanderAGC>();

        // BehaviorAG.pas:505
        public static new GroupBehavior Behavior => GroupBehavior.Expand;

    }

    public partial class ExpanderAGC : AbstractBehaviorAGC
    {
        public ExpanderAGC(ECustomNode owner) : base(owner)
        { }

        // BehaviorAG.pas:511
        public override void ProcessStates()
        {
            AIGroupDefenseControl Defense;
            AIGroupTargetFinder Finder;
            AIRecruitmentGroupFinder Recruitment;
            AIProductionLocationFinder Production;
            AIGroupRazeControl Raze;
            AIGroupWanderControl Wander;
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
                        FTargetSelector.FMoveDayFactor = (float)0.05;
                        FTargetSelector.FPrioritySettings.FSetupBonus = 25;
                        FTargetSelector.FPrioritySettings.FExecuteBonus = 25;
                        FTargetSelector.FPrioritySettings.FMoveDayPenalty = -10;
                        FTargetSelector.FPrioritySettings.FImmediateBonus = 10;
                        FTargetSelector.FPrioritySettings.FTransportPenalty = -25;
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
                    Wander = new AIGroupWanderControl(null);
                    Wander.Setup(this);
                    Wander.FMovePointsMax = 100;
                    ActivatePlugin(Wander);
                    Wander.Release();
                    FState += 1;
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

    public partial class ScorcherAG : AbstractBehaviorAG, IAIGroupClass
    {
        // BehaviorAG.pas:616
        public new const int ClassID = 136291;

        // BehaviorAG.pas:609
        public ScorcherAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:622
        public override EClass GroupControlClass() => EClass.Of<ScorcherAGC>();

        // BehaviorAG.pas:619
        public static new GroupBehavior Behavior => GroupBehavior.Scorch;

    }

    public partial class ScorcherAGC : AbstractBehaviorAGC
    {
        public ScorcherAGC(ECustomNode owner) : base(owner)
        { }

        // BehaviorAG.pas:625
        public override void ProcessStates()
        {
            AIGroupDefenseControl Defense;
            AIGroupTargetFinder Finder;
            AIRecruitmentGroupFinder Recruitment;
            AIProductionLocationFinder Production;
            AIGroupRazeControl Raze;
            AIGroupWanderControl Wander;
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
                        FTargetSelector.FSuperiority = (float)1.75;
                        FTargetSelector.FNearSuperiority = (float)1.3;
                        FTargetSelector.FMultiTargetFactor = 200;
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
                        Raze.FMode = Raze.FMode + Set<AIGroupRazeControlModeElement>.Of(AIGroupRazeControlModeElement.OnMigrate);
                        Raze.FCityStrengthFactor = (float)1.5;
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
                    Wander = new AIGroupWanderControl(null);
                    Wander.Setup(this);
                    Wander.FMovePointsMax = 100;
                    ActivatePlugin(Wander);
                    Wander.Release();
                    FState += 1;
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
}

namespace Aow.Game.Ai.Groups
{

    public partial class BehaviorRegistrator
    {
        // BehaviorAG.pas:136
        public static void MapLoaded(HSMap Sender)
        {
            if (Sender == AoWHSMap.Current)
            {
                AoWHSMap.Current.FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<DefenderAG>(), AIGroupClass.Of<AggressorAG>(), AIGroupClass.Of<ExpanderAG>(), AIGroupClass.Of<ScorcherAG>(), AIGroupClass.Of<NormalAG>(), AIGroupClass.Of<SkirmishAG>()]);
            }
        }

        internal static void SubscribeMapLoaded()
        {
            if (GameTables.ValidateAoWEngine())
                GameTables.AoWEngine.OnHSMLoaded += MapLoaded;
        }

        internal static void UnsubscribeMapLoaded()
        {
            if (GameTables.AoWEngine != null)
                GameTables.AoWEngine.OnHSMLoaded -= MapLoaded;
        }
    }
}
