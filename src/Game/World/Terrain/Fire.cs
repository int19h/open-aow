#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Audio;
using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Terrain;

// { Interface declaration run owned by P1. }
// Mode, TurnsLeft, Age, _showPriority, Player, ExplosionFrame
public class Fire(CustomNode owner) : ImageLibraryTerrainMapObject(owner)
{

    public new const int ClassId = 132_990;

    private Modes _mode = Modes.Expandable;

    public int TurnsLeft { get; set; } = 3;
    public int Age { get; set; }

    private int _showPriority = 1_744_830_464;

    public PlayerId? Player { get; set; } = PlayerId.Independent;

    private int _explosionFrame;

    // Structur.PAS:3402
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        TurnsLeft = persister.UnsignedByte(name: "TurnsLeft", value: TurnsLeft.TruncateToSByte());
        _mode = persister.Set("Mode", value: _mode);
        Player = persister.OptionalPlayer(name: "Player", value: Player, width: IdWidth.UnsignedByte, fallback: 0);
        Age = persister.Byte("Age", value: Age.TruncateToByte());
    }

    // Structur.PAS:3672
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986 && message.Payload.Int == PlayerIdExtensions.ToOriginal(Player))
        {
            NewTurn();
            return;
        }

        if (message.MessageId == 537001987 && message.Payload.Int == PlayerIdExtensions.ToOriginal(Player))
        {
            NewTurnDone();
        }
        else
        {
            if (message.MessageId == 537002242 && (byte)message.Param[1].Int == 6)
            {
                WaterMasteryCast();
            }
        }
    }

    // Structur.PAS:3411
    public override FootprintCell<TerrainType> GetTerrain(int index)
    {
        FootprintCell<TerrainType> result = base.Resource.TerrainList.GetTerrain(index);
        return result.Value is not null ? FootprintCell<TerrainType>.Keep : result;
    }

    // Structur.PAS:3417
    public override int GetShowPriority() => _showPriority;

    // Structur.PAS:3472
    public override void Connect()
    {
        CalculateShowPriority(Owner);
        base.Connect();
    }

    // Structur.PAS:3517
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (_explosionFrame != 0)
        {
            int frame = StrategicMap.Current.Frame - _explosionFrame;
            if (frame >= 0 && frame < base.Resource.ImageSequenceList.GetImageSequence(20).TotalSequenceFrames)
            {
                HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out int x, out int y);
                _ = base.Resource.ImageSequenceList.Show(20, x - pixelXSurface, y - pixelYSurface, frame, lockedSurface);
            }
            else
            {
                if (frame is < -1_000 or >= 0)
                {
                    _explosionFrame = 0;
                    base.Show(pixelXSurface, pixelYSurface, lockedSurface);
                }
            }
        }
        else
        {
            base.Show(pixelXSurface, pixelYSurface, lockedSurface);
        }
    }

    // Structur.PAS:3478
    public override bool PlaceOnMap()
    {
        bool result = base.PlaceOnMap();
        if (result)
        {
            Owner.UpdateMapField();
        }

        return result;
    }

    // Structur.PAS:3484
    public override void RemoveFromMap()
    {
        MapField? mapField = Owner;
        if (mapField is not null)
        {
            base.RemoveFromMap();
            mapField.UpdateMapField();
        }
    }

    // Structur.PAS:3589
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (message.MessageId != 16 || StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            base.MapFieldMessageProc(ref message);
        }

        if (message.MessageId == 4418 && base.Resource.TerrainList.GetTerrain(message.HexNumber).Value is not null)
        {
            ArmyPlaced(message.Param[0].As<ArmyHexSprite>());
            return;
        }

        if (message.MessageId == 4384 && base.Resource.TerrainList.GetTerrain(message.HexNumber).Value is not null)
        {
            if (!message.Param[0].As<ArmyHexSprite>().Army.ImmunityTypes(255).Contains(DamageTypes.Fire))
            {
                message.Result = 1;
            }
        }
    }

    // Structur.PAS:3570
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        return base.Resource.TerrainList.GetTerrain(info.HexNumber).Value is not null
            ? !CanDoDamage(((MoveControl)info.Sender).Settings.Units) ? 1 : 0
            : 1;
    }

    // Structur.PAS:3564
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (base.Resource.TerrainList.GetTerrain(info.HexNumber).Value is not null)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOverFlag;
        }
    }

    // Structur.PAS:3509
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!Status.Contains(MapObjectStatuses.Visible) && _explosionFrame != 0 && StrategicMap.Current.Frame == _explosionFrame)
        {
            int hexX = HexX;
            _ = AudioPlayback.PlaySample(AbilityRules.ExplosionSfx[(hexX + StrategicMap.Current.Frame) % 2]);
        }

        base.MakeVisible(visibilityList);
    }

    // Structur.PAS:3497
    public override bool CanPlace(MapContainer mapContainer, int hexX, int hexY, int hexLevel)
    {
        bool result = base.CanPlace(mapContainer, hexX, hexY, hexLevel);
        if (result)
        {
            int classId = EngineClass.ClassId;
            return mapContainer.GetField(hexX, hexY, hexLevel).FindOwnedChild(classId) is null;
        }

        return result;
    }

    // Structur.PAS:3435
    public override bool PlaceHex(MapContainer mapContainer, int hexX, int hexY, int hexLevel)
    {
        CalculateShowPriority(mapContainer.GetField(hexX, hexY, hexLevel));
        return base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
    }

    // Structur.PAS:3441
    public override int GetMostFrequentMapFieldTerrainType(MapLevel mapLevel, int hexX, int hexY, int defaultValue)
    {
        Span<byte> counts = stackalloc byte[128];
        int result = defaultValue;
        TerrainList terrains = base.Resource.TerrainList;
        if (result < 0)
        {
            result = 0;
        }

        counts.Clear();
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, radius: 0);
        while (walk.HexNumber < terrains.Count)
        {
            if (base.Resource.TerrainList.GetTerrain(walk.HexNumber).Value is not null && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                // A field without a terrain yet does not count (MAPOBJ.PAS:1146 skips the -1).
                if (mapLevel.GetField(walk.X, walk.Y).TryGetTerrain(out TerrainType fieldTerrain))
                {
                    int terrainType = fieldTerrain.ToOriginal();
                    counts[terrainType] = (byte)(counts[terrainType] + 1);
                    if (counts[terrainType] > counts[result])
                    {
                        result = terrainType;
                    }
                }
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        if (defaultValue >= 0)
        {
            if (counts[result] == counts[defaultValue])
            {
                return defaultValue;
            }
        }

        return result;
    }

    // Structur.PAS:3422
    public void CalculateShowPriority(MapField? mapField)
    {
        HexGeometry.HexToPixel(mapField.HexX, mapField.HexY, out int x, out int y);
        HexSprite? hexSprite = mapField.GetMaxLevelNoneTransparentHexSprite();
        _showPriority = 1_744_830_464 + ((y + 36) << 13) + x + 16;
        if (hexSprite != this && hexSprite is not null)
        {
            if (hexSprite.GetShowPriority() >= _showPriority)
            {
                _showPriority = hexSprite.GetShowPriority() + 1;
            }
        }
    }

    // Structur.PAS:3503
    public void SetupExplosion(int delay)
    {
        if (base.Resource.ImageSequenceList.GetImageSequence(20) is not null)
        {
            _explosionFrame = delay + StrategicMap.Current.Frame;
        }
    }

    // Structur.PAS:3539
    public void TriggerDamage()
    {
        MapFieldMessage message = default;
        message.MessageId = 4357;
        MapFieldMessageParameters param = message.Param;
        param[0] = this;
        message.Param = param;
        message.Result = 0;
        Owner.SendMapFieldMessage(ref message);
    }

    // Structur.PAS:3549
    public bool CanDoDamage(UnitUserList units)
    {
        bool result = false;
        int i = units.GetCount();
        while (!result && i != 0)
        {
            i -= 1;
            Troop? troop = units.GetUnit(i);
            result = !troop.GetImmunityTypes().Contains(DamageTypes.Fire);
        }

        return result;
    }

    // Structur.PAS:3584
    public void ArmyPlaced(ArmyHexSprite? armyHexSprite)
    {
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            armyHexSprite.Army.TriggerFireDamage();
        }
    }

    // Structur.PAS:3622
    public void NewTurn()
    {
        if (Age < 99)
        {
            Age = (Age + 1).TruncateToByte();
        }
    }

    // Structur.PAS:3627
    public void NewTurnDone()
    {
        if (Age > 0)
        {
            MapLevel mapLevel = Owner.Owner;
            int hexY = HexY;
            int hexX = HexX;
            HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, hexX, hexY, radius: 1);
            while (walk.Radius == 1)
            {
                // { Original excludes the top row when spreading fire. }
                if (walk.X >= 0 && walk.X < mapLevel.Width && walk.Y > 0 && walk.Y < mapLevel.Height)
                {
                    int chance = MapField.IgnitionChance(mapLevel.GetField(walk.X, walk.Y));
                    if (chance > StrategicMap.Current.Random(1_000))
                    {
                        RandSeed = StrategicMap.Current.Random(16777215);
                        int hexLevel = HexLevel;
                        Fire? fire = GamePack.Current.FireControl.PlaceFire(walk.X, walk.Y, L: hexLevel, fireType: FireType.Large);
                        if (fire is not null)
                        {
                            fire.Player = Player;
                            fire.TurnsLeft = 3;
                        }
                    }
                }

                HexNumberWalkData.WalkToNextHexNumber(ref walk);
            }
        }

        if (TurnsLeft != 0)
        {
            TurnsLeft = (TurnsLeft - 1).TruncateToSByte();
            if (TurnsLeft != 0)
            {
                TriggerDamage();
            }
            else
            {
                Dispose();
            }
        }
        else
        {
            TriggerDamage();
        }
    }

    // Structur.PAS:3667
    public void WaterMasteryCast()
    {
        if (TurnsLeft != 0)
        {
            Dispose();
        }
    }

    [Flags]
    public enum Modes : byte
    {
        None = 0,
        Expandable = 1 << 0,
    }

    // FireType
    public new class Resource(CustomNode owner) : ImageLibraryTerrainMapObjectResource(owner)
    {
        public new const int ClassId = 132_991;

        public FireType FireType { get; private set; }

        // Structur.PAS:3685
        // FireType
        public override void Dispose()
        {
            if (Engine is not null && !GamePack.Current.Status.Contains(HexSpriteSetStatuses.Destroying))
            {
                GamePack.Current.FireControl.UnregisterFireResource(this);
            }

            base.Dispose();
        }

        // Structur.PAS:3707
        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            FireType = persister.Enum("FireType", value: FireType);
        }

        // Structur.PAS:3713
        public override void MessageProc(ref EngineMessage message, EngineObject? sender)
        {
            base.MessageProc(ref message, sender);
            if (message.MessageId == 536870920)
            {
                GamePack.Current.FireControl.RegisterFireResource(this);
            }
        }

        // Structur.PAS:3702
        public override int ResourceUserClassId() => Fire.ClassId;

        // Structur.PAS:3692
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.FireText);

    }
}
