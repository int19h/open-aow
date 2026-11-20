// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.Units;

// Race, Time, Relation
public sealed class MigrationItem(CustomNode owner) : EngineObject(owner)
{
    public sbyte Race { get; init; }
    public int Time { get; init; }
    public Status Relation { get; init; }
}
