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
            int Result = default;
            int I = default;
            int J = default;
            int X = default;
            int Y = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS HS = default;
            Aow.Units.CityWall.TCityWall Wall = default;
            bool NewWall = default;
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
                    {
                        I = 0;
                        int end__47 = Path.Count - 1;
                        for (; I <= end__47; I += 1)
                        {
                            var call__48 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__49 = call__48.Field[Path.List[I].X, Path.List[I].Y];
                            var call__50 = call__49.FindOwnedChild(2228484);
                            HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__50;
                            bool bool__51 = FreeStrikes;
                            if (bool__51)
                            {
                                bool bool__52 = (HS == null);
                                if (!bool__52)
                                {
                                    bool bool__53 = (HS == U.FUnitHS) && ((I - 1) >= 0);
                                    if (bool__53)
                                    {
                                        var call__54 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__55 = call__54.Field[Path.List[I - 1].X, Path.List[I - 1].Y];
                                        var call__56 = call__55.FindOwnedChild(2228484);
                                        bool__53 = (call__56 == null);
                                    }

                                    bool__52 = bool__53;
                                }

                                bool__51 = bool__52;
                            }

                            if (bool__51)
                            {
                                {
                                    J = 1;
                                    byte end__57 = 6;
                                    for (; J <= end__57; J += 1)
                                    {
                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Path.List[I].X, Path.List[I].Y, J, ref X, ref Y);
                                        var call__58 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__59 = call__58.Field[X, Y];
                                        var call__60 = call__59.FindOwnedChild(2228484);
                                        HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__60;
                                        bool bool__61 = (HS != null) && (HS.FUnit != Ignore);
                                        if (bool__61)
                                        {
                                            var call__62 = HS.FUnit.GetSide();
                                            var call__63 = U.GetSide();
                                            bool__61 = (call__62 != call__63);
                                        }

                                        bool bool__64 = bool__61;
                                        if (bool__64)
                                        {
                                            var call__65 = Aow.Units.AoWE.AoWE.MeleeDamageTypes(HS.FUnit);
                                            bool__64 = (call__65 != TSet<Aow.Units.AoWE.TDamageType>.Of());
                                        }

                                        bool bool__66 = bool__64;
                                        if (bool__66)
                                        {
                                            var call__67 = HS.FUnit.GetLocked();
                                            bool__66 = !call__67;
                                        }

                                        bool bool__68 = bool__66;
                                        if (bool__68)
                                        {
                                            var call__69 = HS.FUnit.GetAbilityEnabled(1);
                                            var call__70 = U.GetAbilityEnabled(1);
                                            bool__68 = (call__69 == call__70);
                                        }

                                        if (bool__68)
                                        {
                                            var call__71 = Aow.Units.AoWE.AoWE.StrikeDV(HS.FUnit, U);
                                            var call__72 = tcDVtoDEV(call__71, 0, HS.FUnit, U);
                                            Value = Value - (call__72 * 10);
                                        }
                                    }
                                }
                            }

                            if (I < (Path.Count - 1))
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallType != 0)
                                {
                                    var call__73 = Path[I];
                                    var call__74 = Path[I];
                                    var call__75 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__76 = call__75.Field[call__74.X, call__73.Y];
                                    var call__77 = call__76.FindOwnedChild(2229508);
                                    Wall = (Aow.Units.CityWall.TCityWall)call__77;
                                    if (Wall == null)
                                    {
                                        var call__78 = Path[I];
                                        var call__79 = Path[I];
                                        var call__80 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__81 = call__80.Field[call__79.X, call__78.Y];
                                        var call__82 = call__81.FindOwnedChild(2229510);
                                        Wall = (Aow.Units.CityWall.TCityWall)call__82;
                                    }
                                }
                                else
                                {
                                    Wall = null;
                                }

                                var call__83 = Path[I];
                                var call__84 = Path[I];
                                var call__85 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__86 = call__85.Field[call__84.X, call__83.Y];
                                var call__87 = Path[I];
                                var call__88 = Path[I];
                                var call__89 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__90 = call__89.Field[call__88.X, call__87.Y];
                                Path.List[I].Flag = unchecked((sbyte)MCS.FMovePointTable[call__90.TerrainType][call__86.OverlayType - -1]);
                                if (Wall != null)
                                {
                                    // { The wall lookup uses the current path point and the no-overlay column. }
                                    bool bool__91 = (FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker);
                                    if (bool__91)
                                    {
                                        var call__92 = U.GetAbilityEnabled(65);
                                        bool bool__93 = call__92;
                                        if (!bool__93)
                                        {
                                            var call__94 = U.GetAbilityEnabled(117);
                                            bool__93 = call__94;
                                        }

                                        bool__91 = bool__93;
                                    }

                                    if (bool__91 && (I == 0))
                                    {
                                        var call__95 = Path[I];
                                        var call__96 = Path[I];
                                        var call__97 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__98 = call__97.Field[call__96.X, call__95.Y];
                                        Path.List[I].Flag = unchecked((sbyte)MCS.FMovePointTable[call__98.TerrainType][-1 - -1]);
                                    }
                                    else
                                    {
                                        bool bool__99 = (((FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker) || Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallBreached) || FIgnoreWalls);
                                        if (bool__99)
                                        {
                                            bool bool__100 = (Wall.FHitPoints == 0) || ((FSide == Aow.Units.AoWE.TCombatSideIndex.csDefender) && (Wall is Aow.Units.CityWall.TCityWallDoor));
                                            if (!bool__100)
                                            {
                                                var call__101 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.FUnitHS.FUnit.GetAbilityEnabled(1);
                                                bool__100 = call__101;
                                            }

                                            bool bool__102 = bool__100;
                                            if (!bool__102)
                                            {
                                                var call__103 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.FUnitHS.FUnit.GetAbilityEnabled(66);
                                                bool__102 = call__103;
                                            }

                                            bool bool__104 = bool__102;
                                            if (!bool__104)
                                            {
                                                var call__105 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.FUnitHS.FUnit.GetAbilityEnabled(36);
                                                bool__104 = call__105;
                                            }

                                            bool__99 = bool__104;
                                        }

                                        if (bool__99)
                                        {
                                            var call__106 = Path[I];
                                            var call__107 = Path[I];
                                            var call__108 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                            var call__109 = call__108.Field[call__107.X, call__106.Y];
                                            Path.List[I].Flag = unchecked((sbyte)MCS.FMovePointTable[call__109.TerrainType][-1 - -1]);
                                        }
                                        else
                                        {
                                            Path.List[I].Flag = 100;
                                        }
                                    }

                                    if (FindWalls && (Wall.FHitPoints > 0))
                                    {
                                        NewWall = true;
                                        J = 0;
                                        while (NewWall && (J < Walls.Count))
                                        {
                                            var call__110 = Walls[J];
                                            if (call__110 == Wall)
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

                                var call__111 = Path[I];
                                var call__112 = Path[I];
                                var call__113 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__114 = call__113.Field[call__112.X, call__111.Y];
                                var call__115 = call__114.FindOwnedChild(2228484);
                                if (call__115 != null)
                                {
                                    var call__116 = Path[I];
                                    var call__117 = Path[I];
                                    var call__118 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__119 = call__118.Field[call__117.X, call__116.Y];
                                    Path.List[I].Flag = unchecked((sbyte)MCS.FMovePointTable[call__119.TerrainType][-1 - -1]);
                                }
                            }

                            Result = Result + Path.List[I].Flag;
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                var call__46 = U.FUnit.GetName();
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Eval Movepath", AnsiString.FromPChar(Ptr<byte>.Unbox(((AnsiString)"Error - TCAI ") + call__46)), (int)0);
            }

            return Result;
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
                int end__120 = List.Count - 1;
                for (; I <= end__120; I += 1)
                {
                    var call__121 = List[I];
                    if (((Aow.Units.AoWE.TCombatObject)call__121) != Ignore)
                    {
                        var call__122 = List[I];
                        Target = (Aow.Units.AoWE.TCombatObject)call__122;
                        var call__123 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Spell);
                        call__123.GetCombatDamageValueEx(U, Target, ref DV);
                        if (DV.SourceDamage > 0)
                        {
                            switch (Spell)
                            {
                                case 102:
                                    var call__124 = Target.GetSide();
                                    if (call__124 != FSide)
                                    {
                                        var call__125 = tcDVtoDEV(DV.SourceDamage, 0, U, Target);
                                        Value = Value - call__125;
                                    }
                                    else
                                    {
                                        var call__126 = tcDVtoDEV(DV.SourceDamage, 0, U, Target);
                                        Value = Value + call__126;
                                    }

                                    break;
                                default:
                                    var call__127 = Target.GetSide();
                                    if (call__127 != FSide)
                                    {
                                        var call__128 = Target.GetHitPoints();
                                        var call__129 = Target.GetHits();
                                        var call__130 = tcDVtoDEV(DV.SourceDamage, 0, U, Target);
                                        var call__131 = MultDEV(call__130, call__129, call__128);
                                        Value = Value + call__131;
                                    }
                                    else
                                    {
                                        var call__132 = Target.GetHitPoints();
                                        var call__133 = Target.GetHits();
                                        var call__134 = tcDVtoDEV(DV.SourceDamage, 0, U, Target);
                                        var call__135 = MultDEV(call__134, call__133, call__132);
                                        Value = Value - call__135;
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
            var call__136 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
            var call__137 = call__136.Field[X, Y];
            var call__138 = call__137.FindOwnedChild(2228484);
            HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__138;
            bool bool__139 = (HS != null) && (HS.FUnit != Ignore);
            if (bool__139)
            {
                bool bool__140 = Friendly;
                if (!bool__140)
                {
                    var call__141 = HS.FUnit.GetSide();
                    var call__142 = U.GetSide();
                    bool__140 = (call__141 != call__142);
                }

                bool__139 = bool__140;
            }

            if (bool__139)
            {
                if (Ability == 52)
                {
                    var call__143 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Spell);
                    call__143.GetCombatDamageValueEx(U, HS.FUnit, ref DV);
                }
                else
                {
                    if (Aow.Units.AoWTC.AoWTC.AbilTypes[Ability] == Aow.Units.AoWTC.TfxType.fxTouch)
                    {
                        var call__144 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                        call__144.tcGetDamageValueEx(U, HS.FUnit, ref DV);
                    }
                    else
                    {
                        var call__145 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                        call__145.tcGetDamageValueEx(U, HS.FUnit, ref DV);
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
                                    var call__146 = HS.FUnit.GetSide();
                                    var call__147 = U.GetSide();
                                    if (call__146 != call__147)
                                    {
                                        var call__148 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                        Value = Value + call__148;
                                    }
                                    else
                                    {
                                        var call__149 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                        Value = Value - call__149;
                                    }

                                    break;
                                default:
                                    var call__150 = HS.FUnit.GetSide();
                                    var call__151 = U.GetSide();
                                    if (call__150 != call__151)
                                    {
                                        var call__152 = HS.FUnit.GetHitPoints();
                                        var call__153 = HS.FUnit.GetHits();
                                        var call__154 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                        var call__155 = MultDEV(call__154, call__153, call__152);
                                        Value = Value + call__155;
                                    }
                                    else
                                    {
                                        var call__156 = HS.FUnit.GetHitPoints();
                                        var call__157 = HS.FUnit.GetHits();
                                        var call__158 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                        var call__159 = MultDEV(call__158, call__157, call__156);
                                        Value = Value - call__159;
                                    }

                                    break;
                            }

                            break;
                        default:
                            var call__160 = HS.FUnit.GetSide();
                            var call__161 = U.GetSide();
                            if (call__160 != call__161)
                            {
                                var call__162 = HS.FUnit.GetHitPoints();
                                var call__163 = HS.FUnit.GetHits();
                                var call__164 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                var call__165 = MultDEV(call__164, call__163, call__162);
                                Value = Value + call__165;
                            }
                            else
                            {
                                var call__166 = HS.FUnit.GetHitPoints();
                                var call__167 = HS.FUnit.GetHits();
                                var call__168 = tcDVtoDEV(DV.SourceDamage, 0, U, HS.FUnit);
                                var call__169 = MultDEV(call__168, call__167, call__166);
                                Value = Value - call__169;
                            }

                            break;
                    }
                }
            }

            if (HitWalls && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallType != 0))
            {
                var call__170 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__171 = call__170.Field[X, Y];
                var call__172 = call__171.FindOwnedChild(2229508);
                Wall = (Aow.Units.CityWall.TCityWall)call__172;
                if (Wall == null)
                {
                    var call__173 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__174 = call__173.Field[X, Y];
                    var call__175 = call__174.FindOwnedChild(2229510);
                    Wall = (Aow.Units.CityWall.TCityWall)call__175;
                }

                if (((!Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallBreached && (IgnoreWall != null)) && (Wall != null)) && (((Ignore == null) || !(Ignore is Aow.Units.AoWE.TCombatWall)) || ((Ignore is Aow.Units.AoWE.TCombatWall) && (IgnoreWall != Wall))))
                {
                    if (Ability == 52)
                    {
                        var call__176 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Spell);
                        call__176.GetCombatDamageValueEx(U, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall, ref DV);
                    }
                    else
                    {
                        if (Aow.Units.AoWTC.AoWTC.AbilTypes[Ability] == Aow.Units.AoWTC.TfxType.fxTouch)
                        {
                            var call__177 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                            call__177.tcGetDamageValueEx(U, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall, ref DV);
                        }
                        else
                        {
                            var call__178 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                            call__178.tcGetDamageValueEx(U, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall, ref DV);
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
                            var call__179 = tcDVtoDEV(DV.SourceDamage, 0, U, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall);
                            var call__180 = MultDEV(call__179, Aow.Units.AoWTC.AoWTC.WallMaxHP[Wall.FWallType - 1], Wall.FHitPoints);
                            Value = Value + call__180;
                        }
                        else
                        {
                            var call__181 = tcDVtoDEV(DV.SourceDamage, 0, U, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall);
                            var call__182 = MultDEV(call__181, Aow.Units.AoWTC.AoWTC.WallMaxHP[Wall.FWallType - 1], Wall.FHitPoints);
                            Value = Value - call__182;
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
                byte end__183 = 6;
                for (; I <= end__183; I += 1)
                {
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref X1, ref Y1);
                    bool bool__184 = IncludeCenter;
                    if (!bool__184)
                    {
                        var call__185 = U.FUnitHS.GetXhx();
                        bool__184 = (call__185 != X1);
                    }

                    bool bool__186 = bool__184;
                    if (!bool__186)
                    {
                        var call__187 = U.FUnitHS.GetYhx();
                        bool__186 = (call__187 != Y1);
                    }

                    if (bool__186)
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
                        var call__188 = U.GetSide();
                        Aow.Units.AoWTC.AoWTC.MakeRangedPath(call__188, 50, 20, 50, unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), 0, 0, false, ref Path, ref Obstacles);
                    }
                    else
                    {
                        var call__189 = U.GetSide();
                        Aow.Units.AoWTC.AoWTC.MakeAbilPath(call__189, unchecked((short)Ability), unchecked((short)Spell), unchecked((short)X1), unchecked((short)Y1), unchecked((short)X2), unchecked((short)Y2), ref Path, ref Obstacles);
                    }

                    Total = 0;
                    Original = Value;
                    Chance = 100;
                {
                    I = 0;
                    int end__190 = Obstacles.Count - 1;
                    for (; I <= end__190; I += 1)
                    {
                        var call__191 = Obstacles[I];
                        var call__192 = Path[((Aow.Units.AoWTC.TROb)call__191).PathIndex];
                        var call__193 = Obstacles[I];
                        var call__194 = Path[((Aow.Units.AoWTC.TROb)call__193).PathIndex];
                        Aow.Units.HSEngine.HSEngine.HPtoHX(call__194.X, call__192.Y, ref X, ref Y);
                        V = 0;
                        EvalHex(Ignore, Ability, Spell, X, Y, ref V, HitWalls, true, U, IgnoreWall);
                        try
                        {
                            var call__196 = Obstacles[I];
                            Total = Total + ((((V * Chance) / 100) * ((Aow.Units.AoWTC.TROb)call__196).Chance) / 100);
                        }
                        catch (System.Exception)
                        {
                            var call__195 = Obstacles[I];
                            Total = Total + (((Aow.Units.AoWTC.TROb)call__195).Chance * (((V / 100) * Chance) / 100));
                        }

                        var call__197 = Obstacles[I];
                        Chance = ((100 - ((Aow.Units.AoWTC.TROb)call__197).Chance) * Chance) / 100;
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
                    int end__198 = Obstacles.Count - 1;
                    for (; I <= end__198; I += 1)
                    {
                        var call__199 = Obstacles[I];
                        ((Aow.Rtl.TObject)call__199).Free();
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
            var call__200 = U.FUnitHS.GetXhx();
            var call__201 = U.FUnitHS.GetYhx();
            var call__202 = EvalPath(ref Value, Ignore, Ability, Spell, call__200, call__201, X2, Y2, false, U, null);
            Result = call__202;
            {
                I = 0;
                byte end__203 = 11;
                for (; I <= end__203; I += 1)
                {
                    V = 0;
                    var call__204 = Aow.Units.HSEngine.HSEngine.dHXtoHN(X1, Y1, X2, Y2);
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X1, Y1, Aow.Units.AoWTC.AoWTC.BreathHN[((Aow.Units.AoWTC.AoWTC.BreathDirection[call__204 - 1] * 12) + I) - 12], ref X, ref Y);
                    EvalHex(Ignore, Ability, Spell, X, Y, ref V, true, true, U, null);
                    if (V != 0)
                    {
                        var call__205 = U.FUnitHS.GetXhx();
                        var call__206 = U.FUnitHS.GetYhx();
                        EvalPath(ref V, Ignore, Ability, Spell, call__205, call__206, X, Y, false, U, null);
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
            int SafeValue = default;
            int I = default;
            int V = default;
            int Distance = default;
            int AbilityIndex = default;
            int Range = default;
            int Cost = default;
            int BestIndex = default;
            int BestValue = default;
            int D = default;
            int NX = default;
            int NY = default;
            int OriginalX = default;
            int OriginalY = default;
            int AX = default;
            int AY = default;
            int PathCost = default;
            int TerrainValue = default;
            int PathValue = default;
            int Ability = default;
            int Spell = default;
            int Value = default;
            Aow.Units.HSEngine.TXYLList Path = default;
            Aow.Units.HSEngine.TXYLList DetourPath = default;
            Aow.Units.HSEngine.TXYLList BasePath = default;
            Aow.Units.AoWE.TMeleeRound Round = default;
            Aow.Units.AoWE.TAbilityCombatInfo AbilityInfo = default;
            Aow.Units.AoWE.TSpellCombatInfo SpellInfo = default;
            Aow.Units.CityWall.TCityWall CityWall = default;
            bool Adjacent = default;
            Aow.Units.AoWE.TAbilityDamageValues Values = default;
            Path = Aow.Units.AoWE.TMovepath.Create();
            BasePath = Aow.Units.AoWE.TMovepath.Create();
            DetourPath = Aow.Units.AoWE.TMovepath.Create();
            TerrainValue = 0;
            OriginalX = X;
            OriginalY = Y;
            var call__207 = U.FUnitHS.GetYhx();
            var call__208 = U.FUnitHS.GetXhx();
            var call__209 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__208, call__207, X, Y);
            Distance = call__209;
            if (U.FMovePoints > 0)
            {
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.CalculatePath(Settings, (Aow.Units.AoWE.TMovepath)BasePath, unchecked((sbyte)X), unchecked((sbyte)Y), 0);
                var call__210 = MakeTerrainPath(Settings, ref BasePath, ref TerrainValue, U, (Aow.Units.AoWTC.TTacticalCombatUnit)Target, true, (Target is Aow.Units.AoWTC.TTacticalCombatUnit) && (FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker), 1);
                PathCost = call__210;
                {
                    I = 0;
                    int end__211 = BasePath.Count - 1;
                    for (; I <= end__211; I += 1)
                    {
                        var call__212 = BasePath[I];
                        Path.Add(call__212);
                    }
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
                    bool bool__213 = (Target is Aow.Units.AoWE.TCombatUnit);
                    if (bool__213)
                    {
                        var call__214 = Target.GetSide();
                        var call__215 = U.GetSide();
                        bool__213 = (call__214 != call__215);
                    }

                    bool bool__216 = bool__213;
                    if (bool__216)
                    {
                        var call__217 = Aow.Units.AoWE.AoWE.MeleeDamageTypes(U);
                        bool__216 = (call__217 != TSet<Aow.Units.AoWE.TDamageType>.Of());
                    }

                    bool bool__218 = bool__216;
                    if (bool__218)
                    {
                        var call__219 = U.GetAbilityEnabled(1);
                        bool bool__220 = call__219;
                        if (!bool__220)
                        {
                            var call__221 = Target.GetAbilityEnabled(1);
                            bool__220 = !call__221;
                        }

                        bool__218 = bool__220;
                    }

                    if (bool__218)
                    {
                        Ability = 0;
                        Spell = 0;
                        Round = Aow.Units.AoWE.TMeleeRound.Create();
                        Round.Calculate(U, Target);
                        if (Round.GetAttackerDV() > 0)
                        {
                            var call__222 = ((Aow.Units.AoWE.TMovepath)Path).MovePointsToPos(U.FMovePoints);
                            Adjacent = call__222 == 0;
                            U.FCanFlee = false;
                            if (Target is Aow.Units.AoWE.TCombatWall)
                            {
                                FFlee = false;
                            }

                            var call__223 = Round.GetDefenderDV();
                            var call__224 = Round.GetAttackerDV();
                            var call__225 = tcDVtoDEV(call__224, call__223, U, Target);
                            var call__226 = MultDEV(call__225, HitPoints, Hits);
                            Value = call__226 + TerrainValue;
                            var call__227 = Target.GetTargetStrength();
                            if ((call__227 <= 0) && (Enemies.Count > 1))
                            {
                                Value -= 10000;
                            }

                            Cost = (PathCost - U.FMovePoints) * 10;
                            if (Cost < 1)
                            {
                                Cost = 1;
                            }

                            if (!(U.FUnit is Aow.Units.AoWE.THero))
                            {
                                SafeValue = 0;
                            }
                            else
                            {
                                I = -1;
                                var call__228 = ((Aow.Units.AoWE.TMovepath)Path).MovePointsToPos(U.FMovePoints);
                                BestIndex = call__228;
                                var call__229 = Path[BestIndex];
                                var call__230 = Path[BestIndex];
                                var call__231 = UnitSafe(U, call__230.X, call__229.Y);
                                SafeValue = call__231;
                                while ((BestIndex < (Path.Count - 1)) && (SafeValue < 0))
                                {
                                    var call__232 = Path[BestIndex];
                                    var call__233 = Path[BestIndex];
                                    var call__234 = UnitSafe(U, call__233.X, call__232.Y);
                                    SafeValue = call__234;
                                    if (SafeValue >= 0)
                                    {
                                        I = BestIndex;
                                    }

                                    BestIndex += 1;
                                }

                                if (I > -1)
                                {
                                    {
                                        V = 0;
                                        int end__235 = I - 1;
                                        for (; V <= end__235; V += 1)
                                        {
                                            Path.Delete(0);
                                        }
                                    }
                                }
                            }

                            if (Path.Count > 1)
                            {
                                if (((SafeValue >= 0) || (FNormalUnits == 0)) && Adjacent)
                                {
                                    if ((((Value > 0) || (FNormalUnits == 0)) || !(U.FUnit is Aow.Units.AoWE.THero)) || (Units.Count <= 1))
                                    {
                                        FHeroMoves += 1;
                                        if (U.FUnit is Aow.Units.AoWE.THero)
                                        {
                                            I = 50;
                                        }
                                        else
                                        {
                                            I = 100;
                                        }

                                        var call__236 = MultDEV(Value, I, 1);
                                        CreateMove(Aow.Units.AoWTC.TaiMoveType.aiMelee, X, Y, call__236, Ability, Spell, U, Target);
                                    }
                                }
                                else
                                {
                                    bool bool__237 = FIgnoreWalls;
                                    if (!bool__237)
                                    {
                                        var call__238 = ChargeFromPos((int)(((uint)U.FPosition) >> 4));
                                        bool__237 = call__238;
                                    }

                                    if (bool__237)
                                    {
                                        if (!((((SafeValue < 0) && (FNormalUnits != 0)) || (U.FMovePoints <= 0)) || ((Path.List[Path.Count - 2].Flag > U.FMovePoints) && (Path.List[Path.Count - 2].Flag <= 15))))
                                        {
                                            FHeroMoves += 1;
                                            var call__239 = U.GetMoves();
                                            var call__240 = MultDEV(Value, call__239, Cost);
                                            var call__241 = Path[0];
                                            var call__242 = Path[0];
                                            CreateMove(Aow.Units.AoWTC.TaiMoveType.aiMove, call__242.X, call__241.Y, call__240, Ability, Spell, U, Target);
                                        }
                                        else
                                        {
                                            var call__243 = U.GetMoves();
                                            var call__244 = MultDEV(Value, call__243, Cost);
                                            CreateMove(Aow.Units.AoWTC.TaiMoveType.aiFake, X, Y, call__244, Ability, Spell, U, Target);
                                        }
                                    }
                                }
                            }
                        }

                        Round.Free();
                    }
                }
                catch (System.Exception)
                {
                    Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Melee Check", (AnsiString)"Error - TCAI ", (int)0);
                }

                {
                    AbilityIndex = 0;
                    int end__245 = Abilities.Count - 1;
                    for (; AbilityIndex <= end__245; AbilityIndex += 1)
                    {
                        Path.Clear();
                        {
                            I = 0;
                            int end__246 = BasePath.Count - 1;
                            for (; I <= end__246; I += 1)
                            {
                                var call__247 = BasePath[I];
                                Path.Add(call__247);
                            }
                        }

                        var call__248 = Abilities[AbilityIndex];
                        Ability = ((Aow.Units.AoWTC.TTCAbility)call__248).Ability;
                        Spell = 0;
                        X = OriginalX;
                        Y = OriginalY;
                        var call__249 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                        call__249.GetCombatInfo(U.FUnit, ref AbilityInfo);
                        switch (Ability)
                        {
                            case 52:
                                try
                                {
                                    if (!(Target is Aow.Units.AoWE.TCombatWall) || !Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallBreached)
                                    {
                                        var call__250 = Abilities[AbilityIndex];
                                        Spell = ((Aow.Units.AoWTC.TTCAbility)call__250).Spell;
                                        switch (Aow.Units.AoWTC.AoWTC.SpellTypes[Spell])
                                        {
                                            case Aow.Units.AoWTC.TfxType.fxArrow:
                                            case Aow.Units.AoWTC.TfxType.fxRock:
                                            case Aow.Units.AoWTC.TfxType.fxComet:
                                            case Aow.Units.AoWTC.TfxType.fxBreath:
                                            case Aow.Units.AoWTC.TfxType.fxCometBall:
                                            case Aow.Units.AoWTC.TfxType.fxEffect:
                                            case Aow.Units.AoWTC.TfxType.fxScreenNESW:
                                            case Aow.Units.AoWTC.TfxType.fxScreenNS:
                                            case Aow.Units.AoWTC.TfxType.fxRay:
                                            case Aow.Units.AoWTC.TfxType.fxChain:
                                            case Aow.Units.AoWTC.TfxType.fxSwarm:
                                            case Aow.Units.AoWTC.TfxType.fxScreenSwarm:
                                            case Aow.Units.AoWTC.TfxType.fxSacredWrath:
                                            case Aow.Units.AoWTC.TfxType.fxSWComet:
                                            case Aow.Units.AoWTC.TfxType.fxDispel:
                                            case Aow.Units.AoWTC.TfxType.fxBlood:
                                            case Aow.Units.AoWTC.TfxType.fxAltArrow:
                                            case Aow.Units.AoWTC.TfxType.fxTremors:
                                            case Aow.Units.AoWTC.TfxType.fxHailImpact:
                                            case Aow.Units.AoWTC.TfxType.fxHighPrayer:
                                            case Aow.Units.AoWTC.TfxType.fxMindDecay:
                                            case Aow.Units.AoWTC.TfxType.fxShockWave:
                                            case Aow.Units.AoWTC.TfxType.fxSacFlame:
                                            case Aow.Units.AoWTC.TfxType.fxWinds:
                                                var call__251 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Spell);
                                                call__251.GetCombatInfo(U.FUnit, ref SpellInfo);
                                                var call__252 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Spell);
                                                call__252.GetCombatDamageValueEx(U, Target, ref Values);
                                                if (Values.SourceDamage > 0)
                                                {
                                                    U.FCanFlee = false;
                                                    if (Target is Aow.Units.AoWE.TCombatWall)
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
                                                            var call__253 = tcDVtoDEV(Values.SourceDamage, 0, U, Target);
                                                            Value = call__253;
                                                            break;
                                                        default:
                                                            var call__254 = tcDVtoDEV(Values.SourceDamage, 0, U, Target);
                                                            var call__255 = MultDEV(call__254, HitPoints, Hits);
                                                            Value = call__255;
                                                            break;
                                                    }

                                                    var call__256 = Target.GetTargetStrength();
                                                    if ((call__256 <= 0) && (Enemies.Count > 1))
                                                    {
                                                        Value -= 10000;
                                                    }

                                                    if (U.FMovePoints > 0)
                                                    {
                                                        var call__257 = ((Aow.Units.AoWE.TMovepath)Path).MovePointsToPos(U.FMovePoints);
                                                        I = call__257;
                                                        var call__258 = Path[I];
                                                        var call__259 = Path[I];
                                                        var call__260 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__259.X, call__258.Y, X, Y);
                                                        D = call__260;
                                                        if ((D <= Range) || (Distance <= Range))
                                                        {
                                                            Cost = Value;
                                                            if (Distance <= Range)
                                                            {
                                                                switch (Spell)
                                                                {
                                                                    case 117:
                                                                        var call__261 = U.FUnitHS.GetXhx();
                                                                        var call__262 = U.FUnitHS.GetYhx();
                                                                        var call__263 = EvalPath(ref Value, Target, Ability, Spell, call__261, call__262, X, Y, true, U, Wall);
                                                                        PathValue = call__263;
                                                                        EvalArea(ref Value, Target, Ability, Spell, X, Y, true, true, true, U, Wall);
                                                                        BestIndex = X;
                                                                        BestValue = Y;
                                                                    {
                                                                        I = 1;
                                                                        byte end__264 = 6;
                                                                        for (; I <= end__264; I += 1)
                                                                        {
                                                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                            var call__265 = U.FUnitHS.GetYhx();
                                                                            var call__266 = U.FUnitHS.GetXhx();
                                                                            var call__267 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__266, call__265, NX, NY);
                                                                            if (call__267 <= Range)
                                                                            {
                                                                                D = Cost;
                                                                                var call__268 = U.FUnitHS.GetXhx();
                                                                                var call__269 = U.FUnitHS.GetYhx();
                                                                                var call__270 = EvalPath(ref D, Target, Ability, Spell, call__268, call__269, NX, NY, true, U, Wall);
                                                                                V = call__270;
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
                                                                        byte end__271 = 36;
                                                                        for (; I <= end__271; I += 1)
                                                                        {
                                                                            var call__272 = U.FUnitHS.GetYhx();
                                                                            var call__273 = U.FUnitHS.GetXhx();
                                                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__273, call__272, I, ref NX, ref NY);
                                                                            EvalHex(null, Ability, Spell, NX, NY, ref Value, true, true, U, Wall);
                                                                        }
                                                                    }

                                                                        break;
                                                                    case 128:
                                                                        var call__274 = U.FUnitHS.GetXhx();
                                                                        var call__275 = U.FUnitHS.GetYhx();
                                                                        EvalHex(Target, Ability, Spell, call__274, call__275, ref Value, true, true, U, Wall);
                                                                    {
                                                                        I = 0;
                                                                        byte end__276 = 18;
                                                                        for (; I <= end__276; I += 1)
                                                                        {
                                                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                            EvalHex(Target, Ability, Spell, NX, NY, ref Value, true, true, U, Wall);
                                                                        }
                                                                    }

                                                                        BestIndex = X;
                                                                        BestValue = Y;
                                                                    {
                                                                        I = 1;
                                                                        byte end__277 = 18;
                                                                        for (; I <= end__277; I += 1)
                                                                        {
                                                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                            var call__278 = U.FUnitHS.GetYhx();
                                                                            var call__279 = U.FUnitHS.GetXhx();
                                                                            var call__280 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__279, call__278, NX, NY);
                                                                            if (call__280 <= Range)
                                                                            {
                                                                                D = Cost;
                                                                                var call__281 = U.FUnitHS.GetXhx();
                                                                                var call__282 = U.FUnitHS.GetYhx();
                                                                                EvalHex(Target, Ability, Spell, call__281, call__282, ref D, true, true, U, Wall);
                                                                                {
                                                                                    V = 0;
                                                                                    byte end__283 = 18;
                                                                                    for (; V <= end__283; V += 1)
                                                                                    {
                                                                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(NX, NY, V, ref AX, ref AY);
                                                                                        EvalHex(Target, Ability, Spell, AX, AY, ref D, true, true, U, Wall);
                                                                                    }
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
                                                                        var call__284 = U.FUnitHS.GetXhx();
                                                                        var call__285 = U.FUnitHS.GetYhx();
                                                                        var call__286 = EvalPath(ref Value, Target, Ability, Spell, call__284, call__285, X, Y, false, U, Wall);
                                                                        PathValue = call__286;
                                                                        break;
                                                                    case 116:
                                                                        EvalArea(ref Value, Target, Ability, Spell, X, Y, true, true, true, U, Wall);
                                                                        BestIndex = X;
                                                                        BestValue = Y;
                                                                    {
                                                                        I = 1;
                                                                        byte end__287 = 6;
                                                                        for (; I <= end__287; I += 1)
                                                                        {
                                                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                            var call__288 = U.FUnitHS.GetYhx();
                                                                            var call__289 = U.FUnitHS.GetXhx();
                                                                            var call__290 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__289, call__288, NX, NY);
                                                                            if (call__290 <= Range)
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
                                                                        byte end__291 = 6;
                                                                        for (; I <= end__291; I += 1)
                                                                        {
                                                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                            var call__292 = U.FUnitHS.GetYhx();
                                                                            var call__293 = U.FUnitHS.GetXhx();
                                                                            var call__294 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__293, call__292, NX, NY);
                                                                            if (call__294 <= Range)
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
                                                                        byte end__295 = 6;
                                                                        for (; I <= end__295; I += 1)
                                                                        {
                                                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                                            var call__296 = U.FUnitHS.GetYhx();
                                                                            var call__297 = U.FUnitHS.GetXhx();
                                                                            var call__298 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__297, call__296, NX, NY);
                                                                            if (call__298 <= Range)
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
                                                                        if ((FSide == Aow.Units.AoWE.TCombatSideIndex.csDefender) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallType != 0))
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
                                                                        var call__299 = U.FUnitHS.GetXhx();
                                                                        var call__300 = U.FUnitHS.GetYhx();
                                                                        var call__301 = EvalBreath(ref Value, Target, Ability, Spell, call__299, call__300, X, Y, U);
                                                                        PathValue = call__301;
                                                                        break;
                                                                }
                                                            }
                                                        }

                                                        if ((D <= Range) && ((PathValue > 0) || (Value > 0)))
                                                        {
                                                            if ((PathValue != 0) && (Value < Cost))
                                                            {
                                                                BestIndex = 0;
                                                                BestValue = Value;
                                                                D = Cost;
                                                                I = Path.Count - 2;
                                                                while (I >= 1)
                                                                {
                                                                    var call__302 = Path[I];
                                                                    var call__303 = Path[I];
                                                                    var call__304 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                    var call__305 = call__304.Field[call__303.X, call__302.Y];
                                                                    var call__306 = call__305.FindOwnedChild(2229508);
                                                                    CityWall = (Aow.Units.CityWall.TCityWall)call__306;
                                                                    if ((CityWall == null) && (FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker))
                                                                    {
                                                                        var call__307 = Path[I];
                                                                        var call__308 = Path[I];
                                                                        var call__309 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                        var call__310 = call__309.Field[call__308.X, call__307.Y];
                                                                        var call__311 = call__310.FindOwnedChild(2229510);
                                                                        CityWall = (Aow.Units.CityWall.TCityWall)call__311;
                                                                    }

                                                                    bool bool__312 = (CityWall == null) || (CityWall.FHitPoints <= 0);
                                                                    if (!bool__312)
                                                                    {
                                                                        var call__313 = U.GetAbilityEnabled(1);
                                                                        bool__312 = call__313;
                                                                    }

                                                                    bool bool__314 = bool__312;
                                                                    if (!bool__314)
                                                                    {
                                                                        var call__315 = U.GetAbilityEnabled(36);
                                                                        bool__314 = call__315;
                                                                    }

                                                                    bool bool__316 = bool__314;
                                                                    if (!bool__316)
                                                                    {
                                                                        var call__317 = U.GetAbilityEnabled(66);
                                                                        bool__316 = call__317;
                                                                    }

                                                                    if (!bool__316)
                                                                    {
                                                                        I = 1;
                                                                    }
                                                                    else
                                                                    {
                                                                        var call__318 = Path[I];
                                                                        var call__319 = Path[I];
                                                                        var call__320 = Path[0];
                                                                        var call__321 = Path[0];
                                                                        var call__322 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__321.X, call__320.Y, call__319.X, call__318.Y);
                                                                        if (call__322 <= Range)
                                                                        {
                                                                            var call__323 = Path[I];
                                                                            var call__324 = Path[I];
                                                                            var call__325 = EvalPath(ref Cost, Target, Ability, Spell, call__323.X, call__324.Y, X, Y, Spell == 117, U, Wall);
                                                                            V = call__325;
                                                                            Cost = D;
                                                                            if (V < PathValue)
                                                                            {
                                                                                BestValue = (BestValue + PathValue) - V;
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
                                                                    {
                                                                        I = BestIndex;
                                                                        int end__326 = Path.Count - 1;
                                                                        for (; I <= end__326; I += 1)
                                                                        {
                                                                            var call__327 = Path[I];
                                                                            DetourPath.Add(call__327);
                                                                        }
                                                                    }

                                                                    TerrainValue = 0;
                                                                    PathCost = MakeTerrainPath(Settings, ref DetourPath, ref TerrainValue, U, (Aow.Units.AoWTC.TTacticalCombatUnit)Target, true, false, 1);
                                                                    PathValue += BestValue;
                                                                    PathCost = ((Aow.Units.AoWE.TMovepath)Path).PosToMovePoints(0);
                                                                }
                                                            }

                                                            BestIndex = 0;
                                                            {
                                                                I = 1;
                                                                int end__328 = Path.Count - 2;
                                                                for (; I <= end__328; I += 1)
                                                                {
                                                                    var call__329 = Path[I];
                                                                    var call__330 = Path[I];
                                                                    var call__331 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, call__330.X, call__329.Y);
                                                                    bool bool__332 = (call__331 <= Range);
                                                                    if (bool__332)
                                                                    {
                                                                        var call__333 = ((Aow.Units.AoWE.TMovepath)Path).PosToMovePoints(I);
                                                                        bool__332 = (call__333 <= U.FMovePoints);
                                                                    }

                                                                    if (bool__332)
                                                                    {
                                                                        Cost = 1000;
                                                                        var call__334 = Path[I];
                                                                        var call__335 = Path[I];
                                                                        var call__336 = EvalPath(ref Cost, Target, Ability, Spell, call__334.X, call__335.Y, X, Y, Spell == 117, U, Wall);
                                                                        V = call__336;
                                                                        if ((V == 0) && (Cost == 1000))
                                                                        {
                                                                            BestIndex = I;
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            if (BestIndex > 0)
                                                            {
                                                                {
                                                                    I = 0;
                                                                    int end__337 = BestIndex - 1;
                                                                    for (; I <= end__337; I += 1)
                                                                    {
                                                                        Path.Delete(0);
                                                                    }
                                                                }
                                                            }

                                                            PathCost = ((Aow.Units.AoWE.TMovepath)Path).PosToMovePoints(0);
                                                        }

                                                        // { Keep move selection inside the positive movement guard. }
                                                        Cost = ((PathCost - U.FMovePoints) - (Range * 4)) * 10;
                                                        if (Cost < 1)
                                                        {
                                                            Cost = 1;
                                                        }

                                                        if (!(U.FUnit is Aow.Units.AoWE.THero))
                                                        {
                                                            SafeValue = 0;
                                                        }
                                                        else
                                                        {
                                                            I = -1;
                                                            var call__338 = ((Aow.Units.AoWE.TMovepath)Path).MovePointsToPos(U.FMovePoints);
                                                            BestIndex = call__338;
                                                            var call__339 = Path[BestIndex];
                                                            var call__340 = Path[BestIndex];
                                                            var call__341 = UnitSafe(U, call__340.X, call__339.Y);
                                                            SafeValue = call__341;
                                                            while ((BestIndex < (Path.Count - 1)) && (SafeValue < 0))
                                                            {
                                                                var call__342 = Path[BestIndex];
                                                                var call__343 = Path[BestIndex];
                                                                var call__344 = UnitSafe(U, call__343.X, call__342.Y);
                                                                SafeValue = call__344;
                                                                if (SafeValue >= 0)
                                                                {
                                                                    I = BestIndex;
                                                                }

                                                                BestIndex += 1;
                                                            }

                                                            if (I > -1)
                                                            {
                                                                {
                                                                    V = 0;
                                                                    int end__345 = I - 1;
                                                                    for (; V <= end__345; V += 1)
                                                                    {
                                                                        Path.Delete(0);
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        bool bool__346 = ((PathValue == 0) || (Value != 0)) && (Distance <= Range);
                                                        if (bool__346)
                                                        {
                                                            var call__347 = U.GetMoves();
                                                            bool__346 = (call__347 == U.FMovePoints);
                                                        }

                                                        if (bool__346)
                                                        {
                                                            FHeroMoves += 1;
                                                            I = System.Math.Abs(Value + PathValue);
                                                            if (I < 1)
                                                            {
                                                                I = 1;
                                                            }

                                                            var call__348 = MultDEV(Value, (Value * 100) / I, 1);
                                                            CreateMove(Aow.Units.AoWTC.TaiMoveType.aiRanged, X, Y, call__348, Ability, Spell, U, Target);
                                                            if (((PathValue > 0) && (U.FMovePoints > 0)) && (DetourPath.Count > 1))
                                                            {
                                                                if (!((DetourPath.List[DetourPath.Count - 2].Flag > U.FMovePoints) && (DetourPath.List[DetourPath.Count - 2].Flag <= 15)))
                                                                {
                                                                    FHeroMoves += 1;
                                                                    var call__349 = MultDEV(PathValue + TerrainValue, (100 * PathValue) / I, 1);
                                                                    var call__350 = DetourPath[0];
                                                                    var call__351 = DetourPath[0];
                                                                    CreateMove(Aow.Units.AoWTC.TaiMoveType.aiMoveRanged, call__351.X, call__350.Y, call__349, Ability, Spell, U, Target);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            bool bool__352 = (Path.Count > 1);
                                                            if (bool__352)
                                                            {
                                                                bool bool__353 = FIgnoreWalls;
                                                                if (!bool__353)
                                                                {
                                                                    var call__354 = ChargeFromPos((int)(((uint)U.FPosition) >> 4));
                                                                    bool__353 = call__354;
                                                                }

                                                                bool__352 = bool__353;
                                                            }

                                                            if (bool__352)
                                                            {
                                                                if (!(((((SafeValue < 0) && (FNormalUnits != 0)) || (U.FMovePoints <= 0)) || (Path.Count <= 1)) || ((Path.List[Path.Count - 2].Flag > U.FMovePoints) && (Path.List[Path.Count - 2].Flag <= 15))))
                                                                {
                                                                    var call__355 = U.GetMoves();
                                                                    var call__356 = MultDEV((PathValue + TerrainValue) + Value, call__355, Cost);
                                                                    var call__357 = Path[0];
                                                                    var call__358 = Path[0];
                                                                    CreateMove(Aow.Units.AoWTC.TaiMoveType.aiMoveRanged, call__358.X, call__357.Y, call__356, Ability, Spell, U, Target);
                                                                }
                                                                else
                                                                {
                                                                    var call__359 = U.GetMoves();
                                                                    var call__360 = MultDEV(PathValue, 100, 1);
                                                                    var call__361 = MultDEV((call__360 + TerrainValue) + Value, call__359, Cost);
                                                                    CreateMove(Aow.Units.AoWTC.TaiMoveType.aiFake, X, Y, call__361, Ability, Spell, U, Target);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                break;
                                        }
                                    }
                                }
                                catch (System.Exception)
                                {
                                    Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Spell Check", AnsiString.FromPChar(Ptr<byte>.Unbox(((((AnsiString)"Error - TCAI ") + Aow.Rtl.Rtl.IntToStr(Ability)) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Spell))), (int)0);
                                }

                                break;
                            default:
                                switch (Aow.Units.AoWTC.AoWTC.AbilTypes[Ability])
                                {
                                    case Aow.Units.AoWTC.TfxType.fxTouch:
                                        bool bool__362 = (Target is Aow.Units.AoWE.TCombatWall);
                                        if (!bool__362)
                                        {
                                            bool bool__363 = (Target is Aow.Units.AoWE.TCombatUnit);
                                            if (bool__363)
                                            {
                                                var call__364 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                                                var call__365 = ((Aow.Units.AoWE.TTouchAbility)call__364).CanTouch(U, Target);
                                                bool__363 = call__365;
                                            }

                                            bool__362 = bool__363;
                                        }

                                        if (bool__362)
                                        {
                                            try
                                            {
                                                var call__366 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                                                call__366.tcGetDamageValueEx(U, Target, ref Values);
                                                if (Values.SourceDamage > 0)
                                                {
                                                    var call__367 = ((Aow.Units.AoWE.TMovepath)Path).MovePointsToPos(U.FMovePoints);
                                                    Adjacent = call__367 == 0;
                                                    U.FCanFlee = false;
                                                    if (Target is Aow.Units.AoWE.TCombatWall)
                                                    {
                                                        FFlee = false;
                                                    }

                                                    if (Ability == 65)
                                                    {
                                                        Values.TargetDamage = 0;
                                                    }

                                                    var call__368 = tcDVtoDEV(Values.SourceDamage, Values.TargetDamage, U, Target);
                                                    var call__369 = MultDEV(call__368, HitPoints, Hits);
                                                    Value = call__369 + TerrainValue;
                                                    switch (Ability)
                                                    {
                                                        case >= 27 and <= 29:
                                                        case 40:
                                                        case 60:
                                                        case 106:
                                                        case 107:
                                                        case 148:
                                                            var call__370 = tcDVtoDEV(Values.SourceDamage, 0, U, Target);
                                                            Value = call__370;
                                                            break;
                                                        default:
                                                            var call__371 = tcDVtoDEV(Values.SourceDamage, 0, U, Target);
                                                            var call__372 = MultDEV(call__371, HitPoints, Hits);
                                                            Value = call__372;
                                                            break;
                                                    }

                                                    if ((Ability == 117) && (Aow.Units.AoWE.AoWE.MeleeDamageTypes(U) == TSet<Aow.Units.AoWE.TDamageType>.Of()))
                                                    {
                                                        Value = MultDEV(Value, 100, 1);
                                                    }

                                                    var call__373 = Target.GetTargetStrength();
                                                    if ((call__373 <= 0) && (Enemies.Count > 1))
                                                    {
                                                        Value -= 10000;
                                                    }

                                                    bool bool__374 = (U.FMovePoints > 0);
                                                    if (bool__374)
                                                    {
                                                        var call__375 = ((Aow.Units.AoWE.TMovepath)Path).MovePointsToPos(U.FMovePoints);
                                                        bool__374 = (call__375 == 0);
                                                    }

                                                    if (bool__374)
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

                                                    if (!(U.FUnit is Aow.Units.AoWE.THero))
                                                    {
                                                        SafeValue = 0;
                                                    }
                                                    else
                                                    {
                                                        I = -1;
                                                        var call__376 = ((Aow.Units.AoWE.TMovepath)Path).MovePointsToPos(U.FMovePoints);
                                                        BestIndex = call__376;
                                                        var call__377 = Path[BestIndex];
                                                        var call__378 = Path[BestIndex];
                                                        var call__379 = UnitSafe(U, call__378.X, call__377.Y);
                                                        SafeValue = call__379;
                                                        while ((BestIndex < (Path.Count - 1)) && (SafeValue < 0))
                                                        {
                                                            var call__380 = Path[BestIndex];
                                                            var call__381 = Path[BestIndex];
                                                            var call__382 = UnitSafe(U, call__381.X, call__380.Y);
                                                            SafeValue = call__382;
                                                            if (SafeValue >= 0)
                                                            {
                                                                I = BestIndex;
                                                            }

                                                            BestIndex += 1;
                                                        }

                                                        if (I > -1)
                                                        {
                                                            {
                                                                V = 0;
                                                                int end__383 = I - 1;
                                                                for (; V <= end__383; V += 1)
                                                                {
                                                                    Path.Delete(0);
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (Path.Count > 1)
                                                    {
                                                        if (((SafeValue >= 0) || (FNormalUnits == 0)) && Adjacent)
                                                        {
                                                            FHeroMoves += 1;
                                                            if (U.FUnit is Aow.Units.AoWE.THero)
                                                            {
                                                                I = 1;
                                                            }
                                                            else
                                                            {
                                                                if (((Target is Aow.Units.AoWE.TCombatWall) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallBreached) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FRound > 1))
                                                                {
                                                                    I = 10;
                                                                }
                                                                else
                                                                {
                                                                    I = 100;
                                                                }
                                                            }

                                                            var call__384 = MultDEV(Value, I, 1);
                                                            CreateMove(Aow.Units.AoWTC.TaiMoveType.aiTouch, X, Y, call__384, Ability, Spell, U, Target);
                                                        }
                                                        else
                                                        {
                                                            bool bool__385 = FIgnoreWalls;
                                                            if (!bool__385)
                                                            {
                                                                var call__386 = ChargeFromPos((int)(((uint)U.FPosition) >> 4));
                                                                bool__385 = call__386;
                                                            }

                                                            if (bool__385)
                                                            {
                                                                if (!((((SafeValue < 0) && (FNormalUnits != 0)) || (U.FMovePoints <= 0)) || ((Path.List[Path.Count - 2].Flag > U.FMovePoints) && (Path.List[Path.Count - 2].Flag <= 15))))
                                                                {
                                                                    FHeroMoves += 1;
                                                                    var call__387 = U.GetMoves();
                                                                    var call__388 = MultDEV(Value, call__387, Cost);
                                                                    var call__389 = Path[0];
                                                                    var call__390 = Path[0];
                                                                    CreateMove(Aow.Units.AoWTC.TaiMoveType.aiMove, call__390.X, call__389.Y, call__388, Ability, Spell, U, Target);
                                                                }
                                                                else
                                                                {
                                                                    var call__391 = U.GetMoves();
                                                                    var call__392 = MultDEV(Value, call__391, Cost);
                                                                    CreateMove(Aow.Units.AoWTC.TaiMoveType.aiFake, X, Y, call__392, Ability, Spell, U, Target);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            catch (System.Exception)
                                            {
                                                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Touch Check", AnsiString.FromPChar(Ptr<byte>.Unbox(((((AnsiString)"Error - TCAI ") + Aow.Rtl.Rtl.IntToStr(Ability)) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Spell))), (int)0);
                                            }
                                        }

                                        break;
                                    case Aow.Units.AoWTC.TfxType.fxArrow:
                                    case Aow.Units.AoWTC.TfxType.fxRock:
                                    case Aow.Units.AoWTC.TfxType.fxComet:
                                    case Aow.Units.AoWTC.TfxType.fxBreath:
                                    case Aow.Units.AoWTC.TfxType.fxCometBall:
                                    case Aow.Units.AoWTC.TfxType.fxEffect:
                                    case Aow.Units.AoWTC.TfxType.fxScreenNESW:
                                    case Aow.Units.AoWTC.TfxType.fxScreenNS:
                                    case Aow.Units.AoWTC.TfxType.fxRay:
                                    case Aow.Units.AoWTC.TfxType.fxChain:
                                    case Aow.Units.AoWTC.TfxType.fxSwarm:
                                    case Aow.Units.AoWTC.TfxType.fxScreenSwarm:
                                    case Aow.Units.AoWTC.TfxType.fxSacredWrath:
                                    case Aow.Units.AoWTC.TfxType.fxSWComet:
                                    case Aow.Units.AoWTC.TfxType.fxDispel:
                                    case Aow.Units.AoWTC.TfxType.fxBlood:
                                    case Aow.Units.AoWTC.TfxType.fxAltArrow:
                                    case Aow.Units.AoWTC.TfxType.fxTremors:
                                    case Aow.Units.AoWTC.TfxType.fxHailImpact:
                                    case Aow.Units.AoWTC.TfxType.fxHighPrayer:
                                        try
                                        {
                                            var call__393 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                                            var call__394 = call__393.tcGetDamageValue(U, Target);
                                            Cost = call__394;
                                            if (Cost > 0)
                                            {
                                                U.FCanFlee = false;
                                                if (Target is Aow.Units.AoWE.TCombatWall)
                                                {
                                                    FFlee = false;
                                                }

                                                PathValue = 0;
                                                var call__395 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                                                var call__396 = call__395.GetCombatMode(U.FUnit);
                                                if (call__396 == Aow.Units.AoWE.TCombatMode.cmLongRange)
                                                {
                                                    Range = 12;
                                                }
                                                else
                                                {
                                                    var call__397 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Ability);
                                                    var call__398 = call__397.GetCombatMode(U.FUnit);
                                                    if (call__398 == Aow.Units.AoWE.TCombatMode.cmMediumRange)
                                                    {
                                                        Range = 8;
                                                    }
                                                    else
                                                    {
                                                        Range = 4;
                                                    }
                                                }

                                                var call__399 = tcDVtoDEV(Cost, 0, U, Target);
                                                var call__400 = MultDEV(call__399, HitPoints, Hits);
                                                Value = call__400;
                                                var call__401 = Target.GetTargetStrength();
                                                if ((call__401 <= 0) && (Enemies.Count > 1))
                                                {
                                                    Value -= 10000;
                                                }

                                                if (U.FMovePoints > 0)
                                                {
                                                    var call__402 = ((Aow.Units.AoWE.TMovepath)Path).MovePointsToPos(U.FMovePoints);
                                                    I = call__402;
                                                    var call__403 = Path[I];
                                                    var call__404 = Path[I];
                                                    var call__405 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__404.X, call__403.Y, X, Y);
                                                    D = call__405;
                                                    if ((D <= Range) || (Distance <= Range))
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
                                                                    var call__406 = U.FUnitHS.GetXhx();
                                                                    var call__407 = U.FUnitHS.GetYhx();
                                                                    var call__408 = EvalPath(ref Value, Target, Ability, Spell, call__406, call__407, X, Y, false, U, Wall);
                                                                    PathValue = call__408;
                                                                    break;
                                                                case 24:
                                                                case 44:
                                                                    var call__409 = U.FUnitHS.GetXhx();
                                                                    var call__410 = U.FUnitHS.GetYhx();
                                                                    var call__411 = EvalPath(ref Value, Target, Ability, Spell, call__409, call__410, X, Y, true, U, Wall);
                                                                    PathValue = call__411;
                                                                    break;
                                                                case 30:
                                                                case >= 86 and <= 90:
                                                                    var call__412 = U.FUnitHS.GetXhx();
                                                                    var call__413 = U.FUnitHS.GetYhx();
                                                                    var call__414 = EvalBreath(ref Value, Target, Ability, Spell, call__412, call__413, X, Y, U);
                                                                    PathValue = call__414;
                                                                    break;
                                                            }
                                                        }
                                                    }

                                                    if ((D <= Range) && ((PathValue > 0) || (Value > 0)))
                                                    {
                                                        if ((PathValue != 0) && (Value < Cost))
                                                        {
                                                            BestIndex = 0;
                                                            BestValue = Value;
                                                            D = Cost;
                                                            I = Path.Count - 2;
                                                            while (I >= 1)
                                                            {
                                                                var call__415 = Path[I];
                                                                var call__416 = Path[I];
                                                                var call__417 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                var call__418 = call__417.Field[call__416.X, call__415.Y];
                                                                var call__419 = call__418.FindOwnedChild(2229508);
                                                                CityWall = (Aow.Units.CityWall.TCityWall)call__419;
                                                                if ((CityWall == null) && (FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker))
                                                                {
                                                                    var call__420 = Path[I];
                                                                    var call__421 = Path[I];
                                                                    var call__422 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                                    var call__423 = call__422.Field[call__421.X, call__420.Y];
                                                                    var call__424 = call__423.FindOwnedChild(2229510);
                                                                    CityWall = (Aow.Units.CityWall.TCityWall)call__424;
                                                                }

                                                                bool bool__425 = (CityWall == null) || (CityWall.FHitPoints <= 0);
                                                                if (!bool__425)
                                                                {
                                                                    var call__426 = U.GetAbilityEnabled(1);
                                                                    bool__425 = call__426;
                                                                }

                                                                bool bool__427 = bool__425;
                                                                if (!bool__427)
                                                                {
                                                                    var call__428 = U.GetAbilityEnabled(36);
                                                                    bool__427 = call__428;
                                                                }

                                                                bool bool__429 = bool__427;
                                                                if (!bool__429)
                                                                {
                                                                    var call__430 = U.GetAbilityEnabled(66);
                                                                    bool__429 = call__430;
                                                                }

                                                                if (!bool__429)
                                                                {
                                                                    I = 1;
                                                                }
                                                                else
                                                                {
                                                                    var call__431 = Path[I];
                                                                    var call__432 = Path[I];
                                                                    var call__433 = Path[0];
                                                                    var call__434 = Path[0];
                                                                    var call__435 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__434.X, call__433.Y, call__432.X, call__431.Y);
                                                                    if (call__435 <= Range)
                                                                    {
                                                                        var call__436 = Path[I];
                                                                        var call__437 = Path[I];
                                                                        var call__438 = EvalPath(ref Cost, Target, Ability, Spell, call__436.X, call__437.Y, X, Y, (Ability == 24) || (Ability == 44), U, Wall);
                                                                        V = call__438;
                                                                        Cost = D;
                                                                        if (V < PathValue)
                                                                        {
                                                                            BestValue = (BestValue + PathValue) - V;
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
                                                                {
                                                                    I = BestIndex;
                                                                    int end__439 = Path.Count - 1;
                                                                    for (; I <= end__439; I += 1)
                                                                    {
                                                                        var call__440 = Path[I];
                                                                        DetourPath.Add(call__440);
                                                                    }
                                                                }

                                                                TerrainValue = 0;
                                                                PathCost = MakeTerrainPath(Settings, ref DetourPath, ref TerrainValue, U, (Aow.Units.AoWTC.TTacticalCombatUnit)Target, true, false, 1);
                                                                PathValue += BestValue;
                                                                PathCost = ((Aow.Units.AoWE.TMovepath)Path).PosToMovePoints(0);
                                                            }
                                                        }

                                                        BestIndex = 0;
                                                        {
                                                            I = 1;
                                                            int end__441 = Path.Count - 2;
                                                            for (; I <= end__441; I += 1)
                                                            {
                                                                var call__442 = Path[I];
                                                                var call__443 = Path[I];
                                                                var call__444 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, call__443.X, call__442.Y);
                                                                bool bool__445 = (call__444 <= Range);
                                                                if (bool__445)
                                                                {
                                                                    var call__446 = ((Aow.Units.AoWE.TMovepath)Path).PosToMovePoints(I);
                                                                    bool__445 = (call__446 <= U.FMovePoints);
                                                                }

                                                                if (bool__445)
                                                                {
                                                                    Cost = 1000;
                                                                    var call__447 = Path[I];
                                                                    var call__448 = Path[I];
                                                                    var call__449 = EvalPath(ref Cost, Target, Ability, Spell, call__447.X, call__448.Y, X, Y, Spell == 117, U, Wall);
                                                                    V = call__449;
                                                                    if ((V == 0) && (Cost == 1000))
                                                                    {
                                                                        BestIndex = I;
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        if (BestIndex > 0)
                                                        {
                                                            {
                                                                I = 0;
                                                                int end__450 = BestIndex - 1;
                                                                for (; I <= end__450; I += 1)
                                                                {
                                                                    Path.Delete(0);
                                                                }
                                                            }
                                                        }

                                                        PathCost = ((Aow.Units.AoWE.TMovepath)Path).PosToMovePoints(0);
                                                    }

                                                    // { Keep move selection inside the positive movement guard. }
                                                    Cost = ((PathCost - U.FMovePoints) - (Range * 4)) * 10;
                                                    if (Cost < 1)
                                                    {
                                                        Cost = 1;
                                                    }

                                                    if (!(U.FUnit is Aow.Units.AoWE.THero))
                                                    {
                                                        SafeValue = 0;
                                                    }
                                                    else
                                                    {
                                                        I = -1;
                                                        var call__451 = ((Aow.Units.AoWE.TMovepath)Path).MovePointsToPos(U.FMovePoints);
                                                        BestIndex = call__451;
                                                        var call__452 = Path[BestIndex];
                                                        var call__453 = Path[BestIndex];
                                                        var call__454 = UnitSafe(U, call__453.X, call__452.Y);
                                                        SafeValue = call__454;
                                                        while ((BestIndex < (Path.Count - 1)) && (SafeValue < 0))
                                                        {
                                                            var call__455 = Path[BestIndex];
                                                            var call__456 = Path[BestIndex];
                                                            var call__457 = UnitSafe(U, call__456.X, call__455.Y);
                                                            SafeValue = call__457;
                                                            if (SafeValue >= 0)
                                                            {
                                                                I = BestIndex;
                                                            }

                                                            BestIndex += 1;
                                                        }

                                                        if (I > -1)
                                                        {
                                                            {
                                                                V = 0;
                                                                int end__458 = I - 1;
                                                                for (; V <= end__458; V += 1)
                                                                {
                                                                    Path.Delete(0);
                                                                }
                                                            }
                                                        }
                                                    }

                                                    var call__459 = U.FUnitHS.GetYhx();
                                                    var call__460 = U.FUnitHS.GetXhx();
                                                    var call__461 = UnitSafe(U, call__460, call__459);
                                                    bool bool__462 = (((call__461 < 0) && (FNormalUnits != 0)) || ((PathValue != 0) && (Value == 0))) || (Distance > Range);
                                                    if (!bool__462)
                                                    {
                                                        var call__463 = U.GetMoves();
                                                        bool__462 = (call__463 != U.FMovePoints);
                                                    }

                                                    if (!bool__462)
                                                    {
                                                        FHeroMoves += 1;
                                                        if (((Target is Aow.Units.AoWE.TCombatWall) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallBreached) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FRound > 1))
                                                        {
                                                            I = 10;
                                                        }
                                                        else
                                                        {
                                                            I = 100;
                                                        }

                                                        V = System.Math.Abs(Value + PathValue);
                                                        if (V < 1)
                                                        {
                                                            V = 1;
                                                        }

                                                        var call__464 = MultDEV(Value, (I * Value) / V, 1);
                                                        CreateMove(Aow.Units.AoWTC.TaiMoveType.aiRanged, X, Y, call__464, Ability, Spell, U, Target);
                                                        if (((PathValue > 0) && (U.FMovePoints > 0)) && (DetourPath.Count > 1))
                                                        {
                                                            if (!((DetourPath.List[DetourPath.Count - 2].Flag > U.FMovePoints) && (DetourPath.List[DetourPath.Count - 2].Flag <= 15)))
                                                            {
                                                                FHeroMoves += 1;
                                                                var call__465 = MultDEV(PathValue + TerrainValue, (I * PathValue) / V, 1);
                                                                var call__466 = DetourPath[0];
                                                                var call__467 = DetourPath[0];
                                                                CreateMove(Aow.Units.AoWTC.TaiMoveType.aiMoveRanged, call__467.X, call__466.Y, call__465, Ability, Spell, U, Target);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bool bool__468 = (Path.Count > 1);
                                                        if (bool__468)
                                                        {
                                                            bool bool__469 = FIgnoreWalls;
                                                            if (!bool__469)
                                                            {
                                                                var call__470 = ChargeFromPos((int)(((uint)U.FPosition) >> 4));
                                                                bool__469 = call__470;
                                                            }

                                                            bool__468 = bool__469;
                                                        }

                                                        if (bool__468)
                                                        {
                                                            if (!(((((SafeValue < 0) && (FNormalUnits != 0)) || (U.FMovePoints <= 0)) || (Path.Count <= 1)) || ((Path.List[Path.Count - 2].Flag > U.FMovePoints) && (Path.List[Path.Count - 2].Flag <= 15))))
                                                            {
                                                                var call__471 = U.GetMoves();
                                                                var call__472 = MultDEV((PathValue + TerrainValue) + Value, call__471, Cost);
                                                                var call__473 = Path[0];
                                                                var call__474 = Path[0];
                                                                CreateMove(Aow.Units.AoWTC.TaiMoveType.aiMoveRanged, call__474.X, call__473.Y, call__472, Ability, Spell, U, Target);
                                                            }
                                                            else
                                                            {
                                                                var call__475 = U.GetMoves();
                                                                var call__476 = MultDEV(PathValue, 100, 1);
                                                                var call__477 = MultDEV((call__476 + TerrainValue) + Value, call__475, Cost);
                                                                CreateMove(Aow.Units.AoWTC.TaiMoveType.aiFake, X, Y, call__477, Ability, Spell, U, Target);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch (System.Exception)
                                        {
                                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Ranged Check", AnsiString.FromPChar(Ptr<byte>.Unbox(((((AnsiString)"Error - TCAI ") + Aow.Rtl.Rtl.IntToStr(Ability)) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Spell))), (int)0);
                                        }

                                        break;
                                }

                                break;
                        }
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
            int I = default;
            int J = default;
            int K = default;
            int L = default;
            int N = default;
            int Value = default;
            Aow.Units.AoWTC.TaiMove Move = default;
            Aow.Units.AoWTC.TaiMove OtherMove = default;
            Aow.Units.AoWTC.TTacticalCombatUnit U = default;
            Aow.Units.AoWTC.TTacticalCombatUnit Target = default;
            Aow.Units.AoWTC.TCombatMoveControlSettings Settings = default;
            bool Moved = default;
            bool Found = default;
            bool Visible = default;
            Aow.Rtl.TList Abilities = default;
            int TerrainCost = default;
            int PathCost = default;
            Aow.Rtl.TList Bonuses = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.HSEngine.TXYLList Path = default;
            Aow.Units.AoWE.TAbilityCombatInfo AIInfo = default;
            Aow.Units.AoWE.TSpellCombatInfo SpellInfo = default;
            Aow.Units.CityWall.TCityWall Wall = default;
            Status = Aow.Units.AoWTC.TaiStatus.aiBusy;
            var call__478 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentPlayer);
            if (call__478)
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
                            {
                                I = 0;
                                int end__479 = Moves.Count - 1;
                                for (; I <= end__479; I += 1)
                                {
                                    var call__480 = Moves[I];
                                    ((Aow.Rtl.TObject)call__480).Free();
                                }
                            }

                            Moves.Clear();
                            Walls.Clear();
                            {
                                var w__481 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat;
                                var call__482 = w__481.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)unchecked((sbyte)w__481.FCombatData.FCurrentPlayer)));
                                FSide = (Aow.Units.AoWE.TCombatSideIndex)call__482.FSideIndex;
                                FNormalUnits = 0;
                                {
                                    I = 0;
                                    int end__483 = w__481.FCombatData.GetCount() - 1;
                                    for (; I <= end__483; I += 1)
                                    {
                                        var call__484 = w__481.FCombatData.GetObjects(I);
                                        bool bool__485 = (call__484 != null);
                                        if (bool__485)
                                        {
                                            var call__486 = w__481.FCombatData.GetObjects(I);
                                            bool__485 = (call__486 is Aow.Units.AoWE.TCombatUnit);
                                        }

                                        bool bool__487 = bool__485;
                                        if (bool__487)
                                        {
                                            var call__488 = w__481.FCombatData.GetObjects(I);
                                            var call__489 = call__488.GetEnabled();
                                            bool__487 = call__489;
                                        }

                                        if (bool__487)
                                        {
                                            Visible = false;
                                            J = 0;
                                            while (!Visible && (J < 19))
                                            {
                                                var call__490 = w__481.FCombatData.GetObjects(I);
                                                var call__491 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__490).FUnitHS.GetYhx();
                                                var call__492 = w__481.FCombatData.GetObjects(I);
                                                var call__493 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__492).FUnitHS.GetXhx();
                                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__493, call__491, J, ref K, ref L);
                                                bool bool__494 = !Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
                                                if (!bool__494)
                                                {
                                                    var call__495 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(K, L, 0);
                                                    var call__496 = ((Aow.Units.AoWE.TAoWMapField)call__495).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                                                    bool__494 = call__496;
                                                }

                                                if (bool__494)
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
                                                var call__497 = w__481.FCombatData.GetObjects(I);
                                                var call__498 = call__497.GetSide();
                                                if (call__498 == FSide)
                                                {
                                                    var call__499 = w__481.FCombatData.GetObjects(I);
                                                    if (unchecked((byte)call__499.FCombatPlayer) == w__481.FCombatData.FCurrentPlayer)
                                                    {
                                                        var call__500 = w__481.FCombatData.GetObjects(I);
                                                        var call__501 = call__500.GetLocked();
                                                        if (!call__501)
                                                        {
                                                            var call__502 = w__481.FCombatData.GetObjects(I);
                                                            if (((Aow.Units.AoWTC.TTacticalCombatUnit)call__502).FUnit is Aow.Units.AoWE.THero)
                                                            {
                                                                var call__503 = w__481.FCombatData.GetObjects(I);
                                                                Units.Add(call__503);
                                                            }
                                                            else
                                                            {
                                                                FNormalUnits += 1;
                                                                var call__504 = w__481.FCombatData.GetObjects(I);
                                                                Units.Insert(0, call__504);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        var call__505 = w__481.FCombatData.GetObjects(I);
                                                        Allies.Add(call__505);
                                                    }
                                                }
                                                else
                                                {
                                                    var call__506 = w__481.FCombatData.GetObjects(I);
                                                    Enemies.Add(call__506);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            Aow.Units.AoWE.AoWE.AoWEngine.OutputDebugStr((((AnsiString)"AI found ") + Aow.Rtl.Rtl.IntToStr(Units.Count)) + ((AnsiString)" Own units."));
                            Aow.Units.AoWE.AoWE.AoWEngine.OutputDebugStr((((AnsiString)"AI found ") + Aow.Rtl.Rtl.IntToStr(Allies.Count)) + ((AnsiString)" Ally units."));
                            Aow.Units.AoWE.AoWE.AoWEngine.OutputDebugStr((((AnsiString)"AI found ") + Aow.Rtl.Rtl.IntToStr(Enemies.Count)) + ((AnsiString)" Enemy units."));
                            FIgnoreWalls = FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker;
                            FFlee = ((FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallType != 0)) && !Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallBreached;
                            FMinMoves = 100;
                            {
                                I = 0;
                                int end__507 = Units.Count - 1;
                                for (; I <= end__507; I += 1)
                                {
                                    var call__508 = Units[I];
                                    U = (Aow.Units.AoWTC.TTacticalCombatUnit)call__508;
                                    U.FCanFlee = FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker;
                                    var call__509 = U.GetMoves();
                                    if (call__509 < FMinMoves)
                                    {
                                        var call__510 = U.GetMoves();
                                        FMinMoves = call__510;
                                    }

                                    {
                                        J = 0;
                                        int end__511 = Enemies.Count - 1;
                                        for (; J <= end__511; J += 1)
                                        {
                                            var call__512 = Enemies[J];
                                            Target = (Aow.Units.AoWTC.TTacticalCombatUnit)call__512;
                                            bool bool__513 = !FIgnoreWalls;
                                            if (bool__513)
                                            {
                                                var call__514 = Target.GetSide();
                                                var call__515 = U.GetSide();
                                                bool__513 = (call__514 != call__515);
                                            }

                                            if (bool__513)
                                            {
                                                var call__516 = Target.FUnitHS.GetYhx();
                                                var call__517 = Target.FUnitHS.GetXhx();
                                                var call__518 = U.FUnitHS.GetYhx();
                                                var call__519 = U.FUnitHS.GetXhx();
                                                var call__520 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__519, call__518, call__517, call__516);
                                                K = call__520;
                                                {
                                                    L = 22;
                                                    byte end__521 = 169;
                                                    for (; L <= end__521; L += 1)
                                                    {
                                                        if (Target.GetAbilityEnabled(L))
                                                        {
                                                            if ((L == 52) && (Target.FUnit is Aow.Units.AoWE.THero))
                                                            {
                                                                {
                                                                    N = 0;
                                                                    byte end__522 = 130;
                                                                    for (; N <= end__522; N += 1)
                                                                    {
                                                                        bool bool__523 = (Target.FUnit.FPlayer > 0) && (Aow.Units.AoWTC.AoWTC.SpellTypes[N] != Aow.Units.AoWTC.TfxType.fxNone);
                                                                        if (bool__523)
                                                                        {
                                                                            var call__524 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Target.FUnit.FPlayer);
                                                                            var call__525 = call__524.FMagicControl.FSpells.IndexOf(N);
                                                                            bool__523 = (call__525 != -1);
                                                                        }

                                                                        bool bool__526 = bool__523;
                                                                        if (bool__526)
                                                                        {
                                                                            var call__527 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(N);
                                                                            var call__528 = ((Aow.Units.AoWE.THero)Target.FUnit).CanCastSpellInstantly(N, call__527.FBaseManaCost, ref Reason);
                                                                            bool__526 = call__528;
                                                                        }

                                                                        if (bool__526)
                                                                        {
                                                                            var call__529 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(N);
                                                                            call__529.GetCombatInfo(Target.FUnit, ref SpellInfo);
                                                                            if (SpellInfo.TargetTypes.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy) && ((K <= SpellInfo.Range) || (SpellInfo.Range == -1)))
                                                                            {
                                                                                var call__530 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(N);
                                                                                var call__531 = call__530.GetCombatDamageValue(Target, U);
                                                                                Value = call__531;
                                                                                if (Value > 0)
                                                                                {
                                                                                    if (TSet<Aow.Units.AoWTC.TfxType>.Of(Aow.Units.AoWTC.TfxType.fxArrow, Aow.Units.AoWTC.TfxType.fxRock, Aow.Units.AoWTC.TfxType.fxComet, Aow.Units.AoWTC.TfxType.fxBreath, Aow.Units.AoWTC.TfxType.fxCometBall, Aow.Units.AoWTC.TfxType.fxRay).Contains(Aow.Units.AoWTC.AoWTC.SpellTypes[N]))
                                                                                    {
                                                                                        var call__532 = Target.FUnitHS.GetXhx();
                                                                                        var call__533 = Target.FUnitHS.GetYhx();
                                                                                        var call__534 = U.FUnitHS.GetXhx();
                                                                                        var call__535 = U.FUnitHS.GetYhx();
                                                                                        EvalPath(ref Value, U, L, N, call__532, call__533, call__534, call__535, false, Target, null);
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
                                                            else
                                                            {
                                                                if ((Aow.Units.AoWTC.AoWTC.AbilTypes[L] != Aow.Units.AoWTC.TfxType.fxNone) && (Aow.Units.AoWTC.AoWTC.AbilTypes[L] != Aow.Units.AoWTC.TfxType.fxTouch))
                                                                {
                                                                    var call__536 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(L);
                                                                    call__536.GetCombatInfo(Target.FUnit, ref AIInfo);
                                                                    if ((((byte)AIInfo.TargetTypes.Image) & 2) != 0)
                                                                    {
                                                                        var call__537 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(L);
                                                                        var call__538 = call__537.GetCombatMode(Target.FUnit);
                                                                        if (call__538 == Aow.Units.AoWE.TCombatMode.cmLongRange)
                                                                        {
                                                                            N = 12;
                                                                        }
                                                                        else
                                                                        {
                                                                            var call__539 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(L);
                                                                            var call__540 = call__539.GetCombatMode(Target.FUnit);
                                                                            if (call__540 == Aow.Units.AoWE.TCombatMode.cmMediumRange)
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
                                                                            var call__541 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(L);
                                                                            var call__542 = call__541.tcGetDamageValue(Target, U);
                                                                            Value = call__542;
                                                                            if (Value > 0)
                                                                            {
                                                                                if (TSet<Aow.Units.AoWTC.TfxType>.Of(Aow.Units.AoWTC.TfxType.fxArrow, Aow.Units.AoWTC.TfxType.fxRock, Aow.Units.AoWTC.TfxType.fxComet, Aow.Units.AoWTC.TfxType.fxBreath, Aow.Units.AoWTC.TfxType.fxCometBall, Aow.Units.AoWTC.TfxType.fxRay).Contains(Aow.Units.AoWTC.AoWTC.AbilTypes[L]))
                                                                                {
                                                                                    var call__543 = Target.FUnitHS.GetXhx();
                                                                                    var call__544 = Target.FUnitHS.GetYhx();
                                                                                    var call__545 = U.FUnitHS.GetXhx();
                                                                                    var call__546 = U.FUnitHS.GetYhx();
                                                                                    EvalPath(ref Value, U, L, 0, call__543, call__544, call__545, call__546, false, Target, null);
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
                                    }
                                }
                            }

                            CurrentMove += 1;
                            FUnitIndex = 0;
                            FUnitLimit = 0;
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error - TCAI", (AnsiString)"Error during Create Unit List", (int)0);
                        }

                        break;
                    case 1:
                        try
                        {
                            while ((FUnitIndex < Units.Count) && (FUnitLimit < 1))
                            {
                                var call__547 = Units[FUnitIndex];
                                U = (Aow.Units.AoWTC.TTacticalCombatUnit)call__547;
                                if (!U.GetLocked())
                                {
                                    Abilities = new Aow.Rtl.TList();
                                    try
                                    {
                                        {
                                            J = 0;
                                            byte end__549 = 169;
                                            for (; J <= end__549; J += 1)
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

                                                    if ((J == 52) && (U.FUnit is Aow.Units.AoWE.THero))
                                                    {
                                                        {
                                                            K = 0;
                                                            byte end__550 = 130;
                                                            for (; K <= end__550; K += 1)
                                                            {
                                                                bool bool__551 = (U.FUnit.FPlayer > 0) && (Aow.Units.AoWTC.AoWTC.SpellTypes[K] != Aow.Units.AoWTC.TfxType.fxNone);
                                                                if (bool__551)
                                                                {
                                                                    var call__552 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(U.FUnit.FPlayer);
                                                                    var call__553 = call__552.FMagicControl.FSpells.IndexOf(K);
                                                                    bool__551 = (call__553 != -1);
                                                                }

                                                                bool bool__554 = bool__551;
                                                                if (bool__554)
                                                                {
                                                                    var call__555 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(K);
                                                                    var call__556 = ((Aow.Units.AoWE.THero)U.FUnit).CanCastSpellInstantly(K, call__555.FBaseManaCost, ref Reason);
                                                                    bool__554 = call__556;
                                                                }

                                                                if (bool__554)
                                                                {
                                                                    var call__557 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(K);
                                                                    call__557.GetCombatInfo(U.FUnit, ref SpellInfo);
                                                                    if (SpellInfo.TargetTypes.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy))
                                                                    {
                                                                        var call__558 = Aow.Units.AoWTC.TTCAbility.Create();
                                                                        Abilities.Add(call__558);
                                                                        var call__559 = Abilities[Abilities.Count - 1];
                                                                        ((Aow.Units.AoWTC.TTCAbility)call__559).Ability = J;
                                                                        var call__560 = Abilities[Abilities.Count - 1];
                                                                        ((Aow.Units.AoWTC.TTCAbility)call__560).Spell = K;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Aow.Units.AoWTC.AoWTC.AbilTypes[J] != Aow.Units.AoWTC.TfxType.fxNone)
                                                        {
                                                            var call__561 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(J);
                                                            call__561.GetCombatInfo(U.FUnit, ref AIInfo);
                                                            if ((((byte)AIInfo.TargetTypes.Image) & 2) != 0)
                                                            {
                                                                var call__562 = Aow.Units.AoWTC.TTCAbility.Create();
                                                                Abilities.Add(call__562);
                                                                var call__563 = Abilities[Abilities.Count - 1];
                                                                ((Aow.Units.AoWTC.TTCAbility)call__563).Ability = J;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (System.Exception)
                                    {
                                        var call__548 = U.FUnit.GetName();
                                        Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Eval Enemies", AnsiString.FromPChar(Ptr<byte>.Unbox(((AnsiString)"Error - TCAI ") + call__548)), (int)0);
                                    }

                                    Settings = Aow.Units.AoWTC.TCombatMoveControlSettings.Create(null);
                                    if (U.FMovePoints > 0)
                                    {
                                        var call__564 = U.FUnitHS.GetYhx();
                                        var call__565 = U.FUnitHS.GetXhx();
                                        Settings.SetSourcePos(call__565, call__564, 0);
                                        Settings.FArmyHS = U.FUnitHS;
                                        Settings.FMoveFlags = TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmMelee, Aow.Units.AoWTC.TCombatMoveModeElement.mmaiMove, Aow.Units.AoWTC.TCombatMoveModeElement.mmaiWall);
                                        U.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                        var call__566 = U.GetPlayer();
                                        Settings.FPlayer = unchecked((sbyte)call__566);
                                        Settings.FMovePointsMax = 200;
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.ProcessMove(Settings);
                                    }

                                    bool bool__567 = (Abilities.Count > 0);
                                    if (!bool__567)
                                    {
                                        var call__568 = Aow.Units.AoWE.AoWE.MeleeDamageTypes(U);
                                        bool__567 = (call__568 != TSet<Aow.Units.AoWE.TDamageType>.Of());
                                    }

                                    if (bool__567)
                                    {
                                        {
                                            J = 0;
                                            int end__569 = Enemies.Count - 1;
                                            for (; J <= end__569; J += 1)
                                            {
                                                var call__570 = Enemies[J];
                                                Target = (Aow.Units.AoWTC.TTacticalCombatUnit)call__570;
                                                if (!(U.FUnit is Aow.Units.AoWE.THero) || (FHeroMoves == 0))
                                                {
                                                    var call__571 = Target.GetHits();
                                                    var call__572 = Target.GetHitPoints();
                                                    var call__573 = Target.FUnitHS.GetYhx();
                                                    var call__574 = Target.FUnitHS.GetXhx();
                                                    CheckUnit(call__574, call__573, call__572, call__571, Settings, U, Target, Abilities, null);
                                                }
                                            }
                                        }
                                    }

                                    {
                                        I = 0;
                                        int end__575 = Abilities.Count - 1;
                                        for (; I <= end__575; I += 1)
                                        {
                                            var call__576 = Abilities[I];
                                            ((Aow.Rtl.TObject)call__576).Free();
                                        }
                                    }

                                    Abilities.Clear();
                                    try
                                    {
                                        {
                                            J = 22;
                                            byte end__578 = 169;
                                            for (; J <= end__578; J += 1)
                                            {
                                                if (U.GetAbilityEnabled(J))
                                                {
                                                    if ((J == 52) && (U.FUnit is Aow.Units.AoWE.THero))
                                                    {
                                                        {
                                                            K = 0;
                                                            byte end__579 = 130;
                                                            for (; K <= end__579; K += 1)
                                                            {
                                                                bool bool__580 = (Aow.Units.AoWTC.AoWTC.SpellTypes[K] != Aow.Units.AoWTC.TfxType.fxNone);
                                                                if (bool__580)
                                                                {
                                                                    var call__581 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(U.FUnit.FPlayer);
                                                                    var call__582 = call__581.FMagicControl.FSpells.IndexOf(K);
                                                                    bool__580 = (call__582 != -1);
                                                                }

                                                                bool bool__583 = bool__580;
                                                                if (bool__583)
                                                                {
                                                                    var call__584 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(K);
                                                                    var call__585 = ((Aow.Units.AoWE.THero)U.FUnit).CanCastSpellInstantly(K, call__584.FBaseManaCost, ref Reason);
                                                                    bool__583 = call__585;
                                                                }

                                                                if (bool__583)
                                                                {
                                                                    var call__586 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(K);
                                                                    call__586.GetCombatInfo(U.FUnit, ref SpellInfo);
                                                                    if (SpellInfo.TargetTypes.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend))
                                                                    {
                                                                        var call__587 = Aow.Units.AoWTC.TTCAbility.Create();
                                                                        Abilities.Add(call__587);
                                                                        var call__588 = Abilities[Abilities.Count - 1];
                                                                        ((Aow.Units.AoWTC.TTCAbility)call__588).Ability = J;
                                                                        var call__589 = Abilities[Abilities.Count - 1];
                                                                        ((Aow.Units.AoWTC.TTCAbility)call__589).Spell = K;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Aow.Units.AoWTC.AoWTC.AbilTypes[J] != Aow.Units.AoWTC.TfxType.fxNone)
                                                        {
                                                            var call__590 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(J);
                                                            call__590.GetCombatInfo(U.FUnit, ref AIInfo);
                                                            if ((((byte)AIInfo.TargetTypes.Image) & 1) != 0)
                                                            {
                                                                var call__591 = Aow.Units.AoWTC.TTCAbility.Create();
                                                                Abilities.Add(call__591);
                                                                var call__592 = Abilities[Abilities.Count - 1];
                                                                ((Aow.Units.AoWTC.TTCAbility)call__592).Ability = J;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (System.Exception)
                                    {
                                        var call__577 = U.FUnit.GetName();
                                        Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Eval Friends", AnsiString.FromPChar(Ptr<byte>.Unbox(((AnsiString)"Error - TCAI ") + call__577)), (int)0);
                                    }

                                    if (Abilities.Count > 0)
                                    {
                                        {
                                            J = 0;
                                            int end__593 = Units.Count - 1;
                                            for (; J <= end__593; J += 1)
                                            {
                                                var call__594 = Units[J];
                                                Target = (Aow.Units.AoWTC.TTacticalCombatUnit)call__594;
                                                if (!(U.FUnit is Aow.Units.AoWE.THero) || (FHeroMoves == 0))
                                                {
                                                    var call__595 = Target.GetHits();
                                                    var call__596 = Target.GetHitPoints();
                                                    var call__597 = Target.FUnitHS.GetYhx();
                                                    var call__598 = Target.FUnitHS.GetXhx();
                                                    CheckUnit(call__598, call__597, call__596, call__595, Settings, U, Target, Abilities, null);
                                                }
                                            }
                                        }
                                    }

                                    Settings.Free();
                                    {
                                        I = 0;
                                        int end__599 = Abilities.Count - 1;
                                        for (; I <= end__599; I += 1)
                                        {
                                            var call__600 = Abilities[I];
                                            ((Aow.Rtl.TObject)call__600).Free();
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
                                try
                                {
                                    if (((Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallType != 0) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall != null)) && (FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker))
                                    {
                                        {
                                            J = 0;
                                            int end__601 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FWalls.Count - 1;
                                            for (; J <= end__601; J += 1)
                                            {
                                                var call__602 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FWalls[J];
                                                bool bool__603 = (((Aow.Units.CityWall.TCityWall)call__602).FHitPoints > 0);
                                                if (bool__603)
                                                {
                                                    var call__604 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FWalls[J];
                                                    var call__605 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FWalls[J];
                                                    bool__603 = (((Aow.Units.CityWall.TCityWall)call__604).FHitPoints < Aow.Units.AoWTC.AoWTC.WallMaxHP[((Aow.Units.CityWall.TCityWall)call__605).FWallType - 1]);
                                                }

                                                if (bool__603)
                                                {
                                                    Found = true;
                                                    K = 0;
                                                    while (Found && (K < Walls.Count))
                                                    {
                                                        var call__606 = Walls[K];
                                                        var call__607 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FWalls[J];
                                                        if (call__606 == call__607)
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
                                                        var call__608 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FWalls[J];
                                                        Walls.Add(call__608);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (((Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallType == 0) || (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall == null)) || (FSide == Aow.Units.AoWE.TCombatSideIndex.csDefender))
                                    {
                                        CurrentMove += 1;
                                    }

                                    FUnitIndex = 0;
                                    Aow.Units.AoWE.AoWE.AoWEngine.OutputDebugStr((((AnsiString)"AI calculated ") + Aow.Rtl.Rtl.IntToStr(Units.Count * Enemies.Count)) + ((AnsiString)" Attack paths."));
                                    Aow.Units.AoWE.AoWE.AoWEngine.OutputDebugStr((((AnsiString)"AI calculated ") + Aow.Rtl.Rtl.IntToStr(Moves.Count)) + ((AnsiString)" Moves."));
                                }
                                catch (System.Exception)
                                {
                                    Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error - TCAI ", (AnsiString)"Error during Eval Ending", (int)0);
                                }
                            }

                            FUnitLimit = 0;
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error - TCAI", (AnsiString)"Error during Create Move List", (int)0);
                        }

                        break;
                    case 2:
                        try
                        {
                            while ((FUnitIndex < Units.Count) && (FUnitLimit < 1))
                            {
                                var call__609 = Units[FUnitIndex];
                                U = (Aow.Units.AoWTC.TTacticalCombatUnit)call__609;
                                if (!U.GetLocked())
                                {
                                    Abilities = new Aow.Rtl.TList();
                                    {
                                        J = 0;
                                        byte end__610 = 169;
                                        for (; J <= end__610; J += 1)
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

                                                if ((J == 52) && (U.FUnit is Aow.Units.AoWE.THero))
                                                {
                                                    if (U.FUnit.FPlayer > 0)
                                                    {
                                                        {
                                                            K = 0;
                                                            byte end__611 = 130;
                                                            for (; K <= end__611; K += 1)
                                                            {
                                                                bool bool__612 = (Aow.Units.AoWTC.AoWTC.SpellTypes[K] != Aow.Units.AoWTC.TfxType.fxNone);
                                                                if (bool__612)
                                                                {
                                                                    var call__613 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(U.FUnit.FPlayer);
                                                                    var call__614 = call__613.FMagicControl.FSpells.IndexOf(K);
                                                                    bool__612 = (call__614 != -1);
                                                                }

                                                                bool bool__615 = bool__612;
                                                                if (bool__615)
                                                                {
                                                                    var call__616 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(K);
                                                                    var call__617 = ((Aow.Units.AoWE.THero)U.FUnit).CanCastSpellInstantly(K, call__616.FBaseManaCost, ref Reason);
                                                                    bool__615 = call__617;
                                                                }

                                                                if (bool__615)
                                                                {
                                                                    var call__618 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(K);
                                                                    call__618.GetCombatInfo(U.FUnit, ref SpellInfo);
                                                                    if (SpellInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtWall))
                                                                    {
                                                                        FFlee = false;
                                                                        var call__619 = Aow.Units.AoWTC.TTCAbility.Create();
                                                                        Abilities.Add(call__619);
                                                                        var call__620 = Abilities[Abilities.Count - 1];
                                                                        ((Aow.Units.AoWTC.TTCAbility)call__620).Ability = J;
                                                                        var call__621 = Abilities[Abilities.Count - 1];
                                                                        ((Aow.Units.AoWTC.TTCAbility)call__621).Spell = K;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (Aow.Units.AoWTC.AoWTC.AbilTypes[J] != Aow.Units.AoWTC.TfxType.fxNone)
                                                    {
                                                        var call__622 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(J);
                                                        call__622.GetCombatInfo(U.FUnit, ref AIInfo);
                                                        if (AIInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtWall))
                                                        {
                                                            FFlee = false;
                                                            var call__623 = Aow.Units.AoWTC.TTCAbility.Create();
                                                            Abilities.Add(call__623);
                                                            var call__624 = Abilities[Abilities.Count - 1];
                                                            ((Aow.Units.AoWTC.TTCAbility)call__624).Ability = J;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (Abilities.Count > 0)
                                    {
                                        Settings = Aow.Units.AoWTC.TCombatMoveControlSettings.Create(null);
                                        if (U.FMovePoints > 0)
                                        {
                                            var call__625 = U.FUnitHS.GetYhx();
                                            var call__626 = U.FUnitHS.GetXhx();
                                            Settings.SetSourcePos(call__626, call__625, 0);
                                            Settings.FArmyHS = U.FUnitHS;
                                            Settings.FMoveFlags = TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmMelee, Aow.Units.AoWTC.TCombatMoveModeElement.mmaiMove, Aow.Units.AoWTC.TCombatMoveModeElement.mmaiWall);
                                            var call__627 = U.GetAbilityEnabled(65);
                                            bool bool__628 = call__627;
                                            if (!bool__628)
                                            {
                                                var call__629 = U.GetAbilityEnabled(117);
                                                bool__628 = call__629;
                                            }

                                            if (bool__628)
                                            {
                                                Settings.FMoveFlags = Settings.FMoveFlags + TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmWallCrushing);
                                            }

                                            U.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                            var call__630 = U.GetPlayer();
                                            Settings.FPlayer = unchecked((sbyte)call__630);
                                            Settings.FMovePointsMax = 200;
                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.ProcessMove(Settings);
                                        }

                                        {
                                            J = 0;
                                            int end__631 = Walls.Count - 1;
                                            for (; J <= end__631; J += 1)
                                            {
                                                var call__632 = Walls[J];
                                                Wall = (Aow.Units.CityWall.TCityWall)call__632;
                                                if (!(U.FUnit is Aow.Units.AoWE.THero) || (FHeroMoves == 0))
                                                {
                                                    var call__633 = Wall.GetYhx();
                                                    var call__634 = Wall.GetXhx();
                                                    CheckUnit(call__634, call__633, Wall.FHitPoints, Aow.Units.AoWTC.AoWTC.WallMaxHP[Wall.FWallType - 1] * 2, Settings, U, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall, Abilities, Wall);
                                                }
                                            }
                                        }

                                        if (Walls.Count == 0)
                                        {
                                            L = 100;
                                            Wall = null;
                                            {
                                                J = 0;
                                                int end__635 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FWalls.Count - 1;
                                                for (; J <= end__635; J += 1)
                                                {
                                                    var call__636 = U.FUnitHS.GetYhx();
                                                    var call__637 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FWalls[J];
                                                    var call__638 = ((Aow.Units.CityWall.TCityWall)call__637).GetYhx();
                                                    var call__639 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FWalls[J];
                                                    var call__640 = ((Aow.Units.CityWall.TCityWall)call__639).GetXhx();
                                                    var call__641 = U.FUnitHS.GetXhx();
                                                    var call__642 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__640, call__638, call__641, call__636);
                                                    N = call__642;
                                                    var call__643 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FWalls[J];
                                                    if ((((Aow.Units.CityWall.TCityWall)call__643).FHitPoints > 0) && (N < L))
                                                    {
                                                        var call__644 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FWalls[J];
                                                        Wall = (Aow.Units.CityWall.TCityWall)call__644;
                                                        L = N;
                                                    }
                                                }
                                            }

                                            if (Wall != null)
                                            {
                                                var call__645 = Wall.GetYhx();
                                                var call__646 = Wall.GetXhx();
                                                CheckUnit(call__646, call__645, Wall.FHitPoints * 2, Aow.Units.AoWTC.AoWTC.WallMaxHP[Wall.FWallType - 1], Settings, U, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWall, Abilities, Wall);
                                            }
                                        }

                                        Settings.Free();
                                        {
                                            I = 0;
                                            int end__647 = Abilities.Count - 1;
                                            for (; I <= end__647; I += 1)
                                            {
                                                var call__648 = Abilities[I];
                                                ((Aow.Rtl.TObject)call__648).Free();
                                            }
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
                                Aow.Units.AoWE.AoWE.AoWEngine.OutputDebugStr((((AnsiString)"AI calculated ") + Aow.Rtl.Rtl.IntToStr(Units.Count * Enemies.Count)) + ((AnsiString)" Attack paths."));
                                Aow.Units.AoWE.AoWE.AoWEngine.OutputDebugStr((((AnsiString)"AI calculated ") + Aow.Rtl.Rtl.IntToStr(Moves.Count)) + ((AnsiString)" Moves."));
                            }

                            FUnitLimit = 0;
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error - TCAI", (AnsiString)"Error during Create WallCrush List", (int)0);
                        }

                        break;
                    case 3:
                        try
                        {
                            Bonuses = new Aow.Rtl.TList();
                            {
                                I = 0;
                                int end__649 = Units.Count - 1;
                                for (; I <= end__649; I += 1)
                                {
                                    Bonuses.Add(null);
                                }
                            }

                            while ((FUnitIndex < Moves.Count) && (FUnitLimit < 50))
                            {
                                var call__650 = Moves[FUnitIndex];
                                Move = (Aow.Units.AoWTC.TaiMove)call__650;
                                if (Move.MoveType != Aow.Units.AoWTC.TaiMoveType.aiFake)
                                {
                                    {
                                        J = 0;
                                        int end__651 = Units.Count - 1;
                                        for (; J <= end__651; J += 1)
                                        {
                                            Bonuses[J] = null;
                                        }
                                    }

                                    {
                                        J = 0;
                                        int end__652 = Moves.Count - 1;
                                        for (; J <= end__652; J += 1)
                                        {
                                            var call__653 = Moves[J];
                                            OtherMove = (Aow.Units.AoWTC.TaiMove)call__653;
                                            if (Move.UnitHS.FUnit != OtherMove.UnitHS.FUnit)
                                            {
                                                var call__654 = Aow.Units.HSEngine.HSEngine.dHXtoRad(Move.X, Move.Y, OtherMove.X, OtherMove.Y);
                                                K = (call__654 * 4) - OtherMove.UnitHS.FUnit.FMovePoints;
                                                if (K < 4)
                                                {
                                                    K = 4;
                                                }

                                                var call__655 = OtherMove.UnitHS.FUnit.GetMoves();
                                                L = call__655 * (OtherMove.Value / (K * 10));
                                                var call__656 = Units.IndexOf(OtherMove.UnitHS.FUnit);
                                                K = call__656;
                                                var call__657 = Bonuses[K];
                                                if (((int)Aow.Rtl.Rtl.ObjectToInt((Aow.Rtl.TObject)call__657)) < L)
                                                {
                                                    Bonuses[K] = Aow.Rtl.Rtl.IntToObject((int)L);
                                                }
                                            }
                                        }
                                    }

                                    {
                                        J = 0;
                                        int end__658 = Units.Count - 1;
                                        for (; J <= end__658; J += 1)
                                        {
                                            if ((Move.GroupValue > -1000000) && (Move.GroupValue < 1000000))
                                            {
                                                var call__659 = Bonuses[J];
                                                Move.GroupValue += (int)Aow.Rtl.Rtl.ObjectToInt((Aow.Rtl.TObject)call__659);
                                            }
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
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error - TCAI", (AnsiString)"Error during Build Group Bonuses", (int)0);
                        }

                        break;
                    case 4:
                        try
                        {
                            {
                                I = 0;
                                int end__660 = Units.Count - 1;
                                for (; I <= end__660; I += 1)
                                {
                                    var call__661 = Units[I];
                                    U = (Aow.Units.AoWTC.TTacticalCombatUnit)call__661;
                                    if (((U.FUnit is Aow.Units.AoWE.THero) && (FHeroMoves == 0)) && (U.FMovePoints > 0))
                                    {
                                        var call__662 = U.FUnitHS.GetYhx();
                                        var call__663 = U.FUnitHS.GetXhx();
                                        var call__664 = UnitSafe(U, call__663, call__662);
                                        K = call__664;
                                        Value = K;
                                        N = -1;
                                        {
                                            J = 0;
                                            int end__665 = Units.Count - 1;
                                            for (; J <= end__665; J += 1)
                                            {
                                                var call__666 = Units[J];
                                                Target = (Aow.Units.AoWTC.TTacticalCombatUnit)call__666;
                                                if (U != Target)
                                                {
                                                    var call__667 = Target.FUnitHS.GetYhx();
                                                    var call__668 = Target.FUnitHS.GetXhx();
                                                    var call__669 = UnitSafe(U, call__668, call__667);
                                                    L = call__669;
                                                    if (L > K)
                                                    {
                                                        N = J;
                                                        K = L;
                                                    }
                                                }
                                            }
                                        }

                                        if (K > Value)
                                        {
                                            Settings = Aow.Units.AoWTC.TCombatMoveControlSettings.Create(null);
                                            // { Original uses X for both coordinates. }
                                            var call__670 = U.FUnitHS.GetXhx();
                                            var call__671 = U.FUnitHS.GetXhx();
                                            Settings.SetSourcePos(call__671, call__670, 0);
                                            var call__672 = Units[N];
                                            var call__673 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__672).FUnitHS.GetYhx();
                                            var call__674 = Units[N];
                                            var call__675 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__674).FUnitHS.GetXhx();
                                            Settings.SetDestinationPos(call__675, call__673, 0);
                                            Settings.FArmyHS = U.FUnitHS;
                                            Settings.FMoveFlags = TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmMelee, Aow.Units.AoWTC.TCombatMoveModeElement.mmaiMove, Aow.Units.AoWTC.TCombatMoveModeElement.mmaiWall);
                                            U.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                            var call__676 = U.GetPlayer();
                                            Settings.FPlayer = unchecked((sbyte)call__676);
                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.ProcessMove(Settings);
                                            if (Settings.FPath.Count > 2)
                                            {
                                                Settings.FPath.Delete(0);
                                                var call__677 = Settings.FPath[0];
                                                var call__678 = Settings.FPath[0];
                                                CreateMove(Aow.Units.AoWTC.TaiMoveType.aiMove, call__678.X, call__677.Y, K, 0, 0, U, null);
                                            }

                                            Settings.Free();
                                        }
                                        else
                                        {
                                            if (FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker)
                                            {
                                                U.FCanFlee = true;
                                            }
                                        }
                                    }
                                }
                            }

                            if (FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker)
                            {
                                {
                                    I = 0;
                                    int end__679 = Units.Count - 1;
                                    for (; I <= end__679; I += 1)
                                    {
                                        var call__680 = Units[I];
                                        U = (Aow.Units.AoWTC.TTacticalCombatUnit)call__680;
                                        bool bool__681 = ((!(U.FUnit is Aow.Units.AoWE.THero) || (FHeroMoves == 0)) || ((U.FUnit is Aow.Units.AoWE.THero) && (Units.Count == 1)));
                                        if (bool__681)
                                        {
                                            var call__682 = U.GetEnabled();
                                            bool__681 = call__682;
                                        }

                                        bool bool__683 = bool__681;
                                        if (bool__683)
                                        {
                                            var call__684 = U.GetLocked();
                                            bool__683 = !call__684;
                                        }

                                        if ((bool__683 && (U.FMovePoints > 0)) && (FFlee || U.FCanFlee))
                                        {
                                            {
                                                J = 0;
                                                int end__685 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FFleeLocations.Count - 1;
                                                for (; J <= end__685; J += 1)
                                                {
                                                    var call__686 = U.FUnitHS.GetYhx();
                                                    var call__687 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FFleeLocations[J];
                                                    var call__688 = ((Aow.Units.HSEngine.THexagonSprite)call__687).GetYhx();
                                                    var call__689 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FFleeLocations[J];
                                                    var call__690 = ((Aow.Units.HSEngine.THexagonSprite)call__689).GetXhx();
                                                    var call__691 = U.FUnitHS.GetXhx();
                                                    var call__692 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__690, call__688, call__691, call__686);
                                                    var call__693 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FFleeLocations[J];
                                                    var call__694 = ((Aow.Units.HSEngine.THexagonSprite)call__693).GetYhx();
                                                    var call__695 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FFleeLocations[J];
                                                    var call__696 = ((Aow.Units.HSEngine.THexagonSprite)call__695).GetXhx();
                                                    CreateMove(Aow.Units.AoWTC.TaiMoveType.aiMove, call__696, call__694, 110000000 - (call__692 * 10), 0, 0, U, null);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            I = 0;
                            while (I < Moves.Count)
                            {
                                var call__697 = Moves[I];
                                Move = (Aow.Units.AoWTC.TaiMove)call__697;
                                switch (Move.MoveType)
                                {
                                    case Aow.Units.AoWTC.TaiMoveType.aiFake:
                                        Move.Free();
                                        Moves.Delete(I);
                                        break;
                                    default:
                                    {
                                        if ((Move.Value > -100000000) && (Move.Value < 100000000))
                                        {
                                            Move.Value += Move.GroupValue / 4;
                                        }

                                        I += 1;
                                    }

                                        break;
                                }
                            }

                            {
                                I = 1;
                                int end__698 = Moves.Count - 1;
                                for (; I <= end__698; I += 1)
                                {
                                    var call__699 = Moves[I];
                                    Move = (Aow.Units.AoWTC.TaiMove)call__699;
                                    J = 0;
                                    K = -1;
                                    while (J < I)
                                    {
                                        var call__700 = Moves[J];
                                        OtherMove = (Aow.Units.AoWTC.TaiMove)call__700;
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
                            }

                            CurrentMove += 1;
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error - TCAI", (AnsiString)"Error during Add Group Bonuses", (int)0);
                        }

                        break;
                    case 5:
                        try
                        {
                            Moved = false;
                            while ((Moves.Count > 0) && !Moved)
                            {
                                var call__701 = Moves[0];
                                Move = (Aow.Units.AoWTC.TaiMove)call__701;
                                switch (Move.MoveType)
                                {
                                    case Aow.Units.AoWTC.TaiMoveType.aiMove:
                                    case Aow.Units.AoWTC.TaiMoveType.aiMoveRanged:
                                    {
                                        var w__702 = Move.UnitHS;
                                        if (((Move.UnitHS.FUnit.FUnit is Aow.Units.AoWE.THero) || ((FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FRound == 1))) && (Move.UnitHS.FUnit.FMovePoints > FMinMoves))
                                        {
                                            Move.UnitHS.FUnit.FMovePoints = FMinMoves;
                                        }

                                        Settings = Aow.Units.AoWTC.TCombatMoveControlSettings.Create(null);
                                        var call__703 = w__702.GetLhx();
                                        var call__704 = w__702.GetYhx();
                                        var call__705 = w__702.GetXhx();
                                        Settings.SetSourcePos(call__705, call__704, call__703);
                                        Settings.SetDestinationPos(Move.X, Move.Y, 0);
                                        Settings.FArmyHS = Move.UnitHS;
                                        bool bool__706 = (Move.MoveType == Aow.Units.AoWTC.TaiMoveType.aiMoveRanged);
                                        if (!bool__706)
                                        {
                                            var call__707 = Aow.Units.AoWE.AoWE.MeleeDamageTypes(Move.UnitHS.FUnit);
                                            bool__706 = (call__707 == TSet<Aow.Units.AoWE.TDamageType>.Of());
                                        }

                                        if (bool__706)
                                        {
                                            Settings.FMoveFlags = TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmMoveRanged);
                                        }
                                        else
                                        {
                                            Settings.FMoveFlags = TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmMelee);
                                        }

                                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallType != 0)
                                        {
                                            bool bool__708 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallBreached;
                                            if (bool__708)
                                            {
                                                var call__709 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                var call__710 = call__709.Field[Move.X, Move.Y];
                                                var call__711 = call__710.FindOwnedChild(2229508);
                                                bool__708 = (call__711 == null);
                                            }

                                            bool bool__712 = bool__708;
                                            if (bool__712)
                                            {
                                                var call__713 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                var call__714 = call__713.Field[Move.X, Move.Y];
                                                var call__715 = call__714.FindOwnedChild(2229510);
                                                bool__712 = (call__715 == null);
                                            }

                                            if (!bool__712)
                                            {
                                                bool bool__716 = !FIgnoreWalls;
                                                if (!bool__716)
                                                {
                                                    var call__717 = ChargeFromPos((int)(((uint)Move.UnitHS.FUnit.FPosition) >> 4));
                                                    bool__716 = !call__717;
                                                }

                                                if (bool__716 || (FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker))
                                                {
                                                    Settings.FMoveFlags = Settings.FMoveFlags + TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiWall);
                                                }
                                            }
                                        }

                                        Move.UnitHS.FUnit.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                        var call__718 = Move.UnitHS.FUnit.GetPlayer();
                                        Settings.FPlayer = unchecked((sbyte)call__718);
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.ProcessMove(Settings);
                                        if ((Settings.FPath.Count < 2) && !Settings.FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiWall))
                                        {
                                            Settings.FMoveFlags = Settings.FMoveFlags + TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiWall);
                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.ProcessMove(Settings);
                                        }

                                        var call__719 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__720 = call__719.Field[Move.X, Move.Y];
                                        var call__721 = call__720.FindOwnedChild(2229508);
                                        Wall = (Aow.Units.CityWall.TCityWall)call__721;
                                        if (Wall == null)
                                        {
                                            var call__722 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                            var call__723 = call__722.Field[Move.X, Move.Y];
                                            var call__724 = call__723.FindOwnedChild(2229510);
                                            Wall = (Aow.Units.CityWall.TCityWall)call__724;
                                        }

                                        if (((Settings.FPath.Count == 2) && (Move.MoveType == Aow.Units.AoWTC.TaiMoveType.aiMoveRanged)) && (Wall != null))
                                        {
                                            Move.UnitHS.FUnit.FMovePoints = 0;
                                        }
                                        else
                                        {
                                            if (Settings.FPath.Count > 1)
                                            {
                                                Path = Settings.FPath;
                                                var call__725 = MakeTerrainPath(Settings, ref Path, ref TerrainCost, Move.UnitHS.FUnit, null, false, false, 0);
                                                PathCost = call__725;
                                                if ((Path.Count > 1) && (PathCost > -1))
                                                {
                                                    var call__726 = Path[Path.Count - 2];
                                                    bool bool__727 = (call__726.Flag > 0);
                                                    if (bool__727)
                                                    {
                                                        var call__728 = Path[Path.Count - 2];
                                                        bool__727 = (call__728.Flag <= Move.UnitHS.FUnit.FMovePoints);
                                                    }

                                                    if (bool__727)
                                                    {
                                                        if (Move.MoveType == Aow.Units.AoWTC.TaiMoveType.aiMoveRanged)
                                                        {
                                                            Value = ((Aow.Units.AoWE.TMovepath)Path).PosToMovePoints(0);
                                                            if (Value < Move.UnitHS.FUnit.FMovePoints)
                                                            {
                                                                Move.UnitHS.FUnit.FMovePoints = Value;
                                                            }
                                                        }

                                                        var call__729 = ((Aow.Units.AoWE.TMovepath)Path).MovePointsToPos(Move.UnitHS.FUnit.FMovePoints);
                                                        Value = call__729;
                                                        Value = ((Aow.Units.AoWE.TMovepath)Path).PosToMovePoints(Value);
                                                        if ((Value > 0) && (Value < Move.UnitHS.FUnit.FMovePoints))
                                                        {
                                                            Move.UnitHS.FUnit.FMovePoints = Value;
                                                        }

                                                        // { Original repeats X. }
                                                        Aow.Units.AoWE.AoWE.AoWEngine.OutputDebugStr(((((AnsiString)"AI moves to X: ") + Aow.Rtl.Rtl.IntToStr(Move.X)) + ((AnsiString)" Y: ")) + Aow.Rtl.Rtl.IntToStr(Move.X));
                                                        var call__730 = Aow.Units.CombatTE.CombatTE.MoveCombatUnit(Move.UnitHS.FUnit, (Aow.Units.AoWE.TMovepath)Path);
                                                        Moved = call__730;
                                                        if (Move.UnitHS.FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer)
                                                        {
                                                            var call__731 = Move.UnitHS.GetYhx();
                                                            var call__732 = Move.UnitHS.GetXhx();
                                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(call__732, call__731, 0);
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
                                    case Aow.Units.AoWTC.TaiMoveType.aiMelee:
                                        Settings = Aow.Units.AoWTC.TCombatMoveControlSettings.Create(null);
                                        var call__733 = Move.UnitHS.GetYhx();
                                        var call__734 = Move.UnitHS.GetXhx();
                                        Settings.SetSourcePos(call__734, call__733, 0);
                                        Settings.SetDestinationPos(Move.X, Move.Y, 0);
                                        Settings.FArmyHS = Move.UnitHS;
                                        Settings.FMoveFlags = TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmMelee);
                                        Move.UnitHS.FUnit.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                        var call__735 = Move.UnitHS.FUnit.GetPlayer();
                                        Settings.FPlayer = unchecked((sbyte)call__735);
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.ProcessMove(Settings);
                                        if (Settings.FPath.Count > 1)
                                        {
                                            Path = Settings.FPath;
                                            var call__736 = MakeTerrainPath(Settings, ref Path, ref TerrainCost, Move.UnitHS.FUnit, null, false, false, 1);
                                            PathCost = call__736;
                                            if ((Path.Count > 1) && (PathCost > -1))
                                            {
                                                var call__737 = Path[Path.Count - 2];
                                                if (call__737.Flag <= Move.UnitHS.FUnit.FMovePoints)
                                                {
                                                    // { Original repeats X in the debug text. }
                                                    Aow.Units.AoWE.AoWE.AoWEngine.OutputDebugStr(((((AnsiString)"AI touches at X: ") + Aow.Rtl.Rtl.IntToStr(Move.X)) + ((AnsiString)" Y: ")) + Aow.Rtl.Rtl.IntToStr(Move.X));
                                                    var call__738 = Move.UnitHS.MeleeMoveTC((Aow.Units.AoWE.TMovepath)Path);
                                                    Moved = call__738;
                                                    if (Move.UnitHS.FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer)
                                                    {
                                                        var call__739 = Move.UnitHS.GetYhx();
                                                        var call__740 = Move.UnitHS.GetXhx();
                                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(call__740, call__739, 0);
                                                    }
                                                }
                                            }
                                        }

                                        Settings.Free();
                                        break;
                                    case Aow.Units.AoWTC.TaiMoveType.aiTouch:
                                        var call__741 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Move.Ability);
                                        call__741.GetCombatInfo(Move.UnitHS.FUnit.FUnit, ref AIInfo);
                                        Settings = Aow.Units.AoWTC.TCombatMoveControlSettings.Create(null);
                                        var call__742 = Move.UnitHS.GetYhx();
                                        var call__743 = Move.UnitHS.GetXhx();
                                        Settings.SetSourcePos(call__743, call__742, 0);
                                        Settings.SetDestinationPos(Move.X, Move.Y, 0);
                                        Settings.FArmyHS = Move.UnitHS;
                                        Settings.FMoveFlags = TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmMelee);
                                        if (AIInfo.DamageTypes.Contains(Aow.Units.AoWE.TDamageType.dtWall))
                                        {
                                            Settings.FMoveFlags = Settings.FMoveFlags + TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmWallCrushing);
                                        }

                                        Move.UnitHS.FUnit.FUnit.CreateMovePointTable(ref Settings.FMovePointTable);
                                        var call__744 = Move.UnitHS.FUnit.GetPlayer();
                                        Settings.FPlayer = unchecked((sbyte)call__744);
                                        Move.UnitHS.FSelectedAbility = Move.Ability;
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.ProcessMove(Settings);
                                        if (Settings.FPath.Count > 1)
                                        {
                                            Path = Settings.FPath;
                                            var call__745 = MakeTerrainPath(Settings, ref Path, ref TerrainCost, Move.UnitHS.FUnit, null, false, false, 1);
                                            PathCost = call__745;
                                            if ((Path.Count > 1) && (PathCost > -1))
                                            {
                                                var call__746 = Path[Path.Count - 2];
                                                if (call__746.Flag <= Move.UnitHS.FUnit.FMovePoints)
                                                {
                                                    Move.UnitHS.FSelectedAbility = Move.Ability;
                                                    // { Original repeats X in the debug text. }
                                                    Aow.Units.AoWE.AoWE.AoWEngine.OutputDebugStr(((((AnsiString)"AI melees at X: ") + Aow.Rtl.Rtl.IntToStr(Move.X)) + ((AnsiString)" Y: ")) + Aow.Rtl.Rtl.IntToStr(Move.X));
                                                    var call__747 = Move.UnitHS.AbTouchMoveTC((Aow.Units.AoWE.TMovepath)Path);
                                                    Moved = call__747;
                                                    if (Move.UnitHS.FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer)
                                                    {
                                                        var call__748 = Move.UnitHS.GetYhx();
                                                        var call__749 = Move.UnitHS.GetXhx();
                                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(call__749, call__748, 0);
                                                    }
                                                }
                                            }
                                        }

                                        Settings.Free();
                                        break;
                                    case Aow.Units.AoWTC.TaiMoveType.aiRanged:
                                        Move.UnitHS.FSelectedAbility = Move.Ability;
                                        Move.UnitHS.FSelectedSpell = Move.Spell;
                                        // { Original repeats X in the debug text. }
                                        Aow.Units.AoWE.AoWE.AoWEngine.OutputDebugStr(((((AnsiString)"AI ranged at X: ") + Aow.Rtl.Rtl.IntToStr(Move.X)) + ((AnsiString)" Y: ")) + Aow.Rtl.Rtl.IntToStr(Move.X));
                                        var call__750 = Move.UnitHS.AbRangedTC(unchecked((ushort)Move.X), unchecked((ushort)Move.Y));
                                        Moved = call__750;
                                        if (Move.UnitHS.FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer)
                                        {
                                            var call__751 = Move.UnitHS.GetYhx();
                                            var call__752 = Move.UnitHS.GetXhx();
                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(call__752, call__751, 0);
                                        }

                                        break;
                                }

                                var call__753 = Moves[0];
                                ((Aow.Rtl.TObject)call__753).Free();
                                Moves.Delete(0);
                            }

                            if (!Moved)
                            {
                                Status = Aow.Units.AoWTC.TaiStatus.aiDone;
                            }

                            {
                                I = 0;
                                int end__754 = Moves.Count - 1;
                                for (; I <= end__754; I += 1)
                                {
                                    var call__755 = Moves[I];
                                    ((Aow.Rtl.TObject)call__755).Free();
                                }
                            }

                            Moves.Clear();
                            CurrentMove = 0;
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error - TCAI", (AnsiString)"Error during Execute Moves", (int)0);
                        }

                        break;
                }
            }
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
                        int end__756 = FRange - 2;
                        for (; I <= end__756; I += 1)
                        {
                            First += Count;
                            Count += 6;
                        }
                    }

                    Last = First + Count;
                    {
                        I = First;
                        int end__757 = Last - 1;
                        for (; I <= end__757; I += 1)
                        {
                            var call__758 = FSelection.GetTCUnitHS();
                            var call__759 = call__758.GetYhx();
                            var call__760 = FSelection.GetTCUnitHS();
                            var call__761 = call__760.GetXhx();
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__761, call__759, I, ref NX, ref NY);
                            Aow.Units.HSEngine.HSEngine.HXtoHP(NX, NY, ref PX, ref PY);
                            var call__762 = FTargetImages[2];
                            call__762.Show((PX - XhpSurf) - 8, (PY - YhpSurf) + 2, SurfaceDesc);
                        }
                    }
                }

                var call__763 = FSelection.FRangedPath.Count;
                if (call__763 > 0)
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

                    var call__764 = FSelection.GetTCUnitHS();
                    if (call__764.FSelectedAbility == 52)
                    {
                        var call__765 = FSelection.GetTCUnitHS();
                        EffectType = Aow.Units.AoWTC.AoWTC.SpellTypes[call__765.FSelectedSpell];
                    }
                    else
                    {
                        var call__766 = FSelection.GetTCUnitHS();
                        EffectType = Aow.Units.AoWTC.AoWTC.AbilTypes[call__766.FSelectedAbility];
                    }

                    switch (EffectType)
                    {
                        case Aow.Units.AoWTC.TfxType.fxBreath:
                            var call__767 = FSelection.FRangedPath.Count;
                            var call__768 = FSelection.FRangedPath[call__767 - 1];
                            var call__769 = FSelection.FRangedPath.Count;
                            var call__770 = FSelection.FRangedPath[call__769 - 1];
                            Aow.Units.HSEngine.HSEngine.HPtoHX(call__770.X, call__768.Y, ref X, ref Y);
                        {
                            I = 0;
                            byte end__771 = 11;
                            for (; I <= end__771; I += 1)
                            {
                                var call__772 = FSelection.GetTCUnitHS();
                                var call__773 = call__772.GetYhx();
                                var call__774 = FSelection.GetTCUnitHS();
                                var call__775 = call__774.GetXhx();
                                var call__776 = Aow.Units.HSEngine.HSEngine.dHXtoHN(call__775, call__773, X, Y);
                                var call__777 = FSelection.GetTCUnitHS();
                                var call__778 = call__777.GetYhx();
                                var call__779 = FSelection.GetTCUnitHS();
                                var call__780 = call__779.GetXhx();
                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__780, call__778, Aow.Units.AoWTC.AoWTC.BreathHN[((call__776 * 12) + I) - 12], ref NX, ref NY);
                                Aow.Units.HSEngine.HSEngine.HXtoHP(NX, NY, ref PX, ref PY);
                                var call__781 = FTargetImages[0];
                                call__781.ShowClipped((PX - 8) - XhpSurf, (PY + 2) - YhpSurf, SurfaceDesc);
                            }
                        }

                            break;
                        default:
                            var call__782 = FSelection.GetTCUnitHS();
                            switch (call__782.FSelectedAbility)
                            {
                                case 52:
                                    var call__783 = FSelection.GetTCUnitHS();
                                    switch (call__783.FSelectedSpell)
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
                                            byte end__784 = 18;
                                            for (; I <= end__784; I += 1)
                                            {
                                                var call__785 = FSelection.FRangedPath.Count;
                                                var call__786 = FSelection.FRangedPath[call__785 - 1];
                                                var call__787 = FSelection.FRangedPath.Count;
                                                var call__788 = FSelection.FRangedPath[call__787 - 1];
                                                Aow.Units.HSEngine.HSEngine.HPtoHX(call__788.X, call__786.Y, ref X, ref Y);
                                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                Aow.Units.HSEngine.HSEngine.HXtoHP(NX, NY, ref PX, ref PY);
                                                var call__789 = FTargetImages[0];
                                                call__789.ShowClipped((PX - 8) - XhpSurf, (PY + 2) - YhpSurf, SurfaceDesc);
                                            }
                                        }

                                            break;
                                        case 124:
                                        case 128:
                                        {
                                            I = 0;
                                            byte end__790 = 6;
                                            for (; I <= end__790; I += 1)
                                            {
                                                var call__791 = FSelection.FRangedPath.Count;
                                                var call__792 = FSelection.FRangedPath[call__791 - 1];
                                                var call__793 = FSelection.FRangedPath.Count;
                                                var call__794 = FSelection.FRangedPath[call__793 - 1];
                                                Aow.Units.HSEngine.HSEngine.HPtoHX(call__794.X, call__792.Y, ref X, ref Y);
                                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref NX, ref NY);
                                                Aow.Units.HSEngine.HSEngine.HXtoHP(NX, NY, ref PX, ref PY);
                                                var call__795 = FTargetImages[0];
                                                call__795.ShowClipped((PX - 8) - XhpSurf, (PY + 2) - YhpSurf, SurfaceDesc);
                                            }
                                        }

                                            break;
                                        default:
                                            var call__796 = FSelection.FRangedPath.Count;
                                            var call__797 = FSelection.FRangedPath[call__796 - 1];
                                            var call__798 = FSelection.FRangedPath.Count;
                                            var call__799 = FSelection.FRangedPath[call__798 - 1];
                                            var call__800 = FMiscImages[FFrame + 3];
                                            call__800.ShowClipped((call__799.X - 21) - XhpSurf, (call__797.Y - 20) - YhpSurf, SurfaceDesc);
                                            break;
                                    }

                                    break;
                                default:
                                    var call__801 = FSelection.FRangedPath.Count;
                                    var call__802 = FSelection.FRangedPath[call__801 - 1];
                                    var call__803 = FSelection.FRangedPath.Count;
                                    var call__804 = FSelection.FRangedPath[call__803 - 1];
                                    var call__805 = FMiscImages[FFrame + 3];
                                    call__805.ShowClipped((call__804.X - 21) - XhpSurf, (call__802.Y - 20) - YhpSurf, SurfaceDesc);
                                    break;
                            }

                            break;
                    }

                    {
                        I = 0;
                        int end__806 = FSelection.FRangedPath.Count - 1;
                        for (; I <= end__806; I += 1)
                        {
                            var call__807 = FSelection.FRangedPath[I];
                            var call__808 = FSelection.FRangedPath[I];
                            var call__809 = FMiscImages[2];
                            call__809.Show(call__808.X - XhpSurf, call__807.Y - YhpSurf, SurfaceDesc);
                        }
                    }

                    {
                        I = 0;
                        int end__810 = FSelection.FRangedPath.Count - 1;
                        for (; I <= end__810; I += 1)
                        {
                            var call__811 = FSelection.FRangedPath[I];
                            var call__812 = FSelection.FRangedPath[I];
                            var call__813 = FSelection.FRangedPath[I];
                            var call__814 = FMiscImages[0];
                            call__814.Show(call__813.X - XhpSurf, ((call__811.Y - call__812.Z) - 8) - YhpSurf, SurfaceDesc);
                        }
                    }

                    {
                        I = 0;
                        int end__815 = FSelection.FObstacles.Count - 1;
                        for (; I <= end__815; I += 1)
                        {
                            var call__816 = FSelection.FObstacles[I];
                            var call__817 = FSelection.FRangedPath[((Aow.Units.AoWTC.TROb)call__816).PathIndex];
                            var call__818 = FSelection.FObstacles[I];
                            var call__819 = FSelection.FRangedPath[((Aow.Units.AoWTC.TROb)call__818).PathIndex];
                            Aow.Units.HSEngine.HSEngine.HPtoHX(call__819.X, call__817.Y, ref NX, ref NY);
                            Aow.Units.HSEngine.HSEngine.HXtoHP(NX, NY, ref PX, ref PY);
                            var call__820 = FMiscImages[1];
                            call__820.Show((PX + 5) - XhpSurf, (PY + 5) - YhpSurf, SurfaceDesc);
                            Aow.Units.AoWE.AoWE.AoWEngine.FCombatFont.Color = 0;
                            var call__821 = FSelection.FObstacles[I];
                            Aow.Units.AoWE.AoWE.AoWEngine.FCombatFont.SetTextCentered((PX + 16) - XhpSurf, (PY + 12) - YhpSurf, Aow.Rtl.Rtl.IntToStr(((Aow.Units.AoWTC.TROb)call__821).Chance), SurfaceDesc);
                        }
                    }
                }
            }
        }

        // AoWTC.pas:4852
        public override void Activate()
        {
            base.Activate();
            var call__822 = Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TCombat\\Misc.ILB");
            FMiscImages = call__822;
            var call__823 = Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TargHex.ILB");
            FTargetImages = call__823;
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
            var call__824 = Aow.Units.aowFX.TXYZList.Create();
            FPath = call__824;
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
            Aow.Units.GFXE.TSurfaceDesc SurfaceDesc__value = SurfaceDesc;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int Index = default;
            Aow.Units.ImageLib.TImageSequence FirstSequence = default;
            int I = default;
            int J = default;
            void SelectSequence(int Number)
            {
                if (FAbilityIndex == 52)
                {
                    var call__825 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                    var call__826 = call__825.FCastImages.ImageSequences[Number];
                    Sequence = call__826;
                }
                else
                {
                    var call__827 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                    var call__828 = call__827.FImageSequences.ImageSequences[Number];
                    Sequence = call__828;
                }
            }

            void SelectFrame()
            {
                if (Sequence != null)
                {
                    var call__829 = Sequence.GetUnloopedFrameIndex(FFrame);
                    Index = call__829;
                }

                FirstSequence = Sequence;
            }

            void SelectDamageSequence()
            {
                int N = default;
                switch (FEffectType)
                {
                    case Aow.Units.AoWTC.TfxType.fxDamLightning:
                        N = 40;
                        break;
                    case Aow.Units.AoWTC.TfxType.fxDamFire:
                        N = 41;
                        break;
                    case Aow.Units.AoWTC.TfxType.fxDamPoison:
                        N = 42;
                        break;
                    case Aow.Units.AoWTC.TfxType.fxDamMagic:
                        N = 43;
                        break;
                    case Aow.Units.AoWTC.TfxType.fxDamHoly:
                        N = 44;
                        break;
                    case Aow.Units.AoWTC.TfxType.fxDamDeath:
                        N = 45;
                        break;
                    default:
                        N = 46;
                        break;
                }

                var call__830 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[N];
                Sequence = call__830;
            }

            void ShowMovingLoop()
            {
                while (Sequence != null)
                {
                    var call__831 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                    Index = call__831;
                    bool bool__832 = (Index != -1);
                    if (bool__832)
                    {
                        var call__833 = Sequence.ImageLibrary.ImageExist(Index);
                        bool__832 = call__833;
                    }

                    if (bool__832)
                    {
                        var call__834 = FPath[FFrame];
                        var call__835 = FPath[FFrame];
                        var call__836 = FPath[FFrame];
                        var call__837 = Sequence.ImageLibrary[Index];
                        call__837.ShowClipped((call__836.X - XhpSurf) + Sequence.dXshift, ((call__834.Y - call__835.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                    }

                    Sequence = Sequence.Next;
                }
            }

            void ShowSecondaryFrame()
            {
                while (Sequence != null)
                {
                    var call__838 = Sequence.GetUnloopedFrameIndex(FVariant);
                    Index = call__838;
                    bool bool__839 = (Index != -1);
                    if (bool__839)
                    {
                        var call__840 = Sequence.ImageLibrary.ImageExist(Index);
                        bool__839 = call__840;
                    }

                    if (bool__839)
                    {
                        var call__841 = FPath[FFrame];
                        var call__842 = FPath[FFrame];
                        var call__843 = FPath[FFrame];
                        var call__844 = Sequence.ImageLibrary[Index];
                        call__844.ShowClipped((call__843.X - XhpSurf) + Sequence.dXshift, ((call__841.Y - call__842.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                    }

                    Sequence = Sequence.Next;
                }
            }

            void ShowTrail()
            {
                int N = default;
                int LastFrame = default;
                int Half = default;
                if (Sequence != null)
                {
                    LastFrame = Sequence.GetTotalSequenceFrames() - 1;
                    {
                        N = 0;
                        int end__845 = LastFrame;
                        for (; N <= end__845; N += 1)
                        {
                            Half = N / 2;
                            bool bool__846 = ((((FFrame - 9) + Half) + 5) > -1);
                            if (bool__846)
                            {
                                var call__847 = FPath.Count;
                                bool__846 = ((((FFrame - 9) + Half) + 5) < call__847);
                            }

                            if (bool__846)
                            {
                                Index = Sequence.GetUnloopedFrameIndex(LastFrame - N);
                                bool bool__848 = (Index != -1);
                                if (bool__848)
                                {
                                    var call__849 = Sequence.ImageLibrary.ImageExist(Index);
                                    bool__848 = call__849;
                                }

                                if (bool__848)
                                {
                                    var call__850 = FPath.Count;
                                    if (((((FFrame - 9) + Half) + 5) == (call__850 - 1)) || ((Half * 2) == N))
                                    {
                                        var call__851 = FPath[((FFrame - 9) + Half) + 5];
                                        var call__852 = FPath[((FFrame - 9) + Half) + 5];
                                        var call__853 = FPath[((FFrame - 9) + Half) + 5];
                                        var call__854 = Sequence.ImageLibrary[Index];
                                        call__854.ShowClipped((call__853.X - XhpSurf) + Sequence.dXshift, ((call__851.Y - call__852.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                                    }
                                    else
                                    {
                                        var call__855 = FPath[((FFrame - 9) + Half) + 5];
                                        var call__856 = FPath[((FFrame - 9) + Half) + 5];
                                        var call__857 = FPath[((FFrame - 9) + Half) + 6];
                                        var call__858 = FPath[((FFrame - 9) + Half) + 6];
                                        var call__859 = FPath[((FFrame - 9) + Half) + 5];
                                        var call__860 = FPath[((FFrame - 9) + Half) + 6];
                                        var call__861 = Sequence.ImageLibrary[Index];
                                        call__861.ShowClipped((((call__859.X + call__860.X) / 2) - XhpSurf) + Sequence.dXshift, (((((call__855.Y - call__856.Z) + call__857.Y) - call__858.Z) / 2) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            void ShowRay()
            {
                int N = default;
                if (Sequence != null)
                {
                    {
                        N = 0;
                        int end__862 = FFrame;
                        for (; N <= end__862; N += 1)
                        {
                            Index = Sequence.GetUnloopedFrameIndex(0);
                            bool bool__863 = (Index != -1);
                            if (bool__863)
                            {
                                var call__864 = Sequence.ImageLibrary.ImageExist(Index);
                                bool__863 = call__864;
                            }

                            if (bool__863)
                            {
                                var call__865 = FPath[N];
                                var call__866 = FPath[N];
                                var call__867 = FPath[N];
                                var call__868 = Sequence.ImageLibrary[Index];
                                call__868.ShowClipped((call__867.X - XhpSurf) + Sequence.dXshift, ((call__865.Y - call__866.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                            }
                        }
                    }
                }
            }

            void ShowFrostBeam()
            {
                int N = default;
                int Alpha = default;
                switch (FFrame)
                {
                    case >= 0 and <= 1999:
                        SelectSequence(0);
                        break;
                    default:
                        SelectSequence(1);
                        break;
                }

                if (Sequence != null)
                {
                    N = 0;
                    while (true)
                    {
                        bool bool__869 = (N <= FFrame);
                        if (bool__869)
                        {
                            var call__870 = FPath.Count;
                            bool__869 = (N < call__870);
                        }

                        if (!bool__869)
                            break;
                        {
                            switch (FFrame)
                            {
                                case >= 0 and <= 1999:
                                    Alpha = 25;
                                    // { Original uses the object address as the animation phase. }
                                    var call__871 = Sequence.GetTotalSequenceFrames();
                                    var call__872 = Sequence.GetUnloopedFrameIndex(Aow.Rtl.Rtl.ObjectToInt(this) % call__871);
                                    Index = call__872;
                                    break;
                                case >= 2000 and <= 2998:
                                    if (N <= (FFrame - 2000))
                                    {
                                        var call__873 = FPath.Count;
                                        Alpha = (((FFrame - 2000) * 75) / (call__873 - 1)) + 25;
                                        Index = Sequence.GetUnloopedFrameIndex(0);
                                    }
                                    else
                                    {
                                        if (N == (FFrame - 1999))
                                        {
                                            SelectSequence(0);
                                        }

                                        Alpha = 25;
                                        var call__874 = Sequence.GetTotalSequenceFrames();
                                        var call__875 = Sequence.GetUnloopedFrameIndex(Aow.Rtl.Rtl.ObjectToInt(this) % call__874);
                                        Index = call__875;
                                    }

                                    break;
                                default:
                                    Alpha = 100;
                                    Index = Sequence.GetUnloopedFrameIndex(0);
                                    break;
                            }

                            // { Stop at the path end in the loop condition. }
                            if (Index != -1)
                            {
                                var call__876 = FPath[N];
                                var call__877 = FPath[N];
                                var call__878 = FPath[N];
                                var call__879 = Sequence.ImageLibrary[Index];
                                call__879.ShowBlended((call__878.X - XhpSurf) + Sequence.dXshift, ((call__876.Y - call__877.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Alpha]);
                            }

                            N += 1;
                        }
                    }
                }
            }

            void ShowAtStart()
            {
                while (Sequence != null)
                {
                    var call__880 = Sequence.GetUnloopedFrameIndex(FFrame);
                    Index = call__880;
                    bool bool__881 = (Index != -1);
                    if (bool__881)
                    {
                        var call__882 = Sequence.ImageLibrary.ImageExist(Index);
                        bool__881 = call__882;
                    }

                    if (bool__881)
                    {
                        var call__883 = FPath[0];
                        var call__884 = FPath[0];
                        var call__885 = Sequence.ImageLibrary[Index];
                        call__885.ShowClipped((call__884.X - XhpSurf) + Sequence.dXshift, (call__883.Y - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                    }

                    Sequence = Sequence.Next;
                }
            }

            void ShowAtStartLooped()
            {
                while (Sequence != null)
                {
                    var call__886 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                    Index = call__886;
                    bool bool__887 = (Index != -1);
                    if (bool__887)
                    {
                        var call__888 = Sequence.ImageLibrary.ImageExist(Index);
                        bool__887 = call__888;
                    }

                    if (bool__887)
                    {
                        var call__889 = FPath[0];
                        var call__890 = FPath[0];
                        var call__891 = Sequence.ImageLibrary[Index];
                        call__891.ShowClipped((call__890.X - XhpSurf) + Sequence.dXshift, (call__889.Y - YhpSurf) + Sequence.dYshift, SurfaceDesc__value);
                    }

                    Sequence = Sequence.Next;
                }
            }

            void ShowScreen()
            {
                while (Sequence != null)
                {
                    bool bool__892 = (Index != -1);
                    if (bool__892)
                    {
                        var call__893 = Sequence.ImageLibrary.ImageExist(Index);
                        bool__892 = call__893;
                    }

                    if (bool__892)
                    {
                        var call__894 = Sequence.ImageLibrary[Index];
                        var call__895 = FPath[0];
                        var call__896 = Sequence.ImageLibrary[Index];
                        var call__897 = Sequence.ImageLibrary[Index];
                        var call__898 = FPath[0];
                        var call__899 = Sequence.ImageLibrary[Index];
                        var call__900 = Sequence.ImageLibrary[Index];
                        call__900.ShowClipped((((call__897.Width + SurfaceDesc__value.Width) * call__898.X) / 100) - call__899.Width, (((call__894.Height + SurfaceDesc__value.Height) * call__895.Y) / 100) - call__896.Height, SurfaceDesc__value);
                    }

                    Sequence = Sequence.Next;
                }
            }

            void ShowScreenSwarm()
            {
                int Alpha = default;
                var call__901 = FPath[FFrame];
                Alpha = ((100 - call__901.Z) / 2) - ((((FFrame % 10) * 2) + Aow.Rtl.Rtl.ObjectToInt(this)) % 20);
                if (Alpha < 0)
                {
                    Alpha = 0;
                }

                while (Sequence != null)
                {
                    var call__902 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                    Index = call__902;
                    bool bool__903 = (Index != -1);
                    if (bool__903)
                    {
                        var call__904 = Sequence.ImageLibrary.ImageExist(Index);
                        bool__903 = call__904;
                    }

                    if (bool__903)
                    {
                        var call__905 = FPath[0];
                        var call__906 = FPath[FFrame];
                        var call__907 = FPath[0];
                        var call__908 = FPath[FFrame];
                        var call__909 = FPath[0];
                        var call__910 = FPath[FFrame];
                        var call__911 = FPath[FFrame];
                        var call__912 = Sequence.ImageLibrary[Index];
                        call__912.ShowBlended(((((((XhpSurf + SurfaceDesc__value.Width) - call__905.X) + 100) * call__906.Z) / 100) + (call__907.X - XhpSurf)) + call__908.X, (((((call__909.Y - YhpSurf) + 100) * (100 - call__910.Z)) / 100) + call__911.Y) - 100, SurfaceDesc__value, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Alpha]);
                    }

                    Sequence = Sequence.Next;
                }
            }

            void ShowParticle(bool Shadow)
            {
                int Alpha = default;
                var call__913 = FPath.Count;
                Alpha = (100 - ((FFrame * 100) / call__913)) - ((((FFrame % 10) * 2) + Aow.Rtl.Rtl.ObjectToInt(this)) % 20);
                if (Alpha < 0)
                {
                    Alpha = 0;
                }

                if (Shadow)
                {
                    Alpha += 20;
                }

                if (Alpha > 100)
                {
                    Alpha = 100;
                }

                while (Sequence != null)
                {
                    // { Original uses the object address rather than the current frame. }
                    Index = Sequence.GetUnloopedFrameIndex(Aow.Rtl.Rtl.ObjectToInt(this) % 3);
                    if (Index != -1)
                    {
                        if (Shadow)
                        {
                            var call__914 = Sequence.ImageLibrary.ImageExist(Index);
                            if (call__914)
                            {
                                var call__915 = FPath[FFrame];
                                var call__916 = FPath[FFrame];
                                var call__917 = FPath[FFrame];
                                var call__918 = Sequence.ImageLibrary[Index];
                                call__918.ShowBlended((call__917.X - XhpSurf) + Sequence.dXshift, ((call__915.Y - call__916.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value, Aow.Units.GFXE.GFXE.ShadowBlendTable.LookupTables[Alpha]);
                            }
                        }
                        else
                        {
                            var call__919 = Sequence.ImageLibrary.ImageExist(Index);
                            if (call__919)
                            {
                                var call__920 = FPath[FFrame];
                                var call__921 = FPath[FFrame];
                                var call__922 = FPath[FFrame];
                                var call__923 = Sequence.ImageLibrary[Index];
                                call__923.ShowBlended((call__922.X - XhpSurf) + Sequence.dXshift, ((call__920.Y - call__921.Z) - YhpSurf) + Sequence.dYshift, SurfaceDesc__value, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Alpha]);
                            }
                        }
                    }

                    Sequence = Sequence.Next;
                }
            }

            void FinishPath()
            {
                var call__924 = FPath.Count;
                if (FFrame >= (call__924 - 1))
                {
                    FDone = true;
                    FFrame = -1;
                }
            }

            void FinishSequence()
            {
                bool bool__925 = (FirstSequence == null);
                if (!bool__925)
                {
                    bool bool__926 = (FirstSequence != null);
                    if (bool__926)
                    {
                        var call__927 = FirstSequence.GetTotalSequenceFrames();
                        bool__926 = (FFrame >= (call__927 - 1));
                    }

                    bool__925 = bool__926;
                }

                if (bool__925)
                {
                    FDone = true;
                    FFrame = -1;
                }
            }

            try
            {
                if (FFrame >= 0)
                {
                    Sequence = null;
                    Index = 0;
                    switch (FEffectType)
                    {
                        case Aow.Units.AoWTC.TfxType.fxArrow:
                            SelectSequence(FVariant);
                            ShowMovingLoop();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxAltArrow:
                            SelectSequence(0);
                            ShowSecondaryFrame();
                            SelectSequence(1);
                            ShowTrail();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxRock:
                            SelectSequence(0);
                            ShowMovingLoop();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxBreath:
                            SelectSequence(0);
                            ShowTrail();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxCometBall:
                        case Aow.Units.AoWTC.TfxType.fxScreenNS:
                            SelectSequence(0);
                            ShowMovingLoop();
                            SelectSequence(1);
                            ShowTrail();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxComet:
                        case Aow.Units.AoWTC.TfxType.fxDecayComet:
                            SelectSequence(0);
                            ShowTrail();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxTouch:
                        case Aow.Units.AoWTC.TfxType.fxEffect:
                            SelectSequence(0);
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxFBExplosion:
                            SelectSequence(1);
                            if (Sequence != null)
                            {
                                SelectFrame();
                                ShowAtStart();
                                FinishSequence();
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxSFOverlay:
                            SelectSequence(2);
                            if (Sequence != null)
                            {
                                var call__928 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                                Index = call__928;
                                ShowAtStart();
                                FinishPath();
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxScreenNESW:
                            SelectSequence(Aow.Rtl.Rtl.ObjectToInt(this) % 3);
                            Index = Sequence.GetUnloopedFrameIndex(0);
                            ShowScreen();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxRay:
                            SelectSequence(0);
                            ShowRay();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxFrostBeam:
                            ShowFrostBeam();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxChain:
                            SelectSequence(0);
                            ShowRay();
                            var call__929 = FPath.Count;
                            bool bool__930 = (FFrame >= (call__929 - 1));
                            if (bool__930)
                            {
                                var call__931 = FPath[FFrame];
                                bool__930 = (call__931.Z == 1);
                            }

                            if (bool__930)
                            {
                                FFrame = -1;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxSwarm:
                            SelectSequence(0);
                            ShowMovingLoop();
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxScreenSwarm:
                            SelectSequence(0);
                            ShowScreenSwarm();
                            FinishPath();
                            break;
                        case >= Aow.Units.AoWTC.TfxType.fxDamLightning and <= Aow.Units.AoWTC.TfxType.fxDamCold:
                            SelectDamageSequence();
                            ShowParticle(FEffectType == Aow.Units.AoWTC.TfxType.fxDamDeath);
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxSacredWrath:
                            if (TSet<byte>.WithBounds(0, 255).WithRange(5, 6).Contains((byte)FFrame))
                            {
                                SelectSequence(2);
                                while (Sequence != null)
                                {
                                    Index = Sequence.GetUnloopedFrameIndex(0);
                                    bool bool__932 = (Index != -1);
                                    if (bool__932)
                                    {
                                        var call__933 = Sequence.ImageLibrary.ImageExist(Index);
                                        bool__932 = call__933;
                                    }

                                    if (bool__932)
                                    {
                                        {
                                            I = 0;
                                            int end__934 = SurfaceDesc__value.Width / Sequence.ImageLibrary[Index].Width;
                                            for (; I <= end__934; I += 1)
                                            {
                                                {
                                                    J = 0;
                                                    int end__935 = SurfaceDesc__value.Height / Sequence.ImageLibrary[Index].Height;
                                                    for (; J <= end__935; J += 1)
                                                    {
                                                        var call__936 = Sequence.ImageLibrary[Index];
                                                        var call__937 = Sequence.ImageLibrary[Index];
                                                        var call__938 = Sequence.ImageLibrary[Index];
                                                        call__938.ShowClipped(call__937.Width * I, call__936.Height * J, SurfaceDesc__value);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    Sequence = Sequence.Next;
                                }
                            }

                            SelectSequence(1);
                            if (Sequence != null)
                            {
                                SelectFrame();
                                ShowAtStart();
                                FinishSequence();
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxSWComet:
                            var call__939 = ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.GetYhx();
                            var call__940 = ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.GetXhx();
                            Aow.Units.HSEngine.HSEngine.HXtoHP(call__940, call__939, ref I, ref J);
                            Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)((SurfaceDesc__value.Width / 2) + XhpSurf)), unchecked((short)((SurfaceDesc__value.Height / 2) + YhpSurf)), unchecked((short)I), unchecked((short)J), 25, 50, ref FPath);
                            FEffectType = Aow.Units.AoWTC.TfxType.fxComet;
                            SelectSequence(0);
                            ShowTrail();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxWallSmoke:
                            var call__941 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[5];
                            Sequence = call__941;
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxHitGround:
                            var call__942 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[9];
                            Sequence = call__942;
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxHitWater:
                            var call__943 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[10];
                            Sequence = call__943;
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxBlood:
                            var call__944 = FPath[0];
                            var call__945 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[call__944.Z];
                            Sequence = call__945;
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxGeyser:
                            if (FFrame < 5)
                            {
                                SelectSequence(1);
                                ShowAtStart();
                            }
                            else
                            {
                                SelectSequence(2);
                                ShowAtStartLooped();
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxHailImpact:
                            SelectSequence(2);
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxPrayerGlow:
                            SelectSequence(1);
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxCloudPuff:
                            SelectSequence(0);
                            SelectFrame();
                            ShowAtStart();
                            FinishSequence();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxGeyserWater:
                            SelectSequence(0);
                            ShowParticle((FSpellIndex == 130) || (FSpellIndex == 106));
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxFrost:
                            SelectSequence(1);
                            ShowParticle(FSpellIndex == 130);
                            FinishPath();
                            break;
                        case Aow.Units.AoWTC.TfxType.fxDecayLeg:
                            SelectSequence(0);
                            ShowRay();
                            FCancelled = true;
                            break;
                        case Aow.Units.AoWTC.TfxType.fxSacFire:
                            SelectSequence(0);
                            ShowMovingLoop();
                            FinishPath();
                            break;
                    }
                }
            }
            catch (System.Exception)
            {
                // { Original puts the diagnostic in the message-box caption. }
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Show", AnsiString.FromPChar(Ptr<byte>.Unbox(((((AnsiString)"Error - fxHS ") + Aow.Rtl.Rtl.IntToStr(FAbilityIndex)) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(FSpellIndex))), (int)0);
            }
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
            Aow.Units.AoWTC.TfxHS Effect = default;
            int J = default;
            int I = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            Aow.Units.aowFX.TXYZList NewPath = default;
            int K = default;
            int X = default;
            int Y = default;
            float Z = default;
            float Speed = default;
            float VZ = default;
            float Gravity = default;
            Aow.Units.AoWTC.TfxHS Effect2 = default;
            Aow.Units.AoWTC.TTacticalCombatUnitHS UnitHS = default;
            Aow.Units.HSEngine.TMapFieldMsg Msg = default;
            void MakeCloud(int Count, int Radius)
            {
                int N = default;
                {
                    N = 1;
                    int end__946 = Count - 1;
                    for (; N <= end__946; N += 1)
                    {
                        var call__947 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxCloudPuff);
                        Effect = call__947;
                        Effect.FAbilityIndex = FAbilityIndex;
                        Effect.FSpellIndex = FSpellIndex;
                        var call__948 = Aow.Units.aowFX.TXYZ.Create();
                        Effect.FPath.Add(call__948);
                        J = Aow.Rtl.Rtl.Random(70) + 10;
                        var call__949 = System.Math.PI;
                        var call__950 = System.Math.Cos(((double)(((double)J) * call__949)) / 180);
                        var call__951 = Aow.Rtl.Rtl.Random(Radius);
                        var call__952 = FPath[0];
                        var call__953 = Effect.FPath[0];
                        call__953.X = call__952.X + Aow.Rtl.Rtl.Round(((double)call__950) * (call__951 - (Radius / 2)));
                        var call__954 = System.Math.PI;
                        var call__955 = System.Math.Sin(((double)(((double)J) * call__954)) / 180);
                        var call__956 = Aow.Rtl.Rtl.Random(Radius);
                        var call__957 = FPath[0];
                        var call__958 = Effect.FPath[0];
                        call__958.Y = call__957.Y + (Aow.Rtl.Rtl.Round(((double)call__955) * (call__956 - (Radius / 2))) / 2);
                    }
                }
            }

            void MakeImpact()
            {
                Effect = null;
                var call__959 = FPath[FFrame];
                var call__960 = FPath[FFrame];
                Aow.Units.HSEngine.HSEngine.HPtoHX(call__960.X, call__959.Y, ref I, ref J);
                var call__961 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__962 = call__961.Field[I, J];
                if (call__962.TerrainType == 0)
                {
                    var call__963 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxHitWater);
                    Effect = call__963;
                }
                else
                {
                    var call__964 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__965 = call__964.Field[I, J];
                    bool bool__966 = (call__965.OverlayType != 8);
                    if (bool__966)
                    {
                        var call__967 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__968 = call__967.Field[I, J];
                        var call__969 = call__968.FindOwnedChild(2229508);
                        bool__966 = (call__969 == null);
                    }

                    bool bool__970 = bool__966;
                    if (bool__970)
                    {
                        var call__971 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__972 = call__971.Field[I, J];
                        var call__973 = call__972.FindOwnedChild(2229510);
                        bool__970 = (call__973 == null);
                    }

                    if (bool__970)
                    {
                        var call__974 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxHitGround);
                        Effect = call__974;
                    }
                }

                if (Effect != null)
                {
                    var call__975 = Aow.Units.aowFX.TXYZ.Create();
                    Effect.FPath.Add(call__975);
                    var call__976 = FPath[FFrame];
                    var call__977 = Effect.FPath[0];
                    call__977.X = call__976.X;
                    var call__978 = FPath[FFrame];
                    var call__979 = Effect.FPath[0];
                    call__979.Y = call__978.Y;
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
                        case Aow.Units.AoWTC.TfxType.fxArrow:
                        case Aow.Units.AoWTC.TfxType.fxRock:
                        case Aow.Units.AoWTC.TfxType.fxComet:
                        case Aow.Units.AoWTC.TfxType.fxBreath:
                        case Aow.Units.AoWTC.TfxType.fxCometBall:
                        case Aow.Units.AoWTC.TfxType.fxSwarm:
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

                            if ((((FFrame == 0) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds) && (FAbilityIndex == 52)) && (FSpellIndex == 122))
                            {
                                var call__980 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                call__980.FCastSounds.Play(0);
                            }

                            var call__981 = FPath.Count;
                            if (FFrame >= (call__981 - 1))
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        var call__982 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__982.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__983 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__983.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__984 = FPath.Count;
                                FFrame = call__984 - 1;
                                FDone = true;
                                if (FTarget == null)
                                {
                                    MakeImpact();
                                }

                                var call__985 = FPath[FFrame];
                                var call__986 = FPath[FFrame];
                                var call__987 = FPath[FFrame];
                                Aow.Units.HSEngine.HSEngine.HPtoHX(call__987.X, call__985.Y - call__986.Z, ref I, ref J);
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(I, J, 0, 2);
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxDecayComet:
                            if (((FFrame == 0) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds) && (FAbilityIndex == 52))
                            {
                                var call__988 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                call__988.FCastSounds.Play(0);
                            }

                            var call__989 = FPath.Count;
                            if (FFrame >= (call__989 - 1))
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        var call__990 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__990.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__991 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__991.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__992 = FPath.Count;
                                FFrame = call__992 - 1;
                                FDone = true;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxAltArrow:
                            if (((FFrame == 0) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds) && (FAbilityIndex == 52))
                            {
                                var call__993 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                call__993.FCastSounds.Play(0);
                            }

                            FFrame += 3;
                            var call__994 = FPath.Count;
                            if (FFrame >= (call__994 - 1))
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        var call__995 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__995.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__996 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__996.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__997 = FPath.Count;
                                FFrame = call__997 - 1;
                                FDone = true;
                                if (FTarget == null)
                                {
                                    MakeImpact();
                                }

                                var call__998 = FPath[FFrame];
                                var call__999 = FPath[FFrame];
                                var call__1000 = FPath[FFrame];
                                Aow.Units.HSEngine.HSEngine.HPtoHX(call__1000.X, call__998.Y - call__999.Z, ref I, ref J);
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(I, J, 0, 2);
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxScreenNS:
                            FFrame += 1;
                            var call__1001 = FPath.Count;
                            if (FFrame >= (call__1001 - 1))
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        var call__1002 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__1002.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__1003 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__1003.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__1004 = FPath.Count;
                                FFrame = call__1004 - 1;
                                FDone = true;
                                var call__1005 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxHailImpact);
                                Effect = call__1005;
                                Effect.FAbilityIndex = FAbilityIndex;
                                Effect.FSpellIndex = FSpellIndex;
                                var call__1006 = Aow.Units.aowFX.TXYZ.Create();
                                Effect.FPath.Add(call__1006);
                                var call__1007 = FPath[FFrame];
                                var call__1008 = Effect.FPath[0];
                                call__1008.X = call__1007.X;
                                var call__1009 = FPath[FFrame];
                                var call__1010 = Effect.FPath[0];
                                call__1010.Y = call__1009.Y;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxHailImpact:
                            if (FAbilityIndex == 52)
                            {
                                var call__1011 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                var call__1012 = call__1011.FCastImages.ImageSequences[2];
                                Sequence = call__1012;
                            }
                            else
                            {
                                var call__1013 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                var call__1014 = call__1013.FImageSequences.ImageSequences[2];
                                Sequence = call__1014;
                            }

                            if (Sequence != null)
                            {
                                var call__1015 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= (call__1015 - 1))
                                {
                                    var call__1016 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1016 - 1;
                                    FDone = true;
                                }
                            }
                            else
                            {
                                FFrame = -1;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxPrayerGlow:
                            if (FAbilityIndex == 52)
                            {
                                var call__1017 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                var call__1018 = call__1017.FCastImages.ImageSequences[1];
                                Sequence = call__1018;
                            }
                            else
                            {
                                var call__1019 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                var call__1020 = call__1019.FImageSequences.ImageSequences[1];
                                Sequence = call__1020;
                            }

                            if (Sequence != null)
                            {
                                var call__1021 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= (call__1021 - 1))
                                {
                                    var call__1022 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1022 - 1;
                                    FDone = true;
                                }
                            }
                            else
                            {
                                FFrame = -1;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxSFOverlay:
                            var call__1023 = FPath.Count;
                            if (FFrame >= (call__1023 - 1))
                            {
                                var call__1024 = FPath.Count;
                                FFrame = call__1024 - 1;
                                FDone = true;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxScreenSwarm:
                            if (FFrame == 0)
                            {
                                FFrame = 1;
                            }

                            var call__1025 = FPath.Count;
                            if (FFrame >= (call__1025 - 1))
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if ((FTarget != null) && (FAbilityIndex == 52))
                                    {
                                        var call__1026 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__1026.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__1027 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__1027.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__1028 = FPath.Count;
                                FFrame = call__1028 - 1;
                                FDone = true;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxRay:
                            FFrame += 12;
                            var call__1029 = FPath.Count;
                            if (FFrame >= (call__1029 - 1))
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        var call__1030 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__1030.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__1031 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__1031.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__1032 = FPath.Count;
                                FFrame = call__1032 - 1;
                                FDone = true;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxFrostBeam:
                            switch (FFrame)
                            {
                                case >= 0 and <= 999:
                                    FFrame += 1;
                                    var call__1033 = FPath.Count;
                                    if (FFrame >= (call__1033 - 1))
                                    {
                                        FFrame = 999;
                                    }

                                    J = Aow.Rtl.Rtl.Random(5) + 5;
                                    Aow.Units.HSEngine.HSEngine.HXtoHP(FTargetX, FTargetY, ref K, ref X);
                                {
                                    I = 0;
                                    int end__1034 = J;
                                    for (; I <= end__1034; I += 1)
                                    {
                                        var call__1035 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1035;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__1036 = Aow.Rtl.Rtl.Random(100);
                                        var call__1037 = Aow.Rtl.Rtl.Random(5);
                                        var call__1038 = Aow.Rtl.Rtl.Random(100);
                                        Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)(K - 200)), unchecked((short)(X - 200)), unchecked((short)((call__1038 + K) - 50)), unchecked((short)((call__1036 + X) - 50)), unchecked((short)(call__1037 + 5)), 100, ref Effect.FPath);
                                    }
                                }

                                    J = Aow.Rtl.Rtl.Random(10) + 10;
                                {
                                    I = 0;
                                    int end__1039 = J;
                                    for (; I <= end__1039; I += 1)
                                    {
                                        var call__1040 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1040;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 20)) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)(((double)Aow.Rtl.Rtl.Random(10)) / 10);
                                        Z = 100;
                                        var call__1041 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(K - 200, X - 100, call__1041, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case >= 1000 and <= 1029:
                                    Aow.Units.HSEngine.HSEngine.HXtoHP(FTargetX, FTargetY, ref K, ref X);
                                    J = Aow.Rtl.Rtl.Random(5) + 5;
                                {
                                    I = 0;
                                    int end__1042 = J;
                                    for (; I <= end__1042; I += 1)
                                    {
                                        var call__1043 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1043;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__1044 = Aow.Rtl.Rtl.Random(100);
                                        var call__1045 = Aow.Rtl.Rtl.Random(5);
                                        var call__1046 = Aow.Rtl.Rtl.Random(100);
                                        Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)(K - 200)), unchecked((short)(X - 200)), unchecked((short)((call__1046 + K) - 50)), unchecked((short)((call__1044 + X) - 50)), unchecked((short)(call__1045 + 5)), 100, ref Effect.FPath);
                                    }
                                }

                                    J = Aow.Rtl.Rtl.Random(10) + 10;
                                {
                                    I = 0;
                                    int end__1047 = J;
                                    for (; I <= end__1047; I += 1)
                                    {
                                        var call__1048 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1048;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 20)) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)(((double)Aow.Rtl.Rtl.Random(10)) / 10);
                                        Z = 100;
                                        var call__1049 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(K - 200, X - 100, call__1049, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    J = Aow.Rtl.Rtl.Random(5) + 5;
                                {
                                    I = 0;
                                    int end__1050 = J;
                                    for (; I <= end__1050; I += 1)
                                    {
                                        var call__1051 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1051;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(20) + 20)) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 20)) / 10);
                                        Z = 0;
                                        var call__1052 = FPath.Count;
                                        var call__1053 = FPath[call__1052 - 1];
                                        var call__1054 = FPath.Count;
                                        var call__1055 = FPath[call__1054 - 1];
                                        var call__1056 = FPath.Count;
                                        var call__1057 = FPath[call__1056 - 1];
                                        var call__1058 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(call__1057.X, call__1053.Y - call__1055.Z, call__1058, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case 1030:
                                    FFrame = 1999;
                                    break;
                                case >= 2000 and <= 2999:
                                    FFrame += 1;
                                    var call__1059 = FPath.Count;
                                    if (FFrame >= ((call__1059 + 2000) - 1))
                                    {
                                        FFrame = 2999;
                                    }

                                    J = Aow.Rtl.Rtl.Random(5) + 5;
                                {
                                    I = 0;
                                    int end__1060 = J;
                                    for (; I <= end__1060; I += 1)
                                    {
                                        var call__1061 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1061;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(20) + 20)) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 20)) / 10);
                                        Z = 0;
                                        var call__1062 = FPath.Count;
                                        var call__1063 = FPath[call__1062 - 1];
                                        var call__1064 = FPath.Count;
                                        var call__1065 = FPath[call__1064 - 1];
                                        var call__1066 = FPath.Count;
                                        var call__1067 = FPath[call__1066 - 1];
                                        var call__1068 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(call__1067.X, call__1063.Y - call__1065.Z, call__1068, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case 3000:
                                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            var call__1069 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__1069.FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            var call__1070 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__1070.FSFXLibrary.Play(1);
                                        }
                                    }

                                {
                                    I = 0;
                                    int end__1071 = FPath.Count - 1;
                                    for (; I <= end__1071; I += 1)
                                    {
                                        var call__1072 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxFrost);
                                        Effect = call__1072;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(25) + 25)) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 40)) / 10);
                                        Z = 0;
                                        var call__1073 = FPath[I];
                                        var call__1074 = FPath[I];
                                        var call__1075 = Aow.Rtl.Rtl.Random(360);
                                        var call__1076 = FPath[I];
                                        Aow.Units.aowFX.aowFX.MakeArcPath(call__1076.X, call__1073.Y - call__1074.Z, call__1075, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    J = Aow.Rtl.Rtl.Random(10) + 100;
                                {
                                    I = 0;
                                    int end__1077 = J;
                                    for (; I <= end__1077; I += 1)
                                    {
                                        var call__1078 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxFrost);
                                        Effect = call__1078;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 50)) / 10);
                                        Gravity = (float)0.1;
                                        VZ = (float)(((double)(Aow.Rtl.Rtl.Random(5) + 5)) / 10);
                                        Z = 0;
                                        var call__1079 = FPath.Count;
                                        var call__1080 = FPath[call__1079 - 1];
                                        var call__1081 = FPath.Count;
                                        var call__1082 = FPath[call__1081 - 1];
                                        var call__1083 = FPath.Count;
                                        var call__1084 = FPath[call__1083 - 1];
                                        var call__1085 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(call__1084.X, call__1080.Y - call__1082.Z, call__1085, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    FDone = true;
                                    FTarget = null;
                                    break;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxShockWave:
                            switch (FFrame)
                            {
                                case >= 0 and <= 29:
                                    if ((FFrame == 0) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            var call__1086 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__1086.FCastSounds.Play(0);
                                        }
                                        else
                                        {
                                            var call__1087 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__1087.FSFXLibrary.Play(0);
                                        }
                                    }

                                {
                                    I = 1;
                                    byte end__1088 = 20;
                                    for (; I <= end__1088; I += 1)
                                    {
                                        var call__1089 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1089;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__1090 = Aow.Rtl.Rtl.Random(360);
                                        var call__1091 = FPath[0];
                                        var call__1092 = FPath[0];
                                        var call__1093 = Aow.Rtl.Rtl.Random(2);
                                        var call__1094 = Aow.Rtl.Rtl.Random(10);
                                        var call__1095 = Aow.Rtl.Rtl.Random(10);
                                        Aow.Units.aowFX.aowFX.MakeSwarmPath(call__1095 + 10, call__1094 + (FFrame / 4), ((I % 6) * 60) + FFrame, call__1090, call__1091.X, call__1092.Y, (Aow.Units.aowFX.TSwarmDirection)call__1093, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case >= 30 and <= 59:
                                {
                                    I = 1;
                                    byte end__1096 = 20;
                                    for (; I <= end__1096; I += 1)
                                    {
                                        var call__1097 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1097;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__1098 = Aow.Rtl.Rtl.Random(360);
                                        var call__1099 = FPath[0];
                                        var call__1100 = FPath[0];
                                        var call__1101 = Aow.Rtl.Rtl.Random(2);
                                        var call__1102 = Aow.Rtl.Rtl.Random(10);
                                        var call__1103 = Aow.Rtl.Rtl.Random(10);
                                        Aow.Units.aowFX.aowFX.MakeSwarmPath(call__1103 + 10, call__1102 + ((60 - FFrame) / 4), ((I % 6) * 60) + FFrame, call__1098, call__1099.X, call__1100.Y, (Aow.Units.aowFX.TSwarmDirection)call__1101, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case >= 60 and <= 89:
                                {
                                    I = 1;
                                    byte end__1104 = 20;
                                    for (; I <= end__1104; I += 1)
                                    {
                                        var call__1105 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1105;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__1106 = Aow.Rtl.Rtl.Random(360);
                                        var call__1107 = FPath[0];
                                        var call__1108 = FPath[0];
                                        var call__1109 = Aow.Rtl.Rtl.Random(2);
                                        var call__1110 = Aow.Rtl.Rtl.Random(10);
                                        var call__1111 = Aow.Rtl.Rtl.Random(10);
                                        Aow.Units.aowFX.aowFX.MakeSwarmPath(call__1111 + 10, call__1110 + ((FFrame - 60) / 2), ((I % 6) * 60) + FFrame, call__1106, call__1107.X, call__1108.Y, (Aow.Units.aowFX.TSwarmDirection)call__1109, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case >= 90 and <= 119:
                                {
                                    I = 1;
                                    byte end__1112 = 20;
                                    for (; I <= end__1112; I += 1)
                                    {
                                        var call__1113 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1113;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__1114 = Aow.Rtl.Rtl.Random(360);
                                        var call__1115 = FPath[0];
                                        var call__1116 = FPath[0];
                                        var call__1117 = Aow.Rtl.Rtl.Random(2);
                                        var call__1118 = Aow.Rtl.Rtl.Random(10);
                                        var call__1119 = Aow.Rtl.Rtl.Random(10);
                                        Aow.Units.aowFX.aowFX.MakeSwarmPath(call__1119 + 10, call__1118 + ((120 - FFrame) / 2), ((I % 6) * 60) + FFrame, call__1114, call__1115.X, call__1116.Y, (Aow.Units.aowFX.TSwarmDirection)call__1117, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case 150:
                                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            var call__1120 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__1120.FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            var call__1121 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__1121.FSFXLibrary.Play(1);
                                        }
                                    }

                                    X = 200;
                                    Y = 200;
                                    J = Aow.Rtl.Rtl.Random(5);
                                {
                                    I = 0;
                                    short end__1122 = 359;
                                    for (; I <= end__1122; I += 1)
                                    {
                                        var call__1123 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1123;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        X = -X;
                                        var call__1124 = System.Math.PI;
                                        var call__1125 = System.Math.Sin(((double)(((double)(I + J)) * call__1124)) / 180);
                                        var call__1126 = Aow.Rtl.Rtl.Random(10);
                                        var call__1127 = FPath[0];
                                        var call__1128 = System.Math.PI;
                                        var call__1129 = System.Math.Cos(((double)(((double)(I + J)) * call__1128)) / 180);
                                        var call__1130 = Aow.Rtl.Rtl.Random(10);
                                        var call__1131 = FPath[0];
                                        var call__1132 = FPath[0];
                                        var call__1133 = FPath[0];
                                        Aow.Units.aowFX.aowFX.MakeLinePath(call__1133.X, call__1132.Y, call__1131.X + Aow.Rtl.Rtl.Round(((double)call__1129) * (call__1130 + X)), call__1127.Y + (Aow.Rtl.Rtl.Round(((double)call__1125) * (call__1126 + Y)) / 2), 75, ref Effect.FPath);
                                        if ((I % 10) == 0)
                                        {
                                            {
                                                K = 1;
                                                byte end__1134 = 10;
                                                for (; K <= end__1134; K += 1)
                                                {
                                                    var call__1135 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                                    Effect2 = call__1135;
                                                    Effect2.FAbilityIndex = FAbilityIndex;
                                                    Effect2.FSpellIndex = FSpellIndex;
                                                    var call__1136 = Effect.FPath.Count;
                                                    var call__1137 = Effect.FPath[call__1136 - 1];
                                                    var call__1138 = Effect.FPath.Count;
                                                    var call__1139 = Effect.FPath[call__1138 - 1];
                                                    var call__1140 = FPath[0];
                                                    var call__1141 = FPath[0];
                                                    Aow.Units.aowFX.aowFX.MakeLinePath(call__1141.X, call__1140.Y, call__1139.X, call__1137.Y, (K * 5) + 75, ref Effect2.FPath);
                                                }
                                            }
                                        }
                                    }
                                }

                                    break;
                                case >= 151 and <= 169:
                                case >= 210 and <= 229:
                                    var call__1142 = FPath[0];
                                    var call__1143 = FPath[0];
                                    Aow.Units.HSEngine.HSEngine.HPtoHX(call__1143.X, call__1142.Y, ref X, ref Y);
                                    Msg.MsgID = 200720;
                                    Msg.Param[0] = 1;
                                    Msg.Result = -1;
                                {
                                    I = 1;
                                    byte end__1144 = 36;
                                    for (; I <= end__1144; I += 1)
                                    {
                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                                        var call__1145 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__1146 = call__1145.Field[J, K];
                                        call__1146.SendMapFieldMsg(ref Msg);
                                    }
                                }

                                    break;
                                case >= 170 and <= 184:
                                case >= 195 and <= 209:
                                    var call__1147 = FPath[0];
                                    var call__1148 = FPath[0];
                                    Aow.Units.HSEngine.HSEngine.HPtoHX(call__1148.X, call__1147.Y, ref X, ref Y);
                                    Msg.MsgID = 200720;
                                    Msg.Param[0] = 2;
                                    Msg.Result = -1;
                                {
                                    I = 1;
                                    byte end__1149 = 36;
                                    for (; I <= end__1149; I += 1)
                                    {
                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                                        var call__1150 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__1151 = call__1150.Field[J, K];
                                        call__1151.SendMapFieldMsg(ref Msg);
                                    }
                                }

                                    break;
                                case >= 185 and <= 194:
                                    var call__1152 = FPath[0];
                                    var call__1153 = FPath[0];
                                    Aow.Units.HSEngine.HSEngine.HPtoHX(call__1153.X, call__1152.Y, ref X, ref Y);
                                    Msg.MsgID = 200720;
                                    Msg.Param[0] = 3;
                                    Msg.Result = -1;
                                {
                                    I = 1;
                                    byte end__1154 = 36;
                                    for (; I <= end__1154; I += 1)
                                    {
                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                                        var call__1155 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__1156 = call__1155.Field[J, K];
                                        call__1156.SendMapFieldMsg(ref Msg);
                                    }
                                }

                                    break;
                                case 230:
                                    var call__1157 = FPath[0];
                                    var call__1158 = FPath[0];
                                    Aow.Units.HSEngine.HSEngine.HPtoHX(call__1158.X, call__1157.Y, ref X, ref Y);
                                    Msg.MsgID = 200721;
                                    Msg.Result = -1;
                                {
                                    I = 1;
                                    byte end__1159 = 36;
                                    for (; I <= end__1159; I += 1)
                                    {
                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                                        var call__1160 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__1161 = call__1160.Field[J, K];
                                        call__1161.SendMapFieldMsg(ref Msg);
                                    }
                                }

                                    FFrame = -1;
                                    break;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxWinds:
                            switch (FFrame)
                            {
                                case >= 0 and <= 1999:
                                    var call__1162 = FPath.Count;
                                    if (FFrame >= (call__1162 + 20))
                                    {
                                        bool bool__1163 = (FTarget != null);
                                        if (bool__1163)
                                        {
                                            var call__1164 = FTarget.GetEnabled();
                                            bool__1163 = call__1164;
                                        }

                                        if (bool__1163)
                                        {
                                            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                            {
                                                if (FAbilityIndex == 52)
                                                {
                                                    var call__1165 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                                    call__1165.FCastSounds.Play(1);
                                                }
                                                else
                                                {
                                                    var call__1166 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                                    call__1166.FSFXLibrary.Play(1);
                                                }
                                            }

                                            var call__1167 = FPath.Count;
                                            var call__1168 = FPath[call__1167 - 1];
                                            var call__1169 = FPath.Count;
                                            var call__1170 = FPath[call__1169 - 1];
                                            Aow.Units.HSEngine.HSEngine.HPtoHX(call__1170.X, call__1168.Y, ref J, ref K);
                                            var call__1171 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                            var call__1172 = call__1171.Field[J, K];
                                            ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.MoveTo(call__1172, 0);
                                            ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FOffsetX = 0;
                                            ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FOffsetY = 0;
                                            ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FBusy = false;
                                        }

                                        FFrame = -1;
                                    }
                                    else
                                    {
                                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds && ((FFrame == FTargetX) || (FFrame == FTargetY)))
                                        {
                                            if (FAbilityIndex == 52)
                                            {
                                                var call__1173 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                                call__1173.FCastSounds.Play(0);
                                            }
                                            else
                                            {
                                                var call__1174 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                                call__1174.FSFXLibrary.Play(0);
                                            }
                                        }

                                        bool bool__1175 = (FTarget != null);
                                        if (bool__1175)
                                        {
                                            var call__1176 = FTarget.GetEnabled();
                                            bool__1175 = call__1176;
                                        }

                                        if (bool__1175)
                                        {
                                            var call__1177 = FPath.Count;
                                            if (((FFrame - 20) < call__1177) && (FFrame >= 20))
                                            {
                                                var call__1178 = FPath[FFrame - 20];
                                                var call__1179 = FPath[0];
                                                ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FOffsetX = call__1178.X - call__1179.X;
                                                var call__1180 = FPath[FFrame - 20];
                                                var call__1181 = FPath[FFrame - 20];
                                                var call__1182 = FPath[0];
                                                ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FOffsetY = (call__1180.Y - call__1181.Z) - call__1182.Y;
                                                ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.SetDirection(unchecked((byte)(((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FDirection + 1)));
                                                if (((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FDirection > 6)
                                                {
                                                    ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.SetDirection(1);
                                                }

                                                ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FBusy = true;
                                            }
                                        }

                                        {
                                            I = FFrame - 20;
                                            int end__1183 = FFrame;
                                            for (; I <= end__1183; I += 1)
                                            {
                                                bool bool__1184 = (I >= 0);
                                                if (bool__1184)
                                                {
                                                    var call__1185 = FPath.Count;
                                                    bool__1184 = (I < call__1185);
                                                }

                                                if (bool__1184)
                                                {
                                                    var call__1186 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                                    Effect = call__1186;
                                                    Effect.FAbilityIndex = FAbilityIndex;
                                                    Effect.FSpellIndex = FSpellIndex;
                                                    var call__1187 = Aow.Rtl.Rtl.Random(360);
                                                    var call__1188 = FPath[I];
                                                    var call__1189 = Aow.Rtl.Rtl.Random(10);
                                                    var call__1190 = FPath[I];
                                                    var call__1191 = FPath[I];
                                                    var call__1192 = Aow.Rtl.Rtl.Random(10);
                                                    var call__1193 = Aow.Rtl.Rtl.Random(2);
                                                    var call__1194 = FPath.Count;
                                                    var call__1195 = Aow.Rtl.Rtl.Random(5);
                                                    Aow.Units.aowFX.aowFX.MakeSwarmPath(call__1195 + 20, (call__1194 - I) / 5, 0, call__1187, (call__1188.X + call__1189) - 5, ((call__1190.Y - call__1191.Z) + call__1192) - 5, (Aow.Units.aowFX.TSwarmDirection)call__1193, ref Effect.FPath);
                                                }
                                            }
                                        }
                                    }

                                    break;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxSacFlame:
                            switch (FFrame)
                            {
                                case >= 0 and <= 99:
                                    if ((FFrame == 0) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            var call__1196 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__1196.FCastSounds.Play(0);
                                        }
                                        else
                                        {
                                            var call__1197 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__1197.FSFXLibrary.Play(0);
                                        }
                                    }

                                    J = Aow.Rtl.Rtl.Random(6) + 10;
                                    var call__1198 = ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.GetYhx();
                                    var call__1199 = ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.GetXhx();
                                    Aow.Units.HSEngine.HSEngine.HXtoHP(call__1199, call__1198, ref K, ref X);
                                {
                                    I = 1;
                                    int end__1200 = J - 1;
                                    for (; I <= end__1200; I += 1)
                                    {
                                        var call__1201 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxFrost);
                                        Effect = call__1201;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(25) + 25)) / 10);
                                        Gravity = (float)0.5;
                                        var call__1202 = Aow.Rtl.Rtl.Random(50);
                                        VZ = (float)(((double)(call__1202 + FFrame)) / 10);
                                        Z = 0;
                                        Aow.Units.aowFX.aowFX.MakeArcPath(K + 16, X + 16, (I * 360) / J, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
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
                                            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                            {
                                                if (FAbilityIndex == 52)
                                                {
                                                    var call__1203 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                                    call__1203.FCastSounds.Play(1);
                                                }
                                                else
                                                {
                                                    var call__1204 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                                    call__1204.FSFXLibrary.Play(1);
                                                }
                                            }

                                            J = Aow.Rtl.Rtl.Random(10) + 100;
                                        {
                                            I = 0;
                                            int end__1205 = J;
                                            for (; I <= end__1205; I += 1)
                                            {
                                                var call__1206 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxFrost);
                                                Effect = call__1206;
                                                Effect.FAbilityIndex = FAbilityIndex;
                                                Effect.FSpellIndex = FSpellIndex;
                                                Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 50)) / 10);
                                                Gravity = (float)0.1;
                                                VZ = (float)(((double)(Aow.Rtl.Rtl.Random(5) + 5)) / 10);
                                                Z = 0;
                                                var call__1207 = Aow.Rtl.Rtl.Random(360);
                                                Aow.Units.aowFX.aowFX.MakeArcPath(K + 16, X + 16, call__1207, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                            }
                                        }

                                            break;
                                    }

                                    break;
                                case >= 125 and <= 199:
                                    if (FFrame == 125)
                                    {
                                        var call__1208 = FPath[0];
                                        var call__1209 = FPath[0];
                                        Aow.Units.HSEngine.HSEngine.HPtoHX(call__1209.X, call__1208.Y, ref I, ref J);
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(I, J - 3, 0);
                                    }

                                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            var call__1210 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__1210.FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            var call__1211 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__1211.FSFXLibrary.Play(1);
                                        }
                                    }

                                    J = Aow.Rtl.Rtl.Random(6) + 10;
                                    var call__1212 = FPath[0];
                                    var call__1213 = Aow.Rtl.Rtl.Random(160);
                                    K = (call__1212.X + call__1213) - 80;
                                    var call__1214 = FPath[0];
                                    var call__1215 = Aow.Rtl.Rtl.Random(80);
                                    X = (call__1214.Y + call__1215) - 40;
                                {
                                    I = 1;
                                    int end__1216 = J - 1;
                                    for (; I <= end__1216; I += 1)
                                    {
                                        var call__1217 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxFrost);
                                        Effect = call__1217;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 25)) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 100)) / 10);
                                        Z = 0;
                                        Aow.Units.aowFX.aowFX.MakeArcPath(K, X, (I * 360) / J, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    J = Aow.Rtl.Rtl.Random(10) + 50;
                                {
                                    I = 0;
                                    int end__1218 = J;
                                    for (; I <= end__1218; I += 1)
                                    {
                                        var call__1219 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxFrost);
                                        Effect = call__1219;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 50)) / 10);
                                        Gravity = (float)0.1;
                                        VZ = (float)(((double)(Aow.Rtl.Rtl.Random(5) + 5)) / 10);
                                        Z = 0;
                                        var call__1220 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(K, X, call__1220, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case 200:
                                    FFrame = -1;
                                    break;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxMindDecay:
                            switch (FFrame)
                            {
                                case >= 0 and <= 49:
                                    if ((FFrame % 2) > 0)
                                    {
                                        {
                                            I = 0;
                                            int end__1221 = FFrame;
                                            for (; I <= end__1221; I += 1)
                                            {
                                                {
                                                    J = 0;
                                                    int end__1222 = I / 25;
                                                    for (; J <= end__1222; J += 1)
                                                    {
                                                        var call__1223 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                                        Effect = call__1223;
                                                        Effect.FAbilityIndex = FAbilityIndex;
                                                        Effect.FSpellIndex = FSpellIndex;
                                                        var call__1224 = Aow.Rtl.Rtl.Random(360);
                                                        var call__1225 = FPath[0];
                                                        var call__1226 = FPath[0];
                                                        var call__1227 = Aow.Rtl.Rtl.Random(2);
                                                        var call__1228 = Aow.Rtl.Rtl.Random(10);
                                                        Aow.Units.aowFX.aowFX.MakeSwarmPath(call__1228 + 10, I / 2, 45, call__1224, (call__1225.X + 100) - I, ((call__1226.Y - 200) - 50) + I, (Aow.Units.aowFX.TSwarmDirection)call__1227, ref Effect.FPath);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    break;
                                case >= 50 and <= 149:
                                    if ((FFrame % 2) > 0)
                                    {
                                        {
                                            I = 0;
                                            byte end__1229 = 50;
                                            for (; I <= end__1229; I += 1)
                                            {
                                                {
                                                    J = 0;
                                                    int end__1230 = I / 25;
                                                    for (; J <= end__1230; J += 1)
                                                    {
                                                        var call__1231 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                                        Effect = call__1231;
                                                        Effect.FAbilityIndex = FAbilityIndex;
                                                        Effect.FSpellIndex = FSpellIndex;
                                                        var call__1232 = Aow.Rtl.Rtl.Random(360);
                                                        var call__1233 = FPath[0];
                                                        var call__1234 = FPath[0];
                                                        var call__1235 = Aow.Rtl.Rtl.Random(2);
                                                        var call__1236 = Aow.Rtl.Rtl.Random(10);
                                                        Aow.Units.aowFX.aowFX.MakeSwarmPath(call__1236 + 10, I / 2, 45, call__1232, (call__1233.X + 100) - I, ((call__1234.Y - 200) - 50) + I, (Aow.Units.aowFX.TSwarmDirection)call__1235, ref Effect.FPath);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (((FFrame % 5) == 0) && (FFrame < 85))
                                    {
                                        var call__1237 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxDecayComet);
                                        Effect = call__1237;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__1238 = FPath[0];
                                        var call__1239 = Aow.Rtl.Rtl.Random(50);
                                        var call__1240 = Aow.Rtl.Rtl.Random(5);
                                        var call__1241 = FPath[0];
                                        var call__1242 = Aow.Rtl.Rtl.Random(50);
                                        var call__1243 = FPath[0];
                                        var call__1244 = Aow.Rtl.Rtl.Random(50);
                                        var call__1245 = FPath[0];
                                        var call__1246 = Aow.Rtl.Rtl.Random(50);
                                        Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)((call__1245.X + call__1246) + 25)), unchecked((short)((call__1243.Y + call__1244) - 225)), unchecked((short)((call__1241.X + call__1242) - 25)), unchecked((short)((call__1238.Y + call__1239) - 25)), unchecked((short)(call__1240 + 5)), 100, ref Effect.FPath);
                                        {
                                            I = 0;
                                            byte end__1247 = 10;
                                            for (; I <= end__1247; I += 1)
                                            {
                                                Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 50)) / 10);
                                                Gravity = (float)1.5;
                                                VZ = (float)(((double)(Aow.Rtl.Rtl.Random(25) + 25)) / 10);
                                                Z = 0;
                                                var call__1248 = Effect.FPath.Count;
                                                var call__1249 = Effect.FPath[call__1248 - 1];
                                                var call__1250 = Effect.FPath.Count;
                                                var call__1251 = Effect.FPath[call__1250 - 1];
                                                var call__1252 = Effect.FPath.Count;
                                                var call__1253 = Effect.FPath[call__1252 - 1];
                                                var call__1254 = Aow.Rtl.Rtl.Random(360);
                                                Aow.Units.aowFX.aowFX.MakeArcPath(call__1253.X, call__1249.Y - call__1251.Z, call__1254, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                            }
                                        }

                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FTargetX, FTargetY, (FFrame - 50) / 5, ref J, ref K);
                                        // { Original swaps the converted hex coordinates. }
                                        Aow.Units.HSEngine.HSEngine.HXtoHP(J, K, ref X, ref Y);
                                        var call__1255 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__1256 = call__1255.Field[J, K];
                                        var call__1257 = call__1256.FindOwnedChild(2228484);
                                        UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1257;
                                        bool bool__1258 = (UnitHS != null);
                                        if (bool__1258)
                                        {
                                            var call__1259 = UnitHS.FUnit.GetSide();
                                            var call__1260 = FTarget.GetSide();
                                            bool__1258 = (call__1259 != call__1260);
                                        }

                                        if (bool__1258)
                                        {
                                            Effect.FTarget = UnitHS.FUnit;
                                        }
                                        else
                                        {
                                            Effect.FTarget = null;
                                        }

                                        if (Effect.FTarget != null)
                                        {
                                            var call__1261 = Aow.Rtl.Rtl.Random(3);
                                            var call__1262 = Effect.FPath.Count;
                                            var call__1263 = Effect.FPath[call__1262 - 1];
                                            var call__1264 = Effect.FPath.Count;
                                            var call__1265 = Effect.FPath[call__1264 - 1];
                                            var call__1266 = Effect.FPath.Count;
                                            var call__1267 = Effect.FPath[call__1266 - 1];
                                            Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)call__1267.X), unchecked((short)(call__1263.Y - call__1265.Z)), unchecked((short)(X + 16)), unchecked((short)(Y - 8)), unchecked((short)(call__1261 + 6)), 100, ref Effect.FPath);
                                            {
                                                I = 0;
                                                int end__1268 = Aow.Rtl.Rtl.Random(3) + 5;
                                                for (; I <= end__1268; I += 1)
                                                {
                                                    var call__1269 = Effect.FPath.Count;
                                                    var call__1270 = Effect.FPath[call__1269 - 1];
                                                    var call__1271 = Effect.FPath.Count;
                                                    var call__1272 = Effect.FPath[call__1271 - 1];
                                                    var call__1273 = Effect.FPath.Count;
                                                    var call__1274 = Effect.FPath[call__1273 - 1];
                                                    var call__1275 = Effect.FPath.Count;
                                                    var call__1276 = Effect.FPath[call__1275 - 1];
                                                    var call__1277 = Effect.FPath.Count;
                                                    var call__1278 = Effect.FPath[call__1277 - 1];
                                                    var call__1279 = Effect.FPath.Count;
                                                    var call__1280 = Effect.FPath[call__1279 - 1];
                                                    Aow.Units.aowFX.aowFX.MakeLinePath(call__1280.X, call__1274.Y - call__1276.Z, call__1278.X, (call__1270.Y - call__1272.Z) - 6, 3, ref Effect.FPath);
                                                    var call__1281 = Effect.FPath.Count;
                                                    var call__1282 = Effect.FPath[call__1281 - 1];
                                                    var call__1283 = Effect.FPath.Count;
                                                    var call__1284 = Effect.FPath[call__1283 - 1];
                                                    var call__1285 = Effect.FPath.Count;
                                                    var call__1286 = Effect.FPath[call__1285 - 1];
                                                    var call__1287 = Effect.FPath.Count;
                                                    var call__1288 = Effect.FPath[call__1287 - 1];
                                                    var call__1289 = Effect.FPath.Count;
                                                    var call__1290 = Effect.FPath[call__1289 - 1];
                                                    var call__1291 = Effect.FPath.Count;
                                                    var call__1292 = Effect.FPath[call__1291 - 1];
                                                    Aow.Units.aowFX.aowFX.MakeLinePath(call__1292.X, call__1286.Y - call__1288.Z, call__1290.X, (call__1282.Y - call__1284.Z) + 6, 3, ref Effect.FPath);
                                                }
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
                        case Aow.Units.AoWTC.TfxType.fxChain:
                            NewPath = Aow.Units.aowFX.TXYZList.Create();
                            I = 0;
                            J = 0;
                            K = 0;
                            while (true)
                            {
                                var call__1293 = FPath.Count;
                                if (!(I < call__1293))
                                    break;
                                {
                                    var call__1294 = FPath[I];
                                    if (call__1294.Z > 0)
                                    {
                                        var call__1295 = FPath[I];
                                        if (call__1295.Z > 1)
                                        {
                                            var call__1296 = FPath[I];
                                            K = call__1296.Z;
                                        }

                                        var call__1297 = FPath[I];
                                        var call__1298 = FPath[I];
                                        var call__1299 = FPath[J];
                                        var call__1300 = FPath[J];
                                        Aow.Units.aowFX.aowFX.MakeLightningPath(call__1300.X, call__1299.Y, call__1298.X, call__1297.Y, ref NewPath);
                                        switch (K)
                                        {
                                            case -1:
                                                var call__1301 = NewPath.Count;
                                                var call__1302 = NewPath[call__1301 - 1];
                                                call__1302.Z = 16;
                                                var call__1303 = NewPath.Count;
                                                FFrame = call__1303 - 1;
                                                K = 0;
                                                break;
                                            case >= 2 and <= 16:
                                                var call__1304 = NewPath.Count;
                                                var call__1305 = NewPath[call__1304 - 1];
                                                call__1305.Z = K - 1;
                                                var call__1306 = NewPath.Count;
                                                FFrame = call__1306 - 1;
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
                            }

                            FPath.Free();
                            FPath = NewPath;
                            var call__1307 = FPath.Count;
                            if (FFrame >= (call__1307 - 1))
                            {
                                var call__1308 = FPath.Count;
                                FFrame = call__1308 - 1;
                                var call__1309 = FPath[FFrame];
                                if (call__1309.Z <= 1)
                                {
                                    FDone = true;
                                }
                            }

                            var call__1310 = FPath[FFrame];
                            if (call__1310.Z == 15)
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        var call__1311 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__1311.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__1312 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__1312.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__1313 = FPath[FFrame];
                                var call__1314 = FPath[FFrame];
                                var call__1315 = FPath[FFrame];
                                Aow.Units.HSEngine.HSEngine.HPtoHX(call__1315.X, call__1313.Y - call__1314.Z, ref I, ref J);
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(I, J, 0, 3);
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxTouch:
                        case Aow.Units.AoWTC.TfxType.fxEffect:
                            if (((FFrame == 0) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds) && (FAbilityIndex == 65))
                            {
                                var call__1316 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                call__1316.FSFXLibrary.Play(0);
                            }

                            if (FAbilityIndex == 52)
                            {
                                var call__1317 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                var call__1318 = call__1317.FCastImages.ImageSequences[0];
                                Sequence = call__1318;
                            }
                            else
                            {
                                var call__1319 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                var call__1320 = call__1319.FImageSequences.ImageSequences[0];
                                Sequence = call__1320;
                            }

                            if (Sequence != null)
                            {
                                var call__1321 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= (call__1321 - 1))
                                {
                                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            var call__1322 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__1322.FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            var call__1323 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__1323.FSFXLibrary.Play(1);
                                        }
                                    }

                                    var call__1324 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1324 - 1;
                                    FDone = true;
                                }
                            }
                            else
                            {
                                FFrame = -1;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxSacredWrath:
                            if (FAbilityIndex == 52)
                            {
                                var call__1325 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                var call__1326 = call__1325.FCastImages.ImageSequences[1];
                                Sequence = call__1326;
                            }
                            else
                            {
                                var call__1327 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                var call__1328 = call__1327.FImageSequences.ImageSequences[1];
                                Sequence = call__1328;
                            }

                            if (Sequence == null)
                            {
                                FFrame = -1;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxFBExplosion:
                            var call__1329 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                            var call__1330 = call__1329.FCastImages.ImageSequences[1];
                            Sequence = call__1330;
                            if (Sequence != null)
                            {
                                var call__1331 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= (call__1331 - 1))
                                {
                                    var call__1332 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1332 - 1;
                                    FDone = true;
                                }
                            }
                            else
                            {
                                FFrame = -1;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxScreenNESW:
                            if (FFrame == 0)
                            {
                                var call__1333 = FPath[0];
                                if (call__1333.X == 0)
                                {
                                    var call__1334 = FPath[0];
                                    call__1334.X = 100;
                                    var call__1335 = Aow.Rtl.Rtl.Random(50);
                                    var call__1336 = FPath[0];
                                    call__1336.Y = call__1335;
                                }
                                else
                                {
                                    var call__1337 = Aow.Rtl.Rtl.Random(50);
                                    var call__1338 = FPath[0];
                                    call__1338.X = call__1337 + 50;
                                    var call__1339 = FPath[0];
                                    call__1339.Y = 0;
                                }
                            }
                            else
                            {
                                var call__1340 = FPath[0];
                                call__1340.X -= 3;
                                var call__1341 = FPath[0];
                                call__1341.Y += 3;
                                var call__1342 = FPath[0];
                                if (call__1342.X < 0)
                                {
                                    FFrame = -1;
                                }
                                else
                                {
                                    var call__1343 = FPath[0];
                                    if (call__1343.Y > 100)
                                    {
                                        FFrame = -1;
                                    }
                                }
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxDamLightning:
                        case Aow.Units.AoWTC.TfxType.fxDamFire:
                        case Aow.Units.AoWTC.TfxType.fxDamPoison:
                        case Aow.Units.AoWTC.TfxType.fxDamMagic:
                        case Aow.Units.AoWTC.TfxType.fxDamHoly:
                        case Aow.Units.AoWTC.TfxType.fxDamDeath:
                        case Aow.Units.AoWTC.TfxType.fxDamCold:
                        case Aow.Units.AoWTC.TfxType.fxGeyserWater:
                        case Aow.Units.AoWTC.TfxType.fxFrost:
                        case Aow.Units.AoWTC.TfxType.fxSacFire:
                            var call__1344 = FPath.Count;
                            if (FFrame >= (call__1344 - 1))
                            {
                                var call__1345 = FPath.Count;
                                FFrame = call__1345 - 1;
                                FDone = true;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxWallSmoke:
                            var call__1346 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[5];
                            Sequence = call__1346;
                            if (Sequence == null)
                            {
                                FFrame = -1;
                            }
                            else
                            {
                                var call__1347 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= call__1347)
                                {
                                    var call__1348 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1348 - 1;
                                    FDone = true;
                                }
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxHitGround:
                            var call__1349 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[9];
                            Sequence = call__1349;
                            if (Sequence == null)
                            {
                                FFrame = -1;
                            }
                            else
                            {
                                var call__1350 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= call__1350)
                                {
                                    var call__1351 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1351 - 1;
                                    FDone = true;
                                }
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxHitWater:
                            var call__1352 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[10];
                            Sequence = call__1352;
                            if (Sequence == null)
                            {
                                FFrame = -1;
                            }
                            else
                            {
                                var call__1353 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= call__1353)
                                {
                                    var call__1354 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1354 - 1;
                                    FDone = true;
                                }
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxBlood:
                            var call__1355 = FPath[0];
                            var call__1356 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[call__1355.Z];
                            Sequence = call__1356;
                            if (Sequence == null)
                            {
                                FFrame = -1;
                            }
                            else
                            {
                                var call__1357 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= (call__1357 - 1))
                                {
                                    var call__1358 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1358 - 1;
                                    FDone = true;
                                }
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxTremors:
                            switch (FFrame)
                            {
                                case 0:
                                    var call__1359 = Aow.Units.aowFX.TXYZ.Create();
                                    FPath.Add(call__1359);
                                    var call__1360 = FPath[0];
                                    call__1360.X = Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewXhp;
                                    var call__1361 = FPath[0];
                                    call__1361.Y = Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewYhp;
                                    break;
                                case >= 1 and <= 29:
                                    var call__1362 = FPath[0];
                                    var call__1363 = Aow.Rtl.Rtl.Random(FFrame / 3);
                                    var call__1364 = FPath[0];
                                    var call__1365 = Aow.Rtl.Rtl.Random(FFrame / 3);
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.ChangeViewTo((call__1364.X + call__1365) - (FFrame / 6), (call__1362.Y + call__1363) - (FFrame / 6), Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                    break;
                                case >= 30 and <= 89:
                                    var call__1366 = FPath[0];
                                    var call__1367 = Aow.Rtl.Rtl.Random(10);
                                    var call__1368 = FPath[0];
                                    var call__1369 = Aow.Rtl.Rtl.Random(10);
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.ChangeViewTo((call__1368.X + call__1369) - 5, (call__1366.Y + call__1367) - 5, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                    break;
                                case >= 90 and <= 119:
                                    var call__1370 = FPath[0];
                                    var call__1371 = Aow.Rtl.Rtl.Random((119 - FFrame) / 3);
                                    var call__1372 = FPath[0];
                                    var call__1373 = Aow.Rtl.Rtl.Random((119 - FFrame) / 3);
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.ChangeViewTo((call__1372.X + call__1373) - ((119 - FFrame) / 6), (call__1370.Y + call__1371) - ((119 - FFrame) / 6), Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                    break;
                                case 120:
                                    FFrame = -1;
                                    break;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxCloud:
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
                        case Aow.Units.AoWTC.TfxType.fxCloudPuff:
                            if (FAbilityIndex == 52)
                            {
                                var call__1374 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                var call__1375 = call__1374.FCastImages.ImageSequences[0];
                                Sequence = call__1375;
                            }
                            else
                            {
                                var call__1376 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                var call__1377 = call__1376.FImageSequences.ImageSequences[0];
                                Sequence = call__1377;
                            }

                            if (Sequence != null)
                            {
                                var call__1378 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= (call__1378 - 1))
                                {
                                    var call__1379 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1379 - 1;
                                    FDone = true;
                                }
                            }
                            else
                            {
                                FFrame = -1;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxHighPrayer:
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
                                    int end__1380 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                                    for (; I <= end__1380; I += 1)
                                    {
                                        var call__1381 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                        bool bool__1382 = (call__1381 is Aow.Units.AoWE.TCombatUnit);
                                        if (bool__1382)
                                        {
                                            var call__1383 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            var call__1384 = call__1383.GetEnabled();
                                            bool__1382 = call__1384;
                                        }

                                        bool bool__1385 = bool__1382;
                                        if (bool__1385)
                                        {
                                            var call__1386 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            bool__1385 = (call__1386.FCombatPlayer == FTarget.FCombatPlayer);
                                        }

                                        if (bool__1385)
                                        {
                                            var call__1387 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                            Effect = call__1387;
                                            var call__1388 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            var call__1389 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__1388).FUnitHS.GetYhx();
                                            var call__1390 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            var call__1391 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__1390).FUnitHS.GetXhx();
                                            Aow.Units.HSEngine.HSEngine.HXtoHP(call__1391, call__1389, ref J, ref K);
                                            J = Aow.Rtl.Rtl.Random(20) + (J + 6);
                                            K = Aow.Rtl.Rtl.Random(20) + (K + 6);
                                            Aow.Units.aowFX.aowFX.MakeLinePath(J, K - 100, J, K, 50, ref Effect.FPath);
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                        }
                                    }
                                }

                                    break;
                                case 100:
                                {
                                    I = 0;
                                    int end__1392 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                                    for (; I <= end__1392; I += 1)
                                    {
                                        var call__1393 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                        bool bool__1394 = (call__1393 is Aow.Units.AoWE.TCombatUnit);
                                        if (bool__1394)
                                        {
                                            var call__1395 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            var call__1396 = call__1395.GetEnabled();
                                            bool__1394 = call__1396;
                                        }

                                        bool bool__1397 = bool__1394;
                                        if (bool__1397)
                                        {
                                            var call__1398 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            bool__1397 = (call__1398.FCombatPlayer == FTarget.FCombatPlayer);
                                        }

                                        if (bool__1397)
                                        {
                                            var call__1399 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxPrayerGlow);
                                            Effect = call__1399;
                                            var call__1400 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            var call__1401 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__1400).FUnitHS.GetYhx();
                                            var call__1402 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            var call__1403 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__1402).FUnitHS.GetXhx();
                                            Aow.Units.HSEngine.HSEngine.HXtoHP(call__1403, call__1401, ref J, ref K);
                                            var call__1404 = Aow.Units.aowFX.TXYZ.Create();
                                            Effect.FPath.Add(call__1404);
                                            var call__1405 = Effect.FPath[0];
                                            call__1405.X = J + 16;
                                            var call__1406 = Effect.FPath[0];
                                            call__1406.Y = K + 16;
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var call__1407 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            Effect.FTarget = call__1407;
                                        }
                                    }
                                }

                                    FFrame = -1;
                                    FTarget = null;
                                    break;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxResurrect:
                            switch (FFrame)
                            {
                                case 100:
                                    FFrame = -1;
                                    break;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxDecayLeg:
                            var call__1408 = FPath.Count;
                            if (FFrame >= call__1408)
                            {
                                FFrame = -1;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxGeyser:
                            switch (FFrame)
                            {
                                case 150:
                                    FFrame = -1;
                                    bool bool__1409 = (FTarget != null);
                                    if (bool__1409)
                                    {
                                        var call__1410 = FTarget.GetEnabled();
                                        bool__1409 = call__1410;
                                    }

                                    if (bool__1409)
                                    {
                                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                        {
                                            if (FAbilityIndex == 52)
                                            {
                                                var call__1411 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                                call__1411.FCastSounds.Play(1);
                                            }
                                            else
                                            {
                                                var call__1412 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                                call__1412.FSFXLibrary.Play(1);
                                            }
                                        }

                                        ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FOffsetY = 0;
                                        ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FBusy = false;
                                    }

                                    bool bool__1413 = (FTarget != null);
                                    if (bool__1413)
                                    {
                                        var call__1414 = ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnit.GetMoveTypes();
                                        bool__1413 = call__1414.Contains(Aow.Units.AoWE.TMoveType.mtFlying);
                                    }

                                    if (bool__1413)
                                    {
                                        FTarget = null;
                                    }

                                    break;
                                case >= 0 and <= 99:
                                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds && ((FFrame % 30) == 29))
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            var call__1415 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__1415.FCastSounds.Play(0);
                                        }
                                        else
                                        {
                                            var call__1416 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__1416.FSFXLibrary.Play(0);
                                        }
                                    }

                                    bool bool__1417 = (FTarget != null);
                                    if (bool__1417)
                                    {
                                        var call__1418 = FTarget.GetEnabled();
                                        bool__1417 = call__1418;
                                    }

                                    if (bool__1417)
                                    {
                                        var call__1419 = Aow.Rtl.Rtl.Random(2);
                                        ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FOffsetY = (call__1419 + -((FFrame * 3) / 2)) - 1;
                                        ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FBusy = true;
                                    }

                                    J = Aow.Rtl.Rtl.Random(6) + 20;
                                {
                                    I = 1;
                                    int end__1420 = J - 1;
                                    for (; I <= end__1420; I += 1)
                                    {
                                        var call__1421 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1421;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(10) + 10)) / 10);
                                        Gravity = (float)0.5;
                                        var call__1422 = Aow.Rtl.Rtl.Random(50);
                                        VZ = (float)(((double)(((double)call__1422) + (((double)(FFrame * 3)) * 0.5))) / 10);
                                        Z = 0;
                                        var call__1423 = FPath[0];
                                        var call__1424 = FPath[0];
                                        Aow.Units.aowFX.aowFX.MakeArcPath(call__1424.X, call__1423.Y, (I * 360) / J, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case >= 125 and <= 149:
                                    bool bool__1425 = (FTarget != null);
                                    if (bool__1425)
                                    {
                                        var call__1426 = FTarget.GetEnabled();
                                        bool__1425 = call__1426;
                                    }

                                    if (bool__1425)
                                    {
                                        ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FOffsetY = (FFrame - 150) * 6;
                                        ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.SetDirection(unchecked((byte)((FFrame % 6) + 1)));
                                    }

                                    break;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxSWComet:
                            ;
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
            catch (System.Exception)
            {
                // { Original puts the diagnostic in the message-box caption. }
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during NewFrame", AnsiString.FromPChar(Ptr<byte>.Unbox(((((AnsiString)"Error - fxHS ") + Aow.Rtl.Rtl.IntToStr(FAbilityIndex)) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(FSpellIndex))), (int)0);
            }
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
                    byte end__1427 = 0;
                    for (; I >= end__1427; I -= 1)
                    {
                        var call__1428 = FItems.GetItem(I);
                        call__1428.SetOwner(Items);
                    }
                }
            }
            else
            {
                var call__1429 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL);
                var call__1430 = call__1429.FindOwnedChild(131697);
                ItemHS = (Aow.Units.AoWE.TItemHS)call__1430;
                if (ItemHS == null)
                {
                    ItemHS = Aow.Units.AoWE.TItemHS.Create(null);
                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(ItemHS, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL);
                }

                {
                    I = FItems.GetCount() - 1;
                    byte end__1431 = 0;
                    for (; I >= end__1431; I -= 1)
                    {
                        var call__1432 = FItems.GetItem(I);
                        call__1432.SetOwner(ItemHS.FItems);
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
            var call__1433 = FImages[FImageIndex];
            call__1433.ShowClipped((X - XhpSurf) - 8, (Y - YhpSurf) - 8, SurfaceDesc);
        }

        // AoWTC.pas:5031
        public override void Activate()
        {
            Aow.Units.HSEngine.TXYL P = default;
            base.Activate();
            bool bool__1434 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting);
            if (bool__1434)
            {
                var call__1435 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.ValidatePosition(ref FPosition, ref FIndex);
                bool__1434 = call__1435;
            }

            if (bool__1434)
            {
                var call__1436 = GetYhx();
                var call__1437 = GetXhx();
                var call__1438 = Aow.Units.HSEngine.HSEngine.PackXYL(unchecked((sbyte)call__1437), unchecked((sbyte)call__1436), 0);
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.SetUnitPosition(FPosition, FIndex, call__1438);
                var call__1439 = Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TCUNITS.ILB");
                FImages = call__1439;
                UpdateImageIndex();
            }
            else
            {
                var call__1440 = GetXhx();
                P.X = unchecked((sbyte)call__1440);
                var call__1441 = GetYhx();
                P.Y = unchecked((sbyte)call__1441);
                P.L = 0;
                P.Flag = unchecked((sbyte)FDirection);
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.SetUnitPosition(FPosition, FIndex, P);
                Destroy();
            }
        }

        // AoWTC.pas:5049
        public override void Deactivate()
        {
            var call__1442 = Activated();
            if (call__1442 && ((Aow.Units.HSEngine.TMapLevel)((Aow.Units.HSEngine.TMapField)Owner).Owner).Map.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TCUNITS.ILB");
                var call__1443 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(FPosition, FIndex);
                var call__1444 = GetXhx();
                bool bool__1445 = (call__1443.X == call__1444);
                if (bool__1445)
                {
                    var call__1446 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(FPosition, FIndex);
                    var call__1447 = GetYhx();
                    bool__1445 = (call__1446.Y == call__1447);
                }

                if (bool__1445)
                {
                    var call__1448 = Aow.Units.HSEngine.HSEngine.PackXYL(unchecked((sbyte)-1), unchecked((sbyte)-1), unchecked((sbyte)-1));
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.SetUnitPosition(FPosition, FIndex, call__1448);
                }
            }

            base.Deactivate();
        }

        // AoWTC.pas:5068
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            var call__1449 = Map.GetField(Xhx, Yhx, Lhx);
            var call__1450 = call__1449.FindOwnedChild(2228483);
            bool bool__1451 = (call__1450 == null);
            if (bool__1451)
            {
                var call__1452 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.ValidatePosition(ref FPosition, ref FIndex);
                bool__1451 = call__1452;
            }

            Result = bool__1451;
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
            var call__1453 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[4];
            Sequence = call__1453;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref X, ref Y);
            if (Sequence != null)
            {
                var call__1454 = Sequence.GetUnloopedFrameIndex(FFrame);
                Frame = call__1454;
                var call__1455 = Sequence.ImageLibrary[Frame];
                call__1455.ShowClipped((X - XhpSurf) + FOffsetX, (Y - YhpSurf) + FOffsetY, SurfaceDesc);
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
                var call__1456 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[FSequenceIndex];
                Sequence = call__1456;
                while (Sequence != null)
                {
                    var call__1457 = Sequence.GetUnloopedFrameIndex(FSequenceFrame);
                    Frame = call__1457;
                    if (Frame != -1)
                    {
                        var call__1458 = Sequence.ImageLibrary[Frame];
                        call__1458.Show((XhpSurf + FOffsetX) - 5, (YhpSurf + FOffsetY) + 10, SurfaceDesc);
                    }

                    Sequence = Sequence.Next;
                }
            }

            var call__1459 = FUnitGFX.FSequences.ImageSequences[30];
            Sequence = call__1459;
            while (Sequence != null)
            {
                var call__1460 = Sequence.GetUnloopedFrameIndex(FFrame);
                Frame = call__1460;
                if (Frame != -1)
                {
                    var call__1461 = Sequence.ImageLibrary[Frame];
                    call__1461.Show(XhpSurf + FOffsetX, YhpSurf + FOffsetY, SurfaceDesc);
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
                    var call__1462 = GetYhx();
                    var call__1463 = GetXhx();
                    var call__1464 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__1465 = call__1464.Field[call__1463, call__1462];
                    var call__1466 = call__1465.FindOwnedChild(2228484);
                    if (call__1466 == null)
                    {
                        var call__1467 = GetYhx();
                        var call__1468 = GetXhx();
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(call__1468, call__1467, 0, 5);
                        var call__1469 = GetYhx();
                        var call__1470 = GetXhx();
                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__1470, call__1469, ref X, ref Y);
                        {
                            Ring = 1;
                            byte end__1471 = 3;
                            for (; Ring <= end__1471; Ring += 1)
                            {
                                N = Aow.Rtl.Rtl.Random(5);
                                DX = Aow.Rtl.Rtl.Random(20) + (Ring * 30);
                                DY = Aow.Rtl.Rtl.Random(20) + (Ring * 30);
                                {
                                    I = 0;
                                    byte end__1472 = 17;
                                    for (; I <= end__1472; I += 1)
                                    {
                                        var call__1473 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1473;
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

                                        var call__1474 = System.Math.PI;
                                        var call__1475 = System.Math.Sin(((double)(((double)((I * 5) + N)) * call__1474)) / 180);
                                        var call__1476 = System.Math.PI;
                                        var call__1477 = System.Math.Cos(((double)(((double)((I * 5) + N)) * call__1476)) / 180);
                                        Aow.Units.aowFX.aowFX.MakeLinePath(Aow.Rtl.Rtl.Round(((double)call__1477) * DX) + (X + 16), (Aow.Rtl.Rtl.Round(((double)call__1475) * DY) / 2) + (Y + 16), X + 16, Y + 16, 100, ref Effect.FPath);
                                    }
                                }
                            }
                        }

                        return;
                    }
                }

                if ((Msg.MsgID == 200713) && (Msg.HN == 0))
                {
                    var call__1478 = GetYhx();
                    var call__1479 = GetXhx();
                    var call__1480 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__1481 = call__1480.Field[call__1479, call__1478];
                    var call__1482 = call__1481.FindOwnedChild(2228484);
                    if (call__1482 == null)
                    {
                        var call__1483 = GetYhx();
                        var call__1484 = GetXhx();
                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__1484, call__1483, ref X, ref Y);
                        N = Aow.Rtl.Rtl.Random(6) + 25;
                        {
                            I = 1;
                            int end__1485 = N - 1;
                            for (; I <= end__1485; I += 1)
                            {
                                var call__1486 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                Effect = call__1486;
                                Effect.FAbilityIndex = 52;
                                Effect.FSpellIndex = Msg.Param[1].Value;
                                // { Original uses this smaller value for horizontal speed. }
                                Speed = (float)(((double)(Aow.Rtl.Rtl.Random(10) + 10)) * 0.05);
                                Gravity = (float)0.5;
                                VZ = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 50)) * 0.05);
                                Z = 0;
                                Aow.Units.aowFX.aowFX.MakeArcPath(X + 16, Y + 16, (I * 360) / N, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                            }
                        }

                        switch (Msg.Param[1].Value)
                        {
                            case 130:
                                var call__1487 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                Aow.Rtl.Rtl.RandSeed = call__1487;
                                var call__1488 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(130);
                                var call__1489 = call__1488.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                AnimateAction = call__1489;
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(AnimateAction);
                                AnimateAction.Release();
                                break;
                            case 129:
                                var call__1490 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                Aow.Rtl.Rtl.RandSeed = call__1490;
                                var call__1491 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(129);
                                var call__1492 = call__1491.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                RaiseAction = call__1492;
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(RaiseAction);
                                RaiseAction.Release();
                                break;
                        }

                        var call__1493 = FUnit.GetEnabled();
                        if (call__1493)
                        {
                            UnitHS = Aow.Units.AoWTC.TTacticalCombatUnitHS.Create(null);
                            UnitHS.Initialize(FUnit);
                            var call__1494 = Aow.Rtl.Rtl.Random(6);
                            UnitHS.SetDirection(unchecked((byte)(call__1494 + 1)));
                            var call__1495 = GetYhx();
                            var call__1496 = GetXhx();
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(UnitHS, call__1496, call__1495, 0);
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
            var call__1497 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[FSequenceIndex];
            Sequence = call__1497;
            var call__1498 = Sequence.GetTotalSequenceFrames();
            if ((FSequenceFrame + 1) < call__1498)
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
            var call__1499 = GetYhx();
            var call__1500 = GetXhx();
            Result = (1744830464 + (call__1499 << 13)) + call__1500;
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
                    var call__1501 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[26];
                    Sequence = call__1501;
                    break;
                case 12:
                    var call__1502 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[27];
                    Sequence = call__1502;
                    break;
                default:
                    var call__1503 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[25];
                    Sequence = call__1503;
                    break;
            }

            while (Sequence != null)
            {
                var call__1504 = Sequence.GetTotalSequenceFrames();
                var call__1505 = Sequence.GetUnloopedFrameIndex(Aow.Rtl.Rtl.ObjectToInt(this) % call__1504);
                Frame = call__1505;
                if (Frame != -1)
                {
                    var call__1506 = Sequence.ImageLibrary[Frame];
                    call__1506.Show(XhpSurf, YhpSurf, SurfaceDesc);
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
            var call__1507 = Aow.Units.HSEngine.TXYLList.Create();
            FPositions = call__1507;
            var call__1508 = Aow.Units.HSEngine.HSEngine.PackXYL(unchecked((sbyte)-1), unchecked((sbyte)-1), unchecked((sbyte)-1));
            FPositions.Default = call__1508;
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
                var call__1509 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(X, Y, L);
                if (call__1509.TerrainType != 15)
                {
                    var call__1510 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(X, Y, L);
                    var call__1511 = call__1510.FindOwnedChild(2228483);
                    HS = (Aow.Units.AoWTC.TUnitPositionHS)call__1511;
                    if (HS == null)
                    {
                        HS = Aow.Units.AoWTC.TUnitPositionHS.Create(null);
                        var call__1512 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(HS, X, Y, L);
                        if (!call__1512)
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
                var call__1513 = FPositions[I];
                Result = call__1513;
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
            var call__1514 = FPositions[(Position * 8) + Index];
            Result = call__1514.X == -1;
            if (!Result)
            {
                Index = 0;
                while (true)
                {
                    var call__1515 = FPositions[(Position * 8) + Index];
                    if (!((call__1515.X != -1) && (Index != 8)))
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
                        var call__1516 = FPositions[(Position * 8) + Index];
                        if (!((call__1516.X != -1) && (Index != 8)))
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
            bool bool__1517 = (A1 == unchecked((byte)FCombatPlayer));
            if (bool__1517)
            {
                var call__1518 = GetEnabled();
                bool__1517 = call__1518;
            }

            if (bool__1517)
            {
                var call__1519 = GetMoves();
                FMovePoints = call__1519;
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
                var call__1520 = GetEnabled();
                bool bool__1521 = call__1520 && (FUnitHS != null);
                if (bool__1521)
                {
                    var call__1522 = FUnit.GetBloodType();
                    bool__1521 = (call__1522 != Aow.Units.AoWE.TBloodType.btNone);
                }

                if (bool__1521)
                {
                    var call__1523 = GetHitPoints();
                    bool bool__1524 = (A1 >= call__1523);
                    if (bool__1524)
                    {
                        var call__1525 = GetAbilityEnabled(131);
                        bool__1524 = (call__1525 || (FDeathMethod == Aow.Units.AoWTC.TTCDeathMethod.dmExplode));
                    }

                    if (bool__1524)
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

                        var call__1526 = FUnitHS.GetYhx();
                        var call__1527 = FUnitHS.GetXhx();
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__1527, call__1526, Direction, ref X, ref Y);
                        var call__1528 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__1529 = call__1528.Field[X, Y];
                        bool bool__1530 = (call__1529.TerrainType != 0);
                        if (bool__1530)
                        {
                            var call__1531 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1532 = call__1531.Field[X, Y];
                            bool__1530 = (call__1532.TerrainType != 9);
                        }

                        if (bool__1530)
                        {
                            var call__1533 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1534 = call__1533.Field[X, Y];
                            var call__1535 = Aow.Units.AoWTC.TCBloodHS.Create(call__1534);
                            Blood = call__1535;
                            Blood.Activate();
                            var call__1536 = Aow.Rtl.Rtl.Random(32);
                            Blood.FOffsetX = call__1536 - 32;
                            var call__1537 = Aow.Rtl.Rtl.Random(32);
                            Blood.FOffsetY = call__1537 - 32;
                            var call__1538 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[4];
                            var call__1539 = call__1538.GetTotalSequenceFrames();
                            var call__1540 = Aow.Rtl.Rtl.Random(call__1539);
                            Blood.FFrame = call__1540;
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
                            byte end__1541 = 9;
                            for (; I <= end__1541; I += 1)
                            {
                                var call__1542 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxDecayComet);
                                Effect = call__1542;
                                Effect.FAbilityIndex = 52;
                                Effect.FSpellIndex = 106;
                                var call__1543 = FUnitHS.GetYhx();
                                var call__1544 = FUnitHS.GetXhx();
                                Aow.Units.HSEngine.HSEngine.HXtoHP(call__1544, call__1543, ref X, ref Y);
                                var call__1545 = Aow.Units.aowFX.TXYZ.Create();
                                Effect.FPath.Add(call__1545);
                                var call__1546 = Effect.FPath[0];
                                call__1546.X = X + 16;
                                var call__1547 = Effect.FPath[0];
                                call__1547.Y = Y + 8;
                                {
                                    J = 0;
                                    byte end__1548 = 10;
                                    for (; J <= end__1548; J += 1)
                                    {
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 50)) * 0.05);
                                        Gravity = (float)1.5;
                                        VZ = (float)(((double)(Aow.Rtl.Rtl.Random(25) + 25)) * 0.05);
                                        Z = 0;
                                        var call__1549 = Effect.FPath.Count;
                                        var call__1550 = Effect.FPath[call__1549 - 1];
                                        var call__1551 = Effect.FPath.Count;
                                        var call__1552 = Effect.FPath[call__1551 - 1];
                                        var call__1553 = Effect.FPath.Count;
                                        var call__1554 = Effect.FPath[call__1553 - 1];
                                        var call__1555 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(call__1554.X, call__1550.Y - call__1552.Z, call__1555, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var call__1556 = FUnitHS.GetYhx();
                        var call__1557 = FUnitHS.GetXhx();
                        var call__1558 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__1559 = call__1558.Field[call__1557, call__1556];
                        bool bool__1560 = (call__1559.TerrainType != 0);
                        if (bool__1560)
                        {
                            var call__1561 = FUnitHS.GetYhx();
                            var call__1562 = FUnitHS.GetXhx();
                            var call__1563 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1564 = call__1563.Field[call__1562, call__1561];
                            bool__1560 = (call__1564.TerrainType != 9);
                        }

                        if (bool__1560)
                        {
                            var call__1565 = FUnitHS.GetYhx();
                            var call__1566 = FUnitHS.GetXhx();
                            var call__1567 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1568 = call__1567.Field[call__1566, call__1565];
                            var call__1569 = Aow.Units.AoWTC.TCCorpseHS.Create(call__1568);
                            Corpse = call__1569;
                            Corpse.FUnitGFX = FUnitHS.FUnitGFXResource;
                            var call__1570 = Corpse.FUnitGFX.FSequences.ImageSequences[30];
                            Sequence = call__1570;
                            if (Sequence != null)
                            {
                                var call__1571 = Sequence.GetTotalSequenceFrames();
                                var call__1572 = Aow.Rtl.Rtl.Random(call__1571);
                                Corpse.FFrame = call__1572;
                            }

                            switch (FDeathMethod)
                            {
                                case Aow.Units.AoWTC.TTCDeathMethod.dmNormal:
                                    var call__1573 = FUnit.GetBloodType();
                                    switch (call__1573)
                                    {
                                        case Aow.Units.AoWE.TBloodType.btNone:
                                            Corpse.FSequenceIndex = -1;
                                            break;
                                        default:
                                        {
                                            var call__1574 = FUnit.GetUnitSize();
                                            Corpse.FSequenceIndex = (unchecked((int)call__1574) & 127) + 30;
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

                            var call__1575 = Aow.Rtl.Rtl.Random(10);
                            Corpse.FOffsetX = call__1575 - 5;
                            var call__1576 = Aow.Rtl.Rtl.Random(10);
                            Corpse.FOffsetY = call__1576 - 5;
                            var call__1577 = FUnit.GetUnitType();
                            if (call__1577 != Aow.Units.AoWE.TUnitType.utMachine)
                            {
                                Corpse.FUnit = this;
                            }

                            Corpse.Activate();
                        }
                    }

                    var call__1578 = FUnitHS.GetYhx();
                    var call__1579 = FUnitHS.GetXhx();
                    var call__1580 = FUnitHS.GetYhx();
                    var call__1581 = FUnitHS.GetXhx();
                    var call__1582 = Aow.Rtl.Rtl.Rect(call__1581, call__1580, call__1579 + 1, call__1578 + 1);
                    R = call__1582;
                    FUnitHS.Free();
                    FUnitHS = null;
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.TriggerMapChangedEvent(R, 0);
                }
            }
            else
            {
                if (FUnitHS != null)
                {
                    var call__1583 = FUnit.GetUnitGFXResourceIndex();
                    FUnitHS.SetUnitGFXResourceIndex(call__1583);
                }
            }

            return Result;
        }

        // AoWTC.pas:2605
        public int GetPosValue()
        {
            int Result = default;
            var call__1584 = GetAttack();
            var call__1585 = GetDamage();
            var call__1586 = GetDefense();
            var call__1587 = GetMoves();
            Result = ((call__1584 + call__1585) + call__1586) + (call__1587 / 3);
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
                var call__1588 = FUnitHS.GetYhx();
                var call__1589 = FUnitHS.GetXhx();
                Aow.Units.CombatFire.CombatFire.CreateFire(false, 50, call__1589, call__1588);
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
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int Index = default;
            int X = default;
            int Y = default;
            int DX = default;
            int DY = default;
            double Progress = default;
            int NX = default;
            int NY = default;
            int FlyingOffset = default;
            int Race = default;
            Aow.Units.CityWall.TCityWall Wall = default;
            Aow.Units.CityWall.TCityWall NextWall = default;
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            try
            {
                if (!(FDirection < FUnitGFXResource.FSequences.Count))
                {
                    Aow.Units.AoWTC.AoWTC.OriginalUnitAssert((AnsiString)"Invalid Unit Index", (AnsiString)"D:\\AoWDev\\AoWTC\\inc\\TCUnitHS.pas", 4262);
                }

                Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref X, ref Y);
                XhpSurf = (X - XhpSurf) + FOffsetX;
                YhpSurf = ((Y - YhpSurf) - 8) + FOffsetY;
                var call__1591 = FUnit.GetPlayer();
                if (call__1591 > 0)
                {
                    var call__1592 = FUnit.GetPlayer();
                    var call__1593 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1592);
                    Race = call__1593.FPlayerRace;
                }
                else
                {
                    Race = 32;
                }

                var call__1594 = FUnit.GetAbilityEnabled(1);
                if (call__1594)
                {
                    FlyingOffset = -8;
                }
                else
                {
                    FlyingOffset = 0;
                }

                var call__1595 = FUnit.GetAbilityEnabled(36);
                if (call__1595)
                {
                    var call__1596 = Owner.FindOwnedChild(2229508);
                    Wall = (Aow.Units.CityWall.TCityWall)call__1596;
                    if (Wall == null)
                    {
                        var call__1597 = Owner.FindOwnedChild(2229510);
                        Wall = (Aow.Units.CityWall.TCityWall)call__1597;
                    }

                    if (FMoveStep == -1)
                    {
                        if ((Wall != null) && (Wall.FHitPoints > 0))
                        {
                            YhpSurf -= 32;
                        }
                    }
                    else
                    {
                        var call__1598 = GetYhx();
                        var call__1599 = GetXhx();
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__1599, call__1598, FDirection, ref NX, ref NY);
                        var call__1600 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__1601 = call__1600.Field[NX, NY];
                        var call__1602 = call__1601.FindOwnedChild(2229508);
                        NextWall = (Aow.Units.CityWall.TCityWall)call__1602;
                        if (NextWall == null)
                        {
                            var call__1603 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1604 = call__1603.Field[NX, NY];
                            var call__1605 = call__1604.FindOwnedChild(2229510);
                            NextWall = (Aow.Units.CityWall.TCityWall)call__1605;
                        }

                        var call__1606 = GetMoveSteps();
                        if (FMoveStep < (call__1606 / 2))
                        {
                            if ((Wall != null) && (Wall.FHitPoints > 0))
                            {
                                if ((NextWall != null) && (NextWall.FHitPoints > 0))
                                {
                                    YhpSurf -= 32;
                                }
                                else
                                {
                                    var call__1607 = GetMoveSteps();
                                    var call__1608 = GetMoveSteps();
                                    YhpSurf -= ((call__1607 - FMoveStep) * 32) / call__1608;
                                }
                            }
                        }
                        else
                        {
                            if ((NextWall != null) && (NextWall.FHitPoints > 0))
                            {
                                if ((Wall != null) && (Wall.FHitPoints > 0))
                                {
                                    YhpSurf -= 32;
                                }
                                else
                                {
                                    var call__1609 = GetMoveSteps();
                                    YhpSurf -= (FMoveStep * 32) / call__1609;
                                }
                            }
                        }
                    }
                }

                switch (FAction)
                {
                    case Aow.Units.AoWTC.TTCAction.caMove:
                    case Aow.Units.AoWTC.TTCAction.caBusy:
                    case Aow.Units.AoWTC.TTCAction.caFreeStruck:
                        if ((FAction == Aow.Units.AoWTC.TTCAction.caMove) && (FMoveStep != -1))
                        {
                            Aow.Units.HSEngine.HSEngine.HNtoHP(FDirection, ref DX, ref DY);
                            var call__1610 = GetMoveSteps();
                            Progress = ((double)FMoveStep) / call__1610;
                            XhpSurf += Aow.Rtl.Rtl.Round(((double)DX) * Progress);
                            YhpSurf += Aow.Rtl.Rtl.Round(((double)DY) * Progress);
                            var call__1611 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 5];
                            Sequence = call__1611;
                        }
                        else
                        {
                            var call__1612 = FUnitGFXResource.FSequences.ImageSequences[FDirection - 1];
                            Sequence = call__1612;
                        }

                        while (Sequence != null)
                        {
                            var call__1613 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                            Index = call__1613;
                            if (Index != -1)
                            {
                                var call__1614 = Sequence.ImageLibrary[Index];
                                call__1614.Show(XhpSurf + Sequence.dXshift, (YhpSurf + Sequence.dYshift) + FlyingOffset, SurfaceDesc);
                            }

                            Sequence = Sequence.Next;
                        }

                        break;
                    case Aow.Units.AoWTC.TTCAction.caMeleeHit:
                    case Aow.Units.AoWTC.TTCAction.caMeleeMiss:
                    case Aow.Units.AoWTC.TTCAction.caAbility:
                    case Aow.Units.AoWTC.TTCAction.caBreathAttack:
                        var call__1615 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                        Sequence = call__1615;
                        if (Sequence != null)
                        {
                            while (Sequence != null)
                            {
                                var call__1616 = Sequence.GetUnloopedFrameIndex(FActionFrame);
                                Index = call__1616;
                                if (Index != -1)
                                {
                                    var call__1617 = Sequence.ImageLibrary[Index];
                                    call__1617.Show(XhpSurf + Sequence.dXshift, (YhpSurf + Sequence.dYshift) + FlyingOffset, SurfaceDesc);
                                }

                                Sequence = Sequence.Next;
                            }
                        }

                        break;
                    case Aow.Units.AoWTC.TTCAction.caHit:
                        var call__1618 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 17];
                        Sequence = call__1618;
                        if (Sequence != null)
                        {
                            while (Sequence != null)
                            {
                                var call__1619 = Sequence.GetUnloopedFrameIndex(FActionFrame);
                                Index = call__1619;
                                if (Index != -1)
                                {
                                    var call__1620 = Sequence.ImageLibrary[Index];
                                    call__1620.Show(XhpSurf + Sequence.dXshift, (YhpSurf + Sequence.dYshift) + FlyingOffset, SurfaceDesc);
                                }

                                Sequence = Sequence.Next;
                            }
                        }

                        break;
                    case Aow.Units.AoWTC.TTCAction.caStepUp:
                    case Aow.Units.AoWTC.TTCAction.caStepBack:
                        var call__1621 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 5];
                        Sequence = call__1621;
                        if (Sequence != null)
                        {
                            while (Sequence != null)
                            {
                                var call__1622 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame + FActionFrame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                                Index = call__1622;
                                if (Index != -1)
                                {
                                    var call__1623 = Sequence.ImageLibrary[Index];
                                    call__1623.Show(XhpSurf + Sequence.dXshift, (YhpSurf + Sequence.dYshift) + FlyingOffset, SurfaceDesc);
                                }

                                Sequence = Sequence.Next;
                            }
                        }

                        break;
                    case Aow.Units.AoWTC.TTCAction.caRoundAttack:
                    case Aow.Units.AoWTC.TTCAction.caRoundAttackHit:
                        var call__1624 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[FDirection + 15];
                        Sequence = call__1624;
                        if (Sequence != null)
                        {
                            while (Sequence != null)
                            {
                                var call__1625 = Sequence.GetUnloopedFrameIndex(FActionFrame);
                                Index = call__1625;
                                if (Index != -1)
                                {
                                    if (Sequence.Layer == 0)
                                    {
                                        while (true)
                                        {
                                            var call__1626 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                                            var call__1627 = call__1626.ImageLibrary.ImageExist(Index);
                                            if (!!call__1627)
                                                break;
                                            {
                                                Index -= 1;
                                            }
                                        }

                                        var call__1628 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                                        var call__1629 = call__1628.ImageLibrary[Index];
                                        call__1629.Show((XhpSurf + Sequence.dXshift) + 16, ((YhpSurf + FlyingOffset) + Sequence.dYshift) + 16, SurfaceDesc);
                                    }
                                    else
                                    {
                                        var call__1630 = Sequence.ImageLibrary[Index];
                                        call__1630.Show((XhpSurf + Sequence.dXshift) + 16, ((YhpSurf + Sequence.dYshift) + FlyingOffset) + 16, SurfaceDesc);
                                    }
                                }

                                Sequence = Sequence.Next;
                            }
                        }

                        break;
                }

                if (FUnit.FUnit.FUnitStatus.Contains(Aow.Units.AoWE.TUnitStatusElement.usEnchanted))
                {
                    Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ShowLooped(70, XhpSurf + 16, (YhpSurf + 18) + FlyingOffset, Aow.Units.AoWE.AoWE.AoWHSMap.Frame, SurfaceDesc);
                }

                {
                    NX = 0;
                    byte end__1631 = 13;
                    for (; NX <= end__1631; NX += 1)
                    {
                        var call__1632 = FUnit.GetAbilityEnabled(Aow.Units.AoWTC.AoWTC.StatusAbils[NX]);
                        if (call__1632)
                        {
                            var call__1633 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Units.AoWTC.AoWTC.StatusAbils[NX]);
                            var call__1634 = call__1633.FImageSequences.ImageSequences[0];
                            Sequence = call__1634;
                            while (Sequence != null)
                            {
                                var call__1635 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                                Index = call__1635;
                                if (Index != -1)
                                {
                                    var call__1636 = Sequence.ImageLibrary[Index];
                                    call__1636.Show((XhpSurf + Sequence.dXshift) + 15, ((YhpSurf + Sequence.dYshift) + 10) + FlyingOffset, SurfaceDesc);
                                }

                                Sequence = Sequence.Next;
                            }
                        }
                    }
                }

                var call__1637 = GetSelected();
                if (!call__1637 || ((Aow.Units.AoWTC.AoWTC.AoWCombatMap.Frame & 6) != 6))
                {
                    if (Race == 32)
                    {
                        if ((FUnit.FMovePoints == 0) || (FUnit.FCombatPlayer != Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer))
                        {
                            var call__1638 = Aow.Units.AoWE.AoWE.AoWHSSet.FFlagControl.FFlags[70];
                            call__1638.Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                        }
                        else
                        {
                            var call__1639 = Aow.Units.AoWE.AoWE.AoWHSSet.FFlagControl.FFlags[FMoveIndicator + 69];
                            call__1639.Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                        }
                    }
                    else
                    {
                        if ((FUnit.FMovePoints == 0) || (FUnit.FCombatPlayer != Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer))
                        {
                            var call__1640 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Race);
                            var call__1641 = call__1640.FImageLibrary[70];
                            call__1641.Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                        }
                        else
                        {
                            var call__1642 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Race);
                            var call__1643 = call__1642.FImageLibrary[FMoveIndicator + 69];
                            call__1643.Show(XhpSurf - 5, YhpSurf - 12, SurfaceDesc);
                        }
                    }

                    SD = SurfaceDesc;
                    var call__1644 = FUnit.GetHitPoints();
                    var call__1645 = FUnit.GetHits();
                    NX = (call__1644 * 10) / call__1645;
                    if (NX == 0)
                    {
                        NX = 1;
                    }

                    if (((XhpSurf - 4) + NX) < SD.ClipRect.Right)
                    {
                        SD.ClipRect.Right = (XhpSurf - 4) + NX;
                    }

                    var call__1646 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatImages[1];
                    call__1646.ShowClipped(XhpSurf - 4, YhpSurf - 4, SD);
                }
            }
            catch (System.Exception)
            {
                var call__1590 = FUnit.FUnit.GetName();
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during ShowUnit", AnsiString.FromPChar(Ptr<byte>.Unbox(((AnsiString)"Error - ") + call__1590)), (int)0);
            }
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
                    var call__1647 = FUnit.GetPlayer();
                    if (call__1647 != 0)
                    {
                        var call__1648 = FUnit.GetPlayer();
                        var call__1649 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1648);
                        Msg.Param[0] = Msg.Param[0].Value | (call__1649.FPlayerRace + 1);
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
                    var call__1650 = FUnit.GetPlayer();
                    Msg.Result = call__1650;
                }
                else
                {
                    if ((Msg.MsgID == 200704) && (Msg.HN == 0))
                    {
                        var call__1651 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__1651;
                        var call__1652 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(65);
                        var call__1653 = ((Aow.Units.AoWE.TSelfDestructAbility)call__1652).CreateSelfDestructCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit);
                        SelfDestruct = call__1653;
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
                            var call__1654 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__1654;
                            var call__1655 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(117);
                            var call__1656 = call__1655.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                            SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1656;
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
                                var call__1657 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                Aow.Rtl.Rtl.RandSeed = call__1657;
                                var call__1658 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(113);
                                var call__1659 = call__1658.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1659;
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
                                    var call__1660 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                    Aow.Rtl.Rtl.RandSeed = call__1660;
                                    var call__1661 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(31);
                                    var call__1662 = ((Aow.Units.AoWE.TRangedAttackAbility)call__1661).CreateRangedAttackCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, 0);
                                    RangedDamage = call__1662;
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
                                        var call__1663 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                        Aow.Rtl.Rtl.RandSeed = call__1663;
                                        var call__1664 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Msg.Param[1].Value);
                                        var call__1665 = call__1664.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                        SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1665;
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
                                            var call__1666 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                            Aow.Rtl.Rtl.RandSeed = call__1666;
                                            var call__1667 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Msg.Param[1].Value);
                                            var call__1668 = ((Aow.Units.AoWE.TRangedAttackAbility)call__1667).CreateRangedAttackCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, 0);
                                            RangedDamage = call__1668;
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
                                                var call__1669 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                Aow.Rtl.Rtl.RandSeed = call__1669;
                                                var call__1670 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(104);
                                                var call__1671 = call__1670.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1671;
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
                                                    var call__1672 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                    Aow.Rtl.Rtl.RandSeed = call__1672;
                                                    var call__1673 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(120);
                                                    var call__1674 = call__1673.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                    SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1674;
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
                                                        var call__1675 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                        Aow.Rtl.Rtl.RandSeed = call__1675;
                                                        var call__1676 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(116);
                                                        var call__1677 = call__1676.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                        SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1677;
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
                                                            var call__1678 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                            Aow.Rtl.Rtl.RandSeed = call__1678;
                                                            var call__1679 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(127);
                                                            var call__1680 = call__1679.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                            SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1680;
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
                                                                var call__1681 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                                Aow.Rtl.Rtl.RandSeed = call__1681;
                                                                var call__1682 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(128);
                                                                var call__1683 = call__1682.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                                SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1683;
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
                                                                        var call__1684 = Aow.Rtl.Rtl.Random((Msg.Param[0].Value * 2) + 1);
                                                                        FOffsetX = call__1684 - Msg.Param[0].Value;
                                                                        var call__1685 = Aow.Rtl.Rtl.Random((Msg.Param[0].Value * 2) + 1);
                                                                        FOffsetY = call__1685 - Msg.Param[0].Value;
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
                                                                                var call__1686 = FUnit.FUnit.GetMoveTypes();
                                                                                if ((!call__1686.Contains(Aow.Units.AoWE.TMoveType.mtFlying) && (((Aow.Units.HSEngine.TMapField)Owner).TerrainType != 0)) && (((Aow.Units.HSEngine.TMapField)Owner).TerrainType != 9))
                                                                                {
                                                                                    var call__1687 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                                                    Aow.Rtl.Rtl.RandSeed = call__1687;
                                                                                    var call__1688 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(112);
                                                                                    var call__1689 = call__1688.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                                                    SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1689;
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
                var call__1690 = GetYhx();
                var call__1691 = GetXhx();
                var call__1692 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__1693 = call__1692.Field[call__1691, call__1690];
                var call__1694 = call__1693.FindOwnedChild(2228490);
                Movement = (Aow.Units.FleeLoc.TMovementHS)call__1694;
                if ((Movement != null) && Movement.FEnabled)
                {
                    var call__1695 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                    var call__1696 = FUnit.GetSide();
                    if (call__1695 == call__1696)
                    {
                        MoveResult.Code = 0;
                        MoveResult.Done = false;
                        return;
                    }
                }

                var call__1697 = GetXhx();
                if ((call__1697 != ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX) && (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX != -1))
                {
                    return;
                }

                var call__1698 = GetYhx();
                if ((call__1698 != ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY) && (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY != -1))
                {
                    return;
                }

                bool bool__1699 = !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiMove) && !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmMoveRanged);
                if (bool__1699)
                {
                    var call__1700 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetCount();
                    bool__1699 = (call__1700 >= 2);
                }

                if (bool__1699)
                {
                    return;
                }

                bool bool__1701 = (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility <= -1);
                if (bool__1701)
                {
                    var call__1702 = Aow.Units.AoWE.AoWE.MeleeDamageTypes(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit);
                    bool bool__1703 = (call__1702 == TSet<Aow.Units.AoWE.TDamageType>.Of());
                    if (!bool__1703)
                    {
                        var call__1704 = FUnit.GetAbilityEnabled(1);
                        bool bool__1705 = call__1704;
                        if (bool__1705)
                        {
                            var call__1706 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1);
                            bool__1705 = !call__1706;
                        }

                        bool__1703 = bool__1705;
                    }

                    bool__1701 = bool__1703;
                }

                if ((bool__1701 && !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmMoveRanged)) && !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiMove))
                {
                    return;
                }

                if (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility == -1)
                {
                    if (!((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiMove) && !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmMoveRanged))
                    {
                        var call__1707 = FUnit.GetSide();
                        var call__1708 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                        if (call__1707 == call__1708)
                        {
                            return;
                        }
                    }

                    MoveResult.Code = 0;
                    return;
                }

                var call__1709 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility);
                call__1709.GetCombatInfo(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.FUnit, ref AbilityInfo.Data);
                bool bool__1710 = AbilityInfo.Data.TargetTypes.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend);
                if (bool__1710)
                {
                    var call__1711 = FUnit.GetSide();
                    var call__1712 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                    bool__1710 = (call__1711 == call__1712);
                }

                if (bool__1710)
                {
                    goto Touch;
                }

                var call__1713 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility);
                call__1713.GetCombatInfo(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.FUnit, ref EnemyInfo.Data);
                if (!EnemyInfo.Data.TargetTypes.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy))
                {
                    return;
                }

                var call__1714 = FUnit.GetSide();
                var call__1715 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                if (call__1714 == call__1715)
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
                var call__1716 = GetYhx();
                var call__1717 = GetXhx();
                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__1717, call__1716, Info.HN, ref X, ref Y);
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

                var call__1718 = GetXhx();
                bool bool__1719 = (call__1718 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX);
                if (bool__1719)
                {
                    var call__1720 = GetYhx();
                    bool__1719 = (call__1720 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY);
                }

                if (bool__1719)
                {
                    return;
                }

                var call__1721 = FUnit.GetSide();
                var call__1722 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                if (call__1721 == call__1722)
                {
                    return;
                }

                var call__1723 = Aow.Units.AoWE.AoWE.MeleeDamageTypes(FUnit);
                if (call__1723 == TSet<Aow.Units.AoWE.TDamageType>.Of())
                {
                    return;
                }

                var call__1724 = FUnit.GetLocked();
                if (call__1724)
                {
                    return;
                }

                var call__1725 = FUnit.GetAbilityEnabled(1);
                var call__1726 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1);
                if (call__1725 != call__1726)
                {
                    return;
                }

                bool bool__1727 = (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX >= 0) && (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY >= 0);
                if (bool__1727)
                {
                    var call__1728 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX, ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY);
                    bool__1727 = (call__1728 <= 1);
                }

                if (bool__1727)
                {
                    var call__1729 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__1730 = call__1729.Field[((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX, ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY];
                    var call__1731 = call__1730.FindOwnedChild(2228484);
                    if (call__1731 != null)
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
            var call__1732 = FUnit.FUnit.GetUnitGFXResourceIndex();
            SetUnitGFXResourceIndex(call__1732);
            var call__1733 = new Aow.Rtl.TList();
            FAbilities = call__1733;
            CreateTCAbList();
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.NewFrameEvents.Add(NewFrame);
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled)
            {
                var call__1734 = FUnit.GetPlayer();
                var call__1735 = FUnit.GetVisibilityRange();
                var call__1736 = GetYhx();
                var call__1737 = GetXhx();
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.ExploreArea(call__1737, call__1736, 0, unchecked((sbyte)call__1734), call__1735);
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
                int end__1738 = FAbilities.Count - 1;
                for (; I <= end__1738; I += 1)
                {
                    var call__1739 = FAbilities[I];
                    ((Aow.Rtl.TObject)call__1739).Free();
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
            var call__1740 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.GetTCUnitHS();
            if (call__1740 != this)
            {
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.SetTCUnitHS(this);
            }

            FUnitGFXResource.FSFXLibraries[0].Play(0);
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer == FUnit.FCombatPlayer)
            {
                var call__1741 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer));
                call__1741.FLastUnit = FUnit;
            }
        }

        // AoWTC.pas:3796
        public override void Unselect()
        {
            base.Unselect();
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
            {
                var call__1742 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.GetTCUnitHS();
                if (call__1742 == this)
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
                var call__1743 = GetMoveSteps();
                Progress = (float)(((double)FMoveStep) / call__1743);
                X += Aow.Rtl.Rtl.Round(((double)DX) * Progress);
                Y += Aow.Rtl.Rtl.Round(((double)DY) * Progress);
            }

            var call__1744 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 23];
            Sequence = call__1744;
            while (Sequence != null)
            {
                var call__1745 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                Index = call__1745;
                if (Index != -1)
                {
                    var call__1746 = Sequence.ImageLibrary[Index];
                    call__1746.Show(X + Sequence.dXshift, Y + Sequence.dYshift, SurfaceDesc);
                }

                Sequence = Sequence.Next;
            }
        }

        // AoWTC.pas:3357
        public override int GetMoveSteps()
        {
            int Result = default;
            var call__1747 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetMovementSpeed();
            switch (call__1747)
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
                var call__1748 = Aow.Rtl.Rtl.Random(5);
                FMoveIndicator = unchecked((short)(call__1748 + 1));
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
                    var call__1749 = GetMoveSteps();
                    if (FActionFrame <= call__1749)
                    {
                        Aow.Units.HSEngine.HSEngine.HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
                        var call__1750 = GetMoveSteps();
                        FOffsetX = (FActionFrame * FOffsetX) / (call__1750 * 2);
                        var call__1751 = GetMoveSteps();
                        FOffsetY = (FActionFrame * FOffsetY) / (call__1751 * 2);
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
                    var call__1752 = GetMoveSteps();
                    if (FActionFrame <= call__1752)
                    {
                        Aow.Units.HSEngine.HSEngine.HNtoHP(FDirection, ref FOffsetX, ref FOffsetY);
                        var call__1753 = GetMoveSteps();
                        var call__1754 = GetMoveSteps();
                        FOffsetX = ((call__1753 - FActionFrame) * FOffsetX) / (call__1754 * 2);
                        var call__1755 = GetMoveSteps();
                        var call__1756 = GetMoveSteps();
                        FOffsetY = ((call__1755 - FActionFrame) * FOffsetY) / (call__1756 * 2);
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
                    var call__1757 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                    Sequence = call__1757;
                    if (Sequence != null)
                    {
                        FActionFrame = (short)(((int)FActionFrame) + 1);
                        var call__1758 = Sequence.GetTotalSequenceFrames();
                        if (FActionFrame >= (call__1758 - 1))
                        {
                            var call__1759 = Sequence.GetTotalSequenceFrames();
                            FActionFrame = unchecked((short)(call__1759 - 1));
                            FActionDone = true;
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caBreathAttack:
                    var call__1760 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 11];
                    Sequence = call__1760;
                    if (Sequence != null)
                    {
                        var call__1761 = Sequence.GetTotalSequenceFrames();
                        if (FActionFrame < (call__1761 - 1))
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
                    var call__1762 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 17];
                    Sequence = call__1762;
                    if (Sequence != null)
                    {
                        FActionFrame = (short)(((int)FActionFrame) + 1);
                        var call__1763 = Sequence.GetTotalSequenceFrames();
                        if (FActionFrame >= (call__1763 - 1))
                        {
                            var call__1764 = Sequence.GetTotalSequenceFrames();
                            FActionFrame = unchecked((short)(call__1764 - 1));
                            FActionDone = true;
                        }
                    }

                    break;
                case Aow.Units.AoWTC.TTCAction.caRoundAttack:
                case Aow.Units.AoWTC.TTCAction.caRoundAttackHit:
                    var call__1765 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[FDirection + 16];
                    Sequence = call__1765;
                    if (Sequence != null)
                    {
                        FActionFrame = (short)(((int)FActionFrame) + 1);
                        var call__1766 = Sequence.GetTotalSequenceFrames();
                        if (FActionFrame >= (call__1766 - 1))
                        {
                            var call__1767 = Sequence.GetTotalSequenceFrames();
                            FActionFrame = unchecked((short)(call__1767 - 1));
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
                        var call__1768 = FUnitGFXResource.FSequences.ImageSequences[FDirection + 5];
                        Sequence = call__1768;
                        if (((Sequence != null) && (Sequence.SpeedCount == Sequence.SequenceSpeed)) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                        {
                            var call__1769 = Sequence.GetFrameIndex(Aow.Units.AoWE.AoWE.AoWHSMap.Frame, ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 2)) & 65535);
                            FUnitGFXResource.FSFXLibraries[1].Play(call__1769 % 100);
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
                                    var call__1770 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSelectedSpell);
                                    call__1770.FCastSounds.Play(0);
                                }
                            }
                            else
                            {
                                var call__1771 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FSelectedAbility);
                                call__1771.FSFXLibrary.Play(0);
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
            var call__1772 = FUnit.GetPlayer();
            var call__1773 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(call__1772);
            if (call__1773 && (Path.Count > 1))
            {
                var call__1774 = Aow.Units.CombatTE.CombatTE.CreateTCMeleeMoveTE(FUnit, Path);
                TE = call__1774;
                if (TE != null)
                {
                    var call__1775 = FUnit.GetPlayer();
                    var call__1776 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(call__1775);
                    call__1776.AddEvent(TE);
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
                    int end__1777 = FAbilities.Count - 1;
                    for (; I <= end__1777; I += 1)
                    {
                        var call__1778 = FAbilities[I];
                        if (((Aow.Units.AoWTC.TTCAbility)call__1778).Ability == A1)
                        {
                            Index = I;
                        }
                    }
                }

                var call__1779 = FUnit.GetLocked();
                if ((((!call__1779 && (FUnit.FMovePoints > 0)) && (FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)) && (Index < FCombatAbilityCount)) && (Index > -1))
                {
                    if (A1 == 52)
                    {
                        var call__1780 = FUnit.FUnit.GetMoves();
                        if (call__1780 == FUnit.FMovePoints)
                        {
                            var call__1781 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(A1);
                            call__1781.ActivateCombat(FUnit.FUnit, ref Reason);
                            FSelectedSpell = -1;
                            FSelectedAbility = A1;
                            Result = Index;
                        }
                    }
                    else
                    {
                        var call__1782 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(A1);
                        var call__1783 = call__1782.GetCombatMode(FUnit.FUnit);
                        if (TSet<Aow.Units.AoWE.TCombatMode>.Of(Aow.Units.AoWE.TCombatMode.cmShortRange, Aow.Units.AoWE.TCombatMode.cmMediumRange, Aow.Units.AoWE.TCombatMode.cmLongRange).Contains(call__1783))
                        {
                            var call__1784 = FUnit.FUnit.GetMoves();
                            if (call__1784 == FUnit.FMovePoints)
                            {
                                var call__1785 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(A1);
                                var call__1786 = call__1785.GetCombatMode(FUnit.FUnit);
                                if (call__1786 == Aow.Units.AoWE.TCombatMode.cmShortRange)
                                {
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FRangedPathHS.FRange = 4;
                                }
                                else
                                {
                                    var call__1787 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(A1);
                                    var call__1788 = call__1787.GetCombatMode(FUnit.FUnit);
                                    if (call__1788 == Aow.Units.AoWE.TCombatMode.cmMediumRange)
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
                int end__1789 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetCount() - 1;
                for (; I <= end__1789; I += 1)
                {
                    {
                        var w__1790 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetMoveSettings(I);
                        w__1790.FPath.Clear();
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
            var call__1791 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSelectedSpell);
            call__1791.GetCombatInfo(FUnit.FUnit, ref Info);
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
                int end__1792 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetCount() - 1;
                for (; I <= end__1792; I += 1)
                {
                    {
                        var w__1793 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetMoveSettings(I);
                        w__1793.FPath.Clear();
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
                int end__1794 = FAbilities.Count - 1;
                for (; I <= end__1794; I += 1)
                {
                    var call__1795 = FAbilities[I];
                    ((Aow.Rtl.TObject)call__1795).Free();
                }
            }

            FAbilities.Clear();
            FCombatAbilityCount = 0;
            {
                I = 1;
                byte end__1796 = 169;
                for (; I <= end__1796; I += 1)
                {
                    var call__1797 = FUnit.GetAbilityEnabled(I);
                    if (call__1797)
                    {
                        Ability = Aow.Units.AoWTC.TTCAbility.Create();
                        Ability.Ability = I;
                        var call__1798 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                        var call__1799 = call__1798.GetControlType(FUnit.FUnit);
                        bool bool__1800 = call__1799.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acCombatAction);
                        if (bool__1800)
                        {
                            var call__1801 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                            var call__1802 = call__1801.GetCombatMode(FUnit.FUnit);
                            bool__1800 = (call__1802 != Aow.Units.AoWE.TCombatMode.cmMelee);
                        }

                        if (bool__1800)
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
                int end__1803 = FAbilities.Count - 1;
                for (; I <= end__1803; I += 1)
                {
                    var call__1804 = FAbilities[I];
                    var call__1805 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TTCAbility)call__1804).Ability);
                    var call__1806 = call__1805.GetName(FUnit.FUnit);
                    var call__1807 = FAbilities[I];
                    Names.AddObject(call__1806, Aow.Rtl.Rtl.IntToObject((int)((Aow.Units.AoWTC.TTCAbility)call__1807).Ability));
                    if (Types != null)
                    {
                        var call__1808 = FAbilities[I];
                        var call__1809 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TTCAbility)call__1808).Ability);
                        var call__1810 = call__1809.GetAbilityType();
                        var call__1811 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TAbilityTypeRStr[(int)call__1810]);
                        Types.Add(call__1811);
                    }

                    if (Sources != null)
                    {
                        var call__1812 = FAbilities[I];
                        var call__1813 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TTCAbility)call__1812).Ability);
                        var call__1814 = call__1813.GetSourceName(FUnit.FUnit);
                        Sources.Add(call__1814);
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
            var call__1815 = FUnit.GetPlayer();
            var call__1816 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(call__1815);
            if (call__1816 && (Path.Count > 0))
            {
                if (Path.Count > 1)
                {
                    var call__1817 = Aow.Units.CombatTE.CombatTE.CreateTCAbTouchMoveTE(FUnit, Path, FSelectedAbility);
                    TE = call__1817;
                    if (TE != null)
                    {
                        var call__1818 = FUnit.GetPlayer();
                        var call__1819 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(call__1818);
                        call__1819.AddEvent(TE);
                        TE.Release();
                        Result = true;
                    }
                }
                else
                {
                    var call__1820 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FSelectedAbility);
                    call__1820.GetCombatInfo(FUnit.FUnit, ref Buffer.Info);
                    if (Buffer.Info.TargetTypes.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend))
                    {
                        var call__1821 = Path[0];
                        var call__1822 = Path[0];
                        AbRangedTC(unchecked((ushort)call__1822.X), unchecked((ushort)call__1821.Y));
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
            var call__1823 = FUnit.GetPlayer();
            var call__1824 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(call__1823);
            if (call__1824)
            {
                var call__1825 = Aow.Units.CombatTE.CombatTE.CreateTCAbRangedTE(FUnit, unchecked((ushort)X), unchecked((ushort)Y), FSelectedAbility, FSelectedSpell);
                TE = call__1825;
                if (TE != null)
                {
                    var call__1826 = FUnit.GetPlayer();
                    var call__1827 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(call__1826);
                    call__1827.AddEvent(TE);
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
                int end__1828 = FObstacles.Count - 1;
                for (; I <= end__1828; I += 1)
                {
                    var call__1829 = FObstacles[I];
                    ((Aow.Rtl.TObject)call__1829).Free();
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
                var call__1830 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer));
                if (!call__1830.FAuto)
                {
                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Button == Aow.Rtl.TMouseButton.mbRight)
                    {
                        var call__1831 = GetTCUnitHS();
                        bool bool__1832 = (call__1831 != null);
                        if (bool__1832)
                        {
                            var call__1833 = GetTCUnitHS();
                            bool__1832 = (call__1833.FSelectedAbility > -1);
                        }

                        if (bool__1832)
                        {
                            var call__1834 = GetTCUnitHS();
                            call__1834.SelectAbility(-1);
                            return;
                        }

                        Unselect();
                    }
                    else
                    {
                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Shift.Contains(Aow.Rtl.TShiftState.ssShift))
                        {
                            var call__1835 = FMoveSettingsList.GetCount();
                            bool bool__1836 = (call__1835 == 1);
                            if (bool__1836)
                            {
                                var call__1837 = FMoveSettingsList.GetMoveSettings(0);
                                bool__1836 = (call__1837.FPath.Count != 0);
                            }

                            if (bool__1836)
                            {
                                var call__1838 = FMoveSettingsList.GetMoveSettings(0);
                                var call__1839 = call__1838.FPath[0];
                                var call__1840 = FMoveSettingsList.GetMoveSettings(0);
                                var call__1841 = call__1840.FPath[0];
                                var call__1842 = FMoveSettingsList.GetMoveSettings(0);
                                var call__1843 = call__1842.FPath[0];
                                var call__1844 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__1843.X, call__1841.Y, call__1839.L);
                                var call__1845 = ValidMultiMoveSelection((Aow.Units.AoWE.TAoWMapField)call__1844);
                                if (!call__1845)
                                {
                                    return;
                                }
                            }

                            var call__1846 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                            if (call__1846 != -1)
                            {
                                var call__1847 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                var call__1848 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                var call__1849 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__1848, call__1847, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                var call__1850 = call__1849.FindOwnedChild(2228484);
                                UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1850;
                                if ((UnitHS != null) && (UnitHS.FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer))
                                {
                                    var call__1851 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                    Index = call__1851;
                                    if (Index == -1)
                                    {
                                        AddUnit(UnitHS);
                                        Update();
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
                                        var call__1852 = UnitHS.FUnit.GetLocked();
                                        if (call__1852)
                                        {
                                            var call__1853 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                            var call__1854 = FMoveSettingsList.GetMoveSettings(call__1853);
                                            call__1854.FPath.Clear();
                                        }
                                        else
                                        {
                                            var call__1855 = FMoveSettingsList.GetMoveSettings(0);
                                            if (call__1855.FPath.Count > 0)
                                            {
                                                var call__1856 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__1857 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__1858 = call__1857.FPath[call__1856.FPath.Count - 1];
                                                var call__1859 = UnitHS.GetXhx();
                                                DX = call__1859 - call__1858.X;
                                                var call__1860 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__1861 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__1862 = call__1861.FPath[call__1860.FPath.Count - 1];
                                                var call__1863 = UnitHS.GetYhx();
                                                DY = call__1863 - call__1862.Y;
                                                var call__1864 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__1865 = call__1864.FPath[0];
                                                var call__1866 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__1867 = call__1866.FPath[0];
                                                var call__1868 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                                CalculateMovePath(call__1868, unchecked((sbyte)(call__1867.X + DX)), unchecked((sbyte)(call__1865.Y + DY)), 0);
                                            }
                                        }

                                        SetCurrentMove(-1);
                                    }
                                    else
                                    {
                                        UnitHS.Unselect();
                                        DeleteMoveObject(Index);
                                        var call__1869 = FMoveSettingsList.GetCount();
                                        SetCurrentMove(call__1869 - 1);
                                        Update();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Shift.Contains(Aow.Rtl.TShiftState.ssCtrl))
                            {
                                var call__1870 = FMoveSettingsList.GetCount();
                                bool bool__1871 = (call__1870 > 0);
                                if (bool__1871)
                                {
                                    var call__1872 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                    bool__1871 = (call__1872 != -1);
                                }

                                if (bool__1871)
                                {
                                    var call__1873 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                    var call__1874 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                    var call__1875 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__1874, call__1873, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                    var call__1876 = call__1875.FindOwnedChild(2228484);
                                    UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1876;
                                    if ((UnitHS != null) && (UnitHS.FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer))
                                    {
                                        var call__1877 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                        Index = call__1877;
                                        if (Index == -1)
                                        {
                                            AddUnit(UnitHS);
                                            Update();
                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
                                            var call__1878 = UnitHS.FUnit.GetLocked();
                                            if (call__1878)
                                            {
                                                var call__1879 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                                var call__1880 = FMoveSettingsList.GetMoveSettings(call__1879);
                                                call__1880.FPath.Clear();
                                            }
                                            else
                                            {
                                                var call__1881 = FMoveSettingsList.GetMoveSettings(0);
                                                if (call__1881.FPath.Count > 0)
                                                {
                                                    var call__1882 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__1883 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__1884 = call__1883.FPath[call__1882.FPath.Count - 1];
                                                    var call__1885 = UnitHS.GetXhx();
                                                    DX = call__1885 - call__1884.X;
                                                    var call__1886 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__1887 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__1888 = call__1887.FPath[call__1886.FPath.Count - 1];
                                                    var call__1889 = UnitHS.GetYhx();
                                                    DY = call__1889 - call__1888.Y;
                                                    var call__1890 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__1891 = call__1890.FPath[0];
                                                    var call__1892 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__1893 = call__1892.FPath[0];
                                                    var call__1894 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                                    CalculateMovePath(call__1894, unchecked((sbyte)(call__1893.X + DX)), unchecked((sbyte)(call__1891.Y + DY)), 0);
                                                }
                                            }

                                            var call__1895 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                            SetCurrentMove(call__1895);
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
                                var call__1896 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                if (call__1896 != -1)
                                {
                                    if (GetTCUnitHS() != null)
                                    {
                                        var call__1897 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                        var call__1898 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                        var call__1899 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__1898, call__1897, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                        var call__1900 = call__1899.FindOwnedChild(2228484);
                                        UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1900;
                                        var call__1901 = GetTCUnitHS();
                                        if (call__1901.FSelectedAbility == -1)
                                        {
                                            Targets = TSet<Aow.Units.AoWE.TCombatTargetTypeElement>.Of(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy);
                                        }
                                        else
                                        {
                                            var call__1902 = GetTCUnitHS();
                                            if (call__1902.FSelectedAbility == 52)
                                            {
                                                var call__1903 = GetTCUnitHS();
                                                var call__1904 = GetTCUnitHS();
                                                var call__1905 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(call__1904.FSelectedSpell);
                                                call__1905.GetCombatInfo(call__1903.FUnit.FUnit, ref SpellInfo);
                                                Targets = SpellInfo.TargetTypes;
                                            }
                                            else
                                            {
                                                var call__1906 = GetTCUnitHS();
                                                var call__1907 = GetTCUnitHS();
                                                var call__1908 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(call__1907.FSelectedAbility);
                                                call__1908.GetCombatInfo(call__1906.FUnit.FUnit, ref AbilityInfo);
                                                Targets = AbilityInfo.TargetTypes;
                                            }
                                        }

                                        if (UnitHS == null)
                                        {
                                            ExecuteMoveSelection();
                                        }
                                        else
                                        {
                                            var call__1909 = GetTCUnitHS();
                                            var call__1910 = call__1909.FUnit.GetPlayer();
                                            var call__1911 = UnitHS.FUnit.GetPlayer();
                                            if ((call__1911 == call__1910) && !Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend))
                                            {
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.UnselectLocation();
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
                                            }
                                            else
                                            {
                                                var call__1912 = GetTCUnitHS();
                                                var call__1913 = call__1912.FUnit.GetPlayer();
                                                var call__1914 = UnitHS.FUnit.GetPlayer();
                                                bool bool__1915 = ((call__1914 == call__1913) && Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend));
                                                if (!bool__1915)
                                                {
                                                    var call__1916 = GetTCUnitHS();
                                                    var call__1917 = call__1916.FUnit.GetPlayer();
                                                    var call__1918 = UnitHS.FUnit.GetPlayer();
                                                    bool__1915 = ((call__1918 != call__1917) && Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy));
                                                }

                                                if (bool__1915)
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
                var call__1919 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                var call__1920 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                var call__1921 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__1920, call__1919, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                var call__1922 = call__1921.FindOwnedChild(2228484);
                UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1922;
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
                var w__1923 = FMoveSettingsList.GetMoveSettings(Index);
                Temporary = new Aow.Rtl.TList();
                var call__1924 = FMoveSettingsList.GetCount();
                if (call__1924 > 0)
                {
                    {
                        I = 0;
                        int end__1925 = FMoveSettingsList.GetCount() - 1;
                        for (; I <= end__1925; I += 1)
                        {
                            if (I != Index)
                            {
                                var call__1926 = FMoveSettingsList.GetMoveSettings(I);
                                if (call__1926.FPath.Count > 1)
                                {
                                    Movement = Aow.Units.FleeLoc.TMovementHS.Create(null);
                                    var call__1927 = FMoveSettingsList.GetMoveSettings(I);
                                    var call__1928 = FMoveSettingsList.GetMoveSettings(I);
                                    var call__1929 = call__1928.FPath[call__1927.FPath.Count - 1];
                                    var call__1930 = FMoveSettingsList.GetMoveSettings(I);
                                    var call__1931 = FMoveSettingsList.GetMoveSettings(I);
                                    var call__1932 = call__1931.FPath[call__1930.FPath.Count - 1];
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(Movement, call__1932.X, call__1929.Y, 0);
                                    Movement.FEnabled = true;
                                    Temporary.Add(Movement);
                                    Movement = Aow.Units.FleeLoc.TMovementHS.Create(null);
                                    var call__1933 = FMoveSettingsList.GetMoveSettings(I);
                                    var call__1934 = call__1933.FPath[0];
                                    var call__1935 = FMoveSettingsList.GetMoveSettings(I);
                                    var call__1936 = call__1935.FPath[0];
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(Movement, call__1936.X, call__1934.Y, 0);
                                    Movement.FEnabled = false;
                                    Temporary.Add(Movement);
                                }
                            }
                        }
                    }
                }

                w__1923.FPathMovePoints = 0;
                UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)w__1923.FMoveObject;
                var call__1937 = GetTCUnitHS();
                bool bool__1938 = (call__1937 != null);
                if (bool__1938)
                {
                    var call__1939 = GetTCUnitHS();
                    bool bool__1940 = (call__1939.FSelectedAbility == 117);
                    if (!bool__1940)
                    {
                        var call__1941 = GetTCUnitHS();
                        bool__1940 = (call__1941.FSelectedAbility == 65);
                    }

                    bool__1938 = bool__1940;
                }

                if (bool__1938)
                {
                    ((Aow.Units.AoWTC.TCombatMoveControlSettings)FMoveControlSettings).FMoveFlags = TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.FromMask((ulong)((byte)FPathInvalid.Image)) + TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmClimbing);
                }
                else
                {
                    ((Aow.Units.AoWTC.TCombatMoveControlSettings)FMoveControlSettings).FMoveFlags = TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.FromMask((ulong)((byte)FPathInvalid.Image));
                }

                FMoveControlSettings.Reset();
                var call__1942 = UnitHS.GetLhx();
                var call__1943 = UnitHS.GetYhx();
                var call__1944 = UnitHS.GetXhx();
                FMoveControlSettings.SetSourcePos(call__1944, call__1943, call__1942);
                var call__1945 = UnitHS.FUnit.GetPlayer();
                FMoveControlSettings.FPlayer = unchecked((sbyte)call__1945);
                FMoveControlSettings.FArmyHS = UnitHS;
                FMoveControlSettings.FSelection = 0;
                FMoveControlSettings.SetDestinationPos(X, Y, L);
                UnitHS.FUnit.FUnit.CreateMovePointTable(ref FMoveControlSettings.FMovePointTable);
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.ProcessMove(FMoveControlSettings);
                var call__1946 = FMoveSettingsList.GetMoveSettings(Index);
                call__1946.FPath.Assign(FMoveControlSettings.FPath);
                var call__1947 = FMoveSettingsList.GetMoveSettings(Index);
                call__1947.FMovePoints = UnitHS.FUnit.FMovePoints;
                var call__1948 = UnitHS.FUnit.GetMoves();
                var call__1949 = FMoveSettingsList.GetMoveSettings(Index);
                call__1949.FMoves = call__1948;
                if (FMoveControlSettings.FPath.Count != 0)
                {
                    {
                        I = 0;
                        int end__1950 = FMoveControlSettings.FPath.Count - 2;
                        for (; I <= end__1950; I += 1)
                        {
                            var call__1951 = FMoveControlSettings.FPath[I];
                            var call__1952 = FMoveControlSettings.FPath[I];
                            var call__1953 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1954 = call__1953.Field[call__1952.X, call__1951.Y];
                            var call__1955 = call__1954.FindOwnedChild(2229508);
                            bool bool__1956 = (call__1955 != null);
                            if (!bool__1956)
                            {
                                var call__1957 = FMoveControlSettings.FPath[I];
                                var call__1958 = FMoveControlSettings.FPath[I];
                                var call__1959 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__1960 = call__1959.Field[call__1958.X, call__1957.Y];
                                var call__1961 = call__1960.FindOwnedChild(2229510);
                                bool__1956 = (call__1961 != null);
                            }

                            if (bool__1956)
                            {
                                var call__1962 = FMoveControlSettings.FPath[I];
                                var call__1963 = FMoveControlSettings.FPath[I];
                                var call__1964 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__1965 = call__1964.Field[call__1963.X, call__1962.Y];
                                w__1923.FPath.List[I].Flag = unchecked((sbyte)FMoveControlSettings.FMovePointTable[call__1965.TerrainType][-1 - -1]);
                            }
                            else
                            {
                                var call__1966 = FMoveControlSettings.FPath[I];
                                var call__1967 = FMoveControlSettings.FPath[I];
                                var call__1968 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__1969 = call__1968.Field[call__1967.X, call__1966.Y];
                                var call__1970 = FMoveControlSettings.FPath[I];
                                var call__1971 = FMoveControlSettings.FPath[I];
                                var call__1972 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__1973 = call__1972.Field[call__1971.X, call__1970.Y];
                                w__1923.FPath.List[I].Flag = unchecked((sbyte)FMoveControlSettings.FMovePointTable[call__1973.TerrainType][call__1969.OverlayType - -1]);
                            }
                        }
                    }

                    var call__1974 = w__1923.FPath.PosToMovePoints(0);
                    w__1923.FPathMovePoints = call__1974;
                    FMoveControlSettings.FPath.Clear();
                }

                {
                    I = 0;
                    int end__1975 = Temporary.Count - 1;
                    for (; I <= end__1975; I += 1)
                    {
                        var call__1976 = Temporary[I];
                        ((Aow.Rtl.TObject)call__1976).Free();
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
            var call__1977 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
            if (call__1977 == -1)
            {
                return;
            }

            var call__1978 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
            if (call__1978 == -1)
            {
                return;
            }

            if (UnitHS == null)
            {
                return;
            }

            var call__1979 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
            var call__1980 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
            var call__1981 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__1980, call__1979, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
            Field = (Aow.Units.AoWE.TAoWMapField)call__1981;
            bool bool__1982 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
            if (bool__1982)
            {
                var call__1983 = Field.GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                bool__1982 = !call__1983;
            }

            if (bool__1982)
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
                    var call__1984 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(UnitHS.FSelectedSpell);
                    call__1984.GetCombatInfo(UnitHS.FUnit.FUnit, ref SpellInfo);
                    Targets = SpellInfo.TargetTypes;
                }
                else
                {
                    var call__1985 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                    call__1985.GetCombatInfo(UnitHS.FUnit.FUnit, ref AbilityInfo.Data);
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
                    var call__1986 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                    var call__1987 = call__1986.GetCombatMode(UnitHS.FUnit.FUnit);
                    if (call__1987 == Aow.Units.AoWE.TCombatMode.cmTouch)
                    {
                        bool bool__1988 = Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend);
                        if (bool__1988)
                        {
                            var call__1989 = UnitHS.FUnit.GetSide();
                            var call__1990 = TargetHS.FUnit.GetSide();
                            bool__1988 = (call__1989 == call__1990);
                        }

                        bool bool__1991 = bool__1988;
                        if (!bool__1991)
                        {
                            bool bool__1992 = Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy);
                            if (bool__1992)
                            {
                                var call__1993 = UnitHS.FUnit.GetSide();
                                var call__1994 = TargetHS.FUnit.GetSide();
                                bool__1992 = (call__1993 != call__1994);
                            }

                            bool__1991 = bool__1992;
                        }

                        bool bool__1995 = bool__1991;
                        if (bool__1995)
                        {
                            var call__1996 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                            var call__1997 = ((Aow.Units.AoWE.TTouchAbility)call__1996).CanTouch(UnitHS.FUnit, TargetHS.FUnit);
                            bool__1995 = call__1997;
                        }

                        if (bool__1995)
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
                        bool bool__1998 = Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend);
                        if (bool__1998)
                        {
                            var call__1999 = UnitHS.FUnit.GetSide();
                            var call__2000 = TargetHS.FUnit.GetSide();
                            bool__1998 = (call__1999 == call__2000);
                        }

                        bool bool__2001 = bool__1998;
                        if (!bool__2001)
                        {
                            bool bool__2002 = Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy);
                            if (bool__2002)
                            {
                                var call__2003 = UnitHS.FUnit.GetSide();
                                var call__2004 = TargetHS.FUnit.GetSide();
                                bool__2002 = (call__2003 != call__2004);
                            }

                            bool__2001 = bool__2002;
                        }

                        if (bool__2001)
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
                    var call__2005 = TargetHS.FUnit.GetPlayer();
                    var call__2006 = UnitHS.FUnit.GetPlayer();
                    if (call__2005 == call__2006)
                    {
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.CursorMode = 257;
                    }
                    else
                    {
                        var call__2007 = UnitHS.FUnit.GetAbilityEnabled(1);
                        bool bool__2008 = call__2007;
                        if (!bool__2008)
                        {
                            var call__2009 = TargetHS.FUnit.GetAbilityEnabled(1);
                            bool__2008 = !call__2009;
                        }

                        if (bool__2008)
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

            var call__2010 = UnitHS.FUnit.GetPlayer();
            if (call__2010 == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer)
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
                        bool bool__2011 = !(Wall is Aow.Units.CityWall.TCityWallDoor);
                        if (!bool__2011)
                        {
                            var call__2012 = UnitHS.FUnit.GetSide();
                            bool__2011 = (call__2012 == Aow.Units.AoWE.TCombatSideIndex.csAttacker);
                        }

                        if (bool__2011)
                        {
                            Cost = -1;
                        }

                        bool bool__2013 = !(Wall is Aow.Units.CityWall.TCityWallDoor);
                        if (bool__2013)
                        {
                            var call__2014 = UnitHS.FUnit.FUnit.GetAbilityEnabled(36);
                            bool__2013 = call__2014;
                        }

                        if (bool__2013)
                        {
                            Cost = 1;
                        }

                        var call__2015 = UnitHS.FUnit.FUnit.GetAbilityEnabled(66);
                        bool bool__2016 = call__2015;
                        if (!bool__2016)
                        {
                            var call__2017 = UnitHS.FUnit.FUnit.GetAbilityEnabled(1);
                            bool__2016 = call__2017;
                        }

                        if (bool__2016)
                        {
                            Cost = 1;
                        }
                    }
                }

                bool bool__2018 = (Cost <= 0);
                if (!bool__2018)
                {
                    bool bool__2019 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
                    if (bool__2019)
                    {
                        var call__2020 = Field.GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                        bool__2019 = !call__2020;
                    }

                    bool__2018 = bool__2019;
                }

                if (bool__2018)
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
            bool bool__2021 = (FCurrentMove != -1);
            if (bool__2021)
            {
                var call__2022 = GetTCUnitHS();
                bool__2021 = (call__2022 != null);
            }

            if (bool__2021)
            {
                var call__2023 = GetTCUnitHS();
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.TriggerDeselectEvent(call__2023);
            }

            base.SetCurrentMove(A1);
            bool bool__2024 = (FCurrentMove != -1);
            if (bool__2024)
            {
                var call__2025 = GetTCUnitHS();
                bool__2024 = (call__2025 != null);
            }

            if (bool__2024)
            {
                var call__2026 = GetTCUnitHS();
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.TriggerSelectEvent(call__2026);
            }
        }

        // AoWTC.pas:3316
        public override void Unselect()
        {
            DeactivateRangedPath();
            if (GetTCUnitHS() != null)
            {
                var call__2027 = GetTCUnitHS();
                call__2027.FSelectedAbility = -1;
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
                var call__2028 = GetTCUnitHS();
                bool bool__2029 = (call__2028.FSelectedAbility != 65);
                if (bool__2029)
                {
                    var call__2030 = GetTCUnitHS();
                    bool__2029 = (call__2030.FSelectedAbility != 114);
                }

                if (bool__2029)
                {
                    return;
                }

                var call__2031 = FMoveSettingsList.GetCount();
                bool bool__2032 = (call__2031 == 1);
                if (bool__2032)
                {
                    var call__2033 = FMoveSettingsList.GetMoveSettings(0);
                    bool__2032 = (call__2033.FPath.Count > 1);
                }

                if (bool__2032)
                {
                    {
                        I = 1;
                        byte end__2034 = 6;
                        for (; I <= end__2034; I += 1)
                        {
                            var call__2035 = FMoveSettingsList.GetMoveSettings(0);
                            var call__2036 = call__2035.FPath[1];
                            var call__2037 = FMoveSettingsList.GetMoveSettings(0);
                            var call__2038 = call__2037.FPath[1];
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__2038.X, call__2036.Y, I, ref X, ref Y);
                            Aow.Units.HSEngine.HSEngine.HXtoHP(X, Y, ref Xhp, ref Yhp);
                            var call__2039 = FTargetImages[0];
                            call__2039.Show((Xhp - Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewXhp) - 8, (Yhp - Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewYhp) + 2, Surface);
                        }
                    }
                }
            }
        }

        // AoWTC.pas:2723
        public void Create__init_TCombatUnitSelectionControl()
        {
            base.Create__init_TMoveSelector();
            var call__2040 = Aow.Units.aowFX.TXYZList.Create();
            FRangedPath = call__2040;
            var call__2041 = new Aow.Rtl.TList();
            FObstacles = call__2041;
            var call__2042 = Aow.Units.AoWTC.TRangedPathHS.Create(null);
            FRangedPathHS = call__2042;
            FRangedPathHS.Activate();
            var call__2043 = Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TargHex.ILB");
            FTargetImages = call__2043;
        }

        // AoWTC.pas:2747
        public int AddUnit(Aow.Units.AoWTC.TTacticalCombatUnitHS U)
        {
            int Result = default;
            Result = AddMoveObject(U);
            var call__2044 = FMoveSettingsList.GetMoveSettings(Result);
            call__2044.FMovePoints = U.FUnit.FMovePoints;
            var call__2045 = U.FUnit.GetMoves();
            var call__2046 = FMoveSettingsList.GetMoveSettings(Result);
            call__2046.FMoves = call__2045;
            return Result;
        }

        // AoWTC.pas:2763
        public Aow.Units.AoWTC.TTacticalCombatUnitHS GetTCUnitHS()
        {
            Aow.Units.AoWTC.TTacticalCombatUnitHS Result = default;
            if (FCurrentMove != -1)
            {
                var call__2047 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                Result = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__2047.FMoveObject;
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
                var call__2048 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                var call__2049 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__2048.FMoveObject).FUnit.FUnit.GetMoveTypes();
                Result = call__2049;
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
            var call__2050 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL];
            Level = call__2050;
            Aow.Units.HSEngine.HSEngine.HPtoHX(R.Left, R.Top, ref X1, ref Y1);
            Aow.Units.HSEngine.HSEngine.HPtoHX(R.Right, R.Bottom, ref X2, ref Y2);
            {
                X = X1 - 1;
                int end__2051 = X2 + 1;
                for (; X <= end__2051; X += 1)
                {
                    {
                        Y = Y1 - 1;
                        int end__2052 = Y2 + 1;
                        for (; Y <= end__2052; Y += 1)
                        {
                            Aow.Units.HSEngine.HSEngine.HXtoHP(X, Y, ref PX, ref PY);
                            PX += 16;
                            PY += 8;
                            if ((((((((X >= 0) && (X < Level.Width)) && (Y > 0)) && (Y < Level.Height)) && (PX >= R.Left)) && (PX < R.Right)) && (PY >= R.Top)) && (PY < R.Bottom))
                            {
                                var call__2053 = Level.Field[X, Y];
                                Field = call__2053;
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
                    int end__2054 = Units.Count - 1;
                    for (; I <= end__2054; I += 1)
                    {
                        var call__2055 = Units[I];
                        AddUnit((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__2055);
                        var call__2056 = Units[I];
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS((Aow.Units.HSEngine.THexagonSprite)call__2056);
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
                        var call__2057 = FRangedPath.Count;
                        if (call__2057 == 0)
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
                int end__2058 = FObstacles.Count - 1;
                for (; Direction <= end__2058; Direction += 1)
                {
                    var call__2059 = FObstacles[Direction];
                    ((Aow.Rtl.TObject)call__2059).Free();
                }
            }

            FObstacles.Clear();
            UnitHS = GetTCUnitHS();
            var call__2060 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
            var call__2061 = call__2060.Field[X, Y];
            if (call__2061.TerrainType != 15)
            {
                if (UnitHS.FSelectedAbility == 52)
                {
                    var call__2062 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(UnitHS.FSelectedSpell);
                    call__2062.GetCombatInfo(UnitHS.FUnit.FUnit, ref Info);
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
                    var call__2063 = GetTCUnitHS();
                    var call__2064 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                    var call__2065 = call__2064.GetCombatMode(call__2063.FUnit.FUnit);
                    if (call__2065 == Aow.Units.AoWE.TCombatMode.cmLongRange)
                    {
                        Range = 12;
                    }
                    else
                    {
                        var call__2066 = GetTCUnitHS();
                        var call__2067 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                        var call__2068 = call__2067.GetCombatMode(call__2066.FUnit.FUnit);
                        if (call__2068 == Aow.Units.AoWE.TCombatMode.cmMediumRange)
                        {
                            Range = 8;
                        }
                        else
                        {
                            Range = 4;
                        }
                    }
                }

                var call__2069 = UnitHS.FUnit.GetMoves();
                bool bool__2070 = (call__2069 == UnitHS.FUnit.FMovePoints) && (X > 0);
                if (bool__2070)
                {
                    var call__2071 = UnitHS.GetYhx();
                    var call__2072 = UnitHS.GetXhx();
                    var call__2073 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, call__2072, call__2071);
                    bool__2070 = (call__2073 <= Range);
                }

                if (bool__2070)
                {
                    // { Original retains this unused with temporary. }
                    {
                        var w__2074 = GetTCUnitHS();
                        FTargetX = X;
                        FTargetY = Y;
                        var call__2075 = UnitHS.GetYhx();
                        var call__2076 = UnitHS.GetXhx();
                        var call__2077 = Aow.Units.HSEngine.HSEngine.dHXtoHN(call__2076, call__2075, X, Y);
                        Direction = call__2077;
                        NX = X;
                        NY = Y;
                        var call__2078 = GetTCUnitHS();
                        switch (call__2078.FSelectedAbility)
                        {
                            case 30:
                            case >= 86 and <= 90:
                                var call__2079 = UnitHS.GetYhx();
                                var call__2080 = UnitHS.GetXhx();
                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__2080, call__2079, Aow.Units.AoWTC.AoWTC.BreathHN[(Aow.Units.AoWTC.AoWTC.BreathDirection[Direction - 1] * 12) - 12], ref NX, ref NY);
                                break;
                            case 52:
                                if (Aow.Units.AoWTC.AoWTC.SpellTypes[UnitHS.FSelectedSpell] == Aow.Units.AoWTC.TfxType.fxBreath)
                                {
                                    var call__2081 = UnitHS.GetYhx();
                                    var call__2082 = UnitHS.GetXhx();
                                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__2082, call__2081, Aow.Units.AoWTC.AoWTC.BreathHN[(Aow.Units.AoWTC.AoWTC.BreathDirection[Direction - 1] * 12) - 12], ref NX, ref NY);
                                }

                                break;
                        }

                        var call__2083 = GetTCUnitHS();
                        var call__2084 = call__2083.GetXhx();
                        var call__2085 = GetTCUnitHS();
                        var call__2086 = call__2085.GetYhx();
                        var call__2087 = GetTCUnitHS();
                        var call__2088 = call__2087.FUnit.GetSide();
                        var call__2089 = GetTCUnitHS();
                        var call__2090 = GetTCUnitHS();
                        Aow.Units.AoWTC.AoWTC.MakeAbilPath(call__2088, unchecked((short)call__2090.FSelectedAbility), unchecked((short)call__2089.FSelectedSpell), unchecked((short)call__2084), unchecked((short)call__2086), unchecked((short)NX), unchecked((short)NY), ref FRangedPath, ref FObstacles);
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
            var call__2091 = FMoveSettingsList.GetCount();
            if (call__2091 > 0)
            {
                if (GetTCUnitHS() != null)
                {
                    var call__2092 = FMoveSettingsList.GetMoveSettings(0);
                    if (call__2092.FPath.Count > 0)
                    {
                        {
                            var w__2093 = FMoveSettingsList.GetMoveSettings(0);
                            UnitHS = GetTCUnitHS();
                            if (UnitHS.FSelectedAbility != 117)
                            {
                                var call__2094 = w__2093.FPath[0];
                                var call__2095 = w__2093.FPath[0];
                                var call__2096 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__2097 = call__2096.Field[call__2095.X, call__2094.Y];
                                var call__2098 = call__2097.FindOwnedChild(2228484);
                                Target = call__2098;
                            }
                            else
                            {
                                var call__2099 = w__2093.FPath[0];
                                var call__2100 = w__2093.FPath[0];
                                var call__2101 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__2102 = call__2101.Field[call__2100.X, call__2099.Y];
                                var call__2103 = call__2102.FindOwnedChild(2229508);
                                Target = call__2103;
                                if (Target == null)
                                {
                                    var call__2104 = w__2093.FPath[0];
                                    var call__2105 = w__2093.FPath[0];
                                    var call__2106 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__2107 = call__2106.Field[call__2105.X, call__2104.Y];
                                    var call__2108 = call__2107.FindOwnedChild(2229510);
                                    Target = call__2108;
                                }
                            }

                            var call__2109 = FMoveSettingsList.GetMoveSettings(0);
                            var call__2110 = call__2109.FPath.MovePointsToPos(UnitHS.FUnit.FMovePoints);
                            if ((call__2110 == 0) && (((Target != null) || (UnitHS.FSelectedAbility == 65)) || (UnitHS.FSelectedAbility == 114)))
                            {
                                var call__2111 = GetTCUnitHS();
                                MovePoints = call__2111.FUnit.FMovePoints;
                                if (MovePoints > 0)
                                {
                                    if (UnitHS.FSelectedAbility == -1)
                                    {
                                        var call__2112 = GetTCUnitHS();
                                        var call__2113 = call__2112.MeleeMoveTC(w__2093.FPath);
                                        if (call__2113)
                                        {
                                            w__2093.FPath.Clear();
                                        }
                                    }
                                    else
                                    {
                                        var call__2114 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                                        var call__2115 = call__2114.GetCombatMode(UnitHS.FUnit.FUnit);
                                        if (call__2115 == Aow.Units.AoWE.TCombatMode.cmTouch)
                                        {
                                            var call__2116 = GetTCUnitHS();
                                            var call__2117 = call__2116.AbTouchMoveTC(w__2093.FPath);
                                            if (call__2117)
                                            {
                                                w__2093.FPath.Clear();
                                                UpdateMoveCursor();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var call__2118 = Aow.Units.CombatTE.CombatTE.MoveCombatUnit(UnitHS.FUnit, w__2093.FPath);
                                if (call__2118)
                                {
                                    w__2093.FPath.Clear();
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
            var call__2119 = FMoveSettingsList.GetCount();
            if (call__2119 > 0)
            {
                var call__2120 = FMoveSettingsList.GetCount();
                if (call__2120 > 1)
                {
                    var call__2121 = Aow.Units.CombatTE.CombatTE.SetupMultiTCUnitMove(FMoveSettingsList);
                    if (call__2121)
                    {
                        {
                            I = 0;
                            int end__2122 = FMoveSettingsList.GetCount() - 1;
                            for (; I <= end__2122; I += 1)
                            {
                                {
                                    var w__2123 = FMoveSettingsList.GetMoveSettings(I);
                                    w__2123.FPath.Clear();
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
                var call__2124 = FMoveSettingsList.GetMoveSettings(0);
                bool bool__2125 = (call__2124.FPath.Count > 0);
                if (bool__2125)
                {
                    var call__2126 = FMoveSettingsList.GetMoveSettings(0);
                    var call__2127 = call__2126.FPath[0];
                    var call__2128 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                    bool__2125 = (call__2127.X == call__2128);
                }

                bool bool__2129 = bool__2125;
                if (bool__2129)
                {
                    var call__2130 = FMoveSettingsList.GetMoveSettings(0);
                    var call__2131 = call__2130.FPath[0];
                    var call__2132 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                    bool__2129 = (call__2131.Y == call__2132);
                }

                Result = bool__2129;
                var call__2133 = FMoveSettingsList.GetCount();
                I = call__2133;
                while (Result && (I != 0))
                {
                    I -= 1;
                    {
                        var w__2134 = FMoveSettingsList.GetMoveSettings(I);
                        Result = w__2134.FPath.Count != 0;
                    }
                }

                return Result;
            }

            DeactivateRangedPath();
            var call__2135 = FMoveSettingsList.GetCount();
            if (call__2135 > 1)
            {
                if (FCurrentMove == -1)
                {
                    if (!AllPathsValid())
                    {
                        bool bool__2136 = !Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
                        if (!bool__2136)
                        {
                            var call__2137 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                            var call__2138 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                            var call__2139 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__2138, call__2137, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                            var call__2140 = ((Aow.Units.AoWE.TAoWMapField)call__2139).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                            bool__2136 = call__2140;
                        }

                        if (bool__2136)
                        {
                            {
                                I = 0;
                                int end__2141 = FMoveSettingsList.GetCount() - 1;
                                for (; I <= end__2141; I += 1)
                                {
                                    var call__2142 = FMoveSettingsList.GetMoveSettings(I);
                                    call__2142.FPath.Clear();
                                }
                            }

                            {
                                A = 0;
                                int end__2143 = FMoveSettingsList.GetCount() - 1;
                                for (; A <= end__2143; A += 1)
                                {
                                    {
                                        B = A + 1;
                                        int end__2144 = FMoveSettingsList.GetCount() - 1;
                                        for (; B <= end__2144; B += 1)
                                        {
                                            var call__2145 = FMoveSettingsList.GetMoveSettings(B);
                                            var call__2146 = ((Aow.Units.HSEngine.THexagonSprite)call__2145.FMoveObject).GetYhx();
                                            var call__2147 = FMoveSettingsList.GetMoveSettings(B);
                                            var call__2148 = ((Aow.Units.HSEngine.THexagonSprite)call__2147.FMoveObject).GetXhx();
                                            var call__2149 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                            var call__2150 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                            var call__2151 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__2150, call__2149, call__2148, call__2146);
                                            var call__2152 = FMoveSettingsList.GetMoveSettings(A);
                                            var call__2153 = ((Aow.Units.HSEngine.THexagonSprite)call__2152.FMoveObject).GetYhx();
                                            var call__2154 = FMoveSettingsList.GetMoveSettings(A);
                                            var call__2155 = ((Aow.Units.HSEngine.THexagonSprite)call__2154.FMoveObject).GetXhx();
                                            var call__2156 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                            var call__2157 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                            var call__2158 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__2157, call__2156, call__2155, call__2153);
                                            if (call__2151 < call__2158)
                                            {
                                                FMoveSettingsList.FChildren.Exchange(A, B);
                                            }
                                        }
                                    }
                                }
                            }

                            var call__2159 = FMoveSettingsList.GetMoveSettings(0);
                            var call__2160 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__2159.FMoveObject).FUnit.GetLocked();
                            if (call__2160)
                            {
                                var call__2161 = FMoveSettingsList.GetMoveSettings(0);
                                call__2161.FPath.Clear();
                            }
                            else
                            {
                                var call__2162 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                var call__2163 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                CalculateMovePath(0, unchecked((sbyte)call__2163), unchecked((sbyte)call__2162), unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL));
                            }

                            var call__2164 = FMoveSettingsList.GetMoveSettings(0);
                            if (call__2164.FPath.Count > 1)
                            {
                                {
                                    I = 1;
                                    int end__2165 = FMoveSettingsList.GetCount() - 1;
                                    for (; I <= end__2165; I += 1)
                                    {
                                        var call__2166 = FMoveSettingsList.GetMoveSettings(I);
                                        var call__2167 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__2166.FMoveObject).FUnit.GetLocked();
                                        if (call__2167)
                                        {
                                            var call__2168 = FMoveSettingsList.GetMoveSettings(I);
                                            call__2168.FPath.Clear();
                                        }
                                        else
                                        {
                                            var call__2169 = FMoveSettingsList.GetMoveSettings(I);
                                            var call__2170 = ((Aow.Units.HSEngine.THexagonSprite)call__2169.FMoveObject).GetYhx();
                                            var call__2171 = FMoveSettingsList.GetMoveSettings(I);
                                            var call__2172 = ((Aow.Units.HSEngine.THexagonSprite)call__2171.FMoveObject).GetXhx();
                                            var call__2173 = FMoveSettingsList.GetMoveSettings(0);
                                            var call__2174 = ((Aow.Units.HSEngine.THexagonSprite)call__2173.FMoveObject).GetYhx();
                                            var call__2175 = FMoveSettingsList.GetMoveSettings(0);
                                            var call__2176 = ((Aow.Units.HSEngine.THexagonSprite)call__2175.FMoveObject).GetXhx();
                                            var call__2177 = Aow.Units.HSEngine.HSEngine.dHXtoHN(call__2176, call__2174, call__2172, call__2170);
                                            Direction = call__2177;
                                            var call__2178 = FMoveSettingsList.GetMoveSettings(0);
                                            var call__2179 = call__2178.FPath[0];
                                            var call__2180 = FMoveSettingsList.GetMoveSettings(0);
                                            var call__2181 = call__2180.FPath[0];
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__2181.X, call__2179.Y, Direction, ref X, ref Y);
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
                                int end__2182 = FMoveSettingsList.GetCount() - 1;
                                for (; I <= end__2182; I += 1)
                                {
                                    var call__2183 = FMoveSettingsList.GetMoveSettings(I);
                                    call__2183.FPath.Clear();
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
                        var w__2184 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                        bool bool__2185 = (w__2184.FPath.Count != 0);
                        if (bool__2185)
                        {
                            var call__2186 = w__2184.FPath[0];
                            var call__2187 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                            bool__2185 = (call__2186.X == call__2187);
                        }

                        bool bool__2188 = bool__2185;
                        if (bool__2188)
                        {
                            var call__2189 = w__2184.FPath[0];
                            var call__2190 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                            bool__2188 = (call__2189.Y == call__2190);
                        }

                        bool bool__2191 = bool__2188;
                        if (bool__2191)
                        {
                            var call__2192 = w__2184.FPath[0];
                            bool__2191 = (call__2192.L == Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                        }

                        if (bool__2191)
                        {
                            MoveUnits();
                            return;
                        }
                    }

                    var call__2193 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                    var call__2194 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__2193.FMoveObject).FUnit.GetLocked();
                    if (call__2194)
                    {
                        var call__2195 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                        call__2195.FPath.Clear();
                    }
                    else
                    {
                        var call__2196 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                        var call__2197 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                        CalculateMovePath(FCurrentMove, unchecked((sbyte)call__2197), unchecked((sbyte)call__2196), unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL));
                    }
                }
            }
            else
            {
                var call__2198 = GetTCUnitHS();
                bool bool__2199 = (call__2198.FSelectedAbility == -1);
                if (bool__2199)
                {
                    var call__2200 = GetTCUnitHS();
                    var call__2201 = call__2200.GetXhx();
                    var call__2202 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                    bool__2199 = (call__2201 == call__2202);
                }

                bool bool__2203 = bool__2199;
                if (bool__2203)
                {
                    var call__2204 = GetTCUnitHS();
                    var call__2205 = call__2204.GetYhx();
                    var call__2206 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                    bool__2203 = (call__2205 == call__2206);
                }

                if (bool__2203)
                {
                    var call__2207 = GetTCUnitHS();
                    var call__2208 = call__2207.GetLhx();
                    var call__2209 = GetTCUnitHS();
                    var call__2210 = call__2209.GetYhx();
                    var call__2211 = GetTCUnitHS();
                    var call__2212 = call__2211.GetXhx();
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHX(call__2212, call__2210, call__2208);
                    return;
                }

                var call__2213 = GetTCUnitHS();
                var call__2214 = call__2213.FUnit.GetLocked();
                bool bool__2215 = !call__2214;
                if (bool__2215)
                {
                    var call__2216 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentPlayer);
                    bool__2215 = call__2216;
                }

                if (bool__2215)
                {
                    UnitHS = GetTCUnitHS();
                    if (AllPathsValid())
                    {
                        MoveUnits();
                    }
                    else
                    {
                        var call__2217 = FRangedPath.Count;
                        bool bool__2218 = (call__2217 > 0);
                        if (bool__2218)
                        {
                            var call__2219 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                            bool__2218 = (FTargetX == call__2219);
                        }

                        bool bool__2220 = bool__2218;
                        if (bool__2220)
                        {
                            var call__2221 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                            bool__2220 = (FTargetY == call__2221);
                        }

                        if (bool__2220)
                        {
                            var call__2222 = UnitHS.AbRangedTC(unchecked((ushort)FTargetX), unchecked((ushort)FTargetY));
                            if (call__2222)
                            {
                                FRangedPath.Clear();
                                UpdateMoveCursor();
                            }
                        }
                        else
                        {
                            bool bool__2223 = (UnitHS.FSelectedAbility == -1);
                            if (bool__2223)
                            {
                                var call__2224 = UnitHS.GetXhx();
                                var call__2225 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                bool__2223 = (call__2224 == call__2225);
                            }

                            bool bool__2226 = bool__2223;
                            if (bool__2226)
                            {
                                var call__2227 = UnitHS.GetYhx();
                                var call__2228 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                bool__2226 = (call__2227 == call__2228);
                            }

                            if (bool__2226)
                            {
                                var call__2229 = UnitHS.GetLhx();
                                var call__2230 = UnitHS.GetYhx();
                                var call__2231 = UnitHS.GetXhx();
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHX(call__2231, call__2230, call__2229);
                                return;
                            }

                            if (!UnitHS.Moving())
                            {
                                if (UnitHS.FSelectedAbility == -1)
                                {
                                    bool bool__2232 = (UnitHS.FUnit.FMovePoints > 0);
                                    if (bool__2232)
                                    {
                                        var call__2233 = UnitHS.FUnit.GetLocked();
                                        bool__2232 = !call__2233;
                                    }

                                    if (bool__2232)
                                    {
                                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled)
                                        {
                                            var call__2234 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                            var call__2235 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                            var call__2236 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__2235, call__2234, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                            var call__2237 = ((Aow.Units.AoWE.TAoWMapField)call__2236).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                                            if (!call__2237)
                                            {
                                                return;
                                            }
                                        }

                                        var call__2238 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                        var call__2239 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                        CalculateMovePath(FCurrentMove, unchecked((sbyte)call__2239), unchecked((sbyte)call__2238), 0);
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
                                                var call__2240 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                                var call__2241 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                                var call__2242 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__2241, call__2240, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                                var call__2243 = ((Aow.Units.AoWE.TAoWMapField)call__2242).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                                                if (!call__2243)
                                                {
                                                    return;
                                                }
                                            }

                                            var call__2244 = UnitHS.FUnit.GetLocked();
                                            if (!call__2244)
                                            {
                                                var call__2245 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                                var call__2246 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                                CalculateMovePath(0, unchecked((sbyte)call__2246), unchecked((sbyte)call__2245), 0);
                                            }

                                            break;
                                        default:
                                        {
                                            var call__2247 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                            var call__2248 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                            CalculateRangedPath(unchecked((sbyte)call__2248), unchecked((sbyte)call__2247));
                                            var call__2249 = FRangedPath.Count;
                                            if (call__2249 > 0)
                                            {
                                                var call__2250 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                                var call__2251 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                                var call__2252 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                var call__2253 = call__2252.Field[call__2251, call__2250];
                                                var call__2254 = call__2253.FindOwnedChild(2228484);
                                                FMouseOverTarget = (Aow.Units.HSEngine.THexagonSprite)call__2254;
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
                var call__2255 = GetTCUnitHS();
                call__2255.FUnit.FUnit.CreateMovePointTable(ref FMoveControlSettings.FMovePointTable);
            }

            FPathInvalid = TSet<Aow.Units.AoWTC.AnonymousEnum_piMove>.Of(Aow.Units.AoWTC.AnonymousEnum_piMove.piRanged);
        }

        // AoWTC.pas:3300
        public void SetTCUnitHS(Aow.Units.AoWTC.TTacticalCombatUnitHS U)
        {
            if (GetTCUnitHS() != U)
            {
                var call__2256 = FMoveSettingsList.GetCount();
                if (call__2256 <= 1)
                {
                    Clear();
                    bool bool__2257 = (U != null);
                    if (bool__2257)
                    {
                        var call__2258 = U.FUnit.GetPlayer();
                        bool__2257 = (call__2258 == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                    }

                    if (bool__2257)
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
                var call__2259 = FPlayers.GetCount();
                Result = call__2259;
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
            var call__2260 = FPlayers.GetPlayers(unchecked((sbyte)Index));
            Result = (Aow.Units.AoWTC.TTacticalCombatPlayer)call__2260;
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
                var call__2261 = FEffects[0];
                DynamicFixedVisibleHS.RemoveChild((Aow.Units.HSEngine.THexagonSprite)call__2261);
                var call__2262 = FEffects[0];
                DynamicVisibleHS.RemoveChild((Aow.Units.HSEngine.THexagonSprite)call__2262);
                var call__2263 = FEffects[0];
                ((Aow.Rtl.TObject)call__2263).Free();
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
            var call__2264 = Aow.Units.AoWTC.TUnitPositionControl.Create(null);
            FUnitPositions = call__2264;
            var call__2265 = Aow.Units.Engine.TEventList.Create();
            FChangeTurnEvents = call__2265;
            var call__2266 = Aow.Units.Engine.TEventList.Create();
            FPlayerChangedEvents = call__2266;
            var call__2267 = Aow.Units.Engine.TEventList.Create();
            FSeatedPlayerChangedEvents = call__2267;
            var call__2268 = Aow.Units.AoWTC.TCombatMoveControl.Create(this);
            FMoveControl = call__2268;
            var call__2269 = Aow.Units.Engine.TEventList.Create();
            FSelectEvents = call__2269;
            var call__2270 = Aow.Units.Engine.TEventList.Create();
            FDeselectEvents = call__2270;
            var call__2271 = Aow.Units.Engine.TEventList.Create();
            FMouseOverTargetEvents = call__2271;
            var call__2272 = Aow.Units.Engine.TEventList.Create();
            FUnitChangedEvents = call__2272;
            var call__2273 = Aow.Units.AoWTC.TTacticalCombatPlayerList.Create();
            FPlayers = call__2273;
            var call__2274 = Aow.Units.AoWTC.TCombatUnitSelectionControl.Create();
            FUnitSelection = call__2274;
            ((Aow.Units.AoWTC.TAoWCombatMapMouse)Mouse).FRectangleSelectEvents.Add(FUnitSelection.RectangleSelectEvent);
            var call__2275 = new Aow.Rtl.TList();
            FEffects = call__2275;
            var call__2276 = new Aow.Rtl.TList();
            FWalls = call__2276;
            var call__2277 = new Aow.Rtl.TList();
            FFleeLocations = call__2277;
            var call__2278 = Aow.Units.AoWTC.TCAI.Create();
            FAI = call__2278;
            Mouse.SetCapture(this);
            Mouse.OnMove.Add(MouseMoveEvent);
            Mouse.CursorMode = 256;
            {
                I = 0;
                byte end__2279 = 7;
                for (; I <= end__2279; I += 1)
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
                int end__2280 = MapContainer.Width - 7;
                for (; X <= end__2280; X += 1)
                {
                    {
                        Y = 6;
                        int end__2281 = MapContainer.Height - 7;
                        for (; Y <= end__2281; Y += 1)
                        {
                            Aow.Units.AoWE.AoWE.AoWHSSet.FCombatHexagons.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 1);
                        }
                    }
                }
            }

            Control = Engine.HSSet.HexagonControl;
            {
                var w__2282 = Control;
                {
                    X = 0;
                    int end__2283 = MapContainer.Width - 1;
                    for (; X <= end__2283; X += 1)
                    {
                        {
                            Y = 0;
                            byte end__2284 = 5;
                            for (; Y <= end__2284; Y += 1)
                            {
                                w__2282.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 15);
                            }
                        }

                        {
                            Y = MapContainer.Height - 6;
                            int end__2285 = MapContainer.Height - 1;
                            for (; Y <= end__2285; Y += 1)
                            {
                                w__2282.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 15);
                            }
                        }
                    }
                }

                {
                    Y = 6;
                    int end__2286 = MapContainer.Height - 7;
                    for (; Y <= end__2286; Y += 1)
                    {
                        {
                            X = 0;
                            byte end__2287 = 5;
                            for (; X <= end__2287; X += 1)
                            {
                                w__2282.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 15);
                            }
                        }

                        {
                            X = MapContainer.Width - 6;
                            int end__2288 = MapContainer.Width - 1;
                            for (; X <= end__2288; X += 1)
                            {
                                w__2282.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 15);
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
                    var call__2289 = FEffects[I];
                    Effect = (Aow.Units.AoWTC.TfxHS)call__2289;
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
                bool bool__2290 = FEndTurnPending;
                if (bool__2290)
                {
                    var call__2291 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FCurrentPlayer);
                    bool bool__2292 = (call__2291.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman);
                    if (bool__2292)
                    {
                        var call__2293 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FCombat.FHostPlayer);
                        bool__2292 = (call__2293.ConnectionMode != Aow.Units.NetworkE.TConnectionMode.cmRemote);
                    }

                    bool bool__2294 = bool__2292;
                    if (!bool__2294)
                    {
                        var call__2295 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                        bool bool__2296 = call__2295.FAuto;
                        if (bool__2296)
                        {
                            var call__2297 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FCurrentPlayer);
                            bool__2296 = (call__2297.ConnectionMode != Aow.Units.NetworkE.TConnectionMode.cmRemote);
                        }

                        bool__2294 = bool__2296;
                    }

                    bool__2290 = bool__2294;
                }

                if (bool__2290)
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
            var call__2298 = FCombat.FCombatData.GetCount();
            I = call__2298;
            while ((I != 0) && (X == -1))
            {
                I -= 1;
                var call__2299 = FCombat.FCombatData.GetObjects(I);
                bool bool__2300 = (call__2299 is Aow.Units.AoWTC.TTacticalCombatUnit);
                if (bool__2300)
                {
                    var call__2301 = FCombat.FCombatData.GetObjects(I);
                    bool__2300 = (((Aow.Units.AoWTC.TTacticalCombatUnit)call__2301).FUnit == Hero);
                }

                if (bool__2300)
                {
                    var call__2302 = FCombat.FCombatData.GetObjects(I);
                    var call__2303 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__2302).FUnitHS.GetXhx();
                    X = unchecked((sbyte)call__2303);
                    var call__2304 = FCombat.FCombatData.GetObjects(I);
                    var call__2305 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__2304).FUnitHS.GetYhx();
                    Y = unchecked((sbyte)call__2305);
                    L = 0;
                }
            }

            if (X > -1)
            {
                var call__2306 = MapContainer.GetField(X, Y, L);
                var call__2307 = call__2306.FindOwnedChild(131697);
                HS = (Aow.Units.AoWE.TItemHS)call__2307;
                if (HS == null)
                {
                    HS = Aow.Units.AoWTC.TCItemHS.Create(null);
                    // { Original bug: a failed placement leaves the new item sprite allocated. }
                    var call__2308 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(HS, X, Y, L);
                    if (call__2308)
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
                var call__2309 = MapContainer.GetField(X, Y, L);
                var call__2310 = call__2309.FindOwnedChild(131697);
                HS = (Aow.Units.AoWE.TItemHS)call__2310;
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
            bool bool__2311 = !Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting);
            if (bool__2311)
            {
                var call__2312 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                bool__2311 = !call__2312.FAuto;
            }

            bool bool__2313 = bool__2311;
            if (bool__2313)
            {
                var call__2314 = Mouse.Xhx;
                bool__2313 = (call__2314 != -1);
            }

            bool bool__2315 = bool__2313;
            if (bool__2315)
            {
                var call__2316 = Mouse.Yhx;
                bool__2315 = (call__2316 != -1);
            }

            if (bool__2315 && (Mouse.Capture == null))
            {
                Mouse.CursorMode = 0;
                var call__2317 = Mouse.Yhx;
                var call__2318 = Mouse.Xhx;
                var call__2319 = MapContainer.MapLevels[0];
                var call__2320 = call__2319.Field[call__2318, call__2317];
                var call__2321 = call__2320.FindOwnedChild(2228484);
                HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__2321;
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
                var call__2322 = FCombat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                SetSeatedPlayer(unchecked((sbyte)call__2322.FPlayer));
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

            var call__2323 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
            SetCurrentPlayer(unchecked((sbyte)call__2323.FPlayer));
            FCombat.NewTurn(unchecked((byte)unchecked((byte)FCurrentCombatPlayer)));
            FUnitSelection.Unselect();
            Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.TokenExecuter.ActivateTC(FCurrentPlayer);
            var call__2324 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
            if (call__2324.FPlayer == 0)
            {
                var call__2325 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWTC.AoWTC.IndRStr);
                var call__2326 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWTC.AoWTC.BeginRoundRStr);
                ShowMessage((call__2325 + AnsiString.FromBytes(32)) + call__2326);
            }
            else
            {
                var call__2327 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                var call__2328 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__2327.FPlayer);
                var call__2329 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__2328.FPlayerRace]);
                var call__2330 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWTC.AoWTC.BeginRoundRStr);
                ShowMessage((call__2329 + AnsiString.FromBytes(32)) + call__2330);
            }

            var call__2331 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
            var call__2332 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__2331.FPlayer);
            if (call__2332.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman)
            {
                var call__2333 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                var call__2334 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(call__2333.FPlayer);
                if (call__2334.ConnectionMode != Aow.Units.NetworkE.TConnectionMode.cmRemote)
                {
                    SetSeatedCombatPlayer(unchecked((sbyte)FCurrentCombatPlayer));
                    var call__2335 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                    if (call__2335.FLastX == 0)
                    {
                        U = GetNextUnit(null, true, true);
                        if (U != null)
                        {
                            var call__2336 = U.FUnitHS.GetXhx();
                            var call__2337 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                            call__2337.FLastX = call__2336;
                            var call__2338 = U.FUnitHS.GetYhx();
                            var call__2339 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                            call__2339.FLastY = call__2338;
                            if (FExplorationEnabled)
                            {
                                {
                                    I = 0;
                                    int end__2340 = FPlayers.GetCount() - 1;
                                    for (; I <= end__2340; I += 1)
                                    {
                                        if (FCurrentCombatPlayer != I)
                                        {
                                            var call__2341 = U.FUnitHS.GetXhx();
                                            var call__2342 = FPlayers.GetPlayers(unchecked((sbyte)I));
                                            call__2342.FLastX = call__2341;
                                            var call__2343 = U.FUnitHS.GetYhx();
                                            var call__2344 = FPlayers.GetPlayers(unchecked((sbyte)I));
                                            call__2344.FLastY = call__2343;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var call__2345 = FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                    var call__2346 = FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                    CenterViewTo(call__2346.FLastX, call__2345.FLastY, 0);
                }
            }

            Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Events\\NewEvent.wav");
            Aow.Units.Engine.Engine.InitMsg(ref Msg, 537006081, this);
            SendMsg(ref Msg, this);
            if (FExplorationEnabled)
            {
                var call__2347 = Aow.Rtl.Rtl.Rect(0, 0, 65, 58);
                TriggerMapChangedEvent(call__2347, 0);
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
                var call__2348 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FCurrentPlayer);
                if (call__2348.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman)
                {
                    return Result;
                }

                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer != Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)
                {
                    return Result;
                }
            }

            var call__2349 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(FCurrentPlayer);
            if (call__2349)
            {
                var call__2350 = Aow.Units.CombatTE.CombatTE.CreateTCEndTurnTE(unchecked((sbyte)FCurrentPlayer));
                TE = call__2350;
                if (TE != null)
                {
                    var call__2351 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(FCurrentPlayer);
                    call__2351.AddEvent(TE);
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
                int end__2352 = FPlayers.GetCount() - 1;
                for (; I <= end__2352; I += 1)
                {
                    bool bool__2353 = Result;
                    if (bool__2353)
                    {
                        var call__2354 = FPlayers.GetPlayers(unchecked((sbyte)I));
                        bool__2353 = call__2354.FEndTurn;
                    }

                    Result = bool__2353;
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
                var w__2355 = FCombat;
                Index = 0;
                I = 0;
                Start = 0;
                if (U != null)
                {
                    while (true)
                    {
                        var call__2356 = w__2355.FCombatData.GetCount();
                        if (!(I < call__2356))
                            break;
                        {
                            var call__2357 = w__2355.FCombatData.GetObjects(I);
                            if (call__2357 == U)
                            {
                                Index = I + Step;
                                Start = I;
                                var call__2358 = w__2355.FCombatData.GetCount();
                                I = call__2358;
                            }

                            I += 1;
                        }
                    }
                }

                while (true)
                {
                    var call__2359 = w__2355.FCombatData.GetCount();
                    if (!((Index < call__2359) && (Index > -1)))
                        break;
                    {
                        var call__2360 = w__2355.FCombatData.GetObjects(Index);
                        bool bool__2361 = (call__2360 != null);
                        if (bool__2361)
                        {
                            var call__2362 = w__2355.FCombatData.GetObjects(Index);
                            bool__2361 = (call__2362 is Aow.Units.AoWE.TCombatUnit);
                        }

                        bool bool__2363 = bool__2361;
                        if (bool__2363)
                        {
                            bool bool__2364 = AllUnits;
                            if (!bool__2364)
                            {
                                var call__2365 = w__2355.FCombatData.GetObjects(Index);
                                bool__2364 = (((Aow.Units.AoWTC.TTacticalCombatUnit)call__2365).FMovePoints > 0);
                            }

                            bool__2363 = bool__2364;
                        }

                        bool bool__2366 = bool__2363;
                        if (bool__2366)
                        {
                            var call__2367 = w__2355.FCombatData.GetObjects(Index);
                            var call__2368 = call__2367.GetEnabled();
                            bool__2366 = call__2368;
                        }

                        bool bool__2369 = bool__2366;
                        if (bool__2369)
                        {
                            var call__2370 = w__2355.FCombatData.GetObjects(Index);
                            bool__2369 = (call__2370.FCombatPlayer == FCurrentCombatPlayer);
                        }

                        if (bool__2369)
                        {
                            var call__2371 = w__2355.FCombatData.GetObjects(Index);
                            Result = (Aow.Units.AoWTC.TTacticalCombatUnit)call__2371;
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
                        var call__2372 = w__2355.FCombatData.GetCount();
                        Index = call__2372 - 1;
                    }

                    while (true)
                    {
                        bool bool__2373 = (Index != Start) && (Index > -1);
                        if (bool__2373)
                        {
                            var call__2374 = w__2355.FCombatData.GetCount();
                            bool__2373 = (Index < call__2374);
                        }

                        if (!bool__2373)
                            break;
                        {
                            var call__2375 = w__2355.FCombatData.GetObjects(Index);
                            bool bool__2376 = (call__2375 != null);
                            if (bool__2376)
                            {
                                var call__2377 = w__2355.FCombatData.GetObjects(Index);
                                bool__2376 = (call__2377 is Aow.Units.AoWE.TCombatUnit);
                            }

                            bool bool__2378 = bool__2376;
                            if (bool__2378)
                            {
                                bool bool__2379 = AllUnits;
                                if (!bool__2379)
                                {
                                    var call__2380 = w__2355.FCombatData.GetObjects(Index);
                                    bool__2379 = (((Aow.Units.AoWTC.TTacticalCombatUnit)call__2380).FMovePoints > 0);
                                }

                                bool__2378 = bool__2379;
                            }

                            bool bool__2381 = bool__2378;
                            if (bool__2381)
                            {
                                var call__2382 = w__2355.FCombatData.GetObjects(Index);
                                var call__2383 = call__2382.GetEnabled();
                                bool__2381 = call__2383;
                            }

                            bool bool__2384 = bool__2381;
                            if (bool__2384)
                            {
                                var call__2385 = w__2355.FCombatData.GetObjects(Index);
                                bool__2384 = (call__2385.FCombatPlayer == FCurrentCombatPlayer);
                            }

                            if (bool__2384)
                            {
                                var call__2386 = w__2355.FCombatData.GetObjects(Index);
                                Result = (Aow.Units.AoWTC.TTacticalCombatUnit)call__2386;
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
            bool bool__2387 = (FSeatedPlayer == -1);
            if (!bool__2387)
            {
                var call__2388 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(FSeatedPlayer);
                bool__2387 = call__2388;
            }

            bool bool__2389 = bool__2387 && (FSeatedPlayer == FCurrentPlayer);
            if (bool__2389)
            {
                var call__2390 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                bool__2389 = !call__2390.FAuto;
            }

            if (!(bool__2389 && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)))
            {
                Mouse.Cursor = (((int)(((uint)Frame) >> 2)) % 4) + 6;
            }
            else
            {
                var call__2391 = FUnitSelection.GetTCUnitHS();
                if (call__2391 != null)
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
                var call__2392 = FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                var call__2393 = GetNextUnit(call__2392.FLastUnit, false, Forward);
                U = call__2393;
                if (U != null)
                {
                    var call__2394 = FUnitSelection.GetTCUnitHS();
                    if (call__2394 != U.FUnitHS)
                    {
                        var call__2395 = FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                        call__2395.FLastUnit = U;
                        var call__2396 = U.FUnitHS.GetYhx();
                        var call__2397 = U.FUnitHS.GetXhx();
                        CenterViewTo(call__2397, call__2396, 0);
                        Selection.UnselectLocation();
                        var call__2398 = U.FUnitHS.GetYhx();
                        var call__2399 = U.FUnitHS.GetXhx();
                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__2399, call__2398, ref X, ref Y);
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
                var call__2400 = FCombat.FCombatData.GetCount();
                if (!(I < call__2400))
                    break;
                {
                    var call__2401 = FCombat.FCombatData.GetObjects(I);
                    bool bool__2402 = (((Aow.Units.AoWTC.TTacticalCombatUnit)call__2401).FUnit == U);
                    if (bool__2402)
                    {
                        var call__2403 = FCombat.FCombatData.GetObjects(I);
                        var call__2404 = call__2403.GetEnabled();
                        bool__2402 = call__2404;
                    }

                    if (bool__2402)
                    {
                        var call__2405 = FCombat.FCombatData.GetObjects(I);
                        var call__2406 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__2405).FUnitHS.GetXhx();
                        X = unchecked((sbyte)call__2406);
                        var call__2407 = FCombat.FCombatData.GetObjects(I);
                        var call__2408 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__2407).FUnitHS.GetYhx();
                        Y = unchecked((sbyte)call__2408);
                        L = 0;
                        var call__2409 = FCombat.FCombatData.GetCount();
                        I = call__2409;
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
            var call__2410 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentPlayer);
            if (call__2410 && (FCurrentCombatPlayer == FSeatedCombatPlayer))
            {
                FUnitSelection.Unselect();
                TE = Aow.Units.CombatTE.CombatTE.CreateTCEndCombatTE();
                if (TE != null)
                {
                    var call__2411 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(FCurrentPlayer);
                    call__2411.AddEvent(TE);
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
                int end__2412 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetCount() - 1;
                for (; I <= end__2412; I += 1)
                {
                    if (FHostPlayer == -1)
                    {
                        var call__2413 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                        if (call__2413.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman)
                        {
                            var call__2414 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                            if (call__2414.FStatus != Aow.Units.AoWE.TPlayerStatus.psTerminated)
                            {
                                FHostPlayer = unchecked((sbyte)I);
                            }
                        }
                    }
                }
            }

            {
                I = 0;
                int end__2415 = FCombatData.FPlayers.GetCount() - 1;
                for (; I <= end__2415; I += 1)
                {
                    var call__2416 = FCombatData.FTurnOrder.IndexOf(unchecked((byte)I));
                    if (call__2416 == -1)
                    {
                        var call__2417 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                        if (call__2417.FSideIndex == 0)
                        {
                            FCombatData.FTurnOrder.Add(unchecked((byte)I));
                        }
                    }

                    var call__2418 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                    var call__2419 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__2418.FPlayer);
                    if (call__2419.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman)
                    {
                        if (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC is Aow.Units.NetworkE.TRemotePCC)
                        {
                            var call__2420 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FHostPlayer);
                            var call__2421 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                            ((Aow.Units.NetworkE.TRemotePCC)Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC).SetConnectionMode(call__2421.FPlayer, call__2420.ConnectionMode);
                            var call__2422 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FHostPlayer);
                            var call__2423 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                            ((Aow.Units.NetworkE.TRemotePCC)Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC).SetSystemID(call__2423.FPlayer, call__2422.SystemID);
                        }
                    }
                }
            }

            {
                I = 0;
                int end__2424 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetCount() - 1;
                for (; I <= end__2424; I += 1)
                {
                    var call__2425 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(I);
                    var call__2426 = call__2425.Local();
                    bool bool__2427 = call__2426;
                    if (!bool__2427)
                    {
                        var call__2428 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(I);
                        var call__2429 = call__2428.Global();
                        bool__2427 = call__2429;
                    }

                    if (bool__2427)
                    {
                        var call__2430 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(I);
                        call__2430.ClearEvents();
                    }
                }
            }

            FCombatData.FTurnOrder.Add(unchecked((byte)FCombatData.FDefenderPlayer));
            {
                I = 0;
                int end__2431 = FCombatData.FPlayers.GetCount() - 1;
                for (; I <= end__2431; I += 1)
                {
                    var call__2432 = FCombatData.FTurnOrder.IndexOf(unchecked((byte)I));
                    if (call__2432 == -1)
                    {
                        var call__2433 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                        if (call__2433.FSideIndex == 1)
                        {
                            FCombatData.FTurnOrder.Add(unchecked((byte)I));
                        }
                    }
                }
            }

            SetLogCombatActions(1);
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
            var call__2434 = FCombatData.FParties.GetParty(0);
            Result = call__2434.FWall == 0;
            I = Side.GetCount();
            while (!Result && (I > 0))
            {
                I -= 1;
                var call__2435 = Side.GetObjects(I);
                var call__2436 = call__2435.WallCombatFeatures();
                Result = (call__2436 & TSet<Aow.Units.AoWE.TWallCombatFeaturesElement>.Of(Aow.Units.AoWE.TWallCombatFeaturesElement.wcWallBreaching, Aow.Units.AoWE.TWallCombatFeaturesElement.wcWallPassing)) != TSet<Aow.Units.AoWE.TWallCombatFeaturesElement>.Of();
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
                short end__2437 = 285;
                for (; I <= end__2437; I += 1)
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
                short end__2438 = 285;
                for (; I <= end__2438; I += 1)
                {
                    var call__2439 = new Aow.Rtl.TList();
                    FResources[I] = call__2439;
                }
            }

            {
                J = 0;
                int end__2440 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.GetCount() - 1;
                for (; J <= end__2440; J += 1)
                {
                    var call__2441 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.MORs[J];
                    if (call__2441 is Aow.Units.CombatTerrain.TCombatTerrainResource)
                    {
                        var call__2442 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.MORs[J];
                        var call__2443 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.MORs[J];
                        var call__2444 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.MORs[J];
                        FResources[(unchecked((sbyte)((Aow.Units.CombatTerrain.TCombatTerrainResource)call__2443).TerrainType) * 19) + ((Aow.Units.CombatTerrain.TCombatTerrainResource)call__2444).DecorationType].Add(call__2442);
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
                    int end__2445 = Last;
                    for (; I <= end__2445; I += 1)
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
                byte end__2446 = 6;
                for (; I <= end__2446; I += 1)
                {
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                    var call__2447 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                    var call__2448 = call__2447.Field[J, K];
                    FTerrain[I] = unchecked((sbyte)call__2448.TerrainType);
                    var call__2449 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                    var call__2450 = call__2449.Field[J, K];
                    var call__2451 = ((Aow.Units.AoWE.TAoWMapField)call__2450).GetCombatTerrainTag();
                    FTags[I] = unchecked((sbyte)call__2451);
                }
            }

            if (((FTerrain[0] == 0) || (FTerrain[0] == 9)) || (FTerrain[0] == 10))
            {
                {
                    I = 1;
                    byte end__2452 = 6;
                    for (; I <= end__2452; I += 1)
                    {
                        FTerrain[I] = unchecked((sbyte)FTerrain[0]);
                    }
                }
            }
            else
            {
                {
                    I = 1;
                    byte end__2453 = 6;
                    for (; I <= end__2453; I += 1)
                    {
                        if ((((FTerrain[I] == 0) || (FTerrain[I] == 15)) || (FTerrain[I] == 10)) || (FTerrain[I] == 9))
                        {
                            FTerrain[I] = unchecked((sbyte)FTerrain[0]);
                        }
                    }
                }
            }

            FileName = (AnsiString)"TCMaps\\";
            var call__2454 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
            var call__2455 = call__2454.Field[X, Y];
            var call__2456 = call__2455.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
            Structure = call__2456;
            if (Structure == null)
            {
                var call__2457 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                var call__2458 = call__2457.Field[X, Y];
                var call__2459 = call__2458.FindNoneTransparentHS(typeof(Aow.Units.HxBridge.THexagonBridge));
                Structure = call__2459;
            }

            if (Structure != null)
            {
                switch (Structure.ClassType.ClassID())
                {
                    case 131336:
                        FileName = FileName + AnsiString.FromBytes(48);
                    {
                        I = 1;
                        byte end__2460 = 6;
                        for (; I <= end__2460; I += 1)
                        {
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                            var call__2461 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                            var call__2462 = call__2461.Field[J, K];
                            var call__2463 = call__2462.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                            if (call__2463 != null)
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
                                byte end__2464 = 6;
                                for (; I <= end__2464; I += 1)
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
                                byte end__2465 = 6;
                                for (; I <= end__2465; I += 1)
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
                                byte end__2466 = 6;
                                for (; I <= end__2466; I += 1)
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
                                byte end__2467 = 6;
                                for (; I <= end__2467; I += 1)
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
                                byte end__2468 = 6;
                                for (; I <= end__2468; I += 1)
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
                    byte end__2469 = 52;
                    for (; Row <= end__2469; Row += 1)
                    {
                        {
                            Col = 6;
                            byte end__2470 = 58;
                            for (; Col <= end__2470; Col += 1)
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

                                var call__2471 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__2472 = call__2471.Field[Col, Row];
                                var call__2473 = call__2472.GetDecorationType();
                                if (call__2473 == 9)
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
                                        var call__2474 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                                        var call__2475 = call__2474.Field[K, NX];
                                        var call__2476 = call__2475.FindOwnedChild(131946);
                                        if (call__2476 == null)
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

                var call__2477 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                var call__2478 = call__2477.Field[X, Y];
                if (call__2478.OverlayType == 4)
                {
                    {
                        I = 0;
                        byte end__2479 = 6;
                        for (; I <= end__2479; I += 1)
                        {
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                            var call__2480 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                            var call__2481 = call__2480.Field[J, K];
                            Weights[I] = call__2481.OverlayType;
                        }
                    }

                    {
                        Row = 6;
                        byte end__2482 = 52;
                        for (; Row <= end__2482; Row += 1)
                        {
                            {
                                Col = 6;
                                byte end__2483 = 58;
                                for (; Col <= end__2483; Col += 1)
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
                    byte end__2484 = 52;
                    for (; Row <= end__2484; Row += 1)
                    {
                        {
                            Col = 3;
                            byte end__2485 = 29;
                            for (; Col <= end__2485; Col += 1)
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
                                        byte end__2486 = 18;
                                        for (; I <= end__2486; I += 1)
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
                                        byte end__2487 = 2;
                                        for (; I <= end__2487; I += 1)
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

                                    var call__2488 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__2489 = call__2488.Field[Col * 2, Row];
                                    var call__2490 = call__2489.FindChild(2228484);
                                    HasUnit = call__2490 != null;
                                    if (HasUnit || Blocked(Col * 2, Row, 0, 2))
                                    {
                                        bool bool__2491 = HasUnit;
                                        if (!bool__2491)
                                        {
                                            var call__2492 = Blocked(Col * 2, Row, 4, 8);
                                            bool__2491 = call__2492;
                                        }

                                        bool bool__2493 = bool__2491;
                                        if (bool__2493)
                                        {
                                            bool bool__2494 = HasUnit;
                                            if (!bool__2494)
                                            {
                                                var call__2495 = Blocked(Col * 2, Row, 9, 17);
                                                bool__2494 = call__2495;
                                            }

                                            bool__2493 = bool__2494;
                                        }

                                        if (bool__2493)
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
                                        bool bool__2496 = HasUnit;
                                        if (!bool__2496)
                                        {
                                            var call__2497 = Blocked(Col * 2, Row, 4, 8);
                                            bool__2496 = call__2497;
                                        }

                                        bool bool__2498 = bool__2496;
                                        if (bool__2498)
                                        {
                                            bool bool__2499 = HasUnit;
                                            if (!bool__2499)
                                            {
                                                var call__2500 = Blocked(Col * 2, Row, 9, 17);
                                                bool__2499 = call__2500;
                                            }

                                            bool__2498 = bool__2499;
                                        }

                                        if (bool__2498)
                                        {
                                            Weights[14] = 0;
                                            Weights[15] = 0;
                                            Weights[16] = 0;
                                        }
                                    }

                                    J = 0;
                                    {
                                        I = 0;
                                        byte end__2501 = 18;
                                        for (; I <= end__2501; I += 1)
                                        {
                                            J += Weights[I];
                                        }
                                    }

                                    var call__2502 = Aow.Rtl.Rtl.Random(J);
                                    Tag = call__2502 - Weights[0];
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
                            byte end__2503 = 29;
                            for (; Col <= end__2503; Col += 1)
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
                                        byte end__2504 = 18;
                                        for (; I <= end__2504; I += 1)
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
                                        byte end__2505 = 2;
                                        for (; I <= end__2505; I += 1)
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

                                    var call__2506 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__2507 = call__2506.Field[(Col * 2) + 1, Row];
                                    var call__2508 = call__2507.FindChild(2228484);
                                    HasUnit = call__2508 != null;
                                    if (HasUnit || Blocked((Col * 2) + 1, Row, 0, 2))
                                    {
                                        bool bool__2509 = HasUnit;
                                        if (!bool__2509)
                                        {
                                            var call__2510 = Blocked((Col * 2) + 1, Row, 4, 8);
                                            bool__2509 = call__2510;
                                        }

                                        bool bool__2511 = bool__2509;
                                        if (bool__2511)
                                        {
                                            bool bool__2512 = HasUnit;
                                            if (!bool__2512)
                                            {
                                                var call__2513 = Blocked((Col * 2) + 1, Row, 9, 17);
                                                bool__2512 = call__2513;
                                            }

                                            bool__2511 = bool__2512;
                                        }

                                        if (bool__2511)
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
                                        bool bool__2514 = HasUnit;
                                        if (!bool__2514)
                                        {
                                            var call__2515 = Blocked((Col * 2) + 1, Row, 4, 8);
                                            bool__2514 = call__2515;
                                        }

                                        bool bool__2516 = bool__2514;
                                        if (bool__2516)
                                        {
                                            bool bool__2517 = HasUnit;
                                            if (!bool__2517)
                                            {
                                                var call__2518 = Blocked((Col * 2) + 1, Row, 9, 17);
                                                bool__2517 = call__2518;
                                            }

                                            bool__2516 = bool__2517;
                                        }

                                        if (bool__2516)
                                        {
                                            Weights[14] = 0;
                                            Weights[15] = 0;
                                            Weights[16] = 0;
                                        }
                                    }

                                    J = 0;
                                    {
                                        I = 0;
                                        byte end__2519 = 18;
                                        for (; I <= end__2519; I += 1)
                                        {
                                            J += Weights[I];
                                        }
                                    }

                                    var call__2520 = Aow.Rtl.Rtl.Random(J);
                                    Tag = call__2520 - Weights[0];
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
                    byte end__2521 = 52;
                    for (; Row <= end__2521; Row += 1)
                    {
                        {
                            Col = 6;
                            byte end__2522 = 58;
                            for (; Col <= end__2522; Col += 1)
                            {
                                I = ShapeBuffer[(Row * 65) + Col];
                                if (I > 0)
                                {
                                    J = TerrainBuffer[(Row * 65) + Col];
                                    if (J == 17)
                                    {
                                        var call__2523 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__2524 = call__2523.Field[Col, Row];
                                        call__2524.TerrainType = 7;
                                    }
                                    else
                                    {
                                        var call__2525 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__2526 = call__2525.Field[Col, Row];
                                        call__2526.TerrainType = unchecked((sbyte)FTerrain[I - 1]);
                                        if ((J > 0) && (J < 19))
                                        {
                                            if (FResources[(FTerrain[I - 1] * 19) + J].Count > 0)
                                            {
                                                var call__2527 = Aow.Rtl.Rtl.Random(FResources[(FTerrain[I - 1] * 19) + J].Count);
                                                var call__2528 = FResources[(FTerrain[I - 1] * 19) + J][call__2527];
                                                ((Aow.Units.HSEngine.TMapObjectResource)call__2528).PlaceResourceUser(Aow.Units.AoWTC.AoWTC.AoWCombatMap, unchecked((sbyte)Col), unchecked((sbyte)Row), 0);
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
                    byte end__2529 = 52;
                    for (; Row <= end__2529; Row += 1)
                    {
                        {
                            Col = 6;
                            byte end__2530 = 58;
                            for (; Col <= end__2530; Col += 1)
                            {
                                var call__2531 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__2532 = call__2531.Field[Col, Row];
                                call__2532.TerrainChanged();
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
                byte end__2533 = 7;
                for (; Position <= end__2533; Position += 1)
                {
                    Units.Clear();
                    {
                        I = 0;
                        int end__2534 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                        for (; I <= end__2534; I += 1)
                        {
                            var call__2535 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            if (call__2535 is Aow.Units.AoWE.TCombatUnit)
                            {
                                var call__2536 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                var call__2537 = call__2536.GetEnabled();
                                if (call__2537)
                                {
                                    var call__2538 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                    if (((int)(((uint)call__2538.FPosition) >> 4)) == Position)
                                    {
                                        var call__2539 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                        Units.Add(call__2539);
                                        var call__2540 = Units[Units.Count - 1];
                                        U = (Aow.Units.AoWTC.TTacticalCombatUnit)call__2540;
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
                                                var call__2541 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL];
                                                var call__2542 = call__2541.Field[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY];
                                                var call__2543 = call__2542.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                                                if (call__2543 != null)
                                                {
                                                    ;
                                                }
                                            }
                                        }

                                        if ((FHomePosition > 0) && (U.FPositionGroup == FHomePosition))
                                        {
                                            U.FPositionGroup = 0;
                                        }

                                        var call__2544 = U.GetSide();
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPositionStatus[U.FPositionGroup] = unchecked((byte)unchecked((byte)call__2544));
                                    }
                                }
                            }
                        }
                    }

                    {
                        I = 0;
                        int end__2545 = Units.Count - 1;
                        for (; I <= end__2545; I += 1)
                        {
                            var call__2546 = Units[I];
                            U = (Aow.Units.AoWTC.TTacticalCombatUnit)call__2546;
                            {
                                J = I + 1;
                                int end__2547 = Units.Count - 1;
                                for (; J <= end__2547; J += 1)
                                {
                                    var call__2548 = Units[J];
                                    Other = (Aow.Units.AoWTC.TTacticalCombatUnit)call__2548;
                                    var call__2549 = U.GetPosValue();
                                    var call__2550 = Other.GetPosValue();
                                    if (call__2549 < call__2550)
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
                var call__2551 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL];
                var call__2552 = call__2551.Field[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY];
                var call__2553 = call__2552.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                if ((call__2553 != null) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallType != 0))
                {
                    Position = 1;
                    while (Position < 7)
                    {
                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPositionStatus[Position] == 1)
                        {
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY, Position, ref J, ref Temp);
                            var call__2554 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL];
                            var call__2555 = call__2554.Field[J, Temp];
                            var call__2556 = call__2555.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                            if (call__2556 != null)
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
                int end__2557 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csAttacker].GetCount() - 1;
                for (; Position <= end__2557; Position += 1)
                {
                    var call__2558 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csAttacker].GetObjects(Position);
                    var call__2559 = call__2558.GetEnabled();
                    if (call__2559)
                    {
                        SetupUnit(Aow.Units.AoWE.TCombatSideIndex.csAttacker, Position, ref Direction);
                    }
                }
            }

            {
                Position = 0;
                int end__2560 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].GetCount() - 1;
                for (; Position <= end__2560; Position += 1)
                {
                    var call__2561 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].GetObjects(Position);
                    if (call__2561 is Aow.Units.AoWE.TCombatUnit)
                    {
                        var call__2562 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].GetObjects(Position);
                        var call__2563 = call__2562.GetEnabled();
                        if (call__2563)
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
                    byte end__2564 = 7;
                    for (; Position <= end__2564; Position += 1)
                    {
                        Used[Position] = false;
                    }
                }

                Position = 0;
                while (true)
                {
                    var call__2565 = FItems.GetCount();
                    if (!(Position < call__2565))
                        break;
                    {
                        var call__2566 = Aow.Rtl.Rtl.Random(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.FPositions.Count - 16);
                        I = call__2566;
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
                        var call__2567 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(2, I);
                        J = call__2567.X;
                        var call__2568 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(2, I);
                        Temp = call__2568.Y;
                        ItemHS = Aow.Units.AoWTC.TCItemHS.Create(null);
                        var call__2569 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(ItemHS, J, Temp, 0);
                        if (call__2569)
                        {
                            var call__2570 = FItems.GetItem(Position);
                            call__2570.SetOwner(ItemHS.FItems);
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
            var call__2571 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Side].GetObjects(Index);
            UnitHS.Initialize((Aow.Units.AoWTC.TTacticalCombatUnit)call__2571);
            if (UnitHS.FUnit.FPositionGroup == FHomePosition)
            {
                UnitHS.FUnit.FPositionGroup = 0;
            }

            Position = UnitHS.FUnit.FPositionGroup;
            Slot = UnitHS.FUnit.FPositionIndex;
            var call__2572 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(Position, Slot);
            X = call__2572.X;
            var call__2573 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(Position, Slot);
            Y = call__2573.Y;
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
                var call__2574 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(Position, Slot);
                UnitHS.SetDirection(unchecked((byte)call__2574.Flag));
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
                var call__2575 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__2576 = call__2575.Field[NX, NY];
                Slot = call__2576.OverlayType;
                var call__2577 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__2578 = call__2577.Field[NX, NY];
                var call__2579 = call__2578.FindOwnedChild(2228484);
                TargetHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__2579;
                HN += 1;
            }
            while (!((Slot != 8) && (TargetHS == null)));
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(UnitHS, NX, NY, 0);
            var call__2580 = Aow.Units.HSEngine.HSEngine.dHXtoHN(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FAttackerX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FAttackerY);
            bool bool__2581 = (call__2580 == Position);
            if (bool__2581)
            {
                var call__2582 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)UnitHS.FUnit.FCombatPlayer));
                bool__2581 = (call__2582.FLastX == 0);
            }

            if (bool__2581)
            {
                var call__2583 = UnitHS.GetXhx();
                var call__2584 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)UnitHS.FUnit.FCombatPlayer));
                call__2584.FLastX = call__2583;
                var call__2585 = UnitHS.GetYhx();
                var call__2586 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)UnitHS.FUnit.FCombatPlayer));
                call__2586.FLastY = call__2585;
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
                    var call__2587 = Aow.Units.aowFX.TXYZ.Create();
                    Path.Add(call__2587);
                    Aow.Units.HSEngine.HSEngine.HXtoHP(X2, Y2, ref X, ref Y);
                    var call__2588 = Path[0];
                    call__2588.X = X + 12;
                    var call__2589 = Path[0];
                    call__2589.Y = Y + 12;
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
                                    var call__2590 = Aow.Units.aowFX.TXYZ.Create();
                                    Path.Add(call__2590);
                                    Aow.Units.HSEngine.HSEngine.HXtoHP(X2, Y2, ref X, ref Y);
                                    var call__2591 = Path[0];
                                    call__2591.X = X + 12;
                                    var call__2592 = Path[0];
                                    call__2592.Y = Y + 12;
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
                    var call__2593 = System.Math.Sqrt(Height);
                    Point.Z = Aow.Rtl.Rtl.Round(call__2593) - ((Steps * 10) / Arc);
                }

                I += 1;
            }

            LastX = 0;
            LastY = 0;
            var call__2594 = Path[0];
            Point = call__2594;
            Aow.Units.HSEngine.HSEngine.HPtoHX(Point.X, Point.Y, ref StartX, ref StartY);
            var call__2595 = Path.Count;
            var call__2596 = Path[call__2595 - 1];
            Point = call__2596;
            Aow.Units.HSEngine.HSEngine.HPtoHX(Point.X, Point.Y, ref EndX, ref EndY);
            {
                I = 0;
                int end__2597 = Path.Count - 1;
                for (; I <= end__2597; I += 1)
                {
                    var call__2598 = Path[I];
                    Point = call__2598;
                    Aow.Units.HSEngine.HSEngine.HPtoHX(Point.X, Point.Y, ref X, ref Y);
                    if (((X != StartX) || (Y != StartY)) && ((X != EndX) || (Y != EndY)))
                    {
                        Steps = 0;
                        var call__2599 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__2600 = call__2599.Field[X, Y];
                        if (call__2600.TerrainType == 15)
                        {
                            Steps = 3;
                        }
                        else
                        {
                            var call__2601 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__2602 = call__2601.Field[X, Y];
                            if (call__2602.TerrainType == 7)
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
                                    var call__2603 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__2604 = call__2603.Field[X, Y];
                                    var call__2605 = call__2604.FindOwnedChild(2228484);
                                    UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__2605;
                                }

                                var call__2606 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__2607 = call__2606.Field[X, Y];
                                var call__2608 = call__2607.FindOwnedChild(2229508);
                                Wall = (Aow.Units.CityWall.TCityWall)call__2608;
                                if ((Wall == null) && (Side == Aow.Units.AoWE.TCombatSideIndex.csAttacker))
                                {
                                    var call__2609 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__2610 = call__2609.Field[X, Y];
                                    var call__2611 = call__2610.FindOwnedChild(2229510);
                                    Wall = (Aow.Units.CityWall.TCityWall)call__2611;
                                }

                                if (A == 1)
                                {
                                    bool bool__2612 = (UnitHS != null);
                                    if (bool__2612)
                                    {
                                        var call__2613 = UnitHS.FUnit.GetSide();
                                        bool__2612 = (call__2613 != Side);
                                    }

                                    if (bool__2612)
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
                                            var call__2614 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                            var call__2615 = call__2614.Field[X, Y];
                                            if (call__2615.OverlayType == 8)
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
                                                        var call__2616 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                        var call__2617 = call__2616.Field[NX, NY];
                                                        var call__2618 = call__2617.FindOwnedChild(2229508);
                                                        Wall = (Aow.Units.CityWall.TCityWall)call__2618;
                                                        if (Wall == null)
                                                        {
                                                            var call__2619 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                            var call__2620 = call__2619.Field[NX, NY];
                                                            var call__2621 = call__2620.FindOwnedChild(2229510);
                                                            Wall = (Aow.Units.CityWall.TCityWall)call__2621;
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
                                    var call__2622 = Obstacles[Obstacles.Count - 1];
                                    Obstacle = (Aow.Units.AoWTC.TROb)call__2622;
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
