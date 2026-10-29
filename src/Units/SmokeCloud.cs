// Translated from SmokeCloud.pas.
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

namespace Aow.Units.SmokeCloud
{
    public enum TCloudEnchantmentOptionsElement : byte
    {
        ce0 = 0,
        ce1 = 1,
        ce2 = 2,
        ce3 = 3,
        ce4 = 4,
        ce5 = 5,
        ce6 = 6,
        ce7 = 7
    }

    // { TSmokeCloudEnchantment (instance size 40, VMT 5579BF2C): +$24 FCloud. }
    public partial class TSmokeCloudEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        public Aow.Units.SmokeCloud.TSmokeCloud FCloud;
        // SmokeCloud.pas:91
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FEnchantmentMode = TSet<Aow.Units.AoWE.TGlobalEnchantmentModeElement>.FromMask((TSet<Aow.Units.SmokeCloud.TCloudEnchantmentOptionsElement>.FromMask((ulong)FEnchantmentMode.Mask) - TSet<Aow.Units.SmokeCloud.TCloudEnchantmentOptionsElement>.Of(Aow.Units.SmokeCloud.TCloudEnchantmentOptionsElement.ce0, Aow.Units.SmokeCloud.TCloudEnchantmentOptionsElement.ce1)).Mask);
            FDispelMana = 25;
        }

        // SmokeCloud.pas:102
        public override Aow.Rtl.AnsiString GetName()
        {
            Aow.Rtl.AnsiString Result = default;
            // { needs: the base GetName parameter is a hidden string result pointer. }
            var call__1 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(74);
            Result = call__1.FSpellName;
            return Result;
        }

        // SmokeCloud.pas:156
        public override bool VisibleForPlayer(sbyte Player)
        {
            bool Result = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Result = FPlayerIndex == Player;
            ref var walk = ref Walk;
            var call__3 = FCloud.Yhx;
            var call__4 = FCloud.Xhx;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, call__4, call__3, 0);
            while (!Result && (walk.Radius <= 3))
            {
                var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(walk.X, walk.Y, FCloud.Lhx, unchecked((sbyte)Player));
                Result = call__5;
                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // SmokeCloud.pas:21
        public static new Aow.Units.SmokeCloud.TSmokeCloudEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.SmokeCloud.TSmokeCloudEnchantment o = new Aow.Units.SmokeCloud.TSmokeCloudEnchantment();
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

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // SmokeCloud.pas:99
            public override int ClassID()
            {
                int Result = default;
                Result = 135748;
                return Result;
            }

            // SmokeCloud.pas:21
            public new Aow.Units.SmokeCloud.TSmokeCloudEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.SmokeCloud.TSmokeCloudEnchantment o = (Aow.Units.SmokeCloud.TSmokeCloudEnchantment)NewInstance();
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
                where T : Aow.Units.SmokeCloud.TSmokeCloudEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TSmokeCloud (instance size 36, VMT 5579C044): +$14 FPlayer, +$18 FTurnsRemaining,
//     +$1C FDrifting, +$1D FDirection, +$1E FUpdated, +$1F FVisibleForSeatedPlayer, +$20 FEnchantment. }
    public partial class TSmokeCloud : Aow.Units.HSEngine.TMultiHexMO
    {
        public sbyte FPlayer;
        public Aow.Units.FixedArray_byte_3 FReserved;
        public int FTurnsRemaining;
        public bool FDrifting;
        public byte FDirection;
        public bool FUpdated;
        public bool FVisibleForSeatedPlayer;
        public Aow.Units.SmokeCloud.TSmokeCloudEnchantment FEnchantment;
        // SmokeCloud.pas:181
        public override void Destroy()
        {
            base.Destroy();
            FEnchantment.Release();
        }

        // SmokeCloud.pas:206
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 15);
            Stream.rwInteger(ref FTurnsRemaining, 16);
            Stream.rwBool(ref FDrifting, 17);
            Stream.rwByte(ref FDirection, 18);
            Stream.rwEObject(FEnchantment, 19);
        }

        // SmokeCloud.pas:172
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__6 = Aow.Units.SmokeCloud.TSmokeCloudEnchantment.Create(null);
            FEnchantment = call__6;
            FEnchantment.FCloud = this;
            FVisibleForSeatedPlayer = false;
            FDrifting = false;
        }

        // SmokeCloud.pas:373
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
            else
            {
                if (Msg.MsgID == 537001993)
                {
                    UpdateVisibleForSeatedPlayer();
                }
            }
        }

        // SmokeCloud.pas:190
        public override int GetShowPriority()
        {
            int Result = default;
            Result = base.GetShowPriority() + 201326592;
            return Result;
        }

        // SmokeCloud.pas:300
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            if (FVisibleForSeatedPlayer || Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // SmokeCloud.pas:266
        public override bool PlaceOnMap()
        {
            bool Result = default;
            Result = base.PlaceOnMap();
            if (Result)
            {
                UpdateMapFields((Aow.Units.HSEngine.TMapLevel)Owner.Owner, FXhx, FYhx);
            }

            return Result;
        }

        // SmokeCloud.pas:272
        public override void RemoveFromMap()
        {
            Aow.Units.HSEngine.TMapLevel Level = default;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // SmokeCloud.pas:245
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            if ((Msg.MsgID != 16) || Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                base.MapFieldMsgProc(ref Msg);
            }

            var call__7 = FResource.TerrainList.Terrain[Msg.HN];
            if (call__7 >= -1)
            {
                if ((Msg.MsgID == 4392) && FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moActivated))
                {
                    Msg.Param[0] = 1;
                }
                else
                {
                    if (Msg.MsgID == 4365)
                    {
                        UpdateVisibleForSeatedPlayer();
                    }
                }
            }
        }

        // SmokeCloud.pas:281
        public override void Activate()
        {
            base.Activate();
            if (FDirection == 0)
            {
                var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(6);
                FDirection = unchecked((byte)(call__8 + 1));
            }

            var smokeCloudEnchantment = FEnchantment;
            if ((smokeCloudEnchantment.FPlayerIndex > 0) && !smokeCloudEnchantment.FEnchantmentStatus.Contains(Aow.Units.AoWE.TGlobalEnchantmentStatusElement.getActivated))
            {
                smokeCloudEnchantment.Activate(unchecked((sbyte)smokeCloudEnchantment.FPlayerIndex));
            }

            var call__10 = Yhx;
            var call__11 = Xhx;
            UpdateConcealment(unchecked((sbyte)call__11), unchecked((sbyte)call__10), unchecked((sbyte)Lhx));
            UpdateVisibleForSeatedPlayer();
        }

        // SmokeCloud.pas:292
        public override void Deactivate()
        {
            base.Deactivate();
            if (FEnchantment.FPlayerIndex > 0)
            {
                FEnchantment.Deactivate(unchecked((byte)FEnchantment.FPlayerIndex));
            }

            var call__12 = GetLhx();
            var call__13 = GetYhx();
            var call__14 = GetXhx();
            UpdateConcealment(unchecked((sbyte)call__14), unchecked((sbyte)call__13), unchecked((sbyte)call__12));
        }

        // SmokeCloud.pas:238
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            if ((Info.HN > 18) && (Info.HN < 37))
            {
                ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 16777216;
            }
        }

        // SmokeCloud.pas:193
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            int Xhx = default;
            int Yhx = default;
            int HN = default;
            if (FResource.Engine.HSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                Aow.Units.HSEngine.HSEngine.HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
                var call__15 = Aow.Units.HSEngine.HSEngine.dHXtoHN(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, Xhx, Yhx);
                HN = call__15;
                bool bool__16 = (HN < FResource.TerrainList.Count);
                if (bool__16)
                {
                    var call__17 = FResource.TerrainList.Terrain[HN];
                    bool__16 = (call__17 >= -1);
                }

                Result = bool__16;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // SmokeCloud.pas:216
        public void UpdateConcealment(sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.AoWE.TAoWMapField MapField = default;
            Aow.Units.AoWE.TArmyHS ArmyHS = default;
            var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.MapLevels[Lhx];
            Level = call__18;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (FResource.TerrainList.Count > Walk.HN)
            {
                var call__19 = FResource.TerrainList.Terrain[Walk.HN];
                if (((((call__19 >= -1) && (Walk.X >= 0)) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Level.Height > Walk.Y))
                {
                    var call__20 = Level.GetField(Walk.X, Walk.Y);
                    MapField = (Aow.Units.AoWE.TAoWMapField)call__20;
                    ArmyHS = MapField.GetArmyHS();
                    if (ArmyHS != null)
                    {
                        ArmyHS.FArmy.Update();
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }
        }

        // SmokeCloud.pas:253
        public void UpdateVisibleForSeatedPlayer()
        {
            bool Visible = default;
            var call__21 = GetLhx();
            var call__22 = GetYhx();
            var call__23 = GetXhx();
            var call__24 = Aow.Units.AoWE.AoWE.AoWHSMap.Visible(unchecked((sbyte)call__23), unchecked((sbyte)call__22), unchecked((sbyte)call__21), 3);
            Visible = call__24 || (FEnchantment.FPlayerIndex == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            if (FVisibleForSeatedPlayer ^ Visible)
            {
                FVisibleForSeatedPlayer = Visible;
                Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.TriggerChangedEvent();
            }
        }

        // SmokeCloud.pas:306
        public bool ValidMoveDir(int Direction)
        {
            bool Result = default;
            int Xhx = default;
            int Yhx = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            var call__25 = GetYhx();
            var call__26 = GetXhx();
            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__26, call__25, Direction, ref Xhx, ref Yhx);
            Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
            Result = (((Xhx >= 10) && (Yhx >= 10)) && (Level.Width >= (Xhx + 10))) && (Level.Height >= (Yhx + 10));
            return Result;
        }

        // SmokeCloud.pas:317
        public void UpdateCloud()
        {
            int Xhx = default;
            int Yhx = default;
            Aow.Units.Engine.TByteList Directions = default;
            bool Valid = default;
            int Direction = default;
            int Tries = default;
            int Lhx = default;
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
                        var call__27 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(Directions.Count);
                        var call__28 = Directions[call__27];
                        Direction = call__28;
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
                            var call__29 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(6);
                            Direction = call__29 + 1;
                            Valid = ValidMoveDir(Direction);
                            Tries -= 1;
                        }
                    }

                    if (Valid)
                    {
                        FDirection = unchecked((byte)Direction);
                        Lhx = GetLhx();
                        var call__30 = GetYhx();
                        var call__31 = GetXhx();
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(call__31, call__30, Direction, ref Xhx, ref Yhx);
                        Aow.Units.HSEngine.HSEngine.CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
                        Disconnect();
                        UpdateConcealment(unchecked((sbyte)FXhx), unchecked((sbyte)FYhx), unchecked((sbyte)Lhx));
                        FXhx = unchecked((sbyte)Xhx);
                        FYhx = unchecked((sbyte)Yhx);
                        var call__32 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(Xhx, Yhx, Lhx);
                        Owner = call__32;
                        Connect();
                        UpdateConcealment(unchecked((sbyte)FXhx), unchecked((sbyte)FYhx), unchecked((sbyte)Lhx));
                    }
                }
            }
            else
            {
                Destroy();
            }
        }

        // SmokeCloud.pas:43
        public static new Aow.Units.SmokeCloud.TSmokeCloud Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.SmokeCloud.TSmokeCloud o = new Aow.Units.SmokeCloud.TSmokeCloud();
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

        public new partial class Meta : Aow.Units.HSEngine.TMapObject.Meta
        {
            // SmokeCloud.pas:187
            public override int ClassID()
            {
                int Result = default;
                Result = 135746;
                return Result;
            }

            // SmokeCloud.pas:43
            public new Aow.Units.SmokeCloud.TSmokeCloud Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.SmokeCloud.TSmokeCloud o = (Aow.Units.SmokeCloud.TSmokeCloud)NewInstance();
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
                where T : Aow.Units.SmokeCloud.TSmokeCloud => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TSmokeCloudResource (instance size 68, VMT 5579C1E0) }
    public partial class TSmokeCloudResource : Aow.Units.HSEngine.TMultiHexMOR
    {
        // SmokeCloud.pas:391
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 135746;
            return Result;
        }

        public new partial class Meta : Aow.Units.HSEngine.TMultiHexMOR.Meta
        {
            // SmokeCloud.pas:385
            public override int ClassID()
            {
                int Result = default;
                Result = 135747;
                return Result;
            }

            // SmokeCloud.pas:388
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                var call__33 = Aow.Units.AoWE.AoWE.SmokeCloudRStr;
                var call__34 = Aow.Units.AoWE.AoWE.TranslateRStr(call__33);
                Result = call__34;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.SmokeCloud.TSmokeCloudResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.SmokeCloud.TSmokeCloudResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.SmokeCloud.TSmokeCloudResource o = (Aow.Units.SmokeCloud.TSmokeCloudResource)NewInstance();
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

    public struct TCloudSpellFields
    {
        public Aow.Units.FixedArray_byte_8 Reserved;
        public Aow.Rtl.AnsiString Name;
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct TCloudMapFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public byte SeatedPlayer;
    }

    public static partial class SmokeCloud
    {
        // SmokeCloud.pas:394
        public static void RegisterSmokeCloud(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.SmokeCloud.TSmokeCloud>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.SmokeCloud.TSmokeCloudResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.SmokeCloud.TSmokeCloudEnchantment>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
