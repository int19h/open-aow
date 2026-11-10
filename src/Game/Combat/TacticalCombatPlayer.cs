// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat;

// FAuto. FEndTurn. FIndex. FLastX. FLastY. FLastUnit
public partial class TacticalCombatPlayer : CombatPlayer
{
    public TacticalCombatPlayer(ECustomNode owner) : base(owner)
    { }

    public bool FAuto;
    public bool FEndTurn;
    public sbyte FIndex;
    public int FLastX;
    public int FLastY;
    public TacticalCombatUnit FLastUnit;

    // AoWTC.pas:6419
    public void SetAuto(bool Value)
    {
        FAuto = Value;
        if (AoWCombatMap.Current.FSeatedCombatPlayer == FIndex && AoWCombatMap.Current.FSeatedCombatPlayer == AoWCombatMap.Current.FCurrentCombatPlayer)
        {
            AoWCombatMap.Current.FAI.Reset();
        }
    }

}
