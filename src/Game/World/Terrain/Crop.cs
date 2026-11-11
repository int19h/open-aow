// Translated from Crops.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

namespace Aow.Game.World.Terrain;

// FCropOwner
public partial class Crop : ImageLibraryTerrainMapObject
{
    public Crop(ECustomNode owner) : base(owner)
    {
    }

    // Crops.pas:119
    public new const int ClassID = 131946;

    public PlayerCropStructure FCropOwner;

    // Crops.pas:109

    // Crops.pas:151
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001991)
        {
            EndTurn((sbyte)Msg.Payload.Int);
        }
    }

    // Crops.pas:203
    public override void TerrainChanged(in HexSpriteTerrainInfo Info)
    {
        HexSprite HS;
        if (Info.HN != 0)
        {
            return;
        }

        if (FStatus.Contains(MapObjectStatusElement.Activated))
        {
            if (ValidTerrainType(Info.TerrainType))
            {
                FTerrainType = Info.TerrainType;
                FImageIndex = GetTerrainTypeImage(Info.TerrainType);
                HS = Info.MapField.GetMaxLevelNoneTransparentHS();
                if ((HS != null && HS.GetLevel() > GetLevel() && !(HS is AbstractRoad)) || (Info.OverlayType >= 0 && Info.OverlayType != 4))
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
            FTerrainType = Info.TerrainType;
        }
    }

    // Crops.pas:157
    public override void MapFieldMsgProc(ref MapFieldMessage Msg)
    {
        if (Msg.MsgID != 16)
        {
            base.MapFieldMsgProc(ref Msg);
        }

        if (Msg.MsgID == 4357)
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
        if (FCropOwner != null && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Destroying))
        {
            var playerCropStructure = FCropOwner;
            this.FCropOwner = null;
            playerCropStructure.CropsChanged();
        }

        base.Deactivate();
    }

    // Crops.pas:200
    public override bool CanSelect(int Xhp, int Yhp) => false;

    // Crops.pas:234
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        MapField Field;
        HexSprite HS;
        Field = MapContainer.GetField(Xhx, Yhx, Lhx);
        HS = Field.GetMaxLevelNoneTransparentHS();
        return Field.FindOwnedHS(typeof(Fire)) == null && (HS == null || HS.GetLevel() < 10 || HS is AbstractRoad);
    }

    // Crops.pas:228
    public override int GetTerrainTypeImage(int TerrainType)
    {
        int Result = TerrainType;
        if (FCropOwner != null && FCropOwner.GetRace() == 11)
        {
            Result += 30;
        }

        return Result;
    }

    // Crops.pas:122
    public void SetCropOwner(PlayerCropStructure Value)
    {
        FCropOwner = Value;
        UpdateSettings();
    }

    // Crops.pas:128
    public void UpdateSettings()
    {
        int Index;
        Index = GetTerrainTypeImage(TerrainType);
        this.FImageIndex = Index;
        if (!FResource.ImageLibrary.ImageExist(Index))
        {
            Dispose();
        }
    }

    // Crops.pas:136
    public void EndTurn(sbyte Player)
    {
        sbyte PlayerIndex;
        ArmyHexSprite Army;
        Army = ((AoWMapField)Owner).GetArmyHS();
        PlayerIndex = Player;
        if (Army != null && Army.FArmy.FPlayer == PlayerIndex && FCropOwner.FPlayer > 0 && PlayerIndex != FCropOwner.FPlayer)
        {
            if (AoWHSMap.Current.FPlayers.GetDiplomaticRelation(FCropOwner.FPlayer, PlayerIndex) == DiplomaticRelation.War)
            {
                FCropOwner.CropTrampled(PlayerIndex);
                Dispose();
            }
        }
    }

    // Crops.pas:163
    public void LinkToCropOwner()
    {
        PlayerCropStructure Found;
        MapLevel MapLevel;
        HexNumberWalkData Walk = default;
        Found = null;
        MapLevel = (MapLevel)Owner.Owner;
        HexNumberWalkData.InitHNWalk(ref Walk, FXhx, FYhx, 1);
        while (Walk.Radius <= 1 && Found == null)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
            {
                Found = (PlayerCropStructure)MapLevel.GetField(Walk.X, Walk.Y).FindNoneTransparentHS(typeof(PlayerCropStructure));
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        SetCropOwner(Found);
    }

    public new partial class Resource : ImageLibraryTerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // Crops.pas:245
        public new const int ClassID = 131947;

        // Crops.pas:251
        public override int ResourceUserClassID() => 131946;

        // Crops.pas:248
        public override void SetLevel(byte Value)
        {
            base.SetLevel(5);
        }

        // Crops.pas:257
        public Crop PlaceCrop(sbyte X, sbyte Y, sbyte L, PlayerCropStructure CropOwner)
        {
            Crop Result = new Crop(null);
            Result.SetResource(this);
            Result.ResourceLinked = true;
            Result.FCropOwner = CropOwner;
            if (!AoWHSMap.Current.PlaceHX(Result, X, Y, L))
            {
                Result?.Dispose();
                Result = null;
            }

            return Result;
        }

        // Crops.pas:254
        public override string ResourceEditName() => AbilityRules.TranslateRStr(CropRStr);

        private static readonly string CropRStr = "Crops";

    }

    // Crops.pas:68

}
