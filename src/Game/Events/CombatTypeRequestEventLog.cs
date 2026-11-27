#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Commands;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Events;

// selection. location. TokenEvent. owned object
public partial class CombatTypeRequestEventLog(CustomNode? owner, EngineObject holder) : EventLog(owner, holder: holder)
{
    public CombatType CombatType { get; private set; }
    public sbyte AttackerPlayer { get; set; }
    public sbyte DefenderPlayer { get; set; }
    public MapCoordinate Location { get; set; }

    private EngineObject? _tokenEvent;

    public bool CreateNewMove { get; set; }
    public ArmyId ArmyId { get; set; }

    // Combat2.pas:1
    public override void Dispose()
    {
        SetTokenEvent(null);
        object objectValue = ArmyId;
        (objectValue as IDisposable)?.Dispose();

        base.Dispose();
    }

    // Combat2.pas:18
    public override bool Execute(bool mode, ExecuteTrigger trigger, ExecuteDoneEvent? done) => base.Execute(mode, trigger, done);

    // Combat2.pas:10
    public void SetTokenEvent(TokenEvent? tokenEvent)
    {
        // { Original releases the old token before retaining its replacement. }
        _ = (_tokenEvent?.Release());

        _tokenEvent = tokenEvent;
        _ = (_tokenEvent?.AddRef());
    }

    // Combat2.pas:27
    public void Select(CombatType combatType) => CombatType = combatType;

    // Combat2.pas:30
    public void SetCenterLocation(int X, int Y, int L)
    {
        {
            Location = Location with { X = (sbyte)X };
            Location = Location with { Y = (sbyte)Y };
        }
        Location = Location with { MapLevel = (sbyte)L };
    }
}
