#nullable disable
// Translated from Terrain.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

public partial class TerrainMapObject : MultiHexMapObject
{
    public TerrainMapObject(CustomNode owner) : base(owner)
    {
    }

    // Terrain.pas:65
    public new const int ClassId = 131430;

    // Terrain.pas:60

    // Terrain.pas:179
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info)
    {
        bool result;
        if (this != info.Sender)
        {
            if (Resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
            {
                if (GetMostFrequentMapFieldTerrainType(defaultValue: Resource.TerrainList.GetTerrain(info.HexNumber), mapLevel: Owner.Owner, hexX: StoredHexX, hexY: StoredHexY) == info.MapField.TerrainType)
                {
                    result = CanChangeOwnTerrain(info.HexNumber);
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // Terrain.pas:195
    public override void ChangeTerrain(in HexSpriteTerrainInfo info)
    {
        sbyte terrainType;
        sbyte newTerrainType;
        TerrainMapObjectResource r;
        MapField oldOwner;
        terrainType = Resource.TerrainList.GetTerrain(info.HexNumber);
        if (terrainType >= 0 && terrainType != info.TerrainType)
        {
            newTerrainType = GetMostFrequentMapFieldTerrainType(defaultValue: Resource.TerrainList.GetTerrain(info.HexNumber), mapLevel: Owner.Owner, hexX: StoredHexX, hexY: StoredHexY);
            if (newTerrainType != terrainType)
            {
                r = FindTerrainResource((byte)newTerrainType);
                if (r != null)
                {
                    oldOwner = Owner;
                    RemoveFromMap();
                    SetResource(r);
                    Owner = oldOwner;
                    PlaceOnMap();
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
                message.Result = ((TerrainMapObjectResource)Resource).DecorationType;
            }
        }
    }

    // Terrain.pas:291
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result = base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
        if (result && !GetEditMode())
        {
            OverrideTerrain();
        }

        return result;
    }

    // Terrain.pas:226
    public virtual void ClearTerrain(MapField mapField, int hexNumber)
    {
        int hexX;
        int hexY;
        int hexLevel;
        int x = default;
        int y = default;
        TerrainList terrainList;
        GameMap map;
        TerrainMapObjectResource r;
        MapObjectResourceList resources;
        int i;
        bool found;
        if (Resource.TerrainList.UsedTerrain() > 1)
        {
            r = null;
            resources = GetResourceList(Resource.Engine);
            i = resources.Count;
            found = false;
            while (!found && i != 0)
            {
                i -= 1;
                if (resources[i] is TerrainMapObjectResource terrain)
                {
                    r = terrain;
                    found = r.EngineClass.ClassId == Resource.EngineClass.ClassId && r.DecorationType == ((TerrainMapObjectResource)Resource).DecorationType && r.TerrainType == ((TerrainMapObjectResource)Resource).TerrainType && r.TerrainList.UsedTerrain() == 1 && r.TerrainList.GetTerrain(0) >= 0 && GetOverlay(0) == r.TerrainList.GetOverlay(0);
                }
            }

            if (found)
            {
                hexX = GetHexX();
                hexY = GetHexY();
                hexLevel = GetHexLevel();
                map = Owner.Owner.Map;
                terrainList = Resource.TerrainList;
                Dispose();
                i = 0;
                int last = terrainList.Count - 1;
                for (; i <= last; i += 1)
                {
                    if (terrainList.GetTerrain(i) >= 0 && i != hexNumber)
                    {
                        HexGeometry.CenterHexNumberToHex(hexX, hexY, i, ref x, ref y);
                        r.PlaceResourceUser(map, (sbyte)x, (sbyte)y, (sbyte)hexLevel);
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
        TerrainMapObjectResource result;
        DelphiList<TerrainMapObjectResource> candidates;
        MapObjectResourceList resources;
        TerrainMapObjectResource r;
        candidates = new DelphiList<TerrainMapObjectResource>();
        resources = GetResourceList(Resource.Engine);
        for (int i = 0, last = resources.Count - 1; i <= last; i++)
        {
            if (resources[i] is TerrainMapObjectResource terrain)
            {
                r = terrain;
                if (r.EngineClass.ClassId == Resource.EngineClass.ClassId && r.TerrainType == ((TerrainMapObjectResource)Resource).TerrainType && r.DecorationType == decorationType && r != Resource && r.TerrainList.CompareTerrain(Resource.TerrainList, Set<TerrainCompareModeElement>.Of(TerrainCompareModeElement.SameOverlayType)))
                {
                    candidates.Add(r);
                }
            }
        }

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
        MapLevel level;
        int count;
        HexNumberWalkData walk = default;
        bool result = true;
        level = Owner.Owner;
        count = GetTerrainCount();
        HexNumberWalkData.InitializeHexNumberWalk(out walk, StoredHexX, StoredHexY, 0);
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

    // Terrain.pas:19

}
