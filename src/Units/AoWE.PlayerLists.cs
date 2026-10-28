using Aow.Units.Engine;

namespace Aow.Units.AoWE;

public partial class TPlayerList
{
    public void Update()
    {
        if (AoWE.AoWHSMap.FPlayerCommunicator.PCC is null) return;
        var locals = Rtl.Rtl.ReferenceFromId<TByteList>(F_PlayerList_14);
        locals.Clear();
        FLocalHumanPlayers.Clear();
        FLocalPlayers.Clear();
        FActivePlayers.Clear();
        for (int i = 0; i < FChildren.Count; i++)
        {
            var player = (TPlayer)FChildren.List[i];
            if (player.FStatus != TPlayerStatus.psPlaying) continue;
            if (player.FGameOverStatus != TGameOverStatus.goLost && i != 0) FActivePlayers.Add((byte)i);
            if (player.GetLocal())
            {
                locals.Add((byte)i);
                if (player.FPlayerType == TPlayerType.ptHuman) FLocalHumanPlayers.Add((byte)i);
            }
            if (player.FPlayerType == TPlayerType.ptHuman) FLocalPlayers.Add((byte)i);
        }
    }
}
