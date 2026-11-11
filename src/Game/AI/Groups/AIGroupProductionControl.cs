// Translated from AoWE.pas.
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// control, mode, completion, costs, upkeep, productions, candidates, remaining strengths,
// remainder, requested strength, damage types, requested strengths, priority range, multiple,
// group ID, superiority, move-point priority, allow research, research priority, move types,
// minimum moves, path-strength factor
public partial class AIGroupProductionControl : AIGroupTask
{
    // AIGroup.pas:2440
    public AIGroupProductionControl(ECustomNode owner) : base(owner)
    {
        FProductions = new AIGroupUnitProductionList(null);
        FProduceableUnits = new AIGroupUnitProductionList(null);
    }

    public AIGroupControl FControl;
    public Mode FMode;
    public bool FComplete;
    public int FGold;
    public int FMana;
    public int FGoldUpkeep;
    public int FManaUpkeep;
    public AIGroupUnitProductionList FProductions;
    public AIGroupUnitProductionList FProduceableUnits;
    public Int32Array10 FRemainingStrengths;
    public int FRemainingStrength;
    public int FProduceStrength;
    public Set<DamageType> FDamageTypes;
    public Int32Array10 FProduceStrengths;
    public int FPriorityRange;
    public bool FMultiple;
    public int FGroupID;
    public int FSuperiority;
    public int FMovePointPriority;
    public bool FAllowResearch;
    public int FResearchPriority;
    public Set<MoveType> FMoveTypes;
    public int FMovesMin;
    public float FPathStrengthFactor;

    // AIGroup.pas:2447
    public override void Dispose()
    {
        FProductions?.Dispose();
        FProductions = null;
        FProduceableUnits?.Dispose();
        FProduceableUnits = null;
        base.Dispose();
    }

    // AIGroup.pas:2471
    public override bool GetBusy() => FState != -1;

    // AIGroup.pas:2654
    public override bool Activate(AIGroupControl Control)
    {
        FControl = Control;
        FRemainingStrength = FProduceStrength;
        UpdateCompletion();
        return true;
    }

    // AIGroup.pas:2772
    public override bool Process()
    {
        int Index;
        AIGroupUnitProduction Production;
        bool Finished;
        switch (FState)
        {
            case 0:
                if (FControl.FGroup.FProductions.Count == 0)
                {
                    AoWHSMap.Current.FAIExecuter.UseProcessTime(500);
                    Finished = false;
                    FControl.FProductionLocations.Validate(FControl);
                    while (!FComplete && !Finished)
                    {
                        UpdateProduceableUnits();
                        if (FProduceableUnits.Count != 0)
                        {
                            Index = 0;
                            while (true)
                            {
                                bool condition = FProduceableUnits.Count > Index + 1;
                                if (condition)
                                {
                                    var unitProductions = FProduceableUnits.GetUnitProductions(Index + 1);
                                    var fProduceableUnitsUnitProductions = FProduceableUnits.GetUnitProductions(0);
                                    condition = unitProductions.FPriority > fProduceableUnitsUnitProductions.FPriority - FPriorityRange;
                                }

                                if (!condition)
                                    break;
                                Index += 1;
                            }

                            // { Original passes the last eligible index, not the candidate count. }
                            Index = AoWHSMap.Current.Random(Index);
                            AddProduction(FProduceableUnits.GetUnitProductions(Index));
                            if (!FMultiple)
                            {
                                Finished = true;
                            }
                        }
                        else
                        {
                            Finished = true;
                        }
                    }

                    if ((FProductions.Count != 0 && FMode == Mode.AutoProduce) || (FMode == Mode.CompleteProduceOnly && FComplete))
                    {
                        ExecuteProductions(FGroupID);
                    }

                    Done();
                }
                else
                {
                    for (int I = 0, last = FControl.FGroup.FProductions.Count - 1; I <= last; I++)
                    {
                        Production = new AIGroupUnitProduction(null);
                        Production.SetU(FControl.FGroup.FProductions.GetProductions(I).FControl.FSettings.FUnit);
                        if (Production.FU != null)
                        {
                            Production.FGold = 0;
                            Production.FMana = 0;
                            Production.FGoldUpkeep = 0;
                            Production.FManaUpkeep = 0;
                            var productions = FControl.FGroup.FProductions.GetProductions(I);
                            Production.SetProductionLocation(FControl.FProductionLocations.FindPC(productions.FControl));
                            Production.FPriority = 0;
                            Production.FResearch = false;
                            if (Production.FProductionLocation != null)
                            {
                                AddProduction(Production);
                            }
                        }

                        Production.Release();
                    }

                    Done();
                }

                break;
            default:
                Done();
                break;
        }

        return false;
    }

