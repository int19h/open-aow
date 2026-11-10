// Translated from AoWTC.pas.

namespace Aow.Game.Combat;

// FPlayers
public partial class TacticalCombatPlayerList
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
    public TacticalCombatPlayer GetPlayers(sbyte Index) => (TacticalCombatPlayer)FPlayers.GetPlayers(Index);

}
