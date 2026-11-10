// Translated from CombatTE.pas.
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// { ot=1 }
public partial class TCEndTurnTE : TTokenEvent
{
    public TCEndTurnTE(TECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:175
    public new const int ClassID = 2236420;

    // CombatTE.pas:178
    public override TExecuteResult Execute(TTokenExecuter TE)
    {
        int TurnIndex;
        bool Found;
        base.Execute(TE);
        RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
        TurnIndex = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.FTurnOrder.IndexOf((byte)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer);
        Found = false;
        while (!Found)
        {
            TurnIndex = TurnIndex + 1;
            if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.FTurnOrder.Count <= TurnIndex)
            {
                TurnIndex = 0;
                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.NewRound();
            }

            for (int I = 0, last = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetCount() - 1; I <= last; I++)
            {
                bool condition = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I) != null && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I) is TCombatUnit;
                if (condition)
                {
                    var combatObject = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                    var turnOrder = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.FTurnOrder[TurnIndex];
                    condition = combatObject.FCombatPlayer == turnOrder;
                }

                if (condition && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I).GetEnabled())
                {
                    Found = true;
                }
            }
        }

        var turnOrder2 = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.FTurnOrder[TurnIndex];
        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.NewTurn((sbyte)turnOrder2);
        return TExecuteResult.erDone;
    }

    // NetworkE.pas:230

}

public partial class TCEndCombatTE : TTokenEvent
{
    public TCEndCombatTE(TECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:158
    public new const int ClassID = 2236421;

    // CombatTE.pas:161
    public override TExecuteResult Execute(TTokenExecuter TE)
    {
        base.Execute(TE);
        for (int I = 0, last = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetCount() - 1; I <= last; I++)
        {
            if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I) != null && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I) is TCombatUnit && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I).GetEnabled() && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I).FCombatPlayer == Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer)
            {
                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I).Surrender();
            }
        }

        return TExecuteResult.erDone;
    }

    // NetworkE.pas:230

}

