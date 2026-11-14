// Translated from Gems.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.AI;
using Aow.Game.Audio;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

// { ot=1 }
// Amount, GenerateMode
public partial class Gems : ImageLibraryTerrainMapObject
{
    public Gems(ECustomNode owner) : base(owner)
    {
        _generateMode = GemsGenerateMode.Normal;
        _amount = 0;
    }

    // Gems.pas:53
    public new const int ClassId = 132014;

    private int _amount;
    private GemsGenerateMode _generateMode;

    // Gems.pas:56
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _amount = persister.Int32("Amount", value: _amount);
        _generateMode = persister.Enum("GenerateMode", value: _generateMode);
    }

    // Gems.pas:46

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
            UpdateAITarget(ref message.Param[0].As<AITargetQuery>().Info, ref message.Param[0].As<AITargetQuery>().Data.Priority);
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
        }
    }

    // Gems.pas:74
    public void UpdateAITarget(ref AITargetInfo info, ref int priority)
    {
        int value;
        switch (info.Mode)
        {
            case >= AITargetMode.Attack and <= AITargetMode.Attack:
                if (info.Player != 0)
                {
                    value = _amount / 5 + 15;
                    if (value > priority)
                    {
                        priority = value;
                    }
                }

                break;
        }
    }

    // Gems.pas:99
    public void ArmyEntered(ArmyHexSprite armyHexSprite)
    {
        LocationTextEventLog eventValue;
        if (armyHexSprite.Army.Player != 0)
        {
            bool condition = StrategicMap.Current.SeatedPlayer == armyHexSprite.Army.Player;
            if (!condition)
            {
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                condition = StrategicMap.Current.Watching(hexX, hexY, hexLevel, 0);
            }

            if (condition)
            {
                AudioPlayback.PlaySample("SFX\\EVENTS\\GOLD.WAV");
            }

            var player = StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player);
            player.SetGems(player.Gems + _amount);

            eventValue = new LocationTextEventLog(null) { Holder = CreationHolder() };
            eventValue._player = armyHexSprite.Army.Player;
            eventValue.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), s_goldFoundFormat), [IntToStr(_amount)]);
            var hexLevel2 = GetHexLevel();
            var hexY2 = GetHexY();
            eventValue.SetLocation(GetHexX(), hexY2, hexLevel2);
            StrategicMap.Current.Players.GetPlayers(armyHexSprite.Army.Player).EventLogbook.AddEvent(eventValue);
            eventValue.Release();
            Dispose();
        }
    }

    private static readonly string s_goldFoundFormat = "%s gold pieces found";

    public new partial class Resource : ImageLibraryTerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // Gems.pas:137
        public new const int ClassId = 132015;

        // Gems.pas:140
        public override int ResourceUserClassId() => 132014;

        // Gems.pas:134
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_goldText);

        private static readonly string s_goldText = "Gold";

    }

    // Gems.pas:24

    // Gems.pas:24

}

public enum GemsGenerateMode : byte
{
    UserDefined = 0, Low = 1, Normal = 2, High = 3, Random = 4
}
