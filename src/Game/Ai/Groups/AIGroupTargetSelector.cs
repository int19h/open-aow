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

namespace Aow.Game.Ai.Groups;

// status, plugin, recruitment, production, cooperation, mode, minimum moves, target, priority,
// spare units, next target, battlefield factors, radius, damage types, superiority, move-day
// factor, minimum strength, split factor, path factors, target limit, transport, valid limit,
// priority settings
public partial class AIGroupTargetSelector : AIGroupControlPlugin
{
    // AIGroup.pas:3041
    public AIGroupTargetSelector(ECustomNode owner) : base(owner)
    {
        FRecruitment = new AIGroupRecruitControl(null);
        FProduction = new AIGroupProductionControl(null);
        FCooperation = new AIGroupCooperationControl(null);
        FPrioritySettings = new AIGroupTargetPrioritySettings();
        FSpareUnits = new UnitUserList(null);
    }

    public AIGroupTargetSelectorStatus FTargetStatus;
    public AIGroupControlPlugin FPlugin;
    public AIGroupRecruitControl FRecruitment;
    public AIGroupProductionControl FProduction;
    public AIGroupCooperationControl FCooperation;
    public Set<AIGroupTargetSelectorModeElement> FMode;
    public int FMovesMin;
    public AIGroupControlTarget FTarget;
    public int FPriority;
    public UnitUserList FSpareUnits;
    public AIGroupControlTarget FNextTarget;
    public float FBattleFieldFactor;
    public float FCurrentBattleFieldFactor;
    public float FBattleFieldMoveFactor;
    public int FBattleFieldRadius;
    public Set<DamageType> FDamageTypes;
    public float FSuperiority;
    public float FNearSuperiority;
    public float FMoveDayFactor;
    public int FMinimumStrength;
    public float FMultiTargetFactor;
    public float FPathStrengthFactor;
    public float FCurrentPathStrengthFactor;
    public int FTargetLimit;
    public bool FAllowTransport;
    public int FValidTargetLimit;
    public AIGroupTargetPrioritySettings FPrioritySettings;

    // AIGroup.pas:3051
    public override void Dispose()
    {
        DeactivatePlugin();
        FRecruitment.Release();
        FProduction.Release();
        FCooperation.Release();
        FSpareUnits?.Dispose();
        base.Dispose();
    }

    // AIGroup.pas:3421
    public override void Validate()
    {
        AbstractUnit AUnit;
        for (int I = FSpareUnits.Count - 1; I >= 0; I--)
        {
            AUnit = FSpareUnits.GetUnit(I);
            if (FGroupControl.FGroup.IndexOf(AUnit) == -1)
            {
                FSpareUnits.DeleteUnit(AUnit);
            }
        }

        FGroupControl.FTargets.Validate();
        if (FState != 21 && FTarget != null)
        {
            if (FGroupControl.FTargets.FChildren.IndexOf(FTarget) == -1)
            {
                FTarget = null;
                if (FState > 0 && FState < 10)
                {
                    FState = 0;
                }

                if (FState > 10)
                {
                    FState = 10;
                }
            }
        }
    }

    // AIGroup.pas:3062
    public override bool GetBusy() => FState != -1;

    // AIGroup.pas:3228
    public override bool Activate(AIGroupControl Control)
    {
        AIGroupControlTarget Target = default;
        bool Valid = default;
        FGroupControl = Control;
        FGroupControl.FTargets.Validate();
        FMovesMin = 50;
        for (int I = FGroupControl.FGroup.Count - 1; I >= 0; I--)
        {
            if (FGroupControl.FGroup.GetUnit(I).GetMoves() < FMovesMin)
            {
                FMovesMin = FGroupControl.FGroup.GetUnit(I).GetMoves();
            }
        }

        for (int I = FGroupControl.FTargets.Count - 1; I >= 0; I--)
        {
            Target = FGroupControl.FTargets.GetTargets(I);
            Valid = ValidateTargetOverlap(FGroupControl.FGroup, Target);
            bool condition = Valid;
            if (condition)
            {
                bool test = Target.FData.TargetType != AITargetType.Negotiate;
                if (!test)
                {
                    var targetMana = Target.GetTargetMana();
                    var targetGold = Target.GetTargetGold();
                    test = FGroupControl.FGroup.RequestTargetExpense(targetGold, targetMana);
                }

                condition = test;
            }

            Valid = condition;
            bool passed = Valid;
            if (passed)
            {
                bool test = Target.GetTargetGold() <= 0;
                if (!test)
                {
                    var players = AoWHSMap.Current.FPlayers.GetPlayers(FGroupControl.FGroup.FPlayer);
                    var targetGold = Target.GetTargetGold();
                    test = players.FGems >= targetGold;
                }

                passed = test;
            }

            Valid = passed;
            bool condition2 = Valid;
            if (condition2)
            {
                bool test = Target.GetTargetMana() <= 0;
                if (!test)
                {
                    var players = AoWHSMap.Current.FPlayers.GetPlayers(FGroupControl.FGroup.FPlayer);
                    var targetMana = Target.GetTargetMana();
                    test = players.FMagicControl.FMana >= targetMana;
                }

                condition2 = test;
            }

            Valid = condition2;
            if (Valid)
            {
                Target.FDataA = GetTargetSetupPriority(FGroupControl.FTargets.GetTargets(I));
                Target.FStatus = Set<AIGroupControlTargetStatusElement>.Of();
            }
            else
            {
                Target.FStatus = Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Done);
            }
        }

        FGroupControl.FTargets.FChildren.Sort(CompareTargetSetupPriority);
        for (int I = FGroupControl.FTargets.Count - 1, last = FTargetLimit + 1; I >= last; I--)
        {
            FGroupControl.FTargets.Delete(I);
        }

