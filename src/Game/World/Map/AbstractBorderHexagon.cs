// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

public partial class AbstractBorderHexagon(CustomNode owner) : AbstractHexagon(owner)
{
    public new const int ClassId = 65905;

    public new partial class Resource(CustomNode owner) : AbstractHexagonResource(owner)
    {
    }

}
