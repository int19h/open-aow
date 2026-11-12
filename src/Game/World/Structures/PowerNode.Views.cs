using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class PowerNode
{
    protected override StructureView OwnerPanel() => StructureView.PowerNode;

    public override string CombatMapName(CombatMapSite site) => "NodeC000";
}
