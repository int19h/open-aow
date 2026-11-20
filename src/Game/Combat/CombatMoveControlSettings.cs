// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Combat;

// MoveFlags
public class CombatMoveControlSettings(CustomNode owner) : MoveControlSettings(owner)
{
    public Set<CombatMoveModeElement> MoveFlags { get; set; }
}
