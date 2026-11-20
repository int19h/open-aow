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
using static Aow.Rtl.DelphiRounding;

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
        _ = _targetSelector.Release();
        base.Dispose();
    }

    // MainAG.pas:256
    public FortifyAIGroupControl(CustomNode owner) : base(owner) => _targetSelector = new AIGroupTargetSelector(null) { Holder = CreationHolder() };

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
            case AITargetMode.Raid:
            case AITargetMode.Recruit:
            case AITargetMode.Produce:
            case AITargetMode.Hunt:
            case AITargetMode.Defend:
            case AITargetMode.GuardArea:
            default:
                break;
        }
    }

    // MainAG.pas:407
    public override void ProcessStates()
    {
        AIGroupTargetFinder finder;
        AIProductionLocationFinder productionFinder;
        AIGroupRecruitControl recruit;
        AIGroupDefenseControl defense;
        int i;
        City city;
        switch (State)
        {
            case 0:
                finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                _ = finder.Setup(this);
                AITargetInfo targetInfo = finder.Selector.TargetInfo;
                targetInfo.Mode = AITargetMode.Fortify;
                finder.Selector.TargetInfo = targetInfo;
                finder.Mode += Set<AIGroupTargetFinderModeElement>.Of(AIGroupTargetFinderModeElement.CalculateMovePathBfs);
                finder.Range = 12;
                ActivatePlugin(finder);
                _ = finder.Release();
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
                    int surroundingEnemyStrength = GetSurroundingEnemyStrength(Targets.GetTargets(i).Location, 70);
                    Targets.GetTargets(i).DataA = surroundingEnemyStrength;
                    AIGroupControlTarget target = Targets.GetTargets(i);
                    Targets.GetTargets(i).Status = target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Done);
                }
                else
                {
                    State += 1;
                }

                break;
            case 2:
                _ = Targets.Validate();
                AIGroupControlTarget newTarget = null;
                i = Targets.IndexOfMapCoordinate(Location);
                AIGroupControlTarget currentTarget = i == -1 ? null : Targets.GetTargets(i);

                {
                    i = 0;
                    int last = Targets.Count - 1;
                    for (; i <= last; i += 1)
                    {
                        AIGroupControlTarget targetData = Targets.GetTargets(i);
                        AIGroupControlTarget targetMovePoints = Targets.GetTargets(i);
                        Targets.GetTargets(i).DataB = (targetData.Data.Priority * 10) + (targetMovePoints.MovePoints * -2);
                        int excess = Targets.GetTargets(i).DataA - StrengthMax;
                        if (excess > 0)
                        {
                            AIGroupControlTarget target = Targets.GetTargets(i);
                            target.DataB -= (int)(((uint)excess) >> 4);
                        }

                        excess = Targets.GetTargets(i).PathStrength;
                        if (excess > 0)
                        {
                            AIGroupControlTarget target = Targets.GetTargets(i);
                            target.DataB -= (int)(((uint)excess) >> 3);
                        }

                        if (Targets.GetTargets(i).Location != Location)
                        {
                            AIGroupControlTarget targetLocation = Targets.GetTargets(i);
                            ArmyHexSprite army = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(targetLocation.Location).GetArmyHexSprite();
                            if (army is not null)
                            {
                                int count = army.Army.Units.Count;
                                AIGroupControlTarget target = Targets.GetTargets(i);
                                target.DataB -= count << 2;
                            }
                        }
                    }
                }

                Targets.SortOnPriority();
                if (Targets.Count != 0 && Targets.GetTargets(0) != currentTarget)
                {
                    Targets.SortOnDataB();
                    bool condition = currentTarget is null;
                    if (!condition)
                    {
                        AIGroupControlTarget target = Targets.GetTargets(0);
                        condition = currentTarget.Data.Priority < target.Data.Priority;
                    }

                    newTarget = condition ? Targets.GetTargets(0) : currentTarget;
                }

                _fortifyLocation = newTarget is not null && newTarget != currentTarget
                    ? newTarget.Location
                    : currentTarget is not null ? currentTarget.Location : MapCoordinate.None;

                State += 1;
                break;
            case 3:
                _enemyStrength = _fortifyLocation == MapCoordinate.None
                    ? GetSurroundingEnemyStrength(Location, 70)
                    : GetSurroundingEnemyStrength(_fortifyLocation, 70);

                UpdateRequiredStrength();
                State += 1;
                break;
            case 4:
                AIRecruitmentGroupFinder recruitment = new(null) { Holder = CreationHolder() };
                _ = recruitment.Setup(this);
                ActivatePlugin(recruitment);
                _ = recruitment.Release();
                State += 1;
                break;
            case 5:
                ProductionLocations.Clear();
                productionFinder = new AIProductionLocationFinder(null) { Holder = CreationHolder() };
                _ = productionFinder.Setup(this);
                productionFinder.Selector.MovePointsMax = 24;
                ActivatePlugin(productionFinder);
                _ = productionFinder.Release();
                State += 1;
                break;
            case 6:
                if (RecruitmentGroups.Count != 0 && _requiredStrength > StrengthMax)
                {
                    recruit = new AIGroupRecruitControl(null) { Holder = CreationHolder() };
                    _ = recruit.Setup(this);
                    int strengthMax = Group.Productions.GetStrengthMax(AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                    recruit.RecruitStrength = _requiredStrength - (strengthMax + StrengthMax);
                    recruit.Mode = AIGroupRecruitControlMode.AutoRecruit;
                    ActivatePlugin(recruit);
                    _ = recruit.Release();
                    State += 1;
                }
                else
                {
                    State = 10;
                }

                break;
            case 7:
                _ = RecruitmentGroups.Validate(this);
                if (StrengthMax < _requiredStrength && RecruitmentGroups.Count != 0)
                {
                    recruit = new AIGroupRecruitControl(null) { Holder = CreationHolder() };
                    _ = recruit.Setup(this);
                    int strengthMax = Group.Productions.GetStrengthMax(AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                    recruit.RecruitStrength = _requiredStrength - (strengthMax + StrengthMax);
                    recruit.Mode = AIGroupRecruitControlMode.ForceRecruit;
                    ActivatePlugin(recruit);
                    _ = recruit.Release();
                }

                State = 10;
                break;
            case 10:
                if (StrengthMax >= _enemyStrength && Group.GetAdjacent())
                {
                    Targets.Clear();
                    finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                    _ = finder.Setup(this);
                    AITargetInfo targetInfo2 = finder.Selector.TargetInfo;
                    targetInfo2.Mode = AITargetMode.AttackArmies;
                    finder.Selector.TargetInfo = targetInfo2;
                    finder.Selector.MovePointsMax = 55;
                    finder.Selector.SourceLocation = _fortifyLocation == MapCoordinate.None ? Location : _fortifyLocation;

                    finder.Mode += Set<AIGroupTargetFinderModeElement>.Of(AIGroupTargetFinderModeElement.CalculateMovePathBfs);
                    finder.Range = 12;
                    ActivatePlugin(finder);
                    _ = finder.Release();
                    State += 1;
                }
                else
                {
                    State = StrengthMax < _enemyStrength ? 18 : 17;
                }

                break;
            case 11:
                if (Targets.Count >= 1)
                {
                    if (Targets.Count == 1 || FortifyCity() is null || CanBreachWall())
                    {
                        _ = _targetSelector.Setup(this);
                        _targetSelector.PathStrengthFactor = (float)1.5;
                        _targetSelector.CurrentPathStrengthFactor = (float)1.3;
                        _targetSelector.Superiority = (float)1.6;
                        _targetSelector.NearSuperiority = (float)1.4;
                        _targetSelector.Mode -= Set<AIGroupTargetingModes>.Of(AIGroupTargetingModes.MultiTargets);
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
                city = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(Location).FindNoneTransparentHexSprite<City>();
                if (city is not null && city.GetActiveAction() == CityAction.Migrate)
                {
                    defense = new AIGroupDefenseControl(null)
                    {
                        Holder = CreationHolder(),
                        DefendMode = AIGroupDefenseControl.GroupDefendMode.Custom,
                        CustomState = 19
                    };
                    ActivatePlugin(defense);
                    _ = defense.Release();
                }

                State += 1;
                break;
            case 14:
                Targets.Clear();
                finder = new AIGroupTargetFinder(null) { Holder = CreationHolder() };
                _ = finder.Setup(this);
                AITargetInfo targetInfo3 = finder.Selector.TargetInfo;
                targetInfo3.Mode = AITargetMode.Attack;
                finder.Selector.TargetInfo = targetInfo3;
                finder.Selector.MovePointsMax = 55;
                finder.Selector.SourceLocation = _fortifyLocation == MapCoordinate.None ? Location : _fortifyLocation;

                finder.Mode += Set<AIGroupTargetFinderModeElement>.Of(AIGroupTargetFinderModeElement.CalculateMovePathBfs);
                finder.Range = 12;
                ActivatePlugin(finder);
                _ = finder.Release();
                State += 1;
                break;
            case 15:
                if (Targets.Count >= 1)
                {
                    _ = _targetSelector.Setup(this);
                    _targetSelector.PathStrengthFactor = (float)1.5;
                    _targetSelector.CurrentPathStrengthFactor = (float)1.3;
                    _targetSelector.Superiority = (float)1.6;
                    _targetSelector.NearSuperiority = (float)1.4;
                    _targetSelector.Mode -= Set<AIGroupTargetingModes>.Of(AIGroupTargetingModes.MultiTargets);
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
                    city = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(Location).FindNoneTransparentHexSprite<City>();
                    if (city is not null && city.GetActiveAction() == CityAction.Migrate)
                    {
                        defense = new AIGroupDefenseControl(null)
                        {
                            Holder = CreationHolder(),
                            DefendMode = AIGroupDefenseControl.GroupDefendMode.Custom,
                            CustomState = 19
                        };
                        ActivatePlugin(defense);
                        _ = defense.Release();
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
                if (FortifyCity() is not null)
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
                if (city is not null && city.Production.Status == ProductionControlStatus.Idle)
                {
                    if (city.WallType == 0)
                    {
                        int fortifyCost = city.FortifyCost();
                        if (StrategicMap.Current.Players.GetPlayers(Group.Player).AIBudget.RequestExpense(fortifyCost, 0))
                        {
                            if (city.Fortify(false))
                            {
                                int aCityFortifyCost = city.FortifyCost();
                                _ = StrategicMap.Current.Players.GetPlayers(Group.Player).AIBudget.ExpenseMade(aCityFortifyCost, 0);
                            }
                        }
                    }
                    else
                    {
                        if (city.WallType == 1 && StrategicMap.Current.Day > 29)
                        {
                            int fortifyCost = city.FortifyCost();
                            if (StrategicMap.Current.Players.GetPlayers(Group.Player).AIBudget.RequestExpense(fortifyCost, 0))
                            {
                                if (city.Fortify(false))
                                {
                                    int aCityFortifyCost = city.FortifyCost();
                                    _ = StrategicMap.Current.Players.GetPlayers(Group.Player).AIBudget.ExpenseMade(aCityFortifyCost, 0);
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
                    _ = productionFinder.Setup(this);
                    productionFinder.Selector.MovePointsMax = 24;
                    ActivatePlugin(productionFinder);
                    _ = productionFinder.Release();
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
                    AIGroupProductionControl produce = new(null) { Holder = CreationHolder() };
                    _ = produce.Setup(this);
                    int strengthMax = Group.Productions.GetStrengthMax(AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                    produce.ProduceStrength = _requiredStrength - (strengthMax + StrengthMax);
                    produce.Mode = AIGroupProductionControlMode.AutoProduce;
                    ActivatePlugin(produce);
                    _ = produce.Release();
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
                    case AIGroupTargetStatus.Setup:
                    case AIGroupTargetStatus.Execute:
                    default:
                        break;
                }

                State += 1;
                break;
            case 25:
                Done();
                break;
            default:
                break;
        }
    }

    // MainAG.pas:275
    public override bool Split() => false;

    // MainAG.pas:268
    public City FortifyCity()
    {
        return _fortifyLocation != MapCoordinate.None
            ? StrategicMap.Current.MapContainer.GetFieldMapCoordinate(_fortifyLocation).FindNoneTransparentHexSprite<City>()
            : null;
    }

    // MainAG.pas:287
    public int GetSurroundingEnemyStrength(MapCoordinate location, int radius)
    {
        MapFieldMessage message = default;
        BattleFieldQuery query = new();
        _ = query.Info;
        MoveControlSettings settings = new(null)
        {
            Holder = CreationHolder(),
            Player = Group.Player
        };
        settings.Mode += Set<MoveModeElement>.Of(MoveModeElement.LogProcessedMapFields, MoveModeElement.AI);
        settings.SetSourcePosition(location);
        settings.MovePointsMax = radius;
        settings.ArmyHexSprite = null;
        settings.Selection = 0;
        int moveTypesToInt32 = MovementTables.MoveTypesToInt32(Set<MoveType>.Of(MoveType.Flying));
        settings.MovePointTable = MovementTables.MovePointTables[moveTypesToInt32];
        StrategicMap.Current.MoveControl.ProcessMove(settings);
        BattleFieldInfo.SetupBattleFieldInfo(out BattleFieldInfo info, Group.Player, Group.IndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents), AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall));
        info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
        query.Info = info;
        message.MessageId = 4360;
        MapFieldMessageParameters param = message.Param;
        param[0] = query;
        message.Param = param;
        for (int i = settings.ProcessedMapFields.Count - 1; i >= 0; i--)
        {
            MapField mapField = settings.ProcessedMapFields[i];
            if ((mapField.FieldFlags & MapField.BattleFieldInfoFlag) != 0)
            {
                mapField.SendMapFieldMessage(ref message);
            }
        }

        // Handlers write the query through the message. The strength read here is that write.
        info = query.Info;
        int result = info.Strength;
        _ = settings.Release();
        return result;
    }

    // MainAG.pas:315
    public void UpdateRequiredStrength()
    {
        _requiredStrength = _enemyStrength;
        int strength;
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

        PlayerStatistics statistics = StrategicMap.Current.Players.GetPlayers(Group.Player).Statistics;
        if (statistics.TurnInfo.Count > 0)
        {
            int count = statistics.TurnInfo.Count;
            strength = (int)(((uint)statistics.TurnInfo.GetTurnInfo(count - 1).ArmyStrength) >> 1);
            if (strength < _requiredStrength)
            {
                _requiredStrength = strength;
            }

            int fTurnInfoCount = statistics.TurnInfo.Count;
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
        int excess = StrengthMax - _requiredStrength;
        if (excess > 0)
        {
            UnitUserList units = new(null) { Holder = CreationHolder() };
            UnitUserList passive = new(null) { Holder = CreationHolder() };
            _ = units.AddUnits(Group.Units);
            units.SortChildren<Troop>(CompareFortifyUnits);
            int i = units.Count;
            while (i != 0 && excess > 0)
            {
                i -= 1;
                if (units.GetUnit(i) is not Leader)
                {
                    excess -= units.GetUnit(i).GetStrengthMax(AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall));
                    if (excess >= 0)
                    {
                        _ = passive.AddUnit(units.GetUnit(i));
                        _ = units.DeleteUnit(units.GetUnit(i));
                    }
                }
            }

            if (passive.Count != 0)
            {
                AIGroup group = StrategicMap.Current.AIGroupManager.CreateGroup(Group.Player, Group.IndependentRelation, GroupBehavior.Passive);
                _ = group.AddUnits(passive.Units);
                _ = group.Release();
            }

            passive?.Dispose();
            units?.Dispose();
        }
    }

    // MainAG.pas:391
    public bool CanBreachWall()
    {
        int i = Group.Units.Count;
        bool result = Group.Units.GetStrength(DamageType.Wall) != 0;
        while (!result && i != 0)
        {
            i -= 1;
            Troop troop = Group.Units.GetUnit(i);
            result = troop.GetAbilityEnabled(52) && GamePack.Current.AbilityControl.GetAbility<SpellCastingAbility>(52).CanCastBreachWallSpell(troop);
        }

        return result;
    }

    private static int CompareFortifyUnits(Troop item1, Troop item2)
    {
        int number = item1.GetStrengthMax(AbilityRules.AllDamageTypes - Set<DamageType>.Of([DamageType.Wall]));
        int number2 = item2.GetStrengthMax(AbilityRules.AllDamageTypes - Set<DamageType>.Of([DamageType.Wall]));
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
