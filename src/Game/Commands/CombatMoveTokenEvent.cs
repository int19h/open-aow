// Translated from CombatTE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

public partial class CombatMoveTokenEvent : MoveTokenEvent
{
    public CombatMoveTokenEvent(ECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:206
    public new const int ClassID = 2236416;

    public TacticalBattleUnit FUnit;
    public int FDelay;
    public List<TacticalBattleUnit> FObjects;
    public TacticalBattleUnit FPreviousUnit;
    public TacticalBattleUnit FFreeStrikeUnit;
    public sbyte FNeighbour;
    public TokenStep FStep;
    public StrikeCombatAction FStrike;

    // CombatTE.pas:214
    public override void Dispose()
    {
        if (FObjects != null)
        {
            FObjects?.Clear();
        }

        base.Dispose();
    }

    // CombatTE.pas:209
    public override AoWMapField GetMapF(MapCoordinate Location) => (AoWMapField)CombatMap.Current.MapContainer.GetField(Location.X, Location.Y, 0);

    // CombatTE.pas:234
    public override bool Validate()
    {
        bool Result;
        AoWMapField MapField;
        int Index;
        TacticalBattleUnitHexSprite HS;
        base.Validate();
        Index = FPath.Count - 1;
        MapField = GetMapF(FPath[Index]);
        HS = (TacticalBattleUnitHexSprite)MapField.FindOwnedChild(2228484);
        if (HS != null)
        {
            FObjects = new List<TacticalBattleUnit>();
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
    public override MoveStatus DefaultMove()
    {
        AoWMapField MapField;
        int Direction;
        int X = default;
        int Y = default;
        void CheckFreeStrike()
        {
            int HN = default;
            int NX = default;
            int NY = default;
            TacticalBattleUnitHexSprite HS = default;
            if (FUnit.GetEnabled() && FFreeStrikeUnit == null)
            {
                MapField = (AoWMapField)FUnit.FUnitHS.Owner;
                HN = FNeighbour + 1;
                while (HN <= 6)
                {
                    var yhx = FUnit.FUnitHS.Yhx;
                    HexGeometry.CenterHNtoHX(FUnit.FUnitHS.Xhx, yhx, HN, ref NX, ref NY);
                    HS = (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].GetField(NX, NY).FindOwnedChild(2228484);
                    if (HS != null && FPreviousUnit != HS.FUnit && HS.FUnit.GetEnabled() && !HS.FUnit.GetLocked() && CombatObject.MeleeDamageTypes(HS.FUnit) != Set<DamageType>.Of() && HS.FUnit.GetSide() != FUnit.GetSide() && HS.FAction == TacticalBattleUnitHexSprite.SpriteAction.Move && HS.FUnit.GetAbilityEnabled(1) == FUnit.GetAbilityEnabled(1))
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
                            FStep = TokenStep.Start;
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

        MoveStatus Result = MoveStatus.Moving;
        try
        {
            if (FFreeStrikeUnit != null)
            {
                Result = MoveStatus.Moving;
                switch (FStep)
                {
                    case TokenStep.Start:
                        try
                        {
                            FUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Busy);
                            FFreeStrikeUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepUp);
                            var yhx = FFreeStrikeUnit.FUnitHS.Yhx;
                            var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                            var xhx = FFreeStrikeUnit.FUnitHS.Xhx;
                            var fUnitHSXhx = FUnit.FUnitHS.Xhx;
                            var meleeDirIndex = TacticalTables.GetMeleeDirIndex(FFreeStrikeUnit.FUnitHS.Xhx, xhx - fUnitHSXhx, yhx - fUnitHSYhx);
                            FFreeStrikeUnit.FUnitHS.SetDirection((byte)meleeDirIndex);
                            FStep = TokenStep.InitStrike;
                        }
                        catch
                        {
                            MessageBox(0, "Error during Token Start", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TokenStep.InitStrike:
                        try
                        {
                            if (FFreeStrikeUnit.FUnitHS.FActionDone)
                            {
                                RandSeed = AoWHSMap.Current.Random(16777215);
                                FStrike = CombatObject.CreateStrikeCA(FFreeStrikeUnit, FUnit, true);
                                if (FStrike.FDamage == 0)
                                {
                                    FFreeStrikeUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeMiss);
                                }
                                else
                                {
                                    FFreeStrikeUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.MeleeHit);
                                }

                                FUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Busy);
                                FStep = TokenStep.ExeStrike;
                            }
                        }
                        catch
                        {
                            MessageBox(0, "Error during Token InitStrike", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TokenStep.ExeStrike:
                        try
                        {
                            if (FFreeStrikeUnit.FUnitHS.FActionDone)
                            {
                                if (FStrike.FDamage > 0)
                                {
                                    FUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Hit);
                                    TacticalBattleUnit.MakeHitBlood(FUnit, FStrike.FDamage);
                                }

                                var yhx = FUnit.FUnitHS.Yhx;
                                HexGeometry.HXtoHP(FUnit.FUnitHS.Xhx, yhx, ref X, ref Y);
                                if (FStrike.FDamageTypes.Value - FStrike.FEffects.Value != Set<DamageType>.Of())
                                {
                                    CombatEvents.MakeDEHSList(X, Y, FStrike.FDamageTypes.Value - FStrike.FEffects.Value, false);
                                }

                                if (FStrike.FEffects.Value != Set<DamageType>.Of())
                                {
                                    CombatEvents.MakeDEHSList(X, Y, FStrike.FEffects.Value, true);
                                }

                                FStep = TokenStep.EndStrike;
                            }
                        }
                        catch
                        {
                            MessageBox(0, "Error during Token ExeStrike", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TokenStep.EndStrike:
                        try
                        {
                            if (FUnit.FUnitHS.FActionDone || FStrike.FDamage == 0)
                            {
                                if (FStrike.FDamage > 0)
                                {
                                    CombatMap.Current.FCombat.ExecuteCombatAction(FStrike);
                                }

                                FStep = TokenStep.End;
                                FFreeStrikeUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.StepBack);
                            }
                        }
                        catch
                        {
                            MessageBox(0, "Error during Token EndStrike", "Error - TCMoveToken", 0);
                        }

                        break;
                    case TokenStep.End:
                        try
                        {
                            if (FFreeStrikeUnit.FUnitHS.FActionDone)
                            {
                                if (!FUnit.GetEnabled())
                                {
                                    Result = MoveStatus.Done;
                                }
                                else
                                {
                                    if (FUnit.GetLocked())
                                    {
                                        FUnit.FUnitHS.CancelMove();
                                        Result = MoveStatus.Done;
                                    }
                                    else
                                    {
                                        FUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Move);
                                    }
                                }

                                if (FStrike != null)
                                {
                                    FStrike.Release();
                                    FStrike = null;
                                }

                                FFreeStrikeUnit.FUnitHS.SetAction(TacticalBattleUnitHexSprite.SpriteAction.Move);
                                FStep = TokenStep.Start;
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
                            if (Result == MoveStatus.Moving)
                            {
                                CheckFreeStrike();
                            }
                        }
                        else
                        {
                            Result = MoveStatus.Moving;
                        }
                    }
                    else
                    {
                        if (FMoveIndex != 0)
                        {
                            bool notExplorationEnabled = !CombatMap.Current.FExplorationEnabled;
                            if (!notExplorationEnabled)
                            {
                                var yhx = FUnit.FUnitHS.Yhx;
                                var xhx = FUnit.FUnitHS.Xhx;
                                notExplorationEnabled = ((AoWMapField)CombatMap.Current.MapContainer.GetField(xhx, yhx, 0)).GetExplored(CombatMap.Current.FSeatedPlayer);
                            }

                            if (notExplorationEnabled)
                            {
                                var yhx = FUnit.FUnitHS.Yhx;
                                var xhx = FUnit.FUnitHS.Xhx;
                                CombatMap.Current.MakeVisible(xhx, yhx, 0, 3);
                            }

                            Direction = GetDirection(FMoveIndex);
                            MapField = GetMapF(FPath[FMoveIndex - 1]);
                            if (MapField.GetPlayer() == -1)
                            {
                                if (Direction != 0)
                                {
                                    if (FUnit.FUnitHS.MoveFirst(Direction, false) == -1)
                                    {
                                        Result = MoveStatus.Failed;
                                    }
                                    else
                                    {
                                        Result = MoveStatus.Moving;
                                    }

                                    if (Result == MoveStatus.Moving)
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
                                Result = MoveStatus.Done;
                            }
                        }
                        else
                        {
                            Result = MoveStatus.Done;
                        }
                    }
                }
            }

            if (Result == MoveStatus.Done)
            {
                if (FUnit.GetEnabled() && FUnit.FCombatPlayer == CombatMap.Current.FCurrentCombatPlayer)
                {
                    var xhx = FUnit.FUnitHS.Xhx;
                    CombatMap.Current.FPlayers.GetPlayers(CombatMap.Current.FCurrentCombatPlayer).FLastX = xhx;
                    var yhx = FUnit.FUnitHS.Yhx;
                    CombatMap.Current.FPlayers.GetPlayers(CombatMap.Current.FCurrentCombatPlayer).FLastY = yhx;
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
    public virtual bool Setup(TacticalBattleUnit UnitObject, MovePath Path)
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
    public MoveStatus ExecuteDefaultMove()
    {
        MoveStatus Result;
        AoWMapField NewField;
        AoWMapField OldField;
        Rect R;
        Move();
        OldField = (AoWMapField)FUnit.FUnitHS.Owner;
        NewField = GetMapF(FPath[FMoveIndex]);
        FUnit.FUnitHS.MoveTo(NewField, 0);
        if (NewField.FindChildIndex(FUnit.FUnitHS) != -1)
        {
            Result = ProcessMove();
            var path = FPath[FMoveIndex];
            FUnit.MovedTo(OldField, NewField, path.Flag);
            var rect = new Rect(OldField.Xhx, OldField.Yhx, OldField.Xhx + 1, OldField.Yhx + 1);
            CombatMap.Current.TriggerMapChangedEvent(rect, 0);
            if (FMoveIndex == 0 && NewField.FindChild(2229544) != null && FUnit.FCombatPlayer != CombatMap.Current.FCombat.FCombatData.FDefenderPlayer)
            {
                FUnit.SetStatus(Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Fled));
                var yhx = FUnit.FUnitHS.Yhx;
                var xhx = FUnit.FUnitHS.Xhx;
                var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                R = new Rect(FUnit.FUnitHS.Xhx, fUnitHSYhx, xhx + 1, yhx + 1);
                FUnit.FUnitHS?.Dispose();
                CombatMap.Current.TriggerMapChangedEvent(R, 0);
                FUnit.FUnitHS = null;
                Result = MoveStatus.Done;
            }
            else
            {
                if (CombatMap.Current.FExplorationEnabled)
                {
                    var player = FUnit.GetPlayer();
                    var visibilityRange = FUnit.GetVisibilityRange();
                    var yhx = FUnit.FUnitHS.Yhx;
                    var xhx = FUnit.FUnitHS.Xhx;
                    CombatMap.Current.ExploreArea(xhx, yhx, 0, player, visibilityRange);
                    var fUnitVisibilityRange = FUnit.GetVisibilityRange();
                    var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                    var fUnitHSXhx = FUnit.FUnitHS.Xhx;
                    CombatMap.Current.MapChangedRad(fUnitHSXhx, fUnitHSYhx, 0, fUnitVisibilityRange + 1);
                }
                else
                {
                    var yhx = FUnit.FUnitHS.Yhx;
                    var xhx = FUnit.FUnitHS.Xhx;
                    var fUnitHSYhx = FUnit.FUnitHS.Yhx;
                    var rect2 = new Rect(FUnit.FUnitHS.Xhx, fUnitHSYhx, xhx + 1, yhx + 1);
                    CombatMap.Current.TriggerMapChangedEvent(rect2, 0);
                }

                var fUnitPlayer = FUnit.GetPlayer();
                if (AoWHSMap.Current.FPlayers.GetPlayers(fUnitPlayer).FPlayerType != PlayerType.Human || CombatMap.Current.FPlayers.GetPlayers(FUnit.FCombatPlayer).FAuto)
                {
                    FUnit.FMovePoints = 0;
                }
            }
        }
        else
        {
            FUnit.FUnitHS = null;
            Result = MoveStatus.Done;
        }

        return Result;
    }

    public enum TokenStep : byte
    {
        Start = 0, End = 1, FirstStrike = 2, InitStrike = 3, ExeStrike = 4, EndStrike = 5, ExeEndStrike = 6, StepUp = 7, DelayStrike = 8
    }

    // intf_P5_07.inc:765

}
