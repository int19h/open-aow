
using Aow.Game.Session;

namespace Aow.Game.World.Structures;

public partial class Sign
{
    public override StructureView InfoPanel(PlayerId? viewer) => StatePanel() ?? StructureView.Sign;
}
