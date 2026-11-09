using Aow.Units.AoWE;
using Aow.Units.BldGuild;
using Aow.Units.City;
using Aow.Units.Engine;
using Aow.Units.LifeNode;
using Aow.Units.PrdPlace;
using Game = Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.Tests;

// Brief 9.6-6: production orders against the flat fields and the switches they replaced. The bytes
// were written on main eecc704 by TProductionItem.ReadWrite with FProductionID, FParameter and
// FGroupID set to the triple, alone and inside a TProductionTE. The triples are every property
// set the 9.6 census found per kind, the kinds 10 and 14 as City.pas and PrdPlace.pas build them,
// and two ids no kind registers. The Old* methods copy the switches from main eecc704.
[Collection("Engine")]
[EngineState]
public sealed class ProductionOrderTests
{
    public static readonly TheoryData<int, int, int, string, string> OldBytes = new()
    {
        { 1, 0, 0, "0107009702020001140001000000", "01070098020200080A00150416051706180719081A131C17FFFFFFFF03040102970202000114000100000001000000" },
        { 1, 5, 0, "0107009702020002140015040100000005000000", "01070098020200080A00150416051706180719081A191C1DFFFFFFFF03040102970202000214001504010000000500000001000000" },
        { 1, 0, 12, "010700970202000214001604010000000C000000", "01070098020200080A00150416051706180719081A191C1DFFFFFFFF03040102970202000214001604010000000C00000001000000" },
        { 1, 5, 12, "010700970202000314001504160801000000050000000C000000", "01070098020200080A00150416051706180719081A1F1C23FFFFFFFF03040102970202000314001504160801000000050000000C00000001000000" },
        { 2, 5, 0, "0107009702020002140015040200000005000000", "01070098020200080A00150416051706180719081A191C1DFFFFFFFF03040102970202000214001504020000000500000001000000" },
        { 10, 1, 0, "0107009702020002140015040A00000001000000", "01070098020200080A00150416051706180719081A191C1DFFFFFFFF030401029702020002140015040A0000000100000001000000" },
        { 10, -1, 0, "0107009702020002140015040A000000FFFFFFFF", "01070098020200080A00150416051706180719081A191C1DFFFFFFFF030401029702020002140015040A000000FFFFFFFF01000000" },
        { 11, 0, 0, "010700970202000114000B000000", "01070098020200080A00150416051706180719081A131C17FFFFFFFF03040102970202000114000B00000001000000" },
        { 12, 0, 0, "010700970202000114000C000000", "01070098020200080A00150416051706180719081A131C17FFFFFFFF03040102970202000114000C00000001000000" },
        { 13, 6, 0, "0107009702020002140015040D00000006000000", "01070098020200080A00150416051706180719081A191C1DFFFFFFFF030401029702020002140015040D0000000600000001000000" },
        { 13, 0, 0, "010700970202000114000D000000", "01070098020200080A00150416051706180719081A131C17FFFFFFFF03040102970202000114000D00000001000000" },
        { 14, 0, 0, "010700970202000114000E000000", "01070098020200080A00150416051706180719081A131C17FFFFFFFF03040102970202000114000E00000001000000" },
        { 0, 0, 0, "0107009702020000", "01070098020200080A00150416051706180719081A0D1C11FFFFFFFF03040102970202000001000000" },
        { 99, 7, 3, "0107009702020003140015041608630000000700000003000000", "01070098020200080A00150416051706180719081A1F1C23FFFFFFFF03040102970202000314001504160863000000070000000300000001000000" },
    };

    // The kind each triple reads as, built by hand.
    private static ProductionOrder Expected(int Id, int Parameter, int GroupId) => Id switch
    {
        1 => new ProduceUnit(Parameter, GroupId),
        2 => new ResearchSpell(Parameter),
        10 => new Trade(Parameter),
        11 => new Upgrade(),
        12 => new Fortify(),
        13 => new Migrate(Parameter),
        14 => new Loot(),
        _ => new UnknownOrder(Id, Parameter, GroupId),
    };

