using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed class UnitSelectorTests
{
    private sealed class WoundedHero : Hero
    {
        private readonly sbyte hits;
        private readonly sbyte hitPoints;

        internal WoundedHero(int id, sbyte hits, sbyte hitPoints) : base(null!)
        {
            FUnitID = id;
            this.hits = hits;
            this.hitPoints = hitPoints;
        }

        public override int GetUnitGFXResourceIndex() => 0;

        public override sbyte GetHits() => hits;

        public override sbyte GetHitPoints() => hitPoints;
    }

    // Ability.PAS 686 hard-casts every listed unit to TUnit, and a hero passes through that cast to
    // the virtual calls. Healing a stack that holds a hero once crashed with an invalid cast here.
    [Fact]
    public void HealingSetupAcceptsHeroesAndSelectsOnlyTheWounded()
    {
        AoWHSSet.Current = null!;
        var units = new UnitUserList(null!);
        var selector = new HealingUnitSelector();
        try
        {
            units.AddUnit(new WoundedHero(1, 10, 10));
            units.AddUnit(new WoundedHero(2, 4, 10));
            selector.Setup(null!, units);
            // The sort puts the higher unit id first, so the wounded hero is item 0.
            Assert.Equal(2, selector.FUnits.GetUnit(0).FUnitID);
            Assert.Equal(0b01, selector.FSelectable);
        }
        finally
        {
            selector.FUnits.Clear();
            units.Clear();
        }
    }
}
