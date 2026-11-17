
namespace Aow.Game.World.Structures;

public partial class WizardsTower
{
    public override StructureView InfoPanel(sbyte viewer) => StatePanel() ?? StructureView.WizardsTower;

    public override StructureView InfoButton(sbyte viewer)
    {
        _ = Enter();
        return StructureView.None;
    }

    public override StructureView DoubleClick(sbyte viewer)
    {
        _ = Enter();
        return StructureView.None;
    }
}
