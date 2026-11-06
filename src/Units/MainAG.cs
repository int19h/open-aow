// Translated from MainAG.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngineUnit;
using Aow.Units.City;
using Aow.Units.AGCPlugins;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.MainAG;

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
        TXYL Location = default;
        if (FTargetMode == (byte)TGroupDefendMode.gdmCustom)
        {
            Location = FTargetXYL;
        }
        else
        {
            Location.XYL = base.GetLoc();
        }

        return Location.XYL;
    }

    // MainAG.pas:147
    public override bool GetRecruitableUnits(int Phase, TUnitList Units, TUnitList Recruitable)
    {
        TXYL Location = default;
        TGroupDefenseSelector Selector;
        TAbstractUnit AUnit;
        Location.XYL = GetLoc();
        bool Result = false;
        Selector = new TGroupDefenseSelector();
        Selector.Execute(this, Location.X, Location.Y, Location.L, Units);
        if (Selector.FDefenders.Count != 0)
        {
            for (int I = Units.Count - 1; I >= 0; I--)
            {
                AUnit = Units.GetUnit(I);
                if (Selector.FDefenders.IndexOf(AUnit) == -1)
                {
                    Recruitable.AddUnit(AUnit);
                    Result = true;
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
    public override EClass GroupControlClass() => EClass.Of<TPassiveAGC>();

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
        TGroupDefenseSelector Selector;
        TAIGroup Group;
        TAbstractUnit AUnit;
        switch (FState)
        {
            case 0:
                CombinePassiveGroups();
                Location.XYL = FGroup.GetLoc();
                Selector = new TGroupDefenseSelector();
                Selector.Execute(FGroup, Location.X, Location.Y, Location.L, null);
                if (Selector.FDefenders.Count != FGroup.Count && FGroup.FPlayer != 0)
                {
                    var randomGroupBehavior = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer).FAIControl.GetRandomGroupBehavior();
                    Group = AoWHSMap.FAIGroupManager.CreateGroup(FGroup.FPlayer, FGroup.FIndependentRelation, randomGroupBehavior);
                    for (int I = FGroup.Count - 1; I >= 0; I--)
                    {
                        AUnit = FGroup.GetUnit(I);
                        if (Selector.FDefenders.IndexOf(AUnit) == -1)
                        {
                            Group.AddUnit(AUnit);
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
        TPlayer Player;
        TXYL Location = default;
        TXYL OtherLocation = default;
        TAIGroup Group;
        Player = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer);
        Location.XYL = FGroup.GetLoc();
        for (int I = Player.FAIGroups.Count - 1; I >= 0; I--)
        {
            Group = Player.FAIGroups.GetGroups(I);
            if (FGroup.FGroupID != Group.FGroupID && Group.EClass.ClassID == 136280 && FGroup.FIndependentRelation == Group.FIndependentRelation)
            {
                OtherLocation.XYL = Group.GetLoc();
                if (Location.L == OtherLocation.L && dHXtoRad(Location.X, Location.Y, OtherLocation.X, OtherLocation.Y) <= 1)
                {
                    FGroup.AddUnits(Group);
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
    public override EClass GroupControlClass() => EClass.Of<TFortifyAGC>();

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
        FTargetSelector = new TAIGroupTargetSelector(null);
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
        TAIGroupTargetFinder Finder;
        TAIRecruitmentGroupFinder Recruitment;
        TAIProductionLocationFinder ProductionFinder;
        TAIGroupRecruitControl Recruit;
        TAIGroupProductionControl Produce;
        TAIGroupDefenseControl Defense;
        TAIGroupControlTarget CurrentTarget;
        TAIGroupControlTarget NewTarget;
        int I;
        int Excess;
        TArmyHS Army;
        TCity ACity;
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
                while (I < FTargets.Count && FTargets.GetTargets(I).FStatus.Contains(TAIGroupControlTargetStatusElement.gtsDone))
                {
                    I += 1;
                }

                if (I < FTargets.Count)
                {
                    var surroundingEnemyStrength = GetSurroundingEnemyStrength(FTargets.GetTargets(I).FLocation, 70);
                    FTargets.GetTargets(I).FDataA = surroundingEnemyStrength;
                    var target = FTargets.GetTargets(I);
                    FTargets.GetTargets(I).FStatus = target.FStatus + TSet<TAIGroupControlTargetStatusElement>.Of(TAIGroupControlTargetStatusElement.gtsDone);
                }
                else
                {
                    FState += 1;
                }

                break;
            case 2:
                FTargets.Validate();
                NewTarget = null;
                I = FTargets.IndexOfXYL(FLocation.XYL);
                if (I == -1)
                {
                    CurrentTarget = null;
                }
                else
                {
                    CurrentTarget = FTargets.GetTargets(I);
                }

            {
                I = 0;
                int last = FTargets.Count - 1;
                for (; I <= last; I += 1)
                {
                    var targetData = FTargets.GetTargets(I);
                    var targetMovePoints = FTargets.GetTargets(I);
                    FTargets.GetTargets(I).FDataB = targetData.FData.Priority * 10 + targetMovePoints.FMovePoints * -2;
                    Excess = FTargets.GetTargets(I).FDataA - FStrengthMax;
                    if (Excess > 0)
                    {
                        var target = FTargets.GetTargets(I);
                        target.FDataB -= (int)(((uint)Excess) >> 4);
                    }

                    Excess = FTargets.GetTargets(I).FPathStrength;
                    if (Excess > 0)
                    {
                        var target = FTargets.GetTargets(I);
                        target.FDataB -= (int)(((uint)Excess) >> 3);
                    }

                    if (FTargets.GetTargets(I).FLocation.XYL != FLocation.XYL)
                    {
                        var targetLocation = FTargets.GetTargets(I);
                        Army = ((TAoWMapField)AoWHSMap.MapContainer.GetFieldXYL(targetLocation.FLocation.XYL)).GetArmyHS();
                        if (Army != null)
                        {
                            var count = Army.FArmy.Count;
                            var target = FTargets.GetTargets(I);
                            target.FDataB -= count << 2;
                        }
                    }
                }
            }

                FTargets.SortOnPriority();
                if (FTargets.Count != 0 && FTargets.GetTargets(0) != CurrentTarget)
                {
                    FTargets.SortOnDataB();
                    bool condition = CurrentTarget == null;
                    if (!condition)
                    {
                        var target = FTargets.GetTargets(0);
                        condition = CurrentTarget.FData.Priority < target.FData.Priority;
                    }

                    if (condition)
                    {
                        NewTarget = FTargets.GetTargets(0);
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
                    FEnemyStrength = GetSurroundingEnemyStrength(FLocation, 70);
                }
                else
                {
                    FEnemyStrength = GetSurroundingEnemyStrength(FFortifyLocation, 70);
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
                if (FRecruitmentGroups.Count != 0 && FRequiredStrength > FStrengthMax)
                {
                    Recruit = new TAIGroupRecruitControl(null);
                    Recruit.Setup(this);
                    var strengthMax = FGroup.FProductions.GetStrengthMax(dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                    Recruit.FRecruitStrength = FRequiredStrength - (strengthMax + FStrengthMax);
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
                if (FStrengthMax < FRequiredStrength && FRecruitmentGroups.Count != 0)
                {
                    Recruit = new TAIGroupRecruitControl(null);
                    Recruit.Setup(this);
                    var strengthMax = FGroup.FProductions.GetStrengthMax(dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                    Recruit.FRecruitStrength = FRequiredStrength - (strengthMax + FStrengthMax);
                    Recruit.FMode = TAIGroupRecruitControlMode.rcmForceRecruit;
                    ActivatePlugin(Recruit);
                    Recruit.Release();
                }

                FState = 10;
                break;
            case 10:
                if (FStrengthMax >= FEnemyStrength && FGroup.GetAdjacent())
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
                if (FTargets.Count >= 1)
                {
                    if (FTargets.Count == 1 || FortifyCity() == null || CanBreachWall())
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
                ACity = (TCity)AoWHSMap.MapContainer.GetFieldXYL(FLocation.XYL).FindNoneTransparentHS(typeof(TCity));
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
                if (FTargets.Count >= 1)
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
                    ACity = (TCity)AoWHSMap.MapContainer.GetFieldXYL(FLocation.XYL).FindNoneTransparentHS(typeof(TCity));
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
                        var fortifyCost = ACity.FortifyCost();
                        if (AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer).FAIBudget.RequestExpense(fortifyCost, 0))
                        {
                            if (ACity.Fortify(false))
                            {
                                var aCityFortifyCost = ACity.FortifyCost();
                                AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer).FAIBudget.ExpenseMade(aCityFortifyCost, 0);
                            }
                        }
                    }
                    else
                    {
                        if (ACity.FWallType == 1 && AoWHSMap.FDay > 29)
                        {
                            var fortifyCost = ACity.FortifyCost();
                            if (AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer).FAIBudget.RequestExpense(fortifyCost, 0))
                            {
                                if (ACity.Fortify(false))
                                {
                                    var aCityFortifyCost = ACity.FortifyCost();
                                    AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer).FAIBudget.ExpenseMade(aCityFortifyCost, 0);
                                }
                            }
                        }
                    }
                }

                FState += 1;
                break;
            case 22:
                if (FGroup.FProductions.GetStrengthMax(dtAll - TSet<TDamageType>.Of(TDamageType.dtWall)) + FStrengthMax < FRequiredStrength)
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
                if (FProductionLocations.Count > 0)
                {
                    Produce = new TAIGroupProductionControl(null);
                    Produce.Setup(this);
                    var strengthMax = FGroup.FProductions.GetStrengthMax(dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                    Produce.FProduceStrength = FRequiredStrength - (strengthMax + FStrengthMax);
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
    public override bool Split() => false;

    // MainAG.pas:268
    public TCity FortifyCity()
    {
        TCity Result;
        if (FFortifyLocation.XYL != 16777215)
        {
            Result = (TCity)AoWHSMap.MapContainer.GetFieldXYL(FFortifyLocation.XYL).FindNoneTransparentHS(typeof(TCity));
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
        TMoveControlSettings Settings;
        TMapFieldMsg Msg = default;
        var query = new BattleFieldQuery();
        ref var Info = ref query.Info;
        Settings = new TMoveControlSettings(null);
        Settings.FPlayer = FGroup.FPlayer;
        Settings.FMode = Settings.FMode + TSet<TMoveModeElement>.Of(TMoveModeElement.mmLogProcessedMapFields, TMoveModeElement.mmAI);
        Settings.SetSourcePos(Location.X, Location.Y, Location.L);
        Settings.FMovePointsMax = Radius;
        Settings.FArmyHS = null;
        Settings.FSelection = 0;
        var moveTypesToInt = MoveTypesToInt(TSet<TMoveType>.Of(TMoveType.mtFlying));
        Settings.FMovePointTable = MovePointTables[moveTypesToInt];
        AoWHSMap.FMoveControl.ProcessMove(Settings);
        SetupBattleFieldInfo(ref Info, FGroup.FPlayer, FGroup.FIndependentRelation, TSet<TBattleFieldInfoRequestsElement>.Of(TBattleFieldInfoRequestsElement.bfrIgnorePassiveIndependents), dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
        Info.FriendlyRelations = TSet<byte>.WithBounds(0, 7);
        Msg.MsgID = 4360;
        Msg.Param[0] = query;
        for (int I = Settings.FProcessedMapFields.Count - 1; I >= 0; I--)
        {
            var mapField = (TAoWMapField)Settings.FProcessedMapFields[I];
            if ((mapField.FFieldFlags & 67108864) != 0)
            {
                mapField.SendMapFieldMsg(ref Msg);
            }
        }

        int Result = Info.Strength;
        Settings.Release();
        return Result;
    }

    // MainAG.pas:315
    public void UpdateRequiredStrength()
    {
        int Strength;
        TPlayerStatistics Statistics;
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

        Statistics = AoWHSMap.FPlayers.GetPlayers(FGroup.FPlayer).FStatistics;
        if (Statistics.FTurnInfo.Count > 0)
        {
            var count = Statistics.FTurnInfo.Count;
            Strength = (int)(((uint)Statistics.FTurnInfo.GetTurnInfo(count - 1).FArmyStrength) >> 1);
            if (Strength < FRequiredStrength)
            {
                FRequiredStrength = Strength;
            }

            var fTurnInfoCount = Statistics.FTurnInfo.Count;
            Strength = Round((double)Statistics.FTurnInfo.GetTurnInfo(fTurnInfoCount - 1).FArmyStrength * 0.1);
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
            Units.FChildren.Sort(MainAGUnit.CompareFortifyUnits);
            I = Units.Count;
            while (I != 0 && Excess > 0)
            {
                I -= 1;
                if (!(Units.GetUnit(I) is TLeader))
                {
                    Excess -= Units.GetUnit(I).GetStrengthMax(dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                    if (Excess >= 0)
                    {
                        Passive.AddUnit(Units.GetUnit(I));
                        Units.DeleteUnit(Units.GetUnit(I));
                    }
                }
            }

            if (Passive.Count != 0)
            {
                Group = AoWHSMap.FAIGroupManager.CreateGroup(FGroup.FPlayer, FGroup.FIndependentRelation, TGroupBehavior.gbPassive);
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
        int I;
        TAbstractUnit AUnit;
        I = FGroup.Count;
        bool Result = FGroup.GetStrength(TDamageType.dtWall) != 0;
        while (!Result && I != 0)
        {
            I -= 1;
            AUnit = FGroup.GetUnit(I);
            Result = AUnit.GetAbilityEnabled(52) && ((TSpellCastingAbility)AoWHSSet.FAbilityControl.GetAbility(52)).CanCastBreachWallSpell(AUnit);
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
    public override EClass GroupControlClass() => EClass.Of<TSuicidalAGC>();

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
        var execute = FCombatPredictor.Execute();
        if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crDefeat, TCombatResult.crNoDefenders).Contains(execute))
        {
            if (FCombatPredictor.GetEfficiency() > 0)
            {
                var efficiency = FCombatPredictor.GetEfficiency();
                Target.FData.Priority = efficiency + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                base.TargetFoundEvent(Sender, Target, ref SearchDone);
            }
        }
    }

    // MainAG.pas:759
    public override void ProcessStates()
    {
        TAIGroupTargetFinder Finder;
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
                if (FTargets.Count != 0)
                {
                    FTargets.SortOnPriority();
                    SetupME();
                    var targetLocation = FTargets.GetTargets(0);
                    var target = FTargets.GetTargets(0);
                    ExecuteME(FTargets.GetTargets(0).FLocation.X, target.FLocation.Y, targetLocation.FLocation.L);
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
    public override EClass GroupControlClass() => EClass.Of<TTransportAGC>();

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
        var execute = FCombatPredictor.Execute();
        if (TSet<TCombatResult>.Of(TCombatResult.crVictory, TCombatResult.crDefeat, TCombatResult.crNoDefenders).Contains(execute))
        {
            if (FCombatPredictor.GetEfficiency() > 75)
            {
                var efficiency = FCombatPredictor.GetEfficiency();
                Target.FData.Priority = efficiency + (Target.FData.Priority * 100 - Target.FMovePoints * 5);
                base.TargetFoundEvent(Sender, Target, ref SearchDone);
            }
        }
    }

    // MainAG.pas:849
    public override void ProcessStates()
    {
        TAIGroupTargetFinder Finder;
        TAIGroupWanderControl Wander;
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
                if (FTargets.Count != 0)
                {
                    FTargets.SortOnPriority();
                    SetupME();
                    var targetLocation = FTargets.GetTargets(0);
                    var target = FTargets.GetTargets(0);
                    ExecuteME(FTargets.GetTargets(0).FLocation.X, target.FLocation.Y, targetLocation.FLocation.L);
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
        TAbstractUnit Transporter;
        TArmy Army;
        base.SetupME();
        Transporter = FGroup.FLeader;
        if (Transporter != null && Transporter.Owner is TArmy)
        {
            Army = (TArmy)Transporter.Owner;
            for (int I = 0, last = Army.Count - 1; I <= last; I++)
            {
                bool condition = Transporter != Army.GetUnit(I);
                if (condition)
                {
                    var unit = Army.GetUnit(I);
                    condition = FMoveExecuter.FUnits.IndexOfUnit(unit) == -1;
                }

                if (condition)
                {
                    var unit = Army.GetUnit(I);
                    FMoveExecuter.AddUnit(unit);
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

public static partial class MainAGUnit
{
    // MainAG.pas:341
    public static int CompareFortifyUnits(object Item1, object Item2)
    {
        int Strength1;
        int Strength2;
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

        return Strength2 - Strength1;
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
