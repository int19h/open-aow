using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Tower
{
    protected override StructureView OwnerPanel() => StructureView.WatchTower;

    public override string CombatMapName(CombatMapSite site) => "TFort000";
}
