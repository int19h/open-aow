
namespace Aow.Game.World.Structures;

public partial class WizardsTower
{
    public override StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? StructureView.WizardsTower;

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
