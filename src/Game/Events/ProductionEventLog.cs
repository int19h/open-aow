// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Events;

public partial class ProductionEventLog : LocationTextEventLog
{
    public ProductionEventLog(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131833;

}
