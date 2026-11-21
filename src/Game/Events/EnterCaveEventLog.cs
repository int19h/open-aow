#nullable disable warnings
// Translated from Cave.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.Events;

// Cave
public class EnterCaveEventLog(CustomNode? owner, EngineObject holder) : MessageEventLog(owner, holder)
{
    public Cave Cave { get; init; }

    // Cave.pas:442
    public override bool Yes()
    {
        _ = Cave.Enter();
        return true;
    }

    // Cave.pas:448
    public override bool No() => true;

}
