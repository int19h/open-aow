// Translated from Cave.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.Events;

// FCave
public partial class EnterCaveEventLog : MessageEventLog
{
    public EnterCaveEventLog(ECustomNode owner) : base(owner)
    { }

    public Cave FCave;

    // Cave.pas:442
    public override bool Yes()
    {
        FCave.EnterEx();
        return true;
    }

    // Cave.pas:448
    public override bool No() => true;

}
