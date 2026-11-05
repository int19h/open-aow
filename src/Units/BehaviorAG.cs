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
            bool Result = default;
            Result = base.CooperateWithGroup(Group, Target);
            Result = Result || Group.GroupClass.Behavior == TGroupBehavior.gbFortify;
            return Result;
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
            var call__1 = new TAIGroupTargetSelector(null);
            FTargetSelector = call__1;
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
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TSkirmishAGC>();
            return Result;
        }

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
            TAIGroupTargetFinder Finder = default;
            TAIGroup Group = default;
            switch (FState)
            {
                case 0:
                    FTargets.Clear();
                    Finder = new TAIGroupTargetFinder(null);
                    Finder.Setup(this);
                    if (FGroup.FTransporter == null)
                    {
                        var call__2 = FindAvailableTransporter();
                        Finder.FSelector.SetTransporter(call__2);
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
                    var call__3 = FTargets.Count;
                    if (call__3 != 0)
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
                    var call__4 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<TPlayerType>.Of(TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(call__4.FPlayerType))
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

                    var call__5 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    var call__6 = call__5.FAIControl.GetRandomGroupBehavior();
                    var call__7 = AoWHSMap.FAIGroupManager.CreateGroup(FGroup.FPlayer, 0, call__6);
                    Group = call__7;
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
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TNormalAGC>();
            return Result;
        }

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
            TAIGroupDefenseControl Defense = default;
            TAIGroupTargetFinder Finder = default;
            TAIRecruitmentGroupFinder Recruitment = default;
            TAIProductionLocationFinder Production = default;
            TAIGroupRazeControl Raze = default;
            TAIGroupDefendTargetSelector Defend = default;
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
                    var call__8 = FGroup.Count;
                    if (call__8 > 0)
                    {
                        FTargets.Clear();
                        Finder = new TAIGroupTargetFinder(null);
                        Finder.Setup(this);
                        if (FGroup.FTransporter == null)
                        {
                            var call__9 = FindAvailableTransporter();
                            Finder.FSelector.SetTransporter(call__9);
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
                    var call__10 = FTargets.Count;
                    if (call__10 != 0)
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
                    var call__11 = FTargets.Count;
                    if (call__11 != 0)
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
                    var call__12 = FTargets.Count;
                    if (call__12 != 0)
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
                    var call__13 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<TPlayerType>.Of(TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(call__13.FPlayerType))
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
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TDefenderAGC>();
            return Result;
        }

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
            TAIGroupDefenseControl Defense = default;
            TAIGroupTargetFinder Finder = default;
            TAIRecruitmentGroupFinder Recruitment = default;
            TAIProductionLocationFinder Production = default;
            TAIGroupRazeControl Raze = default;
            TAIGroupDefendTargetSelector Defend = default;
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
                        var call__14 = FindAvailableTransporter();
                        Finder.FSelector.SetTransporter(call__14);
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
                    var call__15 = FTargets.Count;
                    if (call__15 != 0)
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
                    var call__16 = FTargets.Count;
                    if (call__16 != 0)
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
                    var call__17 = FTargets.Count;
                    if (call__17 != 0)
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
                    var call__18 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<TPlayerType>.Of(TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(call__18.FPlayerType))
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
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TAggressorAGC>();
            return Result;
        }

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
            TAIGroupDefenseControl Defense = default;
            TAIGroupTargetFinder Finder = default;
            TAIRecruitmentGroupFinder Recruitment = default;
            TAIProductionLocationFinder Production = default;
            TAIGroupRazeControl Raze = default;
            TAIGroupWanderControl Wander = default;
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
                        var call__19 = FindAvailableTransporter();
                        Finder.FSelector.SetTransporter(call__19);
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
                    var call__20 = FTargets.Count;
                    if (call__20 != 0)
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
                    var call__21 = FTargets.Count;
                    if (call__21 != 0)
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
                    var call__22 = FTargets.Count;
                    if (call__22 != 0)
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
                    var call__23 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<TPlayerType>.Of(TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(call__23.FPlayerType))
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
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TExpanderAGC>();
            return Result;
        }

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
            TAIGroupDefenseControl Defense = default;
            TAIGroupTargetFinder Finder = default;
            TAIRecruitmentGroupFinder Recruitment = default;
            TAIProductionLocationFinder Production = default;
            TAIGroupRazeControl Raze = default;
            TAIGroupWanderControl Wander = default;
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
                        var call__24 = FindAvailableTransporter();
                        Finder.FSelector.SetTransporter(call__24);
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
                    var call__25 = FTargets.Count;
                    if (call__25 != 0)
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
                    var call__26 = FTargets.Count;
                    if (call__26 != 0)
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
                    var call__27 = FTargets.Count;
                    if (call__27 != 0)
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
                    var call__28 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<TPlayerType>.Of(TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(call__28.FPlayerType))
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
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TScorcherAGC>();
            return Result;
        }

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
            TAIGroupDefenseControl Defense = default;
            TAIGroupTargetFinder Finder = default;
            TAIRecruitmentGroupFinder Recruitment = default;
            TAIProductionLocationFinder Production = default;
            TAIGroupRazeControl Raze = default;
            TAIGroupWanderControl Wander = default;
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
                        var call__29 = FindAvailableTransporter();
                        Finder.FSelector.SetTransporter(call__29);
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
                    var call__30 = FTargets.Count;
                    if (call__30 != 0)
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
                    var call__31 = FTargets.Count;
                    if (call__31 != 0)
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
                    var call__32 = FTargets.Count;
                    if (call__32 != 0)
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
                    var call__33 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                    if (TSet<TPlayerType>.Of(TPlayerType.ptCPU3, TPlayerType.ptCPU4, TPlayerType.ptCPU5).Contains(call__33.FPlayerType))
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
