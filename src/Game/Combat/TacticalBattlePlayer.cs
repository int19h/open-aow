#nullable disable
// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat;

// Auto. EndTurn. Index. LastX. LastY. LastUnit
public class TacticalBattlePlayer(CustomNode owner) : CombatPlayer(owner)
{
    public bool Auto { get; private set; }
    public bool EndTurn { get; }

    private readonly sbyte _index;

    public int LastX { get; set; }
    public int LastY { get; set; }
    public TacticalBattleUnit LastUnit { get; set; }

    // AoWTC.pas:6419
    public void SetAuto(bool value)
    {
        Auto = value;
        if (CombatMap.Current.SeatedCombatPlayer == _index && CombatMap.Current.SeatedCombatPlayer == CombatMap.Current.CurrentCombatPlayer)
        {
            CombatMap.Current.AI.Reset();
        }
    }
}
