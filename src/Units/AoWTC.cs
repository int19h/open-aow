// Translated from AoWTC.pas.
using System.Runtime.CompilerServices;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat
{

    public enum CombatMoveModeElement : byte
    {
        WallCrushing = 0,
        Melee = 1,
        MoveRanged = 2,
        Move = 3,
        Wall = 4,
        Climbing = 5
    }

    [LegacyObject]
    // { ot=1 }
    public partial class CombatAbilityRef
    {
        public int Ability;
        public int Spell;

        public CombatAbilityRef()
        {
            CaptureAddress.Assign(this);
        }
    }

    [LegacyObject]
    // PathIndex. Chance
    public partial class ROb
    {
        public int PathIndex;
        public int Chance;

        public ROb()
        {
            CaptureAddress.Assign(this);
        }
    }

    // Inherited Width. Height. Levels. HSSFileName. HSMapClassID. MapLevelClassID
    public partial class AoWNewCombatMapSettings : NewHSMapSettings
    {
        public AoWNewCombatMapSettings() : base()
        {
            HSMapClassID = 2228480;
            MapLevelClassID = 2228482;
            Width = 65;
            Height = 59;
            Levels = 1;
            HSSFileName = GameTables.AoWEngine.HSSFileName;
        }

        // AoWTC.pas:2147

        // AoWTC.pas:70

    }

    [LegacyObject]
    // MoveType. Ability. Spell. Value. GroupValue. UnitHS. Target. X. Y
    public partial class TaiMove
    {
        public TaiMoveType MoveType;
        public short Ability;
        public short Spell;
        public int Value;
        public int GroupValue;
        public TacticalCombatUnitHS UnitHS;
        public CombatObject Target;
        public ushort X;
        public ushort Y;

        public TaiMove()
        {
            CaptureAddress.Assign(this);
        }
    }

    [LegacyObject]
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
            CaptureAddress.Assign(this);
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
                                    Wall = (CityWall)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path2.X, path.Y].FindOwnedChild(2229510);
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
                    Wall = (CityWall)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2229510);
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
                                                                    CityWall = (CityWall)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path3.X, path2.Y].FindOwnedChild(2229510);
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
                                                                CityWall = (CityWall)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path3.X, path2.Y].FindOwnedChild(2229510);
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
                                                Wall = (CityWall)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Move.X, Move.Y].FindOwnedChild(2229510);
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

    // Inherited world-map field layout
    public partial class AoWCombatMapField : AoWMapField
    {
        public AoWCombatMapField(ECustomNode owner) : base(owner)
        { }

        // AoWTC.pas:2158
        public new const int ClassID = 2228481;

        // AoWTC.pas:2163
        public sbyte GetDefenseValue()
        {
            MapFieldMsg Msg = default;
            Msg.MsgID = 135169;
            Msg.Result = -1;
            Msg.Param[0] = 0;
            SendMapFieldMsg(ref Msg);
            return (sbyte)Msg.Param[0].Int;
        }

    }

    // Inherited world-map level layout
    public partial class AoWCombatMapLevel : AoWMapLevel
    {
        public AoWCombatMapLevel(ECustomNode owner) : base(owner)
        { }

        // AoWTC.pas:2173
        public new const int ClassID = 2228482;

        // AoWTC.pas:2178
        public override EClass MapFieldClass()
        {
            // { Original uses the world-map field class. }
            return EClass.Of<AoWMapField>();
        }

    }
}

namespace Aow.Game.Draw
{

    // FMiscImages. FTargetImages. FFrame. FFrameCount. FSelection. FRange
    public partial class RangedPathHS : FixedVisibleHS
    {
        public RangedPathHS(ECustomNode owner) : base(owner)
        { }

        // AoWTC.pas:4846
        public new const int ClassID = 2228485;

        public CustomImageLibrary FMiscImages;
        public CustomImageLibrary FTargetImages;
        public ushort FFrame;
        public ushort FFrameCount;
        public CombatUnitSelectionControl FSelection;
        public ushort FRange;

        // AoWTC.pas:4849
        public override int GetShowPriority() => 1811939328;

        // AoWTC.pas:4859
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
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
                        HexGeometry.CenterHNtoHX(FSelection.GetTCUnitHS().GetXhx(), yhx, I, ref NX, ref NY);
                        HexGeometry.HXtoHP(NX, NY, ref PX, ref PY);
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
                        EffectType = TacticalTables.SpellTypes[tcUnitHS.FSelectedSpell];
                    }
                    else
                    {
                        var tcUnitHS = FSelection.GetTCUnitHS();
                        EffectType = TacticalTables.AbilTypes[tcUnitHS.FSelectedAbility];
                    }

