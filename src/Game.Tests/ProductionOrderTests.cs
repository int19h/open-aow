using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Commands;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

// Brief 9.6-6: production orders against the flat fields and the switches they replaced. The bytes
// were written on main eecc704 by TProductionItem.ReadWrite with FProductionID, FParameter and
// FGroupID set to the triple, alone and inside a TProductionTE. The triples are every property
// set the 9.6 census found per kind, the kinds 10 and 14 as City.pas and PrdPlace.pas build them,
// and two ids no kind registers. The Old* methods copy the switches from main eecc704.
[Collection("Engine")]
[EngineState]
public sealed class ProductionOrderTests
{
    public static TheoryData<int, int, int, string, string> OldBytes { get; } = new()
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
    private static ProductionOrder Expected(int id, int parameter, int groupId) => id switch
    {
        1 => new ProduceUnit(parameter, groupId),
        2 => new ResearchSpell(parameter),
        10 => new Trade(parameter),
        11 => new Upgrade(),
        12 => new Fortify(),
        13 => new Migrate(parameter),
        14 => new Loot(),
        _ => new UnknownOrder(id, parameter, groupId),
    };

    private static string Hex(MemoryStream memory) => Convert.ToHexString(memory.ToArray());

    private static MemoryStream Memory(string hex)
    {
        var bytes = Convert.FromHexString(hex);
        var memory = new MemoryStream();
        memory.WriteBuffer(bytes);
        memory.Position = 0;
        return memory;
    }

    private static GameEngine CreateEngine()
    {
        if (EngineHost.Engines is null) EngineHost.Initialize();
        GameEngine engine = new GameEngine(null!);
        engine.Options -= Set<EngineModeElement>.Of(GameEngine.VerifyClasses);
        engine.RegisterEngineClasses([EngineClass.Of<ProductionItem>(), EngineClass.Of<ProductionTokenEvent>()]);
        return engine;
    }

    private static string Write(GameEngine engine, EngineObject value)
    {
        using var memory = new MemoryStream();
        var writer = new EWriteStorageStream(memory, engine, false);
        writer.rwCreateEObject(ref value, null!, 7);
        writer.Dispose();
        return Hex(memory);
    }

    private static T Read<T>(GameEngine engine, string hex) where T : EngineObject
    {
        using var memory = Memory(hex);
        EngineObject result = null!;
        using (var reader = new EReadStorageStream(memory, engine, false)) { reader.rwCreateEObject(ref result, null!, 7); }
        return Assert.IsType<T>(result);
    }

