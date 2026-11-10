// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

namespace Aow.Game.Events;

// combat-move token
public partial class CombatRestoreHotseatEventLog : EventLog
{
    public CombatRestoreHotseatEventLog(ECustomNode owner) : base(owner)
    { }

    public ArmyCombatMoveTE FCombatMove;

    // MoveT.PAS:2494
    public override bool Execute(bool Mode, ExecuteTrigger Trigger, ExecuteDoneEvent Done)
    {
        FCombatMove.FCombatMode = ArmyCombatMoveTE.CombatMoveMode.Moving;
        FCombatMove.RestoreHotseatMode();
        return false;
    }
}