        FTarget = null;
        FTargetStatus = AIGroupTargetSelectorStatus.Busy;
        return true;
    }

    // AIGroup.pas:3487
    public override bool Process()
    {
        bool Result = default;
        int I = default;
        int Strength = default;
        int Superiority = default;
        int Time = default;
        bool Busy = default;
        AIGroupControlPlugin Plugin = default;
        UnitUserList Optional = default;
        UnitUserList Required = default;
        AIGroup Group = default;
        XYL Location = default;
        Plugin = FPlugin;
        if (Plugin != null)
        {
            Busy = Plugin.MainProcess();
            if (!Busy)
            {
                DeactivatePlugin();
            }
        }
        else
        {
            Busy = false;
        }

        if (!Busy)
        {
            Result = false;
            switch (FState)
            {
                case 0:
                    I = 0;
                    while (I != FGroupControl.FTargets.Count && FGroupControl.FTargets.GetTargets(I).FStatus.Contains(AIGroupControlTargetStatusElement.Done))
                    {
                        I += 1;
                    }

                    if (I < FGroupControl.FTargets.Count && GetValidTargets() < FValidTargetLimit)
                    {
                        FTarget = FGroupControl.FTargets.GetTargets(I);
                        FPriority = FTarget.FDataA;
                        Strength = GetRequiredStrength(FTarget);
                        Superiority = GetRequiredCombatPredictorSuperiority(FTarget);
                        AoWHSMap.Current.FAIExecuter.UseProcessTime(100);
                        Busy = FGroupControl.FStrengthMax < Strength;
                        if (!Busy)
                        {
                            FGroupControl.SetupCP(FTarget);
                            FGroupControl.FCombatPredictor.Execute();
                            Busy = Busy || Set<byte>.WithBounds(0, 255).With(2).With(4).With(8).Contains(FGroupControl.FCombatPredictor.FResult);
                            Busy = Busy || FGroupControl.FCombatPredictor.GetSuperiority() < Superiority;
                        }

                        if (Busy)
                        {
                            if ((FMode & Set<AIGroupTargetSelectorModeElement>.Of(AIGroupTargetSelectorModeElement.Cooperate, AIGroupTargetSelectorModeElement.Recruit, AIGroupTargetSelectorModeElement.Produce)) != Set<AIGroupTargetSelectorModeElement>.Of())
                            {
                                FGroupControl.SetupCP(FTarget);
                                if (FMode.Contains(AIGroupTargetSelectorModeElement.Cooperate))
                                {
                                    FCooperation.Setup(FGroupControl, FTarget);
                                    FCooperation.FSuperiority = Superiority;
                                    FCooperation.FCooperateStrength = Strength - FGroupControl.FStrengthMax;
                                    FCooperation.FMode = AIGroupCooperationControl.AIGroupCooperateControlMode.None;
                                    ActivatePlugin(FCooperation);
                                    FState = 1;
                                }
                                else
                                {
                                    if (FMode.Contains(AIGroupTargetSelectorModeElement.Recruit))
                                    {
                                        FState = 2;
                                    }
                                    else
                                    {
                                        if (FMode.Contains(AIGroupTargetSelectorModeElement.Produce))
                                        {
                                            FState = 4;
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
                            FTarget.FStatus = FTarget.FStatus + Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Valid);
                            var movePointsMin = FGroupControl.FGroup.GetMovePointsMin();
                            if (FTarget.FMovePoints < movePointsMin)
                            {
                                FPriority += FPrioritySettings.FImmediateBonus;
                            }

                            TargetProcessed();
                        }
                    }
                    else
                    {
                        FGroupControl.FTargets.FChildren.Sort(CompareTargetSetupPriority);
                        FState = 10;
                    }

                    break;
                case 1:
                    if (FCooperation.FComplete)
                    {
                        FTarget.FStatus = FTarget.FStatus + Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Valid, AIGroupControlTargetStatusElement.Cooperate);
                        FPriority += FPrioritySettings.FCompleteCooperationPenalty;
                        bool condition = FCooperation.GetCooperateMoveDays() == 0;
                        if (condition)
                        {
                            var movePointsMin = FGroupControl.FGroup.GetMovePointsMin();
                            condition = FTarget.FMovePoints < movePointsMin;
                        }

                        if (condition)
                        {
                            FPriority += FPrioritySettings.FImmediateBonus;
                        }

                        TargetProcessed();
                    }
                    else
                    {
                        if (FCooperation.FGroups.Count != 0)
                        {
                            FTarget.FStatus = FTarget.FStatus + Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Valid, AIGroupControlTargetStatusElement.Cooperate);
                            FPriority += FPrioritySettings.FPartialCooperationPenalty;
                        }

                        if (FMode.Contains(AIGroupTargetSelectorModeElement.Recruit))
                        {
                            FState = 2;
                        }
                        else
                        {
                            if (FMode.Contains(AIGroupTargetSelectorModeElement.Produce))
                            {
                                FState = 4;
                            }
                            else
                            {
                                TargetProcessed();
                            }
                        }
                    }

                    break;
                case 2:
                    FRecruitment.Setup(FGroupControl);
                    var requiredCombatPredictorSuperiority = GetRequiredCombatPredictorSuperiority(FTarget);
                    FRecruitment.FSuperiority = requiredCombatPredictorSuperiority;
                    var requiredStrength = GetRequiredStrength(FTarget);
                    FRecruitment.FRecruitStrength = requiredStrength - FGroupControl.FStrengthMax;
                    if (FTarget.FStatus.Contains(AIGroupControlTargetStatusElement.Cooperate))
                    {
                        var cooperatedStrength = FCooperation.GetCooperatedStrength();
                        FRecruitment.FRecruitStrength -= cooperatedStrength;
                    }

                    FRecruitment.FMode = AIGroupRecruitControl.Mode.None;
                    ActivatePlugin(FRecruitment);
                    FState = 3;
                    break;
                case 3:
                    if (FRecruitment.FComplete)
                    {
                        FTarget.FStatus = FTarget.FStatus + Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Valid, AIGroupControlTargetStatusElement.Recruit);
                        FPriority += FPrioritySettings.FCompleteRecruitmentPenalty;
                        TargetProcessed();
                    }
                    else
                    {
                        if (FRecruitment.FUnits.Count != 0)
                        {
                            FTarget.FStatus = FTarget.FStatus + Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Recruit);
                            FPriority += FPrioritySettings.FPartialRecruitmentPenalty;
                        }

                        if (FMode.Contains(AIGroupTargetSelectorModeElement.Produce))
                        {
                            FState = 4;
                        }
                        else
                        {
                            TargetProcessed();
                        }
                    }

                    break;
                case 4:
                    FProduction.Setup(FGroupControl);
                    var requiredCombatPredictorSuperiority2 = GetRequiredCombatPredictorSuperiority(FTarget);
                    FProduction.FSuperiority = requiredCombatPredictorSuperiority2;
                    var requiredStrength2 = GetRequiredStrength(FTarget);
                    FProduction.FProduceStrength = requiredStrength2 - FGroupControl.FStrengthMax;
                    if (FTarget.FStatus.Contains(AIGroupControlTargetStatusElement.Cooperate))
                    {
                        var cooperatedStrength = FCooperation.GetCooperatedStrength();
                        FProduction.FProduceStrength -= cooperatedStrength;
                    }

                    if (FTarget.FStatus.Contains(AIGroupControlTargetStatusElement.Recruit))
                    {
                        var recruitedStrength = FRecruitment.GetRecruitedStrength();
                        FProduction.FProduceStrength -= recruitedStrength;
                    }

                    FProduction.FMode = AIGroupProductionControl.Mode.None;
                    ActivatePlugin(FProduction);
                    FState = 5;
                    break;
                case 5:
                    if (FProduction.FComplete)
                    {
                        FTarget.FStatus = FTarget.FStatus + Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Valid, AIGroupControlTargetStatusElement.Produce);
                        Time = FProduction.GetCompleteProductionTime();
                        if (Time > 0)
                        {
                            FPriority += Round(FPrioritySettings.FProductionDayPenalty * Time);
                        }
                    }
                    else
                    {
                        if (FProduction.FProductions.Count != 0)
                        {
                            FTarget.FStatus = FTarget.FStatus + Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Valid, AIGroupControlTargetStatusElement.Produce);
                            FPriority += FPrioritySettings.FPartialProductionPenalty;
                            Time = FProduction.GetCompleteProductionTime();
                            if (Time > 0)
                            {
                                FPriority += FPrioritySettings.FProductionDayPenalty * Time;
                            }
                        }
                    }

                    TargetProcessed();
                    break;
                case 10:
                    I = 0;
                    while (I != FGroupControl.FTargets.Count && !FGroupControl.FTargets.GetTargets(I).FStatus.Contains(AIGroupControlTargetStatusElement.Valid))
                    {
                        I += 1;
                    }

                    if (I == FGroupControl.FTargets.Count)
                    {
                        NoTargetsFound();
                        Done();
                    }
                    else
                    {
                        FTarget = FGroupControl.FTargets.GetTargets(I);
                        if ((FTarget.FStatus & Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Recruit, AIGroupControlTargetStatusElement.Produce, AIGroupControlTargetStatusElement.Cooperate)) != Set<AIGroupControlTargetStatusElement>.Of())
                        {
                            FGroupControl.SetupCP(FTarget);
                            if (FTarget.FStatus.Contains(AIGroupControlTargetStatusElement.Cooperate))
                            {
                                FCooperation.Setup(FGroupControl, FTarget);
                                var requiredCombatPredictorSuperiority3 = GetRequiredCombatPredictorSuperiority(FTarget);
                                FCooperation.FSuperiority = requiredCombatPredictorSuperiority3;
                                var requiredStrength3 = GetRequiredStrength(FTarget);
                                FCooperation.FCooperateStrength = requiredStrength3 - FGroupControl.FStrengthMax;
                                FCooperation.FMode = AIGroupCooperationControl.AIGroupCooperateControlMode.AutoCooperate;
                                ActivatePlugin(FCooperation);
                                FState = 11;
                            }
                            else
                            {
                                if (FTarget.FStatus.Contains(AIGroupControlTargetStatusElement.Recruit))
                                {
                                    FState = 12;
                                }
                                else
                                {
                                    if (FTarget.FStatus.Contains(AIGroupControlTargetStatusElement.Produce))
                                    {
                                        FState = 14;
                                    }
                                    else
                                    {
                                        FState = 20;
                                    }
                                }
                            }
                        }
                        else
                        {
                            FTarget.FStatus = FTarget.FStatus + Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Ready);
                            FState = 20;
                        }
                    }

                    break;
                case 11:
                    if (FCooperation.FComplete)
                    {
                        FTarget.FStatus = FTarget.FStatus + Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Ready);
                        FState = 20;
                    }
                    else
                    {
                        if (FTarget.FStatus.Contains(AIGroupControlTargetStatusElement.Recruit))
                        {
                            FState = 12;
                        }
                        else
                        {
                            if (FTarget.FStatus.Contains(AIGroupControlTargetStatusElement.Produce))
                            {
                                FState = 14;
                            }
                            else
                            {
                                FState = 20;
                            }
                        }
                    }

                    break;
                case 12:
                    FRecruitment.Setup(FGroupControl);
                    var requiredCombatPredictorSuperiority4 = GetRequiredCombatPredictorSuperiority(FTarget);
                    FRecruitment.FSuperiority = requiredCombatPredictorSuperiority4;
                    var requiredStrength4 = GetRequiredStrength(FTarget);
                    FRecruitment.FRecruitStrength = requiredStrength4 - FGroupControl.FStrengthMax;
                    FRecruitment.FMode = AIGroupRecruitControl.Mode.AutoRecruit;
                    ActivatePlugin(FRecruitment);
                    FState += 1;
                    break;
                case 13:
                    if (FRecruitment.FComplete)
                    {
                        FTarget.FStatus = FTarget.FStatus + Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Ready);
                        FState = 20;
                    }
                    else
                    {
                        if (FTarget.FStatus.Contains(AIGroupControlTargetStatusElement.Produce))
                        {
                            FState = 14;
                        }
                        else
                        {
                            FState = 20;
                        }
                    }

                    break;
                case 14:
                    FProduction.Setup(FGroupControl);
                    var requiredCombatPredictorSuperiority5 = GetRequiredCombatPredictorSuperiority(FTarget);
                    FProduction.FSuperiority = requiredCombatPredictorSuperiority5;
                    var requiredStrength5 = GetRequiredStrength(FTarget);
                    FProduction.FProduceStrength = requiredStrength5 - FGroupControl.FStrengthMax;
                    FProduction.FMode = AIGroupProductionControl.Mode.AutoProduce;
                    ActivatePlugin(FProduction);
                    FState = 20;
                    break;
                case 20:
                    if (FTarget.FStatus.Contains(AIGroupControlTargetStatusElement.Ready))
                    {
                        if (FTarget.FTransporterID != -1)
                        {
                            var unit = AoWHSMap.Current.FUnitControl.FindUnit(FTarget.FTransporterID);
                            FGroupControl.FGroup.AddTransporter(unit);
                        }
                        else
                        {
                            var group = FGroupControl.FGroup;
                            if (group.FTransporter != null)
                            {
                                group.DeleteUnit(group.FTransporter);
                            }
                        }

                        if (AllowMultiTargets(FTarget))
                        {
                            FState = 30;
                        }
                        else
                        {
                            FGroupControl.FGroup.SetupTarget(FTarget, AIGroupTargetStatus.Execute, 0);
                            FGroupControl.SetupME();
                            FGroupControl.ExecuteME(FTarget.FLocation.X, FTarget.FLocation.Y, FTarget.FLocation.L);
                            Result = true;
                            FState = 21;
                        }
                    }
                    else
                    {
                        if (FGroupControl.FGroup.FProductions.Count != 0)
                        {
                            FGroupControl.FGroup.FProductions.GetProductions(0).FControl.GetLocation(ref Location.X, ref Location.Y, ref Location.L);
                            FGroupControl.SetupME();
                            FGroupControl.ExecuteME(Location.X, Location.Y, Location.L);
                            Result = true;
                            FState = 22;
                        }
                        else
                        {
                            FGroupControl.FProductionLocations.SortOnMovePoints();
                            if (FGroupControl.FProductionLocations.Count == 0 || FGroupControl.FProductionLocations.GetProductionLocation(0).FMovePoints > 31)
                            {
                                FGroupControl.SetupME();
                                FGroupControl.ExecuteME(FGroupControl.FLocation.X, FGroupControl.FLocation.Y, FGroupControl.FLocation.L);
                                Result = true;
                                FState = 22;
                            }
                            else
                            {
                                FGroupControl.SetupME();
                                var productionLocation = FGroupControl.FProductionLocations.GetProductionLocation(0);
                                var fProductionLocationsProductionLocation = FGroupControl.FProductionLocations.GetProductionLocation(0);
                                var productionLocation2 = FGroupControl.FProductionLocations.GetProductionLocation(0);
                                FGroupControl.ExecuteME(productionLocation2.FLocation.X, fProductionLocationsProductionLocation.FLocation.Y, productionLocation.FLocation.L);
                                Result = true;
                                FState = 22;
                            }
                        }
                    }

                    break;
                case 21:
                    if (FGroupControl.FTargets.FChildren.IndexOf(FTarget) == -1)
                    {
                        FGroupControl.FGroup.ResetTargetExpense();
                        FTargetStatus = AIGroupTargetSelectorStatus.TargetConquered;
                        Done();
                    }
                    else
                    {
                        FTargetStatus = AIGroupTargetSelectorStatus.ExecutingTarget;
                        Done();
                    }

                    break;
                case 22:
                    FGroupControl.FGroup.SetupTarget(FTarget, AIGroupTargetStatus.Setup, 0);
                    FTargetStatus = AIGroupTargetSelectorStatus.SettingupTarget;
                    Done();
                    break;
                case 30:
                    ValidateTargetRequirements(FGroupControl.FGroup, FTarget, null, FSpareUnits, FMinimumStrength);
                    if (FSpareUnits.Count != 0)
                    {
                        FTarget.FStatus = FTarget.FStatus - Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Valid);
                        FNextTarget = null;
                        FState = 31;
                    }
                    else
                    {
                        FState = 32;
                    }

                    break;
                case 31:
                    I = FGroupControl.FTargets.FChildren.IndexOf(FNextTarget);
                    while (I < FGroupControl.FTargets.Count && (I == -1 || FGroupControl.FTargets.GetTargets(I).FTransporterID != -1 || !FGroupControl.FTargets.GetTargets(I).FStatus.Contains(AIGroupControlTargetStatusElement.Valid)))
                    {
                        I += 1;
                    }

                    if (I < FGroupControl.FTargets.Count)
                    {
                        Required = new UnitUserList(null);
                        Optional = new UnitUserList(null);
                        FNextTarget = FGroupControl.FTargets.GetTargets(I);
                        if (ValidMultiTarget(FSpareUnits, FNextTarget, Required, Optional))
                        {
                            var group = FGroupControl.FGroup;
                            var behavior = group.GroupClass.Behavior;
                            Group = AoWHSMap.Current.FAIGroupManager.CreateGroup(group.FPlayer, group.FIndependentRelation, behavior);

                            Group.AddUnits(Required);
                            Group.SetupTarget(FNextTarget, AIGroupTargetStatus.Execute, 0);
                            Group.Release();
                            FSpareUnits.Clear();
                            FSpareUnits.AddUnits(Optional);
                            if (FSpareUnits.Count == 0)
                            {
                                FState = 32;
                            }
                        }

                        FNextTarget.FStatus = FNextTarget.FStatus - Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Valid);
                        Optional?.Dispose();
                        Required?.Dispose();
                    }
                    else
                    {
                        FState = 32;
                    }

                    break;
                case 32:
                    FGroupControl.FGroup.SetupTarget(FTarget, AIGroupTargetStatus.Execute, 0);
                    FGroupControl.SetupME();
                    FGroupControl.ExecuteME(FTarget.FLocation.X, FTarget.FLocation.Y, FTarget.FLocation.L);
                    FState = 21;
                    Result = true;
                    break;
                case 40:
                    NoTargetsFound();
                    Done();
                    break;
            }
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // AIGroup.pas:3067
    public bool TargetOverlapFound(AIGroupControlTarget Target, AIGroup IgnoreGroup)
    {
        Player Player;
        int I;
        AIGroup Group;
        Player = AoWHSMap.Current.FPlayers.GetPlayers(FGroupControl.FGroup.FPlayer);
        I = Player.FAIGroups.Count;
        bool Result = false;
        while (I != 0 && !Result)
        {
            I -= 1;
            Group = Player.FAIGroups.GetGroups(I);
            Result = Group != IgnoreGroup && Group.FTargetType == AIGroupTargetStatus.Execute && Target.FLocation.L == Group.FTargetXYL.L && HexGeometry.dHXtoRad(Group.FTargetXYL.X, Group.FTargetXYL.Y, Target.FLocation.X, Target.FLocation.Y) <= 1;
        }

        return Result;
    }

    // AIGroup.pas:3088
    public bool ValidateTargetOverlap(AIGroup Group, AIGroupControlTarget Target)
    {
        int I;
        int Moves;
        int Points;
        int OtherDays;
        int Days;
        Player Player;
        AIGroup Other;
        Player = AoWHSMap.Current.FPlayers.GetPlayers(FGroupControl.FGroup.FPlayer);
        I = Player.FAIGroups.Count;
        bool Result = true;
        while (I != 0 && Result)
        {
            I -= 1;
            Other = Player.FAIGroups.GetGroups(I);
            if (Other != Group && Other.FTargetType == AIGroupTargetStatus.Execute && Other.FTargetXYL.L == Target.FLocation.L && HexGeometry.dHXtoRad(Other.FTargetXYL.X, Other.FTargetXYL.Y, Target.FLocation.X, Target.FLocation.Y) <= 1)
            {
                Moves = Other.GetMovesMin();
                Points = Other.FTargetMovePoints;
                if (!Other.FStatus.Contains(AIGroupStatusElement.Done))
                {
                    Points -= Other.GetMovePointsMin();
                }

                OtherDays = Points / Moves;
                if (Points % Moves != 0)
                {
                    OtherDays += 1;
                }

                Moves = Group.GetMovesMin();
                var movePointsMin = Group.GetMovePointsMin();
                Points = Target.FMovePoints - movePointsMin;
                if (Points > 0)
                {
                    Days = Points / Moves;
                    if (Points % Moves != 0)
                    {
                        Days += 1;
                    }
                }
                else
                {
                    Days = 0;
                }

                Result = Days + 1 <= OtherDays;
            }
        }

        return Result;
    }

    // AIGroup.pas:3125
    public int GetValidTargets()
    {
        int Result = 0;
        for (int I = FGroupControl.FTargets.Count - 1; I >= 0; I--)
        {
            if (FGroupControl.FTargets.GetTargets(I).FStatus.Contains(AIGroupControlTargetStatusElement.Valid))
            {
                Result += 1;
            }
        }

        return Result;
    }

    // AIGroup.pas:3134
    public void Reset()
    {
        FState = 0;
        FTargetStatus = AIGroupTargetSelectorStatus.Idle;
        FSuperiority = (float)1.5;
        FNearSuperiority = (float)1.25;
        FMultiTargetFactor = (float)1.25;
        FMoveDayFactor = (float)0.1;
        FSpareUnits.Clear();
        FMode = Set<AIGroupTargetSelectorModeElement>.Of(AIGroupTargetSelectorModeElement.MultiTargets, AIGroupTargetSelectorModeElement.Cooperate, AIGroupTargetSelectorModeElement.Recruit, AIGroupTargetSelectorModeElement.Produce);
        FDamageTypes = AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall);
        FBattleFieldFactor = 1;
        FCurrentBattleFieldFactor = (float)0.8;
        FBattleFieldMoveFactor = 0;
        FBattleFieldRadius = 10;
        FValidTargetLimit = 5;
        FPathStrengthFactor = 1;
        FCurrentPathStrengthFactor = (float)0.8;
        FAllowTransport = false;
        FTargetLimit = 100;
        this.FPrioritySettings.FTargetFactor = 1;
        this.FPrioritySettings.FImmediateBonus = 500;
        this.FPrioritySettings.FSetupBonus = 25;
        this.FPrioritySettings.FExecuteBonus = 75;
        this.FPrioritySettings.FTransportPenalty = -100;
        this.FPrioritySettings.FPartialRecruitmentPenalty = 0;
        this.FPrioritySettings.FCompleteRecruitmentPenalty = -50;
        this.FPrioritySettings.FCompleteCooperationPenalty = -25;
        this.FPrioritySettings.FPartialCooperationPenalty = -50;
        this.FPrioritySettings.FMoveDayPenalty = -25;
        this.FPrioritySettings.FProductionDayPenalty = -25;
        this.FPrioritySettings.FPartialProductionPenalty = -10;
        this.FPrioritySettings.FFarOverlapPenalty = -199;
        this.FPrioritySettings.FNearOverlapPenalty = -50;
    }

    // AIGroup.pas:3173
    public void ActivatePlugin(AIGroupControlPlugin Plugin)
    {
        DeactivatePlugin();
        FPlugin = Plugin;
        FPlugin.AddRef();
        FPlugin.Activate(FGroupControl);
    }

    // AIGroup.pas:3181
    public void DeactivatePlugin()
    {
        if (FPlugin != null)
        {
            FPlugin.Release();
            FPlugin = null;
        }
    }

    // AIGroup.pas:3190
    public int GetTargetSetupPriority(AIGroupControlTarget Target)
    {
        AIGroup Group;
        int Days;
        int Priority;
        Priority = Round((double)Target.FData.Priority * FPrioritySettings.FTargetFactor);
        var movesMin = FGroupControl.FGroup.GetMovesMin();
        Days = (Target.FMovePoints - 1) / movesMin;
        if (Days > 0)
        {
            Priority += Round(FPrioritySettings.FMoveDayPenalty * Days);
        }

        if (Target.FTransporterID != -1)
        {
            Priority += FPrioritySettings.FTransportPenalty;
        }

        Group = FGroupControl.FGroup;
        if (Unsafe.As<XYL, int>(ref Group.FTargetXYL) == Unsafe.As<XYL, int>(ref Target.FLocation))
        {
            if (Group.FTargetType == AIGroupTargetStatus.Execute)
            {
                Priority += FPrioritySettings.FExecuteBonus;
            }
            else
            {
                if (Group.FTargetType == AIGroupTargetStatus.Setup)
                {
                    Priority += FPrioritySettings.FSetupBonus;
                }
            }
        }

        if (TargetOverlapFound(Target, Group))
        {
            if (Target.FMovePoints <= FMovesMin)
            {
                Priority += FPrioritySettings.FNearOverlapPenalty;
            }
            else
            {
                Priority += FPrioritySettings.FFarOverlapPenalty;
            }
        }

        return Priority;
    }

    // AIGroup.pas:3211
    public bool Setup(AIGroupControl Control)
    {
        Reset();
        switch ((int)AoWHSMap.Current.FPlayers.GetPlayers(Control.FGroup.FPlayer).FPlayerType)
        {
            case (int)PlayerType.CPU1:
                FValidTargetLimit = 5;
                break;
            case (int)PlayerType.CPU2:
                FValidTargetLimit = 7;
                break;
            case (int)PlayerType.CPU3:
            case (int)PlayerType.CPU4:
            case (int)PlayerType.CPU5:
                FValidTargetLimit = 10;
                break;
            default:
                FValidTargetLimit = 5;
                break;
        }

        return true;
    }

    // AIGroup.pas:3265
    public void TargetProcessed()
    {
        FTarget.FDataA = FPriority;
        FTarget.FStatus = FTarget.FStatus + Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Done);
        FTarget = null;
        FState = 0;
    }

    // AIGroup.pas:3273
    public float GetRequiredSuperiority(AIGroupControlTarget Target)
    {
        float Result;
        int Days;
        if (Target.FData.TargetType != AITargetType.Negotiate)
        {
            var group = FGroupControl.FGroup;
            if (Unsafe.As<XYL, int>(ref group.FTargetXYL) == Unsafe.As<XYL, int>(ref Target.FLocation) && group.FTargetType == AIGroupTargetStatus.Execute || group.GetMovesMin() >= Target.FMovePoints)
            {
                Result = FNearSuperiority;
            }
            else
            {
                Result = FSuperiority;
            }

            if (FGroupControl.FGroup.GetMovesMin() > 0)
            {
                var movesMin = FGroupControl.FGroup.GetMovesMin();
                Days = (Target.FMovePoints - 1) / movesMin;
                Result = (float)((double)Result + Round((double)Days * FMoveDayFactor));
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AIGroup.pas:3292
    public int GetRequiredCombatPredictorSuperiority(AIGroupControlTarget Target) => Round((double)GetRequiredSuperiority(Target) * 100);

    // AIGroup.pas:3297
    public int GetRequiredTargetStrength(AIGroupControlTarget Target)
    {
        int Result;
        DefenseRequirements Strengths = default;
        BattleFieldInfo Info = default;
        if (Target.FData.TargetType != AITargetType.Negotiate)
        {
            BattleFieldInfo.SetupBattleFieldInfo(ref Info, FGroupControl.FGroup.FPlayer, FGroupControl.FGroup.FIndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(), FDamageTypes);
            Info.FriendlyRelations = Set<byte>.WithBounds(0, 7);
            AoWHSMap.Current.RetrieveBattleFieldInfo(Target.FLocation.X, Target.FLocation.Y, Target.FLocation.L, 1, ref Info);
            Result = Round((double)GetRequiredSuperiority(Target) * Info.Strength);
            ((AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(Unsafe.As<XYL, int>(ref Target.FLocation))).GetDefenseRequirements(FGroupControl.FGroup.FPlayer, ref Strengths);
            Result += StrengthInfoMax(Bytes.Of(ref Strengths)) / 2;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AIGroup.pas:3317
    public float GetRequiredBattleFieldSuperiority(AIGroupControlTarget Target)
    {
        float Result;
        if (Unsafe.As<XYL, int>(ref Target.FLocation) == Unsafe.As<XYL, int>(ref FGroupControl.FGroup.FTargetXYL) && FGroupControl.FGroup.FTargetType == AIGroupTargetStatus.Execute)
        {
            Result = FCurrentBattleFieldFactor;
        }
        else
        {
            Result = FBattleFieldFactor;
        }

        return (float)((double)Result + Round((double)Target.FMovePoints * FBattleFieldMoveFactor));
    }

    // AIGroup.pas:3325
    public int GetRequiredBattleFieldStrength(AIGroupControlTarget Target)
    {
        int Result;
        BattleFieldInfo Info = default;
        int PathStrength;
        if (((double)FBattleFieldFactor > 0 || (double)FCurrentBattleFieldFactor > 0) && Target.FData.TargetType != AITargetType.Negotiate)
        {
            BattleFieldInfo.SetupBattleFieldInfo(ref Info, FGroupControl.FGroup.FPlayer, FGroupControl.FGroup.FIndependentRelation, Set<BattleFieldInfoRequestsElement>.Of(), FDamageTypes);
            Info.Units = FGroupControl.FGroup;
            Info.Requests = Info.Requests + Set<BattleFieldInfoRequestsElement>.Of(BattleFieldInfoRequestsElement.IgnorePassiveIndependents);
            AoWHSMap.Current.RetrieveBattleFieldInfo(Target.FLocation.X, Target.FLocation.Y, Target.FLocation.L, FBattleFieldRadius, ref Info);
            Result = Round((double)GetRequiredBattleFieldSuperiority(Target) * (Info.Strength - Info.FriendlyStrength));
        }
        else
        {
            Result = 0;
        }

        var group = FGroupControl.FGroup;
        if (Unsafe.As<XYL, int>(ref group.FTargetXYL) == Unsafe.As<XYL, int>(ref Target.FLocation) && group.FTargetType == AIGroupTargetStatus.Execute)
        {
            PathStrength = Round((double)Target.FPathStrength * FCurrentPathStrengthFactor);
        }
        else
        {
            PathStrength = Round((double)Target.FPathStrength * FPathStrengthFactor);
        }

        if (Result < PathStrength)
        {
            Result = PathStrength;
        }

        return Result;
    }

    // AIGroup.pas:3349
    public int GetRequiredStrength(AIGroupControlTarget Target)
    {
        int BattleFieldStrength;
        int Result = GetRequiredTargetStrength(Target);
        BattleFieldStrength = GetRequiredBattleFieldStrength(Target);
        if (BattleFieldStrength > Result)
        {
            Result = BattleFieldStrength;
        }

        AoWHSMap.Current.FAIExecuter.UseProcessTime(250);
        return Result;
    }

    // AIGroup.pas:3359
    public bool ValidateTargetRequirements(UnitList Units, AIGroupControlTarget Target, UnitList Required, UnitList Optional, int MinimumStrength)
    {
        int Strength;
        CombatPredictor Predictor;
        UnitUserList RequiredUnits;
        UnitUserList OptionalUnits;
        int Superiority;
        AbstractUnit AUnit;
        Predictor = new CombatPredictor();
        RequiredUnits = new UnitUserList(null);
        OptionalUnits = new UnitUserList(null);
        Predictor.AddUnits(Units);
        if (((AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(Unsafe.As<XYL, int>(ref Target.FLocation))).GetMoveAction(FGroupControl.FGroup.FPlayer) != MoveAction.Negotiate)
        {
            Predictor.InitAdjacentOpponents((byte)FGroupControl.FGroup.FPlayer, (byte)FGroupControl.FGroup.FIndependentRelation, Target.FLocation.X, Target.FLocation.Y, Target.FLocation.L);
        }

        Superiority = GetRequiredCombatPredictorSuperiority(Target);
        Predictor.Execute();
        bool Result = Predictor.GetSuperiority() > Superiority;
        Predictor.ListRequiredUnits(RequiredUnits, Superiority);
        if (RequiredUnits.Count != Units.Count)
        {
            for (int I = Units.Count - 1; I >= 0; I--)
            {
                if (RequiredUnits.IndexOf(Units.GetUnit(I)) == -1)
                {
                    OptionalUnits.AddUnit(Units.GetUnit(I));
                }
            }
        }

        if (OptionalUnits.Count != 0 || Result)
        {
            Strength = GetRequiredStrength(Target);
            if (Strength < MinimumStrength)
            {
                Strength = MinimumStrength;
            }

            Strength -= RequiredUnits.GetStrengthMax(FDamageTypes);
            while (Strength > 0 && OptionalUnits.Count != 0)
            {
                AUnit = OptionalUnits.GetUnit(OptionalUnits.Count - 1);
                Strength -= AUnit.GetStrengthMax(FDamageTypes);
                RequiredUnits.AddUnit(AUnit);
                OptionalUnits.DeleteUnit(AUnit);
            }

            Result = Result && Strength <= 0;
        }

        if (Required != null)
        {
            Required.AddUnits(RequiredUnits);
        }

        if (Optional != null)
        {
            Optional.AddUnits(OptionalUnits);
        }

        OptionalUnits?.Dispose();
        RequiredUnits?.Dispose();
        Predictor?.Dispose();
        return Result;
    }

    // AIGroup.pas:3406
    public bool ValidMultiTarget(UnitList Units, AIGroupControlTarget Target, UnitList Required, UnitList Optional)
    {
        bool Result;
        if (!TargetOverlapFound(FNextTarget, null) && (FTarget.FLocation.L != Target.FLocation.L || HexGeometry.dHXtoRad(FTarget.FLocation.X, FTarget.FLocation.Y, Target.FLocation.X, Target.FLocation.Y) > 1))
        {
            Result = ValidateTargetRequirements(FSpareUnits, FNextTarget, Required, Optional, FMinimumStrength);
            bool condition = Result;
            if (condition)
            {
                var targetMana = FTarget.GetTargetMana();
                var targetTargetMana = Target.GetTargetMana();
                var targetGold = FTarget.GetTargetGold();
                var targetTargetGold = Target.GetTargetGold();
                condition = FGroupControl.FGroup.RequestTargetExpense(targetGold + targetTargetGold, targetMana + targetTargetMana);
            }

            Result = condition;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AIGroup.pas:3441
    public void ShowTargetInfo()
    {
        string Text = "";
        AIGroupControlTarget Target;
        GameTables.AoWEngine.OutputDebugMessage(AbilityRules.AIDID, "Targets for Group (ID:" + IntToStr(FGroupControl.FGroup.FGroupID) + ")");
        for (int I = 0, last = FGroupControl.FTargets.Count - 1; I <= last; I++)
        {
            Target = FGroupControl.FTargets.GetTargets(I);
            Target.GetDebugInfo(ref Text);
            Text = Text + "/TS_Prio:" + IntToStr(Target.FDataA);
            Text = Text + "/MPBFS:" + IntToStr(Target.FPathStrength);
            Text = Text + "/TG:" + IntToStr(Target.GetTargetGold());
            Text = Text + "/MP:" + IntToStr(Target.FMovePoints);
            Text = Text + "/Status:";
            if (Target.FStatus.Contains(AIGroupControlTargetStatusElement.Cooperate))
            {
                Text = Text + "c";
            }

            if (Target.FStatus.Contains(AIGroupControlTargetStatusElement.Recruit))
            {
                Text = Text + "r";
            }

            if (Target.FStatus.Contains(AIGroupControlTargetStatusElement.Produce))
            {
                Text = Text + "p";
            }

            if (Target.FStatus.Contains(AIGroupControlTargetStatusElement.Valid))
            {
                Text = Text + "v";
            }

            if (Target.FStatus.Contains(AIGroupControlTargetStatusElement.Done))
            {
                Text = Text + "d";
            }

            GameTables.AoWEngine.OutputDebugMessage(AbilityRules.AIDID, Text);
        }
    }

    // AIGroup.pas:3467
    public void NoTargetsFound()
    {
        FTargetStatus = AIGroupTargetSelectorStatus.NoTargetsFound;
        FGroupControl.FGroup.FTargetXYL.Packed = -1;
        FGroupControl.FGroup.FTargetType = AIGroupTargetStatus.None;
        if (FGroupControl.FGroup.FTransporter != null)
        {
            FGroupControl.FGroup.DeleteUnit(FGroupControl.FGroup.FTransporter);
        }
    }

    // AIGroup.pas:3476
    public bool AllowMultiTargets(AIGroupControlTarget Target)
    {
        bool Result = FMode.Contains(AIGroupTargetSelectorModeElement.MultiTargets);
        Result = Result && (Target.FStatus & Set<AIGroupControlTargetStatusElement>.Of(AIGroupControlTargetStatusElement.Recruit, AIGroupControlTargetStatusElement.Produce)) == Set<AIGroupControlTargetStatusElement>.Of();
        bool condition = Result;
        if (condition)
        {
            var requiredStrength = GetRequiredStrength(Target);
            condition = (double)FGroupControl.FStrengthMax >= (double)requiredStrength * FMultiTargetFactor;
        }

        Result = condition;
        Result = Result && FGroupControl.FStrengthMax >= FMinimumStrength;
        bool test = Result;
        if (test)
        {
            bool passed = Target.FData.TargetType != AITargetType.Negotiate;
            if (!passed)
            {
                var movePointsMin = FGroupControl.FGroup.GetMovePointsMin();
                passed = Target.FMovePoints >= movePointsMin;
            }

            test = passed;
        }

        return test;
    }

    private static int StrengthInfoMax(ReadOnlySpan<byte> Strengths)
    {
        int num = 0;
        for (int i = 0; i <= 9; i++)
        {
            DamageType tDamageType = (DamageType)i;
            if (MemoryMarshal.Cast<byte, FixedArray_int_10>(Strengths)[0][(int)tDamageType] > num)
            {
                num = MemoryMarshal.Cast<byte, FixedArray_int_10>(Strengths)[0][(int)tDamageType];
            }
        }
        return num;
    }

    private static int CompareTargetSetupPriority(object Item1, object Item2)
    {
        return ((AIGroupControlTarget)Item2).FDataA - ((AIGroupControlTarget)Item1).FDataA;
    }

    // target factor, immediate bonus, transport penalty, overlap penalties, existing-target
    // bonuses, cooperation penalties, recruitment penalties, move-day penalty, production penalties
    public partial class AIGroupTargetPrioritySettings
    {
        public float FTargetFactor;
        public int FImmediateBonus;
        public int FTransportPenalty;
        public int FFarOverlapPenalty;
        public int FNearOverlapPenalty;
        public int FSetupBonus;
        public int FExecuteBonus;
        public int FCompleteCooperationPenalty;
        public int FPartialCooperationPenalty;
        public int FPartialRecruitmentPenalty;
        public int FCompleteRecruitmentPenalty;
        public int FMoveDayPenalty;
        public int FPartialProductionPenalty;
        public int FProductionDayPenalty;
        // System.pas:111

    }

    // intf_P5_07.inc:624

    // intf_P5_07.inc:624

}
