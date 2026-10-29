// Translated from AoWTC.pas.
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
    }

    // { $04, $08 }
    // { TROb (instance size 12, VMT 00411AD0): $04 PathIndex; $08 Chance. }
    public partial class TROb : Aow.Rtl.TObject
    {
        public int PathIndex;
        // { $04 }
        public int Chance;
    }

    // { $08 }
    // { TAoWNewCombatMapSettings (instance size 32, VMT 00411B34)
//     Inherited $04 Width; $08 Height; $0C Levels; $10 HSSFileName; $14 HSMapClassID; $1C MapLevelClassID. }
    public partial class TAoWNewCombatMapSettings : Aow.Units.HSEngine.TNewHSMapSettings
    {
        // Legacy factory runs the split base body after raw allocation.
        public TAoWNewCombatMapSettings() : base(true) { }

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
        public static Aow.Units.AoWTC.TAoWNewCombatMapSettings Create()
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
        public TCAI()
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
            M = new Aow.Units.AoWTC.TaiMove();
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
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Eval Movepath", Aow.Rtl.Rtl.NullTerminatedText(((AnsiString)"Error - TCAI ") + call__46), (int)0);
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
                    Path = new Aow.Units.aowFX.TXYZList();
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
            Path = new Aow.Units.AoWE.TMovepath();
            BasePath = new Aow.Units.AoWE.TMovepath();
            DetourPath = new Aow.Units.AoWE.TMovepath();
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
                        Round = new Aow.Units.AoWE.TMeleeRound();
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
                                    Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Spell Check", Aow.Rtl.Rtl.NullTerminatedText(((((AnsiString)"Error - TCAI ") + Aow.Rtl.Rtl.IntToStr(Ability)) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Spell)), (int)0);
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
                                                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Touch Check", Aow.Rtl.Rtl.NullTerminatedText(((((AnsiString)"Error - TCAI ") + Aow.Rtl.Rtl.IntToStr(Ability)) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Spell)), (int)0);
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
                                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Ranged Check", Aow.Rtl.Rtl.NullTerminatedText(((((AnsiString)"Error - TCAI ") + Aow.Rtl.Rtl.IntToStr(Ability)) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(Spell)), (int)0);
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
                            var tacticalCombat = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat;
                            var call__482 = tacticalCombat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)unchecked((sbyte)tacticalCombat.FCombatData.FCurrentPlayer)));
                            FSide = (Aow.Units.AoWE.TCombatSideIndex)call__482.FSideIndex;
                            FNormalUnits = 0;
                            {
                                I = 0;
                                int end__483 = tacticalCombat.FCombatData.GetCount() - 1;
                                for (; I <= end__483; I += 1)
                                {
                                    var call__484 = tacticalCombat.FCombatData.GetObjects(I);
                                    bool bool__485 = (call__484 != null);
                                    if (bool__485)
                                    {
                                        var call__486 = tacticalCombat.FCombatData.GetObjects(I);
                                        bool__485 = (call__486 is Aow.Units.AoWE.TCombatUnit);
                                    }

                                    bool bool__487 = bool__485;
                                    if (bool__487)
                                    {
                                        var call__488 = tacticalCombat.FCombatData.GetObjects(I);
                                        var call__489 = call__488.GetEnabled();
                                        bool__487 = call__489;
                                    }

                                    if (bool__487)
                                    {
                                        Visible = false;
                                        J = 0;
                                        while (!Visible && (J < 19))
                                        {
                                            var call__490 = tacticalCombat.FCombatData.GetObjects(I);
                                            var call__491 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__490).FUnitHS.GetYhx();
                                            var call__492 = tacticalCombat.FCombatData.GetObjects(I);
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
                                            var call__497 = tacticalCombat.FCombatData.GetObjects(I);
                                            var call__498 = call__497.GetSide();
                                            if (call__498 == FSide)
                                            {
                                                var call__499 = tacticalCombat.FCombatData.GetObjects(I);
                                                if (unchecked((byte)call__499.FCombatPlayer) == tacticalCombat.FCombatData.FCurrentPlayer)
                                                {
                                                    var call__500 = tacticalCombat.FCombatData.GetObjects(I);
                                                    var call__501 = call__500.GetLocked();
                                                    if (!call__501)
                                                    {
                                                        var call__502 = tacticalCombat.FCombatData.GetObjects(I);
                                                        if (((Aow.Units.AoWTC.TTacticalCombatUnit)call__502).FUnit is Aow.Units.AoWE.THero)
                                                        {
                                                            var call__503 = tacticalCombat.FCombatData.GetObjects(I);
                                                            Units.Add(call__503);
                                                        }
                                                        else
                                                        {
                                                            FNormalUnits += 1;
                                                            var call__504 = tacticalCombat.FCombatData.GetObjects(I);
                                                            Units.Insert(0, call__504);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    var call__505 = tacticalCombat.FCombatData.GetObjects(I);
                                                    Allies.Add(call__505);
                                                }
                                            }
                                            else
                                            {
                                                var call__506 = tacticalCombat.FCombatData.GetObjects(I);
                                                Enemies.Add(call__506);
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
                                                                        var call__558 = new Aow.Units.AoWTC.TTCAbility();
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
                                                                var call__562 = new Aow.Units.AoWTC.TTCAbility();
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
                                        Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Eval Enemies", Aow.Rtl.Rtl.NullTerminatedText(((AnsiString)"Error - TCAI ") + call__548), (int)0);
                                    }

                                    Settings = new Aow.Units.AoWTC.TCombatMoveControlSettings(null);
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
                                                                        var call__587 = new Aow.Units.AoWTC.TTCAbility();
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
                                                                var call__591 = new Aow.Units.AoWTC.TTCAbility();
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
                                        Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during Eval Friends", Aow.Rtl.Rtl.NullTerminatedText(((AnsiString)"Error - TCAI ") + call__577), (int)0);
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
                                                                        var call__619 = new Aow.Units.AoWTC.TTCAbility();
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
                                                            var call__623 = new Aow.Units.AoWTC.TTCAbility();
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
                                        Settings = new Aow.Units.AoWTC.TCombatMoveControlSettings(null);
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
                                            Settings = new Aow.Units.AoWTC.TCombatMoveControlSettings(null);
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
                                        var tacticalCombatUnitHS = Move.UnitHS;
                                        if (((Move.UnitHS.FUnit.FUnit is Aow.Units.AoWE.THero) || ((FSide == Aow.Units.AoWE.TCombatSideIndex.csAttacker) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FRound == 1))) && (Move.UnitHS.FUnit.FMovePoints > FMinMoves))
                                        {
                                            Move.UnitHS.FUnit.FMovePoints = FMinMoves;
                                        }

                                        Settings = new Aow.Units.AoWTC.TCombatMoveControlSettings(null);
                                        var call__703 = tacticalCombatUnitHS.GetLhx();
                                        var call__704 = tacticalCombatUnitHS.GetYhx();
                                        var call__705 = tacticalCombatUnitHS.GetXhx();
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
                                        Settings = new Aow.Units.AoWTC.TCombatMoveControlSettings(null);
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
                                        Settings = new Aow.Units.AoWTC.TCombatMoveControlSettings(null);
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
            var call__824 = new Aow.Units.aowFX.TXYZList();
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
                    int end__825 = Count - 1;
                    for (; N <= end__825; N += 1)
                    {
                        var call__826 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxCloudPuff);
                        Effect = call__826;
                        Effect.FAbilityIndex = FAbilityIndex;
                        Effect.FSpellIndex = FSpellIndex;
                        var call__827 = new Aow.Units.aowFX.TXYZ();
                        Effect.FPath.Add(call__827);
                        J = Aow.Rtl.Rtl.Random(70) + 10;
                        var call__828 = System.Math.PI;
                        var call__829 = System.Math.Cos(((double)(((double)J) * call__828)) / 180);
                        var call__830 = Aow.Rtl.Rtl.Random(Radius);
                        var call__831 = FPath[0];
                        var call__832 = Effect.FPath[0];
                        call__832.X = call__831.X + Aow.Rtl.Rtl.Round(((double)call__829) * (call__830 - (Radius / 2)));
                        var call__833 = System.Math.PI;
                        var call__834 = System.Math.Sin(((double)(((double)J) * call__833)) / 180);
                        var call__835 = Aow.Rtl.Rtl.Random(Radius);
                        var call__836 = FPath[0];
                        var call__837 = Effect.FPath[0];
                        call__837.Y = call__836.Y + (Aow.Rtl.Rtl.Round(((double)call__834) * (call__835 - (Radius / 2))) / 2);
                    }
                }
            }

            void MakeImpact()
            {
                Effect = null;
                var call__838 = FPath[FFrame];
                var call__839 = FPath[FFrame];
                Aow.Units.HSEngine.HSEngine.HPtoHX(call__839.X, call__838.Y, ref I, ref J);
                var call__840 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__841 = call__840.Field[I, J];
                if (call__841.TerrainType == 0)
                {
                    var call__842 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxHitWater);
                    Effect = call__842;
                }
                else
                {
                    var call__843 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__844 = call__843.Field[I, J];
                    bool bool__845 = (call__844.OverlayType != 8);
                    if (bool__845)
                    {
                        var call__846 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__847 = call__846.Field[I, J];
                        var call__848 = call__847.FindOwnedChild(2229508);
                        bool__845 = (call__848 == null);
                    }

                    bool bool__849 = bool__845;
                    if (bool__849)
                    {
                        var call__850 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__851 = call__850.Field[I, J];
                        var call__852 = call__851.FindOwnedChild(2229510);
                        bool__849 = (call__852 == null);
                    }

                    if (bool__849)
                    {
                        var call__853 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxHitGround);
                        Effect = call__853;
                    }
                }

                if (Effect != null)
                {
                    var call__854 = new Aow.Units.aowFX.TXYZ();
                    Effect.FPath.Add(call__854);
                    var call__855 = FPath[FFrame];
                    var call__856 = Effect.FPath[0];
                    call__856.X = call__855.X;
                    var call__857 = FPath[FFrame];
                    var call__858 = Effect.FPath[0];
                    call__858.Y = call__857.Y;
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
                                var call__859 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                call__859.FCastSounds.Play(0);
                            }

                            var call__860 = FPath.Count;
                            if (FFrame >= (call__860 - 1))
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        var call__861 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__861.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__862 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__862.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__863 = FPath.Count;
                                FFrame = call__863 - 1;
                                FDone = true;
                                if (FTarget == null)
                                {
                                    MakeImpact();
                                }

                                var call__864 = FPath[FFrame];
                                var call__865 = FPath[FFrame];
                                var call__866 = FPath[FFrame];
                                Aow.Units.HSEngine.HSEngine.HPtoHX(call__866.X, call__864.Y - call__865.Z, ref I, ref J);
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(I, J, 0, 2);
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxDecayComet:
                            if (((FFrame == 0) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds) && (FAbilityIndex == 52))
                            {
                                var call__867 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                call__867.FCastSounds.Play(0);
                            }

                            var call__868 = FPath.Count;
                            if (FFrame >= (call__868 - 1))
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        var call__869 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__869.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__870 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__870.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__871 = FPath.Count;
                                FFrame = call__871 - 1;
                                FDone = true;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxAltArrow:
                            if (((FFrame == 0) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds) && (FAbilityIndex == 52))
                            {
                                var call__872 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                call__872.FCastSounds.Play(0);
                            }

                            FFrame += 3;
                            var call__873 = FPath.Count;
                            if (FFrame >= (call__873 - 1))
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        var call__874 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__874.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__875 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__875.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__876 = FPath.Count;
                                FFrame = call__876 - 1;
                                FDone = true;
                                if (FTarget == null)
                                {
                                    MakeImpact();
                                }

                                var call__877 = FPath[FFrame];
                                var call__878 = FPath[FFrame];
                                var call__879 = FPath[FFrame];
                                Aow.Units.HSEngine.HSEngine.HPtoHX(call__879.X, call__877.Y - call__878.Z, ref I, ref J);
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(I, J, 0, 2);
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxScreenNS:
                            FFrame += 1;
                            var call__880 = FPath.Count;
                            if (FFrame >= (call__880 - 1))
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        var call__881 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__881.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__882 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__882.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__883 = FPath.Count;
                                FFrame = call__883 - 1;
                                FDone = true;
                                var call__884 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxHailImpact);
                                Effect = call__884;
                                Effect.FAbilityIndex = FAbilityIndex;
                                Effect.FSpellIndex = FSpellIndex;
                                var call__885 = new Aow.Units.aowFX.TXYZ();
                                Effect.FPath.Add(call__885);
                                var call__886 = FPath[FFrame];
                                var call__887 = Effect.FPath[0];
                                call__887.X = call__886.X;
                                var call__888 = FPath[FFrame];
                                var call__889 = Effect.FPath[0];
                                call__889.Y = call__888.Y;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxHailImpact:
                            if (FAbilityIndex == 52)
                            {
                                var call__890 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                var call__891 = call__890.FCastImages.ImageSequences[2];
                                Sequence = call__891;
                            }
                            else
                            {
                                var call__892 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                var call__893 = call__892.FImageSequences.ImageSequences[2];
                                Sequence = call__893;
                            }

                            if (Sequence != null)
                            {
                                var call__894 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= (call__894 - 1))
                                {
                                    var call__895 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__895 - 1;
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
                                var call__896 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                var call__897 = call__896.FCastImages.ImageSequences[1];
                                Sequence = call__897;
                            }
                            else
                            {
                                var call__898 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                var call__899 = call__898.FImageSequences.ImageSequences[1];
                                Sequence = call__899;
                            }

                            if (Sequence != null)
                            {
                                var call__900 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= (call__900 - 1))
                                {
                                    var call__901 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__901 - 1;
                                    FDone = true;
                                }
                            }
                            else
                            {
                                FFrame = -1;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxSFOverlay:
                            var call__902 = FPath.Count;
                            if (FFrame >= (call__902 - 1))
                            {
                                var call__903 = FPath.Count;
                                FFrame = call__903 - 1;
                                FDone = true;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxScreenSwarm:
                            if (FFrame == 0)
                            {
                                FFrame = 1;
                            }

                            var call__904 = FPath.Count;
                            if (FFrame >= (call__904 - 1))
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if ((FTarget != null) && (FAbilityIndex == 52))
                                    {
                                        var call__905 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__905.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__906 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__906.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__907 = FPath.Count;
                                FFrame = call__907 - 1;
                                FDone = true;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxRay:
                            FFrame += 12;
                            var call__908 = FPath.Count;
                            if (FFrame >= (call__908 - 1))
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        var call__909 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__909.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__910 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__910.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__911 = FPath.Count;
                                FFrame = call__911 - 1;
                                FDone = true;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxFrostBeam:
                            switch (FFrame)
                            {
                                case >= 0 and <= 999:
                                    FFrame += 1;
                                    var call__912 = FPath.Count;
                                    if (FFrame >= (call__912 - 1))
                                    {
                                        FFrame = 999;
                                    }

                                    J = Aow.Rtl.Rtl.Random(5) + 5;
                                    Aow.Units.HSEngine.HSEngine.HXtoHP(FTargetX, FTargetY, ref K, ref X);
                                {
                                    I = 0;
                                    int end__913 = J;
                                    for (; I <= end__913; I += 1)
                                    {
                                        var call__914 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__914;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__915 = Aow.Rtl.Rtl.Random(100);
                                        var call__916 = Aow.Rtl.Rtl.Random(5);
                                        var call__917 = Aow.Rtl.Rtl.Random(100);
                                        Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)(K - 200)), unchecked((short)(X - 200)), unchecked((short)((call__917 + K) - 50)), unchecked((short)((call__915 + X) - 50)), unchecked((short)(call__916 + 5)), 100, ref Effect.FPath);
                                    }
                                }

                                    J = Aow.Rtl.Rtl.Random(10) + 10;
                                {
                                    I = 0;
                                    int end__918 = J;
                                    for (; I <= end__918; I += 1)
                                    {
                                        var call__919 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__919;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 20)) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)(((double)Aow.Rtl.Rtl.Random(10)) / 10);
                                        Z = 100;
                                        var call__920 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(K - 200, X - 100, call__920, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case >= 1000 and <= 1029:
                                    Aow.Units.HSEngine.HSEngine.HXtoHP(FTargetX, FTargetY, ref K, ref X);
                                    J = Aow.Rtl.Rtl.Random(5) + 5;
                                {
                                    I = 0;
                                    int end__921 = J;
                                    for (; I <= end__921; I += 1)
                                    {
                                        var call__922 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__922;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__923 = Aow.Rtl.Rtl.Random(100);
                                        var call__924 = Aow.Rtl.Rtl.Random(5);
                                        var call__925 = Aow.Rtl.Rtl.Random(100);
                                        Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)(K - 200)), unchecked((short)(X - 200)), unchecked((short)((call__925 + K) - 50)), unchecked((short)((call__923 + X) - 50)), unchecked((short)(call__924 + 5)), 100, ref Effect.FPath);
                                    }
                                }

                                    J = Aow.Rtl.Rtl.Random(10) + 10;
                                {
                                    I = 0;
                                    int end__926 = J;
                                    for (; I <= end__926; I += 1)
                                    {
                                        var call__927 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__927;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 20)) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)(((double)Aow.Rtl.Rtl.Random(10)) / 10);
                                        Z = 100;
                                        var call__928 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(K - 200, X - 100, call__928, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    J = Aow.Rtl.Rtl.Random(5) + 5;
                                {
                                    I = 0;
                                    int end__929 = J;
                                    for (; I <= end__929; I += 1)
                                    {
                                        var call__930 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__930;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(20) + 20)) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 20)) / 10);
                                        Z = 0;
                                        var call__931 = FPath.Count;
                                        var call__932 = FPath[call__931 - 1];
                                        var call__933 = FPath.Count;
                                        var call__934 = FPath[call__933 - 1];
                                        var call__935 = FPath.Count;
                                        var call__936 = FPath[call__935 - 1];
                                        var call__937 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(call__936.X, call__932.Y - call__934.Z, call__937, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case 1030:
                                    FFrame = 1999;
                                    break;
                                case >= 2000 and <= 2999:
                                    FFrame += 1;
                                    var call__938 = FPath.Count;
                                    if (FFrame >= ((call__938 + 2000) - 1))
                                    {
                                        FFrame = 2999;
                                    }

                                    J = Aow.Rtl.Rtl.Random(5) + 5;
                                {
                                    I = 0;
                                    int end__939 = J;
                                    for (; I <= end__939; I += 1)
                                    {
                                        var call__940 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__940;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(20) + 20)) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 20)) / 10);
                                        Z = 0;
                                        var call__941 = FPath.Count;
                                        var call__942 = FPath[call__941 - 1];
                                        var call__943 = FPath.Count;
                                        var call__944 = FPath[call__943 - 1];
                                        var call__945 = FPath.Count;
                                        var call__946 = FPath[call__945 - 1];
                                        var call__947 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(call__946.X, call__942.Y - call__944.Z, call__947, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case 3000:
                                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            var call__948 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__948.FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            var call__949 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__949.FSFXLibrary.Play(1);
                                        }
                                    }

                                {
                                    I = 0;
                                    int end__950 = FPath.Count - 1;
                                    for (; I <= end__950; I += 1)
                                    {
                                        var call__951 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxFrost);
                                        Effect = call__951;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(25) + 25)) / 10);
                                        Gravity = (float)0.5;
                                        VZ = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 40)) / 10);
                                        Z = 0;
                                        var call__952 = FPath[I];
                                        var call__953 = FPath[I];
                                        var call__954 = Aow.Rtl.Rtl.Random(360);
                                        var call__955 = FPath[I];
                                        Aow.Units.aowFX.aowFX.MakeArcPath(call__955.X, call__952.Y - call__953.Z, call__954, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    J = Aow.Rtl.Rtl.Random(10) + 100;
                                {
                                    I = 0;
                                    int end__956 = J;
                                    for (; I <= end__956; I += 1)
                                    {
                                        var call__957 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxFrost);
                                        Effect = call__957;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 50)) / 10);
                                        Gravity = (float)0.1;
                                        VZ = (float)(((double)(Aow.Rtl.Rtl.Random(5) + 5)) / 10);
                                        Z = 0;
                                        var call__958 = FPath.Count;
                                        var call__959 = FPath[call__958 - 1];
                                        var call__960 = FPath.Count;
                                        var call__961 = FPath[call__960 - 1];
                                        var call__962 = FPath.Count;
                                        var call__963 = FPath[call__962 - 1];
                                        var call__964 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(call__963.X, call__959.Y - call__961.Z, call__964, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
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
                                            var call__965 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__965.FCastSounds.Play(0);
                                        }
                                        else
                                        {
                                            var call__966 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__966.FSFXLibrary.Play(0);
                                        }
                                    }

                                {
                                    I = 1;
                                    byte end__967 = 20;
                                    for (; I <= end__967; I += 1)
                                    {
                                        var call__968 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__968;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__969 = Aow.Rtl.Rtl.Random(360);
                                        var call__970 = FPath[0];
                                        var call__971 = FPath[0];
                                        var call__972 = Aow.Rtl.Rtl.Random(2);
                                        var call__973 = Aow.Rtl.Rtl.Random(10);
                                        var call__974 = Aow.Rtl.Rtl.Random(10);
                                        Aow.Units.aowFX.aowFX.MakeSwarmPath(call__974 + 10, call__973 + (FFrame / 4), ((I % 6) * 60) + FFrame, call__969, call__970.X, call__971.Y, (Aow.Units.aowFX.TSwarmDirection)call__972, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case >= 30 and <= 59:
                                {
                                    I = 1;
                                    byte end__975 = 20;
                                    for (; I <= end__975; I += 1)
                                    {
                                        var call__976 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__976;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__977 = Aow.Rtl.Rtl.Random(360);
                                        var call__978 = FPath[0];
                                        var call__979 = FPath[0];
                                        var call__980 = Aow.Rtl.Rtl.Random(2);
                                        var call__981 = Aow.Rtl.Rtl.Random(10);
                                        var call__982 = Aow.Rtl.Rtl.Random(10);
                                        Aow.Units.aowFX.aowFX.MakeSwarmPath(call__982 + 10, call__981 + ((60 - FFrame) / 4), ((I % 6) * 60) + FFrame, call__977, call__978.X, call__979.Y, (Aow.Units.aowFX.TSwarmDirection)call__980, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case >= 60 and <= 89:
                                {
                                    I = 1;
                                    byte end__983 = 20;
                                    for (; I <= end__983; I += 1)
                                    {
                                        var call__984 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__984;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__985 = Aow.Rtl.Rtl.Random(360);
                                        var call__986 = FPath[0];
                                        var call__987 = FPath[0];
                                        var call__988 = Aow.Rtl.Rtl.Random(2);
                                        var call__989 = Aow.Rtl.Rtl.Random(10);
                                        var call__990 = Aow.Rtl.Rtl.Random(10);
                                        Aow.Units.aowFX.aowFX.MakeSwarmPath(call__990 + 10, call__989 + ((FFrame - 60) / 2), ((I % 6) * 60) + FFrame, call__985, call__986.X, call__987.Y, (Aow.Units.aowFX.TSwarmDirection)call__988, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case >= 90 and <= 119:
                                {
                                    I = 1;
                                    byte end__991 = 20;
                                    for (; I <= end__991; I += 1)
                                    {
                                        var call__992 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__992;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__993 = Aow.Rtl.Rtl.Random(360);
                                        var call__994 = FPath[0];
                                        var call__995 = FPath[0];
                                        var call__996 = Aow.Rtl.Rtl.Random(2);
                                        var call__997 = Aow.Rtl.Rtl.Random(10);
                                        var call__998 = Aow.Rtl.Rtl.Random(10);
                                        Aow.Units.aowFX.aowFX.MakeSwarmPath(call__998 + 10, call__997 + ((120 - FFrame) / 2), ((I % 6) * 60) + FFrame, call__993, call__994.X, call__995.Y, (Aow.Units.aowFX.TSwarmDirection)call__996, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case 150:
                                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            var call__999 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__999.FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            var call__1000 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__1000.FSFXLibrary.Play(1);
                                        }
                                    }

                                    X = 200;
                                    Y = 200;
                                    J = Aow.Rtl.Rtl.Random(5);
                                {
                                    I = 0;
                                    short end__1001 = 359;
                                    for (; I <= end__1001; I += 1)
                                    {
                                        var call__1002 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1002;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        X = -X;
                                        var call__1003 = System.Math.PI;
                                        var call__1004 = System.Math.Sin(((double)(((double)(I + J)) * call__1003)) / 180);
                                        var call__1005 = Aow.Rtl.Rtl.Random(10);
                                        var call__1006 = FPath[0];
                                        var call__1007 = System.Math.PI;
                                        var call__1008 = System.Math.Cos(((double)(((double)(I + J)) * call__1007)) / 180);
                                        var call__1009 = Aow.Rtl.Rtl.Random(10);
                                        var call__1010 = FPath[0];
                                        var call__1011 = FPath[0];
                                        var call__1012 = FPath[0];
                                        Aow.Units.aowFX.aowFX.MakeLinePath(call__1012.X, call__1011.Y, call__1010.X + Aow.Rtl.Rtl.Round(((double)call__1008) * (call__1009 + X)), call__1006.Y + (Aow.Rtl.Rtl.Round(((double)call__1004) * (call__1005 + Y)) / 2), 75, ref Effect.FPath);
                                        if ((I % 10) == 0)
                                        {
                                            {
                                                K = 1;
                                                byte end__1013 = 10;
                                                for (; K <= end__1013; K += 1)
                                                {
                                                    var call__1014 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                                    Effect2 = call__1014;
                                                    Effect2.FAbilityIndex = FAbilityIndex;
                                                    Effect2.FSpellIndex = FSpellIndex;
                                                    var call__1015 = Effect.FPath.Count;
                                                    var call__1016 = Effect.FPath[call__1015 - 1];
                                                    var call__1017 = Effect.FPath.Count;
                                                    var call__1018 = Effect.FPath[call__1017 - 1];
                                                    var call__1019 = FPath[0];
                                                    var call__1020 = FPath[0];
                                                    Aow.Units.aowFX.aowFX.MakeLinePath(call__1020.X, call__1019.Y, call__1018.X, call__1016.Y, (K * 5) + 75, ref Effect2.FPath);
                                                }
                                            }
                                        }
                                    }
                                }

                                    break;
                                case >= 151 and <= 169:
                                case >= 210 and <= 229:
                                    var call__1021 = FPath[0];
                                    var call__1022 = FPath[0];
                                    Aow.Units.HSEngine.HSEngine.HPtoHX(call__1022.X, call__1021.Y, ref X, ref Y);
                                    Msg.MsgID = 200720;
                                    Msg.Param[0] = 1;
                                    Msg.Result = -1;
                                {
                                    I = 1;
                                    byte end__1023 = 36;
                                    for (; I <= end__1023; I += 1)
                                    {
                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                                        var call__1024 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__1025 = call__1024.Field[J, K];
                                        call__1025.SendMapFieldMsg(ref Msg);
                                    }
                                }

                                    break;
                                case >= 170 and <= 184:
                                case >= 195 and <= 209:
                                    var call__1026 = FPath[0];
                                    var call__1027 = FPath[0];
                                    Aow.Units.HSEngine.HSEngine.HPtoHX(call__1027.X, call__1026.Y, ref X, ref Y);
                                    Msg.MsgID = 200720;
                                    Msg.Param[0] = 2;
                                    Msg.Result = -1;
                                {
                                    I = 1;
                                    byte end__1028 = 36;
                                    for (; I <= end__1028; I += 1)
                                    {
                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                                        var call__1029 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__1030 = call__1029.Field[J, K];
                                        call__1030.SendMapFieldMsg(ref Msg);
                                    }
                                }

                                    break;
                                case >= 185 and <= 194:
                                    var call__1031 = FPath[0];
                                    var call__1032 = FPath[0];
                                    Aow.Units.HSEngine.HSEngine.HPtoHX(call__1032.X, call__1031.Y, ref X, ref Y);
                                    Msg.MsgID = 200720;
                                    Msg.Param[0] = 3;
                                    Msg.Result = -1;
                                {
                                    I = 1;
                                    byte end__1033 = 36;
                                    for (; I <= end__1033; I += 1)
                                    {
                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                                        var call__1034 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__1035 = call__1034.Field[J, K];
                                        call__1035.SendMapFieldMsg(ref Msg);
                                    }
                                }

                                    break;
                                case 230:
                                    var call__1036 = FPath[0];
                                    var call__1037 = FPath[0];
                                    Aow.Units.HSEngine.HSEngine.HPtoHX(call__1037.X, call__1036.Y, ref X, ref Y);
                                    Msg.MsgID = 200721;
                                    Msg.Result = -1;
                                {
                                    I = 1;
                                    byte end__1038 = 36;
                                    for (; I <= end__1038; I += 1)
                                    {
                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                                        var call__1039 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__1040 = call__1039.Field[J, K];
                                        call__1040.SendMapFieldMsg(ref Msg);
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
                                    var call__1041 = FPath.Count;
                                    if (FFrame >= (call__1041 + 20))
                                    {
                                        bool bool__1042 = (FTarget != null);
                                        if (bool__1042)
                                        {
                                            var call__1043 = FTarget.GetEnabled();
                                            bool__1042 = call__1043;
                                        }

                                        if (bool__1042)
                                        {
                                            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                            {
                                                if (FAbilityIndex == 52)
                                                {
                                                    var call__1044 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                                    call__1044.FCastSounds.Play(1);
                                                }
                                                else
                                                {
                                                    var call__1045 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                                    call__1045.FSFXLibrary.Play(1);
                                                }
                                            }

                                            var call__1046 = FPath.Count;
                                            var call__1047 = FPath[call__1046 - 1];
                                            var call__1048 = FPath.Count;
                                            var call__1049 = FPath[call__1048 - 1];
                                            Aow.Units.HSEngine.HSEngine.HPtoHX(call__1049.X, call__1047.Y, ref J, ref K);
                                            var call__1050 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                            var call__1051 = call__1050.Field[J, K];
                                            ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.MoveTo(call__1051, 0);
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
                                                var call__1052 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                                call__1052.FCastSounds.Play(0);
                                            }
                                            else
                                            {
                                                var call__1053 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                                call__1053.FSFXLibrary.Play(0);
                                            }
                                        }

                                        bool bool__1054 = (FTarget != null);
                                        if (bool__1054)
                                        {
                                            var call__1055 = FTarget.GetEnabled();
                                            bool__1054 = call__1055;
                                        }

                                        if (bool__1054)
                                        {
                                            var call__1056 = FPath.Count;
                                            if (((FFrame - 20) < call__1056) && (FFrame >= 20))
                                            {
                                                var call__1057 = FPath[FFrame - 20];
                                                var call__1058 = FPath[0];
                                                ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FOffsetX = call__1057.X - call__1058.X;
                                                var call__1059 = FPath[FFrame - 20];
                                                var call__1060 = FPath[FFrame - 20];
                                                var call__1061 = FPath[0];
                                                ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FOffsetY = (call__1059.Y - call__1060.Z) - call__1061.Y;
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
                                            int end__1062 = FFrame;
                                            for (; I <= end__1062; I += 1)
                                            {
                                                bool bool__1063 = (I >= 0);
                                                if (bool__1063)
                                                {
                                                    var call__1064 = FPath.Count;
                                                    bool__1063 = (I < call__1064);
                                                }

                                                if (bool__1063)
                                                {
                                                    var call__1065 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                                    Effect = call__1065;
                                                    Effect.FAbilityIndex = FAbilityIndex;
                                                    Effect.FSpellIndex = FSpellIndex;
                                                    var call__1066 = Aow.Rtl.Rtl.Random(360);
                                                    var call__1067 = FPath[I];
                                                    var call__1068 = Aow.Rtl.Rtl.Random(10);
                                                    var call__1069 = FPath[I];
                                                    var call__1070 = FPath[I];
                                                    var call__1071 = Aow.Rtl.Rtl.Random(10);
                                                    var call__1072 = Aow.Rtl.Rtl.Random(2);
                                                    var call__1073 = FPath.Count;
                                                    var call__1074 = Aow.Rtl.Rtl.Random(5);
                                                    Aow.Units.aowFX.aowFX.MakeSwarmPath(call__1074 + 20, (call__1073 - I) / 5, 0, call__1066, (call__1067.X + call__1068) - 5, ((call__1069.Y - call__1070.Z) + call__1071) - 5, (Aow.Units.aowFX.TSwarmDirection)call__1072, ref Effect.FPath);
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
                                            var call__1075 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__1075.FCastSounds.Play(0);
                                        }
                                        else
                                        {
                                            var call__1076 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__1076.FSFXLibrary.Play(0);
                                        }
                                    }

                                    J = Aow.Rtl.Rtl.Random(6) + 10;
                                    var call__1077 = ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.GetYhx();
                                    var call__1078 = ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.GetXhx();
                                    Aow.Units.HSEngine.HSEngine.HXtoHP(call__1078, call__1077, ref K, ref X);
                                {
                                    I = 1;
                                    int end__1079 = J - 1;
                                    for (; I <= end__1079; I += 1)
                                    {
                                        var call__1080 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxFrost);
                                        Effect = call__1080;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(25) + 25)) / 10);
                                        Gravity = (float)0.5;
                                        var call__1081 = Aow.Rtl.Rtl.Random(50);
                                        VZ = (float)(((double)(call__1081 + FFrame)) / 10);
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
                                                    var call__1082 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                                    call__1082.FCastSounds.Play(1);
                                                }
                                                else
                                                {
                                                    var call__1083 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                                    call__1083.FSFXLibrary.Play(1);
                                                }
                                            }

                                            J = Aow.Rtl.Rtl.Random(10) + 100;
                                        {
                                            I = 0;
                                            int end__1084 = J;
                                            for (; I <= end__1084; I += 1)
                                            {
                                                var call__1085 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxFrost);
                                                Effect = call__1085;
                                                Effect.FAbilityIndex = FAbilityIndex;
                                                Effect.FSpellIndex = FSpellIndex;
                                                Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 50)) / 10);
                                                Gravity = (float)0.1;
                                                VZ = (float)(((double)(Aow.Rtl.Rtl.Random(5) + 5)) / 10);
                                                Z = 0;
                                                var call__1086 = Aow.Rtl.Rtl.Random(360);
                                                Aow.Units.aowFX.aowFX.MakeArcPath(K + 16, X + 16, call__1086, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                            }
                                        }

                                            break;
                                    }

                                    break;
                                case >= 125 and <= 199:
                                    if (FFrame == 125)
                                    {
                                        var call__1087 = FPath[0];
                                        var call__1088 = FPath[0];
                                        Aow.Units.HSEngine.HSEngine.HPtoHX(call__1088.X, call__1087.Y, ref I, ref J);
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.CenterViewTo(I, J - 3, 0);
                                    }

                                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            var call__1089 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__1089.FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            var call__1090 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__1090.FSFXLibrary.Play(1);
                                        }
                                    }

                                    J = Aow.Rtl.Rtl.Random(6) + 10;
                                    var call__1091 = FPath[0];
                                    var call__1092 = Aow.Rtl.Rtl.Random(160);
                                    K = (call__1091.X + call__1092) - 80;
                                    var call__1093 = FPath[0];
                                    var call__1094 = Aow.Rtl.Rtl.Random(80);
                                    X = (call__1093.Y + call__1094) - 40;
                                {
                                    I = 1;
                                    int end__1095 = J - 1;
                                    for (; I <= end__1095; I += 1)
                                    {
                                        var call__1096 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxFrost);
                                        Effect = call__1096;
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
                                    int end__1097 = J;
                                    for (; I <= end__1097; I += 1)
                                    {
                                        var call__1098 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxFrost);
                                        Effect = call__1098;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 50)) / 10);
                                        Gravity = (float)0.1;
                                        VZ = (float)(((double)(Aow.Rtl.Rtl.Random(5) + 5)) / 10);
                                        Z = 0;
                                        var call__1099 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(K, X, call__1099, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
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
                                            int end__1100 = FFrame;
                                            for (; I <= end__1100; I += 1)
                                            {
                                                {
                                                    J = 0;
                                                    int end__1101 = I / 25;
                                                    for (; J <= end__1101; J += 1)
                                                    {
                                                        var call__1102 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                                        Effect = call__1102;
                                                        Effect.FAbilityIndex = FAbilityIndex;
                                                        Effect.FSpellIndex = FSpellIndex;
                                                        var call__1103 = Aow.Rtl.Rtl.Random(360);
                                                        var call__1104 = FPath[0];
                                                        var call__1105 = FPath[0];
                                                        var call__1106 = Aow.Rtl.Rtl.Random(2);
                                                        var call__1107 = Aow.Rtl.Rtl.Random(10);
                                                        Aow.Units.aowFX.aowFX.MakeSwarmPath(call__1107 + 10, I / 2, 45, call__1103, (call__1104.X + 100) - I, ((call__1105.Y - 200) - 50) + I, (Aow.Units.aowFX.TSwarmDirection)call__1106, ref Effect.FPath);
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
                                            byte end__1108 = 50;
                                            for (; I <= end__1108; I += 1)
                                            {
                                                {
                                                    J = 0;
                                                    int end__1109 = I / 25;
                                                    for (; J <= end__1109; J += 1)
                                                    {
                                                        var call__1110 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                                        Effect = call__1110;
                                                        Effect.FAbilityIndex = FAbilityIndex;
                                                        Effect.FSpellIndex = FSpellIndex;
                                                        var call__1111 = Aow.Rtl.Rtl.Random(360);
                                                        var call__1112 = FPath[0];
                                                        var call__1113 = FPath[0];
                                                        var call__1114 = Aow.Rtl.Rtl.Random(2);
                                                        var call__1115 = Aow.Rtl.Rtl.Random(10);
                                                        Aow.Units.aowFX.aowFX.MakeSwarmPath(call__1115 + 10, I / 2, 45, call__1111, (call__1112.X + 100) - I, ((call__1113.Y - 200) - 50) + I, (Aow.Units.aowFX.TSwarmDirection)call__1114, ref Effect.FPath);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (((FFrame % 5) == 0) && (FFrame < 85))
                                    {
                                        var call__1116 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxDecayComet);
                                        Effect = call__1116;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        var call__1117 = FPath[0];
                                        var call__1118 = Aow.Rtl.Rtl.Random(50);
                                        var call__1119 = Aow.Rtl.Rtl.Random(5);
                                        var call__1120 = FPath[0];
                                        var call__1121 = Aow.Rtl.Rtl.Random(50);
                                        var call__1122 = FPath[0];
                                        var call__1123 = Aow.Rtl.Rtl.Random(50);
                                        var call__1124 = FPath[0];
                                        var call__1125 = Aow.Rtl.Rtl.Random(50);
                                        Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)((call__1124.X + call__1125) + 25)), unchecked((short)((call__1122.Y + call__1123) - 225)), unchecked((short)((call__1120.X + call__1121) - 25)), unchecked((short)((call__1117.Y + call__1118) - 25)), unchecked((short)(call__1119 + 5)), 100, ref Effect.FPath);
                                        {
                                            I = 0;
                                            byte end__1126 = 10;
                                            for (; I <= end__1126; I += 1)
                                            {
                                                Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 50)) / 10);
                                                Gravity = (float)1.5;
                                                VZ = (float)(((double)(Aow.Rtl.Rtl.Random(25) + 25)) / 10);
                                                Z = 0;
                                                var call__1127 = Effect.FPath.Count;
                                                var call__1128 = Effect.FPath[call__1127 - 1];
                                                var call__1129 = Effect.FPath.Count;
                                                var call__1130 = Effect.FPath[call__1129 - 1];
                                                var call__1131 = Effect.FPath.Count;
                                                var call__1132 = Effect.FPath[call__1131 - 1];
                                                var call__1133 = Aow.Rtl.Rtl.Random(360);
                                                Aow.Units.aowFX.aowFX.MakeArcPath(call__1132.X, call__1128.Y - call__1130.Z, call__1133, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                            }
                                        }

                                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(FTargetX, FTargetY, (FFrame - 50) / 5, ref J, ref K);
                                        // { Original swaps the converted hex coordinates. }
                                        Aow.Units.HSEngine.HSEngine.HXtoHP(J, K, ref X, ref Y);
                                        var call__1134 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__1135 = call__1134.Field[J, K];
                                        var call__1136 = call__1135.FindOwnedChild(2228484);
                                        UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1136;
                                        bool bool__1137 = (UnitHS != null);
                                        if (bool__1137)
                                        {
                                            var call__1138 = UnitHS.FUnit.GetSide();
                                            var call__1139 = FTarget.GetSide();
                                            bool__1137 = (call__1138 != call__1139);
                                        }

                                        if (bool__1137)
                                        {
                                            Effect.FTarget = UnitHS.FUnit;
                                        }
                                        else
                                        {
                                            Effect.FTarget = null;
                                        }

                                        if (Effect.FTarget != null)
                                        {
                                            var call__1140 = Aow.Rtl.Rtl.Random(3);
                                            var call__1141 = Effect.FPath.Count;
                                            var call__1142 = Effect.FPath[call__1141 - 1];
                                            var call__1143 = Effect.FPath.Count;
                                            var call__1144 = Effect.FPath[call__1143 - 1];
                                            var call__1145 = Effect.FPath.Count;
                                            var call__1146 = Effect.FPath[call__1145 - 1];
                                            Aow.Units.aowFX.aowFX.MakeArcPathXY(unchecked((short)call__1146.X), unchecked((short)(call__1142.Y - call__1144.Z)), unchecked((short)(X + 16)), unchecked((short)(Y - 8)), unchecked((short)(call__1140 + 6)), 100, ref Effect.FPath);
                                            {
                                                I = 0;
                                                int end__1147 = Aow.Rtl.Rtl.Random(3) + 5;
                                                for (; I <= end__1147; I += 1)
                                                {
                                                    var call__1148 = Effect.FPath.Count;
                                                    var call__1149 = Effect.FPath[call__1148 - 1];
                                                    var call__1150 = Effect.FPath.Count;
                                                    var call__1151 = Effect.FPath[call__1150 - 1];
                                                    var call__1152 = Effect.FPath.Count;
                                                    var call__1153 = Effect.FPath[call__1152 - 1];
                                                    var call__1154 = Effect.FPath.Count;
                                                    var call__1155 = Effect.FPath[call__1154 - 1];
                                                    var call__1156 = Effect.FPath.Count;
                                                    var call__1157 = Effect.FPath[call__1156 - 1];
                                                    var call__1158 = Effect.FPath.Count;
                                                    var call__1159 = Effect.FPath[call__1158 - 1];
                                                    Aow.Units.aowFX.aowFX.MakeLinePath(call__1159.X, call__1153.Y - call__1155.Z, call__1157.X, (call__1149.Y - call__1151.Z) - 6, 3, ref Effect.FPath);
                                                    var call__1160 = Effect.FPath.Count;
                                                    var call__1161 = Effect.FPath[call__1160 - 1];
                                                    var call__1162 = Effect.FPath.Count;
                                                    var call__1163 = Effect.FPath[call__1162 - 1];
                                                    var call__1164 = Effect.FPath.Count;
                                                    var call__1165 = Effect.FPath[call__1164 - 1];
                                                    var call__1166 = Effect.FPath.Count;
                                                    var call__1167 = Effect.FPath[call__1166 - 1];
                                                    var call__1168 = Effect.FPath.Count;
                                                    var call__1169 = Effect.FPath[call__1168 - 1];
                                                    var call__1170 = Effect.FPath.Count;
                                                    var call__1171 = Effect.FPath[call__1170 - 1];
                                                    Aow.Units.aowFX.aowFX.MakeLinePath(call__1171.X, call__1165.Y - call__1167.Z, call__1169.X, (call__1161.Y - call__1163.Z) + 6, 3, ref Effect.FPath);
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
                            NewPath = new Aow.Units.aowFX.TXYZList();
                            I = 0;
                            J = 0;
                            K = 0;
                            while (true)
                            {
                                var call__1172 = FPath.Count;
                                if (!(I < call__1172))
                                    break;
                                {
                                    var call__1173 = FPath[I];
                                    if (call__1173.Z > 0)
                                    {
                                        var call__1174 = FPath[I];
                                        if (call__1174.Z > 1)
                                        {
                                            var call__1175 = FPath[I];
                                            K = call__1175.Z;
                                        }

                                        var call__1176 = FPath[I];
                                        var call__1177 = FPath[I];
                                        var call__1178 = FPath[J];
                                        var call__1179 = FPath[J];
                                        Aow.Units.aowFX.aowFX.MakeLightningPath(call__1179.X, call__1178.Y, call__1177.X, call__1176.Y, ref NewPath);
                                        switch (K)
                                        {
                                            case -1:
                                                var call__1180 = NewPath.Count;
                                                var call__1181 = NewPath[call__1180 - 1];
                                                call__1181.Z = 16;
                                                var call__1182 = NewPath.Count;
                                                FFrame = call__1182 - 1;
                                                K = 0;
                                                break;
                                            case >= 2 and <= 16:
                                                var call__1183 = NewPath.Count;
                                                var call__1184 = NewPath[call__1183 - 1];
                                                call__1184.Z = K - 1;
                                                var call__1185 = NewPath.Count;
                                                FFrame = call__1185 - 1;
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
                            var call__1186 = FPath.Count;
                            if (FFrame >= (call__1186 - 1))
                            {
                                var call__1187 = FPath.Count;
                                FFrame = call__1187 - 1;
                                var call__1188 = FPath[FFrame];
                                if (call__1188.Z <= 1)
                                {
                                    FDone = true;
                                }
                            }

                            var call__1189 = FPath[FFrame];
                            if (call__1189.Z == 15)
                            {
                                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                {
                                    if (FAbilityIndex == 52)
                                    {
                                        var call__1190 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                        call__1190.FCastSounds.Play(1);
                                    }
                                    else
                                    {
                                        var call__1191 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                        call__1191.FSFXLibrary.Play(1);
                                    }
                                }

                                var call__1192 = FPath[FFrame];
                                var call__1193 = FPath[FFrame];
                                var call__1194 = FPath[FFrame];
                                Aow.Units.HSEngine.HSEngine.HPtoHX(call__1194.X, call__1192.Y - call__1193.Z, ref I, ref J);
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(I, J, 0, 3);
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxTouch:
                        case Aow.Units.AoWTC.TfxType.fxEffect:
                            if (((FFrame == 0) && Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds) && (FAbilityIndex == 65))
                            {
                                var call__1195 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                call__1195.FSFXLibrary.Play(0);
                            }

                            if (FAbilityIndex == 52)
                            {
                                var call__1196 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                var call__1197 = call__1196.FCastImages.ImageSequences[0];
                                Sequence = call__1197;
                            }
                            else
                            {
                                var call__1198 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                var call__1199 = call__1198.FImageSequences.ImageSequences[0];
                                Sequence = call__1199;
                            }

                            if (Sequence != null)
                            {
                                var call__1200 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= (call__1200 - 1))
                                {
                                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            var call__1201 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__1201.FCastSounds.Play(1);
                                        }
                                        else
                                        {
                                            var call__1202 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__1202.FSFXLibrary.Play(1);
                                        }
                                    }

                                    var call__1203 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1203 - 1;
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
                                var call__1204 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                var call__1205 = call__1204.FCastImages.ImageSequences[1];
                                Sequence = call__1205;
                            }
                            else
                            {
                                var call__1206 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                var call__1207 = call__1206.FImageSequences.ImageSequences[1];
                                Sequence = call__1207;
                            }

                            if (Sequence == null)
                            {
                                FFrame = -1;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxFBExplosion:
                            var call__1208 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                            var call__1209 = call__1208.FCastImages.ImageSequences[1];
                            Sequence = call__1209;
                            if (Sequence != null)
                            {
                                var call__1210 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= (call__1210 - 1))
                                {
                                    var call__1211 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1211 - 1;
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
                                var call__1212 = FPath[0];
                                if (call__1212.X == 0)
                                {
                                    var call__1213 = FPath[0];
                                    call__1213.X = 100;
                                    var call__1214 = Aow.Rtl.Rtl.Random(50);
                                    var call__1215 = FPath[0];
                                    call__1215.Y = call__1214;
                                }
                                else
                                {
                                    var call__1216 = Aow.Rtl.Rtl.Random(50);
                                    var call__1217 = FPath[0];
                                    call__1217.X = call__1216 + 50;
                                    var call__1218 = FPath[0];
                                    call__1218.Y = 0;
                                }
                            }
                            else
                            {
                                var call__1219 = FPath[0];
                                call__1219.X -= 3;
                                var call__1220 = FPath[0];
                                call__1220.Y += 3;
                                var call__1221 = FPath[0];
                                if (call__1221.X < 0)
                                {
                                    FFrame = -1;
                                }
                                else
                                {
                                    var call__1222 = FPath[0];
                                    if (call__1222.Y > 100)
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
                            var call__1223 = FPath.Count;
                            if (FFrame >= (call__1223 - 1))
                            {
                                var call__1224 = FPath.Count;
                                FFrame = call__1224 - 1;
                                FDone = true;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxWallSmoke:
                            var call__1225 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[5];
                            Sequence = call__1225;
                            if (Sequence == null)
                            {
                                FFrame = -1;
                            }
                            else
                            {
                                var call__1226 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= call__1226)
                                {
                                    var call__1227 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1227 - 1;
                                    FDone = true;
                                }
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxHitGround:
                            var call__1228 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[9];
                            Sequence = call__1228;
                            if (Sequence == null)
                            {
                                FFrame = -1;
                            }
                            else
                            {
                                var call__1229 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= call__1229)
                                {
                                    var call__1230 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1230 - 1;
                                    FDone = true;
                                }
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxHitWater:
                            var call__1231 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[10];
                            Sequence = call__1231;
                            if (Sequence == null)
                            {
                                FFrame = -1;
                            }
                            else
                            {
                                var call__1232 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= call__1232)
                                {
                                    var call__1233 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1233 - 1;
                                    FDone = true;
                                }
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxBlood:
                            var call__1234 = FPath[0];
                            var call__1235 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[call__1234.Z];
                            Sequence = call__1235;
                            if (Sequence == null)
                            {
                                FFrame = -1;
                            }
                            else
                            {
                                var call__1236 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= (call__1236 - 1))
                                {
                                    var call__1237 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1237 - 1;
                                    FDone = true;
                                }
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxTremors:
                            switch (FFrame)
                            {
                                case 0:
                                    var call__1238 = new Aow.Units.aowFX.TXYZ();
                                    FPath.Add(call__1238);
                                    var call__1239 = FPath[0];
                                    call__1239.X = Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewXhp;
                                    var call__1240 = FPath[0];
                                    call__1240.Y = Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewYhp;
                                    break;
                                case >= 1 and <= 29:
                                    var call__1241 = FPath[0];
                                    var call__1242 = Aow.Rtl.Rtl.Random(FFrame / 3);
                                    var call__1243 = FPath[0];
                                    var call__1244 = Aow.Rtl.Rtl.Random(FFrame / 3);
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.ChangeViewTo((call__1243.X + call__1244) - (FFrame / 6), (call__1241.Y + call__1242) - (FFrame / 6), Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                    break;
                                case >= 30 and <= 89:
                                    var call__1245 = FPath[0];
                                    var call__1246 = Aow.Rtl.Rtl.Random(10);
                                    var call__1247 = FPath[0];
                                    var call__1248 = Aow.Rtl.Rtl.Random(10);
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.ChangeViewTo((call__1247.X + call__1248) - 5, (call__1245.Y + call__1246) - 5, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                    break;
                                case >= 90 and <= 119:
                                    var call__1249 = FPath[0];
                                    var call__1250 = Aow.Rtl.Rtl.Random((119 - FFrame) / 3);
                                    var call__1251 = FPath[0];
                                    var call__1252 = Aow.Rtl.Rtl.Random((119 - FFrame) / 3);
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.ChangeViewTo((call__1251.X + call__1252) - ((119 - FFrame) / 6), (call__1249.Y + call__1250) - ((119 - FFrame) / 6), Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
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
                                var call__1253 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                var call__1254 = call__1253.FCastImages.ImageSequences[0];
                                Sequence = call__1254;
                            }
                            else
                            {
                                var call__1255 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                var call__1256 = call__1255.FImageSequences.ImageSequences[0];
                                Sequence = call__1256;
                            }

                            if (Sequence != null)
                            {
                                var call__1257 = Sequence.GetTotalSequenceFrames();
                                if (FFrame >= (call__1257 - 1))
                                {
                                    var call__1258 = Sequence.GetTotalSequenceFrames();
                                    FFrame = call__1258 - 1;
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
                                    int end__1259 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                                    for (; I <= end__1259; I += 1)
                                    {
                                        var call__1260 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                        bool bool__1261 = (call__1260 is Aow.Units.AoWE.TCombatUnit);
                                        if (bool__1261)
                                        {
                                            var call__1262 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            var call__1263 = call__1262.GetEnabled();
                                            bool__1261 = call__1263;
                                        }

                                        bool bool__1264 = bool__1261;
                                        if (bool__1264)
                                        {
                                            var call__1265 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            bool__1264 = (call__1265.FCombatPlayer == FTarget.FCombatPlayer);
                                        }

                                        if (bool__1264)
                                        {
                                            var call__1266 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                            Effect = call__1266;
                                            var call__1267 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            var call__1268 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__1267).FUnitHS.GetYhx();
                                            var call__1269 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            var call__1270 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__1269).FUnitHS.GetXhx();
                                            Aow.Units.HSEngine.HSEngine.HXtoHP(call__1270, call__1268, ref J, ref K);
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
                                    int end__1271 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                                    for (; I <= end__1271; I += 1)
                                    {
                                        var call__1272 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                        bool bool__1273 = (call__1272 is Aow.Units.AoWE.TCombatUnit);
                                        if (bool__1273)
                                        {
                                            var call__1274 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            var call__1275 = call__1274.GetEnabled();
                                            bool__1273 = call__1275;
                                        }

                                        bool bool__1276 = bool__1273;
                                        if (bool__1276)
                                        {
                                            var call__1277 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            bool__1276 = (call__1277.FCombatPlayer == FTarget.FCombatPlayer);
                                        }

                                        if (bool__1276)
                                        {
                                            var call__1278 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxPrayerGlow);
                                            Effect = call__1278;
                                            var call__1279 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            var call__1280 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__1279).FUnitHS.GetYhx();
                                            var call__1281 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            var call__1282 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__1281).FUnitHS.GetXhx();
                                            Aow.Units.HSEngine.HSEngine.HXtoHP(call__1282, call__1280, ref J, ref K);
                                            var call__1283 = new Aow.Units.aowFX.TXYZ();
                                            Effect.FPath.Add(call__1283);
                                            var call__1284 = Effect.FPath[0];
                                            call__1284.X = J + 16;
                                            var call__1285 = Effect.FPath[0];
                                            call__1285.Y = K + 16;
                                            Effect.FAbilityIndex = FAbilityIndex;
                                            Effect.FSpellIndex = FSpellIndex;
                                            var call__1286 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                            Effect.FTarget = call__1286;
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
                            var call__1287 = FPath.Count;
                            if (FFrame >= call__1287)
                            {
                                FFrame = -1;
                            }

                            break;
                        case Aow.Units.AoWTC.TfxType.fxGeyser:
                            switch (FFrame)
                            {
                                case 150:
                                    FFrame = -1;
                                    bool bool__1288 = (FTarget != null);
                                    if (bool__1288)
                                    {
                                        var call__1289 = FTarget.GetEnabled();
                                        bool__1288 = call__1289;
                                    }

                                    if (bool__1288)
                                    {
                                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds)
                                        {
                                            if (FAbilityIndex == 52)
                                            {
                                                var call__1290 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                                call__1290.FCastSounds.Play(1);
                                            }
                                            else
                                            {
                                                var call__1291 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                                call__1291.FSFXLibrary.Play(1);
                                            }
                                        }

                                        ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FOffsetY = 0;
                                        ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FBusy = false;
                                    }

                                    bool bool__1292 = (FTarget != null);
                                    if (bool__1292)
                                    {
                                        var call__1293 = ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnit.GetMoveTypes();
                                        bool__1292 = call__1293.Contains(Aow.Units.AoWE.TMoveType.mtFlying);
                                    }

                                    if (bool__1292)
                                    {
                                        FTarget = null;
                                    }

                                    break;
                                case >= 0 and <= 99:
                                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlaySounds && ((FFrame % 30) == 29))
                                    {
                                        if (FAbilityIndex == 52)
                                        {
                                            var call__1294 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSpellIndex);
                                            call__1294.FCastSounds.Play(0);
                                        }
                                        else
                                        {
                                            var call__1295 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FAbilityIndex);
                                            call__1295.FSFXLibrary.Play(0);
                                        }
                                    }

                                    bool bool__1296 = (FTarget != null);
                                    if (bool__1296)
                                    {
                                        var call__1297 = FTarget.GetEnabled();
                                        bool__1296 = call__1297;
                                    }

                                    if (bool__1296)
                                    {
                                        var call__1298 = Aow.Rtl.Rtl.Random(2);
                                        ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FOffsetY = (call__1298 + -((FFrame * 3) / 2)) - 1;
                                        ((Aow.Units.AoWTC.TTacticalCombatUnit)FTarget).FUnitHS.FBusy = true;
                                    }

                                    J = Aow.Rtl.Rtl.Random(6) + 20;
                                {
                                    I = 1;
                                    int end__1299 = J - 1;
                                    for (; I <= end__1299; I += 1)
                                    {
                                        var call__1300 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1300;
                                        Effect.FAbilityIndex = FAbilityIndex;
                                        Effect.FSpellIndex = FSpellIndex;
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(10) + 10)) / 10);
                                        Gravity = (float)0.5;
                                        var call__1301 = Aow.Rtl.Rtl.Random(50);
                                        VZ = (float)(((double)(((double)call__1301) + (((double)(FFrame * 3)) * 0.5))) / 10);
                                        Z = 0;
                                        var call__1302 = FPath[0];
                                        var call__1303 = FPath[0];
                                        Aow.Units.aowFX.aowFX.MakeArcPath(call__1303.X, call__1302.Y, (I * 360) / J, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }

                                    break;
                                case >= 125 and <= 149:
                                    bool bool__1304 = (FTarget != null);
                                    if (bool__1304)
                                    {
                                        var call__1305 = FTarget.GetEnabled();
                                        bool__1304 = call__1305;
                                    }

                                    if (bool__1304)
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
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Error during NewFrame", Aow.Rtl.Rtl.NullTerminatedText(((((AnsiString)"Error - fxHS ") + Aow.Rtl.Rtl.IntToStr(FAbilityIndex)) + AnsiString.FromBytes(32)) + Aow.Rtl.Rtl.IntToStr(FSpellIndex)), (int)0);
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
                    byte end__1306 = 0;
                    for (; I >= end__1306; I -= 1)
                    {
                        var call__1307 = FItems.GetItem(I);
                        call__1307.SetOwner(Items);
                    }
                }
            }
            else
            {
                var call__1308 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL);
                var call__1309 = call__1308.FindOwnedChild(131697);
                ItemHS = (Aow.Units.AoWE.TItemHS)call__1309;
                if (ItemHS == null)
                {
                    ItemHS = Aow.Units.AoWE.TItemHS.Create(null);
                    Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(ItemHS, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL);
                }

                {
                    I = FItems.GetCount() - 1;
                    byte end__1310 = 0;
                    for (; I >= end__1310; I -= 1)
                    {
                        var call__1311 = FItems.GetItem(I);
                        call__1311.SetOwner(ItemHS.FItems);
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
            var call__1312 = FImages[FImageIndex];
            call__1312.ShowClipped((X - XhpSurf) - 8, (Y - YhpSurf) - 8, SurfaceDesc);
        }

        // AoWTC.pas:5031
        public override void Activate()
        {
            Aow.Units.HSEngine.TXYL P = default;
            base.Activate();
            bool bool__1313 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting);
            if (bool__1313)
            {
                var call__1314 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.ValidatePosition(ref FPosition, ref FIndex);
                bool__1313 = call__1314;
            }

            if (bool__1313)
            {
                var call__1315 = GetYhx();
                var call__1316 = GetXhx();
                var call__1317 = Aow.Units.HSEngine.HSEngine.PackXYL(unchecked((sbyte)call__1316), unchecked((sbyte)call__1315), 0);
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.SetUnitPosition(FPosition, FIndex, call__1317);
                var call__1318 = Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TCUNITS.ILB");
                FImages = call__1318;
                UpdateImageIndex();
            }
            else
            {
                var call__1319 = GetXhx();
                P.X = unchecked((sbyte)call__1319);
                var call__1320 = GetYhx();
                P.Y = unchecked((sbyte)call__1320);
                P.L = 0;
                P.Flag = unchecked((sbyte)FDirection);
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.SetUnitPosition(FPosition, FIndex, P);
                Destroy();
            }
        }

        // AoWTC.pas:5049
        public override void Deactivate()
        {
            var call__1321 = Activated();
            if (call__1321 && ((Aow.Units.HSEngine.TMapLevel)((Aow.Units.HSEngine.TMapField)Owner).Owner).Map.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.ReleaseIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TCUNITS.ILB");
                var call__1322 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(FPosition, FIndex);
                var call__1323 = GetXhx();
                bool bool__1324 = (call__1322.X == call__1323);
                if (bool__1324)
                {
                    var call__1325 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(FPosition, FIndex);
                    var call__1326 = GetYhx();
                    bool__1324 = (call__1325.Y == call__1326);
                }

                if (bool__1324)
                {
                    var call__1327 = Aow.Units.HSEngine.HSEngine.PackXYL(unchecked((sbyte)-1), unchecked((sbyte)-1), unchecked((sbyte)-1));
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.SetUnitPosition(FPosition, FIndex, call__1327);
                }
            }

            base.Deactivate();
        }

        // AoWTC.pas:5068
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            var call__1328 = Map.GetField(Xhx, Yhx, Lhx);
            var call__1329 = call__1328.FindOwnedChild(2228483);
            bool bool__1330 = (call__1329 == null);
            if (bool__1330)
            {
                var call__1331 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.ValidatePosition(ref FPosition, ref FIndex);
                bool__1330 = call__1331;
            }

            Result = bool__1330;
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
            var call__1332 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[4];
            Sequence = call__1332;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref X, ref Y);
            if (Sequence != null)
            {
                var call__1333 = Sequence.GetUnloopedFrameIndex(FFrame);
                Frame = call__1333;
                var call__1334 = Sequence.ImageLibrary[Frame];
                call__1334.ShowClipped((X - XhpSurf) + FOffsetX, (Y - YhpSurf) + FOffsetY, SurfaceDesc);
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
                var call__1335 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[FSequenceIndex];
                Sequence = call__1335;
                while (Sequence != null)
                {
                    var call__1336 = Sequence.GetUnloopedFrameIndex(FSequenceFrame);
                    Frame = call__1336;
                    if (Frame != -1)
                    {
                        var call__1337 = Sequence.ImageLibrary[Frame];
                        call__1337.Show((XhpSurf + FOffsetX) - 5, (YhpSurf + FOffsetY) + 10, SurfaceDesc);
                    }

                    Sequence = Sequence.Next;
                }
            }

            var call__1338 = FUnitGFX.FSequences.ImageSequences[30];
            Sequence = call__1338;
            while (Sequence != null)
            {
                var call__1339 = Sequence.GetUnloopedFrameIndex(FFrame);
                Frame = call__1339;
                if (Frame != -1)
                {
                    var call__1340 = Sequence.ImageLibrary[Frame];
                    call__1340.Show(XhpSurf + FOffsetX, YhpSurf + FOffsetY, SurfaceDesc);
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
                    var call__1341 = GetYhx();
                    var call__1342 = GetXhx();
                    var call__1343 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__1344 = call__1343.Field[call__1342, call__1341];
                    var call__1345 = call__1344.FindOwnedChild(2228484);
                    if (call__1345 == null)
                    {
                        var call__1346 = GetYhx();
                        var call__1347 = GetXhx();
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.MakeVisible(call__1347, call__1346, 0, 5);
                        var call__1348 = GetYhx();
                        var call__1349 = GetXhx();
                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__1349, call__1348, ref X, ref Y);
                        {
                            Ring = 1;
                            byte end__1350 = 3;
                            for (; Ring <= end__1350; Ring += 1)
                            {
                                N = Aow.Rtl.Rtl.Random(5);
                                DX = Aow.Rtl.Rtl.Random(20) + (Ring * 30);
                                DY = Aow.Rtl.Rtl.Random(20) + (Ring * 30);
                                {
                                    I = 0;
                                    byte end__1351 = 17;
                                    for (; I <= end__1351; I += 1)
                                    {
                                        var call__1352 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                        Effect = call__1352;
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

                                        var call__1353 = System.Math.PI;
                                        var call__1354 = System.Math.Sin(((double)(((double)((I * 5) + N)) * call__1353)) / 180);
                                        var call__1355 = System.Math.PI;
                                        var call__1356 = System.Math.Cos(((double)(((double)((I * 5) + N)) * call__1355)) / 180);
                                        Aow.Units.aowFX.aowFX.MakeLinePath(Aow.Rtl.Rtl.Round(((double)call__1356) * DX) + (X + 16), (Aow.Rtl.Rtl.Round(((double)call__1354) * DY) / 2) + (Y + 16), X + 16, Y + 16, 100, ref Effect.FPath);
                                    }
                                }
                            }
                        }

                        return;
                    }
                }

                if ((Msg.MsgID == 200713) && (Msg.HN == 0))
                {
                    var call__1357 = GetYhx();
                    var call__1358 = GetXhx();
                    var call__1359 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__1360 = call__1359.Field[call__1358, call__1357];
                    var call__1361 = call__1360.FindOwnedChild(2228484);
                    if (call__1361 == null)
                    {
                        var call__1362 = GetYhx();
                        var call__1363 = GetXhx();
                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__1363, call__1362, ref X, ref Y);
                        N = Aow.Rtl.Rtl.Random(6) + 25;
                        {
                            I = 1;
                            int end__1364 = N - 1;
                            for (; I <= end__1364; I += 1)
                            {
                                var call__1365 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxGeyserWater);
                                Effect = call__1365;
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
                                var call__1366 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                Aow.Rtl.Rtl.RandSeed = call__1366;
                                var call__1367 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(130);
                                var call__1368 = call__1367.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                AnimateAction = call__1368;
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(AnimateAction);
                                AnimateAction.Release();
                                break;
                            case 129:
                                var call__1369 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                Aow.Rtl.Rtl.RandSeed = call__1369;
                                var call__1370 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(129);
                                var call__1371 = call__1370.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                RaiseAction = call__1371;
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.ExecuteCombatAction(RaiseAction);
                                RaiseAction.Release();
                                break;
                        }

                        var call__1372 = FUnit.GetEnabled();
                        if (call__1372)
                        {
                            UnitHS = Aow.Units.AoWTC.TTacticalCombatUnitHS.Create(null);
                            UnitHS.Initialize(FUnit);
                            var call__1373 = Aow.Rtl.Rtl.Random(6);
                            UnitHS.SetDirection(unchecked((byte)(call__1373 + 1)));
                            var call__1374 = GetYhx();
                            var call__1375 = GetXhx();
                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(UnitHS, call__1375, call__1374, 0);
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
            var call__1376 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[FSequenceIndex];
            Sequence = call__1376;
            var call__1377 = Sequence.GetTotalSequenceFrames();
            if ((FSequenceFrame + 1) < call__1377)
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
            var call__1378 = GetYhx();
            var call__1379 = GetXhx();
            Result = (1744830464 + (call__1378 << 13)) + call__1379;
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
                    var call__1380 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[26];
                    Sequence = call__1380;
                    break;
                case 12:
                    var call__1381 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[27];
                    Sequence = call__1381;
                    break;
                default:
                    var call__1382 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[25];
                    Sequence = call__1382;
                    break;
            }

            while (Sequence != null)
            {
                var call__1383 = Sequence.GetTotalSequenceFrames();
                var call__1384 = Sequence.GetUnloopedFrameIndex(Aow.Rtl.Rtl.ObjectToInt(this) % call__1383);
                Frame = call__1384;
                if (Frame != -1)
                {
                    var call__1385 = Sequence.ImageLibrary[Frame];
                    call__1385.Show(XhpSurf, YhpSurf, SurfaceDesc);
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
        public TUnitPositionControl(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__1386 = new Aow.Units.HSEngine.TXYLList();
            FPositions = call__1386;
            var call__1387 = Aow.Units.HSEngine.HSEngine.PackXYL(unchecked((sbyte)-1), unchecked((sbyte)-1), unchecked((sbyte)-1));
            FPositions.Default = call__1387;
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
                var call__1388 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(X, Y, L);
                if (call__1388.TerrainType != 15)
                {
                    var call__1389 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(X, Y, L);
                    var call__1390 = call__1389.FindOwnedChild(2228483);
                    HS = (Aow.Units.AoWTC.TUnitPositionHS)call__1390;
                    if (HS == null)
                    {
                        HS = Aow.Units.AoWTC.TUnitPositionHS.Create(null);
                        var call__1391 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(HS, X, Y, L);
                        if (!call__1391)
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
                var call__1392 = FPositions[I];
                Result = call__1392;
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
            var call__1393 = FPositions[(Position * 8) + Index];
            Result = call__1393.X == -1;
            if (!Result)
            {
                Index = 0;
                while (true)
                {
                    var call__1394 = FPositions[(Position * 8) + Index];
                    if (!((call__1394.X != -1) && (Index != 8)))
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
                        var call__1395 = FPositions[(Position * 8) + Index];
                        if (!((call__1395.X != -1) && (Index != 8)))
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

    }

    // { TCombatMoveControlSettings (instance size 432, VMT 004129E8): $1AC FMoveFlags. }
    public partial class TCombatMoveControlSettings : Aow.Units.AoWE.TMoveControlSettings
    {
        public TCombatMoveControlSettings(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        public TSet<Aow.Units.AoWTC.TCombatMoveModeElement> FMoveFlags;
        // { $1AC }
        public Aow.Units.FixedArray_byte_3 F_CombatMoveControlSettings_1AD;
        // intf_P5_01.inc:63

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
        public static Aow.Units.AoWTC.TCombatMoveControl Create(Aow.Units.HSEngine.THSMap AOwner)
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
        public TTacticalCombatUnit(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

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
            bool bool__1396 = (A1 == unchecked((byte)FCombatPlayer));
            if (bool__1396)
            {
                var call__1397 = GetEnabled();
                bool__1396 = call__1397;
            }

            if (bool__1396)
            {
                var call__1398 = GetMoves();
                FMovePoints = call__1398;
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
                var call__1399 = GetEnabled();
                bool bool__1400 = call__1399 && (FUnitHS != null);
                if (bool__1400)
                {
                    var call__1401 = FUnit.GetBloodType();
                    bool__1400 = (call__1401 != Aow.Units.AoWE.TBloodType.btNone);
                }

                if (bool__1400)
                {
                    var call__1402 = GetHitPoints();
                    bool bool__1403 = (A1 >= call__1402);
                    if (bool__1403)
                    {
                        var call__1404 = GetAbilityEnabled(131);
                        bool__1403 = (call__1404 || (FDeathMethod == Aow.Units.AoWTC.TTCDeathMethod.dmExplode));
                    }

                    if (bool__1403)
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

                        var call__1405 = FUnitHS.GetYhx();
                        var call__1406 = FUnitHS.GetXhx();
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__1406, call__1405, Direction, ref X, ref Y);
                        var call__1407 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__1408 = call__1407.Field[X, Y];
                        bool bool__1409 = (call__1408.TerrainType != 0);
                        if (bool__1409)
                        {
                            var call__1410 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1411 = call__1410.Field[X, Y];
                            bool__1409 = (call__1411.TerrainType != 9);
                        }

                        if (bool__1409)
                        {
                            var call__1412 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1413 = call__1412.Field[X, Y];
                            var call__1414 = Aow.Units.AoWTC.TCBloodHS.Create(call__1413);
                            Blood = call__1414;
                            Blood.Activate();
                            var call__1415 = Aow.Rtl.Rtl.Random(32);
                            Blood.FOffsetX = call__1415 - 32;
                            var call__1416 = Aow.Rtl.Rtl.Random(32);
                            Blood.FOffsetY = call__1416 - 32;
                            var call__1417 = Aow.Units.AoWE.AoWE.AoWHSSet.FImageSequences.ImageSequences[4];
                            var call__1418 = call__1417.GetTotalSequenceFrames();
                            var call__1419 = Aow.Rtl.Rtl.Random(call__1418);
                            Blood.FFrame = call__1419;
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
                            byte end__1420 = 9;
                            for (; I <= end__1420; I += 1)
                            {
                                var call__1421 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.AddEffect(Aow.Units.AoWTC.TfxType.fxDecayComet);
                                Effect = call__1421;
                                Effect.FAbilityIndex = 52;
                                Effect.FSpellIndex = 106;
                                var call__1422 = FUnitHS.GetYhx();
                                var call__1423 = FUnitHS.GetXhx();
                                Aow.Units.HSEngine.HSEngine.HXtoHP(call__1423, call__1422, ref X, ref Y);
                                var call__1424 = new Aow.Units.aowFX.TXYZ();
                                Effect.FPath.Add(call__1424);
                                var call__1425 = Effect.FPath[0];
                                call__1425.X = X + 16;
                                var call__1426 = Effect.FPath[0];
                                call__1426.Y = Y + 8;
                                {
                                    J = 0;
                                    byte end__1427 = 10;
                                    for (; J <= end__1427; J += 1)
                                    {
                                        Speed = (float)(((double)(Aow.Rtl.Rtl.Random(50) + 50)) * 0.05);
                                        Gravity = (float)1.5;
                                        VZ = (float)(((double)(Aow.Rtl.Rtl.Random(25) + 25)) * 0.05);
                                        Z = 0;
                                        var call__1428 = Effect.FPath.Count;
                                        var call__1429 = Effect.FPath[call__1428 - 1];
                                        var call__1430 = Effect.FPath.Count;
                                        var call__1431 = Effect.FPath[call__1430 - 1];
                                        var call__1432 = Effect.FPath.Count;
                                        var call__1433 = Effect.FPath[call__1432 - 1];
                                        var call__1434 = Aow.Rtl.Rtl.Random(360);
                                        Aow.Units.aowFX.aowFX.MakeArcPath(call__1433.X, call__1429.Y - call__1431.Z, call__1434, ref Z, ref Speed, ref VZ, ref Gravity, ref Effect.FPath);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var call__1435 = FUnitHS.GetYhx();
                        var call__1436 = FUnitHS.GetXhx();
                        var call__1437 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__1438 = call__1437.Field[call__1436, call__1435];
                        bool bool__1439 = (call__1438.TerrainType != 0);
                        if (bool__1439)
                        {
                            var call__1440 = FUnitHS.GetYhx();
                            var call__1441 = FUnitHS.GetXhx();
                            var call__1442 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1443 = call__1442.Field[call__1441, call__1440];
                            bool__1439 = (call__1443.TerrainType != 9);
                        }

                        if (bool__1439)
                        {
                            var call__1444 = FUnitHS.GetYhx();
                            var call__1445 = FUnitHS.GetXhx();
                            var call__1446 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1447 = call__1446.Field[call__1445, call__1444];
                            var call__1448 = Aow.Units.AoWTC.TCCorpseHS.Create(call__1447);
                            Corpse = call__1448;
                            Corpse.FUnitGFX = FUnitHS.FUnitGFXResource;
                            var call__1449 = Corpse.FUnitGFX.FSequences.ImageSequences[30];
                            Sequence = call__1449;
                            if (Sequence != null)
                            {
                                var call__1450 = Sequence.GetTotalSequenceFrames();
                                var call__1451 = Aow.Rtl.Rtl.Random(call__1450);
                                Corpse.FFrame = call__1451;
                            }

                            switch (FDeathMethod)
                            {
                                case Aow.Units.AoWTC.TTCDeathMethod.dmNormal:
                                    var call__1452 = FUnit.GetBloodType();
                                    switch (call__1452)
                                    {
                                        case Aow.Units.AoWE.TBloodType.btNone:
                                            Corpse.FSequenceIndex = -1;
                                            break;
                                        default:
                                        {
                                            var call__1453 = FUnit.GetUnitSize();
                                            Corpse.FSequenceIndex = (unchecked((int)call__1453) & 127) + 30;
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

                            var call__1454 = Aow.Rtl.Rtl.Random(10);
                            Corpse.FOffsetX = call__1454 - 5;
                            var call__1455 = Aow.Rtl.Rtl.Random(10);
                            Corpse.FOffsetY = call__1455 - 5;
                            var call__1456 = FUnit.GetUnitType();
                            if (call__1456 != Aow.Units.AoWE.TUnitType.utMachine)
                            {
                                Corpse.FUnit = this;
                            }

                            Corpse.Activate();
                        }
                    }

                    var call__1457 = FUnitHS.GetYhx();
                    var call__1458 = FUnitHS.GetXhx();
                    var call__1459 = FUnitHS.GetYhx();
                    var call__1460 = FUnitHS.GetXhx();
                    var call__1461 = Aow.Rtl.Rtl.Rect(call__1460, call__1459, call__1458 + 1, call__1457 + 1);
                    R = call__1461;
                    FUnitHS.Free();
                    FUnitHS = null;
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.TriggerMapChangedEvent(R, 0);
                }
            }
            else
            {
                if (FUnitHS != null)
                {
                    var call__1462 = FUnit.GetUnitGFXResourceIndex();
                    FUnitHS.SetUnitGFXResourceIndex(call__1462);
                }
            }

            return Result;
        }

        // AoWTC.pas:2605
        public int GetPosValue()
        {
            int Result = default;
            var call__1463 = GetAttack();
            var call__1464 = GetDamage();
            var call__1465 = GetDefense();
            var call__1466 = GetMoves();
            Result = ((call__1463 + call__1464) + call__1465) + (call__1466 / 3);
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
                var call__1467 = FUnitHS.GetYhx();
                var call__1468 = FUnitHS.GetXhx();
                Aow.Units.CombatFire.CombatFire.CreateFire(false, 50, call__1468, call__1467);
            }

            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitChangedEvents.TriggerNotifyEvents(FUnitHS);
        }

        // intf_P3_01.inc:244

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
                    var call__1469 = FUnit.GetPlayer();
                    if (call__1469 != 0)
                    {
                        var call__1470 = FUnit.GetPlayer();
                        var call__1471 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__1470);
                        Msg.Param[0] = Msg.Param[0].Value | (call__1471.FPlayerRace + 1);
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
                    var call__1472 = FUnit.GetPlayer();
                    Msg.Result = call__1472;
                }
                else
                {
                    if ((Msg.MsgID == 200704) && (Msg.HN == 0))
                    {
                        var call__1473 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                        Aow.Rtl.Rtl.RandSeed = call__1473;
                        var call__1474 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(65);
                        var call__1475 = ((Aow.Units.AoWE.TSelfDestructAbility)call__1474).CreateSelfDestructCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit);
                        SelfDestruct = call__1475;
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
                            var call__1476 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                            Aow.Rtl.Rtl.RandSeed = call__1476;
                            var call__1477 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(117);
                            var call__1478 = call__1477.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                            SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1478;
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
                                var call__1479 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                Aow.Rtl.Rtl.RandSeed = call__1479;
                                var call__1480 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(113);
                                var call__1481 = call__1480.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1481;
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
                                    var call__1482 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                    Aow.Rtl.Rtl.RandSeed = call__1482;
                                    var call__1483 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(31);
                                    var call__1484 = ((Aow.Units.AoWE.TRangedAttackAbility)call__1483).CreateRangedAttackCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, 0);
                                    RangedDamage = call__1484;
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
                                        var call__1485 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                        Aow.Rtl.Rtl.RandSeed = call__1485;
                                        var call__1486 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Msg.Param[1].Value);
                                        var call__1487 = call__1486.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                        SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1487;
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
                                            var call__1488 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                            Aow.Rtl.Rtl.RandSeed = call__1488;
                                            var call__1489 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Msg.Param[1].Value);
                                            var call__1490 = ((Aow.Units.AoWE.TRangedAttackAbility)call__1489).CreateRangedAttackCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, 0);
                                            RangedDamage = call__1490;
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
                                                var call__1491 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                Aow.Rtl.Rtl.RandSeed = call__1491;
                                                var call__1492 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(104);
                                                var call__1493 = call__1492.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1493;
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
                                                    var call__1494 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                    Aow.Rtl.Rtl.RandSeed = call__1494;
                                                    var call__1495 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(120);
                                                    var call__1496 = call__1495.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                    SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1496;
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
                                                        var call__1497 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                        Aow.Rtl.Rtl.RandSeed = call__1497;
                                                        var call__1498 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(116);
                                                        var call__1499 = call__1498.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                        SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1499;
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
                                                            var call__1500 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                            Aow.Rtl.Rtl.RandSeed = call__1500;
                                                            var call__1501 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(127);
                                                            var call__1502 = call__1501.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                            SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1502;
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
                                                                var call__1503 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                                Aow.Rtl.Rtl.RandSeed = call__1503;
                                                                var call__1504 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(128);
                                                                var call__1505 = call__1504.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                                SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1505;
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
                                                                        var call__1506 = Aow.Rtl.Rtl.Random((Msg.Param[0].Value * 2) + 1);
                                                                        FOffsetX = call__1506 - Msg.Param[0].Value;
                                                                        var call__1507 = Aow.Rtl.Rtl.Random((Msg.Param[0].Value * 2) + 1);
                                                                        FOffsetY = call__1507 - Msg.Param[0].Value;
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
                                                                                var call__1508 = FUnit.FUnit.GetMoveTypes();
                                                                                if ((!call__1508.Contains(Aow.Units.AoWE.TMoveType.mtFlying) && (((Aow.Units.HSEngine.TMapField)Owner).TerrainType != 0)) && (((Aow.Units.HSEngine.TMapField)Owner).TerrainType != 9))
                                                                                {
                                                                                    var call__1509 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                                                                                    Aow.Rtl.Rtl.RandSeed = call__1509;
                                                                                    var call__1510 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(112);
                                                                                    var call__1511 = call__1510.CreateCA(Msg.Param[0].As<Aow.Units.AoWE.TCombatObject>(), FUnit, false);
                                                                                    SpellDamage = (Aow.Units.AoWE.TDamageCA)call__1511;
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
                var call__1512 = GetYhx();
                var call__1513 = GetXhx();
                var call__1514 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__1515 = call__1514.Field[call__1513, call__1512];
                var call__1516 = call__1515.FindOwnedChild(2228490);
                Movement = (Aow.Units.FleeLoc.TMovementHS)call__1516;
                if ((Movement != null) && Movement.FEnabled)
                {
                    var call__1517 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                    var call__1518 = FUnit.GetSide();
                    if (call__1517 == call__1518)
                    {
                        MoveResult.Code = 0;
                        MoveResult.Done = false;
                        return;
                    }
                }

                var call__1519 = GetXhx();
                if ((call__1519 != ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX) && (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX != -1))
                {
                    return;
                }

                var call__1520 = GetYhx();
                if ((call__1520 != ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY) && (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY != -1))
                {
                    return;
                }

                bool bool__1521 = !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiMove) && !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmMoveRanged);
                if (bool__1521)
                {
                    var call__1522 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetCount();
                    bool__1521 = (call__1522 >= 2);
                }

                if (bool__1521)
                {
                    return;
                }

                bool bool__1523 = (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility <= -1);
                if (bool__1523)
                {
                    var call__1524 = Aow.Units.AoWE.AoWE.MeleeDamageTypes(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit);
                    bool bool__1525 = (call__1524 == TSet<Aow.Units.AoWE.TDamageType>.Of());
                    if (!bool__1525)
                    {
                        var call__1526 = FUnit.GetAbilityEnabled(1);
                        bool bool__1527 = call__1526;
                        if (bool__1527)
                        {
                            var call__1528 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1);
                            bool__1527 = !call__1528;
                        }

                        bool__1525 = bool__1527;
                    }

                    bool__1523 = bool__1525;
                }

                if ((bool__1523 && !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmMoveRanged)) && !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiMove))
                {
                    return;
                }

                if (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility == -1)
                {
                    if (!((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmaiMove) && !((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FMoveFlags.Contains(Aow.Units.AoWTC.TCombatMoveModeElement.mmMoveRanged))
                    {
                        var call__1529 = FUnit.GetSide();
                        var call__1530 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                        if (call__1529 == call__1530)
                        {
                            return;
                        }
                    }

                    MoveResult.Code = 0;
                    return;
                }

                var call__1531 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility);
                call__1531.GetCombatInfo(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.FUnit, ref AbilityInfo.Data);
                bool bool__1532 = AbilityInfo.Data.TargetTypes.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend);
                if (bool__1532)
                {
                    var call__1533 = FUnit.GetSide();
                    var call__1534 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                    bool__1532 = (call__1533 == call__1534);
                }

                if (bool__1532)
                {
                    goto Touch;
                }

                var call__1535 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FSelectedAbility);
                call__1535.GetCombatInfo(((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.FUnit, ref EnemyInfo.Data);
                if (!EnemyInfo.Data.TargetTypes.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy))
                {
                    return;
                }

                var call__1536 = FUnit.GetSide();
                var call__1537 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                if (call__1536 == call__1537)
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
                var call__1538 = GetYhx();
                var call__1539 = GetXhx();
                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__1539, call__1538, Info.HN, ref X, ref Y);
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

                var call__1540 = GetXhx();
                bool bool__1541 = (call__1540 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX);
                if (bool__1541)
                {
                    var call__1542 = GetYhx();
                    bool__1541 = (call__1542 == ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY);
                }

                if (bool__1541)
                {
                    return;
                }

                var call__1543 = FUnit.GetSide();
                var call__1544 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetSide();
                if (call__1543 == call__1544)
                {
                    return;
                }

                var call__1545 = Aow.Units.AoWE.AoWE.MeleeDamageTypes(FUnit);
                if (call__1545 == TSet<Aow.Units.AoWE.TDamageType>.Of())
                {
                    return;
                }

                var call__1546 = FUnit.GetLocked();
                if (call__1546)
                {
                    return;
                }

                var call__1547 = FUnit.GetAbilityEnabled(1);
                var call__1548 = ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FUnitHS.FUnit.GetAbilityEnabled(1);
                if (call__1547 != call__1548)
                {
                    return;
                }

                bool bool__1549 = (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX >= 0) && (((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY >= 0);
                if (bool__1549)
                {
                    var call__1550 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX, ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY);
                    bool__1549 = (call__1550 <= 1);
                }

                if (bool__1549)
                {
                    var call__1551 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                    var call__1552 = call__1551.Field[((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationX, ((Aow.Units.AoWTC.TCombatMoveControl)Info.Sender).FDestinationY];
                    var call__1553 = call__1552.FindOwnedChild(2228484);
                    if (call__1553 != null)
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
            var call__1554 = FUnit.FUnit.GetUnitGFXResourceIndex();
            SetUnitGFXResourceIndex(call__1554);
            var call__1555 = new Aow.Rtl.TList();
            FAbilities = call__1555;
            CreateTCAbList();
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.NewFrameEvents.Add(NewFrame);
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled)
            {
                var call__1556 = FUnit.GetPlayer();
                var call__1557 = FUnit.GetVisibilityRange();
                var call__1558 = GetYhx();
                var call__1559 = GetXhx();
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.ExploreArea(call__1559, call__1558, 0, unchecked((sbyte)call__1556), call__1557);
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
                int end__1560 = FAbilities.Count - 1;
                for (; I <= end__1560; I += 1)
                {
                    var call__1561 = FAbilities[I];
                    ((Aow.Rtl.TObject)call__1561).Free();
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
            var call__1562 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.GetTCUnitHS();
            if (call__1562 != this)
            {
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.SetTCUnitHS(this);
            }

            FUnitGFXResource.FSFXLibraries[0].Play(0);
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer == FUnit.FCombatPlayer)
            {
                var call__1563 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer));
                call__1563.FLastUnit = FUnit;
            }
        }

        // AoWTC.pas:3796
        public override void Unselect()
        {
            base.Unselect();
            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
            {
                var call__1564 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.GetTCUnitHS();
                if (call__1564 == this)
                {
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.Unselect();
                }
            }
        }

        // AoWTC.pas:3357
        public override int GetMoveSteps()
        {
            int Result = default;
            var call__1565 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetMovementSpeed();
            switch (call__1565)
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

        // AoWTC.pas:3804
        public bool MeleeMoveTC(Aow.Units.AoWE.TMovepath Path)
        {
            bool Result = default;
            Aow.Units.CombatTE.TCMeleeMoveTE TE = default;
            Result = false;
            var call__1566 = FUnit.GetPlayer();
            var call__1567 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(call__1566);
            if (call__1567 && (Path.Count > 1))
            {
                var call__1568 = Aow.Units.CombatTE.CombatTE.CreateTCMeleeMoveTE(FUnit, Path);
                TE = call__1568;
                if (TE != null)
                {
                    var call__1569 = FUnit.GetPlayer();
                    var call__1570 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(call__1569);
                    call__1570.AddEvent(TE);
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
                    int end__1571 = FAbilities.Count - 1;
                    for (; I <= end__1571; I += 1)
                    {
                        var call__1572 = FAbilities[I];
                        if (((Aow.Units.AoWTC.TTCAbility)call__1572).Ability == A1)
                        {
                            Index = I;
                        }
                    }
                }

                var call__1573 = FUnit.GetLocked();
                if ((((!call__1573 && (FUnit.FMovePoints > 0)) && (FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)) && (Index < FCombatAbilityCount)) && (Index > -1))
                {
                    if (A1 == 52)
                    {
                        var call__1574 = FUnit.FUnit.GetMoves();
                        if (call__1574 == FUnit.FMovePoints)
                        {
                            var call__1575 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(A1);
                            call__1575.ActivateCombat(FUnit.FUnit, ref Reason);
                            FSelectedSpell = -1;
                            FSelectedAbility = A1;
                            Result = Index;
                        }
                    }
                    else
                    {
                        var call__1576 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(A1);
                        var call__1577 = call__1576.GetCombatMode(FUnit.FUnit);
                        if (TSet<Aow.Units.AoWE.TCombatMode>.Of(Aow.Units.AoWE.TCombatMode.cmShortRange, Aow.Units.AoWE.TCombatMode.cmMediumRange, Aow.Units.AoWE.TCombatMode.cmLongRange).Contains(call__1577))
                        {
                            var call__1578 = FUnit.FUnit.GetMoves();
                            if (call__1578 == FUnit.FMovePoints)
                            {
                                var call__1579 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(A1);
                                var call__1580 = call__1579.GetCombatMode(FUnit.FUnit);
                                if (call__1580 == Aow.Units.AoWE.TCombatMode.cmShortRange)
                                {
                                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FRangedPathHS.FRange = 4;
                                }
                                else
                                {
                                    var call__1581 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(A1);
                                    var call__1582 = call__1581.GetCombatMode(FUnit.FUnit);
                                    if (call__1582 == Aow.Units.AoWE.TCombatMode.cmMediumRange)
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
                int end__1583 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetCount() - 1;
                for (; I <= end__1583; I += 1)
                {
                    var moveSettings = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetMoveSettings(I);
                    moveSettings.FPath.Clear();
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
            var call__1585 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(FSelectedSpell);
            call__1585.GetCombatInfo(FUnit.FUnit, ref Info);
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
                int end__1586 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetCount() - 1;
                for (; I <= end__1586; I += 1)
                {
                    var moveSettings = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitSelection.FMoveSettingsList.GetMoveSettings(I);
                    moveSettings.FPath.Clear();
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
                int end__1588 = FAbilities.Count - 1;
                for (; I <= end__1588; I += 1)
                {
                    var call__1589 = FAbilities[I];
                    ((Aow.Rtl.TObject)call__1589).Free();
                }
            }

            FAbilities.Clear();
            FCombatAbilityCount = 0;
            {
                I = 1;
                byte end__1590 = 169;
                for (; I <= end__1590; I += 1)
                {
                    var call__1591 = FUnit.GetAbilityEnabled(I);
                    if (call__1591)
                    {
                        Ability = new Aow.Units.AoWTC.TTCAbility();
                        Ability.Ability = I;
                        var call__1592 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                        var call__1593 = call__1592.GetControlType(FUnit.FUnit);
                        bool bool__1594 = call__1593.Contains(Aow.Units.AoWE.TAbilityControlTypeElement.acCombatAction);
                        if (bool__1594)
                        {
                            var call__1595 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                            var call__1596 = call__1595.GetCombatMode(FUnit.FUnit);
                            bool__1594 = (call__1596 != Aow.Units.AoWE.TCombatMode.cmMelee);
                        }

                        if (bool__1594)
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
                int end__1597 = FAbilities.Count - 1;
                for (; I <= end__1597; I += 1)
                {
                    var call__1598 = FAbilities[I];
                    var call__1599 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TTCAbility)call__1598).Ability);
                    var call__1600 = call__1599.GetName(FUnit.FUnit);
                    var call__1601 = FAbilities[I];
                    Names.AddObject(call__1600, Aow.Rtl.Rtl.IntToObject((int)((Aow.Units.AoWTC.TTCAbility)call__1601).Ability));
                    if (Types != null)
                    {
                        var call__1602 = FAbilities[I];
                        var call__1603 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TTCAbility)call__1602).Ability);
                        var call__1604 = call__1603.GetAbilityType();
                        var call__1605 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TAbilityTypeRStr[(int)call__1604]);
                        Types.Add(call__1605);
                    }

                    if (Sources != null)
                    {
                        var call__1606 = FAbilities[I];
                        var call__1607 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(((Aow.Units.AoWTC.TTCAbility)call__1606).Ability);
                        var call__1608 = call__1607.GetSourceName(FUnit.FUnit);
                        Sources.Add(call__1608);
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
            var call__1609 = FUnit.GetPlayer();
            var call__1610 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(call__1609);
            if (call__1610 && (Path.Count > 0))
            {
                if (Path.Count > 1)
                {
                    var call__1611 = Aow.Units.CombatTE.CombatTE.CreateTCAbTouchMoveTE(FUnit, Path, FSelectedAbility);
                    TE = call__1611;
                    if (TE != null)
                    {
                        var call__1612 = FUnit.GetPlayer();
                        var call__1613 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(call__1612);
                        call__1613.AddEvent(TE);
                        TE.Release();
                        Result = true;
                    }
                }
                else
                {
                    var call__1614 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(FSelectedAbility);
                    call__1614.GetCombatInfo(FUnit.FUnit, ref Buffer.Info);
                    if (Buffer.Info.TargetTypes.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend))
                    {
                        var call__1615 = Path[0];
                        var call__1616 = Path[0];
                        AbRangedTC(unchecked((ushort)call__1616.X), unchecked((ushort)call__1615.Y));
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
            var call__1617 = FUnit.GetPlayer();
            var call__1618 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(call__1617);
            if (call__1618)
            {
                var call__1619 = Aow.Units.CombatTE.CombatTE.CreateTCAbRangedTE(FUnit, unchecked((ushort)X), unchecked((ushort)Y), FSelectedAbility, FSelectedSpell);
                TE = call__1619;
                if (TE != null)
                {
                    var call__1620 = FUnit.GetPlayer();
                    var call__1621 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(call__1620);
                    call__1621.AddEvent(TE);
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
        // Its guarded legacy factory invokes the inherited body after raw allocation.
        public TCombatUnitSelectionControl() : base(default(Aow.Units.AoWE.RawAoWERuleConstruction)) { }

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
                int end__1622 = FObstacles.Count - 1;
                for (; I <= end__1622; I += 1)
                {
                    var call__1623 = FObstacles[I];
                    ((Aow.Rtl.TObject)call__1623).Free();
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
                var call__1624 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer));
                if (!call__1624.FAuto)
                {
                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Button == Aow.Rtl.TMouseButton.mbRight)
                    {
                        var call__1625 = GetTCUnitHS();
                        bool bool__1626 = (call__1625 != null);
                        if (bool__1626)
                        {
                            var call__1627 = GetTCUnitHS();
                            bool__1626 = (call__1627.FSelectedAbility > -1);
                        }

                        if (bool__1626)
                        {
                            var call__1628 = GetTCUnitHS();
                            call__1628.SelectAbility(-1);
                            return;
                        }

                        Unselect();
                    }
                    else
                    {
                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Shift.Contains(Aow.Rtl.TShiftState.ssShift))
                        {
                            var call__1629 = FMoveSettingsList.GetCount();
                            bool bool__1630 = (call__1629 == 1);
                            if (bool__1630)
                            {
                                var call__1631 = FMoveSettingsList.GetMoveSettings(0);
                                bool__1630 = (call__1631.FPath.Count != 0);
                            }

                            if (bool__1630)
                            {
                                var call__1632 = FMoveSettingsList.GetMoveSettings(0);
                                var call__1633 = call__1632.FPath[0];
                                var call__1634 = FMoveSettingsList.GetMoveSettings(0);
                                var call__1635 = call__1634.FPath[0];
                                var call__1636 = FMoveSettingsList.GetMoveSettings(0);
                                var call__1637 = call__1636.FPath[0];
                                var call__1638 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__1637.X, call__1635.Y, call__1633.L);
                                var call__1639 = ValidMultiMoveSelection((Aow.Units.AoWE.TAoWMapField)call__1638);
                                if (!call__1639)
                                {
                                    return;
                                }
                            }

                            var call__1640 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                            if (call__1640 != -1)
                            {
                                var call__1641 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                var call__1642 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                var call__1643 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__1642, call__1641, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                var call__1644 = call__1643.FindOwnedChild(2228484);
                                UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1644;
                                if ((UnitHS != null) && (UnitHS.FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer))
                                {
                                    var call__1645 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                    Index = call__1645;
                                    if (Index == -1)
                                    {
                                        AddUnit(UnitHS);
                                        Update();
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
                                        var call__1646 = UnitHS.FUnit.GetLocked();
                                        if (call__1646)
                                        {
                                            var call__1647 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                            var call__1648 = FMoveSettingsList.GetMoveSettings(call__1647);
                                            call__1648.FPath.Clear();
                                        }
                                        else
                                        {
                                            var call__1649 = FMoveSettingsList.GetMoveSettings(0);
                                            if (call__1649.FPath.Count > 0)
                                            {
                                                var call__1650 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__1651 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__1652 = call__1651.FPath[call__1650.FPath.Count - 1];
                                                var call__1653 = UnitHS.GetXhx();
                                                DX = call__1653 - call__1652.X;
                                                var call__1654 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__1655 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__1656 = call__1655.FPath[call__1654.FPath.Count - 1];
                                                var call__1657 = UnitHS.GetYhx();
                                                DY = call__1657 - call__1656.Y;
                                                var call__1658 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__1659 = call__1658.FPath[0];
                                                var call__1660 = FMoveSettingsList.GetMoveSettings(0);
                                                var call__1661 = call__1660.FPath[0];
                                                var call__1662 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                                CalculateMovePath(call__1662, unchecked((sbyte)(call__1661.X + DX)), unchecked((sbyte)(call__1659.Y + DY)), 0);
                                            }
                                        }

                                        SetCurrentMove(-1);
                                    }
                                    else
                                    {
                                        UnitHS.Unselect();
                                        DeleteMoveObject(Index);
                                        var call__1663 = FMoveSettingsList.GetCount();
                                        SetCurrentMove(call__1663 - 1);
                                        Update();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Shift.Contains(Aow.Rtl.TShiftState.ssCtrl))
                            {
                                var call__1664 = FMoveSettingsList.GetCount();
                                bool bool__1665 = (call__1664 > 0);
                                if (bool__1665)
                                {
                                    var call__1666 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                    bool__1665 = (call__1666 != -1);
                                }

                                if (bool__1665)
                                {
                                    var call__1667 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                    var call__1668 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                    var call__1669 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__1668, call__1667, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                    var call__1670 = call__1669.FindOwnedChild(2228484);
                                    UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1670;
                                    if ((UnitHS != null) && (UnitHS.FUnit.FCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer))
                                    {
                                        var call__1671 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                        Index = call__1671;
                                        if (Index == -1)
                                        {
                                            AddUnit(UnitHS);
                                            Update();
                                            Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
                                            var call__1672 = UnitHS.FUnit.GetLocked();
                                            if (call__1672)
                                            {
                                                var call__1673 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                                var call__1674 = FMoveSettingsList.GetMoveSettings(call__1673);
                                                call__1674.FPath.Clear();
                                            }
                                            else
                                            {
                                                var call__1675 = FMoveSettingsList.GetMoveSettings(0);
                                                if (call__1675.FPath.Count > 0)
                                                {
                                                    var call__1676 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__1677 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__1678 = call__1677.FPath[call__1676.FPath.Count - 1];
                                                    var call__1679 = UnitHS.GetXhx();
                                                    DX = call__1679 - call__1678.X;
                                                    var call__1680 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__1681 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__1682 = call__1681.FPath[call__1680.FPath.Count - 1];
                                                    var call__1683 = UnitHS.GetYhx();
                                                    DY = call__1683 - call__1682.Y;
                                                    var call__1684 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__1685 = call__1684.FPath[0];
                                                    var call__1686 = FMoveSettingsList.GetMoveSettings(0);
                                                    var call__1687 = call__1686.FPath[0];
                                                    var call__1688 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                                    CalculateMovePath(call__1688, unchecked((sbyte)(call__1687.X + DX)), unchecked((sbyte)(call__1685.Y + DY)), 0);
                                                }
                                            }

                                            var call__1689 = FMoveSettingsList.IndexOfMoveObject(UnitHS);
                                            SetCurrentMove(call__1689);
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
                                var call__1690 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                if (call__1690 != -1)
                                {
                                    if (GetTCUnitHS() != null)
                                    {
                                        var call__1691 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                        var call__1692 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                        var call__1693 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__1692, call__1691, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                        var call__1694 = call__1693.FindOwnedChild(2228484);
                                        UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1694;
                                        var call__1695 = GetTCUnitHS();
                                        if (call__1695.FSelectedAbility == -1)
                                        {
                                            Targets = TSet<Aow.Units.AoWE.TCombatTargetTypeElement>.Of(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy);
                                        }
                                        else
                                        {
                                            var call__1696 = GetTCUnitHS();
                                            if (call__1696.FSelectedAbility == 52)
                                            {
                                                var call__1697 = GetTCUnitHS();
                                                var call__1698 = GetTCUnitHS();
                                                var call__1699 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(call__1698.FSelectedSpell);
                                                call__1699.GetCombatInfo(call__1697.FUnit.FUnit, ref SpellInfo);
                                                Targets = SpellInfo.TargetTypes;
                                            }
                                            else
                                            {
                                                var call__1700 = GetTCUnitHS();
                                                var call__1701 = GetTCUnitHS();
                                                var call__1702 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(call__1701.FSelectedAbility);
                                                call__1702.GetCombatInfo(call__1700.FUnit.FUnit, ref AbilityInfo);
                                                Targets = AbilityInfo.TargetTypes;
                                            }
                                        }

                                        if (UnitHS == null)
                                        {
                                            ExecuteMoveSelection();
                                        }
                                        else
                                        {
                                            var call__1703 = GetTCUnitHS();
                                            var call__1704 = call__1703.FUnit.GetPlayer();
                                            var call__1705 = UnitHS.FUnit.GetPlayer();
                                            if ((call__1705 == call__1704) && !Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend))
                                            {
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.UnselectLocation();
                                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS(UnitHS);
                                            }
                                            else
                                            {
                                                var call__1706 = GetTCUnitHS();
                                                var call__1707 = call__1706.FUnit.GetPlayer();
                                                var call__1708 = UnitHS.FUnit.GetPlayer();
                                                bool bool__1709 = ((call__1708 == call__1707) && Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend));
                                                if (!bool__1709)
                                                {
                                                    var call__1710 = GetTCUnitHS();
                                                    var call__1711 = call__1710.FUnit.GetPlayer();
                                                    var call__1712 = UnitHS.FUnit.GetPlayer();
                                                    bool__1709 = ((call__1712 != call__1711) && Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy));
                                                }

                                                if (bool__1709)
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
                var call__1713 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                var call__1714 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                var call__1715 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__1714, call__1713, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                var call__1716 = call__1715.FindOwnedChild(2228484);
                UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1716;
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
            var moveSettings = FMoveSettingsList.GetMoveSettings(Index);
            Temporary = new Aow.Rtl.TList();
            var call__1718 = FMoveSettingsList.GetCount();
            if (call__1718 > 0)
            {
                {
                    I = 0;
                    int end__1719 = FMoveSettingsList.GetCount() - 1;
                    for (; I <= end__1719; I += 1)
                    {
                        if (I != Index)
                        {
                            var call__1720 = FMoveSettingsList.GetMoveSettings(I);
                            if (call__1720.FPath.Count > 1)
                            {
                                Movement = Aow.Units.FleeLoc.TMovementHS.Create(null);
                                var call__1721 = FMoveSettingsList.GetMoveSettings(I);
                                var call__1722 = FMoveSettingsList.GetMoveSettings(I);
                                var call__1723 = call__1722.FPath[call__1721.FPath.Count - 1];
                                var call__1724 = FMoveSettingsList.GetMoveSettings(I);
                                var call__1725 = FMoveSettingsList.GetMoveSettings(I);
                                var call__1726 = call__1725.FPath[call__1724.FPath.Count - 1];
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(Movement, call__1726.X, call__1723.Y, 0);
                                Movement.FEnabled = true;
                                Temporary.Add(Movement);
                                Movement = Aow.Units.FleeLoc.TMovementHS.Create(null);
                                var call__1727 = FMoveSettingsList.GetMoveSettings(I);
                                var call__1728 = call__1727.FPath[0];
                                var call__1729 = FMoveSettingsList.GetMoveSettings(I);
                                var call__1730 = call__1729.FPath[0];
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(Movement, call__1730.X, call__1728.Y, 0);
                                Movement.FEnabled = false;
                                Temporary.Add(Movement);
                            }
                        }
                    }
                }
            }

            moveSettings.FPathMovePoints = 0;
            UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)moveSettings.FMoveObject;
            var call__1731 = GetTCUnitHS();
            bool bool__1732 = (call__1731 != null);
            if (bool__1732)
            {
                var call__1733 = GetTCUnitHS();
                bool bool__1734 = (call__1733.FSelectedAbility == 117);
                if (!bool__1734)
                {
                    var call__1735 = GetTCUnitHS();
                    bool__1734 = (call__1735.FSelectedAbility == 65);
                }

                bool__1732 = bool__1734;
            }

            if (bool__1732)
            {
                ((Aow.Units.AoWTC.TCombatMoveControlSettings)FMoveControlSettings).FMoveFlags = TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.FromMask((ulong)((byte)FPathInvalid.Image)) + TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.Of(Aow.Units.AoWTC.TCombatMoveModeElement.mmClimbing);
            }
            else
            {
                ((Aow.Units.AoWTC.TCombatMoveControlSettings)FMoveControlSettings).FMoveFlags = TSet<Aow.Units.AoWTC.TCombatMoveModeElement>.FromMask((ulong)((byte)FPathInvalid.Image));
            }

            FMoveControlSettings.Reset();
            var call__1736 = UnitHS.GetLhx();
            var call__1737 = UnitHS.GetYhx();
            var call__1738 = UnitHS.GetXhx();
            FMoveControlSettings.SetSourcePos(call__1738, call__1737, call__1736);
            var call__1739 = UnitHS.FUnit.GetPlayer();
            FMoveControlSettings.FPlayer = unchecked((sbyte)call__1739);
            FMoveControlSettings.FArmyHS = UnitHS;
            FMoveControlSettings.FSelection = 0;
            FMoveControlSettings.SetDestinationPos(X, Y, L);
            UnitHS.FUnit.FUnit.CreateMovePointTable(ref FMoveControlSettings.FMovePointTable);
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.FMoveControl.ProcessMove(FMoveControlSettings);
            var call__1740 = FMoveSettingsList.GetMoveSettings(Index);
            call__1740.FPath.Assign(FMoveControlSettings.FPath);
            var call__1741 = FMoveSettingsList.GetMoveSettings(Index);
            call__1741.FMovePoints = UnitHS.FUnit.FMovePoints;
            var call__1742 = UnitHS.FUnit.GetMoves();
            var call__1743 = FMoveSettingsList.GetMoveSettings(Index);
            call__1743.FMoves = call__1742;
            if (FMoveControlSettings.FPath.Count != 0)
            {
                {
                    I = 0;
                    int end__1744 = FMoveControlSettings.FPath.Count - 2;
                    for (; I <= end__1744; I += 1)
                    {
                        var call__1745 = FMoveControlSettings.FPath[I];
                        var call__1746 = FMoveControlSettings.FPath[I];
                        var call__1747 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__1748 = call__1747.Field[call__1746.X, call__1745.Y];
                        var call__1749 = call__1748.FindOwnedChild(2229508);
                        bool bool__1750 = (call__1749 != null);
                        if (!bool__1750)
                        {
                            var call__1751 = FMoveControlSettings.FPath[I];
                            var call__1752 = FMoveControlSettings.FPath[I];
                            var call__1753 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1754 = call__1753.Field[call__1752.X, call__1751.Y];
                            var call__1755 = call__1754.FindOwnedChild(2229510);
                            bool__1750 = (call__1755 != null);
                        }

                        if (bool__1750)
                        {
                            var call__1756 = FMoveControlSettings.FPath[I];
                            var call__1757 = FMoveControlSettings.FPath[I];
                            var call__1758 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1759 = call__1758.Field[call__1757.X, call__1756.Y];
                            moveSettings.FPath.List[I].Flag = unchecked((sbyte)FMoveControlSettings.FMovePointTable[call__1759.TerrainType][-1 - -1]);
                        }
                        else
                        {
                            var call__1760 = FMoveControlSettings.FPath[I];
                            var call__1761 = FMoveControlSettings.FPath[I];
                            var call__1762 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1763 = call__1762.Field[call__1761.X, call__1760.Y];
                            var call__1764 = FMoveControlSettings.FPath[I];
                            var call__1765 = FMoveControlSettings.FPath[I];
                            var call__1766 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1767 = call__1766.Field[call__1765.X, call__1764.Y];
                            moveSettings.FPath.List[I].Flag = unchecked((sbyte)FMoveControlSettings.FMovePointTable[call__1767.TerrainType][call__1763.OverlayType - -1]);
                        }
                    }
                }

                var call__1768 = moveSettings.FPath.PosToMovePoints(0);
                moveSettings.FPathMovePoints = call__1768;
                FMoveControlSettings.FPath.Clear();
            }

            {
                I = 0;
                int end__1769 = Temporary.Count - 1;
                for (; I <= end__1769; I += 1)
                {
                    var call__1770 = Temporary[I];
                    ((Aow.Rtl.TObject)call__1770).Free();
                }
            }

            Temporary.Free();
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
            var call__1771 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
            if (call__1771 == -1)
            {
                return;
            }

            var call__1772 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
            if (call__1772 == -1)
            {
                return;
            }

            if (UnitHS == null)
            {
                return;
            }

            var call__1773 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
            var call__1774 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
            var call__1775 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__1774, call__1773, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
            Field = (Aow.Units.AoWE.TAoWMapField)call__1775;
            bool bool__1776 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
            if (bool__1776)
            {
                var call__1777 = Field.GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                bool__1776 = !call__1777;
            }

            if (bool__1776)
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
                    var call__1778 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(UnitHS.FSelectedSpell);
                    call__1778.GetCombatInfo(UnitHS.FUnit.FUnit, ref SpellInfo);
                    Targets = SpellInfo.TargetTypes;
                }
                else
                {
                    var call__1779 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                    call__1779.GetCombatInfo(UnitHS.FUnit.FUnit, ref AbilityInfo.Data);
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
                    var call__1780 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                    var call__1781 = call__1780.GetCombatMode(UnitHS.FUnit.FUnit);
                    if (call__1781 == Aow.Units.AoWE.TCombatMode.cmTouch)
                    {
                        bool bool__1782 = Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend);
                        if (bool__1782)
                        {
                            var call__1783 = UnitHS.FUnit.GetSide();
                            var call__1784 = TargetHS.FUnit.GetSide();
                            bool__1782 = (call__1783 == call__1784);
                        }

                        bool bool__1785 = bool__1782;
                        if (!bool__1785)
                        {
                            bool bool__1786 = Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy);
                            if (bool__1786)
                            {
                                var call__1787 = UnitHS.FUnit.GetSide();
                                var call__1788 = TargetHS.FUnit.GetSide();
                                bool__1786 = (call__1787 != call__1788);
                            }

                            bool__1785 = bool__1786;
                        }

                        bool bool__1789 = bool__1785;
                        if (bool__1789)
                        {
                            var call__1790 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                            var call__1791 = ((Aow.Units.AoWE.TTouchAbility)call__1790).CanTouch(UnitHS.FUnit, TargetHS.FUnit);
                            bool__1789 = call__1791;
                        }

                        if (bool__1789)
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
                        bool bool__1792 = Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttFriend);
                        if (bool__1792)
                        {
                            var call__1793 = UnitHS.FUnit.GetSide();
                            var call__1794 = TargetHS.FUnit.GetSide();
                            bool__1792 = (call__1793 == call__1794);
                        }

                        bool bool__1795 = bool__1792;
                        if (!bool__1795)
                        {
                            bool bool__1796 = Targets.Contains(Aow.Units.AoWE.TCombatTargetTypeElement.cttEnemy);
                            if (bool__1796)
                            {
                                var call__1797 = UnitHS.FUnit.GetSide();
                                var call__1798 = TargetHS.FUnit.GetSide();
                                bool__1796 = (call__1797 != call__1798);
                            }

                            bool__1795 = bool__1796;
                        }

                        if (bool__1795)
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
                    var call__1799 = TargetHS.FUnit.GetPlayer();
                    var call__1800 = UnitHS.FUnit.GetPlayer();
                    if (call__1799 == call__1800)
                    {
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.CursorMode = 257;
                    }
                    else
                    {
                        var call__1801 = UnitHS.FUnit.GetAbilityEnabled(1);
                        bool bool__1802 = call__1801;
                        if (!bool__1802)
                        {
                            var call__1803 = TargetHS.FUnit.GetAbilityEnabled(1);
                            bool__1802 = !call__1803;
                        }

                        if (bool__1802)
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

            var call__1804 = UnitHS.FUnit.GetPlayer();
            if (call__1804 == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer)
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
                        bool bool__1805 = !(Wall is Aow.Units.CityWall.TCityWallDoor);
                        if (!bool__1805)
                        {
                            var call__1806 = UnitHS.FUnit.GetSide();
                            bool__1805 = (call__1806 == Aow.Units.AoWE.TCombatSideIndex.csAttacker);
                        }

                        if (bool__1805)
                        {
                            Cost = -1;
                        }

                        bool bool__1807 = !(Wall is Aow.Units.CityWall.TCityWallDoor);
                        if (bool__1807)
                        {
                            var call__1808 = UnitHS.FUnit.FUnit.GetAbilityEnabled(36);
                            bool__1807 = call__1808;
                        }

                        if (bool__1807)
                        {
                            Cost = 1;
                        }

                        var call__1809 = UnitHS.FUnit.FUnit.GetAbilityEnabled(66);
                        bool bool__1810 = call__1809;
                        if (!bool__1810)
                        {
                            var call__1811 = UnitHS.FUnit.FUnit.GetAbilityEnabled(1);
                            bool__1810 = call__1811;
                        }

                        if (bool__1810)
                        {
                            Cost = 1;
                        }
                    }
                }

                bool bool__1812 = (Cost <= 0);
                if (!bool__1812)
                {
                    bool bool__1813 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
                    if (bool__1813)
                    {
                        var call__1814 = Field.GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                        bool__1813 = !call__1814;
                    }

                    bool__1812 = bool__1813;
                }

                if (bool__1812)
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
            Result = new Aow.Units.AoWTC.TCombatMoveControlSettings(null);
            return Result;
        }

        // AoWTC.pas:2754
        public override void SetCurrentMove(int A1)
        {
            bool bool__1815 = (FCurrentMove != -1);
            if (bool__1815)
            {
                var call__1816 = GetTCUnitHS();
                bool__1815 = (call__1816 != null);
            }

            if (bool__1815)
            {
                var call__1817 = GetTCUnitHS();
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.TriggerDeselectEvent(call__1817);
            }

            base.SetCurrentMove(A1);
            bool bool__1818 = (FCurrentMove != -1);
            if (bool__1818)
            {
                var call__1819 = GetTCUnitHS();
                bool__1818 = (call__1819 != null);
            }

            if (bool__1818)
            {
                var call__1820 = GetTCUnitHS();
                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.TriggerSelectEvent(call__1820);
            }
        }

        // AoWTC.pas:3316
        public override void Unselect()
        {
            DeactivateRangedPath();
            if (GetTCUnitHS() != null)
            {
                var call__1821 = GetTCUnitHS();
                call__1821.FSelectedAbility = -1;
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
                var call__1822 = GetTCUnitHS();
                bool bool__1823 = (call__1822.FSelectedAbility != 65);
                if (bool__1823)
                {
                    var call__1824 = GetTCUnitHS();
                    bool__1823 = (call__1824.FSelectedAbility != 114);
                }

                if (bool__1823)
                {
                    return;
                }

                var call__1825 = FMoveSettingsList.GetCount();
                bool bool__1826 = (call__1825 == 1);
                if (bool__1826)
                {
                    var call__1827 = FMoveSettingsList.GetMoveSettings(0);
                    bool__1826 = (call__1827.FPath.Count > 1);
                }

                if (bool__1826)
                {
                    {
                        I = 1;
                        byte end__1828 = 6;
                        for (; I <= end__1828; I += 1)
                        {
                            var call__1829 = FMoveSettingsList.GetMoveSettings(0);
                            var call__1830 = call__1829.FPath[1];
                            var call__1831 = FMoveSettingsList.GetMoveSettings(0);
                            var call__1832 = call__1831.FPath[1];
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__1832.X, call__1830.Y, I, ref X, ref Y);
                            Aow.Units.HSEngine.HSEngine.HXtoHP(X, Y, ref Xhp, ref Yhp);
                            var call__1833 = FTargetImages[0];
                            call__1833.Show((Xhp - Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewXhp) - 8, (Yhp - Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewYhp) + 2, Surface);
                        }
                    }
                }
            }
        }

        // AoWTC.pas:2723
        public void Create__init_TCombatUnitSelectionControl()
        {
            base.Create__init_TMoveSelector();
            var call__1834 = new Aow.Units.aowFX.TXYZList();
            FRangedPath = call__1834;
            var call__1835 = new Aow.Rtl.TList();
            FObstacles = call__1835;
            var call__1836 = Aow.Units.AoWTC.TRangedPathHS.Create(null);
            FRangedPathHS = call__1836;
            FRangedPathHS.Activate();
            var call__1837 = Aow.Units.AoWE.AoWE.AoWHSSet.ImageLibraryManager.LinkToIL(TSet<Aow.Units.ImageLib.TILManagerCategoryElement>.Of(Aow.Units.ImageLib.TILManagerCategoryElement.icDisk), (AnsiString)"TargHex.ILB");
            FTargetImages = call__1837;
        }

        // AoWTC.pas:2747
        public int AddUnit(Aow.Units.AoWTC.TTacticalCombatUnitHS U)
        {
            int Result = default;
            Result = AddMoveObject(U);
            var call__1838 = FMoveSettingsList.GetMoveSettings(Result);
            call__1838.FMovePoints = U.FUnit.FMovePoints;
            var call__1839 = U.FUnit.GetMoves();
            var call__1840 = FMoveSettingsList.GetMoveSettings(Result);
            call__1840.FMoves = call__1839;
            return Result;
        }

        // AoWTC.pas:2763
        public Aow.Units.AoWTC.TTacticalCombatUnitHS GetTCUnitHS()
        {
            Aow.Units.AoWTC.TTacticalCombatUnitHS Result = default;
            if (FCurrentMove != -1)
            {
                var call__1841 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                Result = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1841.FMoveObject;
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
                var call__1842 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                var call__1843 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1842.FMoveObject).FUnit.FUnit.GetMoveTypes();
                Result = call__1843;
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
            var call__1844 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL];
            Level = call__1844;
            Aow.Units.HSEngine.HSEngine.HPtoHX(R.Left, R.Top, ref X1, ref Y1);
            Aow.Units.HSEngine.HSEngine.HPtoHX(R.Right, R.Bottom, ref X2, ref Y2);
            {
                X = X1 - 1;
                int end__1845 = X2 + 1;
                for (; X <= end__1845; X += 1)
                {
                    {
                        Y = Y1 - 1;
                        int end__1846 = Y2 + 1;
                        for (; Y <= end__1846; Y += 1)
                        {
                            Aow.Units.HSEngine.HSEngine.HXtoHP(X, Y, ref PX, ref PY);
                            PX += 16;
                            PY += 8;
                            if ((((((((X >= 0) && (X < Level.Width)) && (Y > 0)) && (Y < Level.Height)) && (PX >= R.Left)) && (PX < R.Right)) && (PY >= R.Top)) && (PY < R.Bottom))
                            {
                                var call__1847 = Level.Field[X, Y];
                                Field = call__1847;
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
                    int end__1848 = Units.Count - 1;
                    for (; I <= end__1848; I += 1)
                    {
                        var call__1849 = Units[I];
                        AddUnit((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1849);
                        var call__1850 = Units[I];
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHS((Aow.Units.HSEngine.THexagonSprite)call__1850);
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
                        var call__1851 = FRangedPath.Count;
                        if (call__1851 == 0)
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
                int end__1852 = FObstacles.Count - 1;
                for (; Direction <= end__1852; Direction += 1)
                {
                    var call__1853 = FObstacles[Direction];
                    ((Aow.Rtl.TObject)call__1853).Free();
                }
            }

            FObstacles.Clear();
            UnitHS = GetTCUnitHS();
            var call__1854 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
            var call__1855 = call__1854.Field[X, Y];
            if (call__1855.TerrainType != 15)
            {
                if (UnitHS.FSelectedAbility == 52)
                {
                    var call__1856 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(UnitHS.FSelectedSpell);
                    call__1856.GetCombatInfo(UnitHS.FUnit.FUnit, ref Info);
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
                    var call__1857 = GetTCUnitHS();
                    var call__1858 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                    var call__1859 = call__1858.GetCombatMode(call__1857.FUnit.FUnit);
                    if (call__1859 == Aow.Units.AoWE.TCombatMode.cmLongRange)
                    {
                        Range = 12;
                    }
                    else
                    {
                        var call__1860 = GetTCUnitHS();
                        var call__1861 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                        var call__1862 = call__1861.GetCombatMode(call__1860.FUnit.FUnit);
                        if (call__1862 == Aow.Units.AoWE.TCombatMode.cmMediumRange)
                        {
                            Range = 8;
                        }
                        else
                        {
                            Range = 4;
                        }
                    }
                }

                var call__1863 = UnitHS.FUnit.GetMoves();
                bool bool__1864 = (call__1863 == UnitHS.FUnit.FMovePoints) && (X > 0);
                if (bool__1864)
                {
                    var call__1865 = UnitHS.GetYhx();
                    var call__1866 = UnitHS.GetXhx();
                    var call__1867 = Aow.Units.HSEngine.HSEngine.dHXtoRad(X, Y, call__1866, call__1865);
                    bool__1864 = (call__1867 <= Range);
                }

                if (bool__1864)
                {
                    // { Original retains this unused with temporary. }
                    var tacticalCombatUnitHS = GetTCUnitHS();
                    FTargetX = X;
                    FTargetY = Y;
                    var call__1869 = UnitHS.GetYhx();
                    var call__1870 = UnitHS.GetXhx();
                    var call__1871 = Aow.Units.HSEngine.HSEngine.dHXtoHN(call__1870, call__1869, X, Y);
                    Direction = call__1871;
                    NX = X;
                    NY = Y;
                    var call__1872 = GetTCUnitHS();
                    switch (call__1872.FSelectedAbility)
                    {
                        case 30:
                        case >= 86 and <= 90:
                            var call__1873 = UnitHS.GetYhx();
                            var call__1874 = UnitHS.GetXhx();
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__1874, call__1873, Aow.Units.AoWTC.AoWTC.BreathHN[(Aow.Units.AoWTC.AoWTC.BreathDirection[Direction - 1] * 12) - 12], ref NX, ref NY);
                            break;
                        case 52:
                            if (Aow.Units.AoWTC.AoWTC.SpellTypes[UnitHS.FSelectedSpell] == Aow.Units.AoWTC.TfxType.fxBreath)
                            {
                                var call__1875 = UnitHS.GetYhx();
                                var call__1876 = UnitHS.GetXhx();
                                Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__1876, call__1875, Aow.Units.AoWTC.AoWTC.BreathHN[(Aow.Units.AoWTC.AoWTC.BreathDirection[Direction - 1] * 12) - 12], ref NX, ref NY);
                            }

                            break;
                    }

                    var call__1877 = GetTCUnitHS();
                    var call__1878 = call__1877.GetXhx();
                    var call__1879 = GetTCUnitHS();
                    var call__1880 = call__1879.GetYhx();
                    var call__1881 = GetTCUnitHS();
                    var call__1882 = call__1881.FUnit.GetSide();
                    var call__1883 = GetTCUnitHS();
                    var call__1884 = GetTCUnitHS();
                    Aow.Units.AoWTC.AoWTC.MakeAbilPath(call__1882, unchecked((short)call__1884.FSelectedAbility), unchecked((short)call__1883.FSelectedSpell), unchecked((short)call__1878), unchecked((short)call__1880), unchecked((short)NX), unchecked((short)NY), ref FRangedPath, ref FObstacles);
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
            var call__1885 = FMoveSettingsList.GetCount();
            if (call__1885 > 0)
            {
                if (GetTCUnitHS() != null)
                {
                    var call__1886 = FMoveSettingsList.GetMoveSettings(0);
                    if (call__1886.FPath.Count > 0)
                    {
                        var moveSettings = FMoveSettingsList.GetMoveSettings(0);
                        UnitHS = GetTCUnitHS();
                        if (UnitHS.FSelectedAbility != 117)
                        {
                            var call__1888 = moveSettings.FPath[0];
                            var call__1889 = moveSettings.FPath[0];
                            var call__1890 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1891 = call__1890.Field[call__1889.X, call__1888.Y];
                            var call__1892 = call__1891.FindOwnedChild(2228484);
                            Target = call__1892;
                        }
                        else
                        {
                            var call__1893 = moveSettings.FPath[0];
                            var call__1894 = moveSettings.FPath[0];
                            var call__1895 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__1896 = call__1895.Field[call__1894.X, call__1893.Y];
                            var call__1897 = call__1896.FindOwnedChild(2229508);
                            Target = call__1897;
                            if (Target == null)
                            {
                                var call__1898 = moveSettings.FPath[0];
                                var call__1899 = moveSettings.FPath[0];
                                var call__1900 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__1901 = call__1900.Field[call__1899.X, call__1898.Y];
                                var call__1902 = call__1901.FindOwnedChild(2229510);
                                Target = call__1902;
                            }
                        }

                        var call__1903 = FMoveSettingsList.GetMoveSettings(0);
                        var call__1904 = call__1903.FPath.MovePointsToPos(UnitHS.FUnit.FMovePoints);
                        if ((call__1904 == 0) && (((Target != null) || (UnitHS.FSelectedAbility == 65)) || (UnitHS.FSelectedAbility == 114)))
                        {
                            var call__1905 = GetTCUnitHS();
                            MovePoints = call__1905.FUnit.FMovePoints;
                            if (MovePoints > 0)
                            {
                                if (UnitHS.FSelectedAbility == -1)
                                {
                                    var call__1906 = GetTCUnitHS();
                                    var call__1907 = call__1906.MeleeMoveTC(moveSettings.FPath);
                                    if (call__1907)
                                    {
                                        moveSettings.FPath.Clear();
                                    }
                                }
                                else
                                {
                                    var call__1908 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(UnitHS.FSelectedAbility);
                                    var call__1909 = call__1908.GetCombatMode(UnitHS.FUnit.FUnit);
                                    if (call__1909 == Aow.Units.AoWE.TCombatMode.cmTouch)
                                    {
                                        var call__1910 = GetTCUnitHS();
                                        var call__1911 = call__1910.AbTouchMoveTC(moveSettings.FPath);
                                        if (call__1911)
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
                            var call__1912 = Aow.Units.CombatTE.CombatTE.MoveCombatUnit(UnitHS.FUnit, moveSettings.FPath);
                            if (call__1912)
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
            int I = default;
            var call__1913 = FMoveSettingsList.GetCount();
            if (call__1913 > 0)
            {
                var call__1914 = FMoveSettingsList.GetCount();
                if (call__1914 > 1)
                {
                    var call__1915 = Aow.Units.CombatTE.CombatTE.SetupMultiTCUnitMove(FMoveSettingsList);
                    if (call__1915)
                    {
                        {
                            I = 0;
                            int end__1916 = FMoveSettingsList.GetCount() - 1;
                            for (; I <= end__1916; I += 1)
                            {
                                var moveSettings = FMoveSettingsList.GetMoveSettings(I);
                                moveSettings.FPath.Clear();
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
                var call__1918 = FMoveSettingsList.GetMoveSettings(0);
                bool bool__1919 = (call__1918.FPath.Count > 0);
                if (bool__1919)
                {
                    var call__1920 = FMoveSettingsList.GetMoveSettings(0);
                    var call__1921 = call__1920.FPath[0];
                    var call__1922 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                    bool__1919 = (call__1921.X == call__1922);
                }

                bool bool__1923 = bool__1919;
                if (bool__1923)
                {
                    var call__1924 = FMoveSettingsList.GetMoveSettings(0);
                    var call__1925 = call__1924.FPath[0];
                    var call__1926 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                    bool__1923 = (call__1925.Y == call__1926);
                }

                Result = bool__1923;
                var call__1927 = FMoveSettingsList.GetCount();
                I = call__1927;
                while (Result && (I != 0))
                {
                    I -= 1;
                    var moveSettings = FMoveSettingsList.GetMoveSettings(I);
                    Result = moveSettings.FPath.Count != 0;
                }

                return Result;
            }

            DeactivateRangedPath();
            var call__1929 = FMoveSettingsList.GetCount();
            if (call__1929 > 1)
            {
                if (FCurrentMove == -1)
                {
                    if (!AllPathsValid())
                    {
                        bool bool__1930 = !Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled;
                        if (!bool__1930)
                        {
                            var call__1931 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                            var call__1932 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                            var call__1933 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__1932, call__1931, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                            var call__1934 = ((Aow.Units.AoWE.TAoWMapField)call__1933).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                            bool__1930 = call__1934;
                        }

                        if (bool__1930)
                        {
                            {
                                I = 0;
                                int end__1935 = FMoveSettingsList.GetCount() - 1;
                                for (; I <= end__1935; I += 1)
                                {
                                    var call__1936 = FMoveSettingsList.GetMoveSettings(I);
                                    call__1936.FPath.Clear();
                                }
                            }

                            {
                                A = 0;
                                int end__1937 = FMoveSettingsList.GetCount() - 1;
                                for (; A <= end__1937; A += 1)
                                {
                                    {
                                        B = A + 1;
                                        int end__1938 = FMoveSettingsList.GetCount() - 1;
                                        for (; B <= end__1938; B += 1)
                                        {
                                            var call__1939 = FMoveSettingsList.GetMoveSettings(B);
                                            var call__1940 = ((Aow.Units.HSEngine.THexagonSprite)call__1939.FMoveObject).GetYhx();
                                            var call__1941 = FMoveSettingsList.GetMoveSettings(B);
                                            var call__1942 = ((Aow.Units.HSEngine.THexagonSprite)call__1941.FMoveObject).GetXhx();
                                            var call__1943 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                            var call__1944 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                            var call__1945 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__1944, call__1943, call__1942, call__1940);
                                            var call__1946 = FMoveSettingsList.GetMoveSettings(A);
                                            var call__1947 = ((Aow.Units.HSEngine.THexagonSprite)call__1946.FMoveObject).GetYhx();
                                            var call__1948 = FMoveSettingsList.GetMoveSettings(A);
                                            var call__1949 = ((Aow.Units.HSEngine.THexagonSprite)call__1948.FMoveObject).GetXhx();
                                            var call__1950 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                            var call__1951 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                            var call__1952 = Aow.Units.HSEngine.HSEngine.dHXtoRad(call__1951, call__1950, call__1949, call__1947);
                                            if (call__1945 < call__1952)
                                            {
                                                FMoveSettingsList.FChildren.Exchange(A, B);
                                            }
                                        }
                                    }
                                }
                            }

                            var call__1953 = FMoveSettingsList.GetMoveSettings(0);
                            var call__1954 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1953.FMoveObject).FUnit.GetLocked();
                            if (call__1954)
                            {
                                var call__1955 = FMoveSettingsList.GetMoveSettings(0);
                                call__1955.FPath.Clear();
                            }
                            else
                            {
                                var call__1956 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                var call__1957 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                CalculateMovePath(0, unchecked((sbyte)call__1957), unchecked((sbyte)call__1956), unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL));
                            }

                            var call__1958 = FMoveSettingsList.GetMoveSettings(0);
                            if (call__1958.FPath.Count > 1)
                            {
                                {
                                    I = 1;
                                    int end__1959 = FMoveSettingsList.GetCount() - 1;
                                    for (; I <= end__1959; I += 1)
                                    {
                                        var call__1960 = FMoveSettingsList.GetMoveSettings(I);
                                        var call__1961 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1960.FMoveObject).FUnit.GetLocked();
                                        if (call__1961)
                                        {
                                            var call__1962 = FMoveSettingsList.GetMoveSettings(I);
                                            call__1962.FPath.Clear();
                                        }
                                        else
                                        {
                                            var call__1963 = FMoveSettingsList.GetMoveSettings(I);
                                            var call__1964 = ((Aow.Units.HSEngine.THexagonSprite)call__1963.FMoveObject).GetYhx();
                                            var call__1965 = FMoveSettingsList.GetMoveSettings(I);
                                            var call__1966 = ((Aow.Units.HSEngine.THexagonSprite)call__1965.FMoveObject).GetXhx();
                                            var call__1967 = FMoveSettingsList.GetMoveSettings(0);
                                            var call__1968 = ((Aow.Units.HSEngine.THexagonSprite)call__1967.FMoveObject).GetYhx();
                                            var call__1969 = FMoveSettingsList.GetMoveSettings(0);
                                            var call__1970 = ((Aow.Units.HSEngine.THexagonSprite)call__1969.FMoveObject).GetXhx();
                                            var call__1971 = Aow.Units.HSEngine.HSEngine.dHXtoHN(call__1970, call__1968, call__1966, call__1964);
                                            Direction = call__1971;
                                            var call__1972 = FMoveSettingsList.GetMoveSettings(0);
                                            var call__1973 = call__1972.FPath[0];
                                            var call__1974 = FMoveSettingsList.GetMoveSettings(0);
                                            var call__1975 = call__1974.FPath[0];
                                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__1975.X, call__1973.Y, Direction, ref X, ref Y);
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
                                int end__1976 = FMoveSettingsList.GetCount() - 1;
                                for (; I <= end__1976; I += 1)
                                {
                                    var call__1977 = FMoveSettingsList.GetMoveSettings(I);
                                    call__1977.FPath.Clear();
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
                    var moveSettings2 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                    bool bool__1979 = (moveSettings2.FPath.Count != 0);
                    if (bool__1979)
                    {
                        var call__1980 = moveSettings2.FPath[0];
                        var call__1981 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                        bool__1979 = (call__1980.X == call__1981);
                    }

                    bool bool__1982 = bool__1979;
                    if (bool__1982)
                    {
                        var call__1983 = moveSettings2.FPath[0];
                        var call__1984 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                        bool__1982 = (call__1983.Y == call__1984);
                    }

                    bool bool__1985 = bool__1982;
                    if (bool__1985)
                    {
                        var call__1986 = moveSettings2.FPath[0];
                        bool__1985 = (call__1986.L == Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                    }

                    if (bool__1985)
                    {
                        MoveUnits();
                        return;
                    }

                    var call__1987 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                    var call__1988 = ((Aow.Units.AoWTC.TTacticalCombatUnitHS)call__1987.FMoveObject).FUnit.GetLocked();
                    if (call__1988)
                    {
                        var call__1989 = FMoveSettingsList.GetMoveSettings(FCurrentMove);
                        call__1989.FPath.Clear();
                    }
                    else
                    {
                        var call__1990 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                        var call__1991 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                        CalculateMovePath(FCurrentMove, unchecked((sbyte)call__1991), unchecked((sbyte)call__1990), unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL));
                    }
                }
            }
            else
            {
                var call__1992 = GetTCUnitHS();
                bool bool__1993 = (call__1992.FSelectedAbility == -1);
                if (bool__1993)
                {
                    var call__1994 = GetTCUnitHS();
                    var call__1995 = call__1994.GetXhx();
                    var call__1996 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                    bool__1993 = (call__1995 == call__1996);
                }

                bool bool__1997 = bool__1993;
                if (bool__1997)
                {
                    var call__1998 = GetTCUnitHS();
                    var call__1999 = call__1998.GetYhx();
                    var call__2000 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                    bool__1997 = (call__1999 == call__2000);
                }

                if (bool__1997)
                {
                    var call__2001 = GetTCUnitHS();
                    var call__2002 = call__2001.GetLhx();
                    var call__2003 = GetTCUnitHS();
                    var call__2004 = call__2003.GetYhx();
                    var call__2005 = GetTCUnitHS();
                    var call__2006 = call__2005.GetXhx();
                    Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHX(call__2006, call__2004, call__2002);
                    return;
                }

                var call__2007 = GetTCUnitHS();
                var call__2008 = call__2007.FUnit.GetLocked();
                bool bool__2009 = !call__2008;
                if (bool__2009)
                {
                    var call__2010 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentPlayer);
                    bool__2009 = call__2010;
                }

                if (bool__2009)
                {
                    UnitHS = GetTCUnitHS();
                    if (AllPathsValid())
                    {
                        MoveUnits();
                    }
                    else
                    {
                        var call__2011 = FRangedPath.Count;
                        bool bool__2012 = (call__2011 > 0);
                        if (bool__2012)
                        {
                            var call__2013 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                            bool__2012 = (FTargetX == call__2013);
                        }

                        bool bool__2014 = bool__2012;
                        if (bool__2014)
                        {
                            var call__2015 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                            bool__2014 = (FTargetY == call__2015);
                        }

                        if (bool__2014)
                        {
                            var call__2016 = UnitHS.AbRangedTC(unchecked((ushort)FTargetX), unchecked((ushort)FTargetY));
                            if (call__2016)
                            {
                                FRangedPath.Clear();
                                UpdateMoveCursor();
                            }
                        }
                        else
                        {
                            bool bool__2017 = (UnitHS.FSelectedAbility == -1);
                            if (bool__2017)
                            {
                                var call__2018 = UnitHS.GetXhx();
                                var call__2019 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                bool__2017 = (call__2018 == call__2019);
                            }

                            bool bool__2020 = bool__2017;
                            if (bool__2020)
                            {
                                var call__2021 = UnitHS.GetYhx();
                                var call__2022 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                bool__2020 = (call__2021 == call__2022);
                            }

                            if (bool__2020)
                            {
                                var call__2023 = UnitHS.GetLhx();
                                var call__2024 = UnitHS.GetYhx();
                                var call__2025 = UnitHS.GetXhx();
                                Aow.Units.AoWTC.AoWTC.AoWCombatMap.Selection.SelectLocationHX(call__2025, call__2024, call__2023);
                                return;
                            }

                            if (!UnitHS.Moving())
                            {
                                if (UnitHS.FSelectedAbility == -1)
                                {
                                    bool bool__2026 = (UnitHS.FUnit.FMovePoints > 0);
                                    if (bool__2026)
                                    {
                                        var call__2027 = UnitHS.FUnit.GetLocked();
                                        bool__2026 = !call__2027;
                                    }

                                    if (bool__2026)
                                    {
                                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FExplorationEnabled)
                                        {
                                            var call__2028 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                            var call__2029 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                            var call__2030 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__2029, call__2028, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                            var call__2031 = ((Aow.Units.AoWE.TAoWMapField)call__2030).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                                            if (!call__2031)
                                            {
                                                return;
                                            }
                                        }

                                        var call__2032 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                        var call__2033 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                        CalculateMovePath(FCurrentMove, unchecked((sbyte)call__2033), unchecked((sbyte)call__2032), 0);
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
                                                var call__2034 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                                var call__2035 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                                var call__2036 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetField(call__2035, call__2034, Aow.Units.AoWTC.AoWTC.AoWCombatMap.ViewL);
                                                var call__2037 = ((Aow.Units.AoWE.TAoWMapField)call__2036).GetExplored(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                                                if (!call__2037)
                                                {
                                                    return;
                                                }
                                            }

                                            var call__2038 = UnitHS.FUnit.GetLocked();
                                            if (!call__2038)
                                            {
                                                var call__2039 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                                var call__2040 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                                CalculateMovePath(0, unchecked((sbyte)call__2040), unchecked((sbyte)call__2039), 0);
                                            }

                                            break;
                                        default:
                                        {
                                            var call__2041 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                            var call__2042 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                            CalculateRangedPath(unchecked((sbyte)call__2042), unchecked((sbyte)call__2041));
                                            var call__2043 = FRangedPath.Count;
                                            if (call__2043 > 0)
                                            {
                                                var call__2044 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Yhx;
                                                var call__2045 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.Mouse.Xhx;
                                                var call__2046 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                var call__2047 = call__2046.Field[call__2045, call__2044];
                                                var call__2048 = call__2047.FindOwnedChild(2228484);
                                                FMouseOverTarget = (Aow.Units.HSEngine.THexagonSprite)call__2048;
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
                var call__2049 = GetTCUnitHS();
                call__2049.FUnit.FUnit.CreateMovePointTable(ref FMoveControlSettings.FMovePointTable);
            }

            FPathInvalid = TSet<Aow.Units.AoWTC.AnonymousEnum_piMove>.Of(Aow.Units.AoWTC.AnonymousEnum_piMove.piRanged);
        }

        // AoWTC.pas:3300
        public void SetTCUnitHS(Aow.Units.AoWTC.TTacticalCombatUnitHS U)
        {
            if (GetTCUnitHS() != U)
            {
                var call__2050 = FMoveSettingsList.GetCount();
                if (call__2050 <= 1)
                {
                    Clear();
                    bool bool__2051 = (U != null);
                    if (bool__2051)
                    {
                        var call__2052 = U.FUnit.GetPlayer();
                        bool__2051 = (call__2052 == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedPlayer);
                    }

                    if (bool__2051)
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
        public static Aow.Units.AoWTC.TCombatUnitSelectionControl Create()
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
        public TTacticalCombatPlayer(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

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
                var call__2053 = FPlayers.GetCount();
                Result = call__2053;
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
            var call__2054 = FPlayers.GetPlayers(unchecked((sbyte)Index));
            Result = (Aow.Units.AoWTC.TTacticalCombatPlayer)call__2054;
            return Result;
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
                var call__2055 = FEffects[0];
                DynamicFixedVisibleHS.RemoveChild((Aow.Units.HSEngine.THexagonSprite)call__2055);
                var call__2056 = FEffects[0];
                DynamicVisibleHS.RemoveChild((Aow.Units.HSEngine.THexagonSprite)call__2056);
                var call__2057 = FEffects[0];
                ((Aow.Rtl.TObject)call__2057).Free();
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
            var call__2058 = new Aow.Units.AoWTC.TUnitPositionControl(null);
            FUnitPositions = call__2058;
            var call__2059 = new Aow.Units.Engine.TEventList();
            FChangeTurnEvents = call__2059;
            var call__2060 = new Aow.Units.Engine.TEventList();
            FPlayerChangedEvents = call__2060;
            var call__2061 = new Aow.Units.Engine.TEventList();
            FSeatedPlayerChangedEvents = call__2061;
            var call__2062 = Aow.Units.AoWTC.TCombatMoveControl.Create(this);
            FMoveControl = call__2062;
            var call__2063 = new Aow.Units.Engine.TEventList();
            FSelectEvents = call__2063;
            var call__2064 = new Aow.Units.Engine.TEventList();
            FDeselectEvents = call__2064;
            var call__2065 = new Aow.Units.Engine.TEventList();
            FMouseOverTargetEvents = call__2065;
            var call__2066 = new Aow.Units.Engine.TEventList();
            FUnitChangedEvents = call__2066;
            var call__2067 = new Aow.Units.AoWTC.TTacticalCombatPlayerList();
            FPlayers = call__2067;
            var call__2068 = Aow.Units.AoWTC.TCombatUnitSelectionControl.Create();
            FUnitSelection = call__2068;
            ((Aow.Units.AoWTC.TAoWCombatMapMouse)Mouse).FRectangleSelectEvents.Add(FUnitSelection.RectangleSelectEvent);
            var call__2069 = new Aow.Rtl.TList();
            FEffects = call__2069;
            var call__2070 = new Aow.Rtl.TList();
            FWalls = call__2070;
            var call__2071 = new Aow.Rtl.TList();
            FFleeLocations = call__2071;
            var call__2072 = new Aow.Units.AoWTC.TCAI();
            FAI = call__2072;
            Mouse.SetCapture(this);
            Mouse.OnMove.Add(MouseMoveEvent);
            Mouse.CursorMode = 256;
            {
                I = 0;
                byte end__2073 = 7;
                for (; I <= end__2073; I += 1)
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
                int end__2074 = MapContainer.Width - 7;
                for (; X <= end__2074; X += 1)
                {
                    {
                        Y = 6;
                        int end__2075 = MapContainer.Height - 7;
                        for (; Y <= end__2075; Y += 1)
                        {
                            Aow.Units.AoWE.AoWE.AoWHSSet.FCombatHexagons.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 1);
                        }
                    }
                }
            }

            Control = Engine.HSSet.HexagonControl;
            {
                X = 0;
                int end__2077 = MapContainer.Width - 1;
                for (; X <= end__2077; X += 1)
                {
                    {
                        Y = 0;
                        byte end__2078 = 5;
                        for (; Y <= end__2078; Y += 1)
                        {
                            Control.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 15);
                        }
                    }

                    {
                        Y = MapContainer.Height - 6;
                        int end__2079 = MapContainer.Height - 1;
                        for (; Y <= end__2079; Y += 1)
                        {
                            Control.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 15);
                        }
                    }
                }
            }

            {
                Y = 6;
                int end__2080 = MapContainer.Height - 7;
                for (; Y <= end__2080; Y += 1)
                {
                    {
                        X = 0;
                        byte end__2081 = 5;
                        for (; X <= end__2081; X += 1)
                        {
                            Control.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 15);
                        }
                    }

                    {
                        X = MapContainer.Width - 6;
                        int end__2082 = MapContainer.Width - 1;
                        for (; X <= end__2082; X += 1)
                        {
                            Control.Place(MapContainer, unchecked((sbyte)X), unchecked((sbyte)Y), 0, 15);
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
                    var call__2083 = FEffects[I];
                    Effect = (Aow.Units.AoWTC.TfxHS)call__2083;
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
                bool bool__2084 = FEndTurnPending;
                if (bool__2084)
                {
                    var call__2085 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FCurrentPlayer);
                    bool bool__2086 = (call__2085.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman);
                    if (bool__2086)
                    {
                        var call__2087 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FCombat.FHostPlayer);
                        bool__2086 = (call__2087.ConnectionMode != Aow.Units.NetworkE.TConnectionMode.cmRemote);
                    }

                    bool bool__2088 = bool__2086;
                    if (!bool__2088)
                    {
                        var call__2089 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                        bool bool__2090 = call__2089.FAuto;
                        if (bool__2090)
                        {
                            var call__2091 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FCurrentPlayer);
                            bool__2090 = (call__2091.ConnectionMode != Aow.Units.NetworkE.TConnectionMode.cmRemote);
                        }

                        bool__2088 = bool__2090;
                    }

                    bool__2084 = bool__2088;
                }

                if (bool__2084)
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
            var call__2092 = FCombat.FCombatData.GetCount();
            I = call__2092;
            while ((I != 0) && (X == -1))
            {
                I -= 1;
                var call__2093 = FCombat.FCombatData.GetObjects(I);
                bool bool__2094 = (call__2093 is Aow.Units.AoWTC.TTacticalCombatUnit);
                if (bool__2094)
                {
                    var call__2095 = FCombat.FCombatData.GetObjects(I);
                    bool__2094 = (((Aow.Units.AoWTC.TTacticalCombatUnit)call__2095).FUnit == Hero);
                }

                if (bool__2094)
                {
                    var call__2096 = FCombat.FCombatData.GetObjects(I);
                    var call__2097 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__2096).FUnitHS.GetXhx();
                    X = unchecked((sbyte)call__2097);
                    var call__2098 = FCombat.FCombatData.GetObjects(I);
                    var call__2099 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__2098).FUnitHS.GetYhx();
                    Y = unchecked((sbyte)call__2099);
                    L = 0;
                }
            }

            if (X > -1)
            {
                var call__2100 = MapContainer.GetField(X, Y, L);
                var call__2101 = call__2100.FindOwnedChild(131697);
                HS = (Aow.Units.AoWE.TItemHS)call__2101;
                if (HS == null)
                {
                    HS = Aow.Units.AoWTC.TCItemHS.Create(null);
                    // { Original bug: a failed placement leaves the new item sprite allocated. }
                    var call__2102 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(HS, X, Y, L);
                    if (call__2102)
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
                var call__2103 = MapContainer.GetField(X, Y, L);
                var call__2104 = call__2103.FindOwnedChild(131697);
                HS = (Aow.Units.AoWE.TItemHS)call__2104;
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
            bool bool__2105 = !Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting);
            if (bool__2105)
            {
                var call__2106 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                bool__2105 = !call__2106.FAuto;
            }

            bool bool__2107 = bool__2105;
            if (bool__2107)
            {
                var call__2108 = Mouse.Xhx;
                bool__2107 = (call__2108 != -1);
            }

            bool bool__2109 = bool__2107;
            if (bool__2109)
            {
                var call__2110 = Mouse.Yhx;
                bool__2109 = (call__2110 != -1);
            }

            if (bool__2109 && (Mouse.Capture == null))
            {
                Mouse.CursorMode = 0;
                var call__2111 = Mouse.Yhx;
                var call__2112 = Mouse.Xhx;
                var call__2113 = MapContainer.MapLevels[0];
                var call__2114 = call__2113.Field[call__2112, call__2111];
                var call__2115 = call__2114.FindOwnedChild(2228484);
                HS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__2115;
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
                var call__2116 = FCombat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                SetSeatedPlayer(unchecked((sbyte)call__2116.FPlayer));
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

            var call__2117 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
            SetCurrentPlayer(unchecked((sbyte)call__2117.FPlayer));
            FCombat.NewTurn(unchecked((byte)unchecked((byte)FCurrentCombatPlayer)));
            FUnitSelection.Unselect();
            Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.TokenExecuter.ActivateTC(FCurrentPlayer);
            var call__2118 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
            if (call__2118.FPlayer == 0)
            {
                var call__2119 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWTC.AoWTC.IndRStr);
                var call__2120 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWTC.AoWTC.BeginRoundRStr);
                ShowMessage((call__2119 + AnsiString.FromBytes(32)) + call__2120);
            }
            else
            {
                var call__2121 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                var call__2122 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__2121.FPlayer);
                var call__2123 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__2122.FPlayerRace]);
                var call__2124 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWTC.AoWTC.BeginRoundRStr);
                ShowMessage((call__2123 + AnsiString.FromBytes(32)) + call__2124);
            }

            var call__2125 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
            var call__2126 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__2125.FPlayer);
            if (call__2126.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman)
            {
                var call__2127 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                var call__2128 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(call__2127.FPlayer);
                if (call__2128.ConnectionMode != Aow.Units.NetworkE.TConnectionMode.cmRemote)
                {
                    SetSeatedCombatPlayer(unchecked((sbyte)FCurrentCombatPlayer));
                    var call__2129 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                    if (call__2129.FLastX == 0)
                    {
                        U = GetNextUnit(null, true, true);
                        if (U != null)
                        {
                            var call__2130 = U.FUnitHS.GetXhx();
                            var call__2131 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                            call__2131.FLastX = call__2130;
                            var call__2132 = U.FUnitHS.GetYhx();
                            var call__2133 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                            call__2133.FLastY = call__2132;
                            if (FExplorationEnabled)
                            {
                                {
                                    I = 0;
                                    int end__2134 = FPlayers.GetCount() - 1;
                                    for (; I <= end__2134; I += 1)
                                    {
                                        if (FCurrentCombatPlayer != I)
                                        {
                                            var call__2135 = U.FUnitHS.GetXhx();
                                            var call__2136 = FPlayers.GetPlayers(unchecked((sbyte)I));
                                            call__2136.FLastX = call__2135;
                                            var call__2137 = U.FUnitHS.GetYhx();
                                            var call__2138 = FPlayers.GetPlayers(unchecked((sbyte)I));
                                            call__2138.FLastY = call__2137;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var call__2139 = FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                    var call__2140 = FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                    CenterViewTo(call__2140.FLastX, call__2139.FLastY, 0);
                }
            }

            Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Events\\NewEvent.wav");
            Aow.Units.Engine.Engine.InitMsg(ref Msg, 537006081, this);
            SendMsg(ref Msg, this);
            if (FExplorationEnabled)
            {
                var call__2141 = Aow.Rtl.Rtl.Rect(0, 0, 65, 58);
                TriggerMapChangedEvent(call__2141, 0);
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
                var call__2142 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FCurrentPlayer);
                if (call__2142.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman)
                {
                    return Result;
                }

                if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer != Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)
                {
                    return Result;
                }
            }

            var call__2143 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(FCurrentPlayer);
            if (call__2143)
            {
                var call__2144 = Aow.Units.CombatTE.CombatTE.CreateTCEndTurnTE(unchecked((sbyte)FCurrentPlayer));
                TE = call__2144;
                if (TE != null)
                {
                    var call__2145 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(FCurrentPlayer);
                    call__2145.AddEvent(TE);
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
                int end__2146 = FPlayers.GetCount() - 1;
                for (; I <= end__2146; I += 1)
                {
                    bool bool__2147 = Result;
                    if (bool__2147)
                    {
                        var call__2148 = FPlayers.GetPlayers(unchecked((sbyte)I));
                        bool__2147 = call__2148.FEndTurn;
                    }

                    Result = bool__2147;
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

            Index = 0;
            I = 0;
            Start = 0;
            if (U != null)
            {
                while (true)
                {
                    var call__2150 = this.FCombat.FCombatData.GetCount();
                    if (!(I < call__2150))
                        break;
                    {
                        var call__2151 = this.FCombat.FCombatData.GetObjects(I);
                        if (call__2151 == U)
                        {
                            Index = I + Step;
                            Start = I;
                            var call__2152 = this.FCombat.FCombatData.GetCount();
                            I = call__2152;
                        }

                        I += 1;
                    }
                }
            }

            while (true)
            {
                var call__2153 = this.FCombat.FCombatData.GetCount();
                if (!((Index < call__2153) && (Index > -1)))
                    break;
                {
                    var call__2154 = this.FCombat.FCombatData.GetObjects(Index);
                    bool bool__2155 = (call__2154 != null);
                    if (bool__2155)
                    {
                        var call__2156 = this.FCombat.FCombatData.GetObjects(Index);
                        bool__2155 = (call__2156 is Aow.Units.AoWE.TCombatUnit);
                    }

                    bool bool__2157 = bool__2155;
                    if (bool__2157)
                    {
                        bool bool__2158 = AllUnits;
                        if (!bool__2158)
                        {
                            var call__2159 = this.FCombat.FCombatData.GetObjects(Index);
                            bool__2158 = (((Aow.Units.AoWTC.TTacticalCombatUnit)call__2159).FMovePoints > 0);
                        }

                        bool__2157 = bool__2158;
                    }

                    bool bool__2160 = bool__2157;
                    if (bool__2160)
                    {
                        var call__2161 = this.FCombat.FCombatData.GetObjects(Index);
                        var call__2162 = call__2161.GetEnabled();
                        bool__2160 = call__2162;
                    }

                    bool bool__2163 = bool__2160;
                    if (bool__2163)
                    {
                        var call__2164 = this.FCombat.FCombatData.GetObjects(Index);
                        bool__2163 = (call__2164.FCombatPlayer == FCurrentCombatPlayer);
                    }

                    if (bool__2163)
                    {
                        var call__2165 = this.FCombat.FCombatData.GetObjects(Index);
                        Result = (Aow.Units.AoWTC.TTacticalCombatUnit)call__2165;
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
                    var call__2166 = this.FCombat.FCombatData.GetCount();
                    Index = call__2166 - 1;
                }

                while (true)
                {
                    bool bool__2167 = (Index != Start) && (Index > -1);
                    if (bool__2167)
                    {
                        var call__2168 = this.FCombat.FCombatData.GetCount();
                        bool__2167 = (Index < call__2168);
                    }

                    if (!bool__2167)
                        break;
                    {
                        var call__2169 = this.FCombat.FCombatData.GetObjects(Index);
                        bool bool__2170 = (call__2169 != null);
                        if (bool__2170)
                        {
                            var call__2171 = this.FCombat.FCombatData.GetObjects(Index);
                            bool__2170 = (call__2171 is Aow.Units.AoWE.TCombatUnit);
                        }

                        bool bool__2172 = bool__2170;
                        if (bool__2172)
                        {
                            bool bool__2173 = AllUnits;
                            if (!bool__2173)
                            {
                                var call__2174 = this.FCombat.FCombatData.GetObjects(Index);
                                bool__2173 = (((Aow.Units.AoWTC.TTacticalCombatUnit)call__2174).FMovePoints > 0);
                            }

                            bool__2172 = bool__2173;
                        }

                        bool bool__2175 = bool__2172;
                        if (bool__2175)
                        {
                            var call__2176 = this.FCombat.FCombatData.GetObjects(Index);
                            var call__2177 = call__2176.GetEnabled();
                            bool__2175 = call__2177;
                        }

                        bool bool__2178 = bool__2175;
                        if (bool__2178)
                        {
                            var call__2179 = this.FCombat.FCombatData.GetObjects(Index);
                            bool__2178 = (call__2179.FCombatPlayer == FCurrentCombatPlayer);
                        }

                        if (bool__2178)
                        {
                            var call__2180 = this.FCombat.FCombatData.GetObjects(Index);
                            Result = (Aow.Units.AoWTC.TTacticalCombatUnit)call__2180;
                            Index = -10;
                        }

                        Index = Index + Step;
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
            bool bool__2181 = (FSeatedPlayer == -1);
            if (!bool__2181)
            {
                var call__2182 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(FSeatedPlayer);
                bool__2181 = call__2182;
            }

            bool bool__2183 = bool__2181 && (FSeatedPlayer == FCurrentPlayer);
            if (bool__2183)
            {
                var call__2184 = FPlayers.GetPlayers(unchecked((sbyte)FCurrentCombatPlayer));
                bool__2183 = !call__2184.FAuto;
            }

            if (!(bool__2183 && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer == Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentCombatPlayer)))
            {
                Mouse.Cursor = (((int)(((uint)Frame) >> 2)) % 4) + 6;
            }
            else
            {
                var call__2185 = FUnitSelection.GetTCUnitHS();
                if (call__2185 != null)
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
                var call__2186 = FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                var call__2187 = GetNextUnit(call__2186.FLastUnit, false, Forward);
                U = call__2187;
                if (U != null)
                {
                    var call__2188 = FUnitSelection.GetTCUnitHS();
                    if (call__2188 != U.FUnitHS)
                    {
                        var call__2189 = FPlayers.GetPlayers(unchecked((sbyte)FSeatedCombatPlayer));
                        call__2189.FLastUnit = U;
                        var call__2190 = U.FUnitHS.GetYhx();
                        var call__2191 = U.FUnitHS.GetXhx();
                        CenterViewTo(call__2191, call__2190, 0);
                        Selection.UnselectLocation();
                        var call__2192 = U.FUnitHS.GetYhx();
                        var call__2193 = U.FUnitHS.GetXhx();
                        Aow.Units.HSEngine.HSEngine.HXtoHP(call__2193, call__2192, ref X, ref Y);
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
                var call__2194 = FCombat.FCombatData.GetCount();
                if (!(I < call__2194))
                    break;
                {
                    var call__2195 = FCombat.FCombatData.GetObjects(I);
                    bool bool__2196 = (((Aow.Units.AoWTC.TTacticalCombatUnit)call__2195).FUnit == U);
                    if (bool__2196)
                    {
                        var call__2197 = FCombat.FCombatData.GetObjects(I);
                        var call__2198 = call__2197.GetEnabled();
                        bool__2196 = call__2198;
                    }

                    if (bool__2196)
                    {
                        var call__2199 = FCombat.FCombatData.GetObjects(I);
                        var call__2200 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__2199).FUnitHS.GetXhx();
                        X = unchecked((sbyte)call__2200);
                        var call__2201 = FCombat.FCombatData.GetObjects(I);
                        var call__2202 = ((Aow.Units.AoWTC.TTacticalCombatUnit)call__2201).FUnitHS.GetYhx();
                        Y = unchecked((sbyte)call__2202);
                        L = 0;
                        var call__2203 = FCombat.FCombatData.GetCount();
                        I = call__2203;
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
            var call__2204 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.PlayerReady(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCurrentPlayer);
            if (call__2204 && (FCurrentCombatPlayer == FSeatedCombatPlayer))
            {
                FUnitSelection.Unselect();
                TE = Aow.Units.CombatTE.CombatTE.CreateTCEndCombatTE();
                if (TE != null)
                {
                    var call__2205 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(FCurrentPlayer);
                    call__2205.AddEvent(TE);
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
        public TTacticalCombat(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        public sbyte FHostPlayer;
        // { $4C }
        public Aow.Units.FixedArray_byte_3 F_TacticalCombat_4D;
        // AoWTC.pas:5086
        public override Aow.Units.AoWE.TCombatUnit CreateCombatUnit()
        {
            Aow.Units.AoWE.TCombatUnit Result = default;
            Result = new Aow.Units.AoWTC.TTacticalCombatUnit(null);
            return Result;
        }

        // AoWTC.pas:5157
        public override Aow.Units.AoWE.TCombatPlayer CreateCombatPlayer()
        {
            Aow.Units.AoWE.TCombatPlayer Result = default;
            Result = new Aow.Units.AoWTC.TTacticalCombatPlayer(null);
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
                int end__2206 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetCount() - 1;
                for (; I <= end__2206; I += 1)
                {
                    if (FHostPlayer == -1)
                    {
                        var call__2207 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                        if (call__2207.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman)
                        {
                            var call__2208 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                            if (call__2208.FStatus != Aow.Units.AoWE.TPlayerStatus.psTerminated)
                            {
                                FHostPlayer = unchecked((sbyte)I);
                            }
                        }
                    }
                }
            }

            {
                I = 0;
                int end__2209 = FCombatData.FPlayers.GetCount() - 1;
                for (; I <= end__2209; I += 1)
                {
                    var call__2210 = FCombatData.FTurnOrder.IndexOf(unchecked((byte)I));
                    if (call__2210 == -1)
                    {
                        var call__2211 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                        if (call__2211.FSideIndex == 0)
                        {
                            FCombatData.FTurnOrder.Add(unchecked((byte)I));
                        }
                    }

                    var call__2212 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                    var call__2213 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__2212.FPlayer);
                    if (call__2213.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman)
                    {
                        if (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC is Aow.Units.NetworkE.TRemotePCC)
                        {
                            var call__2214 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FHostPlayer);
                            var call__2215 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                            ((Aow.Units.NetworkE.TRemotePCC)Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC).SetConnectionMode(call__2215.FPlayer, call__2214.ConnectionMode);
                            var call__2216 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.GetPlayers(FHostPlayer);
                            var call__2217 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                            ((Aow.Units.NetworkE.TRemotePCC)Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC).SetSystemID(call__2217.FPlayer, call__2216.SystemID);
                        }
                    }
                }
            }

            {
                I = 0;
                int end__2218 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetCount() - 1;
                for (; I <= end__2218; I += 1)
                {
                    var call__2219 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(I);
                    var call__2220 = call__2219.Local();
                    bool bool__2221 = call__2220;
                    if (!bool__2221)
                    {
                        var call__2222 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(I);
                        var call__2223 = call__2222.Global();
                        bool__2221 = call__2223;
                    }

                    if (bool__2221)
                    {
                        var call__2224 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(I);
                        call__2224.ClearEvents();
                    }
                }
            }

            FCombatData.FTurnOrder.Add(unchecked((byte)FCombatData.FDefenderPlayer));
            {
                I = 0;
                int end__2225 = FCombatData.FPlayers.GetCount() - 1;
                for (; I <= end__2225; I += 1)
                {
                    var call__2226 = FCombatData.FTurnOrder.IndexOf(unchecked((byte)I));
                    if (call__2226 == -1)
                    {
                        var call__2227 = FCombatData.FPlayers.GetPlayers(unchecked((sbyte)I));
                        if (call__2227.FSideIndex == 1)
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
            var call__2228 = FCombatData.FParties.GetParty(0);
            Result = call__2228.FWall == 0;
            I = Side.GetCount();
            while (!Result && (I > 0))
            {
                I -= 1;
                var call__2229 = Side.GetObjects(I);
                var call__2230 = call__2229.WallCombatFeatures();
                Result = (call__2230 & TSet<Aow.Units.AoWE.TWallCombatFeaturesElement>.Of(Aow.Units.AoWE.TWallCombatFeaturesElement.wcWallBreaching, Aow.Units.AoWE.TWallCombatFeaturesElement.wcWallPassing)) != TSet<Aow.Units.AoWE.TWallCombatFeaturesElement>.Of();
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
            public new Aow.Units.AoWTC.TTacticalCombat Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.AoWTC.TTacticalCombat)base.Create(AOwner);
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
                short end__2231 = 285;
                for (; I <= end__2231; I += 1)
                {
                    FResources[I].Free();
                }
            }

            base.Destroy();
        }

        // AoWTC.pas:4137
        public TRndTacHsm()
        {
            int I = default;
            int J = default;
            ;
            {
                I = 0;
                short end__2232 = 285;
                for (; I <= end__2232; I += 1)
                {
                    var call__2233 = new Aow.Rtl.TList();
                    FResources[I] = call__2233;
                }
            }

            {
                J = 0;
                int end__2234 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.GetCount() - 1;
                for (; J <= end__2234; J += 1)
                {
                    var call__2235 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.MORs[J];
                    if (call__2235 is Aow.Units.CombatTerrain.TCombatTerrainResource)
                    {
                        var call__2236 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.MORs[J];
                        var call__2237 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.MORs[J];
                        var call__2238 = Aow.Units.AoWE.AoWE.AoWHSSet.FCombatResources.MORs[J];
                        FResources[(unchecked((sbyte)((Aow.Units.CombatTerrain.TCombatTerrainResource)call__2237).TerrainType) * 19) + ((Aow.Units.CombatTerrain.TCombatTerrainResource)call__2238).DecorationType].Add(call__2236);
                    }
                }
            }

            InitTables();
        }

        // AoWTC.pas:4156
        public Aow.Units.AoWTC.TAoWCombatMap RandomMap(Aow.Units.AoWTC.TTacticalCombat Combat, int X, int Y, int L)
        {
            Aow.Units.AoWTC.TAoWCombatMap Result = default;
            byte[] TerrainBuffer = [];
            int I = default;
            int J = default;
            int K = default;
            int NX = default;
            int Col = default;
            int Row = default;
            int TerrainType = default;
            int Tag = default;
            byte[] ShapeBuffer = [];
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
                    int end__2239 = Last;
                    for (; I <= end__2239; I += 1)
                    {
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, Aow.Units.AoWTC.AoWTC.clusterChk[I], ref NX, ref NY);
                        Kind = TerrainBuffer[(NY * 65) + NX];
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
                byte end__2240 = 6;
                for (; I <= end__2240; I += 1)
                {
                    Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                    var call__2241 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                    var call__2242 = call__2241.Field[J, K];
                    FTerrain[I] = unchecked((sbyte)call__2242.TerrainType);
                    var call__2243 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                    var call__2244 = call__2243.Field[J, K];
                    var call__2245 = ((Aow.Units.AoWE.TAoWMapField)call__2244).GetCombatTerrainTag();
                    FTags[I] = unchecked((sbyte)call__2245);
                }
            }

            if (((FTerrain[0] == 0) || (FTerrain[0] == 9)) || (FTerrain[0] == 10))
            {
                {
                    I = 1;
                    byte end__2246 = 6;
                    for (; I <= end__2246; I += 1)
                    {
                        FTerrain[I] = unchecked((sbyte)FTerrain[0]);
                    }
                }
            }
            else
            {
                {
                    I = 1;
                    byte end__2247 = 6;
                    for (; I <= end__2247; I += 1)
                    {
                        if ((((FTerrain[I] == 0) || (FTerrain[I] == 15)) || (FTerrain[I] == 10)) || (FTerrain[I] == 9))
                        {
                            FTerrain[I] = unchecked((sbyte)FTerrain[0]);
                        }
                    }
                }
            }

            FileName = (AnsiString)"TCMaps\\";
            var call__2248 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
            var call__2249 = call__2248.Field[X, Y];
            var call__2250 = call__2249.FindNoneTransparentHS(typeof(Aow.Units.AoWE.TStructure));
            Structure = call__2250;
            if (Structure == null)
            {
                var call__2251 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                var call__2252 = call__2251.Field[X, Y];
                var call__2253 = call__2252.FindNoneTransparentHS(typeof(Aow.Units.HxBridge.THexagonBridge));
                Structure = call__2253;
            }

            if (Structure != null)
            {
                switch (Structure.ClassType.ClassID())
                {
                    case 131336:
                        FileName = FileName + AnsiString.FromBytes(48);
                    {
                        I = 1;
                        byte end__2254 = 6;
                        for (; I <= end__2254; I += 1)
                        {
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                            var call__2255 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                            var call__2256 = call__2255.Field[J, K];
                            var call__2257 = call__2256.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                            if (call__2257 != null)
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
                                byte end__2258 = 6;
                                for (; I <= end__2258; I += 1)
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
                                byte end__2259 = 6;
                                for (; I <= end__2259; I += 1)
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
                                byte end__2260 = 6;
                                for (; I <= end__2260; I += 1)
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
                                byte end__2261 = 6;
                                for (; I <= end__2261; I += 1)
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
                                byte end__2262 = 6;
                                for (; I <= end__2262; I += 1)
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
                ShapeBuffer = new byte[3770];
                TerrainBuffer = new byte[3770];
                {
                    Row = 6;
                    byte end__2263 = 52;
                    for (; Row <= end__2263; Row += 1)
                    {
                        {
                            Col = 6;
                            byte end__2264 = 58;
                            for (; Col <= end__2264; Col += 1)
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

                                var call__2265 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__2266 = call__2265.Field[Col, Row];
                                var call__2267 = call__2266.GetDecorationType();
                                if (call__2267 == 9)
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
                                        var call__2268 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                                        var call__2269 = call__2268.Field[K, NX];
                                        var call__2270 = call__2269.FindOwnedChild(131946);
                                        if (call__2270 == null)
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

                var call__2271 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                var call__2272 = call__2271.Field[X, Y];
                if (call__2272.OverlayType == 4)
                {
                    {
                        I = 0;
                        byte end__2273 = 6;
                        for (; I <= end__2273; I += 1)
                        {
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(X, Y, I, ref J, ref K);
                            var call__2274 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[L];
                            var call__2275 = call__2274.Field[J, K];
                            Weights[I] = call__2275.OverlayType;
                        }
                    }

                    {
                        Row = 6;
                        byte end__2276 = 52;
                        for (; Row <= end__2276; Row += 1)
                        {
                            {
                                Col = 6;
                                byte end__2277 = 58;
                                for (; Col <= end__2277; Col += 1)
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
                    byte end__2278 = 52;
                    for (; Row <= end__2278; Row += 1)
                    {
                        {
                            Col = 3;
                            byte end__2279 = 29;
                            for (; Col <= end__2279; Col += 1)
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
                                        byte end__2280 = 18;
                                        for (; I <= end__2280; I += 1)
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
                                        byte end__2281 = 2;
                                        for (; I <= end__2281; I += 1)
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

                                    var call__2282 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__2283 = call__2282.Field[Col * 2, Row];
                                    var call__2284 = call__2283.FindChild(2228484);
                                    HasUnit = call__2284 != null;
                                    if (HasUnit || Blocked(Col * 2, Row, 0, 2))
                                    {
                                        bool bool__2285 = HasUnit;
                                        if (!bool__2285)
                                        {
                                            var call__2286 = Blocked(Col * 2, Row, 4, 8);
                                            bool__2285 = call__2286;
                                        }

                                        bool bool__2287 = bool__2285;
                                        if (bool__2287)
                                        {
                                            bool bool__2288 = HasUnit;
                                            if (!bool__2288)
                                            {
                                                var call__2289 = Blocked(Col * 2, Row, 9, 17);
                                                bool__2288 = call__2289;
                                            }

                                            bool__2287 = bool__2288;
                                        }

                                        if (bool__2287)
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
                                        bool bool__2290 = HasUnit;
                                        if (!bool__2290)
                                        {
                                            var call__2291 = Blocked(Col * 2, Row, 4, 8);
                                            bool__2290 = call__2291;
                                        }

                                        bool bool__2292 = bool__2290;
                                        if (bool__2292)
                                        {
                                            bool bool__2293 = HasUnit;
                                            if (!bool__2293)
                                            {
                                                var call__2294 = Blocked(Col * 2, Row, 9, 17);
                                                bool__2293 = call__2294;
                                            }

                                            bool__2292 = bool__2293;
                                        }

                                        if (bool__2292)
                                        {
                                            Weights[14] = 0;
                                            Weights[15] = 0;
                                            Weights[16] = 0;
                                        }
                                    }

                                    J = 0;
                                    {
                                        I = 0;
                                        byte end__2295 = 18;
                                        for (; I <= end__2295; I += 1)
                                        {
                                            J += Weights[I];
                                        }
                                    }

                                    var call__2296 = Aow.Rtl.Rtl.Random(J);
                                    Tag = call__2296 - Weights[0];
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
                            byte end__2297 = 29;
                            for (; Col <= end__2297; Col += 1)
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
                                        byte end__2298 = 18;
                                        for (; I <= end__2298; I += 1)
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
                                        byte end__2299 = 2;
                                        for (; I <= end__2299; I += 1)
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

                                    var call__2300 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__2301 = call__2300.Field[(Col * 2) + 1, Row];
                                    var call__2302 = call__2301.FindChild(2228484);
                                    HasUnit = call__2302 != null;
                                    if (HasUnit || Blocked((Col * 2) + 1, Row, 0, 2))
                                    {
                                        bool bool__2303 = HasUnit;
                                        if (!bool__2303)
                                        {
                                            var call__2304 = Blocked((Col * 2) + 1, Row, 4, 8);
                                            bool__2303 = call__2304;
                                        }

                                        bool bool__2305 = bool__2303;
                                        if (bool__2305)
                                        {
                                            bool bool__2306 = HasUnit;
                                            if (!bool__2306)
                                            {
                                                var call__2307 = Blocked((Col * 2) + 1, Row, 9, 17);
                                                bool__2306 = call__2307;
                                            }

                                            bool__2305 = bool__2306;
                                        }

                                        if (bool__2305)
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
                                        bool bool__2308 = HasUnit;
                                        if (!bool__2308)
                                        {
                                            var call__2309 = Blocked((Col * 2) + 1, Row, 4, 8);
                                            bool__2308 = call__2309;
                                        }

                                        bool bool__2310 = bool__2308;
                                        if (bool__2310)
                                        {
                                            bool bool__2311 = HasUnit;
                                            if (!bool__2311)
                                            {
                                                var call__2312 = Blocked((Col * 2) + 1, Row, 9, 17);
                                                bool__2311 = call__2312;
                                            }

                                            bool__2310 = bool__2311;
                                        }

                                        if (bool__2310)
                                        {
                                            Weights[14] = 0;
                                            Weights[15] = 0;
                                            Weights[16] = 0;
                                        }
                                    }

                                    J = 0;
                                    {
                                        I = 0;
                                        byte end__2313 = 18;
                                        for (; I <= end__2313; I += 1)
                                        {
                                            J += Weights[I];
                                        }
                                    }

                                    var call__2314 = Aow.Rtl.Rtl.Random(J);
                                    Tag = call__2314 - Weights[0];
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
                    byte end__2315 = 52;
                    for (; Row <= end__2315; Row += 1)
                    {
                        {
                            Col = 6;
                            byte end__2316 = 58;
                            for (; Col <= end__2316; Col += 1)
                            {
                                I = ShapeBuffer[(Row * 65) + Col];
                                if (I > 0)
                                {
                                    J = TerrainBuffer[(Row * 65) + Col];
                                    if (J == 17)
                                    {
                                        var call__2317 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__2318 = call__2317.Field[Col, Row];
                                        call__2318.TerrainType = 7;
                                    }
                                    else
                                    {
                                        var call__2319 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                        var call__2320 = call__2319.Field[Col, Row];
                                        call__2320.TerrainType = unchecked((sbyte)FTerrain[I - 1]);
                                        if ((J > 0) && (J < 19))
                                        {
                                            if (FResources[(FTerrain[I - 1] * 19) + J].Count > 0)
                                            {
                                                var call__2321 = Aow.Rtl.Rtl.Random(FResources[(FTerrain[I - 1] * 19) + J].Count);
                                                var call__2322 = FResources[(FTerrain[I - 1] * 19) + J][call__2321];
                                                ((Aow.Units.HSEngine.TMapObjectResource)call__2322).PlaceResourceUser(Aow.Units.AoWTC.AoWTC.AoWCombatMap, unchecked((sbyte)Col), unchecked((sbyte)Row), 0);
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
                    byte end__2323 = 52;
                    for (; Row <= end__2323; Row += 1)
                    {
                        {
                            Col = 6;
                            byte end__2324 = 58;
                            for (; Col <= end__2324; Col += 1)
                            {
                                var call__2325 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__2326 = call__2325.Field[Col, Row];
                                call__2326.TerrainChanged();
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
                byte end__2327 = 7;
                for (; Position <= end__2327; Position += 1)
                {
                    Units.Clear();
                    {
                        I = 0;
                        int end__2328 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetCount() - 1;
                        for (; I <= end__2328; I += 1)
                        {
                            var call__2329 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                            if (call__2329 is Aow.Units.AoWE.TCombatUnit)
                            {
                                var call__2330 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                var call__2331 = call__2330.GetEnabled();
                                if (call__2331)
                                {
                                    var call__2332 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                    if (((int)(((uint)call__2332.FPosition) >> 4)) == Position)
                                    {
                                        var call__2333 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.GetObjects(I);
                                        Units.Add(call__2333);
                                        var call__2334 = Units[Units.Count - 1];
                                        U = (Aow.Units.AoWTC.TTacticalCombatUnit)call__2334;
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
                                                var call__2335 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL];
                                                var call__2336 = call__2335.Field[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY];
                                                var call__2337 = call__2336.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                                                if (call__2337 != null)
                                                {
                                                    ;
                                                }
                                            }
                                        }

                                        if ((FHomePosition > 0) && (U.FPositionGroup == FHomePosition))
                                        {
                                            U.FPositionGroup = 0;
                                        }

                                        var call__2338 = U.GetSide();
                                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPositionStatus[U.FPositionGroup] = unchecked((byte)unchecked((byte)call__2338));
                                    }
                                }
                            }
                        }
                    }

                    {
                        I = 0;
                        int end__2339 = Units.Count - 1;
                        for (; I <= end__2339; I += 1)
                        {
                            var call__2340 = Units[I];
                            U = (Aow.Units.AoWTC.TTacticalCombatUnit)call__2340;
                            {
                                J = I + 1;
                                int end__2341 = Units.Count - 1;
                                for (; J <= end__2341; J += 1)
                                {
                                    var call__2342 = Units[J];
                                    Other = (Aow.Units.AoWTC.TTacticalCombatUnit)call__2342;
                                    var call__2343 = U.GetPosValue();
                                    var call__2344 = Other.GetPosValue();
                                    if (call__2343 < call__2344)
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
                var call__2345 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL];
                var call__2346 = call__2345.Field[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY];
                var call__2347 = call__2346.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                if ((call__2347 != null) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FWallType != 0))
                {
                    Position = 1;
                    while (Position < 7)
                    {
                        if (Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPositionStatus[Position] == 1)
                        {
                            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY, Position, ref J, ref Temp);
                            var call__2348 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FL];
                            var call__2349 = call__2348.Field[J, Temp];
                            var call__2350 = call__2349.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                            if (call__2350 != null)
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
                int end__2351 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csAttacker].GetCount() - 1;
                for (; Position <= end__2351; Position += 1)
                {
                    var call__2352 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csAttacker].GetObjects(Position);
                    var call__2353 = call__2352.GetEnabled();
                    if (call__2353)
                    {
                        SetupUnit(Aow.Units.AoWE.TCombatSideIndex.csAttacker, Position, ref Direction);
                    }
                }
            }

            {
                Position = 0;
                int end__2354 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].GetCount() - 1;
                for (; Position <= end__2354; Position += 1)
                {
                    var call__2355 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].GetObjects(Position);
                    if (call__2355 is Aow.Units.AoWE.TCombatUnit)
                    {
                        var call__2356 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Aow.Units.AoWE.TCombatSideIndex.csDefender].GetObjects(Position);
                        var call__2357 = call__2356.GetEnabled();
                        if (call__2357)
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
                    byte end__2358 = 7;
                    for (; Position <= end__2358; Position += 1)
                    {
                        Used[Position] = false;
                    }
                }

                Position = 0;
                while (true)
                {
                    var call__2359 = FItems.GetCount();
                    if (!(Position < call__2359))
                        break;
                    {
                        var call__2360 = Aow.Rtl.Rtl.Random(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.FPositions.Count - 16);
                        I = call__2360;
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
                        var call__2361 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(2, I);
                        J = call__2361.X;
                        var call__2362 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(2, I);
                        Temp = call__2362.Y;
                        ItemHS = Aow.Units.AoWTC.TCItemHS.Create(null);
                        var call__2363 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(ItemHS, J, Temp, 0);
                        if (call__2363)
                        {
                            var call__2364 = FItems.GetItem(Position);
                            call__2364.SetOwner(ItemHS.FItems);
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
            var call__2365 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FSides[(int)Side].GetObjects(Index);
            UnitHS.Initialize((Aow.Units.AoWTC.TTacticalCombatUnit)call__2365);
            if (UnitHS.FUnit.FPositionGroup == FHomePosition)
            {
                UnitHS.FUnit.FPositionGroup = 0;
            }

            Position = UnitHS.FUnit.FPositionGroup;
            Slot = UnitHS.FUnit.FPositionIndex;
            var call__2366 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(Position, Slot);
            X = call__2366.X;
            var call__2367 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(Position, Slot);
            Y = call__2367.Y;
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
                var call__2368 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FUnitPositions.GetUnitPosition(Position, Slot);
                UnitHS.SetDirection(unchecked((byte)call__2368.Flag));
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
                var call__2369 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__2370 = call__2369.Field[NX, NY];
                Slot = call__2370.OverlayType;
                var call__2371 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                var call__2372 = call__2371.Field[NX, NY];
                var call__2373 = call__2372.FindOwnedChild(2228484);
                TargetHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__2373;
                HN += 1;
            }
            while (!((Slot != 8) && (TargetHS == null)));
            Aow.Units.AoWTC.AoWTC.AoWCombatMap.PlaceHX(UnitHS, NX, NY, 0);
            var call__2374 = Aow.Units.HSEngine.HSEngine.dHXtoHN(Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FY, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FAttackerX, Aow.Units.AoWTC.AoWTC.AoWCombatMap.FCombat.FCombatData.FAttackerY);
            bool bool__2375 = (call__2374 == Position);
            if (bool__2375)
            {
                var call__2376 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)UnitHS.FUnit.FCombatPlayer));
                bool__2375 = (call__2376.FLastX == 0);
            }

            if (bool__2375)
            {
                var call__2377 = UnitHS.GetXhx();
                var call__2378 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)UnitHS.FUnit.FCombatPlayer));
                call__2378.FLastX = call__2377;
                var call__2379 = UnitHS.GetYhx();
                var call__2380 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.FPlayers.GetPlayers(unchecked((sbyte)UnitHS.FUnit.FCombatPlayer));
                call__2380.FLastY = call__2379;
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
                    var call__2381 = new Aow.Units.aowFX.TXYZ();
                    Path.Add(call__2381);
                    Aow.Units.HSEngine.HSEngine.HXtoHP(X2, Y2, ref X, ref Y);
                    var call__2382 = Path[0];
                    call__2382.X = X + 12;
                    var call__2383 = Path[0];
                    call__2383.Y = Y + 12;
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
                                    var call__2384 = new Aow.Units.aowFX.TXYZ();
                                    Path.Add(call__2384);
                                    Aow.Units.HSEngine.HSEngine.HXtoHP(X2, Y2, ref X, ref Y);
                                    var call__2385 = Path[0];
                                    call__2385.X = X + 12;
                                    var call__2386 = Path[0];
                                    call__2386.Y = Y + 12;
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
                Point = new Aow.Units.aowFX.TXYZ();
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
                    var call__2387 = System.Math.Sqrt(Height);
                    Point.Z = Aow.Rtl.Rtl.Round(call__2387) - ((Steps * 10) / Arc);
                }

                I += 1;
            }

            LastX = 0;
            LastY = 0;
            var call__2388 = Path[0];
            Point = call__2388;
            Aow.Units.HSEngine.HSEngine.HPtoHX(Point.X, Point.Y, ref StartX, ref StartY);
            var call__2389 = Path.Count;
            var call__2390 = Path[call__2389 - 1];
            Point = call__2390;
            Aow.Units.HSEngine.HSEngine.HPtoHX(Point.X, Point.Y, ref EndX, ref EndY);
            {
                I = 0;
                int end__2391 = Path.Count - 1;
                for (; I <= end__2391; I += 1)
                {
                    var call__2392 = Path[I];
                    Point = call__2392;
                    Aow.Units.HSEngine.HSEngine.HPtoHX(Point.X, Point.Y, ref X, ref Y);
                    if (((X != StartX) || (Y != StartY)) && ((X != EndX) || (Y != EndY)))
                    {
                        Steps = 0;
                        var call__2393 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                        var call__2394 = call__2393.Field[X, Y];
                        if (call__2394.TerrainType == 15)
                        {
                            Steps = 3;
                        }
                        else
                        {
                            var call__2395 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                            var call__2396 = call__2395.Field[X, Y];
                            if (call__2396.TerrainType == 7)
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
                                    var call__2397 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__2398 = call__2397.Field[X, Y];
                                    var call__2399 = call__2398.FindOwnedChild(2228484);
                                    UnitHS = (Aow.Units.AoWTC.TTacticalCombatUnitHS)call__2399;
                                }

                                var call__2400 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                var call__2401 = call__2400.Field[X, Y];
                                var call__2402 = call__2401.FindOwnedChild(2229508);
                                Wall = (Aow.Units.CityWall.TCityWall)call__2402;
                                if ((Wall == null) && (Side == Aow.Units.AoWE.TCombatSideIndex.csAttacker))
                                {
                                    var call__2403 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                    var call__2404 = call__2403.Field[X, Y];
                                    var call__2405 = call__2404.FindOwnedChild(2229510);
                                    Wall = (Aow.Units.CityWall.TCityWall)call__2405;
                                }

                                if (A == 1)
                                {
                                    bool bool__2406 = (UnitHS != null);
                                    if (bool__2406)
                                    {
                                        var call__2407 = UnitHS.FUnit.GetSide();
                                        bool__2406 = (call__2407 != Side);
                                    }

                                    if (bool__2406)
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
                                            var call__2408 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                            var call__2409 = call__2408.Field[X, Y];
                                            if (call__2409.OverlayType == 8)
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
                                                        var call__2410 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                        var call__2411 = call__2410.Field[NX, NY];
                                                        var call__2412 = call__2411.FindOwnedChild(2229508);
                                                        Wall = (Aow.Units.CityWall.TCityWall)call__2412;
                                                        if (Wall == null)
                                                        {
                                                            var call__2413 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.MapLevels[0];
                                                            var call__2414 = call__2413.Field[NX, NY];
                                                            var call__2415 = call__2414.FindOwnedChild(2229510);
                                                            Wall = (Aow.Units.CityWall.TCityWall)call__2415;
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
                                    Obstacle = new Aow.Units.AoWTC.TROb();
                                    Obstacle.PathIndex = I;
                                    Obstacles.Add(Obstacle);
                                    LastX = X;
                                    LastY = Y;
                                    Steps = 1;
                                }
                                else
                                {
                                    var call__2416 = Obstacles[Obstacles.Count - 1];
                                    Obstacle = (Aow.Units.AoWTC.TROb)call__2416;
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
            Aow.Rtl.Rtl.Assert(false, Message, Filename.ToString(), LineNumber);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
