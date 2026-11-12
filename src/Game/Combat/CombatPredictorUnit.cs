// Translated from AoWE.pas.
using Aow.Engine.Collections;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Combat;

// { Interface declaration run owned by P3. }
// Troop. CpuId. Predictor. InitialHitPoints. Side. Opponents. Enabled. BehindWall.
// Targets. CombatValue. WallTargets. WallCombatValue. cache flags. RoundTargets.
// CombatValueTotal. CombatRounds. IncomingCombatValue. IneffectiveAttackers.
// TargetPriority. HitPoints. Status
public partial class CombatPredictorUnit : IDisposable
{
    // CombatPredictor.pas:1
    public CombatPredictorUnit()
    {
        RoundTargets = new QuadItemList<CombatPredictorUnit>();
    }

    public Troop Troop;
    public int CpuId;
    public CombatPredictor Predictor;
    public int InitialHitPoints;
    public byte Side;
    public CombatPredictorSide Opponents;
    public byte Enabled;
    public byte BehindWall;
    public QuadItemList<CombatPredictorUnit> Targets;
    public int CombatValue;
    public QuadItemList<CombatPredictorUnit> WallTargets;
    public int WallCombatValue;
    public byte TargetsInvalid;
    public byte WallTargetsInvalid;
    public QuadItemList<CombatPredictorUnit> RoundTargets;
    public int CombatValueTotal;
    public int CombatRounds;
    public int IncomingCombatValue;
    public int IneffectiveAttackers;
    public int TargetPriority;
    public int HitPoints;
    public Set<CombatPredictorUnitStatusElement> Status;

    // CombatPredictor.pas:4
    public virtual void Dispose()
    {
        object objectValue;
        Troop U;
        RoundTargets?.Dispose();
        objectValue = Targets;
        if (objectValue != null)
        {
            (objectValue as IDisposable)?.Dispose();
            Targets = null;
        }

        objectValue = WallTargets;
        if (objectValue != null)
        {
            (objectValue as IDisposable)?.Dispose();
            WallTargets = null;
        }

        U = Troop;
        if (U != null)
        {
            U.Release();
            Troop = null;
        }
    }

    // CombatPredictor.pas:36
    public void SetEnabled(byte value)
    {
        if (value != Enabled)
        {
            Enabled = value;
            Predictor.Invalid = 1;
        }
    }

    // CombatPredictor.pas:45
    public void UpdateTargets()
    {
        int J;
        int V;
        CombatPredictorUnit U;
        if (Targets == null)
        {
            Targets = new QuadItemList<CombatPredictorUnit>();
        }

        for (int I = Targets.Count - 1; I >= 0; I--)
        {
            J = Opponents.IndexOfCpuId(Targets.List[I].Third);
            if (J == -1)
            {
                Targets.Delete(I);
                // { Original reads the shifted entry after deletion. }
                CombatValue -= Targets.List[I].Second;
            }
        }

        if (Targets.Count != 0)
        {
            for (int I = Opponents.Count - 1; I >= 0; I--)
            {
                U = Opponents.GetUnits(I);
                J = Targets.IndexOfItemC(U.CpuId);
                if (J == -1)
                {
                    Predictor.Work += 1;
                    V = Troop.GetCombatValue(U.Troop, 0);
                    if (V < 0)
                    {
                        V = 0;
                    }

                    Targets.AddABCD(U, V, U.CpuId, 0);
                    CombatValue += V;
                }
            }
        }
        else
        {
            CombatValue = 0;
            for (int I = Opponents.Count - 1; I >= 0; I--)
            {
                Predictor.Work += 1;
                U = Opponents.List[I];
                V = Troop.GetCombatValue(U.Troop, 0);
                if (V < 0)
                {
                    V = 0;
                }

                Targets.AddABCD(U, V, U.CpuId, 0);
                CombatValue += V;
            }
        }

        TargetsInvalid = 0;
    }

    // CombatPredictor.pas:89
    public void UpdateWallTargets()
    {
        int J;
        int V;
        CombatPredictorUnit U;
        bool passing;
        byte mode;
        passing = Troop.GetWallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing);
        if (WallTargets == null)
        {
            WallTargets = new QuadItemList<CombatPredictorUnit>();
        }

        for (int I = WallTargets.Count - 1; I >= 0; I--)
        {
            J = Opponents.IndexOfCpuId(WallTargets.List[I].Third);
            if (J == -1)
            {
                WallTargets.Delete(I);
                // { Original reads the shifted entry after deletion. }
                WallCombatValue -= WallTargets.List[I].Second;
            }
        }

