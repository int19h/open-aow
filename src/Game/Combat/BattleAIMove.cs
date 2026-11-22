#nullable disable warnings
// Translated from AoWTC.pas.
using Aow.Game.Draw;

namespace Aow.Game.Combat;

// MoveType. Ability. Spell. Value. GroupValue. UnitHexSprite. Target. X. Y
public class BattleAIMove
{
    public BattleAIMoveType MoveType { get; init; }
    public short Ability { get; init; }
    public short Spell { get; init; }
    public int Value { get; set; }
    public int GroupValue { get; set; }
    public TacticalBattleUnitHexSprite? UnitHexSprite { get; init; }
    public CombatObject? Target { get; init; }
    public int X { get; init; }
    public int Y { get; init; }
}
