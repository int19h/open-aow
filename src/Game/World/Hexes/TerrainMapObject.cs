// Translated from Terrain.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

public partial class TerrainMapObject : MultiHexMapObject
{
    public TerrainMapObject(ECustomNode owner) : base(owner)
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
            if (_resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
            {
                var terrain = _resource.TerrainList.GetTerrain(info.HexNumber);
                if (GetMostFrequentMapFieldTerrainType((MapLevel)Owner.Owner, _hexX, _hexY, terrain) == info.MapField.TerrainType)
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
        TerrainMapObjectResource R;
        ECustomNode oldOwner;
        terrainType = _resource.TerrainList.GetTerrain(info.HexNumber);
        if (terrainType >= 0 && terrainType != info.TerrainType)
        {
            var terrain = _resource.TerrainList.GetTerrain(info.HexNumber);
            newTerrainType = GetMostFrequentMapFieldTerrainType((MapLevel)Owner.Owner, _hexX, _hexY, terrain);
            if (newTerrainType != terrainType)
            {
                R = FindTerrainResource((byte)newTerrainType);
                if (R != null)
                {
                    oldOwner = Owner;
                    RemoveFromMap();
                    SetResource(R);
                    Owner = oldOwner;
                    PlaceOnMap();
                    if (_resource.Level <= 15)
                    {
                        _resource.Engine.HexSpriteMap.StaticSceneInvalid = true;
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
            if (_resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
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
            if (_resource.TerrainList.GetTerrain(message.HexNumber) >= 0)
            {
                message.Result = ((TerrainMapObjectResource)_resource)._decorationType;
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
        int X = default;
        int Y = default;
        TerrainList terrainList;
        HSMap map;
        TerrainMapObjectResource R;
        EngineObjectList resources;
        int I;
        bool found;
        if (_resource.TerrainList.UsedTerrain() > 1)
        {
            R = null;
            resources = GetResourceList(_resource.Engine)._children;
            I = resources.Count;
            found = false;
            while (!found && I != 0)
            {
                I -= 1;
                if (resources.List[I] is TerrainMapObjectResource terrain)
                {
                    R = terrain;
                    found = R.EngineClass.ClassId == _resource.EngineClass.ClassId && R._decorationType == ((TerrainMapObjectResource)_resource)._decorationType && R._terrainType == ((TerrainMapObjectResource)_resource)._terrainType && R.TerrainList.UsedTerrain() == 1 && R.TerrainList.GetTerrain(0) >= 0 && GetOverlay(0) == R.TerrainList.GetOverlay(0);
                }
            }

            if (found)
            {
                hexX = GetHexX();
                hexY = GetHexY();
                hexLevel = GetHexLevel();
                map = ((MapLevel)Owner.Owner).Map;
                terrainList = _resource.TerrainList;
                Dispose();
                I = 0;
                int last = terrainList.Count - 1;
                for (; I <= last; I += 1)
                {
                    if (terrainList.GetTerrain(I) >= 0 && I != hexNumber)
                    {
                        HexGeometry.CenterHexNumberToHex(hexX, hexY, I, ref X, ref Y);
                        R.PlaceResourceUser(map, (sbyte)X, (sbyte)Y, (sbyte)hexLevel);
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
        DelphiList candidates;
        EngineObjectList resources;
        TerrainMapObjectResource R;
        candidates = new DelphiList();
        resources = GetResourceList(_resource.Engine)._children;
        for (int I = 0, last = resources.Count - 1; I <= last; I++)
        {
            if (resources.List[I] is TerrainMapObjectResource terrain)
            {
                R = terrain;
                if (R.EngineClass.ClassId == _resource.EngineClass.ClassId && R._terrainType == ((TerrainMapObjectResource)_resource)._terrainType && R._decorationType == decorationType && R != _resource && R.TerrainList.CompareTerrain(_resource.TerrainList, Set<TerrainCompareModeElement>.Of(TerrainCompareModeElement.SameOverlayType)))
                {
                    candidates.Add(R);
                }
            }
        }

        if (candidates.Count != 0)
        {
            var random = Random(candidates.Count);
            result = (TerrainMapObjectResource)candidates.List[random];
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
        level = (MapLevel)Owner.Owner;
        count = GetTerrainCount();
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, _hexX, _hexY, 0);
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
