
using Aow.Game.Magic;
using Aow.Game.Session;

namespace Aow.Game.World.Structures;

public partial class ProductionPlace
{
    protected override StructureView OwnerPanel() => GetMagicSphere() == MagicSphere.None ? StructureView.Production : StructureView.ProductionIncome;

    public override StructureView InfoButton(PlayerId? viewer) => StructureView.ProductionScreen;

    public override StructureView DoubleClick(PlayerId? viewer) => CanDoubleClick() && Player == viewer ? StructureView.ProductionScreen : StructureView.None;
}
