using System.Reflection;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Platform;

namespace Aow.Game.Tests;

// PE-3-h: EngineObject.Assign, Copy and Compare and the original object images of the game set
// files go through the tree (persistence-tree.md 13). Each case compares the tree path with the
// stream path it replaced: TEngine.WriteEObject of the source, ReadEObject into the target, and
// the images that Compare compared.
public sealed partial class PersistEquivalenceTests
{
    // Every class on the tree, blank and with each integer member a schema name binds set to a
    // value of its own: a copy, an assignment into an object holding other values, an assignment
    // of a blank object (the load clears the target first and gives each absent member its
    // fallback), the comparisons and the object image end alike on both paths, with the same state
    // or the same error.
    [Theory]
    [MemberData(nameof(CopiedClasses))]
    public void AClassCopiesAssignsAndComparesAsTheStreamDid(string className)
    {
        var enlisted = EnlistCopiedClasses();
        try
        {
            WithDestroyingMap(_ => CopyAlike(className));
        }
        finally
        {
            EngineFixture.Engine.UnregisterEngineClasses(enlisted);
        }
    }

    // A unit of an army, with its resource in the game set.
    [Fact]
    public void AnArmyWithAUnitCopiesAsTheStreamDid()
    {
        WithUnitResource(resource => WithDestroyingMap(_ =>
        {
            var army = new Army(null!) { Holder = EngineFixture.Engine };
            var unit = new Unit(null!) { Holder = EngineFixture.Engine, UnitResource = resource, UnitId = 7, GroupId = 2, MoraleValue = -3, Player = 1 };
            unit.SetOwner(army);
            var other = new Army(null!) { Holder = EngineFixture.Engine };
            SameCopies(army, other);
            Discard(other);
            Discard(army);
        }));
    }

    // A hero with an item in its inventory, and Hero.CompatibleHero, which compares two heroes
    // with their group ids cleared.
    [Fact]
    public void AHeroWithAnItemCopiesAndComparesAsTheStreamDid()
    {
        WithDestroyingMap(_ =>
        {
            var hero = new Hero(null!) { Holder = EngineFixture.Engine, HeroName = "Source", Player = 2, LibraryId = 4, GroupId = 3 };
            PlainItem(itemId: 9, name: "Ring").SetOwner(hero.Inventory);
            var other = new Hero(null!) { Holder = EngineFixture.Engine, HeroName = "Other", Player = 5 };
            SameCopies(hero, other);
            var copy = (Hero)hero.Copy(EngineFixture.Engine);
            copy.GroupId = 8;
            Assert.True(hero.CompatibleHero(copy));
            Assert.Equal(StreamCompare(hero, copy), hero.Compare(copy, engine: EngineFixture.Engine));
            Assert.Equal(3, hero.GroupId);
            Assert.Equal(8, copy.GroupId);
            copy.HeroName = "Changed";
            Assert.False(hero.CompatibleHero(copy));
            Discard(copy);
            Discard(other);
            Discard(hero);
        });
    }

    // DiplomaticActionEventLog.SetDiplomaticAction copies the action, and the log copies with it.
    [Fact]
    public void ADiplomaticActionLogCopiesItsActionAsTheStreamDid()
    {
        var enlisted = EnlistCopiedClasses();
        try
        {
            WithDestroyingMap(_ =>
            {
                var proposal = DiplomaticProposal.FromFields(kind: 9, gold: 70, mana: 5) with { Message = ["Hail", "Neighbor"] };
                using var action = new DiplomaticAction(null!) { Holder = EngineFixture.Engine, Proposal = proposal, Source = 1, Target = 2, OtherPlayer = 3, Day = 9 };
                var log = new DiplomaticActionEventLog(null!) { Holder = EngineFixture.Engine };
                log.SetDiplomaticAction(action);
                Assert.NotSame(action, log.Action);
                Assert.Equal(proposal, log.Action.Proposal);
                EqualBytes(Image(StreamCopy(action)), actual: Image(log.Action), label: "DiplomaticAction copy");
                var other = new DiplomaticActionEventLog(null!) { Holder = EngineFixture.Engine };
                SameCopies(log, other);
                Discard(other);
                Discard(log);
            });
        }
        finally
        {
            EngineFixture.Engine.UnregisterEngineClasses(enlisted);
        }
    }

