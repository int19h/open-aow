// Translated from AoWE.pas.
using Aow.Engine.Collections;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Combat;

// { Interface declaration run owned by P3. }
// FUnit. FCPUID. FPredictor. FInitialHitPoints. FSide. FOpponents. FEnabled. FBehindWall.
// FTargets. FCombatValue. FWallTargets. FWallCombatValue. cache flags. FRoundTargets.
// FCombatValueTotal. FCombatRounds. FIncomingCombatValue. FIneffectiveAttackers.
// FTargetPriority. FHitPoints. FStatus
public partial class CombatPredictorUnit : IDisposable
{
    // CombatPredictor.pas:1
    public CombatPredictorUnit()
    {
        FRoundTargets = new QuadItemList<CombatPredictorUnit>();
    }

    public AbstractUnit FUnit;
    public int FCPUID;
    public CombatPredictor FPredictor;
    public int FInitialHitPoints;
    public byte FSide;
    public CombatPredictorSide FOpponents;
    public byte FEnabled;
    public byte FBehindWall;
    public QuadItemList<CombatPredictorUnit> FTargets;
    public int FCombatValue;
    public QuadItemList<CombatPredictorUnit> FWallTargets;
    public int FWallCombatValue;
    public byte FTargetsInvalid;
    public byte FWallTargetsInvalid;
    public QuadItemList<CombatPredictorUnit> FRoundTargets;
    public int FCombatValueTotal;
    public int FCombatRounds;
    public int FIncomingCombatValue;
    public int FIneffectiveAttackers;
    public int FTargetPriority;
    public int FHitPoints;
    public Set<CombatPredictorUnitStatusElement> FStatus;

    // CombatPredictor.pas:4
    public virtual void Dispose()
    {
        object Obj;
        AbstractUnit U;
        FRoundTargets?.Dispose();
        Obj = FTargets;
        if (Obj != null)
        {
            (Obj as IDisposable)?.Dispose();
            FTargets = null;
        }

        Obj = FWallTargets;
        if (Obj != null)
        {
            (Obj as IDisposable)?.Dispose();
            FWallTargets = null;
        }

        U = FUnit;
        if (U != null)
        {
            U.Release();
            FUnit = null;
        }
    }

    // CombatPredictor.pas:36
    public void SetEnabled(byte Value)
    {
        if (Value != FEnabled)
        {
            FEnabled = Value;
            FPredictor.FInvalid = 1;
        }
    }

    // CombatPredictor.pas:45
    public void UpdateTargets()
    {
        int J;
        int V;
        CombatPredictorUnit U;
        if (FTargets == null)
        {
            FTargets = new QuadItemList<CombatPredictorUnit>();
        }

        for (int I = FTargets.Count - 1; I >= 0; I--)
        {
            J = FOpponents.IndexOfCPUID(FTargets.List[I].C);
            if (J == -1)
            {
                FTargets.Delete(I);
                // { Original reads the shifted entry after deletion. }
                FCombatValue -= FTargets.List[I].B;
            }
        }

        if (FTargets.Count != 0)
        {
            for (int I = FOpponents.Count - 1; I >= 0; I--)
            {
                U = FOpponents.GetUnits(I);
                J = FTargets.IndexOfItemC(U.FCPUID);
                if (J == -1)
                {
                    FPredictor.FWork += 1;
                    V = FUnit.GetCombatValue(U.FUnit, 0);
                    if (V < 0)
                    {
                        V = 0;
                    }

                    FTargets.AddABCD(U, V, U.FCPUID, 0);
                    FCombatValue += V;
                }
            }
        }
        else
        {
            FCombatValue = 0;
            for (int I = FOpponents.Count - 1; I >= 0; I--)
            {
                FPredictor.FWork += 1;
                U = (CombatPredictorUnit)FOpponents.List[I];
                V = FUnit.GetCombatValue(U.FUnit, 0);
                if (V < 0)
                {
                    V = 0;
                }

                FTargets.AddABCD(U, V, U.FCPUID, 0);
                FCombatValue += V;
            }
        }

        FTargetsInvalid = 0;
    }

    // CombatPredictor.pas:89
    public void UpdateWallTargets()
    {
        int J;
        int V;
        CombatPredictorUnit U;
        bool Passing;
        byte Mode;
        Passing = FUnit.GetWallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing);
        if (FWallTargets == null)
        {
            FWallTargets = new QuadItemList<CombatPredictorUnit>();
        }

        for (int I = FWallTargets.Count - 1; I >= 0; I--)
        {
            J = FOpponents.IndexOfCPUID(FWallTargets.List[I].C);
            if (J == -1)
            {
                FWallTargets.Delete(I);
                // { Original reads the shifted entry after deletion. }
                FWallCombatValue -= FWallTargets.List[I].B;
            }
        }

