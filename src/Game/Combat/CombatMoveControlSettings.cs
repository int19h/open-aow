// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Combat;

// MoveFlags
public partial class CombatMoveControlSettings : MoveControlSettings
{
    public CombatMoveControlSettings(ECustomNode owner) : base(owner)
    { }

    public Set<CombatMoveModeElement> MoveFlags;
    // intf_P5_01.inc:63

}
