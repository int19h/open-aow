#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// status, plugin, recruitment, production, cooperation, mode, minimum moves, target, priority,
// spare units, next target, battlefield factors, radius, damage types, superiority, move-day
// factor, minimum strength, split factor, path factors, target limit, transport, valid limit,
// priority settings
public partial class AIGroupTargetSelector : AIGroupTask
{
    // AIGroup.pas:3041
    public AIGroupTargetSelector(CustomNode owner) : base(owner)
    {
        _recruitment = new AIGroupRecruitControl(null) { Holder = CreationHolder() };
        _production = new AIGroupProductionControl(null) { Holder = CreationHolder() };
        _cooperation = new AIGroupCooperationControl(null) { Holder = CreationHolder() };
        Priorities = new PrioritySettings();
        _spareUnits = new UnitUserList(null) { Holder = CreationHolder() };
    }

    public AIGroupTargetingStatus TargetStatus { get; private set; }

    private AIGroupTask _aiGroupTask;
    private readonly AIGroupRecruitControl _recruitment;
    private readonly AIGroupProductionControl _production;
    private readonly AIGroupCooperationControl _cooperation;

    public Set<AIGroupTargetingModes> Mode { get; set; }

    private int _movesMin;
    private AIGroupControlTarget _target;
    private int _priority;
    private readonly UnitUserList _spareUnits;
    private AIGroupControlTarget _nextTarget;

    public float BattleFieldFactor { get; set; }
    public float CurrentBattleFieldFactor { get; set; }

    private float _battleFieldMoveFactor;
    private int _battleFieldRadius;
    private Set<DamageType> _damageTypes;

    public float Superiority { get; set; }
    public float NearSuperiority { get; set; }
    public float MoveDayFactor { get; set; }
    public int MinimumStrength { get; set; }
    public float MultiTargetFactor { get; set; }
    public float PathStrengthFactor { get; set; }
    public float CurrentPathStrengthFactor { get; set; }

    private int _targetLimit;

    private int _validTargetLimit;

    public PrioritySettings Priorities { get; }

    // AIGroup.pas:3051
    public override void Dispose()
    {
        DeactivatePlugin();
        _recruitment.Release();
        _production.Release();
        _cooperation.Release();
        _spareUnits?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:3421
    public override void Validate()
    {
        Troop troop;
        for (int i = _spareUnits.Count - 1; i >= 0; i--)
        {
            troop = _spareUnits.GetUnit(i);
            if (GroupControl.Group.Units.IndexOf(troop) == -1)
            {
                _spareUnits.DeleteUnit(troop);
            }
        }

        GroupControl.Targets.Validate();
        if (State != 21 && _target != null)
        {
            if (GroupControl.Targets.IndexOfChild(_target) == -1)
            {
                _target = null;
                if (State > 0 && State < 10)
                {
                    State = 0;
                }

                if (State > 10)
                {
                    State = 10;
                }
            }
        }
    }

    // AIGroup.pas:3062
    public override bool GetBusy() => State != -1;

    // AIGroup.pas:3228
    public override bool Activate(AIGroupControl control)
    {
        AIGroupControlTarget target = default;
        bool valid = default;
        GroupControl = control;
        GroupControl.Targets.Validate();
        _movesMin = 50;
        for (int i = GroupControl.Group.Units.Count - 1; i >= 0; i--)
        {
            if (GroupControl.Group.Units.GetUnit(i).GetMoves() < _movesMin)
            {
                _movesMin = GroupControl.Group.Units.GetUnit(i).GetMoves();
            }
        }

        for (int i = GroupControl.Targets.Count - 1; i >= 0; i--)
        {
            target = GroupControl.Targets.GetTargets(i);
            valid = ValidateTargetOverlap(GroupControl.Group, target);
            bool condition = valid;
            if (condition)
            {
                bool test = target.Data.TargetType != AITargetType.Negotiate;
                if (!test)
                {
                    int targetMana = target.GetTargetMana();
                    int targetGold = target.GetTargetGold();
                    test = GroupControl.Group.RequestTargetExpense(targetGold, targetMana);
                }

                condition = test;
            }

            valid = condition;
            bool passed = valid;
            if (passed)
            {
                bool test = target.GetTargetGold() <= 0;
                if (!test)
                {
                    Player players = StrategicMap.Current.Players.GetPlayers(GroupControl.Group.Player);
                    int targetGold = target.GetTargetGold();
                    test = players.Gems >= targetGold;
                }

                passed = test;
            }

            valid = passed;
            bool condition2 = valid;
            if (condition2)
            {
                bool test = target.GetTargetMana() <= 0;
                if (!test)
                {
                    Player players = StrategicMap.Current.Players.GetPlayers(GroupControl.Group.Player);
                    int targetMana = target.GetTargetMana();
                    test = players.MagicControl.Mana >= targetMana;
                }

                condition2 = test;
            }

            valid = condition2;
            if (valid)
            {
                target.DataA = GetTargetSetupPriority(GroupControl.Targets.GetTargets(i));
                target.Status = Set<AIGroupTargetFlags>.Of();
            }
            else
            {
                target.Status = Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Done);
            }
        }

        GroupControl.Targets.SortChildren<AIGroupControlTarget>(CompareTargetSetupPriority);
        for (int i = GroupControl.Targets.Count - 1, last = _targetLimit + 1; i >= last; i--)
        {
            GroupControl.Targets.Delete(i);
        }

        _target = null;
        TargetStatus = AIGroupTargetingStatus.Busy;
        return true;
    }

