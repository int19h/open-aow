// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Movement;

namespace Aow.Game.Combat;

// MoveFlags
public sealed class CombatMoveControlSettings(CustomNode owner) : MoveControlSettings(owner)
{
    public CombatMoveModes MoveFlags { get; set; }
}
