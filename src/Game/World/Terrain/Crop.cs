#nullable disable
// Translated from Crops.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Terrain;

// CropOwner
public class Crop(CustomNode owner) : ImageLibraryTerrainMapObject(owner)
{

    // Crops.pas:119
    public new const int ClassId = 131_946;

    public PlayerCropStructure CropOwner { get; private set; }

    // Crops.pas:151
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001991)
        {
            EndTurn((sbyte)message.Payload.Int);
        }
    }

    // Crops.pas:203
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        if (info.HexNumber != 0)
        {
            return;
        }

        if (Status.Contains(MapObjectStatuses.Activated))
        {
            if (ValidTerrainType(info.TerrainType))
            {
                TerrainType = info.TerrainType;
                ImageIndex = GetTerrainTypeImage(info.TerrainType);
                HexSprite hexSprite = info.MapField.GetMaxLevelNoneTransparentHexSprite();
                if ((hexSprite is not null && hexSprite.Level > Level && hexSprite is not AbstractRoad) || (info.OverlayType is >= 0 and not 4))
                {
                    Dispose();
                }
            }
            else
            {
                Dispose();
            }
        }
        else
        {
            TerrainType = info.TerrainType;
        }
    }

    // Crops.pas:157
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (message.MessageId != 16)
        {
            base.MapFieldMessageProc(ref message);
        }

        if (message.MessageId == 4357)
        {
            Dispose();
        }
    }

    // Crops.pas:183
    public override void Activate()
    {
        LinkToCropOwner();
        base.Activate();
    }

    // Crops.pas:189
    public override void Deactivate()
    {
        if (CropOwner is not null && !StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Destroying))
        {
            PlayerCropStructure playerCropStructure = CropOwner;
            CropOwner = null;
            playerCropStructure.CropsChanged();
        }

        base.Deactivate();
    }

    // Crops.pas:200
    public override bool CanSelect(int pixelX, int pixelY) => false;

    // Crops.pas:234
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapField field = mapContainer.GetField(hexX, hexY, hexLevel);
        HexSprite hexSprite = field.GetMaxLevelNoneTransparentHexSprite();
        return field.FindOwnedHexSprite(typeof(Fire)) is null && (hexSprite is null || hexSprite.Level < 10 || hexSprite is AbstractRoad);
    }

    // Crops.pas:228
    public override int GetTerrainTypeImage(int terrainType)
    {
        int result = terrainType;
        if (CropOwner is not null && CropOwner.GetRace() == 11)
        {
            result += 30;
        }

        return result;
    }

    // Crops.pas:122
    public void SetCropOwner(PlayerCropStructure value)
    {
        CropOwner = value;
        UpdateSettings();
    }

    // Crops.pas:128
    public void UpdateSettings()
    {
        int index = GetTerrainTypeImage(TerrainType);
        ImageIndex = index;
        if (!base.Resource.ImageLibrary.ImageExist(index))
        {
            Dispose();
        }
    }

    // Crops.pas:136
    public void EndTurn(sbyte player)
    {
        ArmyHexSprite army = Owner.GetArmyHexSprite();
        sbyte playerIndex = player;
        if (army is not null && army.Army.Player == playerIndex && CropOwner.Player > 0 && playerIndex != CropOwner.Player)
        {
            if (StrategicMap.Current.Players.GetDiplomaticRelation(CropOwner.Player, otherPlayer: playerIndex) == DiplomaticRelation.War)
            {
                CropOwner.CropTrampled(playerIndex);
                Dispose();
            }
        }
    }

    // Crops.pas:163
    public void LinkToCropOwner()
    {
        PlayerCropStructure found = null;
        MapLevel mapLevel = Owner.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 1);
        while (walk.Radius <= 1 && found is null)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < mapLevel.Width && walk.Y < mapLevel.Height)
            {
                found = mapLevel.GetField(walk.X, walk.Y).FindNoneTransparentHexSprite<PlayerCropStructure>();
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        SetCropOwner(found);
    }

    public new class Resource(CustomNode owner) : ImageLibraryTerrainMapObjectResource(owner)
    {

        // Crops.pas:245
        public new const int ClassId = 131_947;

        // Crops.pas:251
        public override int ResourceUserClassId() => Crop.ClassId;

        // Crops.pas:248
        public override void SetLevel(byte value) => base.SetLevel(5);

        // Crops.pas:257
        public Crop PlaceCrop(sbyte X, sbyte Y, sbyte L, PlayerCropStructure cropOwner)
        {
            Crop result = new(null) { Holder = CreationHolder() };
            result.SetResource(this);
            result.ResourceLinked = true;
            result.CropOwner = cropOwner;
            if (!StrategicMap.Current.PlaceHex(result, X, Y, L))
            {
                result?.Dispose();
                return null;
            }

            return result;
        }

        // Crops.pas:254
        public override string ResourceEditName() => AbilityRules.TranslateResourceString(GetEngine(), S: s_cropText);

        private static readonly string s_cropText = "Crops";

    }
}