    // AIGroup.pas:3487
    public override bool Process()
    {
        bool result = default;
        int i = default;
        int strength = default;
        int superiority = default;
        int time = default;
        bool busy = default;
        AIGroupTask aiGroupTask = default;
        UnitUserList optional = default;
        UnitUserList required = default;
        AIGroup aiGroup = default;
        MapCoordinate location = default;
        aiGroupTask = _aiGroupTask;
        if (aiGroupTask != null)
        {
            busy = aiGroupTask.MainProcess();
            if (!busy)
            {
                DeactivatePlugin();
            }
        }
        else
        {
            busy = false;
        }

        if (!busy)
        {
            result = false;
            switch (State)
            {
                case 0:
                    i = 0;
                    while (i != GroupControl.Targets.Count && GroupControl.Targets.GetTargets(i).Status.Contains(AIGroupTargetFlags.Done))
                    {
                        i += 1;
                    }

                    if (i < GroupControl.Targets.Count && GetValidTargets() < _validTargetLimit)
                    {
                        _target = GroupControl.Targets.GetTargets(i);
                        _priority = _target.DataA;
                        strength = GetRequiredStrength(_target);
                        superiority = GetRequiredCombatPredictorSuperiority(_target);
                        StrategicMap.Current.AIExecutor.UseProcessTime(100);
                        busy = GroupControl.StrengthMax < strength;
                        if (!busy)
                        {
                            GroupControl.SetupCP(_target);
                            GroupControl.CombatPredictor.Execute();
                            busy = busy || Set<byte>.WithBounds(0, 255).With(2).With(4).With(8).Contains(GroupControl.CombatPredictor.Result);
                            busy = busy || GroupControl.CombatPredictor.GetSuperiority() < superiority;
                        }

                        if (busy)
                        {
                            if ((Mode & Set<AIGroupTargetingModes>.Of(AIGroupTargetingModes.Cooperate, AIGroupTargetingModes.Recruit, AIGroupTargetingModes.Produce)) != Set<AIGroupTargetingModes>.Of())
                            {
                                GroupControl.SetupCP(_target);
                                if (Mode.Contains(AIGroupTargetingModes.Cooperate))
                                {
                                    _cooperation.Setup(GroupControl, _target);
                                    _cooperation.Superiority = superiority;
                                    _cooperation.CooperateStrength = strength - GroupControl.StrengthMax;
                                    _cooperation.Mode = AIGroupCooperationControl.CooperationMode.None;
                                    ActivatePlugin(_cooperation);
                                    State = 1;
                                }
                                else
                                {
                                    if (Mode.Contains(AIGroupTargetingModes.Recruit))
                                    {
                                        State = 2;
                                    }
                                    else
                                    {
                                        if (Mode.Contains(AIGroupTargetingModes.Produce))
                                        {
                                            State = 4;
                                        }
                                        else
                                        {
                                            TargetProcessed();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                TargetProcessed();
                            }
                        }
                        else
                        {
                            _target.Status = _target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid);
                            int movePointsMin = GroupControl.Group.GetMovePointsMin();
                            if (_target.MovePoints < movePointsMin)
                            {
                                _priority += Priorities.ImmediateBonus;
                            }

                            TargetProcessed();
                        }
                    }
                    else
                    {
                        GroupControl.Targets.SortChildren<AIGroupControlTarget>(CompareTargetSetupPriority);
                        State = 10;
                    }

                    break;
                case 1:
                    if (_cooperation.Complete)
                    {
                        _target.Status = _target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid, AIGroupTargetFlags.Cooperate);
                        _priority += Priorities.CompleteCooperationPenalty;
                        bool condition = _cooperation.GetCooperateMoveDays() == 0;
                        if (condition)
                        {
                            int movePointsMin = GroupControl.Group.GetMovePointsMin();
                            condition = _target.MovePoints < movePointsMin;
                        }

                        if (condition)
                        {
                            _priority += Priorities.ImmediateBonus;
                        }

                        TargetProcessed();
                    }
                    else
                    {
                        if (_cooperation.Groups.Count != 0)
                        {
                            _target.Status = _target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid, AIGroupTargetFlags.Cooperate);
                            _priority += Priorities.PartialCooperationPenalty;
                        }

                        if (Mode.Contains(AIGroupTargetingModes.Recruit))
                        {
                            State = 2;
                        }
                        else
                        {
                            if (Mode.Contains(AIGroupTargetingModes.Produce))
                            {
                                State = 4;
                            }
                            else
                            {
                                TargetProcessed();
                            }
                        }
                    }

                    break;
                case 2:
                    _recruitment.Setup(GroupControl);
                    int requiredCombatPredictorSuperiority = GetRequiredCombatPredictorSuperiority(_target);
                    _recruitment.Superiority = requiredCombatPredictorSuperiority;
                    int requiredStrength = GetRequiredStrength(_target);
                    _recruitment.RecruitStrength = requiredStrength - GroupControl.StrengthMax;
                    if (_target.Status.Contains(AIGroupTargetFlags.Cooperate))
                    {
                        int cooperatedStrength = _cooperation.GetCooperatedStrength();
                        _recruitment.RecruitStrength -= cooperatedStrength;
                    }

                    _recruitment.Mode = AIGroupRecruitControlMode.None;
                    ActivatePlugin(_recruitment);
                    State = 3;
                    break;
                case 3:
                    if (_recruitment.Complete)
                    {
                        _target.Status = _target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid, AIGroupTargetFlags.Recruit);
                        _priority += Priorities.CompleteRecruitmentPenalty;
                        TargetProcessed();
                    }
                    else
                    {
                        if (_recruitment.Units.Count != 0)
                        {
                            _target.Status = _target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Recruit);
                            _priority += Priorities.PartialRecruitmentPenalty;
                        }

                        if (Mode.Contains(AIGroupTargetingModes.Produce))
                        {
                            State = 4;
                        }
                        else
                        {
                            TargetProcessed();
                        }
                    }

                    break;
                case 4:
                    _production.Setup(GroupControl);
                    int requiredCombatPredictorSuperiority2 = GetRequiredCombatPredictorSuperiority(_target);
                    _production.Superiority = requiredCombatPredictorSuperiority2;
                    int requiredStrength2 = GetRequiredStrength(_target);
                    _production.ProduceStrength = requiredStrength2 - GroupControl.StrengthMax;
                    if (_target.Status.Contains(AIGroupTargetFlags.Cooperate))
                    {
                        int cooperatedStrength = _cooperation.GetCooperatedStrength();
                        _production.ProduceStrength -= cooperatedStrength;
                    }

                    if (_target.Status.Contains(AIGroupTargetFlags.Recruit))
                    {
                        int recruitedStrength = _recruitment.GetRecruitedStrength();
                        _production.ProduceStrength -= recruitedStrength;
                    }

                    _production.Mode = AIGroupProductionControlMode.None;
                    ActivatePlugin(_production);
                    State = 5;
                    break;
                case 5:
                    if (_production.Complete)
                    {
                        _target.Status = _target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid, AIGroupTargetFlags.Produce);
                        time = _production.GetCompleteProductionTime();
                        if (time > 0)
                        {
                            _priority += Round(Priorities.ProductionDayPenalty * time);
                        }
                    }
                    else
                    {
                        if (_production.Productions.Count != 0)
                        {
                            _target.Status = _target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid, AIGroupTargetFlags.Produce);
                            _priority += Priorities.PartialProductionPenalty;
                            time = _production.GetCompleteProductionTime();
                            if (time > 0)
                            {
                                _priority += Priorities.ProductionDayPenalty * time;
                            }
                        }
                    }

                    TargetProcessed();
                    break;
                case 10:
                    i = 0;
                    while (i != GroupControl.Targets.Count && !GroupControl.Targets.GetTargets(i).Status.Contains(AIGroupTargetFlags.Valid))
                    {
                        i += 1;
                    }

                    if (i == GroupControl.Targets.Count)
                    {
                        NoTargetsFound();
                        Done();
                    }
                    else
                    {
                        _target = GroupControl.Targets.GetTargets(i);
                        if ((_target.Status & Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Recruit, AIGroupTargetFlags.Produce, AIGroupTargetFlags.Cooperate)) != Set<AIGroupTargetFlags>.Of())
                        {
                            GroupControl.SetupCP(_target);
                            if (_target.Status.Contains(AIGroupTargetFlags.Cooperate))
                            {
                                _cooperation.Setup(GroupControl, _target);
                                int requiredCombatPredictorSuperiority3 = GetRequiredCombatPredictorSuperiority(_target);
                                _cooperation.Superiority = requiredCombatPredictorSuperiority3;
                                int requiredStrength3 = GetRequiredStrength(_target);
                                _cooperation.CooperateStrength = requiredStrength3 - GroupControl.StrengthMax;
                                _cooperation.Mode = AIGroupCooperationControl.CooperationMode.AutoCooperate;
                                ActivatePlugin(_cooperation);
                                State = 11;
                            }
                            else
                            {
                                if (_target.Status.Contains(AIGroupTargetFlags.Recruit))
                                {
                                    State = 12;
                                }
                                else
                                {
                                    if (_target.Status.Contains(AIGroupTargetFlags.Produce))
                                    {
                                        State = 14;
                                    }
                                    else
                                    {
                                        State = 20;
                                    }
                                }
                            }
                        }
                        else
                        {
                            _target.Status = _target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Ready);
                            State = 20;
                        }
                    }

                    break;
                case 11:
                    if (_cooperation.Complete)
                    {
                        _target.Status = _target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Ready);
                        State = 20;
                    }
                    else
                    {
                        if (_target.Status.Contains(AIGroupTargetFlags.Recruit))
                        {
                            State = 12;
                        }
                        else
                        {
                            if (_target.Status.Contains(AIGroupTargetFlags.Produce))
                            {
                                State = 14;
                            }
                            else
                            {
                                State = 20;
                            }
                        }
                    }

                    break;
                case 12:
                    _recruitment.Setup(GroupControl);
                    int requiredCombatPredictorSuperiority4 = GetRequiredCombatPredictorSuperiority(_target);
                    _recruitment.Superiority = requiredCombatPredictorSuperiority4;
                    int requiredStrength4 = GetRequiredStrength(_target);
                    _recruitment.RecruitStrength = requiredStrength4 - GroupControl.StrengthMax;
                    _recruitment.Mode = AIGroupRecruitControlMode.AutoRecruit;
                    ActivatePlugin(_recruitment);
                    State += 1;
                    break;
                case 13:
                    if (_recruitment.Complete)
                    {
                        _target.Status = _target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Ready);
                        State = 20;
                    }
                    else
                    {
                        if (_target.Status.Contains(AIGroupTargetFlags.Produce))
                        {
                            State = 14;
                        }
                        else
                        {
                            State = 20;
                        }
                    }

                    break;
                case 14:
                    _production.Setup(GroupControl);
                    int requiredCombatPredictorSuperiority5 = GetRequiredCombatPredictorSuperiority(_target);
                    _production.Superiority = requiredCombatPredictorSuperiority5;
                    int requiredStrength5 = GetRequiredStrength(_target);
                    _production.ProduceStrength = requiredStrength5 - GroupControl.StrengthMax;
                    _production.Mode = AIGroupProductionControlMode.AutoProduce;
                    ActivatePlugin(_production);
                    State = 20;
                    break;
                case 20:
                    if (_target.Status.Contains(AIGroupTargetFlags.Ready))
                    {
                        if (_target.TransporterId != -1)
                        {
                            Troop troop = StrategicMap.Current.UnitControl.FindUnit(_target.TransporterId);
                            GroupControl.Group.AddTransporter(troop);
                        }
                        else
                        {
                            AIGroup group = GroupControl.Group;
                            if (group.Transporter != null)
                            {
                                group.DeleteUnit(group.Transporter);
                            }
                        }

                        if (AllowMultiTargets(_target))
                        {
                            State = 30;
                        }
                        else
                        {
                            GroupControl.Group.SetupTarget(_target, AIGroupTargetStatus.Execute, 0);
                            GroupControl.SetupME();
                            GroupControl.ExecuteME(_target.Location.X, _target.Location.Y, _target.Location.MapLevel);
                            result = true;
                            State = 21;
                        }
                    }
                    else
                    {
                        if (GroupControl.Group.Productions.Count != 0)
                        {
                            MapCoordinate coordinate101 = GroupControl.Group.Productions.GetProductions(0).Control.GetLocation();
                            sbyte locationX = coordinate101.X;
                            sbyte locationY = coordinate101.Y;
                            sbyte locationMapLevel = coordinate101.MapLevel;
                            location = new MapCoordinate(locationX, locationY, locationMapLevel);
                            GroupControl.SetupME();
                            GroupControl.ExecuteME(location.X, location.Y, location.MapLevel);
                            result = true;
                            State = 22;
                        }
                        else
                        {
                            GroupControl.ProductionLocations.SortOnMovePoints();
                            if (GroupControl.ProductionLocations.Count == 0 || GroupControl.ProductionLocations.GetProductionLocation(0).MovePoints > 31)
                            {
                                GroupControl.SetupME();
                                GroupControl.ExecuteME(GroupControl.Location.X, GroupControl.Location.Y, GroupControl.Location.MapLevel);
                                result = true;
                                State = 22;
                            }
                            else
                            {
                                GroupControl.SetupME();
                                AIGroupProductionLocation productionLocation = GroupControl.ProductionLocations.GetProductionLocation(0);
                                AIGroupProductionLocation fProductionLocationsProductionLocation = GroupControl.ProductionLocations.GetProductionLocation(0);
                                AIGroupProductionLocation productionLocation2 = GroupControl.ProductionLocations.GetProductionLocation(0);
                                GroupControl.ExecuteME(productionLocation2.Location.X, fProductionLocationsProductionLocation.Location.Y, productionLocation.Location.MapLevel);
                                result = true;
                                State = 22;
                            }
                        }
                    }

                    break;
                case 21:
                    if (GroupControl.Targets.IndexOfChild(_target) == -1)
                    {
                        GroupControl.Group.ResetTargetExpense();
                        TargetStatus = AIGroupTargetingStatus.TargetConquered;
                        Done();
                    }
                    else
                    {
                        TargetStatus = AIGroupTargetingStatus.ExecutingTarget;
                        Done();
                    }

                    break;
                case 22:
                    GroupControl.Group.SetupTarget(_target, AIGroupTargetStatus.Setup, 0);
                    TargetStatus = AIGroupTargetingStatus.SettingUpTarget;
                    Done();
                    break;
                case 30:
                    ValidateTargetRequirements(GroupControl.Group.Units, _target, null, _spareUnits, MinimumStrength);
                    if (_spareUnits.Count != 0)
                    {
                        _target.Status = _target.Status - Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid);
                        _nextTarget = null;
                        State = 31;
                    }
                    else
                    {
                        State = 32;
                    }

                    break;
                case 31:
                    i = GroupControl.Targets.IndexOfChild(_nextTarget);
                    while (i < GroupControl.Targets.Count && (i == -1 || GroupControl.Targets.GetTargets(i).TransporterId != -1 || !GroupControl.Targets.GetTargets(i).Status.Contains(AIGroupTargetFlags.Valid)))
                    {
                        i += 1;
                    }

                    if (i < GroupControl.Targets.Count)
                    {
                        required = new UnitUserList(null) { Holder = CreationHolder() };
                        optional = new UnitUserList(null) { Holder = CreationHolder() };
                        _nextTarget = GroupControl.Targets.GetTargets(i);
                        if (ValidMultiTarget(_spareUnits, _nextTarget, required, optional))
                        {
                            AIGroup group = GroupControl.Group;
                            GroupBehavior behavior = group.GroupClass.Behavior;
                            aiGroup = StrategicMap.Current.AIGroupManager.CreateGroup(group.Player, group.IndependentRelation, behavior);

                            aiGroup.AddUnits(required.Units);
                            aiGroup.SetupTarget(_nextTarget, AIGroupTargetStatus.Execute, 0);
                            aiGroup.Release();
                            _spareUnits.Clear();
                            _spareUnits.AddUnits(optional.Units);
                            if (_spareUnits.Count == 0)
                            {
                                State = 32;
                            }
                        }

                        _nextTarget.Status = _nextTarget.Status - Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid);
                        optional?.Dispose();
                        required?.Dispose();
                    }
                    else
                    {
                        State = 32;
                    }

                    break;
                case 32:
                    GroupControl.Group.SetupTarget(_target, AIGroupTargetStatus.Execute, 0);
                    GroupControl.SetupME();
                    GroupControl.ExecuteME(_target.Location.X, _target.Location.Y, _target.Location.MapLevel);
                    State = 21;
                    result = true;
                    break;
                case 40:
                    NoTargetsFound();
                    Done();
                    break;
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AIGroup.pas:3067
    public bool TargetOverlapFound(AIGroupControlTarget target, AIGroup ignoreGroup)
    {
        Player player;
        int i;
        AIGroup group;
        player = StrategicMap.Current.Players.GetPlayers(GroupControl.Group.Player);
        i = player.AIGroups.Count;
        bool result = false;
        while (i != 0 && !result)
        {
            i -= 1;
            group = player.AIGroups.GetGroups(i);
            result = group != ignoreGroup && group.TargetType == AIGroupTargetStatus.Execute && target.Location.MapLevel == group.TargetMapCoordinate.Location.MapLevel && HexGeometry.DeltaHexToRadius(group.TargetMapCoordinate.Location.X, group.TargetMapCoordinate.Location.Y, target.Location.X, target.Location.Y) <= 1;
        }

        return result;
    }

    // AIGroup.pas:3088
    public bool ValidateTargetOverlap(AIGroup group, AIGroupControlTarget target)
    {
        int i;
        int moves;
        int points;
        int otherDays;
        int days;
        Player player;
        AIGroup other;
        player = StrategicMap.Current.Players.GetPlayers(GroupControl.Group.Player);
        i = player.AIGroups.Count;
        bool result = true;
        while (i != 0 && result)
        {
            i -= 1;
            other = player.AIGroups.GetGroups(i);
            if (other != group && other.TargetType == AIGroupTargetStatus.Execute && other.TargetMapCoordinate.Location.MapLevel == target.Location.MapLevel && HexGeometry.DeltaHexToRadius(other.TargetMapCoordinate.Location.X, other.TargetMapCoordinate.Location.Y, target.Location.X, target.Location.Y) <= 1)
            {
                moves = other.GetMovesMin();
                points = other.TargetMovePoints;
                if (!other.Status.Contains(AIGroupStatusElement.Done))
                {
                    points -= other.GetMovePointsMin();
                }

                otherDays = points / moves;
                if (points % moves != 0)
                {
                    otherDays += 1;
                }

                moves = group.GetMovesMin();
                int movePointsMin = group.GetMovePointsMin();
                points = target.MovePoints - movePointsMin;
                if (points > 0)
                {
                    days = points / moves;
                    if (points % moves != 0)
                    {
                        days += 1;
                    }
                }
                else
                {
                    days = 0;
                }

                result = days + 1 <= otherDays;
            }
        }

        return result;
    }

    // AIGroup.pas:3125
    public int GetValidTargets()
    {
        int result = 0;
        for (int i = GroupControl.Targets.Count - 1; i >= 0; i--)
        {
            if (GroupControl.Targets.GetTargets(i).Status.Contains(AIGroupTargetFlags.Valid))
            {
                result += 1;
            }
        }

        return result;
    }

    // AIGroup.pas:3134
    public void Reset()
    {
        State = 0;
        TargetStatus = AIGroupTargetingStatus.Idle;
        Superiority = (float)1.5;
        NearSuperiority = (float)1.25;
        MultiTargetFactor = (float)1.25;
        MoveDayFactor = (float)0.1;
        _spareUnits.Clear();
        Mode = Set<AIGroupTargetingModes>.Of(AIGroupTargetingModes.MultiTargets, AIGroupTargetingModes.Cooperate, AIGroupTargetingModes.Recruit, AIGroupTargetingModes.Produce);
        _damageTypes = AbilityRules.AllDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        BattleFieldFactor = 1;
        CurrentBattleFieldFactor = (float)0.8;
        _battleFieldMoveFactor = 0;
        _battleFieldRadius = 10;
        _validTargetLimit = 5;
        PathStrengthFactor = 1;
        CurrentPathStrengthFactor = (float)0.8;
        _targetLimit = 100;
        Priorities.TargetFactor = 1;
        Priorities.ImmediateBonus = 500;
        Priorities.SetupBonus = 25;
        Priorities.ExecuteBonus = 75;
        Priorities.TransportPenalty = -100;
        Priorities.PartialRecruitmentPenalty = 0;
        Priorities.CompleteRecruitmentPenalty = -50;
        Priorities.CompleteCooperationPenalty = -25;
        Priorities.PartialCooperationPenalty = -50;
        Priorities.MoveDayPenalty = -25;
        Priorities.ProductionDayPenalty = -25;
        Priorities.PartialProductionPenalty = -10;
        Priorities.FarOverlapPenalty = -199;
        Priorities.NearOverlapPenalty = -50;
    }

    // AIGroup.pas:3173
    public void ActivatePlugin(AIGroupTask aiGroupTask)
    {
        DeactivatePlugin();
        _aiGroupTask = aiGroupTask;
        _aiGroupTask.AddRef();
        _aiGroupTask.Activate(GroupControl);
    }

    // AIGroup.pas:3181
    public void DeactivatePlugin()
    {
        if (_aiGroupTask != null)
        {
            _aiGroupTask.Release();
            _aiGroupTask = null;
        }
    }

    // AIGroup.pas:3190
    public int GetTargetSetupPriority(AIGroupControlTarget target)
    {
        AIGroup group;
        int days;
        int priority;
        priority = Round((double)target.Data.Priority * Priorities.TargetFactor);
        int movesMin = GroupControl.Group.GetMovesMin();
        days = (target.MovePoints - 1) / movesMin;
        if (days > 0)
        {
            priority += Round(Priorities.MoveDayPenalty * days);
        }

        if (target.TransporterId != -1)
        {
            priority += Priorities.TransportPenalty;
        }

        group = GroupControl.Group;
        if (group.TargetMapCoordinate == AITargetLocation.At(target.Location))
        {
            if (group.TargetType == AIGroupTargetStatus.Execute)
            {
                priority += Priorities.ExecuteBonus;
            }
            else
            {
                if (group.TargetType == AIGroupTargetStatus.Setup)
                {
                    priority += Priorities.SetupBonus;
                }
            }
        }

        if (TargetOverlapFound(target, group))
        {
            if (target.MovePoints <= _movesMin)
            {
                priority += Priorities.NearOverlapPenalty;
            }
            else
            {
                priority += Priorities.FarOverlapPenalty;
            }
        }

        return priority;
    }

    // AIGroup.pas:3211
    public bool Setup(AIGroupControl control)
    {
        Reset();
        switch ((int)StrategicMap.Current.Players.GetPlayers(control.Group.Player).PlayerType)
        {
            case (int)PlayerType.Cpu1:
                _validTargetLimit = 5;
                break;
            case (int)PlayerType.Cpu2:
                _validTargetLimit = 7;
                break;
            case (int)PlayerType.Cpu3:
            case (int)PlayerType.Cpu4:
            case (int)PlayerType.Cpu5:
                _validTargetLimit = 10;
                break;
            default:
                _validTargetLimit = 5;
                break;
        }

        return true;
    }

    // AIGroup.pas:3265
    public void TargetProcessed()
    {
        _target.DataA = _priority;
        _target.Status = _target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Done);
        _target = null;
        State = 0;
    }

    // AIGroup.pas:3273
    public float GetRequiredSuperiority(AIGroupControlTarget target)
    {
        float result;
        int days;
        if (target.Data.TargetType != AITargetType.Negotiate)
        {
            AIGroup group = GroupControl.Group;
            if ((group.TargetMapCoordinate == AITargetLocation.At(target.Location) && group.TargetType == AIGroupTargetStatus.Execute) || group.GetMovesMin() >= target.MovePoints)
            {
                result = NearSuperiority;
            }
            else
            {
                result = Superiority;
            }

            if (GroupControl.Group.GetMovesMin() > 0)
            {
                int movesMin = GroupControl.Group.GetMovesMin();
                days = (target.MovePoints - 1) / movesMin;
                result = (float)((double)result + Round((double)days * MoveDayFactor));
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AIGroup.pas:3292
    public int GetRequiredCombatPredictorSuperiority(AIGroupControlTarget target) => Round((double)GetRequiredSuperiority(target) * 100);

    // AIGroup.pas:3297
    public int GetRequiredTargetStrength(AIGroupControlTarget target)
    {
        int result;
        DefenseRequirements strengths = default;
        BattleFieldInfo info = default;
        if (target.Data.TargetType != AITargetType.Negotiate)
        {
            BattleFieldInfo.SetupBattleFieldInfo(out info, GroupControl.Group.Player, GroupControl.Group.IndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(), _damageTypes);
            info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
            StrategicMap.Current.RetrieveBattleFieldInfo(target.Location.X, target.Location.Y, target.Location.MapLevel, 1, ref info);
            result = Round((double)GetRequiredSuperiority(target) * info.Strength);
            StrategicMap.Current.MapContainer.GetFieldMapCoordinate(target.Location).GetDefenseRequirements(GroupControl.Group.Player, out strengths);
            Array10<int> damageStrengths = strengths.DamageStrengths;
            result += StrengthInfoMax(damageStrengths) / 2;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AIGroup.pas:3317
    public float GetRequiredBattleFieldSuperiority(AIGroupControlTarget target)
    {
        float result;
        if (AITargetLocation.At(target.Location) == GroupControl.Group.TargetMapCoordinate && GroupControl.Group.TargetType == AIGroupTargetStatus.Execute)
        {
            result = CurrentBattleFieldFactor;
        }
        else
        {
            result = BattleFieldFactor;
        }

        return (float)((double)result + Round((double)target.MovePoints * _battleFieldMoveFactor));
    }

    // AIGroup.pas:3325
    public int GetRequiredBattleFieldStrength(AIGroupControlTarget target)
    {
        int result;
        BattleFieldInfo info = default;
        int pathStrength;
        if (((double)BattleFieldFactor > 0 || (double)CurrentBattleFieldFactor > 0) && target.Data.TargetType != AITargetType.Negotiate)
        {
            BattleFieldInfo.SetupBattleFieldInfo(out info, GroupControl.Group.Player, GroupControl.Group.IndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(), _damageTypes);
            info.Units = GroupControl.Group.Units;
            info.Requests = info.Requests + Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents);
            StrategicMap.Current.RetrieveBattleFieldInfo(target.Location.X, target.Location.Y, target.Location.MapLevel, _battleFieldRadius, ref info);
            result = Round((double)GetRequiredBattleFieldSuperiority(target) * (info.Strength - info.FriendlyStrength));
        }
        else
        {
            result = 0;
        }

        AIGroup group = GroupControl.Group;
        if (group.TargetMapCoordinate == AITargetLocation.At(target.Location) && group.TargetType == AIGroupTargetStatus.Execute)
        {
            pathStrength = Round((double)target.PathStrength * CurrentPathStrengthFactor);
        }
        else
        {
            pathStrength = Round((double)target.PathStrength * PathStrengthFactor);
        }

        if (result < pathStrength)
        {
            result = pathStrength;
        }

        return result;
    }

    // AIGroup.pas:3349
    public int GetRequiredStrength(AIGroupControlTarget target)
    {
        int battleFieldStrength;
        int result = GetRequiredTargetStrength(target);
        battleFieldStrength = GetRequiredBattleFieldStrength(target);
        if (battleFieldStrength > result)
        {
            result = battleFieldStrength;
        }

        StrategicMap.Current.AIExecutor.UseProcessTime(250);
        return result;
    }

    // AIGroup.pas:3359
    public bool ValidateTargetRequirements(UnitCollection units, AIGroupControlTarget target, UnitUserList required, UnitUserList optional, int minimumStrength)
    {
        int strength;
        CombatPredictor predictor;
        UnitUserList requiredUnits;
        UnitUserList optionalUnits;
        int superiority;
        Troop troop;
        predictor = new CombatPredictor(this);
        requiredUnits = new UnitUserList(null) { Holder = CreationHolder() };
        optionalUnits = new UnitUserList(null) { Holder = CreationHolder() };
        predictor.AddUnits(units);
        if (StrategicMap.Current.MapContainer.GetFieldMapCoordinate(target.Location).GetMoveAction(GroupControl.Group.Player) != MoveAction.Negotiate)
        {
            predictor.InitializeAdjacentOpponents((byte)GroupControl.Group.Player, (byte)GroupControl.Group.IndependentRelation, target.Location.X, target.Location.Y, target.Location.MapLevel);
        }

        superiority = GetRequiredCombatPredictorSuperiority(target);
        predictor.Execute();
        bool result = predictor.GetSuperiority() > superiority;
        predictor.ListRequiredUnits(requiredUnits, superiority);
        if (requiredUnits.Count != units.Count)
        {
            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (requiredUnits.IndexOf(units.GetUnit(i)) == -1)
                {
                    optionalUnits.AddUnit(units.GetUnit(i));
                }
            }
        }

        if (optionalUnits.Count != 0 || result)
        {
            strength = GetRequiredStrength(target);
            if (strength < minimumStrength)
            {
                strength = minimumStrength;
            }

            strength -= requiredUnits.GetStrengthMax(_damageTypes);
            while (strength > 0 && optionalUnits.Count != 0)
            {
                troop = optionalUnits.GetUnit(optionalUnits.Count - 1);
                strength -= troop.GetStrengthMax(_damageTypes);
                requiredUnits.AddUnit(troop);
                optionalUnits.DeleteUnit(troop);
            }

            result = result && strength <= 0;
        }

        if (required != null)
        {
            required.AddUnits(requiredUnits.Units);
        }

        if (optional != null)
        {
            optional.AddUnits(optionalUnits.Units);
        }

        optionalUnits?.Dispose();
        requiredUnits?.Dispose();
        predictor?.Dispose();
        return result;
    }

    // AIGroup.pas:3406
    public bool ValidMultiTarget(UnitUserList units, AIGroupControlTarget target, UnitUserList required, UnitUserList optional)
    {
        bool result;
        if (!TargetOverlapFound(_nextTarget, null) && (_target.Location.MapLevel != target.Location.MapLevel || HexGeometry.DeltaHexToRadius(_target.Location.X, _target.Location.Y, target.Location.X, target.Location.Y) > 1))
        {
            result = ValidateTargetRequirements(_spareUnits.Units, _nextTarget, required, optional, MinimumStrength);
            bool condition = result;
            if (condition)
            {
                int targetMana = _target.GetTargetMana();
                int targetTargetMana = target.GetTargetMana();
                int targetGold = _target.GetTargetGold();
                int targetTargetGold = target.GetTargetGold();
                condition = GroupControl.Group.RequestTargetExpense(targetGold + targetTargetGold, targetMana + targetTargetMana);
            }

            result = condition;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AIGroup.pas:3441
    public void ShowTargetInfo()
    {
        string text = "";
        AIGroupControlTarget target;
        GetEngine().OutputDebugMessage(DebugChannel.AI, "Targets for Group (ID:" + IntToStr(GroupControl.Group.GroupId) + ")");
        for (int i = 0, last = GroupControl.Targets.Count - 1; i <= last; i++)
        {
            target = GroupControl.Targets.GetTargets(i);
            target.GetDebugInfo(out text);
            text = text + "/TS_Prio:" + IntToStr(target.DataA);
            text = text + "/MPBFS:" + IntToStr(target.PathStrength);
            text = text + "/TG:" + IntToStr(target.GetTargetGold());
            text = text + "/MP:" + IntToStr(target.MovePoints);
            text = text + "/Status:";
            if (target.Status.Contains(AIGroupTargetFlags.Cooperate))
            {
                text = text + "c";
            }

            if (target.Status.Contains(AIGroupTargetFlags.Recruit))
            {
                text = text + "r";
            }

            if (target.Status.Contains(AIGroupTargetFlags.Produce))
            {
                text = text + "p";
            }

            if (target.Status.Contains(AIGroupTargetFlags.Valid))
            {
                text = text + "v";
            }

            if (target.Status.Contains(AIGroupTargetFlags.Done))
            {
                text = text + "d";
            }

            GetEngine().OutputDebugMessage(DebugChannel.AI, text);
        }
    }

    // AIGroup.pas:3467
    public void NoTargetsFound()
    {
        TargetStatus = AIGroupTargetingStatus.NoTargetsFound;
        // The cleared target: the Delphi wrote -1, all four bytes $FF, which differs from None.
        GroupControl.Group.TargetMapCoordinate = AITargetLocation.Cleared;
        GroupControl.Group.TargetType = AIGroupTargetStatus.None;
        if (GroupControl.Group.Transporter != null)
        {
            GroupControl.Group.DeleteUnit(GroupControl.Group.Transporter);
        }
    }

    // AIGroup.pas:3476
    public bool AllowMultiTargets(AIGroupControlTarget target)
    {
        bool result = Mode.Contains(AIGroupTargetingModes.MultiTargets);
        result = result && (target.Status & Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Recruit, AIGroupTargetFlags.Produce)) == Set<AIGroupTargetFlags>.Of();
        bool condition = result;
        if (condition)
        {
            int requiredStrength = GetRequiredStrength(target);
            condition = GroupControl.StrengthMax >= (double)requiredStrength * MultiTargetFactor;
        }

        result = condition;
        result = result && GroupControl.StrengthMax >= MinimumStrength;
        bool test = result;
        if (test)
        {
            bool passed = target.Data.TargetType != AITargetType.Negotiate;
            if (!passed)
            {
                int movePointsMin = GroupControl.Group.GetMovePointsMin();
                passed = target.MovePoints >= movePointsMin;
            }

            test = passed;
        }

        return test;
    }

    private static int StrengthInfoMax(ReadOnlySpan<int> strengths)
    {
        int number = 0;
        for (int i = 0; i <= 9; i++)
        {
            DamageType damageType = (DamageType)i;
            if (strengths[(int)damageType] > number)
            {
                number = strengths[(int)damageType];
            }
        }
        return number;
    }

    private static int CompareTargetSetupPriority(AIGroupControlTarget item1, AIGroupControlTarget item2) => item2.DataA - item1.DataA;

    // target factor, immediate bonus, transport penalty, overlap penalties, existing-target
    // bonuses, cooperation penalties, recruitment penalties, move-day penalty, production penalties
    public partial class PrioritySettings
    {
        public float TargetFactor { get; set; }
        public int ImmediateBonus { get; set; }
        public int TransportPenalty { get; set; }
        public int FarOverlapPenalty { get; set; }
        public int NearOverlapPenalty { get; set; }
        public int SetupBonus { get; set; }
        public int ExecuteBonus { get; set; }
        public int CompleteCooperationPenalty { get; set; }
        public int PartialCooperationPenalty { get; set; }
        public int PartialRecruitmentPenalty { get; set; }
        public int CompleteRecruitmentPenalty { get; set; }
        public int MoveDayPenalty { get; set; }
        public int PartialProductionPenalty { get; set; }
        public int ProductionDayPenalty { get; set; }
        // System.pas:111

    }

    // intf_P5_07.inc:624

    // intf_P5_07.inc:624

}