                    switch (EffectType)
                    {
                        case TfxType.Breath:
                            var count = FSelection.FRangedPath.Count;
                            var rangedPath = FSelection.FRangedPath[count - 1];
                            var fRangedPathCount = FSelection.FRangedPath.Count;
                            HexGeometry.HPtoHX(FSelection.FRangedPath[fRangedPathCount - 1].X, rangedPath.Y, ref X, ref Y);
                            {
                                for (int I = 0; I <= 11; I++)
                                {
                                    var yhx = FSelection.GetTCUnitHS().GetYhx();
                                    var dHXtoHN2 = HexGeometry.dHXtoHN(FSelection.GetTCUnitHS().GetXhx(), yhx, X, Y);
                                    var yhx2 = FSelection.GetTCUnitHS().GetYhx();
                                    HexGeometry.CenterHNtoHX(FSelection.GetTCUnitHS().GetXhx(), yhx2, TacticalTables.BreathHN[dHXtoHN2 * 12 + I - 12], ref NX, ref NY);
                                    HexGeometry.HXtoHP(NX, NY, ref PX, ref PY);
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
                                                    HexGeometry.HPtoHX(FSelection.FRangedPath[count3 - 1].X, rangedPathY.Y, ref X, ref Y);
                                                    HexGeometry.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                    HexGeometry.HXtoHP(NX, NY, ref PX, ref PY);
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
                                                    HexGeometry.HPtoHX(FSelection.FRangedPath[count3 - 1].X, rangedPathY.Y, ref X, ref Y);
                                                    HexGeometry.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                    HexGeometry.HXtoHP(NX, NY, ref PX, ref PY);
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
                        var rangedPath = FSelection.FRangedPath[((ROb)obstacle).PathIndex];
                        var obstacle2 = FSelection.FObstacles[I];
                        HexGeometry.HPtoHX(FSelection.FRangedPath[((ROb)obstacle2).PathIndex].X, rangedPath.Y, ref NX, ref NY);
                        HexGeometry.HXtoHP(NX, NY, ref PX, ref PY);
                        FMiscImages[1].Show(PX + 5 - XhpSurf, PY + 5 - YhpSurf, SurfaceDesc);
                        GameTables.AoWEngine.FCombatFont.Color = 0;
                        var obstacle3 = FSelection.FObstacles[I];
                        GameTables.AoWEngine.FCombatFont.SetTextCentered(PX + 16 - XhpSurf, PY + 12 - YhpSurf, IntToStr(((ROb)obstacle3).Chance), SurfaceDesc);
                    }
                }
            }
        }

        // AoWTC.pas:4852
        public override void Activate()
        {
            base.Activate();
            FMiscImages = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TCombat\\Misc.ILB");
            FTargetImages = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TargHex.ILB");
        }

        // AoWTC.pas:4839
        public override void Deactivate()
        {
            AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TCombat\\Misc.ILB");
            AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TargHex.ILB");
            base.Deactivate();
        }

        // Engine.pas:524

    }

    // FDone. FCancelled. FFrame. FAbilityIndex. FSpellIndex. FPath. FVariant. FTargetX. FTargetY.
    // FTarget. FEffectType
    public partial class TfxHS : FixedVisibleHS
    {

        public TfxHS(ECustomNode owner) : base(owner)
        {
            FPath = new XYZList();
            FFrame = -1;
        }

        // AoWTC.pas:5227
        public new const int ClassID = 2228486;

        public bool FDone;
        public bool FCancelled;
        public int FFrame;
        public int FAbilityIndex;
        public int FSpellIndex;
        public XYZList FPath;
        public int FVariant;
        public int FTargetX;
        public int FTargetY;
        public CombatObject FTarget;
        public CombatAction FCombatAction;
        public TfxType FEffectType;

        // AoWTC.pas:5221
        public override void Dispose()
        {
            FPath?.Dispose();
            base.Dispose();
        }

        // AoWTC.pas:5214

        // AoWTC.pas:5239
        public override int GetShowPriority() => 1811939328;

        // AoWTC.pas:5232
        public override void Activate()
        {
            base.Activate();
            AoWCombatMap.Current.NewFrameEvents += NewFrame;
            FFrame = -1;
        }

        // AoWTC.pas:5208
        public override void Deactivate()
        {
            AoWCombatMap.Current.NewFrameEvents -= NewFrame;
            base.Deactivate();
        }

        // AoWTC.pas:5244
        public void NewFrame(object Sender)
        {
            TfxHS Effect;
            int J = default;
            int I = default;
            ImageSequence Sequence;
            XYZList NewPath;
            int K = default;
            int X = default;
            int Y = default;
            float Z = default;
            float Speed = default;
            float VZ = default;
            float Gravity = default;
            TfxHS Effect2;
            TacticalCombatUnitHS UnitHS;
            MapFieldMsg Msg = default;
            void MakeCloud(int Count, int Radius)
            {
                for (int N = 1; N <= Count - 1; N++)
                {
                    Effect = AoWCombatMap.Current.AddEffect(TfxType.CloudPuff);
                    Effect.FAbilityIndex = FAbilityIndex;
                    Effect.FSpellIndex = FSpellIndex;
                    var xyz = new XYZ();
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
                HexGeometry.HPtoHX(FPath[FFrame].X, pathY.Y, ref I, ref J);
                if (AoWCombatMap.Current.MapContainer.MapLevels[0].Field[I, J].TerrainType == 0)
                {
                    Effect = AoWCombatMap.Current.AddEffect(TfxType.HitWater);
                }
                else
                {
                    if (AoWCombatMap.Current.MapContainer.MapLevels[0].Field[I, J].OverlayType != 8 && AoWCombatMap.Current.MapContainer.MapLevels[0].Field[I, J].FindOwnedChild(2229508) == null && AoWCombatMap.Current.MapContainer.MapLevels[0].Field[I, J].FindOwnedChild(2229510) == null)
                    {
                        Effect = AoWCombatMap.Current.AddEffect(TfxType.HitGround);
                    }
                }

                if (Effect != null)
                {
                    var xyz = new XYZ();
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
                        case TfxType.Arrow:
                        case TfxType.Rock:
                        case TfxType.Comet:
                        case TfxType.Breath:
                        case TfxType.CometBall:
                        case TfxType.Swarm:
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

                            if (FFrame == 0 && AoWCombatMap.Current.FPlaySounds && FAbilityIndex == 52 && FSpellIndex == 122)
                            {
                                AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                            }

                            var count = FPath.Count;
                            if (FFrame >= count - 1)
                            {
                                if (AoWCombatMap.Current.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
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
                                HexGeometry.HPtoHX(FPath[FFrame].X, pathY.Y - pathZ.Z, ref I, ref J);
                                AoWCombatMap.Current.MakeVisible(I, J, 0, 2);
                            }

                            break;
                        case TfxType.DecayComet:
                            if (FFrame == 0 && AoWCombatMap.Current.FPlaySounds && FAbilityIndex == 52)
                            {
                                AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                            }

                            var fPathCount = FPath.Count;
                            if (FFrame >= fPathCount - 1)
                            {
                                if (AoWCombatMap.Current.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                    }
                                }

                                FFrame = FPath.Count - 1;
                                FDone = true;
                            }

                            break;
                        case TfxType.AltArrow:
                            if (FFrame == 0 && AoWCombatMap.Current.FPlaySounds && FAbilityIndex == 52)
                            {
                                AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                            }

                            FFrame += 3;
                            var count2 = FPath.Count;
                            if (FFrame >= count2 - 1)
                            {
                                if (AoWCombatMap.Current.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
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
                                HexGeometry.HPtoHX(FPath[FFrame].X, pathY.Y - pathZ.Z, ref I, ref J);
                                AoWCombatMap.Current.MakeVisible(I, J, 0, 2);
                            }

                            break;
                        case TfxType.ScreenNS:
                            FFrame += 1;
                            var count3 = FPath.Count;
                            if (FFrame >= count3 - 1)
                            {
                                if (AoWCombatMap.Current.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                    }
                                }

                                FFrame = FPath.Count - 1;
                                FDone = true;
                                Effect = AoWCombatMap.Current.AddEffect(TfxType.HailImpact);
                                Effect.FAbilityIndex = FAbilityIndex;
                                Effect.FSpellIndex = FSpellIndex;
                                var xyz = new XYZ();
                                Effect.FPath.Add(xyz);
                                var pathX = FPath[FFrame];
                                Effect.FPath[0].X = pathX.X;
                                var pathY = FPath[FFrame];
                                Effect.FPath[0].Y = pathY.Y;
                            }

                            break;
                        case TfxType.HailImpact:
                            if (FAbilityIndex == 52)
                            {
                                Sequence = AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[2];
                            }
                            else
                            {
                                Sequence = AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[2];
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
                        case TfxType.PrayerGlow:
                            if (FAbilityIndex == 52)
                            {
                                Sequence = AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[1];
                            }
                            else
                            {
                                Sequence = AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[1];
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
                        case TfxType.SFOverlay:
                            var count4 = FPath.Count;
                            if (FFrame >= count4 - 1)
                            {
                                FFrame = FPath.Count - 1;
                                FDone = true;
                            }

                            break;
                        case TfxType.ScreenSwarm:
                            if (FFrame == 0)
                            {
                                FFrame = 1;
                            }

                            var count5 = FPath.Count;
                            if (FFrame >= count5 - 1)
                            {
                                if (AoWCombatMap.Current.FPlaySounds)
                                {
                                    if (FTarget != null && FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                    }
                                }

                                FFrame = FPath.Count - 1;
                                FDone = true;
                            }

                            break;
                        case TfxType.Ray:
                            FFrame += 12;
                            var count6 = FPath.Count;
                            if (FFrame >= count6 - 1)
                            {
                                if (AoWCombatMap.Current.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                    }
                                }

                                FFrame = FPath.Count - 1;
                                FDone = true;
                            }

                            break;
                        case TfxType.FrostBeam:
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
                                    HexGeometry.HXtoHP(FTargetX, FTargetY, ref K, ref X);
                                    {
                                        I = 0;
                                        int last = J;
                                        for (; I <= last; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var random = Random(100);
                                            var random2 = Random(5);
                                            var random3 = Random(100);
                                            EffectPaths.MakeArcPathXY((short)(K - 200), (short)(X - 200), (short)(random3 + K - 50), (short)(random + X - 50), (short)(random2 + 5), 100, ref Effect.FPath);
                                        }
                                    }

                                    J = Random(10) + 10;
                                    {
                                        I = 0;
                                        int last = J;
                                        for (; I <= last; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            Speed = (float)((double)(Random(50) + 20) / 10);
                                            Gravity = (float)0.5;
                                            VZ = (float)((double)Random(10) / 10);
                                            Z = 100;
                                            var random = Random(360);
                                            EffectPaths.MakeArcPath(K - 200, X - 100, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                        }
                                    }

                                    break;
                                case >= 1000 and <= 1029:
                                    HexGeometry.HXtoHP(FTargetX, FTargetY, ref K, ref X);
                                    J = Random(5) + 5;
                                    {
                                        I = 0;
                                        int last = J;
                                        for (; I <= last; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var random = Random(100);
                                            var random2 = Random(5);
                                            var random3 = Random(100);
                                            EffectPaths.MakeArcPathXY((short)(K - 200), (short)(X - 200), (short)(random3 + K - 50), (short)(random + X - 50), (short)(random2 + 5), 100, ref Effect.FPath);
                                        }
                                    }

                                    J = Random(10) + 10;
                                    {
                                        I = 0;
                                        int last = J;
                                        for (; I <= last; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            Speed = (float)((double)(Random(50) + 20) / 10);
                                            Gravity = (float)0.5;
                                            VZ = (float)((double)Random(10) / 10);
                                            Z = 100;
                                            var random = Random(360);
                                            EffectPaths.MakeArcPath(K - 200, X - 100, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                        }
                                    }

                                    J = Random(5) + 5;
                                    {
                                        I = 0;
                                        int last = J;
                                        for (; I <= last; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
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
                                            EffectPaths.MakeArcPath(pathX.X, path.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
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
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
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
                                            EffectPaths.MakeArcPath(pathX.X, path.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                        }
                                    }

                                    break;
                                case 3000:
                                    if (AoWCombatMap.Current.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                        }
                                    }

                                    {
                                        I = 0;
                                        int last = FPath.Count - 1;
                                        for (; I <= last; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.Frost);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            Speed = (float)((double)(Random(25) + 25) / 10);
                                            Gravity = (float)0.5;
                                            VZ = (float)((double)(Random(50) + 40) / 10);
                                            Z = 0;
                                            var pathY = FPath[I];
                                            var pathZ = FPath[I];
                                            var random = Random(360);
                                            EffectPaths.MakeArcPath(FPath[I].X, pathY.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                        }
                                    }

                                    J = Random(10) + 100;
                                    {
                                        I = 0;
                                        int last = J;
                                        for (; I <= last; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.Frost);
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
                                            EffectPaths.MakeArcPath(pathX.X, path.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                        }
                                    }

                                    FDone = true;
                                    FTarget = null;
                                    break;
                            }

                            break;
                        case TfxType.ShockWave:
                            switch (FFrame)
                            {
                                case >= 0 and <= 29:
                                    if (FFrame == 0 && AoWCombatMap.Current.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                                        }
                                        else
                                        {
                                            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                                        }
                                    }

                                    {
                                        I = 1;
                                        for (; I <= 20; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var random = Random(360);
                                            var pathX = FPath[0];
                                            var pathY = FPath[0];
                                            var random2 = Random(2);
                                            var random3 = Random(10);
                                            EffectPaths.MakeSwarmPath(Random(10) + 10, random3 + FFrame / 4, I % 6 * 60 + FFrame, random, pathX.X, pathY.Y, (SwarmDirection)random2, ref Effect.FPath);
                                        }
                                    }

                                    break;
                                case >= 30 and <= 59:
                                    {
                                        I = 1;
                                        for (; I <= 20; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var random = Random(360);
                                            var pathX = FPath[0];
                                            var pathY = FPath[0];
                                            var random2 = Random(2);
                                            var random3 = Random(10);
                                            EffectPaths.MakeSwarmPath(Random(10) + 10, random3 + (60 - FFrame) / 4, I % 6 * 60 + FFrame, random, pathX.X, pathY.Y, (SwarmDirection)random2, ref Effect.FPath);
                                        }
                                    }

                                    break;
                                case >= 60 and <= 89:
                                    {
                                        I = 1;
                                        for (; I <= 20; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var random = Random(360);
                                            var pathX = FPath[0];
                                            var pathY = FPath[0];
                                            var random2 = Random(2);
                                            var random3 = Random(10);
                                            EffectPaths.MakeSwarmPath(Random(10) + 10, random3 + (FFrame - 60) / 2, I % 6 * 60 + FFrame, random, pathX.X, pathY.Y, (SwarmDirection)random2, ref Effect.FPath);
                                        }
                                    }

                                    break;
                                case >= 90 and <= 119:
                                    {
                                        I = 1;
                                        for (; I <= 20; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var random = Random(360);
                                            var pathX = FPath[0];
                                            var pathY = FPath[0];
                                            var random2 = Random(2);
                                            var random3 = Random(10);
                                            EffectPaths.MakeSwarmPath(Random(10) + 10, random3 + (120 - FFrame) / 2, I % 6 * 60 + FFrame, random, pathX.X, pathY.Y, (SwarmDirection)random2, ref Effect.FPath);
                                        }
                                    }

                                    break;
                                case 150:
                                    if (AoWCombatMap.Current.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                        }
                                    }

                                    X = 200;
                                    Y = 200;
                                    J = Random(5);
                                    {
                                        I = 0;
                                        for (; I <= 359; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
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
                                            EffectPaths.MakeLinePath(FPath[0].X, path.Y, pathX.X + Round((double)cos * (random2 + X)), pathY.Y + Round((double)sin * (random + Y)) / 2, 75, ref Effect.FPath);
                                            if (I % 10 == 0)
                                            {
                                                K = 1;
                                                for (; K <= 10; K += 1)
                                                {
                                                    Effect2 = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                                                    Effect2.FAbilityIndex = FAbilityIndex;
                                                    Effect2.FSpellIndex = FSpellIndex;
                                                    var count7 = Effect.FPath.Count;
                                                    var path2 = Effect.FPath[count7 - 1];
                                                    var count8 = Effect.FPath.Count;
                                                    var path3 = Effect.FPath[count8 - 1];
                                                    var path4 = FPath[0];
                                                    EffectPaths.MakeLinePath(FPath[0].X, path4.Y, path3.X, path2.Y, K * 5 + 75, ref Effect2.FPath);
                                                }
                                            }
                                        }
                                    }

                                    break;
                                case >= 151 and <= 169:
                                case >= 210 and <= 229:
                                    var path5 = FPath[0];
                                    HexGeometry.HPtoHX(FPath[0].X, path5.Y, ref X, ref Y);
                                    Msg.MsgID = 200720;
                                    Msg.Param[0] = 1;
                                    Msg.Result = -1;
                                    {
                                        I = 1;
                                        for (; I <= 36; I += 1)
                                        {
                                            HexGeometry.CenterHNtoHX(X, Y, I, ref J, ref K);
                                            AoWCombatMap.Current.MapContainer.MapLevels[0].Field[J, K].SendMapFieldMsg(ref Msg);
                                        }
                                    }

                                    break;
                                case >= 170 and <= 184:
                                case >= 195 and <= 209:
                                    var path6 = FPath[0];
                                    HexGeometry.HPtoHX(FPath[0].X, path6.Y, ref X, ref Y);
                                    Msg.MsgID = 200720;
                                    Msg.Param[0] = 2;
                                    Msg.Result = -1;
                                    {
                                        I = 1;
                                        for (; I <= 36; I += 1)
                                        {
                                            HexGeometry.CenterHNtoHX(X, Y, I, ref J, ref K);
                                            AoWCombatMap.Current.MapContainer.MapLevels[0].Field[J, K].SendMapFieldMsg(ref Msg);
                                        }
                                    }

                                    break;
                                case >= 185 and <= 194:
                                    var path7 = FPath[0];
                                    HexGeometry.HPtoHX(FPath[0].X, path7.Y, ref X, ref Y);
                                    Msg.MsgID = 200720;
                                    Msg.Param[0] = 3;
                                    Msg.Result = -1;
                                    {
                                        I = 1;
                                        for (; I <= 36; I += 1)
                                        {
                                            HexGeometry.CenterHNtoHX(X, Y, I, ref J, ref K);
                                            AoWCombatMap.Current.MapContainer.MapLevels[0].Field[J, K].SendMapFieldMsg(ref Msg);
                                        }
                                    }

                                    break;
                                case 230:
                                    var path8 = FPath[0];
                                    HexGeometry.HPtoHX(FPath[0].X, path8.Y, ref X, ref Y);
                                    Msg.MsgID = 200721;
                                    Msg.Result = -1;
                                    {
                                        I = 1;
                                        for (; I <= 36; I += 1)
                                        {
                                            HexGeometry.CenterHNtoHX(X, Y, I, ref J, ref K);
                                            AoWCombatMap.Current.MapContainer.MapLevels[0].Field[J, K].SendMapFieldMsg(ref Msg);
                                        }
                                    }

                                    FFrame = -1;
                                    break;
                            }

                            break;
                        case TfxType.Winds:
                            switch (FFrame)
                            {
                                case >= 0 and <= 1999:
                                    var count7 = FPath.Count;
                                    if (FFrame >= count7 + 20)
                                    {
                                        if (FTarget != null && FTarget.GetEnabled())
                                        {
                                            if (AoWCombatMap.Current.FPlaySounds)
                                            {
                                                if (FAbilityIndex == 52)
                                                {
                                                    AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                                }
                                                else
                                                {
                                                    AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                                }
                                            }

                                            var count8 = FPath.Count;
                                            var path = FPath[count8 - 1];
                                            var count9 = FPath.Count;
                                            HexGeometry.HPtoHX(FPath[count9 - 1].X, path.Y, ref J, ref K);
                                            var mapField = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[J, K];
                                            ((TacticalCombatUnit)FTarget).FUnitHS.MoveTo(mapField, 0);
                                            ((TacticalCombatUnit)FTarget).FUnitHS.FOffsetX = 0;
                                            ((TacticalCombatUnit)FTarget).FUnitHS.FOffsetY = 0;
                                            ((TacticalCombatUnit)FTarget).FUnitHS.FBusy = false;
                                        }

                                        FFrame = -1;
                                    }
                                    else
                                    {
                                        if (AoWCombatMap.Current.FPlaySounds && (FFrame == FTargetX || FFrame == FTargetY))
                                        {
                                            if (FAbilityIndex == 52)
                                            {
                                                AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                                            }
                                            else
                                            {
                                                AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                                            }
                                        }

                                        if (FTarget != null && FTarget.GetEnabled())
                                        {
                                            var count8 = FPath.Count;
                                            if (FFrame - 20 < count8 && FFrame >= 20)
                                            {
                                                var pathX = FPath[FFrame - 20];
                                                var path = FPath[0];
                                                ((TacticalCombatUnit)FTarget).FUnitHS.FOffsetX = pathX.X - path.X;
                                                var pathY = FPath[FFrame - 20];
                                                var pathZ = FPath[FFrame - 20];
                                                var path2 = FPath[0];
                                                ((TacticalCombatUnit)FTarget).FUnitHS.FOffsetY = pathY.Y - pathZ.Z - path2.Y;
                                                ((TacticalCombatUnit)FTarget).FUnitHS.SetDirection((byte)(((TacticalCombatUnit)FTarget).FUnitHS.FDirection + 1));
                                                if (((TacticalCombatUnit)FTarget).FUnitHS.FDirection > 6)
                                                {
                                                    ((TacticalCombatUnit)FTarget).FUnitHS.SetDirection(1);
                                                }

                                                ((TacticalCombatUnit)FTarget).FUnitHS.FBusy = true;
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
                                                Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
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
                                                EffectPaths.MakeSwarmPath(Random(5) + 20, (count8 - I) / 5, 0, random, pathX.X + random2 - 5, pathY.Y - pathZ.Z + random3 - 5, (SwarmDirection)random4, ref Effect.FPath);
                                            }
                                        }
                                    }

                                    break;
                            }

                            break;
                        case TfxType.SacFlame:
                            switch (FFrame)
                            {
                                case >= 0 and <= 99:
                                    if (FFrame == 0 && AoWCombatMap.Current.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                                        }
                                        else
                                        {
                                            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                                        }
                                    }

                                    J = Random(6) + 10;
                                    var yhx = ((TacticalCombatUnit)FTarget).FUnitHS.GetYhx();
                                    HexGeometry.HXtoHP(((TacticalCombatUnit)FTarget).FUnitHS.GetXhx(), yhx, ref K, ref X);
                                    {
                                        I = 1;
                                        int last = J - 1;
                                        for (; I <= last; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.Frost);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            Speed = (float)((double)(Random(25) + 25) / 10);
                                            Gravity = (float)0.5;
                                            VZ = (float)((double)(Random(50) + FFrame) / 10);
                                            Z = 0;
                                            EffectPaths.MakeArcPath(K + 16, X + 16, I * 360 / J, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
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
                                            if (AoWCombatMap.Current.FPlaySounds)
                                            {
                                                if (FAbilityIndex == 52)
                                                {
                                                    AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                                }
                                                else
                                                {
                                                    AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                                }
                                            }

                                            J = Random(10) + 100;
                                            {
                                                I = 0;
                                                int last = J;
                                                for (; I <= last; I += 1)
                                                {
                                                    Effect = AoWCombatMap.Current.AddEffect(TfxType.Frost);
                                                    Effect.FAbilityIndex = FAbilityIndex;
                                                    Effect.FSpellIndex = FSpellIndex;
                                                    Speed = (float)((double)(Random(50) + 50) / 10);
                                                    Gravity = (float)0.1;
                                                    VZ = (float)((double)(Random(5) + 5) / 10);
                                                    Z = 0;
                                                    var random = Random(360);
                                                    EffectPaths.MakeArcPath(K + 16, X + 16, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                                }
                                            }

                                            break;
                                    }

                                    break;
                                case >= 125 and <= 199:
                                    if (FFrame == 125)
                                    {
                                        var path = FPath[0];
                                        HexGeometry.HPtoHX(FPath[0].X, path.Y, ref I, ref J);
                                        AoWCombatMap.Current.CenterViewTo(I, J - 3, 0);
                                    }

                                    if (AoWCombatMap.Current.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
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
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.Frost);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            Speed = (float)((double)(Random(50) + 25) / 10);
                                            Gravity = (float)0.5;
                                            VZ = (float)((double)(Random(50) + 100) / 10);
                                            Z = 0;
                                            EffectPaths.MakeArcPath(K, X, I * 360 / J, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                        }
                                    }

                                    J = Random(10) + 50;
                                    {
                                        I = 0;
                                        int last = J;
                                        for (; I <= last; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.Frost);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            Speed = (float)((double)(Random(50) + 50) / 10);
                                            Gravity = (float)0.1;
                                            VZ = (float)((double)(Random(5) + 5) / 10);
                                            Z = 0;
                                            var random = Random(360);
                                            EffectPaths.MakeArcPath(K, X, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                        }
                                    }

                                    break;
                                case 200:
                                    FFrame = -1;
                                    break;
                            }

                            break;
                        case TfxType.MindDecay:
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
                                                Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                                                Effect.FAbilityIndex = FAbilityIndex;
                                                Effect.FSpellIndex = FSpellIndex;
                                                var random = Random(360);
                                                var pathX = FPath[0];
                                                var pathY = FPath[0];
                                                var random2 = Random(2);
                                                EffectPaths.MakeSwarmPath(Random(10) + 10, I / 2, 45, random, pathX.X + 100 - I, pathY.Y - 200 - 50 + I, (SwarmDirection)random2, ref Effect.FPath);
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
                                                Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                                                Effect.FAbilityIndex = FAbilityIndex;
                                                Effect.FSpellIndex = FSpellIndex;
                                                var random = Random(360);
                                                var pathX = FPath[0];
                                                var pathY = FPath[0];
                                                var random2 = Random(2);
                                                EffectPaths.MakeSwarmPath(Random(10) + 10, I / 2, 45, random, pathX.X + 100 - I, pathY.Y - 200 - 50 + I, (SwarmDirection)random2, ref Effect.FPath);
                                            }
                                        }
                                    }

                                    if (FFrame % 5 == 0 && FFrame < 85)
                                    {
                                        Effect = AoWCombatMap.Current.AddEffect(TfxType.DecayComet);
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
                                        EffectPaths.MakeArcPathXY((short)(path2.X + random5 + 25), (short)(path.Y + random4 - 225), (short)(pathX.X + random3 - 25), (short)(pathY.Y + random - 25), (short)(random2 + 5), 100, ref Effect.FPath);
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
                                            EffectPaths.MakeArcPath(path4.X, path3.Y - pathZ.Z, random6, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                        }

                                        HexGeometry.CenterHNtoHX(FTargetX, FTargetY, (FFrame - 50) / 5, ref J, ref K);
                                        // { Original swaps the converted hex coordinates. }
                                        HexGeometry.HXtoHP(J, K, ref X, ref Y);
                                        UnitHS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[J, K].FindOwnedChild(2228484);
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
                                            EffectPaths.MakeArcPathXY((short)Effect.FPath[count9 - 1].X, (short)(path3.Y - pathZ.Z), (short)(X + 16), (short)(Y - 8), (short)(random6 + 6), 100, ref Effect.FPath);
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
                                                EffectPaths.MakeLinePath(Effect.FPath[count15 - 1].X, path6.Y - path7.Z, path8.X, path4.Y - path5.Z - 6, 3, ref Effect.FPath);
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
                                                EffectPaths.MakeLinePath(Effect.FPath[count21 - 1].X, fPathItem.Y - path10.Z, path11.X, path9.Y - item.Z + 6, 3, ref Effect.FPath);
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
                        case TfxType.Chain:
                            NewPath = new XYZList();
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
                                    EffectPaths.MakeLightningPath(FPath[J].X, path.Y, pathX.X, pathY.Y, ref NewPath);
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

                            FPath?.Dispose();
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
                                if (AoWCombatMap.Current.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                    }
                                }

                                var pathY = FPath[FFrame];
                                var pathZ = FPath[FFrame];
                                HexGeometry.HPtoHX(FPath[FFrame].X, pathY.Y - pathZ.Z, ref I, ref J);
                                AoWCombatMap.Current.MakeVisible(I, J, 0, 3);
                            }

                            break;
                        case TfxType.Touch:
                        case TfxType.Effect:
                            if (FFrame == 0 && AoWCombatMap.Current.FPlaySounds && FAbilityIndex == 65)
                            {
                                AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                            }

                            if (FAbilityIndex == 52)
                            {
                                Sequence = AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[0];
                            }
                            else
                            {
                                Sequence = AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[0];
                            }

                            if (Sequence != null)
                            {
                                var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= totalSequenceFrames - 1)
                                {
                                    if (AoWCombatMap.Current.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
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
                        case TfxType.SacredWrath:
                            if (FAbilityIndex == 52)
                            {
                                Sequence = AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[1];
                            }
                            else
                            {
                                Sequence = AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[1];
                            }

                            if (Sequence == null)
                            {
                                FFrame = -1;
                            }

                            break;
                        case TfxType.FBExplosion:
                            Sequence = AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[1];
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
                        case TfxType.ScreenNESW:
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
                        case TfxType.DamLightning:
                        case TfxType.DamFire:
                        case TfxType.DamPoison:
                        case TfxType.DamMagic:
                        case TfxType.DamHoly:
                        case TfxType.DamDeath:
                        case TfxType.DamCold:
                        case TfxType.GeyserWater:
                        case TfxType.Frost:
                        case TfxType.SacFire:
                            var count23 = FPath.Count;
                            if (FFrame >= count23 - 1)
                            {
                                FFrame = FPath.Count - 1;
                                FDone = true;
                            }

                            break;
                        case TfxType.WallSmoke:
                            Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[5];
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
                        case TfxType.HitGround:
                            Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[9];
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
                        case TfxType.HitWater:
                            Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[10];
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
                        case TfxType.Blood:
                            var pathZ2 = FPath[0];
                            Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[pathZ2.Z];
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
                        case TfxType.Tremors:
                            switch (FFrame)
                            {
                                case 0:
                                    var xyz = new XYZ();
                                    FPath.Add(xyz);
                                    FPath[0].X = AoWCombatMap.Current.ViewXhp;
                                    FPath[0].Y = AoWCombatMap.Current.ViewYhp;
                                    break;
                                case >= 1 and <= 29:
                                    var pathY = FPath[0];
                                    var random = Random(FFrame / 3);
                                    var pathX = FPath[0];
                                    var random2 = Random(FFrame / 3);
                                    AoWCombatMap.Current.ChangeViewTo(pathX.X + random2 - FFrame / 6, pathY.Y + random - FFrame / 6, AoWCombatMap.Current.ViewL);
                                    break;
                                case >= 30 and <= 89:
                                    var path = FPath[0];
                                    var random3 = Random(10);
                                    var path2 = FPath[0];
                                    var random4 = Random(10);
                                    AoWCombatMap.Current.ChangeViewTo(path2.X + random4 - 5, path.Y + random3 - 5, AoWCombatMap.Current.ViewL);
                                    break;
                                case >= 90 and <= 119:
                                    var path3 = FPath[0];
                                    var random5 = Random((119 - FFrame) / 3);
                                    var path4 = FPath[0];
                                    var random6 = Random((119 - FFrame) / 3);
                                    AoWCombatMap.Current.ChangeViewTo(path4.X + random6 - (119 - FFrame) / 6, path3.Y + random5 - (119 - FFrame) / 6, AoWCombatMap.Current.ViewL);
                                    break;
                                case 120:
                                    FFrame = -1;
                                    break;
                            }

                            break;
                        case TfxType.Cloud:
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
                        case TfxType.CloudPuff:
                            if (FAbilityIndex == 52)
                            {
                                Sequence = AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastImages.ImageSequences[0];
                            }
                            else
                            {
                                Sequence = AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FImageSequences.ImageSequences[0];
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
                        case TfxType.HighPrayer:
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
                                        int last = AoWCombatMap.Current.FCombat.FCombatData.GetCount() - 1;
                                        for (; I <= last; I += 1)
                                        {
                                            if (AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled() && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).FCombatPlayer == FTarget.FCombatPlayer)
                                            {
                                                Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                                                var yhx = ((TacticalCombatUnit)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
                                                HexGeometry.HXtoHP(((TacticalCombatUnit)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx(), yhx, ref J, ref K);
                                                J = Random(20) + (J + 6);
                                                K = Random(20) + (K + 6);
                                                EffectPaths.MakeLinePath(J, K - 100, J, K, 50, ref Effect.FPath);
                                                Effect.FAbilityIndex = FAbilityIndex;
                                                Effect.FSpellIndex = FSpellIndex;
                                            }
                                        }
                                    }

                                    break;
                                case 100:
                                    {
                                        I = 0;
                                        int last = AoWCombatMap.Current.FCombat.FCombatData.GetCount() - 1;
                                        for (; I <= last; I += 1)
                                        {
                                            if (AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled() && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).FCombatPlayer == FTarget.FCombatPlayer)
                                            {
                                                Effect = AoWCombatMap.Current.AddEffect(TfxType.PrayerGlow);
                                                var yhx = ((TacticalCombatUnit)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
                                                HexGeometry.HXtoHP(((TacticalCombatUnit)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx(), yhx, ref J, ref K);
                                                var xyz = new XYZ();
                                                Effect.FPath.Add(xyz);
                                                Effect.FPath[0].X = J + 16;
                                                Effect.FPath[0].Y = K + 16;
                                                Effect.FAbilityIndex = FAbilityIndex;
                                                Effect.FSpellIndex = FSpellIndex;
                                                var combatObject = AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I);
                                                Effect.FTarget = combatObject;
                                            }
                                        }
                                    }

                                    FFrame = -1;
                                    FTarget = null;
                                    break;
                            }

                            break;
                        case TfxType.Resurrect:
                            switch (FFrame)
                            {
                                case 100:
                                    FFrame = -1;
                                    break;
                            }

                            break;
                        case TfxType.DecayLeg:
                            var count24 = FPath.Count;
                            if (FFrame >= count24)
                            {
                                FFrame = -1;
                            }

                            break;
                        case TfxType.Geyser:
                            switch (FFrame)
                            {
                                case 150:
                                    FFrame = -1;
                                    if (FTarget != null && FTarget.GetEnabled())
                                    {
                                        if (AoWCombatMap.Current.FPlaySounds)
                                        {
                                            if (FAbilityIndex == 52)
                                            {
                                                AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(1);
                                            }
                                            else
                                            {
                                                AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(1);
                                            }
                                        }

                                        ((TacticalCombatUnit)FTarget).FUnitHS.FOffsetY = 0;
                                        ((TacticalCombatUnit)FTarget).FUnitHS.FBusy = false;
                                    }

                                    if (FTarget != null && ((CombatUnit)FTarget).FUnit.GetMoveTypes().Contains(MoveType.Flying))
                                    {
                                        FTarget = null;
                                    }

                                    break;
                                case >= 0 and <= 99:
                                    if (AoWCombatMap.Current.FPlaySounds && FFrame % 30 == 29)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            AoWHSSet.Current.FSpellControl.GetSpell(FSpellIndex).FCastSounds.Play(0);
                                        }
                                        else
                                        {
                                            AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityIndex).FSFXLibrary.Play(0);
                                        }
                                    }

                                    if (FTarget != null && FTarget.GetEnabled())
                                    {
                                        var random = Random(2);
                                        ((TacticalCombatUnit)FTarget).FUnitHS.FOffsetY = random + -(FFrame * 3 / 2) - 1;
                                        ((TacticalCombatUnit)FTarget).FUnitHS.FBusy = true;
                                    }

                                    J = Random(6) + 20;
                                    {
                                        I = 1;
                                        int last = J - 1;
                                        for (; I <= last; I += 1)
                                        {
                                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            Speed = (float)((double)(Random(10) + 10) / 10);
                                            Gravity = (float)0.5;
                                            VZ = (float)((double)((double)Random(50) + (double)(FFrame * 3) * 0.5) / 10);
                                            Z = 0;
                                            var path = FPath[0];
                                            EffectPaths.MakeArcPath(FPath[0].X, path.Y, I * 360 / J, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                        }
                                    }

                                    break;
                                case >= 125 and <= 149:
                                    if (FTarget != null && FTarget.GetEnabled())
                                    {
                                        ((TacticalCombatUnit)FTarget).FUnitHS.FOffsetY = (FFrame - 150) * 6;
                                        ((TacticalCombatUnit)FTarget).FUnitHS.SetDirection((byte)(FFrame % 6 + 1));
                                    }

                                    break;
                            }

                            break;
                        case TfxType.SWComet:
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

    public partial class CItemHS : ItemHS
    {

        public CItemHS(ECustomNode owner) : base(owner)
        { }

        // AoWTC.pas:2586
        public override void Deactivate()
        {
            ItemHS ItemHS;
            ECustomNode Items;
            FItems.FOnChanged = null;
            if (AoWCombatMap.Current.FCombat.FExplorationSite == 1)
            {
                Items = (ECustomNode)AoWCombatMap.Current.FCombat.FItems;
                for (int I = FItems.GetCount() - 1; I >= 0; I--)
                {
                    FItems.GetItem(I).SetOwner(Items);
                }
            }
            else
            {
                ItemHS = (ItemHS)AoWHSMap.Current.MapContainer.GetField(AoWCombatMap.Current.FCombat.FCombatData.FX, AoWCombatMap.Current.FCombat.FCombatData.FY, AoWCombatMap.Current.FCombat.FCombatData.FL).FindOwnedChild(131697);
                if (ItemHS == null)
                {
                    ItemHS = new ItemHS(null);
                    AoWHSMap.Current.PlaceHX(ItemHS, AoWCombatMap.Current.FCombat.FCombatData.FX, AoWCombatMap.Current.FCombat.FCombatData.FY, AoWCombatMap.Current.FCombat.FCombatData.FL);
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
    public partial class UnitPositionHS : SingleHS
    {
        public UnitPositionHS(ECustomNode owner) : base(owner)
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
        public CustomImageLibrary FImages;
        public byte FDirection;

        // AoWTC.pas:5060
        public override void ReadWrite(EStorageStream Stream)
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
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
            FImages[FImageIndex].ShowClipped(X - XhpSurf - 8, Y - YhpSurf - 8, SurfaceDesc);
        }

        // AoWTC.pas:5031
        public override void Activate()
        {
            XYL P = default;
            base.Activate();
            if (AoWCombatMap.Current.Status.Contains(HSMapStatusElement.Editting) && AoWCombatMap.Current.FUnitPositions.ValidatePosition(ref FPosition, ref FIndex))
            {
                var yhx = GetYhx();
                var packXYL = HexGeometry.PackXYL(GetXhx(), yhx, 0);
                AoWCombatMap.Current.FUnitPositions.SetUnitPosition(FPosition, FIndex, packXYL);
                FImages = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TCUNITS.ILB");
                UpdateImageIndex();
            }
            else
            {
                P.X = GetXhx();
                P.Y = GetYhx();
                P.L = 0;
                P.Flag = (sbyte)FDirection;
                AoWCombatMap.Current.FUnitPositions.SetUnitPosition(FPosition, FIndex, P);
                Dispose();
            }
        }

        // AoWTC.pas:5049
        public override void Deactivate()
        {
            if (Activated() && ((MapLevel)Owner.Owner).Map.Status.Contains(HSMapStatusElement.Editting))
            {
                AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TCUNITS.ILB");
                var unitPosition = AoWCombatMap.Current.FUnitPositions.GetUnitPosition(FPosition, FIndex);
                var xhx = GetXhx();
                bool condition = unitPosition.X == xhx;
                if (condition)
                {
                    var fUnitPositionsUnitPosition = AoWCombatMap.Current.FUnitPositions.GetUnitPosition(FPosition, FIndex);
                    var yhx = GetYhx();
                    condition = fUnitPositionsUnitPosition.Y == yhx;
                }

                if (condition)
                {
                    var packXYL = HexGeometry.PackXYL(-1, -1, -1);
                    AoWCombatMap.Current.FUnitPositions.SetUnitPosition(FPosition, FIndex, packXYL);
                }
            }

            base.Deactivate();
        }

        // AoWTC.pas:5068
        public override bool CanPlace(MapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx) => Map.GetField(Xhx, Yhx, Lhx).FindOwnedChild(2228483) == null && AoWCombatMap.Current.FUnitPositions.ValidatePosition(ref FPosition, ref FIndex);

        // AoWTC.pas:5026
        public void UpdateImageIndex()
        {
            FImageIndex = FPosition * 8 + FIndex;
        }

        // AoWTC.pas:223

    }

    // FFrame. FOffsetX. FOffsetY
    public partial class CBloodHS : SingleHS
    {
        public CBloodHS(ECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 2228487;

        public int FFrame;
        public int FOffsetX;
        public int FOffsetY;

        // AoWTC.pas:6393
        public override byte GetLevel() => 26;

        // AoWTC.pas:6398
        public override int GetShowPriority() => 1744830464 + (FOffsetY << 13) + FOffsetX;

        // AoWTC.pas:6403
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int Frame;
            ImageSequence Sequence;
            int X = default;
            int Y = default;
            Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[4];
            HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
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
    public partial class CCorpseHS : SingleHS
    {
        public CCorpseHS(ECustomNode owner) : base(owner)
        { }

        public new const int ClassID = 2228488;

        public TacticalCombatUnit FUnit;
        public UnitGFXResource FUnitGFX;
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
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int Frame;
            int X = default;
            int Y = default;
            ImageSequence Sequence;
            HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
            XhpSurf = X - XhpSurf - 15;
            YhpSurf = Y - YhpSurf - 25;
            if (FSequenceIndex > -1)
            {
                Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[FSequenceIndex];
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
        public override void MapFieldMsgProc(ref MapFieldMsg Msg)
        {
            CombatAction AnimateAction;
            CombatAction RaiseAction;
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
            TacticalCombatUnitHS UnitHS;
            base.MapFieldMsgProc(ref Msg);
            if (FUnit != null)
            {
                if (Msg.MsgID == 200712 && Msg.HN == 0)
                {
                    var yhx = GetYhx();
                    var xhx = GetXhx();
                    if (AoWCombatMap.Current.MapContainer.MapLevels[0].Field[xhx, yhx].FindOwnedChild(2228484) == null)
                    {
                        var yhx2 = GetYhx();
                        var xhx2 = GetXhx();
                        AoWCombatMap.Current.MakeVisible(xhx2, yhx2, 0, 5);
                        var yhx3 = GetYhx();
                        HexGeometry.HXtoHP(GetXhx(), yhx3, ref X, ref Y);
                        for (int Ring = 1; Ring <= 3; Ring++)
                        {
                            N = Random(5);
                            DX = Random(20) + Ring * 30;
                            DY = Random(20) + Ring * 30;
                            for (int I = 0; I <= 17; I++)
                            {
                                Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
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
                                EffectPaths.MakeLinePath(Round((double)Math.Cos((double)((double)(I * 5 + N) * mathPI) / 180) * DX) + (X + 16), Round((double)sin * DY) / 2 + (Y + 16), X + 16, Y + 16, 100, ref Effect.FPath);
                            }
                        }

                        return;
                    }
                }

                if (Msg.MsgID == 200713 && Msg.HN == 0)
                {
                    var yhx = GetYhx();
                    var xhx = GetXhx();
                    if (AoWCombatMap.Current.MapContainer.MapLevels[0].Field[xhx, yhx].FindOwnedChild(2228484) == null)
                    {
                        var yhx2 = GetYhx();
                        HexGeometry.HXtoHP(GetXhx(), yhx2, ref X, ref Y);
                        N = Random(6) + 25;
                        for (int I = 1; I <= N - 1; I++)
                        {
                            Effect = AoWCombatMap.Current.AddEffect(TfxType.GeyserWater);
                            Effect.FAbilityIndex = 52;
                            Effect.FSpellIndex = Msg.Param[1].Int;
                            // { Original uses this smaller value for horizontal speed. }
                            Speed = (float)((double)(Random(10) + 10) * 0.05);
                            Gravity = (float)0.5;
                            VZ = (float)((double)(Random(50) + 50) * 0.05);
                            Z = 0;
                            EffectPaths.MakeArcPath(X + 16, Y + 16, I * 360 / N, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                        }

                        switch (Msg.Param[1].Int)
                        {
                            case 130:
                                RandSeed = AoWHSMap.Current.Random(16777215);
                                AnimateAction = AoWHSSet.Current.FSpellControl.GetSpell(130).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                AoWCombatMap.Current.FCombat.ExecuteCombatAction(AnimateAction);
                                AnimateAction.Release();
                                break;
                            case 129:
                                RandSeed = AoWHSMap.Current.Random(16777215);
                                RaiseAction = AoWHSSet.Current.FSpellControl.GetSpell(129).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                AoWCombatMap.Current.FCombat.ExecuteCombatAction(RaiseAction);
                                RaiseAction.Release();
                                break;
                        }

                        if (FUnit.GetEnabled())
                        {
                            UnitHS = new TacticalCombatUnitHS(null);
                            UnitHS.Initialize(FUnit);
                            UnitHS.SetDirection((byte)(Random(6) + 1));
                            var yhx3 = GetYhx();
                            var xhx2 = GetXhx();
                            AoWCombatMap.Current.PlaceHX(UnitHS, xhx2, yhx3, 0);
                        }

                        this.Dispose();
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
                AoWCombatMap.Current.NewFrameEvents += NewFrame;
            }

            FUnitGFX.AddRef();
            FSequenceFrame = -1;
        }

        // AoWTC.pas:3966
        public override void Deactivate()
        {
            AoWCombatMap.Current.NewFrameEvents -= NewFrame;
            FUnitGFX.Release();
            base.Deactivate();
        }

        // AoWTC.pas:3978
        public void NewFrame(object Sender)
        {
            ImageSequence Sequence;
            Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[FSequenceIndex];
            var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
            if (FSequenceFrame + 1 < totalSequenceFrames)
            {
                FSequenceFrame += 1;
            }
        }

        // Engine.pas:524

    }

    // Inherited single-hexagon sprite layout
    public partial class COozeHS : SingleHS
    {
        public COozeHS(ECustomNode owner) : base(owner)
        { }

        // AoWTC.pas:4092
        public new const int ClassID = 2228489;

        // AoWTC.pas:4097
        public override sbyte GetOverlay(int HN) => 11;

        // AoWTC.pas:4121
        public override byte GetLevel() => 26;

        // AoWTC.pas:4126
        public override int GetShowPriority() => 1744830464 + (GetYhx() << 13) + GetXhx();

        // AoWTC.pas:4102
        public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
        {
            int Frame;
            int X = default;
            int Y = default;
            ImageSequence Sequence;
            HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
            XhpSurf = X - XhpSurf - 16;
            YhpSurf = Y - 48 - YhpSurf;
            switch (((MapField)Owner).TerrainType)
            {
                case 3:
                case 6:
                case 13:
                    Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[26];
                    break;
                case 12:
                    Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[27];
                    break;
                default:
                    Sequence = AoWHSSet.Current.FImageSequences.ImageSequences[25];
                    break;
            }

            while (Sequence != null)
            {
                var totalSequenceFrames = Sequence.GetTotalSequenceFrames();
                Frame = Sequence.GetUnloopedFrameIndex(FrameFixtureInputs.PhaseModulo(this, totalSequenceFrames));
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
            ((MapField)Owner).UpdateTerrain();
            return Result;
        }

        // Engine.pas:524

    }
}

namespace Aow.Game.Combat
{

    // FPositions: packed XYL entries, eight per army position
    public partial class UnitPositionControl : EObject
    {
        public XYLList FPositions;

        // AoWTC.pas:4943
        public override void Dispose()
        {
            FPositions?.Dispose();
            FPositions = null;
            base.Dispose();
        }

        // AoWTC.pas:4936
        public UnitPositionControl(ECustomNode owner) : base(owner)
        {
            FPositions = new XYLList();
            var packXYL = HexGeometry.PackXYL(-1, -1, -1);
            FPositions.Default = packXYL;
        }

        // AoWTC.pas:4990
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            UnitPositionHS HS;
            int X = default;
            int Y = default;
            int L;
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 65541)
            {
                L = Msg.Data[1];
                HexGeometry.HPtoHX(Msg.Payload.Int, Msg.Data[0], ref X, ref Y);
                if (AoWCombatMap.Current.MapContainer.GetField(X, Y, L).TerrainType != 15)
                {
                    HS = (UnitPositionHS)AoWCombatMap.Current.MapContainer.GetField(X, Y, L).FindOwnedChild(2228483);
                    if (HS == null)
                    {
                        HS = new UnitPositionHS(null);
                        if (!AoWCombatMap.Current.PlaceHX(HS, X, Y, L))
                        {
                            HS?.Dispose();
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
        public XYL GetUnitPosition(int Position, int Index)
        {
            XYL Result = default;
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
        public void SetUnitPosition(int Position, int Index, XYL Value)
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
    public partial class CombatMoveControlSettings : MoveControlSettings
    {
        public CombatMoveControlSettings(ECustomNode owner) : base(owner)
        { }

        public Set<CombatMoveModeElement> FMoveFlags;
        // intf_P5_01.inc:63

    }

    public partial class CombatMoveControl : MoveControl
    {

        public CombatMoveControl(HSMap owner) : base(owner)
        { }

        public TacticalCombatUnitHS FUnitHS;
        public Set<CombatMoveModeElement> FMoveFlags;

        // AoWTC.pas:2716
        public override void ProcessMove(MoveControlSettings Settings)
        {
            FUnitHS = (TacticalCombatUnitHS)Settings.FArmyHS;
            FMoveFlags = ((CombatMoveControlSettings)Settings).FMoveFlags;
            base.ProcessMove(Settings);
        }

        // intf_P5_01.inc:120

    }

    // FMovePoints. FUnitHS. FPositionIndex. FPositionGroup. FCanFlee. FDeathMethod
    public partial class TacticalCombatUnit : CombatUnit
    {
        public TacticalCombatUnit(ECustomNode owner) : base(owner)
        { }

        public int FMovePoints;
        public TacticalCombatUnitHS FUnitHS;
        public byte FPositionIndex;
        public byte FPositionGroup;
        public bool FCanFlee;
        public DeathMethod FDeathMethod;

        // AoWTC.pas:2626
        public override void MsgProc(ref EMsg Msg, EObject Sender)
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
            CCorpseHS Corpse;
            ImageSequence Sequence;
            TfxHS Effect;
            float Speed = default;
            float VZ = default;
            float Gravity = default;
            float Z = default;
            int X = default;
            int Y = default;
            Rect R;
            void MakeBlood()
            {
                int Count;
                int X = default;
                int Y = default;
                int Direction;
                CBloodHS Blood;
                if (GetEnabled() && FUnitHS != null && FUnit.GetBloodType() != BloodType.None)
                {
                    var hitPoints = GetHitPoints();
                    if (Damage >= hitPoints && (GetAbilityEnabled(131) || FDeathMethod == DeathMethod.Explode))
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
                        HexGeometry.CenterHNtoHX(FUnitHS.GetXhx(), yhx, Direction, ref X, ref Y);
                        if (AoWCombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].TerrainType != 0 && AoWCombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].TerrainType != 9)
                        {
                            Blood = new CBloodHS(AoWCombatMap.Current.MapContainer.MapLevels[0].Field[X, Y]);
                            Blood.Activate();
                            Blood.FOffsetX = Random(32) - 32;
                            Blood.FOffsetY = Random(32) - 32;
                            Blood.FFrame = Random(AoWHSSet.Current.FImageSequences.ImageSequences[4].GetTotalSequenceFrames());
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
                            Effect = AoWCombatMap.Current.AddEffect(TfxType.DecayComet);
                            Effect.FAbilityIndex = 52;
                            Effect.FSpellIndex = 106;
                            var yhx = FUnitHS.GetYhx();
                            HexGeometry.HXtoHP(FUnitHS.GetXhx(), yhx, ref X, ref Y);
                            var xyz = new XYZ();
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
                                EffectPaths.MakeArcPath(pathX.X, path.Y - pathZ.Z, random, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                            }
                        }
                    }
                    else
                    {
                        var yhx = FUnitHS.GetYhx();
                        var xhx = FUnitHS.GetXhx();
                        bool condition = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[xhx, yhx].TerrainType != 0;
                        if (condition)
                        {
                            var fUnitHSYhx = FUnitHS.GetYhx();
                            var fUnitHSXhx = FUnitHS.GetXhx();
                            condition = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[fUnitHSXhx, fUnitHSYhx].TerrainType != 9;
                        }

                        if (condition)
                        {
                            var fUnitHSYhx = FUnitHS.GetYhx();
                            var fUnitHSXhx = FUnitHS.GetXhx();
                            Corpse = new CCorpseHS(AoWCombatMap.Current.MapContainer.MapLevels[0].Field[fUnitHSXhx, fUnitHSYhx]);
                            Corpse.FUnitGFX = FUnitHS.FUnitGFXResource;
                            Sequence = Corpse.FUnitGFX.FSequences.ImageSequences[30];
                            if (Sequence != null)
                            {
                                Corpse.FFrame = Random(Sequence.GetTotalSequenceFrames());
                            }

                            switch (FDeathMethod)
                            {
                                case DeathMethod.Normal:
                                    switch (FUnit.GetBloodType())
                                    {
                                        case BloodType.None:
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
                                case DeathMethod.Explode:
                                    Corpse.FSequenceIndex = -1;
                                    break;
                            }

                            Corpse.FOffsetX = Random(10) - 5;
                            Corpse.FOffsetY = Random(10) - 5;
                            if (FUnit.GetUnitType() != UnitType.Machine)
                            {
                                Corpse.FUnit = this;
                            }

                            Corpse.Activate();
                        }
                    }

                    var yhx2 = FUnitHS.GetYhx();
                    var xhx2 = FUnitHS.GetXhx();
                    var yhx3 = FUnitHS.GetYhx();
                    R = new Rect(FUnitHS.GetXhx(), yhx3, xhx2 + 1, yhx2 + 1);
                    FUnitHS?.Dispose();
                    FUnitHS = null;
                    AoWCombatMap.Current.TriggerMapChangedEvent(R, 0);
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
            if (SupportCombatMode(Set<CombatMode>.Of(CombatMode.MediumRange)))
            {
                Result = Result - 100;
            }

            if (SupportCombatMode(Set<CombatMode>.Of(CombatMode.LongRange)))
            {
                Result = Result - 200;
            }

            return Result;
        }

        // AoWTC.pas:2618
        public void MovedTo(AoWMapField OldField, AoWMapField NewField, int MoveCost)
        {
            FMovePoints = FMovePoints - MoveCost;
            if (FMovePoints < 0)
            {
                FMovePoints = 0;
            }

            if (GetAbilityEnabled(43))
            {
                var yhx = FUnitHS.GetYhx();
                EffectPaths.CreateFire(false, 50, FUnitHS.GetXhx(), yhx);
            }

            AoWCombatMap.Current.FUnitChangedEventsStore.Raise(FUnitHS);
        }

        internal static void MakeHitBlood(TacticalCombatUnit UnitObject, int Damage)
        {
            TfxHS Effect;
            XYZ Point;
            Effect = AoWCombatMap.Current.AddEffect(TfxType.Blood);
            Point = new XYZ();
            Effect.FPath.Add(Point);
            switch (Damage)
            {
                case >= 1 and <= 3:
                    Point.Z = 1;
                    break;
                case >= 4 and <= 7:
                    Point.Z = 2;
                    break;
                case >= 8 and <= 100:
                    Point.Z = 3;
                    break;
            }

            switch (UnitObject.FUnit.GetBloodType())
            {
                case BloodType.None:
                    Point.Z = Point.Z + 5;
                    break;
            }

            var yhx = UnitObject.FUnitHS.Yhx;
            HexGeometry.HXtoHP(UnitObject.FUnitHS.Xhx, yhx, ref Point.X, ref Point.Y);
            Point.X = Random(10) + Point.X + 11;
            var unitSize = UnitObject.FUnit.GetUnitSize();
            Point.Y = Random(10) + Point.Y + 9 - (int)unitSize * 10;
        }

        internal static CombatMoveTE CreateCombatMoveTE(TacticalCombatUnit UnitObject, Movepath Path)
        {
            CombatMoveTE Result = new CombatMoveTE(null);
            if (Result != null)
            {
                Result.Setup(UnitObject, Path);
            }

            return Result;
        }

        internal static bool MoveCombatUnit(TacticalCombatUnit UnitObject, Movepath Path)
        {
            int MovePoints;
            int Cost;
            int Index;
            CombatMoveTE Event;
            Movepath NewPath;
            bool Result = false;
            var player = UnitObject.GetPlayer();
            if (AoWHSMap.Current.FTokenManager.PlayerReady(player) && Path.Count > 1)
            {
                MovePoints = UnitObject.FMovePoints;
                Cost = 0;
                Index = Path.Count;
                while (Index != 0 && Cost <= MovePoints)
                {
                    Index = Index - 1;
                    Cost = Cost + Path[Index].Flag;
                }

                if (Cost > MovePoints)
                {
                    Index = Index + 1;
                }

                if (Index < Path.Count - 1)
                {
                    NewPath = new Movepath();
                    for (int I = Index, last = Path.Count - 1; I <= last; I++)
                    {
                        NewPath.Add(Path[I]);
                    }

                    Event = CreateCombatMoveTE(UnitObject, NewPath);
                    if (Event != null)
                    {
                        var unitObjectPlayer = UnitObject.GetPlayer();
                        AoWHSMap.Current.FTokenManager.GetTokenControl(unitObjectPlayer).AddEvent(Event);
                        Event.Release();
                        Result = true;
                    }

                    NewPath?.Dispose();
                }
            }

            return Result;
        }

        public enum DeathMethod : byte
        {
            Normal = 0, Explode = 1
        }

        // intf_P3_01.inc:244

        // intf_P3_01.inc:244

    }
}

namespace Aow.Game.Draw
{

    // FUnit. FMoveIndicator. FIndicatorStep. FAction. FActionDone. FActionFrame. FOffsetX.
    // FOffsetY. FSelectedAbility. FSelectedSpell. FCombatAbilityCount. FAbilities. FBusy
    public partial class TacticalCombatUnitHS : UnitHS
    {
        public TacticalCombatUnitHS(ECustomNode owner) : base(owner)
        { }

        // AoWTC.pas:3348
        public new const int ClassID = 2228484;

        public TacticalCombatUnit FUnit;
        public short FMoveIndicator;
        public short FIndicatorStep;
        public SpriteAction FAction;
        public bool FActionDone;
        public short FActionFrame;
        public int FOffsetX;
        public int FOffsetY;
        public int FSelectedAbility;
        public int FSelectedSpell;
        public int FCombatAbilityCount;
        public DelphiList FAbilities;
        public bool FBusy;

        // AoWTC.pas:3566
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID != 536936471 && (Msg.MsgID & 1073741824) == 0 && FUnit != null)
            {
                FUnit.MsgProc(ref Msg, Sender);
            }
        }

        // AoWTC.pas:3461
        public override void MapFieldMsgProc(ref MapFieldMsg Msg)
        {
            SelfDestructCA SelfDestruct;
            DamageCA SpellDamage;
            RangedAttackCA RangedDamage;
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4364)
            {
                if (Msg.HN == 0 && (!AoWCombatMap.Current.FFogEnabled || Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((AoWMapField)Owner).FFieldFlags).Hi).Lo != 0))
                {
                    if (FUnit.GetPlayer() != 0)
                    {
                        var player = FUnit.GetPlayer();
                        var playerPlayerRace = AoWHSMap.Current.FPlayers.GetPlayers(player);
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
                        RandSeed = AoWHSMap.Current.Random(16777215);
                        SelfDestruct = ((SelfDestructAbility)AoWHSSet.Current.FAbilityControl.GetAbility(65)).CreateSelfDestructCA(Msg.Param[0].As<CombatObject>(), FUnit);
                        if (SelfDestruct.FDamage > 0)
                        {
                            SetAction(SpriteAction.Hit);
                        }

                        AoWCombatMap.Current.FCombat.ExecuteCombatAction(SelfDestruct);
                        SelfDestruct.Release();
                    }
                    else
                    {
                        if (Msg.MsgID == 200707 && Msg.HN == 0)
                        {
                            RandSeed = AoWHSMap.Current.Random(16777215);
                            SpellDamage = (DamageCA)AoWHSSet.Current.FSpellControl.GetSpell(117).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                            if (SpellDamage.FDamage > 0)
                            {
                                SetAction(SpriteAction.Hit);
                            }

                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                            SpellDamage.Release();
                        }
                        else
                        {
                            if (Msg.MsgID == 200715 && Msg.HN == 0)
                            {
                                RandSeed = AoWHSMap.Current.Random(16777215);
                                SpellDamage = (DamageCA)AoWHSSet.Current.FSpellControl.GetSpell(113).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                if (SpellDamage.FDamage > 0)
                                {
                                    SetAction(SpriteAction.Hit);
                                }

                                AoWCombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                                SpellDamage.Release();
                            }
                            else
                            {
                                if (Msg.MsgID == 200714 && Msg.HN == 0)
                                {
                                    RandSeed = AoWHSMap.Current.Random(16777215);
                                    RangedDamage = ((RangedAttackAbility)AoWHSSet.Current.FAbilityControl.GetAbility(31)).CreateRangedAttackCA(Msg.Param[0].As<CombatObject>(), FUnit, 0);
                                    if (RangedDamage.FDamage > 0)
                                    {
                                        SetAction(SpriteAction.Hit);
                                    }

                                    AoWCombatMap.Current.FCombat.ExecuteCombatAction(RangedDamage);
                                    RangedDamage.Release();
                                }
                                else
                                {
                                    if (Msg.MsgID == 200723 && Msg.HN == 0)
                                    {
                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                        SpellDamage = (DamageCA)AoWHSSet.Current.FSpellControl.GetSpell(Msg.Param[1].Int).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                        if (SpellDamage.FDamage > 0)
                                        {
                                            SetAction(SpriteAction.Hit);
                                        }

                                        AoWCombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                                        SpellDamage.Release();
                                    }
                                    else
                                    {
                                        if (Msg.MsgID == 200722 && Msg.HN == 0)
                                        {
                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                            RangedDamage = ((RangedAttackAbility)AoWHSSet.Current.FAbilityControl.GetAbility(Msg.Param[1].Int)).CreateRangedAttackCA(Msg.Param[0].As<CombatObject>(), FUnit, 0);
                                            if (RangedDamage.FDamage > 0)
                                            {
                                                SetAction(SpriteAction.Hit);
                                            }

                                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(RangedDamage);
                                            RangedDamage.Release();
                                        }
                                        else
                                        {
                                            if (Msg.MsgID == 200709 && Msg.HN == 0)
                                            {
                                                RandSeed = AoWHSMap.Current.Random(16777215);
                                                SpellDamage = (DamageCA)AoWHSSet.Current.FSpellControl.GetSpell(104).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                                if (SpellDamage.FDamage > 0)
                                                {
                                                    SetAction(SpriteAction.Hit);
                                                }

                                                AoWCombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                                                SpellDamage.Release();
                                            }
                                            else
                                            {
                                                if (Msg.MsgID == 200716 && Msg.HN == 0)
                                                {
                                                    RandSeed = AoWHSMap.Current.Random(16777215);
                                                    SpellDamage = (DamageCA)AoWHSSet.Current.FSpellControl.GetSpell(120).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                                    if (SpellDamage.FDamage > 0)
                                                    {
                                                        SetAction(SpriteAction.Hit);
                                                    }

                                                    AoWCombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                                                    SpellDamage.Release();
                                                }
                                                else
                                                {
                                                    if (Msg.MsgID == 200710 && Msg.HN == 0)
                                                    {
                                                        RandSeed = AoWHSMap.Current.Random(16777215);
                                                        SpellDamage = (DamageCA)AoWHSSet.Current.FSpellControl.GetSpell(116).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                                        if (SpellDamage.FDamage > 0)
                                                        {
                                                            SetAction(SpriteAction.Hit);
                                                        }

                                                        AoWCombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                                                        SpellDamage.Release();
                                                    }
                                                    else
                                                    {
                                                        if (Msg.MsgID == 200717 && Msg.HN == 0)
                                                        {
                                                            RandSeed = AoWHSMap.Current.Random(16777215);
                                                            SpellDamage = (DamageCA)AoWHSSet.Current.FSpellControl.GetSpell(127).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                                            if (SpellDamage.FDamage > 0)
                                                            {
                                                                SetAction(SpriteAction.Hit);
                                                            }

                                                            AoWCombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
                                                            SpellDamage.Release();
                                                        }
                                                        else
                                                        {
                                                            if (Msg.MsgID == 200718 && Msg.HN == 0)
                                                            {
                                                                RandSeed = AoWHSMap.Current.Random(16777215);
                                                                SpellDamage = (DamageCA)AoWHSSet.Current.FSpellControl.GetSpell(128).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                                                if (SpellDamage.FDamage > 0)
                                                                {
                                                                    SetAction(SpriteAction.Hit);
                                                                }

                                                                AoWCombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
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
                                                                                if (!FUnit.FUnit.GetMoveTypes().Contains(MoveType.Flying) && ((MapField)Owner).TerrainType != 0 && ((MapField)Owner).TerrainType != 9)
                                                                                {
                                                                                    RandSeed = AoWHSMap.Current.Random(16777215);
                                                                                    SpellDamage = (DamageCA)AoWHSSet.Current.FSpellControl.GetSpell(112).CreateCA(Msg.Param[0].As<CombatObject>(), FUnit, false);
                                                                                    if (SpellDamage.FDamage > 0)
                                                                                    {
                                                                                        SetAction(SpriteAction.Hit);
                                                                                    }

                                                                                    AoWCombatMap.Current.FCombat.ExecuteCombatAction(SpellDamage);
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
        public override void CanMoveOn(in HSMoveOnInfo Info, ref HSMoveOnResult MoveResult)
        {
            int X = default;
            int Y = default;
            MovementHS Movement;
            AbilityCombatInfo AbilityInfo = default;
            AbilityCombatInfo EnemyInfo = default;
            if (Info.HN == 0)
            {
                MoveResult.Code = -1;
                MoveResult.Done = true;
                var yhx = GetYhx();
                var xhx = GetXhx();
                Movement = (MovementHS)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[xhx, yhx].FindOwnedChild(2228490);
                if (Movement != null && Movement.FEnabled)
                {
                    if (((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide() == FUnit.GetSide())
                    {
                        MoveResult.Code = 0;
                        MoveResult.Done = false;
                        return;
                    }
                }

                if (GetXhx() != ((MoveControl)Info.Sender).FDestinationX && ((MoveControl)Info.Sender).FDestinationX != -1)
                {
                    return;
                }

                if (GetYhx() != ((MoveControl)Info.Sender).FDestinationY && ((MoveControl)Info.Sender).FDestinationY != -1)
                {
                    return;
                }

                if (!((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Move) && !((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.MoveRanged) && AoWCombatMap.Current.FUnitSelection.FMoveSettingsList.GetCount() >= 2)
                {
                    return;
                }

                if (((CombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility <= -1 && (CombatObject.MeleeDamageTypes(((CombatMoveControl)Info.Sender).FUnitHS.FUnit) == Set<DamageType>.Of() || FUnit.GetAbilityEnabled(1) && !((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1)) && !((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.MoveRanged) && !((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Move))
                {
                    return;
                }

                if (((CombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility == -1)
                {
                    if (!((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.Move) && !((CombatMoveControl)Info.Sender).FMoveFlags.Contains(CombatMoveModeElement.MoveRanged))
                    {
                        if (FUnit.GetSide() == ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide())
                        {
                            return;
                        }
                    }

                    MoveResult.Code = 0;
                    return;
                }

                AoWHSSet.Current.FAbilityControl.GetAbility(((CombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility).GetCombatInfo(((CombatMoveControl)Info.Sender).FUnitHS.FUnit.FUnit, ref AbilityInfo);
                if (AbilityInfo.TargetTypes.Contains(CombatTargetTypeElement.Friend) && FUnit.GetSide() == ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide())
                {
                    goto Touch;
                }

                AoWHSSet.Current.FAbilityControl.GetAbility(((CombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility).GetCombatInfo(((CombatMoveControl)Info.Sender).FUnitHS.FUnit.FUnit, ref EnemyInfo);
                if (!EnemyInfo.TargetTypes.Contains(CombatTargetTypeElement.Enemy))
                {
                    return;
                }

                if (FUnit.GetSide() == ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide())
                {
                    return;
                }

            Touch:
                if (((TouchAbility)AoWHSSet.Current.FAbilityControl.GetAbility(((CombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility)).CanTouch(((CombatMoveControl)Info.Sender).FUnitHS.FUnit, FUnit))
                {
                    MoveResult.Code = 0;
                }
            }
            else
            {
                var yhx = GetYhx();
                HexGeometry.CenterHNtoHX(GetXhx(), yhx, Info.HN, ref X, ref Y);
                if (MoveResult.Code < 30)
                {
                    MoveResult.Code = ((MoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][Info.MapField.OverlayType - -1];
                }

                if (MoveResult.Code <= -1)
                {
                    return;
                }

                if (Info.HN >= 7)
                {
                    return;
                }

                if (GetXhx() == ((MoveControl)Info.Sender).FDestinationX && GetYhx() == ((MoveControl)Info.Sender).FDestinationY)
                {
                    return;
                }

                if (FUnit.GetSide() == ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide())
                {
                    return;
                }

                if (CombatObject.MeleeDamageTypes(FUnit) == Set<DamageType>.Of())
                {
                    return;
                }

                if (FUnit.GetLocked())
                {
                    return;
                }

                if (FUnit.GetAbilityEnabled(1) != ((CombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1))
                {
                    return;
                }

                if (((MoveControl)Info.Sender).FDestinationX >= 0 && ((MoveControl)Info.Sender).FDestinationY >= 0 && HexGeometry.dHXtoRad(X, Y, ((MoveControl)Info.Sender).FDestinationX, ((MoveControl)Info.Sender).FDestinationY) <= 1)
                {
                    if (AoWCombatMap.Current.MapContainer.MapLevels[0].Field[((MoveControl)Info.Sender).FDestinationX, ((MoveControl)Info.Sender).FDestinationY].FindOwnedChild(2228484) != null)
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
            FAbilities = new DelphiList();
            CreateTCAbList();
            AoWCombatMap.Current.NewFrameEvents += NewFrame;
            if (AoWCombatMap.Current.FExplorationEnabled)
            {
                var player = FUnit.GetPlayer();
                var visibilityRange = FUnit.GetVisibilityRange();
                var yhx = GetYhx();
                var xhx = GetXhx();
                AoWCombatMap.Current.ExploreArea(xhx, yhx, 0, player, visibilityRange);
            }

            FActionFrame = -1;
            FSelectedAbility = -1;
            FSelectedSpell = -1;
        }

        // AoWTC.pas:3339
        public override void Deactivate()
        {
            if (AoWCombatMap.Current != null)
            {
                AoWCombatMap.Current.NewFrameEvents -= NewFrame;
            }

            for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
            {
                (FAbilities[I] as IDisposable)?.Dispose();
            }

            FAbilities?.Dispose();
            base.Deactivate();
        }

        // AoWTC.pas:3439
        public override void UpdateMapField(in HSMapFieldInfo Info)
        {
            if (Info.HN == 0)
            {
                ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 16384 | 8192 | 33554432;
            }
            else
            {
                if (Info.HN <= 6)
                {
                    ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 16384;
                }
            }
        }

        // AoWTC.pas:3784
        public override void Select()
        {
            base.Select();
            CreateTCAbList();
            FSelectedAbility = -1;
            if (AoWCombatMap.Current.FUnitSelection.GetTCUnitHS() != this)
            {
                AoWCombatMap.Current.FUnitSelection.SetTCUnitHS(this);
            }

            FUnitGFXResource.FSFXLibraries[0].Play(0);
            if (AoWCombatMap.Current.FSeatedCombatPlayer == FUnit.FCombatPlayer)
            {
                AoWCombatMap.Current.FPlayers.GetPlayers(AoWCombatMap.Current.FSeatedCombatPlayer).FLastUnit = FUnit;
            }
        }

        // AoWTC.pas:3796
        public override void Unselect()
        {
            base.Unselect();
            if (AoWCombatMap.Current != null)
            {
                if (AoWCombatMap.Current.FUnitSelection.GetTCUnitHS() == this)
                {
                    AoWCombatMap.Current.FUnitSelection.Unselect();
                }
            }
        }

        // AoWTC.pas:3357
        public override int GetMoveSteps()
        {
            int Result;
            switch (GameTables.AoWEngine.FRegistry.GetMovementSpeed())
            {
                case AoWRegistry.MovementSpeed.Fast:
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
                case AoWRegistry.MovementSpeed.Turbo:
                    Result = 2;
                    break;
                default:
                    Result = base.GetMoveSteps();
                    break;
            }

            return Result;
        }

        // AoWTC.pas:3351
        public void Initialize(TacticalCombatUnit U)
        {
            FUnit = U;
            FUnit.FUnitHS = this;
        }

        // AoWTC.pas:3804
        public bool MeleeMoveTC(Movepath Path)
        {
            CMeleeMoveTE TE;
            bool Result = false;
            var player = FUnit.GetPlayer();
            if (AoWHSMap.Current.FTokenManager.PlayerReady(player) && Path.Count > 1)
            {
                TE = CreateTCMeleeMoveTE(FUnit, Path);
                if (TE != null)
                {
                    var fUnitPlayer = FUnit.GetPlayer();
                    AoWHSMap.Current.FTokenManager.GetTokenControl(fUnitPlayer).AddEvent(TE);
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
            AoWCombatMap.Current.FUnitSelection.DeactivateRangedPath();
            FSelectedAbility = -1;
            int Result = -1;
            Index = -1;
            if (AoWCombatMap.Current.FCurrentCombatPlayer == AoWCombatMap.Current.FSeatedCombatPlayer)
            {
                for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
                {
                    if (((CombatAbilityRef)FAbilities[I]).Ability == AbilityID)
                    {
                        Index = I;
                    }
                }

                if (!FUnit.GetLocked() && FUnit.FMovePoints > 0 && FUnit.FCombatPlayer == AoWCombatMap.Current.FCurrentCombatPlayer && Index < FCombatAbilityCount && Index > -1)
                {
                    if (AbilityID == 52)
                    {
                        if (FUnit.FUnit.GetMoves() == FUnit.FMovePoints)
                        {
                            AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).ActivateCombat(FUnit.FUnit, ref Reason);
                            FSelectedSpell = -1;
                            FSelectedAbility = AbilityID;
                            Result = Index;
                        }
                    }
                    else
                    {
                        var combatMode = AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetCombatMode(FUnit.FUnit);
                        if (Set<CombatMode>.Of(CombatMode.ShortRange, CombatMode.MediumRange, CombatMode.LongRange).Contains(combatMode))
                        {
                            if (FUnit.FUnit.GetMoves() == FUnit.FMovePoints)
                            {
                                if (AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetCombatMode(FUnit.FUnit) == CombatMode.ShortRange)
                                {
                                    AoWCombatMap.Current.FUnitSelection.FRangedPathHS.FRange = 4;
                                }
                                else
                                {
                                    if (AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).GetCombatMode(FUnit.FUnit) == CombatMode.MediumRange)
                                    {
                                        AoWCombatMap.Current.FUnitSelection.FRangedPathHS.FRange = 8;
                                    }
                                    else
                                    {
                                        AoWCombatMap.Current.FUnitSelection.FRangedPathHS.FRange = 12;
                                    }
                                }

                                AoWCombatMap.Current.FUnitSelection.FRangedPathHS.FSelection = AoWCombatMap.Current.FUnitSelection;
                                AoWCombatMap.Current.DynamicFixedVisibleHS.AddChild(AoWCombatMap.Current.FUnitSelection.FRangedPathHS);
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

            for (int I = 0, last = AoWCombatMap.Current.FUnitSelection.FMoveSettingsList.GetCount() - 1; I <= last; I++)
            {
                var moveSettings = AoWCombatMap.Current.FUnitSelection.FMoveSettingsList.GetMoveSettings(I);
                moveSettings.FPath.Clear();
            }

            AoWCombatMap.Current.FUnitSelection.FRangedPath.Clear();
            AoWCombatMap.Current.FUnitSelection.UpdateMoveCursor();
            AoWCombatMap.Current.FUnitChangedEventsStore.Raise(this);
            return Result;
        }

        // AoWTC.pas:3870
        public int SelectSpell(int SpellID)
        {
            SpellCombatInfo Info = default;
            AoWCombatMap.Current.FUnitSelection.DeactivateRangedPath();
            FSelectedSpell = SpellID;
            AoWHSSet.Current.FSpellControl.GetSpell(FSelectedSpell).GetCombatInfo(FUnit.FUnit, ref Info);
            if (Info.Range != 0)
            {
                if (Info.Range == -1)
                {
                    AoWCombatMap.Current.FUnitSelection.FRangedPathHS.FRange = 100;
                }
                else
                {
                    AoWCombatMap.Current.FUnitSelection.FRangedPathHS.FRange = (ushort)Info.Range;
                }

                AoWCombatMap.Current.FUnitSelection.FRangedPathHS.FSelection = AoWCombatMap.Current.FUnitSelection;
                AoWCombatMap.Current.DynamicFixedVisibleHS.AddChild(AoWCombatMap.Current.FUnitSelection.FRangedPathHS);
            }

            int Result = SpellID;
            for (int I = 0, last = AoWCombatMap.Current.FUnitSelection.FMoveSettingsList.GetCount() - 1; I <= last; I++)
            {
                var moveSettings = AoWCombatMap.Current.FUnitSelection.FMoveSettingsList.GetMoveSettings(I);
                moveSettings.FPath.Clear();
            }

            AoWCombatMap.Current.FUnitSelection.FRangedPath.Clear();
            switch (TacticalTables.SpellTypes[SpellID])
            {
                case TfxType.ScreenNESW:
                case TfxType.SacredWrath:
                case TfxType.Tremors:
                case TfxType.HighPrayer:
                case TfxType.ShockWave:
                    AbRangedTC(0, 0);
                    Result = -1;
                    Unselect();
                    break;
            }

            AoWCombatMap.Current.FUnitSelection.UpdateMoveCursor();
            AoWCombatMap.Current.FUnitChangedEventsStore.Raise(this);
            return Result;
        }

        // AoWTC.pas:3895
        public void CreateTCAbList()
        {
            CombatAbilityRef Ability;
            for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
            {
                (FAbilities[I] as IDisposable)?.Dispose();
            }

            FAbilities.Clear();
            FCombatAbilityCount = 0;
            for (int I = 1; I <= 169; I++)
            {
                if (FUnit.GetAbilityEnabled(I))
                {
                    Ability = new CombatAbilityRef();
                    Ability.Ability = I;
                    if (AoWHSSet.Current.FAbilityControl.GetAbility(I).GetControlType(FUnit.FUnit).Contains(AbilityControlTypeElement.CombatAction) && AoWHSSet.Current.FAbilityControl.GetAbility(I).GetCombatMode(FUnit.FUnit) != CombatMode.Melee)
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
        public void ListAbilities(StringCollection Names, StringCollection Types, StringCollection Sources)
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
                Names.AddObject(AoWHSSet.Current.FAbilityControl.GetAbility(((CombatAbilityRef)ability).Ability).GetName(FUnit.FUnit), IntToObject(((CombatAbilityRef)FAbilities[I]).Ability));
                if (Types != null)
                {
                    var ability2 = FAbilities[I];
                    var abilityType = AoWHSSet.Current.FAbilityControl.GetAbility(((CombatAbilityRef)ability2).Ability).GetAbilityType();
                    Types.Add(AbilityRules.TranslateRStr(GameTables.TAbilityTypeRStr[(int)abilityType]));
                }

                if (Sources != null)
                {
                    var ability2 = FAbilities[I];
                    Sources.Add(AoWHSSet.Current.FAbilityControl.GetAbility(((CombatAbilityRef)ability2).Ability).GetSourceName(FUnit.FUnit));
                }
            }
        }

        // AoWTC.pas:3923
        public void SetAction(SpriteAction Action)
        {
            FActionDone = false;
            switch (Action)
            {
                case SpriteAction.StepUp:
                case SpriteAction.StepBack:
                    FActionFrame = 0;
                    break;
                case SpriteAction.RoundAttack:
                case SpriteAction.RoundAttackHit:
                    if (FAction != SpriteAction.RoundAttack && FAction != SpriteAction.RoundAttackHit)
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
        public bool AbTouchMoveTC(Movepath Path)
        {
            TokenEvent TE;
            AbilityCombatInfo Buffer = default;
            bool Result = false;
            var player = FUnit.GetPlayer();
            if (AoWHSMap.Current.FTokenManager.PlayerReady(player) && Path.Count > 0)
            {
                if (Path.Count > 1)
                {
                    TE = CreateTCAbTouchMoveTE(FUnit, Path, FSelectedAbility);
                    if (TE != null)
                    {
                        var fUnitPlayer = FUnit.GetPlayer();
                        AoWHSMap.Current.FTokenManager.GetTokenControl(fUnitPlayer).AddEvent(TE);
                        TE.Release();
                        Result = true;
                    }
                }
                else
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(FSelectedAbility).GetCombatInfo(FUnit.FUnit, ref Buffer);
                    if (Buffer.TargetTypes.Contains(CombatTargetTypeElement.Friend))
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
            CAbRangedTE TE;
            bool Result = false;
            var player = FUnit.GetPlayer();
            if (AoWHSMap.Current.FTokenManager.PlayerReady(player))
            {
                TE = CreateTCAbRangedTE(FUnit, X, Y, FSelectedAbility, FSelectedSpell);
                if (TE != null)
                {
                    var fUnitPlayer = FUnit.GetPlayer();
                    AoWHSMap.Current.FTokenManager.GetTokenControl(fUnitPlayer).AddEvent(TE);
                    TE.Release();
                    Result = true;
                }
            }

            return Result;
        }

        private static FixedArray_int_14 StatusAbils = FixedArray_int_14.From([149, 150, 48, 92, 93, 94, 95, 96, 97, 98, 107, 163, 127, 34]);

        public static void OriginalUnitAssert(string Message, string Filename, int LineNumber)
        {
            RtlUnit.Assert(false, Message, Filename, LineNumber);
        }

        private static CMeleeMoveTE CreateTCMeleeMoveTE(TacticalCombatUnit UnitObject, Movepath Path)
        {
            CMeleeMoveTE tCMeleeMoveTE = new CMeleeMoveTE(null);
            tCMeleeMoveTE?.Setup(UnitObject, Path);
            return tCMeleeMoveTE;
        }

        private static CAbTouchMoveTE CreateTCAbTouchMoveTE(TacticalCombatUnit UnitObject, Movepath Path, int AbilityID)
        {
            CAbTouchMoveTE tCAbTouchMoveTE = new CAbTouchMoveTE(null);
            if (tCAbTouchMoveTE != null)
            {
                tCAbTouchMoveTE.Setup(UnitObject, Path);
                tCAbTouchMoveTE.FAbilityID = AbilityID;
            }
            return tCAbTouchMoveTE;
        }

        private static CAbRangedTE CreateTCAbRangedTE(TacticalCombatUnit UnitObject, ushort X, ushort Y, int AbilityID, int SpellID)
        {
            CAbRangedTE tCAbRangedTE = new CAbRangedTE(null);
            if (tCAbRangedTE != null)
            {
                tCAbRangedTE.FSourceX = (ushort)UnitObject.FUnitHS.Xhx;
                tCAbRangedTE.FSourceY = (ushort)UnitObject.FUnitHS.Yhx;
                tCAbRangedTE.FTargetX = X;
                tCAbRangedTE.FTargetY = Y;
                tCAbRangedTE.FAbilityID = AbilityID;
                tCAbRangedTE.FSpellID = SpellID;
            }
            return tCAbRangedTE;
        }

        public enum SpriteAction : byte
        {
            Move = 0, MeleeHit = 1, MeleeMiss = 2, Ability = 3, Hit = 4, Killed = 5, StepUp = 6, StepBack = 7, Busy = 8, RoundAttack = 9, RoundAttackHit = 10, FreeStruck = 11, BreathAttack = 12
        }

        // intf_P2_03.inc:74

        // intf_P2_03.inc:74

    }
}

namespace Aow.Game.Combat
{

    // FPathInvalid. FTargetImages. FRangedPathHS. FTargetX. FTargetY. FCursorImage. FCursorFrame.
    // FRangedPath. FObstacles
    public partial class CombatUnitSelectionControl : MoveSelector
    {
        // AoWTC.pas:2723
        public CombatUnitSelectionControl() : base()
        {
            FRangedPath = new XYZList();
            FObstacles = new DelphiList();
            FRangedPathHS = new RangedPathHS(null);
            FRangedPathHS.Activate();
            FTargetImages = AoWHSSet.Current.ImageLibraryManager.LinkToIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TargHex.ILB");
        }

        public Set<AnonymousEnum_piMove> FPathInvalid;
        public CustomImageLibrary FTargetImages;
        public RangedPathHS FRangedPathHS;
        public int FTargetX;
        public int FTargetY;
        public ushort FCursorImage;
        public ushort FCursorFrame;
        public HexagonSprite FMouseOverTarget;
        public XYZList FRangedPath;
        public DelphiList FObstacles;

        // AoWTC.pas:2733
        public override void Dispose()
        {
            for (int I = 0, last = FObstacles.Count - 1; I <= last; I++)
            {
                (FObstacles[I] as IDisposable)?.Dispose();
            }

            FObstacles?.Dispose();
            FRangedPath?.Dispose();
            FRangedPathHS?.Dispose();
            AoWHSSet.Current.ImageLibraryManager.ReleaseIL(Set<ILManagerCategoryElement>.Of(ILManagerCategoryElement.Disk), "TargHex.ILB");
            base.Dispose();
        }

        // AoWTC.pas:3080
        public override void MouseDownEvent(object Sender)
        {
            TacticalCombatUnitHS UnitHS;
            int Index;
            int DX;
            int DY;
            Set<CombatTargetTypeElement> Targets;
            SpellCombatInfo SpellInfo = default;
            AbilityCombatInfo AbilityInfo = default;
            if (AoWCombatMap.Current.FSeatedCombatPlayer == AoWCombatMap.Current.FCurrentCombatPlayer)
            {
                if (!AoWCombatMap.Current.FPlayers.GetPlayers(AoWCombatMap.Current.FCurrentCombatPlayer).FAuto)
                {
                    if (AoWCombatMap.Current.Mouse.Button == MouseButton.Right)
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
                        if (AoWCombatMap.Current.Mouse.Shift.Contains(ShiftState.Shift))
                        {
                            if (FMoveSettingsList.GetCount() == 1 && FMoveSettingsList.GetMoveSettings(0).FPath.Count != 0)
                            {
                                var pathL = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                var pathY = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                var pathX = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                if (!ValidMultiMoveSelection((AoWMapField)AoWCombatMap.Current.MapContainer.GetField(pathX.X, pathY.Y, pathL.L)))
                                {
                                    return;
                                }
                            }

                            if (AoWCombatMap.Current.Mouse.Xhx != -1 && AoWCombatMap.Current.Mouse.Yhx != -1)
                            {
                                var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                var xhx = AoWCombatMap.Current.Mouse.Xhx;
                                UnitHS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL).FindOwnedChild(2228484);
                                if (UnitHS != null && UnitHS.FUnit.FCombatPlayer == AoWCombatMap.Current.FSeatedCombatPlayer)
                                {
                                    Index = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                    if (Index == -1)
                                    {
                                        AddUnit(UnitHS);
                                        Update();
                                        AoWCombatMap.Current.Selection.SelectLocationHS(UnitHS);
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
                            if (AoWCombatMap.Current.Mouse.Shift.Contains(ShiftState.Ctrl))
                            {
                                if (FMoveSettingsList.GetCount() > 0 && AoWCombatMap.Current.Mouse.Xhx != -1 && AoWCombatMap.Current.Mouse.Yhx != -1)
                                {
                                    var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                    var xhx = AoWCombatMap.Current.Mouse.Xhx;
                                    UnitHS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL).FindOwnedChild(2228484);
                                    if (UnitHS != null && UnitHS.FUnit.FCombatPlayer == AoWCombatMap.Current.FSeatedCombatPlayer)
                                    {
                                        Index = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                        if (Index == -1)
                                        {
                                            AddUnit(UnitHS);
                                            Update();
                                            AoWCombatMap.Current.Selection.SelectLocationHS(UnitHS);
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
                                if (AoWCombatMap.Current.Mouse.Xhx != -1 && AoWCombatMap.Current.Mouse.Yhx != -1)
                                {
                                    if (GetTCUnitHS() != null)
                                    {
                                        var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                        var xhx = AoWCombatMap.Current.Mouse.Xhx;
                                        UnitHS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL).FindOwnedChild(2228484);
                                        if (GetTCUnitHS().FSelectedAbility == -1)
                                        {
                                            Targets = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
                                        }
                                        else
                                        {
                                            if (GetTCUnitHS().FSelectedAbility == 52)
                                            {
                                                var tcUnitHS = GetTCUnitHS();
                                                var tcUnitHS2 = GetTCUnitHS();
                                                AoWHSSet.Current.FSpellControl.GetSpell(tcUnitHS2.FSelectedSpell).GetCombatInfo(tcUnitHS.FUnit.FUnit, ref SpellInfo);
                                                Targets = SpellInfo.TargetTypes;
                                            }
                                            else
                                            {
                                                var tcUnitHS = GetTCUnitHS();
                                                var tcUnitHS2 = GetTCUnitHS();
                                                AoWHSSet.Current.FAbilityControl.GetAbility(tcUnitHS2.FSelectedAbility).GetCombatInfo(tcUnitHS.FUnit.FUnit, ref AbilityInfo);
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
                                            if (UnitHS.FUnit.GetPlayer() == player && !Targets.Contains(CombatTargetTypeElement.Friend))
                                            {
                                                AoWCombatMap.Current.Selection.UnselectLocation();
                                                AoWCombatMap.Current.Selection.SelectLocationHS(UnitHS);
                                            }
                                            else
                                            {
                                                var fUnitPlayer = GetTCUnitHS().FUnit.GetPlayer();
                                                bool condition = UnitHS.FUnit.GetPlayer() == fUnitPlayer && Targets.Contains(CombatTargetTypeElement.Friend);
                                                if (!condition)
                                                {
                                                    var player2 = GetTCUnitHS().FUnit.GetPlayer();
                                                    condition = UnitHS.FUnit.GetPlayer() != player2 && Targets.Contains(CombatTargetTypeElement.Enemy);
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
                var yhx = AoWCombatMap.Current.Mouse.Yhx;
                var xhx = AoWCombatMap.Current.Mouse.Xhx;
                UnitHS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL).FindOwnedChild(2228484);
                AoWCombatMap.Current.Selection.UnselectLocation();
                if (UnitHS != null && AoWCombatMap.Current.Mouse.Button == MouseButton.Left)
                {
                    AoWCombatMap.Current.Selection.SelectLocationHS(UnitHS);
                }
            }
        }

        // AoWTC.pas:2823
        public override void CalculateMovePath(int Index, sbyte X, sbyte Y, sbyte L)
        {
            TacticalCombatUnitHS UnitHS;
            DelphiList Temporary;
            MovementHS Movement;
            var moveSettings = FMoveSettingsList.GetMoveSettings(Index);
            Temporary = new DelphiList();
            if (FMoveSettingsList.GetCount() > 0)
            {
                for (int I = 0, last = FMoveSettingsList.GetCount() - 1; I <= last; I++)
                {
                    if (I != Index)
                    {
                        if (FMoveSettingsList.GetMoveSettings(I).FPath.Count > 1)
                        {
                            Movement = new MovementHS(null);
                            var moveSettingPath = FMoveSettingsList.GetMoveSettings(I);
                            var path = FMoveSettingsList.GetMoveSettings(I).FPath[moveSettingPath.FPath.Count - 1];
                            var moveSetting = FMoveSettingsList.GetMoveSettings(I);
                            var pathX = FMoveSettingsList.GetMoveSettings(I).FPath[moveSetting.FPath.Count - 1];
                            AoWCombatMap.Current.PlaceHX(Movement, pathX.X, path.Y, 0);
                            Movement.FEnabled = true;
                            Temporary.Add(Movement);
                            Movement = new MovementHS(null);
                            var pathY = FMoveSettingsList.GetMoveSettings(I).FPath[0];
                            var path2 = FMoveSettingsList.GetMoveSettings(I).FPath[0];
                            AoWCombatMap.Current.PlaceHX(Movement, path2.X, pathY.Y, 0);
                            Movement.FEnabled = false;
                            Temporary.Add(Movement);
                        }
                    }
                }
            }

            moveSettings.FPathMovePoints = 0;
            UnitHS = (TacticalCombatUnitHS)moveSettings.FMoveObject;
            if (GetTCUnitHS() != null && (GetTCUnitHS().FSelectedAbility == 117 || GetTCUnitHS().FSelectedAbility == 65))
            {
                ((CombatMoveControlSettings)FMoveControlSettings).FMoveFlags = Set<CombatMoveModeElement>.FromMask((byte)FPathInvalid.Image) + Set<CombatMoveModeElement>.Of(CombatMoveModeElement.Climbing);
            }
            else
            {
                ((CombatMoveControlSettings)FMoveControlSettings).FMoveFlags = Set<CombatMoveModeElement>.FromMask((byte)FPathInvalid.Image);
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
            AoWCombatMap.Current.FMoveControl.ProcessMove(FMoveControlSettings);
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
                    bool condition = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y].FindOwnedChild(2229508) != null;
                    if (!condition)
                    {
                        var path = FMoveControlSettings.FPath[I];
                        var path2 = FMoveControlSettings.FPath[I];
                        condition = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path2.X, path.Y].FindOwnedChild(2229510) != null;
                    }

                    if (condition)
                    {
                        var path = FMoveControlSettings.FPath[I];
                        var path2 = FMoveControlSettings.FPath[I];
                        var mapField = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path2.X, path.Y];
                        moveSettings.FPath.List[I].Flag = FMoveControlSettings.FMovePointTable[mapField.TerrainType][-1 - -1];
                    }
                    else
                    {
                        var path = FMoveControlSettings.FPath[I];
                        var path2 = FMoveControlSettings.FPath[I];
                        var item = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path2.X, path.Y];
                        var path3 = FMoveControlSettings.FPath[I];
                        var path4 = FMoveControlSettings.FPath[I];
                        var fieldItem = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path4.X, path3.Y];
                        moveSettings.FPath.List[I].Flag = FMoveControlSettings.FMovePointTable[fieldItem.TerrainType][item.OverlayType - -1];
                    }
                }

                moveSettings.FPathMovePoints = moveSettings.FPath.PosToMovePoints(0);
                FMoveControlSettings.FPath.Clear();
            }

            for (int I = 0, last = Temporary.Count - 1; I <= last; I++)
            {
                (Temporary[I] as IDisposable)?.Dispose();
            }

            Temporary?.Dispose();
        }

        // AoWTC.pas:3178
        public override void UpdateMoveCursor()
        {
            AoWMapField Field;
            int Cost;
            TacticalCombatUnitHS UnitHS;
            TacticalCombatUnitHS TargetHS;
            Set<CombatTargetTypeElement> Targets;
            CityWall Wall;
            SpellCombatInfo SpellInfo = default;
            AbilityCombatInfo AbilityInfo = default;
            if (AoWCombatMap.Current == null)
            {
                return;
            }

            UnitHS = GetTCUnitHS();
            AoWCombatMap.Current.Mouse.CursorMode = 0;
            if (AoWCombatMap.Current.Mouse.Xhx == -1)
            {
                return;
            }

            if (AoWCombatMap.Current.Mouse.Yhx == -1)
            {
                return;
            }

            if (UnitHS == null)
            {
                return;
            }

            var yhx = AoWCombatMap.Current.Mouse.Yhx;
            var xhx = AoWCombatMap.Current.Mouse.Xhx;
            Field = (AoWMapField)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL);
            if (AoWCombatMap.Current.FExplorationEnabled && !Field.GetExplored(AoWCombatMap.Current.FSeatedPlayer))
            {
                goto FinishCursor;
            }

            TargetHS = (TacticalCombatUnitHS)Field.FindOwnedChild(2228484);
            if (UnitHS.FUnit.FCombatPlayer != AoWCombatMap.Current.FSeatedCombatPlayer && TargetHS != null)
            {
                AoWCombatMap.Current.Mouse.CursorMode = 257;
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
                    AoWHSSet.Current.FSpellControl.GetSpell(UnitHS.FSelectedSpell).GetCombatInfo(UnitHS.FUnit.FUnit, ref SpellInfo);
                    Targets = SpellInfo.TargetTypes;
                }
                else
                {
                    AoWHSSet.Current.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatInfo(UnitHS.FUnit.FUnit, ref AbilityInfo);
                    Targets = AbilityInfo.TargetTypes;
                }

                if (TargetHS == null || (TargetHS == UnitHS && !Targets.Contains(CombatTargetTypeElement.Friend)))
                {
                    if (FCursorImage < 85 || FCursorImage > 88)
                    {
                        FCursorImage = 85;
                    }
                }
                else
                {
                    if (AoWHSSet.Current.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatMode(UnitHS.FUnit.FUnit) == CombatMode.Touch)
                    {
                        if ((Targets.Contains(CombatTargetTypeElement.Friend) && UnitHS.FUnit.GetSide() == TargetHS.FUnit.GetSide() || Targets.Contains(CombatTargetTypeElement.Enemy) && UnitHS.FUnit.GetSide() != TargetHS.FUnit.GetSide()) && ((TouchAbility)AoWHSSet.Current.FAbilityControl.GetAbility(UnitHS.FSelectedAbility)).CanTouch(UnitHS.FUnit, TargetHS.FUnit))
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
                        if (Targets.Contains(CombatTargetTypeElement.Friend) && UnitHS.FUnit.GetSide() == TargetHS.FUnit.GetSide() || Targets.Contains(CombatTargetTypeElement.Enemy) && UnitHS.FUnit.GetSide() != TargetHS.FUnit.GetSide())
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

                AoWCombatMap.Current.Mouse.CursorMode = 256;
                AoWCombatMap.Current.Mouse.Cursor = FCursorImage;
            }
            else
            {
                if (TargetHS != null && TargetHS != UnitHS)
                {
                    if (TargetHS.FUnit.GetPlayer() == UnitHS.FUnit.GetPlayer())
                    {
                        AoWCombatMap.Current.Mouse.CursorMode = 257;
                    }
                    else
                    {
                        if (UnitHS.FUnit.GetAbilityEnabled(1) || !TargetHS.FUnit.GetAbilityEnabled(1))
                        {
                            AoWCombatMap.Current.Mouse.CursorMode = 258;
                        }
                        else
                        {
                            FCursorImage = 89;
                            AoWCombatMap.Current.Mouse.CursorMode = 256;
                            AoWCombatMap.Current.Mouse.Cursor = FCursorImage;
                        }
                    }
                }
            }

        FinishCursor:
            if (AoWCombatMap.Current.Mouse.CursorMode != 0)
            {
                return;
            }

            if (UnitHS.FUnit.GetPlayer() == AoWCombatMap.Current.FSeatedPlayer)
            {
                if (GetMoveTypes().Contains(MoveType.Walking))
                {
                    AoWCombatMap.Current.Mouse.Cursor = 64;
                }
                else
                {
                    if (GetMoveTypes().Contains(MoveType.Swimming))
                    {
                        AoWCombatMap.Current.Mouse.Cursor = 66;
                    }
                    else
                    {
                        if (GetMoveTypes().Contains(MoveType.Flying))
                        {
                            AoWCombatMap.Current.Mouse.Cursor = 68;
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
                    Wall = (CityWall)Field.FindOwnedChild(2229508);
                    if (Wall == null)
                    {
                        Wall = (CityWall)Field.FindOwnedChild(2229510);
                    }

                    if (Wall != null)
                    {
                        if (!(Wall is CityWallDoor) || UnitHS.FUnit.GetSide() == CombatSideIndex.Attacker)
                        {
                            Cost = -1;
                        }

                        if (!(Wall is CityWallDoor) && UnitHS.FUnit.FUnit.GetAbilityEnabled(36))
                        {
                            Cost = 1;
                        }

                        if (UnitHS.FUnit.FUnit.GetAbilityEnabled(66) || UnitHS.FUnit.FUnit.GetAbilityEnabled(1))
                        {
                            Cost = 1;
                        }
                    }
                }

                if (Cost <= 0 || AoWCombatMap.Current.FExplorationEnabled && !Field.GetExplored(AoWCombatMap.Current.FSeatedPlayer))
                {
                    AoWCombatMap.Current.Mouse.Cursor = AoWCombatMap.Current.Mouse.Cursor + 1;
                }

                AoWCombatMap.Current.Mouse.CursorMode = 256;
            }
        }

        // AoWTC.pas:2744
        public override MoveControlSettings CreateMCS() => new CombatMoveControlSettings(null);

        // AoWTC.pas:2754
        public override void SetCurrentMove(int Value)
        {
            if (FCurrentMove != -1 && GetTCUnitHS() != null)
            {
                var tcUnitHS = GetTCUnitHS();
                AoWCombatMap.Current.Selection.TriggerDeselectEvent(tcUnitHS);
            }

            base.SetCurrentMove(Value);
            if (FCurrentMove != -1 && GetTCUnitHS() != null)
            {
                var tcUnitHS = GetTCUnitHS();
                AoWCombatMap.Current.Selection.TriggerSelectEvent(tcUnitHS);
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

            AoWCombatMap.Current.Selection.UnselectLocation();
            DeactivateMoveSelection();
            base.Unselect();
        }

        // AoWTC.pas:3274
        public override void ShowMovePaths(in SurfaceDesc Surface)
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
                        HexGeometry.CenterHNtoHX(FMoveSettingsList.GetMoveSettings(0).FPath[1].X, path.Y, I, ref X, ref Y);
                        HexGeometry.HXtoHP(X, Y, ref Xhp, ref Yhp);
                        FTargetImages[0].Show(Xhp - AoWCombatMap.Current.ViewXhp - 8, Yhp - AoWCombatMap.Current.ViewYhp + 2, Surface);
                    }
                }
            }
        }

        // AoWTC.pas:2747
        public int AddUnit(TacticalCombatUnitHS U)
        {
            int Result = AddMoveObject(U);
            FMoveSettingsList.GetMoveSettings(Result).FMovePoints = U.FUnit.FMovePoints;
            var moves = U.FUnit.GetMoves();
            FMoveSettingsList.GetMoveSettings(Result).FMoves = moves;
            return Result;
        }

        // AoWTC.pas:2763
        public TacticalCombatUnitHS GetTCUnitHS()
        {
            TacticalCombatUnitHS Result;
            if (FCurrentMove != -1)
            {
                Result = (TacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(FCurrentMove).FMoveObject;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // AoWTC.pas:2770
        public Set<MoveType> GetMoveTypes()
        {
            Set<MoveType> Result;
            if (FCurrentMove != -1)
            {
                Result = ((TacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(FCurrentMove).FMoveObject).FUnit.FUnit.GetMoveTypes();
            }
            else
            {
                Result = Set<MoveType>.Of();
            }

            return Result;
        }

        // AoWTC.pas:2777
        public void RectangleSelectEvent(AbstractAoWHSMapMouse Sender)
        {
            int X1 = default;
            int Y1 = default;
            int X2 = default;
            int Y2 = default;
            int PX = default;
            int PY = default;
            TacticalCombatUnitHS UnitHS;
            MapField Field;
            MapLevel Level;
            DelphiList Units;
            Rect R;
            Units = new DelphiList();
            R = Sender.FSelectionRectangle;
            Level = AoWCombatMap.Current.MapContainer.MapLevels[AoWCombatMap.Current.ViewL];
            HexGeometry.HPtoHX(R.Left, R.Top, ref X1, ref Y1);
            HexGeometry.HPtoHX(R.Right, R.Bottom, ref X2, ref Y2);
            for (int X = X1 - 1, last = X2 + 1; X <= last; X++)
            {
                for (int Y = Y1 - 1, lastY = Y2 + 1; Y <= lastY; Y++)
                {
                    HexGeometry.HXtoHP(X, Y, ref PX, ref PY);
                    PX += 16;
                    PY += 8;
                    if (X >= 0 && X < Level.Width && Y > 0 && Y < Level.Height && PX >= R.Left && PX < R.Right && PY >= R.Top && PY < R.Bottom)
                    {
                        Field = Level.Field[X, Y];
                        UnitHS = (TacticalCombatUnitHS)Field.FindOwnedChild(2228484);
                        if (UnitHS != null && UnitHS.FUnit.FCombatPlayer == AoWCombatMap.Current.FSeatedCombatPlayer)
                        {
                            Units.Add(UnitHS);
                        }
                    }
                }
            }

            if (Units.Count != 0)
            {
                Clear();
                AoWCombatMap.Current.Selection.UnselectLocation();
                for (int I = 0, last = Units.Count - 1; I <= last; I++)
                {
                    AddUnit((TacticalCombatUnitHS)Units[I]);
                    var unit = Units[I];
                    AoWCombatMap.Current.Selection.SelectLocationHS((HexagonSprite)unit);
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

            Units?.Dispose();
        }

        // AoWTC.pas:2807
        public void MoveSelectionEvent()
        {
            if (AoWCombatMap.Current.FSeatedCombatPlayer == AoWCombatMap.Current.FCurrentCombatPlayer)
            {
                if (AoWCombatMap.Current.Mouse.Capture != this)
                {
                    AoWCombatMap.Current.Selection.UnselectLocation();
                }
                else
                {
                    if (AoWCombatMap.Current.Mouse.Button == MouseButton.Right)
                    {
                        if (FRangedPath.Count == 0)
                        {
                            AoWCombatMap.Current.Selection.UnselectLocation();
                        }
                        else
                        {
                            FRangedPath.Clear();
                            FMouseOverTarget = null;
                            AoWCombatMap.Current.FMouseOverTargetEventsStore.Raise(FMouseOverTarget);
                        }
                    }
                    else
                    {
                        if (AoWCombatMap.Current.Mouse.Capture == this && AoWCombatMap.Current.Mouse.Button == MouseButton.Left)
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
            TacticalCombatUnitHS UnitHS;
            ushort Range;
            SpellCombatInfo Info = default;
            int Direction;
            int NX;
            int NY;
            FRangedPath.Clear();
            Direction = 0;
            int last = FObstacles.Count - 1;
            for (; Direction <= last; Direction += 1)
            {
                (FObstacles[Direction] as IDisposable)?.Dispose();
            }

            FObstacles.Clear();
            UnitHS = GetTCUnitHS();
            if (AoWCombatMap.Current.MapContainer.MapLevels[0].Field[X, Y].TerrainType != 15)
            {
                if (UnitHS.FSelectedAbility == 52)
                {
                    AoWHSSet.Current.FSpellControl.GetSpell(UnitHS.FSelectedSpell).GetCombatInfo(UnitHS.FUnit.FUnit, ref Info);
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
                    if (AoWHSSet.Current.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatMode(tcUnitHS.FUnit.FUnit) == CombatMode.LongRange)
                    {
                        Range = 12;
                    }
                    else
                    {
                        var tcUnitHS2 = GetTCUnitHS();
                        if (AoWHSSet.Current.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatMode(tcUnitHS2.FUnit.FUnit) == CombatMode.MediumRange)
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
                    condition = HexGeometry.dHXtoRad(X, Y, UnitHS.GetXhx(), yhx) <= Range;
                }

                if (condition)
                {
                    // { Original retains this unused with temporary. }
                    GetTCUnitHS();
                    FTargetX = X;
                    FTargetY = Y;
                    var yhx = UnitHS.GetYhx();
                    Direction = HexGeometry.dHXtoHN(UnitHS.GetXhx(), yhx, X, Y);
                    NX = X;
                    NY = Y;
                    switch (GetTCUnitHS().FSelectedAbility)
                    {
                        case 30:
                        case >= 86 and <= 90:
                            var unitHSYhx = UnitHS.GetYhx();
                            HexGeometry.CenterHNtoHX(UnitHS.GetXhx(), unitHSYhx, TacticalTables.BreathHN[TacticalTables.BreathDirection[Direction - 1] * 12 - 12], ref NX, ref NY);
                            break;
                        case 52:
                            if (TacticalTables.SpellTypes[UnitHS.FSelectedSpell] == TfxType.Breath)
                            {
                                var yhx2 = UnitHS.GetYhx();
                                HexGeometry.CenterHNtoHX(UnitHS.GetXhx(), yhx2, TacticalTables.BreathHN[TacticalTables.BreathDirection[Direction - 1] * 12 - 12], ref NX, ref NY);
                            }

                            break;
                    }

                    var xhx = GetTCUnitHS().GetXhx();
                    var yhx3 = GetTCUnitHS().GetYhx();
                    var side = GetTCUnitHS().FUnit.GetSide();
                    var tcUnitHS = GetTCUnitHS();
                    TacticalTables.MakeAbilPath(side, (short)GetTCUnitHS().FSelectedAbility, (short)tcUnitHS.FSelectedSpell, xhx, yhx3, (short)NX, (short)NY, ref FRangedPath, ref FObstacles);
                }
            }

            FRangedPathHS.FSelection = this;
            AoWCombatMap.Current.DynamicFixedVisibleHS.AddChild(FRangedPathHS);
        }

        // AoWTC.pas:2918
        public void MoveSelectedRoute()
        {
            TacticalCombatUnitHS UnitHS;
            EObject Target;
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
                            Target = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y].FindOwnedChild(2228484);
                        }
                        else
                        {
                            var pathY = moveSettings.FPath[0];
                            var pathX = moveSettings.FPath[0];
                            Target = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[pathX.X, pathY.Y].FindOwnedChild(2229508);
                            if (Target == null)
                            {
                                var path = moveSettings.FPath[0];
                                var path2 = moveSettings.FPath[0];
                                Target = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[path2.X, path.Y].FindOwnedChild(2229510);
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
                                    if (AoWHSSet.Current.FAbilityControl.GetAbility(UnitHS.FSelectedAbility).GetCombatMode(UnitHS.FUnit.FUnit) == CombatMode.Touch)
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
                            if (TacticalCombatUnit.MoveCombatUnit(UnitHS.FUnit, moveSettings.FPath))
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

                    AoWCombatMap.Current.Selection.UnselectLocation();
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
            TacticalCombatUnitHS UnitHS;
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
                    var xhx = AoWCombatMap.Current.Mouse.Xhx;
                    condition = path.X == xhx;
                }

                bool test = condition;
                if (test)
                {
                    var path = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                    var yhx = AoWCombatMap.Current.Mouse.Yhx;
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
                        bool notExplorationEnabled = !AoWCombatMap.Current.FExplorationEnabled;
                        if (!notExplorationEnabled)
                        {
                            var yhx = AoWCombatMap.Current.Mouse.Yhx;
                            var xhx = AoWCombatMap.Current.Mouse.Xhx;
                            notExplorationEnabled = ((AoWMapField)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL)).GetExplored(AoWCombatMap.Current.FSeatedPlayer);
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
                                    var yhx = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(B).FMoveObject).GetYhx();
                                    var xhx = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(B).FMoveObject).GetXhx();
                                    var mouseYhx = AoWCombatMap.Current.Mouse.Yhx;
                                    var dHXtoRad2 = HexGeometry.dHXtoRad(AoWCombatMap.Current.Mouse.Xhx, mouseYhx, xhx, yhx);
                                    var yhx2 = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(A).FMoveObject).GetYhx();
                                    var xhx2 = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(A).FMoveObject).GetXhx();
                                    var yhx3 = AoWCombatMap.Current.Mouse.Yhx;
                                    if (dHXtoRad2 < HexGeometry.dHXtoRad(AoWCombatMap.Current.Mouse.Xhx, yhx3, xhx2, yhx2))
                                    {
                                        FMoveSettingsList.FChildren.Exchange(A, B);
                                    }
                                }
                            }

                            if (((TacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(0).FMoveObject).FUnit.GetLocked())
                            {
                                FMoveSettingsList.GetMoveSettings(0).FPath.Clear();
                            }
                            else
                            {
                                var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                CalculateMovePath(0, (sbyte)AoWCombatMap.Current.Mouse.Xhx, (sbyte)yhx, (sbyte)AoWCombatMap.Current.ViewL);
                            }

                            if (FMoveSettingsList.GetMoveSettings(0).FPath.Count > 1)
                            {
                                for (int I = 1, last = FMoveSettingsList.GetCount() - 1; I <= last; I++)
                                {
                                    if (((TacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(I).FMoveObject).FUnit.GetLocked())
                                    {
                                        FMoveSettingsList.GetMoveSettings(I).FPath.Clear();
                                    }
                                    else
                                    {
                                        var yhx = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(I).FMoveObject).GetYhx();
                                        var xhx = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(I).FMoveObject).GetXhx();
                                        var yhx2 = ((HexagonSprite)FMoveSettingsList.GetMoveSettings(0).FMoveObject).GetYhx();
                                        Direction = HexGeometry.dHXtoHN(((HexagonSprite)FMoveSettingsList.GetMoveSettings(0).FMoveObject).GetXhx(), yhx2, xhx, yhx);
                                        var path = FMoveSettingsList.GetMoveSettings(0).FPath[0];
                                        HexGeometry.CenterHNtoHX(FMoveSettingsList.GetMoveSettings(0).FPath[0].X, path.Y, Direction, ref X, ref Y);
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
                        var xhx = AoWCombatMap.Current.Mouse.Xhx;
                        condition = path.X == xhx;
                    }

                    bool test = condition;
                    if (test)
                    {
                        var path = moveSettings.FPath[0];
                        var yhx = AoWCombatMap.Current.Mouse.Yhx;
                        test = path.Y == yhx;
                    }

                    if (test && moveSettings.FPath[0].L == AoWCombatMap.Current.ViewL)
                    {
                        MoveUnits();
                        return;
                    }

                    if (((TacticalCombatUnitHS)FMoveSettingsList.GetMoveSettings(FCurrentMove).FMoveObject).FUnit.GetLocked())
                    {
                        FMoveSettingsList.GetMoveSettings(FCurrentMove).FPath.Clear();
                    }
                    else
                    {
                        var yhx = AoWCombatMap.Current.Mouse.Yhx;
                        var xhx = AoWCombatMap.Current.Mouse.Xhx;
                        CalculateMovePath(FCurrentMove, (sbyte)xhx, (sbyte)yhx, (sbyte)AoWCombatMap.Current.ViewL);
                    }
                }
            }
            else
            {
                if (GetTCUnitHS().FSelectedAbility == -1 && GetTCUnitHS().GetXhx() == AoWCombatMap.Current.Mouse.Xhx && GetTCUnitHS().GetYhx() == AoWCombatMap.Current.Mouse.Yhx)
                {
                    var lhx = GetTCUnitHS().GetLhx();
                    var yhx = GetTCUnitHS().GetYhx();
                    var xhx = GetTCUnitHS().GetXhx();
                    AoWCombatMap.Current.Selection.SelectLocationHX(xhx, yhx, lhx);
                    return;
                }

                if (!GetTCUnitHS().FUnit.GetLocked() && AoWHSMap.Current.FTokenManager.PlayerReady(AoWCombatMap.Current.FCurrentPlayer))
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
                            var xhx = AoWCombatMap.Current.Mouse.Xhx;
                            condition = FTargetX == xhx;
                        }

                        bool test = condition;
                        if (test)
                        {
                            var yhx = AoWCombatMap.Current.Mouse.Yhx;
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
                            if (UnitHS.FSelectedAbility == -1 && UnitHS.GetXhx() == AoWCombatMap.Current.Mouse.Xhx && UnitHS.GetYhx() == AoWCombatMap.Current.Mouse.Yhx)
                            {
                                var lhx = UnitHS.GetLhx();
                                var yhx = UnitHS.GetYhx();
                                var xhx = UnitHS.GetXhx();
                                AoWCombatMap.Current.Selection.SelectLocationHX(xhx, yhx, lhx);
                                return;
                            }

                            if (!UnitHS.Moving())
                            {
                                if (UnitHS.FSelectedAbility == -1)
                                {
                                    if (UnitHS.FUnit.FMovePoints > 0 && !UnitHS.FUnit.GetLocked())
                                    {
                                        if (AoWCombatMap.Current.FExplorationEnabled)
                                        {
                                            var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                            var xhx = AoWCombatMap.Current.Mouse.Xhx;
                                            if (!((AoWMapField)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL)).GetExplored(AoWCombatMap.Current.FSeatedPlayer))
                                            {
                                                return;
                                            }
                                        }

                                        var mouseYhx = AoWCombatMap.Current.Mouse.Yhx;
                                        var mouseXhx = AoWCombatMap.Current.Mouse.Xhx;
                                        CalculateMovePath(FCurrentMove, (sbyte)mouseXhx, (sbyte)mouseYhx, 0);
                                    }
                                }
                                else
                                {
                                    if (UnitHS.FSelectedAbility == 52)
                                    {
                                        EffectType = TacticalTables.SpellTypes[UnitHS.FSelectedSpell];
                                    }
                                    else
                                    {
                                        EffectType = TacticalTables.AbilTypes[UnitHS.FSelectedAbility];
                                    }

                                    switch (EffectType)
                                    {
                                        case TfxType.None:
                                            UnitHS.SelectAbility(-1);
                                            break;
                                        case TfxType.Touch:
                                            if (AoWCombatMap.Current.FExplorationEnabled)
                                            {
                                                var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                                var xhx = AoWCombatMap.Current.Mouse.Xhx;
                                                if (!((AoWMapField)AoWCombatMap.Current.MapContainer.GetField(xhx, yhx, AoWCombatMap.Current.ViewL)).GetExplored(AoWCombatMap.Current.FSeatedPlayer))
                                                {
                                                    return;
                                                }
                                            }

                                            if (!UnitHS.FUnit.GetLocked())
                                            {
                                                var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                                CalculateMovePath(0, (sbyte)AoWCombatMap.Current.Mouse.Xhx, (sbyte)yhx, 0);
                                            }

                                            break;
                                        default:
                                            {
                                                var yhx = AoWCombatMap.Current.Mouse.Yhx;
                                                CalculateRangedPath((sbyte)AoWCombatMap.Current.Mouse.Xhx, (sbyte)yhx);
                                                if (FRangedPath.Count > 0)
                                                {
                                                    var mouseYhx = AoWCombatMap.Current.Mouse.Yhx;
                                                    var xhx = AoWCombatMap.Current.Mouse.Xhx;
                                                    FMouseOverTarget = (HexagonSprite)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[xhx, mouseYhx].FindOwnedChild(2228484);
                                                    AoWCombatMap.Current.FMouseOverTargetEventsStore.Raise(FMouseOverTarget);
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
        public bool ValidMultiMoveSelection(AoWMapField Field)
        {
            sbyte Player;
            Player = Field.GetPlayer();
            return Player == -1;
        }

        // AoWTC.pas:3259
        public void ActivateMoveSelection()
        {
            if (!FStatus.Contains(MoveSelectorStatusElement.Activated))
            {
                Activate(AoWCombatMap.Current);
            }
        }

        // AoWTC.pas:3264
        public void DeactivateMoveSelection()
        {
            if (FStatus.Contains(MoveSelectorStatusElement.Activated))
            {
                Deactivate();
                FRangedPath.Clear();
                FMoveSettingsList.Clear();
                AoWCombatMap.Current.MouseMoveEvent(this);
            }
        }

        // AoWTC.pas:3293
        public void Update()
        {
            if (GetTCUnitHS() != null)
            {
                GetTCUnitHS().FUnit.FUnit.CreateMovePointTable(ref FMoveControlSettings.FMovePointTable);
            }

            FPathInvalid = Set<AnonymousEnum_piMove>.Of(AnonymousEnum_piMove.Ranged);
        }

        // AoWTC.pas:3300
        public void SetTCUnitHS(TacticalCombatUnitHS U)
        {
            if (GetTCUnitHS() != U)
            {
                if (FMoveSettingsList.GetCount() <= 1)
                {
                    Clear();
                    if (U != null && U.FUnit.GetPlayer() == AoWCombatMap.Current.FSeatedPlayer)
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
        public void Select(TacticalCombatUnitHS U)
        {
            AoWCombatMap.Current.Selection.UnselectLocation();
            if (U != null)
            {
                AoWCombatMap.Current.Selection.SelectLocationHS(U);
            }
        }

        // AoWTC.pas:3331
        public void DeactivateRangedPath()
        {
            if (FRangedPathHS.FSelection != null)
            {
                AoWCombatMap.Current.DynamicFixedVisibleHS.RemoveChild(FRangedPathHS);
                FRangedPathHS.FSelection = null;
            }
        }

        private static bool SetupMultiTCUnitMove(MoveSettingsList Moves)
        {
            bool flag = false;
            MultiMoveTE tMultiMoveTE = null;
            CombatMoveTE tCombatMoveTE = null;
            Movepath tMovepath = null;
            bool flag2 = Moves.Count != 0;
            if (flag2)
            {
                sbyte player = ((TacticalCombatUnitHS)Moves.GetMoveSettings(0).FMoveObject).FUnit.GetPlayer();
                flag2 = AoWHSMap.Current.FTokenManager.PlayerReady(player);
            }
            if (flag2)
            {
                tMovepath = new Movepath();
                tMultiMoveTE = new MultiMoveTE(null);
                tMultiMoveTE.FSimultaneous = false;
                Moves.CalculateMovePositions(ValidateTCMovePosition);
                int i = 1;
                for (int num = Moves.Count - 1; i <= num; i++)
                {
                    for (int j = 0; j <= i - 1; j++)
                    {
                        MoveSettings moveSettings = Moves.GetMoveSettings(j);
                        MoveSettings moveSettings2 = Moves.GetMoveSettings(i);
                        if (moveSettings.FPath.Count > moveSettings2.FPath.Count)
                        {
                            ((DelphiList)Moves.FChildren).Exchange(i, j);
                        }
                    }
                }
                int k = 0;
                for (int num2 = Moves.Count - 1; k <= num2; k++)
                {
                    MoveSettings moveSettings3 = Moves.GetMoveSettings(k);
                    if (moveSettings3.FMovePos < moveSettings3.FPath.Count - 1)
                    {
                        tMovepath.Clear();
                        int l = moveSettings3.FMovePos;
                        for (int num3 = moveSettings3.FPath.Count - 1; l <= num3; l++)
                        {
                            tMovepath.Add(moveSettings3.FPath[l]);
                        }
                        tCombatMoveTE = new CombatMoveTE(null);
                        if (tCombatMoveTE.Setup(((TacticalCombatUnitHS)moveSettings3.FMoveObject).FUnit, tMovepath))
                        {
                            tMultiMoveTE.FEvents.Add(tCombatMoveTE);
                        }
                        tCombatMoveTE.Release();
                    }
                }
                if (tMultiMoveTE.FEvents.Count != 0)
                {
                    sbyte player2 = ((TacticalCombatUnitHS)Moves.GetMoveSettings(0).FMoveObject).FUnit.GetPlayer();
                    AoWHSMap.Current.FTokenManager.GetTokenControl(player2).AddEvent(tMultiMoveTE);
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                tMultiMoveTE.Release();
                if (tMovepath != null)
                {
                    ((Persistent)tMovepath).Dispose();
                }
                int m = 0;
                for (int num4 = AoWCombatMap.Current.FUnitSelection.FMoveSettingsList.Count - 1; m <= num4; m++)
                {
                    MoveSettings moveSettings4 = AoWCombatMap.Current.FUnitSelection.FMoveSettingsList.GetMoveSettings(m);
                    moveSettings4.FPath.Clear();
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        private static bool ValidateTCMovePosition(MoveSettingsList Moves, int Index)
        {
            bool flag = true;
            if (flag)
            {
                for (int num = Moves.Count - 1; num >= 0; num--)
                {
                    if (num != Index)
                    {
                        MoveSettings moveSettings = Moves.GetMoveSettings(num);
                        bool flag2 = moveSettings.FValid && Moves.GetMoveSettings(Index).GetMovePrio() >= moveSettings.GetMovePrio();
                        if (flag2)
                        {
                            XYL moveLoc = Moves.GetMoveSettings(Index).GetMoveLoc();
                            flag2 = moveSettings.GetMoveLoc().Packed == moveLoc.Packed;
                        }
                        if (flag2)
                        {
                            flag = false;
                        }
                    }
                }
            }
            return flag;
        }

        public enum AnonymousEnum_piMove : byte
        {
            Move = 0, Ranged = 1
        }

    }

    // FAuto. FEndTurn. FIndex. FLastX. FLastY. FLastUnit
    public partial class TacticalCombatPlayer : CombatPlayer
    {
        public TacticalCombatPlayer(ECustomNode owner) : base(owner)
        { }

        public bool FAuto;
        public bool FEndTurn;
        public sbyte FIndex;
        public int FLastX;
        public int FLastY;
        public TacticalCombatUnit FLastUnit;

        // AoWTC.pas:6419
        public void SetAuto(bool Value)
        {
            FAuto = Value;
            if (AoWCombatMap.Current.FSeatedCombatPlayer == FIndex && AoWCombatMap.Current.FSeatedCombatPlayer == AoWCombatMap.Current.FCurrentCombatPlayer)
            {
                AoWCombatMap.Current.FAI.Reset();
            }
        }

    }

    [LegacyObject]
    // FPlayers
    public partial class TacticalCombatPlayerList
    {
        public CombatPlayerList FPlayers;

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
        public TacticalCombatPlayer GetPlayers(sbyte Index) => (TacticalCombatPlayer)FPlayers.GetPlayers(Index);

        public TacticalCombatPlayerList()
        {
            CaptureAddress.Assign(this);
        }

    }

    public partial class AoWCombatMapMouse : AbstractAoWHSMapMouse
    {

        public AoWCombatMapMouse() : base()
        { }

        // intf_P1_05.inc:68

    }

    // FPlayers. FCombat. FUnitPositions. FSeatedCombatPlayer. FCurrentCombatPlayer. turn/player
    // events. FMoveControl. FUnitSelection. selection/unit events. FAI. FEffects. FWalls.
    // FFleeLocations. FEndTurnPending. FPlaySounds. FPositionStatus. FRound
    public partial class AoWCombatMap : AbstractAoWCombatMap
    {
        // AoWTC.pas:2183
        public new const int ClassID = 2228480;

        public TacticalCombatPlayerList FPlayers;
        public TacticalCombat FCombat;
        public UnitPositionControl FUnitPositions;
        public sbyte FSeatedCombatPlayer;
        public sbyte FCurrentCombatPlayer;

        internal EventStore<sbyte> FChangeTurnEventsStore;

        public event NotifyEvent<sbyte> FChangeTurnEvents
        {
            add => FChangeTurnEventsStore.Add(value);
            remove => FChangeTurnEventsStore.Remove(value);
        }

        internal EventStore<AoWCombatMap> FPlayerChangedEventsStore;

        public event NotifyEvent<AoWCombatMap> FPlayerChangedEvents
        {
            add => FPlayerChangedEventsStore.Add(value);
            remove => FPlayerChangedEventsStore.Remove(value);
        }

        internal EventStore<sbyte> FSeatedPlayerChangedEventsStore;

        public event NotifyEvent<sbyte> FSeatedPlayerChangedEvents
        {
            add => FSeatedPlayerChangedEventsStore.Add(value);
            remove => FSeatedPlayerChangedEventsStore.Remove(value);
        }

        public CombatMoveControl FMoveControl;
        public CombatUnitSelectionControl FUnitSelection;

        internal EventStore<TacticalCombat> FSelectEventsStore;

        public event NotifyEvent<TacticalCombat> FSelectEvents
        {
            add => FSelectEventsStore.Add(value);
            remove => FSelectEventsStore.Remove(value);
        }

        internal EventStore<TacticalCombat> FDeselectEventsStore;

        public event NotifyEvent<TacticalCombat> FDeselectEvents
        {
            add => FDeselectEventsStore.Add(value);
            remove => FDeselectEventsStore.Remove(value);
        }

        public MapObject FCenterViewLocation;

        internal EventStore<HexagonSprite> FMouseOverTargetEventsStore;

        public event NotifyEvent<HexagonSprite> FMouseOverTargetEvents
        {
            add => FMouseOverTargetEventsStore.Add(value);
            remove => FMouseOverTargetEventsStore.Remove(value);
        }

        // CMapCtrl

        internal EventStore<TacticalCombatUnitHS> FUnitChangedEventsStore;

        public event NotifyEvent<TacticalCombatUnitHS> FUnitChangedEvents
        {
            add => FUnitChangedEventsStore.Add(value);
            remove => FUnitChangedEventsStore.Remove(value);
        }

        // CMapCtrl
        public CAI FAI;
        public DelphiList FEffects;
        public DelphiList FWalls;
        public DelphiList FFleeLocations;
        public bool FEndTurnPending;
        public bool FPlaySounds;
        public FixedArray_byte_8 FPositionStatus;
        public int FRound;

        // AoWTC.pas:2213
        public override void Dispose()
        {
            while (FEffects.Count > 0)
            {
                var effect = FEffects[0];
                DynamicFixedVisibleHS.RemoveChild((EObject)effect);
                var effect2 = FEffects[0];
                DynamicVisibleHS.RemoveChild((EObject)effect2);
                (FEffects[0] as IDisposable)?.Dispose();
                FEffects.Delete(0);
            }

            base.Dispose();
            Current = null;
            FUnitPositions?.Dispose();
            FChangeTurnEventsStore.Clear();
            FPlayerChangedEventsStore.Clear();
            FSeatedPlayerChangedEventsStore.Clear();
            FSelectEventsStore.Clear();
            FDeselectEventsStore.Clear();
            FMouseOverTargetEventsStore.Clear();
            FUnitChangedEventsStore.Clear();
            FMoveControl?.Dispose();
            FUnitSelection?.Dispose();
            FEffects?.Dispose();
            FWalls?.Dispose();
            FFleeLocations?.Dispose();
            FAI?.Dispose();
        }

        // AoWTC.pas:2189
        private protected sealed override void InitializeBeforeInherited() => Current = this;

        // AoWTC.pas:2186
        public AoWCombatMap(ECustomNode owner) : base(owner)
        {
            FUnitPositions = new UnitPositionControl(null);
            FChangeTurnEventsStore = new();
            FPlayerChangedEventsStore = new();
            FSeatedPlayerChangedEventsStore = new();
            FMoveControl = new CombatMoveControl(this);
            FSelectEventsStore = new();
            FDeselectEventsStore = new();
            FMouseOverTargetEventsStore = new();
            FUnitChangedEventsStore = new();
            FPlayers = new TacticalCombatPlayerList();
            FUnitSelection = new CombatUnitSelectionControl();
            ((AbstractAoWHSMapMouse)Mouse).FRectangleSelectEvents += FUnitSelection.RectangleSelectEvent;
            FEffects = new DelphiList();
            FWalls = new DelphiList();
            FFleeLocations = new DelphiList();
            FAI = new CAI();
            Mouse.SetCapture(this);
            Mouse.OnMove += MouseMoveEvent;
            Mouse.CursorMode = 256;
            for (int I = 0; I <= 7; I++)
            {
                FPositionStatus[I] = 2;
            }
        }

        // AoWTC.pas:2249
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            if (Msg.MsgID == 536870920 && !Status.Contains(HSMapStatusElement.Editting))
            {
                SetCombat((TacticalCombat)AoWHSMap.Current.FCombat);
            }

            base.MsgProc(ref Msg, Sender);
        }

        // AoWTC.pas:2240
        public override HSMapMouse CreateMouse() => new AoWCombatMapMouse();

        // AoWTC.pas:2267
        public override void InitializeNewMap()
        {
            HexagonControl Control;
            for (int X = 6, last = MapContainer.Width - 7; X <= last; X++)
            {
                for (int Y = 6, lastY = MapContainer.Height - 7; Y <= lastY; Y++)
                {
                    AoWHSSet.Current.FCombatHexagons.Place(MapContainer, (sbyte)X, (sbyte)Y, 0, 1);
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
        public override void DrawDynamicScene(ref SurfaceDesc SD)
        {
            base.DrawDynamicScene(ref SD);
            if (AoWHSMap.Current != null)
            {
                DrawFog(SD);
            }

            if (FShowGrid)
            {
                DrawGrid(SD);
            }

            DrawMoveRoute(SD);
            if (((AbstractAoWHSMapMouse)Mouse).FSelectionRectangle.Left != -1)
            {
                DrawSelectionRect(ViewXhp, ViewYhp, ((AbstractAoWHSMapMouse)Mouse).FSelectionRectangle, SD);
            }
        }

        // AoWTC.pas:2429
        public override void NewFrame()
        {
            int I;
            TfxHS Effect;
            if (!Status.Contains(HSMapStatusElement.Editting))
            {
                I = 0;
                while (I < FEffects.Count)
                {
                    Effect = (TfxHS)FEffects[I];
                    if (Effect.FDone)
                    {
                        DynamicFixedVisibleHS.RemoveChild(Effect);
                        DynamicVisibleHS.RemoveChild(Effect);
                        Effect?.Dispose();
                        FEffects.Delete(I);
                    }
                    else
                    {
                        I += 1;
                    }
                }
            }

            base.NewFrame();
            if (!Status.Contains(HSMapStatusElement.Editting) && FCombat != null)
            {
                UpdateCursor();
                if (FEndTurnPending && (AoWHSMap.Current.FPlayers.GetPlayers(FCurrentPlayer).FPlayerType != PlayerType.Human && AoWHSMap.Current.FPlayerCommunicator.PCC.GetPlayers(FCombat.FHostPlayer).ConnectionMode != ConnectionMode.Remote || FPlayers.GetPlayers(FCurrentCombatPlayer).FAuto && AoWHSMap.Current.FPlayerCommunicator.PCC.GetPlayers(FCurrentPlayer).ConnectionMode != ConnectionMode.Remote))
                {
                    switch (FAI.Status)
                    {
                        case CAI.TaiStatus.Idle:
                            GameTables.AoWEngine.OutputDebugStr("AI for Player " + IntToStr(FCurrentPlayer) + " turn begins.");
                            FAI.BeginTurn();
                            break;
                        case CAI.TaiStatus.Busy:
                            FAI.ContinueTurn();
                            break;
                        case CAI.TaiStatus.Done:
                            if (EndTurn(true))
                            {
                                FAI.Status = CAI.TaiStatus.Idle;
                                GameTables.AoWEngine.OutputDebugStr("AI for Player " + IntToStr(FCurrentPlayer) + " turn ends.");
                            }

                            break;
                    }
                }
            }
        }

        // AoWTC.pas:2515
        public override void DropHeroItem(Hero Hero, Item Item)
        {
            sbyte X;
            sbyte Y;
            sbyte L;
            ItemHS HS;
            int I;
            X = -1;
            Y = -1;
            L = -1;
            I = FCombat.FCombatData.GetCount();
            while (I != 0 && X == -1)
            {
                I -= 1;
                if (FCombat.FCombatData.GetObjects(I) is TacticalCombatUnit && ((CombatUnit)FCombat.FCombatData.GetObjects(I)).FUnit == Hero)
                {
                    X = ((TacticalCombatUnit)FCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx();
                    Y = ((TacticalCombatUnit)FCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
                    L = 0;
                }
            }

            if (X > -1)
            {
                HS = (ItemHS)MapContainer.GetField(X, Y, L).FindOwnedChild(131697);
                if (HS == null)
                {
                    HS = new CItemHS(null);
                    // { Original bug: a failed placement leaves the new item sprite allocated. }
                    if (Current.PlaceHX(HS, X, Y, L))
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
        public override ItemList GetHeroItemsOnGround(Hero Hero)
        {
            ItemHS HS;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            ItemList Result = null;
            GetUnitLocation(Hero, ref X, ref Y, ref L);
            if (X > -1)
            {
                HS = (ItemHS)MapContainer.GetField(X, Y, L).FindOwnedChild(131697);
                if (HS != null)
                {
                    Result = HS.FItems;
                }
            }

            return Result;
        }

        // AoWTC.pas:2243
        public void SetCombat(TacticalCombat Combat)
        {
            FCombat = Combat;
            if (FCombat != null)
            {
                FPlayers.FPlayers = FCombat.FCombatData.FPlayers;
            }
        }

        // AoWTC.pas:2256
        public void MouseMoveEvent(object Sender)
        {
            TacticalCombatUnitHS HS;
            if (!Current.Status.Contains(HSMapStatusElement.Editting) && !FPlayers.GetPlayers(FCurrentCombatPlayer).FAuto && Mouse.Xhx != -1 && Mouse.Yhx != -1 && Mouse.Capture == null)
            {
                Mouse.CursorMode = 0;
                var yhx = Mouse.Yhx;
                var xhx = Mouse.Xhx;
                HS = (TacticalCombatUnitHS)MapContainer.MapLevels[0].Field[xhx, yhx].FindOwnedChild(2228484);
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

            FSeatedPlayerChangedEventsStore.Raise(FCurrentCombatPlayer);
        }

        // AoWTC.pas:2296
        public void NewTurn(sbyte Player)
        {
            TacticalCombatUnit U;
            EMsg Msg = default;
            FCurrentCombatPlayer = Player;
            if (Player == 0)
            {
                FRound += 1;
            }

            SetCurrentPlayer(FPlayers.GetPlayers(FCurrentCombatPlayer).FPlayer);
            FCombat.NewTurn((byte)FCurrentCombatPlayer);
            FUnitSelection.Unselect();
            AoWHSMap.Current.FTokenManager.TokenExecuter.ActivateTC(FCurrentPlayer);
            if (FPlayers.GetPlayers(FCurrentCombatPlayer).FPlayer == 0)
            {
                var ind = AbilityRules.TranslateRStr(IndRStr);
                var beginRound = AbilityRules.TranslateRStr(BeginRoundRStr);
                ShowMessage(ind + " " + beginRound);
            }
            else
            {
                var player = FPlayers.GetPlayers(FCurrentCombatPlayer);
                var playerPlayerRace = AoWHSMap.Current.FPlayers.GetPlayers(player.FPlayer);
                var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[playerPlayerRace.FPlayerRace]);
                var beginRound = AbilityRules.TranslateRStr(BeginRoundRStr);
                ShowMessage(raceNamesPlural + " " + beginRound);
            }

            var playerPlayer = FPlayers.GetPlayers(FCurrentCombatPlayer);
            if (AoWHSMap.Current.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType == PlayerType.Human)
            {
                var player = FPlayers.GetPlayers(FCurrentCombatPlayer);
                if (AoWHSMap.Current.FPlayerCommunicator.PCC.GetPlayers(player.FPlayer).ConnectionMode != ConnectionMode.Remote)
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

            AudioPlayback.PlaySample("Sfx\\Events\\NewEvent.wav");
            EMsg.InitMsg(ref Msg, 537006081, this);
            SendMsg(ref Msg, this);
            if (FExplorationEnabled)
            {
                var rect = new Rect(0, 0, 65, 58);
                TriggerMapChangedEvent(rect, 0);
            }

            FUnitSelection.Unselect();
            FChangeTurnEventsStore.Raise(FCurrentCombatPlayer);
        }

        // AoWTC.pas:2348
        public void DrawMoveRoute(in SurfaceDesc SD)
        {
            FUnitSelection.ShowMovePaths(SD);
        }

        // AoWTC.pas:2351
        public bool EndTurn(bool Automatic)
        {
            CEndTurnTE TE;
            bool Result = false;
            if (!Automatic)
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(FCurrentPlayer).FPlayerType != PlayerType.Human)
                {
                    return Result;
                }

                if (Current.FSeatedCombatPlayer != Current.FCurrentCombatPlayer)
                {
                    return Result;
                }
            }

            if (AoWHSMap.Current.FTokenManager.PlayerReady(FCurrentPlayer))
            {
                TE = CreateTCEndTurnTE(FCurrentPlayer);
                if (TE != null)
                {
                    AoWHSMap.Current.FTokenManager.GetTokenControl(FCurrentPlayer).AddEvent(TE);
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
        public TacticalCombatUnit GetNextUnit(TacticalCombatUnit U, bool AllUnits, bool Forward)
        {
            int Index;
            int I;
            int Start;
            int Step;
            if (U != null && !U.GetEnabled())
            {
                U = null;
            }

            TacticalCombatUnit Result = U;
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
                if (this.FCombat.FCombatData.GetObjects(Index) != null && this.FCombat.FCombatData.GetObjects(Index) is CombatUnit && (AllUnits || ((TacticalCombatUnit)this.FCombat.FCombatData.GetObjects(Index)).FMovePoints > 0) && this.FCombat.FCombatData.GetObjects(Index).GetEnabled() && this.FCombat.FCombatData.GetObjects(Index).FCombatPlayer == FCurrentCombatPlayer)
                {
                    Result = (TacticalCombatUnit)this.FCombat.FCombatData.GetObjects(Index);
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
                    if (this.FCombat.FCombatData.GetObjects(Index) != null && this.FCombat.FCombatData.GetObjects(Index) is CombatUnit && (AllUnits || ((TacticalCombatUnit)this.FCombat.FCombatData.GetObjects(Index)).FMovePoints > 0) && this.FCombat.FCombatData.GetObjects(Index).GetEnabled() && this.FCombat.FCombatData.GetObjects(Index).FCombatPlayer == FCurrentCombatPlayer)
                    {
                        Result = (TacticalCombatUnit)this.FCombat.FCombatData.GetObjects(Index);
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
            if (!((FSeatedPlayer == -1 || AoWHSMap.Current.FTokenManager.PlayerReady(FSeatedPlayer)) && FSeatedPlayer == FCurrentPlayer && !FPlayers.GetPlayers(FCurrentCombatPlayer).FAuto && Current.FSeatedCombatPlayer == Current.FCurrentCombatPlayer))
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
        public void EndCombat()
        {
            TerminateCombatTE TE;
            TE = new TerminateCombatTE(null);
            AoWHSMap.Current.FTokenManager.TokenExecuter.AddSystemEvent(TE);
            TE.Release();
        }

        // AoWTC.pas:2497
        public void SelectNextUnit(bool Forward)
        {
            TacticalCombatUnit U;
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
                        HexGeometry.HXtoHP(U.FUnitHS.GetXhx(), fUnitHSYhx, ref X, ref Y);
                        Selection.SelectLocation(X, Y, 0);
                    }
                }
            }
        }

        // AoWTC.pas:2557
        // The combat object list holds the wall beside the units. The original casts every object to
        // TTacticalCombatUnit and reads FUnit. On a wall that offset is FWallType, FHitPoints and FLocked,
        // a small integer, so the unit comparison fails and the search continues. The port skips objects
        // that are not tactical units, which has the same result.
        public void GetUnitLocation(AbstractUnit U, ref sbyte X, ref sbyte Y, ref sbyte L)
        {
            int I;
            X = -1;
            I = 0;
            while (I < FCombat.FCombatData.GetCount())
            {
                if (FCombat.FCombatData.GetObjects(I) is TacticalCombatUnit && ((CombatUnit)FCombat.FCombatData.GetObjects(I)).FUnit == U && FCombat.FCombatData.GetObjects(I).GetEnabled())
                {
                    X = ((TacticalCombatUnit)FCombat.FCombatData.GetObjects(I)).FUnitHS.GetXhx();
                    Y = ((TacticalCombatUnit)FCombat.FCombatData.GetObjects(I)).FUnitHS.GetYhx();
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
            CEndCombatTE TE;
            if (AoWHSMap.Current.FTokenManager.PlayerReady(Current.FCurrentPlayer) && FCurrentCombatPlayer == FSeatedCombatPlayer)
            {
                FUnitSelection.Unselect();
                TE = CreateTCEndCombatTE();
                if (TE != null)
                {
                    AoWHSMap.Current.FTokenManager.GetTokenControl(FCurrentPlayer).AddEvent(TE);
                    TE.Release();
                }
            }
        }

        private static readonly string IndRStr = "Independents";

        private static readonly string BeginRoundRStr = "begin their round.";

        private static CEndCombatTE CreateTCEndCombatTE() => new CEndCombatTE(null);

        private static CEndTurnTE CreateTCEndTurnTE(sbyte Player)
        {
            CEndTurnTE Result;
            if (Player != -1 && AoWHSMap.Current.FTokenManager.PlayerReady(Player))
            {
                Result = new CEndTurnTE(null);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        internal static void PlaceOverviewUnit(AoWCombatMap Map, AbstractUnit U, int Position, int Index)
        {
            OverviewUnitHS HS;
            sbyte Overlay;
            TacticalCombatUnitHS Occupant;
            int X0;
            int Y0;
            int X = default;
            int Y = default;
            int I;
            XYL P;
            HS = new OverviewUnitHS(null);
            HS.SetU(U);
            P = Map.FUnitPositions.GetUnitPosition(Position, Index);
            X0 = P.X;
            Y0 = P.Y;
            I = 0;
            do
            {
                HexGeometry.CenterHNtoHX(X0, Y0, I, ref X, ref Y);
                Overlay = Current.MapContainer.MapLevels[0].Field[X, Y].OverlayType;
                Occupant = (TacticalCombatUnitHS)Current.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2228484);
                I += 1;
            }
            while (!(Overlay != 8 && Occupant == null));
            if (!Map.PlaceHX(HS, X, Y, 0))
            {
                HS?.Dispose();
            }
        }

        internal static AoWCombatMap Current;

        // AoWTC.pas:484

        // AoWTC.pas:484

    }

    // Inherited combat layout. FHostPlayer
    public partial class TacticalCombat : Combat
    {
        // AoWTC.pas:5081
        public new const int ClassID = 2228496;

        public TacticalCombat(ECustomNode owner) : base(owner)
        { }

        public sbyte FHostPlayer;

        // AoWTC.pas:5086
        public override CombatUnit CreateCombatUnit() => new TacticalCombatUnit(null);

        // AoWTC.pas:5157
        public override CombatPlayer CreateCombatPlayer() => new TacticalCombatPlayer(null);

        // AoWTC.pas:5097
        public override void Initialize()
        {
            base.Initialize();
            FCombatData.FTurnOrder.Clear();
            FCombatData.FTurnOrder.Add((byte)FCombatData.FAttackerPlayer);
            FHostPlayer = -1;
            for (int I = 0, last = AoWHSMap.Current.FPlayers.GetCount() - 1; I <= last; I++)
            {
                if (FHostPlayer == -1)
                {
                    if (AoWHSMap.Current.FPlayers.GetPlayers(I).FPlayerType == PlayerType.Human)
                    {
                        if (AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus != Player.Status.Terminated)
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
                if (AoWHSMap.Current.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType != PlayerType.Human)
                {
                    if (AoWHSMap.Current.FPlayerCommunicator.PCC is RemotePCC)
                    {
                        var playerConnectionMode = AoWHSMap.Current.FPlayerCommunicator.PCC.GetPlayers(FHostPlayer);
                        var player = FCombatData.FPlayers.GetPlayers((sbyte)I);
                        ((RemotePCC)AoWHSMap.Current.FPlayerCommunicator.PCC).SetConnectionMode(player.FPlayer, playerConnectionMode.ConnectionMode);
                        var playerSystemID = AoWHSMap.Current.FPlayerCommunicator.PCC.GetPlayers(FHostPlayer);
                        var player2 = FCombatData.FPlayers.GetPlayers((sbyte)I);
                        ((RemotePCC)AoWHSMap.Current.FPlayerCommunicator.PCC).SetSystemID(player2.FPlayer, playerSystemID.SystemID);
                    }
                }
            }

            for (int I = 0, last = AoWHSMap.Current.FTokenManager.GetCount() - 1; I <= last; I++)
            {
                if (AoWHSMap.Current.FTokenManager.GetTokenControl(I).Local() || AoWHSMap.Current.FTokenManager.GetTokenControl(I).Global())
                {
                    AoWHSMap.Current.FTokenManager.GetTokenControl(I).ClearEvents();
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
            if (FCombatResult != CombatResult.CannotBreachWall)
            {
                UpdateStatus();
            }

            base.Deactivate();
        }

        // AoWTC.pas:5145
        public bool ValidateWallCombat()
        {
            int I;
            CombatSide Side;
            Side = FSides[(int)CombatSideIndex.Attacker];
            bool Result = FCombatData.FParties.GetParty(0).FWall == 0;
            I = Side.GetCount();
            while (!Result && I > 0)
            {
                I -= 1;
                Result = (Side.GetObjects(I).WallCombatFeatures() & Set<WallCombatFeaturesElement>.Of(WallCombatFeaturesElement.WallBreaching, WallCombatFeaturesElement.WallPassing)) != Set<WallCombatFeaturesElement>.Of();
            }

            return Result;
        }

        public override CombatResult Execute()
        {
            if (HeadlessHooks.Tactical || FrameFixtureInputs.Tactical)
            {
                FHostPlayer = 0;
                HeadlessHooks.TacticalExecutions++;
            }
            CombatResult Result = default;
            RndTacHsm Generator = default;
            int Seated = default;
            if (!ValidateWallCombat())
            {
                SetLogCombatActions(1);
                SetLogbookHeader(AbilityRules.TranslateRStr(GameText.CannotBreachWallRStr));
                FCombatResult = CombatResult.CannotBreachWall;
                Result = FCombatResult;
            }
            else
            {
                if (FSides[(int)CombatSideIndex.Defender].GetCount() == FSides[(int)CombatSideIndex.Defender].FConquerCount)
                {
                    SetLogCombatActions(1);
                    SetLogbookHeader(AbilityRules.TranslateRStr(GameText.CannotInflictDamageRStr));
                    FCombatResult = CombatResult.CannotInflictDamage;
                    Result = FCombatResult;
                }
                else
                {
                    if (FSides[(int)CombatSideIndex.Defender].GetCount() == 0)
                    {
                        Result = CombatResult.NoDefenders;
                    }
                    else
                    {
                        AoWHSMap.Current.FTokenManager.TokenExecuter.HoldTC = true;
                        Generator = new RndTacHsm();
                        Generator.RandomMap(this, FCombatData.FX, FCombatData.FY, FCombatData.FL);
                        Generator?.Dispose();
                        bool condition = AoWHSMap.Current.FStartingGame || AoWHSMap.Current.VisibleForPlayer(FCombatData.FX, FCombatData.FY, FCombatData.FL, AoWHSMap.Current.FSeatedPlayer);
                        AoWCombatMap.Current.FPlaySounds = condition;
                        if (!AoWCombatMap.Current.FPlaySounds)
                        {
                            for (int I = 0, last = FCombatData.FPlayers.GetCount() - 1; I <= last; I++)
                            {
                                var playerPlayer = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                if (AoWHSMap.Current.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType == PlayerType.Human)
                                {
                                    var player = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                    if (AoWHSMap.Current.FPlayers.GetPlayers(player.FPlayer).GetLocal())
                                    {
                                        AoWCombatMap.Current.FPlaySounds = true;
                                    }
                                }
                            }
                        }

                        AoWCombatMap.Current.FDeselectEventsStore.Raise(this);
                        Seated = -1;
                        for (int I = 0, last = FCombatData.FPlayers.GetCount() - 1; I <= last; I++)
                        {
                            if (Seated == -1)
                            {
                                var playerPlayer = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                if (AoWHSMap.Current.FPlayers.GetPlayers(playerPlayer.FPlayer).FPlayerType == PlayerType.Human)
                                {
                                    var player = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                    if (AoWHSMap.Current.FPlayerCommunicator.PCC.GetPlayers(player.FPlayer).ConnectionMode != ConnectionMode.Remote)
                                    {
                                        var player2 = FCombatData.FPlayers.GetPlayers((sbyte)I);
                                        if (AoWHSMap.Current.FPlayers.GetPlayers(player2.FPlayer).FStatus != Player.Status.Terminated)
                                        {
                                            Seated = I;
                                        }
                                    }
                                }
                            }
                        }

                        AoWCombatMap.Current.SetSeatedCombatPlayer((sbyte)Seated);
                        AoWCombatMap.Current.CenterViewTo(32, 29, 0);
                        AoWCombatMap.Current.FCombat.NewRound();
                        var turnOrder = FCombatData.FTurnOrder[0];
                        AoWCombatMap.Current.NewTurn((sbyte)turnOrder);
                        Result = CombatResult.Busy;
                        FOnTerminated = _ => AoWCombatMap.Current.EndCombat();
                        SetLogCombatActions(1);
                    }
                }
            }

            FrameFixtureInputs.TacticalInitialized?.Invoke();
            return Result;
        }
    }

    [LegacyObject]
    // FItems. FRandomTerrain. neighbor weights. terrain weights. terrain-tag weights. terrain
    // samples. tag samples. resource lists. FHomePosition
    public partial class RndTacHsm : IDisposable
    {
        public ItemList FItems;
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
        public virtual void Dispose()
        {
            for (int I = 0; I <= 285; I++)
            {
                FResources[I]?.Dispose();
            }
        }

        // AoWTC.pas:4137
        public RndTacHsm()
        {
            CaptureAddress.Assign(this);
            for (int I = 0; I <= 285; I++)
            {
                var list = new DelphiList();
                FResources[I] = list;
            }

            for (int J = 0, last = AoWHSSet.Current.FCombatResources.GetCount() - 1; J <= last; J++)
            {
                if (AoWHSSet.Current.FCombatResources.MORs[J] is CombatTerrain.Resource)
                {
                    var mor = AoWHSSet.Current.FCombatResources.MORs[J];
                    var mor2 = AoWHSSet.Current.FCombatResources.MORs[J];
                    var mor3 = AoWHSSet.Current.FCombatResources.MORs[J];
                    FResources[(sbyte)((TerrainMOR)mor2).TerrainType * 19 + ((TerrainMOR)mor3).DecorationType].Add(mor);
                }
            }

            InitTables();
        }

        // AoWTC.pas:4156
        public AoWCombatMap RandomMap(TacticalCombat Combat, int X, int Y, int L)
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
            Structure Structure;
            HexagonBridge Bridge;
            FixedArray_int_20 Weights = default;
            bool Blocked(int X, int Y, int First, int Last)
            {
                int Kind;
                int NX = default;
                int NY = default;
                bool Result = false;
                for (int I = First; I <= Last; I++)
                {
                    HexGeometry.CenterHNtoHX(X, Y, clusterChk[I], ref NX, ref NY);
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
            CombatPlacement.CombatXLoc = X;
            CombatPlacement.CombatYLoc = Y;
            CombatPlacement.CombatLLoc = L;
            RandSeed = Y * 128 + X + L * 16384 + AoWHSMap.Current.FInitialRandomSeed;
            I = 0;
            for (; I <= 6; I += 1)
            {
                HexGeometry.CenterHNtoHX(X, Y, I, ref J, ref K);
                var mapField = AoWHSMap.Current.MapContainer.MapLevels[L].Field[J, K];
                FTerrain[I] = mapField.TerrainType;
                var combatTerrainTag = ((AoWMapField)AoWHSMap.Current.MapContainer.MapLevels[L].Field[J, K]).GetCombatTerrainTag();
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
            Structure = (Structure)AoWHSMap.Current.MapContainer.MapLevels[L].Field[X, Y].FindNoneTransparentHS(typeof(Structure));
            Bridge = null;
            if (Structure == null)
            {
                Bridge = (HexagonBridge)AoWHSMap.Current.MapContainer.MapLevels[L].Field[X, Y].FindNoneTransparentHS(typeof(HexagonBridge));
            }

            if (Structure != null)
            {
                // The kind names the map (Structure.Views.cs) and reports what the map takes from the combat.
                var site = new CombatMapSite(SiteCombat, RandomTerrain, (Direction, Kind) =>
                {
                    HexGeometry.CenterHNtoHX(X, Y, Direction, ref J, ref K);
                    return AoWHSMap.Current.MapContainer.MapLevels[L].Field[J, K].FindNoneTransparentHS(Kind) != null;
                });
                FileName = FileName + Structure.CombatMapName(site);
                RandomTerrain = site.RandomTerrain;
                if (site.TakesCombatItems)
                {
                    FItems = (ItemList)Combat.FItems;
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
                GameTables.AoWEngine.LoadHSMEx(FileBoundary.ReadName(GameTables.AoWEngine.StartupDirectory + FileName), false, null, Aow.Formats.GameProfile.TacticalMap);
            }
            else
            {
                AoWHSMap.Current.InitializeCombatMap(Combat, FileName);
            }

            AoWCombatMap.Current.SetCombat(Combat);
            AoWCombatMap.Current.SetExplorationEnabled(SiteCombat);
            AoWCombatMap.Current.SetFogEnabled(false);
            if (Combat != null)
            {
                PlaceCombatUnits();
            }

            if (RandomTerrain)
            {
                AoWCombatMap.Current.LockMapChangedEvent();
                ShapeBuffer = new byte[3770];
                TerrainBuffer = new byte[3770];
                for (int Row = 6; Row <= 52; Row++)
                {
                    for (int Col = 6; Col <= 58; Col++)
                    {
                        Shape = MapShape[Row * 65 + Col];
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

                        if (AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Col, Row].GetDecorationType() == 9)
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
                                HexGeometry.CenterHNtoHX(X, Y, J, ref K, ref NX);
                                if (AoWHSMap.Current.MapContainer.MapLevels[L].Field[K, NX].FindOwnedChild(131946) == null)
                                {
                                    AoWCombatMap.Current.ClearTerrain(Col, Row, 0, 0, -1);
                                }
                            }
                        }

                        ShapeBuffer[Row * 65 + Col] = Shape;
                    }
                }

                if (AoWHSMap.Current.MapContainer.MapLevels[L].Field[X, Y].OverlayType == 4)
                {
                    I = 0;
                    for (; I <= 6; I += 1)
                    {
                        HexGeometry.CenterHNtoHX(X, Y, I, ref J, ref K);
                        var item = AoWHSMap.Current.MapContainer.MapLevels[L].Field[J, K];
                        Weights[I] = item.OverlayType;
                    }

                    for (int Row = 6; Row <= 52; Row++)
                    {
                        for (int Col = 6; Col <= 58; Col++)
                        {
                            Shape = MapRoads[Row * 65 + Col];
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

                            if (Set<byte>.WithBounds(0, 255).WithRange(1, 7).Contains(Shape))
                            {
                                if (FTags[Shape - 1] != 127 && Weights[Shape - 1] == 4)
                                {
                                    AoWHSSet.Current.FCombatRoads.Place(AoWCombatMap.Current.MapContainer, (sbyte)Col, (sbyte)Row, 0, 1);
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
                                HexGeometry.CenterHNtoHX(Col * 2, Row, clusterChk[I], ref J, ref K);
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

                            HasUnit = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Col * 2, Row].FindChild(2228484) != null;
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
                                    HexGeometry.CenterHNtoHX(Col * 2, Row, 6, ref K, ref NX);
                                    TerrainBuffer[NX * 65 + K] = 252;
                                    break;
                                case 15:
                                    HexGeometry.CenterHNtoHX(Col * 2, Row, 6, ref K, ref NX);
                                    TerrainBuffer[NX * 65 + K] = 253;
                                    break;
                                case 16:
                                    HexGeometry.CenterHNtoHX(Col * 2, Row, 1, ref K, ref NX);
                                    TerrainBuffer[NX * 65 + K] = 254;
                                    HexGeometry.CenterHNtoHX(Col * 2, Row, 2, ref K, ref NX);
                                    TerrainBuffer[NX * 65 + K] = 254;
                                    HexGeometry.CenterHNtoHX(Col * 2, Row, 6, ref K, ref NX);
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
                                HexGeometry.CenterHNtoHX(Col * 2 + 1, Row, clusterChk[I], ref J, ref K);
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

                            HasUnit = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Col * 2 + 1, Row].FindChild(2228484) != null;
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
                                    HexGeometry.CenterHNtoHX(Col * 2 + 1, Row, 6, ref K, ref NX);
                                    TerrainBuffer[NX * 65 + K] = 252;
                                    break;
                                case 15:
                                    HexGeometry.CenterHNtoHX(Col * 2 + 1, Row, 6, ref K, ref NX);
                                    TerrainBuffer[NX * 65 + K] = 253;
                                    break;
                                case 16:
                                    HexGeometry.CenterHNtoHX(Col * 2 + 1, Row, 1, ref K, ref NX);
                                    TerrainBuffer[NX * 65 + K] = 254;
                                    HexGeometry.CenterHNtoHX(Col * 2 + 1, Row, 2, ref K, ref NX);
                                    TerrainBuffer[NX * 65 + K] = 254;
                                    HexGeometry.CenterHNtoHX(Col * 2 + 1, Row, 6, ref K, ref NX);
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
                                AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Col, Row].TerrainType = 7;
                            }
                            else
                            {
                                AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Col, Row].TerrainType = FTerrain[I - 1];
                                if (J > 0 && J < 19)
                                {
                                    if (FResources[FTerrain[I - 1] * 19 + J].Count > 0)
                                    {
                                        var random = Random(FResources[FTerrain[I - 1] * 19 + J].Count);
                                        ((MapObjectResource)FResources[FTerrain[I - 1] * 19 + J][random]).PlaceResourceUser(AoWCombatMap.Current, (sbyte)Col, (sbyte)Row, 0);
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
                        AoWCombatMap.Current.MapContainer.MapLevels[0].Field[Col, Row].TerrainChanged();
                    }
                }

                TerrainBuffer = default;
                AoWCombatMap.Current.UnlockMapChangedEvent();
            }

            return AoWCombatMap.Current;
        }

        // AoWTC.pas:4519
        public void PlaceCombatUnits()
        {
            int Position;
            int I;
            int J = default;
            int Temp = default;
            DelphiList Units;
            TacticalCombatUnit U;
            TacticalCombatUnit Other;
            int Direction;
            FixedArray_bool_8 Used = default;
            CItemHS ItemHS;
            Direction = 32;
            FHomePosition = -1;
            Units = new DelphiList();
            Position = 0;
            for (; Position <= 7; Position += 1)
            {
                Units.Clear();
                I = 0;
                int last = AoWCombatMap.Current.FCombat.FCombatData.GetCount() - 1;
                for (; I <= last; I += 1)
                {
                    if (AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit)
                    {
                        if (AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled())
                        {
                            if ((int)(((uint)AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).FPosition) >> 4) == Position)
                            {
                                Units.Add(AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I));
                                U = (TacticalCombatUnit)Units[Units.Count - 1];
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
                                    if (U.GetSide() == CombatSideIndex.Defender)
                                    {
                                        if (AoWHSMap.Current.MapContainer.MapLevels[AoWCombatMap.Current.FCombat.FCombatData.FL].Field[AoWCombatMap.Current.FCombat.FCombatData.FX, AoWCombatMap.Current.FCombat.FCombatData.FY].FindNoneTransparentHS(typeof(City)) != null)
                                        {
                                        }
                                    }
                                }

                                if (FHomePosition > 0 && U.FPositionGroup == FHomePosition)
                                {
                                    U.FPositionGroup = 0;
                                }

                                var side = U.GetSide();
                                AoWCombatMap.Current.FPositionStatus[U.FPositionGroup] = (byte)side;
                            }
                        }
                    }
                }

                I = 0;
                int limit = Units.Count - 1;
                for (; I <= limit; I += 1)
                {
                    U = (TacticalCombatUnit)Units[I];
                    J = I + 1;
                    int last2 = Units.Count - 1;
                    for (; J <= last2; J += 1)
                    {
                        Other = (TacticalCombatUnit)Units[J];
                        if (U.GetPosValue() < Other.GetPosValue())
                        {
                            Temp = U.FPositionIndex;
                            U.FPositionIndex = Other.FPositionIndex;
                            Other.FPositionIndex = (byte)Temp;
                        }
                    }
                }
            }

            if (AoWCombatMap.Current.FPositionStatus[0] != 1)
            {
                if (AoWHSMap.Current.MapContainer.MapLevels[AoWCombatMap.Current.FCombat.FCombatData.FL].Field[AoWCombatMap.Current.FCombat.FCombatData.FX, AoWCombatMap.Current.FCombat.FCombatData.FY].FindNoneTransparentHS(typeof(City)) != null && AoWCombatMap.Current.FCombat.FCombatData.FWallType != 0)
                {
                    Position = 1;
                    while (Position < 7)
                    {
                        if (AoWCombatMap.Current.FPositionStatus[Position] == 1)
                        {
                            HexGeometry.CenterHNtoHX(AoWCombatMap.Current.FCombat.FCombatData.FX, AoWCombatMap.Current.FCombat.FCombatData.FY, Position, ref J, ref Temp);
                            if (AoWHSMap.Current.MapContainer.MapLevels[AoWCombatMap.Current.FCombat.FCombatData.FL].Field[J, Temp].FindNoneTransparentHS(typeof(City)) != null)
                            {
                                FHomePosition = Position;
                                AoWCombatMap.Current.FPositionStatus[Position] = (int)CombatSideIndex.Neutral;
                                AoWCombatMap.Current.FPositionStatus[0] = 1;
                                Position = 7;
                            }
                        }

                        Position += 1;
                    }
                }
            }

            Position = 0;
            int last3 = AoWCombatMap.Current.FCombat.FSides[(int)CombatSideIndex.Attacker].GetCount() - 1;
            for (; Position <= last3; Position += 1)
            {
                if (AoWCombatMap.Current.FCombat.FSides[(int)CombatSideIndex.Attacker].GetObjects(Position).GetEnabled())
                {
                    SetupUnit(CombatSideIndex.Attacker, Position, ref Direction);
                }
            }

            Position = 0;
            int last4 = AoWCombatMap.Current.FCombat.FSides[(int)CombatSideIndex.Defender].GetCount() - 1;
            for (; Position <= last4; Position += 1)
            {
                if (AoWCombatMap.Current.FCombat.FSides[(int)CombatSideIndex.Defender].GetObjects(Position) is CombatUnit)
                {
                    if (AoWCombatMap.Current.FCombat.FSides[(int)CombatSideIndex.Defender].GetObjects(Position).GetEnabled())
                    {
                        SetupUnit(CombatSideIndex.Defender, Position, ref Direction);
                    }
                }
            }

            Units?.Dispose();
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
                    I = Random(AoWCombatMap.Current.FUnitPositions.FPositions.Count - 16);
                    J = I - 1;
                    if (J < 0)
                    {
                        J = AoWCombatMap.Current.FUnitPositions.FPositions.Count - 17;
                    }

                    // { Original indexes its eight-slot occupancy array with the position-list index. }
                    while (Used[I] && I != J)
                    {
                        I += 1;
                        if (I > AoWCombatMap.Current.FUnitPositions.FPositions.Count - 17)
                        {
                            I = 0;
                        }
                    }

                    Used[I] = true;
                    J = AoWCombatMap.Current.FUnitPositions.GetUnitPosition(2, I).X;
                    Temp = AoWCombatMap.Current.FUnitPositions.GetUnitPosition(2, I).Y;
                    ItemHS = new CItemHS(null);
                    if (AoWCombatMap.Current.PlaceHX(ItemHS, J, Temp, 0))
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
        public void SetupUnit(CombatSideIndex Side, int Index, ref int Direction)
        {
            TacticalCombatUnitHS UnitHS;
            TacticalCombatUnitHS TargetHS;
            int X;
            int Y;
            int Position;
            int Slot;
            int HN;
            int NX = default;
            int NY = default;
            UnitHS = new TacticalCombatUnitHS(null);
            UnitHS.Initialize((TacticalCombatUnit)AoWCombatMap.Current.FCombat.FSides[(int)Side].GetObjects(Index));
            if (UnitHS.FUnit.FPositionGroup == FHomePosition)
            {
                UnitHS.FUnit.FPositionGroup = 0;
            }

            Position = UnitHS.FUnit.FPositionGroup;
            Slot = UnitHS.FUnit.FPositionIndex;
            X = AoWCombatMap.Current.FUnitPositions.GetUnitPosition(Position, Slot).X;
            Y = AoWCombatMap.Current.FUnitPositions.GetUnitPosition(Position, Slot).Y;
            if (Side == CombatSideIndex.Defender && FRandomTerrain)
            {
                switch (Position)
                {
                    case 0:
                        if (Direction > 0)
                        {
                            X = DefPosX[(Direction - 1) * 8 + Slot];
                            Y = DefPosY[(Direction - 1) * 8 + Slot];
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
                UnitHS.SetDirection((byte)AoWCombatMap.Current.FUnitPositions.GetUnitPosition(Position, Slot).Flag);
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
                HexGeometry.CenterHNtoHX(X, Y, HN, ref NX, ref NY);
                Slot = AoWCombatMap.Current.MapContainer.MapLevels[0].Field[NX, NY].OverlayType;
                TargetHS = (TacticalCombatUnitHS)AoWCombatMap.Current.MapContainer.MapLevels[0].Field[NX, NY].FindOwnedChild(2228484);
                HN += 1;
            }
            while (!(Slot != 8 && TargetHS == null));
            AoWCombatMap.Current.PlaceHX(UnitHS, NX, NY, 0);
            if (HexGeometry.dHXtoHN(AoWCombatMap.Current.FCombat.FCombatData.FX, AoWCombatMap.Current.FCombat.FCombatData.FY, AoWCombatMap.Current.FCombat.FCombatData.FAttackerX, AoWCombatMap.Current.FCombat.FCombatData.FAttackerY) == Position && AoWCombatMap.Current.FPlayers.GetPlayers(UnitHS.FUnit.FCombatPlayer).FLastX == 0)
            {
                var xhx = UnitHS.GetXhx();
                AoWCombatMap.Current.FPlayers.GetPlayers(UnitHS.FUnit.FCombatPlayer).FLastX = xhx;
                var yhx = UnitHS.GetYhx();
                AoWCombatMap.Current.FPlayers.GetPlayers(UnitHS.FUnit.FCombatPlayer).FLastY = yhx;
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

        private static FixedArray_int_48 DefPosX = FixedArray_int_48.From([32, 30, 34, 32, 28, 36, 30, 34, 34, 36, 32, 38, 30, 36, 32, 34, 36, 34, 38, 32, 36, 30, 34, 32, 32, 34, 30, 32, 36, 28, 34, 30, 30, 28, 32, 26, 34, 28, 32, 30, 28, 30, 26, 32, 28, 34, 30, 32]);
        private static FixedArray_int_48 DefPosY = FixedArray_int_48.From([26, 27, 27, 28, 28, 28, 29, 29, 27, 28, 26, 29, 27, 30, 28, 29, 30, 31, 29, 32, 28, 31, 29, 30, 32, 31, 31, 30, 30, 30, 29, 29, 31, 30, 32, 29, 31, 28, 30, 29, 28, 27, 29, 26, 30, 27, 29, 28]);

        private static FixedArray_byte_3770 MapShape = FixedArray_byte_3770.From((ReadOnlySpan<byte>)new byte[3770]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 2, 2, 2, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 2, 2, 2, 2, 2, 2,
            2, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 10, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 11, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 7,
            7, 7, 10, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 11, 3, 3, 3, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 7, 7, 7, 7, 7, 10, 10, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 11, 11, 3, 3,
            3, 3, 3, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 7, 7, 7, 7, 7,
            7, 7, 7, 10, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 11, 3, 3, 3, 3, 3, 3, 3, 3,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 10,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 11, 3, 3, 3, 3,
            3, 3, 3, 3, 3, 3, 3, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 10, 10, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 2, 11,
            11, 3, 3, 3, 3, 3, 3, 3, 3, 3,
            3, 3, 3, 3, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            10, 2, 2, 2, 2, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 11, 3, 3, 3, 3, 3,
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
            3, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 10, 2, 2, 2,
            2, 2, 2, 2, 2, 2, 2, 2, 11, 3,
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
            3, 3, 3, 3, 3, 3, 3, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 10, 10, 2, 14, 2, 14, 2, 14, 2,
            14, 2, 11, 11, 3, 3, 3, 3, 3, 3,
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
            3, 3, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 19, 14, 1,
            14, 1, 14, 1, 14, 1, 14, 15, 3, 3,
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
            3, 3, 3, 3, 3, 3, 3, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            19, 19, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 15, 15, 3, 3, 3, 3, 3,
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
            3, 3, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 19, 19, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            15, 15, 3, 3, 3, 3, 3, 3, 3, 3,
            3, 3, 3, 3, 3, 3, 3, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 19, 19, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 15, 15, 3,
            3, 3, 3, 3, 3, 3, 3, 3, 3, 3,
            3, 3, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 19,
            19, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 15, 15, 3, 3, 3, 3,
            3, 3, 3, 3, 3, 3, 3, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 19, 19, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 15, 15, 3, 3, 3, 3, 3, 3, 3,
            3, 3, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
            7, 7, 7, 7, 7, 19, 19, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 15, 15,
            3, 3, 3, 3, 3, 3, 3, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 7, 7, 7, 7, 7, 7, 19,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 15, 3, 3, 3, 3,
            3, 3, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
            7, 7, 7, 7, 19, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            15, 3, 3, 3, 3, 3, 3, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 8, 8, 8, 8, 8, 8, 19,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 15, 9, 9, 9, 9,
            9, 9, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
            6, 6, 6, 6, 18, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            16, 4, 4, 4, 4, 4, 4, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 6, 6, 6, 6, 6, 6, 18,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 16, 4, 4, 4, 4,
            4, 4, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
            6, 6, 6, 6, 18, 18, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 16,
            16, 4, 4, 4, 4, 4, 4, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 6, 6, 6, 6, 6, 6, 6,
            6, 18, 18, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 16, 16, 4, 4, 4, 4, 4, 4,
            4, 4, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 18, 18,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 16, 16, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 18, 18, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 16, 16,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 18, 18, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 16, 16, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 18,
            18, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 1, 1, 1, 16, 16, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 18, 17, 1, 1,
            1, 1, 1, 1, 1, 1, 1, 17, 16, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 12, 5, 17, 17, 17, 17, 17, 17, 17,
            17, 17, 5, 13, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 12, 12, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5, 13, 13,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            12, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 13, 4, 4, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 12, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            13, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            4, 4, 4, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 12, 12,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 13, 13, 4, 4, 4,
            4, 4, 4, 4, 4, 4, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 6, 6, 6, 6,
            6, 6, 6, 12, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 13, 4, 4, 4, 4, 4, 4, 4, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 6, 6, 6, 6, 12, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 13, 4, 4,
            4, 4, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            6, 12, 12, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 13, 13, 4, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 5, 5, 5, 5, 5,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 5, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        });

        private static FixedArray_byte_3770 MapRoads = FixedArray_byte_3770.From((ReadOnlySpan<byte>)new byte[3770]
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 2, 2, 2, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 26, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 22, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 7, 7, 7, 26, 26, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 22, 22, 3, 3,
            3, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 7, 7,
            7, 7, 7, 7, 26, 26, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 22,
            22, 3, 3, 3, 3, 3, 3, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 26, 26, 7, 7, 7, 7, 7,
            7, 26, 26, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 22, 22, 3, 3, 3, 3, 3, 3,
            22, 22, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            26, 26, 7, 7, 7, 7, 7, 7, 26, 26,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 22, 22, 3, 3, 3,
            3, 3, 3, 22, 22, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 26, 26, 7,
            7, 7, 7, 7, 7, 26, 26, 0, 0, 0,
            0, 0, 0, 0, 0, 21, 2, 2, 2, 21,
            0, 0, 0, 0, 0, 0, 0, 0, 22, 22,
            3, 3, 3, 3, 3, 3, 22, 22, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 26, 26, 7, 7, 7, 7,
            7, 7, 26, 26, 0, 0, 0, 0, 0, 0,
            21, 2, 2, 2, 21, 0, 0, 0, 0, 0,
            0, 22, 22, 3, 3, 3, 3, 3, 3, 22,
            22, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 26, 26, 7, 7, 7, 7, 7, 7, 26,
            26, 0, 0, 0, 0, 21, 2, 2, 2, 21,
            0, 0, 0, 0, 22, 22, 3, 3, 3, 3,
            3, 3, 22, 22, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 26, 26,
            7, 7, 7, 7, 7, 7, 26, 26, 0, 0,
            21, 2, 2, 2, 21, 0, 0, 22, 22, 3,
            3, 3, 3, 3, 3, 22, 22, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 26, 26, 7, 7, 7,
            7, 7, 7, 26, 26, 21, 2, 2, 2, 21,
            22, 22, 3, 3, 3, 3, 3, 3, 22, 22,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 26, 26, 7, 7, 7, 7, 7, 7,
            21, 2, 2, 2, 21, 3, 3, 3, 3, 3,
            3, 22, 22, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 26,
            26, 7, 7, 7, 7, 7, 1, 1, 1, 3,
            3, 3, 3, 3, 22, 22, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 26, 26, 7, 7,
            1, 1, 1, 1, 1, 3, 3, 22, 22, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 25, 25, 6, 1, 1, 1, 1, 1,
            4, 23, 23, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 25, 25, 6, 6, 6,
            1, 1, 1, 1, 1, 4, 4, 4, 23, 23,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 25, 25,
            6, 6, 6, 6, 6, 6, 5, 1, 5, 4,
            4, 4, 4, 4, 4, 23, 23, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 25, 25, 6, 6, 6, 6, 6, 6, 25,
            24, 5, 5, 5, 24, 23, 4, 4, 4, 4,
            4, 4, 23, 23, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 25, 25, 6, 6, 6, 6,
            6, 6, 25, 25, 0, 24, 5, 5, 5, 24,
            0, 23, 23, 4, 4, 4, 4, 4, 4, 23,
            23, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 25, 25, 6,
            6, 6, 6, 6, 6, 25, 25, 0, 0, 0,
            24, 5, 5, 5, 24, 0, 0, 0, 23, 23,
            4, 4, 4, 4, 4, 4, 23, 23, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            25, 25, 6, 6, 6, 6, 6, 6, 25, 25,
            0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
            0, 0, 0, 0, 0, 23, 23, 4, 4, 4,
            4, 4, 4, 23, 23, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 25, 25, 6, 6, 6, 6, 6,
            6, 25, 25, 0, 0, 0, 0, 0, 0, 0,
            24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
            0, 0, 23, 23, 4, 4, 4, 4, 4, 4,
            23, 23, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 25, 25, 6, 6,
            6, 6, 6, 6, 25, 25, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 23,
            23, 4, 4, 4, 4, 4, 4, 23, 23, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 25,
            25, 6, 6, 6, 6, 6, 6, 25, 25, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 23, 23, 4, 4,
            4, 4, 4, 4, 23, 23, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 25, 6, 6, 6, 6, 6, 6,
            25, 25, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 23, 23, 4, 4, 4, 4, 4,
            4, 23, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 6, 6,
            6, 6, 6, 25, 25, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            23, 23, 4, 4, 4, 4, 4, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 6, 25, 25, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 23, 23, 4,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            24, 5, 5, 5, 24, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 24, 5, 5, 5, 24,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 5, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0
        });

        private static FixedArray_int_18 clusterChk = FixedArray_int_18.From([1, 2, 6, 16, 7, 8, 9, 17, 18, 19, 20, 21, 22, 23, 33, 34, 35, 36]);

    }

    public enum FireStart : byte
    {
        New = 0,
        Spread = 1
    }
}

namespace Aow.Game.Combat
{

    public enum TfxType : byte
    {
        None = 0,
        Arrow = 1,
        Rock = 2,
        Comet = 3,
        Touch = 4,
        Breath = 5,
        CometBall = 6,
        Effect = 7,
        FBExplosion = 8,
        ScreenNESW = 9,
        ScreenNS = 10,
        Ray = 11,
        Chain = 12,
        Swarm = 13,
        ScreenSwarm = 14,
        SFOverlay = 15,
        DamLightning = 16,
        DamFire = 17,
        DamPoison = 18,
        DamMagic = 19,
        DamHoly = 20,
        DamDeath = 21,
        DamCold = 22,
        SacredWrath = 23,
        SWComet = 24,
        Dispel = 25,
        WallSmoke = 26,
        Blood = 27,
        AltArrow = 28,
        Tremors = 29,
        Geyser = 30,
        GeyserWater = 31,
        HailImpact = 32,
        Cloud = 33,
        CloudPuff = 34,
        HitGround = 35,
        HitWater = 36,
        HighPrayer = 37,
        PrayerGlow = 38,
        Resurrect = 39,
        FrostBeam = 40,
        Frost = 41,
        MindDecay = 42,
        DecayLeg = 43,
        ShockWave = 44,
        DecayComet = 45,
        SacFlame = 46,
        Winds = 47,
        SacFire = 48,
        BreathDam = 49
    }

    public enum RndHexMask : byte
    {
        None = 0,
        Center = 1,
        North = 2,
        NE = 3,
        SE = 4,
        South = 5,
        SW = 6,
        NW = 7,
        NWSW = 8,
        NESE = 9,
        NNW = 10,
        NNE = 11,
        SSW = 12,
        SSE = 13,
        CN = 14,
        CNE = 15,
        CSE = 16,
        CS = 17,
        CSW = 18,
        CNW = 19,
        RC = 20,
        RN = 21,
        RNE = 22,
        RSE = 23,
        RS = 24,
        RSW = 25,
        RNW = 26
    }

    public enum CombatAbilityIndex : byte
    {
        WallCrushing = 0,
        Fire = 1,
        Max = 2
    }

    public enum TaiMoveType : byte
    {
        Move = 0,
        Touch = 1,
        Melee = 2,
        Ranged = 3,
        Fake = 4,
        MoveRanged = 5,
        WallTouch = 6,
        WallRanged = 7
    }
}
