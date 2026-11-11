// Translated from AoWTC.pas.
using Aow.Game.Draw;

namespace Aow.Game.Combat;

// MoveType. Ability. Spell. Value. GroupValue. UnitHexSprite. Target. X. Y
public partial class BattleAIMove
{
    public BattleAIMoveType MoveType;
    public short Ability;
    public short Spell;
    public int Value;
    public int GroupValue;
    public TacticalBattleUnitHexSprite UnitHS;
    public CombatObject Target;
    public ushort X;
    public ushort Y;
}
