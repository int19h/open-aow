#nullable disable
// Translated from Cave.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.Events;

// Cave
public partial class EnterCaveEventLog(CustomNode owner, EngineObject holder) : MessageEventLog(owner, holder)
{
    public Cave Cave { get; set; }

    // Cave.pas:442
    public override bool Yes()
    {
        _ = Cave.Enter();
        return true;
    }

    // Cave.pas:448
    public override bool No() => true;

}
