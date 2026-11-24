
using Aow.Game.Session;

namespace Aow.Game.World.Structures;

public partial class WizardsTower
{
    public override StructureView InfoPanel(PlayerId? viewer) => StatePanel() ?? StructureView.WizardsTower;

    public override StructureView InfoButton(PlayerId? viewer)
    {
        _ = Enter();
        return StructureView.None;
    }

    public override StructureView DoubleClick(PlayerId? viewer)
    {
        _ = Enter();
        return StructureView.None;
    }
}
