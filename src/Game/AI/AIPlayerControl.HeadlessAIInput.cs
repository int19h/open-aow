#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.AI.Groups;
using Aow.Game.Host;
using Aow.Game.World.Map;

namespace Aow.Game.AI;

public sealed partial class AIPlayerControl
{
    // AI.pas:488. The input callback runs at the native debugger boundary.
    public bool Process(AIExecutor executor, bool tokenControlDone)
    {
        HeadlessHooks.BeforeAIPlayerProcess?.Invoke();
        // A directed input that ends the game does it inside the callback, before this test.
        // The turn stops there. The native driver stops inside that call and does not run the
        // rest of Process, and the saves are already written. The stop serves that hook. With
        // the hook clear, a terminated map still takes the busy and ready test and the phase
        // switch.
        if (HeadlessHooks.BeforeAIPlayerProcess is not null
            && GetEngine().StrategicMap.GameStatus == StrategicMapGameStatus.Terminated)
        {
            return false;
        }

        if (Player.GetBusy() || !GetEngine().StrategicMap.TokenManager.PlayerReady(Player.PlayerIndex))
        {
            return false;
        }

        bool done;
        switch (Phase)
        {
            case AIPlayerProcessPhase.Main:
                done = false;
                while (!done && Actions.Count != 0 && !Player.GetBusy() &&
                    GetEngine().StrategicMap.TokenManager.PlayerReady(Player.PlayerIndex))
                {
                    if (MoveExecutor.Status != AIMoveExecutorStatus.Busy)
                    {
                        while (_action is null && Actions.Count != 0)
                        {
                            if (!ActivateAction(0))
                            {
                                Actions.GetActions(0)?.Dispose();
                            }
                        }

                        if (_action is not null)
                        {
                            done = _action.Process();
                            if (!done)
                            {
                                AIPlayerAction action = _action;
                                DeactivateAction();
                                action?.Dispose();
                            }
                        }
                    }
                    else
                    {
                        done = MoveExecutor.Process();
                    }

                    if (!done && executor.ProcessTimeLeft <= 0)
                    {
                        done = true;
                    }
                }
                if (Actions.Count == 0)
                {
                    SetupProcessPhase(AIPlayerProcessPhase.Loop);
                }

                return !Player.GetBusy() && GetEngine().StrategicMap.TokenManager.PlayerReady(Player.PlayerIndex);
            case AIPlayerProcessPhase.Loop:
                done = false;
                int index = 0;
                while (!done && index < Actions.Count && !Player.GetBusy() &&
                    GetEngine().StrategicMap.TokenManager.PlayerReady(Player.PlayerIndex))
                {
                    if (MoveExecutor.Status != AIMoveExecutorStatus.Busy)
                    {
                        while (_action is null && index != Actions.Count)
                        {
                            index++;
                            _actionIndex++;
                            if (_actionIndex >= Actions.Count)
                            {
                                _actionIndex = 0;
                            }

                            if (_actionIndex < Actions.Count)
                            {
                                _ = ActivateAction(_actionIndex);
                            }
                        }
                        if (_action is not null)
                        {
                            done = _action.Process();
                            if (!done)
                            {
                                DeactivateAction();
                            }
                        }
                    }
                    else
                    {
                        done = MoveExecutor.Process();
                    }

                    if (!done && executor.ProcessTimeLeft <= 0)
                    {
                        done = true;
                    }
                }
                return Actions.Count != 0 && !Player.GetBusy() &&
                    GetEngine().StrategicMap.TokenManager.PlayerReady(Player.PlayerIndex);
            case AIPlayerProcessPhase.None:
            default: return false;
        }
    }
}
