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

namespace Aow.Units.ManaCrystals;

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
            if (FResource.TerrainList.Terrain[Msg.HN] >= 0)
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
            FSize = (byte)(AoWHSMap.Random(3) + 1);
        }

        switch (FSize)
        {
            case 1:
                FAmount = AoWHSMap.Random(25) + 25;
                break;
            case 2:
                FAmount = AoWHSMap.Random(25) + 50;
                break;
            case 3:
                FAmount = AoWHSMap.Random(50) + 100;
                break;
        }
    }

    // ManaCrystals.pas:100
    public void UpdateAITarget(ref TCrystalAIInfo Info, ref int Priority)
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
    public void ArmyEntered(TArmyHS ArmyHS)
    {
        TLocationTextEventLog Log;
        if ((byte)ArmyHS.FArmy.FPlayer == 0)
        {
            return;
        }

        bool condition = (byte)AoWHSMap.FSeatedPlayer == (byte)ArmyHS.FArmy.FPlayer;
        if (!condition)
        {
            var lhx = GetLhx();
            var yhx = GetYhx();
            var xhx = GetXhx();
            condition = AoWHSMap.Watching(xhx, yhx, lhx, 0);
        }

        if (condition)
        {
            PlaySample("SFX\\EVENTS\\MANA.WAV");
        }

        var player = AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
        player.FMagicControl.SetMana(player.FMagicControl.FMana + FAmount);

        Log = new TLocationTextEventLog(null);
        Log.FPlayer = (sbyte)(byte)ArmyHS.FArmy.FPlayer;
        Log.FText = Rtl.Rtl.Format(TranslateRStr(ManaCrystalsFoundRStr), [IntToStr(FAmount)]);
        var lhx2 = GetLhx();
        var yhx2 = GetYhx();
        Log.SetLocation(GetXhx(), yhx2, lhx2);
        AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer).FEventLogbook.AddEvent(Log);
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
    public override int ResourceUserClassID() => 132030;

    // ManaCrystals.pas:165
    public override string ResourceEditName() => TranslateRStr(ManaCrystalsRStr);

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
