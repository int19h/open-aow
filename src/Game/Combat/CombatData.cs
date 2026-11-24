#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiAssert;

namespace Aow.Game.Combat;

// damage set. byte lists. objects. players. parties. round. header string. Boolean. combat wall
public sealed partial class CombatData : EngineObject
{
    // Combat2.pas:184
    public new const int ClassId = 131_747;

    // Combat2.pas:162
    public CombatData(CustomNode? owner) : base(owner)
    {
        Players = new CombatPlayerList(null) { Holder = CreationHolder() };
        Parties = new CombatPartyList(null) { Holder = CreationHolder() };
        CombatObjects = new OwningNode(null) { Holder = CreationHolder() };
        DefeatedLeaders = [];
        TurnOrder = [];
        InvolvedLeaders = [];
    }

    public MapCoordinate Location { get; set; }
    public MapCoordinate AttackerLocation { get; set; }

    public byte[] TerrainAndOverlayTypes { get; } = new byte[2];

    public DamageTypes ImmunityTypes { get; private set; }
    public sbyte AttackerPlayer { get; set; }
    public sbyte DefenderPlayer { get; set; }

    public ByteList InvolvedLeaders { get; internal set; }
    public ByteList DefeatedLeaders { get; internal set; }

    public int CombatResult { get; set; }

    public byte CurrentPlayer { get; set; }
    public OwningNode CombatObjects { get; internal set; }
    public CombatPlayerList Players { get; }
    public CombatPartyList Parties { get; }
    public ByteList TurnOrder { get; }

    public int Round { get; set; }
    public string Header { get; set; } = "";
    public int WallType { get; set; }
    public bool WallBreached { get; set; }

    public CombatWall? Wall { get; set; }

    // Combat2.pas:173
    public override void Dispose()
    {
        Parties?.Dispose();
        Players?.Dispose();
        CombatObjects?.Dispose();
        DefeatedLeaders?.Dispose();
        InvolvedLeaders?.Dispose();
        base.Dispose();
    }

    // Combat2.pas:212
    public bool GetSideLeaderInvolved(int side)
    {
        bool result = false;
        result = InvolvedLeaders.Any(item => Players.GetPlayers((sbyte)item).SideIndex == side);
        return result;
    }

    // Combat2.pas:231
    public int GetCount() => CombatObjects.Count;

    // Combat2.pas:234
    public CombatObject GetObjects(int index) => CombatObjects.GetChild<CombatObject>(index);

    // The caller holds the guard that admits only the named kind, so the cast never fails there.
    public T GetObjects<T>(int index) where T : CombatObject => (T)GetObjects(index);

    // Combat2.pas:246
    public int IndexOfId(int id)
    {
        for (int result = CombatObjects.Count - 1; result >= 0; result--)
        {
            if (CombatObjects.GetChild<CombatObject>(result).Id == id)
            {
                return result;
            }
        }

        return -1;
    }

    // Combat2.pas:253
    public CombatObject? FindId(int id)
    {
        int i = CombatObjects.Count;
        while (i != 0)
        {
            i -= 1;
            if (CombatObjects.GetChild<CombatObject>(i).Id == id)
            {
                return CombatObjects.GetChild<CombatObject>(i);
            }
        }

        return null;
    }

    // The id names a unit in the caller's combat action, so the cast never fails there.
    public T FindId<T>(int id) where T : CombatObject => (T)FindId(id);

    // Combat2.pas:269
    public CombatObject? FindPosition(byte position)
    {
        CombatObject? result = null;
        int i = CombatObjects.Count;
        while (i != 0 && result is null)
        {
            i -= 1;
            if (position == CombatObjects.GetChild<CombatObject>(i).Position)
            {
                result = CombatObjects.GetChild<CombatObject>(i);
            }
        }

        return result;
    }

    // Combat2.pas:282
    public void AddObject(CombatObject combatObject)
    {
        Assert(FindId(combatObject.Id) is null && FindPosition(combatObject.Position) is null, message: "CombatObject Position/ID double defined", file: "Combat2.pas", line: 284);
        combatObject.SetOwner(CombatObjects);
    }
}
