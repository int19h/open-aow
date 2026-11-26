using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Combat;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

// Issue 317 package 4. Each fact pins a converted Units or Session query on duplicates, an empty slot, and an empty list.
[Collection("Engine")]
[EngineState]
public sealed class LinqHandSessionTests
{
    [Fact]
    public void RemotePlayerListKeepsTheFirstRemoteIdAndThrowsOnANullSlot()
    {
        RemotePlayerList empty = new(null!) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, empty.IndexOfRemoteId(4));

        RemotePlayerList players = new(null!) { Holder = EngineFixture.Engine };
        RemotePlayer first = new(players) { RemoteId = 4 };
        RemotePlayer second = new(players) { RemoteId = 4 };
        Assert.Equal(0, players.IndexOfRemoteId(4));
        Assert.Equal(4, second.RemoteId);
        Assert.Same(first, players.GetPlayers(0));
        _ = players.AddChild(null);
        _ = Assert.Throws<NullReferenceException>(() => players.IndexOfRemoteId(5));
    }

    [Fact]
    public void SetupSettingsKeepTheFirstMatchAndThrowOnANullSlot()
    {
        SetupSettings empty = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, empty.IndexOfPlayer(PlayerId.Player4));
        Assert.Equal(-1, empty.IndexOfRemoteId(9));
        Assert.Equal(-1, empty.IndexOfRace(4));

        SetupSettings settings = new(null) { Holder = EngineFixture.Engine };
        PlayerSetupSettings first = new(settings)
        {
            PlayerId = PlayerId.Player4,
            Race = 4,
            Behavior = new SetupPlayerBehavior(PlayerType: 0, Flags: 9),
        };
        _ = new PlayerSetupSettings(settings)
        {
            PlayerId = PlayerId.Player4,
            Race = 4,
            Behavior = new SetupPlayerBehavior(PlayerType: 0, Flags: 9),
        };
        Assert.Equal(0, settings.IndexOfPlayer(PlayerId.Player4));
        Assert.Equal(0, settings.IndexOfRemoteId(9));
        Assert.Equal(0, settings.IndexOfRace(4));
        Assert.Same(first, settings.GetSettings(0));
        _ = settings.AddChild(null);
        _ = Assert.Throws<NullReferenceException>(() => settings.IndexOfPlayer(PlayerId.Player3));
        _ = Assert.Throws<NullReferenceException>(() => settings.IndexOfRemoteId(8));
        _ = Assert.Throws<NullReferenceException>(() => settings.IndexOfRace(3));
    }

    [Fact]
    public void RaceListCountsEnabledRacesAndSkipsANullSlot()
    {
        RaceList races = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(0, races.GetEnabledRaces());
        _ = races.AddChild(null);
        _ = new Race(races) { Enabled = false };
        _ = new Race(races) { Enabled = true };
        _ = new Race(races) { Enabled = true };
        Assert.Equal(2, races.GetEnabledRaces());
    }

    [Fact]
    public void RaceListRandomRaceDrawsOnlyFromEnabledRacesAndSkipsANullSlot()
    {
        RaceList races = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, races.GetRandomRace());
        _ = races.AddChild(null);
        _ = new Race(races) { Enabled = false, RaceIndex = 3 };
        Assert.Equal(-1, races.GetRandomRace());
        _ = new Race(races) { Enabled = true, RaceIndex = 7 };
        Assert.Equal(7, races.GetRandomRace());
    }

    [Fact]
    public void FaceListReturnsTheLastUnlinkedImageAndSkipsANullSlot()
    {
        FaceResourceList faces = new(null!) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, faces.IndexOfFaceImage(null!));
        _ = new FaceResource(faces);
        _ = faces.AddChild(null);
        _ = new FaceResource(faces);
        Assert.Equal(2, faces.IndexOfFaceImage(null!));
        Assert.Equal(-1, faces.IndexOfFaceImage(new LibraryImage(null)));
    }

    [Fact]
    public void HeroResourceListKeepsTheFirstRaceAndSkipsANullSlot()
    {
        HeroResourceList resources = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, resources.Find(race: 0, heroType: HeroType.Hero));
        _ = resources.AddChild(null);
        HeroResource first = new(resources);
        _ = new HeroResource(resources);
        Assert.Equal(1, resources.Find(race: 0, heroType: HeroType.Hero));
        Assert.Equal(-1, resources.Find(race: 0, heroType: HeroType.Leader));
        Assert.Equal(0, first.Race);
    }

    [Fact]
    public void ItemGraphicsListKeepsTheFirstTypeAndSkipsANullSlot()
    {
        ItemGFXList graphics = new(null!) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, graphics.FindItemTypeGraphics(ItemType.Ring));
        _ = graphics.AddChild(null);
        _ = new ItemGraphics(graphics) { ItemType = ItemType.Torso };
        _ = new ItemGraphics(graphics) { ItemType = ItemType.Ring };
        _ = new ItemGraphics(graphics) { ItemType = ItemType.Ring };
        Assert.Equal(2, graphics.FindItemTypeGraphics(ItemType.Ring));
        Assert.Equal(-1, graphics.FindItemTypeGraphics(ItemType.Scroll));
    }

    [Fact]
    public void UnitProductionListReturnsTheLastIndexAndThrowsOnANullSlot()
    {
        UnitProductionList empty = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, empty.IndexOf(3));

        UnitProductionList productions = new(null) { Holder = EngineFixture.Engine };
        _ = new UnitProduction(productions) { UnitIndex = 3 };
        _ = new UnitProduction(productions) { UnitIndex = 3 };
        Assert.Equal(1, productions.IndexOf(3));
        _ = productions.AddChild(null);
        _ = Assert.Throws<NullReferenceException>(() => productions.IndexOf(3));
    }

    [Fact]
    public void HeroListKeepsTheFirstId()
    {
        HeroList empty = new(null!) { Holder = EngineFixture.Engine };
        Assert.Null(empty.FindHero(4));

        HeroList heroes = new(null!) { Holder = EngineFixture.Engine };
        Hero first = new(null) { Holder = EngineFixture.Engine, UnitId = 4 };
        Hero second = new(null) { Holder = EngineFixture.Engine, UnitId = 4 };
        _ = heroes.AddChild(first);
        _ = heroes.AddChild(second);
        Assert.Same(first, heroes.FindHero(4));
    }

    [Fact]
    public void HeroControlLibrarySearchReturnsTheLastName()
    {
        HeroControl control = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, control.IndexOfLibrary(libraryName: "north", libraryId: 4));
        _ = control.RegisterHero(new Hero(null) { Holder = EngineFixture.Engine, LibraryId = 4, LibraryName = "north" });
        _ = control.RegisterHero(new Hero(null) { Holder = EngineFixture.Engine, LibraryId = 4, LibraryName = "north" });
        _ = control.RegisterHero(new Hero(null) { Holder = EngineFixture.Engine, LibraryId = 4, LibraryName = "south" });
        Assert.Equal(1, control.IndexOfLibrary(libraryName: "north", libraryId: 4));
    }

    [Fact]
    public void HeroControlMinFreeLevelKeepsTheSeedForAnOwnedHero()
    {
        HeroControl control = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(30, control.GetMinFreeHeroLevel());
        _ = control.RegisterHero(new Hero(null) { Holder = EngineFixture.Engine, Player = PlayerId.Player1 });
        Assert.Equal(30, control.GetMinFreeHeroLevel());
        _ = control.RegisterHero(new Hero(null) { Holder = EngineFixture.Engine });
        _ = control.RegisterHero(new Hero(null) { Holder = EngineFixture.Engine, Experience = 10_000 });
        Assert.Equal(1, control.GetMinFreeHeroLevel());
    }

    [Fact]
    public void ItemControlCreateUniqueIdSkipsAnExistingId()
    {
        ItemControl control = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(1, control.CreateUniqueId());
        Item item = new(null) { Holder = EngineFixture.Engine, ItemId = 2 };
        control.RegisterItem(item);
        Assert.Equal(2, item.ItemId);
        Assert.Equal(3, control.CreateUniqueId());
    }

    [Fact]
    public void HeroLibraryCreateUniqueIdSkipsAnExistingId()
    {
        HeroLibrary library = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(1, library.CreateUniqueLibraryId());
        Hero hero = new(null) { Holder = EngineFixture.Engine, LibraryId = 2 };
        _ = library.AddChild(hero);
        Assert.Equal(2, hero.LibraryId);
        Assert.Equal(3, library.CreateUniqueLibraryId());
    }

    [Fact]
    public void ItemLibraryCreateUniqueIdSkipsAnExistingId()
    {
        ItemLibrary library = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(1, library.CreateUniqueLibraryId());
        Item item = new(null) { Holder = EngineFixture.Engine, LibraryId = 2 };
        _ = library.AddChild(item);
        Assert.Equal(2, item.LibraryId);
        Assert.Equal(3, library.CreateUniqueLibraryId());
    }

    [Fact]
    public void PlayerCollectionRaceSearchStartsAfterTheIndependentSlotAndThrowsOnAnEmptyList()
    {
        PlayerCollection players = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(-1, players.RaceToPlayer(-1));
        Assert.Equal(1, players.RaceToPlayer(0));
        Assert.Equal(2, players.RaceToPlayer(1));
        players[2].PlayerRace = 0;
        Assert.Equal(1, players.RaceToPlayer(0));
        players.ClearChildren();
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => players.RaceToPlayer(0));

        PlayerCollection withHole = new(null) { Holder = EngineFixture.Engine };
        _ = withHole.AddChild(null);
        _ = Assert.Throws<NullReferenceException>(() => withHole.RaceToPlayer(99));
    }

    [Fact]
    public void PlayerCollectionReturnsTheLastPlayerTypeAndThrowsOnANullSlot()
    {
        PlayerCollection players = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(0, players.IndexOfPlayerType(PlayerType.Independent));
        Assert.Equal(2, players.IndexOfPlayerType(PlayerType.Human));
        players.ClearChildren();
        Assert.Equal(-1, players.IndexOfPlayerType(PlayerType.Human));

        PlayerCollection withHole = new(null) { Holder = EngineFixture.Engine };
        _ = withHole.AddChild(null);
        _ = Assert.Throws<NullReferenceException>(() => withHole.IndexOfPlayerType(PlayerType.Human));
    }

    [Fact]
    public void InventoryAndWornItemsTakeTheHighestAbilityCount()
    {
        HeroInventory inventory = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(0, inventory.GetAbilityCount());
        _ = new Item(inventory) { AbilityCount = -3 };
        _ = new Item(inventory) { AbilityCount = 2 };
        _ = new Item(inventory) { AbilityCount = 2 };
        Assert.Equal(2, inventory.GetAbilityCount());

        HeroItems worn = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(MoveTypes.None, worn.GetMoveTypes());
        Assert.Equal(DamageTypes.None, worn.GetImmunityTypes());
        Assert.Equal(DamageTypes.None, worn.GetProtectionTypes());
        Assert.Equal(0, worn.GetAbilityCount());
        _ = new Item(worn) { AbilityCount = 5 };
        Assert.Equal(5, worn.GetAbilityCount());
    }

    [Fact]
    public void ArmyGroupSearchReturnsTheLastMatchAndAnEmptyArmyStaysAtEight()
    {
        Army army = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(8, army.MaxSize());
        Assert.Equal(-1, army.Units.IndexOfBehavior(GroupBehavior.Fortify));
        Assert.Equal(-1, army.Units.IndexOfGroupId(7));
        _ = new Troop(army) { Holder = EngineFixture.Engine };
        Assert.Equal(8, army.MaxSize());
        FortifyAIGroup first = new(null!) { Holder = EngineFixture.Engine, GroupId = 7 };
        FortifyAIGroup second = new(null!) { Holder = EngineFixture.Engine, GroupId = 7 };
        _ = new Troop(army) { Holder = EngineFixture.Engine, Group = first };
        _ = new Troop(army) { Holder = EngineFixture.Engine, Group = second };
        Assert.Equal(2, army.Units.IndexOfBehavior(GroupBehavior.Fortify));
        Assert.Equal(2, army.Units.IndexOfGroupId(7));
        Assert.Equal(-1, army.Units.IndexOfBehavior(GroupBehavior.Scout));
        Assert.Equal(-1, army.Units.IndexOfGroupId(8));
    }

    [Fact]
    public void ArmyMaxSizeTakesTheSmallestCapacityPlusOneBelowTheSeed()
    {
        Army army = new(null) { Holder = EngineFixture.Engine };
        _ = new Troop(army) { Holder = EngineFixture.Engine };
        _ = new CarrierTroop(army, capacity: 9) { Holder = EngineFixture.Engine };
        Assert.Equal(8, army.MaxSize());
        _ = new CarrierTroop(army, capacity: 5) { Holder = EngineFixture.Engine };
        Assert.Equal(6, army.MaxSize());
        _ = new CarrierTroop(army, capacity: 3) { Holder = EngineFixture.Engine };
        _ = new CarrierTroop(army, capacity: 4) { Holder = EngineFixture.Engine };
        Assert.Equal(4, army.MaxSize());
    }

    [Fact]
    public void PowerSumThrowsOnOverflow()
    {
        PlayerMagicControl control = new(null) { Holder = EngineFixture.Engine };
        Assert.Equal(0, control.GetPower());
        _ = control.PowerSources.AddChild(new FixedPowerSource(int.MaxValue) { Holder = EngineFixture.Engine });
        Assert.Equal(int.MaxValue, control.GetPower());
        _ = control.PowerSources.AddChild(new FixedPowerSource(1) { Holder = EngineFixture.Engine });
        _ = Assert.Throws<OverflowException>(() => control.GetPower());
    }

    [Fact]
    public void IncomeIsWrittenPerSourceSoAThrowLeavesThePartialSum()
    {
        Player player = new(null) { Holder = EngineFixture.Engine };
        FixedIncomeSource failing = new(2) { Holder = EngineFixture.Engine };
        player.RegisterIncomeSource(failing);
        player.RegisterIncomeSource(new FixedIncomeSource(5) { Holder = EngineFixture.Engine });
        Assert.Equal(player.BaseIncome + 7, player.Income);
        failing.IsThrowing = true;
        _ = Assert.Throws<InvalidOperationException>(player.UpdateIncome);
        Assert.Equal(player.BaseIncome + 5, player.Income);
    }

    private sealed class CarrierTroop(CustomNode owner, int capacity) : Troop(owner)
    {
        public override int GetTransportCapacity() => capacity;
    }

    private sealed class FixedPowerSource(int power) : PowerSource(null!)
    {
        public override int Power() => power;
    }

    private sealed class FixedIncomeSource(int income) : IncomeSource(null!)
    {
        public bool IsThrowing { get; set; }

        public override int Income() => IsThrowing ? throw new InvalidOperationException("income source fails") : income;
    }
}
