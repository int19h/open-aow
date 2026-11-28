using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Second-engine pack ownership (issue 335, decision of 2026-11-24). Removing strike from an owner
// walks the ability control of the owner's engine. The strike ability itself and the legacy global
// both belong to another engine, whose pack has no strike enhancement.
[Collection("Engine")]
[EngineState]
public sealed class StrikeAbilityOwnershipTests
{
    private const int StrikeAbilityId = 21;

    [Fact]
    public void RemovalWalksThePackOfTheOwnersEngine()
    {
        GameEngine engine = EngineFixture.Engine;
        AbilityControl control = engine.GamePack.AbilityControl;
        int enhancementId = control.Abilities.FindIndex(ability => ability is StrikeEnhancementAbility);
        Assert.True(enhancementId >= 0);
        AbilityOwner owner = new(null) { Holder = engine };
        owner.SetAbilityCount(control.Abilities.Count);
        owner.SetAbilitySet(StrikeAbilityId, value: 1);
        owner.SetAbilitySet(enhancementId, value: 1);

        GameEngine otherEngine = new(null);
        GamePack otherPack = GamePack.Create(null, holder: otherEngine);
        otherEngine.GamePack = otherPack;
        StrikeAbility otherStrike = new(null, holder: otherEngine);
        otherPack.AbilityControl.RegisterAbility(otherStrike);
        Assert.Same(otherEngine, otherPack.GetEngine());
        Assert.Same(otherEngine, otherStrike.GetEngine());
        // The strike was built while the global held the fixture's pack. Its image sequences still
        // take the image library manager of its own engine's pack.
        Assert.Same(otherPack.ImageLibraryManager, otherStrike.ImageSequences.ImageLibraryManager);
        Assert.NotSame(engine.GamePack.ImageLibraryManager, otherStrike.ImageSequences.ImageLibraryManager);
        GamePack? previousPack = GamePack.Current;
        GamePack.Current = otherPack;
        try
        {
            Assert.True(otherStrike.Remove(owner));
        }
        finally
        {
            GamePack.Current = previousPack;
        }

        Assert.False(owner.GetAbilitySetAt(StrikeAbilityId));
        Assert.False(owner.GetAbilitySetAt(enhancementId));
        Assert.Equal(StrikeAbilityId + 1, otherPack.AbilityControl.Abilities.Count);
        Assert.Same(otherStrike, Assert.Single(otherPack.AbilityControl.Abilities, ability => ability is not null));
        otherPack.Dispose();
    }
}
