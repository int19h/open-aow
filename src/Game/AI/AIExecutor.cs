// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiAssert;
using static Aow.Rtl.LegacyApplication;

namespace Aow.Game.AI;

// cycles, players, current player, player cycles, status, token done, time mode, time, used
// time, start, token cycles, token cycle, lock count
public partial class AIExecutor(CustomNode owner) : EngineObject(owner)
{

    public int ProcessCycles { get; set; }

    private readonly ByteList _players = new();

    public sbyte Player { get; private set; } = -1;

    private int _playerProcessCycles;

    public Set<AIExecutorStatusElement> Status { get; private set; }

    private bool _tokenControlDone;
    private readonly AIExecutorProcessTimeMode _processTimeMode = AIExecutorProcessTimeMode.Static;
    private readonly int _processTime = 5000;
    private int _processTimeUsed;
    private int _processStarted;
    private int _tokenCycles = 5;
    private int _tokenCycle;

    public int LockCount { get; private set; }

    // AI.pas:569
    public override void Dispose()
    {
        _players?.Dispose();
        base.Dispose();
    }

    // AI.pas:575
    public bool RequestProcessTime(int time)
    {
        switch (_processTimeMode)
        {
            case AIExecutorProcessTimeMode.Static:
                int used = _processTimeUsed;
                return used == 0 || time <= _processTime - used;
            case AIExecutorProcessTimeMode.Dynamic:
                return _processTimeUsed == 0 || time < GetProcessTimeLeft();
            default:
                return false;
        }
    }

    // AI.pas:593
    public int GetProcessTimeLeft()
    {
        switch (_processTimeMode)
        {
            case AIExecutorProcessTimeMode.Static:
                return _processTime - _processTimeUsed;
            case AIExecutorProcessTimeMode.Dynamic:
                // { Original behavior: the millisecond delta is multiplied by 1000. }
                int elapsedMilliseconds = PixelFormats.GetElapsedMilliseconds();
                return _processTime - ((elapsedMilliseconds - _processStarted) * 1000);
            default:
                return -1;
        }
    }

    // AI.pas:603
    public int GetProcessTimeElapsed()
    {
        return _processTimeMode switch
        {
            AIExecutorProcessTimeMode.Static => _processTimeUsed,
            AIExecutorProcessTimeMode.Dynamic => (PixelFormats.GetElapsedMilliseconds() - _processStarted) * 1000,
            _ => -1,
        };
    }

    // AI.pas:617
    public bool ActivatePlayer(sbyte player)
    {
        if (_players.IndexOf((byte)player) == -1)
        {
            _ = _players.Add((byte)player);
            Status += Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Enabled);
            StrategicMap.Current.Players.GetPlayers(player).AIControl.Activate(this);
            if (Player == -1)
            {
                Player = player;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    // AI.pas:630
    public void DeactivatePlayer(sbyte player)
    {
        if (_players.IndexOf((byte)player) != -1)
        {
            _ = _players.Remove((byte)player);
            StrategicMap.Current.Players.GetPlayers(player).AIControl.Deactivate();
            if (_players.Count == 0)
            {
                Player = -1;
                Status -= Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Enabled);
            }
        }
    }

    // AI.pas:644
    public void TokenControlDoneEvent(object sender)
    {
        if (Status.Contains(AIExecutorStatusElement.Enabled) && StrategicMap.Current.PlayMode == PlayMode.Simultaneous)
        {
            _tokenControlDone = true;
            if (StrategicMap.Current.CurrentPlayer == 0 && StrategicMap.Current.CombatMap is null)
            {
                CaptureTokenCycles(5);
            }
        }
    }

    // AI.pas:654
    public void Lock() => LockCount += 1;

    // AI.pas:659
    public void Unlock()
    {
        Assert(LockCount != 0, "", "AI.pas", 931);
        LockCount -= 1;
    }

    // AI.pas:935
    public void CaptureTokenCycles(int cycles)
    {
        Assert(!Status.Contains(AIExecutorStatusElement.TokenLocked), "", "AI.pas", 937);
        Status += Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.TokenLocked);
        StrategicMap.Current.TokenManager.TokenExecutor.Lock();
        _tokenCycle = 0;
        _tokenCycles = cycles;
    }

    // AI.pas:944
    public void TokenCyclesCaptured()
    {
        Assert(Status.Contains(AIExecutorStatusElement.TokenLocked), "", "AI.pas", 947);
        Status -= Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.TokenLocked);
        StrategicMap.Current.TokenManager.TokenExecutor.Unlock();
    }

    // AI.pas:952
    public void Process()
    {
        if (Status.Contains(AIExecutorStatusElement.Enabled) && LockCount == 0)
        {
            Status += Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Executing);
            StrategicMap.Current.SynchronizeBegin();
            _processTimeUsed = 0;
            _processStarted = _processTimeMode == AIExecutorProcessTimeMode.Dynamic ? PixelFormats.GetElapsedMilliseconds() : 0;

            try
            {
                switch (StrategicMap.Current.PlayMode)
                {
                    case PlayMode.Turn:
                        if (!StrategicMap.Current.Players.GetPlayers(Player).GetBusy() && StrategicMap.Current.TokenManager.PlayerReady(Player))
                        {
                            ProcessCycles += 1;
                            _ = StrategicMap.Current.Players.GetPlayers(Player).AIControl.Process(this, _tokenControlDone);
                            _tokenControlDone = false;
                        }

                        break;
                    case PlayMode.Simultaneous:
                        if (Status.Contains(AIExecutorStatusElement.TokenLocked))
                        {
                            ProcessCycles += 1;
                            bool done = false;
                            int i = 0;
                            while (!done && GetProcessTimeLeft() > 0 && Status.Contains(AIExecutorStatusElement.Enabled) && i < _players.Count)
                            {
                                done = StrategicMap.Current.Players.GetPlayers(Player).AIControl.Process(this, _tokenControlDone);
                                if (done)
                                {
                                    _playerProcessCycles += 1;
                                    i = 0;
                                }
                                else
                                {
                                    i += 1;
                                }

                                _tokenControlDone = false;
                                if (!done || StrategicMap.Current.Players.GetPlayers(Player).AIControl.GetProcessCyclesMax() <= _playerProcessCycles)
                                {
                                    int index = _players.IndexOf((byte)Player);
                                    index += 1;
                                    _playerProcessCycles = 0;
                                    // { Original behavior: Index = Count is not wrapped here. }
                                    if (index > _players.Count)
                                    {
                                        index = 0;
                                    }

                                    Player = (sbyte)_players[index];
                                }
                            }

                            _tokenCycle += 1;
                            if (i >= _players.Count || _tokenCycle >= _tokenCycles)
                            {
                                TokenCyclesCaptured();
                            }
                        }

                        break;
                    case PlayMode.None:
                    default:
                        break;
                }
            }
            catch
            {
                ShowMessage("AI ERROR");
            }

            StrategicMap.Current.SynchronizeEnd();
            Status -= Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Executing);
        }
    }

    // intf_P5_06.inc:108

}

public enum AIExecutorProcessTimeMode : byte
{
    Static = 0, Dynamic = 1
}
