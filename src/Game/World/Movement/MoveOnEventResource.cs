// Translated from MoveOnEvents.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Movement;

public partial class MoveOnEventResource : MapObjectResource
{
    public MoveOnEventResource(ECustomNode owner) : base(owner)
    {
        Level = 27;
    }

    // MoveOnEvents.pas:261

    // MoveOnEvents.pas:267
    public override void SetLevel(byte Value)
    {
        base.SetLevel(27);
    }

    // MoveOnEvents.pas:48

    // MoveOnEvents.pas:272
    public override string ResourceEditName() => "MoveOnEvent";

}
