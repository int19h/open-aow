using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Abilities.Passive;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Units.Tests;

// Ability owners are not always units, and a unit id is not always a hero. These cases drive the
// sites that used to throw InvalidCastException for the other class.
[Collection("Engine")]
[EngineState]
public sealed class UncheckedOwnerCastTests
{
    private sealed class MapScope : IDisposable
    {
        private readonly AoWHSMap previous = AoWHSMap.Current;

        public MapScope()
        {
            Map = new AoWHSMap(null!);
            AoWHSMap.Current = Map;
        }

        public AoWHSMap Map { get; }

        public void Dispose() => AoWHSMap.Current = previous;
    }

    // The list is the owner without going through AddChild.
    private sealed class PlacedUnit : AbstractUnit
    {
        public PlacedUnit(int id, UnitList list) : base(null!)
        {
            FUnitID = id;
            FOwner = list;
        }
    }

    private sealed class RecordingUnit : AbstractUnit
    {
        public RecordingUnit() : base(null!)
        {
        }

        public int KilledCount { get; private set; }

        public override void Killed()
        {
            KilledCount += 1;
        }
    }

    [Fact]
    public void PossessStrengthLeavesAnItemAloneAndComputesAUnit()
    {
        var ability = new PossessAbility(null!);
        var item = new Item(null!);
        var unit = new AbstractUnit(null!);
        int strength = 7;
        var types = Set<DamageType>.Of(DamageType.Fire);
        ability.GetOffensiveStrength(item, ref strength, ref types);
        Assert.Equal(7, strength);
        Assert.Equal(Set<DamageType>.Of(DamageType.Fire), types);

        ability.GetOffensiveStrength(unit, ref strength, ref types);
        Assert.Equal(0, strength);
        Assert.Equal(Set<DamageType>.Of(DamageType.None), types);
    }

    [Fact]
    public void EnchantedWeaponPriorityIsZeroForAnItem()
    {
        var ability = new EnchantedWeaponAbility(null!);
        Assert.Equal(0, ability.GetAIUpkeepPriority(new Item(null!)));
        Assert.Equal(225, ability.GetAIUpkeepPriority(new AbstractUnit(null!)));
    }

    [Fact]
    public void CrusaderRemoveSkipsKilledOnAnItem()
    {
        var ability = new CrusaderAbility(null!);
        Assert.False(ability.Remove(new Item(null!)));

        var unit = new RecordingUnit();
        var data = new DurationAbilityData(null!);
        data.FAbilityID = (ushort)ability.FAbilityID;
        unit.AddAbilityData(data);
        Assert.True(ability.Remove(unit));
        Assert.Equal(1, unit.KilledCount);
    }

    [Fact]
    public void HealingActivateSkipsSetupForAnItemAndRecordsAUnit()
    {
        using var scope = new MapScope();
        var list = new UnitUserList(null!);
        var unit = new PlacedUnit(9, list);
        int id = -1;
        int source = -1;
        bool fired = false;
        scope.Map.FOnActivateAbility += sender =>
        {
            var info = (UnitSelectorAAI)sender;
            fired = true;
            id = info.FUnitID;
            source = info.FUnitSelector.FSourceUnitID;
        };
        var ability = new HealingAbility(null!);
        string reason = "";
        Assert.True(ability.Activate(new Item(null!), ref reason));
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
        var unit = new AbstractUnit(null!) { FUnitID = 42 };
        scope.Map.FUnitControl.RegisterUnit(unit);
        var caster = new UnitSpellCaster(null!) { FCasterSourceID = 42 };
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
        var unit = new Unit(null!) { FUnitID = 42, FAbAttack = 4 };
        var hero = new Hero(null!)
        {
            FUnitID = 43,
            FPlayer = 0,
            FHitPoints = 1,
            FUnitAttack = 0,
            FUpgradePending = true,
            FHeroResource = new HeroResource(null!),
        };
        scope.Map.FUnitControl.RegisterUnit(unit);
        scope.Map.FUnitControl.RegisterUnit(hero);

        var skipped = new HeroUpgradeTE(null!) { FUnitID = 42, FPlayer = 0, FAttack = 1 };
        Assert.Equal(ExecuteResult.Done, skipped.Execute(null!));
        Assert.Null(scope.Map.FUnitControl.FindUnit<Hero>(42));
        Assert.Equal(4, unit.FAbAttack);
        Assert.Equal(0, hero.FUnitAttack);
        Assert.True(hero.FUpgradePending);

        var applied = new HeroUpgradeTE(null!) { FUnitID = 43, FPlayer = 0, FAttack = 1 };
        Assert.Equal(ExecuteResult.Done, applied.Execute(null!));
        Assert.False(hero.FUpgradePending);
        Assert.Equal(1, hero.FUnitAttack);
    }

    [Fact]
    public void UpdateTokenHaltsForANonHeroAndAMissingId()
    {
        using var scope = new MapScope();
        var unit = new AbstractUnit(null!) { FUnitID = 42 };
        scope.Map.FUnitControl.RegisterUnit(unit);
        var token = new HeroUpdateTE(null!) { FUnitID = 42, FMode = HeroUpdateTE.HeroUpdateMode.CancelCasting };
        Assert.Equal(ExecuteResult.DoneHalt, token.Execute(null!));

        token.FUnitID = 99;
        Assert.Throws<NullReferenceException>(() => token.Execute(null!));
        string shown = "";
        Action<string> saved = Runtime.MessageHandler;
        Runtime.MessageHandler = message => shown = message;
        try
        {
            var executer = new TokenExecuter(null!);
            Assert.Equal(ExecuteResult.Done, executer.ExecuteTokenEvent(token));
            Assert.Contains("Exception occured during ", shown);
        }
        finally
        {
            Runtime.MessageHandler = saved;
        }
    }

    [Fact]
    public void DisjunctionWardAndCheatRunForANonHero()
    {
        using var scope = new MapScope();
        var player = scope.Map.FPlayers.GetPlayers(1);
        player.FPlayerType = PlayerType.Human;
        player.FMagicControl.FCheatEnabled = true;
        var unit = new AbstractUnit(null!) { FUnitID = 42, FPlayer = 1 };
        scope.Map.FUnitControl.RegisterUnit(unit);
        var enchantment = new GlobalEnchantment(null!)
        {
            FEnchantmentID = 7,
            FEnchantmentType = GlobalEnchantmentType.Flood,
        };
        scope.Map.FGlobalMagicControl.FEnchantments.FChildren.Add(enchantment);
        scope.Map.FGlobalMagicControl.FActiveEnchantments = Set<GlobalEnchantmentType>.Of(GlobalEnchantmentType.SpellWard);
        var caster = new DisjunctionSpellCaster(null!) { FCasterSourceID = 42, FEnchantmentID = 7 };
        string reason = "";
        Assert.False(caster.CanCast(ref reason));
        Assert.Equal(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(DisjunctionSpellCaster.CannotDispelWhenSpellWardIsActiveRStr), reason);

        scope.Map.FGlobalMagicControl.FActiveEnchantments = Set<GlobalEnchantmentType>.Of();
        Assert.True(caster.CanCast(ref reason));
        Assert.True(caster.GetEnoughMana());
    }
}