public partial class TCombatMoveTE : TMoveTE
{
    public TCombatMoveTE(TECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:206
    public new const int ClassID = 2236416;

    public TTacticalCombatUnit FUnit;
    public int FDelay;
    public TList FObjects;
    public TTacticalCombatUnit FPreviousUnit;
    public TTacticalCombatUnit FFreeStrikeUnit;
    public sbyte FNeighbour;
    public TTokenStep FStep;
    public TStrikeCA FStrike;

    // CombatTE.pas:214
    public override void Dispose()
    {
        if (FObjects != null)
        {
            FObjects?.Dispose();
        }

        base.Dispose();
    }

    // CombatTE.pas:209
    public override TAoWMapField GetMapF(TXYL Location) => (TAoWMapField)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.GetField(Location.X, Location.Y, 0);

    // CombatTE.pas:234
    public override bool Validate()
    {
        bool Result;
        TAoWMapField MapField;
        int Index;
        TTacticalCombatUnitHS HS;
        base.Validate();
        Index = FPath.Count - 1;
        MapField = GetMapF(FPath[Index]);
        HS = (TTacticalCombatUnitHS)MapField.FindOwnedChild(2228484);
        if (HS != null)
        {
            FObjects = new TList();
            FUnit = HS.FUnit;
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // CombatTE.pas:299
    public override TMoveStatus DefaultMove()
    {
        TAoWMapField MapField;
        int Direction;
        int X = default;
        int Y = default;
        void CheckFreeStrike()
        {
            int HN = default;
            int NX = default;
            int NY = default;
            TTacticalCombatUnitHS HS = default;
            if (FUnit.GetEnabled() && FFreeStrikeUnit == null)
            {
                MapField = (TAoWMapField)FUnit.FUnitHS.Owner;
                HN = FNeighbour + 1;
                while (HN <= 6)
                {
                    var yhx = FUnit.FUnitHS.Yhx;
                    Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FUnit.FUnitHS.Xhx, yhx, HN, ref NX, ref NY);
                    HS = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(NX, NY).FindOwnedChild(2228484);
                    if (HS != null && FPreviousUnit != HS.FUnit && HS.FUnit.GetEnabled() && !HS.FUnit.GetLocked() && Aow.Game.Combat.TCombatObject.MeleeDamageTypes(HS.FUnit) != TSet<TDamageType>.Of() && HS.FUnit.GetSide() != FUnit.GetSide() && HS.FAction == TTCAction.caMove && HS.FUnit.GetAbilityEnabled(1) == FUnit.GetAbilityEnabled(1))
                    {
                        FNeighbour = (sbyte)HN;
                        FFreeStrikeUnit = HS.FUnit;
                        NX = 0;
                        int last = FObjects.Count - 1;
                        for (; NX <= last; NX += 1)
                        {
                            if (FObjects[NX] == FFreeStrikeUnit)
                            {
                                FFreeStrikeUnit = null;
                            }
                        }

                        if (FFreeStrikeUnit != null)
                        {
                            HN = 6;
                            FStep = TTokenStep.tokStart;
                        }
                    }

                    HN = HN + 1;
                }

                if (FFreeStrikeUnit == null)
                {
                    FNeighbour = 0;
                }
                else
                {
                    FObjects.Add(FFreeStrikeUnit);
                }
            }
        }

        TMoveStatus Result = TMoveStatus.msMoving;
        try
        {
            if (FFreeStrikeUnit != null)
            {
                Result = TMoveStatus.msMoving;
                switch (FStep)
                {
                    case TTokenStep.tokStart:
                        try
                        {
                            FUnit.FUnitHS.SetAction(TTCAction.caBusy);
                            FFreeStrikeUnit.FUnitHS.SetAction(TTCAction.caStepUp);
                            var yhx = FFreeStrikeUnit.FUnitHS.Yhx;
                            var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                            var xhx = FFreeStrikeUnit.FUnitHS.Xhx;
                            var fUnitHSXhx = FUnit.FUnitHS.Xhx;
                            var meleeDirIndex = Aow.Game.Combat.TacticalTables.GetMeleeDirIndex(FFreeStrikeUnit.FUnitHS.Xhx, xhx - fUnitHSXhx, yhx - fUnitHSYhx);
                            FFreeStrikeUnit.FUnitHS.SetDirection((byte)meleeDirIndex);
                            FStep = TTokenStep.tokInitStrike;
                        }
                        catch
                        {
                            MessageBox(0, "Error during Token Start", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TTokenStep.tokInitStrike:
                        try
                        {
                            if (FFreeStrikeUnit.FUnitHS.FActionDone)
                            {
                                RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                FStrike = Aow.Game.Combat.TCombatObject.CreateStrikeCA(FFreeStrikeUnit, FUnit, true);
                                if (FStrike.FDamage == 0)
                                {
                                    FFreeStrikeUnit.FUnitHS.SetAction(TTCAction.caMeleeMiss);
                                }
                                else
                                {
                                    FFreeStrikeUnit.FUnitHS.SetAction(TTCAction.caMeleeHit);
                                }

                                FUnit.FUnitHS.SetAction(TTCAction.caBusy);
                                FStep = TTokenStep.tokExeStrike;
                            }
                        }
                        catch
                        {
                            MessageBox(0, "Error during Token InitStrike", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TTokenStep.tokExeStrike:
                        try
                        {
                            if (FFreeStrikeUnit.FUnitHS.FActionDone)
                            {
                                if (FStrike.FDamage > 0)
                                {
                                    FUnit.FUnitHS.SetAction(TTCAction.caHit);
                                    Aow.Game.Combat.TTacticalCombatUnit.MakeHitBlood(FUnit, FStrike.FDamage);
                                }

                                var yhx = FUnit.FUnitHS.Yhx;
                                Aow.Game.World.Map.HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx, ref X, ref Y);
                                if (FStrike.FDamageTypes.Value - FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                {
                                    Aow.Game.Commands.CombatEvents.MakeDEHSList(X, Y, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                }

                                if (FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                {
                                    Aow.Game.Commands.CombatEvents.MakeDEHSList(X, Y, FStrike.FEffects.Value, true);
                                }

                                FStep = TTokenStep.tokEndStrike;
                            }
                        }
                        catch
                        {
                            MessageBox(0, "Error during Token ExeStrike", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TTokenStep.tokEndStrike:
                        try
                        {
                            if (FUnit.FUnitHS.FActionDone || FStrike.FDamage == 0)
                            {
                                if (FStrike.FDamage > 0)
                                {
                                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                }

                                FStep = TTokenStep.tokEnd;
                                FFreeStrikeUnit.FUnitHS.SetAction(TTCAction.caStepBack);
                            }
                        }
                        catch
                        {
                            MessageBox(0, "Error during Token EndStrike", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TTokenStep.tokEnd:
                        try
                        {
                            if (FFreeStrikeUnit.FUnitHS.FActionDone)
                            {
                                if (!FUnit.GetEnabled())
                                {
                                    Result = TMoveStatus.msDone;
                                }
                                else
                                {
                                    if (FUnit.GetLocked())
                                    {
                                        FUnit.FUnitHS.CancelMove();
                                        Result = TMoveStatus.msDone;
                                    }
                                    else
                                    {
                                        FUnit.FUnitHS.SetAction(TTCAction.caMove);
                                    }
                                }

                                if (FStrike != null)
                                {
                                    FStrike.Release();
                                    FStrike = null;
                                }

                                FFreeStrikeUnit.FUnitHS.SetAction(TTCAction.caMove);
                                FStep = TTokenStep.tokStart;
                                FFreeStrikeUnit = null;
                                CheckFreeStrike();
                            }
                        }
                        catch
                        {
                            MessageBox(0, "Error during Token End", "Error - TCMoveToken", 0);
                        }

                        break;
                }
            }
            else
            {
                if (FUnit.GetEnabled())
                {
                    if (FUnit.FUnitHS.Moving())
                    {
                        if (FUnit.FUnitHS.MoveNext() == 100 || !FUnit.FUnitHS.GetVisible())
                        {
                            Result = ExecuteDefaultMove();
                            if (Result == TMoveStatus.msMoving)
                            {
                                CheckFreeStrike();
                            }
                        }
                        else
                        {
                            Result = TMoveStatus.msMoving;
                        }
                    }
                    else
                    {
                        if (FMoveIndex != 0)
                        {
                            bool notExplorationEnabled = !Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FExplorationEnabled;
                            if (!notExplorationEnabled)
                            {
                                var yhx = FUnit.FUnitHS.Yhx;
                                var xhx = FUnit.FUnitHS.Xhx;
                                notExplorationEnabled = ((TAoWMapField)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.GetField(xhx, yhx, 0)).GetExplored(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedPlayer);
                            }

                            if (notExplorationEnabled)
                            {
                                var yhx = FUnit.FUnitHS.Yhx;
                                var xhx = FUnit.FUnitHS.Xhx;
                                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MakeVisible(xhx, yhx, 0, 3);
                            }

                            Direction = GetDirection(FMoveIndex);
                            MapField = GetMapF(FPath[FMoveIndex - 1]);
                            if (MapField.GetPlayer() == -1)
                            {
                                if (Direction != 0)
                                {
                                    if (FUnit.FUnitHS.MoveFirst(Direction, false) == -1)
                                    {
                                        Result = TMoveStatus.msFailed;
                                    }
                                    else
                                    {
                                        Result = TMoveStatus.msMoving;
                                    }

                                    if (Result == TMoveStatus.msMoving)
                                    {
                                        CheckFreeStrike();
                                    }
                                }
                                else
                                {
                                    Result = ExecuteDefaultMove();
                                }
                            }
                            else
                            {
                                Result = TMoveStatus.msDone;
                            }
                        }
                        else
                        {
                            Result = TMoveStatus.msDone;
                        }
                    }
                }
            }

            if (Result == TMoveStatus.msDone)
            {
                if (FUnit.GetEnabled() && FUnit.FCombatPlayer == Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer)
                {
                    var xhx = FUnit.FUnitHS.Xhx;
                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlayers.GetPlayers(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer).FLastX = xhx;
                    var yhx = FUnit.FUnitHS.Yhx;
                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlayers.GetPlayers(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer).FLastY = yhx;
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Token", "Error - TCMoveToken", 0);
        }

        return Result;
    }

    // CombatTE.pas:220
    public virtual bool Setup(TTacticalCombatUnit UnitObject, TMovepath Path)
    {
        bool Result;
        int StartIndex;
        FPath.Clear();
        StartIndex = Path.MovePointsToPos(UnitObject.FMovePoints);
        if (StartIndex != Path.Count - 1)
        {
            for (int I = StartIndex, last = Path.Count - 1; I <= last; I++)
            {
                var path = Path[I];
                FPath.Add(path);
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // CombatTE.pas:253
    public TMoveStatus ExecuteDefaultMove()
    {
        TMoveStatus Result;
        TAoWMapField NewField;
        TAoWMapField OldField;
        TRect R;
        Move();
        OldField = (TAoWMapField)FUnit.FUnitHS.Owner;
        NewField = GetMapF(FPath[FMoveIndex]);
        FUnit.FUnitHS.MoveTo(NewField, 0);
        if (NewField.FindChildIndex(FUnit.FUnitHS) != -1)
        {
            Result = ProcessMove();
            var path = FPath[FMoveIndex];
            FUnit.MovedTo(OldField, NewField, path.Flag);
            var rect = Rect(OldField.Xhx, OldField.Yhx, OldField.Xhx + 1, OldField.Yhx + 1);
            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.TriggerMapChangedEvent(rect, 0);
            if (FMoveIndex == 0 && NewField.FindChild(2229544) != null && FUnit.FCombatPlayer != Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.FDefenderPlayer)
            {
                FUnit.SetStatus(TSet<TCombatObjectStatusElement>.Of(TCombatObjectStatusElement.csFled));
                var yhx = FUnit.FUnitHS.Yhx;
                var xhx = FUnit.FUnitHS.Xhx;
                var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                R = Rect(FUnit.FUnitHS.Xhx, fUnitHSYhx, xhx + 1, yhx + 1);
                FUnit.FUnitHS?.Dispose();
                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.TriggerMapChangedEvent(R, 0);
                FUnit.FUnitHS = null;
                Result = TMoveStatus.msDone;
            }
            else
            {
                if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FExplorationEnabled)
                {
                    var player = FUnit.GetPlayer();
                    var visibilityRange = FUnit.GetVisibilityRange();
                    var yhx = FUnit.FUnitHS.Yhx;
                    var xhx = FUnit.FUnitHS.Xhx;
                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.ExploreArea(xhx, yhx, 0, player, visibilityRange);
                    var fUnitVisibilityRange = FUnit.GetVisibilityRange();
                    var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                    var fUnitHSXhx = FUnit.FUnitHS.Xhx;
                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapChangedRad(fUnitHSXhx, fUnitHSYhx, 0, fUnitVisibilityRange + 1);
                }
                else
                {
                    var yhx = FUnit.FUnitHS.Yhx;
                    var xhx = FUnit.FUnitHS.Xhx;
                    var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                    var rect2 = Rect(FUnit.FUnitHS.Xhx, fUnitHSYhx, xhx + 1, yhx + 1);
                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.TriggerMapChangedEvent(rect2, 0);
                }

                var fUnitPlayer = FUnit.GetPlayer();
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(fUnitPlayer).FPlayerType != TPlayerType.ptHuman || Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlayers.GetPlayers(FUnit.FCombatPlayer).FAuto)
                {
                    FUnit.FMovePoints = 0;
                }
            }
        }
        else
        {
            FUnit.FUnitHS = null;
            Result = TMoveStatus.msDone;
        }

        return Result;
    }

    // intf_P5_07.inc:765

}

public partial class TCMeleeMoveTE : TCombatMoveTE
{
    public TCMeleeMoveTE(TECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 2236417;

    public TMeleeRound FMeleeRound;
    public int FStrikeIndex;

    // CombatTE.pas:454
    public override TMoveStatus LastMove()
    {
        TTacticalCombatUnitHS HS;
        int X = default;
        int Y = default;
        TMeleeStrike Strike = default;
        TMoveStatus Result = TMoveStatus.msMoving;
        try
        {
            switch (FStep)
            {
                case TTokenStep.tokStart:
                    try
                    {
                        Result = TMoveStatus.msMoving;
                        FUnit.FUnitHS.CancelMove();
                        bool notExplorationEnabled = !Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FExplorationEnabled;
                        if (!notExplorationEnabled)
                        {
                            var yhx = FUnit.FUnitHS.Yhx;
                            var xhx = FUnit.FUnitHS.Xhx;
                            notExplorationEnabled = ((TAoWMapField)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.GetField(xhx, yhx, 0)).GetExplored(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedPlayer);
                        }

                        if (notExplorationEnabled)
                        {
                            var yhx = FUnit.FUnitHS.Yhx;
                            var xhx = FUnit.FUnitHS.Xhx;
                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MakeVisible(xhx, yhx, 0, 5);
                        }

                        var pathY = FPath[0];
                        var pathX = FPath[0];
                        HS = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(pathX.X, pathY.Y).FindOwnedChild(2228484);
                        if (HS == null)
                        {
                            FPreviousUnit = null;
                        }
                        else
                        {
                            FPreviousUnit = HS.FUnit;
                        }

                        if (FPreviousUnit == null)
                        {
                            Result = TMoveStatus.msDone;
                        }
                        else
                        {
                            FMeleeRound = new TMeleeRound();
                            FMeleeRound.Calculate(FUnit, FPreviousUnit);
                            if (FMeleeRound.FCount == 0)
                            {
                                Result = TMoveStatus.msDone;
                            }
                            else
                            {
                                FUnit.FUnitHS.SetAction(TTCAction.caStepUp);
                                var yhx = FUnit.FUnitHS.Yhx;
                                var fUnitHSYhx = FPreviousUnit.FUnitHS.Yhx;
                                var xhx = FUnit.FUnitHS.Xhx;
                                var fUnitHSXhx = FPreviousUnit.FUnitHS.Xhx;
                                var meleeDirIndex = Aow.Game.Combat.TacticalTables.GetMeleeDirIndex(FUnit.FUnitHS.Xhx, xhx - fUnitHSXhx, yhx - fUnitHSYhx);
                                FUnit.FUnitHS.SetDirection((byte)meleeDirIndex);
                                FPreviousUnit.FUnitHS.SetDirection((byte)(FUnit.FUnitHS.FDirection + 3));
                                if (FPreviousUnit.FUnitHS.FDirection > 6)
                                {
                                    FPreviousUnit.FUnitHS.SetDirection((byte)(FPreviousUnit.FUnitHS.FDirection - 6));
                                }
                            }
                        }

                        FStep = TTokenStep.tokFirstStrike;
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token Start", "Error - TCMelMoveToken", 0);
                    }

                    break;
                case TTokenStep.tokFirstStrike:
                    if (FUnit.FUnitHS.FActionDone)
                    {
                        ExecuteStrike();
                    }

                    break;
                case TTokenStep.tokDelayStrike:
                    if (FDelay > 0)
                    {
                        FDelay = FDelay - 1;
                    }
                    else
                    {
                        ExecuteStrike();
                    }

                    break;
                case TTokenStep.tokInitStrike:
                    try
                    {
                        FMeleeRound.GetMeleeStrike(FStrikeIndex - 1, ref Strike);
                        if (Strike.Side == TCombatSideIndex.csDefender && FPreviousUnit.FUnitHS.FActionDone)
                        {
                            if (FStrike.FDamage > 0)
                            {
                                FUnit.FUnitHS.SetAction(TTCAction.caHit);
                                Aow.Game.Combat.TTacticalCombatUnit.MakeHitBlood(FUnit, FStrike.FDamage);
                                var yhx = FUnit.FUnitHS.Yhx;
                                Aow.Game.World.Map.HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx, ref X, ref Y);
                                if (FStrike.FDamageTypes.Value - FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                {
                                    Aow.Game.Commands.CombatEvents.MakeDEHSList(X + FUnit.FUnitHS.FOffsetX, Y + FUnit.FUnitHS.FOffsetY, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                }

                                if (FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                {
                                    Aow.Game.Commands.CombatEvents.MakeDEHSList(X + FUnit.FUnitHS.FOffsetX, Y + FUnit.FUnitHS.FOffsetY, FStrike.FEffects.Value, true);
                                }
                            }

                            FStep = TTokenStep.tokExeStrike;
                        }
                        else
                        {
                            FMeleeRound.GetMeleeStrike(FStrikeIndex - 1, ref Strike);
                            if (Strike.Side == TCombatSideIndex.csAttacker && FUnit.FUnitHS.FActionDone)
                            {
                                if (FStrike.FDamage > 0)
                                {
                                    FPreviousUnit.FUnitHS.SetAction(TTCAction.caHit);
                                    Aow.Game.Combat.TTacticalCombatUnit.MakeHitBlood(FPreviousUnit, FStrike.FDamage);
                                    var yhx = FPreviousUnit.FUnitHS.Yhx;
                                    Aow.Game.World.Map.HexGeometry.HXtoHP(FPreviousUnit.FUnitHS.Xhx, yhx, ref X, ref Y);
                                    if (FStrike.FDamageTypes.Value - FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                    {
                                        Aow.Game.Commands.CombatEvents.MakeDEHSList(X, Y, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                    }

                                    if (FStrike.FEffects.Value != TSet<TDamageType>.Of())
                                    {
                                        Aow.Game.Commands.CombatEvents.MakeDEHSList(X, Y, FStrike.FEffects.Value, true);
                                    }
                                }

                                FStep = TTokenStep.tokExeStrike;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token InitStrike", "Error - TCMelMoveToken", 0);
                    }

                    break;
                case TTokenStep.tokExeStrike:
                    try
                    {
                        if (FStrike.FDamage == 0)
                        {
                            NextStrike();
                        }
                        else
                        {
                            FMeleeRound.GetMeleeStrike(FStrikeIndex - 1, ref Strike);
                            if (Strike.Side == TCombatSideIndex.csAttacker && FPreviousUnit.FUnitHS.FActionDone)
                            {
                                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                if (!FPreviousUnit.GetEnabled())
                                {
                                    FStep = TTokenStep.tokEnd;
                                    FUnit.FUnitHS.SetAction(TTCAction.caStepBack);
                                }
                                else
                                {
                                    NextStrike();
                                }
                            }
                            else
                            {
                                if (FUnit.FUnitHS.FActionDone)
                                {
                                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                    if (FUnit.GetEnabled())
                                    {
                                        NextStrike();
                                    }
                                    else
                                    {
                                        Result = TMoveStatus.msDone;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token ExeStrike", "Error - TCMelMoveToken", 0);
                    }

                    break;
                case TTokenStep.tokEnd:
                    if (FUnit.FUnitHS.FActionDone)
                    {
                        Result = TMoveStatus.msDone;
                    }
                    else
                    {
                        Result = TMoveStatus.msMoving;
                    }

                    break;
            }

            if (Result == TMoveStatus.msDone)
            {
                if (FMeleeRound != null)
                {
                    FMeleeRound?.Dispose();
                }

                if (FStrike != null)
                {
                    FStrike.Release();
                }

                if (FUnit.GetEnabled())
                {
                    FUnit.FMovePoints = 0;
                    FUnit.FUnitHS.Unselect();
                    if (FUnit.FCombatPlayer == Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer)
                    {
                        var xhx = FUnit.FUnitHS.Xhx;
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlayers.GetPlayers(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer).FLastX = xhx;
                        var yhx = FUnit.FUnitHS.Yhx;
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlayers.GetPlayers(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer).FLastY = yhx;
                    }
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Token", "Error - TCMelMoveToken", 0);
        }

        return Result;
    }

    // CombatTE.pas:577
    public void NextStrike()
    {
        bool Locked;
        TMeleeStrike Strike = default;
        Locked = true;
        while (Locked && FStrikeIndex < FMeleeRound.FCount)
        {
            FMeleeRound.GetMeleeStrike(FStrikeIndex, ref Strike);
            if (Strike.Side == TCombatSideIndex.csAttacker)
            {
                Locked = FUnit.GetLocked();
            }
            else
            {
                Locked = FPreviousUnit.GetLocked();
            }

            if (Locked)
            {
                FStrikeIndex = FStrikeIndex + 1;
            }
        }

        if (FStrikeIndex < FMeleeRound.FCount)
        {
            FStep = TTokenStep.tokDelayStrike;
            FDelay = 3;
        }
        else
        {
            FStep = TTokenStep.tokEnd;
            FUnit.FUnitHS.SetAction(TTCAction.caStepBack);
        }
    }

    // CombatTE.pas:602
    public void ExecuteStrike()
    {
        TMeleeStrike Strike = default;
        if (FStrike != null)
        {
            FStrike.Release();
        }

        RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
        FStrike = FMeleeRound.CreateStrikeCA(FStrikeIndex);
        FMeleeRound.GetMeleeStrike(FStrikeIndex, ref Strike);
        if (Strike.Side == TCombatSideIndex.csAttacker)
        {
            if (FStrike.FDamage == 0)
            {
                FUnit.FUnitHS.SetAction(TTCAction.caMeleeMiss);
            }
            else
            {
                FUnit.FUnitHS.SetAction(TTCAction.caMeleeHit);
            }
        }
        else
        {
            if (FStrike.FDamage == 0)
            {
                FPreviousUnit.FUnitHS.SetAction(TTCAction.caMeleeMiss);
            }
            else
            {
                FPreviousUnit.FUnitHS.SetAction(TTCAction.caMeleeHit);
            }
        }

        FStrikeIndex = FStrikeIndex + 1;
        FStep = TTokenStep.tokInitStrike;
    }

    // intf_P5_07.inc:765

}

public partial class TCAbTouchMoveTE : TCombatMoveTE
{
    public TCAbTouchMoveTE(TECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:624
    public new const int ClassID = 2236418;

    public TfxHS FEffect;
    public int FAbilityID;
    public TPossessCA FPossessAction;
    public TEntangleCA FEntangleAction;
    public TWebCA FWebAction;
    public TTurnUndeadCA FTurnUndeadAction;
    public THealingCA FHealingAction;
    public TCommandCA FCommandAction;
    public TInvokeDeathCA FDeathAction;
    public TDispelMagicAbilityCA FDispelAction;
    public TfxType FEffectType;
    public bool FSuccessful;
    public int FDamage;
    public TCityWall FWall;

    // CombatTE.pas:627
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FAbilityID, 30);
    }

    // CombatTE.pas:633
    public override TMoveStatus LastMove()
    {
        TXYZ Point;
        int I = default;
        int X = default;
        int Y = default;
        TTacticalCombatUnitHS HS;
        TMapFieldMsg Msg = default;
        TMoveStatus Result = TMoveStatus.msMoving;
        try
        {
            switch (FStep)
            {
                case TTokenStep.tokStart:
                    try
                    {
                        FEffectType = Aow.Game.Combat.TacticalTables.AbilTypes[FAbilityID];
                        FUnit.FUnitHS.CancelMove();
                        FUnit.FUnitHS.Unselect();
                        FUnit.FUnitHS.FSelectedAbility = FAbilityID;
                        bool notExplorationEnabled = !Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FExplorationEnabled;
                        if (!notExplorationEnabled)
                        {
                            var yhx = FUnit.FUnitHS.Yhx;
                            var xhx = FUnit.FUnitHS.Xhx;
                            notExplorationEnabled = ((TAoWMapField)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.GetField(xhx, yhx, 0)).GetExplored(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedPlayer);
                        }

                        if (notExplorationEnabled)
                        {
                            var yhx = FUnit.FUnitHS.Yhx;
                            var xhx = FUnit.FUnitHS.Xhx;
                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MakeVisible(xhx, yhx, 0, 5);
                        }

                        var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                        var pathY = FPath[0];
                        var fUnitHSXhx = FUnit.FUnitHS.Xhx;
                        var pathX = FPath[0];
                        var meleeDirIndex = Aow.Game.Combat.TacticalTables.GetMeleeDirIndex(FUnit.FUnitHS.Xhx, fUnitHSXhx - pathX.X, fUnitHSYhx - pathY.Y);
                        FUnit.FUnitHS.SetDirection((byte)meleeDirIndex);
                        if (FAbilityID == 65)
                        {
                            FStep = TTokenStep.tokInitStrike;
                        }
                        else
                        {
                            if (FAbilityID == 114)
                            {
                                FStep = TTokenStep.tokExeStrike;
                                FUnit.FUnitHS.SetAction(TTCAction.caRoundAttack);
                            }
                            else
                            {
                                FUnit.FUnitHS.SetAction(TTCAction.caStepUp);
                                var path = FPath[0];
                                var path2 = FPath[0];
                                HS = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(path2.X, path.Y).FindOwnedChild(2228484);
                                if (HS == null)
                                {
                                    FPreviousUnit = null;
                                }
                                else
                                {
                                    FPreviousUnit = HS.FUnit;
                                }

                                if (FPreviousUnit != null && FAbilityID != 117)
                                {
                                    FPreviousUnit.FUnitHS.SetDirection((byte)(FUnit.FUnitHS.FDirection + 3));
                                    if (FPreviousUnit.FUnitHS.FDirection > 6)
                                    {
                                        FPreviousUnit.FUnitHS.SetDirection((byte)(FPreviousUnit.FUnitHS.FDirection - 6));
                                    }
                                }
                                else
                                {
                                    if (FAbilityID == 117)
                                    {
                                        var path3 = FPath[0];
                                        var path4 = FPath[0];
                                        FWall = (TCityWall)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(path4.X, path3.Y).FindOwnedChild(2229508);
                                        if (FWall == null)
                                        {
                                            var path5 = FPath[0];
                                            var path6 = FPath[0];
                                            FWall = (TCityWall)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(path6.X, path5.Y).FindOwnedChild(2229510);
                                        }

                                        if (FWall == null && FAbilityID == 117)
                                        {
                                            Result = TMoveStatus.msDone;
                                        }
                                    }
                                    else
                                    {
                                        Result = TMoveStatus.msDone;
                                    }
                                }

                                FStep = TTokenStep.tokStepUp;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token Start", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                    }

                    break;
                case TTokenStep.tokStepUp:
                    try
                    {
                        if (FUnit.FUnitHS.FActionDone)
                        {
                            FUnit.FUnitHS.SetAction(TTCAction.caAbility);
                            FStep = TTokenStep.tokInitStrike;
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token StepUp", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                    }

                    break;
                case TTokenStep.tokInitStrike:
                    try
                    {
                        if (FPreviousUnit != null)
                        {
                            if (FUnit.FUnitHS.FActionDone)
                            {
                                FEffect = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.AddEffect(FEffectType);
                                FEffect.FAbilityIndex = FAbilityID;
                                Point = new TXYZ();
                                var yhx = FPreviousUnit.FUnitHS.Yhx;
                                Aow.Game.World.Map.HexGeometry.HXtoHP(FPreviousUnit.FUnitHS.Xhx, yhx, ref Point.X, ref Point.Y);
                                Point.X += 16;
                                FEffect.FPath.Add(Point);
                                FStep = TTokenStep.tokExeStrike;
                            }
                        }
                        else
                        {
                            if (FWall != null || FAbilityID == 65)
                            {
                                switch (FAbilityID)
                                {
                                    case 117:
                                        FStep = TTokenStep.tokEnd;
                                        FDamage = 0;
                                        RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                        var defense = FWall.GetDefense();
                                        var executeDamageRole = Aow.Game.Magic.Abilities.AbilityRules.ExecuteDamageRole(Aow.Game.Combat.TacticalTables.TCDamage[0], Aow.Game.Combat.TacticalTables.TCAttack[0] - defense);
                                        FWall.SetHitPoints(FWall.FHitPoints - executeDamageRole);
                                        FUnit.FUnitHS.SetAction(TTCAction.caStepBack);
                                        break;
                                    case 65:
                                        FEffect = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.AddEffect(FEffectType);
                                        FEffect.FAbilityIndex = FAbilityID;
                                        Point = new TXYZ();
                                        var path = FPath[1];
                                        Aow.Game.World.Map.HexGeometry.HXtoHP(FPath[1].X, path.Y, ref Point.X, ref Point.Y);
                                        Point.X += 16;
                                        FEffect.FPath.Add(Point);
                                        FStep = TTokenStep.tokExeStrike;
                                        break;
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token InitStrike", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                    }

                    break;
                case TTokenStep.tokExeStrike:
                    try
                    {
                        if (FEffect == null)
                        {
                            if (!FUnit.GetEnabled())
                            {
                                Result = TMoveStatus.msDone;
                            }
                            else
                            {
                                switch (FAbilityID)
                                {
                                    case 114:
                                        if (FUnit.FUnitHS.FActionDone || FUnit.FUnitHS.FActionFrame >= 17)
                                        {
                                            Result = TMoveStatus.msDone;
                                        }
                                        else
                                        {
                                            if (Aow.Game.Combat.TacticalTables.raDir[FUnit.FUnitHS.FActionFrame] > 0)
                                            {
                                                I = (Aow.Game.Combat.TacticalTables.raDir[FUnit.FUnitHS.FActionFrame] + FUnit.FUnitHS.FDirection) % 6 + 1;
                                                var path = FPath[1];
                                                Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FPath[1].X, path.Y, I, ref X, ref Y);
                                                HS = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(X, Y).FindOwnedChild(2228484);
                                                if (HS == null || HS == FUnit.FUnitHS)
                                                {
                                                    FPreviousUnit = null;
                                                }
                                                else
                                                {
                                                    if (HS.FUnit.GetAbilityEnabled(1))
                                                    {
                                                        FPreviousUnit = null;
                                                    }
                                                    else
                                                    {
                                                        RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                                        FStrike = Aow.Game.Combat.TCombatObject.CreateStrikeCA(FUnit, HS.FUnit, true);
                                                        if (FStrike.FDamage > 0)
                                                        {
                                                            HS.SetAction(TTCAction.caHit);
                                                            FUnit.FUnitHS.SetAction(TTCAction.caRoundAttackHit);
                                                            Aow.Game.Combat.TTacticalCombatUnit.MakeHitBlood(HS.FUnit, FStrike.FDamage);
                                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                                                        }
                                                        else
                                                        {
                                                            FUnit.FUnitHS.SetAction(TTCAction.caRoundAttack);
                                                        }

                                                        FStrike.Release();
                                                        FStrike = null;
                                                    }
                                                }
                                            }
                                        }

                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (FEffect.FDone)
                            {
                                if (!FUnit.GetEnabled())
                                {
                                    Result = TMoveStatus.msDone;
                                }
                                else
                                {
                                    switch (FAbilityID)
                                    {
                                        case 40:
                                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                            FEntangleAction = ((TEntangleAbility)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(FAbilityID)).CreateEntangleCA(FUnit, FPreviousUnit);
                                            FSuccessful = FEntangleAction.FSuccessful;
                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(FEntangleAction);
                                            FEntangleAction.Release();
                                            break;
                                        case 27:
                                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                            FWebAction = ((TWebAbility)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(FAbilityID)).CreateWebCA(FUnit, FPreviousUnit);
                                            FSuccessful = FWebAction.FSuccessful;
                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(FWebAction);
                                            FWebAction.Release();
                                            break;
                                        case 28:
                                        case 29:
                                        case 148:
                                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                            FCommandAction = ((TCommandAbility)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(FAbilityID)).CreateCommandCA(FUnit, FPreviousUnit);
                                            FSuccessful = FCommandAction.FSuccessful;
                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(FCommandAction);
                                            FCommandAction.Release();
                                            break;
                                        case 38:
                                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                            FTurnUndeadAction = ((TTurnUndeadAbility)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(FAbilityID)).CreateTurnUndeadCA(FUnit, FPreviousUnit);
                                            FSuccessful = FTurnUndeadAction.GetSuccessfull();
                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(FTurnUndeadAction);
                                            FTurnUndeadAction.Release();
                                            break;
                                        case 35:
                                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                            FDeathAction = ((TInvokeDeathAbility)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(FAbilityID)).CreateCA(FUnit, FPreviousUnit);
                                            FSuccessful = FDeathAction.FSuccessful;
                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(FDeathAction);
                                            FDeathAction.Release();
                                            break;
                                        case 47:
                                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                            FHealingAction = ((THealingAbility)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(FAbilityID)).CreateHealingCA(FUnit, FPreviousUnit);
                                            FSuccessful = true;
                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(FHealingAction);
                                            FHealingAction.Release();
                                            break;
                                        case 60:
                                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                            FDispelAction = ((TDispelMagicAbility)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(FAbilityID)).CreateCA(FUnit, FPreviousUnit);
                                            FSuccessful = FDispelAction.FEnchantmentIDs.Count > 0;
                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(FDispelAction);
                                            FDispelAction.Release();
                                            break;
                                        case 106:
                                        case 107:
                                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                            FPossessAction = ((TPossessAbility)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(FAbilityID)).CreatePossessCA(FUnit, FPreviousUnit);
                                            FSuccessful = FPossessAction.FSuccessful;
                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(FPossessAction);
                                            FPossessAction.Release();
                                            var unitGFXResourceIndex = FUnit.FUnit.GetUnitGFXResourceIndex();
                                            FUnit.FUnitHS.SetUnitGFXResourceIndex(unitGFXResourceIndex);
                                            if (!FPreviousUnit.GetEnabled())
                                            {
                                                FUnit.FUnitHS.MoveTo((TMapField)FPreviousUnit.FUnitHS.Owner, 0);
                                                FUnit.FUnitHS.SetDirection(FPreviousUnit.FUnitHS.FDirection);
                                                FUnit.FUnitHS.FOffsetX = 0;
                                                FUnit.FUnitHS.FOffsetY = 0;
                                                FPreviousUnit.FUnitHS?.Dispose();
                                                FPreviousUnit.FUnitHS = null;
                                                Result = TMoveStatus.msDone;
                                            }

                                            break;
                                    }
                                }

                                if (!FUnit.GetEnabled() || ((FAbilityID == 106 || FAbilityID == 107) && FSuccessful))
                                {
                                    Result = TMoveStatus.msDone;
                                }
                                else
                                {
                                    if (!FSuccessful && FPreviousUnit != null && FPreviousUnit.GetEnabled() && !FPreviousUnit.GetLocked() && Aow.Game.Combat.TCombatObject.MeleeDamageTypes(FPreviousUnit) != TSet<TDamageType>.Of() && FPreviousUnit.GetSide() != FUnit.GetSide())
                                    {
                                        RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                        FStrike = Aow.Game.Combat.TCombatObject.CreateStrikeCA(FPreviousUnit, FUnit, true);
                                        if (FStrike.FDamage == 0)
                                        {
                                            FPreviousUnit.FUnitHS.SetAction(TTCAction.caMeleeMiss);
                                        }
                                        else
                                        {
                                            FPreviousUnit.FUnitHS.SetAction(TTCAction.caMeleeHit);
                                        }

                                        FUnit.FUnitHS.SetAction(TTCAction.caBusy);
                                        FStep = TTokenStep.tokEndStrike;
                                    }
                                    else
                                    {
                                        FStep = TTokenStep.tokEnd;
                                        FUnit.FUnitHS.SetAction(TTCAction.caStepBack);
                                    }
                                }
                            }
                            else
                            {
                                if (FUnit.GetEnabled())
                                {
                                    switch (FAbilityID)
                                    {
                                        case 65:
                                            if (FEffect.FFrame == 5)
                                            {
                                                Msg.MsgID = 200704;
                                                Msg.Param[0] = FUnit;
                                                Msg.Result = -1;
                                                I = 0;
                                                for (; I <= 6; I += 1)
                                                {
                                                    var path = FPath[1];
                                                    Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FPath[1].X, path.Y, I, ref X, ref Y);
                                                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                    Aow.Game.Draw.EffectPaths.CreateFire(false, 50, X, Y);
                                                }

                                                FUnit.FDeathMethod = TTCDeathMethod.dmExplode;
                                            }

                                            break;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token ExeStrike", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                    }

                    break;
                case TTokenStep.tokEndStrike:
                    try
                    {
                        if (FPreviousUnit.FUnitHS.FActionDone)
                        {
                            if (FStrike.FDamage > 0)
                            {
                                FUnit.FUnitHS.SetAction(TTCAction.caHit);
                                Aow.Game.Combat.TTacticalCombatUnit.MakeHitBlood(FUnit, FStrike.FDamage);
                            }

                            var yhx = FUnit.FUnitHS.Yhx;
                            Aow.Game.World.Map.HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx, ref I, ref X);
                            if (FStrike.FDamageTypes.Value - FStrike.FEffects.Value != TSet<TDamageType>.Of())
                            {
                                Aow.Game.Commands.CombatEvents.MakeDEHSList(I + FUnit.FUnitHS.FOffsetX, X + FUnit.FUnitHS.FOffsetY, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                            }

                            if (FStrike.FEffects.Value != TSet<TDamageType>.Of())
                            {
                                Aow.Game.Commands.CombatEvents.MakeDEHSList(I + FUnit.FUnitHS.FOffsetX, X + FUnit.FUnitHS.FOffsetY, FStrike.FEffects.Value, true);
                            }

                            FStep = TTokenStep.tokExeEndStrike;
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token StrikeBack", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                    }

                    break;
                case TTokenStep.tokExeEndStrike:
                    try
                    {
                        if (FUnit.FUnitHS.FActionDone || FStrike.FDamage == 0)
                        {
                            if (FStrike.FDamage > 0)
                            {
                                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(FStrike);
                            }

                            if (FUnit.GetEnabled())
                            {
                                FStep = TTokenStep.tokEnd;
                                FUnit.FUnitHS.SetAction(TTCAction.caStepBack);
                            }
                            else
                            {
                                Result = TMoveStatus.msDone;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token ExeStrikeBack", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
                    }

                    break;
                case TTokenStep.tokEnd:
                    if (FUnit.FUnitHS.FActionDone)
                    {
                        Result = TMoveStatus.msDone;
                    }

                    break;
            }

            if (Result == TMoveStatus.msDone)
            {
                if (FUnit.GetEnabled())
                {
                    FUnit.FUnitHS.SetAction(TTCAction.caMove);
                    FUnit.FUnitHS.FSelectedAbility = -1;
                    FUnit.FMovePoints = 0;
                    FUnit.FUnitHS.Unselect();
                    if (FUnit.FCombatPlayer == Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer)
                    {
                        var xhx = FUnit.FUnitHS.Xhx;
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlayers.GetPlayers(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer).FLastX = xhx;
                        var yhx = FUnit.FUnitHS.Yhx;
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlayers.GetPlayers(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer).FLastY = yhx;
                    }
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Token", "Error - TCTouchToken " + IntToStr(FAbilityID), 0);
        }

        return Result;
    }

    // intf_P5_07.inc:765

}

public partial class TCAbRangedTE : TTokenEvent
{
    public TCAbRangedTE(TECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:917
    public new const int ClassID = 2236419;

    public TTacticalCombatUnit FUnit;
    public int FAbilityID;
    public int FSpellID;
    public ushort FSourceX;
    public ushort FSourceY;
    public ushort FTargetX;
    public ushort FTargetY;
    public TfxType FEffectType;
    public int FStrikeCount;
    public object FSound;
    public int FDelay;
    public int FStrikeIndex;
    public int FState;

    // CombatTE.pas:920
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FAbilityID, 30);
        Stream.rwInteger(ref FSpellID, 31);
        Stream.rwWord(ref FTargetX, 32);
        Stream.rwWord(ref FTargetY, 33);
        Stream.rwWord(ref FSourceX, 34);
        Stream.rwWord(ref FSourceY, 35);
    }

    // CombatTE.pas:931
    public override TExecuteResult Execute(TTokenExecuter TE)
    {
        int I = default;
        int X = default;
        int Y = default;
        int EffectIndex = default;
        TfxHS Effect = default;
        TfxHS ExtraEffect = default;
        int EndX = default;
        int EndY = default;
        TSpellCombatInfo Info = default;
        TTacticalCombatUnitHS HS;
        TCOozeHS Ooze = default;
        TCombatAction Action = default;
        TMapFieldMsg Msg = default;
        base.Execute(TE);
        TExecuteResult Result = TExecuteResult.erRetry;
        try
        {
            switch (FState)
            {
                case 0:
                    try
                    {
                        HS = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(FSourceX, FSourceY).FindOwnedChild(2228484);
                        if (HS == null)
                        {
                            Result = TExecuteResult.erDone;
                        }
                        else
                        {
                            FUnit = HS.FUnit;
                            FUnit.FUnitHS.Unselect();
                            FUnit.FUnitHS.FSelectedAbility = FAbilityID;
                            FUnit.FUnitHS.FSelectedSpell = FSpellID;
                            if (FAbilityID == 52)
                            {
                                FEffectType = Aow.Game.Combat.TacticalTables.SpellTypes[FSpellID];
                            }
                            else
                            {
                                FEffectType = Aow.Game.Combat.TacticalTables.AbilTypes[FAbilityID];
                            }

                            if (FAbilityID == 52)
                            {
                                Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FSpellID).CombatSpellCast(FUnit);
                            }

                            bool notExplorationEnabled = !Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FExplorationEnabled;
                            if (!notExplorationEnabled)
                            {
                                var yhx = FUnit.FUnitHS.Yhx;
                                var xhx = FUnit.FUnitHS.Xhx;
                                notExplorationEnabled = ((TAoWMapField)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.GetField(xhx, yhx, 0)).GetExplored(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedPlayer);
                            }

                            if (notExplorationEnabled)
                            {
                                var yhx = FUnit.FUnitHS.Yhx;
                                var xhx = FUnit.FUnitHS.Xhx;
                                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MakeVisible(xhx, yhx, 0, 2);
                            }

                            switch (FEffectType)
                            {
                                case TfxType.fxBreath:
                                    FStrikeCount = 20;
                                    break;
                                case TfxType.fxScreenNESW:
                                    FStrikeCount = 10;
                                    break;
                                case TfxType.fxScreenNS:
                                    switch (FSpellID)
                                    {
                                        case 116:
                                            FStrikeCount = 21;
                                            break;
                                        case 124:
                                            FStrikeCount = 19;
                                            break;
                                        default:
                                            FStrikeCount = 35;
                                            break;
                                    }

                                    break;
                                case TfxType.fxShockWave:
                                    FTargetX = FSourceX;
                                    FTargetY = FSourceY;
                                    FStrikeCount = 1;
                                    break;
                                case TfxType.fxTouch:
                                    FStrikeCount = 1;
                                    break;
                                case TfxType.fxSwarm:
                                case TfxType.fxScreenSwarm:
                                    FStrikeCount = 30;
                                    break;
                                default:
                                    if (FAbilityID == 52)
                                    {
                                        Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FSpellID).GetCombatInfo(FUnit.FUnit, ref Info);
                                        FStrikeCount = Info.RepeatCount;
                                    }
                                    else
                                    {
                                        FStrikeCount = ((TRangedAttackAbility)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(FAbilityID)).GetAttackRepeatRA(FUnit.FUnit);
                                    }

                                    break;
                            }

                            switch (FEffectType)
                            {
                                case TfxType.fxTremors:
                                    FSound = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.PlayEx(0, 64, true, 0, TSamplePanning.spCenter, null);
                                    NextStrike();
                                    break;
                                case TfxType.fxScreenSwarm:
                                    FSound = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FSpellID).FCastSounds.PlayEx(2, 64, true, 0, TSamplePanning.spCenter, null);
                                    {
                                        I = 0;
                                        int last = FStrikeCount - 1;
                                        for (; I <= last; I += 1)
                                        {
                                            NextStrike();
                                        }
                                    }

                                    break;
                                default:
                                    NextStrike();
                                    break;
                            }

                            FUnit.FMovePoints = 0;
                            FState += 1;
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token Start", "Error - TCRangedToken " + IntToStr(FAbilityID) + "/" + IntToStr(FSpellID), 0);
                    }

                    break;
                case 1:
                    try
                    {
                        EffectIndex = 0;
                        while (EffectIndex < Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FEffects.Count)
                        {
                            EndX = -1;
                            EndY = -1;
                            Effect = (TfxHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FEffects[EffectIndex];
                            if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 117)
                            {
                                if (Effect.FEffectType == TfxType.fxRock)
                                {
                                    if (Effect.FDone)
                                    {
                                        Effect.FEffectType = TfxType.fxFBExplosion;
                                        Effect.FDone = false;
                                        while (Effect.FPath.Count > 1)
                                        {
                                            Effect.FPath.Delete(0);
                                        }
                                    }
                                }
                                else
                                {
                                    switch (Effect.FFrame)
                                    {
                                        case 5:
                                            Msg.MsgID = 200707;
                                            Msg.Param[0] = FUnit;
                                            Msg.Result = -1;
                                            var path = Effect.FPath[0];
                                            Aow.Game.World.Map.HexGeometry.HPtoHX(Effect.FPath[0].X, path.Y, ref EndX, ref EndY);
                                            {
                                                I = 0;
                                                for (; I <= 6; I += 1)
                                                {
                                                    Aow.Game.World.Map.HexGeometry.CenterHNtoHX(EndX, EndY, I, ref X, ref Y);
                                                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                    Aow.Game.Draw.EffectPaths.CreateFire(false, 50, X, Y);
                                                }
                                            }

                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 107)
                                {
                                    if (Effect.FFrame != -1 && Effect.FPath[Effect.FFrame].Z == 15)
                                    {
                                        var path = Effect.FPath[Effect.FFrame];
                                        Aow.Game.World.Map.HexGeometry.HPtoHX(Effect.FPath[Effect.FFrame].X, path.Y, ref X, ref Y);
                                        HS = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(X, Y).FindOwnedChild(2228484);
                                        if (HS != null)
                                        {
                                            RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                            Effect.FCombatAction = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(Effect.FSpellIndex).CreateCA(FUnit, HS.FUnit, false);
                                            var yhx = HS.Yhx;
                                            Aow.Game.World.Map.HexGeometry.HXtoHP(HS.Xhx, yhx, ref I, ref X);
                                            if (((TDamageCA)Effect.FCombatAction).FDamageTypes.Value - ((TDamageCA)Effect.FCombatAction).FEffects.Value != TSet<TDamageType>.Of())
                                            {
                                                Aow.Game.Commands.CombatEvents.MakeDEHSList(I, X, ((TDamageCA)Effect.FCombatAction).FDamageTypes.Value - ((TDamageCA)Effect.FCombatAction).FEffects.Value, false);
                                            }

                                            if (((TDamageCA)Effect.FCombatAction).FEffects.Value != TSet<TDamageType>.Of())
                                            {
                                                Aow.Game.Commands.CombatEvents.MakeDEHSList(I, X, ((TDamageCA)Effect.FCombatAction).FEffects.Value, true);
                                            }

                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(Effect.FCombatAction);
                                            Effect.FCombatAction.Release();
                                            Effect.FCombatAction = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 112)
                                    {
                                        switch (Effect.FFrame)
                                        {
                                            case 89:
                                                Msg.MsgID = 200708;
                                                Msg.Param[0] = FUnit;
                                                Msg.Param[1] = 0;
                                                Msg.Result = -1;
                                                {
                                                    I = 6;
                                                    for (; I <= 59; I += 1)
                                                    {
                                                        X = 6;
                                                        for (; X <= 52; X += 1)
                                                        {
                                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(I, X).SendMapFieldMsg(ref Msg);
                                                        }
                                                    }
                                                }

                                                break;
                                        }
                                    }
                                    else
                                    {
                                        if (Effect.FAbilityIndex == 52 && Effect.FEffectType == TfxType.fxResurrect && (Effect.FSpellIndex == 130 || Effect.FSpellIndex == 129))
                                        {
                                            switch (Effect.FFrame)
                                            {
                                                case 0:
                                                    Msg.MsgID = 200712;
                                                    Msg.Param[0] = FUnit;
                                                    Msg.Param[1] = FSpellID;
                                                    Msg.Result = -1;
                                                    {
                                                        I = 0;
                                                        for (; I <= 6; I += 1)
                                                        {
                                                            Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                        }
                                                    }

                                                    break;
                                                case 99:
                                                    Msg.MsgID = 200713;
                                                    Msg.Param[0] = FUnit;
                                                    Msg.Param[1] = FSpellID;
                                                    Msg.Result = -1;
                                                    {
                                                        I = 0;
                                                        for (; I <= 6; I += 1)
                                                        {
                                                            Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                        }
                                                    }

                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 104)
                                            {
                                                switch (Effect.FFrame)
                                                {
                                                    case 199:
                                                        Msg.MsgID = 200709;
                                                        Msg.Param[0] = FUnit;
                                                        Msg.Result = -1;
                                                        {
                                                            I = 0;
                                                            for (; I <= 6; I += 1)
                                                            {
                                                                Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                            }
                                                        }

                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 127)
                                                {
                                                    switch (Effect.FFrame)
                                                    {
                                                        case 175:
                                                            Msg.MsgID = 200717;
                                                            Msg.Param[0] = FUnit;
                                                            Msg.Result = -1;
                                                            {
                                                                I = 1;
                                                                for (; I <= 6; I += 1)
                                                                {
                                                                    Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                                }
                                                            }

                                                            break;
                                                        case 200:
                                                            Msg.MsgID = 200717;
                                                            Msg.Param[0] = FUnit;
                                                            Msg.Result = -1;
                                                            {
                                                                I = 7;
                                                                for (; I <= 18; I += 1)
                                                                {
                                                                    Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                                }
                                                            }

                                                            break;
                                                        case 225:
                                                            Msg.MsgID = 200717;
                                                            Msg.Param[0] = FUnit;
                                                            Msg.Result = -1;
                                                            {
                                                                I = 19;
                                                                for (; I <= 36; I += 1)
                                                                {
                                                                    Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                                }
                                                            }

                                                            break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 128)
                                                    {
                                                        switch (Effect.FFrame)
                                                        {
                                                            case 100:
                                                                var yhx = FUnit.FUnitHS.Yhx;
                                                                Aow.Game.Draw.EffectPaths.CreateFire(false, 50, FUnit.FUnitHS.Xhx, yhx);
                                                                Msg.MsgID = 200718;
                                                                Msg.Param[0] = FUnit;
                                                                Msg.Result = -1;
                                                                var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                                                                var xhx = FUnit.FUnitHS.Xhx;
                                                                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(xhx, fUnitHSYhx).SendMapFieldMsg(ref Msg);
                                                                break;
                                                            case 175:
                                                                Msg.MsgID = 200718;
                                                                Msg.Param[0] = FUnit;
                                                                Msg.Result = -1;
                                                                {
                                                                    I = 0;
                                                                    for (; I <= 18; I += 1)
                                                                    {
                                                                        Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                                        Aow.Game.Draw.EffectPaths.CreateFire(false, 50, X, Y);
                                                                    }
                                                                }

                                                                break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 105 && Effect.FTarget != null)
                                                        {
                                                            I = 0;
                                                            int last = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                                                            for (; I <= last; I += 1)
                                                            {
                                                                if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I) != null && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I) is TCombatUnit && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I).GetSide() != FUnit.GetSide() && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I).GetEnabled() && ((TTacticalCombatUnit)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I)).FUnitHS != null)
                                                                {
                                                                    RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                                                    var combatObject = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                                                    Action = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FSpellID).CreateCA(FUnit, combatObject, false);
                                                                    if (((TExclusiveCombatSpellCA)Action).FSucceeded)
                                                                    {
                                                                        ((TTacticalCombatUnit)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I)).FUnitHS.SetAction(TTCAction.caHit);
                                                                    }

                                                                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(Action);
                                                                    Action.Release();
                                                                }
                                                            }

                                                            Effect.FTarget = null;
                                                        }
                                                        else
                                                        {
                                                            if (Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 120)
                                                            {
                                                                switch (Effect.FFrame)
                                                                {
                                                                    case 2999:
                                                                        Msg.MsgID = 200716;
                                                                        Msg.Param[0] = FUnit;
                                                                        Msg.Result = -1;
                                                                        {
                                                                            I = 0;
                                                                            for (; I <= 6; I += 1)
                                                                            {
                                                                                Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref X, ref Y);
                                                                                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(X, Y).SendMapFieldMsg(ref Msg);
                                                                            }
                                                                        }

                                                                        break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (Effect.FEffectType == TfxType.fxSacredWrath && Effect.FAbilityIndex == 52 && Effect.FSpellIndex == 125)
                                                                {
                                                                    switch (Effect.FFrame)
                                                                    {
                                                                        case 5:
                                                                            {
                                                                                I = 0;
                                                                                int last = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                                                                                for (; I <= last; I += 1)
                                                                                {
                                                                                    if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I) is TCombatUnit && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I).GetEnabled())
                                                                                    {
                                                                                        HS = ((TTacticalCombatUnit)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I)).FUnitHS;
                                                                                        if (HS != null)
                                                                                        {
                                                                                            ExtraEffect = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.AddEffect(TfxType.fxSWComet);
                                                                                            ExtraEffect.FAbilityIndex = FAbilityID;
                                                                                            ExtraEffect.FSpellIndex = FSpellID;
                                                                                            ExtraEffect.FTarget = HS.FUnit;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }

                                                                            break;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (Effect.FDone && Effect.FTarget != null && Effect.FTarget.GetEnabled())
                                                                    {
                                                                        I = 0;
                                                                        RandSeed = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(16777215);
                                                                        if (FAbilityID == 52)
                                                                        {
                                                                            Effect.FCombatAction = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(FSpellID).CreateCA(FUnit, Effect.FTarget, false);
                                                                        }
                                                                        else
                                                                        {
                                                                            if (FAbilityID == 47)
                                                                            {
                                                                                Effect.FCombatAction = ((THealingAbility)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(FAbilityID)).CreateHealingCA(FUnit, Effect.FTarget);
                                                                            }
                                                                            else
                                                                            {
                                                                                if (FAbilityID == 60)
                                                                                {
                                                                                    Effect.FCombatAction = ((TDispelMagicAbility)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(FAbilityID)).CreateCA(FUnit, Effect.FTarget);
                                                                                }
                                                                                else
                                                                                {
                                                                                    Effect.FCombatAction = ((TRangedAttackAbility)Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(FAbilityID)).CreateRangedAttackCA(FUnit, Effect.FTarget, 0);
                                                                                }
                                                                            }
                                                                        }

                                                                        if (Effect.FCombatAction is TDamageCA)
                                                                        {
                                                                            I = ((TDamageCA)Effect.FCombatAction).FDamage;
                                                                            var yhx = ((TTacticalCombatUnit)Effect.FTarget).FUnitHS.Yhx;
                                                                            Aow.Game.World.Map.HexGeometry.HXtoHP(((TTacticalCombatUnit)Effect.FTarget).FUnitHS.Xhx, yhx, ref X, ref Y);
                                                                            if (((TDamageCA)Effect.FCombatAction).FDamageTypes.Value - ((TDamageCA)Effect.FCombatAction).FEffects.Value != TSet<TDamageType>.Of())
                                                                            {
                                                                                Aow.Game.Commands.CombatEvents.MakeDEHSList(X, Y, ((TDamageCA)Effect.FCombatAction).FDamageTypes.Value - ((TDamageCA)Effect.FCombatAction).FEffects.Value, false);
                                                                            }

                                                                            if (((TDamageCA)Effect.FCombatAction).FEffects.Value != TSet<TDamageType>.Of())
                                                                            {
                                                                                Aow.Game.Commands.CombatEvents.MakeDEHSList(X, Y, ((TDamageCA)Effect.FCombatAction).FEffects.Value, true);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (Effect.FCombatAction is TEntangleSpellCA)
                                                                            {
                                                                                if (((TExclusiveCombatSpellCA)Effect.FCombatAction).FSucceeded)
                                                                                {
                                                                                    ((TTacticalCombatUnit)Effect.FTarget).FUnitHS.SetAction(TTCAction.caHit);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (Effect.FCombatAction is TMindDecayCA && ((TExclusiveCombatSpellCA)Effect.FCombatAction).FSucceeded)
                                                                                {
                                                                                    ((TTacticalCombatUnit)Effect.FTarget).FUnitHS.SetAction(TTCAction.caHit);
                                                                                }
                                                                            }
                                                                        }

                                                                        if (I > 0)
                                                                        {
                                                                            ((TTacticalCombatUnit)Effect.FTarget).FUnitHS.SetAction(TTCAction.caHit);
                                                                            Aow.Game.Combat.TTacticalCombatUnit.MakeHitBlood((TTacticalCombatUnit)Effect.FTarget, I);
                                                                            Effect.FCancelled = true;
                                                                        }
                                                                    }

                                                                    if (Effect.FDone)
                                                                    {
                                                                        var count = Effect.FPath.Count;
                                                                        var path = Effect.FPath[count - 1];
                                                                        var fPathCount = Effect.FPath.Count;
                                                                        Aow.Game.World.Map.HexGeometry.HPtoHX(Effect.FPath[fPathCount - 1].X, path.Y, ref EndX, ref EndY);
                                                                        if (Effect.FCombatAction != null && Effect.FTarget != null)
                                                                        {
                                                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.ExecuteCombatAction(Effect.FCombatAction);
                                                                            Effect.FCombatAction.Release();
                                                                            Effect.FCombatAction = null;
                                                                            if (!Effect.FTarget.GetEnabled())
                                                                            {
                                                                                I = 0;
                                                                                int last = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FEffects.Count - 1;
                                                                                for (; I <= last; I += 1)
                                                                                {
                                                                                    if (I != EffectIndex)
                                                                                    {
                                                                                        if (((TfxHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FEffects[I]).FTarget == Effect.FTarget)
                                                                                        {
                                                                                            ((TfxHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FEffects[I]).FTarget = null;
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (EndX > -1)
                                                                            {
                                                                                switch (Effect.FAbilityIndex)
                                                                                {
                                                                                    case 30:
                                                                                    case >= 86 and <= 90:
                                                                                        if (Effect.FTargetX > 0)
                                                                                        {
                                                                                            Msg.MsgID = 200722;
                                                                                            Msg.Param[0] = FUnit;
                                                                                            Msg.Param[1] = Effect.FAbilityIndex;
                                                                                            Msg.Result = -1;
                                                                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(Effect.FTargetX, Effect.FTargetY).SendMapFieldMsg(ref Msg);
                                                                                        }

                                                                                        break;
                                                                                    case 24:
                                                                                        Msg.MsgID = 200705;
                                                                                        Msg.Param[0] = FUnit;
                                                                                        Msg.Result = -1;
                                                                                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                        break;
                                                                                    case 44:
                                                                                        Msg.MsgID = 200706;
                                                                                        Msg.Param[0] = FUnit;
                                                                                        Msg.Result = -1;
                                                                                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                        break;
                                                                                    case 31:
                                                                                        Msg.MsgID = 200714;
                                                                                        Msg.Param[0] = FUnit;
                                                                                        Msg.Result = -1;
                                                                                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                        Aow.Game.Draw.EffectPaths.CreateFire(false, 100, EndX, EndY);
                                                                                        break;
                                                                                    case 52:
                                                                                        switch (Effect.FEffectType)
                                                                                        {
                                                                                            case TfxType.fxBreath:
                                                                                                if (Effect.FTargetX > 0)
                                                                                                {
                                                                                                    Msg.MsgID = 200723;
                                                                                                    Msg.Param[0] = FUnit;
                                                                                                    Msg.Param[1] = Effect.FSpellIndex;
                                                                                                    Msg.Result = -1;
                                                                                                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(Effect.FTargetX, Effect.FTargetY).SendMapFieldMsg(ref Msg);
                                                                                                }

                                                                                                break;
                                                                                            case TfxType.fxScreenNS:
                                                                                                switch (Effect.FSpellIndex)
                                                                                                {
                                                                                                    case 116:
                                                                                                        Msg.MsgID = 200710;
                                                                                                        Msg.Param[0] = FUnit;
                                                                                                        Msg.Result = -1;
                                                                                                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                                        break;
                                                                                                    case 124:
                                                                                                        if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 0 && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 10 && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(EndX, EndY).OverlayType != 2 && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(EndX, EndY).OverlayType != 11 && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(EndX, EndY).OverlayType != 8 && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 9 && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 15 && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(EndX, EndY).TerrainType != 7)
                                                                                                        {
                                                                                                            Ooze = new TCOozeHS(null);
                                                                                                            Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.PlaceHX(Ooze, EndX, EndY, 0);
                                                                                                        }

                                                                                                        break;
                                                                                                }

                                                                                                break;
                                                                                            case TfxType.fxEffect:
                                                                                                switch (Effect.FSpellIndex)
                                                                                                {
                                                                                                    case 113:
                                                                                                        Msg.MsgID = 200715;
                                                                                                        Msg.Param[0] = FUnit;
                                                                                                        Msg.Result = -1;
                                                                                                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(EndX, EndY).SendMapFieldMsg(ref Msg);
                                                                                                        Aow.Game.Draw.EffectPaths.CreateFire(false, 100, EndX, EndY);
                                                                                                        break;
                                                                                                }

                                                                                                break;
                                                                                        }

                                                                                        break;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            EffectIndex += 1;
                        }

                        if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FEffects.Count == 0 && (FStrikeIndex >= FStrikeCount || !FUnit.GetEnabled()))
                        {
                            if (FUnit.GetEnabled())
                            {
                                FUnit.FUnitHS.SetAction(TTCAction.caMove);
                            }

                            Result = TExecuteResult.erDone;
                        }

                        if (FUnit.GetEnabled())
                        {
                            if (FEffectType == TfxType.fxBreath)
                            {
                                NextStrike();
                            }
                            else
                            {
                                if (FUnit.FUnitHS.FActionDone || (FAbilityID == 52 && FDelay <= 0))
                                {
                                    FDelay = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(3) + 4;
                                }

                                FDelay -= 1;
                                if (FDelay == 0)
                                {
                                    NextStrike();
                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox(0, "Error during Token Execute", "Error - TCRangedToken " + IntToStr(FAbilityID) + "/" + IntToStr(FSpellID), 0);
                    }

                    break;
            }

            if (Result == TExecuteResult.erDone)
            {
                if (FSound != null)
                {
                    (FSound as IDisposable)?.Dispose();
                }

                if (FUnit != null && FUnit.GetEnabled())
                {
                    FUnit.FUnitHS.FSelectedAbility = -1;
                    FUnit.FUnitHS.FSelectedSpell = -1;
                    FUnit.FUnitHS.Unselect();
                    if (FUnit.FCombatPlayer == Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer)
                    {
                        var xhx = FUnit.FUnitHS.Xhx;
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlayers.GetPlayers(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer).FLastX = xhx;
                        var yhx = FUnit.FUnitHS.Yhx;
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlayers.GetPlayers(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentCombatPlayer).FLastY = yhx;
                    }
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Token", "Error - TCRangedToken " + IntToStr(FAbilityID) + "/" + IntToStr(FSpellID), 0);
        }

        return Result;
    }

    // CombatTE.pas:1373
    public void NextStrike()
    {
        TList Used;
        TTacticalCombatUnit Target;
        TfxHS Effect;
        TfxHS ExtraEffect;
        TTacticalCombatUnitHS HS;
        float A;
        float Sweep;
        float Angle;
        float Radius;
        int X1 = default;
        int Y1 = default;
        int X2 = default;
        int Y2 = default;
        int DX = default;
        int DY = default;
        int I;
        int J = default;
        int K = default;
        TXYZ Point;
        THexagonSprite HitObject;
        TXYZList Path;
        TList Obstacles;
        TROb Obstacle;
        bool Found;
        TTacticalCombatUnit FindTarget(int X, int Y)
        {
            int Distance;
            int BestDistance;
            bool Available;
            TTacticalCombatUnit Result = null;
            BestDistance = 6;
            for (int I = 0, last = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetCount() - 1; I <= last; I++)
            {
                if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I) is TCombatUnit && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I).GetEnabled() && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I).GetSide() != FUnit.GetSide())
                {
                    Available = true;
                    for (int J = 0, lastJ = Used.Count - 1; J <= lastJ; J++)
                    {
                        if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I) == Used[J])
                        {
                            Available = false;
                        }
                    }

                    if (Available)
                    {
                        var yhx = ((TTacticalCombatUnit)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I)).FUnitHS.Yhx;
                        Distance = Aow.Game.World.Map.HexGeometry.dHXtoRad(X, Y, ((TTacticalCombatUnit)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I)).FUnitHS.Xhx, yhx);
                        if (Distance < BestDistance)
                        {
                            Result = (TTacticalCombatUnit)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            BestDistance = Distance;
                        }
                    }
                }
            }

            return Result;
        }

        void ChooseTarget()
        {
            int I;
            int Index;
            int Total;
            int Distance;
            TList Weights;
            Used = new TList();
            Weights = new TList();
            Total = 0;
            I = 0;
            int last = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
            for (; I <= last; I += 1)
            {
                if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I) is TCombatUnit && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I).GetEnabled() && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I) != Target)
                {
                    var yhx = ((TTacticalCombatUnit)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I)).FUnitHS.Yhx;
                    var xhx = ((TTacticalCombatUnit)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I)).FUnitHS.Xhx;
                    Distance = Aow.Game.World.Map.HexGeometry.dHXtoRad(FTargetX, FTargetY, xhx, yhx);
                    if (Distance < 13)
                    {
                        Total = Total + (13 - Distance);
                        var combatObject = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                        Used.Add(combatObject);
                        Weights.Add(IntToObject(13 - Distance));
                    }
                }
            }

            if (Target != null)
            {
                Used.Add(Target);
                Weights.Add(IntToObject(Total));
                Total = Total + Total;
            }

            if (Used.Count > 0)
            {
                I = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(Total) - ObjectToInt((object)Weights[0]);
                Index = 0;
                while (Index < Used.Count - 1 && I > 0)
                {
                    Index = Index + 1;
                    I = I - ObjectToInt((object)Weights[Index]);
                }

                Target = (TTacticalCombatUnit)Used[Index];
            }

            Used?.Dispose();
            Weights?.Dispose();
        }

        if (FStrikeIndex >= FStrikeCount)
        {
            return;
        }

        Effect = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.AddEffect(TfxType.fxNone);
        Obstacles = new TList();
        switch (FEffectType)
        {
            case TfxType.fxBreath:
                var yhx = FUnit.FUnitHS.Yhx;
                var dHXtoHN2 = Aow.Game.World.Map.HexGeometry.dHXtoHN(FUnit.FUnitHS.Xhx, yhx, FTargetX, FTargetY);
                var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FUnit.FUnitHS.Xhx, fUnitHSYhx, Aow.Game.Combat.TacticalTables.BreathHN[Aow.Game.Combat.TacticalTables.BreathDirection[dHXtoHN2 - 1] * 12 - 3], ref J, ref K);
                var yhx2 = FUnit.FUnitHS.Yhx;
                Aow.Game.World.Map.HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx2, ref X1, ref Y1);
                Aow.Game.World.Map.HexGeometry.HXtoHP(J, K, ref X2, ref Y2);
                Sweep = FStrikeIndex;
                if ((double)Sweep > 9)
                {
                    Sweep = (float)((double)19 - Sweep);
                }

                if (Y2 == Y1)
                {
                    A = 0;
                }
                else
                {
                    if (X2 == X1)
                    {
                        A = (float)((double)Math.PI / 2);
                    }
                    else
                    {
                        A = (float)Math.Atan((double)Math.Abs(X2 - X1) / Math.Abs(Y2 - Y1));
                    }
                }

                if (Y2 == Y1 || X2 == X1)
                {
                    var pi = Math.PI;
                    var mathPI = Math.PI;
                    Angle = (float)((double)((double)((double)pi / 2) - A) + (double)((double)((double)((double)Sweep * 6) - 30) * mathPI) / 180);
                }
                else
                {
                    Angle = (float)((double)A + (double)((double)((double)((double)Sweep * 6) - 30) * Math.PI) / 180);
                }

                Radius = (float)Math.Sqrt((X2 - X1) * (X2 - X1) + (Y2 - Y1) * (Y2 - Y1));
                if (X2 >= X1)
                {
                    DX = Round((double)Math.Sin(Angle) * Radius) - X2 + X1;
                }
                else
                {
                    DX = X1 - X2 - Round((double)Math.Sin(Angle) * Radius);
                }

                if (Y2 >= Y1)
                {
                    DY = Round((double)Math.Cos(Angle) * Radius) - Y2 + Y1;
                }
                else
                {
                    DY = Y1 - Y2 - Round((double)Math.Cos(Angle) * Radius);
                }

                var xhx = FUnit.FUnitHS.Xhx;
                var yhx3 = FUnit.FUnitHS.Yhx;
                Aow.Game.Combat.TacticalTables.MakeRangedPath(FUnit.GetSide(), 50, 20, 50, xhx, yhx3, (short)J, (short)K, (short)DX, (short)DY, false, ref Effect.FPath, ref Obstacles);
                if (FStrikeIndex < 12)
                {
                    var yhx4 = FUnit.FUnitHS.Yhx;
                    var dHXtoHN3 = Aow.Game.World.Map.HexGeometry.dHXtoHN(FUnit.FUnitHS.Xhx, yhx4, FTargetX, FTargetY);
                    var yhx5 = FUnit.FUnitHS.Yhx;
                    Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FUnit.FUnitHS.Xhx, yhx5, Aow.Game.Combat.TacticalTables.BreathHN[Aow.Game.Combat.TacticalTables.BreathDirection[dHXtoHN3 - 1] * 12 + Aow.Game.Commands.TCAbRangedTE.BreathHit[FStrikeIndex] - 12], ref Effect.FTargetX, ref Effect.FTargetY);
                }

                break;
            case TfxType.fxTouch:
            case TfxType.fxEffect:
            case TfxType.fxSacredWrath:
            case TfxType.fxGeyser:
            case TfxType.fxCloud:
            case TfxType.fxMindDecay:
            case TfxType.fxShockWave:
            case TfxType.fxSacFlame:
                if (FEffectType == TfxType.fxSacredWrath)
                {
                    FTargetX = 32;
                    FTargetY = 29;
                }

                Aow.Game.World.Map.HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                switch (FEffectType)
                {
                    case TfxType.fxGeyser:
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.CenterViewTo(FTargetX, FTargetY - 5, 0);
                        break;
                    case TfxType.fxMindDecay:
                        Effect.FTargetX = FTargetX;
                        Effect.FTargetY = FTargetY;
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.CenterViewTo(FTargetX + 1, FTargetY - 4, 0);
                        break;
                    case TfxType.fxShockWave:
                        var yhx4 = FUnit.FUnitHS.Yhx;
                        var fUnitHSXhx = FUnit.FUnitHS.Xhx;
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.CenterViewTo(fUnitHSXhx, yhx4, 0);
                        break;
                    case TfxType.fxSacFlame:
                        var yhx5 = FUnit.FUnitHS.Yhx;
                        var xhx2 = FUnit.FUnitHS.Xhx;
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.CenterViewTo(xhx2, yhx5 - 3, 0);
                        break;
                    default:
                        Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.CenterViewTo(FTargetX, FTargetY, 0);
                        break;
                }

                Point = new TXYZ();
                Point.X = X2 + 16;
                Point.Y = Y2 + 16;
                Effect.FPath.Add(Point);
                HS = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
                if (HS != null)
                {
                    Effect.FTarget = HS.FUnit;
                }

                break;
            case TfxType.fxWinds:
                Aow.Game.World.Map.HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                Point = new TXYZ();
                Point.X = X2 + 16;
                Point.Y = Y2 + 16;
                Effect.FPath.Add(Point);
                HS = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
                if (HS != null)
                {
                    Effect.FTarget = HS.FUnit;
                }

                I = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(37) + 1;
                Found = false;
                while (!Found)
                {
                    Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FTargetX, FTargetY, I, ref J, ref K);
                    bool condition = J > 5 && J < 60 && K > 5 && K < 53 && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2228484) == null;
                    if (condition)
                    {
                        bool test = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).OverlayType != 8;
                        if (!test)
                        {
                            var fieldValue = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K);
                            var fieldValue2 = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY);
                            test = fieldValue.OverlayType == fieldValue2.OverlayType;
                        }

                        condition = test;
                    }

                    bool passed = condition && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 15 && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 7;
                    if (passed)
                    {
                        bool test = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 0 && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 10 && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).TerrainType != 9;
                        if (!test)
                        {
                            var fieldValue = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K);
                            var fieldValue2 = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY);
                            test = fieldValue.TerrainType == fieldValue2.TerrainType;
                        }

                        passed = test;
                    }

                    if (passed)
                    {
                        Found = true;
                    }
                    else
                    {
                        I += 1;
                    }
                }

                Aow.Game.World.Map.HexGeometry.HXtoHP(J, K, ref DX, ref DY);
                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.CenterViewTo(FTargetX, FTargetY, 0);
                {
                    I = 0;
                    for (; I <= 29; I += 1)
                    {
                        var xyz = new TXYZ();
                        Effect.FPath.Add(xyz);
                        var pathX = Effect.FPath[0];
                        var random = Random(30);
                        var count = Effect.FPath.Count;
                        Effect.FPath[count - 1].X = pathX.X + random - 15;
                        var pathY = Effect.FPath[0];
                        var random2 = Random(30);
                        var fPathCount = Effect.FPath.Count;
                        Effect.FPath[fPathCount - 1].Y = pathY.Y + random2 - 15;
                    }
                }

                J = Random(300);
                K = Random(300);
                var count2 = Effect.FPath.Count;
                var path = Effect.FPath[count2 - 1];
                var count3 = Effect.FPath.Count;
                var pathZ = Effect.FPath[count3 - 1];
                var count4 = Effect.FPath.Count;
                var path2 = Effect.FPath[count4 - 1];
                var count5 = Effect.FPath.Count;
                var path3 = Effect.FPath[count5 - 1];
                var count6 = Effect.FPath.Count;
                var path4 = Effect.FPath[count6 - 1];
                var count7 = Effect.FPath.Count;
                Aow.Game.Draw.EffectPaths.MakeArcPathXY((short)Effect.FPath[count7 - 1].X, (short)(path2.Y - path3.Z), (short)(path4.X + J - 150), (short)(path.Y - pathZ.Z + K - 150), (short)((Math.Abs(J - 150) + Math.Abs(K - 150)) / 50 + 2), 100, ref Effect.FPath);
                Effect.FTargetX = Effect.FPath.Count;
                J = Random(200);
                K = Random(200);
                var count8 = Effect.FPath.Count;
                var path5 = Effect.FPath[count8 - 1];
                var count9 = Effect.FPath.Count;
                var path6 = Effect.FPath[count9 - 1];
                var count10 = Effect.FPath.Count;
                var path7 = Effect.FPath[count10 - 1];
                var count11 = Effect.FPath.Count;
                var path8 = Effect.FPath[count11 - 1];
                var count12 = Effect.FPath.Count;
                var path9 = Effect.FPath[count12 - 1];
                var count13 = Effect.FPath.Count;
                Aow.Game.Draw.EffectPaths.MakeArcPathXY((short)Effect.FPath[count13 - 1].X, (short)(path7.Y - path8.Z), (short)(path9.X + J - 100), (short)(path5.Y - path6.Z + K - 100), (short)((Math.Abs(J - 100) + Math.Abs(K - 100)) / 50 + 3), 100, ref Effect.FPath);
                Effect.FTargetY = Effect.FPath.Count;
                var count14 = Effect.FPath.Count;
                var item = Effect.FPath[count14 - 1];
                var count15 = Effect.FPath.Count;
                var fPathItem = Effect.FPath[count15 - 1];
                var count16 = Effect.FPath.Count;
                var path10 = Effect.FPath[count16 - 1];
                var count17 = Effect.FPath.Count;
                var path11 = Effect.FPath[count17 - 1];
                var count18 = Effect.FPath.Count;
                var path12 = Effect.FPath[count18 - 1];
                var count19 = Effect.FPath.Count;
                Aow.Game.Draw.EffectPaths.MakeArcPathXY((short)Effect.FPath[count19 - 1].X, (short)(path11.Y - path12.Z), (short)(DX + 16), (short)(DY + 16), (short)((Math.Abs(item.Y - fPathItem.Z - DY) + Math.Abs(path10.X - DX)) / 50 + 4), 100, ref Effect.FPath);
                break;
            case TfxType.fxScreenNESW:
                Point = new TXYZ();
                if (Random(100) < 50)
                {
                    Point.X = 0;
                }
                else
                {
                    Point.X = 1;
                }

                Effect.FPath.Add(Point);
                if (FStrikeIndex == 9)
                {
                    Effect.FTarget = FUnit;
                }

                break;
            case TfxType.fxScreenNS:
                switch (FSpellID)
                {
                    case 124:
                        I = 19;
                        break;
                    default:
                        I = 7;
                        break;
                }

                Aow.Game.World.Map.HexGeometry.CenterHNtoHX(FTargetX, FTargetY, Aow.Game.Commands.TCAbRangedTE.rndHex[FStrikeIndex % I], ref DX, ref DY);
                Aow.Game.World.Map.HexGeometry.HXtoHP(DX, DY, ref X2, ref Y2);
                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MakeVisible(DX, DY, 0, 5);
                X1 = Random(20) + 6;
                Y1 = Random(20) + 6;
                Aow.Game.Draw.EffectPaths.MakeLinePath(X2 + X1 - 100, Y2 + Y1 - 300, X2 + X1, Y2 + Y1, 30, ref Effect.FPath);
                switch (FSpellID)
                {
                    case 124:
                        Effect.FTarget = null;
                        break;
                    default:
                        {
                            HS = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(DX, DY).FindOwnedChild(2228484);
                            if (HS != null)
                            {
                                Effect.FTarget = HS.FUnit;
                            }
                        }

                        break;
                }

                break;
            case TfxType.fxChain:
                var yhx6 = FUnit.FUnitHS.Yhx;
                Aow.Game.World.Map.HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx6, ref X1, ref Y1);
                Aow.Game.World.Map.HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                Aow.Game.Draw.EffectPaths.MakeLightningPath(X1 + 16, Y1, X2 + 16, Y2, ref Effect.FPath);
                var count20 = Effect.FPath.Count;
                Effect.FPath[count20 - 1].Z = 16;
                I = 3;
                Used = new TList();
                Used.Add(FUnit);
                HS = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
                if (HS != null)
                {
                    Used.Add(HS.FUnit);
                }

                Target = FindTarget(FTargetX, FTargetY);
                while (Target != null && I > 0)
                {
                    Used.Add(Target);
                    X1 = X2 + 16;
                    Y1 = Y2 + 16;
                    var yhx4 = Target.FUnitHS.Yhx;
                    Aow.Game.World.Map.HexGeometry.HXtoHP(Target.FUnitHS.Xhx, yhx4, ref X2, ref Y2);
                    Aow.Game.Draw.EffectPaths.MakeLightningPath(X1, Y1, X2 + 16, Y2, ref Effect.FPath);
                    I -= 1;
                    var yhx5 = Target.FUnitHS.Yhx;
                    Target = FindTarget(Target.FUnitHS.Xhx, yhx5);
                }

                Used?.Dispose();
                break;
            case TfxType.fxSwarm:
                var yhx7 = FUnit.FUnitHS.Yhx;
                Aow.Game.World.Map.HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx7, ref X1, ref Y1);
                var random3 = Random(360);
                var random4 = Random(20);
                var random5 = Random(20);
                var random6 = Random(2);
                var random7 = Random(90);
                var random8 = Random(5);
                Aow.Game.Draw.EffectPaths.MakeSwarmPath(Random(10) + 10, random8 + 20, random7, random3, random4 + X1 + 6, random5 + Y1 - 10, (TSwarmDirection)random6, ref Effect.FPath);
                K = Effect.FPath.Count - 1;
                {
                    I = 0;
                    for (; I <= 2; I += 1)
                    {
                        J = 0;
                        int last = K;
                        for (; J <= last; J += 1)
                        {
                            Point = new TXYZ();
                            Point.X = Effect.FPath[J].X;
                            Point.Y = Effect.FPath[J].Y;
                            Effect.FPath.Add(Point);
                        }
                    }
                }

                Aow.Game.World.Map.HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                var count21 = Effect.FPath.Count;
                var path13 = Effect.FPath[count21 - 1];
                var count22 = Effect.FPath.Count;
                Aow.Game.Draw.EffectPaths.MakeLinePath(Effect.FPath[count22 - 1].X, path13.Y, X2 + 16, Y2, 15, ref Effect.FPath);
                Path = new TXYZList();
                var random9 = Random(360);
                var count23 = Effect.FPath.Count;
                var path14 = Effect.FPath[count23 - 1];
                var count24 = Effect.FPath.Count;
                var path15 = Effect.FPath[count24 - 1];
                var random10 = Random(2);
                var random11 = Random(90);
                var random12 = Random(5);
                Aow.Game.Draw.EffectPaths.MakeSwarmPath(Random(10) + 10, random12 + 20, random11, random9, path14.X, path15.Y, (TSwarmDirection)random10, ref Path);
                var pathItem = Path[0];
                var count25 = Effect.FPath.Count;
                var path16 = Effect.FPath[count25 - 1];
                var count26 = Effect.FPath.Count;
                var path17 = Effect.FPath[count26 - 1];
                var pathX2 = Path[0];
                Aow.Game.Draw.EffectPaths.MakeLinePath(path17.X, path16.Y, pathX2.X, pathItem.Y, 5, ref Effect.FPath);
                {
                    I = 0;
                    int last = Path.Count - 1;
                    for (; I <= last; I += 1)
                    {
                        // { Original shares the points, then frees Path. }
                        var path18 = Path[I];
                        Effect.FPath.Add(path18);
                    }
                }

                K = Path.Count - 1;
                {
                    I = 0;
                    for (; I <= 2; I += 1)
                    {
                        J = 0;
                        int last = K;
                        for (; J <= last; J += 1)
                        {
                            Point = new TXYZ();
                            Point.X = Path[J].X;
                            Point.Y = Path[J].Y;
                            Effect.FPath.Add(Point);
                        }
                    }
                }

                Path?.Dispose();
                break;
            case TfxType.fxScreenSwarm:
                HS = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
                if (HS == null)
                {
                    Target = null;
                }
                else
                {
                    Target = HS.FUnit;
                }

                DX = FTargetX;
                DY = FTargetY;
                ChooseTarget();
                if (Target != null)
                {
                    DX = Target.FUnitHS.Xhx;
                    DY = Target.FUnitHS.Yhx;
                }

                Effect.FTarget = Target;
                Aow.Game.World.Map.HexGeometry.HXtoHP(DX, DY, ref X2, ref Y2);
                if (FStrikeIndex == 0)
                {
                    Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.CenterViewTo(FTargetX, FTargetY, 0);
                }

                Point = new TXYZ();
                Point.X = X2;
                Point.Y = Y2;
                Effect.FPath.Add(Point);
                var random13 = Random(360);
                var random14 = Random(20);
                var random15 = Random(20);
                var random16 = Random(2);
                var random17 = Random(90);
                var random18 = Random(10);
                Aow.Game.Draw.EffectPaths.MakeSwarmPath(Random(10) + 10, random18 + 25, random17, random13, random14, random15 - 10, (TSwarmDirection)random16, ref Effect.FPath);
                K = Effect.FPath.Count - 1;
                {
                    I = 0;
                    for (; I <= 4; I += 1)
                    {
                        J = 1;
                        int last = K;
                        for (; J <= last; J += 1)
                        {
                            Point = new TXYZ();
                            Point.X = Effect.FPath[J].X;
                            Point.Y = Effect.FPath[J].Y;
                            Effect.FPath.Add(Point);
                        }
                    }
                }

                {
                    I = 0;
                    for (; I <= 49; I += 1)
                    {
                        Effect.FPath[I + 1].Z = 100 - I * 2;
                    }
                }

                break;
            case TfxType.fxFrostBeam:
                Aow.Game.World.Map.HexGeometry.HXtoHP(FTargetX, FTargetY, ref X2, ref Y2);
                Effect.FTargetX = FTargetX;
                Effect.FTargetY = FTargetY;
                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.CenterViewTo(FTargetX - 3, FTargetY - 3, 0);
                Aow.Game.Draw.EffectPaths.MakeArcPathXY((short)(X2 - 200), (short)(Y2 - 200), (short)(X2 + 16), (short)(Y2 + 16), 5, 100, ref Effect.FPath);
                break;
            case TfxType.fxRay:
                Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MakeVisible(FTargetX - 3, FTargetY - 3, 0, 3);
                var xhx3 = FUnit.FUnitHS.Xhx;
                var yhx8 = FUnit.FUnitHS.Yhx;
                Aow.Game.Combat.TacticalTables.MakeRangedPath(FUnit.GetSide(), 10, 0, 50, xhx3, yhx8, (short)FTargetX, (short)FTargetY, 0, 0, true, ref Effect.FPath, ref Obstacles);
                break;
            default:
                var xhx4 = FUnit.FUnitHS.Xhx;
                var yhx9 = FUnit.FUnitHS.Yhx;
                Aow.Game.Combat.TacticalTables.MakeAbilPath(FUnit.GetSide(), (short)FAbilityID, (short)FSpellID, xhx4, yhx9, (short)FTargetX, (short)FTargetY, ref Effect.FPath, ref Obstacles);
                break;
        }

        Effect.FAbilityIndex = FAbilityID;
        Effect.FSpellIndex = FSpellID;
        Effect.FEffectType = FEffectType;
        switch (FEffectType)
        {
            case TfxType.fxArrow:
            case TfxType.fxRock:
            case TfxType.fxComet:
            case TfxType.fxBreath:
            case TfxType.fxCometBall:
            case TfxType.fxRay:
            case TfxType.fxSwarm:
            case TfxType.fxAltArrow:
                HS = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(FTargetX, FTargetY).FindOwnedChild(2228484);
                if (HS != null)
                {
                    Effect.FTarget = HS.FUnit;
                }

                I = 0;
                while (I < Obstacles.Count)
                {
                    Obstacle = (TROb)Obstacles[I];
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(100) < Obstacle.Chance)
                    {
                        var path = Effect.FPath[Obstacle.PathIndex];
                        Aow.Game.World.Map.HexGeometry.HPtoHX(Effect.FPath[Obstacle.PathIndex].X, path.Y, ref J, ref K);
                        if (Effect.FEffectType == TfxType.fxBreath)
                        {
                            HitObject = null;
                        }
                        else
                        {
                            HitObject = (THexagonSprite)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2228484);
                        }

                        if (HitObject == null)
                        {
                            HitObject = (THexagonSprite)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2228582);
                        }

                        if (HitObject == null)
                        {
                            HitObject = (THexagonSprite)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2229508);
                        }

                        if (HitObject == null)
                        {
                            HitObject = (THexagonSprite)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).FindOwnedChild(2229510);
                        }

                        if (HitObject != null || Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).TerrainType == 15 || Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).TerrainType == 7 || Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].GetField(J, K).OverlayType == 8)
                        {
                            J = Obstacle.PathIndex + 1;
                            int last = Effect.FPath.Count - 1;
                            for (; J <= last; J += 1)
                            {
                                var count = Effect.FPath.Count;
                                Effect.FPath.Delete(count - 1);
                            }

                            if (!(HitObject is TTacticalCombatUnitHS))
                            {
                                Effect.FTarget = null;
                                if (Effect.FEffectType == TfxType.fxBreath)
                                {
                                    Effect.FTargetX = 0;
                                }
                            }
                            else
                            {
                                Effect.FTarget = ((TTacticalCombatUnitHS)HitObject).FUnit;
                            }

                            I = Obstacles.Count;
                        }
                    }

                    I += 1;
                }

                break;
        }

        if (FEffectType == TfxType.fxBreath)
        {
            Effect.FTarget = null;
        }
        else
        {
            switch (FAbilityID)
            {
                case 31:
                    Effect.FTarget = null;
                    break;
                case 52:
                    switch (FSpellID)
                    {
                        case 104:
                        case 107:
                        case 113:
                        case 116:
                        case 117:
                        case 127:
                        case 129:
                        case 130:
                            Effect.FTarget = null;
                            break;
                        case 102:
                        case 106:
                        case 120:
                        case 128:
                            Effect.FTarget = FUnit;
                            break;
                        case 115:
                            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Random(100) < 75)
                            {
                                Effect.FTarget = null;
                            }

                            break;
                        case 100:
                            ExtraEffect = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.AddEffect(TfxType.fxSFOverlay);
                            var yhx = FUnit.FUnitHS.Yhx;
                            Aow.Game.World.Map.HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx, ref X1, ref Y1);
                            while (ExtraEffect.FPath.Count < Effect.FPath.Count)
                            {
                                Point = new TXYZ();
                                Point.X = X1 + 16;
                                Point.Y = Y1;
                                ExtraEffect.FPath.Add(Point);
                            }

                            ExtraEffect.FAbilityIndex = FAbilityID;
                            ExtraEffect.FSpellIndex = FSpellID;
                            break;
                    }

                    break;
            }
        }

        var yhx10 = FUnit.FUnitHS.Yhx;
        var xhx5 = FUnit.FUnitHS.Xhx;
        Effect.FVariant = Aow.Game.Commands.TCAbRangedTE.GetRangedDirIndex(FTargetX - xhx5, FTargetY - yhx10);
        if (FStrikeIndex == 0)
        {
            switch (FEffectType)
            {
                case TfxType.fxArrow:
                case TfxType.fxRock:
                case TfxType.fxComet:
                case TfxType.fxBreath:
                case TfxType.fxCometBall:
                case TfxType.fxEffect:
                case TfxType.fxRay:
                case TfxType.fxChain:
                case TfxType.fxSwarm:
                case TfxType.fxScreenSwarm:
                case TfxType.fxSFOverlay:
                case TfxType.fxDispel:
                case TfxType.fxAltArrow:
                case TfxType.fxGeyser:
                case TfxType.fxFrostBeam:
                    switch (Effect.FVariant)
                    {
                        case 0:
                        case 1:
                        case 23:
                            FUnit.FUnitHS.SetDirection(1);
                            break;
                        case >= 2 and <= 6:
                            FUnit.FUnitHS.SetDirection(2);
                            break;
                        case >= 7 and <= 10:
                            FUnit.FUnitHS.SetDirection(3);
                            break;
                        case >= 11 and <= 13:
                            FUnit.FUnitHS.SetDirection(4);
                            break;
                        case >= 14 and <= 17:
                            FUnit.FUnitHS.SetDirection(5);
                            break;
                        case >= 18 and <= 22:
                            FUnit.FUnitHS.SetDirection(6);
                            break;
                    }

                    break;
            }
        }

        if (TSet<TfxType>.Of(TfxType.fxRock, TfxType.fxComet, TfxType.fxBreath, TfxType.fxCometBall, TfxType.fxScreenNS).Contains(FEffectType))
        {
            Effect.FVariant = 0;
        }

        if (FEffectType == TfxType.fxBreath && FStrikeIndex == 0)
        {
            FUnit.FUnitHS.SetAction(TTCAction.caBreathAttack);
        }
        else
        {
            if ((FAbilityID != 52 && Aow.Game.Combat.TacticalTables.AbilTypes[FAbilityID] != TfxType.fxBreath) || FStrikeIndex == 0)
            {
                FUnit.FUnitHS.SetAction(TTCAction.caAbility);
            }
        }

        I = 0;
        int limit = Obstacles.Count - 1;
        for (; I <= limit; I += 1)
        {
            (Obstacles[I] as IDisposable)?.Dispose();
        }

        Obstacles?.Dispose();
        FStrikeIndex += 1;
    }

    private static global::Aow.Game.Engine.Buffers.FixedArray_int_19 rndHex = global::Aow.Game.Engine.Buffers.FixedArray_int_19.From([6, 0, 1, 3, 4, 5, 2, 18, 12, 7, 11, 10, 17, 15, 9, 16, 13, 8, 14]);
    private static global::Aow.Game.Engine.Buffers.FixedArray_int_12 BreathHit = global::Aow.Game.Engine.Buffers.FixedArray_int_12.From([0, 1, 4, 7, 8, 2, 5, 9, 10, 3, 6, 11]);

    private static ushort GetRangedDirIndex(int DX, int DY)
    {
        ushort result;
        if (DX == 0)
        {
            result = 0;
        }
        else
        {
            int ratio = Math.Abs(DY * 100 / DX);
            switch (ratio)
            {
                case >= 0 and <= 12:
                    result = 6;
                    break;
                case >= 13 and <= 40:
                    result = 5;
                    break;
                case >= 41 and <= 76:
                    result = 4;
                    break;
                case >= 77 and <= 129:
                    result = 3;
                    break;
                case >= 130 and <= 240:
                    result = 2;
                    break;
                case >= 241 and <= 759:
                    result = 1;
                    break;
                default:
                    result = 0;
                    break;
            }
        }

        if (DY > 0)
        {
            result = (ushort)(12 - result);
        }

        if (DX < 0 && result > 0)
        {
            result = (ushort)(24 - result);
        }

        return result;
    }

    // NetworkE.pas:230

    // NetworkE.pas:230

}

public partial class TTerminateCombatTE : TTokenEvent
{

    public TTerminateCombatTE(TECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:149
    public new const int ClassID = 2236422;

    // CombatTE.pas:152
    public override TExecuteResult Execute(TTokenExecuter TE)
    {
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.DestroyCombat();
        return TExecuteResult.erDone;
    }

    // NetworkE.pas:230

}

public enum TTokenStep : byte
{
    tokStart = 0,
    tokEnd = 1,
    tokFirstStrike = 2,
    tokInitStrike = 3,
    tokExeStrike = 4,
    tokEndStrike = 5,
    tokExeEndStrike = 6,
    tokStepUp = 7,
    tokDelayStrike = 8
}
