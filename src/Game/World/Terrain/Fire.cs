// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Audio;
using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Terrain;

// { Interface declaration run owned by P1. }
// Mode, TurnsLeft, Age, _showPriority, Player, ExplosionFrame
public partial class Fire : ImageLibraryTerrainMapObject
{

    public new const int ClassId = 132990;

    private Set<ModeElement> _mode;
    public sbyte TurnsLeft;
    public byte Age;
    private int _showPriority;
    public sbyte Player;
    private int _explosionFrame;

    // Structur.PAS:3402
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref TurnsLeft), 5);
        TransferSet(ref _mode, 1, 6, stream.Bytes, 0);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref Player), 7);
        stream.Byte(ref Age, 8);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        TurnsLeft = PersistSupport.UnsignedByte(persister, name: "TurnsLeft", value: TurnsLeft);
        _mode = persister.Set("Mode", value: _mode);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        Age = persister.Byte("Age", value: Age);
    }

    // Structur.PAS:3386
    // mode, lifetime, age, priority, owner player, explosion frame
    public Fire(ECustomNode owner) : base(owner)
    {
        TurnsLeft = 3;
        Age = 0;
        Player = 0;
        _showPriority = 1744830464;
        _mode = Set<ModeElement>.Of(ModeElement.Expandable);
        _explosionFrame = 0;
    }

    // Structur.PAS:3672
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001986 && message.Payload.Int == Player)
        {
            NewTurn();
            return;
        }

        if (message.MessageId == 537001987 && message.Payload.Int == Player)
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
    public override sbyte GetTerrain(int index)
    {
        sbyte result = _resource.TerrainList.GetTerrain(index);
        if (result >= 0)
        {
            result = -1;
        }

        return result;
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
        int frame;
        int X = default;
        int Y = default;
        if (_explosionFrame != 0)
        {
            frame = StrategicMap.Current.Frame - _explosionFrame;
            if (frame >= 0 && frame < _resource.ImageSequenceList.GetImageSequence(20).GetTotalSequenceFrames())
            {
                HexGeometry.HexToPixel(Owner.HexX, Owner.HexY, out X, out Y);
                _resource.ImageSequenceList.Show(20, X - pixelXSurface, Y - pixelYSurface, frame, lockedSurface);
            }
            else
            {
                if (frame < -1000 || frame >= 0)
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
        MapField mapField;
        mapField = Owner;
        if (mapField != null)
        {
            base.RemoveFromMap();
            mapField.UpdateMapField();
        }
    }

    // Structur.PAS:3589
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (message.MessageId != 16 || StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            base.MapFieldMessageProc(ref message);
        }

        if (message.MessageId == 4418 && _resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
        {
            ArmyPlaced(message.Param[0].As<ArmyHexSprite>());
            return;
        }

        if (message.MessageId == 4384 && _resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
        {
            if (!message.Param[0].As<ArmyHexSprite>().Army.ImmunityTypes(255).Contains(DamageType.Fire))
            {
                message.Result = 1;
            }
        }
    }

    // Structur.PAS:3570
    public override int CanMoveOver(in HexSpriteSenderInfo info)
    {
        int result;
        if (_resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
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

    // Structur.PAS:3564
    public override void UpdateMapField(in HexSpriteMapFieldInfo info)
    {
        if (_resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            info.MapField.FieldFlags = info.MapField.FieldFlags | MapField.MoveOverFlag;
        }
    }

    // Structur.PAS:3509
    public override void MakeVisible(VisibilityList visibilityList)
    {
        if (!Status.Contains(MapObjectStatusElement.Visible) && _explosionFrame != 0 && StrategicMap.Current.Frame == _explosionFrame)
        {
            var hexX = GetHexX();
            AudioPlayback.PlaySample(AbilityRules.ExplosionSfx[(hexX + StrategicMap.Current.Frame) % 2]);
        }

        base.MakeVisible(visibilityList);
    }

    // Structur.PAS:3497
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result = base.CanPlace(mapContainer, hexX, hexY, hexLevel);
        if (result)
        {
            var classId = EngineClass.ClassId;
            result = mapContainer.GetField(hexX, hexY, hexLevel).FindOwnedChild(classId) == null;
        }

        return result;
    }

    // Structur.PAS:3435
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        CalculateShowPriority(mapContainer.GetField(hexX, hexY, hexLevel));
        return base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
    }

    // Structur.PAS:3441
    public override sbyte GetMostFrequentMapFieldTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY, sbyte defaultValue)
    {
        TerrainList terrains;
        Span<byte> counts = stackalloc byte[128];
        HexNumberWalkData walk = default;
        sbyte terrainType;
        sbyte result = defaultValue;
        terrains = _resource.TerrainList;
        if (result < 0)
        {
            result = 0;
        }

        counts.Clear();
        HexNumberWalkData.InitializeHexNumberWalk(out walk, hexX, hexY, 0);
        while (walk.HexNumber < terrains.Count)
        {
            if (_resource.TerrainList.GetTerrain(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                terrainType = mapLevel.GetField(walk.X, walk.Y).TerrainType;
                if (terrainType >= 0)
                {
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
                result = defaultValue;
            }
        }

        return result;
    }

    // Structur.PAS:3422
    public void CalculateShowPriority(MapField mapField)
    {
        int X = default;
        int Y = default;
        HexSprite hexSprite;
        HexGeometry.HexToPixel(mapField.HexX, mapField.HexY, out X, out Y);
        hexSprite = mapField.GetMaxLevelNoneTransparentHexSprite();
        _showPriority = 1744830464 + ((Y + 36) << 13) + (X + 16);
        if (hexSprite != this && hexSprite != null)
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
        if (_resource.ImageSequenceList.GetImageSequence(20) != null)
        {
            _explosionFrame = delay + StrategicMap.Current.Frame;
        }
    }

    // Structur.PAS:3539
    public void TriggerDamage()
    {
        MapFieldMessage message = default;
        message.MessageId = 4357;
        message.Param[0] = this;
        message.Result = 0;
        Owner.SendMapFieldMessage(ref message);
    }

    // Structur.PAS:3549
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
            result = !troop.GetImmunityTypes().Contains(DamageType.Fire);
        }

        return result;
    }

    // Structur.PAS:3584
    public void ArmyPlaced(ArmyHexSprite armyHexSprite)
    {
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            armyHexSprite.Army.TriggerFireDamage();
        }
    }

    // Structur.PAS:3622
    public void NewTurn()
    {
        if (Age < 99)
        {
            Age = (byte)(Age + 1);
        }
    }

    // Structur.PAS:3627
    public void NewTurnDone()
    {
        HexNumberWalkData walk = default;
        MapLevel mapLevel;
        int chance;
        Fire fire;
        if (Age > 0)
        {
            mapLevel = Owner.Owner;
            var hexY = GetHexY();
            var hexX = GetHexX();
            HexNumberWalkData.InitializeHexNumberWalk(out walk, hexX, hexY, 1);
            while (walk.Radius == 1)
            {
                // { Original excludes the top row when spreading fire. }
                if (walk.X >= 0 && walk.X < mapLevel.Width && walk.Y > 0 && walk.Y < mapLevel.Height)
                {
                    chance = MapField.IgnitionChance(mapLevel.GetField(walk.X, walk.Y));
                    if (chance > StrategicMap.Current.Random(1000))
                    {
                        RandSeed = StrategicMap.Current.Random(16777215);
                        var hexLevel = GetHexLevel();
                        fire = GamePack.Current.FireControl.PlaceFire(walk.X, walk.Y, hexLevel, FireType.Large);
                        if (fire != null)
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
            TurnsLeft = (sbyte)(TurnsLeft - 1);
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

    public enum ModeElement : byte
    {
        Expandable = 0
    }

    // FireType
    public new partial class Resource : ImageLibraryTerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        public new const int ClassId = 132991;

        public FireType FireType;

        // Structur.PAS:3685
        // FireType
        public override void Dispose()
        {
            if (Engine != null && !GamePack.Current.Status.Contains(HexSpriteSetStatusElement.Destroying))
            {
                GamePack.Current.FireControl.UnregisterFireResource(this);
            }

            base.Dispose();
        }

        // Structur.PAS:3707
        public override void ReadWrite(StorageStream stream)
        {
            base.ReadWrite(stream);
            stream.Bytes(Bytes.Of(ref FireType, 1), 1, 20);
        }

        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            FireType = persister.Enum("FireType", value: FireType);
        }

        // Structur.PAS:3713
        public override void MessageProc(ref EngineMessage message, EngineObject sender)
        {
            base.MessageProc(ref message, sender);
            if (message.MessageId == 536870920)
            {
                GamePack.Current.FireControl.RegisterFireResource(this);
            }
        }

        // Structur.PAS:3702
        public override int ResourceUserClassId() => 132990;

        // Structur.PAS:3692
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), GameTables.FireText);

    }

    // intf_P1_11.inc:16

}
