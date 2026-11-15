#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

// damage set. byte lists. objects. players. parties. round. header string. Boolean. combat wall
public partial class CombatData : EngineObject
{
    // Combat2.pas:184
    public new const int ClassId = 131747;

    // Combat2.pas:162
    public CombatData(CustomNode owner) : base(owner)
    {
        Players = new CombatPlayerList(null) { Holder = CreationHolder() };
        Parties = new CombatPartyList(null) { Holder = CreationHolder() };
        CombatObjects = new OwningNode(null) { Holder = CreationHolder() };
        DefeatedLeaders = new ByteList();
        TurnOrder = new ByteList();
        InvolvedLeaders = new ByteList();
    }

    public MapCoordinate Location { get; set; }
    public MapCoordinate AttackerLocation { get; set; }

    public byte[] TerrainAndOverlayTypes { get; } = new byte[2];

    public Set<DamageType> ImmunityTypes { get; private set; }
    public sbyte AttackerPlayer { get; set; }
    public sbyte DefenderPlayer { get; set; }

    public ByteList InvolvedLeaders { get; set; }
    public ByteList DefeatedLeaders { get; set; }

    public byte CombatResult { get; set; }

    public byte CurrentPlayer { get; set; }
    public OwningNode CombatObjects { get; set; }
    public CombatPlayerList Players { get; }
    public CombatPartyList Parties { get; }
    public ByteList TurnOrder { get; }

    public int Round { get; set; }
    public string Header { get; set; } = "";
    public byte WallType { get; set; }
    public bool WallBreached { get; set; }

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
        int i;
        bool result = false;
        i = InvolvedLeaders.Count;
        while (!result && i != 0)
        {
            i -= 1;
            var item = InvolvedLeaders[i];
            result = Players.GetPlayers((sbyte)item).SideIndex == side;
        }

        return result;
    }

    // Combat2.pas:231
    public int GetCount() => CombatObjects.Count;

    // Combat2.pas:234
    public CombatObject GetObjects(int index) => (CombatObject)CombatObjects.GetChild(index);

    // The caller holds the guard that admits only the named kind, so the cast never fails there.
    public T GetObjects<T>(int index) where T : CombatObject => (T)GetObjects(index);

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
        int i;
        i = CombatObjects.Count;
        while (i != 0)
        {
            i -= 1;
            if (((CombatObject)CombatObjects.GetChild(i)).Id == id)
            {
                return (CombatObject)CombatObjects.GetChild(i);
            }
        }

        return null;
    }

    // The id names a unit in the caller's combat action, so the cast never fails there.
    public T FindId<T>(int id) where T : CombatObject => (T)FindId(id);

    // Combat2.pas:269
    public CombatObject FindPosition(byte position)
    {
        int i;
        CombatObject result = null;
        i = CombatObjects.Count;
        while (i != 0 && result == null)
        {
            i -= 1;
            if (position == ((CombatObject)CombatObjects.GetChild(i)).Position)
            {
                result = (CombatObject)CombatObjects.GetChild(i);
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
