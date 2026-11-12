// Translated from AoWE.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
    public AIGroupTargetSelector(ECustomNode owner) : base(owner)
    {
        Recruitment = new AIGroupRecruitControl(null);
        Production = new AIGroupProductionControl(null);
        Cooperation = new AIGroupCooperationControl(null);
        Priorities = new PrioritySettings();
        SpareUnits = new UnitUserList(null);
    }

    public AIGroupTargetingStatus TargetStatus;
    public AIGroupTask AIGroupTask;
    public AIGroupRecruitControl Recruitment;
    public AIGroupProductionControl Production;
    public AIGroupCooperationControl Cooperation;
    public Set<AIGroupTargetingModes> Mode;
    public int MovesMin;
    public AIGroupControlTarget Target;
    public int Priority;
    public UnitUserList SpareUnits;
    public AIGroupControlTarget NextTarget;
    public float BattleFieldFactor;
    public float CurrentBattleFieldFactor;
    public float BattleFieldMoveFactor;
    public int BattleFieldRadius;
    public Set<DamageType> DamageTypes;
    public float Superiority;
    public float NearSuperiority;
    public float MoveDayFactor;
    public int MinimumStrength;
    public float MultiTargetFactor;
    public float PathStrengthFactor;
    public float CurrentPathStrengthFactor;
    public int TargetLimit;
    public bool AllowTransport;
    public int ValidTargetLimit;
    public PrioritySettings Priorities;

    // AIGroup.pas:3051
    public override void Dispose()
    {
        DeactivatePlugin();
        Recruitment.Release();
        Production.Release();
        Cooperation.Release();
        SpareUnits?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:3421
    public override void Validate()
    {
        Troop troop;
        for (int I = SpareUnits.Count - 1; I >= 0; I--)
        {
            troop = SpareUnits.GetUnit(I);
            if (GroupControl.Group.IndexOf(troop) == -1)
            {
                SpareUnits.DeleteUnit(troop);
            }
        }

        GroupControl.Targets.Validate();
        if (State != 21 && Target != null)
        {
            if (GroupControl.Targets._children.IndexOf(Target) == -1)
            {
                Target = null;
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
        MovesMin = 50;
        for (int I = GroupControl.Group.Count - 1; I >= 0; I--)
        {
            if (GroupControl.Group.GetUnit(I).GetMoves() < MovesMin)
            {
                MovesMin = GroupControl.Group.GetUnit(I).GetMoves();
            }
        }

        for (int I = GroupControl.Targets.Count - 1; I >= 0; I--)
        {
            target = GroupControl.Targets.GetTargets(I);
            valid = ValidateTargetOverlap(GroupControl.Group, target);
            bool condition = valid;
            if (condition)
            {
                bool test = target.Data.TargetType != AITargetType.Negotiate;
                if (!test)
                {
                    var targetMana = target.GetTargetMana();
                    var targetGold = target.GetTargetGold();
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
                    var players = AoWHSMap.Current.Players.GetPlayers(GroupControl.Group.Player);
                    var targetGold = target.GetTargetGold();
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
                    var players = AoWHSMap.Current.Players.GetPlayers(GroupControl.Group.Player);
                    var targetMana = target.GetTargetMana();
                    test = players.MagicControl.Mana >= targetMana;
                }

                condition2 = test;
            }

            valid = condition2;
            if (valid)
            {
                target.DataA = GetTargetSetupPriority(GroupControl.Targets.GetTargets(I));
                target.Status = Set<AIGroupTargetFlags>.Of();
            }
            else
            {
                target.Status = Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Done);
            }
        }

        GroupControl.Targets._children.Sort(CompareTargetSetupPriority);
        for (int I = GroupControl.Targets.Count - 1, last = TargetLimit + 1; I >= last; I--)
        {
            GroupControl.Targets.Delete(I);
        }

        Target = null;
        TargetStatus = AIGroupTargetingStatus.Busy;
        return true;
    }

    // AIGroup.pas:3487
    public override bool Process()
    {
        bool result = default;
        int I = default;
        int strength = default;
        int superiority = default;
        int time = default;
        bool busy = default;
        AIGroupTask aiGroupTask = default;
        UnitUserList optional = default;
        UnitUserList required = default;
        AIGroup Group = default;
        MapCoordinate location = default;
        aiGroupTask = AIGroupTask;
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
                    I = 0;
                    while (I != GroupControl.Targets.Count && GroupControl.Targets.GetTargets(I).Status.Contains(AIGroupTargetFlags.Done))
                    {
                        I += 1;
                    }

                    if (I < GroupControl.Targets.Count && GetValidTargets() < ValidTargetLimit)
                    {
                        Target = GroupControl.Targets.GetTargets(I);
                        Priority = Target.DataA;
                        strength = GetRequiredStrength(Target);
                        superiority = GetRequiredCombatPredictorSuperiority(Target);
                        AoWHSMap.Current.AIExecutor.UseProcessTime(100);
                        busy = GroupControl.StrengthMax < strength;
                        if (!busy)
                        {
                            GroupControl.SetupCP(Target);
                            GroupControl.CombatPredictor.Execute();
                            busy = busy || Set<byte>.WithBounds(0, 255).With(2).With(4).With(8).Contains(GroupControl.CombatPredictor.Result);
                            busy = busy || GroupControl.CombatPredictor.GetSuperiority() < superiority;
                        }

                        if (busy)
                        {
                            if ((Mode & Set<AIGroupTargetingModes>.Of(AIGroupTargetingModes.Cooperate, AIGroupTargetingModes.Recruit, AIGroupTargetingModes.Produce)) != Set<AIGroupTargetingModes>.Of())
                            {
                                GroupControl.SetupCP(Target);
                                if (Mode.Contains(AIGroupTargetingModes.Cooperate))
                                {
                                    Cooperation.Setup(GroupControl, Target);
                                    Cooperation.Superiority = superiority;
                                    Cooperation.CooperateStrength = strength - GroupControl.StrengthMax;
                                    Cooperation.Mode = AIGroupCooperationControl.CooperationMode.None;
                                    ActivatePlugin(Cooperation);
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
                            Target.Status = Target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid);
                            var movePointsMin = GroupControl.Group.GetMovePointsMin();
                            if (Target.MovePoints < movePointsMin)
                            {
                                Priority += Priorities.ImmediateBonus;
                            }

                            TargetProcessed();
                        }
                    }
                    else
                    {
                        GroupControl.Targets._children.Sort(CompareTargetSetupPriority);
                        State = 10;
                    }

                    break;
                case 1:
                    if (Cooperation.Complete)
                    {
                        Target.Status = Target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid, AIGroupTargetFlags.Cooperate);
                        Priority += Priorities.CompleteCooperationPenalty;
                        bool condition = Cooperation.GetCooperateMoveDays() == 0;
                        if (condition)
                        {
                            var movePointsMin = GroupControl.Group.GetMovePointsMin();
                            condition = Target.MovePoints < movePointsMin;
                        }

                        if (condition)
                        {
                            Priority += Priorities.ImmediateBonus;
                        }

                        TargetProcessed();
                    }
                    else
                    {
                        if (Cooperation.Groups.Count != 0)
                        {
                            Target.Status = Target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid, AIGroupTargetFlags.Cooperate);
                            Priority += Priorities.PartialCooperationPenalty;
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
                    Recruitment.Setup(GroupControl);
                    var requiredCombatPredictorSuperiority = GetRequiredCombatPredictorSuperiority(Target);
                    Recruitment.Superiority = requiredCombatPredictorSuperiority;
                    var requiredStrength = GetRequiredStrength(Target);
                    Recruitment.RecruitStrength = requiredStrength - GroupControl.StrengthMax;
                    if (Target.Status.Contains(AIGroupTargetFlags.Cooperate))
                    {
                        var cooperatedStrength = Cooperation.GetCooperatedStrength();
                        Recruitment.RecruitStrength -= cooperatedStrength;
                    }

                    Recruitment.Mode = AIGroupRecruitControlMode.None;
                    ActivatePlugin(Recruitment);
                    State = 3;
                    break;
                case 3:
                    if (Recruitment.Complete)
                    {
                        Target.Status = Target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid, AIGroupTargetFlags.Recruit);
                        Priority += Priorities.CompleteRecruitmentPenalty;
                        TargetProcessed();
                    }
                    else
                    {
                        if (Recruitment.Units.Count != 0)
                        {
                            Target.Status = Target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Recruit);
                            Priority += Priorities.PartialRecruitmentPenalty;
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
                    Production.Setup(GroupControl);
                    var requiredCombatPredictorSuperiority2 = GetRequiredCombatPredictorSuperiority(Target);
                    Production.Superiority = requiredCombatPredictorSuperiority2;
                    var requiredStrength2 = GetRequiredStrength(Target);
                    Production.ProduceStrength = requiredStrength2 - GroupControl.StrengthMax;
                    if (Target.Status.Contains(AIGroupTargetFlags.Cooperate))
                    {
                        var cooperatedStrength = Cooperation.GetCooperatedStrength();
                        Production.ProduceStrength -= cooperatedStrength;
                    }

                    if (Target.Status.Contains(AIGroupTargetFlags.Recruit))
                    {
                        var recruitedStrength = Recruitment.GetRecruitedStrength();
                        Production.ProduceStrength -= recruitedStrength;
                    }

                    Production.Mode = AIGroupProductionControlMode.None;
                    ActivatePlugin(Production);
                    State = 5;
                    break;
                case 5:
                    if (Production.Complete)
                    {
                        Target.Status = Target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid, AIGroupTargetFlags.Produce);
                        time = Production.GetCompleteProductionTime();
                        if (time > 0)
                        {
                            Priority += Round(Priorities.ProductionDayPenalty * time);
                        }
                    }
                    else
                    {
                        if (Production.Productions.Count != 0)
                        {
                            Target.Status = Target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid, AIGroupTargetFlags.Produce);
                            Priority += Priorities.PartialProductionPenalty;
                            time = Production.GetCompleteProductionTime();
                            if (time > 0)
                            {
                                Priority += Priorities.ProductionDayPenalty * time;
                            }
                        }
                    }

                    TargetProcessed();
                    break;
                case 10:
                    I = 0;
                    while (I != GroupControl.Targets.Count && !GroupControl.Targets.GetTargets(I).Status.Contains(AIGroupTargetFlags.Valid))
                    {
                        I += 1;
                    }

                    if (I == GroupControl.Targets.Count)
                    {
                        NoTargetsFound();
                        Done();
                    }
                    else
                    {
                        Target = GroupControl.Targets.GetTargets(I);
                        if ((Target.Status & Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Recruit, AIGroupTargetFlags.Produce, AIGroupTargetFlags.Cooperate)) != Set<AIGroupTargetFlags>.Of())
                        {
                            GroupControl.SetupCP(Target);
                            if (Target.Status.Contains(AIGroupTargetFlags.Cooperate))
                            {
                                Cooperation.Setup(GroupControl, Target);
                                var requiredCombatPredictorSuperiority3 = GetRequiredCombatPredictorSuperiority(Target);
                                Cooperation.Superiority = requiredCombatPredictorSuperiority3;
                                var requiredStrength3 = GetRequiredStrength(Target);
                                Cooperation.CooperateStrength = requiredStrength3 - GroupControl.StrengthMax;
                                Cooperation.Mode = AIGroupCooperationControl.CooperationMode.AutoCooperate;
                                ActivatePlugin(Cooperation);
                                State = 11;
                            }
                            else
                            {
                                if (Target.Status.Contains(AIGroupTargetFlags.Recruit))
                                {
                                    State = 12;
                                }
                                else
                                {
                                    if (Target.Status.Contains(AIGroupTargetFlags.Produce))
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
                            Target.Status = Target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Ready);
                            State = 20;
                        }
                    }

                    break;
                case 11:
                    if (Cooperation.Complete)
                    {
                        Target.Status = Target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Ready);
                        State = 20;
                    }
                    else
                    {
                        if (Target.Status.Contains(AIGroupTargetFlags.Recruit))
                        {
                            State = 12;
                        }
                        else
                        {
                            if (Target.Status.Contains(AIGroupTargetFlags.Produce))
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
                    Recruitment.Setup(GroupControl);
                    var requiredCombatPredictorSuperiority4 = GetRequiredCombatPredictorSuperiority(Target);
                    Recruitment.Superiority = requiredCombatPredictorSuperiority4;
                    var requiredStrength4 = GetRequiredStrength(Target);
                    Recruitment.RecruitStrength = requiredStrength4 - GroupControl.StrengthMax;
                    Recruitment.Mode = AIGroupRecruitControlMode.AutoRecruit;
                    ActivatePlugin(Recruitment);
                    State += 1;
                    break;
                case 13:
                    if (Recruitment.Complete)
                    {
                        Target.Status = Target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Ready);
                        State = 20;
                    }
                    else
                    {
                        if (Target.Status.Contains(AIGroupTargetFlags.Produce))
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
                    Production.Setup(GroupControl);
                    var requiredCombatPredictorSuperiority5 = GetRequiredCombatPredictorSuperiority(Target);
                    Production.Superiority = requiredCombatPredictorSuperiority5;
                    var requiredStrength5 = GetRequiredStrength(Target);
                    Production.ProduceStrength = requiredStrength5 - GroupControl.StrengthMax;
                    Production.Mode = AIGroupProductionControlMode.AutoProduce;
                    ActivatePlugin(Production);
                    State = 20;
                    break;
                case 20:
                    if (Target.Status.Contains(AIGroupTargetFlags.Ready))
                    {
                        if (Target.TransporterId != -1)
                        {
                            var troop = AoWHSMap.Current.UnitControl.FindUnit(Target.TransporterId);
                            GroupControl.Group.AddTransporter(troop);
                        }
                        else
                        {
                            var group = GroupControl.Group;
                            if (group.Transporter != null)
                            {
                                group.DeleteUnit(group.Transporter);
                            }
                        }

                        if (AllowMultiTargets(Target))
                        {
                            State = 30;
                        }
                        else
                        {
                            GroupControl.Group.SetupTarget(Target, AIGroupTargetStatus.Execute, 0);
                            GroupControl.SetupME();
                            GroupControl.ExecuteME(Target.Location.X, Target.Location.Y, Target.Location.MapLevel);
                            result = true;
                            State = 21;
                        }
                    }
                    else
                    {
                        if (GroupControl.Group.Productions.Count != 0)
                        {
                            GroupControl.Group.Productions.GetProductions(0).Control.GetLocation(ref location.X, ref location.Y, ref location.MapLevel);
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
                                var productionLocation = GroupControl.ProductionLocations.GetProductionLocation(0);
                                var fProductionLocationsProductionLocation = GroupControl.ProductionLocations.GetProductionLocation(0);
                                var productionLocation2 = GroupControl.ProductionLocations.GetProductionLocation(0);
                                GroupControl.ExecuteME(productionLocation2.Location.X, fProductionLocationsProductionLocation.Location.Y, productionLocation.Location.MapLevel);
                                result = true;
                                State = 22;
                            }
                        }
                    }

                    break;
                case 21:
                    if (GroupControl.Targets._children.IndexOf(Target) == -1)
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
                    GroupControl.Group.SetupTarget(Target, AIGroupTargetStatus.Setup, 0);
                    TargetStatus = AIGroupTargetingStatus.SettingUpTarget;
                    Done();
                    break;
                case 30:
                    ValidateTargetRequirements(GroupControl.Group, Target, null, SpareUnits, MinimumStrength);
                    if (SpareUnits.Count != 0)
                    {
                        Target.Status = Target.Status - Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid);
                        NextTarget = null;
                        State = 31;
                    }
                    else
                    {
                        State = 32;
                    }

                    break;
                case 31:
                    I = GroupControl.Targets._children.IndexOf(NextTarget);
                    while (I < GroupControl.Targets.Count && (I == -1 || GroupControl.Targets.GetTargets(I).TransporterId != -1 || !GroupControl.Targets.GetTargets(I).Status.Contains(AIGroupTargetFlags.Valid)))
                    {
                        I += 1;
                    }

                    if (I < GroupControl.Targets.Count)
                    {
                        required = new UnitUserList(null);
                        optional = new UnitUserList(null);
                        NextTarget = GroupControl.Targets.GetTargets(I);
                        if (ValidMultiTarget(SpareUnits, NextTarget, required, optional))
                        {
                            var group = GroupControl.Group;
                            var behavior = group.GroupClass.Behavior;
                            Group = AoWHSMap.Current.AIGroupManager.CreateGroup(group.Player, group.IndependentRelation, behavior);

                            Group.AddUnits(required);
                            Group.SetupTarget(NextTarget, AIGroupTargetStatus.Execute, 0);
                            Group.Release();
                            SpareUnits.Clear();
                            SpareUnits.AddUnits(optional);
                            if (SpareUnits.Count == 0)
                            {
                                State = 32;
                            }
                        }

                        NextTarget.Status = NextTarget.Status - Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Valid);
                        optional?.Dispose();
                        required?.Dispose();
                    }
                    else
                    {
                        State = 32;
                    }

                    break;
                case 32:
                    GroupControl.Group.SetupTarget(Target, AIGroupTargetStatus.Execute, 0);
                    GroupControl.SetupME();
                    GroupControl.ExecuteME(Target.Location.X, Target.Location.Y, Target.Location.MapLevel);
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
        int I;
        AIGroup group;
        player = AoWHSMap.Current.Players.GetPlayers(GroupControl.Group.Player);
        I = player.AIGroups.Count;
        bool result = false;
        while (I != 0 && !result)
        {
            I -= 1;
            group = player.AIGroups.GetGroups(I);
            result = group != ignoreGroup && group.TargetType == AIGroupTargetStatus.Execute && target.Location.MapLevel == group.TargetMapCoordinate.MapLevel && HexGeometry.DeltaHexToRadius(group.TargetMapCoordinate.X, group.TargetMapCoordinate.Y, target.Location.X, target.Location.Y) <= 1;
        }

        return result;
    }

    // AIGroup.pas:3088
    public bool ValidateTargetOverlap(AIGroup group, AIGroupControlTarget target)
    {
        int I;
        int moves;
        int points;
        int otherDays;
        int days;
        Player player;
        AIGroup other;
        player = AoWHSMap.Current.Players.GetPlayers(GroupControl.Group.Player);
        I = player.AIGroups.Count;
        bool result = true;
        while (I != 0 && result)
        {
            I -= 1;
            other = player.AIGroups.GetGroups(I);
            if (other != group && other.TargetType == AIGroupTargetStatus.Execute && other.TargetMapCoordinate.MapLevel == target.Location.MapLevel && HexGeometry.DeltaHexToRadius(other.TargetMapCoordinate.X, other.TargetMapCoordinate.Y, target.Location.X, target.Location.Y) <= 1)
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
                var movePointsMin = group.GetMovePointsMin();
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
        for (int I = GroupControl.Targets.Count - 1; I >= 0; I--)
        {
            if (GroupControl.Targets.GetTargets(I).Status.Contains(AIGroupTargetFlags.Valid))
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
        SpareUnits.Clear();
        Mode = Set<AIGroupTargetingModes>.Of(AIGroupTargetingModes.MultiTargets, AIGroupTargetingModes.Cooperate, AIGroupTargetingModes.Recruit, AIGroupTargetingModes.Produce);
        DamageTypes = AbilityRules.s_allDamageTypes - Set<DamageType>.Of(DamageType.Wall);
        BattleFieldFactor = 1;
        CurrentBattleFieldFactor = (float)0.8;
        BattleFieldMoveFactor = 0;
        BattleFieldRadius = 10;
        ValidTargetLimit = 5;
        PathStrengthFactor = 1;
        CurrentPathStrengthFactor = (float)0.8;
        AllowTransport = false;
        TargetLimit = 100;
        this.Priorities.TargetFactor = 1;
        this.Priorities.ImmediateBonus = 500;
        this.Priorities.SetupBonus = 25;
        this.Priorities.ExecuteBonus = 75;
        this.Priorities.TransportPenalty = -100;
        this.Priorities.PartialRecruitmentPenalty = 0;
        this.Priorities.CompleteRecruitmentPenalty = -50;
        this.Priorities.CompleteCooperationPenalty = -25;
        this.Priorities.PartialCooperationPenalty = -50;
        this.Priorities.MoveDayPenalty = -25;
        this.Priorities.ProductionDayPenalty = -25;
        this.Priorities.PartialProductionPenalty = -10;
        this.Priorities.FarOverlapPenalty = -199;
        this.Priorities.NearOverlapPenalty = -50;
    }

    // AIGroup.pas:3173
    public void ActivatePlugin(AIGroupTask aiGroupTask)
    {
        DeactivatePlugin();
        AIGroupTask = aiGroupTask;
        AIGroupTask.AddRef();
        AIGroupTask.Activate(GroupControl);
    }

    // AIGroup.pas:3181
    public void DeactivatePlugin()
    {
        if (AIGroupTask != null)
        {
            AIGroupTask.Release();
            AIGroupTask = null;
        }
    }

    // AIGroup.pas:3190
    public int GetTargetSetupPriority(AIGroupControlTarget target)
    {
        AIGroup group;
        int days;
        int priority;
        priority = Round((double)target.Data.Priority * Priorities.TargetFactor);
        var movesMin = GroupControl.Group.GetMovesMin();
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
        if (Unsafe.As<MapCoordinate, int>(ref group.TargetMapCoordinate) == Unsafe.As<MapCoordinate, int>(ref target.Location))
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
            if (target.MovePoints <= MovesMin)
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
        switch ((int)AoWHSMap.Current.Players.GetPlayers(control.Group.Player).PlayerType)
        {
            case (int)PlayerType.Cpu1:
                ValidTargetLimit = 5;
                break;
            case (int)PlayerType.Cpu2:
                ValidTargetLimit = 7;
                break;
            case (int)PlayerType.Cpu3:
            case (int)PlayerType.Cpu4:
            case (int)PlayerType.Cpu5:
                ValidTargetLimit = 10;
                break;
            default:
                ValidTargetLimit = 5;
                break;
        }

        return true;
    }

    // AIGroup.pas:3265
    public void TargetProcessed()
    {
        Target.DataA = Priority;
        Target.Status = Target.Status + Set<AIGroupTargetFlags>.Of(AIGroupTargetFlags.Done);
        Target = null;
        State = 0;
    }

    // AIGroup.pas:3273
    public float GetRequiredSuperiority(AIGroupControlTarget target)
    {
        float result;
        int days;
        if (target.Data.TargetType != AITargetType.Negotiate)
        {
            var group = GroupControl.Group;
            if (Unsafe.As<MapCoordinate, int>(ref group.TargetMapCoordinate) == Unsafe.As<MapCoordinate, int>(ref target.Location) && group.TargetType == AIGroupTargetStatus.Execute || group.GetMovesMin() >= target.MovePoints)
            {
                result = NearSuperiority;
            }
            else
            {
                result = Superiority;
            }

            if (GroupControl.Group.GetMovesMin() > 0)
            {
                var movesMin = GroupControl.Group.GetMovesMin();
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
            BattleFieldInfo.SetupBattleFieldInfo(ref info, GroupControl.Group.Player, GroupControl.Group.IndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(), DamageTypes);
            info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
            AoWHSMap.Current.RetrieveBattleFieldInfo(target.Location.X, target.Location.Y, target.Location.MapLevel, 1, ref info);
            result = Round((double)GetRequiredSuperiority(target) * info.Strength);
            ((AoWMapField)AoWHSMap.Current.MapContainer.GetFieldMapCoordinate(Unsafe.As<MapCoordinate, int>(ref target.Location))).GetDefenseRequirements(GroupControl.Group.Player, ref strengths);
            result += StrengthInfoMax(Bytes.Of(ref strengths)) / 2;
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
        if (Unsafe.As<MapCoordinate, int>(ref target.Location) == Unsafe.As<MapCoordinate, int>(ref GroupControl.Group.TargetMapCoordinate) && GroupControl.Group.TargetType == AIGroupTargetStatus.Execute)
        {
            result = CurrentBattleFieldFactor;
        }
        else
        {
            result = BattleFieldFactor;
        }

        return (float)((double)result + Round((double)target.MovePoints * BattleFieldMoveFactor));
    }

    // AIGroup.pas:3325
    public int GetRequiredBattleFieldStrength(AIGroupControlTarget target)
    {
        int result;
        BattleFieldInfo info = default;
        int pathStrength;
        if (((double)BattleFieldFactor > 0 || (double)CurrentBattleFieldFactor > 0) && target.Data.TargetType != AITargetType.Negotiate)
        {
            BattleFieldInfo.SetupBattleFieldInfo(ref info, GroupControl.Group.Player, GroupControl.Group.IndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(), DamageTypes);
            info.Units = GroupControl.Group;
            info.Requests = info.Requests + Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents);
            AoWHSMap.Current.RetrieveBattleFieldInfo(target.Location.X, target.Location.Y, target.Location.MapLevel, BattleFieldRadius, ref info);
            result = Round((double)GetRequiredBattleFieldSuperiority(target) * (info.Strength - info.FriendlyStrength));
        }
        else
        {
            result = 0;
        }

        var group = GroupControl.Group;
        if (Unsafe.As<MapCoordinate, int>(ref group.TargetMapCoordinate) == Unsafe.As<MapCoordinate, int>(ref target.Location) && group.TargetType == AIGroupTargetStatus.Execute)
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

        AoWHSMap.Current.AIExecutor.UseProcessTime(250);
        return result;
    }

    // AIGroup.pas:3359
    public bool ValidateTargetRequirements(UnitList units, AIGroupControlTarget target, UnitList required, UnitList optional, int minimumStrength)
    {
        int strength;
        CombatPredictor predictor;
        UnitUserList requiredUnits;
        UnitUserList optionalUnits;
        int superiority;
        Troop troop;
        predictor = new CombatPredictor();
        requiredUnits = new UnitUserList(null);
        optionalUnits = new UnitUserList(null);
        predictor.AddUnits(units);
        if (((AoWMapField)AoWHSMap.Current.MapContainer.GetFieldMapCoordinate(Unsafe.As<MapCoordinate, int>(ref target.Location))).GetMoveAction(GroupControl.Group.Player) != MoveAction.Negotiate)
        {
            predictor.InitializeAdjacentOpponents((byte)GroupControl.Group.Player, (byte)GroupControl.Group.IndependentRelation, target.Location.X, target.Location.Y, target.Location.MapLevel);
        }

        superiority = GetRequiredCombatPredictorSuperiority(target);
        predictor.Execute();
        bool result = predictor.GetSuperiority() > superiority;
        predictor.ListRequiredUnits(requiredUnits, superiority);
        if (requiredUnits.Count != units.Count)
        {
            for (int I = units.Count - 1; I >= 0; I--)
            {
                if (requiredUnits.IndexOf(units.GetUnit(I)) == -1)
                {
                    optionalUnits.AddUnit(units.GetUnit(I));
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

            strength -= requiredUnits.GetStrengthMax(DamageTypes);
            while (strength > 0 && optionalUnits.Count != 0)
            {
                troop = optionalUnits.GetUnit(optionalUnits.Count - 1);
                strength -= troop.GetStrengthMax(DamageTypes);
                requiredUnits.AddUnit(troop);
                optionalUnits.DeleteUnit(troop);
            }

            result = result && strength <= 0;
        }

        if (required != null)
        {
            required.AddUnits(requiredUnits);
        }

        if (optional != null)
        {
            optional.AddUnits(optionalUnits);
        }

        optionalUnits?.Dispose();
        requiredUnits?.Dispose();
        predictor?.Dispose();
        return result;
    }

    // AIGroup.pas:3406
    public bool ValidMultiTarget(UnitList units, AIGroupControlTarget target, UnitList required, UnitList optional)
    {
        bool result;
        if (!TargetOverlapFound(NextTarget, null) && (Target.Location.MapLevel != target.Location.MapLevel || HexGeometry.DeltaHexToRadius(Target.Location.X, Target.Location.Y, target.Location.X, target.Location.Y) > 1))
        {
            result = ValidateTargetRequirements(SpareUnits, NextTarget, required, optional, MinimumStrength);
            bool condition = result;
            if (condition)
            {
                var targetMana = Target.GetTargetMana();
                var targetTargetMana = target.GetTargetMana();
                var targetGold = Target.GetTargetGold();
                var targetTargetGold = target.GetTargetGold();
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
        GameTables.GameEngine.OutputDebugMessage(AbilityRules.s_aiDebugChannel, "Targets for Group (ID:" + IntToStr(GroupControl.Group.GroupId) + ")");
        for (int I = 0, last = GroupControl.Targets.Count - 1; I <= last; I++)
        {
            target = GroupControl.Targets.GetTargets(I);
            target.GetDebugInfo(ref text);
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

            GameTables.GameEngine.OutputDebugMessage(AbilityRules.s_aiDebugChannel, text);
        }
    }

    // AIGroup.pas:3467
    public void NoTargetsFound()
    {
        TargetStatus = AIGroupTargetingStatus.NoTargetsFound;
        GroupControl.Group.TargetMapCoordinate.Packed = -1;
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
            var requiredStrength = GetRequiredStrength(target);
            condition = (double)GroupControl.StrengthMax >= (double)requiredStrength * MultiTargetFactor;
        }

        result = condition;
        result = result && GroupControl.StrengthMax >= MinimumStrength;
        bool test = result;
        if (test)
        {
            bool passed = target.Data.TargetType != AITargetType.Negotiate;
            if (!passed)
            {
                var movePointsMin = GroupControl.Group.GetMovePointsMin();
                passed = target.MovePoints >= movePointsMin;
            }

            test = passed;
        }

        return test;
    }

    private static int StrengthInfoMax(ReadOnlySpan<byte> strengths)
    {
        int number = 0;
        for (int i = 0; i <= 9; i++)
        {
            DamageType damageType = (DamageType)i;
            if (MemoryMarshal.Cast<byte, Int32Array10>(strengths)[0][(int)damageType] > number)
            {
                number = MemoryMarshal.Cast<byte, Int32Array10>(strengths)[0][(int)damageType];
            }
        }
        return number;
    }

    private static int CompareTargetSetupPriority(object item1, object item2)
    {
        return ((AIGroupControlTarget)item2).DataA - ((AIGroupControlTarget)item1).DataA;
    }

    // target factor, immediate bonus, transport penalty, overlap penalties, existing-target
    // bonuses, cooperation penalties, recruitment penalties, move-day penalty, production penalties
    public partial class PrioritySettings
    {
        public float TargetFactor;
        public int ImmediateBonus;
        public int TransportPenalty;
        public int FarOverlapPenalty;
        public int NearOverlapPenalty;
        public int SetupBonus;
        public int ExecuteBonus;
        public int CompleteCooperationPenalty;
        public int PartialCooperationPenalty;
        public int PartialRecruitmentPenalty;
        public int CompleteRecruitmentPenalty;
        public int MoveDayPenalty;
        public int PartialProductionPenalty;
        public int ProductionDayPenalty;
        // System.pas:111

    }

    // intf_P5_07.inc:624

    // intf_P5_07.inc:624

}
