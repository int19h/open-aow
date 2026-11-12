using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Abilities.Passive;
using Aow.Game.Magic.Spells;

using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

// Ability owners are not always units, and a unit id is not always a hero. These cases drive the
// sites that used to throw InvalidCastException for the other class.
[Collection("Engine")]
[EngineState]
public sealed class UncheckedOwnerCastTests
{
    private sealed class MapScope : IDisposable
    {
        private readonly AoWHSMap _previous = AoWHSMap.Current;

        public MapScope()
        {
            Map = new AoWHSMap(null!);
            AoWHSMap.Current = Map;
        }

        public AoWHSMap Map { get; }

        public void Dispose() => AoWHSMap.Current = _previous;
    }

    // The list is the owner without going through AddChild.
    private sealed class PlacedUnit : Troop
    {
        public PlacedUnit(int id, UnitList list) : base(null!)
        {
            UnitId = id;
            _owner = list;
        }
    }

    private sealed class RecordingUnit : Troop
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
        var troop = new Troop(null!);
        int strength = 7;
        var types = Set<DamageType>.Of(DamageType.Fire);
        ability.GetOffensiveStrength(item, ref strength, ref types);
        Assert.Equal(7, strength);
        Assert.Equal(Set<DamageType>.Of(DamageType.Fire), types);

        ability.GetOffensiveStrength(troop, ref strength, ref types);
        Assert.Equal(0, strength);
        Assert.Equal(Set<DamageType>.Of(DamageType.None), types);
    }

    [Fact]
    public void EnchantedWeaponPriorityIsZeroForAnItem()
    {
        var ability = new EnchantedWeaponAbility(null!);
        Assert.Equal(0, ability.GetAIUpkeepPriority(new Item(null!)));
        Assert.Equal(225, ability.GetAIUpkeepPriority(new Troop(null!)));
    }

    [Fact]
    public void CrusaderRemoveSkipsKilledOnAnItem()
    {
        var ability = new CrusaderAbility(null!);
        Assert.False(ability.Remove(new Item(null!)));

        var unit = new RecordingUnit();
        var data = new DurationAbilityData(null!);
        data.AbilityId = (ushort)ability.AbilityId;
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
            var info = (UnitSelectionActivation)sender;
            fired = true;
            id = info.UnitId;
            source = info.UnitSelector.SourceUnitId;
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
        var troop = new Troop(null!) { UnitId = 42 };
        scope.Map.UnitControl.RegisterUnit(troop);
        var caster = new UnitSpellCaster(null!) { CasterSourceId = 42 };
        Assert.Equal(-1, caster.GetRequiredMana());
        Assert.True(caster.Valid());
        caster.CasterSourceId = 99;
        Assert.Equal(-1, caster.GetRequiredMana());
        Assert.False(caster.Valid());
    }

    [Fact]
    public void UpgradeTokenSkipsANonHeroAndStillUpgradesAHero()
    {
        using var scope = new MapScope();
        var unit = new Unit(null!) { UnitId = 42, AbilityAttack = 4 };
        var hero = new Hero(null!)
        {
            UnitId = 43,
            Player = 0,
            HitPoints = 1,
            UnitAttack = 0,
            UpgradePending = true,
            HeroResource = new HeroResource(null!),
        };
        scope.Map.UnitControl.RegisterUnit(unit);
        scope.Map.UnitControl.RegisterUnit(hero);

        var skipped = new HeroUpgradeTokenEvent(null!) { UnitId = 42, Player = 0, Attack = 1 };
        Assert.Equal(ExecuteResult.Done, skipped.Execute(null!));
        Assert.Null(scope.Map.UnitControl.FindUnit<Hero>(42));
        Assert.Equal(4, unit.AbilityAttack);
        Assert.Equal(0, hero.UnitAttack);
        Assert.True(hero.UpgradePending);

        var applied = new HeroUpgradeTokenEvent(null!) { UnitId = 43, Player = 0, Attack = 1 };
        Assert.Equal(ExecuteResult.Done, applied.Execute(null!));
        Assert.False(hero.UpgradePending);
        Assert.Equal(1, hero.UnitAttack);
    }

    [Fact]
    public void UpdateTokenHaltsForANonHeroAndAMissingId()
    {
        using var scope = new MapScope();
        var troop = new Troop(null!) { UnitId = 42 };
        scope.Map.UnitControl.RegisterUnit(troop);
        var token = new HeroUpdateTokenEvent(null!) { UnitId = 42, Mode = HeroUpdateTokenEventMode.CancelCasting };
        Assert.Equal(ExecuteResult.DoneHalt, token.Execute(null!));

        token.UnitId = 99;
        Assert.Throws<NullReferenceException>(() => token.Execute(null!));
        string shown = "";
        Action<string> saved = Runtime.MessageHandler;
        Runtime.MessageHandler = message => shown = message;
        try
        {
            var executor = new TokenExecutor(null!);
            Assert.Equal(ExecuteResult.Done, executor.ExecuteTokenEvent(token));
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
        var player = scope.Map.Players.GetPlayers(1);
        player.PlayerType = PlayerType.Human;
        player.MagicControl.CheatEnabled = true;
        var troop = new Troop(null!) { UnitId = 42, Player = 1 };
        scope.Map.UnitControl.RegisterUnit(troop);
        var enchantment = new GlobalEnchantment(null!)
        {
            _enchantmentId = 7,
            EnchantmentType = GlobalEnchantmentType.Flood,
        };
        scope.Map.GlobalMagicControl.Enchantments._children.Add(enchantment);
        scope.Map.GlobalMagicControl.ActiveEnchantments = Set<GlobalEnchantmentType>.Of(GlobalEnchantmentType.SpellWard);
        var caster = new DisjunctionSpellCaster(null!) { CasterSourceId = 42, EnchantmentId = 7 };
        string reason = "";
        Assert.False(caster.CanCast(ref reason));
        Assert.Equal(AbilityRules.TranslateResourceString(DisjunctionSpellCaster.s_cannotDispelWhenSpellWardIsActiveText), reason);

        scope.Map.GlobalMagicControl.ActiveEnchantments = Set<GlobalEnchantmentType>.Of();
        Assert.True(caster.CanCast(ref reason));
        Assert.True(caster.GetEnoughMana());
    }
}
