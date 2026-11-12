
using Aow.Game.AI.Groups;
using Aow.Game.Host;

using Aow.Game.World.Map;

namespace Aow.Game.AI;

public partial class AIPlayerControl
{
    // AI.pas:488. The input callback runs at the native debugger boundary.
    public bool Process(AIExecutor executor, bool tokenControlDone)
    {
        HeadlessHooks.BeforeAIPlayerProcess?.Invoke();
        if (Player.GetBusy() || !AoWHSMap.Current.TokenManager.PlayerReady(Player.PlayerIndex)) return false;
        bool done;
        switch (Phase)
        {
            case AIPlayerProcessPhase.Main:
                done = false;
                while (!done && Actions.Count != 0 && !Player.GetBusy() &&
                    AoWHSMap.Current.TokenManager.PlayerReady(Player.PlayerIndex))
                {
                    if (MoveExecutor.Status != AIMoveExecutorStatus.Busy)
                    {
                        while (Action is null && Actions.Count != 0)
                            if (!ActivateAction(0)) Actions.GetActions(0)?.Dispose();
                        if (Action is not null)
                        {
                            done = Action.Process();
                            if (!done)
                            {
                                var action = Action;
                                DeactivateAction();
                                action?.Dispose();
                            }
                        }
                    }
                    else done = MoveExecutor.Process();
                    if (!done && executor.GetProcessTimeLeft() <= 0) done = true;
                }
                if (Actions.Count == 0) SetupProcessPhase(AIPlayerProcessPhase.Loop);
                return !Player.GetBusy() && AoWHSMap.Current.TokenManager.PlayerReady(Player.PlayerIndex);
            case AIPlayerProcessPhase.Loop:
                done = false;
                int index = 0;
                while (!done && index < Actions.Count && !Player.GetBusy() &&
                    AoWHSMap.Current.TokenManager.PlayerReady(Player.PlayerIndex))
                {
                    if (MoveExecutor.Status != AIMoveExecutorStatus.Busy)
                    {
                        while (Action is null && index != Actions.Count)
                        {
                            index++;
                            ActionIndex++;
                            if (ActionIndex >= Actions.Count) ActionIndex = 0;
                            if (ActionIndex < Actions.Count) ActivateAction(ActionIndex);
                        }
                        if (Action is not null)
                        {
                            done = Action.Process();
                            if (!done) DeactivateAction();
                        }
                    }
                    else done = MoveExecutor.Process();
                    if (!done && executor.GetProcessTimeLeft() <= 0) done = true;
                }
                return Actions.Count != 0 && !Player.GetBusy() &&
                    AoWHSMap.Current.TokenManager.PlayerReady(Player.PlayerIndex);
            default: return false;
        }
    }
}
