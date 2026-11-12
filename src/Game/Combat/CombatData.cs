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
    public new const int ClassId = 131747;

    // Combat2.pas:162
    public CombatData(ECustomNode owner) : base(owner)
    {
        Players = new CombatPlayerList(null);
        Parties = new CombatPartyList(null);
        CombatObjects = new ENode(null);
        DefeatedLeaders = new ByteList();
        TurnOrder = new ByteList();
        InvolvedLeaders = new ByteList();
    }

    public int X;
    public int Y;
    public int MapLevel;
    public int AttackerX;
    public int AttackerY;
    public int AttackerMapLevel;

    public byte[] TerrainAndOverlayTypes { get; } = new byte[2];

    public Set<DamageType> ImmunityTypes;
    public sbyte AttackerPlayer;
    public sbyte DefenderPlayer;

    public ByteList InvolvedLeaders { get; set; }
    public ByteList DefeatedLeaders { get; set; }

    public byte CombatResult;

    public byte CurrentPlayer { get; set; }
    public ENode CombatObjects { get; set; }
    public CombatPlayerList Players { get; }
    public CombatPartyList Parties { get; }
    public ByteList TurnOrder { get; }

    public int Round;
    public string Header = "";
    public byte WallType;
    public bool WallBreached;

    public CombatWall Wall { get; set; }

    // Combat2.pas:173
    public override void Dispose()
    {
        Parties?.Dispose();
        Players?.Dispose();
        CombatObjects?.Dispose();
        DefeatedLeaders?.Dispose();
        InvolvedLeaders?.Dispose();
        TurnOrder?.Dispose();
        base.Dispose();
    }

    // Combat2.pas:212
    public bool GetSideLeaderInvolved(byte side)
    {
        int I;
        bool result = false;
        I = InvolvedLeaders.Count;
        while (!result && I != 0)
        {
            I -= 1;
            var item = InvolvedLeaders[I];
            result = Players.GetPlayers((sbyte)item).SideIndex == side;
        }

        return result;
    }

    // Combat2.pas:231
    public int GetCount() => CombatObjects.Count;

    // Combat2.pas:234
    public CombatObject GetObjects(int index) => (CombatObject)CombatObjects.GetChild(index);

    // Combat2.pas:246
    public int IndexOfId(int id)
    {
        for (int result = CombatObjects.Count - 1; result >= 0; result--)
        {
            if (((CombatObject)CombatObjects.GetChild(result)).Id == id)
            {
                return result;
            }
        }

        return -1;
    }

    // Combat2.pas:253
    public CombatObject FindId(int id)
    {
        int I;
        I = CombatObjects.Count;
        while (I != 0)
        {
            I -= 1;
            if (((CombatObject)CombatObjects.GetChild(I)).Id == id)
            {
                return (CombatObject)CombatObjects.GetChild(I);
            }
        }

        return null;
    }

    // Combat2.pas:269
    public CombatObject FindPosition(byte position)
    {
        int I;
        CombatObject result = null;
        I = CombatObjects.Count;
        while (I != 0 && result == null)
        {
            I -= 1;
            if (position == ((CombatObject)CombatObjects.GetChild(I)).Position)
            {
                result = (CombatObject)CombatObjects.GetChild(I);
            }
        }

        return result;
    }

    // Combat2.pas:282
    public void AddObject(CombatObject combatObject)
    {
        Assert(FindId(combatObject.Id) == null && FindPosition(combatObject.Position) == null, "CombatObject Position/ID double defined", "Combat2.pas", 284);
        combatObject.SetOwner(CombatObjects);
    }

    // intf_P3_02.inc:252

}
