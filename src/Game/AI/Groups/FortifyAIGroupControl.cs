#nullable disable
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
    public FortifyAIGroupControl(CustomNode owner) : base(owner)
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
        int i;
        int excess;
        ArmyHexSprite army;
        City city;
        switch (State)
        {
            case 0:
                finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                finder.Setup(this);
                var targetInfo = finder.Selector.TargetInfo;
                targetInfo.Mode = AITargetMode.Fortify;
                finder.Selector.TargetInfo = targetInfo;
                finder.Mode = finder.Mode + Set<AIGroupTargetFinderModeElement>.Of(AIGroupTargetFinderModeElement.CalculateMovePathBfs);
                finder.Range = 12;
                ActivatePlugin(finder);
                finder.Release();
                State += 1;
                break;
            case 1:
                i = 0;
                while (i < Targets.Count && Targets.GetTargets(i).Status.Contains(AIGroupTargetFlags.Done))
                {
                    i += 1;
                }

                if (i < Targets.Count)
                {
                    var surroundingEnemyStrength = GetSurroundingEnemyStrength(Targets.GetTargets(i).Location, 70);
                    Targets.GetTargets(i).DataA = surroundingEnemyStrength;
                    var target = Targets.GetTargets(i);
                    Targets.GetTargets(i).Status = target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Done);
                }
                else
                {
                    State += 1;
                }

                break;
            case 2:
                Targets.Validate();
                newTarget = null;
                i = Targets.IndexOfMapCoordinate(Location);
                if (i == -1)
                {
                    currentTarget = null;
                }
                else
                {
                    currentTarget = Targets.GetTargets(i);
                }

                {
                    i = 0;
                    int last = Targets.Count - 1;
                    for (; i <= last; i += 1)
                    {
                        var targetData = Targets.GetTargets(i);
                        var targetMovePoints = Targets.GetTargets(i);
                        Targets.GetTargets(i).DataB = targetData.Data.Priority * 10 + targetMovePoints.MovePoints * -2;
                        excess = Targets.GetTargets(i).DataA - StrengthMax;
                        if (excess > 0)
                        {
                            var target = Targets.GetTargets(i);
                            target.DataB -= (int)(((uint)excess) >> 4);
                        }

                        excess = Targets.GetTargets(i).PathStrength;
                        if (excess > 0)
                        {
                            var target = Targets.GetTargets(i);
                            target.DataB -= (int)(((uint)excess) >> 3);
                        }

                        if (Targets.GetTargets(i).Location != Location)
                        {
                            var targetLocation = Targets.GetTargets(i);
                            army = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(targetLocation.Location).GetArmyHexSprite();
                            if (army != null)
                            {
                                var count = army.Army.Units.Count;
                                var target = Targets.GetTargets(i);
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
                        _fortifyLocation = MapCoordinate.None;
                    }
                }

                State += 1;
                break;
            case 3:
                if (_fortifyLocation == MapCoordinate.None)
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
                    var strengthMax = Group.Productions.GetStrengthMax(AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall));
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
                    var strengthMax = Group.Productions.GetStrengthMax(AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall));
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
                    var targetInfo2 = finder.Selector.TargetInfo;
                    targetInfo2.Mode = AITargetMode.AttackArmies;
                    finder.Selector.TargetInfo = targetInfo2;
                    finder.Selector.MovePointsMax = 55;
                    if (_fortifyLocation == MapCoordinate.None)
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
                    if (_fortifyLocation != MapCoordinate.None)
                    {
                        SetupME();
                        ExecuteME(_fortifyLocation.X, _fortifyLocation.Y, _fortifyLocation.MapLevel);
                    }

                    State = 20;
                }

                break;
            case 13:
                city = (City)StrategicMap.Current.MapContainer.GetFieldMapCoordinate(Location).FindNoneTransparentHexSprite(typeof(City));
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
                var targetInfo3 = finder.Selector.TargetInfo;
                targetInfo3.Mode = AITargetMode.Attack;
                finder.Selector.TargetInfo = targetInfo3;
                finder.Selector.MovePointsMax = 55;
                if (_fortifyLocation == MapCoordinate.None)
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
                    if (_fortifyLocation != MapCoordinate.None && !Set<AIGroupTargetingStatus>.Of(AIGroupTargetingStatus.SettingUpTarget, AIGroupTargetingStatus.ExecutingTarget).Contains(_targetSelector.TargetStatus))
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
                    city = (City)StrategicMap.Current.MapContainer.GetFieldMapCoordinate(Location).FindNoneTransparentHexSprite(typeof(City));
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
                if (_fortifyLocation == MapCoordinate.None)
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
                if (Group.Productions.GetStrengthMax(AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall)) + StrengthMax < _requiredStrength)
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
                    var strengthMax = Group.Productions.GetStrengthMax(AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall));
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
        if (_fortifyLocation != MapCoordinate.None)
        {
            result = (City)StrategicMap.Current.MapContainer.GetFieldMapCoordinate(_fortifyLocation).FindNoneTransparentHexSprite(typeof(City));
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
        var info = query.Info;
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
        BattleFieldInfo.SetupBattleFieldInfo(out info, Group.Player, Group.IndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents), AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall));
        info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
        query.Info = info;
        message.MessageId = 4360;
        var param = message.Param;
        param[0] = query;
        message.Param = param;
        for (int i = settings.ProcessedMapFields.Count - 1; i >= 0; i--)
        {
            var mapField = settings.ProcessedMapFields[i];
            if ((mapField.FieldFlags & MapField.BattleFieldInfoFlag) != 0)
            {
                mapField.SendMapFieldMessage(ref message);
            }
        }

        // Handlers write the query through the message. The strength read here is that write.
        info = query.Info;
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
            _requiredStrength = Round(_requiredStrength * StrategicMap.Current.Day * 0.1);
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
            strength = Round(statistics.TurnInfo.GetTurnInfo(fTurnInfoCount - 1).ArmyStrength * 0.1);
            if (strength > 50000)
            {
                strength = 50000;
            }

            if (StrategicMap.Current.Day < 10)
            {
                strength = Round(strength * StrategicMap.Current.Day * 0.1);
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
        int i = default;
        UnitUserList units = default;
        UnitUserList passive = default;
        AIGroup group = default;
        excess = StrengthMax - _requiredStrength;
        if (excess > 0)
        {
            units = new UnitUserList(null) { Holder = CreationHolder() };
            passive = new UnitUserList(null) { Holder = CreationHolder() };
            units.AddUnits(Group.Units);
            units.SortChildren<Troop>(CompareFortifyUnits);
            i = units.Count;
            while (i != 0 && excess > 0)
            {
                i -= 1;
                if (!(units.GetUnit(i) is Leader))
                {
                    excess -= units.GetUnit(i).GetStrengthMax(AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                    if (excess >= 0)
                    {
                        passive.AddUnit(units.GetUnit(i));
                        units.DeleteUnit(units.GetUnit(i));
                    }
                }
            }

            if (passive.Count != 0)
            {
                group = StrategicMap.Current.AIGroupManager.CreateGroup(Group.Player, Group.IndependentRelation, GroupBehavior.Passive);
                group.AddUnits(passive.Units);
                group.Release();
            }

            passive?.Dispose();
            units?.Dispose();
        }
    }

    // MainAG.pas:391
    public bool CanBreachWall()
    {
        int i;
        Troop troop;
        i = Group.Units.Count;
        bool result = Group.Units.GetStrength(DamageType.Wall) != 0;
        while (!result && i != 0)
        {
            i -= 1;
            troop = Group.Units.GetUnit(i);
            result = troop.GetAbilityEnabled(52) && ((SpellCastingAbility)GamePack.Current.AbilityControl.GetAbility(52)).CanCastBreachWallSpell(troop);
        }

        return result;
    }

    private static int CompareFortifyUnits(Troop item1, Troop item2)
    {
        int number = item1.GetStrengthMax(AbilityRules.AllDamageTypes - Set<DamageType>.Of(new DamageType[1] { DamageType.Wall }));
        int number2 = item2.GetStrengthMax(AbilityRules.AllDamageTypes - Set<DamageType>.Of(new DamageType[1] { DamageType.Wall }));
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