    // AIGroup.pas:2647
    public virtual bool Setup(AIGroupControl Control)
    {
        Reset();
        FGroupID = Control.FGroup.FGroupID;
        return true;
    }

    // AIGroup.pas:2456
    public int GetProducedStrength() => FProduceStrength - FRemainingStrength;

    // AIGroup.pas:2461
    public int GetProduceStrengthEx(DamageType DamageType) => FProduceStrengths[(int)DamageType];

    // AIGroup.pas:2466
    public void SetProduceStrengthEx(DamageType DamageType, int Value)
    {
        FProduceStrengths[(int)DamageType] = Value;
    }

    // AIGroup.pas:2476
    public int UnitProductionPriority(Unit AUnit)
    {
        bool Usefull;
        DamageType D;
        int Result = -1;
        Usefull = AUnit.GetMoves() > FMovesMin && (AUnit.GetMoveTypes() & FMoveTypes) != Set<MoveType>.Of();
        Usefull = Usefull && AUnit.GetUnitMoraleValue(FControl.FGroup.FPlayer, FControl.FGroup.FIndependentRelation) >= 50;
        if (Usefull && FControl.FGroup.FPlayer == 0)
        {
            Usefull = FControl.FGroup.FIndependentRelation < 0 || AUnit.GetIndependentRelation() < 0 || AUnit.GetIndependentRelation() == FControl.FGroup.FIndependentRelation;
        }

        if (Usefull)
        {
            Usefull = FRemainingStrength > 0 && AUnit.GetStrengthMax(FDamageTypes) > 0;
            if (!Usefull)
            {
                for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
                {
                    D = (DamageType)dOrdinal;
                    Usefull = FRemainingStrengths[(int)D] > 0 && AUnit.GetStrength(D) > 0;
                    // { Original bug: a useful per-type strength returns the initial -1 priority. }
                    if (Usefull)
                    {
                        return Result;
                    }
                }

                if (!Usefull && FSuperiority != -1)
                {
                    Usefull = (FControl.FCombatPredictor.Usefullness(AUnit) & 8388608) != 0;
                    if (Usefull)
                    {
                        Result += 50;
                    }
                }
            }
        }

        if (Usefull)
        {
            Result += RosterTables.GetUnitIndexAIPriority(AUnit.FUnitResource.UnitResourceIndex(), FControl.FGroup.FPlayer, FControl.FGroup.FIndependentRelation);
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // AIGroup.pas:2514
    public int GetCurrentProductionTime()
    {
        int Result = -1;
        for (int I = 0, last = FProductions.Count - 1; I <= last; I++)
        {
            if (FProductions.GetUnitProductions(I).FProductionTime > Result)
            {
                Result = FProductions.GetUnitProductions(I).FProductionTime;
            }
        }

        return Result;
    }

    // AIGroup.pas:2524
    public int GetCompleteProductionTime()
    {
        int Result = GetCurrentProductionTime();
        if (Result != -1 && FRemainingStrength > 0 && GetProducedStrength() > 0)
        {
            var producedStrength = GetProducedStrength();
            Result += (int)((double)((double)((double)FRemainingStrength / producedStrength) * Result) + 0.999);
        }

        return Result;
    }

    // AIGroup.pas:2531
    public void UpdateCompletion()
    {
        DamageType D;
        FComplete = FRemainingStrength <= 0;
        if (FComplete)
        {
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                if (FRemainingStrengths[(int)D] > 0)
                {
                    FComplete = false;
                    return;
                }
            }

            if (FSuperiority != -1)
            {
                FControl.FCombatPredictor.Execute();
                if (FControl.FCombatPredictor.GetSuperiority() < FSuperiority)
                {
                    FComplete = false;
                }
            }
        }
    }

    // AIGroup.pas:2553
    public bool ExecuteProductions(int GroupID)
    {
        AIGroupUnitProduction Production;
        ProductionControl Control;
        bool Result = true;
        for (int I = 0, last = FProductions.Count - 1; I <= last; I++)
        {
            Production = FProductions.GetUnitProductions(I);
            Control = Production.FProductionLocation.FProductionControl;
            if (!Production.FResearch)
            {
                var unitResourceIndex = Production.FU.FUnitResource.UnitResourceIndex();
                if (Control.ExecuteAIProduction(Control.FUnitProductions.IndexOf(unitResourceIndex), GroupID))
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(FControl.FGroup.FPlayer).FAIBudget.ProductionExpenseMade(Production.FGold, Production.FGoldUpkeep, Production.FMana, Production.FManaUpkeep);
                }
                else
                {
                    Result = false;
                }
            }
            else
            {
                var unitResourceIndex = Production.FU.FUnitResource.UnitResourceIndex();
                if (Control.ExecuteAIResearch(Control.FUnitProductions.IndexOf(unitResourceIndex)))
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(FControl.FGroup.FPlayer).FAIBudget.ProductionExpenseMade(Production.FGold, Production.FGoldUpkeep, Production.FMana, Production.FManaUpkeep);
                }
            }
        }

