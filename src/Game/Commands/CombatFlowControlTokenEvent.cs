#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Combat;
using Aow.Game.Net;
using Aow.Game.World.Map;

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
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (Mode.Contains(CombatFlowControlModes.SetAutoSwitch))
        {
            strategicMap.TokenManager.TokenExecutor.HoldTacticalBattle = false;
        }

        if (Mode.Contains(CombatFlowControlModes.LockAIExecutor))
        {
            strategicMap.AIExecutor.Lock();
        }

        if (Mode.Contains(CombatFlowControlModes.UnlockAIExecutor))
        {
            strategicMap.AIExecutor.Unlock();
        }

        if (Mode.Contains(CombatFlowControlModes.ActivateTacticalBattle))
        {
            strategicMap.TokenManager.TokenExecutor.ActivateTacticalBattle(Player);
        }

        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        if (Mode.Contains(CombatFlowControlModes.EnableTurnTimer))
        {
            engineStrategicMap.EnableTurnTimer();
        }

        if (Mode.Contains(CombatFlowControlModes.DisableTurnTimer))
        {
            engineStrategicMap.DisableTurnTimer();
        }

        return ExecuteResult.Done;
    }
}
