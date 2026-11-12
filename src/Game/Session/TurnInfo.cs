// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Session;

public partial class TurnInfo : ENode, IChildList<TurnInfoItem>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a TurnInfoItem, so the cast never fails.
    public TurnInfoItem this[int index] => (TurnInfoItem)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is TurnInfoItem;

    int IChildSink<TurnInfoItem>.AddChild(TurnInfoItem child) => AddChild(child);

    public TurnInfo(ECustomNode owner) : base(owner)
    { }

    // Player.PAS:177
    public TurnInfoItem GetTurnInfo(int index) => this[index];

    // Engine.pas:631

}
