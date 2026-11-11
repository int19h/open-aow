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

    public new const int ClassID = 135448;

    public Set<CombatFlowControlModeElement> FMode;
    public int FPlayer;

    // MoveT.PAS:3596
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        RwSet(ref FMode, 1, 30, Stream.rwData, 0);
        Stream.rwInteger(ref FPlayer, 31);
    }

    // MoveT.PAS:3603
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        if (FMode.Contains(CombatFlowControlModeElement.SetAutoSwitch))
        {
            AoWHSMap.Current.FTokenManager.TokenExecuter.HoldTC = false;
        }

        if (FMode.Contains(CombatFlowControlModeElement.LockAIExecuter))
        {
            AoWHSMap.Current.FAIExecuter.Lock();
        }

        if (FMode.Contains(CombatFlowControlModeElement.UnlockAIExecuter))
        {
            AoWHSMap.Current.FAIExecuter.Unlock();
        }

        if (FMode.Contains(CombatFlowControlModeElement.ActivateTC))
        {
            AoWHSMap.Current.FTokenManager.TokenExecuter.ActivateTC(FPlayer);
        }

        if (FMode.Contains(CombatFlowControlModeElement.EnableTurnTimer))
        {
            AoWHSMap.Current.EnableTurnTimer();
        }

        if (FMode.Contains(CombatFlowControlModeElement.DisableTurnTimer))
        {
            AoWHSMap.Current.DisableTurnTimer();
        }

        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
