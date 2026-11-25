#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Combat;
using Aow.Game.Net;

namespace Aow.Game.Commands;

// flow mode, player
public class CombatFlowControlTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = 135_448;

    public CombatFlowControlModes Mode { get; set; }
    public int Player { get; set; }

    // MoveT.PAS:3596
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Mode = persister.Set("Mode", value: Mode);
        Player = persister.Int32("Player", value: Player);
    }

    // MoveT.PAS:3603
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        if (Mode.Contains(CombatFlowControlModes.SetAutoSwitch))
        {
            GetEngine().StrategicMap.TokenManager.TokenExecutor.HoldTacticalBattle = false;
        }

        if (Mode.Contains(CombatFlowControlModes.LockAIExecutor))
        {
            GetEngine().StrategicMap.AIExecutor.Lock();
        }

        if (Mode.Contains(CombatFlowControlModes.UnlockAIExecutor))
        {
            GetEngine().StrategicMap.AIExecutor.Unlock();
        }

        if (Mode.Contains(CombatFlowControlModes.ActivateTacticalBattle))
        {
            GetEngine().StrategicMap.TokenManager.TokenExecutor.ActivateTacticalBattle(Player);
        }

        if (Mode.Contains(CombatFlowControlModes.EnableTurnTimer))
        {
            GetEngine().StrategicMap.EnableTurnTimer();
        }

        if (Mode.Contains(CombatFlowControlModes.DisableTurnTimer))
        {
            GetEngine().StrategicMap.DisableTurnTimer();
        }

        return ExecuteResult.Done;
    }
}
