using Aow.Game.Combat;

namespace Aow.Game.World.Structures;

public partial class Mine
{
    protected override StructureView OwnerPanel() => StructureView.Mine;

    public override string CombatMapName(CombatMapSite Site) => "Minex000";
}
