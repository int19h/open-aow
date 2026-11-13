using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class UnitSelectorTests
{
    private sealed class WoundedHero : Hero
    {
        private readonly sbyte _hits;
        private readonly sbyte _hitPoints;

        internal WoundedHero(int id, sbyte hits, sbyte hitPoints) : base(null!)
        {
            UnitId = id;
            this._hits = hits;
            this._hitPoints = hitPoints;
        }

        public override int GetUnitGraphicsResourceIndex() => 0;

        public override sbyte GetHits() => _hits;

        public override sbyte GetHitPoints() => _hitPoints;
    }

    // Ability.PAS 686 hard-casts every listed unit to TUnit, and a hero passes through that cast to
    // the virtual calls. Healing a stack that holds a hero once crashed with an invalid cast here.
    [Fact]
    public void HealingSetupAcceptsHeroesAndSelectsOnlyTheWounded()
    {
        GamePack.Current = null!;
        var units = new UnitUserList(null!) { Holder = EngineFixture.Engine };
        var selector = new HealingUnitSelector(EngineFixture.Engine);
        try
        {
            units.AddUnit(new WoundedHero(1, 10, 10));
            units.AddUnit(new WoundedHero(2, 4, 10));
            selector.Setup(null!, units);
            // The sort puts the higher unit id first, so the wounded hero is item 0.
            Assert.Equal(2, selector.Units.GetUnit(0).UnitId);
            Assert.Equal(0b01, selector.Selectable);
        }
        finally
        {
            selector.Units.Clear();
            units.Clear();
        }
    }
}
