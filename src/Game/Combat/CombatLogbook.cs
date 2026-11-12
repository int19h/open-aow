// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Combat;

// RefCount. InitialData. FinalData. Actions. result flag
public partial class CombatLogbook : EngineObject
{
    // Combat2.pas:381
    public new const int ClassId = 131749;

    // Combat2.pas:349
    public CombatLogbook(ECustomNode owner) : base(owner)
    {
        _refCount = 1;
        Actions = new CombatActionList(null) { Holder = CreationHolder() };
        FinalData = new CombatData(null) { Holder = CreationHolder() };
        InitialData = new CombatData(null) { Holder = CreationHolder() };
        PlayerEliminated = false;
    }

    private int _refCount;

    public CombatData InitialData { get; }
    public CombatData FinalData { get; }
    public CombatActionList Actions { get; }
    public CombatResult CombatResult { get; }

    public bool PlayerEliminated;

    // Combat2.pas:359
    public override void Dispose()
    {
        Clear();
        Actions?.Dispose();
        FinalData?.Dispose();
        InitialData?.Dispose();
        base.Dispose();
    }

    // Combat2.pas:396
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Object(InitialData, 5);
        stream.Object(FinalData, 6);
        stream.Object(Actions, 7);
        stream.Boolean(ref PlayerEliminated, 8);
    }

    // Combat2.pas:384
    public override int AddRef()
    {
        _refCount += 1;
        return _refCount;
    }

    // Combat2.pas:387
    public override int Release()
    {
        int N;
        _refCount -= 1;
        N = _refCount;
        int result = N;
        if (N == 0)
        {
            Dispose();
        }

        return result;
    }

    // Combat2.pas:374
    public override void Clear()
    {
        FinalData.Clear();
        InitialData.Clear();
        Actions.Clear();
    }

    // Combat2.pas:368
    public void SetInitialData(CombatData data)
    {
        InitialData.Assign(data, GetEngine());
    }

    // Combat2.pas:371
    public void SetFinalData(CombatData data)
    {
        FinalData.Assign(data, GetEngine());
    }

    // intf_P3_02.inc:278

}
