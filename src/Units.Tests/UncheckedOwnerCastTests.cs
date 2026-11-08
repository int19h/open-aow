using Aow.Units.AoWE;
using Aow.Units.NetworkE;
using Aow.Units.PassiveAb;
using Game = Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Tests;

// Ability owners are not always units, and a unit id is not always a hero. These cases drive the
// sites that used to throw InvalidCastException for the other class.
[Collection("Engine")]
[EngineState]
public sealed class UncheckedOwnerCastTests
{
    private sealed class MapScope : IDisposable
    {
        private readonly TAoWHSMap previous = Game.AoWHSMap;

        public MapScope()
        {
            Map = new TAoWHSMap(null!);
            Game.AoWHSMap = Map;
        }

        public TAoWHSMap Map { get; }

        public void Dispose() => Game.AoWHSMap = previous;
    }

    // The list is the owner without going through AddChild.
    private sealed class PlacedUnit : TAbstractUnit
    {
        public PlacedUnit(int id, TUnitList list) : base(null!)
        {
            FUnitID = id;
            FOwner = list;
        }
    }

    [Fact]
    public void PossessStrengthLeavesAnItemAloneAndComputesAUnit()
    {
        var ability = new TPossessAbility(null!);
        var item = new TItem(null!);
        var unit = new TAbstractUnit(null!);
        int strength = 7;
        var types = TSet<TDamageType>.Of(TDamageType.dtFire);
        ability.GetOffensiveStrength(item, ref strength, ref types);
        Assert.Equal(7, strength);
        Assert.Equal(TSet<TDamageType>.Of(TDamageType.dtFire), types);

        ability.GetOffensiveStrength(unit, ref strength, ref types);
        Assert.Equal(0, strength);
        Assert.Equal(TSet<TDamageType>.Of(TDamageType.dtNone), types);
    }

    [Fact]
    public void EnchantedWeaponPriorityIsZeroForAnItem()
    {
        var ability = new TEnchantedWeaponAbility(null!);
        Assert.Equal(0, ability.GetAIUpkeepPriority(new TItem(null!)));
        Assert.Equal(225, ability.GetAIUpkeepPriority(new TAbstractUnit(null!)));
    }

    [Fact]
    public void CrusaderRemoveSkipsKilledOnAnItem()
    {
        var ability = new TCrusaderAbility(null!);
        Assert.False(ability.Remove(new TItem(null!)));
    }

    [Fact]
    public void HealingActivateSkipsSetupForAnItemAndRecordsAUnit()
    {
        using var scope = new MapScope();
        var list = new TUnitUserList(null!);
        var unit = new PlacedUnit(9, list);
        int id = -1;
        int source = -1;
        bool fired = false;
        scope.Map.FOnActivateAbility.Add(sender =>
        {
            var info = (TUnitSelectorAAI)sender;
            fired = true;
            id = info.FUnitID;
            source = info.FUnitSelector.FSourceUnitID;
        });
        var ability = new THealingAbility(null!);
        string reason = "";
        Assert.True(ability.Activate(new TItem(null!), ref reason));
        Assert.True(fired);
        Assert.Equal(0, id);
        Assert.Equal(0, source);

        fired = false;
        Assert.True(ability.Activate(unit, ref reason));
        Assert.True(fired);
        Assert.Equal(9, id);
        Assert.Equal(9, source);
    }

    [Fact]
    public void RequiredManaOfANonHeroIsTheMissingHeroResult()
    {
        using var scope = new MapScope();
        var unit = new TAbstractUnit(null!) { FUnitID = 42 };
        scope.Map.FUnitControl.RegisterUnit(unit);
        var caster = new TUnitSpellCaster(null!) { FCasterSourceID = 42 };
        Assert.Equal(-1, caster.GetRequiredMana());
        Assert.True(caster.Valid());
        caster.FCasterSourceID = 99;
        Assert.Equal(-1, caster.GetRequiredMana());
        Assert.False(caster.Valid());
    }

    [Fact]
    public void UpgradeTokenSkipsANonHeroAndStillUpgradesAHero()
    {
        using var scope = new MapScope();
        var unit = new TAbstractUnit(null!) { FUnitID = 42 };
        var hero = new THero(null!)
        {
            FUnitID = 43,
            FPlayer = 0,
            FHitPoints = 1,
            FUpgradePending = true,
            FHeroResource = new THeroResource(null!),
        };
        scope.Map.FUnitControl.RegisterUnit(unit);
        scope.Map.FUnitControl.RegisterUnit(hero);

        var skipped = new THeroUpgradeTE(null!) { FUnitID = 42, FPlayer = 0, FAttack = 1 };
        Assert.Equal(TExecuteResult.erDone, skipped.Execute(null!));
        Assert.Null(scope.Map.FUnitControl.FindUnit<THero>(42));

        var applied = new THeroUpgradeTE(null!) { FUnitID = 43, FPlayer = 0, FAttack = 1 };
        Assert.Equal(TExecuteResult.erDone, applied.Execute(null!));
        Assert.False(hero.FUpgradePending);
        Assert.Equal(1, hero.FUnitAttack);
    }

    [Fact]
    public void UpdateTokenHaltsForANonHeroAndAMissingId()
    {
        using var scope = new MapScope();
        var unit = new TAbstractUnit(null!) { FUnitID = 42 };
        scope.Map.FUnitControl.RegisterUnit(unit);
        var token = new THeroUpdateTE(null!) { FUnitID = 42, FMode = THeroUpdateMode.hmCancelCasting };
        Assert.Equal(TExecuteResult.erDoneHalt, token.Execute(null!));
        token.FUnitID = 99;
        Assert.Equal(TExecuteResult.erDoneHalt, token.Execute(null!));
    }
}
