#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Structures;

namespace Aow.Game.Session;

// { Interface declaration run owned by P1. }
public sealed class PlayerStructureList(CustomNode owner) : ObservableEventList(owner), IChildList<PlayerStructure>
{
    // The structure in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a player structure. Another class throws, the same way the cast did.
    public PlayerStructure this[int index] => GetChild<PlayerStructure>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is PlayerStructure;

    int IChildSink<PlayerStructure>.AddChild(PlayerStructure child) => AddChild(child);

    public Player Player { get; init; }

    // Player.PAS:1
    public PlayerStructure GetStructure(int index) => this[index];
}
