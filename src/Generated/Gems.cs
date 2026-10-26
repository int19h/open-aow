// Generated from Gems.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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

namespace Aow.Units.Gems
{
    // { ot=1 }
    // { TGems (instance size 36, VMT 557C4D54): $1C FAmount, $20 FGenerateMode. }
    public partial class TGems : Aow.Units.ILTer.TILTerrainMO
    {
        public int FAmount;
        public Aow.Units.Gems.TGemsGenerateMode FGenerateMode;
        public Aow.Units.FixedArray_byte_3 F_Gems_21;
        // Gems.pas:56
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FAmount, 5);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FGenerateMode, 1), 1, 6);
        }

        // Gems.pas:46
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FGenerateMode = Aow.Units.Gems.TGemsGenerateMode.gmNormal;
            FAmount = 0;
        }

        // Gems.pas:93
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if ((Msg.MsgID == 537001985) && (Aow.Units.AoWE.AoWE.AoWHSMap.FDay == 1))
            {
                GenerateGemsAmount();
            }
        }

        // Gems.pas:119
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            // { Unmatched: tried record cast of dereferenced AI info pointer; zero-shift AI info address; untyped pointer var argument rejected by compiler; restored. }
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4354)
            {
                ArmyEntered(Msg.Param[0].As<Aow.Units.AoWE.TArmyHS>());
                return;
            }

            if ((Msg.MsgID == 4608) && (Msg.HN == 0))
            {
                /* TODO(T-A3) Integer address requires managed pointer provenance */
                throw new System.NotImplementedException();
                /* TODO(T-A3) Integer address requires managed pointer provenance */
                throw new System.NotImplementedException();
            }
        }

        // Gems.pas:87
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            if (Info.HN == 0)
            {
                ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 134217728;
            }
        }

        // Gems.pas:63
        public void GenerateGemsAmount()
        {
            if (FGenerateMode == Aow.Units.Gems.TGemsGenerateMode.gmRandom)
            {
                var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(3);
                FGenerateMode = (Aow.Units.Gems.TGemsGenerateMode)(call__1 + 1);
            }

            switch (FGenerateMode)
            {
                case Aow.Units.Gems.TGemsGenerateMode.gmLow:
                    var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(25);
                    FAmount = call__2 + 25;
                    break;
                case Aow.Units.Gems.TGemsGenerateMode.gmNormal:
                    var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(25);
                    FAmount = call__3 + 50;
                    break;
                case Aow.Units.Gems.TGemsGenerateMode.gmHigh:
                    var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(50);
                    FAmount = call__4 + 100;
                    break;
            }
        }

        // Gems.pas:74
        public void UpdateAITarget(ref Aow.Units.AoWE.TAITargetInfo Info, ref int Priority)
        {
            int Value = default;
            switch (Info.Mode)
            {
                case >= Aow.Units.AoWE.TAITargetMode.tmAttack and <= Aow.Units.AoWE.TAITargetMode.tmAttack:
                    if (Info.Player != 0)
                    {
                        Value = (FAmount / 5) + 15;
                        if (Value > Priority)
                        {
                            Priority = Value;
                        }
                    }

                    break;
            }
        }

        // Gems.pas:99
        public void ArmyEntered(Aow.Units.AoWE.TArmyHS ArmyHS)
        {
            Aow.Units.EventLog.TLocationTextEventLog Event = default;
            if (ArmyHS.FArmy.FPlayer != 0)
            {
                bool bool__5 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == ArmyHS.FArmy.FPlayer);
                if (!bool__5)
                {
                    var call__6 = GetLhx();
                    var call__7 = GetYhx();
                    var call__8 = GetXhx();
                    var call__9 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)call__8), unchecked((sbyte)call__7), unchecked((sbyte)call__6), 0);
                    bool__5 = call__9;
                }

                if (bool__5)
                {
                    Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\EVENTS\\GOLD.WAV");
                }

                {
                    var w__10 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
                    w__10.SetGems(w__10.FGems + FAmount);
                }

                Event = Aow.Units.EventLog.TLocationTextEventLog.Create(null);
                Event.FPlayer = unchecked((sbyte)ArmyHS.FArmy.FPlayer);
                /* TODO(T-4.3) Address needs a cell or array backing */
                throw new System.NotImplementedException();
                var call__12 = GetLhx();
                var call__13 = GetYhx();
                var call__14 = GetXhx();
                Event.SetLocation(unchecked((sbyte)call__14), unchecked((sbyte)call__13), unchecked((sbyte)call__12));
                var call__15 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
                call__15.FEventLogbook.AddEvent(Event);
                Event.Release();
                Destroy();
            }
        }

        // Gems.pas:24
        public static new Aow.Units.Gems.TGems Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Gems.TGems o = new Aow.Units.Gems.TGems();
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
            // Gems.pas:53
            public override int ClassID()
            {
                int Result = default;
                Result = 132014;
                return Result;
            }

            // Gems.pas:24
            public new Aow.Units.Gems.TGems Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Gems.TGems o = (Aow.Units.Gems.TGems)NewInstance();
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
                where T : Aow.Units.Gems.TGems => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TGemsResource (instance size 68, VMT 557C4EF0): no additional fields. }
    public partial class TGemsResource : Aow.Units.ILTer.TILTerrainMOR
    {
        // Gems.pas:140
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132014;
            return Result;
        }

        public new partial class Meta : Aow.Units.ILTer.TILTerrainMOR.Meta
        {
            // Gems.pas:137
            public override int ClassID()
            {
                int Result = default;
                Result = 132015;
                return Result;
            }

            // Gems.pas:134
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                /* TODO(T-4.3) Address needs a cell or array backing */
                throw new System.NotImplementedException();
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.Gems.TGemsResource => (Meta)MetaRegistry.Of(typeof(T));
            // HSEngine.pas:1194
            public new Aow.Units.Gems.TGemsResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.Gems.TGemsResource o = (Aow.Units.Gems.TGemsResource)NewInstance();
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

    public enum TGemsGenerateMode : byte
    {
        gmUserDefined = 0,
        gmLow = 1,
        gmNormal = 2,
        gmHigh = 3,
        gmRandom = 4
    }

    public static partial class Gems
    {
        // Gems.pas:143
        public static void RegisterGems(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Gems.TGems>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.Gems.TGemsResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
