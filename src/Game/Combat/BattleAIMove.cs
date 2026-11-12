// Translated from AoWTC.pas.
using Aow.Game.Draw;

namespace Aow.Game.Combat;

// MoveType. Ability. Spell. Value. GroupValue. UnitHexSprite. Target. X. Y
public partial class BattleAIMove
{
    public BattleAIMoveType MoveType { get; set; }
    public short Ability { get; set; }
    public short Spell { get; set; }
    public int Value { get; set; }
    public int GroupValue { get; set; }
    public TacticalBattleUnitHexSprite UnitHexSprite { get; set; }
    public CombatObject Target { get; set; }
    public ushort X { get; set; }
    public ushort Y { get; set; }
}
