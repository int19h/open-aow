// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Combat;

// FRefCount. FInitialData. FFinalData. FActions. result flag
public partial class CombatLogbook : EObject
{
    // Combat2.pas:381
    public new const int ClassID = 131749;

    // Combat2.pas:349
    public CombatLogbook(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
        FActions = new CombatActionList(null);
        FFinalData = new CombatData(null);
        FInitialData = new CombatData(null);
        F_CombatLogbook_19 = false;
    }

    public int FRefCount;
    public CombatData FInitialData;
    public CombatData FFinalData;
    public CombatActionList FActions;
    public CombatResult FCombatResult;
    public bool F_CombatLogbook_19;

    // Combat2.pas:359
    public override void Dispose()
    {
        Clear();
        FActions?.Dispose();
        FFinalData?.Dispose();
        FInitialData?.Dispose();
        base.Dispose();
    }

    // Combat2.pas:396
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwEObject(FInitialData, 5);
        Stream.rwEObject(FFinalData, 6);
        Stream.rwEObject(FActions, 7);
        Stream.rwBool(ref F_CombatLogbook_19, 8);
    }

    // Combat2.pas:384
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // Combat2.pas:387
    public override int Release()
    {
        int N;
        FRefCount -= 1;
        N = FRefCount;
        int Result = N;
        if (N == 0)
        {
            Dispose();
        }

        return Result;
    }

    // Combat2.pas:374
    public override void Clear()
    {
        FFinalData.Clear();
        FInitialData.Clear();
        FActions.Clear();
    }

    // Combat2.pas:368
    public void SetInitialData(CombatData Data)
    {
        FInitialData.Assign(Data, GameTables.AoWEngine);
    }

    // Combat2.pas:371
    public void SetFinalData(CombatData Data)
    {
        FFinalData.Assign(Data, GameTables.AoWEngine);
    }

    // intf_P3_02.inc:278

}
