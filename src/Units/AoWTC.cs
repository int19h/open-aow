// Translated from AoWTC.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.EngineUnit;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.Sound.SoundUnit;
using Aow.Units.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFXUnit;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngineUnit;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;
using Aow.Units.FleeLoc;
using Aow.Units.CombatTE;
using static Aow.Units.CombatTE.CombatTEUnit;
using Aow.Units.AoWReg;
using Aow.Units.CombatTerrain;
using Aow.Units.HxBridge;
using Aow.Units.City;
using Aow.Units.CityWall;
using static Aow.Units.CombatFire.CombatFireUnit;

namespace Aow.Units.AoWTC;

public enum TCombatMoveModeElement : byte
{
    mmWallCrushing = 0,
    mmMelee = 1,
    mmMoveRanged = 2,
    mmaiMove = 3,
    mmaiWall = 4,
    mmClimbing = 5
}

public enum AnonymousEnum_piMove : byte
{
    piMove = 0,
    piRanged = 1
}

// { ot=1 }
public partial class TTCAbility : TObject
{
    public int Ability;
    public int Spell;
}

// PathIndex. Chance
public partial class TROb : TObject
{
    public int PathIndex;
    public int Chance;
}

// Inherited Width. Height. Levels. HSSFileName. HSMapClassID. MapLevelClassID
public partial class TAoWNewCombatMapSettings : TNewHSMapSettings
{
    public TAoWNewCombatMapSettings() : base()
    {
        HSMapClassID = 2228480;
        MapLevelClassID = 2228482;
        Width = 65;
        Height = 59;
        Levels = 1;
        HSSFileName = AoWEngine.HSSFileName;
    }


    // AoWTC.pas:2147


    // AoWTC.pas:70

}

// MoveType. Ability. Spell. Value. GroupValue. UnitHS. Target. X. Y
public partial class TaiMove : TObject
{
    public TaiMoveType MoveType;
    public short Ability;
    public short Spell;
    public int Value;
    public int GroupValue;
    public TTacticalCombatUnitHS UnitHS;
    public TCombatObject Target;
    public ushort X;
    public ushort Y;
}

// FSide. Units. Allies. Enemies. Moves. Walls. CurrentMove. FUnitIndex. FUnitLimit. FMinMoves.
// FHeroMoves. FNormalUnits. FIgnoreWalls. FFlee. Status
public partial class TCAI : TObject
{
    public TCombatSideIndex FSide;
    public TList Units;
    public TList Allies;
    public TList Enemies;
    public TList Moves;
    public TList Walls;
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
    public override void Destroy()
    {
        Units.Free();
        Allies.Free();
        Enemies.Free();
        Walls.Free();
        for (int I = 0, last = Moves.Count - 1; I <= last; I++)
        {
            ((TObject)Moves[I]).Free();
        }

        Moves.Free();
        base.Destroy();
    }

    // AoWTC.pas:752
    public TCAI()
    {
        Units = new TList();
        Allies = new TList();
        Enemies = new TList();
        Moves = new TList();
        Walls = new TList();
    }

