using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Farm
{
    protected override StructureView OwnerPanel() => StructureView.Farm;

    public override string CombatMapName(CombatMapSite site) => "Farmx000";
}
