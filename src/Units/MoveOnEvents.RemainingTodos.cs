using Aow.Units.AoWE;
using Aow.Units.HSEngine;

namespace Aow.Units.MoveOnEvents;

public partial class TMoveOnEvent
{
    public virtual void ArmyPlaced(object army)
    {
        sbyte player = ((TArmyHS)army).FArmy.FPlayer;
        if (!GetTriggerPlayer(player)) return;
        MovedOnTrigger(player);
        if (FTriggerPlayers == 0 && !AoWE.AoWE.AoWHSMap.Status.Contains(THSMapStatusElement.msEditting)) Destroy();
    }
}
