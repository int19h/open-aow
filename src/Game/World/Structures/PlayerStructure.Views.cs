
namespace Aow.Game.World.Structures;

public partial class PlayerStructure
{
    public override StructureView InfoPanel(sbyte Viewer) => StatePanel() ?? (ShownToOwner(Viewer) ? OwnerPanel() : StructureView.OtherPlayer);

    // The panel the owner sees, and every player in debug mode.
    protected virtual StructureView OwnerPanel() => StructureView.Generic;

    protected bool ShownToOwner(sbyte Viewer) => FPlayer == Viewer || Magic.Abilities.GameTables.AoWEngine.FDebug;
}
