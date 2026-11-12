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

    internal EventStore<CombatSpellCaster> CombatStartedEventsStore { get; }

    public event NotifyEvent<CombatSpellCaster> FCombatStartedEvents
    {
        add => CombatStartedEventsStore.Add(value);
        remove => CombatStartedEventsStore.Remove(value);
    }

    internal EventStore<AbilityActivation> AbilityActivatedEventsStore { get; }

    public event NotifyEvent<AbilityActivation> FAbilityActivatedEvents
    {
        add => AbilityActivatedEventsStore.Add(value);
        remove => AbilityActivatedEventsStore.Remove(value);
    }

    // AoWHSMap.PAS:914
    public override void Dispose()
    {
        base.Dispose();
        CombatStartedEventsStore.Clear();
        AbilityActivatedEventsStore.Clear();
    }

    // AoWHSMap.PAS:907
    public CustomCombatMap(ECustomNode owner) : base(owner)
    {
        CombatStartedEventsStore = new();
        AbilityActivatedEventsStore = new();
    }

}