    private static string Hex(TMemoryStream memory) => Convert.ToHexString(memory.Memory.AsSpan(0, memory.Size));

    private static TMemoryStream Memory(string hex)
    {
        var bytes = Convert.FromHexString(hex);
        var memory = new TMemoryStream();
        memory.WriteBuffer(bytes);
        memory.Position = 0;
        return memory;
    }

    private static TEngine CreateEngine()
    {
        if (EngineUnit.Engines is null) EngineUnit.Initialize();
        var engine = new TEngine(null!);
        engine.Options -= TSet<TEngineModeElement>.Of(EngineUnit.eoVerifyClasses);
        engine.RegisterEClasses([EClass.Of<TProductionItem>(), EClass.Of<TProductionTE>()]);
        return engine;
    }

    private static string Write(TEngine engine, TEObject value)
    {
        using var memory = new TMemoryStream();
        var writer = new TEWriteStorageStream(memory, engine, false);
        writer.rwCreateEObject(ref value, null!, 7);
        writer.Dispose();
        return Hex(memory);
    }

    private static T Read<T>(TEngine engine, string hex) where T : TEObject
    {
        using var memory = Memory(hex);
        TEObject result = null!;
        using (var reader = new TEReadStorageStream(memory, engine, false)) { reader.rwCreateEObject(ref result, null!, 7); }
        return Assert.IsType<T>(result);
    }

    [Theory]
    [MemberData(nameof(OldBytes))]
    public void OrdersWriteTheOldBytes(int Id, int Parameter, int GroupId, string ItemBytes, string EventBytes)
    {
        var engine = CreateEngine();
        try
        {
            var order = ProductionOrder.FromFields(Id, Parameter, GroupId);
            Assert.Equal(Expected(Id, Parameter, GroupId), order);
            Assert.Equal((Id, Parameter, GroupId), order.ToFields());

            var item = new TProductionItem(null!) { Order = Expected(Id, Parameter, GroupId) };
            Assert.Equal(ItemBytes, Write(engine, item));
            item.Dispose();

            var te = new TProductionTE(null!) { FX = 3, FY = 4, FL = 1, FPlayer = 2, FQueue = true, FIndex = 1 };
            te.FItem = new TProductionItem(null!) { Order = Expected(Id, Parameter, GroupId) };
            Assert.Equal(EventBytes, Write(engine, te));
            te.Dispose();

            var loaded = Read<TProductionItem>(engine, ItemBytes);
            Assert.Equal(Expected(Id, Parameter, GroupId), loaded.Order);
            Assert.Equal(ItemBytes, Write(engine, loaded));
            loaded.Dispose();

            var loadedEvent = Read<TProductionTE>(engine, EventBytes);
            Assert.Equal(Expected(Id, Parameter, GroupId), loadedEvent.FItem.Order);
            Assert.Equal(EventBytes, Write(engine, loadedEvent));
            loadedEvent.Dispose();
        }
        finally
        {
            engine.Dispose();
        }
    }

    [Fact]
    public void UnknownIdsWriteBackUnchanged()
    {
        foreach (var (id, parameter, groupId) in new[] { (99, 7, 3), (0, 0, 0), (3, -5, 8), (15, 1, 1) })
        {
            var order = ProductionOrder.FromFields(id, parameter, groupId);
            Assert.Equal(new UnknownOrder(id, parameter, groupId), order);
            Assert.Equal((id, parameter, groupId), order.ToFields());
        }
        Assert.Equal(new UnknownOrder(0, 0, 0), new TProductionItem(null!).Order);
    }

    [Fact]
    public void OriginalKindsRegisterOnce()
    {
        foreach (int id in new[] { 1, 2, 10, 11, 12, 13, 14 })
        {
            Assert.True(ProductionOrder.IsRegistered(id), "production id " + id);
            Assert.Throws<InvalidOperationException>(() => ProductionOrder.Register(id, static (id, parameter, groupId) => new UnknownOrder(id, parameter, groupId)));
        }
        foreach (int id in new[] { 0, 3, 9, 15 }) Assert.False(ProductionOrder.IsRegistered(id), "production id " + id);
    }

