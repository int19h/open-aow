// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.Units;

// Race, Time, Relation
public partial class MigrationItem : EngineObject
{
    public MigrationItem(ECustomNode owner) : base(owner)
    { }

    public sbyte Race;
    public int Time;
    public Status Relation;
}
