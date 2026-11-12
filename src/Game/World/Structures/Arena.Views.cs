
namespace Aow.Game.World.Structures;

public partial class Arena
{
    public override StructureView InfoPanel(sbyte viewer) => StatePanel() ?? StructureView.Arena;

    public override StructureView InfoButton(sbyte viewer)
    {
        Enter();
        return StructureView.None;
    }

    public override StructureView DoubleClick(sbyte viewer)
    {
        Enter();
        return StructureView.None;
    }
}
