// Translated from ManaCrystals.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.AI;
using Aow.Game.Audio;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

// FAmount, FSize
public partial class ManaCrystals : ImageLibraryTerrainMapObject
{
    public ManaCrystals(ECustomNode owner) : base(owner)
    {
        FSize = 2;
        FAmount = 0;
    }

    // ManaCrystals.pas:80
    public new const int ClassID = 132030;

    public int FAmount;
    public byte FSize;

    // ManaCrystals.pas:83
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FAmount, 5);
        Stream.rwData(Bytes.Of(ref FSize, 1), 1, 6);
    }

    // ManaCrystals.pas:73

    // ManaCrystals.pas:118
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001985 && AoWHSMap.Current.FDay == 1)
        {
            GenerateManaCrystalsAmount();
        }
    }

    // ManaCrystals.pas:148
    public override void MapFieldMsgProc(ref MapFieldMessage Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4354)
        {
            ArmyEntered(Msg.Param[0].As<ArmyHexSprite>());
            return;
        }

        if (Msg.MsgID == 4608)
        {
            if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
            {
                var query = Msg.Param[0].As<AITargetQuery>();
                var info = new CrystalAIInfo { Player = query.Info.Player, OtherPlayer = query.Info.IndependentRelation, Kind = (byte)query.Info.Mode };
                UpdateAITarget(ref info, ref query.Data.Priority);
                query.Info.Player = info.Player;
                query.Info.IndependentRelation = info.OtherPlayer;
                query.Info.Mode = (AITargetMode)info.Kind;
                // { Original marks the message only when the resulting priority is negative. }
                if (Msg.Param[0].As<AITargetQuery>().Data.Priority < 0)
                {
                    Msg.Result = 1;
                }
            }
        }
    }

    // ManaCrystals.pas:111
    public override void UpdateMapField(in HexSpriteMapFieldInfo Info)
    {
        if (Info.HN == 0)
        {
            ((AoWMapField)(object)Info.MapField).FFieldFlags = ((AoWMapField)(object)Info.MapField).FFieldFlags | 134217728;
        }
    }

    // ManaCrystals.pas:90
    public void GenerateManaCrystalsAmount()
    {
        if (FSize == 4)
        {
            FSize = (byte)(AoWHSMap.Current.Random(3) + 1);
        }

        switch (FSize)
        {
            case 1:
                FAmount = AoWHSMap.Current.Random(25) + 25;
                break;
            case 2:
                FAmount = AoWHSMap.Current.Random(25) + 50;
                break;
            case 3:
                FAmount = AoWHSMap.Current.Random(50) + 100;
                break;
        }
    }

    // ManaCrystals.pas:100
    public void UpdateAITarget(ref CrystalAIInfo Info, ref int Priority)
    {
        int P;
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
    public void ArmyEntered(ArmyHexSprite ArmyHS)
    {
        LocationTextEventLog Log;
        if ((byte)ArmyHS.FArmy.FPlayer == 0)
        {
            return;
        }

        bool condition = (byte)AoWHSMap.Current.FSeatedPlayer == (byte)ArmyHS.FArmy.FPlayer;
        if (!condition)
        {
            var lhx = GetLhx();
            var yhx = GetYhx();
            var xhx = GetXhx();
            condition = AoWHSMap.Current.Watching(xhx, yhx, lhx, 0);
        }

        if (condition)
        {
            AudioPlayback.PlaySample("SFX\\EVENTS\\MANA.WAV");
        }

        var player = AoWHSMap.Current.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
        player.FMagicControl.SetMana(player.FMagicControl.FMana + FAmount);

        Log = new LocationTextEventLog(null);
        Log.FPlayer = (sbyte)(byte)ArmyHS.FArmy.FPlayer;
        Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(ManaCrystalsFoundRStr), [IntToStr(FAmount)]);
        var lhx2 = GetLhx();
        var yhx2 = GetYhx();
        Log.SetLocation(GetXhx(), yhx2, lhx2);
        AoWHSMap.Current.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer).FEventLogbook.AddEvent(Log);
        Log.Release();
        Dispose();
    }

    private static readonly string ManaCrystalsFoundRStr = "%s mana crystals found";

    // { ManaCrystals of AoWEPACK.dpl: recovered bodies and original instance layouts.
    //   Remaining inherited declaration constraints are documented at local ABI adapters. }
    [StructLayout(LayoutKind.Explicit, Size = 3)]
    public struct CrystalAIInfo
    {
        [FieldOffset(0)]
        public sbyte Player;
        [FieldOffset(1)]
        public sbyte OtherPlayer;
        [FieldOffset(2)]
        public byte Kind;
    }

    public new partial class Resource : ImageLibraryTerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // ManaCrystals.pas:168
        public new const int ClassID = 132031;

        // ManaCrystals.pas:171
        public override int ResourceUserClassID() => 132030;

        // ManaCrystals.pas:165
        public override string ResourceEditName() => AbilityRules.TranslateRStr(ManaCrystalsRStr);

        private static readonly string ManaCrystalsRStr = "Mana Crystals";

    }

    // ManaCrystals.pas:28

    // ManaCrystals.pas:28

}
