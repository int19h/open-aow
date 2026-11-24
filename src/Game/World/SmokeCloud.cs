#nullable disable warnings
// Translated from SmokeCloud.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World;

// Player, TurnsRemaining, Drifting, Direction, Updated, VisibleForSeatedPlayer,
// Enchantment
public class SmokeCloud : MultiHexMapObject
{
    public SmokeCloud(CustomNode owner) : base(owner)
    {
        Enchantment = new SmokeCloudEnchantment(null)
        {
            Holder = CreationHolder(),
            Cloud = this
        };
        _visibleForSeatedPlayer = false;
        Drifting = false;
    }

    // SmokeCloud.pas:187
    public new const int ClassId = 135_746;

    public PlayerId? Player { get; set; } = PlayerId.Independent;
    public int TurnsRemaining { get; set; }
    public bool Drifting { get; set; }

    private int _direction;
    private bool _updated;
    private bool _visibleForSeatedPlayer;

    public SmokeCloudEnchantment Enchantment { get; }

    // SmokeCloud.pas:181
    public override void Dispose()
    {
        base.Dispose();
        _ = Enchantment.Release();
    }

    // SmokeCloud.pas:206
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Player = persister.OptionalPlayer(name: "Player", value: Player, width: IdWidth.UnsignedByte, fallback: 0);
        TurnsRemaining = persister.Int32("TurnsRemaining", value: TurnsRemaining);
        Drifting = persister.Boolean("Drifting", value: Drifting);
        _direction = persister.Byte("Direction", value: _direction.TruncateToByte());
        persister.Object("Enchantment", objectValue: Enchantment);
    }

    // SmokeCloud.pas:373
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986 && message.Payload.Int == PlayerIdExtensions.ToOriginal(Player) && !_updated)
        {
            UpdateCloud();
            return;
        }

        if (message.MessageId == 537001987 && message.Payload.Int == PlayerIdExtensions.ToOriginal(Player))
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
    public override int GetShowPriority() => base.GetShowPriority() + 201_326_592;

    // SmokeCloud.pas:300
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (_visibleForSeatedPlayer || StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
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
            UpdateMapFields(Owner.Owner, StoredHexX, StoredHexY);
        }

        return result;
    }

    // SmokeCloud.pas:272
    public override void RemoveFromMap()
    {
        MapLevel level = Owner.Owner;
        base.RemoveFromMap();
        UpdateMapFields(level, StoredHexX, StoredHexY);
    }

    // SmokeCloud.pas:245
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (message.MessageId != 16 || StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            base.MapFieldMessageProc(ref message);
        }

        if (base.Resource.TerrainList.Terrain[message.HexNumber] >= -1)
        {
            if (message.MessageId == 4392 && Status.Contains(MapObjectStatuses.Activated))
            {
                MapFieldMessageParameters param = message.Param;
                param[0] = 1;
                message.Param = param;
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
            _direction = (StrategicMap.Current.Random(6) + 1).TruncateToByte();
        }

        SmokeCloudEnchantment smokeCloudEnchantment = Enchantment;
        if (PlayerIdExtensions.ToOriginal(smokeCloudEnchantment.PlayerIndex) > 0 && !smokeCloudEnchantment.EnchantmentStatus.Contains(GlobalEnchantmentStatuses.Activated))
        {
            _ = smokeCloudEnchantment.Activate(smokeCloudEnchantment.PlayerIndex);
        }
        UpdateConcealment(hexY: HexY, hexX: HexX, hexLevel: HexLevel);
        UpdateVisibleForSeatedPlayer();
    }

    // SmokeCloud.pas:292
    public override void Deactivate()
    {
        base.Deactivate();
        if (PlayerIdExtensions.ToOriginal(Enchantment.PlayerIndex) > 0)
        {
            _ = Enchantment.Deactivate(PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(Enchantment.PlayerIndex)));
        }
        UpdateConcealment(hexLevel: HexLevel, hexY: HexY, hexX: HexX);
    }

    // SmokeCloud.pas:238
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (info.HexNumber is > 18 and < 37)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.PlayerChangeListenerFlag;
        }
    }

    // SmokeCloud.pas:193
    public override bool CanSelect(int pixelX, int pixelY)
    {
        if (base.Resource.Engine.HexSpriteMap.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            HexGeometry.PixelToHex(pixelX, vertical: pixelY, out int hexX, out int hexY);
            int hexNumber = HexGeometry.DeltaHexToHexNumber(Owner.HexX, Owner.HexY, hexX, hexY);
            return hexNumber < base.Resource.TerrainList.Count && base.Resource.TerrainList.Terrain[hexNumber] >= -1;
        }
        else
        {
            return false;
        }
    }

    // SmokeCloud.pas:216
    public void UpdateConcealment(int hexX, int hexY, int hexLevel)
    {
        MapLevel level = StrategicMap.Current.MapContainer.MapLevels[hexLevel];
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, radius: 0);
        while (base.Resource.TerrainList.Count > walk.HexNumber)
        {
            if (base.Resource.TerrainList.Terrain[walk.HexNumber] >= -1 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && level.Height > walk.Y)
            {
                MapField mapField = level.GetField(walk.X, walk.Y);
                ArmyHexSprite armyHexSprite = mapField.GetArmyHexSprite();
                armyHexSprite?.Army.Update();
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
    }

    // SmokeCloud.pas:253
    public void UpdateVisibleForSeatedPlayer()
    {
        int hexLevel = HexLevel;
        int hexY = HexY;
        int hexX = HexX;
        bool visible = StrategicMap.Current.Visible(hexX, hexY, L: hexLevel, radius: 3) || Enchantment.PlayerIndex == StrategicMap.Current.SeatedPlayer;
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
        MapLevel level = Owner.Owner;
        int yhx = HexY;
        HexGeometry.CenterHexNumberToHex(HexX, yhx, hexNumber: direction, ref hexX, ref hexY);
        HexGeometry.CenterHexNumberToHex(hexX, hexY, hexNumber: direction, ref hexX, ref hexY);
        return hexX >= 10 && hexY >= 10 && level.Width >= hexX + 10 && level.Height >= hexY + 10;
    }

    // SmokeCloud.pas:317
    public void UpdateCloud()
    {
        if (TurnsRemaining != 1)
        {
            _updated = true;
            if (TurnsRemaining != 0)
            {
                TurnsRemaining -= 1;
            }

            if (Drifting)
            {
                int direction = 0;
                ByteList directions = [];
                bool valid = false;
                _ = directions.Add(_direction.TruncateToByte());
                _ = _direction != 1 ? directions.Add((_direction - 1).TruncateToByte()) : directions.Add(6);

                _ = _direction != 6 ? directions.Add((_direction + 1).TruncateToByte()) : directions.Add(1);

                while (directions.Count != 0 && !valid)
                {
                    direction = directions[StrategicMap.Current.Random(directions.Count)];
                    valid = ValidMoveDirection(direction);
                    if (!valid)
                    {
                        _ = directions.Remove(direction.TruncateToByte());
                    }
                }
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
                    _direction = direction.TruncateToByte();
                    int hexLevel = HexLevel;
                    int yhx = HexY;
                    int hexX = default;
                    int hexY = default;
                    HexGeometry.CenterHexNumberToHex(HexX, yhx, hexNumber: direction, ref hexX, ref hexY);
                    HexGeometry.CenterHexNumberToHex(hexX, hexY, hexNumber: direction, ref hexX, ref hexY);
                    Disconnect();
                    UpdateConcealment(StoredHexX, StoredHexY, hexLevel.TruncateToSByte());
                    StoredHexX = hexX.TruncateToSByte();
                    StoredHexY = hexY.TruncateToSByte();
                    Owner = StrategicMap.Current.MapContainer.GetField(hexX, hexY, hexLevel);
                    Connect();
                    UpdateConcealment(StoredHexX, StoredHexY, hexLevel.TruncateToSByte());
                }
            }
        }
        else
        {
            Dispose();
        }
    }

    public new class Resource(CustomNode owner) : MultiHexMapObjectResource(owner)
    {

        // SmokeCloud.pas:385
        public new const int ClassId = 135_747;

        // SmokeCloud.pas:391
        public override int ResourceUserClassId() => SmokeCloud.ClassId;

        // SmokeCloud.pas:388
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: GameText.SmokeCloudText);

    }
}
