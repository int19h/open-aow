#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.Session;

namespace Aow.Game.World.Structures;

public partial class PlayerStructure
{
    public override StructureView InfoPanel(PlayerId? viewer) => StatePanel() ?? (ShownToOwner(viewer) ? OwnerPanel() : StructureView.OtherPlayer);

    // The panel the owner sees, and every player in debug mode.
    protected virtual StructureView OwnerPanel() => StructureView.Generic;

    protected bool ShownToOwner(PlayerId? viewer) => Player == viewer || GetEngine().Debug;
}
