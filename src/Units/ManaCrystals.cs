// Translated from ManaCrystals.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using static Aow.Units.Sound.Sound;
using Aow.Units.HSEngine;
using Aow.Units.ILTer;
using Aow.Units.EventLog;
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

    // FAmount, FSize
    public partial class TManaCrystals : TILTerrainMO
    {
        public TManaCrystals(TECustomNode owner) : base(owner)
        {
            FSize = 2;
            FAmount = 0;
        }

        // ManaCrystals.pas:80
        public new const int ClassID = 132030;

        public int FAmount;
        public byte FSize;
        // ManaCrystals.pas:83
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FAmount, 5);
            Stream.rwData(Bytes.Of(ref FSize, 1), 1, 6);
        }

        // ManaCrystals.pas:73


        // ManaCrystals.pas:118
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001985 && AoWHSMap.FDay == 1)
            {
                GenerateManaCrystalsAmount();
            }
        }

        // ManaCrystals.pas:148
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4354)
            {
                ArmyEntered(Msg.Param[0].As<TArmyHS>());
                return;
            }

            if (Msg.MsgID == 4608)
            {
                var call__1 = FResource.TerrainList.Terrain[Msg.HN];
                if (call__1 >= 0)
                {
                    var query = (AITargetQuery)Msg.Param[0].Ref;
                    var info = new TCrystalAIInfo { Player = query.Info.Player, OtherPlayer = query.Info.IndependentRelation, Kind = (byte)query.Info.Mode };
                    UpdateAITarget(ref info, ref query.Data.Priority);
                    query.Info.Player = info.Player;
                    query.Info.IndependentRelation = info.OtherPlayer;
                    query.Info.Mode = (TAITargetMode)info.Kind;
                    // { Original marks the message only when the resulting priority is negative. }
                    if (((AITargetQuery)Msg.Param[0].Ref).Data.Priority < 0)
                    {
                        Msg.Result = 1;
                    }
                }
            }
        }

        // ManaCrystals.pas:111
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            if (Info.HN == 0)
            {
                ((TAoWMapField)(object)Info.MapField).FFieldFlags = ((TAoWMapField)(object)Info.MapField).FFieldFlags | 134217728;
            }
        }

        // ManaCrystals.pas:90
        public void GenerateManaCrystalsAmount()
        {
            if (FSize == 4)
            {
                var call__2 = AoWHSMap.Random(3);
                FSize = (byte)(call__2 + 1);
            }

            switch (FSize)
            {
                case 1:
                    var call__3 = AoWHSMap.Random(25);
                    FAmount = call__3 + 25;
                    break;
                case 2:
                    var call__4 = AoWHSMap.Random(25);
                    FAmount = call__4 + 50;
                    break;
                case 3:
                    var call__5 = AoWHSMap.Random(50);
                    FAmount = call__5 + 100;
                    break;
            }
        }

        // ManaCrystals.pas:100
        public void UpdateAITarget(ref TCrystalAIInfo Info, ref int Priority)
        {
            int P = default;
            switch (Info.Kind)
            {
                case 0:
                    if (Info.Player != 0)
                    {
                        P = FAmount / 5 + 15;
                        if (Priority < P)
                        {
                            Priority = P;
                        }
                    }

                    break;
            }
        }

        // ManaCrystals.pas:125
        public void ArmyEntered(TArmyHS ArmyHS)
        {
            TLocationTextEventLog Log = default;
            if ((byte)ArmyHS.FArmy.FPlayer == 0)
            {
                return;
            }

            bool bool__6 = (byte)AoWHSMap.FSeatedPlayer == (byte)ArmyHS.FArmy.FPlayer;
            if (!bool__6)
            {
                var call__7 = GetLhx();
                var call__8 = GetYhx();
                var call__9 = GetXhx();
                var call__10 = AoWHSMap.Watching(call__9, call__8, call__7, 0);
                bool__6 = call__10;
            }

            if (bool__6)
            {
                PlaySample((AnsiString)"SFX\\EVENTS\\MANA.WAV");
            }

            var player = AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
            player.FMagicControl.SetMana(player.FMagicControl.FMana + FAmount);

            Log = new TLocationTextEventLog(null);
            Log.FPlayer = (sbyte)(byte)ArmyHS.FArmy.FPlayer;
            var call__12 = TranslateRStr(ManaCrystalsFoundRStr);
            var call__13 = Rtl.Rtl.Format(call__12, [IntToStr(FAmount)]);
            Log.FText = call__13;
            var call__14 = GetLhx();
            var call__15 = GetYhx();
            var call__16 = GetXhx();
            Log.SetLocation(call__16, call__15, call__14);
            var call__17 = AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
            call__17.FEventLogbook.AddEvent(Log);
            Log.Release();
            Destroy();
        }

        // ManaCrystals.pas:28


    }

    public partial class TManaCrystalsResource : TILTerrainMOR
    {
        public TManaCrystalsResource(TECustomNode owner) : base(owner)
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
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(ManaCrystalsRStr);
            return Result;
        }

    }

    public partial class TCrystalMagic : TPlayerMagicControl
    {
        public TCrystalMagic(TECustomNode owner) : base(owner)
        { }

        public int Mana { get => FMana; set => SetMana(value); }
    }

    [StructLayout(LayoutKind.Explicit, Size = 166)]
    public struct TCrystalMapFields
    {
        [FieldOffset(0)]
        public FixedArray_byte_165 Reserved;
        [FieldOffset(165)]
        public sbyte SeatedPlayer;
    }

    [StructLayout(LayoutKind.Explicit, Size = 28)]
    public struct TCrystalFieldFlags
    {
        [FieldOffset(0)]
        public FixedArray_byte_24 Reserved;
        [FieldOffset(24)]
        public int Flags;
    }

    public static partial class ManaCrystals
    {
        // ManaCrystals.pas:174
        public static void RegisterManaCrystals(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TManaCrystals>(), EClass.Of<TManaCrystalsResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
