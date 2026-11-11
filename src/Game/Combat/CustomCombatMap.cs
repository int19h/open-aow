// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Combat;

// combat event lists
public partial class CustomCombatMap : GameMap
{

    internal EventStore<CombatSpellCaster> FCombatStartedEventsStore;

    public event NotifyEvent<CombatSpellCaster> FCombatStartedEvents
    {
        add => FCombatStartedEventsStore.Add(value);
        remove => FCombatStartedEventsStore.Remove(value);
    }

    internal EventStore<AbilityActivation> FAbilityActivatedEventsStore;

    public event NotifyEvent<AbilityActivation> FAbilityActivatedEvents
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
    public CustomCombatMap(ECustomNode owner) : base(owner)
    {
        FCombatStartedEventsStore = new();
        FAbilityActivatedEventsStore = new();
    }

}
