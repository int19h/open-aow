// Generated from AoWTC.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.FleeLoc;
using static Aow.Units.FleeLoc.FleeLoc;
using Aow.Units.CombatHex;
using static Aow.Units.CombatHex.CombatHex;
using Aow.Units.CombatTE;
using static Aow.Units.CombatTE.CombatTE;
using Aow.Units.AoWReg;
using static Aow.Units.AoWReg.AoWReg;
using Aow.Units.CombatTerrain;
using static Aow.Units.CombatTerrain.CombatTerrain;
using Aow.Units.HxBridge;
using static Aow.Units.HxBridge.HxBridge;
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.CombatRoad;
using static Aow.Units.CombatRoad.CombatRoad;
using Aow.Units.CombatObstacle;
using static Aow.Units.CombatObstacle.CombatObstacle;
using Aow.Units.CityWall;
using static Aow.Units.CityWall.CityWall;
using Aow.Units.CombatFire;
using static Aow.Units.CombatFire.CombatFire;

namespace Aow.Units.AoWTC
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Record_TCombatUnitSelectionControl_UpdateMoveCursor_AbilityInfo_3187_3
    {
        [FieldOffset(0)]
        public Aow.Units.AoWE.TAbilityCombatInfo Data;
        [FieldOffset(0)]
        public Aow.Units.FixedArray_int_2 Align;
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Record_TTacticalCombatUnitHS_CanMoveOn_AbilityInfo_3374_3
    {
        [FieldOffset(0)]
        public Aow.Units.AoWE.TAbilityCombatInfo Data;
        [FieldOffset(0)]
        public Aow.Units.FixedArray_int_2 Align;
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Record_TTacticalCombatUnitHS_CanMoveOn_EnemyInfo_3374_16
    {
        [FieldOffset(0)]
        public Aow.Units.AoWE.TAbilityCombatInfo Data;
        [FieldOffset(0)]
        public Aow.Units.FixedArray_int_2 Align;
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Record_TTacticalCombatUnitHS_AbTouchMoveTC_Buffer_3936_22
    {
        [FieldOffset(0)]
        public Aow.Units.AoWE.TAbilityCombatInfo Info;
        [FieldOffset(0)]
        public Aow.Units.FixedArray_int_2 Storage;
    }

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
    // { TTCAbility (instance size 12, VMT 00411A60) }
    public partial class TTCAbility : Aow.Rtl.TObject
    {
        public int Ability;
        public int Spell;
        // System.pas:111
        public static Aow.Units.AoWTC.TTCAbility Create()
        {
            return new Aow.Units.AoWTC.TTCAbility();
        }
    }

    // { $04, $08 }
    // { TROb (instance size 12, VMT 00411AD0): $04 PathIndex; $08 Chance. }
    public partial class TROb : Aow.Rtl.TObject
    {
        public int PathIndex;
        // { $04 }
        public int Chance;
        // System.pas:111
        public static Aow.Units.AoWTC.TROb Create()
        {
            return new Aow.Units.AoWTC.TROb();
        }
    }

    // { $08 }
    // { TAoWNewCombatMapSettings (instance size 32, VMT 00411B34)
//     Inherited $04 Width; $08 Height; $0C Levels; $10 HSSFileName; $14 HSMapClassID; $1C MapLevelClassID. }
    public partial class TAoWNewCombatMapSettings : Aow.Units.HSEngine.TNewHSMapSettings
    {
        // AoWTC.pas:2147
        public void Create__init_TAoWNewCombatMapSettings()
        {
            base.Create__init_TNewHSMapSettings();
            HSMapClassID = 2228480;
            MapLevelClassID = 2228482;
            Width = 65;
            Height = 59;
            Levels = 1;
            HSSFileName = Aow.Units.AoWE.AoWE.AoWEngine.HSSFileName;
        }

        // AoWTC.pas:70
        public static new Aow.Units.AoWTC.TAoWNewCombatMapSettings Create()
        {
            Aow.Units.AoWTC.TAoWNewCombatMapSettings o = new Aow.Units.AoWTC.TAoWNewCombatMapSettings();
            try
            {
                o.Create__init_TAoWNewCombatMapSettings();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TaiMove (instance size 32, VMT 00411BC0)
//     $04 MoveType; $06 Ability; $08 Spell; $0C Value; $10 GroupValue;
//     $14 UnitHS; $18 Target; $1C X; $1E Y. }
    public partial class TaiMove : Aow.Rtl.TObject
    {
        public Aow.Units.AoWTC.TaiMoveType MoveType;
        // { $04 }
        public byte F_aiMove_05;
        public short Ability;
        // { $06 }
        public short Spell;
        // { $08 }
        public Aow.Units.FixedArray_byte_2 F_aiMove_0A;
        public int Value;
        // { $0C }
        public int GroupValue;
        // { $10 }
        public Aow.Units.AoWTC.TTacticalCombatUnitHS UnitHS;
        // { $14 }
        public Aow.Units.AoWE.TCombatObject Target;
        // { $18 }
        public ushort X;
        // { $1C }
        public ushort Y;
        // System.pas:111
        public static Aow.Units.AoWTC.TaiMove Create()
        {
            return new Aow.Units.AoWTC.TaiMove();
        }
    }

    // { $1E }
    // { TCAI (instance size 56, VMT 00411C28)
//     $04 FSide; $08 Units; $0C Allies; $10 Enemies; $14 Moves; $18 Walls;
//     $1C CurrentMove; $20 FUnitIndex; $24 FUnitLimit; $28 FMinMoves; $2C FHeroMoves; $30 FNormalUnits; $34 FIgnoreWalls; $35 FFlee; $36 Status. }
    public partial class TCAI : Aow.Rtl.TObject
    {
        public Aow.Units.AoWE.TCombatSideIndex FSide;
        // { $04 }
        public Aow.Units.FixedArray_byte_3 F_CAI_05;
        public Aow.Rtl.TList Units;
        // { $08 }
        public Aow.Rtl.TList Allies;
        // { $0C }
        public Aow.Rtl.TList Enemies;
        // { $10 }
        public Aow.Rtl.TList Moves;
        // { $14 }
        public Aow.Rtl.TList Walls;
        // { $18 }
        public int CurrentMove;
        // { $1C }
        public int FUnitIndex;
        // { $20 }
        public int FUnitLimit;
        // { $24 }
        public int FMinMoves;
        // { $28 }
        public int FHeroMoves;
        // { $2C }
        public int FNormalUnits;
        // { $30 }
        public bool FIgnoreWalls;
        // { $34 }
        public bool FFlee;
        // { $35 }
        public Aow.Units.AoWTC.TaiStatus Status;
        // { $36 }
        public Aow.Units.FixedArray_byte_1 F_CAI_37;
        // AoWTC.pas:762
        public override void Destroy()
        {
            int I = default;
            Units.Free();
            Allies.Free();
            Enemies.Free();
            Walls.Free();
            {
                I = 0;
                int end__1 = Moves.Count - 1;
                for (; I <= end__1; I += 1)
                {
                    var call__2 = Moves[I];
                    ((Aow.Rtl.TObject)call__2).Free();
                }
            }

            Moves.Free();
            base.Destroy();
        }

        // AoWTC.pas:752
        public void Create__init_TCAI()
        {
            ;
            var call__3 = new Aow.Rtl.TList();
            Units = call__3;
            var call__4 = new Aow.Rtl.TList();
            Allies = call__4;
            var call__5 = new Aow.Rtl.TList();
            Enemies = call__5;
            var call__6 = new Aow.Rtl.TList();
            Moves = call__6;
            var call__7 = new Aow.Rtl.TList();
            Walls = call__7;
        }

        // AoWTC.pas:774
        public void Reset()
        {
            Status = Aow.Units.AoWTC.TaiStatus.aiIdle;
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
        public void CreateMove(Aow.Units.AoWTC.TaiMoveType MoveType, int X, int Y, int Value, int Ability, int Spell, Aow.Units.AoWTC.TTacticalCombatUnit U, Aow.Units.AoWE.TCombatObject Target)
        {
            Aow.Units.AoWTC.TaiMove M = default;
            M = Aow.Units.AoWTC.TaiMove.Create();
            M.UnitHS = U.FUnitHS;
            M.Target = Target;
            M.MoveType = MoveType;
            M.Ability = unchecked((short)Ability);
            M.Spell = unchecked((short)Spell);
            M.X = unchecked((ushort)X);
            M.Y = unchecked((ushort)Y);
            M.Value = Value;
            Moves.Add(M);
        }

        // AoWTC.pas:805
        public int tcDVtoDEV(int DV1, int DV2, Aow.Units.AoWE.TCombatObject Source, Aow.Units.AoWE.TCombatObject Target)
        {
            int Result = default;
            int Strength = default;
            Strength = Target.GetTargetStrength();
            if (Target is Aow.Units.AoWTC.TTacticalCombatUnit)
            {
                if (((Aow.Units.AoWTC.TTacticalCombatUnit)Target).FUnit is Aow.Units.AoWE.THero)
                {
                    Strength = Strength * 2;
                }

                if (((Aow.Units.AoWTC.TTacticalCombatUnit)Target).FUnit is Aow.Units.AoWE.TLeader)
                {
                    Strength = Strength * 2;
                }
            }

            Result = Strength * DV1;
            Strength = Source.GetTargetStrength();
            if (Source is Aow.Units.AoWTC.TTacticalCombatUnit)
            {
                if (((Aow.Units.AoWTC.TTacticalCombatUnit)Source).FUnit is Aow.Units.AoWE.THero)
                {
                    Strength = Strength * 2;
                }

                if (((Aow.Units.AoWTC.TTacticalCombatUnit)Source).FUnit is Aow.Units.AoWE.TLeader)
                {
                    Strength = Strength * 2;
                }
            }

            Result = Result - (Strength * DV2);
            Result = Result / 100;
            return Result;
        }

        // AoWTC.pas:823
        public bool ChargeFromPos(int Position)
        {
            bool Result = default;
            int I = default;
            int J = default;
            int Empty = default;
            if (Position == 0)
            {
                Result = false;
                Empty = 0;
                if (((FSide == Aow.Units.AoWE.TCombatSideIndex.csDefender) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallType != 0)) && !Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallBreached)
                {
                    Result = true;
                }
                else
                {
                    {
                        I = 1;
                        byte end__8 = 6;
                        for (; I <= end__8; I += 1)
                        {
                            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPositionStatus[I] == 1)
                            {
                                J = I - 1;
                                if (J == 0)
                                {
                                    J = 6;
                                }

                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPositionStatus[J] == 0)
                                {
                                    Result = true;
                                }

                                J = I + 1;
                                if (J == 7)
                                {
                                    J = 1;
                                }

                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPositionStatus[J] == 0)
                                {
                                    Result = true;
                                }
                            }
                            else
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPositionStatus[I] == 0)
                                {
                                    Empty += 1;
                                }
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
        public int UnitSafe(Aow.Units.AoWTC.TTacticalCombatUnit U, int X, int Y)
        {
            int Result = default;
            int I = default;
            int Distance = default;
            if (!(U.FUnit is Aow.Units.AoWE.THero) || ((Units.Count == 1) && (FSide == Aow.Units.AoWE.TCombatSideIndex.csDefender)))
            {
                Result = 0;
            }
            else
            {
                var call__9 = U.GetTargetStrength();
                var call__10 = U.GetHitPoints();
                var call__11 = U.GetHits();
                Result = ((call__9 * 20) * call__10) / call__11;
                if (U.FUnit is Aow.Units.AoWE.TLeader)
                {
                    Result = Result / 2;
                }

                {
                    I = 0;
                    int end__12 = Enemies.Count - 1;
                    for (; I <= end__12; I += 1)
                    {
                        var call__13 = Enemies[I];
                        var call__14 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__13).FUnitHS.GetYhx();
                        var call__15 = Enemies[I];
                        var call__16 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__15).FUnitHS.GetXhx();
                        var call__17 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, call__16, call__14);
                        Distance = call__17;
                        bool bool__18 = (Distance < 15);
                        if (bool__18)
                        {
                            var call__19 = Enemies[I];
                            var call__20 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__19).GetTargetStrength();
                            bool__18 = (call__20 > 0);
                        }

                        if (bool__18)
                        {
                            var call__21 = Enemies[I];
                            var call__22 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__21).GetTargetStrength();
                            Result = Result - (call__22 * (15 - Distance));
                        }
                    }
                }

                {
                    I = 0;
                    int end__23 = Units.Count - 1;
                    for (; I <= end__23; I += 1)
                    {
                        var call__24 = Units[I];
                        if (((Aow.Units.AoWTC.TTacticalCombatUnit)call__24) != U)
                        {
                            var call__25 = Units[I];
                            var call__26 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__25).FUnitHS.GetYhx();
                            var call__27 = Units[I];
                            var call__28 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__27).FUnitHS.GetXhx();
                            var call__29 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, call__28, call__26);
                            Distance = call__29;
                            bool bool__30 = (Distance < 15);
                            if (bool__30)
                            {
                                var call__31 = Units[I];
                                var call__32 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__31).GetTargetStrength();
                                bool__30 = (call__32 > 0);
                            }

                            if (bool__30)
                            {
                                var call__33 = Units[I];
                                var call__34 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__33).GetTargetStrength();
                                Result = Result + (call__34 * (15 - Distance));
                            }
                        }
                    }
                }

                {
                    I = 0;
                    int end__35 = Allies.Count - 1;
                    for (; I <= end__35; I += 1)
                    {
                        var call__36 = Allies[I];
                        var call__37 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__36).FUnitHS.GetYhx();
                        var call__38 = Allies[I];
                        var call__39 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__38).FUnitHS.GetXhx();
                        var call__40 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, call__39, call__37);
                        Distance = call__40;
                        bool bool__41 = (Distance < 15);
                        if (bool__41)
                        {
                            var call__42 = Allies[I];
                            var call__43 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__42).GetTargetStrength();
                            bool__41 = (call__43 > 0);
                        }

                        if (bool__41)
                        {
                            var call__44 = Allies[I];
                            var call__45 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__44).GetTargetStrength();
                            Result = Result + (call__45 * (15 - Distance));
                        }
                    }
                }
            }

            return Result;
        }

        // AoWTC.pas:880
        public int MakeTerrainPath(Aow.Units.AoWTC.TCombatMoveControlSettings MCS, ref Aow.Units.HSEngine.TXYLList Path, ref int Value, Aow.Units.AoWTC.TTacticalCombatUnit U, Aow.Units.AoWTC.TTacticalCombatUnit Ignore, bool FreeStrikes, bool FindWalls, int Cost)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AoWTC.pas:952
        public int MultDEV(int Value, int Multiplier, int Divider)
        {
            int Result = default;
            Result = Value;
            if (Value >= 0)
            {
                try
                {
                    if (Multiplier != 1)
                    {
                        Result = Result * Multiplier;
                    }

                    if ((Divider != 1) && (Divider != 0))
                    {
                        Result = Result / Divider;
                    }
                }
                catch (System.Exception)
                {
                    try
                    {
                        if ((Divider != 1) && (Divider != 0))
                        {
                            Result = Result / Divider;
                        }

                        if (Multiplier != 1)
                        {
                            Result = Result * Multiplier;
                        }
                    }
                    catch (System.Exception)
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

                    if ((Multiplier != 1) && (Multiplier != 0))
                    {
                        Result = Result / Multiplier;
                    }
                }
                catch (System.Exception)
                {
                    try
                    {
                        if ((Multiplier != 1) && (Multiplier != 0))
                        {
                            Result = Result / Multiplier;
                        }

                        if (Divider != 1)
                        {
                            Result = Result * Divider;
                        }
                    }
                    catch (System.Exception)
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
        public void EvalAll(ref int Value, Aow.Rtl.TList List, Aow.Units.AoWE.TCombatObject Ignore, int Spell, Aow.Units.AoWTC.TTacticalCombatUnit U)
        {
            int I = default;
            Aow.Units.AoWE.TCombatObject Target = default;
            Aow.Units.AoWE.TAbilityDamageValues DV = default;
            {
                I = 0;
                int end__46 = List.Count - 1;
                for (; I <= end__46; I += 1)
                {
                    var call__47 = List[I];
                    if (((Aow.Units.AoWE.TCombatObject)call__47) != Ignore)
                    {
                        var call__48 = List[I];
                        Target = (Aow.Units.AoWE.TCombatObject)call__48;
                        var call__49 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Spell);
                        call__49.GetCombatDamageValueEx(U, Target, ref DV);
                        if (DV.SourceDamage > 0)
                        {
                            switch (Spell)
                            {
                                case 102:
                                    var call__50 = Target.GetSide();
                                    if (call__50 != FSide)
                                    {
                                        var call__51 = tcDVtoDEV(DV.SourceDamage, 0, U, Target);
                                        Value = Value - call__51;
                                    }
                                    else
                                    {
                                        var call__52 = tcDVtoDEV(DV.SourceDamage, 0, U, Target);
                                        Value = Value + call__52;
                                    }

                                    break;
                                default:
                                    var call__53 = Target.GetSide();
                                    if (call__53 != FSide)
                                    {
                                        var call__54 = Target.GetHitPoints();
                                        var call__55 = Target.GetHits();
                                        var call__56 = tcDVtoDEV(DV.SourceDamage, 0, U, Target);
                                        var call__57 = MultDEV(call__56, call__55, call__54);
                                        Value = Value + call__57;
                                    }
                                    else
                                    {
                                        var call__58 = Target.GetHitPoints();
                                        var call__59 = Target.GetHits();
                                        var call__60 = tcDVtoDEV(DV.SourceDamage, 0, U, Target);
                                        var call__61 = MultDEV(call__60, call__59, call__58);
                                        Value = Value - call__61;
                                    }

                                    break;
                            }
                        }
                    }
                }
            }
        }

        // AoWTC.pas:1007
        public void EvalHex(Aow.Units.AoWE.TCombatObject Ignore, int Ability, int Spell, int X, int Y, ref int Value, bool HitWalls, bool Friendly, Aow.Units.AoWTC.TTacticalCombatUnit U, Aow.Rtl.TObject IgnoreWall)
        {
            Aow.Units.AoWTC.TTacticalCombatUnitHS HS = default;
            Aow.Units.CityWall.TCityWall Wall = default;
            Aow.Units.AoWE.TAbilityDamageValues DV = default;
            var call__62 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
            var call__63 = call__62.Field[X, Y];
            var call__64 = call__63.FindOwnedChild(2228484);
            HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__64;
            bool bool__65 = (HS != null) && (HS.FUnit != Ignore);
            if (bool__65)
            {
                bool bool__66 = Friendly;
                if (!bool__66)
                {
                    var call__67 = HS.FUnit.GetSide();
                    var call__68 = U.GetSide();
                    bool__66 = (call__67 != call__68);
                }

                bool__65 = bool__66;
            }

            if (bool__65)
            {
                if (Ability == 52)
                {
                    var call__69 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Spell);
                    call__69.GetCombatDamageValueEx(U, HS.FUnit, ref DV);
                }
                else
                {
                    if (Aow.Units.AoWTC.AoWTC.AbilTypes[Ability] == Aow.Units.AoWTC.TfxType.fxTouch)
                    {
                        var call__70 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                        call__70.tcGetDamageValueEx(U, HS.FUnit, ref DV);
                    }
                    else
                    {
                        var call__71 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                        call__71.tcGetDamageValueEx(U, HS.FUnit, ref DV);
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
                                    var call__72 = HS.FUnit.GetSide();
                                    var call__73 = U.GetSide();
                                    if (call__72 != call__73)
                                    {
                                        var call__74 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                        Value = Value + call__74;
                                    }
                                    else
                                    {
                                        var call__75 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                        Value = Value - call__75;
                                    }

                                    break;
                                default:
                                    var call__76 = HS.FUnit.GetSide();
                                    var call__77 = U.GetSide();
                                    if (call__76 != call__77)
                                    {
                                        var call__78 = HS.FUnit.GetHitPoints();
                                        var call__79 = HS.FUnit.GetHits();
                                        var call__80 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                        var call__81 = MultDEV(call__80, call__79, call__78);
                                        Value = Value + call__81;
                                    }
                                    else
                                    {
                                        var call__82 = HS.FUnit.GetHitPoints();
                                        var call__83 = HS.FUnit.GetHits();
                                        var call__84 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                        var call__85 = MultDEV(call__84, call__83, call__82);
                                        Value = Value - call__85;
                                    }

                                    break;
                            }

                            break;
                        default:
                            var call__86 = HS.FUnit.GetSide();
                            var call__87 = U.GetSide();
                            if (call__86 != call__87)
                            {
                                var call__88 = HS.FUnit.GetHitPoints();
                                var call__89 = HS.FUnit.GetHits();
                                var call__90 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                var call__91 = MultDEV(call__90, call__89, call__88);
                                Value = Value + call__91;
                            }
                            else
                            {
                                var call__92 = HS.FUnit.GetHitPoints();
                                var call__93 = HS.FUnit.GetHits();
                                var call__94 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                var call__95 = MultDEV(call__94, call__93, call__92);
                                Value = Value - call__95;
                            }

                            break;
                    }
                }
            }

            if (HitWalls && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallType != 0))
            {
                var call__96 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__97 = call__96.Field[X, Y];
                var call__98 = call__97.FindOwnedChild(2229508);
                Wall = (Aow.Units.CityWall.TCityWall)call__98;
                if (Wall == null)
                {
                    var call__99 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__100 = call__99.Field[X, Y];
                    var call__101 = call__100.FindOwnedChild(2229510);
                    Wall = (Aow.Units.CityWall.TCityWall)call__101;
                }

                if (((!Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallBreached && (IgnoreWall != null)) && (Wall != null)) && (((Ignore == null) || !(Ignore is Aow.Units.AoWE.TCombatWall)) || ((Ignore is Aow.Units.AoWE.TCombatWall) && (IgnoreWall != Wall))))
                {
                    if (Ability == 52)
                    {
                        var call__102 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Spell);
                        call__102.GetCombatDamageValueEx(U, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall, ref DV);
                    }
                    else
                    {
                        if (Aow.Units.AoWTC.AoWTC.AbilTypes[Ability] == Aow.Units.AoWTC.TfxType.fxTouch)
                        {
                            var call__103 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                            call__103.tcGetDamageValueEx(U, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall, ref DV);
                        }
                        else
                        {
                            var call__104 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                            call__104.tcGetDamageValueEx(U, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall, ref DV);
                        }
                    }

                    if (Ability == 65)
                    {
                        DV.TargetDamage = 0;
                    }

                    if (DV.SourceDamage > 0)
                    {
                        if (U.GetSide() == Aow.Units.AoWE.TCombatSideIndex.csAttacker)
                        {
                            var call__105 = tcDVtoDEV(DV.SourceDamage, 0, U, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall);
                            var call__106 = MultDEV(call__105, Aow.Units.AoWTC.AoWTC.WallMaxHP[Wall.FWallType - 1], Wall.FHitPoints);
                            Value = Value + call__106;
                        }
                        else
                        {
                            var call__107 = tcDVtoDEV(DV.SourceDamage, 0, U, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall);
                            var call__108 = MultDEV(call__107, Aow.Units.AoWTC.AoWTC.WallMaxHP[Wall.FWallType - 1], Wall.FHitPoints);
                            Value = Value - call__108;
                        }
                    }
                }
            }
        }

        // AoWTC.pas:1072
        public void EvalArea(ref int Value, Aow.Units.AoWE.TCombatObject Ignore, int Ability, int Spell, int X, int Y, bool HitWalls, bool IncludeCenter, bool Friendly, Aow.Units.AoWTC.TTacticalCombatUnit U, Aow.Rtl.TObject IgnoreWall)
        {
            int I = default;
            int X1 = default;
            int Y1 = default;
            {
                I = 0;
                byte end__109 = 6;
                for (; I <= end__109; I += 1)
                {
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref X1, ref Y1);
                    bool bool__110 = IncludeCenter;
                    if (!bool__110)
                    {
                        var call__111 = U.FUnitHS.GetXhx();
                        bool__110 = (call__111 != X1);
                    }

                    bool bool__112 = bool__110;
                    if (!bool__112)
                    {
                        var call__113 = U.FUnitHS.GetYhx();
                        bool__112 = (call__113 != Y1);
                    }

                    if (bool__112)
                    {
                        EvalHex(Ignore, Ability, Spell, X1, Y1, ref Value, HitWalls, Friendly, U, IgnoreWall);
                    }
                }
            }
        }

        // AoWTC.pas:1084
        public int EvalPath(ref int Value, Aow.Units.AoWE.TCombatObject Ignore, int Ability, int Spell, int X1, int Y1, int X2, int Y2, bool HitWalls, Aow.Units.AoWTC.TTacticalCombatUnit U, Aow.Rtl.TObject IgnoreWall)
        {
            int Result = default;
            Aow.Units.aowFX.TXYZList Path = default;
            Aow.Rtl.TList Obstacles = default;
            int I = default;
            int X = default;
            int Y = default;
            int V = default;
            int Total = default;
            int Original = default;
            int Chance = default;
            Aow.Units.AoWTC.TfxType FX = default;
            if (Ability == 52)
            {
                FX = Aow.Units.AoWTC.AoWTC.SpellTypes[Spell];
            }
            else
            {
                FX = Aow.Units.AoWTC.AoWTC.AbilTypes[Ability];
            }

            switch (FX)
            {
                case Aow.Units.AoWTC.TfxType.fxArrow:
                case Aow.Units.AoWTC.TfxType.fxRock:
                case Aow.Units.AoWTC.TfxType.fxComet:
                case Aow.Units.AoWTC.TfxType.fxBreath:
                case Aow.Units.AoWTC.TfxType.fxCometBall:
                case Aow.Units.AoWTC.TfxType.fxRay:
                case Aow.Units.AoWTC.TfxType.fxAltArrow:
                    Path = Aow.Units.aowFX.TXYZList.Create();
                    Obstacles = new Aow.Rtl.TList();
                    if (FX == Aow.Units.AoWTC.TfxType.fxBreath)
                    {
                        var call__114 = U.GetSide();
                        Aow.Units.AoWTC.AoWTC.MakeRangedPath(call__114, 50, 20, 50, unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), 0, 0, false, ref Path, ref Obstacles);
                    }
                    else
                    {
                        var call__115 = U.GetSide();
                        Aow.Units.AoWTC.AoWTC.MakeAbilPath(call__115, unchecked((short)Ability), unchecked((short)Spell), unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), ref Path, ref Obstacles);
                    }

                    Total = 0;
                    Original = Value;
                    Chance = 100;
                {
                    I = 0;
                    int end__116 = Obstacles.Count - 1;
                    for (; I <= end__116; I += 1)
                    {
                        var call__117 = Obstacles[I];
                        var call__118 = Path[((Aow.Units.AoWTC.TROb)call__117).PathIndex];
                        var call__119 = Obstacles[I];
                        var call__120 = Path[((Aow.Units.AoWTC.TROb)call__119).PathIndex];
                        Aow.Units.HSEngine.HSEngine.HPtoHX(call__120.X, call__118.Y, ref X, ref Y);
                        V = 0;
                        EvalHex(Ignore, Ability, Spell, X, Y, ref V, HitWalls, true, U, IgnoreWall);
                        try
                        {
                            var call__122 = Obstacles[I];
                            Total = Total + ((((V * Chance) / 100) * ((Aow.Units.AoWTC.TROb)call__122).Chance) / 100);
                        }
                        catch (System.Exception)
                        {
                            var call__121 = Obstacles[I];
                            Total = Total + (((Aow.Units.AoWTC.TROb)call__121).Chance * (((V / 100) * Chance) / 100));
                        }

                        var call__123 = Obstacles[I];
                        Chance = ((100 - ((Aow.Units.AoWTC.TROb)call__123).Chance) * Chance) / 100;
                    }
                }

                    try
                    {
                        Value = (Value * Chance) / 100;
                    }
                    catch (System.Exception)
                    {
                        Value = (Value / 100) * Chance;
                    }

                    Value = Value + Total;
                    Path.Free();
                {
                    I = 0;
                    int end__124 = Obstacles.Count - 1;
                    for (; I <= end__124; I += 1)
                    {
                        var call__125 = Obstacles[I];
                        ((Aow.Rtl.TObject)call__125).Free();
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
        public int EvalBreath(ref int Value, Aow.Units.AoWE.TCombatObject Ignore, int Ability, int Spell, int X1, int Y1, int X2, int Y2, Aow.Units.AoWTC.TTacticalCombatUnit U)
        {
            int Result = default;
            int I = default;
            int V = default;
            int X = default;
            int Y = default;
            var call__126 = U.FUnitHS.GetXhx();
            var call__127 = U.FUnitHS.GetYhx();
            var call__128 = EvalPath(ref Value, Ignore, Ability, Spell, call__126, call__127, X2, Y2, false, U, null);
            Result = call__128;
            {
                I = 0;
                byte end__129 = 11;
                for (; I <= end__129; I += 1)
                {
                    V = 0;
                    var call__130 = Aow.Units.HSEngine.HSEngine.dHXtoHN(X1, Y1, X2, Y2);
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X1, Y1, Aow.Units.AoWTC.AoWTC.BreathHN[((Aow.Units.AoWTC.AoWTC.BreathDirection[call__130 - 1] * 12) + I) - 12], ref X, ref Y);
                    EvalHex(Ignore, Ability, Spell, X, Y, ref V, true, true, U, null);
                    if (V != 0)
                    {
                        var call__131 = U.FUnitHS.GetXhx();
                        var call__132 = U.FUnitHS.GetYhx();
                        EvalPath(ref V, Ignore, Ability, Spell, call__131, call__132, X, Y, false, U, null);
                    }

                    Value = Value + V;
                    Result = Result - V;
                }
            }

            return Result;
        }

        // AoWTC.pas:1149
        public void CheckUnit(int X, int Y, int Hits, int HitPoints, Aow.Units.AoWTC.TCombatMoveControlSettings Settings, Aow.Units.AoWTC.TTacticalCombatUnit U, Aow.Units.AoWE.TCombatObject Target, Aow.Rtl.TList Abilities, Aow.Rtl.TObject Wall)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AoWTC.pas:1628
        public void EvalBattle()
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AoWTC.pas:114
        public static Aow.Units.AoWTC.TCAI Create()
        {
            Aow.Units.AoWTC.TCAI o = new Aow.Units.AoWTC.TCAI();
            try
            {
                o.Create__init_TCAI();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TAoWCombatMapField (instance size 28, VMT 00411C8C)
//     Inherited world-map field layout; no additional fields. }
    public partial class TAoWCombatMapField : Aow.Units.AoWE.TAoWMapField
    {
        // AoWTC.pas:2163
        public sbyte GetDefenseValue()
        {
            sbyte Result = default;
            Aow.Units.HSEngine.TMapFieldMsg Msg = default;
            Msg.MsgID = 135169;
            Msg.Result = -1;
            Msg.Param[0] = 0;
            SendMapFieldMsg(ref Msg);
            Result = unchecked((sbyte)Msg.Param[0].Value);
            return Result;
        }

        public new partial class Meta : Aow.Units.AoWE.TAoWMapField.Meta
        {
            // AoWTC.pas:2158
            public override int ClassID()
            {
                int Result = default;
                Result = 2228481;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AoWTC.TAoWCombatMapField => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:219
            public new Aow.Units.AoWTC.TAoWCombatMapField Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AoWTC.TAoWCombatMapField o = (Aow.Units.AoWTC.TAoWCombatMapField)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TAoWCombatMapLevel (instance size 128, VMT 00411D98)
//     Inherited world-map level layout; no additional fields. }
    public partial class TAoWCombatMapLevel : Aow.Units.AoWE.TAoWMapLevel
    {
        // AoWTC.pas:2178
        public override Aow.Units.Engine.TEObject.Meta MapFieldClass()
        {
            Aow.Units.Engine.TEObject.Meta Result = default;
            // { Original uses the world-map field class. }
            Result = Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWE.TAoWMapField>();
            return Result;
        }

        public new partial class Meta : Aow.Units.AoWE.TAoWMapLevel.Meta
        {
            // AoWTC.pas:2173
            public override int ClassID()
            {
                int Result = default;
                Result = 2228482;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AoWTC.TAoWCombatMapLevel => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:312
            public new Aow.Units.AoWTC.TAoWCombatMapLevel Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AoWTC.TAoWCombatMapLevel o = (Aow.Units.AoWTC.TAoWCombatMapLevel)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TRangedPathHS (instance size 32, VMT 00411EA8): $0C FMiscImages; $10 FTargetImages; $14 FFrame; $16 FFrameCount; $18 FSelection; $1C FRange. }
    public partial class TRangedPathHS : Aow.Units.HSEngine.TFixedVisibleHS
    {
        public Aow.Units.ImageLib.TCustomImageLibrary FMiscImages;
        // { $0C }
        public Aow.Units.ImageLib.TCustomImageLibrary FTargetImages;
        // { $10 }
        public ushort FFrame;
        public ushort FFrameCount;
        public Aow.Units.AoWTC.TCombatUnitSelectionControl FSelection;
        // { $18 }
        public ushort FRange;
        public Aow.Units.FixedArray_byte_2 F_RangedPathHS_1E;
        // AoWTC.pas:4849
        public override int GetShowPriority()
        {
            int Result = default;
            Result = 1811939328;
            return Result;
        }

        // AoWTC.pas:4859
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int I = default;
            int X = default;
            int Y = default;
            int Count = default;
            int First = default;
            int Last = default;
            int NX = default;
            int NY = default;
            int PX = default;
            int PY = default;
            Aow.Units.AoWTC.TfxType EffectType = default;
            if (FSelection != null)
            {
                if (FRange < 100)
                {
                    Count = 6;
                    First = 1;
                    {
                        I = 0;
                        int end__133 = FRange - 2;
                        for (; I <= end__133; I += 1)
                        {
                            First += Count;
                            Count += 6;
                        }
                    }

                    Last = First + Count;
                    {
                        I = First;
                        int end__134 = Last - 1;
                        for (; I <= end__134; I += 1)
                        {
                            var call__135 = FSelection.GetTCUnitHS();
                            var call__136 = call__135.GetYhx();
                            var call__137 = FSelection.GetTCUnitHS();
                            var call__138 = call__137.GetXhx();
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__138, call__136, I, ref NX, ref NY);
                            Aow.Units.HSEngine.HSEngine.HXtoHP(NX, NY, ref PX, ref PY);
                            var call__139 = FTargetImages[2];
                            call__139.Show((PX - XhpSurf) - 8, (PY - YhpSurf) + 2, SurfaceDesc);
                        }
                    }
                }

                var call__140 = FSelection.FRangedPath.Count;
                if (call__140 > 0)
                {
                    FFrameCount = (ushort)(((int)FFrameCount) + 1);
                    if (FFrameCount >= 5)
                    {
                        FFrameCount = 0;
                        FFrame = (ushort)(((int)FFrame) + 1);
                    }

                    if (FFrame > 1)
                    {
                        FFrame = 0;
                    }

                    var call__141 = FSelection.GetTCUnitHS();
                    if (call__141.FSelectedAbility == 52)
                    {
                        var call__142 = FSelection.GetTCUnitHS();
                        EffectType = Aow.Units.AoWTC.AoWTC.SpellTypes[call__142.FSelectedSpell];
                    }
                    else
                    {
                        var call__143 = FSelection.GetTCUnitHS();
                        EffectType = Aow.Units.AoWTC.AoWTC.AbilTypes[call__143.FSelectedAbility];
                    }

                    switch (EffectType)
                    {
                        case Aow.Units.AoWTC.TfxType.fxBreath:
                            var call__144 = FSelection.FRangedPath.Count;
                            var call__145 = FSelection.FRangedPath[call__144 - 1];
                            var call__146 = FSelection.FRangedPath.Count;
                            var call__147 = FSelection.FRangedPath[call__146 - 1];
                            Aow.Units.HSEngine.HSEngine.HPtoHX(call__147.X, call__145.Y, ref X, ref Y);
                        {
                            I = 0;
                            byte end__148 = 11;
                            for (; I <= end__148; I += 1)
                            {
                                var call__149 = FSelection.GetTCUnitHS();
                                var call__150 = call__149.GetYhx();
                                var call__151 = FSelection.GetTCUnitHS();
                                var call__152 = call__151.GetXhx();
                                var call__153 = Aow.Units.HSEngine.HSEngine.dHXtoHN(call__152, call__150, X, Y);
                                var call__154 = FSelection.GetTCUnitHS();
                                var call__155 = call__154.GetYhx();
                                var call__156 = FSelection.GetTCUnitHS();
                                var call__157 = call__156.GetXhx();
                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__157, call__155, Aow.Units.AoWTC.AoWTC.BreathHN[((call__153 * 12) + I) - 12], ref NX, ref NY);
                                Aow.Units.HSEngine.HSEngine.HXtoHP(NX, NY, ref PX, ref PY);
                                var call__158 = FTargetImages[0];
                                call__158.ShowClipped((PX - 8) - XhpSurf, (PY + 2) - YhpSurf, SurfaceDesc);
                            }
                        }

                            break;
                        default:
                            var call__159 = FSelection.GetTCUnitHS();
                            switch (call__159.FSelectedAbility)
                            {
                                case 52:
                                    var call__160 = FSelection.GetTCUnitHS();
                                    switch (call__160.FSelectedSpell)
                                    {
                                        case 104:
                                        case 106:
                                        case 116:
                                        case 117:
                                        case 120:
                                        case 129:
                                        case 130:
                                        {
                                            I = 0;
                                            byte end__161 = 18;
                                            for (; I <= end__161; I += 1)
                                            {
                                                var call__162 = FSelection.FRangedPath.Count;
                                                var call__163 = FSelection.FRangedPath[call__162 - 1];
                                                var call__164 = FSelection.FRangedPath.Count;
                                                var call__165 = FSelection.FRangedPath[call__164 - 1];
                                                Aow.Units.HSEngine.HSEngine.HPtoHX(call__165.X, call__163.Y, ref X, ref Y);
                                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                Aow.Units.HSEngine.HSEngine.HXtoHP(NX, NY, ref PX, ref PY);
                                                var call__166 = FTargetImages[0];
                                                call__166.ShowClipped((PX - 8) - XhpSurf, (PY + 2) - YhpSurf, SurfaceDesc);
                                            }
                                        }

                                            break;
                                        case 124:
                                        case 128:
                                        {
                                            I = 0;
                                            byte end__167 = 6;
                                            for (; I <= end__167; I += 1)
                                            {
                                                var call__168 = FSelection.FRangedPath.Count;
                                                var call__169 = FSelection.FRangedPath[call__168 - 1];
                                                var call__170 = FSelection.FRangedPath.Count;
                                                var call__171 = FSelection.FRangedPath[call__170 - 1];
                                                Aow.Units.HSEngine.HSEngine.HPtoHX(call__171.X, call__169.Y, ref X, ref Y);
                                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                Aow.Units.HSEngine.HSEngine.HXtoHP(NX, NY, ref PX, ref PY);
                                                var call__172 = FTargetImages[0];
                                                call__172.ShowClipped((PX - 8) - XhpSurf, (PY + 2) - YhpSurf, SurfaceDesc);
                                            }
                                        }

                                            break;
                                        default:
                                            var call__173 = FSelection.FRangedPath.Count;
                                            var call__174 = FSelection.FRangedPath[call__173 - 1];
                                            var call__175 = FSelection.FRangedPath.Count;
                                            var call__176 = FSelection.FRangedPath[call__175 - 1];
                                            var call__177 = FMiscImages[FFrame + 3];
                                            call__177.ShowClipped((call__176.X - 21) - XhpSurf, (call__174.Y - 20) - YhpSurf, SurfaceDesc);
                                            break;
                                    }

                                    break;
                                default:
                                    var call__178 = FSelection.FRangedPath.Count;
                                    var call__179 = FSelection.FRangedPath[call__178 - 1];
                                    var call__180 = FSelection.FRangedPath.Count;
                                    var call__181 = FSelection.FRangedPath[call__180 - 1];
                                    var call__182 = FMiscImages[FFrame + 3];
                                    call__182.ShowClipped((call__181.X - 21) - XhpSurf, (call__179.Y - 20) - YhpSurf, SurfaceDesc);
                                    break;
                            }

                            break;
                    }

                    {
                        I = 0;
                        int end__183 = FSelection.FRangedPath.Count - 1;
                        for (; I <= end__183; I += 1)
                        {
                            var call__184 = FSelection.FRangedPath[I];
                            var call__185 = FSelection.FRangedPath[I];
                            var call__186 = FMiscImages[2];
                            call__186.Show(call__185.X - XhpSurf, call__184.Y - YhpSurf, SurfaceDesc);
                        }
                    }

                    {
                        I = 0;
                        int end__187 = FSelection.FRangedPath.Count - 1;
                        for (; I <= end__187; I += 1)
                        {
                            var call__188 = FSelection.FRangedPath[I];
                            var call__189 = FSelection.FRangedPath[I];
                            var call__190 = FSelection.FRangedPath[I];
                            var call__191 = FMiscImages[0];
                            call__191.Show(call__190.X - XhpSurf, ((call__188.Y - call__189.Z) - 8) - YhpSurf, SurfaceDesc);
                        }
                    }

                    {
                        I = 0;
                        int end__192 = FSelection.FObstacles.Count - 1;
                        for (; I <= end__192; I += 1)
                        {
                            var call__193 = FSelection.FObstacles[I];
                            var call__194 = FSelection.FRangedPath[((Aow.Units.AoWTC.TROb)call__193).PathIndex];
                            var call__195 = FSelection.FObstacles[I];
                            var call__196 = FSelection.FRangedPath[((Aow.Units.AoWTC.TROb)call__195).PathIndex];
                            Aow.Units.HSEngine.HSEngine.HPtoHX(call__196.X, call__194.Y, ref NX, ref NY);
                            Aow.Units.HSEngine.HSEngine.HXtoHP(NX, NY, ref PX, ref PY);
                            var call__197 = FMiscImages[1];
                            call__197.Show((PX + 5) - XhpSurf, (PY + 5) - YhpSurf, SurfaceDesc);
                            Aow.Units.AoWE.AoWE.AoWEngine.FCombatFont.Color = 0;
                            var call__198 = FSelection.FObstacles[I];
                            Aow.Units.AoWE.AoWE.AoWEngine.FCombatFont.SetTextCentered((PX + 16) - XhpSurf, (PY + 12) - YhpSurf, Aow.Rtl.Rtl.IntToStr(((Aow.Units.AoWTC.TROb)call__198).Chance), SurfaceDesc);
                        }
                    }
                }
            }
        }

        // AoWTC.pas:4852
        public override void Activate()
        {
            base.Activate();
            var call__199 = Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TCombat\\Misc.ILB");
            FMiscImages = call__199;
            var call__200 = Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TargHex.ILB");
            FTargetImages = call__200;
        }

        // AoWTC.pas:4839
        public override void Deactivate()
        {
            Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TCombat\\Misc.ILB");
            Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TargHex.ILB");
            base.Deactivate();
        }

        // Engine.pas:524
        public static new Aow.Units.AoWTC.TRangedPathHS Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWTC.TRangedPathHS o = new Aow.Units.AoWTC.TRangedPathHS();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.HSEngine.THexagonSprite.Meta
        {
            // AoWTC.pas:4846
            public override int ClassID()
            {
                int Result = default;
                Result = 2228485;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AoWTC.TRangedPathHS => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.AoWTC.TRangedPathHS Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AoWTC.TRangedPathHS o = (Aow.Units.AoWTC.TRangedPathHS)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TfxHS (instance size 56, VMT 00412028)
//     $0C FDone; $0D FCancelled; $10 FFrame; $14 FAbilityIndex; $18 FSpellIndex;
//     $1C FPath; $20 FVariant; $24 FTargetX; $28 FTargetY; $2C FTarget; $34 FEffectType. }
    public partial class TfxHS : Aow.Units.HSEngine.TFixedVisibleHS
    {
        public bool FDone;
        // { $0C }
        public bool FCancelled;
        // { $0D }
        // { conflict: FHit: Boolean; }
        public Aow.Units.FixedArray_byte_2 F_fxHS_0E;
        public int FFrame;
        // { $10 }
        public int FAbilityIndex;
        // { $14 }
        // { conflict: FAbilityID: Integer; }
        public int FSpellIndex;
        // { $18 }
        // { conflict: FSpellID: Integer; }
        public Aow.Units.aowFX.TXYZList FPath;
        // { $1C }
        public int FVariant;
        // { $20 }
        // { conflict: FDirection: Integer; }
        public int FTargetX;
        // { $24 }
        public int FTargetY;
        // { $28 }
        public Aow.Units.AoWE.TCombatObject FTarget;
        // { $2C }
        // { conflict: FTarget: TTacticalCombatUnit; }
        public Aow.Units.AoWE.TCombatAction FCombatAction;
        // { $30; $40c44b }
        public Aow.Units.AoWTC.TfxType FEffectType;
        // { $34 }
        public Aow.Units.FixedArray_byte_3 F_fxHS_35;
        // AoWTC.pas:5221
        public override void Destroy()
        {
            FPath.Free();
            base.Destroy();
        }

        // AoWTC.pas:5214
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__201 = Aow.Units.aowFX.TXYZList.Create();
            FPath = call__201;
            FFrame = -1;
        }

        // AoWTC.pas:5239
        public override int GetShowPriority()
        {
            int Result = default;
            Result = 1811939328;
            return Result;
        }

        // AoWTC.pas:6076
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AoWTC.pas:5232
        public override void Activate()
        {
            base.Activate();
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.NewFrameEvents.Add(NewFrame);
            FFrame = -1;
        }

        // AoWTC.pas:5208
        public override void Deactivate()
        {
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.NewFrameEvents.Remove(NewFrame);
            base.Deactivate();
        }

        // AoWTC.pas:5244
        public void NewFrame(Aow.Rtl.TObject Sender)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AoWTC.pas:196
        public static new Aow.Units.AoWTC.TfxHS Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWTC.TfxHS o = new Aow.Units.AoWTC.TfxHS();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.HSEngine.THexagonSprite.Meta
        {
            // AoWTC.pas:5227
            public override int ClassID()
            {
                int Result = default;
                Result = 2228486;
                return Result;
            }

            // AoWTC.pas:196
            public new Aow.Units.AoWTC.TfxHS Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AoWTC.TfxHS o = (Aow.Units.AoWTC.TfxHS)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AoWTC.TfxHS => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TCItemHS (instance size 20, VMT 00412198) }
    public partial class TCItemHS : Aow.Units.AoWE.TItemHS
    {
        // AoWTC.pas:2586
        public override void Deactivate()
        {
            Aow.Units.AoWE.TItemHS ItemHS = default;
            Aow.Units.Engine.TECustomNode Items = default;
            int I = default;
            FItems.FOnChanged = null;
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FExplorationSite == 1)
            {
                Items = (Aow.Units.Engine.TECustomNode)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FItems;
                {
                    I = FItems.GetCount() - 1;
                    byte end__202 = 0;
                    for (; I >= end__202; I -= 1)
                    {
                        var call__203 = FItems.GetItem(I);
                        call__203.SetOwner(Items);
                    }
                }
            }
            else
            {
                var call__204 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL);
                var call__205 = call__204.FindOwnedChild(131697);
                ItemHS = (Aow.Units.AoWE.TItemHS)call__205;
                if (ItemHS == null)
                {
                    ItemHS = Aow.Units.AoWE.TItemHS.Create(null);
                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(ItemHS, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL);
                }

                {
                    I = FItems.GetCount() - 1;
                    byte end__206 = 0;
                    for (; I >= end__206; I -= 1)
                    {
                        var call__207 = FItems.GetItem(I);
                        call__207.SetOwner(ItemHS.FItems);
                    }
                }
            }

            base.Deactivate();
        }

        // intf_P2_04.inc:216
        public static new Aow.Units.AoWTC.TCItemHS Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWTC.TCItemHS o = new Aow.Units.AoWTC.TCItemHS();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TUnitPositionHS (instance size 28, VMT 00412314)
//     $0C FPosition; $0D FIndex; $10 FImageIndex; $14 FImages; $18 FDirection. }
    public partial class TUnitPositionHS : Aow.Units.HSEngine.TSingleHS
    {
        public sbyte FPosition;
        // { $0C }
        public sbyte FIndex;
        // { $0D }
        public Aow.Units.FixedArray_byte_2 F_UnitPositionHS_0E;
        public int FImageIndex;
        // { $10 }
        public Aow.Units.ImageLib.TCustomImageLibrary FImages;
        // { $14 }
        public byte FDirection;
        // { $18 }
        public Aow.Units.FixedArray_byte_3 F_UnitPositionHS_19;
        // AoWTC.pas:5060
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPosition), 25);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FIndex), 26);
            Stream.rwByte(ref FDirection, 27);
        }

        // AoWTC.pas:5008
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FPosition = 0;
            FIndex = 0;
            FDirection = 1;
        }

        // AoWTC.pas:5021
        public override byte GetLevel()
        {
            byte Result = default;
            Result = 27;
            return Result;
        }

        // AoWTC.pas:5074
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref X, ref Y);
            var call__208 = FImages[FImageIndex];
            call__208.ShowClipped((X - XhpSurf) - 8, (Y - YhpSurf) - 8, SurfaceDesc);
        }

        // AoWTC.pas:5031
        public override void Activate()
        {
            Aow.Units.HSEngine.TXYL P = default;
            base.Activate();
            bool bool__209 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting);
            if (bool__209)
            {
                var call__210 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.ValidatePosition(ref FPosition, ref FIndex);
                bool__209 = call__210;
            }

            if (bool__209)
            {
                var call__211 = GetYhx();
                var call__212 = GetXhx();
                var call__213 = Aow.Units.HSEngine.HSEngine.PackXYL(unchecked((sbyte)call__212), unchecked((sbyte)call__211), 0);
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.SetUnitPosition(FPosition, FIndex, call__213);
                var call__214 = Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TCUNITS.ILB");
                FImages = call__214;
                UpdateImageIndex();
            }
            else
            {
                var call__215 = GetXhx();
                P.X = unchecked((sbyte)call__215);
                var call__216 = GetYhx();
                P.Y = unchecked((sbyte)call__216);
                P.L = 0;
                P.Flag = unchecked((sbyte)FDirection);
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.SetUnitPosition(FPosition, FIndex, P);
                Destroy();
            }
        }

        // AoWTC.pas:5049
        public override void Deactivate()
        {
            var call__217 = Activated();
            if (call__217 && ((Aow.Units.HSEngine.TMapLevel)((Aow.Units.HSEngine.TMapField)Owner).Owner).Map.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TCUNITS.ILB");
                var call__218 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(FPosition, FIndex);
                var call__219 = GetXhx();
                bool bool__220 = (call__218.X == call__219);
                if (bool__220)
                {
                    var call__221 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(FPosition, FIndex);
                    var call__222 = GetYhx();
                    bool__220 = (call__221.Y == call__222);
                }

                if (bool__220)
                {
                    var call__223 = Aow.Units.HSEngine.HSEngine.PackXYL(unchecked((sbyte)-1), unchecked((sbyte)-1), unchecked((sbyte)-1));
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.SetUnitPosition(FPosition, FIndex, call__223);
                }
            }

            base.Deactivate();
        }

        // AoWTC.pas:5068
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            var call__224 = Map.GetField(Xhx, Yhx, Lhx);
            var call__225 = call__224.FindOwnedChild(2228483);
            bool bool__226 = (call__225 == null);
            if (bool__226)
            {
                var call__227 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.ValidatePosition(ref FPosition, ref FIndex);
                bool__226 = call__227;
            }

            Result = bool__226;
            return Result;
        }

        // AoWTC.pas:5026
        public void UpdateImageIndex()
        {
            FImageIndex = (FPosition * 8) + FIndex;
        }

        // AoWTC.pas:223
        public static new Aow.Units.AoWTC.TUnitPositionHS Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWTC.TUnitPositionHS o = new Aow.Units.AoWTC.TUnitPositionHS();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.HSEngine.THexagonSprite.Meta
        {
            // AoWTC.pas:5016
            public override int ClassID()
            {
                int Result = default;
                Result = 2228483;
                return Result;
            }

            // AoWTC.pas:223
            public new Aow.Units.AoWTC.TUnitPositionHS Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AoWTC.TUnitPositionHS o = (Aow.Units.AoWTC.TUnitPositionHS)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AoWTC.TUnitPositionHS => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TCBloodHS (instance size 28, VMT 0041249C)
//     $10 FFrame; $14 FOffsetX; $18 FOffsetY. }
    public partial class TCBloodHS : Aow.Units.HSEngine.TSingleHS
    {
        public Aow.Units.FixedArray_byte_4 F_CBloodHS_0C;
        public int FFrame;
        // { $10 }
        public int FOffsetX;
        // { $14 }
        public int FOffsetY;
        // AoWTC.pas:6393
        public override byte GetLevel()
        {
            byte Result = default;
            Result = 26;
            return Result;
        }

        // AoWTC.pas:6398
        public override int GetShowPriority()
        {
            int Result = default;
            Result = (1744830464 + (FOffsetY << 13)) + FOffsetX;
            return Result;
        }

        // AoWTC.pas:6403
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Frame = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int X = default;
            int Y = default;
            var call__228 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[4];
            Sequence = call__228;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref X, ref Y);
            if (Sequence != null)
            {
                var call__229 = Sequence.GetUnloopedFrameIndex(FFrame);
                Frame = call__229;
                var call__230 = Sequence.ImageLibrary[Frame];
                call__230.ShowClipped((X - XhpSurf) + FOffsetX, (Y - YhpSurf) + FOffsetY, SurfaceDesc);
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
        public static new Aow.Units.AoWTC.TCBloodHS Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWTC.TCBloodHS o = new Aow.Units.AoWTC.TCBloodHS();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.HSEngine.THexagonSprite.Meta
        {
            // AoWTC.pas:6388
            // { $18 }
            public override int ClassID()
            {
                int Result = default;
                Result = 2228487;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AoWTC.TCBloodHS => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.AoWTC.TCBloodHS Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AoWTC.TCBloodHS o = (Aow.Units.AoWTC.TCBloodHS)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TCCorpseHS (instance size 44, VMT 0041261C)
//     $0C FUnit; $10 FUnitGFX; $18 FFrame; $1C FOffsetX; $20 FOffsetY; $24 FSequenceIndex; $28 FSequenceFrame. }
    public partial class TCCorpseHS : Aow.Units.HSEngine.TSingleHS
    {
        public Aow.Units.AoWTC.TTacticalCombatUnit FUnit;
        // { $0C }
        public Aow.Units.AoWE.TUnitGFXResource FUnitGFX;
        // { $10 }
        public Aow.Units.FixedArray_byte_4 F_CCorpseHS_14;
        public int FFrame;
        // { $18 }
        public int FOffsetX;
        // { $1C }
        public int FOffsetY;
        // { $20 }
        public int FSequenceIndex;
        // { $24 }
        public int FSequenceFrame;
        // AoWTC.pas:4007
        public override byte GetLevel()
        {
            byte Result = default;
            Result = 26;
            return Result;
        }

        // AoWTC.pas:4012
        public override int GetShowPriority()
        {
            int Result = default;
            Result = (1744830464 + (FOffsetY << 13)) + FOffsetX;
            return Result;
        }

        // AoWTC.pas:3985
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Frame = default;
            int X = default;
            int Y = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref X, ref Y);
            XhpSurf = (X - XhpSurf) - 15;
            YhpSurf = (Y - YhpSurf) - 25;
            if (FSequenceIndex > -1)
            {
                var call__231 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[FSequenceIndex];
                Sequence = call__231;
                while (Sequence != null)
                {
                    var call__232 = Sequence.GetUnloopedFrameIndex(FSequenceFrame);
                    Frame = call__232;
                    if (Frame != -1)
                    {
                        var call__233 = Sequence.ImageLibrary[Frame];
                        call__233.Show((XhpSurf + FOffsetX) - 5, (YhpSurf + FOffsetY) + 10, SurfaceDesc);
                    }

                    Sequence = Sequence.Next;
                }
            }

            var call__234 = FUnitGFX.FSequences.ImageSequences[30];
            Sequence = call__234;
            while (Sequence != null)
            {
                var call__235 = Sequence.GetUnloopedFrameIndex(FFrame);
                Frame = call__235;
                if (Frame != -1)
                {
                    var call__236 = Sequence.ImageLibrary[Frame];
                    call__236.Show(XhpSurf + FOffsetX, YhpSurf + FOffsetY, SurfaceDesc);
                }

                Sequence = Sequence.Next;
            }
        }

        // AoWTC.pas:4025
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            Aow.Units.AoWE.TCombatAction AnimateAction = default;
            Aow.Units.AoWE.TCombatAction RaiseAction = default;
            int I = default;
            int N = default;
            int Ring = default;
            int X = default;
            int Y = default;
            int DX = default;
            int DY = default;
            Aow.Units.AoWTC.TfxHS Effect = default;
            float Z = default;
            float Speed = default;
            float VZ = default;
            float Gravity = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS UnitHS = default;
            base.MapFieldMsgProc(ref Msg);
            if (FUnit != null)
            {
                if ((Msg.MsgID == 200712) && (Msg.HN == 0))
                {
                    var call__237 = GetYhx();
                    var call__238 = GetXhx();
                    var call__239 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__240 = call__239.Field[call__238, call__237];
                    var call__241 = call__240.FindOwnedChild(2228484);
                    if (call__241 == null)
                    {
                        var call__242 = GetYhx();
                        var call__243 = GetXhx();
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(call__243, call__242, 0, 5);
                        var call__244 = GetYhx();
                        var call__245 = GetXhx();
                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__245, call__244, ref X, ref Y);
                        {
                            Ring = 1;
                            byte end__246 = 3;
                            for (; Ring <= end__246; Ring += 1)
                            {
                                N = Aow.Rtl.Rtl.Random(5);
                                DX = Aow.Rtl.Rtl.Random(20) + (Ring * 30);
                                DY = Aow.Rtl.Rtl.Random(20) + (Ring * 30);
                                {
                                    I = 0;
                                    byte end__247 = 17;
                                    for (; I <= end__247; I += 1)
                                    {
                                        var call__248 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__248;
                                        Effect.FAbilityIndex = 52;
                                        Effect.FSpellIndex = Msg.Param[1].Value;
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

                                        var call__249 = System.Math.PI;
                                        var call__250 = System.Math.Sin(((double)(((I * 5) + N) * call__249)) / 180);
                                        var call__251 = System.Math.PI;
                                        var call__252 = System.Math.Cos(((double)(((I * 5) + N) * call__251)) / 180);
                                        Aow.Units.aowFX.aowFX.MakeLinePath(Aow.Rtl.Rtl.Round(call__252 * DX) + (X + 16), (Aow.Rtl.Rtl.Round(call__250 * DY) / 2) + (Y + 16), X + 16, Y + 16, 100, ref Effect.FPath);
                                    }
                                }
                            }
                        }

                        return;
                    }
                }

                if ((Msg.MsgID == 200713) && (Msg.HN == 0))
                {
                    var call__253 = GetYhx();
                    var call__254 = GetXhx();
                    var call__255 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__256 = call__255.Field[call__254, call__253];
                    var call__257 = call__256.FindOwnedChild(2228484);
                    if (call__257 == null)
                    {
                        var call__258 = GetYhx();
                        var call__259 = GetXhx();
                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__259, call__258, ref X, ref Y);
                        N = Aow.Rtl.Rtl.Random(6) + 25;
                        {
                            I = 1;
                            int end__260 = N - 1;
                            for (; I <= end__260; I += 1)
                            {
                                var call__261 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                Effect = call__261;
                                Effect.FAbilityIndex = 52;
                                Effect.FSpellIndex = Msg.Param[1].Value;
                                // { Original uses this smaller value for horizontal speed. }
                                Speed = (float)((Aow.Rtl.Rtl.Random(10) + 10) * 0.05);
                                Gravity = (float)0.5;
                                VZ = (float)((Aow.Rtl.Rtl.Random(50) + 50) * 0.05);
                                Z = 0;
                                Aow.Units.aowFX.aowFX.MakeArcPath(X + 16, Y + 16, (I * 360) / N, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                            }
                        }

                        switch (Msg.Param[1].Value)
                        {
                            case 130:
                                var call__262 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                Aow.Rtl.Rtl.RandSeed = call__262;
                                var call__263 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(130);
                                var call__264 = call__263.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                AnimateAction = call__264;
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(AnimateAction);
                                AnimateAction.Release();
                                break;
                            case 129:
                                var call__265 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                Aow.Rtl.Rtl.RandSeed = call__265;
                                var call__266 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(129);
                                var call__267 = call__266.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                RaiseAction = call__267;
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(RaiseAction);
                                RaiseAction.Release();
                                break;
                        }

                        var call__268 = FUnit.GetEnabled();
                        if (call__268)
                        {
                            UnitHS = Aow.Units.AoWTC.TTacticalCombatUnitHS.Create(null);
                            UnitHS.Initialize(FUnit);
                            var call__269 = Aow.Rtl.Rtl.Random(6);
                            UnitHS.SetDirection(unchecked((byte)(call__269 + 1)));
                            var call__270 = GetYhx();
                            var call__271 = GetXhx();
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(UnitHS, call__271, call__270, 0);
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
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.NewFrameEvents.Add(NewFrame);
            }

            FUnitGFX.AddRef();
            FSequenceFrame = -1;
        }

        // AoWTC.pas:3966
        public override void Deactivate()
        {
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.NewFrameEvents.Remove(NewFrame);
            FUnitGFX.Release();
            base.Deactivate();
        }

        // AoWTC.pas:3978
        public void NewFrame(Aow.Rtl.TObject Sender)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            var call__272 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[FSequenceIndex];
            Sequence = call__272;
            var call__273 = Sequence.GetTotalSequenceFrames();
            if ((FSequenceFrame + 1) < call__273)
            {
                FSequenceFrame += 1;
            }
        }

        // Engine.pas:524
        public static new Aow.Units.AoWTC.TCCorpseHS Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWTC.TCCorpseHS o = new Aow.Units.AoWTC.TCCorpseHS();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.HSEngine.THexagonSprite.Meta
        {
            // AoWTC.pas:3973
            // { $28 }
            public override int ClassID()
            {
                int Result = default;
                Result = 2228488;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AoWTC.TCCorpseHS => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.AoWTC.TCCorpseHS Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AoWTC.TCCorpseHS o = (Aow.Units.AoWTC.TCCorpseHS)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TCOozeHS (instance size 12, VMT 0041279C)
//     Inherited single-hexagon sprite layout; no additional fields. }
    public partial class TCOozeHS : Aow.Units.HSEngine.TSingleHS
    {
        // AoWTC.pas:4097
        public override sbyte GetOverlay(int HN)
        {
            sbyte Result = default;
            Result = 11;
            return Result;
        }

        // AoWTC.pas:4121
        public override byte GetLevel()
        {
            byte Result = default;
            Result = 26;
            return Result;
        }

        // AoWTC.pas:4126
        public override int GetShowPriority()
        {
            int Result = default;
            var call__274 = GetYhx();
            var call__275 = GetXhx();
            Result = (1744830464 + (call__274 << 13)) + call__275;
            return Result;
        }

        // AoWTC.pas:4102
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Frame = default;
            int X = default;
            int Y = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref X, ref Y);
            XhpSurf = (X - XhpSurf) - 16;
            YhpSurf = (Y - 48) - YhpSurf;
            switch (((Aow.Units.HSEngine.TMapField)Owner).TerrainType)
            {
                case 3:
                case 6:
                case 13:
                    var call__276 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[26];
                    Sequence = call__276;
                    break;
                case 12:
                    var call__277 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[27];
                    Sequence = call__277;
                    break;
                default:
                    var call__278 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[25];
                    Sequence = call__278;
                    break;
            }

            while (Sequence != null)
            {
                var call__279 = Sequence.GetTotalSequenceFrames();
                var call__280 = Sequence.GetUnloopedFrameIndex(Aow.Rtl.Rtl.ObjectToInt(this) % call__279);
                Frame = call__280;
                if (Frame != -1)
                {
                    var call__281 = Sequence.ImageLibrary[Frame];
                    call__281.Show(XhpSurf, YhpSurf, SurfaceDesc);
                }

                Sequence = Sequence.Next;
            }
        }

        // AoWTC.pas:4131
        public override bool PlaceOnMap()
        {
            bool Result = default;
            Result = base.PlaceOnMap();
            ((Aow.Units.HSEngine.TMapField)Owner).UpdateTerrain();
            return Result;
        }

        // Engine.pas:524
        public static new Aow.Units.AoWTC.TCOozeHS Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWTC.TCOozeHS o = new Aow.Units.AoWTC.TCOozeHS();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.HSEngine.THexagonSprite.Meta
        {
            // AoWTC.pas:4092
            public override int ClassID()
            {
                int Result = default;
                Result = 2228489;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AoWTC.TCOozeHS => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.AoWTC.TCOozeHS Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AoWTC.TCOozeHS o = (Aow.Units.AoWTC.TCOozeHS)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TUnitPositionControl (instance size 12, VMT 00412918)
//     $08 FPositions: packed XYL entries, eight per army position. }
    public partial class TUnitPositionControl : Aow.Units.Engine.TEObject
    {
        public Aow.Units.HSEngine.TXYLList FPositions;
        // AoWTC.pas:4943
        // { $08 }
        public override void Destroy()
        {
            FPositions.Free();
            FPositions = null;
            base.Destroy();
        }

        // AoWTC.pas:4936
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__282 = Aow.Units.HSEngine.TXYLList.Create();
            FPositions = call__282;
            var call__283 = Aow.Units.HSEngine.HSEngine.PackXYL(unchecked((sbyte)-1), unchecked((sbyte)-1), unchecked((sbyte)-1));
            FPositions.Default = call__283;
        }

        // AoWTC.pas:4990
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            Aow.Units.AoWTC.TUnitPositionHS HS = default;
            int X = default;
            int Y = default;
            int L = default;
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 65541)
            {
                L = Msg.Data[1];
                Aow.Units.HSEngine.HSEngine.HPtoHX(Msg.Value, Msg.Data[0], ref X, ref Y);
                var call__284 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(X, Y, L);
                if (call__284.TerrainType != 15)
                {
                    var call__285 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(X, Y, L);
                    var call__286 = call__285.FindOwnedChild(2228483);
                    HS = (Aow.Units.AoWTC.TUnitPositionHS)call__286;
                    if (HS == null)
                    {
                        HS = Aow.Units.AoWTC.TUnitPositionHS.Create(null);
                        var call__287 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(HS, X, Y, L);
                        if (!call__287)
                        {
                            HS.Free();
                            HS = null;
                        }
                    }

                    if (HS != null)
                    {
                        ;
                    }
                }
            }
        }

        // AoWTC.pas:4950
        public Aow.Units.HSEngine.TXYL GetUnitPosition(int Position, int Index)
        {
            Aow.Units.HSEngine.TXYL Result = default;
            int I = default;
            I = (Position * 8) + Index;
            if (I < FPositions.Count)
            {
                var call__288 = FPositions[I];
                Result = call__288;
            }
            else
            {
                Result.X = unchecked((sbyte)-1);
                Result.Y = unchecked((sbyte)-1);
                Result.L = unchecked((sbyte)-1);
            }

            return Result;
        }

        // AoWTC.pas:4963
        public void SetUnitPosition(int Position, int Index, Aow.Units.HSEngine.TXYL Value)
        {
            if ((Position < 7) && (Index < 8))
            {
                FPositions[(Position * 8) + Index] = Value;
            }
        }

        // AoWTC.pas:4969
        public bool ValidatePosition(ref sbyte Position, ref sbyte Index)
        {
            bool Result = default;
            var call__289 = FPositions[(Position * 8) + Index];
            Result = call__289.X == -1;
            if (!Result)
            {
                Index = 0;
                while (true)
                {
                    var call__290 = FPositions[(Position * 8) + Index];
                    if (!((call__290.X != -1) && (Index != 8)))
                        break;
                    {
                        Index = (sbyte)(((int)Index) + 1);
                    }
                }

                Result = Index != 8;
            }

            if (Result == false)
            {
                Position = 0;
                while ((Result == false) && (Position < 7))
                {
                    while (true)
                    {
                        var call__291 = FPositions[(Position * 8) + Index];
                        if (!((call__291.X != -1) && (Index != 8)))
                            break;
                        {
                            Index = (sbyte)(((int)Index) + 1);
                        }
                    }

                    Result = Index != 8;
                    if (!Result)
                    {
                        Index = 0;
                        Position = (sbyte)(((int)Position) + 1);
                    }
                }
            }

            return Result;
        }

        // AoWTC.pas:289
        public static new Aow.Units.AoWTC.TUnitPositionControl Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWTC.TUnitPositionControl o = new Aow.Units.AoWTC.TUnitPositionControl();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TCombatMoveControlSettings (instance size 432, VMT 004129E8): $1AC FMoveFlags. }
    public partial class TCombatMoveControlSettings : Aow.Units.AoWE.TMoveControlSettings
    {
        public TSet<Aow.Units.AoWTC.TCombatMoveModeElement> FMoveFlags;
        // { $1AC }
        public Aow.Units.FixedArray_byte_3 F_CombatMoveControlSettings_1AD;
        // intf_P5_01.inc:63
        public static new Aow.Units.AoWTC.TCombatMoveControlSettings Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWTC.TCombatMoveControlSettings o = new Aow.Units.AoWTC.TCombatMoveControlSettings();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TCombatMoveControl (instance size 488, VMT 00412AC4) }
    public partial class TCombatMoveControl : Aow.Units.AoWE.TMoveControl
    {
        public Aow.Units.AoWTC.TTacticalCombatUnitHS FUnitHS;
        // { $1E0 }
        public TSet<Aow.Units.AoWTC.TCombatMoveModeElement> FMoveFlags;
        // { $1E4; $405884 }
        public Aow.Units.FixedArray_byte_3 F_CombatMoveControl_1E5;
        // AoWTC.pas:2716
        public override void ProcessMove(Aow.Units.AoWE.TMoveControlSettings A1)
        {
            FUnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)A1.FArmyHS;
            FMoveFlags = ((Aow.Units.AoWTC.TCombatMoveControlSettings)A1).FMoveFlags;
            base.ProcessMove(A1);
        }

        // intf_P5_01.inc:120
        public static new Aow.Units.AoWTC.TCombatMoveControl Create(Aow.Units.HSEngine.THSMap AOwner)
        {
            Aow.Units.AoWTC.TCombatMoveControl o = new Aow.Units.AoWTC.TCombatMoveControl();
            try
            {
                o.Create__init_TMoveControl(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TTacticalCombatUnit (instance size 104, VMT 00412B4C)
//     $5C FMovePoints; $60 FUnitHS; $64 FPositionIndex; $65 FPositionGroup; $66 FCanFlee; $67 FDeathMethod. }
    public partial class TTacticalCombatUnit : Aow.Units.AoWE.TCombatUnit
    {
        public int FMovePoints;
        // { $5C }
        public Aow.Units.AoWTC.TTacticalCombatUnitHS FUnitHS;
        // { $60 }
        public byte FPositionIndex;
        // { $64 }
        public byte FPositionGroup;
        // { $65 }
        public bool FCanFlee;
        // { $66 }
        public Aow.Units.AoWTC.TTCDeathMethod FDeathMethod;
        // AoWTC.pas:2626
        // { $67 }
        // { conflict: FUsedAbility: Boolean; }
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
        }

        // AoWTC.pas:2612
        public override void NewTurn(byte A1)
        {
            base.NewTurn(unchecked((byte)A1));
            bool bool__292 = (A1 == unchecked((byte)FCombatPlayer));
            if (bool__292)
            {
                var call__293 = GetEnabled();
                bool__292 = call__293;
            }

            if (bool__292)
            {
                var call__294 = GetMoves();
                FMovePoints = call__294;
            }
        }

        // AoWTC.pas:2629
        public override int ExecuteDamage(int A1)
        {
            int Result = default;
            Aow.Units.AoWTC.TCCorpseHS Corpse = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            Aow.Units.AoWTC.TfxHS Effect = default;
            float Speed = default;
            float VZ = default;
            float Gravity = default;
            float Z = default;
            int J = default;
            int X = default;
            int Y = default;
            int I = default;
            Aow.Rtl.TRect R = default;
            void MakeBlood()
            {
                int Count = default;
                int X = default;
                int Y = default;
                int Direction = default;
                Aow.Units.AoWTC.TCBloodHS Blood = default;
                var call__295 = GetEnabled();
                bool bool__296 = call__295 && (FUnitHS != null);
                if (bool__296)
                {
                    var call__297 = FUnit.GetBloodType();
                    bool__296 = (call__297 != Aow.Units.AoWE.TBloodType.btNone);
                }

                if (bool__296)
                {
                    var call__298 = GetHitPoints();
                    bool bool__299 = (A1 >= call__298);
                    if (bool__299)
                    {
                        var call__300 = GetAbilityEnabled(131);
                        bool__299 = (call__300 || (FDeathMethod == Aow.Units.AoWTC.TTCDeathMethod.dmExplode));
                    }

                    if (bool__299)
                    {
                        Count = Aow.Rtl.Rtl.Random(5) + 10;
                    }
                    else
                    {
                        Count = Aow.Rtl.Rtl.Random(A1 + 7) - 4;
                    }

                    while (Count > 0)
                    {
                        Direction = Aow.Rtl.Rtl.Random(12);
                        if (Direction > 6)
                        {
                            Direction = 0;
                        }

                        var call__301 = FUnitHS.GetYhx();
                        var call__302 = FUnitHS.GetXhx();
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__302, call__301, Direction, ref X, ref Y);
                        var call__303 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__304 = call__303.Field[X, Y];
                        bool bool__305 = (call__304.TerrainType != 0);
                        if (bool__305)
                        {
                            var call__306 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__307 = call__306.Field[X, Y];
                            bool__305 = (call__307.TerrainType != 9);
                        }

                        if (bool__305)
                        {
                            var call__308 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__309 = call__308.Field[X, Y];
                            var call__310 = Aow.Units.AoWTC.TCBloodHS.Create(call__309);
                            Blood = call__310;
                            Blood.Activate();
                            var call__311 = Aow.Rtl.Rtl.Random(32);
                            Blood.FOffsetX = call__311 - 32;
                            var call__312 = Aow.Rtl.Rtl.Random(32);
                            Blood.FOffsetY = call__312 - 32;
                            var call__313 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[4];
                            var call__314 = call__313.GetTotalSequenceFrames();
                            var call__315 = Aow.Rtl.Rtl.Random(call__314);
                            Blood.FFrame = call__315;
                        }

                        Count -= 1;
                    }
                }
            }

            MakeBlood();
            Result = base.ExecuteDamage(A1);
            if (!GetEnabled())
            {
                if (FUnitHS != null)
                {
                    if (GetAbilityEnabled(131))
                    {
                        Aow.Units.CombatTE.CombatTE.MakeHitBlood(this, 100);
                        {
                            I = 0;
                            byte end__316 = 9;
                            for (; I <= end__316; I += 1)
                            {
                                var call__317 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxDecayComet);
                                Effect = call__317;
                                Effect.FAbilityIndex = 52;
                                Effect.FSpellIndex = 106;
                                var call__318 = FUnitHS.GetYhx();
                                var call__319 = FUnitHS.GetXhx();
                                Aow.Units.HSEngine.HSEngine.HXtoHP(call__319, call__318, ref X, ref Y);
                                var call__320 = Aow.Units.aowFX.TXYZ.Create();
                                Effect.FPath.Add(call__320);
                                var call__321 = Effect.FPath[0];
                                call__321.X = X + 16;
                                var call__322 = Effect.FPath[0];
                                call__322.Y = Y + 8;
                                {
                                    J = 0;
                                    byte end__323 = 10;
                                    for (; J <= end__323; J += 1)
                                    {
                                        Speed = (float)((Aow.Rtl.Rtl.Random(50) + 50) * 0.05);
                                        Gravity = (float)1.5;
                                        VZ = (float)((Aow.Rtl.Rtl.Random(25) + 25) * 0.05);
                                        Z = 0;
                                        var call__324 = Effect.FPath.Count;
                                        var call__325 = Effect.FPath[call__324 - 1];
                                        var call__326 = Effect.FPath.Count;
                                        var call__327 = Effect.FPath[call__326 - 1];
                                        var call__328 = Effect.FPath.Count;
                                        var call__329 = Effect.FPath[call__328 - 1];
                                        var call__330 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(call__329.X, call__325.Y - call__327.Z, call__330, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var call__331 = FUnitHS.GetYhx();
                        var call__332 = FUnitHS.GetXhx();
                        var call__333 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__334 = call__333.Field[call__332, call__331];
                        bool bool__335 = (call__334.TerrainType != 0);
                        if (bool__335)
                        {
                            var call__336 = FUnitHS.GetYhx();
                            var call__337 = FUnitHS.GetXhx();
                            var call__338 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__339 = call__338.Field[call__337, call__336];
                            bool__335 = (call__339.TerrainType != 9);
                        }

                        if (bool__335)
                        {
                            var call__340 = FUnitHS.GetYhx();
                            var call__341 = FUnitHS.GetXhx();
                            var call__342 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__343 = call__342.Field[call__341, call__340];
                            var call__344 = Aow.Units.AoWTC.TCCorpseHS.Create(call__343);
                            Corpse = call__344;
                            Corpse.FUnitGFX = FUnitHS.FUnitGFXResource;
                            var call__345 = Corpse.FUnitGFX.FSequences.ImageSequences[30];
                            Sequence = call__345;
                            if (Sequence != null)
                            {
                                var call__346 = Sequence.GetTotalSequenceFrames();
                                var call__347 = Aow.Rtl.Rtl.Random(call__346);
                                Corpse.FFrame = call__347;
                            }

                            switch (FDeathMethod)
                            {
                                case Aow.Units.AoWTC.TTCDeathMethod.dmNormal:
                                    var call__348 = FUnit.GetBloodType();
                                    switch (call__348)
                                    {
                                        case Aow.Units.AoWE.TBloodType.btNone:
                                            Corpse.FSequenceIndex = -1;
                                            break;
                                        default:
                                        {
                                            var call__349 = FUnit.GetUnitSize();
                                            Corpse.FSequenceIndex = (unchecked((int)call__349) & 127) + 30;
                                            if (Corpse.FSequenceIndex > 32)
                                            {
                                                Corpse.FSequenceIndex = 32;
                                            }
                                        }

                                            break;
                                    }

                                    break;
                                case Aow.Units.AoWTC.TTCDeathMethod.dmExplode:
                                    Corpse.FSequenceIndex = -1;
                                    break;
                            }

                            var call__350 = Aow.Rtl.Rtl.Random(10);
                            Corpse.FOffsetX = call__350 - 5;
                            var call__351 = Aow.Rtl.Rtl.Random(10);
                            Corpse.FOffsetY = call__351 - 5;
                            var call__352 = FUnit.GetUnitType();
                            if (call__352 != Aow.Units.AoWE.TUnitType.utMachine)
                            {
                                Corpse.FUnit = this;
                            }

                            Corpse.Activate();
                        }
                    }

                    var call__353 = FUnitHS.GetYhx();
                    var call__354 = FUnitHS.GetXhx();
                    var call__355 = FUnitHS.GetYhx();
                    var call__356 = FUnitHS.GetXhx();
                    var call__357 = Aow.Rtl.Rtl.Rect(call__356, call__355, call__354 + 1, call__353 + 1);
                    R = call__357;
                    FUnitHS.Free();
                    FUnitHS = null;
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.TriggerMapChangedEvent(R, 0);
                }
            }
            else
            {
                if (FUnitHS != null)
                {
                    var call__358 = FUnit.GetUnitGFXResourceIndex();
                    FUnitHS.SetUnitGFXResourceIndex(call__358);
                }
            }

            return Result;
        }

        // AoWTC.pas:2605
        public int GetPosValue()
        {
            int Result = default;
            var call__359 = GetAttack();
            var call__360 = GetDamage();
            var call__361 = GetDefense();
            var call__362 = GetMoves();
            Result = ((call__359 + call__360) + call__361) + (call__362 / 3);
            if (SupportCombatMode(TSet<Aow.Units.AoWE.TCombatMode>.Of(Aow.Units.AoWE.TCombatMode.cmMediumRange)))
            {
                Result = Result - 100;
            }

            if (SupportCombatMode(TSet<Aow.Units.AoWE.TCombatMode>.Of(Aow.Units.AoWE.TCombatMode.cmLongRange)))
            {
                Result = Result - 200;
            }

            return Result;
        }

        // AoWTC.pas:2618
        public void MovedTo(Aow.Units.AoWE.TAoWMapField OldField, Aow.Units.AoWE.TAoWMapField NewField, int MoveCost)
        {
            FMovePoints = FMovePoints - MoveCost;
            if (FMovePoints < 0)
            {
                FMovePoints = 0;
            }

            if (GetAbilityEnabled(43))
            {
                var call__363 = FUnitHS.GetYhx();
                var call__364 = FUnitHS.GetXhx();
                Aow.Units.CombatFire.CombatFire.CreateFire(false, 50, call__364, call__363);
            }

            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitChangedEvents.TriggerNotifyEvents(FUnitHS);
        }

        // intf_P3_01.inc:244
        public static new Aow.Units.AoWTC.TTacticalCombatUnit Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWTC.TTacticalCombatUnit o = new Aow.Units.AoWTC.TTacticalCombatUnit();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TTacticalCombatUnitHS (instance size 72, VMT 00412D08): $1C FUnit;
//     $20 FMoveIndicator; $22 FIndicatorStep; $24 FAction; $25 FActionDone; $26 FActionFrame; $2C FOffsetX; $30 FOffsetY; $34 FSelectedAbility;
//     $38 FSelectedSpell; $3C FCombatAbilityCount; $40 FAbilities; $44 FBusy. }
    public partial class TTacticalCombatUnitHS : Aow.Units.AoWE.TUnitHS
    {
        public Aow.Units.AoWTC.TTacticalCombatUnit FUnit;
        // { $1C }
        // { conflict: FCombatUnit: TTacticalCombatUnit; }
        public short FMoveIndicator;
        // { $20 }
        public short FIndicatorStep;
        // { $22 }
        public Aow.Units.AoWTC.TTCAction FAction;
        // { $24 }
        public bool FActionDone;
        // { $25 }
        public short FActionFrame;
        // { $26 }
        public Aow.Units.FixedArray_byte_4 F_TacticalCombatUnitHS_28;
        public int FOffsetX;
        // { $2C }
        public int FOffsetY;
        // { $30 }
        public int FSelectedAbility;
        // { $34 }
        public int FSelectedSpell;
        // { $38 }
        public int FCombatAbilityCount;
        // { $3C }
        public Aow.Rtl.TList FAbilities;
        // { $40 }
        public bool FBusy;
        // { $44 }
        public Aow.Units.FixedArray_byte_3 F_TacticalCombatUnitHS_45;
        // AoWTC.pas:3566
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (((Msg.MsgID != 536936471) && ((Msg.MsgID & 1073741824) == 0)) && (FUnit != null))
            {
                FUnit.MsgProc(ref Msg, Sender);
            }
        }

        // AoWTC.pas:3665
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AoWTC.pas:3461
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            Aow.Units.AoWE.TSelfDestructCA SelfDestruct = default;
            Aow.Units.AoWE.TDamageCA SpellDamage = default;
            Aow.Units.AoWE.TRangedAttackCA RangedDamage = default;
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4364)
            {
                if ((Msg.HN == 0) && (!Aow.Units.AoWTC.AoWTC.AoWCombatMap.FFogEnabled || (Unsafe.As<ushort, Aow.Rtl.WordRec>(ref Unsafe.As<int, Aow.Rtl.LongRec>(ref ((Aow.Units.AoWE.TAoWMapField)Owner).FFieldFlags).Hi).Lo != 0)))
                {
                    var call__365 = FUnit.GetPlayer();
                    if (call__365 != 0)
                    {
                        var call__366 = FUnit.GetPlayer();
                        var call__367 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__366);
                        Msg.Param[0] = Msg.Param[0].Value | (call__367.FPlayerRace + 1);
                    }
                    else
                    {
                        Msg.Param[0] = Msg.Param[0].Value | 13;
                    }
                }
            }
            else
            {
                if ((Msg.MsgID == 4352) && (Msg.HN == 0))
                {
                    var call__368 = FUnit.GetPlayer();
                    Msg.Result = call__368;
                }
                else
                {
                    if ((Msg.MsgID == 200704) && (Msg.HN == 0))
                    {
                        var call__369 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__369;
                        var call__370 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(65);
                        var call__371 = ((Aow.Units.AoWE.TSelfDestructAbility)call__370).CreateSelfDestructCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit);
                        SelfDestruct = call__371;
                        if (SelfDestruct.FDamage > 0)
                        {
                            SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                        }

                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(SelfDestruct);
                        SelfDestruct.Release();
                    }
                    else
                    {
                        if ((Msg.MsgID == 200707) && (Msg.HN == 0))
                        {
                            var call__372 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__372;
                            var call__373 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(117);
                            var call__374 = call__373.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                            SpellDamage = (Aow.Units.AoWE.TDamageCA)call__374;
                            if (SpellDamage.FDamage > 0)
                            {
                                SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                            }

                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                            SpellDamage.Release();
                        }
                        else
                        {
                            if ((Msg.MsgID == 200715) && (Msg.HN == 0))
                            {
                                var call__375 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                Aow.Rtl.Rtl.RandSeed = call__375;
                                var call__376 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(113);
                                var call__377 = call__376.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                SpellDamage = (Aow.Units.AoWE.TDamageCA)call__377;
                                if (SpellDamage.FDamage > 0)
                                {
                                    SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                }

                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                SpellDamage.Release();
                            }
                            else
                            {
                                if ((Msg.MsgID == 200714) && (Msg.HN == 0))
                                {
                                    var call__378 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                    Aow.Rtl.Rtl.RandSeed = call__378;
                                    var call__379 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(31);
                                    var call__380 = ((Aow.Units.AoWE.TRangedAttackAbility)call__379).CreateRangedAttackCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, 0);
                                    RangedDamage = call__380;
                                    if (RangedDamage.FDamage > 0)
                                    {
                                        SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                    }

                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(RangedDamage);
                                    RangedDamage.Release();
                                }
                                else
                                {
                                    if ((Msg.MsgID == 200723) && (Msg.HN == 0))
                                    {
                                        var call__381 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                        Aow.Rtl.Rtl.RandSeed = call__381;
                                        var call__382 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Msg.Param[1].Value);
                                        var call__383 = call__382.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                        SpellDamage = (Aow.Units.AoWE.TDamageCA)call__383;
                                        if (SpellDamage.FDamage > 0)
                                        {
                                            SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                        }

                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                        SpellDamage.Release();
                                    }
                                    else
                                    {
                                        if ((Msg.MsgID == 200722) && (Msg.HN == 0))
                                        {
                                            var call__384 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                            Aow.Rtl.Rtl.RandSeed = call__384;
                                            var call__385 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Msg.Param[1].Value);
                                            var call__386 = ((Aow.Units.AoWE.TRangedAttackAbility)call__385).CreateRangedAttackCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, 0);
                                            RangedDamage = call__386;
                                            if (RangedDamage.FDamage > 0)
                                            {
                                                SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                            }

                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(RangedDamage);
                                            RangedDamage.Release();
                                        }
                                        else
                                        {
                                            if ((Msg.MsgID == 200709) && (Msg.HN == 0))
                                            {
                                                var call__387 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                Aow.Rtl.Rtl.RandSeed = call__387;
                                                var call__388 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(104);
                                                var call__389 = call__388.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                SpellDamage = (Aow.Units.AoWE.TDamageCA)call__389;
                                                if (SpellDamage.FDamage > 0)
                                                {
                                                    SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                                }

                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                                SpellDamage.Release();
                                            }
                                            else
                                            {
                                                if ((Msg.MsgID == 200716) && (Msg.HN == 0))
                                                {
                                                    var call__390 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                    Aow.Rtl.Rtl.RandSeed = call__390;
                                                    var call__391 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(120);
                                                    var call__392 = call__391.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                    SpellDamage = (Aow.Units.AoWE.TDamageCA)call__392;
                                                    if (SpellDamage.FDamage > 0)
                                                    {
                                                        SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                                    }

                                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                                    SpellDamage.Release();
                                                }
                                                else
                                                {
                                                    if ((Msg.MsgID == 200710) && (Msg.HN == 0))
                                                    {
                                                        var call__393 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                        Aow.Rtl.Rtl.RandSeed = call__393;
                                                        var call__394 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(116);
                                                        var call__395 = call__394.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                        SpellDamage = (Aow.Units.AoWE.TDamageCA)call__395;
                                                        if (SpellDamage.FDamage > 0)
                                                        {
                                                            SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                                        }

                                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                                        SpellDamage.Release();
                                                    }
                                                    else
                                                    {
                                                        if ((Msg.MsgID == 200717) && (Msg.HN == 0))
                                                        {
                                                            var call__396 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                            Aow.Rtl.Rtl.RandSeed = call__396;
                                                            var call__397 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(127);
                                                            var call__398 = call__397.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                            SpellDamage = (Aow.Units.AoWE.TDamageCA)call__398;
                                                            if (SpellDamage.FDamage > 0)
                                                            {
                                                                SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                                            }

                                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                                            SpellDamage.Release();
                                                        }
                                                        else
                                                        {
                                                            if ((Msg.MsgID == 200718) && (Msg.HN == 0))
                                                            {
                                                                var call__399 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                                Aow.Rtl.Rtl.RandSeed = call__399;
                                                                var call__400 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(128);
                                                                var call__401 = call__400.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                                SpellDamage = (Aow.Units.AoWE.TDamageCA)call__401;
                                                                if (SpellDamage.FDamage > 0)
                                                                {
                                                                    SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                                                }

                                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
                                                                SpellDamage.Release();
                                                            }
                                                            else
                                                            {
                                                                if ((Msg.MsgID == 200719) && (Msg.HN == 0))
                                                                {
                                                                    SetDirection(unchecked((byte)(FDirection + 1)));
                                                                    if (FDirection > 6)
                                                                    {
                                                                        SetDirection(1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if ((Msg.MsgID == 200720) && (Msg.HN == 0))
                                                                    {
                                                                        var call__402 = Aow.Rtl.Rtl.Random((Msg.Param[0].Value * 2) + 1);
                                                                        FOffsetX = call__402 - Msg.Param[0].Value;
                                                                        var call__403 = Aow.Rtl.Rtl.Random((Msg.Param[0].Value * 2) + 1);
                                                                        FOffsetY = call__403 - Msg.Param[0].Value;
                                                                    }
                                                                    else
                                                                    {
                                                                        if ((Msg.MsgID == 200721) && (Msg.HN == 0))
                                                                        {
                                                                            FOffsetX = 0;
                                                                            FOffsetY = 0;
                                                                        }
                                                                        else
                                                                        {
                                                                            if ((Msg.MsgID == 200708) && (Msg.HN == 0))
                                                                            {
                                                                                var call__404 = FUnit.FUnit.GetMoveTypes();
                                                                                if ((!call__404.Contains(Aow.Units.AoWE.TMoveType.mtFlying) && (((Aow.Units.HSEngine.TMapField)Owner).TerrainType != 0)) && (((Aow.Units.HSEngine.TMapField)Owner).TerrainType != 9))
                                                                                {
                                                                                    var call__405 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                                                    Aow.Rtl.Rtl.RandSeed = call__405;
                                                                                    var call__406 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(112);
                                                                                    var call__407 = call__406.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                                                    SpellDamage = (Aow.Units.AoWE.TDamageCA)call__407;
                                                                                    if (SpellDamage.FDamage > 0)
                                                                                    {
                                                                                        SetAction(Aow.Units.AoWTC.TTCAction.caHit);
                                                                                    }

                                                                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(SpellDamage);
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
        public override void CanMoveOn(in Aow.Units.HSEngine.THSMoveOnInfo Info, ref Aow.Units.HSEngine.THSMoveOnResult MoveResult)
        {
            int X = default;
            int Y = default;
            Aow.Units.FleeLoc.TMovementHS Movement = default;
            Aow.Units.AoWTC.Record_TTacticalCombatUnitHS_CanMoveOn_AbilityInfo_3374_3 AbilityInfo = default;
            Aow.Units.AoWTC.Record_TTacticalCombatUnitHS_CanMoveOn_EnemyInfo_3374_16 EnemyInfo = default;
            if (Info.HN == 0)
            {
                MoveResult.Code = -1;
                MoveResult.Done = true;
                var call__408 = GetYhx();
                var call__409 = GetXhx();
                var call__410 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__411 = call__410.Field[call__409, call__408];
                var call__412 = call__411.FindOwnedChild(2228490);
                Movement = (Aow.Units.FleeLoc.TMovementHS)call__412;
                if ((Movement != null) && Movement.FEnabled)
                {
                    var call__413 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                    var call__414 = FUnit.GetSide();
                    if (call__413 == call__414)
                    {
                        MoveResult.Code = 0;
                        MoveResult.Done = false;
                        return;
                    }
                }

                var call__415 = GetXhx();
                if ((call__415 != ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX) && (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX != -1))
                {
                    return;
                }

                var call__416 = GetYhx();
                if ((call__416 != ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY) && (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY != -1))
                {
                    return;
                }

                bool bool__417 = !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiMove) && !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmMoveRanged);
                if (bool__417)
                {
                    var call__418 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetCount();
                    bool__417 = (call__418 >= 2);
                }

                if (bool__417)
                {
                    return;
                }

                bool bool__419 = (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility <= -1);
                if (bool__419)
                {
                    var call__420 = Aow.Units.AoWE.AoWE.MeleeDamageTypes(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit);
                    bool bool__421 = (call__420 == TSet<Aow.Units.AoWE.TDamageType>.Of());
                    if (!bool__421)
                    {
                        var call__422 = FUnit.GetAbilityEnabled(1);
                        bool bool__423 = call__422;
                        if (bool__423)
                        {
                            var call__424 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1);
                            bool__423 = !call__424;
                        }

                        bool__421 = bool__423;
                    }

                    bool__419 = bool__421;
                }

                if ((bool__419 && !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmMoveRanged)) && !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiMove))
                {
                    return;
                }

                if (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility == -1)
                {
                    if (!((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiMove) && !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmMoveRanged))
                    {
                        var call__425 = FUnit.GetSide();
                        var call__426 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                        if (call__425 == call__426)
                        {
                            return;
                        }
                    }

                    MoveResult.Code = 0;
                    return;
                }

                var call__427 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility);
                call__427.GetCombatInfo(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.FUnit, ref AbilityInfo.Data);
                bool bool__428 = AbilityInfo.Data.TargetTypes.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend);
                if (bool__428)
                {
                    var call__429 = FUnit.GetSide();
                    var call__430 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                    bool__428 = (call__429 == call__430);
                }

                if (bool__428)
                {
                    goto Touch;
                }

                var call__431 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility);
                call__431.GetCombatInfo(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.FUnit, ref EnemyInfo.Data);
                if (!EnemyInfo.Data.TargetTypes.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy))
                {
                    return;
                }

                var call__432 = FUnit.GetSide();
                var call__433 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                if (call__432 == call__433)
                {
                    return;
                }

                Touch:
                    if (((Aow.Units.AoWE.TTouchAbility)Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility)).CanTouch(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit, FUnit))
                    {
                        MoveResult.Code = 0;
                    }
            }
            else
            {
                var call__434 = GetYhx();
                var call__435 = GetXhx();
                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__435, call__434, Info.HN, ref X, ref Y);
                if (MoveResult.Code < 30)
                {
                    MoveResult.Code = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMovePointTable[Info.MapField.TerrainType][Info.MapField.OverlayType - -1];
                }

                if (MoveResult.Code <= -1)
                {
                    return;
                }

                if (Info.HN >= 7)
                {
                    return;
                }

                var call__436 = GetXhx();
                bool bool__437 = (call__436 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX);
                if (bool__437)
                {
                    var call__438 = GetYhx();
                    bool__437 = (call__438 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY);
                }

                if (bool__437)
                {
                    return;
                }

                var call__439 = FUnit.GetSide();
                var call__440 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                if (call__439 == call__440)
                {
                    return;
                }

                var call__441 = Aow.Units.AoWE.AoWE.MeleeDamageTypes(FUnit);
                if (call__441 == TSet<Aow.Units.AoWE.TDamageType>.Of())
                {
                    return;
                }

                var call__442 = FUnit.GetLocked();
                if (call__442)
                {
                    return;
                }

                var call__443 = FUnit.GetAbilityEnabled(1);
                var call__444 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1);
                if (call__443 != call__444)
                {
                    return;
                }

                bool bool__445 = (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX >= 0) && (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY >= 0);
                if (bool__445)
                {
                    var call__446 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX, ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY);
                    bool__445 = (call__446 <= 1);
                }

                if (bool__445)
                {
                    var call__447 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__448 = call__447.Field[((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX, ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY];
                    var call__449 = call__448.FindOwnedChild(2228484);
                    if (call__449 != null)
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
            var call__450 = FUnit.FUnit.GetUnitGFXResourceIndex();
            SetUnitGFXResourceIndex(call__450);
            var call__451 = new Aow.Rtl.TList();
            FAbilities = call__451;
            CreateTCAbList();
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.NewFrameEvents.Add(NewFrame);
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled)
            {
                var call__452 = FUnit.GetPlayer();
                var call__453 = FUnit.GetVisibilityRange();
                var call__454 = GetYhx();
                var call__455 = GetXhx();
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.ExploreArea(call__455, call__454, 0, unchecked((sbyte)call__452), call__453);
            }

            FActionFrame = unchecked((short)-1);
            FSelectedAbility = -1;
            FSelectedSpell = -1;
        }

        // AoWTC.pas:3339
        public override void Deactivate()
        {
            int I = default;
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
            {
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.NewFrameEvents.Remove(NewFrame);
            }

            {
                I = 0;
                int end__456 = FAbilities.Count - 1;
                for (; I <= end__456; I += 1)
                {
                    var call__457 = FAbilities[I];
                    ((Aow.Rtl.TObject)call__457).Free();
                }
            }

            FAbilities.Free();
            base.Deactivate();
        }

        // AoWTC.pas:3439
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            if (Info.HN == 0)
            {
                ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = ((((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 16384) | 8192) | 33554432;
            }
            else
            {
                if (Info.HN <= 6)
                {
                    ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 16384;
                }
            }
        }

        // AoWTC.pas:3784
        public override void Select()
        {
            base.Select();
            CreateTCAbList();
            FSelectedAbility = -1;
            var call__458 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.GetTCUnitHS();
            if (call__458 != this)
            {
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.SetTCUnitHS(this);
            }

            FUnitGFXResource.FSFXLibraries[0].Play(0);
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer == FUnit.FCombatPlayer)
            {
                var call__459 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer));
                call__459.FLastUnit = FUnit;
            }
        }

        // AoWTC.pas:3796
        public override void Unselect()
        {
            base.Unselect();
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
            {
                var call__460 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.GetTCUnitHS();
                if (call__460 == this)
                {
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.Unselect();
                }
            }
        }

        // AoWTC.pas:3818
        public override void ShowShadow(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            float Progress = default;
            int PX = default;
            int PY = default;
            int Index = default;
            int DX = default;
            int DY = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref PX, ref PY);
            X = (PX - X) + FOffsetX;
            if (FBusy)
            {
                Y = (PY - Y) - 8;
            }
            else
            {
                Y = ((PY - Y) - 8) + FOffsetY;
            }

            if (FMoveStep != -1)
            {
                Aow.Units.HSEngine.HSEngine.HNtoHP(FDirection, ref DX, ref DY);
                var call__461 = GetMoveSteps();
                Progress = (float)(((double)FMoveStep) / call__461);
                X += Aow.Rtl.Rtl.Round(DX * Progress);
                Y += Aow.Rtl.Rtl.Round(DY * Progress);
            }

            var call__462 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 23];
            Sequence = call__462;
            while (Sequence != null)
            {
                var call__463 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                Index = call__463;
                if (Index != -1)
                {
                    var call__464 = Sequence.ImageLibrary[Index];
                    call__464.Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                }

                Sequence = Sequence.Next;
            }
        }

        // AoWTC.pas:3357
        public override int GetMoveSteps()
        {
            int Result = default;
            var call__465 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetMovementSpeed();
            switch (call__465)
            {
                case Aow.Units.AoWReg.TMovementSpeed.mmsFast:
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
                case Aow.Units.AoWReg.TMovementSpeed.mmsTurbo:
                    Result = 2;
                    break;
                default:
                    Result = base.GetMoveSteps();
                    break;
            }

            return Result;
        }

        // AoWTC.pas:3351
        public void Initialize(Aow.Units.AoWTC.TTacticalCombatUnit U)
        {
            FUnit = U;
            FUnit.FUnitHS = this;
        }

        // AoWTC.pas:3573
        public void NewFrame(Aow.Rtl.TObject Sender)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            if (FActionDone)
            {
                SetAction(Aow.Units.AoWTC.TTCAction.caMove);
            }

            if (FMoveIndicator == 0)
            {
                var call__466 = Aow.Rtl.Rtl.Random(5);
                FMoveIndicator = unchecked((short)(call__466 + 1));
                FIndicatorStep = 1;
            }
            else
            {
                FMoveIndicator = unchecked((short)(FMoveIndicator + FIndicatorStep));
                if (FMoveIndicator == 1)
                {
                    FIndicatorStep = 1;
                }
                else
                {
                    if (FMoveIndicator == 6)
                    {
                        FIndicatorStep = unchecked((short)-1);
                    }
                }
            }

            switch (FAction)
            {
                case Aow.Units.AoWTC.TTCAction.caStepUp:
                    var call__467 = GetMoveSteps();
                    if (FActionFrame <= call__467)
                    {
                        Aow.Units.HSEngine.HSEngine.HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
                        var call__468 = GetMoveSteps();
                        FOffsetX = (FActionFrame * FOffsetX) / (call__468 * 2);
                        var call__469 = GetMoveSteps();
                        FOffsetY = (FActionFrame * FOffsetY) / (call__469 * 2);
                        FActionFrame = (short)(((int)FActionFrame) + 2);
                    }
                    else
                    {
                        Aow.Units.HSEngine.HSEngine.HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
                        FOffsetX = FOffsetX / 2;
                        FOffsetY = FOffsetY / 2;
                        FActionDone = true;
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caStepBack:
                    var call__470 = GetMoveSteps();
                    if (FActionFrame <= call__470)
                    {
                        Aow.Units.HSEngine.HSEngine.HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
                        var call__471 = GetMoveSteps();
                        var call__472 = GetMoveSteps();
                        FOffsetX = ((call__471 - FActionFrame) * FOffsetX) / (call__472 * 2);
                        var call__473 = GetMoveSteps();
                        var call__474 = GetMoveSteps();
                        FOffsetY = ((call__473 - FActionFrame) * FOffsetY) / (call__474 * 2);
                        FActionFrame = (short)(((int)FActionFrame) + 2);
                    }
                    else
                    {
                        FOffsetX = 0;
                        FOffsetY = 0;
                        FActionDone = true;
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caMeleeHit:
                case Aow.Units.AoWTC.TTCAction.caMeleeMiss:
                case Aow.Units.AoWTC.TTCAction.caAbility:
                    var call__475 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                    Sequence = call__475;
                    if (Sequence != null)
                    {
                        FActionFrame = (short)(((int)FActionFrame) + 1);
                        var call__476 = Sequence.GetTotalSequenceFrames();
                        if (FActionFrame >= (call__476 - 1))
                        {
                            var call__477 = Sequence.GetTotalSequenceFrames();
                            FActionFrame = unchecked((short)(call__477 - 1));
                            FActionDone = true;
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caBreathAttack:
                    var call__478 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                    Sequence = call__478;
                    if (Sequence != null)
                    {
                        var call__479 = Sequence.GetTotalSequenceFrames();
                        if (FActionFrame < (call__479 - 1))
                        {
                            FActionFrame = (short)(((int)FActionFrame) + 1);
                        }
                        else
                        {
                            FActionDone = true;
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caHit:
                    var call__480 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 17];
                    Sequence = call__480;
                    if (Sequence != null)
                    {
                        FActionFrame = (short)(((int)FActionFrame) + 1);
                        var call__481 = Sequence.GetTotalSequenceFrames();
                        if (FActionFrame >= (call__481 - 1))
                        {
                            var call__482 = Sequence.GetTotalSequenceFrames();
                            FActionFrame = unchecked((short)(call__482 - 1));
                            FActionDone = true;
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caRoundAttack:
                case Aow.Units.AoWTC.TTCAction.caRoundAttackHit:
                    var call__483 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[FDirection + 16];
                    Sequence = call__483;
                    if (Sequence != null)
                    {
                        FActionFrame = (short)(((int)FActionFrame) + 1);
                        var call__484 = Sequence.GetTotalSequenceFrames();
                        if (FActionFrame >= (call__484 - 1))
                        {
                            var call__485 = Sequence.GetTotalSequenceFrames();
                            FActionFrame = unchecked((short)(call__485 - 1));
                            FActionDone = true;
                        }
                    }

                    break;
                default:
                    FActionFrame = unchecked((short)-1);
                    break;
            }

            switch (FAction)
            {
                case Aow.Units.AoWTC.TTCAction.caMove:
                case Aow.Units.AoWTC.TTCAction.caStepUp:
                case Aow.Units.AoWTC.TTCAction.caStepBack:
                    if (FMoveStep != -1)
                    {
                        var call__486 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 5];
                        Sequence = call__486;
                        if (((Sequence != null) && (Sequence.SpeedCount == Sequence.SequenceSpeed)) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                        {
                            var call__487 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                            FUnitGFXResource.FSFXLibraries[1].Play(call__487 % 100);
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caMeleeHit:
                case Aow.Units.AoWTC.TTCAction.caMeleeMiss:
                case Aow.Units.AoWTC.TTCAction.caAbility:
                case Aow.Units.AoWTC.TTCAction.caBreathAttack:
                    if ((FActionFrame == 0) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                    {
                        if (FSelectedAbility > -1)
                        {
                            if (FSelectedAbility == 52)
                            {
                                if (((FSelectedSpell != 112) && (FSelectedSpell != 122)) && (FSelectedSpell != 119))
                                {
                                    var call__488 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSelectedSpell);
                                    call__488.FCastSounds.Play(0);
                                }
                            }
                            else
                            {
                                var call__489 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FSelectedAbility);
                                call__489.FSFXLibrary.Play(0);
                            }
                        }
                        else
                        {
                            if (FAction == Aow.Units.AoWTC.TTCAction.caMeleeMiss)
                            {
                                FUnitGFXResource.FSFXLibraries[2].Play(1);
                            }
                            else
                            {
                                FUnitGFXResource.FSFXLibraries[2].Play(0);
                            }
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caHit:
                case Aow.Units.AoWTC.TTCAction.caKilled:
                    if ((FActionFrame == 0) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                    {
                        if (FAction == Aow.Units.AoWTC.TTCAction.caHit)
                        {
                            FUnitGFXResource.FSFXLibraries[3].Play(0);
                        }
                        else
                        {
                            FUnitGFXResource.FSFXLibraries[4].Play(0);
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caRoundAttack:
                case Aow.Units.AoWTC.TTCAction.caRoundAttackHit:
                    if (((FActionFrame < 18) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds) && (Aow.Units.AoWTC.AoWTC.raDir[FActionFrame] > 0))
                    {
                        if (FAction == Aow.Units.AoWTC.TTCAction.caRoundAttack)
                        {
                            FUnitGFXResource.FSFXLibraries[2].Play(1);
                        }
                        else
                        {
                            FUnitGFXResource.FSFXLibraries[2].Play(0);
                        }
                    }

                    break;
            }
        }

        // AoWTC.pas:3804
        public bool MeleeMoveTC(Aow.Units.AoWE.TMovepath Path)
        {
            bool Result = default;
            Aow.Units.CombatTE.TCMeleeMoveTE TE = default;
            Result = false;
            var call__490 = FUnit.GetPlayer();
            var call__491 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(call__490);
            if (call__491 && (Path.Count > 1))
            {
                var call__492 = Aow.Units.CombatTE.CombatTE.CreateTCMeleeMoveTE(FUnit, Path);
                TE = call__492;
                if (TE != null)
                {
                    var call__493 = FUnit.GetPlayer();
                    var call__494 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(call__493);
                    call__494.AddEvent(TE);
                    TE.Release();
                    Result = true;
                }
            }

            return Result;
        }

        // AoWTC.pas:3836
        public int SelectAbility(int A1)
        {
            int Result = default;
            int I = default;
            int Index = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.DeactivateRangedPath();
            FSelectedAbility = -1;
            Result = -1;
            Index = -1;
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer)
            {
                {
                    I = 0;
                    int end__495 = FAbilities.Count - 1;
                    for (; I <= end__495; I += 1)
                    {
                        var call__496 = FAbilities[I];
                        if (((Aow.Units.AoWTC.TTCAbility)call__496).Ability == A1)
                        {
                            Index = I;
                        }
                    }
                }

                var call__497 = FUnit.GetLocked();
                if ((((!call__497 && (FUnit.FMovePoints > 0)) && (FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)) && (Index < FCombatAbilityCount)) && (Index > -1))
                {
                    if (A1 == 52)
                    {
                        var call__498 = FUnit.FUnit.GetMoves();
                        if (call__498 == FUnit.FMovePoints)
                        {
                            var call__499 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(A1);
                            call__499.ActivateCombat(FUnit.FUnit, ref Reason);
                            FSelectedSpell = -1;
                            FSelectedAbility = A1;
                            Result = Index;
                        }
                    }
                    else
                    {
                        var call__500 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(A1);
                        var call__501 = call__500.GetCombatMode(FUnit.FUnit);
                        if (TSet<Aow.Units.AoWE.TCombatMode>.Of(Aow.Units.AoWE.TCombatMode.cmShortRange, Aow.Units.AoWE.TCombatMode.cmMediumRange, Aow.Units.AoWE.TCombatMode.cmLongRange).Contains(call__501))
                        {
                            var call__502 = FUnit.FUnit.GetMoves();
                            if (call__502 == FUnit.FMovePoints)
                            {
                                var call__503 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(A1);
                                var call__504 = call__503.GetCombatMode(FUnit.FUnit);
                                if (call__504 == Aow.Units.AoWE.TCombatMode.cmShortRange)
                                {
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FRangedPathHS.FRange = 4;
                                }
                                else
                                {
                                    var call__505 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(A1);
                                    var call__506 = call__505.GetCombatMode(FUnit.FUnit);
                                    if (call__506 == Aow.Units.AoWE.TCombatMode.cmMediumRange)
                                    {
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FRangedPathHS.FRange = 8;
                                    }
                                    else
                                    {
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FRangedPathHS.FRange = 12;
                                    }
                                }

                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FRangedPathHS.FSelection = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection;
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.DynamicFixedVisibleHS.AddChild(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FRangedPathHS);
                                FSelectedAbility = A1;
                                Result = Index;
                            }
                        }
                        else
                        {
                            FSelectedAbility = A1;
                            Result = Index;
                        }
                    }
                }
            }

            {
                I = 0;
                int end__507 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetCount() - 1;
                for (; I <= end__507; I += 1)
                {
                    {
                        var w__508 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetMoveSettings(I);
                        w__508.FPath.Clear();
                    }
                }
            }

            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FRangedPath.Clear();
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.UpdateMoveCursor();
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitChangedEvents.TriggerNotifyEvents(this);
            return Result;
        }

        // AoWTC.pas:3870
        public int SelectSpell(int A1)
        {
            int Result = default;
            int I = default;
            Aow.Units.AoWE.TSpellCombatInfo Info = default;
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.DeactivateRangedPath();
            FSelectedSpell = A1;
            var call__509 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSelectedSpell);
            call__509.GetCombatInfo(FUnit.FUnit, ref Info);
            if (Info.Range != 0)
            {
                if (Info.Range == -1)
                {
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FRangedPathHS.FRange = 100;
                }
                else
                {
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FRangedPathHS.FRange = unchecked((ushort)Info.Range);
                }

                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FRangedPathHS.FSelection = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection;
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.DynamicFixedVisibleHS.AddChild(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FRangedPathHS);
            }

            Result = A1;
            {
                I = 0;
                int end__510 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetCount() - 1;
                for (; I <= end__510; I += 1)
                {
                    {
                        var w__511 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetMoveSettings(I);
                        w__511.FPath.Clear();
                    }
                }
            }

            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FRangedPath.Clear();
            switch (Aow.Units.AoWTC.AoWTC.SpellTypes[A1])
            {
                case Aow.Units.AoWTC.TfxType.fxScreenNESW:
                case Aow.Units.AoWTC.TfxType.fxSacredWrath:
                case Aow.Units.AoWTC.TfxType.fxTremors:
                case Aow.Units.AoWTC.TfxType.fxHighPrayer:
                case Aow.Units.AoWTC.TfxType.fxShockWave:
                    AbRangedTC(0, 0);
                    Result = -1;
                    Unselect();
                    break;
            }

            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.UpdateMoveCursor();
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitChangedEvents.TriggerNotifyEvents(this);
            return Result;
        }

        // AoWTC.pas:3895
        public void CreateTCAbList()
        {
            int I = default;
            Aow.Units.AoWTC.TTCAbility Ability = default;
            {
                I = 0;
                int end__512 = FAbilities.Count - 1;
                for (; I <= end__512; I += 1)
                {
                    var call__513 = FAbilities[I];
                    ((Aow.Rtl.TObject)call__513).Free();
                }
            }

            FAbilities.Clear();
            FCombatAbilityCount = 0;
            {
                I = 1;
                byte end__514 = 169;
                for (; I <= end__514; I += 1)
                {
                    var call__515 = FUnit.GetAbilityEnabled(I);
                    if (call__515)
                    {
                        Ability = Aow.Units.AoWTC.TTCAbility.Create();
                        Ability.Ability = I;
                        var call__516 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                        var call__517 = call__516.GetControlType(FUnit.FUnit);
                        bool bool__518 = call__517.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acCombatAction);
                        if (bool__518)
                        {
                            var call__519 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                            var call__520 = call__519.GetCombatMode(FUnit.FUnit);
                            bool__518 = (call__520 != Aow.Units.AoWE.TCombatMode.cmMelee);
                        }

                        if (bool__518)
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
        }

        // AoWTC.pas:3910
        public void ListAbilities(Aow.Rtl.TStrings Names, Aow.Rtl.TStrings Types, Aow.Rtl.TStrings Sources)
        {
            int I = default;
            Names.Clear();
            if (Types != null)
            {
                Types.Clear();
            }

            if (Sources != null)
            {
                Sources.Clear();
            }

            {
                I = 0;
                int end__521 = FAbilities.Count - 1;
                for (; I <= end__521; I += 1)
                {
                    var call__522 = FAbilities[I];
                    var call__523 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TTCAbility)call__522).Ability);
                    var call__524 = call__523.GetName(FUnit.FUnit);
                    var call__525 = FAbilities[I];
                    Names.AddObject(call__524, Aow.Rtl.Rtl.IntToObject((int)((Aow.Units.AoWTC.TTCAbility)call__525).Ability));
                    if (Types != null)
                    {
                        var call__526 = FAbilities[I];
                        var call__527 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TTCAbility)call__526).Ability);
                        var call__528 = call__527.GetAbilityType();
                        var call__529 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TAbilityTypeRStr[(int)call__528]);
                        Types.Add(call__529);
                    }

                    if (Sources != null)
                    {
                        var call__530 = FAbilities[I];
                        var call__531 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TTCAbility)call__530).Ability);
                        var call__532 = call__531.GetSourceName(FUnit.FUnit);
                        Sources.Add(call__532);
                    }
                }
            }
        }

        // AoWTC.pas:3923
        public void SetAction(Aow.Units.AoWTC.TTCAction Action)
        {
            FActionDone = false;
            switch (Action)
            {
                case Aow.Units.AoWTC.TTCAction.caStepUp:
                case Aow.Units.AoWTC.TTCAction.caStepBack:
                    FActionFrame = 0;
                    break;
                case Aow.Units.AoWTC.TTCAction.caRoundAttack:
                case Aow.Units.AoWTC.TTCAction.caRoundAttackHit:
                    if ((FAction != Aow.Units.AoWTC.TTCAction.caRoundAttack) && (FAction != Aow.Units.AoWTC.TTCAction.caRoundAttackHit))
                    {
                        FActionFrame = unchecked((short)-1);
                    }

                    break;
                default:
                    FActionFrame = unchecked((short)-1);
                    break;
            }

            FAction = Action;
        }

        // AoWTC.pas:3935
        public bool AbTouchMoveTC(Aow.Units.AoWE.TMovepath Path)
        {
            bool Result = default;
            Aow.Units.NetworkE.TTokenEvent TE = default;
            Aow.Units.AoWTC.Record_TTacticalCombatUnitHS_AbTouchMoveTC_Buffer_3936_22 Buffer = default;
            Result = false;
            var call__533 = FUnit.GetPlayer();
            var call__534 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(call__533);
            if (call__534 && (Path.Count > 0))
            {
                if (Path.Count > 1)
                {
                    var call__535 = Aow.Units.CombatTE.CombatTE.CreateTCAbTouchMoveTE(FUnit, Path, FSelectedAbility);
                    TE = call__535;
                    if (TE != null)
                    {
                        var call__536 = FUnit.GetPlayer();
                        var call__537 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(call__536);
                        call__537.AddEvent(TE);
                        TE.Release();
                        Result = true;
                    }
                }
                else
                {
                    var call__538 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FSelectedAbility);
                    call__538.GetCombatInfo(FUnit.FUnit, ref Buffer.Info);
                    if (Buffer.Info.TargetTypes.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend))
                    {
                        var call__539 = Path[0];
                        var call__540 = Path[0];
                        AbRangedTC(unchecked((ushort)call__540.X), unchecked((ushort)call__539.Y));
                    }
                }
            }

            return Result;
        }

        // AoWTC.pas:3952
        public bool AbRangedTC(ushort X, ushort Y)
        {
            bool Result = default;
            Aow.Units.CombatTE.TCAbRangedTE TE = default;
            Result = false;
            var call__541 = FUnit.GetPlayer();
            var call__542 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(call__541);
            if (call__542)
            {
                var call__543 = Aow.Units.CombatTE.CombatTE.CreateTCAbRangedTE(FUnit, unchecked((ushort)X), unchecked((ushort)Y), FSelectedAbility, FSelectedSpell);
                TE = call__543;
                if (TE != null)
                {
                    var call__544 = FUnit.GetPlayer();
                    var call__545 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(call__544);
                    call__545.AddEvent(TE);
                    TE.Release();
                    Result = true;
                }
            }

            return Result;
        }

        // intf_P2_03.inc:74
        public static new Aow.Units.AoWTC.TTacticalCombatUnitHS Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWTC.TTacticalCombatUnitHS o = new Aow.Units.AoWTC.TTacticalCombatUnitHS();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.AoWE.TUnitHS.Meta
        {
            // AoWTC.pas:3348
            public override int ClassID()
            {
                int Result = default;
                Result = 2228484;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AoWTC.TTacticalCombatUnitHS => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P2_03.inc:74
            public new Aow.Units.AoWTC.TTacticalCombatUnitHS Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AoWTC.TTacticalCombatUnitHS o = (Aow.Units.AoWTC.TTacticalCombatUnitHS)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TCombatUnitSelectionControl (instance size 60, VMT 00412EB4): $18 FPathInvalid;
//     $1C FTargetImages; $20 FRangedPathHS; $24 FTargetX; $28 FTargetY; $2C FCursorImage; $2E FCursorFrame; $34 FRangedPath; $38 FObstacles. }
    public partial class TCombatUnitSelectionControl : Aow.Units.AoWE.TMoveSelector
    {
        public TSet<Aow.Units.AoWTC.AnonymousEnum_piMove> FPathInvalid;
        // { $18 }
        public Aow.Units.FixedArray_byte_3 F_CombatUnitSelectionControl_19;
        public Aow.Units.ImageLib.TCustomImageLibrary FTargetImages;
        // { $1C }
        public Aow.Units.AoWTC.TRangedPathHS FRangedPathHS;
        // { $20 }
        public int FTargetX;
        // { $24 }
        public int FTargetY;
        // { $28 }
        public ushort FCursorImage;
        public ushort FCursorFrame;
        public Aow.Units.HSEngine.THexagonSprite FMouseOverTarget;
        // { $30; $436B53 }
        public Aow.Units.aowFX.TXYZList FRangedPath;
        // { $34 }
        public Aow.Rtl.TList FObstacles;
        // AoWTC.pas:2733
        // { $38 }
        public override void Destroy()
        {
            int I = default;
            {
                I = 0;
                int end__546 = FObstacles.Count - 1;
                for (; I <= end__546; I += 1)
                {
                    var call__547 = FObstacles[I];
                    ((Aow.Rtl.TObject)call__547).Free();
                }
            }

            FObstacles.Free();
            FRangedPath.Free();
            FRangedPathHS.Free();
            Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TargHex.ILB");
            base.Destroy();
        }

        // AoWTC.pas:3080
        public override void MouseDownEvent(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWTC.TTacticalCombatUnitHS UnitHS = default;
            int Index = default;
            int DX = default;
            int DY = default;
            TSet<Aow.Units.AoWE.TCombatTargetTypeElement> Targets = default;
            Aow.Units.AoWE.TSpellCombatInfo SpellInfo = default;
            Aow.Units.AoWE.TAbilityCombatInfo AbilityInfo = default;
            // { Match trials (8): addressable/absolute locals, record storage, declaration/operand order, with scopes, Pointer casts, zero-shift scheduling, O+; still differs. }
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)
            {
                var call__548 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer));
                if (!call__548.FAuto)
                {
                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Button == Aow.Rtl.TMouseButton.mbRight)
                    {
                        var call__549 = GetTCUnitHS();
                        bool bool__550 = (call__549 != null);
                        if (bool__550)
                        {
                            var call__551 = GetTCUnitHS();
                            bool__550 = (call__551.FSelectedAbility > -1);
                        }

                        if (bool__550)
                        {
                            var call__552 = GetTCUnitHS();
                            call__552.SelectAbility(-1);
                            return;
                        }

                        Unselect();
                    }
                    else
                    {
                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Shift.Contains(Aow.Rtl.TShiftState.ssShift))
                        {
                            var call__553 = FMoveSettingsList.GetCount();
                            bool bool__554 = (call__553 == 1);
                            if (bool__554)
                            {
                                var call__555 = FMoveSettingsList.GetMoveSettings(0);
                                bool__554 = (call__555.FPath.Count != 0);
                            }

                            if (bool__554)
                            {
                                var call__556 = FMoveSettingsList.GetMoveSettings(0);
                                var call__557 = call__556.FPath[0];
                                var call__558 = FMoveSettingsList.GetMoveSettings(0);
                                var call__559 = call__558.FPath[0];
                                var call__560 = FMoveSettingsList.GetMoveSettings(0);
                                var call__561 = call__560.FPath[0];
                                var call__562 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__561.X, call__559.Y, call__557.L);
                                var call__563 = ValidMultiMoveSelection((Aow.Units.AoWE.TAoWMapField)call__562);
                                if (!call__563)
                                {
                                    return;
                                }
                            }

                            var call__564 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                            if (call__564 != -1)
                            {
                                var call__565 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                var call__566 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                var call__567 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__566, call__565, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                var call__568 = call__567.FindOwnedChild(2228484);
                                UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__568;
                                if ((UnitHS != null) && (UnitHS.FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer))
                                {
                                    var call__569 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                    Index = call__569;
                                    if (Index == -1)
                                    {
                                        AddUnit(UnitHS);
                                        Update();
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
                                        var call__570 = UnitHS.FUnit.GetLocked();
                                        if (call__570)
                                        {
                                            var call__571 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                            var call__572 = FMoveSettingsList.GetMoveSettings(call__571);
                                            call__572.FPath.Clear();
                                        }
                                        else
                                        {
                                            var call__573 = FMoveSettingsList.GetMoveSettings(0);
                                            if (call__573.FPath.Count > 0)
                                            {
                                                var call__574 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__575 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__576 = call__575.FPath[call__574.FPath.Count - 1];
                                                var call__577 = UnitHS.GetXhx();
                                                DX = call__577 - call__576.X;
                                                var call__578 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__579 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__580 = call__579.FPath[call__578.FPath.Count - 1];
                                                var call__581 = UnitHS.GetYhx();
                                                DY = call__581 - call__580.Y;
                                                var call__582 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__583 = call__582.FPath[0];
                                                var call__584 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__585 = call__584.FPath[0];
                                                var call__586 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                                CalculateMovePath(call__586, unchecked((sbyte)(call__585.X + DX)), unchecked((sbyte)(call__583.Y + DY)), 0);
                                            }
                                        }

                                        SetCurrentMove(-1);
                                    }
                                    else
                                    {
                                        UnitHS.Unselect();
                                        DeleteMoveObject(Index);
                                        var call__587 = FMoveSettingsList.GetCount();
                                        SetCurrentMove(call__587 - 1);
                                        Update();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Shift.Contains(Aow.Rtl.TShiftState.ssCtrl))
                            {
                                var call__588 = FMoveSettingsList.GetCount();
                                bool bool__589 = (call__588 > 0);
                                if (bool__589)
                                {
                                    var call__590 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                    bool__589 = (call__590 != -1);
                                }

                                if (bool__589)
                                {
                                    var call__591 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                    var call__592 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                    var call__593 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__592, call__591, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                    var call__594 = call__593.FindOwnedChild(2228484);
                                    UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__594;
                                    if ((UnitHS != null) && (UnitHS.FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer))
                                    {
                                        var call__595 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                        Index = call__595;
                                        if (Index == -1)
                                        {
                                            AddUnit(UnitHS);
                                            Update();
                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
                                            var call__596 = UnitHS.FUnit.GetLocked();
                                            if (call__596)
                                            {
                                                var call__597 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                                var call__598 = FMoveSettingsList.GetMoveSettings(call__597);
                                                call__598.FPath.Clear();
                                            }
                                            else
                                            {
                                                var call__599 = FMoveSettingsList.GetMoveSettings(0);
                                                if (call__599.FPath.Count > 0)
                                                {
                                                    var call__600 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__601 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__602 = call__601.FPath[call__600.FPath.Count - 1];
                                                    var call__603 = UnitHS.GetXhx();
                                                    DX = call__603 - call__602.X;
                                                    var call__604 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__605 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__606 = call__605.FPath[call__604.FPath.Count - 1];
                                                    var call__607 = UnitHS.GetYhx();
                                                    DY = call__607 - call__606.Y;
                                                    var call__608 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__609 = call__608.FPath[0];
                                                    var call__610 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__611 = call__610.FPath[0];
                                                    var call__612 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                                    CalculateMovePath(call__612, unchecked((sbyte)(call__611.X + DX)), unchecked((sbyte)(call__609.Y + DY)), 0);
                                                }
                                            }

                                            var call__613 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                            SetCurrentMove(call__613);
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
                                var call__614 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                if (call__614 != -1)
                                {
                                    if (GetTCUnitHS() != null)
                                    {
                                        var call__615 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                        var call__616 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                        var call__617 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__616, call__615, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                        var call__618 = call__617.FindOwnedChild(2228484);
                                        UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__618;
                                        var call__619 = GetTCUnitHS();
                                        if (call__619.FSelectedAbility == -1)
                                        {
                                            Targets = TSet<Aow.Units.AoWE.TCombatTargetTypeElement>.Of(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy);
                                        }
                                        else
                                        {
                                            var call__620 = GetTCUnitHS();
                                            if (call__620.FSelectedAbility == 52)
                                            {
                                                var call__621 = GetTCUnitHS();
                                                var call__622 = GetTCUnitHS();
                                                var call__623 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(call__622.FSelectedSpell);
                                                call__623.GetCombatInfo(call__621.FUnit.FUnit, ref SpellInfo);
                                                Targets = SpellInfo.TargetTypes;
                                            }
                                            else
                                            {
                                                var call__624 = GetTCUnitHS();
                                                var call__625 = GetTCUnitHS();
                                                var call__626 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(call__625.FSelectedAbility);
                                                call__626.GetCombatInfo(call__624.FUnit.FUnit, ref AbilityInfo);
                                                Targets = AbilityInfo.TargetTypes;
                                            }
                                        }

                                        if (UnitHS == null)
                                        {
                                            ExecuteMoveSelection();
                                        }
                                        else
                                        {
                                            var call__627 = GetTCUnitHS();
                                            var call__628 = call__627.FUnit.GetPlayer();
                                            var call__629 = UnitHS.FUnit.GetPlayer();
                                            if ((call__629 == call__628) && !Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend))
                                            {
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.UnselectLocation();
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
                                            }
                                            else
                                            {
                                                var call__630 = GetTCUnitHS();
                                                var call__631 = call__630.FUnit.GetPlayer();
                                                var call__632 = UnitHS.FUnit.GetPlayer();
                                                bool bool__633 = ((call__632 == call__631) && Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend));
                                                if (!bool__633)
                                                {
                                                    var call__634 = GetTCUnitHS();
                                                    var call__635 = call__634.FUnit.GetPlayer();
                                                    var call__636 = UnitHS.FUnit.GetPlayer();
                                                    bool__633 = ((call__636 != call__635) && Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy));
                                                }

                                                if (bool__633)
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
                var call__637 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                var call__638 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                var call__639 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__638, call__637, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                var call__640 = call__639.FindOwnedChild(2228484);
                UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__640;
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.UnselectLocation();
                if ((UnitHS != null) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Button == Aow.Rtl.TMouseButton.mbLeft))
                {
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
                }
            }
        }

        // AoWTC.pas:2823
        public override void CalculateMovePath(int Index, sbyte X, sbyte Y, sbyte L)
        {
            Aow.Units.AoWTC.TTacticalCombatUnitHS UnitHS = default;
            int I = default;
            Aow.Rtl.TList Temporary = default;
            Aow.Units.FleeLoc.TMovementHS Movement = default;
            {
                var w__641 = FMoveSettingsList.GetMoveSettings(Index);
                Temporary = new Aow.Rtl.TList();
                var call__642 = FMoveSettingsList.GetCount();
                if (call__642 > 0)
                {
                    {
                        I = 0;
                        int end__643 = FMoveSettingsList.GetCount() - 1;
                        for (; I <= end__643; I += 1)
                        {
                            if (I != Index)
                            {
                                var call__644 = FMoveSettingsList.GetMoveSettings(I);
                                if (call__644.FPath.Count > 1)
                                {
                                    Movement = Aow.Units.FleeLoc.TMovementHS.Create(null);
                                    var call__645 = FMoveSettingsList.GetMoveSettings(I);
                                    var call__646 = FMoveSettingsList.GetMoveSettings(I);
                                    var call__647 = call__646.FPath[call__645.FPath.Count - 1];
                                    var call__648 = FMoveSettingsList.GetMoveSettings(I);
                                    var call__649 = FMoveSettingsList.GetMoveSettings(I);
                                    var call__650 = call__649.FPath[call__648.FPath.Count - 1];
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(Movement, call__650.X, call__647.Y, 0);
                                    Movement.FEnabled = true;
                                    Temporary.Add(Movement);
                                    Movement = Aow.Units.FleeLoc.TMovementHS.Create(null);
                                    var call__651 = FMoveSettingsList.GetMoveSettings(I);
                                    var call__652 = call__651.FPath[0];
                                    var call__653 = FMoveSettingsList.GetMoveSettings(I);
                                    var call__654 = call__653.FPath[0];
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(Movement, call__654.X, call__652.Y, 0);
                                    Movement.FEnabled = false;
                                    Temporary.Add(Movement);
                                }
                            }
                        }
                    }
                }

                w__641.FPathMovePoints = 0;
                UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)w__641.FMoveObject;
                var call__655 = GetTCUnitHS();
                bool bool__656 = (call__655 != null);
                if (bool__656)
                {
                    var call__657 = GetTCUnitHS();
                    bool bool__658 = (call__657.FSelectedAbility == 117);
                    if (!bool__658)
                    {
                        var call__659 = GetTCUnitHS();
                        bool__658 = (call__659.FSelectedAbility == 65);
                    }

                    bool__656 = bool__658;
                }

                if (bool__656)
                {
                    ((Aow.Units.AoWTC.TCombatMoveControlSettings)FMoveControlSettings).FMoveFlags = TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.FromMask((ulong)((byte)FPathInvalid.Image)) + TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmClimbing);
                }
                else
                {
                    ((Aow.Units.AoWTC.TCombatMoveControlSettings)FMoveControlSettings).FMoveFlags = TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.FromMask((ulong)((byte)FPathInvalid.Image));
                }

                FMoveControlSettings.Reset();
                var call__660 = UnitHS.GetLhx();
                var call__661 = UnitHS.GetYhx();
                var call__662 = UnitHS.GetXhx();
                FMoveControlSettings.SetSourcePos(call__662, call__661, call__660);
                var call__663 = UnitHS.FUnit.GetPlayer();
                FMoveControlSettings.FPlayer = unchecked((sbyte)call__663);
                FMoveControlSettings.FArmyHS = UnitHS;
                FMoveControlSettings.FSelection = 0;
                FMoveControlSettings.SetDestinationPos(X, Y, L);
                UnitHS.FUnit.FUnit.CreateMovePointTable(ref FMoveControlSettings.FMovePointTable);
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.ProcessMove(FMoveControlSettings);
                var call__664 = FMoveSettingsList.GetMoveSettings(Index);
                call__664.FPath.Assign(FMoveControlSettings.FPath);
                var call__665 = FMoveSettingsList.GetMoveSettings(Index);
                call__665.FMovePoints = UnitHS.FUnit.FMovePoints;
                var call__666 = UnitHS.FUnit.GetMoves();
                var call__667 = FMoveSettingsList.GetMoveSettings(Index);
                call__667.FMoves = call__666;
                if (FMoveControlSettings.FPath.Count != 0)
                {
                    {
                        I = 0;
                        int end__668 = FMoveControlSettings.FPath.Count - 2;
                        for (; I <= end__668; I += 1)
                        {
                            var call__669 = FMoveControlSettings.FPath[I];
                            var call__670 = FMoveControlSettings.FPath[I];
                            var call__671 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__672 = call__671.Field[call__670.X, call__669.Y];
                            var call__673 = call__672.FindOwnedChild(2229508);
                            bool bool__674 = (call__673 != null);
                            if (!bool__674)
                            {
                                var call__675 = FMoveControlSettings.FPath[I];
                                var call__676 = FMoveControlSettings.FPath[I];
                                var call__677 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__678 = call__677.Field[call__676.X, call__675.Y];
                                var call__679 = call__678.FindOwnedChild(2229510);
                                bool__674 = (call__679 != null);
                            }

                            if (bool__674)
                            {
                                var call__680 = FMoveControlSettings.FPath[I];
                                var call__681 = FMoveControlSettings.FPath[I];
                                var call__682 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__683 = call__682.Field[call__681.X, call__680.Y];
                                w__641.FPath.List[I].Flag = unchecked((sbyte)FMoveControlSettings.FMovePointTable[call__683.TerrainType][-1 - -1]);
                            }
                            else
                            {
                                var call__684 = FMoveControlSettings.FPath[I];
                                var call__685 = FMoveControlSettings.FPath[I];
                                var call__686 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__687 = call__686.Field[call__685.X, call__684.Y];
                                var call__688 = FMoveControlSettings.FPath[I];
                                var call__689 = FMoveControlSettings.FPath[I];
                                var call__690 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__691 = call__690.Field[call__689.X, call__688.Y];
                                w__641.FPath.List[I].Flag = unchecked((sbyte)FMoveControlSettings.FMovePointTable[call__691.TerrainType][call__687.OverlayType - -1]);
                            }
                        }
                    }

                    var call__692 = w__641.FPath.PosToMovePoints(0);
                    w__641.FPathMovePoints = call__692;
                    FMoveControlSettings.FPath.Clear();
                }

                {
                    I = 0;
                    int end__693 = Temporary.Count - 1;
                    for (; I <= end__693; I += 1)
                    {
                        var call__694 = Temporary[I];
                        ((Aow.Rtl.TObject)call__694).Free();
                    }
                }

                Temporary.Free();
            }
        }

        // AoWTC.pas:3178
        public override void UpdateMoveCursor()
        {
            Aow.Units.AoWE.TAoWMapField Field = default;
            int Cost = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS UnitHS = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS TargetHS = default;
            TSet<Aow.Units.AoWE.TCombatTargetTypeElement> Targets = default;
            Aow.Units.CityWall.TCityWall Wall = default;
            Aow.Units.AoWE.TSpellCombatInfo SpellInfo = default;
            Aow.Units.AoWTC.Record_TCombatUnitSelectionControl_UpdateMoveCursor_AbilityInfo_3187_3 AbilityInfo = default;
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap == null)
            {
                return;
            }

            UnitHS = GetTCUnitHS();
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.CursorMode = 0;
            var call__695 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
            if (call__695 == -1)
            {
                return;
            }

            var call__696 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
            if (call__696 == -1)
            {
                return;
            }

            if (UnitHS == null)
            {
                return;
            }

            var call__697 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
            var call__698 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
            var call__699 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__698, call__697, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
            Field = (Aow.Units.AoWE.TAoWMapField)call__699;
            bool bool__700 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
            if (bool__700)
            {
                var call__701 = Field.GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                bool__700 = !call__701;
            }

            if (bool__700)
            {
                goto FinishCursor;
            }

            TargetHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)Field.FindOwnedChild(2228484);
            if ((UnitHS.FUnit.FCombatPlayer != Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer) && (TargetHS != null))
            {
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.CursorMode = 257;
                goto FinishCursor;
            }

            if (UnitHS.FSelectedAbility > -1)
            {
                FCursorFrame = (ushort)(((int)FCursorFrame) + 1);
                if (FCursorFrame >= 5)
                {
                    FCursorFrame = 0;
                    FCursorImage = (ushort)(((int)FCursorImage) + 1);
                }

                if (UnitHS.FSelectedAbility == 52)
                {
                    var call__702 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(UnitHS.FSelectedSpell);
                    call__702.GetCombatInfo(UnitHS.FUnit.FUnit, ref SpellInfo);
                    Targets = SpellInfo.TargetTypes;
                }
                else
                {
                    var call__703 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                    call__703.GetCombatInfo(UnitHS.FUnit.FUnit, ref AbilityInfo.Data);
                    Targets = AbilityInfo.Data.TargetTypes;
                }

                if ((TargetHS == null) || ((TargetHS == UnitHS) && !Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend)))
                {
                    if ((FCursorImage < 85) || (FCursorImage > 88))
                    {
                        FCursorImage = 85;
                    }
                }
                else
                {
                    var call__704 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                    var call__705 = call__704.GetCombatMode(UnitHS.FUnit.FUnit);
                    if (call__705 == Aow.Units.AoWE.TCombatMode.cmTouch)
                    {
                        bool bool__706 = Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend);
                        if (bool__706)
                        {
                            var call__707 = UnitHS.FUnit.GetSide();
                            var call__708 = TargetHS.FUnit.GetSide();
                            bool__706 = (call__707 == call__708);
                        }

                        bool bool__709 = bool__706;
                        if (!bool__709)
                        {
                            bool bool__710 = Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy);
                            if (bool__710)
                            {
                                var call__711 = UnitHS.FUnit.GetSide();
                                var call__712 = TargetHS.FUnit.GetSide();
                                bool__710 = (call__711 != call__712);
                            }

                            bool__709 = bool__710;
                        }

                        bool bool__713 = bool__709;
                        if (bool__713)
                        {
                            var call__714 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                            var call__715 = ((Aow.Units.AoWE.TTouchAbility)call__714).CanTouch(UnitHS.FUnit, TargetHS.FUnit);
                            bool__713 = call__715;
                        }

                        if (bool__713)
                        {
                            if ((FCursorImage < 105) || (FCursorImage > 107))
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
                        bool bool__716 = Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend);
                        if (bool__716)
                        {
                            var call__717 = UnitHS.FUnit.GetSide();
                            var call__718 = TargetHS.FUnit.GetSide();
                            bool__716 = (call__717 == call__718);
                        }

                        bool bool__719 = bool__716;
                        if (!bool__719)
                        {
                            bool bool__720 = Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy);
                            if (bool__720)
                            {
                                var call__721 = UnitHS.FUnit.GetSide();
                                var call__722 = TargetHS.FUnit.GetSide();
                                bool__720 = (call__721 != call__722);
                            }

                            bool__719 = bool__720;
                        }

                        if (bool__719)
                        {
                            if ((FCursorImage < 80) || (FCursorImage > 83))
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

                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.CursorMode = 256;
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Cursor = FCursorImage;
            }
            else
            {
                if ((TargetHS != null) && (TargetHS != UnitHS))
                {
                    var call__723 = TargetHS.FUnit.GetPlayer();
                    var call__724 = UnitHS.FUnit.GetPlayer();
                    if (call__723 == call__724)
                    {
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.CursorMode = 257;
                    }
                    else
                    {
                        var call__725 = UnitHS.FUnit.GetAbilityEnabled(1);
                        bool bool__726 = call__725;
                        if (!bool__726)
                        {
                            var call__727 = TargetHS.FUnit.GetAbilityEnabled(1);
                            bool__726 = !call__727;
                        }

                        if (bool__726)
                        {
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.CursorMode = 258;
                        }
                        else
                        {
                            FCursorImage = 89;
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.CursorMode = 256;
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Cursor = FCursorImage;
                        }
                    }
                }
            }

            FinishCursor:
                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.CursorMode != 0)
                {
                    return;
                }

            var call__728 = UnitHS.FUnit.GetPlayer();
            if (call__728 == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer)
            {
                if (GetMoveTypes().Contains(Aow.Units.AoWE.TMoveType.mtWalking))
                {
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Cursor = 64;
                }
                else
                {
                    if (GetMoveTypes().Contains(Aow.Units.AoWE.TMoveType.mtSwimming))
                    {
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Cursor = 66;
                    }
                    else
                    {
                        if (GetMoveTypes().Contains(Aow.Units.AoWE.TMoveType.mtFlying))
                        {
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Cursor = 68;
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
                    Wall = (Aow.Units.CityWall.TCityWall)Field.FindOwnedChild(2229508);
                    if (Wall == null)
                    {
                        Wall = (Aow.Units.CityWall.TCityWall)Field.FindOwnedChild(2229510);
                    }

                    if (Wall != null)
                    {
                        bool bool__729 = !(Wall is Aow.Units.CityWall.TCityWallDoor);
                        if (!bool__729)
                        {
                            var call__730 = UnitHS.FUnit.GetSide();
                            bool__729 = (call__730 == Aow.Units.AoWE.TCombatSideIndex.csAttacker);
                        }

                        if (bool__729)
                        {
                            Cost = -1;
                        }

                        bool bool__731 = !(Wall is Aow.Units.CityWall.TCityWallDoor);
                        if (bool__731)
                        {
                            var call__732 = UnitHS.FUnit.FUnit.GetAbilityEnabled(36);
                            bool__731 = call__732;
                        }

                        if (bool__731)
                        {
                            Cost = 1;
                        }

                        var call__733 = UnitHS.FUnit.FUnit.GetAbilityEnabled(66);
                        bool bool__734 = call__733;
                        if (!bool__734)
                        {
                            var call__735 = UnitHS.FUnit.FUnit.GetAbilityEnabled(1);
                            bool__734 = call__735;
                        }

                        if (bool__734)
                        {
                            Cost = 1;
                        }
                    }
                }

                bool bool__736 = (Cost <= 0);
                if (!bool__736)
                {
                    bool bool__737 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
                    if (bool__737)
                    {
                        var call__738 = Field.GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                        bool__737 = !call__738;
                    }

                    bool__736 = bool__737;
                }

                if (bool__736)
                {
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Cursor = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Cursor + 1;
                }

                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.CursorMode = 256;
            }
        }

        // AoWTC.pas:2744
        public override Aow.Units.AoWE.TMoveControlSettings CreateMCS()
        {
            Aow.Units.AoWE.TMoveControlSettings Result = default;
            Result = Aow.Units.AoWTC.TCombatMoveControlSettings.Create(null);
            return Result;
        }

        // AoWTC.pas:2754
        public override void SetCurrentMove(int A1)
        {
            bool bool__739 = (FCurrentMove != -1);
            if (bool__739)
            {
                var call__740 = GetTCUnitHS();
                bool__739 = (call__740 != null);
            }

            if (bool__739)
            {
                var call__741 = GetTCUnitHS();
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.TriggerDeselectEvent(call__741);
            }

            base.SetCurrentMove(A1);
            bool bool__742 = (FCurrentMove != -1);
            if (bool__742)
            {
                var call__743 = GetTCUnitHS();
                bool__742 = (call__743 != null);
            }

            if (bool__742)
            {
                var call__744 = GetTCUnitHS();
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.TriggerSelectEvent(call__744);
            }
        }

        // AoWTC.pas:3316
        public override void Unselect()
        {
            DeactivateRangedPath();
            if (GetTCUnitHS() != null)
            {
                var call__745 = GetTCUnitHS();
                call__745.FSelectedAbility = -1;
            }

            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.UnselectLocation();
            DeactivateMoveSelection();
            base.Unselect();
        }

        // AoWTC.pas:3274
        public override void ShowMovePaths(in Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            int I = default;
            int X = default;
            int Y = default;
            int Xhp = default;
            int Yhp = default;
            base.ShowMovePaths(Surface);
            if (GetTCUnitHS() != null)
            {
                var call__746 = GetTCUnitHS();
                bool bool__747 = (call__746.FSelectedAbility != 65);
                if (bool__747)
                {
                    var call__748 = GetTCUnitHS();
                    bool__747 = (call__748.FSelectedAbility != 114);
                }

                if (bool__747)
                {
                    return;
                }

                var call__749 = FMoveSettingsList.GetCount();
                bool bool__750 = (call__749 == 1);
                if (bool__750)
                {
                    var call__751 = FMoveSettingsList.GetMoveSettings(0);
                    bool__750 = (call__751.FPath.Count > 1);
                }

                if (bool__750)
                {
                    {
                        I = 1;
                        byte end__752 = 6;
                        for (; I <= end__752; I += 1)
                        {
                            var call__753 = FMoveSettingsList.GetMoveSettings(0);
                            var call__754 = call__753.FPath[1];
                            var call__755 = FMoveSettingsList.GetMoveSettings(0);
                            var call__756 = call__755.FPath[1];
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__756.X, call__754.Y, I, ref X, ref Y);
                            Aow.Units.HSEngine.HSEngine.HXtoHP(X, Y, ref Xhp, ref Yhp);
                            var call__757 = FTargetImages[0];
                            call__757.Show((Xhp - Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewXhp) - 8, (Yhp - Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewYhp) + 2, Surface);
                        }
                    }
                }
            }
        }

        // AoWTC.pas:2723
        public void Create__init_TCombatUnitSelectionControl()
        {
            base.Create__init_TMoveSelector();
            var call__758 = Aow.Units.aowFX.TXYZList.Create();
            FRangedPath = call__758;
            var call__759 = new Aow.Rtl.TList();
            FObstacles = call__759;
            var call__760 = Aow.Units.AoWTC.TRangedPathHS.Create(null);
            FRangedPathHS = call__760;
            FRangedPathHS.Activate();
            var call__761 = Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TargHex.ILB");
            FTargetImages = call__761;
        }

        // AoWTC.pas:2747
        public int AddUnit(Aow.Units.AoWTC.TTacticalCombatUnitHS U)
        {
            int Result = default;
            Result = AddMoveObject(U);
            var call__762 = FMoveSettingsList.GetMoveSettings(Result);
            call__762.FMovePoints = U.FUnit.FMovePoints;
            var call__763 = U.FUnit.GetMoves();
            var call__764 = FMoveSettingsList.GetMoveSettings(Result);
            call__764.FMoves = call__763;
            return Result;
        }

        // AoWTC.pas:2763
        public Aow.Units.AoWTC.TTacticalCombatUnitHS GetTCUnitHS()
        {
            Aow.Units.AoWTC.TTacticalCombatUnitHS Result = default;
            if (FCurrentMove != -1)
            {
                var call__765 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                Result = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__765.FMoveObject;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // AoWTC.pas:2770
        public TSet<Aow.Units.AoWE.TMoveType> GetMoveTypes()
        {
            TSet<Aow.Units.AoWE.TMoveType> Result = default;
            if (FCurrentMove != -1)
            {
                var call__766 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                var call__767 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__766.FMoveObject).FUnit.FUnit.GetMoveTypes();
                Result = call__767;
            }
            else
            {
                Result = TSet<Aow.Units.AoWE.TMoveType>.Of();
            }

            return Result;
        }

        // AoWTC.pas:2777
        public void RectangleSelectEvent(Aow.Rtl.TObject Sender)
        {
            int I = default;
            int X = default;
            int Y = default;
            int X1 = default;
            int Y1 = default;
            int X2 = default;
            int Y2 = default;
            int PX = default;
            int PY = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS UnitHS = default;
            Aow.Units.HSEngine.TMapField Field = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Rtl.TList Units = default;
            Aow.Rtl.TRect R = default;
            Units = new Aow.Rtl.TList();
            R = ((Aow.Units.AoWE.TAbstractAoWHSMapMouse)Sender).FSelectionRectangle;
            var call__768 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL];
            Level = call__768;
            Aow.Units.HSEngine.HSEngine.HPtoHX(R.Left, R.Top, ref X1, ref Y1);
            Aow.Units.HSEngine.HSEngine.HPtoHX(R.Right, R.Bottom, ref X2, ref Y2);
            {
                X = X1 - 1;
                int end__769 = X2 + 1;
                for (; X <= end__769; X += 1)
                {
                    {
                        Y = Y1 - 1;
                        int end__770 = Y2 + 1;
                        for (; Y <= end__770; Y += 1)
                        {
                            Aow.Units.HSEngine.HSEngine.HXtoHP(X, Y, ref PX, ref PY);
                            PX += 16;
                            PY += 8;
                            if ((((((((X >= 0) && (X < Level.Width)) && (Y > 0)) && (Y < Level.Height)) && (PX >= R.Left)) && (PX < R.Right)) && (PY >= R.Top)) && (PY < R.Bottom))
                            {
                                var call__771 = Level.Field[X, Y];
                                Field = call__771;
                                UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)Field.FindOwnedChild(2228484);
                                if ((UnitHS != null) && (UnitHS.FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer))
                                {
                                    Units.Add(UnitHS);
                                }
                            }
                        }
                    }
                }
            }

            if (Units.Count != 0)
            {
                Clear();
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.UnselectLocation();
                {
                    I = 0;
                    int end__772 = Units.Count - 1;
                    for (; I <= end__772; I += 1)
                    {
                        var call__773 = Units[I];
                        AddUnit((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__773);
                        var call__774 = Units[I];
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS((Aow.Units.HSEngine.THexagonSprite)call__774);
                    }
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
        public void MoveSelectionEvent(Aow.Rtl.TObject Sender)
        {
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)
            {
                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Capture != this)
                {
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.UnselectLocation();
                }
                else
                {
                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Button == Aow.Rtl.TMouseButton.mbRight)
                    {
                        var call__775 = FRangedPath.Count;
                        if (call__775 == 0)
                        {
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.UnselectLocation();
                        }
                        else
                        {
                            FRangedPath.Clear();
                            FMouseOverTarget = null;
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMouseOverTargetEvents.TriggerNotifyEvents(FMouseOverTarget);
                        }
                    }
                    else
                    {
                        if ((Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Capture == this) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Button == Aow.Rtl.TMouseButton.mbLeft))
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
            Aow.Units.AoWTC.TTacticalCombatUnitHS UnitHS = default;
            ushort Range = default;
            Aow.Units.AoWE.TSpellCombatInfo Info = default;
            int Direction = default;
            int NX = default;
            int NY = default;
            FRangedPath.Clear();
            {
                Direction = 0;
                int end__776 = FObstacles.Count - 1;
                for (; Direction <= end__776; Direction += 1)
                {
                    var call__777 = FObstacles[Direction];
                    ((Aow.Rtl.TObject)call__777).Free();
                }
            }

            FObstacles.Clear();
            UnitHS = GetTCUnitHS();
            var call__778 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
            var call__779 = call__778.Field[X, Y];
            if (call__779.TerrainType != 15)
            {
                if (UnitHS.FSelectedAbility == 52)
                {
                    var call__780 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(UnitHS.FSelectedSpell);
                    call__780.GetCombatInfo(UnitHS.FUnit.FUnit, ref Info);
                    if (Info.Range > 0)
                    {
                        Range = unchecked((ushort)Info.Range);
                    }
                    else
                    {
                        Range = 100;
                    }
                }
                else
                {
                    var call__781 = GetTCUnitHS();
                    var call__782 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                    var call__783 = call__782.GetCombatMode(call__781.FUnit.FUnit);
                    if (call__783 == Aow.Units.AoWE.TCombatMode.cmLongRange)
                    {
                        Range = 12;
                    }
                    else
                    {
                        var call__784 = GetTCUnitHS();
                        var call__785 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                        var call__786 = call__785.GetCombatMode(call__784.FUnit.FUnit);
                        if (call__786 == Aow.Units.AoWE.TCombatMode.cmMediumRange)
                        {
                            Range = 8;
                        }
                        else
                        {
                            Range = 4;
                        }
                    }
                }

                var call__787 = UnitHS.FUnit.GetMoves();
                bool bool__788 = (call__787 == UnitHS.FUnit.FMovePoints) && (X > 0);
                if (bool__788)
                {
                    var call__789 = UnitHS.GetYhx();
                    var call__790 = UnitHS.GetXhx();
                    var call__791 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, call__790, call__789);
                    bool__788 = (call__791 <= Range);
                }

                if (bool__788)
                {
                    // { Original retains this unused with temporary. }
                    {
                        var w__792 = GetTCUnitHS();
                        FTargetX = X;
                        FTargetY = Y;
                        var call__793 = UnitHS.GetYhx();
                        var call__794 = UnitHS.GetXhx();
                        var call__795 = Aow.Units.HSEngine.HSEngine.dHXtoHN(call__794, call__793, X, Y);
                        Direction = call__795;
                        NX = X;
                        NY = Y;
                        var call__796 = GetTCUnitHS();
                        switch (call__796.FSelectedAbility)
                        {
                            case 30:
                            case >= 86 and <= 90:
                                var call__797 = UnitHS.GetYhx();
                                var call__798 = UnitHS.GetXhx();
                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__798, call__797, Aow.Units.AoWTC.AoWTC.BreathHN[(Aow.Units.AoWTC.AoWTC.BreathDirection[Direction - 1] * 12) - 12], ref NX, ref NY);
                                break;
                            case 52:
                                if (Aow.Units.AoWTC.AoWTC.SpellTypes[UnitHS.FSelectedSpell] == Aow.Units.AoWTC.TfxType.fxBreath)
                                {
                                    var call__799 = UnitHS.GetYhx();
                                    var call__800 = UnitHS.GetXhx();
                                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__800, call__799, Aow.Units.AoWTC.AoWTC.BreathHN[(Aow.Units.AoWTC.AoWTC.BreathDirection[Direction - 1] * 12) - 12], ref NX, ref NY);
                                }

                                break;
                        }

                        var call__801 = GetTCUnitHS();
                        var call__802 = call__801.GetXhx();
                        var call__803 = GetTCUnitHS();
                        var call__804 = call__803.GetYhx();
                        var call__805 = GetTCUnitHS();
                        var call__806 = call__805.FUnit.GetSide();
                        var call__807 = GetTCUnitHS();
                        var call__808 = GetTCUnitHS();
                        Aow.Units.AoWTC.AoWTC.MakeAbilPath(call__806, unchecked((short)call__808.FSelectedAbility), unchecked((short)call__807.FSelectedSpell), unchecked((short)call__802), unchecked((short)call__804), unchecked((short)NX), unchecked((short)NY), ref FRangedPath, ref FObstacles);
                    }
                }
            }

            FRangedPathHS.FSelection = this;
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.DynamicFixedVisibleHS.AddChild(FRangedPathHS);
        }

        // AoWTC.pas:2918
        public void MoveSelectedRoute()
        {
            Aow.Units.AoWTC.TTacticalCombatUnitHS UnitHS = default;
            Aow.Units.Engine.TEObject Target = default;
            int MovePoints = default;
            var call__809 = FMoveSettingsList.GetCount();
            if (call__809 > 0)
            {
                if (GetTCUnitHS() != null)
                {
                    var call__810 = FMoveSettingsList.GetMoveSettings(0);
                    if (call__810.FPath.Count > 0)
                    {
                        {
                            var w__811 = FMoveSettingsList.GetMoveSettings(0);
                            UnitHS = GetTCUnitHS();
                            if (UnitHS.FSelectedAbility != 117)
                            {
                                var call__812 = w__811.FPath[0];
                                var call__813 = w__811.FPath[0];
                                var call__814 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__815 = call__814.Field[call__813.X, call__812.Y];
                                var call__816 = call__815.FindOwnedChild(2228484);
                                Target = call__816;
                            }
                            else
                            {
                                var call__817 = w__811.FPath[0];
                                var call__818 = w__811.FPath[0];
                                var call__819 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__820 = call__819.Field[call__818.X, call__817.Y];
                                var call__821 = call__820.FindOwnedChild(2229508);
                                Target = call__821;
                                if (Target == null)
                                {
                                    var call__822 = w__811.FPath[0];
                                    var call__823 = w__811.FPath[0];
                                    var call__824 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__825 = call__824.Field[call__823.X, call__822.Y];
                                    var call__826 = call__825.FindOwnedChild(2229510);
                                    Target = call__826;
                                }
                            }

                            var call__827 = FMoveSettingsList.GetMoveSettings(0);
                            var call__828 = call__827.FPath.MovePointsToPos(UnitHS.FUnit.FMovePoints);
                            if ((call__828 == 0) && (((Target != null) || (UnitHS.FSelectedAbility == 65)) || (UnitHS.FSelectedAbility == 114)))
                            {
                                var call__829 = GetTCUnitHS();
                                MovePoints = call__829.FUnit.FMovePoints;
                                if (MovePoints > 0)
                                {
                                    if (UnitHS.FSelectedAbility == -1)
                                    {
                                        var call__830 = GetTCUnitHS();
                                        var call__831 = call__830.MeleeMoveTC(w__811.FPath);
                                        if (call__831)
                                        {
                                            w__811.FPath.Clear();
                                        }
                                    }
                                    else
                                    {
                                        var call__832 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                                        var call__833 = call__832.GetCombatMode(UnitHS.FUnit.FUnit);
                                        if (call__833 == Aow.Units.AoWE.TCombatMode.cmTouch)
                                        {
                                            var call__834 = GetTCUnitHS();
                                            var call__835 = call__834.AbTouchMoveTC(w__811.FPath);
                                            if (call__835)
                                            {
                                                w__811.FPath.Clear();
                                                UpdateMoveCursor();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var call__836 = Aow.Units.CombatTE.CombatTE.MoveCombatUnit(UnitHS.FUnit, w__811.FPath);
                                if (call__836)
                                {
                                    w__811.FPath.Clear();
                                }
                            }
                        }
                    }
                }
            }
        }

        // AoWTC.pas:2943
        public void MoveUnits()
        {
            int I = default;
            var call__837 = FMoveSettingsList.GetCount();
            if (call__837 > 0)
            {
                var call__838 = FMoveSettingsList.GetCount();
                if (call__838 > 1)
                {
                    var call__839 = Aow.Units.CombatTE.CombatTE.SetupMultiTCUnitMove(FMoveSettingsList);
                    if (call__839)
                    {
                        {
                            I = 0;
                            int end__840 = FMoveSettingsList.GetCount() - 1;
                            for (; I <= end__840; I += 1)
                            {
                                {
                                    var w__841 = FMoveSettingsList.GetMoveSettings(I);
                                    w__841.FPath.Clear();
                                }
                            }
                        }
                    }

                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.UnselectLocation();
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
            Aow.Units.AoWTC.TTacticalCombatUnitHS UnitHS = default;
            Aow.Units.AoWTC.TfxType EffectType = default;
            int I = default;
            int X = default;
            int Y = default;
            int Direction = default;
            int A = default;
            int B = default;
            bool AllPathsValid()
            {
                bool Result = default;
                int I = default;
                var call__842 = FMoveSettingsList.GetMoveSettings(0);
                bool bool__843 = (call__842.FPath.Count > 0);
                if (bool__843)
                {
                    var call__844 = FMoveSettingsList.GetMoveSettings(0);
                    var call__845 = call__844.FPath[0];
                    var call__846 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                    bool__843 = (call__845.X == call__846);
                }

                bool bool__847 = bool__843;
                if (bool__847)
                {
                    var call__848 = FMoveSettingsList.GetMoveSettings(0);
                    var call__849 = call__848.FPath[0];
                    var call__850 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                    bool__847 = (call__849.Y == call__850);
                }

                Result = bool__847;
                var call__851 = FMoveSettingsList.GetCount();
                I = call__851;
                while (Result && (I != 0))
                {
                    I -= 1;
                    {
                        var w__852 = FMoveSettingsList.GetMoveSettings(I);
                        Result = w__852.FPath.Count != 0;
                    }
                }

                return Result;
            }

            DeactivateRangedPath();
            var call__853 = FMoveSettingsList.GetCount();
            if (call__853 > 1)
            {
                if (FCurrentMove == -1)
                {
                    if (!AllPathsValid())
                    {
                        bool bool__854 = !Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
                        if (!bool__854)
                        {
                            var call__855 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                            var call__856 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                            var call__857 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__856, call__855, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                            var call__858 = ((Aow.Units.AoWE.TAoWMapField)call__857).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                            bool__854 = call__858;
                        }

                        if (bool__854)
                        {
                            {
                                I = 0;
                                int end__859 = FMoveSettingsList.GetCount() - 1;
                                for (; I <= end__859; I += 1)
                                {
                                    var call__860 = FMoveSettingsList.GetMoveSettings(I);
                                    call__860.FPath.Clear();
                                }
                            }

                            {
                                A = 0;
                                int end__861 = FMoveSettingsList.GetCount() - 1;
                                for (; A <= end__861; A += 1)
                                {
                                    {
                                        B = A + 1;
                                        int end__862 = FMoveSettingsList.GetCount() - 1;
                                        for (; B <= end__862; B += 1)
                                        {
                                            var call__863 = FMoveSettingsList.GetMoveSettings(B);
                                            var call__864 = ((Aow.Units.HSEngine.THexagonSprite)call__863.FMoveObject).GetYhx();
                                            var call__865 = FMoveSettingsList.GetMoveSettings(B);
                                            var call__866 = ((Aow.Units.HSEngine.THexagonSprite)call__865.FMoveObject).GetXhx();
                                            var call__867 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                            var call__868 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                            var call__869 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__868, call__867, call__866, call__864);
                                            var call__870 = FMoveSettingsList.GetMoveSettings(A);
                                            var call__871 = ((Aow.Units.HSEngine.THexagonSprite)call__870.FMoveObject).GetYhx();
                                            var call__872 = FMoveSettingsList.GetMoveSettings(A);
                                            var call__873 = ((Aow.Units.HSEngine.THexagonSprite)call__872.FMoveObject).GetXhx();
                                            var call__874 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                            var call__875 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                            var call__876 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__875, call__874, call__873, call__871);
                                            if (call__869 < call__876)
                                            {
                                                FMoveSettingsList.FChildren.Exchange(A, B);
                                            }
                                        }
                                    }
                                }
                            }

                            var call__877 = FMoveSettingsList.GetMoveSettings(0);
                            var call__878 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__877.FMoveObject).FUnit.GetLocked();
                            if (call__878)
                            {
                                var call__879 = FMoveSettingsList.GetMoveSettings(0);
                                call__879.FPath.Clear();
                            }
                            else
                            {
                                var call__880 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                var call__881 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                CalculateMovePath(0, unchecked((sbyte)call__881), unchecked((sbyte)call__880), unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL));
                            }

                            var call__882 = FMoveSettingsList.GetMoveSettings(0);
                            if (call__882.FPath.Count > 1)
                            {
                                {
                                    I = 1;
                                    int end__883 = FMoveSettingsList.GetCount() - 1;
                                    for (; I <= end__883; I += 1)
                                    {
                                        var call__884 = FMoveSettingsList.GetMoveSettings(I);
                                        var call__885 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__884.FMoveObject).FUnit.GetLocked();
                                        if (call__885)
                                        {
                                            var call__886 = FMoveSettingsList.GetMoveSettings(I);
                                            call__886.FPath.Clear();
                                        }
                                        else
                                        {
                                            var call__887 = FMoveSettingsList.GetMoveSettings(I);
                                            var call__888 = ((Aow.Units.HSEngine.THexagonSprite)call__887.FMoveObject).GetYhx();
                                            var call__889 = FMoveSettingsList.GetMoveSettings(I);
                                            var call__890 = ((Aow.Units.HSEngine.THexagonSprite)call__889.FMoveObject).GetXhx();
                                            var call__891 = FMoveSettingsList.GetMoveSettings(0);
                                            var call__892 = ((Aow.Units.HSEngine.THexagonSprite)call__891.FMoveObject).GetYhx();
                                            var call__893 = FMoveSettingsList.GetMoveSettings(0);
                                            var call__894 = ((Aow.Units.HSEngine.THexagonSprite)call__893.FMoveObject).GetXhx();
                                            var call__895 = Aow.Units.HSEngine.HSEngine.dHXtoHN(call__894, call__892, call__890, call__888);
                                            Direction = call__895;
                                            var call__896 = FMoveSettingsList.GetMoveSettings(0);
                                            var call__897 = call__896.FPath[0];
                                            var call__898 = FMoveSettingsList.GetMoveSettings(0);
                                            var call__899 = call__898.FPath[0];
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__899.X, call__897.Y, Direction, ref X, ref Y);
                                            CalculateMovePath(I, unchecked((sbyte)X), unchecked((sbyte)Y), 0);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            {
                                I = 0;
                                int end__900 = FMoveSettingsList.GetCount() - 1;
                                for (; I <= end__900; I += 1)
                                {
                                    var call__901 = FMoveSettingsList.GetMoveSettings(I);
                                    call__901.FPath.Clear();
                                }
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
                    {
                        var w__902 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                        bool bool__903 = (w__902.FPath.Count != 0);
                        if (bool__903)
                        {
                            var call__904 = w__902.FPath[0];
                            var call__905 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                            bool__903 = (call__904.X == call__905);
                        }

                        bool bool__906 = bool__903;
                        if (bool__906)
                        {
                            var call__907 = w__902.FPath[0];
                            var call__908 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                            bool__906 = (call__907.Y == call__908);
                        }

                        bool bool__909 = bool__906;
                        if (bool__909)
                        {
                            var call__910 = w__902.FPath[0];
                            bool__909 = (call__910.L == Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                        }

                        if (bool__909)
                        {
                            MoveUnits();
                            return;
                        }
                    }

                    var call__911 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                    var call__912 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__911.FMoveObject).FUnit.GetLocked();
                    if (call__912)
                    {
                        var call__913 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                        call__913.FPath.Clear();
                    }
                    else
                    {
                        var call__914 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                        var call__915 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                        CalculateMovePath(FCurrentMove, unchecked((sbyte)call__915), unchecked((sbyte)call__914), unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL));
                    }
                }
            }
            else
            {
                var call__916 = GetTCUnitHS();
                bool bool__917 = (call__916.FSelectedAbility == -1);
                if (bool__917)
                {
                    var call__918 = GetTCUnitHS();
                    var call__919 = call__918.GetXhx();
                    var call__920 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                    bool__917 = (call__919 == call__920);
                }

                bool bool__921 = bool__917;
                if (bool__921)
                {
                    var call__922 = GetTCUnitHS();
                    var call__923 = call__922.GetYhx();
                    var call__924 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                    bool__921 = (call__923 == call__924);
                }

                if (bool__921)
                {
                    var call__925 = GetTCUnitHS();
                    var call__926 = call__925.GetLhx();
                    var call__927 = GetTCUnitHS();
                    var call__928 = call__927.GetYhx();
                    var call__929 = GetTCUnitHS();
                    var call__930 = call__929.GetXhx();
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHX(call__930, call__928, call__926);
                    return;
                }

                var call__931 = GetTCUnitHS();
                var call__932 = call__931.FUnit.GetLocked();
                bool bool__933 = !call__932;
                if (bool__933)
                {
                    var call__934 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentPlayer);
                    bool__933 = call__934;
                }

                if (bool__933)
                {
                    UnitHS = GetTCUnitHS();
                    if (AllPathsValid())
                    {
                        MoveUnits();
                    }
                    else
                    {
                        var call__935 = FRangedPath.Count;
                        bool bool__936 = (call__935 > 0);
                        if (bool__936)
                        {
                            var call__937 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                            bool__936 = (FTargetX == call__937);
                        }

                        bool bool__938 = bool__936;
                        if (bool__938)
                        {
                            var call__939 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                            bool__938 = (FTargetY == call__939);
                        }

                        if (bool__938)
                        {
                            var call__940 = UnitHS.AbRangedTC(unchecked((ushort)FTargetX), unchecked((ushort)FTargetY));
                            if (call__940)
                            {
                                FRangedPath.Clear();
                                UpdateMoveCursor();
                            }
                        }
                        else
                        {
                            bool bool__941 = (UnitHS.FSelectedAbility == -1);
                            if (bool__941)
                            {
                                var call__942 = UnitHS.GetXhx();
                                var call__943 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                bool__941 = (call__942 == call__943);
                            }

                            bool bool__944 = bool__941;
                            if (bool__944)
                            {
                                var call__945 = UnitHS.GetYhx();
                                var call__946 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                bool__944 = (call__945 == call__946);
                            }

                            if (bool__944)
                            {
                                var call__947 = UnitHS.GetLhx();
                                var call__948 = UnitHS.GetYhx();
                                var call__949 = UnitHS.GetXhx();
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHX(call__949, call__948, call__947);
                                return;
                            }

                            if (!UnitHS.Moving())
                            {
                                if (UnitHS.FSelectedAbility == -1)
                                {
                                    bool bool__950 = (UnitHS.FUnit.FMovePoints > 0);
                                    if (bool__950)
                                    {
                                        var call__951 = UnitHS.FUnit.GetLocked();
                                        bool__950 = !call__951;
                                    }

                                    if (bool__950)
                                    {
                                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled)
                                        {
                                            var call__952 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                            var call__953 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                            var call__954 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__953, call__952, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                            var call__955 = ((Aow.Units.AoWE.TAoWMapField)call__954).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                                            if (!call__955)
                                            {
                                                return;
                                            }
                                        }

                                        var call__956 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                        var call__957 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                        CalculateMovePath(FCurrentMove, unchecked((sbyte)call__957), unchecked((sbyte)call__956), 0);
                                    }
                                }
                                else
                                {
                                    if (UnitHS.FSelectedAbility == 52)
                                    {
                                        EffectType = Aow.Units.AoWTC.AoWTC.SpellTypes[UnitHS.FSelectedSpell];
                                    }
                                    else
                                    {
                                        EffectType = Aow.Units.AoWTC.AoWTC.AbilTypes[UnitHS.FSelectedAbility];
                                    }

                                    switch (EffectType)
                                    {
                                        case Aow.Units.AoWTC.TfxType.fxNone:
                                            UnitHS.SelectAbility(-1);
                                            break;
                                        case Aow.Units.AoWTC.TfxType.fxTouch:
                                            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled)
                                            {
                                                var call__958 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                                var call__959 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                                var call__960 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__959, call__958, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                                var call__961 = ((Aow.Units.AoWE.TAoWMapField)call__960).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                                                if (!call__961)
                                                {
                                                    return;
                                                }
                                            }

                                            var call__962 = UnitHS.FUnit.GetLocked();
                                            if (!call__962)
                                            {
                                                var call__963 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                                var call__964 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                                CalculateMovePath(0, unchecked((sbyte)call__964), unchecked((sbyte)call__963), 0);
                                            }

                                            break;
                                        default:
                                        {
                                            var call__965 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                            var call__966 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                            CalculateRangedPath(unchecked((sbyte)call__966), unchecked((sbyte)call__965));
                                            var call__967 = FRangedPath.Count;
                                            if (call__967 > 0)
                                            {
                                                var call__968 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                                var call__969 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                                var call__970 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                var call__971 = call__970.Field[call__969, call__968];
                                                var call__972 = call__971.FindOwnedChild(2228484);
                                                FMouseOverTarget = (Aow.Units.HSEngine.THexagonSprite)call__972;
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMouseOverTargetEvents.TriggerNotifyEvents(FMouseOverTarget);
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
        public bool ValidMultiMoveSelection(Aow.Units.AoWE.TAoWMapField Field)
        {
            bool Result = default;
            sbyte Player = default;
            Player = unchecked((sbyte)Field.GetPlayer());
            Result = Player == -1;
            return Result;
        }

        // AoWTC.pas:3259
        public void ActivateMoveSelection()
        {
            if (!FStatus.Contains(Aow.Units.AoWE.TMoveSelectorStatusElement.mssActivated))
            {
                Activate(Aow.Units.AoWTC.AoWTC.AoWCombatMap);
            }
        }

        // AoWTC.pas:3264
        public void DeactivateMoveSelection()
        {
            if (FStatus.Contains(Aow.Units.AoWE.TMoveSelectorStatusElement.mssActivated))
            {
                Deactivate();
                FRangedPath.Clear();
                FMoveSettingsList.Clear();
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.MouseMoveEvent(this);
            }
        }

        // AoWTC.pas:3293
        public void Update()
        {
            if (GetTCUnitHS() != null)
            {
                var call__973 = GetTCUnitHS();
                call__973.FUnit.FUnit.CreateMovePointTable(ref FMoveControlSettings.FMovePointTable);
            }

            FPathInvalid = TSet<Aow.Units.AoWTC.AnonymousEnum_piMove>.Of(Aow.Units.AoWTC.AnonymousEnum_piMove.piRanged);
        }

        // AoWTC.pas:3300
        public void SetTCUnitHS(Aow.Units.AoWTC.TTacticalCombatUnitHS U)
        {
            if (GetTCUnitHS() != U)
            {
                var call__974 = FMoveSettingsList.GetCount();
                if (call__974 <= 1)
                {
                    Clear();
                    bool bool__975 = (U != null);
                    if (bool__975)
                    {
                        var call__976 = U.FUnit.GetPlayer();
                        bool__975 = (call__976 == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                    }

                    if (bool__975)
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
        public void Select(Aow.Units.AoWTC.TTacticalCombatUnitHS U)
        {
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.UnselectLocation();
            if (U != null)
            {
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS(U);
            }
        }

        // AoWTC.pas:3331
        public void DeactivateRangedPath()
        {
            if (FRangedPathHS.FSelection != null)
            {
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.DynamicFixedVisibleHS.RemoveChild(FRangedPathHS);
                FRangedPathHS.FSelection = null;
            }
        }

        // AoWTC.pas:398
        public static new Aow.Units.AoWTC.TCombatUnitSelectionControl Create()
        {
            Aow.Units.AoWTC.TCombatUnitSelectionControl o = new Aow.Units.AoWTC.TCombatUnitSelectionControl();
            try
            {
                o.Create__init_TCombatUnitSelectionControl();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TTacticalCombatPlayer (instance size 28, VMT 00412F78)
//     $0C FAuto; $0D FEndTurn; $0E FIndex; $10 FLastX; $14 FLastY; $18 FLastUnit. }
    public partial class TTacticalCombatPlayer : Aow.Units.AoWE.TCombatPlayer
    {
        public bool FAuto;
        // { $0C }
        public bool FEndTurn;
        // { $0D }
        public sbyte FIndex;
        // { $0E }
        public byte FReserved0F;
        public int FLastX;
        public int FLastY;
        public Aow.Units.AoWTC.TTacticalCombatUnit FLastUnit;
        // AoWTC.pas:6419
        // { $10, $14, $18 }
        public void SetAuto(bool Value)
        {
            FAuto = Value;
            if ((Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer == FIndex) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer))
            {
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FAI.Reset();
            }
        }

        // Engine.pas:524
        public static new Aow.Units.AoWTC.TTacticalCombatPlayer Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWTC.TTacticalCombatPlayer o = new Aow.Units.AoWTC.TTacticalCombatPlayer();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TTacticalCombatPlayerList (instance size 8, VMT 0041304C)
//     $04 FPlayers. }
    public partial class TTacticalCombatPlayerList : Aow.Rtl.TObject
    {
        public Aow.Units.AoWE.TCombatPlayerList FPlayers;
        // AoWTC.pas:6426
        // { $04 }
        public int GetCount()
        {
            int Result = default;
            if (FPlayers != null)
            {
                var call__977 = FPlayers.GetCount();
                Result = call__977;
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // AoWTC.pas:6431
        public Aow.Units.AoWTC.TTacticalCombatPlayer GetPlayers(sbyte Index)
        {
            Aow.Units.AoWTC.TTacticalCombatPlayer Result = default;
            var call__978 = FPlayers.GetPlayers(unchecked((sbyte)Index));
            Result = (Aow.Units.AoWTC.TTacticalCombatPlayer)call__978;
            return Result;
        }

        // System.pas:111
        public static Aow.Units.AoWTC.TTacticalCombatPlayerList Create()
        {
            return new Aow.Units.AoWTC.TTacticalCombatPlayerList();
        }
    }

    // { conflict: function GetPlayers(Player: Shortint): TTacticalCombatPlayer; }
    // { TAoWCombatMapMouse (instance size 80, VMT 004130DC) }
    public partial class TAoWCombatMapMouse : Aow.Units.AoWE.TAbstractAoWHSMapMouse
    {
        // intf_P1_05.inc:68
        public static new Aow.Units.AoWTC.TAoWCombatMapMouse Create()
        {
            Aow.Units.AoWTC.TAoWCombatMapMouse o = new Aow.Units.AoWTC.TAoWCombatMapMouse();
            try
            {
                o.Create__init_TAbstractAoWHSMapMouse();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TAoWCombatMap (instance size 312, VMT 00413184)
//     $E0 FPlayers; $E4 FCombat; $E8 FUnitPositions; $EC FSeatedCombatPlayer;
//     $ED FCurrentCombatPlayer; $F0..$F8 turn/player events; $FC FMoveControl;
//     $100 FUnitSelection; $104..$114 selection/unit events; $118 FAI; $11C FEffects;
//     $120 FWalls; $124 FFleeLocations; $128 FEndTurnPending; $129 FPlaySounds;
//     $12A FPositionStatus; $134 FRound. }
    public partial class TAoWCombatMap : Aow.Units.AoWE.TAbstractAoWCombatMap
    {
        public Aow.Units.AoWTC.TTacticalCombatPlayerList FPlayers;
        // { $E0 }
        public Aow.Units.AoWTC.TTacticalCombat FCombat;
        // { $E4 }
        public Aow.Units.AoWTC.TUnitPositionControl FUnitPositions;
        // { $E8 }
        public sbyte FSeatedCombatPlayer;
        // { $EC }
        // { conflict: FSeatedCombatPlayer: Byte; }
        public sbyte FCurrentCombatPlayer;
        // { $ED }
        // { conflict: FCurrentCombatPlayer: Shortint; }
        public Aow.Units.FixedArray_byte_2 F_AoWCombatMap_EE;
        public Aow.Units.Engine.TEventList FChangeTurnEvents;
        // { $F0 }
        // { conflict: FTurnChangedEvents: TEventList; }
        public Aow.Units.Engine.TEventList FPlayerChangedEvents;
        // { $F4 }
        public Aow.Units.Engine.TEventList FSeatedPlayerChangedEvents;
        // { $F8 }
        public Aow.Units.AoWTC.TCombatMoveControl FMoveControl;
        // { $FC }
        public Aow.Units.AoWTC.TCombatUnitSelectionControl FUnitSelection;
        // { $100 }
        public Aow.Units.Engine.TEventList FSelectEvents;
        // { $104 }
        // { conflict: FDeactivateMapEvents: TEventList; }
        public Aow.Units.Engine.TEventList FDeselectEvents;
        // { $108 }
        // { conflict: FActivateMapEvents: TEventList; }
        public Aow.Units.HSEngine.TMapObject FCenterViewLocation;
        // { $10C; $435a3e }
        public Aow.Units.Engine.TEventList FMouseOverTargetEvents;
        // { $110; CMapCtrl 004364E4 }
        // { conflict: FUnitChangedEvents: TEventList; }
        public Aow.Units.Engine.TEventList FUnitChangedEvents;
        // { $114; CMapCtrl 004364FF }
        // { conflict: FMouseOverTargetEvents: TEventList; }
        public Aow.Units.AoWTC.TCAI FAI;
        // { $118 }
        public Aow.Rtl.TList FEffects;
        // { $11C }
        public Aow.Rtl.TList FWalls;
        // { $120; $405741 }
        public Aow.Rtl.TList FFleeLocations;
        // { $124 }
        public bool FEndTurnPending;
        // { $128 }
        public bool FPlaySounds;
        // { $129; $405B38 }
        public Aow.Units.FixedArray_byte_8 FPositionStatus;
        // { $12A }
        public Aow.Units.FixedArray_byte_2 F_AoWCombatMap_132;
        public int FRound;
        // AoWTC.pas:2213
        // { $134 }
        public override void Destroy()
        {
            while (FEffects.Count > 0)
            {
                var call__979 = FEffects[0];
                DynamicFixedVisibleHS.RemoveChild((Aow.Units.HSEngine.THexagonSprite)call__979);
                var call__980 = FEffects[0];
                DynamicVisibleHS.RemoveChild((Aow.Units.HSEngine.THexagonSprite)call__980);
                var call__981 = FEffects[0];
                ((Aow.Rtl.TObject)call__981).Free();
                FEffects.Delete(0);
            }

            base.Destroy();
            Aow.Units.AoWTC.AoWTC.AoWCombatMap = null;
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

        // AoWTC.pas:2186
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            int I = default;
            Aow.Units.AoWTC.AoWTC.AoWCombatMap = this;
            base.Create__init(AOwner);
            var call__982 = Aow.Units.AoWTC.TUnitPositionControl.Create(null);
            FUnitPositions = call__982;
            var call__983 = Aow.Units.Engine.TEventList.Create();
            FChangeTurnEvents = call__983;
            var call__984 = Aow.Units.Engine.TEventList.Create();
            FPlayerChangedEvents = call__984;
            var call__985 = Aow.Units.Engine.TEventList.Create();
            FSeatedPlayerChangedEvents = call__985;
            var call__986 = Aow.Units.AoWTC.TCombatMoveControl.Create(this);
            FMoveControl = call__986;
            var call__987 = Aow.Units.Engine.TEventList.Create();
            FSelectEvents = call__987;
            var call__988 = Aow.Units.Engine.TEventList.Create();
            FDeselectEvents = call__988;
            var call__989 = Aow.Units.Engine.TEventList.Create();
            FMouseOverTargetEvents = call__989;
            var call__990 = Aow.Units.Engine.TEventList.Create();
            FUnitChangedEvents = call__990;
            var call__991 = Aow.Units.AoWTC.TTacticalCombatPlayerList.Create();
            FPlayers = call__991;
            var call__992 = Aow.Units.AoWTC.TCombatUnitSelectionControl.Create();
            FUnitSelection = call__992;
            ((Aow.Units.AoWTC.TAoWCombatMapMouse)Mouse).FRectangleSelectEvents.Add(FUnitSelection.RectangleSelectEvent);
            var call__993 = new Aow.Rtl.TList();
            FEffects = call__993;
            var call__994 = new Aow.Rtl.TList();
            FWalls = call__994;
            var call__995 = new Aow.Rtl.TList();
            FFleeLocations = call__995;
            var call__996 = Aow.Units.AoWTC.TCAI.Create();
            FAI = call__996;
            Mouse.SetCapture(this);
            Mouse.OnMove.Add(MouseMoveEvent);
            Mouse.CursorMode = 256;
            {
                I = 0;
                byte end__997 = 7;
                for (; I <= end__997; I += 1)
                {
                    FPositionStatus[I] = 2;
                }
            }
        }

        // AoWTC.pas:2249
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            if ((Msg.MsgID == 536870920) && !Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                SetCombat((Aow.Units.AoWTC.TTacticalCombat)Aow.Units.AoWE.AoWE.AoWHSMap.FCombat);
            }

            base.MsgProc(ref Msg, Sender);
        }

        // AoWTC.pas:2240
        public override Aow.Units.HSEngine.THSMapMouse CreateMouse()
        {
            Aow.Units.HSEngine.THSMapMouse Result = default;
            Result = Aow.Units.AoWTC.TAoWCombatMapMouse.Create();
            return Result;
        }

        // AoWTC.pas:2267
        public override void InitializeNewMap()
        {
            Aow.Units.HSEngine.THexagonControl Control = default;
            int X = default;
            int Y = default;
            {
                X = 6;
                int end__998 = MapContainer.Width - 7;
                for (; X <= end__998; X += 1)
                {
                    {
                        Y = 6;
                        int end__999 = MapContainer.Height - 7;
                        for (; Y <= end__999; Y += 1)
                        {
                            Aow.Units.AoWE.AoWE.AoWHSSet.FCombatHexagons.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 1);
                        }
                    }
                }
            }

            Control = Engine.HSSet.HexagonControl;
            {
                var w__1000 = Control;
                {
                    X = 0;
                    int end__1001 = MapContainer.Width - 1;
                    for (; X <= end__1001; X += 1)
                    {
                        {
                            Y = 0;
                            byte end__1002 = 5;
                            for (; Y <= end__1002; Y += 1)
                            {
                                w__1000.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 15);
                            }
                        }

                        {
                            Y = MapContainer.Height - 6;
                            int end__1003 = MapContainer.Height - 1;
                            for (; Y <= end__1003; Y += 1)
                            {
                                w__1000.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 15);
                            }
                        }
                    }
                }

                {
                    Y = 6;
                    int end__1004 = MapContainer.Height - 7;
                    for (; Y <= end__1004; Y += 1)
                    {
                        {
                            X = 0;
                            byte end__1005 = 5;
                            for (; X <= end__1005; X += 1)
                            {
                                w__1000.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 15);
                            }
                        }

                        {
                            X = MapContainer.Width - 6;
                            int end__1006 = MapContainer.Width - 1;
                            for (; X <= end__1006; X += 1)
                            {
                                w__1000.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 15);
                            }
                        }
                    }
                }
            }
        }

        // AoWTC.pas:2338
        public override void DrawDynamicScene(ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            base.DrawDynamicScene(ref SD);
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                DrawFog(SD);
            }

            if (FShowGrid)
            {
                DrawGrid(SD);
            }

            DrawMoveRoute(SD);
            if (((Aow.Units.AoWTC.TAoWCombatMapMouse)Mouse).FSelectionRectangle.Left != -1)
            {
                DrawSelectionRect(ViewXhp, ViewYhp, ((Aow.Units.AoWTC.TAoWCombatMapMouse)Mouse).FSelectionRectangle, SD);
            }
        }

        // AoWTC.pas:2429
        public override void NewFrame()
        {
            int I = default;
            Aow.Units.AoWTC.TfxHS Effect = default;
            if (!Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                I = 0;
                while (I < FEffects.Count)
                {
                    var call__1007 = FEffects[I];
                    Effect = (Aow.Units.AoWTC.TfxHS)call__1007;
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
            if (!Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting) && (FCombat != null))
            {
                UpdateCursor();
                bool bool__1008 = FEndTurnPending;
                if (bool__1008)
                {
                    var call__1009 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FCurrentPlayer);
                    bool bool__1010 = (call__1009.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman);
                    if (bool__1010)
                    {
                        var call__1011 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FCombat.FHostPlayer);
                        bool__1010 = (call__1011.ConnectionMode != Aow.Units.NetworkE.TConnectionMode.cmRemote);
                    }

                    bool bool__1012 = bool__1010;
                    if (!bool__1012)
                    {
                        var call__1013 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                        bool bool__1014 = call__1013.FAuto;
                        if (bool__1014)
                        {
                            var call__1015 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FCurrentPlayer);
                            bool__1014 = (call__1015.ConnectionMode != Aow.Units.NetworkE.TConnectionMode.cmRemote);
                        }

                        bool__1012 = bool__1014;
                    }

                    bool__1008 = bool__1012;
                }

                if (bool__1008)
                {
                    switch (FAI.Status)
                    {
                        case Aow.Units.AoWTC.TaiStatus.aiIdle:
                            Aow.Units.AoWE.AoWE.AoWEngine.OutputDebugStr((((AnsiString)"AI for Player ") + Aow.Rtl.Rtl.IntToStr(FCurrentPlayer)) + ((AnsiString)" turn begins."));
                            FAI.BeginTurn();
                            break;
                        case Aow.Units.AoWTC.TaiStatus.aiBusy:
                            FAI.ContinueTurn();
                            break;
                        case Aow.Units.AoWTC.TaiStatus.aiDone:
                            if (EndTurn(true))
                            {
                                FAI.Status = Aow.Units.AoWTC.TaiStatus.aiIdle;
                                Aow.Units.AoWE.AoWE.AoWEngine.OutputDebugStr((((AnsiString)"AI for Player ") + Aow.Rtl.Rtl.IntToStr(FCurrentPlayer)) + ((AnsiString)" turn ends."));
                            }

                            break;
                    }
                }
            }
        }

        // AoWTC.pas:2515
        public override void DropHeroItem(Aow.Units.AoWE.THero Hero, Aow.Units.AoWE.TItem Item)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Aow.Units.AoWE.TItemHS HS = default;
            int I = default;
            X = unchecked((sbyte)-1);
            Y = unchecked((sbyte)-1);
            L = unchecked((sbyte)-1);
            var call__1016 = FCombat.FCombatData.GetCount();
            I = call__1016;
            while ((I != 0) && (X == -1))
            {
                I -= 1;
                var call__1017 = FCombat.FCombatData.GetObjects(I);
                bool bool__1018 = (call__1017 is Aow.Units.AoWTC.TTacticalCombatUnit);
                if (bool__1018)
                {
                    var call__1019 = FCombat.FCombatData.GetObjects(I);
                    bool__1018 = (((Aow.Units.AoWTC.TTacticalCombatUnit)call__1019).FUnit == Hero);
                }

                if (bool__1018)
                {
                    var call__1020 = FCombat.FCombatData.GetObjects(I);
                    var call__1021 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__1020).FUnitHS.GetXhx();
                    X = unchecked((sbyte)call__1021);
                    var call__1022 = FCombat.FCombatData.GetObjects(I);
                    var call__1023 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__1022).FUnitHS.GetYhx();
                    Y = unchecked((sbyte)call__1023);
                    L = 0;
                }
            }

            if (X > -1)
            {
                var call__1024 = MapContainer.GetField(X, Y, L);
                var call__1025 = call__1024.FindOwnedChild(131697);
                HS = (Aow.Units.AoWE.TItemHS)call__1025;
                if (HS == null)
                {
                    HS = Aow.Units.AoWTC.TCItemHS.Create(null);
                    // { Original bug: a failed placement leaves the new item sprite allocated. }
                    var call__1026 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(HS, X, Y, L);
                    if (call__1026)
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
        public override Aow.Units.AoWE.TItemList GetHeroItemsOnGround(Aow.Units.AoWE.THero Hero)
        {
            Aow.Units.AoWE.TItemList Result = default;
            Aow.Units.AoWE.TItemHS HS = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Result = null;
            GetUnitLocation(Hero, ref X, ref Y, ref L);
            if (X > -1)
            {
                var call__1027 = MapContainer.GetField(X, Y, L);
                var call__1028 = call__1027.FindOwnedChild(131697);
                HS = (Aow.Units.AoWE.TItemHS)call__1028;
                if (HS != null)
                {
                    Result = HS.FItems;
                }
            }

            return Result;
        }

        // AoWTC.pas:2243
        public void SetCombat(Aow.Units.AoWTC.TTacticalCombat Combat)
        {
            FCombat = Combat;
            if (FCombat != null)
            {
                FPlayers.FPlayers = FCombat.FCombatData.FPlayers;
            }
        }

        // AoWTC.pas:2256
        public void MouseMoveEvent(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWTC.TTacticalCombatUnitHS HS = default;
            bool bool__1029 = !Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting);
            if (bool__1029)
            {
                var call__1030 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                bool__1029 = !call__1030.FAuto;
            }

            bool bool__1031 = bool__1029;
            if (bool__1031)
            {
                var call__1032 = Mouse.Xhx;
                bool__1031 = (call__1032 != -1);
            }

            bool bool__1033 = bool__1031;
            if (bool__1033)
            {
                var call__1034 = Mouse.Yhx;
                bool__1033 = (call__1034 != -1);
            }

            if (bool__1033 && (Mouse.Capture == null))
            {
                Mouse.CursorMode = 0;
                var call__1035 = Mouse.Yhx;
                var call__1036 = Mouse.Xhx;
                var call__1037 = MapContainer.MapLevels[0];
                var call__1038 = call__1037.Field[call__1036, call__1035];
                var call__1039 = call__1038.FindOwnedChild(2228484);
                HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1039;
                if (HS != null)
                {
                    Mouse.CursorMode = 257;
                }
            }
        }

        // AoWTC.pas:2288
        public void SetSeatedCombatPlayer(sbyte Player)
        {
            FSeatedCombatPlayer = unchecked((sbyte)Player);
            if (Player == -1)
            {
                SetSeatedPlayer(unchecked((sbyte)-1));
            }
            else
            {
                var call__1040 = FCombat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                SetSeatedPlayer(unchecked((sbyte)call__1040.FPlayer));
            }

            FSeatedPlayerChangedEvents.TriggerNotifyEvents(Aow.Rtl.Rtl.IntToObject((int)FCurrentCombatPlayer));
        }

        // AoWTC.pas:2296
        public void NewTurn(sbyte Player)
        {
            Aow.Units.AoWTC.TTacticalCombatUnit U = default;
            int I = default;
            Aow.Units.Engine.TEMsg Msg = default;
            FCurrentCombatPlayer = unchecked((sbyte)Player);
            if (Player == 0)
            {
                FRound += 1;
            }

            var call__1041 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
            SetCurrentPlayer(unchecked((sbyte)call__1041.FPlayer));
            FCombat.NewTurn(unchecked((byte)unchecked((byte)FCurrentCombatPlayer)));
            FUnitSelection.Unselect();
            Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.TokenExecuter.ActivateTC(FCurrentPlayer);
            var call__1042 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
            if (call__1042.FPlayer == 0)
            {
                var call__1043 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWTC.AoWTC.IndRStr);
                var call__1044 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWTC.AoWTC.BeginRoundRStr);
                ShowMessage((call__1043 + AnsiString.FromBytes(32)) + call__1044);
            }
            else
            {
                var call__1045 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                var call__1046 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1045.FPlayer);
                var call__1047 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__1046.FPlayerRace]);
                var call__1048 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWTC.AoWTC.BeginRoundRStr);
                ShowMessage((call__1047 + AnsiString.FromBytes(32)) + call__1048);
            }

            var call__1049 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
            var call__1050 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1049.FPlayer);
            if (call__1050.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman)
            {
                var call__1051 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                var call__1052 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(call__1051.FPlayer);
                if (call__1052.ConnectionMode != Aow.Units.NetworkE.TConnectionMode.cmRemote)
                {
                    SetSeatedCombatPlayer(unchecked((sbyte)FCurrentCombatPlayer));
                    var call__1053 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                    if (call__1053.FLastX == 0)
                    {
                        U = GetNextUnit(null, true, true);
                        if (U != null)
                        {
                            var call__1054 = U.FUnitHS.GetXhx();
                            var call__1055 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                            call__1055.FLastX = call__1054;
                            var call__1056 = U.FUnitHS.GetYhx();
                            var call__1057 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                            call__1057.FLastY = call__1056;
                            if (FExplorationEnabled)
                            {
                                {
                                    I = 0;
                                    int end__1058 = FPlayers.GetCount() - 1;
                                    for (; I <= end__1058; I += 1)
                                    {
                                        if (FCurrentCombatPlayer != I)
                                        {
                                            var call__1059 = U.FUnitHS.GetXhx();
                                            var call__1060 = FPlayers.GetPlayers(unchecked((sbyte)I));
                                            call__1060.FLastX = call__1059;
                                            var call__1061 = U.FUnitHS.GetYhx();
                                            var call__1062 = FPlayers.GetPlayers(unchecked((sbyte)I));
                                            call__1062.FLastY = call__1061;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var call__1063 = FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                    var call__1064 = FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                    CenterViewTo(call__1064.FLastX, call__1063.FLastY, 0);
                }
            }

            Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Events\\NewEvent.wav");
            Aow.Units.Engine.Engine.InitMsg(ref Msg, 537006081, this);
            SendMsg(ref Msg, this);
            if (FExplorationEnabled)
            {
                var call__1065 = Aow.Rtl.Rtl.Rect(0, 0, 65, 58);
                TriggerMapChangedEvent(call__1065, 0);
            }

            FUnitSelection.Unselect();
            FChangeTurnEvents.TriggerNotifyEvents(Aow.Rtl.Rtl.IntToObject((int)unchecked((int)FCurrentCombatPlayer)));
        }

        // AoWTC.pas:2348
        public void DrawMoveRoute(in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            FUnitSelection.ShowMovePaths(SD);
        }

        // AoWTC.pas:2351
        public bool EndTurn(bool Automatic)
        {
            bool Result = default;
            Aow.Units.CombatTE.TCEndTurnTE TE = default;
            Result = false;
            if (!Automatic)
            {
                var call__1066 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FCurrentPlayer);
                if (call__1066.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman)
                {
                    return Result;
                }

                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer != Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)
                {
                    return Result;
                }
            }

            var call__1067 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(FCurrentPlayer);
            if (call__1067)
            {
                var call__1068 = Aow.Units.CombatTE.CombatTE.CreateTCEndTurnTE(unchecked((sbyte)FCurrentPlayer));
                TE = call__1068;
                if (TE != null)
                {
                    var call__1069 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(FCurrentPlayer);
                    call__1069.AddEvent(TE);
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
            bool Result = default;
            int I = default;
            Result = true;
            {
                I = 0;
                int end__1070 = FPlayers.GetCount() - 1;
                for (; I <= end__1070; I += 1)
                {
                    bool bool__1071 = Result;
                    if (bool__1071)
                    {
                        var call__1072 = FPlayers.GetPlayers(unchecked((sbyte)I));
                        bool__1071 = call__1072.FEndTurn;
                    }

                    Result = bool__1071;
                }
            }

            return Result;
        }

        // AoWTC.pas:2381
        public Aow.Units.AoWTC.TTacticalCombatUnit GetNextUnit(Aow.Units.AoWTC.TTacticalCombatUnit U, bool AllUnits, bool Forward)
        {
            Aow.Units.AoWTC.TTacticalCombatUnit Result = default;
            int Index = default;
            int I = default;
            int Start = default;
            int Step = default;
            if ((U != null) && !U.GetEnabled())
            {
                U = null;
            }

            Result = U;
            if (Forward)
            {
                Step = 1;
            }
            else
            {
                Step = -1;
            }

            {
                var w__1073 = FCombat;
                Index = 0;
                I = 0;
                Start = 0;
                if (U != null)
                {
                    while (true)
                    {
                        var call__1074 = w__1073.FCombatData.GetCount();
                        if (!(I < call__1074))
                            break;
                        {
                            var call__1075 = w__1073.FCombatData.GetObjects(I);
                            if (call__1075 == U)
                            {
                                Index = I + Step;
                                Start = I;
                                var call__1076 = w__1073.FCombatData.GetCount();
                                I = call__1076;
                            }

                            I += 1;
                        }
                    }
                }

                while (true)
                {
                    var call__1077 = w__1073.FCombatData.GetCount();
                    if (!((Index < call__1077) && (Index > -1)))
                        break;
                    {
                        var call__1078 = w__1073.FCombatData.GetObjects(Index);
                        bool bool__1079 = (call__1078 != null);
                        if (bool__1079)
                        {
                            var call__1080 = w__1073.FCombatData.GetObjects(Index);
                            bool__1079 = (call__1080 is Aow.Units.AoWE.TCombatUnit);
                        }

                        bool bool__1081 = bool__1079;
                        if (bool__1081)
                        {
                            bool bool__1082 = AllUnits;
                            if (!bool__1082)
                            {
                                var call__1083 = w__1073.FCombatData.GetObjects(Index);
                                bool__1082 = (((Aow.Units.AoWTC.TTacticalCombatUnit)call__1083).FMovePoints > 0);
                            }

                            bool__1081 = bool__1082;
                        }

                        bool bool__1084 = bool__1081;
                        if (bool__1084)
                        {
                            var call__1085 = w__1073.FCombatData.GetObjects(Index);
                            var call__1086 = call__1085.GetEnabled();
                            bool__1084 = call__1086;
                        }

                        bool bool__1087 = bool__1084;
                        if (bool__1087)
                        {
                            var call__1088 = w__1073.FCombatData.GetObjects(Index);
                            bool__1087 = (call__1088.FCombatPlayer == FCurrentCombatPlayer);
                        }

                        if (bool__1087)
                        {
                            var call__1089 = w__1073.FCombatData.GetObjects(Index);
                            Result = (Aow.Units.AoWTC.TTacticalCombatUnit)call__1089;
                            Index = -10;
                        }

                        Index = Index + Step;
                    }
                }

                if (Result == U)
                {
                    if (Forward)
                    {
                        Index = 0;
                    }
                    else
                    {
                        var call__1090 = w__1073.FCombatData.GetCount();
                        Index = call__1090 - 1;
                    }

                    while (true)
                    {
                        bool bool__1091 = (Index != Start) && (Index > -1);
                        if (bool__1091)
                        {
                            var call__1092 = w__1073.FCombatData.GetCount();
                            bool__1091 = (Index < call__1092);
                        }

                        if (!bool__1091)
                            break;
                        {
                            var call__1093 = w__1073.FCombatData.GetObjects(Index);
                            bool bool__1094 = (call__1093 != null);
                            if (bool__1094)
                            {
                                var call__1095 = w__1073.FCombatData.GetObjects(Index);
                                bool__1094 = (call__1095 is Aow.Units.AoWE.TCombatUnit);
                            }

                            bool bool__1096 = bool__1094;
                            if (bool__1096)
                            {
                                bool bool__1097 = AllUnits;
                                if (!bool__1097)
                                {
                                    var call__1098 = w__1073.FCombatData.GetObjects(Index);
                                    bool__1097 = (((Aow.Units.AoWTC.TTacticalCombatUnit)call__1098).FMovePoints > 0);
                                }

                                bool__1096 = bool__1097;
                            }

                            bool bool__1099 = bool__1096;
                            if (bool__1099)
                            {
                                var call__1100 = w__1073.FCombatData.GetObjects(Index);
                                var call__1101 = call__1100.GetEnabled();
                                bool__1099 = call__1101;
                            }

                            bool bool__1102 = bool__1099;
                            if (bool__1102)
                            {
                                var call__1103 = w__1073.FCombatData.GetObjects(Index);
                                bool__1102 = (call__1103.FCombatPlayer == FCurrentCombatPlayer);
                            }

                            if (bool__1102)
                            {
                                var call__1104 = w__1073.FCombatData.GetObjects(Index);
                                Result = (Aow.Units.AoWTC.TTacticalCombatUnit)call__1104;
                                Index = -10;
                            }

                            Index = Index + Step;
                        }
                    }
                }
            }

            return Result;
        }

        // AoWTC.pas:2466
        public Aow.Units.AoWTC.TfxHS AddEffect(Aow.Units.AoWTC.TfxType EffectType)
        {
            Aow.Units.AoWTC.TfxHS Result = default;
            Result = Aow.Units.AoWTC.TfxHS.Create(null);
            FEffects.Add(Result);
            Result.Activate();
            Result.FEffectType = EffectType;
            DynamicFixedVisibleHS.AddChild(Result);
            return Result;
        }

        // AoWTC.pas:2475
        // { conflict: function AddEffect(Effect: Byte): TfxHS; }
        public void UpdateCursor()
        {
            bool bool__1105 = (FSeatedPlayer == -1);
            if (!bool__1105)
            {
                var call__1106 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(FSeatedPlayer);
                bool__1105 = call__1106;
            }

            bool bool__1107 = bool__1105 && (FSeatedPlayer == FCurrentPlayer);
            if (bool__1107)
            {
                var call__1108 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                bool__1107 = !call__1108.FAuto;
            }

            if (!(bool__1107 && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)))
            {
                Mouse.Cursor = (((int)(((uint)Frame) >> 2)) % 4) + 6;
            }
            else
            {
                var call__1109 = FUnitSelection.GetTCUnitHS();
                if (call__1109 != null)
                {
                    FUnitSelection.UpdateMoveCursor();
                }

                switch (Mouse.CursorMode)
                {
                    case 0:
                        Mouse.Cursor = 0;
                        break;
                    case 257:
                        Mouse.Cursor = (((int)(((uint)Frame) >> 2)) % 4) + 52;
                        break;
                    case 258:
                        Mouse.Cursor = (((int)(((uint)Frame) >> 2)) & 1) + 49;
                        break;
                }
            }
        }

        // AoWTC.pas:2489
        public void EndCombat(Aow.Rtl.TObject Sender)
        {
            Aow.Units.CombatTE.TTerminateCombatTE TE = default;
            TE = Aow.Units.CombatTE.TTerminateCombatTE.Create(null);
            Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.TokenExecuter.AddSystemEvent(TE);
            TE.Release();
        }

        // AoWTC.pas:2497
        public void SelectNextUnit(bool Forward)
        {
            Aow.Units.AoWTC.TTacticalCombatUnit U = default;
            int X = default;
            int Y = default;
            if (FSeatedCombatPlayer == FCurrentCombatPlayer)
            {
                var call__1110 = FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                var call__1111 = GetNextUnit(call__1110.FLastUnit, false, Forward);
                U = call__1111;
                if (U != null)
                {
                    var call__1112 = FUnitSelection.GetTCUnitHS();
                    if (call__1112 != U.FUnitHS)
                    {
                        var call__1113 = FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                        call__1113.FLastUnit = U;
                        var call__1114 = U.FUnitHS.GetYhx();
                        var call__1115 = U.FUnitHS.GetXhx();
                        CenterViewTo(call__1115, call__1114, 0);
                        Selection.UnselectLocation();
                        var call__1116 = U.FUnitHS.GetYhx();
                        var call__1117 = U.FUnitHS.GetXhx();
                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__1117, call__1116, ref X, ref Y);
                        Selection.SelectLocation(X, Y, 0);
                    }
                }
            }
        }

        // AoWTC.pas:2557
        public void GetUnitLocation(Aow.Units.AoWE.TAbstractUnit U, ref sbyte X, ref sbyte Y, ref sbyte L)
        {
            int I = default;
            X = unchecked((sbyte)-1);
            I = 0;
            while (true)
            {
                var call__1118 = FCombat.FCombatData.GetCount();
                if (!(I < call__1118))
                    break;
                {
                    var call__1119 = FCombat.FCombatData.GetObjects(I);
                    bool bool__1120 = (((Aow.Units.AoWTC.TTacticalCombatUnit)call__1119).FUnit == U);
                    if (bool__1120)
                    {
                        var call__1121 = FCombat.FCombatData.GetObjects(I);
                        var call__1122 = call__1121.GetEnabled();
                        bool__1120 = call__1122;
                    }

                    if (bool__1120)
                    {
                        var call__1123 = FCombat.FCombatData.GetObjects(I);
                        var call__1124 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__1123).FUnitHS.GetXhx();
                        X = unchecked((sbyte)call__1124);
                        var call__1125 = FCombat.FCombatData.GetObjects(I);
                        var call__1126 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__1125).FUnitHS.GetYhx();
                        Y = unchecked((sbyte)call__1126);
                        L = 0;
                        var call__1127 = FCombat.FCombatData.GetCount();
                        I = call__1127;
                    }
                    else
                    {
                        I += 1;
                    }
                }
            }
        }

        // AoWTC.pas:2572
        public void Surrender(sbyte Mode)
        {
            Aow.Units.CombatTE.TCEndCombatTE TE = default;
            var call__1128 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentPlayer);
            if (call__1128 && (FCurrentCombatPlayer == FSeatedCombatPlayer))
            {
                FUnitSelection.Unselect();
                TE = Aow.Units.CombatTE.CombatTE.CreateTCEndCombatTE();
                if (TE != null)
                {
                    var call__1129 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(FCurrentPlayer);
                    call__1129.AddEvent(TE);
                    TE.Release();
                }
            }
        }

        // AoWTC.pas:484
        public static new Aow.Units.AoWTC.TAoWCombatMap Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWTC.TAoWCombatMap o = new Aow.Units.AoWTC.TAoWCombatMap();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.HSEngine.THSMap.Meta
        {
            // AoWTC.pas:2183
            public override int ClassID()
            {
                int Result = default;
                Result = 2228480;
                return Result;
            }

            // AoWTC.pas:484
            public new Aow.Units.AoWTC.TAoWCombatMap Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AoWTC.TAoWCombatMap o = (Aow.Units.AoWTC.TAoWCombatMap)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AoWTC.TAoWCombatMap => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TTacticalCombat (instance size 80, VMT 00413314)
//     Inherited combat layout; $4C FHostPlayer. }
    public partial class TTacticalCombat : Aow.Units.AoWE.TCombat
    {
        public sbyte FHostPlayer;
        // { $4C }
        public Aow.Units.FixedArray_byte_3 F_TacticalCombat_4D;
        // AoWTC.pas:5086
        public override Aow.Units.AoWE.TCombatUnit CreateCombatUnit()
        {
            Aow.Units.AoWE.TCombatUnit Result = default;
            Result = Aow.Units.AoWTC.TTacticalCombatUnit.Create(null);
            return Result;
        }

        // AoWTC.pas:5157
        public override Aow.Units.AoWE.TCombatPlayer CreateCombatPlayer()
        {
            Aow.Units.AoWE.TCombatPlayer Result = default;
            Result = Aow.Units.AoWTC.TTacticalCombatPlayer.Create(null);
            return Result;
        }

        // AoWTC.pas:5162
        public override Aow.Units.AoWE.TCombatResult Execute()
        {
            Aow.Units.AoWE.TCombatResult Result = default;
            Aow.Units.AoWTC.TRndTacHsm Generator = default;
            int I = default;
            int Seated = default;
            if (!ValidateWallCombat())
            {
                SetLogCombatActions(1);
                var call__1130 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CannotBreachWallRStr);
                SetLogbookHeader(call__1130);
                FCombatResult = Aow.Units.AoWE.TCombatResult.crCannotBreachWall;
                Result = FCombatResult;
            }
            else
            {
                var call__1131 = FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].GetCount();
                if (call__1131 == FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].FConquerCount)
                {
                    SetLogCombatActions(1);
                    var call__1132 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CannotInflictDamageRStr);
                    SetLogbookHeader(call__1132);
                    FCombatResult = Aow.Units.AoWE.TCombatResult.crCannotInflictDamage;
                    Result = FCombatResult;
                }
                else
                {
                    var call__1133 = FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].GetCount();
                    if (call__1133 == 0)
                    {
                        Result = Aow.Units.AoWE.TCombatResult.crNoDefenders;
                    }
                    else
                    {
                        Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.TokenExecuter.HoldTC = true;
                        Generator = Aow.Units.AoWTC.TRndTacHsm.Create();
                        Generator.RandomMap(this, FCombatData.FX, FCombatData.FY, FCombatData.FL);
                        Generator.Free();
                        bool bool__1134 = Aow.Units.AoWE.AoWE.AoWHSMap.FStartingGame;
                        if (!bool__1134)
                        {
                            var call__1135 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(FCombatData.FX, FCombatData.FY, FCombatData.FL, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                            bool__1134 = call__1135;
                        }

                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds = bool__1134;
                        if (!Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                        {
                            {
                                I = 0;
                                int end__1136 = FCombatData.FPlayers.GetCount() - 1;
                                for (; I <= end__1136; I += 1)
                                {
                                    var call__1137 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                                    var call__1138 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1137.FPlayer);
                                    if (call__1138.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman)
                                    {
                                        var call__1139 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                                        var call__1140 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1139.FPlayer);
                                        var call__1141 = call__1140.GetLocal();
                                        if (call__1141)
                                        {
                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds = true;
                                        }
                                    }
                                }
                            }
                        }

                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FDeselectEvents.TriggerNotifyEvents(this);
                        Seated = -1;
                        {
                            I = 0;
                            int end__1142 = FCombatData.FPlayers.GetCount() - 1;
                            for (; I <= end__1142; I += 1)
                            {
                                if (Seated == -1)
                                {
                                    var call__1143 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                                    var call__1144 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1143.FPlayer);
                                    if (call__1144.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman)
                                    {
                                        var call__1145 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                                        var call__1146 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(call__1145.FPlayer);
                                        if (call__1146.ConnectionMode != Aow.Units.NetworkE.TConnectionMode.cmRemote)
                                        {
                                            var call__1147 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                                            var call__1148 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1147.FPlayer);
                                            if (call__1148.FStatus != Aow.Units.AoWE.TPlayerStatus.psTerminated)
                                            {
                                                Seated = I;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.SetSeatedCombatPlayer(unchecked((sbyte)Seated));
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(32, 29, 0);
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.NewRound();
                        var call__1149 = FCombatData.FTurnOrder[0];
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.NewTurn(unchecked((sbyte)call__1149));
                        Result = Aow.Units.AoWE.TCombatResult.crBusy;
                        FOnTerminated = Aow.Units.AoWTC.AoWTC.AoWCombatMap.EndCombat;
                        SetLogCombatActions(1);
                    }
                }
            }

            return Result;
        }

        // AoWTC.pas:5097
        public override void Initialize()
        {
            int I = default;
            base.Initialize();
            FCombatData.FTurnOrder.Clear();
            FCombatData.FTurnOrder.Add(unchecked((byte)FCombatData.FAttackerPlayer));
            FHostPlayer = unchecked((sbyte)-1);
            {
                I = 0;
                int end__1150 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetCount() - 1;
                for (; I <= end__1150; I += 1)
                {
                    if (FHostPlayer == -1)
                    {
                        var call__1151 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                        if (call__1151.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman)
                        {
                            var call__1152 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                            if (call__1152.FStatus != Aow.Units.AoWE.TPlayerStatus.psTerminated)
                            {
                                FHostPlayer = unchecked((sbyte)I);
                            }
                        }
                    }
                }
            }

            {
                I = 0;
                int end__1153 = FCombatData.FPlayers.GetCount() - 1;
                for (; I <= end__1153; I += 1)
                {
                    var call__1154 = FCombatData.FTurnOrder.IndexOf(unchecked((byte)I));
                    if (call__1154 == -1)
                    {
                        var call__1155 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                        if (call__1155.FSideIndex == 0)
                        {
                            FCombatData.FTurnOrder.Add(unchecked((byte)I));
                        }
                    }

                    var call__1156 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                    var call__1157 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1156.FPlayer);
                    if (call__1157.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman)
                    {
                        if (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC is Aow.Units.NetworkE.TRemotePCC)
                        {
                            var call__1158 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FHostPlayer);
                            var call__1159 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                            ((Aow.Units.NetworkE.TRemotePCC)Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC).SetConnectionMode(call__1159.FPlayer, call__1158.ConnectionMode);
                            var call__1160 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FHostPlayer);
                            var call__1161 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                            ((Aow.Units.NetworkE.TRemotePCC)Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC).SetSystemID(call__1161.FPlayer, call__1160.SystemID);
                        }
                    }
                }
            }

            {
                I = 0;
                int end__1162 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetCount() - 1;
                for (; I <= end__1162; I += 1)
                {
                    var call__1163 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(I);
                    var call__1164 = call__1163.Local();
                    bool bool__1165 = call__1164;
                    if (!bool__1165)
                    {
                        var call__1166 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(I);
                        var call__1167 = call__1166.Global();
                        bool__1165 = call__1167;
                    }

                    if (bool__1165)
                    {
                        var call__1168 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(I);
                        call__1168.ClearEvents();
                    }
                }
            }

            FCombatData.FTurnOrder.Add(unchecked((byte)FCombatData.FDefenderPlayer));
            {
                I = 0;
                int end__1169 = FCombatData.FPlayers.GetCount() - 1;
                for (; I <= end__1169; I += 1)
                {
                    var call__1170 = FCombatData.FTurnOrder.IndexOf(unchecked((byte)I));
                    if (call__1170 == -1)
                    {
                        var call__1171 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                        if (call__1171.FSideIndex == 1)
                        {
                            FCombatData.FTurnOrder.Add(unchecked((byte)I));
                        }
                    }
                }
            }

            SetLogCombatActions(1);
        }

        // AoWTC.pas:5129
        public override void Finalize()
        {
            int I = default;
            {
                I = 0;
                int end__1172 = FCombatData.FPlayers.GetCount() - 1;
                for (; I <= end__1172; I += 1)
                {
                    var call__1173 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                    var call__1174 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1173.FPlayer);
                    if (call__1174.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman)
                    {
                        var call__1175 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                        ((Aow.Units.NetworkE.TRemotePCC)Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC).SetConnectionMode(call__1175.FPlayer, Aow.Units.NetworkE.TConnectionMode.cmGlobal);
                        var call__1176 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                        ((Aow.Units.NetworkE.TRemotePCC)Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC).SetSystemID(call__1176.FPlayer, 0);
                    }
                }
            }

            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
            {
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.Clear();
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSelectEvents.TriggerNotifyEvents(this);
                Aow.Units.AoWE.AoWE.AoWHSMap.ReleaseCombatMap();
            }

            base.Finalize();
        }

        // AoWTC.pas:5091
        public override void Deactivate()
        {
            if (FCombatResult != Aow.Units.AoWE.TCombatResult.crCannotBreachWall)
            {
                UpdateStatus();
            }

            base.Deactivate();
        }

        // AoWTC.pas:5145
        public bool ValidateWallCombat()
        {
            bool Result = default;
            int I = default;
            Aow.Units.AoWE.TCombatSide Side = default;
            Side = FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csAttacker];
            var call__1177 = FCombatData.FParties.GetParty(0);
            Result = call__1177.FWall == 0;
            I = Side.GetCount();
            while (!Result && (I > 0))
            {
                I -= 1;
                var call__1178 = Side.GetObjects(I);
                var call__1179 = call__1178.WallCombatFeatures();
                Result = (call__1179 & TSet<Aow.Units.AoWE.TWallCombatFeaturesElement>.Of(Aow.Units.AoWE.TWallCombatFeaturesElement.wcWallBreaching, Aow.Units.AoWE.TWallCombatFeaturesElement.wcWallPassing)) != TSet<Aow.Units.AoWE.TWallCombatFeaturesElement>.Of();
            }

            return Result;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // AoWTC.pas:5081
            public override int ClassID()
            {
                int Result = default;
                Result = 2228496;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AoWTC.TTacticalCombat => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P3_02.inc:318
            public new Aow.Units.AoWTC.TTacticalCombat Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AoWTC.TTacticalCombat o = (Aow.Units.AoWTC.TTacticalCombat)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TRndTacHsm (instance size 3708, VMT 0041340C)
//     $04 FItems; $08 FRandomTerrain; $0C neighbor weights; $5C terrain weights;
//     $520 terrain-tag weights; $9F0 terrain samples; $9F7 tag samples; $A00 resource lists; $E78 FHomePosition. }
    public partial class TRndTacHsm : Aow.Rtl.TObject
    {
        public Aow.Units.AoWE.TItemList FItems;
        // { $04; 00426CD4 calls GetItem at VMT $8C. }
        public bool FRandomTerrain;
        // { $08 }
        public Aow.Units.FixedArray_byte_3 F_RndTacHsm_09;
        public Aow.Units.FixedArray_int_20 FNeighborWeights;
        // { $0C }
        public Aow.Units.FixedArray_int_305 FTerrainWeights;
        // { $5C }
        public Aow.Units.FixedArray_int_308 FTagWeights;
        // { $520 }
        public Aow.Units.FixedArray_sbyte_7 FTerrain;
        // { $9F0 }
        public Aow.Units.FixedArray_sbyte_7 FTags;
        // { $9F7 }
        public Aow.Units.FixedArray_byte_2 F_RndTacHsm_9FE;
        public Aow.Units.FixedArray_Aow_Rtl_TList_286 FResources;
        // { $A00; original allocates one extra list }
        public int FHomePosition;
        // AoWTC.pas:4149
        // { $E78 }
        public override void Destroy()
        {
            int I = default;
            {
                I = 0;
                short end__1180 = 285;
                for (; I <= end__1180; I += 1)
                {
                    FResources[I].Free();
                }
            }

            base.Destroy();
        }

        // AoWTC.pas:4137
        public void Create__init_TRndTacHsm()
        {
            int I = default;
            int J = default;
            ;
            {
                I = 0;
                short end__1181 = 285;
                for (; I <= end__1181; I += 1)
                {
                    var call__1182 = new Aow.Rtl.TList();
                    FResources[I] = call__1182;
                }
            }

            {
                J = 0;
                int end__1183 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.GetCount() - 1;
                for (; J <= end__1183; J += 1)
                {
                    var call__1184 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.MORs[J];
                    if (call__1184 is Aow.Units.CombatTerrain.TCombatTerrainResource)
                    {
                        var call__1185 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.MORs[J];
                        var call__1186 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.MORs[J];
                        var call__1187 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.MORs[J];
                        FResources[(unchecked((sbyte)((Aow.Units.CombatTerrain.TCombatTerrainResource)call__1186).TerrainType) * 19) + ((Aow.Units.CombatTerrain.TCombatTerrainResource)call__1187).DecorationType].Add(call__1185);
                    }
                }
            }

            InitTables();
        }

        // AoWTC.pas:4156
        public Aow.Units.AoWTC.TAoWCombatMap RandomMap(Aow.Units.AoWTC.TTacticalCombat Combat, int X, int Y, int L)
        {
            Aow.Units.AoWTC.TAoWCombatMap Result = default;
            Ptr<byte> TerrainBuffer = default;
            int I = default;
            int J = default;
            int K = default;
            int NX = default;
            int Col = default;
            int Row = default;
            int TerrainType = default;
            int Tag = default;
            Ptr<byte> ShapeBuffer = default;
            byte Shape = default;
            bool HasUnit = default;
            bool SiteCombat = default;
            bool RandomTerrain = default;
            Aow.Rtl.AnsiString FileName = default;
            Aow.Units.HSEngine.THexagonSprite Structure = default;
            Aow.Units.FixedArray_int_20 Weights = default;
            bool Blocked(int X, int Y, int First, int Last)
            {
                bool Result = default;
                int I = default;
                int Kind = default;
                int NX = default;
                int NY = default;
                Result = false;
                {
                    I = First;
                    int end__1188 = Last;
                    for (; I <= end__1188; I += 1)
                    {
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, Aow.Units.AoWTC.AoWTC.clusterChk[I], ref NX, ref NY);
                        Kind = TerrainBuffer.Reinterpret<byte>().Reinterpret<byte>()[(NY * 65) + NX];
                        Result = ((((((((((((((Result || (Kind == 1)) || (Kind == 2)) || (Kind == 3)) || (Kind == 5)) || (Kind == 6)) || (Kind == 7)) || (Kind == 11)) || (Kind == 14)) || (Kind == 15)) || (Kind == 16)) || (Kind == 17)) || (Kind == 18)) || (Kind == 252)) || (Kind == 253)) || (Kind == 254);
                    }
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
                RandomTerrain = !(SiteCombat || (Combat.FExplorationSite == 2));
            }

            FRandomTerrain = RandomTerrain;
            Aow.Units.AoWTC.AoWTC.CombatXLoc = X;
            Aow.Units.AoWTC.AoWTC.CombatYLoc = Y;
            Aow.Units.AoWTC.AoWTC.CombatLLoc = L;
            Aow.Rtl.Rtl.RandSeed = (((Y * 128) + X) + (L * 16384)) + Aow.Units.AoWE.AoWE.AoWHSMap.FInitialRandomSeed;
            {
                I = 0;
                byte end__1189 = 6;
                for (; I <= end__1189; I += 1)
                {
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                    var call__1190 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                    var call__1191 = call__1190.Field[J, K];
                    FTerrain[I] = unchecked((sbyte)call__1191.TerrainType);
                    var call__1192 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                    var call__1193 = call__1192.Field[J, K];
                    var call__1194 = ((Aow.Units.AoWE.TAoWMapField)call__1193).GetCombatTerrainTag();
                    FTags[I] = unchecked((sbyte)call__1194);
                }
            }

            if (((FTerrain[0] == 0) || (FTerrain[0] == 9)) || (FTerrain[0] == 10))
            {
                {
                    I = 1;
                    byte end__1195 = 6;
                    for (; I <= end__1195; I += 1)
                    {
                        FTerrain[I] = unchecked((sbyte)FTerrain[0]);
                    }
                }
            }
            else
            {
                {
                    I = 1;
                    byte end__1196 = 6;
                    for (; I <= end__1196; I += 1)
                    {
                        if ((((FTerrain[I] == 0) || (FTerrain[I] == 15)) || (FTerrain[I] == 10)) || (FTerrain[I] == 9))
                        {
                            FTerrain[I] = unchecked((sbyte)FTerrain[0]);
                        }
                    }
                }
            }

            FileName = (AnsiString)"TCMaps\\";
            var call__1197 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
            var call__1198 = call__1197.Field[X, Y];
            var call__1199 = call__1198.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
            Structure = call__1199;
            if (Structure == null)
            {
                var call__1200 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                var call__1201 = call__1200.Field[X, Y];
                var call__1202 = call__1201.FindNoneTransparentHS(typeof(Aow.Units.HxBridge.THexagonBridge));
                Structure = call__1202;
            }

            if (Structure != null)
            {
                switch (Structure.ClassType.ClassID())
                {
                    case 131336:
                        FileName = FileName + AnsiString.FromBytes(48);
                    {
                        I = 1;
                        byte end__1203 = 6;
                        for (; I <= end__1203; I += 1)
                        {
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                            var call__1204 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                            var call__1205 = call__1204.Field[J, K];
                            var call__1206 = call__1205.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                            if (call__1206 != null)
                            {
                                FileName = FileName + Aow.Rtl.Rtl.IntToStr(I);
                                FTags[I] = 127;
                            }
                        }
                    }

                        while (FileName.Length < 11)
                        {
                            FileName = FileName + AnsiString.FromBytes(120);
                        }

                        FileName = FileName + ((AnsiString)"C000");
                        break;
                    case 131950:
                        FileName = FileName + ((AnsiString)"Farmx000");
                        break;
                    case 131962:
                        FileName = FileName + ((AnsiString)"Minex000");
                        break;
                    case 131328:
                        FileName = FileName + ((AnsiString)"SLife000");
                        break;
                    case 133454:
                        FileName = FileName + ((AnsiString)"NodeA000");
                        break;
                    case 132270:
                        FileName = FileName + ((AnsiString)"NodeD000");
                        break;
                    case 131482:
                        FileName = FileName + ((AnsiString)"NodeC000");
                        break;
                    case 132302:
                        FileName = FileName + ((AnsiString)"NodeE000");
                        break;
                    case 132062:
                        FileName = FileName + ((AnsiString)"NodeF000");
                        break;
                    case 131953:
                        FileName = FileName + ((AnsiString)"NodeL000");
                        break;
                    case 132275:
                        FileName = FileName + ((AnsiString)"NodeW000");
                        break;
                    case 132670:
                        FileName = FileName + ((AnsiString)"RPool000");
                        break;
                    case 133422:
                        FileName = FileName + ((AnsiString)"BGuil000");
                        break;
                    case 132286:
                        FileName = FileName + ((AnsiString)"SYard000");
                        break;
                    case 132158:
                        FileName = FileName + ((AnsiString)"TFort000");
                        break;
                    case 131966:
                        if (RandomTerrain)
                        {
                            FileName = FileName + ((AnsiString)"CaveN000");
                        }
                        else
                        {
                            FileName = FileName + ((AnsiString)"CaveE000");
                        }

                        break;
                    case 131332:
                        if (FRandomTerrain)
                        {
                            FileName = FileName + ((AnsiString)"TeleN000");
                        }
                        else
                        {
                            FileName = FileName + ((AnsiString)"Telep000");
                        }

                        RandomTerrain = true;
                        break;
                    case 131886:
                        if (SiteCombat)
                        {
                            FItems = (Aow.Units.AoWE.TItemList)Combat.FItems;
                            FileName = (FileName + ((AnsiString)"eMrLr00")) + Aow.Rtl.Rtl.IntToStr(Aow.Rtl.Rtl.Random(4));
                            {
                                I = 1;
                                byte end__1207 = 6;
                                for (; I <= end__1207; I += 1)
                                {
                                    FTags[I] = 127;
                                }
                            }
                        }
                        else
                        {
                            FileName = FileName + ((AnsiString)"Blank000");
                        }

                        break;
                    case 131982:
                        if (SiteCombat)
                        {
                            FItems = (Aow.Units.AoWE.TItemList)Combat.FItems;
                            FileName = (FileName + ((AnsiString)"eZigg00")) + Aow.Rtl.Rtl.IntToStr(Aow.Rtl.Rtl.Random(2));
                            {
                                I = 1;
                                byte end__1208 = 6;
                                for (; I <= end__1208; I += 1)
                                {
                                    FTags[I] = 127;
                                }
                            }
                        }
                        else
                        {
                            FileName = FileName + ((AnsiString)"Blank000");
                        }

                        break;
                    case 131850:
                        if (SiteCombat)
                        {
                            FItems = (Aow.Units.AoWE.TItemList)Combat.FItems;
                            FileName = (FileName + ((AnsiString)"eCrpt00")) + Aow.Rtl.Rtl.IntToStr(Aow.Rtl.Rtl.Random(2));
                            {
                                I = 1;
                                byte end__1209 = 6;
                                for (; I <= end__1209; I += 1)
                                {
                                    FTags[I] = 127;
                                }
                            }
                        }
                        else
                        {
                            FileName = FileName + ((AnsiString)"Blank000");
                        }

                        break;
                    case 131994:
                        if (SiteCombat)
                        {
                            FItems = (Aow.Units.AoWE.TItemList)Combat.FItems;
                            FileName = (FileName + ((AnsiString)"eRuin00")) + Aow.Rtl.Rtl.IntToStr(Aow.Rtl.Rtl.Random(2));
                            {
                                I = 1;
                                byte end__1210 = 6;
                                for (; I <= end__1210; I += 1)
                                {
                                    FTags[I] = 127;
                                }
                            }
                        }
                        else
                        {
                            FileName = FileName + ((AnsiString)"Blank000");
                        }

                        break;
                    case 131882:
                        if (SiteCombat)
                        {
                            FileName = (FileName + ((AnsiString)"eDung00")) + Aow.Rtl.Rtl.IntToStr(Aow.Rtl.Rtl.Random(2));
                            {
                                I = 1;
                                byte end__1211 = 6;
                                for (; I <= end__1211; I += 1)
                                {
                                    FTags[I] = 127;
                                }
                            }
                        }
                        else
                        {
                            FileName = FileName + ((AnsiString)"Blank000");
                        }

                        break;
                    case 133242:
                        RandomTerrain = false;
                        FRandomTerrain = false;
                        FileName = FileName + ((AnsiString)"Br");
                        switch (FTerrain[0])
                        {
                            case 10:
                                FileName = FileName + ((AnsiString)"UW");
                                break;
                            case 6:
                                FileName = FileName + ((AnsiString)"Ic");
                                break;
                            case 9:
                                FileName = FileName + ((AnsiString)"La");
                                break;
                            case 13:
                                FileName = FileName + ((AnsiString)"UI");
                                break;
                            default:
                                FileName = FileName + ((AnsiString)"Wa");
                                break;
                        }

                        switch (((Aow.Units.HxBridge.THexagonBridge)Structure).FDirection)
                        {
                            case 1:
                                FileName = FileName + ((AnsiString)"A000");
                                break;
                            case 2:
                                FileName = FileName + ((AnsiString)"B000");
                                break;
                            default:
                                FileName = FileName + ((AnsiString)"C000");
                                break;
                        }

                        break;
                    default:
                        FileName = FileName + ((AnsiString)"Blank000");
                        break;
                }

                FTags[0] = 127;
            }

            if (FTags[0] != 127)
            {
                FileName = FileName + ((AnsiString)"Blank000");
            }

            FileName = FileName + ((AnsiString)".HSM");
            if (Combat == null)
            {
                Aow.Units.AoWE.AoWE.AoWEngine.LoadHSMEx(Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory + FileName, false, null);
            }
            else
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.InitializeCombatMap(Combat, FileName);
            }

            Aow.Units.AoWTC.AoWTC.AoWCombatMap.SetCombat(Combat);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.SetExplorationEnabled(SiteCombat);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.SetFogEnabled(false);
            if (Combat != null)
            {
                PlaceCombatUnits();
            }

            if (RandomTerrain)
            {
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.LockMapChangedEvent();
                ShapeBuffer = Ptr<byte>.Unbox(Aow.Rtl.Rtl.AllocMem(unchecked((uint)((uint)3770))));
                TerrainBuffer = Ptr<byte>.Unbox(Aow.Rtl.Rtl.AllocMem(unchecked((uint)((uint)3770))));
                {
                    Row = 6;
                    byte end__1212 = 52;
                    for (; Row <= end__1212; Row += 1)
                    {
                        {
                            Col = 6;
                            byte end__1213 = 58;
                            for (; Col <= end__1213; Col += 1)
                            {
                                Shape = unchecked((byte)Aow.Units.AoWTC.AoWTC.MapShape[(Row * 65) + Col]);
                                switch (Shape)
                                {
                                    case 14:
                                        if (Aow.Rtl.Rtl.Random(100) < 50)
                                        {
                                            Shape = 1;
                                        }
                                        else
                                        {
                                            Shape = 2;
                                        }

                                        break;
                                    case 15:
                                        if (Aow.Rtl.Rtl.Random(100) < 50)
                                        {
                                            Shape = 1;
                                        }
                                        else
                                        {
                                            Shape = 3;
                                        }

                                        break;
                                    case 16:
                                        if (Aow.Rtl.Rtl.Random(100) < 50)
                                        {
                                            Shape = 1;
                                        }
                                        else
                                        {
                                            Shape = 4;
                                        }

                                        break;
                                    case 17:
                                        if (Aow.Rtl.Rtl.Random(100) < 50)
                                        {
                                            Shape = 1;
                                        }
                                        else
                                        {
                                            Shape = 5;
                                        }

                                        break;
                                    case 18:
                                        if (Aow.Rtl.Rtl.Random(100) < 50)
                                        {
                                            Shape = 1;
                                        }
                                        else
                                        {
                                            Shape = 6;
                                        }

                                        break;
                                    case 19:
                                        if (Aow.Rtl.Rtl.Random(100) < 50)
                                        {
                                            Shape = 1;
                                        }
                                        else
                                        {
                                            Shape = 7;
                                        }

                                        break;
                                    case 11:
                                        if (Aow.Rtl.Rtl.Random(100) < 50)
                                        {
                                            Shape = 2;
                                        }
                                        else
                                        {
                                            Shape = 3;
                                        }

                                        break;
                                    case 9:
                                        if (Aow.Rtl.Rtl.Random(100) < 50)
                                        {
                                            Shape = 3;
                                        }
                                        else
                                        {
                                            Shape = 4;
                                        }

                                        break;
                                    case 13:
                                        if (Aow.Rtl.Rtl.Random(100) < 50)
                                        {
                                            Shape = 5;
                                        }
                                        else
                                        {
                                            Shape = 4;
                                        }

                                        break;
                                    case 12:
                                        if (Aow.Rtl.Rtl.Random(100) < 50)
                                        {
                                            Shape = 5;
                                        }
                                        else
                                        {
                                            Shape = 6;
                                        }

                                        break;
                                    case 8:
                                        if (Aow.Rtl.Rtl.Random(100) < 50)
                                        {
                                            Shape = 7;
                                        }
                                        else
                                        {
                                            Shape = 6;
                                        }

                                        break;
                                    case 10:
                                        if (Aow.Rtl.Rtl.Random(100) < 50)
                                        {
                                            Shape = 2;
                                        }
                                        else
                                        {
                                            Shape = 7;
                                        }

                                        break;
                                }

                                var call__1214 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__1215 = call__1214.Field[Col, Row];
                                var call__1216 = call__1215.GetDecorationType();
                                if (call__1216 == 9)
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
                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, J, ref K, ref NX);
                                        var call__1217 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                                        var call__1218 = call__1217.Field[K, NX];
                                        var call__1219 = call__1218.FindOwnedChild(131946);
                                        if (call__1219 == null)
                                        {
                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.ClearTerrain(Col, Row, 0, 0, -1);
                                        }
                                    }
                                }

                                ShapeBuffer[(Row * 65) + Col] = unchecked((byte)unchecked((byte)Shape));
                            }
                        }
                    }
                }

                var call__1220 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                var call__1221 = call__1220.Field[X, Y];
                if (call__1221.OverlayType == 4)
                {
                    {
                        I = 0;
                        byte end__1222 = 6;
                        for (; I <= end__1222; I += 1)
                        {
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                            var call__1223 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                            var call__1224 = call__1223.Field[J, K];
                            Weights[I] = call__1224.OverlayType;
                        }
                    }

                    {
                        Row = 6;
                        byte end__1225 = 52;
                        for (; Row <= end__1225; Row += 1)
                        {
                            {
                                Col = 6;
                                byte end__1226 = 58;
                                for (; Col <= end__1226; Col += 1)
                                {
                                    Shape = unchecked((byte)Aow.Units.AoWTC.AoWTC.MapRoads[(Row * 65) + Col]);
                                    switch (Shape)
                                    {
                                        case 21:
                                            if (Aow.Rtl.Rtl.Random(100) < 50)
                                            {
                                                Shape = 0;
                                            }
                                            else
                                            {
                                                Shape = 2;
                                            }

                                            break;
                                        case 22:
                                            if (Aow.Rtl.Rtl.Random(100) < 50)
                                            {
                                                Shape = 0;
                                            }
                                            else
                                            {
                                                Shape = 3;
                                            }

                                            break;
                                        case 23:
                                            if (Aow.Rtl.Rtl.Random(100) < 50)
                                            {
                                                Shape = 0;
                                            }
                                            else
                                            {
                                                Shape = 4;
                                            }

                                            break;
                                        case 24:
                                            if (Aow.Rtl.Rtl.Random(100) < 50)
                                            {
                                                Shape = 0;
                                            }
                                            else
                                            {
                                                Shape = 5;
                                            }

                                            break;
                                        case 25:
                                            if (Aow.Rtl.Rtl.Random(100) < 50)
                                            {
                                                Shape = 0;
                                            }
                                            else
                                            {
                                                Shape = 6;
                                            }

                                            break;
                                        case 26:
                                            if (Aow.Rtl.Rtl.Random(100) < 50)
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
                                        if ((FTags[Shape - 1] != 127) && (Weights[Shape - 1] == 4))
                                        {
                                            Aow.Units.AoWE.AoWE.AoWHSSet.FCombatRoads.Place(Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer, unchecked((sbyte)Col), unchecked((sbyte)Row), 0, 1);
                                            TerrainBuffer[(Row * 65) + Col] = unchecked((byte)unchecked((byte)251));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                {
                    Row = 6;
                    byte end__1227 = 52;
                    for (; Row <= end__1227; Row += 1)
                    {
                        {
                            Col = 3;
                            byte end__1228 = 29;
                            for (; Col <= end__1228; Col += 1)
                            {
                                I = ShapeBuffer[(Row * 65) + (Col * 2)];
                                if (I > 0)
                                {
                                    if ((FTags[I - 1] == 127) || (unchecked((int)TerrainBuffer[(Row * 65) + (Col * 2)]) == 251))
                                    {
                                        I = 0;
                                    }
                                }

                                if (I > 0)
                                {
                                    TerrainType = FTerrain[I - 1];
                                    Tag = FTags[I - 1];
                                    {
                                        I = 0;
                                        byte end__1229 = 18;
                                        for (; I <= end__1229; I += 1)
                                        {
                                            Weights[I] = FTerrainWeights[(TerrainType * 19) + I];
                                            if (Tag < 14)
                                            {
                                                Weights[I] += FTagWeights[(Tag * 19) + I];
                                            }
                                        }
                                    }

                                    {
                                        I = 0;
                                        byte end__1230 = 2;
                                        for (; I <= end__1230; I += 1)
                                        {
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Col * 2, Row, Aow.Units.AoWTC.AoWTC.clusterChk[I], ref J, ref K);
                                            Tag = TerrainBuffer[(K * 65) + J];
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
                                    }

                                    var call__1231 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__1232 = call__1231.Field[Col * 2, Row];
                                    var call__1233 = call__1232.FindChild(2228484);
                                    HasUnit = call__1233 != null;
                                    if (HasUnit || Blocked(Col * 2, Row, 0, 2))
                                    {
                                        bool bool__1234 = HasUnit;
                                        if (!bool__1234)
                                        {
                                            var call__1235 = Blocked(Col * 2, Row, 4, 8);
                                            bool__1234 = call__1235;
                                        }

                                        bool bool__1236 = bool__1234;
                                        if (bool__1236)
                                        {
                                            bool bool__1237 = HasUnit;
                                            if (!bool__1237)
                                            {
                                                var call__1238 = Blocked(Col * 2, Row, 9, 17);
                                                bool__1237 = call__1238;
                                            }

                                            bool__1236 = bool__1237;
                                        }

                                        if (bool__1236)
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
                                        bool bool__1239 = HasUnit;
                                        if (!bool__1239)
                                        {
                                            var call__1240 = Blocked(Col * 2, Row, 4, 8);
                                            bool__1239 = call__1240;
                                        }

                                        bool bool__1241 = bool__1239;
                                        if (bool__1241)
                                        {
                                            bool bool__1242 = HasUnit;
                                            if (!bool__1242)
                                            {
                                                var call__1243 = Blocked(Col * 2, Row, 9, 17);
                                                bool__1242 = call__1243;
                                            }

                                            bool__1241 = bool__1242;
                                        }

                                        if (bool__1241)
                                        {
                                            Weights[14] = 0;
                                            Weights[15] = 0;
                                            Weights[16] = 0;
                                        }
                                    }

                                    J = 0;
                                    {
                                        I = 0;
                                        byte end__1244 = 18;
                                        for (; I <= end__1244; I += 1)
                                        {
                                            J += Weights[I];
                                        }
                                    }

                                    var call__1245 = Aow.Rtl.Rtl.Random(J);
                                    Tag = call__1245 - Weights[0];
                                    J = 0;
                                    while ((Tag > 0) && (J < 18))
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
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Col * 2, Row, 6, ref K, ref NX);
                                            TerrainBuffer[(NX * 65) + K] = unchecked((byte)unchecked((byte)252));
                                            break;
                                        case 15:
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Col * 2, Row, 6, ref K, ref NX);
                                            TerrainBuffer[(NX * 65) + K] = unchecked((byte)unchecked((byte)253));
                                            break;
                                        case 16:
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Col * 2, Row, 1, ref K, ref NX);
                                            TerrainBuffer[(NX * 65) + K] = unchecked((byte)unchecked((byte)254));
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Col * 2, Row, 2, ref K, ref NX);
                                            TerrainBuffer[(NX * 65) + K] = unchecked((byte)unchecked((byte)254));
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Col * 2, Row, 6, ref K, ref NX);
                                            TerrainBuffer[(NX * 65) + K] = unchecked((byte)unchecked((byte)254));
                                            break;
                                    }

                                    TerrainBuffer[(Row * 65) + (Col * 2)] = unchecked((byte)unchecked((byte)J));
                                }
                            }
                        }

                        {
                            Col = 3;
                            byte end__1246 = 29;
                            for (; Col <= end__1246; Col += 1)
                            {
                                I = ShapeBuffer[((Row * 65) + (Col * 2)) + 1];
                                if (I > 0)
                                {
                                    if ((FTags[I - 1] == 127) || (unchecked((int)TerrainBuffer[((Row * 65) + (Col * 2)) + 1]) == 251))
                                    {
                                        I = 0;
                                    }
                                }

                                if (I > 0)
                                {
                                    TerrainType = FTerrain[I - 1];
                                    Tag = FTags[I - 1];
                                    {
                                        I = 0;
                                        byte end__1247 = 18;
                                        for (; I <= end__1247; I += 1)
                                        {
                                            Weights[I] = FTerrainWeights[(TerrainType * 19) + I];
                                            if (Tag < 14)
                                            {
                                                Weights[I] += FTagWeights[(Tag * 19) + I];
                                            }
                                        }
                                    }

                                    {
                                        I = 0;
                                        byte end__1248 = 2;
                                        for (; I <= end__1248; I += 1)
                                        {
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX((Col * 2) + 1, Row, Aow.Units.AoWTC.AoWTC.clusterChk[I], ref J, ref K);
                                            Tag = TerrainBuffer[(K * 65) + J];
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
                                    }

                                    var call__1249 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__1250 = call__1249.Field[(Col * 2) + 1, Row];
                                    var call__1251 = call__1250.FindChild(2228484);
                                    HasUnit = call__1251 != null;
                                    if (HasUnit || Blocked((Col * 2) + 1, Row, 0, 2))
                                    {
                                        bool bool__1252 = HasUnit;
                                        if (!bool__1252)
                                        {
                                            var call__1253 = Blocked((Col * 2) + 1, Row, 4, 8);
                                            bool__1252 = call__1253;
                                        }

                                        bool bool__1254 = bool__1252;
                                        if (bool__1254)
                                        {
                                            bool bool__1255 = HasUnit;
                                            if (!bool__1255)
                                            {
                                                var call__1256 = Blocked((Col * 2) + 1, Row, 9, 17);
                                                bool__1255 = call__1256;
                                            }

                                            bool__1254 = bool__1255;
                                        }

                                        if (bool__1254)
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
                                        bool bool__1257 = HasUnit;
                                        if (!bool__1257)
                                        {
                                            var call__1258 = Blocked((Col * 2) + 1, Row, 4, 8);
                                            bool__1257 = call__1258;
                                        }

                                        bool bool__1259 = bool__1257;
                                        if (bool__1259)
                                        {
                                            bool bool__1260 = HasUnit;
                                            if (!bool__1260)
                                            {
                                                var call__1261 = Blocked((Col * 2) + 1, Row, 9, 17);
                                                bool__1260 = call__1261;
                                            }

                                            bool__1259 = bool__1260;
                                        }

                                        if (bool__1259)
                                        {
                                            Weights[14] = 0;
                                            Weights[15] = 0;
                                            Weights[16] = 0;
                                        }
                                    }

                                    J = 0;
                                    {
                                        I = 0;
                                        byte end__1262 = 18;
                                        for (; I <= end__1262; I += 1)
                                        {
                                            J += Weights[I];
                                        }
                                    }

                                    var call__1263 = Aow.Rtl.Rtl.Random(J);
                                    Tag = call__1263 - Weights[0];
                                    J = 0;
                                    while ((Tag > 0) && (J < 18))
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
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX((Col * 2) + 1, Row, 6, ref K, ref NX);
                                            TerrainBuffer[(NX * 65) + K] = unchecked((byte)unchecked((byte)252));
                                            break;
                                        case 15:
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX((Col * 2) + 1, Row, 6, ref K, ref NX);
                                            TerrainBuffer[(NX * 65) + K] = unchecked((byte)unchecked((byte)253));
                                            break;
                                        case 16:
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX((Col * 2) + 1, Row, 1, ref K, ref NX);
                                            TerrainBuffer[(NX * 65) + K] = unchecked((byte)unchecked((byte)254));
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX((Col * 2) + 1, Row, 2, ref K, ref NX);
                                            TerrainBuffer[(NX * 65) + K] = unchecked((byte)unchecked((byte)254));
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX((Col * 2) + 1, Row, 6, ref K, ref NX);
                                            TerrainBuffer[(NX * 65) + K] = unchecked((byte)unchecked((byte)254));
                                            break;
                                    }

                                    TerrainBuffer[((Row * 65) + (Col * 2)) + 1] = unchecked((byte)unchecked((byte)J));
                                }
                            }
                        }
                    }
                }

                {
                    Row = 6;
                    byte end__1264 = 52;
                    for (; Row <= end__1264; Row += 1)
                    {
                        {
                            Col = 6;
                            byte end__1265 = 58;
                            for (; Col <= end__1265; Col += 1)
                            {
                                I = ShapeBuffer[(Row * 65) + Col];
                                if (I > 0)
                                {
                                    J = TerrainBuffer[(Row * 65) + Col];
                                    if (J == 17)
                                    {
                                        var call__1266 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__1267 = call__1266.Field[Col, Row];
                                        call__1267.TerrainType = 7;
                                    }
                                    else
                                    {
                                        var call__1268 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__1269 = call__1268.Field[Col, Row];
                                        call__1269.TerrainType = unchecked((sbyte)FTerrain[I - 1]);
                                        if ((J > 0) && (J < 19))
                                        {
                                            if (FResources[(FTerrain[I - 1] * 19) + J].Count > 0)
                                            {
                                                var call__1270 = Aow.Rtl.Rtl.Random(FResources[(FTerrain[I - 1] * 19) + J].Count);
                                                var call__1271 = FResources[(FTerrain[I - 1] * 19) + J][call__1270];
                                                ((Aow.Units.HSEngine.TMapObjectResource)call__1271).PlaceResourceUser(Aow.Units.AoWTC.AoWTC.AoWCombatMap, unchecked((sbyte)Col), unchecked((sbyte)Row), 0);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                {
                    Row = 6;
                    byte end__1272 = 52;
                    for (; Row <= end__1272; Row += 1)
                    {
                        {
                            Col = 6;
                            byte end__1273 = 58;
                            for (; Col <= end__1273; Col += 1)
                            {
                                var call__1274 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__1275 = call__1274.Field[Col, Row];
                                call__1275.TerrainChanged();
                            }
                        }
                    }
                }

                ShapeBuffer = default;
                TerrainBuffer = default;
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.UnlockMapChangedEvent();
            }

            Result = Aow.Units.AoWTC.AoWTC.AoWCombatMap;
            return Result;
        }

        // AoWTC.pas:4519
        public void PlaceCombatUnits()
        {
            int Position = default;
            int I = default;
            int J = default;
            int Temp = default;
            Aow.Rtl.TList Units = default;
            Aow.Units.AoWTC.TTacticalCombatUnit U = default;
            Aow.Units.AoWTC.TTacticalCombatUnit Other = default;
            int Direction = default;
            Aow.Units.FixedArray_bool_8 Used = default;
            Aow.Units.AoWTC.TCItemHS ItemHS = default;
            Direction = 32;
            FHomePosition = -1;
            Units = new Aow.Rtl.TList();
            {
                Position = 0;
                byte end__1276 = 7;
                for (; Position <= end__1276; Position += 1)
                {
                    Units.Clear();
                    {
                        I = 0;
                        int end__1277 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                        for (; I <= end__1277; I += 1)
                        {
                            var call__1278 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            if (call__1278 is Aow.Units.AoWE.TCombatUnit)
                            {
                                var call__1279 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                var call__1280 = call__1279.GetEnabled();
                                if (call__1280)
                                {
                                    var call__1281 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                    if (((int)(((uint)call__1281.FPosition) >> 4)) == Position)
                                    {
                                        var call__1282 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                        Units.Add(call__1282);
                                        var call__1283 = Units[Units.Count - 1];
                                        U = (Aow.Units.AoWTC.TTacticalCombatUnit)call__1283;
                                        U.FPositionIndex = unchecked((byte)(U.FPosition & 15));
                                        U.FPositionGroup = unchecked((byte)((int)(((uint)U.FPosition) >> 4)));
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
                                            if (U.GetSide() == Aow.Units.AoWE.TCombatSideIndex.csDefender)
                                            {
                                                var call__1284 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL];
                                                var call__1285 = call__1284.Field[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY];
                                                var call__1286 = call__1285.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                                                if (call__1286 != null)
                                                {
                                                    ;
                                                }
                                            }
                                        }

                                        if ((FHomePosition > 0) && (U.FPositionGroup == FHomePosition))
                                        {
                                            U.FPositionGroup = 0;
                                        }

                                        var call__1287 = U.GetSide();
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPositionStatus[U.FPositionGroup] = unchecked((byte)unchecked((byte)call__1287));
                                    }
                                }
                            }
                        }
                    }

                    {
                        I = 0;
                        int end__1288 = Units.Count - 1;
                        for (; I <= end__1288; I += 1)
                        {
                            var call__1289 = Units[I];
                            U = (Aow.Units.AoWTC.TTacticalCombatUnit)call__1289;
                            {
                                J = I + 1;
                                int end__1290 = Units.Count - 1;
                                for (; J <= end__1290; J += 1)
                                {
                                    var call__1291 = Units[J];
                                    Other = (Aow.Units.AoWTC.TTacticalCombatUnit)call__1291;
                                    var call__1292 = U.GetPosValue();
                                    var call__1293 = Other.GetPosValue();
                                    if (call__1292 < call__1293)
                                    {
                                        Temp = U.FPositionIndex;
                                        U.FPositionIndex = unchecked((byte)Other.FPositionIndex);
                                        Other.FPositionIndex = unchecked((byte)Temp);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPositionStatus[0] != 1)
            {
                var call__1294 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL];
                var call__1295 = call__1294.Field[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY];
                var call__1296 = call__1295.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                if ((call__1296 != null) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallType != 0))
                {
                    Position = 1;
                    while (Position < 7)
                    {
                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPositionStatus[Position] == 1)
                        {
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY, Position, ref J, ref Temp);
                            var call__1297 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL];
                            var call__1298 = call__1297.Field[J, Temp];
                            var call__1299 = call__1298.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                            if (call__1299 != null)
                            {
                                FHomePosition = Position;
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPositionStatus[Position] = unchecked((byte)((int)Aow.Units.AoWE.TCombatSideIndex.csNeutral));
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPositionStatus[0] = 1;
                                Position = 7;
                            }
                        }

                        Position += 1;
                    }
                }
            }

            {
                Position = 0;
                int end__1300 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csAttacker].GetCount() - 1;
                for (; Position <= end__1300; Position += 1)
                {
                    var call__1301 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csAttacker].GetObjects(Position);
                    var call__1302 = call__1301.GetEnabled();
                    if (call__1302)
                    {
                        SetupUnit(Aow.Units.AoWE.TCombatSideIndex.csAttacker, Position, ref Direction);
                    }
                }
            }

            {
                Position = 0;
                int end__1303 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].GetCount() - 1;
                for (; Position <= end__1303; Position += 1)
                {
                    var call__1304 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].GetObjects(Position);
                    if (call__1304 is Aow.Units.AoWE.TCombatUnit)
                    {
                        var call__1305 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].GetObjects(Position);
                        var call__1306 = call__1305.GetEnabled();
                        if (call__1306)
                        {
                            SetupUnit(Aow.Units.AoWE.TCombatSideIndex.csDefender, Position, ref Direction);
                        }
                    }
                }
            }

            Units.Free();
            if (FItems != null)
            {
                {
                    Position = 0;
                    byte end__1307 = 7;
                    for (; Position <= end__1307; Position += 1)
                    {
                        Used[Position] = false;
                    }
                }

                Position = 0;
                while (true)
                {
                    var call__1308 = FItems.GetCount();
                    if (!(Position < call__1308))
                        break;
                    {
                        var call__1309 = Aow.Rtl.Rtl.Random(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.FPositions.Count - 16);
                        I = call__1309;
                        J = I - 1;
                        if (J < 0)
                        {
                            J = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.FPositions.Count - 17;
                        }

                        // { Original indexes its eight-slot occupancy array with the position-list index. }
                        while (Used[I] && (I != J))
                        {
                            I += 1;
                            if (I > (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.FPositions.Count - 17))
                            {
                                I = 0;
                            }
                        }

                        Used[I] = true;
                        var call__1310 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(2, I);
                        J = call__1310.X;
                        var call__1311 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(2, I);
                        Temp = call__1311.Y;
                        ItemHS = Aow.Units.AoWTC.TCItemHS.Create(null);
                        var call__1312 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(ItemHS, J, Temp, 0);
                        if (call__1312)
                        {
                            var call__1313 = FItems.GetItem(Position);
                            call__1313.SetOwner(ItemHS.FItems);
                        }
                        else
                        {
                            Position += 1;
                        }
                    }
                }
            }
        }

        // AoWTC.pas:4610
        public void SetupUnit(Aow.Units.AoWE.TCombatSideIndex Side, int Index, ref int Direction)
        {
            Aow.Units.AoWTC.TTacticalCombatUnitHS UnitHS = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS TargetHS = default;
            int X = default;
            int Y = default;
            int Position = default;
            int Slot = default;
            int HN = default;
            int NX = default;
            int NY = default;
            UnitHS = Aow.Units.AoWTC.TTacticalCombatUnitHS.Create(null);
            var call__1314 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Side].GetObjects(Index);
            UnitHS.Initialize((Aow.Units.AoWTC.TTacticalCombatUnit)call__1314);
            if (UnitHS.FUnit.FPositionGroup == FHomePosition)
            {
                UnitHS.FUnit.FPositionGroup = 0;
            }

            Position = UnitHS.FUnit.FPositionGroup;
            Slot = UnitHS.FUnit.FPositionIndex;
            var call__1315 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(Position, Slot);
            X = call__1315.X;
            var call__1316 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(Position, Slot);
            Y = call__1316.Y;
            if ((Side == Aow.Units.AoWE.TCombatSideIndex.csDefender) && FRandomTerrain)
            {
                switch (Position)
                {
                    case 0:
                        if (Direction > 0)
                        {
                            X = Aow.Units.AoWTC.AoWTC.DefPosX[((Direction - 1) * 8) + Slot];
                            Y = Aow.Units.AoWTC.AoWTC.DefPosY[((Direction - 1) * 8) + Slot];
                            UnitHS.SetDirection(unchecked((byte)Direction));
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
                var call__1317 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(Position, Slot);
                UnitHS.SetDirection(unchecked((byte)call__1317.Flag));
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
                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, HN, ref NX, ref NY);
                var call__1318 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__1319 = call__1318.Field[NX, NY];
                Slot = call__1319.OverlayType;
                var call__1320 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__1321 = call__1320.Field[NX, NY];
                var call__1322 = call__1321.FindOwnedChild(2228484);
                TargetHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1322;
                HN += 1;
            }
            while (!((Slot != 8) && (TargetHS == null)));
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(UnitHS, NX, NY, 0);
            var call__1323 = Aow.Units.HSEngine.HSEngine.dHXtoHN(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FAttackerX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FAttackerY);
            bool bool__1324 = (call__1323 == Position);
            if (bool__1324)
            {
                var call__1325 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)UnitHS.FUnit.FCombatPlayer));
                bool__1324 = (call__1325.FLastX == 0);
            }

            if (bool__1324)
            {
                var call__1326 = UnitHS.GetXhx();
                var call__1327 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)UnitHS.FUnit.FCombatPlayer));
                call__1327.FLastX = call__1326;
                var call__1328 = UnitHS.GetYhx();
                var call__1329 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)UnitHS.FUnit.FCombatPlayer));
                call__1329.FLastY = call__1328;
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

        // AoWTC.pas:544
        public static Aow.Units.AoWTC.TRndTacHsm Create()
        {
            Aow.Units.AoWTC.TRndTacHsm o = new Aow.Units.AoWTC.TRndTacHsm();
            try
            {
                o.Create__init_TRndTacHsm();
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
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

    public static partial class AoWTC
    {
        public static byte TCombatUnitSelectionControl_CalculateMovePath__ClimbFlag = 1;
        // AoWTC.pas:6436
        public static void MakeAbilPath(Aow.Units.AoWE.TCombatSideIndex Side, short Ability, short Spell, short X1, short Y1, short X2, short Y2, ref Aow.Units.aowFX.TXYZList Path, ref Aow.Rtl.TList Obstacles)
        {
            int X = default;
            int Y = default;
            switch (Ability)
            {
                case 55:
                    Aow.Units.AoWTC.AoWTC.MakeRangedPath(Side, 25, 0, 50, unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), 0, 0, true, ref Path, ref Obstacles);
                    break;
                case 22:
                case 24:
                case 25:
                    Aow.Units.AoWTC.AoWTC.MakeRangedPath(Side, 50, 100, 50, unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), 0, 0, true, ref Path, ref Obstacles);
                    break;
                case 44:
                case 45:
                case 57:
                case 58:
                case 123:
                    Aow.Units.AoWTC.AoWTC.MakeRangedPath(Side, 50, 20, 50, unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), 0, 0, true, ref Path, ref Obstacles);
                    break;
                case 30:
                case 31:
                case >= 86 and <= 90:
                    var call__1330 = Aow.Units.aowFX.TXYZ.Create();
                    Path.Add(call__1330);
                    Aow.Units.HSEngine.HSEngine.HXtoHP(X2, Y2, ref X, ref Y);
                    var call__1331 = Path[0];
                    call__1331.X = X + 12;
                    var call__1332 = Path[0];
                    call__1332.Y = Y + 12;
                    Obstacles.Clear();
                    break;
                case 52:
                    switch (Spell)
                    {
                        case 103:
                            Aow.Units.AoWTC.AoWTC.MakeRangedPath(Side, 25, 0, 50, unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), 0, 0, true, ref Path, ref Obstacles);
                            break;
                        case 107:
                            Aow.Units.AoWTC.AoWTC.MakeRangedPath(Side, 25, 0, 50, unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), 0, 0, true, ref Path, ref Obstacles);
                            Obstacles.Clear();
                            break;
                        case 117:
                            Aow.Units.AoWTC.AoWTC.MakeRangedPath(Side, 50, 50, 50, unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), 0, 0, true, ref Path, ref Obstacles);
                            break;
                        case 100:
                            Aow.Units.AoWTC.AoWTC.MakeRangedPath(Side, 50, 35, 50, unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), 0, 0, true, ref Path, ref Obstacles);
                            break;
                        case 119:
                            Aow.Units.AoWTC.AoWTC.MakeRangedPath(Side, 50, 20, 50, unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), 0, 0, true, ref Path, ref Obstacles);
                            break;
                        default:
                            switch (Aow.Units.AoWTC.AoWTC.SpellTypes[Spell])
                            {
                                case Aow.Units.AoWTC.TfxType.fxBreath:
                                case Aow.Units.AoWTC.TfxType.fxEffect:
                                case Aow.Units.AoWTC.TfxType.fxScreenNS:
                                case Aow.Units.AoWTC.TfxType.fxScreenSwarm:
                                case Aow.Units.AoWTC.TfxType.fxGeyser:
                                case Aow.Units.AoWTC.TfxType.fxTremors:
                                case Aow.Units.AoWTC.TfxType.fxCloud:
                                case Aow.Units.AoWTC.TfxType.fxResurrect:
                                case Aow.Units.AoWTC.TfxType.fxFrostBeam:
                                case Aow.Units.AoWTC.TfxType.fxMindDecay:
                                case Aow.Units.AoWTC.TfxType.fxShockWave:
                                case Aow.Units.AoWTC.TfxType.fxSacFlame:
                                case Aow.Units.AoWTC.TfxType.fxWinds:
                                    var call__1333 = Aow.Units.aowFX.TXYZ.Create();
                                    Path.Add(call__1333);
                                    Aow.Units.HSEngine.HSEngine.HXtoHP(X2, Y2, ref X, ref Y);
                                    var call__1334 = Path[0];
                                    call__1334.X = X + 12;
                                    var call__1335 = Path[0];
                                    call__1335.Y = Y + 12;
                                    Obstacles.Clear();
                                    break;
                                default:
                                    Aow.Units.AoWTC.AoWTC.MakeRangedPath(Side, 50, 50, 50, unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), 0, 0, true, ref Path, ref Obstacles);
                                    break;
                            }

                            break;
                    }

                    break;
                default:
                    Aow.Units.AoWTC.AoWTC.MakeRangedPath(Side, 50, 50, 50, unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), 0, 0, true, ref Path, ref Obstacles);
                    break;
            }
        }

        // AoWTC.pas:6479
        public static void MakeRangedPath(Aow.Units.AoWE.TCombatSideIndex Side, short Speed, short Arc, short UnusedHeight, short X1, short Y1, short X2, short Y2, short OffsetX, short OffsetY, bool IncludeUnits, ref Aow.Units.aowFX.TXYZList Path, ref Aow.Rtl.TList Obstacles)
        {
            int I = default;
            int A = default;
            int B = default;
            int NX = default;
            int NY = default;
            int Steps = default;
            int X = default;
            int Y = default;
            int LastX = default;
            int LastY = default;
            int StartX = default;
            int StartY = default;
            int EndX = default;
            int EndY = default;
            Aow.Units.aowFX.TXYZ Point = default;
            float Height = default;
            Aow.Units.AoWTC.TROb Obstacle = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS UnitHS = default;
            Aow.Units.CityWall.TCityWall Wall = default;
            Path.Clear();
            Obstacles.Clear();
            Aow.Units.HSEngine.HSEngine.HXtoHP(X1, Y1, ref X, ref Y);
            Aow.Units.HSEngine.HSEngine.HXtoHP(X2, Y2, ref LastX, ref LastY);
            X += 13;
            Y += 13;
            LastX = (LastX + 13) + OffsetX;
            LastY = (LastY + 13) + OffsetY;
            I = 0;
            A = System.Math.Abs(X2 - X1);
            B = System.Math.Abs(Y2 - Y1);
            Steps = ((A + B) * 100) / Speed;
            if (Steps < 2)
            {
                Steps = 2;
            }

            while (I <= Steps)
            {
                Point = Aow.Units.aowFX.TXYZ.Create();
                Path.Add(Point);
                Point.X = (((LastX - X) * I) / Steps) + X;
                Point.Y = (((LastY - Y) * I) / Steps) + Y;
                B = Steps / 2;
                if (I > B)
                {
                    A = (Steps - I) + 1;
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
                    Height = ((A * Steps) * Arc) / 10;
                    var call__1336 = System.Math.Sqrt(Height);
                    Point.Z = Aow.Rtl.Rtl.Round(call__1336) - ((Steps * 10) / Arc);
                }

                I += 1;
            }

            LastX = 0;
            LastY = 0;
            var call__1337 = Path[0];
            Point = call__1337;
            Aow.Units.HSEngine.HSEngine.HPtoHX(Point.X, Point.Y, ref StartX, ref StartY);
            var call__1338 = Path.Count;
            var call__1339 = Path[call__1338 - 1];
            Point = call__1339;
            Aow.Units.HSEngine.HSEngine.HPtoHX(Point.X, Point.Y, ref EndX, ref EndY);
            {
                I = 0;
                int end__1340 = Path.Count - 1;
                for (; I <= end__1340; I += 1)
                {
                    var call__1341 = Path[I];
                    Point = call__1341;
                    Aow.Units.HSEngine.HSEngine.HPtoHX(Point.X, Point.Y, ref X, ref Y);
                    if (((X != StartX) || (Y != StartY)) && ((X != EndX) || (Y != EndY)))
                    {
                        Steps = 0;
                        var call__1342 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__1343 = call__1342.Field[X, Y];
                        if (call__1343.TerrainType == 15)
                        {
                            Steps = 3;
                        }
                        else
                        {
                            var call__1344 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1345 = call__1344.Field[X, Y];
                            if (call__1345.TerrainType == 7)
                            {
                                Steps = 3;
                            }
                            else
                            {
                                A = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, StartX, StartY);
                                if (!IncludeUnits)
                                {
                                    UnitHS = null;
                                }
                                else
                                {
                                    var call__1346 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__1347 = call__1346.Field[X, Y];
                                    var call__1348 = call__1347.FindOwnedChild(2228484);
                                    UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1348;
                                }

                                var call__1349 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__1350 = call__1349.Field[X, Y];
                                var call__1351 = call__1350.FindOwnedChild(2229508);
                                Wall = (Aow.Units.CityWall.TCityWall)call__1351;
                                if ((Wall == null) && (Side == Aow.Units.AoWE.TCombatSideIndex.csAttacker))
                                {
                                    var call__1352 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__1353 = call__1352.Field[X, Y];
                                    var call__1354 = call__1353.FindOwnedChild(2229510);
                                    Wall = (Aow.Units.CityWall.TCityWall)call__1354;
                                }

                                if (A == 1)
                                {
                                    bool bool__1355 = (UnitHS != null);
                                    if (bool__1355)
                                    {
                                        var call__1356 = UnitHS.FUnit.GetSide();
                                        bool__1355 = (call__1356 != Side);
                                    }

                                    if (bool__1355)
                                    {
                                        Steps = 1;
                                    }

                                    if (((Wall != null) && (Wall.FHitPoints > 0)) && (Side == Aow.Units.AoWE.TCombatSideIndex.csAttacker))
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
                                            var call__1357 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                            var call__1358 = call__1357.Field[X, Y];
                                            if (call__1358.OverlayType == 8)
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
                                                    while ((Wall == null) && (B < 7))
                                                    {
                                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(StartX, StartY, B, ref NX, ref NY);
                                                        var call__1359 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                        var call__1360 = call__1359.Field[NX, NY];
                                                        var call__1361 = call__1360.FindOwnedChild(2229508);
                                                        Wall = (Aow.Units.CityWall.TCityWall)call__1361;
                                                        if (Wall == null)
                                                        {
                                                            var call__1362 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                            var call__1363 = call__1362.Field[NX, NY];
                                                            var call__1364 = call__1363.FindOwnedChild(2229510);
                                                            Wall = (Aow.Units.CityWall.TCityWall)call__1364;
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
                                    Aow.Units.HSEngine.HSEngine.HXtoHP(X, Y, ref A, ref B);
                                    A = (Point.X + 11) - A;
                                    if (A > 23)
                                    {
                                        A = 47 - A;
                                    }

                                    B = (Point.Y + 3) - B;
                                    if (B > 15)
                                    {
                                        B = 31 - B;
                                    }

                                    A = ((A + B) - Point.Z) * 2;
                                    break;
                                case 2:
                                    A = 200 - (Point.Z * 3);
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
                                if ((X != LastX) || (Y != LastY))
                                {
                                    Obstacle = Aow.Units.AoWTC.TROb.Create();
                                    Obstacle.PathIndex = I;
                                    Obstacles.Add(Obstacle);
                                    LastX = X;
                                    LastY = Y;
                                    Steps = 1;
                                }
                                else
                                {
                                    var call__1365 = Obstacles[Obstacles.Count - 1];
                                    Obstacle = (Aow.Units.AoWTC.TROb)call__1365;
                                    if ((Steps == 2) && (Obstacle.Chance < A))
                                    {
                                        Obstacle.Chance = A;
                                    }
                                }

                                if ((Steps != 2) && (Steps != 4))
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
        }

        // AoWTC.pas:6591
        // { conflict: procedure MakeRangedPath(Side: TCombatSideIndex; Height, Step, Arc: Smallint; X1, Y1, X2, Y2: Smallint; Ignore: TMapObject; Mode: Boolean; Z: Smallint; var Path: TXYZList; var Obstacles: TList); }
        // { conflict: procedure MakeRangedPath(Side: TCombatSideIndex; Height, Step, Arc: Smallint; X1, Y1, X2, Y2: Smallint; OffsetX, OffsetY: Smallint; Mode: Boolean; var Path: TXYZList; var Obstacles: TList); }
        public static void RegisterAoWTCClasses(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.AoWE.AoWE.AoWEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWTC.TAoWCombatMap>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWTC.TAoWCombatMapLevel>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWTC.TUnitPositionHS>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWTC.TTacticalCombat>()]);
        }

        // AoWTC.pas:6596
        public static ushort GetMeleeDirIndex(int A1, int A2, int A3)
        {
            ushort Result = default;
            Result = 1;
            switch (A3)
            {
                case -1:
                    switch (A2)
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
                    switch (A2)
                    {
                        case -1:
                            if (((A1 / 2) * 2) == A1)
                            {
                                Result = 3;
                            }
                            else
                            {
                                Result = 2;
                            }

                            break;
                        case 1:
                            if (((A1 / 2) * 2) == A1)
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
                    switch (A2)
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
        public static ushort GetRangedDirIndex(int A1, int A2)
        {
            ushort Result = default;
            int Ratio = default;
            if (A1 == 0)
            {
                Result = 0;
            }
            else
            {
                Ratio = System.Math.Abs((A2 * 100) / A1);
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

            if (A2 > 0)
            {
                Result = unchecked((ushort)(12 - Result));
            }

            if ((A1 < 0) && (Result > 0))
            {
                Result = unchecked((ushort)(24 - Result));
            }

            return Result;
        }

        // { conflict: function GetRangedDirIndex(A1: Integer; A2: Integer): Integer; }
        public static Aow.Units.AoWTC.TAoWCombatMap AoWCombatMap;
        public static int CombatXLoc;
        public static int CombatYLoc;
        public static int CombatLLoc;
        // { $46C058..$46C064 }
        public static Aow.Units.FixedArray_int_2 TCAttack;
        public static Aow.Units.FixedArray_int_2 TCDamage;
        // { conflict: const TCAttack: array[0..1] of Integer = (6, 10); }
        // { conflict: const TCDamage: array[0..1] of Integer = (6, 1); }
        public static Aow.Units.FixedArray_int_18 raDir = Aow.Units.FixedArray_int_18.From([0, 0, 0, 1, 0, 2, 3, 4, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0]);
        // { conflict: raDir: array[0..16] of Integer = (0, 0, 0, 1, 0, 2, 3, 4, 5, 6, 0, 0, 0, 0, 0, 0, 0); }
        public static Aow.Units.FixedArray_Aow_Units_AoWTC_TfxType_170 AbilTypes = Aow.Units.FixedArray_Aow_Units_AoWTC_TfxType_170.From([Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxArrow, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxRock, Aow.Units.AoWTC.TfxType.fxRock, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxBreath, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxRock, Aow.Units.AoWTC.TfxType.fxRock, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxRay, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxArrow, Aow.Units.AoWTC.TfxType.fxArrow, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxArrow, Aow.Units.AoWTC.TfxType.fxComet, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxBreath, Aow.Units.AoWTC.TfxType.fxBreath, Aow.Units.AoWTC.TfxType.fxBreath, Aow.Units.AoWTC.TfxType.fxBreath, Aow.Units.AoWTC.TfxType.fxBreath, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxComet, Aow.Units.AoWTC.TfxType.fxComet, Aow.Units.AoWTC.TfxType.fxComet, Aow.Units.AoWTC.TfxType.fxComet, Aow.Units.AoWTC.TfxType.fxComet, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxTouch, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone]);
        public static Aow.Units.FixedArray_Aow_Units_AoWTC_TfxType_131 SpellTypes = Aow.Units.FixedArray_Aow_Units_AoWTC_TfxType_131.From([Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxCometBall, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxHighPrayer, Aow.Units.AoWTC.TfxType.fxRay, Aow.Units.AoWTC.TfxType.fxCloud, Aow.Units.AoWTC.TfxType.fxScreenNESW, Aow.Units.AoWTC.TfxType.fxMindDecay, Aow.Units.AoWTC.TfxType.fxChain, Aow.Units.AoWTC.TfxType.fxBreath, Aow.Units.AoWTC.TfxType.fxNone, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxTremors, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxBreath, Aow.Units.AoWTC.TfxType.fxScreenSwarm, Aow.Units.AoWTC.TfxType.fxScreenNS, Aow.Units.AoWTC.TfxType.fxRock, Aow.Units.AoWTC.TfxType.fxGeyser, Aow.Units.AoWTC.TfxType.fxAltArrow, Aow.Units.AoWTC.TfxType.fxFrostBeam, Aow.Units.AoWTC.TfxType.fxEffect, Aow.Units.AoWTC.TfxType.fxRock, Aow.Units.AoWTC.TfxType.fxCometBall, Aow.Units.AoWTC.TfxType.fxScreenNS, Aow.Units.AoWTC.TfxType.fxSacredWrath, Aow.Units.AoWTC.TfxType.fxWinds, Aow.Units.AoWTC.TfxType.fxShockWave, Aow.Units.AoWTC.TfxType.fxSacFlame, Aow.Units.AoWTC.TfxType.fxResurrect, Aow.Units.AoWTC.TfxType.fxResurrect]);
        public static Aow.Units.FixedArray_int_72 BreathHN = Aow.Units.FixedArray_int_72.From([1, 18, 7, 8, 36, 19, 20, 59, 60, 37, 38, 39, 2, 8, 9, 10, 21, 22, 23, 39, 40, 41, 42, 43, 3, 10, 11, 12, 24, 25, 26, 43, 44, 45, 46, 47, 4, 12, 13, 14, 27, 28, 29, 47, 48, 49, 50, 51, 5, 14, 15, 16, 30, 31, 32, 51, 52, 53, 54, 55, 6, 16, 17, 18, 33, 34, 35, 55, 56, 57, 58, 59]);
        public static Aow.Units.FixedArray_int_60 BreathDirection = Aow.Units.FixedArray_int_60.From([1, 2, 3, 4, 5, 6, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 1]);
        public static Aow.Units.FixedArray_int_14 StatusAbils = Aow.Units.FixedArray_int_14.From([149, 150, 48, 92, 93, 94, 95, 96, 97, 98, 107, 163, 127, 34]);
        public static Aow.Units.FixedArray_int_2 WallISIndex = Aow.Units.FixedArray_int_2.From([30, 0]);
        public static Aow.Units.FixedArray_int_19 rndHex = Aow.Units.FixedArray_int_19.From([6, 0, 1, 3, 4, 5, 2, 18, 12, 7, 11, 10, 17, 15, 9, 16, 13, 8, 14]);
        public static Aow.Units.FixedArray_int_12 BreathHit = Aow.Units.FixedArray_int_12.From([0, 1, 4, 7, 8, 2, 5, 9, 10, 3, 6, 11]);
        public static Aow.Units.FixedArray_int_2 WallMaxHP = Aow.Units.FixedArray_int_2.From([7, 13]);
        public static readonly Aow.Rtl.AnsiString IndRStr = (AnsiString)"Independents";
        public static readonly Aow.Rtl.AnsiString BeginRoundRStr = (AnsiString)"begin their round.";
        public static Aow.Units.FixedArray_int_48 DefPosX = Aow.Units.FixedArray_int_48.From([32, 30, 34, 32, 28, 36, 30, 34, 34, 36, 32, 38, 30, 36, 32, 34, 36, 34, 38, 32, 36, 30, 34, 32, 32, 34, 30, 32, 36, 28, 34, 30, 30, 28, 32, 26, 34, 28, 32, 30, 28, 30, 26, 32, 28, 34, 30, 32]);
        public static Aow.Units.FixedArray_int_48 DefPosY = Aow.Units.FixedArray_int_48.From([26, 27, 27, 28, 28, 28, 29, 29, 27, 28, 26, 29, 27, 30, 28, 29, 30, 31, 29, 32, 28, 31, 29, 30, 32, 31, 31, 30, 30, 30, 29, 29, 31, 30, 32, 29, 31, 28, 30, 29, 28, 27, 29, 26, 30, 27, 29, 28]);
        public static Aow.Units.FixedArray_byte_3770 MapShape = Aow.Units.FixedArray_byte_3770.From([0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 10, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 11, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 10, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 11, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 10, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 11, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 10, 10, 2, 14, 2, 14, 2, 14, 2, 14, 2, 11, 11, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 19, 14, 1, 14, 1, 14, 1, 14, 1, 14, 15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 19, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 19, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 19, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 19, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 7, 7, 19, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 15, 3, 3, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 7, 19, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 15, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 8, 8, 8, 8, 8, 19, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 15, 9, 9, 9, 9, 9, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 18, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 16, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 18, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 16, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 18, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 16, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 18, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 16, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 18, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 16, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 18, 18, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 16, 16, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 18, 17, 1, 1, 1, 1, 1, 1, 1, 1, 1, 17, 16, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 12, 5, 17, 17, 17, 17, 17, 17, 17, 17, 17, 5, 13, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 12, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 13, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 6, 6, 12, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 13, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 6, 6, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 12, 12, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 13, 13, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 5, 5, 5, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
        public static Aow.Units.FixedArray_byte_3770 MapRoads = Aow.Units.FixedArray_byte_3770.From([0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 26, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22, 22, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 0, 0, 21, 2, 2, 2, 21, 0, 0, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 0, 0, 21, 2, 2, 2, 21, 0, 0, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 26, 26, 21, 2, 2, 2, 21, 22, 22, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 7, 21, 2, 2, 2, 21, 3, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 7, 7, 7, 1, 1, 1, 3, 3, 3, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 26, 26, 7, 7, 1, 1, 1, 1, 1, 3, 3, 22, 22, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 1, 1, 1, 1, 1, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 1, 1, 1, 1, 1, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 5, 1, 5, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 24, 5, 5, 5, 24, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 24, 5, 5, 5, 24, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 25, 6, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 4, 23, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 6, 6, 6, 6, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 23, 23, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 25, 25, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 23, 23, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 24, 5, 5, 5, 24, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
        public static Aow.Units.FixedArray_int_18 clusterChk = Aow.Units.FixedArray_int_18.From([1, 2, 6, 16, 7, 8, 9, 17, 18, 19, 20, 21, 22, 23, 33, 34, 35, 36]);
        // AoWTC.pas:3664
        public static void OriginalUnitAssert(Aow.Rtl.AnsiString Message, Aow.Rtl.AnsiString Filename, int LineNumber)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
