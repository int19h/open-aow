#nullable disable
// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.AI.Groups;

// references, cycles, control, status, state
public class AIGroupTask(CustomNode owner) : EngineObject(owner)
{
    private int _refCount = 1;
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Construction snapshots record this field, and the Delphi field is only written.")]
    private int _processCycles;

    public AIGroupControl GroupControl { get; protected set; }
    public Set<AIGroupTaskStatusElement> Status { get; private set; }
    public int State { get; protected set; } = -1;

    // AIGroup.pas:1361
    public override int AddRef()
    {
        _refCount += 1;
        return _refCount;
    }

    // AIGroup.pas:1367
    public override int Release()
    {
        _refCount -= 1;
        int result = _refCount;
        if (_refCount == 0)
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
        _processCycles = 0;
        GroupControl = control;
        Status += Set<AIGroupTaskStatusElement>.Of(AIGroupTaskStatusElement.Activated);
        State = 0;
        return true;
    }

    // AIGroup.pas:1397
    public virtual void Deactivate() => Status -= Set<AIGroupTaskStatusElement>.Of(AIGroupTaskStatusElement.Activated);

    // AIGroup.pas:1402
    public virtual bool Process()
    {
        Done();
        return false;
    }

    // AIGroup.pas:1378
    public void Done() => State = -1;

    // AIGroup.pas:1408
    public bool MainProcess()
    {
        try
        {
            _processCycles += 1;
            Validate();
            bool busy = false;
            int i = 0;
            while (!busy && State != -1 && (i == 0 || StrategicMap.Current.AIExecutor.GetProcessTimeLeft() > 0))
            {
                i += 1;
                busy = Process();
            }
        }
        catch
        {
            throw new InvalidOperationException($"{GetType().Name} exception during state ({IntToStr(State)})");
        }

        return State != -1;
    }
}
