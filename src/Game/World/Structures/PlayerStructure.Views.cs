
namespace Aow.Game.World.Structures;

public partial class PlayerStructure
{
    public override StructureView InfoPanel(sbyte viewer) => StatePanel() ?? (ShownToOwner(viewer) ? OwnerPanel() : StructureView.OtherPlayer);

    // The panel the owner sees, and every player in debug mode.
    protected virtual StructureView OwnerPanel() => StructureView.Generic;

    protected bool ShownToOwner(sbyte viewer) => Player == viewer || Magic.Abilities.GameTables.GameEngine.Debug;
}
