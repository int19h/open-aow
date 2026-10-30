// Translated from PoisonC.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
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

namespace Aow.Units.PoisonC
{
    // { TPoisonCloud (instance size 32, VMT 557CA640): $14 FPlayer,
//     $18 FTurnsRemaining, $1C FDrifting, $1D FDirection, $1E FUpdated. }
    public partial class TPoisonCloud : Aow.Units.HSEngine.TMultiHexMO
    {
        public TPoisonCloud(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FDrifting = false;
        }

        // PoisonC.pas:64
        public new const int ClassID = 135743;

        public sbyte FPlayer;
        public Aow.Units.FixedArray_byte_3 F_PoisonCloud_15;
        public int FTurnsRemaining;
        public bool FDrifting;
        public byte FDirection;
        public bool FUpdated;
        public byte FReserved1F;
        // PoisonC.pas:149
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 15);
            Stream.rwInteger(ref FTurnsRemaining, 16);
            Stream.rwBool(ref FDrifting, 17);
            Stream.rwByte(ref FDirection, 18);
        }

        // PoisonC.pas:58


        // PoisonC.pas:275
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (((Msg.MsgID == 537001986) && (Msg.Value == FPlayer)) && !FUpdated)
            {
                UpdateCloud();
                return;
            }

            if ((Msg.MsgID == 537001987) && (Msg.Value == FPlayer))
            {
                FUpdated = false;
            }
        }

        // PoisonC.pas:67
        public override int GetShowPriority()
        {
            int Result = default;
            Result = base.GetShowPriority() + 201326592;
            return Result;
        }

        // PoisonC.pas:89
        public override bool PlaceOnMap()
        {
            bool Result = default;
            Result = base.PlaceOnMap();
            if (Result)
            {
                var call__1 = GetYhx();
                var call__2 = GetXhx();
                UpdateMapFields((Aow.Units.HSEngine.TMapLevel)Owner.Owner, call__2, call__1);
            }

            return Result;
        }

        // PoisonC.pas:111
        public override void RemoveFromMap()
        {
            Aow.Units.HSEngine.TMapLevel Level = default;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // PoisonC.pas:182
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            if ((Msg.MsgID != 16) || Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                base.MapFieldMsgProc(ref Msg);
            }

            var call__3 = FResource.TerrainList.Terrain[Msg.HN];
            if (call__3 >= -1)
            {
                if (Msg.MsgID == 4418)
                {
                    ArmyPlaced(Msg.Param[0].As<Aow.Units.AoWE.TArmyHS>());
                }
                else
                {
                    if (Msg.MsgID == 4384)
                    {
                        var call__4 = Msg.Param[0].As<Aow.Units.AoWE.TArmyHS>().FArmy.ImmunityTypes(255);
                        if (!call__4.Contains(Aow.Units.AoWE.TDamageType.dtPoison))
                        {
                            Msg.Result = 1;
                        }
                    }
                    else
                    {
                        if (Msg.MsgID == 4432)
                        {
                            if (((unchecked((byte)Msg.Param[0].Value) == 1) && (Msg.Param[1].Value == 0)) && (Msg.HN == 0))
                            {
                                Destroy();
                            }
                        }
                    }
                }
            }
        }

        // PoisonC.pas:119
        public override int CanMoveOver(in Aow.Units.HSEngine.THSSenderInfo Info)
        {
            int Result = default;
            var call__5 = FResource.TerrainList.Terrain[Info.HN];
            if (call__5 >= -1)
            {
                var call__6 = CanDoDamage(((Aow.Units.AoWE.TMoveControl)Info.Sender).FSettings.FUnits);
                if (!call__6)
                {
                    Result = 1;
                }
                else
                {
                    Result = 0;
                    return Result;
                }
            }
            else
            {
                Result = 1;
            }

            return Result;
        }

        // PoisonC.pas:198
        public override void Activate()
        {
            base.Activate();
            if (FDirection == 0)
            {
                var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(6);
                FDirection = unchecked((byte)(call__7 + 1));
            }
        }

        // PoisonC.pas:83
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            var call__8 = FResource.TerrainList.Terrain[Info.HN];
            if (call__8 >= -1)
            {
                ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 8192;
            }
        }

        // PoisonC.pas:70
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            int Xhx = default;
            int Yhx = default;
            int HN = default;
            if (FResource.Engine.HSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                Aow.Units.HSEngine.HSEngine.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
                var call__9 = Aow.Units.HSEngine.HSEngine.dHXtoHN(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, Xhx, Yhx);
                HN = call__9;
                bool bool__10 = (HN < FResource.TerrainList.Count);
                if (bool__10)
                {
                    var call__11 = FResource.TerrainList.Terrain[HN];
                    bool__10 = (call__11 >= -1);
                }

                Result = bool__10;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // PoisonC.pas:95
        public new void UpdateMapFields(Aow.Units.HSEngine.TMapLevel Level, int Xhx, int Yhx)
        {
            Aow.Units.HSEngine.THNWalkData Walk = default;
            int Count = default;
            Count = FResource.TerrainList.Count;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (Walk.HN != Count)
            {
                var call__12 = GetTerrain(Walk.HN);
                if (((((call__12 >= -1) && (Walk.X >= 0)) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__13 = Level.GetField(Walk.X, Walk.Y);
                    call__13.UpdateMapField();
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }
        }

        // PoisonC.pas:129
        public bool CanDoDamage(Aow.Units.AoWE.TUnitList Units)
        {
            bool Result = default;
            int I = default;
            Aow.Units.AoWE.TAbstractUnit AUnit = default;
            Result = false;
            I = Units.GetCount();
            while (!Result && (I != 0))
            {
                I -= 1;
                AUnit = Units.GetUnit(I);
                var call__14 = AUnit.GetImmunityTypes();
                bool bool__15 = !call__14.Contains(Aow.Units.AoWE.TDamageType.dtPoison);
                if (bool__15)
                {
                    var call__16 = AUnit.GetAbilityEnabled(1);
                    bool__15 = !call__16;
                }

                Result = bool__15;
            }

            return Result;
        }

        // PoisonC.pas:142
        public void ArmyPlaced(Aow.Units.AoWE.TArmyHS ArmyHS)
        {
            bool bool__17 = !Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting);
            if (bool__17)
            {
                var call__18 = ((Aow.Units.AoWE.TAoWMapField)ArmyHS.Owner).GetMagicShieldActive();
                bool__17 = !call__18;
            }

            if (bool__17)
            {
                ArmyHS.FArmy.IncommingStorm(Aow.Units.AoWE.TStormType.stPestilence, 0, unchecked((sbyte)ArmyHS.FArmy.FPlayer));
            }
        }

        // PoisonC.pas:158
        public void TriggerDamage()
        {
            Aow.Units.HSEngine.TMapFieldMsg Msg = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            var call__19 = GetYhx();
            var call__20 = GetXhx();
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, call__20, call__19, 0);
            Msg.MsgID = 4432;
            Msg.Param[0] = (int)Aow.Units.AoWE.TStormType.stPestilence;
            Msg.Param[2] = FPlayer;
            ref var walk = ref Walk;
            while (FResource.TerrainList.Count > walk.HN)
            {
                var call__22 = FResource.TerrainList.GetTerrain(walk.HN);
                if (((((call__22 >= -1) && (walk.X >= 0)) && (walk.Y >= 0)) && (walk.X < Level.Width)) && (Level.Height > walk.Y))
                {
                    Msg.Param[1] = walk.Radius;
                    var call__23 = Level.GetField(walk.X, walk.Y);
                    call__23.SendMapFieldMsg(ref Msg);
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }
        }

        // PoisonC.pas:204
        public bool ValidMoveDir(int Direction)
        {
            bool Result = default;
            int Xhx = default;
            int Yhx = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            var call__24 = GetYhx();
            var call__25 = GetXhx();
            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__25, call__24, Direction, ref Xhx, ref Yhx);
            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
            Result = (((Xhx >= 10) && (Yhx >= 10)) && ((Xhx + 10) <= Level.Width)) && ((Yhx + 10) <= Level.Height);
            return Result;
        }

        // PoisonC.pas:216
        public void UpdateCloud()
        {
            int Xhx = default;
            int Yhx = default;
            Aow.Units.Engine.TByteList Directions = default;
            bool Valid = default;
            int Direction = default;
            int Tries = default;
            int Lhx = default;
            int OldXhx = default;
            int OldYhx = default;
            if (FTurnsRemaining != 1)
            {
                FUpdated = true;
                if (FTurnsRemaining != 0)
                {
                    FTurnsRemaining -= 1;
                }

                if (FDrifting)
                {
                    Direction = 0;
                    Directions = new Aow.Units.Engine.TByteList();
                    Valid = false;
                    Directions.Add(unchecked((byte)FDirection));
                    if (FDirection != 1)
                    {
                        Directions.Add(unchecked((byte)(FDirection - 1)));
                    }
                    else
                    {
                        Directions.Add(6);
                    }

                    if (FDirection != 6)
                    {
                        Directions.Add(unchecked((byte)(FDirection + 1)));
                    }
                    else
                    {
                        Directions.Add(1);
                    }

                    while ((Directions.Count != 0) && !Valid)
                    {
                        var call__26 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(Directions.Count);
                        var call__27 = Directions[call__26];
                        Direction = call__27;
                        Valid = ValidMoveDir(Direction);
                        if (!Valid)
                        {
                            Directions.Remove(unchecked((byte)Direction));
                        }
                    }

                    Directions.Free();
                    if (!Valid)
                    {
                        Tries = 20;
                        while (!Valid && (Tries != 0))
                        {
                            var call__28 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(6);
                            Direction = call__28 + 1;
                            Valid = ValidMoveDir(Direction);
                            Tries -= 1;
                        }
                    }

                    if (Valid)
                    {
                        OldXhx = GetXhx();
                        OldYhx = GetYhx();
                        FDirection = unchecked((byte)Direction);
                        Lhx = GetLhx();
                        var call__29 = GetYhx();
                        var call__30 = GetXhx();
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__30, call__29, Direction, ref Xhx, ref Yhx);
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
                        Disconnect();
                        var call__31 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(Lhx);
                        UpdateMapFields(call__31, OldXhx, OldYhx);
                        FXhx = unchecked((sbyte)Xhx);
                        FYhx = unchecked((sbyte)Yhx);
                        var call__32 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(Xhx, Yhx, Lhx);
                        Owner = call__32;
                        Connect();
                        var call__33 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetMapLevel(Lhx);
                        UpdateMapFields(call__33, Xhx, Yhx);
                    }
                }

                TriggerDamage();
            }
            else
            {
                Destroy();
            }
        }

        // PoisonC.pas:27


        public new partial class Meta : Aow.Units.HSEngine.TMapObject.Meta
        {
            // PoisonC.pas:64
            public override int ClassID() => Aow.Units.PoisonC.TPoisonCloud.ClassID;

            // PoisonC.pas:27
            public new Aow.Units.PoisonC.TPoisonCloud Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.PoisonC.TPoisonCloud)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.PoisonC.TPoisonCloud => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TPoisonCloudResource (instance size 68, VMT 557CA7E0) }
    public partial class TPoisonCloudResource : Aow.Units.HSEngine.TMultiHexMOR
    {
        public TPoisonCloudResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // PoisonC.pas:286
        public new const int ClassID = 135744;

        // PoisonC.pas:292
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 135743;
            return Result;
        }

        // PoisonC.pas:289
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__34 = Aow.Units.AoWE.AoWE.PoisonCloudRStr;
            var call__35 = Aow.Units.AoWE.AoWE.TranslateRStr(call__34);
            Result = call__35;
            return Result;
        }

        public new partial class Meta : Aow.Units.HSEngine.TMultiHexMOR.Meta
        {
            // PoisonC.pas:286
            public override int ClassID() => Aow.Units.PoisonC.TPoisonCloudResource.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.PoisonC.TPoisonCloudResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.PoisonC.TPoisonCloudResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.PoisonC.TPoisonCloudResource)base.Create(AOwner);
        }
    }

    public static partial class PoisonC
    {
        // PoisonC.pas:295
        public static void RegisterPoisonCloud(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.PoisonC.TPoisonCloud>(), Aow.Units.Engine.EClass.Of<Aow.Units.PoisonC.TPoisonCloudResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
