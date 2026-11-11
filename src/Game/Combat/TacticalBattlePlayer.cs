// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat;

// FAuto. FEndTurn. FIndex. FLastX. FLastY. FLastUnit
public partial class TacticalBattlePlayer : CombatPlayer
{
    public TacticalBattlePlayer(ECustomNode owner) : base(owner)
    { }

    public bool FAuto;
    public bool FEndTurn;
    public sbyte FIndex;
    public int FLastX;
    public int FLastY;
    public TacticalBattleUnit FLastUnit;

    // AoWTC.pas:6419
    public void SetAuto(bool Value)
    {
        FAuto = Value;
        if (CombatMap.Current.FSeatedCombatPlayer == FIndex && CombatMap.Current.FSeatedCombatPlayer == CombatMap.Current.FCurrentCombatPlayer)
        {
            CombatMap.Current.FAI.Reset();
        }
    }

}