    [Fact]
    public void QueueFindsAnEqualOrder()
    {
        using var queue = new TProductionQueue(null!);
        foreach (var order in new ProductionOrder[] { new ProduceUnit(5, 12), new ResearchSpell(5), new Migrate(6), new Upgrade() })
            _ = new TProductionItem(queue) { Order = order };
        Assert.Equal(0, queue.IndexOf(new ProduceUnit(5, 12)));
        Assert.Equal(-1, queue.IndexOf(new ProduceUnit(5, 0)));
        Assert.Equal(1, queue.IndexOf(new ResearchSpell(5)));
        Assert.Equal(-1, queue.IndexOf(new ResearchSpell(6)));
        Assert.Equal(2, queue.IndexOf(new Migrate(6)));
        Assert.Equal(3, queue.IndexOf(new Upgrade()));
        Assert.Equal(-1, queue.IndexOf(new Fortify()));
        Assert.Equal(-1, queue.IndexOf(new UnknownOrder(11, 0, 0)));
        Assert.Equal(1, queue.IndexOfID(2));
    }

    // City.pas:3220 ProductionIDToCityAction.
    private static TCityAction OldCityAction(int ID) => ID switch
    {
        1 => TCityAction.caProduce,
        2 => TCityAction.caResearch,
        10 => TCityAction.caTrade,
        11 => TCityAction.caUpgrade,
        12 => TCityAction.caFortify,
        13 => TCityAction.caMigrate,
        14 => TCityAction.caLoot,
        _ => TCityAction.caNone,
    };

    // Structur.PAS:837 and City.pas:604 LoopableProduction.
    private static bool OldLoopable(int ID, bool City)
    {
        bool Result = ID != 2;
        if (City && OldCityAction(ID) is TCityAction.caMigrate or TCityAction.caLoot or TCityAction.caFortify or TCityAction.caUpgrade) Result = false;
        return Result;
    }

    // Structur.PAS:834 and City.pas:595 LockQueueProduction.
    private static bool OldLocksQueue(int ID, bool City) => City && OldCityAction(ID) is TCityAction.caMigrate or TCityAction.caLoot;

    // AoWCityScreen.pas:904 ActivityImage.
    private static int OldCityImage(TCityAction Action, byte WallType) => Action switch
    {
        TCityAction.caTrade => 40,
        TCityAction.caUpgrade => 43,
        TCityAction.caFortify => WallType == 0 ? 41 : 42,
        TCityAction.caMigrate => 44,
        TCityAction.caLoot => 45,
        TCityAction.caRaze => 46,
        TCityAction.caProduce => 4,
        TCityAction.caResearch => 5,
        _ => 40,
    };

    // AoWProdScreen.pas:719 ActivityImage.
    private static int OldPlaceImage(int ID, TMagicSphere Sphere) => ID switch
    {
        1 => 4,
        10 => Sphere != TMagicSphere.msNone ? 48 : 47,
        _ => 4,
    };

    // City.pas:2503, 2539, 2575: the sequence of row Row, null where the race image shows.
    private static int? OldSequence(TCityAction Action, byte WallType, int Row)
    {
        if (Action is TCityAction.caNone or TCityAction.caTrade or TCityAction.caProduce) return null;
        return Action switch
        {
            TCityAction.caFortify => WallType switch { 0 => 30 + Row, 1 => 48 + Row, _ => -1 },
            TCityAction.caUpgrade => 33 + Row,
            TCityAction.caMigrate => 39 + Row,
            TCityAction.caRaze => 51 + Row,
            TCityAction.caLoot => 45 + Row,
            _ => -1,
        };
    }

    // AoWRealm.pas, the activity column.
    private static string OldRealm(TCityAction Action, byte WallType) => ((int)Action & 127) switch
    {
        3 => WallType == 1 ? "1" : "0",
        2 => "2",
        5 => "3",
        4 => "4",
        7 or 8 => "5",
        _ => "-",
    };

