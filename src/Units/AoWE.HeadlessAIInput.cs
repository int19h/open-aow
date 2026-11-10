using Aow.Game.Ai.Groups;
using Aow.Game.Engine;
using Aow.Game.Magic.Abilities;

namespace Aow.Game.Ai;

public partial class AIPlayerControl
{
    // AI.pas:488. The input callback runs at the native debugger boundary.
    public bool Process(AIExecuter executer, bool tokenControlDone)
    {
        HeadlessHooks.BeforeAIPlayerProcess?.Invoke();
        if (FPlayer.GetBusy() || !Aow.Game.World.Map.AoWHSMap.Current.FTokenManager.PlayerReady(FPlayer.FPlayerIndex)) return false;
        bool done;
        switch (FPhase)
        {
            case AIPlayerProcessPhase.Main:
                done = false;
                while (!done && FActions.Count != 0 && !FPlayer.GetBusy() &&
                    Aow.Game.World.Map.AoWHSMap.Current.FTokenManager.PlayerReady(FPlayer.FPlayerIndex))
                {
                    if (FMoveExecuter.FStatus != Aow.Game.Ai.Groups.AIMoveExecuter.Status.Busy)
                    {
                        while (FAction is null && FActions.Count != 0)
                            if (!ActivateAction(0)) FActions.GetActions(0)?.Dispose();
                        if (FAction is not null)
                        {
                            done = FAction.Process();
                            if (!done)
                            {
                                var action = FAction;
                                DeactivateAction();
                                action?.Dispose();
                            }
                        }
                    }
                    else done = FMoveExecuter.Process();
                    if (!done && executer.GetProcessTimeLeft() <= 0) done = true;
                }
                if (FActions.Count == 0) SetupProcessPhase(AIPlayerProcessPhase.Loop);
                return !FPlayer.GetBusy() && Aow.Game.World.Map.AoWHSMap.Current.FTokenManager.PlayerReady(FPlayer.FPlayerIndex);
            case AIPlayerProcessPhase.Loop:
                done = false;
                int index = 0;
                while (!done && index < FActions.Count && !FPlayer.GetBusy() &&
                    Aow.Game.World.Map.AoWHSMap.Current.FTokenManager.PlayerReady(FPlayer.FPlayerIndex))
                {
                    if (FMoveExecuter.FStatus != Aow.Game.Ai.Groups.AIMoveExecuter.Status.Busy)
                    {
                        while (FAction is null && index != FActions.Count)
                        {
                            index++;
                            FActionIndex++;
                            if (FActionIndex >= FActions.Count) FActionIndex = 0;
                            if (FActionIndex < FActions.Count) ActivateAction(FActionIndex);
                        }
                        if (FAction is not null)
                        {
                            done = FAction.Process();
                            if (!done) DeactivateAction();
                        }
                    }
                    else done = FMoveExecuter.Process();
                    if (!done && executer.GetProcessTimeLeft() <= 0) done = true;
                }
                return FActions.Count != 0 && !FPlayer.GetBusy() &&
                    Aow.Game.World.Map.AoWHSMap.Current.FTokenManager.PlayerReady(FPlayer.FPlayerIndex);
            default: return false;
        }
    }
}
