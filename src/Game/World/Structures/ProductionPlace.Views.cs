
using Aow.Game.Magic;

namespace Aow.Game.World.Structures;

public partial class ProductionPlace
{
    protected override StructureView OwnerPanel() => GetMagicSphere() == MagicSphere.None ? StructureView.Production : StructureView.ProductionIncome;

    public override StructureView InfoButton(sbyte viewer) => StructureView.ProductionScreen;

    public override StructureView DoubleClick(sbyte viewer) => CanDoubleClick() && Player == viewer ? StructureView.ProductionScreen : StructureView.None;
}
