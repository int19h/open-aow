
namespace Aow.Game.World.Structures;

public partial class Arena
{
    public override StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? StructureView.Arena;

    public override StructureView InfoButton(sbyte Viewer)
    {
        Enter();
        return StructureView.None;
    }

    public override StructureView DoubleClick(sbyte Viewer)
    {
        Enter();
        return StructureView.None;
    }
}