        return Result;
    }

    // AIGroup.pas:2581
    public void AddProduction(AIGroupUnitProduction Production)
    {
        DamageType D;
        FProductions.AddChild(Production);
        FGold += Production.FGold;
        FMana += Production.FMana;
        // { Original bug: upkeep is based on total costs rather than accumulated upkeep. }
        FGoldUpkeep = FGold + Production.FGoldUpkeep;
        FManaUpkeep = FMana + Production.FManaUpkeep;
        for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
        {
            D = (DamageType)dOrdinal;
            var strength = Production.FU.GetStrength(D);
            FRemainingStrengths[(int)D] -= strength;
        }

        var strengthMax = Production.FU.GetStrengthMax(FDamageTypes);
        FRemainingStrength -= strengthMax;
        if (FSuperiority != -1)
        {
            FControl.FCombatPredictor.AddUnit(Production.FU);
        }

        UpdateCompletion();
    }

    // AIGroup.pas:2599
    public void DeleteProduction(int Index)
    {
        AIGroupUnitProduction Production;
        DamageType D;
        if (Index >= 0 && Index < FProductions.Count)
        {
            Production = FProductions.GetUnitProductions(Index);
            FGold -= Production.FGold;
            FMana -= Production.FMana;
            FGoldUpkeep -= Production.FGoldUpkeep;
            FManaUpkeep -= Production.FManaUpkeep;
            for (int dOrdinal = (int)(DamageType)0; dOrdinal <= (int)(DamageType)9; dOrdinal++)
            {
                D = (DamageType)dOrdinal;
                var strength = Production.FU.GetStrength(D);
                FRemainingStrengths[(int)D] += strength;
            }

            var strengthMax = Production.FU.GetStrengthMax(FDamageTypes);
            FRemainingStrength += strengthMax;
            if (FSuperiority != -1)
            {
                FControl.FCombatPredictor.DeleteUnit(Production.FU);
            }

            FProductions.RemoveChild(Production);
            UpdateCompletion();
        }
    }

    // AIGroup.pas:2621
    public void Reset()
    {
        FProductions.Clear();
        FState = 0;
        FMovesMin = 0;
        FSuperiority = -1;
        // { Original clears the production list twice and leaves the candidate list alone. }
        FProductions.Clear();
        FGold = 0;
        FMana = 0;
        FGoldUpkeep = 0;
        FManaUpkeep = 0;
        FProduceStrength = 0;
        FDamageTypes = AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall);
        FProduceStrengths = default;
        FMoveTypes = Set<MoveType>.Of(MoveType.Walking, MoveType.Flying);
        FComplete = false;
        FMode = Mode.AutoProduce;
        FMultiple = false;
        FGroupID = 0;
        FMovePointPriority = -5;
        FResearchPriority = -500;
        FAllowResearch = true;
        FPathStrengthFactor = (float)0.8;
    }

    // AIGroup.pas:2662
    public bool ListProduceableUnits(AIGroupProductionLocation Location)
    {
        AIGroupUnitProduction Production;
        int Priority;
        Unit TestUnit;
        Unit NewUnit;
        bool Result = false;
        if (Location.FProductionControl.FStatus == ProductionControl.Status.Idle)
        {
            TestUnit = new Unit(null);
            for (int I = 0, last = Location.FProductionControl.FUnitProductions.Count - 1; I <= last; I++)
            {
                if (Location.FProductionControl.FUnitProductions.GetItem(I).FUnitIndex != -1)
                {
                    if (Location.FProductionControl.FUnitProductions.GetItem(I).FResearched)
                    {
                        var unitProductionManaUpkeep = Location.FProductionControl.UnitProductionManaUpkeep(I);
                        var unitProductionMana = Location.FProductionControl.UnitProductionMana(I);
                        var unitProductionGoldUpkeep = Location.FProductionControl.UnitProductionGoldUpkeep(I);
                        var unitProductionGold = Location.FProductionControl.UnitProductionGold(I);
                        if (AoWHSMap.Current.FPlayers.GetPlayers(FControl.FGroup.FPlayer).FAIBudget.RequestProductionExpense(unitProductionGold + FGold, unitProductionGoldUpkeep + FGoldUpkeep, unitProductionMana + FMana, unitProductionManaUpkeep + FManaUpkeep))
                        {
                            var unitProduction = Location.FProductionControl.FUnitProductions.GetItem(I);
                            TestUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(unitProduction.FUnitIndex));
                            Priority = UnitProductionPriority(TestUnit);
                            if (Priority != -1)
                            {
                                NewUnit = new Unit(null);
                                var fUnitProductionsItem = Location.FProductionControl.FUnitProductions.GetItem(I);
                                NewUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(fUnitProductionsItem.FUnitIndex));
                                Production = new AIGroupUnitProduction(null);
                                Production.SetU(NewUnit);
                                Production.FGold = Location.FProductionControl.UnitProductionGold(I);
                                Production.FMana = Location.FProductionControl.UnitProductionMana(I);
                                Production.FGoldUpkeep = Location.FProductionControl.UnitProductionGoldUpkeep(I);
                                Production.FManaUpkeep = Location.FProductionControl.UnitProductionManaUpkeep(I);
                                Production.SetProductionLocation(Location);
                                Production.FPriority = FMovePointPriority * Location.FMovePoints + Priority;
                                Production.FResearch = false;
                                Production.FProductionTime = Location.FProductionControl.UnitProductionTime(I);
                                FProduceableUnits.AddChild(Production);
                                Production.Release();
                                NewUnit.Release();
                                Result = true;
                            }
                        }
                    }
                    else
                    {
                        if (FAllowResearch)
                        {
                            var unitResearchMana = Location.FProductionControl.UnitResearchMana(I);
                            var unitResearchGold = Location.FProductionControl.UnitResearchGold(I);
                            if (AoWHSMap.Current.FPlayers.GetPlayers(FControl.FGroup.FPlayer).FAIBudget.RequestProductionExpense(unitResearchGold + FGold, FGoldUpkeep, unitResearchMana + FMana, FManaUpkeep))
                            {
                                var item = Location.FProductionControl.FUnitProductions.GetItem(I);
                                TestUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(item.FUnitIndex));
                                Priority = UnitProductionPriority(TestUnit);
                                if (Priority != -1)
                                {
                                    NewUnit = new Unit(null);
                                    var fUnitProductionsItem = Location.FProductionControl.FUnitProductions.GetItem(I);
                                    NewUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(fUnitProductionsItem.FUnitIndex));
                                    Production = new AIGroupUnitProduction(null);
                                    Production.SetU(NewUnit);
                                    Production.FGold = Location.FProductionControl.UnitResearchGold(I);
                                    Production.FMana = Location.FProductionControl.UnitResearchMana(I);
                                    Production.FGoldUpkeep = Location.FProductionControl.UnitProductionGoldUpkeep(I);
                                    Production.FManaUpkeep = Location.FProductionControl.UnitProductionManaUpkeep(I);
                                    Production.SetProductionLocation(Location);
                                    Production.FPriority = FMovePointPriority * Location.FMovePoints + Priority + FResearchPriority;
                                    Production.FResearch = true;
                                    FProduceableUnits.AddChild(Production);
                                    Production.Release();
                                    NewUnit.Release();
                                    Result = true;
                                }
                            }
                        }
                    }
                }
            }

            TestUnit?.Dispose();
        }

        return Result;
    }

    // AIGroup.pas:2743
    public bool ValidProductionLocation(AIGroupProductionLocation Location)
    {
        bool Result;
        if (Location.FPathStrength != 0)
        {
            Result = FControl.FGroup.GetStrengthMax(AbilityRules.dtAll - Set<DamageType>.Of(DamageType.Wall)) >= Round((double)Location.FPathStrength * FPathStrengthFactor) || FControl.FGroup.GetMovePointsMin() >= Location.FMovePoints;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // AIGroup.pas:2761
    public void UpdateProduceableUnits()
    {
        FProduceableUnits.Clear();
        for (int I = 0, last = FControl.FProductionLocations.Count - 1; I <= last; I++)
        {
            if (ValidProductionLocation(FControl.FProductionLocations.GetProductionLocation(I)))
            {
                ListProduceableUnits(FControl.FProductionLocations.GetProductionLocation(I));
            }
        }

        FProduceableUnits.FChildren.Sort(CompareProduceableUnits);
    }

    private static int CompareProduceableUnits(object Item1, object Item2)
    {
        return ((AIGroupUnitProduction)Item2).FPriority - ((AIGroupUnitProduction)Item1).FPriority;
    }

    public enum Mode : byte
    {
        None = 0, AutoProduce = 1, CompleteProduceOnly = 2
    }

    // intf_P5_07.inc:537

    // intf_P5_07.inc:537

}
