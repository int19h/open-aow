using Aow.Game.Magic.Abilities;
using Aow.Game.Units;

namespace Aow.Units.Tests;

[Collection("Engine")]
[EngineState]
public sealed class UnitSelectorTests
{
    private sealed class WoundedHero : THero
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
        Aow.Game.Magic.Abilities.AoWEUnit.AoWHSSet = null!;
        var units = new TUnitUserList(null!);
        var selector = new THealingUnitSelector();
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
