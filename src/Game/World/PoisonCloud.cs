// Translated from PoisonC.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World;

// Player, TurnsRemaining, Drifting, Direction, Updated
public partial class PoisonCloud : MultiHexMapObject
{
    public PoisonCloud(ECustomNode owner) : base(owner)
    {
        _drifting = false;
    }

    // PoisonC.pas:64
    public new const int ClassId = 135743;

    public sbyte Player { get; set; }
    public int TurnsRemaining { get; set; }

    private bool _drifting;
    private byte _direction;
    private bool _updated;

    // PoisonC.pas:149
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        TurnsRemaining = persister.Int32("TurnsRemaining", value: TurnsRemaining);
        _drifting = persister.Boolean("Drifting", value: _drifting);
        _direction = persister.Byte("Direction", value: _direction);
    }

    // PoisonC.pas:58

    // PoisonC.pas:275
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986 && message.Payload.Int == Player && !_updated)
        {
            UpdateCloud();
            return;
        }

        if (message.MessageId == 537001987 && message.Payload.Int == Player)
        {
            _updated = false;
        }
    }

    // PoisonC.pas:67
    public override int GetShowPriority() => base.GetShowPriority() + 201326592;

    // PoisonC.pas:89
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (result)
        {
            var hexY = GetHexY();
            var hexX = GetHexX();
            UpdateMapFields(Owner.Owner, hexX, hexY);
        }

        return result;
    }

    // PoisonC.pas:111
    public override void RemoveFromMap()
    {
        MapLevel level;
        level = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(level, _hexX, _hexY);
    }

    // PoisonC.pas:182
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (message.MessageId != 16 || StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            base.MapFieldMessageProc(ref message);
        }

        if (_resource.TerrainList.Terrain[message.HexNumber] >= -1)
        {
            if (message.MessageId == 4418)
            {
                ArmyPlaced(message.Param[0].As<ArmyHexSprite>());
            }
            else
            {
                if (message.MessageId == 4384)
                {
                    if (!message.Param[0].As<ArmyHexSprite>().Army.ImmunityTypes(255).Contains(DamageType.Poison))
                    {
                        message.Result = 1;
                    }
                }
                else
                {
                    if (message.MessageId == 4432)
                    {
                        if ((byte)message.Param[0].Int == 1 && message.Param[1].Int == 0 && message.HexNumber == 0)
                        {
                            Dispose();
                        }
                    }
                }
            }
        }
    }

    // PoisonC.pas:119
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        int result;
        if (_resource.TerrainList.Terrain[info.HexNumber] >= -1)
        {
            if (!CanDoDamage(((MoveControl)info.Sender).Settings.Units))
            {
                result = 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            result = 1;
        }

        return result;
    }

    // PoisonC.pas:198
    public override void Activate()
    {
        base.Activate();
        if (_direction == 0)
        {
            _direction = (byte)(StrategicMap.Current.Random(6) + 1);
        }
    }

    // PoisonC.pas:83
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (_resource.TerrainList.Terrain[info.HexNumber] >= -1)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOverFlag;
        }
    }

    // PoisonC.pas:70
    public override bool CanSelect(int pixelX, int pixelY)
    {
        bool result;
        int hexX = default;
        int hexY = default;
        int hexNumber;
        if (_resource.Engine.HexSpriteMap.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            HexGeometry.PixelToHex(pixelX, pixelY, out hexX, out hexY);
            hexNumber = HexGeometry.DeltaHexToHexNumber(Owner.HexX, Owner.HexY, hexX, hexY);
            result = hexNumber < _resource.TerrainList.Count && _resource.TerrainList.Terrain[hexNumber] >= -1;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // PoisonC.pas:95
    public new void UpdateMapFields(MapLevel level, int hexX, int hexY)
    {
        HexNumberWalkData walk = default;
        int count;
        count = _resource.TerrainList.Count;
        HexNumberWalkData.InitializeHexNumberWalk(out walk, hexX, hexY, 0);
        while (walk.HexNumber != count)
        {
            if (GetTerrain(walk.HexNumber) >= -1 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                level.GetField(walk.X, walk.Y).UpdateMapField();
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // PoisonC.pas:129
    public bool CanDoDamage(UnitList units)
    {
        int I;
        Troop troop;
        bool result = false;
        I = units.GetCount();
        while (!result && I != 0)
        {
            I -= 1;
            troop = units.GetUnit(I);
            result = !troop.GetImmunityTypes().Contains(DamageType.Poison) && !troop.GetAbilityEnabled(1);
        }

        return result;
    }

    // PoisonC.pas:142
    public void ArmyPlaced(ArmyHexSprite armyHexSprite)
    {
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && !armyHexSprite.Owner.GetMagicShieldActive())
        {
            armyHexSprite.Army.IncomingStorm(StormType.Pestilence, 0, armyHexSprite.Army.Player);
        }
    }

    // PoisonC.pas:158
    public void TriggerDamage()
    {
        MapFieldMessage message = default;
        HexNumberWalkData Walk = default;
        MapLevel level;
        level = Owner.Owner;
        var hexY = GetHexY();
        var hexX = GetHexX();
        HexNumberWalkData.InitializeHexNumberWalk(out Walk, hexX, hexY, 0);
        message.MessageId = 4432;
        message.Param[0] = (int)StormType.Pestilence;
        message.Param[2] = Player;
        ref var walk = ref Walk;
        while (_resource.TerrainList.Count > walk.HexNumber)
        {
            if (_resource.TerrainList.GetTerrain(walk.HexNumber) >= -1 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && level.Height > walk.Y)
            {
                message.Param[1] = walk.Radius;
                level.GetField(walk.X, walk.Y).SendMapFieldMessage(ref message);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref Walk);
        }
    }

    // PoisonC.pas:204
    public bool ValidMoveDirection(int direction)
    {
        int hexX = default;
        int hexY = default;
        MapLevel level;
        level = Owner.Owner;
        var yhx = GetHexY();
        HexGeometry.CenterHexNumberToHex(GetHexX(), yhx, direction, ref hexX, ref hexY);
        HexGeometry.CenterHexNumberToHex(hexX, hexY, direction, ref hexX, ref hexY);
        return hexX >= 10 && hexY >= 10 && hexX + 10 <= level.Width && hexY + 10 <= level.Height;
    }

    // PoisonC.pas:216
    public void UpdateCloud()
    {
        int hexX = default;
        int hexY = default;
        ByteList directions;
        bool valid;
        int direction;
        int tries;
        int hexLevel;
        int oldHexX;
        int oldHexY;
        if (TurnsRemaining != 1)
        {
            _updated = true;
            if (TurnsRemaining != 0)
            {
                TurnsRemaining -= 1;
            }

            if (_drifting)
            {
                direction = 0;
                directions = new ByteList();
                valid = false;
                directions.Add(_direction);
                if (_direction != 1)
                {
                    directions.Add((byte)(_direction - 1));
                }
                else
                {
                    directions.Add(6);
                }

                if (_direction != 6)
                {
                    directions.Add((byte)(_direction + 1));
                }
                else
                {
                    directions.Add(1);
                }

                while (directions.Count != 0 && !valid)
                {
                    direction = directions[StrategicMap.Current.Random(directions.Count)];
                    valid = ValidMoveDirection(direction);
                    if (!valid)
                    {
                        directions.Remove((byte)direction);
                    }
                }

                directions?.Dispose();
                if (!valid)
                {
                    tries = 20;
                    while (!valid && tries != 0)
                    {
                        direction = StrategicMap.Current.Random(6) + 1;
                        valid = ValidMoveDirection(direction);
                        tries -= 1;
                    }
                }

                if (valid)
                {
                    oldHexX = GetHexX();
                    oldHexY = GetHexY();
                    _direction = (byte)direction;
                    hexLevel = GetHexLevel();
                    var yhx = GetHexY();
                    HexGeometry.CenterHexNumberToHex(GetHexX(), yhx, direction, ref hexX, ref hexY);
                    HexGeometry.CenterHexNumberToHex(hexX, hexY, direction, ref hexX, ref hexY);
                    Disconnect();
                    UpdateMapFields(StrategicMap.Current.MapContainer.GetMapLevel(hexLevel), oldHexX, oldHexY);
                    _hexX = (sbyte)hexX;
                    _hexY = (sbyte)hexY;
                    Owner = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
                    Connect();
                    UpdateMapFields(StrategicMap.Current.MapContainer.GetMapLevel(hexLevel), hexX, hexY);
                }
            }

            TriggerDamage();
        }
        else
        {
            Dispose();
        }
    }

    public new partial class Resource : MultiHexMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // PoisonC.pas:286
        public new const int ClassId = 135744;

        // PoisonC.pas:292
        public override int ResourceUserClassId() => 135743;

        // PoisonC.pas:289
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_poisonCloudText);

        private static readonly string s_poisonCloudText = "Poison Cloud";

    }

    // PoisonC.pas:27

}
