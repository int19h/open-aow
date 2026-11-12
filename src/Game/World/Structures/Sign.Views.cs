
namespace Aow.Game.World.Structures;

public partial class Sign
{
    public override StructureView InfoPanel(sbyte viewer) => StatePanel() ?? StructureView.Sign;
}