    // CombatLogbook.SetInitialData assigns the combat data, which holds a party and a combat unit
    // with a hero, and CombatViewer assigns the logbook's data back to the battle's data.
    [Fact]
    public void CombatDataAssignsAsTheStreamDid()
    {
        var enlisted = EnlistCopiedClasses();
        try
        {
            WithDestroyingMap(_ =>
            {
                var data = new CombatData(null!) { Holder = EngineFixture.Engine, X = 4, Y = 5, MapLevel = 1, Round = 2, Header = "Fight" };
                data.TurnOrder.Add(1);
                data.TurnOrder.Add(0);
                new CombatParty(null!) { Holder = EngineFixture.Engine, X = 4, Y = 5, PlayerIndex = 1 }.SetOwner(data.Parties);
                var hero = new Hero(null!) { Holder = EngineFixture.Engine, HeroName = "Fighter", Player = 1 };
                var unit = new CombatUnit(null!) { Holder = EngineFixture.Engine, Id = 3, Position = 1 };
                unit.SetUnit(hero);
                hero.Release();
                unit.SetOwner(data.CombatObjects);
                var logbook = new CombatLogbook(null!) { Holder = EngineFixture.Engine };
                logbook.SetInitialData(data);
                var expected = new CombatData(null!) { Holder = EngineFixture.Engine };
                StreamAssign(expected, data);
                EqualBytes(Image(expected), actual: Image(logbook.InitialData), label: "CombatLogbook initial data");
                var other = new CombatData(null!) { Holder = EngineFixture.Engine, X = 9, Header = "Other" };
                SameCopies(data, other);
                SameCopies(logbook, new CombatLogbook(null!) { Holder = EngineFixture.Engine });
                Discard(other);
                Discard(expected);
                Discard(logbook);
                Discard(data);
            });
        }
        finally
        {
            EngineFixture.Engine.UnregisterEngineClasses(enlisted);
        }
    }

    // A battle's combat objects are tactical battle units, which keep the class id of a combat unit.
    // The stream stored that id, and its load created the class the engine table holds there, a
    // combat unit in the game's table. The tree copy creates the same class.
    [Fact]
    public void AClassWithItsParentsIdComesBackAsTheRegisteredClass()
    {
        WithDestroyingMap(_ =>
        {
            var data = new CombatData(null!) { Holder = EngineFixture.Engine, Round = 1 };
            new TacticalBattleUnit(null!) { Holder = EngineFixture.Engine, Id = 5, Position = 2 }.SetOwner(data.CombatObjects);
            var viaTree = new CombatData(null!) { Holder = EngineFixture.Engine };
            var viaStream = new CombatData(null!) { Holder = EngineFixture.Engine };
            viaTree.Assign(data, engine: EngineFixture.Engine);
            StreamAssign(viaStream, data);
            Assert.Equal(1, viaTree.CombatObjects.GetCount());
            Assert.Equal(viaStream.CombatObjects.GetChild(0).GetType(), viaTree.CombatObjects.GetChild(0).GetType());
            Assert.IsType<CombatUnit>(viaTree.CombatObjects.GetChild(0));
            EqualBytes(Image(viaStream), actual: Image(viaTree), label: "CombatData with a tactical battle unit");
            Discard(viaStream);
            Discard(viaTree);
            Discard(data);
        });
    }

