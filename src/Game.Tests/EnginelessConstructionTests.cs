using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// An ability or a spell is built for one engine (issue 335). With no holder and no owner that
// reaches an engine, construction refuses, even while the legacy global holds a pack.
[Collection("Engine")]
[EngineState]
public sealed class EnginelessConstructionTests
{
    [Fact]
    public void AnAbilityOrSpellWithNoEngineRefusesConstruction()
    {
        Assert.NotNull(GamePack.Current);

        InvalidOperationException ability = Assert.Throws<InvalidOperationException>(() => new Ability(null, holder: null!));
        InvalidOperationException spell = Assert.Throws<InvalidOperationException>(() => new Spell(null, holder: null!));

        Assert.Equal("Ability is built with no holder or owner that reaches an engine.", ability.Message);
        Assert.Equal("Spell is built with no holder or owner that reaches an engine.", spell.Message);
    }
}
