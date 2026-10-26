// Generated from ManaCrystals.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FSize = 2;
            FAmount = 0;
        }

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
                    /* TODO(T-A3) Integer address requires managed pointer provenance */
                    throw new System.NotImplementedException();
                    // { Original marks the message only when the resulting priority is negative. }
                    /* TODO(T-A3) Integer address requires managed pointer provenance */
                    throw new System.NotImplementedException();
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
                var call__2 = Aow.Units.ManaCrystals.ManaCrystals.CrystalRandom(Aow.Units.AoWE.AoWE.AoWHSMap, 3);
                FSize = unchecked((byte)(call__2 + 1));
            }

            switch (FSize)
            {
                case 1:
                    var call__3 = Aow.Units.ManaCrystals.ManaCrystals.CrystalRandom(Aow.Units.AoWE.AoWE.AoWHSMap, 25);
                    FAmount = call__3 + 25;
                    break;
                case 2:
                    var call__4 = Aow.Units.ManaCrystals.ManaCrystals.CrystalRandom(Aow.Units.AoWE.AoWE.AoWHSMap, 25);
                    FAmount = call__4 + 50;
                    break;
                case 3:
                    var call__5 = Aow.Units.ManaCrystals.ManaCrystals.CrystalRandom(Aow.Units.AoWE.AoWE.AoWHSMap, 50);
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

            {
                var w__11 = Aow.Units.ManaCrystals.ManaCrystals.CrystalPlayer(Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers, ArmyHS.FArmy.FPlayer);
                ((Aow.Units.ManaCrystals.TCrystalMagic)w__11.FMagicControl).Mana = ((Aow.Units.ManaCrystals.TCrystalMagic)w__11.FMagicControl).Mana + FAmount;
            }

            Log = Aow.Units.EventLog.TLocationTextEventLog.Create(null);
            Log.FPlayer = unchecked((sbyte)unchecked((byte)ArmyHS.FArmy.FPlayer));
            /* TODO(T-4.3) Address needs a cell or array backing */
            throw new System.NotImplementedException();
            var call__14 = GetLhx();
            var call__15 = GetYhx();
            var call__16 = GetXhx();
            Log.SetLocation(unchecked((sbyte)call__16), unchecked((sbyte)call__15), unchecked((sbyte)call__14));
            var call__17 = Aow.Units.ManaCrystals.ManaCrystals.CrystalPlayer(Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers, ArmyHS.FArmy.FPlayer);
            call__17.FEventLogbook.AddEvent(Log);
            Log.Release();
            Destroy();
        }

        // ManaCrystals.pas:28
        public static new Aow.Units.ManaCrystals.TManaCrystals Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.ManaCrystals.TManaCrystals o = new Aow.Units.ManaCrystals.TManaCrystals();
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

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMO.Meta
        {
            // ManaCrystals.pas:80
            public override int ClassID()
            {
                int Result = default;
                Result = 132030;
                return Result;
            }

            // ManaCrystals.pas:28
            public new Aow.Units.ManaCrystals.TManaCrystals Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.ManaCrystals.TManaCrystals o = (Aow.Units.ManaCrystals.TManaCrystals)NewInstance();
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
                where T : Aow.Units.ManaCrystals.TManaCrystals => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TManaCrystalsResource (instance size 68, VMT 557B4B00) }
    public partial class TManaCrystalsResource : Aow.Units.ILTer.TILTerrainMOR
    {
        // ManaCrystals.pas:171
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132030;
            return Result;
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMOR.Meta
        {
            // ManaCrystals.pas:168
            public override int ClassID()
            {
                int Result = default;
                Result = 132031;
                return Result;
            }

            // ManaCrystals.pas:165
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.ManaCrystals.ManaCrystals.TranslateCrystalName(Aow.Units.AoWE.AoWE.ManaCrystalsRStr);
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.ManaCrystals.TManaCrystalsResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.ManaCrystals.TManaCrystalsResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.ManaCrystals.TManaCrystalsResource o = (Aow.Units.ManaCrystals.TManaCrystalsResource)NewInstance();
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

    public partial class TCrystalMagic : Aow.Units.AoWE.TPlayerMagicControl
    {
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
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.ManaCrystals.TManaCrystals>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.ManaCrystals.TManaCrystalsResource>()]);
        }

        // ManaCrystals.pas:65
        // { needs: the draft methods return integers and objects, and Watching returns Boolean. }
        public static int CrystalRandom(Aow.Units.AoWE.TAoWHSMap Map, int Range)
        {
            int Result = default;
            Result = Map.Random(Range);
            return Result;
        }

        // ManaCrystals.pas:67
        public static Aow.Units.AoWE.TPlayer CrystalPlayer(Aow.Units.AoWE.TPlayerList List, int Player)
        {
            Aow.Units.AoWE.TPlayer Result = default;
            Result = List.GetPlayers(Player);
            return Result;
        }

        // ManaCrystals.pas:69
        public static Aow.Rtl.AnsiString TranslateCrystalName(Aow.Rtl.AnsiString S)
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(S);
            return Result;
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