    [Theory]
    [MemberData(nameof(OldBytes))]
    public void OrdersWriteTheOldBytes(int id, int parameter, int groupId, string itemBytes, string eventBytes)
    {
        GameEngine engine = CreateEngine();
        try
        {
            var order = ProductionOrder.FromFields(id, parameter, groupId);
            Assert.Equal(Expected(id, parameter, groupId), order);
            Assert.Equal((id, parameter, groupId), order.ToFields());

            var item = new ProductionItem(null!) { Order = Expected(id, parameter, groupId) };
            Assert.Equal(itemBytes, Write(engine, item));
            item.Dispose();

            var tokenEvent = new ProductionTokenEvent(null!) { X = 3, Y = 4, MapLevel = 1, Player = 2, Queue = true, Index = 1 };
            tokenEvent.Item = new ProductionItem(null!) { Order = Expected(id, parameter, groupId) };
            Assert.Equal(eventBytes, Write(engine, tokenEvent));
            tokenEvent.Dispose();

            var loaded = Read<ProductionItem>(engine, itemBytes);
            Assert.Equal(Expected(id, parameter, groupId), loaded.Order);
            Assert.Equal(itemBytes, Write(engine, loaded));
            loaded.Dispose();

            var loadedEvent = Read<ProductionTokenEvent>(engine, eventBytes);
            Assert.Equal(Expected(id, parameter, groupId), loadedEvent.Item.Order);
            Assert.Equal(eventBytes, Write(engine, loadedEvent));
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
        Assert.Equal(new UnknownOrder(0, 0, 0), new ProductionItem(null!).Order);
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
        using var queue = new ProductionQueue(null!);
        foreach (var order in new ProductionOrder[] { new ProduceUnit(5, 12), new ResearchSpell(5), new Migrate(6), new Upgrade() })
            _ = new ProductionItem(queue) { Order = order };
        Assert.Equal(0, queue.IndexOf(new ProduceUnit(5, 12)));
        Assert.Equal(-1, queue.IndexOf(new ProduceUnit(5, 0)));
        Assert.Equal(1, queue.IndexOf(new ResearchSpell(5)));
        Assert.Equal(-1, queue.IndexOf(new ResearchSpell(6)));
        Assert.Equal(2, queue.IndexOf(new Migrate(6)));
        Assert.Equal(3, queue.IndexOf(new Upgrade()));
        Assert.Equal(-1, queue.IndexOf(new Fortify()));
        Assert.Equal(-1, queue.IndexOf(new UnknownOrder(11, 0, 0)));
        Assert.Equal(1, queue.IndexOfId(2));
    }

    // City.pas:3220 ProductionIDToCityAction.
    private static CityAction OldCityAction(int id) => id switch
    {
        1 => CityAction.Produce,
        2 => CityAction.Research,
        10 => CityAction.Trade,
        11 => CityAction.Upgrade,
        12 => CityAction.Fortify,
        13 => CityAction.Migrate,
        14 => CityAction.Loot,
        _ => CityAction.None,
    };

    // Structur.PAS:837 and City.pas:604 LoopableProduction.
    private static bool OldLoopable(int id, bool city)
    {
        bool result = id != 2;
        if (city && OldCityAction(id) is CityAction.Migrate or CityAction.Loot or CityAction.Fortify or CityAction.Upgrade) result = false;
        return result;
    }

    // Structur.PAS:834 and City.pas:595 LockQueueProduction.
    private static bool OldLocksQueue(int id, bool city) => city && OldCityAction(id) is CityAction.Migrate or CityAction.Loot;

    // AoWCityScreen.pas:904 ActivityImage.
    private static int OldCityImage(CityAction action, byte wallType) => action switch
    {
        CityAction.Trade => 40,
        CityAction.Upgrade => 43,
        CityAction.Fortify => wallType == 0 ? 41 : 42,
        CityAction.Migrate => 44,
        CityAction.Loot => 45,
        CityAction.Raze => 46,
        CityAction.Produce => 4,
        CityAction.Research => 5,
        _ => 40,
    };

    // AoWProdScreen.pas:719 ActivityImage.
    private static int OldPlaceImage(int id, MagicSphere sphere) => id switch
    {
        1 => 4,
        10 => sphere != MagicSphere.None ? 48 : 47,
        _ => 4,
    };

    // City.pas:2503, 2539, 2575: the sequence of row Row, null where the race image shows.
    private static int? OldSequence(CityAction action, byte wallType, int row)
    {
        if (action is CityAction.None or CityAction.Trade or CityAction.Produce) return null;
        return action switch
        {
            CityAction.Fortify => wallType switch { 0 => 30 + row, 1 => 48 + row, _ => -1 },
            CityAction.Upgrade => 33 + row,
            CityAction.Migrate => 39 + row,
            CityAction.Raze => 51 + row,
            CityAction.Loot => 45 + row,
            _ => -1,
        };
    }

    // AoWRealm.pas, the activity column.
    private static string OldRealm(CityAction action, byte wallType) => ((int)action & 127) switch
    {
        3 => wallType == 1 ? "1" : "0",
        2 => "2",
        5 => "3",
        4 => "4",
        7 or 8 => "5",
        _ => "-",
    };

    // Structur.PAS:840 with City.pas:615 and PrdPlace.pas:252, 453, for the kinds whose name
    // reads no unit production.
    private static string OldName(int id, int parameter, bool city, MagicSphere sphere)
    {
        string result = AbilityRules.TranslateResourceString(GameTables.UnknownText);
        if (city)
        {
            switch (OldCityAction(id))
            {
                case CityAction.Trade: result = AbilityRules.TranslateResourceString(GameText.ProduceMerchandiseText); break;
                case CityAction.Upgrade: result = AbilityRules.TranslateResourceString(GameText.UpgradeText); break;
                case CityAction.Migrate: result = Runtime.Format(AbilityRules.TranslateResourceString(Migrate.MigrateFormat), [AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[parameter])]); break;
                case CityAction.Loot: result = AbilityRules.TranslateResourceString(GameText.LootText); break;
                case CityAction.Fortify: result = AbilityRules.TranslateResourceString(GameText.FortifyText); break;
            }
        }
        else if (id == 10)
        {
            result = AbilityRules.TranslateResourceString(sphere != MagicSphere.None ? Trade.GenerateExtraPowerText : GameText.ProduceMerchandiseText);
        }
        return result;
    }

