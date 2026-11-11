// Translated from City.pas.
using Aow.Engine.Nodes;

using Aow.Game.Session;

namespace Aow.Game.Units;

// FRace, FTime, FRelation
public partial class MigrationItem : EngineObject
{
    public MigrationItem(ECustomNode owner) : base(owner)
    { }

    public sbyte FRace;
    public int FTime;
    public Status FRelation;
}
