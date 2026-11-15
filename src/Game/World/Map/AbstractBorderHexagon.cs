// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

public partial class AbstractBorderHexagon : AbstractHexagon
{
    public new const int ClassId = 65905;

    public AbstractBorderHexagon(CustomNode owner) : base(owner)
    { }

    public new partial class Resource : AbstractHexagonResource
    {
        public Resource(CustomNode owner) : base(owner)
        { }

    }

}
