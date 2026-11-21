#nullable disable
// Translated from Engine.pas.
using Aow.Formats.Tree;
using Aow.Game;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

// FEngine, FControlStyle. A node that knows its engine: message $20000008
// (TEngine.ActivateEObject) carries the engine in the message
public class EngineNode : OwningNode
{
    // Engine.pas:8257
    public new const int ClassId = 259;

    // Engine.pas:8250
    public EngineNode(CustomNode owner) : this(null, owner) { }

    private protected EngineNode(ConstructionContext context, CustomNode owner) : base(context, owner)
    {
        SetOwnerWithoutNotify(owner);
        _controlStyle = base.GetControlStyle();
    }

    private GameEngine _engine;
    private ControlStyles _controlStyle;

    // Engine.pas:8262. Before the activation message the node reaches the engine as any node does.
    public override GameEngine GetEngine() => _engine ?? base.GetEngine();

    // Engine.pas:8267
    public void SetEngine(GameEngine engine) => _engine = engine;

    // Engine.pas:8272
    public override ControlStyles GetControlStyle() => _controlStyle;

    // Engine.pas:8277
    public void SetControlStyle(ControlStyles value) => _controlStyle = value;

    // Engine.pas:8282
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _controlStyle = persister.Set("ControlStyle", value: _controlStyle);
    }

    // Engine.pas:8288
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        if (message.MessageId == 536870920)
        {
            _engine = message.Payload.As<GameEngine>();
        }

        base.MessageProc(ref message, sender);
    }
}
