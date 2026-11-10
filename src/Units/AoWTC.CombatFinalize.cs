using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;

namespace Aow.Game.Combat;

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
            if (AoWEUnit.AoWHSMap.FPlayers.GetPlayers((sbyte)player).FPlayerType != TPlayerType.ptHuman)
                RestorePlayerConnection(AoWEUnit.AoWHSMap.FPlayerCommunicator.PCC, player);
        }
        if (AoWTCUnit.AoWCombatMap != null)
        {
            AoWTCUnit.AoWCombatMap.FUnitSelection.Clear();
            AoWTCUnit.AoWCombatMap.FSelectEventsStore.Raise(this);
            AoWEUnit.AoWHSMap.ReleaseCombatMap();
        }
        base.Finalize();
    }
}