        if (WallTargets.Count != 0)
        {
            for (int I = Opponents.Count - 1; I >= 0; I--)
            {
                U = Opponents.List[I];
                J = WallTargets.IndexOfItemC(U.CpuId);
                if (J == -1)
                {
                    if (!passing)
                    {
                        if (BehindWall != 0)
                        {
                            if (U.BehindWall == 0)
                            {
                                mode = 1;
                            }
                            else
                            {
                                mode = 0;
                            }
                        }
                        else
                        {
                            if (U.BehindWall != 0)
                            {
                                mode = 2;
                            }
                            else
                            {
                                mode = 0;
                            }
                        }
                    }
                    else
                    {
                        mode = 0;
                    }

                    Predictor.Work += 1;
                    V = Troop.GetCombatValue(U.Troop, mode);
                    if (V < 0)
                    {
                        V = 0;
                    }

                    WallTargets.AddABCD(U, V, 0, 0);
                    WallCombatValue += V;
                }
            }
        }
        else
        {
            WallCombatValue = 0;
            for (int I = Opponents.Count - 1; I >= 0; I--)
            {
                U = Opponents.List[I];
                if (!passing)
                {
                    if (BehindWall != 0)
                    {
                        if (U.BehindWall == 0)
                        {
                            mode = 1;
                        }
                        else
                        {
                            mode = 0;
                        }
                    }
                    else
                    {
                        if (U.BehindWall != 0)
                        {
                            mode = 2;
                        }
                        else
                        {
                            mode = 0;
                        }
                    }
                }
                else
                {
                    mode = 0;
                }

                Predictor.Work += 1;
                V = Troop.GetCombatValue(U.Troop, mode);
                if (V < 0)
                {
                    V = 0;
                }

                // { Original initializes both C and D to zero in the wall cache. }
                WallTargets.AddABCD(U, V, 0, 0);
                WallCombatValue += V;
            }
        }

        WallTargetsInvalid = 0;
    }

    // CombatPredictor.pas:145
    public void InvalidateTargets()
    {
        TargetsInvalid = 1;
        WallTargetsInvalid = 1;
    }

    // CombatPredictor.pas:148
    public void ResetCombat()
    {
        HitPoints = InitialHitPoints;
        CombatRounds = 0;
        Status = Set<CombatPredictorUnitStatusElement>.Of();
    }

    // CombatPredictor.pas:155
    public void ResetRoundTargets()
    {
        RoundTargets.Clear();
        CombatValueTotal = 0;
        IncomingCombatValue = 0;
        IneffectiveAttackers = 0;
    }

    // CombatPredictor.pas:163
    public void SetupRoundTargets(byte wallStanding)
    {
        int V;
        CombatPredictorUnit U;
        if (Enabled != 0)
        {
            if (wallStanding != 0)
            {
                if (WallTargetsInvalid != 0)
                {
                    UpdateWallTargets();
                }

                RoundTargets.Assign(WallTargets);
            }
            else
            {
                if (TargetsInvalid != 0)
                {
                    UpdateTargets();
                }

                RoundTargets.Assign(Targets);
            }

            for (int I = RoundTargets.Count - 1; I >= 0; I--)
            {
                // { Read the round list directly. }
                U = RoundTargets.List[I].First;
                if (U.Enabled != 0)
                {
                    V = RoundTargets.List[I].Second;
                    if (V > 0)
                    {
                        U.IncomingCombatValue += V;
                        CombatValueTotal += V;
                    }
                    else
                    {
                        U.IneffectiveAttackers += 1;
                        RoundTargets.Delete(I);
                    }
                }
                else
                {
                    RoundTargets.Delete(I);
                }
            }
        }
    }

    // CombatPredictor.pas:201
    public void UpdateRoundTargetPriority()
    {
        if (Enabled != 0)
        {
            TargetPriority = 0;
            for (int I = RoundTargets.Count - 1; I >= 0; I--)
            {
                TargetPriority += RoundTargets.List[I].First.IneffectiveAttackers;
            }
        }
        else
        {
            TargetPriority = 0;
        }
    }

    // CombatPredictor.pas:216
    public void SortRoundTargets()
    {
        int V = default;
        int priority = default;
        CombatPredictorUnit U = default;
        for (int I = RoundTargets.Count - 1; I >= 0; I--)
        {
            U = RoundTargets.List[I].First;
            V = RoundTargets.List[I].Second;
            if (U.CombatValueTotal != 0)
            {
                priority = V * 100 * 1000 / U.IncomingCombatValue + U.TargetPriority * 100 + V;
            }
            else
            {
                priority = 0;
            }

            RoundTargets.List[I].Fourth = priority;
        }

        RoundTargets.Sort(QuadItem<CombatPredictorUnit>.CompareRoundTargets);
    }

    // CombatPredictor.pas:231
    public bool ExecuteRound()
    {
        int I;
        CombatPredictorUnit U;
        I = RoundTargets.Count;
        bool result = false;
        if (I != 0)
        {
            CombatRounds += 1;
        }

        while (I != 0 && !result)
        {
            I -= 1;
            U = RoundTargets.List[I].First;
            if (U.HitPoints > 0)
            {
                U.HitPoints -= RoundTargets.List[I].Second;
                result = true;
            }
            else
            {
                RoundTargets.Delete(I);
            }
        }

        return result;
    }

    // CombatPredictor.pas:250
    public void FinishCombat()
    {
        if (HitPoints <= 0)
        {
            Status = Status + Set<CombatPredictorUnitStatusElement>.Of(CombatPredictorUnitStatusElement.Destroyed);
        }
    }

    // intf_P3_01.inc:38

}
