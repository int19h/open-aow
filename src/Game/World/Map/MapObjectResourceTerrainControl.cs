#nullable disable
// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.World.Map;

// FHSEngine, FTries, FResources, FMOClass. The map object resources of one kind
// (THexagonControl: hexagons, TRoadControl: roads) sorted by terrain type: FResources holds one
// TList of resources per terrain type name of the set (THSSet.TerrainTypes), filled by
// RegisterResource when a resource is activated. Place and PlaceClass put a random resource of
// a terrain type on a map field
public class MapObjectResourceTerrainControl(CustomNode owner) : EngineObject(owner)
{

    // HSEngine.pas:4522
    internal void Initialize()
    {
        Clear();
        for (int i = 0, last = HexSpriteEngine.HexSpriteSet.TerrainTypes.Count - 1; i <= last; i++)
        {
            DelphiList<MapObjectResource> list = new();
            Resources.Add(list);
        }
    }

    // HSEngine.pas:4497
    public override void Dispose()
    {
        Clear();
        Resources?.Clear();
        Resources = null;
        base.Dispose();
    }

    // HSEngine.pas:4505
    public override ControlStyles GetControlStyle() => EngineServices.Shared;

    // HSEngine.pas:4510
    public override void Clear()
    {
        if (Resources is not null)
        {
            for (int i = Resources.Count - 1; i >= 0; i--)
            {
                (Resources[i] as IDisposable)?.Dispose();
                Resources.RemoveAt(i);
            }
        }
    }

    // HSEngine.pas:4531
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            HexSpriteEngine = message.Payload.As<GameEngine>();
            Initialize();
        }
    }

    // HSEngine.pas:4585
    public virtual MapObject CreateMapObject(sbyte terrainType)
    {
        int i = terrainType;
        DelphiList<MapObjectResource> list = Resources[i];
        int count = list.Count;
        if (count != 0)
        {
            int random = Random(count);
            MapObjectResource r = list.List[random];
            return r.CreateResourceUser();
        }
        else
        {
            return null;
        }
    }

    // HSEngine.pas:4604
    public virtual MapObject Place(MapContainer container, sbyte X, sbyte Y, sbyte L, sbyte terrainType)
    {
        MapObject result = null;
        DelphiList<MapObjectResource> list = Resources[terrainType];
        if (list.Count != 0)
        {
            MapObjectResource r = null;
            MapField mapField = container.GetField(X, Y, L);
            MapObject mapObject = mapField.FindOwnedHexSprite<MapObject>(MapObjectClass);
            if (mapObject is not null)
            {
                r = GetRandomResource(terrainType, mapObject.Resource.EngineClass.ClassId);
                if (r is not null)
                {
                    mapObject.SetResource(r);
                    mapField.UpdateTerrain();
                    result = mapObject;
                }
            }

            if (r is null)
            {
                int random = Random(list.Count);
                r = list.List[random];
                if (r is not null)
                {
                    return r.PlaceResourceUser(container.Map, X, Y, L);
                }
            }
        }

        return result;
    }

    // HSEngine.pas:4637
    public virtual MapObject PlaceClass(MapContainer container, sbyte X, sbyte Y, sbyte L, sbyte terrainType, int classId)
    {
        MapObject result = null;
        if (Resources[terrainType].Count != 0)
        {
            MapObjectResource r = null;
            MapField mapField = container.GetField(X, Y, L);
            MapObject mapObject = mapField.FindOwnedHexSprite<MapObject>(MapObjectClass);
            if (mapObject is not null && mapObject.Resource.EngineClass.ClassId == classId)
            {
                r = GetRandomResource(terrainType, mapObject.Resource.EngineClass.ClassId);
                if (r is not null)
                {
                    mapObject.SetResource(r);
                    mapField.UpdateTerrain();
                    result = mapObject;
                }
            }

            if (r is null)
            {
                r = GetRandomResource(terrainType, classId);
                if (r is not null)
                {
                    return r.PlaceResourceUser(container.Map, X, Y, L);
                }
            }
        }

        return result;
    }

    // HSEngine.pas:4541
    public MapObjectResource GetRandomResource(int terrainType, int classId)
    {
        DelphiList<MapObjectResource> list = Resources[terrainType];
        MapObjectResource result;
        if (list.Count != 0)
        {
            int i = 0;
            result = null;
            while (i != Tries && result is null)
            {
                int random = Random(list.Count);
                result = list.List[random];
                if (result.EngineClass.ClassId != classId)
                {
                    result = null;
                }

                i += 1;
            }

            if (result is null)
            {
                i = 0;
                while (i != list.Count && result is null)
                {
                    result = list.List[i];
                    if (result.EngineClass.ClassId != classId)
                    {
                        result = null;
                    }

                    i += 1;
                }
            }
        }
        else
        {
            return null;
        }

        return result;
    }

    // HSEngine.pas:4572
    public void RegisterResource(MapObjectResource resource)
    {
        sbyte terrainType = resource.TerrainList.GetTerrain(0);
        if (terrainType >= 0 && terrainType < Resources.Count)
        {
            if (Resources[terrainType].IndexOf(resource) != -1 || resource is null)
            {
                throw new InvalidOperationException("Resource double defined (TMORTerrainControl)");
            }

            _ = Resources[terrainType].Add(resource);
        }
    }

    public GameEngine HexSpriteEngine { get; private set; }
    public int Tries { get; } = 5;
    public List<DelphiList<MapObjectResource>> Resources { get; private set; } = [];
    public Type MapObjectClass { get; init; } = typeof(MapObject);

}
