// Translated from AoWTC.pas.
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// FSide. Units. Allies. Enemies. Moves. Walls. CurrentMove. FUnitIndex. FUnitLimit. FMinMoves.
// FHeroMoves. FNormalUnits. FIgnoreWalls. FFlee. Status
public partial class CAI : IDisposable
{
    public CombatSideIndex FSide;
    public DelphiList Units;
    public DelphiList Allies;
    public DelphiList Enemies;
    public DelphiList Moves;
    public DelphiList Walls;
    public int CurrentMove;
    public int FUnitIndex;
    public int FUnitLimit;
    public int FMinMoves;
    public int FHeroMoves;
    public int FNormalUnits;
    public bool FIgnoreWalls;
    public bool FFlee;
    public TaiStatus Status;

    // AoWTC.pas:762
    public virtual void Dispose()
    {
        Units?.Dispose();
        Allies?.Dispose();
        Enemies?.Dispose();
        Walls?.Dispose();
        for (int I = 0, last = Moves.Count - 1; I <= last; I++)
        {
            (Moves[I] as IDisposable)?.Dispose();
        }

        Moves?.Dispose();
    }

    // AoWTC.pas:752
    public CAI()
    {
        Units = new DelphiList();
        Allies = new DelphiList();
        Enemies = new DelphiList();
        Moves = new DelphiList();
        Walls = new DelphiList();
    }

    // AoWTC.pas:774
    public void Reset()
    {
        Status = TaiStatus.Idle;
        CurrentMove = 0;
    }

    // AoWTC.pas:780
    public void BeginTurn()
    {
        EvalBattle();
    }

    // AoWTC.pas:785
    public void ContinueTurn()
    {
        EvalBattle();
    }

    // AoWTC.pas:790
    public void CreateMove(TaiMoveType MoveType, int X, int Y, int Value, int Ability, int Spell, TacticalCombatUnit U, CombatObject Target)
    {
        TaiMove M;
        M = new TaiMove();
        M.UnitHS = U.FUnitHS;
        M.Target = Target;
        M.MoveType = MoveType;
        M.Ability = (short)Ability;
        M.Spell = (short)Spell;
        M.X = (ushort)X;
        M.Y = (ushort)Y;
        M.Value = Value;
        Moves.Add(M);
    }

    // AoWTC.pas:805
    public int tcDVtoDEV(int DV1, int DV2, CombatObject Source, CombatObject Target)
    {
        int Strength;
        Strength = Target.GetTargetStrength();
        if (Target is TacticalCombatUnit)
        {
            if (((CombatUnit)Target).FUnit is Hero)
            {
                Strength = Strength * 2;
            }

            if (((CombatUnit)Target).FUnit is Leader)
            {
                Strength = Strength * 2;
            }
        }

        int Result = Strength * DV1;
        Strength = Source.GetTargetStrength();
        if (Source is TacticalCombatUnit)
        {
            if (((CombatUnit)Source).FUnit is Hero)
            {
                Strength = Strength * 2;
            }

            if (((CombatUnit)Source).FUnit is Leader)
            {
                Strength = Strength * 2;
            }
        }

        Result = Result - Strength * DV2;
        return Result / 100;
    }

