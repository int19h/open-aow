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
        FPlayers = new ByteList();
        FPlayer = -1;
        FProcessTimeMode = ProcessTimeMode.Static;
        FProcessTime = 5000;
        FTokenCycles = 5;
    }

    public int FProcessCycles;
    public ByteList FPlayers;
    public sbyte FPlayer;
    public int FPlayerProcessCycles;
    public Set<AIExecutorStatusElement> FStatus;
    public bool FTokenControlDone;
    public ProcessTimeMode FProcessTimeMode;
    public int FProcessTime;
    public int FProcessTimeUsed;
    public int FProcessStarted;
    public int FTokenCycles;
    public int FTokenCycle;
    public int FLockCount;

    // AI.pas:569
    public override void Dispose()
    {
        FPlayers?.Dispose();
        base.Dispose();
    }

    // AI.pas:575
    public bool RequestProcessTime(int Time)
    {
        bool Result;
        int Used;
        switch (FProcessTimeMode)
        {
            case ProcessTimeMode.Static:
                Used = FProcessTimeUsed;
                if (Used != 0)
                {
                    Result = Time <= FProcessTime - Used;
                }
                else
                {
                    Result = true;
                }

                break;
            case ProcessTimeMode.Dynamic:
                if (FProcessTimeUsed != 0)
                {
                    Result = Time < GetProcessTimeLeft();
                }
                else
                {
                    Result = true;
                }

                break;
            default:
                Result = false;
                break;
        }

        return Result;
    }

    // AI.pas:593
    public int GetProcessTimeLeft()
    {
        int Result;
        switch (FProcessTimeMode)
        {
            case ProcessTimeMode.Static:
                Result = FProcessTime - FProcessTimeUsed;
                break;
            case ProcessTimeMode.Dynamic:
                // { Original behavior: the millisecond delta is multiplied by 1000. }
                var elapsedMilliSeconds = PixelFormats.GetElapsedMilliSeconds();
                Result = FProcessTime - (elapsedMilliSeconds - FProcessStarted) * 1000;
                break;
            default:
                Result = -1;
                break;
        }

        return Result;
    }

    // AI.pas:603
    public int GetProcessTimeElapsed()
    {
        int Result;
        switch (FProcessTimeMode)
        {
            case ProcessTimeMode.Static:
                Result = FProcessTimeUsed;
                break;
            case ProcessTimeMode.Dynamic:
                Result = (PixelFormats.GetElapsedMilliSeconds() - FProcessStarted) * 1000;
                break;
            default:
                Result = -1;
                break;
        }

        return Result;
    }

    // AI.pas:617
    public bool ActivatePlayer(sbyte Player)
    {
        bool Result;
        if (FPlayers.IndexOf((byte)Player) == -1)
        {
            FPlayers.Add((byte)Player);
            FStatus = FStatus + Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Enabled);
            AoWHSMap.Current.FPlayers.GetPlayers(Player).FAIControl.Activate(this);
            if (FPlayer == -1)
            {
                FPlayer = Player;
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AI.pas:630
    public void DeactivatePlayer(sbyte Player)
    {
        if (FPlayers.IndexOf((byte)Player) != -1)
        {
            FPlayers.Remove((byte)Player);
            AoWHSMap.Current.FPlayers.GetPlayers(Player).FAIControl.Deactivate();
            if (FPlayers.Count == 0)
            {
                FPlayer = -1;
                FStatus = FStatus - Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Enabled);
            }
        }
    }

    // AI.pas:644
    public void TokenControlDoneEvent(object Sender)
    {
        if (FStatus.Contains(AIExecutorStatusElement.Enabled) && AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous)
        {
            FTokenControlDone = true;
            if (AoWHSMap.Current.FCurrentPlayer == 0 && AoWHSMap.Current.FCombatMap == null)
            {
                CaptureTokenCycles(5);
            }
        }
    }

    // AI.pas:654
    public void Lock()
    {
        FLockCount += 1;
    }

    // AI.pas:659
    public void Unlock()
    {
        Assert(FLockCount != 0, "", "AI.pas", 931);
        FLockCount -= 1;
    }

    // AI.pas:935
    public void CaptureTokenCycles(int Cycles)
    {
        Assert(!FStatus.Contains(AIExecutorStatusElement.TokenLocked), "", "AI.pas", 937);
        FStatus = FStatus + Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.TokenLocked);
        AoWHSMap.Current.FTokenManager.TokenExecuter.Lock();
        FTokenCycle = 0;
        FTokenCycles = Cycles;
    }

    // AI.pas:944
    public void TokenCyclesCaptured()
    {
        Assert(FStatus.Contains(AIExecutorStatusElement.TokenLocked), "", "AI.pas", 947);
        FStatus = FStatus - Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.TokenLocked);
        AoWHSMap.Current.FTokenManager.TokenExecuter.Unlock();
    }

    // AI.pas:952
    public void Process()
    {
        bool Done;
        int I;
        int Index;
        if (FStatus.Contains(AIExecutorStatusElement.Enabled) && FLockCount == 0)
        {
            FStatus = FStatus + Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Executing);
            AoWHSMap.Current.SynchroniseBegin();
            FProcessTimeUsed = 0;
            if (FProcessTimeMode == ProcessTimeMode.Dynamic)
            {
                FProcessStarted = PixelFormats.GetElapsedMilliSeconds();
            }
            else
            {
                FProcessStarted = 0;
            }

            try
            {
                switch (AoWHSMap.Current.FPlayMode)
                {
                    case PlayMode.Turn:
                        if (!AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).GetBusy() && AoWHSMap.Current.FTokenManager.PlayerReady(FPlayer))
                        {
                            FProcessCycles += 1;
                            AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FAIControl.Process(this, FTokenControlDone);
                            FTokenControlDone = false;
                        }

                        break;
                    case PlayMode.Simultaneous:
                        if (FStatus.Contains(AIExecutorStatusElement.TokenLocked))
                        {
                            FProcessCycles += 1;
                            Done = false;
                            I = 0;
                            while (!Done && GetProcessTimeLeft() > 0 && FStatus.Contains(AIExecutorStatusElement.Enabled) && I < FPlayers.Count)
                            {
                                Done = AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FAIControl.Process(this, FTokenControlDone);
                                if (Done)
                                {
                                    FPlayerProcessCycles += 1;
                                    I = 0;
                                }
                                else
                                {
                                    I += 1;
                                }

                                FTokenControlDone = false;
                                if (!Done || AoWHSMap.Current.FPlayers.GetPlayers(FPlayer).FAIControl.GetProcessCyclesMax() <= FPlayerProcessCycles)
                                {
                                    Index = FPlayers.IndexOf((byte)FPlayer);
                                    Index += 1;
                                    FPlayerProcessCycles = 0;
                                    // { Original behavior: Index = Count is not wrapped here. }
                                    if (Index > FPlayers.Count)
                                    {
                                        Index = 0;
                                    }

                                    FPlayer = (sbyte)FPlayers[Index];
                                }
                            }

                            FTokenCycle += 1;
                            if (I >= FPlayers.Count || FTokenCycle >= FTokenCycles)
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

            AoWHSMap.Current.SynchroniseEnd();
            FStatus = FStatus - Set<AIExecutorStatusElement>.Of(AIExecutorStatusElement.Executing);
        }
    }

    public enum ProcessTimeMode : byte
    {
        Static = 0, Dynamic = 1
    }

    // intf_P5_06.inc:108

}
