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
        for (int i = 0; i < CombatData.Players.GetCount(); i++)
        {
            int player = CombatData.Players.GetPlayers((sbyte)i).Player;
            if (World.Map.StrategicMap.Current.Players.GetPlayers((sbyte)player).PlayerType != PlayerType.Human)
            {
                RestorePlayerConnection(World.Map.StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl, player);
            }
        }
        if (CombatMap.Current is not null)
        {
            CombatMap.Current.UnitSelection.Clear();
            CombatMap.Current.OnBattleEnding(this);
            World.Map.StrategicMap.Current.ReleaseCombatMap();
        }
        base.Finalize();
    }
}
