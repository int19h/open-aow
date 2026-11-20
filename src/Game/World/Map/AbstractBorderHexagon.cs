// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

public class AbstractBorderHexagon(CustomNode owner) : AbstractHexagon(owner)
{
    public new const int ClassId = 65905;

    public new class Resource(CustomNode owner) : AbstractHexagonResource(owner)
    {
    }
}
