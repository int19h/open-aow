// Translated from HSEngine.pas and AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

// A map level owns its fields in row order. The fields are its children.
// Field size and neighbour offsets keep the recovered layout values.
public partial class MapLevel : ECustomNode, IOwned<MapContainer>, IOwnedSink<MapContainer>
{
    // The container the level belongs to. AcceptsOwner admits only a container, so the cast never
    // fails.
    public new MapContainer Owner { get => (MapContainer)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is MapContainer;

    void IOwnedSink<MapContainer>.SetOwner(MapContainer owner) => SetOwner(owner);

    // AoWHSMap.PAS (TAoWMapLevel). HSEngine.pas:2837 gives the engine level 65794.
    public new const int ClassId = 131091;

    public MapLevel(ECustomNode owner) : base(owner)
    {
        _mapFieldClassId = 65793;
    }

    private int _hexLevel;
    private int _width;
    private int _height;
    private int _count;
    private MapContainer _mapContainer;
    private GameMap _map;

    protected internal GameEngine Engine { get; private set; }

    private int _mapFieldClassId;

    // The index offset in the field array of each hex direction, per row parity. HSEngine.pas
    // stores byte offsets into its raw allocation, the managed array counts fields.
    public int[][] HexNumberOffsets { get; } = [new int[8], new int[8]];

    // HSEngine.pas:2925. Before the activation message the level reaches the engine as any node does.
    public override GameEngine GetEngine() => Engine ?? base.GetEngine();

    // HSEngine.pas:2913
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        if (message.MessageId == 536870920)
        {
            Engine = message.Payload.As<GameEngine>();
            _mapContainer = Owner;
            _map = _mapContainer.Map;
        }
        else
        {
            if (message.MessageId == 536936471)
            {
                DestroyHexSprite();
            }
            else
            {
                base.MessageProc(ref message, sender);
            }
        }
    }

    // HSEngine.pas:2935
    public override void SetChild(int index, EngineObject child)
    {
        throw new InvalidOperationException("Cannot set child in TMapLevel");
    }

    // HSEngine.pas:2950
    public override int GetCount() => _count;

    // HSEngine.pas:2945
    public override int RemoveChild(EngineObject child)
    {
        throw new InvalidOperationException("Cannot remove from TMapLevel");
    }

    // HSEngine.pas:2940
    public override int AddChild(EngineObject child)
    {
        throw new InvalidOperationException("Cannot add child to TMapLevel");
    }

    // HSEngine.pas:3140
    public virtual void New(NewHexSpriteMapSettings settings)
    {
        Clear();
        try
        {
            CreateMapFieldList(settings.Width, settings.Height);
        }
        catch
        {
            Clear();
            throw;
        }
    }

    // AoWHSMap.PAS:231. HSEngine.pas:2842 names the engine field class, which the fold made the same class.
    public virtual EngineClass MapFieldClass() => EngineClass.Of<MapField>();

    public int HexLevel { get => _hexLevel; set => _hexLevel = value; }
    public int Width { get => _width; }
    public int Height { get => _height; }
    public MapContainer MapContainer { get => _mapContainer; }
    public GameMap Map { get => _map; }
    public int MapFieldClassId { get => _mapFieldClassId; set => _mapFieldClassId = value; }
    public ReadOnlySpan<MapField> Fields => _fields;

    public sealed class FieldAccessor(MapLevel target)
    {
        public MapField this[int X, int Y] { get => target.GetField(X, Y); }
    }

    public FieldAccessor Field => new FieldAccessor(this);
}
