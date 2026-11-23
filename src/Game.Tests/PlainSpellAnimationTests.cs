#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Game.Commands;
using Aow.Game.Magic.Spells;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Healing showers and tornado used to cast a plain animation to an image sequence. That cast
// always failed, so the path never ran. Each event now stores the plain animation the original
// constructs, and Process builds it and activates it.
[Collection("Engine")]
[EngineState]
public sealed class PlainSpellAnimationTests
{
    private sealed class MapScope : IDisposable
    {
        private readonly IDisposable _previousScope = EngineFixture.UseMap(StrategicMap.Current);

        public MapScope()
        {
            Map = new StrategicMap(null, holder: EngineFixture.Engine);
            EngineFixture.SetMap(Map);
            MapContainer container = new(null) { Holder = EngineFixture.Engine };
            MapLevel level = new(container);
            level.CreateMapFieldList(7, 7);
            container.Map = Map;
            Map.MapContainer = container;
        }

        public StrategicMap Map { get; }

        public void Dispose() => _previousScope.Dispose();
    }

    private static void Run(Spell spell, Action<Spell> process)
    {
        SpellControl control = EngineFixture.Engine.GamePack.SpellControl;
        int count = control.RegisteredSpells.Count;
        control.RegisterSpell(spell);
        try
        {
            process(spell);
        }
        finally
        {
            control.UnregisterSpell(spell);
            if (control.RegisteredSpells.Count > count)
            {
                control.RegisteredSpells.RemoveRange(count, count: control.RegisteredSpells.Count - count);
            }
        }
    }

    [Fact]
    public void HealingShowersBuildsAPlainAnimation()
    {
        using MapScope scope = new();
        Run(new HealingShowers(null, holder: EngineFixture.Engine), _ =>
        {
            Unit source = new(null) { Holder = EngineFixture.Engine, Player = 2 };
            HealingShowersTokenEvent showers = new(null) { Holder = EngineFixture.Engine };
            showers.Initialize(spellId: 73, playerIndex: 1, sourceId: 0, mana: 0);
            showers.SetLocation(x: 0, y: 0, layer: 0);
            int before = scope.Map.DynamicFixedVisibleHexSprite.Count;
            Assert.True(showers.Process(source, value: 0));
            Assert.Equal(before + 1, scope.Map.DynamicFixedVisibleHexSprite.Count);
        });
    }

    [Fact]
    public void TornadoBuildsAPlainAnimation()
    {
        using MapScope scope = new();
        Run(new Tornado(null, holder: EngineFixture.Engine), _ =>
        {
            Unit source = new(null) { Holder = EngineFixture.Engine, Player = 2 };
            TornadoTokenEvent tornado = new(null) { Holder = EngineFixture.Engine };
            tornado.Initialize(spellId: 68, playerIndex: 1, sourceId: 0, mana: 0);
            tornado.SetLocation(x: 0, y: 0, layer: 0);
            int before = scope.Map.DynamicFixedVisibleHexSprite.Count;
            Assert.True(tornado.Process(source, value: 0));
            Assert.Equal(before + 1, scope.Map.DynamicFixedVisibleHexSprite.Count);
        });
    }
}
