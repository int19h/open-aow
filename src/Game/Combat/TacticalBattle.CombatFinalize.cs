#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.Net;
using Aow.Game.Session;

namespace Aow.Game.Combat;

public sealed partial class TacticalBattle
{
    // AoWTC.pas:5134. The native remote methods access only the common PCS list.
    public static void RestorePlayerConnection(PlayerCommunicationControl control, int player)
    {
        PlayerCommunicationSettings settings = control.PlayerCommunicationSettingsList.Players[player];
        settings.ConnectionMode = ConnectionMode.Global;
        settings.SystemId = 0;
    }

    public override void Finalize()
    {
        // A battle that reaches no engine has no world map and no battle map, so it takes the
        // original's no-map branches (issue 335, G5 guard rule).
        GameEngine? engine = GetEngine();
        World.Map.StrategicMap? strategicMap = engine?.StrategicMap;
        if (strategicMap is not null)
        {
            for (int i = 0; i < CombatData.Players.GetCount(); i++)
            {
                int player = CombatData.Players.GetPlayers((sbyte)i).Player;
                if (strategicMap.Players.GetPlayers((sbyte)player).PlayerType != PlayerType.Human)
                {
                    RestorePlayerConnection(strategicMap.PlayerCommunicator.PlayerCommunicationControl, player);
                }
            }
        }

        CombatMap? combatMap = engine?.CombatMap;
        if (combatMap is not null)
        {
            combatMap.UnitSelection.Clear();
            combatMap.OnBattleEnding(this);
            engine.StrategicMap.ReleaseCombatMap();
        }

        base.Finalize();
    }
}