        if (FWallTargets.Count != 0)
        {
            for (int I = FOpponents.Count - 1; I >= 0; I--)
            {
                U = (CombatPredictorUnit)FOpponents.List[I];
                J = FWallTargets.IndexOfItemC(U.FCPUID);
                if (J == -1)
                {
                    if (!Passing)
                    {
                        if (FBehindWall != 0)
                        {
                            if (U.FBehindWall == 0)
                            {
                                Mode = 1;
                            }
                            else
                            {
                                Mode = 0;
                            }
                        }
                        else
                        {
                            if (U.FBehindWall != 0)
                            {
                                Mode = 2;
                            }
                            else
                            {
                                Mode = 0;
                            }
                        }
                    }
                    else
                    {
                        Mode = 0;
                    }

                    FPredictor.FWork += 1;
                    V = FUnit.GetCombatValue(U.FUnit, Mode);
                    if (V < 0)
                    {
                        V = 0;
                    }

                    FWallTargets.AddABCD(U, V, 0, 0);
                    FWallCombatValue += V;
                }
            }
        }
        else
        {
            FWallCombatValue = 0;
            for (int I = FOpponents.Count - 1; I >= 0; I--)
            {
                U = (CombatPredictorUnit)FOpponents.List[I];
                if (!Passing)
                {
                    if (FBehindWall != 0)
                    {
                        if (U.FBehindWall == 0)
                        {
                            Mode = 1;
                        }
                        else
                        {
                            Mode = 0;
                        }
                    }
                    else
                    {
                        if (U.FBehindWall != 0)
                        {
                            Mode = 2;
                        }
                        else
                        {
                            Mode = 0;
                        }
                    }
                }
                else
                {
                    Mode = 0;
                }

                FPredictor.FWork += 1;
                V = FUnit.GetCombatValue(U.FUnit, Mode);
                if (V < 0)
                {
                    V = 0;
                }

                // { Original initializes both C and D to zero in the wall cache. }
                FWallTargets.AddABCD(U, V, 0, 0);
                FWallCombatValue += V;
            }
        }

        FWallTargetsInvalid = 0;
    }

    // CombatPredictor.pas:145
    public void InvalidateTargets()
    {
        FTargetsInvalid = 1;
        FWallTargetsInvalid = 1;
    }

    // CombatPredictor.pas:148
    public void ResetCombat()
    {
        FHitPoints = FInitialHitPoints;
        FCombatRounds = 0;
        FStatus = Set<CombatPredictorUnitStatusElement>.Of();
    }

    // CombatPredictor.pas:155
    public void ResetRoundTargets()
    {
        FRoundTargets.Clear();
        FCombatValueTotal = 0;
        FIncomingCombatValue = 0;
        FIneffectiveAttackers = 0;
    }

    // CombatPredictor.pas:163
    public void SetupRoundTargets(byte WallStanding)
    {
        int V;
        CombatPredictorUnit U;
        if (FEnabled != 0)
        {
            if (WallStanding != 0)
            {
                if (FWallTargetsInvalid != 0)
                {
                    UpdateWallTargets();
                }

                FRoundTargets.Assign(FWallTargets);
            }
            else
            {
                if (FTargetsInvalid != 0)
                {
                    UpdateTargets();
                }

                FRoundTargets.Assign(FTargets);
            }

            for (int I = FRoundTargets.Count - 1; I >= 0; I--)
            {
                // { Read the round list directly. }
                U = FRoundTargets.List[I].A;
                if (U.FEnabled != 0)
                {
                    V = FRoundTargets.List[I].B;
                    if (V > 0)
                    {
                        U.FIncomingCombatValue += V;
                        FCombatValueTotal += V;
                    }
                    else
                    {
                        U.FIneffectiveAttackers += 1;
                        FRoundTargets.Delete(I);
                    }
                }
                else
                {
                    FRoundTargets.Delete(I);
                }
            }
        }
    }

    // CombatPredictor.pas:201
    public void UpdateRoundTargetPriority()
    {
        if (FEnabled != 0)
        {
            FTargetPriority = 0;
            for (int I = FRoundTargets.Count - 1; I >= 0; I--)
            {
                FTargetPriority += FRoundTargets.List[I].A.FIneffectiveAttackers;
            }
        }
        else
        {
            FTargetPriority = 0;
        }
    }

    // CombatPredictor.pas:216
    public void SortRoundTargets()
    {
        int V = default;
        int Priority = default;
        CombatPredictorUnit U = default;
        for (int I = FRoundTargets.Count - 1; I >= 0; I--)
        {
            U = FRoundTargets.List[I].A;
            V = FRoundTargets.List[I].B;
            if (U.FCombatValueTotal != 0)
            {
                Priority = V * 100 * 1000 / U.FIncomingCombatValue + U.FTargetPriority * 100 + V;
            }
            else
            {
                Priority = 0;
            }

            FRoundTargets.List[I].D = Priority;
        }

        FRoundTargets.Sort(QuadItem<CombatPredictorUnit>.P3CompareRoundTargets);
    }

    // CombatPredictor.pas:231
    public bool ExecuteRound()
    {
        int I;
        CombatPredictorUnit U;
        I = FRoundTargets.Count;
        bool Result = false;
        if (I != 0)
        {
            FCombatRounds += 1;
        }

        while (I != 0 && !Result)
        {
            I -= 1;
            U = FRoundTargets.List[I].A;
            if (U.FHitPoints > 0)
            {
                U.FHitPoints -= FRoundTargets.List[I].B;
                Result = true;
            }
            else
            {
                FRoundTargets.Delete(I);
            }
        }

        return Result;
    }

    // CombatPredictor.pas:250
    public void FinishCombat()
    {
        if (FHitPoints <= 0)
        {
            FStatus = FStatus + Set<CombatPredictorUnitStatusElement>.Of(CombatPredictorUnitStatusElement.Destroyed);
        }
    }

    // intf_P3_01.inc:38

}
