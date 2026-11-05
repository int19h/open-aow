// Translated from MainAG.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.City;
using Aow.Units.AGCPlugins;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.MainAG
{
    public partial class TPassiveAG : TAIGroup, IAIGroupClass
    {
        // MainAG.pas:129
        public new const int ClassID = 136280;

        // MainAG.pas:122
        public TPassiveAG(TECustomNode owner) : base(owner)
        {
            FProcessPriority = 0;
            FRecruitmentPriority = 1;
        }

        // MainAG.pas:138
        public override int GetLoc()
        {
            int Result = default;
            TXYL Location = default;
            if (FTargetMode == (byte)TGroupDefendMode.gdmCustom)
            {
                Location = FTargetXYL;
            }
            else
            {
                Location.XYL = base.GetLoc();
            }

            Result = Location.XYL;
            return Result;
        }

        // MainAG.pas:147
        public override bool GetRecruitableUnits(int Phase, TUnitList Units, TUnitList Recruitable)
        {
            bool Result = default;
            TXYL Location = default;
            TGroupDefenseSelector Selector = default;
            int I = default;
            TAbstractUnit AUnit = default;
            var call__1 = GetLoc();
            Location.XYL = call__1;
            Result = false;
            Selector = new TGroupDefenseSelector();
            Selector.Execute(this, Location.X, Location.Y, Location.L, Units);
            var call__2 = Selector.FDefenders.Count;
            if (call__2 != 0)
            {
                {
                    I = Units.Count - 1;
                    byte end__3 = 0;
                    for (; I >= end__3; I -= 1)
                    {
                        AUnit = Units.GetUnit(I);
                        var call__4 = Selector.FDefenders.IndexOf(AUnit);
                        if (call__4 == -1)
                        {
                            Recruitable.AddUnit(AUnit);
                            Result = true;
                        }
                    }
                }
            }
            else
            {
                Recruitable.AddUnits(Units);
                Result = true;
            }

            Selector.Free();
            return Result;
        }

        // MainAG.pas:135
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TPassiveAGC>();
            return Result;
        }

        // MainAG.pas:132
        public static new TGroupBehavior Behavior => TGroupBehavior.gbPassive;

    }

    public partial class TPassiveAGC : TAIGroupControl
    {
        public TPassiveAGC(TECustomNode owner) : base(owner)
        { }

        // MainAG.pas:199
        public override void ProcessStates()
        {
            TXYL Location = default;
            TGroupDefenseSelector Selector = default;
            TAIGroup Group = default;
            int I = default;
            TAbstractUnit AUnit = default;
            switch (FState)
            {
                case 0:
                    CombinePassiveGroups();
                    var call__5 = FGroup.GetLoc();
                    Location.XYL = call__5;
                    Selector = new TGroupDefenseSelector();
                    Selector.Execute(FGroup, Location.X, Location.Y, Location.L, null);
                    var call__6 = Selector.FDefenders.Count;
                    var call__7 = FGroup.Count;
                    if (call__6 != call__7 && FGroup.FPlayer != 0)
                    {
                        var call__8 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                        var call__9 = call__8.FAIControl.GetRandomGroupBehavior();
                        var call__10 = AoWHSMap.FAIGroupManager.CreateGroup(FGroup.FPlayer, FGroup.FIndependentRelation, call__9);
                        Group = call__10;
                        {
                            I = FGroup.Count - 1;
                            byte end__11 = 0;
                            for (; I >= end__11; I -= 1)
                            {
                                var call__12 = FGroup.GetUnit(I);
                                AUnit = call__12;
                                var call__13 = Selector.FDefenders.IndexOf(AUnit);
                                if (call__13 == -1)
                                {
                                    Group.AddUnit(AUnit);
                                }
                            }
                        }

                        Group.Release();
                    }

                    if (Selector.FDefenseLocation.XYL != 16777215)
                    {
                        FGroup.FTargetXYL = Selector.FDefenseLocation;
                        FGroup.FTargetMode = (byte)TGroupDefendMode.gdmCustom;
                        SetupME();
                        FMoveExecuter.FMode = FMoveExecuter.FMode - TSet<TAIMoveExecuterModeElement>.Of(TAIMoveExecuterModeElement.memAdjacent);
                        ExecuteME(Selector.FDefenseLocation.X, Selector.FDefenseLocation.Y, Selector.FDefenseLocation.L);
                        Done();
                    }
                    else
                    {
                        Done();
                    }

                    Selector.Free();
                    break;
            }
        }

        // MainAG.pas:176
        public void CombinePassiveGroups()
        {
            TPlayer Player = default;
            TXYL Location = default;
            TXYL OtherLocation = default;
            int I = default;
            TAIGroup Group = default;
            var call__14 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
            Player = call__14;
            var call__15 = FGroup.GetLoc();
            Location.XYL = call__15;
            {
                I = Player.FAIGroups.Count - 1;
                byte end__16 = 0;
                for (; I >= end__16; I -= 1)
                {
                    var call__17 = Player.FAIGroups.GetGroups(I);
                    Group = call__17;
                    bool bool__18 = FGroup.FGroupID != Group.FGroupID;
                    if (bool__18)
                    {
                        var call__19 = Group.EClass.ClassID;
                        bool__18 = call__19 == 136280;
                    }

                    if (bool__18 && FGroup.FIndependentRelation == Group.FIndependentRelation)
                    {
                        var call__20 = Group.GetLoc();
                        OtherLocation.XYL = call__20;
                        bool bool__21 = Location.L == OtherLocation.L;
                        if (bool__21)
                        {
                            var call__22 = dHXtoRad(Location.X, Location.Y, OtherLocation.X, OtherLocation.Y);
                            bool__21 = call__22 <= 1;
                        }

                        if (bool__21)
                        {
                            FGroup.AddUnits(Group);
                        }
                    }
                }
            }
        }
    }

    public partial class TFortifyAG : TAIGroup, IAIGroupClass
    {
        // MainAG.pas:247
        public new const int ClassID = 136281;

        // MainAG.pas:240
        public TFortifyAG(TECustomNode owner) : base(owner)
        {
            FProcessPriority = 15;
            FRecruitmentPriority = 0;
        }

        // MainAG.pas:253
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TFortifyAGC>();
            return Result;
        }

        // MainAG.pas:250
        public static new TGroupBehavior Behavior => TGroupBehavior.gbFortify;

    }

    // city location, enemy strength, required strength, target selector
    public partial class TFortifyAGC : TAIGroupControl
    {
        public TXYL FFortifyLocation;
        public int FEnemyStrength;
        public int FRequiredStrength;
        public TAIGroupTargetSelector FTargetSelector;
        // MainAG.pas:262
        public override void Destroy()
        {
            FTargetSelector.Release();
            base.Destroy();
        }

        // MainAG.pas:256
        public TFortifyAGC(TECustomNode owner) : base(owner)
        {
            var call__23 = new TAIGroupTargetSelector(null);
            FTargetSelector = call__23;
        }

        // MainAG.pas:278
        public override void TargetFoundEvent(TAIGroupControlPlugin Sender, TAIGroupControlTarget Target, ref bool SearchDone)
        {
            switch (((TAIGroupTargetFinder)Sender).FSelector.FTargetInfo.Mode)
            {
                case TAITargetMode.tmFortify:
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                    break;
                case TAITargetMode.tmAttackArmies:
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                    break;
                case TAITargetMode.tmAttack:
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                    break;
            }
        }

        // MainAG.pas:407
        public override void ProcessStates()
        {
            TAIGroupTargetFinder Finder = default;
            TAIRecruitmentGroupFinder Recruitment = default;
            TAIProductionLocationFinder ProductionFinder = default;
            TAIGroupRecruitControl Recruit = default;
            TAIGroupProductionControl Produce = default;
            TAIGroupDefenseControl Defense = default;
            TAIGroupControlTarget CurrentTarget = default;
            TAIGroupControlTarget NewTarget = default;
            int I = default;
            int Excess = default;
            TArmyHS Army = default;
            TCity ACity = default;
            switch (FState)
            {
                case 0:
                    Finder = new TAIGroupTargetFinder(null);
                    Finder.Setup(this);
                    Finder.FSelector.FTargetInfo.Mode = TAITargetMode.tmFortify;
                    Finder.FMode = Finder.FMode + TSet<TAIGroupTargetFinderModeElement>.Of(TAIGroupTargetFinderModeElement.tfmCalculateMovepathBFS);
                    Finder.FRange = 12;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 1:
                    I = 0;
                    while (true)
                    {
                        var call__24 = FTargets.Count;
                        bool bool__25 = I < call__24;
                        if (bool__25)
                        {
                            var call__26 = FTargets.GetTargets(I);
                            bool__25 = call__26.FStatus.Contains(TAIGroupControlTargetStatusElement.gtsDone);
                        }

                        if (!bool__25)
                            break;
                        {
                            I += 1;
                        }
                    }

                    var call__27 = FTargets.Count;
                    if (I < call__27)
                    {
                        var call__28 = FTargets.GetTargets(I);
                        var call__29 = GetSurroundingEnemyStrength(call__28.FLocation, 70);
                        var call__30 = FTargets.GetTargets(I);
                        call__30.FDataA = call__29;
                        var call__31 = FTargets.GetTargets(I);
                        var call__32 = FTargets.GetTargets(I);
                        call__32.FStatus = call__31.FStatus + TSet<TAIGroupControlTargetStatusElement>.Of(TAIGroupControlTargetStatusElement.gtsDone);
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 2:
                    FTargets.Validate();
                    NewTarget = null;
                    var call__33 = FTargets.IndexOfXYL(FLocation.XYL);
                    I = call__33;
                    if (I == -1)
                    {
                        CurrentTarget = null;
                    }
                    else
                    {
                        var call__34 = FTargets.GetTargets(I);
                        CurrentTarget = call__34;
                    }

                {
                    I = 0;
                    int end__35 = FTargets.Count - 1;
                    for (; I <= end__35; I += 1)
                    {
                        var call__36 = FTargets.GetTargets(I);
                        var call__37 = FTargets.GetTargets(I);
                        var call__38 = FTargets.GetTargets(I);
                        call__38.FDataB = call__36.FData.Priority * 10 + call__37.FMovePoints * -2;
                        var call__39 = FTargets.GetTargets(I);
                        Excess = call__39.FDataA - FStrengthMax;
                        if (Excess > 0)
                        {
                            var call__40 = FTargets.GetTargets(I);
                            call__40.FDataB -= (int)(((uint)Excess) >> 4);
                        }

                        var call__41 = FTargets.GetTargets(I);
                        Excess = call__41.FPathStrength;
                        if (Excess > 0)
                        {
                            var call__42 = FTargets.GetTargets(I);
                            call__42.FDataB -= (int)(((uint)Excess) >> 3);
                        }

                        var call__43 = FTargets.GetTargets(I);
                        if (call__43.FLocation.XYL != FLocation.XYL)
                        {
                            var call__44 = FTargets.GetTargets(I);
                            var call__45 = AoWHSMap.MapContainer.GetFieldXYL(call__44.FLocation.XYL);
                            var call__46 = ((TAoWMapField)call__45).GetArmyHS();
                            Army = call__46;
                            if (Army != null)
                            {
                                var call__47 = Army.FArmy.Count;
                                var call__48 = FTargets.GetTargets(I);
                                call__48.FDataB -= call__47 << 2;
                            }
                        }
                    }
                }

                    FTargets.SortOnPriority();
                    var call__49 = FTargets.Count;
                    bool bool__50 = call__49 != 0;
                    if (bool__50)
                    {
                        var call__51 = FTargets.GetTargets(0);
                        bool__50 = call__51 != CurrentTarget;
                    }

                    if (bool__50)
                    {
                        FTargets.SortOnDataB();
                        bool bool__52 = CurrentTarget == null;
                        if (!bool__52)
                        {
                            var call__53 = FTargets.GetTargets(0);
                            bool__52 = CurrentTarget.FData.Priority < call__53.FData.Priority;
                        }

                        if (bool__52)
                        {
                            var call__54 = FTargets.GetTargets(0);
                            NewTarget = call__54;
                        }
                        else
                        {
                            NewTarget = CurrentTarget;
                        }
                    }

                    if (NewTarget != null && NewTarget != CurrentTarget)
                    {
                        FFortifyLocation = NewTarget.FLocation;
                    }
                    else
                    {
                        if (CurrentTarget != null)
                        {
                            FFortifyLocation = CurrentTarget.FLocation;
                        }
                        else
                        {
                            FFortifyLocation.XYL = 16777215;
                        }
                    }

                    FState += 1;
                    break;
                case 3:
                    if (FFortifyLocation.XYL == 16777215)
                    {
                        var call__55 = GetSurroundingEnemyStrength(FLocation, 70);
                        FEnemyStrength = call__55;
                    }
                    else
                    {
                        var call__56 = GetSurroundingEnemyStrength(FFortifyLocation, 70);
                        FEnemyStrength = call__56;
                    }

                    UpdateRequiredStrength();
                    FState += 1;
                    break;
                case 4:
                    Recruitment = new TAIRecruitmentGroupFinder(null);
                    Recruitment.Setup(this);
                    ActivatePlugin(Recruitment);
                    Recruitment.Release();
                    FState += 1;
                    break;
                case 5:
                    FProductionLocations.Clear();
                    ProductionFinder = new TAIProductionLocationFinder(null);
                    ProductionFinder.Setup(this);
                    ProductionFinder.FSelector.FMovePointsMax = 24;
                    ActivatePlugin(ProductionFinder);
                    ProductionFinder.Release();
                    FState += 1;
                    break;
                case 6:
                    var call__57 = FRecruitmentGroups.Count;
                    if (call__57 != 0 && FRequiredStrength > FStrengthMax)
                    {
                        Recruit = new TAIGroupRecruitControl(null);
                        Recruit.Setup(this);
                        var call__58 = FGroup.FProductions.GetStrengthMax(dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                        Recruit.FRecruitStrength = FRequiredStrength - (call__58 + FStrengthMax);
                        Recruit.FMode = TAIGroupRecruitControlMode.rcmAutoRecruit;
                        ActivatePlugin(Recruit);
                        Recruit.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState = 10;
                    }

                    break;
                case 7:
                    FRecruitmentGroups.Validate(this);
                    bool bool__59 = FStrengthMax < FRequiredStrength;
                    if (bool__59)
                    {
                        var call__60 = FRecruitmentGroups.Count;
                        bool__59 = call__60 != 0;
                    }

                    if (bool__59)
                    {
                        Recruit = new TAIGroupRecruitControl(null);
                        Recruit.Setup(this);
                        var call__61 = FGroup.FProductions.GetStrengthMax(dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                        Recruit.FRecruitStrength = FRequiredStrength - (call__61 + FStrengthMax);
                        Recruit.FMode = TAIGroupRecruitControlMode.rcmForceRecruit;
                        ActivatePlugin(Recruit);
                        Recruit.Release();
                    }

                    FState = 10;
                    break;
                case 10:
                    bool bool__62 = FStrengthMax >= FEnemyStrength;
                    if (bool__62)
                    {
                        var call__63 = FGroup.GetAdjacent();
                        bool__62 = call__63;
                    }

                    if (bool__62)
                    {
                        FTargets.Clear();
                        Finder = new TAIGroupTargetFinder(null);
                        Finder.Setup(this);
                        Finder.FSelector.FTargetInfo.Mode = TAITargetMode.tmAttackArmies;
                        Finder.FSelector.FMovePointsMax = 55;
                        if (FFortifyLocation.XYL == 16777215)
                        {
                            Finder.FSelector.FSourceLocation = FLocation;
                        }
                        else
                        {
                            Finder.FSelector.FSourceLocation = FFortifyLocation;
                        }

                        Finder.FMode = Finder.FMode + TSet<TAIGroupTargetFinderModeElement>.Of(TAIGroupTargetFinderModeElement.tfmCalculateMovepathBFS);
                        Finder.FRange = 12;
                        ActivatePlugin(Finder);
                        Finder.Release();
                        FState += 1;
                    }
                    else
                    {
                        if (FStrengthMax < FEnemyStrength)
                        {
                            FState = 18;
                        }
                        else
                        {
                            FState = 17;
                        }
                    }

                    break;
                case 11:
                    var call__64 = FTargets.Count;
                    if (call__64 >= 1)
                    {
                        var call__65 = FTargets.Count;
                        bool bool__66 = call__65 == 1;
                        if (!bool__66)
                        {
                            var call__67 = FortifyCity();
                            bool__66 = call__67 == null;
                        }

                        bool bool__68 = bool__66;
                        if (!bool__68)
                        {
                            var call__69 = CanBreachWall();
                            bool__68 = call__69;
                        }

                        if (bool__68)
                        {
                            FTargetSelector.Setup(this);
                            FTargetSelector.FPathStrengthFactor = (float)1.5;
                            FTargetSelector.FCurrentPathStrengthFactor = (float)1.3;
                            FTargetSelector.FSuperiority = (float)1.6;
                            FTargetSelector.FNearSuperiority = (float)1.4;
                            FTargetSelector.FMode = FTargetSelector.FMode - TSet<TAIGroupTargetSelectorModeElement>.Of(TAIGroupTargetSelectorModeElement.tsmMultiTargets);
                            ActivatePlugin(FTargetSelector);
                            FState += 1;
                        }
                        else
                        {
                            FState = 18;
                        }
                    }
                    else
                    {
                        FState = 13;
                    }

                    break;
                case 12:
                    if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        FState = 10;
                    }
                    else
                    {
                        if (FFortifyLocation.XYL != 16777215)
                        {
                            SetupME();
                            ExecuteME(FFortifyLocation.X, FFortifyLocation.Y, FFortifyLocation.L);
                        }

                        FState = 20;
                    }

                    break;
                case 13:
                    var call__70 = AoWHSMap.MapContainer.GetFieldXYL(FLocation.XYL);
                    var call__71 = call__70.FindNoneTransparentHS(typeof(TCity));
                    ACity = (TCity)call__71;
                    if (ACity != null && ACity.GetActiveAction() == TCityAction.caMigrate)
                    {
                        Defense = new TAIGroupDefenseControl(null);
                        Defense.FDefendMode = TGroupDefendMode.gdmCustom;
                        Defense.FCustomState = 19;
                        ActivatePlugin(Defense);
                        Defense.Release();
                    }

                    FState += 1;
                    break;
                case 14:
                    FTargets.Clear();
                    Finder = new TAIGroupTargetFinder(null);
                    Finder.Setup(this);
                    Finder.FSelector.FTargetInfo.Mode = TAITargetMode.tmAttack;
                    Finder.FSelector.FMovePointsMax = 55;
                    if (FFortifyLocation.XYL == 16777215)
                    {
                        Finder.FSelector.FSourceLocation = FLocation;
                    }
                    else
                    {
                        Finder.FSelector.FSourceLocation = FFortifyLocation;
                    }

                    Finder.FMode = Finder.FMode + TSet<TAIGroupTargetFinderModeElement>.Of(TAIGroupTargetFinderModeElement.tfmCalculateMovepathBFS);
                    Finder.FRange = 12;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 15:
                    var call__72 = FTargets.Count;
                    if (call__72 >= 1)
                    {
                        FTargetSelector.Setup(this);
                        FTargetSelector.FPathStrengthFactor = (float)1.5;
                        FTargetSelector.FCurrentPathStrengthFactor = (float)1.3;
                        FTargetSelector.FSuperiority = (float)1.6;
                        FTargetSelector.FNearSuperiority = (float)1.4;
                        FTargetSelector.FMode = FTargetSelector.FMode - TSet<TAIGroupTargetSelectorModeElement>.Of(TAIGroupTargetSelectorModeElement.tsmMultiTargets);
                        ActivatePlugin(FTargetSelector);
                        FState += 1;
                    }
                    else
                    {
                        FState = 18;
                    }

                    break;
                case 16:
                    if (FTargetSelector.FTargetStatus == TAIGroupTargetSelectorStatus.tssTargetConquered)
                    {
                        FState = 13;
                    }
                    else
                    {
                        if (FFortifyLocation.XYL != 16777215 && !TSet<TAIGroupTargetSelectorStatus>.Of(TAIGroupTargetSelectorStatus.tssSettingupTarget, TAIGroupTargetSelectorStatus.tssExecutingTarget).Contains(FTargetSelector.FTargetStatus))
                        {
                            SetupME();
                            ExecuteME(FFortifyLocation.X, FFortifyLocation.Y, FFortifyLocation.L);
                        }

                        FState = 20;
                    }

                    break;
                case 17:
                    if (FStrengthMax >= FEnemyStrength)
                    {
                        var call__73 = AoWHSMap.MapContainer.GetFieldXYL(FLocation.XYL);
                        var call__74 = call__73.FindNoneTransparentHS(typeof(TCity));
                        ACity = (TCity)call__74;
                        if (ACity != null && ACity.GetActiveAction() == TCityAction.caMigrate)
                        {
                            Defense = new TAIGroupDefenseControl(null);
                            Defense.FDefendMode = TGroupDefendMode.gdmCustom;
                            Defense.FCustomState = 19;
                            ActivatePlugin(Defense);
                            Defense.Release();
                        }
                    }

                    FState += 1;
                    break;
                case 18:
                    if (FFortifyLocation.XYL == 16777215)
                    {
                        SetupME();
                        ExecuteME(FLocation.X, FLocation.Y, FLocation.L);
                    }
                    else
                    {
                        SetupME();
                        ExecuteME(FFortifyLocation.X, FFortifyLocation.Y, FFortifyLocation.L);
                    }

                    FState = 20;
                    break;
                case 19:
                    SetupME();
                    ExecuteME(FLocation.X, FLocation.Y, FLocation.L);
                    FState = 20;
                    break;
                case 20:
                    if (FortifyCity() != null)
                    {
                        FState = 21;
                    }
                    else
                    {
                        Done();
                    }

                    break;
                case 21:
                    ACity = FortifyCity();
                    if (ACity != null && ACity.FProduction.FStatus == TProductionControlStatus.pcsIdle)
                    {
                        if (ACity.FWallType == 0)
                        {
                            var call__75 = ACity.FortifyCost();
                            var call__76 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                            var call__77 = call__76.FAIBudget.RequestExpense(call__75, 0);
                            if (call__77)
                            {
                                if (ACity.Fortify(false))
                                {
                                    var call__78 = ACity.FortifyCost();
                                    var call__79 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                                    call__79.FAIBudget.ExpenseMade(call__78, 0);
                                }
                            }
                        }
                        else
                        {
                            if (ACity.FWallType == 1 && AoWHSMap.FDay > 29)
                            {
                                var call__80 = ACity.FortifyCost();
                                var call__81 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                                var call__82 = call__81.FAIBudget.RequestExpense(call__80, 0);
                                if (call__82)
                                {
                                    if (ACity.Fortify(false))
                                    {
                                        var call__83 = ACity.FortifyCost();
                                        var call__84 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
                                        call__84.FAIBudget.ExpenseMade(call__83, 0);
                                    }
                                }
                            }
                        }
                    }

                    FState += 1;
                    break;
                case 22:
                    var call__85 = FGroup.FProductions.GetStrengthMax(dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                    if (call__85 + FStrengthMax < FRequiredStrength)
                    {
                        FProductionLocations.Clear();
                        ProductionFinder = new TAIProductionLocationFinder(null);
                        ProductionFinder.Setup(this);
                        ProductionFinder.FSelector.FMovePointsMax = 24;
                        ActivatePlugin(ProductionFinder);
                        ProductionFinder.Release();
                        FState += 1;
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 23:
                    var call__86 = FProductionLocations.Count;
                    if (call__86 > 0)
                    {
                        Produce = new TAIGroupProductionControl(null);
                        Produce.Setup(this);
                        var call__87 = FGroup.FProductions.GetStrengthMax(dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                        Produce.FProduceStrength = FRequiredStrength - (call__87 + FStrengthMax);
                        Produce.FMode = TAIGroupProductionControlMode.pcmAutoProduce;
                        ActivatePlugin(Produce);
                        Produce.Release();
                    }

                    FState += 1;
                    break;
                case 24:
                    switch (FGroup.FTargetType)
                    {
                        case TAIGroupTargetStatus.gtsNone:
                        case TAIGroupTargetStatus.gtsIdle:
                            MakeUselessUnitsPassive();
                            break;
                    }

                    FState += 1;
                    break;
                case 25:
                    Done();
                    break;
            }
        }

        // MainAG.pas:275
        public override bool Split()
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // MainAG.pas:268
        public TCity FortifyCity()
        {
            TCity Result = default;
            if (FFortifyLocation.XYL != 16777215)
            {
                var call__88 = AoWHSMap.MapContainer.GetFieldXYL(FFortifyLocation.XYL);
                var call__89 = call__88.FindNoneTransparentHS(typeof(TCity));
                Result = (TCity)call__89;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // MainAG.pas:287
        public int GetSurroundingEnemyStrength(TXYL Location, int Radius)
        {
            int Result = default;
            TMoveControlSettings Settings = default;
            TMapFieldMsg Msg = default;
            var query = new BattleFieldQuery();
            ref var Info = ref query.Info;
            int I = default;
            Settings = new TMoveControlSettings(null);
            Settings.FPlayer = FGroup.FPlayer;
            Settings.FMode = Settings.FMode + TSet<TMoveModeElement>.Of(TMoveModeElement.mmLogProcessedMapFields, TMoveModeElement.mmAI);
            Settings.SetSourcePos(Location.X, Location.Y, Location.L);
            Settings.FMovePointsMax = Radius;
            Settings.FArmyHS = null;
            Settings.FSelection = 0;
            var call__90 = MoveTypesToInt(TSet<TMoveType>.Of(TMoveType.mtFlying));
            Settings.FMovePointTable = MovePointTables[call__90];
            AoWHSMap.FMoveControl.ProcessMove(Settings);
            SetupBattleFieldInfo(ref Info, FGroup.FPlayer, FGroup.FIndependentRelation, TSet<TBattleFieldInfoRequestsElement>.Of(TBattleFieldInfoRequestsElement.bfrIgnorePassiveIndependents), dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
            Info.FriendlyRelations = TSet<byte>.WithBounds(0, 7);
            Msg.MsgID = 4360;
            Msg.Param[0] = query;
            {
                I = Settings.FProcessedMapFields.Count - 1;
                byte end__91 = 0;
                for (; I >= end__91; I -= 1)
                {
                    var mapField = (TAoWMapField)Settings.FProcessedMapFields[I];
                    if ((mapField.FFieldFlags & 67108864) != 0)
                    {
                        mapField.SendMapFieldMsg(ref Msg);
                    }
                }
            }

            Result = Info.Strength;
            Settings.Release();
            return Result;
        }

        // MainAG.pas:315
        public void UpdateRequiredStrength()
        {
            int Strength = default;
            TPlayerStatistics Statistics = default;
            FRequiredStrength = FEnemyStrength;
            if (AoWHSMap.FDay > 5)
            {
                Strength = AoWHSMap.FDay / 5 * 5000;
                if (Strength > 50000)
                {
                    Strength = 50000;
                }

                if (Strength > FEnemyStrength)
                {
                    FRequiredStrength = Strength;
                }
            }

            if (FRequiredStrength > 0 && AoWHSMap.FDay < 10)
            {
                FRequiredStrength = Round((double)(FRequiredStrength * AoWHSMap.FDay) * 0.1);
            }

            var call__93 = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
            Statistics = call__93.FStatistics;
            var call__94 = Statistics.FTurnInfo.Count;
            if (call__94 > 0)
            {
                var call__95 = Statistics.FTurnInfo.Count;
                var call__96 = Statistics.FTurnInfo.GetTurnInfo(call__95 - 1);
                Strength = (int)(((uint)call__96.FArmyStrength) >> 1);
                if (Strength < FRequiredStrength)
                {
                    FRequiredStrength = Strength;
                }

                var call__97 = Statistics.FTurnInfo.Count;
                var call__98 = Statistics.FTurnInfo.GetTurnInfo(call__97 - 1);
                Strength = Round((double)call__98.FArmyStrength * 0.1);
                if (Strength > 50000)
                {
                    Strength = 50000;
                }

                if (AoWHSMap.FDay < 10)
                {
                    Strength = Round((double)(Strength * AoWHSMap.FDay) * 0.1);
                }

                if (Strength > FRequiredStrength)
                {
                    FRequiredStrength = Strength;
                }
            }
        }

        // MainAG.pas:352
        public void MakeUselessUnitsPassive()
        {
            int Excess = default;
            int I = default;
            TUnitUserList Units = default;
            TUnitUserList Passive = default;
            TAIGroup Group = default;
            Excess = FStrengthMax - FRequiredStrength;
            if (Excess > 0)
            {
                Units = new TUnitUserList(null);
                Passive = new TUnitUserList(null);
                Units.AddUnits(FGroup);
                Units.FChildren.Sort(MainAG.CompareFortifyUnits);
                var call__99 = Units.Count;
                I = call__99;
                while (I != 0 && Excess > 0)
                {
                    I -= 1;
                    if (!(Units.GetUnit(I) is TLeader))
                    {
                        var call__100 = Units.GetUnit(I);
                        var call__101 = call__100.GetStrengthMax(dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                        Excess -= call__101;
                        if (Excess >= 0)
                        {
                            var call__102 = Units.GetUnit(I);
                            Passive.AddUnit(call__102);
                            var call__103 = Units.GetUnit(I);
                            Units.DeleteUnit(call__103);
                        }
                    }
                }

                var call__104 = Passive.Count;
                if (call__104 != 0)
                {
                    var call__105 = AoWHSMap.FAIGroupManager.CreateGroup(FGroup.FPlayer, FGroup.FIndependentRelation, TGroupBehavior.gbPassive);
                    Group = call__105;
                    Group.AddUnits(Passive);
                    Group.Release();
                }

                Passive.Free();
                Units.Free();
            }
        }

        // MainAG.pas:391
        public bool CanBreachWall()
        {
            bool Result = default;
            int I = default;
            TAbstractUnit AUnit = default;
            var call__106 = FGroup.Count;
            I = call__106;
            var call__107 = FGroup.GetStrength(TDamageType.dtWall);
            Result = call__107 != 0;
            while (!Result && I != 0)
            {
                I -= 1;
                var call__108 = FGroup.GetUnit(I);
                AUnit = call__108;
                var call__109 = AUnit.GetAbilityEnabled(52);
                bool bool__110 = call__109;
                if (bool__110)
                {
                    var call__111 = AoWHSSet.FAbilityControl.GetAbility(52);
                    var call__112 = ((TSpellCastingAbility)call__111).CanCastBreachWallSpell(AUnit);
                    bool__110 = call__112;
                }

                Result = bool__110;
            }

            return Result;
        }

    }

    public partial class TSuicidalAG : TAIGroup, IAIGroupClass
    {
        // MainAG.pas:729
        public new const int ClassID = 136276;

        // MainAG.pas:722
        public TSuicidalAG(TECustomNode owner) : base(owner)
        {
            FProcessPriority = 1;
            FRecruitmentPriority = 2;
        }

        // MainAG.pas:735
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TSuicidalAGC>();
            return Result;
        }

        // MainAG.pas:732
        public static new TGroupBehavior Behavior => TGroupBehavior.gbSuicidal;

    }

    public partial class TSuicidalAGC : TAIGroupControl
    {
        public TSuicidalAGC(TECustomNode owner) : base(owner)
        { }

        // MainAG.pas:750
        public override void MoveExecuterDone(TObject Sender)
        {
            switch (FState)
            {
                case 1:
                    if (FMoveExecuter.FStatus == TAIMoveExecuterStatus.mesOutOfMovePoints)
                    {
                        Done();
                    }
                    else
                    {
                        if (FMoveExecuter.FStatus == TAIMoveExecuterStatus.mesInvalidated)
                        {
                            Done();
                        }
                    }

                    break;
            }
        }

        // MainAG.pas:738
        public override void TargetFoundEvent(TAIGroupControlPlugin Sender, TAIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            var call__113 = FCombatPredictor.Execute();
            if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crDefeat, TCombatResult.crNoDefenders).Contains(call__113))
            {
                var call__114 = FCombatPredictor.GetEfficiency();
                if (call__114 > 0)
                {
                    var call__115 = FCombatPredictor.GetEfficiency();
                    Target.FData.Priority = call__115 + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                }
            }
        }

        // MainAG.pas:759
        public override void ProcessStates()
        {
            TAIGroupTargetFinder Finder = default;
            switch (FState)
            {
                case 0:
                    Finder = new TAIGroupTargetFinder(null);
                    Finder.Setup(this);
                    Finder.FSelector.FMovePointsMax = 100;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 1:
                    FTargets.Validate();
                    var call__116 = FTargets.Count;
                    if (call__116 != 0)
                    {
                        FTargets.SortOnPriority();
                        SetupME();
                        var call__117 = FTargets.GetTargets(0);
                        var call__118 = FTargets.GetTargets(0);
                        var call__119 = FTargets.GetTargets(0);
                        ExecuteME(call__119.FLocation.X, call__118.FLocation.Y, call__117.FLocation.L);
                        FTargets.Delete(0);
                    }
                    else
                    {
                        Done();
                    }

                    break;
            }
        }
    }

    public partial class TTransportAG : TAIGroup, IAIGroupClass
    {
        // MainAG.pas:801
        public new const int ClassID = 136295;

        // MainAG.pas:796
        public override void Destroy()
        {
            base.Destroy();
        }

        // MainAG.pas:789
        public TTransportAG(TECustomNode owner) : base(owner)
        {
            FProcessPriority = 2;
            FRecruitmentPriority = 20;
        }

        // MainAG.pas:807
        public override EClass GroupControlClass()
        {
            EClass Result = default;
            Result = EClass.Of<TTransportAGC>();
            return Result;
        }

        // MainAG.pas:804
        public static new TGroupBehavior Behavior => TGroupBehavior.gbTransport;

    }

    public partial class TTransportAGC : TAIGroupControl
    {
        public TTransportAGC(TECustomNode owner) : base(owner)
        { }

        // MainAG.pas:822
        public override void MoveExecuterDone(TObject Sender)
        {
            switch (FState)
            {
                case 1:
                    if (FMoveExecuter.FStatus == TAIMoveExecuterStatus.mesOutOfMovePoints)
                    {
                        Done();
                    }
                    else
                    {
                        if (FMoveExecuter.FStatus == TAIMoveExecuterStatus.mesInvalidated)
                        {
                            Done();
                        }
                    }

                    break;
            }
        }

        // MainAG.pas:810
        public override void TargetFoundEvent(TAIGroupControlPlugin Sender, TAIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            var call__120 = FCombatPredictor.Execute();
            if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crDefeat, TCombatResult.crNoDefenders).Contains(call__120))
            {
                var call__121 = FCombatPredictor.GetEfficiency();
                if (call__121 > 75)
                {
                    var call__122 = FCombatPredictor.GetEfficiency();
                    Target.FData.Priority = call__122 + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                }
            }
        }

        // MainAG.pas:849
        public override void ProcessStates()
        {
            TAIGroupTargetFinder Finder = default;
            TAIGroupWanderControl Wander = default;
            switch (FState)
            {
                case 0:
                    Finder = new TAIGroupTargetFinder(null);
                    Finder.Setup(this);
                    Finder.FSelector.FMovePointsMax = 100;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 1:
                    FTargets.Validate();
                    var call__123 = FTargets.Count;
                    if (call__123 != 0)
                    {
                        FTargets.SortOnPriority();
                        SetupME();
                        var call__124 = FTargets.GetTargets(0);
                        var call__125 = FTargets.GetTargets(0);
                        var call__126 = FTargets.GetTargets(0);
                        ExecuteME(call__126.FLocation.X, call__125.FLocation.Y, call__124.FLocation.L);
                        FTargets.Delete(0);
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 2:
                    Wander = new TAIGroupWanderControl(null);
                    Wander.Setup(this);
                    Wander.FMovePointsMax = 125;
                    ActivatePlugin(Wander);
                    Wander.Release();
                    FState += 1;
                    break;
                case 3:
                    Done();
                    break;
            }
        }

        // MainAG.pas:831
        public override void SetupME()
        {
            TAbstractUnit Transporter = default;
            TArmy Army = default;
            int I = default;
            base.SetupME();
            Transporter = FGroup.FLeader;
            if (Transporter != null && Transporter.Owner is TArmy)
            {
                Army = (TArmy)Transporter.Owner;
                {
                    I = 0;
                    int end__127 = Army.Count - 1;
                    for (; I <= end__127; I += 1)
                    {
                        var call__128 = Army.GetUnit(I);
                        bool bool__129 = Transporter != call__128;
                        if (bool__129)
                        {
                            var call__130 = Army.GetUnit(I);
                            var call__131 = FMoveExecuter.FUnits.IndexOfUnit(call__130);
                            bool__129 = call__131 == -1;
                        }

                        if (bool__129)
                        {
                            var call__132 = Army.GetUnit(I);
                            FMoveExecuter.AddUnit(call__132);
                        }
                    }
                }
            }
        }
    }

    // no fields. map-load registration callback
    public partial class TRegistrator : TObject
    {
        // MainAG.pas:115
        public static void MapLoaded(TObject Sender)
        {
            if (Sender == AoWHSMap)
            {
                AoWHSMap.FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<TPassiveAG>(), AIGroupClass.Of<TSuicidalAG>(), AIGroupClass.Of<TFortifyAG>(), AIGroupClass.Of<TTransportAG>()]);
            }
        }

    }

    public static partial class MainAG
    {
        // MainAG.pas:341
        public static int CompareFortifyUnits(object Item1, object Item2)
        {
            int Result = default;
            int Strength1 = default;
            int Strength2 = default;
            Strength1 = ((TAbstractUnit)Item1).GetStrengthMax(dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
            Strength2 = ((TAbstractUnit)Item2).GetStrengthMax(dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
            if ((TObject)Item1 is TLeader)
            {
                // { Original uses Strength2. }
                Strength1 = Strength2 + 16777215;
            }

            if ((TObject)Item2 is TLeader)
            {
                Strength2 += 16777215;
            }

            Result = Strength2 - Strength1;
            return Result;
        }

        public static void Initialize()
        {
            if (ValidateAoWEngine())
            {
                AoWEngine.RegisterEClasses([EClass.Of<TSuicidalAG>(), EClass.Of<TFortifyAG>(), EClass.Of<TPassiveAG>(), EClass.Of<TTransportAG>()]);
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
