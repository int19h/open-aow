using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Game.World.Map;

public partial class MapLevel
{
    private MapField[] _fields = [];

    public override EngineObject GetChild(int index) => _fields[index];
    public MapField GetField(int x, int y) => _fields[(y * Width) + x];

    // HSEngine.pas:2960 stores adjacent instances in a raw allocation.
    // Managed instances retain the same row order and virtual construction.
    public void CreateMapFieldList(int width, int height)
    {
        DestroyMapFieldList();
        EngineClass type = MapFieldClass();
        Width = width;
        Height = height;
        _count = width * height;
        _fields = new MapField[_count];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                MapField field = type.Create<MapField>(this, holder: CreationHolder());
                field.HexX = x.TruncateToSByte();
                field.HexY = y.TruncateToSByte();
                field.HexLevel = HexLevel.TruncateToSByte();
                _fields[(y * width) + x] = field;
            }
        }

        foreach (int[]? offsets in HexNumberOffsets)
        {
            Array.Clear(offsets);
        }

        for (int hexNumber = 1; hexNumber <= 6; hexNumber++)
        {
            for (int parity = 0; parity <= 1; parity++)
            {
                HexNumberOffsets[parity][hexNumber] = HexGeometry.HexNumberToXYTable[parity][hexNumber].X
                    + (HexGeometry.HexNumberToXYTable[parity][hexNumber].Y * width);
            }
        }
    }

    public void DestroyMapFieldList() => _fields = [];

    public override void Dispose()
    {
        foreach (MapField field in _fields)
        {
            field?.Dispose();
        }

        _count = 0;
        DestroyMapFieldList();
        base.Dispose();
    }

    // The fields are a grid member, read only when present, written always, where the original
    // ReadWrite stores them as sized objects at ids 100 and on.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        bool loading = persister.Loading;
        if (loading)
        {
            Engine = persister.Engine();
            MapContainer = Owner;
            Map = MapContainer.Map;
        }
        Width = persister.Byte("Width", (byte)Width);
        Height = persister.Byte("Height", (byte)Height);
        HexLevel = persister.Byte("HexLevel", (byte)HexLevel);
        MapProgress progress = new()
        {
            Level = HexLevel,
            Percent = 0,
            ProgressType = loading ? MapProgressType.Loading : MapProgressType.Saving,
        };
        // The translate stream notifies no progress, as the original's test of Translate says.
        bool notify = !persister.Translating();
        if (notify)
        {
            Map.NotifyProgress(progress);
        }

        if (loading && Width != 0)
        {
            CreateMapFieldList(Width, Height);
        }

        if (loading && !persister.Has("Fields"))
        {
            return;
        }

        int step = _count / 10;
        persister.Grid("Fields", width: Width, height: Height, fields: _fields, visited: _ =>
        {
            if (--step == 0)
            {
                progress.Percent += 10;
                if (notify)
                {
                    Map.NotifyProgress(progress);
                }

                step = _count / 10;
            }
        });
    }

    private void VisitFields(MapProgressType type, int divisor, int increment, Action<MapField> action, bool final)
    {
        MapProgress progress = new() { Level = HexLevel, Percent = 0, ProgressType = type };
        Map.NotifyProgress(progress);
        int step = _count / divisor;
        foreach (MapField field in _fields)
        {
            action(field);
            if (--step == 0)
            {
                progress.Percent += increment;
                Map.NotifyProgress(progress);
                step = _count / divisor;
            }
        }
        if (final)
        {
            progress.Percent = 100;
            Map.NotifyProgress(progress);
        }
    }

    public void DestroyHexSprite() => VisitFields(MapProgressType.Destroying, divisor: 10, increment: 10, action: f => f.DestroyHexSprite(), final: false);
    public void UpdateTerrain() => VisitFields(MapProgressType.UpdatingTerrain, divisor: 5, increment: 20, action: f => f.Initialize(), final: true);
    public void Loaded() => VisitFields(MapProgressType.Initializing, divisor: 5, increment: 20, action: f => f.Loaded(), final: true);
    public void Activate() => VisitFields(MapProgressType.Activating, divisor: 5, increment: 20, action: f => f.Activate(), final: true);
}
