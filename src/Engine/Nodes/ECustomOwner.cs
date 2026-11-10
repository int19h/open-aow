// Translated from Engine.pas.
using Aow.Engine.Storage;

using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Nodes;

// FEngine, FControlStyle. A node that knows its engine: message $20000008
// (TEngine.ActivateEObject) carries the engine in the message
public partial class ECustomOwner : ENode
{
    // Engine.pas:8257
    public new const int ClassID = 259;

    // Engine.pas:8250
    public ECustomOwner(ECustomNode owner) : this(null, owner) { }

    private protected ECustomOwner(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        FOwner = owner;
        FControlStyle = base.GetControlStyle();
    }

    internal GameEngine FEngine;
    internal Set<EControlStyleElement> FControlStyle;

    // Engine.pas:8262
    public override GameEngine GetEngine() => FEngine;

    // Engine.pas:8267
    public void SetEngine(GameEngine AEngine)
    {
        FEngine = AEngine;
    }

    // Engine.pas:8272
    public override Set<EControlStyleElement> GetControlStyle() => FControlStyle;

    // Engine.pas:8277
    public void SetControlStyle(Set<EControlStyleElement> Value)
    {
        FControlStyle = Value;
    }

    // Engine.pas:8282
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        RwSet(ref FControlStyle, 1, 2, Stream.rwData, 0);
    }

    // Engine.pas:8288
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        if (Msg.MsgID == 536870920)
        {
            FEngine = Msg.Payload.As<GameEngine>();
        }

        base.MsgProc(ref Msg, Sender);
    }

    // Engine.pas:855
}