    private static readonly (int Id, int Parameter, int GroupId)[] s_kinds =
    [
        (1, 5, 12), (2, 5, 0), (10, 1, 0), (10, -1, 0), (11, 0, 0), (12, 0, 0), (13, 6, 0), (14, 0, 0), (0, 0, 0), (99, 7, 3),
    ];

    [Fact]
    public void OrdersAnswerAsTheOldSwitches()
    {
        var previousMap = AoWHSMap.Current;
        var map = new AoWHSMap(null!);
        AoWHSMap.Current = map;
        var owner = new EngineFixture.Owner();
        var city = (City)EngineClass.Of<City>().CreateInstance(owner);
        var guild = (BuildersGuild)EngineClass.Of<BuildersGuild>().CreateInstance(owner);
        var node = (LifeNode)EngineClass.Of<LifeNode>().CreateInstance(owner);
        try
        {
            ProductionControl[] controls = [city.Production, guild.ProductionControl, node.ProductionControl];
            int checks = 0;
            foreach (var (id, parameter, groupId) in s_kinds)
            {
                var order = ProductionOrder.FromFields(id, parameter, groupId);
                var action = OldCityAction(id);
                Assert.Equal(action, order.CityAction);
                foreach (var control in controls)
                {
                    bool inCity = control is CityProductionControl;
                    var sphere = control is ProductionPlaceControl place ? place.ProductionPlace.GetMagicSphere() : MagicSphere.None;
                    string where = order + " in " + control.GetType().Name;
                    Assert.True(OldLoopable(id, inCity) == order.Loopable(control), where);
                    Assert.True(OldLocksQueue(id, inCity) == order.LocksQueue(control), where);
                    if (id is not (1 or 2)) Assert.True(OldName(id, parameter, inCity, sphere) == order.Name(control), where);
                    checks += 3;
                }
                Assert.Equal(OldPlaceImage(id, MagicSphere.None), order.ActivityImage(guild));
                Assert.Equal(OldPlaceImage(id, MagicSphere.Life), order.ActivityImage(node));
                foreach (byte wall in new byte[] { 0, 1, 2 })
                {
                    city.WallType = wall;
                    Assert.Equal(OldCityImage(action, wall), order.ActivityImage(city));
                    Assert.Equal(OldRealm(action, wall), order.RealmActivity(city));
                    for (int row = 0; row < 3; row++) Assert.Equal(OldSequence(action, wall, row), order.CitySequence(city, row));
                    checks += 5;
                }
            }
            Assert.Equal(s_kinds.Length * 24, checks);
        }
        finally
        {
            foreach (Structure structure in new Structure[] { city, guild, node }) _ = Record.Exception(structure.Dispose);
            AoWHSMap.Current = previousMap;
            map.Dispose();
        }
    }
}
