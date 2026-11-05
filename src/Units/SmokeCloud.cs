// Translated from SmokeCloud.pas.
using Aow.Rtl;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
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

    // FCloud
    public partial class TSmokeCloudEnchantment : TGlobalEnchantment
    {
        // SmokeCloud.pas:99
        public new const int ClassID = 135748;

        public TSmokeCloud FCloud;
        // SmokeCloud.pas:91
        public TSmokeCloudEnchantment(TECustomNode owner) : base(owner)
        {
            FEnchantmentMode = TSet<TGlobalEnchantmentModeElement>.FromMask((TSet<TCloudEnchantmentOptionsElement>.FromMask(FEnchantmentMode.Mask) - TSet<TCloudEnchantmentOptionsElement>.Of(TCloudEnchantmentOptionsElement.ce0, TCloudEnchantmentOptionsElement.ce1)).Mask);
            FDispelMana = 25;
        }

        // SmokeCloud.pas:102
        public override AnsiString GetName()
        {
            AnsiString Result = default;
            var call__1 = AoWHSSet.FSpellControl.GetSpell(74);
            Result = call__1.FSpellName;
            return Result;
        }

        // SmokeCloud.pas:156
        public override bool VisibleForPlayer(sbyte Player)
        {
            bool Result = default;
            THNWalkData Walk = default;
            Result = FPlayerIndex == Player;
            ref var walk = ref Walk;
            var call__3 = FCloud.Yhx;
            var call__4 = FCloud.Xhx;
            InitHNWalk(ref Walk, call__4, call__3, 0);
            while (!Result && walk.Radius <= 3)
            {
                var call__5 = AoWHSMap.VisibleForPlayer(walk.X, walk.Y, FCloud.Lhx, Player);
                Result = call__5;
                WalkToNextHN(ref Walk);
            }

            return Result;
        }

    }

    // FPlayer, FTurnsRemaining, FDrifting, FDirection, FUpdated, FVisibleForSeatedPlayer,
    // FEnchantment
    public partial class TSmokeCloud : TMultiHexMO
    {
        public TSmokeCloud(TECustomNode owner) : base(owner)
        {
            var call__6 = new TSmokeCloudEnchantment(null);
            FEnchantment = call__6;
            FEnchantment.FCloud = this;
            FVisibleForSeatedPlayer = false;
            FDrifting = false;
        }

        // SmokeCloud.pas:187
        public new const int ClassID = 135746;

        public sbyte FPlayer;
        public int FTurnsRemaining;
        public bool FDrifting;
        public byte FDirection;
        public bool FUpdated;
        public bool FVisibleForSeatedPlayer;
        public TSmokeCloudEnchantment FEnchantment;
        // SmokeCloud.pas:181
        public override void Destroy()
        {
            base.Destroy();
            FEnchantment.Release();
        }

        // SmokeCloud.pas:206
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 15);
            Stream.rwInteger(ref FTurnsRemaining, 16);
            Stream.rwBool(ref FDrifting, 17);
            Stream.rwByte(ref FDirection, 18);
            Stream.rwEObject(FEnchantment, 19);
        }

        // SmokeCloud.pas:172


        // SmokeCloud.pas:373
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001986 && Msg.Value == FPlayer && !FUpdated)
            {
                UpdateCloud();
                return;
            }

            if (Msg.MsgID == 537001987 && Msg.Value == FPlayer)
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
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            if (FVisibleForSeatedPlayer || AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
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
                UpdateMapFields((TMapLevel)Owner.Owner, FXhx, FYhx);
            }

            return Result;
        }

        // SmokeCloud.pas:272
        public override void RemoveFromMap()
        {
            TMapLevel Level = default;
            Level = (TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(Level, FXhx, FYhx);
        }

        // SmokeCloud.pas:245
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            if (Msg.MsgID != 16 || AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                base.MapFieldMsgProc(ref Msg);
            }

            var call__7 = FResource.TerrainList.Terrain[Msg.HN];
            if (call__7 >= -1)
            {
                if (Msg.MsgID == 4392 && FStatus.Contains(TMapObjectStatusElement.moActivated))
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
                var call__8 = AoWHSMap.Random(6);
                FDirection = (byte)(call__8 + 1);
            }

            var smokeCloudEnchantment = FEnchantment;
            if (smokeCloudEnchantment.FPlayerIndex > 0 && !smokeCloudEnchantment.FEnchantmentStatus.Contains(TGlobalEnchantmentStatusElement.getActivated))
            {
                smokeCloudEnchantment.Activate(smokeCloudEnchantment.FPlayerIndex);
            }

            var call__10 = Yhx;
            var call__11 = Xhx;
            UpdateConcealment(call__11, call__10, Lhx);
            UpdateVisibleForSeatedPlayer();
        }

        // SmokeCloud.pas:292
        public override void Deactivate()
        {
            base.Deactivate();
            if (FEnchantment.FPlayerIndex > 0)
            {
                FEnchantment.Deactivate((byte)FEnchantment.FPlayerIndex);
            }

            var call__12 = GetLhx();
            var call__13 = GetYhx();
            var call__14 = GetXhx();
            UpdateConcealment(call__14, call__13, call__12);
        }

        // SmokeCloud.pas:238
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            if (Info.HN > 18 && Info.HN < 37)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 16777216;
            }
        }

        // SmokeCloud.pas:193
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            int Xhx = default;
            int Yhx = default;
            int HN = default;
            if (FResource.Engine.HSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                HPtoHX(Xhp, Yhp, ref Xhx, ref Yhx);
                var call__15 = dHXtoHN(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, Xhx, Yhx);
                HN = call__15;
                bool bool__16 = HN < FResource.TerrainList.Count;
                if (bool__16)
                {
                    var call__17 = FResource.TerrainList.Terrain[HN];
                    bool__16 = call__17 >= -1;
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
            THNWalkData Walk = default;
            TMapLevel Level = default;
            TAoWMapField MapField = default;
            TArmyHS ArmyHS = default;
            var call__18 = AoWHSMap.MapContainer.MapLevels[Lhx];
            Level = call__18;
            InitHNWalk(ref Walk, Xhx, Yhx, 0);
            while (FResource.TerrainList.Count > Walk.HN)
            {
                var call__19 = FResource.TerrainList.Terrain[Walk.HN];
                if (call__19 >= -1 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Level.Height > Walk.Y)
                {
                    var call__20 = Level.GetField(Walk.X, Walk.Y);
                    MapField = (TAoWMapField)call__20;
                    ArmyHS = MapField.GetArmyHS();
                    if (ArmyHS != null)
                    {
                        ArmyHS.FArmy.Update();
                    }
                }

                WalkToNextHN(ref Walk);
            }
        }

        // SmokeCloud.pas:253
        public void UpdateVisibleForSeatedPlayer()
        {
            bool Visible = default;
            var call__21 = GetLhx();
            var call__22 = GetYhx();
            var call__23 = GetXhx();
            var call__24 = AoWHSMap.Visible(call__23, call__22, call__21, 3);
            Visible = call__24 || FEnchantment.FPlayerIndex == AoWHSMap.FSeatedPlayer;
            if (FVisibleForSeatedPlayer ^ Visible)
            {
                FVisibleForSeatedPlayer = Visible;
                AoWHSMap.FGlobalMagicControl.TriggerChangedEvent();
            }
        }

        // SmokeCloud.pas:306
        public bool ValidMoveDir(int Direction)
        {
            bool Result = default;
            int Xhx = default;
            int Yhx = default;
            TMapLevel Level = default;
            Level = (TMapLevel)Owner.Owner;
            var call__25 = GetYhx();
            var call__26 = GetXhx();
            CenterHNtoHX(call__26, call__25, Direction, ref Xhx, ref Yhx);
            CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
            Result = Xhx >= 10 && Yhx >= 10 && Level.Width >= Xhx + 10 && Level.Height >= Yhx + 10;
            return Result;
        }

        // SmokeCloud.pas:317
        public void UpdateCloud()
        {
            int Xhx = default;
            int Yhx = default;
            TByteList Directions = default;
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
                    Directions = new TByteList();
                    Valid = false;
                    Directions.Add(FDirection);
                    if (FDirection != 1)
                    {
                        Directions.Add((byte)(FDirection - 1));
                    }
                    else
                    {
                        Directions.Add(6);
                    }

                    if (FDirection != 6)
                    {
                        Directions.Add((byte)(FDirection + 1));
                    }
                    else
                    {
                        Directions.Add(1);
                    }

                    while (Directions.Count != 0 && !Valid)
                    {
                        var call__27 = AoWHSMap.Random(Directions.Count);
                        var call__28 = Directions[call__27];
                        Direction = call__28;
                        Valid = ValidMoveDir(Direction);
                        if (!Valid)
                        {
                            Directions.Remove((byte)Direction);
                        }
                    }

                    Directions.Free();
                    if (!Valid)
                    {
                        Tries = 20;
                        while (!Valid && Tries != 0)
                        {
                            var call__29 = AoWHSMap.Random(6);
                            Direction = call__29 + 1;
                            Valid = ValidMoveDir(Direction);
                            Tries -= 1;
                        }
                    }

                    if (Valid)
                    {
                        FDirection = (byte)Direction;
                        Lhx = GetLhx();
                        var call__30 = GetYhx();
                        var call__31 = GetXhx();
                        CenterHNtoHX(call__31, call__30, Direction, ref Xhx, ref Yhx);
                        CenterHNtoHX(Xhx, Yhx, Direction, ref Xhx, ref Yhx);
                        Disconnect();
                        UpdateConcealment(FXhx, FYhx, (sbyte)Lhx);
                        FXhx = (sbyte)Xhx;
                        FYhx = (sbyte)Yhx;
                        var call__32 = AoWHSMap.MapContainer.GetField(Xhx, Yhx, Lhx);
                        Owner = call__32;
                        Connect();
                        UpdateConcealment(FXhx, FYhx, (sbyte)Lhx);
                    }
                }
            }
            else
            {
                Destroy();
            }
        }

        // SmokeCloud.pas:43


    }

    public partial class TSmokeCloudResource : TMultiHexMOR
    {
        public TSmokeCloudResource(TECustomNode owner) : base(owner)
        {}

        // SmokeCloud.pas:385
        public new const int ClassID = 135747;

        // SmokeCloud.pas:391
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 135746;
            return Result;
        }

        // SmokeCloud.pas:388
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            var call__33 = SmokeCloudRStr;
            var call__34 = TranslateRStr(call__33);
            Result = call__34;
            return Result;
        }

    }

    public struct TCloudSpellFields
    {
        public FixedArray_byte_8 Reserved;
        public AnsiString Name;
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct TCloudMapFields
    {
        [FieldOffset(0)]
        public FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public byte SeatedPlayer;
    }

    public static partial class SmokeCloud
    {
        // SmokeCloud.pas:394
        public static void RegisterSmokeCloud(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TSmokeCloud>(), EClass.Of<TSmokeCloudResource>(), EClass.Of<TSmokeCloudEnchantment>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
