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
        private readonly StrategicMap _previous = StrategicMap.Current;

        public MapScope()
        {
            Map = new StrategicMap(null, holder: EngineFixture.Engine);
            StrategicMap.Current = Map;
        }

        public StrategicMap Map { get; }

        public void Dispose() => StrategicMap.Current = _previous;
    }

    // The list is the owner without going through AddChild.
    private sealed class PlacedUnit : Troop
    {
        public PlacedUnit(int id, UnitUserList list) : base(null)
        {
            UnitId = id;
            SetOwnerWithoutNotify(list);
        }
    }

    private sealed class RecordingUnit : Troop
    {
        public RecordingUnit() : base(null)
        {
        }

        public int KilledCount { get; private set; }

        public override void Killed() => KilledCount += 1;
    }

    [Fact]
    public void PossessStrengthLeavesAnItemAloneAndComputesAUnit()
    {
        PossessAbility ability = new(null, holder: EngineFixture.Engine);
        Item item = new(null) { Holder = EngineFixture.Engine };
        Troop troop = new(null) { Holder = EngineFixture.Engine };
        int strength = 7;
        DamageTypes types = DamageTypes.Fire;
        ability.GetOffensiveStrength(item, ref strength, ref types);
        Assert.Equal(7, strength);
        Assert.Equal(DamageTypes.Fire, types);

        ability.GetOffensiveStrength(troop, ref strength, ref types);
        Assert.Equal(0, strength);
        Assert.Equal(DamageTypes.NonSpecific, types);
    }

    [Fact]
    public void EnchantedWeaponPriorityIsZeroForAnItem()
    {
        EnchantedWeaponAbility ability = new(null!, holder: EngineFixture.Engine);
        Assert.Equal(0, ability.GetAIUpkeepPriority(new Item(null!) { Holder = EngineFixture.Engine }));
        Assert.Equal(225, ability.GetAIUpkeepPriority(new Troop(null!) { Holder = EngineFixture.Engine }));
    }

    [Fact]
    public void CrusaderRemoveSkipsKilledOnAnItem()
    {
        CrusaderAbility ability = new(null!, holder: EngineFixture.Engine);
        Assert.False(ability.Remove(new Item(null!) { Holder = EngineFixture.Engine }));

        RecordingUnit unit = new();
        DurationAbilityData data = new(null!)
        {
            Holder = EngineFixture.Engine,
            AbilityId = (ushort)ability.AbilityId
        };
        unit.AddAbilityData(data);
        Assert.True(ability.Remove(unit));
        Assert.Equal(1, unit.KilledCount);
    }

    [Fact]
    public void HealingActivateSkipsSetupForAnItemAndRecordsAUnit()
    {
        using MapScope scope = new();
        PlacedUnit unit = new(list: new UnitUserList(null) { Holder = EngineFixture.Engine }, id: 9);
        int id = -1;
        int source = -1;
        bool fired = false;
        scope.Map.AbilityActivated += sender =>
        {
            UnitSelectionActivation info = (UnitSelectionActivation)sender;
            fired = true;
            id = info.UnitId;
            source = info.UnitSelector.SourceUnitId;
        };
        HealingAbility ability = new(null, holder: EngineFixture.Engine);
        string reason = "";
        Assert.True(ability.Activate(new Item(null) { Holder = EngineFixture.Engine }, ref reason));
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
        using MapScope scope = new();
        Troop troop = new(null) { Holder = EngineFixture.Engine, UnitId = 42 };
        scope.Map.UnitControl.RegisterUnit(troop);
        UnitSpellCaster caster = new(null) { Holder = EngineFixture.Engine, CasterSourceId = 42 };
        Assert.Equal(-1, caster.GetRequiredMana());
        Assert.True(caster.Valid());
        caster.CasterSourceId = 99;
        Assert.Equal(-1, caster.GetRequiredMana());
        Assert.False(caster.Valid());
    }

    [Fact]
    public void UpgradeTokenSkipsANonHeroAndStillUpgradesAHero()
    {
        using MapScope scope = new();
        Unit unit = new(null) { Holder = EngineFixture.Engine, UnitId = 42, AbilityAttack = 4 };
        Hero hero = new(null)
        {
            Holder = EngineFixture.Engine,
            UnitId = 43,
            Player = 0,
            HitPoints = 1,
            UnitAttack = 0,
            UpgradePending = true,
            HeroResource = new HeroResource(null) { Holder = EngineFixture.Engine },
        };
        scope.Map.UnitControl.RegisterUnit(unit);
        scope.Map.UnitControl.RegisterUnit(hero);

        HeroUpgradeTokenEvent skipped = new(null) { Holder = EngineFixture.Engine, UnitId = 42, Player = 0, Attack = 1 };
        Assert.Equal(ExecuteResult.Done, skipped.Execute(null));
        Assert.Null(scope.Map.UnitControl.FindUnit<Hero>(42));
        Assert.Equal(4, unit.AbilityAttack);
        Assert.Equal(0, hero.UnitAttack);
        Assert.True(hero.UpgradePending);

        HeroUpgradeTokenEvent applied = new(null) { Holder = EngineFixture.Engine, UnitId = 43, Player = 0, Attack = 1 };
        Assert.Equal(ExecuteResult.Done, applied.Execute(null));
        Assert.False(hero.UpgradePending);
        Assert.Equal(1, hero.UnitAttack);
    }

    [Fact]
    public void UpdateTokenHaltsForANonHeroAndAMissingId()
    {
        using MapScope scope = new();
        Troop troop = new(null) { Holder = EngineFixture.Engine, UnitId = 42 };
        scope.Map.UnitControl.RegisterUnit(troop);
        HeroUpdateTokenEvent token = new(null) { Holder = EngineFixture.Engine, UnitId = 42, Mode = HeroUpdateTokenEventMode.CancelCasting };
        Assert.Equal(ExecuteResult.DoneHalt, token.Execute(null));

        token.UnitId = 99;
        _ = Assert.Throws<NullReferenceException>(() => token.Execute(null));
        string shown = "";
        Action<string> saved = LegacyApplication.MessageHandler;
        LegacyApplication.MessageHandler = message => shown = message;
        try
        {
            TokenExecutor executor = new(null);
            Assert.Equal(ExecuteResult.Done, executor.ExecuteTokenEvent(token));
            Assert.Contains("Exception occured during ", shown);
        }
        finally
        {
            LegacyApplication.MessageHandler = saved;
        }
    }

    [Fact]
    public void DisjunctionWardAndCheatRunForANonHero()
    {
        using MapScope scope = new();
        Player player = scope.Map.Players.GetPlayers(1);
        player.PlayerType = PlayerType.Human;
        player.MagicControl.CheatEnabled = true;
        Troop troop = new(null) { Holder = EngineFixture.Engine, UnitId = 42, Player = 1 };
        scope.Map.UnitControl.RegisterUnit(troop);
        GlobalEnchantment enchantment = new(null)
        {
            Holder = EngineFixture.Engine,
            EnchantmentId = 7,
            EnchantmentType = GlobalEnchantmentType.Flood,
        };
        _ = scope.Map.GlobalMagicControl.Enchantments.AddChild(enchantment);
        scope.Map.GlobalMagicControl.ActiveEnchantments = GlobalEnchantmentTypes.SpellWard;
        DisjunctionSpellCaster caster = new(null) { Holder = EngineFixture.Engine, CasterSourceId = 42, EnchantmentId = 7 };
        string reason = "";
        Assert.False(caster.CanCast(ref reason));
        Assert.Equal(AbilityRules.TranslateResourceString(EngineFixture.Engine, S: DisjunctionSpellCaster.CannotDispelWhenSpellWardIsActiveText), reason);

        scope.Map.GlobalMagicControl.ActiveEnchantments = GlobalEnchantmentTypes.None;
        Assert.True(caster.CanCast(ref reason));
        Assert.True(caster.GetEnoughMana());
    }
}
