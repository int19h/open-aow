
using Aow.Game.Net;
using Aow.Game.Session;

namespace Aow.Game.Combat;

public partial class TacticalBattle
{
    // AoWTC.pas:5134. The native remote methods access only the common PCS list.
    public static void RestorePlayerConnection(PlayerCommunicationControl control, int player)
    {
        var settings = control.PlayerCommunicationSettingsList.Players[player];
        settings.ConnectionMode = ConnectionMode.Global;
        settings.SystemId = 0;
    }

    public override void Finalize()
    {
        for (int i = 0; i < CombatData.Players.GetCount(); i++)
        {
            int player = CombatData.Players.GetPlayers((sbyte)i).Player;
            if (World.Map.AoWHSMap.Current.Players.GetPlayers((sbyte)player).PlayerType != PlayerType.Human)
                RestorePlayerConnection(World.Map.AoWHSMap.Current.PlayerCommunicator.PlayerCommunicationControl, player);
        }
        if (CombatMap.Current != null)
        {
            CombatMap.Current.UnitSelection.Clear();
            CombatMap.Current.SelectEventsStore.Raise(this);
            World.Map.AoWHSMap.Current.ReleaseCombatMap();
        }
        base.Finalize();
    }
}
