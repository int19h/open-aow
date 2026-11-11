// Translated from AoWTC.pas.

namespace Aow.Game.Combat;

// FPlayers
public partial class TacticalBattlePlayerList
{
    public CombatPlayerList FPlayers;

    // AoWTC.pas:6426
    public int GetCount()
    {
        int Result;
        if (FPlayers != null)
        {
            Result = FPlayers.GetCount();
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWTC.pas:6431
    public TacticalBattlePlayer GetPlayers(sbyte Index) => (TacticalBattlePlayer)FPlayers.GetPlayers(Index);

}
