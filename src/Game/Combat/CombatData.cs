// Translated from AoWE.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// damage set. byte lists. objects. players. parties. round. header string. Boolean. combat wall
public partial class CombatData : EngineObject
{
    // Combat2.pas:184
    public new const int ClassID = 131747;

    // Combat2.pas:162
    public CombatData(ECustomNode owner) : base(owner)
    {
        FPlayers = new CombatPlayerList(null);
        FParties = new CombatPartyList(null);
        FCombatObjects = new ENode(null);
        FDefeatedLeaders = new ByteList();
        FTurnOrder = new ByteList();
        FInvolvedLeaders = new ByteList();
    }

    public int FX;
    public int FY;
    public int FL;
    public int FAttackerX;
    public int FAttackerY;
    public int FAttackerL;
    public ByteArray2 F_CombatData_20;
    public Set<DamageType> F_CombatData_22;
    public sbyte FAttackerPlayer;
    public sbyte FDefenderPlayer;
    public ByteList FInvolvedLeaders;
    public ByteList FDefeatedLeaders;
    public byte F_CombatData_30;
    public byte FCurrentPlayer;
    public ENode FCombatObjects;
    public CombatPlayerList FPlayers;
    public CombatPartyList FParties;
    public ByteList FTurnOrder;
    public int FRound;
    public string FHeader = "";
    public byte FWallType;
    public bool FWallBreached;
    public CombatWall FWall;

    // Combat2.pas:173
    public override void Dispose()
    {
        FParties?.Dispose();
        FPlayers?.Dispose();
        FCombatObjects?.Dispose();
        FDefeatedLeaders?.Dispose();
        FInvolvedLeaders?.Dispose();
        FTurnOrder?.Dispose();
        base.Dispose();
    }

    // Combat2.pas:212
    public bool GetSideLeaderInvolved(byte Side)
    {
        int I;
        bool Result = false;
        I = FInvolvedLeaders.Count;
        while (!Result && I != 0)
        {
            I -= 1;
            var item = FInvolvedLeaders[I];
            Result = FPlayers.GetPlayers((sbyte)item).FSideIndex == Side;
        }

        return Result;
    }

    // Combat2.pas:231
    public int GetCount() => FCombatObjects.FChildren.Count;

    // Combat2.pas:234
    public CombatObject GetObjects(int Index) => (CombatObject)FCombatObjects.FChildren.List[Index];

    // Combat2.pas:246
    public int IndexOfID(int ID)
    {
        for (int Result = FCombatObjects.Count - 1; Result >= 0; Result--)
        {
            if (((CombatObject)FCombatObjects.FChildren.List[Result]).FID == ID)
            {
                return Result;
            }
        }

        return -1;
    }

    // Combat2.pas:253
    public CombatObject FindID(int ID)
    {
        int I;
        I = FCombatObjects.FChildren.Count;
        while (I != 0)
        {
            I -= 1;
            if (((CombatObject)FCombatObjects.FChildren.List[I]).FID == ID)
            {
                return (CombatObject)FCombatObjects.FChildren.List[I];
            }
        }

        return null;
    }

    // Combat2.pas:269
    public CombatObject FindPosition(byte Position)
    {
        int I;
        CombatObject Result = null;
        I = FCombatObjects.FChildren.Count;
        while (I != 0 && Result == null)
        {
            I -= 1;
            if (Position == ((CombatObject)FCombatObjects.FChildren.List[I]).FPosition)
            {
                Result = (CombatObject)FCombatObjects.FChildren.List[I];
            }
        }

        return Result;
    }

    // Combat2.pas:282
    public void AddObject(CombatObject CombatObject)
    {
        Assert(FindID(CombatObject.FID) == null && FindPosition(CombatObject.FPosition) == null, "CombatObject Position/ID double defined", "Combat2.pas", 284);
        CombatObject.SetOwner(FCombatObjects);
    }

    // intf_P3_02.inc:252

}
