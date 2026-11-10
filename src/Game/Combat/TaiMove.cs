// Translated from AoWTC.pas.
using Aow.Game.Draw;

namespace Aow.Game.Combat;

// MoveType. Ability. Spell. Value. GroupValue. UnitHS. Target. X. Y
public partial class TaiMove
{
    public TaiMoveType MoveType;
    public short Ability;
    public short Spell;
    public int Value;
    public int GroupValue;
    public TacticalCombatUnitHS UnitHS;
    public CombatObject Target;
    public ushort X;
    public ushort Y;
}
