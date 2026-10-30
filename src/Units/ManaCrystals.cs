// Translated from ManaCrystals.pas.
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
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.ManaCrystals
{
    // { ManaCrystals of AoWEPACK.dpl: recovered bodies and original instance layouts.
//   Remaining inherited declaration constraints are documented at local ABI adapters. }
    [StructLayout(LayoutKind.Explicit, Size = 3)]
    public struct TCrystalAIInfo
    {
        [FieldOffset(0)]
        public sbyte Player;
        [FieldOffset(1)]
        public sbyte OtherPlayer;
        [FieldOffset(2)]
        public byte Kind;
    }

    // { TManaCrystals (instance size 36, VMT 557B494C): +$1C FAmount, +$20 FSize. }
    public partial class TManaCrystals : Aow.Units.ILTer.TILTerrainMO
    {
        public TManaCrystals(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FSize = 2;
            FAmount = 0;
        }

        // ManaCrystals.pas:80
        public new const int ClassID = 132030;

        public int FAmount;
        public byte FSize;
        public Aow.Units.FixedArray_byte_3 F_ManaCrystals_21;
        // ManaCrystals.pas:83
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FAmount, 5);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FSize, 1), 1, 6);
        }

        // ManaCrystals.pas:73


        // ManaCrystals.pas:118
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if ((Msg.MsgID == 537001985) && (Aow.Units.AoWE.AoWE.AoWHSMap.FDay == 1))
            {
                GenerateManaCrystalsAmount();
            }
        }

        // ManaCrystals.pas:148
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            // { Unmatched: tried record cast of dereferenced AI info pointer; zero-shift AI info address; untyped pointer var argument rejected by compiler; restored. }
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4354)
            {
                ArmyEntered(Msg.Param[0].As<Aow.Units.AoWE.TArmyHS>());
                return;
            }

            if (Msg.MsgID == 4608)
            {
                var call__1 = FResource.TerrainList.Terrain[Msg.HN];
                if (call__1 >= 0)
                {
                    var query = (Aow.Units.AoWE.AITargetQuery)Msg.Param[0].Ref;
                    var info = new TCrystalAIInfo { Player = query.Info.Player, OtherPlayer = query.Info.IndependentRelation, Kind = (byte)query.Info.Mode };
                    UpdateAITarget(ref info, ref query.Data.Priority);
                    query.Info.Player = info.Player;
                    query.Info.IndependentRelation = info.OtherPlayer;
                    query.Info.Mode = (Aow.Units.AoWE.TAITargetMode)info.Kind;
                    // { Original marks the message only when the resulting priority is negative. }
                    if (((Aow.Units.AoWE.AITargetQuery)Msg.Param[0].Ref).Data.Priority < 0)
                    {
                        Msg.Result = 1;
                    }
                }
            }
        }

        // ManaCrystals.pas:111
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            if (Info.HN == 0)
            {
                ((Aow.Units.AoWE.TAoWMapField)((System.Object)Info.MapField)).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)((System.Object)Info.MapField)).FFieldFlags | 134217728;
            }
        }

        // ManaCrystals.pas:90
        public void GenerateManaCrystalsAmount()
        {
            if (FSize == 4)
            {
                var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(3);
                FSize = unchecked((byte)(call__2 + 1));
            }

            switch (FSize)
            {
                case 1:
                    var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(25);
                    FAmount = call__3 + 25;
                    break;
                case 2:
                    var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(25);
                    FAmount = call__4 + 50;
                    break;
                case 3:
                    var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(50);
                    FAmount = call__5 + 100;
                    break;
            }
        }

        // ManaCrystals.pas:100
        public void UpdateAITarget(ref Aow.Units.ManaCrystals.TCrystalAIInfo Info, ref int Priority)
        {
            int P = default;
            switch (Info.Kind)
            {
                case 0:
                    if (Info.Player != 0)
                    {
                        P = (FAmount / 5) + 15;
                        if (Priority < P)
                        {
                            Priority = P;
                        }
                    }

                    break;
            }
        }

        // ManaCrystals.pas:125
        public void ArmyEntered(Aow.Units.AoWE.TArmyHS ArmyHS)
        {
            Aow.Units.EventLog.TLocationTextEventLog Log = default;
            if (unchecked((byte)ArmyHS.FArmy.FPlayer) == 0)
            {
                return;
            }

            bool bool__6 = (unchecked((byte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer) == unchecked((byte)ArmyHS.FArmy.FPlayer));
            if (!bool__6)
            {
                var call__7 = GetLhx();
                var call__8 = GetYhx();
                var call__9 = GetXhx();
                var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)call__9), unchecked((sbyte)call__8), unchecked((sbyte)call__7), 0);
                bool__6 = call__10;
            }

            if (bool__6)
            {
                Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\EVENTS\\MANA.WAV");
            }

            var player = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers((int)(ArmyHS.FArmy.FPlayer));
            player.FMagicControl.SetMana(player.FMagicControl.FMana + FAmount);

            Log = new Aow.Units.EventLog.TLocationTextEventLog(null);
            Log.FPlayer = unchecked((sbyte)unchecked((byte)ArmyHS.FArmy.FPlayer));
            var call__12 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ManaCrystalsFoundRStr);
            var call__13 = Aow.Rtl.Rtl.Format(call__12, [Aow.Rtl.Rtl.IntToStr(FAmount)]);
            Log.FText = call__13;
            var call__14 = GetLhx();
            var call__15 = GetYhx();
            var call__16 = GetXhx();
            Log.SetLocation(unchecked((sbyte)call__16), unchecked((sbyte)call__15), unchecked((sbyte)call__14));
            var call__17 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers((int)(ArmyHS.FArmy.FPlayer));
            call__17.FEventLogbook.AddEvent(Log);
            Log.Release();
            Destroy();
        }

        // ManaCrystals.pas:28


        public new partial class Meta : Aow.Units.ILTer.TILTerrainMO.Meta
        {
            // ManaCrystals.pas:80
            public override int ClassID() => Aow.Units.ManaCrystals.TManaCrystals.ClassID;

            // ManaCrystals.pas:28
            public new Aow.Units.ManaCrystals.TManaCrystals Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.ManaCrystals.TManaCrystals)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.ManaCrystals.TManaCrystals => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TManaCrystalsResource (instance size 68, VMT 557B4B00) }
    public partial class TManaCrystalsResource : Aow.Units.ILTer.TILTerrainMOR
    {
        public TManaCrystalsResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // ManaCrystals.pas:168
        public new const int ClassID = 132031;

        // ManaCrystals.pas:171
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132030;
            return Result;
        }

        // ManaCrystals.pas:165
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ManaCrystalsRStr);
            return Result;
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMOR.Meta
        {
            // ManaCrystals.pas:168
            public override int ClassID() => Aow.Units.ManaCrystals.TManaCrystalsResource.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.ManaCrystals.TManaCrystalsResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.ManaCrystals.TManaCrystalsResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.ManaCrystals.TManaCrystalsResource)base.Create(AOwner);
        }
    }

    public partial class TCrystalMagic : Aow.Units.AoWE.TPlayerMagicControl
    {
        public TCrystalMagic(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        public int Mana { get => FMana; set => SetMana(value); }
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct TCrystalMapFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public sbyte SeatedPlayer;
    }

    [StructLayout(LayoutKind.Explicit, Size = 28)]
    public struct TCrystalFieldFlags
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_24 Reserved;
        [FieldOffset(24)]
        public int Flags;
    }

    public static partial class ManaCrystals
    {
        // ManaCrystals.pas:174
        public static void RegisterManaCrystals(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.ManaCrystals.TManaCrystals>(), Aow.Units.Engine.EClass.Of<Aow.Units.ManaCrystals.TManaCrystalsResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
