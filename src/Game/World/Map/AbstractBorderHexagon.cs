// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

public partial class AbstractBorderHexagon : AbstractHexagon
{
    public new const int ClassId = 65905;

    public AbstractBorderHexagon(ECustomNode owner) : base(owner)
    { }

    public new partial class Resource : AbstractHexagonResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

    }

}
