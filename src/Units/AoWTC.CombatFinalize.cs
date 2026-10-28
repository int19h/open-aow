using Aow.Units.AoWE;
using Aow.Units.NetworkE;

namespace Aow.Units.AoWTC;

public partial class TTacticalCombat
{
    // AoWTC.pas:5134. The native remote methods access only the common PCS list.
    public static void RestorePlayerConnection(TPlayerCommunicationControl control, int player)
    {
        var settings = control.PCSList.Players[player];
        settings.ConnectionMode = TConnectionMode.cmGlobal;
        settings.SystemID = 0;
    }

    public override void Finalize()
    {
        for (int i = 0; i < FCombatData.FPlayers.GetCount(); i++)
        {
            int player = FCombatData.FPlayers.GetPlayers((sbyte)i).FPlayer;
            if (AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers((sbyte)player).FPlayerType != TPlayerType.ptHuman)
                RestorePlayerConnection(AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC, player);
        }
        if (AoWTC.AoWCombatMap != null)
        {
            AoWTC.AoWCombatMap.FUnitSelection.Clear();
            AoWTC.AoWCombatMap.FSelectEvents.TriggerNotifyEvents(this);
            AoWE.AoWE.AoWHSMap.ReleaseCombatMap();
        }
        base.Finalize();
    }
}
