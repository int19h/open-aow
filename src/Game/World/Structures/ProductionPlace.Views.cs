
using Aow.Game.Magic;

namespace Aow.Game.World.Structures;

public partial class ProductionPlace
{
    protected override StructureView OwnerPanel() => GetMagicSphere() == MagicSphere.None ? StructureView.Production : StructureView.ProductionIncome;

    public override StructureView InfoButton(sbyte Viewer) => StructureView.ProductionScreen;

    public override StructureView DoubleClick(sbyte Viewer) => CanDblClick() && FPlayer == Viewer ? StructureView.ProductionScreen : StructureView.None;
}
