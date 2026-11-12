// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI;

// cycles, players, current player, player cycles, status, token done, time mode, time, used
// time, start, token cycles, token cycle, lock count
public partial class AIExecutor : EngineObject
{

    // AI.pas:559
    public AIExecutor(ECustomNode owner) : base(owner)
    {
        _players = new ByteList();
        Player = -1;
        _processTimeMode = AIExecutorProcessTimeMode.Static;
        _processTime = 5000;
        _tokenCycles = 5;
    }

    public int ProcessCycles { get; set; }

    private readonly ByteList _players;

    public sbyte Player { get; private set; }

    private int _playerProcessCycles;

    public Set<AIExecutorStatusElement> Status { get; private set; }

    private bool _tokenControlDone;
    private readonly AIExecutorProcessTimeMode _processTimeMode;
    private readonly int _processTime;
    private int _processTimeUsed;
    private int _processStarted;
    private int _tokenCycles;
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
        bool result;
        int used;
        switch (_processTimeMode)
        {
            case AIExecutorProcessTimeMode.Static:
                used = _processTimeUsed;
                if (used != 0)
                {
                    result = time <= _processTime - used;
                }
                else
                {
                    result = true;
                }

                break;
            case AIExecutorProcessTimeMode.Dynamic:
                if (_processTimeUsed != 0)
                {
                    result = time < GetProcessTimeLeft();
                }
                else
                {
                    result = true;
                }

                break;
            default:
                result = false;
                break;
        }

        return result;
    }

    // AI.pas:593
    public int GetProcessTimeLeft()
    {
        int result;
        switch (_processTimeMode)
        {
            case AIExecutorProcessTimeMode.Static:
                result = _processTime - _processTimeUsed;
                break;
            case AIExecutorProcessTimeMode.Dynamic:
                // { Original behavior: the millisecond delta is multiplied by 1000. }
                var elapsedMilliseconds = PixelFormats.GetElapsedMilliseconds();
                result = _processTime - (elapsedMilliseconds - _processStarted) * 1000;
                break;
            default:
                result = -1;
                break;
        }

        return result;
    }

    // AI.pas:603
    public int GetProcessTimeElapsed()
    {
        int result;
        switch (_processTimeMode)
        {
            case AIExecutorProcessTimeMode.Static:
                result = _processTimeUsed;
                break;
            case AIExecutorProcessTimeMode.Dynamic:
                result = (PixelFormats.GetElapsedMilliseconds() - _processStarted) * 1000;
                break;
            default:
                result = -1;
                break;
        }

        return result;
    }

    // AI.pas:617
    public bool ActivatePlayer(sbyte player)
    {
        bool result;
        if (_players.IndexOf((byte)player) == -1)
        {
            _players.Add((byte)player);
            Status = Status + Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Enabled);
            StrategicMap.Current.Players.GetPlayers(player).AIControl.Activate(this);
            if (Player == -1)
            {
                Player = player;
            }

            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // AI.pas:630
    public void DeactivatePlayer(sbyte player)
    {
        if (_players.IndexOf((byte)player) != -1)
        {
            _players.Remove((byte)player);
            StrategicMap.Current.Players.GetPlayers(player).AIControl.Deactivate();
            if (_players.Count == 0)
            {
                Player = -1;
                Status = Status - Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Enabled);
            }
        }
    }

    // AI.pas:644
    public void TokenControlDoneEvent(object sender)
    {
        if (Status.Contains(AIExecutorStatusElement.Enabled) && StrategicMap.Current.PlayMode == PlayMode.Simultaneous)
        {
            _tokenControlDone = true;
            if (StrategicMap.Current.CurrentPlayer == 0 && StrategicMap.Current.CombatMap == null)
            {
                CaptureTokenCycles(5);
            }
        }
    }

    // AI.pas:654
    public void Lock()
    {
        LockCount += 1;
    }

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
        Status = Status + Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.TokenLocked);
        StrategicMap.Current.TokenManager.TokenExecutor.Lock();
        _tokenCycle = 0;
        _tokenCycles = cycles;
    }

    // AI.pas:944
    public void TokenCyclesCaptured()
    {
        Assert(Status.Contains(AIExecutorStatusElement.TokenLocked), "", "AI.pas", 947);
        Status = Status - Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.TokenLocked);
        StrategicMap.Current.TokenManager.TokenExecutor.Unlock();
    }

    // AI.pas:952
    public void Process()
    {
        bool done;
        int I;
        int index;
        if (Status.Contains(AIExecutorStatusElement.Enabled) && LockCount == 0)
        {
            Status = Status + Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Executing);
            StrategicMap.Current.SynchronizeBegin();
            _processTimeUsed = 0;
            if (_processTimeMode == AIExecutorProcessTimeMode.Dynamic)
            {
                _processStarted = PixelFormats.GetElapsedMilliseconds();
            }
            else
            {
                _processStarted = 0;
            }

            try
            {
                switch (StrategicMap.Current.PlayMode)
                {
                    case PlayMode.Turn:
                        if (!StrategicMap.Current.Players.GetPlayers(Player).GetBusy() && StrategicMap.Current.TokenManager.PlayerReady(Player))
                        {
                            ProcessCycles += 1;
                            StrategicMap.Current.Players.GetPlayers(Player).AIControl.Process(this, _tokenControlDone);
                            _tokenControlDone = false;
                        }

                        break;
                    case PlayMode.Simultaneous:
                        if (Status.Contains(AIExecutorStatusElement.TokenLocked))
                        {
                            ProcessCycles += 1;
                            done = false;
                            I = 0;
                            while (!done && GetProcessTimeLeft() > 0 && Status.Contains(AIExecutorStatusElement.Enabled) && I < _players.Count)
                            {
                                done = StrategicMap.Current.Players.GetPlayers(Player).AIControl.Process(this, _tokenControlDone);
                                if (done)
                                {
                                    _playerProcessCycles += 1;
                                    I = 0;
                                }
                                else
                                {
                                    I += 1;
                                }

                                _tokenControlDone = false;
                                if (!done || StrategicMap.Current.Players.GetPlayers(Player).AIControl.GetProcessCyclesMax() <= _playerProcessCycles)
                                {
                                    index = _players.IndexOf((byte)Player);
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
                            if (I >= _players.Count || _tokenCycle >= _tokenCycles)
                            {
                                TokenCyclesCaptured();
                            }
                        }

                        break;
                }
            }
            catch
            {
                ShowMessage("AI ERROR");
            }

            StrategicMap.Current.SynchronizeEnd();
            Status = Status - Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Executing);
        }
    }

    // intf_P5_06.inc:108

}

public enum AIExecutorProcessTimeMode : byte
{
    Static = 0, Dynamic = 1
}
