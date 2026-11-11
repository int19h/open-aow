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
    internal GameEngine FHSEngine;
    internal int FTries;
    internal List<DelphiList> FResources;
    internal Type FMOClass;

    // HSEngine.pas:4522
    internal void Initialize()
    {
        Clear();
        for (int I = 0, last = FHSEngine.FHSSet.TerrainTypes.Count - 1; I <= last; I++)
        {
            var list = new DelphiList();
            FResources.Add(list);
        }
    }

    // HSEngine.pas:4497
    public override void Dispose()
    {
        Clear();
        FResources?.Clear();
        FResources = null;
        base.Dispose();
    }

    // HSEngine.pas:4505
    public override Set<ControlStyleElement> GetControlStyle() => Set<ControlStyleElement>.Of(EngineServices.csShared);

    // HSEngine.pas:4489
    public MapObjectResourceTerrainControl(ECustomNode owner) : base(owner)
    {
        FResources = new List<DelphiList>();
        FMOClass = typeof(MapObject);
        FTries = 5;
    }

    // HSEngine.pas:4510
    public override void Clear()
    {
        if (FResources != null)
        {
            for (int I = FResources.Count - 1; I >= 0; I--)
            {
                (FResources[I] as IDisposable)?.Dispose();
                FResources.RemoveAt(I);
            }
        }
    }

    // HSEngine.pas:4531
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            FHSEngine = Msg.Payload.As<GameEngine>();
            Initialize();
        }
    }

    // HSEngine.pas:4585
    public virtual MapObject CreateMO(sbyte TerrainType)
    {
        MapObject Result;
        int I;
        DelphiList List;
        int Count;
        MapObjectResource R;
        I = TerrainType;
        List = FResources[I];
        Count = List.Count;
        if (Count != 0)
        {
            var random = Random(Count);
            R = (MapObjectResource)List.List[random];
            Result = (MapObject)R.CreateResourceUser();
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // HSEngine.pas:4604
    public virtual MapObject Place(MapContainer Container, sbyte X, sbyte Y, sbyte L, sbyte TerrainType)
    {
        DelphiList List;
        MapObjectResource R;
        MapField MapF;
        MapObject MO;
        MapObject Result = null;
        List = FResources[TerrainType];
        if (List.Count != 0)
        {
            R = null;
            MapF = Container.GetField(X, Y, L);
            MO = (MapObject)MapF.FindOwnedHS(FMOClass);
            if (MO != null)
            {
                R = GetRndResource(TerrainType, MO.FResource.EClass.ClassID);
                if (R != null)
                {
                    MO.SetResource(R);
                    MapF.UpdateTerrain();
                    Result = MO;
                }
            }

            if (R == null)
            {
                var random = Random(List.Count);
                R = (MapObjectResource)List.List[random];
                if (R != null)
                {
                    Result = R.PlaceResourceUser(Container.FMap, X, Y, L);
                }
            }
        }

        return Result;
    }

    // HSEngine.pas:4637
    public virtual MapObject PlaceClass(MapContainer Container, sbyte X, sbyte Y, sbyte L, sbyte TerrainType, int AClassID)
    {
        MapObjectResource R;
        MapField MapF;
        MapObject MO;
        MapObject Result = null;
        if ((FResources[TerrainType]).Count != 0)
        {
            R = null;
            MapF = Container.GetField(X, Y, L);
            MO = (MapObject)MapF.FindOwnedHS(FMOClass);
            if (MO != null && MO.FResource.EClass.ClassID == AClassID)
            {
                R = GetRndResource(TerrainType, MO.FResource.EClass.ClassID);
                if (R != null)
                {
                    MO.SetResource(R);
                    MapF.UpdateTerrain();
                    Result = MO;
                }
            }

            if (R == null)
            {
                R = GetRndResource(TerrainType, AClassID);
                if (R != null)
                {
                    Result = R.PlaceResourceUser(Container.FMap, X, Y, L);
                }
            }
        }

        return Result;
    }

    // HSEngine.pas:4541
    public MapObjectResource GetRndResource(int TerrainType, int AClassID)
    {
        MapObjectResource Result;
        DelphiList List;
        int I;
        List = FResources[TerrainType];
        if (List.Count != 0)
        {
            I = 0;
            Result = null;
            while (I != FTries && Result == null)
            {
                var random = Random(List.Count);
                Result = (MapObjectResource)List.List[random];
                if (Result.EClass.ClassID != AClassID)
                {
                    Result = null;
                }

                I += 1;
            }

            if (Result == null)
            {
                I = 0;
                while (I != List.Count && Result == null)
                {
                    Result = (MapObjectResource)List.List[I];
                    if (Result.EClass.ClassID != AClassID)
                    {
                        Result = null;
                    }

                    I += 1;
                }
            }
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // HSEngine.pas:4572
    public void RegisterResource(MapObjectResource Resource)
    {
        sbyte TerrainType;
        TerrainType = Resource.FTerrainList.GetTerrain(0);
        if (TerrainType >= 0 && TerrainType < FResources.Count)
        {
            if ((FResources[TerrainType]).IndexOf(Resource) != -1 || Resource == null)
            {
                throw new InvalidOperationException("Resource double defined (TMORTerrainControl)");
            }

            (FResources[TerrainType]).Add(Resource);
        }
    }

    public GameEngine HSEngine { get => FHSEngine; }
    public int Tries { get => FTries; set => FTries = value; }
    public List<DelphiList> Resources { get => FResources; }
    public Type MOClass { get => FMOClass; set => FMOClass = value; }

}
