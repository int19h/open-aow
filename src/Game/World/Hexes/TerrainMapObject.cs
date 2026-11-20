#nullable disable
// Translated from Terrain.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Hexes;

public partial class TerrainMapObject(CustomNode owner) : MultiHexMapObject(owner)
{

    // Terrain.pas:65
    public new const int ClassId = 131430;

    public override TerrainMapObjectResource Resource => GetResource<TerrainMapObjectResource>();

    // Terrain.pas:179
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info) => this == info.Sender || Resource.TerrainList.GetTerrain(info.HexNumber) < 0 || (GetMostFrequentMapFieldTerrainType(defaultValue: Resource.TerrainList.GetTerrain(info.HexNumber), mapLevel: Owner.Owner, hexX: StoredHexX, hexY: StoredHexY) == info.MapField.TerrainType && CanChangeOwnTerrain(info.HexNumber));

    // Terrain.pas:195
    public override void ChangeTerrain(in HexSpriteTerrainInfo info)
    {
        sbyte terrainType = Resource.TerrainList.GetTerrain(info.HexNumber);
        if (terrainType >= 0 && terrainType != info.TerrainType)
        {
            sbyte newTerrainType = GetMostFrequentMapFieldTerrainType(defaultValue: Resource.TerrainList.GetTerrain(info.HexNumber), mapLevel: Owner.Owner, hexX: StoredHexX, hexY: StoredHexY);
            if (newTerrainType != terrainType)
            {
                TerrainMapObjectResource r = FindTerrainResource((byte)newTerrainType);
                if (r is not null)
                {
                    MapField oldOwner = Owner;
                    RemoveFromMap();
                    SetResource(r);
                    Owner = oldOwner;
                    _ = PlaceOnMap();
                    if (Resource.Level <= 15)
                    {
                        Resource.Engine.HexSpriteMap.StaticSceneInvalid = true;
                    }

                    OverrideTerrain();
                }
                else
                {
                    Dispose();
                }
            }
        }
    }

    // Terrain.pas:275
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (message.MessageId == 16)
        {
            if (Resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
            {
                ClearTerrain(message.MapField, message.HexNumber);
            }
            else
            {
                if (message.HexNumber == 0)
                {
                    Dispose();
                }
            }
        }
        else
        {
            base.MapFieldMessageProc(ref message);
        }

        if (message.MessageId == 17)
        {
            if (Resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
            {
                message.Result = Resource.DecorationType;
            }
        }
    }

    // Terrain.pas:291
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result = base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
        if (result && !EditMode)
        {
            OverrideTerrain();
        }

        return result;
    }

    // Terrain.pas:226
    public virtual void ClearTerrain(MapField mapField, int hexNumber)
    {
        if (Resource.TerrainList.UsedTerrain() > 1)
        {
            TerrainMapObjectResource r = null;
            MapObjectResourceList resources = GetResourceList(Resource.Engine);
            int i = resources.Count;
            bool found = false;
            while (!found && i != 0)
            {
                i -= 1;
                if (resources[i] is TerrainMapObjectResource terrain)
                {
                    r = terrain;
                    found = r.EngineClass.ClassId == Resource.EngineClass.ClassId && r.DecorationType == Resource.DecorationType && r.TerrainType == Resource.TerrainType && r.TerrainList.UsedTerrain() == 1 && r.TerrainList.GetTerrain(0) >= 0 && GetOverlay(0) == r.TerrainList.GetOverlay(0);
                }
            }

            if (found)
            {
                int hexX = HexX;
                int hexY = HexY;
                int hexLevel = HexLevel;
                GameMap map = Owner.Owner.Map;
                TerrainList terrainList = Resource.TerrainList;
                Dispose();
                i = 0;
                int last = terrainList.Count - 1;
                int x = default;
                int y = default;
                for (; i <= last; i += 1)
                {
                    if (terrainList.GetTerrain(i) >= 0 && i != hexNumber)
                    {
                        HexGeometry.CenterHexNumberToHex(hexX, hexY, hexNumber: i, ref x, ref y);
                        _ = r.PlaceResourceUser(map, (sbyte)x, (sbyte)y, (sbyte)hexLevel);
                    }
                }
            }
            else
            {
                Dispose();
            }
        }
        else
        {
            Dispose();
        }
    }

    // Terrain.pas:70
    public virtual TerrainMapObjectResource FindDecorationResource(byte decorationType)
    {
        DelphiList<TerrainMapObjectResource> candidates = new();
        MapObjectResourceList resources = GetResourceList(Resource.Engine);
        for (int i = 0, last = resources.Count - 1; i <= last; i++)
        {
            if (resources[i] is TerrainMapObjectResource terrain)
            {
                TerrainMapObjectResource r = terrain;
                if (r.EngineClass.ClassId == Resource.EngineClass.ClassId && r.TerrainType == Resource.TerrainType && r.DecorationType == decorationType && r != Resource && r.TerrainList.CompareTerrain(Resource.TerrainList, mode: Set<TerrainCompareModeElement>.Of(TerrainCompareModeElement.SameOverlayType)))
                {
                    _ = candidates.Add(r);
                }
            }
        }

        TerrainMapObjectResource result;
        if (candidates.Count != 0)
        {
            int random = Random(candidates.Count);
            result = candidates.List[random];
        }
        else
        {
            result = null;
        }

        candidates?.Dispose();
        return result;
    }

    // Terrain.pas:160
    public bool CanChangeOwnTerrain(int hexNumber)
    {
        bool result = true;
        MapLevel level = Owner.Owner;
        int count = TerrainCount;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, StoredHexX, StoredHexY, radius: 0);
        while (walk.HexNumber < count && result)
        {
            if (walk.HexNumber != hexNumber && GetTerrain(walk.HexNumber) >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                result = level.GetField(walk.X, walk.Y).CanChangeTerrain(this);
            }

            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }

        return result;
    }
}