    // A player with a leader and an item, which PlayerList.Assign assigns player by player.
    [Fact]
    public void APlayerListAssignsItsPlayersAsTheStreamDid()
    {
        WithDestroyingMap(_ =>
        {
            var source = new PlayerList(null!) { Holder = EngineFixture.Engine };
            source.SetNumberOfPlayers(2);
            source.GetPlayers(1).Leader = new Leader(null!) { Holder = EngineFixture.Engine, HeroName = "Chosen", Player = 1 };
            source.GetPlayers(2).Day = 120;
            source.GetPlayers(2).Name = "Second";
            var viaTree = new PlayerList(null!) { Holder = EngineFixture.Engine };
            var viaStream = new PlayerList(null!) { Holder = EngineFixture.Engine };
            viaTree.Assign(source, engine: EngineFixture.Engine);
            viaStream.SetNumberOfPlayers(source.GetNumberOfPlayers());
            for (int index = 0; index < viaStream.Count; index++) StreamAssign(viaStream.GetPlayers(index), source.GetPlayers(index));
            for (int index = 0; index < viaStream.Count; index++)
                EqualBytes(Image(viaStream.GetPlayers(index)), actual: Image(viaTree.GetPlayers(index)), label: "Player " + index);
            SameCopies(source.GetPlayers(1), source.GetPlayers(2));
            Discard(viaStream);
            Discard(viaTree);
            Discard(source);
        });
    }

    // CombatLogbook.Actions is a reference list. It holds a child with no owner, a child it owns
    // and a child another list owns. The stream write made the list the owner of the first while
    // it wrote and left the second with no owner after. The tree write leaves every owner as it
    // was, and the copies hold the same two children.
    [Fact]
    public void AReferenceListCopiesAsTheStreamDidAndKeepsItsOwners()
    {
        var list = new CombatActionList(null!) { Holder = EngineFixture.Engine };
        var other = new CombatActionList(null!) { Holder = EngineFixture.Engine };
        EngineObject? copy = null;
        EngineObject? expected = null;
        try
        {
            var free = new DamageCombatAction(null!) { Holder = EngineFixture.Engine, Damage = 9, SourceId = 3, TargetId = 4 };
            list.AddChild(free);
            var owned = new DamageCombatAction(null!) { Holder = EngineFixture.Engine, Damage = 1 };
            list.AddChild(owned);
            owned.SetOwnerWithoutNotify(list);
            var elsewhere = new DamageCombatAction(null!) { Holder = EngineFixture.Engine, Damage = 2 };
            list.AddChild(elsewhere);
            elsewhere.SetOwnerWithoutNotify(other);
            copy = list.Copy(EngineFixture.Engine);
            Assert.Null(free.Owner);
            Assert.Same(list, owned.Owner);
            Assert.Same(other, elsewhere.Owner);
            expected = StreamCopy(list);
            Assert.Null(free.Owner);
            Assert.Null(owned.Owner);
            Assert.Same(other, elsewhere.Owner);
            Assert.Equal(2, ((CombatActionList)copy).Count);
            EqualBytes(Image(expected), actual: Image(copy), label: "CombatActionList copy");
        }
        finally
        {
            Discard(expected);
            Discard(copy);
            list.Dispose();
            other.Dispose();
        }
    }

    // The original image of a game set file: the image the stream wrote, and a load of it through
    // the tree ends where the stream load ended, for the classes the files hold.
    [Fact]
    public void GameSetImagesMatchTheStream()
    {
        WithUnitResource(_ =>
        {
            var classes = new AbilityControl(null!) { Holder = EngineFixture.Engine };
            SameObjectImage(classes, "AbilityControl", classTable: "game");
            var resources = new UnitResourceList(null!) { Holder = EngineFixture.Engine };
            new UnitResource(resources) { Race = 3, Attack = 5, ObtainValue = 50 };
            SameObjectImage(resources, "UnitResourceList", classTable: "game");
            var library = new HeroLibrary(null!) { Holder = EngineFixture.Engine };
            new Hero(null!) { Holder = EngineFixture.Engine, HeroName = "Library", Player = 1 }.SetOwner(library);
            SameObjectImage(library, "HeroLibrary", classTable: "game");
            var games = new EmailGameList(null!) { Holder = EngineFixture.Engine };
            new EmailGame(null!) { Holder = EngineFixture.Engine, UniqueLocalId = 2, UniqueId = 17 }.SetOwner(games);
            SameObjectImage(games, "EmailGameList", classTable: "game");
            Discard(games);
            Discard(library);
            Discard(resources);
            Discard(classes);
        });
    }

