using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.World.Map;

public partial class MapLevel
{
    private MapField[] _fields = [];

    public override EngineObject GetChild(int index) => _fields[index];
    public MapField GetField(int x, int y) => _fields[y * _width + x];

    // HSEngine.pas:2960 stores adjacent instances in a raw allocation.
    // Managed instances retain the same row order and virtual construction.
    public void CreateMapFieldList(int width, int height)
    {
        DestroyMapFieldList();
        var type = MapFieldClass();
        _width = width;
        _height = height;
        _count = width * height;
        _fieldSize = type.InstanceSize;
        FieldsSize = _fieldSize * _count;
        _fields = new MapField[_count];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
            {
                var field = (MapField)type.CreateInstance(this);
                field.HexX = (sbyte)x;
                field.HexY = (sbyte)y;
                field.HexLevel = (sbyte)_hexLevel;
                _fields[y * width + x] = field;
            }
        HexNumberOffsets = default;
        for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
            for (int parity = 0; parity <= 1; parity++)
                HexNumberOffsets[parity][hexNumber] = (HexGeometry.s_hexNumberToXYTable[parity][hexNumber].X
                    + HexGeometry.s_hexNumberToXYTable[parity][hexNumber].Y * width) * _fieldSize;

    }

    public void DestroyMapFieldList()
    {
        _fields = [];
        FieldsSize = 0;
    }

    public override void Dispose()
    {
        foreach (var field in _fields) field?.Dispose();
        _count = 0;
        DestroyMapFieldList();
        base.Dispose();
    }

    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        var mode = stream.StorageMode();
        if ((mode & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Text))
            == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            Engine = stream.Engine;
            _mapContainer = Owner;
            _map = _mapContainer.Map;
        }
        byte width = (byte)_width, height = (byte)_height, level = (byte)_hexLevel;
        stream.rwByte(ref width, 3);
        stream.rwByte(ref height, 4);
        stream.rwByte(ref level, 5);
        _width = width;
        _height = height;
        _hexLevel = level;
        MapProgress progress = new()
        {
            Level = _hexLevel,
            Percent = 0,
            ProgressType = mode.Contains(StorageModeElement.Read) ? MapProgressType.Loading : MapProgressType.Saving
        };
        bool notify = !mode.Contains(StorageModeElement.Translate);
        if (notify) _map.NotifyProgress(progress);
        if ((mode & Set<StorageModeElement>.Of(StorageModeElement.Read, EngineServices.Text, StorageModeElement.Compare))
            == Set<StorageModeElement>.Of(StorageModeElement.Read) && width != 0)
            CreateMapFieldList(width, height);
        if (mode.Contains(StorageModeElement.SetupCompare))
            foreach (var field in _fields) field.Status += Set<MapFieldStatusElement>.Of(MapFieldStatusElement.Comparing);
        var property = stream.OpenPropertyStream(6);
        if (property != null || mode.Contains(EngineServices.Text))
        {
            int step = _count / 10;
            for (int i = 0; i < _count; i++)
            {
                stream.rwEObjectStream(_fields[i], i + 100);
                if (--step == 0)
                {
                    progress.Percent += 10;
                    if (notify) _map.NotifyProgress(progress);
                    step = _count / 10;
                }
            }
        }
    }

    private void VisitFields(MapProgressType type, int divisor, int increment, Action<MapField> action, bool final)
    {
        MapProgress progress = new() { Level = _hexLevel, Percent = 0, ProgressType = type };
        _map.NotifyProgress(progress);
        int step = _count / divisor;
        foreach (var field in _fields)
        {
            action(field);
            if (--step == 0)
            {
                progress.Percent += increment;
                _map.NotifyProgress(progress);
                step = _count / divisor;
            }
        }
        if (final) { progress.Percent = 100; _map.NotifyProgress(progress); }
    }

    public void DestroyHexSprite() => VisitFields(MapProgressType.Destroying, 10, 10, f => f.DestroyHexSprite(), false);
    public void UpdateTerrain() => VisitFields(MapProgressType.UpdatingTerrain, 5, 20, f => f.Initialize(), true);
    public void Loaded() => VisitFields(MapProgressType.Initializing, 5, 20, f => f.Loaded(), true);
    public void Activate() => VisitFields(MapProgressType.Activating, 5, 20, f => f.Activate(), true);
}
