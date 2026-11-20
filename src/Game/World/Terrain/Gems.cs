#nullable disable
// Translated from Gems.pas.
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

// { ot=1 }
// Amount, GenerateMode
public class Gems(CustomNode owner) : ImageLibraryTerrainMapObject(owner)
{

    // Gems.pas:53
    public new const int ClassId = 132014;

    private int _amount;
    private GemsGenerateMode _generateMode = GemsGenerateMode.Normal;

    // Gems.pas:56
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _amount = persister.Int32("Amount", value: _amount);
        _generateMode = persister.Enum("GenerateMode", value: _generateMode);
    }

    // Gems.pas:93
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001985 && StrategicMap.Current.Day == 1)
        {
            GenerateGemsAmount();
        }
    }

    // Gems.pas:119
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 4354)
        {
            ArmyEntered(message.Param[0].As<ArmyHexSprite>());
            return;
        }

        if (message.MessageId == 4608 && message.HexNumber == 0)
        {
            AITargetInfo info = message.Param[0].As<AITargetQuery>().Info;
            AITargetData data = message.Param[0].As<AITargetQuery>().Data;
            int priority = data.Priority;
            UpdateAITarget(in info, ref priority);
            message.Param[0].As<AITargetQuery>().Info = info;
            data.Priority = priority;
            message.Param[0].As<AITargetQuery>().Data = data;
            if (message.Param[0].As<AITargetQuery>().Data.Priority < 0)
            {
                message.Result = 1;
            }
        }
    }

    // Gems.pas:87
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber == 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.AITargetFlag;
        }
    }

    // Gems.pas:63
    public void GenerateGemsAmount()
    {
        if (_generateMode == GemsGenerateMode.Random)
        {
            _generateMode = (GemsGenerateMode)(StrategicMap.Current.Random(3) + 1);
        }

        switch (_generateMode)
        {
            case GemsGenerateMode.Low:
                _amount = StrategicMap.Current.Random(25) + 25;
                break;
            case GemsGenerateMode.Normal:
                _amount = StrategicMap.Current.Random(25) + 50;
                break;
            case GemsGenerateMode.High:
                _amount = StrategicMap.Current.Random(50) + 100;
                break;
            case GemsGenerateMode.UserDefined:
            case GemsGenerateMode.Random:
            default:
                break;
        }
    }

    // Gems.pas:74
    public void UpdateAITarget(ref readonly AITargetInfo info, ref int priority)
    {
        switch (info.Mode)
        {
            case AITargetMode.Attack:
                if (info.Player != 0)
                {
                    int value = (_amount / 5) + 15;
                    if (value > priority)
                    {
                        priority = value;
                    }
                }

                break;
            case AITargetMode.Raid:
            case AITargetMode.Recruit:
            case AITargetMode.Produce:
            case AITargetMode.Hunt:
            case AITargetMode.Fortify:
            case AITargetMode.Defend:
            case AITargetMode.GuardArea:
            case AITargetMode.AttackArmies:
            default:
                break;
        }
    }

    // Gems.pas:99
    public void ArmyEntered(ArmyHexSprite armyHexSprite)
    {
        if (armyHexSprite.Army.Player != 0)
        {
            bool condition = StrategicMap.Current.SeatedPlayer == armyHexSprite.Army.Player;
            if (!condition)
            {
                sbyte hexLevel = HexLevel;
                sbyte hexY = HexY;
                sbyte hexX = HexX;
                condition = StrategicMap.Current.Watching(hexX, hexY, L: hexLevel, radius: 0);
            }

            if (condition)
            {
                _ = AudioPlayback.PlaySample("SFX\\EVENTS\\GOLD.WAV");
            }

            Player player = StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player);
            player.SetGems(player.Gems + _amount);

            LocationTextEventLog eventValue = new(null)
            {
                Holder = CreationHolder(),
                Player = armyHexSprite.Army.Player,
                Text = Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_goldFoundFormat), args: [IntToStr(_amount)])
            };
            eventValue.SetLocation(level: HexLevel, y: HexY, x: HexX);
            StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).EventLogbook.AddEvent(eventValue);
            _ = eventValue.Release();
            Dispose();
        }
    }

    private static readonly string s_goldFoundFormat = "%s gold pieces found";

    public new class Resource(CustomNode owner) : ImageLibraryTerrainMapObjectResource(owner)
    {

        // Gems.pas:137
        public new const int ClassId = 132015;

        // Gems.pas:140
        public override int ResourceUserClassId() => 132014;

        // Gems.pas:134
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_goldText);

        private static readonly string s_goldText = "Gold";

    }
}

public enum GemsGenerateMode : byte
{
    UserDefined = 0, Low = 1, Normal = 2, High = 3, Random = 4
}