    // The file forms: WriteToFileCrc writes the stream's image between the two CRC words, and
    // ReadFromFileCrc loads it. A changed byte fails the check and loads nothing. The files go
    // through the layered file service, as on a host.
    [Fact]
    public void AGameSetFileRoundTripsBetweenItsCrcWords()
    {
        string root = Path.Combine(Path.GetTempPath(), "open-aow-pe3h-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(root, "root"));
        Directory.CreateDirectory(Path.Combine(root, "user"));
        var previous = Runtime.Files;
        Runtime.Files = new LayeredFileSystem(Path.Combine(root, "root"), userDirectory: Path.Combine(root, "user"));
        var resources = new HeroResourceList(null!) { Holder = EngineFixture.Engine };
        var loaded = new HeroResourceList(null!) { Holder = EngineFixture.Engine };
        var refused = new HeroResourceList(null!) { Holder = EngineFixture.Engine };
        try
        {
            string file = Path.Combine(root, "root", "HERORES.PFS");
            new HeroResource(resources) { Name = "Elf" };
            EngineFixture.Engine.WriteToFileCrc(file, objectValue: resources, progress: null);
            byte[] bytes;
            using (var stream = Runtime.Files.Open(file, mode: Runtime.fmOpenRead))
            using (var memory = new MemoryStream())
            {
                stream.CopyTo(memory);
                bytes = memory.ToArray();
            }
            EqualBytes(WrittenImage(resources), actual: bytes[4..^4], label: "HERORES.PFS image");
            Assert.True(EngineFixture.Engine.ReadFromFileCrc(file, objectValue: loaded, progress: null));
            EqualBytes(Image(resources), actual: Image(loaded), label: "HERORES.PFS load");
            bytes[10] ^= 1;
            using (var stream = Runtime.Files.Open(file, mode: Runtime.fmCreate))
                stream.Write(bytes);
            Assert.False(EngineFixture.Engine.ReadFromFileCrc(file, objectValue: refused, progress: null));
            Assert.Equal(0, refused.Count);
        }
        finally
        {
            Discard(refused);
            Discard(loaded);
            Discard(resources);
            Runtime.Files = previous;
            Directory.Delete(root, recursive: true);
        }
    }

    // The installed game set files, the system hero and item libraries included: a load through
    // the tree ends where the stream load ends, and the object image the tree writes is the image
    // the stream writes. The unit resources of Unitres.pfs are in the game set while the others
    // load, as the scenario set's load puts them there.
    [CorpusFact]
    public void TheInstalledGameSetFilesLoadAndWriteAsTheStreamDid()
    {
        (string Relative, string RootClass, string Table, bool Crc)[] files =
        [
            ("Release/Ability.pfs", "AbilityControl", "game", true),
            ("Release/Spells.pfs", "SpellControl", "game", true),
            ("Release/Unitgfx.pfs", "UnitGFXResourceList", "game", true),
            ("Release/Unitres.pfs", "UnitResourceList", "game", true),
            ("Release/HERORES.PFS", "HeroResourceList", "game", true),
            ("Release/ITEMGFX.PFS", "ItemGFXList", "game", true),
            ("Release/FACERES.PFS", "FaceResourceList", "game", true),
            ("Release/TEXT.pfs", "TextControl", "game", true),
            ("Release/comb_res.pfs", "MapObjectResourceList", "tactical", true),
            ("Release/HEROES.PFS", "HeroLibrary", "game", false),
            ("Release/ITEMS.PFS", "ItemLibrary", "game", false),
        ];
        var enlisted = EnlistCopiedClasses();
        var gameSet = GamePack.Current;
        var previous = gameSet.UnitResources;
        var unitResources = new UnitResourceList(null!) { Holder = EngineFixture.Engine };
        try
        {
            var unitBytes = File.ReadAllBytes(Path.Combine(InstalledSave.Root, "Release", "Unitres.pfs"));
            EngineFixture.Engine.ReadObjectImage(unitBytes[4..^4], objectValue: unitResources, progress: null, classTable: "game");
            gameSet.UnitResources = unitResources;
            WithDestroyingMap(_ =>
            {
                foreach (var (relative, rootClass, table, crc) in files)
                {
                    var bytes = File.ReadAllBytes(Path.Combine(InstalledSave.Root, relative));
                    var image = crc ? bytes[4..^4] : bytes;
                    int seed = Runtime.RandSeed;
                    var viaStream = Blank(rootClass);
                    var viaTree = Blank(rootClass);
                    try
                    {
                        Runtime.RandSeed = seed;
                        ReadWritten(viaStream, image);
                        int streamSeed = Runtime.RandSeed;
                        Runtime.RandSeed = seed;
                        EngineFixture.Engine.ReadObjectImage(image, objectValue: viaTree, progress: null, classTable: table);
                        Assert.True(streamSeed == Runtime.RandSeed, relative + " ends on seed " + Runtime.RandSeed + ", the stream load on " + streamSeed);
                        var written = WrittenImage(viaStream);
                        EqualBytes(written, actual: WrittenImage(viaTree), label: relative + " load");
                        EqualBytes(written, actual: EngineFixture.Engine.WriteObjectImage(viaTree, progress: null, classTable: table), label: relative + " write");
                        // The ability and spell controls load only the slots of registered
                        // abilities and spells, and a blank control has none.
                        if (rootClass is not ("AbilityControl" or "SpellControl"))
                            Assert.True(written.Length > 64, relative + " loads " + written.Length + " bytes");
                    }
                    finally
                    {
                        Runtime.RandSeed = seed;
                        Discard(viaTree);
                        Discard(viaStream);
                    }
                }
            });
        }
        finally
        {
            gameSet.UnitResources = previous;
            unitResources.Dispose();
            EngineFixture.Engine.UnregisterEngineClasses(enlisted);
        }
    }

    private static void CopyAlike(string className)
    {
        var source = Blank(className);
        var target = Blank(className);
        var blank = Blank(className);
        try
        {
            var classValue = s_schema.Get(className);
            StoredNameBindingTests.Fill(source, classValue: classValue, skip: s_sizes);
            StoredNameBindingTests.Fill(target, classValue: classValue, first: 130, skip: s_sizes);
            // An item sprite with no item removes itself when its item list changes.
            foreach (var sprite in new[] { source, target, blank }.OfType<ItemHexSprite>())
                PlainItem(itemId: 3, name: "Ring").SetOwner(sprite.Items);
            SameCopies(source, target);
            SameCopies(blank, target);
            string table = classValue.Engines.Count == 0 || classValue.Engines.Contains("game") ? "game" : classValue.Engines[0];
            // A game set file of a class that refuses its write (a sort node) is not a case: the
            // stream wrote an empty outer table, and the file write through the tree raises.
            if (Record.Exception(() => Image(source)) is null) SameObjectImage(source, className, classTable: table);
        }
        finally
        {
            Discard(blank);
            Discard(target);
            Discard(source);
        }
    }

    // Copy, Assign and Compare on the tree against the stream: the copy, the assignment of source
    // into a copy of other and the comparisons with the copy and with other. Each step starts from
    // the same random seed on both paths, as an image sequence draws on its load, and ends on the
    // same seed.
    private static void SameCopies(EngineObject source, EngineObject other)
    {
        string label = source.GetType().Name;
        int seed = Runtime.RandSeed;
        int streamSeed = 0;
        int treeSeed = 0;
        EngineObject? viaTree = null;
        EngineObject? viaStream = null;
        try
        {
            bool copied = AlikeOutcome(
                () => { Runtime.RandSeed = seed; viaStream = StreamCopy(source); streamSeed = Runtime.RandSeed; },
                viaPersist: () => { Runtime.RandSeed = seed; viaTree = source.Copy(EngineFixture.Engine); treeSeed = Runtime.RandSeed; },
                label: label + " copy");
            if (!copied) return;
            Assert.Equal(streamSeed, treeSeed);
            SameImages(viaStream!, viaTree!, label: label + " copy");
            Assert.Equal(StreamCompare(source, viaTree!), source.Compare(viaTree!, engine: EngineFixture.Engine));
            Assert.Equal(StreamCompare(source, other), source.Compare(other, engine: EngineFixture.Engine));
            Assert.Equal(StreamCompare(other, source), other.Compare(source, engine: EngineFixture.Engine));
            if (AssignOverridden(source.GetType())) return;
            Discard(viaTree);
            Discard(viaStream);
            viaTree = null;
            viaStream = null;
            var stream = viaStream = other.Copy(EngineFixture.Engine);
            var tree = viaTree = other.Copy(EngineFixture.Engine);
            bool assigned = AlikeOutcome(
                () => { Runtime.RandSeed = seed; StreamAssign(stream, source); streamSeed = Runtime.RandSeed; },
                viaPersist: () => { Runtime.RandSeed = seed; tree.Assign(source, engine: EngineFixture.Engine); treeSeed = Runtime.RandSeed; },
                label: label + " assign");
            if (!assigned) return;
            Assert.Equal(streamSeed, treeSeed);
            SameImages(stream, tree, label: label + " assign");
        }
        finally
        {
            Runtime.RandSeed = seed;
            Discard(viaTree);
            Discard(viaStream);
        }
    }

    // The images of two objects, or the same failure of both writes (a sort node refuses to be
    // written).
    private static void SameImages(EngineObject expected, EngineObject actual, string label)
    {
        byte[]? left = null;
        byte[]? right = null;
        if (AlikeOutcome(() => left = Image(expected), viaPersist: () => right = Image(actual), label: label))
            EqualBytes(left!, actual: right!, label: label);
    }

    // The object image of the game set files: WriteObjectImage writes the stream's image, and
    // ReadObjectImage loads it where the stream load ends.
    private static void SameObjectImage(EngineObject source, string className, string classTable)
    {
        byte[]? expected = null;
        byte[]? actual = null;
        if (!AlikeOutcome(() => expected = WrittenImage(source), viaPersist: () => actual = EngineFixture.Engine.WriteObjectImage(source, progress: null, classTable: classTable), label: className + " object image"))
            return;
        EqualBytes(expected!, actual: actual!, label: className + " object image");
        int seed = Runtime.RandSeed;
        var viaStream = Blank(className);
        var viaTree = Blank(className);
        try
        {
            Runtime.RandSeed = seed;
            ReadWritten(viaStream, expected!);
            int streamSeed = Runtime.RandSeed;
            Runtime.RandSeed = seed;
            EngineFixture.Engine.ReadObjectImage(expected!, objectValue: viaTree, progress: null, classTable: classTable);
            Assert.Equal(streamSeed, Runtime.RandSeed);
            EqualBytes(Image(viaStream), actual: Image(viaTree), label: className + " object image load");
        }
        finally
        {
            Runtime.RandSeed = seed;
            Discard(viaTree);
            Discard(viaStream);
        }
    }

    // Whether a class assigns through an override of its own, which the original also had
    // (PlayerList assigns player by player). The base brackets of ECustomNode and
    // EChangeNotifyNode are part of the stream assignment below.
    private static bool AssignOverridden(Type type)
    {
        var declaring = type.GetMethod(nameof(EngineObject.Assign), [typeof(EngineObject), typeof(GameEngine)])!.DeclaringType;
        return declaring != typeof(EngineObject) && declaring != typeof(ECustomNode) && declaring != typeof(EChangeNotifyNode);
    }

    // The stream copy PE-3-h replaced (Engine.pas 2543): WriteEObject, a new object of the class,
    // ReadEObject into it.
    private static EngineObject StreamCopy(EngineObject source)
    {
        var image = WrittenImage(source);
        var result = source.EngineClass.CreateInstance(null!, holder: EngineFixture.Engine);
        ReadWritten(result, image);
        return result;
    }

    // The stream assignment PE-3-h replaced (Engine.pas 2529), with the brackets of the node
    // overrides: EChangeNotifyNode calls BeginUpdate and EndUpdate around ECustomNode's, which
    // clears the node first.
    private static void StreamAssign(EngineObject target, EngineObject source)
    {
        (target as EChangeNotifyNode)?.BeginUpdate();
        (target as ECustomNode)?.Clear();
        ReadWritten(target, WrittenImage(source));
        (target as EChangeNotifyNode)?.EndUpdate();
    }

    // The stream comparison PE-3-h replaced (Engine.pas 2511): the same class and equal images.
    private static bool StreamCompare(EngineObject left, EngineObject right) =>
        left.EngineClass.ClassId == right.EngineClass.ClassId && WrittenImage(left).AsSpan().SequenceEqual(WrittenImage(right));

    // TEngine.WriteEObject: the object at id 0 of the outer table.
    private static byte[] WrittenImage(EngineObject objectValue)
    {
        using var memory = new MemoryStream();
        using (var writer = new StorageWriter(memory, engine: EngineFixture.Engine, partial: false))
        {
            writer.Object(objectValue, 0);
        }
        return memory.ToArray();
    }

    // TEngine.ReadEObject.
    private static void ReadWritten(EngineObject objectValue, byte[] image)
    {
        using var memory = new MemoryStream(image);
        using var reader = new StorageReader(memory, engine: EngineFixture.Engine, partial: false);
        reader.Object(objectValue, 0);
    }

    // The classes whose write touches files beside the scenario set, that need a map the fixture
    // does not give a blank instance, or that are the engine or an abstract node.
    private static readonly HashSet<string> s_copyExceptions =
        ["GameMap", "StrategicMap", "CombatMap", "GamePack", "MapContainer", "MapLevel", "CombatMapLevel", "MapField", "DynamicIsometricHex", "FlagControl", "HeroLibraryManager", "ItemLibraryManager", "GameEngine", "ECustomNode"];

    // The members that size an array the object keeps beside them. A value of their own would
    // write past that array on both paths.
    private static readonly HashSet<string> s_sizes = ["AbilityCount", "Width", "Height"];

    public static TheoryData<string> CopiedClasses()
    {
        var names = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var engineClass in EngineClass.All)
        {
            var type = engineClass.Type;
            if (type.Assembly != typeof(GamePack).Assembly || type.IsAbstract) continue;
            if (EngineObject.TreePersistenceRefusal(type) is not null) continue;
            if (FormatSchema.Current.Find(engineClass.StorageKey) is null || s_copyExceptions.Contains(engineClass.StorageKey)) continue;
            names.Add(engineClass.StorageKey);
        }
        var data = new TheoryData<string>();
        foreach (var name in names) data.Add(name);
        return data;
    }

    // The game enlists the classes of its screens, the AI and combat when they start, and the
    // fixture engine holds only the classes of the engine's start. Each class id is enlisted once.
    private static EngineClass[] EnlistCopiedClasses()
    {
        var table = EngineFixture.Engine.ClassTable;
        var added = new Dictionary<int, EngineClass>();
        foreach (var engineClass in EngineClass.All)
        {
            if (engineClass.Type.Assembly != typeof(GamePack).Assembly) continue;
            if (table.ContainsKey(engineClass.ClassId) || added.ContainsKey(engineClass.ClassId)) continue;
            added[engineClass.ClassId] = engineClass;
        }
        var classes = added.Values.ToArray();
        EngineFixture.Engine.RegisterEngineClasses(classes);
        return classes;
    }
}
