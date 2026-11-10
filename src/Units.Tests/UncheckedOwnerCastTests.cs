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
        private readonly TAoWHSMap previous = Aow.Game.Magic.Abilities.AoWEUnit.AoWHSMap;

        public MapScope()
        {
            Map = new TAoWHSMap(null!);
            Aow.Game.Magic.Abilities.AoWEUnit.AoWHSMap = Map;
        }

        public TAoWHSMap Map { get; }

        public void Dispose() => Aow.Game.Magic.Abilities.AoWEUnit.AoWHSMap = previous;
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

    private sealed class RecordingUnit : TAbstractUnit
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

        var unit = new RecordingUnit();
        var data = new TDurationAbilityData(null!);
        data.FAbilityID = (ushort)ability.FAbilityID;
        unit.AddAbilityData(data);
        Assert.True(ability.Remove(unit));
        Assert.Equal(1, unit.KilledCount);
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
        scope.Map.FOnActivateAbility += sender =>
        {
            var info = (TUnitSelectorAAI)sender;
            fired = true;
            id = info.FUnitID;
            source = info.FUnitSelector.FSourceUnitID;
        };
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
        var unit = new TUnit(null!) { FUnitID = 42, FAbAttack = 4 };
        var hero = new THero(null!)
        {
            FUnitID = 43,
            FPlayer = 0,
            FHitPoints = 1,
            FUnitAttack = 0,
            FUpgradePending = true,
            FHeroResource = new THeroResource(null!),
        };
        scope.Map.FUnitControl.RegisterUnit(unit);
        scope.Map.FUnitControl.RegisterUnit(hero);

        var skipped = new THeroUpgradeTE(null!) { FUnitID = 42, FPlayer = 0, FAttack = 1 };
        Assert.Equal(TExecuteResult.erDone, skipped.Execute(null!));
        Assert.Null(scope.Map.FUnitControl.FindUnit<THero>(42));
        Assert.Equal(4, unit.FAbAttack);
        Assert.Equal(0, hero.FUnitAttack);
        Assert.True(hero.FUpgradePending);

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
        Assert.Throws<NullReferenceException>(() => token.Execute(null!));
        string shown = "";
        Action<string> saved = Runtime.MessageHandler;
        Runtime.MessageHandler = message => shown = message;
        try
        {
            var executer = new TTokenExecuter(null!);
            Assert.Equal(TExecuteResult.erDone, executer.ExecuteTokenEvent(token));
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
        player.FPlayerType = TPlayerType.ptHuman;
        player.FMagicControl.FCheatEnabled = true;
        var unit = new TAbstractUnit(null!) { FUnitID = 42, FPlayer = 1 };
        scope.Map.FUnitControl.RegisterUnit(unit);
        var enchantment = new TGlobalEnchantment(null!)
        {
            FEnchantmentID = 7,
            FEnchantmentType = TGlobalEnchantmentType.getFlood,
        };
        scope.Map.FGlobalMagicControl.FEnchantments.FChildren.Add(enchantment);
        scope.Map.FGlobalMagicControl.FActiveEnchantments = TSet<TGlobalEnchantmentType>.Of(TGlobalEnchantmentType.getSpellWard);
        var caster = new TDisjunctionSpellCaster(null!) { FCasterSourceID = 42, FEnchantmentID = 7 };
        string reason = "";
        Assert.False(caster.CanCast(ref reason));
        Assert.Equal(Aow.Game.Magic.Abilities.AoWEUnit.TranslateRStr(Aow.Game.Magic.Abilities.AoWEUnit.CannotDispelWhenSpellWardIsActiveRStr), reason);

        scope.Map.FGlobalMagicControl.FActiveEnchantments = TSet<TGlobalEnchantmentType>.Of();
        Assert.True(caster.CanCast(ref reason));
        Assert.True(caster.GetEnoughMana());
    }
}
