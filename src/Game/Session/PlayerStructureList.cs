// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.Session;

// { Interface declaration run owned by P1. }
public partial class PlayerStructureList : EChangeNotifyEventList
{
    public PlayerStructureList(ECustomNode owner) : base(owner)
    { }

    public Player FPlayer;

    // Player.PAS:1
    public PlayerStructure GetStructure(int Index) => (PlayerStructure)FChildren.List[Index];

    // Engine.pas:708

}
