// Translated from MainAG.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// city location, enemy strength, required strength, target selector
public partial class FortifyAIGroupControl : AIGroupControl
{
    private MapCoordinate _fortifyLocation;
    private int _enemyStrength;
    private int _requiredStrength;
    private readonly AIGroupTargetSelector _targetSelector;

    // MainAG.pas:262
    public override void Dispose()
    {
        _targetSelector.Release();
        base.Dispose();
    }

    // MainAG.pas:256
    public FortifyAIGroupControl(ECustomNode owner) : base(owner)
    {
        _targetSelector = new AIGroupTargetSelector(null) { Holder = CreationHolder() };
    }

    // MainAG.pas:278
    public override void TargetFoundEvent(AIGroupTargetFinder sender, AIGroupControlTarget target, ref bool searchDone)
    {
        switch (sender.Selector.TargetInfo.Mode)
        {
            case AITargetMode.Fortify:
                base.TargetFoundEvent(sender, target, ref searchDone);
                break;
            case AITargetMode.AttackArmies:
                base.TargetFoundEvent(sender, target, ref searchDone);
                break;
            case AITargetMode.Attack:
                base.TargetFoundEvent(sender, target, ref searchDone);
                break;
        }
    }

    // MainAG.pas:407
    public override void ProcessStates()
    {
        AIGroupTargetFinder finder;
        AIRecruitmentGroupFinder recruitment;
        AIProductionLocationFinder productionFinder;
        AIGroupRecruitControl recruit;
        AIGroupProductionControl produce;
        AIGroupDefenseControl defense;
        AIGroupControlTarget currentTarget;
        AIGroupControlTarget newTarget;
        int I;
        int excess;
        ArmyHexSprite army;
        City city;
        switch (State)
        {
            case 0:
                finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                finder.Setup(this);
                finder.Selector.TargetInfo.Mode = AITargetMode.Fortify;
                finder.Mode = finder.Mode + Set<AIGroupTargetFinderModeElement>.Of(AIGroupTargetFinderModeElement.CalculateMovePathBfs);
                finder.Range = 12;
                ActivatePlugin(finder);
                finder.Release();
                State += 1;
                break;
            case 1:
                I = 0;
                while (I < Targets.Count && Targets.GetTargets(I).Status.Contains(AIGroupTargetFlags.Done))
                {
                    I += 1;
                }

                if (I < Targets.Count)
                {
                    var surroundingEnemyStrength = GetSurroundingEnemyStrength(Targets.GetTargets(I).Location, 70);
                    Targets.GetTargets(I).DataA = surroundingEnemyStrength;
                    var target = Targets.GetTargets(I);
                    Targets.GetTargets(I).Status = target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Done);
                }
                else
                {
                    State += 1;
                }

                break;
            case 2:
                Targets.Validate();
                newTarget = null;
                I = Targets.IndexOfMapCoordinate(Location.Packed);
                if (I == -1)
                {
                    currentTarget = null;
                }
                else
                {
                    currentTarget = Targets.GetTargets(I);
                }

                {
                    I = 0;
                    int last = Targets.Count - 1;
                    for (; I <= last; I += 1)
                    {
                        var targetData = Targets.GetTargets(I);
                        var targetMovePoints = Targets.GetTargets(I);
                        Targets.GetTargets(I).DataB = targetData.Data.Priority * 10 + targetMovePoints.MovePoints * -2;
                        excess = Targets.GetTargets(I).DataA - StrengthMax;
                        if (excess > 0)
                        {
                            var target = Targets.GetTargets(I);
                            target.DataB -= (int)(((uint)excess) >> 4);
                        }

                        excess = Targets.GetTargets(I).PathStrength;
                        if (excess > 0)
                        {
                            var target = Targets.GetTargets(I);
                            target.DataB -= (int)(((uint)excess) >> 3);
                        }

                        if (Targets.GetTargets(I).Location.Packed != Location.Packed)
                        {
                            var targetLocation = Targets.GetTargets(I);
                            army = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(targetLocation.Location.Packed).GetArmyHexSprite();
                            if (army != null)
                            {
                                var count = army.Army.Count;
                                var target = Targets.GetTargets(I);
                                target.DataB -= count << 2;
                            }
                        }
                    }
                }

                Targets.SortOnPriority();
                if (Targets.Count != 0 && Targets.GetTargets(0) != currentTarget)
                {
                    Targets.SortOnDataB();
                    bool condition = currentTarget == null;
                    if (!condition)
                    {
                        var target = Targets.GetTargets(0);
                        condition = currentTarget.Data.Priority < target.Data.Priority;
                    }

                    if (condition)
                    {
                        newTarget = Targets.GetTargets(0);
                    }
                    else
                    {
                        newTarget = currentTarget;
                    }
                }

                if (newTarget != null && newTarget != currentTarget)
                {
                    _fortifyLocation = newTarget.Location;
                }
                else
                {
                    if (currentTarget != null)
                    {
                        _fortifyLocation = currentTarget.Location;
                    }
                    else
                    {
                        _fortifyLocation.Packed = 16777215;
                    }
                }

                State += 1;
                break;
            case 3:
                if (_fortifyLocation.Packed == 16777215)
                {
                    _enemyStrength = GetSurroundingEnemyStrength(Location, 70);
                }
                else
                {
                    _enemyStrength = GetSurroundingEnemyStrength(_fortifyLocation, 70);
                }

                UpdateRequiredStrength();
                State += 1;
                break;
            case 4:
                recruitment = new AIRecruitmentGroupFinder(null) { Holder = CreationHolder() };
                recruitment.Setup(this);
                ActivatePlugin(recruitment);
                recruitment.Release();
                State += 1;
                break;
            case 5:
                ProductionLocations.Clear();
                productionFinder = new AIProductionLocationFinder(null) { Holder = CreationHolder() };
                productionFinder.Setup(this);
                productionFinder.Selector.MovePointsMax = 24;
                ActivatePlugin(productionFinder);
                productionFinder.Release();
                State += 1;
                break;
            case 6:
                if (RecruitmentGroups.Count != 0 && _requiredStrength > StrengthMax)
                {
                    recruit = new AIGroupRecruitControl(null) { Holder = CreationHolder() };
                    recruit.Setup(this);
                    var strengthMax = Group.Productions.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                    recruit.RecruitStrength = _requiredStrength - (strengthMax + StrengthMax);
                    recruit.Mode = AIGroupRecruitControlMode.AutoRecruit;
                    ActivatePlugin(recruit);
                    recruit.Release();
                    State += 1;
                }
                else
                {
                    State = 10;
                }

                break;
            case 7:
                RecruitmentGroups.Validate(this);
                if (StrengthMax < _requiredStrength && RecruitmentGroups.Count != 0)
                {
                    recruit = new AIGroupRecruitControl(null) { Holder = CreationHolder() };
                    recruit.Setup(this);
                    var strengthMax = Group.Productions.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                    recruit.RecruitStrength = _requiredStrength - (strengthMax + StrengthMax);
                    recruit.Mode = AIGroupRecruitControlMode.ForceRecruit;
                    ActivatePlugin(recruit);
                    recruit.Release();
                }

                State = 10;
                break;
            case 10:
                if (StrengthMax >= _enemyStrength && Group.GetAdjacent())
                {
                    Targets.Clear();
                    finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                    finder.Setup(this);
                    finder.Selector.TargetInfo.Mode = AITargetMode.AttackArmies;
                    finder.Selector.MovePointsMax = 55;
                    if (_fortifyLocation.Packed == 16777215)
                    {
                        finder.Selector.SourceLocation = Location;
                    }
                    else
                    {
                        finder.Selector.SourceLocation = _fortifyLocation;
                    }

                    finder.Mode = finder.Mode + Set<AIGroupTargetFinderModeElement>.Of(AIGroupTargetFinderModeElement.CalculateMovePathBfs);
                    finder.Range = 12;
                    ActivatePlugin(finder);
                    finder.Release();
                    State += 1;
                }
                else
                {
                    if (StrengthMax < _enemyStrength)
                    {
                        State = 18;
                    }
                    else
                    {
                        State = 17;
                    }
                }

                break;
            case 11:
                if (Targets.Count >= 1)
                {
                    if (Targets.Count == 1 || FortifyCity() == null || CanBreachWall())
                    {
                        _targetSelector.Setup(this);
                        _targetSelector.PathStrengthFactor = (float)1.5;
                        _targetSelector.CurrentPathStrengthFactor = (float)1.3;
                        _targetSelector.Superiority = (float)1.6;
                        _targetSelector.NearSuperiority = (float)1.4;
                        _targetSelector.Mode = _targetSelector.Mode - Set<AIGroupTargetingModes>.Of(AIGroupTargetingModes.MultiTargets);
                        ActivatePlugin(_targetSelector);
                        State += 1;
                    }
                    else
                    {
                        State = 18;
                    }
                }
                else
                {
                    State = 13;
                }

                break;
            case 12:
                if (_targetSelector.TargetStatus == AIGroupTargetingStatus.TargetConquered)
                {
                    State = 10;
                }
                else
                {
                    if (_fortifyLocation.Packed != 16777215)
                    {
                        SetupME();
                        ExecuteME(_fortifyLocation.X, _fortifyLocation.Y, _fortifyLocation.MapLevel);
                    }

                    State = 20;
                }

                break;
            case 13:
                city = (City)StrategicMap.Current.MapContainer.GetFieldMapCoordinate(Location.Packed).FindNoneTransparentHexSprite(typeof(City));
                if (city != null && city.GetActiveAction() == CityAction.Migrate)
                {
                    defense = new AIGroupDefenseControl(null) { Holder = CreationHolder() };
                    defense.DefendMode = AIGroupDefenseControl.GroupDefendMode.Custom;
                    defense.CustomState = 19;
                    ActivatePlugin(defense);
                    defense.Release();
                }

                State += 1;
                break;
            case 14:
                Targets.Clear();
                finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                finder.Setup(this);
                finder.Selector.TargetInfo.Mode = AITargetMode.Attack;
                finder.Selector.MovePointsMax = 55;
                if (_fortifyLocation.Packed == 16777215)
                {
                    finder.Selector.SourceLocation = Location;
                }
                else
                {
                    finder.Selector.SourceLocation = _fortifyLocation;
                }

                finder.Mode = finder.Mode + Set<AIGroupTargetFinderModeElement>.Of(AIGroupTargetFinderModeElement.CalculateMovePathBfs);
                finder.Range = 12;
                ActivatePlugin(finder);
                finder.Release();
                State += 1;
                break;
            case 15:
                if (Targets.Count >= 1)
                {
                    _targetSelector.Setup(this);
                    _targetSelector.PathStrengthFactor = (float)1.5;
                    _targetSelector.CurrentPathStrengthFactor = (float)1.3;
                    _targetSelector.Superiority = (float)1.6;
                    _targetSelector.NearSuperiority = (float)1.4;
                    _targetSelector.Mode = _targetSelector.Mode - Set<AIGroupTargetingModes>.Of(AIGroupTargetingModes.MultiTargets);
                    ActivatePlugin(_targetSelector);
                    State += 1;
                }
                else
                {
                    State = 18;
                }

                break;
            case 16:
                if (_targetSelector.TargetStatus == AIGroupTargetingStatus.TargetConquered)
                {
                    State = 13;
                }
                else
                {
                    if (_fortifyLocation.Packed != 16777215 && !Set<AIGroupTargetingStatus>.Of(AIGroupTargetingStatus.SettingUpTarget, AIGroupTargetingStatus.ExecutingTarget).Contains(_targetSelector.TargetStatus))
                    {
                        SetupME();
                        ExecuteME(_fortifyLocation.X, _fortifyLocation.Y, _fortifyLocation.MapLevel);
                    }

                    State = 20;
                }

                break;
            case 17:
                if (StrengthMax >= _enemyStrength)
                {
                    city = (City)StrategicMap.Current.MapContainer.GetFieldMapCoordinate(Location.Packed).FindNoneTransparentHexSprite(typeof(City));
                    if (city != null && city.GetActiveAction() == CityAction.Migrate)
                    {
                        defense = new AIGroupDefenseControl(null) { Holder = CreationHolder() };
                        defense.DefendMode = AIGroupDefenseControl.GroupDefendMode.Custom;
                        defense.CustomState = 19;
                        ActivatePlugin(defense);
                        defense.Release();
                    }
                }

                State += 1;
                break;
            case 18:
                if (_fortifyLocation.Packed == 16777215)
                {
                    SetupME();
                    ExecuteME(Location.X, Location.Y, Location.MapLevel);
                }
                else
                {
                    SetupME();
                    ExecuteME(_fortifyLocation.X, _fortifyLocation.Y, _fortifyLocation.MapLevel);
                }

                State = 20;
                break;
            case 19:
                SetupME();
                ExecuteME(Location.X, Location.Y, Location.MapLevel);
                State = 20;
                break;
            case 20:
                if (FortifyCity() != null)
                {
                    State = 21;
                }
                else
                {
                    Done();
                }

                break;
            case 21:
                city = FortifyCity();
                if (city != null && city.Production.Status == ProductionControlStatus.Idle)
                {
                    if (city.WallType == 0)
                    {
                        var fortifyCost = city.FortifyCost();
                        if (StrategicMap.Current.Players.GetPlayers(Group.Player).AIBudget.RequestExpense(fortifyCost, 0))
                        {
                            if (city.Fortify(false))
                            {
                                var aCityFortifyCost = city.FortifyCost();
                                StrategicMap.Current.Players.GetPlayers(Group.Player).AIBudget.ExpenseMade(aCityFortifyCost, 0);
                            }
                        }
                    }
                    else
                    {
                        if (city.WallType == 1 && StrategicMap.Current.Day > 29)
                        {
                            var fortifyCost = city.FortifyCost();
                            if (StrategicMap.Current.Players.GetPlayers(Group.Player).AIBudget.RequestExpense(fortifyCost, 0))
                            {
                                if (city.Fortify(false))
                                {
                                    var aCityFortifyCost = city.FortifyCost();
                                    StrategicMap.Current.Players.GetPlayers(Group.Player).AIBudget.ExpenseMade(aCityFortifyCost, 0);
                                }
                            }
                        }
                    }
                }

                State += 1;
                break;
            case 22:
                if (Group.Productions.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall)) + StrengthMax < _requiredStrength)
                {
                    ProductionLocations.Clear();
                    productionFinder = new AIProductionLocationFinder(null) { Holder = CreationHolder() };
                    productionFinder.Setup(this);
                    productionFinder.Selector.MovePointsMax = 24;
                    ActivatePlugin(productionFinder);
                    productionFinder.Release();
                    State += 1;
                }
                else
                {
                    State += 1;
                }

                break;
            case 23:
                if (ProductionLocations.Count > 0)
                {
                    produce = new AIGroupProductionControl(null) { Holder = CreationHolder() };
                    produce.Setup(this);
                    var strengthMax = Group.Productions.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                    produce.ProduceStrength = _requiredStrength - (strengthMax + StrengthMax);
                    produce.Mode = AIGroupProductionControlMode.AutoProduce;
                    ActivatePlugin(produce);
                    produce.Release();
                }

                State += 1;
                break;
            case 24:
                switch (Group.TargetType)
                {
                    case AIGroupTargetStatus.None:
                    case AIGroupTargetStatus.Idle:
                        MakeUselessUnitsPassive();
                        break;
                }

                State += 1;
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
        City result;
        if (_fortifyLocation.Packed != 16777215)
        {
            result = (City)StrategicMap.Current.MapContainer.GetFieldMapCoordinate(_fortifyLocation.Packed).FindNoneTransparentHexSprite(typeof(City));
        }
        else
        {
            result = null;
        }

        return result;
    }

    // MainAG.pas:287
    public int GetSurroundingEnemyStrength(MapCoordinate location, int radius)
    {
        MoveControlSettings settings;
        MapFieldMessage message = default;
        var query = new BattleFieldQuery();
        ref var info = ref query.Info;
        settings = new MoveControlSettings(null) { Holder = CreationHolder() };
        settings.Player = Group.Player;
        settings.Mode = settings.Mode + Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields, MoveModeElement.AI);
        settings.SetSourcePosition(location.X, location.Y, location.MapLevel);
        settings.MovePointsMax = radius;
        settings.ArmyHexSprite = null;
        settings.Selection = 0;
        var moveTypesToInt32 = MovementTables.MoveTypesToInt32(Set<MoveType>.Of(MoveType.Flying));
        settings.MovePointTable = MovementTables.MovePointTables[moveTypesToInt32];
        StrategicMap.Current.MoveControl.ProcessMove(settings);
        BattleFieldInfo.SetupBattleFieldInfo(out info, Group.Player, Group.IndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents), AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
        info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
        message.MessageId = 4360;
        message.Param[0] = query;
        for (int I = settings.ProcessedMapFields.Count - 1; I >= 0; I--)
        {
            var mapField = settings.ProcessedMapFields[I];
            if ((mapField.FieldFlags & 67108864) != 0)
            {
                mapField.SendMapFieldMessage(ref message);
            }
        }

        int result = info.Strength;
        settings.Release();
        return result;
    }

    // MainAG.pas:315
    public void UpdateRequiredStrength()
    {
        int strength;
        PlayerStatistics statistics;
        _requiredStrength = _enemyStrength;
        if (StrategicMap.Current.Day > 5)
        {
            strength = StrategicMap.Current.Day / 5 * 5000;
            if (strength > 50000)
            {
                strength = 50000;
            }

            if (strength > _enemyStrength)
            {
                _requiredStrength = strength;
            }
        }

        if (_requiredStrength > 0 && StrategicMap.Current.Day < 10)
        {
            _requiredStrength = Round((double)(_requiredStrength * StrategicMap.Current.Day) * 0.1);
        }

        statistics = StrategicMap.Current.Players.GetPlayers(Group.Player).Statistics;
        if (statistics.TurnInfo.Count > 0)
        {
            var count = statistics.TurnInfo.Count;
            strength = (int)(((uint)statistics.TurnInfo.GetTurnInfo(count - 1).ArmyStrength) >> 1);
            if (strength < _requiredStrength)
            {
                _requiredStrength = strength;
            }

            var fTurnInfoCount = statistics.TurnInfo.Count;
            strength = Round((double)statistics.TurnInfo.GetTurnInfo(fTurnInfoCount - 1).ArmyStrength * 0.1);
            if (strength > 50000)
            {
                strength = 50000;
            }

            if (StrategicMap.Current.Day < 10)
            {
                strength = Round((double)(strength * StrategicMap.Current.Day) * 0.1);
            }

            if (strength > _requiredStrength)
            {
                _requiredStrength = strength;
            }
        }
    }

    // MainAG.pas:352
    public void MakeUselessUnitsPassive()
    {
        int excess = default;
        int I = default;
        UnitUserList units = default;
        UnitUserList passive = default;
        AIGroup group = default;
        excess = StrengthMax - _requiredStrength;
        if (excess > 0)
        {
            units = new UnitUserList(null) { Holder = CreationHolder() };
            passive = new UnitUserList(null) { Holder = CreationHolder() };
            units.AddUnits(Group);
            units.SortChildren<Troop>(CompareFortifyUnits);
            I = units.Count;
            while (I != 0 && excess > 0)
            {
                I -= 1;
                if (!(units.GetUnit(I) is Leader))
                {
                    excess -= units.GetUnit(I).GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                    if (excess >= 0)
                    {
                        passive.AddUnit(units.GetUnit(I));
                        units.DeleteUnit(units.GetUnit(I));
                    }
                }
            }

            if (passive.Count != 0)
            {
                group = StrategicMap.Current.AIGroupManager.CreateGroup(Group.Player, Group.IndependentRelation, GroupBehavior.Passive);
                group.AddUnits(passive);
                group.Release();
            }

            passive?.Dispose();
            units?.Dispose();
        }
    }

    // MainAG.pas:391
    public bool CanBreachWall()
    {
        int I;
        Troop troop;
        I = Group.Count;
        bool result = Group.GetStrength(DamageType.Wall) != 0;
        while (!result && I != 0)
        {
            I -= 1;
            troop = Group.GetUnit(I);
            result = troop.GetAbilityEnabled(52) && ((SpellCastingAbility)GamePack.Current.AbilityControl.GetAbility(52)).CanCastBreachWallSpell(troop);
        }

        return result;
    }

    private static int CompareFortifyUnits(Troop item1, Troop item2)
    {
        int number = item1.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(new DamageType[1] { DamageType.Wall }));
        int number2 = item2.GetStrengthMax(AbilityRules.s_allDamageTypes - Set<DamageType>.Of(new DamageType[1] { DamageType.Wall }));
        if (item1 is Leader)
        {
            number = number2 + 16777215;
        }
        if (item2 is Leader)
        {
            number2 += 16777215;
        }
        return number2 - number;
    }

}
