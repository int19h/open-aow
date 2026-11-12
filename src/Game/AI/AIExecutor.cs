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
        Players = new ByteList();
        Player = -1;
        ProcessTimeMode = AIExecutorProcessTimeMode.Static;
        ProcessTime = 5000;
        TokenCycles = 5;
    }

    public int ProcessCycles;
    public ByteList Players;
    public sbyte Player;
    public int PlayerProcessCycles;
    public Set<AIExecutorStatusElement> Status;
    public bool TokenControlDone;
    public AIExecutorProcessTimeMode ProcessTimeMode;
    public int ProcessTime;
    public int ProcessTimeUsed;
    public int ProcessStarted;
    public int TokenCycles;
    public int TokenCycle;
    public int LockCount;

    // AI.pas:569
    public override void Dispose()
    {
        Players?.Dispose();
        base.Dispose();
    }

    // AI.pas:575
    public bool RequestProcessTime(int time)
    {
        bool result;
        int used;
        switch (ProcessTimeMode)
        {
            case AIExecutorProcessTimeMode.Static:
                used = ProcessTimeUsed;
                if (used != 0)
                {
                    result = time <= ProcessTime - used;
                }
                else
                {
                    result = true;
                }

                break;
            case AIExecutorProcessTimeMode.Dynamic:
                if (ProcessTimeUsed != 0)
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
        switch (ProcessTimeMode)
        {
            case AIExecutorProcessTimeMode.Static:
                result = ProcessTime - ProcessTimeUsed;
                break;
            case AIExecutorProcessTimeMode.Dynamic:
                // { Original behavior: the millisecond delta is multiplied by 1000. }
                var elapsedMilliseconds = PixelFormats.GetElapsedMilliseconds();
                result = ProcessTime - (elapsedMilliseconds - ProcessStarted) * 1000;
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
        switch (ProcessTimeMode)
        {
            case AIExecutorProcessTimeMode.Static:
                result = ProcessTimeUsed;
                break;
            case AIExecutorProcessTimeMode.Dynamic:
                result = (PixelFormats.GetElapsedMilliseconds() - ProcessStarted) * 1000;
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
        if (Players.IndexOf((byte)player) == -1)
        {
            Players.Add((byte)player);
            Status = Status + Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Enabled);
            AoWHSMap.Current.Players.GetPlayers(player).AIControl.Activate(this);
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
        if (Players.IndexOf((byte)player) != -1)
        {
            Players.Remove((byte)player);
            AoWHSMap.Current.Players.GetPlayers(player).AIControl.Deactivate();
            if (Players.Count == 0)
            {
                Player = -1;
                Status = Status - Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Enabled);
            }
        }
    }

    // AI.pas:644
    public void TokenControlDoneEvent(object sender)
    {
        if (Status.Contains(AIExecutorStatusElement.Enabled) && AoWHSMap.Current.PlayMode == PlayMode.Simultaneous)
        {
            TokenControlDone = true;
            if (AoWHSMap.Current.CurrentPlayer == 0 && AoWHSMap.Current.CombatMap == null)
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
        AoWHSMap.Current.TokenManager.TokenExecutor.Lock();
        TokenCycle = 0;
        TokenCycles = cycles;
    }

    // AI.pas:944
    public void TokenCyclesCaptured()
    {
        Assert(Status.Contains(AIExecutorStatusElement.TokenLocked), "", "AI.pas", 947);
        Status = Status - Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.TokenLocked);
        AoWHSMap.Current.TokenManager.TokenExecutor.Unlock();
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
            AoWHSMap.Current.SynchronizeBegin();
            ProcessTimeUsed = 0;
            if (ProcessTimeMode == AIExecutorProcessTimeMode.Dynamic)
            {
                ProcessStarted = PixelFormats.GetElapsedMilliseconds();
            }
            else
            {
                ProcessStarted = 0;
            }

            try
            {
                switch (AoWHSMap.Current.PlayMode)
                {
                    case PlayMode.Turn:
                        if (!AoWHSMap.Current.Players.GetPlayers(Player).GetBusy() && AoWHSMap.Current.TokenManager.PlayerReady(Player))
                        {
                            ProcessCycles += 1;
                            AoWHSMap.Current.Players.GetPlayers(Player).AIControl.Process(this, TokenControlDone);
                            TokenControlDone = false;
                        }

                        break;
                    case PlayMode.Simultaneous:
                        if (Status.Contains(AIExecutorStatusElement.TokenLocked))
                        {
                            ProcessCycles += 1;
                            done = false;
                            I = 0;
                            while (!done && GetProcessTimeLeft() > 0 && Status.Contains(AIExecutorStatusElement.Enabled) && I < Players.Count)
                            {
                                done = AoWHSMap.Current.Players.GetPlayers(Player).AIControl.Process(this, TokenControlDone);
                                if (done)
                                {
                                    PlayerProcessCycles += 1;
                                    I = 0;
                                }
                                else
                                {
                                    I += 1;
                                }

                                TokenControlDone = false;
                                if (!done || AoWHSMap.Current.Players.GetPlayers(Player).AIControl.GetProcessCyclesMax() <= PlayerProcessCycles)
                                {
                                    index = Players.IndexOf((byte)Player);
                                    index += 1;
                                    PlayerProcessCycles = 0;
                                    // { Original behavior: Index = Count is not wrapped here. }
                                    if (index > Players.Count)
                                    {
                                        index = 0;
                                    }

                                    Player = (sbyte)Players[index];
                                }
                            }

                            TokenCycle += 1;
                            if (I >= Players.Count || TokenCycle >= TokenCycles)
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

            AoWHSMap.Current.SynchronizeEnd();
            Status = Status - Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Executing);
        }
    }

    // intf_P5_06.inc:108

}

public enum AIExecutorProcessTimeMode : byte
{
    Static = 0, Dynamic = 1
}
