#nullable disable
// Translated from ManaCrystals.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiFormat;

namespace Aow.Game.World.Terrain;

// Amount, Size
public partial class ManaCrystals(CustomNode owner) : ImageLibraryTerrainMapObject(owner)
{

    // ManaCrystals.pas:80
    public new const int ClassId = 132030;

    private int _amount;
    private byte _size = 2;

    // ManaCrystals.pas:83
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _amount = persister.Int32("Amount", value: _amount);
        _size = persister.RawByte("Size", value: _size);
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
            if (base.Resource.TerrainList.Terrain[message.HexNumber] >= 0)
            {
                AITargetQuery query = message.Param[0].As<AITargetQuery>();
                CrystalAIInfo info = new() { Player = query.Info.Player, OtherPlayer = query.Info.IndependentRelation, Kind = (byte)query.Info.Mode };
                AITargetData data = query.Data;
                int priority = data.Priority;
                UpdateAITarget(info, ref priority);
                data.Priority = priority;
                query.Data = data;
                AITargetInfo info2 = query.Info;
                info2.Player = info.Player;
                query.Info = info2;
                AITargetInfo info3 = query.Info;
                info3.IndependentRelation = info.OtherPlayer;
                query.Info = info3;
                AITargetInfo info4 = query.Info;
                info4.Mode = (AITargetMode)info.Kind;
                query.Info = info4;
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
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.AITargetFlag;
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
            default:
                break;
        }
    }

    // ManaCrystals.pas:100
    public void UpdateAITarget(in CrystalAIInfo info, ref int priority)
    {
        switch (info.Kind)
        {
            case 0:
                if (info.Player != 0)
                {
                    int p = (_amount / 5) + 15;
                    if (priority < p)
                    {
                        priority = p;
                    }
                }

                break;
            default:
                break;
        }
    }

    // ManaCrystals.pas:125
    public void ArmyEntered(ArmyHexSprite armyHexSprite)
    {
        if ((byte)armyHexSprite.Army.Player == 0)
        {
            return;
        }

        bool condition = (byte)StrategicMap.Current.SeatedPlayer == (byte)armyHexSprite.Army.Player;
        if (!condition)
        {
            sbyte hexLevel = GetHexLevel();
            sbyte hexY = GetHexY();
            sbyte hexX = GetHexX();
            condition = StrategicMap.Current.Watching(hexX, hexY, hexLevel, 0);
        }

        if (condition)
        {
            _ = AudioPlayback.PlaySample("SFX\\EVENTS\\MANA.WAV");
        }

        Player player = StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player);
        player.MagicControl.SetMana(player.MagicControl.Mana + _amount);

        LocationTextEventLog log = new(null)
        {
            Holder = CreationHolder(),
            Player = (sbyte)(byte)armyHexSprite.Army.Player,
            Text = Format(AbilityRules.TranslateResourceString(GetEngine(), s_manaCrystalsFoundFormat), [IntToStr(_amount)])
        };
        log.SetLocation(level: GetHexLevel(), y: GetHexY(), x: GetHexX());
        StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).EventLogbook.AddEvent(log);
        _ = log.Release();
        Dispose();
    }

    private static readonly string s_manaCrystalsFoundFormat = "%s mana crystals found";

    // { ManaCrystals of AoWEPACK.dpl: recovered bodies and original instance layouts.
    //   Remaining inherited declaration constraints are documented at local ABI adapters. }
    public readonly struct CrystalAIInfo
    {
        public sbyte Player { get; init; }
        public sbyte OtherPlayer { get; init; }
        public byte Kind { get; init; }
    }

    public new partial class Resource(CustomNode owner) : ImageLibraryTerrainMapObjectResource(owner)
    {

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
