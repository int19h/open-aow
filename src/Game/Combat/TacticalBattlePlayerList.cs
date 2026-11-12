// Translated from AoWTC.pas.

namespace Aow.Game.Combat;

// Players
public partial class TacticalBattlePlayerList
{
    public CombatPlayerList Players;

    // AoWTC.pas:6426
    public int GetCount()
    {
        int result;
        if (Players != null)
        {
            result = Players.GetCount();
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // AoWTC.pas:6431
    public TacticalBattlePlayer GetPlayers(sbyte index) => (TacticalBattlePlayer)Players.GetPlayers(index);

}
