// Translated from Gems.pas.
using Aow.Game.Ai;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Game.Audio.SoundUnit;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

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
            UpdateAITarget(ref Msg.Param[0].As<AITargetQuery>().Info, ref Msg.Param[0].As<AITargetQuery>().Data.Priority);
            if (Msg.Param[0].As<AITargetQuery>().Data.Priority < 0)
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
            FGenerateMode = (TGemsGenerateMode)(AoWHSMap.Random(3) + 1);
        }

        switch (FGenerateMode)
        {
            case TGemsGenerateMode.gmLow:
                FAmount = AoWHSMap.Random(25) + 25;
                break;
            case TGemsGenerateMode.gmNormal:
                FAmount = AoWHSMap.Random(25) + 50;
                break;
            case TGemsGenerateMode.gmHigh:
                FAmount = AoWHSMap.Random(50) + 100;
                break;
        }
    }

    // Gems.pas:74
    public void UpdateAITarget(ref TAITargetInfo Info, ref int Priority)
    {
        int Value;
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
        TLocationTextEventLog Event;
        if (ArmyHS.FArmy.FPlayer != 0)
        {
            bool condition = AoWHSMap.FSeatedPlayer == ArmyHS.FArmy.FPlayer;
            if (!condition)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                condition = AoWHSMap.Watching(xhx, yhx, lhx, 0);
            }

            if (condition)
            {
                PlaySample("SFX\\EVENTS\\GOLD.WAV");
            }

            var player = AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
            player.SetGems(player.FGems + FAmount);

            Event = new TLocationTextEventLog(null);
            Event.FPlayer = ArmyHS.FArmy.FPlayer;
            Event.FText = RtlUnit.Format(TranslateRStr(GoldFoundRStr), [IntToStr(FAmount)]);
            var lhx2 = GetLhx();
            var yhx2 = GetYhx();
            Event.SetLocation(GetXhx(), yhx2, lhx2);
            AoWHSMap.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer).FEventLogbook.AddEvent(Event);
            Event.Release();
            Dispose();
        }
    }

    // Gems.pas:24

}

public partial class TGemsResource : TILTerrainMOR
{
    public TGemsResource(TECustomNode owner) : base(owner)
    { }

    // Gems.pas:137
    public new const int ClassID = 132015;

    // Gems.pas:140
    public override int ResourceUserClassID() => 132014;

    // Gems.pas:134
    public override string ResourceEditName() => TranslateRStr(GoldRStr);

}

public enum TGemsGenerateMode : byte
{
    gmUserDefined = 0,
    gmLow = 1,
    gmNormal = 2,
    gmHigh = 3,
    gmRandom = 4
}

public static partial class GemsUnit
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
