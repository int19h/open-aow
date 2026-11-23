#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Issue 349 part B. Each spell raise site asks the engine input once, and the unattended input
// throws. The fixture engine carries the recording input.
[Collection("Engine")]
[EngineState]
public sealed class PlayerInputTests
{
    private sealed class MapScope : IDisposable
    {
        private readonly StrategicMap _previous = StrategicMap.Current;

        public MapScope()
        {
            Map = new StrategicMap(null, holder: EngineFixture.Engine);
            StrategicMap.Current = Map;
        }

        public StrategicMap Map { get; }

        public void Dispose() => StrategicMap.Current = _previous;
    }

    private sealed class TestSpell(CustomNode owner) : Spell(owner)
    {
        public override bool CanActivateCombat(Troop source, ref string reason) => true;
    }

    private sealed class TestDisjunction(CustomNode owner, EngineObject holder) : Disjunction(owner, holder: holder)
    {
        public override bool CanActivate(Troop source, ref string messageText) => true;
    }

    [Fact]
    public void UnattendedInputThrows()
    {
        UnitSpellCaster caster = new(null) { Holder = EngineFixture.Engine };
        _ = Assert.Throws<InvalidOperationException>(() => PlayerInput.Unattended.ChooseSpellTargets(caster));
        // The refused call released the handed caster, so this release lands below zero.
        Assert.Equal(-1, caster.Release());

        CombatSpellCaster combatCaster = new(null) { Holder = EngineFixture.Engine };
        _ = Assert.Throws<InvalidOperationException>(() => PlayerInput.Unattended.ChooseCombatSpellTarget(combatCaster));
        Assert.Equal(-1, combatCaster.Release());
    }

    // The headless host input of a directed run: the cast stays accepted, no dialog opens and
    // nothing throws, as a raise with no subscriber behaved.
    [Fact]
    public void DirectedDisjunctionRunsWithoutThrowing()
    {
        GameEngine engine = GameEngine.Create(null, playerInput: new Headless.HeadlessPlayerInput());
        TestDisjunction spell = new(null, holder: engine);
        Troop source = new(null) { Holder = engine, Player = 2, UnitId = 11 };
        Assert.True(spell.Activate(source));
    }

    [Fact]
    public void CosmagicSurgeryCastAsksTheInputOnce()
    {
        using MapScope scope = new();
        EngineFixture.PlayerInput.Clear();
        CosmagicSurgery spell = new(null, holder: EngineFixture.Engine);
        Troop source = new(null) { Holder = EngineFixture.Engine, Player = 2, UnitId = 11 };
        Troop target = new(null) { Holder = EngineFixture.Engine, Player = 3, UnitId = 22 };
        using UnitUserList targets = new(null) { Holder = EngineFixture.Engine };
        _ = targets.AddUnit(target);
        Assert.True(spell.CastSpell(source, targets: targets));
        _ = Assert.Single(EngineFixture.PlayerInput.SpellCalls);
        Assert.Equal("CosmagicSurgerySpellCaster:59:11:CosmagicSurgery", EngineFixture.PlayerInput.SpellCalls[0]);
    }

    [Fact]
    public void DisjunctionActivateAsksTheInputOnce()
    {
        using MapScope scope = new();
        EngineFixture.PlayerInput.Clear();
        TestDisjunction spell = new(null, holder: EngineFixture.Engine);
        Troop source = new(null) { Holder = EngineFixture.Engine, Player = 2, UnitId = 11 };
        Assert.True(spell.Activate(source));
        _ = Assert.Single(EngineFixture.PlayerInput.SpellCalls);
        Assert.Equal("DisjunctionSpellCaster:51:11:Disjunction", EngineFixture.PlayerInput.SpellCalls[0]);
    }

    [Fact]
    public void RangeZeroUnitSpellActivateAsksTheInputOnce()
    {
        using MapScope scope = new();
        EngineFixture.PlayerInput.Clear();
        UnitSpell spell = new(null) { Holder = EngineFixture.Engine };
        Army army = new(null) { Holder = EngineFixture.Engine };
        Troop source = new(null) { Holder = EngineFixture.Engine, Player = 2, UnitId = 11 };
        source.SetOwner(army);
        Assert.True(spell.Activate(source));
        _ = Assert.Single(EngineFixture.PlayerInput.SpellCalls);
        Assert.Equal("UnitSpellCaster:0:11:UnitTargets", EngineFixture.PlayerInput.SpellCalls[0]);
    }

    [Fact]
    public void TargetPartySelectedAsksTheInputOnce()
    {
        using MapScope scope = new();
        EngineFixture.PlayerInput.Clear();
        UnitSpell spell = new(null) { Holder = EngineFixture.Engine };
        ArmyHexSprite party = new(null) { Holder = EngineFixture.Engine };
        Troop source = new(null) { Holder = EngineFixture.Engine, Player = 2, UnitId = 11 };
        source.SetOwner(party.Army);
        spell.TargetPartySelected(source, party: party);
        _ = Assert.Single(EngineFixture.PlayerInput.SpellCalls);
        Assert.Equal("UnitSpellCaster:0:11:UnitTargets", EngineFixture.PlayerInput.SpellCalls[0]);
    }

    [Fact]
    public void ActivateCombatAsksTheInputOnce()
    {
        using MapScope scope = new();
        EngineFixture.PlayerInput.Clear();
        TestSpell spell = new(null) { Holder = EngineFixture.Engine };
        Troop source = new(null) { Holder = EngineFixture.Engine, Player = 2, UnitId = 11 };
        Assert.True(spell.ActivateCombat(source));
        _ = Assert.Single(EngineFixture.PlayerInput.CombatCalls);
        Assert.Equal("CombatSpellCaster:0:11", EngineFixture.PlayerInput.CombatCalls[0]);
        Assert.Empty(EngineFixture.PlayerInput.SpellCalls);
    }

    [Fact]
    public void EveryCasterKindHasAScreenOrNoView()
    {
        Assert.False(MapEventsCatalogHasNone());
        foreach (EngineClass engineClass in EngineFixture.Engine.GetDescendantClasses(typeof(SpellCaster)))
        {
            SpellCaster caster = (SpellCaster)engineClass.CreateInstance(null, holder: EngineFixture.Engine);
            try
            {
                bool hasScreen = UI.Dialogs.MapEvents.SpellCasterScreens.TryGet(caster.InputView, out UI.Dialogs.MapEvents.SpellCasterScreen screen);
                Assert.True(caster.InputView == SpellCasterView.None
                    || (hasScreen && screen.CasterType.IsAssignableFrom(caster.GetType())),
                    userMessage: $"{caster.GetType().Name} view {caster.InputView.Name} has no screen for its kind");
            }
            finally
            {
                _ = caster.Release();
            }
        }
    }

    private static bool MapEventsCatalogHasNone()
        => UI.Dialogs.MapEvents.SpellCasterScreens.TryGet(SpellCasterView.None, out _);
}
