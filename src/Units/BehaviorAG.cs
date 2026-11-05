// Translated from BehaviorAG.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AGCPlugins;

namespace Aow.Units.BehaviorAG
{
    public partial class TAbstractBehaviorAG : TAIGroup
    {
        public TAbstractBehaviorAG(TECustomNode owner) : base(owner)
        { }

        // BehaviorAG.pas:137
        public override bool CooperateWithGroup(TAIGroup Group, TAIGroupControlTarget Target)
        {
            bool Result = base.CooperateWithGroup(Group, Target);
            return Result || Group.GroupClass.Behavior == TGroupBehavior.gbFortify;
        }
    }

    // FTargetSelector
    public partial class TAbstractBehaviorAGC : TAIGroupControl
    {
        public TAIGroupTargetSelector FTargetSelector;
        // BehaviorAG.pas:150
        public override void Destroy()
        {
            FTargetSelector.Release();
            base.Destroy();
        }

        // BehaviorAG.pas:144
        public TAbstractBehaviorAGC(TECustomNode owner) : base(owner)
        {
            FTargetSelector = new TAIGroupTargetSelector(null);
        }

    }

    public partial class TSkirmishAG : TAbstractBehaviorAG, IAIGroupClass
    {
        // BehaviorAG.pas:729
        public new const int ClassID = 136294;

        // BehaviorAG.pas:722
        public TSkirmishAG(TECustomNode owner) : base(owner)
        {
            FProcessPriority = 12;
            FRecruitmentPriority = 12;
        }

        // BehaviorAG.pas:735
        public override EClass GroupControlClass() => EClass.Of<TSkirmishAGC>();

        // BehaviorAG.pas:732
        public static new TGroupBehavior Behavior => TGroupBehavior.gbSkirmish;

    }

    public partial class TSkirmishAGC : TAbstractBehaviorAGC
    {
        public TSkirmishAGC(TECustomNode owner) : base(owner)
        { }

