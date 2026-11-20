#nullable disable
// Translated from AoWTC.pas.

namespace Aow.Game.Combat;

// Players
public partial class TacticalBattlePlayerList
{
    public CombatPlayerList Players { get; set; }

    // AoWTC.pas:6426
    public int GetCount()
    {
        int result = Players is not null ? Players.GetCount() : 0;
        return result;
    }

    // AoWTC.pas:6431
    public TacticalBattlePlayer GetPlayers(sbyte index) => Players.GetPlayers<TacticalBattlePlayer>(index);

}
