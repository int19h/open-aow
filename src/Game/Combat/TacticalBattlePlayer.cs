// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat;

// Auto. EndTurn. Index. LastX. LastY. LastUnit
public partial class TacticalBattlePlayer : CombatPlayer
{
    public TacticalBattlePlayer(ECustomNode owner) : base(owner)
    { }

    public bool Auto;
    public bool EndTurn;
    public sbyte Index;
    public int LastX;
    public int LastY;
    public TacticalBattleUnit LastUnit;

    // AoWTC.pas:6419
    public void SetAuto(bool value)
    {
        Auto = value;
        if (CombatMap.Current.SeatedCombatPlayer == Index && CombatMap.Current.SeatedCombatPlayer == CombatMap.Current.CurrentCombatPlayer)
        {
            CombatMap.Current.AI.Reset();
        }
    }

}