    // AoWTC.pas:823
    public bool ChargeFromPos(int Position)
    {
        bool Result;
        int J;
        int Empty;
        if (Position == 0)
        {
            Result = false;
            Empty = 0;
            if (FSide == CombatSideIndex.Defender && AoWCombatMap.Current.FCombat.FCombatData.FWallType != 0 && !AoWCombatMap.Current.FCombat.FCombatData.FWallBreached)
            {
                Result = true;
            }
            else
            {
                for (int I = 1; I <= 6; I++)
                {
                    if (AoWCombatMap.Current.FPositionStatus[I] == 1)
                    {
                        J = I - 1;
                        if (J == 0)
                        {
                            J = 6;
                        }

                        if (AoWCombatMap.Current.FPositionStatus[J] == 0)
                        {
                            Result = true;
                        }

                        J = I + 1;
                        if (J == 7)
                        {
                            J = 1;
                        }

                        if (AoWCombatMap.Current.FPositionStatus[J] == 0)
                        {
                            Result = true;
                        }
                    }
                    else
                    {
                        if (AoWCombatMap.Current.FPositionStatus[I] == 0)
                        {
                            Empty += 1;
                        }
                    }
                }

                if (Empty > 1)
                {
                    Result = true;
                }
            }
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // AoWTC.pas:849
    public int UnitSafe(TacticalCombatUnit U, int X, int Y)
    {
        int Result;
        int Distance;
        if (!(U.FUnit is Hero) || (Units.Count == 1 && FSide == CombatSideIndex.Defender))
        {
            Result = 0;
        }
        else
        {
            Result = U.GetTargetStrength() * 20 * U.GetHitPoints() / U.GetHits();
            if (U.FUnit is Leader)
            {
                Result = Result / 2;
            }

            for (int I = 0, last = Enemies.Count - 1; I <= last; I++)
            {
                var yhx = ((TacticalCombatUnit)Enemies[I]).FUnitHS.GetYhx();
                Distance = HexGeometry.dHXtoRad(X, Y, ((TacticalCombatUnit)Enemies[I]).FUnitHS.GetXhx(), yhx);
                if (Distance < 15 && ((CombatObject)Enemies[I]).GetTargetStrength() > 0)
                {
                    Result = Result - ((CombatObject)Enemies[I]).GetTargetStrength() * (15 - Distance);
                }
            }

            for (int I = 0, last = Units.Count - 1; I <= last; I++)
            {
                if ((TacticalCombatUnit)Units[I] != U)
                {
                    var yhx = ((TacticalCombatUnit)Units[I]).FUnitHS.GetYhx();
                    Distance = HexGeometry.dHXtoRad(X, Y, ((TacticalCombatUnit)Units[I]).FUnitHS.GetXhx(), yhx);
                    if (Distance < 15 && ((CombatObject)Units[I]).GetTargetStrength() > 0)
                    {
                        Result = Result + ((CombatObject)Units[I]).GetTargetStrength() * (15 - Distance);
                    }
                }
            }

            for (int I = 0, last = Allies.Count - 1; I <= last; I++)
            {
                var yhx = ((TacticalCombatUnit)Allies[I]).FUnitHS.GetYhx();
                Distance = HexGeometry.dHXtoRad(X, Y, ((TacticalCombatUnit)Allies[I]).FUnitHS.GetXhx(), yhx);
                if (Distance < 15 && ((CombatObject)Allies[I]).GetTargetStrength() > 0)
                {
                    Result = Result + ((CombatObject)Allies[I]).GetTargetStrength() * (15 - Distance);
                }
            }
        }

        return Result;
    }

    // AoWTC.pas:880
    // The tactical AI passes the wall as Ignore. The original casts that object to TTacticalCombatUnit
    // without a check and compares the reference with the sprite's unit. Ignore is a combat object so the
    // wall reaches the same comparison.
    public int MakeTerrainPath(CombatMoveControlSettings MCS, ref XYLList Path, ref int Value, TacticalCombatUnit U, CombatObject Ignore, bool FreeStrikes, bool FindWalls, int Cost)
    {
        int Result;
        int J;
        int X = default;
        int Y = default;
        TacticalCombatUnitHS HS;
        CityWall Wall;
        bool NewWall;
        if (Path.Count < 2)
        {
            Result = -1;
        }
        else
        {
            Result = 0;
        }

        try
        {
            Value = 0;
            if (Result != -1)
            {
                for (int I = 0, last = Path.Count - 1; I <= last; I++)
                {
                    HS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Path.List[I].X, Path.List[I].Y].FindOwnedChild(2228484);
                    if (FreeStrikes && (HS == null || HS == U.FUnitHS && I - 1 >= 0 && AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Path.List[I - 1].X, Path.List[I - 1].Y].FindOwnedChild(2228484) == null))
                    {
                        J = 1;
                        for (; J <= 6; J += 1)
                        {
                            HexGeometry.CenterHNtoHX(Path.List[I].X, Path.List[I].Y, J, ref X, ref Y);
                            HS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2228484);
                            if (HS != null && HS.FUnit != Ignore && HS.FUnit.GetSide() != U.GetSide() && CombatObject.MeleeDamageTypes(HS.FUnit) != Set<DamageType>.Of() && !HS.FUnit.GetLocked() && HS.FUnit.GetAbilityEnabled(1) == U.GetAbilityEnabled(1))
                            {
                                var tcDVtoDEV2 = tcDVtoDEV(CombatObject.StrikeDV(HS.FUnit, U), 0, HS.FUnit, U);
                                Value = Value - tcDVtoDEV2 * 10;
                            }
                        }
                    }

                    if (I < Path.Count - 1)
                    {
                        if (AoWCombatMap.Current.FCombat.FCombatData.FWallType != 0)
                        {
                            var pathY = Path[I];
                            var pathX = Path[I];
                            Wall = (CityWall)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y].FindOwnedChild(2229508);
                            if (Wall == null)
                            {
                                var path = Path[I];
                                var path2 = Path[I];
                                Wall = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path2.X, path.Y].FindOwnedChild(2229510) as CityWall;
                            }
                        }
                        else
                        {
                            Wall = null;
                        }

                        var path3 = Path[I];
                        var path4 = Path[I];
                        var mapField = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path4.X, path3.Y];
                        var path5 = Path[I];
                        var path6 = Path[I];
                        var fieldItem = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path6.X, path5.Y];
                        Path.List[I].Flag = MCS.FMovePointTable[fieldItem.TerrainType][mapField.OverlayType - -1];
                        if (Wall != null)
                        {
                            // { The wall lookup uses the current path point and the no-overlay column. }
                            if (FSide == CombatSideIndex.Attacker && (U.GetAbilityEnabled(65) || U.GetAbilityEnabled(117)) && I == 0)
                            {
                                var pathY = Path[I];
                                var pathX = Path[I];
                                var item = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y];
                                Path.List[I].Flag = MCS.FMovePointTable[item.TerrainType][-1 - -1];
                            }
                            else
                            {
                                if ((FSide == CombatSideIndex.Attacker || AoWCombatMap.Current.FCombat.FCombatData.FWallBreached || FIgnoreWalls) && (Wall.FHitPoints == 0 || (FSide == CombatSideIndex.Defender && Wall is CityWallDoor) || AoWCombatMap.Current.FMoveControl.FUnitHS.FUnit.GetAbilityEnabled(1) || AoWCombatMap.Current.FMoveControl.FUnitHS.FUnit.GetAbilityEnabled(66) || AoWCombatMap.Current.FMoveControl.FUnitHS.FUnit.GetAbilityEnabled(36)))
                                {
                                    var pathY = Path[I];
                                    var pathX = Path[I];
                                    var item2 = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y];
                                    Path.List[I].Flag = MCS.FMovePointTable[item2.TerrainType][-1 - -1];
                                }
                                else
                                {
                                    Path.List[I].Flag = 100;
                                }
                            }

                            if (FindWalls && Wall.FHitPoints > 0)
                            {
                                NewWall = true;
                                J = 0;
                                while (NewWall && J < Walls.Count)
                                {
                                    if (Walls[J] == Wall)
                                    {
                                        NewWall = false;
                                    }
                                    else
                                    {
                                        J += 1;
                                    }
                                }

                                if (NewWall)
                                {
                                    Walls.Add(Wall);
                                }
                            }
                        }

                        var path7 = Path[I];
                        var path8 = Path[I];
                        if (AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path8.X, path7.Y].FindOwnedChild(2228484) != null)
                        {
                            var pathY = Path[I];
                            var pathX = Path[I];
                            var item2 = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y];
                            Path.List[I].Flag = MCS.FMovePointTable[item2.TerrainType][-1 - -1];
                        }
                    }

                    Result = Result + Path.List[I].Flag;
                }
            }
        }
        catch
        {
            MessageBox(0, "Error during Eval Movepath", NullTerminatedText("Error - TCAI " + U.FUnit.GetName()), 0);
        }

        return Result;
    }

    // AoWTC.pas:952
    public int MultDEV(int Value, int Multiplier, int Divider)
    {
        int Result = Value;
        if (Value >= 0)
        {
            try
            {
                if (Multiplier != 1)
                {
                    Result = Result * Multiplier;
                }

                if (Divider != 1 && Divider != 0)
                {
                    Result = Result / Divider;
                }
            }
            catch
            {
                try
                {
                    if (Divider != 1 && Divider != 0)
                    {
                        Result = Result / Divider;
                    }

                    if (Multiplier != 1)
                    {
                        Result = Result * Multiplier;
                    }
                }
                catch
                {
                    Result = 1000000;
                }
            }
        }
        else
        {
            try
            {
                if (Divider != 1)
                {
                    Result = Result * Divider;
                }

                if (Multiplier != 1 && Multiplier != 0)
                {
                    Result = Result / Multiplier;
                }
            }
            catch
            {
                try
                {
                    if (Multiplier != 1 && Multiplier != 0)
                    {
                        Result = Result / Multiplier;
                    }

                    if (Divider != 1)
                    {
                        Result = Result * Divider;
                    }
                }
                catch
                {
                    Result = -1000000;
                }
            }
        }

        if (Result > 1000000)
        {
            Result = 1000000;
        }

        if (Result < -1000000)
        {
            Result = -1000000;
        }

        return Result;
    }

    // AoWTC.pas:980
    public void EvalAll(ref int Value, DelphiList List, CombatObject Ignore, int Spell, TacticalCombatUnit U)
    {
        CombatObject Target;
        AbilityDamageValues DV = default;
        for (int I = 0, last = List.Count - 1; I <= last; I++)
        {
            if ((CombatObject)List[I] != Ignore)
            {
                Target = (CombatObject)List[I];
                AoWHSSet.Current.FSpellControl.GetSpell(Spell).GetCombatDamageValueEx(U, Target, ref DV);
                if (DV.SourceDamage > 0)
                {
                    switch (Spell)
                    {
                        case 102:
                            if (Target.GetSide() != FSide)
                            {
                                var tcDVtoDEV2 = tcDVtoDEV(DV.SourceDamage, 0, U, Target);
                                Value = Value - tcDVtoDEV2;
                            }
                            else
                            {
                                var tcDVtoDEV2 = tcDVtoDEV(DV.SourceDamage, 0, U, Target);
                                Value = Value + tcDVtoDEV2;
                            }

                            break;
                        default:
                            if (Target.GetSide() != FSide)
                            {
                                var hitPoints = Target.GetHitPoints();
                                var hits = Target.GetHits();
                                var multDEV = MultDEV(tcDVtoDEV(DV.SourceDamage, 0, U, Target), hits, hitPoints);
                                Value = Value + multDEV;
                            }
                            else
                            {
                                var hitPoints = Target.GetHitPoints();
                                var hits = Target.GetHits();
                                var multDEV = MultDEV(tcDVtoDEV(DV.SourceDamage, 0, U, Target), hits, hitPoints);
                                Value = Value - multDEV;
                            }

                            break;
                    }
                }
            }
        }
    }

    // AoWTC.pas:1007
    public void EvalHex(CombatObject Ignore, int Ability, int Spell, int X, int Y, ref int Value, bool HitWalls, bool Friendly, TacticalCombatUnit U, object IgnoreWall)
    {
        TacticalCombatUnitHS HS;
        CityWall Wall;
        AbilityDamageValues DV = default;
        HS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2228484);
        if (HS != null && HS.FUnit != Ignore && (Friendly || HS.FUnit.GetSide() != U.GetSide()))
        {
            if (Ability == 52)
            {
                AoWHSSet.Current.FSpellControl.GetSpell(Spell).GetCombatDamageValueEx(U, HS.FUnit, ref DV);
            }
            else
            {
                if (TacticalTables.AbilTypes[Ability] == TfxType.Touch)
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(Ability).tcGetDamageValueEx(U, HS.FUnit, ref DV);
                }
                else
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(Ability).tcGetDamageValueEx(U, HS.FUnit, ref DV);
                }
            }

            if (Ability == 65)
            {
                DV.TargetDamage = 0;
            }

            if (DV.SourceDamage > 0)
            {
                switch (Ability)
                {
                    case 52:
                        switch (Spell)
                        {
                            case 106:
                                if (HS.FUnit.GetSide() != U.GetSide())
                                {
                                    var tcDVtoDEV2 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                    Value = Value + tcDVtoDEV2;
                                }
                                else
                                {
                                    var tcDVtoDEV2 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                    Value = Value - tcDVtoDEV2;
                                }

                                break;
                            default:
                                if (HS.FUnit.GetSide() != U.GetSide())
                                {
                                    var hitPoints = HS.FUnit.GetHitPoints();
                                    var hits = HS.FUnit.GetHits();
                                    var multDEV = MultDEV(tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit), hits, hitPoints);
                                    Value = Value + multDEV;
                                }
                                else
                                {
                                    var hitPoints = HS.FUnit.GetHitPoints();
                                    var hits = HS.FUnit.GetHits();
                                    var multDEV = MultDEV(tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit), hits, hitPoints);
                                    Value = Value - multDEV;
                                }

                                break;
                        }

                        break;
                    default:
                        if (HS.FUnit.GetSide() != U.GetSide())
                        {
                            var hitPoints = HS.FUnit.GetHitPoints();
                            var hits = HS.FUnit.GetHits();
                            var multDEV = MultDEV(tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit), hits, hitPoints);
                            Value = Value + multDEV;
                        }
                        else
                        {
                            var hitPoints = HS.FUnit.GetHitPoints();
                            var hits = HS.FUnit.GetHits();
                            var multDEV = MultDEV(tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit), hits, hitPoints);
                            Value = Value - multDEV;
                        }

                        break;
                }
            }
        }

        if (HitWalls && AoWCombatMap.Current.FCombat.FCombatData.FWallType != 0)
        {
            Wall = (CityWall)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2229508);
            if (Wall == null)
            {
                Wall = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2229510) as CityWall;
            }

            if (!AoWCombatMap.Current.FCombat.FCombatData.FWallBreached && IgnoreWall != null && Wall != null && (Ignore == null || !(Ignore is CombatWall) || (Ignore is CombatWall && IgnoreWall != Wall)))
            {
                if (Ability == 52)
                {
                    AoWHSSet.Current.FSpellControl.GetSpell(Spell).GetCombatDamageValueEx(U, AoWCombatMap.Current.FCombat.FCombatData.FWall, ref DV);
                }
                else
                {
                    if (TacticalTables.AbilTypes[Ability] == TfxType.Touch)
                    {
                        AoWHSSet.Current.FAbilityControl.GetAbility(Ability).tcGetDamageValueEx(U, AoWCombatMap.Current.FCombat.FCombatData.FWall, ref DV);
                    }
                    else
                    {
                        AoWHSSet.Current.FAbilityControl.GetAbility(Ability).tcGetDamageValueEx(U, AoWCombatMap.Current.FCombat.FCombatData.FWall, ref DV);
                    }
                }

                if (Ability == 65)
                {
                    DV.TargetDamage = 0;
                }

                if (DV.SourceDamage > 0)
                {
                    if (U.GetSide() == CombatSideIndex.Attacker)
                    {
                        var multDEV = MultDEV(tcDVtoDEV(DV.SourceDamage, 0, U, AoWCombatMap.Current.FCombat.FCombatData.FWall), TacticalTables.WallMaxHP[Wall.FWallType - 1], Wall.FHitPoints);
                        Value = Value + multDEV;
                    }
                    else
                    {
                        var multDEV = MultDEV(tcDVtoDEV(DV.SourceDamage, 0, U, AoWCombatMap.Current.FCombat.FCombatData.FWall), TacticalTables.WallMaxHP[Wall.FWallType - 1], Wall.FHitPoints);
                        Value = Value - multDEV;
                    }
                }
            }
        }
    }

    // AoWTC.pas:1072
    public void EvalArea(ref int Value, CombatObject Ignore, int Ability, int Spell, int X, int Y, bool HitWalls, bool IncludeCenter, bool Friendly, TacticalCombatUnit U, object IgnoreWall)
    {
        int X1 = default;
        int Y1 = default;
        for (int I = 0; I <= 6; I++)
        {
            HexGeometry.CenterHNtoHX(X, Y, I, ref X1, ref Y1);
            if (IncludeCenter || U.FUnitHS.GetXhx() != X1 || U.FUnitHS.GetYhx() != Y1)
            {
                EvalHex(Ignore, Ability, Spell, X1, Y1, ref Value, HitWalls, Friendly, U, IgnoreWall);
            }
        }
    }

    // AoWTC.pas:1084
    public int EvalPath(ref int Value, CombatObject Ignore, int Ability, int Spell, int X1, int Y1, int X2, int Y2, bool HitWalls, TacticalCombatUnit U, object IgnoreWall)
    {
        int Result;
        XYZList Path;
        DelphiList Obstacles;
        int X = default;
        int Y = default;
        int V = default;
        int Total;
        int Original;
        int Chance;
        TfxType FX;
        if (Ability == 52)
        {
            FX = TacticalTables.SpellTypes[Spell];
        }
        else
        {
            FX = TacticalTables.AbilTypes[Ability];
        }

        switch (FX)
        {
            case TfxType.Arrow:
            case TfxType.Rock:
            case TfxType.Comet:
            case TfxType.Breath:
            case TfxType.CometBall:
            case TfxType.Ray:
            case TfxType.AltArrow:
                Path = new XYZList();
                Obstacles = new DelphiList();
                if (FX == TfxType.Breath)
                {
                    TacticalTables.MakeRangedPath(U.GetSide(), 50, 20, 50, (short)X1, (short)Y1, (short)X2, (short)Y2, 0, 0, false, ref Path, ref Obstacles);
                }
                else
                {
                    TacticalTables.MakeAbilPath(U.GetSide(), (short)Ability, (short)Spell, (short)X1, (short)Y1, (short)X2, (short)Y2, ref Path, ref Obstacles);
                }

                Total = 0;
                Original = Value;
                Chance = 100;
                {
                    for (int I = 0, last = Obstacles.Count - 1; I <= last; I++)
                    {
                        var obstacle = Obstacles[I];
                        var path = Path[((ROb)obstacle).PathIndex];
                        var obstacle2 = Obstacles[I];
                        HexGeometry.HPtoHX(Path[((ROb)obstacle2).PathIndex].X, path.Y, ref X, ref Y);
                        V = 0;
                        EvalHex(Ignore, Ability, Spell, X, Y, ref V, HitWalls, true, U, IgnoreWall);
                        try
                        {
                            var obstacle3 = Obstacles[I];
                            Total = Total + V * Chance / 100 * ((ROb)obstacle3).Chance / 100;
                        }
                        catch
                        {
                            Total = Total + ((ROb)Obstacles[I]).Chance * (V / 100 * Chance / 100);
                        }

                        Chance = (100 - ((ROb)Obstacles[I]).Chance) * Chance / 100;
                    }
                }

                try
                {
                    Value = Value * Chance / 100;
                }
                catch
                {
                    Value = Value / 100 * Chance;
                }

                Value = Value + Total;
                Path?.Dispose();
                {
                    for (int I = 0, last = Obstacles.Count - 1; I <= last; I++)
                    {
                        (Obstacles[I] as IDisposable)?.Dispose();
                    }
                }

                Obstacles?.Dispose();
                Result = Original - Value;
                break;
            default:
                Result = 0;
                break;
        }

        return Result;
    }

    // AoWTC.pas:1130
    public int EvalBreath(ref int Value, CombatObject Ignore, int Ability, int Spell, int X1, int Y1, int X2, int Y2, TacticalCombatUnit U)
    {
        int V = default;
        int X = default;
        int Y = default;
        var xhx = U.FUnitHS.GetXhx();
        var yhx = U.FUnitHS.GetYhx();
        int Result = EvalPath(ref Value, Ignore, Ability, Spell, xhx, yhx, X2, Y2, false, U, null);
        for (int I = 0; I <= 11; I++)
        {
            V = 0;
            var dHXtoHN2 = HexGeometry.dHXtoHN(X1, Y1, X2, Y2);
            HexGeometry.CenterHNtoHX(X1, Y1, TacticalTables.BreathHN[TacticalTables.BreathDirection[dHXtoHN2 - 1] * 12 + I - 12], ref X, ref Y);
            EvalHex(Ignore, Ability, Spell, X, Y, ref V, true, true, U, null);
            if (V != 0)
            {
                var fUnitHSXhx = U.FUnitHS.GetXhx();
                var fUnitHSYhx = U.FUnitHS.GetYhx();
                EvalPath(ref V, Ignore, Ability, Spell, fUnitHSXhx, fUnitHSYhx, X, Y, false, U, null);
            }

            Value = Value + V;
            Result = Result - V;
        }

        return Result;
    }

    // AoWTC.pas:1149
    public void CheckUnit(int X, int Y, int Hits, int HitPoints, CombatMoveControlSettings Settings, TacticalCombatUnit U, CombatObject Target, DelphiList Abilities, object Wall)
    {
        int SafeValue;
        int I;
        int V;
        int Distance;
        int Range;
        int Cost = default;
        int BestIndex;
        int BestValue;
        int D = default;
        int NX = default;
        int NY = default;
        int OriginalX;
        int OriginalY;
        int AX = default;
        int AY = default;
        int PathCost;
        int TerrainValue;
        int PathValue;
        int Ability;
        int Spell;
        int Value = default;
        XYLList Path;
        XYLList DetourPath;
        XYLList BasePath;
        MeleeRound Round;
        AbilityCombatInfo AbilityInfo = default;
        SpellCombatInfo SpellInfo = default;
        CityWall CityWall;
        bool Adjacent;
        AbilityDamageValues Values = default;
        Path = new Movepath();
        BasePath = new Movepath();
        DetourPath = new Movepath();
        TerrainValue = 0;
        OriginalX = X;
        OriginalY = Y;
        var yhx = U.FUnitHS.GetYhx();
        Distance = HexGeometry.dHXtoRad(U.FUnitHS.GetXhx(), yhx, X, Y);
        if (U.FMovePoints > 0)
        {
            AoWCombatMap.Current.FMoveControl.CalculatePath(Settings, (Movepath)BasePath, (sbyte)X, (sbyte)Y, 0);
            PathCost = MakeTerrainPath(Settings, ref BasePath, ref TerrainValue, U, Target, true, Target is TacticalCombatUnit && FSide == CombatSideIndex.Attacker, 1);
            I = 0;
            int last = BasePath.Count - 1;
            for (; I <= last; I += 1)
            {
                Path.Add(BasePath[I]);
            }
        }
        else
        {
            PathCost = Distance * 4;
        }

        if (PathCost != -1)
        {
            try
            {
                if (Target is CombatUnit && Target.GetSide() != U.GetSide() && CombatObject.MeleeDamageTypes(U) != Set<DamageType>.Of() && (U.GetAbilityEnabled(1) || !Target.GetAbilityEnabled(1)))
                {
                    Ability = 0;
                    Spell = 0;
                    Round = new MeleeRound();
                    Round.Calculate(U, Target);
                    if (Round.GetAttackerDV() > 0)
                    {
                        Adjacent = ((Movepath)Path).MovePointsToPos(U.FMovePoints) == 0;
                        U.FCanFlee = false;
                        if (Target is CombatWall)
                        {
                            FFlee = false;
                        }

                        var defenderDV = Round.GetDefenderDV();
                        Value = MultDEV(tcDVtoDEV(Round.GetAttackerDV(), defenderDV, U, Target), HitPoints, Hits) + TerrainValue;
                        if (Target.GetTargetStrength() <= 0 && Enemies.Count > 1)
                        {
                            Value -= 10000;
                        }

                        Cost = (PathCost - U.FMovePoints) * 10;
                        if (Cost < 1)
                        {
                            Cost = 1;
                        }

                        if (!(U.FUnit is Hero))
                        {
                            SafeValue = 0;
                        }
                        else
                        {
                            I = -1;
                            BestIndex = ((Movepath)Path).MovePointsToPos(U.FMovePoints);
                            var pathY = Path[BestIndex];
                            SafeValue = UnitSafe(U, Path[BestIndex].X, pathY.Y);
                            while (BestIndex < Path.Count - 1 && SafeValue < 0)
                            {
                                var path = Path[BestIndex];
                                SafeValue = UnitSafe(U, Path[BestIndex].X, path.Y);
                                if (SafeValue >= 0)
                                {
                                    I = BestIndex;
                                }

                                BestIndex += 1;
                            }

                            if (I > -1)
                            {
                                V = 0;
                                for (; V <= I - 1; V += 1)
                                {
                                    Path.Delete(0);
                                }
                            }
                        }

                        if (Path.Count > 1)
                        {
                            if ((SafeValue >= 0 || FNormalUnits == 0) && Adjacent)
                            {
                                if (Value > 0 || FNormalUnits == 0 || !(U.FUnit is Hero) || Units.Count <= 1)
                                {
                                    FHeroMoves += 1;
                                    if (U.FUnit is Hero)
                                    {
                                        I = 50;
                                    }
                                    else
                                    {
                                        I = 100;
                                    }

                                    CreateMove(TaiMoveType.Melee, X, Y, MultDEV(Value, I, 1), Ability, Spell, U, Target);
                                }
                            }
                            else
                            {
                                if (FIgnoreWalls || ChargeFromPos((int)(((uint)U.FPosition) >> 4)))
                                {
                                    if (!((SafeValue < 0 && FNormalUnits != 0) || U.FMovePoints <= 0 || (Path.List[Path.Count - 2].Flag > U.FMovePoints && Path.List[Path.Count - 2].Flag <= 15)))
                                    {
                                        FHeroMoves += 1;
                                        var moves = U.GetMoves();
                                        var multDEV = MultDEV(Value, moves, Cost);
                                        var path = Path[0];
                                        CreateMove(TaiMoveType.Move, Path[0].X, path.Y, multDEV, Ability, Spell, U, Target);
                                    }
                                    else
                                    {
                                        var moves = U.GetMoves();
                                        CreateMove(TaiMoveType.Fake, X, Y, MultDEV(Value, moves, Cost), Ability, Spell, U, Target);
                                    }
                                }
                            }
                        }
                    }

                    Round?.Dispose();
                }
            }
            catch
            {
                MessageBox(0, "Error during Melee Check", "Error - TCAI ", 0);
            }

            for (int AbilityIndex = 0, last = Abilities.Count - 1; AbilityIndex <= last; AbilityIndex++)
            {
                Path.Clear();
                I = 0;
                int limit = BasePath.Count - 1;
                for (; I <= limit; I += 1)
                {
                    Path.Add(BasePath[I]);
                }

                Ability = ((CombatAbilityRef)Abilities[AbilityIndex]).Ability;
                Spell = 0;
                X = OriginalX;
                Y = OriginalY;
                AoWHSSet.Current.FAbilityControl.GetAbility(Ability).GetCombatInfo(U.FUnit, ref AbilityInfo);
                switch (Ability)
                {
                    case 52:
                        try
                        {
                            if (!(Target is CombatWall) || !AoWCombatMap.Current.FCombat.FCombatData.FWallBreached)
                            {
                                Spell = ((CombatAbilityRef)Abilities[AbilityIndex]).Spell;
                                switch (TacticalTables.SpellTypes[Spell])
                                {
                                    case TfxType.Arrow:
                                    case TfxType.Rock:
                                    case TfxType.Comet:
                                    case TfxType.Breath:
                                    case TfxType.CometBall:
                                    case TfxType.Effect:
                                    case TfxType.ScreenNESW:
                                    case TfxType.ScreenNS:
                                    case TfxType.Ray:
                                    case TfxType.Chain:
                                    case TfxType.Swarm:
                                    case TfxType.ScreenSwarm:
                                    case TfxType.SacredWrath:
                                    case TfxType.SWComet:
                                    case TfxType.Dispel:
                                    case TfxType.Blood:
                                    case TfxType.AltArrow:
                                    case TfxType.Tremors:
                                    case TfxType.HailImpact:
                                    case TfxType.HighPrayer:
                                    case TfxType.MindDecay:
                                    case TfxType.ShockWave:
                                    case TfxType.SacFlame:
                                    case TfxType.Winds:
                                        AoWHSSet.Current.FSpellControl.GetSpell(Spell).GetCombatInfo(U.FUnit, ref SpellInfo);
                                        AoWHSSet.Current.FSpellControl.GetSpell(Spell).GetCombatDamageValueEx(U, Target, ref Values);
                                        if (Values.SourceDamage > 0)
                                        {
                                            U.FCanFlee = false;
                                            if (Target is CombatWall)
                                            {
                                                FFlee = false;
                                            }

                                            PathValue = 0;
                                            Range = SpellInfo.Range;
                                            switch (Spell)
                                            {
                                                case 108:
                                                case 114:
                                                    Range = 4;
                                                    break;
                                            }

                                            if (Range == -1)
                                            {
                                                Range = 100;
                                            }

                                            switch (Spell)
                                            {
                                                case 8:
                                                case >= 15 and <= 17:
                                                case 32:
                                                case 33:
                                                case 36:
                                                case 37:
                                                case 40:
                                                case 42:
                                                case 43:
                                                case 49:
                                                case 56:
                                                case 77:
                                                case 102:
                                                case 106:
                                                case 110:
                                                case 111:
                                                    Value = tcDVtoDEV(Values.SourceDamage, 0, U, Target);
                                                    break;
                                                default:
                                                    Value = MultDEV(tcDVtoDEV(Values.SourceDamage, 0, U, Target), HitPoints, Hits);
                                                    break;
                                            }

                                            if (Target.GetTargetStrength() <= 0 && Enemies.Count > 1)
                                            {
                                                Value -= 10000;
                                            }

                                            if (U.FMovePoints > 0)
                                            {
                                                I = ((Movepath)Path).MovePointsToPos(U.FMovePoints);
                                                var pathY = Path[I];
                                                D = HexGeometry.dHXtoRad(Path[I].X, pathY.Y, X, Y);
                                                if (D <= Range || Distance <= Range)
                                                {
                                                    Cost = Value;
                                                    if (Distance <= Range)
                                                    {
                                                        switch (Spell)
                                                        {
                                                            case 117:
                                                                var xhx = U.FUnitHS.GetXhx();
                                                                var fUnitHSYhx = U.FUnitHS.GetYhx();
                                                                PathValue = EvalPath(ref Value, Target, Ability, Spell, xhx, fUnitHSYhx, X, Y, true, U, Wall);
                                                                EvalArea(ref Value, Target, Ability, Spell, X, Y, true, true, true, U, Wall);
                                                                BestIndex = X;
                                                                BestValue = Y;
                                                                {
                                                                    I = 1;
                                                                    for (; I <= 6; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                        var yhx2 = U.FUnitHS.GetYhx();
                                                                        if (HexGeometry.dHXtoRad(U.FUnitHS.GetXhx(), yhx2, NX, NY) <= Range)
                                                                        {
                                                                            D = Cost;
                                                                            var fUnitHSXhx = U.FUnitHS.GetXhx();
                                                                            var yhx3 = U.FUnitHS.GetYhx();
                                                                            V = EvalPath(ref D, Target, Ability, Spell, fUnitHSXhx, yhx3, NX, NY, true, U, Wall);
                                                                            EvalArea(ref D, Target, Ability, Spell, NX, NY, true, true, true, U, Wall);
                                                                            if (D > Value)
                                                                            {
                                                                                Value = D;
                                                                                PathValue = V;
                                                                                BestIndex = NX;
                                                                                BestValue = NY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                X = BestIndex;
                                                                Y = BestValue;
                                                                break;
                                                            case 127:
                                                                Value = 0;
                                                                Cost = 0;
                                                                {
                                                                    I = 1;
                                                                    for (; I <= 36; I += 1)
                                                                    {
                                                                        var yhx2 = U.FUnitHS.GetYhx();
                                                                        HexGeometry.CenterHNtoHX(U.FUnitHS.GetXhx(), yhx2, I, ref NX, ref NY);
                                                                        EvalHex(null, Ability, Spell, NX, NY, ref Value, true, true, U, Wall);
                                                                    }
                                                                }

                                                                break;
                                                            case 128:
                                                                EvalHex(Target, Ability, Spell, U.FUnitHS.GetXhx(), U.FUnitHS.GetYhx(), ref Value, true, true, U, Wall);
                                                                {
                                                                    I = 0;
                                                                    for (; I <= 18; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                        EvalHex(Target, Ability, Spell, NX, NY, ref Value, true, true, U, Wall);
                                                                    }
                                                                }

                                                                BestIndex = X;
                                                                BestValue = Y;
                                                                {
                                                                    I = 1;
                                                                    for (; I <= 18; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                        var yhx2 = U.FUnitHS.GetYhx();
                                                                        if (HexGeometry.dHXtoRad(U.FUnitHS.GetXhx(), yhx2, NX, NY) <= Range)
                                                                        {
                                                                            D = Cost;
                                                                            EvalHex(Target, Ability, Spell, U.FUnitHS.GetXhx(), U.FUnitHS.GetYhx(), ref D, true, true, U, Wall);
                                                                            V = 0;
                                                                            for (; V <= 18; V += 1)
                                                                            {
                                                                                HexGeometry.CenterHNtoHX(NX, NY, V, ref AX, ref AY);
                                                                                EvalHex(Target, Ability, Spell, AX, AY, ref D, true, true, U, Wall);
                                                                            }

                                                                            if (D > Value)
                                                                            {
                                                                                Value = D;
                                                                                BestIndex = NX;
                                                                                BestValue = NY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                X = BestIndex;
                                                                Y = BestValue;
                                                                break;
                                                            case 100:
                                                            case 103:
                                                            case 119:
                                                            case 122:
                                                            case 123:
                                                                var xhx2 = U.FUnitHS.GetXhx();
                                                                var yhx4 = U.FUnitHS.GetYhx();
                                                                PathValue = EvalPath(ref Value, Target, Ability, Spell, xhx2, yhx4, X, Y, false, U, Wall);
                                                                break;
                                                            case 116:
                                                                EvalArea(ref Value, Target, Ability, Spell, X, Y, true, true, true, U, Wall);
                                                                BestIndex = X;
                                                                BestValue = Y;
                                                                {
                                                                    I = 1;
                                                                    for (; I <= 6; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                        var yhx2 = U.FUnitHS.GetYhx();
                                                                        if (HexGeometry.dHXtoRad(U.FUnitHS.GetXhx(), yhx2, NX, NY) <= Range)
                                                                        {
                                                                            D = Cost;
                                                                            EvalArea(ref D, Target, Ability, Spell, NX, NY, true, true, true, U, Wall);
                                                                            if (D > Value)
                                                                            {
                                                                                Value = D;
                                                                                BestIndex = NX;
                                                                                BestValue = NY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                X = BestIndex;
                                                                Y = BestValue;
                                                                break;
                                                            case 104:
                                                            case 120:
                                                                EvalArea(ref Value, Target, Ability, Spell, X, Y, false, true, true, U, Wall);
                                                                BestIndex = X;
                                                                BestValue = Y;
                                                                {
                                                                    I = 1;
                                                                    for (; I <= 6; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                        var yhx2 = U.FUnitHS.GetYhx();
                                                                        if (HexGeometry.dHXtoRad(U.FUnitHS.GetXhx(), yhx2, NX, NY) <= Range)
                                                                        {
                                                                            D = Cost;
                                                                            EvalArea(ref D, Target, Ability, Spell, NX, NY, false, true, true, U, Wall);
                                                                            if (D > Value)
                                                                            {
                                                                                Value = D;
                                                                                BestIndex = NX;
                                                                                BestValue = NY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                X = BestIndex;
                                                                Y = BestValue;
                                                                break;
                                                            case 106:
                                                                EvalArea(ref Value, Target, Ability, Spell, X, Y, false, false, false, U, Wall);
                                                                BestIndex = X;
                                                                BestValue = Y;
                                                                {
                                                                    I = 1;
                                                                    for (; I <= 6; I += 1)
                                                                    {
                                                                        HexGeometry.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                        var yhx2 = U.FUnitHS.GetYhx();
                                                                        if (HexGeometry.dHXtoRad(U.FUnitHS.GetXhx(), yhx2, NX, NY) <= Range)
                                                                        {
                                                                            D = Cost;
                                                                            EvalArea(ref D, Target, Ability, Spell, NX, NY, false, false, false, U, Wall);
                                                                            if (D > Value)
                                                                            {
                                                                                Value = D;
                                                                                BestIndex = NX;
                                                                                BestValue = NY;
                                                                            }
                                                                        }
                                                                    }
                                                                }

                                                                X = BestIndex;
                                                                Y = BestValue;
                                                                break;
                                                            case 112:
                                                                if (FSide == CombatSideIndex.Defender && AoWCombatMap.Current.FCombat.FCombatData.FWallType != 0)
                                                                {
                                                                    Value = -9999;
                                                                }
                                                                else
                                                                {
                                                                    EvalAll(ref Value, Enemies, Target, Spell, U);
                                                                    EvalAll(ref Value, Allies, Target, Spell, U);
                                                                    EvalAll(ref Value, Units, Target, Spell, U);
                                                                }

                                                                break;
                                                            case 125:
                                                                EvalAll(ref Value, Enemies, Target, Spell, U);
                                                                EvalAll(ref Value, Allies, Target, Spell, U);
                                                                EvalAll(ref Value, Units, Target, Spell, U);
                                                                break;
                                                            case 102:
                                                                EvalAll(ref Value, Units, Target, Spell, U);
                                                                break;
                                                            case 105:
                                                                EvalAll(ref Value, Enemies, Target, Spell, U);
                                                                break;
                                                            case 108:
                                                            case 114:
                                                                var xhx3 = U.FUnitHS.GetXhx();
                                                                var yhx5 = U.FUnitHS.GetYhx();
                                                                PathValue = EvalBreath(ref Value, Target, Ability, Spell, xhx3, yhx5, X, Y, U);
                                                                break;
                                                        }
                                                    }
                                                }

                                                if (D <= Range && (PathValue > 0 || Value > 0))
                                                {
                                                    if (PathValue != 0 && Value < Cost)
                                                    {
                                                        BestIndex = 0;
                                                        BestValue = Value;
                                                        D = Cost;
                                                        I = Path.Count - 2;
                                                        while (I >= 1)
                                                        {
                                                            var path = Path[I];
                                                            var pathX = Path[I];
                                                            CityWall = (CityWall)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[pathX.X, path.Y].FindOwnedChild(2229508);
                                                            if (CityWall == null && FSide == CombatSideIndex.Attacker)
                                                            {
                                                                var path2 = Path[I];
                                                                var path3 = Path[I];
                                                                CityWall = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path3.X, path2.Y].FindOwnedChild(2229510) as CityWall;
                                                            }

                                                            if (!(CityWall == null || CityWall.FHitPoints <= 0 || U.GetAbilityEnabled(1) || U.GetAbilityEnabled(36) || U.GetAbilityEnabled(66)))
                                                            {
                                                                I = 1;
                                                            }
                                                            else
                                                            {
                                                                var path2 = Path[I];
                                                                var path3 = Path[I];
                                                                var path4 = Path[0];
                                                                if (HexGeometry.dHXtoRad(Path[0].X, path4.Y, path3.X, path2.Y) <= Range)
                                                                {
                                                                    var path5 = Path[I];
                                                                    var path6 = Path[I];
                                                                    V = EvalPath(ref Cost, Target, Ability, Spell, path5.X, path6.Y, X, Y, Spell == 117, U, Wall);
                                                                    Cost = D;
                                                                    if (V < PathValue)
                                                                    {
                                                                        BestValue = BestValue + PathValue - V;
                                                                        PathValue = V;
                                                                        BestIndex = I;
                                                                    }
                                                                }
                                                            }

                                                            I -= 1;
                                                        }

                                                        if (BestIndex > 0)
                                                        {
                                                            DetourPath.Clear();
                                                            I = BestIndex;
                                                            int last2 = Path.Count - 1;
                                                            for (; I <= last2; I += 1)
                                                            {
                                                                var path = Path[I];
                                                                DetourPath.Add(path);
                                                            }

                                                            TerrainValue = 0;
                                                            PathCost = MakeTerrainPath(Settings, ref DetourPath, ref TerrainValue, U, Target, true, false, 1);
                                                            PathValue += BestValue;
                                                            PathCost = ((Movepath)Path).PosToMovePoints(0);
                                                        }
                                                    }

                                                    BestIndex = 0;
                                                    I = 1;
                                                    int last3 = Path.Count - 2;
                                                    for (; I <= last3; I += 1)
                                                    {
                                                        var path = Path[I];
                                                        if (HexGeometry.dHXtoRad(X, Y, Path[I].X, path.Y) <= Range && ((Movepath)Path).PosToMovePoints(I) <= U.FMovePoints)
                                                        {
                                                            Cost = 1000;
                                                            var pathX = Path[I];
                                                            var path2 = Path[I];
                                                            V = EvalPath(ref Cost, Target, Ability, Spell, pathX.X, path2.Y, X, Y, Spell == 117, U, Wall);
                                                            if (V == 0 && Cost == 1000)
                                                            {
                                                                BestIndex = I;
                                                            }
                                                        }
                                                    }

                                                    if (BestIndex > 0)
                                                    {
                                                        I = 0;
                                                        for (; I <= BestIndex - 1; I += 1)
                                                        {
                                                            Path.Delete(0);
                                                        }
                                                    }

                                                    PathCost = ((Movepath)Path).PosToMovePoints(0);
                                                }

                                                // { Keep move selection inside the positive movement guard. }
                                                Cost = (PathCost - U.FMovePoints - Range * 4) * 10;
                                                if (Cost < 1)
                                                {
                                                    Cost = 1;
                                                }

                                                if (!(U.FUnit is Hero))
                                                {
                                                    SafeValue = 0;
                                                }
                                                else
                                                {
                                                    I = -1;
                                                    BestIndex = ((Movepath)Path).MovePointsToPos(U.FMovePoints);
                                                    var path = Path[BestIndex];
                                                    SafeValue = UnitSafe(U, Path[BestIndex].X, path.Y);
                                                    while (BestIndex < Path.Count - 1 && SafeValue < 0)
                                                    {
                                                        var path2 = Path[BestIndex];
                                                        SafeValue = UnitSafe(U, Path[BestIndex].X, path2.Y);
                                                        if (SafeValue >= 0)
                                                        {
                                                            I = BestIndex;
                                                        }

                                                        BestIndex += 1;
                                                    }

                                                    if (I > -1)
                                                    {
                                                        V = 0;
                                                        for (; V <= I - 1; V += 1)
                                                        {
                                                            Path.Delete(0);
                                                        }
                                                    }
                                                }

                                                if ((PathValue == 0 || Value != 0) && Distance <= Range && U.GetMoves() == U.FMovePoints)
                                                {
                                                    FHeroMoves += 1;
                                                    I = Math.Abs(Value + PathValue);
                                                    if (I < 1)
                                                    {
                                                        I = 1;
                                                    }

                                                    CreateMove(TaiMoveType.Ranged, X, Y, MultDEV(Value, Value * 100 / I, 1), Ability, Spell, U, Target);
                                                    if (PathValue > 0 && U.FMovePoints > 0 && DetourPath.Count > 1)
                                                    {
                                                        if (!(DetourPath.List[DetourPath.Count - 2].Flag > U.FMovePoints && DetourPath.List[DetourPath.Count - 2].Flag <= 15))
                                                        {
                                                            FHeroMoves += 1;
                                                            var multDEV = MultDEV(PathValue + TerrainValue, 100 * PathValue / I, 1);
                                                            var detourPath = DetourPath[0];
                                                            CreateMove(TaiMoveType.MoveRanged, DetourPath[0].X, detourPath.Y, multDEV, Ability, Spell, U, Target);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (Path.Count > 1 && (FIgnoreWalls || ChargeFromPos((int)(((uint)U.FPosition) >> 4))))
                                                    {
                                                        if (!((SafeValue < 0 && FNormalUnits != 0) || U.FMovePoints <= 0 || Path.Count <= 1 || (Path.List[Path.Count - 2].Flag > U.FMovePoints && Path.List[Path.Count - 2].Flag <= 15)))
                                                        {
                                                            var moves = U.GetMoves();
                                                            var multDEV = MultDEV(PathValue + TerrainValue + Value, moves, Cost);
                                                            var path = Path[0];
                                                            CreateMove(TaiMoveType.MoveRanged, Path[0].X, path.Y, multDEV, Ability, Spell, U, Target);
                                                        }
                                                        else
                                                        {
                                                            var moves = U.GetMoves();
                                                            CreateMove(TaiMoveType.Fake, X, Y, MultDEV(MultDEV(PathValue, 100, 1) + TerrainValue + Value, moves, Cost), Ability, Spell, U, Target);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        break;
                                }
                            }
                        }
                        catch
                        {
                            MessageBox(0, "Error during Spell Check", NullTerminatedText("Error - TCAI " + IntToStr(Ability) + " " + IntToStr(Spell)), 0);
                        }

                        break;
                    default:
                        switch (TacticalTables.AbilTypes[Ability])
                        {
                            case TfxType.Touch:
                                if (Target is CombatWall || Target is CombatUnit && ((TouchAbility)AoWHSSet.Current.FAbilityControl.GetAbility(Ability)).CanTouch(U, Target))
                                {
                                    try
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(Ability).tcGetDamageValueEx(U, Target, ref Values);
                                        if (Values.SourceDamage > 0)
                                        {
                                            Adjacent = ((Movepath)Path).MovePointsToPos(U.FMovePoints) == 0;
                                            U.FCanFlee = false;
                                            if (Target is CombatWall)
                                            {
                                                FFlee = false;
                                            }

                                            if (Ability == 65)
                                            {
                                                Values.TargetDamage = 0;
                                            }

                                            Value = MultDEV(tcDVtoDEV(Values.SourceDamage, Values.TargetDamage, U, Target), HitPoints, Hits) + TerrainValue;
                                            switch (Ability)
                                            {
                                                case >= 27 and <= 29:
                                                case 40:
                                                case 60:
                                                case 106:
                                                case 107:
                                                case 148:
                                                    Value = tcDVtoDEV(Values.SourceDamage, 0, U, Target);
                                                    break;
                                                default:
                                                    Value = MultDEV(tcDVtoDEV(Values.SourceDamage, 0, U, Target), HitPoints, Hits);
                                                    break;
                                            }

                                            if (Ability == 117 && CombatObject.MeleeDamageTypes(U) == Set<DamageType>.Of())
                                            {
                                                Value = MultDEV(Value, 100, 1);
                                            }

                                            if (Target.GetTargetStrength() <= 0 && Enemies.Count > 1)
                                            {
                                                Value -= 10000;
                                            }

                                            if (U.FMovePoints > 0 && ((Movepath)Path).MovePointsToPos(U.FMovePoints) == 0)
                                            {
                                                switch (Ability)
                                                {
                                                    case 65:
                                                        EvalArea(ref Value, Target, Ability, Spell, Path.List[1].X, Path.List[1].Y, true, false, true, U, Wall);
                                                        break;
                                                    case 114:
                                                        EvalArea(ref Value, Target, Ability, Spell, Path.List[1].X, Path.List[1].Y, false, false, true, U, Wall);
                                                        break;
                                                }
                                            }

                                            Cost = (PathCost - U.FMovePoints) * 10;
                                            if (Cost < 1)
                                            {
                                                Cost = 1;
                                            }

                                            if (!(U.FUnit is Hero))
                                            {
                                                SafeValue = 0;
                                            }
                                            else
                                            {
                                                I = -1;
                                                BestIndex = ((Movepath)Path).MovePointsToPos(U.FMovePoints);
                                                var pathY = Path[BestIndex];
                                                SafeValue = UnitSafe(U, Path[BestIndex].X, pathY.Y);
                                                while (BestIndex < Path.Count - 1 && SafeValue < 0)
                                                {
                                                    var path = Path[BestIndex];
                                                    SafeValue = UnitSafe(U, Path[BestIndex].X, path.Y);
                                                    if (SafeValue >= 0)
                                                    {
                                                        I = BestIndex;
                                                    }

                                                    BestIndex += 1;
                                                }

                                                if (I > -1)
                                                {
                                                    V = 0;
                                                    for (; V <= I - 1; V += 1)
                                                    {
                                                        Path.Delete(0);
                                                    }
                                                }
                                            }

                                            if (Path.Count > 1)
                                            {
                                                if ((SafeValue >= 0 || FNormalUnits == 0) && Adjacent)
                                                {
                                                    FHeroMoves += 1;
                                                    if (U.FUnit is Hero)
                                                    {
                                                        I = 1;
                                                    }
                                                    else
                                                    {
                                                        if (Target is CombatWall && AoWCombatMap.Current.FCombat.FCombatData.FWallBreached && AoWCombatMap.Current.FRound > 1)
                                                        {
                                                            I = 10;
                                                        }
                                                        else
                                                        {
                                                            I = 100;
                                                        }
                                                    }

                                                    CreateMove(TaiMoveType.Touch, X, Y, MultDEV(Value, I, 1), Ability, Spell, U, Target);
                                                }
                                                else
                                                {
                                                    if (FIgnoreWalls || ChargeFromPos((int)(((uint)U.FPosition) >> 4)))
                                                    {
                                                        if (!((SafeValue < 0 && FNormalUnits != 0) || U.FMovePoints <= 0 || (Path.List[Path.Count - 2].Flag > U.FMovePoints && Path.List[Path.Count - 2].Flag <= 15)))
                                                        {
                                                            FHeroMoves += 1;
                                                            var moves = U.GetMoves();
                                                            var multDEV = MultDEV(Value, moves, Cost);
                                                            var path = Path[0];
                                                            CreateMove(TaiMoveType.Move, Path[0].X, path.Y, multDEV, Ability, Spell, U, Target);
                                                        }
                                                        else
                                                        {
                                                            var moves = U.GetMoves();
                                                            CreateMove(TaiMoveType.Fake, X, Y, MultDEV(Value, moves, Cost), Ability, Spell, U, Target);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        MessageBox(0, "Error during Touch Check", NullTerminatedText("Error - TCAI " + IntToStr(Ability) + " " + IntToStr(Spell)), 0);
                                    }
                                }

                                break;
                            case TfxType.Arrow:
                            case TfxType.Rock:
                            case TfxType.Comet:
                            case TfxType.Breath:
                            case TfxType.CometBall:
                            case TfxType.Effect:
                            case TfxType.ScreenNESW:
                            case TfxType.ScreenNS:
                            case TfxType.Ray:
                            case TfxType.Chain:
                            case TfxType.Swarm:
                            case TfxType.ScreenSwarm:
                            case TfxType.SacredWrath:
                            case TfxType.SWComet:
                            case TfxType.Dispel:
                            case TfxType.Blood:
                            case TfxType.AltArrow:
                            case TfxType.Tremors:
                            case TfxType.HailImpact:
                            case TfxType.HighPrayer:
                                try
                                {
                                    Cost = AoWHSSet.Current.FAbilityControl.GetAbility(Ability).tcGetDamageValue(U, Target);
                                    if (Cost > 0)
                                    {
                                        U.FCanFlee = false;
                                        if (Target is CombatWall)
                                        {
                                            FFlee = false;
                                        }

                                        PathValue = 0;
                                        if (AoWHSSet.Current.FAbilityControl.GetAbility(Ability).GetCombatMode(U.FUnit) == CombatMode.LongRange)
                                        {
                                            Range = 12;
                                        }
                                        else
                                        {
                                            if (AoWHSSet.Current.FAbilityControl.GetAbility(Ability).GetCombatMode(U.FUnit) == CombatMode.MediumRange)
                                            {
                                                Range = 8;
                                            }
                                            else
                                            {
                                                Range = 4;
                                            }
                                        }

                                        Value = MultDEV(tcDVtoDEV(Cost, 0, U, Target), HitPoints, Hits);
                                        if (Target.GetTargetStrength() <= 0 && Enemies.Count > 1)
                                        {
                                            Value -= 10000;
                                        }

                                        if (U.FMovePoints > 0)
                                        {
                                            I = ((Movepath)Path).MovePointsToPos(U.FMovePoints);
                                            var pathY = Path[I];
                                            D = HexGeometry.dHXtoRad(Path[I].X, pathY.Y, X, Y);
                                            if (D <= Range || Distance <= Range)
                                            {
                                                Cost = Value;
                                                if (Distance <= Range)
                                                {
                                                    switch (Ability)
                                                    {
                                                        case 22:
                                                        case 25:
                                                        case 45:
                                                        case 55:
                                                        case 57:
                                                        case 58:
                                                        case 61:
                                                        case 62:
                                                        case >= 120 and <= 124:
                                                            var xhx = U.FUnitHS.GetXhx();
                                                            var fUnitHSYhx = U.FUnitHS.GetYhx();
                                                            PathValue = EvalPath(ref Value, Target, Ability, Spell, xhx, fUnitHSYhx, X, Y, false, U, Wall);
                                                            break;
                                                        case 24:
                                                        case 44:
                                                            var fUnitHSXhx = U.FUnitHS.GetXhx();
                                                            var yhx2 = U.FUnitHS.GetYhx();
                                                            PathValue = EvalPath(ref Value, Target, Ability, Spell, fUnitHSXhx, yhx2, X, Y, true, U, Wall);
                                                            break;
                                                        case 30:
                                                        case >= 86 and <= 90:
                                                            var xhx2 = U.FUnitHS.GetXhx();
                                                            var yhx3 = U.FUnitHS.GetYhx();
                                                            PathValue = EvalBreath(ref Value, Target, Ability, Spell, xhx2, yhx3, X, Y, U);
                                                            break;
                                                    }
                                                }
                                            }

                                            if (D <= Range && (PathValue > 0 || Value > 0))
                                            {
                                                if (PathValue != 0 && Value < Cost)
                                                {
                                                    BestIndex = 0;
                                                    BestValue = Value;
                                                    D = Cost;
                                                    I = Path.Count - 2;
                                                    while (I >= 1)
                                                    {
                                                        var path = Path[I];
                                                        var pathX = Path[I];
                                                        CityWall = (CityWall)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[pathX.X, path.Y].FindOwnedChild(2229508);
                                                        if (CityWall == null && FSide == CombatSideIndex.Attacker)
                                                        {
                                                            var path2 = Path[I];
                                                            var path3 = Path[I];
                                                            CityWall = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path3.X, path2.Y].FindOwnedChild(2229510) as CityWall;
                                                        }

                                                        if (!(CityWall == null || CityWall.FHitPoints <= 0 || U.GetAbilityEnabled(1) || U.GetAbilityEnabled(36) || U.GetAbilityEnabled(66)))
                                                        {
                                                            I = 1;
                                                        }
                                                        else
                                                        {
                                                            var path2 = Path[I];
                                                            var path3 = Path[I];
                                                            var path4 = Path[0];
                                                            if (HexGeometry.dHXtoRad(Path[0].X, path4.Y, path3.X, path2.Y) <= Range)
                                                            {
                                                                var path5 = Path[I];
                                                                var path6 = Path[I];
                                                                V = EvalPath(ref Cost, Target, Ability, Spell, path5.X, path6.Y, X, Y, Ability == 24 || Ability == 44, U, Wall);
                                                                Cost = D;
                                                                if (V < PathValue)
                                                                {
                                                                    BestValue = BestValue + PathValue - V;
                                                                    PathValue = V;
                                                                    BestIndex = I;
                                                                }
                                                            }
                                                        }

                                                        I -= 1;
                                                    }

                                                    if (BestIndex > 0)
                                                    {
                                                        DetourPath.Clear();
                                                        I = BestIndex;
                                                        int last2 = Path.Count - 1;
                                                        for (; I <= last2; I += 1)
                                                        {
                                                            var path = Path[I];
                                                            DetourPath.Add(path);
                                                        }

                                                        TerrainValue = 0;
                                                        PathCost = MakeTerrainPath(Settings, ref DetourPath, ref TerrainValue, U, Target, true, false, 1);
                                                        PathValue += BestValue;
                                                        PathCost = ((Movepath)Path).PosToMovePoints(0);
                                                    }
                                                }

                                                BestIndex = 0;
                                                I = 1;
                                                int last3 = Path.Count - 2;
                                                for (; I <= last3; I += 1)
                                                {
                                                    var path = Path[I];
                                                    if (HexGeometry.dHXtoRad(X, Y, Path[I].X, path.Y) <= Range && ((Movepath)Path).PosToMovePoints(I) <= U.FMovePoints)
                                                    {
                                                        Cost = 1000;
                                                        var pathX = Path[I];
                                                        var path2 = Path[I];
                                                        V = EvalPath(ref Cost, Target, Ability, Spell, pathX.X, path2.Y, X, Y, Spell == 117, U, Wall);
                                                        if (V == 0 && Cost == 1000)
                                                        {
                                                            BestIndex = I;
                                                        }
                                                    }
                                                }

                                                if (BestIndex > 0)
                                                {
                                                    I = 0;
                                                    for (; I <= BestIndex - 1; I += 1)
                                                    {
                                                        Path.Delete(0);
                                                    }
                                                }

                                                PathCost = ((Movepath)Path).PosToMovePoints(0);
                                            }

                                            // { Keep move selection inside the positive movement guard. }
                                            Cost = (PathCost - U.FMovePoints - Range * 4) * 10;
                                            if (Cost < 1)
                                            {
                                                Cost = 1;
                                            }

                                            if (!(U.FUnit is Hero))
                                            {
                                                SafeValue = 0;
                                            }
                                            else
                                            {
                                                I = -1;
                                                BestIndex = ((Movepath)Path).MovePointsToPos(U.FMovePoints);
                                                var path = Path[BestIndex];
                                                SafeValue = UnitSafe(U, Path[BestIndex].X, path.Y);
                                                while (BestIndex < Path.Count - 1 && SafeValue < 0)
                                                {
                                                    var path2 = Path[BestIndex];
                                                    SafeValue = UnitSafe(U, Path[BestIndex].X, path2.Y);
                                                    if (SafeValue >= 0)
                                                    {
                                                        I = BestIndex;
                                                    }

                                                    BestIndex += 1;
                                                }

                                                if (I > -1)
                                                {
                                                    V = 0;
                                                    for (; V <= I - 1; V += 1)
                                                    {
                                                        Path.Delete(0);
                                                    }
                                                }
                                            }

                                            var yhx4 = U.FUnitHS.GetYhx();
                                            if (!((UnitSafe(U, U.FUnitHS.GetXhx(), yhx4) < 0 && FNormalUnits != 0) || (PathValue != 0 && Value == 0) || Distance > Range || U.GetMoves() != U.FMovePoints))
                                            {
                                                FHeroMoves += 1;
                                                if (Target is CombatWall && AoWCombatMap.Current.FCombat.FCombatData.FWallBreached && AoWCombatMap.Current.FRound > 1)
                                                {
                                                    I = 10;
                                                }
                                                else
                                                {
                                                    I = 100;
                                                }

                                                V = Math.Abs(Value + PathValue);
                                                if (V < 1)
                                                {
                                                    V = 1;
                                                }

                                                CreateMove(TaiMoveType.Ranged, X, Y, MultDEV(Value, I * Value / V, 1), Ability, Spell, U, Target);
                                                if (PathValue > 0 && U.FMovePoints > 0 && DetourPath.Count > 1)
                                                {
                                                    if (!(DetourPath.List[DetourPath.Count - 2].Flag > U.FMovePoints && DetourPath.List[DetourPath.Count - 2].Flag <= 15))
                                                    {
                                                        FHeroMoves += 1;
                                                        var multDEV = MultDEV(PathValue + TerrainValue, I * PathValue / V, 1);
                                                        var detourPath = DetourPath[0];
                                                        CreateMove(TaiMoveType.MoveRanged, DetourPath[0].X, detourPath.Y, multDEV, Ability, Spell, U, Target);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Path.Count > 1 && (FIgnoreWalls || ChargeFromPos((int)(((uint)U.FPosition) >> 4))))
                                                {
                                                    if (!((SafeValue < 0 && FNormalUnits != 0) || U.FMovePoints <= 0 || Path.Count <= 1 || (Path.List[Path.Count - 2].Flag > U.FMovePoints && Path.List[Path.Count - 2].Flag <= 15)))
                                                    {
                                                        var moves = U.GetMoves();
                                                        var multDEV = MultDEV(PathValue + TerrainValue + Value, moves, Cost);
                                                        var path = Path[0];
                                                        CreateMove(TaiMoveType.MoveRanged, Path[0].X, path.Y, multDEV, Ability, Spell, U, Target);
                                                    }
                                                    else
                                                    {
                                                        var moves = U.GetMoves();
                                                        CreateMove(TaiMoveType.Fake, X, Y, MultDEV(MultDEV(PathValue, 100, 1) + TerrainValue + Value, moves, Cost), Ability, Spell, U, Target);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    MessageBox(0, "Error during Ranged Check", NullTerminatedText("Error - TCAI " + IntToStr(Ability) + " " + IntToStr(Spell)), 0);
                                }

                                break;
                        }

                        break;
                }
            }
        }

        Path?.Dispose();
        BasePath?.Dispose();
        DetourPath?.Dispose();
    }

    // AoWTC.pas:1628
    public void EvalBattle()
    {
        int I;
        int J;
        int K = default;
        int L = default;
        int N;
        int Value = default;
        TaiMove Move;
        TaiMove OtherMove;
        TacticalCombatUnit U;
        TacticalCombatUnit Target;
        CombatMoveControlSettings Settings;
        bool Moved;
        bool Found;
        bool Visible;
        DelphiList Abilities;
        int TerrainCost = default;
        int PathCost;
        DelphiList Bonuses;
        string Reason = "";
        XYLList Path = default;
        AbilityCombatInfo AIInfo = default;
        SpellCombatInfo SpellInfo = default;
        CityWall Wall;
        Status = TaiStatus.Busy;
        if (AoWHSMap.Current.FTokenManager.PlayerReady(AoWCombatMap.Current.FCurrentPlayer))
        {
            switch (CurrentMove)
            {
                case 0:
                    try
                    {
                        FUnitIndex = 0;
                        FHeroMoves = 0;
                        Units.Clear();
                        Allies.Clear();
                        Enemies.Clear();
                        I = 0;
                        int last = Moves.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            (Moves[I] as IDisposable)?.Dispose();
                        }

                        Moves.Clear();
                        Walls.Clear();
                        var tacticalCombat = AoWCombatMap.Current.FCombat;
                        FSide = (CombatSideIndex)tacticalCombat.FCombatData.FPlayers.GetPlayers((sbyte)tacticalCombat.FCombatData.FCurrentPlayer).FSideIndex;
                        FNormalUnits = 0;
                        I = 0;
                        int limit = tacticalCombat.FCombatData.GetCount() - 1;
                        for (; I <= limit; I += 1)
                        {
                            if (tacticalCombat.FCombatData.GetObjects(I) != null && tacticalCombat.FCombatData.GetObjects(I) is CombatUnit && tacticalCombat.FCombatData.GetObjects(I).GetEnabled())
                            {
                                Visible = false;
                                J = 0;
                                while (!Visible && J < 19)
                                {
                                    var yhx = ((TacticalCombatUnit)tacticalCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
                                    HexGeometry.CenterHNtoHX(((TacticalCombatUnit)tacticalCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx(), yhx, J, ref K, ref L);
                                    if (!AoWCombatMap.Current.FExplorationEnabled || ((AoWMapField)AoWCombatMap.Current.MapContainer.GetField(K, L, 0)).GetExplored(AoWCombatMap.Current.FSeatedPlayer))
                                    {
                                        Visible = true;
                                    }
                                    else
                                    {
                                        J += 1;
                                    }
                                }

                                if (Visible)
                                {
                                    if (tacticalCombat.FCombatData.GetObjects(I).GetSide() == FSide)
                                    {
                                        if ((byte)tacticalCombat.FCombatData.GetObjects(I).FCombatPlayer == tacticalCombat.FCombatData.FCurrentPlayer)
                                        {
                                            if (!tacticalCombat.FCombatData.GetObjects(I).GetLocked())
                                            {
                                                if (((CombatUnit)tacticalCombat.FCombatData.GetObjects(I)).FUnit is Hero)
                                                {
                                                    var combatObject = tacticalCombat.FCombatData.GetObjects(I);
                                                    Units.Add(combatObject);
                                                }
                                                else
                                                {
                                                    FNormalUnits += 1;
                                                    var objectValue = tacticalCombat.FCombatData.GetObjects(I);
                                                    Units.Insert(0, objectValue);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var objectValue = tacticalCombat.FCombatData.GetObjects(I);
                                            Allies.Add(objectValue);
                                        }
                                    }
                                    else
                                    {
                                        var objectValue = tacticalCombat.FCombatData.GetObjects(I);
                                        Enemies.Add(objectValue);
                                    }
                                }
                            }
                        }

                        GameTables.AoWEngine.OutputDebugStr("AI found " + IntToStr(Units.Count) + " Own units.");
                        GameTables.AoWEngine.OutputDebugStr("AI found " + IntToStr(Allies.Count) + " Ally units.");
                        GameTables.AoWEngine.OutputDebugStr("AI found " + IntToStr(Enemies.Count) + " Enemy units.");
                        FIgnoreWalls = FSide == CombatSideIndex.Attacker;
                        FFlee = FSide == CombatSideIndex.Attacker && AoWCombatMap.Current.FCombat.FCombatData.FWallType != 0 && !AoWCombatMap.Current.FCombat.FCombatData.FWallBreached;
                        FMinMoves = 100;
                        I = 0;
                        int last2 = Units.Count - 1;
                        for (; I <= last2; I += 1)
                        {
                            U = (TacticalCombatUnit)Units[I];
                            U.FCanFlee = FSide == CombatSideIndex.Attacker;
                            if (U.GetMoves() < FMinMoves)
                            {
                                FMinMoves = U.GetMoves();
                            }

                            J = 0;
                            int last3 = Enemies.Count - 1;
                            for (; J <= last3; J += 1)
                            {
                                Target = (TacticalCombatUnit)Enemies[J];
                                if (!FIgnoreWalls && Target.GetSide() != U.GetSide())
                                {
                                    var yhx = Target.FUnitHS.GetYhx();
                                    var xhx = Target.FUnitHS.GetXhx();
                                    var fUnitHSYhx = U.FUnitHS.GetYhx();
                                    K = HexGeometry.dHXtoRad(U.FUnitHS.GetXhx(), fUnitHSYhx, xhx, yhx);
                                    L = 22;
                                    for (; L <= 169; L += 1)
                                    {
                                        if (Target.GetAbilityEnabled(L))
                                        {
                                            if (L == 52 && Target.FUnit is Hero)
                                            {
                                                N = 0;
                                                for (; N <= 130; N += 1)
                                                {
                                                    bool condition = Target.FUnit.FPlayer > 0 && TacticalTables.SpellTypes[N] != TfxType.None && AoWHSMap.Current.FPlayers.GetPlayers(Target.FUnit.FPlayer).FMagicControl.FSpells.IndexOf(N) != -1;
                                                    if (condition)
                                                    {
                                                        var spell = AoWHSSet.Current.FSpellControl.GetSpell(N);
                                                        condition = ((Hero)Target.FUnit).CanCastSpellInstantly(N, spell.FBaseManaCost, ref Reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        AoWHSSet.Current.FSpellControl.GetSpell(N).GetCombatInfo(Target.FUnit, ref SpellInfo);
                                                        if (SpellInfo.TargetTypes.Contains(CombatTargetTypeElement.Enemy) && (K <= SpellInfo.Range || SpellInfo.Range == -1))
                                                        {
                                                            Value = AoWHSSet.Current.FSpellControl.GetSpell(N).GetCombatDamageValue(Target, U);
                                                            if (Value > 0)
                                                            {
                                                                if (Set<TfxType>.Of(TfxType.Arrow, TfxType.Rock, TfxType.Comet, TfxType.Breath, TfxType.CometBall, TfxType.Ray).Contains(TacticalTables.SpellTypes[N]))
                                                                {
                                                                    var fUnitHSXhx = Target.FUnitHS.GetXhx();
                                                                    var yhx2 = Target.FUnitHS.GetYhx();
                                                                    var xhx2 = U.FUnitHS.GetXhx();
                                                                    var yhx3 = U.FUnitHS.GetYhx();
                                                                    EvalPath(ref Value, U, L, N, fUnitHSXhx, yhx2, xhx2, yhx3, false, Target, null);
                                                                    if (Value > 0)
                                                                    {
                                                                        FIgnoreWalls = true;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    FIgnoreWalls = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (TacticalTables.AbilTypes[L] != TfxType.None && TacticalTables.AbilTypes[L] != TfxType.Touch)
                                                {
                                                    AoWHSSet.Current.FAbilityControl.GetAbility(L).GetCombatInfo(Target.FUnit, ref AIInfo);
                                                    if ((((byte)AIInfo.TargetTypes.Image) & 2) != 0)
                                                    {
                                                        if (AoWHSSet.Current.FAbilityControl.GetAbility(L).GetCombatMode(Target.FUnit) == CombatMode.LongRange)
                                                        {
                                                            N = 12;
                                                        }
                                                        else
                                                        {
                                                            if (AoWHSSet.Current.FAbilityControl.GetAbility(L).GetCombatMode(Target.FUnit) == CombatMode.MediumRange)
                                                            {
                                                                N = 8;
                                                            }
                                                            else
                                                            {
                                                                N = 4;
                                                            }
                                                        }

                                                        if (K <= N)
                                                        {
                                                            Value = AoWHSSet.Current.FAbilityControl.GetAbility(L).tcGetDamageValue(Target, U);
                                                            if (Value > 0)
                                                            {
                                                                if (Set<TfxType>.Of(TfxType.Arrow, TfxType.Rock, TfxType.Comet, TfxType.Breath, TfxType.CometBall, TfxType.Ray).Contains(TacticalTables.AbilTypes[L]))
                                                                {
                                                                    var fUnitHSXhx = Target.FUnitHS.GetXhx();
                                                                    var yhx2 = Target.FUnitHS.GetYhx();
                                                                    var xhx2 = U.FUnitHS.GetXhx();
                                                                    var yhx3 = U.FUnitHS.GetYhx();
                                                                    EvalPath(ref Value, U, L, 0, fUnitHSXhx, yhx2, xhx2, yhx3, false, Target, null);
                                                                    if (Value > 0)
                                                                    {
                                                                        FIgnoreWalls = true;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    FIgnoreWalls = true;
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

                        CurrentMove += 1;
                        FUnitIndex = 0;
                        FUnitLimit = 0;
                    }
                    catch
                    {
                        MessageBox(0, "Error - TCAI", "Error during Create Unit List", 0);
                    }

                    break;
                case 1:
                    try
                    {
                        while (FUnitIndex < Units.Count && FUnitLimit < 1)
                        {
                            U = (TacticalCombatUnit)Units[FUnitIndex];
                            if (!U.GetLocked())
                            {
                                Abilities = new DelphiList();
                                try
                                {
                                    J = 0;
                                    for (; J <= 169; J += 1)
                                    {
                                        if (U.GetAbilityEnabled(J))
                                        {
                                            switch (J)
                                            {
                                                case 1:
                                                case 36:
                                                case 66:
                                                case 117:
                                                    FFlee = false;
                                                    break;
                                            }

                                            if (J == 52 && U.FUnit is Hero)
                                            {
                                                K = 0;
                                                for (; K <= 130; K += 1)
                                                {
                                                    bool condition = U.FUnit.FPlayer > 0 && TacticalTables.SpellTypes[K] != TfxType.None && AoWHSMap.Current.FPlayers.GetPlayers(U.FUnit.FPlayer).FMagicControl.FSpells.IndexOf(K) != -1;
                                                    if (condition)
                                                    {
                                                        var spell = AoWHSSet.Current.FSpellControl.GetSpell(K);
                                                        condition = ((Hero)U.FUnit).CanCastSpellInstantly(K, spell.FBaseManaCost, ref Reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        AoWHSSet.Current.FSpellControl.GetSpell(K).GetCombatInfo(U.FUnit, ref SpellInfo);
                                                        if (SpellInfo.TargetTypes.Contains(CombatTargetTypeElement.Enemy))
                                                        {
                                                            Abilities.Add(new CombatAbilityRef());
                                                            ((CombatAbilityRef)Abilities[Abilities.Count - 1]).Ability = J;
                                                            ((CombatAbilityRef)Abilities[Abilities.Count - 1]).Spell = K;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (TacticalTables.AbilTypes[J] != TfxType.None)
                                                {
                                                    AoWHSSet.Current.FAbilityControl.GetAbility(J).GetCombatInfo(U.FUnit, ref AIInfo);
                                                    if ((((byte)AIInfo.TargetTypes.Image) & 2) != 0)
                                                    {
                                                        Abilities.Add(new CombatAbilityRef());
                                                        ((CombatAbilityRef)Abilities[Abilities.Count - 1]).Ability = J;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    MessageBox(0, "Error during Eval Enemies", NullTerminatedText("Error - TCAI " + U.FUnit.GetName()), 0);
                                }

                                Settings = new CombatMoveControlSettings(null);
                                if (U.FMovePoints > 0)
                                {
                                    var yhx = U.FUnitHS.GetYhx();
                                    Settings.SetSourcePos(U.FUnitHS.GetXhx(), yhx, 0);
                                    Settings.FArmyHS = U.FUnitHS;
                                    Settings.FMoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee, CombatMoveModeElement.Move, CombatMoveModeElement.Wall);
                                    U.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                    Settings.FPlayer = U.GetPlayer();
                                    Settings.FMovePointsMax = 200;
                                    AoWCombatMap.Current.FMoveControl.ProcessMove(Settings);
                                }

                                if (Abilities.Count > 0 || CombatObject.MeleeDamageTypes(U) != Set<DamageType>.Of())
                                {
                                    J = 0;
                                    int last = Enemies.Count - 1;
                                    for (; J <= last; J += 1)
                                    {
                                        Target = (TacticalCombatUnit)Enemies[J];
                                        if (!(U.FUnit is Hero) || FHeroMoves == 0)
                                        {
                                            var hits = Target.GetHits();
                                            var hitPoints = Target.GetHitPoints();
                                            var yhx = Target.FUnitHS.GetYhx();
                                            CheckUnit(Target.FUnitHS.GetXhx(), yhx, hitPoints, hits, Settings, U, Target, Abilities, null);
                                        }
                                    }
                                }

                                I = 0;
                                int limit = Abilities.Count - 1;
                                for (; I <= limit; I += 1)
                                {
                                    (Abilities[I] as IDisposable)?.Dispose();
                                }

                                Abilities.Clear();
                                try
                                {
                                    J = 22;
                                    for (; J <= 169; J += 1)
                                    {
                                        if (U.GetAbilityEnabled(J))
                                        {
                                            if (J == 52 && U.FUnit is Hero)
                                            {
                                                K = 0;
                                                for (; K <= 130; K += 1)
                                                {
                                                    bool condition = TacticalTables.SpellTypes[K] != TfxType.None && AoWHSMap.Current.FPlayers.GetPlayers(U.FUnit.FPlayer).FMagicControl.FSpells.IndexOf(K) != -1;
                                                    if (condition)
                                                    {
                                                        var spell = AoWHSSet.Current.FSpellControl.GetSpell(K);
                                                        condition = ((Hero)U.FUnit).CanCastSpellInstantly(K, spell.FBaseManaCost, ref Reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        AoWHSSet.Current.FSpellControl.GetSpell(K).GetCombatInfo(U.FUnit, ref SpellInfo);
                                                        if (SpellInfo.TargetTypes.Contains(CombatTargetTypeElement.Friend))
                                                        {
                                                            Abilities.Add(new CombatAbilityRef());
                                                            ((CombatAbilityRef)Abilities[Abilities.Count - 1]).Ability = J;
                                                            ((CombatAbilityRef)Abilities[Abilities.Count - 1]).Spell = K;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (TacticalTables.AbilTypes[J] != TfxType.None)
                                                {
                                                    AoWHSSet.Current.FAbilityControl.GetAbility(J).GetCombatInfo(U.FUnit, ref AIInfo);
                                                    if ((((byte)AIInfo.TargetTypes.Image) & 1) != 0)
                                                    {
                                                        Abilities.Add(new CombatAbilityRef());
                                                        ((CombatAbilityRef)Abilities[Abilities.Count - 1]).Ability = J;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    MessageBox(0, "Error during Eval Friends", NullTerminatedText("Error - TCAI " + U.FUnit.GetName()), 0);
                                }

                                if (Abilities.Count > 0)
                                {
                                    J = 0;
                                    int last = Units.Count - 1;
                                    for (; J <= last; J += 1)
                                    {
                                        Target = (TacticalCombatUnit)Units[J];
                                        if (!(U.FUnit is Hero) || FHeroMoves == 0)
                                        {
                                            var hits = Target.GetHits();
                                            var hitPoints = Target.GetHitPoints();
                                            var yhx = Target.FUnitHS.GetYhx();
                                            CheckUnit(Target.FUnitHS.GetXhx(), yhx, hitPoints, hits, Settings, U, Target, Abilities, null);
                                        }
                                    }
                                }

                                Settings?.Dispose();
                                I = 0;
                                int last2 = Abilities.Count - 1;
                                for (; I <= last2; I += 1)
                                {
                                    (Abilities[I] as IDisposable)?.Dispose();
                                }

                                Abilities?.Dispose();
                            }

                            FUnitIndex += 1;
                            FUnitLimit += 1;
                        }

                        if (FUnitIndex >= Units.Count)
                        {
                            CurrentMove += 1;
                            try
                            {
                                if (AoWCombatMap.Current.FCombat.FCombatData.FWallType != 0 && AoWCombatMap.Current.FCombat.FCombatData.FWall != null && FSide == CombatSideIndex.Attacker)
                                {
                                    J = 0;
                                    int last = AoWCombatMap.Current.FWalls.Count - 1;
                                    for (; J <= last; J += 1)
                                    {
                                        bool condition = ((CityWall)AoWCombatMap.Current.FWalls[J]).FHitPoints > 0;
                                        if (condition)
                                        {
                                            var wall = AoWCombatMap.Current.FWalls[J];
                                            var wall2 = AoWCombatMap.Current.FWalls[J];
                                            condition = ((CityWall)wall).FHitPoints < TacticalTables.WallMaxHP[((CityWall)wall2).FWallType - 1];
                                        }

                                        if (condition)
                                        {
                                            Found = true;
                                            K = 0;
                                            while (Found && K < Walls.Count)
                                            {
                                                if (Walls[K] == AoWCombatMap.Current.FWalls[J])
                                                {
                                                    Found = false;
                                                }
                                                else
                                                {
                                                    K += 1;
                                                }
                                            }

                                            if (Found)
                                            {
                                                var wall = AoWCombatMap.Current.FWalls[J];
                                                Walls.Add(wall);
                                            }
                                        }
                                    }
                                }

                                if (AoWCombatMap.Current.FCombat.FCombatData.FWallType == 0 || AoWCombatMap.Current.FCombat.FCombatData.FWall == null || FSide == CombatSideIndex.Defender)
                                {
                                    CurrentMove += 1;
                                }

                                FUnitIndex = 0;
                                GameTables.AoWEngine.OutputDebugStr("AI calculated " + IntToStr(Units.Count * Enemies.Count) + " Attack paths.");
                                GameTables.AoWEngine.OutputDebugStr("AI calculated " + IntToStr(Moves.Count) + " Moves.");
                            }
                            catch
                            {
                                MessageBox(0, "Error - TCAI ", "Error during Eval Ending", 0);
                            }
                        }

                        FUnitLimit = 0;
                    }
                    catch
                    {
                        MessageBox(0, "Error - TCAI", "Error during Create Move List", 0);
                    }

                    break;
                case 2:
                    try
                    {
                        while (FUnitIndex < Units.Count && FUnitLimit < 1)
                        {
                            U = (TacticalCombatUnit)Units[FUnitIndex];
                            if (!U.GetLocked())
                            {
                                Abilities = new DelphiList();
                                J = 0;
                                for (; J <= 169; J += 1)
                                {
                                    if (U.GetAbilityEnabled(J))
                                    {
                                        switch (J)
                                        {
                                            case 1:
                                            case 36:
                                            case 66:
                                            case 117:
                                                FFlee = false;
                                                break;
                                        }

                                        if (J == 52 && U.FUnit is Hero)
                                        {
                                            if (U.FUnit.FPlayer > 0)
                                            {
                                                K = 0;
                                                for (; K <= 130; K += 1)
                                                {
                                                    bool condition = TacticalTables.SpellTypes[K] != TfxType.None && AoWHSMap.Current.FPlayers.GetPlayers(U.FUnit.FPlayer).FMagicControl.FSpells.IndexOf(K) != -1;
                                                    if (condition)
                                                    {
                                                        var spell = AoWHSSet.Current.FSpellControl.GetSpell(K);
                                                        condition = ((Hero)U.FUnit).CanCastSpellInstantly(K, spell.FBaseManaCost, ref Reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        AoWHSSet.Current.FSpellControl.GetSpell(K).GetCombatInfo(U.FUnit, ref SpellInfo);
                                                        if (SpellInfo.DamageTypes.Contains(DamageType.Wall))
                                                        {
                                                            FFlee = false;
                                                            Abilities.Add(new CombatAbilityRef());
                                                            ((CombatAbilityRef)Abilities[Abilities.Count - 1]).Ability = J;
                                                            ((CombatAbilityRef)Abilities[Abilities.Count - 1]).Spell = K;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (TacticalTables.AbilTypes[J] != TfxType.None)
                                            {
                                                AoWHSSet.Current.FAbilityControl.GetAbility(J).GetCombatInfo(U.FUnit, ref AIInfo);
                                                if (AIInfo.DamageTypes.Contains(DamageType.Wall))
                                                {
                                                    FFlee = false;
                                                    Abilities.Add(new CombatAbilityRef());
                                                    ((CombatAbilityRef)Abilities[Abilities.Count - 1]).Ability = J;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (Abilities.Count > 0)
                                {
                                    Settings = new CombatMoveControlSettings(null);
                                    if (U.FMovePoints > 0)
                                    {
                                        var yhx = U.FUnitHS.GetYhx();
                                        Settings.SetSourcePos(U.FUnitHS.GetXhx(), yhx, 0);
                                        Settings.FArmyHS = U.FUnitHS;
                                        Settings.FMoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee, CombatMoveModeElement.Move, CombatMoveModeElement.Wall);
                                        if (U.GetAbilityEnabled(65) || U.GetAbilityEnabled(117))
                                        {
                                            Settings.FMoveFlags = Settings.FMoveFlags + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.WallCrushing);
                                        }

                                        U.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                        Settings.FPlayer = U.GetPlayer();
                                        Settings.FMovePointsMax = 200;
                                        AoWCombatMap.Current.FMoveControl.ProcessMove(Settings);
                                    }

                                    J = 0;
                                    int last = Walls.Count - 1;
                                    for (; J <= last; J += 1)
                                    {
                                        Wall = (CityWall)Walls[J];
                                        if (!(U.FUnit is Hero) || FHeroMoves == 0)
                                        {
                                            var yhx = Wall.GetYhx();
                                            CheckUnit(Wall.GetXhx(), yhx, Wall.FHitPoints, TacticalTables.WallMaxHP[Wall.FWallType - 1] * 2, Settings, U, AoWCombatMap.Current.FCombat.FCombatData.FWall, Abilities, Wall);
                                        }
                                    }

                                    if (Walls.Count == 0)
                                    {
                                        L = 100;
                                        Wall = null;
                                        J = 0;
                                        int limit = AoWCombatMap.Current.FWalls.Count - 1;
                                        for (; J <= limit; J += 1)
                                        {
                                            var yhx = U.FUnitHS.GetYhx();
                                            var yhx2 = ((HexagonSprite)AoWCombatMap.Current.FWalls[J]).GetYhx();
                                            N = HexGeometry.dHXtoRad(((HexagonSprite)AoWCombatMap.Current.FWalls[J]).GetXhx(), yhx2, U.FUnitHS.GetXhx(), yhx);
                                            if (((CityWall)AoWCombatMap.Current.FWalls[J]).FHitPoints > 0 && N < L)
                                            {
                                                Wall = (CityWall)AoWCombatMap.Current.FWalls[J];
                                                L = N;
                                            }
                                        }

                                        if (Wall != null)
                                        {
                                            var yhx = Wall.GetYhx();
                                            CheckUnit(Wall.GetXhx(), yhx, Wall.FHitPoints * 2, TacticalTables.WallMaxHP[Wall.FWallType - 1], Settings, U, AoWCombatMap.Current.FCombat.FCombatData.FWall, Abilities, Wall);
                                        }
                                    }

                                    Settings?.Dispose();
                                    I = 0;
                                    int last2 = Abilities.Count - 1;
                                    for (; I <= last2; I += 1)
                                    {
                                        (Abilities[I] as IDisposable)?.Dispose();
                                    }
                                }

                                Abilities?.Dispose();
                            }

                            FUnitIndex += 1;
                            FUnitLimit += 1;
                        }

                        if (FUnitIndex >= Units.Count)
                        {
                            CurrentMove += 1;
                            Walls.Clear();
                            FUnitIndex = 0;
                            GameTables.AoWEngine.OutputDebugStr("AI calculated " + IntToStr(Units.Count * Enemies.Count) + " Attack paths.");
                            GameTables.AoWEngine.OutputDebugStr("AI calculated " + IntToStr(Moves.Count) + " Moves.");
                        }

                        FUnitLimit = 0;
                    }
                    catch
                    {
                        MessageBox(0, "Error - TCAI", "Error during Create WallCrush List", 0);
                    }

                    break;
                case 3:
                    try
                    {
                        Bonuses = new DelphiList();
                        I = 0;
                        int last = Units.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            Bonuses.Add(null);
                        }

                        while (FUnitIndex < Moves.Count && FUnitLimit < 50)
                        {
                            Move = (TaiMove)Moves[FUnitIndex];
                            if (Move.MoveType != TaiMoveType.Fake)
                            {
                                J = 0;
                                int limit = Units.Count - 1;
                                for (; J <= limit; J += 1)
                                {
                                    Bonuses[J] = null;
                                }

                                J = 0;
                                int last2 = Moves.Count - 1;
                                for (; J <= last2; J += 1)
                                {
                                    OtherMove = (TaiMove)Moves[J];
                                    if (Move.UnitHS.FUnit != OtherMove.UnitHS.FUnit)
                                    {
                                        K = HexGeometry.dHXtoRad(Move.X, Move.Y, OtherMove.X, OtherMove.Y) * 4 - OtherMove.UnitHS.FUnit.FMovePoints;
                                        if (K < 4)
                                        {
                                            K = 4;
                                        }

                                        L = OtherMove.UnitHS.FUnit.GetMoves() * (OtherMove.Value / (K * 10));
                                        K = Units.IndexOf(OtherMove.UnitHS.FUnit);
                                        if (ObjectToInt((object)Bonuses[K]) < L)
                                        {
                                            Bonuses[K] = IntToObject(L);
                                        }
                                    }
                                }

                                J = 0;
                                int last3 = Units.Count - 1;
                                for (; J <= last3; J += 1)
                                {
                                    if (Move.GroupValue > -1000000 && Move.GroupValue < 1000000)
                                    {
                                        var bonuse = Bonuses[J];
                                        Move.GroupValue += ObjectToInt((object)bonuse);
                                    }
                                }
                            }

                            FUnitIndex += 1;
                            FUnitLimit += 1;
                        }

                        if (FUnitIndex >= Moves.Count)
                        {
                            CurrentMove += 1;
                        }

                        FUnitLimit = 0;
                        Bonuses?.Dispose();
                    }
                    catch
                    {
                        MessageBox(0, "Error - TCAI", "Error during Build Group Bonuses", 0);
                    }

                    break;
                case 4:
                    try
                    {
                        I = 0;
                        int last = Units.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            U = (TacticalCombatUnit)Units[I];
                            if (U.FUnit is Hero && FHeroMoves == 0 && U.FMovePoints > 0)
                            {
                                var yhx = U.FUnitHS.GetYhx();
                                K = UnitSafe(U, U.FUnitHS.GetXhx(), yhx);
                                Value = K;
                                N = -1;
                                J = 0;
                                int limit = Units.Count - 1;
                                for (; J <= limit; J += 1)
                                {
                                    Target = (TacticalCombatUnit)Units[J];
                                    if (U != Target)
                                    {
                                        var fUnitHSYhx = Target.FUnitHS.GetYhx();
                                        L = UnitSafe(U, Target.FUnitHS.GetXhx(), fUnitHSYhx);
                                        if (L > K)
                                        {
                                            N = J;
                                            K = L;
                                        }
                                    }
                                }

                                if (K > Value)
                                {
                                    Settings = new CombatMoveControlSettings(null);
                                    // { Original uses X for both coordinates. }
                                    var xhx = U.FUnitHS.GetXhx();
                                    Settings.SetSourcePos(U.FUnitHS.GetXhx(), xhx, 0);
                                    var fUnitHSYhx = ((TacticalCombatUnit)Units[N]).FUnitHS.GetYhx();
                                    Settings.SetDestinationPos(((TacticalCombatUnit)Units[N]).FUnitHS.GetXhx(), fUnitHSYhx, 0);
                                    Settings.FArmyHS = U.FUnitHS;
                                    Settings.FMoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee, CombatMoveModeElement.Move, CombatMoveModeElement.Wall);
                                    U.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                    Settings.FPlayer = U.GetPlayer();
                                    AoWCombatMap.Current.FMoveControl.ProcessMove(Settings);
                                    if (Settings.FPath.Count > 2)
                                    {
                                        Settings.FPath.Delete(0);
                                        var path = Settings.FPath[0];
                                        CreateMove(TaiMoveType.Move, Settings.FPath[0].X, path.Y, K, 0, 0, U, null);
                                    }

                                    Settings?.Dispose();
                                }
                                else
                                {
                                    if (FSide == CombatSideIndex.Attacker)
                                    {
                                        U.FCanFlee = true;
                                    }
                                }
                            }
                        }

                        if (FSide == CombatSideIndex.Attacker)
                        {
                            I = 0;
                            int limit = Units.Count - 1;
                            for (; I <= limit; I += 1)
                            {
                                U = (TacticalCombatUnit)Units[I];
                                if ((!(U.FUnit is Hero) || FHeroMoves == 0 || (U.FUnit is Hero && Units.Count == 1)) && U.GetEnabled() && !U.GetLocked() && U.FMovePoints > 0 && (FFlee || U.FCanFlee))
                                {
                                    J = 0;
                                    int last2 = AoWCombatMap.Current.FFleeLocations.Count - 1;
                                    for (; J <= last2; J += 1)
                                    {
                                        var yhx = U.FUnitHS.GetYhx();
                                        var yhx2 = ((HexagonSprite)AoWCombatMap.Current.FFleeLocations[J]).GetYhx();
                                        var dHXtoRad2 = HexGeometry.dHXtoRad(((HexagonSprite)AoWCombatMap.Current.FFleeLocations[J]).GetXhx(), yhx2, U.FUnitHS.GetXhx(), yhx);
                                        var yhx3 = ((HexagonSprite)AoWCombatMap.Current.FFleeLocations[J]).GetYhx();
                                        CreateMove(TaiMoveType.Move, ((HexagonSprite)AoWCombatMap.Current.FFleeLocations[J]).GetXhx(), yhx3, 110000000 - dHXtoRad2 * 10, 0, 0, U, null);
                                    }
                                }
                            }
                        }

                        I = 0;
                        while (I < Moves.Count)
                        {
                            Move = (TaiMove)Moves[I];
                            switch (Move.MoveType)
                            {
                                case TaiMoveType.Fake:
                                    Moves.Delete(I);
                                    break;
                                default:
                                    {
                                        if (Move.Value > -100000000 && Move.Value < 100000000)
                                        {
                                            Move.Value += Move.GroupValue / 4;
                                        }

                                        I += 1;
                                    }

                                    break;
                            }
                        }

                        I = 1;
                        int last3 = Moves.Count - 1;
                        for (; I <= last3; I += 1)
                        {
                            Move = (TaiMove)Moves[I];
                            J = 0;
                            K = -1;
                            while (J < I)
                            {
                                OtherMove = (TaiMove)Moves[J];
                                if (Move.Value > OtherMove.Value)
                                {
                                    K = J;
                                    J = I;
                                }
                                else
                                {
                                    J += 1;
                                }
                            }

                            if (K > -1)
                            {
                                Moves.Move(I, K);
                            }
                        }

                        CurrentMove += 1;
                    }
                    catch
                    {
                        MessageBox(0, "Error - TCAI", "Error during Add Group Bonuses", 0);
                    }

                    break;
                case 5:
                    try
                    {
                        Moved = false;
                        while (Moves.Count > 0 && !Moved)
                        {
                            Move = (TaiMove)Moves[0];
                            switch (Move.MoveType)
                            {
                                case TaiMoveType.Move:
                                case TaiMoveType.MoveRanged:
                                    {
                                        var tacticalCombatUnitHS = Move.UnitHS;
                                        if ((Move.UnitHS.FUnit.FUnit is Hero || (FSide == CombatSideIndex.Attacker && AoWCombatMap.Current.FRound == 1)) && Move.UnitHS.FUnit.FMovePoints > FMinMoves)
                                        {
                                            Move.UnitHS.FUnit.FMovePoints = FMinMoves;
                                        }

                                        Settings = new CombatMoveControlSettings(null);
                                        var lhx = tacticalCombatUnitHS.GetLhx();
                                        var yhx = tacticalCombatUnitHS.GetYhx();
                                        Settings.SetSourcePos(tacticalCombatUnitHS.GetXhx(), yhx, lhx);
                                        Settings.SetDestinationPos(Move.X, Move.Y, 0);
                                        Settings.FArmyHS = Move.UnitHS;
                                        if (Move.MoveType == TaiMoveType.MoveRanged || CombatObject.MeleeDamageTypes(Move.UnitHS.FUnit) == Set<DamageType>.Of())
                                        {
                                            Settings.FMoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.MoveRanged);
                                        }
                                        else
                                        {
                                            Settings.FMoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee);
                                        }

                                        if (AoWCombatMap.Current.FCombat.FCombatData.FWallType != 0)
                                        {
                                            if (!(AoWCombatMap.Current.FCombat.FCombatData.FWallBreached && AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Move.X, Move.Y].FindOwnedChild(2229508) == null && AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Move.X, Move.Y].FindOwnedChild(2229510) == null))
                                            {
                                                if (!FIgnoreWalls || !ChargeFromPos((int)(((uint)Move.UnitHS.FUnit.FPosition) >> 4)) || FSide == CombatSideIndex.Attacker)
                                                {
                                                    Settings.FMoveFlags = Settings.FMoveFlags + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Wall);
                                                }
                                            }
                                        }

                                        Move.UnitHS.FUnit.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                        Settings.FPlayer = Move.UnitHS.FUnit.GetPlayer();
                                        AoWCombatMap.Current.FMoveControl.ProcessMove(Settings);
                                        if (Settings.FPath.Count < 2 && !Settings.FMoveFlags.Contains(CombatMoveModeElement.Wall))
                                        {
                                            Settings.FMoveFlags = Settings.FMoveFlags + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Wall);
                                            AoWCombatMap.Current.FMoveControl.ProcessMove(Settings);
                                        }

                                        Wall = (CityWall)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Move.X, Move.Y].FindOwnedChild(2229508);
                                        if (Wall == null)
                                        {
                                            Wall = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Move.X, Move.Y].FindOwnedChild(2229510) as CityWall;
                                        }

                                        if (Settings.FPath.Count == 2 && Move.MoveType == TaiMoveType.MoveRanged && Wall != null)
                                        {
                                            Move.UnitHS.FUnit.FMovePoints = 0;
                                        }
                                        else
                                        {
                                            if (Settings.FPath.Count > 1)
                                            {
                                                Path = Settings.FPath;
                                                PathCost = MakeTerrainPath(Settings, ref Path, ref TerrainCost, Move.UnitHS.FUnit, null, false, false, 0);
                                                if (Path.Count > 1 && PathCost > -1)
                                                {
                                                    if (Path[Path.Count - 2].Flag > 0 && Path[Path.Count - 2].Flag <= Move.UnitHS.FUnit.FMovePoints)
                                                    {
                                                        if (Move.MoveType == TaiMoveType.MoveRanged)
                                                        {
                                                            Value = ((Movepath)Path).PosToMovePoints(0);
                                                            if (Value < Move.UnitHS.FUnit.FMovePoints)
                                                            {
                                                                Move.UnitHS.FUnit.FMovePoints = Value;
                                                            }
                                                        }

                                                        Value = ((Movepath)Path).MovePointsToPos(Move.UnitHS.FUnit.FMovePoints);
                                                        Value = ((Movepath)Path).PosToMovePoints(Value);
                                                        if (Value > 0 && Value < Move.UnitHS.FUnit.FMovePoints)
                                                        {
                                                            Move.UnitHS.FUnit.FMovePoints = Value;
                                                        }

                                                        // { Original repeats X. }
                                                        GameTables.AoWEngine.OutputDebugStr("AI moves to X: " + IntToStr(Move.X) + " Y: " + IntToStr(Move.X));
                                                        Moved = TacticalCombatUnit.MoveCombatUnit(Move.UnitHS.FUnit, (Movepath)Path);
                                                        if (Move.UnitHS.FUnit.FCombatPlayer == AoWCombatMap.Current.FSeatedPlayer)
                                                        {
                                                            var unitHSYhx = Move.UnitHS.GetYhx();
                                                            var xhx = Move.UnitHS.GetXhx();
                                                            AoWCombatMap.Current.CenterViewTo(xhx, unitHSYhx, 0);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Move.UnitHS.FUnit.FMovePoints = 0;
                                                    }
                                                }
                                            }
                                        }

                                        Settings?.Dispose();
                                    }

                                    break;
                                case TaiMoveType.Melee:
                                    Settings = new CombatMoveControlSettings(null);
                                    var yhx2 = Move.UnitHS.GetYhx();
                                    Settings.SetSourcePos(Move.UnitHS.GetXhx(), yhx2, 0);
                                    Settings.SetDestinationPos(Move.X, Move.Y, 0);
                                    Settings.FArmyHS = Move.UnitHS;
                                    Settings.FMoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee);
                                    Move.UnitHS.FUnit.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                    Settings.FPlayer = Move.UnitHS.FUnit.GetPlayer();
                                    AoWCombatMap.Current.FMoveControl.ProcessMove(Settings);
                                    if (Settings.FPath.Count > 1)
                                    {
                                        Path = Settings.FPath;
                                        PathCost = MakeTerrainPath(Settings, ref Path, ref TerrainCost, Move.UnitHS.FUnit, null, false, false, 1);
                                        if (Path.Count > 1 && PathCost > -1)
                                        {
                                            if (Path[Path.Count - 2].Flag <= Move.UnitHS.FUnit.FMovePoints)
                                            {
                                                // { Original repeats X in the debug text. }
                                                GameTables.AoWEngine.OutputDebugStr("AI touches at X: " + IntToStr(Move.X) + " Y: " + IntToStr(Move.X));
                                                Moved = Move.UnitHS.MeleeMoveTC((Movepath)Path);
                                                if (Move.UnitHS.FUnit.FCombatPlayer == AoWCombatMap.Current.FSeatedPlayer)
                                                {
                                                    var yhx = Move.UnitHS.GetYhx();
                                                    var xhx = Move.UnitHS.GetXhx();
                                                    AoWCombatMap.Current.CenterViewTo(xhx, yhx, 0);
                                                }
                                            }
                                        }
                                    }

                                    Settings?.Dispose();
                                    break;
                                case TaiMoveType.Touch:
                                    AoWHSSet.Current.FAbilityControl.GetAbility(Move.Ability).GetCombatInfo(Move.UnitHS.FUnit.FUnit, ref AIInfo);
                                    Settings = new CombatMoveControlSettings(null);
                                    var yhx3 = Move.UnitHS.GetYhx();
                                    Settings.SetSourcePos(Move.UnitHS.GetXhx(), yhx3, 0);
                                    Settings.SetDestinationPos(Move.X, Move.Y, 0);
                                    Settings.FArmyHS = Move.UnitHS;
                                    Settings.FMoveFlags = Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Melee);
                                    if (AIInfo.DamageTypes.Contains(DamageType.Wall))
                                    {
                                        Settings.FMoveFlags = Settings.FMoveFlags + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.WallCrushing);
                                    }

                                    Move.UnitHS.FUnit.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                    Settings.FPlayer = Move.UnitHS.FUnit.GetPlayer();
                                    Move.UnitHS.FSelectedAbility = Move.Ability;
                                    AoWCombatMap.Current.FMoveControl.ProcessMove(Settings);
                                    if (Settings.FPath.Count > 1)
                                    {
                                        Path = Settings.FPath;
                                        PathCost = MakeTerrainPath(Settings, ref Path, ref TerrainCost, Move.UnitHS.FUnit, null, false, false, 1);
                                        if (Path.Count > 1 && PathCost > -1)
                                        {
                                            if (Path[Path.Count - 2].Flag <= Move.UnitHS.FUnit.FMovePoints)
                                            {
                                                Move.UnitHS.FSelectedAbility = Move.Ability;
                                                // { Original repeats X in the debug text. }
                                                GameTables.AoWEngine.OutputDebugStr("AI melees at X: " + IntToStr(Move.X) + " Y: " + IntToStr(Move.X));
                                                Moved = Move.UnitHS.AbTouchMoveTC((Movepath)Path);
                                                if (Move.UnitHS.FUnit.FCombatPlayer == AoWCombatMap.Current.FSeatedPlayer)
                                                {
                                                    var yhx = Move.UnitHS.GetYhx();
                                                    var xhx = Move.UnitHS.GetXhx();
                                                    AoWCombatMap.Current.CenterViewTo(xhx, yhx, 0);
                                                }
                                            }
                                        }
                                    }

                                    Settings?.Dispose();
                                    break;
                                case TaiMoveType.Ranged:
                                    Move.UnitHS.FSelectedAbility = Move.Ability;
                                    Move.UnitHS.FSelectedSpell = Move.Spell;
                                    // { Original repeats X in the debug text. }
                                    GameTables.AoWEngine.OutputDebugStr("AI ranged at X: " + IntToStr(Move.X) + " Y: " + IntToStr(Move.X));
                                    Moved = Move.UnitHS.AbRangedTC(Move.X, Move.Y);
                                    if (Move.UnitHS.FUnit.FCombatPlayer == AoWCombatMap.Current.FSeatedPlayer)
                                    {
                                        var yhx = Move.UnitHS.GetYhx();
                                        var xhx = Move.UnitHS.GetXhx();
                                        AoWCombatMap.Current.CenterViewTo(xhx, yhx, 0);
                                    }

                                    break;
                            }

                            (
                            ((object)Moves[0]) as IDisposable)?.Dispose();
                            Moves.Delete(0);
                        }

                        if (!Moved)
                        {
                            Status = TaiStatus.Done;
                        }

                        I = 0;
                        int last = Moves.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            (Moves[I] as IDisposable)?.Dispose();
                        }

                        Moves.Clear();
                        CurrentMove = 0;
                    }
                    catch
                    {
                        MessageBox(0, "Error - TCAI", "Error during Execute Moves", 0);
                    }

                    break;
            }
        }
    }

    public enum TaiStatus : byte
    {
        Idle = 0, Busy = 1, Done = 2
    }

}
