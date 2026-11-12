// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Map;

// FHSEngine, FTries, FResources, FMOClass. The map object resources of one kind
// (THexagonControl: hexagons, TRoadControl: roads) sorted by terrain type: FResources holds one
// TList of resources per terrain type name of the set (THSSet.TerrainTypes), filled by
// RegisterResource when a resource is activated. Place and PlaceClass put a random resource of
// a terrain type on a map field
public partial class MapObjectResourceTerrainControl : EngineObject
{
    internal GameEngine _hexSpriteEngine;
    internal int _tries;
    internal List<DelphiList> _resources;
    internal Type _mapObjectClass;

    // HSEngine.pas:4522
    internal void Initialize()
    {
        Clear();
        for (int I = 0, last = _hexSpriteEngine._hexSpriteSet.TerrainTypes.Count - 1; I <= last; I++)
        {
            var list = new DelphiList();
            _resources.Add(list);
        }
    }

    // HSEngine.pas:4497
    public override void Dispose()
    {
        Clear();
        _resources?.Clear();
        _resources = null;
        base.Dispose();
    }

    // HSEngine.pas:4505
    public override Set<ControlStyleElement> GetControlStyle() => Set<ControlStyleElement>.Of(EngineServices.Shared);

    // HSEngine.pas:4489
    public MapObjectResourceTerrainControl(ECustomNode owner) : base(owner)
    {
        _resources = new List<DelphiList>();
        _mapObjectClass = typeof(MapObject);
        _tries = 5;
    }

    // HSEngine.pas:4510
    public override void Clear()
    {
        if (_resources != null)
        {
            for (int I = _resources.Count - 1; I >= 0; I--)
            {
                (_resources[I] as IDisposable)?.Dispose();
                _resources.RemoveAt(I);
            }
        }
    }

    // HSEngine.pas:4531
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            _hexSpriteEngine = message.Payload.As<GameEngine>();
            Initialize();
        }
    }

    // HSEngine.pas:4585
    public virtual MapObject CreateMapObject(sbyte terrainType)
    {
        MapObject result;
        int I;
        DelphiList list;
        int count;
        MapObjectResource R;
        I = terrainType;
        list = _resources[I];
        count = list.Count;
        if (count != 0)
        {
            var random = Random(count);
            R = (MapObjectResource)list.List[random];
            result = (MapObject)R.CreateResourceUser();
        }
        else
        {
            result = null;
        }

        return result;
    }

    // HSEngine.pas:4604
    public virtual MapObject Place(MapContainer container, sbyte X, sbyte Y, sbyte L, sbyte terrainType)
    {
        DelphiList list;
        MapObjectResource R;
        MapField mapField;
        MapObject mapObject;
        MapObject result = null;
        list = _resources[terrainType];
        if (list.Count != 0)
        {
            R = null;
            mapField = container.GetField(X, Y, L);
            mapObject = (MapObject)mapField.FindOwnedHexSprite(_mapObjectClass);
            if (mapObject != null)
            {
                R = GetRandomResource(terrainType, mapObject._resource.EngineClass.ClassId);
                if (R != null)
                {
                    mapObject.SetResource(R);
                    mapField.UpdateTerrain();
                    result = mapObject;
                }
            }

            if (R == null)
            {
                var random = Random(list.Count);
                R = (MapObjectResource)list.List[random];
                if (R != null)
                {
                    result = R.PlaceResourceUser(container._map, X, Y, L);
                }
            }
        }

        return result;
    }

    // HSEngine.pas:4637
    public virtual MapObject PlaceClass(MapContainer container, sbyte X, sbyte Y, sbyte L, sbyte terrainType, int classId)
    {
        MapObjectResource R;
        MapField mapField;
        MapObject mapObject;
        MapObject result = null;
        if ((_resources[terrainType]).Count != 0)
        {
            R = null;
            mapField = container.GetField(X, Y, L);
            mapObject = (MapObject)mapField.FindOwnedHexSprite(_mapObjectClass);
            if (mapObject != null && mapObject._resource.EngineClass.ClassId == classId)
            {
                R = GetRandomResource(terrainType, mapObject._resource.EngineClass.ClassId);
                if (R != null)
                {
                    mapObject.SetResource(R);
                    mapField.UpdateTerrain();
                    result = mapObject;
                }
            }

            if (R == null)
            {
                R = GetRandomResource(terrainType, classId);
                if (R != null)
                {
                    result = R.PlaceResourceUser(container._map, X, Y, L);
                }
            }
        }

        return result;
    }

    // HSEngine.pas:4541
    public MapObjectResource GetRandomResource(int terrainType, int classId)
    {
        MapObjectResource result;
        DelphiList list;
        int I;
        list = _resources[terrainType];
        if (list.Count != 0)
        {
            I = 0;
            result = null;
            while (I != _tries && result == null)
            {
                var random = Random(list.Count);
                result = (MapObjectResource)list.List[random];
                if (result.EngineClass.ClassId != classId)
                {
                    result = null;
                }

                I += 1;
            }

            if (result == null)
            {
                I = 0;
                while (I != list.Count && result == null)
                {
                    result = (MapObjectResource)list.List[I];
                    if (result.EngineClass.ClassId != classId)
                    {
                        result = null;
                    }

                    I += 1;
                }
            }
        }
        else
        {
            result = null;
        }

        return result;
    }

    // HSEngine.pas:4572
    public void RegisterResource(MapObjectResource resource)
    {
        sbyte terrainType;
        terrainType = resource._terrainList.GetTerrain(0);
        if (terrainType >= 0 && terrainType < _resources.Count)
        {
            if ((_resources[terrainType]).IndexOf(resource) != -1 || resource == null)
            {
                throw new InvalidOperationException("Resource double defined (TMORTerrainControl)");
            }

            (_resources[terrainType]).Add(resource);
        }
    }

    public GameEngine HexSpriteEngine { get => _hexSpriteEngine; }
    public int Tries { get => _tries; set => _tries = value; }
    public List<DelphiList> Resources { get => _resources; }
    public Type MapObjectClass { get => _mapObjectClass; set => _mapObjectClass = value; }

}
