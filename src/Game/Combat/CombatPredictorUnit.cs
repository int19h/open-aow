#nullable disable
// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
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
    public CombatPredictorUnit() => RoundTargets = new QuadItemList<CombatPredictorUnit>();

    public Troop Troop { get; private set; }
    public int CpuId { get; set; }

    private CombatPredictor _predictor;

    public int InitialHitPoints { get; private set; }
    public byte Side { get; private set; }

    private CombatPredictorSide _opponents;

    public byte Enabled { get; set; }
    public byte BehindWall { get; set; }
    public QuadItemList<CombatPredictorUnit> Targets { get; set; }
    public int CombatValue { get; private set; }
    public QuadItemList<CombatPredictorUnit> WallTargets { get; private set; }
    public int WallCombatValue { get; private set; }

    private byte _targetsInvalid;
    private byte _wallTargetsInvalid;

    public QuadItemList<CombatPredictorUnit> RoundTargets { get; }
    public int CombatValueTotal { get; private set; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Construction snapshots record this field, and the Delphi field is only written.")]
    private int _combatRounds;

    public int IncomingCombatValue { get; private set; }

    private int _ineffectiveAttackers;
    private int _targetPriority;

    public int HitPoints { get; set; }
    public Set<CombatPredictorUnitStatusElement> Status { get; private set; }

    // CombatPredictor.pas:4
    public virtual void Dispose()
    {
        object objectValue;
        Troop u;
        RoundTargets?.Dispose();
        objectValue = Targets;
        if (objectValue is not null)
        {
            (objectValue as IDisposable)?.Dispose();
            Targets = null;
        }

        objectValue = WallTargets;
        if (objectValue is not null)
        {
            (objectValue as IDisposable)?.Dispose();
            WallTargets = null;
        }

        u = Troop;
        if (u is not null)
        {
            _ = u.Release();
            Troop = null;
        }
    }

    // CombatPredictor.pas:36
    public void SetEnabled(byte value)
    {
        if (value != Enabled)
        {
            Enabled = value;
            _predictor.Invalid = 1;
        }
    }

    // CombatPredictor.pas:45
    public void UpdateTargets()
    {
        int j;
        int v;
        CombatPredictorUnit u;
        Targets ??= new QuadItemList<CombatPredictorUnit>();

        for (int i = Targets.Count - 1; i >= 0; i--)
        {
            j = _opponents.IndexOfCpuId(Targets.List[i].Third);
            if (j == -1)
            {
                Targets.Delete(i);
                // { Original reads the shifted entry after deletion. }
                CombatValue -= Targets.List[i].Second;
            }
        }

        if (Targets.Count != 0)
        {
            for (int i = _opponents.Count - 1; i >= 0; i--)
            {
                u = _opponents.GetUnits(i);
                j = Targets.IndexOfItemC(u.CpuId);
                if (j == -1)
                {
                    _predictor.Work += 1;
                    v = Troop.GetCombatValue(u.Troop, 0);
                    if (v < 0)
                    {
                        v = 0;
                    }

                    _ = Targets.AddABCD(u, v, u.CpuId, 0);
                    CombatValue += v;
                }
            }
        }
        else
        {
            CombatValue = 0;
            for (int i = _opponents.Count - 1; i >= 0; i--)
            {
                _predictor.Work += 1;
                u = _opponents.List[i];
                v = Troop.GetCombatValue(u.Troop, 0);
                if (v < 0)
                {
                    v = 0;
                }

                _ = Targets.AddABCD(u, v, u.CpuId, 0);
                CombatValue += v;
            }
        }

        _targetsInvalid = 0;
    }

    // CombatPredictor.pas:89
    public void UpdateWallTargets()
    {
        int j;
        int v;
        CombatPredictorUnit u;
        bool passing;
        byte mode;
        passing = Troop.GetWallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing);
        WallTargets ??= new QuadItemList<CombatPredictorUnit>();

        for (int i = WallTargets.Count - 1; i >= 0; i--)
        {
            j = _opponents.IndexOfCpuId(WallTargets.List[i].Third);
            if (j == -1)
            {
                WallTargets.Delete(i);
                // { Original reads the shifted entry after deletion. }
                WallCombatValue -= WallTargets.List[i].Second;
            }
        }

        if (WallTargets.Count != 0)
        {
            for (int i = _opponents.Count - 1; i >= 0; i--)
            {
                u = _opponents.List[i];
                j = WallTargets.IndexOfItemC(u.CpuId);
                if (j == -1)
                {
                    mode = !passing ? BehindWall != 0 ? u.BehindWall == 0 ? (byte)1 : (byte)0 : u.BehindWall != 0 ? (byte)2 : (byte)0 : (byte)0;

                    _predictor.Work += 1;
                    v = Troop.GetCombatValue(u.Troop, mode);
                    if (v < 0)
                    {
                        v = 0;
                    }

                    _ = WallTargets.AddABCD(u, v, 0, 0);
                    WallCombatValue += v;
                }
            }
        }
        else
        {
            WallCombatValue = 0;
            for (int i = _opponents.Count - 1; i >= 0; i--)
            {
                u = _opponents.List[i];
                mode = !passing ? BehindWall != 0 ? u.BehindWall == 0 ? (byte)1 : (byte)0 : u.BehindWall != 0 ? (byte)2 : (byte)0 : (byte)0;

                _predictor.Work += 1;
                v = Troop.GetCombatValue(u.Troop, mode);
                if (v < 0)
                {
                    v = 0;
                }

                // { Original initializes both C and D to zero in the wall cache. }
                _ = WallTargets.AddABCD(u, v, 0, 0);
                WallCombatValue += v;
            }
        }

        _wallTargetsInvalid = 0;
    }

    // CombatPredictor.pas:145
    public void InvalidateTargets()
    {
        _targetsInvalid = 1;
        _wallTargetsInvalid = 1;
    }

    // CombatPredictor.pas:148
    public void ResetCombat()
    {
        HitPoints = InitialHitPoints;
        _combatRounds = 0;
        Status = Set<CombatPredictorUnitStatusElement>.Of();
    }

    // CombatPredictor.pas:155
    public void ResetRoundTargets()
    {
        RoundTargets.Clear();
        CombatValueTotal = 0;
        IncomingCombatValue = 0;
        _ineffectiveAttackers = 0;
    }

    // CombatPredictor.pas:163
    public void SetupRoundTargets(byte wallStanding)
    {
        int v;
        CombatPredictorUnit u;
        if (Enabled != 0)
        {
            if (wallStanding != 0)
            {
                if (_wallTargetsInvalid != 0)
                {
                    UpdateWallTargets();
                }

                RoundTargets.Assign(WallTargets);
            }
            else
            {
                if (_targetsInvalid != 0)
                {
                    UpdateTargets();
                }

                RoundTargets.Assign(Targets);
            }

            for (int i = RoundTargets.Count - 1; i >= 0; i--)
            {
                // { Read the round list directly. }
                u = RoundTargets.List[i].First;
                if (u.Enabled != 0)
                {
                    v = RoundTargets.List[i].Second;
                    if (v > 0)
                    {
                        u.IncomingCombatValue += v;
                        CombatValueTotal += v;
                    }
                    else
                    {
                        u._ineffectiveAttackers += 1;
                        RoundTargets.Delete(i);
                    }
                }
                else
                {
                    RoundTargets.Delete(i);
                }
            }
        }
    }

    // CombatPredictor.pas:201
    public void UpdateRoundTargetPriority()
    {
        if (Enabled != 0)
        {
            _targetPriority = 0;
            for (int i = RoundTargets.Count - 1; i >= 0; i--)
            {
                _targetPriority += RoundTargets.List[i].First._ineffectiveAttackers;
            }
        }
        else
        {
            _targetPriority = 0;
        }
    }

    // CombatPredictor.pas:216
    public void SortRoundTargets()
    {
        int v = default;
        int priority = default;
        CombatPredictorUnit u = default;
        for (int i = RoundTargets.Count - 1; i >= 0; i--)
        {
            u = RoundTargets.List[i].First;
            v = RoundTargets.List[i].Second;
            priority = u.CombatValueTotal != 0 ? (v * 100 * 1000 / u.IncomingCombatValue) + (u._targetPriority * 100) + v : 0;

            RoundTargets.List[i].Fourth = priority;
        }

        RoundTargets.Sort(QuadItem<CombatPredictorUnit>.CompareRoundTargets);
    }

    // CombatPredictor.pas:231
    public bool ExecuteRound()
    {
        int i;
        CombatPredictorUnit u;
        i = RoundTargets.Count;
        bool result = false;
        if (i != 0)
        {
            _combatRounds += 1;
        }

        while (i != 0 && !result)
        {
            i -= 1;
            u = RoundTargets.List[i].First;
            if (u.HitPoints > 0)
            {
                u.HitPoints -= RoundTargets.List[i].Second;
                result = true;
            }
            else
            {
                RoundTargets.Delete(i);
            }
        }

        return result;
    }

    // CombatPredictor.pas:250
    public void FinishCombat()
    {
        if (HitPoints <= 0)
        {
            Status += Set<CombatPredictorUnitStatusElement>.Of(CombatPredictorUnitStatusElement.Destroyed);
        }
    }

    // intf_P3_01.inc:38

}
