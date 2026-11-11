// Translated from HSEngine.pas.
using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

// A map level owns its fields in row order. The fields are its children.
// Field size and neighbour offsets keep the recovered layout values.
public partial class MapLevel : ECustomNode
{
    // HSEngine.pas:2837
    public new const int ClassID = 65794;

    public MapLevel(ECustomNode owner) : base(owner)
    {
        FMapFieldCID = 65793;
    }

    protected internal int FLhx;
    protected internal int FWidth;
    protected internal int FHeight;
    protected internal int FCount;
    protected internal MapContainer FMapContainer;
    protected internal HSMap FMap;
    protected internal GameEngine FEngine;
    protected internal int FMapFieldCID;
    protected internal int FFieldSize;
    protected internal int FFieldsSize;
    public Int32Array8By2 FHNOffsets;

    // HSEngine.pas:2925
    public override GameEngine GetEngine() => FEngine;

    // HSEngine.pas:2913
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        if (Msg.MsgID == 536870920)
        {
            FEngine = Msg.Payload.As<GameEngine>();
            FMapContainer = (MapContainer)Owner;
            FMap = FMapContainer.FMap;
        }
        else
        {
            if (Msg.MsgID == 536936471)
            {
                DestroyHS();
            }
            else
            {
                base.MsgProc(ref Msg, Sender);
            }
        }
    }

    // HSEngine.pas:2935
    public override void SetChild(int Index, EngineObject Child)
    {
        throw new InvalidOperationException("Cannot set child in TMapLevel");
    }

    // HSEngine.pas:2950
    public override int GetCount() => FCount;

    // HSEngine.pas:2945
    public override int RemoveChild(EngineObject Child)
    {
        throw new InvalidOperationException("Cannot remove from TMapLevel");
    }

    // HSEngine.pas:2940
    public override int AddChild(EngineObject Child)
    {
        throw new InvalidOperationException("Cannot add child to TMapLevel");
    }

    // HSEngine.pas:3140
    public virtual void New(NewHexSpriteMapSettings Settings)
    {
        Clear();
        try
        {
            CreateMapFieldList(Settings.Width, Settings.Height);
        }
        catch
        {
            Clear();
            throw;
        }
    }

    // HSEngine.pas:2842
    public virtual EngineClass MapFieldClass() => EngineClass.Of<MapField>();

    public int Lhx { get => FLhx; set => FLhx = value; }
    public int Width { get => FWidth; }
    public int Height { get => FHeight; }
    public MapContainer MapContainer { get => FMapContainer; }
    public HSMap Map { get => FMap; }
    public int MapFieldCID { get => FMapFieldCID; set => FMapFieldCID = value; }
    public int FieldSize { get => FFieldSize; }
    public ReadOnlySpan<MapField> Fields => fields;

    public sealed class FieldAccessor(MapLevel target)
    {
        public MapField this[int X, int Y] { get => target.GetField(X, Y); }
    }

    public FieldAccessor Field => new FieldAccessor(this);

}