        // BehaviorAG.pas:738
        public override void ProcessStates()
        {
            TAIGroupTargetFinder Finder;
            TAIGroup Group;
            switch (FState)
            {
                case 0:
                    FTargets.Clear();
                    Finder = new TAIGroupTargetFinder(null);
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
                        FTargetSelector.FMode = FTargetSelector.FMode - TSet<TAIGroupTargetSelectorModeElement>.Of(TAIGroupTargetSelectorModeElement.tsmCooperate, TAIGroupTargetSelectorModeElement.tsmRecruit, TAIGroupTargetSelectorModeElement.tsmProduce);
                        ActivatePlugin(FTargetSelector);
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 2:
                    if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        FState = 5;
                    }
                    else
                    {
                        if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssNoTargetsFound)
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
                    var player = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<TPlayerType>.Of(TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(player.FPlayerType))
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

                    var randomGroupBehavior = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer).FAIControl.GetRandomGroupBehavior();
                    Group = AoWHSMap.FAIGroupManager.CreateGroup(FGroup.FPlayer, 0, randomGroupBehavior);
                    Group.AddUnits(FGroup);
                    Group.Release();
                    Done();
                    break;
            }
        }
    }

    public partial class TNormalAG : TAbstractBehaviorAG, IAIGroupClass
    {
        // BehaviorAG.pas:163
        public new const int ClassID = 136292;

        // BehaviorAG.pas:156
        public TNormalAG(TECustomNode owner) : base(owner)
        {
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:169
        public override EClass GroupControlClass() => EClass.Of<TNormalAGC>();

        // BehaviorAG.pas:166
        public static new TGroupBehavior Behavior => TGroupBehavior.gbNormal;

    }

    public partial class TNormalAGC : TAbstractBehaviorAGC
    {
        public TNormalAGC(TECustomNode owner) : base(owner)
        { }

        // BehaviorAG.pas:172
        public override void ProcessStates()
        {
            TAIGroupDefenseControl Defense;
            TAIGroupTargetFinder Finder;
            TAIRecruitmentGroupFinder Recruitment;
            TAIProductionLocationFinder Production;
            TAIGroupRazeControl Raze;
            TAIGroupDefendTargetSelector Defend;
            switch (FState)
            {
                case 0:
                    Defense = new TAIGroupDefenseControl(null);
                    Defense.FDefendMode = TGroupDefendMode.gdmSplit;
                    ActivatePlugin(Defense);
                    Defense.Release();
                    FState += 1;
                    break;
                case 1:
                    if (FGroup.Count > 0)
                    {
                        FTargets.Clear();
                        Finder = new TAIGroupTargetFinder(null);
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
                        Recruitment = new TAIRecruitmentGroupFinder(null);
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
                        Production = new TAIProductionLocationFinder(null);
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
                    if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        Raze = new TAIGroupRazeControl(null);
                        Raze.Setup(this);
                        ActivatePlugin(Raze);
                        Raze.Release();
                        FState = 6;
                    }
                    else
                    {
                        if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssNoTargetsFound)
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
                    var player = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<TPlayerType>.Of(TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(player.FPlayerType))
                    {
                        FState = 0;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 10:
                    Defend = new TAIGroupDefendTargetSelector(null);
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

    public partial class TDefenderAG : TAbstractBehaviorAG, IAIGroupClass
    {
        // BehaviorAG.pas:277
        public new const int ClassID = 136278;

        // BehaviorAG.pas:270
        public TDefenderAG(TECustomNode owner) : base(owner)
        {
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:283
        public override EClass GroupControlClass() => EClass.Of<TDefenderAGC>();

        // BehaviorAG.pas:280
        public static new TGroupBehavior Behavior => TGroupBehavior.gbDefensive;

    }

    public partial class TDefenderAGC : TAbstractBehaviorAGC
    {
        public TDefenderAGC(TECustomNode owner) : base(owner)
        { }

        // BehaviorAG.pas:286
        public override void ProcessStates()
        {
            TAIGroupDefenseControl Defense;
            TAIGroupTargetFinder Finder;
            TAIRecruitmentGroupFinder Recruitment;
            TAIProductionLocationFinder Production;
            TAIGroupRazeControl Raze;
            TAIGroupDefendTargetSelector Defend;
            switch (FState)
            {
                case 0:
                    Defense = new TAIGroupDefenseControl(null);
                    Defense.FDefendMode = TGroupDefendMode.gdmSplit;
                    ActivatePlugin(Defense);
                    Defense.Release();
                    FState += 1;
                    break;
                case 1:
                    FTargets.Clear();
                    Finder = new TAIGroupTargetFinder(null);
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
                        Recruitment = new TAIRecruitmentGroupFinder(null);
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
                        Production = new TAIProductionLocationFinder(null);
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
                    if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        Raze = new TAIGroupRazeControl(null);
                        Raze.Setup(this);
                        ActivatePlugin(Raze);
                        Raze.Release();
                        FState = 6;
                    }
                    else
                    {
                        if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssNoTargetsFound)
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
                    var player = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<TPlayerType>.Of(TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(player.FPlayerType))
                    {
                        FState = 0;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 10:
                    Defend = new TAIGroupDefendTargetSelector(null);
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

    public partial class TAggressorAG : TAbstractBehaviorAG, IAIGroupClass
    {
        // BehaviorAG.pas:390
        public new const int ClassID = 136279;

        // BehaviorAG.pas:383
        public TAggressorAG(TECustomNode owner) : base(owner)
        {
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:396
        public override EClass GroupControlClass() => EClass.Of<TAggressorAGC>();

        // BehaviorAG.pas:393
        public static new TGroupBehavior Behavior => TGroupBehavior.gbAggressive;

    }

    public partial class TAggressorAGC : TAbstractBehaviorAGC
    {
        public TAggressorAGC(TECustomNode owner) : base(owner)
        { }

        // BehaviorAG.pas:399
        public override void ProcessStates()
        {
            TAIGroupDefenseControl Defense;
            TAIGroupTargetFinder Finder;
            TAIRecruitmentGroupFinder Recruitment;
            TAIProductionLocationFinder Production;
            TAIGroupRazeControl Raze;
            TAIGroupWanderControl Wander;
            switch (FState)
            {
                case 0:
                    Defense = new TAIGroupDefenseControl(null);
                    Defense.FDefendMode = TGroupDefendMode.gdmSplit;
                    ActivatePlugin(Defense);
                    Defense.Release();
                    FState += 1;
                    break;
                case 1:
                    FTargets.Clear();
                    Finder = new TAIGroupTargetFinder(null);
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
                        Recruitment = new TAIRecruitmentGroupFinder(null);
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
                        Production = new TAIProductionLocationFinder(null);
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
                    if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        Raze = new TAIGroupRazeControl(null);
                        Raze.Setup(this);
                        ActivatePlugin(Raze);
                        Raze.Release();
                        FState = 6;
                    }
                    else
                    {
                        if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssNoTargetsFound)
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
                    var player = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<TPlayerType>.Of(TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(player.FPlayerType))
                    {
                        FState = 0;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 10:
                    Wander = new TAIGroupWanderControl(null);
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

    public partial class TExpanderAG : TAbstractBehaviorAG, IAIGroupClass
    {
        // BehaviorAG.pas:502
        public new const int ClassID = 136290;

        // BehaviorAG.pas:495
        public TExpanderAG(TECustomNode owner) : base(owner)
        {
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:508
        public override EClass GroupControlClass() => EClass.Of<TExpanderAGC>();

        // BehaviorAG.pas:505
        public static new TGroupBehavior Behavior => TGroupBehavior.gbExpand;

    }

    public partial class TExpanderAGC : TAbstractBehaviorAGC
    {
        public TExpanderAGC(TECustomNode owner) : base(owner)
        { }

        // BehaviorAG.pas:511
        public override void ProcessStates()
        {
            TAIGroupDefenseControl Defense;
            TAIGroupTargetFinder Finder;
            TAIRecruitmentGroupFinder Recruitment;
            TAIProductionLocationFinder Production;
            TAIGroupRazeControl Raze;
            TAIGroupWanderControl Wander;
            switch (FState)
            {
                case 0:
                    Defense = new TAIGroupDefenseControl(null);
                    Defense.FDefendMode = TGroupDefendMode.gdmSplit;
                    ActivatePlugin(Defense);
                    Defense.Release();
                    FState += 1;
                    break;
                case 1:
                    FTargets.Clear();
                    Finder = new TAIGroupTargetFinder(null);
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
                        Recruitment = new TAIRecruitmentGroupFinder(null);
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
                        Production = new TAIProductionLocationFinder(null);
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
                    if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        Raze = new TAIGroupRazeControl(null);
                        Raze.Setup(this);
                        ActivatePlugin(Raze);
                        Raze.Release();
                        FState = 6;
                    }
                    else
                    {
                        if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssNoTargetsFound)
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
                    var player = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<TPlayerType>.Of(TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(player.FPlayerType))
                    {
                        FState = 0;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 10:
                    Wander = new TAIGroupWanderControl(null);
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

    public partial class TScorcherAG : TAbstractBehaviorAG, IAIGroupClass
    {
        // BehaviorAG.pas:616
        public new const int ClassID = 136291;

        // BehaviorAG.pas:609
        public TScorcherAG(TECustomNode owner) : base(owner)
        {
            FProcessPriority = 10;
            FRecruitmentPriority = 10;
        }

        // BehaviorAG.pas:622
        public override EClass GroupControlClass() => EClass.Of<TScorcherAGC>();

        // BehaviorAG.pas:619
        public static new TGroupBehavior Behavior => TGroupBehavior.gbScorch;

    }

    public partial class TScorcherAGC : TAbstractBehaviorAGC
    {
        public TScorcherAGC(TECustomNode owner) : base(owner)
        { }

        // BehaviorAG.pas:625
        public override void ProcessStates()
        {
            TAIGroupDefenseControl Defense;
            TAIGroupTargetFinder Finder;
            TAIRecruitmentGroupFinder Recruitment;
            TAIProductionLocationFinder Production;
            TAIGroupRazeControl Raze;
            TAIGroupWanderControl Wander;
            switch (FState)
            {
                case 0:
                    Defense = new TAIGroupDefenseControl(null);
                    Defense.FDefendMode = TGroupDefendMode.gdmSplit;
                    ActivatePlugin(Defense);
                    Defense.Release();
                    FState += 1;
                    break;
                case 1:
                    FTargets.Clear();
                    Finder = new TAIGroupTargetFinder(null);
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
                        Recruitment = new TAIRecruitmentGroupFinder(null);
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
                        Production = new TAIProductionLocationFinder(null);
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
                    if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        Raze = new TAIGroupRazeControl(null);
                        Raze.Setup(this);
                        Raze.FMode = Raze.FMode + TSet<TAIGroupRazeControlModeElement>.Of(TAIGroupRazeControlModeElement.rcmOnMigrate);
                        Raze.FCityStrengthFactor = (float)1.5;
                        ActivatePlugin(Raze);
                        Raze.Release();
                        FState = 6;
                    }
                    else
                    {
                        if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssNoTargetsFound)
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
                    var player = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<TPlayerType>.Of(TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(player.FPlayerType))
                    {
                        FState = 0;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 10:
                    Wander = new TAIGroupWanderControl(null);
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

    public partial class TRegistrator : TObject
    {
        // BehaviorAG.pas:136
        public static void MapLoaded(TObject Sender)
        {
            if (Sender == AoWHSMap)
            {
                AoWHSMap.FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<TDefenderAG>(), AIGroupClass.Of<TAggressorAG>(), AIGroupClass.Of<TExpanderAG>(), AIGroupClass.Of<TScorcherAG>(), AIGroupClass.Of<TNormalAG>(), AIGroupClass.Of<TSkirmishAG>()]);
            }
        }

    }

    public static partial class BehaviorAG
    {
        public static void Initialize()
        {
            if (ValidateAoWEngine())
            {
                AoWEngine.RegisterEClasses([EClass.Of<TDefenderAG>(), EClass.Of<TAggressorAG>(), EClass.Of<TExpanderAG>(), EClass.Of<TScorcherAG>(), EClass.Of<TNormalAG>(), EClass.Of<TSkirmishAG>()]);
                AoWEngine.OnHSMLoaded.Add(TRegistrator.MapLoaded);
            }
        }

        public static void Finalize()
        {
            if (AoWEngine != null)
            {
                AoWEngine.OnHSMLoaded.Remove(TRegistrator.MapLoaded);
            }
        }
    }
}