    // AoWTC.pas:774
    public void Reset()
    {
        Status = TaiStatus.aiIdle;
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
    public void CreateMove(TaiMoveType MoveType, int X, int Y, int Value, int Ability, int Spell, TTacticalCombatUnit U, TCombatObject Target)
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
    public int tcDVtoDEV(int DV1, int DV2, TCombatObject Source, TCombatObject Target)
    {
        int Strength;
        Strength = Target.GetTargetStrength();
        if (Target is TTacticalCombatUnit)
        {
            if (((TTacticalCombatUnit)Target).FUnit is THero)
            {
                Strength = Strength * 2;
            }

            if (((TTacticalCombatUnit)Target).FUnit is TLeader)
            {
                Strength = Strength * 2;
            }
        }

        int Result = Strength * DV1;
        Strength = Source.GetTargetStrength();
        if (Source is TTacticalCombatUnit)
        {
            if (((TTacticalCombatUnit)Source).FUnit is THero)
            {
                Strength = Strength * 2;
            }

            if (((TTacticalCombatUnit)Source).FUnit is TLeader)
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
            if (FSide == TCombatSideIndex.csDefender && AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallType != 0 && !AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallBreached)
            {
                Result = true;
            }
            else
            {
                for (int I = 1; I <= 6; I++)
                {
                    if (AoWTCUnit.AoWCombatMap.FPositionStatus[I] == 1)
                    {
                        J = I - 1;
                        if (J == 0)
                        {
                            J = 6;
                        }

                        if (AoWTCUnit.AoWCombatMap.FPositionStatus[J] == 0)
                        {
                            Result = true;
                        }

                        J = I + 1;
                        if (J == 7)
                        {
                            J = 1;
                        }

                        if (AoWTCUnit.AoWCombatMap.FPositionStatus[J] == 0)
                        {
                            Result = true;
                        }
                    }
                    else
                    {
                        if (AoWTCUnit.AoWCombatMap.FPositionStatus[I] == 0)
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
    public int UnitSafe(TTacticalCombatUnit U, int X, int Y)
    {
        int Result;
        int Distance;
        if (!(U.FUnit is THero) || (Units.Count == 1 && FSide == TCombatSideIndex.csDefender))
        {
            Result = 0;
        }
        else
        {
            Result = U.GetTargetStrength() * 20 * U.GetHitPoints() / U.GetHits();
            if (U.FUnit is TLeader)
            {
                Result = Result / 2;
            }

            for (int I = 0, last = Enemies.Count - 1; I <= last; I++)
            {
                var yhx = ((TTacticalCombatUnit)Enemies[I]).FUnitHS.GetYhx();
                Distance = dHXtoRad(X, Y, ((TTacticalCombatUnit)Enemies[I]).FUnitHS.GetXhx(), yhx);
                if (Distance < 15 && ((TTacticalCombatUnit)Enemies[I]).GetTargetStrength() > 0)
                {
                    Result = Result - ((TTacticalCombatUnit)Enemies[I]).GetTargetStrength() * (15 - Distance);
                }
            }

            for (int I = 0, last = Units.Count - 1; I <= last; I++)
            {
                if ((TTacticalCombatUnit)Units[I] != U)
                {
                    var yhx = ((TTacticalCombatUnit)Units[I]).FUnitHS.GetYhx();
                    Distance = dHXtoRad(X, Y, ((TTacticalCombatUnit)Units[I]).FUnitHS.GetXhx(), yhx);
                    if (Distance < 15 && ((TTacticalCombatUnit)Units[I]).GetTargetStrength() > 0)
                    {
                        Result = Result + ((TTacticalCombatUnit)Units[I]).GetTargetStrength() * (15 - Distance);
                    }
                }
            }

            for (int I = 0, last = Allies.Count - 1; I <= last; I++)
            {
                var yhx = ((TTacticalCombatUnit)Allies[I]).FUnitHS.GetYhx();
                Distance = dHXtoRad(X, Y, ((TTacticalCombatUnit)Allies[I]).FUnitHS.GetXhx(), yhx);
                if (Distance < 15 && ((TTacticalCombatUnit)Allies[I]).GetTargetStrength() > 0)
                {
                    Result = Result + ((TTacticalCombatUnit)Allies[I]).GetTargetStrength() * (15 - Distance);
                }
            }
        }

        return Result;
    }

    // AoWTC.pas:880
    public int MakeTerrainPath(TCombatMoveControlSettings MCS, ref TXYLList Path, ref int Value, TTacticalCombatUnit U, TTacticalCombatUnit Ignore, bool FreeStrikes, bool FindWalls, int Cost)
    {
        int Result;
        int J;
        int X = default;
        int Y = default;
        TTacticalCombatUnitHS HS;
        TCityWall Wall;
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
                    HS = (TTacticalCombatUnitHS)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[Path.List[I].X, Path.List[I].Y].FindOwnedChild(2228484);
                    if (FreeStrikes && (HS == null || HS == U.FUnitHS && I - 1 >= 0 && AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[Path.List[I - 1].X, Path.List[I - 1].Y].FindOwnedChild(2228484) == null))
                    {
                        J = 1;
                        for (; J <= 6; J += 1)
                        {
                            CenterHNtoHX(Path.List[I].X, Path.List[I].Y, J, ref X, ref Y);
                            HS = (TTacticalCombatUnitHS)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2228484);
                            if (HS != null && HS.FUnit != Ignore && HS.FUnit.GetSide() != U.GetSide() && MeleeDamageTypes(HS.FUnit) != TSet<TDamageType>.Of() && !HS.FUnit.GetLocked() && HS.FUnit.GetAbilityEnabled(1) == U.GetAbilityEnabled(1))
                            {
                                var tcDVtoDEV2 = tcDVtoDEV(StrikeDV(HS.FUnit, U), 0, HS.FUnit, U);
                                Value = Value - tcDVtoDEV2 * 10;
                            }
                        }
                    }

                    if (I < Path.Count - 1)
                    {
                        if (AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallType != 0)
                        {
                            var pathY = Path[I];
                            var pathX = Path[I];
                            Wall = (TCityWall)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y].FindOwnedChild(2229508);
                            if (Wall == null)
                            {
                                var path = Path[I];
                                var path2 = Path[I];
                                Wall = (TCityWall)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[path2.X, path.Y].FindOwnedChild(2229510);
                            }
                        }
                        else
                        {
                            Wall = null;
                        }

                        var path3 = Path[I];
                        var path4 = Path[I];
                        var mapField = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[path4.X, path3.Y];
                        var path5 = Path[I];
                        var path6 = Path[I];
                        var fieldItem = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[path6.X, path5.Y];
                        Path.List[I].Flag = MCS.FMovePointTable[fieldItem.TerrainType][mapField.OverlayType - -1];
                        if (Wall != null)
                        {
                            // { The wall lookup uses the current path point and the no-overlay column. }
                            if (FSide == TCombatSideIndex.csAttacker && (U.GetAbilityEnabled(65) || U.GetAbilityEnabled(117)) && I == 0)
                            {
                                var pathY = Path[I];
                                var pathX = Path[I];
                                var item = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y];
                                Path.List[I].Flag = MCS.FMovePointTable[item.TerrainType][-1 - -1];
                            }
                            else
                            {
                                if ((FSide == TCombatSideIndex.csAttacker || AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallBreached || FIgnoreWalls) && (Wall.FHitPoints == 0 || (FSide == TCombatSideIndex.csDefender && Wall is TCityWallDoor) || AoWTCUnit.AoWCombatMap.FMoveControl.FUnitHS.FUnit.GetAbilityEnabled(1) || AoWTCUnit.AoWCombatMap.FMoveControl.FUnitHS.FUnit.GetAbilityEnabled(66) || AoWTCUnit.AoWCombatMap.FMoveControl.FUnitHS.FUnit.GetAbilityEnabled(36)))
                                {
                                    var pathY = Path[I];
                                    var pathX = Path[I];
                                    var item2 = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y];
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
                        if (AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[path8.X, path7.Y].FindOwnedChild(2228484) != null)
                        {
                            var pathY = Path[I];
                            var pathX = Path[I];
                            var item2 = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y];
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
    public void EvalAll(ref int Value, TList List, TCombatObject Ignore, int Spell, TTacticalCombatUnit U)
    {
        TCombatObject Target;
        TAbilityDamageValues DV = default;
        for (int I = 0, last = List.Count - 1; I <= last; I++)
        {
            if ((TCombatObject)List[I] != Ignore)
            {
                Target = (TCombatObject)List[I];
                AoWHSSet.FSpellControl.GetSpell(Spell).GetCombatDamageValueEx(U, Target, ref DV);
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
    public void EvalHex(TCombatObject Ignore, int Ability, int Spell, int X, int Y, ref int Value, bool HitWalls, bool Friendly, TTacticalCombatUnit U, TObject IgnoreWall)
    {
        TTacticalCombatUnitHS HS;
        TCityWall Wall;
        TAbilityDamageValues DV = default;
        HS = (TTacticalCombatUnitHS)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2228484);
        if (HS != null && HS.FUnit != Ignore && (Friendly || HS.FUnit.GetSide() != U.GetSide()))
        {
            if (Ability == 52)
            {
                AoWHSSet.FSpellControl.GetSpell(Spell).GetCombatDamageValueEx(U, HS.FUnit, ref DV);
            }
            else
            {
                if (AoWTCUnit.AbilTypes[Ability] == TfxType.fxTouch)
                {
                    AoWHSSet.FAbilityControl.GetAbility(Ability).tcGetDamageValueEx(U, HS.FUnit, ref DV);
                }
                else
                {
                    AoWHSSet.FAbilityControl.GetAbility(Ability).tcGetDamageValueEx(U, HS.FUnit, ref DV);
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

        if (HitWalls && AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallType != 0)
        {
            Wall = (TCityWall)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2229508);
            if (Wall == null)
            {
                Wall = (TCityWall)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2229510);
            }

            if (!AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallBreached && IgnoreWall != null && Wall != null && (Ignore == null || !(Ignore is TCombatWall) || (Ignore is TCombatWall && IgnoreWall != Wall)))
            {
                if (Ability == 52)
                {
                    AoWHSSet.FSpellControl.GetSpell(Spell).GetCombatDamageValueEx(U, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWall, ref DV);
                }
                else
                {
                    if (AoWTCUnit.AbilTypes[Ability] == TfxType.fxTouch)
                    {
                        AoWHSSet.FAbilityControl.GetAbility(Ability).tcGetDamageValueEx(U, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWall, ref DV);
                    }
                    else
                    {
                        AoWHSSet.FAbilityControl.GetAbility(Ability).tcGetDamageValueEx(U, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWall, ref DV);
                    }
                }

                if (Ability == 65)
                {
                    DV.TargetDamage = 0;
                }

                if (DV.SourceDamage > 0)
                {
                    if (U.GetSide() == TCombatSideIndex.csAttacker)
                    {
                        var multDEV = MultDEV(tcDVtoDEV(DV.SourceDamage, 0, U, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWall), AoWTCUnit.WallMaxHP[Wall.FWallType - 1], Wall.FHitPoints);
                        Value = Value + multDEV;
                    }
                    else
                    {
                        var multDEV = MultDEV(tcDVtoDEV(DV.SourceDamage, 0, U, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWall), AoWTCUnit.WallMaxHP[Wall.FWallType - 1], Wall.FHitPoints);
                        Value = Value - multDEV;
                    }
                }
            }
        }
    }

    // AoWTC.pas:1072
    public void EvalArea(ref int Value, TCombatObject Ignore, int Ability, int Spell, int X, int Y, bool HitWalls, bool IncludeCenter, bool Friendly, TTacticalCombatUnit U, TObject IgnoreWall)
    {
        int X1 = default;
        int Y1 = default;
        for (int I = 0; I <= 6; I++)
        {
            CenterHNtoHX(X, Y, I, ref X1, ref Y1);
            if (IncludeCenter || U.FUnitHS.GetXhx() != X1 || U.FUnitHS.GetYhx() != Y1)
            {
                EvalHex(Ignore, Ability, Spell, X1, Y1, ref Value, HitWalls, Friendly, U, IgnoreWall);
            }
        }
    }

    // AoWTC.pas:1084
    public int EvalPath(ref int Value, TCombatObject Ignore, int Ability, int Spell, int X1, int Y1, int X2, int Y2, bool HitWalls, TTacticalCombatUnit U, TObject IgnoreWall)
    {
        int Result;
        TXYZList Path;
        TList Obstacles;
        int X = default;
        int Y = default;
        int V = default;
        int Total;
        int Original;
        int Chance;
        TfxType FX;
        if (Ability == 52)
        {
            FX = AoWTCUnit.SpellTypes[Spell];
        }
        else
        {
            FX = AoWTCUnit.AbilTypes[Ability];
        }

        switch (FX)
        {
            case TfxType.fxArrow:
            case TfxType.fxRock:
            case TfxType.fxComet:
            case TfxType.fxBreath:
            case TfxType.fxCometBall:
            case TfxType.fxRay:
            case TfxType.fxAltArrow:
                Path = new TXYZList();
                Obstacles = new TList();
                if (FX == TfxType.fxBreath)
                {
                    AoWTCUnit.MakeRangedPath(U.GetSide(), 50, 20, 50, (short)X1, (short)Y1, (short)X2, (short)Y2, 0, 0, false, ref Path, ref Obstacles);
                }
                else
                {
                    AoWTCUnit.MakeAbilPath(U.GetSide(), (short)Ability, (short)Spell, (short)X1, (short)Y1, (short)X2, (short)Y2, ref Path, ref Obstacles);
                }

                Total = 0;
                Original = Value;
                Chance = 100;
            {
                for (int I = 0, last = Obstacles.Count - 1; I <= last; I++)
                {
                    var obstacle = Obstacles[I];
                    var path = Path[((TROb)obstacle).PathIndex];
                    var obstacle2 = Obstacles[I];
                    HPtoHX(Path[((TROb)obstacle2).PathIndex].X, path.Y, ref X, ref Y);
                    V = 0;
                    EvalHex(Ignore, Ability, Spell, X, Y, ref V, HitWalls, true, U, IgnoreWall);
                    try
                    {
                        var obstacle3 = Obstacles[I];
                        Total = Total + V * Chance / 100 * ((TROb)obstacle3).Chance / 100;
                    }
                    catch
                    {
                        Total = Total + ((TROb)Obstacles[I]).Chance * (V / 100 * Chance / 100);
                    }

                    Chance = (100 - ((TROb)Obstacles[I]).Chance) * Chance / 100;
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
                Path.Free();
            {
                for (int I = 0, last = Obstacles.Count - 1; I <= last; I++)
                {
                    ((TObject)Obstacles[I]).Free();
                }
            }

                Obstacles.Free();
                Result = Original - Value;
                break;
            default:
                Result = 0;
                break;
        }

        return Result;
    }

    // AoWTC.pas:1130
    public int EvalBreath(ref int Value, TCombatObject Ignore, int Ability, int Spell, int X1, int Y1, int X2, int Y2, TTacticalCombatUnit U)
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
            var dHXtoHN2 = dHXtoHN(X1, Y1, X2, Y2);
            CenterHNtoHX(X1, Y1, AoWTCUnit.BreathHN[AoWTCUnit.BreathDirection[dHXtoHN2 - 1] * 12 + I - 12], ref X, ref Y);
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
    public void CheckUnit(int X, int Y, int Hits, int HitPoints, TCombatMoveControlSettings Settings, TTacticalCombatUnit U, TCombatObject Target, TList Abilities, TObject Wall)
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
        TXYLList Path;
        TXYLList DetourPath;
        TXYLList BasePath;
        TMeleeRound Round;
        TAbilityCombatInfo AbilityInfo = default;
        TSpellCombatInfo SpellInfo = default;
        TCityWall CityWall;
        bool Adjacent;
        TAbilityDamageValues Values = default;
        Path = new TMovepath();
        BasePath = new TMovepath();
        DetourPath = new TMovepath();
        TerrainValue = 0;
        OriginalX = X;
        OriginalY = Y;
        var yhx = U.FUnitHS.GetYhx();
        Distance = dHXtoRad(U.FUnitHS.GetXhx(), yhx, X, Y);
        if (U.FMovePoints > 0)
        {
            AoWTCUnit.AoWCombatMap.FMoveControl.CalculatePath(Settings, (TMovepath)BasePath, (sbyte)X, (sbyte)Y, 0);
            PathCost = MakeTerrainPath(Settings, ref BasePath, ref TerrainValue, U, (TTacticalCombatUnit)Target, true, Target is TTacticalCombatUnit && FSide == TCombatSideIndex.csAttacker, 1);
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
                if (Target is TCombatUnit && Target.GetSide() != U.GetSide() && MeleeDamageTypes(U) != TSet<TDamageType>.Of() && (U.GetAbilityEnabled(1) || !Target.GetAbilityEnabled(1)))
                {
                    Ability = 0;
                    Spell = 0;
                    Round = new TMeleeRound();
                    Round.Calculate(U, Target);
                    if (Round.GetAttackerDV() > 0)
                    {
                        Adjacent = ((TMovepath)Path).MovePointsToPos(U.FMovePoints) == 0;
                        U.FCanFlee = false;
                        if (Target is TCombatWall)
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

                        if (!(U.FUnit is THero))
                        {
                            SafeValue = 0;
                        }
                        else
                        {
                            I = -1;
                            BestIndex = ((TMovepath)Path).MovePointsToPos(U.FMovePoints);
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
                                if (Value > 0 || FNormalUnits == 0 || !(U.FUnit is THero) || Units.Count <= 1)
                                {
                                    FHeroMoves += 1;
                                    if (U.FUnit is THero)
                                    {
                                        I = 50;
                                    }
                                    else
                                    {
                                        I = 100;
                                    }

                                    CreateMove(TaiMoveType.aiMelee, X, Y, MultDEV(Value, I, 1), Ability, Spell, U, Target);
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
                                        CreateMove(TaiMoveType.aiMove, Path[0].X, path.Y, multDEV, Ability, Spell, U, Target);
                                    }
                                    else
                                    {
                                        var moves = U.GetMoves();
                                        CreateMove(TaiMoveType.aiFake, X, Y, MultDEV(Value, moves, Cost), Ability, Spell, U, Target);
                                    }
                                }
                            }
                        }
                    }

                    Round.Free();
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

                Ability = ((TTCAbility)Abilities[AbilityIndex]).Ability;
                Spell = 0;
                X = OriginalX;
                Y = OriginalY;
                AoWHSSet.FAbilityControl.GetAbility(Ability).GetCombatInfo(U.FUnit, ref AbilityInfo);
                switch (Ability)
                {
                    case 52:
                        try
                        {
                            if (!(Target is TCombatWall) || !AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallBreached)
                            {
                                Spell = ((TTCAbility)Abilities[AbilityIndex]).Spell;
                                switch (AoWTCUnit.SpellTypes[Spell])
                                {
                                    case TfxType.fxArrow:
                                    case TfxType.fxRock:
                                    case TfxType.fxComet:
                                    case TfxType.fxBreath:
                                    case TfxType.fxCometBall:
                                    case TfxType.fxEffect:
                                    case TfxType.fxScreenNESW:
                                    case TfxType.fxScreenNS:
                                    case TfxType.fxRay:
                                    case TfxType.fxChain:
                                    case TfxType.fxSwarm:
                                    case TfxType.fxScreenSwarm:
                                    case TfxType.fxSacredWrath:
                                    case TfxType.fxSWComet:
                                    case TfxType.fxDispel:
                                    case TfxType.fxBlood:
                                    case TfxType.fxAltArrow:
                                    case TfxType.fxTremors:
                                    case TfxType.fxHailImpact:
                                    case TfxType.fxHighPrayer:
                                    case TfxType.fxMindDecay:
                                    case TfxType.fxShockWave:
                                    case TfxType.fxSacFlame:
                                    case TfxType.fxWinds:
                                        AoWHSSet.FSpellControl.GetSpell(Spell).GetCombatInfo(U.FUnit, ref SpellInfo);
                                        AoWHSSet.FSpellControl.GetSpell(Spell).GetCombatDamageValueEx(U, Target, ref Values);
                                        if (Values.SourceDamage > 0)
                                        {
                                            U.FCanFlee = false;
                                            if (Target is TCombatWall)
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
                                                I = ((TMovepath)Path).MovePointsToPos(U.FMovePoints);
                                                var pathY = Path[I];
                                                D = dHXtoRad(Path[I].X, pathY.Y, X, Y);
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
                                                                    CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                    var yhx2 = U.FUnitHS.GetYhx();
                                                                    if (dHXtoRad(U.FUnitHS.GetXhx(), yhx2, NX, NY) <= Range)
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
                                                                    CenterHNtoHX(U.FUnitHS.GetXhx(), yhx2, I, ref NX, ref NY);
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
                                                                    CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                    EvalHex(Target, Ability, Spell, NX, NY, ref Value, true, true, U, Wall);
                                                                }
                                                            }

                                                                BestIndex = X;
                                                                BestValue = Y;
                                                            {
                                                                I = 1;
                                                                for (; I <= 18; I += 1)
                                                                {
                                                                    CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                    var yhx2 = U.FUnitHS.GetYhx();
                                                                    if (dHXtoRad(U.FUnitHS.GetXhx(), yhx2, NX, NY) <= Range)
                                                                    {
                                                                        D = Cost;
                                                                        EvalHex(Target, Ability, Spell, U.FUnitHS.GetXhx(), U.FUnitHS.GetYhx(), ref D, true, true, U, Wall);
                                                                        V = 0;
                                                                        for (; V <= 18; V += 1)
                                                                        {
                                                                            CenterHNtoHX(NX, NY, V, ref AX, ref AY);
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
                                                                    CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                    var yhx2 = U.FUnitHS.GetYhx();
                                                                    if (dHXtoRad(U.FUnitHS.GetXhx(), yhx2, NX, NY) <= Range)
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
                                                                    CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                    var yhx2 = U.FUnitHS.GetYhx();
                                                                    if (dHXtoRad(U.FUnitHS.GetXhx(), yhx2, NX, NY) <= Range)
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
                                                                    CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                    var yhx2 = U.FUnitHS.GetYhx();
                                                                    if (dHXtoRad(U.FUnitHS.GetXhx(), yhx2, NX, NY) <= Range)
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
                                                                if (FSide == TCombatSideIndex.csDefender && AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallType != 0)
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
                                                            CityWall = (TCityWall)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[pathX.X, path.Y].FindOwnedChild(2229508);
                                                            if (CityWall == null && FSide == TCombatSideIndex.csAttacker)
                                                            {
                                                                var path2 = Path[I];
                                                                var path3 = Path[I];
                                                                CityWall = (TCityWall)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[path3.X, path2.Y].FindOwnedChild(2229510);
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
                                                                if (dHXtoRad(Path[0].X, path4.Y, path3.X, path2.Y) <= Range)
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
                                                            PathCost = MakeTerrainPath(Settings, ref DetourPath, ref TerrainValue, U, (TTacticalCombatUnit)Target, true, false, 1);
                                                            PathValue += BestValue;
                                                            PathCost = ((TMovepath)Path).PosToMovePoints(0);
                                                        }
                                                    }

                                                    BestIndex = 0;
                                                    I = 1;
                                                    int last3 = Path.Count - 2;
                                                    for (; I <= last3; I += 1)
                                                    {
                                                        var path = Path[I];
                                                        if (dHXtoRad(X, Y, Path[I].X, path.Y) <= Range && ((TMovepath)Path).PosToMovePoints(I) <= U.FMovePoints)
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

                                                    PathCost = ((TMovepath)Path).PosToMovePoints(0);
                                                }

                                                // { Keep move selection inside the positive movement guard. }
                                                Cost = (PathCost - U.FMovePoints - Range * 4) * 10;
                                                if (Cost < 1)
                                                {
                                                    Cost = 1;
                                                }

                                                if (!(U.FUnit is THero))
                                                {
                                                    SafeValue = 0;
                                                }
                                                else
                                                {
                                                    I = -1;
                                                    BestIndex = ((TMovepath)Path).MovePointsToPos(U.FMovePoints);
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

                                                    CreateMove(TaiMoveType.aiRanged, X, Y, MultDEV(Value, Value * 100 / I, 1), Ability, Spell, U, Target);
                                                    if (PathValue > 0 && U.FMovePoints > 0 && DetourPath.Count > 1)
                                                    {
                                                        if (!(DetourPath.List[DetourPath.Count - 2].Flag > U.FMovePoints && DetourPath.List[DetourPath.Count - 2].Flag <= 15))
                                                        {
                                                            FHeroMoves += 1;
                                                            var multDEV = MultDEV(PathValue + TerrainValue, 100 * PathValue / I, 1);
                                                            var detourPath = DetourPath[0];
                                                            CreateMove(TaiMoveType.aiMoveRanged, DetourPath[0].X, detourPath.Y, multDEV, Ability, Spell, U, Target);
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
                                                            CreateMove(TaiMoveType.aiMoveRanged, Path[0].X, path.Y, multDEV, Ability, Spell, U, Target);
                                                        }
                                                        else
                                                        {
                                                            var moves = U.GetMoves();
                                                            CreateMove(TaiMoveType.aiFake, X, Y, MultDEV(MultDEV(PathValue, 100, 1) + TerrainValue + Value, moves, Cost), Ability, Spell, U, Target);
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
                        switch (AoWTCUnit.AbilTypes[Ability])
                        {
                            case TfxType.fxTouch:
                                if (Target is TCombatWall || Target is TCombatUnit && ((TTouchAbility)AoWHSSet.FAbilityControl.GetAbility(Ability)).CanTouch(U, Target))
                                {
                                    try
                                    {
                                        AoWHSSet.FAbilityControl.GetAbility(Ability).tcGetDamageValueEx(U, Target, ref Values);
                                        if (Values.SourceDamage > 0)
                                        {
                                            Adjacent = ((TMovepath)Path).MovePointsToPos(U.FMovePoints) == 0;
                                            U.FCanFlee = false;
                                            if (Target is TCombatWall)
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

                                            if (Ability == 117 && MeleeDamageTypes(U) == TSet<TDamageType>.Of())
                                            {
                                                Value = MultDEV(Value, 100, 1);
                                            }

                                            if (Target.GetTargetStrength() <= 0 && Enemies.Count > 1)
                                            {
                                                Value -= 10000;
                                            }

                                            if (U.FMovePoints > 0 && ((TMovepath)Path).MovePointsToPos(U.FMovePoints) == 0)
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

                                            if (!(U.FUnit is THero))
                                            {
                                                SafeValue = 0;
                                            }
                                            else
                                            {
                                                I = -1;
                                                BestIndex = ((TMovepath)Path).MovePointsToPos(U.FMovePoints);
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
                                                    if (U.FUnit is THero)
                                                    {
                                                        I = 1;
                                                    }
                                                    else
                                                    {
                                                        if (Target is TCombatWall && AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallBreached && AoWTCUnit.AoWCombatMap.FRound > 1)
                                                        {
                                                            I = 10;
                                                        }
                                                        else
                                                        {
                                                            I = 100;
                                                        }
                                                    }

                                                    CreateMove(TaiMoveType.aiTouch, X, Y, MultDEV(Value, I, 1), Ability, Spell, U, Target);
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
                                                            CreateMove(TaiMoveType.aiMove, Path[0].X, path.Y, multDEV, Ability, Spell, U, Target);
                                                        }
                                                        else
                                                        {
                                                            var moves = U.GetMoves();
                                                            CreateMove(TaiMoveType.aiFake, X, Y, MultDEV(Value, moves, Cost), Ability, Spell, U, Target);
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
                            case TfxType.fxArrow:
                            case TfxType.fxRock:
                            case TfxType.fxComet:
                            case TfxType.fxBreath:
                            case TfxType.fxCometBall:
                            case TfxType.fxEffect:
                            case TfxType.fxScreenNESW:
                            case TfxType.fxScreenNS:
                            case TfxType.fxRay:
                            case TfxType.fxChain:
                            case TfxType.fxSwarm:
                            case TfxType.fxScreenSwarm:
                            case TfxType.fxSacredWrath:
                            case TfxType.fxSWComet:
                            case TfxType.fxDispel:
                            case TfxType.fxBlood:
                            case TfxType.fxAltArrow:
                            case TfxType.fxTremors:
                            case TfxType.fxHailImpact:
                            case TfxType.fxHighPrayer:
                                try
                                {
                                    Cost = AoWHSSet.FAbilityControl.GetAbility(Ability).tcGetDamageValue(U, Target);
                                    if (Cost > 0)
                                    {
                                        U.FCanFlee = false;
                                        if (Target is TCombatWall)
                                        {
                                            FFlee = false;
                                        }

                                        PathValue = 0;
                                        if (AoWHSSet.FAbilityControl.GetAbility(Ability).GetCombatMode(U.FUnit) == TCombatMode.cmLongRange)
                                        {
                                            Range = 12;
                                        }
                                        else
                                        {
                                            if (AoWHSSet.FAbilityControl.GetAbility(Ability).GetCombatMode(U.FUnit) == TCombatMode.cmMediumRange)
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
                                            I = ((TMovepath)Path).MovePointsToPos(U.FMovePoints);
                                            var pathY = Path[I];
                                            D = dHXtoRad(Path[I].X, pathY.Y, X, Y);
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
                                                        CityWall = (TCityWall)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[pathX.X, path.Y].FindOwnedChild(2229508);
                                                        if (CityWall == null && FSide == TCombatSideIndex.csAttacker)
                                                        {
                                                            var path2 = Path[I];
                                                            var path3 = Path[I];
                                                            CityWall = (TCityWall)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[path3.X, path2.Y].FindOwnedChild(2229510);
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
                                                            if (dHXtoRad(Path[0].X, path4.Y, path3.X, path2.Y) <= Range)
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
                                                        PathCost = MakeTerrainPath(Settings, ref DetourPath, ref TerrainValue, U, (TTacticalCombatUnit)Target, true, false, 1);
                                                        PathValue += BestValue;
                                                        PathCost = ((TMovepath)Path).PosToMovePoints(0);
                                                    }
                                                }

                                                BestIndex = 0;
                                                I = 1;
                                                int last3 = Path.Count - 2;
                                                for (; I <= last3; I += 1)
                                                {
                                                    var path = Path[I];
                                                    if (dHXtoRad(X, Y, Path[I].X, path.Y) <= Range && ((TMovepath)Path).PosToMovePoints(I) <= U.FMovePoints)
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

                                                PathCost = ((TMovepath)Path).PosToMovePoints(0);
                                            }

                                            // { Keep move selection inside the positive movement guard. }
                                            Cost = (PathCost - U.FMovePoints - Range * 4) * 10;
                                            if (Cost < 1)
                                            {
                                                Cost = 1;
                                            }

                                            if (!(U.FUnit is THero))
                                            {
                                                SafeValue = 0;
                                            }
                                            else
                                            {
                                                I = -1;
                                                BestIndex = ((TMovepath)Path).MovePointsToPos(U.FMovePoints);
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
                                                if (Target is TCombatWall && AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallBreached && AoWTCUnit.AoWCombatMap.FRound > 1)
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

                                                CreateMove(TaiMoveType.aiRanged, X, Y, MultDEV(Value, I * Value / V, 1), Ability, Spell, U, Target);
                                                if (PathValue > 0 && U.FMovePoints > 0 && DetourPath.Count > 1)
                                                {
                                                    if (!(DetourPath.List[DetourPath.Count - 2].Flag > U.FMovePoints && DetourPath.List[DetourPath.Count - 2].Flag <= 15))
                                                    {
                                                        FHeroMoves += 1;
                                                        var multDEV = MultDEV(PathValue + TerrainValue, I * PathValue / V, 1);
                                                        var detourPath = DetourPath[0];
                                                        CreateMove(TaiMoveType.aiMoveRanged, DetourPath[0].X, detourPath.Y, multDEV, Ability, Spell, U, Target);
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
                                                        CreateMove(TaiMoveType.aiMoveRanged, Path[0].X, path.Y, multDEV, Ability, Spell, U, Target);
                                                    }
                                                    else
                                                    {
                                                        var moves = U.GetMoves();
                                                        CreateMove(TaiMoveType.aiFake, X, Y, MultDEV(MultDEV(PathValue, 100, 1) + TerrainValue + Value, moves, Cost), Ability, Spell, U, Target);
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

        Path.Free();
        BasePath.Free();
        DetourPath.Free();
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
        TTacticalCombatUnit U;
        TTacticalCombatUnit Target;
        TCombatMoveControlSettings Settings;
        bool Moved;
        bool Found;
        bool Visible;
        TList Abilities;
        int TerrainCost = default;
        int PathCost;
        TList Bonuses;
        string Reason = "";
        TXYLList Path = default;
        TAbilityCombatInfo AIInfo = default;
        TSpellCombatInfo SpellInfo = default;
        TCityWall Wall;
        Status = TaiStatus.aiBusy;
        if (AoWHSMap.FTokenManager.PlayerReady(AoWTCUnit.AoWCombatMap.FCurrentPlayer))
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
                            ((TObject)Moves[I]).Free();
                        }

                        Moves.Clear();
                        Walls.Clear();
                        var tacticalCombat = AoWTCUnit.AoWCombatMap.FCombat;
                        FSide = (TCombatSideIndex)tacticalCombat.FCombatData.FPlayers.GetPlayers((sbyte)tacticalCombat.FCombatData.FCurrentPlayer).FSideIndex;
                        FNormalUnits = 0;
                        I = 0;
                        int limit = tacticalCombat.FCombatData.GetCount() - 1;
                        for (; I <= limit; I += 1)
                        {
                            if (tacticalCombat.FCombatData.GetObjects(I) != null && tacticalCombat.FCombatData.GetObjects(I) is TCombatUnit && tacticalCombat.FCombatData.GetObjects(I).GetEnabled())
                            {
                                Visible = false;
                                J = 0;
                                while (!Visible && J < 19)
                                {
                                    var yhx = ((TTacticalCombatUnit)tacticalCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
                                    CenterHNtoHX(((TTacticalCombatUnit)tacticalCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx(), yhx, J, ref K, ref L);
                                    if (!AoWTCUnit.AoWCombatMap.FExplorationEnabled || ((TAoWMapField)AoWTCUnit.AoWCombatMap.MapContainer.GetField(K, L, 0)).GetExplored(AoWTCUnit.AoWCombatMap.FSeatedPlayer))
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
                                                if (((TTacticalCombatUnit)tacticalCombat.FCombatData.GetObjects(I)).FUnit is THero)
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

                        AoWEngine.OutputDebugStr("AI found " + IntToStr(Units.Count) + " Own units.");
                        AoWEngine.OutputDebugStr("AI found " + IntToStr(Allies.Count) + " Ally units.");
                        AoWEngine.OutputDebugStr("AI found " + IntToStr(Enemies.Count) + " Enemy units.");
                        FIgnoreWalls = FSide == TCombatSideIndex.csAttacker;
                        FFlee = FSide == TCombatSideIndex.csAttacker && AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallType != 0 && !AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallBreached;
                        FMinMoves = 100;
                        I = 0;
                        int last2 = Units.Count - 1;
                        for (; I <= last2; I += 1)
                        {
                            U = (TTacticalCombatUnit)Units[I];
                            U.FCanFlee = FSide == TCombatSideIndex.csAttacker;
                            if (U.GetMoves() < FMinMoves)
                            {
                                FMinMoves = U.GetMoves();
                            }

                            J = 0;
                            int last3 = Enemies.Count - 1;
                            for (; J <= last3; J += 1)
                            {
                                Target = (TTacticalCombatUnit)Enemies[J];
                                if (!FIgnoreWalls && Target.GetSide() != U.GetSide())
                                {
                                    var yhx = Target.FUnitHS.GetYhx();
                                    var xhx = Target.FUnitHS.GetXhx();
                                    var fUnitHSYhx = U.FUnitHS.GetYhx();
                                    K = dHXtoRad(U.FUnitHS.GetXhx(), fUnitHSYhx, xhx, yhx);
                                    L = 22;
                                    for (; L <= 169; L += 1)
                                    {
                                        if (Target.GetAbilityEnabled(L))
                                        {
                                            if (L == 52 && Target.FUnit is THero)
                                            {
                                                N = 0;
                                                for (; N <= 130; N += 1)
                                                {
                                                    bool condition = Target.FUnit.FPlayer > 0 && AoWTCUnit.SpellTypes[N] != TfxType.fxNone && AoWHSMap.FPlayers.GetPlayers(Target.FUnit.FPlayer).FMagicControl.FSpells.IndexOf(N) != -1;
                                                    if (condition)
                                                    {
                                                        var spell = AoWHSSet.FSpellControl.GetSpell(N);
                                                        condition = ((THero)Target.FUnit).CanCastSpellInstantly(N, spell.FBaseManaCost, ref Reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        AoWHSSet.FSpellControl.GetSpell(N).GetCombatInfo(Target.FUnit, ref SpellInfo);
                                                        if (SpellInfo.TargetTypes.Contains(TCombatTargetTypeElement.cttEnemy) && (K <= SpellInfo.Range || SpellInfo.Range == -1))
                                                        {
                                                            Value = AoWHSSet.FSpellControl.GetSpell(N).GetCombatDamageValue(Target, U);
                                                            if (Value > 0)
                                                            {
                                                                if (TSet<TfxType>.Of(TfxType.fxArrow, TfxType.fxRock, TfxType.fxComet, TfxType.fxBreath, TfxType.fxCometBall, TfxType.fxRay).Contains(AoWTCUnit.SpellTypes[N]))
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
                                                if (AoWTCUnit.AbilTypes[L] != TfxType.fxNone && AoWTCUnit.AbilTypes[L] != TfxType.fxTouch)
                                                {
                                                    AoWHSSet.FAbilityControl.GetAbility(L).GetCombatInfo(Target.FUnit, ref AIInfo);
                                                    if ((((byte)AIInfo.TargetTypes.Image) & 2) != 0)
                                                    {
                                                        if (AoWHSSet.FAbilityControl.GetAbility(L).GetCombatMode(Target.FUnit) == TCombatMode.cmLongRange)
                                                        {
                                                            N = 12;
                                                        }
                                                        else
                                                        {
                                                            if (AoWHSSet.FAbilityControl.GetAbility(L).GetCombatMode(Target.FUnit) == TCombatMode.cmMediumRange)
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
                                                            Value = AoWHSSet.FAbilityControl.GetAbility(L).tcGetDamageValue(Target, U);
                                                            if (Value > 0)
                                                            {
                                                                if (TSet<TfxType>.Of(TfxType.fxArrow, TfxType.fxRock, TfxType.fxComet, TfxType.fxBreath, TfxType.fxCometBall, TfxType.fxRay).Contains(AoWTCUnit.AbilTypes[L]))
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
                            U = (TTacticalCombatUnit)Units[FUnitIndex];
                            if (!U.GetLocked())
                            {
                                Abilities = new TList();
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

                                            if (J == 52 && U.FUnit is THero)
                                            {
                                                K = 0;
                                                for (; K <= 130; K += 1)
                                                {
                                                    bool condition = U.FUnit.FPlayer > 0 && AoWTCUnit.SpellTypes[K] != TfxType.fxNone && AoWHSMap.FPlayers.GetPlayers(U.FUnit.FPlayer).FMagicControl.FSpells.IndexOf(K) != -1;
                                                    if (condition)
                                                    {
                                                        var spell = AoWHSSet.FSpellControl.GetSpell(K);
                                                        condition = ((THero)U.FUnit).CanCastSpellInstantly(K, spell.FBaseManaCost, ref Reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        AoWHSSet.FSpellControl.GetSpell(K).GetCombatInfo(U.FUnit, ref SpellInfo);
                                                        if (SpellInfo.TargetTypes.Contains(TCombatTargetTypeElement.cttEnemy))
                                                        {
                                                            Abilities.Add(new TTCAbility());
                                                            ((TTCAbility)Abilities[Abilities.Count - 1]).Ability = J;
                                                            ((TTCAbility)Abilities[Abilities.Count - 1]).Spell = K;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (AoWTCUnit.AbilTypes[J] != TfxType.fxNone)
                                                {
                                                    AoWHSSet.FAbilityControl.GetAbility(J).GetCombatInfo(U.FUnit, ref AIInfo);
                                                    if ((((byte)AIInfo.TargetTypes.Image) & 2) != 0)
                                                    {
                                                        Abilities.Add(new TTCAbility());
                                                        ((TTCAbility)Abilities[Abilities.Count - 1]).Ability = J;
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

                                Settings = new TCombatMoveControlSettings(null);
                                if (U.FMovePoints > 0)
                                {
                                    var yhx = U.FUnitHS.GetYhx();
                                    Settings.SetSourcePos(U.FUnitHS.GetXhx(), yhx, 0);
                                    Settings.FArmyHS = U.FUnitHS;
                                    Settings.FMoveFlags = TSet<TCombatMoveModeElement>.Of(TCombatMoveModeElement.mmMelee, TCombatMoveModeElement.mmaiMove, TCombatMoveModeElement.mmaiWall);
                                    U.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                    Settings.FPlayer = U.GetPlayer();
                                    Settings.FMovePointsMax = 200;
                                    AoWTCUnit.AoWCombatMap.FMoveControl.ProcessMove(Settings);
                                }

                                if (Abilities.Count > 0 || MeleeDamageTypes(U) != TSet<TDamageType>.Of())
                                {
                                    J = 0;
                                    int last = Enemies.Count - 1;
                                    for (; J <= last; J += 1)
                                    {
                                        Target = (TTacticalCombatUnit)Enemies[J];
                                        if (!(U.FUnit is THero) || FHeroMoves == 0)
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
                                    ((TObject)Abilities[I]).Free();
                                }

                                Abilities.Clear();
                                try
                                {
                                    J = 22;
                                    for (; J <= 169; J += 1)
                                    {
                                        if (U.GetAbilityEnabled(J))
                                        {
                                            if (J == 52 && U.FUnit is THero)
                                            {
                                                K = 0;
                                                for (; K <= 130; K += 1)
                                                {
                                                    bool condition = AoWTCUnit.SpellTypes[K] != TfxType.fxNone && AoWHSMap.FPlayers.GetPlayers(U.FUnit.FPlayer).FMagicControl.FSpells.IndexOf(K) != -1;
                                                    if (condition)
                                                    {
                                                        var spell = AoWHSSet.FSpellControl.GetSpell(K);
                                                        condition = ((THero)U.FUnit).CanCastSpellInstantly(K, spell.FBaseManaCost, ref Reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        AoWHSSet.FSpellControl.GetSpell(K).GetCombatInfo(U.FUnit, ref SpellInfo);
                                                        if (SpellInfo.TargetTypes.Contains(TCombatTargetTypeElement.cttFriend))
                                                        {
                                                            Abilities.Add(new TTCAbility());
                                                            ((TTCAbility)Abilities[Abilities.Count - 1]).Ability = J;
                                                            ((TTCAbility)Abilities[Abilities.Count - 1]).Spell = K;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (AoWTCUnit.AbilTypes[J] != TfxType.fxNone)
                                                {
                                                    AoWHSSet.FAbilityControl.GetAbility(J).GetCombatInfo(U.FUnit, ref AIInfo);
                                                    if ((((byte)AIInfo.TargetTypes.Image) & 1) != 0)
                                                    {
                                                        Abilities.Add(new TTCAbility());
                                                        ((TTCAbility)Abilities[Abilities.Count - 1]).Ability = J;
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
                                        Target = (TTacticalCombatUnit)Units[J];
                                        if (!(U.FUnit is THero) || FHeroMoves == 0)
                                        {
                                            var hits = Target.GetHits();
                                            var hitPoints = Target.GetHitPoints();
                                            var yhx = Target.FUnitHS.GetYhx();
                                            CheckUnit(Target.FUnitHS.GetXhx(), yhx, hitPoints, hits, Settings, U, Target, Abilities, null);
                                        }
                                    }
                                }

                                Settings.Free();
                                I = 0;
                                int last2 = Abilities.Count - 1;
                                for (; I <= last2; I += 1)
                                {
                                    ((TObject)Abilities[I]).Free();
                                }

                                Abilities.Free();
                            }

                            FUnitIndex += 1;
                            FUnitLimit += 1;
                        }

                        if (FUnitIndex >= Units.Count)
                        {
                            CurrentMove += 1;
                            try
                            {
                                if (AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallType != 0 && AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWall != null && FSide == TCombatSideIndex.csAttacker)
                                {
                                    J = 0;
                                    int last = AoWTCUnit.AoWCombatMap.FWalls.Count - 1;
                                    for (; J <= last; J += 1)
                                    {
                                        bool condition = ((TCityWall)AoWTCUnit.AoWCombatMap.FWalls[J]).FHitPoints > 0;
                                        if (condition)
                                        {
                                            var wall = AoWTCUnit.AoWCombatMap.FWalls[J];
                                            var wall2 = AoWTCUnit.AoWCombatMap.FWalls[J];
                                            condition = ((TCityWall)wall).FHitPoints < AoWTCUnit.WallMaxHP[((TCityWall)wall2).FWallType - 1];
                                        }

                                        if (condition)
                                        {
                                            Found = true;
                                            K = 0;
                                            while (Found && K < Walls.Count)
                                            {
                                                if (Walls[K] == AoWTCUnit.AoWCombatMap.FWalls[J])
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
                                                var wall = AoWTCUnit.AoWCombatMap.FWalls[J];
                                                Walls.Add(wall);
                                            }
                                        }
                                    }
                                }

                                if (AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallType == 0 || AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWall == null || FSide == TCombatSideIndex.csDefender)
                                {
                                    CurrentMove += 1;
                                }

                                FUnitIndex = 0;
                                AoWEngine.OutputDebugStr("AI calculated " + IntToStr(Units.Count * Enemies.Count) + " Attack paths.");
                                AoWEngine.OutputDebugStr("AI calculated " + IntToStr(Moves.Count) + " Moves.");
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
                            U = (TTacticalCombatUnit)Units[FUnitIndex];
                            if (!U.GetLocked())
                            {
                                Abilities = new TList();
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

                                        if (J == 52 && U.FUnit is THero)
                                        {
                                            if (U.FUnit.FPlayer > 0)
                                            {
                                                K = 0;
                                                for (; K <= 130; K += 1)
                                                {
                                                    bool condition = AoWTCUnit.SpellTypes[K] != TfxType.fxNone && AoWHSMap.FPlayers.GetPlayers(U.FUnit.FPlayer).FMagicControl.FSpells.IndexOf(K) != -1;
                                                    if (condition)
                                                    {
                                                        var spell = AoWHSSet.FSpellControl.GetSpell(K);
                                                        condition = ((THero)U.FUnit).CanCastSpellInstantly(K, spell.FBaseManaCost, ref Reason);
                                                    }

                                                    if (condition)
                                                    {
                                                        AoWHSSet.FSpellControl.GetSpell(K).GetCombatInfo(U.FUnit, ref SpellInfo);
                                                        if (SpellInfo.DamageTypes.Contains(TDamageType.dtWall))
                                                        {
                                                            FFlee = false;
                                                            Abilities.Add(new TTCAbility());
                                                            ((TTCAbility)Abilities[Abilities.Count - 1]).Ability = J;
                                                            ((TTCAbility)Abilities[Abilities.Count - 1]).Spell = K;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (AoWTCUnit.AbilTypes[J] != TfxType.fxNone)
                                            {
                                                AoWHSSet.FAbilityControl.GetAbility(J).GetCombatInfo(U.FUnit, ref AIInfo);
                                                if (AIInfo.DamageTypes.Contains(TDamageType.dtWall))
                                                {
                                                    FFlee = false;
                                                    Abilities.Add(new TTCAbility());
                                                    ((TTCAbility)Abilities[Abilities.Count - 1]).Ability = J;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (Abilities.Count > 0)
                                {
                                    Settings = new TCombatMoveControlSettings(null);
                                    if (U.FMovePoints > 0)
                                    {
                                        var yhx = U.FUnitHS.GetYhx();
                                        Settings.SetSourcePos(U.FUnitHS.GetXhx(), yhx, 0);
                                        Settings.FArmyHS = U.FUnitHS;
                                        Settings.FMoveFlags = TSet<TCombatMoveModeElement>.Of(TCombatMoveModeElement.mmMelee, TCombatMoveModeElement.mmaiMove, TCombatMoveModeElement.mmaiWall);
                                        if (U.GetAbilityEnabled(65) || U.GetAbilityEnabled(117))
                                        {
                                            Settings.FMoveFlags = Settings.FMoveFlags + TSet<TCombatMoveModeElement>.Of(TCombatMoveModeElement.mmWallCrushing);
                                        }

                                        U.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                        Settings.FPlayer = U.GetPlayer();
                                        Settings.FMovePointsMax = 200;
                                        AoWTCUnit.AoWCombatMap.FMoveControl.ProcessMove(Settings);
                                    }

                                    J = 0;
                                    int last = Walls.Count - 1;
                                    for (; J <= last; J += 1)
                                    {
                                        Wall = (TCityWall)Walls[J];
                                        if (!(U.FUnit is THero) || FHeroMoves == 0)
                                        {
                                            var yhx = Wall.GetYhx();
                                            CheckUnit(Wall.GetXhx(), yhx, Wall.FHitPoints, AoWTCUnit.WallMaxHP[Wall.FWallType - 1] * 2, Settings, U, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWall, Abilities, Wall);
                                        }
                                    }

                                    if (Walls.Count == 0)
                                    {
                                        L = 100;
                                        Wall = null;
                                        J = 0;
                                        int limit = AoWTCUnit.AoWCombatMap.FWalls.Count - 1;
                                        for (; J <= limit; J += 1)
                                        {
                                            var yhx = U.FUnitHS.GetYhx();
                                            var yhx2 = ((TCityWall)AoWTCUnit.AoWCombatMap.FWalls[J]).GetYhx();
                                            N = dHXtoRad(((TCityWall)AoWTCUnit.AoWCombatMap.FWalls[J]).GetXhx(), yhx2, U.FUnitHS.GetXhx(), yhx);
                                            if (((TCityWall)AoWTCUnit.AoWCombatMap.FWalls[J]).FHitPoints > 0 && N < L)
                                            {
                                                Wall = (TCityWall)AoWTCUnit.AoWCombatMap.FWalls[J];
                                                L = N;
                                            }
                                        }

                                        if (Wall != null)
                                        {
                                            var yhx = Wall.GetYhx();
                                            CheckUnit(Wall.GetXhx(), yhx, Wall.FHitPoints * 2, AoWTCUnit.WallMaxHP[Wall.FWallType - 1], Settings, U, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWall, Abilities, Wall);
                                        }
                                    }

                                    Settings.Free();
                                    I = 0;
                                    int last2 = Abilities.Count - 1;
                                    for (; I <= last2; I += 1)
                                    {
                                        ((TObject)Abilities[I]).Free();
                                    }
                                }

                                Abilities.Free();
                            }

                            FUnitIndex += 1;
                            FUnitLimit += 1;
                        }

                        if (FUnitIndex >= Units.Count)
                        {
                            CurrentMove += 1;
                            Walls.Clear();
                            FUnitIndex = 0;
                            AoWEngine.OutputDebugStr("AI calculated " + IntToStr(Units.Count * Enemies.Count) + " Attack paths.");
                            AoWEngine.OutputDebugStr("AI calculated " + IntToStr(Moves.Count) + " Moves.");
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
                        Bonuses = new TList();
                        I = 0;
                        int last = Units.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            Bonuses.Add(null);
                        }

                        while (FUnitIndex < Moves.Count && FUnitLimit < 50)
                        {
                            Move = (TaiMove)Moves[FUnitIndex];
                            if (Move.MoveType != TaiMoveType.aiFake)
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
                                        K = dHXtoRad(Move.X, Move.Y, OtherMove.X, OtherMove.Y) * 4 - OtherMove.UnitHS.FUnit.FMovePoints;
                                        if (K < 4)
                                        {
                                            K = 4;
                                        }

                                        L = OtherMove.UnitHS.FUnit.GetMoves() * (OtherMove.Value / (K * 10));
                                        K = Units.IndexOf(OtherMove.UnitHS.FUnit);
                                        if (ObjectToInt((TObject)Bonuses[K]) < L)
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
                                        Move.GroupValue += ObjectToInt((TObject)bonuse);
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
                        Bonuses.Free();
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
                            U = (TTacticalCombatUnit)Units[I];
                            if (U.FUnit is THero && FHeroMoves == 0 && U.FMovePoints > 0)
                            {
                                var yhx = U.FUnitHS.GetYhx();
                                K = UnitSafe(U, U.FUnitHS.GetXhx(), yhx);
                                Value = K;
                                N = -1;
                                J = 0;
                                int limit = Units.Count - 1;
                                for (; J <= limit; J += 1)
                                {
                                    Target = (TTacticalCombatUnit)Units[J];
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
                                    Settings = new TCombatMoveControlSettings(null);
                                    // { Original uses X for both coordinates. }
                                    var xhx = U.FUnitHS.GetXhx();
                                    Settings.SetSourcePos(U.FUnitHS.GetXhx(), xhx, 0);
                                    var fUnitHSYhx = ((TTacticalCombatUnit)Units[N]).FUnitHS.GetYhx();
                                    Settings.SetDestinationPos(((TTacticalCombatUnit)Units[N]).FUnitHS.GetXhx(), fUnitHSYhx, 0);
                                    Settings.FArmyHS = U.FUnitHS;
                                    Settings.FMoveFlags = TSet<TCombatMoveModeElement>.Of(TCombatMoveModeElement.mmMelee, TCombatMoveModeElement.mmaiMove, TCombatMoveModeElement.mmaiWall);
                                    U.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                    Settings.FPlayer = U.GetPlayer();
                                    AoWTCUnit.AoWCombatMap.FMoveControl.ProcessMove(Settings);
                                    if (Settings.FPath.Count > 2)
                                    {
                                        Settings.FPath.Delete(0);
                                        var path = Settings.FPath[0];
                                        CreateMove(TaiMoveType.aiMove, Settings.FPath[0].X, path.Y, K, 0, 0, U, null);
                                    }

                                    Settings.Free();
                                }
                                else
                                {
                                    if (FSide == TCombatSideIndex.csAttacker)
                                    {
                                        U.FCanFlee = true;
                                    }
                                }
                            }
                        }

                        if (FSide == TCombatSideIndex.csAttacker)
                        {
                            I = 0;
                            int limit = Units.Count - 1;
                            for (; I <= limit; I += 1)
                            {
                                U = (TTacticalCombatUnit)Units[I];
                                if ((!(U.FUnit is THero) || FHeroMoves == 0 || (U.FUnit is THero && Units.Count == 1)) && U.GetEnabled() && !U.GetLocked() && U.FMovePoints > 0 && (FFlee || U.FCanFlee))
                                {
                                    J = 0;
                                    int last2 = AoWTCUnit.AoWCombatMap.FFleeLocations.Count - 1;
                                    for (; J <= last2; J += 1)
                                    {
                                        var yhx = U.FUnitHS.GetYhx();
                                        var yhx2 = ((THexagonSprite)AoWTCUnit.AoWCombatMap.FFleeLocations[J]).GetYhx();
                                        var dHXtoRad2 = dHXtoRad(((THexagonSprite)AoWTCUnit.AoWCombatMap.FFleeLocations[J]).GetXhx(), yhx2, U.FUnitHS.GetXhx(), yhx);
                                        var yhx3 = ((THexagonSprite)AoWTCUnit.AoWCombatMap.FFleeLocations[J]).GetYhx();
                                        CreateMove(TaiMoveType.aiMove, ((THexagonSprite)AoWTCUnit.AoWCombatMap.FFleeLocations[J]).GetXhx(), yhx3, 110000000 - dHXtoRad2 * 10, 0, 0, U, null);
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
                                case TaiMoveType.aiFake:
                                    Move.Free();
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
                                case TaiMoveType.aiMove:
                                case TaiMoveType.aiMoveRanged:
                                {
                                    var tacticalCombatUnitHS = Move.UnitHS;
                                    if ((Move.UnitHS.FUnit.FUnit is THero || (FSide == TCombatSideIndex.csAttacker && AoWTCUnit.AoWCombatMap.FRound == 1)) && Move.UnitHS.FUnit.FMovePoints > FMinMoves)
                                    {
                                        Move.UnitHS.FUnit.FMovePoints = FMinMoves;
                                    }

                                    Settings = new TCombatMoveControlSettings(null);
                                    var lhx = tacticalCombatUnitHS.GetLhx();
                                    var yhx = tacticalCombatUnitHS.GetYhx();
                                    Settings.SetSourcePos(tacticalCombatUnitHS.GetXhx(), yhx, lhx);
                                    Settings.SetDestinationPos(Move.X, Move.Y, 0);
                                    Settings.FArmyHS = Move.UnitHS;
                                    if (Move.MoveType == TaiMoveType.aiMoveRanged || MeleeDamageTypes(Move.UnitHS.FUnit) == TSet<TDamageType>.Of())
                                    {
                                        Settings.FMoveFlags = TSet<TCombatMoveModeElement>.Of(TCombatMoveModeElement.mmMoveRanged);
                                    }
                                    else
                                    {
                                        Settings.FMoveFlags = TSet<TCombatMoveModeElement>.Of(TCombatMoveModeElement.mmMelee);
                                    }

                                    if (AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallType != 0)
                                    {
                                        if (!(AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallBreached && AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[Move.X, Move.Y].FindOwnedChild(2229508) == null && AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[Move.X, Move.Y].FindOwnedChild(2229510) == null))
                                        {
                                            if (!FIgnoreWalls || !ChargeFromPos((int)(((uint)Move.UnitHS.FUnit.FPosition) >> 4)) || FSide == TCombatSideIndex.csAttacker)
                                            {
                                                Settings.FMoveFlags = Settings.FMoveFlags + TSet<TCombatMoveModeElement>.Of(TCombatMoveModeElement.mmaiWall);
                                            }
                                        }
                                    }

                                    Move.UnitHS.FUnit.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                    Settings.FPlayer = Move.UnitHS.FUnit.GetPlayer();
                                    AoWTCUnit.AoWCombatMap.FMoveControl.ProcessMove(Settings);
                                    if (Settings.FPath.Count < 2 && !Settings.FMoveFlags.Contains(TCombatMoveModeElement.mmaiWall))
                                    {
                                        Settings.FMoveFlags = Settings.FMoveFlags + TSet<TCombatMoveModeElement>.Of(TCombatMoveModeElement.mmaiWall);
                                        AoWTCUnit.AoWCombatMap.FMoveControl.ProcessMove(Settings);
                                    }

                                    Wall = (TCityWall)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[Move.X, Move.Y].FindOwnedChild(2229508);
                                    if (Wall == null)
                                    {
                                        Wall = (TCityWall)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[Move.X, Move.Y].FindOwnedChild(2229510);
                                    }

                                    if (Settings.FPath.Count == 2 && Move.MoveType == TaiMoveType.aiMoveRanged && Wall != null)
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
                                                    if (Move.MoveType == TaiMoveType.aiMoveRanged)
                                                    {
                                                        Value = ((TMovepath)Path).PosToMovePoints(0);
                                                        if (Value < Move.UnitHS.FUnit.FMovePoints)
                                                        {
                                                            Move.UnitHS.FUnit.FMovePoints = Value;
                                                        }
                                                    }

                                                    Value = ((TMovepath)Path).MovePointsToPos(Move.UnitHS.FUnit.FMovePoints);
                                                    Value = ((TMovepath)Path).PosToMovePoints(Value);
                                                    if (Value > 0 && Value < Move.UnitHS.FUnit.FMovePoints)
                                                    {
                                                        Move.UnitHS.FUnit.FMovePoints = Value;
                                                    }

                                                    // { Original repeats X. }
                                                    AoWEngine.OutputDebugStr("AI moves to X: " + IntToStr(Move.X) + " Y: " + IntToStr(Move.X));
                                                    Moved = MoveCombatUnit(Move.UnitHS.FUnit, (TMovepath)Path);
                                                    if (Move.UnitHS.FUnit.FCombatPlayer == AoWTCUnit.AoWCombatMap.FSeatedPlayer)
                                                    {
                                                        var unitHSYhx = Move.UnitHS.GetYhx();
                                                        var xhx = Move.UnitHS.GetXhx();
                                                        AoWTCUnit.AoWCombatMap.CenterViewTo(xhx, unitHSYhx, 0);
                                                    }
                                                }
                                                else
                                                {
                                                    Move.UnitHS.FUnit.FMovePoints = 0;
                                                }
                                            }
                                        }
                                    }

                                    Settings.Free();
                                }

                                    break;
                                case TaiMoveType.aiMelee:
                                    Settings = new TCombatMoveControlSettings(null);
                                    var yhx2 = Move.UnitHS.GetYhx();
                                    Settings.SetSourcePos(Move.UnitHS.GetXhx(), yhx2, 0);
                                    Settings.SetDestinationPos(Move.X, Move.Y, 0);
                                    Settings.FArmyHS = Move.UnitHS;
                                    Settings.FMoveFlags = TSet<TCombatMoveModeElement>.Of(TCombatMoveModeElement.mmMelee);
                                    Move.UnitHS.FUnit.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                    Settings.FPlayer = Move.UnitHS.FUnit.GetPlayer();
                                    AoWTCUnit.AoWCombatMap.FMoveControl.ProcessMove(Settings);
                                    if (Settings.FPath.Count > 1)
                                    {
                                        Path = Settings.FPath;
                                        PathCost = MakeTerrainPath(Settings, ref Path, ref TerrainCost, Move.UnitHS.FUnit, null, false, false, 1);
                                        if (Path.Count > 1 && PathCost > -1)
                                        {
                                            if (Path[Path.Count - 2].Flag <= Move.UnitHS.FUnit.FMovePoints)
                                            {
                                                // { Original repeats X in the debug text. }
                                                AoWEngine.OutputDebugStr("AI touches at X: " + IntToStr(Move.X) + " Y: " + IntToStr(Move.X));
                                                Moved = Move.UnitHS.MeleeMoveTC((TMovepath)Path);
                                                if (Move.UnitHS.FUnit.FCombatPlayer == AoWTCUnit.AoWCombatMap.FSeatedPlayer)
                                                {
                                                    var yhx = Move.UnitHS.GetYhx();
                                                    var xhx = Move.UnitHS.GetXhx();
                                                    AoWTCUnit.AoWCombatMap.CenterViewTo(xhx, yhx, 0);
                                                }
                                            }
                                        }
                                    }

                                    Settings.Free();
                                    break;
                                case TaiMoveType.aiTouch:
                                    AoWHSSet.FAbilityControl.GetAbility(Move.Ability).GetCombatInfo(Move.UnitHS.FUnit.FUnit, ref AIInfo);
                                    Settings = new TCombatMoveControlSettings(null);
                                    var yhx3 = Move.UnitHS.GetYhx();
                                    Settings.SetSourcePos(Move.UnitHS.GetXhx(), yhx3, 0);
                                    Settings.SetDestinationPos(Move.X, Move.Y, 0);
                                    Settings.FArmyHS = Move.UnitHS;
                                    Settings.FMoveFlags = TSet<TCombatMoveModeElement>.Of(TCombatMoveModeElement.mmMelee);
                                    if (AIInfo.DamageTypes.Contains(TDamageType.dtWall))
                                    {
                                        Settings.FMoveFlags = Settings.FMoveFlags + TSet<TCombatMoveModeElement>.Of(TCombatMoveModeElement.mmWallCrushing);
                                    }

                                    Move.UnitHS.FUnit.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                    Settings.FPlayer = Move.UnitHS.FUnit.GetPlayer();
                                    Move.UnitHS.FSelectedAbility = Move.Ability;
                                    AoWTCUnit.AoWCombatMap.FMoveControl.ProcessMove(Settings);
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
                                                AoWEngine.OutputDebugStr("AI melees at X: " + IntToStr(Move.X) + " Y: " + IntToStr(Move.X));
                                                Moved = Move.UnitHS.AbTouchMoveTC((TMovepath)Path);
                                                if (Move.UnitHS.FUnit.FCombatPlayer == AoWTCUnit.AoWCombatMap.FSeatedPlayer)
                                                {
                                                    var yhx = Move.UnitHS.GetYhx();
                                                    var xhx = Move.UnitHS.GetXhx();
                                                    AoWTCUnit.AoWCombatMap.CenterViewTo(xhx, yhx, 0);
                                                }
                                            }
                                        }
                                    }

                                    Settings.Free();
                                    break;
                                case TaiMoveType.aiRanged:
                                    Move.UnitHS.FSelectedAbility = Move.Ability;
                                    Move.UnitHS.FSelectedSpell = Move.Spell;
                                    // { Original repeats X in the debug text. }
                                    AoWEngine.OutputDebugStr("AI ranged at X: " + IntToStr(Move.X) + " Y: " + IntToStr(Move.X));
                                    Moved = Move.UnitHS.AbRangedTC(Move.X, Move.Y);
                                    if (Move.UnitHS.FUnit.FCombatPlayer == AoWTCUnit.AoWCombatMap.FSeatedPlayer)
                                    {
                                        var yhx = Move.UnitHS.GetYhx();
                                        var xhx = Move.UnitHS.GetXhx();
                                        AoWTCUnit.AoWCombatMap.CenterViewTo(xhx, yhx, 0);
                                    }

                                    break;
                            }

                            ((TObject)Moves[0]).Free();
                            Moves.Delete(0);
                        }

                        if (!Moved)
                        {
                            Status = TaiStatus.aiDone;
                        }

                        I = 0;
                        int last = Moves.Count - 1;
                        for (; I <= last; I += 1)
                        {
                            ((TObject)Moves[I]).Free();
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

}

// Inherited world-map field layout
public partial class TAoWCombatMapField : TAoWMapField
{
    public TAoWCombatMapField(TECustomNode owner) : base(owner)
    {}

    // AoWTC.pas:2158
    public new const int ClassID = 2228481;

    // AoWTC.pas:2163
    public sbyte GetDefenseValue()
    {
        TMapFieldMsg Msg = default;
        Msg.MsgID = 135169;
        Msg.Result = -1;
        Msg.Param[0] = 0;
        SendMapFieldMsg(ref Msg);
        return (sbyte)Msg.Param[0].Int;
    }

}

// Inherited world-map level layout
public partial class TAoWCombatMapLevel : TAoWMapLevel
{
    public TAoWCombatMapLevel(TECustomNode owner) : base(owner)
    {}

    // AoWTC.pas:2173
    public new const int ClassID = 2228482;

    // AoWTC.pas:2178
    public override EClass MapFieldClass()
    {
        // { Original uses the world-map field class. }
        return EClass.Of<TAoWMapField>();
    }

}

// FMiscImages. FTargetImages. FFrame. FFrameCount. FSelection. FRange
public partial class TRangedPathHS : TFixedVisibleHS
{
    public TRangedPathHS(TECustomNode owner) : base(owner)
    {}

    // AoWTC.pas:4846
    public new const int ClassID = 2228485;

    public TCustomImageLibrary FMiscImages;
    public TCustomImageLibrary FTargetImages;
    public ushort FFrame;
    public ushort FFrameCount;
    public TCombatUnitSelectionControl FSelection;
    public ushort FRange;
    // AoWTC.pas:4849
    public override int GetShowPriority() => 1811939328;

    // AoWTC.pas:4859
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int X = default;
        int Y = default;
        int Count;
        int First;
        int Last;
        int NX = default;
        int NY = default;
        int PX = default;
        int PY = default;
        TfxType EffectType;
        if (FSelection != null)
        {
            if (FRange < 100)
            {
                Count = 6;
                First = 1;
                for (int I = 0, last = FRange - 2; I <= last; I++)
                {
                    First += Count;
                    Count += 6;
                }

                Last = First + Count;
                for (int I = First; I <= Last - 1; I++)
                {
                    var yhx = FSelection.GetTCUnitHS().GetYhx();
                    CenterHNtoHX(FSelection.GetTCUnitHS().GetXhx(), yhx, I, ref NX, ref NY);
                    HXtoHP(NX, NY, ref PX, ref PY);
                    FTargetImages[2].Show(PX - XhpSurf - 8, PY - YhpSurf + 2, SurfaceDesc);
                }
            }

            if (FSelection.FRangedPath.Count > 0)
            {
                FFrameCount = (ushort)(FFrameCount + 1);
                if (FFrameCount >= 5)
                {
                    FFrameCount = 0;
                    FFrame = (ushort)(FFrame + 1);
                }

                if (FFrame > 1)
                {
                    FFrame = 0;
                }

                if (FSelection.GetTCUnitHS().FSelectedAbility == 52)
                {
                    var tcUnitHS = FSelection.GetTCUnitHS();
                    EffectType = AoWTCUnit.SpellTypes[tcUnitHS.FSelectedSpell];
                }
                else
                {
                    var tcUnitHS = FSelection.GetTCUnitHS();
                    EffectType = AoWTCUnit.AbilTypes[tcUnitHS.FSelectedAbility];
                }

                switch (EffectType)
                {
                    case TfxType.fxBreath:
                        var count = FSelection.FRangedPath.Count;
                        var rangedPath = FSelection.FRangedPath[count - 1];
                        var fRangedPathCount = FSelection.FRangedPath.Count;
                        HPtoHX(FSelection.FRangedPath[fRangedPathCount - 1].X, rangedPath.Y, ref X, ref Y);
                    {
                        for (int I = 0; I <= 11; I++)
                        {
                            var yhx = FSelection.GetTCUnitHS().GetYhx();
                            var dHXtoHN2 = dHXtoHN(FSelection.GetTCUnitHS().GetXhx(), yhx, X, Y);
                            var yhx2 = FSelection.GetTCUnitHS().GetYhx();
                            CenterHNtoHX(FSelection.GetTCUnitHS().GetXhx(), yhx2, AoWTCUnit.BreathHN[dHXtoHN2 * 12 + I - 12], ref NX, ref NY);
                            HXtoHP(NX, NY, ref PX, ref PY);
                            FTargetImages[0].ShowClipped(PX - 8 - XhpSurf, PY + 2 - YhpSurf, SurfaceDesc);
                        }
                    }

                        break;
                    default:
                        switch (FSelection.GetTCUnitHS().FSelectedAbility)
                        {
                            case 52:
                                switch (FSelection.GetTCUnitHS().FSelectedSpell)
                                {
                                    case 104:
                                    case 106:
                                    case 116:
                                    case 117:
                                    case 120:
                                    case 129:
                                    case 130:
                                    {
                                        for (int I = 0; I <= 18; I++)
                                        {
                                            var count2 = FSelection.FRangedPath.Count;
                                            var rangedPathY = FSelection.FRangedPath[count2 - 1];
                                            var count3 = FSelection.FRangedPath.Count;
                                            HPtoHX(FSelection.FRangedPath[count3 - 1].X, rangedPathY.Y, ref X, ref Y);
                                            CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                            HXtoHP(NX, NY, ref PX, ref PY);
                                            FTargetImages[0].ShowClipped(PX - 8 - XhpSurf, PY + 2 - YhpSurf, SurfaceDesc);
                                        }
                                    }

                                        break;
                                    case 124:
                                    case 128:
                                    {
                                        for (int I = 0; I <= 6; I++)
                                        {
                                            var count2 = FSelection.FRangedPath.Count;
                                            var rangedPathY = FSelection.FRangedPath[count2 - 1];
                                            var count3 = FSelection.FRangedPath.Count;
                                            HPtoHX(FSelection.FRangedPath[count3 - 1].X, rangedPathY.Y, ref X, ref Y);
                                            CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                            HXtoHP(NX, NY, ref PX, ref PY);
                                            FTargetImages[0].ShowClipped(PX - 8 - XhpSurf, PY + 2 - YhpSurf, SurfaceDesc);
                                        }
                                    }

                                        break;
                                    default:
                                        var count4 = FSelection.FRangedPath.Count;
                                        var rangedPath2 = FSelection.FRangedPath[count4 - 1];
                                        var count5 = FSelection.FRangedPath.Count;
                                        var rangedPathX = FSelection.FRangedPath[count5 - 1];
                                        FMiscImages[FFrame + 3].ShowClipped(rangedPathX.X - 21 - XhpSurf, rangedPath2.Y - 20 - YhpSurf, SurfaceDesc);
                                        break;
                                }

                                break;
                            default:
                                var count6 = FSelection.FRangedPath.Count;
                                var rangedPath3 = FSelection.FRangedPath[count6 - 1];
                                var count7 = FSelection.FRangedPath.Count;
                                var rangedPath4 = FSelection.FRangedPath[count7 - 1];
                                FMiscImages[FFrame + 3].ShowClipped(rangedPath4.X - 21 - XhpSurf, rangedPath3.Y - 20 - YhpSurf, SurfaceDesc);
                                break;
                        }

                        break;
                }

                for (int I = 0, last = FSelection.FRangedPath.Count - 1; I <= last; I++)
                {
                    var rangedPathY = FSelection.FRangedPath[I];
                    var rangedPathX = FSelection.FRangedPath[I];
                    FMiscImages[2].Show(rangedPathX.X - XhpSurf, rangedPathY.Y - YhpSurf, SurfaceDesc);
                }

                for (int I = 0, last = FSelection.FRangedPath.Count - 1; I <= last; I++)
                {
                    var rangedPathY = FSelection.FRangedPath[I];
                    var rangedPathZ = FSelection.FRangedPath[I];
                    var rangedPathX = FSelection.FRangedPath[I];
                    FMiscImages[0].Show(rangedPathX.X - XhpSurf, rangedPathY.Y - rangedPathZ.Z - 8 - YhpSurf, SurfaceDesc);
                }

                for (int I = 0, last = FSelection.FObstacles.Count - 1; I <= last; I++)
                {
                    var obstacle = FSelection.FObstacles[I];
                    var rangedPath = FSelection.FRangedPath[((TROb)obstacle).PathIndex];
                    var obstacle2 = FSelection.FObstacles[I];
                    HPtoHX(FSelection.FRangedPath[((TROb)obstacle2).PathIndex].X, rangedPath.Y, ref NX, ref NY);
                    HXtoHP(NX, NY, ref PX, ref PY);
                    FMiscImages[1].Show(PX + 5 - XhpSurf, PY + 5 - YhpSurf, SurfaceDesc);
                    AoWEngine.FCombatFont.Color = 0;
                    var obstacle3 = FSelection.FObstacles[I];
                    AoWEngine.FCombatFont.SetTextCentered(PX + 16 - XhpSurf, PY + 12 - YhpSurf, IntToStr(((TROb)obstacle3).Chance), SurfaceDesc);
                }
            }
        }
    }

    // AoWTC.pas:4852
    public override void Activate()
    {
        base.Activate();
        FMiscImages = AoWHSSet.ImageLibraryManager.LinkToIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), "TCombat\\Misc.ILB");
        FTargetImages = AoWHSSet.ImageLibraryManager.LinkToIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), "TargHex.ILB");
    }

    // AoWTC.pas:4839
    public override void Deactivate()
    {
        AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), "TCombat\\Misc.ILB");
        AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), "TargHex.ILB");
        base.Deactivate();
    }

    // Engine.pas:524


}

// FDone. FCancelled. FFrame. FAbilityIndex. FSpellIndex. FPath. FVariant. FTargetX. FTargetY.
// FTarget. FEffectType
public partial class TfxHS : TFixedVisibleHS
{

    public TfxHS(TECustomNode owner) : base(owner)
    {
        FPath = new TXYZList();
        FFrame = -1;
    }

    // AoWTC.pas:5227
    public new const int ClassID = 2228486;

    public bool FDone;
    public bool FCancelled;
    public int FFrame;
    public int FAbilityIndex;
    public int FSpellIndex;
    public TXYZList FPath;
    public int FVariant;
    public int FTargetX;
    public int FTargetY;
    public TCombatObject FTarget;
    public TCombatAction FCombatAction;
    public TfxType FEffectType;
    // AoWTC.pas:5221
    public override void Destroy()
    {
        FPath.Free();
        base.Destroy();
    }

    // AoWTC.pas:5214


    // AoWTC.pas:5239
    public override int GetShowPriority() => 1811939328;

    // AoWTC.pas:5232
    public override void Activate()
    {
        base.Activate();
        AoWTCUnit.AoWCombatMap.NewFrameEvents.Add(NewFrame);
        FFrame = -1;
    }

    // AoWTC.pas:5208
    public override void Deactivate()
    {
        AoWTCUnit.AoWCombatMap.NewFrameEvents.Remove(NewFrame);
        base.Deactivate();
    }

    // AoWTC.pas:5244
    public void NewFrame(TObject Sender)
    {
        TfxHS Effect;
        int J = default;
        int I = default;
        TImageSequence Sequence;
        TXYZList NewPath;
        int K = default;
        int X = default;
        int Y = default;
        float Z = default;
        float Speed = default;
        float VZ = default;
        float Gravity = default;
        TfxHS Effect2;
        TTacticalCombatUnitHS UnitHS;
        TMapFieldMsg Msg = default;
        void MakeCloud(int Count, int Radius)
        {
            for (int N = 1; N <= Count - 1; N++)
            {
                Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxCloudPuff);
                Effect.FAbilityIndex = FAbilityIndex;
                Effect.FSpellIndex = FSpellIndex;
                var xyz = new TXYZ();
                Effect.FPath.Add(xyz);
                J = Random(70) + 10;
                var pi = Math.PI;
                var cos = Math.Cos((double)((double)J * pi) / 180);
                var random = Random(Radius);
                var pathX = FPath[0];
                Effect.FPath[0].X = pathX.X + Round((double)cos * (random - Radius / 2));
                var mathPI = Math.PI;
                var sin = Math.Sin((double)((double)J * mathPI) / 180);
                var random2 = Random(Radius);
                var pathY = FPath[0];
                Effect.FPath[0].Y = pathY.Y + Round((double)sin * (random2 - Radius / 2)) / 2;
            }
        }

        void MakeImpact()
        {
            Effect = null;
            var pathY = FPath[FFrame];
            HPtoHX(FPath[FFrame].X, pathY.Y, ref I, ref J);
            if (AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[I, J].TerrainType == 0)
            {
                Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxHitWater);
            }
            else
            {
                if (AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[I, J].OverlayType != 8 && AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[I, J].FindOwnedChild(2229508) == null && AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[I, J].FindOwnedChild(2229510) == null)
                {
                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxHitGround);
                }
            }

            if (Effect != null)
            {
                var xyz = new TXYZ();
                Effect.FPath.Add(xyz);
                var pathX = FPath[FFrame];
                Effect.FPath[0].X = pathX.X;
                var path = FPath[FFrame];
                Effect.FPath[0].Y = path.Y;
            }
        }

        try
        {
            if (FCancelled)
            {
                FFrame = -1;
            }
            else
            {
                FFrame += 1;
                switch (FEffectType)
                {
                    case TfxType.fxArrow:
                    case TfxType.fxRock:
                    case TfxType.fxComet:
                    case TfxType.fxBreath:
                    case TfxType.fxCometBall:
                    case TfxType.fxSwarm:
                        switch (FAbilityIndex)
                        {
                            case 44:
                            case 45:
                            case 57:
                            case 58:
                            case 123:
                                FFrame += 1;
                                break;
                            case 52:
                                switch (FSpellIndex)
                                {
                                    case 117:
                                        FFrame += 1;
                                        break;
                                }

                                break;
                        }

                        if (FFrame == 0 && AoWTCUnit.AoWCombatMap.FPlaySounds && FAbilityIndex == 52 && FSpellIndex == 122)
                        {
                            AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                        }

                        var count = FPath.Count;
                        if (FFrame >= count - 1)
                        {
                            if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                            {
                                if (FAbilityIndex == 52)
                                {
                                    AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            FFrame = FPath.Count - 1;
                            FDone = true;
                            if (FTarget == null)
                            {
                                MakeImpact();
                            }

                            var pathY = FPath[FFrame];
                            var pathZ = FPath[FFrame];
                            HPtoHX(FPath[FFrame].X, pathY.Y - pathZ.Z, ref I, ref J);
                            AoWTCUnit.AoWCombatMap.MakeVisible(I, J, 0, 2);
                        }

                        break;
                    case TfxType.fxDecayComet:
                        if (FFrame == 0 && AoWTCUnit.AoWCombatMap.FPlaySounds && FAbilityIndex == 52)
                        {
                            AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                        }

                        var fPathCount = FPath.Count;
                        if (FFrame >= fPathCount - 1)
                        {
                            if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                            {
                                if (FAbilityIndex == 52)
                                {
                                    AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            FFrame = FPath.Count - 1;
                            FDone = true;
                        }

                        break;
                    case TfxType.fxAltArrow:
                        if (FFrame == 0 && AoWTCUnit.AoWCombatMap.FPlaySounds && FAbilityIndex == 52)
                        {
                            AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                        }

                        FFrame += 3;
                        var count2 = FPath.Count;
                        if (FFrame >= count2 - 1)
                        {
                            if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                            {
                                if (FAbilityIndex == 52)
                                {
                                    AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            FFrame = FPath.Count - 1;
                            FDone = true;
                            if (FTarget == null)
                            {
                                MakeImpact();
                            }

                            var pathY = FPath[FFrame];
                            var pathZ = FPath[FFrame];
                            HPtoHX(FPath[FFrame].X, pathY.Y - pathZ.Z, ref I, ref J);
                            AoWTCUnit.AoWCombatMap.MakeVisible(I, J, 0, 2);
                        }

                        break;
                    case TfxType.fxScreenNS:
                        FFrame += 1;
                        var count3 = FPath.Count;
                        if (FFrame >= count3 - 1)
                        {
                            if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                            {
                                if (FAbilityIndex == 52)
                                {
                                    AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            FFrame = FPath.Count - 1;
                            FDone = true;
                            Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxHailImpact);
                            Effect.FAbilityIndex = FAbilityIndex;
                            Effect.FSpellIndex = FSpellIndex;
                            var xyz = new TXYZ();
                            Effect.FPath.Add(xyz);
                            var pathX = FPath[FFrame];
                            Effect.FPath[0].X = pathX.X;
                            var pathY = FPath[FFrame];
                            Effect.FPath[0].Y = pathY.Y;
                        }

                        break;
                    case TfxType.fxHailImpact:
                        if (FAbilityIndex == 52)
                        {
                            Sequence = AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[2];
                        }
                        else
                        {
                            Sequence = AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[2];
                        }

                        if (Sequence != null)
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames - 1)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }
                        else
                        {
                            FFrame = -1;
                        }

                        break;
                    case TfxType.fxPrayerGlow:
                        if (FAbilityIndex == 52)
                        {
                            Sequence = AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[1];
                        }
                        else
                        {
                            Sequence = AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[1];
                        }

                        if (Sequence != null)
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames - 1)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }
                        else
                        {
                            FFrame = -1;
                        }

                        break;
                    case TfxType.fxSFOverlay:
                        var count4 = FPath.Count;
                        if (FFrame >= count4 - 1)
                        {
                            FFrame = FPath.Count - 1;
                            FDone = true;
                        }

                        break;
                    case TfxType.fxScreenSwarm:
                        if (FFrame == 0)
                        {
                            FFrame = 1;
                        }

                        var count5 = FPath.Count;
                        if (FFrame >= count5 - 1)
                        {
                            if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                            {
                                if (FTarget != null && FAbilityIndex == 52)
                                {
                                    AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            FFrame = FPath.Count - 1;
                            FDone = true;
                        }

                        break;
                    case TfxType.fxRay:
                        FFrame += 12;
                        var count6 = FPath.Count;
                        if (FFrame >= count6 - 1)
                        {
                            if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                            {
                                if (FAbilityIndex == 52)
                                {
                                    AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            FFrame = FPath.Count - 1;
                            FDone = true;
                        }

                        break;
                    case TfxType.fxFrostBeam:
                        switch (FFrame)
                        {
                            case >= 0 and <= 999:
                                FFrame += 1;
                                var count7 = FPath.Count;
                                if (FFrame >= count7 - 1)
                                {
                                    FFrame = 999;
                                }

                                J = Random(5) + 5;
                                HXtoHP(FTargetX, FTargetY, ref K, ref X);
                            {
                                I = 0;
                                int last = J;
                                for (; I <= last; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    var random = Random(100);
                                    var random2 = Random(5);
                                    var random3 = Random(100);
                                    MakeArcPathXY((short)(K - 200), (short)(X - 200), (short)(random3 + K - 50), (short)(random + X - 50), (short)(random2 + 5), 100, ref Effect.FPath);
                                }
                            }

                                J = Random(10) + 10;
                            {
                                I = 0;
                                int last = J;
                                for (; I <= last; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    Speed = (float)((double)(Random(50) + 20) / 10);
                                    Gravity = (float)0.5;
                                    VZ = (float)((double)Random(10) / 10);
                                    Z = 100;
                                    var random = Random(360);
                                    MakeArcPath(K - 200, X - 100, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                }
                            }

                                break;
                            case >= 1000 and <= 1029:
                                HXtoHP(FTargetX, FTargetY, ref K, ref X);
                                J = Random(5) + 5;
                            {
                                I = 0;
                                int last = J;
                                for (; I <= last; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    var random = Random(100);
                                    var random2 = Random(5);
                                    var random3 = Random(100);
                                    MakeArcPathXY((short)(K - 200), (short)(X - 200), (short)(random3 + K - 50), (short)(random + X - 50), (short)(random2 + 5), 100, ref Effect.FPath);
                                }
                            }

                                J = Random(10) + 10;
                            {
                                I = 0;
                                int last = J;
                                for (; I <= last; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    Speed = (float)((double)(Random(50) + 20) / 10);
                                    Gravity = (float)0.5;
                                    VZ = (float)((double)Random(10) / 10);
                                    Z = 100;
                                    var random = Random(360);
                                    MakeArcPath(K - 200, X - 100, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                }
                            }

                                J = Random(5) + 5;
                            {
                                I = 0;
                                int last = J;
                                for (; I <= last; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    Speed = (float)((double)(Random(20) + 20) / 10);
                                    Gravity = (float)0.5;
                                    VZ = (float)((double)(Random(50) + 20) / 10);
                                    Z = 0;
                                    var count8 = FPath.Count;
                                    var path = FPath[count8 - 1];
                                    var count9 = FPath.Count;
                                    var pathZ = FPath[count9 - 1];
                                    var count10 = FPath.Count;
                                    var pathX = FPath[count10 - 1];
                                    var random = Random(360);
                                    MakeArcPath(pathX.X, path.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                }
                            }

                                break;
                            case 1030:
                                FFrame = 1999;
                                break;
                            case >= 2000 and <= 2999:
                                FFrame += 1;
                                var count11 = FPath.Count;
                                if (FFrame >= count11 + 2000 - 1)
                                {
                                    FFrame = 2999;
                                }

                                J = Random(5) + 5;
                            {
                                I = 0;
                                int last = J;
                                for (; I <= last; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    Speed = (float)((double)(Random(20) + 20) / 10);
                                    Gravity = (float)0.5;
                                    VZ = (float)((double)(Random(50) + 20) / 10);
                                    Z = 0;
                                    var count8 = FPath.Count;
                                    var path = FPath[count8 - 1];
                                    var count9 = FPath.Count;
                                    var pathZ = FPath[count9 - 1];
                                    var count10 = FPath.Count;
                                    var pathX = FPath[count10 - 1];
                                    var random = Random(360);
                                    MakeArcPath(pathX.X, path.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                }
                            }

                                break;
                            case 3000:
                                if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                    }
                                }

                            {
                                I = 0;
                                int last = FPath.Count - 1;
                                for (; I <= last; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxFrost);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    Speed = (float)((double)(Random(25) + 25) / 10);
                                    Gravity = (float)0.5;
                                    VZ = (float)((double)(Random(50) + 40) / 10);
                                    Z = 0;
                                    var pathY = FPath[I];
                                    var pathZ = FPath[I];
                                    var random = Random(360);
                                    MakeArcPath(FPath[I].X, pathY.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                }
                            }

                                J = Random(10) + 100;
                            {
                                I = 0;
                                int last = J;
                                for (; I <= last; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxFrost);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    Speed = (float)((double)(Random(50) + 50) / 10);
                                    Gravity = (float)0.1;
                                    VZ = (float)((double)(Random(5) + 5) / 10);
                                    Z = 0;
                                    var count8 = FPath.Count;
                                    var path = FPath[count8 - 1];
                                    var count9 = FPath.Count;
                                    var pathZ = FPath[count9 - 1];
                                    var count10 = FPath.Count;
                                    var pathX = FPath[count10 - 1];
                                    var random = Random(360);
                                    MakeArcPath(pathX.X, path.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                }
                            }

                                FDone = true;
                                FTarget = null;
                                break;
                        }

                        break;
                    case TfxType.fxShockWave:
                        switch (FFrame)
                        {
                            case >= 0 and <= 29:
                                if (FFrame == 0 && AoWTCUnit.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                                    }
                                    else
                                    {
                                        AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                                    }
                                }

                            {
                                I = 1;
                                for (; I <= 20; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    var random = Random(360);
                                    var pathX = FPath[0];
                                    var pathY = FPath[0];
                                    var random2 = Random(2);
                                    var random3 = Random(10);
                                    MakeSwarmPath(Random(10) + 10, random3 + FFrame / 4, I % 6 * 60 + FFrame, random, pathX.X, pathY.Y, (TSwarmDirection)random2, ref Effect.FPath);
                                }
                            }

                                break;
                            case >= 30 and <= 59:
                            {
                                I = 1;
                                for (; I <= 20; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    var random = Random(360);
                                    var pathX = FPath[0];
                                    var pathY = FPath[0];
                                    var random2 = Random(2);
                                    var random3 = Random(10);
                                    MakeSwarmPath(Random(10) + 10, random3 + (60 - FFrame) / 4, I % 6 * 60 + FFrame, random, pathX.X, pathY.Y, (TSwarmDirection)random2, ref Effect.FPath);
                                }
                            }

                                break;
                            case >= 60 and <= 89:
                            {
                                I = 1;
                                for (; I <= 20; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    var random = Random(360);
                                    var pathX = FPath[0];
                                    var pathY = FPath[0];
                                    var random2 = Random(2);
                                    var random3 = Random(10);
                                    MakeSwarmPath(Random(10) + 10, random3 + (FFrame - 60) / 2, I % 6 * 60 + FFrame, random, pathX.X, pathY.Y, (TSwarmDirection)random2, ref Effect.FPath);
                                }
                            }

                                break;
                            case >= 90 and <= 119:
                            {
                                I = 1;
                                for (; I <= 20; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    var random = Random(360);
                                    var pathX = FPath[0];
                                    var pathY = FPath[0];
                                    var random2 = Random(2);
                                    var random3 = Random(10);
                                    MakeSwarmPath(Random(10) + 10, random3 + (120 - FFrame) / 2, I % 6 * 60 + FFrame, random, pathX.X, pathY.Y, (TSwarmDirection)random2, ref Effect.FPath);
                                }
                            }

                                break;
                            case 150:
                                if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                    }
                                }

                                X = 200;
                                Y = 200;
                                J = Random(5);
                            {
                                I = 0;
                                for (; I <= 359; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    X = -X;
                                    var pi = Math.PI;
                                    var sin = Math.Sin((double)((double)(I + J) * pi) / 180);
                                    var random = Random(10);
                                    var pathY = FPath[0];
                                    var mathPI = Math.PI;
                                    var cos = Math.Cos((double)((double)(I + J) * mathPI) / 180);
                                    var random2 = Random(10);
                                    var pathX = FPath[0];
                                    var path = FPath[0];
                                    MakeLinePath(FPath[0].X, path.Y, pathX.X + Round((double)cos * (random2 + X)), pathY.Y + Round((double)sin * (random + Y)) / 2, 75, ref Effect.FPath);
                                    if (I % 10 == 0)
                                    {
                                        K = 1;
                                        for (; K <= 10; K += 1)
                                        {
                                            Effect2 = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                            Effect2.FAbilityIndex = FAbilityIndex;
                                            Effect2.FSpellIndex = FSpellIndex;
                                            var count7 = Effect.FPath.Count;
                                            var path2 = Effect.FPath[count7 - 1];
                                            var count8 = Effect.FPath.Count;
                                            var path3 = Effect.FPath[count8 - 1];
                                            var path4 = FPath[0];
                                            MakeLinePath(FPath[0].X, path4.Y, path3.X, path2.Y, K * 5 + 75, ref Effect2.FPath);
                                        }
                                    }
                                }
                            }

                                break;
                            case >= 151 and <= 169:
                            case >= 210 and <= 229:
                                var path5 = FPath[0];
                                HPtoHX(FPath[0].X, path5.Y, ref X, ref Y);
                                Msg.MsgID = 200720;
                                Msg.Param[0] = 1;
                                Msg.Result = -1;
                            {
                                I = 1;
                                for (; I <= 36; I += 1)
                                {
                                    CenterHNtoHX(X, Y, I, ref J, ref K);
                                    AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[J, K].SendMapFieldMsg(ref Msg);
                                }
                            }

                                break;
                            case >= 170 and <= 184:
                            case >= 195 and <= 209:
                                var path6 = FPath[0];
                                HPtoHX(FPath[0].X, path6.Y, ref X, ref Y);
                                Msg.MsgID = 200720;
                                Msg.Param[0] = 2;
                                Msg.Result = -1;
                            {
                                I = 1;
                                for (; I <= 36; I += 1)
                                {
                                    CenterHNtoHX(X, Y, I, ref J, ref K);
                                    AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[J, K].SendMapFieldMsg(ref Msg);
                                }
                            }

                                break;
                            case >= 185 and <= 194:
                                var path7 = FPath[0];
                                HPtoHX(FPath[0].X, path7.Y, ref X, ref Y);
                                Msg.MsgID = 200720;
                                Msg.Param[0] = 3;
                                Msg.Result = -1;
                            {
                                I = 1;
                                for (; I <= 36; I += 1)
                                {
                                    CenterHNtoHX(X, Y, I, ref J, ref K);
                                    AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[J, K].SendMapFieldMsg(ref Msg);
                                }
                            }

                                break;
                            case 230:
                                var path8 = FPath[0];
                                HPtoHX(FPath[0].X, path8.Y, ref X, ref Y);
                                Msg.MsgID = 200721;
                                Msg.Result = -1;
                            {
                                I = 1;
                                for (; I <= 36; I += 1)
                                {
                                    CenterHNtoHX(X, Y, I, ref J, ref K);
                                    AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[J, K].SendMapFieldMsg(ref Msg);
                                }
                            }

                                FFrame = -1;
                                break;
                        }

                        break;
                    case TfxType.fxWinds:
                        switch (FFrame)
                        {
                            case >= 0 and <= 1999:
                                var count7 = FPath.Count;
                                if (FFrame >= count7 + 20)
                                {
                                    if (FTarget != null && FTarget.GetEnabled())
                                    {
                                        if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                                        {
                                            if (FAbilityIndex == 52)
                                            {
                                                AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                            }
                                            else
                                            {
                                                AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                            }
                                        }

                                        var count8 = FPath.Count;
                                        var path = FPath[count8 - 1];
                                        var count9 = FPath.Count;
                                        HPtoHX(FPath[count9 - 1].X, path.Y, ref J, ref K);
                                        var mapField = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[J, K];
                                        ((TTacticalCombatUnit)FTarget).FUnitHS.MoveTo(mapField, 0);
                                        ((TTacticalCombatUnit)FTarget).FUnitHS.FOffsetX = 0;
                                        ((TTacticalCombatUnit)FTarget).FUnitHS.FOffsetY = 0;
                                        ((TTacticalCombatUnit)FTarget).FUnitHS.FBusy = false;
                                    }

                                    FFrame = -1;
                                }
                                else
                                {
                                    if (AoWTCUnit.AoWCombatMap.FPlaySounds && (FFrame == FTargetX || FFrame == FTargetY))
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                                        }
                                        else
                                        {
                                            AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                                        }
                                    }

                                    if (FTarget != null && FTarget.GetEnabled())
                                    {
                                        var count8 = FPath.Count;
                                        if (FFrame - 20 < count8 && FFrame >= 20)
                                        {
                                            var pathX = FPath[FFrame - 20];
                                            var path = FPath[0];
                                            ((TTacticalCombatUnit)FTarget).FUnitHS.FOffsetX = pathX.X - path.X;
                                            var pathY = FPath[FFrame - 20];
                                            var pathZ = FPath[FFrame - 20];
                                            var path2 = FPath[0];
                                            ((TTacticalCombatUnit)FTarget).FUnitHS.FOffsetY = pathY.Y - pathZ.Z - path2.Y;
                                            ((TTacticalCombatUnit)FTarget).FUnitHS.SetDirection((byte)(((TTacticalCombatUnit)FTarget).FUnitHS.FDirection + 1));
                                            if (((TTacticalCombatUnit)FTarget).FUnitHS.FDirection > 6)
                                            {
                                                ((TTacticalCombatUnit)FTarget).FUnitHS.SetDirection(1);
                                            }

                                            ((TTacticalCombatUnit)FTarget).FUnitHS.FBusy = true;
                                        }
                                    }

                                    I = FFrame - 20;
                                    int last = FFrame;
                                    for (; I <= last; I += 1)
                                    {
                                        bool condition = I >= 0;
                                        if (condition)
                                        {
                                            var count8 = FPath.Count;
                                            condition = I < count8;
                                        }

                                        if (condition)
                                        {
                                            Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var random = Random(360);
                                            var pathX = FPath[I];
                                            var random2 = Random(10);
                                            var pathY = FPath[I];
                                            var pathZ = FPath[I];
                                            var random3 = Random(10);
                                            var random4 = Random(2);
                                            var count8 = FPath.Count;
                                            MakeSwarmPath(Random(5) + 20, (count8 - I) / 5, 0, random, pathX.X + random2 - 5, pathY.Y - pathZ.Z + random3 - 5, (TSwarmDirection)random4, ref Effect.FPath);
                                        }
                                    }
                                }

                                break;
                        }

                        break;
                    case TfxType.fxSacFlame:
                        switch (FFrame)
                        {
                            case >= 0 and <= 99:
                                if (FFrame == 0 && AoWTCUnit.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                                    }
                                    else
                                    {
                                        AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                                    }
                                }

                                J = Random(6) + 10;
                                var yhx = ((TTacticalCombatUnit)FTarget).FUnitHS.GetYhx();
                                HXtoHP(((TTacticalCombatUnit)FTarget).FUnitHS.GetXhx(), yhx, ref K, ref X);
                            {
                                I = 1;
                                int last = J - 1;
                                for (; I <= last; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxFrost);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    Speed = (float)((double)(Random(25) + 25) / 10);
                                    Gravity = (float)0.5;
                                    VZ = (float)((double)(Random(50) + FFrame) / 10);
                                    Z = 0;
                                    MakeArcPath(K + 16, X + 16, I * 360 / J, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                }
                            }

                                switch (FFrame)
                                {
                                    case 0:
                                    case 20:
                                    case 40:
                                    case 60:
                                    case 75:
                                    case 85:
                                    case 90:
                                    case 94:
                                    case 97:
                                    case 99:
                                        if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                                        {
                                            if (FAbilityIndex == 52)
                                            {
                                                AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                            }
                                            else
                                            {
                                                AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                            }
                                        }

                                        J = Random(10) + 100;
                                    {
                                        I = 0;
                                        int last = J;
                                        for (; I <= last; I += 1)
                                        {
                                            Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxFrost);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            Speed = (float)((double)(Random(50) + 50) / 10);
                                            Gravity = (float)0.1;
                                            VZ = (float)((double)(Random(5) + 5) / 10);
                                            Z = 0;
                                            var random = Random(360);
                                            MakeArcPath(K + 16, X + 16, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                        }
                                    }

                                        break;
                                }

                                break;
                            case >= 125 and <= 199:
                                if (FFrame == 125)
                                {
                                    var path = FPath[0];
                                    HPtoHX(FPath[0].X, path.Y, ref I, ref J);
                                    AoWTCUnit.AoWCombatMap.CenterViewTo(I, J - 3, 0);
                                }

                                if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                    }
                                }

                                J = Random(6) + 10;
                                var pathX = FPath[0];
                                var random2 = Random(160);
                                K = pathX.X + random2 - 80;
                                var pathY = FPath[0];
                                var random3 = Random(80);
                                X = pathY.Y + random3 - 40;
                            {
                                I = 1;
                                int last = J - 1;
                                for (; I <= last; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxFrost);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    Speed = (float)((double)(Random(50) + 25) / 10);
                                    Gravity = (float)0.5;
                                    VZ = (float)((double)(Random(50) + 100) / 10);
                                    Z = 0;
                                    MakeArcPath(K, X, I * 360 / J, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                }
                            }

                                J = Random(10) + 50;
                            {
                                I = 0;
                                int last = J;
                                for (; I <= last; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxFrost);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    Speed = (float)((double)(Random(50) + 50) / 10);
                                    Gravity = (float)0.1;
                                    VZ = (float)((double)(Random(5) + 5) / 10);
                                    Z = 0;
                                    var random = Random(360);
                                    MakeArcPath(K, X, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                }
                            }

                                break;
                            case 200:
                                FFrame = -1;
                                break;
                        }

                        break;
                    case TfxType.fxMindDecay:
                        switch (FFrame)
                        {
                            case >= 0 and <= 49:
                                if (FFrame % 2 > 0)
                                {
                                    I = 0;
                                    int last = FFrame;
                                    for (; I <= last; I += 1)
                                    {
                                        J = 0;
                                        int limit = I / 25;
                                        for (; J <= limit; J += 1)
                                        {
                                            Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var random = Random(360);
                                            var pathX = FPath[0];
                                            var pathY = FPath[0];
                                            var random2 = Random(2);
                                            MakeSwarmPath(Random(10) + 10, I / 2, 45, random, pathX.X + 100 - I, pathY.Y - 200 - 50 + I, (TSwarmDirection)random2, ref Effect.FPath);
                                        }
                                    }
                                }

                                break;
                            case >= 50 and <= 149:
                                if (FFrame % 2 > 0)
                                {
                                    I = 0;
                                    for (; I <= 50; I += 1)
                                    {
                                        J = 0;
                                        int last = I / 25;
                                        for (; J <= last; J += 1)
                                        {
                                            Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var random = Random(360);
                                            var pathX = FPath[0];
                                            var pathY = FPath[0];
                                            var random2 = Random(2);
                                            MakeSwarmPath(Random(10) + 10, I / 2, 45, random, pathX.X + 100 - I, pathY.Y - 200 - 50 + I, (TSwarmDirection)random2, ref Effect.FPath);
                                        }
                                    }
                                }

                                if (FFrame % 5 == 0 && FFrame < 85)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxDecayComet);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    var pathY = FPath[0];
                                    var random = Random(50);
                                    var random2 = Random(5);
                                    var pathX = FPath[0];
                                    var random3 = Random(50);
                                    var path = FPath[0];
                                    var random4 = Random(50);
                                    var path2 = FPath[0];
                                    var random5 = Random(50);
                                    MakeArcPathXY((short)(path2.X + random5 + 25), (short)(path.Y + random4 - 225), (short)(pathX.X + random3 - 25), (short)(pathY.Y + random - 25), (short)(random2 + 5), 100, ref Effect.FPath);
                                    I = 0;
                                    for (; I <= 10; I += 1)
                                    {
                                        Speed = (float)((double)(Random(50) + 50) / 10);
                                        Gravity = (float)1.5;
                                        VZ = (float)((double)(Random(25) + 25) / 10);
                                        Z = 0;
                                        var count7 = Effect.FPath.Count;
                                        var path3 = Effect.FPath[count7 - 1];
                                        var count8 = Effect.FPath.Count;
                                        var pathZ = Effect.FPath[count8 - 1];
                                        var count9 = Effect.FPath.Count;
                                        var path4 = Effect.FPath[count9 - 1];
                                        var random6 = Random(360);
                                        MakeArcPath(path4.X, path3.Y - pathZ.Z, random6, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }

                                    CenterHNtoHX(FTargetX, FTargetY, (FFrame - 50) / 5, ref J, ref K);
                                    // { Original swaps the converted hex coordinates. }
                                    HXtoHP(J, K, ref X, ref Y);
                                    UnitHS = (TTacticalCombatUnitHS)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[J, K].FindOwnedChild(2228484);
                                    if (UnitHS != null && UnitHS.FUnit.GetSide() != FTarget.GetSide())
                                    {
                                        Effect.FTarget = UnitHS.FUnit;
                                    }
                                    else
                                    {
                                        Effect.FTarget = null;
                                    }

                                    if (Effect.FTarget != null)
                                    {
                                        var random6 = Random(3);
                                        var count7 = Effect.FPath.Count;
                                        var path3 = Effect.FPath[count7 - 1];
                                        var count8 = Effect.FPath.Count;
                                        var pathZ = Effect.FPath[count8 - 1];
                                        var count9 = Effect.FPath.Count;
                                        MakeArcPathXY((short)Effect.FPath[count9 - 1].X, (short)(path3.Y - pathZ.Z), (short)(X + 16), (short)(Y - 8), (short)(random6 + 6), 100, ref Effect.FPath);
                                        I = 0;
                                        int last = Random(3) + 5;
                                        for (; I <= last; I += 1)
                                        {
                                            var count10 = Effect.FPath.Count;
                                            var path4 = Effect.FPath[count10 - 1];
                                            var count11 = Effect.FPath.Count;
                                            var path5 = Effect.FPath[count11 - 1];
                                            var count12 = Effect.FPath.Count;
                                            var path6 = Effect.FPath[count12 - 1];
                                            var count13 = Effect.FPath.Count;
                                            var path7 = Effect.FPath[count13 - 1];
                                            var count14 = Effect.FPath.Count;
                                            var path8 = Effect.FPath[count14 - 1];
                                            var count15 = Effect.FPath.Count;
                                            MakeLinePath(Effect.FPath[count15 - 1].X, path6.Y - path7.Z, path8.X, path4.Y - path5.Z - 6, 3, ref Effect.FPath);
                                            var count16 = Effect.FPath.Count;
                                            var path9 = Effect.FPath[count16 - 1];
                                            var count17 = Effect.FPath.Count;
                                            var item = Effect.FPath[count17 - 1];
                                            var count18 = Effect.FPath.Count;
                                            var fPathItem = Effect.FPath[count18 - 1];
                                            var count19 = Effect.FPath.Count;
                                            var path10 = Effect.FPath[count19 - 1];
                                            var count20 = Effect.FPath.Count;
                                            var path11 = Effect.FPath[count20 - 1];
                                            var count21 = Effect.FPath.Count;
                                            MakeLinePath(Effect.FPath[count21 - 1].X, fPathItem.Y - path10.Z, path11.X, path9.Y - item.Z + 6, 3, ref Effect.FPath);
                                        }
                                    }
                                }

                                break;
                            case 200:
                                FFrame = -1;
                                FTarget = null;
                                break;
                        }

                        break;
                    case TfxType.fxChain:
                        NewPath = new TXYZList();
                        I = 0;
                        J = 0;
                        K = 0;
                        while (true)
                        {
                            var count7 = FPath.Count;
                            if (!(I < count7))
                                break;
                            if (FPath[I].Z > 0)
                            {
                                if (FPath[I].Z > 1)
                                {
                                    K = FPath[I].Z;
                                }

                                var pathY = FPath[I];
                                var pathX = FPath[I];
                                var path = FPath[J];
                                MakeLightningPath(FPath[J].X, path.Y, pathX.X, pathY.Y, ref NewPath);
                                switch (K)
                                {
                                    case -1:
                                        var newPathCount = NewPath.Count;
                                        NewPath[newPathCount - 1].Z = 16;
                                        FFrame = NewPath.Count - 1;
                                        K = 0;
                                        break;
                                    case >= 2 and <= 16:
                                        var count8 = NewPath.Count;
                                        NewPath[count8 - 1].Z = K - 1;
                                        FFrame = NewPath.Count - 1;
                                        if (K == 2)
                                        {
                                            K = -1;
                                        }
                                        else
                                        {
                                            K = 0;
                                        }

                                        break;
                                }

                                J = I;
                            }

                            I += 1;
                        }

                        FPath.Free();
                        FPath = NewPath;
                        var count22 = FPath.Count;
                        if (FFrame >= count22 - 1)
                        {
                            FFrame = FPath.Count - 1;
                            if (FPath[FFrame].Z <= 1)
                            {
                                FDone = true;
                            }
                        }

                        if (FPath[FFrame].Z == 15)
                        {
                            if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                            {
                                if (FAbilityIndex == 52)
                                {
                                    AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                }
                                else
                                {
                                    AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                }
                            }

                            var pathY = FPath[FFrame];
                            var pathZ = FPath[FFrame];
                            HPtoHX(FPath[FFrame].X, pathY.Y - pathZ.Z, ref I, ref J);
                            AoWTCUnit.AoWCombatMap.MakeVisible(I, J, 0, 3);
                        }

                        break;
                    case TfxType.fxTouch:
                    case TfxType.fxEffect:
                        if (FFrame == 0 && AoWTCUnit.AoWCombatMap.FPlaySounds && FAbilityIndex == 65)
                        {
                            AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                        }

                        if (FAbilityIndex == 52)
                        {
                            Sequence = AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[0];
                        }
                        else
                        {
                            Sequence = AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[0];
                        }

                        if (Sequence != null)
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames - 1)
                            {
                                if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                    }
                                }

                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }
                        else
                        {
                            FFrame = -1;
                        }

                        break;
                    case TfxType.fxSacredWrath:
                        if (FAbilityIndex == 52)
                        {
                            Sequence = AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[1];
                        }
                        else
                        {
                            Sequence = AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[1];
                        }

                        if (Sequence == null)
                        {
                            FFrame = -1;
                        }

                        break;
                    case TfxType.fxFBExplosion:
                        Sequence = AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[1];
                        if (Sequence != null)
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames - 1)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }
                        else
                        {
                            FFrame = -1;
                        }

                        break;
                    case TfxType.fxScreenNESW:
                        if (FFrame == 0)
                        {
                            if (FPath[0].X == 0)
                            {
                                FPath[0].X = 100;
                                var random = Random(50);
                                FPath[0].Y = random;
                            }
                            else
                            {
                                var random = Random(50);
                                FPath[0].X = random + 50;
                                FPath[0].Y = 0;
                            }
                        }
                        else
                        {
                            var pathX = FPath[0];
                            pathX.X -= 3;
                            var pathY = FPath[0];
                            pathY.Y += 3;
                            if (FPath[0].X < 0)
                            {
                                FFrame = -1;
                            }
                            else
                            {
                                if (FPath[0].Y > 100)
                                {
                                    FFrame = -1;
                                }
                            }
                        }

                        break;
                    case TfxType.fxDamLightning:
                    case TfxType.fxDamFire:
                    case TfxType.fxDamPoison:
                    case TfxType.fxDamMagic:
                    case TfxType.fxDamHoly:
                    case TfxType.fxDamDeath:
                    case TfxType.fxDamCold:
                    case TfxType.fxGeyserWater:
                    case TfxType.fxFrost:
                    case TfxType.fxSacFire:
                        var count23 = FPath.Count;
                        if (FFrame >= count23 - 1)
                        {
                            FFrame = FPath.Count - 1;
                            FDone = true;
                        }

                        break;
                    case TfxType.fxWallSmoke:
                        Sequence = AoWHSSet.FImageSequences.ImageSequences[5];
                        if (Sequence == null)
                        {
                            FFrame = -1;
                        }
                        else
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }

                        break;
                    case TfxType.fxHitGround:
                        Sequence = AoWHSSet.FImageSequences.ImageSequences[9];
                        if (Sequence == null)
                        {
                            FFrame = -1;
                        }
                        else
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }

                        break;
                    case TfxType.fxHitWater:
                        Sequence = AoWHSSet.FImageSequences.ImageSequences[10];
                        if (Sequence == null)
                        {
                            FFrame = -1;
                        }
                        else
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }

                        break;
                    case TfxType.fxBlood:
                        var pathZ2 = FPath[0];
                        Sequence = AoWHSSet.FImageSequences.ImageSequences[pathZ2.Z];
                        if (Sequence == null)
                        {
                            FFrame = -1;
                        }
                        else
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames - 1)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }

                        break;
                    case TfxType.fxTremors:
                        switch (FFrame)
                        {
                            case 0:
                                var xyz = new TXYZ();
                                FPath.Add(xyz);
                                FPath[0].X = AoWTCUnit.AoWCombatMap.ViewXhp;
                                FPath[0].Y = AoWTCUnit.AoWCombatMap.ViewYhp;
                                break;
                            case >= 1 and <= 29:
                                var pathY = FPath[0];
                                var random = Random(FFrame / 3);
                                var pathX = FPath[0];
                                var random2 = Random(FFrame / 3);
                                AoWTCUnit.AoWCombatMap.ChangeViewTo(pathX.X + random2 - FFrame / 6, pathY.Y + random - FFrame / 6, AoWTCUnit.AoWCombatMap.ViewL);
                                break;
                            case >= 30 and <= 89:
                                var path = FPath[0];
                                var random3 = Random(10);
                                var path2 = FPath[0];
                                var random4 = Random(10);
                                AoWTCUnit.AoWCombatMap.ChangeViewTo(path2.X + random4 - 5, path.Y + random3 - 5, AoWTCUnit.AoWCombatMap.ViewL);
                                break;
                            case >= 90 and <= 119:
                                var path3 = FPath[0];
                                var random5 = Random((119 - FFrame) / 3);
                                var path4 = FPath[0];
                                var random6 = Random((119 - FFrame) / 3);
                                AoWTCUnit.AoWCombatMap.ChangeViewTo(path4.X + random6 - (119 - FFrame) / 6, path3.Y + random5 - (119 - FFrame) / 6, AoWTCUnit.AoWCombatMap.ViewL);
                                break;
                            case 120:
                                FFrame = -1;
                                break;
                        }

                        break;
                    case TfxType.fxCloud:
                        switch (FFrame)
                        {
                            case 250:
                                FFrame = -1;
                                break;
                            case >= 1 and <= 149:
                                MakeCloud(FFrame, FFrame);
                                break;
                            case >= 150 and <= 199:
                                MakeCloud(150, 150);
                                break;
                            case >= 200 and <= 249:
                                MakeCloud((250 - FFrame) * 3, 150);
                                break;
                        }

                        break;
                    case TfxType.fxCloudPuff:
                        if (FAbilityIndex == 52)
                        {
                            Sequence = AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[0];
                        }
                        else
                        {
                            Sequence = AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[0];
                        }

                        if (Sequence != null)
                        {
                            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                            if (FFrame >= totalSequenceFrames - 1)
                            {
                                FFrame = Sequence.GetTotalSequenceFrames() - 1;
                                FDone = true;
                            }
                        }
                        else
                        {
                            FFrame = -1;
                        }

                        break;
                    case TfxType.fxHighPrayer:
                        switch (FFrame)
                        {
                            case 0:
                            case 5:
                            case 10:
                            case 15:
                            case 20:
                            case 25:
                            case 30:
                            case 35:
                            case 40:
                            case 45:
                            case 50:
                            case 55:
                            case 60:
                            case 65:
                            case 70:
                            case 75:
                            case 80:
                            case 85:
                            case 90:
                            case 95:
                            {
                                I = 0;
                                int last = AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                                for (; I <= last; I += 1)
                                {
                                    if (AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I) is TCombatUnit && AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I).GetEnabled() && AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I).FCombatPlayer == FTarget.FCombatPlayer)
                                    {
                                        Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                        var yhx = ((TTacticalCombatUnit)AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
                                        HXtoHP(((TTacticalCombatUnit)AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx(), yhx, ref J, ref K);
                                        J = Random(20) + (J + 6);
                                        K = Random(20) + (K + 6);
                                        MakeLinePath(J, K - 100, J, K, 50, ref Effect.FPath);
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                    }
                                }
                            }

                                break;
                            case 100:
                            {
                                I = 0;
                                int last = AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                                for (; I <= last; I += 1)
                                {
                                    if (AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I) is TCombatUnit && AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I).GetEnabled() && AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I).FCombatPlayer == FTarget.FCombatPlayer)
                                    {
                                        Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxPrayerGlow);
                                        var yhx = ((TTacticalCombatUnit)AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
                                        HXtoHP(((TTacticalCombatUnit)AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx(), yhx, ref J, ref K);
                                        var xyz = new TXYZ();
                                        Effect.FPath.Add(xyz);
                                        Effect.FPath[0].X = J + 16;
                                        Effect.FPath[0].Y = K + 16;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var combatObject = AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                        Effect.FTarget = combatObject;
                                    }
                                }
                            }

                                FFrame = -1;
                                FTarget = null;
                                break;
                        }

                        break;
                    case TfxType.fxResurrect:
                        switch (FFrame)
                        {
                            case 100:
                                FFrame = -1;
                                break;
                        }

                        break;
                    case TfxType.fxDecayLeg:
                        var count24 = FPath.Count;
                        if (FFrame >= count24)
                        {
                            FFrame = -1;
                        }

                        break;
                    case TfxType.fxGeyser:
                        switch (FFrame)
                        {
                            case 150:
                                FFrame = -1;
                                if (FTarget != null && FTarget.GetEnabled())
                                {
                                    if (AoWTCUnit.AoWCombatMap.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                        }
                                    }

                                    ((TTacticalCombatUnit)FTarget).FUnitHS.FOffsetY = 0;
                                    ((TTacticalCombatUnit)FTarget).FUnitHS.FBusy = false;
                                }

                                if (FTarget != null && ((TTacticalCombatUnit)FTarget).FUnit.GetMoveTypes().Contains(TMoveType.mtFlying))
                                {
                                    FTarget = null;
                                }

                                break;
                            case >= 0 and <= 99:
                                if (AoWTCUnit.AoWCombatMap.FPlaySounds && FFrame % 30 == 29)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                                    }
                                    else
                                    {
                                        AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                                    }
                                }

                                if (FTarget != null && FTarget.GetEnabled())
                                {
                                    var random = Random(2);
                                    ((TTacticalCombatUnit)FTarget).FUnitHS.FOffsetY = random + -(FFrame * 3 / 2) - 1;
                                    ((TTacticalCombatUnit)FTarget).FUnitHS.FBusy = true;
                                }

                                J = Random(6) + 20;
                            {
                                I = 1;
                                int last = J - 1;
                                for (; I <= last; I += 1)
                                {
                                    Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                                    Effect.FAbilityIndex = FAbilityIndex;
                                    Effect.FSpellIndex = FSpellIndex;
                                    Speed = (float)((double)(Random(10) + 10) / 10);
                                    Gravity = (float)0.5;
                                    VZ = (float)((double)((double)Random(50) + (double)(FFrame * 3) * 0.5) / 10);
                                    Z = 0;
                                    var path = FPath[0];
                                    MakeArcPath(FPath[0].X, path.Y, I * 360 / J, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                }
                            }

                                break;
                            case >= 125 and <= 149:
                                if (FTarget != null && FTarget.GetEnabled())
                                {
                                    ((TTacticalCombatUnit)FTarget).FUnitHS.FOffsetY = (FFrame - 150) * 6;
                                    ((TTacticalCombatUnit)FTarget).FUnitHS.SetDirection((byte)(FFrame % 6 + 1));
                                }

                                break;
                        }

                        break;
                    case TfxType.fxSWComet:
                        break;
                    default:
                        FFrame = -1;
                        break;
                }
            }

            if (FFrame == -1)
            {
                FDone = true;
            }
        }
        catch
        {
            // { Original puts the diagnostic in the message-box caption. }
            MessageBox(0, "Error during NewFrame", NullTerminatedText("Error - fxHS " + IntToStr(FAbilityIndex) + " " + IntToStr(FSpellIndex)), 0);
        }
    }

    // AoWTC.pas:196


}

public partial class TCItemHS : TItemHS
{

    public TCItemHS(TECustomNode owner) : base(owner)
    {}

    // AoWTC.pas:2586
    public override void Deactivate()
    {
        TItemHS ItemHS;
        TECustomNode Items;
        FItems.FOnChanged = null;
        if (AoWTCUnit.AoWCombatMap.FCombat.FExplorationSite == 1)
        {
            Items = (TECustomNode)AoWTCUnit.AoWCombatMap.FCombat.FItems;
            for (int I = FItems.GetCount() - 1; I >= 0; I--)
            {
                FItems.GetItem(I).SetOwner(Items);
            }
        }
        else
        {
            ItemHS = (TItemHS)AoWHSMap.MapContainer.GetField(AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FX, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FY, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FL).FindOwnedChild(131697);
            if (ItemHS == null)
            {
                ItemHS = new TItemHS(null);
                AoWHSMap.PlaceHX(ItemHS, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FX, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FY, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FL);
            }

            for (int I = FItems.GetCount() - 1; I >= 0; I--)
            {
                FItems.GetItem(I).SetOwner(ItemHS.FItems);
            }
        }

        base.Deactivate();
    }

    // intf_P2_04.inc:216

}

// FPosition. FIndex. FImageIndex. FImages. FDirection
public partial class TUnitPositionHS : TSingleHS
{
    public TUnitPositionHS(TECustomNode owner) : base(owner)
    {
        FPosition = 0;
        FIndex = 0;
        FDirection = 1;
    }

    // AoWTC.pas:5016
    public new const int ClassID = 2228483;

    public sbyte FPosition;
    public sbyte FIndex;
    public int FImageIndex;
    public TCustomImageLibrary FImages;
    public byte FDirection;
    // AoWTC.pas:5060
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPosition), 25);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FIndex), 26);
        Stream.rwByte(ref FDirection, 27);
    }

    // AoWTC.pas:5008


    // AoWTC.pas:5021
    public override byte GetLevel() => 27;

    // AoWTC.pas:5074
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int X = default;
        int Y = default;
        HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref X, ref Y);
        FImages[FImageIndex].ShowClipped(X - XhpSurf - 8, Y - YhpSurf - 8, SurfaceDesc);
    }

    // AoWTC.pas:5031
    public override void Activate()
    {
        TXYL P = default;
        base.Activate();
        if (AoWTCUnit.AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting) && AoWTCUnit.AoWCombatMap.FUnitPositions.ValidatePosition(ref FPosition, ref FIndex))
        {
            var yhx = GetYhx();
            var packXYL = PackXYL(GetXhx(), yhx, 0);
            AoWTCUnit.AoWCombatMap.FUnitPositions.SetUnitPosition(FPosition, FIndex, packXYL);
            FImages = AoWHSSet.ImageLibraryManager.LinkToIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), "TCUNITS.ILB");
            UpdateImageIndex();
        }
        else
        {
            P.X = GetXhx();
            P.Y = GetYhx();
            P.L = 0;
            P.Flag = (sbyte)FDirection;
            AoWTCUnit.AoWCombatMap.FUnitPositions.SetUnitPosition(FPosition, FIndex, P);
            Destroy();
        }
    }

    // AoWTC.pas:5049
    public override void Deactivate()
    {
        if (Activated() && ((TMapLevel)((TMapField)Owner).Owner).Map.Status.Contains(THSMapStatusElement.msEditting))
        {
            AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), "TCUNITS.ILB");
            var unitPosition = AoWTCUnit.AoWCombatMap.FUnitPositions.GetUnitPosition(FPosition, FIndex);
            var xhx = GetXhx();
            bool condition = unitPosition.X == xhx;
            if (condition)
            {
                var fUnitPositionsUnitPosition = AoWTCUnit.AoWCombatMap.FUnitPositions.GetUnitPosition(FPosition, FIndex);
                var yhx = GetYhx();
                condition = fUnitPositionsUnitPosition.Y == yhx;
            }

            if (condition)
            {
                var packXYL = PackXYL(-1, -1, -1);
                AoWTCUnit.AoWCombatMap.FUnitPositions.SetUnitPosition(FPosition, FIndex, packXYL);
            }
        }

        base.Deactivate();
    }

    // AoWTC.pas:5068
    public override bool CanPlace(TMapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx) => Map.GetField(Xhx, Yhx, Lhx).FindOwnedChild(2228483) == null && AoWTCUnit.AoWCombatMap.FUnitPositions.ValidatePosition(ref FPosition, ref FIndex);

    // AoWTC.pas:5026
    public void UpdateImageIndex()
    {
        FImageIndex = FPosition * 8 + FIndex;
    }

    // AoWTC.pas:223


}

// FFrame. FOffsetX. FOffsetY
public partial class TCBloodHS : TSingleHS
{
    public TCBloodHS(TECustomNode owner) : base(owner)
    {}

    public new const int ClassID = 2228487;

    public int FFrame;
    public int FOffsetX;
    public int FOffsetY;
    // AoWTC.pas:6393
    public override byte GetLevel() => 26;

    // AoWTC.pas:6398
    public override int GetShowPriority() => 1744830464 + (FOffsetY << 13) + FOffsetX;

    // AoWTC.pas:6403
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int Frame;
        TImageSequence Sequence;
        int X = default;
        int Y = default;
        Sequence = AoWHSSet.FImageSequences.ImageSequences[4];
        HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref X, ref Y);
        if (Sequence != null)
        {
            Frame = Sequence.GetUnloopedFrameIndex(FFrame);
            Sequence.ImageLibrary[Frame].ShowClipped(X - XhpSurf + FOffsetX, Y - YhpSurf + FOffsetY, SurfaceDesc);
        }
    }

    // AoWTC.pas:6414
    public override void Activate()
    {
        base.Activate();
    }

    // AoWTC.pas:6383
    public override void Deactivate()
    {
        base.Deactivate();
    }

    // Engine.pas:524


}

// FUnit. FUnitGFX. FFrame. FOffsetX. FOffsetY. FSequenceIndex. FSequenceFrame
public partial class TCCorpseHS : TSingleHS
{
    public TCCorpseHS(TECustomNode owner) : base(owner)
    {}

    public new const int ClassID = 2228488;

    public TTacticalCombatUnit FUnit;
    public TUnitGFXResource FUnitGFX;
    public int FFrame;
    public int FOffsetX;
    public int FOffsetY;
    public int FSequenceIndex;
    public int FSequenceFrame;
    // AoWTC.pas:4007
    public override byte GetLevel() => 26;

    // AoWTC.pas:4012
    public override int GetShowPriority() => 1744830464 + (FOffsetY << 13) + FOffsetX;

    // AoWTC.pas:3985
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int Frame;
        int X = default;
        int Y = default;
        TImageSequence Sequence;
        HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref X, ref Y);
        XhpSurf = X - XhpSurf - 15;
        YhpSurf = Y - YhpSurf - 25;
        if (FSequenceIndex > -1)
        {
            Sequence = AoWHSSet.FImageSequences.ImageSequences[FSequenceIndex];
            while (Sequence != null)
            {
                Frame = Sequence.GetUnloopedFrameIndex(FSequenceFrame);
                if (Frame != -1)
                {
                    Sequence.ImageLibrary[Frame].Show(XhpSurf + FOffsetX - 5, YhpSurf + FOffsetY + 10, SurfaceDesc);
                }

                Sequence = Sequence.Next;
            }
        }

        Sequence = FUnitGFX.FSequences.ImageSequences[30];
        while (Sequence != null)
        {
            Frame = Sequence.GetUnloopedFrameIndex(FFrame);
            if (Frame != -1)
            {
                Sequence.ImageLibrary[Frame].Show(XhpSurf + FOffsetX, YhpSurf + FOffsetY, SurfaceDesc);
            }

            Sequence = Sequence.Next;
        }
    }

    // AoWTC.pas:4025
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        TCombatAction AnimateAction;
        TCombatAction RaiseAction;
        int N;
        int X = default;
        int Y = default;
        int DX;
        int DY;
        TfxHS Effect;
        float Z = default;
        float Speed = default;
        float VZ = default;
        float Gravity = default;
        TTacticalCombatUnitHS UnitHS;
        base.MapFieldMsgProc(ref Msg);
        if (FUnit != null)
        {
            if (Msg.MsgID == 200712 && Msg.HN == 0)
            {
                var yhx = GetYhx();
                var xhx = GetXhx();
                if (AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[xhx, yhx].FindOwnedChild(2228484) == null)
                {
                    var yhx2 = GetYhx();
                    var xhx2 = GetXhx();
                    AoWTCUnit.AoWCombatMap.MakeVisible(xhx2, yhx2, 0, 5);
                    var yhx3 = GetYhx();
                    HXtoHP(GetXhx(), yhx3, ref X, ref Y);
                    for (int Ring = 1; Ring <= 3; Ring++)
                    {
                        N = Random(5);
                        DX = Random(20) + Ring * 30;
                        DY = Random(20) + Ring * 30;
                        for (int I = 0; I <= 17; I++)
                        {
                            Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                            Effect.FAbilityIndex = 52;
                            Effect.FSpellIndex = Msg.Param[1].Int;
                            DX = -DX;
                            switch (I)
                            {
                                case 1:
                                case 3:
                                case 5:
                                case 7:
                                case 9:
                                case 11:
                                case 13:
                                case 15:
                                case 17:
                                    DY = -DY;
                                    break;
                            }

                            var pi = Math.PI;
                            var sin = Math.Sin((double)((double)(I * 5 + N) * pi) / 180);
                            var mathPI = Math.PI;
                            MakeLinePath(Round((double)Math.Cos((double)((double)(I * 5 + N) * mathPI) / 180) * DX) + (X + 16), Round((double)sin * DY) / 2 + (Y + 16), X + 16, Y + 16, 100, ref Effect.FPath);
                        }
                    }

                    return;
                }
            }

            if (Msg.MsgID == 200713 && Msg.HN == 0)
            {
                var yhx = GetYhx();
                var xhx = GetXhx();
                if (AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[xhx, yhx].FindOwnedChild(2228484) == null)
                {
                    var yhx2 = GetYhx();
                    HXtoHP(GetXhx(), yhx2, ref X, ref Y);
                    N = Random(6) + 25;
                    for (int I = 1; I <= N - 1; I++)
                    {
                        Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxGeyserWater);
                        Effect.FAbilityIndex = 52;
                        Effect.FSpellIndex = Msg.Param[1].Int;
                        // { Original uses this smaller value for horizontal speed. }
                        Speed = (float)((double)(Random(10) + 10) * 0.05);
                        Gravity = (float)0.5;
                        VZ = (float)((double)(Random(50) + 50) * 0.05);
                        Z = 0;
                        MakeArcPath(X + 16, Y + 16, I * 360 / N, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                    }

                    switch (Msg.Param[1].Int)
                    {
                        case 130:
                            RandSeed = AoWHSMap.Random(16777215);
                            AnimateAction = AoWHSSet.FSpellControl.GetSpell(130).CreateCA(Msg.Param[0].As<TCombatObject>(), FUnit, false);
                            AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(AnimateAction);
                            AnimateAction.Release();
                            break;
                        case 129:
                            RandSeed = AoWHSMap.Random(16777215);
                            RaiseAction = AoWHSSet.FSpellControl.GetSpell(129).CreateCA(Msg.Param[0].As<TCombatObject>(), FUnit, false);
                            AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(RaiseAction);
                            RaiseAction.Release();
                            break;
                    }

                    if (FUnit.GetEnabled())
                    {
                        UnitHS = new TTacticalCombatUnitHS(null);
                        UnitHS.Initialize(FUnit);
                        UnitHS.SetDirection((byte)(Random(6) + 1));
                        var yhx3 = GetYhx();
                        var xhx2 = GetXhx();
                        AoWTCUnit.AoWCombatMap.PlaceHX(UnitHS, xhx2, yhx3, 0);
                    }

                    this.Free();
                }
            }
        }
    }

    // AoWTC.pas:4017
    public override void Activate()
    {
        base.Activate();
        if (FSequenceIndex > -1)
        {
            AoWTCUnit.AoWCombatMap.NewFrameEvents.Add(NewFrame);
        }

        FUnitGFX.AddRef();
        FSequenceFrame = -1;
    }

    // AoWTC.pas:3966
    public override void Deactivate()
    {
        AoWTCUnit.AoWCombatMap.NewFrameEvents.Remove(NewFrame);
        FUnitGFX.Release();
        base.Deactivate();
    }

    // AoWTC.pas:3978
    public void NewFrame(TObject Sender)
    {
        TImageSequence Sequence;
        Sequence = AoWHSSet.FImageSequences.ImageSequences[FSequenceIndex];
        var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
        if (FSequenceFrame + 1 < totalSequenceFrames)
        {
            FSequenceFrame += 1;
        }
    }

    // Engine.pas:524


}

// Inherited single-hexagon sprite layout
public partial class TCOozeHS : TSingleHS
{
    public TCOozeHS(TECustomNode owner) : base(owner)
    {}

    // AoWTC.pas:4092
    public new const int ClassID = 2228489;

    // AoWTC.pas:4097
    public override sbyte GetOverlay(int HN) => 11;

    // AoWTC.pas:4121
    public override byte GetLevel() => 26;

    // AoWTC.pas:4126
    public override int GetShowPriority() => 1744830464 + (GetYhx() << 13) + GetXhx();

    // AoWTC.pas:4102
    public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
    {
        int Frame;
        int X = default;
        int Y = default;
        TImageSequence Sequence;
        HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref X, ref Y);
        XhpSurf = X - XhpSurf - 16;
        YhpSurf = Y - 48 - YhpSurf;
        switch (((TMapField)Owner).TerrainType)
        {
            case 3:
            case 6:
            case 13:
                Sequence = AoWHSSet.FImageSequences.ImageSequences[26];
                break;
            case 12:
                Sequence = AoWHSSet.FImageSequences.ImageSequences[27];
                break;
            default:
                Sequence = AoWHSSet.FImageSequences.ImageSequences[25];
                break;
        }

        while (Sequence != null)
        {
            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
            Frame = Sequence.GetUnloopedFrameIndex(InstanceId % totalSequenceFrames);
            if (Frame != -1)
            {
                Sequence.ImageLibrary[Frame].Show(XhpSurf, YhpSurf, SurfaceDesc);
            }

            Sequence = Sequence.Next;
        }
    }

    // AoWTC.pas:4131
    public override bool PlaceOnMap()
    {
        bool Result = base.PlaceOnMap();
        ((TMapField)Owner).UpdateTerrain();
        return Result;
    }

    // Engine.pas:524


}

// FPositions: packed XYL entries, eight per army position
public partial class TUnitPositionControl : TEObject
{
    public TXYLList FPositions;
    // AoWTC.pas:4943
    public override void Destroy()
    {
        FPositions.Free();
        FPositions = null;
        base.Destroy();
    }

    // AoWTC.pas:4936
    public TUnitPositionControl(TECustomNode owner) : base(owner)
    {
        FPositions = new TXYLList();
        var packXYL = PackXYL(-1, -1, -1);
        FPositions.Default = packXYL;
    }

    // AoWTC.pas:4990
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        TUnitPositionHS HS;
        int X = default;
        int Y = default;
        int L;
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 65541)
        {
            L = Msg.Data[1];
            HPtoHX(Msg.Payload.Int, Msg.Data[0], ref X, ref Y);
            if (AoWTCUnit.AoWCombatMap.MapContainer.GetField(X, Y, L).TerrainType != 15)
            {
                HS = (TUnitPositionHS)AoWTCUnit.AoWCombatMap.MapContainer.GetField(X, Y, L).FindOwnedChild(2228483);
                if (HS == null)
                {
                    HS = new TUnitPositionHS(null);
                    if (!AoWTCUnit.AoWCombatMap.PlaceHX(HS, X, Y, L))
                    {
                        HS.Free();
                        HS = null;
                    }
                }

                if (HS != null)
                {
                }
            }
        }
    }

    // AoWTC.pas:4950
    public TXYL GetUnitPosition(int Position, int Index)
    {
        TXYL Result = default;
        int I;
        I = Position * 8 + Index;
        if (I < FPositions.Count)
        {
            Result = FPositions[I];
        }
        else
        {
            Result.X = -1;
            Result.Y = -1;
            Result.L = -1;
        }

        return Result;
    }

    // AoWTC.pas:4963
    public void SetUnitPosition(int Position, int Index, TXYL Value)
    {
        if (Position < 7 && Index < 8)
        {
            FPositions[Position * 8 + Index] = Value;
        }
    }

    // AoWTC.pas:4969
    public bool ValidatePosition(ref sbyte Position, ref sbyte Index)
    {
        bool Result = FPositions[Position * 8 + Index].X == -1;
        if (!Result)
        {
            Index = 0;
            while (FPositions[Position * 8 + Index].X != -1 && Index != 8)
            {
                Index = (sbyte)(Index + 1);
            }

            Result = Index != 8;
        }

        if (!Result)
        {
            Position = 0;
            while (!Result && Position < 7)
            {
                while (FPositions[Position * 8 + Index].X != -1 && Index != 8)
                {
                    Index = (sbyte)(Index + 1);
                }

                Result = Index != 8;
                if (!Result)
                {
                    Index = 0;
                    Position = (sbyte)(Position + 1);
                }
            }
        }

        return Result;
    }

}

// FMoveFlags
public partial class TCombatMoveControlSettings : TMoveControlSettings
{
    public TCombatMoveControlSettings(TECustomNode owner) : base(owner)
    { }

    public TSet<TCombatMoveModeElement> FMoveFlags;
    // intf_P5_01.inc:63

}

public partial class TCombatMoveControl : TMoveControl
{

    public TCombatMoveControl(THSMap owner) : base(owner)
    { }

    public TTacticalCombatUnitHS FUnitHS;
    public TSet<TCombatMoveModeElement> FMoveFlags;
    // AoWTC.pas:2716
    public override void ProcessMove(TMoveControlSettings Settings)
    {
        FUnitHS = (TTacticalCombatUnitHS)Settings.FArmyHS;
        FMoveFlags = ((TCombatMoveControlSettings)Settings).FMoveFlags;
        base.ProcessMove(Settings);
    }

    // intf_P5_01.inc:120

}

// FMovePoints. FUnitHS. FPositionIndex. FPositionGroup. FCanFlee. FDeathMethod
public partial class TTacticalCombatUnit : TCombatUnit
{
    public TTacticalCombatUnit(TECustomNode owner) : base(owner)
    { }

    public int FMovePoints;
    public TTacticalCombatUnitHS FUnitHS;
    public byte FPositionIndex;
    public byte FPositionGroup;
    public bool FCanFlee;
    public TTCDeathMethod FDeathMethod;
    // AoWTC.pas:2626
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
    }

    // AoWTC.pas:2612
    public override void NewTurn(byte CombatPlayer)
    {
        base.NewTurn(CombatPlayer);
        if (CombatPlayer == (byte)FCombatPlayer && GetEnabled())
        {
            FMovePoints = GetMoves();
        }
    }

    // AoWTC.pas:2629
    public override int ExecuteDamage(int Damage)
    {
        TCCorpseHS Corpse;
        TImageSequence Sequence;
        TfxHS Effect;
        float Speed = default;
        float VZ = default;
        float Gravity = default;
        float Z = default;
        int X = default;
        int Y = default;
        TRect R;
        void MakeBlood()
        {
            int Count;
            int X = default;
            int Y = default;
            int Direction;
            TCBloodHS Blood;
            if (GetEnabled() && FUnitHS != null && FUnit.GetBloodType() != TBloodType.btNone)
            {
                var hitPoints = GetHitPoints();
                if (Damage >= hitPoints && (GetAbilityEnabled(131) || FDeathMethod == TTCDeathMethod.dmExplode))
                {
                    Count = Random(5) + 10;
                }
                else
                {
                    Count = Random(Damage + 7) - 4;
                }

                while (Count > 0)
                {
                    Direction = Random(12);
                    if (Direction > 6)
                    {
                        Direction = 0;
                    }

                    var yhx = FUnitHS.GetYhx();
                    CenterHNtoHX(FUnitHS.GetXhx(), yhx, Direction, ref X, ref Y);
                    if (AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].TerrainType != 0 && AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].TerrainType != 9)
                    {
                        Blood = new TCBloodHS(AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y]);
                        Blood.Activate();
                        Blood.FOffsetX = Random(32) - 32;
                        Blood.FOffsetY = Random(32) - 32;
                        Blood.FFrame = Random(AoWHSSet.FImageSequences.ImageSequences[4].GetTotalSequenceFrames());
                    }

                    Count -= 1;
                }
            }
        }

        MakeBlood();
        int Result = base.ExecuteDamage(Damage);
        if (!GetEnabled())
        {
            if (FUnitHS != null)
            {
                if (GetAbilityEnabled(131))
                {
                    MakeHitBlood(this, 100);
                    for (int I = 0; I <= 9; I++)
                    {
                        Effect = AoWTCUnit.AoWCombatMap.AddEffect(TfxType.fxDecayComet);
                        Effect.FAbilityIndex = 52;
                        Effect.FSpellIndex = 106;
                        var yhx = FUnitHS.GetYhx();
                        HXtoHP(FUnitHS.GetXhx(), yhx, ref X, ref Y);
                        var xyz = new TXYZ();
                        Effect.FPath.Add(xyz);
                        Effect.FPath[0].X = X + 16;
                        Effect.FPath[0].Y = Y + 8;
                        for (int J = 0; J <= 10; J++)
                        {
                            Speed = (float)((double)(Random(50) + 50) * 0.05);
                            Gravity = (float)1.5;
                            VZ = (float)((double)(Random(25) + 25) * 0.05);
                            Z = 0;
                            var count = Effect.FPath.Count;
                            var path = Effect.FPath[count - 1];
                            var fPathCount = Effect.FPath.Count;
                            var pathZ = Effect.FPath[fPathCount - 1];
                            var count2 = Effect.FPath.Count;
                            var pathX = Effect.FPath[count2 - 1];
                            var random = Random(360);
                            MakeArcPath(pathX.X, path.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                        }
                    }
                }
                else
                {
                    var yhx = FUnitHS.GetYhx();
                    var xhx = FUnitHS.GetXhx();
                    bool condition = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[xhx, yhx].TerrainType != 0;
                    if (condition)
                    {
                        var fUnitHSYhx = FUnitHS.GetYhx();
                        var fUnitHSXhx = FUnitHS.GetXhx();
                        condition = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[fUnitHSXhx, fUnitHSYhx].TerrainType != 9;
                    }

                    if (condition)
                    {
                        var fUnitHSYhx = FUnitHS.GetYhx();
                        var fUnitHSXhx = FUnitHS.GetXhx();
                        Corpse = new TCCorpseHS(AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[fUnitHSXhx, fUnitHSYhx]);
                        Corpse.FUnitGFX = FUnitHS.FUnitGFXResource;
                        Sequence = Corpse.FUnitGFX.FSequences.ImageSequences[30];
                        if (Sequence != null)
                        {
                            Corpse.FFrame = Random(Sequence.GetTotalSequenceFrames());
                        }

                        switch (FDeathMethod)
                        {
                            case TTCDeathMethod.dmNormal:
                                switch (FUnit.GetBloodType())
                                {
                                    case TBloodType.btNone:
                                        Corpse.FSequenceIndex = -1;
                                        break;
                                    default:
                                    {
                                        Corpse.FSequenceIndex = ((int)FUnit.GetUnitSize() & 127) + 30;
                                        if (Corpse.FSequenceIndex > 32)
                                        {
                                            Corpse.FSequenceIndex = 32;
                                        }
                                    }

                                        break;
                                }

                                break;
                            case TTCDeathMethod.dmExplode:
                                Corpse.FSequenceIndex = -1;
                                break;
                        }

                        Corpse.FOffsetX = Random(10) - 5;
                        Corpse.FOffsetY = Random(10) - 5;
                        if (FUnit.GetUnitType() != TUnitType.utMachine)
                        {
                            Corpse.FUnit = this;
                        }

                        Corpse.Activate();
                    }
                }

                var yhx2 = FUnitHS.GetYhx();
                var xhx2 = FUnitHS.GetXhx();
                var yhx3 = FUnitHS.GetYhx();
                R = Rect(FUnitHS.GetXhx(), yhx3, xhx2 + 1, yhx2 + 1);
                FUnitHS.Free();
                FUnitHS = null;
                AoWTCUnit.AoWCombatMap.TriggerMapChangedEvent(R, 0);
            }
        }
        else
        {
            if (FUnitHS != null)
            {
                var unitGFXResourceIndex = FUnit.GetUnitGFXResourceIndex();
                FUnitHS.SetUnitGFXResourceIndex(unitGFXResourceIndex);
            }
        }

        return Result;
    }

    // AoWTC.pas:2605
    public int GetPosValue()
    {
        int Result = GetAttack() + GetDamage() + GetDefense() + GetMoves() / 3;
        if (SupportCombatMode(TSet<TCombatMode>.Of(TCombatMode.cmMediumRange)))
        {
            Result = Result - 100;
        }

        if (SupportCombatMode(TSet<TCombatMode>.Of(TCombatMode.cmLongRange)))
        {
            Result = Result - 200;
        }

        return Result;
    }

    // AoWTC.pas:2618
    public void MovedTo(TAoWMapField OldField, TAoWMapField NewField, int MoveCost)
    {
        FMovePoints = FMovePoints - MoveCost;
        if (FMovePoints < 0)
        {
            FMovePoints = 0;
        }

        if (GetAbilityEnabled(43))
        {
            var yhx = FUnitHS.GetYhx();
            CreateFire(false, 50, FUnitHS.GetXhx(), yhx);
        }

        AoWTCUnit.AoWCombatMap.FUnitChangedEvents.TriggerNotifyEvents(FUnitHS);
    }

    // intf_P3_01.inc:244

}

// FUnit. FMoveIndicator. FIndicatorStep. FAction. FActionDone. FActionFrame. FOffsetX.
// FOffsetY. FSelectedAbility. FSelectedSpell. FCombatAbilityCount. FAbilities. FBusy
public partial class TTacticalCombatUnitHS : TUnitHS
{
    public TTacticalCombatUnitHS(TECustomNode owner) : base(owner)
    {}

    // AoWTC.pas:3348
    public new const int ClassID = 2228484;

    public TTacticalCombatUnit FUnit;
    public short FMoveIndicator;
    public short FIndicatorStep;
    public TTCAction FAction;
    public bool FActionDone;
    public short FActionFrame;
    public int FOffsetX;
    public int FOffsetY;
    public int FSelectedAbility;
    public int FSelectedSpell;
    public int FCombatAbilityCount;
    public TList FAbilities;
    public bool FBusy;
    // AoWTC.pas:3566
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID != 536936471 && (Msg.MsgID & 1073741824) == 0 && FUnit != null)
        {
            FUnit.MsgProc(ref Msg, Sender);
        }
    }

    // AoWTC.pas:3461
    public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
    {
        TSelfDestructCA SelfDestruct;
        TDamageCA SpellDamage;
        TRangedAttackCA RangedDamage;
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4364)
        {
            if (Msg.HN == 0 && (!AoWTCUnit.AoWCombatMap.FFogEnabled || Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((TAoWMapField)Owner).FFieldFlags).Hi).Lo != 0))
            {
                if (FUnit.GetPlayer() != 0)
                {
                    var player = FUnit.GetPlayer();
                    var playerPlayerRace = AoWHSMap.FPlayers.GetPlayers(player);
                    Msg.Param[0] = Msg.Param[0].Int | (playerPlayerRace.FPlayerRace + 1);
                }
                else
                {
                    Msg.Param[0] = Msg.Param[0].Int | 13;
                }
            }
        }
        else
        {
            if (Msg.MsgID == 4352 && Msg.HN == 0)
            {
                var player = FUnit.GetPlayer();
                Msg.Result = player;
            }
            else
            {
                if (Msg.MsgID == 200704 && Msg.HN == 0)
                {
                    RandSeed = AoWHSMap.Random(16777215);
                    SelfDestruct = ((TSelfDestructAbility)AoWHSSet.FAbilityControl.GetAbility(65)).CreateSelfDestructCA(Msg.Param[0].As<TCombatObject>(), FUnit);
                    if (SelfDestruct.FDamage > 0)
                    {
                        SetAction(TTCAction.caHit);
                    }

                    AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(SelfDestruct);
                    SelfDestruct.Release();
                }
                else
                {
                    if (Msg.MsgID == 200707 && Msg.HN == 0)
                    {
                        RandSeed = AoWHSMap.Random(16777215);
                        SpellDamage = (TDamageCA)AoWHSSet.FSpellControl.GetSpell(117).CreateCA(Msg.Param[0].As<TCombatObject>(), FUnit, false);
                        if (SpellDamage.FDamage > 0)
                        {
                            SetAction(TTCAction.caHit);
                        }

                        AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                        SpellDamage.Release();
                    }
                    else
                    {
                        if (Msg.MsgID == 200715 && Msg.HN == 0)
                        {
                            RandSeed = AoWHSMap.Random(16777215);
                            SpellDamage = (TDamageCA)AoWHSSet.FSpellControl.GetSpell(113).CreateCA(Msg.Param[0].As<TCombatObject>(), FUnit, false);
                            if (SpellDamage.FDamage > 0)
                            {
                                SetAction(TTCAction.caHit);
                            }

                            AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                            SpellDamage.Release();
                        }
                        else
                        {
                            if (Msg.MsgID == 200714 && Msg.HN == 0)
                            {
                                RandSeed = AoWHSMap.Random(16777215);
                                RangedDamage = ((TRangedAttackAbility)AoWHSSet.FAbilityControl.GetAbility(31)).CreateRangedAttackCA(Msg.Param[0].As<TCombatObject>(), FUnit, 0);
                                if (RangedDamage.FDamage > 0)
                                {
                                    SetAction(TTCAction.caHit);
                                }

                                AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(RangedDamage);
                                RangedDamage.Release();
                            }
                            else
                            {
                                if (Msg.MsgID == 200723 && Msg.HN == 0)
                                {
                                    RandSeed = AoWHSMap.Random(16777215);
                                    SpellDamage = (TDamageCA)AoWHSSet.FSpellControl.GetSpell(Msg.Param[1].Int).CreateCA(Msg.Param[0].As<TCombatObject>(), FUnit, false);
                                    if (SpellDamage.FDamage > 0)
                                    {
                                        SetAction(TTCAction.caHit);
                                    }

                                    AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                    SpellDamage.Release();
                                }
                                else
                                {
                                    if (Msg.MsgID == 200722 && Msg.HN == 0)
                                    {
                                        RandSeed = AoWHSMap.Random(16777215);
                                        RangedDamage = ((TRangedAttackAbility)AoWHSSet.FAbilityControl.GetAbility(Msg.Param[1].Int)).CreateRangedAttackCA(Msg.Param[0].As<TCombatObject>(), FUnit, 0);
                                        if (RangedDamage.FDamage > 0)
                                        {
                                            SetAction(TTCAction.caHit);
                                        }

                                        AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(RangedDamage);
                                        RangedDamage.Release();
                                    }
                                    else
                                    {
                                        if (Msg.MsgID == 200709 && Msg.HN == 0)
                                        {
                                            RandSeed = AoWHSMap.Random(16777215);
                                            SpellDamage = (TDamageCA)AoWHSSet.FSpellControl.GetSpell(104).CreateCA(Msg.Param[0].As<TCombatObject>(), FUnit, false);
                                            if (SpellDamage.FDamage > 0)
                                            {
                                                SetAction(TTCAction.caHit);
                                            }

                                            AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                            SpellDamage.Release();
                                        }
                                        else
                                        {
                                            if (Msg.MsgID == 200716 && Msg.HN == 0)
                                            {
                                                RandSeed = AoWHSMap.Random(16777215);
                                                SpellDamage = (TDamageCA)AoWHSSet.FSpellControl.GetSpell(120).CreateCA(Msg.Param[0].As<TCombatObject>(), FUnit, false);
                                                if (SpellDamage.FDamage > 0)
                                                {
                                                    SetAction(TTCAction.caHit);
                                                }

                                                AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                                SpellDamage.Release();
                                            }
                                            else
                                            {
                                                if (Msg.MsgID == 200710 && Msg.HN == 0)
                                                {
                                                    RandSeed = AoWHSMap.Random(16777215);
                                                    SpellDamage = (TDamageCA)AoWHSSet.FSpellControl.GetSpell(116).CreateCA(Msg.Param[0].As<TCombatObject>(), FUnit, false);
                                                    if (SpellDamage.FDamage > 0)
                                                    {
                                                        SetAction(TTCAction.caHit);
                                                    }

                                                    AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                                    SpellDamage.Release();
                                                }
                                                else
                                                {
                                                    if (Msg.MsgID == 200717 && Msg.HN == 0)
                                                    {
                                                        RandSeed = AoWHSMap.Random(16777215);
                                                        SpellDamage = (TDamageCA)AoWHSSet.FSpellControl.GetSpell(127).CreateCA(Msg.Param[0].As<TCombatObject>(), FUnit, false);
                                                        if (SpellDamage.FDamage > 0)
                                                        {
                                                            SetAction(TTCAction.caHit);
                                                        }

                                                        AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                                        SpellDamage.Release();
                                                    }
                                                    else
                                                    {
                                                        if (Msg.MsgID == 200718 && Msg.HN == 0)
                                                        {
                                                            RandSeed = AoWHSMap.Random(16777215);
                                                            SpellDamage = (TDamageCA)AoWHSSet.FSpellControl.GetSpell(128).CreateCA(Msg.Param[0].As<TCombatObject>(), FUnit, false);
                                                            if (SpellDamage.FDamage > 0)
                                                            {
                                                                SetAction(TTCAction.caHit);
                                                            }

                                                            AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                                            SpellDamage.Release();
                                                        }
                                                        else
                                                        {
                                                            if (Msg.MsgID == 200719 && Msg.HN == 0)
                                                            {
                                                                SetDirection((byte)(FDirection + 1));
                                                                if (FDirection > 6)
                                                                {
                                                                    SetDirection(1);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (Msg.MsgID == 200720 && Msg.HN == 0)
                                                                {
                                                                    FOffsetX = Random(Msg.Param[0].Int * 2 + 1) - Msg.Param[0].Int;
                                                                    FOffsetY = Random(Msg.Param[0].Int * 2 + 1) - Msg.Param[0].Int;
                                                                }
                                                                else
                                                                {
                                                                    if (Msg.MsgID == 200721 && Msg.HN == 0)
                                                                    {
                                                                        FOffsetX = 0;
                                                                        FOffsetY = 0;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (Msg.MsgID == 200708 && Msg.HN == 0)
                                                                        {
                                                                            if (!FUnit.FUnit.GetMoveTypes().Contains(TMoveType.mtFlying) && ((TMapField)Owner).TerrainType != 0 && ((TMapField)Owner).TerrainType != 9)
                                                                            {
                                                                                RandSeed = AoWHSMap.Random(16777215);
                                                                                SpellDamage = (TDamageCA)AoWHSSet.FSpellControl.GetSpell(112).CreateCA(Msg.Param[0].As<TCombatObject>(), FUnit, false);
                                                                                if (SpellDamage.FDamage > 0)
                                                                                {
                                                                                    SetAction(TTCAction.caHit);
                                                                                }

                                                                                AoWTCUnit.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                                                                SpellDamage.Release();
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
                    }
                }
            }
        }
    }

    // AoWTC.pas:3370
    public override void CanMoveOn(in THSMoveOnInfo Info, ref THSMoveOnResult MoveResult)
    {
        int X = default;
        int Y = default;
        TMovementHS Movement;
        TAbilityCombatInfo AbilityInfo = default;
        TAbilityCombatInfo EnemyInfo = default;
        if (Info.HN == 0)
        {
            MoveResult.Code = -1;
            MoveResult.Done = true;
            var yhx = GetYhx();
            var xhx = GetXhx();
            Movement = (TMovementHS)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[xhx, yhx].FindOwnedChild(2228490);
            if (Movement != null && Movement.FEnabled)
            {
                if (((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide() == FUnit.GetSide())
                {
                    MoveResult.Code = 0;
                    MoveResult.Done = false;
                    return;
                }
            }

            if (GetXhx() != ((TCombatMoveControl)Info.Sender).FDestinationX && ((TCombatMoveControl)Info.Sender).FDestinationX != -1)
            {
                return;
            }

            if (GetYhx() != ((TCombatMoveControl)Info.Sender).FDestinationY && ((TCombatMoveControl)Info.Sender).FDestinationY != -1)
            {
                return;
            }

            if (!((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmaiMove) && !((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmMoveRanged) && AoWTCUnit.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetCount() >= 2)
            {
                return;
            }

            if (((TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility <= -1 && (MeleeDamageTypes(((TCombatMoveControl)Info.Sender).FUnitHS.FUnit) == TSet<TDamageType>.Of() || FUnit.GetAbilityEnabled(1) && !((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1)) && !((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmMoveRanged) && !((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmaiMove))
            {
                return;
            }

            if (((TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility == -1)
            {
                if (!((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmaiMove) && !((TCombatMoveControl)Info.Sender).FMoveFlags.Contains(TCombatMoveModeElement.mmMoveRanged))
                {
                    if (FUnit.GetSide() == ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide())
                    {
                        return;
                    }
                }

                MoveResult.Code = 0;
                return;
            }

            AoWHSSet.FAbilityControl.GetAbility(((TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility).GetCombatInfo(((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.FUnit, ref AbilityInfo);
            if (AbilityInfo.TargetTypes.Contains(TCombatTargetTypeElement.cttFriend) && FUnit.GetSide() == ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide())
            {
                goto Touch;
            }

            AoWHSSet.FAbilityControl.GetAbility(((TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility).GetCombatInfo(((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.FUnit, ref EnemyInfo);
            if (!EnemyInfo.TargetTypes.Contains(TCombatTargetTypeElement.cttEnemy))
            {
                return;
            }

            if (FUnit.GetSide() == ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide())
            {
                return;
            }

            Touch:
                if (((TTouchAbility)AoWHSSet.FAbilityControl.GetAbility(((TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility)).CanTouch(((TCombatMoveControl)Info.Sender).FUnitHS.FUnit, FUnit))
                {
                    MoveResult.Code = 0;
                }
        }
        else
        {
            var yhx = GetYhx();
            CenterHNtoHX(GetXhx(), yhx, Info.HN, ref X, ref Y);
            if (MoveResult.Code < 30)
            {
                MoveResult.Code = ((TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][Info.MapField.OverlayType - -1];
            }

            if (MoveResult.Code <= -1)
            {
                return;
            }

            if (Info.HN >= 7)
            {
                return;
            }

            if (GetXhx() == ((TCombatMoveControl)Info.Sender).FDestinationX && GetYhx() == ((TCombatMoveControl)Info.Sender).FDestinationY)
            {
                return;
            }

            if (FUnit.GetSide() == ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide())
            {
                return;
            }

            if (MeleeDamageTypes(FUnit) == TSet<TDamageType>.Of())
            {
                return;
            }

            if (FUnit.GetLocked())
            {
                return;
            }

            if (FUnit.GetAbilityEnabled(1) != ((TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1))
            {
                return;
            }

            if (((TCombatMoveControl)Info.Sender).FDestinationX >= 0 && ((TCombatMoveControl)Info.Sender).FDestinationY >= 0 && dHXtoRad(X, Y, ((TCombatMoveControl)Info.Sender).FDestinationX, ((TCombatMoveControl)Info.Sender).FDestinationY) <= 1)
            {
                if (AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[((TCombatMoveControl)Info.Sender).FDestinationX, ((TCombatMoveControl)Info.Sender).FDestinationY].FindOwnedChild(2228484) != null)
                {
                    return;
                }
            }

            if (MoveResult.Code > 96)
            {
                MoveResult.Code = 127;
                return;
            }

            MoveResult.Code += 30;
        }
    }

    // AoWTC.pas:3447
    public override void Activate()
    {
        base.Activate();
        SetUnitGFXResourceIndex(FUnit.FUnit.GetUnitGFXResourceIndex());
        FAbilities = new TList();
        CreateTCAbList();
        AoWTCUnit.AoWCombatMap.NewFrameEvents.Add(NewFrame);
        if (AoWTCUnit.AoWCombatMap.FExplorationEnabled)
        {
            var player = FUnit.GetPlayer();
            var visibilityRange = FUnit.GetVisibilityRange();
            var yhx = GetYhx();
            var xhx = GetXhx();
            AoWTCUnit.AoWCombatMap.ExploreArea(xhx, yhx, 0, player, visibilityRange);
        }

        FActionFrame = -1;
        FSelectedAbility = -1;
        FSelectedSpell = -1;
    }

    // AoWTC.pas:3339
    public override void Deactivate()
    {
        if (AoWTCUnit.AoWCombatMap != null)
        {
            AoWTCUnit.AoWCombatMap.NewFrameEvents.Remove(NewFrame);
        }

        for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
        {
            ((TObject)FAbilities[I]).Free();
        }

        FAbilities.Free();
        base.Deactivate();
    }

    // AoWTC.pas:3439
    public override void UpdateMapField(in THSMapFieldInfo Info)
    {
        if (Info.HN == 0)
        {
            ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 16384 | 8192 | 33554432;
        }
        else
        {
            if (Info.HN <= 6)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 16384;
            }
        }
    }

    // AoWTC.pas:3784
    public override void Select()
    {
        base.Select();
        CreateTCAbList();
        FSelectedAbility = -1;
        if (AoWTCUnit.AoWCombatMap.FUnitSelection.GetTCUnitHS() != this)
        {
            AoWTCUnit.AoWCombatMap.FUnitSelection.SetTCUnitHS(this);
        }

        FUnitGFXResource.FSFXLibraries[0].Play(0);
        if (AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer == FUnit.FCombatPlayer)
        {
            AoWTCUnit.AoWCombatMap.FPlayers.GetPlayers(AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer).FLastUnit = FUnit;
        }
    }

    // AoWTC.pas:3796
    public override void Unselect()
    {
        base.Unselect();
        if (AoWTCUnit.AoWCombatMap != null)
        {
            if (AoWTCUnit.AoWCombatMap.FUnitSelection.GetTCUnitHS() == this)
            {
                AoWTCUnit.AoWCombatMap.FUnitSelection.Unselect();
            }
        }
    }

    // AoWTC.pas:3357
    public override int GetMoveSteps()
    {
        int Result;
        switch (AoWEngine.FRegistry.GetMovementSpeed())
        {
            case TMovementSpeed.mmsFast:
                Result = base.GetMoveSteps();
                if (Result > 8)
                {
                    Result = Result - 4;
                }
                else
                {
                    Result = 4;
                }

                break;
            case TMovementSpeed.mmsTurbo:
                Result = 2;
                break;
            default:
                Result = base.GetMoveSteps();
                break;
        }

        return Result;
    }

    // AoWTC.pas:3351
    public void Initialize(TTacticalCombatUnit U)
    {
        FUnit = U;
        FUnit.FUnitHS = this;
    }

    // AoWTC.pas:3804
    public bool MeleeMoveTC(TMovepath Path)
    {
        TCMeleeMoveTE TE;
        bool Result = false;
        var player = FUnit.GetPlayer();
        if (AoWHSMap.FTokenManager.PlayerReady(player) && Path.Count > 1)
        {
            TE = CreateTCMeleeMoveTE(FUnit, Path);
            if (TE != null)
            {
                var fUnitPlayer = FUnit.GetPlayer();
                AoWHSMap.FTokenManager.GetTokenControl(fUnitPlayer).AddEvent(TE);
                TE.Release();
                Result = true;
            }
        }

        return Result;
    }

    // AoWTC.pas:3836
    public int SelectAbility(int AbilityID)
    {
        int Index;
        string Reason = "";
        AoWTCUnit.AoWCombatMap.FUnitSelection.DeactivateRangedPath();
        FSelectedAbility = -1;
        int Result = -1;
        Index = -1;
        if (AoWTCUnit.AoWCombatMap.FCurrentCombatPlayer == AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer)
        {
            for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
            {
                if (((TTCAbility)FAbilities[I]).Ability == AbilityID)
                {
                    Index = I;
                }
            }

            if (!FUnit.GetLocked() && FUnit.FMovePoints > 0 && FUnit.FCombatPlayer == AoWTCUnit.AoWCombatMap.FCurrentCombatPlayer && Index < FCombatAbilityCount && Index > -1)
            {
                if (AbilityID == 52)
                {
                    if (FUnit.FUnit.GetMoves() == FUnit.FMovePoints)
                    {
                        AoWHSSet.FAbilityControl.GetAbility(AbilityID).ActivateCombat(FUnit.FUnit, ref Reason);
                        FSelectedSpell = -1;
                        FSelectedAbility = AbilityID;
                        Result = Index;
                    }
                }
                else
                {
                    var combatMode = AoWHSSet.FAbilityControl.GetAbility(AbilityID).GetCombatMode(FUnit.FUnit);
                    if (TSet<TCombatMode>.Of(TCombatMode.cmShortRange, TCombatMode.cmMediumRange, TCombatMode.cmLongRange).Contains(combatMode))
                    {
                        if (FUnit.FUnit.GetMoves() == FUnit.FMovePoints)
                        {
                            if (AoWHSSet.FAbilityControl.GetAbility(AbilityID).GetCombatMode(FUnit.FUnit) == TCombatMode.cmShortRange)
                            {
                                AoWTCUnit.AoWCombatMap.FUnitSelection.FRangedPathHS.FRange = 4;
                            }
                            else
                            {
                                if (AoWHSSet.FAbilityControl.GetAbility(AbilityID).GetCombatMode(FUnit.FUnit) == TCombatMode.cmMediumRange)
                                {
                                    AoWTCUnit.AoWCombatMap.FUnitSelection.FRangedPathHS.FRange = 8;
                                }
                                else
                                {
                                    AoWTCUnit.AoWCombatMap.FUnitSelection.FRangedPathHS.FRange = 12;
                                }
                            }

                            AoWTCUnit.AoWCombatMap.FUnitSelection.FRangedPathHS.FSelection = AoWTCUnit.AoWCombatMap.FUnitSelection;
                            AoWTCUnit.AoWCombatMap.DynamicFixedVisibleHS.AddChild(AoWTCUnit.AoWCombatMap.FUnitSelection.FRangedPathHS);
                            FSelectedAbility = AbilityID;
                            Result = Index;
                        }
                    }
                    else
                    {
                        FSelectedAbility = AbilityID;
                        Result = Index;
                    }
                }
            }
        }

        for (int I = 0, last = AoWTCUnit.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetCount() - 1; I <= last; I++)
        {
            var moveSettings = AoWTCUnit.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetMoveSettings(I);
            moveSettings.FPath.Clear();
        }

        AoWTCUnit.AoWCombatMap.FUnitSelection.FRangedPath.Clear();
        AoWTCUnit.AoWCombatMap.FUnitSelection.UpdateMoveCursor();
        AoWTCUnit.AoWCombatMap.FUnitChangedEvents.TriggerNotifyEvents(this);
        return Result;
    }

    // AoWTC.pas:3870
    public int SelectSpell(int SpellID)
    {
        TSpellCombatInfo Info = default;
        AoWTCUnit.AoWCombatMap.FUnitSelection.DeactivateRangedPath();
        FSelectedSpell = SpellID;
        AoWHSSet.FSpellControl.GetSpell(FSelectedSpell).GetCombatInfo(FUnit.FUnit, ref Info);
        if (Info.Range != 0)
        {
            if (Info.Range == -1)
            {
                AoWTCUnit.AoWCombatMap.FUnitSelection.FRangedPathHS.FRange = 100;
            }
            else
            {
                AoWTCUnit.AoWCombatMap.FUnitSelection.FRangedPathHS.FRange = (ushort)Info.Range;
            }

            AoWTCUnit.AoWCombatMap.FUnitSelection.FRangedPathHS.FSelection = AoWTCUnit.AoWCombatMap.FUnitSelection;
            AoWTCUnit.AoWCombatMap.DynamicFixedVisibleHS.AddChild(AoWTCUnit.AoWCombatMap.FUnitSelection.FRangedPathHS);
        }

        int Result = SpellID;
        for (int I = 0, last = AoWTCUnit.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetCount() - 1; I <= last; I++)
        {
            var moveSettings = AoWTCUnit.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetMoveSettings(I);
            moveSettings.FPath.Clear();
        }

        AoWTCUnit.AoWCombatMap.FUnitSelection.FRangedPath.Clear();
        switch (AoWTCUnit.SpellTypes[SpellID])
        {
            case TfxType.fxScreenNESW:
            case TfxType.fxSacredWrath:
            case TfxType.fxTremors:
            case TfxType.fxHighPrayer:
            case TfxType.fxShockWave:
                AbRangedTC(0, 0);
                Result = -1;
                Unselect();
                break;
        }

        AoWTCUnit.AoWCombatMap.FUnitSelection.UpdateMoveCursor();
        AoWTCUnit.AoWCombatMap.FUnitChangedEvents.TriggerNotifyEvents(this);
        return Result;
    }

    // AoWTC.pas:3895
    public void CreateTCAbList()
    {
        TTCAbility Ability;
        for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
        {
            ((TObject)FAbilities[I]).Free();
        }

        FAbilities.Clear();
        FCombatAbilityCount = 0;
        for (int I = 1; I <= 169; I++)
        {
            if (FUnit.GetAbilityEnabled(I))
            {
                Ability = new TTCAbility();
                Ability.Ability = I;
                if (AoWHSSet.FAbilityControl.GetAbility(I).GetControlType(FUnit.FUnit).Contains(TAbilityControlTypeElement.acCombatAction) && AoWHSSet.FAbilityControl.GetAbility(I).GetCombatMode(FUnit.FUnit) != TCombatMode.cmMelee)
                {
                    FAbilities.Insert(FCombatAbilityCount, Ability);
                    FCombatAbilityCount += 1;
                }
                else
                {
                    FAbilities.Add(Ability);
                }
            }
        }
    }

    // AoWTC.pas:3910
    public void ListAbilities(TStrings Names, TStrings Types, TStrings Sources)
    {
        Names.Clear();
        if (Types != null)
        {
            Types.Clear();
        }

        if (Sources != null)
        {
            Sources.Clear();
        }

        for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
        {
            var ability = FAbilities[I];
            Names.AddObject(AoWHSSet.FAbilityControl.GetAbility(((TTCAbility)ability).Ability).GetName(FUnit.FUnit), IntToObject(((TTCAbility)FAbilities[I]).Ability));
            if (Types != null)
            {
                var ability2 = FAbilities[I];
                var abilityType = AoWHSSet.FAbilityControl.GetAbility(((TTCAbility)ability2).Ability).GetAbilityType();
                Types.Add(TranslateRStr(TAbilityTypeRStr[(int)abilityType]));
            }

            if (Sources != null)
            {
                var ability2 = FAbilities[I];
                Sources.Add(AoWHSSet.FAbilityControl.GetAbility(((TTCAbility)ability2).Ability).GetSourceName(FUnit.FUnit));
            }
        }
    }

    // AoWTC.pas:3923
    public void SetAction(TTCAction Action)
    {
        FActionDone = false;
        switch (Action)
        {
            case TTCAction.caStepUp:
            case TTCAction.caStepBack:
                FActionFrame = 0;
                break;
            case TTCAction.caRoundAttack:
            case TTCAction.caRoundAttackHit:
                if (FAction != TTCAction.caRoundAttack && FAction != TTCAction.caRoundAttackHit)
                {
                    FActionFrame = -1;
                }

                break;
            default:
                FActionFrame = -1;
                break;
        }

        FAction = Action;
    }

    // AoWTC.pas:3935
    public bool AbTouchMoveTC(TMovepath Path)
    {
        TTokenEvent TE;
        TAbilityCombatInfo Buffer = default;
        bool Result = false;
        var player = FUnit.GetPlayer();
        if (AoWHSMap.FTokenManager.PlayerReady(player) && Path.Count > 0)
        {
            if (Path.Count > 1)
            {
                TE = CreateTCAbTouchMoveTE(FUnit, Path, FSelectedAbility);
                if (TE != null)
                {
                    var fUnitPlayer = FUnit.GetPlayer();
                    AoWHSMap.FTokenManager.GetTokenControl(fUnitPlayer).AddEvent(TE);
                    TE.Release();
                    Result = true;
                }
            }
            else
            {
                AoWHSSet.FAbilityControl.GetAbility(FSelectedAbility).GetCombatInfo(FUnit.FUnit, ref Buffer);
                if (Buffer.TargetTypes.Contains(TCombatTargetTypeElement.cttFriend))
                {
                    var path = Path[0];
                    AbRangedTC((ushort)Path[0].X, (ushort)path.Y);
                }
            }
        }

        return Result;
    }

    // AoWTC.pas:3952
    public bool AbRangedTC(ushort X, ushort Y)
    {
        TCAbRangedTE TE;
        bool Result = false;
        var player = FUnit.GetPlayer();
        if (AoWHSMap.FTokenManager.PlayerReady(player))
        {
            TE = CreateTCAbRangedTE(FUnit, X, Y, FSelectedAbility, FSelectedSpell);
            if (TE != null)
            {
                var fUnitPlayer = FUnit.GetPlayer();
                AoWHSMap.FTokenManager.GetTokenControl(fUnitPlayer).AddEvent(TE);
                TE.Release();
                Result = true;
            }
        }

        return Result;
    }

    // intf_P2_03.inc:74


}

// FPathInvalid. FTargetImages. FRangedPathHS. FTargetX. FTargetY. FCursorImage. FCursorFrame.
// FRangedPath. FObstacles
public partial class TCombatUnitSelectionControl : TMoveSelector
{
    // AoWTC.pas:2723
    public TCombatUnitSelectionControl() : base()
    {
        FRangedPath = new TXYZList();
        FObstacles = new TList();
        FRangedPathHS = new TRangedPathHS(null);
        FRangedPathHS.Activate();
        FTargetImages = AoWHSSet.ImageLibraryManager.LinkToIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), "TargHex.ILB");
    }

    public TSet<AnonymousEnum_piMove> FPathInvalid;
    public TCustomImageLibrary FTargetImages;
    public TRangedPathHS FRangedPathHS;
    public int FTargetX;
    public int FTargetY;
    public ushort FCursorImage;
    public ushort FCursorFrame;
    public THexagonSprite FMouseOverTarget;
    public TXYZList FRangedPath;
    public TList FObstacles;
    // AoWTC.pas:2733
    public override void Destroy()
    {
        for (int I = 0, last = FObstacles.Count - 1; I <= last; I++)
        {
            ((TObject)FObstacles[I]).Free();
        }

        FObstacles.Free();
        FRangedPath.Free();
        FRangedPathHS.Free();
        AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), "TargHex.ILB");
        base.Destroy();
    }

    // AoWTC.pas:3080
    public override void MouseDownEvent(TObject Sender)
    {
        TTacticalCombatUnitHS UnitHS;
        int Index;
        int DX;
        int DY;
        TSet<TCombatTargetTypeElement> Targets;
        TSpellCombatInfo SpellInfo = default;
        TAbilityCombatInfo AbilityInfo = default;
        if (AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer == AoWTCUnit.AoWCombatMap.FCurrentCombatPlayer)
        {
            if (!AoWTCUnit.AoWCombatMap.FPlayers.GetPlayers(AoWTCUnit.AoWCombatMap.FCurrentCombatPlayer).FAuto)
            {
                if (AoWTCUnit.AoWCombatMap.Mouse.Button == TMouseButton.mbRight)
                {
                    if (GetTCUnitHS() != null && GetTCUnitHS().FSelectedAbility > -1)
                    {
                        GetTCUnitHS().SelectAbility(-1);
                        return;
                    }

                    Unselect();
                }
                else
                {
                    if (AoWTCUnit.AoWCombatMap.Mouse.Shift.Contains(TShiftState.ssShift))
                    {
                        if (FMoveSettingsList.GetCount() == 1 && FMoveSettingsList.GetMoveSettings(0).FPath.Count != 0)
                        {
                            var pathL = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                            var pathY = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                            var pathX = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                            if (!ValidMultiMoveSelection((TAoWMapField)AoWTCUnit.AoWCombatMap.MapContainer.GetField(pathX.X, pathY.Y, pathL.L)))
                            {
                                return;
                            }
                        }

                        if (AoWTCUnit.AoWCombatMap.Mouse.Xhx != -1)
                        {
                            var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                            var xhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
                            UnitHS = (TTacticalCombatUnitHS)AoWTCUnit.AoWCombatMap.MapContainer.GetField(xhx, yhx, AoWTCUnit.AoWCombatMap.ViewL).FindOwnedChild(2228484);
                            if (UnitHS != null && UnitHS.FUnit.FCombatPlayer == AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer)
                            {
                                Index = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                if (Index == -1)
                                {
                                    AddUnit(UnitHS);
                                    Update();
                                    AoWTCUnit.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
                                    if (UnitHS.FUnit.GetLocked())
                                    {
                                        var indexOfMoveObject = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                        FMoveSettingsList.GetMoveSettings(indexOfMoveObject).FPath.Clear();
                                    }
                                    else
                                    {
                                        if (FMoveSettingsList.GetMoveSettings(0).FPath.Count > 0)
                                        {
                                            var moveSettingPath = FMoveSettingsList.GetMoveSettings(0);
                                            var path = FMoveSettingsList.GetMoveSettings(0).FPath[moveSettingPath.FPath.Count - 1];
                                            DX = UnitHS.GetXhx() - path.X;
                                            var moveSetting = FMoveSettingsList.GetMoveSettings(0);
                                            var pathY = FMoveSettingsList.GetMoveSettings(0).FPath[moveSetting.FPath.Count - 1];
                                            DY = UnitHS.GetYhx() - pathY.Y;
                                            var path2 = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                            var pathX = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                            CalculateMovePath(FMoveSettingsList.IndexOfMoveObject(UnitHS), (sbyte)(pathX.X + DX), (sbyte)(path2.Y + DY), 0);
                                        }
                                    }

                                    SetCurrentMove(-1);
                                }
                                else
                                {
                                    UnitHS.Unselect();
                                    DeleteMoveObject(Index);
                                    SetCurrentMove(FMoveSettingsList.GetCount() - 1);
                                    Update();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (AoWTCUnit.AoWCombatMap.Mouse.Shift.Contains(TShiftState.ssCtrl))
                        {
                            if (FMoveSettingsList.GetCount() > 0 && AoWTCUnit.AoWCombatMap.Mouse.Xhx != -1)
                            {
                                var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                                var xhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
                                UnitHS = (TTacticalCombatUnitHS)AoWTCUnit.AoWCombatMap.MapContainer.GetField(xhx, yhx, AoWTCUnit.AoWCombatMap.ViewL).FindOwnedChild(2228484);
                                if (UnitHS != null && UnitHS.FUnit.FCombatPlayer == AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer)
                                {
                                    Index = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                    if (Index == -1)
                                    {
                                        AddUnit(UnitHS);
                                        Update();
                                        AoWTCUnit.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
                                        if (UnitHS.FUnit.GetLocked())
                                        {
                                            var indexOfMoveObject = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                            FMoveSettingsList.GetMoveSettings(indexOfMoveObject).FPath.Clear();
                                        }
                                        else
                                        {
                                            if (FMoveSettingsList.GetMoveSettings(0).FPath.Count > 0)
                                            {
                                                var moveSettingPath = FMoveSettingsList.GetMoveSettings(0);
                                                var path = FMoveSettingsList.GetMoveSettings(0).FPath[moveSettingPath.FPath.Count - 1];
                                                DX = UnitHS.GetXhx() - path.X;
                                                var moveSetting = FMoveSettingsList.GetMoveSettings(0);
                                                var pathY = FMoveSettingsList.GetMoveSettings(0).FPath[moveSetting.FPath.Count - 1];
                                                DY = UnitHS.GetYhx() - pathY.Y;
                                                var path2 = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                                var pathX = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                                CalculateMovePath(FMoveSettingsList.IndexOfMoveObject(UnitHS), (sbyte)(pathX.X + DX), (sbyte)(path2.Y + DY), 0);
                                            }
                                        }

                                        SetCurrentMove(FMoveSettingsList.IndexOfMoveObject(UnitHS));
                                    }
                                    else
                                    {
                                        if (FCurrentMove != Index)
                                        {
                                            SetCurrentMove(Index);
                                            Update();
                                        }
                                        else
                                        {
                                            SetCurrentMove(-1);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (AoWTCUnit.AoWCombatMap.Mouse.Xhx != -1)
                            {
                                if (GetTCUnitHS() != null)
                                {
                                    var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                                    var xhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
                                    UnitHS = (TTacticalCombatUnitHS)AoWTCUnit.AoWCombatMap.MapContainer.GetField(xhx, yhx, AoWTCUnit.AoWCombatMap.ViewL).FindOwnedChild(2228484);
                                    if (GetTCUnitHS().FSelectedAbility == -1)
                                    {
                                        Targets = TSet<TCombatTargetTypeElement>.Of(TCombatTargetTypeElement.cttEnemy);
                                    }
                                    else
                                    {
                                        if (GetTCUnitHS().FSelectedAbility == 52)
                                        {
                                            var tcUnitHS = GetTCUnitHS();
                                            var tcUnitHS2 = GetTCUnitHS();
                                            AoWHSSet.FSpellControl.GetSpell(tcUnitHS2.FSelectedSpell).GetCombatInfo(tcUnitHS.FUnit.FUnit, ref SpellInfo);
                                            Targets = SpellInfo.TargetTypes;
                                        }
                                        else
                                        {
                                            var tcUnitHS = GetTCUnitHS();
                                            var tcUnitHS2 = GetTCUnitHS();
                                            AoWHSSet.FAbilityControl.GetAbility(tcUnitHS2.FSelectedAbility).GetCombatInfo(tcUnitHS.FUnit.FUnit, ref AbilityInfo);
                                            Targets = AbilityInfo.TargetTypes;
                                        }
                                    }

                                    if (UnitHS == null)
                                    {
                                        ExecuteMoveSelection();
                                    }
                                    else
                                    {
                                        var player = GetTCUnitHS().FUnit.GetPlayer();
                                        if (UnitHS.FUnit.GetPlayer() == player && !Targets.Contains(TCombatTargetTypeElement.cttFriend))
                                        {
                                            AoWTCUnit.AoWCombatMap.Selection.UnselectLocation();
                                            AoWTCUnit.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
                                        }
                                        else
                                        {
                                            var fUnitPlayer = GetTCUnitHS().FUnit.GetPlayer();
                                            bool condition = UnitHS.FUnit.GetPlayer() == fUnitPlayer && Targets.Contains(TCombatTargetTypeElement.cttFriend);
                                            if (!condition)
                                            {
                                                var player2 = GetTCUnitHS().FUnit.GetPlayer();
                                                condition = UnitHS.FUnit.GetPlayer() != player2 && Targets.Contains(TCombatTargetTypeElement.cttEnemy);
                                            }

                                            if (condition)
                                            {
                                                ExecuteMoveSelection();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ExecuteMoveSelection();
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
            var xhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
            UnitHS = (TTacticalCombatUnitHS)AoWTCUnit.AoWCombatMap.MapContainer.GetField(xhx, yhx, AoWTCUnit.AoWCombatMap.ViewL).FindOwnedChild(2228484);
            AoWTCUnit.AoWCombatMap.Selection.UnselectLocation();
            if (UnitHS != null && AoWTCUnit.AoWCombatMap.Mouse.Button == TMouseButton.mbLeft)
            {
                AoWTCUnit.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
            }
        }
    }

    // AoWTC.pas:2823
    public override void CalculateMovePath(int Index, sbyte X, sbyte Y, sbyte L)
    {
        TTacticalCombatUnitHS UnitHS;
        TList Temporary;
        TMovementHS Movement;
        var moveSettings = FMoveSettingsList.GetMoveSettings(Index);
        Temporary = new TList();
        if (FMoveSettingsList.GetCount() > 0)
        {
            for (int I = 0, last = FMoveSettingsList.GetCount() - 1; I <= last; I++)
            {
                if (I != Index)
                {
                    if (FMoveSettingsList.GetMoveSettings(I).FPath.Count > 1)
                    {
                        Movement = new TMovementHS(null);
                        var moveSettingPath = FMoveSettingsList.GetMoveSettings(I);
                        var path = FMoveSettingsList.GetMoveSettings(I).FPath[moveSettingPath.FPath.Count - 1];
                        var moveSetting = FMoveSettingsList.GetMoveSettings(I);
                        var pathX = FMoveSettingsList.GetMoveSettings(I).FPath[moveSetting.FPath.Count - 1];
                        AoWTCUnit.AoWCombatMap.PlaceHX(Movement, pathX.X, path.Y, 0);
                        Movement.FEnabled = true;
                        Temporary.Add(Movement);
                        Movement = new TMovementHS(null);
                        var pathY = FMoveSettingsList.GetMoveSettings(I).FPath[0];
                        var path2 = FMoveSettingsList.GetMoveSettings(I).FPath[0];
                        AoWTCUnit.AoWCombatMap.PlaceHX(Movement, path2.X, pathY.Y, 0);
                        Movement.FEnabled = false;
                        Temporary.Add(Movement);
                    }
                }
            }
        }

        moveSettings.FPathMovePoints = 0;
        UnitHS = (TTacticalCombatUnitHS)moveSettings.FMoveObject;
        if (GetTCUnitHS() != null && (GetTCUnitHS().FSelectedAbility == 117 || GetTCUnitHS().FSelectedAbility == 65))
        {
            ((TCombatMoveControlSettings)FMoveControlSettings).FMoveFlags = TSet<TCombatMoveModeElement>.FromMask((byte)FPathInvalid.Image) + TSet<TCombatMoveModeElement>.Of(TCombatMoveModeElement.mmClimbing);
        }
        else
        {
            ((TCombatMoveControlSettings)FMoveControlSettings).FMoveFlags = TSet<TCombatMoveModeElement>.FromMask((byte)FPathInvalid.Image);
        }

        FMoveControlSettings.Reset();
        var lhx = UnitHS.GetLhx();
        var yhx = UnitHS.GetYhx();
        var xhx = UnitHS.GetXhx();
        FMoveControlSettings.SetSourcePos(xhx, yhx, lhx);
        var player = UnitHS.FUnit.GetPlayer();
        FMoveControlSettings.FPlayer = player;
        FMoveControlSettings.FArmyHS = UnitHS;
        FMoveControlSettings.FSelection = 0;
        FMoveControlSettings.SetDestinationPos(X, Y, L);
        UnitHS.FUnit.FUnit.CreateMovePointTable(ref FMoveControlSettings.FMovePointTable);
        AoWTCUnit.AoWCombatMap.FMoveControl.ProcessMove(FMoveControlSettings);
        FMoveSettingsList.GetMoveSettings(Index).FPath.Assign(FMoveControlSettings.FPath);
        FMoveSettingsList.GetMoveSettings(Index).FMovePoints = UnitHS.FUnit.FMovePoints;
        var moves = UnitHS.FUnit.GetMoves();
        FMoveSettingsList.GetMoveSettings(Index).FMoves = moves;
        if (FMoveControlSettings.FPath.Count != 0)
        {
            for (int I = 0, last = FMoveControlSettings.FPath.Count - 2; I <= last; I++)
            {
                var pathY = FMoveControlSettings.FPath[I];
                var pathX = FMoveControlSettings.FPath[I];
                bool condition = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y].FindOwnedChild(2229508) != null;
                if (!condition)
                {
                    var path = FMoveControlSettings.FPath[I];
                    var path2 = FMoveControlSettings.FPath[I];
                    condition = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[path2.X, path.Y].FindOwnedChild(2229510) != null;
                }

                if (condition)
                {
                    var path = FMoveControlSettings.FPath[I];
                    var path2 = FMoveControlSettings.FPath[I];
                    var mapField = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[path2.X, path.Y];
                    moveSettings.FPath.List[I].Flag = FMoveControlSettings.FMovePointTable[mapField.TerrainType][-1 - -1];
                }
                else
                {
                    var path = FMoveControlSettings.FPath[I];
                    var path2 = FMoveControlSettings.FPath[I];
                    var item = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[path2.X, path.Y];
                    var path3 = FMoveControlSettings.FPath[I];
                    var path4 = FMoveControlSettings.FPath[I];
                    var fieldItem = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[path4.X, path3.Y];
                    moveSettings.FPath.List[I].Flag = FMoveControlSettings.FMovePointTable[fieldItem.TerrainType][item.OverlayType - -1];
                }
            }

            moveSettings.FPathMovePoints = moveSettings.FPath.PosToMovePoints(0);
            FMoveControlSettings.FPath.Clear();
        }

        for (int I = 0, last = Temporary.Count - 1; I <= last; I++)
        {
            ((TObject)Temporary[I]).Free();
        }

        Temporary.Free();
    }

    // AoWTC.pas:3178
    public override void UpdateMoveCursor()
    {
        TAoWMapField Field;
        int Cost;
        TTacticalCombatUnitHS UnitHS;
        TTacticalCombatUnitHS TargetHS;
        TSet<TCombatTargetTypeElement> Targets;
        TCityWall Wall;
        TSpellCombatInfo SpellInfo = default;
        TAbilityCombatInfo AbilityInfo = default;
        if (AoWTCUnit.AoWCombatMap == null)
        {
            return;
        }

        UnitHS = GetTCUnitHS();
        AoWTCUnit.AoWCombatMap.Mouse.CursorMode = 0;
        if (AoWTCUnit.AoWCombatMap.Mouse.Xhx == -1)
        {
            return;
        }

        if (AoWTCUnit.AoWCombatMap.Mouse.Yhx == -1)
        {
            return;
        }

        if (UnitHS == null)
        {
            return;
        }

        var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
        var xhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
        Field = (TAoWMapField)AoWTCUnit.AoWCombatMap.MapContainer.GetField(xhx, yhx, AoWTCUnit.AoWCombatMap.ViewL);
        if (AoWTCUnit.AoWCombatMap.FExplorationEnabled && !Field.GetExplored(AoWTCUnit.AoWCombatMap.FSeatedPlayer))
        {
            goto FinishCursor;
        }

        TargetHS = (TTacticalCombatUnitHS)Field.FindOwnedChild(2228484);
        if (UnitHS.FUnit.FCombatPlayer != AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer && TargetHS != null)
        {
            AoWTCUnit.AoWCombatMap.Mouse.CursorMode = 257;
            goto FinishCursor;
        }

        if (UnitHS.FSelectedAbility > -1)
        {
            FCursorFrame = (ushort)(FCursorFrame + 1);
            if (FCursorFrame >= 5)
            {
                FCursorFrame = 0;
                FCursorImage = (ushort)(FCursorImage + 1);
            }

            if (UnitHS.FSelectedAbility == 52)
            {
                AoWHSSet.FSpellControl.GetSpell(UnitHS.FSelectedSpell).GetCombatInfo(UnitHS.FUnit.FUnit, ref SpellInfo);
                Targets = SpellInfo.TargetTypes;
            }
            else
            {
                AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatInfo(UnitHS.FUnit.FUnit, ref AbilityInfo);
                Targets = AbilityInfo.TargetTypes;
            }

            if (TargetHS == null || (TargetHS == UnitHS && !Targets.Contains(TCombatTargetTypeElement.cttFriend)))
            {
                if (FCursorImage < 85 || FCursorImage > 88)
                {
                    FCursorImage = 85;
                }
            }
            else
            {
                if (AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatMode(UnitHS.FUnit.FUnit) == TCombatMode.cmTouch)
                {
                    if ((Targets.Contains(TCombatTargetTypeElement.cttFriend) && UnitHS.FUnit.GetSide() == TargetHS.FUnit.GetSide() || Targets.Contains(TCombatTargetTypeElement.cttEnemy) && UnitHS.FUnit.GetSide() != TargetHS.FUnit.GetSide()) && ((TTouchAbility)AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility)).CanTouch(UnitHS.FUnit, TargetHS.FUnit))
                    {
                        if (FCursorImage < 105 || FCursorImage > 107)
                        {
                            FCursorImage = 105;
                        }
                    }
                    else
                    {
                        FCursorImage = 89;
                    }
                }
                else
                {
                    if (Targets.Contains(TCombatTargetTypeElement.cttFriend) && UnitHS.FUnit.GetSide() == TargetHS.FUnit.GetSide() || Targets.Contains(TCombatTargetTypeElement.cttEnemy) && UnitHS.FUnit.GetSide() != TargetHS.FUnit.GetSide())
                    {
                        if (FCursorImage < 80 || FCursorImage > 83)
                        {
                            FCursorImage = 80;
                        }
                    }
                    else
                    {
                        FCursorImage = 89;
                    }
                }
            }

            AoWTCUnit.AoWCombatMap.Mouse.CursorMode = 256;
            AoWTCUnit.AoWCombatMap.Mouse.Cursor = FCursorImage;
        }
        else
        {
            if (TargetHS != null && TargetHS != UnitHS)
            {
                if (TargetHS.FUnit.GetPlayer() == UnitHS.FUnit.GetPlayer())
                {
                    AoWTCUnit.AoWCombatMap.Mouse.CursorMode = 257;
                }
                else
                {
                    if (UnitHS.FUnit.GetAbilityEnabled(1) || !TargetHS.FUnit.GetAbilityEnabled(1))
                    {
                        AoWTCUnit.AoWCombatMap.Mouse.CursorMode = 258;
                    }
                    else
                    {
                        FCursorImage = 89;
                        AoWTCUnit.AoWCombatMap.Mouse.CursorMode = 256;
                        AoWTCUnit.AoWCombatMap.Mouse.Cursor = FCursorImage;
                    }
                }
            }
        }

        FinishCursor:
            if (AoWTCUnit.AoWCombatMap.Mouse.CursorMode != 0)
            {
                return;
            }

        if (UnitHS.FUnit.GetPlayer() == AoWTCUnit.AoWCombatMap.FSeatedPlayer)
        {
            if (GetMoveTypes().Contains(TMoveType.mtWalking))
            {
                AoWTCUnit.AoWCombatMap.Mouse.Cursor = 64;
            }
            else
            {
                if (GetMoveTypes().Contains(TMoveType.mtSwimming))
                {
                    AoWTCUnit.AoWCombatMap.Mouse.Cursor = 66;
                }
                else
                {
                    if (GetMoveTypes().Contains(TMoveType.mtFlying))
                    {
                        AoWTCUnit.AoWCombatMap.Mouse.Cursor = 68;
                    }
                }
            }

            Cost = FMoveControlSettings.FMovePointTable[Field.TerrainType][Field.OverlayType - -1];
            if (UnitHS.FUnit.FMovePoints == 0)
            {
                Cost = 0;
            }
            else
            {
                Wall = (TCityWall)Field.FindOwnedChild(2229508);
                if (Wall == null)
                {
                    Wall = (TCityWall)Field.FindOwnedChild(2229510);
                }

                if (Wall != null)
                {
                    if (!(Wall is TCityWallDoor) || UnitHS.FUnit.GetSide() == TCombatSideIndex.csAttacker)
                    {
                        Cost = -1;
                    }

                    if (!(Wall is TCityWallDoor) && UnitHS.FUnit.FUnit.GetAbilityEnabled(36))
                    {
                        Cost = 1;
                    }

                    if (UnitHS.FUnit.FUnit.GetAbilityEnabled(66) || UnitHS.FUnit.FUnit.GetAbilityEnabled(1))
                    {
                        Cost = 1;
                    }
                }
            }

            if (Cost <= 0 || AoWTCUnit.AoWCombatMap.FExplorationEnabled && !Field.GetExplored(AoWTCUnit.AoWCombatMap.FSeatedPlayer))
            {
                AoWTCUnit.AoWCombatMap.Mouse.Cursor = AoWTCUnit.AoWCombatMap.Mouse.Cursor + 1;
            }

            AoWTCUnit.AoWCombatMap.Mouse.CursorMode = 256;
        }
    }

    // AoWTC.pas:2744
    public override TMoveControlSettings CreateMCS() => new TCombatMoveControlSettings(null);

    // AoWTC.pas:2754
    public override void SetCurrentMove(int Value)
    {
        if (FCurrentMove != -1 && GetTCUnitHS() != null)
        {
            var tcUnitHS = GetTCUnitHS();
            AoWTCUnit.AoWCombatMap.Selection.TriggerDeselectEvent(tcUnitHS);
        }

        base.SetCurrentMove(Value);
        if (FCurrentMove != -1 && GetTCUnitHS() != null)
        {
            var tcUnitHS = GetTCUnitHS();
            AoWTCUnit.AoWCombatMap.Selection.TriggerSelectEvent(tcUnitHS);
        }
    }

    // AoWTC.pas:3316
    public override void Unselect()
    {
        DeactivateRangedPath();
        if (GetTCUnitHS() != null)
        {
            GetTCUnitHS().FSelectedAbility = -1;
        }

        AoWTCUnit.AoWCombatMap.Selection.UnselectLocation();
        DeactivateMoveSelection();
        base.Unselect();
    }

    // AoWTC.pas:3274
    public override void ShowMovePaths(in TSurfaceDesc Surface)
    {
        int X = default;
        int Y = default;
        int Xhp = default;
        int Yhp = default;
        base.ShowMovePaths(Surface);
        if (GetTCUnitHS() != null)
        {
            if (GetTCUnitHS().FSelectedAbility != 65 && GetTCUnitHS().FSelectedAbility != 114)
            {
                return;
            }

            if (FMoveSettingsList.GetCount() == 1 && FMoveSettingsList.GetMoveSettings(0).FPath.Count > 1)
            {
                for (int I = 1; I <= 6; I++)
                {
                    var path = FMoveSettingsList.GetMoveSettings(0).FPath[1];
                    CenterHNtoHX(FMoveSettingsList.GetMoveSettings(0).FPath[1].X, path.Y, I, ref X, ref Y);
                    HXtoHP(X, Y, ref Xhp, ref Yhp);
                    FTargetImages[0].Show(Xhp - AoWTCUnit.AoWCombatMap.ViewXhp - 8, Yhp - AoWTCUnit.AoWCombatMap.ViewYhp + 2, Surface);
                }
            }
        }
    }

    // AoWTC.pas:2747
    public int AddUnit(TTacticalCombatUnitHS U)
    {
        int Result = AddMoveObject(U);
        FMoveSettingsList.GetMoveSettings(Result).FMovePoints = U.FUnit.FMovePoints;
        var moves = U.FUnit.GetMoves();
        FMoveSettingsList.GetMoveSettings(Result).FMoves = moves;
        return Result;
    }

    // AoWTC.pas:2763
    public TTacticalCombatUnitHS GetTCUnitHS()
    {
        TTacticalCombatUnitHS Result;
        if (FCurrentMove != -1)
        {
            Result = (TTacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(FCurrentMove).FMoveObject;
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // AoWTC.pas:2770
    public TSet<TMoveType> GetMoveTypes()
    {
        TSet<TMoveType> Result;
        if (FCurrentMove != -1)
        {
            Result = ((TTacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(FCurrentMove).FMoveObject).FUnit.FUnit.GetMoveTypes();
        }
        else
        {
            Result = TSet<TMoveType>.Of();
        }

        return Result;
    }

    // AoWTC.pas:2777
    public void RectangleSelectEvent(TObject Sender)
    {
        int X1 = default;
        int Y1 = default;
        int X2 = default;
        int Y2 = default;
        int PX = default;
        int PY = default;
        TTacticalCombatUnitHS UnitHS;
        TMapField Field;
        TMapLevel Level;
        TList Units;
        TRect R;
        Units = new TList();
        R = ((TAbstractAoWHSMapMouse)Sender).FSelectionRectangle;
        Level = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[AoWTCUnit.AoWCombatMap.ViewL];
        HPtoHX(R.Left, R.Top, ref X1, ref Y1);
        HPtoHX(R.Right, R.Bottom, ref X2, ref Y2);
        for (int X = X1 - 1, last = X2 + 1; X <= last; X++)
        {
            for (int Y = Y1 - 1, lastY = Y2 + 1; Y <= lastY; Y++)
            {
                HXtoHP(X, Y, ref PX, ref PY);
                PX += 16;
                PY += 8;
                if (X >= 0 && X < Level.Width && Y > 0 && Y < Level.Height && PX >= R.Left && PX < R.Right && PY >= R.Top && PY < R.Bottom)
                {
                    Field = Level.Field[X, Y];
                    UnitHS = (TTacticalCombatUnitHS)Field.FindOwnedChild(2228484);
                    if (UnitHS != null && UnitHS.FUnit.FCombatPlayer == AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer)
                    {
                        Units.Add(UnitHS);
                    }
                }
            }
        }

        if (Units.Count != 0)
        {
            Clear();
            AoWTCUnit.AoWCombatMap.Selection.UnselectLocation();
            for (int I = 0, last = Units.Count - 1; I <= last; I++)
            {
                AddUnit((TTacticalCombatUnitHS)Units[I]);
                var unit = Units[I];
                AoWTCUnit.AoWCombatMap.Selection.SelectLocationHS((THexagonSprite)unit);
            }

            if (Units.Count == 1)
            {
                SetCurrentMove(0);
            }
            else
            {
                SetCurrentMove(-1);
            }

            Update();
        }

        Units.Free();
    }

    // AoWTC.pas:2807
    public void MoveSelectionEvent(TObject Sender)
    {
        if (AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer == AoWTCUnit.AoWCombatMap.FCurrentCombatPlayer)
        {
            if (AoWTCUnit.AoWCombatMap.Mouse.Capture != this)
            {
                AoWTCUnit.AoWCombatMap.Selection.UnselectLocation();
            }
            else
            {
                if (AoWTCUnit.AoWCombatMap.Mouse.Button == TMouseButton.mbRight)
                {
                    if (FRangedPath.Count == 0)
                    {
                        AoWTCUnit.AoWCombatMap.Selection.UnselectLocation();
                    }
                    else
                    {
                        FRangedPath.Clear();
                        FMouseOverTarget = null;
                        AoWTCUnit.AoWCombatMap.FMouseOverTargetEvents.TriggerNotifyEvents(FMouseOverTarget);
                    }
                }
                else
                {
                    if (AoWTCUnit.AoWCombatMap.Mouse.Capture == this && AoWTCUnit.AoWCombatMap.Mouse.Button == TMouseButton.mbLeft)
                    {
                        ExecuteMoveSelection();
                    }
                }
            }
        }
    }

    // AoWTC.pas:2887
    public void CalculateRangedPath(sbyte X, sbyte Y)
    {
        TTacticalCombatUnitHS UnitHS;
        ushort Range;
        TSpellCombatInfo Info = default;
        int Direction;
        int NX;
        int NY;
        FRangedPath.Clear();
        Direction = 0;
        int last = FObstacles.Count - 1;
        for (; Direction <= last; Direction += 1)
        {
            ((TObject)FObstacles[Direction]).Free();
        }

        FObstacles.Clear();
        UnitHS = GetTCUnitHS();
        if (AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].TerrainType != 15)
        {
            if (UnitHS.FSelectedAbility == 52)
            {
                AoWHSSet.FSpellControl.GetSpell(UnitHS.FSelectedSpell).GetCombatInfo(UnitHS.FUnit.FUnit, ref Info);
                if (Info.Range > 0)
                {
                    Range = (ushort)Info.Range;
                }
                else
                {
                    Range = 100;
                }
            }
            else
            {
                var tcUnitHS = GetTCUnitHS();
                if (AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatMode(tcUnitHS.FUnit.FUnit) == TCombatMode.cmLongRange)
                {
                    Range = 12;
                }
                else
                {
                    var tcUnitHS2 = GetTCUnitHS();
                    if (AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatMode(tcUnitHS2.FUnit.FUnit) == TCombatMode.cmMediumRange)
                    {
                        Range = 8;
                    }
                    else
                    {
                        Range = 4;
                    }
                }
            }

            bool condition = UnitHS.FUnit.GetMoves() == UnitHS.FUnit.FMovePoints && X > 0;
            if (condition)
            {
                var yhx = UnitHS.GetYhx();
                condition = dHXtoRad(X, Y, UnitHS.GetXhx(), yhx) <= Range;
            }

            if (condition)
            {
                // { Original retains this unused with temporary. }
                GetTCUnitHS();
                FTargetX = X;
                FTargetY = Y;
                var yhx = UnitHS.GetYhx();
                Direction = dHXtoHN(UnitHS.GetXhx(), yhx, X, Y);
                NX = X;
                NY = Y;
                switch (GetTCUnitHS().FSelectedAbility)
                {
                    case 30:
                    case >= 86 and <= 90:
                        var unitHSYhx = UnitHS.GetYhx();
                        CenterHNtoHX(UnitHS.GetXhx(), unitHSYhx, AoWTCUnit.BreathHN[AoWTCUnit.BreathDirection[Direction - 1] * 12 - 12], ref NX, ref NY);
                        break;
                    case 52:
                        if (AoWTCUnit.SpellTypes[UnitHS.FSelectedSpell] == TfxType.fxBreath)
                        {
                            var yhx2 = UnitHS.GetYhx();
                            CenterHNtoHX(UnitHS.GetXhx(), yhx2, AoWTCUnit.BreathHN[AoWTCUnit.BreathDirection[Direction - 1] * 12 - 12], ref NX, ref NY);
                        }

                        break;
                }

                var xhx = GetTCUnitHS().GetXhx();
                var yhx3 = GetTCUnitHS().GetYhx();
                var side = GetTCUnitHS().FUnit.GetSide();
                var tcUnitHS = GetTCUnitHS();
                AoWTCUnit.MakeAbilPath(side, (short)GetTCUnitHS().FSelectedAbility, (short)tcUnitHS.FSelectedSpell, xhx, yhx3, (short)NX, (short)NY, ref FRangedPath, ref FObstacles);
            }
        }

        FRangedPathHS.FSelection = this;
        AoWTCUnit.AoWCombatMap.DynamicFixedVisibleHS.AddChild(FRangedPathHS);
    }

    // AoWTC.pas:2918
    public void MoveSelectedRoute()
    {
        TTacticalCombatUnitHS UnitHS;
        TEObject Target;
        int MovePoints;
        if (FMoveSettingsList.GetCount() > 0)
        {
            if (GetTCUnitHS() != null)
            {
                if (FMoveSettingsList.GetMoveSettings(0).FPath.Count > 0)
                {
                    var moveSettings = FMoveSettingsList.GetMoveSettings(0);
                    UnitHS = GetTCUnitHS();
                    if (UnitHS.FSelectedAbility != 117)
                    {
                        var pathY = moveSettings.FPath[0];
                        var pathX = moveSettings.FPath[0];
                        Target = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y].FindOwnedChild(2228484);
                    }
                    else
                    {
                        var pathY = moveSettings.FPath[0];
                        var pathX = moveSettings.FPath[0];
                        Target = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y].FindOwnedChild(2229508);
                        if (Target == null)
                        {
                            var path = moveSettings.FPath[0];
                            var path2 = moveSettings.FPath[0];
                            Target = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[path2.X, path.Y].FindOwnedChild(2229510);
                        }
                    }

                    if (FMoveSettingsList.GetMoveSettings(0).FPath.MovePointsToPos(UnitHS.FUnit.FMovePoints) == 0 && (Target != null || UnitHS.FSelectedAbility == 65 || UnitHS.FSelectedAbility == 114))
                    {
                        MovePoints = GetTCUnitHS().FUnit.FMovePoints;
                        if (MovePoints > 0)
                        {
                            if (UnitHS.FSelectedAbility == -1)
                            {
                                if (GetTCUnitHS().MeleeMoveTC(moveSettings.FPath))
                                {
                                    moveSettings.FPath.Clear();
                                }
                            }
                            else
                            {
                                if (AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatMode(UnitHS.FUnit.FUnit) == TCombatMode.cmTouch)
                                {
                                    if (GetTCUnitHS().AbTouchMoveTC(moveSettings.FPath))
                                    {
                                        moveSettings.FPath.Clear();
                                        UpdateMoveCursor();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (MoveCombatUnit(UnitHS.FUnit, moveSettings.FPath))
                        {
                            moveSettings.FPath.Clear();
                        }
                    }
                }
            }
        }
    }

    // AoWTC.pas:2943
    public void MoveUnits()
    {
        if (FMoveSettingsList.GetCount() > 0)
        {
            if (FMoveSettingsList.GetCount() > 1)
            {
                if (SetupMultiTCUnitMove(FMoveSettingsList))
                {
                    for (int I = 0, last = FMoveSettingsList.GetCount() - 1; I <= last; I++)
                    {
                        var moveSettings = FMoveSettingsList.GetMoveSettings(I);
                        moveSettings.FPath.Clear();
                    }
                }

                AoWTCUnit.AoWCombatMap.Selection.UnselectLocation();
                DeactivateMoveSelection();
                Clear();
            }
            else
            {
                MoveSelectedRoute();
            }
        }
    }

    // AoWTC.pas:2956
    public void ExecuteMoveSelection()
    {
        TTacticalCombatUnitHS UnitHS;
        TfxType EffectType;
        int X = default;
        int Y = default;
        int Direction;
        bool AllPathsValid()
        {
            int I;
            bool condition = FMoveSettingsList.GetMoveSettings(0).FPath.Count > 0;
            if (condition)
            {
                var path = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                var xhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
                condition = path.X == xhx;
            }

            bool test = condition;
            if (test)
            {
                var path = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                test = path.Y == yhx;
            }

            bool Result = test;
            I = FMoveSettingsList.GetCount();
            while (Result && I != 0)
            {
                I -= 1;
                var moveSettings = FMoveSettingsList.GetMoveSettings(I);
                Result = moveSettings.FPath.Count != 0;
            }

            return Result;
        }

        DeactivateRangedPath();
        if (FMoveSettingsList.GetCount() > 1)
        {
            if (FCurrentMove == -1)
            {
                if (!AllPathsValid())
                {
                    bool notExplorationEnabled = !AoWTCUnit.AoWCombatMap.FExplorationEnabled;
                    if (!notExplorationEnabled)
                    {
                        var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                        var xhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
                        notExplorationEnabled = ((TAoWMapField)AoWTCUnit.AoWCombatMap.MapContainer.GetField(xhx, yhx, AoWTCUnit.AoWCombatMap.ViewL)).GetExplored(AoWTCUnit.AoWCombatMap.FSeatedPlayer);
                    }

                    if (notExplorationEnabled)
                    {
                        for (int I = 0, last = FMoveSettingsList.GetCount() - 1; I <= last; I++)
                        {
                            FMoveSettingsList.GetMoveSettings(I).FPath.Clear();
                        }

                        for (int A = 0, last = FMoveSettingsList.GetCount() - 1; A <= last; A++)
                        {
                            for (int B = A + 1, lastB = FMoveSettingsList.GetCount() - 1; B <= lastB; B++)
                            {
                                var yhx = ((THexagonSprite)FMoveSettingsList.GetMoveSettings(B).FMoveObject).GetYhx();
                                var xhx = ((THexagonSprite)FMoveSettingsList.GetMoveSettings(B).FMoveObject).GetXhx();
                                var mouseYhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                                var dHXtoRad2 = dHXtoRad(AoWTCUnit.AoWCombatMap.Mouse.Xhx, mouseYhx, xhx, yhx);
                                var yhx2 = ((THexagonSprite)FMoveSettingsList.GetMoveSettings(A).FMoveObject).GetYhx();
                                var xhx2 = ((THexagonSprite)FMoveSettingsList.GetMoveSettings(A).FMoveObject).GetXhx();
                                var yhx3 = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                                if (dHXtoRad2 < dHXtoRad(AoWTCUnit.AoWCombatMap.Mouse.Xhx, yhx3, xhx2, yhx2))
                                {
                                    FMoveSettingsList.FChildren.Exchange(A, B);
                                }
                            }
                        }

                        if (((TTacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(0).FMoveObject).FUnit.GetLocked())
                        {
                            FMoveSettingsList.GetMoveSettings(0).FPath.Clear();
                        }
                        else
                        {
                            var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                            CalculateMovePath(0, (sbyte)AoWTCUnit.AoWCombatMap.Mouse.Xhx, (sbyte)yhx, (sbyte)AoWTCUnit.AoWCombatMap.ViewL);
                        }

                        if (FMoveSettingsList.GetMoveSettings(0).FPath.Count > 1)
                        {
                            for (int I = 1, last = FMoveSettingsList.GetCount() - 1; I <= last; I++)
                            {
                                if (((TTacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(I).FMoveObject).FUnit.GetLocked())
                                {
                                    FMoveSettingsList.GetMoveSettings(I).FPath.Clear();
                                }
                                else
                                {
                                    var yhx = ((THexagonSprite)FMoveSettingsList.GetMoveSettings(I).FMoveObject).GetYhx();
                                    var xhx = ((THexagonSprite)FMoveSettingsList.GetMoveSettings(I).FMoveObject).GetXhx();
                                    var yhx2 = ((THexagonSprite)FMoveSettingsList.GetMoveSettings(0).FMoveObject).GetYhx();
                                    Direction = dHXtoHN(((THexagonSprite)FMoveSettingsList.GetMoveSettings(0).FMoveObject).GetXhx(), yhx2, xhx, yhx);
                                    var path = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                    CenterHNtoHX(FMoveSettingsList.GetMoveSettings(0).FPath[0].X, path.Y, Direction, ref X, ref Y);
                                    CalculateMovePath(I, (sbyte)X, (sbyte)Y, 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int I = 0, last = FMoveSettingsList.GetCount() - 1; I <= last; I++)
                        {
                            FMoveSettingsList.GetMoveSettings(I).FPath.Clear();
                        }

                        return;
                    }
                }
                else
                {
                    MoveUnits();
                }
            }
            else
            {
                var moveSettings = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                bool condition = moveSettings.FPath.Count != 0;
                if (condition)
                {
                    var path = moveSettings.FPath[0];
                    var xhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
                    condition = path.X == xhx;
                }

                bool test = condition;
                if (test)
                {
                    var path = moveSettings.FPath[0];
                    var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                    test = path.Y == yhx;
                }

                if (test && moveSettings.FPath[0].L == AoWTCUnit.AoWCombatMap.ViewL)
                {
                    MoveUnits();
                    return;
                }

                if (((TTacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(FCurrentMove).FMoveObject).FUnit.GetLocked())
                {
                    FMoveSettingsList.GetMoveSettings(FCurrentMove).FPath.Clear();
                }
                else
                {
                    var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                    var xhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
                    CalculateMovePath(FCurrentMove, (sbyte)xhx, (sbyte)yhx, (sbyte)AoWTCUnit.AoWCombatMap.ViewL);
                }
            }
        }
        else
        {
            if (GetTCUnitHS().FSelectedAbility == -1 && GetTCUnitHS().GetXhx() == AoWTCUnit.AoWCombatMap.Mouse.Xhx && GetTCUnitHS().GetYhx() == AoWTCUnit.AoWCombatMap.Mouse.Yhx)
            {
                var lhx = GetTCUnitHS().GetLhx();
                var yhx = GetTCUnitHS().GetYhx();
                var xhx = GetTCUnitHS().GetXhx();
                AoWTCUnit.AoWCombatMap.Selection.SelectLocationHX(xhx, yhx, lhx);
                return;
            }

            if (!GetTCUnitHS().FUnit.GetLocked() && AoWHSMap.FTokenManager.PlayerReady(AoWTCUnit.AoWCombatMap.FCurrentPlayer))
            {
                UnitHS = GetTCUnitHS();
                if (AllPathsValid())
                {
                    MoveUnits();
                }
                else
                {
                    bool condition = FRangedPath.Count > 0;
                    if (condition)
                    {
                        var xhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
                        condition = FTargetX == xhx;
                    }

                    bool test = condition;
                    if (test)
                    {
                        var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                        test = FTargetY == yhx;
                    }

                    if (test)
                    {
                        if (UnitHS.AbRangedTC((ushort)FTargetX, (ushort)FTargetY))
                        {
                            FRangedPath.Clear();
                            UpdateMoveCursor();
                        }
                    }
                    else
                    {
                        if (UnitHS.FSelectedAbility == -1 && UnitHS.GetXhx() == AoWTCUnit.AoWCombatMap.Mouse.Xhx && UnitHS.GetYhx() == AoWTCUnit.AoWCombatMap.Mouse.Yhx)
                        {
                            var lhx = UnitHS.GetLhx();
                            var yhx = UnitHS.GetYhx();
                            var xhx = UnitHS.GetXhx();
                            AoWTCUnit.AoWCombatMap.Selection.SelectLocationHX(xhx, yhx, lhx);
                            return;
                        }

                        if (!UnitHS.Moving())
                        {
                            if (UnitHS.FSelectedAbility == -1)
                            {
                                if (UnitHS.FUnit.FMovePoints > 0 && !UnitHS.FUnit.GetLocked())
                                {
                                    if (AoWTCUnit.AoWCombatMap.FExplorationEnabled)
                                    {
                                        var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                                        var xhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
                                        if (!((TAoWMapField)AoWTCUnit.AoWCombatMap.MapContainer.GetField(xhx, yhx, AoWTCUnit.AoWCombatMap.ViewL)).GetExplored(AoWTCUnit.AoWCombatMap.FSeatedPlayer))
                                        {
                                            return;
                                        }
                                    }

                                    var mouseYhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                                    var mouseXhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
                                    CalculateMovePath(FCurrentMove, (sbyte)mouseXhx, (sbyte)mouseYhx, 0);
                                }
                            }
                            else
                            {
                                if (UnitHS.FSelectedAbility == 52)
                                {
                                    EffectType = AoWTCUnit.SpellTypes[UnitHS.FSelectedSpell];
                                }
                                else
                                {
                                    EffectType = AoWTCUnit.AbilTypes[UnitHS.FSelectedAbility];
                                }

                                switch (EffectType)
                                {
                                    case TfxType.fxNone:
                                        UnitHS.SelectAbility(-1);
                                        break;
                                    case TfxType.fxTouch:
                                        if (AoWTCUnit.AoWCombatMap.FExplorationEnabled)
                                        {
                                            var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                                            var xhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
                                            if (!((TAoWMapField)AoWTCUnit.AoWCombatMap.MapContainer.GetField(xhx, yhx, AoWTCUnit.AoWCombatMap.ViewL)).GetExplored(AoWTCUnit.AoWCombatMap.FSeatedPlayer))
                                            {
                                                return;
                                            }
                                        }

                                        if (!UnitHS.FUnit.GetLocked())
                                        {
                                            var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                                            CalculateMovePath(0, (sbyte)AoWTCUnit.AoWCombatMap.Mouse.Xhx, (sbyte)yhx, 0);
                                        }

                                        break;
                                    default:
                                    {
                                        var yhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                                        CalculateRangedPath((sbyte)AoWTCUnit.AoWCombatMap.Mouse.Xhx, (sbyte)yhx);
                                        if (FRangedPath.Count > 0)
                                        {
                                            var mouseYhx = AoWTCUnit.AoWCombatMap.Mouse.Yhx;
                                            var xhx = AoWTCUnit.AoWCombatMap.Mouse.Xhx;
                                            FMouseOverTarget = (THexagonSprite)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[xhx, mouseYhx].FindOwnedChild(2228484);
                                            AoWTCUnit.AoWCombatMap.FMouseOverTargetEvents.TriggerNotifyEvents(FMouseOverTarget);
                                        }
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

    // AoWTC.pas:3073
    public bool ValidMultiMoveSelection(TAoWMapField Field)
    {
        sbyte Player;
        Player = Field.GetPlayer();
        return Player == -1;
    }

    // AoWTC.pas:3259
    public void ActivateMoveSelection()
    {
        if (!FStatus.Contains(TMoveSelectorStatusElement.mssActivated))
        {
            Activate(AoWTCUnit.AoWCombatMap);
        }
    }

    // AoWTC.pas:3264
    public void DeactivateMoveSelection()
    {
        if (FStatus.Contains(TMoveSelectorStatusElement.mssActivated))
        {
            Deactivate();
            FRangedPath.Clear();
            FMoveSettingsList.Clear();
            AoWTCUnit.AoWCombatMap.MouseMoveEvent(this);
        }
    }

    // AoWTC.pas:3293
    public void Update()
    {
        if (GetTCUnitHS() != null)
        {
            GetTCUnitHS().FUnit.FUnit.CreateMovePointTable(ref FMoveControlSettings.FMovePointTable);
        }

        FPathInvalid = TSet<AnonymousEnum_piMove>.Of(AnonymousEnum_piMove.piRanged);
    }

    // AoWTC.pas:3300
    public void SetTCUnitHS(TTacticalCombatUnitHS U)
    {
        if (GetTCUnitHS() != U)
        {
            if (FMoveSettingsList.GetCount() <= 1)
            {
                Clear();
                if (U != null && U.FUnit.GetPlayer() == AoWTCUnit.AoWCombatMap.FSeatedPlayer)
                {
                    ActivateMoveSelection();
                    AddUnit(U);
                    SetCurrentMove(0);
                    Update();
                }
                else
                {
                    DeactivateMoveSelection();
                }

                FRangedPath.Clear();
                UpdateMoveCursor();
            }
        }
    }

    // AoWTC.pas:3325
    public void Select(TTacticalCombatUnitHS U)
    {
        AoWTCUnit.AoWCombatMap.Selection.UnselectLocation();
        if (U != null)
        {
            AoWTCUnit.AoWCombatMap.Selection.SelectLocationHS(U);
        }
    }

    // AoWTC.pas:3331
    public void DeactivateRangedPath()
    {
        if (FRangedPathHS.FSelection != null)
        {
            AoWTCUnit.AoWCombatMap.DynamicFixedVisibleHS.RemoveChild(FRangedPathHS);
            FRangedPathHS.FSelection = null;
        }
    }

}

// FAuto. FEndTurn. FIndex. FLastX. FLastY. FLastUnit
public partial class TTacticalCombatPlayer : TCombatPlayer
{
    public TTacticalCombatPlayer(TECustomNode owner) : base(owner)
    { }

    public bool FAuto;
    public bool FEndTurn;
    public sbyte FIndex;
    public int FLastX;
    public int FLastY;
    public TTacticalCombatUnit FLastUnit;
    // AoWTC.pas:6419
    public void SetAuto(bool Value)
    {
        FAuto = Value;
        if (AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer == FIndex && AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer == AoWTCUnit.AoWCombatMap.FCurrentCombatPlayer)
        {
            AoWTCUnit.AoWCombatMap.FAI.Reset();
        }
    }

}

// FPlayers
public partial class TTacticalCombatPlayerList : TObject
{
    public TCombatPlayerList FPlayers;
    // AoWTC.pas:6426
    public int GetCount()
    {
        int Result;
        if (FPlayers != null)
        {
            Result = FPlayers.GetCount();
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWTC.pas:6431
    public TTacticalCombatPlayer GetPlayers(sbyte Index) => (TTacticalCombatPlayer)FPlayers.GetPlayers(Index);

}

public partial class TAoWCombatMapMouse : TAbstractAoWHSMapMouse
{

    public TAoWCombatMapMouse() : base()
    {}

    // intf_P1_05.inc:68

}

// FPlayers. FCombat. FUnitPositions. FSeatedCombatPlayer. FCurrentCombatPlayer. turn/player
// events. FMoveControl. FUnitSelection. selection/unit events. FAI. FEffects. FWalls.
// FFleeLocations. FEndTurnPending. FPlaySounds. FPositionStatus. FRound
public partial class TAoWCombatMap : TAbstractAoWCombatMap
{
    // AoWTC.pas:2183
    public new const int ClassID = 2228480;

    public TTacticalCombatPlayerList FPlayers;
    public TTacticalCombat FCombat;
    public TUnitPositionControl FUnitPositions;
    public sbyte FSeatedCombatPlayer;
    public sbyte FCurrentCombatPlayer;
    public TEventList FChangeTurnEvents;
    public TEventList FPlayerChangedEvents;
    public TEventList FSeatedPlayerChangedEvents;
    public TCombatMoveControl FMoveControl;
    public TCombatUnitSelectionControl FUnitSelection;
    public TEventList FSelectEvents;
    public TEventList FDeselectEvents;
    public TMapObject FCenterViewLocation;
    public TEventList FMouseOverTargetEvents;
    // CMapCtrl
    public TEventList FUnitChangedEvents;
    // CMapCtrl
    public TCAI FAI;
    public TList FEffects;
    public TList FWalls;
    public TList FFleeLocations;
    public bool FEndTurnPending;
    public bool FPlaySounds;
    public FixedArray_byte_8 FPositionStatus;
    public int FRound;
    // AoWTC.pas:2213
    public override void Destroy()
    {
        while (FEffects.Count > 0)
        {
            var effect = FEffects[0];
            DynamicFixedVisibleHS.RemoveChild((THexagonSprite)effect);
            var effect2 = FEffects[0];
            DynamicVisibleHS.RemoveChild((THexagonSprite)effect2);
            ((TObject)FEffects[0]).Free();
            FEffects.Delete(0);
        }

        base.Destroy();
        AoWTCUnit.AoWCombatMap = null;
        FUnitPositions.Free();
        FChangeTurnEvents.Free();
        FPlayerChangedEvents.Free();
        FSeatedPlayerChangedEvents.Free();
        FSelectEvents.Free();
        FDeselectEvents.Free();
        FMouseOverTargetEvents.Free();
        FUnitChangedEvents.Free();
        FMoveControl.Free();
        FPlayers.Free();
        FUnitSelection.Free();
        FEffects.Free();
        FWalls.Free();
        FFleeLocations.Free();
        FAI.Free();
    }

    // AoWTC.pas:2189
    private protected sealed override void InitializeBeforeInherited() => AoWTCUnit.AoWCombatMap = this;

    // AoWTC.pas:2186
    public TAoWCombatMap(TECustomNode owner) : base(owner)
    {
        FUnitPositions = new TUnitPositionControl(null);
        FChangeTurnEvents = new TEventList();
        FPlayerChangedEvents = new TEventList();
        FSeatedPlayerChangedEvents = new TEventList();
        FMoveControl = new TCombatMoveControl(this);
        FSelectEvents = new TEventList();
        FDeselectEvents = new TEventList();
        FMouseOverTargetEvents = new TEventList();
        FUnitChangedEvents = new TEventList();
        FPlayers = new TTacticalCombatPlayerList();
        FUnitSelection = new TCombatUnitSelectionControl();
        ((TAoWCombatMapMouse)Mouse).FRectangleSelectEvents.Add(FUnitSelection.RectangleSelectEvent);
        FEffects = new TList();
        FWalls = new TList();
        FFleeLocations = new TList();
        FAI = new TCAI();
        Mouse.SetCapture(this);
        Mouse.OnMove.Add(MouseMoveEvent);
        Mouse.CursorMode = 256;
        for (int I = 0; I <= 7; I++)
        {
            FPositionStatus[I] = 2;
        }
    }

    // AoWTC.pas:2249
    public override void MsgProc(ref TEMsg Msg, TEObject Sender)
    {
        if (Msg.MsgID == 536870920 && !Status.Contains(THSMapStatusElement.msEditting))
        {
            SetCombat((TTacticalCombat)AoWHSMap.FCombat);
        }

        base.MsgProc(ref Msg, Sender);
    }

    // AoWTC.pas:2240
    public override THSMapMouse CreateMouse() => new TAoWCombatMapMouse();

    // AoWTC.pas:2267
    public override void InitializeNewMap()
    {
        THexagonControl Control;
        for (int X = 6, last = MapContainer.Width - 7; X <= last; X++)
        {
            for (int Y = 6, lastY = MapContainer.Height - 7; Y <= lastY; Y++)
            {
                AoWHSSet.FCombatHexagons.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 1);
            }
        }

        Control = Engine.HSSet.HexagonControl;
        for (int X = 0, last = MapContainer.Width - 1; X <= last; X++)
        {
            for (int Y = 0; Y <= 5; Y++)
            {
                Control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 15);
            }

            for (int Y = MapContainer.Height - 6, lastY = MapContainer.Height - 1; Y <= lastY; Y++)
            {
                Control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 15);
            }
        }

        for (int Y = 6, lastY = MapContainer.Height - 7; Y <= lastY; Y++)
        {
            for (int X = 0; X <= 5; X++)
            {
                Control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 15);
            }

            for (int X = MapContainer.Width - 6, last = MapContainer.Width - 1; X <= last; X++)
            {
                Control.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 15);
            }
        }
    }

    // AoWTC.pas:2338
    public override void DrawDynamicScene(ref TSurfaceDesc SD)
    {
        base.DrawDynamicScene(ref SD);
        if (AoWHSMap != null)
        {
            DrawFog(SD);
        }

        if (FShowGrid)
        {
            DrawGrid(SD);
        }

        DrawMoveRoute(SD);
        if (((TAoWCombatMapMouse)Mouse).FSelectionRectangle.Left != -1)
        {
            DrawSelectionRect(ViewXhp, ViewYhp, ((TAoWCombatMapMouse)Mouse).FSelectionRectangle, SD);
        }
    }

    // AoWTC.pas:2429
    public override void NewFrame()
    {
        int I;
        TfxHS Effect;
        if (!Status.Contains(THSMapStatusElement.msEditting))
        {
            I = 0;
            while (I < FEffects.Count)
            {
                Effect = (TfxHS)FEffects[I];
                if (Effect.FDone)
                {
                    DynamicFixedVisibleHS.RemoveChild(Effect);
                    DynamicVisibleHS.RemoveChild(Effect);
                    Effect.Free();
                    FEffects.Delete(I);
                }
                else
                {
                    I += 1;
                }
            }
        }

        base.NewFrame();
        if (!Status.Contains(THSMapStatusElement.msEditting) && FCombat != null)
        {
            UpdateCursor();
            if (FEndTurnPending && (AoWHSMap.FPlayers.GetPlayers(FCurrentPlayer).FPlayerType != TPlayerType.ptHuman && AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FCombat.FHostPlayer).ConnectionMode != TConnectionMode.cmRemote || FPlayers.GetPlayers(FCurrentCombatPlayer).FAuto && AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FCurrentPlayer).ConnectionMode != TConnectionMode.cmRemote))
            {
                switch (FAI.Status)
                {
                    case TaiStatus.aiIdle:
                        AoWEngine.OutputDebugStr("AI for Player " + IntToStr(FCurrentPlayer) + " turn begins.");
                        FAI.BeginTurn();
                        break;
                    case TaiStatus.aiBusy:
                        FAI.ContinueTurn();
                        break;
                    case TaiStatus.aiDone:
                        if (EndTurn(true))
                        {
                            FAI.Status = TaiStatus.aiIdle;
                            AoWEngine.OutputDebugStr("AI for Player " + IntToStr(FCurrentPlayer) + " turn ends.");
                        }

                        break;
                }
            }
        }
    }

    // AoWTC.pas:2515
    public override void DropHeroItem(THero Hero, TItem Item)
    {
        sbyte X;
        sbyte Y;
        sbyte L;
        TItemHS HS;
        int I;
        X = -1;
        Y = -1;
        L = -1;
        I = FCombat.FCombatData.GetCount();
        while (I != 0 && X == -1)
        {
            I -= 1;
            if (FCombat.FCombatData.GetObjects(I) is TTacticalCombatUnit && ((TTacticalCombatUnit)FCombat.FCombatData.GetObjects(I)).FUnit == Hero)
            {
                X = ((TTacticalCombatUnit)FCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx();
                Y = ((TTacticalCombatUnit)FCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
                L = 0;
            }
        }

        if (X > -1)
        {
            HS = (TItemHS)MapContainer.GetField(X, Y, L).FindOwnedChild(131697);
            if (HS == null)
            {
                HS = new TCItemHS(null);
                // { Original bug: a failed placement leaves the new item sprite allocated. }
                if (AoWTCUnit.AoWCombatMap.PlaceHX(HS, X, Y, L))
                {
                    Item.SetOwner(HS.FItems);
                }
            }
            else
            {
                Item.SetOwner(HS.FItems);
            }
        }
    }

    // AoWTC.pas:2544
    public override TItemList GetHeroItemsOnGround(THero Hero)
    {
        TItemHS HS;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        TItemList Result = null;
        GetUnitLocation(Hero, ref X, ref Y, ref L);
        if (X > -1)
        {
            HS = (TItemHS)MapContainer.GetField(X, Y, L).FindOwnedChild(131697);
            if (HS != null)
            {
                Result = HS.FItems;
            }
        }

        return Result;
    }

    // AoWTC.pas:2243
    public void SetCombat(TTacticalCombat Combat)
    {
        FCombat = Combat;
        if (FCombat != null)
        {
            FPlayers.FPlayers = FCombat.FCombatData.FPlayers;
        }
    }

    // AoWTC.pas:2256
    public void MouseMoveEvent(TObject Sender)
    {
        TTacticalCombatUnitHS HS;
        if (!AoWTCUnit.AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting) && !FPlayers.GetPlayers(FCurrentCombatPlayer).FAuto && Mouse.Xhx != -1 && Mouse.Yhx != -1 && Mouse.Capture == null)
        {
            Mouse.CursorMode = 0;
            var yhx = Mouse.Yhx;
            var xhx = Mouse.Xhx;
            HS = (TTacticalCombatUnitHS)MapContainer.MapLevels[0].Field[xhx, yhx].FindOwnedChild(2228484);
            if (HS != null)
            {
                Mouse.CursorMode = 257;
            }
        }
    }

    // AoWTC.pas:2288
    public void SetSeatedCombatPlayer(sbyte Player)
    {
        FSeatedCombatPlayer = Player;
        if (Player == -1)
        {
            SetSeatedPlayer(-1);
        }
        else
        {
            SetSeatedPlayer(FCombat.FCombatData.FPlayers.GetPlayers(FSeatedCombatPlayer).FPlayer);
        }

        FSeatedPlayerChangedEvents.TriggerNotifyEvents(IntToObject(FCurrentCombatPlayer));
    }

    // AoWTC.pas:2296
    public void NewTurn(sbyte Player)
    {
        TTacticalCombatUnit U;
        TEMsg Msg = default;
        FCurrentCombatPlayer = Player;
        if (Player == 0)
        {
            FRound += 1;
        }

        SetCurrentPlayer(FPlayers.GetPlayers(FCurrentCombatPlayer).FPlayer);
        FCombat.NewTurn((byte)FCurrentCombatPlayer);
        FUnitSelection.Unselect();
        AoWHSMap.FTokenManager.TokenExecuter.ActivateTC(FCurrentPlayer);
        if (FPlayers.GetPlayers(FCurrentCombatPlayer).FPlayer == 0)
        {
            var ind = TranslateRStr(AoWTCUnit.IndRStr);
            var beginRound = TranslateRStr(AoWTCUnit.BeginRoundRStr);
            ShowMessage(ind + " " + beginRound);
        }
        else
        {
            var player = FPlayers.GetPlayers(FCurrentCombatPlayer);
            var playerPlayerRace = AoWHSMap.FPlayers.GetPlayers(player.FPlayer);
            var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[playerPlayerRace.FPlayerRace]);
            var beginRound = TranslateRStr(AoWTCUnit.BeginRoundRStr);
            ShowMessage(raceNamesPlural + " " + beginRound);
        }

        var playerPlayer = FPlayers.GetPlayers(FCurrentCombatPlayer);
        if (AoWHSMap.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType == TPlayerType.ptHuman)
        {
            var player = FPlayers.GetPlayers(FCurrentCombatPlayer);
            if (AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(player.FPlayer).ConnectionMode != TConnectionMode.cmRemote)
            {
                SetSeatedCombatPlayer(FCurrentCombatPlayer);
                if (FPlayers.GetPlayers(FCurrentCombatPlayer).FLastX == 0)
                {
                    U = GetNextUnit(null, true, true);
                    if (U != null)
                    {
                        var xhx = U.FUnitHS.GetXhx();
                        FPlayers.GetPlayers(FCurrentCombatPlayer).FLastX = xhx;
                        var yhx = U.FUnitHS.GetYhx();
                        FPlayers.GetPlayers(FCurrentCombatPlayer).FLastY = yhx;
                        if (FExplorationEnabled)
                        {
                            for (int I = 0, last = FPlayers.GetCount() - 1; I <= last; I++)
                            {
                                if (FCurrentCombatPlayer != I)
                                {
                                    var fUnitHSXhx = U.FUnitHS.GetXhx();
                                    FPlayers.GetPlayers((sbyte)I).FLastX = fUnitHSXhx;
                                    var fUnitHSYhx = U.FUnitHS.GetYhx();
                                    FPlayers.GetPlayers((sbyte)I).FLastY = fUnitHSYhx;
                                }
                            }
                        }
                    }
                }

                var playerLastY = FPlayers.GetPlayers(FSeatedCombatPlayer);
                CenterViewTo(FPlayers.GetPlayers(FSeatedCombatPlayer).FLastX, playerLastY.FLastY, 0);
            }
        }

        PlaySample("Sfx\\Events\\NewEvent.wav");
        InitMsg(ref Msg, 537006081, this);
        SendMsg(ref Msg, this);
        if (FExplorationEnabled)
        {
            var rect = Rect(0, 0, 65, 58);
            TriggerMapChangedEvent(rect, 0);
        }

        FUnitSelection.Unselect();
        FChangeTurnEvents.TriggerNotifyEvents(IntToObject(FCurrentCombatPlayer));
    }

    // AoWTC.pas:2348
    public void DrawMoveRoute(in TSurfaceDesc SD)
    {
        FUnitSelection.ShowMovePaths(SD);
    }

    // AoWTC.pas:2351
    public bool EndTurn(bool Automatic)
    {
        TCEndTurnTE TE;
        bool Result = false;
        if (!Automatic)
        {
            if (AoWHSMap.FPlayers.GetPlayers(FCurrentPlayer).FPlayerType != TPlayerType.ptHuman)
            {
                return Result;
            }

            if (AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer != AoWTCUnit.AoWCombatMap.FCurrentCombatPlayer)
            {
                return Result;
            }
        }

        if (AoWHSMap.FTokenManager.PlayerReady(FCurrentPlayer))
        {
            TE = CreateTCEndTurnTE(FCurrentPlayer);
            if (TE != null)
            {
                AoWHSMap.FTokenManager.GetTokenControl(FCurrentPlayer).AddEvent(TE);
                TE.Release();
                Result = true;
                FEndTurnPending = false;
            }
        }

        return Result;
    }

    // AoWTC.pas:2370
    public void CancelEndTurn()
    {
    }

    // AoWTC.pas:2373
    // { Intentionally empty in the original. }
    public bool GetEndTurnStatus()
    {
        bool Result = true;
        for (int I = 0, last = FPlayers.GetCount() - 1; I <= last; I++)
        {
            Result = Result && FPlayers.GetPlayers((sbyte)I).FEndTurn;
        }

        return Result;
    }

    // AoWTC.pas:2381
    public TTacticalCombatUnit GetNextUnit(TTacticalCombatUnit U, bool AllUnits, bool Forward)
    {
        int Index;
        int I;
        int Start;
        int Step;
        if (U != null && !U.GetEnabled())
        {
            U = null;
        }

        TTacticalCombatUnit Result = U;
        if (Forward)
        {
            Step = 1;
        }
        else
        {
            Step = -1;
        }

        Index = 0;
        I = 0;
        Start = 0;
        if (U != null)
        {
            while (I < this.FCombat.FCombatData.GetCount())
            {
                if (this.FCombat.FCombatData.GetObjects(I) == U)
                {
                    Index = I + Step;
                    Start = I;
                    I = this.FCombat.FCombatData.GetCount();
                }

                I += 1;
            }
        }

        while (Index < this.FCombat.FCombatData.GetCount() && Index > -1)
        {
            if (this.FCombat.FCombatData.GetObjects(Index) != null && this.FCombat.FCombatData.GetObjects(Index) is TCombatUnit && (AllUnits || ((TTacticalCombatUnit)this.FCombat.FCombatData.GetObjects(Index)).FMovePoints > 0) && this.FCombat.FCombatData.GetObjects(Index).GetEnabled() && this.FCombat.FCombatData.GetObjects(Index).FCombatPlayer == FCurrentCombatPlayer)
            {
                Result = (TTacticalCombatUnit)this.FCombat.FCombatData.GetObjects(Index);
                Index = -10;
            }

            Index = Index + Step;
        }

        if (Result == U)
        {
            if (Forward)
            {
                Index = 0;
            }
            else
            {
                Index = this.FCombat.FCombatData.GetCount() - 1;
            }

            while (Index != Start && Index > -1 && Index < this.FCombat.FCombatData.GetCount())
            {
                if (this.FCombat.FCombatData.GetObjects(Index) != null && this.FCombat.FCombatData.GetObjects(Index) is TCombatUnit && (AllUnits || ((TTacticalCombatUnit)this.FCombat.FCombatData.GetObjects(Index)).FMovePoints > 0) && this.FCombat.FCombatData.GetObjects(Index).GetEnabled() && this.FCombat.FCombatData.GetObjects(Index).FCombatPlayer == FCurrentCombatPlayer)
                {
                    Result = (TTacticalCombatUnit)this.FCombat.FCombatData.GetObjects(Index);
                    Index = -10;
                }

                Index = Index + Step;
            }
        }

        return Result;
    }

    // AoWTC.pas:2466
    public TfxHS AddEffect(TfxType EffectType)
    {
        TfxHS Result = new TfxHS(null);
        FEffects.Add(Result);
        Result.Activate();
        Result.FEffectType = EffectType;
        DynamicFixedVisibleHS.AddChild(Result);
        return Result;
    }

    // AoWTC.pas:2475
    public void UpdateCursor()
    {
        if (!((FSeatedPlayer == -1 || AoWHSMap.FTokenManager.PlayerReady(FSeatedPlayer)) && FSeatedPlayer == FCurrentPlayer && !FPlayers.GetPlayers(FCurrentCombatPlayer).FAuto && AoWTCUnit.AoWCombatMap.FSeatedCombatPlayer == AoWTCUnit.AoWCombatMap.FCurrentCombatPlayer))
        {
            Mouse.Cursor = (int)(((uint)Frame) >> 2) % 4 + 6;
        }
        else
        {
            if (FUnitSelection.GetTCUnitHS() != null)
            {
                FUnitSelection.UpdateMoveCursor();
            }

            switch (Mouse.CursorMode)
            {
                case 0:
                    Mouse.Cursor = 0;
                    break;
                case 257:
                    Mouse.Cursor = (int)(((uint)Frame) >> 2) % 4 + 52;
                    break;
                case 258:
                    Mouse.Cursor = (((int)(((uint)Frame) >> 2)) & 1) + 49;
                    break;
            }
        }
    }

    // AoWTC.pas:2489
    public void EndCombat(TObject Sender)
    {
        TTerminateCombatTE TE;
        TE = new TTerminateCombatTE(null);
        AoWHSMap.FTokenManager.TokenExecuter.AddSystemEvent(TE);
        TE.Release();
    }

    // AoWTC.pas:2497
    public void SelectNextUnit(bool Forward)
    {
        TTacticalCombatUnit U;
        int X = default;
        int Y = default;
        if (FSeatedCombatPlayer == FCurrentCombatPlayer)
        {
            U = GetNextUnit(FPlayers.GetPlayers(FSeatedCombatPlayer).FLastUnit, false, Forward);
            if (U != null)
            {
                if (FUnitSelection.GetTCUnitHS() != U.FUnitHS)
                {
                    FPlayers.GetPlayers(FSeatedCombatPlayer).FLastUnit = U;
                    var yhx = U.FUnitHS.GetYhx();
                    CenterViewTo(U.FUnitHS.GetXhx(), yhx, 0);
                    Selection.UnselectLocation();
                    var fUnitHSYhx = U.FUnitHS.GetYhx();
                    HXtoHP(U.FUnitHS.GetXhx(), fUnitHSYhx, ref X, ref Y);
                    Selection.SelectLocation(X, Y, 0);
                }
            }
        }
    }

    // AoWTC.pas:2557
    public void GetUnitLocation(TAbstractUnit U, ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        int I;
        X = -1;
        I = 0;
        while (I < FCombat.FCombatData.GetCount())
        {
            if (((TTacticalCombatUnit)FCombat.FCombatData.GetObjects(I)).FUnit == U && FCombat.FCombatData.GetObjects(I).GetEnabled())
            {
                X = ((TTacticalCombatUnit)FCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx();
                Y = ((TTacticalCombatUnit)FCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
                L = 0;
                I = FCombat.FCombatData.GetCount();
            }
            else
            {
                I += 1;
            }
        }
    }

    // AoWTC.pas:2572
    public void Surrender(sbyte Mode)
    {
        TCEndCombatTE TE;
        if (AoWHSMap.FTokenManager.PlayerReady(AoWTCUnit.AoWCombatMap.FCurrentPlayer) && FCurrentCombatPlayer == FSeatedCombatPlayer)
        {
            FUnitSelection.Unselect();
            TE = CreateTCEndCombatTE();
            if (TE != null)
            {
                AoWHSMap.FTokenManager.GetTokenControl(FCurrentPlayer).AddEvent(TE);
                TE.Release();
            }
        }
    }

    // AoWTC.pas:484

}

// Inherited combat layout. FHostPlayer
public partial class TTacticalCombat : TCombat
{
    // AoWTC.pas:5081
    public new const int ClassID = 2228496;

    public TTacticalCombat(TECustomNode owner) : base(owner)
    { }

    public sbyte FHostPlayer;
    // AoWTC.pas:5086
    public override TCombatUnit CreateCombatUnit() => new TTacticalCombatUnit(null);

    // AoWTC.pas:5157
    public override TCombatPlayer CreateCombatPlayer() => new TTacticalCombatPlayer(null);

    // AoWTC.pas:5097
    public override void Initialize()
    {
        base.Initialize();
        FCombatData.FTurnOrder.Clear();
        FCombatData.FTurnOrder.Add((byte)FCombatData.FAttackerPlayer);
        FHostPlayer = -1;
        for (int I = 0, last = AoWHSMap.FPlayers.GetCount() - 1; I <= last; I++)
        {
            if (FHostPlayer == -1)
            {
                if (AoWHSMap.FPlayers.GetPlayers(I).FPlayerType == TPlayerType.ptHuman)
                {
                    if (AoWHSMap.FPlayers.GetPlayers(I).FStatus != TPlayerStatus.psTerminated)
                    {
                        FHostPlayer = (sbyte)I;
                    }
                }
            }
        }

        for (int I = 0, last = FCombatData.FPlayers.GetCount() - 1; I <= last; I++)
        {
            if (FCombatData.FTurnOrder.IndexOf((byte)I) == -1)
            {
                if (FCombatData.FPlayers.GetPlayers((sbyte)I).FSideIndex == 0)
                {
                    FCombatData.FTurnOrder.Add((byte)I);
                }
            }

            var playerPlayer = FCombatData.FPlayers.GetPlayers((sbyte)I);
            if (AoWHSMap.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType != TPlayerType.ptHuman)
            {
                if (AoWHSMap.FPlayerCommunicator.PCC is TRemotePCC)
                {
                    var playerConnectionMode = AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FHostPlayer);
                    var player = FCombatData.FPlayers.GetPlayers((sbyte)I);
                    ((TRemotePCC)AoWHSMap.FPlayerCommunicator.PCC).SetConnectionMode(player.FPlayer, playerConnectionMode.ConnectionMode);
                    var playerSystemID = AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FHostPlayer);
                    var player2 = FCombatData.FPlayers.GetPlayers((sbyte)I);
                    ((TRemotePCC)AoWHSMap.FPlayerCommunicator.PCC).SetSystemID(player2.FPlayer, playerSystemID.SystemID);
                }
            }
        }

        for (int I = 0, last = AoWHSMap.FTokenManager.GetCount() - 1; I <= last; I++)
        {
            if (AoWHSMap.FTokenManager.GetTokenControl(I).Local() || AoWHSMap.FTokenManager.GetTokenControl(I).Global())
            {
                AoWHSMap.FTokenManager.GetTokenControl(I).ClearEvents();
            }
        }

        FCombatData.FTurnOrder.Add((byte)FCombatData.FDefenderPlayer);
        for (int I = 0, last = FCombatData.FPlayers.GetCount() - 1; I <= last; I++)
        {
            if (FCombatData.FTurnOrder.IndexOf((byte)I) == -1)
            {
                if (FCombatData.FPlayers.GetPlayers((sbyte)I).FSideIndex == 1)
                {
                    FCombatData.FTurnOrder.Add((byte)I);
                }
            }
        }

        SetLogCombatActions(1);
    }

    // AoWTC.pas:5091
    public override void Deactivate()
    {
        if (FCombatResult != TCombatResult.crCannotBreachWall)
        {
            UpdateStatus();
        }

        base.Deactivate();
    }

    // AoWTC.pas:5145
    public bool ValidateWallCombat()
    {
        int I;
        TCombatSide Side;
        Side = FSides[(int)TCombatSideIndex.csAttacker];
        bool Result = FCombatData.FParties.GetParty(0).FWall == 0;
        I = Side.GetCount();
        while (!Result && I > 0)
        {
            I -= 1;
            Result = (Side.GetObjects(I).WallCombatFeatures() & TSet<TWallCombatFeaturesElement>.Of(TWallCombatFeaturesElement.wcWallBreaching, TWallCombatFeaturesElement.wcWallPassing)) != TSet<TWallCombatFeaturesElement>.Of();
        }

        return Result;
    }

    public override TCombatResult Execute()
    {
        if (HeadlessHooks.Tactical || FrameFixtureInputs.Tactical)
        {
            FHostPlayer = 0;
            HeadlessHooks.TacticalExecutions++;
        }
        TCombatResult Result = default;
        TRndTacHsm Generator = default;
        int Seated = default;
        if (!ValidateWallCombat())
        {
            SetLogCombatActions(1);
            SetLogbookHeader(TranslateRStr(CannotBreachWallRStr));
            FCombatResult = TCombatResult.crCannotBreachWall;
            Result = FCombatResult;
        }
        else
        {
            if (FSides[(int)TCombatSideIndex.csDefender].GetCount() == FSides[(int)TCombatSideIndex.csDefender].FConquerCount)
            {
                SetLogCombatActions(1);
                SetLogbookHeader(TranslateRStr(CannotInflictDamageRStr));
                FCombatResult = TCombatResult.crCannotInflictDamage;
                Result = FCombatResult;
            }
            else
            {
                if (FSides[(int)TCombatSideIndex.csDefender].GetCount() == 0)
                {
                    Result = TCombatResult.crNoDefenders;
                }
                else
                {
                    AoWHSMap.FTokenManager.TokenExecuter.HoldTC = true;
                    Generator = new TRndTacHsm();
                    Generator.RandomMap(this, FCombatData.FX, FCombatData.FY, FCombatData.FL);
                    Generator.Free();
                    bool condition = AoWHSMap.FStartingGame || AoWHSMap.VisibleForPlayer(FCombatData.FX, FCombatData.FY, FCombatData.FL, AoWHSMap.FSeatedPlayer);
                    AoWTCUnit.AoWCombatMap.FPlaySounds = condition;
                    if (!AoWTCUnit.AoWCombatMap.FPlaySounds)
                    {
                        for (int I = 0, last = FCombatData.FPlayers.GetCount() - 1; I <= last; I++)
                        {
                            var playerPlayer = FCombatData.FPlayers.GetPlayers((sbyte)I);
                            if (AoWHSMap.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType == TPlayerType.ptHuman)
                            {
                                var player = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                if (AoWHSMap.FPlayers.GetPlayers(player.FPlayer).GetLocal())
                                {
                                    AoWTCUnit.AoWCombatMap.FPlaySounds = true;
                                }
                            }
                        }
                    }

                    AoWTCUnit.AoWCombatMap.FDeselectEvents.TriggerNotifyEvents(this);
                    Seated = -1;
                    for (int I = 0, last = FCombatData.FPlayers.GetCount() - 1; I <= last; I++)
                    {
                        if (Seated == -1)
                        {
                            var playerPlayer = FCombatData.FPlayers.GetPlayers((sbyte)I);
                            if (AoWHSMap.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType == TPlayerType.ptHuman)
                            {
                                var player = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                if (AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(player.FPlayer).ConnectionMode != TConnectionMode.cmRemote)
                                {
                                    var player2 = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                    if (AoWHSMap.FPlayers.GetPlayers(player2.FPlayer).FStatus != TPlayerStatus.psTerminated)
                                    {
                                        Seated = I;
                                    }
                                }
                            }
                        }
                    }

                    AoWTCUnit.AoWCombatMap.SetSeatedCombatPlayer((sbyte)Seated);
                    AoWTCUnit.AoWCombatMap.CenterViewTo(32, 29, 0);
                    AoWTCUnit.AoWCombatMap.FCombat.NewRound();
                    var turnOrder = FCombatData.FTurnOrder[0];
                    AoWTCUnit.AoWCombatMap.NewTurn((sbyte)turnOrder);
                    Result = TCombatResult.crBusy;
                    FOnTerminated = AoWTCUnit.AoWCombatMap.EndCombat;
                    SetLogCombatActions(1);
                }
            }
        }

        FrameFixtureInputs.TacticalInitialized?.Invoke();
        return Result;
    }
}

// FItems. FRandomTerrain. neighbor weights. terrain weights. terrain-tag weights. terrain
// samples. tag samples. resource lists. FHomePosition
public partial class TRndTacHsm : TObject
{
    public TItemList FItems;
    // calls GetItem
    public bool FRandomTerrain;
    public FixedArray_int_20 FNeighborWeights;
    public FixedArray_int_305 FTerrainWeights;
    public FixedArray_int_308 FTagWeights;
    public FixedArray_sbyte_7 FTerrain;
    public FixedArray_sbyte_7 FTags;
    public FixedArray_Aow_Rtl_TList_286 FResources;
    // original allocates one extra list
    public int FHomePosition;
    // AoWTC.pas:4149
    public override void Destroy()
    {
        for (int I = 0; I <= 285; I++)
        {
            FResources[I].Free();
        }

        base.Destroy();
    }

    // AoWTC.pas:4137
    public TRndTacHsm()
    {
        for (int I = 0; I <= 285; I++)
        {
            var list = new TList();
            FResources[I] = list;
        }

        for (int J = 0, last = AoWHSSet.FCombatResources.GetCount() - 1; J <= last; J++)
        {
            if (AoWHSSet.FCombatResources.MORs[J] is TCombatTerrainResource)
            {
                var mor = AoWHSSet.FCombatResources.MORs[J];
                var mor2 = AoWHSSet.FCombatResources.MORs[J];
                var mor3 = AoWHSSet.FCombatResources.MORs[J];
                FResources[(sbyte)((TCombatTerrainResource)mor2).TerrainType * 19 + ((TCombatTerrainResource)mor3).DecorationType].Add(mor);
            }
        }

        InitTables();
    }

    // AoWTC.pas:4156
    public TAoWCombatMap RandomMap(TTacticalCombat Combat, int X, int Y, int L)
    {
        byte[] TerrainBuffer = [];
        int I;
        int J = default;
        int K = default;
        int NX = default;
        int TerrainType = default;
        int Tag = default;
        byte[] ShapeBuffer = [];
        byte Shape = default;
        bool HasUnit = default;
        bool SiteCombat;
        bool RandomTerrain;
        string FileName;
        TStructure Structure;
        THexagonBridge Bridge;
        FixedArray_int_20 Weights = default;
        bool Blocked(int X, int Y, int First, int Last)
        {
            int Kind;
            int NX = default;
            int NY = default;
            bool Result = false;
            for (int I = First; I <= Last; I++)
            {
                CenterHNtoHX(X, Y, AoWTCUnit.clusterChk[I], ref NX, ref NY);
                Kind = TerrainBuffer[NY * 65 + NX];
                Result = Result || Kind == 1 || Kind == 2 || Kind == 3 || Kind == 5 || Kind == 6 || Kind == 7 || Kind == 11 || Kind == 14 || Kind == 15 || Kind == 16 || Kind == 17 || Kind == 18 || Kind == 252 || Kind == 253 || Kind == 254;
            }

            return Result;
        }

        FItems = null;
        if (Combat == null)
        {
            SiteCombat = false;
        }
        else
        {
            SiteCombat = Combat.FExplorationSite == 1;
        }

        if (Combat == null)
        {
            RandomTerrain = true;
        }
        else
        {
            RandomTerrain = !(SiteCombat || Combat.FExplorationSite == 2);
        }

        FRandomTerrain = RandomTerrain;
        AoWTCUnit.CombatXLoc = X;
        AoWTCUnit.CombatYLoc = Y;
        AoWTCUnit.CombatLLoc = L;
        RandSeed = Y * 128 + X + L * 16384 + AoWHSMap.FInitialRandomSeed;
        I = 0;
        for (; I <= 6; I += 1)
        {
            CenterHNtoHX(X, Y, I, ref J, ref K);
            var mapField = AoWHSMap.MapContainer.MapLevels[L].Field[J, K];
            FTerrain[I] = mapField.TerrainType;
            var combatTerrainTag = ((TAoWMapField)AoWHSMap.MapContainer.MapLevels[L].Field[J, K]).GetCombatTerrainTag();
            FTags[I] = (sbyte)combatTerrainTag;
        }

        if (FTerrain[0] == 0 || FTerrain[0] == 9 || FTerrain[0] == 10)
        {
            I = 1;
            for (; I <= 6; I += 1)
            {
                FTerrain[I] = FTerrain[0];
            }
        }
        else
        {
            I = 1;
            for (; I <= 6; I += 1)
            {
                if (FTerrain[I] == 0 || FTerrain[I] == 15 || FTerrain[I] == 10 || FTerrain[I] == 9)
                {
                    FTerrain[I] = FTerrain[0];
                }
            }
        }

        FileName = "TCMaps\\";
        Structure = (TStructure)AoWHSMap.MapContainer.MapLevels[L].Field[X, Y].FindNoneTransparentHS(typeof(TStructure));
        Bridge = null;
        if (Structure == null)
        {
            Bridge = (THexagonBridge)AoWHSMap.MapContainer.MapLevels[L].Field[X, Y].FindNoneTransparentHS(typeof(THexagonBridge));
        }

        if (Structure != null)
        {
            // The kind names the map (Structure.Views.cs) and reports what the map takes from the combat.
            var site = new CombatMapSite(SiteCombat, RandomTerrain, (Direction, Kind) =>
            {
                CenterHNtoHX(X, Y, Direction, ref J, ref K);
                return AoWHSMap.MapContainer.MapLevels[L].Field[J, K].FindNoneTransparentHS(Kind) != null;
            });
            FileName = FileName + Structure.CombatMapName(site);
            RandomTerrain = site.RandomTerrain;
            if (site.TakesCombatItems)
            {
                FItems = (TItemList)Combat.FItems;
            }

            for (int Direction = 1; Direction <= 6; Direction++)
            {
                if ((site.FixedTags & (1 << Direction)) != 0)
                {
                    FTags[Direction] = 127;
                }
            }

            FTags[0] = 127;
        }
        else if (Bridge != null)
        {
            RandomTerrain = false;
            FRandomTerrain = false;
            FileName = FileName + "Br";
            switch (FTerrain[0])
            {
                case 10:
                    FileName = FileName + "UW";
                    break;
                case 6:
                    FileName = FileName + "Ic";
                    break;
                case 9:
                    FileName = FileName + "La";
                    break;
                case 13:
                    FileName = FileName + "UI";
                    break;
                default:
                    FileName = FileName + "Wa";
                    break;
            }

            switch (Bridge.FDirection)
            {
                case 1:
                    FileName = FileName + "A000";
                    break;
                case 2:
                    FileName = FileName + "B000";
                    break;
                default:
                    FileName = FileName + "C000";
                    break;
            }

            FTags[0] = 127;
        }

        if (FTags[0] != 127)
        {
            FileName = FileName + "Blank000";
        }

        FileName = FileName + ".HSM";
        if (Combat == null)
        {
            AoWEngine.LoadHSMEx(FileBoundary.ReadName(AoWEngine.StartupDirectory + FileName), false, null, Aow.Formats.GameProfile.TacticalMap);
        }
        else
        {
            AoWHSMap.InitializeCombatMap(Combat, FileName);
        }

        AoWTCUnit.AoWCombatMap.SetCombat(Combat);
        AoWTCUnit.AoWCombatMap.SetExplorationEnabled(SiteCombat);
        AoWTCUnit.AoWCombatMap.SetFogEnabled(false);
        if (Combat != null)
        {
            PlaceCombatUnits();
        }

        if (RandomTerrain)
        {
            AoWTCUnit.AoWCombatMap.LockMapChangedEvent();
            ShapeBuffer = new byte[3770];
            TerrainBuffer = new byte[3770];
            for (int Row = 6; Row <= 52; Row++)
            {
                for (int Col = 6; Col <= 58; Col++)
                {
                    Shape = AoWTCUnit.MapShape[Row * 65 + Col];
                    switch (Shape)
                    {
                        case 14:
                            if (Random(100) < 50)
                            {
                                Shape = 1;
                            }
                            else
                            {
                                Shape = 2;
                            }

                            break;
                        case 15:
                            if (Random(100) < 50)
                            {
                                Shape = 1;
                            }
                            else
                            {
                                Shape = 3;
                            }

                            break;
                        case 16:
                            if (Random(100) < 50)
                            {
                                Shape = 1;
                            }
                            else
                            {
                                Shape = 4;
                            }

                            break;
                        case 17:
                            if (Random(100) < 50)
                            {
                                Shape = 1;
                            }
                            else
                            {
                                Shape = 5;
                            }

                            break;
                        case 18:
                            if (Random(100) < 50)
                            {
                                Shape = 1;
                            }
                            else
                            {
                                Shape = 6;
                            }

                            break;
                        case 19:
                            if (Random(100) < 50)
                            {
                                Shape = 1;
                            }
                            else
                            {
                                Shape = 7;
                            }

                            break;
                        case 11:
                            if (Random(100) < 50)
                            {
                                Shape = 2;
                            }
                            else
                            {
                                Shape = 3;
                            }

                            break;
                        case 9:
                            if (Random(100) < 50)
                            {
                                Shape = 3;
                            }
                            else
                            {
                                Shape = 4;
                            }

                            break;
                        case 13:
                            if (Random(100) < 50)
                            {
                                Shape = 5;
                            }
                            else
                            {
                                Shape = 4;
                            }

                            break;
                        case 12:
                            if (Random(100) < 50)
                            {
                                Shape = 5;
                            }
                            else
                            {
                                Shape = 6;
                            }

                            break;
                        case 8:
                            if (Random(100) < 50)
                            {
                                Shape = 7;
                            }
                            else
                            {
                                Shape = 6;
                            }

                            break;
                        case 10:
                            if (Random(100) < 50)
                            {
                                Shape = 2;
                            }
                            else
                            {
                                Shape = 7;
                            }

                            break;
                    }

                    if (AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[Col, Row].GetDecorationType() == 9)
                    {
                        switch (Shape)
                        {
                            case 1:
                                J = 0;
                                break;
                            case 2:
                                J = 1;
                                break;
                            case 3:
                                J = 2;
                                break;
                            case 4:
                                J = 3;
                                break;
                            case 5:
                                J = 4;
                                break;
                            case 6:
                                J = 5;
                                break;
                            case 7:
                                J = 6;
                                break;
                            default:
                                J = -1;
                                break;
                        }

                        if (J > -1)
                        {
                            CenterHNtoHX(X, Y, J, ref K, ref NX);
                            if (AoWHSMap.MapContainer.MapLevels[L].Field[K, NX].FindOwnedChild(131946) == null)
                            {
                                AoWTCUnit.AoWCombatMap.ClearTerrain(Col, Row, 0, 0, -1);
                            }
                        }
                    }

                    ShapeBuffer[Row * 65 + Col] = Shape;
                }
            }

            if (AoWHSMap.MapContainer.MapLevels[L].Field[X, Y].OverlayType == 4)
            {
                I = 0;
                for (; I <= 6; I += 1)
                {
                    CenterHNtoHX(X, Y, I, ref J, ref K);
                    var item = AoWHSMap.MapContainer.MapLevels[L].Field[J, K];
                    Weights[I] = item.OverlayType;
                }

                for (int Row = 6; Row <= 52; Row++)
                {
                    for (int Col = 6; Col <= 58; Col++)
                    {
                        Shape = AoWTCUnit.MapRoads[Row * 65 + Col];
                        switch (Shape)
                        {
                            case 21:
                                if (Random(100) < 50)
                                {
                                    Shape = 0;
                                }
                                else
                                {
                                    Shape = 2;
                                }

                                break;
                            case 22:
                                if (Random(100) < 50)
                                {
                                    Shape = 0;
                                }
                                else
                                {
                                    Shape = 3;
                                }

                                break;
                            case 23:
                                if (Random(100) < 50)
                                {
                                    Shape = 0;
                                }
                                else
                                {
                                    Shape = 4;
                                }

                                break;
                            case 24:
                                if (Random(100) < 50)
                                {
                                    Shape = 0;
                                }
                                else
                                {
                                    Shape = 5;
                                }

                                break;
                            case 25:
                                if (Random(100) < 50)
                                {
                                    Shape = 0;
                                }
                                else
                                {
                                    Shape = 6;
                                }

                                break;
                            case 26:
                                if (Random(100) < 50)
                                {
                                    Shape = 0;
                                }
                                else
                                {
                                    Shape = 7;
                                }

                                break;
                        }

                        if (TSet<byte>.WithBounds(0, 255).WithRange(1, 7).Contains(Shape))
                        {
                            if (FTags[Shape - 1] != 127 && Weights[Shape - 1] == 4)
                            {
                                AoWHSSet.FCombatRoads.Place(AoWTCUnit.AoWCombatMap.MapContainer, (sbyte)Col, (sbyte)Row, 0, 1);
                                TerrainBuffer[Row * 65 + Col] = 251;
                            }
                        }
                    }
                }
            }

            for (int Row = 6; Row <= 52; Row++)
            {
                for (int Col = 3; Col <= 29; Col++)
                {
                    I = ShapeBuffer[Row * 65 + Col * 2];
                    if (I > 0)
                    {
                        if (FTags[I - 1] == 127 || TerrainBuffer[Row * 65 + Col * 2] == 251)
                        {
                            I = 0;
                        }
                    }

                    if (I > 0)
                    {
                        TerrainType = FTerrain[I - 1];
                        Tag = FTags[I - 1];
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            Weights[I] = FTerrainWeights[TerrainType * 19 + I];
                            if (Tag < 14)
                            {
                                Weights[I] += FTagWeights[Tag * 19 + I];
                            }
                        }

                        I = 0;
                        for (; I <= 2; I += 1)
                        {
                            CenterHNtoHX(Col * 2, Row, AoWTCUnit.clusterChk[I], ref J, ref K);
                            Tag = TerrainBuffer[K * 65 + J];
                            if (Tag <= 19)
                            {
                                Weights[Tag] += FNeighborWeights[Tag];
                            }

                            switch (Tag)
                            {
                                case >= 1 and <= 3:
                                    Weights[5] += 10;
                                    Weights[10] += 10;
                                    Weights[4] += 10;
                                    break;
                                case 14:
                                case 16:
                                    Weights[11] += FNeighborWeights[11];
                                    break;
                            }
                        }

                        HasUnit = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[Col * 2, Row].FindChild(2228484) != null;
                        if (HasUnit || Blocked(Col * 2, Row, 0, 2))
                        {
                            if ((HasUnit || Blocked(Col * 2, Row, 4, 8)) && (HasUnit || Blocked(Col * 2, Row, 9, 17)))
                            {
                                Weights[1] = 0;
                                Weights[2] = 0;
                                Weights[3] = 0;
                                Weights[5] = 0;
                                Weights[6] = 0;
                                Weights[7] = 0;
                                Weights[11] = 0;
                                Weights[14] = 0;
                                Weights[15] = 0;
                                Weights[16] = 0;
                                Weights[17] = 0;
                                Weights[18] = 0;
                            }
                        }
                        else
                        {
                            if ((HasUnit || Blocked(Col * 2, Row, 4, 8)) && (HasUnit || Blocked(Col * 2, Row, 9, 17)))
                            {
                                Weights[14] = 0;
                                Weights[15] = 0;
                                Weights[16] = 0;
                            }
                        }

                        J = 0;
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            J += Weights[I];
                        }

                        Tag = Random(J) - Weights[0];
                        J = 0;
                        while (Tag > 0 && J < 18)
                        {
                            J += 1;
                            Tag -= Weights[J];
                        }

                        if (TerrainType == 8)
                        {
                            J = 17;
                        }

                        switch (J)
                        {
                            case 14:
                                CenterHNtoHX(Col * 2, Row, 6, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 252;
                                break;
                            case 15:
                                CenterHNtoHX(Col * 2, Row, 6, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 253;
                                break;
                            case 16:
                                CenterHNtoHX(Col * 2, Row, 1, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 254;
                                CenterHNtoHX(Col * 2, Row, 2, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 254;
                                CenterHNtoHX(Col * 2, Row, 6, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 254;
                                break;
                        }

                        TerrainBuffer[Row * 65 + Col * 2] = (byte)J;
                    }
                }

                for (int Col = 3; Col <= 29; Col++)
                {
                    I = ShapeBuffer[Row * 65 + Col * 2 + 1];
                    if (I > 0)
                    {
                        if (FTags[I - 1] == 127 || TerrainBuffer[Row * 65 + Col * 2 + 1] == 251)
                        {
                            I = 0;
                        }
                    }

                    if (I > 0)
                    {
                        TerrainType = FTerrain[I - 1];
                        Tag = FTags[I - 1];
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            Weights[I] = FTerrainWeights[TerrainType * 19 + I];
                            if (Tag < 14)
                            {
                                Weights[I] += FTagWeights[Tag * 19 + I];
                            }
                        }

                        I = 0;
                        for (; I <= 2; I += 1)
                        {
                            CenterHNtoHX(Col * 2 + 1, Row, AoWTCUnit.clusterChk[I], ref J, ref K);
                            Tag = TerrainBuffer[K * 65 + J];
                            if (Tag <= 19)
                            {
                                Weights[Tag] += FNeighborWeights[Tag];
                            }

                            switch (Tag)
                            {
                                case >= 1 and <= 3:
                                    Weights[5] += 10;
                                    Weights[10] += 10;
                                    Weights[4] += 10;
                                    break;
                                case 14:
                                case 16:
                                    Weights[11] += FNeighborWeights[11];
                                    break;
                            }
                        }

                        HasUnit = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[Col * 2 + 1, Row].FindChild(2228484) != null;
                        if (HasUnit || Blocked(Col * 2 + 1, Row, 0, 2))
                        {
                            if ((HasUnit || Blocked(Col * 2 + 1, Row, 4, 8)) && (HasUnit || Blocked(Col * 2 + 1, Row, 9, 17)))
                            {
                                Weights[1] = 0;
                                Weights[2] = 0;
                                Weights[3] = 0;
                                Weights[5] = 0;
                                Weights[6] = 0;
                                Weights[7] = 0;
                                Weights[11] = 0;
                                Weights[14] = 0;
                                Weights[15] = 0;
                                Weights[16] = 0;
                                Weights[17] = 0;
                                Weights[18] = 0;
                            }
                        }
                        else
                        {
                            if ((HasUnit || Blocked(Col * 2 + 1, Row, 4, 8)) && (HasUnit || Blocked(Col * 2 + 1, Row, 9, 17)))
                            {
                                Weights[14] = 0;
                                Weights[15] = 0;
                                Weights[16] = 0;
                            }
                        }

                        J = 0;
                        I = 0;
                        for (; I <= 18; I += 1)
                        {
                            J += Weights[I];
                        }

                        Tag = Random(J) - Weights[0];
                        J = 0;
                        while (Tag > 0 && J < 18)
                        {
                            J += 1;
                            Tag -= Weights[J];
                        }

                        if (TerrainType == 8)
                        {
                            J = 17;
                        }

                        switch (J)
                        {
                            case 14:
                                CenterHNtoHX(Col * 2 + 1, Row, 6, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 252;
                                break;
                            case 15:
                                CenterHNtoHX(Col * 2 + 1, Row, 6, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 253;
                                break;
                            case 16:
                                CenterHNtoHX(Col * 2 + 1, Row, 1, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 254;
                                CenterHNtoHX(Col * 2 + 1, Row, 2, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 254;
                                CenterHNtoHX(Col * 2 + 1, Row, 6, ref K, ref NX);
                                TerrainBuffer[NX * 65 + K] = 254;
                                break;
                        }

                        TerrainBuffer[Row * 65 + Col * 2 + 1] = (byte)J;
                    }
                }
            }

            for (int Row = 6; Row <= 52; Row++)
            {
                for (int Col = 6; Col <= 58; Col++)
                {
                    I = ShapeBuffer[Row * 65 + Col];
                    if (I > 0)
                    {
                        J = TerrainBuffer[Row * 65 + Col];
                        if (J == 17)
                        {
                            AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[Col, Row].TerrainType = 7;
                        }
                        else
                        {
                            AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[Col, Row].TerrainType = FTerrain[I - 1];
                            if (J > 0 && J < 19)
                            {
                                if (FResources[FTerrain[I - 1] * 19 + J].Count > 0)
                                {
                                    var random = Random(FResources[FTerrain[I - 1] * 19 + J].Count);
                                    ((TMapObjectResource)FResources[FTerrain[I - 1] * 19 + J][random]).PlaceResourceUser(AoWTCUnit.AoWCombatMap, (sbyte)Col, (sbyte)Row, 0);
                                }
                            }
                        }
                    }
                }
            }

            for (int Row = 6; Row <= 52; Row++)
            {
                for (int Col = 6; Col <= 58; Col++)
                {
                    AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[Col, Row].TerrainChanged();
                }
            }

            TerrainBuffer = default;
            AoWTCUnit.AoWCombatMap.UnlockMapChangedEvent();
        }

        return AoWTCUnit.AoWCombatMap;
    }

    // AoWTC.pas:4519
    public void PlaceCombatUnits()
    {
        int Position;
        int I;
        int J = default;
        int Temp = default;
        TList Units;
        TTacticalCombatUnit U;
        TTacticalCombatUnit Other;
        int Direction;
        FixedArray_bool_8 Used = default;
        TCItemHS ItemHS;
        Direction = 32;
        FHomePosition = -1;
        Units = new TList();
        Position = 0;
        for (; Position <= 7; Position += 1)
        {
            Units.Clear();
            I = 0;
            int last = AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
            for (; I <= last; I += 1)
            {
                if (AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I) is TCombatUnit)
                {
                    if (AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I).GetEnabled())
                    {
                        if ((int)(((uint)AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I).FPosition) >> 4) == Position)
                        {
                            Units.Add(AoWTCUnit.AoWCombatMap.FCombat.FCombatData.GetObjects(I));
                            U = (TTacticalCombatUnit)Units[Units.Count - 1];
                            U.FPositionIndex = (byte)(U.FPosition & 15);
                            U.FPositionGroup = (byte)(int)(((uint)U.FPosition) >> 4);
                            if (U.FPositionGroup == 7)
                            {
                                U.FPositionGroup = 1;
                            }

                            if (U.FPositionGroup == 0)
                            {
                                FHomePosition = 0;
                            }

                            if (FHomePosition == -1)
                            {
                                if (U.GetSide() == TCombatSideIndex.csDefender)
                                {
                                    if (AoWHSMap.MapContainer.MapLevels[AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FL].Field[AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FX, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FY].FindNoneTransparentHS(typeof(TCity)) != null)
                                    {
                                    }
                                }
                            }

                            if (FHomePosition > 0 && U.FPositionGroup == FHomePosition)
                            {
                                U.FPositionGroup = 0;
                            }

                            var side = U.GetSide();
                            AoWTCUnit.AoWCombatMap.FPositionStatus[U.FPositionGroup] = (byte)side;
                        }
                    }
                }
            }

            I = 0;
            int limit = Units.Count - 1;
            for (; I <= limit; I += 1)
            {
                U = (TTacticalCombatUnit)Units[I];
                J = I + 1;
                int last2 = Units.Count - 1;
                for (; J <= last2; J += 1)
                {
                    Other = (TTacticalCombatUnit)Units[J];
                    if (U.GetPosValue() < Other.GetPosValue())
                    {
                        Temp = U.FPositionIndex;
                        U.FPositionIndex = Other.FPositionIndex;
                        Other.FPositionIndex = (byte)Temp;
                    }
                }
            }
        }

        if (AoWTCUnit.AoWCombatMap.FPositionStatus[0] != 1)
        {
            if (AoWHSMap.MapContainer.MapLevels[AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FL].Field[AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FX, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FY].FindNoneTransparentHS(typeof(TCity)) != null && AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FWallType != 0)
            {
                Position = 1;
                while (Position < 7)
                {
                    if (AoWTCUnit.AoWCombatMap.FPositionStatus[Position] == 1)
                    {
                        CenterHNtoHX(AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FX, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FY, Position, ref J, ref Temp);
                        if (AoWHSMap.MapContainer.MapLevels[AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FL].Field[J, Temp].FindNoneTransparentHS(typeof(TCity)) != null)
                        {
                            FHomePosition = Position;
                            AoWTCUnit.AoWCombatMap.FPositionStatus[Position] = (int)TCombatSideIndex.csNeutral;
                            AoWTCUnit.AoWCombatMap.FPositionStatus[0] = 1;
                            Position = 7;
                        }
                    }

                    Position += 1;
                }
            }
        }

        Position = 0;
        int last3 = AoWTCUnit.AoWCombatMap.FCombat.FSides[(int)TCombatSideIndex.csAttacker].GetCount() - 1;
        for (; Position <= last3; Position += 1)
        {
            if (AoWTCUnit.AoWCombatMap.FCombat.FSides[(int)TCombatSideIndex.csAttacker].GetObjects(Position).GetEnabled())
            {
                SetupUnit(TCombatSideIndex.csAttacker, Position, ref Direction);
            }
        }

        Position = 0;
        int last4 = AoWTCUnit.AoWCombatMap.FCombat.FSides[(int)TCombatSideIndex.csDefender].GetCount() - 1;
        for (; Position <= last4; Position += 1)
        {
            if (AoWTCUnit.AoWCombatMap.FCombat.FSides[(int)TCombatSideIndex.csDefender].GetObjects(Position) is TCombatUnit)
            {
                if (AoWTCUnit.AoWCombatMap.FCombat.FSides[(int)TCombatSideIndex.csDefender].GetObjects(Position).GetEnabled())
                {
                    SetupUnit(TCombatSideIndex.csDefender, Position, ref Direction);
                }
            }
        }

        Units.Free();
        if (FItems != null)
        {
            Position = 0;
            for (; Position <= 7; Position += 1)
            {
                Used[Position] = false;
            }

            Position = 0;
            while (Position < FItems.GetCount())
            {
                I = Random(AoWTCUnit.AoWCombatMap.FUnitPositions.FPositions.Count - 16);
                J = I - 1;
                if (J < 0)
                {
                    J = AoWTCUnit.AoWCombatMap.FUnitPositions.FPositions.Count - 17;
                }

                // { Original indexes its eight-slot occupancy array with the position-list index. }
                while (Used[I] && I != J)
                {
                    I += 1;
                    if (I > AoWTCUnit.AoWCombatMap.FUnitPositions.FPositions.Count - 17)
                    {
                        I = 0;
                    }
                }

                Used[I] = true;
                J = AoWTCUnit.AoWCombatMap.FUnitPositions.GetUnitPosition(2, I).X;
                Temp = AoWTCUnit.AoWCombatMap.FUnitPositions.GetUnitPosition(2, I).Y;
                ItemHS = new TCItemHS(null);
                if (AoWTCUnit.AoWCombatMap.PlaceHX(ItemHS, J, Temp, 0))
                {
                    FItems.GetItem(Position).SetOwner(ItemHS.FItems);
                }
                else
                {
                    Position += 1;
                }
            }
        }
    }

    // AoWTC.pas:4610
    public void SetupUnit(TCombatSideIndex Side, int Index, ref int Direction)
    {
        TTacticalCombatUnitHS UnitHS;
        TTacticalCombatUnitHS TargetHS;
        int X;
        int Y;
        int Position;
        int Slot;
        int HN;
        int NX = default;
        int NY = default;
        UnitHS = new TTacticalCombatUnitHS(null);
        UnitHS.Initialize((TTacticalCombatUnit)AoWTCUnit.AoWCombatMap.FCombat.FSides[(int)Side].GetObjects(Index));
        if (UnitHS.FUnit.FPositionGroup == FHomePosition)
        {
            UnitHS.FUnit.FPositionGroup = 0;
        }

        Position = UnitHS.FUnit.FPositionGroup;
        Slot = UnitHS.FUnit.FPositionIndex;
        X = AoWTCUnit.AoWCombatMap.FUnitPositions.GetUnitPosition(Position, Slot).X;
        Y = AoWTCUnit.AoWCombatMap.FUnitPositions.GetUnitPosition(Position, Slot).Y;
        if (Side == TCombatSideIndex.csDefender && FRandomTerrain)
        {
            switch (Position)
            {
                case 0:
                    if (Direction > 0)
                    {
                        X = AoWTCUnit.DefPosX[(Direction - 1) * 8 + Slot];
                        Y = AoWTCUnit.DefPosY[(Direction - 1) * 8 + Slot];
                        UnitHS.SetDirection((byte)Direction);
                    }
                    else
                    {
                        switch (Slot)
                        {
                            case 0:
                                UnitHS.SetDirection(1);
                                break;
                            case 1:
                                UnitHS.SetDirection(4);
                                break;
                            case 2:
                            case 6:
                                UnitHS.SetDirection(5);
                                break;
                            case 3:
                            case 7:
                                UnitHS.SetDirection(2);
                                break;
                            case 4:
                                UnitHS.SetDirection(3);
                                break;
                            case 5:
                                UnitHS.SetDirection(6);
                                break;
                        }
                    }

                    break;
                case 1:
                    Y += 2;
                    break;
                case 2:
                    X -= 2;
                    Y += 1;
                    break;
                case 3:
                    X -= 2;
                    Y -= 1;
                    break;
                case 4:
                    Y -= 2;
                    break;
                case 5:
                    X += 2;
                    Y -= 1;
                    break;
                case 6:
                    X += 2;
                    Y += 1;
                    break;
            }
        }
        else
        {
            if (Direction != Position)
            {
                if (Direction == 32)
                {
                    Direction = Position;
                }
                else
                {
                    Direction = 0;
                }
            }
        }

        if (!FRandomTerrain)
        {
            UnitHS.SetDirection((byte)AoWTCUnit.AoWCombatMap.FUnitPositions.GetUnitPosition(Position, Slot).Flag);
        }
        else
        {
            switch (Position)
            {
                case 1:
                    UnitHS.SetDirection(4);
                    break;
                case 2:
                    UnitHS.SetDirection(5);
                    break;
                case 3:
                    UnitHS.SetDirection(6);
                    break;
                case 4:
                    UnitHS.SetDirection(1);
                    break;
                case 5:
                    UnitHS.SetDirection(2);
                    break;
                case 6:
                    UnitHS.SetDirection(3);
                    break;
            }
        }

        HN = 0;
        do
        {
            CenterHNtoHX(X, Y, HN, ref NX, ref NY);
            Slot = AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[NX, NY].OverlayType;
            TargetHS = (TTacticalCombatUnitHS)AoWTCUnit.AoWCombatMap.MapContainer.MapLevels[0].Field[NX, NY].FindOwnedChild(2228484);
            HN += 1;
        }
        while (!(Slot != 8 && TargetHS == null));
        AoWTCUnit.AoWCombatMap.PlaceHX(UnitHS, NX, NY, 0);
        if (dHXtoHN(AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FX, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FY, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FAttackerX, AoWTCUnit.AoWCombatMap.FCombat.FCombatData.FAttackerY) == Position && AoWTCUnit.AoWCombatMap.FPlayers.GetPlayers(UnitHS.FUnit.FCombatPlayer).FLastX == 0)
        {
            var xhx = UnitHS.GetXhx();
            AoWTCUnit.AoWCombatMap.FPlayers.GetPlayers(UnitHS.FUnit.FCombatPlayer).FLastX = xhx;
            var yhx = UnitHS.GetYhx();
            AoWTCUnit.AoWCombatMap.FPlayers.GetPlayers(UnitHS.FUnit.FCombatPlayer).FLastY = yhx;
        }
    }

    // AoWTC.pas:4667
    public void InitTables()
    {
        FNeighborWeights[1] = 500;
        FNeighborWeights[2] = 500;
        FNeighborWeights[4] = 1500;
        FNeighborWeights[5] = 200;
        FNeighborWeights[10] = 1500;
        FNeighborWeights[11] = 500;
        FNeighborWeights[17] = 2000;
        FNeighborWeights[18] = 1500;
        FTerrainWeights[19] = 2500;
        FTerrainWeights[20] = 0;
        FTerrainWeights[21] = 35;
        FTerrainWeights[22] = 0;
        FTerrainWeights[23] = 6;
        FTerrainWeights[24] = 2;
        FTerrainWeights[25] = 1;
        FTerrainWeights[26] = 2;
        FTerrainWeights[27] = 5;
        FTerrainWeights[29] = 90;
        FTerrainWeights[30] = 60;
        FTerrainWeights[31] = 2;
        FTerrainWeights[32] = 30;
        FTerrainWeights[33] = 10;
        FTerrainWeights[34] = 0;
        FTerrainWeights[35] = 3;
        FTerrainWeights[38] = 2500;
        FTerrainWeights[39] = 5;
        FTerrainWeights[40] = 0;
        FTerrainWeights[41] = 0;
        FTerrainWeights[42] = 6;
        FTerrainWeights[43] = 0;
        FTerrainWeights[44] = 5;
        FTerrainWeights[45] = 5;
        FTerrainWeights[46] = 2;
        FTerrainWeights[48] = 35;
        FTerrainWeights[49] = 20;
        FTerrainWeights[50] = 5;
        FTerrainWeights[51] = 30;
        FTerrainWeights[52] = 5;
        FTerrainWeights[53] = 2;
        FTerrainWeights[54] = 1;
        FTerrainWeights[57] = 2500;
        FTerrainWeights[58] = 8;
        FTerrainWeights[59] = 0;
        FTerrainWeights[60] = 0;
        FTerrainWeights[61] = 6;
        FTerrainWeights[62] = 0;
        FTerrainWeights[63] = 2;
        FTerrainWeights[64] = 2;
        FTerrainWeights[65] = 1;
        FTerrainWeights[67] = 15;
        FTerrainWeights[68] = 20;
        FTerrainWeights[69] = 2;
        FTerrainWeights[70] = 30;
        FTerrainWeights[71] = 5;
        FTerrainWeights[72] = 1;
        FTerrainWeights[73] = 1;
        FTerrainWeights[76] = 2500;
        FTerrainWeights[77] = 8;
        FTerrainWeights[78] = 0;
        FTerrainWeights[79] = 0;
        FTerrainWeights[80] = 6;
        FTerrainWeights[81] = 0;
        FTerrainWeights[82] = 2;
        FTerrainWeights[83] = 2;
        FTerrainWeights[84] = 1;
        FTerrainWeights[86] = 15;
        FTerrainWeights[87] = 20;
        FTerrainWeights[88] = 2;
        FTerrainWeights[89] = 20;
        FTerrainWeights[90] = 5;
        FTerrainWeights[91] = 1;
        FTerrainWeights[92] = 1;
        FTerrainWeights[95] = 2500;
        FTerrainWeights[96] = 5;
        FTerrainWeights[97] = 0;
        FTerrainWeights[98] = 1;
        FTerrainWeights[99] = 3;
        FTerrainWeights[100] = 0;
        FTerrainWeights[101] = 5;
        FTerrainWeights[102] = 5;
        FTerrainWeights[103] = 4;
        FTerrainWeights[105] = 10;
        FTerrainWeights[106] = 45;
        FTerrainWeights[107] = 5;
        FTerrainWeights[108] = 12;
        FTerrainWeights[109] = 10;
        FTerrainWeights[110] = 4;
        FTerrainWeights[111] = 3;
        FTerrainWeights[114] = 2500;
        FTerrainWeights[115] = 0;
        FTerrainWeights[116] = 0;
        FTerrainWeights[117] = 0;
        FTerrainWeights[118] = 0;
        FTerrainWeights[119] = 0;
        FTerrainWeights[120] = 0;
        FTerrainWeights[121] = 0;
        FTerrainWeights[122] = 0;
        FTerrainWeights[124] = 0;
        FTerrainWeights[125] = 0;
        FTerrainWeights[126] = 0;
        FTerrainWeights[127] = 0;
        FTerrainWeights[128] = 0;
        FTerrainWeights[129] = 0;
        FTerrainWeights[130] = 0;
        FTerrainWeights[228] = 2500;
        FTerrainWeights[229] = 0;
        FTerrainWeights[230] = 0;
        FTerrainWeights[231] = 0;
        FTerrainWeights[232] = 0;
        FTerrainWeights[233] = 10;
        FTerrainWeights[234] = 0;
        FTerrainWeights[235] = 0;
        FTerrainWeights[236] = 0;
        FTerrainWeights[238] = 30;
        FTerrainWeights[239] = 70;
        FTerrainWeights[240] = 4;
        FTerrainWeights[241] = 0;
        FTerrainWeights[242] = 50;
        FTerrainWeights[243] = 4;
        FTerrainWeights[244] = 10;
        FTerrainWeights[245] = 80;
        FTagWeights[20] = 800;
        FTagWeights[21] = 100;
        FTagWeights[29] = 300;
        FTagWeights[25] = 20;
        FTagWeights[26] = 40;
        FTagWeights[27] = 10;
        FTagWeights[40] = 900;
        FTagWeights[48] = 400;
        FTagWeights[44] = 20;
        FTagWeights[45] = 40;
        FTagWeights[46] = 10;
        FTagWeights[60] = 250;
        FTagWeights[61] = 200;
        FTagWeights[86] = 300;
        FTagWeights[80] = 500;
        FTagWeights[100] = 500;
        FTagWeights[105] = 200;
        FTagWeights[99] = 100;
        FTagWeights[120] = 250;
        FTagWeights[121] = 250;
        FTagWeights[122] = 100;
        FTagWeights[144] = 600;
        FTagWeights[147] = 250;
        FTagWeights[149] = 150;
        FTagWeights[164] = 250;
        FTagWeights[167] = 100;
        FTagWeights[163] = 200;
        FTagWeights[162] = 200;
        FTagWeights[181] = 500;
        FTagWeights[191] = 450;
        FTagWeights[200] = 150;
        FTagWeights[196] = 40;
        FTagWeights[197] = 20;
        FTagWeights[198] = 10;
        FTagWeights[211] = 450;
        FTagWeights[219] = 150;
        FTagWeights[215] = 20;
        FTagWeights[216] = 20;
        FTagWeights[217] = 40;
        FTagWeights[241] = 500;
        FTagWeights[239] = 200;
        FTagWeights[242] = 50;
        FTagWeights[244] = 20;
        FTagWeights[258] = 600;
        FTagWeights[261] = 350;
        FTagWeights[263] = 300;
        FTagWeights[181] = 600;
    }

}

public enum TFireStart : byte
{
    fsNew = 0,
    fsSpread = 1
}

public enum TTCDeathMethod : byte
{
    dmNormal = 0,
    dmExplode = 1
}

public enum TTCAction : byte
{
    caMove = 0,
    caMeleeHit = 1,
    caMeleeMiss = 2,
    caAbility = 3,
    caHit = 4,
    caKilled = 5,
    caStepUp = 6,
    caStepBack = 7,
    caBusy = 8,
    caRoundAttack = 9,
    caRoundAttackHit = 10,
    caFreeStruck = 11,
    caBreathAttack = 12
}

public enum TfxType : byte
{
    fxNone = 0,
    fxArrow = 1,
    fxRock = 2,
    fxComet = 3,
    fxTouch = 4,
    fxBreath = 5,
    fxCometBall = 6,
    fxEffect = 7,
    fxFBExplosion = 8,
    fxScreenNESW = 9,
    fxScreenNS = 10,
    fxRay = 11,
    fxChain = 12,
    fxSwarm = 13,
    fxScreenSwarm = 14,
    fxSFOverlay = 15,
    fxDamLightning = 16,
    fxDamFire = 17,
    fxDamPoison = 18,
    fxDamMagic = 19,
    fxDamHoly = 20,
    fxDamDeath = 21,
    fxDamCold = 22,
    fxSacredWrath = 23,
    fxSWComet = 24,
    fxDispel = 25,
    fxWallSmoke = 26,
    fxBlood = 27,
    fxAltArrow = 28,
    fxTremors = 29,
    fxGeyser = 30,
    fxGeyserWater = 31,
    fxHailImpact = 32,
    fxCloud = 33,
    fxCloudPuff = 34,
    fxHitGround = 35,
    fxHitWater = 36,
    fxHighPrayer = 37,
    fxPrayerGlow = 38,
    fxResurrect = 39,
    fxFrostBeam = 40,
    fxFrost = 41,
    fxMindDecay = 42,
    fxDecayLeg = 43,
    fxShockWave = 44,
    fxDecayComet = 45,
    fxSacFlame = 46,
    fxWinds = 47,
    fxSacFire = 48,
    fxBreathDam = 49
}

public enum TRndHexMask : byte
{
    rndNone = 0,
    rndCenter = 1,
    rndNorth = 2,
    rndNE = 3,
    rndSE = 4,
    rndSouth = 5,
    rndSW = 6,
    rndNW = 7,
    rndNWSW = 8,
    rndNESE = 9,
    rndNNW = 10,
    rndNNE = 11,
    rndSSW = 12,
    rndSSE = 13,
    rndCN = 14,
    rndCNE = 15,
    rndCSE = 16,
    rndCS = 17,
    rndCSW = 18,
    rndCNW = 19,
    rndRC = 20,
    rndRN = 21,
    rndRNE = 22,
    rndRSE = 23,
    rndRS = 24,
    rndRSW = 25,
    rndRNW = 26
}

public enum TTCAbil : byte
{
    tcWallCrushing = 0,
    tcFire = 1,
    tcabMax = 2
}

public enum TaiStatus : byte
{
    aiIdle = 0,
    aiBusy = 1,
    aiDone = 2
}

public enum TaiMoveType : byte
{
    aiMove = 0,
    aiTouch = 1,
    aiMelee = 2,
    aiRanged = 3,
    aiFake = 4,
    aiMoveRanged = 5,
    aiWallTouch = 6,
    aiWallRanged = 7
}

public static partial class AoWTCUnit
{
    public static byte TCombatUnitSelectionControl_CalculateMovePath__ClimbFlag = 1;
    // AoWTC.pas:6436
    public static void MakeAbilPath(TCombatSideIndex Side, short Ability, short Spell, short X1, short Y1, short X2, short Y2, ref TXYZList Path, ref TList Obstacles)
    {
        int X = default;
        int Y = default;
        switch (Ability)
        {
            case 55:
                MakeRangedPath(Side, 25, 0, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                break;
            case 22:
            case 24:
            case 25:
                MakeRangedPath(Side, 50, 100, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                break;
            case 44:
            case 45:
            case 57:
            case 58:
            case 123:
                MakeRangedPath(Side, 50, 20, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                break;
            case 30:
            case 31:
            case >= 86 and <= 90:
                var xyz = new TXYZ();
                Path.Add(xyz);
                HXtoHP(X2, Y2, ref X, ref Y);
                Path[0].X = X + 12;
                Path[0].Y = Y + 12;
                Obstacles.Clear();
                break;
            case 52:
                switch (Spell)
                {
                    case 103:
                        MakeRangedPath(Side, 25, 0, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                        break;
                    case 107:
                        MakeRangedPath(Side, 25, 0, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                        Obstacles.Clear();
                        break;
                    case 117:
                        MakeRangedPath(Side, 50, 50, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                        break;
                    case 100:
                        MakeRangedPath(Side, 50, 35, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                        break;
                    case 119:
                        MakeRangedPath(Side, 50, 20, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                        break;
                    default:
                        switch (SpellTypes[Spell])
                        {
                            case TfxType.fxBreath:
                            case TfxType.fxEffect:
                            case TfxType.fxScreenNS:
                            case TfxType.fxScreenSwarm:
                            case TfxType.fxGeyser:
                            case TfxType.fxTremors:
                            case TfxType.fxCloud:
                            case TfxType.fxResurrect:
                            case TfxType.fxFrostBeam:
                            case TfxType.fxMindDecay:
                            case TfxType.fxShockWave:
                            case TfxType.fxSacFlame:
                            case TfxType.fxWinds:
                                var xyz2 = new TXYZ();
                                Path.Add(xyz2);
                                HXtoHP(X2, Y2, ref X, ref Y);
                                Path[0].X = X + 12;
                                Path[0].Y = Y + 12;
                                Obstacles.Clear();
                                break;
                            default:
                                MakeRangedPath(Side, 50, 50, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                                break;
                        }

                        break;
                }

                break;
            default:
                MakeRangedPath(Side, 50, 50, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                break;
        }
    }

    // AoWTC.pas:6479
    public static void MakeRangedPath(TCombatSideIndex Side, short Speed, short Arc, short UnusedHeight, short X1, short Y1, short X2, short Y2, short OffsetX, short OffsetY, bool IncludeUnits, ref TXYZList Path, ref TList Obstacles)
    {
        int I;
        int A;
        int B;
        int NX = default;
        int NY = default;
        int Steps;
        int X = default;
        int Y = default;
        int LastX = default;
        int LastY = default;
        int StartX = default;
        int StartY = default;
        int EndX = default;
        int EndY = default;
        TXYZ Point;
        float Height;
        TROb Obstacle;
        TTacticalCombatUnitHS UnitHS;
        TCityWall Wall;
        Path.Clear();
        Obstacles.Clear();
        HXtoHP(X1, Y1, ref X, ref Y);
        HXtoHP(X2, Y2, ref LastX, ref LastY);
        X += 13;
        Y += 13;
        LastX = LastX + 13 + OffsetX;
        LastY = LastY + 13 + OffsetY;
        I = 0;
        A = Math.Abs(X2 - X1);
        B = Math.Abs(Y2 - Y1);
        Steps = (A + B) * 100 / Speed;
        if (Steps < 2)
        {
            Steps = 2;
        }

        while (I <= Steps)
        {
            Point = new TXYZ();
            Path.Add(Point);
            Point.X = (LastX - X) * I / Steps + X;
            Point.Y = (LastY - Y) * I / Steps + Y;
            B = Steps / 2;
            if (I > B)
            {
                A = Steps - I + 1;
            }
            else
            {
                if (I == B)
                {
                    A = I;
                }
                else
                {
                    A = I + 1;
                }
            }

            if (Arc == 0)
            {
                Point.Z = 0;
            }
            else
            {
                Height = A * Steps * Arc / 10;
                Point.Z = Round(Math.Sqrt(Height)) - Steps * 10 / Arc;
            }

            I += 1;
        }

        LastX = 0;
        LastY = 0;
        Point = Path[0];
        HPtoHX(Point.X, Point.Y, ref StartX, ref StartY);
        var count = Path.Count;
        Point = Path[count - 1];
        HPtoHX(Point.X, Point.Y, ref EndX, ref EndY);
        I = 0;
        int last = Path.Count - 1;
        for (; I <= last; I += 1)
        {
            Point = Path[I];
            HPtoHX(Point.X, Point.Y, ref X, ref Y);
            if ((X != StartX || Y != StartY) && (X != EndX || Y != EndY))
            {
                Steps = 0;
                if (AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].TerrainType == 15)
                {
                    Steps = 3;
                }
                else
                {
                    if (AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].TerrainType == 7)
                    {
                        Steps = 3;
                    }
                    else
                    {
                        A = dHXtoRad(X, Y, StartX, StartY);
                        if (!IncludeUnits)
                        {
                            UnitHS = null;
                        }
                        else
                        {
                            UnitHS = (TTacticalCombatUnitHS)AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2228484);
                        }

                        Wall = (TCityWall)AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2229508);
                        if (Wall == null && Side == TCombatSideIndex.csAttacker)
                        {
                            Wall = (TCityWall)AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2229510);
                        }

                        if (A == 1)
                        {
                            if (UnitHS != null && UnitHS.FUnit.GetSide() != Side)
                            {
                                Steps = 1;
                            }

                            if (Wall != null && Wall.FHitPoints > 0 && Side == TCombatSideIndex.csAttacker)
                            {
                                Steps = 4;
                            }
                        }
                        else
                        {
                            if (A > 1)
                            {
                                if (UnitHS != null)
                                {
                                    Steps = 1;
                                }

                                if (Wall == null)
                                {
                                    if (AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].OverlayType == 8)
                                    {
                                        Steps = 1;
                                    }
                                }
                                else
                                {
                                    if (Wall.FHitPoints > 0)
                                    {
                                        if (A > 2)
                                        {
                                            Steps = 2;
                                        }
                                        else
                                        {
                                            Wall = null;
                                            B = 1;
                                            while (Wall == null && B < 7)
                                            {
                                                CenterHNtoHX(StartX, StartY, B, ref NX, ref NY);
                                                Wall = (TCityWall)AoWCombatMap.MapContainer.MapLevels[0].Field[NX, NY].FindOwnedChild(2229508);
                                                if (Wall == null)
                                                {
                                                    Wall = (TCityWall)AoWCombatMap.MapContainer.MapLevels[0].Field[NX, NY].FindOwnedChild(2229510);
                                                }

                                                B += 1;
                                            }

                                            if (Wall == null)
                                            {
                                                Steps = 2;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (Steps > 0)
                {
                    switch (Steps)
                    {
                        case 1:
                            HXtoHP(X, Y, ref A, ref B);
                            A = Point.X + 11 - A;
                            if (A > 23)
                            {
                                A = 47 - A;
                            }

                            B = Point.Y + 3 - B;
                            if (B > 15)
                            {
                                B = 31 - B;
                            }

                            A = (A + B - Point.Z) * 2;
                            break;
                        case 2:
                            A = 200 - Point.Z * 3;
                            break;
                        case 3:
                            A = 100;
                            break;
                        case 4:
                            A = 50;
                            break;
                    }

                    if (A > 0)
                    {
                        if (X != LastX || Y != LastY)
                        {
                            Obstacle = new TROb();
                            Obstacle.PathIndex = I;
                            Obstacles.Add(Obstacle);
                            LastX = X;
                            LastY = Y;
                            Steps = 1;
                        }
                        else
                        {
                            Obstacle = (TROb)Obstacles[Obstacles.Count - 1];
                            if (Steps == 2 && Obstacle.Chance < A)
                            {
                                Obstacle.Chance = A;
                            }
                        }

                        if (Steps != 2 && Steps != 4)
                        {
                            Obstacle.Chance += A;
                        }

                        if (Obstacle.Chance > 100)
                        {
                            Obstacle.Chance = 100;
                        }
                    }
                }
            }
        }
    }

    // AoWTC.pas:6591
    public static void RegisterAoWTCClasses(TEngine AEngine)
    {
        AoWEngine.RegisterEClasses([EClass.Of<TAoWCombatMap>(), EClass.Of<TAoWCombatMapLevel>(), EClass.Of<TUnitPositionHS>(), EClass.Of<TTacticalCombat>()]);
    }

    // AoWTC.pas:6596
    public static ushort GetMeleeDirIndex(int X, int DX, int DY)
    {
        ushort Result = 1;
        switch (DY)
        {
            case -1:
                switch (DX)
                {
                    case -1:
                        Result = 3;
                        break;
                    case 0:
                        Result = 4;
                        break;
                    case 1:
                        Result = 5;
                        break;
                }

                break;
            case 0:
                switch (DX)
                {
                    case -1:
                        if (X / 2 * 2 == X)
                        {
                            Result = 3;
                        }
                        else
                        {
                            Result = 2;
                        }

                        break;
                    case 1:
                        if (X / 2 * 2 == X)
                        {
                            Result = 5;
                        }
                        else
                        {
                            Result = 6;
                        }

                        break;
                }

                break;
            case 1:
                switch (DX)
                {
                    case -1:
                        Result = 2;
                        break;
                    case 0:
                        Result = 1;
                        break;
                    case 1:
                        Result = 6;
                        break;
                }

                break;
        }

        return Result;
    }

    // AoWTC.pas:6608
    public static ushort GetRangedDirIndex(int DX, int DY)
    {
        ushort Result;
        int Ratio;
        if (DX == 0)
        {
            Result = 0;
        }
        else
        {
            Ratio = Math.Abs(DY * 100 / DX);
            switch (Ratio)
            {
                case >= 0 and <= 12:
                    Result = 6;
                    break;
                case >= 13 and <= 40:
                    Result = 5;
                    break;
                case >= 41 and <= 76:
                    Result = 4;
                    break;
                case >= 77 and <= 129:
                    Result = 3;
                    break;
                case >= 130 and <= 240:
                    Result = 2;
                    break;
                case >= 241 and <= 759:
                    Result = 1;
                    break;
                default:
                    Result = 0;
                    break;
            }
        }

        if (DY > 0)
        {
            Result = (ushort)(12 - Result);
        }

        if (DX < 0 && Result > 0)
        {
            Result = (ushort)(24 - Result);
        }

        return Result;
    }

    public static TAoWCombatMap AoWCombatMap;
    public static int CombatXLoc;
    public static int CombatYLoc;
    public static int CombatLLoc;
    public static FixedArray_int_2 TCAttack;
    public static FixedArray_int_2 TCDamage;
    public static FixedArray_int_18 raDir = FixedArray_int_18.From([0, 0, 0, 1, 0, 2, 3, 4, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0]);
    public static FixedArray_Aow_Units_AoWTC_TfxType_170 AbilTypes = FixedArray_Aow_Units_AoWTC_TfxType_170.From([TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxArrow, TfxType.fxNone, TfxType.fxRock, TfxType.fxRock, TfxType.fxNone, TfxType.fxTouch, TfxType.fxTouch, TfxType.fxTouch, TfxType.fxBreath, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxRock, TfxType.fxRock, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxRay, TfxType.fxNone, TfxType.fxArrow, TfxType.fxArrow, TfxType.fxNone, TfxType.fxTouch, TfxType.fxArrow, TfxType.fxComet, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxBreath, TfxType.fxBreath, TfxType.fxBreath, TfxType.fxBreath, TfxType.fxBreath, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxComet, TfxType.fxComet, TfxType.fxComet, TfxType.fxComet, TfxType.fxComet, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone]);
    public static FixedArray_Aow_Units_AoWTC_TfxType_131 SpellTypes = FixedArray_Aow_Units_AoWTC_TfxType_131.From([TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxNone, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxNone, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxCometBall, TfxType.fxEffect, TfxType.fxHighPrayer, TfxType.fxRay, TfxType.fxCloud, TfxType.fxScreenNESW, TfxType.fxMindDecay, TfxType.fxChain, TfxType.fxBreath, TfxType.fxNone, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxTremors, TfxType.fxEffect, TfxType.fxBreath, TfxType.fxScreenSwarm, TfxType.fxScreenNS, TfxType.fxRock, TfxType.fxGeyser, TfxType.fxAltArrow, TfxType.fxFrostBeam, TfxType.fxEffect, TfxType.fxRock, TfxType.fxCometBall, TfxType.fxScreenNS, TfxType.fxSacredWrath, TfxType.fxWinds, TfxType.fxShockWave, TfxType.fxSacFlame, TfxType.fxResurrect, TfxType.fxResurrect]);
    public static FixedArray_int_72 BreathHN = FixedArray_int_72.From([1, 18, 7, 8, 36, 19, 20, 59, 60, 37, 38, 39, 2, 8, 9, 10, 21, 22, 23, 39, 40, 41, 42, 43, 3, 10, 11, 12, 24, 25, 26, 43, 44, 45, 46, 47, 4, 12, 13, 14, 27, 28, 29, 47, 48, 49, 50, 51, 5, 14, 15, 16, 30, 31, 32, 51, 52, 53, 54, 55, 6, 16, 17, 18, 33, 34, 35, 55, 56, 57, 58, 59]);
    public static FixedArray_int_60 BreathDirection = FixedArray_int_60.From([1, 2, 3, 4, 5, 6, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 1]);
    public static FixedArray_int_14 StatusAbils = FixedArray_int_14.From([149, 150, 48, 92, 93, 94, 95, 96, 97, 98, 107, 163, 127, 34]);
    public static FixedArray_int_2 WallISIndex = FixedArray_int_2.From([30, 0]);
    public static FixedArray_int_19 rndHex = FixedArray_int_19.From([6, 0, 1, 3, 4, 5, 2, 18, 12, 7, 11, 10, 17, 15, 9, 16, 13, 8, 14]);
    public static FixedArray_int_12 BreathHit = FixedArray_int_12.From([0, 1, 4, 7, 8, 2, 5, 9, 10, 3, 6, 11]);
    public static FixedArray_int_2 WallMaxHP = FixedArray_int_2.From([7, 13]);
    public static readonly string IndRStr = "Independents";
    public static readonly string BeginRoundRStr = "begin their round.";
    public static FixedArray_int_48 DefPosX = FixedArray_int_48.From([32, 30, 34, 32, 28, 36, 30, 34, 34, 36, 32, 38, 30, 36, 32, 34, 36, 34, 38, 32, 36, 30, 34, 32, 32, 34, 30, 32, 36, 28, 34, 30, 30, 28, 32, 26, 34, 28, 32, 30, 28, 30, 26, 32, 28, 34, 30, 32]);
    public static FixedArray_int_48 DefPosY = FixedArray_int_48.From([26, 27, 27, 28, 28, 28, 29, 29, 27, 28, 26, 29, 27, 30, 28, 29, 30, 31, 29, 32, 28, 31, 29, 30, 32, 31, 31, 30, 30, 30, 29, 29, 31, 30, 32, 29, 31, 28, 30, 29, 28, 27, 29, 26, 30, 27, 29, 28]);
    public static FixedArray_byte_3770 MapShape = FixedArray_byte_3770.From([0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 10, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 11, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 10, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 11, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 10, 10, 2, 14, 2, 14, 2, 14, 2, 14, 2, 11, 11, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 19, 14, 1, 14, 1, 14, 1, 14, 1, 14, 15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 19, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 19, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 19, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 19, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 19, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 19, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 15, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 8, 8, 8, 8, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 9, 9, 9, 9, 9, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 18, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 16, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 18, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 16, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 18, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 16, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 18, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 16, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 18, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 16, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 18, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 16, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 18, 17, 1, 1, 1, 1, 1, 1, 1, 1, 1, 17, 16, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 12, 5, 17, 17, 17, 17, 17, 17, 17, 17, 17, 5, 13, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 12, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 13, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 12, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 13, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 12, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 13, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
    public static FixedArray_byte_3770 MapRoads = FixedArray_byte_3770.From([0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 26, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22, 22, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 21, 2, 2, 2, 21, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 21, 2, 2, 2, 21, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 21, 2, 2, 2, 21, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 1, 1, 1, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 1, 1, 1, 1, 1, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 1, 1, 1, 1, 1, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 1, 1, 1, 1, 1, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 5, 1, 5, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 24, 5, 5, 5, 24, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 24, 5, 5, 5, 24, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 23, 23, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
    public static FixedArray_int_18 clusterChk = FixedArray_int_18.From([1, 2, 6, 16, 7, 8, 9, 17, 18, 19, 20, 21, 22, 23, 33, 34, 35, 36]);
    // AoWTC.pas:3664
    public static void OriginalUnitAssert(string Message, string Filename, int LineNumber)
    {
        Assert(false, Message, Filename, LineNumber);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
