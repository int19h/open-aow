using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;

namespace Aow.Game.Combat;

public partial class TacticalBattle
{
    // AoWTC.pas:5134. The native remote methods access only the common PCS list.
    public static void RestorePlayerConnection(PlayerCommunicationControl control, int player)
    {
        var settings = control.PCSList.Players[player];
        settings.ConnectionMode = ConnectionMode.Global;
        settings.SystemID = 0;
    }

    public override void Finalize()
    {
        for (int i = 0; i < FCombatData.FPlayers.GetCount(); i++)
        {
            int player = FCombatData.FPlayers.GetPlayers((sbyte)i).FPlayer;
            if (World.Map.AoWHSMap.Current.FPlayers.GetPlayers((sbyte)player).FPlayerType != PlayerType.Human)
                RestorePlayerConnection(World.Map.AoWHSMap.Current.FPlayerCommunicator.PCC, player);
        }
        if (AoWCombatMap.Current != null)
        {
            AoWCombatMap.Current.FUnitSelection.Clear();
            AoWCombatMap.Current.FSelectEventsStore.Raise(this);
            World.Map.AoWHSMap.Current.ReleaseCombatMap();
        }
        base.Finalize();
    }
}
