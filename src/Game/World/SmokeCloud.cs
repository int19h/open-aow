// Translated from SmokeCloud.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Draw;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World;

// Player, TurnsRemaining, Drifting, Direction, Updated, VisibleForSeatedPlayer,
// Enchantment
public partial class SmokeCloud : MultiHexMapObject
{
    public SmokeCloud(ECustomNode owner) : base(owner)
    {
        Enchantment = new SmokeCloudEnchantment(null);
        Enchantment.Cloud = this;
        _visibleForSeatedPlayer = false;
        Drifting = false;
    }

    // SmokeCloud.pas:187
    public new const int ClassId = 135746;

    public sbyte Player;
    public int TurnsRemaining;
    public bool Drifting;
    private byte _direction;
    private bool _updated;
    private bool _visibleForSeatedPlayer;

    public SmokeCloudEnchantment Enchantment { get; }

    // SmokeCloud.pas:181
    public override void Dispose()
    {
        base.Dispose();
        Enchantment.Release();
    }

    // SmokeCloud.pas:206
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Player), 15);
        stream.Int32(ref TurnsRemaining, 16);
        stream.Boolean(ref Drifting, 17);
        stream.Byte(ref _direction, 18);
        stream.Object(Enchantment, 19);
    }

    // SmokeCloud.pas:172

    // SmokeCloud.pas:373
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
        else
        {
            if (message.MessageId == 537001993)
            {
                UpdateVisibleForSeatedPlayer();
            }
        }
    }

    // SmokeCloud.pas:190
    public override int GetShowPriority() => base.GetShowPriority() + 201326592;

    // SmokeCloud.pas:300
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (_visibleForSeatedPlayer || StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            base.Show(pixelXSurface, pixelYSurface, lockedSurface);
        }
    }

    // SmokeCloud.pas:266
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (result)
        {
            UpdateMapFields(Owner.Owner, _hexX, _hexY);
        }

        return result;
    }

    // SmokeCloud.pas:272
    public override void RemoveFromMap()
    {
        MapLevel level;
        level = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(level, _hexX, _hexY);
    }

    // SmokeCloud.pas:245
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (message.MessageId != 16 || StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            base.MapFieldMessageProc(ref message);
        }

        if (_resource.TerrainList.Terrain[message.HexNumber] >= -1)
        {
            if (message.MessageId == 4392 && Status.Contains(MapObjectStatusElement.Activated))
            {
                message.Param[0] = 1;
            }
            else
            {
                if (message.MessageId == 4365)
                {
                    UpdateVisibleForSeatedPlayer();
                }
            }
        }
    }

    // SmokeCloud.pas:281
    public override void Activate()
    {
        base.Activate();
        if (_direction == 0)
        {
            _direction = (byte)(StrategicMap.Current.Random(6) + 1);
        }

        var smokeCloudEnchantment = Enchantment;
        if (smokeCloudEnchantment.PlayerIndex > 0 && !smokeCloudEnchantment.EnchantmentStatus.Contains(GlobalEnchantmentStatusElement.Activated))
        {
            smokeCloudEnchantment.Activate(smokeCloudEnchantment.PlayerIndex);
        }

        var hexY = HexY;
        UpdateConcealment(HexX, hexY, HexLevel);
        UpdateVisibleForSeatedPlayer();
    }

    // SmokeCloud.pas:292
    public override void Deactivate()
    {
        base.Deactivate();
        if (Enchantment.PlayerIndex > 0)
        {
            Enchantment.Deactivate((byte)Enchantment.PlayerIndex);
        }

        var hexLevel = GetHexLevel();
        var hexY = GetHexY();
        UpdateConcealment(GetHexX(), hexY, hexLevel);
    }

    // SmokeCloud.pas:238
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber > 18 && info.HexNumber < 37)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | 16777216;
        }
    }

    // SmokeCloud.pas:193
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

    // SmokeCloud.pas:216
    public void UpdateConcealment(sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        HexNumberWalkData walk = default;
        MapLevel level;
        MapField mapField;
        ArmyHexSprite armyHexSprite;
        level = StrategicMap.Current.MapContainer.MapLevels[hexLevel];
        HexNumberWalkData.InitializeHexNumberWalk(out walk, hexX, hexY, 0);
        while (_resource.TerrainList.Count > walk.HexNumber)
        {
            if (_resource.TerrainList.Terrain[walk.HexNumber] >= -1 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && level.Height > walk.Y)
            {
                mapField = level.GetField(walk.X, walk.Y);
                armyHexSprite = mapField.GetArmyHexSprite();
                if (armyHexSprite != null)
                {
                    armyHexSprite.Army.Update();
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // SmokeCloud.pas:253
    public void UpdateVisibleForSeatedPlayer()
    {
        bool visible;
        var hexLevel = GetHexLevel();
        var hexY = GetHexY();
        var hexX = GetHexX();
        visible = StrategicMap.Current.Visible(hexX, hexY, hexLevel, 3) || Enchantment.PlayerIndex == StrategicMap.Current.SeatedPlayer;
        if (_visibleForSeatedPlayer ^ visible)
        {
            _visibleForSeatedPlayer = visible;
            StrategicMap.Current.GlobalMagicControl.TriggerChangedEvent();
        }
    }

    // SmokeCloud.pas:306
    public bool ValidMoveDirection(int direction)
    {
        int hexX = default;
        int hexY = default;
        MapLevel level;
        level = Owner.Owner;
        var yhx = GetHexY();
        HexGeometry.CenterHexNumberToHex(GetHexX(), yhx, direction, ref hexX, ref hexY);
        HexGeometry.CenterHexNumberToHex(hexX, hexY, direction, ref hexX, ref hexY);
        return hexX >= 10 && hexY >= 10 && level.Width >= hexX + 10 && level.Height >= hexY + 10;
    }

    // SmokeCloud.pas:317
    public void UpdateCloud()
    {
        int hexX = default;
        int hexY = default;
        ByteList directions;
        bool valid;
        int direction;
        int tries;
        int hexLevel;
        if (TurnsRemaining != 1)
        {
            _updated = true;
            if (TurnsRemaining != 0)
            {
                TurnsRemaining -= 1;
            }

            if (Drifting)
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
                    _direction = (byte)direction;
                    hexLevel = GetHexLevel();
                    var yhx = GetHexY();
                    HexGeometry.CenterHexNumberToHex(GetHexX(), yhx, direction, ref hexX, ref hexY);
                    HexGeometry.CenterHexNumberToHex(hexX, hexY, direction, ref hexX, ref hexY);
                    Disconnect();
                    UpdateConcealment(_hexX, _hexY, (sbyte)hexLevel);
                    _hexX = (sbyte)hexX;
                    _hexY = (sbyte)hexY;
                    Owner = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
                    Connect();
                    UpdateConcealment(_hexX, _hexY, (sbyte)hexLevel);
                }
            }
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

        // SmokeCloud.pas:385
        public new const int ClassId = 135747;

        // SmokeCloud.pas:391
        public override int ResourceUserClassId() => 135746;

        // SmokeCloud.pas:388
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GameText.SmokeCloudText);

    }

    // SmokeCloud.pas:43

}
