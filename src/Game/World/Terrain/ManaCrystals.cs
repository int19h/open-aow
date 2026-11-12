// Translated from ManaCrystals.pas.
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

// Amount, Size
public partial class ManaCrystals : ImageLibraryTerrainMapObject
{
    public ManaCrystals(ECustomNode owner) : base(owner)
    {
        _size = 2;
        _amount = 0;
    }

    // ManaCrystals.pas:80
    public new const int ClassId = 132030;

    private int _amount;
    private byte _size;

    // ManaCrystals.pas:83
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref _amount, 5);
        stream.Bytes(Bytes.Of(ref _size, 1), 1, 6);
    }

    // ManaCrystals.pas:73

    // ManaCrystals.pas:118
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001985 && StrategicMap.Current.Day == 1)
        {
            GenerateManaCrystalsAmount();
        }
    }

    // ManaCrystals.pas:148
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 4354)
        {
            ArmyEntered(message.Param[0].As<ArmyHexSprite>());
            return;
        }

        if (message.MessageId == 4608)
        {
            if (_resource.TerrainList.Terrain[message.HexNumber] >= 0)
            {
                var query = message.Param[0].As<AITargetQuery>();
                var info = new CrystalAIInfo { Player = query.Info.Player, OtherPlayer = query.Info.IndependentRelation, Kind = (byte)query.Info.Mode };
                UpdateAITarget(ref info, ref query.Data.Priority);
                query.Info.Player = info.Player;
                query.Info.IndependentRelation = info.OtherPlayer;
                query.Info.Mode = (AITargetMode)info.Kind;
                // { Original marks the message only when the resulting priority is negative. }
                if (message.Param[0].As<AITargetQuery>().Data.Priority < 0)
                {
                    message.Result = 1;
                }
            }
        }
    }

    // ManaCrystals.pas:111
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber == 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | 134217728;
        }
    }

    // ManaCrystals.pas:90
    public void GenerateManaCrystalsAmount()
    {
        if (_size == 4)
        {
            _size = (byte)(StrategicMap.Current.Random(3) + 1);
        }

        switch (_size)
        {
            case 1:
                _amount = StrategicMap.Current.Random(25) + 25;
                break;
            case 2:
                _amount = StrategicMap.Current.Random(25) + 50;
                break;
            case 3:
                _amount = StrategicMap.Current.Random(50) + 100;
                break;
        }
    }

    // ManaCrystals.pas:100
    public void UpdateAITarget(ref CrystalAIInfo info, ref int priority)
    {
        int P;
        switch (info.Kind)
        {
            case 0:
                if (info.Player != 0)
                {
                    P = _amount / 5 + 15;
                    if (priority < P)
                    {
                        priority = P;
                    }
                }

                break;
        }
    }

    // ManaCrystals.pas:125
    public void ArmyEntered(ArmyHexSprite armyHexSprite)
    {
        LocationTextEventLog log;
        if ((byte)armyHexSprite.Army.Player == 0)
        {
            return;
        }

        bool condition = (byte)StrategicMap.Current.SeatedPlayer == (byte)armyHexSprite.Army.Player;
        if (!condition)
        {
            var hexLevel = GetHexLevel();
            var hexY = GetHexY();
            var hexX = GetHexX();
            condition = StrategicMap.Current.Watching(hexX, hexY, hexLevel, 0);
        }

        if (condition)
        {
            AudioPlayback.PlaySample("SFX\\EVENTS\\MANA.WAV");
        }

        var player = StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player);
        player.MagicControl.SetMana(player.MagicControl.Mana + _amount);

        log = new LocationTextEventLog(null) { Holder = CreationHolder() };
        log._player = (sbyte)(byte)armyHexSprite.Army.Player;
        log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), s_manaCrystalsFoundFormat), [IntToStr(_amount)]);
        var hexLevel2 = GetHexLevel();
        var hexY2 = GetHexY();
        log.SetLocation(GetHexX(), hexY2, hexLevel2);
        StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).EventLogbook.AddEvent(log);
        log.Release();
        Dispose();
    }

    private static readonly string s_manaCrystalsFoundFormat = "%s mana crystals found";

    // { ManaCrystals of AoWEPACK.dpl: recovered bodies and original instance layouts.
    //   Remaining inherited declaration constraints are documented at local ABI adapters. }
    public struct CrystalAIInfo
    {
        public sbyte Player;
        public sbyte OtherPlayer;
        public byte Kind;
    }

    public new partial class Resource : ImageLibraryTerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // ManaCrystals.pas:168
        public new const int ClassId = 132031;

        // ManaCrystals.pas:171
        public override int ResourceUserClassId() => 132030;

        // ManaCrystals.pas:165
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_manaCrystalsText);

        private static readonly string s_manaCrystalsText = "Mana Crystals";

    }

    // ManaCrystals.pas:28

    // ManaCrystals.pas:28

}
