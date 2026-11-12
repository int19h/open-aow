// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI.Groups;

// references, cycles, control, status, state
public partial class AIGroupTask : EngineObject
{

    // AIGroup.pas:1354
    public AIGroupTask(ECustomNode owner) : base(owner)
    {
        RefCount = 1;
        State = -1;
    }

    public int RefCount;
    public int ProcessCycles;
    public AIGroupControl GroupControl;
    public Set<AIGroupTaskStatusElement> Status;
    public int State;

    // AIGroup.pas:1361
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // AIGroup.pas:1367
    public override int Release()
    {
        RefCount -= 1;
        int result = RefCount;
        if (RefCount == 0)
        {
            Dispose();
        }

        return result;
    }

    // AIGroup.pas:1374
    public virtual void Validate()
    {
    }

    // AIGroup.pas:1383
    public virtual bool GetBusy() => Status.Contains(AIGroupTaskStatusElement.Activated) && State != -1;

    // AIGroup.pas:1388
    public virtual bool Activate(AIGroupControl control)
    {
        ProcessCycles = 0;
        GroupControl = control;
        Status = Status + Set<AIGroupTaskStatusElement>.Of(AIGroupTaskStatusElement.Activated);
        State = 0;
        return true;
    }

    // AIGroup.pas:1397
    public virtual void Deactivate()
    {
        Status = Status - Set<AIGroupTaskStatusElement>.Of(AIGroupTaskStatusElement.Activated);
    }

    // AIGroup.pas:1402
    public virtual bool Process()
    {
        Done();
        return false;
    }

    // AIGroup.pas:1378
    public void Done()
    {
        State = -1;
    }

    // AIGroup.pas:1408
    public bool MainProcess()
    {
        bool busy;
        int I;
        try
        {
            ProcessCycles += 1;
            Validate();
            busy = false;
            I = 0;
            while (!busy && State != -1 && (I == 0 || AoWHSMap.Current.AIExecutor.GetProcessTimeLeft() > 0))
            {
                I += 1;
                busy = Process();
            }
        }
        catch
        {
            throw new InvalidOperationException(this.GetType().Name + " exception during state (" + IntToStr(State) + ")");
        }

        return State != -1;
    }

    // intf_P5_07.inc:218

}
