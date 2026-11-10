// Translated from ManaCrystals.pas.
using System.Runtime.InteropServices;
using Aow.Game.Ai;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

// FAmount, FSize
public partial class ManaCrystals : ILTerrainMO
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
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001985 && Aow.Game.World.Map.AoWHSMap.Current.FDay == 1)
        {
            GenerateManaCrystalsAmount();
        }
    }

    // ManaCrystals.pas:148
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4354)
        {
            ArmyEntered(Msg.Param[0].As<ArmyHS>());
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
    public override void UpdateMapField(in HSMapFieldInfo Info)
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
            FSize = (byte)(Aow.Game.World.Map.AoWHSMap.Current.Random(3) + 1);
        }

        switch (FSize)
        {
            case 1:
                FAmount = Aow.Game.World.Map.AoWHSMap.Current.Random(25) + 25;
                break;
            case 2:
                FAmount = Aow.Game.World.Map.AoWHSMap.Current.Random(25) + 50;
                break;
            case 3:
                FAmount = Aow.Game.World.Map.AoWHSMap.Current.Random(50) + 100;
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
    public void ArmyEntered(ArmyHS ArmyHS)
    {
        LocationTextEventLog Log;
        if ((byte)ArmyHS.FArmy.FPlayer == 0)
        {
            return;
        }

        bool condition = (byte)Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == (byte)ArmyHS.FArmy.FPlayer;
        if (!condition)
        {
            var lhx = GetLhx();
            var yhx = GetYhx();
            var xhx = GetXhx();
            condition = Aow.Game.World.Map.AoWHSMap.Current.Watching(xhx, yhx, lhx, 0);
        }

        if (condition)
        {
            Aow.Game.Audio.AudioPlayback.PlaySample("SFX\\EVENTS\\MANA.WAV");
        }

        var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
        player.FMagicControl.SetMana(player.FMagicControl.FMana + FAmount);

        Log = new LocationTextEventLog(null);
        Log.FPlayer = (sbyte)(byte)ArmyHS.FArmy.FPlayer;
        Log.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Terrain.ManaCrystals.ManaCrystalsFoundRStr), [IntToStr(FAmount)]);
        var lhx2 = GetLhx();
        var yhx2 = GetYhx();
        Log.SetLocation(GetXhx(), yhx2, lhx2);
        Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer).FEventLogbook.AddEvent(Log);
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

    public new partial class Resource : ILTerrainMOR
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // ManaCrystals.pas:168
        public new const int ClassID = 132031;

        // ManaCrystals.pas:171
        public override int ResourceUserClassID() => 132030;

        // ManaCrystals.pas:165
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.Terrain.ManaCrystals.Resource.ManaCrystalsRStr);

        private static readonly string ManaCrystalsRStr = "Mana Crystals";

    }

    // ManaCrystals.pas:28

    // ManaCrystals.pas:28

}

public partial class CrystalMagic : PlayerMagicControl
{
    public CrystalMagic(ECustomNode owner) : base(owner)
    { }

    public int Mana { get => FMana; set => SetMana(value); }
}

[StructLayout(LayoutKind.Explicit, Size = 166)]
public struct CrystalMapFields
{
    [FieldOffset(0)]
    public FixedArray_byte_165 Reserved;
    [FieldOffset(165)]
    public sbyte SeatedPlayer;
}

[StructLayout(LayoutKind.Explicit, Size = 28)]
public struct CrystalFieldFlags
{
    [FieldOffset(0)]
    public FixedArray_byte_24 Reserved;
    [FieldOffset(24)]
    public int Flags;
}
