// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat;

// combat event lists
public partial class AbstractAoWCombatMap : AbstractAoWHSMap
{

    internal EventStore<CombatSpellCaster> FCombatStartedEventsStore;

    public event NotifyEvent<CombatSpellCaster> FCombatStartedEvents
    {
        add => FCombatStartedEventsStore.Add(value);
        remove => FCombatStartedEventsStore.Remove(value);
    }

    internal EventStore<ActivateAbilityInfo> FAbilityActivatedEventsStore;

    public event NotifyEvent<ActivateAbilityInfo> FAbilityActivatedEvents
    {
        add => FAbilityActivatedEventsStore.Add(value);
        remove => FAbilityActivatedEventsStore.Remove(value);
    }

    // AoWHSMap.PAS:914
    public override void Dispose()
    {
        base.Dispose();
        FCombatStartedEventsStore.Clear();
        FAbilityActivatedEventsStore.Clear();
    }

    // AoWHSMap.PAS:907
    public AbstractAoWCombatMap(ECustomNode owner) : base(owner)
    {
        FCombatStartedEventsStore = new();
        FAbilityActivatedEventsStore = new();
    }

}
