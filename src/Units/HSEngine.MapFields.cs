using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Rtl;

namespace Aow.Game.World.Map;

public partial class MapLevel
{
    private MapField[] fields = [];

    public override EObject GetChild(int index) => fields[index];
    public MapField GetField(int x, int y) => fields[y * FWidth + x];

    // HSEngine.pas:2960 stores adjacent instances in a raw allocation.
    // Managed instances retain the same row order and virtual construction.
    public void CreateMapFieldList(int width, int height)
    {
        DestroyMapFieldList();
        var type = MapFieldClass();
        FWidth = width;
        FHeight = height;
        FCount = width * height;
        FFieldSize = type.InstanceSize;
        FFieldsSize = FFieldSize * FCount;
        fields = new MapField[FCount];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var field = (MapField)type.Create(this);
                field.FXhx = (sbyte)x;
                field.FYhx = (sbyte)y;
                field.FLhx = (sbyte)FLhx;
                fields[y * width + x] = field;
            }
        FHNOffsets = default;
        for (int hn = 1; hn <= 6; hn++)
            for (int parity = 0; parity <= 1; parity++)
                FHNOffsets[parity][hn] = (HexGeometry.HNtoXYTable[parity][hn].X
                    + HexGeometry.HNtoXYTable[parity][hn].Y * width) * FFieldSize;

    }

    public void DestroyMapFieldList()
    {
        fields = [];
        FFieldsSize = 0;
    }

    public override void Dispose()
    {
        foreach (var field in fields) field?.Dispose();
        FCount = 0;
        DestroyMapFieldList();
        base.Dispose();
    }

    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        var mode = stream.StorageMode();
        if ((mode & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText))
            == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            FEngine = stream.Engine;
            FMapContainer = (MapContainer)Owner;
            FMap = FMapContainer.FMap;
        }
        byte width = (byte)FWidth, height = (byte)FHeight, level = (byte)FLhx;
        stream.rwByte(ref width, 3);
        stream.rwByte(ref height, 4);
        stream.rwByte(ref level, 5);
        FWidth = width;
        FHeight = height;
        FLhx = level;
        MapProgress progress = new()
        {
            Level = FLhx,
            Percent = 0,
            ProgressType = mode.Contains(StorageModeElement.Read) ? MapProgressType.Loading : MapProgressType.Saving
        };
        bool notify = !mode.Contains(StorageModeElement.Translate);
        if (notify) FMap.NotifyProgress(progress);
        if ((mode & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.smText, StorageModeElement.Compare))
            == Set<StorageModeElement>.Of(StorageModeElement.Read) && width != 0)
            CreateMapFieldList(width, height);
        if (mode.Contains(StorageModeElement.SetupCompare))
            foreach (var field in fields) field.FStatus += Set<MapFieldStatusElement>.Of(MapFieldStatusElement.Comparing);
        var property = stream.OpenPropertyStream(6);
        if (property != null || mode.Contains(EngineServices.smText))
        {
            int step = FCount / 10;
            for (int i = 0; i < FCount; i++)
            {
                stream.rwEObjectStream(fields[i], i + 100);
                if (--step == 0)
                {
                    progress.Percent += 10;
                    if (notify) FMap.NotifyProgress(progress);
                    step = FCount / 10;
                }
            }
        }
    }

    private void VisitFields(MapProgressType type, int divisor, int increment, Action<MapField> action, bool final)
    {
        MapProgress progress = new() { Level = FLhx, Percent = 0, ProgressType = type };
        FMap.NotifyProgress(progress);
        int step = FCount / divisor;
        foreach (var field in fields)
        {
            action(field);
            if (--step == 0)
            {
                progress.Percent += increment;
                FMap.NotifyProgress(progress);
                step = FCount / divisor;
            }
        }
        if (final) { progress.Percent = 100; FMap.NotifyProgress(progress); }
    }

    public void DestroyHS() => VisitFields(MapProgressType.Destroying, 10, 10, f => f.DestroyHS(), false);
    public void UpdateTerrain() => VisitFields(MapProgressType.UpdatingTerrain, 5, 20, f => f.Initialize(), true);
    public void Loaded() => VisitFields(MapProgressType.Initializing, 5, 20, f => f.Loaded(), true);
    public void Activate() => VisitFields(MapProgressType.Activating, 5, 20, f => f.Activate(), true);
}

public partial class MapContainer
{
    public MapField GetField(int x, int y, int level) => GetMapLevel(level).GetField(x, y);
    public MapField GetFieldXYL(int location) => GetField((sbyte)location, (sbyte)(location >> 8), (sbyte)(location >> 16));

    public void RemoveHS(int x, int y, int level, HexagonSprite sprite)
    {
        if (GetMapLevel(level).GetField(x, y).RemoveHS(sprite) != -1 && sprite.GetLevel() <= 15)
            FMap.InvalidateStaticScene(x, y, level);
    }

    public void InsertHS(int x, int y, int level, HexagonSprite sprite, int hn)
    {
        GetMapLevel(level).GetField(x, y).AddHS(sprite, hn);
        if (sprite.GetLevel() <= 15) FMap.InvalidateStaticScene(x, y, level);
    }
}
