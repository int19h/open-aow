// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Combat;

using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// flow mode, player
public partial class CombatFlowControlTokenEvent : TokenEvent
{
    public CombatFlowControlTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135448;

    public Set<CombatFlowControlModeElement> Mode;
    public int Player;

    // MoveT.PAS:3596
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        RwSet(ref Mode, 1, 30, stream.rwData, 0);
        stream.rwInteger(ref Player, 31);
    }

    // MoveT.PAS:3603
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        if (Mode.Contains(CombatFlowControlModeElement.SetAutoSwitch))
        {
            StrategicMap.Current.TokenManager.TokenExecutor.HoldTacticalBattle = false;
        }

        if (Mode.Contains(CombatFlowControlModeElement.LockAIExecutor))
        {
            StrategicMap.Current.AIExecutor.Lock();
        }

        if (Mode.Contains(CombatFlowControlModeElement.UnlockAIExecutor))
        {
            StrategicMap.Current.AIExecutor.Unlock();
        }

        if (Mode.Contains(CombatFlowControlModeElement.ActivateTacticalBattle))
        {
            StrategicMap.Current.TokenManager.TokenExecutor.ActivateTacticalBattle(Player);
        }

        if (Mode.Contains(CombatFlowControlModeElement.EnableTurnTimer))
        {
            StrategicMap.Current.EnableTurnTimer();
        }

        if (Mode.Contains(CombatFlowControlModeElement.DisableTurnTimer))
        {
            StrategicMap.Current.DisableTurnTimer();
        }

        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
