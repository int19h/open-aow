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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

// { ot=1 }
// FAmount, FGenerateMode
public partial class Gems : ILTerrainMO
{
    public Gems(ECustomNode owner) : base(owner)
    {
        FGenerateMode = GenerateMode.Normal;
        FAmount = 0;
    }

    // Gems.pas:53
    public new const int ClassID = 132014;

    public int FAmount;
    public GenerateMode FGenerateMode;

    // Gems.pas:56
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FAmount, 5);
        Stream.rwData(Bytes.Of(ref FGenerateMode, 1), 1, 6);
    }

    // Gems.pas:46

    // Gems.pas:93
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001985 && AoWHSMap.Current.FDay == 1)
        {
            GenerateGemsAmount();
        }
    }

    // Gems.pas:119
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4354)
        {
            ArmyEntered(Msg.Param[0].As<ArmyHS>());
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
    public override void UpdateMapField(in HSMapFieldInfo Info)
    {
        if (Info.HN == 0)
        {
            ((AoWMapField)Info.MapField).FFieldFlags = ((AoWMapField)Info.MapField).FFieldFlags | 134217728;
        }
    }

    // Gems.pas:63
    public void GenerateGemsAmount()
    {
        if (FGenerateMode == GenerateMode.Random)
        {
            FGenerateMode = (GenerateMode)(Aow.Game.World.Map.AoWHSMap.Current.Random(3) + 1);
        }

        switch (FGenerateMode)
        {
            case GenerateMode.Low:
                FAmount = Aow.Game.World.Map.AoWHSMap.Current.Random(25) + 25;
                break;
            case GenerateMode.Normal:
                FAmount = Aow.Game.World.Map.AoWHSMap.Current.Random(25) + 50;
                break;
            case GenerateMode.High:
                FAmount = Aow.Game.World.Map.AoWHSMap.Current.Random(50) + 100;
                break;
        }
    }

    // Gems.pas:74
    public void UpdateAITarget(ref AITargetInfo Info, ref int Priority)
    {
        int Value;
        switch (Info.Mode)
        {
            case >= AITargetMode.Attack and <= AITargetMode.Attack:
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
    public void ArmyEntered(ArmyHS ArmyHS)
    {
        LocationTextEventLog Event;
        if (ArmyHS.FArmy.FPlayer != 0)
        {
            bool condition = AoWHSMap.Current.FSeatedPlayer == ArmyHS.FArmy.FPlayer;
            if (!condition)
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                condition = Aow.Game.World.Map.AoWHSMap.Current.Watching(xhx, yhx, lhx, 0);
            }

            if (condition)
            {
                Aow.Game.Audio.AudioPlayback.PlaySample("SFX\\EVENTS\\GOLD.WAV");
            }

            var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer);
            player.SetGems(player.FGems + FAmount);

            Event = new LocationTextEventLog(null);
            Event.FPlayer = ArmyHS.FArmy.FPlayer;
            Event.FText = RtlUnit.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GoldFoundRStr), [IntToStr(FAmount)]);
            var lhx2 = GetLhx();
            var yhx2 = GetYhx();
            Event.SetLocation(GetXhx(), yhx2, lhx2);
            Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(ArmyHS.FArmy.FPlayer).FEventLogbook.AddEvent(Event);
            Event.Release();
            Dispose();
        }
    }

    private static readonly string GoldFoundRStr = "%s gold pieces found";

    public new partial class Resource : ILTerrainMOR
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // Gems.pas:137
        public new const int ClassID = 132015;

        // Gems.pas:140
        public override int ResourceUserClassID() => 132014;

        // Gems.pas:134
        public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GoldRStr);

        private static readonly string GoldRStr = "Gold";

    }

    public enum GenerateMode : byte
    {
        UserDefined = 0, Low = 1, Normal = 2, High = 3, Random = 4
    }

    // Gems.pas:24

    // Gems.pas:24

}
