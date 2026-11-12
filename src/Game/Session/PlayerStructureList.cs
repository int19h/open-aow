// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.Session;

// { Interface declaration run owned by P1. }
public partial class PlayerStructureList : EChangeNotifyEventList, IChildList<PlayerStructure>
{
    // The structure in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a structure, so the cast never fails.
    public PlayerStructure this[int index] => (PlayerStructure)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is PlayerStructure;

    int IChildSink<PlayerStructure>.AddChild(PlayerStructure child) => AddChild(child);

    public PlayerStructureList(ECustomNode owner) : base(owner)
    { }

    public Player Player { get; set; }

    // Player.PAS:1
    public PlayerStructure GetStructure(int index) => this[index];

    // Engine.pas:708

}
