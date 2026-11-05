// Translated from Gems.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using static Aow.Units.Sound.Sound;
using Aow.Units.HSEngine;
using Aow.Units.ILTer;
using Aow.Units.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Gems
{
    // { ot=1 }
    // FAmount, FGenerateMode
    public partial class TGems : TILTerrainMO
    {
        public TGems(TECustomNode owner) : base(owner)
        {
            FGenerateMode = TGemsGenerateMode.gmNormal;
            FAmount = 0;
        }

        // Gems.pas:53
        public new const int ClassID = 132014;

        public int FAmount;
        public TGemsGenerateMode FGenerateMode;
        // Gems.pas:56
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FAmount, 5);
            Stream.rwData(Bytes.Of(ref FGenerateMode, 1), 1, 6);
        }

        // Gems.pas:46


        // Gems.pas:93
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001985 && AoWHSMap.FDay == 1)
            {
                GenerateGemsAmount();
            }
        }

        // Gems.pas:119
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4354)
            {
                ArmyEntered(Msg.Param[0].As<TArmyHS>());
                return;
            }

            if (Msg.MsgID == 4608 && Msg.HN == 0)
            {
                UpdateAITarget(ref ((AITargetQuery)Msg.Param[0].Ref).Info, ref ((AITargetQuery)Msg.Param[0].Ref).Data.Priority);
                if (((AITargetQuery)Msg.Param[0].Ref).Data.Priority < 0)
                {
                    Msg.Result = 1;
                }
            }
        }

        // Gems.pas:87
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            if (Info.HN == 0)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 134217728;
            }
        }

        // Gems.pas:63
        public void GenerateGemsAmount()
        {
            if (FGenerateMode == TGemsGenerateMode.gmRandom)
            {
                var call__1 = AoWHSMap.Random(3);
                FGenerateMode = (TGemsGenerateMode)(call__1 + 1);
            }

            switch (FGenerateMode)
            {
                case TGemsGenerateMode.gmLow:
                    var call__2 = AoWHSMap.Random(25);
                    FAmount = call__2 + 25;
                    break;
                case TGemsGenerateMode.gmNormal:
                    var call__3 = AoWHSMap.Random(25);
                    FAmount = call__3 + 50;
                    break;
                case TGemsGenerateMode.gmHigh:
                    var call__4 = AoWHSMap.Random(50);
                    FAmount = call__4 + 100;
                    break;
            }
        }

        // Gems.pas:74
        public void UpdateAITarget(ref TAITargetInfo Info, ref int Priority)
        {
            int Value = default;
            switch (Info.Mode)
            {
                case >= TAITargetMode.tmAttack and <= TAITargetMode.tmAttack:
                    if (Info.Player != 0)
                    {
                        Value = FAmount / 5 + 15;
                        if (Value > Priority)
                        {
                            Priority = Value;
                        }
                    }

                    break;
            }
        }

        // Gems.pas:99
        public void ArmyEntered(TArmyHS ArmyHS)
        {
            TLocationTextEventLog Event = default;
            if (ArmyHS.FArmy.FPlayer != 0)
            {
                bool bool__5 = AoWHSMap.FSeatedPlayer == ArmyHS.FArmy.FPlayer;
                if (!bool__5)
                {
                    var call__6 = GetLhx();
                    var call__7 = GetYhx();
                    var call__8 = GetXhx();
                    var call__9 = AoWHSMap.Watching(call__8, call__7, call__6, 0);
                    bool__5 = call__9;
                }

                if (bool__5)
                {
                    PlaySample((AnsiString)"SFX\\EVENTS\\GOLD.WAV");
                }

                var player = AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
                player.SetGems(player.FGems + FAmount);

                Event = new TLocationTextEventLog(null);
                Event.FPlayer = ArmyHS.FArmy.FPlayer;
                var call__11 = GoldFoundRStr;
                var call__12 = TranslateRStr(call__11);
                var call__13 = Rtl.Rtl.Format(call__12, [IntToStr(FAmount)]);
                Event.FText = call__13;
                var call__14 = GetLhx();
                var call__15 = GetYhx();
                var call__16 = GetXhx();
                Event.SetLocation(call__16, call__15, call__14);
                var call__17 = AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
                call__17.FEventLogbook.AddEvent(Event);
                Event.Release();
                Destroy();
            }
        }

        // Gems.pas:24


    }

    public partial class TGemsResource : TILTerrainMOR
    {
        public TGemsResource(TECustomNode owner) : base(owner)
        {}

        // Gems.pas:137
        public new const int ClassID = 132015;

        // Gems.pas:140
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132014;
            return Result;
        }

        // Gems.pas:134
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            var call__18 = GoldRStr;
            var call__19 = TranslateRStr(call__18);
            Result = call__19;
            return Result;
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
        public static void RegisterGems(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TGems>(), EClass.Of<TGemsResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
