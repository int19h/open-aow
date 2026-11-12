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

    internal EventStore<CombatSpellCaster> _combatStartedEventsStore;

    public event NotifyEvent<CombatSpellCaster> FCombatStartedEvents
    {
        add => _combatStartedEventsStore.Add(value);
        remove => _combatStartedEventsStore.Remove(value);
    }

    internal EventStore<AbilityActivation> _abilityActivatedEventsStore;

    public event NotifyEvent<AbilityActivation> FAbilityActivatedEvents
    {
        add => _abilityActivatedEventsStore.Add(value);
        remove => _abilityActivatedEventsStore.Remove(value);
    }

    // AoWHSMap.PAS:914
    public override void Dispose()
    {
        base.Dispose();
        _combatStartedEventsStore.Clear();
        _abilityActivatedEventsStore.Clear();
    }

    // AoWHSMap.PAS:907
    public CustomCombatMap(ECustomNode owner) : base(owner)
    {
        _combatStartedEventsStore = new();
        _abilityActivatedEventsStore = new();
    }

}
