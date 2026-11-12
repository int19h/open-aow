using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

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
        foreach (var offsets in HexNumberOffsets) Array.Clear(offsets);
        for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
            for (int parity = 0; parity <= 1; parity++)
                HexNumberOffsets[parity][hexNumber] = (HexGeometry.HexNumberToXYTable[parity][hexNumber].X
                    + HexGeometry.HexNumberToXYTable[parity][hexNumber].Y * width);

    }

    public void DestroyMapFieldList()
    {
        _fields = [];
    }

    public override void Dispose()
    {
        foreach (var field in _fields) field?.Dispose();
        _count = 0;
        DestroyMapFieldList();
        base.Dispose();
    }

    public override void ReadWrite(StorageStream stream)
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
        stream.Byte(ref width, 3);
        stream.Byte(ref height, 4);
        stream.Byte(ref level, 5);
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
        var property = stream.OpenProperty(6);
        if (property != null || mode.Contains(EngineServices.Text))
        {
            int step = _count / 10;
            for (int i = 0; i < _count; i++)
            {
                stream.SizedObject(_fields[i], i + 100);
                if (--step == 0)
                {
                    progress.Percent += 10;
                    if (notify) _map.NotifyProgress(progress);
                    step = _count / 10;
                }
            }
        }
    }

    // The tree form of ReadWrite. The fields are a grid member, read only when present, written always.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        bool loading = persister.Loading;
        if (loading)
        {
            Engine = PersistSupport.Engine(persister);
            _mapContainer = Owner;
            _map = _mapContainer.Map;
        }
        _width = persister.Byte("Width", (byte)_width);
        _height = persister.Byte("Height", (byte)_height);
        _hexLevel = persister.Byte("HexLevel", (byte)_hexLevel);
        MapProgress progress = new()
        {
            Level = _hexLevel,
            Percent = 0,
            ProgressType = loading ? MapProgressType.Loading : MapProgressType.Saving,
        };
        _map.NotifyProgress(progress);
        if (loading && _width != 0)
            CreateMapFieldList(_width, _height);
        if (loading && !persister.Has("Fields"))
        {
            return;
        }

        int step = _count / 10;
        persister.Grid("Fields", width: _width, height: _height, fields: _fields, visited: _ =>
        {
            if (--step == 0)
            {
                progress.Percent += 10;
                _map.NotifyProgress(progress);
                step = _count / 10;
            }
        });
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
