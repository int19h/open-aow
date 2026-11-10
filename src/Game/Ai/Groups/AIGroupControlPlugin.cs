// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai.Groups;

// references, cycles, control, status, state
public partial class AIGroupControlPlugin : EObject
{

    // AIGroup.pas:1354
    public AIGroupControlPlugin(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
        FState = -1;
    }

    public int FRefCount;
    public int FProcessCycles;
    public AIGroupControl FGroupControl;
    public Set<AIGroupControlPluginStatusElement> FStatus;
    public int FState;

    // AIGroup.pas:1361
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // AIGroup.pas:1367
    public override int Release()
    {
        FRefCount -= 1;
        int Result = FRefCount;
        if (FRefCount == 0)
        {
            Dispose();
        }

        return Result;
    }

    // AIGroup.pas:1374
    public virtual void Validate()
    {
    }

    // AIGroup.pas:1383
    public virtual bool GetBusy() => FStatus.Contains(AIGroupControlPluginStatusElement.Activated) && FState != -1;

    // AIGroup.pas:1388
    public virtual bool Activate(AIGroupControl Control)
    {
        FProcessCycles = 0;
        FGroupControl = Control;
        FStatus = FStatus + Set<AIGroupControlPluginStatusElement>.Of(AIGroupControlPluginStatusElement.Activated);
        FState = 0;
        return true;
    }

    // AIGroup.pas:1397
    public virtual void Deactivate()
    {
        FStatus = FStatus - Set<AIGroupControlPluginStatusElement>.Of(AIGroupControlPluginStatusElement.Activated);
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
        FState = -1;
    }

    // AIGroup.pas:1408
    public bool MainProcess()
    {
        bool Busy;
        int I;
        try
        {
            FProcessCycles += 1;
            Validate();
            Busy = false;
            I = 0;
            while (!Busy && FState != -1 && (I == 0 || AoWHSMap.Current.FAIExecuter.GetProcessTimeLeft() > 0))
            {
                I += 1;
                Busy = Process();
            }
        }
        catch
        {
            throw new InvalidOperationException(this.GetType().Name + " exception during state (" + IntToStr(FState) + ")");
        }

        return FState != -1;
    }

    // intf_P5_07.inc:218

}
