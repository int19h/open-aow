#nullable disable
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
public class PoisonCloud(CustomNode owner) : MultiHexMapObject(owner)
{

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
            sbyte hexY = HexY;
            sbyte hexX = HexX;
            UpdateMapFields(Owner.Owner, hexX, hexY);
        }

        return result;
    }

    // PoisonC.pas:111
    public override void RemoveFromMap()
    {
        MapLevel level = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(level, StoredHexX, StoredHexY);
    }

    // PoisonC.pas:182
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (message.MessageId != 16 || StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            base.MapFieldMessageProc(ref message);
        }

        if (base.Resource.TerrainList.Terrain[message.HexNumber] >= -1)
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
    public override int CanMoveOver(in HexSpriteSenderInfo info) => base.Resource.TerrainList.Terrain[info.HexNumber] >= -1 ? !CanDoDamage(((MoveControl)info.Sender).Settings.Units) ? 1 : 0 : 1;

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
        if (base.Resource.TerrainList.Terrain[info.HexNumber] >= -1)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOverFlag;
        }
    }

    // PoisonC.pas:70
    public override bool CanSelect(int pixelX, int pixelY)
    {
        if (base.Resource.Engine.HexSpriteMap.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            HexGeometry.PixelToHex(pixelX, pixelY, out int hexX, out int hexY);
            int hexNumber = HexGeometry.DeltaHexToHexNumber(Owner.HexX, Owner.HexY, hexX, hexY);
            return hexNumber < base.Resource.TerrainList.Count && base.Resource.TerrainList.Terrain[hexNumber] >= -1;
        }
        else
        {
            return false;
        }
    }

    // PoisonC.pas:95
    public new void UpdateMapFields(MapLevel level, int hexX, int hexY)
    {
        int count = base.Resource.TerrainList.Count;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, 0);
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
    public bool CanDoDamage(UnitUserList units)
    {
        bool result = false;
        int i = units.GetCount();
        while (!result && i != 0)
        {
            i -= 1;
            Troop troop = units.GetUnit(i);
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
        MapLevel level = Owner.Owner;
        sbyte hexY = HexY;
        sbyte hexX = HexX;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData hexNumberWalkData, hexX, hexY, 0);
        message.MessageId = 4432;
        MapFieldMessageParameters param = message.Param;
        param[0] = (int)StormType.Pestilence;
        message.Param = param;
        MapFieldMessageParameters param2 = message.Param;
        param2[2] = Player;
        message.Param = param2;
        ref HexNumberWalkData walk = ref hexNumberWalkData;
        while (base.Resource.TerrainList.Count > walk.HexNumber)
        {
            if (base.Resource.TerrainList.GetTerrain(walk.HexNumber) >= -1 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && level.Height > walk.Y)
            {
                MapFieldMessageParameters param3 = message.Param;
                param3[1] = walk.Radius;
                message.Param = param3;
                level.GetField(walk.X, walk.Y).SendMapFieldMessage(ref message);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref hexNumberWalkData);
        }
    }

    // PoisonC.pas:204
    public bool ValidMoveDirection(int direction)
    {
        int hexX = default;
        int hexY = default;
        MapLevel level = Owner.Owner;
        sbyte yhx = HexY;
        HexGeometry.CenterHexNumberToHex(HexX, yhx, direction, ref hexX, ref hexY);
        HexGeometry.CenterHexNumberToHex(hexX, hexY, direction, ref hexX, ref hexY);
        return hexX >= 10 && hexY >= 10 && hexX + 10 <= level.Width && hexY + 10 <= level.Height;
    }

    // PoisonC.pas:216
    public void UpdateCloud()
    {
        if (TurnsRemaining != 1)
        {
            _updated = true;
            if (TurnsRemaining != 0)
            {
                TurnsRemaining -= 1;
            }

            if (_drifting)
            {
                int direction = 0;
                ByteList directions = new();
                bool valid = false;
                _ = directions.Add(_direction);
                _ = _direction != 1 ? directions.Add((byte)(_direction - 1)) : directions.Add(6);

                _ = _direction != 6 ? directions.Add((byte)(_direction + 1)) : directions.Add(1);

                while (directions.Count != 0 && !valid)
                {
                    direction = directions[StrategicMap.Current.Random(directions.Count)];
                    valid = ValidMoveDirection(direction);
                    if (!valid)
                    {
                        _ = directions.Remove((byte)direction);
                    }
                }

                directions?.Dispose();
                if (!valid)
                {
                    int tries = 20;
                    while (!valid && tries != 0)
                    {
                        direction = StrategicMap.Current.Random(6) + 1;
                        valid = ValidMoveDirection(direction);
                        tries -= 1;
                    }
                }

                if (valid)
                {
                    int oldHexX = HexX;
                    int oldHexY = HexY;
                    _direction = (byte)direction;
                    int hexLevel = HexLevel;
                    sbyte yhx = HexY;
                    int hexX = default;
                    int hexY = default;
                    HexGeometry.CenterHexNumberToHex(HexX, yhx, direction, ref hexX, ref hexY);
                    HexGeometry.CenterHexNumberToHex(hexX, hexY, direction, ref hexX, ref hexY);
                    Disconnect();
                    UpdateMapFields(StrategicMap.Current.MapContainer.GetMapLevel(hexLevel), oldHexX, oldHexY);
                    StoredHexX = (sbyte)hexX;
                    StoredHexY = (sbyte)hexY;
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

    public new class Resource(CustomNode owner) : MultiHexMapObjectResource(owner)
    {

        // PoisonC.pas:286
        public new const int ClassId = 135744;

        // PoisonC.pas:292
        public override int ResourceUserClassId() => 135743;

        // PoisonC.pas:289
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), s_poisonCloudText);

        private static readonly string s_poisonCloudText = "Poison Cloud";

    }
}
