// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Events;

public class ProductionEventLog(CustomNode owner, EngineObject holder) : LocationTextEventLog(owner, holder: holder)
{
    public new const int ClassId = 131_833;

}
