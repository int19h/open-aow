#nullable disable warnings
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
public class Crop(CustomNode? owner) : ImageLibraryTerrainMapObject(owner)
{

    // Crops.pas:119
    public new const int ClassId = 131_946;

    public PlayerCropStructure? CropOwner { get; private set; }

    // Crops.pas:151
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001991)
        {
            EndTurn(PlayerId.FromOriginal((sbyte)message.Payload.Int));
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
            if (ValidTerrainType(info.TerrainNumber))
            {
                TerrainType = info.TerrainNumber;
                ImageIndex = GetTerrainTypeImage(info.TerrainNumber);
                HexSprite? hexSprite = info.MapField.GetMaxLevelNoneTransparentHexSprite();
                if ((hexSprite is not null && hexSprite.Level > Level && hexSprite is not AbstractRoad) || (info.OverlayType is not null and not OverlayType.Road))
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
            TerrainType = info.TerrainNumber;
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
        if (CropOwner is not null && !GetEngine().StrategicMap.Status.Contains(HexSpriteMapStatuses.Destroying))
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
    public override bool CanPlace(MapContainer mapContainer, int hexX, int hexY, int hexLevel)
    {
        MapField field = mapContainer.GetField(hexX, hexY, hexLevel);
        HexSprite? hexSprite = field.GetMaxLevelNoneTransparentHexSprite();
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
    public void SetCropOwner(PlayerCropStructure? value)
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
    public void EndTurn(PlayerId? player)
    {
        ArmyHexSprite army = Owner.GetArmyHexSprite();
        sbyte playerIndex = (sbyte)PlayerIdExtensions.ToOriginal(player);
        if (army is not null && PlayerIdExtensions.ToOriginal(army.Army.Player) == playerIndex && PlayerIdExtensions.ToOriginal(CropOwner.Player) > 0 && playerIndex != PlayerIdExtensions.ToOriginal(CropOwner.Player))
        {
            if (GetEngine().StrategicMap.Players.GetDiplomaticRelation(CropOwner.Player, otherPlayer: PlayerId.FromOriginal(playerIndex)) == DiplomaticRelation.War)
            {
                CropOwner.CropTrampled(PlayerId.FromOriginal(playerIndex));
                Dispose();
            }
        }
    }

    // Crops.pas:163
    public void LinkToCropOwner()
    {
        PlayerCropStructure? found = null;
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
        public override void SetLevel(int value) => base.SetLevel(5);

        // Crops.pas:257
        public Crop? PlaceCrop(int X, int Y, int L, PlayerCropStructure cropOwner)
        {
            Crop result = new(null) { Holder = CreationHolder() };
            result.SetResource(this);
            result.ResourceLinked = true;
            result.CropOwner = cropOwner;
            if (!GetEngine().StrategicMap.PlaceHex(result, X, Y, L))
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
