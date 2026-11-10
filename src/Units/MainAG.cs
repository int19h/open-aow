// Translated from MainAG.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai.Groups
{

    public partial class PassiveAG : AIGroup, IAIGroupClass
    {
        // MainAG.pas:129
        public new const int ClassID = 136280;

        // MainAG.pas:122
        public PassiveAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 0;
            FRecruitmentPriority = 1;
        }

        // MainAG.pas:138
        public override int GetLoc()
        {
            XYL Location = default;
            if (FTargetMode == (byte)AIGroupDefenseControl.GroupDefendMode.Custom)
            {
                Location = FTargetXYL;
            }
            else
            {
                Location.Packed = base.GetLoc();
            }

            return Location.Packed;
        }

        // MainAG.pas:147
        public override bool GetRecruitableUnits(AIGroup Recruiter, UnitList Units, UnitList Recruitable)
        {
            XYL Location = default;
            GroupDefenseSelector Selector;
            AbstractUnit AUnit;
            Location.Packed = GetLoc();
            bool Result = false;
            Selector = new GroupDefenseSelector();
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

            Selector?.Dispose();
            return Result;
        }

        // MainAG.pas:135
        public override EClass GroupControlClass() => EClass.Of<PassiveAGC>();

        // MainAG.pas:132
        public static new GroupBehavior Behavior => GroupBehavior.Passive;

    }

    public partial class PassiveAGC : AIGroupControl
    {
        public PassiveAGC(ECustomNode owner) : base(owner)
        { }

        // MainAG.pas:199
        public override void ProcessStates()
        {
            XYL Location = default;
            GroupDefenseSelector Selector;
            AIGroup Group;
            AbstractUnit AUnit;
            switch (FState)
            {
                case 0:
                    CombinePassiveGroups();
                    Location.Packed = FGroup.GetLoc();
                    Selector = new GroupDefenseSelector();
                    Selector.Execute(FGroup, Location.X, Location.Y, Location.L, null);
                    if (Selector.FDefenders.Count != FGroup.Count && FGroup.FPlayer != 0)
                    {
                        var randomGroupBehavior = AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer).FAIControl.GetRandomGroupBehavior();
                        Group = AoWHSMap.Current.FAIGroupManager.CreateGroup(FGroup.FPlayer, FGroup.FIndependentRelation, randomGroupBehavior);
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

                    if (Selector.FDefenseLocation.Packed != 16777215)
                    {
                        FGroup.FTargetXYL = Selector.FDefenseLocation;
                        FGroup.FTargetMode = (byte)AIGroupDefenseControl.GroupDefendMode.Custom;
                        SetupME();
                        FMoveExecuter.FMode = FMoveExecuter.FMode - Set<AIMoveExecuterModeElement>.Of(AIMoveExecuterModeElement.Adjacent);
                        ExecuteME(Selector.FDefenseLocation.X, Selector.FDefenseLocation.Y, Selector.FDefenseLocation.L);
                        Done();
                    }
                    else
                    {
                        Done();
                    }

                    Selector?.Dispose();
                    break;
            }
        }

        // MainAG.pas:176
        public void CombinePassiveGroups()
        {
            Player Player;
            XYL Location = default;
            XYL OtherLocation = default;
            AIGroup Group;
            Player = AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer);
            Location.Packed = FGroup.GetLoc();
            for (int I = Player.FAIGroups.Count - 1; I >= 0; I--)
            {
                Group = Player.FAIGroups.GetGroups(I);
                if (FGroup.FGroupID != Group.FGroupID && Group.EClass.ClassID == 136280 && FGroup.FIndependentRelation == Group.FIndependentRelation)
                {
                    OtherLocation.Packed = Group.GetLoc();
                    if (Location.L == OtherLocation.L && HexGeometry.dHXtoRad(Location.X, Location.Y, OtherLocation.X, OtherLocation.Y) <= 1)
                    {
                        FGroup.AddUnits(Group);
                    }
                }
            }
        }
    }

    public partial class FortifyAG : AIGroup, IAIGroupClass
    {
        // MainAG.pas:247
        public new const int ClassID = 136281;

        // MainAG.pas:240
        public FortifyAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 15;
            FRecruitmentPriority = 0;
        }

        // MainAG.pas:253
        public override EClass GroupControlClass() => EClass.Of<FortifyAGC>();

        // MainAG.pas:250
        public static new GroupBehavior Behavior => GroupBehavior.Fortify;

    }

    // city location, enemy strength, required strength, target selector
    public partial class FortifyAGC : AIGroupControl
    {
        public XYL FFortifyLocation;
        public int FEnemyStrength;
        public int FRequiredStrength;
        public AIGroupTargetSelector FTargetSelector;

        // MainAG.pas:262
        public override void Dispose()
        {
            FTargetSelector.Release();
            base.Dispose();
        }

        // MainAG.pas:256
        public FortifyAGC(ECustomNode owner) : base(owner)
        {
            FTargetSelector = new AIGroupTargetSelector(null);
        }

        // MainAG.pas:278
        public override void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
        {
            switch (Sender.FSelector.FTargetInfo.Mode)
            {
                case AITargetMode.Fortify:
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                    break;
                case AITargetMode.AttackArmies:
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                    break;
                case AITargetMode.Attack:
                    base.TargetFoundEvent(Sender, Target, ref SearchDone);
                    break;
            }
        }

        // MainAG.pas:407
        public override void ProcessStates()
        {
            AIGroupTargetFinder Finder;
            AIRecruitmentGroupFinder Recruitment;
            AIProductionLocationFinder ProductionFinder;
            AIGroupRecruitControl Recruit;
            AIGroupProductionControl Produce;
            AIGroupDefenseControl Defense;
            AIGroupControlTarget CurrentTarget;
            AIGroupControlTarget NewTarget;
            int I;
            int Excess;
            ArmyHS Army;
            City ACity;
            switch (FState)
            {
                case 0:
                    Finder = new AIGroupTargetFinder(null);
                    Finder.Setup(this);
                    Finder.FSelector.FTargetInfo.Mode = AITargetMode.Fortify;
                    Finder.FMode = Finder.FMode + Set<AIGroupTargetFinderModeElement>.Of(AIGroupTargetFinderModeElement.CalculateMovepathBFS);
                    Finder.FRange = 12;
                    ActivatePlugin(Finder);
                    Finder.Release();
                    FState += 1;
                    break;
                case 1:
                    I = 0;
                    while (I < FTargets.Count && FTargets.GetTargets(I).FStatus.Contains(AIGroupControlTargetStatusElement.Done))
                    {
                        I += 1;
                    }

                    if (I < FTargets.Count)
                    {
                        var surroundingEnemyStrength = GetSurroundingEnemyStrength(FTargets.GetTargets(I).FLocation, 70);
                        FTargets.GetTargets(I).FDataA = surroundingEnemyStrength;
                        var target = FTargets.GetTargets(I);
                        FTargets.GetTargets(I).FStatus = target.FStatus + Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Done);
                    }
                    else
                    {
                        FState += 1;
                    }

                    break;
                case 2:
                    FTargets.Validate();
                    NewTarget = null;
                    I = FTargets.IndexOfXYL(FLocation.Packed);
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

                            if (FTargets.GetTargets(I).FLocation.Packed != FLocation.Packed)
                            {
                                var targetLocation = FTargets.GetTargets(I);
                                Army = ((AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(targetLocation.FLocation.Packed)).GetArmyHS();
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
                            FFortifyLocation.Packed = 16777215;
                        }
                    }

                    FState += 1;
                    break;
                case 3:
                    if (FFortifyLocation.Packed == 16777215)
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
                    Recruitment = new AIRecruitmentGroupFinder(null);
                    Recruitment.Setup(this);
                    ActivatePlugin(Recruitment);
                    Recruitment.Release();
                    FState += 1;
                    break;
                case 5:
                    FProductionLocations.Clear();
                    ProductionFinder = new AIProductionLocationFinder(null);
                    ProductionFinder.Setup(this);
                    ProductionFinder.FSelector.FMovePointsMax = 24;
                    ActivatePlugin(ProductionFinder);
                    ProductionFinder.Release();
                    FState += 1;
                    break;
                case 6:
                    if (FRecruitmentGroups.Count != 0 && FRequiredStrength > FStrengthMax)
                    {
                        Recruit = new AIGroupRecruitControl(null);
                        Recruit.Setup(this);
                        var strengthMax = FGroup.FProductions.GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall));
                        Recruit.FRecruitStrength = FRequiredStrength - (strengthMax + FStrengthMax);
                        Recruit.FMode = AIGroupRecruitControl.Mode.AutoRecruit;
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
                        Recruit = new AIGroupRecruitControl(null);
                        Recruit.Setup(this);
                        var strengthMax = FGroup.FProductions.GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall));
                        Recruit.FRecruitStrength = FRequiredStrength - (strengthMax + FStrengthMax);
                        Recruit.FMode = AIGroupRecruitControl.Mode.ForceRecruit;
                        ActivatePlugin(Recruit);
                        Recruit.Release();
                    }

                    FState = 10;
                    break;
                case 10:
                    if (FStrengthMax >= FEnemyStrength && FGroup.GetAdjacent())
                    {
                        FTargets.Clear();
                        Finder = new AIGroupTargetFinder(null);
                        Finder.Setup(this);
                        Finder.FSelector.FTargetInfo.Mode = AITargetMode.AttackArmies;
                        Finder.FSelector.FMovePointsMax = 55;
                        if (FFortifyLocation.Packed == 16777215)
                        {
                            Finder.FSelector.FSourceLocation = FLocation;
                        }
                        else
                        {
                            Finder.FSelector.FSourceLocation = FFortifyLocation;
                        }

                        Finder.FMode = Finder.FMode + Set<AIGroupTargetFinderModeElement>.Of(AIGroupTargetFinderModeElement.CalculateMovepathBFS);
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
                            FTargetSelector.FMode = FTargetSelector.FMode - Set<AIGroupTargetSelectorModeElement>.Of(AIGroupTargetSelectorModeElement.MultiTargets);
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
                    if (FTargetSelector.FTargetStatus == AIGroupTargetSelectorStatus.TargetConquered)
                    {
                        FState = 10;
                    }
                    else
                    {
                        if (FFortifyLocation.Packed != 16777215)
                        {
                            SetupME();
                            ExecuteME(FFortifyLocation.X, FFortifyLocation.Y, FFortifyLocation.L);
                        }

                        FState = 20;
                    }

                    break;
                case 13:
                    ACity = (City)AoWHSMap.Current.MapContainer.GetFieldXYL(FLocation.Packed).FindNoneTransparentHS(typeof(City));
                    if (ACity != null && ACity.GetActiveAction() == CityAction.Migrate)
                    {
                        Defense = new AIGroupDefenseControl(null);
                        Defense.FDefendMode = AIGroupDefenseControl.GroupDefendMode.Custom;
                        Defense.FCustomState = 19;
                        ActivatePlugin(Defense);
                        Defense.Release();
                    }

                    FState += 1;
                    break;
                case 14:
                    FTargets.Clear();
                    Finder = new AIGroupTargetFinder(null);
                    Finder.Setup(this);
                    Finder.FSelector.FTargetInfo.Mode = AITargetMode.Attack;
                    Finder.FSelector.FMovePointsMax = 55;
                    if (FFortifyLocation.Packed == 16777215)
                    {
                        Finder.FSelector.FSourceLocation = FLocation;
                    }
                    else
                    {
                        Finder.FSelector.FSourceLocation = FFortifyLocation;
                    }

                    Finder.FMode = Finder.FMode + Set<AIGroupTargetFinderModeElement>.Of(AIGroupTargetFinderModeElement.CalculateMovepathBFS);
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
                        FTargetSelector.FMode = FTargetSelector.FMode - Set<AIGroupTargetSelectorModeElement>.Of(AIGroupTargetSelectorModeElement.MultiTargets);
                        ActivatePlugin(FTargetSelector);
                        FState += 1;
                    }
                    else
                    {
                        FState = 18;
                    }

                    break;
                case 16:
                    if (FTargetSelector.FTargetStatus == AIGroupTargetSelectorStatus.TargetConquered)
                    {
                        FState = 13;
                    }
                    else
                    {
                        if (FFortifyLocation.Packed != 16777215 && !Set<AIGroupTargetSelectorStatus>.Of(AIGroupTargetSelectorStatus.SettingupTarget, AIGroupTargetSelectorStatus.ExecutingTarget).Contains(FTargetSelector.FTargetStatus))
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
                        ACity = (City)AoWHSMap.Current.MapContainer.GetFieldXYL(FLocation.Packed).FindNoneTransparentHS(typeof(City));
                        if (ACity != null && ACity.GetActiveAction() == CityAction.Migrate)
                        {
                            Defense = new AIGroupDefenseControl(null);
                            Defense.FDefendMode = AIGroupDefenseControl.GroupDefendMode.Custom;
                            Defense.FCustomState = 19;
                            ActivatePlugin(Defense);
                            Defense.Release();
                        }
                    }

                    FState += 1;
                    break;
                case 18:
                    if (FFortifyLocation.Packed == 16777215)
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
                    if (ACity != null && ACity.FProduction.FStatus == ProductionControl.Status.Idle)
                    {
                        if (ACity.FWallType == 0)
                        {
                            var fortifyCost = ACity.FortifyCost();
                            if (AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer).FAIBudget.RequestExpense(fortifyCost, 0))
                            {
                                if (ACity.Fortify(false))
                                {
                                    var aCityFortifyCost = ACity.FortifyCost();
                                    AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer).FAIBudget.ExpenseMade(aCityFortifyCost, 0);
                                }
                            }
                        }
                        else
                        {
                            if (ACity.FWallType == 1 && AoWHSMap.Current.FDay > 29)
                            {
                                var fortifyCost = ACity.FortifyCost();
                                if (AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer).FAIBudget.RequestExpense(fortifyCost, 0))
                                {
                                    if (ACity.Fortify(false))
                                    {
                                        var aCityFortifyCost = ACity.FortifyCost();
                                        AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer).FAIBudget.ExpenseMade(aCityFortifyCost, 0);
                                    }
                                }
                            }
                        }
                    }

                    FState += 1;
                    break;
                case 22:
                    if (FGroup.FProductions.GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall)) + FStrengthMax < FRequiredStrength)
                    {
                        FProductionLocations.Clear();
                        ProductionFinder = new AIProductionLocationFinder(null);
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
                        Produce = new AIGroupProductionControl(null);
                        Produce.Setup(this);
                        var strengthMax = FGroup.FProductions.GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall));
                        Produce.FProduceStrength = FRequiredStrength - (strengthMax + FStrengthMax);
                        Produce.FMode = AIGroupProductionControl.Mode.AutoProduce;
                        ActivatePlugin(Produce);
                        Produce.Release();
                    }

                    FState += 1;
                    break;
                case 24:
                    switch (FGroup.FTargetType)
                    {
                        case AIGroupTargetStatus.None:
                        case AIGroupTargetStatus.Idle:
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
        public City FortifyCity()
        {
            City Result;
            if (FFortifyLocation.Packed != 16777215)
            {
                Result = (City)AoWHSMap.Current.MapContainer.GetFieldXYL(FFortifyLocation.Packed).FindNoneTransparentHS(typeof(City));
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // MainAG.pas:287
        public int GetSurroundingEnemyStrength(XYL Location, int Radius)
        {
            MoveControlSettings Settings;
            MapFieldMsg Msg = default;
            var query = new BattleFieldQuery();
            ref var Info = ref query.Info;
            Settings = new MoveControlSettings(null);
            Settings.FPlayer = FGroup.FPlayer;
            Settings.FMode = Settings.FMode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields, MoveModeElement.AI);
            Settings.SetSourcePos(Location.X, Location.Y, Location.L);
            Settings.FMovePointsMax = Radius;
            Settings.FArmyHS = null;
            Settings.FSelection = 0;
            var moveTypesToInt = MovementTables.MoveTypesToInt(Set<MoveType>.Of(MoveType.Flying));
            Settings.FMovePointTable = MovementTables.MovePointTables[moveTypesToInt];
            AoWHSMap.Current.FMoveControl.ProcessMove(Settings);
            BattleFieldInfo.SetupBattleFieldInfo(ref Info, FGroup.FPlayer, FGroup.FIndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents), AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall));
            Info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
            Msg.MsgID = 4360;
            Msg.Param[0] = query;
            for (int I = Settings.FProcessedMapFields.Count - 1; I >= 0; I--)
            {
                var mapField = (AoWMapField)Settings.FProcessedMapFields[I];
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
            PlayerStatistics Statistics;
            FRequiredStrength = FEnemyStrength;
            if (AoWHSMap.Current.FDay > 5)
            {
                Strength = AoWHSMap.Current.FDay / 5 * 5000;
                if (Strength > 50000)
                {
                    Strength = 50000;
                }

                if (Strength > FEnemyStrength)
                {
                    FRequiredStrength = Strength;
                }
            }

            if (FRequiredStrength > 0 && AoWHSMap.Current.FDay < 10)
            {
                FRequiredStrength = Round((double)(FRequiredStrength * AoWHSMap.Current.FDay) * 0.1);
            }

            Statistics = AoWHSMap.Current.FPlayers.GetPlayers(FGroup.FPlayer).FStatistics;
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

                if (AoWHSMap.Current.FDay < 10)
                {
                    Strength = Round((double)(Strength * AoWHSMap.Current.FDay) * 0.1);
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
            UnitUserList Units = default;
            UnitUserList Passive = default;
            AIGroup Group = default;
            Excess = FStrengthMax - FRequiredStrength;
            if (Excess > 0)
            {
                Units = new UnitUserList(null);
                Passive = new UnitUserList(null);
                Units.AddUnits(FGroup);
                Units.FChildren.Sort(CompareFortifyUnits);
                I = Units.Count;
                while (I != 0 && Excess > 0)
                {
                    I -= 1;
                    if (!(Units.GetUnit(I) is Leader))
                    {
                        Excess -= Units.GetUnit(I).GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall));
                        if (Excess >= 0)
                        {
                            Passive.AddUnit(Units.GetUnit(I));
                            Units.DeleteUnit(Units.GetUnit(I));
                        }
                    }
                }

                if (Passive.Count != 0)
                {
                    Group = AoWHSMap.Current.FAIGroupManager.CreateGroup(FGroup.FPlayer, FGroup.FIndependentRelation, GroupBehavior.Passive);
                    Group.AddUnits(Passive);
                    Group.Release();
                }

                Passive?.Dispose();
                Units?.Dispose();
            }
        }

        // MainAG.pas:391
        public bool CanBreachWall()
        {
            int I;
            AbstractUnit AUnit;
            I = FGroup.Count;
            bool Result = FGroup.GetStrength(DamageType.Wall) != 0;
            while (!Result && I != 0)
            {
                I -= 1;
                AUnit = FGroup.GetUnit(I);
                Result = AUnit.GetAbilityEnabled(52) && ((SpellCastingAbility)AoWHSSet.Current.FAbilityControl.GetAbility(52)).CanCastBreachWallSpell(AUnit);
            }

            return Result;
        }

        private static int CompareFortifyUnits(object Item1, object Item2)
        {
            int num = ((AbstractUnit)Item1).GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(new DamageType[1] { DamageType.Wall }));
            int num2 = ((AbstractUnit)Item2).GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(new DamageType[1] { DamageType.Wall }));
            if (Item1 is Leader)
            {
                num = num2 + 16777215;
            }
            if (Item2 is Leader)
            {
                num2 += 16777215;
            }
            return num2 - num;
        }

    }

    public partial class SuicidalAG : AIGroup, IAIGroupClass
    {
        // MainAG.pas:729
        public new const int ClassID = 136276;

        // MainAG.pas:722
        public SuicidalAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 1;
            FRecruitmentPriority = 2;
        }

        // MainAG.pas:735
        public override EClass GroupControlClass() => EClass.Of<SuicidalAGC>();

        // MainAG.pas:732
        public static new GroupBehavior Behavior => GroupBehavior.Suicidal;

    }

    public partial class SuicidalAGC : AIGroupControl
    {
        public SuicidalAGC(ECustomNode owner) : base(owner)
        { }

        // MainAG.pas:750
        public override void MoveExecuterDone(object Sender)
        {
            switch (FState)
            {
                case 1:
                    if (FMoveExecuter.FStatus == AIMoveExecuter.Status.OutOfMovePoints)
                    {
                        Done();
                    }
                    else
                    {
                        if (FMoveExecuter.FStatus == AIMoveExecuter.Status.Invalidated)
                        {
                            Done();
                        }
                    }

                    break;
            }
        }

        // MainAG.pas:738
        public override void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            var execute = FCombatPredictor.Execute();
            if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
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
            AIGroupTargetFinder Finder;
            switch (FState)
            {
                case 0:
                    Finder = new AIGroupTargetFinder(null);
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

    public partial class TransportAG : AIGroup, IAIGroupClass
    {
        // MainAG.pas:801
        public new const int ClassID = 136295;

        // MainAG.pas:789
        public TransportAG(ECustomNode owner) : base(owner)
        {
            FProcessPriority = 2;
            FRecruitmentPriority = 20;
        }

        // MainAG.pas:807
        public override EClass GroupControlClass() => EClass.Of<TransportAGC>();

        // MainAG.pas:804
        public static new GroupBehavior Behavior => GroupBehavior.Transport;

    }

    public partial class TransportAGC : AIGroupControl
    {
        public TransportAGC(ECustomNode owner) : base(owner)
        { }

        // MainAG.pas:822
        public override void MoveExecuterDone(object Sender)
        {
            switch (FState)
            {
                case 1:
                    if (FMoveExecuter.FStatus == AIMoveExecuter.Status.OutOfMovePoints)
                    {
                        Done();
                    }
                    else
                    {
                        if (FMoveExecuter.FStatus == AIMoveExecuter.Status.Invalidated)
                        {
                            Done();
                        }
                    }

                    break;
            }
        }

        // MainAG.pas:810
        public override void TargetFoundEvent(AIGroupTargetFinder Sender, AIGroupControlTarget Target, ref bool SearchDone)
        {
            SetupCP(Target);
            var execute = FCombatPredictor.Execute();
            if (Set<CombatResult>.Of(CombatResult.Victory, CombatResult.Defeat, CombatResult.NoDefenders).Contains(execute))
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
            AIGroupTargetFinder Finder;
            AIGroupWanderControl Wander;
            switch (FState)
            {
                case 0:
                    Finder = new AIGroupTargetFinder(null);
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
                    Wander = new AIGroupWanderControl(null);
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
            AbstractUnit Transporter;
            Army Army;
            base.SetupME();
            Transporter = FGroup.FLeader;
            if (Transporter != null && Transporter.Owner is Army)
            {
                Army = Transporter.ArmyOwner;
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
}

namespace Aow.Game.Ai.Groups
{

    // no fields. map-load registration callback
    public partial class MainRegistrator
    {
        // MainAG.pas:115
        public static void MapLoaded(HSMap Sender)
        {
            if (Sender == AoWHSMap.Current)
            {
                AoWHSMap.Current.FAIGroupManager.RegisterAIGroupClasses([AIGroupClass.Of<PassiveAG>(), AIGroupClass.Of<SuicidalAG>(), AIGroupClass.Of<FortifyAG>(), AIGroupClass.Of<TransportAG>()]);
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