    // Structur.PAS:840 with City.pas:615 and PrdPlace.pas:252, 453, for the kinds whose name
    // reads no unit production.
    private static string OldName(int ID, int Parameter, bool City, TMagicSphere Sphere)
    {
        string Result = Game.TranslateRStr(Game.UnknownRStr);
        if (City)
        {
            switch (OldCityAction(ID))
            {
                case TCityAction.caTrade: Result = Game.TranslateRStr(Game.ProduceMerchandiseRStr); break;
                case TCityAction.caUpgrade: Result = Game.TranslateRStr(Game.UpgradeRStr); break;
                case TCityAction.caMigrate: Result = Runtime.Format(Game.TranslateRStr(Game.MigrateToXRStr), [Game.TranslateRStr(Game.RaceNamesPluralRStr[Parameter])]); break;
                case TCityAction.caLoot: Result = Game.TranslateRStr(Game.LootRStr); break;
                case TCityAction.caFortify: Result = Game.TranslateRStr(Game.FortifyRStr); break;
            }
        }
        else if (ID == 10)
        {
            Result = Game.TranslateRStr(Sphere != TMagicSphere.msNone ? Game.GenerateExtraPowerRStr : Game.ProduceMerchandiseRStr);
        }
        return Result;
    }

    private static readonly (int Id, int Parameter, int GroupId)[] Kinds =
    [
        (1, 5, 12), (2, 5, 0), (10, 1, 0), (10, -1, 0), (11, 0, 0), (12, 0, 0), (13, 6, 0), (14, 0, 0), (0, 0, 0), (99, 7, 3),
    ];

    [Fact]
    public void OrdersAnswerAsTheOldSwitches()
    {
        var previousMap = Game.AoWHSMap;
        var map = new TAoWHSMap(null!);
        Game.AoWHSMap = map;
        var owner = new EngineFixture.Owner();
        var city = (TCity)EClass.Of<TCity>().Create(owner);
        var guild = (TBuildersGuild)EClass.Of<TBuildersGuild>().Create(owner);
        var node = (TLifeNode)EClass.Of<TLifeNode>().Create(owner);
        try
        {
            TProductionControl[] controls = [city.FProduction, guild.FProductionControl, node.FProductionControl];
            int checks = 0;
            foreach (var (id, parameter, groupId) in Kinds)
            {
                var order = ProductionOrder.FromFields(id, parameter, groupId);
                var action = OldCityAction(id);
                Assert.Equal(action, order.CityAction);
                foreach (var control in controls)
                {
                    bool inCity = control is TCityProductionControl;
                    var sphere = control is TProductionPlaceControl place ? place.FProductionPlace.GetMagicSphere() : TMagicSphere.msNone;
                    string where = order + " in " + control.GetType().Name;
                    Assert.True(OldLoopable(id, inCity) == order.Loopable(control), where);
                    Assert.True(OldLocksQueue(id, inCity) == order.LocksQueue(control), where);
                    if (id is not (1 or 2)) Assert.True(OldName(id, parameter, inCity, sphere) == order.Name(control), where);
                    checks += 3;
                }
                Assert.Equal(OldPlaceImage(id, TMagicSphere.msNone), order.ActivityImage(guild));
                Assert.Equal(OldPlaceImage(id, TMagicSphere.msLife), order.ActivityImage(node));
                foreach (byte wall in new byte[] { 0, 1, 2 })
                {
                    city.FWallType = wall;
                    Assert.Equal(OldCityImage(action, wall), order.ActivityImage(city));
                    Assert.Equal(OldRealm(action, wall), order.RealmActivity(city));
                    for (int row = 0; row < 3; row++) Assert.Equal(OldSequence(action, wall, row), order.CitySequence(city, row));
                    checks += 5;
                }
            }
            Assert.Equal(Kinds.Length * 24, checks);
        }
        finally
        {
            foreach (TStructure structure in new TStructure[] { city, guild, node }) _ = Record.Exception(structure.Dispose);
            Game.AoWHSMap = previousMap;
            map.Dispose();
        }
    }
}
